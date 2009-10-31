Imports System
Imports ClassXmlProject.XmlProjectTools
Imports System.Windows.Forms
Imports System.Collections
Imports System.Drawing
Imports Microsoft.VisualBasic

Public Class XmlDataGridView
    Inherits DataGridView

    Private m_xmlBinding As XmlBindingDataGridView
    Private m_bRaiseDataError As Boolean
    Private m_iColumnDragStart As Integer = 0
    Private m_keyDown As Keys

    Public Event RowValuesChanged(ByVal sender As Object)

    Public Property ColumnDragStart() As Integer
        Get
            Return m_iColumnDragStart
        End Get
        Set(ByVal value As Integer)
            m_iColumnDragStart = value
        End Set
    End Property

    Public ReadOnly Property SelectedItem() As Object
        Get
            If Me.CurrentRow IsNot Nothing Then
                Return Me.Rows(Me.CurrentRow.Index).DataBoundItem
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property Binding() As XmlBindingDataGridView
        Get
            Return m_xmlBinding
        End Get
    End Property

    Public WriteOnly Property CatchDataError() As Boolean
        Set(ByVal value As Boolean)
            m_bRaiseDataError = value
        End Set
    End Property

    Public Sub New()
        m_xmlBinding = New XmlBindingDataGridView(Me)
        m_bRaiseDataError = False
        Me.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect

    End Sub

    Public Sub DeleteSelectedItems(Optional ByVal iRemainRow As Integer = -1)
        Dim bResult As Boolean = False
        Try
            Dim iterator As IEnumerator = Me.SelectedRows.GetEnumerator
            Dim component As XmlComponent = Nothing

            If Me.RowCount - Me.SelectedRows.Count - 1 < iRemainRow Then
                MsgBox(CStr(iRemainRow) + " row(s) must remain in the list", MsgBoxStyle.Critical, "'Delete' command")
                Exit Sub
            End If

            While iterator.MoveNext
                component = CType(CType(iterator.Current, DataGridViewRow).DataBoundItem, XmlComponent)
                If m_xmlBinding.DeleteItem(component) Then
                    bResult = True
                End If
            End While
            m_xmlBinding.ResetBindings(bResult)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Function CopySelectedItem() As Boolean
        Dim bResult As Boolean = False
        Try
            Dim component As XmlComponent = CType(Me.SelectedItem, XmlComponent)
            bResult = m_xmlBinding.CopyItem(component)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Function CutSelectedItem() As Boolean
        Dim bResult As Boolean = False
        Try
            Dim component As XmlComponent = CType(Me.SelectedItem, XmlComponent)
            bResult = m_xmlBinding.CutItem(component)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Function PasteItem() As Boolean
        Try
            Return m_xmlBinding.PasteItem()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return False
    End Function

    Public Sub DuplicateSelectedItem()
        Try
            Dim component As XmlComponent = CType(Me.SelectedItem, XmlComponent)
            If component IsNot Nothing Then
                If m_xmlBinding.DuplicateOrPasteItem(component) = False Then
                    MsgBox("Sorry can't duplicate node '" + component.NodeName + "'!", MsgBoxStyle.Exclamation, "'Duplicate' command")
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub AddItem(Optional ByVal strNodeName As String = Nothing)
        Try
            m_xmlBinding.AddItem(strNodeName)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub EditCurrentItem()
        Try
            If Me.CurrentRow IsNot Nothing Then
                Dim e As New DataGridViewCellEventArgs(0, Me.CurrentRow.Index)
                m_xmlBinding.CellContentClick(Me, e, True)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub UpdateCells()
        ' TODO: check if rows are updated....
        If MyBase.EndEdit() Then
            m_xmlBinding.EndEdit()
        End If
    End Sub

    Private Sub XmlDataGridView_CellContentClick(ByVal sender As XmlDataGridView, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellContentClick
        Try
            If m_xmlBinding.CellContentClick(sender, e, False) Then
                OnItemChanged()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub XmlDataGridView_CellContentDoubleClick(ByVal sender As XmlDataGridView, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellContentDoubleClick
        Try
            If m_xmlBinding.CellContentClick(sender, e, True) Then
                OnItemChanged()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub XmlDataGridView_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Me.DataError
        If m_bRaiseDataError Then
            If MsgBox(e.Exception.ToString + vbCrLf + vbCrLf + "Please press Cancel if you have notice the reason of this error.", _
                      cstMsgOkCancelCritical, "Data error") _
                        = MsgBoxResult.Cancel Then

                m_bRaiseDataError = False
            End If
        End If
    End Sub

    Private Sub XmlDataGridView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown

        Debug.Print("MouseDown:=" + e.Button.ToString + " - keyDown:=" + m_keyDown.ToString)

        If e.Button = Windows.Forms.MouseButtons.Left And m_keyDown = Keys.ControlKey _
        Then
            Dim Index As Integer = Me.HitTest(e.X, e.Y).RowIndex
            Dim Col As Integer = Me.HitTest(e.X, e.Y).ColumnIndex

            If Index > -1 And m_iColumnDragStart = Col Then
                If m_xmlBinding.CanDragItem(Me.Rows(Index).DataBoundItem) Then
                    ' We send in drag operation the data object
                    'Debug.Print("ItemDrag=" + Index.ToString + " - " + Me.Rows(Index).DataBoundItem.ToString)
                    Me.DoDragDrop(Me.Rows(Index).DataBoundItem, DragDropEffects.Move)
                End If
            End If
        End If
    End Sub

    Private Sub XmlDataGridView_DragOver(ByVal sender As System.Object, _
                                       ByVal e As System.Windows.Forms.DragEventArgs) _
                                       Handles Me.DragOver
        If e.KeyState = 9 Then
            e.Effect = DragDropEffects.Move
        Else
            m_keyDown = Keys.None
            e.Effect = DragDropEffects.None
        End If
        Debug.Print("DragOver:=" + e.Effect.ToString)
    End Sub

    Private Sub XmlDataGridView_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) _
                                        Handles Me.DragDrop

        Dim clientPoint = Me.PointToClient(New Point(e.X, e.Y))
        Dim Index As Integer = Me.HitTest(clientPoint.X, clientPoint.Y).RowIndex

        If Index > -1 Then
            If Me.Rows(Index).DataBoundItem IsNot Nothing Then
                Debug.Print("DragDrop=" + Index.ToString + " - " + Me.Rows(Index).DataBoundItem.ToString)
                If e.Data.GetDataPresent(e.Data.GetFormats()(0)) _
                Then
                    If m_xmlBinding.CanDropItem(Me.Rows(Index).DataBoundItem, e.Data.GetData(e.Data.GetFormats()(0))) Then
                        OnItemChanged()
                    End If
                End If
            End If
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub XmlDataGridView_RowHeaderMouseClick(ByVal sender As XmlDataGridView, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles Me.RowHeaderMouseClick
        Try
            UpdateCells()
            If m_xmlBinding.RowHeaderClick(sender, e, False) Then
                OnItemChanged()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub XmlDataGridView_RowHeaderMouseDoubleClick(ByVal sender As XmlDataGridView, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles Me.RowHeaderMouseDoubleClick
        Try
            UpdateCells()
            If m_xmlBinding.RowHeaderClick(sender, e, True) Then
                OnItemChanged()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub OnItemChanged()
        RaiseEvent RowValuesChanged(Me)
    End Sub

    Private Sub XmlDataGridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellValueChanged
        m_xmlBinding.CellValueChanged(sender, e)
        RaiseEvent RowValuesChanged(Me)
    End Sub

    Private Sub XmlDataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Debug.Print("KeyDown:=" + e.KeyCode.ToString)
        m_keyDown = e.KeyCode
    End Sub

    Private Sub XmlDataGridView_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Debug.Print("KeyUp:=" + e.KeyCode.ToString)
        m_keyDown = Keys.None
    End Sub
End Class
