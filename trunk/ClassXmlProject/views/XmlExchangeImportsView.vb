Imports System.Windows.Forms
Imports ClassXmlProject.XmlNodeListView
Imports System.Collections

Public Class XmlExchangeImportsView
    Inherits XmlImportSpec
    Implements InterfViewForm

    Private m_listImports As ListBox
    Private m_listDestination As ListBox
    Private m_listSource As ListBox
    Private m_xmlDestination As XmlImportSpec = Nothing

#Region "Public methods"

    Public Sub LoadValues()
        m_xmlNodeManager = XmlNodeManager.GetInstance
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgImportExchange
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingName(ByRef strName As String)
        strName = "Imports " + MyBase.Name
    End Sub

    Public Sub InitBindingImports(ByVal listBox As ListBox)
        m_listImports = listBox
        UpdateImports()
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

    Public Sub AddImport(ByVal list As ListBox)
        Dim parent As XmlComposite = m_xmlNodeManager.CreateDocument(Me.Document.SelectSingleNode("/root"))
        parent.AddNewComponent("import")
        UpdateImports()
        Me.Updated = True
    End Sub

    Public Sub Edit(ByVal list As ListBox)
        If list.SelectedItem IsNot Nothing _
            Then
            Dim document As XmlComponent = CType(list.SelectedItem, XmlComponent)
            Dim fen As Form = m_xmlNodeManager.CreateForm(document)
            Dim InterfCounter As InterfNodeCounter = TryCast(fen, InterfNodeCounter)
            If InterfCounter IsNot Nothing Then InterfCounter.NodeCounter = m_xmlReferenceNodeCounter

            fen.ShowDialog()
            If CType(fen.Tag, Boolean) Then
                UpdateSource()
                UpdateDestination()
                Me.Updated = True
            End If
        End If
    End Sub

    Public Sub Delete(ByVal list As ListBox)
        If list.SelectedItem IsNot Nothing _
            Then
            Dim document As XmlComponent = CType(list.SelectedItem, XmlComponent)
            Dim parent As XmlComposite = m_xmlNodeManager.CreateDocument(document.Node.ParentNode)
            If parent.CanRemove(document) Then
                If parent.RemoveComponent(document) Then
                    UpdateImports()
                    Me.Updated = True
                End If
            End If
        End If
    End Sub
#End Region

#Region "Private methods"

    Private Sub UpdateImports()
        Dim listNode As New ArrayList
        AddNodeList(Me, listNode, "//import[@name!='" + Me.Name + "']", Me)
        SortNodeList(listNode)

        With m_listImports
            .DataSource = listNode
            .DisplayMember = cstFullpathClassName
            .SelectionMode = SelectionMode.One
            ' Selection automatically call "UpdateDestination"
            .SelectedIndex = 0
        End With
    End Sub

    Private Sub UpdateSource()
        Dim listNode As New ArrayList
        AddNodeList(Me, listNode, "descendant::reference | descendant::interface", Me)
        SortNodeList(listNode)

        With m_listSource
            .DataSource = listNode
            .DisplayMember = cstFullpathClassName
            .SelectionMode = SelectionMode.MultiSimple
            .SelectedIndex = -1
        End With
    End Sub

    Private Sub UpdateDestination()
        Dim listNode As New ArrayList
        AddNodeList(m_xmlDestination, listNode, "descendant::reference | descendant::interface", Me)
        SortNodeList(listNode)

        With m_listDestination
            .DataSource = listNode
            .DisplayMember = cstFullpathClassName
            .SelectionMode = SelectionMode.MultiSimple
            .SelectedIndex = -1
        End With
    End Sub
#End Region
End Class
