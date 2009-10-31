Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports ClassXmlProject.XmlClassListView
Imports ClassXmlProject.XmlProjectTools

Public Class XmlClassDependencyView
    Inherits XmlDependencySpec
    Implements InterfGridViewNotifier
    Implements InterfViewControl


    Public Sub InitControl(ByVal control As System.Windows.Forms.Control) Implements InterfViewControl.InitControl
        Try
            Dim data As XmlDataGridView = CType(control, XmlDataGridView)
            Dim col1 As DataGridViewComboBoxColumn
            Dim col2 As DataGridViewTextBoxColumn

            Debug.Print("grid view '" + Me.ToString + "': " + XmlProjectTools.GetName(MyBase.Document.DocumentElement))

            With data
                .Columns.Clear()
                .AllowDrop = True
                .ColumnDragStart = -1   ' Row header
                .AutoGenerateColumns = False
                .AllowUserToDeleteRows = False
                .AllowUserToAddRows = True
                .AllowUserToResizeRows = True
                .AllowUserToOrderColumns = True
            End With

            If DEBUG_COMMANDS_ACTIVE = True Then
                col2 = New DataGridViewTextBoxColumn
                With col2
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    .DataPropertyName = cstIdref
                    .HeaderText = cstIdref
                    .Name = "ControlName_" + cstIdref
                End With
                data.Columns.Add(col2)
            Else
                col1 = New DataGridViewComboBoxColumn
                AddListComboBoxColumn(Me, cstIdref, "Name", col1)
                col1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                data.Columns.Add(col1)
            End If

            If Me.GenerationLanguage <> ELanguage.Language_Vbasic _
            Then
                col1 = New DataGridViewComboBoxColumn
                With col1
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .HeaderText = "Kind"
                    .DataPropertyName = "StringKind"
                    .DisplayStyleForCurrentCellOnly = True
                    .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
                    .Items.AddRange(New Object() {"body", "reference", "interface"})
                End With
                data.Columns.Add(col1)
            End If

            col2 = New DataGridViewTextBoxColumn
            With col2
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                .DataPropertyName = "Action"
                .HeaderText = "Action"
                .Name = "ControlName_Action"
            End With
            data.Columns.Add(col2)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function EventClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventClick
        Return False
    End Function

    Public Function EventDoubleClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventDoubleClick
        Return False
    End Function

    Public Function Compare(ByVal nodeName As String) As Integer Implements InterfViewControl.Compare
        Return 0
    End Function

    Public Function CanDropItem(ByVal component As XmlComponent) As Boolean Implements InterfGridViewNotifier.CanDropItem
        If component.NodeName = "dependency" Then
            Me.DropInsertComponent(component)
            Return True
        End If
        Return False
    End Function

    Public Function CanDragItem() As Boolean Implements InterfGridViewNotifier.CanDragItem
        Return True
    End Function
End Class
