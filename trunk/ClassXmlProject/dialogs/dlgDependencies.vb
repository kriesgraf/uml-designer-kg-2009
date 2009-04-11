Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports System.Collections
Imports ClassXmlProject.XmlNodeListView

Public Class dlgDependencies
    Implements InterfFormDocument

    Private m_xmlDataSource As XmlComponent
    Private m_bEmpty As Boolean = True
    Private m_bNoTitle As Boolean = True

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlDataSource = value
            ' get a useful tag that transmit generation language ID
            m_xmlDataSource.Tag = value.Tag
        End Set
    End Property

    Public ReadOnly Property IsEmpty() As Boolean
        Get
            Return m_bEmpty
        End Get
    End Property

    Public WriteOnly Property Title() As String
        Set(ByVal value As String)
            If value IsNot Nothing Then
                m_bNoTitle = False
                Me.Text = value
            End If
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
        ' Nothing TODO
    End Sub

    Public Shared Function ShowDependencies(ByVal component As XmlComponent, ByRef bIsEmpty As Boolean, Optional ByVal title As String = Nothing) As Boolean
        Dim fen As New dlgDependencies
        fen.Title = title
        fen.Document = component
        fen.ShowDialog()
        bIsEmpty = fen.IsEmpty
        Return (CType(fen.Tag, Boolean))
    End Function

    Public Shared Function GetQuery(ByVal component As XmlComponent) As String
        Dim strNodeName As String
        Dim strQuery As String = Nothing

        strNodeName = component.NodeName

        Select Case strNodeName
            Case "property", "method"
                Dim strID As String = component.GetAttribute("id", "parent::class | parent::interface")
                strQuery = "//class[" + strNodeName + "[@overrides='" + strID + "']]"
                Return strQuery

            Case "package", "import"
                strQuery = "descendant::import | class | package | descendant::reference | descendant::interface"

            Case Else
                Dim strID As String = component.GetAttribute("id")
                strQuery = "//*[@*[.='" + strID + "' and name()!='id' and name()!='overrides']" + _
                           " and (ancestor::class/@id!='" + strID + "' or ancestor::interface/@id!='" + strID + "')]"
        End Select
        Return strQuery
    End Function

    Private Sub dlgDependencies_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            RefreshList()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub lsbDependencies_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsbDependencies.DoubleClick
        Try
            If lsbDependencies.SelectedItem IsNot Nothing Then
                Dim xmlcpnt As XmlComponent = XmlNodeManager.GetInstance().CreateDocument(CType(lsbDependencies.SelectedItem, XmlNodeListView).MasterNode)
                xmlcpnt.Tag = m_xmlDataSource.Tag
                Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(xmlcpnt)
                fen.ShowDialog()
                If CType(fen.Tag, Boolean) = True Then
                    RefreshList()
                    m_xmlDataSource.Updated = True
                    Me.Tag = True
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            Dim MyList As New ArrayList

            If m_bNoTitle Then Me.Text = m_xmlDataSource.Name + " dependencies"

            Dim strQuery As String = GetQuery(m_xmlDataSource)

            If strQuery Is Nothing _
            Then
                lsbDependencies.Items.Add("Sorry this node can't be referenced")
                lsbDependencies.Enabled = False
            Else
                AddNodeList(m_xmlDataSource, MyList, strQuery)

                If MyList.Count = 0 _
                Then
                    lsbDependencies.DataSource = Nothing
                    lsbDependencies.Enabled = False
                    m_bEmpty = True

                    If m_bNoTitle Then
                        lsbDependencies.Items.Add("Sorry no dependencies found")
                    Else
                        lsbDependencies.Items.Add("Close this dialog box to complete the operation")
                    End If
                Else
                    XmlNodeListView.SortNodeList(MyList)
                    lsbDependencies.DataSource = MyList
                    lsbDependencies.DisplayMember = "FullpathClassName"
                    m_bEmpty = False
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class