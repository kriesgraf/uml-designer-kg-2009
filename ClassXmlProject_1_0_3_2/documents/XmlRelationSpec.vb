﻿Imports System
Imports System.ComponentModel
Imports System.Xml
Imports ClassXmlProject.XmlProjectTools

Public Class XmlRelationSpec
    Inherits XmlComponent

    Public Enum EKindRelation
        Composition
        Aggregation
        Assembly
    End Enum

    Private m_xmlFather As XmlRelationParentSpec = Nothing
    Private m_xmlChild As XmlRelationParentSpec = Nothing

    Protected Friend ReadOnly Property Father() As XmlRelationParentSpec
        Get
            If m_xmlFather IsNot Nothing Then
                m_xmlFather.GenerationLanguage = Me.GenerationLanguage
            End If
            Return m_xmlFather
        End Get
    End Property

    Protected Friend ReadOnly Property Child() As XmlRelationParentSpec
        Get
            If m_xmlChild IsNot Nothing Then
                m_xmlChild.GenerationLanguage = Me.GenerationLanguage
            End If
            Return m_xmlChild
        End Get
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Relation description")> _
    Public ReadOnly Property Comment() As String
        Get
            Return "(" + Me.Father.StringCardinal + ")" + Me.Father.FullpathClassName + _
                    " --> " + Me.Child.FullpathClassName + "(" + Me.Child.StringCardinal + ")"
        End Get
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Component id")> _
    Public Property Id() As String
        Get
            Return GetAttribute("id")
        End Get
        Set(ByVal value As String)
            SetAttribute("id", value)
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    Browsable(False), _
    DescriptionAttribute("Relation action")> _
    Public Overrides Property Name() As String
        Get
            Return Me.Action
        End Get
        Set(ByVal value As String)
            Me.Action = value
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Relation action")> _
    Public Property Action() As String
        Get
            Return GetAttribute("action")
        End Get
        Set(ByVal value As String)
            SetAttribute("action", value)
        End Set
    End Property

    Public Property Kind() As EKindRelation
        Get
            Dim eResult As EKindRelation
            Select Case GetAttribute("type")
                Case "assembl"
                    eResult = EKindRelation.Assembly
                Case "aggreg"
                    eResult = EKindRelation.Aggregation
                Case "comp"
                    eResult = EKindRelation.Composition
                Case Else
                    eResult = EKindRelation.Assembly
            End Select
            Return eResult
        End Get
        Set(ByVal value As EKindRelation)
            Dim strResult As String
            Select Case value
                Case EKindRelation.Assembly
                    strResult = "assembl"
                Case EKindRelation.Aggregation
                    strResult = "aggreg"
                Case EKindRelation.Composition
                    strResult = "comp"
                Case Else
                    strResult = "assembl"
            End Select
            SetAttribute("type", strResult)
        End Set
    End Property

    Public ReadOnly Property Reference(ByVal strId As String) As XmlComponent
        Get
            If strId = Me.Father.Idref _
            Then
                Return Me.Child

            ElseIf strId = Me.Child.Idref _
            Then
                Return Me.Father
            Else
                Throw New Exception("Reference id '" + strId + "' is unknown")
            End If
        End Get
    End Property

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing)
        MyBase.New(xmlNode)
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow
            ChangeReferences(False)

            Me.Father.SetDefaultValues(bCreateNodeNow)
            ' Set an existing class element
            Dim strFatherID As String = GetFirstClassId(Me)
            Me.Father.Idref = strFatherID
            Me.Child.SetDefaultValues(bCreateNodeNow)
            ' Set an existing class element that could be father
            Dim strChildID As String = GetFirstClassId(Me, strFatherID)
            If strChildID.Length = 0 Then
                Me.Child.Idref = GetFirstClassId(Me)
            Else
                Me.Child.Idref = strChildID
            End If
            Id = "relation0"
            Action = "New_Action"
            Kind = EKindRelation.Assembly

        Catch ex As Exception
            ' Debug.Print(ex.StackTrace)
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

        Me.Id = XmlComponent.UID
        Dim strIndex As String = xmlRefNodeCounter.GetNewRelationId()
        Name = "New_" + Me.NodeName + strIndex
        Me.Father.Name = "New_Father_" + strIndex
        Me.Child.Name = "New_Child_" + strIndex
    End Sub

    Public Overrides Sub NotifyInsert(Optional ByVal before As XmlComponent = Nothing)
        XmlProjectTools.UpdateOneCollaboration(Me.Document, Me.Father.Idref)
        XmlProjectTools.UpdateOneCollaboration(Me.Document, Me.Child.Idref)
    End Sub

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        Try
            MyBase.ChangeReferences(bLoadChildren)
            Dim nodeXml As XmlNode = GetNode("father")

            If nodeXml Is Nothing And m_bCreateNodeNow Then
                nodeXml = CreateAppendNode("father")
            End If
            m_xmlFather = CreateDocument(nodeXml, bLoadChildren)
            If m_xmlFather IsNot Nothing Then m_xmlFather.GenerationLanguage = Me.GenerationLanguage

            nodeXml = GetNode("child")
            If nodeXml Is Nothing And m_bCreateNodeNow Then
                nodeXml = CreateAppendNode("child")
            End If
            m_xmlChild = CreateDocument(nodeXml, bLoadChildren)
            If m_xmlChild IsNot Nothing Then m_xmlChild.GenerationLanguage = Me.GenerationLanguage

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Overrides Function RemoveMe() As Boolean
        Dim strFatherID As String = Me.Father.Idref
        Dim strChildID As String = Me.Child.Idref
        If MyBase.RemoveMe() Then
            XmlProjectTools.UpdateOneCollaboration(Me.Document, strFatherID)
            If strFatherID <> strChildID Then
                XmlProjectTools.UpdateOneCollaboration(Me.Document, strChildID)
            End If
            Return True
        End If
        Return False
    End Function
End Class
