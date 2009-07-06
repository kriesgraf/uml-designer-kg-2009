Imports System
Imports System.Collections.Generic
Imports System.Collections
Imports System.Xml
Imports System.Xml.Schema
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic

#If _APP_UML = "1" Then
Imports System.Windows.Forms
Imports ClassXmlProject.UmlNodesManager
Imports ClassXmlProject.UmlCodeGenerator
#End If

Public Class shlwapi
    <System.Runtime.InteropServices.DllImport("shlwapi.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
    Shared Function PathCompactPath(ByVal hDC As IntPtr, ByVal lpszPath As StringBuilder, ByVal dx As Integer) As Boolean
    End Function
End Class

Public Class user32
    <System.Runtime.InteropServices.DllImport("user32")> _
    Shared Function GetWindowDC(ByVal hWnd As IntPtr) As IntPtr
    End Function
End Class

Public Class XmlProjectTools

#Region "Class declarations"
    Public Shared DEBUG_COMMANDS_ACTIVE As Boolean = False

    Private Shared PrefixNameDocument As New XmlDocument

    Public Const cstMsgYesNoExclamation As MsgBoxStyle = CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, MsgBoxStyle)
    Public Const cstMsgYesNoCancelExclamation As MsgBoxStyle = CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNoCancel + MsgBoxStyle.DefaultButton2, MsgBoxStyle)
    Public Const cstMsgYesNoQuestion As MsgBoxStyle = CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, MsgBoxStyle)
    Public Const cstMsgOkCancelCritical As MsgBoxStyle = CType(MsgBoxStyle.Critical + MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton1, MsgBoxStyle)

    Public Const cstMaxCircularReferences As Integer = 20
    Public Const cstExternalToolsFileVersion As String = "1.0"
    Public Const cstXslTemplate As String = "uml2template.xsl"
    Public Const cstPrefixNameFilename As String = "language.xml"

    Private Const cstSchemaName As String = "class-model"
    Private Const cstSchemaVersion As String = "1.3"
    Private Const cstTempDoxygenFile As String = "__doxygenTempFile"
    Private Const cstTempUmlFile As String = "__umlTempFile"
    Private Const cstTag2ImportStyle As String = "tag2imp.xsl"
    Private Const cstDoxygen2ProjectStyle As String = "dox2prj.xsl"
    Private Const cstXmi2ProjectStyle As String = "xmi2prj.xsl"
    Private Const cstProject2XmiStyle As String = "xprj2xmi.xsl"
    Private Const cstIbmXmi2ProjectStyle As String = "rhp-xmi2prj.xsl"
    Private Const cstV1_2_To_V1_3_Patch As String = "Patch_V1_2ToV1_3.xsl"

    Public Shared regVariableName As Regex = New Regex("^[a-zA-Z_][a-zA-Z0-9_]{0,}$")
    Public Shared regVbAndJavaPackage As Regex = New Regex("^([a-zA-Z_][a-zA-Z0-9_]{1,}\.){0,}[a-zA-Z_][a-zA-Z0-9_]{1,}$")
    Public Shared regCppPackage As Regex = New Regex("^([a-zA-Z_][a-zA-Z0-9_]{1,}\:\:){0,}[a-zA-Z_][a-zA-Z0-9_]{1,}$")
    Public Shared regCppHeader As Regex = New Regex("^([a-zA-Z0-9_]{1,}(\/|\\)){0,}[a-zA-Z0-9_]{1,}(|\.h|\.hpp)$")
    Private Shared regOperator As New Regex("^(IsFalse|IsTrue|Not|" + _
                                            "\+|\+\+|\-|\-\-|\*|\/|\\|\&|\&\&|\||\|\||\%|\^|\>\>|\<\<|\=|\=\=|\!|\!\=|\<\>|\>|\>\=|\<|\<\=|" + _
                                            "And|Like|Mod|Or|Xor|CType)$")


    Public Enum EResult
        Completed
        Failed
        Converted
    End Enum

    Public Enum ECardinal
        Fix
        Variable
        EmptyList
        FullList
    End Enum

    Public Enum EKindParent
        Reference
        Array
        Container
    End Enum

    Public Enum EImplementation
        Unknown
        Interf
        Leaf
        Simple
        Root
        Node
        Exception
        Container
    End Enum

    Public Enum EDtdFileExist
        Equal
        EqualButDifferent
        NotFound
        Older
        MoreRecent
        SourceNotFound
    End Enum

    Public Structure TSimpleDeclaration
        Dim strTypeName As String
        Dim bConst As Boolean
        Dim bReference As Boolean
        Dim ilevel As Integer
        Dim strIdref As String
    End Structure

#End Region

#Region "Public shared methods"

