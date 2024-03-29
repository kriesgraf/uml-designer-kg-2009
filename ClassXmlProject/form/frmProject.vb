﻿Imports System
Imports System.Xml
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.MenuItemCommand

Public Class frmProject

#Region "Class declarations"

    Private m_xmlProject As New XmlProjectView
    Private m_bIsVbCodeReverse As Boolean = False

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

    Public Property IsVbCodeReverse() As Boolean
        Get
            Return m_bIsVbCodeReverse
        End Get
        Set(ByVal value As Boolean)
            m_bIsVbCodeReverse = value
        End Set
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
            Return True
        End If
        Return False
    End Function

    Public Sub SaveProject()
        If m_xmlProject.Save() = False Then
            SaveAs()
        End If
        RefreshUpdatedPath(True)
    End Sub

    Public Sub PrintPreview()
        Me.docvwProjectDisplay.ShowPrintPreviewDialog()
    End Sub

    Public Sub PrintPage()
        Me.docvwProjectDisplay.ShowPrintDialog()
    End Sub

    Public Sub GenerateExternalTool(ByVal item As MenuItemNode)
        Try
            Dim xmlcpnt As XmlComponent = CType(lvwProjectMembers.SelectedItem, XmlComponent)

            If xmlcpnt Is Nothing Then
                xmlcpnt = lvwProjectMembers.Binding.Parent
            End If


            If xmlcpnt IsNot Nothing Then
                Dim strTransformation As String = ""

                If m_xmlProject.GenerateExternalTool(xmlcpnt, item, Me.Mainframe, strTransformation) _
                Then
                    Me.docvwProjectDisplay.Display = strTransformation
                End If
            Else
                MsgBox("Select first an objet or click in an empty zone", MsgBoxStyle.Exclamation)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub
#End Region

#Region "Private methods"

    Private Sub RefreshUpdatedPath(ByVal bPathUpdated As Boolean)
        If bPathUpdated Then
            lvwProjectMembers.Binding.UpdatePath()
        End If

        If m_xmlProject.Updated Then
            Me.Text = m_xmlProject.Filename + " - " + lvwProjectMembers.Path + " *"
        Else
            Me.Text = m_xmlProject.Filename + " - " + lvwProjectMembers.Path
        End If
        Dim strNodeName As String = m_xmlProject.Name
        If lvwProjectMembers.SelectedItem IsNot Nothing Then
            strNodeName = CType(lvwProjectMembers.SelectedItem, XmlProjectMemberView).Label(0)
        End If

        Me.Mainframe.UpdateItemControls(m_xmlProject, strNodeName)
    End Sub

    Private Sub RefreshProjectView(ByVal component As Object)
        Dim oldCursor As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            docvwProjectDisplay.DataSource = CType(component, XmlComponent).Node

        Finally
            Me.Cursor = oldCursor
        End Try
    End Sub

    Private Sub RefreshButtonsListView()
        btnHome.Enabled = lvwProjectMembers.Binding.IsNotHome()
        btnUp.Enabled = btnHome.Enabled
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
                MsgBox("The filename was not valid, we propose to rename:" + vbCrLf + strFilename, "'Save as' command")
            End If
            dlgSaveFile.FileName = strFilename

            If (dlgSaveFile.ShowDialog(Me) = DialogResult.OK) Then
                My.Settings.CurrentFolder = XmlProjectTools.GetProjectPath(dlgSaveFile.FileName)
                My.Settings.CurrentProject = dlgSaveFile.FileName
                m_xmlProject.SaveAs(dlgSaveFile.FileName)
                RefreshUpdatedPath(True)
                mnuFileSave.Enabled = False
                Return True
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return False
    End Function

    Private Sub MouseWheelMsg()
        MsgBox("The document view uses Internet Explorer application to display project info." + _
                vbCrLf + "But your IE version is not compatible with this command, also we invite you to use the mouse wheel." + _
                vbCrLf + vbCrLf + "To zoom out with mouse wheel:" + vbCrLf + "Please click inside document view, press the key 'Ctrl' and hold down while rotate the wheel.", _
                MsgBoxStyle.Critical, "'Zoom' commands")

        btnZoomIn.Enabled = False
        btnZoomOut.Enabled = False
    End Sub

#End Region

