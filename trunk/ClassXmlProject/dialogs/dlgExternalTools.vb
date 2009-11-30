Imports System
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports ClassXmlProject.MenuItemCommand
Imports ClassXmlProject.XmlProjectTools

Public Class dlgExternalTools

    Private m_xmlControl As MenuItemCommand = Nothing
    Private m_xmlBindingsList As New XmlBindingsList
    Private m_bNameChanged As Boolean = False
    Private m_bItemChanged As Boolean = False
    Private m_bInitOk As Boolean = False
    Private m_bSelectionChanges As Boolean = False

    Public WriteOnly Property Control() As MenuItemCommand
        Set(ByVal value As MenuItemCommand)
            m_xmlControl = value
        End Set
    End Property

    Private Sub dlgExternalTools_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Tag = False
            m_bInitOk = True

            m_xmlBindingsList.Init()
            If m_xmlControl.LoadTools() Then
                m_xmlControl.RefreshList(lsbExternalTools)
            Else
                Me.Close()
            End If

            UpdateLayout()

            OK_Button.Enabled = False
            btnApply.Enabled = False

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bInitOk = False
        End Try
    End Sub

    Private Sub lsbExternalTools_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsbExternalTools.SelectedIndexChanged

        Dim menuItem As MenuItemNode = TryCast(lsbExternalTools.SelectedItem, MenuItemNode)
        Try
            m_bSelectionChanges = True

            m_xmlBindingsList.UpdateValues()

            If menuItem IsNot Nothing Then
                SelectItemControl(txtName, menuItem, "Name")
                SelectItemControl(txtStylesheet, menuItem, "Stylesheet")
                SelectItemControl(txtXslParams, menuItem, "XslParams")
                SelectItemControl(txtCommand, menuItem, "Tool")
                SelectItemControl(txtArguments, menuItem, "ToolArguments")
                SelectItemControl(txtDiffTool, menuItem, "DiffTool")
                SelectItemControl(txtDiffToolArguments, menuItem, "DiffArguments")

                EnableControls(True)

                If txtCommand.Text.Length = 0 Then
                    chkCommand.Checked = False
                    chkCommand_CheckedChanged(sender, e)
                Else
                    chkCommand.Checked = True
                End If
                If txtDiffTool.Text.Length = 0 Then
                    chkDiffTool_CheckedChanged(sender, e)
                    chkDiffTool.Checked = False
                Else
                    chkDiffTool.Checked = True
                End If
            Else
                m_xmlBindingsList.RemoveBinding(txtName, "Name")
                m_xmlBindingsList.RemoveBinding(txtStylesheet, "Stylesheet")
                m_xmlBindingsList.RemoveBinding(txtXslParams, "XslParams")
                m_xmlBindingsList.RemoveBinding(txtCommand, "Tool")
                m_xmlBindingsList.RemoveBinding(txtArguments, "ToolArguments")
                m_xmlBindingsList.RemoveBinding(txtDiffTool, "DiffTool")
                m_xmlBindingsList.RemoveBinding(txtDiffToolArguments, "DiffArguments")

                EnableControls(False)
                ClearControls()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bSelectionChanges = False
        End Try
    End Sub

    Private Sub txtBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles txtArguments.TextChanged, _
            txtCommand.TextChanged, _
            txtDiffTool.TextChanged, _
            txtDiffToolArguments.TextChanged, _
            txtStylesheet.TextChanged, _
            txtXslParams.TextChanged

        If m_bInitOk Or m_bSelectionChanges Then Exit Sub

        OK_Button.Enabled = True
        btnApply.Enabled = True

        m_bItemChanged = True
    End Sub

    Private Sub txtName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.TextChanged

        If m_bInitOk Or m_bSelectionChanges Then Exit Sub

        m_bNameChanged = True

        OK_Button.Enabled = True
        btnApply.Enabled = True
    End Sub

    Private Sub txtName_Validated(ByVal sender As TextBox, ByVal e As System.EventArgs) _
            Handles txtName.Validated, txtStylesheet.Validated

        Me.errorProvider.SetError(sender, "")

    End Sub

    Private Sub txtName_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) _
            Handles txtName.Validating, txtStylesheet.Validating

        e.Cancel = False

            If m_bInitOk Or m_bSelectionChanges Then Exit Sub

            If sender.Text.Length = 0 Then
                sender.Select(0, sender.Text.Length)

                errorProvider.SetIconPadding(sender, 0)
                errorProvider.SetIconAlignment(sender, ErrorIconAlignment.BottomLeft)
                errorProvider.SetError(sender, "Name can't be empty!" + vbCrLf + "Press on left button to create one from a predefined template." + vbCrLf + "Press on right button to select exsiting one." + vbCrLf + "Press on Cancel button to quit.")
                e.Cancel = True
            End If
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If ApplyChange() Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        ApplyChange()
    End Sub

    Private Function ApplyChange() As Boolean
        Dim bResult As Boolean = True
        Try
            m_bInitOk = True
            If Me.tblpFields.Enabled And My.Computer.FileSystem.FileExists(Me.txtStylesheet.Text) = False Then
                bResult = False
            End If

            If bResult Then
                OK_Button.Enabled = False
                btnApply.Enabled = False

                m_xmlBindingsList.UpdateValues()

                If m_xmlControl.SaveTools() And m_bNameChanged Then
                    Me.Tag = True
                End If

                If m_bNameChanged Then m_xmlControl.RefreshList(lsbExternalTools)

                m_bItemChanged = False
                m_bNameChanged = False
            Else
                MsgBox("Please check if stylesheet is a valid filename:" + vbCrLf + vbCrLf + _
                       Me.txtStylesheet.Text, MsgBoxStyle.Exclamation)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bInitOk = False
        End Try
        Return bResult
    End Function

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            m_bInitOk = True
            OK_Button.Enabled = True
            btnApply.Enabled = True

            m_bItemChanged = True
            m_bNameChanged = True

            m_xmlControl.CreateCommand()
            m_xmlControl.RefreshList(lsbExternalTools, True)
            UpdateLayout()

        Finally
            m_bInitOk = False
        End Try
    End Sub

    Private Sub EnableControls(Optional ByVal bEnabled As Boolean = True)
        txtName.Enabled = bEnabled
        txtStylesheet.Enabled = bEnabled
        txtXslParams.Enabled = bEnabled
        txtCommand.Enabled = bEnabled
        txtArguments.Enabled = bEnabled
        txtDiffTool.Enabled = bEnabled
        txtDiffToolArguments.Enabled = bEnabled
        chkCommand.Enabled = bEnabled
        chkDiffTool.Enabled = bEnabled
        btnArguments.Enabled = bEnabled
        btnCommandPath.Enabled = bEnabled
        btnDiffArgs.Enabled = bEnabled
        btnDiffPath.Enabled = bEnabled
        btnStylesheetPath.Enabled = bEnabled
        btnXslParams.Enabled = bEnabled
        btnXslStylesheet.Enabled = bEnabled
    End Sub

    Private Sub ClearControls()
        txtName.Text = ""
        txtStylesheet.Text = ""
        txtXslParams.Text = ""
        txtCommand.Text = ""
        txtArguments.Text = ""
        txtDiffTool.Text = ""
        txtDiffToolArguments.Text = ""
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            Dim menuItem As MenuItemNode = TryCast(lsbExternalTools.SelectedItem, MenuItemNode)
            If menuItem IsNot Nothing _
            Then
                m_bInitOk = True
                m_xmlControl.DeleteCommand(menuItem)
                m_xmlControl.RefreshList(lsbExternalTools)
                UpdateLayout()

                OK_Button.Enabled = True
                btnApply.Enabled = True

                m_bItemChanged = True
                m_bNameChanged = True
            End If
        Finally
            m_bInitOk = False
        End Try
    End Sub

    Private Sub btnXslStylesheet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXslStylesheet.Click
        Dim tempo As String = ""
        Try
            ' This command raise an exception when filename is wrong
            tempo = Path.GetDirectoryName(txtStylesheet.Text)

            If My.Computer.FileSystem.FileExists(txtStylesheet.Text) Then
                tempo = txtStylesheet.Text
            Else
                tempo = ""
            End If
        Catch ex As Exception
            tempo = ""
        End Try
        Try
            If tempo = "" Then
                Dim dlgSaveFile As New SaveFileDialog

                If My.Settings.ExternalToolsFolder = "" _
                Then
                    dlgSaveFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                Else
                    dlgSaveFile.InitialDirectory = My.Settings.ExternalToolsFolder
                End If

                dlgSaveFile.Title = "Create a new XSL transformation file..."
                dlgSaveFile.Filter = "XSL style sheet (*.xsl)|*.xsl"

                dlgSaveFile.FileName = "NewStylesheet"

                If (dlgSaveFile.ShowDialog(Me) = DialogResult.OK) Then
                    Dim strOrg As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
                    strOrg = My.Computer.FileSystem.CombinePath(strOrg, cstXslTemplate)

                    My.Computer.FileSystem.CopyFile(strOrg, dlgSaveFile.FileName, True)
                    txtStylesheet.Text = dlgSaveFile.FileName
                    My.Settings.ExternalToolsFolder = Path.GetDirectoryName(dlgSaveFile.FileName)
                End If
            Else
                System.Diagnostics.Process.Start(tempo)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub btnCommandPath_Click(ByVal sender As Button, ByVal e As System.EventArgs) _
            Handles btnCommandPath.Click, btnDiffPath.Click, btnStylesheetPath.Click
        Try
            Dim dlgOpenFile As New OpenFileDialog
            Dim strPath = ""
            If sender Is btnCommandPath _
            Then
                strPath = txtCommand.Text

            ElseIf sender Is btnDiffPath _
            Then
                strPath = txtDiffTool.Text

            ElseIf sender Is btnStylesheetPath _
            Then
                strPath = txtStylesheet.Text
            End If

            If strPath <> "" _
            Then
                Try
                    dlgOpenFile.InitialDirectory = Path.GetDirectoryName(strPath)
                Catch ex As Exception
                    dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                End Try
            ElseIf My.Settings.ExternalToolsFolder = "" _
            Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.ExternalToolsFolder
            End If

            Select Case CInt(sender.Tag)
                Case 0
                    dlgOpenFile.Title = "Select one application file..."
                    dlgOpenFile.Filter = "Application (*.bat;*.exe)|*.bat;*.exe"
                    dlgOpenFile.Multiselect = False
                Case 1
                    dlgOpenFile.Title = "Select one XSL transformation file..."
                    dlgOpenFile.Filter = "XSL style sheet (*.xsl*)|*.xsl*"
                    dlgOpenFile.Multiselect = False
            End Select

            If (dlgOpenFile.ShowDialog(Me) = DialogResult.OK) _
            Then
                If sender Is btnCommandPath _
                Then
                    txtCommand.Text = dlgOpenFile.FileName

                ElseIf sender Is btnDiffPath _
                Then
                    txtDiffTool.Text = dlgOpenFile.FileName

                ElseIf sender Is btnStylesheetPath _
                Then
                    My.Settings.ExternalToolsFolder = Path.GetDirectoryName(dlgOpenFile.FileName)
                    txtStylesheet.Text = dlgOpenFile.FileName
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try

    End Sub

    Private Sub SelectItemControl(ByVal dataControl As Control, ByVal item As MenuItemNode, ByVal strMember As String)

        m_xmlBindingsList.RemoveBinding(dataControl, strMember)
        m_xmlBindingsList.AddBinding(dataControl, item, strMember)

    End Sub

    Private Sub btnDiffArgs_Click(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnDiffArgs.Click
        DiffArgsMnuStrip.Show(btnDiffArgs, 0, 0)
    End Sub

    Private Sub DiffArgsMnuStrip_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
        Handles CurrentElementToolStripMenuItem.Click, _
                NewElementToolStripMenuItem.Click, _
                ProjectFolderToolStripMenuItem.Click

        txtDiffToolArguments.SelectedText = CStr(sender.Tag) + " "
    End Sub

    Private Sub btnXslParams_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnXslParams.Click
        txtXslParams.SelectedText = "-Param=value "
    End Sub

    Private Sub btnArguments_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnArguments.Click
        ToolArgsMnuStrip.Show(btnArguments, 0, 0)
    End Sub

    Private Sub ToolArgsMnuStrip_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
    Handles FirstElementToolStripMenuItem.Click, _
            SecondElementToolStripMenuItem.Click, _
            NinthElementToolStripMenuItem.Click, _
            ProjectFolderToolStripMenuItem1.Click

        txtArguments.SelectedText = CStr(sender.Tag) + " "
    End Sub

    Private Sub chkCommand_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCommand.CheckedChanged
        If chkCommand.Checked = False Then
            btnCommandPath.Enabled = False
            btnArguments.Enabled = False
            txtCommand.Text = ""
            txtArguments.Text = ""
            txtCommand.Enabled = False
            txtArguments.Enabled = False
        Else
            btnCommandPath.Enabled = True
            btnArguments.Enabled = True
            txtCommand.Enabled = True
            txtArguments.Enabled = True
        End If
    End Sub

    Private Sub chkDiffTool_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDiffTool.CheckedChanged
        If chkDiffTool.Checked = False Then
            btnDiffPath.Enabled = False
            btnDiffArgs.Enabled = False
            txtDiffTool.Text = ""
            txtDiffToolArguments.Text = ""
            txtDiffTool.Enabled = False
            txtDiffToolArguments.Enabled = False
        Else
            btnDiffPath.Enabled = True
            btnDiffArgs.Enabled = True
            txtDiffTool.Enabled = True
            txtDiffToolArguments.Enabled = True
        End If
    End Sub

    Private Sub UpdateLayout()
        Me.tblpFields.Enabled = (Me.lsbExternalTools.Items.Count > 0)
        Me.btnDelete.Enabled = (Me.lsbExternalTools.Items.Count > 0)
    End Sub
End Class
