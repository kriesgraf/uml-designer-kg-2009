Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports ClassXmlProject.XmlMethodSpec
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class XmlClassMemberView
    Inherits XmlComponent
    Implements InterfGridViewNotifier
    Implements InterfViewControl
    Implements InterfNodeCounter
    Implements InterfObject

    Private m_xmlAdapter As XmlTypeVarSpec = Nothing
    Private m_xmlClassView As XmlClassGlobalView = Nothing
    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

    Public ReadOnly Property TypeVarDefinition() As XmlTypeVarSpec
        Get
            If m_xmlAdapter IsNot Nothing Then
                m_xmlAdapter.GenerationLanguage = Me.GenerationLanguage
            End If
            Return m_xmlAdapter
        End Get
    End Property

    Public Overrides Property Name() As String
        Get

            If MyBase.NodeName = "method" Then
                Dim tempo As String = GetAttribute("operator")
                If tempo IsNot Nothing _
                Then
                    Return tempo

                ElseIf MyBase.Name = "#method" _
                Then ' No name
                    Return "Constructor"
                End If
            End If
            Return MyBase.Name
        End Get
        Set(ByVal value As String)
            If GetAttribute("operator") Is Nothing _
            Then
                MyBase.Name = value
            Else
                SetAttribute("operator", value)
            End If
        End Set
    End Property

    Public Property Member() As String
        Get
            If NodeName <> "typedef" Then
                Return GetAttribute("member")
            Else
                Return "object"
            End If
        End Get
        Set(ByVal value As String)
            If NodeName <> "typedef" Then
                SetAttribute("member", value)
            End If
        End Set
    End Property

    Public Property Range() As String
        Get
            If m_xmlAdapter Is Nothing Then
                Return GetAttribute("constructor")
            Else
                Return Me.TypeVarDefinition.Range
            End If
        End Get
        Set(ByVal value As String)
            If m_xmlAdapter IsNot Nothing Then
                Me.TypeVarDefinition.Range = value
            End If
        End Set
    End Property

    Public Property InterfObject() As Object Implements InterfObject.InterfObject
        Get
            Return m_xmlClassView
        End Get
        Set(ByVal value As Object)
            m_xmlClassView = TryCast(value, XmlClassGlobalView)
        End Set
    End Property

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

    Public ReadOnly Property FullpathTypeDescription() As String
        Get
            If m_xmlAdapter Is Nothing Then
                Return "<constructor>"
            Else
                Return Me.TypeVarDefinition.FullpathTypeDescription
            End If
        End Get
    End Property

    Public Sub UpdateObject() Implements InterfObject.Update
        Select Case Me.NodeName
            Case "property"
                Dim xmlProperty As XmlPropertySpec = CreateDocument(Me.Node)
                xmlProperty.GenerationLanguage = Me.GenerationLanguage

                Select Case m_xmlClassView.CurrentClassImpl
                    Case XmlProjectTools.EImplementation.Interf

                        xmlProperty.OverridableProperty = True
                        xmlProperty.AccessSetInline = False
                        xmlProperty.AccessGetInline = False
                        xmlProperty.MemberAttribute = False

                    Case XmlProjectTools.EImplementation.Node, _
                         XmlProjectTools.EImplementation.Root
                        ' Ignore

                    Case Else
                        xmlProperty.OverridableProperty = False
                End Select

            Case "method"
                Dim xmlMethod As XmlMethodSpec = CreateDocument(Me.Node)
                xmlMethod.GenerationLanguage = Me.GenerationLanguage

                Select Case m_xmlClassView.CurrentClassImpl
                    Case XmlProjectTools.EImplementation.Interf

                        xmlMethod.Implementation = XmlProjectTools.EImplementation.Interf

                    Case XmlProjectTools.EImplementation.Leaf
                        Select Case xmlMethod.Implementation
                            Case XmlProjectTools.EImplementation.Node, _
                                 XmlProjectTools.EImplementation.Root
                                xmlMethod.Implementation = XmlProjectTools.EImplementation.Leaf

                            Case Else
                                ' Ignore
                        End Select

                    Case XmlProjectTools.EImplementation.Node, _
                         XmlProjectTools.EImplementation.Root
                        ' Ignore

                    Case Else
                        xmlMethod.Implementation = XmlProjectTools.EImplementation.Simple
                End Select

            Case Else
                ' Ignore
        End Select
    End Sub

    Public Function EventClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventClick
        Try
            ' For checkbox column "Member" must return false to execute control updates

            If dataMember = XmlTypeVarSpec.cstFullpathTypeDescription Then

                Dim fen As Form = Nothing

                If m_xmlAdapter IsNot Nothing Then
                    If m_xmlAdapter.GetNode("list") IsNot Nothing Then

                        fen = XmlNodeManager.GetInstance().CreateForm(Me)
                        CType(fen, InterfFormDocument).DisableMemberAttributes()

                    ElseIf m_xmlAdapter.GetNode("element") IsNot Nothing Then

                        fen = XmlNodeManager.GetInstance().CreateForm(Me)
                        CType(fen, InterfFormDocument).DisableMemberAttributes()
                    Else
                        fen = XmlNodeManager.GetInstance().CreateForm(Me.TypeVarDefinition)
                    End If
                Else
                    fen = XmlNodeManager.GetInstance().CreateForm(Me)
                    CType(fen, InterfFormDocument).DisableMemberAttributes()
                End If
                fen.ShowDialog()
                Return CType(fen.Tag, Boolean)
            End If
            Return False
        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Public Function EventDoubleClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventDoubleClick
        Try
            Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(Me)
            Select Case Me.NodeName
                Case "property"
                    CType(fen, InterfFormClass).ClassImpl = m_xmlClassView.CurrentClassImpl

                Case "method"
                    If Me.CheckAttribute("constructor", "no", "no") = True _
                    Then
                        CType(fen, InterfFormClass).ClassImpl = m_xmlClassView.CurrentClassImpl
                    End If
                Case Else
                    ' Ignore
            End Select
            fen.ShowDialog()
            Return CType(fen.Tag, Boolean)

        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Public Sub InitControl(ByVal control As Control) Implements InterfViewControl.InitControl
        Dim data As XmlDataGridView = CType(control, XmlDataGridView)
        Dim size As DataGridViewAutoSizeColumnMode = DataGridViewAutoSizeColumnMode.Fill
        Dim size2 As DataGridViewAutoSizeColumnMode = DataGridViewAutoSizeColumnMode.AllCells
        With data
            .Columns.Clear()
            .AllowDrop = True
            .ColumnDragStart = 0   ' Name column
            .AutoGenerateColumns = False
            .AllowUserToDeleteRows = False
            .AllowUserToAddRows = True
            .AllowUserToResizeRows = True
            .AllowUserToOrderColumns = True
            .EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
        End With

        Dim col1 As DataGridViewColumn = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = size2
            .DataPropertyName = "Name"
            .HeaderText = "Name"
            .Name = "ControlName_Name"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = size
            .DataPropertyName = "NodeName"
            .ReadOnly = True
            .HeaderText = "Node"
            .Name = "ControlName_Node"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewButtonColumn
        With col1
            .AutoSizeMode = size2
            .DataPropertyName = XmlTypeVarSpec.cstFullpathTypeDescription
            .HeaderText = "Type"
            .Name = "ControlName_Type"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewComboBoxColumn
        With CType(col1, DataGridViewComboBoxColumn)
            .DisplayStyleForCurrentCellOnly = True
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .DataPropertyName = "Range"
            .HeaderText = "Visibility"
            .Name = "ControlName_Range"
        End With

        With CType(col1, DataGridViewComboBoxColumn)
            .DisplayStyleForCurrentCellOnly = True
            .AutoSizeMode = size
            .Items.AddRange(New Object() {"private", "protected", "public"})
            .DisplayStyleForCurrentCellOnly = True
            .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
        End With

        data.Columns.Add(col1)

        col1 = New DataGridViewCheckBoxColumn
        With col1
            .AutoSizeMode = size
            .DataPropertyName = "Member"
            .HeaderText = "Class member"
            .Name = "ControlName_Member"
        End With

        With CType(col1, DataGridViewCheckBoxColumn)
            .ThreeState = False
            .FalseValue = "object"
            '        .IndeterminateValue = ""
            .TrueValue = "class"
        End With

        data.Columns.Add(col1)
    End Sub

    Public Function Compare(ByVal nodeName As String) As Integer Implements InterfViewControl.Compare
        Dim iResult As Integer = 0
        Select Case nodeName
            Case "property"
                If Me.NodeName = "method" Then
                    iResult = -1
                End If
            Case "typedef"
                If Me.NodeName <> "typedef" Then
                    iResult = -1
                End If
            Case "container_doc"
                If Me.NodeName <> "typedef" Then
                    iResult = -1
                End If
            Case "structure_doc"
                If Me.NodeName <> "typedef" Then
                    iResult = -1
                End If
        End Select
        Return iResult
    End Function

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        If Node Is Nothing Then
            ' Nothing to do
        ElseIf Node.Name = "method" Then
            If TestNode("return/type") Then
                m_xmlAdapter = MyBase.CreateDocument(GetNode("return/type"))
            Else
                m_xmlAdapter = Nothing
            End If
        Else
            m_xmlAdapter = MyBase.CreateDocument(GetNode("type"))
        End If
        If m_xmlAdapter IsNot Nothing Then m_xmlAdapter.GenerationLanguage = Me.GenerationLanguage
    End Sub

    Public Function CanDropItem(ByVal component As XmlComponent) As Boolean Implements InterfGridViewNotifier.CanDropItem
        If Me.NodeName = component.NodeName Then
            Me.DropInsertComponent(component)
            Return True
        End If
        Return False
    End Function

    Public Function CanDragItem() As Boolean Implements InterfGridViewNotifier.CanDragItem
        Return (Me.NodeName = "typedef" And Me.GenerationLanguage = ELanguage.Language_CplusPlus)
    End Function
End Class
