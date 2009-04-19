Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Xml
Imports System.IO
Imports System.Collections
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.UmlNodesManager
Imports ClassXmlProject.XmlNodeListView
Imports Microsoft.VisualBasic

Public Class XmlProjectMemberView
    Inherits XmlComposite
    Implements InterfListViewNotifier
    Implements InterfListViewControl
    Implements InterfListViewContext
    Implements InterfViewControl
    Implements InterfNodeCounter
    Implements IEnumerable  ' Use by BindingSource to load information
    Implements IComparer
    Implements InterfObject

#Region "Class declarations"

    Public Enum EIcon
        Package
        ClassElement
        RelationShip
        Import
        Typedef
        PropertyElement
        Method
        Param
        Exception
    End Enum

    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter
    Private m_xmlParentView As XmlProjectMemberView = Nothing

#End Region

#Region "Properties"

    Public Overrides Property Tag() As Integer
        Get
            If MyBase.NodeName = "root" Then
                Return CInt(GetAttribute("language", "generation"))
            End If
            Return MyBase.Tag
        End Get
        Set(ByVal value As Integer)
            If MyBase.NodeName = "root" Then
                SetAttribute("language", CStr(value), "generation")
            End If
            MyBase.Tag = value
        End Set
    End Property

    Public ReadOnly Property ClassImpl() As EImplementation
        Get
            If Me.NodeName = "class" _
            Then
                Return ConvertDtdToEnumImpl(GetAttribute("implementation"))
            Else
                Return EImplementation.Unknown
            End If
        End Get
    End Property

    Public ReadOnly Property CurrentContext() As String Implements InterfListViewContext.CurrentContext
        Get
            Dim strNodeName As String = MyBase.NodeName
            If strNodeName = "root" Then
                Return "project"
            End If
            Return strNodeName
        End Get
    End Property

    Public ReadOnly Property Label() As String()
        Get
            Select Case MyBase.NodeName
                Case "relationship"
                    Dim xmlcpnt As XmlRelationSpec = New XmlRelationSpec(Me.Node)
                    xmlcpnt.ChangeReferences()
                    xmlcpnt.Tag = Me.Tag
                    Return New String() {xmlcpnt.Action, xmlcpnt.Comment, xmlcpnt.NodeName}

                Case "import"
                    Dim xmlcpnt As XmlImportSpec = New XmlImportSpec(Me.Node)
                    xmlcpnt.ChangeReferences()
                    xmlcpnt.Tag = Me.Tag
                    Return New String() {xmlcpnt.Name, xmlcpnt.Comment, xmlcpnt.NodeName}

                Case "reference"
                    Dim xmlcpnt As XmlReferenceSpec = New XmlReferenceSpec(Me.Node)
                    xmlcpnt.ChangeReferences()
                    xmlcpnt.Tag = Me.Tag
                    Return New String() {xmlcpnt.Name, xmlcpnt.Comment, MyBase.NodeName}

                Case "interface"
                    Dim xmlcpnt As XmlInterfaceSpec = New XmlInterfaceSpec(Me.Node)
                    xmlcpnt.ChangeReferences()
                    xmlcpnt.Tag = Me.Tag
                    Return New String() {xmlcpnt.Name, xmlcpnt.Comment, MyBase.NodeName}

                Case "package"
                    Dim strComment As String = MyBase.GetAttribute("folder")
                    If strComment <> "" Then
                        strComment = "(" + strComment + ") "
                    End If
                    strComment += MyBase.GetAttribute("brief", "comment")
                    Return New String() {MyBase.Name, strComment, MyBase.NodeName}

                Case "typedef", "property"
                    Return New String() {MyBase.Name, MyBase.GetNodeString("comment"), MyBase.NodeName}

                Case "method"
                    Dim xmlcpnt As XmlMethodSpec = New XmlMethodSpec(Me.Node)
                    xmlcpnt.ChangeReferences()
                    xmlcpnt.Tag = Me.Tag
                    Return New String() {xmlcpnt.Name, xmlcpnt.Comment, MyBase.NodeName}

                Case "class"
                    Return New String() {MyBase.Name, MyBase.GetAttribute("brief", "comment"), MyBase.NodeName}
            End Select
            Return New String() {MyBase.Name, MyBase.ToString, MyBase.NodeName}
        End Get
    End Property

    Public ReadOnly Property ToolTipText() As String
        Get
            Select Case MyBase.NodeName
                Case "relationship"
                    Dim xmlcpnt As XmlRelationSpec = New XmlRelationSpec(Me.Node)
                    xmlcpnt.ChangeReferences()
                    xmlcpnt.Tag = Me.Tag
                    Return xmlcpnt.Comment

                Case "reference"
                    Dim xmlcpnt As XmlReferenceSpec = New XmlReferenceSpec(Me.Node)
                    xmlcpnt.ChangeReferences()
                    xmlcpnt.Tag = Me.Tag
                    Return xmlcpnt.Comment

                Case "interface"
                    Dim xmlcpnt As XmlInterfaceSpec = New XmlInterfaceSpec(Me.Node)
                    xmlcpnt.ChangeReferences()
                    xmlcpnt.Tag = Me.Tag
                    Return xmlcpnt.Comment

                Case "import"
                    Dim xmlcpnt As XmlImportSpec = New XmlImportSpec(Me.Node)
                    xmlcpnt.ChangeReferences()
                    xmlcpnt.Tag = Me.Tag
                    Return xmlcpnt.Comment

                Case "typedef"
                    Return MyBase.GetNodeString("comment")

                Case "property"
                    Return MyBase.GetNodeString("comment")

                Case Else
                    Return MyBase.GetAttribute("brief", "comment") + vbCrLf + vbCrLf + MyBase.GetNodeString("comment")
            End Select
        End Get
    End Property

    Public ReadOnly Property Icon() As EIcon
        Get
            Select Case MyBase.NodeName
                Case "package"
                    Return EIcon.Package
                Case "class"
                    Return EIcon.ClassElement
                Case "reference", "interface"
                    Return EIcon.ClassElement
                Case "relationship"
                    Return EIcon.RelationShip
                Case "import"
                    Return EIcon.Import
                Case "typedef"
                    Return EIcon.Typedef
                Case "property"
                    Return EIcon.PropertyElement
                Case "method"
                    Return EIcon.Method
                Case "param"
                    Return EIcon.Param
                Case "exception"
                    Return EIcon.Exception
            End Select
            Return EIcon.Package
        End Get
    End Property

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

    Public Property InterfObject() As Object Implements InterfObject.InterfObject
        Get
            Return m_xmlParentView
        End Get
        Set(ByVal value As Object)
            m_xmlParentView = CType(value, XmlProjectMemberView)
        End Set
    End Property

