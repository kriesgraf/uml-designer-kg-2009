<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgConstructor
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
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtBrief = New System.Windows.Forms.TextBox
        Me.txtComment = New System.Windows.Forms.TextBox
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.chkCopy = New System.Windows.Forms.CheckBox
        Me.chkInline = New System.Windows.Forms.CheckBox
        Me.cmbRange = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.mnuParam = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddParam = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditParam = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPaste = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDuplicate = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.grdParams = New ClassXmlProject.XmlDataGridView
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.mnuParam.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdParams, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.CausesValidation = False
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel2.SetColumnSpan(Me.TableLayoutPanel1, 2)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 91.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDelete, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 331)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(641, 35)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.CausesValidation = False
        Me.Cancel_Button.Location = New System.Drawing.Point(562, 6)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(461, 6)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnDelete.CausesValidation = False
        Me.btnDelete.Location = New System.Drawing.Point(3, 6)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(67, 23)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.86339!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.13661!))
        Me.TableLayoutPanel2.Controls.Add(Me.Label1, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label2, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.txtBrief, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.txtComment, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel1, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.cmbRange, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label3, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.grdParams, 0, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 0, 5)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 6
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.78008!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.21992!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(647, 369)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label1.Location = New System.Drawing.Point(80, 58)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Brief comment:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label2.Location = New System.Drawing.Point(103, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 80)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Comment:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtBrief
        '
        Me.txtBrief.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBrief.Location = New System.Drawing.Point(163, 61)
        Me.txtBrief.Name = "txtBrief"
        Me.txtBrief.Size = New System.Drawing.Size(481, 20)
        Me.txtBrief.TabIndex = 4
        '
        'txtComment
        '
        Me.txtComment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtComment.Location = New System.Drawing.Point(163, 86)
        Me.txtComment.Multiline = True
        Me.txtComment.Name = "txtComment"
        Me.txtComment.Size = New System.Drawing.Size(481, 74)
        Me.txtComment.TabIndex = 5
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.chkCopy)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkInline)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(163, 3)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(481, 27)
        Me.FlowLayoutPanel1.TabIndex = 3
        '
        'chkCopy
        '
        Me.chkCopy.AutoSize = True
        Me.chkCopy.CheckAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkCopy.Location = New System.Drawing.Point(3, 3)
        Me.chkCopy.Name = "chkCopy"
        Me.chkCopy.Size = New System.Drawing.Size(106, 17)
        Me.chkCopy.TabIndex = 0
        Me.chkCopy.Text = "Copy constructor"
        Me.chkCopy.UseVisualStyleBackColor = True
        '
        'chkInline
        '
        Me.chkInline.AutoSize = True
        Me.chkInline.CheckAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkInline.Location = New System.Drawing.Point(115, 3)
        Me.chkInline.Name = "chkInline"
        Me.chkInline.Size = New System.Drawing.Size(115, 17)
        Me.chkInline.TabIndex = 1
        Me.chkInline.Text = "Custom inline code"
        Me.chkInline.UseVisualStyleBackColor = True
        '
        'cmbRange
        '
        Me.cmbRange.Dock = System.Windows.Forms.DockStyle.Left
        Me.cmbRange.FormattingEnabled = True
        Me.cmbRange.Location = New System.Drawing.Point(163, 36)
        Me.cmbRange.Name = "cmbRange"
        Me.cmbRange.Size = New System.Drawing.Size(109, 21)
        Me.cmbRange.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label3.Location = New System.Drawing.Point(115, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 25)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Range:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'mnuParam
        '
        Me.mnuParam.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddParam, Me.mnuEditParam, Me.ToolStripSeparator2, Me.mnuCopy, Me.mnuPaste, Me.mnuDuplicate, Me.mnuProperties, Me.ToolStripSeparator1, Me.mnuDelete})
        Me.mnuParam.Name = "mnuParam"
        Me.mnuParam.Size = New System.Drawing.Size(161, 170)
        '
        'mnuAddParam
        '
        Me.mnuAddParam.Name = "mnuAddParam"
        Me.mnuAddParam.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuAddParam.Size = New System.Drawing.Size(160, 22)
        Me.mnuAddParam.Text = "Add param"
        '
        'mnuEditParam
        '
        Me.mnuEditParam.Image = Global.ClassXmlProject.My.Resources.Resources.Rename___Edit
        Me.mnuEditParam.Name = "mnuEditParam"
        Me.mnuEditParam.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuEditParam.Size = New System.Drawing.Size(160, 22)
        Me.mnuEditParam.Text = "Edit..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(157, 6)
        '
        'mnuCopy
        '
        Me.mnuCopy.Image = Global.ClassXmlProject.My.Resources.Resources.Copy
        Me.mnuCopy.Name = "mnuCopy"
        Me.mnuCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuCopy.Size = New System.Drawing.Size(160, 22)
        Me.mnuCopy.Text = "Copy"
        '
        'mnuPaste
        '
        Me.mnuPaste.Image = Global.ClassXmlProject.My.Resources.Resources.Paste
        Me.mnuPaste.Name = "mnuPaste"
        Me.mnuPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuPaste.Size = New System.Drawing.Size(160, 22)
        Me.mnuPaste.Text = "Paste"
        '
        'mnuDuplicate
        '
        Me.mnuDuplicate.Name = "mnuDuplicate"
        Me.mnuDuplicate.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.mnuDuplicate.Size = New System.Drawing.Size(160, 22)
        Me.mnuDuplicate.Text = "Duplicate"
        '
        'mnuProperties
        '
        Me.mnuProperties.Name = "mnuProperties"
        Me.mnuProperties.Size = New System.Drawing.Size(160, 22)
        Me.mnuProperties.Text = "Parameters..."
        Me.mnuProperties.Visible = False
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(157, 6)
        '
        'mnuDelete
        '
        Me.mnuDelete.Image = Global.ClassXmlProject.My.Resources.Resources.Stop_2
        Me.mnuDelete.Name = "mnuDelete"
        Me.mnuDelete.Size = New System.Drawing.Size(160, 22)
        Me.mnuDelete.Text = "Delete"
        '
        'errorProvider
        '
        Me.errorProvider.ContainerControl = Me
        '
        'grdParams
        '
        Me.grdParams.AllowDrop = True
        Me.grdParams.ColumnDragStart = 0
        Me.grdParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel2.SetColumnSpan(Me.grdParams, 2)
        Me.grdParams.ContextMenuStrip = Me.mnuParam
        Me.grdParams.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdParams.Location = New System.Drawing.Point(20, 166)
        Me.grdParams.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.grdParams.Name = "grdParams"
        Me.grdParams.Size = New System.Drawing.Size(624, 159)
        Me.grdParams.TabIndex = 7
        Me.grdParams.Tag = "param"
        Me.ToolTip1.SetToolTip(Me.grdParams, "Click right to update grid")
        '
        'dlgConstructor
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(647, 369)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgConstructor"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgConstructor"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.mnuParam.ResumeLayout(False)
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdParams, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents txtBrief As System.Windows.Forms.TextBox
    Friend WithEvents txtComment As System.Windows.Forms.TextBox
    Friend WithEvents cmbRange As System.Windows.Forms.ComboBox
    Friend WithEvents chkCopy As System.Windows.Forms.CheckBox
    Friend WithEvents chkInline As System.Windows.Forms.CheckBox
    Friend WithEvents grdParams As ClassXmlProject.XmlDataGridView
    Friend WithEvents mnuParam As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAddParam As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditParam As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDuplicate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider

End Class
