<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgReference
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
        Me.tblLayout = New System.Windows.Forms.TableLayoutPanel
        Me.lblName = New System.Windows.Forms.Label
        Me.lblParentClass = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtParentClass = New System.Windows.Forms.TextBox
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblContainer = New System.Windows.Forms.Label
        Me.txtPackage = New System.Windows.Forms.TextBox
        Me.cmbContainer = New System.Windows.Forms.ComboBox
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.tblLayout.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.CausesValidation = False
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.tblLayout.SetColumnSpan(Me.TableLayoutPanel1, 4)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.83262!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.16738!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 74.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDelete, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 93)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(533, 43)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.CausesValidation = False
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(462, 10)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(385, 10)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnDelete.CausesValidation = False
        Me.btnDelete.Location = New System.Drawing.Point(3, 10)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(67, 23)
        Me.btnDelete.TabIndex = 3
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'tblLayout
        '
        Me.tblLayout.ColumnCount = 4
        Me.tblLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tblLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160.0!))
        Me.tblLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tblLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.tblLayout.Controls.Add(Me.TableLayoutPanel1, 0, 3)
        Me.tblLayout.Controls.Add(Me.lblName, 0, 0)
        Me.tblLayout.Controls.Add(Me.lblParentClass, 0, 1)
        Me.tblLayout.Controls.Add(Me.lblType, 0, 2)
        Me.tblLayout.Controls.Add(Me.txtName, 1, 0)
        Me.tblLayout.Controls.Add(Me.txtParentClass, 1, 1)
        Me.tblLayout.Controls.Add(Me.cmbType, 1, 2)
        Me.tblLayout.Controls.Add(Me.Label1, 2, 1)
        Me.tblLayout.Controls.Add(Me.lblContainer, 2, 2)
        Me.tblLayout.Controls.Add(Me.txtPackage, 3, 1)
        Me.tblLayout.Controls.Add(Me.cmbContainer, 3, 2)
        Me.tblLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblLayout.Location = New System.Drawing.Point(0, 0)
        Me.tblLayout.Name = "tblLayout"
        Me.tblLayout.RowCount = 3
        Me.tblLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tblLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tblLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tblLayout.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tblLayout.Size = New System.Drawing.Size(539, 139)
        Me.tblLayout.TabIndex = 1
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblName.Location = New System.Drawing.Point(39, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 30)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblParentClass
        '
        Me.lblParentClass.AutoSize = True
        Me.lblParentClass.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblParentClass.Location = New System.Drawing.Point(9, 30)
        Me.lblParentClass.Name = "lblParentClass"
        Me.lblParentClass.Size = New System.Drawing.Size(68, 30)
        Me.lblParentClass.TabIndex = 2
        Me.lblParentClass.Text = "Parent class:"
        Me.lblParentClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblType.Location = New System.Drawing.Point(43, 60)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(34, 30)
        Me.lblType.TabIndex = 3
        Me.lblType.Text = "Type:"
        Me.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtName
        '
        Me.tblLayout.SetColumnSpan(Me.txtName, 2)
        Me.txtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtName.Location = New System.Drawing.Point(83, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(234, 20)
        Me.txtName.TabIndex = 4
        '
        'txtParentClass
        '
        Me.txtParentClass.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtParentClass.Location = New System.Drawing.Point(83, 33)
        Me.txtParentClass.Name = "txtParentClass"
        Me.txtParentClass.Size = New System.Drawing.Size(154, 20)
        Me.txtParentClass.TabIndex = 5
        '
        'cmbType
        '
        Me.cmbType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(83, 63)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(154, 21)
        Me.cmbType.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label1.Location = New System.Drawing.Point(264, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 30)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Package:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblContainer
        '
        Me.lblContainer.AutoSize = True
        Me.lblContainer.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblContainer.Location = New System.Drawing.Point(262, 60)
        Me.lblContainer.Name = "lblContainer"
        Me.lblContainer.Size = New System.Drawing.Size(55, 30)
        Me.lblContainer.TabIndex = 8
        Me.lblContainer.Text = "Container:"
        Me.lblContainer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPackage
        '
        Me.txtPackage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPackage.Location = New System.Drawing.Point(323, 33)
        Me.txtPackage.Margin = New System.Windows.Forms.Padding(3, 3, 20, 3)
        Me.txtPackage.Name = "txtPackage"
        Me.txtPackage.Size = New System.Drawing.Size(196, 20)
        Me.txtPackage.TabIndex = 9
        '
        'cmbContainer
        '
        Me.cmbContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbContainer.FormattingEnabled = True
        Me.cmbContainer.Location = New System.Drawing.Point(323, 63)
        Me.cmbContainer.Name = "cmbContainer"
        Me.cmbContainer.Size = New System.Drawing.Size(213, 21)
        Me.cmbContainer.TabIndex = 10
        '
        'errorProvider
        '
        Me.errorProvider.ContainerControl = Me
        '
        'dlgReference
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(539, 139)
        Me.Controls.Add(Me.tblLayout)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgReference"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgReference"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.tblLayout.ResumeLayout(False)
        Me.tblLayout.PerformLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents tblLayout As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblParentClass As System.Windows.Forms.Label
    Friend WithEvents lblType As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtParentClass As System.Windows.Forms.TextBox
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblContainer As System.Windows.Forms.Label
    Friend WithEvents txtPackage As System.Windows.Forms.TextBox
    Friend WithEvents cmbContainer As System.Windows.Forms.ComboBox
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents btnDelete As System.Windows.Forms.Button

End Class
