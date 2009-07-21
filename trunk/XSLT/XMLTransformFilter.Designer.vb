<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class XMLTransformFilter
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
    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.
    'Ne la modifiez pas à l'aide de l'éditeur de code.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdRechargerXSL = New System.Windows.Forms.Button
        Me.cmdNew = New System.Windows.Forms.Button
        Me.cmdParams = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.cmdRechargerXML = New System.Windows.Forms.Button
        Me.txtXMLFilter = New System.Windows.Forms.TextBox
        Me.FileXSL = New Microsoft.VisualBasic.Compatibility.VB6.FileListBox
        Me.FileXML = New Microsoft.VisualBasic.Compatibility.VB6.FileListBox
        Me.txtMaskXSL = New System.Windows.Forms.TextBox
        Me.chkSynchronize = New System.Windows.Forms.CheckBox
        Me.WebBrowser = New System.Windows.Forms.WebBrowser
        Me.FrameXSL = New System.Windows.Forms.GroupBox
        Me.tblXsltPanel = New System.Windows.Forms.TableLayoutPanel
        Me.DriveXSL = New Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
        Me.SplitFrameXSL = New System.Windows.Forms.SplitContainer
        Me.DirXSL = New Microsoft.VisualBasic.Compatibility.VB6.DirListBox
        Me.FrameXML = New System.Windows.Forms.GroupBox
        Me.tblXmlPanel = New System.Windows.Forms.TableLayoutPanel
        Me.DriveXML = New Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
        Me.SplitExplorers = New System.Windows.Forms.SplitContainer
        Me.DirXML = New Microsoft.VisualBasic.Compatibility.VB6.DirListBox
        Me.lblTransform = New System.Windows.Forms.Label
        Me.LabelXSL = New System.Windows.Forms.Label
        Me.LabelXML = New System.Windows.Forms.Label
        Me.tblExplorersPanel = New System.Windows.Forms.TableLayoutPanel
        Me.tblButtonsPanel = New System.Windows.Forms.TableLayoutPanel
        Me.FlowLayoutFilters = New System.Windows.Forms.FlowLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmdCode = New System.Windows.Forms.Button
        Me.cmdValidate = New System.Windows.Forms.Button
        Me.TabOutputControl = New System.Windows.Forms.TabControl
        Me.TabPageBrowser = New System.Windows.Forms.TabPage
        Me.TabPageEditor = New System.Windows.Forms.TabPage
        Me.TextEditBox = New System.Windows.Forms.RichTextBox
        Me.SplitMain = New System.Windows.Forms.SplitContainer
        Me.SplitView = New System.Windows.Forms.SplitContainer
        Me.strpProgressBar = New System.Windows.Forms.ProgressBar
        Me.FrameXSL.SuspendLayout()
        Me.tblXsltPanel.SuspendLayout()
        Me.SplitFrameXSL.Panel1.SuspendLayout()
        Me.SplitFrameXSL.Panel2.SuspendLayout()
        Me.SplitFrameXSL.SuspendLayout()
        Me.FrameXML.SuspendLayout()
        Me.tblXmlPanel.SuspendLayout()
        Me.SplitExplorers.Panel1.SuspendLayout()
        Me.SplitExplorers.Panel2.SuspendLayout()
        Me.SplitExplorers.SuspendLayout()
        Me.tblExplorersPanel.SuspendLayout()
        Me.tblButtonsPanel.SuspendLayout()
        Me.FlowLayoutFilters.SuspendLayout()
        Me.TabOutputControl.SuspendLayout()
        Me.TabPageBrowser.SuspendLayout()
        Me.TabPageEditor.SuspendLayout()
        Me.SplitMain.Panel1.SuspendLayout()
        Me.SplitMain.Panel2.SuspendLayout()
        Me.SplitMain.SuspendLayout()
        Me.SplitView.Panel1.SuspendLayout()
        Me.SplitView.Panel2.SuspendLayout()
        Me.SplitView.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdRechargerXSL
        '
        Me.cmdRechargerXSL.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRechargerXSL.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRechargerXSL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdRechargerXSL.Enabled = False
        Me.cmdRechargerXSL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRechargerXSL.Location = New System.Drawing.Point(101, 36)
        Me.cmdRechargerXSL.Name = "cmdRechargerXSL"
        Me.cmdRechargerXSL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRechargerXSL.Size = New System.Drawing.Size(106, 27)
        Me.cmdRechargerXSL.TabIndex = 19
        Me.cmdRechargerXSL.Text = "Reload XSLT"
        Me.ToolTip1.SetToolTip(Me.cmdRechargerXSL, "Reload stylesheet and restart process")
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
        Me.cmdNew.Size = New System.Drawing.Size(92, 27)
        Me.cmdNew.TabIndex = 23
        Me.cmdNew.Text = "New"
        Me.ToolTip1.SetToolTip(Me.cmdNew, "Reset output file")
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
        Me.cmdParams.Size = New System.Drawing.Size(92, 27)
        Me.cmdParams.TabIndex = 22
        Me.cmdParams.Text = "XSLT params..."
        Me.ToolTip1.SetToolTip(Me.cmdParams, "Input external XSLT parameters")
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
        Me.cmdSave.Size = New System.Drawing.Size(92, 27)
        Me.cmdSave.TabIndex = 11
        Me.cmdSave.Text = "Save as..."
        Me.ToolTip1.SetToolTip(Me.cmdSave, "Select file to save output")
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdRechargerXML
        '
        Me.cmdRechargerXML.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRechargerXML.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRechargerXML.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdRechargerXML.Enabled = False
        Me.cmdRechargerXML.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRechargerXML.Location = New System.Drawing.Point(101, 3)
        Me.cmdRechargerXML.Name = "cmdRechargerXML"
        Me.cmdRechargerXML.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRechargerXML.Size = New System.Drawing.Size(106, 27)
        Me.cmdRechargerXML.TabIndex = 10
        Me.cmdRechargerXML.Text = "Reload XML"
        Me.ToolTip1.SetToolTip(Me.cmdRechargerXML, "Reload XML document and restart process")
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
        Me.ToolTip1.SetToolTip(Me.txtXMLFilter, "XML document filters")
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
        Me.FileXSL.Size = New System.Drawing.Size(177, 147)
        Me.FileXSL.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.FileXSL, "Select a stylesheet")
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
        Me.FileXML.Size = New System.Drawing.Size(177, 121)
        Me.FileXML.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.FileXML, "Select an XML document")
        '
        'txtMaskXSL
        '
        Me.txtMaskXSL.Location = New System.Drawing.Point(203, 3)
        Me.txtMaskXSL.Name = "txtMaskXSL"
        Me.txtMaskXSL.Size = New System.Drawing.Size(61, 20)
        Me.txtMaskXSL.TabIndex = 27
        Me.txtMaskXSL.Text = "*.xsl"
        Me.ToolTip1.SetToolTip(Me.txtMaskXSL, "Stylesheets filter")
        '
        'chkSynchronize
        '
        Me.chkSynchronize.BackColor = System.Drawing.SystemColors.Control
        Me.chkSynchronize.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSynchronize.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkSynchronize.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSynchronize.Location = New System.Drawing.Point(3, 351)
        Me.chkSynchronize.Name = "chkSynchronize"
        Me.chkSynchronize.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSynchronize.Size = New System.Drawing.Size(193, 29)
        Me.chkSynchronize.TabIndex = 15
        Me.chkSynchronize.Text = "Synchronize both explorers"
        Me.chkSynchronize.UseVisualStyleBackColor = False
        '
        'WebBrowser
        '
        Me.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser.Location = New System.Drawing.Point(3, 3)
        Me.WebBrowser.Name = "WebBrowser"
        Me.WebBrowser.Size = New System.Drawing.Size(713, 562)
        Me.WebBrowser.TabIndex = 14
        '
        'FrameXSL
        '
        Me.FrameXSL.BackColor = System.Drawing.SystemColors.Control
        Me.FrameXSL.Controls.Add(Me.tblXsltPanel)
        Me.FrameXSL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FrameXSL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameXSL.Location = New System.Drawing.Point(3, 386)
        Me.FrameXSL.Name = "FrameXSL"
        Me.FrameXSL.Padding = New System.Windows.Forms.Padding(5)
        Me.FrameXSL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameXSL.Size = New System.Drawing.Size(193, 342)
        Me.FrameXSL.TabIndex = 4
        Me.FrameXSL.TabStop = False
        Me.FrameXSL.Text = "XSLT Stylesheet"
        '
        'tblXsltPanel
        '
        Me.tblXsltPanel.ColumnCount = 1
        Me.tblXsltPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblXsltPanel.Controls.Add(Me.DriveXSL, 0, 0)
        Me.tblXsltPanel.Controls.Add(Me.SplitFrameXSL, 0, 1)
        Me.tblXsltPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblXsltPanel.Location = New System.Drawing.Point(5, 18)
        Me.tblXsltPanel.Name = "tblXsltPanel"
        Me.tblXsltPanel.RowCount = 2
        Me.tblXsltPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.tblXsltPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblXsltPanel.Size = New System.Drawing.Size(183, 319)
        Me.tblXsltPanel.TabIndex = 0
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
        Me.DriveXSL.Size = New System.Drawing.Size(177, 21)
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
        Me.SplitFrameXSL.Size = New System.Drawing.Size(177, 285)
        Me.SplitFrameXSL.SplitterDistance = 132
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
        Me.DirXSL.Size = New System.Drawing.Size(177, 132)
        Me.DirXSL.TabIndex = 6
        '
        'FrameXML
        '
        Me.FrameXML.BackColor = System.Drawing.SystemColors.Control
        Me.FrameXML.Controls.Add(Me.tblXmlPanel)
        Me.FrameXML.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FrameXML.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameXML.Location = New System.Drawing.Point(3, 3)
        Me.FrameXML.Name = "FrameXML"
        Me.FrameXML.Padding = New System.Windows.Forms.Padding(5)
        Me.FrameXML.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameXML.Size = New System.Drawing.Size(193, 342)
        Me.FrameXML.TabIndex = 0
        Me.FrameXML.TabStop = False
        Me.FrameXML.Text = "XML Document"
        '
        'tblXmlPanel
        '
        Me.tblXmlPanel.ColumnCount = 1
        Me.tblXmlPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblXmlPanel.Controls.Add(Me.DriveXML, 0, 0)
        Me.tblXmlPanel.Controls.Add(Me.SplitExplorers, 0, 1)
        Me.tblXmlPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblXmlPanel.Location = New System.Drawing.Point(5, 18)
        Me.tblXmlPanel.Name = "tblXmlPanel"
        Me.tblXmlPanel.RowCount = 2
        Me.tblXmlPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.tblXmlPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblXmlPanel.Size = New System.Drawing.Size(183, 319)
        Me.tblXmlPanel.TabIndex = 0
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
        Me.DriveXML.Size = New System.Drawing.Size(177, 21)
        Me.DriveXML.TabIndex = 1
        '
        'SplitExplorers
        '
        Me.SplitExplorers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitExplorers.Location = New System.Drawing.Point(3, 31)
        Me.SplitExplorers.Name = "SplitExplorers"
        Me.SplitExplorers.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitExplorers.Panel1
        '
        Me.SplitExplorers.Panel1.Controls.Add(Me.DirXML)
        '
        'SplitExplorers.Panel2
        '
        Me.SplitExplorers.Panel2.Controls.Add(Me.FileXML)
        Me.SplitExplorers.Size = New System.Drawing.Size(177, 285)
        Me.SplitExplorers.SplitterDistance = 158
        Me.SplitExplorers.TabIndex = 2
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
        Me.DirXML.Size = New System.Drawing.Size(177, 158)
        Me.DirXML.TabIndex = 2
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
        Me.lblTransform.Size = New System.Drawing.Size(511, 33)
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
        Me.LabelXSL.Size = New System.Drawing.Size(511, 33)
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
        Me.LabelXML.Size = New System.Drawing.Size(511, 33)
        Me.LabelXML.TabIndex = 8
        Me.LabelXML.Text = "XML file"
        Me.LabelXML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tblExplorersPanel
        '
        Me.tblExplorersPanel.ColumnCount = 1
        Me.tblExplorersPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblExplorersPanel.Controls.Add(Me.FrameXSL, 0, 2)
        Me.tblExplorersPanel.Controls.Add(Me.chkSynchronize, 0, 1)
        Me.tblExplorersPanel.Controls.Add(Me.FrameXML, 0, 0)
        Me.tblExplorersPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblExplorersPanel.Location = New System.Drawing.Point(0, 0)
        Me.tblExplorersPanel.Name = "tblExplorersPanel"
        Me.tblExplorersPanel.RowCount = 3
        Me.tblExplorersPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblExplorersPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.tblExplorersPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblExplorersPanel.Size = New System.Drawing.Size(199, 731)
        Me.tblExplorersPanel.TabIndex = 26
        '
        'tblButtonsPanel
        '
        Me.tblButtonsPanel.ColumnCount = 3
        Me.tblButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98.0!))
        Me.tblButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112.0!))
        Me.tblButtonsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblButtonsPanel.Controls.Add(Me.cmdNew, 0, 0)
        Me.tblButtonsPanel.Controls.Add(Me.cmdRechargerXML, 1, 0)
        Me.tblButtonsPanel.Controls.Add(Me.cmdSave, 0, 1)
        Me.tblButtonsPanel.Controls.Add(Me.cmdRechargerXSL, 1, 1)
        Me.tblButtonsPanel.Controls.Add(Me.cmdParams, 0, 2)
        Me.tblButtonsPanel.Controls.Add(Me.LabelXML, 2, 0)
        Me.tblButtonsPanel.Controls.Add(Me.LabelXSL, 2, 1)
        Me.tblButtonsPanel.Controls.Add(Me.lblTransform, 2, 2)
        Me.tblButtonsPanel.Controls.Add(Me.FlowLayoutFilters, 0, 3)
        Me.tblButtonsPanel.Controls.Add(Me.cmdValidate, 1, 2)
        Me.tblButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblButtonsPanel.Location = New System.Drawing.Point(0, 0)
        Me.tblButtonsPanel.Name = "tblButtonsPanel"
        Me.tblButtonsPanel.RowCount = 4
        Me.tblButtonsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.tblButtonsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.tblButtonsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.tblButtonsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tblButtonsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tblButtonsPanel.Size = New System.Drawing.Size(727, 133)
        Me.tblButtonsPanel.TabIndex = 27
        '
        'FlowLayoutFilters
        '
        Me.tblButtonsPanel.SetColumnSpan(Me.FlowLayoutFilters, 3)
        Me.FlowLayoutFilters.Controls.Add(Me.Label1)
        Me.FlowLayoutFilters.Controls.Add(Me.txtXMLFilter)
        Me.FlowLayoutFilters.Controls.Add(Me.Label2)
        Me.FlowLayoutFilters.Controls.Add(Me.txtMaskXSL)
        Me.FlowLayoutFilters.Controls.Add(Me.cmdCode)
        Me.FlowLayoutFilters.Controls.Add(Me.strpProgressBar)
        Me.FlowLayoutFilters.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutFilters.Location = New System.Drawing.Point(3, 102)
        Me.FlowLayoutFilters.Name = "FlowLayoutFilters"
        Me.FlowLayoutFilters.Size = New System.Drawing.Size(721, 29)
        Me.FlowLayoutFilters.TabIndex = 26
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
        'cmdCode
        '
        Me.cmdCode.Location = New System.Drawing.Point(270, 3)
        Me.cmdCode.Name = "cmdCode"
        Me.cmdCode.Size = New System.Drawing.Size(93, 20)
        Me.cmdCode.TabIndex = 28
        Me.cmdCode.Text = "Generate code"
        Me.cmdCode.UseVisualStyleBackColor = True
        '
        'cmdValidate
        '
        Me.cmdValidate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdValidate.Location = New System.Drawing.Point(101, 69)
        Me.cmdValidate.Name = "cmdValidate"
        Me.cmdValidate.Size = New System.Drawing.Size(106, 27)
        Me.cmdValidate.TabIndex = 27
        Me.cmdValidate.Text = "Validate output"
        Me.cmdValidate.UseVisualStyleBackColor = True
        '
        'TabOutputControl
        '
        Me.TabOutputControl.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TabOutputControl.Controls.Add(Me.TabPageBrowser)
        Me.TabOutputControl.Controls.Add(Me.TabPageEditor)
        Me.TabOutputControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabOutputControl.Location = New System.Drawing.Point(0, 0)
        Me.TabOutputControl.Multiline = True
        Me.TabOutputControl.Name = "TabOutputControl"
        Me.TabOutputControl.SelectedIndex = 0
        Me.TabOutputControl.Size = New System.Drawing.Size(727, 594)
        Me.TabOutputControl.TabIndex = 28
        '
        'TabPageBrowser
        '
        Me.TabPageBrowser.Controls.Add(Me.WebBrowser)
        Me.TabPageBrowser.Location = New System.Drawing.Point(4, 4)
        Me.TabPageBrowser.Name = "TabPageBrowser"
        Me.TabPageBrowser.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageBrowser.Size = New System.Drawing.Size(719, 568)
        Me.TabPageBrowser.TabIndex = 0
        Me.TabPageBrowser.Text = "Browser"
        Me.TabPageBrowser.UseVisualStyleBackColor = True
        '
        'TabPageEditor
        '
        Me.TabPageEditor.Controls.Add(Me.TextEditBox)
        Me.TabPageEditor.Location = New System.Drawing.Point(4, 4)
        Me.TabPageEditor.Name = "TabPageEditor"
        Me.TabPageEditor.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageEditor.Size = New System.Drawing.Size(719, 568)
        Me.TabPageEditor.TabIndex = 1
        Me.TabPageEditor.Text = "Editor"
        Me.TabPageEditor.UseVisualStyleBackColor = True
        '
        'TextEditBox
        '
        Me.TextEditBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextEditBox.Location = New System.Drawing.Point(3, 3)
        Me.TextEditBox.Name = "TextEditBox"
        Me.TextEditBox.Size = New System.Drawing.Size(713, 562)
        Me.TextEditBox.TabIndex = 0
        Me.TextEditBox.Text = ""
        '
        'SplitMain
        '
        Me.SplitMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitMain.Location = New System.Drawing.Point(0, 0)
        Me.SplitMain.Name = "SplitMain"
        '
        'SplitMain.Panel1
        '
        Me.SplitMain.Panel1.Controls.Add(Me.tblExplorersPanel)
        Me.SplitMain.Panel1MinSize = 7
        '
        'SplitMain.Panel2
        '
        Me.SplitMain.Panel2.Controls.Add(Me.SplitView)
        Me.SplitMain.Panel2MinSize = 100
        Me.SplitMain.Size = New System.Drawing.Size(930, 731)
        Me.SplitMain.SplitterDistance = 199
        Me.SplitMain.TabIndex = 29
        '
        'SplitView
        '
        Me.SplitView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitView.Location = New System.Drawing.Point(0, 0)
        Me.SplitView.Name = "SplitView"
        Me.SplitView.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitView.Panel1
        '
        Me.SplitView.Panel1.Controls.Add(Me.tblButtonsPanel)
        Me.SplitView.Panel1MinSize = 7
        '
        'SplitView.Panel2
        '
        Me.SplitView.Panel2.Controls.Add(Me.TabOutputControl)
        Me.SplitView.Panel2MinSize = 100
        Me.SplitView.Size = New System.Drawing.Size(727, 731)
        Me.SplitView.SplitterDistance = 133
        Me.SplitView.TabIndex = 0
        '
        'strpProgressBar
        '
        Me.strpProgressBar.Location = New System.Drawing.Point(369, 3)
        Me.strpProgressBar.Name = "strpProgressBar"
        Me.strpProgressBar.Size = New System.Drawing.Size(93, 18)
        Me.strpProgressBar.TabIndex = 29
        Me.strpProgressBar.Visible = False
        '
        'XMLTransformFilter
        '
        Me.AcceptButton = Me.cmdRechargerXSL
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(930, 731)
        Me.Controls.Add(Me.SplitMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Location = New System.Drawing.Point(145, 54)
        Me.Name = "XMLTransformFilter"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "XSL Transformation"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.FrameXSL.ResumeLayout(False)
        Me.tblXsltPanel.ResumeLayout(False)
        Me.SplitFrameXSL.Panel1.ResumeLayout(False)
        Me.SplitFrameXSL.Panel2.ResumeLayout(False)
        Me.SplitFrameXSL.ResumeLayout(False)
        Me.FrameXML.ResumeLayout(False)
        Me.tblXmlPanel.ResumeLayout(False)
        Me.SplitExplorers.Panel1.ResumeLayout(False)
        Me.SplitExplorers.Panel2.ResumeLayout(False)
        Me.SplitExplorers.ResumeLayout(False)
        Me.tblExplorersPanel.ResumeLayout(False)
        Me.tblButtonsPanel.ResumeLayout(False)
        Me.FlowLayoutFilters.ResumeLayout(False)
        Me.FlowLayoutFilters.PerformLayout()
        Me.TabOutputControl.ResumeLayout(False)
        Me.TabPageBrowser.ResumeLayout(False)
        Me.TabPageEditor.ResumeLayout(False)
        Me.SplitMain.Panel1.ResumeLayout(False)
        Me.SplitMain.Panel2.ResumeLayout(False)
        Me.SplitMain.ResumeLayout(False)
        Me.SplitView.Panel1.ResumeLayout(False)
        Me.SplitView.Panel2.ResumeLayout(False)
        Me.SplitView.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tblXsltPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SplitFrameXSL As System.Windows.Forms.SplitContainer
    Friend WithEvents tblExplorersPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tblXmlPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SplitExplorers As System.Windows.Forms.SplitContainer
    Friend WithEvents tblButtonsPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TabOutputControl As System.Windows.Forms.TabControl
    Friend WithEvents TabPageBrowser As System.Windows.Forms.TabPage
    Friend WithEvents TabPageEditor As System.Windows.Forms.TabPage
    Friend WithEvents TextEditBox As System.Windows.Forms.RichTextBox
    Friend WithEvents FlowLayoutFilters As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMaskXSL As System.Windows.Forms.TextBox
    Friend WithEvents cmdValidate As System.Windows.Forms.Button
    Friend WithEvents SplitMain As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitView As System.Windows.Forms.SplitContainer
    Friend WithEvents cmdCode As System.Windows.Forms.Button
    Friend WithEvents strpProgressBar As System.Windows.Forms.ProgressBar
#End Region
End Class