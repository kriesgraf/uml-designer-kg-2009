﻿Imports System
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlReferenceNodeCounter
Imports System.ComponentModel
Imports System.Xml

Public Class XmlPropertySpec
    Inherits XmlComponent

#Region "Class declarations"

    Private m_xmlType As XmlTypeVarSpec
#End Region

#Region "Properties (Adapter pattern)"

    <CategoryAttribute("Code generation"), _
    DescriptionAttribute("Project language")> _
    Public ReadOnly Property GenerationLanguage() As ELanguage
        Get
            Return CType(MyBase.Tag, ELanguage)
        End Get
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("VB.NET property declaration")> _
    Public Property Behaviour() As String
        Get
            Dim strResult As String = GetAttribute("behaviour")
            If strResult Is Nothing Then
                strResult = "Normal"
            End If
            Return strResult
        End Get
        Set(ByVal value As String)
            If value <> "Normal" Then
                AddAttribute("behaviour", value)
            Else
                RemoveAttribute("behaviour")
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Comment")> _
    Public Property Comment() As String
        Get
            Return GetNodeString("comment")
        End Get
        Set(ByVal value As String)
            SetNodeString("comment", value)
        End Set
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Component member id")> _
    Public Property NumId() As Integer
        Get
            Return CInt(GetAttribute("num-id"))
        End Get
        Set(ByVal value As Integer)
            SetAttribute("num-id", value.ToString)
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Full path type description")> _
    Public ReadOnly Property FullpathTypeDescription() As String
        Get
            Return m_xmlType.FullpathTypeDescription
        End Get
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Type node")> _
    Public ReadOnly Property TypeVarDefinition() As XmlTypeVarSpec
        Get
            If m_xmlType IsNot Nothing Then
                m_xmlType.Tag = Me.Tag
            End If
            Return m_xmlType
        End Get
    End Property

    <TypeConverter(GetType(UmlMemberVisibility)), _
    CategoryAttribute("UML design"), _
   DescriptionAttribute("Member visibility")> _
    Public Property Range() As String
        Get
            Return m_xmlType.Range
        End Get
        Set(ByVal value As String)
            m_xmlType.Range = value
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberVisibility)), _
    CategoryAttribute("UML design"), _
   DescriptionAttribute("Get accessor visibility")> _
    Public Property AccessGetRange() As String
        Get
            Return GetAttribute("range", "get")
        End Get
        Set(ByVal value As String)
            SetAttribute("range", value, "get")
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberValue)), _
    CategoryAttribute("UML design"), _
   DescriptionAttribute("Member access by")> _
    Public Property AccessGetBy() As String
        Get
            Return GetAttribute("by", "get")
        End Get
        Set(ByVal value As String)
            SetAttribute("by", value, "get")
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Get accessor const value")> _
    Public Property AccessGetModifier() As Boolean
        Get
            Return (GetAttribute("modifier", "get") = "const")
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("modifier", "const", "get")
            Else
                SetAttribute("modifier", "var", "get")
            End If
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberVisibility)), _
    CategoryAttribute("UML design"), _
   DescriptionAttribute("Set accessor visibility")> _
    Public Property AccessSetRange() As String
        Get
            Return GetAttribute("range", "set")
        End Get
        Set(ByVal value As String)
            SetAttribute("range", value, "set")
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberValue)), _
    CategoryAttribute("UML design"), _
   DescriptionAttribute("Member access by")> _
    Public Property AccessSetBy() As String
        Get
            Return GetAttribute("by", "set")
        End Get
        Set(ByVal value As String)
            SetAttribute("by", value, "set")
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberClass)), _
    CategoryAttribute("UML design"), _
   DescriptionAttribute("Class member")> _
    Public Property Member() As String
        Get
            Return GetAttribute("member")
        End Get
        Set(ByVal value As String)
            SetAttribute("member", value)
        End Set
    End Property
#End Region

#Region "Constructor/Destructor"

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode)
        Try
            ChangeReferences(bLoadChildren)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Public methods"

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            ' This method reset m_bCreateNodeNow !
            MyBase.SetDefaultValues(bCreateNodeNow)

            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()
            m_xmlType.SetDefaultValues(bCreateNodeNow)
            m_xmlType.Descriptor = "int16"
            m_xmlType.Range = "private"

            ' Range is initialized in class XmlTypeVarSpec, see m_xmlType member
            Member = "object"
            NumId = 0
            Comment = "Insert here a comment"

            AccessGetBy = "val"
            AccessGetModifier = False
            AccessGetRange = "public"
            AccessSetBy = "val"
            AccessSetRange = "no"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Sub NotifyInsert(Optional ByVal before As XmlComponent = Nothing)
        NumId = CInt(GenerateNumericId(Me.Node.ParentNode, "property"))
        Name = "New_property" + CStr(NumId)
    End Sub
#End Region

#Region "Protected methods"

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        Try
            MyBase.ChangeReferences(bLoadChildren)

            Dim nodeXml As XmlNode
            If TestNode("type") = False And m_bCreateNodeNow Then
                nodeXml = CreateAppendNode("type")
            Else
                nodeXml = GetNode("type")
            End If
            m_xmlType = TryCast(CreateDocument(nodeXml, bLoadChildren), XmlTypeVarSpec)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
End Class

