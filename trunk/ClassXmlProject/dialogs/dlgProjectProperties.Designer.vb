<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgProjectProperties
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbLanguage = New System.Windows.Forms.ComboBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtDetails = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.fileListBox = New Microsoft.VisualBasic.Compatibility.VB6.FileListBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtBrief = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnPath = New System.Windows.Forms.Button
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.CausesValidation = False
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(6, 347)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.CausesValidation = False
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.txtPath, 1, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.cmbLanguage, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.txtName, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.txtDetails, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.Label5, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.fileListBox, 1, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.Label6, 0, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.txtBrief, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label4, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.btnPath, 0, 4)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 6
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 156.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(443, 338)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'txtPath
        '
        Me.txtPath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPath.Location = New System.Drawing.Point(93, 152)
        Me.txtPath.Margin = New System.Windows.Forms.Padding(3, 12, 3, 3)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(347, 20)
        Me.txtPath.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label1.Location = New System.Drawing.Point(15, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 28)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Project name:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label2.Location = New System.Drawing.Point(5, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 28)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Code language:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbLanguage
        '
        Me.cmbLanguage.Dock = System.Windows.Forms.DockStyle.Left
        Me.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLanguage.FormattingEnabled = True
        Me.cmbLanguage.Location = New System.Drawing.Point(93, 31)
        Me.cmbLanguage.Name = "cmbLanguage"
        Me.cmbLanguage.Size = New System.Drawing.Size(174, 21)
        Me.cmbLanguage.TabIndex = 2
        '
        'txtName
        '
        Me.txtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtName.Location = New System.Drawing.Point(93, 3)
        Me.txtName.Margin = New System.Windows.Forms.Padding(3, 3, 20, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(330, 20)
        Me.txtName.TabIndex = 3
        '
        'txtDetails
        '
        Me.txtDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDetails.Location = New System.Drawing.Point(93, 87)
        Me.txtDetails.Multiline = True
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.Size = New System.Drawing.Size(347, 50)
        Me.txtDetails.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label5.Location = New System.Drawing.Point(34, 84)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 56)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Details comment:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'fileListBox
        '
        Me.fileListBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.fileListBox.FormattingEnabled = True
        Me.fileListBox.Location = New System.Drawing.Point(93, 185)
        Me.fileListBox.Name = "fileListBox"
        Me.fileListBox.Pattern = "*.*"
        Me.fileListBox.Size = New System.Drawing.Size(347, 147)
        Me.fileListBox.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label6.Location = New System.Drawing.Point(3, 182)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 156)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Source location content:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtBrief
        '
        Me.txtBrief.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBrief.Location = New System.Drawing.Point(93, 59)
        Me.txtBrief.Name = "txtBrief"
        Me.txtBrief.Size = New System.Drawing.Size(347, 20)
        Me.txtBrief.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label4.Location = New System.Drawing.Point(10, 56)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 28)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Brief comment:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnPath
        '
        Me.btnPath.CausesValidation = False
        Me.btnPath.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnPath.Location = New System.Drawing.Point(3, 143)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(84, 36)
        Me.btnPath.TabIndex = 14
        Me.btnPath.Text = "Sources location"
        Me.btnPath.UseVisualStyleBackColor = True
        '
        'errorProvider
        '
        Me.errorProvider.ContainerControl = Me
        '
        'dlgProjectProperties
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(443, 388)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgProjectProperties"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgProjectProperties"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents fileListBox As Microsoft.VisualBasic.Compatibility.VB6.FileListBox
    Friend WithEvents txtBrief As System.Windows.Forms.TextBox
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnPath As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider

End Class
