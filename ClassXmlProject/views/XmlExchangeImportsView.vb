Imports System.Windows.Forms
Imports ClassXmlProject.XmlNodeListView
Imports System.Collections

Public Class XmlExchangeImportsView
    Inherits XmlImportSpec
    Implements InterfViewForm

    Private m_listDestination As ListBox
    Private m_listSource As ListBox
    Private m_xmlDestination As XmlImportSpec = Nothing

#Region "Public methods"

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgImportExchange
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingName(ByRef strName As String)
        strName = "Imports " + MyBase.Name
    End Sub

    Public Sub InitBindingImports(ByVal listBox As ListBox)
        Dim listNode As New ArrayList
        AddNodeList(Me, listNode, "//import[@name!='" + Me.Name + "']", Me)
        SortNodeList(listNode)

        listBox.DataSource = listNode
        listBox.DisplayMember = cstFullpathClassName
        listBox.SelectionMode = SelectionMode.One
        listBox.SelectedIndex = 0
    End Sub

    Public Sub InitBindingSource(ByVal listBox As ListBox)
        m_listSource = listBox
        UpdateSource()
    End Sub

    Public Sub SelectDestination(ByVal listBox As ListBox)
        Dim element As XmlNodeListView = TryCast(listBox.SelectedItem, XmlNodeListView)
        If element IsNot Nothing Then
            m_xmlDestination = CType(CreateDocument(element.Node), XmlImportSpec)
            m_xmlDestination.Tag = Me.Tag
            UpdateDestination()
        Else
            m_xmlDestination = Nothing
        End If
    End Sub

    Public Sub InitBindingDestination(ByVal listBox As ListBox)
        m_listDestination = listBox
    End Sub

    Public Sub MoveSource()
        For Each child As XmlNodeListView In m_listDestination.SelectedItems
            Me.AppendComponent(child)
        Next
        UpdateSource()
        UpdateDestination()
    End Sub

    Public Sub MoveDestination()
        If m_xmlDestination Is Nothing Then Exit Sub
        For Each child As XmlNodeListView In m_listSource.SelectedItems
            m_xmlDestination.AppendComponent(child)
        Next
        UpdateSource()
        UpdateDestination()
    End Sub
#End Region

#Region "Private methods"

    Private Sub UpdateSource()
        Dim listNode As New ArrayList
        AddNodeList(Me, listNode, "descendant::reference | descendant::interface", Me)
        SortNodeList(listNode)

        m_listSource.DataSource = listNode
        m_listSource.DisplayMember = cstFullpathClassName
        m_listSource.SelectionMode = SelectionMode.MultiSimple
    End Sub

    Private Sub UpdateDestination()
        Dim listNode As New ArrayList
        AddNodeList(m_xmlDestination, listNode, "descendant::reference | descendant::interface", Me)
        SortNodeList(listNode)

        m_listDestination.DataSource = listNode
        m_listDestination.DisplayMember = cstFullpathClassName
        m_listDestination.SelectionMode = SelectionMode.MultiSimple
    End Sub
#End Region
End Class
