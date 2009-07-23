Imports System
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic

Public Class VbCodeReverse

    Public Shared Function Reverse(ByVal folder As String, ByVal document As XmlDocument) As Boolean
        Dim bResult As String = False
        Try
            document.AppendChild(document.CreateNode(XmlNodeType.Element, "root", ""))
            bResult = LoadFolders(folder, document.DocumentElement)
            If bResult Then document.Save(My.Computer.FileSystem.CombinePath(folder, "example.xml"))

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

#Region "Private methods"

    Private Shared Function LoadFolders(ByVal folder As String, ByVal package As XmlNode) As Boolean

        Dim bResult As String = False
        Try
            Dim child As XmlNode

            For Each foundFile As String In My.Computer.FileSystem.GetFiles(folder, FileIO.SearchOption.SearchTopLevelOnly, "*.vb")
                LoadFile(foundFile, package)
            Next

            For Each directory As String In My.Computer.FileSystem.GetDirectories(folder)

                child = package.AppendChild(package.OwnerDocument.CreateNode(XmlNodeType.Element, "package", ""))

                LoadFolders(My.Computer.FileSystem.CombinePath(folder, directory), child)
            Next

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
            analyser.Analyse(filename)
            package.AppendChild(package.OwnerDocument.ImportNode(analyser.Document.DocumentElement, True))
            analyser.Dispose()

            bResult = True

        Catch ex As Exception
            Throw ex
        End Try
        Return (bResult)
    End Function
#End Region
End Class
