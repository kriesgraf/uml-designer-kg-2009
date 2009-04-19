<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgRedundancy
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
        Me.Ignore_All = New System.Windows.Forms.Button
        Me.lsbRedundantClasses = New System.Windows.Forms.ListBox
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.lsbRemainClasses = New System.Windows.Forms.ListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.lblMessage = New System.Windows.Forms.Label
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.optDisplay_0 = New System.Windows.Forms.RadioButton
        Me.optDisplay_1 = New System.Windows.Forms.RadioButton
        Me.optDisplay_2 = New System.Windows.Forms.RadioButton
        Me.optDisplayArray = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.optDisplayArray, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.5969!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.62016!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.09244!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Ignore_All, 2, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(253, 352)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(258, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(11, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(75, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(102, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(68, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Ignore"
        '
        'Ignore_All
        '
        Me.Ignore_All.Location = New System.Drawing.Point(179, 3)
        Me.Ignore_All.Name = "Ignore_All"
        Me.Ignore_All.Size = New System.Drawing.Size(70, 23)
        Me.Ignore_All.TabIndex = 3
        Me.Ignore_All.Text = "Ignore all"
        Me.Ignore_All.UseVisualStyleBackColor = True
        '
        'lsbRedundantClasses
        '
        Me.lsbRedundantClasses.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbRedundantClasses.FormattingEnabled = True
        Me.lsbRedundantClasses.Location = New System.Drawing.Point(3, 103)
        Me.lsbRedundantClasses.Name = "lsbRedundantClasses"
        Me.lsbRedundantClasses.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lsbRedundantClasses.Size = New System.Drawing.Size(508, 95)
        Me.lsbRedundantClasses.TabIndex = 1
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.lsbRedundantClasses, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label1, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lsbRemainClasses, 0, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.Label2, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 0, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel3, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 6
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(514, 384)
        Me.TableLayoutPanel2.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(178, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Select objects that remain in project:"
        '
        'lsbRemainClasses
        '
        Me.lsbRemainClasses.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbRemainClasses.FormattingEnabled = True
        Me.lsbRemainClasses.Location = New System.Drawing.Point(3, 239)
        Me.lsbRemainClasses.Name = "lsbRemainClasses"
        Me.lsbRemainClasses.Size = New System.Drawing.Size(508, 95)
        Me.lsbRemainClasses.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Location = New System.Drawing.Point(3, 213)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(508, 23)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Choose one to replace others (not selected above):"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 188.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.lblMessage, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel1, 1, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(508, 74)
        Me.TableLayoutPanel3.TabIndex = 6
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblMessage.Location = New System.Drawing.Point(3, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(314, 74)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.Text = "Label3"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.optDisplay_0)
        Me.FlowLayoutPanel1.Controls.Add(Me.optDisplay_1)
        Me.FlowLayoutPanel1.Controls.Add(Me.optDisplay_2)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(323, 3)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(182, 68)
        Me.FlowLayoutPanel1.TabIndex = 1
        '
        'optDisplay_0
        '
        Me.optDisplay_0.AutoSize = True
        Me.optDisplay_0.Checked = True
        Me.optDisplayArray.SetIndex(Me.optDisplay_0, CType(0, Short))
        Me.optDisplay_0.Location = New System.Drawing.Point(3, 3)
        Me.optDisplay_0.Name = "optDisplay_0"
        Me.optDisplay_0.Size = New System.Drawing.Size(91, 17)
        Me.optDisplay_0.TabIndex = 0
        Me.optDisplay_0.TabStop = True
        Me.optDisplay_0.Text = "Full UML path"
        Me.optDisplay_0.UseVisualStyleBackColor = True
        '
        'optDisplay_1
        '
        Me.optDisplay_1.AutoSize = True
        Me.optDisplayArray.SetIndex(Me.optDisplay_1, CType(1, Short))
        Me.optDisplay_1.Location = New System.Drawing.Point(3, 26)
        Me.optDisplay_1.Name = "optDisplay_1"
        Me.optDisplay_1.Size = New System.Drawing.Size(111, 17)
        Me.optDisplay_1.TabIndex = 1
        Me.optDisplay_1.TabStop = True
        Me.optDisplay_1.Text = "Full Package path"
        Me.optDisplay_1.UseVisualStyleBackColor = True
        '
        'optDisplay_2
        '
        Me.optDisplay_2.AutoSize = True
        Me.optDisplayArray.SetIndex(Me.optDisplay_2, CType(2, Short))
        Me.optDisplay_2.Location = New System.Drawing.Point(3, 49)
        Me.optDisplay_2.Name = "optDisplay_2"
        Me.optDisplay_2.Size = New System.Drawing.Size(69, 17)
        Me.optDisplay_2.TabIndex = 2
        Me.optDisplay_2.TabStop = True
        Me.optDisplay_2.Text = "More info"
        Me.optDisplay_2.UseVisualStyleBackColor = True
        '
        'dlgRedundancy
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(514, 384)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgRedundancy"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgRedundancy"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        CType(Me.optDisplayArray, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents lsbRedundantClasses As System.Windows.Forms.ListBox
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Ignore_All As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lsbRemainClasses As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents optDisplay_0 As System.Windows.Forms.RadioButton
    Friend WithEvents optDisplay_1 As System.Windows.Forms.RadioButton
    Friend WithEvents optDisplay_2 As System.Windows.Forms.RadioButton
    Friend WithEvents optDisplayArray As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray

End Class