#End Region

#Region "Public methods"

    Public Sub New(Optional ByVal nodeXml As XmlNode = Nothing)
        MyBase.New(nodeXml)
    End Sub

    Public Sub UpdateObject() Implements InterfObject.Update
        Select Case Me.NodeName
            Case "property"
                If m_xmlParentView.ClassImpl = EImplementation.Interf _
                Then
                    Dim xmlcpnt As XmlPropertySpec = CreateDocument(Me.Node)
                    xmlcpnt.Tag = Me.Tag
                    xmlcpnt.OverridableProperty = True
                End If
            Case "method"
                If m_xmlParentView.ClassImpl = EImplementation.Interf _
                Then
                    Dim xmlcpnt As XmlMethodSpec = CreateDocument(Me.Node)
                    xmlcpnt.Tag = Me.Tag
                    xmlcpnt.Implementation = EImplementation.Interf
                End If

            Case Else
                ' Ignore
        End Select
    End Sub

    Public Function CanDrag() As Boolean Implements InterfListViewNotifier.CanDrag
        Return (Me.NodeName <> "relationship")
    End Function

    Public Function CanDropItem(ByVal child As XmlComponent, Optional ByVal bCheckOnly As Boolean = True) As Boolean Implements InterfListViewNotifier.CanDropItem
        Dim bResult As Boolean = True
        Select Case Me.NodeName
            Case "relationship"
                bResult = False

            Case "class"
                bResult = False

            Case "import"
                Dim xmlcpnt As XmlImportSpec = CreateDocument(Me.Node)
                xmlcpnt.Tag = Me.Tag
                bResult = xmlcpnt.CanDropItem(child)
        End Select
        If bResult And bCheckOnly = False Then
            bResult = DropAppendComponent(child)
        End If
        Return bResult
    End Function

    Public Overrides Function DropAppendComponent(ByVal child As XmlComponent) As Boolean
        Dim parent As XmlComposite = CType(CreateDocument(Me.Node), XmlComposite)
        Select Case parent.NodeName
            Case "import"
                Select Case child.NodeName
                    Case "class"
                        If MsgBox("This operation is irreversible, would you want to continue ?" _
                                  , cstMsgYesNoExclamation, "Convert class as reference/interface node") _
                                        = MsgBoxResult.Yes _
                        Then
                            Return ExportClassAndRemove(parent, child)
                        End If

                    Case Else
                        Return parent.DropAppendComponent(child)
                End Select

            Case "package"
                Return parent.DropAppendComponent(child)
        End Select

        Return False
    End Function

    Public Overrides Function CanPasteItem(ByVal child As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Select Case Me.NodeName
            Case "package", "root"
                Select Case child.NodeName
                    Case "class", "import", "package"
                        bResult = True
                End Select

            Case "import"
                Select Case child.NodeName
                    Case "reference", "interface"
                        bResult = True
                End Select

            Case "class"
                Select Case child.NodeName
                    Case "import", "typedef", "property", "method"
                        bResult = True
                End Select
        End Select
        If bResult = False Then
            Return MyBase.CanPasteItem(child)
        End If
        Return True
    End Function

    Public Function EventClick() As Boolean Implements InterfListViewNotifier.EventClick
        Return False
    End Function

    Public Function EventDoubleClick(ByRef bDisplayChildren As Boolean, Optional ByVal bEditMode As Boolean = False) As Boolean Implements InterfListViewNotifier.EventDoubleClick
        bDisplayChildren = False
        Dim bResult As Boolean = False

        If bEditMode = False _
        Then
            Select Case Me.NodeName
                Case "package"
                    bDisplayChildren = Me.TestNode("import | class | package")
                Case "class"
                    bDisplayChildren = Me.TestNode("import | typedef | property | method")
                Case "import"
                    bDisplayChildren = Me.TestNode("export/*")
            End Select
        End If

        If bDisplayChildren = False Then

            Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(Me)

            Dim Interf As InterfNodeCounter = TryCast(fen, InterfNodeCounter)
            If Interf IsNot Nothing Then Interf.NodeCounter = m_xmlReferenceNodeCounter

            fen.ShowDialog()
            bResult = CType(fen.Tag, Boolean)
        End If
        Return bResult
    End Function

    Public Overrides Sub LoadChildrenList(Optional ByVal strViewName As String = "")
        Try
            'Debug.Print("(LoadChildrenList)" + Me.ToString + ":=" + Str(Me.Tag))
            AddChildren(SelectNodes("import | class | export/reference | export/interface | package | relationship | typedef | property | method"), strViewName)
            MyBase.ChildrenList.Sort(Me)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overrides Function DuplicateComponent(ByVal component As XmlComponent) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Select Case component.NodeName
            Case "import", "relationship"
                xmlResult = Nothing

            Case Else
                xmlResult = MyBase.DuplicateComponent(component)
                xmlResult.Tag = Me.Tag
        End Select
        Return xmlResult
    End Function

    Public Function Compare(ByVal insertNodeName As String) As Integer Implements InterfViewControl.Compare
        Return CompareNodeName(Me.NodeName, insertNodeName)
    End Function

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return Me.ChildrenList.GetEnumerator()
    End Function

    Public Sub InitControl(ByVal control As System.Windows.Forms.Control) Implements InterfViewControl.InitControl
        Dim dataControl As DataListView = control

        ' Create and initialize column headers for myListView.
        dataControl.Clear()

        Dim columnHeader As ColumnHeader = New ColumnHeader
        With columnHeader
            .Text = "Name"
            .Width = 200
        End With
        dataControl.Columns.Add(columnHeader)

        columnHeader = New ColumnHeader()
        With columnHeader
            .Text = "Comment"
            .Width = 250
        End With
        dataControl.Columns.Add(columnHeader)

        columnHeader = New ColumnHeader()
        With columnHeader
            .Text = "Node"
            .Width = 100
        End With
        dataControl.Columns.Add(columnHeader)

        With dataControl
            ' .Alignment = ListViewAlignment.SnapToGrid
            ' .AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            '.AutoArrange = True
            .DisplayMember = "Label"
            .ValueMember = "Icon"
            .View = View.Tile
            .MultiSelect = False
        End With
    End Sub

    Public Sub ChangeView(ByVal dataControl As DataListView) Implements InterfListViewControl.ChangeView
        Select Case dataControl.View
            Case View.Tile
                dataControl.TileSize = New Size(200, 90)
                dataControl.ArrangeIcons(ListViewAlignment.SnapToGrid)
                dataControl.Update()

            Case View.LargeIcon
                'dataControl.Alignment = ListViewAlignment.SnapToGrid
                'dataControl.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
                dataControl.AutoArrange = True
                dataControl.Update()

            Case View.SmallIcon
                dataControl.Alignment = ListViewAlignment.SnapToGrid
                dataControl.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
                dataControl.AutoArrange = True
                dataControl.Update()

            Case View.Details
                dataControl.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
                dataControl.Update()

            Case Else
                dataControl.Alignment = ListViewAlignment.Default
                dataControl.AutoArrange = False
                dataControl.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                dataControl.Update()
        End Select
    End Sub

    Public Overrides Function CompareComponent(ByVal x As Object, ByVal y As Object) As Integer

        Dim obj1 As XmlProjectMemberView = CType(x, XmlProjectMemberView)
        Dim obj2 As XmlProjectMemberView = CType(y, XmlProjectMemberView)

        Dim iResult As Integer = CompareNodeName(obj2.NodeName, obj1.NodeName)

        If iResult = 0 Then
            iResult = Comparer.DefaultInvariant.Compare(obj1.Label(0), obj2.Label(0))
        End If
        Return iResult
    End Function

    Public Overrides Function AppendComponent(ByVal nodeXml As XmlComponent, Optional ByVal observer As Object = Nothing) As XmlNode
        If CanAddComponent(nodeXml) _
        Then
            Dim xmlcpnt As XmlComponent = CreateDocument(Me.Node)
            xmlcpnt.Tag = Me.Tag

            Return xmlcpnt.AppendComponent(nodeXml, observer)
        End If
        Return Nothing
    End Function

    Public Overrides Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim xmlcpnt As XmlProjectMemberView = CType(removeNode, XmlProjectMemberView)

            Select Case xmlcpnt.NodeName

                Case "typedef", "class", "reference", "interface"
                    ' Search link from parent node
                    If SelectNodes(GetQueryListDependencies(xmlcpnt)).Count > 0 Then

                        If MsgBox("Some elements reference this, you can dereference them and then this will be deleted." + _
                              vbCrLf + "Do you want to proceed ?", _
                            cstMsgYesNoQuestion, _
                            xmlcpnt.Label(0)) = MsgBoxResult.Yes _
                        Then
                            Dim bIsEmpty As Boolean = False

                            If dlgDependencies.ShowDependencies(xmlcpnt, bIsEmpty, "Remove references to " + xmlcpnt.Label(0)) Then
                                bResult = True
                            End If

                            If bIsEmpty = False Then
                                Return bResult
                            End If
                        Else
                            Return False
                        End If
                    End If

                Case "package", "import"
                    ' Search children from removed node
                    If xmlcpnt.SelectNodes(GetQueryListDependencies(xmlcpnt)).Count > 0 Then
                        MsgBox("This element is not empty", MsgBoxStyle.Exclamation, xmlcpnt.Label(0))
                        Return False
                    End If
            End Select

            If MsgBox("Confirm to delete:" + vbCrLf + "Name: " + xmlcpnt.Label(0) + vbCrLf + "Comment: " + xmlcpnt.Label(1), _
                       cstMsgYesNoQuestion, _
                       xmlcpnt.Label(0)) = MsgBoxResult.Yes _
            Then
                Dim parent As XmlComposite = CreateDocument(MyBase.Node)
                parent.Tag = Me.Tag
                If parent.RemoveComponent(xmlcpnt) Then
                    bResult = True
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Function MoveUpComponent(ByVal child As XmlComponent) As Boolean
        Try
            'Not necessary to remove node, command Append will do it for us
            Dim member As XmlComposite = CreateDocument(Me.Node)
            Select Case member.NodeName
                Case "import"
                    Return CType(member, XmlImportSpec).MoveUpComponent(child)

                Case Else
                    Dim parent As XmlComposite = CreateDocument(Me.Node.ParentNode)
                    parent.Tag = Me.Tag
                    Return (parent.AppendComponent(child) IsNot Nothing)
            End Select

        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Public Function ImportReferences(ByVal fen As Form, ByVal fileName As String) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim import As XmlImportSpec = Nothing

            Select Case Me.NodeName
                Case "root"
                    Dim root As XmlProjectProperties = CreateDocument(Me.Node)
                    import = CreateDocument("import")
                    root.AppendComponent(import)

                Case "package"
                    Dim package As XmlPackageSpec = CreateDocument(Me.Node)
                    import = CreateDocument("import")
                    package.AppendComponent(import)
            End Select

            If import IsNot Nothing Then
                import.NodeCounter = m_xmlReferenceNodeCounter
                Dim FileInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(fileName)
                If import.LoadDocument(fen, FileInfo) Then
                    ExtractExternalReferences(Me.Node, import.ChildExportNode.Node)
                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Overrides Function RemoveMe() As Boolean
        Dim bResult As Boolean = False
        If Me.NodeName = "relationship" _
        Then
            Dim xmlcpnt As XmlRelationSpec = CreateDocument(Me.Node)
            xmlcpnt.Tag = Me.Tag

            bResult = xmlcpnt.RemoveMe()
        Else
            bResult = MyBase.RemoveMe()
        End If
        Return bResult
    End Function
#End Region

#Region "Private methods"

    Private Function CompareNodeName(ByVal currentNode As String, ByVal insertNode As String) As Integer
        Dim iResult As Integer = 0  ' currentNode Equal to insertNode
        Select Case currentNode

            Case "import"
                If insertNode <> "import" Then
                    iResult = 1    ' currentNode Greater than insertNode
                End If

            Case "class"
                If insertNode = "import" Then
                    iResult = -1
                ElseIf insertNode <> "class" Then
                    iResult = 1 ' currentNode Less than insertNode
                End If

            Case "package"
                If insertNode = "relationship" Then
                    iResult = 1
                ElseIf insertNode <> "package" Then
                    iResult = -1
                End If

            Case "relationship"
                If insertNode <> "relationship" Then
                    iResult = -1
                End If

            Case "typedef", "container_doc", "structure_doc"
                If insertNode = "import" Then
                    iResult = -1
                ElseIf insertNode <> "typedef" And _
                        insertNode <> "container_doc" And _
                        insertNode <> "structure_doc" _
                Then
                    iResult = 1
                End If

            Case "property"
                If insertNode = "method" Or insertNode = "constructor_doc" Then
                    iResult = 1
                ElseIf insertNode <> "property" Then
                    iResult = -1
                End If

            Case "method"
                If insertNode <> "method" And insertNode <> "constructor_doc" Then
                    iResult = -1
                End If
        End Select
        Return iResult
    End Function

    Public Function ExportClassAndRemove(ByVal parent As XmlComponent, ByVal child As XmlComponent) As Boolean
        Dim strPackage As String = GetFullpathPackage(child.Node, Me.Tag)
        Dim fragment As XmlDocumentFragment = Me.Document.CreateDocumentFragment()
        fragment.InnerXml = ExportElementClass(child.Node, strPackage)
        parent.AppendNode(fragment.FirstChild)
        child.RemoveMe()
        Return True
    End Function
#End Region
End Class
