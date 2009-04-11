Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class XmlMethodMemberView
    Inherits XmlComponent
    Implements InterfGridViewNotifier
    Implements InterfViewControl

    Private m_xmlAdapter As XmlTypeVarSpec = Nothing

    Public ReadOnly Property TypeVarDefinition() As XmlTypeVarSpec
        Get
            If m_xmlAdapter IsNot Nothing Then
                m_xmlAdapter.Tag = Me.Tag
            End If
            Return m_xmlAdapter
        End Get
    End Property

    Public Property Comment() As String
        Get
            Return GetNodeString("comment")
        End Get
        Set(ByVal value As String)
            SetNodeString("comment", value)
        End Set
    End Property

    Public Overrides Property Name() As String
        Get
            Return MyBase.Name
        End Get
        Set(ByVal value As String)
            MyBase.Name = value
        End Set
    End Property

    Public ReadOnly Property FullpathTypeDescription() As String
        Get
            If m_xmlAdapter Is Nothing Then
                Dim xmlNode As XmlNode = MyBase.GetElementById(MyBase.GetAttribute("idref"))

                Dim eLang As ELanguage = CType(Me.Tag, ELanguage)
                Dim strResult As String = GetFullpathDescription(xmlNode, eLang)
                If DEBUG_COMMANDS_ACTIVE Then strResult += " (" + eLang.ToString + ")"
                Return strResult
            Else
                Return Me.TypeVarDefinition.FullpathTypeDescription
            End If
        End Get
    End Property

    Public Function EventClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventClick
        Try
            Dim fen As Form = Nothing

            If dataMember = XmlTypeVarSpec.cstFullpathTypeDescription Then

                If m_xmlAdapter IsNot Nothing _
                Then
                    m_xmlAdapter.Tag = Me.Tag
                    fen = XmlNodeManager.GetInstance().CreateForm(Me.TypeVarDefinition)
                Else
                    Dim xmlcpnt As XmlComponent = New XmlComponent(MyBase.Node.ParentNode)
                    fen = New dlgException
                    CType(fen, InterfFormDocument).Document = xmlcpnt
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
            Dim fen As Form

            If m_xmlAdapter IsNot Nothing _
            Then
                fen = XmlNodeManager.GetInstance().CreateForm(Me)
            Else
                Dim xmlcpnt As XmlComponent = New XmlComponent(MyBase.Node.ParentNode)
                fen = New dlgException
                CType(fen, InterfFormDocument).Document = xmlcpnt
            End If

            fen.ShowDialog()
            Return CType(fen.Tag, Boolean)

        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        If Node Is Nothing Then
            m_xmlAdapter = Nothing

        ElseIf TestNode("type") Then

            m_xmlAdapter = New XmlTypeVarSpec(GetNode("type"))
            m_xmlAdapter.Tag = Me.Tag
        Else
            m_xmlAdapter = Nothing
        End If
    End Sub

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

        col1 = New DataGridViewButtonColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .DataPropertyName = XmlTypeVarSpec.cstFullpathTypeDescription
            .HeaderText = "Type"
            .Name = "ControlName_Type"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .DataPropertyName = "Comment"
            .HeaderText = "Comment"
            .Name = "ControlName_Comment"
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
        End Select
        Return iResult
    End Function

    Public Function CanDropItem(ByVal component As XmlComponent) As Boolean Implements InterfGridViewNotifier.CanDropItem
        If Me.NodeName = component.NodeName Then
            Me.DropInsertComponent(component)
            Return True
        End If
        Return False
    End Function

    Public Function CanDragItem() As Boolean Implements InterfGridViewNotifier.CanDragItem
        Return True
    End Function
End Class