#Region "Private event methods"

    Private Sub frmProject_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        mnuEditPaste.Enabled = XmlComponent.Clipboard.CanPaste
        btnPaste.Enabled = mnuEditPaste.Enabled

        RefreshUpdatedPath(False)
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

        Me.WindowState = FormWindowState.Maximized
        Try
            If m_xmlProject.IsNew _
            Then
                ' We call project by its form name at beginning
                m_xmlProject.Updated = True
                mnuFileSave.Enabled = False
            End If

            With m_xmlProject
                .ListViewControl = lvwProjectMembers

                ' Specific option relative Java language
                If m_xmlProject.Properties.GenerationLanguage = ELanguage.Language_Java Then
                    mnuAddClass.Visible = False
                    mnuPackageAddPackage.Visible = False
                    mnuPackageAddImport.Visible = False
                    AddClassImport.Visible = False
                End If

                .AddMenuProject(mnuProjectList)
                .AddMenuPackage(mnuPackageList)
                .AddMenuClass(mnuClassMembers)
                .AddMenuImport(mnuEditReference)
                .LoadMembers()

                docvwProjectDisplay.Language = .Properties.GenerationLanguage
                ' change display of docvwProjectDisplay & btnDocView in one shot
                DocViewMenuItem_Click(UmlViewToolStripMenuItem, Nothing)

                If Me.IsVbCodeReverse Then

                    If m_xmlProject.SortImportReferences() Then
                        lvwProjectMembers.Binding.ResetBindings(True)
                        RefreshUpdatedPath(False)
                    End If
                ElseIf .IsNew Then

                    RefreshProjectView(lvwProjectMembers.Binding.Parent)
                    RefreshUpdatedPath(True)
                    mnuProjectProperties_Click(Me, Nothing)
                Else
                    lvwProjectMembers.SelectItem(0)
                    If lvwProjectMembers.SelectedItem IsNot Nothing _
                    Then
                        RefreshProjectView(lvwProjectMembers.SelectedItem)
                    Else
                        RefreshProjectView(lvwProjectMembers.Binding.Parent)
                    End If
                    RefreshUpdatedPath(True)
                End If

                .UpdateMenuClass(lvwProjectMembers.Binding.Parent, AddClassTypedef, AddClassConstructor)
            End With

            RefreshButtonsListView()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub frmProject_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If m_xmlProject.IsNew _
            Then
                If m_xmlProject.Updated Then

                    Select MsgBox("Would you want to save updates ?", cstMsgYesNoCancelExclamation, m_xmlProject.Name)
                        Case MsgBoxResult.Yes
                            Dim bContinue As Boolean = True
                            While bContinue
                                If SaveAs() = False Then
                                    If MsgBox("Retry to save this project ?", vbRetryCancel + vbQuestion + vbDefaultButton2, m_xmlProject.Name) = vbCancel Then
                                        bContinue = False
                                    End If
                                Else
                                    bContinue = False
                                End If
                            End While
                            m_xmlProject.Updated = False
                            e.Cancel = False

                        Case MsgBoxResult.Cancel
                            e.Cancel = True

                        Case Else
                            m_xmlProject.Updated = False
                            e.Cancel = False
                    End Select
                End If
            ElseIf m_xmlProject.Updated _
            Then
                Select Case MsgBox("Would you want to save updates ?", cstMsgYesNoCancelExclamation, m_xmlProject.Filename)
                    Case MsgBoxResult.Yes
                        m_xmlProject.Save()
                        m_xmlProject.Updated = False
                        e.Cancel = False

                    Case MsgBoxResult.Cancel
                        e.Cancel = True

                    Case Else
                        m_xmlProject.Updated = False
                        e.Cancel = False
                End Select
            End If

            RefreshUpdatedPath(False)

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
        RefreshProjectView(lvwProjectMembers.Binding.Parent)
        RefreshUpdatedPath(False)
    End Sub

    Private Sub lvwProjectMembers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwProjectMembers.SelectedIndexChanged
        Try
            If lvwProjectMembers.SelectedItem IsNot Nothing _
            Then
                RefreshProjectView(lvwProjectMembers.SelectedItem)
            Else
                RefreshProjectView(lvwProjectMembers.Binding.Parent)
            End If
            RefreshUpdatedPath(False)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub lvwProjectMembers_ChangeLevel(ByVal sender As Object, ByVal e As DataListViewEventArgs) _
                Handles lvwProjectMembers.GoHomeLevel, _
                        lvwProjectMembers.GoParentLevel, _
                        lvwProjectMembers.GoChildLevel
        Try
            RefreshUpdatedPath(True)
            RefreshProjectView(lvwProjectMembers.Binding.Parent)
            m_xmlProject.UpdateMenuClass(lvwProjectMembers.Binding.Parent, AddClassTypedef, AddClassConstructor)
            RefreshButtonsListView()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub lvwProjectMembers_ItemChanged(ByVal sender As Object, ByVal e As DataListViewEventArgs) Handles lvwProjectMembers.ItemChanged
        Try
            ' TODO: for future use
            RefreshUpdatedPath(False)

            If lvwProjectMembers.SelectedItem IsNot Nothing _
            Then
                RefreshProjectView(lvwProjectMembers.SelectedItem)
            Else
                RefreshProjectView(lvwProjectMembers.Binding.Parent)
            End If

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
        RefreshUpdatedPath(True)
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
            m_xmlProject.Updated = True
            RefreshUpdatedPath(True)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuProjectEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles mnuProjectEdit.Click, mnuPackageEdit.Click, mnuClassMemberEdit.Click, EditReference.Click

        Try
            If lvwProjectMembers.EditCurrentItem() _
            Then
                RefreshUpdatedPath(False)
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
                RefreshUpdatedPath(False)
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
                lvwProjectMembers.Binding.ResetBindings(True)
                RefreshUpdatedPath(True)
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuProjectParameters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuProjectParameters.Click
        Try
            If m_xmlProject.EditParameters() Then
                m_xmlProject.UpdateMenuClass(lvwProjectMembers.Binding.Parent, AddClassTypedef, AddClassConstructor)
                RefreshUpdatedPath(True)
                docvwProjectDisplay.Language = m_xmlProject.Properties.GenerationLanguage
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuPackageMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                Handles mnuPackageMoveUp.Click, _
                mnuImportMoveUp.Click
        Try
            With lvwProjectMembers
                If m_xmlProject.MoveUpNode(CType(.Binding.Parent, XmlProjectMemberView), CType(.SelectedItem, XmlComponent)) Then
                    RefreshProjectView(.Binding.Parent)
                    RefreshUpdatedPath(False)
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
            RefreshUpdatedPath(False)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuConvertNode_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) Handles ConvertToReference.Click

        Try
            m_xmlProject.ConvertImportElement(lvwProjectMembers.Binding.Parent, lvwProjectMembers.SelectedItem)
            RefreshUpdatedPath(False)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuImportExport_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
        Handles mnuReplaceExport.Click, mnuMergeExport.Click, mnuConfirmExport.Click
        Try
            If m_xmlProject.AddReferences(Me.Mainframe, lvwProjectMembers.Binding.Parent, CType(sender.Tag, XmlImportSpec.EImportMode)) _
            Then
                RefreshUpdatedPath(False)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuFindRedundant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles mnuProjectRedundancies.Click, _
                    mnuPackageRedundancies.Click, _
                    mnuClassRedundancies.Click, _
                    FindRedundant.Click
        Try
            With lvwProjectMembers
                If m_xmlProject.RemoveRedundantReference(.Binding.Parent, CType(.SelectedItem, XmlComponent)) Then
                    RefreshProjectView(.Binding.Parent)
                    RefreshUpdatedPath(False)
                End If
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveAll.Click
        Try
            If m_xmlProject.RemoveAllReferences(lvwProjectMembers.Binding.Parent) _
            Then
                RefreshProjectView(lvwProjectMembers.Binding.Parent)
                RefreshUpdatedPath(False)
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
                RefreshUpdatedPath(False)
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuOverrideMethods_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuOverrideMethods.Click
        Try
            If m_xmlProject.OverrideMethods(lvwProjectMembers.Binding.Parent) Then
                RefreshUpdatedPath(False)
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

        'Debug.Print(CType(sender, ToolStripMenuItem).Name)
        If m_xmlProject.SearchDependencies(CType(lvwProjectMembers.SelectedItem, XmlComponent)) Then
            RefreshUpdatedPath(False)
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
                ' Needs to go back why curent children change totaly
                lvwProjectMembers.GoBack()
                lvwProjectMembers.Binding.ResetBindings(True)
            End If
            If bRefresh Then
                RefreshUpdatedPath(False)
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
            Dim eView As View = CType(sender.Tag, View)
            lvwProjectMembers.View = eView
            btnProjectView.Text = btnProjectView.DropDownItems(eView).Text

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

            RefreshUpdatedPath(False)
        End If
    End Sub

    Private Sub UpdatesCollaborations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdatesCollaborations.Click
        m_xmlProject.UpdatesCollaborations()
        RefreshUpdatedPath(False)
    End Sub

    Private Sub RenumberDatabaseIndex_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RenumberDatabaseIndex.Click
        m_xmlProject.RenumberDatabaseIndex()
        RefreshUpdatedPath(False)
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
            RefreshUpdatedPath(False)
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

    Private Sub mnuProjectImportReferences_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles mnuProjectImportReferences.Click, mnuPackageImportReference.Click
        Try
            If m_xmlProject.ImportReferences(Me.Mainframe, lvwProjectMembers.Binding.Parent) Then
                RefreshUpdatedPath(True)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuUpdatePrefixNames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUpdatePrefixNames.Click
        m_xmlProject.UpdatePrefixNames()
    End Sub

    Private Sub mnuFileExportOmgUmlFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileExportOmgUmlFile.Click
        Try
            Dim dlgSaveFile As New SaveFileDialog

            If My.Settings.ExportFolder = "." + Path.DirectorySeparatorChar.ToString Then
                dlgSaveFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgSaveFile.InitialDirectory = My.Settings.ExportFolder
            End If

            dlgSaveFile.Title = "Export to OMG UML XMI 2.1 file..."
            dlgSaveFile.Filter = "OMG UML XMI 2.1 model (*.xmi)|*.xmi"

            Dim strFilename As String = m_xmlProject.Name
            If XmlProjectTools.GetValidFilename(strFilename) Then
                MsgBox("The filename was not valid, we propose to rename:" + vbCrLf + strFilename, "'Save as' command")
            End If
            dlgSaveFile.FileName = strFilename

            If (dlgSaveFile.ShowDialog(Me) = DialogResult.OK) Then
                My.Settings.ExportFolder = XmlProjectTools.GetProjectPath(dlgSaveFile.FileName)
                m_xmlProject.ExportOmgUmlFile(Me.ParentForm, dlgSaveFile.FileName)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuFileNewOmgUmlFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileNewOmgUmlFile.Click
        Me.Mainframe.ImportFromOmgUmlModel()
    End Sub

    Private Sub VbCodeReverse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VbCodeReverse.Click
        Me.Mainframe.ImportFromVbCodeSource()
    End Sub

    Private Sub mnuTrimComments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuTrimComments.Click
        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Try
            m_xmlProject.TrimComments()
            RefreshProjectView(lvwProjectMembers.Binding.Parent)
            RefreshUpdatedPath(False)

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            Me.Cursor = oldCursor
        End Try
    End Sub

    Private Sub mnuExchangeImports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles mnuProjectExchangeImports.Click, mnuPackageExchangeImports.Click

        Try
            If m_xmlProject.ExchangeImports(lvwProjectMembers.SelectedItem) Then
                lvwProjectMembers.Binding.ResetBindings(True)
                RefreshUpdatedPath(False)
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuProjectConvertExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles mnuProjectConvertExport.Click, mnuPackageConvertExport.Click
        Try
            If lvwProjectMembers.SelectedItem Is Nothing Then
                If m_xmlProject.ConvertToImportNodes(Me.Mainframe, lvwProjectMembers.Binding.Parent) Then
                    ' Needs to go back why curent children change totaly
                    lvwProjectMembers.GoBack()
                    lvwProjectMembers.Binding.ResetBindings(True)
                    RefreshUpdatedPath(False)
                End If
            ElseIf m_xmlProject.ConvertToImportNodes(Me.Mainframe, lvwProjectMembers.SelectedItem) Then
                lvwProjectMembers.Binding.ResetBindings(True)
                RefreshUpdatedPath(False)
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub
#End Region
End Class