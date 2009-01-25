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
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbMember = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbGetAccess = New System.Windows.Forms.ComboBox
        Me.lblGetBy = New System.Windows.Forms.Label
        Me.cmbGetBy = New System.Windows.Forms.ComboBox
        Me.chkModifier = New System.Windows.Forms.CheckBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbSetAccess = New System.Windows.Forms.ComboBox
        Me.lblSetBy = New System.Windows.Forms.Label
        Me.cmbSetby = New System.Windows.Forms.ComboBox
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
        Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel
        Me.lblBehaviour = New System.Windows.Forms.Label
        Me.cmbBehaviour = New System.Windows.Forms.ComboBox
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel3.SuspendLayout()
        CType(Me.optTypeArray, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.FlowLayoutPanel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtBrief
        '
        Me.txtBrief.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBrief.Location = New System.Drawing.Point(88, 72)
        Me.txtBrief.Name = "txtBrief"
        Me.txtBrief.Size = New System.Drawing.Size(557, 20)
        Me.txtBrief.TabIndex = 4
        '
        'txtName
        '
        Me.txtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtName.Location = New System.Drawing.Point(88, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(557, 20)
        Me.txtName.TabIndex = 2
        '
        'cmdType
        '
        Me.cmdType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cmdType.Location = New System.Drawing.Point(3, 3)
        Me.cmdType.Name = "cmdType"
        Me.cmdType.Size = New System.Drawing.Size(334, 29)
        Me.cmdType.TabIndex = 5
        Me.cmdType.Text = "<Type>"
        Me.cmdType.UseVisualStyleBackColor = True
        '
        'cmbRange
        '
        Me.cmbRange.FormattingEnabled = True
        Me.cmbRange.Location = New System.Drawing.Point(3, 3)
        Me.cmbRange.Name = "cmbRange"
        Me.cmbRange.Size = New System.Drawing.Size(119, 21)
        Me.cmbRange.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(40, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 38)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Range:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label1.Location = New System.Drawing.Point(44, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 28)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Name:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.OK_Button.Location = New System.Drawing.Point(493, 7)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 69)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 27)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Brief comment:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbMember
        '
        Me.cmbMember.FormattingEnabled = True
        Me.cmbMember.Location = New System.Drawing.Point(182, 3)
        Me.cmbMember.Name = "cmbMember"
        Me.cmbMember.Size = New System.Drawing.Size(119, 21)
        Me.cmbMember.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(128, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 27)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Member:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(18, 134)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(64, 36)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Get access:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.cmbGetAccess)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblGetBy)
        Me.FlowLayoutPanel1.Controls.Add(Me.cmbGetBy)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkModifier)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(85, 134)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(563, 36)
        Me.FlowLayoutPanel1.TabIndex = 11
        '
        'cmbGetAccess
        '
        Me.cmbGetAccess.FormattingEnabled = True
        Me.cmbGetAccess.Location = New System.Drawing.Point(3, 3)
        Me.cmbGetAccess.Name = "cmbGetAccess"
        Me.cmbGetAccess.Size = New System.Drawing.Size(119, 21)
        Me.cmbGetAccess.TabIndex = 10
        '
        'lblGetBy
        '
        Me.lblGetBy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lblGetBy.AutoSize = True
        Me.lblGetBy.Location = New System.Drawing.Point(128, 0)
        Me.lblGetBy.Name = "lblGetBy"
        Me.lblGetBy.Size = New System.Drawing.Size(22, 27)
        Me.lblGetBy.TabIndex = 13
        Me.lblGetBy.Text = "By:"
        Me.lblGetBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbGetBy
        '
        Me.cmbGetBy.FormattingEnabled = True
        Me.cmbGetBy.Location = New System.Drawing.Point(156, 3)
        Me.cmbGetBy.Name = "cmbGetBy"
        Me.cmbGetBy.Size = New System.Drawing.Size(64, 21)
        Me.cmbGetBy.TabIndex = 11
        '
        'chkModifier
        '
        Me.chkModifier.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.chkModifier.AutoSize = True
        Me.chkModifier.Location = New System.Drawing.Point(226, 3)
        Me.chkModifier.Name = "chkModifier"
        Me.chkModifier.Size = New System.Drawing.Size(53, 21)
        Me.chkModifier.TabIndex = 12
        Me.chkModifier.Text = "Const"
        Me.chkModifier.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(19, 170)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(63, 29)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Set access:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Controls.Add(Me.cmbSetAccess)
        Me.FlowLayoutPanel2.Controls.Add(Me.lblSetBy)
        Me.FlowLayoutPanel2.Controls.Add(Me.cmbSetby)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(85, 170)
        Me.FlowLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(563, 29)
        Me.FlowLayoutPanel2.TabIndex = 13
        '
        'cmbSetAccess
        '
        Me.cmbSetAccess.FormattingEnabled = True
        Me.cmbSetAccess.Location = New System.Drawing.Point(3, 3)
        Me.cmbSetAccess.Name = "cmbSetAccess"
        Me.cmbSetAccess.Size = New System.Drawing.Size(119, 21)
        Me.cmbSetAccess.TabIndex = 10
        '
        'lblSetBy
        '
        Me.lblSetBy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lblSetBy.AutoSize = True
        Me.lblSetBy.Location = New System.Drawing.Point(128, 0)
        Me.lblSetBy.Name = "lblSetBy"
        Me.lblSetBy.Size = New System.Drawing.Size(22, 27)
        Me.lblSetBy.TabIndex = 13
        Me.lblSetBy.Text = "By:"
        Me.lblSetBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbSetby
        '
        Me.cmbSetby.FormattingEnabled = True
        Me.cmbSetby.Location = New System.Drawing.Point(156, 3)
        Me.cmbSetby.Name = "cmbSetby"
        Me.cmbSetby.Size = New System.Drawing.Size(64, 21)
        Me.cmbSetby.TabIndex = 11
        '
        'Label9
        '
        Me.Label9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(48, 28)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 41)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Type:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel3.SetColumnSpan(Me.TableLayoutPanel1, 2)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.61972!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.38028!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDelete, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 202)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(642, 38)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Cancel_Button.DialogResult = DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(568, 7)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left
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
        Me.FlowLayoutPanel3.Location = New System.Drawing.Point(343, 3)
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
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 217.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel3, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.cmdType, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(88, 31)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(557, 35)
        Me.TableLayoutPanel2.TabIndex = 16
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.18681!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.81319!))
        Me.TableLayoutPanel3.Controls.Add(Me.txtBrief, 1, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel2, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.Label7, 0, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.txtName, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel2, 1, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel1, 1, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.Label5, 0, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.Label9, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.Label3, 0, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.Label2, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel4, 1, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel1, 0, 6)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
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
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(648, 243)
        Me.TableLayoutPanel3.TabIndex = 17
        '
        'FlowLayoutPanel4
        '
        Me.FlowLayoutPanel4.Controls.Add(Me.cmbRange)
        Me.FlowLayoutPanel4.Controls.Add(Me.Label4)
        Me.FlowLayoutPanel4.Controls.Add(Me.cmbMember)
        Me.FlowLayoutPanel4.Controls.Add(Me.lblBehaviour)
        Me.FlowLayoutPanel4.Controls.Add(Me.cmbBehaviour)
        Me.FlowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel4.Location = New System.Drawing.Point(88, 99)
        Me.FlowLayoutPanel4.Name = "FlowLayoutPanel4"
        Me.FlowLayoutPanel4.Size = New System.Drawing.Size(557, 32)
        Me.FlowLayoutPanel4.TabIndex = 17
        '
        'lblBehaviour
        '
        Me.lblBehaviour.AutoSize = True
        Me.lblBehaviour.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblBehaviour.Location = New System.Drawing.Point(307, 0)
        Me.lblBehaviour.Name = "lblBehaviour"
        Me.lblBehaviour.Size = New System.Drawing.Size(58, 27)
        Me.lblBehaviour.TabIndex = 9
        Me.lblBehaviour.Text = "Behaviour:"
        Me.lblBehaviour.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbBehaviour
        '
        Me.cmbBehaviour.FormattingEnabled = True
        Me.cmbBehaviour.Location = New System.Drawing.Point(371, 3)
        Me.cmbBehaviour.Name = "cmbBehaviour"
        Me.cmbBehaviour.Size = New System.Drawing.Size(82, 21)
        Me.cmbBehaviour.TabIndex = 10
        '
        'dlgProperty
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(648, 243)
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
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtBrief As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents cmdType As System.Windows.Forms.Button
    Friend WithEvents cmbRange As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents cmbMember As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbGetAccess As System.Windows.Forms.ComboBox
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbGetBy As System.Windows.Forms.ComboBox
    Friend WithEvents chkModifier As System.Windows.Forms.CheckBox
    Friend WithEvents lblGetBy As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
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

End Class
