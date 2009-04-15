Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Collections
Imports Microsoft.VisualBasic

Public Class XmlDataListView
    Inherits DataListView

#Region "Class declarations"

    Private m_xmlBinding As XmlBindingDataListView
    Private m_lstMenus As SortedList
    Private m_lstViews As SortedList
    Private m_lviCurrent As ListViewItem
    Private m_strContext As String

    Public Event GoHomeLevel(ByVal sender As Object, ByVal e As DataListViewEventArgs)
    Public Event GoParentLevel(ByVal sender As Object, ByVal e As DataListViewEventArgs)
    Public Event GoChildLevel(ByVal sender As Object, ByVal e As DataListViewEventArgs)
    Public Event ItemChanged(ByVal sender As Object, ByVal e As DataListViewEventArgs)

#End Region

#Region "Properties"

    Public Overloads Property View() As View
        Get
            Return MyBase.View
        End Get
        Set(ByVal value As View)
            ChangeCurrentView(value)
            m_xmlBinding.ChangeView()
        End Set
    End Property

    Public WriteOnly Property CurrentContext() As String
        Set(ByVal value As String)

            Dim mnuCurrent As ContextMenuStrip = Nothing
            m_strContext = value

            If m_lstMenus.Contains(value) _
            Then
                Me.View = CType(m_lstViews.Item(value), View)
                mnuCurrent = TryCast(m_lstMenus.Item(value), ContextMenuStrip)
                If mnuCurrent IsNot Nothing _
                Then
                    MyBase.ContextMenuStrip = mnuCurrent
                End If
            Else
                Throw New Exception(Me.ToString + ".CurrentMenu;menu strip '" + value + "' is not defined !")
            End If
        End Set
    End Property

    Public ReadOnly Property Binding() As XmlBindingDataListView
        Get
            Return m_xmlBinding
        End Get
    End Property

    Public ReadOnly Property Path() As String
        Get
            Return m_xmlBinding.Path
        End Get
    End Property

#End Region

#Region "Public methods"

    Public Sub New()
        m_xmlBinding = New XmlBindingDataListView(Me)
        m_lstMenus = New SortedList
        m_lstViews = New SortedList
    End Sub

    Public Function UpdatePath() As String
        m_xmlBinding.UpdatePath()
        Return m_xmlBinding.Path
    End Function

    Public Sub ChangeCurrentView(ByVal view As View)
        MyBase.View = view
        If m_lstMenus.ContainsKey(m_strContext) Then
            m_lstViews.Item(m_strContext) = view
        End If
    End Sub

    Public Sub AddContext(ByVal strName As String, ByVal menuStrip As ContextMenuStrip, ByVal view As View)
        m_lstMenus.Add(strName, menuStrip)
        m_lstViews.Add(strName, view)
    End Sub

    Public Sub GoBack()
        m_xmlBinding.PopNode()
        OnGoParentLevel(New DataListViewEventArgs)
    End Sub

    Public Sub GoHome()
        m_xmlBinding.GoHomeNode()
        OnGoHomeLevel(New DataListViewEventArgs)
    End Sub

    Public Function CutSelectedItem() As Boolean
        Dim bResult As Boolean = False
        Try
            If Me.SelectedItem IsNot Nothing Then
                Dim component As XmlComponent = CType(Me.SelectedItem, XmlComponent)
                bResult = m_xmlBinding.CutItem(component)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Function CopySelectedItem() As Boolean
        Dim bResult As Boolean = False
        Try
            If Me.SelectedItem IsNot Nothing Then
                Dim component As XmlComponent = CType(Me.SelectedItem, XmlComponent)
                bResult = m_xmlBinding.CopyItem(component)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Function PasteItem() As Boolean
        Dim bResult As Boolean = False
        Try
            bResult = m_xmlBinding.PasteItem()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Function DuplicateSelectedItem(Optional ByVal bAskRefresh As Boolean = True) As Boolean
        Dim bChanged As Boolean = False
        Try

            If Me.SelectedItem IsNot Nothing Then
                Dim component As XmlComponent = CType(Me.SelectedItem, XmlComponent)
                If m_xmlBinding.DuplicateOrPasteItem(component) Then
                    bChanged = True
                Else
                    MsgBox("Sorry can't dupplicate node '" + component.NodeName + " ' !", MsgBoxStyle.Exclamation)
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bChanged
    End Function

    Public Function DeleteSelectedItems() As Boolean
        Dim bChanged As Boolean = False
        Try
            Dim component As XmlComponent = Nothing

            If MyBase.SelectedIndices.Count > 0 Then
                For Each index As Integer In MyBase.SelectedIndices
                    component = CType(MyBase.DataBoundItem(index), XmlComponent)
                    If m_xmlBinding.DeleteItem(component) Then
                        bChanged = True
                    End If
                Next
                If bChanged Then m_xmlBinding.ResetBindings(True)
                If MyBase.Items.Count = 0 Then
                    MyBase.OnEmptyZoneClick()
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bChanged
    End Function

    Public Sub AddItem(Optional ByVal strNodeName As String = "")
        Try
            m_xmlBinding.AddItem(strNodeName)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Function EditCurrentItem() As Boolean
        Dim bChanged As Boolean = False
        Try
            If Me.SelectedItem IsNot Nothing Then
                Dim e As New DataListViewEventArgs(Me.SelectedIndex)
                If m_xmlBinding.ItemClick(bChanged, Me, e, True, True) _
                Then
                    OnItemChanged(e)

                ElseIf bChanged _
                Then
                    OnItemChanged(e)
                End If
                ' Done two times!               m_xmlBinding.ResetBindings(True)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bChanged
    End Function

