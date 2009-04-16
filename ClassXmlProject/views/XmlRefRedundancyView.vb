Imports System
Imports System.Windows.Forms
Imports System.Xml
Imports System.Collections

Public Class XmlRefRedundancyView
    Inherits XmlComponent

    Private m_xmlProjectNode As XmlComponent = Nothing
    Private m_strImportName As String = ""

    Public WriteOnly Property ProjectNode() As XmlComponent
        Set(ByVal value As XmlComponent)
            m_xmlProjectNode = value
        End Set
    End Property

    Public WriteOnly Property Redundant() As XmlNode
        Set(ByVal value As XmlNode)
            Me.Node = value
        End Set
    End Property

    Public WriteOnly Property ImportName() As String
        Set(ByVal value As String)
            m_strImportName = value
        End Set
    End Property

    Public Function UpdateRemainingList(ByVal listRemoved As ListBox, ByVal listRemained As ListBox) As Boolean
        Dim RemovedArray As ArrayList = listRemoved.DataSource
        Dim RemainedArray As New ArrayList
        Dim copy As XmlNodeListView
        For Each child As XmlNodeListView In RemovedArray
            If child.CheckedView Then
                copy = New XmlNodeListView(child.Node)
                copy.Tag = Me.Tag
                RemainedArray.Add(copy)
            End If
        Next
        listRemained.DataSource = Nothing
        listRemained.Items.Clear()
        listRemained.SelectionMode = SelectionMode.One
        listRemained.DisplayMember = "FullUmlPathName"
        listRemained.ValueMember = "Id"
        listRemained.DataSource = RemainedArray
    End Function

    Public Function UpdateValues(ByVal listRemoved As ListBox, ByVal listRemained As ListBox) As Boolean
        Dim bResult As Boolean = False
        Try
            If listRemained.SelectedIndex <> -1 Then
                With CType(listRemained.SelectedItem, XmlNodeListView)
                    Dim myList As ArrayList = CType(listRemoved.DataSource, ArrayList)

                    For Each child As XmlNodeListView In myList
                        If child.CheckedView = False Then
                            If XmlProjectTools.ChangeID(child.Node, m_xmlProjectNode.Node, .Id) Then
                                bResult = True
                            End If
                            If child.RemoveMe() Then
                                bResult = True
                            End If
                        End If
                    Next
                End With
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function LoadNodes(ByVal listbox As ListBox) As Boolean
        Try
            Dim listResult As New ArrayList
            If XmlNodeListView.GetListRedundancies(m_xmlProjectNode, Me.Node, listResult) _
            Then
                ' We add current redundant node to propose user to choose wide list
                Dim xmlcpnt As XmlNodeListView = Nothing
                If m_strImportName = "" Then
                    xmlcpnt = New XmlNodeListView(Me.Node)
                Else
                    xmlcpnt = New XmlNodeListView(Me.Name + " (" + m_strImportName + ")")
                    xmlcpnt.Node = Me.Node
                End If

                xmlcpnt.Tag = Me.Tag
                listResult.Add(xmlcpnt)

                XmlNodeListView.SortNodeList(listResult)

                listbox.SelectionMode = SelectionMode.MultiSimple
                listbox.DisplayMember = "FullUmlPathName"
                listbox.ValueMember = "Id"
                listbox.DataSource = listResult
                listbox.SetSelected(0, False)
                Return True
            Else
                listbox.Items.Add("No redundancy detected.")
                listbox.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function
End Class
