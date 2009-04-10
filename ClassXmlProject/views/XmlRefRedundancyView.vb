Imports System
Imports System.Windows.Forms
Imports System.Xml
Imports System.Collections

Public Class XmlRefRedundancyView
    Inherits XmlComponent

    Public WriteOnly Property Redundant() As XmlNode
        Set(ByVal value As XmlNode)
            Me.Node = value
        End Set
    End Property

    Public Function UpdateValues(ByVal listBox As ListBox) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim list As XmlNodeList
            Dim child As XmlNode
            Dim strNewID As String

            If listBox.SelectedIndex <> -1 Then

                strNewID = CType(listBox.SelectedItem(), XmlNodeListView).Id

                list = SelectNodes("//*[@idref='" + XmlProjectTools.GetID(Me.Node) + "']")

                For Each child In list
                    XmlProjectTools.AddAttributeValue(child, "idref", strNewID)
                Next child

                Dim xlmcpnt As XmlComposite = CreateDocument(Me.Node.ParentNode.ParentNode)
                bResult = xlmcpnt.RemoveComponent(Me)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Sub LoadNodes(ByVal listbox As ListBox)
        Try
            Dim listResult As New ArrayList
            If XmlNodeListView.GetListReferences(Me, listResult) _
            Then
                listbox.SelectionMode = SelectionMode.One
                listbox.DisplayMember = "FullUmlPathName"
                listbox.ValueMember = "Id"
                listbox.DataSource = listResult
            Else
                listbox.Items.Add("No redundancy detected.")
                listbox.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
