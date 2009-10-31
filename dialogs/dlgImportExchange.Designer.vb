<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgImportExchange
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
        Me.tblpMain = New System.Windows.Forms.TableLayoutPanel
        Me.lsbDestination = New System.Windows.Forms.ListBox
        Me.lsbImports = New System.Windows.Forms.ListBox
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel
        Me.btnDestination = New System.Windows.Forms.Button
        Me.btnSource = New System.Windows.Forms.Button
        Me.lsbSource = New System.Windows.Forms.ListBox
        Me.tbplComment = New System.Windows.Forms.TableLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.tblpMain.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.tbplComment.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tblpMain
        '
        Me.tblpMain.ColumnCount = 3
        Me.tblpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.tblpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblpMain.Controls.Add(Me.lsbDestination, 2, 3)
        Me.tblpMain.Controls.Add(Me.lsbImports, 2, 1)
        Me.tblpMain.Controls.Add(Me.FlowLayoutPanel2, 1, 3)
        Me.tblpMain.Controls.Add(Me.lsbSource, 0, 3)
        Me.tblpMain.Controls.Add(Me.tbplComment, 0, 1)
        Me.tblpMain.Controls.Add(Me.FlowLayoutPanel1, 1, 1)
        Me.tblpMain.Controls.Add(Me.Label6, 2, 0)
        Me.tblpMain.Controls.Add(Me.Label7, 0, 2)
        Me.tblpMain.Controls.Add(Me.Label8, 2, 2)
        Me.tblpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblpMain.Location = New System.Drawing.Point(0, 0)
        Me.tblpMain.Name = "tblpMain"
        Me.tblpMain.RowCount = 4
        Me.tblpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tblpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.tblpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tblpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.0!))
        Me.tblpMain.Size = New System.Drawing.Size(598, 433)
        Me.tblpMain.TabIndex = 0
        '
        'lsbDestination
        '
        Me.lsbDestination.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbDestination.FormattingEnabled = True
        Me.lsbDestination.Location = New System.Drawing.Point(327, 160)
        Me.lsbDestination.Name = "lsbDestination"
        Me.lsbDestination.Size = New System.Drawing.Size(268, 264)
        Me.lsbDestination.TabIndex = 1
        '
        'lsbImports
        '
        Me.lsbImports.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbImports.FormattingEnabled = True
        Me.lsbImports.Location = New System.Drawing.Point(327, 23)
        Me.lsbImports.Name = "lsbImports"
        Me.lsbImports.Size = New System.Drawing.Size(268, 108)
        Me.lsbImports.TabIndex = 2
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Controls.Add(Me.btnDestination)
        Me.FlowLayoutPanel2.Controls.Add(Me.btnSource)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(277, 160)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(44, 270)
        Me.FlowLayoutPanel2.TabIndex = 4
        '
        'btnDestination
        '
        Me.btnDestination.Location = New System.Drawing.Point(3, 3)
        Me.btnDestination.Name = "btnDestination"
        Me.btnDestination.Size = New System.Drawing.Size(40, 26)
        Me.btnDestination.TabIndex = 0
        Me.btnDestination.Text = "--->"
        Me.btnDestination.UseVisualStyleBackColor = True
        '
        'btnSource
        '
        Me.btnSource.Location = New System.Drawing.Point(3, 35)
        Me.btnSource.Name = "btnSource"
        Me.btnSource.Size = New System.Drawing.Size(40, 26)
        Me.btnSource.TabIndex = 1
        Me.btnSource.Text = "<---"
        Me.btnSource.UseVisualStyleBackColor = True
        '
        'lsbSource
        '
        Me.lsbSource.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbSource.FormattingEnabled = True
        Me.lsbSource.Location = New System.Drawing.Point(3, 160)
        Me.lsbSource.Name = "lsbSource"
        Me.lsbSource.Size = New System.Drawing.Size(268, 264)
        Me.lsbSource.TabIndex = 0
        '
        'tbplComment
        '
        Me.tbplComment.ColumnCount = 1
        Me.tbplComment.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tbplComment.Controls.Add(Me.Label1, 0, 1)
        Me.tbplComment.Controls.Add(Me.Label2, 0, 0)
        Me.tbplComment.Controls.Add(Me.Label3, 0, 2)
        Me.tbplComment.Controls.Add(Me.Label4, 0, 3)
        Me.tbplComment.Controls.Add(Me.Label5, 0, 4)
        Me.tbplComment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbplComment.Location = New System.Drawing.Point(3, 23)
        Me.tbplComment.Name = "tbplComment"
        Me.tbplComment.RowCount = 6
        Me.tbplComment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tbplComment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tbplComment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tbplComment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tbplComment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tbplComment.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tbplComment.Size = New System.Drawing.Size(268, 111)
        Me.tbplComment.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(193, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select in right list the Import destination."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(197, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Select in bottom list an element to move."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(219, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To edit, double-click or Enter on one element"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(151, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "To move: Left and Right arrow"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(122, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "To add/remove: Ins/Del"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAdd)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnDelete)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(277, 23)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(44, 111)
        Me.FlowLayoutPanel1.TabIndex = 6
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(3, 3)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(40, 26)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "Add>"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(3, 35)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(40, 26)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "Del>"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label6.Location = New System.Drawing.Point(327, 7)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(268, 13)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Possible imports:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label7.Location = New System.Drawing.Point(3, 144)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(268, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "From:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label8.Location = New System.Drawing.Point(327, 144)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(268, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "To:"
        '
        'dlgImportExchange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(598, 433)
        Me.Controls.Add(Me.tblpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimizeBox = False
        Me.Name = "dlgImportExchange"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgImportExchange"
        Me.tblpMain.ResumeLayout(False)
        Me.tblpMain.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.tbplComment.ResumeLayout(False)
        Me.tbplComment.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tblpMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lsbSource As System.Windows.Forms.ListBox
    Friend WithEvents lsbDestination As System.Windows.Forms.ListBox
    Friend WithEvents lsbImports As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnDestination As System.Windows.Forms.Button
    Friend WithEvents btnSource As System.Windows.Forms.Button
    Friend WithEvents tbplComment As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
End Class