#End Region

#Region "Private methods"

    Private Sub XmlDataListView_ItemClick(ByVal sender As DataListView, ByVal e As DataListViewEventArgs) Handles Me.ItemClick
        Try
            Dim bChange As Boolean
            m_xmlBinding.ItemClick(bChange, sender, e, False, False)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub XmlDataListView_ItemDoubleClick(ByVal sender As DataListView, ByVal e As DataListViewEventArgs) Handles Me.ItemDoubleClick
        Try
            Dim bChanged As Boolean
            If m_xmlBinding.ItemClick(bChanged, Me, e, True, False) Then
                OnGoChildLevel(e)
            ElseIf bChanged _
            Then
                OnItemChanged(e)
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub XmlDataListView_AfterLabelEdit(ByVal sender As Object, ByVal e As LabelEditEventArgs) Handles Me.AfterLabelEdit
        If m_xmlBinding.AfterLabelEdit(Me, New DataListViewEventArgs(e)) Then
            OnItemChanged(New DataListViewEventArgs(e))
        End If
    End Sub


    Private Sub OnGoHomeLevel(ByVal e As DataListViewEventArgs)
        RaiseEvent GoHomeLevel(Me, e)
    End Sub

    Private Sub OnGoParentLevel(ByVal e As DataListViewEventArgs)
        RaiseEvent GoParentLevel(Me, e)
    End Sub

    Private Sub OnGoChildLevel(ByVal e As DataListViewEventArgs)
        RaiseEvent GoChildLevel(Me, e)
    End Sub

    Private Sub OnItemChanged(ByVal e As DataListViewEventArgs)
        RaiseEvent ItemChanged(Me, e)
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'XmlDataListView
        '
        Me.ResumeLayout(False)

    End Sub

    Private Sub lvwProjectMembers_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles Me.ItemDrag
        If m_xmlBinding.CanDragItem(TryCast(e.Item, ListViewItem)) Then
            m_lviCurrent = Nothing
            DoDragDrop(e.Item, DragDropEffects.Move)
        End If
    End Sub

    Private Sub lvwProjectMembers_DragDrop(ByVal sender As ListView, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
        Dim destItem As ListViewItem = GetDestinationItem(sender, e)
        If e.Data.GetDataPresent("System.Windows.Forms.ListViewItem", False) Then
            'dragging a listview item
            Dim lvItem As ListViewItem = CType(e.Data.GetData("System.Windows.Forms.ListViewItem"), ListViewItem)
            If destItem IsNot Nothing Then
                If m_xmlBinding.DropItem(destItem, lvItem) Then
                    lvItem.Remove()
                    OnItemChanged(New DataListViewEventArgs(destItem.Index))
                End If
            End If
        End If
    End Sub

    Private Sub lvwProjectMembers_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub lvwProjectMembers_DragOver(ByVal sender As ListView, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragOver
        Dim destItem As ListViewItem = GetDestinationItem(sender, e)

        If e.Data.GetDataPresent("System.Windows.Forms.ListViewItem", False) Then

            Dim lvItem As ListViewItem = CType(e.Data.GetData("System.Windows.Forms.ListViewItem"), ListViewItem)
            If destItem IsNot Nothing Then
                If destItem IsNot lvItem Then
                    'If m_lviCurrent IsNot destItem Then
                    '    If m_lviCurrent IsNot Nothing Then m_lviCurrent.Selected = False
                    '    m_lviCurrent = destItem
                    '    destItem.Selected = True
                    'End If
                    If m_xmlBinding.CanDropItem(destItem, lvItem) = False Then
                        e.Effect = DragDropEffects.None
                    Else
                        e.Effect = DragDropEffects.Move
                    End If
                End If
            End If
        End If
    End Sub

    Private Function GetDestinationItem(ByVal destListview As ListView, ByVal oEvent As System.Windows.Forms.DragEventArgs) As ListViewItem
        Dim clX As Integer = destListview.PointToClient(New Point(oEvent.X, oEvent.Y)).X
        Dim clY As Integer = destListview.PointToClient(New Point(oEvent.X, oEvent.Y)).Y
        Return destListview.GetItemAt(clX, clY)
    End Function


#End Region
End Class
