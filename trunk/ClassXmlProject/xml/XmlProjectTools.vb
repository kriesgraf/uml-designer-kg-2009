Imports System
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Collections
Imports System.Xml
Imports System.Xml.Schema
Imports System.IO
Imports System.Text.RegularExpressions
Imports ClassXmlProject.UmlNodesManager
Imports Microsoft.VisualBasic

#If _APP_UML = "1" Then
Imports ClassXmlProject.UmlCodeGenerator
#End If

Public Class XmlProjectTools

    Public Const cstMsgYesNoExclamation As MsgBoxStyle = CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, MsgBoxStyle)
    Public Const cstMsgYesNoCancelExclamation As MsgBoxStyle = CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNoCancel + MsgBoxStyle.DefaultButton2, MsgBoxStyle)
    Public Const cstMsgYesNoQuestion As MsgBoxStyle = CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, MsgBoxStyle)
    Public Const cstMsgOkCancelCritical As MsgBoxStyle = CType(MsgBoxStyle.Critical + MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton1, MsgBoxStyle)

    Private Const cstSchemaName As String = "class-model"
    Private Const cstTempDoxygenFile As String = "__doxygenTempFile"
    Private Const cstTag2ImportStyle As String = "tag2imp.xsl"
    Private Const cstDoxygen2ProjectStyle As String = "dox2prj.xsl"

    Public Enum ECardinal
        EFix
        EVariable
        EEmptyList
        EFullList
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

    Public Shared Function CreateNewProject() As String
        Return "<?xml version='1.0' encoding='utf-8'?>" + vbCrLf + _
               "<!DOCTYPE root SYSTEM '" + GetDtdRessource() + "'>" + vbCrLf
    End Function

    Public Shared Function GetDocTypeDeclarationFile() As String
        Return cstSchemaName + ".dtd"
    End Function

    Public Shared Function GetDtdRessource() As String
        Return My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder + cstSchemaName + ".xml")
    End Function

    Public Shared Function GetDestinationDtdFile(ByVal strDestinationFolder As String) As String
        Return My.Computer.FileSystem.CombinePath(strDestinationFolder, GetDocTypeDeclarationFile())
    End Function

    Public Shared Function CheckDocTypeDeclarationFile(ByVal strDestinationFolder As String) As EDtdFileExist
        Dim strDestination As String = GetDestinationDtdFile(strDestinationFolder)
        Dim strOrigin As String = GetDtdRessource()

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
                Return EDtdFileExist.Older

            ElseIf originInfo.LastWriteTime < destInfo.LastWriteTime _
            Then
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

            Case EDtdFileExist.SourceNotFound
                MsgBox("The resource " + GetDtdRessource() + " is missing. " + _
                       vbCrLf + "Please retry installation to replace missing files. ")
                Return False

            Case EDtdFileExist.Equal
                ' Nothing to do

            Case EDtdFileExist.MoreRecent
                Select Case MsgBox("File '" + GetDestinationDtdFile(strSourceFolder) + vbCrLf + " is more recent than application resource. " + vbCrLf + _
                           "Maybe this project would corrupt application process. " + vbCrLf + _
                           "Please confirm overwriting this file (Yes), open this project as it is (No), don't open this project (Cancel).", _
                           cstMsgYesNoCancelExclamation)

                    Case MsgBoxResult.No
                        Return True

                    Case MsgBoxResult.Cancel
                        Return False
                End Select

            Case EDtdFileExist.NotFound
                If MsgBox("Can't find File '" + GetDestinationDtdFile(strSourceFolder) + vbCrLf + _
                          "An application ressource file will be copy in this folder to replace it. " + vbCrLf + _
                       "Maybe this project would be incompatible with this ressource file and  would corrupt application process." + vbCrLf + _
                       "Maybe you should apply a patch to upgrade this project and remove this warning." + vbCrLf + _
                       "Please confirm replace.", cstMsgYesNoExclamation) _
                       = MsgBoxResult.No _
                Then
                    Return False
                End If

            Case EDtdFileExist.Older
                If MsgWarningBox("File '" + GetDestinationDtdFile(strSourceFolder) + "'," + vbCrLf + _
                    "is more older than application ressource. " + vbCrLf + _
                    "Maybe oldest version projects remain in this folder, overwrite this file would corrupt these projects." + vbCrLf + _
                    "If you confirm this operation, you would have to apply a patch to each project in this folder." + vbCrLf + _
                    "Also, we recommend you to move manually this project file in an other folder and press Cancel now. !") _
                    = DialogResult.Cancel _
                Then
                    Return False
                End If

            Case Else
                Throw New Exception("Method 'CheckDocTypeDeclarationFile' has returned a wrong value")
        End Select

        CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strSourceFolder))

        Return True
    End Function

    Public Shared Function UseDocTypeDeclarationFileForImport(ByVal strSourceFolder As String) As Boolean
        Select Case CheckDocTypeDeclarationFile(strSourceFolder)

            Case EDtdFileExist.SourceNotFound
                MsgBox("The resource " + GetDtdRessource() + " is missing. " + _
                       vbCrLf + "Please retry installation to replace missing files. ")
                Return False

            Case EDtdFileExist.Equal
                ' Nothing to do

            Case EDtdFileExist.MoreRecent
                If MsgBox("File '" + GetDestinationDtdFile(strSourceFolder) + vbCrLf + " is more recent than application ressource. " + vbCrLf + _
                       "Maybe this import would corrupt current project. Please confirm importation.", _
                       cstMsgYesNoExclamation) = MsgBoxResult.No _
                Then
                    Return False
                End If

            Case EDtdFileExist.NotFound
                If MsgBox("Can't find File '" + GetDestinationDtdFile(strSourceFolder) + vbCrLf + _
                          "An application ressource file will be copy in this folder to replace it. " + vbCrLf + _
                       "Maybe this import would be incompatible with this ressource file. Please confirm replace.", _
                       cstMsgYesNoExclamation) = MsgBoxResult.No _
                Then
                    Return False
                Else
                    CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strSourceFolder))
                End If

            Case EDtdFileExist.Older
                If MsgBox("File '" + GetDestinationDtdFile(strSourceFolder) + vbCrLf + " is more older than application ressource. " + vbCrLf + _
                       "Maybe this import would corrupt current project. Please confirm importation.", _
                       cstMsgYesNoExclamation) = MsgBoxResult.No _
                Then
                    Return False
                End If

            Case Else
                Throw New Exception("Method 'CheckDocTypeDeclarationFile' has returned a wrong value")
        End Select
        Return True
    End Function

    Public Shared Function CopyDocTypeDeclarationFile(ByVal strDestinationFolder As String, Optional ByVal bNoAdvertising As Boolean = False) As Boolean
        ' With Xml Schema, we won't save document structure file into project folder.
        Dim bResult As Boolean = False
        Try
            Select Case CheckDocTypeDeclarationFile(strDestinationFolder)

                Case EDtdFileExist.SourceNotFound
                    MsgBox("The resource " + GetDtdRessource() + " is missing. " + _
                           vbCrLf + "Please retry installation to replace missing files. ", MsgBoxStyle.Critical)
                    Return False

                Case EDtdFileExist.NotFound
                    CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strDestinationFolder))
                    bResult = True

                Case EDtdFileExist.MoreRecent
                    If bNoAdvertising _
                    Then
                        CopyResourceFile(cstSchemaName + ".xml", GetDestinationDtdFile(strDestinationFolder))
                        bResult = True

                    ElseIf MsgBox("File '" + GetDestinationDtdFile(strDestinationFolder) + "'," + vbCrLf + "is more recent than application ressource." + vbCrLf + _
                              "Please confirm overwrite this file ?", _
                               cstMsgOkCancelCritical) _
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

                    ElseIf MsgWarningBox("File '" + GetDestinationDtdFile(strDestinationFolder) + "'," + vbCrLf + "is more older than application ressource. " + vbCrLf + _
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

        Dim strQuery As String
        Dim strID As String
        Dim list As XmlNodeList
        Dim collaboration As XmlNode

        For Each node As XmlNode In document.SelectNodes("//class")

            strQuery = "collaboration"
            list = SelectNodes(node, strQuery)

            For Each child In list
                node.RemoveChild(child)
            Next child

            strQuery = "//relationship[*/@idref='" + GetID(node) + "']"
            list = SelectNodes(node, strQuery)

            For Each child As XmlNode In list
                strID = GetID(child)
                collaboration = CreateAppendCollaboration(node)
                AddAttributeValue(collaboration, "idref", strID)
            Next
        Next

    End Sub

    Public Shared Sub ConvertDoxygenIndexFile(ByVal strFilename As String, ByRef strTempFile As String)
        Try
            Dim styleXsl As New XslSimpleTransform(True)
            styleXsl.Load(My.Computer.FileSystem.CombinePath(Application.StartupPath, _
                                                             My.Settings.ToolsFolder + cstDoxygen2ProjectStyle))

            strTempFile = My.Computer.FileSystem.CombinePath(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData, _
                                                             cstTempDoxygenFile + ".xprj")

            Dim argList As New Dictionary(Of String, String)
            argList.Add("DoxFolder", GetProjectPath(strFilename) + "\")

            ' This transformation generates anmetafile 80% compliant with end-generated file
            styleXsl.Transform(strFilename, strTempFile, argList)

            Dim document As New XmlDocument

            LoadDocument(document, strTempFile)

            ' Some doxygen types reference in "ref" element, the refid references the relationship child class and not the container type
            ChangeDoxygenTypeReferences(document)

            ' Some doxygen types remain as simple C++ declaration, we must translate them to UML design Xml elements
            RenameTypeDoxygenResultFile(ELanguage.Language_CplusPlus, document)

            ' Merge iterator with container
            MergeIteratorContainer(document)

            'Find template C++ class
            FindTemplateClasses(document)

            ' Temporary saving to check result before renumber
            document.Save(strTempFile)

            ' We insert the external DTD file declaration to be sure that generated file is full compliant
            Dim docType As XmlDocumentType = document.CreateDocumentType("root", Nothing, "class-model.dtd", "")
            document.InsertBefore(docType, document.FirstChild.NextSibling)

            ' Apply UML Designer element indexation
            RenumberProject(document)

            ' Temporary saving to check result before renumber
            document.Save(strTempFile)

            ' Remove prefix in properties , accessors, convert doxygen comments
            CleanPrefixProperties(document)
            MergeAccessorProperties(document)
            ConvertDoxygenComments(document)

            ' Find collaboration
            FindCollaborations(document)

            ' Find overrided methods
            'FindOverridedMethods(document)

            ' Now document is no more "Standalone"
            CType(document.FirstChild, XmlDeclaration).Standalone = "no"
            document.Save(strTempFile)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function ApplyPatch(ByVal ParentForm As Form, ByVal strOldFile As String, ByRef strNewFile As String) As Boolean
        Dim oldCursor As Cursor = ParentForm.Cursor
        Try
            strNewFile = ""
            Dim dlgOpenFile As New OpenFileDialog
            dlgOpenFile.InitialDirectory = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            dlgOpenFile.Title = "Select a patch file..."
            dlgOpenFile.Filter = "UML  patch (*.xptch)|*.xptch"

            If (dlgOpenFile.ShowDialog() = DialogResult.OK) _
            Then
                ParentForm.Cursor = Cursors.WaitCursor

                Dim FilePatch As String = dlgOpenFile.FileName
                strNewFile = strOldFile + ".new.xprj"
                ApplyPatchFile(strOldFile, strNewFile, FilePatch)
                MsgBox("Please find result of conversion in file:" + vbCrLf + strNewFile, MsgBoxStyle.Exclamation)
                Return True
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            ParentForm.Cursor = oldCursor
        End Try
        Return False
    End Function

    Public Shared Sub ConvertDoxygenTagFile(ByVal document As XmlDocument, ByVal strFilename As String)
        Try
            Dim styleXsl As New XslSimpleTransform(True)
            styleXsl.Load(My.Computer.FileSystem.CombinePath(Application.StartupPath, _
                                                             My.Settings.ToolsFolder + cstTag2ImportStyle))

            Dim strTempFile As String = My.Computer.FileSystem.CombinePath(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData, _
                                                                           cstTempDoxygenFile + ".ximp")
            styleXsl.Transform(strFilename, strTempFile)
            LoadDocument(document, strTempFile)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub LoadDocument(ByVal document As XmlDocument, ByVal strFilename As String, Optional ByVal bWithXsdValidation As Boolean = False)
        ' TODO replace optional argument bWithXsdValidation with true when XmlSchema validation works with ID/IDREF
        Try
            If bWithXsdValidation Then
                ' With Xml Schema, we don't save document structure file into project folder.
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
                Using reader As XmlReader = XmlReader.Create(strFilename, settings)
                    document.Load(reader)
                End Using
            End If
        Catch ex As XmlSchemaException

            Throw New Exception("LineNumber=" + ex.LineNumber.ToString + vbCrLf + _
                                "LinePosition=" + ex.LinePosition.ToString + vbCrLf + _
                                "Message=" + ex.Message + vbCrLf + _
                                "SourceUri=" + ex.SourceUri, ex)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function CreateAppendNode(ByVal node As XmlNode, ByVal szElement As String, Optional ByVal bInsertLf As Boolean = True) As XmlNode
        Dim xmlresult As XmlNode = node.OwnerDocument.CreateNode(XmlNodeType.Element, szElement, "")
        node.AppendChild(xmlresult)
        Return xmlresult
    End Function

    Private Shared Function CreateAppendCollaboration(ByVal node As XmlNode) As XmlNode
        Dim collaboration As XmlNode = CreateNode(node, "collaboration")
        Dim before As XmlNode = node.SelectSingleNode("inline")
        If before Is Nothing Then
            before = node.SelectSingleNode("comment")
        End If
        node.InsertBefore(collaboration, before)
        Return collaboration
    End Function

    Public Shared Sub AddAttributeValue(ByRef node As XmlNode, ByVal strAttribute As String, Optional ByVal strValue As String = "")
        Try
            Dim attrib As XmlAttribute = node.Attributes.GetNamedItem(strAttribute)

            If attrib Is Nothing Then
                attrib = node.OwnerDocument.CreateAttribute(strAttribute)
                node.Attributes.SetNamedItem(attrib)
            End If

            attrib.Value = strValue

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub RemoveAttribute(ByRef node As XmlNode, ByVal strAttribute As String)
        Try
            Dim attrib As XmlAttribute = node.Attributes.GetNamedItem(strAttribute)

            If attrib IsNot Nothing Then
                attrib = node.OwnerDocument.CreateAttribute(strAttribute)
                node.Attributes.RemoveNamedItem(strAttribute)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

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

    Public Shared Function GetNodeRefCount(ByVal node As XmlNode, ByRef strList As String, ByVal eTag As ELanguage) As Integer
        Dim iResult As Integer = -1
        Try
            Dim list As XmlNodeList
            Dim child As XmlNode
            Dim parent As XmlNode
            Dim strQuery As String

            iResult = 0
            strQuery = "//*[@idref='" + GetID(node) + "' or @index-idref='" + GetID(node) + "']"
            list = SelectNodes(node, strQuery)

            For Each child In list
                Select Case child.Name
                    Case "father"
                        strList = strList + vbCrLf + GetFatherRelation(child)
                        iResult = iResult + 1

                    Case "child"
                        strList = strList + vbCrLf + GetChildRelation(child)
                        iResult = iResult + 1

                    Case "dependency"
                        parent = GetNode(child, "parent::class")
                        strList = strList + vbCrLf + "Dependency '" + GetAttributeValue(child, "action") + "' with " + GetFullpathDescription(parent, eTag)
                        iResult = iResult + 1

                    Case "inherited"
                        parent = GetNode(child, "parent::class")
                        strList = strList + vbCrLf + "Customize by " + GetFullpathDescription(parent, eTag)
                        iResult = iResult + 1

                    Case "list"
                        parent = child.ParentNode
                        If GetID(node) = GetAttributeValue(child, "index-idref") Then
                            strList = strList + vbCrLf + "Used as index in container "
                        Else
                            strList = strList + vbCrLf + "Used as container in "
                        End If
                        Select Case parent.Name
                            Case "type"
                                strList = strList + GetTypeRelation(parent, eTag)
                            Case "child"
                                strList = strList + GetChildRelation(parent)
                            Case "father"
                                strList = strList + GetFatherRelation(parent)
                        End Select

                        iResult = iResult + 1

                    Case "type"
                        strList = strList + vbCrLf + "Used as type in " + GetTypeRelation(child, eTag)
                        iResult = iResult + 1

                    Case "element"
                        strList = strList + vbCrLf + "Used as type in " + GetTypeRelation(child.ParentNode, eTag) + ", attribute '" + GetName(child) + "'"
                        iResult = iResult + 1

                    Case Else
                        strList = strList + vbCrLf + "Referenced by node '" + child.Name + "' (" + GetName(child) + ")"
                        iResult = iResult + 1

                End Select
            Next child
        Catch ex As Exception
            Throw ex
        End Try
        Return iResult
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

    Public Shared Function GetNode(ByVal node As XmlNode, ByVal strElement As String) As XmlNode
        Try
            Return node.SelectSingleNode(strElement)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

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

    Public Shared Function GetName(ByVal node As XmlNode) As String
        Return GetAttributeValue(node, "name")
    End Function

    Public Shared Function GetPackage(ByVal node As XmlNode) As String
        If node.Name = "reference" Then
            Return GetAttributeValue(node, "package")
        End If
        Return GetName(node.ParentNode)
    End Function

    Public Shared Function GetNextClassId(ByVal node As XmlNode) As Integer

        Dim num_id As Integer
        Dim list As XmlNodeList
        Dim child As XmlNode

        GetNextClassId = CInt(XmlNodeCounter.AfterStr(GetNodeString(node, "@id"), "class"))
        list = SelectNodes(node, "descendant::*/@id")

        For Each child In list

            num_id = CInt(XmlNodeCounter.AfterStr(child.Value, "class"))

            If num_id > GetNextClassId Then
                GetNextClassId = num_id
            End If
        Next child

        GetNextClassId = GetNextClassId + 1

    End Function

    Public Shared Function GetCurrentName(ByVal iterator As IEnumerator) As String
        Return GetAttributeValue(CType(iterator.Current, XmlNode), "name")
    End Function

    Public Shared Function GetAttributeValue(ByVal nodeXml As XmlNode, ByVal name As String) As String
        Dim strResult As String = Nothing
        Try
            If nodeXml IsNot Nothing Then
                nodeXml = nodeXml.Attributes.GetNamedItem(name)
                If nodeXml IsNot Nothing Then
                    strResult = nodeXml.Value
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
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
            Case Else
                Return "simple"
        End Select
    End Function

    Public Shared Function ConvertViewToEnumImpl(ByVal strImpl As String) As EImplementation
        If strImpl Is Nothing Then
            Throw New Exception("Argument can't be null")
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
            Return " ..."
        Else
            Return ", ...}"
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

    Public Shared Function GetFullUmlPath(ByVal current As XmlNode) As String
        Try
            Select Case current.Name
                Case "package", "class", "import"
                    Return GetFullUmlPath(current.ParentNode) + "/" + GetName(current)

                Case "export"
                    Return GetFullUmlPath(current.ParentNode.ParentNode) + "/" + GetName(current)

                Case Else
                    Return "/" + GetName(current)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetFullpathDescription(ByVal current As XmlNode, ByVal eTag As ELanguage) As String
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
                        current = current.SelectSingleNode("ancestor::typedef")
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

                    Case "type"
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

                    Case "reference"
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

                        If eTag <> ELanguage.Language_CplusPlus Then
                            strTempo = GetAttributeValue(current, "param")
                            If strTempo IsNot Nothing _
                            Then
                                strResult = strTempo + strSeparator + strResult
                            End If
                        End If

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

    Public Shared Function GetSimpleTypesFilename(ByVal value As Integer) As String
        Dim strFilename As String = My.Settings.ToolsFolder
        strFilename = My.Computer.FileSystem.CombinePath(Application.StartupPath, strFilename)
        Dim index As ELanguage = CType(value, ELanguage)
        Select Case index
            Case ELanguage.Language_Java
                strFilename = My.Computer.FileSystem.CombinePath(strFilename, "LanguageJava.xml")
            Case ELanguage.Language_Vbasic
                strFilename = My.Computer.FileSystem.CombinePath(strFilename, "LanguageVbasic.xml")
            Case Else
                strFilename = My.Computer.FileSystem.CombinePath(strFilename, "LanguageCplusPlus.xml")
        End Select
        Return strFilename
    End Function

    Public Shared Function StringToCardinal(ByVal strCardinal As String) As ECardinal
        Dim eResult As ECardinal
        Select Case strCardinal
            Case "1"
                eResult = ECardinal.EFix
            Case "01"
                eResult = ECardinal.EVariable
            Case "0n"
                eResult = ECardinal.EEmptyList
            Case "1n"
                eResult = ECardinal.EFullList
            Case Else
                eResult = ECardinal.EFix
        End Select
        Return eResult
    End Function

    Public Shared Function CardinalToString(ByVal eCardinal As ECardinal) As String
        Dim strResult As String
        Select Case eCardinal
            Case eCardinal.EFix
                strResult = "1"
            Case eCardinal.EVariable
                strResult = "01"
            Case eCardinal.EEmptyList
                strResult = "0n"
            Case eCardinal.EFullList
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
                        If parent.Name <> "typedef" Then
                            parent = parent.ParentNode
                        End If
                        strID = GetID(child)
                        strID = "enum" + XmlNodeCounter.AfterStr(GetID(parent), "class") + "_" + XmlNodeCounter.AfterStr(strID, "_")
                        'Debug.Print("StartInsertion: renumber (" + GetID(child) + ") " + child.Name + "-->" + strID)
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

    Public Shared Sub ChangeID(ByVal nodeReference As XmlNode, ByVal source As XmlNode, ByVal szNewID As String)
        Dim list As XmlNodeList
        Dim attrib As XmlNode

        Dim strOldID As String = GetID(nodeReference)
        Dim strQuery As String
        If strOldID.StartsWith("class") Or strOldID.StartsWith("relation") Then
            strQuery = "//@idref[.='" + strOldID + "']"
            list = source.SelectNodes(strQuery)
            For Each attrib In list
                'Debug.Print(attrib.OuterXml)
                attrib.Value = szNewID
            Next attrib
        Else
            strQuery = "//@valref[.='" + strOldID + "']"
            list = source.SelectNodes(strQuery)
            For Each attrib In list
                'Debug.Print(attrib.OuterXml)
                attrib.Value = szNewID
            Next attrib

            strQuery = "//@sizeref[.='" + strOldID + "']"
            list = source.SelectNodes(strQuery)
            For Each attrib In list
                'Debug.Print(attrib.OuterXml)
                attrib.Value = szNewID
            Next attrib
        End If
        SetID(nodeReference, szNewID)
    End Sub

    Public Shared Sub LoadTreeInherited(ByVal parent As XmlNode, ByVal list As List(Of String))
        Try
            SelectInherited(SelectNodeId(parent), list)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function CreateNode(ByVal treeNode As XmlNode, ByVal strElement As String) As XmlNode
        Return treeNode.OwnerDocument.CreateElement(strElement)
    End Function

    Private Shared Sub FindTemplateClasses(ByVal document As XmlDocument)
        Try
            Dim i As Int16
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

    Private Shared Sub ReplaceTemplateType(ByVal index As Int16, ByVal strLabel As String, ByVal classNode As XmlNode)
        For Each typeNode As XmlNode In classNode.SelectNodes("descendant::type[@desc='" + strLabel + "']")
            RemoveAttribute(typeNode, "desc")
            AddAttributeValue(typeNode, "idref", "templ" + CStr(index))
        Next
    End Sub

    Private Shared Sub MergeAccessorProperties(ByVal document As XmlDocument, _
                                               Optional ByVal prefixGet As String = "Get", _
                                               Optional ByVal prefixSet As String = "Set")
        Try
            Dim Accessor As XmlNode = Nothing

            For Each prop As XmlNode In document.SelectNodes("//property")
                Accessor = prop.ParentNode.SelectSingleNode("method[@name='" + prefixGet + GetName(prop) + "']")
                If Accessor IsNot Nothing Then
                    AddAttributeValue(GetNode(prop, "get"), "range", GetAttributeValue(GetNode(Accessor, "descendant::return/variable"), "range"))
                    AddAttributeValue(GetNode(prop, "get"), "by", GetAttributeValue(GetNode(Accessor, "descendant::return/type"), "by"))
                    AddAttributeValue(GetNode(prop, "get"), "modifier", GetAttributeValue(GetNode(Accessor, "descendant::return/type"), "modifier"))
                    prop.ParentNode.RemoveChild(Accessor)
                End If
                Accessor = prop.ParentNode.SelectSingleNode("method[@name='" + prefixSet + GetName(prop) + "']")
                If Accessor IsNot Nothing Then
                    AddAttributeValue(GetNode(prop, "set"), "range", GetAttributeValue(GetNode(Accessor, "descendant::return/variable"), "range"))
                    AddAttributeValue(GetNode(prop, "get"), "by", GetAttributeValue(GetNode(Accessor, "descendant::param/type"), "by"))
                    prop.ParentNode.RemoveChild(Accessor)
                End If
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub CleanPrefixProperties(ByVal document As XmlDocument, _
                                             Optional ByVal regexPrefixMember As String = "^m_[a-z]{1,4}", _
                                             Optional ByVal prefixMember As String = "m_")
        Dim regex As New Regex(regexPrefixMember, RegexOptions.Compiled)

        Try
            For Each name As XmlNode In document.SelectNodes("//property/@name")
                If regex.IsMatch(name.InnerText) Then
                    Dim groups As String() = regex.Split(name.InnerText)
                    If groups.Length > 1 Then
                        name.InnerText = groups(1)
                    End If
                ElseIf name.InnerText.StartsWith(prefixMember) Then
                    name.InnerText = name.InnerText.Substring(Len(prefixMember))
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

    Private Shared Sub FindCollaborations(ByVal document As XmlDocument)
        Try
            Dim list As XmlNodeList = document.SelectNodes("//property[id(type/@idref)[name()='class']]")
            Dim index As Int16 = 1
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

    Private Shared Function CreateRelationShip(ByVal index As Int16, ByVal propNode As XmlNode, ByVal document As XmlDocument) As Boolean
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

    Private Shared Function CreateRelationShipFromType(ByVal index As Int16, ByVal propNode As XmlNode, ByVal typeNode As XmlNode, ByVal document As XmlDocument, Optional ByVal prefixList As String = "List") As Boolean
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

    Private Shared Sub SelectInherited(ByRef parent As XmlNode, ByVal list As List(Of String))
        Try
            'Debug.Print("inherited=" + GetName(parent))
            list.Add(GetID(parent))

            For Each child In SelectNodes(parent, "inherited")
                Dim inherited As XmlNode = SelectNodeId(child, parent)
                SelectInherited(inherited, list)
            Next child
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub CopyResourceFile(ByVal strRessource As String, ByVal strDestinationFile As String)
        Dim strOrigin As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, _
                                                                     My.Settings.ToolsFolder + strRessource)
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

    Private Shared Sub RenameTypeDoxygenResultFile(ByVal eLang As ELanguage, ByVal document As XmlDocument)
        Try
            Dim list As XmlNodeList
            Dim child As XmlNode
            Dim new_child As XmlNode

            child = document.SelectSingleNode("//generation")
            new_child = document.CreateNode(XmlNodeType.Element, "generation", "")

            Dim strProjectPath As String = GetProjectPath(child.FirstChild.InnerText.Replace("/", "\"))
            AddAttributeValue(new_child, "language", CStr(CType(eLang, Integer)))
            AddAttributeValue(new_child, "destination", strProjectPath)
            child.ParentNode.ReplaceChild(new_child, child)

            list = SelectNodes(document, "//package")

            Dim strCurrentPath As String
            For Each child In list
                new_child = GetNode(child, "location")
                strCurrentPath = GetProjectPath(new_child.InnerText.Replace("/", "\"))
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

    Private Shared Function GetFatherRelation(ByVal father As XmlNode) As String
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

    Private Shared Function GetChildRelation(ByVal father As XmlNode) As String
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

    Private Shared Function GetTypeRelation(ByVal node As XmlNode, ByVal eTag As ELanguage) As String
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

                    If GetName(method) = "" Then
                        strResult = GetFullpathDescription(GetNode(method, "parent::class"), eTag) + strSeparator + "<Constructor>, argument " + GetName(method)
                    Else
                        strResult = GetFullpathDescription(GetNode(method, "parent::class"), eTag) + strSeparator + GetName(method) + ", argument " + GetName(parent)
                    End If

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
End Class
