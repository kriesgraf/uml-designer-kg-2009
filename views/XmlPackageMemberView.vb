Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class XmlPackageMemberView
    Inherits XmlClassSpec
    Implements InterfGridViewNotifier
    Implements InterfNodeCounter
    Implements InterfViewControl

    Public ReadOnly Property ImplementationView() As String
        Get
            Return ConvertEnumImplToView(MyBase.Implementation)
        End Get
    End Property

    Public Function EventClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventClick
        Return False
    End Function

    Public Function EventDoubleClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventDoubleClick
        Try
            Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(Me)
            CType(fen, InterfNodeCounter).NodeCounter = m_xmlReferenceNodeCounter
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
            .AllowUserToAddRows = False
            .AllowUserToResizeRows = True
            .AllowUserToOrderColumns = False
        End With

        Dim col1 As DataGridViewColumn = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .DataPropertyName = "Name"
            .HeaderText = "Name"
            .Name = "ControlName_Name"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .DataPropertyName = "ImplementationView"
            .ReadOnly = True
            .HeaderText = "Implementation"
            .Name = "ControlName_Implementation"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewComboBoxColumn
        With CType(col1, DataGridViewComboBoxColumn)
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .DataPropertyName = "Visibility"
            .HeaderText = "Visibility"
            .Name = "ControlName_Range"
            .Items.AddRange(New Object() {"package", "common"})
            .DisplayStyleForCurrentCellOnly = True
            .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .DataPropertyName = "BriefComment"
            .HeaderText = "Brief comment"
            .Name = "ControlName_BriefComment"
        End With
        data.Columns.Add(col1)
    End Sub

    Public Function Compare(ByVal insertNode As String) As Integer Implements InterfViewControl.Compare
        If Me.NodeName = "package" And insertNode = "class" Then
            Return -1
        End If
        Return 0
    End Function

    Public Function CanDropItem(ByVal component As XmlComponent) As Boolean Implements InterfGridViewNotifier.CanDropItem
        Return False
    End Function

    Public Function CanDragItem() As Boolean Implements InterfGridViewNotifier.CanDragItem
        Return False
    End Function
End Class
