<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgRelationParent
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
        Me.chkIterator = New System.Windows.Forms.CheckBox
        Me.optType_0 = New System.Windows.Forms.RadioButton
        Me.optType_1 = New System.Windows.Forms.RadioButton
        Me.chkIndex = New System.Windows.Forms.CheckBox
        Me.flpIndex = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbIndex = New System.Windows.Forms.ComboBox
        Me.chkPackage = New System.Windows.Forms.CheckBox
        Me.cmbContainer = New System.Windows.Forms.ComboBox
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel
        Me.lblIndexLevel = New System.Windows.Forms.Label
        Me.cmbIndexLevel = New System.Windows.Forms.ComboBox
        Me.cmbArraySize = New System.Windows.Forms.ComboBox
        Me.lblContainer = New System.Windows.Forms.Label
        Me.optTypeArray = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.flpIndex.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(273, 123)
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
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 3
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.chkIterator, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.optType_0, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 2, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.optType_1, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.chkIndex, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.flpIndex, 2, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.cmbContainer, 2, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel2, 2, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.cmbArraySize, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblContainer, 1, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 5
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(422, 155)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'chkIterator
        '
        Me.chkIterator.AutoSize = True
        Me.chkIterator.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkIterator.Location = New System.Drawing.Point(61, 86)
        Me.chkIterator.Name = "chkIterator"
        Me.chkIterator.Size = New System.Drawing.Size(59, 27)
        Me.chkIterator.TabIndex = 6
        Me.chkIterator.Text = "Iterator"
        Me.chkIterator.UseVisualStyleBackColor = True
        '
        'optType_0
        '
        Me.optType_0.AutoSize = True
        Me.optType_0.Dock = System.Windows.Forms.DockStyle.Left
        Me.optTypeArray.SetIndex(Me.optType_0, CType(0, Short))
        Me.optType_0.Location = New System.Drawing.Point(3, 3)
        Me.optType_0.Name = "optType_0"
        Me.optType_0.Size = New System.Drawing.Size(52, 19)
        Me.optType_0.TabIndex = 0
        Me.optType_0.TabStop = True
        Me.optType_0.Text = "Array:"
        Me.optType_0.UseVisualStyleBackColor = True
        '
        'optType_1
        '
        Me.optType_1.AutoSize = True
        Me.optType_1.Dock = System.Windows.Forms.DockStyle.Left
        Me.optTypeArray.SetIndex(Me.optType_1, CType(1, Short))
        Me.optType_1.Location = New System.Drawing.Point(3, 28)
        Me.optType_1.Name = "optType_1"
        Me.optType_1.Size = New System.Drawing.Size(44, 19)
        Me.optType_1.TabIndex = 1
        Me.optType_1.TabStop = True
        Me.optType_1.Text = "List:"
        Me.optType_1.UseVisualStyleBackColor = True
        '
        'chkIndex
        '
        Me.chkIndex.AutoSize = True
        Me.chkIndex.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkIndex.Location = New System.Drawing.Point(61, 53)
        Me.chkIndex.Name = "chkIndex"
        Me.chkIndex.Size = New System.Drawing.Size(59, 27)
        Me.chkIndex.TabIndex = 2
        Me.chkIndex.Text = "Index:"
        Me.chkIndex.UseVisualStyleBackColor = True
        '
        'flpIndex
        '
        Me.flpIndex.Controls.Add(Me.cmbIndex)
        Me.flpIndex.Controls.Add(Me.chkPackage)
        Me.flpIndex.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpIndex.Location = New System.Drawing.Point(126, 53)
        Me.flpIndex.Name = "flpIndex"
        Me.flpIndex.Size = New System.Drawing.Size(293, 27)
        Me.flpIndex.TabIndex = 3
        '
        'cmbIndex
        '
        Me.cmbIndex.FormattingEnabled = True
        Me.cmbIndex.Location = New System.Drawing.Point(3, 3)
        Me.cmbIndex.Name = "cmbIndex"
        Me.cmbIndex.Size = New System.Drawing.Size(187, 21)
        Me.cmbIndex.TabIndex = 0
        '
        'chkPackage
        '
        Me.chkPackage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkPackage.AutoSize = True
        Me.chkPackage.Enabled = False
        Me.chkPackage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.chkPackage.Location = New System.Drawing.Point(196, 3)
        Me.chkPackage.Name = "chkPackage"
        Me.chkPackage.Size = New System.Drawing.Size(92, 21)
        Me.chkPackage.TabIndex = 1
        Me.chkPackage.Text = "Only package"
        Me.chkPackage.UseVisualStyleBackColor = True
        '
        'cmbContainer
        '
        Me.cmbContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbContainer.FormattingEnabled = True
        Me.cmbContainer.Location = New System.Drawing.Point(126, 28)
        Me.cmbContainer.Name = "cmbContainer"
        Me.cmbContainer.Size = New System.Drawing.Size(293, 21)
        Me.cmbContainer.TabIndex = 4
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Controls.Add(Me.lblIndexLevel)
        Me.FlowLayoutPanel2.Controls.Add(Me.cmbIndexLevel)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(126, 86)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(293, 27)
        Me.FlowLayoutPanel2.TabIndex = 7
        '
        'lblIndexLevel
        '
        Me.lblIndexLevel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblIndexLevel.AutoSize = True
        Me.lblIndexLevel.Location = New System.Drawing.Point(3, 0)
        Me.lblIndexLevel.Name = "lblIndexLevel"
        Me.lblIndexLevel.Size = New System.Drawing.Size(61, 27)
        Me.lblIndexLevel.TabIndex = 7
        Me.lblIndexLevel.Text = "Index level:"
        Me.lblIndexLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbIndexLevel
        '
        Me.cmbIndexLevel.FormattingEnabled = True
        Me.cmbIndexLevel.Location = New System.Drawing.Point(70, 3)
        Me.cmbIndexLevel.Name = "cmbIndexLevel"
        Me.cmbIndexLevel.Size = New System.Drawing.Size(120, 21)
        Me.cmbIndexLevel.TabIndex = 8
        '
        'cmbArraySize
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.cmbArraySize, 2)
        Me.cmbArraySize.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbArraySize.FormattingEnabled = True
        Me.cmbArraySize.Location = New System.Drawing.Point(61, 3)
        Me.cmbArraySize.Name = "cmbArraySize"
        Me.cmbArraySize.Size = New System.Drawing.Size(358, 21)
        Me.cmbArraySize.TabIndex = 5
        '
        'lblContainer
        '
        Me.lblContainer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblContainer.AutoSize = True
        Me.lblContainer.Location = New System.Drawing.Point(65, 25)
        Me.lblContainer.Name = "lblContainer"
        Me.lblContainer.Size = New System.Drawing.Size(55, 25)
        Me.lblContainer.TabIndex = 8
        Me.lblContainer.Text = "Container:"
        Me.lblContainer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'optTypeArray
        '
        '
        'dlgRelationParent
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(422, 155)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgRelationParent"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgRelationParent"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.flpIndex.ResumeLayout(False)
        Me.flpIndex.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.FlowLayoutPanel2.PerformLayout()
        CType(Me.optTypeArray, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents optType_0 As System.Windows.Forms.RadioButton
    Friend WithEvents optType_1 As System.Windows.Forms.RadioButton
    Friend WithEvents chkIndex As System.Windows.Forms.CheckBox
    Friend WithEvents flpIndex As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbIndex As System.Windows.Forms.ComboBox
    Friend WithEvents chkPackage As System.Windows.Forms.CheckBox
    Friend WithEvents cmbContainer As System.Windows.Forms.ComboBox
    Friend WithEvents cmbArraySize As System.Windows.Forms.ComboBox
    Friend WithEvents chkIterator As System.Windows.Forms.CheckBox
    Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents lblIndexLevel As System.Windows.Forms.Label
    Friend WithEvents cmbIndexLevel As System.Windows.Forms.ComboBox
    Friend WithEvents lblContainer As System.Windows.Forms.Label
    Friend WithEvents optTypeArray As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray

End Class
