Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class XmlRelationParentSpec
    Inherits XmlComponent

#Region "Properties"

    Public Function CreateDialogBox() As Form
        Return XmlNodeManager.GetInstance().CreateForm(Me)
    End Function

    Public Property Kind() As EKindParent
        Get
            Dim eKind As EKindParent

            If TestNode("list") Then

                eKind = EKindParent.Container

            ElseIf TestNode("array") Then

                eKind = EKindParent.Array

            Else
                eKind = EKindParent.Reference
            End If
            Return eKind
        End Get
        Set(ByVal value As EKindParent)
            UpdateNodes(value)
        End Set
    End Property

    Public ReadOnly Property FullpathClassName() As String
        Get
            Return GetFullpathClassName(Me.Idref)
        End Get
    End Property

    Protected Function GetFullpathClassName(ByVal strIdref As String) As String
        Dim nodeClass As XmlNode = Me.GetElementById(strIdref)

        Dim eLang As ELanguage = CType(Me.Tag, ELanguage)
        Dim strResult As String = GetFullpathDescription(nodeClass, eLang)
        If DEBUG_COMMANDS_ACTIVE Then strResult += " (" + eLang.ToString + ")"
        Return strResult
    End Function

    Public ReadOnly Property FullpathTypeDescription() As String
        Get
            Return GetFullpathTypeDescription(Me.Idref, Me.Level)
        End Get
    End Property

    Public Property Indexed() As Boolean
        Get
            If TestNode("list") Then
                Return (CheckAttribute("type", "indexed", "simple", "list"))
            End If
            Return False
        End Get
        Set(ByVal value As Boolean)
            If TestNode("list") Then
                If value Then
                    SetAttribute("type", "indexed", "list")
                Else
                    SetAttribute("type", "simple", "list")
                End If
            End If
        End Set
    End Property

    Public Property IndexRef() As String
        Get
            Return GetAttribute("index-idref", "list")
        End Get
        Set(ByVal value As String)
            ValidateIdReference(value, m_bCreateNodeNow)
            ReplaceAttribute("index-desc", "index-idref", "list")
            SetAttribute("index-idref", value, "list")
            SetAttribute("type", "indexed", "list")
        End Set
    End Property

    Public Property IndexDesc() As String
        Get
            Return GetAttribute("index-desc", "list")
        End Get
        Set(ByVal value As String)
            ReplaceAttribute("index-idref", "index-desc", "list")
            SetAttribute("index-desc", value, "list")
            SetAttribute("type", "indexed", "list")
        End Set
    End Property

    Public Property IndexLevel() As Integer
        Get
            Dim s As String = GetAttribute("level", "list")
            If s = "" Then
                Return 0
            End If
            Return CInt(s)
        End Get
        Set(ByVal value As Integer)
            AddAttribute("level", CStr(value), "list")
        End Set
    End Property

    Public Property Iterator() As Boolean
        Get
            Return (CheckAttribute("iterator", "yes", "no", "list"))
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("iterator", "yes", "list")
            Else
                SetAttribute("iterator", "no", "list")
            End If
        End Set
    End Property

    Public Property AccessorGet() As Boolean
        Get
            Return Not (CheckAttribute("range", "no", "no", "get"))  ' Not: in fact we test 'public' or 'protected'
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("range", "public", "get")
            Else
                SetAttribute("range", "no", "get")
            End If
        End Set
    End Property

    Public Property AccessorSet() As Boolean
        Get
            Return Not (CheckAttribute("range", "no", "no", "set"))  ' Not: in fact we test 'public' or 'protected'
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("range", "public", "set")
            Else
                SetAttribute("range", "no", "set")
            End If
        End Set
    End Property

    Public Property Idref() As String
        Get
            Return GetAttribute("idref")
        End Get
        Set(ByVal value As String)
            ValidateIdReference(value, m_bCreateNodeNow)
            SetAttribute("idref", value)
        End Set
    End Property

    Public ReadOnly Property StringCardinal() As String
        Get
            Dim strResult As String
            Select Case GetAttribute("cardinal")
                Case "01"
                    strResult = "0..1"
                Case "0n"
                    strResult = "0..n"
                Case "1n"
                    strResult = "1..n"
                Case Else
                    strResult = "1"
            End Select
            Return strResult
        End Get
    End Property

    Public Property Cardinal() As ECardinal
        Get
            Return StringToCardinal(GetAttribute("cardinal"))
        End Get
        Set(ByVal value As ECardinal)
            SetAttribute("cardinal", CardinalToString(value))
        End Set
    End Property

    Public Property Level() As Integer
        Get
            Return CInt(GetAttribute("level"))
        End Get
        Set(ByVal value As Integer)
            SetAttribute("level", CStr(value))
        End Set
    End Property

    Public Property Member() As Boolean
        Get
            Return (CheckAttribute("member", "class", "object"))
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("member", "class")
            Else
                SetAttribute("member", "object")
            End If
        End Set
    End Property

    Public Property Range() As String
        Get
            Return GetAttribute("range")
        End Get
        Set(ByVal value As String)
            SetAttribute("range", value)
        End Set
    End Property

    Public Property VarSize() As String
        Get
            Return GetAttribute("size", "array")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("size", "array")
                RemoveAttribute("sizeref", "array")
            Else
                ReplaceAttribute("sizeref", "size", "array")
                SetAttribute("size", value, "array")
            End If
        End Set
    End Property

    Public Property SizeRef() As String
        Get
            Return GetAttribute("sizeref", "array")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("size", "array")
                RemoveAttribute("sizeref", "array")
            Else
                ReplaceAttribute("size", "sizeref", "array")
                SetAttribute("sizeref", value, "array")
            End If
        End Set
    End Property

    Public Property ContainerDesc() As String
        Get
            Return GetAttribute("desc", "list")
        End Get
        Set(ByVal value As String)
            ReplaceAttribute("idref", "desc", "list")
            AddAttribute("desc", value, "list")
        End Set
    End Property

    Public Property ContainerType() As String
        Get
            Return GetAttribute("type", "list")
        End Get
        Set(ByVal value As String)
            If value = "simple" Then
                RemoveAttribute("index-idref", "list")
                RemoveAttribute("index-desc", "list")
                AddAttribute("type", value, "list")
            Else
                AddAttribute("index-desc", "list")
                AddAttribute("type", value)
            End If
        End Set
    End Property

    Public Property ContainerRef() As String
        Get
            Return GetAttribute("idref", "list")
        End Get
        Set(ByVal value As String)
            ValidateIdReference(value, m_bCreateNodeNow)
            ReplaceAttribute("desc", "idref", "list")
            SetAttribute("idref", value, "list")
        End Set
    End Property
