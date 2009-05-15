Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class XmlClassRelationView
    Inherits XmlCollaborationSpec
    Implements InterfGridViewNotifier
    Implements InterfNodeCounter
    Implements InterfViewControl

    Private Const cstFullpathTypeDescription As String = "Type"
    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

    Public ReadOnly Property ClassName() As String
        Get
            Return MyBase.RelationParent.FullpathTypeDescription
        End Get
    End Property

    Public ReadOnly Property Type() As String
        Get
            Return MyBase.RelationSpec.Kind.ToString
        End Get
    End Property


    Public Sub InitControl(ByVal control As System.Windows.Forms.Control) Implements InterfViewControl.InitControl
        Try
            Dim data As XmlDataGridView = CType(control, XmlDataGridView)
            Dim size As DataGridViewAutoSizeColumnMode = DataGridViewAutoSizeColumnMode.Fill
            Dim size2 As DataGridViewAutoSizeColumnMode = DataGridViewAutoSizeColumnMode.AllCells

            With data
                .Columns.Clear()
                .AllowDrop = True
                .ColumnDragStart = 0   ' Row header
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
                .DataPropertyName = "ClassName"
                .HeaderText = "Class"
                .Name = "ControlName_Class"
            End With
            data.Columns.Add(col1)

            col1 = New DataGridViewButtonColumn
            With col1
                .AutoSizeMode = size2
                .DataPropertyName = cstFullpathTypeDescription
                .HeaderText = "Type"
                .Name = "ControlName_Type"
                .ReadOnly = True
            End With
            data.Columns.Add(col1)

            Dim col2 As DataGridViewComboBoxColumn = New DataGridViewComboBoxColumn
            With col2
                .DisplayStyleForCurrentCellOnly = True
                .AutoSizeMode = size2
                .DataPropertyName = "Range"
                .HeaderText = "Visibility"
                .Name = "ControlName_Range"
                .Items.AddRange(New Object() {"no", "private", "protected", "public"})
            End With
            data.Columns.Add(col2)

            Dim col3 As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn
            With col3
                .AutoSizeMode = size2
                .DataPropertyName = "Member"
                .HeaderText = "Class member"
                .Name = "ControlName_Member"
            End With
            data.Columns.Add(col3)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function EventClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventClick
        ' For checkbox column "Member" must return false execute control updates
        If dataMember = cstFullpathTypeDescription Then
            Return EventDoubleClick(dataMember)
        End If
        Return False
    End Function

    Public Function EventDoubleClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventDoubleClick
        Try
            Dim frmResult As Form = XmlNodeManager.GetInstance().CreateForm(MyBase.RelationSpec)
            CType(frmResult, InterfFormCollaboration).ClassID = Me.ClassId
            CType(frmResult, InterfFormDocument).DisableMemberAttributes()
            frmResult.ShowDialog()
            Return CType(frmResult.Tag, Boolean)

        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Public Function Compare(ByVal nodeName As String) As Integer Implements InterfViewControl.Compare
        Return 0
    End Function

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

    Public Function CanDropItem(ByVal component As XmlComponent) As Boolean Implements InterfGridViewNotifier.CanDropItem
        Return False
    End Function

    Public Function CanDragItem() As Boolean Implements InterfGridViewNotifier.CanDragItem
        Return False
    End Function
End Class
