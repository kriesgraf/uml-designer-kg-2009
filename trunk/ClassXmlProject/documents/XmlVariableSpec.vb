Imports System
Imports System.Xml

Public Class XmlVariableSpec
    Inherits XmlComponent

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

        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub
End Class
