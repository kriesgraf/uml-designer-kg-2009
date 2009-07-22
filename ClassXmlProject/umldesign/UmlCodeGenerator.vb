Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Collections
Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.IO
Imports Microsoft.VisualBasic

#If _APP_UML = "1" Then

Imports ClassXmlProject.MenuItemCommand
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.UmlNodesManager

#Else

Public Interface InterfProgression
    WriteOnly Property Minimum() As Integer
    WriteOnly Property Maximum() As Integer
    WriteOnly Property ProgressBarVisible() As Boolean
    Sub Increment(ByVal value As Integer)
End Interface

#End If

Public Class UmlCodeGenerator

#Region "Class declarations"

    Private Const cstDocumentElement As String = "document"
    Private Const cstFolderElement As String = "package"
    Private Const cstFileElement As String = "code"
    Private Const cstUpdate As String = "_codegen"
    Private Const cstCodeSourceHeaderCppStyleSheet As String = "uml2cpp-h.xsl"
    Private Const cstCodeSourceVbDotNetStyleSheet As String = "uml2vbnet.xsl"
    Private Const cstCodeSourceJavaStyleSheet As String = "uml2java.xsl"
    Private Const cstTempExport As String = ".umlexp"

    Private Shared m_xsltCppSourceHeaderStyleSheet As XslSimpleTransform = Nothing
    Private Shared m_xsltVbClassModuleStyleSheet As XslSimpleTransform = Nothing
    Private Shared m_xsltJavaModuleStyleSheet As XslSimpleTransform = Nothing

    Private Shared m_bTransformActive As Boolean = False
    Private Shared m_strSeparator As String = Path.DirectorySeparatorChar.ToString
#End Region

#Region "Public shared methods"

#If _APP_UML = "1" Then

    Public Shared Function Generate(ByVal fen As System.Windows.Forms.Form, _
                                    ByVal node As XmlNode, ByVal strClassId As String, _
                               ByVal strPackageId As String, ByVal eLanguage As ELanguage, _
                               ByVal strProgramFolder As String, ByRef strTransformation As String) As Boolean

        Dim bResult As Boolean = False

        Try

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim argList As New XslSimpleTransform.Arguments

            argList.Add("ProjectFolder", strProgramFolder)
            argList.Add("ToolsFolder", strUmlFolder)
            argList.Add("LanguageFolder", Application.LocalUserAppDataPath.ToString)
            argList.Add("InputClass", strClassId)
            argList.Add("InputPackage", strPackageId)

            ' strRootExtractionFolder could be changed downward
            Dim strRootExtractionFolder As String = strProgramFolder

            Select Case eLanguage
                Case eLanguage.Language_CplusPlus
                    If GenerateCppSourceHeader(fen, node.OwnerDocument.DocumentElement, _
                                            strClassId, strPackageId, strRootExtractionFolder, argList, _
                                            strTransformation) _
                    Then
                        bResult = True
                    End If

                Case eLanguage.Language_Vbasic
                    If GenerateVbClassModule(fen, node.OwnerDocument.DocumentElement, _
                                            strClassId, strPackageId, strRootExtractionFolder, argList, _
                                            strTransformation) _
                    Then
                        bResult = True
                    End If

                Case eLanguage.Language_Java
                    If GenerateJavaModule(fen, node.OwnerDocument.DocumentElement, _
                                            strClassId, strPackageId, strRootExtractionFolder, argList, _
                                            strTransformation) _
                    Then
                        bResult = True
                    End If
                Case Else
                    Throw New Exception("Macro for " + eLanguage.ToString + " not yet implemented")
            End Select
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Shared Function GenerateExternalTool(ByVal fen As System.Windows.Forms.Form, ByVal node As XmlNode, _
                                                ByVal strClassId As String, ByVal strPackageId As String, _
                                                ByVal eLanguage As ELanguage, ByVal strProgramFolder As String, _
                                                ByRef strTransformation As String, _
                                                ByVal ExternalCommand As MenuItemNode) As Boolean

        Dim bResult As Boolean = False

        Try

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim argList As New XslSimpleTransform.Arguments

            Try
                argList.Add("ToolsFolder", Path.GetDirectoryName(ExternalCommand.Stylesheet))

            Catch ex1 As Exception
                MsgExceptionBox(New Exception("Failed to extract directory path from file:" + vbCrLf + ExternalCommand.Stylesheet, ex1))
                Return False
            End Try

            argList.Add("ProjectFolder", strProgramFolder)
            argList.Add("LanguageFolder", Application.LocalUserAppDataPath.ToString)
            argList.Add("InputClass", strClassId)
            argList.Add("InputPackage", strPackageId)

            If GenerateAndLaunchExternalTool(fen, node.OwnerDocument.DocumentElement, _
                                            strClassId, strPackageId, strProgramFolder, argList, _
                                            strTransformation, ExternalCommand) _
            Then
                bResult = True
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
            Return bResult
    End Function
