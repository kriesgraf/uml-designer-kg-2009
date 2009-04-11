Imports System
Imports System.Xml
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlProjectTools

Public Class frmProject

#Region "Class declarations"

    Private m_xmlProject As New XmlProjectView
    Private OptionVisuMode As Integer = -1

#End Region

#Region "Properties"

    Public ReadOnly Property Mainframe() As MDIParent
        Get
            Return CType(Me.MdiParent, MDIParent)
        End Get
    End Property

    Public ReadOnly Property ProjectName() As String
        Get
            Return m_xmlProject.Filename
        End Get
    End Property

#End Region

#Region "Public methods"

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        XmlNodeManager.GetInstance()
    End Sub

    Public Function OpenProject(ByVal parent As Form, ByVal filename As String) As Boolean
        If m_xmlProject.Open(parent, filename) Then
            If m_xmlProject.Updated Then
                RefreshProjectDisplay()
            End If
            Return True
        End If
        Return False
    End Function

    Public Sub SaveProject()
        If m_xmlProject.Save() = False Then
            SaveAs()
        Else
            RefreshProjectDisplay()
        End If
    End Sub

    Public Sub PrintPreview()
        Me.docvwProjectDisplay.ShowPrintPreviewDialog()
    End Sub

    Public Sub PrintPage()
        Me.docvwProjectDisplay.ShowPrintDialog()
    End Sub
#End Region

