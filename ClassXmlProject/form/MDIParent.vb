Imports System.Windows.Forms

Public Class MDIParent

#Region "Class declaration"

    Private m_ChildFormNumber As Integer

#End Region

#Region "Public methods"

    Public Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFileNew.Click, NewToolStripButton.Click, NewWindowToolStripMenuItem.Click
        ' Créez une nouvelle instance du formulaire enfant.
        Dim ChildForm As New frmProject
        ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
        ChildForm.MdiParent = Me

        m_ChildFormNumber += 1
        ChildForm.Text = "New project " & m_ChildFormNumber
        ' this property instantiate the view component
        ChildForm.ProjectName = ""
        ChildForm.Show()

    End Sub

    Public Sub UpdateItemControls(ByVal xmLProject As XmlProjectView)
        SaveToolStripButton.Enabled = xmLProject.Updated
    End Sub

    Public Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFileOpen.Click, OpenToolStripButton.Click
        Dim oldCursor As Cursor = Me.Cursor
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If My.Settings.CurrentFolder = ".\" Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.CurrentFolder
            End If

            dlgOpenFile.Title = "Select a project file..."
            dlgOpenFile.Filter = "UML project (*.xprj)|*.xprj|Doxygen XML index file|index.xml"

            If (dlgOpenFile.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) _
            Then
                Dim FileName As String = dlgOpenFile.FileName
                Dim bFromDoxygenIndex As Boolean = False
                My.Settings.CurrentFolder = XmlProjectTools.GetProjectPath(FileName)

                If dlgOpenFile.SafeFileName = "index.xml" Then
                    Me.Cursor = Cursors.WaitCursor
                    XmlProjectTools.ConvertDoxygenIndexFile(dlgOpenFile.FileName, FileName)
                    Me.Cursor = oldCursor
                    bFromDoxygenIndex = True
                End If

                Dim strTempFolder = XmlProjectTools.GetProjectPath(FileName)

                If XmlProjectTools.UseDocTypeDeclarationFileForProject(strTempFolder) = True Then
                    Dim ChildForm As New frmProject
                    ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                    ChildForm.ProjectName = FileName
                    ChildForm.MdiParent = Me
                    If bFromDoxygenIndex Then ChildForm.Text = "From Doxygen index"
                    ChildForm.Show()
                End If
            End If
        Catch ex As Exception
            Me.Cursor = oldCursor
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub ExitProgram(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFileExit.Click
        Me.Close()
    End Sub

    Public Sub ImportFromOmgUmlModel()

    End Sub

    Public Sub ImportFromDoxygenIndex()
        Dim oldCursor As Cursor = Me.Cursor
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If My.Settings.CurrentFolder = ".\" Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.CurrentFolder
            End If

            dlgOpenFile.Title = "Select the Doxygen index file..."
            dlgOpenFile.Filter = "Doxygen XML index file|index.xml"

            If (dlgOpenFile.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) _
            Then
                Dim strTempFile As String = ""
                My.Settings.CurrentFolder = XmlProjectTools.GetProjectPath(dlgOpenFile.FileName)

                Me.Cursor = Cursors.WaitCursor
                XmlProjectTools.ConvertDoxygenIndexFile(dlgOpenFile.FileName, strTempFile)

                Me.Cursor = oldCursor

                Dim ChildForm As New frmProject
                ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                ChildForm.ProjectName = strTempFile
                ChildForm.MdiParent = Me
                ChildForm.Text = "From Doxygen index"
                ChildForm.Show()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            Me.Cursor = oldCursor
        End Try
    End Sub

    Public Sub ApplyPatch()
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If My.Settings.CurrentFolder = ".\" Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.CurrentFolder
            End If

            dlgOpenFile.Title = "Select a project file..."
            dlgOpenFile.Filter = "UML project (*.xprj)|*.xprj"

            If (dlgOpenFile.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) _
            Then
                Dim strNewProject As String = ""
                My.Settings.CurrentFolder = XmlProjectTools.GetProjectPath(dlgOpenFile.FileName)

                If XmlProjectTools.ApplyPatch(Me, dlgOpenFile.FileName, strNewProject) Then

                    Dim ChildForm As New frmProject
                    ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                    ChildForm.ProjectName = strNewProject
                    ChildForm.MdiParent = Me
                    ChildForm.Show()
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub
#End Region

#Region "Private methods"

    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        Me.ToolStrip.Visible = Me.ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        Me.StatusStrip.Visible = Me.StatusBarToolStripMenuItem.Checked
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Fermez tous les formulaires enfants du parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    Private Sub PrintPreviewToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripButton.Click
        ' TODO
        Dim fen As frmProject = TryCast(Me.ActiveMdiChild, frmProject)

        If fen IsNot Nothing Then
            fen.PrintPreview()
        End If
    End Sub

    Public Sub PrintSetup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilePrintSetup.Click
        MsgBox("Not yet implemented !", MsgBoxStyle.Exclamation)
    End Sub

    Private Sub PrintToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PrintToolStripButton.Click
        ' TODO
        Dim fen As frmProject = TryCast(Me.ActiveMdiChild, frmProject)

        If fen IsNot Nothing Then
            fen.PrintPage()
        End If
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        ' TODO
    End Sub

    Private Sub SaveToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripButton.Click
        ' TODO
        Dim fen As frmProject = TryCast(Me.ActiveMdiChild, frmProject)

        If fen IsNot Nothing Then
            fen.SaveProject()
        End If
    End Sub

    Private Sub MDIParent_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Dereference all objects here 
        XmlNodeManager.Destroy()
    End Sub

    Private Sub MDIParent_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim strTmpFolder As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData
            If My.Computer.FileSystem.DirectoryExists(strTmpFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(strTmpFolder)
            End If

            XmlProjectTools.CopyDocTypeDeclarationFile(strTmpFolder, False)

            If My.Application.CommandLineArgs.Count > 0 Then
                Dim strFilename As String = My.Application.CommandLineArgs.Item(0)
                Dim ChildForm As New frmProject
                ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                ChildForm.ProjectName = strFilename
                ChildForm.MdiParent = Me
                ChildForm.Show()
            Else
                OpenFile(sender, e)
            End If


        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuFileNewDoxygenFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileNewDoxygenFile.Click
        ImportFromDoxygenIndex()
    End Sub

    Private Sub mnuPatchApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPatchApply.Click
        ApplyPatch()
    End Sub

    Private Sub mnuEditDatabase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditDatabase.Click
        MsgBox("Not yet implemented !", MsgBoxStyle.Exclamation)
    End Sub
#End Region

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox(Application.StartupPath, MsgBoxStyle.Information)
    End Sub
End Class
