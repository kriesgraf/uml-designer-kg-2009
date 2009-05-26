Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports System.Collections
Imports Microsoft.VisualBasic
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
        If component IsNot Nothing Then
            Select Case component.NodeName
                Case "import", "package"
                    If component.SelectNodes(GetQueryListDependencies(component)).Count > 0 _
                    Then
                        MsgBox("No search at this level!", MsgBoxStyle.Exclamation, "'Dependencies' command")
                    Else
                        MsgBox("Element empty!", MsgBoxStyle.Information, "'Dependencies' command")
                    End If
                    Return False
            End Select

            Dim fen As New dlgDependencies
            fen.Title = title
            fen.Document = component
            fen.ShowDialog()
            bIsEmpty = fen.IsEmpty
            Return (CType(fen.Tag, Boolean))
        End If
        Return False
    End Function

    Private Sub dlgDependencies_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _
            Handles Me.KeyUp, lsbDependencies.KeyUp

        Select Case e.KeyCode
            Case Keys.Escape
                Me.Close()

            Case Keys.Enter
                lsbDependencies_DoubleClick(lsbDependencies, Nothing)
        End Select
    End Sub

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

            Dim strQuery As String = GetQueryListDependencies(m_xmlDataSource)

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
                    lsbDependencies.DisplayMember = "FullUmlPathName"
                    m_bEmpty = False
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class