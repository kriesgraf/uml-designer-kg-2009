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
                If Me.NodeName = "reference" Or Me.NodeName = "interface" Then
                    Dim xlmcpnt As XmlImportSpec = CreateDocument(Me.Node.ParentNode.ParentNode)
                    xlmcpnt.RemoveReference(Me)
                Else
                    XmlProjectTools.RemoveNode(Me.Node)
                End If
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Sub LoadNodes(ByVal listbox As ListBox)
        Try
            Dim listResult As ArrayList = GetListReferences()

            If listResult IsNot Nothing Then
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

    Public Function GetListReferences() As ArrayList
        Dim listResult As ArrayList = Nothing
        Try
            Dim strName As String = Me.Name
            Dim strID As String = XmlProjectTools.GetID(Me.Node)

            Dim listNodes As XmlNodeList = SelectNodes("//*[not(self::root) and @name='" + strName + "' and @id!='" + strID + "']")

            If listNodes.Count > 0 Then
                listResult = New ArrayList

                Dim iterator As IEnumerator = listNodes.GetEnumerator()
                iterator.Reset()

                While iterator.MoveNext()
                    Dim xmlcpnt As New XmlNodeListView(CType(iterator.Current, XmlNode))
                    xmlcpnt.Tag = Me.Tag
                    listResult.Add(xmlcpnt)
                End While
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return listResult
    End Function
End Class
