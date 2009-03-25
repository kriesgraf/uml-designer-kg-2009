Imports System
Imports System.Xml
Imports System.ComponentModel
Imports ClassXmlProject.XmlReferenceNodeCounter

Public Class XmlElementSpec
    Inherits XmlComponent

#Region "Properties"

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Component class id")> _
    Public Property Reference() As String
        Get
            Return GetAttribute("idref")
        End Get
        Set(ByVal value As String)
            ValidateIdReference(value, m_bCreateNodeNow)
            ReplaceAttribute("desc", "idref")
            SetAttribute("idref", value)
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Simple type description")> _
    Public Property Descriptor() As String
        Get
            Return GetAttribute("desc")
        End Get
        Set(ByVal value As String)
            ReplaceAttribute("idref", "desc")
            SetAttribute("desc", value)
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Const element")> _
    Public Property Modifier() As Boolean
        Get
            Return (GetAttribute("modifier") = "const")
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("modifier", "const")
            Else
                SetAttribute("modifier", "var")
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("C++ pointer declaration")> _
    Public Property Level() As Integer
        Get
            Return CInt(GetAttribute("level"))
        End Get
        Set(ByVal value As Integer)
            SetAttribute("level", CStr(value))
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Brief comment")> _
    Public Property Comment() As String
        Get
            If MyBase.Node IsNot Nothing Then
                Return MyBase.Node.InnerText
            End If
            Return ""
        End Get
        Set(ByVal value As String)
            MyBase.Node.InnerText = value
        End Set
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Component element id")> _
    Public Property NumId() As String
        Get
            Return GetAttribute("num-id")
        End Get
        Set(ByVal value As String)
            SetAttribute("num-id", value)
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Numeric array size")> _
    Public Property VarSize() As String
        Get
            Return GetAttribute("size")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("size")
                RemoveAttribute("sizeref")
            Else
                ReplaceAttribute("sizeref", "size")
                SetAttribute("size", value)
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Reference constant array size")> _
    Public Property SizeRef() As String
        Get
            Return GetAttribute("sizeref")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("size")
                RemoveAttribute("sizeref")
            Else
                ReplaceAttribute("size", "sizeref")
                SetAttribute("sizeref", value)
            End If
        End Set
    End Property

#End Region

#Region "Public methods"

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing)
        MyBase.New(xmlNode)
    End Sub

    Public Overrides Sub NotifyInsert(Optional ByVal before As XmlComponent = Nothing)
        NumId = GenerateNumericId(Me.Node.ParentNode, "element")
        Name = "New_element" + CStr(NumId)
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow
            ChangeReferences()
            Name = "NewElement"
            NumId = "0"
            Descriptor = "int16"
            Level = 0
            Modifier = False
            Comment = "Insert brief comment"
        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

#End Region
End Class