#Region "Private methods"

    Private Sub RefreshProjectDisplay()
        If lvwProjectMembers.SelectedItem IsNot Nothing _
        Then
            docvwProjectDisplay.DataSource = CType(lvwProjectMembers.SelectedItem, XmlComponent).Node
        Else
            docvwProjectDisplay.DataSource = CType(lvwProjectMembers.Binding.Parent, XmlComponent).Node
        End If

        Me.Mainframe.UpdateItemControls(m_xmlProject)

        If m_xmlProject.Updated Then
            Me.Text = lvwProjectMembers.Path + " *"
        Else
            Me.Text = lvwProjectMembers.Path
        End If
    End Sub

    Private Sub UpdateButtons()
        RefreshProjectDisplay()
        btnHome.Enabled = lvwProjectMembers.Binding.IsNotHome()
        btnUp.Enabled = btnHome.Enabled
        UpdateButtonProjectView()
    End Sub

    Private Sub UpdateButtonProjectView()
        btnProjectView.Text = btnProjectView.DropDownItems(lvwProjectMembers.View).Text
    End Sub

    Private Function SaveAs() As Boolean
        Try
            Dim dlgSaveFile As New SaveFileDialog

            If My.Settings.CurrentFolder = "." + Path.DirectorySeparatorChar.ToString Then
                dlgSaveFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgSaveFile.InitialDirectory = My.Settings.CurrentFolder
            End If

            dlgSaveFile.Title = "Save the project file..."
            dlgSaveFile.Filter = "UML project (*.xprj)|*.xprj"

            Dim strFilename As String = m_xmlProject.Name
            If XmlProjectTools.GetValidFilename(strFilename) Then
                MsgBox("The filename was not valid, we propose to rename:" + vbCrLf + strFilename)
            End If
            dlgSaveFile.FileName = strFilename

            If (dlgSaveFile.ShowDialog(Me) = DialogResult.OK) Then
                My.Settings.CurrentFolder = XmlProjectTools.GetProjectPath(dlgSaveFile.FileName)
                m_xmlProject.SaveAs(dlgSaveFile.FileName)
                lvwProjectMembers.UpdatePath()
                RefreshProjectDisplay()
                mnuFileSave.Enabled = False
                Return True
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return False
    End Function

    Private Sub frmProject_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        mnuEditPaste.Enabled = XmlComponent.Clipboard.CanPaste
        btnPaste.Enabled = mnuEditPaste.Enabled

        Me.Mainframe.UpdateItemControls(m_xmlProject)
    End Sub

    Private Sub frmProject_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If DEBUG_COMMANDS_ACTIVE Then
            mnuProjectParameters.Visible = True
            mnuEditProperties.Visible = True
            mnuPackageProperties.Visible = True
            mnuClassMemberProperties.Visible = True
            ReferenceProperties.Visible = True
            RenumberDatabaseIndex.Visible = True
            UpdatesCollaborations.Visible = True
            mnuEditDatabase.Visible = True
        End If

        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Me.WindowState = FormWindowState.Maximized
        Try
            btnUp.Enabled = False

            If m_xmlProject.IsNew _
            Then
                ' We call project by its form name at beginning
                m_xmlProject.Updated = True
                m_xmlProject.Name = Me.Text
                mnuFileSave.Enabled = False
            End If

            With m_xmlProject
                .ListViewControl = lvwProjectMembers
                .AddMenuProject(mnuProjectList)
                .AddMenuPackage(mnuPackageList)
                .AddMenuClass(mnuClassMembers)
                .AddMenuImport(mnuEditReference)
                .LoadMembers()

                docvwProjectDisplay.Language = .Properties.GenerationLanguage
                ' change display of docvwProjectDisplay & btnDocView in one shot
                DocViewMenuItem_Click(UmlViewToolStripMenuItem, Nothing)

                If .IsNew Then
                    docvwProjectDisplay.DataSource = CType(lvwProjectMembers.Binding.Parent, XmlComponent).Node
                    mnuProjectProperties_Click(Me, Nothing)
                Else
                    Me.Text = .Name
                    lvwProjectMembers.SelectItem(0)
                    If lvwProjectMembers.SelectedItem IsNot Nothing Then
                        docvwProjectDisplay.DataSource = CType(lvwProjectMembers.SelectedItem, XmlComponent).Node
                    Else
                        docvwProjectDisplay.DataSource = CType(lvwProjectMembers.Binding.Parent, XmlComponent).Node
                    End If
                End If

                .UpdateMenuClass(lvwProjectMembers.Binding.Parent, AddClassTypedef, AddClassConstructor)
            End With

            UpdateButtons()

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            Me.Cursor = oldCursor
        End Try
    End Sub

    Private Sub frmProject_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If m_xmlProject.IsNew _
            Then
                If m_xmlProject.Updated Then

                    m_xmlProject.Updated = False
                    'Me.Mainframe.UpdateItemControls(m_xmlProject)

                    If MsgBox("Would you want to save updates ?", vbYesNo + vbDefaultButton1 + vbQuestion, Me.Text) = vbYes Then
                        If SaveAs() = False Then
                            If MsgBox("Retry to save this project ?", vbRetryCancel + vbQuestion + vbDefaultButton2, Me.Text) = vbCancel Then
                                e.Cancel = False
                            Else
                                e.Cancel = True
                                SaveAs()  ' We retry save but leave without confirmation
                            End If
                        End If
                    End If
                End If

            ElseIf m_xmlProject.Updated _
            Then
                Dim eResult As Microsoft.VisualBasic.MsgBoxResult
                eResult = MsgBox("Would you want to save updates ?", vbYesNoCancel + vbDefaultButton1 + vbQuestion, Me.Text)
                If eResult = vbYes Then
                    m_xmlProject.Save()
                    'Me.Mainframe.UpdateItemControls(m_xmlProject)
                    e.Cancel = False
                ElseIf eResult = vbCancel Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub btnHome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHome.Click
        Try
            lvwProjectMembers.GoHome()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub btnUp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUp.Click
        Try
            lvwProjectMembers.GoBack()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub lvwProjectMembers_EmptyZoneClick(ByVal sender As DataListView, ByVal e As System.EventArgs) Handles lvwProjectMembers.EmptyZoneClick
        docvwProjectDisplay.DataSource = lvwProjectMembers.Binding.Parent.Node
    End Sub

    Private Sub lvwProjectMembers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwProjectMembers.SelectedIndexChanged
        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Try
            If lvwProjectMembers.SelectedIndex <> -1 Then
                docvwProjectDisplay.DataSource = CType(lvwProjectMembers.SelectedItem, XmlComponent).Node
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            Me.Cursor = oldCursor
        End Try
    End Sub

    Private Sub lvwProjectMembers_ChangeLevel(ByVal sender As Object, ByVal e As DataListViewEventArgs) _
                Handles lvwProjectMembers.GoHomeLevel, _
                        lvwProjectMembers.GoParentLevel, _
                        lvwProjectMembers.GoChildLevel
        Try
            docvwProjectDisplay.DataSource = lvwProjectMembers.Binding.Parent.Node
            m_xmlProject.UpdateMenuClass(lvwProjectMembers.Binding.Parent, AddClassTypedef, AddClassConstructor)
            UpdateButtons()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub lvwProjectMembers_ItemChanged(ByVal sender As Object, ByVal e As DataListViewEventArgs) Handles lvwProjectMembers.ItemChanged
        lvwProjectMembers.SelectItem(e.SelectedIndex)
        Try
            If lvwProjectMembers.SelectedItem IsNot Nothing Then
                docvwProjectDisplay.DataSource = CType(lvwProjectMembers.SelectedItem, XmlComponent).Node
            Else
                docvwProjectDisplay.DataSource = CType(lvwProjectMembers.Binding.Parent, XmlComponent).Node
            End If
            RefreshProjectDisplay()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuFileSaveAs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFileSaveAs.Click
        SaveAs()
        Me.Text = lvwProjectMembers.UpdatePath()
    End Sub

    Private Sub mnuFileSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
        m_xmlProject.Save()
        RefreshProjectDisplay()
    End Sub

    Private Sub mnuFileNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileNew.Click
        Me.Mainframe.ShowNewForm(sender, e)
    End Sub

    Private Sub mnuFileOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileOpen.Click
        Me.Mainframe.OpenMultipleFiles(sender, e)
    End Sub

    Private Sub mnuFileNewDoxygenFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileNewDoxygenFile.Click
        Me.Mainframe.ImportFromDoxygenIndex()
    End Sub

    Private Sub mnuFileNewOmgUmlFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileNewOmgUmlFile.Click
        Me.Mainframe.ImportFromOmgUmlModel()
    End Sub

    Private Sub mnuFilePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilePrint.Click
        Me.PrintPage()
    End Sub

    Private Sub mnuFilePrintPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilePrintPreview.Click
        Me.PrintPreview()
    End Sub

    Private Sub mnuFileExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileExit.Click
        Try
            Me.Mainframe.ExitProgram(sender, e)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuFileClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileClose.Click
        Me.Close()
    End Sub

    Private Sub mnuEditProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles mnuEditProperties.Click, _
                mnuPackageProperties.Click, _
                mnuClassMemberProperties.Click, _
                ReferenceProperties.Click
        Try
            dlgXmlNodeProperties.DisplayProperties(lvwProjectMembers.SelectedItem)

            RefreshProjectDisplay()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuProjectEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles mnuProjectEdit.Click, mnuPackageEdit.Click, mnuClassMemberEdit.Click, EditReference.Click

        Try
            If lvwProjectMembers.EditCurrentItem() _
            Then
                RefreshProjectDisplay()
            End If

            mnuEditPaste.Enabled = XmlComponent.Clipboard.CanPaste
            btnPaste.Enabled = mnuEditPaste.Enabled

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuProjectDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
                Handles mnuProjectDelete.Click, mnuPackageDelete.Click, mnuClassDeleteMember.Click, _
                DeleteReference.Click
        Try
            If lvwProjectMembers.DeleteSelectedItems() _
            Then
                RefreshProjectDisplay()
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuProjectProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuProjectProperties.Click
        Try
            If m_xmlProject.EditProperties() Then
                m_xmlProject.UpdateMenuClass(lvwProjectMembers.Binding.Parent, AddClassTypedef, AddClassConstructor)
                docvwProjectDisplay.Language = m_xmlProject.Properties.GenerationLanguage
                RefreshProjectDisplay()
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuProjectParameters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuProjectParameters.Click
        Try
            If m_xmlProject.EditParameters() Then
                m_xmlProject.UpdateMenuClass(lvwProjectMembers.Binding.Parent, AddClassTypedef, AddClassConstructor)
                RefreshProjectDisplay()
                docvwProjectDisplay.Language = m_xmlProject.Properties.GenerationLanguage
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuPackageMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPackageMoveUp.Click
        Try
            With lvwProjectMembers
                If .SelectedItem IsNot Nothing And .Binding.Parent IsNot Nothing Then
                    If m_xmlProject.MoveUpNode(.Binding.Parent, CType(.SelectedItem, XmlComponent)) Then
                        .Binding.ResetBindings(True)
                        docvwProjectDisplay.DataSource = .Binding.Parent.Node
                        RefreshProjectDisplay()
                    End If
                End If
            End With
            Me.Activate()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuAddNode_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
        Handles mnuAddClass.Click, mnuPackageAddClass.Click, _
        AddClassStructure.Click, AddClassContainer.Click, AddClassTypedef.Click, _
        AddClassConstructor.Click, AddClassMethod.Click, _
        AddClassProperty.Click, mnuAddRelationship.Click, _
        mnuAddPackage.Click, mnuPackageAddPackage.Click, _
        mnuAddImport.Click, mnuPackageAddImport.Click, _
        NewReference.Click, NewInterface.Click, AddClassImport.Click

        Try
            lvwProjectMembers.AddItem(CType(sender.Tag, String))

            RefreshProjectDisplay()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuImportExport_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
        Handles mnuReplaceExport.Click, mnuMergeExport.Click, mnuConfirmExport.Click
        Try
            If lvwProjectMembers.Binding.Parent IsNot Nothing Then
                If m_xmlProject.AddReferences(Me.Mainframe, lvwProjectMembers.Binding.Parent, CType(sender.Tag, XmlImportSpec.EImportMode)) Then
                    RefreshProjectDisplay()
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuFindRedundant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindRedundant.Click
        Try
            With lvwProjectMembers
                If .SelectedItem IsNot Nothing And .Binding.Parent IsNot Nothing Then
                    If m_xmlProject.RemoveRedundantReference(.Binding.Parent, CType(.SelectedItem, XmlComponent)) Then
                        .Binding.ResetBindings(True)
                        docvwProjectDisplay.DataSource = .Binding.Parent.Node
                        RefreshProjectDisplay()
                    End If
                End If
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveAll.Click
        Try
            If lvwProjectMembers.Binding.Parent IsNot Nothing Then
                If m_xmlProject.RemoveAllReferences(lvwProjectMembers.Binding.Parent) Then
                    docvwProjectDisplay.DataSource = lvwProjectMembers.Binding.Parent.Node
                    RefreshProjectDisplay()
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuGenerateCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles mnuProjectGenerate.Click, mnuPackageGenerate.Click
        Try
            Dim xmlcpnt As XmlComponent = CType(lvwProjectMembers.SelectedItem, XmlComponent)

            If xmlcpnt Is Nothing Then
                xmlcpnt = lvwProjectMembers.Binding.Parent
            End If

            If xmlcpnt IsNot Nothing Then
                Dim strTransformation As String = ""

                If m_xmlProject.GenerateCode(xmlcpnt, Me.Mainframe, strTransformation) _
                Then
                    Me.docvwProjectDisplay.Display = strTransformation
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuProjectMakefile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuProjectMakefile.Click
        Try
            m_xmlProject.GenerateMakefile(Me)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuOverrideProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOverrideProperties.Click
        Try
            If m_xmlProject.OverrideProperties(lvwProjectMembers.Binding.Parent) Then
                RefreshProjectDisplay()
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuOverrideMethods_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuOverrideMethods.Click
        Try
            If m_xmlProject.OverrideMethods(lvwProjectMembers.Binding.Parent) Then
                RefreshProjectDisplay()
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuExportReferences_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles mnuProjectExport.Click, mnuPackageExportReference.Click, mnuClassMemberExport.Click

        Try
            If lvwProjectMembers.SelectedItem IsNot Nothing Then
                m_xmlProject.ExportReferences(Me.Mainframe, CType(lvwProjectMembers.SelectedItem, XmlComponent))
            Else
                m_xmlProject.ExportReferences(Me.Mainframe, lvwProjectMembers.Binding.Parent)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuImportNodes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles mnuProjectImportNodes.Click, mnuPackageImportNodes.Click

        Try
            m_xmlProject.ImportNodes(Me.Mainframe, lvwProjectMembers.Binding.Parent)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuUpdateNodes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles mnuProjectUpdateNodes.Click, mnuPackageUpdateNodes.Click

        Dim myobject As XmlComponent = CType(lvwProjectMembers.SelectedItem, XmlComponent)

        If myobject Is Nothing Then
            myobject = lvwProjectMembers.Binding.Parent
        End If

        If myobject IsNot Nothing Then
            m_xmlProject.ImportNodes(Me.Mainframe, myobject, True)
        End If
    End Sub

    Private Sub mnuDependencies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles mnuRefDependencies.Click, mnuProjectDependencies.Click, _
                mnuPackageDependencies.Click, mnuClassDependencies.Click

        If lvwProjectMembers.SelectedItem IsNot Nothing Then
            'Debug.Print(CType(sender, ToolStripMenuItem).Name)
            Dim bIsEmpty As Boolean = False
            If dlgDependencies.ShowDependencies(CType(lvwProjectMembers.SelectedItem, XmlComponent), bIsEmpty) Then
                RefreshProjectDisplay()
            End If
        End If
    End Sub

    Private Sub mnuExportNodesSimpleCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles ProjectExportNodesSimpleCopy.Click, PackageExportNodesSimpleCopy.Click
        Try
            If lvwProjectMembers.SelectedItem IsNot Nothing Then
                m_xmlProject.ExportNodes(Me.Mainframe, CType(lvwProjectMembers.SelectedItem, XmlComponent))
            Else
                m_xmlProject.ExportNodes(Me.Mainframe, lvwProjectMembers.Binding.Parent)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuExportNodesExtract_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles ProjectExportNodesExtract.Click, PackageExportNodesExtract.Click
        Try
            Dim bRefresh As Boolean = False
            If lvwProjectMembers.SelectedItem IsNot Nothing Then
                bRefresh = m_xmlProject.ExportNodesExtract(Me.Mainframe, CType(lvwProjectMembers.SelectedItem, XmlComponent))
            Else
                bRefresh = m_xmlProject.ExportNodesExtract(Me.Mainframe, lvwProjectMembers.Binding.Parent)
            End If
            If bRefresh Then
                RefreshProjectDisplay()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuUpdateSimpleTypes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUpdateSimpleTypes.Click
        m_xmlProject.UpdateSimpleTypes()
    End Sub

    Private Sub DocViewMenuItem_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
                Handles DatabaseToolStripMenuItem.Click, CodeSourceToolStripMenuItem.Click, _
                UmlViewToolStripMenuItem.Click

        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Try
            docvwProjectDisplay.View = CType(sender.Tag, XmlDocumentViewMode)
            btnDocView.Text = sender.Text

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            Me.Cursor = oldCursor
        End Try
    End Sub

    Private Sub ProjectViewMenuItem_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
                Handles LargeIconsToolStripMenuItem.Click, DetailsToolStripMenuItem.Click, _
                ListToolStripMenuItem.Click, SmallIconsToolStripMenuItem.Click, _
                TileToolStripMenuItem.Click

        Try
            lvwProjectMembers.View = CType(sender.Tag, View)
            UpdateButtonProjectView()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuEditDatabase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditDatabase.Click
        MsgBox("Not yet implemented !", MsgBoxStyle.Exclamation)
    End Sub

    Private Sub mnuApplyPatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuApplyPatch.Click
        Me.Mainframe.ApplyPatch()
    End Sub

    Private Sub mnuEditDuplicate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditDuplicate.Click
        If lvwProjectMembers.DuplicateSelectedItem() Then

            RefreshProjectDisplay()
        End If
    End Sub

    Private Sub UpdatesCollaborations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdatesCollaborations.Click
        m_xmlProject.UpdatesCollaborations()
        RefreshProjectDisplay()
    End Sub

    Private Sub RenumberDatabaseIndex_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RenumberDatabaseIndex.Click
        m_xmlProject.RenumberDatabaseIndex()
        RefreshProjectDisplay()
    End Sub

    Private Sub mnuEditCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles mnuEditCopy.Click, btnCopy.Click

        lvwProjectMembers.CopySelectedItem()
        mnuEditPaste.Enabled = True
        btnPaste.Enabled = True
    End Sub

    Private Sub mnuEditPaste_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
                Handles mnuEditPaste.Click, btnPaste.Click

        If lvwProjectMembers.PasteItem() Then
            RefreshProjectDisplay()
        End If
        mnuEditPaste.Enabled = False
        btnPaste.Enabled = False
    End Sub

    Private Sub mnuEditCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles mnuEditCut.Click, btnCut.Click

        lvwProjectMembers.CutSelectedItem()
        mnuEditPaste.Enabled = True
        btnPaste.Enabled = True
    End Sub

    Private Sub btnZoomIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomIn.Click
        If docvwProjectDisplay.IncreaseTextSize() = False Then
            MouseWheelMsg()
        End If
    End Sub

    Private Sub btnZoomOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomOut.Click
        If docvwProjectDisplay.DicreaseTextSize() = False Then
            MouseWheelMsg()
        End If
    End Sub

    Private Sub MouseWheelMsg()
        MsgBox("The document view uses Internet Explorer application to display project info." + _
                vbCrLf + "But your IE version is not compatible with this command, also we invite you to use the mouse wheel." + _
                vbCrLf + vbCrLf + "To zoom out with mouse whell:" + vbCrLf + "Please click inside document view, press the key 'Ctrl' and hold down while rotate the wheel.", _
                MsgBoxStyle.Critical)

        btnZoomIn.Enabled = False
        btnZoomOut.Enabled = False
    End Sub
#End Region
End Class