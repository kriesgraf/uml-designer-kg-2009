Imports ClassXmlProject.MenuItemCommand

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgExternalTools
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
        Me.btnApply = New System.Windows.Forms.Button
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.txtDiffToolArguments = New System.Windows.Forms.TextBox
        Me.txtDiffTool = New System.Windows.Forms.TextBox
        Me.txtArguments = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtCommand = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtStylesheet = New System.Windows.Forms.TextBox
        Me.btnStylesheetPath = New System.Windows.Forms.Button
        Me.btnCommandPath = New System.Windows.Forms.Button
        Me.btnDiffPath = New System.Windows.Forms.Button
        Me.btnArguments = New System.Windows.Forms.Button
        Me.btnDiffArgs = New System.Windows.Forms.Button
        Me.chkCommand = New System.Windows.Forms.CheckBox
        Me.chkDiffTool = New System.Windows.Forms.CheckBox
        Me.txtXslParams = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnXslParams = New System.Windows.Forms.Button
        Me.btnXslStylesheet = New System.Windows.Forms.Button
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.lsbExternalTools = New System.Windows.Forms.ListBox
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.DiffArgsMnuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewElementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CurrentElementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ProjectFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ToolArgsMnuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.FirstElementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SecondElementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NinthElementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ProjectFolderToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.DiffArgsMnuStrip.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolArgsMnuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.61905!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.38095!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnApply, 2, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(184, 378)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(237, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Enabled = False
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(59, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Tag = "false"
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(68, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(65, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'btnApply
        '
        Me.btnApply.Enabled = False
        Me.btnApply.Location = New System.Drawing.Point(139, 3)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(72, 23)
        Me.btnApply.TabIndex = 2
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel3, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel4, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 3
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 215.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(424, 410)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 3
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.txtDiffToolArguments, 1, 6)
        Me.TableLayoutPanel3.Controls.Add(Me.txtDiffTool, 1, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.txtArguments, 1, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.Label3, 0, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.Label5, 0, 6)
        Me.TableLayoutPanel3.Controls.Add(Me.txtCommand, 1, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.txtName, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.Label6, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.txtStylesheet, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.btnStylesheetPath, 2, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.btnCommandPath, 2, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.btnDiffPath, 2, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.btnArguments, 2, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.btnDiffArgs, 2, 6)
        Me.TableLayoutPanel3.Controls.Add(Me.chkCommand, 0, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.chkDiffTool, 0, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.txtXslParams, 1, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.Label2, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.btnXslParams, 2, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.btnXslStylesheet, 0, 1)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 157)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 7
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(418, 209)
        Me.TableLayoutPanel3.TabIndex = 1
        '
        'txtDiffToolArguments
        '
        Me.txtDiffToolArguments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDiffToolArguments.Location = New System.Drawing.Point(108, 183)
        Me.txtDiffToolArguments.Name = "txtDiffToolArguments"
        Me.txtDiffToolArguments.Size = New System.Drawing.Size(277, 20)
        Me.txtDiffToolArguments.TabIndex = 7
        '
        'txtDiffTool
        '
        Me.txtDiffTool.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDiffTool.Location = New System.Drawing.Point(108, 153)
        Me.txtDiffTool.Name = "txtDiffTool"
        Me.txtDiffTool.Size = New System.Drawing.Size(277, 20)
        Me.txtDiffTool.TabIndex = 6
        '
        'txtArguments
        '
        Me.txtArguments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtArguments.Location = New System.Drawing.Point(108, 123)
        Me.txtArguments.Name = "txtArguments"
        Me.txtArguments.Size = New System.Drawing.Size(277, 20)
        Me.txtArguments.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label3.Location = New System.Drawing.Point(42, 120)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 30)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Arguments:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label5.Location = New System.Drawing.Point(4, 180)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(98, 31)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Diff tool arguments:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCommand
        '
        Me.txtCommand.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtCommand.Location = New System.Drawing.Point(108, 93)
        Me.txtCommand.Name = "txtCommand"
        Me.txtCommand.Size = New System.Drawing.Size(277, 20)
        Me.txtCommand.TabIndex = 4
        '
        'txtName
        '
        Me.txtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtName.Location = New System.Drawing.Point(108, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(277, 20)
        Me.txtName.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label6.Location = New System.Drawing.Point(64, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(38, 30)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Name:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtStylesheet
        '
        Me.txtStylesheet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtStylesheet.Location = New System.Drawing.Point(108, 33)
        Me.txtStylesheet.Name = "txtStylesheet"
        Me.txtStylesheet.Size = New System.Drawing.Size(277, 20)
        Me.txtStylesheet.TabIndex = 10
        '
        'btnStylesheetPath
        '
        Me.btnStylesheetPath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnStylesheetPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStylesheetPath.Location = New System.Drawing.Point(391, 33)
        Me.btnStylesheetPath.Name = "btnStylesheetPath"
        Me.btnStylesheetPath.Size = New System.Drawing.Size(24, 24)
        Me.btnStylesheetPath.TabIndex = 12
        Me.btnStylesheetPath.Tag = "1"
        Me.btnStylesheetPath.Text = "..."
        Me.btnStylesheetPath.UseVisualStyleBackColor = True
        '
        'btnCommandPath
        '
        Me.btnCommandPath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnCommandPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCommandPath.Location = New System.Drawing.Point(391, 93)
        Me.btnCommandPath.Name = "btnCommandPath"
        Me.btnCommandPath.Size = New System.Drawing.Size(24, 24)
        Me.btnCommandPath.TabIndex = 13
        Me.btnCommandPath.Tag = "0"
        Me.btnCommandPath.Text = "..."
        Me.btnCommandPath.UseVisualStyleBackColor = True
        '
        'btnDiffPath
        '
        Me.btnDiffPath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDiffPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDiffPath.Location = New System.Drawing.Point(391, 153)
        Me.btnDiffPath.Name = "btnDiffPath"
        Me.btnDiffPath.Size = New System.Drawing.Size(24, 24)
        Me.btnDiffPath.TabIndex = 14
        Me.btnDiffPath.Tag = "0"
        Me.btnDiffPath.Text = "..."
        Me.btnDiffPath.UseVisualStyleBackColor = True
        '
        'btnArguments
        '
        Me.btnArguments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnArguments.Location = New System.Drawing.Point(391, 123)
        Me.btnArguments.Name = "btnArguments"
        Me.btnArguments.Size = New System.Drawing.Size(24, 24)
        Me.btnArguments.TabIndex = 15
        Me.btnArguments.Text = "Parameters"
        Me.btnArguments.UseVisualStyleBackColor = True
        '
        'btnDiffArgs
        '
        Me.btnDiffArgs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDiffArgs.Location = New System.Drawing.Point(391, 183)
        Me.btnDiffArgs.Name = "btnDiffArgs"
        Me.btnDiffArgs.Size = New System.Drawing.Size(24, 25)
        Me.btnDiffArgs.TabIndex = 16
        Me.btnDiffArgs.Text = "Parameters"
        Me.btnDiffArgs.UseVisualStyleBackColor = True
        '
        'chkCommand
        '
        Me.chkCommand.AutoSize = True
        Me.chkCommand.Dock = System.Windows.Forms.DockStyle.Right
        Me.chkCommand.Location = New System.Drawing.Point(26, 93)
        Me.chkCommand.Name = "chkCommand"
        Me.chkCommand.Size = New System.Drawing.Size(76, 24)
        Me.chkCommand.TabIndex = 17
        Me.chkCommand.Text = "Command:"
        Me.chkCommand.UseVisualStyleBackColor = True
        '
        'chkDiffTool
        '
        Me.chkDiffTool.AutoSize = True
        Me.chkDiffTool.Dock = System.Windows.Forms.DockStyle.Right
        Me.chkDiffTool.Location = New System.Drawing.Point(37, 153)
        Me.chkDiffTool.Name = "chkDiffTool"
        Me.chkDiffTool.Size = New System.Drawing.Size(65, 24)
        Me.chkDiffTool.TabIndex = 18
        Me.chkDiffTool.Text = "Diff tool:"
        Me.chkDiffTool.UseVisualStyleBackColor = True
        '
        'txtXslParams
        '
        Me.txtXslParams.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtXslParams.Location = New System.Drawing.Point(108, 63)
        Me.txtXslParams.Name = "txtXslParams"
        Me.txtXslParams.Size = New System.Drawing.Size(277, 20)
        Me.txtXslParams.TabIndex = 19
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label2.Location = New System.Drawing.Point(17, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 30)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "XSL parameters:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnXslParams
        '
        Me.btnXslParams.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnXslParams.Location = New System.Drawing.Point(391, 63)
        Me.btnXslParams.Name = "btnXslParams"
        Me.btnXslParams.Size = New System.Drawing.Size(24, 24)
        Me.btnXslParams.TabIndex = 21
        Me.btnXslParams.Text = "Parameters"
        Me.btnXslParams.UseVisualStyleBackColor = True
        '
        'btnXslStylesheet
        '
        Me.btnXslStylesheet.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnXslStylesheet.Location = New System.Drawing.Point(9, 33)
        Me.btnXslStylesheet.Name = "btnXslStylesheet"
        Me.btnXslStylesheet.Size = New System.Drawing.Size(93, 24)
        Me.btnXslStylesheet.TabIndex = 22
        Me.btnXslStylesheet.Text = "XSL stylesheet"
        Me.btnXslStylesheet.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.lsbExternalTools, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.FlowLayoutPanel1, 1, 1)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 2
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(418, 148)
        Me.TableLayoutPanel4.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Menu content:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lsbExternalTools
        '
        Me.lsbExternalTools.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbExternalTools.FormattingEnabled = True
        Me.lsbExternalTools.Location = New System.Drawing.Point(3, 24)
        Me.lsbExternalTools.Name = "lsbExternalTools"
        Me.lsbExternalTools.Size = New System.Drawing.Size(302, 121)
        Me.lsbExternalTools.TabIndex = 1
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAdd)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnDelete)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(311, 24)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(104, 121)
        Me.FlowLayoutPanel1.TabIndex = 2
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(3, 3)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(98, 24)
        Me.btnAdd.TabIndex = 0
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(3, 33)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(98, 24)
        Me.btnDelete.TabIndex = 1
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'DiffArgsMnuStrip
        '
        Me.DiffArgsMnuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewElementToolStripMenuItem, Me.CurrentElementToolStripMenuItem, Me.ProjectFolderToolStripMenuItem})
        Me.DiffArgsMnuStrip.Name = "ContextMenuStrip1"
        Me.DiffArgsMnuStrip.Size = New System.Drawing.Size(186, 70)
        '
        'NewElementToolStripMenuItem
        '
        Me.NewElementToolStripMenuItem.Name = "NewElementToolStripMenuItem"
        Me.NewElementToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.NewElementToolStripMenuItem.Tag = "{0}"
        Me.NewElementToolStripMenuItem.Text = "{0} Generated element"
        '
        'CurrentElementToolStripMenuItem
        '
        Me.CurrentElementToolStripMenuItem.Name = "CurrentElementToolStripMenuItem"
        Me.CurrentElementToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.CurrentElementToolStripMenuItem.Tag = "{1}"
        Me.CurrentElementToolStripMenuItem.Text = "{1} Current element"
        '
        'ProjectFolderToolStripMenuItem
        '
        Me.ProjectFolderToolStripMenuItem.Name = "ProjectFolderToolStripMenuItem"
        Me.ProjectFolderToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.ProjectFolderToolStripMenuItem.Tag = "{$ProjectFolder}"
        Me.ProjectFolderToolStripMenuItem.Text = "{$ProjectFolder}"
        '
        'errorProvider
        '
        Me.errorProvider.ContainerControl = Me
        '
        'ToolArgsMnuStrip
        '
        Me.ToolArgsMnuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FirstElementToolStripMenuItem, Me.SecondElementToolStripMenuItem, Me.NinthElementToolStripMenuItem, Me.ProjectFolderToolStripMenuItem1})
        Me.ToolArgsMnuStrip.Name = "ToolArgsMnuStrip"
        Me.ToolArgsMnuStrip.Size = New System.Drawing.Size(170, 92)
        '
        'FirstElementToolStripMenuItem
        '
        Me.FirstElementToolStripMenuItem.Name = "FirstElementToolStripMenuItem"
        Me.FirstElementToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.FirstElementToolStripMenuItem.Tag = "{0}"
        Me.FirstElementToolStripMenuItem.Text = "{0} First element"
        '
        'SecondElementToolStripMenuItem
        '
        Me.SecondElementToolStripMenuItem.Name = "SecondElementToolStripMenuItem"
        Me.SecondElementToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.SecondElementToolStripMenuItem.Tag = "{1}"
        Me.SecondElementToolStripMenuItem.Text = "{1} Second element"
        '
        'NinthElementToolStripMenuItem
        '
        Me.NinthElementToolStripMenuItem.Name = "NinthElementToolStripMenuItem"
        Me.NinthElementToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.NinthElementToolStripMenuItem.Tag = "{9}"
        Me.NinthElementToolStripMenuItem.Text = "{9} Ninth element"
        '
        'ProjectFolderToolStripMenuItem1
        '
        Me.ProjectFolderToolStripMenuItem1.Name = "ProjectFolderToolStripMenuItem1"
        Me.ProjectFolderToolStripMenuItem1.Size = New System.Drawing.Size(169, 22)
        Me.ProjectFolderToolStripMenuItem1.Tag = "{$ProjectFolder}"
        Me.ProjectFolderToolStripMenuItem1.Text = "{$ProjectFolder}"
        '
        'dlgExternalTools
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(424, 410)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgExternalTools"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "External tools"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.DiffArgsMnuStrip.ResumeLayout(False)
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolArgsMnuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lsbExternalTools As System.Windows.Forms.ListBox
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtDiffToolArguments As System.Windows.Forms.TextBox
    Friend WithEvents txtDiffTool As System.Windows.Forms.TextBox
    Friend WithEvents txtArguments As System.Windows.Forms.TextBox
    Friend WithEvents txtCommand As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents txtStylesheet As System.Windows.Forms.TextBox
    Friend WithEvents btnStylesheetPath As System.Windows.Forms.Button
    Friend WithEvents btnCommandPath As System.Windows.Forms.Button
    Friend WithEvents btnDiffPath As System.Windows.Forms.Button
    Friend WithEvents DiffArgsMnuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewElementToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnArguments As System.Windows.Forms.Button
    Friend WithEvents btnDiffArgs As System.Windows.Forms.Button
    Friend WithEvents ProjectFolderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CurrentElementToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolArgsMnuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents FirstElementToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SecondElementToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectFolderToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCommand As System.Windows.Forms.CheckBox
    Friend WithEvents chkDiffTool As System.Windows.Forms.CheckBox
    Friend WithEvents txtXslParams As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnXslParams As System.Windows.Forms.Button
    Friend WithEvents NinthElementToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnXslStylesheet As System.Windows.Forms.Button

End Class
