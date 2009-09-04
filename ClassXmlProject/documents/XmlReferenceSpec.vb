Imports System
Imports System.Xml
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class XmlReferenceSpec
    Inherits XmlComponent

    Public Enum EReferenceKind
        Exception
        [Class]
        Typedef
        Enumeration
    End Enum

    Public ReadOnly Property Title() As String
        Get
            If Me.Kind = "class" Then
                Return "Class " + Me.Name
            End If
            Return "Typedef " + Me.Name
        End Get
    End Property

    Public ReadOnly Property Comment() As String
        Get
            If Me.RefKind = EReferenceKind.Typedef Then
                Return "Imports typedef " + FullpathClassName
            End If
            Return "Imports class " + FullpathClassName
        End Get
    End Property

    Public ReadOnly Property FullpathClassName() As String
        Get
            Dim eLang As ELanguage = CType(Me.Tag, ELanguage)
            Dim strResult As String = GetFullpathDescription(Me.Node, eLang)
            If DEBUG_COMMANDS_ACTIVE Then strResult += " (" + eLang.ToString + ")"
            Return strResult
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

    Public Property External() As String
        Get
            Return GetAttribute("external")
        End Get
        Set(ByVal value As String)
            SetAttribute("external", value)
        End Set
    End Property

    Public ReadOnly Property Implementation() As EImplementation
        Get
            Return EImplementation.Simple
        End Get
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

    Public ReadOnly Property RefKind() As EReferenceKind
        Get
            Select Case Kind
                Case "typedef"
                    If Me.SelectNodes("enumvalue").Count > 0 Then
                        Return EReferenceKind.Enumeration
                    End If
                    Return EReferenceKind.Typedef
                Case "exception"
                    Return EReferenceKind.Exception
                Case Else
                    Return EReferenceKind.Class
            End Select
        End Get
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

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing)
        MyBase.New(xmlNode)
    End Sub

    Protected Friend Overrides Function AppendNode(ByVal child As System.Xml.XmlNode, Optional ByVal observer As Object = Nothing) As System.Xml.XmlNode
        Dim before As XmlNode = Nothing
        Select Case child.Name
            Case "collaboration"
                before = GetNode("enumvalue")

            Case "enumvalue"
                ' Ignore
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
            Container = 0
            Id = "class0"
            Kind = "class"
            External = "no"
            Package = ""
            ParentClass = ""

        Catch ex As Exception
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

        Id = xmlRefNodeCounter.GetNewClassId()
        Dim strId As String = XmlNodeCounter.AfterStr(Me.Id, "class")

        Select Case eRename
            Case ENameReplacement.NewName
                Name = "New_reference_" + strId
            Case ENameReplacement.AddCopyName
                ' Name is set by caller
                Name = Name + "_" + strId
        End Select

        ' Use this option only to paste this node from another project
        If bSetIdrefChildren Then
            Me.RemoveAllNodes("collaboration")
        End If
    End Sub

End Class
