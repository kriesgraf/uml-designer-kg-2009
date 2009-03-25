<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgTypeVar
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
        Me.GroupType = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.optType_0 = New System.Windows.Forms.RadioButton
        Me.optType_1 = New System.Windows.Forms.RadioButton
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbTypedefs = New System.Windows.Forms.ComboBox
        Me.chkVisibility = New System.Windows.Forms.CheckBox
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel
        Me.chkTypeConst = New System.Windows.Forms.CheckBox
        Me.chkReference = New System.Windows.Forms.CheckBox
        Me.lblTypeLevel = New System.Windows.Forms.Label
        Me.cmbTypeLevel = New System.Windows.Forms.ComboBox
        Me.gridEnumeration = New ClassXmlProject.XmlDataGridView
        Me.mnuEnumValue = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAdd = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEnumProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupVariable = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.chkArray = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbValue = New System.Windows.Forms.ComboBox
        Me.cmbSize = New System.Windows.Forms.ComboBox
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.optTypeArray = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupType.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        CType(Me.gridEnumeration, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuEnumValue.SuspendLayout()
        Me.GroupVariable.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.optTypeArray, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(488, 248)
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
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'GroupType
        '
        Me.GroupType.Controls.Add(Me.TableLayoutPanel4)
        Me.GroupType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupType.Location = New System.Drawing.Point(3, 3)
        Me.GroupType.Name = "GroupType"
        Me.GroupType.Size = New System.Drawing.Size(631, 145)
        Me.GroupType.TabIndex = 1
        Me.GroupType.TabStop = False
        Me.GroupType.Text = "Type (left hand side)"
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.optType_0, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.optType_1, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.FlowLayoutPanel1, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.gridEnumeration, 1, 1)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 2
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(625, 126)
        Me.TableLayoutPanel4.TabIndex = 0
        '
        'optType_0
        '
        Me.optType_0.AutoSize = True
        Me.optType_0.Checked = True
        Me.optTypeArray.SetIndex(Me.optType_0, CType(0, Short))
        Me.optType_0.Location = New System.Drawing.Point(3, 3)
        Me.optType_0.Name = "optType_0"
        Me.optType_0.Size = New System.Drawing.Size(89, 17)
        Me.optType_0.TabIndex = 0
        Me.optType_0.TabStop = True
        Me.optType_0.Text = "Specific type:"
        Me.optType_0.UseVisualStyleBackColor = True
        '
        'optType_1
        '
        Me.optType_1.AutoSize = True
        Me.optTypeArray.SetIndex(Me.optType_1, CType(1, Short))
        Me.optType_1.Location = New System.Drawing.Point(3, 73)
        Me.optType_1.Name = "optType_1"
        Me.optType_1.Size = New System.Drawing.Size(87, 17)
        Me.optType_1.TabIndex = 1
        Me.optType_1.TabStop = True
        Me.optType_1.Text = "Enumeration:"
        Me.optType_1.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.Controls.Add(Me.cmbTypedefs)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkVisibility)
        Me.FlowLayoutPanel1.Controls.Add(Me.FlowLayoutPanel2)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(103, 3)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(519, 64)
        Me.FlowLayoutPanel1.TabIndex = 3
        '
        'cmbTypedefs
        '
        Me.cmbTypedefs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbTypedefs.FormattingEnabled = True
        Me.cmbTypedefs.Location = New System.Drawing.Point(3, 3)
        Me.cmbTypedefs.Name = "cmbTypedefs"
        Me.cmbTypedefs.Size = New System.Drawing.Size(345, 21)
        Me.cmbTypedefs.TabIndex = 0
        '
        'chkVisibility
        '
        Me.chkVisibility.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkVisibility.AutoSize = True
        Me.chkVisibility.Enabled = False
        Me.chkVisibility.Location = New System.Drawing.Point(354, 3)
        Me.chkVisibility.Name = "chkVisibility"
        Me.chkVisibility.Size = New System.Drawing.Size(92, 17)
        Me.chkVisibility.TabIndex = 1
        Me.chkVisibility.Text = "Only package"
        Me.chkVisibility.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Controls.Add(Me.chkTypeConst)
        Me.FlowLayoutPanel2.Controls.Add(Me.chkReference)
        Me.FlowLayoutPanel2.Controls.Add(Me.lblTypeLevel)
        Me.FlowLayoutPanel2.Controls.Add(Me.cmbTypeLevel)
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(3, 30)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(395, 30)
        Me.FlowLayoutPanel2.TabIndex = 8
        '
        'chkTypeConst
        '
        Me.chkTypeConst.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkTypeConst.AutoSize = True
        Me.chkTypeConst.Location = New System.Drawing.Point(3, 3)
        Me.chkTypeConst.Name = "chkTypeConst"
        Me.chkTypeConst.Size = New System.Drawing.Size(68, 21)
        Me.chkTypeConst.TabIndex = 7
        Me.chkTypeConst.Text = "Constant"
        Me.chkTypeConst.UseVisualStyleBackColor = True
        '
        'chkReference
        '
        Me.chkReference.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkReference.AutoSize = True
        Me.chkReference.Location = New System.Drawing.Point(77, 3)
        Me.chkReference.Name = "chkReference"
        Me.chkReference.Size = New System.Drawing.Size(86, 21)
        Me.chkReference.TabIndex = 6
        Me.chkReference.Text = "By reference"
        Me.chkReference.UseVisualStyleBackColor = True
        '
        'lblTypeLevel
        '
        Me.lblTypeLevel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblTypeLevel.AutoSize = True
        Me.lblTypeLevel.Location = New System.Drawing.Point(169, 7)
        Me.lblTypeLevel.Name = "lblTypeLevel"
        Me.lblTypeLevel.Size = New System.Drawing.Size(36, 13)
        Me.lblTypeLevel.TabIndex = 4
        Me.lblTypeLevel.Text = "Level:"
        Me.lblTypeLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbTypeLevel
        '
        Me.cmbTypeLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTypeLevel.FormattingEnabled = True
        Me.cmbTypeLevel.Location = New System.Drawing.Point(211, 3)
        Me.cmbTypeLevel.Name = "cmbTypeLevel"
        Me.cmbTypeLevel.Size = New System.Drawing.Size(97, 21)
        Me.cmbTypeLevel.TabIndex = 5
        '
        'gridEnumeration
        '
        Me.gridEnumeration.AllowDrop = True
        Me.gridEnumeration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridEnumeration.ContextMenuStrip = Me.mnuEnumValue
        Me.gridEnumeration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridEnumeration.Location = New System.Drawing.Point(103, 73)
        Me.gridEnumeration.Name = "gridEnumeration"
        Me.gridEnumeration.Size = New System.Drawing.Size(519, 54)
        Me.gridEnumeration.TabIndex = 4
        Me.gridEnumeration.Tag = "enumvalue"
        Me.ToolTip1.SetToolTip(Me.gridEnumeration, "Click right to update grid")
        '
        'mnuEnumValue
        '
        Me.mnuEnumValue.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAdd, Me.mnuEnumProperties, Me.ToolStripSeparator1, Me.mnuDelete})
        Me.mnuEnumValue.Name = "mnuEnumValue"
        Me.mnuEnumValue.Size = New System.Drawing.Size(142, 76)
        '
        'mnuAdd
        '
        Me.mnuAdd.Name = "mnuAdd"
        Me.mnuAdd.Size = New System.Drawing.Size(141, 22)
        Me.mnuAdd.Text = "Add"
        '
        'mnuEnumProperties
        '
        Me.mnuEnumProperties.Name = "mnuEnumProperties"
        Me.mnuEnumProperties.Size = New System.Drawing.Size(141, 22)
        Me.mnuEnumProperties.Text = "Parameters..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(138, 6)
        '
        'mnuDelete
        '
        Me.mnuDelete.Name = "mnuDelete"
        Me.mnuDelete.Size = New System.Drawing.Size(141, 22)
        Me.mnuDelete.Text = "Delete"
        '
        'GroupVariable
        '
        Me.GroupVariable.Controls.Add(Me.TableLayoutPanel3)
        Me.GroupVariable.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupVariable.Location = New System.Drawing.Point(3, 156)
        Me.GroupVariable.Name = "GroupVariable"
        Me.GroupVariable.Size = New System.Drawing.Size(631, 82)
        Me.GroupVariable.TabIndex = 2
        Me.GroupVariable.TabStop = False
        Me.GroupVariable.Text = "Variable (right hand side)"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 3
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.chkArray, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.Label1, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.Label2, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.cmbValue, 2, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.cmbSize, 2, 1)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(625, 63)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'chkArray
        '
        Me.chkArray.AutoSize = True
        Me.chkArray.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkArray.Location = New System.Drawing.Point(3, 35)
        Me.chkArray.Name = "chkArray"
        Me.chkArray.Size = New System.Drawing.Size(50, 26)
        Me.chkArray.TabIndex = 1
        Me.chkArray.Text = "Array"
        Me.chkArray.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(59, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 32)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Value:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Location = New System.Drawing.Point(59, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 32)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Size:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbValue
        '
        Me.cmbValue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbValue.FormattingEnabled = True
        Me.cmbValue.Location = New System.Drawing.Point(105, 3)
        Me.cmbValue.Name = "cmbValue"
        Me.cmbValue.Size = New System.Drawing.Size(517, 21)
        Me.cmbValue.TabIndex = 4
        '
        'cmbSize
        '
        Me.cmbSize.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbSize.FormattingEnabled = True
        Me.cmbSize.Location = New System.Drawing.Point(105, 35)
        Me.cmbSize.Name = "cmbSize"
        Me.cmbSize.Size = New System.Drawing.Size(517, 21)
        Me.cmbSize.TabIndex = 5
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.GroupVariable, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.GroupType, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 3
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(637, 280)
        Me.TableLayoutPanel2.TabIndex = 3
        '
        'dlgTypeVar
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(637, 280)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgTypeVar"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Type/Variable specifications"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupType.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.FlowLayoutPanel2.PerformLayout()
        CType(Me.gridEnumeration, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuEnumValue.ResumeLayout(False)
        Me.GroupVariable.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        CType(Me.optTypeArray, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GroupType As System.Windows.Forms.GroupBox
    Friend WithEvents GroupVariable As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents chkArray As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbValue As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSize As System.Windows.Forms.ComboBox
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents optType_0 As System.Windows.Forms.RadioButton
    Friend WithEvents optType_1 As System.Windows.Forms.RadioButton
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbTypedefs As System.Windows.Forms.ComboBox
    Friend WithEvents chkVisibility As System.Windows.Forms.CheckBox
    Friend WithEvents lblTypeLevel As System.Windows.Forms.Label
    Friend WithEvents cmbTypeLevel As System.Windows.Forms.ComboBox
    Friend WithEvents mnuEnumValue As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAdd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents optTypeArray As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Friend WithEvents gridEnumeration As XmlDataGridView
    Friend WithEvents chkReference As System.Windows.Forms.CheckBox
    Friend WithEvents chkTypeConst As System.Windows.Forms.CheckBox
    Friend WithEvents mnuEnumProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel

End Class
