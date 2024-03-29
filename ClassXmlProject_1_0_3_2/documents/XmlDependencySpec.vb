﻿Imports System
Imports System.Xml
Imports System.ComponentModel
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class XmlDependencySpec
    Inherits XmlComponent

    Private m_xmlClassNode As XmlNode = Nothing

    Public Enum EKindDependency
        Reference
        Body
        Interf
    End Enum

    Protected Const cstFullpathClassName As String = "FullpathClassName"
    Protected Const cstIdref As String = "Idref"

    <CategoryAttribute("UML design"), _
    Browsable(False), _
    DescriptionAttribute("Dependency action")> _
    Public Overrides Property Name() As String
        Get
            Return Me.Action
        End Get
        Set(ByVal value As String)
            Me.Action = value
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("C++ dependency class declaration")> _
    Public ReadOnly Property FullpathClassName() As String
        Get
            Dim nodeClass As XmlNode = GetClassNode()
            Dim eLang As ELanguage = Me.GenerationLanguage
            Dim strResult As String = GetFullpathDescription(nodeClass, eLang)
            If DEBUG_COMMANDS_ACTIVE Then strResult += " (" + eLang.ToString + ")"
            Return strResult
        End Get
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Dependency owner component id")> _
    Public ReadOnly Property ParentClassId() As String
        Get
            Return MyBase.GetAttribute("id", "parent::class")
        End Get
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Component id reference")> _
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

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Dependency action")> _
    Public Property Action() As String
        Get
            Return GetAttribute("action")
        End Get
        Set(ByVal value As String)
            SetAttribute("action", value)
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Kind of dependency")> _
    Public Property Kind() As EKindDependency
        Get
            Select Case GetAttribute("type")
                Case "reference"
                    Return EKindDependency.Reference
                Case "body"
                    Return EKindDependency.Body
                Case Else
                    Return EKindDependency.Interf
            End Select
        End Get
        Set(ByVal value As EKindDependency)
            Select Case value
                Case EKindDependency.Reference
                    SetAttribute("type", "reference")
                Case EKindDependency.Body
                    SetAttribute("type", "body")
                Case Else
                    SetAttribute("type", "interface")
            End Select
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Kind of dependency")> _
    Public Property StringKind() As String
        Get
            Return GetAttribute("type")
        End Get
        Set(ByVal value As String)
            SetAttribute("type", value)
        End Set
    End Property

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing)
        MyBase.New(xmlNode)
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            '            MyBase.SetDefaultValues(bCreateNodeNow)
            m_bCreateNodeNow = bCreateNodeNow
            StringKind = "reference"
            Action = "New_dependency"
            Idref = "class0"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Sub NotifyInsert(Optional ByVal before As XmlComponent = Nothing)
        Idref = GetFirstClassId(Me, Me.ParentClassId)
    End Sub

    Private Function GetClassNode() As XmlNode
        If m_xmlClassNode Is Nothing Then
            m_xmlClassNode = Me.GetElementById(Me.Idref)
        End If
        Return m_xmlClassNode
    End Function
End Class
