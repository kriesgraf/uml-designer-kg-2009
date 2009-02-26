<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgContainer
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtBrief = New System.Windows.Forms.TextBox
        Me.flpRange = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbRange = New System.Windows.Forms.ComboBox
        Me.lblLevel = New System.Windows.Forms.Label
        Me.cmbLevel = New System.Windows.Forms.ComboBox
        Me.chkModifier = New System.Windows.Forms.CheckBox
        Me.flpIndex = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbIndex = New System.Windows.Forms.ComboBox
        Me.chkIndexPackage = New System.Windows.Forms.CheckBox
        Me.chkIndex = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.flpContainer = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbContainer = New System.Windows.Forms.ComboBox
        Me.chkPackageContainer = New System.Windows.Forms.CheckBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.chkTypePackage = New System.Windows.Forms.CheckBox
        Me.flpIndexLevel = New System.Windows.Forms.FlowLayoutPanel
        Me.chkIterator = New System.Windows.Forms.CheckBox
        Me.lblIndexLevel = New System.Windows.Forms.Label
        Me.cmbIndexLevel = New System.Windows.Forms.ComboBox
        Me.btnDelete = New System.Windows.Forms.Button
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.flpRange.SuspendLayout()
        Me.flpIndex.SuspendLayout()
        Me.flpContainer.SuspendLayout()
        Me.FlowLayoutPanel4.SuspendLayout()
        Me.flpIndexLevel.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel2.SetColumnSpan(Me.TableLayoutPanel1, 2)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.20834!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.79167!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDelete, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 218)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(457, 34)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(312, 5)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(387, 5)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.57143!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.42857!))
        Me.TableLayoutPanel2.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.txtName, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.txtBrief, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.flpRange, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.flpIndex, 1, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.chkIndex, 0, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.Label5, 0, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.flpContainer, 1, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.Label6, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel4, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.flpIndexLevel, 1, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 0, 8)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 9
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(463, 255)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label1.Location = New System.Drawing.Point(44, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 25)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Name:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label2.Location = New System.Drawing.Point(28, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 25)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Comment:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label3.Location = New System.Drawing.Point(40, 50)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 33)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Range:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtName
        '
        Me.txtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtName.Location = New System.Drawing.Point(88, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(372, 20)
        Me.txtName.TabIndex = 4
        '
        'txtBrief
        '
        Me.txtBrief.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBrief.Location = New System.Drawing.Point(88, 28)
        Me.txtBrief.Name = "txtBrief"
        Me.txtBrief.Size = New System.Drawing.Size(372, 20)
        Me.txtBrief.TabIndex = 5
        '
        'flpRange
        '
        Me.flpRange.Controls.Add(Me.cmbRange)
        Me.flpRange.Controls.Add(Me.lblLevel)
        Me.flpRange.Controls.Add(Me.cmbLevel)
        Me.flpRange.Controls.Add(Me.chkModifier)
        Me.flpRange.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpRange.Location = New System.Drawing.Point(88, 53)
        Me.flpRange.Name = "flpRange"
        Me.flpRange.Size = New System.Drawing.Size(372, 27)
        Me.flpRange.TabIndex = 7
        '
        'cmbRange
        '
        Me.cmbRange.FormattingEnabled = True
        Me.cmbRange.Location = New System.Drawing.Point(3, 3)
        Me.cmbRange.Name = "cmbRange"
        Me.cmbRange.Size = New System.Drawing.Size(94, 21)
        Me.cmbRange.TabIndex = 6
        '
        'lblLevel
        '
        Me.lblLevel.AutoSize = True
        Me.lblLevel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblLevel.Location = New System.Drawing.Point(103, 0)
        Me.lblLevel.Name = "lblLevel"
        Me.lblLevel.Size = New System.Drawing.Size(36, 27)
        Me.lblLevel.TabIndex = 7
        Me.lblLevel.Text = "Level:"
        Me.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbLevel
        '
        Me.cmbLevel.FormattingEnabled = True
        Me.cmbLevel.Location = New System.Drawing.Point(145, 3)
        Me.cmbLevel.Name = "cmbLevel"
        Me.cmbLevel.Size = New System.Drawing.Size(91, 21)
        Me.cmbLevel.TabIndex = 8
        '
        'chkModifier
        '
        Me.chkModifier.AutoSize = True
        Me.chkModifier.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkModifier.Location = New System.Drawing.Point(242, 3)
        Me.chkModifier.Name = "chkModifier"
        Me.chkModifier.Size = New System.Drawing.Size(68, 21)
        Me.chkModifier.TabIndex = 9
        Me.chkModifier.Text = "Constant"
        Me.chkModifier.UseVisualStyleBackColor = True
        '
        'flpIndex
        '
        Me.flpIndex.Controls.Add(Me.cmbIndex)
        Me.flpIndex.Controls.Add(Me.chkIndexPackage)
        Me.flpIndex.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpIndex.Location = New System.Drawing.Point(88, 152)
        Me.flpIndex.Name = "flpIndex"
        Me.flpIndex.Size = New System.Drawing.Size(372, 27)
        Me.flpIndex.TabIndex = 11
        '
        'cmbIndex
        '
        Me.cmbIndex.FormattingEnabled = True
        Me.cmbIndex.Location = New System.Drawing.Point(3, 3)
        Me.cmbIndex.Name = "cmbIndex"
        Me.cmbIndex.Size = New System.Drawing.Size(257, 21)
        Me.cmbIndex.TabIndex = 0
        '
        'chkIndexPackage
        '
        Me.chkIndexPackage.AutoSize = True
        Me.chkIndexPackage.Enabled = False
        Me.chkIndexPackage.Location = New System.Drawing.Point(266, 3)
        Me.chkIndexPackage.Name = "chkIndexPackage"
        Me.chkIndexPackage.Size = New System.Drawing.Size(91, 17)
        Me.chkIndexPackage.TabIndex = 1
        Me.chkIndexPackage.Text = "Package only"
        Me.chkIndexPackage.UseVisualStyleBackColor = True
        '
        'chkIndex
        '
        Me.chkIndex.AutoSize = True
        Me.chkIndex.Dock = System.Windows.Forms.DockStyle.Right
        Me.chkIndex.Location = New System.Drawing.Point(27, 152)
        Me.chkIndex.Name = "chkIndex"
        Me.chkIndex.Size = New System.Drawing.Size(55, 27)
        Me.chkIndex.TabIndex = 9
        Me.chkIndex.Text = "Index:"
        Me.chkIndex.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label5.Location = New System.Drawing.Point(27, 116)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 33)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Container:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'flpContainer
        '
        Me.flpContainer.Controls.Add(Me.cmbContainer)
        Me.flpContainer.Controls.Add(Me.chkPackageContainer)
        Me.flpContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpContainer.Location = New System.Drawing.Point(88, 119)
        Me.flpContainer.Name = "flpContainer"
        Me.flpContainer.Size = New System.Drawing.Size(372, 27)
        Me.flpContainer.TabIndex = 10
        '
        'cmbContainer
        '
        Me.cmbContainer.FormattingEnabled = True
        Me.cmbContainer.Location = New System.Drawing.Point(3, 3)
        Me.cmbContainer.Name = "cmbContainer"
        Me.cmbContainer.Size = New System.Drawing.Size(257, 21)
        Me.cmbContainer.TabIndex = 0
        '
        'chkPackageContainer
        '
        Me.chkPackageContainer.AutoSize = True
        Me.chkPackageContainer.Dock = System.Windows.Forms.DockStyle.Right
        Me.chkPackageContainer.Enabled = False
        Me.chkPackageContainer.Location = New System.Drawing.Point(266, 3)
        Me.chkPackageContainer.Name = "chkPackageContainer"
        Me.chkPackageContainer.Size = New System.Drawing.Size(91, 21)
        Me.chkPackageContainer.TabIndex = 1
        Me.chkPackageContainer.Text = "Package only"
        Me.chkPackageContainer.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label6.Location = New System.Drawing.Point(48, 83)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 33)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Type:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FlowLayoutPanel4
        '
        Me.FlowLayoutPanel4.Controls.Add(Me.cmbType)
        Me.FlowLayoutPanel4.Controls.Add(Me.chkTypePackage)
        Me.FlowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel4.Location = New System.Drawing.Point(88, 86)
        Me.FlowLayoutPanel4.Name = "FlowLayoutPanel4"
        Me.FlowLayoutPanel4.Size = New System.Drawing.Size(372, 27)
        Me.FlowLayoutPanel4.TabIndex = 13
        '
        'cmbType
        '
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(3, 3)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(257, 21)
        Me.cmbType.TabIndex = 0
        '
        'chkTypePackage
        '
        Me.chkTypePackage.AutoSize = True
        Me.chkTypePackage.Enabled = False
        Me.chkTypePackage.Location = New System.Drawing.Point(266, 3)
        Me.chkTypePackage.Name = "chkTypePackage"
        Me.chkTypePackage.Size = New System.Drawing.Size(91, 17)
        Me.chkTypePackage.TabIndex = 1
        Me.chkTypePackage.Text = "Package only"
        Me.chkTypePackage.UseVisualStyleBackColor = True
        '
        'flpIndexLevel
        '
        Me.flpIndexLevel.Controls.Add(Me.chkIterator)
        Me.flpIndexLevel.Controls.Add(Me.lblIndexLevel)
        Me.flpIndexLevel.Controls.Add(Me.cmbIndexLevel)
        Me.flpIndexLevel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpIndexLevel.Location = New System.Drawing.Point(88, 185)
        Me.flpIndexLevel.Name = "flpIndexLevel"
        Me.flpIndexLevel.Size = New System.Drawing.Size(372, 27)
        Me.flpIndexLevel.TabIndex = 14
        '
        'chkIterator
        '
        Me.chkIterator.AutoSize = True
        Me.chkIterator.Location = New System.Drawing.Point(3, 3)
        Me.chkIterator.Name = "chkIterator"
        Me.chkIterator.Size = New System.Drawing.Size(59, 17)
        Me.chkIterator.TabIndex = 0
        Me.chkIterator.Text = "Iterator"
        Me.chkIterator.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkIterator.UseVisualStyleBackColor = True
        '
        'lblIndexLevel
        '
        Me.lblIndexLevel.AutoSize = True
        Me.lblIndexLevel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblIndexLevel.Location = New System.Drawing.Point(68, 0)
        Me.lblIndexLevel.Name = "lblIndexLevel"
        Me.lblIndexLevel.Size = New System.Drawing.Size(61, 27)
        Me.lblIndexLevel.TabIndex = 1
        Me.lblIndexLevel.Text = "Index level:"
        Me.lblIndexLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbIndexLevel
        '
        Me.cmbIndexLevel.FormattingEnabled = True
        Me.cmbIndexLevel.Location = New System.Drawing.Point(135, 3)
        Me.cmbIndexLevel.Name = "cmbIndexLevel"
        Me.cmbIndexLevel.Size = New System.Drawing.Size(97, 21)
        Me.cmbIndexLevel.TabIndex = 2
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnDelete.Location = New System.Drawing.Point(3, 5)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(67, 23)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'dlgContainer
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(463, 255)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgContainer"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgContainer"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.flpRange.ResumeLayout(False)
        Me.flpRange.PerformLayout()
        Me.flpIndex.ResumeLayout(False)
        Me.flpIndex.PerformLayout()
        Me.flpContainer.ResumeLayout(False)
        Me.flpContainer.PerformLayout()
        Me.FlowLayoutPanel4.ResumeLayout(False)
        Me.FlowLayoutPanel4.PerformLayout()
        Me.flpIndexLevel.ResumeLayout(False)
        Me.flpIndexLevel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtBrief As System.Windows.Forms.TextBox
    Friend WithEvents flpRange As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbRange As System.Windows.Forms.ComboBox
    Friend WithEvents lblLevel As System.Windows.Forms.Label
    Friend WithEvents cmbLevel As System.Windows.Forms.ComboBox
    Friend WithEvents chkModifier As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkIndex As System.Windows.Forms.CheckBox
    Friend WithEvents flpContainer As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpIndex As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbContainer As System.Windows.Forms.ComboBox
    Friend WithEvents chkPackageContainer As System.Windows.Forms.CheckBox
    Friend WithEvents cmbIndex As System.Windows.Forms.ComboBox
    Friend WithEvents chkIndexPackage As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel4 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents chkTypePackage As System.Windows.Forms.CheckBox
    Friend WithEvents flpIndexLevel As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents chkIterator As System.Windows.Forms.CheckBox
    Friend WithEvents lblIndexLevel As System.Windows.Forms.Label
    Friend WithEvents cmbIndexLevel As System.Windows.Forms.ComboBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button

End Class
