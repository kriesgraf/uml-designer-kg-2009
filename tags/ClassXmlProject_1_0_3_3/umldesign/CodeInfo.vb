#If _APP_UML = "1" Then
Imports ClassXmlProject.UmlNodesManager
#End If

Public Class CodeInfo
    Public Property Filename() As String
        Get
            Return strFilename
        End Get
        Set(ByVal value As String)
#If _APP_UML = "1" Then
            strFilename = ComputeRelativePath(value, strReleaseFile)
#End If
        End Set
    End Property

    Private strFilename As String

    Public strTempFile As String
    Public strReleaseFile As String
    Public bCodeMerge As Boolean
    Public bSourceExists As Boolean
End Class
