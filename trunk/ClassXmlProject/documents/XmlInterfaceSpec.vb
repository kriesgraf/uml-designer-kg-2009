Imports System
Imports System.Xml
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class XmlInterfaceSpec
    Inherits XmlComposite

    Public ReadOnly Property Comment() As String
        Get
            Return "Get interface " + FullpathClassName
        End Get
    End Property

    Public ReadOnly Property FullpathClassName() As String
        Get
            Return GetFullpathDescription(Me.Node, CType(Me.Tag, ELanguage))
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

    Public Overrides Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Select Case removeNode.NodeName
                Case "property"
                    If RemoveOverridedProperty(Me, removeNode) = False Then
                        Return False
                    End If

                Case "method"
                    If RemoveOverridedMethod(Me, removeNode) = False Then
                        Return False
                    End If
            End Select

            bResult = MyBase.RemoveComponent(removeNode)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Protected Friend Overrides Function AppendNode(ByVal child As System.Xml.XmlNode) As System.Xml.XmlNode
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

    Protected Friend Overrides Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, Optional ByVal bParam As Boolean = False)
        If xmlRefNodeCounter Is Nothing Then
            Throw New Exception("Argument 'xmlRefNodeCounter' is null")
        End If

        Id = xmlRefNodeCounter.GetNewClassId()
        Name = "New_interface_" + Id
    End Sub

End Class
