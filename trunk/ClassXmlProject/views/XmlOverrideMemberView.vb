Imports System.Xml
Imports ClassXmlProject.XmlProjectTools

Public Class XmlOverrideMemberView
    Inherits XmlComponent

    Public Sub New(ByVal node As XmlNode)
        MyBase.New(node)
    End Sub

    Public ReadOnly Property NumID() As String
        Get
            Return GetAttribute("num-id")
        End Get
    End Property

    Public ReadOnly Property ParenClassName() As String
        Get
            Return GetAttribute("name", "parent::class")
        End Get
    End Property

    Public ReadOnly Property ClassId() As String
        Get
            Return GetAttribute("id", "parent::class")
        End Get
    End Property

    Public ReadOnly Property ClassImpl() As EImplementation
        Get
            Return ConvertDtdToEnumImpl(GetAttribute("implementation", "parent::class"))
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

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return (Me.Name = CType(obj, XmlComponent).Name)
    End Function
End Class
