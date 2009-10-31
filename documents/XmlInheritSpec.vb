Imports System
Imports System.Xml
Imports System.ComponentModel
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class XmlInheritSpec
    Inherits XmlComponent

    Private m_xmlClassNode As XmlNode = Nothing

    <CategoryAttribute("UML design"), _
    Browsable(False), _
    DescriptionAttribute("Inherited class name")> _
    Public Overrides Property Name() As String
        Get
            Dim parent As XmlNode = GetClassNode()

            If parent IsNot Nothing Then
                Dim attrib As XmlAttribute = parent.Attributes.ItemOf("name")
                If attrib IsNot Nothing Then Return attrib.Value
            End If
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
            Dim parent As XmlNode = GetClassNode()

            Dim eLang As ELanguage = Me.GenerationLanguage
            Dim strResult As String = GetFullpathDescription(parent, eLang)
            If DEBUG_COMMANDS_ACTIVE Then strResult += " (" + eLang.ToString + ")"
            Return strResult
        End Get
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Mother class implementation")> _
    Public ReadOnly Property Implementation() As EImplementation
        Get
            Return ConvertDtdToEnumImpl(GetImplementation())
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
            m_xmlClassNode = Nothing
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

    Private Function GetImplementation() As String
        Dim strResult As String = Nothing

        Dim parent As XmlNode = GetClassNode()

        If parent IsNot Nothing Then
            Select Case m_xmlClassNode.Name
                Case "class"
                    strResult = GetAttributeValue(parent, "implementation")

                Case "interface"
                    If GetAttributeValue(parent, "root") = "yes" _
                    Then
                        strResult = "root"
                    Else
                        strResult = "abstract"
                    End If

                Case Else
                    strResult = "simple"
            End Select
        End If

        Return strResult
    End Function

    Private Function GetClassNode() As XmlNode
        If m_xmlClassNode Is Nothing Then
            m_xmlClassNode = Me.GetElementById(Me.Idref)
        End If
        Return m_xmlClassNode
    End Function
End Class
