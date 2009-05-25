Imports System
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.XmlNodeListView
Imports ClassXmlProject.UmlNodesManager

Public Class XmlPackageView
    Inherits XmlPackageSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_gridMembers As XmlDataGridView
    Private m_chkFolder As CheckBox
    Private m_txtFolder As TextBox

    Public ReadOnly Property ProjectFolder() As String
        Get
            Return GetAttribute("destination", "//generation")
        End Get
    End Property

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As Form = New dlgPackage
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub LoadValues()
        Debug.Print(Me.ToString + ".LoadValues")
        m_xmlBindingsList.Init()
    End Sub

    Public Sub UpdateValues()
        Debug.Print(Me.ToString + ".UpdateValues")
        Try
            If m_chkFolder.Checked = False Then
                m_txtFolder.Text = ""
            End If
            m_xmlBindingsList.UpdateValues()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function SearchDependencies(ByVal component As XmlComponent) As Boolean
        Try
            If component Is Nothing Then Return False

            Dim bIsEmpty As Boolean = False
            If dlgDependencies.ShowDependencies(component, bIsEmpty) _
            Then
                m_gridMembers.Binding.ResetBindings(True)
                Me.Updated = True
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Public Sub InitBindingName(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingCheckFolder(ByVal dataControl As CheckBox)
        Try
            m_chkFolder = dataControl
            m_xmlBindingsList.AddBinding(dataControl, Me, "IsFolder", "Checked")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingTextFolder(ByVal dataControl As TextBox)
        Try
            m_txtFolder = dataControl
            m_xmlBindingsList.AddBinding(dataControl, Me, "Folder")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingBriefComment(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "BriefComment")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingComment(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub LoadClasses(ByVal dataControl As XmlDataGridView)
        Try
            m_gridMembers = dataControl
            dataControl.Binding.LoadXmlNodes(Me, "import | class | package", "package_class_view")
            dataControl.Binding.NodeCounter = m_xmlReferenceNodeCounter

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub ExportReferences(ByVal fen As Form, ByVal component As XmlComponent)
        Try
            Dim dlgSaveFile As New SaveFileDialog
            Dim strFullPackage As String

            dlgSaveFile.InitialDirectory = My.Settings.ImportFolder

            Dim strFilename As String = component.Name
            If XmlProjectTools.GetValidFilename(strFilename) Then
                MsgBox("The filename was not valid, we propose to rename:" + vbCrLf + strFilename, "Rename file")
            End If
            dlgSaveFile.FileName = strFilename
            dlgSaveFile.Filter = "Package references (*.ximp)|*.ximp"

            If dlgSaveFile.ShowDialog() = DialogResult.OK Then

                strFilename = dlgSaveFile.FileName
                If strFilename.EndsWith(".ximp") = False Then
                    strFilename += ".ximp"
                End If

                My.Settings.ImportFolder = Path.GetDirectoryName(strFilename)

                Dim eLang As ELanguage = CType(Me.Tag, ELanguage)

                Select Case component.NodeName
                    Case "package"
                        strFullPackage = GetFullpathPackage(component.Node, eLang)

                        If component.SelectNodes("descendant::import").Count > 0 Then
                            MsgBox("Import members will not be exported", vbExclamation, "'Export' command")
                        End If
                        If component.SelectNodes("descendant::class[@visibility='package']").Count > 0 Then
                            ExportPackageReferences(fen, component.Node, strFilename, strFullPackage, eLang)
                        Else
                            MsgBox("Class " + component.Name + " has no class members with package visibility", vbExclamation, "'Export' command")
                        End If

                    Case "class"
                        strFullPackage = GetFullpathPackage(component.Node, eLang)

                        If component.GetAttribute("visibility") = "package" Then
                            ExportClassReferences(fen, component.Node, strFilename, strFullPackage, eLang)
                        Else
                            MsgBox("Class " + component.Name + " has not a package visibility", vbExclamation, "'Export' command")
                        End If

                    Case "import"
                        If component.Node.HasChildNodes = True Then
                            ReExport(fen, component.Node.LastChild, strFilename, component.GetAttribute("param"))
                        Else
                            MsgBox("Import " + component.Name + ", nothing to export", MsgBoxStyle.Exclamation, "'Export' command")
                        End If
                End Select
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ImportReferences(ByVal fen As Form) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim dlgOpenFile As New OpenFileDialog

            dlgOpenFile.InitialDirectory = My.Settings.ImportFolder
            dlgOpenFile.Title = "Select a package references file..."
            dlgOpenFile.Filter = "Package references (*.ximp)|*.ximp|Doxygen TAG file (*.tag)|*.tag"

            If dlgOpenFile.ShowDialog() = DialogResult.OK Then

                My.Settings.ImportFolder = Path.GetDirectoryName(dlgOpenFile.FileName)
                bResult = LoadImport(fen, dlgOpenFile.FileName)

                If bResult Then
                    Me.Updated = True
                    m_gridMembers.Binding.ResetBindings(True)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function ExportNodes(ByVal fen As Form, ByVal component As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try

            Dim dlgSaveFile As New SaveFileDialog

            dlgSaveFile.InitialDirectory = My.Settings.ImportFolder

            Dim strFilename As String = component.Name
            If XmlProjectTools.GetValidFilename(strFilename) Then
                MsgBox("The filename was not valid, we propose to rename:" + vbCrLf + strFilename, "Rename file")
            End If
            dlgSaveFile.FileName = strFilename
            dlgSaveFile.Filter = "UML project (*.xprj)|*.xprj"


            If dlgSaveFile.ShowDialog() = DialogResult.OK Then

                My.Settings.ImportFolder = Path.GetDirectoryName(dlgSaveFile.FileName)

                Dim eLang As ELanguage = CType(Me.Tag, ELanguage)
                Dim strFullPackage As String

                Select Case component.NodeName
                    Case "package"
                        strFullPackage = GetFullpathPackage(component.Node, eLang)
                        UmlNodesManager.ExportNodes(fen, component.Node, dlgSaveFile.FileName, strFullPackage, eLang)

                    Case "class"
                        strFullPackage = GetFullpathPackage(component.Node, eLang)
                        UmlNodesManager.ExportNodes(fen, component.Node, dlgSaveFile.FileName, strFullPackage, eLang)

                    Case Else
                        MsgBox("Can't export this node", MsgBoxStyle.Exclamation, "'Export' command")
                End Select
            End If
            ' Set flat Updated to prevent to close project without saving
            If bResult Then Me.Updated = True
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Sub ImportNodes(ByVal form As Form, Optional ByVal bUpdateOnly As Boolean = False)
        Dim bResult As Boolean = False
        Try
            Dim dlgOpenFile As New OpenFileDialog

            If My.Settings.ImportFolder = "." + Path.DirectorySeparatorChar.ToString Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.ImportFolder
            End If

            dlgOpenFile.Title = "Select a project file..."
            dlgOpenFile.Filter = "UML project (*.xprj)|*.xprj"

            If dlgOpenFile.ShowDialog() = DialogResult.OK Then

                Dim FileName As String = dlgOpenFile.FileName
                Dim i As Integer = InStrRev(FileName, Path.DirectorySeparatorChar.ToString)

                If i > 0 Then
                    My.Settings.ImportFolder = XmlProjectTools.GetProjectPath(FileName)
                Else
                    My.Settings.ImportFolder = dlgOpenFile.InitialDirectory
                End If

                If UmlNodesManager.ImportNodes(form, Me, FileName, m_xmlReferenceNodeCounter, bUpdateOnly) Then
                    m_gridMembers.Binding.ResetBindings(True)
                    Me.Updated = True
                    bResult = True
                End If

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overrides Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim xmlcpnt As XmlComponent = CreateDocument(removeNode.Node)
            xmlcpnt.Tag = Me.Tag

            Select Case xmlcpnt.NodeName

                Case "package", "import"
                    ' Search children from removed node
                    If xmlcpnt.SelectNodes(GetQueryListDependencies(xmlcpnt)).Count > 0 Then
                        MsgBox("This element is not empty", MsgBoxStyle.Exclamation, xmlcpnt.Name)
                        Return False
                    End If

                Case Else
                    If SelectNodes(GetQueryListDependencies(xmlcpnt)).Count > 0 Then

                        If MsgBox("Some elements reference this, you can dereference them and then this will be deleted." + _
                                  vbCrLf + "Do you want to proceed", _
                                    cstMsgYesNoQuestion, _
                                    xmlcpnt.Name) = MsgBoxResult.Yes _
                        Then
                            Dim bIsEmpty As Boolean = False

                            If dlgDependencies.ShowDependencies(xmlcpnt, bIsEmpty, "Remove references to " + xmlcpnt.Name) Then
                                bResult = True
                            End If

                            If bIsEmpty = False Then
                                Return bResult
                            End If
                        Else
                            Return False
                        End If
                    End If
            End Select

            Dim strName As String = xmlcpnt.Name
            If MsgBox("Confirm to delete:" + vbCrLf + "Name: " + strName, _
                       cstMsgYesNoQuestion, "'Delete' command") = MsgBoxResult.Yes _
            Then
                Dim strNodeName As String = removeNode.NodeName
                If MyBase.RemoveComponent(removeNode) Then
                    If strNodeName = "inherited" Then
                        m_gridMembers.Binding.ResetBindings(True)
                    End If
                    bResult = True
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Private Function LoadImport(ByVal fen As Form, ByVal fileName As String) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim import As XmlImportSpec = Nothing
            import = CreateDocument("import")
            Me.AppendComponent(import)

            If import IsNot Nothing Then
                import.NodeCounter = m_xmlReferenceNodeCounter
                Dim FileInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(fileName)
                If import.LoadDocument(fen, FileInfo) Then
                    ExtractExternalReferences(Me.Node, import.ChildExportNode.Node)
                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function
End Class
