Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class XmlClassOverrideMethodsView
    Inherits XmlClassSpec

    Private m_eImplementation As EImplementation

    Public Property CurrentClassImpl() As EImplementation
        Get
            Return m_eImplementation
        End Get
        Set(ByVal value As EImplementation)
            m_eImplementation = value
        End Set
    End Property

    Public Sub InitListMethods(ByVal listbox As ListBox)
        Try
            Dim list As New ArrayList
            For Each child As XmlNode In SelectNodes("inherited")
                SelectInheritedMethods(SelectNodeId(child), list)
            Next

            Dim i As Integer = 0
            While i < list.Count
                Dim xmlcpnt As XmlOverrideMemberView = CType(list.Item(i), XmlOverrideMemberView)
                If xmlcpnt.OverridableMember = False Then
                    If i = list.Count - 1 Then
                        list.Remove(xmlcpnt)
                        Exit While
                    Else
                        list.Remove(xmlcpnt)
                    End If
                Else
                    i += 1
                End If
            End While

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
            ' We add "final" methods to be sure to avoid adding method from "root" node
            For Each child In SelectNodes(node, "method[@constructor!='no' or @implementation='final' or @implementation='virtual' or @implementation='root' or @implementation='abstract']")
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

                AddAttributeValue(newClassMember, "implementation", ConvertEnumImplToDtd(m_eImplementation))

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AddUnknownParams(ByVal dstMethod As XmlNode, ByVal srcMethod As XmlNode)
        Try
            Dim child As XmlNode
            Dim xmlcpnt As XmlMethodSpec = CreateDocument(dstMethod)
            Dim list As XmlNodeList = SelectNodes(dstMethod, "param")

            For Each child In list
                dstMethod.RemoveChild(child)
            Next child

            For Each child In SelectNodes(srcMethod, "param")
                xmlcpnt.AppendNode(child.CloneNode(True))
            Next child

            ReorderParams(dstMethod)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ReplaceVirtualMethod(ByVal dstMethod As XmlNode, ByVal srcMethod As XmlNode)
        Dim child As XmlNode
        Dim xmlcpnt As XmlMethodSpec = CreateDocument(dstMethod)
        Dim list As XmlNodeList = SelectNodes(dstMethod, "param")

        For Each child In list
            dstMethod.RemoveChild(child)
        Next child

        For Each child In SelectNodes(srcMethod, "param")
            xmlcpnt.AppendNode(child.CloneNode(True))
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
