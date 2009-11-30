Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlProjectTools

Public Class XmlClassOverrideMethodsView
    Inherits XmlClassSpec

    Private m_list As SortedList = Nothing
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

        listbox.DisplayMember = "FullDescription"
        listbox.DataSource = m_list.GetValueList
        listbox.SelectionMode = SelectionMode.MultiSimple
        Dim i As Integer = 0
        For Each nodeXml As XmlOverrideMemberView In m_list.GetValueList
            If nodeXml.CheckedView Then
                listbox.SetSelected(i, True)
            End If
            i += 1
        Next
    End Sub

    Public Function InitListMethods(ByVal strIgnoredClasses As String) As Boolean

        m_list = New SortedList
        Dim iteration As Integer = 0
        Dim nodeList As XmlNodeList
        If strIgnoredClasses = "" Then
            nodeList = SelectNodes("inherited")
        Else
            nodeList = SelectNodes("inherited[not(contains('" + strIgnoredClasses + "',concat(@idref,';')))]")
        End If
        For Each child As XmlNode In nodeList
            SelectInheritedMethods(iteration, m_eImplementation, SelectNodeId(child), m_list)
        Next

        Dim i As Integer = 0
        Dim jSecure As Integer = m_list.Count     ' Use to exit loop in case of errors
        While jSecure > 0
            jSecure -= 1
            If i < m_list.Count Then
                Dim xmlcpnt As XmlOverrideMemberView = CType(m_list.GetValueList(i), XmlOverrideMemberView)
                If xmlcpnt.OverridableMember = False Then
                    m_list.Remove(xmlcpnt)
                Else
                    i += 1
                End If
            End If
        End While

        Return (m_list.Count > 0)
    End Function

    Public Function AddMethods(ByVal listbox As ListBox) As Boolean

        For Each element As XmlOverrideMemberView In listbox.SelectedItems()
            'Debug.Print(element.FullDescription)
            AppendVirtualMethod(element)
        Next
        Return True
    End Function

    Private Sub AppendVirtualMethod(ByVal virtualClassMember As XmlOverrideMemberView)

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
            oldClassMember = Nothing
            Dim signature As String = virtualClassMember.Signature
            Dim motherId As String = virtualClassMember.ClassId

            For Each child As XmlNode In SelectNodes("method[@name='" + virtualClassMember.Name + "' and @overrides='" + motherId + "']")
                If signature = XmlProjectTools.GetSignature(child) Then
                    oldClassMember = child
                End If
            Next

            If oldClassMember Is Nothing _
            Then
                newClassMember = AppendNewMethod(virtualClassMember)
            Else
                newClassMember = oldClassMember
            End If

            AddAttributeValue(newClassMember, "overrides", motherId)
            AddAttributeValue(newClassMember, "implementation", ConvertEnumImplToDtd(m_eImplementation))

        End If
    End Sub

    Private Sub AddUnknownParams(ByVal dstMethod As XmlNode, ByVal srcMethod As XmlNode)

        Dim child As XmlNode
        Dim xmlcpnt As XmlMethodSpec = CreateDocument(dstMethod)
        xmlcpnt.GenerationLanguage = Me.GenerationLanguage

        Dim list As XmlNodeList = SelectNodes(dstMethod, "param")

        For Each child In list
            dstMethod.RemoveChild(child)
        Next child

        For Each child In SelectNodes(srcMethod, "param")
            xmlcpnt.AppendNode(child.CloneNode(True))
        Next child

        ReorderParams(dstMethod)

    End Sub

    Private Function AppendNewMethod(ByVal virtualClassMember As XmlOverrideMemberView) As XmlNode

        Dim oldClassMember As XmlNode = GetNode("method[@name='" + virtualClassMember.Name + "' and not(@overrides)]")
        Dim dstMethod As XmlNode = virtualClassMember.Node.CloneNode(True)

        Me.Node.InsertBefore(dstMethod, Me.Node.SelectSingleNode("method"))

        If oldClassMember IsNot Nothing Then
            Dim strParams As String = ""
            For Each child As XmlNode In oldClassMember.SelectNodes("param")
                strParams += " " + GetName(child)
            Next
            strParams = GetName(oldClassMember) + "(" + strParams.Trim + ")"

            If MsgBox("Found a method with same name '" + strParams + "' that is not an overridden-kind!" + vbCrLf + _
                      "Please confirm erasing it.", cstMsgYesNoQuestion) _
                = MsgBoxResult.Yes _
            Then
                AddAttributeValue(dstMethod, "num-id", GetAttributeValue(oldClassMember, "num-id"))
                RemoveNode(oldClassMember)
            End If
        End If
        Return dstMethod
    End Function

    Private Sub ReorderParams(ByVal nodeXML As XmlNode)
        Dim child As XmlNode
        Dim numID As Integer
        numID = 1
        For Each child In SelectNodes(nodeXML, "param")
            AddAttributeValue(child, "num-id", CStr(numID))
            numID = numID + 1
        Next child
    End Sub

    Public Sub New(Optional ByVal nodeXml As XmlNode = Nothing)
        MyBase.New(nodeXml)
    End Sub
End Class
