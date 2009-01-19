Imports System.Xml
Imports ClassXmlProject.XmlMethodSpec
Imports ClassXmlProject.UmlCodeGenerator

Public Class XmlClassMemberView
    Inherits XmlComponent
    Implements InterfGridViewNotifier
    Implements InterfViewControl
    Implements InterfNodeCounter
    Implements InterfObject

    Private m_xmlAdapter As XmlTypeVarSpec
    Private m_xmlClassView As XmlClassGlobalView
    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

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
                Return m_xmlAdapter.Range
            End If
        End Get
        Set(ByVal value As String)
            If m_xmlAdapter IsNot Nothing Then
                m_xmlAdapter.Range = value
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
                m_xmlAdapter.Tag = Me.Tag
                Return m_xmlAdapter.FullpathTypeDescription
            End If
        End Get
    End Property

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
                        m_xmlAdapter.Tag = Me.Tag
                        fen = XmlNodeManager.GetInstance().CreateForm(m_xmlAdapter)
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
            If TypeOf (fen) Is dlgMethod Then
                CType(fen, dlgMethod).ClassImpl = m_xmlClassView.CurrentClassImpl
            End If
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

        col1 = New DataGridViewCheckBoxColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
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
    End Sub

    Public Function CanDropItem(ByVal component As XmlComponent) As Boolean Implements InterfGridViewNotifier.CanDropItem
        If Me.NodeName = component.NodeName Then
            Me.DropInsertComponent(component)
            Return True
        End If
        Return False
    End Function

    Public Function CanDragItem() As Boolean Implements InterfGridViewNotifier.CanDragItem
        Return (Me.NodeName = "typedef" And Me.Tag = ELanguage.Language_CplusPlus)
    End Function
End Class
