Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class XmlReferenceSpec
    Inherits XmlComponent
    Implements InterfRelation

    Public ReadOnly Property Comment() As String
        Get
            If Me.Kind = "typedef" Then
                Return "Imports typedef " + FullpathClassName
            End If
            Return "Imports class " + FullpathClassName
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

    Public Property ParentClass() As String
        Get
            Return GetAttribute("class")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("class")
            Else
                AddAttribute("class", value)
            End If
        End Set
    End Property

    Public Property Kind() As String
        Get
            Return GetAttribute("type")
        End Get
        Set(ByVal value As String)
            SetAttribute("type", value)
        End Set
    End Property

    Public Property Container() As Integer
        Get
            Return CInt(GetAttribute("container"))
        End Get
        Set(ByVal value As Integer)
            SetAttribute("container", CStr(value))
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

    Public Sub New(Optional ByRef xmlNode As Xml.XmlNode = Nothing)
        MyBase.New(xmlNode)
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            MyBase.SetDefaultValues(bCreateNodeNow)
            m_bCreateNodeNow = bCreateNodeNow
            Container = 0
            Id = "class0"
            Kind = "class"
            Package = ""
            ParentClass = ""
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
        Name = "New_reference_" + Id
    End Sub

    Public Sub AddCollaboration(ByVal strId As String) Implements InterfRelation.AddCollaboration
        If strId = "" Then
            Throw New Exception("Fails to add a collaboration to " + Me.ToString + " because Id is null")
        End If
        m_bCreateNodeNow = True
        AddAttribute("idref", strId, "collaboration")
        m_bCreateNodeNow = False
    End Sub

    Public Function RemoveCollaboration(ByVal strId As String) As Boolean Implements InterfRelation.RemoveCollaboration
        Try
            If strId = "" Then
                RemoveAllNodes("collaboration")
            Else
                Dim xpath As String = "collaboration[@idref='" + strId + "']"
                RemoveSingleNode(xpath)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
