Imports System
Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.UmlNodesManager
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.MenuItemCommand

Public Class XmlProjectView

#Region "Class declarations"

    Private m_Control As XmlDataListView
    Private m_xmlDocument As New XmlDocument
    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter
    Private m_xmlProperties As XmlProjectProperties = Nothing
    Private m_strFilename As String
    Private m_mnuTypedef As ToolStripItem

    Private Shared m_strCurrentFolder As String = "." + Path.DirectorySeparatorChar.ToString
#End Region

#Region "Properties"

    Public WriteOnly Property ListViewControl() As XmlDataListView
        Set(ByVal value As XmlDataListView)
            m_Control = value
        End Set
    End Property


    Public Property Updated() As Boolean
        Get
            ' This flag prevents to close project without saving
            Return Me.Properties.Updated
        End Get
        Set(ByVal value As Boolean)
            Me.Properties.Updated = value
        End Set
    End Property

    Public ReadOnly Property IsNew() As Boolean
        Get
            Return (m_strFilename = "")
        End Get
    End Property

    Public ReadOnly Property Filename() As String
        Get
            Return My.Computer.FileSystem.GetName(m_strFilename)
        End Get
    End Property

    Public ReadOnly Property Document() As XmlDocument
        Get
            Return m_xmlDocument
        End Get
    End Property

    Public Property Name() As String
        Get
            Return Properties.Name
        End Get
        Set(ByVal value As String)
            Properties.Name = value
        End Set
    End Property

    Public ReadOnly Property Properties() As XmlProjectProperties
        Get
            Return m_xmlProperties
        End Get
    End Property

#End Region

