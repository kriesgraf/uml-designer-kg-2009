<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgDependencies
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
        Me.lsbDependencies = New System.Windows.Forms.ListBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'lsbDependencies
        '
        Me.lsbDependencies.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsbDependencies.FormattingEnabled = True
        Me.lsbDependencies.Location = New System.Drawing.Point(0, 0)
        Me.lsbDependencies.Name = "lsbDependencies"
        Me.lsbDependencies.Size = New System.Drawing.Size(292, 264)
        Me.lsbDependencies.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.lsbDependencies, "Double click to open editor")
        '
        'dlgDependencies
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.lsbDependencies)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "dlgDependencies"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "dlgDependencies"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lsbDependencies As System.Windows.Forms.ListBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
