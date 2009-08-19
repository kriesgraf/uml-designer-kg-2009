Imports System
Imports System.Xml
Imports System.IO
Imports System.Collections.ObjectModel
Imports Microsoft.VisualBasic

Public Class VbCodeReverse

    Public Shared Function Reverse(ByVal observer As InterfProgression, ByVal folder As String, _
                                   ByVal document As XmlDocument, Optional ByVal bRecursive As Boolean = True) As Boolean
        Dim bResult As String = False

        Dim directory As String() = folder.Split(Path.DirectorySeparatorChar)

        document.AppendChild(document.CreateNode(XmlNodeType.Element, "project", ""))
        document.DocumentElement.Attributes.SetNamedItem(document.CreateAttribute("name")).Value = directory(directory.Length - 1)
        observer.Log = "Folder: " + directory(directory.Length - 1)
        bResult = LoadFolders(observer, folder, document.DocumentElement, bRecursive)

#If TARGET = "exe" Then
        If bResult Then document.Save(My.Computer.FileSystem.CombinePath(folder, "example.xml"))
#End If
        Return bResult
    End Function

#Region "Private methods"

    Private Shared Function LoadFolders(ByVal observer As InterfProgression, ByVal folder As String, ByVal root As XmlNode, ByVal bRecursive As Boolean) As Boolean

        Dim bResult As String = False
        Dim files As ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(folder, FileIO.SearchOption.SearchTopLevelOnly, "*.vb")

        observer.Minimum = 0
        observer.Maximum = files.Count

        For Each foundFile As String In files
            LoadFile(foundFile, root)
            observer.Increment(1)
        Next

        Dim split As String()

        If bRecursive Then
            For Each directory As String In My.Computer.FileSystem.GetDirectories(folder, FileIO.SearchOption.SearchTopLevelOnly, "*")
                split = directory.Split(Path.DirectorySeparatorChar)
                Dim tempo As String = split(split.Length - 1)
                If tempo.StartsWith(".") = False Then
                    observer.Log = "Folder: " + tempo.PadRight(30)
                    LoadFolders(observer, directory, root, bRecursive)
                End If
            Next
        End If
        bResult = True

        Return (bResult)
    End Function

    Private Shared Function LoadFile(ByVal filename As String, ByVal root As XmlNode) As Boolean

        Dim bResult As String = False
        Dim analyser As New VbCodeAnalyser

        analyser.IsCodeReverse = True

        analyser.Analyse(filename)

        Dim node As XmlNode = analyser.Document.DocumentElement
        Dim package As XmlNode = node.SelectSingleNode("descendant::package")
        Dim child As XmlNode

        If package IsNot Nothing Then

            Dim name As String = package.Attributes.GetNamedItem("name").Value
            Dim package2 As XmlNode = root.SelectSingleNode("//package[@name='" + name + "']")

            If package2 Is Nothing Then
                root.AppendChild(root.OwnerDocument.ImportNode(package, True))
            Else
                For Each child In package.SelectNodes("*")
                    package2.AppendChild(root.OwnerDocument.ImportNode(child, True))
                Next
            End If
            child = node.SelectSingleNode("//imports")
            If child IsNot Nothing Then
                root.AppendChild(root.OwnerDocument.ImportNode(child, True))
            End If
        Else
            For Each child In node.SelectNodes("*")
                root.AppendChild(root.OwnerDocument.ImportNode(child, True))
            Next
        End If


        analyser.Dispose()
        analyser = Nothing

        bResult = True

        Return (bResult)
    End Function
#End Region
End Class
