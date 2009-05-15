<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgProperty
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
        Me.txtBrief = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.cmdType = New System.Windows.Forms.Button
        Me.cmbRange = New System.Windows.Forms.ComboBox
        Me.lblName = New System.Windows.Forms.Label
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblGetAccess = New System.Windows.Forms.Label
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbGetAccess = New System.Windows.Forms.ComboBox
        Me.lblGetBy = New System.Windows.Forms.Label
        Me.cmbGetBy = New System.Windows.Forms.ComboBox
        Me.chkModifier = New System.Windows.Forms.CheckBox
        Me.chkGetInline = New System.Windows.Forms.CheckBox
        Me.lblSetAccess = New System.Windows.Forms.Label
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbSetAccess = New System.Windows.Forms.ComboBox
        Me.lblSetBy = New System.Windows.Forms.Label
        Me.cmbSetby = New System.Windows.Forms.ComboBox
        Me.chkSetInline = New System.Windows.Forms.CheckBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.FlowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel
        Me.optType_0 = New System.Windows.Forms.RadioButton
        Me.optType_1 = New System.Windows.Forms.RadioButton
        Me.optType_2 = New System.Windows.Forms.RadioButton
        Me.optTypeArray = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.chkAttribute = New System.Windows.Forms.CheckBox
        Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel
        Me.lblBehaviour = New System.Windows.Forms.Label
        Me.cmbBehaviour = New System.Windows.Forms.ComboBox
        Me.chkMember = New System.Windows.Forms.CheckBox
        Me.chkOverridable = New System.Windows.Forms.CheckBox
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel3.SuspendLayout()
        CType(Me.optTypeArray, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.FlowLayoutPanel4.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtBrief
        '
        Me.txtBrief.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBrief.Location = New System.Drawing.Point(107, 72)
        Me.txtBrief.Name = "txtBrief"
        Me.txtBrief.Size = New System.Drawing.Size(464, 20)
        Me.txtBrief.TabIndex = 4
        '
        'txtName
        '
        Me.txtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtName.Location = New System.Drawing.Point(107, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(464, 20)
        Me.txtName.TabIndex = 2
        '
        'cmdType
        '
        Me.cmdType.CausesValidation = False
        Me.cmdType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cmdType.Location = New System.Drawing.Point(3, 3)
        Me.cmdType.Name = "cmdType"
        Me.cmdType.Size = New System.Drawing.Size(241, 29)
        Me.cmdType.TabIndex = 5
        Me.cmdType.Text = "<Type>"
        Me.cmdType.UseVisualStyleBackColor = True
        '
        'cmbRange
        '
        Me.cmbRange.FormattingEnabled = True
        Me.cmbRange.Location = New System.Drawing.Point(3, 3)
        Me.cmbRange.Name = "cmbRange"
        Me.cmbRange.Size = New System.Drawing.Size(80, 21)
        Me.cmbRange.TabIndex = 6
        '
        'lblName
        '
        Me.lblName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblName.AutoSize = True
        Me.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblName.Location = New System.Drawing.Point(63, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 28)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.OK_Button.Location = New System.Drawing.Point(411, 7)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(57, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(24, 69)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 27)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Brief comment:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblGetAccess
        '
        Me.lblGetAccess.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblGetAccess.AutoSize = True
        Me.lblGetAccess.Location = New System.Drawing.Point(37, 134)
        Me.lblGetAccess.Name = "lblGetAccess"
        Me.lblGetAccess.Size = New System.Drawing.Size(64, 36)
        Me.lblGetAccess.TabIndex = 9
        Me.lblGetAccess.Text = "Get access:"
        Me.lblGetAccess.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.cmbGetAccess)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblGetBy)
        Me.FlowLayoutPanel1.Controls.Add(Me.cmbGetBy)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkModifier)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkGetInline)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(104, 134)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(470, 36)
        Me.FlowLayoutPanel1.TabIndex = 11
        '
        'cmbGetAccess
        '
        Me.cmbGetAccess.FormattingEnabled = True
        Me.cmbGetAccess.Location = New System.Drawing.Point(3, 3)
        Me.cmbGetAccess.Name = "cmbGetAccess"
        Me.cmbGetAccess.Size = New System.Drawing.Size(83, 21)
        Me.cmbGetAccess.TabIndex = 10
        '
        'lblGetBy
        '
        Me.lblGetBy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lblGetBy.AutoSize = True
        Me.lblGetBy.Location = New System.Drawing.Point(92, 0)
        Me.lblGetBy.Name = "lblGetBy"
        Me.lblGetBy.Size = New System.Drawing.Size(22, 27)
        Me.lblGetBy.TabIndex = 13
        Me.lblGetBy.Text = "By:"
        Me.lblGetBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbGetBy
        '
        Me.cmbGetBy.FormattingEnabled = True
        Me.cmbGetBy.Location = New System.Drawing.Point(120, 3)
        Me.cmbGetBy.Name = "cmbGetBy"
        Me.cmbGetBy.Size = New System.Drawing.Size(64, 21)
        Me.cmbGetBy.TabIndex = 11
        '
        'chkModifier
        '
        Me.chkModifier.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.chkModifier.AutoSize = True
        Me.chkModifier.Location = New System.Drawing.Point(190, 3)
        Me.chkModifier.Name = "chkModifier"
        Me.chkModifier.Size = New System.Drawing.Size(53, 21)
        Me.chkModifier.TabIndex = 12
        Me.chkModifier.Text = "Const"
        Me.chkModifier.UseVisualStyleBackColor = True
        '
        'chkGetInline
        '
        Me.chkGetInline.AutoSize = True
        Me.chkGetInline.CheckAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkGetInline.Location = New System.Drawing.Point(249, 3)
        Me.chkGetInline.Name = "chkGetInline"
        Me.chkGetInline.Size = New System.Drawing.Size(115, 17)
        Me.chkGetInline.TabIndex = 14
        Me.chkGetInline.Text = "Custom inline code"
        Me.chkGetInline.UseVisualStyleBackColor = True
        '
        'lblSetAccess
        '
        Me.lblSetAccess.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSetAccess.AutoSize = True
        Me.lblSetAccess.Location = New System.Drawing.Point(38, 170)
        Me.lblSetAccess.Name = "lblSetAccess"
        Me.lblSetAccess.Size = New System.Drawing.Size(63, 29)
        Me.lblSetAccess.TabIndex = 12
        Me.lblSetAccess.Text = "Set access:"
        Me.lblSetAccess.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Controls.Add(Me.cmbSetAccess)
        Me.FlowLayoutPanel2.Controls.Add(Me.lblSetBy)
        Me.FlowLayoutPanel2.Controls.Add(Me.cmbSetby)
        Me.FlowLayoutPanel2.Controls.Add(Me.chkSetInline)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(104, 170)
        Me.FlowLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(470, 29)
        Me.FlowLayoutPanel2.TabIndex = 13
        '
        'cmbSetAccess
        '
        Me.cmbSetAccess.FormattingEnabled = True
        Me.cmbSetAccess.Location = New System.Drawing.Point(3, 3)
        Me.cmbSetAccess.Name = "cmbSetAccess"
        Me.cmbSetAccess.Size = New System.Drawing.Size(83, 21)
        Me.cmbSetAccess.TabIndex = 10
        '
        'lblSetBy
        '
        Me.lblSetBy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lblSetBy.AutoSize = True
        Me.lblSetBy.Location = New System.Drawing.Point(92, 0)
        Me.lblSetBy.Name = "lblSetBy"
        Me.lblSetBy.Size = New System.Drawing.Size(22, 27)
        Me.lblSetBy.TabIndex = 13
        Me.lblSetBy.Text = "By:"
        Me.lblSetBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbSetby
        '
        Me.cmbSetby.FormattingEnabled = True
        Me.cmbSetby.Location = New System.Drawing.Point(120, 3)
        Me.cmbSetby.Name = "cmbSetby"
        Me.cmbSetby.Size = New System.Drawing.Size(64, 21)
        Me.cmbSetby.TabIndex = 11
        '
        'chkSetInline
        '
        Me.chkSetInline.AutoSize = True
        Me.chkSetInline.CheckAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkSetInline.Location = New System.Drawing.Point(190, 3)
        Me.chkSetInline.Name = "chkSetInline"
        Me.chkSetInline.Size = New System.Drawing.Size(115, 17)
        Me.chkSetInline.TabIndex = 14
        Me.chkSetInline.Text = "Custom inline code"
        Me.chkSetInline.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(67, 28)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 41)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Type:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.CausesValidation = False
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel3.SetColumnSpan(Me.TableLayoutPanel1, 2)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.61972!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.38028!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDelete, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 202)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(568, 38)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Cancel_Button.CausesValidation = False
        Me.Cancel_Button.Location = New System.Drawing.Point(474, 7)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnDelete.CausesValidation = False
        Me.btnDelete.Location = New System.Drawing.Point(3, 7)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(67, 23)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel3
        '
        Me.FlowLayoutPanel3.Controls.Add(Me.optType_0)
        Me.FlowLayoutPanel3.Controls.Add(Me.optType_1)
        Me.FlowLayoutPanel3.Controls.Add(Me.optType_2)
        Me.FlowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel3.Location = New System.Drawing.Point(250, 3)
        Me.FlowLayoutPanel3.Name = "FlowLayoutPanel3"
        Me.FlowLayoutPanel3.Size = New System.Drawing.Size(211, 29)
        Me.FlowLayoutPanel3.TabIndex = 15
        '
        'optType_0
        '
        Me.optType_0.AutoSize = True
        Me.optTypeArray.SetIndex(Me.optType_0, CType(0, Short))
        Me.optType_0.Location = New System.Drawing.Point(3, 3)
        Me.optType_0.Name = "optType_0"
        Me.optType_0.Size = New System.Drawing.Size(86, 17)
        Me.optType_0.TabIndex = 0
        Me.optType_0.TabStop = True
        Me.optType_0.Text = "Simp./Enum."
        Me.optType_0.UseVisualStyleBackColor = True
        '
        'optType_1
        '
        Me.optType_1.AutoSize = True
        Me.optTypeArray.SetIndex(Me.optType_1, CType(1, Short))
        Me.optType_1.Location = New System.Drawing.Point(95, 3)
        Me.optType_1.Name = "optType_1"
        Me.optType_1.Size = New System.Drawing.Size(50, 17)
        Me.optType_1.TabIndex = 1
        Me.optType_1.TabStop = True
        Me.optType_1.Text = "Cont."
        Me.optType_1.UseVisualStyleBackColor = True
        '
        'optType_2
        '
        Me.optType_2.AutoSize = True
        Me.optTypeArray.SetIndex(Me.optType_2, CType(2, Short))
        Me.optType_2.Location = New System.Drawing.Point(151, 3)
        Me.optType_2.Name = "optType_2"
        Me.optType_2.Size = New System.Drawing.Size(56, 17)
        Me.optType_2.TabIndex = 2
        Me.optType_2.TabStop = True
        Me.optType_2.Text = "Struct."
        Me.optType_2.UseVisualStyleBackColor = True
        '
        'optTypeArray
        '
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.CausesValidation = False
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 217.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel3, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.cmdType, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(107, 31)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(464, 35)
        Me.TableLayoutPanel2.TabIndex = 16
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.11847!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.88153!))
        Me.TableLayoutPanel3.Controls.Add(Me.chkAttribute, 0, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.txtBrief, 1, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel2, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.lblSetAccess, 0, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.txtName, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.lblName, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel2, 1, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel1, 1, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.lblGetAccess, 0, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.Label9, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.Label2, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel4, 1, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel1, 0, 6)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Right
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 7
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(574, 243)
        Me.TableLayoutPanel3.TabIndex = 17
        '
        'chkAttribute
        '
        Me.chkAttribute.AutoSize = True
        Me.chkAttribute.Dock = System.Windows.Forms.DockStyle.Right
        Me.chkAttribute.Location = New System.Drawing.Point(3, 99)
        Me.chkAttribute.Name = "chkAttribute"
        Me.chkAttribute.Size = New System.Drawing.Size(98, 32)
        Me.chkAttribute.TabIndex = 13
        Me.chkAttribute.Text = "Attribute range:"
        Me.chkAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAttribute.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel4
        '
        Me.FlowLayoutPanel4.Controls.Add(Me.cmbRange)
        Me.FlowLayoutPanel4.Controls.Add(Me.lblBehaviour)
        Me.FlowLayoutPanel4.Controls.Add(Me.cmbBehaviour)
        Me.FlowLayoutPanel4.Controls.Add(Me.chkMember)
        Me.FlowLayoutPanel4.Controls.Add(Me.chkOverridable)
        Me.FlowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel4.Location = New System.Drawing.Point(107, 99)
        Me.FlowLayoutPanel4.Name = "FlowLayoutPanel4"
        Me.FlowLayoutPanel4.Size = New System.Drawing.Size(464, 32)
        Me.FlowLayoutPanel4.TabIndex = 17
        '
        'lblBehaviour
        '
        Me.lblBehaviour.AutoSize = True
        Me.lblBehaviour.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblBehaviour.Location = New System.Drawing.Point(89, 0)
        Me.lblBehaviour.Name = "lblBehaviour"
        Me.lblBehaviour.Size = New System.Drawing.Size(58, 27)
        Me.lblBehaviour.TabIndex = 9
        Me.lblBehaviour.Text = "Behaviour:"
        Me.lblBehaviour.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbBehaviour
        '
        Me.cmbBehaviour.FormattingEnabled = True
        Me.cmbBehaviour.Location = New System.Drawing.Point(153, 3)
        Me.cmbBehaviour.Name = "cmbBehaviour"
        Me.cmbBehaviour.Size = New System.Drawing.Size(82, 21)
        Me.cmbBehaviour.TabIndex = 10
        '
        'chkMember
        '
        Me.chkMember.AutoSize = True
        Me.chkMember.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkMember.Location = New System.Drawing.Point(241, 3)
        Me.chkMember.Name = "chkMember"
        Me.chkMember.Size = New System.Drawing.Size(91, 21)
        Me.chkMember.TabIndex = 12
        Me.chkMember.Text = "Class member"
        Me.chkMember.UseVisualStyleBackColor = True
        '
        'chkOverridable
        '
        Me.chkOverridable.AutoSize = True
        Me.chkOverridable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkOverridable.Location = New System.Drawing.Point(338, 3)
        Me.chkOverridable.Name = "chkOverridable"
        Me.chkOverridable.Size = New System.Drawing.Size(80, 21)
        Me.chkOverridable.TabIndex = 11
        Me.chkOverridable.Text = "Overridable"
        Me.chkOverridable.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOverridable.UseVisualStyleBackColor = True
        '
        'errorProvider
        '
        Me.errorProvider.ContainerControl = Me
        '
        'dlgProperty
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(574, 243)
        Me.Controls.Add(Me.TableLayoutPanel3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgProperty"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgProperty"
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.FlowLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel3.ResumeLayout(False)
        Me.FlowLayoutPanel3.PerformLayout()
        CType(Me.optTypeArray, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.FlowLayoutPanel4.ResumeLayout(False)
        Me.FlowLayoutPanel4.PerformLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtBrief As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents cmdType As System.Windows.Forms.Button
    Friend WithEvents cmbRange As System.Windows.Forms.ComboBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents lblGetAccess As System.Windows.Forms.Label
    Friend WithEvents cmbGetAccess As System.Windows.Forms.ComboBox
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbGetBy As System.Windows.Forms.ComboBox
    Friend WithEvents chkModifier As System.Windows.Forms.CheckBox
    Friend WithEvents lblGetBy As System.Windows.Forms.Label
    Friend WithEvents lblSetAccess As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbSetAccess As System.Windows.Forms.ComboBox
    Friend WithEvents lblSetBy As System.Windows.Forms.Label
    Friend WithEvents cmbSetby As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel3 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents optType_0 As System.Windows.Forms.RadioButton
    Friend WithEvents optType_1 As System.Windows.Forms.RadioButton
    Friend WithEvents optType_2 As System.Windows.Forms.RadioButton
    Friend WithEvents optTypeArray As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents FlowLayoutPanel4 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents lblBehaviour As System.Windows.Forms.Label
    Friend WithEvents cmbBehaviour As System.Windows.Forms.ComboBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents chkOverridable As System.Windows.Forms.CheckBox
    Friend WithEvents chkMember As System.Windows.Forms.CheckBox
    Friend WithEvents chkAttribute As System.Windows.Forms.CheckBox
    Friend WithEvents chkGetInline As System.Windows.Forms.CheckBox
    Friend WithEvents chkSetInline As System.Windows.Forms.CheckBox
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider

End Class
