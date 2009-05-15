Imports System
Imports ClassXmlProject.XmlProjectTools
Imports System.Xml
Imports Microsoft.VisualBasic

Public Class XmlCodeInline
    Inherits XmlComponent

    Public Property Kind() As String
        Get
            Return GetAttribute("type")
        End Get
        Set(ByVal value As String)
            SetAttribute("type", value)
        End Set
    End Property

    Public Property CodeSource() As String
        Get
            Return ConvertCodeText()
        End Get
        Set(ByVal value As String)
            ConvertTextCode(value)
        End Set
    End Property

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing)
        MyBase.New(xmlNode)
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            Kind = "method"
            CodeSource = "/* Insert code here */"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Private Function ConvertCodeText() As String
        Dim strCode As String = ""
        Try
            Dim child As XmlNode

            If Me.Node IsNot Nothing Then
                For Each child In SelectNodes()
                    strCode = strCode + GetAttributeValue(child, "value") + vbCrLf
                Next child
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strCode
    End Function

    Private Sub ConvertTextCode(ByVal value As String)
        Dim strTempo As String
        Try
            Dim i As Integer

            RemoveAllNodes("line")

            strTempo = value
            i = InStr(strTempo, vbCrLf)

            While i > 0
                CreateAppendNode("line")
                AddAttribute("value", Left(strTempo, i - 1), "line[last()]")
                strTempo = Mid(strTempo, i + 2)
                i = InStr(strTempo, vbCrLf)
            End While

            If Len(strTempo) > 0 Then
                CreateAppendNode("line")
                AddAttribute("value", strTempo, "line[last()]")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
