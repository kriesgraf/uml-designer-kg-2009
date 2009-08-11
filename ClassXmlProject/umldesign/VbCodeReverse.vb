Imports System
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic

Public Class VbCodeReverse

    Public Shared Function Reverse(ByVal folder As String, ByVal document As XmlDocument, _
                                   Optional ByVal bRecursive As Boolean = True) As Boolean
        Dim bResult As String = False
        Try
            document.AppendChild(document.CreateNode(XmlNodeType.Element, "project", ""))
            bResult = LoadFolders(folder, document.DocumentElement, bRecursive)
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
            Dim child As XmlNode

            package.Attributes.SetNamedItem(package.OwnerDocument.CreateAttribute("folder"))
            package.Attributes.GetNamedItem("folder").Value = folder

            For Each foundFile As String In My.Computer.FileSystem.GetFiles(folder, FileIO.SearchOption.SearchTopLevelOnly, "*.vb")
                LoadFile(foundFile, package)
            Next
            If bRecursive Then
                For Each directory As String In My.Computer.FileSystem.GetDirectories(folder)

                    child = package.AppendChild(package.OwnerDocument.CreateNode(XmlNodeType.Element, "package", ""))

                    LoadFolders(My.Computer.FileSystem.CombinePath(folder, directory), child, bRecursive)
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
