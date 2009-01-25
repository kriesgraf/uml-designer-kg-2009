Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.UmlNodesManager
Imports ClassXmlProject.XmlProjectTools
Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Xml.Schema
Imports Microsoft.VisualBasic

Public Class XmlProjectView

#Region "Class declarations"

    Private m_xmlDocument As XmlDocument
    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter
    Private m_xmlProperties As XmlProjectProperties
    Private m_strFilename As String
    Private m_mnuTypedef As ToolStripItem
    Private m_bUpdated As Boolean = False

    Private Shared m_bPrototypesLoaded As Boolean = InitPrototypes()

#End Region

#Region "Properties"

    Public Property Updated() As Boolean
        Get
            ' This flag prevents to close project without saving
            Return m_bUpdated
        End Get
        Set(ByVal value As Boolean)
            m_bUpdated = value
        End Set
    End Property

    Public ReadOnly Property IsNew() As Boolean
        Get
            Return (m_strFilename = "")
        End Get
    End Property

    Public ReadOnly Property Filename() As String
        Get
            Return m_strFilename
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

    Public Sub New(Optional ByVal strFilename As String = "")

        m_strFilename = strFilename
        m_xmlDocument = New XmlDocument

        If strFilename <> "" _
        Then
            Dim strTempPath As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData
            LoadDocument(strFilename)
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
        End If
    End Sub

    Public Function SaveAs(ByVal strFilename As String) As Boolean
        Try
            m_xmlProperties.Name = GetProjectName(strFilename)

            If CopyDocTypeDeclarationFile(GetProjectPath(strFilename)) = False Then
                Return False
            End If

            Dim strXML As String = "<?xml version='1.0' encoding='utf-8'?>" + vbCrLf + _
                                    GetDtdDeclaration("root") + _
                                    m_xmlProperties.Node.OuterXml

            m_xmlDocument.LoadXml(strXML)
            ' After load, document reference change and old nodes must be updated
            m_xmlProperties.Node = m_xmlDocument.LastChild
            m_xmlProperties.Name = ExtractName(strFilename)
            m_strFilename = strFilename
            Save()
            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ExtractName(ByVal strFilename As String) As String
        Dim slashPosition As Integer = strFilename.LastIndexOf("\")
        Dim filenameOnly As String = strFilename.Substring(slashPosition + 1)
        Return filenameOnly.Substring(0, filenameOnly.LastIndexOf("."))
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

    Public Sub UpdatesCollaborations()
        XmlProjectTools.UpdatesCollaborations(m_xmlDocument)
    End Sub

    Public Sub RenumberDatabaseIndex()
        Try
            RenumberProject(Me.Properties.Node)
            m_xmlReferenceNodeCounter.InitItemCounters(Me.Properties.Node.OwnerDocument)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub GenerateMakefile(ByVal fen As Form)
        'UmlMakeGenerator.Generate(fen, m_xmlProperties)
        MsgBox("Not yet implemented !", MsgBoxStyle.Exclamation)
    End Sub

    Public Function ExportNodesExtract(ByVal component As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            bResult = ExportNodes(component, True)
            ' Set flag Updated to prevent to close project without saving
            If bResult Then Me.Updated = True

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function ExportNodes(ByVal component As XmlComponent, Optional ByVal bExtractReferences As Boolean = False) As Boolean
        Dim bResult As Boolean = False
        Try

            If bExtractReferences Then
                If MsgBox("Nodes will be exported and current converted as imports. Do you confirm ?", _
                          cstMsgYesNoQuestion) _
                          = MsgBoxResult.No _
                Then
                    Return False
                End If
            End If

            Dim node As XmlNode = component.Node
            Dim dlgSaveFile As New SaveFileDialog

            If My.Settings.ImportFolder = ".\" Then
                dlgSaveFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgSaveFile.InitialDirectory = My.Settings.ImportFolder
            End If

            dlgSaveFile.Filter = "UML project (*.xprj)|*.xprj"
            dlgSaveFile.FileName = GetName(node)

            If dlgSaveFile.ShowDialog() = DialogResult.OK Then

                Dim FileName As String = dlgSaveFile.FileName
                Dim i As Integer = InStrRev(FileName, "\")

                If i > 0 Then
                    My.Settings.ImportFolder = Strings.Left(FileName, i - 1)
                Else
                    My.Settings.ImportFolder = dlgSaveFile.InitialDirectory
                End If

                Dim strFullPackage As String

                Select Case node.Name
                    Case "root"
                        MsgBox("Needless to export a whole project", MsgBoxStyle.Exclamation)

                    Case "package"
                        strFullPackage = GetFullpathPackage(node, Me.Properties.GenerationLanguage)
                        UmlNodesManager.ExportNodes(node, dlgSaveFile.FileName, strFullPackage)
                        If bExtractReferences Then
                            strFullPackage = GetFullpathPackage(node, _
                                                                                Me.Properties.GenerationLanguage, _
                                                                                GetName(node))
                            bResult = UmlNodesManager.ExtractReferences(node, strFullPackage, _
                                                                        GetSeparator(Me.Properties.GenerationLanguage))
                        End If

                    Case "class"
                        strFullPackage = GetFullpathPackage(node, Me.Properties.GenerationLanguage)
                        UmlNodesManager.ExportNodes(node, dlgSaveFile.FileName, strFullPackage)
                        If bExtractReferences Then
                            bResult = UmlNodesManager.ExtractReferences(node, strFullPackage, _
                                                                        GetSeparator(Me.Properties.GenerationLanguage))
                        End If

                    Case Else
                        MsgBox("Can't export this node", MsgBoxStyle.Exclamation)
                End Select
            End If
            ' Set flat Updated to prevent to close project without saving
            If bResult And bExtractReferences Then Me.Updated = True
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function ImportNodes(ByVal component As XmlComponent, Optional ByVal bUpdateOnly As Boolean = False) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim node As XmlNode = component.Node
            Dim dlgOpenFile As New OpenFileDialog

            If My.Settings.ImportFolder = ".\" Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.ImportFolder
            End If

            dlgOpenFile.Title = "Select a project file..."
            dlgOpenFile.Filter = "UML project (*.xprj)|*.xprj"
            dlgOpenFile.FileName = GetName(node)

            If dlgOpenFile.ShowDialog() = DialogResult.OK Then

                Dim FileName As String = dlgOpenFile.FileName
                Dim i As Integer = InStrRev(FileName, "\")

                If i > 0 Then
                    My.Settings.ImportFolder = Strings.Left(FileName, i - 1)
                Else
                    My.Settings.ImportFolder = dlgOpenFile.InitialDirectory
                End If

                bResult = UmlNodesManager.ImportNodes(component, FileName, m_xmlReferenceNodeCounter, bUpdateOnly)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Sub ExportReferences(ByVal component As XmlComponent)
        Try
            Dim node As XmlNode = component.Node
            Dim dlgSaveFile As New SaveFileDialog
            Dim strFullPackage As String

            If My.Settings.ImportFolder = ".\" Then
                dlgSaveFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgSaveFile.InitialDirectory = My.Settings.ImportFolder
            End If

            dlgSaveFile.Filter = "Package references (*.ximp)|*.ximp"
            dlgSaveFile.FileName = GetName(node)

            If dlgSaveFile.ShowDialog() = DialogResult.OK Then

                Dim FileName As String = dlgSaveFile.FileName
                If FileName.EndsWith(".ximp") = False Then
                    FileName += ".ximp"
                End If
                Dim i As Integer = InStrRev(FileName, "\")

                If i > 0 Then
                    My.Settings.ImportFolder = Strings.Left(FileName, i - 1)
                Else
                    My.Settings.ImportFolder = dlgSaveFile.InitialDirectory
                End If

                Select Case node.Name
                    Case "root"
                        strFullPackage = GetName(node)
                        ExportPackageReferences(node, FileName, strFullPackage, _
                                                         GetSeparator(Me.Properties.GenerationLanguage))

                    Case "package"
                        strFullPackage = GetFullpathPackage(node, Me.Properties.GenerationLanguage)

                        If SelectNodes(node, "descendant::import").Count > 0 Then
                            MsgBox("Import members will not be exported", vbExclamation)
                        End If
                        If SelectNodes(node, "descendant::class[@visibility='package']").Count > 0 Then
                            ExportPackageReferences(node, FileName, strFullPackage, _
                                                         GetSeparator(Me.Properties.GenerationLanguage))
                        Else
                            MsgBox("Class " + GetName(node) + " has no class members with package visibility", vbExclamation)
                        End If

                    Case "class"
                        strFullPackage = GetFullpathPackage(node, Me.Properties.GenerationLanguage)

                        If GetNodeString(node, "@visibility") = "package" Then
                            ExportClassReferences(node, FileName, strFullPackage)
                        Else
                            MsgBox("Class " + GetName(node) + " has not a package visibility", vbExclamation)
                        End If
                    Case "typedef"
                        strFullPackage = GetFullpathPackage(node.ParentNode, Me.Properties.GenerationLanguage)

                        If GetNodeString(node.ParentNode, "@visibility") = "package" Then
                            If GetNodeString(node, "variable/@range") = "public" Then
                                ExportTypedefReferences(node, FileName, strFullPackage)
                            Else
                                MsgBox("Typedef " + GetName(node) + " is not public", vbExclamation)
                            End If
                        Else
                            MsgBox("Class " + GetName(node.ParentNode) + " has not a package visibility", vbExclamation)
                        End If

                    Case "import"
                        If node.HasChildNodes = True Then
                            ReExport(node.LastChild, FileName, GetAttributeValue(node, "param"))
                        Else
                            MsgBox("Import " + GetName(node) + ", nothing to export", MsgBoxStyle.Exclamation)
                        End If
                End Select
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GenerateCode(ByVal component As XmlComponent, ByVal fen As Form, _
                                            ByRef strTransformation As String) As Boolean
        Dim bResult As Boolean = False

        Select Case component.NodeName

            Case "class"
                bResult = UmlCodeGenerator.Generate(fen, component.Node, GetID(component.Node), "", _
                                                      Me.m_xmlProperties.GenerationLanguage, _
                                                      Me.m_xmlProperties.GenerationFolder, _
                                                      strTransformation)
            Case "package"
                bResult = UmlCodeGenerator.Generate(fen, component.Node, "", GetID(component.Node), _
                                                      Me.m_xmlProperties.GenerationLanguage, _
                                                      Me.m_xmlProperties.GenerationFolder, _
                                                      strTransformation)
            Case "root"
                bResult = UmlCodeGenerator.Generate(fen, component.Node, "", "", Me.m_xmlProperties.GenerationLanguage, _
                                                      Me.m_xmlProperties.GenerationFolder, strTransformation)
            Case Else
                Throw New Exception("Argument " + component.ToString + " is not compatible with code generation")
        End Select
        Return bResult
    End Function

    Public Sub LoadMembers(ByVal control As XmlDataListView)
        Try
            control.Binding.NodeCounter = m_xmlReferenceNodeCounter
            Dim xmlcpnt As XmlProjectMemberView = New XmlProjectMemberView(Me.Properties.Node)
            control.Binding.LoadItems(xmlcpnt, "project_member_view", "import")
            control.CurrentContext = "project"
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub AddMenuProject(ByVal control As XmlDataListView, ByVal menuStrip As ContextMenuStrip)
        control.AddContext("project", menuStrip, View.List)
    End Sub

    Public Sub AddMenuPackage(ByVal control As XmlDataListView, ByVal menuStrip As ContextMenuStrip)
        control.AddContext("package", menuStrip, View.LargeIcon)
    End Sub

    Public Sub AddMenuClass(ByVal control As XmlDataListView, ByVal menuStrip As ContextMenuStrip)
        control.AddContext("class", menuStrip, View.Details)
    End Sub

    Public Sub UpdateMenuClass(ByVal item1 As ToolStripItem)
        If Me.Properties.GenerationLanguage <> ELanguage.Language_Vbasic Then
            item1.Text = "Typedef"
        Else
            item1.Text = "Enumeration"
        End If
    End Sub

    Public Sub AddMenuImport(ByVal control As XmlDataListView, ByVal menuStrip As ContextMenuStrip)
        control.AddContext("import", menuStrip, View.LargeIcon)
    End Sub

    Public Function AddReferences(ByVal composite As XmlComposite, ByVal eMode As XmlImportSpec.EImportMode) As Boolean
        Dim xmlcpnt As XmlImportView = XmlNodeManager.GetInstance().CreateView(composite.Node, "import")
        xmlcpnt.NodeCounter = m_xmlReferenceNodeCounter
        If xmlcpnt.AddReferences(eMode) Then
            Me.Updated = True
            Return True
        End If
        Return False
    End Function

    Public Function RemoveRedundantReference(ByVal import As XmlComposite, ByVal reference As XmlComponent) As Boolean
        Dim xmlcpnt As XmlImportView = XmlNodeManager.GetInstance().CreateView(import.Node, "import")
        If xmlcpnt.RemoveRedundant(reference) Then
            Me.Updated = True
            Return True
        End If
        Return False
    End Function

    Public Function MoveUpNode(ByVal parent As XmlComposite, ByVal child As XmlComponent) As Boolean
        Select Case child.NodeName
            Case "package", "class", "import"
                If parent.MoveUpComponent(child) Then
                    Me.Updated = True
                    Return True
                End If
        End Select
        Return False
    End Function

    Public Function RemoveAllReferences(ByVal composite As XmlComposite) As Boolean
        Dim xmlcpnt As XmlImportView = XmlNodeManager.GetInstance().CreateView(composite.Node, "import")
        xmlcpnt.NodeCounter = m_xmlReferenceNodeCounter
        If xmlcpnt.RemoveAllReferences() Then
            Me.Updated = True
            Return True
        End If
        Return False
    End Function

    Public Function OverrideProperties(ByVal composite As XmlComposite) As Boolean
        If composite.NodeName = "class" Then
            Dim xmlcpnt As XmlClassSpec = XmlNodeManager.GetInstance().CreateDocument(composite.Node)
            If xmlcpnt.OverrideProperties() Then
                Me.Updated = True
                Return True
            End If
        End If
        Return False
    End Function

    Public Function OverrideMethods(ByVal composite As XmlComposite) As Boolean
        If composite.NodeName = "class" Then
            Dim xmlcpnt As XmlClassSpec = XmlNodeManager.GetInstance().CreateDocument(composite.Node)
            If xmlcpnt.OverrideMethods() Then
                Me.Updated = True
                Return True
            End If
        End If
        Return False
    End Function

    Public Sub UpdateSimpleTypes()
        UmlNodesManager.UpdateSimpleTypes(GetSimpleTypesFilename(Me.Properties.GenerationLanguage))
    End Sub

#End Region

#Region "Private methods"

    Private Shared Function InitPrototypes() As Boolean
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
                .AddView("method_member_view", New XmlMethodMemberView)
                .AddView("class_inherited_view", New XmlClassInheritedView)
                .AddView("type_enumvalue_view", New XmlEnumView)
                .AddView("type_element_view", New XmlElementView)
                .AddView("method_exception_view", New XmlMethodExceptionView)
                .AddView("project_member_view", New XmlProjectMemberView)
                .AddView("class_override_methods", New XmlClassOverrideMethodsView)
                .AddView("reference_redundancy", New XmlRefRedundancyView)
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
            Return False
        End Try
        Return True
    End Function

    Private Sub LoadDocument(ByVal strFilename As String)
        Try
            XmlProjectTools.LoadDocument(m_xmlDocument, strFilename)

            If m_xmlDocument.DocumentElement IsNot Nothing _
            Then
                m_xmlProperties = XmlNodeManager.GetInstance().CreateDocument(m_xmlDocument.DocumentElement)
                m_xmlReferenceNodeCounter = New XmlReferenceNodeCounter(m_xmlDocument)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function GetProjectName(ByVal strFullpathFilename As String) As String
        Dim strProjectName As String = Mid(strFullpathFilename, InStrRev(strFullpathFilename, "\") + 1)
        strProjectName = Left(strProjectName, InStrRev(strProjectName, ".") - 1)
        Return strProjectName
    End Function

#End Region
End Class
