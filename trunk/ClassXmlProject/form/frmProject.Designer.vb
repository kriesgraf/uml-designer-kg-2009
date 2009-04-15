<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProject
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
    '  <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProject))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.lvwProjectMembers = New ClassXmlProject.XmlDataListView
        Me.mnuProjectList = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddImport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddClass = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddPackage = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAddRelationship = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuProjectEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuProjectGenerate = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator25 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuProjectDependencies = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuProjectExport = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator23 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuProjectExportNodes = New System.Windows.Forms.ToolStripMenuItem
        Me.ProjectExportNodesSimpleCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.ProjectExportNodesExtract = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProjectImportNodes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProjectUpdateNodes = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuProjectRedundancies = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProjectDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.LargeIconList = New System.Windows.Forms.ImageList(Me.components)
        Me.SmallIconList = New System.Windows.Forms.ImageList(Me.components)
        Me.docvwProjectDisplay = New ClassXmlProject.XmlDocumentView
        Me.tlstrpNavigation = New System.Windows.Forms.ToolStrip
        Me.btnHome = New System.Windows.Forms.ToolStripButton
        Me.btnUp = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator30 = New System.Windows.Forms.ToolStripSeparator
        Me.btnProjectView = New System.Windows.Forms.ToolStripDropDownButton
        Me.LargeIconsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SmallIconsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator31 = New System.Windows.Forms.ToolStripSeparator
        Me.btnCopy = New System.Windows.Forms.ToolStripButton
        Me.btnCut = New System.Windows.Forms.ToolStripButton
        Me.btnPaste = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator32 = New System.Windows.Forms.ToolStripSeparator
        Me.btnDocView = New System.Windows.Forms.ToolStripDropDownButton
        Me.DatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UmlViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CodeSourceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnZoomIn = New System.Windows.Forms.ToolStripButton
        Me.btnZoomOut = New System.Windows.Forms.ToolStripButton
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer
        Me.mnuBar = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileOpen = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFileNewDoxygenFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileNewOmgUmlFile = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuApplyPatch = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditDatabase = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFileSave = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSaveAs = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFileClose = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFilePrint = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilePrintPreview = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem
        Me.EditMenu = New System.Windows.Forms.ToolStripMenuItem
        Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuEditCut = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditPaste = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditDuplicate = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuEditSelectAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProject = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProjectProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProjectMakefile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuUpdateSimpleTypes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProjectParameters = New System.Windows.Forms.ToolStripMenuItem
        Me.RenumberDatabaseIndex = New System.Windows.Forms.ToolStripMenuItem
        Me.UpdatesCollaborations = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPackageList = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuPackageAddImport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPackageAddClass = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPackageAddPackage = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPackageEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPackageMoveUp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPackageProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPackageGenerate = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator26 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPackageDependencies = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator21 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPackageExportReference = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator24 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPackageExportNodes = New System.Windows.Forms.ToolStripMenuItem
        Me.PackageExportNodesSimpleCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.PackageExportNodesExtract = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPackageImportNodes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPackageUpdateNodes = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPackageRedundancies = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPackageDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClassMembers = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddClassMember = New System.Windows.Forms.ToolStripMenuItem
        Me.AddClassImport = New System.Windows.Forms.ToolStripMenuItem
        Me.AddClassTypedef = New System.Windows.Forms.ToolStripMenuItem
        Me.AddClassContainer = New System.Windows.Forms.ToolStripMenuItem
        Me.AddClassStructure = New System.Windows.Forms.ToolStripMenuItem
        Me.AddClassProperty = New System.Windows.Forms.ToolStripMenuItem
        Me.AddClassConstructor = New System.Windows.Forms.ToolStripMenuItem
        Me.AddClassMethod = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClassMemberEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClassMemberProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuOverrides = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOverrideProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOverrideMethods = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator27 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuClassDependencies = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator22 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuClassMemberExport = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuClassDeleteMember = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEditReference = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewReference = New System.Windows.Forms.ToolStripMenuItem
        Me.NewInterface = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator
        Me.EditReference = New System.Windows.Forms.ToolStripMenuItem
        Me.ReferenceProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator34 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuRefDependencies = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator
        Me.AddReferences = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReplaceExport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMergeExport = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuConfirmExport = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator
        Me.FindRedundant = New System.Windows.Forms.ToolStripMenuItem
        Me.DeleteReference = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveAll = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.mnuProjectList.SuspendLayout()
        Me.tlstrpNavigation.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.mnuBar.SuspendLayout()
        Me.mnuPackageList.SuspendLayout()
        Me.mnuClassMembers.SuspendLayout()
        Me.mnuEditReference.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lvwProjectMembers)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.docvwProjectDisplay)
        Me.SplitContainer1.Size = New System.Drawing.Size(897, 610)
        Me.SplitContainer1.SplitterDistance = 267
        Me.SplitContainer1.TabIndex = 0
        '
        'lvwProjectMembers
        '
        Me.lvwProjectMembers.AllowDrop = True
        Me.lvwProjectMembers.BindingSource = Nothing
        Me.lvwProjectMembers.ContextMenuStrip = Me.mnuProjectList
        Me.lvwProjectMembers.DataSource = Nothing
        Me.lvwProjectMembers.DisplayMember = Nothing
        Me.lvwProjectMembers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwProjectMembers.LabelEdit = True
        Me.lvwProjectMembers.LargeImageList = Me.LargeIconList
        Me.lvwProjectMembers.Location = New System.Drawing.Point(0, 0)
        Me.lvwProjectMembers.MultiSelect = False
        Me.lvwProjectMembers.Name = "lvwProjectMembers"
        Me.lvwProjectMembers.ShowGroups = False
        Me.lvwProjectMembers.ShowItemToolTips = True
        Me.lvwProjectMembers.Size = New System.Drawing.Size(267, 610)
        Me.lvwProjectMembers.SmallImageList = Me.SmallIconList
        Me.lvwProjectMembers.TabIndex = 1
        Me.lvwProjectMembers.TileSize = New System.Drawing.Size(160, 90)
        Me.ToolTip1.SetToolTip(Me.lvwProjectMembers, "Click right to display menu")
        Me.lvwProjectMembers.UseCompatibleStateImageBehavior = False
        Me.lvwProjectMembers.ValueMember = Nothing
        '
        'mnuProjectList
        '
        Me.mnuProjectList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddImport, Me.mnuAddClass, Me.mnuAddPackage, Me.mnuAddRelationship, Me.ToolStripSeparator2, Me.mnuProjectEdit, Me.mnuEditProperties, Me.ToolStripSeparator8, Me.mnuProjectGenerate, Me.ToolStripSeparator25, Me.mnuProjectDependencies, Me.ToolStripSeparator20, Me.mnuProjectExport, Me.ToolStripSeparator23, Me.mnuProjectExportNodes, Me.mnuProjectImportNodes, Me.mnuProjectUpdateNodes, Me.ToolStripSeparator16, Me.mnuProjectRedundancies, Me.mnuProjectDelete})
        Me.mnuProjectList.Name = "mnuEditList"
        Me.mnuProjectList.Size = New System.Drawing.Size(189, 348)
        '
        'mnuAddImport
        '
        Me.mnuAddImport.Image = Global.ClassXmlProject.My.Resources.Resources.library
        Me.mnuAddImport.Name = "mnuAddImport"
        Me.mnuAddImport.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.mnuAddImport.Size = New System.Drawing.Size(188, 22)
        Me.mnuAddImport.Tag = "import"
        Me.mnuAddImport.Text = "Add import"
        '
        'mnuAddClass
        '
        Me.mnuAddClass.Image = Global.ClassXmlProject.My.Resources.Resources.Address_Book
        Me.mnuAddClass.Name = "mnuAddClass"
        Me.mnuAddClass.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuAddClass.Size = New System.Drawing.Size(188, 22)
        Me.mnuAddClass.Tag = "class"
        Me.mnuAddClass.Text = "Add class"
        '
        'mnuAddPackage
        '
        Me.mnuAddPackage.Image = Global.ClassXmlProject.My.Resources.Resources.my_Documents
        Me.mnuAddPackage.Name = "mnuAddPackage"
        Me.mnuAddPackage.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuAddPackage.Size = New System.Drawing.Size(188, 22)
        Me.mnuAddPackage.Tag = "package"
        Me.mnuAddPackage.Text = "Add package"
        '
        'mnuAddRelationship
        '
        Me.mnuAddRelationship.Image = Global.ClassXmlProject.My.Resources.Resources.network
        Me.mnuAddRelationship.Name = "mnuAddRelationship"
        Me.mnuAddRelationship.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.mnuAddRelationship.Size = New System.Drawing.Size(188, 22)
        Me.mnuAddRelationship.Tag = "relationship"
        Me.mnuAddRelationship.Text = "Add relationship"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(185, 6)
        '
        'mnuProjectEdit
        '
        Me.mnuProjectEdit.Image = Global.ClassXmlProject.My.Resources.Resources.Rename___Edit
        Me.mnuProjectEdit.Name = "mnuProjectEdit"
        Me.mnuProjectEdit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuProjectEdit.Size = New System.Drawing.Size(188, 22)
        Me.mnuProjectEdit.Text = "Edit..."
        '
        'mnuEditProperties
        '
        Me.mnuEditProperties.Name = "mnuEditProperties"
        Me.mnuEditProperties.Size = New System.Drawing.Size(188, 22)
        Me.mnuEditProperties.Text = "Parameters..."
        Me.mnuEditProperties.Visible = False
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(185, 6)
        '
        'mnuProjectGenerate
        '
        Me.mnuProjectGenerate.Image = Global.ClassXmlProject.My.Resources.Resources.Publish
        Me.mnuProjectGenerate.Name = "mnuProjectGenerate"
        Me.mnuProjectGenerate.Size = New System.Drawing.Size(188, 22)
        Me.mnuProjectGenerate.Text = "Generate code"
        '
        'ToolStripSeparator25
        '
        Me.ToolStripSeparator25.Name = "ToolStripSeparator25"
        Me.ToolStripSeparator25.Size = New System.Drawing.Size(185, 6)
        '
        'mnuProjectDependencies
        '
        Me.mnuProjectDependencies.Image = Global.ClassXmlProject.My.Resources.Resources.Search
        Me.mnuProjectDependencies.Name = "mnuProjectDependencies"
        Me.mnuProjectDependencies.Size = New System.Drawing.Size(188, 22)
        Me.mnuProjectDependencies.Text = "Search dependencies..."
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(185, 6)
        '
        'mnuProjectExport
        '
        Me.mnuProjectExport.Image = Global.ClassXmlProject.My.Resources.Resources.Forward_2
        Me.mnuProjectExport.Name = "mnuProjectExport"
        Me.mnuProjectExport.Size = New System.Drawing.Size(188, 22)
        Me.mnuProjectExport.Text = "Export references..."
        '
        'ToolStripSeparator23
        '
        Me.ToolStripSeparator23.Name = "ToolStripSeparator23"
        Me.ToolStripSeparator23.Size = New System.Drawing.Size(185, 6)
        '
        'mnuProjectExportNodes
        '
        Me.mnuProjectExportNodes.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProjectExportNodesSimpleCopy, Me.ProjectExportNodesExtract})
        Me.mnuProjectExportNodes.Image = Global.ClassXmlProject.My.Resources.Resources.Forward_2
        Me.mnuProjectExportNodes.Name = "mnuProjectExportNodes"
        Me.mnuProjectExportNodes.Size = New System.Drawing.Size(188, 22)
        Me.mnuProjectExportNodes.Text = "Export nodes"
        '
        'ProjectExportNodesSimpleCopy
        '
        Me.ProjectExportNodesSimpleCopy.Image = Global.ClassXmlProject.My.Resources.Resources.Copy
        Me.ProjectExportNodesSimpleCopy.Name = "ProjectExportNodesSimpleCopy"
        Me.ProjectExportNodesSimpleCopy.Size = New System.Drawing.Size(142, 22)
        Me.ProjectExportNodesSimpleCopy.Text = "Simple copy..."
        '
        'ProjectExportNodesExtract
        '
        Me.ProjectExportNodesExtract.Image = Global.ClassXmlProject.My.Resources.Resources.SFX1
        Me.ProjectExportNodesExtract.Name = "ProjectExportNodesExtract"
        Me.ProjectExportNodesExtract.Size = New System.Drawing.Size(142, 22)
        Me.ProjectExportNodesExtract.Text = "Extract..."
        '
        'mnuProjectImportNodes
        '
        Me.mnuProjectImportNodes.Image = Global.ClassXmlProject.My.Resources.Resources.Back_2
        Me.mnuProjectImportNodes.Name = "mnuProjectImportNodes"
        Me.mnuProjectImportNodes.Size = New System.Drawing.Size(188, 22)
        Me.mnuProjectImportNodes.Text = "Import nodes..."
        '
        'mnuProjectUpdateNodes
        '
        Me.mnuProjectUpdateNodes.Image = Global.ClassXmlProject.My.Resources.Resources.Back_2
        Me.mnuProjectUpdateNodes.Name = "mnuProjectUpdateNodes"
        Me.mnuProjectUpdateNodes.Size = New System.Drawing.Size(188, 22)
        Me.mnuProjectUpdateNodes.Text = "Merge nodes..."
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(185, 6)
        '
        'mnuProjectRedundancies
        '
        Me.mnuProjectRedundancies.Image = Global.ClassXmlProject.My.Resources.Resources._Stop
        Me.mnuProjectRedundancies.Name = "mnuProjectRedundancies"
        Me.mnuProjectRedundancies.Size = New System.Drawing.Size(188, 22)
        Me.mnuProjectRedundancies.Text = "Remove redundancy..."
        '
        'mnuProjectDelete
        '
        Me.mnuProjectDelete.Image = Global.ClassXmlProject.My.Resources.Resources.Stop_2
        Me.mnuProjectDelete.Name = "mnuProjectDelete"
        Me.mnuProjectDelete.Size = New System.Drawing.Size(188, 22)
        Me.mnuProjectDelete.Text = "Delete"
        '
        'LargeIconList
        '
        Me.LargeIconList.ImageStream = CType(resources.GetObject("LargeIconList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.LargeIconList.TransparentColor = System.Drawing.Color.Transparent
        Me.LargeIconList.Images.SetKeyName(0, "my Documents.ico")
        Me.LargeIconList.Images.SetKeyName(1, "Address-Book.ico")
        Me.LargeIconList.Images.SetKeyName(2, "network.ico")
        Me.LargeIconList.Images.SetKeyName(3, "library.ico")
        Me.LargeIconList.Images.SetKeyName(4, "Apps.ico")
        Me.LargeIconList.Images.SetKeyName(5, "Properties.ico")
        Me.LargeIconList.Images.SetKeyName(6, "Move.ico")
        '
        'SmallIconList
        '
        Me.SmallIconList.ImageStream = CType(resources.GetObject("SmallIconList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.SmallIconList.TransparentColor = System.Drawing.Color.Transparent
        Me.SmallIconList.Images.SetKeyName(0, "my Documents.ico")
        Me.SmallIconList.Images.SetKeyName(1, "Address-Book.ico")
        Me.SmallIconList.Images.SetKeyName(2, "network.ico")
        Me.SmallIconList.Images.SetKeyName(3, "library.ico")
        Me.SmallIconList.Images.SetKeyName(4, "Apps.ico")
        Me.SmallIconList.Images.SetKeyName(5, "Properties.ico")
        Me.SmallIconList.Images.SetKeyName(6, "Move.ico")
        '
        'docvwProjectDisplay
        '
        Me.docvwProjectDisplay.DataSource = Nothing
        Me.docvwProjectDisplay.Display = ""
        Me.docvwProjectDisplay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.docvwProjectDisplay.IsWebBrowserContextMenuEnabled = False
        Me.docvwProjectDisplay.Language = 0
        Me.docvwProjectDisplay.Location = New System.Drawing.Point(0, 0)
        Me.docvwProjectDisplay.MinimumSize = New System.Drawing.Size(20, 20)
        Me.docvwProjectDisplay.Name = "docvwProjectDisplay"
        Me.docvwProjectDisplay.Size = New System.Drawing.Size(626, 610)
        Me.docvwProjectDisplay.TabIndex = 0
        Me.docvwProjectDisplay.View = ClassXmlProject.XmlDocumentViewMode.Unknown
        '
        'tlstrpNavigation
        '
        Me.tlstrpNavigation.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnHome, Me.btnUp, Me.ToolStripSeparator30, Me.btnProjectView, Me.ToolStripSeparator31, Me.btnCopy, Me.btnCut, Me.btnPaste, Me.ToolStripSeparator32, Me.btnDocView, Me.btnZoomIn, Me.btnZoomOut})
        Me.tlstrpNavigation.Location = New System.Drawing.Point(0, 0)
        Me.tlstrpNavigation.Name = "tlstrpNavigation"
        Me.tlstrpNavigation.Size = New System.Drawing.Size(897, 25)
        Me.tlstrpNavigation.TabIndex = 1
        Me.tlstrpNavigation.Text = "ToolStrip1"
        '
        'btnHome
        '
        Me.btnHome.Image = CType(resources.GetObject("btnHome.Image"), System.Drawing.Image)
        Me.btnHome.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnHome.Name = "btnHome"
        Me.btnHome.Size = New System.Drawing.Size(54, 22)
        Me.btnHome.Text = "Home"
        Me.btnHome.ToolTipText = "Go to project root"
        '
        'btnUp
        '
        Me.btnUp.Image = CType(resources.GetObject("btnUp.Image"), System.Drawing.Image)
        Me.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(40, 22)
        Me.btnUp.Text = "Up"
        Me.btnUp.ToolTipText = "Go to parent node"
        '
        'ToolStripSeparator30
        '
        Me.ToolStripSeparator30.Name = "ToolStripSeparator30"
        Me.ToolStripSeparator30.Size = New System.Drawing.Size(6, 25)
        '
        'btnProjectView
        '
        Me.btnProjectView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LargeIconsToolStripMenuItem, Me.DetailsToolStripMenuItem, Me.SmallIconsToolStripMenuItem, Me.ListToolStripMenuItem, Me.TileToolStripMenuItem})
        Me.btnProjectView.Image = Global.ClassXmlProject.My.Resources.Resources.btnProjectView_Image
        Me.btnProjectView.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnProjectView.Name = "btnProjectView"
        Me.btnProjectView.Size = New System.Drawing.Size(95, 22)
        Me.btnProjectView.Text = "Project view"
        '
        'LargeIconsToolStripMenuItem
        '
        Me.LargeIconsToolStripMenuItem.Name = "LargeIconsToolStripMenuItem"
        Me.LargeIconsToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.LargeIconsToolStripMenuItem.Tag = "0"
        Me.LargeIconsToolStripMenuItem.Text = "Large icons"
        '
        'DetailsToolStripMenuItem
        '
        Me.DetailsToolStripMenuItem.Name = "DetailsToolStripMenuItem"
        Me.DetailsToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.DetailsToolStripMenuItem.Tag = "1"
        Me.DetailsToolStripMenuItem.Text = "Details"
        '
        'SmallIconsToolStripMenuItem
        '
        Me.SmallIconsToolStripMenuItem.Name = "SmallIconsToolStripMenuItem"
        Me.SmallIconsToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.SmallIconsToolStripMenuItem.Tag = "2"
        Me.SmallIconsToolStripMenuItem.Text = "Small icons"
        '
        'ListToolStripMenuItem
        '
        Me.ListToolStripMenuItem.Name = "ListToolStripMenuItem"
        Me.ListToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ListToolStripMenuItem.Tag = "3"
        Me.ListToolStripMenuItem.Text = "List"
        '
        'TileToolStripMenuItem
        '
        Me.TileToolStripMenuItem.Name = "TileToolStripMenuItem"
        Me.TileToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.TileToolStripMenuItem.Tag = "4"
        Me.TileToolStripMenuItem.Text = "Tile"
        '
        'ToolStripSeparator31
        '
        Me.ToolStripSeparator31.Name = "ToolStripSeparator31"
        Me.ToolStripSeparator31.Size = New System.Drawing.Size(6, 25)
        '
        'btnCopy
        '
        Me.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnCopy.Image = Global.ClassXmlProject.My.Resources.Resources.CopyToolStripMenuItem_Image
        Me.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(23, 22)
        Me.btnCopy.Text = "Copy"
        '
        'btnCut
        '
        Me.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnCut.Image = Global.ClassXmlProject.My.Resources.Resources.CutToolStripMenuItem_Image
        Me.btnCut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCut.Name = "btnCut"
        Me.btnCut.Size = New System.Drawing.Size(23, 22)
        Me.btnCut.Text = "Cut"
        '
        'btnPaste
        '
        Me.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnPaste.Enabled = False
        Me.btnPaste.Image = Global.ClassXmlProject.My.Resources.Resources.PasteToolStripMenuItem_Image
        Me.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPaste.Name = "btnPaste"
        Me.btnPaste.Size = New System.Drawing.Size(23, 22)
        Me.btnPaste.Text = "Paste"
        '
        'ToolStripSeparator32
        '
        Me.ToolStripSeparator32.Name = "ToolStripSeparator32"
        Me.ToolStripSeparator32.Size = New System.Drawing.Size(6, 25)
        '
        'btnDocView
        '
        Me.btnDocView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DatabaseToolStripMenuItem, Me.UmlViewToolStripMenuItem, Me.CodeSourceToolStripMenuItem})
        Me.btnDocView.Image = Global.ClassXmlProject.My.Resources.Resources.btnDocView_Image
        Me.btnDocView.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDocView.Name = "btnDocView"
        Me.btnDocView.Size = New System.Drawing.Size(109, 22)
        Me.btnDocView.Text = "Document view"
        '
        'DatabaseToolStripMenuItem
        '
        Me.DatabaseToolStripMenuItem.Name = "DatabaseToolStripMenuItem"
        Me.DatabaseToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.DatabaseToolStripMenuItem.Tag = "1"
        Me.DatabaseToolStripMenuItem.Text = "Database"
        '
        'UmlViewToolStripMenuItem
        '
        Me.UmlViewToolStripMenuItem.Name = "UmlViewToolStripMenuItem"
        Me.UmlViewToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.UmlViewToolStripMenuItem.Tag = "2"
        Me.UmlViewToolStripMenuItem.Text = "UML view"
        '
        'CodeSourceToolStripMenuItem
        '
        Me.CodeSourceToolStripMenuItem.Name = "CodeSourceToolStripMenuItem"
        Me.CodeSourceToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.CodeSourceToolStripMenuItem.Tag = "3"
        Me.CodeSourceToolStripMenuItem.Text = "Code source"
        '
        'btnZoomIn
        '
        Me.btnZoomIn.Image = CType(resources.GetObject("btnZoomIn.Image"), System.Drawing.Image)
        Me.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnZoomIn.Name = "btnZoomIn"
        Me.btnZoomIn.Size = New System.Drawing.Size(64, 22)
        Me.btnZoomIn.Text = "Zoom in"
        '
        'btnZoomOut
        '
        Me.btnZoomOut.Image = CType(resources.GetObject("btnZoomOut.Image"), System.Drawing.Image)
        Me.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnZoomOut.Name = "btnZoomOut"
        Me.btnZoomOut.Size = New System.Drawing.Size(72, 22)
        Me.btnZoomOut.Text = "Zoom out"
        '
        'ToolStripContainer1
        '
        Me.ToolStripContainer1.BottomToolStripPanelVisible = False
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SplitContainer1)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(897, 610)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.LeftToolStripPanelVisible = False
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 25)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.RightToolStripPanelVisible = False
        Me.ToolStripContainer1.Size = New System.Drawing.Size(897, 610)
        Me.ToolStripContainer1.TabIndex = 2
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        Me.ToolStripContainer1.TopToolStripPanelVisible = False
        '
        'mnuBar
        '
        Me.mnuBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.EditMenu, Me.mnuProject})
        Me.mnuBar.Location = New System.Drawing.Point(0, 0)
        Me.mnuBar.Name = "mnuBar"
        Me.mnuBar.Size = New System.Drawing.Size(897, 24)
        Me.mnuBar.TabIndex = 3
        Me.mnuBar.Text = "MenuStrip"
        Me.mnuBar.Visible = False
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileNew, Me.mnuFileOpen, Me.ToolStripSeparator18, Me.mnuFileNewDoxygenFile, Me.mnuFileNewOmgUmlFile, Me.ToolStripMenuItem1, Me.ToolStripSeparator3, Me.mnuFileSave, Me.mnuFileSaveAs, Me.ToolStripSeparator4, Me.mnuFileClose, Me.ToolStripSeparator9, Me.mnuFilePrint, Me.mnuFilePrintPreview, Me.ToolStripSeparator5, Me.mnuFileExit})
        Me.mnuFile.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder
        Me.mnuFile.MergeAction = System.Windows.Forms.MergeAction.Replace
        Me.mnuFile.MergeIndex = 0
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(35, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuFileNew
        '
        Me.mnuFileNew.Image = Global.ClassXmlProject.My.Resources.Resources.NewToolStripMenuItem_Image
        Me.mnuFileNew.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuFileNew.Name = "mnuFileNew"
        Me.mnuFileNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mnuFileNew.Size = New System.Drawing.Size(252, 22)
        Me.mnuFileNew.Text = "&New"
        '
        'mnuFileOpen
        '
        Me.mnuFileOpen.Image = Global.ClassXmlProject.My.Resources.Resources.OpenToolStripMenuItem_Image
        Me.mnuFileOpen.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuFileOpen.Name = "mnuFileOpen"
        Me.mnuFileOpen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuFileOpen.Size = New System.Drawing.Size(252, 22)
        Me.mnuFileOpen.Text = "&Open..."
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(249, 6)
        '
        'mnuFileNewDoxygenFile
        '
        Me.mnuFileNewDoxygenFile.Name = "mnuFileNewDoxygenFile"
        Me.mnuFileNewDoxygenFile.Size = New System.Drawing.Size(252, 22)
        Me.mnuFileNewDoxygenFile.Text = "Import from Doxygen XML file..."
        '
        'mnuFileNewOmgUmlFile
        '
        Me.mnuFileNewOmgUmlFile.Name = "mnuFileNewOmgUmlFile"
        Me.mnuFileNewOmgUmlFile.Size = New System.Drawing.Size(252, 22)
        Me.mnuFileNewOmgUmlFile.Text = "Import from OMG UML  2.1 XMI file..."
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuApplyPatch, Me.mnuEditDatabase})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(252, 22)
        Me.ToolStripMenuItem1.Text = "Patchs"
        '
        'mnuApplyPatch
        '
        Me.mnuApplyPatch.Name = "mnuApplyPatch"
        Me.mnuApplyPatch.Size = New System.Drawing.Size(152, 22)
        Me.mnuApplyPatch.Text = "Apply...."
        '
        'mnuEditDatabase
        '
        Me.mnuEditDatabase.Name = "mnuEditDatabase"
        Me.mnuEditDatabase.Size = New System.Drawing.Size(152, 22)
        Me.mnuEditDatabase.Text = "Edit database..."
        Me.mnuEditDatabase.Visible = False
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(249, 6)
        '
        'mnuFileSave
        '
        Me.mnuFileSave.Image = Global.ClassXmlProject.My.Resources.Resources.SaveToolStripMenuItem_Image
        Me.mnuFileSave.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuFileSave.Name = "mnuFileSave"
        Me.mnuFileSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuFileSave.Size = New System.Drawing.Size(252, 22)
        Me.mnuFileSave.Text = "&Save"
        '
        'mnuFileSaveAs
        '
        Me.mnuFileSaveAs.Name = "mnuFileSaveAs"
        Me.mnuFileSaveAs.Size = New System.Drawing.Size(252, 22)
        Me.mnuFileSaveAs.Text = "Save &as..."
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(249, 6)
        '
        'mnuFileClose
        '
        Me.mnuFileClose.Name = "mnuFileClose"
        Me.mnuFileClose.Size = New System.Drawing.Size(252, 22)
        Me.mnuFileClose.Text = "Close"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(249, 6)
        '
        'mnuFilePrint
        '
        Me.mnuFilePrint.Image = Global.ClassXmlProject.My.Resources.Resources.PrintToolStripMenuItem_Image
        Me.mnuFilePrint.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuFilePrint.Name = "mnuFilePrint"
        Me.mnuFilePrint.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuFilePrint.Size = New System.Drawing.Size(252, 22)
        Me.mnuFilePrint.Text = "Prin&t..."
        '
        'mnuFilePrintPreview
        '
        Me.mnuFilePrintPreview.Image = Global.ClassXmlProject.My.Resources.Resources.PrintPreviewToolStripMenuItem_Image
        Me.mnuFilePrintPreview.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuFilePrintPreview.Name = "mnuFilePrintPreview"
        Me.mnuFilePrintPreview.Size = New System.Drawing.Size(252, 22)
        Me.mnuFilePrintPreview.Text = "Print pre&view..."
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(249, 6)
        '
        'mnuFileExit
        '
        Me.mnuFileExit.Name = "mnuFileExit"
        Me.mnuFileExit.Size = New System.Drawing.Size(252, 22)
        Me.mnuFileExit.Text = "&Quitter"
        '
        'EditMenu
        '
        Me.EditMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem, Me.RedoToolStripMenuItem, Me.ToolStripSeparator6, Me.mnuEditCut, Me.mnuEditCopy, Me.mnuEditPaste, Me.mnuEditDuplicate, Me.ToolStripSeparator7, Me.mnuEditSelectAll})
        Me.EditMenu.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.EditMenu.MergeIndex = 1
        Me.EditMenu.Name = "EditMenu"
        Me.EditMenu.Size = New System.Drawing.Size(37, 20)
        Me.EditMenu.Text = "&Edit"
        '
        'UndoToolStripMenuItem
        '
        Me.UndoToolStripMenuItem.Enabled = False
        Me.UndoToolStripMenuItem.Image = Global.ClassXmlProject.My.Resources.Resources.UndoToolStripMenuItem_Image
        Me.UndoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.UndoToolStripMenuItem.Text = "&Undo"
        '
        'RedoToolStripMenuItem
        '
        Me.RedoToolStripMenuItem.Enabled = False
        Me.RedoToolStripMenuItem.Image = Global.ClassXmlProject.My.Resources.Resources.RedoToolStripMenuItem_Image
        Me.RedoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black
        Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
        Me.RedoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.RedoToolStripMenuItem.Text = "&Redo"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(157, 6)
        '
        'mnuEditCut
        '
        Me.mnuEditCut.Image = Global.ClassXmlProject.My.Resources.Resources.CutToolStripMenuItem_Image
        Me.mnuEditCut.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuEditCut.Name = "mnuEditCut"
        Me.mnuEditCut.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.mnuEditCut.Size = New System.Drawing.Size(160, 22)
        Me.mnuEditCut.Text = "C&ut"
        '
        'mnuEditCopy
        '
        Me.mnuEditCopy.Image = Global.ClassXmlProject.My.Resources.Resources.CopyToolStripMenuItem_Image
        Me.mnuEditCopy.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuEditCopy.Name = "mnuEditCopy"
        Me.mnuEditCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuEditCopy.Size = New System.Drawing.Size(160, 22)
        Me.mnuEditCopy.Text = "&Copy"
        '
        'mnuEditPaste
        '
        Me.mnuEditPaste.Enabled = False
        Me.mnuEditPaste.Image = Global.ClassXmlProject.My.Resources.Resources.PasteToolStripMenuItem_Image
        Me.mnuEditPaste.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuEditPaste.Name = "mnuEditPaste"
        Me.mnuEditPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuEditPaste.Size = New System.Drawing.Size(160, 22)
        Me.mnuEditPaste.Text = "Pas&te"
        '
        'mnuEditDuplicate
        '
        Me.mnuEditDuplicate.Name = "mnuEditDuplicate"
        Me.mnuEditDuplicate.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.mnuEditDuplicate.Size = New System.Drawing.Size(160, 22)
        Me.mnuEditDuplicate.Text = "&Duplicate"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(157, 6)
        '
        'mnuEditSelectAll
        '
        Me.mnuEditSelectAll.Enabled = False
        Me.mnuEditSelectAll.Name = "mnuEditSelectAll"
        Me.mnuEditSelectAll.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.mnuEditSelectAll.Size = New System.Drawing.Size(160, 22)
        Me.mnuEditSelectAll.Text = "Select &All"
        '
        'mnuProject
        '
        Me.mnuProject.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuProjectProperties, Me.mnuProjectMakefile, Me.mnuUpdateSimpleTypes, Me.mnuProjectParameters, Me.RenumberDatabaseIndex, Me.UpdatesCollaborations})
        Me.mnuProject.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuProject.MergeIndex = 2
        Me.mnuProject.Name = "mnuProject"
        Me.mnuProject.Size = New System.Drawing.Size(53, 20)
        Me.mnuProject.Text = "Project"
        '
        'mnuProjectProperties
        '
        Me.mnuProjectProperties.Name = "mnuProjectProperties"
        Me.mnuProjectProperties.Size = New System.Drawing.Size(200, 22)
        Me.mnuProjectProperties.Text = "Properties..."
        '
        'mnuProjectMakefile
        '
        Me.mnuProjectMakefile.Name = "mnuProjectMakefile"
        Me.mnuProjectMakefile.Size = New System.Drawing.Size(200, 22)
        Me.mnuProjectMakefile.Text = "Generate makefile"
        '
        'mnuUpdateSimpleTypes
        '
        Me.mnuUpdateSimpleTypes.Name = "mnuUpdateSimpleTypes"
        Me.mnuUpdateSimpleTypes.Size = New System.Drawing.Size(200, 22)
        Me.mnuUpdateSimpleTypes.Text = "Update simple types..."
        '
        'mnuProjectParameters
        '
        Me.mnuProjectParameters.Name = "mnuProjectParameters"
        Me.mnuProjectParameters.Size = New System.Drawing.Size(200, 22)
        Me.mnuProjectParameters.Text = "Parameters..."
        Me.mnuProjectParameters.Visible = False
        '
        'RenumberDatabaseIndex
        '
        Me.RenumberDatabaseIndex.Name = "RenumberDatabaseIndex"
        Me.RenumberDatabaseIndex.Size = New System.Drawing.Size(200, 22)
        Me.RenumberDatabaseIndex.Text = "Renumber database index"
        Me.RenumberDatabaseIndex.Visible = False
        '
        'UpdatesCollaborations
        '
        Me.UpdatesCollaborations.Name = "UpdatesCollaborations"
        Me.UpdatesCollaborations.Size = New System.Drawing.Size(200, 22)
        Me.UpdatesCollaborations.Text = "Updates collaborations"
        Me.UpdatesCollaborations.Visible = False
        '
        'mnuPackageList
        '
        Me.mnuPackageList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPackageAddImport, Me.mnuPackageAddClass, Me.mnuPackageAddPackage, Me.ToolStripSeparator10, Me.mnuPackageEdit, Me.mnuPackageMoveUp, Me.mnuPackageProperties, Me.ToolStripSeparator11, Me.mnuPackageGenerate, Me.ToolStripSeparator26, Me.mnuPackageDependencies, Me.ToolStripSeparator21, Me.mnuPackageExportReference, Me.ToolStripSeparator24, Me.mnuPackageExportNodes, Me.mnuPackageImportNodes, Me.mnuPackageUpdateNodes, Me.ToolStripSeparator17, Me.mnuPackageRedundancies, Me.mnuPackageDelete})
        Me.mnuPackageList.Name = "mnuEditList"
        Me.mnuPackageList.Size = New System.Drawing.Size(189, 348)
        '
        'mnuPackageAddImport
        '
        Me.mnuPackageAddImport.Image = Global.ClassXmlProject.My.Resources.Resources.library
        Me.mnuPackageAddImport.Name = "mnuPackageAddImport"
        Me.mnuPackageAddImport.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.mnuPackageAddImport.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageAddImport.Tag = "import"
        Me.mnuPackageAddImport.Text = "Add import"
        '
        'mnuPackageAddClass
        '
        Me.mnuPackageAddClass.Image = Global.ClassXmlProject.My.Resources.Resources.Address_Book
        Me.mnuPackageAddClass.Name = "mnuPackageAddClass"
        Me.mnuPackageAddClass.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuPackageAddClass.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageAddClass.Tag = "class"
        Me.mnuPackageAddClass.Text = "Add class"
        '
        'mnuPackageAddPackage
        '
        Me.mnuPackageAddPackage.Image = Global.ClassXmlProject.My.Resources.Resources.my_Documents
        Me.mnuPackageAddPackage.Name = "mnuPackageAddPackage"
        Me.mnuPackageAddPackage.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuPackageAddPackage.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageAddPackage.Tag = "package"
        Me.mnuPackageAddPackage.Text = "Add package"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(185, 6)
        '
        'mnuPackageEdit
        '
        Me.mnuPackageEdit.Image = Global.ClassXmlProject.My.Resources.Resources.Rename___Edit
        Me.mnuPackageEdit.Name = "mnuPackageEdit"
        Me.mnuPackageEdit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuPackageEdit.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageEdit.Text = "Edit..."
        '
        'mnuPackageMoveUp
        '
        Me.mnuPackageMoveUp.Image = Global.ClassXmlProject.My.Resources.Resources.up
        Me.mnuPackageMoveUp.Name = "mnuPackageMoveUp"
        Me.mnuPackageMoveUp.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageMoveUp.Text = "Move up"
        '
        'mnuPackageProperties
        '
        Me.mnuPackageProperties.Name = "mnuPackageProperties"
        Me.mnuPackageProperties.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageProperties.Text = "Parameters..."
        Me.mnuPackageProperties.Visible = False
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(185, 6)
        '
        'mnuPackageGenerate
        '
        Me.mnuPackageGenerate.Image = Global.ClassXmlProject.My.Resources.Resources.Publish
        Me.mnuPackageGenerate.Name = "mnuPackageGenerate"
        Me.mnuPackageGenerate.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageGenerate.Text = "Generate code"
        '
        'ToolStripSeparator26
        '
        Me.ToolStripSeparator26.Name = "ToolStripSeparator26"
        Me.ToolStripSeparator26.Size = New System.Drawing.Size(185, 6)
        '
        'mnuPackageDependencies
        '
        Me.mnuPackageDependencies.Image = Global.ClassXmlProject.My.Resources.Resources.Search
        Me.mnuPackageDependencies.Name = "mnuPackageDependencies"
        Me.mnuPackageDependencies.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageDependencies.Text = "Search dependencies..."
        '
        'ToolStripSeparator21
        '
        Me.ToolStripSeparator21.Name = "ToolStripSeparator21"
        Me.ToolStripSeparator21.Size = New System.Drawing.Size(185, 6)
        '
        'mnuPackageExportReference
        '
        Me.mnuPackageExportReference.Image = Global.ClassXmlProject.My.Resources.Resources.Forward_2
        Me.mnuPackageExportReference.Name = "mnuPackageExportReference"
        Me.mnuPackageExportReference.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageExportReference.Text = "Export references"
        '
        'ToolStripSeparator24
        '
        Me.ToolStripSeparator24.Name = "ToolStripSeparator24"
        Me.ToolStripSeparator24.Size = New System.Drawing.Size(185, 6)
        '
        'mnuPackageExportNodes
        '
        Me.mnuPackageExportNodes.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PackageExportNodesSimpleCopy, Me.PackageExportNodesExtract})
        Me.mnuPackageExportNodes.Image = Global.ClassXmlProject.My.Resources.Resources.Forward_2
        Me.mnuPackageExportNodes.Name = "mnuPackageExportNodes"
        Me.mnuPackageExportNodes.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageExportNodes.Text = "Export nodes..."
        '
        'PackageExportNodesSimpleCopy
        '
        Me.PackageExportNodesSimpleCopy.Image = Global.ClassXmlProject.My.Resources.Resources.Copy
        Me.PackageExportNodesSimpleCopy.Name = "PackageExportNodesSimpleCopy"
        Me.PackageExportNodesSimpleCopy.Size = New System.Drawing.Size(142, 22)
        Me.PackageExportNodesSimpleCopy.Text = "Simple copy..."
        '
        'PackageExportNodesExtract
        '
        Me.PackageExportNodesExtract.Image = Global.ClassXmlProject.My.Resources.Resources.SFX1
        Me.PackageExportNodesExtract.Name = "PackageExportNodesExtract"
        Me.PackageExportNodesExtract.Size = New System.Drawing.Size(142, 22)
        Me.PackageExportNodesExtract.Text = "Extract..."
        '
        'mnuPackageImportNodes
        '
        Me.mnuPackageImportNodes.Image = Global.ClassXmlProject.My.Resources.Resources.Back_2
        Me.mnuPackageImportNodes.Name = "mnuPackageImportNodes"
        Me.mnuPackageImportNodes.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageImportNodes.Text = "Import nodes..."
        '
        'mnuPackageUpdateNodes
        '
        Me.mnuPackageUpdateNodes.Image = Global.ClassXmlProject.My.Resources.Resources.Back_2
        Me.mnuPackageUpdateNodes.Name = "mnuPackageUpdateNodes"
        Me.mnuPackageUpdateNodes.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageUpdateNodes.Text = "Merge nodes..."
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(185, 6)
        '
        'mnuPackageRedundancies
        '
        Me.mnuPackageRedundancies.Image = Global.ClassXmlProject.My.Resources.Resources._Stop
        Me.mnuPackageRedundancies.Name = "mnuPackageRedundancies"
        Me.mnuPackageRedundancies.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageRedundancies.Text = "Remove redundancy..."
        '
        'mnuPackageDelete
        '
        Me.mnuPackageDelete.Image = Global.ClassXmlProject.My.Resources.Resources.Stop_2
        Me.mnuPackageDelete.Name = "mnuPackageDelete"
        Me.mnuPackageDelete.Size = New System.Drawing.Size(188, 22)
        Me.mnuPackageDelete.Text = "Delete"
        '
        'mnuClassMembers
        '
        Me.mnuClassMembers.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddClassMember, Me.mnuClassMemberEdit, Me.mnuClassMemberProperties, Me.ToolStripSeparator12, Me.mnuOverrides, Me.ToolStripSeparator27, Me.mnuClassDependencies, Me.ToolStripSeparator22, Me.mnuClassMemberExport, Me.ToolStripSeparator19, Me.mnuClassDeleteMember})
        Me.mnuClassMembers.Name = "mnuMembers"
        Me.mnuClassMembers.Size = New System.Drawing.Size(189, 182)
        '
        'mnuAddClassMember
        '
        Me.mnuAddClassMember.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddClassImport, Me.AddClassTypedef, Me.AddClassContainer, Me.AddClassStructure, Me.AddClassProperty, Me.AddClassConstructor, Me.AddClassMethod})
        Me.mnuAddClassMember.Name = "mnuAddClassMember"
        Me.mnuAddClassMember.Size = New System.Drawing.Size(188, 22)
        Me.mnuAddClassMember.Text = "Add"
        '
        'AddClassImport
        '
        Me.AddClassImport.Image = Global.ClassXmlProject.My.Resources.Resources.library
        Me.AddClassImport.Name = "AddClassImport"
        Me.AddClassImport.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.AddClassImport.Size = New System.Drawing.Size(165, 22)
        Me.AddClassImport.Tag = "import"
        Me.AddClassImport.Text = "Import"
        '
        'AddClassTypedef
        '
        Me.AddClassTypedef.Image = Global.ClassXmlProject.My.Resources.Resources.Apps
        Me.AddClassTypedef.Name = "AddClassTypedef"
        Me.AddClassTypedef.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.AddClassTypedef.Size = New System.Drawing.Size(165, 22)
        Me.AddClassTypedef.Tag = "typedef"
        Me.AddClassTypedef.Text = "Typedef"
        '
        'AddClassContainer
        '
        Me.AddClassContainer.Image = Global.ClassXmlProject.My.Resources.Resources.Apps
        Me.AddClassContainer.Name = "AddClassContainer"
        Me.AddClassContainer.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.AddClassContainer.Size = New System.Drawing.Size(165, 22)
        Me.AddClassContainer.Tag = "container_doc"
        Me.AddClassContainer.Text = "Container"
        '
        'AddClassStructure
        '
        Me.AddClassStructure.Image = Global.ClassXmlProject.My.Resources.Resources.Apps
        Me.AddClassStructure.Name = "AddClassStructure"
        Me.AddClassStructure.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.AddClassStructure.Size = New System.Drawing.Size(165, 22)
        Me.AddClassStructure.Tag = "structure_doc"
        Me.AddClassStructure.Text = "Structure"
        '
        'AddClassProperty
        '
        Me.AddClassProperty.Image = Global.ClassXmlProject.My.Resources.Resources.Properties
        Me.AddClassProperty.Name = "AddClassProperty"
        Me.AddClassProperty.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.AddClassProperty.Size = New System.Drawing.Size(165, 22)
        Me.AddClassProperty.Tag = "property"
        Me.AddClassProperty.Text = "Property"
        '
        'AddClassConstructor
        '
        Me.AddClassConstructor.Image = Global.ClassXmlProject.My.Resources.Resources.Move
        Me.AddClassConstructor.Name = "AddClassConstructor"
        Me.AddClassConstructor.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.AddClassConstructor.Size = New System.Drawing.Size(165, 22)
        Me.AddClassConstructor.Tag = "constructor_doc"
        Me.AddClassConstructor.Text = "Constructor"
        '
        'AddClassMethod
        '
        Me.AddClassMethod.Image = Global.ClassXmlProject.My.Resources.Resources.Move
        Me.AddClassMethod.Name = "AddClassMethod"
        Me.AddClassMethod.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.AddClassMethod.Size = New System.Drawing.Size(165, 22)
        Me.AddClassMethod.Tag = "method"
        Me.AddClassMethod.Text = "Method"
        '
        'mnuClassMemberEdit
        '
        Me.mnuClassMemberEdit.Image = Global.ClassXmlProject.My.Resources.Resources.Rename___Edit
        Me.mnuClassMemberEdit.Name = "mnuClassMemberEdit"
        Me.mnuClassMemberEdit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mnuClassMemberEdit.Size = New System.Drawing.Size(188, 22)
        Me.mnuClassMemberEdit.Text = "Edit..."
        '
        'mnuClassMemberProperties
        '
        Me.mnuClassMemberProperties.Name = "mnuClassMemberProperties"
        Me.mnuClassMemberProperties.Size = New System.Drawing.Size(188, 22)
        Me.mnuClassMemberProperties.Text = "Parameters..."
        Me.mnuClassMemberProperties.Visible = False
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(185, 6)
        '
        'mnuOverrides
        '
        Me.mnuOverrides.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOverrideProperties, Me.mnuOverrideMethods})
        Me.mnuOverrides.Name = "mnuOverrides"
        Me.mnuOverrides.Size = New System.Drawing.Size(188, 22)
        Me.mnuOverrides.Text = "Overrides"
        '
        'mnuOverrideProperties
        '
        Me.mnuOverrideProperties.Image = Global.ClassXmlProject.My.Resources.Resources.Properties
        Me.mnuOverrideProperties.Name = "mnuOverrideProperties"
        Me.mnuOverrideProperties.Size = New System.Drawing.Size(135, 22)
        Me.mnuOverrideProperties.Text = "Properties..."
        '
        'mnuOverrideMethods
        '
        Me.mnuOverrideMethods.Image = Global.ClassXmlProject.My.Resources.Resources.Move
        Me.mnuOverrideMethods.Name = "mnuOverrideMethods"
        Me.mnuOverrideMethods.Size = New System.Drawing.Size(135, 22)
        Me.mnuOverrideMethods.Text = "Methods..."
        '
        'ToolStripSeparator27
        '
        Me.ToolStripSeparator27.Name = "ToolStripSeparator27"
        Me.ToolStripSeparator27.Size = New System.Drawing.Size(185, 6)
        '
        'mnuClassDependencies
        '
        Me.mnuClassDependencies.Image = Global.ClassXmlProject.My.Resources.Resources.Search
        Me.mnuClassDependencies.Name = "mnuClassDependencies"
        Me.mnuClassDependencies.Size = New System.Drawing.Size(188, 22)
        Me.mnuClassDependencies.Text = "Search dependencies..."
        '
        'ToolStripSeparator22
        '
        Me.ToolStripSeparator22.Name = "ToolStripSeparator22"
        Me.ToolStripSeparator22.Size = New System.Drawing.Size(185, 6)
        '
        'mnuClassMemberExport
        '
        Me.mnuClassMemberExport.Image = Global.ClassXmlProject.My.Resources.Resources.Forward_2
        Me.mnuClassMemberExport.Name = "mnuClassMemberExport"
        Me.mnuClassMemberExport.Size = New System.Drawing.Size(188, 22)
        Me.mnuClassMemberExport.Text = "Export references..."
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(185, 6)
        '
        'mnuClassDeleteMember
        '
        Me.mnuClassDeleteMember.Image = Global.ClassXmlProject.My.Resources.Resources.Stop_2
        Me.mnuClassDeleteMember.Name = "mnuClassDeleteMember"
        Me.mnuClassDeleteMember.Size = New System.Drawing.Size(188, 22)
        Me.mnuClassDeleteMember.Text = "Delete"
        '
        'mnuEditReference
        '
        Me.mnuEditReference.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewReference, Me.NewInterface, Me.ToolStripSeparator14, Me.EditReference, Me.ReferenceProperties, Me.ToolStripSeparator34, Me.mnuRefDependencies, Me.ToolStripSeparator13, Me.AddReferences, Me.ToolStripSeparator15, Me.FindRedundant, Me.DeleteReference, Me.RemoveAll})
        Me.mnuEditReference.Name = "ContextMenuStrip1"
        Me.mnuEditReference.Size = New System.Drawing.Size(202, 248)
        '
        'NewReference
        '
        Me.NewReference.Image = Global.ClassXmlProject.My.Resources.Resources.Address_Book
        Me.NewReference.Name = "NewReference"
        Me.NewReference.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.NewReference.Size = New System.Drawing.Size(201, 22)
        Me.NewReference.Tag = "reference"
        Me.NewReference.Text = "Add new reference"
        '
        'NewInterface
        '
        Me.NewInterface.Name = "NewInterface"
        Me.NewInterface.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.NewInterface.Size = New System.Drawing.Size(201, 22)
        Me.NewInterface.Tag = "interface"
        Me.NewInterface.Text = "Add new interface"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(198, 6)
        '
        'EditReference
        '
        Me.EditReference.Image = Global.ClassXmlProject.My.Resources.Resources.Rename___Edit
        Me.EditReference.Name = "EditReference"
        Me.EditReference.Size = New System.Drawing.Size(201, 22)
        Me.EditReference.Text = "Edit..."
        '
        'ReferenceProperties
        '
        Me.ReferenceProperties.Name = "ReferenceProperties"
        Me.ReferenceProperties.Size = New System.Drawing.Size(201, 22)
        Me.ReferenceProperties.Text = "Properties..."
        Me.ReferenceProperties.Visible = False
        '
        'ToolStripSeparator34
        '
        Me.ToolStripSeparator34.Name = "ToolStripSeparator34"
        Me.ToolStripSeparator34.Size = New System.Drawing.Size(198, 6)
        '
        'mnuRefDependencies
        '
        Me.mnuRefDependencies.Image = Global.ClassXmlProject.My.Resources.Resources.Search
        Me.mnuRefDependencies.Name = "mnuRefDependencies"
        Me.mnuRefDependencies.Size = New System.Drawing.Size(201, 22)
        Me.mnuRefDependencies.Text = "Search dependencies..."
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(198, 6)
        '
        'AddReferences
        '
        Me.AddReferences.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReplaceExport, Me.mnuMergeExport, Me.mnuConfirmExport})
        Me.AddReferences.Name = "AddReferences"
        Me.AddReferences.Size = New System.Drawing.Size(201, 22)
        Me.AddReferences.Text = "Import references"
        '
        'mnuReplaceExport
        '
        Me.mnuReplaceExport.Name = "mnuReplaceExport"
        Me.mnuReplaceExport.Size = New System.Drawing.Size(124, 22)
        Me.mnuReplaceExport.Tag = "0"
        Me.mnuReplaceExport.Text = "Replace..."
        '
        'mnuMergeExport
        '
        Me.mnuMergeExport.Name = "mnuMergeExport"
        Me.mnuMergeExport.Size = New System.Drawing.Size(124, 22)
        Me.mnuMergeExport.Tag = "1"
        Me.mnuMergeExport.Text = "Merge..."
        '
        'mnuConfirmExport
        '
        Me.mnuConfirmExport.Name = "mnuConfirmExport"
        Me.mnuConfirmExport.Size = New System.Drawing.Size(124, 22)
        Me.mnuConfirmExport.Tag = "2"
        Me.mnuConfirmExport.Text = "Confirm..."
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(198, 6)
        '
        'FindRedundant
        '
        Me.FindRedundant.Image = Global.ClassXmlProject.My.Resources.Resources._Stop
        Me.FindRedundant.Name = "FindRedundant"
        Me.FindRedundant.Size = New System.Drawing.Size(201, 22)
        Me.FindRedundant.Text = "Remove redundancy..."
        '
        'DeleteReference
        '
        Me.DeleteReference.Image = Global.ClassXmlProject.My.Resources.Resources.Stop_2
        Me.DeleteReference.Name = "DeleteReference"
        Me.DeleteReference.Size = New System.Drawing.Size(201, 22)
        Me.DeleteReference.Text = "Delete"
        '
        'RemoveAll
        '
        Me.RemoveAll.Image = Global.ClassXmlProject.My.Resources.Resources.Delete
        Me.RemoveAll.Name = "RemoveAll"
        Me.RemoveAll.Size = New System.Drawing.Size(201, 22)
        Me.RemoveAll.Text = "Remove all"
        '
        'frmProject
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(897, 635)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Controls.Add(Me.tlstrpNavigation)
        Me.Controls.Add(Me.mnuBar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmProject"
        Me.Text = "frmProject"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.mnuProjectList.ResumeLayout(False)
        Me.tlstrpNavigation.ResumeLayout(False)
        Me.tlstrpNavigation.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.mnuBar.ResumeLayout(False)
        Me.mnuBar.PerformLayout()
        Me.mnuPackageList.ResumeLayout(False)
        Me.mnuClassMembers.ResumeLayout(False)
        Me.mnuEditReference.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SmallIconList As System.Windows.Forms.ImageList
    Friend WithEvents LargeIconList As System.Windows.Forms.ImageList
    Friend WithEvents lvwProjectMembers As ClassXmlProject.XmlDataListView
    Friend WithEvents tlstrpNavigation As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents docvwProjectDisplay As ClassXmlProject.XmlDocumentView
    Friend WithEvents btnUp As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnHome As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFilePrintPreview As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilePrint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFileSaveAs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileNew As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFileSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuBar As System.Windows.Forms.MenuStrip
    Friend WithEvents EditMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RedoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuEditCut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuEditSelectAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProjectList As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAddClass As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddPackage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuProjectDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProjectEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFileClose As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuProject As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProjectProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddImport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAddRelationship As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPackageList As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuPackageAddImport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPackageAddClass As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPackageAddPackage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPackageEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPackageProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPackageDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuClassMembers As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuAddClassMember As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddClassTypedef As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddClassContainer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddClassStructure As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddClassProperty As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddClassConstructor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddClassMethod As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuClassMemberEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuClassMemberProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuClassDeleteMember As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProjectParameters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditReference As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewReference As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditReference As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FindRedundant As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteReference As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AddReferences As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ReferenceProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuProjectGenerate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPackageGenerate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AddClassImport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents mnuProjectMakefile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOverrides As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOverrideProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOverrideMethods As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator19 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuEditDuplicate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator20 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuProjectExport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator21 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPackageExportReference As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator22 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuClassMemberExport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProjectExportNodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProjectImportNodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator24 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPackageExportNodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPackageImportNodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator25 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuProjectDependencies As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator26 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPackageDependencies As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator27 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuClassDependencies As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRefDependencies As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectExportNodesSimpleCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectExportNodesExtract As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PackageExportNodesSimpleCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PackageExportNodesExtract As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReplaceExport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMergeExport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuConfirmExport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFileNewDoxygenFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuUpdateSimpleTypes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnDocView As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents DatabaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UmlViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CodeSourceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnProjectView As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents LargeIconsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DetailsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SmallIconsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ListToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPackageMoveUp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProjectUpdateNodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPackageUpdateNodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator23 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuApplyPatch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEditDatabase As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator30 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator31 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnCopy As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnCut As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnPaste As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator32 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents RenumberDatabaseIndex As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdatesCollaborations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator34 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFileNewOmgUmlFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnZoomIn As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnZoomOut As System.Windows.Forms.ToolStripButton
    Friend WithEvents NewInterface As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProjectRedundancies As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPackageRedundancies As System.Windows.Forms.ToolStripMenuItem
End Class