#If _APP_UML = "1" Then
    Public Shared Function CompactPath(ByVal control As Control, ByVal strFullPathFilename As String) As String
        Dim strTempo As New StringBuilder(strFullPathFilename)
        shlwapi.PathCompactPath(user32.GetWindowDC(control.Handle), strTempo, control.ClientSize.Width)
        Return strTempo.ToString
    End Function

    Public Shared Function CreateToolsFile() As String
        Return "<?xml version='1.0' encoding='utf-8'?>" + vbCrLf + _
               "<root version='" + cstExternalToolsFileVersion + "'/>" + vbCrLf
    End Function

    Public Shared Function CreateNewProject() As String
        Return "<?xml version='1.0' encoding='utf-8'?>" + vbCrLf + _
               "<!DOCTYPE root SYSTEM '" + GetDtdResource() + "'>" + vbCrLf
    End Function

    Public Shared Function GetDocTypeDeclarationFile() As String
        Return cstSchemaName + ".dtd"
    End Function

    Public Shared Function GetDtdResource() As String
        Return My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder + cstSchemaName + ".xml")
    End Function

    Public Shared Function GetDestinationDtdFile(ByVal strDestinationFolder As String) As String
        Return My.Computer.FileSystem.CombinePath(strDestinationFolder, GetDocTypeDeclarationFile())
    End Function

    Public Shared Sub ReloadPrefixNameDocument(ByVal filename As String)
        Try
            PrefixNameDocument.Load(filename)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function GetPrefixName(ByVal name As String) As String
        Dim node As XmlNode = PrefixNameDocument.SelectSingleNode("//" + name)
        If node IsNot Nothing Then
            Return node.InnerText
        End If
        Return Nothing
    End Function

    Public Shared Function CheckVersionDtdFile(ByVal strDestinationFolder As String) As Boolean
        Try
            Dim fullpathname As String = GetDestinationDtdFile(strDestinationFolder)
            Dim document As New XmlDocument

            Dim strXML As String = "<?xml version='1.0' encoding='utf-8'?>" + vbCrLf + _
                                   "<!DOCTYPE root SYSTEM '" + fullpathname + "'>" + vbCrLf + _
                                   "<root name='Test'><generation destination='test' language='0' /><comment brief=''></comment></root>"

            document.LoadXml(strXML)

            Dim strCurrentVersion As String = GetAttributeValue(document.DocumentElement, "version")

            If strCurrentVersion IsNot Nothing _
            Then
                Return (strCurrentVersion = cstSchemaVersion)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return False
    End Function

    Public Shared Function CheckDocTypeDeclarationFile(ByVal strDestinationFolder As String) As EDtdFileExist
        Dim strDestination As String = GetDestinationDtdFile(strDestinationFolder)
        Dim strOrigin As String = GetDtdResource()

        If My.Computer.FileSystem.FileExists(strDestination) = False _
        Then
            Return EDtdFileExist.NotFound

        ElseIf My.Computer.FileSystem.FileExists(strOrigin) = False _
        Then
            Return EDtdFileExist.SourceNotFound
        Else
            Dim originInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(strOrigin)
            Dim destInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(strDestination)

            If originInfo.LastWriteTime > destInfo.LastWriteTime _
            Then
                If CheckVersionDtdFile(strDestinationFolder) Then
                    Return EDtdFileExist.EqualButDifferent
                End If
                Return EDtdFileExist.Older

            ElseIf originInfo.LastWriteTime < destInfo.LastWriteTime _
            Then
                If CheckVersionDtdFile(strDestinationFolder) Then
                    Return EDtdFileExist.EqualButDifferent
                End If
                Return EDtdFileExist.MoreRecent
            Else
                Return EDtdFileExist.Equal
            End If
        End If
        Return EDtdFileExist.NotFound
    End Function

    Public Shared Function GetDtdDeclaration(ByVal strRootElement As String) As String
        Return "<!DOCTYPE " + strRootElement + " SYSTEM '" + GetDocTypeDeclarationFile() + "'>" + vbCrLf
    End Function

    Public Shared Function GetProjectPath(ByVal strFullpathFilename As String) As String
        Return System.IO.Path.GetDirectoryName(strFullpathFilename)
    End Function

    Public Shared Function UseDocTypeDeclarationFileForProject(ByVal strSourceFolder As String) As Boolean

        Select Case CheckDocTypeDeclarationFile(strSourceFolder)
            Case EDtdFileExist.EqualButDifferent
                ' Copy file below

            Case EDtdFileExist.SourceNotFound
                MsgBox("The resource " + GetDtdResource() + " is missing. " + _
                       vbCrLf + "Please retry installation to replace missing files. ", MsgBoxStyle.Critical, "DTD resources")
                Return False

            Case EDtdFileExist.Equal
                ' Nothing to do
                Return True

            Case EDtdFileExist.MoreRecent
                Select Case MsgBox("File '" + GetDestinationDtdFile(strSourceFolder) + vbCrLf + " is more recent than application resource. " + vbCrLf + _
                           "Maybe this project would corrupt application process. " + vbCrLf + _
                           "Please confirm overwriting this file (Yes), open this project as it is (No), don't open this project (Cancel).", _
                           cstMsgYesNoCancelExclamation, strSourceFolder)

                    Case MsgBoxResult.Yes
                        ' Copy file below

                    Case MsgBoxResult.No
                        Return True

                    Case MsgBoxResult.Cancel
                        Return False
                End Select

            Case EDtdFileExist.NotFound
                If MsgBox("Can't find File '" + GetDestinationDtdFile(strSourceFolder) + vbCrLf + _
                          "An application resource file will be copy in this folder to replace it. " + vbCrLf + _
                       "Maybe this project would be incompatible with this resource file and  would corrupt application process." + vbCrLf + _
                       "Maybe you should apply a patch to upgrade this project and remove this warning." + vbCrLf + _
                       "Please confirm replace.", cstMsgYesNoExclamation, strSourceFolder) _
                       = MsgBoxResult.No _
                Then
                    Return False
                Else
                    ' Copy file below
                End If

            Case EDtdFileExist.Older
                If MsgWarningBox("File '" + GetDestinationDtdFile(strSourceFolder) + "'," + vbCrLf + _
                    "is more older than application resource. " + vbCrLf + _
                    "Maybe oldest version projects remain in this folder, overwrite this file would corrupt these projects." + vbCrLf + _
                    "If you confirm this operation, you would have to apply a patch to each project in this folder." + vbCrLf + _
                    "Also, we recommend you to move manually this project file in an other folder and press Cancel now. !", strSourceFolder) _
                    = System.Windows.Forms.DialogResult.Cancel _
                Then
                    Return False
                Else
                    ' Copy file below
                End If

            Case Else
                Throw New Exception("Method 'CheckDocTypeDeclarationFile' has returned a wrong value")
        End Select

        CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strSourceFolder))

        Return True
    End Function

    Public Shared Function UseDocTypeDeclarationFileForImport(ByVal strSourceFolder As String) As Boolean
        Select Case CheckDocTypeDeclarationFile(strSourceFolder)
            Case EDtdFileExist.EqualButDifferent
                CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strSourceFolder))

            Case EDtdFileExist.SourceNotFound
                MsgBox("The resource " + GetDtdResource() + " is missing. " + _
                       vbCrLf + "Please retry installation to replace missing files. ", MsgBoxStyle.Critical, "DTD resources")
                Return False

            Case EDtdFileExist.Equal
                ' Nothing to do

            Case EDtdFileExist.MoreRecent
                If MsgBox("File '" + GetDestinationDtdFile(strSourceFolder) + vbCrLf + " is more recent than application resource. " + vbCrLf + _
                       "Maybe this import would corrupt current project. Please confirm importation.", _
                       cstMsgYesNoExclamation, strSourceFolder) = MsgBoxResult.No _
                Then
                    Return False
                End If

            Case EDtdFileExist.NotFound
                If MsgBox("Can't find File '" + GetDestinationDtdFile(strSourceFolder) + vbCrLf + _
                          "An application resource file will be copy in this folder to replace it. " + vbCrLf + _
                       "Maybe this import would be incompatible with this resource file. Please confirm replace.", _
                       cstMsgYesNoExclamation, strSourceFolder) = MsgBoxResult.No _
                Then
                    Return False
                Else
                    CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strSourceFolder))
                End If

            Case EDtdFileExist.Older
                If MsgBox("File '" + GetDestinationDtdFile(strSourceFolder) + vbCrLf + " is more older than application resource. " + vbCrLf + _
                       "Maybe this import would corrupt current project. Please confirm importation.", _
                       cstMsgYesNoExclamation, strSourceFolder) = MsgBoxResult.No _
                Then
                    Return False
                End If

            Case Else
                Throw New Exception("Method 'CheckDocTypeDeclarationFile' has returned a wrong value")
        End Select
        Return True
    End Function

    Public Shared Function CopyRessourcesInUserPath(ByVal strDestinationFolder As String) As Boolean
        Try
            If Not CopyDocTypeDeclarationFile(strDestinationFolder, True) Then
                Return False
            ElseIf Not CopyLanguagePrefix(strDestinationFolder) Then
                Return False
            ElseIf Not CopyLanguageSimpleTypes(strDestinationFolder, ELanguage.Language_CplusPlus) Then
                Return False
            ElseIf Not CopyLanguageSimpleTypes(strDestinationFolder, ELanguage.Language_Vbasic) Then
                Return False
            ElseIf Not CopyLanguageSimpleTypes(strDestinationFolder, ELanguage.Language_Java) Then
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Public Shared Function CopyDocTypeDeclarationFile(ByVal strDestinationFolder As String, Optional ByVal bNoAdvertising As Boolean = False) As Boolean
        ' With Xml Schema, we won't save document structure file into project folder.
        Dim bResult As Boolean = False
        Try
            Select Case CheckDocTypeDeclarationFile(strDestinationFolder)
                Case EDtdFileExist.EqualButDifferent
                    CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strDestinationFolder))
                    bResult = True

                Case EDtdFileExist.SourceNotFound
                    MsgBox("The resource " + GetDtdResource() + " is missing. " + _
                           vbCrLf + "Please retry installation to replace missing files. ", MsgBoxStyle.Critical, "DTD resources")
                    Return False

                Case EDtdFileExist.NotFound
                    CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strDestinationFolder))
                    bResult = True

                Case EDtdFileExist.MoreRecent
                    If bNoAdvertising _
                    Then
                        CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strDestinationFolder))
                        bResult = True

                    ElseIf MsgBox("File '" + GetDestinationDtdFile(strDestinationFolder) + "'," + vbCrLf + "is more recent than application resource." + vbCrLf + _
                              "Please confirm overwrite this file ?", _
                               cstMsgOkCancelCritical, strDestinationFolder) _
                                = MsgBoxResult.Ok _
                    Then
                        CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strDestinationFolder))
                        bResult = True
                    End If

                Case EDtdFileExist.Older
                    If bNoAdvertising _
                    Then
                        CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strDestinationFolder))
                        bResult = True

                    ElseIf MsgWarningBox("File '" + GetDestinationDtdFile(strDestinationFolder) + "'," + vbCrLf + "is more older than application resource. " + vbCrLf + _
                        "Maybe oldest version projects remain in this folder. Overwrite this file would corrupt these projects." + vbCrLf + _
                        "Also, we recommend you to choose an other folder and press Cancel now. !") _
                        = DialogResult.OK _
                    Then
                        CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strDestinationFolder))
                        bResult = True
                    End If

                Case EDtdFileExist.Equal
                    bResult = True

                Case Else
                    Throw New Exception("method 'CheckDocTypeDeclarationFile' has returned a wrong value")
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Sub UpdatesCollaborations(ByVal document As XmlDocument)
        For Each node As XmlNode In document.SelectNodes("//class | //interface | //reference")
            UpdateOneCollaboration(document, GetID(node))
        Next
    End Sub

    Public Shared Sub UpdateOneCollaboration(ByVal document As XmlDocument, ByVal strID As String)
        Dim strQuery As String
        Dim list As XmlNodeList
        Dim collaboration As XmlNode

        Dim node As XmlNode = document.SelectSingleNode("(//class | //interface | //reference)[@id='" + strID + "']")

        strQuery = "collaboration"
        list = SelectNodes(node, strQuery)

        For Each child As XmlNode In list
            node.RemoveChild(child)
        Next child

        strQuery = "//relationship[*/@idref='" + GetID(node) + "']"
        list = SelectNodes(node, strQuery)

        For Each child As XmlNode In list
            strID = GetID(child)
            collaboration = CreateAppendCollaboration(node)
            AddAttributeValue(collaboration, "idref", strID)
        Next
    End Sub

    Public Shared Sub ExportOmgUmlFile(ByVal form As Form, ByVal document As XmlDocument, ByVal fileName As String)
        Dim oldCursor As Cursor = form.Cursor
        Dim observer As InterfProgression = CType(form, InterfProgression)

        Try
            form.Cursor = Cursors.WaitCursor

            observer.Minimum = 0
            observer.Maximum = 4
            observer.ProgressBarVisible = True

            Dim styleXsl As New XslSimpleTransform(True)
            styleXsl.Load(My.Computer.FileSystem.CombinePath(Application.StartupPath, _
                                                                     My.Settings.ToolsFolder + cstProject2XmiStyle))

            observer.Increment(2)

            Dim argList As New XslSimpleTransform.Arguments
            argList.Add("LanguageFolder", Application.LocalUserAppDataPath.ToString)

            styleXsl.Transform(document.DocumentElement, fileName, argList)
            observer.Increment(2)

        Catch ex As Exception
            Throw ex
        Finally
            form.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try
    End Sub

    Public Shared Function ConvertOmgUmlModel(ByVal form As Form, ByVal strFilename As String, _
                                              ByRef strTempFile As String, Optional ByVal iMode As Integer = 1) As Boolean
        Dim oldCursor As Cursor = form.Cursor
        Dim observer As InterfProgression = CType(form, InterfProgression)
        Dim bResult As Boolean = False
        strTempFile = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, _
                                                         cstTempUmlFile + ".xprj")
        Try
            form.Cursor = Cursors.WaitCursor

            observer.Minimum = 0
            observer.Maximum = 12
            observer.ProgressBarVisible = True

            Dim styleXsl As New XslSimpleTransform(True)
            Select Case iMode
                Case 0
                    styleXsl.Load(My.Computer.FileSystem.CombinePath(Application.StartupPath, _
                                                                     My.Settings.ToolsFolder + cstXmi2ProjectStyle))
                Case Else
                    styleXsl.Load(My.Computer.FileSystem.CombinePath(Application.StartupPath, _
                                                                     My.Settings.ToolsFolder + cstIbmXmi2ProjectStyle))
            End Select
            observer.Increment(1)
            Dim argList As New XslSimpleTransform.Arguments
            argList.Add("FolderDTD", Application.LocalUserAppDataPath.ToString + Path.DirectorySeparatorChar)

            ' This transformation generates a metafile 85% compliant with end-generated file
            styleXsl.Transform(strFilename, strTempFile, argList)
            observer.Increment(1)

        Catch ex As Exception
            Throw ex
        Finally
            form.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try

        Dim stage As String = "Format final project to UML Designer"
        Dim document As New XmlDocument

        Try
            observer.ProgressBarVisible = True
            LoadDocument(form, document, strTempFile, True)
            observer.Increment(1)

        Catch ex As Exception
            Throw New Exception("Fails to complete conversion during stage '" + stage + "', with temporary file:" + vbCrLf + strTempFile, ex)
        Finally
            observer.ProgressBarVisible = False
        End Try

        Try
            form.Cursor = Cursors.WaitCursor
            observer.ProgressBarVisible = True

            stage = "Merge iterator with container"
            MergeIteratorContainer(document)
            observer.Increment(1)

            stage = "Find template class"
            FindTemplateClasses(document, ELanguage.Language_CplusPlus)
            observer.Increment(1)

            ' Temporary saving to check result before renumber
            document.Save(strTempFile)
            observer.Increment(1)

            stage = "Apply UML Designer element indexation"
            RenumberProject(document.DocumentElement, True)
            observer.Increment(1)

            stage = "Remove prefix in properties"
            CleanPrefixProperties(document)
            observer.Increment(1)

            stage = "Merge properties and accessors"
            Dim strGetter As String = GetPrefixName("GetName")
            Dim strSetter As String = GetPrefixName("SetName")
            MergeAccessorProperties(document, strGetter, strSetter)
            observer.Increment(1)

            stage = "Update nodes collabration"
            UpdatesCollaborations(document)
            observer.Increment(1)

            stage = "Final saving"
            document.Save(strTempFile)
            observer.Increment(1)

        Catch ex As Exception
            Throw New Exception("Fails to complete conversion during stage '" + stage + "', with temporary file:" + vbCrLf + strTempFile, ex)
        Finally
            form.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try

        Try
            observer.ProgressBarVisible = True
            stage = "Reload to check DTD"
            LoadDocument(form, document, strTempFile, True)
            observer.Increment(1)
            bResult = True

        Catch ex As Exception
            Throw New Exception("Fails to complete conversion during stage '" + stage + "', with temporary file:" + vbCrLf + strTempFile, ex)
        Finally
            observer.ProgressBarVisible = False
        End Try
        Return bResult
    End Function

    Public Shared Function ConvertDoxygenIndexFile(ByVal form As Form, ByVal strFilename As String, _
                                                   ByRef strTempFile As String) As Boolean
        Dim eLanguage As ELanguage

        If MsgBox("Do you want to convert file to C++ language (Yes) or Java (No) ?", cstMsgYesNoQuestion, "Doxygen Tag file import") = MsgBoxResult.Yes Then
            eLanguage = eLanguage.Language_CplusPlus
        Else
            eLanguage = eLanguage.Language_Java
        End If

        Dim observer As InterfProgression = CType(form, InterfProgression)
        Dim bResult As Boolean = False
        Dim oldCursor As Cursor = form.Cursor

        strTempFile = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, _
                                                         cstTempDoxygenFile + ".xprj")
        Try
            form.Cursor = Cursors.WaitCursor

            observer.Minimum = 0
            observer.Maximum = 18
            observer.ProgressBarVisible = True

            Dim styleXsl As New XslSimpleTransform(True)
            styleXsl.Load(My.Computer.FileSystem.CombinePath(Application.StartupPath, _
                                                             My.Settings.ToolsFolder + cstDoxygen2ProjectStyle))
            observer.Increment(1)

            Dim argList As New XslSimpleTransform.Arguments
            argList.Add("DoxFolder", GetProjectPath(strFilename) + Path.DirectorySeparatorChar.ToString)

            ' This transformation generates a metafile 80% compliant with end-generated file
            styleXsl.Transform(strFilename, strTempFile, argList)
            observer.Increment(1)

        Catch ex As Exception
            Throw ex
        Finally
            form.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try

        Dim stage As String = "Format final project to UML Designer"
        Dim document As New XmlDocument

        Try
            observer.ProgressBarVisible = True
            LoadDocument(form, document, strTempFile, True)
            observer.Increment(1)

        Catch ex As Exception
            Throw New Exception("Fails to complete conversion during stage '" + stage + "', with temporary file:" + vbCrLf + strTempFile, ex)
        Finally
            observer.ProgressBarVisible = False
        End Try

        Try
            observer.ProgressBarVisible = True
            form.Cursor = Cursors.WaitCursor

            ' Some doxygen types reference in "ref" element, the refid references the relationship child class and not the container type
            stage = "Change Doxygen type references"
            ChangeDoxygenTypeReferences(document)
            observer.Increment(1)

            ' Some doxygen types remain as simple declaration, we must translate them to UML design Xml elements
            stage = "Rename simple " + eLanguage.ToString + " declaration"
            RenameTypeDoxygenResultFile(eLanguage, document)
            observer.Increment(1)


            stage = "Merge iterator with container"
            MergeIteratorContainer(document)
            observer.Increment(1)

            stage = "Find template class"
            FindTemplateClasses(document, eLanguage)
            observer.Increment(1)

            ' Temporary saving to check result before renumber
            document.Save(strTempFile)
            observer.Increment(1)

            ' We insert the external DTD file declaration to be sure that generated file is full compliant
            stage = "Insert the external DTD file declaration"
            Dim docType As XmlDocumentType = document.CreateDocumentType("root", Nothing, "class-model.dtd", "")
            observer.Increment(1)
            document.InsertBefore(docType, document.FirstChild.NextSibling)

            stage = "Apply UML Designer element indexation"
            RenumberProject(document.DocumentElement)
            observer.Increment(1)

            ' Temporary saving to check result before renumber
            document.Save(strTempFile)
            observer.Increment(1)

            ' Remove prefix in properties , accessors, convert doxygen comments
            stage = "Remove prefix in properties"
            CleanPrefixProperties(document)
            observer.Increment(1)

            stage = "Merge properties and accessors"
            Dim strGetter As String = GetPrefixName("GetName")
            Dim strSetter As String = GetPrefixName("SetName")
            MergeAccessorProperties(document, strGetter, strSetter)
            observer.Increment(1)

            stage = "update Doxygen comments"
            ConvertDoxygenComments(document)
            observer.Increment(1)

            ' Find collaboration
            stage = "Find collaborations"
            FindCollaborations(document)
            observer.Increment(1)

            ' Find overrided methods
            stage = "Find overrided methods"
            FindOverridedMethods(document)
            observer.Increment(1)

            ' Now document is no more "Standalone"
            stage = "Final saving"
            CType(document.FirstChild, XmlDeclaration).Standalone = "no"
            document.Save(strTempFile)
            observer.Increment(1)

        Catch ex As Exception
            Throw New Exception("Fails to complete conversion during stage '" + stage + "', with temporary file:" + vbCrLf + strTempFile, ex)
        Finally
            form.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try

        Try
            observer.ProgressBarVisible = True
            stage = "Reload to check DTD"
            LoadDocument(form, document, strTempFile, True)
            observer.Increment(1)
            bResult = True

        Catch ex As Exception
            Throw New Exception("Fails to complete conversion during stage '" + stage + "', with temporary file:" + vbCrLf + strTempFile, ex)
        Finally
            observer.ProgressBarVisible = False
        End Try
        Return bResult
    End Function

    Public Shared Function ApplyPatch(ByVal form As Form, ByVal strOldFile As String, ByRef strNewFile As String) As Boolean
        Dim oldCursor As Cursor = form.Cursor
        Try
            strNewFile = ""
            Dim dlgOpenFile As New OpenFileDialog
            dlgOpenFile.InitialDirectory = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            dlgOpenFile.Title = "Select a patch file..."
            dlgOpenFile.Filter = "UML  patch (*.xptch)|*.xptch|XSL transformation patch (*.xsl*)|*.xsl*"

            If (dlgOpenFile.ShowDialog() = DialogResult.OK) _
            Then
                form.Cursor = Cursors.WaitCursor

                Dim FilePatch As String = dlgOpenFile.FileName
                strNewFile = strOldFile + ".new.xprj"
                ApplyPatchFile(strOldFile, strNewFile, FilePatch)

                ' Using avoid to remain the file locked for another process
                ' the following to Validate succeeds.
                Dim settings As XmlReaderSettings = New XmlReaderSettings()
                settings.ProhibitDtd = False
                settings.ValidationType = System.Xml.ValidationType.DTD

                Dim document As New XmlDocument
                Using reader As XmlReader = XmlReader.Create(strNewFile, settings)
                    document.Load(reader)
                End Using

                form.Cursor = oldCursor
                MsgBox("Please find result of conversion in file:" + vbCrLf + strNewFile, MsgBoxStyle.Exclamation, "File converted")
                Return True
            End If

        Catch ex As Exception
            form.Cursor = oldCursor
            MsgExceptionBox(ex)
        End Try
        Return False
    End Function

    Public Shared Sub ConvertDoxygenTagFile(ByVal form As Form, ByVal document As XmlDocument, _
                                            ByVal strFilename As String, ByVal eLanguage As ELanguage)
        Dim observer As InterfProgression = CType(form, InterfProgression)
        Dim oldCursor As Cursor = form.Cursor
        Dim strTempFile As String = ""

        Try
            form.Cursor = Cursors.WaitCursor

            observer.Minimum = 0
            observer.Maximum = 14
            observer.ProgressBarVisible = True

            Dim styleXsl As New XslSimpleTransform(True)
            styleXsl.Load(My.Computer.FileSystem.CombinePath(Application.StartupPath, _
                                                             My.Settings.ToolsFolder + cstTag2ImportStyle))
            observer.Increment(2)
            form.Cursor = oldCursor

            strTempFile = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, _
                                                                           cstTempDoxygenFile + ".ximp")

            Dim argList As New XslSimpleTransform.Arguments

            argList.Add("Language", CInt(eLanguage).ToString)

            ' This transformation generates a metafile 80% compliant with end-generated file
            styleXsl.Transform(strFilename, strTempFile)
            observer.Increment(2)

        Catch ex As Exception
            Throw ex
        Finally
            form.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try

        Dim stage As String = "Format final project to UML Designer"

        Try
            observer.ProgressBarVisible = True
            LoadDocument(form, document, strTempFile, True)
            observer.Increment(2)

            ' Some doxygen types remain as simple declaration, we must translate them to UML design Xml elements
            stage = "Rename simple " + ELanguage.ToString + " declaration"
            RenameTypeDoxygenTagFile(ELanguage, document)
            observer.Increment(2)

            stage = "Merge properties and accessors"
            Dim strGetter As String = GetPrefixName("GetName")
            Dim strSetter As String = GetPrefixName("SetName")
            MergeAccessorProperties(document, strGetter, strSetter, True)
            observer.Increment(2)

            Dim tempo As String = Path.GetFileNameWithoutExtension(strFilename)
            AddAttributeValue(document.DocumentElement, "name", tempo)

            ' We insert the external DTD file declaration to be sure that generated file is full compliant
            stage = "Insert the external DTD file declaration"
            Dim docType As XmlDocumentType = document.CreateDocumentType("export", Nothing, "class-model.dtd", "")
            observer.Increment(2)
            document.InsertBefore(docType, document.FirstChild.NextSibling)

            stage = "Final saving"
            ' Now document is no more "Standalone"
            CType(document.FirstChild, XmlDeclaration).Standalone = "no"
            document.Save(strTempFile)
            observer.Increment(2)

        Catch ex As Exception
            Throw New Exception("Fails to complete conversion during stage '" + stage + "', with temporary file:" + vbCrLf + strTempFile, ex)
        Finally
            form.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try

        Try
            observer.ProgressBarVisible = True
            stage = "Reload to check DTD"
            LoadDocument(form, document, strTempFile, True)

        Catch ex As Exception
            Throw New Exception("Fails to complete conversion during stage '" + stage + "', with temporary file:" + vbCrLf + strTempFile, ex)
        Finally
            form.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try
    End Sub

    Public Shared Function LoadDocument(ByVal document As XmlDocument, ByVal strFilename As String) As EResult
        Try
            document.Load(strFilename)

        Catch ex As Exception
            Throw ex
        End Try
        Return XmlProjectTools.EResult.Completed
    End Function

    Public Shared Function LoadDocument(ByVal form As Form, ByVal document As XmlDocument, ByVal strFilename As String, _
                                        Optional ByVal bThrowsException As Boolean = False, _
                                        Optional ByVal bWithXsdValidation As Boolean = False) As EResult

        Dim oldCursor As Cursor = form.Cursor
        Dim eResult As EResult = eResult.Failed
        Dim bDtdError As Boolean = False
        Dim currentException As Exception = Nothing
        Try
            ' TODO replace optional argument bWithXsdValidation with true when XmlSchema validation works with ID/IDREF
            If bWithXsdValidation Then
                ' With Xml Schema, we don't save document structure file into project folder.
                form.Cursor = Cursors.WaitCursor
                document.Load(strFilename)
                document.Schemas.Add("http://www.classxmlproject.com", _
                                     My.Computer.FileSystem.CombinePath(Application.StartupPath, _
                                                                        My.Settings.ToolsFolder + cstSchemaName + ".xsd"))
                ' the following to Validate succeeds.
                document.Validate(Nothing)
            Else
                ' the following to Validate succeeds.
                Dim settings As XmlReaderSettings = New XmlReaderSettings()
                settings.ProhibitDtd = False
                settings.ValidationType = System.Xml.ValidationType.DTD

                ' Using avoid to remain the file locked for another process
                form.Cursor = Cursors.WaitCursor
                Using reader As XmlReader = XmlReader.Create(strFilename, settings)
                    Try
                        document.Load(reader)
			            eResult = XmlProjectTools.EResult.Completed

                    Catch ex As XmlSchemaException
                        eResult = XmlProjectTools.EResult.Failed
                        bDtdError = True
                        currentException = New Exception( _
                                             "Reader=" + reader.Name + "-" + reader.Value + "(" + reader.ValueType.ToString + ")" + vbCrLf + _
                                             "LineNumber=" + ex.LineNumber.ToString + vbCrLf + _
                                            "LinePosition=" + ex.LinePosition.ToString + vbCrLf + _
                                            "Message=" + ex.Message + vbCrLf + _
                                            "SourceUri=" + ex.SourceUri, _
                                            ex)

                        If bThrowsException Then Throw currentException

                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End If

        Catch ex As Exception
            Throw ex
        Finally
            form.Cursor = oldCursor
        End Try

        If bDtdError Then
            eResult = ConvertAndCorrectErrors(form, document, strFilename, bDtdError, currentException)
        End If
        Return eResult
    End Function

    Public Shared Function CreateAppendNode(ByVal node As XmlNode, ByVal szElement As String, Optional ByVal bInsertLf As Boolean = True) As XmlNode
        Dim xmlresult As XmlNode = node.OwnerDocument.CreateNode(XmlNodeType.Element, szElement, "")
        node.AppendChild(xmlresult)
        Return xmlresult
    End Function

    Public Shared Sub RemoveAttribute(ByRef node As XmlNode, ByVal strAttribute As String)
        Try
            Dim attrib As XmlAttribute = node.Attributes.ItemOf(strAttribute)

            If attrib IsNot Nothing Then
                attrib = node.OwnerDocument.CreateAttribute(strAttribute)
                node.Attributes.RemoveNamedItem(strAttribute)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function CanRemoveOverridedProperty(ByVal parentNode As XmlComposite, ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = True
        Try
            Dim strQuery As String = "//property[@name='" + removeNode.GetAttribute("name") + "' and @overrides='" + GetID(parentNode.Node) + "']"
            If parentNode.SelectNodes(strQuery).Count > 0 Then
                MsgBox("Sorry but this property is overrided", MsgBoxStyle.Critical, "'Remove' command")
                bResult = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Function CanRemoveOverridedMethod(ByVal parentNode As XmlComposite, ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = True
        Try
            Dim strQuery As String = "//method[@name='" + removeNode.GetAttribute("name") + "' and @overrides='" + GetID(parentNode.Node) + "']"
            If parentNode.SelectNodes(strQuery).Count > 0 Then
                MsgBox("Sorry but this method is overrided", MsgBoxStyle.Critical, "'Remove' command")
                bResult = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Function RemoveInheritedProperties(ByVal parentNode As XmlComposite, ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim listID As New List(Of String)
            LoadTreeInherited(removeNode.Node, listID)

            For Each strId As String In listID
                Dim listProperty As XmlNodeList = parentNode.SelectNodes("property[@overrides='" + strId + "']")
                For Each method As XmlNode In listProperty
                    parentNode.Node.RemoveChild(method)
                    bResult = True
                Next
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return bResult
    End Function

    Public Shared Function RemoveInheritedMethods(ByVal parentNode As XmlComposite, ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim listID As New List(Of String)
            LoadTreeInherited(removeNode.Node, listID)

            For Each strId As String In listID
                Dim listMethod As XmlNodeList = parentNode.SelectNodes("method[@overrides='" + strId + "']")
                For Each method As XmlNode In listMethod
                    parentNode.Node.RemoveChild(method)
                    bResult = True
                Next
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return bResult
    End Function

    Public Shared Sub RemoveNode(ByRef node As XmlNode)
        node.ParentNode.RemoveChild(node)
    End Sub

    Public Shared Sub RemoveNode(ByRef parent As XmlNode, ByVal xpath As String)
        For Each child As XmlNode In parent.SelectNodes(xpath)
            parent.RemoveChild(child)
        Next
    End Sub

    Public Shared Function SetID(ByRef node As XmlNode, ByVal strID As String, Optional ByVal strAttribute As String = "id") As XmlNode
        Dim xmlResult As XmlNode = node.SelectSingleNode("@" + strAttribute)
        xmlResult.Value = strID
        Return xmlResult
    End Function

    Public Shared Function GetFirstClassId(ByVal component As XmlComponent, Optional ByVal strExcludeId As String = "") As String
        If component.Document Is Nothing Then
            Throw New Exception("Property m_xmlDocument is null in component " + component.ToString())
        End If

        Dim current As XmlNode
        Dim strResult As String = ""
        Dim strQuery As String = "//class | //interface | //reference[@type='class'][(not(@container) or @container='0')]"
        If strExcludeId <> "" Then
            strQuery = "//class[@id!='" + strExcludeId + "'] | //interface[@id!='" + strExcludeId + "']" + _
                        " | //reference[@id!='" + strExcludeId + "'][@type='class'][(not(@container) or @container='0')]"
        End If
        current = component.Document.SelectSingleNode(strQuery)
        If current IsNot Nothing Then
            strResult = GetID(current)
        End If
        Return strResult
    End Function

    Public Shared Function GetFather(ByVal node As XmlNode) As XmlNode
        Try
            Return GetNode(node, "father")

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetMasterNode(ByVal current As XmlNode) As XmlNode
        Dim xmlResult As XmlNode = current
        Select Case current.Name
            Case "list", "array"
                xmlResult = xmlResult.ParentNode
                xmlResult = xmlResult.ParentNode

            Case "collaboration", "dependency", "inherited", "father", "child"
                xmlResult = xmlResult.ParentNode

            Case "type"
                xmlResult = xmlResult.ParentNode
                If xmlResult.Name = "return" Or xmlResult.Name = "param" Then
                    xmlResult = xmlResult.ParentNode
                End If
        End Select
        Return xmlResult
    End Function

    Public Shared Function GetID(ByVal node As XmlNode, Optional ByVal strID As String = "id") As String
        Try
            Return GetAttributeValue(node, strID)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetCurrentIDREF(ByVal iterator As IEnumerator, Optional ByVal strIdref As String = "idref") As String
        Try
            Return GetAttributeValue(CType(iterator.Current, XmlNode), strIdref)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetIDREF(ByVal node As XmlNode, Optional ByVal strIdref As String = "idref") As String
        Try
            Return GetAttributeValue(node, strIdref)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function SelectNodes(ByVal node As XmlNode, ByVal strElement As String, Optional ByVal strExcludeId As String = "", Optional ByVal strSubElement As String = "") As XmlNodeList
        Try
            Dim strQuery As String

            If Len(strExcludeId) > 0 Then
                strQuery = strElement + "[@id!='" + strExcludeId + "']" + strSubElement
            Else
                strQuery = strElement
            End If
            Return node.SelectNodes(strQuery)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Sub SetNodeString(ByRef node As XmlNode, ByVal strValue As String)
        Try
            If strValue IsNot Nothing Then
                node.InnerText = strValue
            Else
                node.InnerText = ""
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function GetNodeString(ByRef node As XmlNode, ByVal strElement As String, Optional ByVal strDefault As String = "") As String
        Dim strNodeString As String = Nothing
        Try
            Dim child As XmlNode

            If node Is Nothing _
            Then
                strNodeString = strDefault
            Else
                strNodeString = strDefault

                child = GetNode(node, strElement)
                If Not child Is Nothing Then
                    strNodeString = child.InnerText
                End If
            End If
            Return strNodeString

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetPackage(ByVal node As XmlNode) As String
        Select Case node.Name
            Case "reference", "interface"
                Return GetAttributeValue(node, "package")

            Case "typedef"
                Return GetName(node.ParentNode.ParentNode)
        End Select
        Return GetName(node.ParentNode)
    End Function

    Public Shared Function GetNextClassId(ByVal node As XmlNode) As Integer

        Dim num_id As Integer
        Dim tempo As String
        Dim list As XmlNodeList
        Dim child As XmlNode

        tempo = XmlNodeCounter.AfterStr(GetNodeString(node, "@id"), "class")
        If IsNumeric(tempo) Then
            GetNextClassId = CInt(tempo)
        Else
            GetNextClassId = 0
        End If

        list = SelectNodes(node, "descendant::*/@id")

        For Each child In list
            tempo = XmlNodeCounter.AfterStr(child.Value, "class")
            If IsNumeric(tempo) Then
                num_id = CInt(tempo)
            Else
                num_id = 0
            End If

            If num_id > GetNextClassId Then
                GetNextClassId = num_id
            End If
        Next child

        GetNextClassId = GetNextClassId + 1

    End Function

    Public Shared Function GetCurrentName(ByVal iterator As IEnumerator) As String
        Return GetAttributeValue(CType(iterator.Current, XmlNode), "name")
    End Function

    Public Shared Function SelectNodeId(ByVal nodeXML As XmlNode, Optional ByVal TreeNodeXML As XmlNode = Nothing, Optional ByVal strIdref As String = "@idref") As XmlNode
        Try
            Dim child As XmlNode

            If TreeNodeXML Is Nothing Then
                TreeNodeXML = nodeXML
            End If

            child = GetNode(nodeXML, strIdref)

            If Not child Is Nothing Then
                Return SelectNodeStringId(TreeNodeXML, child.Value)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return Nothing
    End Function

    Public Shared Function SelectNodeStringId(ByVal TreeNodeXML As XmlNode, ByVal strID As String) As XmlNode
        Try
            Return TreeNodeXML.OwnerDocument.GetElementById(strID)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function ConvertDtdToEnumImpl(ByVal strImpl As String) As EImplementation
        Select Case strImpl
            Case ""
                Return EImplementation.Unknown
            Case "abstract"
                Return EImplementation.Interf
            Case "final"
                Return EImplementation.Leaf
            Case "virtual"
                Return EImplementation.Node
            Case "root"
                Return EImplementation.Root
            Case "exception"
                Return EImplementation.Exception
            Case "container"
                Return EImplementation.Container
            Case Else
                Return EImplementation.Simple
        End Select
    End Function

    Public Shared Function ConvertEnumImplToView(ByVal eImplmt As EImplementation, Optional ByVal bMethod As Boolean = False) As String
        Select Case eImplmt
            Case EImplementation.Interf
                If bMethod Then Return "abstract"
                Return "interface"
            Case EImplementation.Leaf
                If bMethod Then Return "final"
                Return "leaf"
            Case EImplementation.Root
                If bMethod Then Return "overridable"
                Return "root"
            Case EImplementation.Node
                If bMethod Then Return "overrides"
                Return "node"
            Case EImplementation.Exception
                Return "exception"
            Case EImplementation.Container
                Return "container"
            Case EImplementation.Unknown
                Return ""
            Case Else
                Return "simple"
        End Select
    End Function

    Public Shared Function ConvertViewToEnumImpl(ByVal strImpl As String) As EImplementation
        If strImpl Is Nothing Then
            'Throw New Exception("Argument can't be null")
            Return EImplementation.Simple
        End If
        Dim eResult As EImplementation
        Select Case strImpl
            Case "abstract"
                eResult = EImplementation.Interf
            Case "interface"
                eResult = EImplementation.Interf
            Case "final"
                eResult = EImplementation.Leaf
            Case "leaf"
                eResult = EImplementation.Leaf
            Case "overridable"
                eResult = EImplementation.Root
            Case "root"
                eResult = EImplementation.Root
            Case "overrides"
                eResult = EImplementation.Node
            Case "node"
                eResult = EImplementation.Node
            Case "exception"
                eResult = EImplementation.Exception
            Case "container"
                eResult = EImplementation.Container
            Case Else
                eResult = EImplementation.Simple
        End Select
        Return eResult
    End Function

    Public Shared Function ConvertEnumImplToDtd(ByVal eImpl As EImplementation) As String
        Select Case eImpl
            Case EImplementation.Interf
                Return "abstract"
            Case EImplementation.Leaf
                Return "final"
            Case EImplementation.Root
                Return "root"
            Case EImplementation.Node
                Return "virtual"
            Case EImplementation.Exception
                Return "exception"
            Case EImplementation.Container
                Return "container"
            Case Else
                Return "simple"
        End Select
    End Function

    Public Shared Function GetLanguage(ByVal eTag As ELanguage) As String
        Select Case eTag
            Case ELanguage.Language_CplusPlus
                Return "C++"
            Case ELanguage.Language_Java
                Return "Java"
            Case ELanguage.Language_Vbasic
                Return "Vb.NET"
            Case ELanguage.Language_Tools
                Return "External style sheet"
            Case Else
                Return "Unknown"
        End Select
    End Function

    Public Shared Function GetSeparator(ByVal eTag As ELanguage) As String
        If eTag <> ELanguage.Language_CplusPlus Then
            Return "."
        Else
            Return "::"
        End If
    End Function

    Public Shared Function GetEndContainer(ByVal eTag As ELanguage) As String
        If eTag <> ELanguage.Language_CplusPlus Then
            Return ")"
        Else
            Return ">"
        End If
    End Function

    Public Shared Function GetBeginContainer(ByVal eTag As ELanguage) As String
        If eTag <> ELanguage.Language_CplusPlus Then
            Return "Container(Of "
        Else
            Return "container<"
        End If
    End Function

    Public Shared Function GetBeginStruct(ByVal eTag As ELanguage) As String
        If eTag <> ELanguage.Language_CplusPlus Then
            Return "Structure "
        Else
            Return "struct {"
        End If
    End Function

    Public Shared Function GetArrayString(ByVal eTag As ELanguage) As String
        If eTag <> ELanguage.Language_CplusPlus Then
            Return "()"
        Else
            Return "[]"
        End If
    End Function

    Public Shared Function GetBeginEnum(ByVal eTag As ELanguage) As String
        If eTag <> ELanguage.Language_CplusPlus Then
            Return "Enum "
        Else
            Return "enum {"
        End If
    End Function

    Public Shared Function GetEndStruct(ByVal eTag As ELanguage) As String
        If eTag <> ELanguage.Language_CplusPlus Then
            Return " "
        Else
            Return "}"
        End If
    End Function

    Public Shared Function GetFullpathPackage(ByVal current As XmlNode, ByVal eTag As ELanguage, Optional ByVal strPackage As String = "") As String
        Dim strResult As String = strPackage
        Try
            Dim strSeparator As String = GetSeparator(eTag)
            While current.ParentNode IsNot current.OwnerDocument
                If strResult <> "" Then
                    strResult = GetName(current.ParentNode) + strSeparator + strResult
                Else
                    strResult = GetName(current.ParentNode)
                End If
                current = current.ParentNode
            End While
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Shared Function GetFullpathDescription(ByVal current As XmlNode, ByVal eTag As ELanguage, _
                                                    Optional ByVal strCurrentPath As String = "") As String

        Dim strResult As String = ""
        Try
            Dim strSeparator As String
            If eTag <> ELanguage.Language_CplusPlus Then
                strSeparator = "."
            Else
                strSeparator = "::"
            End If

            If current IsNot Nothing Then
                strResult = GetName(current)

                Select Case current.Name
                    Case "father", "child"
                        current = current.ParentNode
                        strResult = GetFullpathDescription(current, eTag)

                    Case "relationship"
                        strResult = "Relationship (" + GetAttributeValue(current, "action") + ")"

                    Case "enumvalue"
                        If current.SelectSingleNode("ancestor::typedef") Is Nothing _
                        Then
                            current = current.SelectSingleNode("parent::reference")
                        Else
                            current = current.SelectSingleNode("ancestor::typedef")
                        End If
                        strResult = GetFullpathDescription(current, eTag) + strSeparator + strResult

                    Case "list"
                        current = current.ParentNode

                        Select Case current.Name
                            Case "father", "child"
                                current = current.ParentNode
                                strResult = GetFullpathDescription(current, eTag)

                            Case Else
                                strResult = GetFullpathDescription(current, eTag)
                        End Select

                    Case "type", "variable"
                        current = current.ParentNode

                        Select Case current.Name
                            Case "return"
                                current = current.ParentNode
                                strResult = GetFullpathDescription(current, eTag)

                            Case "param"
                                strResult = GetName(current)
                                current = current.ParentNode
                                strResult = GetFullpathDescription(current, eTag) + "(" + strResult + ")"

                            Case Else
                                strResult = GetFullpathDescription(current, eTag)
                        End Select

                    Case "collaboration", "dependency", "inherited"
                        current = current.ParentNode
                        strResult = GetFullpathDescription(current, eTag)

                    Case "typedef"
                        current = current.ParentNode
                        strResult = GetFullpathDescription(current, eTag) + strSeparator + strResult

                    Case "reference", "interface"
                        If GetAttributeValue(current, "type") = "typedef" Then
                            If GetAttributeValue(current, "class") IsNot Nothing Then
                                strResult = GetAttributeValue(current, "class") + strSeparator + strResult
                            End If
                        End If
                        Dim strTempo As String = GetPackage(current)
                        If strTempo IsNot Nothing _
                        Then
                            strResult = strTempo + strSeparator + strResult
                        End If

                        current = current.SelectSingleNode("ancestor::import")

                        strTempo = GetAttributeValue(current, "param")
                        If strCurrentPath <> "" _
                        Then
                            If eTag = ELanguage.Language_CplusPlus Then
                                strResult = strResult + " (Include """ + strCurrentPath + """)"
                            Else
                                strResult = strCurrentPath + strSeparator + strResult
                            End If
                        ElseIf strTempo IsNot Nothing _
                        Then
                            If eTag = ELanguage.Language_CplusPlus Then
                                strResult = strResult + " (Include """ + strTempo + """)"
                            Else
                                strResult = strTempo + strSeparator + strResult
                            End If
                        End If

                    Case "model"
                        strResult = "Model " + GetName(current)

                    Case "param"
                        Dim tempo As String = GetName(current)
                        current = current.ParentNode
                        strResult = GetMethodName(current, eTag)
                        current = current.ParentNode
                        strResult = tempo + " in method " + GetFullpathDescription(current, eTag) + strSeparator + strResult

                    Case "method"
                        strResult = GetMethodName(current, eTag)
                        current = current.ParentNode
                        strResult = GetFullpathDescription(current, eTag) + strSeparator + strResult

                    Case Else
                        current = current.ParentNode
                        If current.Name <> "root" Then
                            strResult = GetName(current) + strSeparator + strResult
                        End If
                End Select
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Shared Function GetMethodName(ByVal current As XmlNode, ByVal eTag As ELanguage) As String
        If GetAttributeValue(current, "constructor") = "no" Then
            Return GetName(current)
        End If
        Return GetConstructorName(current, eTag)
    End Function

    Public Shared Function GetConstructorName(ByVal current As XmlNode, ByVal eTag As ELanguage) As String
        Select Case eTag
            Case ELanguage.Language_Vbasic
                Return "New"

            Case Else
                Return GetName(GetNode(current, "parent::class"))
        End Select
    End Function

    Public Shared Function GetSimpleTypesFilename(ByVal value As Integer, Optional ByVal strPath As String = "") As String
        If strPath = "" Then
            strPath = Application.LocalUserAppDataPath.ToString
        End If

        Dim strFilename As String
        Dim index As ELanguage = CType(value, ELanguage)

        Select Case index
            Case ELanguage.Language_Java
                strFilename = My.Computer.FileSystem.CombinePath(strPath, "LanguageJava.xml")
            Case ELanguage.Language_Vbasic
                strFilename = My.Computer.FileSystem.CombinePath(strPath, "LanguageVbasic.xml")
            Case Else
                strFilename = My.Computer.FileSystem.CombinePath(strPath, "LanguageCplusPlus.xml")
        End Select
        Return strFilename
    End Function

    Public Shared Function StringToCardinal(ByVal strCardinal As String) As ECardinal
        Dim eResult As ECardinal
        Select Case strCardinal
            Case "1"
                eResult = ECardinal.Fix
            Case "01"
                eResult = ECardinal.Variable
            Case "0n"
                eResult = ECardinal.EmptyList
            Case "1n"
                eResult = ECardinal.FullList
            Case Else
                eResult = ECardinal.Fix
        End Select
        Return eResult
    End Function

    Public Shared Function CardinalToString(ByVal eCardinal As ECardinal) As String
        Dim strResult As String
        Select Case eCardinal
            Case eCardinal.Fix
                strResult = "1"
            Case eCardinal.Variable
                strResult = "01"
            Case eCardinal.EmptyList
                strResult = "0n"
            Case eCardinal.FullList
                strResult = "1n"
            Case Else
                strResult = "1"
        End Select
        Return strResult
    End Function

    Public Shared Function DisplayLevel(ByVal level As Integer, ByVal eTag As ELanguage) As String

        If eTag <> ELanguage.Language_CplusPlus Then Return ""

        Dim strResult As String = ""
        Try
            Select Case level
                Case 1
                    strResult = "*"
                Case 2
                    strResult = "**"
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Shared Sub StartInsertion(ByVal import As XmlDocument, ByVal oRefCounter As XmlReferenceNodeCounter)
        Try
            Dim child As XmlNode
            Dim parent As XmlNode
            Dim strID As String
            'Dim numID As Integer
            Dim oImportCounter As New XmlReferenceNodeCounter(import)

            For Each child In SelectNodes(import, "//*[@id]")

                Select Case child.Name
                    Case "package"
                        strID = CStr(oRefCounter.GetNewPackageId(oImportCounter))
                        'Debug.Print("StartInsertion: renumber (" + GetID(child) + ") " + child.Name + "-->" + strID)
                        SetID(child, strID)

                    Case "export"
                        'Debug.Print("StartInsertion: no renumber (" + GetID(child) + ") " + child.Name)

                    Case "relationship"
                        strID = CStr(oRefCounter.GetNewRelationId(oImportCounter))
                        'Debug.Print("StartInsertion: renumber (" + GetID(child) + ") " + child.Name + "-->" + strID)
                        ChangeID(child, import, strID)

                    Case "enumvalue"
                        parent = GetNode(child, "parent::type/parent::*")
                        Dim strPrefix As String = ""

                        Select Case parent.Name
                            Case "typedef"
                                strPrefix += XmlNodeCounter.AfterStr(GetID(parent), "class") + "_"
                            Case "property"
                                strPrefix = GetNodeString(parent, "@num-id") + "_"
                                parent = GetNode(parent, "parent::class")
                                strPrefix = XmlNodeCounter.AfterStr(GetID(parent), "class") + "_" + strPrefix
                        End Select
                        strPrefix = "enum" + strPrefix
                        strID = XmlReferenceNodeCounter.GenerateNumericId(child.ParentNode, "enumvalue", strPrefix, "id")
                        ChangeID(child, import, strID)

                    Case Else
                        strID = CStr(oRefCounter.GetNewClassId(oImportCounter))
                        'Debug.Print("StartInsertion: renumber (" + GetID(child) + ") " + child.Name + "-->" + strID)
                        ChangeID(child, import, strID)
                End Select
            Next child
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function ChangeTypeDesc(ByVal nodeReference As XmlNode, ByVal szNewID As String) As Boolean
        Dim child As XmlNode = nodeReference.SelectSingleNode("type")
        If child IsNot Nothing Then
            RemoveAttribute(child, "desc")
            AddAttributeValue(child, "idref", szNewID)
            Return True
        End If
        Return False
    End Function

    Public Shared Function ChangeID(ByVal nodeReference As XmlNode, ByVal treeNode As XmlNode, ByVal szNewID As String) As Boolean
        Dim bResult As Boolean = False
        Dim list As XmlNodeList
        Dim attrib As XmlNode

        Dim strOldID As String = GetID(nodeReference)
        Dim strQuery As String
        If strOldID.StartsWith("class") Or strOldID.StartsWith("relation") Then
            strQuery = "//@idref[.='" + strOldID + "']"
            list = treeNode.SelectNodes(strQuery)
            For Each attrib In list
                'Debug.Print(attrib.OuterXml)
                attrib.Value = szNewID
                bResult = True
            Next attrib
        Else
            strQuery = "//@valref[.='" + strOldID + "']"
            list = treeNode.SelectNodes(strQuery)
            For Each attrib In list
                'Debug.Print(attrib.OuterXml)
                attrib.Value = szNewID
                bResult = True
            Next attrib

            strQuery = "//@sizeref[.='" + strOldID + "']"
            list = treeNode.SelectNodes(strQuery)
            For Each attrib In list
                'Debug.Print(attrib.OuterXml)
                attrib.Value = szNewID
                bResult = True
            Next attrib
        End If
        SetID(nodeReference, szNewID)
        Return bResult
    End Function

    Public Shared Sub ExtractExternalReferences(ByVal parent As XmlNode, ByVal export As XmlNode)
        Dim list As XmlNodeList = SelectNodes(export, "reference[@external='yes']")

        If list.Count > 0 Then
            Dim export2 As XmlNode = CreateNode(parent, "import")
            AddAttributeValue(export2, "name", "External" + export2.GetHashCode().ToString)
            AddAttributeValue(export2, "visibility", "package")

            parent.InsertBefore(export2, export.ParentNode)

            export2 = CreateAppendNode(export2, "export")
            AddAttributeValue(export2, "name", "External")

            For Each child As XmlNode In list
                ' Append node without cloning it, induce moving node.
                export2.AppendChild(child)
                AddAttributeValue(child, "external", "no")
            Next
        End If
    End Sub

    Public Shared Sub LoadTreeInherited(ByVal parent As XmlNode, ByVal list As List(Of String))
        Try
            Dim iteration As Integer = 0
            SelectInherited(iteration, SelectNodeId(parent), list)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function CreateNode(ByVal doc As XmlDocument, ByVal strElement As String) As XmlNode
        Return doc.CreateElement(strElement)
    End Function

    Public Shared Function CreateNode(ByVal treeNode As XmlNode, ByVal strElement As String) As XmlNode
        Return treeNode.OwnerDocument.CreateElement(strElement)
    End Function

    Public Shared Sub SelectInheritedMethods(ByRef iteration As Integer, ByVal eImplementation As EImplementation, _
                                             ByRef node As XmlNode, ByVal list As ArrayList)
        Try
            iteration += 1
            If iteration > cstMaxCircularReferences Then
                MsgBox("Inherited tree deepth is too big, or has circular references!", MsgBoxStyle.Critical, "Circular references")
                Return
            End If
            ' We add "final" methods to be sure to avoid adding method from "root" node
            For Each child In SelectNodes(node, "method[@constructor!='no' or @implementation='final' or @implementation='virtual' or @implementation='root' or @implementation='abstract']")
                'Debug.Print("virtual=" + GetName(child))
                Dim xmlcpnt As XmlOverrideMemberView = New XmlOverrideMemberView(child)
                xmlcpnt.CheckedView = True

                Dim bAddNode As Boolean = False

                Select Case eImplementation
                    Case eImplementation.Container, eImplementation.Exception, eImplementation.Simple
                        bAddNode = (xmlcpnt.Implementation = eImplementation.Interf)

                    Case Else
                        bAddNode = True
                End Select

                If list.Contains(xmlcpnt) = False And bAddNode Then
                    list.Add(xmlcpnt)
                End If
            Next child

            Select Case eImplementation
                Case eImplementation.Container, eImplementation.Exception, eImplementation.Simple
                    ' Ignore inheritance tree

                Case Else
                    For Each child In SelectNodes(node, "inherited")
                        Dim inherited As XmlNode = SelectNodeId(child, node)
                        'Debug.Print("inherited=" + GetName(inherited))
                        SelectInheritedMethods(iteration, eImplementation, inherited, list)
                    Next child
            End Select

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub SelectInheritedProperties(ByRef iteration As Integer, ByVal eImplementation As EImplementation, _
                                                ByRef node As XmlNode, ByVal list As ArrayList)
        Try
            iteration += 1
            If iteration > cstMaxCircularReferences Then
                MsgBox("Inherited tree deepth is too big, or has circular references!", MsgBoxStyle.Critical, "Circular references")
                Return
            End If
            For Each child As XmlNode In SelectNodes(node, "property[@overridable='yes' or @overrides!='']")
                'Debug.Print("virtual=" + GetName(child))
                Dim xmlcpnt As XmlOverrideMemberView = New XmlOverrideMemberView(child)
                xmlcpnt.CheckedView = True

                Dim bAddNode As Boolean = False

                Select Case eImplementation
                    Case eImplementation.Container, eImplementation.Exception, eImplementation.Simple
                        bAddNode = (xmlcpnt.Implementation = eImplementation.Interf)

                    Case Else
                        bAddNode = True
                End Select

                If list.Contains(xmlcpnt) = False And bAddNode Then
                    list.Add(xmlcpnt)
                End If
            Next child

            Select Case eImplementation
                Case eImplementation.Container, eImplementation.Exception, eImplementation.Simple
                    ' Ignore inheritance tree

                Case Else
                    For Each child As XmlNode In SelectNodes(node, "inherited")
                        Dim inherited As XmlNode = SelectNodeId(child, node)
                        'Debug.Print("inherited=" + GetName(inherited))
                        SelectInheritedProperties(iteration, eImplementation, inherited, list)
                    Next child
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function IsInvalidProjectName(ByRef dataControl As TextBox, ByVal provider As ErrorProvider, ByVal eLang As ELanguage, _
                                                Optional ByVal eAlignment As ErrorIconAlignment = ErrorIconAlignment.TopLeft) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim strErrorMsg As String = "No error"

            If eLang = ELanguage.Language_Vbasic _
            Then
                If regVbAndJavaPackage.IsMatch(dataControl.Text) = False _
                Then
                    strErrorMsg = "Must contains characters compliant with project name:" + vbCrLf + _
                                  "name1 or name1.name2"
                    bResult = True
                End If

            ElseIf eLang = ELanguage.Language_Java _
            Then
                bResult = False

            ElseIf regCppPackage.IsMatch(dataControl.Text) = False _
            Then
                strErrorMsg = "Must contains characters compliant with project name:" + vbCrLf + _
                                  "name1 or name1::name2"
                bResult = True
            End If

            If bResult = True Then
                dataControl.Select(0, dataControl.Text.Length)

                provider.SetIconPadding(dataControl, 0)
                provider.SetIconAlignment(dataControl, eAlignment)
                provider.SetError(dataControl, strErrorMsg)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Function IsInvalidPackageName(ByRef dataControl As TextBox, ByVal provider As ErrorProvider, _
                                                ByVal eLang As ELanguage, Optional ByVal bNoHeaderFile As Boolean = False, _
                                                Optional ByVal eAlignment As ErrorIconAlignment = ErrorIconAlignment.TopLeft) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim strErrorMsg As String = ""
            If dataControl.Text = "" Then
                ' Ignore

            ElseIf eLang = ELanguage.Language_Vbasic _
            Then
                If regVbAndJavaPackage.IsMatch(dataControl.Text) = False _
                Then
                    strErrorMsg = "Must contains characters compliant with namespace:" + vbCrLf + _
                                  "name1 or name1.name2"
                    bResult = True
                End If

            ElseIf eLang = ELanguage.Language_Java _
            Then
                If regVbAndJavaPackage.IsMatch(dataControl.Text) = False _
                Then
                    strErrorMsg = "Must contains characters compliant with package name:" + vbCrLf + _
                                  "name1 or name1.name2"
                    bResult = True
                End If

            ElseIf regCppHeader.IsMatch(dataControl.Text) = True _
            Then
                If bNoHeaderFile Then
                    If regCppPackage.IsMatch(dataControl.Text) = False Then
                        strErrorMsg = "Must contains characters compliant with namespace: name1 or name1::name2"
                        bResult = True
                    End If
                End If

            ElseIf regCppPackage.IsMatch(dataControl.Text) = False _
            Then
                If bNoHeaderFile Then
                    strErrorMsg = "Must contains characters compliant with namespace: name1 or name1::name2"
                Else
                    strErrorMsg = "Must contains characters compliant with namespace or header files:" + vbCrLf + _
                                  "name1 or name1::name2" + vbCrLf + _
                                  "name.h or name.hpp" + vbCrLf + _
                                  "include/name1/name2.h  or include\name1\name2.h"
                End If
                bResult = True
            End If

            If bResult = True Then
                dataControl.Select(0, dataControl.Text.Length)

                provider.SetIconPadding(dataControl, 0)
                provider.SetIconAlignment(dataControl, eAlignment)
                provider.SetError(dataControl, strErrorMsg)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Function IsInvalidVariableName(ByVal dataControl As XmlDataGridView, _
                                                 ByVal e As DataGridViewCellValidatingEventArgs, _
                                                 ByVal provider As ErrorProvider, _
                                                 Optional ByVal eAlignment As ErrorIconAlignment = ErrorIconAlignment.TopLeft, _
                                                 Optional ByVal strDataPropertyName As String = "Name") As Boolean
        Dim bResult As Boolean = False
        Try
            Dim strErrorMsg As String = ""
            Dim strFormattedValue As String = e.FormattedValue
            If strDataPropertyName <> dataControl.Columns(e.ColumnIndex).DataPropertyName _
            Then
                bResult = False

            ElseIf strFormattedValue.Length = 0 _
            Then
                bResult = True

            Else
                Dim component As XmlComponent = CType(dataControl.Rows(e.RowIndex).DataBoundItem, XmlComponent)
                Dim eLang As ELanguage = CType(component.Tag, ELanguage)

                Select Case component.NodeName
                    Case "method"
                        If component.GetAttribute("operator") IsNot Nothing Then
                            If regOperator.IsMatch(strFormattedValue) = False _
                            Then
                                If eLang = ELanguage.Language_Vbasic _
                                Then
                                    strErrorMsg = "Please enter an operator: " _
                                                + vbCrLf + "Unary: +, -, IsFalse, IsTrue, Not" + vbCrLf _
                                                + "Binary: +, -, *, /, \, " + Chr(38) + ", ^, >>, <<, =, <>, >, >=, <, <=, And, Like, Mod, Or, Xor" + vbCrLf _
                                                + "Conversion:  CType"
                                Else
                                    strErrorMsg = "Please enter an operator: +, ++, --, -, !, *, /, >>, <<, ==, =, !=, >, >=, <, <=, " + Chr(38) + ", " + Chr(38) + Chr(38) + ", %, |, ||, ^"
                                End If
                                bResult = True
                            End If
                        ElseIf regVariableName.IsMatch(strFormattedValue) = False _
                        Then
                            strErrorMsg = "Must contains characters compliant with variable, function, or class name"
                            bResult = True
                        End If

                    Case Else
                        If regVariableName.IsMatch(strFormattedValue) = False _
                        Then
                            strErrorMsg = "Must contains characters compliant with variable, function, or class name"
                            bResult = True
                        End If
                End Select
            End If

            If bResult = True Then
                provider.SetIconPadding(dataControl, 0)
                provider.SetIconAlignment(dataControl, eAlignment)
                provider.SetError(dataControl, strErrorMsg)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Function IsInvalidType(ByRef combo As ComboBox, ByVal provider As ErrorProvider, _
                                         Optional ByVal eAlignment As ErrorIconAlignment = ErrorIconAlignment.TopRight) As Boolean

        Dim bResult As Boolean = False
        Try
            If String.IsNullOrEmpty(combo.Text) = False _
            Then
                bResult = False

            ElseIf String.IsNullOrEmpty(combo.SelectedText) = False _
            Then
                bResult = False

            ElseIf combo.SelectedItem IsNot Nothing _
            Then
                bResult = False

            ElseIf combo.SelectedValue IsNot Nothing _
            Then
                bResult = False
            Else
                bResult = True
            End If

            If bResult = True Then
                combo.Select(0, combo.Text.Length)

                ' Set the ErrorProvider error with the text to display. 
                Dim errorMsg As String = "Data types can't be empty !"
                provider.SetIconPadding(combo, 0)
                provider.SetIconAlignment(combo, eAlignment)
                provider.SetError(combo, errorMsg)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Function IsInvalidOperator(ByRef name As TextBox, ByVal provider As ErrorProvider, _
                                             ByVal eLang As ELanguage, _
                                             Optional ByVal eAlignment As ErrorIconAlignment = ErrorIconAlignment.TopLeft) As Boolean
        Dim bResult As Boolean = False
        Try
            If name.Text.Length = 0 _
            Then
                bResult = True

            ElseIf regOperator.IsMatch(name.Text) = False _
            Then
                bResult = True
            End If

            If bResult = True Then
                name.Select(0, name.Text.Length)

                ' Set the ErrorProvider error with the text to display. 
                If eLang = ELanguage.Language_Vbasic _
                Then
                    Dim errorMsg As String = "Unary: +, -, IsFalse, IsTrue, Not" + vbCrLf _
                                            + "Binary: +, -, *, /, \, " + Chr(38) + ", ^, >>, <<, =, <>, >, >=, <, <=, And, Like, Mod, Or, Xor" + vbCrLf _
                                            + "Conversion:  CType"

                    provider.SetError(name, errorMsg)
                Else
                    Dim errorMsg As String = "+, ++, --, -, !, *, /, >>, <<, ==, =, !=, >, >=, <, <=, " + Chr(38) + ", " + Chr(38) + Chr(38) + ", %, |, ||, ^"

                    provider.SetError(name, errorMsg)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Function IsInvalidVariableName(ByRef name As TextBox, ByVal provider As ErrorProvider, _
                                                 Optional ByVal eAlignment As ErrorIconAlignment = ErrorIconAlignment.TopLeft) As Boolean
        Dim bResult As Boolean = False
        Try
            If name.Text.Length = 0 _
            Then
                bResult = True

            ElseIf regVariableName.IsMatch(name.Text) = False _
            Then
                bResult = True
            End If

            If bResult = True Then
                name.Select(0, name.Text.Length)

                ' Set the ErrorProvider error with the text to display. 
                provider.SetIconPadding(name, 0)
                provider.SetIconAlignment(name, eAlignment)
                provider.SetError(name, "Must contains characters compliant with variable, function, or class name")
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Function GetValidFilename(ByRef filename As String) As Boolean
        Dim bResult As Boolean = False
        Try

            If filename Is Nothing Then
                filename = "ValidFilename"
                Return True
            End If

            Dim index As Integer = InStr(filename, "/")
            If index > 0 Then
                bResult = True
                filename = filename.Substring(0, index - 1) + "-" + filename.Substring(index)
            End If

            index = InStr(filename, "\")
            If index > 0 Then
                bResult = True
                filename = filename.Substring(0, index - 1) + "-" + filename.Substring(index)
            End If

            ' Determines if there are bad characters in the name.
            For Each badChar As Char In System.IO.Path.GetInvalidPathChars
                index = InStr(filename, badChar)
                If index > 0 Then
                    bResult = True
                    filename = filename.Substring(0, index - 1) + "-" + filename.Substring(index)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try

        ' The name passes basic validation.
        Return bResult
    End Function

    Public Shared Function GetTypeRelation(ByVal node As XmlNode, ByVal eTag As ELanguage) As String
        Dim strResult As String = Nothing
        Try
            Dim parent As XmlNode
            Dim method As XmlNode
            Dim strSeparator As String = "::"
            If eTag <> ELanguage.Language_CplusPlus Then
                strSeparator = "."
            End If

            parent = node.ParentNode
            Select Case parent.Name
                Case "param"

                    method = parent.ParentNode
                    Dim classNode As XmlNode = method.ParentNode

                    If GetName(method) = "" Then
                        strResult = GetFullpathDescription(classNode, eTag) + strSeparator + "<Constructor>, argument " + GetName(method)
                    Else
                        strResult = GetFullpathDescription(classNode, eTag) + strSeparator + GetName(method) + ", argument " + GetName(parent)
                    End If

                Case "return"

                    method = parent.ParentNode
                    Dim classNode As XmlNode = method.ParentNode
                    strResult = GetFullpathDescription(classNode, eTag) + strSeparator + GetName(method) + ", return value"

                Case "property"

                    strResult = GetFullpathDescription(GetNode(parent, "ancestor::class"), eTag) + strSeparator + GetName(parent)

                Case Else

                    strResult = GetFullpathDescription(parent, eTag)

            End Select

        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Shared Function GetFatherRelation(ByVal father As XmlNode) As String
        Dim strResult As String = Nothing
        Try
            Dim parent As XmlNode
            Dim sibbling As XmlNode

            parent = father.ParentNode
            sibbling = SelectNodeId(GetNode(parent, "child"))
            strResult = "Relation '" + GetAttributeValue(parent, "action") + "' to " + GetName(sibbling)

        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Shared Function GetChildRelation(ByVal father As XmlNode) As String
        Dim strResult As String = Nothing
        Try
            Dim parent As XmlNode
            Dim sibbling As XmlNode

            parent = father.ParentNode
            sibbling = SelectNodeId(GetFather(parent))
            strResult = "Relation '" + GetAttributeValue(parent, "action") + "' to " + GetName(sibbling)

        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Shared Function GetName(ByVal node As XmlNode) As String
        Return GetAttributeValue(node, "name")
    End Function

    Public Shared Function GetAttributeValue(ByVal nodeXml As XmlNode, ByVal name As String) As String
        Dim strResult As String = Nothing
        Try
            If nodeXml IsNot Nothing Then
                nodeXml = nodeXml.Attributes.ItemOf(name)
                If nodeXml IsNot Nothing Then
                    strResult = nodeXml.Value
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Shared Function GetNode(ByVal node As XmlNode, ByVal strElement As String) As XmlNode
        Try
            Return node.SelectSingleNode(strElement)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Sub AddAttributeValue(ByRef node As XmlNode, ByVal strAttribute As String, Optional ByVal strValue As String = "")
        Try
            Dim attrib As XmlAttribute = node.Attributes.ItemOf(strAttribute)

            If attrib Is Nothing Then
                attrib = node.OwnerDocument.CreateAttribute(strAttribute)
                node.Attributes.SetNamedItem(attrib)
            End If

            attrib.Value = strValue

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End If

    Public Shared Sub MergeAccessorProperties(ByVal document As XmlDocument, _
                                               ByVal prefixGet As String, _
                                               ByVal prefixSet As String, _
                                               Optional ByVal bFilterImports As Boolean = False)
        Try
            'Dim Accessor As XmlNode = Nothing
            Dim nodeProperty As XmlNode = Nothing
            Dim Getter, Setter As XmlNode
            Dim node, child As XmlNode
            Dim numID As Integer = 1

            If bFilterImports Then
                For Each node In document.SelectNodes("//interface[method[starts-with(@name,'" + prefixGet + "')]]")

                    For Each child In node.SelectNodes("method[starts-with(@name,'" + prefixGet + "')]")

                        nodeProperty = AddNewProperty(node, GetName(child).Substring(prefixSet.Length), "no", "yes", numID, child)

                        Dim strRange As String = "public"

                        If child.SelectSingleNode("return") IsNot Nothing Then
                            strRange = child.SelectSingleNode("return/variable/@range").Value
                        End If

                        Getter = nodeProperty.SelectSingleNode("get")
                        AddAttributeValue(Getter, "range", strRange)

                        node.RemoveChild(child)
                        numID += 1

                    Next
                Next
            End If

            Dim strName As String = ""
            numID = 1

            For Each node In document.SelectNodes("//class[(method[starts-with(@name,'" + prefixGet + "') or starts-with(@name,'" + prefixSet + "')])]")
                For Each child In node.SelectNodes("method[starts-with(@name,'" + prefixGet + "')]")
                    strName = GetName(child).Substring(prefixGet.Length)
                    nodeProperty = node.SelectSingleNode("property[@name='" + strName + "']")

                    If nodeProperty Is Nothing Then
                        nodeProperty = AddNewProperty(node, GetName(child).Substring(prefixGet.Length), "no", "no", numID, child)
                        numID += 1
                    End If
                Next
                For Each child In node.SelectNodes("method[starts-with(@name,'" + prefixSet + "')]")
                    strName = GetName(child).Substring(prefixGet.Length)
                    nodeProperty = node.SelectSingleNode("property[@name='" + strName + "']")
                    If nodeProperty Is Nothing Then
                        nodeProperty = AddNewProperty(node, GetName(child).Substring(prefixSet.Length), "no", "no", numID, child)
                        numID += 1
                    End If
                Next
            Next

            Dim strImplementation As String = Nothing

            For Each child In document.SelectNodes("//property")
                Getter = child.ParentNode.SelectSingleNode("method[@name='" + prefixGet + GetName(child) + "']")
                If Getter IsNot Nothing Then
                    Select Case ConvertDtdToEnumImpl(GetAttributeValue(Getter, "implementation"))
                        Case EImplementation.Interf, EImplementation.Node, EImplementation.Root
                            strImplementation = "yes"
                        Case Else
                            strImplementation = "no"
                    End Select

                    AddAttributeValue(child, "overridable", strImplementation)
                    AddAttributeValue(GetNode(child, "get"), "range", GetAttributeValue(GetNode(Getter, "descendant::return/variable"), "range"))
                    AddAttributeValue(GetNode(child, "get"), "by", GetAttributeValue(GetNode(Getter, "descendant::return/type"), "by"))
                    AddAttributeValue(GetNode(child, "get"), "modifier", GetAttributeValue(GetNode(Getter, "descendant::return/type"), "modifier"))
                    child.ParentNode.RemoveChild(Getter)
                End If
                Setter = child.ParentNode.SelectSingleNode("method[@name='" + prefixSet + GetName(child) + "']")
                If Setter IsNot Nothing Then
                    If strImplementation Is Nothing Then
                        Select Case ConvertDtdToEnumImpl(GetAttributeValue(Getter, "implementation"))
                            Case EImplementation.Interf, EImplementation.Node, EImplementation.Root
                                strImplementation = "yes"
                            Case Else
                                strImplementation = "no"
                        End Select
                        AddAttributeValue(child, "overridable", strImplementation)
                    End If

                    AddAttributeValue(GetNode(child, "set"), "range", GetAttributeValue(GetNode(Setter, "descendant::return/variable"), "range"))
                    AddAttributeValue(GetNode(child, "get"), "by", GetAttributeValue(GetNode(Setter, "descendant::param/type"), "by"))
                    child.ParentNode.RemoveChild(Setter)
                End If
            Next

            If bFilterImports Then
                numID = 1
                For Each node In document.SelectNodes("//interface[method[starts-with(@name,'" + prefixSet + "')]]")

                    For Each child In node.SelectNodes("method[starts-with(@name,'" + prefixSet + "')]")

                        '                    Debug.Print(child.OuterXml)

                        nodeProperty = AddNewProperty(node, GetName(child).Substring(prefixSet.Length), "no", "yes", numID, child)

                        Dim strRange As String = "public"

                        If child.SelectSingleNode("return") IsNot Nothing Then
                            strRange = child.SelectSingleNode("return/variable/@range").Value
                        End If

                        Setter = nodeProperty.SelectSingleNode("set")
                        AddAttributeValue(Setter, "range", strRange)

                        node.RemoveChild(child)
                        numID += 1

                    Next
                Next
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function AddNewProperty(ByVal node As XmlNode, ByVal name As String, _
                                    ByVal attribute As String, ByVal strOverridable As String, _
                                    ByVal numID As Integer, ByVal method As XmlNode) As XmlNode

        Dim xmlResult As XmlNode = Nothing
        Try
            xmlResult = node.OwnerDocument.CreateNode(XmlNodeType.Element, "property", "")
            AddAttributeValue(xmlResult, "attribute", attribute)
            AddAttributeValue(xmlResult, "overridable", strOverridable)
            AddAttributeValue(xmlResult, "name", name)
            AddAttributeValue(xmlResult, "member", "object")
            AddAttributeValue(xmlResult, "num-id", numID.ToString)

            If method.SelectSingleNode("return") IsNot Nothing Then
                Dim element As XmlNode = method.OwnerDocument.ImportNode(method.SelectSingleNode("return/type"), True)
                xmlResult.AppendChild(element)
                element = method.OwnerDocument.ImportNode(method.SelectSingleNode("return/variable"), True)
                xmlResult.AppendChild(element)
                element = method.OwnerDocument.ImportNode(method.SelectSingleNode("return/comment"), True)
                xmlResult.AppendChild(element)
            End If

            Dim Getter As XmlNode = node.OwnerDocument.CreateNode(XmlNodeType.Element, "get", "")
            AddAttributeValue(Getter, "inline", "no")
            AddAttributeValue(Getter, "by", "val")
            AddAttributeValue(Getter, "modifier", "var")
            AddAttributeValue(Getter, "range", "no")
            xmlResult.AppendChild(Getter)

            Dim Setter As XmlNode = node.OwnerDocument.CreateNode(XmlNodeType.Element, "set", "")
            AddAttributeValue(Setter, "inline", "no")
            AddAttributeValue(Setter, "by", "val")
            AddAttributeValue(Setter, "range", "no")
            xmlResult.AppendChild(Setter)

            node.InsertBefore(xmlResult, node.SelectSingleNode("method"))

        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

#End Region

#Region "Private shared methods"

#If _APP_UML = "1" Then
    Private Shared Function ConvertAndCorrectErrors(ByVal form As Form, ByVal document As XmlDocument, ByVal strFilename As String, ByRef bDtdError As Boolean, _
                                                    ByVal currentException As Exception) As EResult
        Dim oldCursor As Cursor = form.Cursor
        Dim eResult As EResult
        Try
            Dim strToolsFolder = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim strPatchFile As String = My.Computer.FileSystem.CombinePath(strToolsFolder, cstV1_2_To_V1_3_Patch)
            Dim strTempFile As String = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath.ToString, _
                                                                            cstTempUmlFile + ".xprj")
            Dim dialog As New dlgUpgradeProject

            dialog.Filename = strFilename
            dialog.Warning = currentException

            If dialog.ShowDialog(form) = MsgBoxResult.Cancel _
            Then
                eResult = XmlProjectTools.EResult.Failed
            Else
                form.Cursor = Cursors.WaitCursor

                ApplyPatchFile(strFilename, strTempFile, strPatchFile)

                ' Using avoid to remain the file locked for another process
                ' the following to Validate succeeds.
                Dim settings As XmlReaderSettings = New XmlReaderSettings()
                settings.ProhibitDtd = False
                settings.ValidationType = System.Xml.ValidationType.DTD

                ' Check second time to validate corrections.
                Using reader As XmlReader = XmlReader.Create(strTempFile, settings)
                    document.Load(reader)
                End Using

                RenumberProject(document, True)
                UpdatesCollaborations(document)
                document.Save(strTempFile)

                ' Check second time to validate corrections.
                Using reader As XmlReader = XmlReader.Create(strTempFile, settings)
                    document.Load(reader)
                End Using

                bDtdError = False
                form.Cursor = oldCursor
                eResult = XmlProjectTools.EResult.Converted
                MsgBox("Conversion completed!", MsgBoxStyle.Exclamation, "File converted")
            End If

        Catch ex As XmlSchemaException
            form.Cursor = oldCursor
            eResult = XmlProjectTools.EResult.Failed
            bDtdError = True
            MsgExceptionBox(New Exception( _
                                 "LineNumber=" + ex.LineNumber.ToString + vbCrLf + _
                                "LinePosition=" + ex.LinePosition.ToString + vbCrLf + _
                                "Message=" + ex.Message + vbCrLf + _
                                "SourceUri=" + ex.SourceUri, _
                                ex), "Click on icon to see inner exception." + vbCrLf + "Click on text message to copy 'error stacktrace' to clipboard." + vbCrLf _
                                + "Click on link to send a new issue and paste 'stracktrace' and join mentioned temporary file.", 640)

        Catch ex As Exception
            form.Cursor = oldCursor
            eResult = XmlProjectTools.EResult.Failed
            bDtdError = True
            MsgExceptionBox(ex)
        End Try
        Return eResult
    End Function

    Private Shared Function CreateAppendCollaboration(ByVal node As XmlNode) As XmlNode
        Dim collaboration As XmlNode = CreateNode(node, "collaboration")
        Dim parent As XmlComponent = XmlNodeManager.GetInstance().CreateDocument(node)
        Return parent.AppendNode(collaboration)
    End Function

    Private Shared Sub FindTemplateClasses(ByVal document As XmlDocument, ByVal eLanguage As ELanguage)
        Try
            If eLanguage = ClassXmlProject.ELanguage.Language_Vbasic Then Return

            Dim i As Integer
            For Each classNode As XmlNode In document.SelectNodes("//class[@implementation='container']")
                i = 1
                For Each model As XmlNode In classNode.SelectNodes("model")
                    ReplaceTemplateType(i, GetName(model), classNode)
                    AddAttributeValue(model, "id", "templ" + CStr(i))
                    i += 1
                Next
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub ReplaceTemplateType(ByVal index As Integer, ByVal strLabel As String, ByVal classNode As XmlNode)
        For Each typeNode As XmlNode In classNode.SelectNodes("descendant::type[@desc='" + strLabel + "']")
            RemoveAttribute(typeNode, "desc")
            AddAttributeValue(typeNode, "idref", "templ" + CStr(index))
        Next
    End Sub

    Shared Sub CleanPrefixProperties(ByVal document As XmlDocument, _
                                            Optional ByVal regexPrefixMember As String = "([a-z]{1,2}|[a-z]{0,1}str)([A-Z])", _
                                            Optional ByVal prefixMember As String = "m_")
        Dim regex As New Regex(regexPrefixMember, RegexOptions.Compiled)

        Try
            For Each name As XmlNode In document.SelectNodes("//property/@name")
                Dim strName As String = name.InnerText
                If strName.StartsWith(prefixMember) Then
                    strName = strName.Substring(Len(prefixMember))
                    If regex.IsMatch(strName) Then
                        Dim groups As String() = regex.Split(strName)
                        If groups.Length = 4 Then
                            strName = groups(2) + groups(3)
                        End If
                    End If
                    If strName.Length > 0 Then
                        name.InnerText = strName
                    End If
                End If
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub ConvertDoxygenComments(ByVal document As XmlDocument)
        Try
            Dim strTempo As String
            Dim brief, detailed As XmlNode
            Dim comment As XmlNode
            Dim delim As String = Chr(10) + Chr(13) + Chr(9) + Chr(32)

            For Each comment In document.SelectNodes("//enumvalue")
                comment.InnerText = comment.InnerText.Trim(delim.ToCharArray())
            Next

            For Each comment In document.SelectNodes("//comment")
                strTempo = ""
                Select Case comment.ParentNode.Name
                    Case "property", "typedef", "param"
                        brief = comment.SelectSingleNode("briefdescription")
                        If brief Is Nothing Then brief = comment.SelectSingleNode("parameterdescription")
                        detailed = comment.SelectSingleNode("detaileddescription")

                        If brief IsNot Nothing _
                        Then
                            strTempo = brief.InnerText

                        ElseIf detailed IsNot Nothing Then
                            strTempo = detailed.InnerText
                        End If
                        RemoveNode(comment, "node()")
                        comment.InnerText = strTempo.Trim(delim.ToCharArray())

                    Case "return", "root"
                        comment.InnerText = comment.InnerText.Trim(delim.ToCharArray())

                    Case Else
                        detailed = comment.SelectSingleNode("detaileddescription")
                        brief = comment.SelectSingleNode("briefdescription")

                        If detailed IsNot Nothing Then
                            strTempo = detailed.InnerText.Trim(delim.ToCharArray())
                            If brief Is Nothing Then
                                AddAttributeValue(comment, "brief", strTempo)
                                strTempo = ""
                            Else
                                AddAttributeValue(comment, "brief", brief.InnerText.Trim(delim.ToCharArray()))
                            End If
                        ElseIf brief Is Nothing _
                        Then
                            AddAttributeValue(comment, "brief", brief.InnerText.Trim(delim.ToCharArray()))
                            strTempo = ""
                        End If
                        RemoveNode(comment, "node()")
                        comment.InnerText = strTempo
                End Select
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub FindOverridedMethods(ByVal document As XmlDocument)

    End Sub

    Private Shared Sub FindCollaborations(ByVal document As XmlDocument)
        Try
            Dim list As XmlNodeList = document.SelectNodes("//property[id(type/@idref)[name()='class']]")
            Dim index As Integer = 1
            Dim propNode As XmlNode

            For Each propNode In list
                CreateRelationShip(index, propNode, document)
                index += 1
            Next

            list = document.SelectNodes("//typedef[id(type/@idref)[name()='class'] and type/@struct='container']")

            For Each typeNode As XmlNode In list
                propNode = typeNode.ParentNode.SelectSingleNode("property[type/@idref='" + GetID(typeNode) + "']")
                If propNode IsNot Nothing Then
                    CreateRelationShipFromType(index, propNode, typeNode, document)
                    index += 1
                End If
            Next

            UpdatesCollaborations(document)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function CreateRelationShip(ByVal index As Integer, ByVal propNode As XmlNode, ByVal document As XmlDocument) As Boolean
        Try
            Dim strComment As String = GetNodeString(propNode, "comment")
            Dim classFather As XmlNode = propNode.ParentNode
            Dim classChild As XmlNode = document.GetElementById(GetIDREF(GetNode(propNode, "type")))
            Dim range As String = GetNodeString(propNode, "variable/@range")

            Dim strXml As String = "<relationship id='relation" + index.ToString + "' action='" + strComment + "' type='assembl'>"
            strXml += "<father name='New_father' range='no' cardinal='01' level='0' member='object' idref='" + GetID(classFather) + "'>"
            strXml += "<get range='no' by='val' modifier='var' />"
            strXml += "<set range='no' by='val' />"
            strXml += "</father>"
            strXml += "<child name='" + GetName(propNode) + "' range='" + range + "' cardinal='01' level='0' member='object' idref='" + GetID(classChild) + "'>"
            strXml += "<get range='no' by='val' modifier='var' />"
            strXml += "<set range='no' by='val' />"
            strXml += "</child>"
            strXml += "</relationship>"
            Dim xmlFrag As New XmlDocument
            xmlFrag.LoadXml(strXml)
            document.DocumentElement.AppendChild(document.ImportNode(xmlFrag.FirstChild, True))

            ' Finally, we can erase property after use
            classFather.RemoveChild(propNode)
            Return True

        Catch ex As Exception
            Throw New Exception("Fails to create relationship", ex)
        End Try
        Return False
    End Function

    Private Shared Function CreateRelationShipFromType(ByVal index As Integer, ByVal propNode As XmlNode, ByVal typeNode As XmlNode, ByVal document As XmlDocument, Optional ByVal prefixList As String = "List") As Boolean
        Try
            Dim strComment As String = GetNodeString(propNode, "comment")
            Dim classFather As XmlNode = propNode.ParentNode
            Dim classChild As XmlNode = document.GetElementById(GetIDREF(GetNode(typeNode, "type")))
            Dim range As String = GetNodeString(propNode, "variable/@range")
            Dim name As String = GetName(propNode)

            If name.StartsWith(prefixList) Then name = name.Substring(Len(prefixList))

            Dim strXml As String = "<relationship id='relation" + index.ToString + "' action='" + strComment + "' type='assembl'>"
            strXml += "<father name='New_father' range='no' cardinal='01' level='0' member='object' idref='" + GetID(classFather) + "'>"
            strXml += "<get range='no' by='val' modifier='var' />"
            strXml += "<set range='no' by='val' />"
            strXml += "</father>"
            strXml += "<child name='" + name + "' range='" + range + "' cardinal='0n' level='0' member='object' idref='" + GetID(classChild) + "'>"
            strXml += typeNode.FirstChild.FirstChild.OuterXml
            strXml += "</child>"
            strXml += "</relationship>"
            Dim xmlFrag As New XmlDocument
            xmlFrag.LoadXml(strXml)
            document.DocumentElement.AppendChild(document.ImportNode(xmlFrag.FirstChild, True))

            ' Finally, we can erase typedef/property after use
            classFather.RemoveChild(propNode)
            classFather.RemoveChild(typeNode)
            Return True

        Catch ex As Exception
            Throw New Exception("Fails to create relationship", ex)
        End Try
        Return False
    End Function

    Private Shared Sub MergeIteratorContainer(ByVal document As XmlDocument, Optional ByVal strIterator As String = "::iterator")
        Try
            For Each container As XmlNode In document.SelectNodes("//typedef[type/@struct='container']")
                Dim query As String = "typedef[type/@desc='" + GetName(container) + strIterator + "']"
                Dim iterNode As XmlNode = container.ParentNode.SelectSingleNode(query)
                If iterNode IsNot Nothing Then
                    AddAttributeValue(GetNode(container, "type/list"), "iterator", "yes")
                    iterNode.ParentNode.RemoveChild(iterNode)
                End If
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub SelectInherited(ByRef iteration As Integer, ByRef parent As XmlNode, ByVal list As List(Of String))
        Try
            iteration += 1
            If iteration > cstMaxCircularReferences Then
                ' Inherited tree deepth is too big, or has circular references
                Return
            End If
            'Debug.Print("inherited=" + GetName(parent))
            list.Add(GetID(parent))

            For Each child As XmlNode In SelectNodes(parent, "inherited")
                Dim inherited As XmlNode = SelectNodeId(child, parent)
                SelectInherited(iteration, inherited, list)
            Next child
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub CopyResourceFile(ByVal strResource As String, ByVal strDestinationFile As String)
        Dim strOrigin As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, _
                                                                     My.Settings.ToolsFolder + strResource)
        File.Copy(strOrigin, strDestinationFile, True)

    End Sub

    Private Shared Sub ApplyPatchFile(ByVal strOldFile As String, ByRef strNewFile As String, ByVal FilePatch As String)
        Try
            Dim xslStylesheet As XslSimpleTransform = New XslSimpleTransform(True)
            xslStylesheet.Load(FilePatch)
            xslStylesheet.Transform(strOldFile, strNewFile)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub ChangeDoxygenTypeReferences(ByVal document As XmlDocument)
        For Each typedef As XmlNode In document.SelectNodes("//typedef")
            Dim name As String = GetName(typedef)
            Dim index As String = GetID(typedef)
            For Each reference As XmlNode In typedef.ParentNode.SelectNodes("property/type/ref[text()='" + name + "']")
                AddAttributeValue(reference, "refid", index)
            Next
        Next
    End Sub

    Private Shared Sub RenameTypeDoxygenTagFile(ByVal eLang As ELanguage, ByVal document As XmlDocument)
        Dim regArguments As New Regex("\((.*.|)\)")
        Dim arguments As String()
        Dim regReturnType As New Regex("(abstract|virtual|)(.*.)")
        Dim returnType As String = ""
        Dim child As XmlNode

        For Each method As XmlNode In document.SelectNodes("//method")
            child = method.SelectSingleNode("return")
            arguments = regReturnType.Split(child.InnerText)
            returnType = arguments(2)
            RenameType(eLang, child, returnType)
            arguments = regArguments.Split(method.SelectSingleNode("arglist").InnerText)
            arguments = arguments(1).Split(",")
            Dim i As Integer
            For Each value As String In arguments
                AddArgument(eLang, method, value, i)
            Next
            method.RemoveChild(method.SelectSingleNode("arglist"))
        Next
    End Sub

    Private Shared Sub RenameTypeDoxygenResultFile(ByVal eLang As ELanguage, ByVal document As XmlDocument)
        Try
            Dim list As XmlNodeList
            Dim child As XmlNode
            Dim new_child As XmlNode

            child = document.SelectSingleNode("//generation")
            new_child = document.CreateNode(XmlNodeType.Element, "generation", "")

            Dim strProjectPath As String = "c:\" 'GetProjectPath(child.FirstChild.InnerText.Replace("/", Path.DirectorySeparatorChar.ToString))
            If child.FirstChild IsNot Nothing Then
                strProjectPath = GetProjectPath(child.FirstChild.InnerText.Replace("/", Path.DirectorySeparatorChar.ToString))
            End If
            AddAttributeValue(new_child, "language", CStr(CType(eLang, Integer)))
            AddAttributeValue(new_child, "destination", strProjectPath)
            child.ParentNode.ReplaceChild(new_child, child)

            list = SelectNodes(document, "//package")

            Dim strCurrentPath As String
            For Each child In list
                new_child = GetNode(child, "location")
                strCurrentPath = GetProjectPath(new_child.InnerText.Replace("/", Path.DirectorySeparatorChar.ToString))
                child.RemoveChild(new_child)
                strCurrentPath = ComputeRelativePath(strProjectPath, strCurrentPath)
                If GetName(child) <> strCurrentPath Then
                    AddAttributeValue(child, "folder", strCurrentPath)
                End If
            Next

            list = SelectNodes(document, "//*[type]")

            For Each child In list
                RenameType(eLang, child)
            Next child

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function CopyLanguagePrefix(ByVal strDestinationFolder As String) As Boolean
        Try
            Dim srcFile As String = My.Settings.ToolsFolder
            srcFile = My.Computer.FileSystem.CombinePath(Application.StartupPath, srcFile)
            srcFile = My.Computer.FileSystem.CombinePath(srcFile, "language.xml")

            Dim dstFile As String = My.Computer.FileSystem.CombinePath(strDestinationFolder, Path.GetFileName(srcFile))

            If My.Computer.FileSystem.FileExists(dstFile) _
            Then
                CorrectCodePrefix(srcFile, dstFile)
            Else
                File.Copy(srcFile, dstFile, True)
            End If

            ReloadPrefixNameDocument(dstFile)

        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Private Shared Function CopyLanguageSimpleTypes(ByVal strDestinationFolder As String, ByVal eLang As ELanguage) As Boolean
        Try
            Dim strFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim srcFile As String = GetSimpleTypesFilename(eLang, strFolder)
            Dim dstFile As String = GetSimpleTypesFilename(eLang, strDestinationFolder)

            If My.Computer.FileSystem.FileExists(dstFile) _
            Then
                CorrectSimpleType(srcFile, dstFile)
            Else
                File.Copy(srcFile, dstFile, True)
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Private Shared Function CorrectSimpleType(ByVal srcFile As String, ByVal dstFile As String) As Boolean
        Try

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function CorrectCodePrefix(ByVal srcFile As String, ByVal dstFile As String) As Boolean
        Try

        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End If
#End Region
End Class
