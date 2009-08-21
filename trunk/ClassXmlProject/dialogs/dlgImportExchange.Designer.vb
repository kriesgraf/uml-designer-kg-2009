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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.lsbSource = New System.Windows.Forms.ListBox
        Me.lsbDestination = New System.Windows.Forms.ListBox
        Me.lsbImports = New System.Windows.Forms.ListBox
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel
        Me.btnDestination = New System.Windows.Forms.Button
        Me.btnSource = New System.Windows.Forms.Button
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.lsbSource, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lsbDestination, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lsbImports, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel2, 1, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(568, 257)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'lsbSource
        '
        Me.lsbSource.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbSource.FormattingEnabled = True
        Me.lsbSource.Location = New System.Drawing.Point(3, 80)
        Me.lsbSource.Name = "lsbSource"
        Me.lsbSource.Size = New System.Drawing.Size(256, 173)
        Me.lsbSource.TabIndex = 0
        '
        'lsbDestination
        '
        Me.lsbDestination.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbDestination.FormattingEnabled = True
        Me.lsbDestination.Location = New System.Drawing.Point(308, 80)
        Me.lsbDestination.Name = "lsbDestination"
        Me.lsbDestination.Size = New System.Drawing.Size(257, 173)
        Me.lsbDestination.TabIndex = 1
        '
        'lsbImports
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.lsbImports, 2)
        Me.lsbImports.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbImports.FormattingEnabled = True
        Me.lsbImports.Location = New System.Drawing.Point(265, 3)
        Me.lsbImports.Name = "lsbImports"
        Me.lsbImports.Size = New System.Drawing.Size(300, 69)
        Me.lsbImports.TabIndex = 2
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.Label1)
        Me.FlowLayoutPanel1.Controls.Add(Me.Label2)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(256, 71)
        Me.FlowLayoutPanel1.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(193, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select in right list the Import destination."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(197, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Select in bottom list an element to move."
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Controls.Add(Me.btnDestination)
        Me.FlowLayoutPanel2.Controls.Add(Me.btnSource)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(265, 80)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(37, 174)
        Me.FlowLayoutPanel2.TabIndex = 4
        '
        'btnDestination
        '
        Me.btnDestination.Location = New System.Drawing.Point(3, 3)
        Me.btnDestination.Name = "btnDestination"
        Me.btnDestination.Size = New System.Drawing.Size(33, 26)
        Me.btnDestination.TabIndex = 0
        Me.btnDestination.Text = "->"
        Me.btnDestination.UseVisualStyleBackColor = True
        '
        'btnSource
        '
        Me.btnSource.Location = New System.Drawing.Point(3, 35)
        Me.btnSource.Name = "btnSource"
        Me.btnSource.Size = New System.Drawing.Size(33, 26)
        Me.btnSource.TabIndex = 1
        Me.btnSource.Text = "<-"
        Me.btnSource.UseVisualStyleBackColor = True
        '
        'dlgImportExchange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(568, 257)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimizeBox = False
        Me.Name = "dlgImportExchange"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgImportExchange"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lsbSource As System.Windows.Forms.ListBox
    Friend WithEvents lsbDestination As System.Windows.Forms.ListBox
    Friend WithEvents lsbImports As System.Windows.Forms.ListBox
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnDestination As System.Windows.Forms.Button
    Friend WithEvents btnSource As System.Windows.Forms.Button
End Class
