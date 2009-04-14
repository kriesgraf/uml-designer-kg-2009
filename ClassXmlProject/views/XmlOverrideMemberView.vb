Imports System.Xml
Imports ClassXmlProject.XmlProjectTools

Public Class XmlOverrideMemberView
    Inherits XmlComponent

    Private m_bChecked As Boolean = False

    Public Property CheckedView() As Boolean
        Get
            Return m_bChecked
        End Get
        Set(ByVal value As Boolean)
            m_bChecked = value
        End Set
    End Property

    Public ReadOnly Property NumID() As String
        Get
            Return GetAttribute("num-id")
        End Get
    End Property

    Public ReadOnly Property Implementation() As EImplementation
        Get
            Select Case Me.NodeName
                Case "property"
                    If CheckAttribute("overridable", "yes", "no") Then
                        If CheckAttribute("implementation", "abstract", "simple", "parent::class") Then
                            Return EImplementation.Interf

                        ElseIf CheckAttribute("root", "no", "no", "parent::interface") Then

                            Return EImplementation.Interf
                        Else
                            Return EImplementation.Leaf
                        End If
                    Else
                        Return EImplementation.Leaf
                    End If

                Case "method"
                    Return ConvertDtdToEnumImpl(GetAttribute("implementation"))
            End Select
            Return EImplementation.Unknown
        End Get
    End Property

    Public Property InterfaceMember() As Boolean
        Get
            Select Case Me.NodeName
                Case "property"
                    Return CheckAttribute("overridable", "yes", "no")

                Case "method"
                    Select Case ConvertDtdToEnumImpl(GetAttribute("implementation"))
                        Case EImplementation.Interf, EImplementation.Node, EImplementation.Root
                            Return True
                    End Select
            End Select
            Return False
        End Get
        Set(ByVal value As Boolean)
            Select Case Me.NodeName
                Case "property"
                    SetBooleanAttribute("overridable", value)

                Case "method"
                    SetBooleanAttribute("implementation", value, "root", "final")
            End Select
        End Set
    End Property

    Public ReadOnly Property OverridableMember() As Boolean
        Get
            Select Case Me.NodeName
                Case "property"
                    Return CheckAttribute("overridable", "yes", "no")

                Case "method"
                    Select Case ConvertDtdToEnumImpl(GetAttribute("implementation"))
                        Case EImplementation.Interf, EImplementation.Node, EImplementation.Root
                            Return True

                        Case EImplementation.Simple
                            Return Not (CheckAttribute("constructor", "no", "no"))
                    End Select
            End Select
            Return False
        End Get
    End Property

    Public ReadOnly Property ParenClassName() As String
        Get
            Return GetAttribute("name", "parent::class | parent::interface")
        End Get
    End Property

    Public ReadOnly Property ClassId() As String
        Get
            Return GetAttribute("id", "parent::class | parent::interface")
        End Get
    End Property

    Public ReadOnly Property ClassImpl() As EImplementation
        Get
            Return ConvertDtdToEnumImpl(GetAttribute("implementation", "parent::class | parent::interface"))
        End Get
    End Property

    Public ReadOnly Property FullDescription() As String
        Get
            Dim strResult As String = Me.ParenClassName + "." + Me.Name
            If Me.NodeName = "method" Then
                strResult += "("
                Dim strParams As String = ""
                For Each child As XmlNode In SelectNodes("param")
                    If strParams = "" Then
                        strParams = GetName(child)
                    Else
                        strParams += ", " + GetName(child)
                    End If
                Next
                strResult += strParams + ")"
            End If
            Return strResult
        End Get
    End Property

    Public Sub New(ByVal nodeXml As XmlNode)
        MyBase.New(nodeXml)
    End Sub

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return (Me.Name = CType(obj, XmlComponent).Name)
    End Function
End Class
