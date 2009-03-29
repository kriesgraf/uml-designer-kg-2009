Imports System
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Microsoft.Win32

Public Class MDIParent

#Region "Class declaration"

    Private m_iChildFormNumber As Integer

    ' Internet Explorer page setup  is used for printing document
    Private Const cstPrintSetupKey As String = "Software\Microsoft\Internet Explorer\PageSetup"
    Private m_strSetupHeader As String
    Private m_strSetupFooter As String
    Private m_strCurrentFolder As String = "." + Path.DirectorySeparatorChar.ToString

#End Region

#Region "Public methods"

    Public Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFileNew.Click, NewToolStripButton.Click
        ' Créez une nouvelle instance du formulaire enfant.
        Dim ChildForm As New frmProject
        ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
        ChildForm.MdiParent = Me

        m_iChildFormNumber += 1
        ChildForm.Text = "New project " & m_iChildFormNumber
        ' this property instantiate the view component
        ChildForm.ProjectName = ""
        ChildForm.Show()

    End Sub

    Public Sub UpdateItemControls(ByVal xmLProject As XmlProjectView)
        SaveToolStripButton.Enabled = xmLProject.Updated
        strpStatusLabel.Text = "Language: " + xmLProject.Properties.Language
    End Sub

    Public Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFileOpen.Click, OpenToolStripButton.Click
        Dim oldCursor As Cursor = Me.Cursor
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If My.Settings.CurrentFolder = m_strCurrentFolder Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.CurrentFolder
            End If

            dlgOpenFile.Title = "Select a project file..."
            dlgOpenFile.Filter = "UML project (*.xprj)|*.xprj|Doxygen XML index file|index.xml|UML 2.1 XMI file|*.xmi"

            If (dlgOpenFile.ShowDialog(Me) = DialogResult.OK) _
            Then
                Dim FileName As String = dlgOpenFile.FileName
                Dim bFromDoxygenIndex As Boolean = False
                Dim bFromOmgXmiFile As Boolean = False
                My.Settings.CurrentFolder = XmlProjectTools.GetProjectPath(FileName)

                If Path.GetFileName(FileName).ToLower = "index.xml" _
                Then
                    Me.Cursor = Cursors.WaitCursor
                    If XmlProjectTools.ConvertDoxygenIndexFile(FileName, FileName) = False Then
                        Me.Cursor = oldCursor
                        Exit Sub
                    End If
                    Me.Cursor = oldCursor
                    bFromDoxygenIndex = True
                ElseIf Path.GetExtension(FileName).ToLower = ".xmi" _
                Then
                    Me.Cursor = Cursors.WaitCursor
                    If XmlProjectTools.ConvertOmgUmlModel(FileName, FileName) = False Then
                        Me.Cursor = oldCursor
                        Exit Sub
                    End If
                    Me.Cursor = oldCursor
                    bFromOmgXmiFile = True
                End If

                Dim strTempFolder = XmlProjectTools.GetProjectPath(FileName)

                If XmlProjectTools.UseDocTypeDeclarationFileForProject(strTempFolder) = True Then
                    Dim ChildForm As New frmProject
                    ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                    ChildForm.ProjectName = FileName
                    ChildForm.MdiParent = Me
                    If bFromDoxygenIndex Then
                        ChildForm.Text = "Project imported from Doxygen"
                    ElseIf bFromOmgXmiFile Then
                        ChildForm.Text = "Project imported from XMI"
                    End If
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
        Dim oldCursor As Cursor = Me.Cursor
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If My.Settings.CurrentFolder = m_strCurrentFolder Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.CurrentFolder
            End If

            dlgOpenFile.Title = "Select the OMG UML 2.1 XMI file..."
            dlgOpenFile.Filter = "UML 2.1 XMI file|*.xml;*.xmi"

            If (dlgOpenFile.ShowDialog(Me) = DialogResult.OK) _
            Then
                Dim strTempFile As String = ""
                My.Settings.CurrentFolder = XmlProjectTools.GetProjectPath(dlgOpenFile.FileName)

                Me.Cursor = Cursors.WaitCursor
                XmlProjectTools.ConvertOmgUmlModel(dlgOpenFile.FileName, strTempFile)

                Me.Cursor = oldCursor

                Dim ChildForm As New frmProject
                ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                ChildForm.ProjectName = strTempFile
                ChildForm.MdiParent = Me
                ChildForm.Text = "Project imported from XMI"
                ChildForm.Show()
            End If
        Catch ex As Exception
            Me.Cursor = oldCursor
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub ImportFromDoxygenIndex()
        Dim oldCursor As Cursor = Me.Cursor
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If My.Settings.CurrentFolder = m_strCurrentFolder Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.CurrentFolder
            End If

            dlgOpenFile.Title = "Select the Doxygen index file..."
            dlgOpenFile.Filter = "Doxygen XML index file|index.xml"

            If (dlgOpenFile.ShowDialog(Me) = DialogResult.OK) _
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
                ChildForm.Text = "Project imported from Doxygen"
                ChildForm.Show()
            End If
        Catch ex As Exception
            Me.Cursor = oldCursor
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub ApplyPatch()
        Dim oldCursor As Cursor = Me.Cursor
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If My.Settings.CurrentFolder = m_strCurrentFolder Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.CurrentFolder
            End If

            dlgOpenFile.Title = "Select a project file..."
            dlgOpenFile.Filter = "UML project (*.xprj)|*.xprj"

            If (dlgOpenFile.ShowDialog(Me) = DialogResult.OK) _
            Then
                Dim strNewProject As String = ""
                My.Settings.CurrentFolder = XmlProjectTools.GetProjectPath(dlgOpenFile.FileName)

                Me.Cursor = Cursors.WaitCursor

                If XmlProjectTools.ApplyPatch(Me, dlgOpenFile.FileName, strNewProject) Then

                    Me.Cursor = oldCursor

                    Dim ChildForm As New frmProject
                    ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                    ChildForm.ProjectName = strNewProject
                    ChildForm.MdiParent = Me
                    ChildForm.Show()
                Else
                    Me.Cursor = oldCursor
                End If
            End If
        Catch ex As Exception
            Me.Cursor = oldCursor
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

        Dim key As RegistryKey = Registry.CurrentUser.OpenSubKey(cstPrintSetupKey, True)
        Try
            If key IsNot Nothing Then
                key.SetValue("footer", m_strSetupFooter)
                key.SetValue("header", m_strSetupHeader)
            End If
        Catch ex As Exception
        Finally
            If key IsNot Nothing Then
                key.Close()
            End If
        End Try
        ' Dereference all objects here 
        XmlNodeManager.Destroy()
    End Sub

    Private Sub MDIParent_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim key As RegistryKey = Registry.CurrentUser.OpenSubKey(cstPrintSetupKey)
        Try
            If key IsNot Nothing Then
                m_strSetupFooter = key.GetValue("footer").ToString
                m_strSetupHeader = key.GetValue("header").ToString
            Else
                m_strSetupFooter = "&u&bPage &p sur &P"
                m_strSetupHeader = "&w&b&d"
            End If
        Catch ex As Exception
        Finally
            If key IsNot Nothing Then
                key.Close()
            End If
        End Try

        Try
            If Deployment.Application.ApplicationDeployment.IsNetworkDeployed Then
                Me.Text = Me.Text + " - " + My.Application.Deployment.CurrentVersion.ToString
            Else
                Me.Text = Me.Text + " - " + "Version not published"
            End If

            Dim strTmpFolder As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData
            If My.Computer.FileSystem.DirectoryExists(strTmpFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(strTmpFolder)
            End If

            If XmlProjectTools.CopyDocTypeDeclarationFile(strTmpFolder, True) = False Then

                Me.Close()

            ElseIf My.Application.CommandLineArgs.Count > 0 Then
                Dim strFilename As String = My.Application.CommandLineArgs.Item(0)
                Dim ChildForm As New frmProject
                ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                ChildForm.ProjectName = strFilename
                ChildForm.MdiParent = Me
                ChildForm.Show()
            Else
                OpenFile(sender, e)
            End If

            Me.VbMergeToolStripOption.Checked = My.Settings.VbMergeTool


        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuFileNewDoxygenFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileNewDoxygenFile.Click
        ImportFromDoxygenIndex()
    End Sub

    Private Sub mnuFileNewOmgUmlFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuFileNewOmgUmlFile.Click
        ImportFromOmgUmlModel()
    End Sub

    Private Sub mnuPatchApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPatchApply.Click
        ApplyPatch()
    End Sub

    Private Sub mnuEditDatabase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditDatabase.Click
        MsgBox("Not yet implemented !", MsgBoxStyle.Exclamation)
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dim msg As String = ""
        msg += vbCrLf + "------------------------------------------------------------------------------------------"
        msg += vbCrLf + "Application paths and declarations:"
        msg += vbCrLf + "------------------------------------------------------------------------------------------"
        msg += vbCrLf + "- CommonAppDataPath:    " + (Application.CommonAppDataPath)
        msg += vbCrLf + "- CommonAppDataRegistry:" + Application.CommonAppDataRegistry.ToString
        msg += vbCrLf + "- CurrentCulture:       " + Application.CurrentCulture.ToString
        msg += vbCrLf + "- CurrentInputLanguage: " + Application.CurrentInputLanguage.LayoutName
        msg += vbCrLf + "- ExecutablePath:       " + (Application.ExecutablePath)
        msg += vbCrLf + "- LocalUserAppDataPath: " + (Application.LocalUserAppDataPath)
        msg += vbCrLf + "- StartupPath:          " + (Application.StartupPath)
        msg += vbCrLf + "- UserAppDataPath:      " + (Application.UserAppDataPath)
        msg += vbCrLf + "- UserAppDataRegistry:  " + Application.UserAppDataRegistry.ToString
        msg += vbCrLf + "------------------------------------------------------------------------------------------"
        msg += vbCrLf + "Computer paths:"
        msg += vbCrLf + "------------------------------------------------------------------------------------------"
        msg += vbCrLf + "- CurrentUserApplicationData: " + (My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData)
        msg += vbCrLf + "- AllUsersApplicationData:    " + (My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData)
        msg += vbCrLf + "- MyDocuments:                " + (My.Computer.FileSystem.SpecialDirectories.MyDocuments)

        Dim form As New dlgAboutBox

        form.Message = msg
        form.ShowDialog()
    End Sub

    Private Sub ContentsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ContentsToolStripMenuItem.Click
        ' Navigate to a URL.
        System.Diagnostics.Process.Start("http://code.google.com/p/uml-designer-kg-2009/wiki/Getting_started")
    End Sub

    Private Sub IndexToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IndexToolStripMenuItem.Click
        ' Navigate to a URL.
        System.Diagnostics.Process.Start("http://code.google.com/p/uml-designer-kg-2009/")
    End Sub

    Private Sub SearchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchToolStripMenuItem.Click
        ' Navigate to a URL.
        System.Diagnostics.Process.Start("http://code.google.com/p/uml-designer-kg-2009/w/list")
    End Sub

    Private Sub HelpToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripButton.Click
        ' Navigate to a URL.
        System.Diagnostics.Process.Start("http://code.google.com/p/uml-designer-kg-2009/")
    End Sub

    Private Sub DiffToolStripOption_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiffToolStripOption.Click
        Dim fen As Form = New dlgDiffTool
        fen.ShowDialog()
    End Sub

    Private Sub VbMergeToolStripOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles VbMergeToolStripOption.Click
        My.Settings.VbMergeTool = Not (My.Settings.VbMergeTool)
        Me.VbMergeToolStripOption.Checked = My.Settings.VbMergeTool
    End Sub
#End Region
End Class