#End Region

#Region "Public methods"

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            MyBase.SetDefaultValues(bCreateNodeNow)
            m_bCreateNodeNow = bCreateNodeNow
            If NodeName = "father" Then
                Name = "New_father"
            Else
                Name = "New_child"
            End If
            Range = "private"
            Cardinal = ECardinal.Fix
            Level = 0
            Member = False
            Idref = "class0"

            Kind = EKindParent.Reference

        Catch ex As Exception
            'Debug.Print(ex.StackTrace)
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Protected Friend Function GetFullpathTypeDescription(ByVal strIdref As String, ByVal ilevel As Integer) As String

        If strIdref Is Nothing Then
            Throw New Exception("Argument strId null in call of " + Me.ToString + ".GetFullpathTypeDescription()")
        ElseIf strIdref = "" Then
            Throw New Exception("Argument strId is empty in call of " + Me.ToString + ".GetFullpathTypeDescription()")
        End If

        Dim strResult As String = GetFullpathClassName(strIdref)
        strResult = strResult + DisplayLevel(ilevel, CType(Me.Tag, ELanguage))

        If TestNode("array") Then
            strResult = strResult + GetArrayString(CType(Me.Tag, ELanguage))
        End If

        If TestNode("list") Then
            If ContainerType = "indexed" Then
                Dim strTempo As String = ""
                If IndexRef IsNot Nothing Then
                    strTempo = XmlTypeVarSpec.GetReferenceTypeDescription(Me.GetElementById(IndexRef), CType(Me.Tag, ELanguage), True)
                ElseIf IndexDesc IsNot Nothing Then
                    strTempo = IndexDesc
                End If
                strTempo = strTempo + DisplayLevel(IndexLevel, CType(Me.Tag, ELanguage)) + ", "
                strResult = strTempo + strResult
            ElseIf TestNode("id(list/@idref)[@container='3']") Then
                If CType(Me.Tag, ELanguage) <> ELanguage.Language_CplusPlus Then
                    strResult = "Object"
                Else
                    strResult = "CObject"
                End If
            End If
            strResult = GetBeginContainer(CType(Me.Tag, ELanguage)) + strResult + GetEndContainer(CType(Me.Tag, ELanguage))
        End If
        Return strResult
    End Function
#End Region

    Private Sub UpdateNodes(ByVal eKind As EKindParent)
        Try
            Select Case eKind
                Case EKindParent.Reference
                    RemoveSingleNode("list")
                    AppendNode(CreateNode("get"))
                    AddAttribute("range", "no", "get")
                    AddAttribute("by", "val", "get")
                    AddAttribute("modifier", "var", "get")
                    AppendNode(CreateNode("set"))
                    AddAttribute("range", "no", "set")
                    AddAttribute("by", "val", "set")

                Case EKindParent.Container
                    RemoveAllNodes("array")
                    RemoveAllNodes("get")
                    RemoveAllNodes("set")
                    AppendNode(CreateNode("list"))
                    SetAttribute("type", "simple", "list")
                    AddAttribute("iterator", "no", "list")
                    ContainerType = "simple"
                    ContainerDesc = "undef_template"

                Case EKindParent.Array
                    RemoveAttribute("get")
                    RemoveAttribute("set")
                    RemoveSingleNode("list")
                    AppendNode(CreateNode("array"))
                    VarSize = "99999"
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
