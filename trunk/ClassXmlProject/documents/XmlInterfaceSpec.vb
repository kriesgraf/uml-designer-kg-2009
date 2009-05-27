Imports System
Imports System.Xml
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class XmlInterfaceSpec
    Inherits XmlComposite
    Implements InterfNodeCounter

    Protected m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

    Public ReadOnly Property Comment() As String
        Get
            Return "Get interface " + FullpathClassName
        End Get
    End Property

    Public ReadOnly Property FullpathClassName() As String
        Get
            Dim eLang As ELanguage = CType(Me.Tag, ELanguage)
            Dim strResult As String = GetFullpathDescription(Me.Node, eLang)
            If DEBUG_COMMANDS_ACTIVE Then strResult += " (" + eLang.ToString + ")"
            Return strResult
        End Get
    End Property

    Public Property Id() As String
        Get
            Return GetAttribute("id")
        End Get
        Set(ByVal value As String)
            SetAttribute("id", value)
        End Set
    End Property

    Public ReadOnly Property Implementation() As EImplementation
        Get
            If Root Then
                Return EImplementation.Root
            End If
            Return EImplementation.Interf
        End Get
    End Property

    Public Property Root() As Boolean
        Get
            Return CheckAttribute("root", "yes", "no")
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("root", "yes")
            Else
                SetAttribute("root", "no")
            End If
        End Set
    End Property

    Public Property Package() As String
        Get
            Return GetAttribute("package")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("package")
            Else
                AddAttribute("package", value)
            End If
        End Set
    End Property

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing)
        MyBase.New(xmlNode)
    End Sub

    Public Overrides Function CanAddComponent(ByVal nodeXml As XmlComponent) As Boolean
        Select Case nodeXml.NodeName
            Case "property"
                With CType(nodeXml, XmlPropertySpec)
                    .OverridableProperty = True
                    If Me.CheckAttribute("root", "no", "no") _
                    Then
                        .MemberAttribute = False
                    End If
                End With

            Case "method"
                With CType(nodeXml, XmlMethodSpec)
                    If Me.CheckAttribute("root", "yes", "no") _
                    Then
                        .Implementation = XmlProjectTools.EImplementation.Root
                    Else
                        .Implementation = XmlProjectTools.EImplementation.Interf
                    End If
                End With
        End Select
        Return True
    End Function

    Public Overrides Function CanRemove(ByVal removeNode As XmlComponent) As Boolean
        Try
            Select Case removeNode.NodeName
                Case "property"
                    If CanRemoveOverridedProperty(Me, removeNode) Then
                        Return True
                    End If

                Case "method"
                    If CanRemoveOverridedMethod(Me, removeNode) Then
                        Return True
                    End If

                Case Else
                    Return MyBase.CanRemove(removeNode)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Protected Friend Overrides Function AppendNode(ByVal child As System.Xml.XmlNode, Optional ByVal observer As Object = Nothing) As System.Xml.XmlNode
        Dim before As XmlNode = Nothing
        Select Case child.Name
            Case "collaboration"
                before = GetNode("property")
                If before Is Nothing Then
                    before = GetNode("method")
                End If

            Case "property"
                before = GetNode("method")
        End Select

        If before Is Nothing Then
            Return Me.Node.AppendChild(child)
        Else
            Return Me.Node.InsertBefore(child, before)
        End If
    End Function

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            MyBase.SetDefaultValues(bCreateNodeNow)
            m_bCreateNodeNow = bCreateNodeNow
            Id = "class0"
            Package = ""
            Root = False

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Protected Friend Overrides Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, _
                                                    Optional ByVal eRename As ENameReplacement = ENameReplacement.NewName, _
                                                    Optional ByVal bSetIdrefChildren As Boolean = False)
        If xmlRefNodeCounter Is Nothing Then
            Throw New Exception("Argument 'xmlRefNodeCounter' is null")
        End If

        Me.Id = xmlRefNodeCounter.GetNewClassId()
        Dim strID As String = XmlNodeCounter.AfterStr(Me.Id, "class")

        Select Case eRename
            Case ENameReplacement.NewName
                Name = "New_interface_" + strID
            Case ENameReplacement.AddCopyName
                ' Name is set by caller
                Name = Name + "_" + strID
        End Select

        ' Use this option only to paste this node from another project
        If bSetIdrefChildren Then
            Me.RemoveAllNodes("collaboration")
            ' Change idref for attributes: type/@idref or list/@idref and list/@index-ref
            For Each unref As XmlNode In Me.SelectNodes("descendant::*/@idref")
                unref.Value = Me.Id       ' We change to this arbitray ID to avoid error. We let user to change it himself
            Next
        End If
    End Sub
End Class
