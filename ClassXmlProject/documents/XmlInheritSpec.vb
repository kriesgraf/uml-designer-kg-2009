Imports System
Imports System.Xml
Imports System.ComponentModel
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class XmlInheritSpec
    Inherits XmlComponent

    <CategoryAttribute("UML design"), _
    Browsable(False), _
    DescriptionAttribute("Inherited class name")> _
    Public Overrides Property Name() As String
        Get
            Dim attrib As XmlAttribute = GetElementById(Me.Idref).Attributes.GetNamedItem("name")
            If attrib IsNot Nothing Then Return attrib.Value
            Return Nothing
        End Get
        Set(ByVal value As String)
            ' No update
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("C++ mother class declaration")> _
    Public ReadOnly Property FullpathClassName() As String
        Get
            Dim parent As XmlNode = MyBase.GetElementById(Me.Idref)
            Return GetFullpathDescription(parent, CType(Me.Tag, ELanguage))
        End Get
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Mother class implementation")> _
    Public ReadOnly Property Implementation() As EImplementation
        Get
            Return ConvertDtdToEnumImpl(GetImplementation(Me.Idref))
        End Get
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Mother class component id")> _
    Public Property Idref() As String
        Get
            Return GetAttribute("idref")
        End Get
        Set(ByVal value As String)
            ValidateIdReference(value, m_bCreateNodeNow)
            SetAttribute("idref", value)
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberVisibility)), _
    CategoryAttribute("UML design"), _
    DescriptionAttribute("Inheritance visibility")> _
    Public Property Range() As String
        Get
            Return GetAttribute("range")
        End Get
        Set(ByVal value As String)
            SetAttribute("range", value)
        End Set
    End Property

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing)
        MyBase.New(xmlNode)
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow
            Range = "public"
            Idref = "class0"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Protected Friend Function GetImplementation(ByVal strIdref As String) As String
        Dim strResult As String = "<unknown>"
        Dim nodeClass As XmlNode = Me.GetElementById(strIdref)
        If nodeClass IsNot Nothing Then
            If nodeClass.Name = "class" Then
                strResult = GetAttributeValue(nodeClass, "implementation")
            Else
                strResult = "<import>"
            End If
        End If
        Return strResult
    End Function
End Class
