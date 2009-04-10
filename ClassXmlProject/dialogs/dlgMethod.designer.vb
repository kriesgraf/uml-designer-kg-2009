<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgMethod
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.txtBrief = New System.Windows.Forms.TextBox
        Me.lblRange = New System.Windows.Forms.Label
        Me.btnType = New System.Windows.Forms.Button
        Me.lblMember = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblReturnComment = New System.Windows.Forms.Label
        Me.txtReturnComments = New System.Windows.Forms.TextBox
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbRange = New System.Windows.Forms.ComboBox
        Me.lblImplementation = New System.Windows.Forms.Label
        Me.cmbImplementation = New System.Windows.Forms.ComboBox
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbMember = New System.Windows.Forms.ComboBox
        Me.chkConst = New System.Windows.Forms.CheckBox
        Me.chkInline = New System.Windows.Forms.CheckBox
        Me.lblBehaviour = New System.Windows.Forms.Label
        Me.cmbBehaviour = New System.Windows.Forms.ComboBox
        Me.grdParams = New ClassXmlProject.XmlDataGridView
        Me.mnuMembers = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddParam = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddException = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditParam = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPaste = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDuplicate = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.Label7 = New System.Windows.Forms.Label
        Me.FlowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel
        Me.txtName = New System.Windows.Forms.TextBox
        Me.chkOperator = New System.Windows.Forms.CheckBox
        Me.txtOperator = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.txtComment = New System.Windows.Forms.TextBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel3.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        CType(Me.grdParams, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuMembers.SuspendLayout()
        Me.FlowLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label2.Location = New System.Drawing.Point(19, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 25)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Brief comment:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblName.Location = New System.Drawing.Point(58, 33)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 33)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.Label2, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.lblName, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.txtBrief, 1, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.lblRange, 0, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.btnType, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.lblMember, 0, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.Label6, 0, 6)
        Me.TableLayoutPanel3.Controls.Add(Me.lblReturnComment, 0, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.txtReturnComments, 1, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel1, 1, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel2, 1, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.grdParams, 0, 7)
        Me.TableLayoutPanel3.Controls.Add(Me.Label7, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel3, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel1, 0, 8)
        Me.TableLayoutPanel3.Controls.Add(Me.txtComment, 1, 6)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 9
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.59609!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68.40391!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(606, 529)
        Me.TableLayoutPanel3.TabIndex = 2
        '
        'txtBrief
        '
        Me.txtBrief.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBrief.Location = New System.Drawing.Point(102, 69)
        Me.txtBrief.Name = "txtBrief"
        Me.txtBrief.Size = New System.Drawing.Size(501, 20)
        Me.txtBrief.TabIndex = 4
        '
        'lblRange
        '
        Me.lblRange.AutoSize = True
        Me.lblRange.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblRange.Location = New System.Drawing.Point(54, 116)
        Me.lblRange.Name = "lblRange"
        Me.lblRange.Size = New System.Drawing.Size(42, 33)
        Me.lblRange.TabIndex = 4
        Me.lblRange.Text = "Range:"
        Me.lblRange.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnType
        '
        Me.btnType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnType.Location = New System.Drawing.Point(102, 3)
        Me.btnType.Name = "btnType"
        Me.btnType.Size = New System.Drawing.Size(501, 27)
        Me.btnType.TabIndex = 5
        Me.btnType.Text = "<Type>"
        Me.btnType.UseVisualStyleBackColor = True
        '
        'lblMember
        '
        Me.lblMember.AutoSize = True
        Me.lblMember.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblMember.Location = New System.Drawing.Point(48, 149)
        Me.lblMember.Name = "lblMember"
        Me.lblMember.Size = New System.Drawing.Size(48, 32)
        Me.lblMember.TabIndex = 8
        Me.lblMember.Text = "Member:"
        Me.lblMember.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label6.Location = New System.Drawing.Point(37, 181)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(59, 97)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Comments:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReturnComment
        '
        Me.lblReturnComment.AutoSize = True
        Me.lblReturnComment.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblReturnComment.Location = New System.Drawing.Point(3, 91)
        Me.lblReturnComment.Name = "lblReturnComment"
        Me.lblReturnComment.Size = New System.Drawing.Size(93, 25)
        Me.lblReturnComment.TabIndex = 0
        Me.lblReturnComment.Text = "Return comments:"
        Me.lblReturnComment.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtReturnComments
        '
        Me.txtReturnComments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtReturnComments.Location = New System.Drawing.Point(102, 94)
        Me.txtReturnComments.Name = "txtReturnComments"
        Me.txtReturnComments.Size = New System.Drawing.Size(501, 20)
        Me.txtReturnComments.TabIndex = 1
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.cmbRange)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblImplementation)
        Me.FlowLayoutPanel1.Controls.Add(Me.cmbImplementation)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(102, 119)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(501, 27)
        Me.FlowLayoutPanel1.TabIndex = 13
        '
        'cmbRange
        '
        Me.cmbRange.FormattingEnabled = True
        Me.cmbRange.Location = New System.Drawing.Point(3, 3)
        Me.cmbRange.Name = "cmbRange"
        Me.cmbRange.Size = New System.Drawing.Size(102, 21)
        Me.cmbRange.TabIndex = 6
        '
        'lblImplementation
        '
        Me.lblImplementation.AutoSize = True
        Me.lblImplementation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblImplementation.Location = New System.Drawing.Point(111, 0)
        Me.lblImplementation.Name = "lblImplementation"
        Me.lblImplementation.Size = New System.Drawing.Size(81, 27)
        Me.lblImplementation.TabIndex = 7
        Me.lblImplementation.Text = "Implementation:"
        Me.lblImplementation.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbImplementation
        '
        Me.cmbImplementation.FormattingEnabled = True
        Me.cmbImplementation.Location = New System.Drawing.Point(198, 3)
        Me.cmbImplementation.Name = "cmbImplementation"
        Me.cmbImplementation.Size = New System.Drawing.Size(119, 21)
        Me.cmbImplementation.TabIndex = 9
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Controls.Add(Me.cmbMember)
        Me.FlowLayoutPanel2.Controls.Add(Me.chkConst)
        Me.FlowLayoutPanel2.Controls.Add(Me.chkInline)
        Me.FlowLayoutPanel2.Controls.Add(Me.lblBehaviour)
        Me.FlowLayoutPanel2.Controls.Add(Me.cmbBehaviour)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(102, 152)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(501, 26)
        Me.FlowLayoutPanel2.TabIndex = 14
        '
        'cmbMember
        '
        Me.cmbMember.FormattingEnabled = True
        Me.cmbMember.Location = New System.Drawing.Point(3, 3)
        Me.cmbMember.Name = "cmbMember"
        Me.cmbMember.Size = New System.Drawing.Size(82, 21)
        Me.cmbMember.TabIndex = 10
        '
        'chkConst
        '
        Me.chkConst.AutoSize = True
        Me.chkConst.CheckAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkConst.Location = New System.Drawing.Point(91, 3)
        Me.chkConst.Name = "chkConst"
        Me.chkConst.Size = New System.Drawing.Size(68, 17)
        Me.chkConst.TabIndex = 11
        Me.chkConst.Text = "Constant"
        Me.chkConst.UseVisualStyleBackColor = True
        '
        'chkInline
        '
        Me.chkInline.AutoSize = True
        Me.chkInline.CheckAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkInline.Location = New System.Drawing.Point(165, 3)
        Me.chkInline.Name = "chkInline"
        Me.chkInline.Size = New System.Drawing.Size(115, 17)
        Me.chkInline.TabIndex = 12
        Me.chkInline.Text = "Custom inline code"
        Me.chkInline.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkInline.UseVisualStyleBackColor = True
        '
        'lblBehaviour
        '
        Me.lblBehaviour.AutoSize = True
        Me.lblBehaviour.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblBehaviour.Location = New System.Drawing.Point(286, 0)
        Me.lblBehaviour.Name = "lblBehaviour"
        Me.lblBehaviour.Size = New System.Drawing.Size(58, 27)
        Me.lblBehaviour.TabIndex = 14
        Me.lblBehaviour.Text = "Behaviour:"
        Me.lblBehaviour.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbBehaviour
        '
        Me.cmbBehaviour.FormattingEnabled = True
        Me.cmbBehaviour.Location = New System.Drawing.Point(350, 3)
        Me.cmbBehaviour.Name = "cmbBehaviour"
        Me.cmbBehaviour.Size = New System.Drawing.Size(97, 21)
        Me.cmbBehaviour.TabIndex = 15
        '
        'grdParams
        '
        Me.grdParams.AllowDrop = True
        Me.grdParams.ColumnDragStart = 0
        Me.grdParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel3.SetColumnSpan(Me.grdParams, 2)
        Me.grdParams.ContextMenuStrip = Me.mnuMembers
        Me.grdParams.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdParams.Location = New System.Drawing.Point(3, 281)
        Me.grdParams.Name = "grdParams"
        Me.grdParams.Size = New System.Drawing.Size(600, 204)
        Me.grdParams.TabIndex = 15
        Me.grdParams.Tag = "param"
        Me.ToolTip1.SetToolTip(Me.grdParams, "Click right to update grid")
        '
        'mnuMembers
        '
        Me.mnuMembers.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddParam, Me.mnuAddException, Me.mnuEditParam, Me.ToolStripSeparator1, Me.mnuCopy, Me.mnuPaste, Me.mnuDuplicate, Me.mnuProperties, Me.ToolStripSeparator2, Me.mnuDelete})
        Me.mnuMembers.Name = "mnuParam"
        Me.mnuMembers.Size = New System.Drawing.Size(190, 192)
        '
        'mnuAddParam
        '
        Me.mnuAddParam.Name = "mnuAddParam"
        Me.mnuAddParam.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuAddParam.Size = New System.Drawing.Size(189, 22)
        Me.mnuAddParam.Text = "Add param"
        '
        'mnuAddException
        '
        Me.mnuAddException.Name = "mnuAddException"
        Me.mnuAddException.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuAddException.Size = New System.Drawing.Size(189, 22)
        Me.mnuAddException.Text = "Add exception..."
        '
        'mnuEditParam
        '
        Me.mnuEditParam.Image = Global.ClassXmlProject.My.Resources.Resources.Rename___Edit
        Me.mnuEditParam.Name = "mnuEditParam"
        Me.mnuEditParam.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuEditParam.Size = New System.Drawing.Size(189, 22)
        Me.mnuEditParam.Text = "Edit param..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(186, 6)
        '
        'mnuCopy
        '
        Me.mnuCopy.Image = Global.ClassXmlProject.My.Resources.Resources.Copy
        Me.mnuCopy.Name = "mnuCopy"
        Me.mnuCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuCopy.Size = New System.Drawing.Size(189, 22)
        Me.mnuCopy.Text = "Copy"
        '
        'mnuPaste
        '
        Me.mnuPaste.Image = Global.ClassXmlProject.My.Resources.Resources.Paste
        Me.mnuPaste.Name = "mnuPaste"
        Me.mnuPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuPaste.Size = New System.Drawing.Size(189, 22)
        Me.mnuPaste.Text = "Paste"
        '
        'mnuDuplicate
        '
        Me.mnuDuplicate.Name = "mnuDuplicate"
        Me.mnuDuplicate.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.mnuDuplicate.Size = New System.Drawing.Size(189, 22)
        Me.mnuDuplicate.Text = "Duplicate"
        '
        'mnuProperties
        '
        Me.mnuProperties.Name = "mnuProperties"
        Me.mnuProperties.Size = New System.Drawing.Size(189, 22)
        Me.mnuProperties.Text = "Properties..."
        Me.mnuProperties.Visible = False
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(186, 6)
        '
        'mnuDelete
        '
        Me.mnuDelete.Image = Global.ClassXmlProject.My.Resources.Resources.Stop_2
        Me.mnuDelete.Name = "mnuDelete"
        Me.mnuDelete.Size = New System.Drawing.Size(189, 22)
        Me.mnuDelete.Text = "Delete"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label7.Location = New System.Drawing.Point(31, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 33)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Return type:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FlowLayoutPanel3
        '
        Me.FlowLayoutPanel3.Controls.Add(Me.txtName)
        Me.FlowLayoutPanel3.Controls.Add(Me.chkOperator)
        Me.FlowLayoutPanel3.Controls.Add(Me.txtOperator)
        Me.FlowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel3.Location = New System.Drawing.Point(102, 36)
        Me.FlowLayoutPanel3.Name = "FlowLayoutPanel3"
        Me.FlowLayoutPanel3.Size = New System.Drawing.Size(501, 27)
        Me.FlowLayoutPanel3.TabIndex = 17
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(3, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(123, 20)
        Me.txtName.TabIndex = 2
        '
        'chkOperator
        '
        Me.chkOperator.AutoSize = True
        Me.chkOperator.CheckAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkOperator.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkOperator.Location = New System.Drawing.Point(132, 3)
        Me.chkOperator.Name = "chkOperator"
        Me.chkOperator.Size = New System.Drawing.Size(67, 20)
        Me.chkOperator.TabIndex = 3
        Me.chkOperator.Text = "Operator"
        Me.chkOperator.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.chkOperator.UseVisualStyleBackColor = True
        '
        'txtOperator
        '
        Me.txtOperator.Location = New System.Drawing.Point(205, 3)
        Me.txtOperator.Name = "txtOperator"
        Me.txtOperator.Size = New System.Drawing.Size(119, 20)
        Me.txtOperator.TabIndex = 4
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel3.SetColumnSpan(Me.TableLayoutPanel1, 2)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.55682!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.44318!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDelete, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 491)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(600, 35)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.Location = New System.Drawing.Point(519, 6)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(415, 6)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnDelete.Location = New System.Drawing.Point(3, 6)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(67, 23)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'txtComment
        '
        Me.txtComment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtComment.Location = New System.Drawing.Point(102, 184)
        Me.txtComment.Multiline = True
        Me.txtComment.Name = "txtComment"
        Me.txtComment.Size = New System.Drawing.Size(501, 91)
        Me.txtComment.TabIndex = 18
        '
        'dlgMethod
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(606, 529)
        Me.Controls.Add(Me.TableLayoutPanel3)
        Me.MinimizeBox = False
        Me.Name = "dlgMethod"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgMethod"
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.FlowLayoutPanel2.PerformLayout()
        CType(Me.grdParams, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuMembers.ResumeLayout(False)
        Me.FlowLayoutPanel3.ResumeLayout(False)
        Me.FlowLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents txtBrief As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents btnType As System.Windows.Forms.Button
    Friend WithEvents cmbRange As System.Windows.Forms.ComboBox
    Friend WithEvents lblRange As System.Windows.Forms.Label
    Friend WithEvents lblImplementation As System.Windows.Forms.Label
    Friend WithEvents lblMember As System.Windows.Forms.Label
    Friend WithEvents cmbImplementation As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMember As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblReturnComment As System.Windows.Forms.Label
    Friend WithEvents txtReturnComments As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents chkConst As System.Windows.Forms.CheckBox
    Friend WithEvents chkInline As System.Windows.Forms.CheckBox
    Friend WithEvents grdParams As ClassXmlProject.XmlDataGridView
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel3 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents chkOperator As System.Windows.Forms.CheckBox
    Friend WithEvents txtOperator As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblBehaviour As System.Windows.Forms.Label
    Friend WithEvents cmbBehaviour As System.Windows.Forms.ComboBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents txtComment As System.Windows.Forms.TextBox
    Friend WithEvents mnuMembers As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAddParam As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddException As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditParam As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDuplicate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuDelete As System.Windows.Forms.ToolStripMenuItem

End Class
