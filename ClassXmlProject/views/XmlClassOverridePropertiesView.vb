Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlProjectTools

Public Class XmlClassOverridePropertiesView
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

    Public Sub New(Optional ByVal node As XmlNode = Nothing)
        MyBase.New(node)
    End Sub

    Public Sub LoadProperties(ByVal listbox As ListBox)
        Try
            listbox.DisplayMember = "FullDescription"
            listbox.DataSource = m_listArray
            listbox.SelectedIndex = -1

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function InitListProperties() As Boolean
        Try
            m_listArray = New ArrayList
            Dim iteration As Integer = 0
            For Each child As XmlNode In SelectNodes("inherited")
                SelectInheritedProperties(iteration, SelectNodeId(child), m_listArray)
            Next

            Dim i As Integer = 0
            While i < m_listArray.Count
                Dim xmlcpnt As XmlOverrideMemberView = CType(m_listArray.Item(i), XmlOverrideMemberView)
                If xmlcpnt.OverridableMember = False Then
                    If i = m_listArray.Count - 1 Then
                        m_listArray.Remove(xmlcpnt)
                        Exit While
                    Else
                        m_listArray.Remove(xmlcpnt)
                    End If
                Else
                    i += 1
                End If
            End While
        Catch ex As Exception
            Throw ex
        End Try
        Return (m_listArray.Count > 0)
    End Function

    Public Function AddProperties(ByVal listbox As ListBox) As Boolean
        Try
            For Each element As XmlOverrideMemberView In listbox.SelectedItems()
                'Debug.Print(element.FullDescription)
                AppendVirtualProperty(element)
            Next
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub SelectInheritedProperties(ByRef iteration As Integer, ByRef node As XmlNode, ByVal list As ArrayList)
        Try
            iteration += 1
            If iteration > cstMaxCircularReferences Then
                MsgBox("Inherited tree deepth is too big, or has circular references!", MsgBoxStyle.Critical)
                Return
            End If
            For Each child As XmlNode In SelectNodes(node, "property[@overridable='yes' or @overrides!='']")
                'Debug.Print("virtual=" + GetName(child))
                Dim xmlcpnt As XmlOverrideMemberView = New XmlOverrideMemberView(child)

                If list.Contains(xmlcpnt) = False Then
                    list.Add(xmlcpnt)
                End If
            Next child

            For Each child As XmlNode In SelectNodes(node, "inherited")
                Dim inherited As XmlNode = SelectNodeId(child, node)
                'Debug.Print("inherited=" + GetName(inherited))
                SelectInheritedProperties(iteration, inherited, list)
            Next child
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AppendVirtualProperty(ByVal virtualClassMember As XmlOverrideMemberView)
        Try
            Dim oldClassMember As XmlNode
            Dim newClassMember As XmlNode

            oldClassMember = GetNode("property[@name='" + virtualClassMember.Name + "']")

            newClassMember = virtualClassMember.Node.CloneNode(True)

            AddAttributeValue(newClassMember, "overrides", virtualClassMember.ClassId)

            If oldClassMember Is Nothing _
            Then
                Me.AppendNode(newClassMember)
            Else
                ReplaceVirtualProperty(oldClassMember, newClassMember)
            End If

            Select Case m_eImplementation
                Case EImplementation.Leaf
                    AddAttributeValue(newClassMember, "overridable", "no")

                Case EImplementation.Root, EImplementation.Node
                    AddAttributeValue(newClassMember, "overridable", "yes")

                Case Else
                    AddAttributeValue(newClassMember, "overridable", "no")
            End Select

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ReplaceVirtualProperty(ByVal dstProperty As XmlNode, ByVal srcProperty As XmlNode)
        Dim child As XmlNode
        Dim xmlcpnt As XmlPropertySpec = CreateDocument(dstProperty)
        Dim list As XmlNodeList = SelectNodes(dstProperty, "param")

        dstProperty.RemoveAll()

        For Each child In srcProperty.ChildNodes
            xmlcpnt.AppendNode(child.CloneNode(True))
        Next child
    End Sub
End Class
