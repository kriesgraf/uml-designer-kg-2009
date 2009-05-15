Imports System
Imports System.Windows.Forms
Imports System.Collections
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlProjectTools

Public Class dlgNodeListView

    Private m_strDisplayMember As String
    Private m_strLockedMessage As String
    Private m_list As ArrayList ' Of XmlNodeListView

    Public WriteOnly Property DisplayMember() As String
        Set(ByVal value As String)
            m_strDisplayMember = value
        End Set
    End Property

    Public WriteOnly Property LockedMessage() As String
        Set(ByVal value As String)
            m_strLockedMessage = value
        End Set
    End Property

    Public WriteOnly Property NodeList() As ArrayList
        Set(ByVal value As ArrayList)
            m_list = value
        End Set
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            For Each element As XmlNodeListView In lstNodeList.SelectedItems()
                element.CheckedView = True
            Next
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgNodeListView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lstNodeList.DisplayMember = m_strDisplayMember
        lstNodeList.DataSource = m_list
        lstNodeList.SelectionMode = SelectionMode.MultiSimple
        Dim i As Integer = 0
        For Each node As XmlNodeListView In m_list
            If node.CheckedView Then
                lstNodeList.SetSelected(i, True)
            End If
            i += 1
        Next
    End Sub

    Private Sub lstNodeList_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstNodeList.MouseClick
        Dim index As Integer = lstNodeList.IndexFromPoint(e.X, e.Y)
        If index > -1 Then
            If CType(lstNodeList.Items(index), XmlNodeListView).CheckLocked Then
                MsgBox(m_strLockedMessage, MsgBoxStyle.Critical, "Locked nodes")
                lstNodeList.SetSelected(index, True)
            End If
        End If
    End Sub
End Class
