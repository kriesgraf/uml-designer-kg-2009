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

    Public Sub New(Optional ByVal nodeXml As XmlNode = Nothing)
        MyBase.New(nodeXml)
    End Sub

    Public Sub LoadProperties(ByVal listbox As ListBox)
        Try
            listbox.DisplayMember = "FullDescription"
            listbox.DataSource = m_listArray
            listbox.SelectionMode = SelectionMode.MultiSimple

            Dim i As Integer = 0

            For Each nodeXml As XmlOverrideMemberView In m_listArray
                If nodeXml.CheckedView Then
                    listbox.SetSelected(i, True)
                End If
                i += 1
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function InitListProperties(ByVal strIgnoredClasses As String) As Boolean
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
                SelectInheritedProperties(iteration, m_eImplementation, SelectNodeId(child), m_listArray)
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

    Private Sub AppendVirtualProperty(ByVal virtualClassMember As XmlOverrideMemberView)
        Try
            Dim oldClassMember As XmlNode
            Dim newClassMember As XmlNode

            oldClassMember = GetNode("property[@name='" + virtualClassMember.Name + "' and @overrides='" + virtualClassMember.ClassId + "']")

            If oldClassMember Is Nothing _
            Then
                newClassMember = AppendNewProperty(virtualClassMember)
            Else
                newClassMember = ReplaceVirtualProperty(oldClassMember, virtualClassMember)
            End If

            AddAttributeValue(newClassMember, "overrides", virtualClassMember.ClassId)

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

    Private Function AppendNewProperty(ByVal virtualClassMember As XmlOverrideMemberView) As XmlNode
        Dim oldClassMember As XmlNode = GetNode("property[@name='" + virtualClassMember.Name + "']")
        Dim dstProperty As XmlNode = virtualClassMember.Node.CloneNode(True)

        Me.Node.InsertBefore(dstProperty, Me.Node.SelectSingleNode("method"))

        If oldClassMember IsNot Nothing Then
            If MsgBox("Found a property with same name '" + virtualClassMember.Name + "' that is not an overridden-kind!" + vbCrLf + _
                      "Please confirm erasing it.", cstMsgYesNoQuestion) _
                = MsgBoxResult.Yes _
            Then
                AddAttributeValue(dstProperty, "num-id", GetAttributeValue(oldClassMember, "num-id"))
                RemoveNode(oldClassMember)
            End If
        End If
        Return dstProperty
    End Function

    Private Function ReplaceVirtualProperty(ByVal srcProperty As XmlNode, ByVal virtualClassMember As XmlOverrideMemberView) As XmlNode

        Dim dstProperty As XmlNode = virtualClassMember.Node.CloneNode(True)

        ' We change only the type if necessary
        srcProperty.ReplaceChild(dstProperty.SelectSingleNode("type"), srcProperty.SelectSingleNode("type"))

        Return srcProperty
    End Function
End Class
