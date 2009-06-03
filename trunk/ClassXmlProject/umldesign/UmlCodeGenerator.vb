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
Imports ClassXmlProject.MenuItemCommand

Public Class UmlCodeGenerator

#Region "Class declarations"

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
    Private Shared m_xsltExternalToolStyleSheet As XslSimpleTransform = Nothing

    Private Shared m_bTransformActive As Boolean = False
    Private Shared m_strSeparator As String = Path.DirectorySeparatorChar.ToString
#End Region

#Region "Public shared methods"

    Public Shared Function Generate(ByVal fen As System.Windows.Forms.Form, _
                                    ByVal node As XmlNode, ByVal strClassId As String, _
                               ByVal strPackageId As String, ByVal eLanguage As ELanguage, _
                               ByVal strProgramFolder As String, ByRef strTransformation As String) As Boolean

        Dim bResult As Boolean = False

        Try

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim argList As New Dictionary(Of String, String)

            argList.Add("ProjectFolder", strProgramFolder)
            argList.Add("ToolsFolder", strUmlFolder)
            argList.Add("LanguageFolder", Application.LocalUserAppDataPath.ToString)
            argList.Add("InputClass", strClassId)
            argList.Add("InputPackage", strPackageId)

            Select Case eLanguage
                Case eLanguage.Language_CplusPlus
                    If GenerateCppSourceHeader(fen, node.OwnerDocument.DocumentElement, _
                                            strClassId, strPackageId, strProgramFolder, argList, _
                                            strTransformation) _
                    Then
                        bResult = True
                    End If

                Case eLanguage.Language_Vbasic
                    If GenerateVbClassModule(fen, node.OwnerDocument.DocumentElement, _
                                            strClassId, strPackageId, strProgramFolder, argList, _
                                            strTransformation) _
                    Then
                        bResult = True
                    End If

                Case eLanguage.Language_Java
                    If GenerateJavaModule(fen, node.OwnerDocument.DocumentElement, _
                                            strClassId, strPackageId, strProgramFolder, argList, _
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
            Dim argList As New Dictionary(Of String, String)

            argList.Add("ProjectFolder", strProgramFolder)
            argList.Add("ToolsFolder", Path.GetDirectoryName(ExternalCommand.Stylesheet))
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
#End Region

#Region "Private shared methods"

    Private Shared Function GenerateCppSourceHeader(ByVal fen As System.Windows.Forms.Form, _
                                                    ByVal node As XmlNode, ByVal strClassId As String, _
                                                ByVal strPackageId As String, ByVal strPath As String, _
                                                ByVal argList As Dictionary(Of String, String), _
                                                ByRef strTransformation As String) As Boolean
        Dim bResult As Boolean = False
        Dim bCodeMerge As Boolean = False
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        fen.Cursor = Cursors.WaitCursor

        Try
            If m_bTransformActive Then Return bResult
            m_bTransformActive = True

            Dim count As Integer = 1    ' One step for XSLT transformation
            If strClassId <> "" Then
                count += 1
            ElseIf strPackageId <> "" _
            Then
                Dim child As XmlNode = node.SelectSingleNode("//package[@id='" + strPackageId + "']")
                count += child.SelectNodes("descendant::package | descendant::class").Count
            Else
                count += node.SelectNodes("descendant::package | descendant::class").Count
            End If

            observer.Minimum = 0
            observer.Maximum = count
            observer.ProgressBarVisible = True

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            strTransformation = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, cstUpdate + ".xml")
            Dim strStyleSheet As String = My.Computer.FileSystem.CombinePath(strUmlFolder, cstCodeSourceHeaderCppStyleSheet)

            If node IsNot Nothing Then
                If XmlProjectTools.DEBUG_COMMANDS_ACTIVE _
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

                ExtractCode(bCodeMerge, observer, strTransformation, strPath, _
                            ELanguage.Language_CplusPlus, My.Settings.DiffTool, "{0} {1}")
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
                                                ByVal strPackageId As String, ByVal strPath As String, _
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

            Dim count As Integer = 1    ' One step for XSLT transformation
            If strClassId <> "" Then
                count += 1
            ElseIf strPackageId <> "" _
            Then
                Dim child As XmlNode = node.SelectSingleNode("//package[@id='" + strPackageId + "']")
                count += child.SelectNodes("descendant::package | descendant::class").Count
            Else
                count += node.SelectNodes("descendant::package | descendant::class").Count
            End If

            observer.Minimum = 0
            observer.Maximum = count
            observer.ProgressBarVisible = True

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim strStyleSheet As String = My.Computer.FileSystem.CombinePath(strUmlFolder, cstCodeSourceVbDotNetStyleSheet)
            strTransformation = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, cstUpdate + ".xml")

            If node IsNot Nothing Then
                If XmlProjectTools.DEBUG_COMMANDS_ACTIVE _
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

                ExtractCode(bCodeMerge, observer, strTransformation, strPath, _
                            ELanguage.Language_Vbasic, My.Settings.DiffTool, "{0} {1}")
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
                                                ByVal strPackageId As String, ByVal strPath As String, _
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

            Dim count As Integer = 1    ' One step for XSLT transformation
            If strClassId <> "" Then
                count += 1
            ElseIf strPackageId <> "" _
            Then
                Dim child As XmlNode = node.SelectSingleNode("//package[@id='" + strPackageId + "']")
                count += child.SelectNodes("descendant::package | descendant::class").Count
            Else
                count += node.SelectNodes("descendant::package | descendant::class").Count
            End If

            observer.Minimum = 0
            observer.Maximum = count
            observer.ProgressBarVisible = True

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim strStyleSheet As String = My.Computer.FileSystem.CombinePath(strUmlFolder, cstCodeSourceJavaStyleSheet)
            strTransformation = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, cstUpdate + ".xml")

            If node IsNot Nothing Then
                If XmlProjectTools.DEBUG_COMMANDS_ACTIVE _
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

                ExtractCode(bCodeMerge, observer, strTransformation, strPath, _
                            ELanguage.Language_Java, My.Settings.DiffTool, "{0} {1}")
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
                                                ByVal strPackageId As String, ByVal strProgramFolder As String, _
                                                ByVal argList As Dictionary(Of String, String), _
                                                ByRef strTransformation As String, _
                                                ByVal ExternalTool As MenuItemNode) As Boolean
        Dim bCodeMerge As Boolean = False
        Dim bResult As Boolean = False
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        fen.Cursor = Cursors.WaitCursor
        Try
            If m_bTransformActive Then Return bResult
            m_bTransformActive = True

            Dim count As Integer = 1    ' One step for XSLT transformation
            If strClassId <> "" Then
                count += 1
            ElseIf strPackageId <> "" _
            Then
                Dim child As XmlNode = node.SelectSingleNode("//package[@id='" + strPackageId + "']")
                count += child.SelectNodes("descendant::package | descendant::class").Count
            Else
                count += node.SelectNodes("descendant::package | descendant::class").Count
            End If

            observer.Minimum = 0
            observer.Maximum = count
            observer.ProgressBarVisible = True

            strTransformation = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, cstUpdate + ".xml")

            If node IsNot Nothing Then
                If XmlProjectTools.DEBUG_COMMANDS_ACTIVE _
                Then
                    m_xsltExternalToolStyleSheet = New XslSimpleTransform(True)
                    m_xsltExternalToolStyleSheet.Load(ExternalTool.Stylesheet)
                Else
                    If m_xsltExternalToolStyleSheet Is Nothing Then
                        m_xsltExternalToolStyleSheet = New XslSimpleTransform(True)
                        m_xsltExternalToolStyleSheet.Load(ExternalTool.Stylesheet)
                    End If
                End If

                ConvertXslParams(argList, ExternalTool.XslParams)

                m_xsltExternalToolStyleSheet.Transform(node, strTransformation, argList)
                observer.Increment(1)

                Dim strDiffArguments As String = ConvertArguments(ExternalTool.DiffArguments, strProgramFolder)
                Dim strToolArguments As String = ConvertArguments(ExternalTool.ToolArguments, strProgramFolder)

                Dim lstFileList As New ArrayList

                ExtractCode(bCodeMerge, observer, strTransformation, strProgramFolder, _
                            ELanguage.Language_Tools, ExternalTool.DiffTool, _
                            strDiffArguments, lstFileList)

                If String.IsNullOrEmpty(ExternalTool.Tool) = False Then
                    LaunchProcess(ExternalTool.Tool, strToolArguments, strProgramFolder, lstFileList)
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
            Dim regex As New Regex("\-(\w+)\=(\b\w+\b)")
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

    Private Shared Sub ExtractCode(ByRef bCodeMerge As Boolean, ByVal observer As InterfProgression, _
                                   ByVal strTransformation As String, ByVal strFolder As String, _
                                   ByVal eLang As ELanguage, ByVal strExternalMerger As String, _
                                   ByVal strArguments As String, Optional ByVal lstFileList As ArrayList = Nothing)
        Try
            If eLang <> ELanguage.Language_CplusPlus Then
                bCodeMerge = True
            End If
            ' Load the reader with the data file and ignore all white space nodes.         
            Using reader As XmlTextReader = New XmlTextReader(strTransformation)

                reader.WhitespaceHandling = WhitespaceHandling.None

                ' Parse the file and display each of the nodes.
                While reader.Read()
                    Select Case reader.NodeType
                        Case XmlNodeType.Element
                            Select Case reader.Name
                                Case cstFolderElement
                                    ExtractPackage(observer, strFolder, reader, eLang, True, _
                                                   strExternalMerger, strArguments, lstFileList)

                                Case cstFileElement
                                    ExtractClass(observer, strFolder, reader, eLang, True, _
                                                 strExternalMerger, strArguments, lstFileList)
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
            Throw ex
        End Try
    End Sub

    Private Shared Sub ExtractPackage(ByVal observer As InterfProgression, _
                                      ByVal currentFolder As String, ByVal reader As XmlTextReader, _
                                      ByVal eLang As ELanguage, ByVal bUseTempFolder As Boolean, _
                                      ByVal strExternalMerger As String, ByVal strArguments As String, Optional ByVal lstFileList As ArrayList = Nothing)
        Try
            observer.Increment(1)

            reader.MoveToFirstAttribute()
            If reader.Name <> "name" Then
                Exit Sub
            End If

            Dim strNewFolder As String = reader.Value
            reader.MoveToElement()

            strNewFolder = CreateBranch(currentFolder, strNewFolder)

            While reader.Read()
                Select Case reader.NodeType
                    Case XmlNodeType.Element
                        Select Case reader.Name
                            Case cstFolderElement
                                ExtractPackage(observer, strNewFolder, reader, eLang, _
                                               bUseTempFolder, strExternalMerger, strArguments)

                            Case cstFileElement
                                ExtractClass(observer, strNewFolder, reader, eLang, _
                                             bUseTempFolder, strExternalMerger, strArguments)
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

    Private Shared Sub ExtractClass(ByVal observer As InterfProgression, _
                                    ByVal currentFolder As String, ByVal reader As XmlTextReader, _
                                    ByVal eLang As ELanguage, ByVal bUseTempFolder As Boolean, _
                                    ByVal strExternalMerger As String, ByVal strArguments As String, Optional ByVal lstFileList As ArrayList = Nothing)
        Try
            observer.Increment(1)

            reader.MoveToFirstAttribute()
            Dim bCodeMerge As Boolean = False

            If reader.Name <> "name" Then
                bCodeMerge = (reader.Value = "yes")
                reader.MoveToNextAttribute()
            End If

            Dim strNewFile As String = reader.Value
            reader.MoveToElement()

            'Debug.Print("ExtractClass:=" + currentFolder + strNewFile)
            Dim strTempFile As String = My.Computer.FileSystem.CombinePath(currentFolder, strNewFile)
            Dim strReleaseFile As String = strTempFile
            Dim bSourceExists As Boolean = My.Computer.FileSystem.FileExists(strReleaseFile)

            If bUseTempFolder And bSourceExists Then
                strTempFile = My.Computer.FileSystem.CombinePath(CreateTempFolder(currentFolder), strNewFile)
            End If

            Using streamWriter As StreamWriter = New StreamWriter(strTempFile)
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

            If bUseTempFolder Then
                Select Case eLang
                    Case ELanguage.Language_Vbasic
                        If bSourceExists Then
                            ' Use can choose between automatic and manual external merger
                            If My.Settings.VbMergeTool Then
                                VbCodeMerger.Merge(currentFolder, strNewFile, cstTempExport)
                            Else
                                CompareAndMergeFiles(strExternalMerger, strArguments, strTempFile, strReleaseFile)
                            End If
                        Else
                            ' No need to merge or backup
                        End If

                    Case ELanguage.Language_Tools
                        If bCodeMerge And bSourceExists And strExternalMerger.Length > 0 Then
                            ' Temporary use an external merger to add/remove code
                            CompareAndMergeFiles(strExternalMerger, strArguments, strTempFile, strReleaseFile)

                        ElseIf bSourceExists Then
                            ' No need to merge
                            BackupFile(strTempFile, strReleaseFile)
                        End If

                    Case Else
                        If bCodeMerge And bSourceExists Then
                            ' Temporary use an external merger to add/remove code
                            CompareAndMergeFiles(strExternalMerger, strArguments, strTempFile, strReleaseFile)

                        ElseIf bSourceExists Then
                            ' No need to merge
                            BackupFile(strTempFile, strReleaseFile)
                        End If
                End Select
            End If

            If lstFileList IsNot Nothing Then
                lstFileList.Add(strReleaseFile)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function CreateBranch(ByVal ExistingPath As String, ByVal NewBranch As String) As String
        Dim strResult As String = My.Computer.FileSystem.CombinePath(ExistingPath, NewBranch)
        Try

            If My.Computer.FileSystem.DirectoryExists(strResult) = False _
            Then
                My.Computer.FileSystem.CreateDirectory(strResult)
                'Debug.Print("CreateBranch:=" + strResult)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Private Shared Function CreateTempFolder(ByVal ExistingPath As String) As String
        Dim strResult As String = My.Computer.FileSystem.CombinePath(ExistingPath, cstTempExport)
        Try

            If My.Computer.FileSystem.DirectoryExists(strResult) = False _
            Then
                My.Computer.FileSystem.CreateDirectory(strResult)
                Dim Info As DirectoryInfo = My.Computer.FileSystem.GetDirectoryInfo(strResult)
                Info.Attributes = Info.Attributes Or FileAttributes.Hidden
                'Debug.Print("CreateTempFolder:=" + strResult)
            End If
        Catch ex As Exception
            Throw ex
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

    Private Shared Sub CompareAndMergeFiles(ByVal strExternalMerger As String, ByVal strArguments As String, _
                                            ByVal strTempFile As String, ByVal strReleaseFile As String)

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
            proc.WaitForExit()

        Catch ex1 As Win32Exception

        Catch ex As Exception
            Throw New Exception("External merger failed!" + vbCrLf + vbCrLf + "Command: " + strExternalMerger + vbCrLf + "Arguments: " + tempo, ex)
        End Try
    End Sub

#End Region
End Class
