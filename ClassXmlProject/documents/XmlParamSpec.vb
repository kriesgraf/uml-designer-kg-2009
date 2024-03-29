﻿Imports System
Imports System.ComponentModel
Imports ClassXmlProject.XmlReferenceNodeCounter
Imports System.Xml

Public Class XmlParamSpec
    Inherits XmlComponent

    Private m_xmlType As XmlTypeVarSpec = Nothing

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Full path type description")> _
    Public ReadOnly Property FullpathTypeDescription() As String
        Get
            Return TypeVarDefinition.FullpathTypeDescription
        End Get
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Type node")> _
    Public ReadOnly Property TypeVarDefinition() As XmlTypeVarSpec
        Get
            If m_xmlType IsNot Nothing Then
                m_xmlType.GenerationLanguage = Me.GenerationLanguage
            End If
            Return m_xmlType
        End Get
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
    Public Property NumId() As String
        Get
            Return GetAttribute("num-id")
        End Get
        Set(ByVal value As String)
            SetAttribute("num-id", value)
        End Set
    End Property

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode)

        ChangeReferences(bLoadChildren)

    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            ' This method reset m_bCreateNodeNow !
            MyBase.SetDefaultValues(bCreateNodeNow)

            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()
            Me.TypeVarDefinition.SetDefaultValues(bCreateNodeNow)
            Me.TypeVarDefinition.Descriptor = "int16"

            ' Range is initialized in class XmlTypeVarSpec, see m_xmlType member
            NumId = "0"
            Comment = "Insert here a comment"

        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Sub NotifyInsert(Optional ByVal before As XmlComponent = Nothing)
        NumId = GenerateNumericId(Me.Node.ParentNode, "param")
        Name = "New_param" + CStr(NumId)
    End Sub

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)

        MyBase.ChangeReferences(bLoadChildren)

        Dim nodeXml As XmlNode
        If TestNode("type") = False And m_bCreateNodeNow Then
            nodeXml = CreateAppendNode("type")
        Else
            nodeXml = GetNode("type")
        End If

        m_xmlType = TryCast(CreateDocument(nodeXml, bLoadChildren), XmlTypeVarSpec)
        If m_xmlType IsNot Nothing Then m_xmlType.GenerationLanguage = Me.GenerationLanguage

    End Sub
End Class

