<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgRelation
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
        Me.tabButtons = New System.Windows.Forms.TableLayoutPanel
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.tabRelationship = New System.Windows.Forms.TableLayoutPanel
        Me.flytAction = New System.Windows.Forms.FlowLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtAction = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbTypeComposition = New System.Windows.Forms.ComboBox
        Me.grbxRelationFather = New System.Windows.Forms.GroupBox
        Me.tabGroupFather = New System.Windows.Forms.TableLayoutPanel
        Me.lblAccessor = New System.Windows.Forms.Label
        Me.lblFatherType = New System.Windows.Forms.Label
        Me.lblFatherCardinal = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtFatherName = New System.Windows.Forms.TextBox
        Me.cmbFatherClass = New System.Windows.Forms.ComboBox
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbFatherCardinal = New System.Windows.Forms.ComboBox
        Me.lblFatherLevel = New System.Windows.Forms.Label
        Me.cmbFatherLevel = New System.Windows.Forms.ComboBox
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbFatherRange = New System.Windows.Forms.ComboBox
        Me.chkFatherMember = New System.Windows.Forms.CheckBox
        Me.btnFatherType = New System.Windows.Forms.Button
        Me.FlowLayoutPanel5 = New System.Windows.Forms.FlowLayoutPanel
        Me.chkGetFather = New System.Windows.Forms.CheckBox
        Me.chkSetFather = New System.Windows.Forms.CheckBox
        Me.grbxRelationChild = New System.Windows.Forms.GroupBox
        Me.tabGroupChild = New System.Windows.Forms.TableLayoutPanel
        Me.Label14 = New System.Windows.Forms.Label
        Me.lblChildType = New System.Windows.Forms.Label
        Me.cmbChildClass = New System.Windows.Forms.ComboBox
        Me.txtChildName = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.FlowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbChildRange = New System.Windows.Forms.ComboBox
        Me.chkChildMember = New System.Windows.Forms.CheckBox
        Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel
        Me.cmbChildCardinal = New System.Windows.Forms.ComboBox
        Me.lblChildLevel = New System.Windows.Forms.Label
        Me.cmbChildLevel = New System.Windows.Forms.ComboBox
        Me.btnChildType = New System.Windows.Forms.Button
        Me.FlowLayoutPanel6 = New System.Windows.Forms.FlowLayoutPanel
        Me.chkGetChild = New System.Windows.Forms.CheckBox
        Me.chkSetChild = New System.Windows.Forms.CheckBox
        Me.tabMain = New System.Windows.Forms.TableLayoutPanel
        Me.tabButtons.SuspendLayout()
        Me.tabRelationship.SuspendLayout()
        Me.flytAction.SuspendLayout()
        Me.grbxRelationFather.SuspendLayout()
        Me.tabGroupFather.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.FlowLayoutPanel5.SuspendLayout()
        Me.grbxRelationChild.SuspendLayout()
        Me.tabGroupChild.SuspendLayout()
        Me.FlowLayoutPanel3.SuspendLayout()
        Me.FlowLayoutPanel4.SuspendLayout()
        Me.FlowLayoutPanel6.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabButtons
        '
        Me.tabButtons.ColumnCount = 3
        Me.tabButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.0!))
        Me.tabButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.0!))
        Me.tabButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.tabButtons.Controls.Add(Me.Cancel_Button, 2, 0)
        Me.tabButtons.Controls.Add(Me.OK_Button, 1, 0)
        Me.tabButtons.Controls.Add(Me.btnDelete, 0, 0)
        Me.tabButtons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabButtons.Location = New System.Drawing.Point(3, 252)
        Me.tabButtons.Name = "tabButtons"
        Me.tabButtons.RowCount = 1
        Me.tabButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tabButtons.Size = New System.Drawing.Size(668, 34)
        Me.tabButtons.TabIndex = 0
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(589, 5)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(506, 5)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(66, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
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
        'tabRelationship
        '
        Me.tabRelationship.ColumnCount = 2
        Me.tabRelationship.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tabRelationship.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tabRelationship.Controls.Add(Me.flytAction, 0, 0)
        Me.tabRelationship.Controls.Add(Me.grbxRelationFather, 0, 1)
        Me.tabRelationship.Controls.Add(Me.grbxRelationChild, 1, 1)
        Me.tabRelationship.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabRelationship.Location = New System.Drawing.Point(3, 3)
        Me.tabRelationship.Name = "tabRelationship"
        Me.tabRelationship.RowCount = 2
        Me.tabRelationship.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.tabRelationship.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tabRelationship.Size = New System.Drawing.Size(668, 243)
        Me.tabRelationship.TabIndex = 0
        '
        'flytAction
        '
        Me.tabRelationship.SetColumnSpan(Me.flytAction, 2)
        Me.flytAction.Controls.Add(Me.Label1)
        Me.flytAction.Controls.Add(Me.txtAction)
        Me.flytAction.Controls.Add(Me.Label2)
        Me.flytAction.Controls.Add(Me.cmbTypeComposition)
        Me.flytAction.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flytAction.Location = New System.Drawing.Point(3, 3)
        Me.flytAction.Name = "flytAction"
        Me.flytAction.Size = New System.Drawing.Size(662, 27)
        Me.flytAction.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 27)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Action:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAction
        '
        Me.txtAction.Location = New System.Drawing.Point(49, 3)
        Me.txtAction.Name = "txtAction"
        Me.txtAction.Size = New System.Drawing.Size(252, 20)
        Me.txtAction.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(307, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 27)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Type:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbTypeComposition
        '
        Me.cmbTypeComposition.FormattingEnabled = True
        Me.cmbTypeComposition.Location = New System.Drawing.Point(347, 3)
        Me.cmbTypeComposition.Name = "cmbTypeComposition"
        Me.cmbTypeComposition.Size = New System.Drawing.Size(89, 21)
        Me.cmbTypeComposition.TabIndex = 1
        '
        'grbxRelationFather
        '
        Me.grbxRelationFather.Controls.Add(Me.tabGroupFather)
        Me.grbxRelationFather.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grbxRelationFather.Location = New System.Drawing.Point(3, 36)
        Me.grbxRelationFather.Name = "grbxRelationFather"
        Me.grbxRelationFather.Size = New System.Drawing.Size(328, 204)
        Me.grbxRelationFather.TabIndex = 1
        Me.grbxRelationFather.TabStop = False
        Me.grbxRelationFather.Text = "Father"
        '
        'tabGroupFather
        '
        Me.tabGroupFather.ColumnCount = 2
        Me.tabGroupFather.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.tabGroupFather.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tabGroupFather.Controls.Add(Me.lblAccessor, 0, 4)
        Me.tabGroupFather.Controls.Add(Me.lblFatherType, 0, 5)
        Me.tabGroupFather.Controls.Add(Me.lblFatherCardinal, 0, 3)
        Me.tabGroupFather.Controls.Add(Me.Label3, 0, 0)
        Me.tabGroupFather.Controls.Add(Me.Label4, 0, 1)
        Me.tabGroupFather.Controls.Add(Me.Label5, 0, 2)
        Me.tabGroupFather.Controls.Add(Me.txtFatherName, 1, 0)
        Me.tabGroupFather.Controls.Add(Me.cmbFatherClass, 1, 1)
        Me.tabGroupFather.Controls.Add(Me.FlowLayoutPanel1, 1, 3)
        Me.tabGroupFather.Controls.Add(Me.FlowLayoutPanel2, 1, 2)
        Me.tabGroupFather.Controls.Add(Me.btnFatherType, 1, 5)
        Me.tabGroupFather.Controls.Add(Me.FlowLayoutPanel5, 1, 4)
        Me.tabGroupFather.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabGroupFather.Location = New System.Drawing.Point(3, 16)
        Me.tabGroupFather.Name = "tabGroupFather"
        Me.tabGroupFather.RowCount = 7
        Me.tabGroupFather.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.tabGroupFather.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.tabGroupFather.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.tabGroupFather.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.tabGroupFather.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.tabGroupFather.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tabGroupFather.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tabGroupFather.Size = New System.Drawing.Size(322, 185)
        Me.tabGroupFather.TabIndex = 0
        '
        'lblAccessor
        '
        Me.lblAccessor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAccessor.AutoSize = True
        Me.lblAccessor.Location = New System.Drawing.Point(3, 116)
        Me.lblAccessor.Name = "lblAccessor"
        Me.lblAccessor.Size = New System.Drawing.Size(54, 28)
        Me.lblAccessor.TabIndex = 16
        Me.lblAccessor.Text = "Accessor:"
        Me.lblAccessor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFatherType
        '
        Me.lblFatherType.AutoSize = True
        Me.lblFatherType.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFatherType.Location = New System.Drawing.Point(3, 144)
        Me.lblFatherType.Name = "lblFatherType"
        Me.lblFatherType.Size = New System.Drawing.Size(54, 38)
        Me.lblFatherType.TabIndex = 13
        Me.lblFatherType.Text = "Type:"
        Me.lblFatherType.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFatherCardinal
        '
        Me.lblFatherCardinal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFatherCardinal.AutoSize = True
        Me.lblFatherCardinal.Location = New System.Drawing.Point(9, 83)
        Me.lblFatherCardinal.Name = "lblFatherCardinal"
        Me.lblFatherCardinal.Size = New System.Drawing.Size(48, 33)
        Me.lblFatherCardinal.TabIndex = 7
        Me.lblFatherCardinal.Text = "Cardinal:"
        Me.lblFatherCardinal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(19, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 25)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Name:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(22, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 25)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Class:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(42, 33)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Range:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFatherName
        '
        Me.txtFatherName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFatherName.Location = New System.Drawing.Point(63, 3)
        Me.txtFatherName.Name = "txtFatherName"
        Me.txtFatherName.Size = New System.Drawing.Size(256, 20)
        Me.txtFatherName.TabIndex = 4
        '
        'cmbFatherClass
        '
        Me.cmbFatherClass.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbFatherClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFatherClass.DropDownWidth = 256
        Me.cmbFatherClass.FormattingEnabled = True
        Me.cmbFatherClass.Location = New System.Drawing.Point(63, 28)
        Me.cmbFatherClass.Name = "cmbFatherClass"
        Me.cmbFatherClass.Size = New System.Drawing.Size(256, 21)
        Me.cmbFatherClass.TabIndex = 5
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.cmbFatherCardinal)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblFatherLevel)
        Me.FlowLayoutPanel1.Controls.Add(Me.cmbFatherLevel)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(63, 86)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(256, 27)
        Me.FlowLayoutPanel1.TabIndex = 11
        '
        'cmbFatherCardinal
        '
        Me.cmbFatherCardinal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFatherCardinal.FormattingEnabled = True
        Me.cmbFatherCardinal.Location = New System.Drawing.Point(3, 3)
        Me.cmbFatherCardinal.Name = "cmbFatherCardinal"
        Me.cmbFatherCardinal.Size = New System.Drawing.Size(82, 21)
        Me.cmbFatherCardinal.TabIndex = 8
        '
        'lblFatherLevel
        '
        Me.lblFatherLevel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFatherLevel.AutoSize = True
        Me.lblFatherLevel.Location = New System.Drawing.Point(91, 0)
        Me.lblFatherLevel.Name = "lblFatherLevel"
        Me.lblFatherLevel.Size = New System.Drawing.Size(36, 27)
        Me.lblFatherLevel.TabIndex = 3
        Me.lblFatherLevel.Text = "Level:"
        Me.lblFatherLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbFatherLevel
        '
        Me.cmbFatherLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFatherLevel.FormattingEnabled = True
        Me.cmbFatherLevel.Location = New System.Drawing.Point(133, 3)
        Me.cmbFatherLevel.Name = "cmbFatherLevel"
        Me.cmbFatherLevel.Size = New System.Drawing.Size(82, 21)
        Me.cmbFatherLevel.TabIndex = 10
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Controls.Add(Me.cmbFatherRange)
        Me.FlowLayoutPanel2.Controls.Add(Me.chkFatherMember)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(63, 53)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(256, 27)
        Me.FlowLayoutPanel2.TabIndex = 12
        '
        'cmbFatherRange
        '
        Me.cmbFatherRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFatherRange.FormattingEnabled = True
        Me.cmbFatherRange.Location = New System.Drawing.Point(3, 3)
        Me.cmbFatherRange.Name = "cmbFatherRange"
        Me.cmbFatherRange.Size = New System.Drawing.Size(121, 21)
        Me.cmbFatherRange.TabIndex = 6
        '
        'chkFatherMember
        '
        Me.chkFatherMember.AutoSize = True
        Me.chkFatherMember.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkFatherMember.Location = New System.Drawing.Point(130, 3)
        Me.chkFatherMember.Name = "chkFatherMember"
        Me.chkFatherMember.Size = New System.Drawing.Size(91, 21)
        Me.chkFatherMember.TabIndex = 9
        Me.chkFatherMember.Text = "Class member"
        Me.chkFatherMember.UseVisualStyleBackColor = True
        '
        'btnFatherType
        '
        Me.btnFatherType.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnFatherType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFatherType.Location = New System.Drawing.Point(63, 147)
        Me.btnFatherType.Name = "btnFatherType"
        Me.btnFatherType.Size = New System.Drawing.Size(256, 32)
        Me.btnFatherType.TabIndex = 14
        Me.btnFatherType.Text = "<Type>"
        Me.btnFatherType.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel5
        '
        Me.FlowLayoutPanel5.Controls.Add(Me.chkGetFather)
        Me.FlowLayoutPanel5.Controls.Add(Me.chkSetFather)
        Me.FlowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel5.Location = New System.Drawing.Point(63, 119)
        Me.FlowLayoutPanel5.Name = "FlowLayoutPanel5"
        Me.FlowLayoutPanel5.Size = New System.Drawing.Size(256, 22)
        Me.FlowLayoutPanel5.TabIndex = 15
        '
        'chkGetFather
        '
        Me.chkGetFather.AutoSize = True
        Me.chkGetFather.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkGetFather.Location = New System.Drawing.Point(3, 3)
        Me.chkGetFather.Name = "chkGetFather"
        Me.chkGetFather.Size = New System.Drawing.Size(43, 17)
        Me.chkGetFather.TabIndex = 0
        Me.chkGetFather.Text = "Get"
        Me.chkGetFather.UseVisualStyleBackColor = True
        '
        'chkSetFather
        '
        Me.chkSetFather.AutoSize = True
        Me.chkSetFather.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkSetFather.Location = New System.Drawing.Point(52, 3)
        Me.chkSetFather.Name = "chkSetFather"
        Me.chkSetFather.Size = New System.Drawing.Size(42, 17)
        Me.chkSetFather.TabIndex = 1
        Me.chkSetFather.Text = "Set"
        Me.chkSetFather.UseVisualStyleBackColor = True
        '
        'grbxRelationChild
        '
        Me.grbxRelationChild.Controls.Add(Me.tabGroupChild)
        Me.grbxRelationChild.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grbxRelationChild.Location = New System.Drawing.Point(337, 36)
        Me.grbxRelationChild.Name = "grbxRelationChild"
        Me.grbxRelationChild.Size = New System.Drawing.Size(328, 204)
        Me.grbxRelationChild.TabIndex = 2
        Me.grbxRelationChild.TabStop = False
        Me.grbxRelationChild.Text = "Child"
        '
        'tabGroupChild
        '
        Me.tabGroupChild.ColumnCount = 2
        Me.tabGroupChild.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.tabGroupChild.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tabGroupChild.Controls.Add(Me.Label14, 0, 4)
        Me.tabGroupChild.Controls.Add(Me.lblChildType, 0, 5)
        Me.tabGroupChild.Controls.Add(Me.cmbChildClass, 1, 1)
        Me.tabGroupChild.Controls.Add(Me.txtChildName, 1, 0)
        Me.tabGroupChild.Controls.Add(Me.Label11, 0, 3)
        Me.tabGroupChild.Controls.Add(Me.Label8, 0, 0)
        Me.tabGroupChild.Controls.Add(Me.Label9, 0, 1)
        Me.tabGroupChild.Controls.Add(Me.Label10, 0, 2)
        Me.tabGroupChild.Controls.Add(Me.FlowLayoutPanel3, 1, 2)
        Me.tabGroupChild.Controls.Add(Me.FlowLayoutPanel4, 1, 3)
        Me.tabGroupChild.Controls.Add(Me.btnChildType, 1, 5)
        Me.tabGroupChild.Controls.Add(Me.FlowLayoutPanel6, 1, 4)
        Me.tabGroupChild.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabGroupChild.Location = New System.Drawing.Point(3, 16)
        Me.tabGroupChild.Name = "tabGroupChild"
        Me.tabGroupChild.RowCount = 7
        Me.tabGroupChild.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.tabGroupChild.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.tabGroupChild.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.tabGroupChild.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.tabGroupChild.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.tabGroupChild.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tabGroupChild.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tabGroupChild.Size = New System.Drawing.Size(322, 185)
        Me.tabGroupChild.TabIndex = 0
        '
        'Label14
        '
        Me.Label14.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(3, 116)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(54, 28)
        Me.Label14.TabIndex = 24
        Me.Label14.Text = "Accessor:"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblChildType
        '
        Me.lblChildType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblChildType.AutoSize = True
        Me.lblChildType.Location = New System.Drawing.Point(23, 144)
        Me.lblChildType.Name = "lblChildType"
        Me.lblChildType.Size = New System.Drawing.Size(34, 38)
        Me.lblChildType.TabIndex = 21
        Me.lblChildType.Text = "Type:"
        Me.lblChildType.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbChildClass
        '
        Me.cmbChildClass.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbChildClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbChildClass.DropDownWidth = 256
        Me.cmbChildClass.FormattingEnabled = True
        Me.cmbChildClass.Location = New System.Drawing.Point(63, 28)
        Me.cmbChildClass.Name = "cmbChildClass"
        Me.cmbChildClass.Size = New System.Drawing.Size(256, 21)
        Me.cmbChildClass.TabIndex = 14
        '
        'txtChildName
        '
        Me.txtChildName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtChildName.Location = New System.Drawing.Point(63, 3)
        Me.txtChildName.Name = "txtChildName"
        Me.txtChildName.Size = New System.Drawing.Size(256, 20)
        Me.txtChildName.TabIndex = 13
        '
        'Label11
        '
        Me.Label11.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(9, 83)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(48, 33)
        Me.Label11.TabIndex = 11
        Me.Label11.Text = "Cardinal:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(19, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(38, 25)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Name:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(22, 25)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 25)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Class:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(15, 50)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(42, 33)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "Range:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FlowLayoutPanel3
        '
        Me.FlowLayoutPanel3.Controls.Add(Me.cmbChildRange)
        Me.FlowLayoutPanel3.Controls.Add(Me.chkChildMember)
        Me.FlowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel3.Location = New System.Drawing.Point(63, 53)
        Me.FlowLayoutPanel3.Name = "FlowLayoutPanel3"
        Me.FlowLayoutPanel3.Size = New System.Drawing.Size(256, 27)
        Me.FlowLayoutPanel3.TabIndex = 19
        '
        'cmbChildRange
        '
        Me.cmbChildRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbChildRange.FormattingEnabled = True
        Me.cmbChildRange.Location = New System.Drawing.Point(3, 3)
        Me.cmbChildRange.Name = "cmbChildRange"
        Me.cmbChildRange.Size = New System.Drawing.Size(121, 21)
        Me.cmbChildRange.TabIndex = 15
        '
        'chkChildMember
        '
        Me.chkChildMember.AutoSize = True
        Me.chkChildMember.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkChildMember.Location = New System.Drawing.Point(130, 3)
        Me.chkChildMember.Name = "chkChildMember"
        Me.chkChildMember.Size = New System.Drawing.Size(91, 21)
        Me.chkChildMember.TabIndex = 18
        Me.chkChildMember.Text = "Class member"
        Me.chkChildMember.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel4
        '
        Me.FlowLayoutPanel4.Controls.Add(Me.cmbChildCardinal)
        Me.FlowLayoutPanel4.Controls.Add(Me.lblChildLevel)
        Me.FlowLayoutPanel4.Controls.Add(Me.cmbChildLevel)
        Me.FlowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel4.Location = New System.Drawing.Point(63, 86)
        Me.FlowLayoutPanel4.Name = "FlowLayoutPanel4"
        Me.FlowLayoutPanel4.Size = New System.Drawing.Size(256, 27)
        Me.FlowLayoutPanel4.TabIndex = 20
        '
        'cmbChildCardinal
        '
        Me.cmbChildCardinal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbChildCardinal.FormattingEnabled = True
        Me.cmbChildCardinal.Location = New System.Drawing.Point(3, 3)
        Me.cmbChildCardinal.Name = "cmbChildCardinal"
        Me.cmbChildCardinal.Size = New System.Drawing.Size(82, 21)
        Me.cmbChildCardinal.TabIndex = 16
        '
        'lblChildLevel
        '
        Me.lblChildLevel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblChildLevel.AutoSize = True
        Me.lblChildLevel.Location = New System.Drawing.Point(91, 0)
        Me.lblChildLevel.Name = "lblChildLevel"
        Me.lblChildLevel.Size = New System.Drawing.Size(36, 27)
        Me.lblChildLevel.TabIndex = 12
        Me.lblChildLevel.Text = "Level:"
        Me.lblChildLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbChildLevel
        '
        Me.cmbChildLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbChildLevel.FormattingEnabled = True
        Me.cmbChildLevel.Location = New System.Drawing.Point(133, 3)
        Me.cmbChildLevel.Name = "cmbChildLevel"
        Me.cmbChildLevel.Size = New System.Drawing.Size(82, 21)
        Me.cmbChildLevel.TabIndex = 17
        '
        'btnChildType
        '
        Me.btnChildType.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnChildType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnChildType.Location = New System.Drawing.Point(63, 147)
        Me.btnChildType.Name = "btnChildType"
        Me.btnChildType.Size = New System.Drawing.Size(256, 32)
        Me.btnChildType.TabIndex = 22
        Me.btnChildType.Text = "<Type>"
        Me.btnChildType.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel6
        '
        Me.FlowLayoutPanel6.Controls.Add(Me.chkGetChild)
        Me.FlowLayoutPanel6.Controls.Add(Me.chkSetChild)
        Me.FlowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel6.Location = New System.Drawing.Point(63, 119)
        Me.FlowLayoutPanel6.Name = "FlowLayoutPanel6"
        Me.FlowLayoutPanel6.Size = New System.Drawing.Size(256, 22)
        Me.FlowLayoutPanel6.TabIndex = 23
        '
        'chkGetChild
        '
        Me.chkGetChild.AutoSize = True
        Me.chkGetChild.Location = New System.Drawing.Point(3, 3)
        Me.chkGetChild.Name = "chkGetChild"
        Me.chkGetChild.Size = New System.Drawing.Size(43, 17)
        Me.chkGetChild.TabIndex = 0
        Me.chkGetChild.Text = "Get"
        Me.chkGetChild.UseVisualStyleBackColor = True
        '
        'chkSetChild
        '
        Me.chkSetChild.AutoSize = True
        Me.chkSetChild.Location = New System.Drawing.Point(52, 3)
        Me.chkSetChild.Name = "chkSetChild"
        Me.chkSetChild.Size = New System.Drawing.Size(42, 17)
        Me.chkSetChild.TabIndex = 1
        Me.chkSetChild.Text = "Set"
        Me.chkSetChild.UseVisualStyleBackColor = True
        '
        'tabMain
        '
        Me.tabMain.ColumnCount = 1
        Me.tabMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tabMain.Controls.Add(Me.tabRelationship, 0, 0)
        Me.tabMain.Controls.Add(Me.tabButtons, 0, 1)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.RowCount = 2
        Me.tabMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tabMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.tabMain.Size = New System.Drawing.Size(674, 289)
        Me.tabMain.TabIndex = 2
        '
        'dlgRelation
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(674, 289)
        Me.Controls.Add(Me.tabMain)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgRelation"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgRelation"
        Me.tabButtons.ResumeLayout(False)
        Me.tabRelationship.ResumeLayout(False)
        Me.flytAction.ResumeLayout(False)
        Me.flytAction.PerformLayout()
        Me.grbxRelationFather.ResumeLayout(False)
        Me.tabGroupFather.ResumeLayout(False)
        Me.tabGroupFather.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.FlowLayoutPanel2.PerformLayout()
        Me.FlowLayoutPanel5.ResumeLayout(False)
        Me.FlowLayoutPanel5.PerformLayout()
        Me.grbxRelationChild.ResumeLayout(False)
        Me.tabGroupChild.ResumeLayout(False)
        Me.tabGroupChild.PerformLayout()
        Me.FlowLayoutPanel3.ResumeLayout(False)
        Me.FlowLayoutPanel3.PerformLayout()
        Me.FlowLayoutPanel4.ResumeLayout(False)
        Me.FlowLayoutPanel4.PerformLayout()
        Me.FlowLayoutPanel6.ResumeLayout(False)
        Me.FlowLayoutPanel6.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabButtons As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents tabRelationship As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flytAction As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAction As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbTypeComposition As System.Windows.Forms.ComboBox
    Friend WithEvents grbxRelationFather As System.Windows.Forms.GroupBox
    Friend WithEvents grbxRelationChild As System.Windows.Forms.GroupBox
    Friend WithEvents tabMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tabGroupFather As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblFatherCardinal As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtFatherName As System.Windows.Forms.TextBox
    Friend WithEvents cmbFatherClass As System.Windows.Forms.ComboBox
    Friend WithEvents cmbFatherRange As System.Windows.Forms.ComboBox
    Friend WithEvents lblFatherLevel As System.Windows.Forms.Label
    Friend WithEvents cmbFatherCardinal As System.Windows.Forms.ComboBox
    Friend WithEvents chkFatherMember As System.Windows.Forms.CheckBox
    Friend WithEvents cmbFatherLevel As System.Windows.Forms.ComboBox
    Friend WithEvents tabGroupChild As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cmbChildLevel As System.Windows.Forms.ComboBox
    Friend WithEvents cmbChildCardinal As System.Windows.Forms.ComboBox
    Friend WithEvents cmbChildRange As System.Windows.Forms.ComboBox
    Friend WithEvents cmbChildClass As System.Windows.Forms.ComboBox
    Friend WithEvents txtChildName As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblChildLevel As System.Windows.Forms.Label
    Friend WithEvents chkChildMember As System.Windows.Forms.CheckBox
    Friend WithEvents lblFatherType As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel3 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents lblChildType As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel4 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnFatherType As System.Windows.Forms.Button
    Friend WithEvents btnChildType As System.Windows.Forms.Button
    Friend WithEvents lblAccessor As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel5 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel6 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents chkGetFather As System.Windows.Forms.CheckBox
    Friend WithEvents chkSetFather As System.Windows.Forms.CheckBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents chkGetChild As System.Windows.Forms.CheckBox
    Friend WithEvents chkSetChild As System.Windows.Forms.CheckBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button

End Class
