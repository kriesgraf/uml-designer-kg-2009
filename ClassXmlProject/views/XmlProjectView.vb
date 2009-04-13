﻿Imports System
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

    Public Function Open(ByVal form As Form, Optional ByVal strFilename As String = "") As Boolean
        Try
            m_strFilename = strFilename
            If strFilename <> "" _
            Then
                Dim strTempPath As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData

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
            m_xmlProperties.Name = GetProjectName(strFilename)
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
                          cstMsgYesNoQuestion) _
                          = MsgBoxResult.No _
                Then
                    Return False
                End If
            End If

            Dim node As XmlNode = component.Node
            Dim dlgSaveFile As New SaveFileDialog

            If My.Settings.ImportFolder = m_strCurrentFolder Then
                dlgSaveFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgSaveFile.InitialDirectory = My.Settings.ImportFolder
            End If

            Dim strFilename As String = GetName(node)
            If XmlProjectTools.GetValidFilename(strFilename) Then
                MsgBox("The filename was not valid, we propose to rename:" + vbCrLf + strFilename)
            End If
            dlgSaveFile.FileName = strFilename
            dlgSaveFile.Filter = "UML project (*.xprj)|*.xprj"


            If dlgSaveFile.ShowDialog() = DialogResult.OK Then

                Dim FileName As String = dlgSaveFile.FileName
                UpdateCurrentImportFolder(FileName, dlgSaveFile.InitialDirectory)

                Dim eLang As ELanguage = CType(Me.Properties.GenerationLanguage, ELanguage)
                Dim strFullPackage As String

                Select Case node.Name
                    Case "root"
                        MsgBox("Needless to export a whole project", MsgBoxStyle.Exclamation)

                    Case "package"
                        strFullPackage = GetFullpathPackage(node, eLang)
                        UmlNodesManager.ExportNodes(fen, node, dlgSaveFile.FileName, strFullPackage, eLang)
                        If bExtractReferences Then
                            strFullPackage = GetFullpathPackage(node, eLang, GetName(node))
                            bResult = UmlNodesManager.ExtractReferences(fen, node, strFullPackage, eLang)
                        End If

                    Case "class"
                        strFullPackage = GetFullpathPackage(node, eLang)
                        UmlNodesManager.ExportNodes(fen, node, dlgSaveFile.FileName, strFullPackage, eLang)
                        If bExtractReferences Then
                            bResult = UmlNodesManager.ExtractReferences(fen, node, strFullPackage, eLang)
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

    Public Function ImportNodes(ByVal form As Form, ByVal component As XmlComponent, Optional ByVal bUpdateOnly As Boolean = False) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim node As XmlNode = component.Node
            Dim dlgOpenFile As New OpenFileDialog

            If My.Settings.ImportFolder = m_strCurrentFolder Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.ImportFolder
            End If

            dlgOpenFile.Title = "Select a project file..."
            dlgOpenFile.Filter = "UML project (*.xprj)|*.xprj"
            dlgOpenFile.FileName = GetName(node)

            If dlgOpenFile.ShowDialog() = DialogResult.OK Then

                Dim FileName As String = dlgOpenFile.FileName
                UpdateCurrentImportFolder(FileName, dlgOpenFile.InitialDirectory)

                bResult = UmlNodesManager.ImportNodes(form, component, FileName, m_xmlReferenceNodeCounter, bUpdateOnly)
                If bResult Then
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
            Dim node As XmlNode = component.Node
            Dim dlgSaveFile As New SaveFileDialog
            Dim strFullPackage As String

            If My.Settings.ImportFolder = m_strCurrentFolder Then
                dlgSaveFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgSaveFile.InitialDirectory = My.Settings.ImportFolder
            End If

            Dim strFilename As String = GetName(node)
            If XmlProjectTools.GetValidFilename(strFilename) Then
                MsgBox("The filename was not valid, we propose to rename:" + vbCrLf + strFilename)
            End If
            dlgSaveFile.FileName = strFilename
            dlgSaveFile.Filter = "Package references (*.ximp)|*.ximp"

            If dlgSaveFile.ShowDialog() = DialogResult.OK Then

                Dim FileName As String = dlgSaveFile.FileName
                If FileName.EndsWith(".ximp") = False Then
                    FileName += ".ximp"
                End If

                UpdateCurrentImportFolder(FileName, dlgSaveFile.InitialDirectory)

                Dim eLang As ELanguage = CType(Me.Properties.GenerationLanguage, ELanguage)

                Select Case node.Name
                    Case "root"
                        strFullPackage = GetName(node)
                        ExportPackageReferences(fen, node, FileName, strFullPackage, eLang)

                    Case "package"
                        strFullPackage = GetFullpathPackage(node, eLang)

                        If SelectNodes(node, "descendant::import").Count > 0 Then
                            MsgBox("Import members will not be exported", vbExclamation)
                        End If
                        If SelectNodes(node, "descendant::class[@visibility='package']").Count > 0 Then
                            ExportPackageReferences(fen, node, FileName, strFullPackage, eLang)
                        Else
                            MsgBox("Class " + GetName(node) + " has no class members with package visibility", vbExclamation)
                        End If

                    Case "class"
                        strFullPackage = GetFullpathPackage(node, eLang)

                        If GetNodeString(node, "@visibility") = "package" Then
                            ExportClassReferences(fen, node, FileName, strFullPackage, eLang)
                        Else
                            MsgBox("Class " + GetName(node) + " has not a package visibility", vbExclamation)
                        End If
                    Case "typedef"
                        strFullPackage = GetFullpathPackage(node.ParentNode, eLang)

                        If GetNodeString(node.ParentNode, "@visibility") = "package" Then
                            If GetNodeString(node, "variable/@range") = "public" Then
                                ExportTypedefReferences(fen, node, FileName, strFullPackage)
                            Else
                                MsgBox("Typedef " + GetName(node) + " is not public", vbExclamation)
                            End If
                        Else
                            MsgBox("Class " + GetName(node.ParentNode) + " has not a package visibility", vbExclamation)
                        End If

                    Case "import"
                        If node.HasChildNodes = True Then
                            ReExport(fen, node.LastChild, FileName, GetAttributeValue(node, "param"))
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
        Return bResult
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

    Public Function RemoveRedundantReference(ByVal import As XmlComposite, ByVal reference As XmlComponent) As Boolean
        If import IsNot Nothing And reference IsNot Nothing Then
            Dim xmlcpnt As XmlImportView = XmlNodeManager.GetInstance().CreateView(import.Node, "import")
            xmlcpnt.Tag = import.Tag
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
                Case "package", "class", "import"
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
                MsgBox("No properties to override!", MsgBoxStyle.Exclamation)
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
                MsgBox("No methods to override!", MsgBoxStyle.Exclamation)
            End If
        End If
        Return False
    End Function

    Public Sub UpdateSimpleTypes()
        UmlNodesManager.UpdateSimpleTypes(GetSimpleTypesFilename(CType(Me.Properties.GenerationLanguage, ELanguage)))
    End Sub

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
                Me.Updated = bChanged
                m_xmlReferenceNodeCounter = New XmlReferenceNodeCounter(m_xmlDocument)
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
        Dim i As Integer = InStr(Filename, Path.DirectorySeparatorChar.ToString)

        If i > 0 Then
            My.Settings.ImportFolder = Path.GetFileName(Filename)
        Else
            My.Settings.ImportFolder = strDefault
        End If
    End Sub

#End Region
End Class
