<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class dlgParams
#Region "Code généré par le Concepteur Windows Form "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
        'Cet appel est requis par le Concepteur Windows Form.
        Me.m_bInitializeComponent = True
        InitializeComponent()
        Me.m_bInitializeComponent = False
    End Sub
	'Form remplace la méthode Dispose pour nettoyer la liste des composants.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Requise par le Concepteur Windows Form
	Private components As System.ComponentModel.IContainer
    Public WithEvents cmdFile As System.Windows.Forms.Button
	Public WithEvents cmdPath As System.Windows.Forms.Button
	Public WithEvents txtValeur As System.Windows.Forms.TextBox
	Public WithEvents cmbListeParams As System.Windows.Forms.ComboBox
    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cmdFile = New System.Windows.Forms.Button
        Me.cmdPath = New System.Windows.Forms.Button
        Me.txtValeur = New System.Windows.Forms.TextBox
        Me.cmbListeParams = New System.Windows.Forms.ComboBox
        Me.chkSeparator = New System.Windows.Forms.CheckBox
        Me.btnValider = New System.Windows.Forms.Button
        Me.btnXmlFolder = New System.Windows.Forms.Button
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdFile
        '
        Me.cmdFile.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFile.Location = New System.Drawing.Point(57, 3)
        Me.cmdFile.Name = "cmdFile"
        Me.cmdFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFile.Size = New System.Drawing.Size(49, 26)
        Me.cmdFile.TabIndex = 5
        Me.cmdFile.Text = "File"
        Me.cmdFile.UseVisualStyleBackColor = False
        '
        'cmdPath
        '
        Me.cmdPath.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPath.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPath.Location = New System.Drawing.Point(3, 3)
        Me.cmdPath.Name = "cmdPath"
        Me.cmdPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPath.Size = New System.Drawing.Size(48, 26)
        Me.cmdPath.TabIndex = 4
        Me.cmdPath.Text = "Path"
        Me.cmdPath.UseVisualStyleBackColor = False
        '
        'txtValeur
        '
        Me.txtValeur.AcceptsReturn = True
        Me.txtValeur.BackColor = System.Drawing.SystemColors.Window
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtValeur, 2)
        Me.txtValeur.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValeur.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtValeur.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtValeur.Location = New System.Drawing.Point(118, 65)
        Me.txtValeur.MaxLength = 0
        Me.txtValeur.Multiline = True
        Me.txtValeur.Name = "txtValeur"
        Me.txtValeur.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtValeur.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtValeur.Size = New System.Drawing.Size(368, 100)
        Me.txtValeur.TabIndex = 3
        '
        'cmbListeParams
        '
        Me.cmbListeParams.BackColor = System.Drawing.SystemColors.Window
        Me.TableLayoutPanel1.SetColumnSpan(Me.cmbListeParams, 2)
        Me.cmbListeParams.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbListeParams.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbListeParams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbListeParams.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbListeParams.Location = New System.Drawing.Point(3, 35)
        Me.cmbListeParams.Name = "cmbListeParams"
        Me.cmbListeParams.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbListeParams.Size = New System.Drawing.Size(368, 21)
        Me.cmbListeParams.TabIndex = 2
        '
        'chkSeparator
        '
        Me.chkSeparator.AutoSize = True
        Me.TableLayoutPanel2.SetColumnSpan(Me.chkSeparator, 2)
        Me.chkSeparator.Dock = System.Windows.Forms.DockStyle.Top
        Me.chkSeparator.Location = New System.Drawing.Point(3, 67)
        Me.chkSeparator.Name = "chkSeparator"
        Me.chkSeparator.Size = New System.Drawing.Size(103, 17)
        Me.chkSeparator.TabIndex = 6
        Me.chkSeparator.Text = "Add '\'"
        Me.chkSeparator.UseVisualStyleBackColor = True
        '
        'btnValider
        '
        Me.btnValider.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnValider.Location = New System.Drawing.Point(377, 35)
        Me.btnValider.Name = "btnValider"
        Me.btnValider.Size = New System.Drawing.Size(109, 24)
        Me.btnValider.TabIndex = 7
        Me.btnValider.Text = "Valider"
        Me.btnValider.UseVisualStyleBackColor = True
        '
        'btnXmlFolder
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.btnXmlFolder, 2)
        Me.btnXmlFolder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnXmlFolder.Location = New System.Drawing.Point(3, 35)
        Me.btnXmlFolder.Name = "btnXmlFolder"
        Me.btnXmlFolder.Size = New System.Drawing.Size(103, 26)
        Me.btnXmlFolder.TabIndex = 8
        Me.btnXmlFolder.Text = "XML folder"
        Me.btnXmlFolder.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.cmbListeParams, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnValider, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txtValeur, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(489, 168)
        Me.TableLayoutPanel1.TabIndex = 9
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.cmdPath, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.chkSeparator, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.btnXmlFolder, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.cmdFile, 1, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 65)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 3
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(109, 100)
        Me.TableLayoutPanel2.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label1, 2)
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(368, 32)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Appuyez sur 'Valider' pour chaque paramètre modifié, puis fermez la fenêtre pour " & _
            "relancer la transformation..."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dlgParams
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(489, 168)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Location = New System.Drawing.Point(184, 250)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgParams"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Modification des paramètres de la transformation XSL"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkSeparator As System.Windows.Forms.CheckBox
    Friend WithEvents btnValider As System.Windows.Forms.Button
    Friend WithEvents btnXmlFolder As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
#End Region 
End Class