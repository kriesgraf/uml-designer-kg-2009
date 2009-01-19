Imports System.Xml
Imports ClassXmlProject.XmlProjectTools

Public Class XmlClassOverrideMethodsView
    Inherits XmlClassSpec

    Public Sub InitListMethods(ByVal listbox As ListBox)
        Try
            Dim list As New ArrayList
            For Each child As XmlNode In SelectNodes("inherited")
                SelectInheritedMethods(SelectNodeId(child), list)
            Next
            listbox.DisplayMember = "FullDescription"
            listbox.DataSource = list
            listbox.SelectedIndex = -1

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function AddMethods(ByVal listbox As ListBox) As Boolean
        Try
            For Each element As XmlOverrideMemberView In listbox.SelectedItems()
                'Debug.Print(element.FullDescription)
                AppendVirtualMethod(element)
            Next
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub SelectInheritedMethods(ByRef node As XmlNode, ByVal list As ArrayList)
        Try
            For Each child In SelectNodes(node, "method[@constructor!='no' or @implementation='virtual' or @implementation='root' or @implementation='abstract']")
                'Debug.Print("virtual=" + GetName(child))
                Dim xmlcpnt As XmlOverrideMemberView = New XmlOverrideMemberView(child)

                If list.Contains(xmlcpnt) = False Then
                    list.Add(xmlcpnt)
                End If
            Next child

            For Each child In SelectNodes(node, "inherited")
                Dim inherited As XmlNode = SelectNodeId(child, node)
                'Debug.Print("inherited=" + GetName(inherited))
                SelectInheritedMethods(inherited, list)
            Next child
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AppendVirtualMethod(ByVal virtualClassMember As XmlOverrideMemberView)
        Try
            Dim oldClassMember As XmlNode
            Dim newClassMember As XmlNode

            If virtualClassMember.GetAttribute("constructor") <> "no" _
            Then
                oldClassMember = GetNode("method[@num-id='" + virtualClassMember.NumID + "']")

                If oldClassMember Is Nothing _
                        Then
                    newClassMember = virtualClassMember.Node.CloneNode(True)
                    Me.AppendNode(newClassMember)
                Else
                    AddUnknownParams(oldClassMember, virtualClassMember.Node)
                End If
            Else
                oldClassMember = GetNode("method[@name='" + virtualClassMember.Name + "']")

                newClassMember = virtualClassMember.Node.CloneNode(True)

                AddAttributeValue(newClassMember, "overrides", virtualClassMember.ClassId)

                If oldClassMember Is Nothing _
                Then
                    Me.AppendNode(newClassMember)
                Else
                    ReplaceVirtualMethod(oldClassMember, newClassMember)
                End If

                AddAttributeValue(newClassMember, "implementation", ConvertEnumImplToDtd(Me.Implementation))
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AddUnknownParams(ByVal dstMethod As XmlNode, ByVal srcMethod As XmlNode)
        Try
            Dim child As XmlNode

            For Each child In SelectNodes(srcMethod, "param")
                If GetNode(dstMethod, "param[@name='" + GetName(child) + "']") Is Nothing Then
                    dstMethod.AppendChild(child.CloneNode(True))
                End If
            Next child

            ReorderParams(dstMethod)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ReplaceVirtualMethod(ByVal dstMethod As XmlNode, ByVal srcMethod As XmlNode)
        Dim child As XmlNode

        For Each child In SelectNodes(dstMethod, "param")
            dstMethod.RemoveChild(child)
        Next child

        For Each child In SelectNodes(srcMethod, "param")
            dstMethod.AppendChild(child.CloneNode(True))
        Next child

        ReorderParams(dstMethod)

    End Sub

    Private Sub ReorderParams(ByVal nodeXML As XmlNode)
        Dim child As XmlNode
        Dim numID As Integer
        numID = 1
        For Each child In SelectNodes(nodeXML, "param")
            AddAttributeValue(child, "num-id", CStr(numID))
            numID = numID + 1
        Next child

    End Sub

    Public Sub New(Optional ByVal node As XmlNode = Nothing)
        MyBase.New(node)
    End Sub
End Class
