Imports System
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports Microsoft.Win32
Imports ClassXmlProject.MenuItemCommand

Public Class MDIParent
    Implements InterfProgression

#Region "Class declaration"

    Private m_iChildFormNumber As Integer

    ' Internet Explorer page setup  is used for printing document
    Private Const cstPrintSetupKey As String = "Software\Microsoft\Internet Explorer\PageSetup"
    Private m_strSetupHeader As String
    Private m_strSetupFooter As String
    Private m_strCurrentFolder As String = "." + Path.DirectorySeparatorChar.ToString
    Private m_ctrlExternalTools As MenuItemCommand

#End Region

#Region "Properties"

    Public WriteOnly Property Maximum() As Integer Implements InterfProgression.Maximum
        Set(ByVal value As Integer)
            Me.strpProgressBar.Maximum = value
            Debug.Print("Maximum=" + value.ToString)
        End Set
    End Property

    Public WriteOnly Property Minimum() As Integer Implements InterfProgression.Minimum
        Set(ByVal value As Integer)
            Me.strpProgressBar.Minimum = value
            Me.strpProgressBar.Value = value
            Debug.Print("Minimum=" + value.ToString)
        End Set
    End Property

    Public WriteOnly Property ProgressBarVisible() As Boolean Implements InterfProgression.ProgressBarVisible
        Set(ByVal value As Boolean)
            Me.strpProgressBar.Visible = value
            Application.DoEvents()  ' To ose time to dispatch event
        End Set
    End Property
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
        ChildForm.OpenProject(Me, "")
        ChildForm.Show()

    End Sub

    Public Sub UpdateItemControls(ByVal xmlProject As XmlProjectView, ByVal strNodeName As String)
        SaveToolStripButton.Enabled = xmlProject.Updated
        strpStatusLabel.Text = "Language: " + xmlProject.Properties.Language + " - Node: " + strNodeName
    End Sub

    Public Sub OpenMultipleFiles(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFileOpen.Click, OpenToolStripButton.Click
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If My.Settings.CurrentFolder = m_strCurrentFolder Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.CurrentFolder
            End If

            dlgOpenFile.Title = "Select one or several project file..."
            dlgOpenFile.Filter = "UML project (*.xprj)|*.xprj|Doxygen XML index file|index.xml|UML 2.1 XMI file|*.xmi"
            dlgOpenFile.Multiselect = True

            If (dlgOpenFile.ShowDialog(Me) = DialogResult.OK) _
            Then
                For Each filename As String In dlgOpenFile.FileNames
                    OpenOneFile(filename)
                Next
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub ExitProgram(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFileExit.Click
        Me.Close()
    End Sub

    Public Sub ImportFromOmgUmlModel(Optional ByVal iMode As Integer = 1)
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If My.Settings.ImportFolder = m_strCurrentFolder Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.ImportFolder
            End If

            dlgOpenFile.Title = "Select the OMG UML XMI 2.1 file..."
            dlgOpenFile.Filter = "XMI export file|*.xml;*.xmi"

            If (dlgOpenFile.ShowDialog(Me) = DialogResult.OK) _
            Then
                Dim strTempFile As String = ""
                My.Settings.ImportFolder = XmlProjectTools.GetProjectPath(dlgOpenFile.FileName)

                XmlProjectTools.ConvertOmgUmlModel(Me, dlgOpenFile.FileName, strTempFile, iMode)

                Dim ChildForm As New frmProject
                ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                If ChildForm.OpenProject(Me, strTempFile) Then
                    ChildForm.MdiParent = Me
                    ChildForm.Text = "Project imported from XMI"
                    ChildForm.Show()
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub ImportFromDoxygenIndex()
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

                XmlProjectTools.ConvertDoxygenIndexFile(Me, dlgOpenFile.FileName, strTempFile)

                Dim ChildForm As New frmProject
                ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                If ChildForm.OpenProject(Me, strTempFile) Then
                    ChildForm.MdiParent = Me
                    ChildForm.Text = "Project imported from Doxygen"
                    ChildForm.Show()
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub ApplyPatch()
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

                If XmlProjectTools.ApplyPatch(Me, dlgOpenFile.FileName, strNewProject) Then

                    Dim ChildForm As New frmProject
                    ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                    If ChildForm.OpenProject(Me, strNewProject) Then
                        ChildForm.MdiParent = Me
                        ChildForm.Show()
                    End If
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub Increment(ByVal value As Integer) Implements InterfProgression.Increment
        Me.strpProgressBar.Increment(value)
        Application.DoEvents()  ' To lose time to dispatch event
        Debug.Print("Step=" + Me.strpProgressBar.Value.ToString)
        System.Threading.Thread.Sleep(50)
    End Sub
#End Region

#Region "Private methods"

    Private Function LoadExternalTools() As Boolean
        Try
            m_ctrlExternalTools = New MenuItemCommand(Me.ToolsMenu, Me.ExternalTools)
            m_ctrlExternalTools.LoadTools()

            While m_ctrlExternalTools.MoveNext()
                AddHandler m_ctrlExternalTools.InsertCommand(m_ctrlExternalTools.Current).Click, AddressOf ExternalTool1_Click
            End While

        Catch ex As Exception
            MsgExceptionBox(ex)
            Return False
        End Try
        Return True
    End Function

    ' Handling managed by MenuItemCommand class
    Private Sub ExternalTool1_Click(ByVal sender As ToolStripMenuItem, ByVal e As EventArgs)
        Try
            Debug.Print(sender.Tag.ToString)
            Dim item As MenuItemNode = m_ctrlExternalTools.Find(CInt(sender.Tag))
            If item IsNot Nothing Then
                Dim fen As frmProject = TryCast(Me.ActiveMdiChild, frmProject)
                If fen IsNot Nothing Then
                    fen.GenerateExternalTool(item)
                Else
                    MsgBox("Please open first a project.", MsgBoxStyle.Exclamation)
                End If
            Else
                Throw New Exception("Can't find external tool: " + sender.Text + "(Id=" + sender.Tag.ToString + ")")
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub ExternalTools_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) Handles ExternalTools.Click
        Dim fen As New dlgExternalTools
        Try

            fen.Control = m_ctrlExternalTools
            fen.ShowDialog(Me)

            If CType(fen.Tag, Boolean) = True Then
                m_ctrlExternalTools.RefreshMenu()

                While m_ctrlExternalTools.MoveNext()
                    AddHandler m_ctrlExternalTools.InsertCommand(m_ctrlExternalTools.Current).Click, AddressOf ExternalTool1_Click
                End While
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub OpenOneFile(ByVal filename As String)
        Try
            Dim bFromDoxygenIndex As Boolean = False
            Dim bFromOmgXmiFile As Boolean = False
            My.Settings.CurrentFolder = XmlProjectTools.GetProjectPath(filename)

            If Path.GetFileName(filename).ToLower = "index.xml" _
            Then
                If XmlProjectTools.ConvertDoxygenIndexFile(Me, filename, filename) = False Then
                    Exit Sub
                End If
                bFromDoxygenIndex = True
            ElseIf Path.GetExtension(filename).ToLower = ".xmi" _
            Then
                If XmlProjectTools.ConvertOmgUmlModel(Me, filename, filename) = False Then
                    Exit Sub
                End If
                bFromOmgXmiFile = True
            End If

            Dim strTempFolder = XmlProjectTools.GetProjectPath(filename)

            If XmlProjectTools.UseDocTypeDeclarationFileForProject(strTempFolder) = True Then
                Dim ChildForm As New frmProject
                ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                If ChildForm.OpenProject(Me, filename) Then

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
            MsgExceptionBox(ex)
        End Try
    End Sub

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
            XmlProjectView.InitPrototypes()

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

            Dim strTmpFolder As String = Application.LocalUserAppDataPath.ToString
            If My.Computer.FileSystem.DirectoryExists(strTmpFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(strTmpFolder)
            End If

            SaveToolStripButton.Enabled = False
            strpStatusLabel.Text = "Ready"

            If XmlProjectTools.CopyRessourcesInUserPath(strTmpFolder) = False Then

                Me.Close()

            ElseIf LoadExternalTools() = False Then

            ElseIf My.Application.CommandLineArgs.Count > 0 Then
                Dim strFilename As String = My.Application.CommandLineArgs.Item(0)
                Dim ChildForm As New frmProject
                ' Configurez-la en tant qu'enfant de ce formulaire MDI avant de l'afficher.
                If ChildForm.OpenProject(Me, strFilename) Then
                    ChildForm.MdiParent = Me
                    ChildForm.Show()
                End If
            Else
                OpenMultipleFiles(sender, e)
            End If

            Me.VbMergeToolStripOption.Checked = My.Settings.VbMergeTool


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

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dim msg As String = ""
        msg += vbCrLf + "------------------------------------------------------------------------------------------"
        If Deployment.Application.ApplicationDeployment.IsNetworkDeployed Then
            msg += vbCrLf + "- Version:               " + My.Application.Deployment.CurrentVersion.ToString
            msg += vbCrLf + "- CommonAppDataRegistry: " + Application.CommonAppDataRegistry.ToString
            msg += vbCrLf + "- CurrentCulture:        " + Application.CurrentCulture.ToString
            msg += vbCrLf + "- CurrentInputLanguage:  " + Application.CurrentInputLanguage.LayoutName
            msg += vbCrLf + "- StartupPath:           " + (Application.StartupPath)
            msg += vbCrLf + "- UserAppDataRegistry:   " + Application.UserAppDataRegistry.ToString
            msg += vbCrLf + "- LocalUserAppDataPath:  " + Application.LocalUserAppDataPath.ToString
            msg += vbCrLf + "------------------------------------------------------------------------------------------"
            msg += vbCrLf + "Computer paths:"
            msg += vbCrLf + "------------------------------------------------------------------------------------------"
            msg += vbCrLf + "- MyDocuments:           " + (My.Computer.FileSystem.SpecialDirectories.MyDocuments)
        Else
            msg += vbCrLf + "- Version:               not published"
            msg += vbCrLf + "- CommonAppDataRegistry: " + Application.CommonAppDataRegistry.ToString
            msg += vbCrLf + "- CurrentCulture:        " + Application.CurrentCulture.ToString
            msg += vbCrLf + "- CurrentInputLanguage:  " + Application.CurrentInputLanguage.LayoutName
            msg += vbCrLf + "- StartupPath:           " + (Application.StartupPath)
            msg += vbCrLf + "- UserAppDataRegistry:   " + Application.UserAppDataRegistry.ToString
            msg += vbCrLf + "- LocalUserAppDataPath:  " + Application.LocalUserAppDataPath.ToString
            msg += vbCrLf + "------------------------------------------------------------------------------------------"
            msg += vbCrLf + "Computer paths:"
            msg += vbCrLf + "------------------------------------------------------------------------------------------"
            msg += vbCrLf + "- MyDocuments:           " + (My.Computer.FileSystem.SpecialDirectories.MyDocuments)
        End If

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

    Private Sub DebugToolStripOption_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DebugToolStripOption.Click
        Me.DebugToolStripOption.Checked = Not (Me.DebugToolStripOption.Checked)
        If Me.DebugToolStripOption.Checked Then
            MsgBox("Close all projects to apply command", MsgBoxStyle.Information)
        End If
        XmlProjectTools.DEBUG_COMMANDS_ACTIVE = Me.DebugToolStripOption.Checked
    End Sub
#End Region

    Private Sub mnuFileNewOmgUmlFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileNewOmgUmlFile.Click
        ImportFromOmgUmlModel()
    End Sub
End Class
