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

    Protected Friend Overrides Function AppendNode(ByVal child As System.Xml.XmlNode) As System.Xml.XmlNode
        Dim before As XmlNode = Nothing
        Select Case child.Name
            Case "property"
                ' TODO: to move to the right place
                child.Attributes.ItemOf("overridable").Value = "yes"
                before = GetNode("method")

            Case "method"
                ' TODO: to move to the right place
                child.Attributes.ItemOf("implementation").Value = "abstract"
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