#End If
#End Region

#Region "Private shared methods"

#If _APP_UML = "1" Then

    Private Shared Function GenerateCppSourceHeader(ByVal fen As System.Windows.Forms.Form, _
                                                    ByVal node As XmlNode, ByVal strClassId As String, _
                                                ByVal strPackageId As String, ByRef strRootExtractionFolder As String, _
                                                ByVal argList As Dictionary(Of String, String), _
                                                ByRef strTransformation As String) As Boolean
        Dim bResult As Boolean = False
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        fen.Cursor = Cursors.WaitCursor

        Try
            If m_bTransformActive Then Return bResult
            m_bTransformActive = True

            observer.Minimum = 0
            observer.Maximum = 3
            observer.ProgressBarVisible = True

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            strTransformation = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, cstUpdate + ".xml")
            Dim strStyleSheet As String = My.Computer.FileSystem.CombinePath(strUmlFolder, cstCodeSourceHeaderCppStyleSheet)

            If node IsNot Nothing Then
                If DEBUG_COMMANDS_ACTIVE _
                Then
                    m_xsltCppSourceHeaderStyleSheet = New XslSimpleTransform(True)
                    m_xsltCppSourceHeaderStyleSheet.Load(strStyleSheet)
                Else
                    If m_xsltCppSourceHeaderStyleSheet Is Nothing Then
                        m_xsltCppSourceHeaderStyleSheet = New XslSimpleTransform(True)
                        m_xsltCppSourceHeaderStyleSheet.Load(strStyleSheet)
                    End If
                End If
                m_xsltCppSourceHeaderStyleSheet.Transform(node, strTransformation, argList)

                Dim lstFileList As New ArrayList
                ExtractCode(observer, strTransformation, strRootExtractionFolder, lstFileList)
                MergeCode(ELanguage.Language_CplusPlus, strRootExtractionFolder, My.Settings.DiffTool, _
                          My.Settings.DiffToolArguments, lstFileList)
                bResult = True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            observer.ProgressBarVisible = False
            fen.Cursor = oldCursor
            m_bTransformActive = False
        End Try

        Return bResult
    End Function

    Private Shared Function GenerateVbClassModule(ByVal fen As System.Windows.Forms.Form, _
                                                  ByVal node As XmlNode, ByVal strClassId As String, _
                                                ByVal strPackageId As String, ByRef strRootExtractionFolder As String, _
                                                ByVal argList As Dictionary(Of String, String), _
                                                ByRef strTransformation As String) As Boolean
        Dim bCodeMerge As Boolean = False
        Dim bResult As Boolean = False
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        fen.Cursor = Cursors.WaitCursor
        Try
            If m_bTransformActive Then Return bResult
            m_bTransformActive = True

            observer.Minimum = 0
            observer.Maximum = 3
            observer.ProgressBarVisible = True

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim strStyleSheet As String = My.Computer.FileSystem.CombinePath(strUmlFolder, cstCodeSourceVbDotNetStyleSheet)
            strTransformation = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, cstUpdate + ".xml")

            If node IsNot Nothing Then
                If DEBUG_COMMANDS_ACTIVE _
                Then
                    m_xsltVbClassModuleStyleSheet = New XslSimpleTransform(True)
                    m_xsltVbClassModuleStyleSheet.Load(strStyleSheet)
                Else
                    If m_xsltVbClassModuleStyleSheet Is Nothing Then
                        m_xsltVbClassModuleStyleSheet = New XslSimpleTransform(True)
                        m_xsltVbClassModuleStyleSheet.Load(strStyleSheet)
                    End If
                End If
                m_xsltVbClassModuleStyleSheet.Transform(node, strTransformation, argList)
                observer.Increment(1)


                Dim lstFileList As New ArrayList
                ExtractCode(observer, strTransformation, strRootExtractionFolder, lstFileList)
                MergeCode(ELanguage.Language_Vbasic, strRootExtractionFolder, My.Settings.DiffTool, _
                          My.Settings.DiffToolArguments, lstFileList)
                bResult = True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            observer.ProgressBarVisible = False
            fen.Cursor = oldCursor
            m_bTransformActive = False
        End Try

        Return bResult
    End Function

    Private Shared Function GenerateJavaModule(ByVal fen As System.Windows.Forms.Form, _
                                                  ByVal node As XmlNode, ByVal strClassId As String, _
                                                ByVal strPackageId As String, ByRef strRootExtractionFolder As String, _
                                                ByVal argList As Dictionary(Of String, String), _
                                                ByRef strTransformation As String) As Boolean
        Dim bCodeMerge As Boolean = False
        Dim bResult As Boolean = False
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        fen.Cursor = Cursors.WaitCursor
        Try
            If m_bTransformActive Then Return bResult
            m_bTransformActive = True

            observer.Minimum = 0
            observer.Maximum = 3
            observer.ProgressBarVisible = True

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim strStyleSheet As String = My.Computer.FileSystem.CombinePath(strUmlFolder, cstCodeSourceJavaStyleSheet)
            strTransformation = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, cstUpdate + ".xml")

            If node IsNot Nothing Then
                If DEBUG_COMMANDS_ACTIVE _
                Then
                    m_xsltJavaModuleStyleSheet = New XslSimpleTransform(True)
                    m_xsltJavaModuleStyleSheet.Load(strStyleSheet)
                Else
                    If m_xsltJavaModuleStyleSheet Is Nothing Then
                        m_xsltJavaModuleStyleSheet = New XslSimpleTransform(True)
                        m_xsltJavaModuleStyleSheet.Load(strStyleSheet)
                    End If
                End If
                m_xsltJavaModuleStyleSheet.Transform(node, strTransformation, argList)
                observer.Increment(1)

                Dim lstFileList As New ArrayList
                ExtractCode(observer, strTransformation, strRootExtractionFolder, lstFileList)
                MergeCode(ELanguage.Language_Java, strRootExtractionFolder, My.Settings.DiffTool, _
                          My.Settings.DiffToolArguments, lstFileList)
                bResult = True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            observer.ProgressBarVisible = False
            fen.Cursor = oldCursor
            m_bTransformActive = False
        End Try

        Return bResult
    End Function

    Private Shared Function GenerateAndLaunchExternalTool(ByVal fen As System.Windows.Forms.Form, _
                                                  ByVal node As XmlNode, ByVal strClassId As String, _
                                                ByVal strPackageId As String, ByRef strRootExtractionFolder As String, _
                                                ByVal argList As Dictionary(Of String, String), _
                                                ByRef strTransformation As String, _
                                                ByVal ExternalTool As MenuItemNode) As Boolean
        Dim bResult As Boolean = False
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        fen.Cursor = Cursors.WaitCursor
        Try
            If m_bTransformActive Then Return bResult
            m_bTransformActive = True

            observer.Minimum = 0
            observer.Maximum = 3
            observer.ProgressBarVisible = True

            strTransformation = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, cstUpdate + ".xml")

            If node IsNot Nothing Then
                Dim xsltExternalToolStyleSheet As XslSimpleTransform = New XslSimpleTransform(True)
                xsltExternalToolStyleSheet.Load(ExternalTool.Stylesheet)

                ConvertXslParams(argList, ExternalTool.XslParams)

                xsltExternalToolStyleSheet.Transform(node, strTransformation, argList)
                observer.Increment(1)

                Dim strDiffArguments As String = ConvertArguments(ExternalTool.DiffArguments, strRootExtractionFolder)
                Dim strToolArguments As String = ConvertArguments(ExternalTool.ToolArguments, strRootExtractionFolder)

                Dim lstFileList As New ArrayList
                Dim bPostGeneration As Boolean = Not (String.IsNullOrEmpty(ExternalTool.Tool))

                ExtractCode(observer, strTransformation, strRootExtractionFolder, lstFileList)

                MergeCode(ELanguage.Language_Tools, strRootExtractionFolder, ExternalTool.DiffTool, _
                              strDiffArguments, lstFileList, bPostGeneration)

                If bPostGeneration Then
                    LaunchProcess(ExternalTool.Tool, strToolArguments, strRootExtractionFolder, lstFileList)
                End If

                bResult = True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            observer.ProgressBarVisible = False
            fen.Cursor = oldCursor
            m_bTransformActive = False
        End Try

        Return bResult
    End Function

    Private Shared Sub ConvertXslParams(ByVal argList As Dictionary(Of String, String), ByVal strArguments As String)
        Try
            Dim regex As New Regex("\-(\w+)\=(\b[a-zA-Z0-9.\-_]+\b)")
            If String.IsNullOrEmpty(strArguments) Then
                Return
            ElseIf regex.IsMatch(strArguments) _
            Then
                Dim m As Match = regex.Match(strArguments)
                While (m.Success)
                    Dim param As Group = m.Groups(1)
                    Dim value As Group = m.Groups(2)
                    If argList.ContainsKey(param.Value) _
                    Then
                        MsgBox("XSL param '" + param.Value + "' already defined or reserved!", MsgBoxStyle.Exclamation, "XSL transformation parameters")
                    Else
                        argList.Add(param.Value, value.Value)
                    End If
                    m = m.NextMatch()
                End While
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function ConvertFileListArgs(ByVal FileList As ArrayList, ByVal strArguments As String) As String
        Dim strResult As String = strArguments
        Try
            Dim regex As New Regex("(\{[0-9]\})")
            If String.IsNullOrEmpty(strArguments) Then
                Return ""
            ElseIf regex.IsMatch(strArguments) Then
                Dim tempo() As Object = FileList.ToArray()
                strResult = String.Format(strArguments, tempo)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Private Shared Function ConvertArguments(ByVal strArguments As String, ByVal strProgramFolder As String) As String
        Dim strResult As String = strArguments
        Try
            Dim regex As New Regex("(\{\$ProjectFolder\})")
            If String.IsNullOrEmpty(strArguments) Then
                Return ""
            ElseIf regex.IsMatch(strArguments) Then
                strResult = regex.Replace(strArguments, Chr(34) + strProgramFolder + Chr(34))
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function
#End If

    Public Shared Function GenerateSimpleTransformation(ByVal fen As System.Windows.Forms.Form, _
                                                         ByRef strTransformation As String) As Boolean
        Dim bResult As Boolean = False
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        fen.Cursor = Cursors.WaitCursor
        Dim strRootExtractionFolder As String = ""

        Try

            observer.Minimum = 0
            observer.Maximum = 3
            observer.ProgressBarVisible = True

            Dim lstFileList As New ArrayList
            Dim bMergeOk As Boolean = False

            ExtractCode(observer, strTransformation, strRootExtractionFolder, lstFileList)

            For Each node As CodeInfo In lstFileList
                If node.bSourceExists Then
                    BackupFile(node.strTempFile, node.strReleaseFile)
                End If
                If node.bCodeMerge And node.bSourceExists Then
                    bMergeOk = True
                End If
            Next
            If bMergeOk Then
                MsgBox("Process has detected 'merged' files but doesn't assume it.", MsgBoxStyle.Exclamation)
            End If
            bResult = True

        Catch ex As Exception
            Throw ex
        Finally
            observer.ProgressBarVisible = False
            fen.Cursor = oldCursor
            m_bTransformActive = False
        End Try

        Return bResult
    End Function

    Private Shared Sub ExtractCode(ByVal observer As InterfProgression, _
                                   ByVal strTransformation As String, ByRef strRootExtractionFolder As String, _
                                   ByVal lstFileList As ArrayList)

        Try
            ' This document is used only to count element to generate
            Dim doc As New XmlDocument
            doc.Load(strTransformation)
            observer.Increment(1)

            ' One for XSL transformation
            Dim count As Integer = 1
            count += doc.SelectNodes("descendant::package | descendant::code").Count
            doc = Nothing

            observer.Maximum = count

            ' Load the reader with the data file and ignore all white space nodes.         
            Using reader As XmlTextReader = New XmlTextReader(strTransformation)

                reader.WhitespaceHandling = WhitespaceHandling.None

                ' Parse the file and display each of the nodes.
                While reader.Read()
                    Select Case reader.NodeType
                        Case XmlNodeType.Element
                            Select Case reader.Name
                                Case cstDocumentElement
                                    ExtractRootFolder(strRootExtractionFolder, reader)

                                Case cstFolderElement
                                    ExtractPackageFolder(observer, strRootExtractionFolder, reader, lstFileList)

                                Case cstFileElement
                                    ExtractFileNode(observer, strRootExtractionFolder, reader, lstFileList)
                                Case Else
                                    'Debug.Print("Node ignored:=" + reader.Name)
                            End Select
                        Case XmlNodeType.EndElement
                            'Debug.Print("End Node:=" + reader.Name)
                    End Select
                End While
                reader.Close()
            End Using
        Catch ex As Exception
            Throw New Exception("Code extraction failed, see inner exception and temporary file bellow:" + vbCrLf + vbCrLf + strTransformation, ex)
            Throw ex
        End Try
    End Sub

    Private Shared Function ReadFileDescriptor(ByVal reader As XmlTextReader, ByVal currentFolder As String) As CodeInfo
        Dim fileDescriptor As New CodeInfo
        Try
            fileDescriptor.bCodeMerge = False
            fileDescriptor.bCodeMerge = False
            fileDescriptor.bSourceExists = False

            reader.MoveToFirstAttribute()
            Do
                Select Case reader.Name
                    Case "name"
                        Try
                            fileDescriptor.strTempFile = My.Computer.FileSystem.CombinePath(currentFolder, reader.Value.Trim())

                        Catch ex As Exception
                            Throw New Exception("Filename '" + reader.Value + "' or current folder '" + currentFolder + "' is not available", ex)
                        End Try

                    Case "Merge"
                        fileDescriptor.bCodeMerge = (reader.Value.Trim() = "yes")
                End Select
            Loop While reader.MoveToNextAttribute

            If String.IsNullOrEmpty(fileDescriptor.strTempFile) = False _
            Then
                'Debug.Print("ExtractClass:=" + currentFolder + strNewFile)
                Dim strNewFile As String = Path.GetFileName(fileDescriptor.strTempFile)
                fileDescriptor.strReleaseFile = fileDescriptor.strTempFile
                fileDescriptor.bSourceExists = My.Computer.FileSystem.FileExists(fileDescriptor.strReleaseFile)

                If fileDescriptor.bSourceExists Then
                    fileDescriptor.strTempFile = My.Computer.FileSystem.CombinePath(CreateTempFolder(currentFolder), _
                                                                                    strNewFile + ".new")
                End If
            Else
                Throw New Exception("Attribute 'name' missed in a 'code' node !")
            End If

            reader.MoveToElement()

        Catch ex As Exception
            Throw ex
        End Try
        Return fileDescriptor
    End Function

    Private Shared Sub ExtractRootFolder(ByRef rootFolder As String, ByVal reader As XmlTextReader)
        reader.MoveToFirstAttribute()
        Do
            Select Case reader.Name
                Case "project"
                    rootFolder = reader.Value.Trim()
            End Select
        Loop While reader.MoveToNextAttribute
        reader.MoveToElement()
    End Sub

    Private Shared Sub ExtractPackageFolder(ByVal observer As InterfProgression, _
                                      ByVal currentFolder As String, ByVal reader As XmlTextReader, _
                                      ByVal lstFileList As ArrayList)
        Try
            observer.Increment(1)

            reader.MoveToFirstAttribute()
            If reader.Name <> "name" Then
                Exit Sub
            End If

            Dim strNewFolder As String = reader.Value.Trim()
            reader.MoveToElement()

            strNewFolder = CreateBranch(currentFolder, strNewFolder)

            While reader.Read()
                Select Case reader.NodeType
                    Case XmlNodeType.Element
                        Select Case reader.Name
                            Case cstFolderElement
                                ExtractPackageFolder(observer, strNewFolder, reader, lstFileList)

                            Case cstFileElement
                                ExtractFileNode(observer, strNewFolder, reader, lstFileList)

                            Case Else
                                'Debug.Print("Node ignored:=" + reader.Name)
                        End Select
                    Case XmlNodeType.EndElement
                        'Debug.Print("End Node:=" + reader.Name)
                        If reader.Name = cstFolderElement Then Exit While
                End Select
            End While
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub ExtractFileNode(ByVal observer As InterfProgression, _
                                    ByVal currentFolder As String, ByVal reader As XmlTextReader, _
                                    ByVal lstFileList As ArrayList)
        Try
            observer.Increment(1)

            Dim bCodeMerge As Boolean = False
            Dim fileInfo As CodeInfo = ReadFileDescriptor(reader, currentFolder)

            Using streamWriter As StreamWriter = New StreamWriter(fileInfo.strTempFile)
                While reader.Read()
                    Select Case reader.NodeType
                        Case XmlNodeType.Element

                        Case XmlNodeType.CDATA
                            streamWriter.Write(reader.Value)

                        Case XmlNodeType.EndElement
                            If reader.Name = "code" Then
                                Exit While
                            End If
                    End Select
                End While
                streamWriter.Close()
            End Using

            If lstFileList IsNot Nothing Then
                lstFileList.Add(fileInfo)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#If _APP_UML = "1" Then

    Private Shared Sub MergeCode(ByVal eLang As ELanguage, ByVal strFolder As String, _
                                 ByVal strExternalMerger As String, _
                                 ByVal strArguments As String, ByVal lstFileList As ArrayList, _
                                 Optional ByVal bPostGeneration As Boolean = False)
        Try
            Dim lstFilesToMerge As New ArrayList
            For Each fileInfo As CodeInfo In lstFileList
                If MergeNode(eLang, (strExternalMerger.Length > 0), fileInfo) Then
                    fileInfo.Filename = strFolder
                    lstFilesToMerge.Add(fileInfo)
                End If
            Next

            lstFileList.Clear()

            If lstFilesToMerge.Count = 1 _
            Then
                Dim fileInfo As CodeInfo = CType(lstFilesToMerge.Item(0), CodeInfo)
                CompareAndMergeFiles(strExternalMerger, strArguments, _
                                     fileInfo.strTempFile, fileInfo.strReleaseFile, _
                                     bPostGeneration)

            ElseIf lstFilesToMerge.Count > 1 _
            Then
                Dim fen As New dlgMergeFileList
                fen.ProjectFolder = strFolder
                fen.FileList = lstFilesToMerge
                fen.ExternalMerger = strExternalMerger
                fen.Arguments = strArguments
                fen.Show()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function MergeNode(ByVal eLang As ELanguage, ByVal bExternalMerger As Boolean, ByVal fileInfo As CodeInfo) As Boolean

        Dim bResult As Boolean = True
        Try
            Select Case eLang
                Case ELanguage.Language_Vbasic
                    If fileInfo.bSourceExists Then
                        ' User can choose between automatic and manual external merger
                        If My.Settings.VbMergeTool Then
                            VbCodeMerger.Merge(fileInfo, cstTempExport)
                            bResult = False

                        ElseIf bExternalMerger Then
                            ' Use an external merger to add/remove code as user ask it
                            bResult = True
                            '                           CompareAndMergeFiles(strExternalMerger, strArguments, fileInfo.strTempFile, fileInfo.strReleaseFile)
                        Else
                            ' Can't merge, tool unavailable
                            BackupFile(fileInfo.strTempFile, fileInfo.strReleaseFile)
                            bResult = False
                        End If
                    Else
                        ' No need to merge or backup
                        bResult = False
                    End If

                Case Else
                    If fileInfo.bSourceExists Then
                        If fileInfo.bCodeMerge And bExternalMerger Then
                            ' Use an external merger to add/remove code as user ask it
                            bResult = True
                            '                        CompareAndMergeFiles(strExternalMerger, strArguments, fileInfo.strTempFile, fileInfo.strReleaseFile)

                        Else
                            ' No need to merge
                            BackupFile(fileInfo.strTempFile, fileInfo.strReleaseFile)
                            bResult = False
                        End If
                    Else
                        ' No need to merge or backup
                        bResult = False
                    End If
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function
#End If

    Private Shared Function CreateBranch(ByVal ExistingPath As String, ByVal NewBranch As String) As String
        Dim strResult As String = ""
        Try
            strResult = My.Computer.FileSystem.CombinePath(ExistingPath, NewBranch)

            If My.Computer.FileSystem.DirectoryExists(strResult) = False _
            Then
                My.Computer.FileSystem.CreateDirectory(strResult)
                'Debug.Print("CreateBranch:=" + strResult)
            End If
        Catch ex As Exception
            Throw New Exception("Can't create branch '" + NewBranch + "' at existing path '" + ExistingPath + "'", ex)
        End Try
        Return strResult
    End Function

    Private Shared Function CreateTempFolder(ByVal ExistingPath As String) As String
        Dim strResult As String = ""
        Try
            strResult = My.Computer.FileSystem.CombinePath(ExistingPath, cstTempExport)

            If My.Computer.FileSystem.DirectoryExists(strResult) = False _
            Then
                My.Computer.FileSystem.CreateDirectory(strResult)
                Dim Info As DirectoryInfo = My.Computer.FileSystem.GetDirectoryInfo(strResult)
                Info.Attributes = Info.Attributes Or FileAttributes.Hidden
                'Debug.Print("CreateTempFolder:=" + strResult)
            End If
        Catch ex As Exception
            Throw New Exception("Can't create temporary folder '" + cstTempExport + "' at existing path '" + ExistingPath + "'", ex)
        End Try
        Return strResult
    End Function

    Private Shared Sub BackupFile(ByVal strTempFile As String, ByVal strReleaseFile As String)
        Try
            My.Computer.FileSystem.MoveFile(strReleaseFile, strReleaseFile + ".bak", True)
            My.Computer.FileSystem.MoveFile(strTempFile, strReleaseFile)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#If _APP_UML = "1" Then

    Private Shared Sub LaunchProcess(ByVal strProcess As String, ByVal strArguments As String, _
                                     ByVal strProjectFolder As String, ByVal lstFileList As ArrayList)

        Dim tempo As String = strArguments
        Try
            If lstFileList.Count > 0 Then
                tempo = ConvertFileListArgs(lstFileList, tempo)
            End If

            Dim proc As New Process()
            proc.StartInfo.FileName = strProcess
            proc.StartInfo.Arguments = tempo
            proc.StartInfo.CreateNoWindow = False
            proc.StartInfo.UseShellExecute = True
            proc.StartInfo.WorkingDirectory = strProjectFolder
            proc.StartInfo.ErrorDialog = True
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal

            ' Run it.
            proc.Start()

        Catch ex1 As Win32Exception

        Catch ex As Exception
            Throw New Exception("External tool failed!" + vbCrLf + vbCrLf + "Command: " + strProcess + vbCrLf + "Arguments: " + tempo, ex)
        End Try
    End Sub

    Public Shared Sub CompareAndMergeFiles(ByVal strExternalMerger As String, ByVal strArguments As String, _
                                            ByVal strTempFile As String, ByVal strReleaseFile As String, _
                                            Optional ByVal bWaitForExit As Boolean = False)

        Dim tempo As String = strArguments
        Try
            If My.Computer.FileSystem.FileExists(strExternalMerger) = False Then
                Dim fen As Form = New dlgDiffTool
                If fen.ShowDialog() = System.Windows.Forms.DialogResult.Cancel Then
                    MsgBox("Sorry but you should install WinMerge or an equivalent tool, please!", MsgBoxStyle.Critical, "Compare and merge tool")
                    Exit Sub
                End If
            End If
            If My.Computer.FileSystem.FileExists(strExternalMerger) = False Then
                MsgBox("Sorry but you should install WinMerge or an equivalent tool, please!", MsgBoxStyle.Critical, "Compare and merge tool")
                Exit Sub
            End If

            tempo = String.Format(strArguments, Chr(34) + strTempFile + Chr(34), Chr(34) + strReleaseFile + Chr(34))

            Dim proc As New Process()
            proc.StartInfo.FileName = strExternalMerger
            proc.StartInfo.Arguments = tempo
            proc.StartInfo.ErrorDialog = True
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal

            ' Run it and wait end infinite.
            proc.Start()
            If bWaitForExit Then proc.WaitForExit()

        Catch ex1 As Win32Exception

        Catch ex As Exception
            Throw New Exception("External merger failed!" + vbCrLf + vbCrLf + "Command: " + strExternalMerger + vbCrLf + "Arguments: " + tempo, ex)
        End Try
    End Sub
#End If
#End Region
End Class
