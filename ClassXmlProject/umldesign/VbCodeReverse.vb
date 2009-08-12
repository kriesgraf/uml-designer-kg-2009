Imports System
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic

Public Class VbCodeReverse

    Public Shared Function Reverse(ByVal folder As String, ByVal document As XmlDocument, _
                                   Optional ByVal bRecursive As Boolean = True) As Boolean
        Dim bResult As String = False
        Try
            Dim directory As String() = folder.Split(Path.DirectorySeparatorChar)

            document.AppendChild(document.CreateNode(XmlNodeType.Element, "project", ""))
            document.DocumentElement.Attributes.SetNamedItem(document.CreateAttribute("name")).Value = directory.Last
            bResult = LoadFolders(folder, document.DocumentElement, bRecursive)

            ' TODO: implements into workspace
            If bResult Then document.Save(My.Computer.FileSystem.CombinePath(folder, "example.xml"))

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

#Region "Private methods"

    Private Shared Function LoadFolders(ByVal folder As String, ByVal package As XmlNode, ByVal bRecursive As Boolean) As Boolean

        Dim bResult As String = False
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(folder, FileIO.SearchOption.SearchTopLevelOnly, "*.vb")
                LoadFile(foundFile, package)
            Next
            If bRecursive Then
                For Each directory As String In My.Computer.FileSystem.GetDirectories(folder)
                    LoadFolders(My.Computer.FileSystem.CombinePath(folder, directory), package, bRecursive)
                Next
            End If
            bResult = True

        Catch ex As Exception
            Throw ex
        End Try
        Return (bResult)
    End Function

    Private Shared Function LoadFile(ByVal filename As String, ByVal package As XmlNode) As Boolean

        Dim bResult As String = False
        Try
            Dim analyser As New VbCodeAnalyser

            analyser.InheritsDeclaration = True
            analyser.TypedefsDeclaration = True
            analyser.VbDocComment = True

            analyser.Analyse(filename)

            package.AppendChild(package.OwnerDocument.ImportNode(analyser.Document.DocumentElement, True))

            analyser.Dispose()
            analyser = Nothing

            bResult = True

        Catch ex As Exception
            Throw ex
        End Try
        Return (bResult)
    End Function
#End Region
End Class
