<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgImport
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
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblParam = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtParam = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.lblVisibility = New System.Windows.Forms.Label
        Me.cmbVisibility = New System.Windows.Forms.ComboBox
        Me.chkInterface = New System.Windows.Forms.CheckBox
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.tblDeclaration = New System.Windows.Forms.TableLayoutPanel
        Me.lsbReferences = New System.Windows.Forms.ListBox
        Me.mnuEditReference = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewReference = New System.Windows.Forms.ToolStripMenuItem
        Me.EditReference = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPaste = New System.Windows.Forms.ToolStripMenuItem
        Me.DuplicateReference = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuRefDependencies = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.AddReferences = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReplace = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMerge = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuConfirm = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.RemoveRedundant = New System.Windows.Forms.ToolStripMenuItem
        Me.DeleteReference = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveAll = New System.Windows.Forms.ToolStripMenuItem
        Me.txtInterface = New System.Windows.Forms.TextBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.tblDeclaration.SuspendLayout()
        Me.mnuEditReference.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(220, 284)
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
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.39018!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.60982!))
        Me.TableLayoutPanel3.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.lblParam, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.txtName, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.txtParam, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel4, 0, 2)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 3
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(363, 84)
        Me.TableLayoutPanel3.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 27)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Name:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblParam
        '
        Me.lblParam.AutoSize = True
        Me.lblParam.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblParam.Location = New System.Drawing.Point(3, 27)
        Me.lblParam.Name = "lblParam"
        Me.lblParam.Size = New System.Drawing.Size(93, 27)
        Me.lblParam.TabIndex = 1
        Me.lblParam.Text = "FullPathName:"
        Me.lblParam.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtName
        '
        Me.txtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtName.Location = New System.Drawing.Point(102, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(258, 20)
        Me.txtName.TabIndex = 3
        '
        'txtParam
        '
        Me.txtParam.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtParam.Location = New System.Drawing.Point(102, 30)
        Me.txtParam.Name = "txtParam"
        Me.txtParam.Size = New System.Drawing.Size(258, 20)
        Me.txtParam.TabIndex = 4
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 3
        Me.TableLayoutPanel3.SetColumnSpan(Me.TableLayoutPanel4, 2)
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.38461!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.61538!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 181.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.lblVisibility, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.cmbVisibility, 2, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.chkInterface, 0, 0)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 57)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(357, 24)
        Me.TableLayoutPanel4.TabIndex = 5
        '
        'lblVisibility
        '
        Me.lblVisibility.AutoSize = True
        Me.lblVisibility.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblVisibility.Location = New System.Drawing.Point(118, 0)
        Me.lblVisibility.Name = "lblVisibility"
        Me.lblVisibility.Size = New System.Drawing.Size(54, 24)
        Me.lblVisibility.TabIndex = 2
        Me.lblVisibility.Text = "Visibility:"
        Me.lblVisibility.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbVisibility
        '
        Me.cmbVisibility.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbVisibility.FormattingEnabled = True
        Me.cmbVisibility.Location = New System.Drawing.Point(178, 3)
        Me.cmbVisibility.Name = "cmbVisibility"
        Me.cmbVisibility.Size = New System.Drawing.Size(176, 21)
        Me.cmbVisibility.TabIndex = 5
        '
        'chkInterface
        '
        Me.chkInterface.AutoSize = True
        Me.chkInterface.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkInterface.Location = New System.Drawing.Point(3, 3)
        Me.chkInterface.Name = "chkInterface"
        Me.chkInterface.Size = New System.Drawing.Size(109, 18)
        Me.chkInterface.TabIndex = 6
        Me.chkInterface.Text = "Interface declaration"
        Me.chkInterface.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel3, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.tblDeclaration, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 3
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.64605!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.35395!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(369, 316)
        Me.TableLayoutPanel2.TabIndex = 2
        '
        'tblDeclaration
        '
        Me.tblDeclaration.ColumnCount = 2
        Me.tblDeclaration.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.54651!))
        Me.tblDeclaration.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.45349!))
        Me.tblDeclaration.Controls.Add(Me.lsbReferences, 0, 0)
        Me.tblDeclaration.Controls.Add(Me.txtInterface, 1, 0)
        Me.tblDeclaration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblDeclaration.Location = New System.Drawing.Point(3, 93)
        Me.tblDeclaration.Name = "tblDeclaration"
        Me.tblDeclaration.RowCount = 1
        Me.tblDeclaration.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblDeclaration.Size = New System.Drawing.Size(363, 180)
        Me.tblDeclaration.TabIndex = 2
        '
        'lsbReferences
        '
        Me.lsbReferences.ContextMenuStrip = Me.mnuEditReference
        Me.lsbReferences.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbReferences.FormattingEnabled = True
        Me.lsbReferences.Location = New System.Drawing.Point(3, 3)
        Me.lsbReferences.Name = "lsbReferences"
        Me.lsbReferences.Size = New System.Drawing.Size(170, 173)
        Me.lsbReferences.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.lsbReferences, "Please click right to update list")
        '
        'mnuEditReference
        '
        Me.mnuEditReference.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewReference, Me.EditReference, Me.ToolStripSeparator3, Me.mnuCopy, Me.mnuPaste, Me.DuplicateReference, Me.ToolStripSeparator4, Me.mnuRefDependencies, Me.ToolStripSeparator1, Me.AddReferences, Me.ToolStripSeparator2, Me.RemoveRedundant, Me.DeleteReference, Me.RemoveAll})
        Me.mnuEditReference.Name = "ContextMenuStrip1"
        Me.mnuEditReference.Size = New System.Drawing.Size(189, 270)
        '
        'NewReference
        '
        Me.NewReference.Image = Global.ClassXmlProject.My.Resources.Resources.Address_Book
        Me.NewReference.Name = "NewReference"
        Me.NewReference.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.NewReference.Size = New System.Drawing.Size(188, 22)
        Me.NewReference.Text = "New"
        '
        'EditReference
        '
        Me.EditReference.Image = Global.ClassXmlProject.My.Resources.Resources.Rename___Edit
        Me.EditReference.Name = "EditReference"
        Me.EditReference.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.EditReference.Size = New System.Drawing.Size(188, 22)
        Me.EditReference.Text = "Edit..."
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(185, 6)
        '
        'mnuCopy
        '
        Me.mnuCopy.Image = Global.ClassXmlProject.My.Resources.Resources.Copy
        Me.mnuCopy.Name = "mnuCopy"
        Me.mnuCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuCopy.Size = New System.Drawing.Size(188, 22)
        Me.mnuCopy.Text = "Copy"
        '
        'mnuPaste
        '
        Me.mnuPaste.Image = Global.ClassXmlProject.My.Resources.Resources.Paste
        Me.mnuPaste.Name = "mnuPaste"
        Me.mnuPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuPaste.Size = New System.Drawing.Size(188, 22)
        Me.mnuPaste.Text = "Paste"
        '
        'DuplicateReference
        '
        Me.DuplicateReference.Name = "DuplicateReference"
        Me.DuplicateReference.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.DuplicateReference.Size = New System.Drawing.Size(188, 22)
        Me.DuplicateReference.Text = "Duplicate"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(185, 6)
        '
        'mnuRefDependencies
        '
        Me.mnuRefDependencies.Image = Global.ClassXmlProject.My.Resources.Resources.Search
        Me.mnuRefDependencies.Name = "mnuRefDependencies"
        Me.mnuRefDependencies.Size = New System.Drawing.Size(188, 22)
        Me.mnuRefDependencies.Text = "Search dependencies..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(185, 6)
        '
        'AddReferences
        '
        Me.AddReferences.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReplace, Me.mnuMerge, Me.mnuConfirm})
        Me.AddReferences.Name = "AddReferences"
        Me.AddReferences.Size = New System.Drawing.Size(188, 22)
        Me.AddReferences.Text = "Import references"
        '
        'mnuReplace
        '
        Me.mnuReplace.Name = "mnuReplace"
        Me.mnuReplace.Size = New System.Drawing.Size(124, 22)
        Me.mnuReplace.Tag = "0"
        Me.mnuReplace.Text = "Replace..."
        '
        'mnuMerge
        '
        Me.mnuMerge.Name = "mnuMerge"
        Me.mnuMerge.Size = New System.Drawing.Size(124, 22)
        Me.mnuMerge.Tag = "1"
        Me.mnuMerge.Text = "Merge..."
        '
        'mnuConfirm
        '
        Me.mnuConfirm.Name = "mnuConfirm"
        Me.mnuConfirm.Size = New System.Drawing.Size(124, 22)
        Me.mnuConfirm.Tag = "3"
        Me.mnuConfirm.Text = "Confirm..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(185, 6)
        '
        'RemoveRedundant
        '
        Me.RemoveRedundant.Image = Global.ClassXmlProject.My.Resources.Resources._Stop
        Me.RemoveRedundant.Name = "RemoveRedundant"
        Me.RemoveRedundant.Size = New System.Drawing.Size(188, 22)
        Me.RemoveRedundant.Text = "Remove redundancy..."
        '
        'DeleteReference
        '
        Me.DeleteReference.Image = Global.ClassXmlProject.My.Resources.Resources.Stop_2
        Me.DeleteReference.Name = "DeleteReference"
        Me.DeleteReference.Size = New System.Drawing.Size(188, 22)
        Me.DeleteReference.Text = "Delete"
        '
        'RemoveAll
        '
        Me.RemoveAll.Image = Global.ClassXmlProject.My.Resources.Resources.Delete
        Me.RemoveAll.Name = "RemoveAll"
        Me.RemoveAll.Size = New System.Drawing.Size(188, 22)
        Me.RemoveAll.Text = "Remove all"
        '
        'txtInterface
        '
        Me.txtInterface.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtInterface.Location = New System.Drawing.Point(179, 3)
        Me.txtInterface.Multiline = True
        Me.txtInterface.Name = "txtInterface"
        Me.txtInterface.Size = New System.Drawing.Size(181, 174)
        Me.txtInterface.TabIndex = 2
        '
        'dlgImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(369, 316)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgImport"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgImport"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.tblDeclaration.ResumeLayout(False)
        Me.tblDeclaration.PerformLayout()
        Me.mnuEditReference.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblParam As System.Windows.Forms.Label
    Friend WithEvents lblVisibility As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtParam As System.Windows.Forms.TextBox
    Friend WithEvents cmbVisibility As System.Windows.Forms.ComboBox
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents chkInterface As System.Windows.Forms.CheckBox
    Friend WithEvents tblDeclaration As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lsbReferences As System.Windows.Forms.ListBox
    Friend WithEvents mnuEditReference As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewReference As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditReference As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteReference As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtInterface As System.Windows.Forms.TextBox
    Friend WithEvents RemoveRedundant As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AddReferences As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuRefDependencies As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuReplace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMerge As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuConfirm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DuplicateReference As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator

End Class
