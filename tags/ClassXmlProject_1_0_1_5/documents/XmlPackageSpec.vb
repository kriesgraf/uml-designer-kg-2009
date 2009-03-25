﻿Imports System
Imports System.ComponentModel
Imports System.Xml

Public Class XmlPackageSpec
    Inherits XmlComposite
    Implements InterfNodeCounter

    Protected m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

#Region "Properties"

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    Browsable(False), _
   DescriptionAttribute("Check location folder")> _
    Public ReadOnly Property IsFolder() As Boolean
        Get
            Return (GetAttribute("folder") <> "")
        End Get
    End Property


    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Source location folder. When empty same as name.")> _
    Public Property Folder() As String
        Get
            Dim strResult As String = GetAttribute("folder")
            'If strResult = "" Then
            '    strResult = Me.Name
            'End If
            Return strResult
        End Get
        Set(ByVal value As String)
            If value = "" Or value = Me.Name Then
                RemoveAttribute("folder")
            Else
                AddAttribute("folder", value)
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Brief comment")> _
    Public Property BriefComment() As String
        Get
            Return GetAttribute("brief", "comment")
        End Get
        Set(ByVal value As String)
            SetAttribute("brief", value, "comment")
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Detail comment")> _
    Public Property Comment() As String
        Get
            Return GetNodeString("comment")
        End Get
        Set(ByVal value As String)
            SetNodeString("comment", value)
        End Set
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

#End Region

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode, bLoadChildren)
    End Sub

    Public Overrides Function CanPasteItem(ByVal child As XmlComponent) As Boolean
        Select Case child.NodeName
            Case "class", "import", "package"
                Return True

            Case Else
                Return MyBase.CanPasteItem(child)
        End Select
        Return MyBase.CanPasteItem(child)
    End Function

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()
            Name = "New_package"
            Id = "package0"
            Comment = "Insert here details"
            BriefComment = "Insert here a brief comment"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Sub LoadChildrenList(Optional ByVal strViewName As String = "")
        Try
            AddChildren(SelectNodes("*[name()!='comment']"), strViewName)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Overrides Function AppendNode(ByVal child As XmlNode) As XmlNode
        Dim before As XmlNode = Nothing
        Select Case child.Name
            Case "import"
                before = GetNode("class")

                If before Is Nothing Then
                    before = GetNode("package")
                End If

            Case "class"
                before = GetNode("package")
        End Select

        If before Is Nothing Then
            Return Me.Node.AppendChild(child)
        Else
            Return Me.Node.InsertBefore(child, before)
        End If
    End Function

    Protected Friend Overrides Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, Optional ByVal bParam As Boolean = False)
        Id = xmlRefNodeCounter.GetNewPackageId()
        If bParam = False Then
            Name = "New_" + Id
        Else
            Name = Name + "_" + Id
            Me.LoadChildrenList()

            For Each component As XmlComponent In Me.ChildrenList
                component.SetIdReference(xmlRefNodeCounter, bParam)
            Next
        End If
    End Sub
End Class

