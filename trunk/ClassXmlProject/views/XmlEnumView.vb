Imports System.Windows.Forms

Public Class XmlEnumView
    Inherits XmlEnumSpec
    Implements InterfViewControl
    Implements InterfGridViewNotifier

    Public Function Compare(ByVal nodeName As String) As Integer Implements InterfViewControl.Compare

    End Function

    Public Sub InitControl(ByVal control As System.Windows.Forms.Control) Implements InterfViewControl.InitControl
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

        Dim size As DataGridViewAutoSizeColumnMode = DataGridViewAutoSizeColumnMode.Fill
        Dim size2 As DataGridViewAutoSizeColumnMode = DataGridViewAutoSizeColumnMode.AllCells

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
            .AutoSizeMode = size2
            .DataPropertyName = "Value"
            .HeaderText = "Value"
            .Name = "ControlName_Value"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = size
            .DataPropertyName = "Comment"
            .HeaderText = "Comment"
            .Name = "ControlName_Comment"
        End With
        data.Columns.Add(col1)
    End Sub

    Public Function CanDragItem() As Boolean Implements InterfGridViewNotifier.CanDragItem
        Return True
    End Function

    Public Function CanDropItem(ByVal component As XmlComponent) As Boolean Implements InterfGridViewNotifier.CanDropItem
        Me.DropInsertComponent(component)
        Return True
    End Function

    Public Function EventClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventClick
        Return False
    End Function

    Public Function EventDoubleClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventDoubleClick
        Return False
    End Function
End Class
