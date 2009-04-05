Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlProjectTools

Public Class XmlClassOverrideMethodsView
    Inherits XmlClassSpec

    Private m_listArray As ArrayList = Nothing
    Private m_eImplementation As EImplementation

    Public Property CurrentClassImpl() As EImplementation
        Get
            Return m_eImplementation
        End Get
        Set(ByVal value As EImplementation)
            m_eImplementation = value
        End Set
    End Property

    Public Sub LoadMethods(ByVal listbox As ListBox)
        Try
            listbox.DisplayMember = "FullDescription"
            listbox.DataSource = m_listArray
            listbox.SelectionMode = SelectionMode.MultiSimple
            Dim i As Integer = 0
            For Each node As XmlOverrideMemberView In m_listArray
                If node.CheckedView Then
                    listbox.SetSelected(i, True)
                End If
                i += 1
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function InitListMethods(ByVal strIgnoredClasses As String) As Boolean
        Try
            m_listArray = New ArrayList
            Dim iteration As Integer = 0
            Dim nodeList As XmlNodeList
            If strIgnoredClasses = "" Then
                nodeList = SelectNodes("inherited")
            Else
                nodeList = SelectNodes("inherited[not(contains('" + strIgnoredClasses + "',concat(@idref,';')))]")
            End If
            For Each child As XmlNode In nodeList
                SelectInheritedMethods(iteration, m_eImplementation, SelectNodeId(child), m_listArray)
            Next

            Dim i As Integer = 0
            Dim jSecure As Integer = m_listArray.Count     ' Use to exit loop in case of errors
            While jSecure > 0
                jSecure -= 1
                If i < m_listArray.Count Then
                    Dim xmlcpnt As XmlOverrideMemberView = CType(m_listArray.Item(i), XmlOverrideMemberView)
                    If xmlcpnt.OverridableMember = False Then
                        m_listArray.Remove(xmlcpnt)
                    Else
                        i += 1
                    End If
                End If
            End While
        Catch ex As Exception
            Throw ex
        End Try
        Return (m_listArray.Count > 0)
    End Function

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
