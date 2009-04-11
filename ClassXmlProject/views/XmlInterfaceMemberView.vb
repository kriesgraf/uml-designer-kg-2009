Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports ClassXmlProject.XmlMethodSpec
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class XmlInterfaceMemberView
    Inherits XmlComponent
    Implements InterfGridViewNotifier
    Implements InterfViewControl
    Implements InterfObject

    Private m_xmlAdapter As XmlTypeVarSpec = Nothing
    Private m_xmlClassView As XmlInterfaceView = Nothing
    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

    Public Property InterfObject() As Object Implements InterfObject.InterfObject
        Get
            Return m_xmlClassView
        End Get
        Set(ByVal value As Object)
            m_xmlClassView = TryCast(value, XmlInterfaceView)
        End Set
    End Property

    Public ReadOnly Property TypeVarDefinition() As XmlTypeVarSpec
        Get
            If m_xmlAdapter IsNot Nothing Then
                m_xmlAdapter.Tag = Me.Tag
            End If
            Return m_xmlAdapter
        End Get
    End Property

    Public ReadOnly Property ImplementationView() As String
        Get
            Select Case Me.NodeName
                Case "property"
                    If CheckAttribute("overridable", "yes", "no") Then
                        Return "overridable"
                    End If
                    Return "not overridable"

                Case "method"
                    Dim eImplementation As EImplementation = ConvertDtdToEnumImpl(GetAttribute("implementation"))
                    Return ConvertEnumImplToView(eImplementation, True)
            End Select
            Return ""
        End Get
    End Property

    Public Overrides Property Name() As String
        Get
            If MyBase.NodeName = "method" And MyBase.Name = cstOperator Then
                Return GetAttribute("operator")
            End If
            Return MyBase.Name
        End Get
        Set(ByVal value As String)
            If MyBase.NodeName = "method" And MyBase.Name = cstOperator Then
                SetAttribute("operator", value)
            Else
                MyBase.Name = value
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
                xmlProperty.OverridableProperty = True

            Case "method"
                If m_xmlClassView.CurrentClassImpl = XmlProjectTools.EImplementation.Interf _
                Then
                    Dim xmlMethod As XmlMethodSpec = CreateDocument(Me.Node)
                    xmlMethod.Implementation = XmlProjectTools.EImplementation.Interf
                End If

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

                    ElseIf m_xmlAdapter.GetNode("element") IsNot Nothing Then

                        fen = XmlNodeManager.GetInstance().CreateForm(Me)
                    Else
                        fen = XmlNodeManager.GetInstance().CreateForm(Me.TypeVarDefinition)
                    End If
                Else
                    fen = XmlNodeManager.GetInstance().CreateForm(Me)
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
                    CType(fen, InterfFormClass).ClassImpl = m_xmlClassView.CurrentClassImpl
            End Select

            fen.ShowDialog()
            Return CType(fen.Tag, Boolean)

        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Public Sub InitControl(ByVal control As Control) Implements InterfViewControl.InitControl
        Dim data As XmlDataGridView = CType(control, XmlDataGridView)
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
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .DataPropertyName = "Name"
            .HeaderText = "Name"
            .Name = "ControlName_Name"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .DataPropertyName = "NodeName"
            .ReadOnly = False
            .HeaderText = "Node"
            .Name = "ControlName_Node"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewTextBoxColumn
        With col1
            .ReadOnly = True
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .DataPropertyName = "ImplementationView"
            .HeaderText = "Implementation"
            .Name = "ControlName_Implementation"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewButtonColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
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
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Items.AddRange(New Object() {"private", "protected", "public"})
            .DisplayStyleForCurrentCellOnly = True
            .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
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
        If m_xmlAdapter IsNot Nothing Then m_xmlAdapter.Tag = Me.Tag
    End Sub

    Public Function CanDropItem(ByVal component As XmlComponent) As Boolean Implements InterfGridViewNotifier.CanDropItem
        Return False
    End Function

    Public Function CanDragItem() As Boolean Implements InterfGridViewNotifier.CanDragItem
        Return False
    End Function
End Class
