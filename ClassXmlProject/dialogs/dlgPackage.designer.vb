<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgPackage
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
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
        Me.btnDelete = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.strpProgressBar = New System.Windows.Forms.ProgressBar
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtBrief = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtDetails = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.chkFolder = New System.Windows.Forms.CheckBox
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.btnFolder = New System.Windows.Forms.Button
        Me.txtFolder = New System.Windows.Forms.TextBox
        Me.mnuClass = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAdd = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddImport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddClass = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddPackage = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPaste = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDuplicate = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuDependencies = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuImportReferences = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExportReferences = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuExportNodes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuImportNodes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuUpdateNodes = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuRedundancies = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.gridClasses = New ClassXmlProject.XmlDataGridView
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.mnuClass.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridClasses, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.CausesValidation = False
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnDelete, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.strpProgressBar, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 479)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(742, 27)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnDelete.CausesValidation = False
        Me.btnDelete.Location = New System.Drawing.Point(3, 3)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(67, 21)
        Me.btnDelete.TabIndex = 3
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.CausesValidation = False
        Me.Cancel_Button.Location = New System.Drawing.Point(662, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 21)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OK_Button.Location = New System.Drawing.Point(575, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 21)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'strpProgressBar
        '
        Me.strpProgressBar.Location = New System.Drawing.Point(93, 3)
        Me.strpProgressBar.Name = "strpProgressBar"
        Me.strpProgressBar.Size = New System.Drawing.Size(121, 21)
        Me.strpProgressBar.TabIndex = 4
        Me.strpProgressBar.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label1.Location = New System.Drawing.Point(49, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 30)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Name:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtName
        '
        Me.txtName.Dock = System.Windows.Forms.DockStyle.Left
        Me.txtName.Location = New System.Drawing.Point(93, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(492, 20)
        Me.txtName.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label2.Location = New System.Drawing.Point(10, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 30)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Brief comment:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtBrief
        '
        Me.txtBrief.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBrief.Location = New System.Drawing.Point(93, 33)
        Me.txtBrief.Name = "txtBrief"
        Me.txtBrief.Size = New System.Drawing.Size(646, 20)
        Me.txtBrief.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label3.Location = New System.Drawing.Point(28, 90)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 104)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Comments:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtDetails
        '
        Me.txtDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDetails.Location = New System.Drawing.Point(93, 93)
        Me.txtDetails.Multiline = True
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDetails.Size = New System.Drawing.Size(646, 98)
        Me.txtDetails.TabIndex = 6
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.CausesValidation = False
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.txtDetails, 1, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.Label3, 0, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.txtBrief, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.txtName, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.chkFolder, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel1, 1, 2)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 4
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(742, 194)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'chkFolder
        '
        Me.chkFolder.AutoSize = True
        Me.chkFolder.Dock = System.Windows.Forms.DockStyle.Right
        Me.chkFolder.Location = New System.Drawing.Point(32, 63)
        Me.chkFolder.Name = "chkFolder"
        Me.chkFolder.Size = New System.Drawing.Size(55, 24)
        Me.chkFolder.TabIndex = 7
        Me.chkFolder.Text = "Folder"
        Me.chkFolder.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.CausesValidation = False
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFolder)
        Me.FlowLayoutPanel1.Controls.Add(Me.txtFolder)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(93, 63)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(646, 24)
        Me.FlowLayoutPanel1.TabIndex = 8
        '
        'btnFolder
        '
        Me.btnFolder.CausesValidation = False
        Me.btnFolder.Location = New System.Drawing.Point(3, 3)
        Me.btnFolder.Name = "btnFolder"
        Me.btnFolder.Size = New System.Drawing.Size(56, 21)
        Me.btnFolder.TabIndex = 0
        Me.btnFolder.Text = "Path..."
        Me.btnFolder.UseVisualStyleBackColor = True
        '
        'txtFolder
        '
        Me.txtFolder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFolder.Location = New System.Drawing.Point(65, 3)
        Me.txtFolder.Name = "txtFolder"
        Me.txtFolder.Size = New System.Drawing.Size(427, 20)
        Me.txtFolder.TabIndex = 1
        '
        'mnuClass
        '
        Me.mnuClass.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdd, Me.mnuEdit, Me.ToolStripSeparator4, Me.mnuCopy, Me.mnuPaste, Me.mnuDuplicate, Me.mnuProperties, Me.ToolStripSeparator3, Me.mnuDependencies, Me.ToolStripSeparator5, Me.mnuImportReferences, Me.mnuExportReferences, Me.ToolStripSeparator2, Me.mnuExportNodes, Me.mnuImportNodes, Me.mnuUpdateNodes, Me.ToolStripSeparator1, Me.mnuRedundancies, Me.mnuDelete})
        Me.mnuClass.Name = "mnuClass"
        Me.mnuClass.Size = New System.Drawing.Size(246, 342)
        '
        'mnuAdd
        '
        Me.mnuAdd.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddImport, Me.mnuAddClass, Me.mnuAddPackage})
        Me.mnuAdd.Name = "mnuAdd"
        Me.mnuAdd.Size = New System.Drawing.Size(245, 22)
        Me.mnuAdd.Text = "Add"
        '
        'mnuAddImport
        '
        Me.mnuAddImport.Image = Global.ClassXmlProject.My.Resources.Resources.library
        Me.mnuAddImport.Name = "mnuAddImport"
        Me.mnuAddImport.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.mnuAddImport.Size = New System.Drawing.Size(148, 22)
        Me.mnuAddImport.Tag = "import"
        Me.mnuAddImport.Text = "Import"
        '
        'mnuAddClass
        '
        Me.mnuAddClass.Image = Global.ClassXmlProject.My.Resources.Resources.Address_Book
        Me.mnuAddClass.Name = "mnuAddClass"
        Me.mnuAddClass.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuAddClass.Size = New System.Drawing.Size(148, 22)
        Me.mnuAddClass.Tag = "class"
        Me.mnuAddClass.Text = "Class"
        '
        'mnuAddPackage
        '
        Me.mnuAddPackage.Image = Global.ClassXmlProject.My.Resources.Resources.my_Documents
        Me.mnuAddPackage.Name = "mnuAddPackage"
        Me.mnuAddPackage.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuAddPackage.Size = New System.Drawing.Size(148, 22)
        Me.mnuAddPackage.Tag = "package"
        Me.mnuAddPackage.Text = "Package"
        '
        'mnuEdit
        '
        Me.mnuEdit.Image = Global.ClassXmlProject.My.Resources.Resources.Rename___Edit
        Me.mnuEdit.Name = "mnuEdit"
        Me.mnuEdit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuEdit.Size = New System.Drawing.Size(245, 22)
        Me.mnuEdit.Text = "Edit..."
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(242, 6)
        '
        'mnuCopy
        '
        Me.mnuCopy.Image = Global.ClassXmlProject.My.Resources.Resources.Copy
        Me.mnuCopy.Name = "mnuCopy"
        Me.mnuCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuCopy.Size = New System.Drawing.Size(245, 22)
        Me.mnuCopy.Text = "Copy"
        '
        'mnuPaste
        '
        Me.mnuPaste.Image = Global.ClassXmlProject.My.Resources.Resources.Paste
        Me.mnuPaste.Name = "mnuPaste"
        Me.mnuPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuPaste.Size = New System.Drawing.Size(245, 22)
        Me.mnuPaste.Text = "Paste"
        '
        'mnuDuplicate
        '
        Me.mnuDuplicate.Name = "mnuDuplicate"
        Me.mnuDuplicate.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.mnuDuplicate.Size = New System.Drawing.Size(245, 22)
        Me.mnuDuplicate.Text = "Duplicate"
        '
        'mnuProperties
        '
        Me.mnuProperties.Name = "mnuProperties"
        Me.mnuProperties.Size = New System.Drawing.Size(245, 22)
        Me.mnuProperties.Text = "Properties..."
        Me.mnuProperties.Visible = False
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(242, 6)
        '
        'mnuDependencies
        '
        Me.mnuDependencies.Image = Global.ClassXmlProject.My.Resources.Resources.Search
        Me.mnuDependencies.Name = "mnuDependencies"
        Me.mnuDependencies.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.mnuDependencies.Size = New System.Drawing.Size(245, 22)
        Me.mnuDependencies.Text = "Search dependencies..."
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(242, 6)
        '
        'mnuImportReferences
        '
        Me.mnuImportReferences.Image = Global.ClassXmlProject.My.Resources.Resources.Back_2
        Me.mnuImportReferences.Name = "mnuImportReferences"
        Me.mnuImportReferences.Size = New System.Drawing.Size(245, 22)
        Me.mnuImportReferences.Text = "Import references..."
        '
        'mnuExportReferences
        '
        Me.mnuExportReferences.Image = Global.ClassXmlProject.My.Resources.Resources.Forward_2
        Me.mnuExportReferences.Name = "mnuExportReferences"
        Me.mnuExportReferences.Size = New System.Drawing.Size(245, 22)
        Me.mnuExportReferences.Text = "Export references..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(242, 6)
        '
        'mnuExportNodes
        '
        Me.mnuExportNodes.Image = Global.ClassXmlProject.My.Resources.Resources.Forward_2
        Me.mnuExportNodes.Name = "mnuExportNodes"
        Me.mnuExportNodes.Size = New System.Drawing.Size(245, 22)
        Me.mnuExportNodes.Text = "Export nodes..."
        '
        'mnuImportNodes
        '
        Me.mnuImportNodes.Image = Global.ClassXmlProject.My.Resources.Resources.Back_2
        Me.mnuImportNodes.Name = "mnuImportNodes"
        Me.mnuImportNodes.Size = New System.Drawing.Size(245, 22)
        Me.mnuImportNodes.Tag = "false"
        Me.mnuImportNodes.Text = "Import nodes..."
        '
        'mnuUpdateNodes
        '
        Me.mnuUpdateNodes.Image = Global.ClassXmlProject.My.Resources.Resources.Back_2
        Me.mnuUpdateNodes.Name = "mnuUpdateNodes"
        Me.mnuUpdateNodes.Size = New System.Drawing.Size(245, 22)
        Me.mnuUpdateNodes.Tag = "true"
        Me.mnuUpdateNodes.Text = "Merge nodes..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(242, 6)
        '
        'mnuRedundancies
        '
        Me.mnuRedundancies.Image = Global.ClassXmlProject.My.Resources.Resources._Stop
        Me.mnuRedundancies.Name = "mnuRedundancies"
        Me.mnuRedundancies.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Delete), System.Windows.Forms.Keys)
        Me.mnuRedundancies.Size = New System.Drawing.Size(245, 22)
        Me.mnuRedundancies.Text = "Remove redundancy..."
        '
        'mnuDelete
        '
        Me.mnuDelete.Image = Global.ClassXmlProject.My.Resources.Resources.Stop_2
        Me.mnuDelete.Name = "mnuDelete"
        Me.mnuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.mnuDelete.Size = New System.Drawing.Size(245, 22)
        Me.mnuDelete.Text = "Delete"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.SplitContainer1, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(748, 509)
        Me.TableLayoutPanel2.TabIndex = 2
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TableLayoutPanel3)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.gridClasses)
        Me.SplitContainer1.Size = New System.Drawing.Size(742, 470)
        Me.SplitContainer1.SplitterDistance = 194
        Me.SplitContainer1.TabIndex = 1
        '
        'errorProvider
        '
        Me.errorProvider.ContainerControl = Me
        '
        'gridClasses
        '
        Me.gridClasses.ColumnDragStart = 0
        Me.gridClasses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridClasses.ContextMenuStrip = Me.mnuClass
        Me.gridClasses.Dock = System.Windows.Forms.DockStyle.Right
        Me.gridClasses.Location = New System.Drawing.Point(22, 0)
        Me.gridClasses.Name = "gridClasses"
        Me.gridClasses.Size = New System.Drawing.Size(720, 272)
        Me.gridClasses.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.gridClasses, "Click right to update grid")
        '
        'dlgPackage
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(748, 509)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.MinimizeBox = False
        Me.Name = "dlgPackage"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgPackage"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.mnuClass.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridClasses, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtBrief As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents mnuClass As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAdd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridClasses As ClassXmlProject.XmlDataGridView
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents chkFolder As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents mnuAddClass As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddPackage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuImportNodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuUpdateNodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnFolder As System.Windows.Forms.Button
    Friend WithEvents txtFolder As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDuplicate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddImport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDependencies As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuRedundancies As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents mnuImportReferences As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents strpProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents mnuExportReferences As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExportNodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog

End Class