#Region "Public methods"

    Public Function Open(ByVal form As Form, Optional ByVal strFilename As String = "") As Boolean
        Try
            m_strFilename = strFilename
            If strFilename <> "" _
            Then
                Dim strTempPath As String = Application.LocalUserAppDataPath.ToString

                If LoadDocument(form, strFilename) = False Then
                    Return False
                End If

                If GetProjectPath(strFilename) = strTempPath Then
                    m_strFilename = ""
                End If
            Else
                m_xmlProperties = XmlNodeManager.GetInstance().CreateDocument("root", m_xmlDocument)

                Dim strXML As String = CreateNewProject() + m_xmlProperties.Node.OuterXml
                m_xmlDocument.LoadXml(strXML)

                m_xmlReferenceNodeCounter = New XmlReferenceNodeCounter(m_xmlDocument)

                ' After load, document reference change and old nodes must be updated
                m_xmlProperties.Node = m_xmlDocument.LastChild
                m_xmlProperties.NodeCounter = m_xmlReferenceNodeCounter
            End If
            Return True

        Catch ex As Exception
            If strFilename.Length = 0 Then
                Throw New Exception("Fails to created new project object 'XmlProjectView'", ex)
            Else
                Throw New Exception("Fails to open project '" + strFilename + "'", ex)
            End If
        End Try
        Return False
    End Function

    Public Function SaveAs(ByVal strFilename As String) As Boolean
        Try
            'm_xmlProperties.Name = GetProjectName(strFilename) ' No more save project name as filename !

            If CopyDocTypeDeclarationFile(GetProjectPath(strFilename)) = False Then
                Return False
            End If

            Dim strXML As String = "<?xml version='1.0' encoding='utf-8'?>" + vbCrLf + _
                                    GetDtdDeclaration("root") + _
                                    m_xmlProperties.Node.OuterXml

            m_xmlDocument.LoadXml(strXML)
            ' After load, document reference change and old nodes must be updated
            m_xmlProperties.Node = m_xmlDocument.LastChild
            'm_xmlProperties.Name = GetProjectName(strFilename) don't rename any more the project when "save as".
            m_strFilename = strFilename
            Save()

            ' We reload members to be sure to work with last reference of "root" node
            LoadMembers()

            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Save() As Boolean
        Dim bResult As Boolean = False
        Try
            If m_strFilename <> "" Then
                m_xmlDocument.Save(m_strFilename)
                ' Reset flag Updated after saving
                Me.Updated = False
                bResult = True
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Function SortImportReferences() As Boolean
        Dim child As XmlNode = m_xmlProperties.Document.SelectSingleNode("//import[@name='" + cstImportsToSort + "']")
        Dim import As XmlImportSpec = XmlNodeManager.GetInstance().CreateDocument(child)
        import.Tag = m_xmlProperties.Tag
        Return import.ExchangeImports()
    End Function

    Public Sub ExportOmgUmlFile(ByVal form As Form, ByVal fileName As String)
        XmlProjectTools.ExportOmgUmlFile(form, Me.Document, fileName)
    End Sub

    Public Function EditProperties() As Boolean
        If m_xmlProperties.Edit() Then
            ' We go home to be sure that language info is reflected back to all tree nodes
            m_Control.GoHome()
            Me.Updated = True
            Return True
        End If
        Return False
    End Function

    Public Function EditParameters() As Boolean
        dlgXmlNodeProperties.DisplayProperties(m_xmlProperties)
        ' We go home to be sure that language info is reflected back to all tree nodes
        m_Control.GoHome()
        Me.Updated = True
        Return True
    End Function

    Public Sub UpdatesCollaborations()
        XmlProjectTools.UpdatesCollaborations(m_xmlDocument)
    End Sub

    Public Sub RenumberDatabaseIndex()
        Try
            RenumberProject(Me.Properties.Node, True)
            m_xmlReferenceNodeCounter.InitItemCounters(Me.Properties.Node.OwnerDocument)
            Me.Updated = True

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub GenerateMakefile(ByVal fen As Form)
        'UmlMakeGenerator.Generate(fen, m_xmlProperties)
        MsgBox("Not yet implemented !", MsgBoxStyle.Exclamation)
    End Sub

    Public Function ExportNodesExtract(ByVal fen As Form, ByVal component As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            bResult = ExportNodes(fen, component, True)
            ' Set flag Updated to prevent to close project without saving
            If bResult Then
                Me.Updated = True
                m_Control.Binding.ResetBindings(True)
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function ExportNodes(ByVal fen As Form, ByVal component As XmlComponent, _
                                Optional ByVal bExtractReferences As Boolean = False) As Boolean
        Dim bResult As Boolean = False
        Try

            If bExtractReferences Then
                If MsgBox("Nodes will be exported and current converted as imports. Do you confirm ?", _
                          cstMsgYesNoQuestion, "'Export' command") _
                          = MsgBoxResult.No _
                Then
                    Return False
                End If
            End If

            Dim nodeXml As XmlNode = component.Node
            Dim dlgSaveFile As New SaveFileDialog

            If My.Settings.ExportFolder = m_strCurrentFolder Then
                dlgSaveFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgSaveFile.InitialDirectory = My.Settings.ExportFolder
            End If

            Dim strFilename As String = GetName(nodeXml)
            If XmlProjectTools.GetValidFilename(strFilename) Then
                MsgBox("The filename was not valid, we propose to rename:" + vbCrLf + strFilename, "Rename file")
            End If
            dlgSaveFile.FileName = strFilename
            dlgSaveFile.Filter = "UML project (*.xprj)|*.xprj"


            If dlgSaveFile.ShowDialog() = DialogResult.OK Then

                UpdateCurrentImportFolder(dlgSaveFile.FileName, dlgSaveFile.InitialDirectory)

                Dim eLang As ELanguage = CType(Me.Properties.GenerationLanguage, ELanguage)
                Dim strFullPackage As String

                Select Case nodeXml.Name
                    Case "root"
                        MsgBox("Needless to export a whole project", MsgBoxStyle.Exclamation, "'Export' command")

                    Case "package"
                        strFullPackage = GetFullpathPackage(nodeXml, eLang)
                        UmlNodesManager.ExportNodes(fen, nodeXml, dlgSaveFile.FileName, strFullPackage, eLang)
                        If bExtractReferences Then
                            strFullPackage = GetFullpathPackage(nodeXml, eLang, GetName(nodeXml))
                            bResult = UmlNodesManager.ExtractReferences(fen, nodeXml, strFullPackage, eLang)
                        End If

                    Case "class"
                        strFullPackage = GetFullpathPackage(nodeXml, eLang)
                        UmlNodesManager.ExportNodes(fen, nodeXml, dlgSaveFile.FileName, strFullPackage, eLang)
                        If bExtractReferences Then
                            bResult = UmlNodesManager.ExtractReferences(fen, nodeXml, strFullPackage, eLang)
                        End If

                    Case Else
                        MsgBox("Can't export this node", MsgBoxStyle.Exclamation, "'Export' command")
                End Select
            End If
            ' Set flat Updated to prevent to close project without saving
            If bResult And bExtractReferences Then Me.Updated = True
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function ImportNodes(ByVal form As Form, ByVal component As XmlComponent, Optional ByVal bUpdateOnly As Boolean = False) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim nodeXml As XmlNode = component.Node
            Dim dlgOpenFile As New OpenFileDialog

            If My.Settings.ImportFolder = m_strCurrentFolder Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.ImportFolder
            End If

            dlgOpenFile.Title = "Select a project file..."
            dlgOpenFile.Filter = "UML project (*.xprj)|*.xprj"
            dlgOpenFile.FileName = GetName(nodeXml)

            If dlgOpenFile.ShowDialog() = DialogResult.OK Then

                UpdateCurrentImportFolder(dlgOpenFile.FileName, dlgOpenFile.InitialDirectory)

                bResult = UmlNodesManager.ImportNodes(form, component, dlgOpenFile.FileName, m_xmlReferenceNodeCounter, bUpdateOnly)
                If bResult Then
                    m_Control.Binding.ResetBindings(True)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function ImportReferences(ByVal fen As Form, ByVal component As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim nodeXml As XmlNode = component.Node
            Dim dlgOpenFile As New OpenFileDialog

            If My.Settings.ImportFolder = m_strCurrentFolder Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.ImportFolder
            End If

            dlgOpenFile.Title = "Select a package references file..."
            dlgOpenFile.Filter = "Package references (*.ximp)|*.ximp|Doxygen TAG file (*.tag)|*.tag"

            If dlgOpenFile.ShowDialog() = DialogResult.OK Then

                UpdateCurrentImportFolder(dlgOpenFile.FileName, dlgOpenFile.InitialDirectory)

                Dim member As XmlProjectMemberView = CType(component, XmlProjectMemberView)

                bResult = member.ImportReferences(fen, dlgOpenFile.FileName)

                If bResult Then
                    Me.Properties.Updated = True
                    m_Control.Binding.ResetBindings(True)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Sub ExportReferences(ByVal fen As Form, ByVal component As XmlComponent)
        Try
            Dim nodeXml As XmlNode = component.Node
            Dim dlgSaveFile As New SaveFileDialog
            Dim strFullPackage As String

            If My.Settings.ExportFolder = m_strCurrentFolder Then
                dlgSaveFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgSaveFile.InitialDirectory = My.Settings.ExportFolder
            End If

            Dim strFilename As String = GetName(nodeXml)
            If XmlProjectTools.GetValidFilename(strFilename) Then
                MsgBox("The filename was not valid, we propose to rename:" + vbCrLf + strFilename, "Rename file")
            End If
            dlgSaveFile.FileName = strFilename
            dlgSaveFile.Filter = "Package references (*.ximp)|*.ximp"

            If dlgSaveFile.ShowDialog() = DialogResult.OK Then

                strFilename = dlgSaveFile.FileName
                If strFilename.EndsWith(".ximp") = False Then
                    strFilename += ".ximp"
                End If

                UpdateCurrentImportFolder(strFilename, dlgSaveFile.InitialDirectory)

                Dim eLang As ELanguage = CType(Me.Properties.GenerationLanguage, ELanguage)

                Select Case nodeXml.Name
                    Case "root"
                        ExportRootReferences(fen, nodeXml, strFilename, eLang)

                    Case "package"
                        strFullPackage = GetFullpathPackage(nodeXml, eLang)

                        If SelectNodes(nodeXml, "descendant::import").Count > 0 Then
                            MsgBox("Import members will not be exported", vbExclamation, "'Export' command")
                        End If
                        If SelectNodes(nodeXml, "descendant::class[@visibility='package']").Count > 0 Then
                            ExportPackageReferences(fen, nodeXml, strFilename, strFullPackage, eLang)
                        Else
                            MsgBox("Class " + GetName(nodeXml) + " has no class members with package visibility", vbExclamation, "'Export' command")
                        End If

                    Case "class"
                        strFullPackage = GetFullpathPackage(nodeXml, eLang)

                        If GetNodeString(nodeXml, "@visibility") = "package" Then
                            ExportClassReferences(fen, nodeXml, strFilename, strFullPackage, eLang)
                        Else
                            MsgBox("Class " + GetName(nodeXml) + " has not a package visibility", vbExclamation, "'Export' command")
                        End If
                    Case "typedef"
                        strFullPackage = GetFullpathPackage(nodeXml.ParentNode, eLang)

                        If GetNodeString(nodeXml.ParentNode, "@visibility") = "package" Then
                            If GetNodeString(nodeXml, "variable/@range") = "public" Then
                                ExportTypedefReferences(fen, nodeXml, strFilename, strFullPackage)
                            Else
                                MsgBox("Typedef " + GetName(nodeXml) + " is not public", vbExclamation, "'Export' command")
                            End If
                        Else
                            MsgBox("Class " + GetName(nodeXml.ParentNode) + " has not a package visibility", vbExclamation, "'Export' command")
                        End If

                    Case "import"
                        If nodeXml.HasChildNodes = True Then
                            ReExport(fen, nodeXml.LastChild, strFilename, GetAttributeValue(nodeXml, "param"))
                        Else
                            MsgBox("Import " + GetName(nodeXml) + ", nothing to export", MsgBoxStyle.Exclamation, "'Export' command")
                        End If
                End Select
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GenerateExternalTool(ByVal component As XmlComponent, ByVal item As MenuItemNode, ByVal fen As Form, _
                                            ByRef strTransformation As String) As Boolean
        Dim bResult As Boolean = False
        Try
            Select Case component.NodeName

                Case "class"
                    bResult = UmlCodeGenerator.GenerateExternalTool(fen, component.Node, GetID(component.Node), "", _
                                                          CType(Me.m_xmlProperties.GenerationLanguage, ELanguage), _
                                                          Me.m_xmlProperties.GenerationFolder, _
                                                          strTransformation, item)
                Case "package"
                    bResult = UmlCodeGenerator.GenerateExternalTool(fen, component.Node, "", GetID(component.Node), _
                                                          CType(Me.m_xmlProperties.GenerationLanguage, ELanguage), _
                                                          Me.m_xmlProperties.GenerationFolder, _
                                                          strTransformation, item)
                Case "root"
                    bResult = UmlCodeGenerator.GenerateExternalTool(fen, component.Node, "", "", CType(Me.m_xmlProperties.GenerationLanguage, ELanguage), _
                                                          Me.m_xmlProperties.GenerationFolder, strTransformation, item)
                Case Else
                    Throw New Exception("Argument " + component.ToString + " is not compatible with code generation")
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function GenerateCode(ByVal component As XmlComponent, ByVal fen As Form, _
                                            ByRef strTransformation As String) As Boolean
        Dim bResult As Boolean = False
        Try
            Select Case component.NodeName

                Case "class"
                    bResult = UmlCodeGenerator.Generate(fen, component.Node, GetID(component.Node), "", _
                                                          CType(Me.m_xmlProperties.GenerationLanguage, ELanguage), _
                                                          Me.m_xmlProperties.GenerationFolder, _
                                                          strTransformation)
                Case "package"
                    bResult = UmlCodeGenerator.Generate(fen, component.Node, "", GetID(component.Node), _
                                                          CType(Me.m_xmlProperties.GenerationLanguage, ELanguage), _
                                                          Me.m_xmlProperties.GenerationFolder, _
                                                          strTransformation)
                Case "root"
                    bResult = UmlCodeGenerator.Generate(fen, component.Node, "", "", CType(Me.m_xmlProperties.GenerationLanguage, ELanguage), _
                                                          Me.m_xmlProperties.GenerationFolder, strTransformation)
                Case Else
                    Throw New Exception("Argument " + component.ToString + " is not compatible with code generation")
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function SearchDependencies(ByVal component As XmlComponent) As Boolean
        Try
            If component Is Nothing Then Return False

            Dim bIsEmpty As Boolean = False
            If dlgDependencies.ShowDependencies(m_xmlReferenceNodeCounter, component, bIsEmpty) _
            Then
                Me.Updated = True
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Public Sub LoadMembers()
        Try
            m_Control.Binding.NodeCounter = m_xmlReferenceNodeCounter
            Dim xmlcpnt As XmlComposite = CType(Me.Properties, XmlComposite)
            m_Control.Binding.LoadItems(xmlcpnt, "project_member_view", "import")
            m_Control.CurrentContext = "project"
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub AddMenuProject(ByVal menuStrip As ContextMenuStrip)
        m_Control.AddContext("project", menuStrip, Windows.Forms.View.List)
    End Sub

    Public Sub AddMenuPackage(ByVal menuStrip As ContextMenuStrip)
        m_Control.AddContext("package", menuStrip, Windows.Forms.View.LargeIcon)
    End Sub

    Public Sub AddMenuClass(ByVal menuStrip As ContextMenuStrip)
        m_Control.AddContext("class", menuStrip, Windows.Forms.View.Details)
    End Sub

    Public Sub AddMenuImport(ByVal menuStrip As ContextMenuStrip)
        m_Control.AddContext("import", menuStrip, Windows.Forms.View.LargeIcon)
    End Sub

    Public Sub UpdateMenuClass(ByVal parent As XmlComposite, ByVal mnuTypedef As ToolStripItem, ByVal mnuConstructor As ToolStripItem)

        If CType(Me.Properties.GenerationLanguage, ELanguage) <> ELanguage.Language_Vbasic _
        Then
            mnuTypedef.Text = "Typedef"
        Else
            mnuTypedef.Text = "Enumeration"
        End If

        If parent.NodeName = "class" _
        Then
            If ConvertDtdToEnumImpl(parent.GetAttribute("implementation")) = EImplementation.Interf _
            Then
                mnuConstructor.Visible = False
            Else
                mnuConstructor.Visible = True
            End If
        End If
    End Sub

    Public Function AddReferences(ByVal form As Form, ByVal composite As XmlComposite, ByVal eMode As XmlImportSpec.EImportMode) As Boolean

        If composite IsNot Nothing Then
            Dim xmlcpnt As XmlImportView = XmlNodeManager.GetInstance().CreateView(composite.Node, "import")
            xmlcpnt.Tag = composite.Tag
            xmlcpnt.NodeCounter = m_xmlReferenceNodeCounter
            If xmlcpnt.AddReferences(form, eMode) Then
                Me.Updated = True
                m_Control.Binding.ResetBindings(True)
                m_Control.SelectItem(0)
                Return True
            End If
        End If
        Return False
    End Function

    Public Function RemoveRedundantReference(ByVal parent As XmlComposite, ByVal reference As XmlComponent) As Boolean
        If parent IsNot Nothing And reference IsNot Nothing Then
            Dim xmlcpnt As XmlComponent = XmlNodeManager.GetInstance().CreateDocument(parent.Node)
            xmlcpnt.Tag = parent.Tag
            If xmlcpnt.RemoveRedundant(reference) Then
                m_Control.Binding.ResetBindings(True)
                Me.Updated = True
                Return True
            End If
        End If
        Return False
    End Function

    Public Function MoveUpNode(ByVal parent As XmlProjectMemberView, ByVal child As XmlComponent) As Boolean
        If parent IsNot Nothing And child IsNot Nothing Then
            Select Case child.NodeName
                Case "package", "class", "import", "reference", "interface"
                    If parent.MoveUpComponent(child) Then
                        m_Control.Binding.ResetBindings(True)
                        Me.Updated = True
                        Return True
                    End If
            End Select
        End If
        Return False
    End Function

    Public Function RemoveAllReferences(ByVal composite As XmlComposite) As Boolean
        If composite IsNot Nothing Then
            Dim xmlcpnt As XmlImportView = XmlNodeManager.GetInstance().CreateView(composite.Node, "import")
            xmlcpnt.Tag = composite.Tag
            xmlcpnt.NodeCounter = m_xmlReferenceNodeCounter
            If xmlcpnt.RemoveAllReferences() Then
                m_Control.Binding.ResetBindings(True)
                Me.Updated = True
                Return True
            End If
        End If
        Return False
    End Function

    Public Function OverrideProperties(ByVal composite As XmlComposite) As Boolean
        If composite.NodeName = "class" Then
            Dim xmlcpnt As XmlClassSpec = XmlNodeManager.GetInstance().CreateDocument(composite.Node)
            xmlcpnt.Tag = Me.Properties.Tag

            If xmlcpnt.OverrideProperties(xmlcpnt.Implementation) Then
                Me.Updated = True
                m_Control.Binding.ResetBindings(True)
                Return True
            Else
                MsgBox("No properties to override!", MsgBoxStyle.Exclamation, "'Override' command")
            End If
        End If
        Return False
    End Function

    Public Function OverrideMethods(ByVal composite As XmlComposite) As Boolean
        If composite.NodeName = "class" Then
            Dim xmlcpnt As XmlClassSpec = XmlNodeManager.GetInstance().CreateDocument(composite.Node)
            xmlcpnt.Tag = Me.Properties.Tag

            If xmlcpnt.OverrideMethods(xmlcpnt.Implementation) Then
                Me.Updated = True
                m_Control.Binding.ResetBindings(True)
                Return True
            Else
                MsgBox("No methods to override!", MsgBoxStyle.Exclamation, "'Override' command")
            End If
        End If
        Return False
    End Function

    Public Sub UpdatePrefixNames()
        UmlNodesManager.UpdatePrefixNames()
    End Sub

    Public Sub UpdateSimpleTypes()
        Dim eLang As ELanguage = CType(Me.Properties.GenerationLanguage, ELanguage)
        UmlNodesManager.UpdateSimpleTypes(eLang)
    End Sub

    Public Sub TrimComments()
        m_xmlProperties.TrimComments()
        Me.Updated = True
    End Sub

    Public Sub ConvertImportElement(ByVal composite As XmlComposite, ByVal child As XmlComponent)
        Dim parent As XmlImportSpec = TryCast(XmlNodeManager.GetInstance().CreateDocument(composite.Node), XmlImportSpec)
        parent.Tag = composite.Tag
        If parent IsNot Nothing Then
            parent.ConvertComponent(child)
            m_Control.Binding.ResetBindings(True)
            Me.Updated = True
        End If
    End Sub

    Public Function ExchangeImports(ByVal composite As XmlComposite) As Boolean
        Dim parent As XmlImportSpec = TryCast(XmlNodeManager.GetInstance().CreateDocument(composite.Node), XmlImportSpec)
        If parent Is Nothing Then
            MsgBox("Please retry with an 'import' object'!", MsgBoxStyle.Critical, "Exchange imports")
        Else
            parent.Tag = composite.Tag
            If parent IsNot Nothing Then
                If parent.ExchangeImports() Then
                    Me.Updated = True
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Public Shared Function InitPrototypes() As Boolean
        Try
            ' Use XmlNodeManager.CreateDocument to clone this objects
            With XmlNodeManager.GetInstance()
                .AddDocument("father", New XmlRelationParentSpec)
                .AddDocument("child", New XmlRelationParentSpec)
                .AddDocument("body", New XmlCodeInline)
                .AddDocument("class", New XmlClassSpec)
                .AddDocument("collaboration", New XmlCollaborationSpec)
                .AddDocument("dependency", New XmlDependencySpec)
                .AddDocument("element", New XmlElementSpec)
                .AddDocument("enumvalue", New XmlEnumSpec)
                .AddDocument("import", New XmlImportSpec)
                .AddDocument("export", New XmlExportSpec)
                .AddDocument("inherited", New XmlInheritSpec)
                .AddDocument("method", New XmlMethodSpec)
                .AddDocument("package", New XmlPackageSpec)
                .AddDocument("param", New XmlParamSpec)
                .AddDocument("property", New XmlPropertySpec)
                .AddDocument("reference", New XmlReferenceSpec)
                .AddDocument("interface", New XmlInterfaceSpec)
                .AddDocument("relationship", New XmlRelationSpec)
                .AddDocument("root", New XmlProjectProperties)
                .AddDocument("type", New XmlTypeVarSpec)
                .AddDocument("typedef", New XmlTypedefSpec)
                .AddDocument("variable", New XmlVariableSpec)
                .AddDocument("exception", New XmlExceptionSpec)

                ' Use XmlNodeManager.CreateDocument to clone this objects
                .AddDocument("container_doc", New XmlContainerSpec)
                .AddDocument("structure_doc", New XmlStructureSpec)
                .AddDocument("constructor_doc", New XmlConstructorSpec)

                ' Use XmlNodeManager.CreateView/XmlNodeManager.GetView to create/get this objects
                .AddView("root", New XmlProjectPropertiesView)
                .AddView("package", New XmlPackageView)
                .AddView("reference", New XmlReferenceView)
                .AddView("interface", New XmlInterfaceView)
                .AddView("typedef", New XmlTypedefView)
                .AddView("property", New XmlPropertyView)
                .AddView("method", New XmlMethodView)
                .AddView("type", New XmlTypeView)
                .AddView("body", New XmlInlineView)
                .AddView("collaboration", New XmlClassRelationView)
                .AddView("class", New XmlClassGlobalView)
                .AddView("import", New XmlImportView)
                .AddView("element", New XmlElementView)
                .AddView("exception", New XmlMethodExceptionView)
                .AddView("param", New XmlParamView)
                .AddView("relationship", New XmlRelationView)
                .AddView("father", New XmlRelationParentView)
                .AddView("child", New XmlRelationParentView)
                .AddView("constructor_view", New XmlConstructorView)
                .AddView("container_view", New XmlContainerView)
                .AddView("structure_view", New XmlStructureView)

                ' Use XmlNodeManager.CreateView/XmlNodeManager.GetView to create/get this objects
                .AddView("class_superclass_view", New XmlSuperClassView)
                .AddView("package_class_view", New XmlPackageMemberView)
                .AddView("class_relation_view", New XmlClassRelationView)
                .AddView("class_dependency_view", New XmlClassDependencyView)
                .AddView("class_member_view", New XmlClassMemberView)
                .AddView("interface_member_view", New XmlInterfaceMemberView)
                .AddView("method_member_view", New XmlMethodMemberView)
                .AddView("class_inherited_view", New XmlClassInheritedView)
                .AddView("type_enumvalue_view", New XmlEnumView)
                .AddView("type_element_view", New XmlElementView)
                .AddView("method_exception_view", New XmlMethodExceptionView)
                .AddView("project_member_view", New XmlProjectMemberView)
                .AddView("class_override_properties", New XmlClassOverridePropertiesView)
                .AddView("class_override_methods", New XmlClassOverrideMethodsView)
                .AddView("reference_redundancy", New XmlRefRedundancyView)
                .AddView("import_exchange", New XmlExchangeImportsView)
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
            Return False
        End Try
        Return True
    End Function
#End Region

#Region "Private methods"

    Private Function LoadDocument(ByVal form As Form, ByVal strFilename As String) As Boolean
        Try
            Dim bChanged As Boolean = False

            Select Case XmlProjectTools.LoadDocument(form, m_xmlDocument, strFilename)
                Case EResult.Completed
                    ' Ok, nohting to do

                Case EResult.Failed
                    Return False

                Case EResult.Converted
                    bChanged = True
            End Select

            If m_xmlDocument.DocumentElement IsNot Nothing _
            Then
                m_xmlProperties = XmlNodeManager.GetInstance().CreateDocument(m_xmlDocument.DocumentElement)

                m_xmlProperties.Updated = bChanged
                m_xmlProperties.Tag = m_xmlProperties.GenerationLanguage

                Me.Updated = bChanged
                m_xmlReferenceNodeCounter = New XmlReferenceNodeCounter(m_xmlDocument)
                m_xmlProperties.NodeCounter = m_xmlReferenceNodeCounter
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Private Function GetProjectName(ByVal strFullpathFilename As String) As String
        Dim strProjectName As String = Path.GetFileNameWithoutExtension(strFullpathFilename)
        Return strProjectName
    End Function

    Private Sub UpdateCurrentImportFolder(ByVal strFullpathFilename As String, ByVal strDefault As String)
        Dim i As Integer = InStr(strFullpathFilename, Path.DirectorySeparatorChar.ToString)

        If i > 0 Then
            My.Settings.ImportFolder = Path.GetDirectoryName(strFullpathFilename)
        Else
            My.Settings.ImportFolder = strDefault
        End If
    End Sub

#End Region
End Class
