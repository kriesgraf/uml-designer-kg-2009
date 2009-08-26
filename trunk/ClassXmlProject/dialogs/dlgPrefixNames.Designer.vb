<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgPrefixNames
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
        Me.grdPrefixNameList = New System.Windows.Forms.DataGridView
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.grdPrefixNameList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ShowInTaskbar = False
        Me.SuspendLayout()
        '
        'grdSimpleTypeList
        '
        Me.grdPrefixNameList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdPrefixNameList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdPrefixNameList.Location = New System.Drawing.Point(0, 0)
        Me.grdPrefixNameList.Name = "grdSimpleTypeList"
        Me.grdPrefixNameList.Size = New System.Drawing.Size(403, 474)
        Me.grdPrefixNameList.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.grdPrefixNameList, "Click cell Prefix to update name")
        '
        'dlgPrefixNames
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(403, 474)
        Me.Controls.Add(Me.grdPrefixNameList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "dlgPrefixNames"
        Me.Text = "dlgPrefixNames"
        CType(Me.grdPrefixNameList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grdPrefixNameList As System.Windows.Forms.DataGridView
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
