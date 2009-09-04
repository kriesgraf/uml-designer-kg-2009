<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgInterface
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
        Me.lblName = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtPackage = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.btnDelete = New System.Windows.Forms.Button
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.gridMembers = New ClassXmlProject.XmlDataGridView
        Me.mnuMembers = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddProperty = New System.Windows.Forms.ToolStripMenuItem
        Me.AddMethod = New System.Windows.Forms.ToolStripMenuItem
        Me.AddEnumeration = New System.Windows.Forms.ToolStripMenuItem
        Me.EditMember = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.CopyMember = New System.Windows.Forms.ToolStripMenuItem
        Me.PasteMember = New System.Windows.Forms.ToolStripMenuItem
        Me.DuplicateMember = New System.Windows.Forms.ToolStripMenuItem
        Me.MemberProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuMemberDependencies = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.DeleteMember = New System.Windows.Forms.ToolStripMenuItem
        Me.chkRoot = New System.Windows.Forms.CheckBox
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.lblClass = New System.Windows.Forms.Label
        Me.txtClass = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.gridMembers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuMembers.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblName.Location = New System.Drawing.Point(10, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 26)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name:"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtName
        '
        Me.txtName.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtName.Location = New System.Drawing.Point(54, 3)
        Me.txtName.Margin = New System.Windows.Forms.Padding(3, 3, 20, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(191, 20)
        Me.txtName.TabIndex = 4
        '
        'txtPackage
        '
        Me.txtPackage.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtPackage.Location = New System.Drawing.Point(328, 3)
        Me.txtPackage.Margin = New System.Windows.Forms.Padding(3, 3, 20, 3)
        Me.txtPackage.Name = "txtPackage"
        Me.txtPackage.Size = New System.Drawing.Size(192, 20)
        Me.txtPackage.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label1.Location = New System.Drawing.Point(269, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 26)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Package:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.CausesValidation = False
        Me.Cancel_Button.Location = New System.Drawing.Point(458, 5)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(367, 5)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.CausesValidation = False
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel2.SetColumnSpan(Me.TableLayoutPanel1, 4)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnDelete, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 2, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 201)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(534, 34)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'btnDelete
        '
        Me.btnDelete.CausesValidation = False
        Me.btnDelete.Location = New System.Drawing.Point(3, 3)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(67, 23)
        Me.btnDelete.TabIndex = 3
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 4
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 51.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.txtPackage, 3, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label1, 2, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblName, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.txtName, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.gridMembers, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.chkRoot, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblClass, 2, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.txtClass, 3, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 4
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(540, 238)
        Me.TableLayoutPanel2.TabIndex = 2
        '
        'gridMembers
        '
        Me.gridMembers.ColumnDragStart = 0
        Me.gridMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel2.SetColumnSpan(Me.gridMembers, 3)
        Me.gridMembers.ContextMenuStrip = Me.mnuMembers
        Me.gridMembers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridMembers.Location = New System.Drawing.Point(54, 57)
        Me.gridMembers.Name = "gridMembers"
        Me.gridMembers.Size = New System.Drawing.Size(483, 138)
        Me.gridMembers.TabIndex = 10
        '
        'mnuMembers
        '
        Me.mnuMembers.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddProperty, Me.AddMethod, Me.AddEnumeration, Me.EditMember, Me.ToolStripSeparator7, Me.CopyMember, Me.PasteMember, Me.DuplicateMember, Me.MemberProperties, Me.ToolStripSeparator5, Me.mnuMemberDependencies, Me.ToolStripSeparator2, Me.DeleteMember})
        Me.mnuMembers.Name = "mnuMembers"
        Me.mnuMembers.Size = New System.Drawing.Size(227, 242)
        '
        'AddProperty
        '
        Me.AddProperty.Image = Global.ClassXmlProject.My.Resources.Resources.Properties
        Me.AddProperty.Name = "AddProperty"
        Me.AddProperty.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.AddProperty.Size = New System.Drawing.Size(226, 22)
        Me.AddProperty.Tag = "property"
        Me.AddProperty.Text = "Property"
        '
        'AddMethod
        '
        Me.AddMethod.Image = Global.ClassXmlProject.My.Resources.Resources.Move
        Me.AddMethod.Name = "AddMethod"
        Me.AddMethod.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.AddMethod.Size = New System.Drawing.Size(226, 22)
        Me.AddMethod.Tag = "method"
        Me.AddMethod.Text = "Method"
        '
        'AddEnumeration
        '
        Me.AddEnumeration.Image = Global.ClassXmlProject.My.Resources.Resources.Apps
        Me.AddEnumeration.Name = "AddEnumeration"
        Me.AddEnumeration.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.AddEnumeration.Size = New System.Drawing.Size(226, 22)
        Me.AddEnumeration.Tag = "enumvalue"
        Me.AddEnumeration.Text = "Enumeration value"
        '
        'EditMember
        '
        Me.EditMember.Image = Global.ClassXmlProject.My.Resources.Resources.Rename___Edit
        Me.EditMember.Name = "EditMember"
        Me.EditMember.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.EditMember.Size = New System.Drawing.Size(226, 22)
        Me.EditMember.Text = "Edit..."
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(223, 6)
        '
        'CopyMember
        '
        Me.CopyMember.Image = Global.ClassXmlProject.My.Resources.Resources.Copy
        Me.CopyMember.Name = "CopyMember"
        Me.CopyMember.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CopyMember.Size = New System.Drawing.Size(226, 22)
        Me.CopyMember.Text = "Copy"
        '
        'PasteMember
        '
        Me.PasteMember.Image = Global.ClassXmlProject.My.Resources.Resources.Paste
        Me.PasteMember.Name = "PasteMember"
        Me.PasteMember.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.PasteMember.Size = New System.Drawing.Size(226, 22)
        Me.PasteMember.Text = "Paste"
        '
        'DuplicateMember
        '
        Me.DuplicateMember.Name = "DuplicateMember"
        Me.DuplicateMember.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.DuplicateMember.Size = New System.Drawing.Size(226, 22)
        Me.DuplicateMember.Text = "Duplicate"
        '
        'MemberProperties
        '
        Me.MemberProperties.Name = "MemberProperties"
        Me.MemberProperties.Size = New System.Drawing.Size(226, 22)
        Me.MemberProperties.Text = "Parameters..."
        Me.MemberProperties.Visible = False
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(223, 6)
        '
        'mnuMemberDependencies
        '
        Me.mnuMemberDependencies.Image = Global.ClassXmlProject.My.Resources.Resources.Search
        Me.mnuMemberDependencies.Name = "mnuMemberDependencies"
        Me.mnuMemberDependencies.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.mnuMemberDependencies.Size = New System.Drawing.Size(226, 22)
        Me.mnuMemberDependencies.Text = "Search dependencies..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(223, 6)
        '
        'DeleteMember
        '
        Me.DeleteMember.Image = Global.ClassXmlProject.My.Resources.Resources.Stop_2
        Me.DeleteMember.Name = "DeleteMember"
        Me.DeleteMember.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.DeleteMember.Size = New System.Drawing.Size(226, 22)
        Me.DeleteMember.Text = "Delete"
        '
        'chkRoot
        '
        Me.chkRoot.AutoSize = True
        Me.chkRoot.Dock = System.Windows.Forms.DockStyle.Left
        Me.chkRoot.Location = New System.Drawing.Point(54, 29)
        Me.chkRoot.Name = "chkRoot"
        Me.chkRoot.Size = New System.Drawing.Size(76, 22)
        Me.chkRoot.TabIndex = 11
        Me.chkRoot.Text = "Root class"
        Me.chkRoot.UseVisualStyleBackColor = True
        '
        'errorProvider
        '
        Me.errorProvider.ContainerControl = Me
        '
        'lblClass
        '
        Me.lblClass.AutoSize = True
        Me.lblClass.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblClass.Location = New System.Drawing.Point(287, 26)
        Me.lblClass.Name = "lblClass"
        Me.lblClass.Size = New System.Drawing.Size(35, 28)
        Me.lblClass.TabIndex = 12
        Me.lblClass.Text = "Class:"
        Me.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtClass
        '
        Me.txtClass.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtClass.Location = New System.Drawing.Point(328, 29)
        Me.txtClass.Margin = New System.Windows.Forms.Padding(3, 3, 20, 3)
        Me.txtClass.Name = "txtClass"
        Me.txtClass.Size = New System.Drawing.Size(192, 20)
        Me.txtClass.TabIndex = 13
        '
        'dlgInterface
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(540, 238)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Name = "dlgInterface"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgInterface"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        CType(Me.gridMembers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuMembers.ResumeLayout(False)
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtPackage As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents mnuMembers As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddProperty As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddMethod As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditMember As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CopyMember As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteMember As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DuplicateMember As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MemberProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuMemberDependencies As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteMember As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridMembers As ClassXmlProject.XmlDataGridView
    Friend WithEvents chkRoot As System.Windows.Forms.CheckBox
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents AddEnumeration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblClass As System.Windows.Forms.Label
    Friend WithEvents txtClass As System.Windows.Forms.TextBox

End Class
