<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class XMLTransformFilter
#Region "Code g�n�r� par le Concepteur Windows Form "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
        'Cet appel est requis par le Concepteur Windows Form.
        Me.m_bInitializeComponent = True
        InitializeComponent()
        Me.m_bInitializeComponent = False
    End Sub
	'Form remplace la m�thode Dispose pour nettoyer la liste des composants.
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
	Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents txtXMLFilter As System.Windows.Forms.TextBox
    Public WithEvents cmdRechargerXSL As System.Windows.Forms.Button
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public WithEvents cmdParams As System.Windows.Forms.Button
    Public WithEvents chkSynchronize As System.Windows.Forms.CheckBox
	Public WithEvents WebBrowser As System.Windows.Forms.WebBrowser
    Public WithEvents cmdSave As System.Windows.Forms.Button
	Public WithEvents cmdRechargerXML As System.Windows.Forms.Button
	Public WithEvents DriveXSL As Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
	Public WithEvents DirXSL As Microsoft.VisualBasic.Compatibility.VB6.DirListBox
	Public WithEvents FileXSL As Microsoft.VisualBasic.Compatibility.VB6.FileListBox
	Public WithEvents FrameXSL As System.Windows.Forms.GroupBox
	Public WithEvents FileXML As Microsoft.VisualBasic.Compatibility.VB6.FileListBox
	Public WithEvents DirXML As Microsoft.VisualBasic.Compatibility.VB6.DirListBox
	Public WithEvents DriveXML As Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
	Public WithEvents FrameXML As System.Windows.Forms.GroupBox
	Public WithEvents lblTransform As System.Windows.Forms.Label
    Public WithEvents LabelXSL As System.Windows.Forms.Label
	Public WithEvents LabelXML As System.Windows.Forms.Label
    'REMARQUE�: la proc�dure suivante est requise par le Concepteur Windows Form
    'Elle peut �tre modifi�e � l'aide du Concepteur Windows Form.
    'Ne la modifiez pas � l'aide de l'�diteur de code.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdRechargerXSL = New System.Windows.Forms.Button
        Me.cmdNew = New System.Windows.Forms.Button
        Me.cmdParams = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.cmdRechargerXML = New System.Windows.Forms.Button
        Me.txtXMLFilter = New System.Windows.Forms.TextBox
        Me.chkSynchronize = New System.Windows.Forms.CheckBox
        Me.WebBrowser = New System.Windows.Forms.WebBrowser
        Me.FrameXSL = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.DriveXSL = New Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
        Me.SplitFrameXSL = New System.Windows.Forms.SplitContainer
        Me.DirXSL = New Microsoft.VisualBasic.Compatibility.VB6.DirListBox
        Me.FileXSL = New Microsoft.VisualBasic.Compatibility.VB6.FileListBox
        Me.FrameXML = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.DriveXML = New Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.DirXML = New Microsoft.VisualBasic.Compatibility.VB6.DirListBox
        Me.FileXML = New Microsoft.VisualBasic.Compatibility.VB6.FileListBox
        Me.lblTransform = New System.Windows.Forms.Label
        Me.LabelXSL = New System.Windows.Forms.Label
        Me.LabelXML = New System.Windows.Forms.Label
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtMaskXSL = New System.Windows.Forms.TextBox
        Me.cmdValidate = New System.Windows.Forms.Button
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.TextEditBox = New System.Windows.Forms.RichTextBox
        Me.FrameXSL.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SplitFrameXSL.Panel1.SuspendLayout()
        Me.SplitFrameXSL.Panel2.SuspendLayout()
        Me.SplitFrameXSL.SuspendLayout()
        Me.FrameXML.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdRechargerXSL
        '
        Me.cmdRechargerXSL.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRechargerXSL.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRechargerXSL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdRechargerXSL.Enabled = False
        Me.cmdRechargerXSL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRechargerXSL.Location = New System.Drawing.Point(93, 36)
        Me.cmdRechargerXSL.Name = "cmdRechargerXSL"
        Me.cmdRechargerXSL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRechargerXSL.Size = New System.Drawing.Size(114, 27)
        Me.cmdRechargerXSL.TabIndex = 19
        Me.cmdRechargerXSL.Text = "XSL Transformation"
        Me.ToolTip1.SetToolTip(Me.cmdRechargerXSL, "Effectue la transformation XSL s�lectionn�e")
        Me.cmdRechargerXSL.UseVisualStyleBackColor = False
        '
        'cmdNew
        '
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNew.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNew.Location = New System.Drawing.Point(3, 3)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNew.Size = New System.Drawing.Size(84, 27)
        Me.cmdNew.TabIndex = 23
        Me.cmdNew.Text = "Nouveau"
        Me.ToolTip1.SetToolTip(Me.cmdNew, "S�lectionne le fichier pour sauvegarder le r�sultat de la transformation")
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdParams
        '
        Me.cmdParams.BackColor = System.Drawing.SystemColors.Control
        Me.cmdParams.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdParams.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdParams.Enabled = False
        Me.cmdParams.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdParams.Location = New System.Drawing.Point(3, 69)
        Me.cmdParams.Name = "cmdParams"
        Me.cmdParams.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdParams.Size = New System.Drawing.Size(84, 27)
        Me.cmdParams.TabIndex = 22
        Me.cmdParams.Text = "Param�tres..."
        Me.ToolTip1.SetToolTip(Me.cmdParams, "Modifie les variables globales d�finies dans le document XSL(T)")
        Me.cmdParams.UseVisualStyleBackColor = False
        '
        'cmdSave
        '
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSave.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdSave.Enabled = False
        Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSave.Location = New System.Drawing.Point(3, 36)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSave.Size = New System.Drawing.Size(84, 27)
        Me.cmdSave.TabIndex = 11
        Me.cmdSave.Text = "Enregistrer..."
        Me.ToolTip1.SetToolTip(Me.cmdSave, "S�lectionne le fichier pour sauvegarder le r�sultat de la transformation")
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdRechargerXML
        '
        Me.cmdRechargerXML.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRechargerXML.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRechargerXML.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdRechargerXML.Enabled = False
        Me.cmdRechargerXML.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRechargerXML.Location = New System.Drawing.Point(93, 3)
        Me.cmdRechargerXML.Name = "cmdRechargerXML"
        Me.cmdRechargerXML.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRechargerXML.Size = New System.Drawing.Size(114, 27)
        Me.cmdRechargerXML.TabIndex = 10
        Me.cmdRechargerXML.Text = "Document XML"
        Me.ToolTip1.SetToolTip(Me.cmdRechargerXML, "Effectue la transformation XSL s�lectionn�e")
        Me.cmdRechargerXML.UseVisualStyleBackColor = False
        '
        'txtXMLFilter
        '
        Me.txtXMLFilter.AcceptsReturn = True
        Me.txtXMLFilter.BackColor = System.Drawing.SystemColors.Window
        Me.txtXMLFilter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtXMLFilter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtXMLFilter.Location = New System.Drawing.Point(71, 5)
        Me.txtXMLFilter.Margin = New System.Windows.Forms.Padding(5)
        Me.txtXMLFilter.MaxLength = 0
        Me.txtXMLFilter.Name = "txtXMLFilter"
        Me.txtXMLFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtXMLFilter.Size = New System.Drawing.Size(60, 20)
        Me.txtXMLFilter.TabIndex = 25
        Me.txtXMLFilter.Text = "*.xml"
        '
        'chkSynchronize
        '
        Me.chkSynchronize.BackColor = System.Drawing.SystemColors.Control
        Me.chkSynchronize.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSynchronize.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkSynchronize.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSynchronize.Location = New System.Drawing.Point(3, 348)
        Me.chkSynchronize.Name = "chkSynchronize"
        Me.chkSynchronize.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSynchronize.Size = New System.Drawing.Size(240, 29)
        Me.chkSynchronize.TabIndex = 15
        Me.chkSynchronize.Text = "Synchroniser les 2 explorateurs de fichiers"
        Me.chkSynchronize.UseVisualStyleBackColor = False
        '
        'WebBrowser
        '
        Me.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser.Location = New System.Drawing.Point(3, 3)
        Me.WebBrowser.Name = "WebBrowser"
        Me.WebBrowser.Size = New System.Drawing.Size(658, 553)
        Me.WebBrowser.TabIndex = 14
        '
        'FrameXSL
        '
        Me.FrameXSL.BackColor = System.Drawing.SystemColors.Control
        Me.FrameXSL.Controls.Add(Me.TableLayoutPanel2)
        Me.FrameXSL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FrameXSL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameXSL.Location = New System.Drawing.Point(3, 383)
        Me.FrameXSL.Name = "FrameXSL"
        Me.FrameXSL.Padding = New System.Windows.Forms.Padding(5)
        Me.FrameXSL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameXSL.Size = New System.Drawing.Size(240, 339)
        Me.FrameXSL.TabIndex = 4
        Me.FrameXSL.TabStop = False
        Me.FrameXSL.Text = "XSL Transformation"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.DriveXSL, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.SplitFrameXSL, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(5, 18)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(230, 316)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'DriveXSL
        '
        Me.DriveXSL.BackColor = System.Drawing.SystemColors.Window
        Me.DriveXSL.Cursor = System.Windows.Forms.Cursors.Default
        Me.DriveXSL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DriveXSL.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DriveXSL.FormattingEnabled = True
        Me.DriveXSL.Location = New System.Drawing.Point(3, 3)
        Me.DriveXSL.Name = "DriveXSL"
        Me.DriveXSL.Size = New System.Drawing.Size(224, 21)
        Me.DriveXSL.TabIndex = 7
        '
        'SplitFrameXSL
        '
        Me.SplitFrameXSL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitFrameXSL.Location = New System.Drawing.Point(3, 31)
        Me.SplitFrameXSL.Name = "SplitFrameXSL"
        Me.SplitFrameXSL.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitFrameXSL.Panel1
        '
        Me.SplitFrameXSL.Panel1.Controls.Add(Me.DirXSL)
        '
        'SplitFrameXSL.Panel2
        '
        Me.SplitFrameXSL.Panel2.Controls.Add(Me.FileXSL)
        Me.SplitFrameXSL.Size = New System.Drawing.Size(224, 282)
        Me.SplitFrameXSL.SplitterDistance = 131
        Me.SplitFrameXSL.TabIndex = 8
        '
        'DirXSL
        '
        Me.DirXSL.BackColor = System.Drawing.SystemColors.Window
        Me.DirXSL.Cursor = System.Windows.Forms.Cursors.Default
        Me.DirXSL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DirXSL.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DirXSL.FormattingEnabled = True
        Me.DirXSL.IntegralHeight = False
        Me.DirXSL.Location = New System.Drawing.Point(0, 0)
        Me.DirXSL.Name = "DirXSL"
        Me.DirXSL.Size = New System.Drawing.Size(224, 131)
        Me.DirXSL.TabIndex = 6
        '
        'FileXSL
        '
        Me.FileXSL.BackColor = System.Drawing.SystemColors.Window
        Me.FileXSL.Cursor = System.Windows.Forms.Cursors.Default
        Me.FileXSL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FileXSL.ForeColor = System.Drawing.SystemColors.WindowText
        Me.FileXSL.FormattingEnabled = True
        Me.FileXSL.Location = New System.Drawing.Point(0, 0)
        Me.FileXSL.Name = "FileXSL"
        Me.FileXSL.Pattern = "*.xsl*"
        Me.FileXSL.Size = New System.Drawing.Size(224, 147)
        Me.FileXSL.TabIndex = 5
        '
        'FrameXML
        '
        Me.FrameXML.BackColor = System.Drawing.SystemColors.Control
        Me.FrameXML.Controls.Add(Me.TableLayoutPanel3)
        Me.FrameXML.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FrameXML.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameXML.Location = New System.Drawing.Point(3, 3)
        Me.FrameXML.Name = "FrameXML"
        Me.FrameXML.Padding = New System.Windows.Forms.Padding(5)
        Me.FrameXML.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameXML.Size = New System.Drawing.Size(240, 339)
        Me.FrameXML.TabIndex = 0
        Me.FrameXML.TabStop = False
        Me.FrameXML.Text = "Document XML"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 1
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.DriveXML, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.SplitContainer2, 0, 1)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(5, 18)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(230, 316)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'DriveXML
        '
        Me.DriveXML.BackColor = System.Drawing.SystemColors.Window
        Me.DriveXML.Cursor = System.Windows.Forms.Cursors.Default
        Me.DriveXML.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DriveXML.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DriveXML.FormattingEnabled = True
        Me.DriveXML.Location = New System.Drawing.Point(3, 3)
        Me.DriveXML.Name = "DriveXML"
        Me.DriveXML.Size = New System.Drawing.Size(224, 21)
        Me.DriveXML.TabIndex = 1
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(3, 31)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.DirXML)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.FileXML)
        Me.SplitContainer2.Size = New System.Drawing.Size(224, 282)
        Me.SplitContainer2.SplitterDistance = 157
        Me.SplitContainer2.TabIndex = 2
        '
        'DirXML
        '
        Me.DirXML.BackColor = System.Drawing.SystemColors.Window
        Me.DirXML.Cursor = System.Windows.Forms.Cursors.Default
        Me.DirXML.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DirXML.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DirXML.FormattingEnabled = True
        Me.DirXML.IntegralHeight = False
        Me.DirXML.Location = New System.Drawing.Point(0, 0)
        Me.DirXML.Name = "DirXML"
        Me.DirXML.Size = New System.Drawing.Size(224, 157)
        Me.DirXML.TabIndex = 2
        '
        'FileXML
        '
        Me.FileXML.BackColor = System.Drawing.SystemColors.Window
        Me.FileXML.Cursor = System.Windows.Forms.Cursors.Default
        Me.FileXML.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FileXML.ForeColor = System.Drawing.SystemColors.WindowText
        Me.FileXML.FormattingEnabled = True
        Me.FileXML.Location = New System.Drawing.Point(0, 0)
        Me.FileXML.Name = "FileXML"
        Me.FileXML.Pattern = "*.xml"
        Me.FileXML.Size = New System.Drawing.Size(224, 121)
        Me.FileXML.TabIndex = 3
        '
        'lblTransform
        '
        Me.lblTransform.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransform.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransform.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTransform.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransform.Location = New System.Drawing.Point(213, 66)
        Me.lblTransform.Name = "lblTransform"
        Me.lblTransform.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransform.Size = New System.Drawing.Size(456, 33)
        Me.lblTransform.TabIndex = 21
        Me.lblTransform.Text = "Ouput file"
        Me.lblTransform.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelXSL
        '
        Me.LabelXSL.BackColor = System.Drawing.SystemColors.Control
        Me.LabelXSL.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelXSL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelXSL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelXSL.Location = New System.Drawing.Point(213, 33)
        Me.LabelXSL.Name = "LabelXSL"
        Me.LabelXSL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelXSL.Size = New System.Drawing.Size(456, 33)
        Me.LabelXSL.TabIndex = 9
        Me.LabelXSL.Text = "XSL file"
        Me.LabelXSL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelXML
        '
        Me.LabelXML.BackColor = System.Drawing.SystemColors.Control
        Me.LabelXML.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelXML.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelXML.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelXML.Location = New System.Drawing.Point(213, 0)
        Me.LabelXML.Name = "LabelXML"
        Me.LabelXML.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelXML.Size = New System.Drawing.Size(456, 33)
        Me.LabelXML.TabIndex = 8
        Me.LabelXML.Text = "XML file"
        Me.LabelXML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.FrameXSL, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.chkSynchronize, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.FrameXML, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel5.SetRowSpan(Me.TableLayoutPanel1, 2)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(246, 725)
        Me.TableLayoutPanel1.TabIndex = 26
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 3
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.cmdNew, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.cmdRechargerXML, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.cmdSave, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.cmdRechargerXSL, 1, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.cmdParams, 0, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.LabelXML, 2, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.LabelXSL, 2, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.lblTransform, 2, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.FlowLayoutPanel1, 0, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.cmdValidate, 1, 2)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(255, 3)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 4
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(672, 134)
        Me.TableLayoutPanel4.TabIndex = 27
        '
        'FlowLayoutPanel1
        '
        Me.TableLayoutPanel4.SetColumnSpan(Me.FlowLayoutPanel1, 3)
        Me.FlowLayoutPanel1.Controls.Add(Me.Label1)
        Me.FlowLayoutPanel1.Controls.Add(Me.txtXMLFilter)
        Me.FlowLayoutPanel1.Controls.Add(Me.Label2)
        Me.FlowLayoutPanel1.Controls.Add(Me.txtMaskXSL)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 102)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(666, 29)
        Me.FlowLayoutPanel1.TabIndex = 26
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 30)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "XML mask:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(139, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 30)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "XSL mask:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMaskXSL
        '
        Me.txtMaskXSL.Location = New System.Drawing.Point(203, 3)
        Me.txtMaskXSL.Name = "txtMaskXSL"
        Me.txtMaskXSL.Size = New System.Drawing.Size(61, 20)
        Me.txtMaskXSL.TabIndex = 27
        Me.txtMaskXSL.Text = "*.xsl"
        '
        'cmdValidate
        '
        Me.cmdValidate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdValidate.Location = New System.Drawing.Point(93, 69)
        Me.cmdValidate.Name = "cmdValidate"
        Me.cmdValidate.Size = New System.Drawing.Size(114, 27)
        Me.cmdValidate.TabIndex = 27
        Me.cmdValidate.Text = "Valider destination"
        Me.cmdValidate.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.ColumnCount = 2
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.14286!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.85714!))
        Me.TableLayoutPanel5.Controls.Add(Me.TableLayoutPanel4, 1, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.TableLayoutPanel1, 0, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.TabControl1, 1, 1)
        Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 2
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140.0!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(930, 731)
        Me.TableLayoutPanel5.TabIndex = 28
        '
        'TabControl1
        '
        Me.TabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(255, 143)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(672, 585)
        Me.TabControl1.TabIndex = 28
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.WebBrowser)
        Me.TabPage1.Location = New System.Drawing.Point(4, 4)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(664, 559)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Browser"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.TextEditBox)
        Me.TabPage2.Location = New System.Drawing.Point(4, 4)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(664, 559)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Editor"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TextEditBox
        '
        Me.TextEditBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextEditBox.Location = New System.Drawing.Point(3, 3)
        Me.TextEditBox.Name = "TextEditBox"
        Me.TextEditBox.Size = New System.Drawing.Size(658, 553)
        Me.TextEditBox.TabIndex = 0
        Me.TextEditBox.Text = ""
        '
        'XMLTransformFilter
        '
        Me.AcceptButton = Me.cmdRechargerXSL
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(930, 731)
        Me.Controls.Add(Me.TableLayoutPanel5)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Location = New System.Drawing.Point(145, 54)
        Me.Name = "XMLTransformFilter"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "XSL Transformation"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.FrameXSL.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.SplitFrameXSL.Panel1.ResumeLayout(False)
        Me.SplitFrameXSL.Panel2.ResumeLayout(False)
        Me.SplitFrameXSL.ResumeLayout(False)
        Me.FrameXML.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SplitFrameXSL As System.Windows.Forms.SplitContainer
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TextEditBox As System.Windows.Forms.RichTextBox
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMaskXSL As System.Windows.Forms.TextBox
    Friend WithEvents cmdValidate As System.Windows.Forms.Button
#End Region
End Class