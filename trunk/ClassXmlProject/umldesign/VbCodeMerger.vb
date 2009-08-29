Imports System
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic

Public Class VbCodeMerger

#Region "Public methods"

    Public Shared Sub Merge(ByVal fileInfo As CodeInfo, ByVal strTempFolder As String)

        Dim strFolder As String = Path.GetDirectoryName(fileInfo.strReleaseFile)
        Dim strNewFile As String = fileInfo.strReleaseFile + ".new"
        Dim strTempPath As String = Path.GetDirectoryName(fileInfo.strTempFile)
        Dim bMergeOk As Boolean = False
        Dim strSource As String
        Dim strDestination As String

        Try
            Using PreviousModule As New VbCodeAnalyser
                Using NextModule As New VbCodeAnalyser

                    strSource = PreviousModule.Analyse(strFolder, Path.GetFileName(fileInfo.strReleaseFile), ".prev", strTempFolder)
                    strDestination = NextModule.Analyse(strTempPath, Path.GetFileName(fileInfo.strTempFile), ".next")

                    Dim newDocXml As New XmlDocument
                    newDocXml.AppendChild(newDocXml.CreateNode(XmlNodeType.Element, "root", ""))

                    CompareFiles(PreviousModule, NextModule, newDocXml)
#If DEBUG Then
                    NextModule.Document.Save(My.Computer.FileSystem.CombinePath(strTempPath, "test2.xml"))
                    newDocXml.Save(My.Computer.FileSystem.CombinePath(strTempPath, "test.xml"))
#End If
                    MergeFiles(NextModule, newDocXml)
#If DEBUG Then
                    newDocXml.Save(My.Computer.FileSystem.CombinePath(strTempPath, "test.xml"))
#End If
                    bMergeOk = ProcessMerging(newDocXml, PreviousModule, strNewFile)
                End Using
            End Using

#If DEBUG Then
#Else
            ' Move, rename files outside 'Using' blocks
            If bMergeOk Then
                MoveFile(fileInfo.strReleaseFile, fileInfo.strReleaseFile + ".bak")
                MoveFile(strNewFile, fileInfo.strReleaseFile)
            else
                MsgBox("Automatic merge tool has not found find code to add or update in module '" + fileInfo.strReleaseFile + "'" + _
                       vbCrLf + vbCrLf + "We invite you to look at temporary & hidden folder '" + strTempPath + "'.", MsgBoxStyle.Critical, "Automatic merge tool")
            End If
            ' Remove temporay files, except VB generated one
            RemoveFile(strSource)
            RemoveFile(strDestination)
#End If

        Catch ex As Exception

#If TARGET = "exe" Then
            Console.WriteLine("Fails to merge file:" + vbCrLf + ex.Message + vbCrLf + fileInfo.strTempFile + _
                              "' with '" + fileInfo.strReleaseFile + "'" + vbCrLf + vbCrLf + ex.StackTrace)
            Console.ReadKey()
#Else
            Throw New Exception("Fails to merge file '" + fileInfo.strTempFile + "' with '" + fileInfo.strReleaseFile + "'", ex)
#End If
        End Try
    End Sub

#End Region

#Region "Private methods"

    Private Shared Sub CompareFiles(ByVal PrevModule As VbCodeAnalyser, ByVal NextModule As VbCodeAnalyser, _
                             ByVal newDocXml As XmlDocument)

        Dim prevDocXml As XmlDocument = PrevModule.Document
        Dim nextDocXml As XmlDocument = NextModule.Document
        Dim clone, current, body As XmlNode
        Try
            current = newDocXml.DocumentElement

            Dim child As XmlNode = prevDocXml.SelectSingleNode("/root/imports")
            Dim replaceChild As XmlNode = nextDocXml.SelectSingleNode("/root/imports")

            If replaceChild IsNot Nothing Then
                SetCheck(replaceChild)

                If child Is Nothing Then
                    ' Add immediately "Imports" if not exists in previous source
                    clone = newDocXml.ImportNode(replaceChild, False)
                    current.AppendChild(clone)
                    body = newDocXml.CreateNode(XmlNodeType.Element, "body", "")
                    clone.AppendChild(body)
                    body.InnerText = LoadLines(NextModule, replaceChild)
                    SetInsertLine(clone, 1)
                Else
                    SetCheck(child)

                    ' Otherwise, copy new imports in old imports
                    clone = newDocXml.ImportNode(child, False)
                    current.AppendChild(clone)

                    body = newDocXml.CreateNode(XmlNodeType.Element, "body", "")
                    clone.AppendChild(body)
                    body.InnerText = LoadLines(NextModule, replaceChild)
                    SetLastEndLine(clone, GetLastEndLine(child))
                End If
                ' If new imports not exists do not insert anything
            End If

            Dim iStopLine As Integer = 0
            child = prevDocXml.SelectSingleNode("/root/package")
            replaceChild = nextDocXml.SelectSingleNode("/root/package")

            If replaceChild IsNot Nothing Then
                SetCheck(replaceChild)

                If child Is Nothing Then

                    ' Insert immediately a new package declaration and parse current class
                    ' Find first node under root to get its text position
                    child = prevDocXml.SelectSingleNode("/root/*[self::vb-doc or self::class]")

                    clone = newDocXml.ImportNode(replaceChild, False)
                    current.AppendChild(clone)
                    iStopLine = GetStartLine(child)
                    SetStartLine(clone, iStopLine)
                    ' To force insert these lines, 
                    ' Don't call SetInsertLine because must not insert end-package node !
                    SetStopLine(clone, iStopLine - 1)

                    body = newDocXml.CreateNode(XmlNodeType.Element, "body", "")
                    clone.AppendChild(body)
                    body.InnerText = NextModule.LoadLines(GetStartLine(replaceChild), GetStopLine(replaceChild))

                    ParsePackageDeclaration(prevDocXml.DocumentElement, replaceChild, clone, NextModule, iStopLine)

                    ' Don't insert 'end-package' but 'package' node !
                    clone = newDocXml.CreateNode(XmlNodeType.Element, "package", "")
                    current.AppendChild(clone)
                    body = newDocXml.CreateNode(XmlNodeType.Element, "body", "")
                    clone.AppendChild(body)
                    body.InnerText = "End Namespace" + vbCrLf
                    SetInsertLine(clone, iStopLine + 1)
                Else
                    SetCheck(child)

                    ' Copy new package declaration in old declaration
                    clone = newDocXml.ImportNode(child, False)
                    current.AppendChild(clone)

                    body = newDocXml.CreateNode(XmlNodeType.Element, "body", "")
                    clone.AppendChild(body)
                    body.InnerText = NextModule.LoadLines(GetStartLine(replaceChild), GetStopLine(replaceChild))

                    ParsePackageDeclaration(child, replaceChild, clone, NextModule, iStopLine)
                End If
            Else
                ParsePackageDeclaration(prevDocXml.DocumentElement, nextDocXml.DocumentElement, current, NextModule, iStopLine)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub MergeFiles(ByVal NextModule As VbCodeAnalyser, ByVal newDocXml As XmlDocument)
        ' Only one class in the "next" file
        Dim firstParent As XmlNode = NextModule.Document.SelectSingleNode("//class")
        If firstParent Is Nothing Then
            Throw New Exception("File '" + NextModule.Filename + "' generated from code doesn't contain node 'class'")
        End If
        ' Only the first class id generated
        Dim secondParent As XmlNode = newDocXml.DocumentElement.SelectSingleNode("//class")
        If secondParent Is Nothing Then
            Throw New Exception("XML merged document doesn't contain node 'class'")
        End If

        MergeNodes(firstParent, secondParent, NextModule, newDocXml)
    End Sub

    Private Shared Sub MergeNodes(ByVal firstParent As XmlNode, ByVal secondParent As XmlNode, _
                                  ByVal NextModule As VbCodeAnalyser, ByVal newDocXml As XmlDocument)

        Dim vbdoc As XmlNode = Nothing
        Dim previous As XmlNode = Nothing
        Dim body, clone, second As XmlNode
        Try
            For Each child As XmlNode In firstParent.ChildNodes()
                Dim strNodeName As String = GetNodeName(child)
                If IsNotCheck(child) = False _
                Then
                    Select Case strNodeName
                        Case "region", "property"
                            second = GetReferenceName(secondParent, child)
                            MergeNodes(child, second, NextModule, newDocXml)
                            previous = child

                        Case "vb-doc"
                            ' Ignore 

                        Case Else
                            previous = child
                    End Select
                Else
                    Select Case strNodeName
                        Case "vb-doc"
                            vbdoc = child

                        Case Else
                            Select Case strNodeName
                                Case "region", "property"
                                    clone = newDocXml.ImportNode(child, False)

                                Case Else
                                    clone = newDocXml.ImportNode(child, True)
                            End Select

                            SetCheck(clone)

                            Dim iPos As Integer = 0

                            If previous IsNot Nothing _
                            Then
                                Dim before As XmlNode = GetReferenceName(secondParent, previous)
                                If before IsNot Nothing Then
                                    before = before.NextSibling
                                End If

                                If before Is Nothing Then
                                    before = secondParent.SelectSingleNode("end-property")
                                End If

                                If before Is Nothing Then
                                    before = secondParent.SelectSingleNode("end-region")
                                End If

                                If before Is Nothing Then
                                    before = secondParent.SelectSingleNode("end-class")
                                End If

                                Select Case GetNodeName(before)
                                    Case "end-class", "end-region", "end-property"
                                        iPos = GetLastEndLine(secondParent)
                                    Case Else
                                        iPos = GetStartLine(before)
                                End Select

                                secondParent.InsertBefore(clone, before)
                            Else
                                Dim before As XmlNode = secondParent.SelectSingleNode("*[not(self::body)]")
                                Select Case GetNodeName(before)
                                    Case "end-class", "end-region", "end-property"
                                        iPos = GetLastEndLine(secondParent)
                                    Case Else
                                        iPos = GetStartLine(before)
                                End Select
                                before = secondParent.InsertBefore(clone, before)
                            End If

                            Dim strNewLines As String = ""

                            If vbdoc IsNot Nothing Then
                                strNewLines += vbdoc.FirstChild.InnerText + vbCrLf
                            End If

                            strNewLines += LoadLines(NextModule, child)

                            body = newDocXml.CreateNode(XmlNodeType.Element, "body", "")
                            clone.InsertBefore(body, clone.FirstChild)
                            Select Case strNodeName
                                Case "get", "set"
                                    body.InnerText = strNewLines
                                Case Else
                                    body.InnerText = strNewLines + vbCrLf
                            End Select

                            SetPosition(clone, 1)   ' To force write on first column
                            SetInsertLine(clone, iPos)

                            SetCheck(child)
                            previous = child
                            vbdoc = Nothing
                    End Select
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub ParsePackageDeclaration(ByVal PrevPackageNode As XmlNode, ByVal NextPackageNode As XmlNode, _
                                        ByVal NewPackageNode As XmlNode, ByVal NextModule As VbCodeAnalyser, _
                                        ByRef iStopClassDeclaration As Integer)

        Dim vbdocPrev As XmlNode = Nothing
        Dim clone As XmlNode
        Dim iClassCounter As Integer = 0
        Try
            For Each child As XmlNode In PrevPackageNode.SelectNodes("*")
                Dim strNodeName As String = GetNodeName(child)
                Select Case strNodeName
                    Case "class", "typedef", "region"   ' Allow a second class or several typedef outside project declaration

                        If strNodeName = "class" Then iClassCounter += 1

                        Dim replaceChild As XmlNode = NextPackageNode.SelectSingleNode(strNodeName + "[@name='" + GetName(child) + "']")
                        Dim vbdocNext As XmlNode
                        Dim body As XmlNode

                        If replaceChild Is Nothing _
                        Then
                            vbdocNext = Nothing

                            If strNodeName = "class" And iClassCounter = 1 Then
                                ' First class is necessarily the generated class
                                ' Perhaps this is renamed, also we search it in next module
                                replaceChild = NextPackageNode.SelectSingleNode(strNodeName)

                                If GetNodeName(replaceChild.PreviousSibling) = "vb-doc" _
                                Then
                                    vbdocNext = replaceChild.PreviousSibling
                                End If
                            End If

                        ElseIf GetNodeName(replaceChild.PreviousSibling) = "vb-doc" _
                        Then
                            vbdocNext = replaceChild.PreviousSibling
                        Else
                            vbdocNext = Nothing
                        End If

                        If replaceChild IsNot Nothing _
                        Then
                            SetCheck(replaceChild)
                            SetCheck(child)

                            ' Clone only 'class' node and replace each declaration
                            If vbdocPrev IsNot Nothing Then
                                clone = NewPackageNode.OwnerDocument.ImportNode(vbdocPrev, True)
                                NewPackageNode.AppendChild(clone)
                                If vbdocNext IsNot Nothing Then
                                    SetCheck(clone)
                                    SetCheck(vbdocNext)
                                    clone.FirstChild.InnerText = vbdocNext.FirstChild.InnerText
                                End If
                            End If

                            clone = NewPackageNode.OwnerDocument.ImportNode(child, False)
                            NewPackageNode.AppendChild(clone)

                            Dim strNewLine As String = ""

                            If vbdocPrev Is Nothing And vbdocNext IsNot Nothing Then
                                strNewLine = vbdocNext.FirstChild.InnerText
                            End If

                            body = NewPackageNode.OwnerDocument.CreateNode(XmlNodeType.Element, "body", "")
                            clone.AppendChild(body)
                            strNewLine += NextModule.LoadLines(GetStartLine(replaceChild), GetStopLine(replaceChild))
                            body.InnerText = strNewLine

                            ParseClassDeclaration(child, replaceChild, NextModule, clone)
                        Else
                            If vbdocPrev IsNot Nothing Then
                                clone = NewPackageNode.OwnerDocument.ImportNode(vbdocPrev, True)
                                NewPackageNode.AppendChild(clone)
                            End If
                            ' Clone 'class' node deeply to see in next pass
                            clone = NewPackageNode.OwnerDocument.ImportNode(child, True)
                            NewPackageNode.AppendChild(clone)
                        End If
                        iStopClassDeclaration = GetLastEndLine(clone)

                        vbdocPrev = Nothing

                    Case "vb-doc"
                        vbdocPrev = child

                    Case "end-package"
                        vbdocPrev = Nothing
                        clone = NewPackageNode.OwnerDocument.ImportNode(child, True)
                        NewPackageNode.AppendChild(clone)

                    Case Else
                        vbdocPrev = Nothing
                End Select
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub ParseClassDeclaration(ByVal PrevClassNode As XmlNode, ByVal NextClassNode As XmlNode, ByVal NextModule As VbCodeAnalyser, ByVal NewClassNode As XmlNode)

        Dim vbdocPrev As XmlNode = Nothing
        Try
            For Each child As XmlNode In PrevClassNode.SelectNodes("*")

                Dim replaceChild, vbdocNext, clone, body As XmlNode
                Dim strNodeName As String = GetNodeName(child)

                If strNodeName <> "vb-doc" _
                Then
                    Select Case strNodeName
                        Case "attribute", "method", "property", "get", "set", "typedef", "class", "region"
                            replaceChild = GetReferenceName(NextClassNode, child)

                            If replaceChild Is Nothing _
                            Then
                                vbdocNext = Nothing

                            ElseIf GetNodeName(replaceChild.PreviousSibling) = "vb-doc" _
                            Then
                                vbdocNext = replaceChild.PreviousSibling
                            Else
                                vbdocNext = Nothing
                            End If

                            If replaceChild IsNot Nothing _
                            Then
                                ' If new node is not yet replaced
                                If IsNotCheck(replaceChild) Then
                                    SetCheck(replaceChild)
                                    SetCheck(child)

                                    ' Clone only 'class' node and replace each declaration
                                    If vbdocPrev IsNot Nothing Then
                                        clone = NewClassNode.OwnerDocument.ImportNode(vbdocPrev, True)
                                        NewClassNode.AppendChild(clone)
                                        If vbdocNext IsNot Nothing Then
                                            SetCheck(clone)
                                            SetCheck(vbdocNext)
                                            clone.FirstChild.InnerText = vbdocNext.FirstChild.InnerText
                                        End If
                                    End If

                                    clone = NewClassNode.OwnerDocument.ImportNode(child, False)
                                    NewClassNode.AppendChild(clone)

                                    Dim strNewLine As String = ""

                                    If vbdocPrev Is Nothing And vbdocNext IsNot Nothing Then
                                        strNewLine = vbdocNext.FirstChild.InnerText
                                    End If

                                    body = NewClassNode.OwnerDocument.CreateNode(XmlNodeType.Element, "body", "")
                                    clone.AppendChild(body)

                                    Select Case strNodeName
                                        Case "class", "typedef"         ' Nested classes and Structure, Enum
                                            SetStopLine(clone, GetLastEndLine(child))
                                            strNewLine += LoadLines(NextModule, replaceChild)
                                            body.InnerText = strNewLine

                                        Case "region"
                                            strNewLine += NextModule.LoadLines(GetStartLine(replaceChild), GetStopLine(replaceChild))
                                            body.InnerText = strNewLine

                                            ' We reparse 'region' content with same method
                                            ParseClassDeclaration(child, replaceChild, NextModule, clone)

                                        Case "property"
                                            strNewLine += NextModule.LoadLines(GetStartLine(replaceChild), GetStopLine(replaceChild))
                                            body.InnerText = strNewLine

                                            ' We reparse 'property' content with same method
                                            ParseClassDeclaration(child, replaceChild, NextModule, clone)

                                        Case Else
                                            strNewLine += NextModule.LoadLines(GetStartLine(replaceChild), GetStopLine(replaceChild))
                                            body.InnerText = strNewLine
                                    End Select
                                Else
                                    ' If new node is already replaced
                                    If vbdocPrev IsNot Nothing Then
                                        clone = NewClassNode.OwnerDocument.ImportNode(vbdocPrev, True)
                                        NewClassNode.AppendChild(clone)
                                    End If
                                    ' Clone dub 'class' node deeply to see in next pass
                                    clone = NewClassNode.OwnerDocument.ImportNode(child, True)
                                    NewClassNode.AppendChild(clone)
                                End If
                            Else
                                If vbdocPrev IsNot Nothing Then
                                    clone = NewClassNode.OwnerDocument.ImportNode(vbdocPrev, True)
                                    NewClassNode.AppendChild(clone)
                                End If
                                ' Clone dub 'class' node deeply to see in next pass
                                clone = NewClassNode.OwnerDocument.ImportNode(child, True)
                                NewClassNode.AppendChild(clone)
                            End If

                        Case Else
                            If vbdocPrev IsNot Nothing Then
                                clone = NewClassNode.OwnerDocument.ImportNode(vbdocPrev, True)
                                NewClassNode.AppendChild(clone)
                            End If
                            ' Clone sub 'class' node deeply to see in next pass
                            clone = NewClassNode.OwnerDocument.ImportNode(child, True)
                            NewClassNode.AppendChild(clone)
                    End Select

                    vbdocPrev = Nothing
                Else
                    vbdocPrev = child
                    vbdocNext = Nothing
                    replaceChild = Nothing
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function ProcessMerging(ByVal docXml As XmlDocument, ByVal VbCode As VbCodeAnalyser, _
                                            ByVal strTempFile As String) As Boolean

        Dim bResult As Boolean = False
        Dim iStartLine As Integer = 1
        Dim iStopLine As Integer = 1

        Try
            Using streamWriter As StreamWriter = New StreamWriter(strTempFile)

                For Each child As XmlNode In docXml.SelectNodes("/root/*")
                    bResult = True

                    'Debug.Print(child.OuterXml)

                    iStopLine = GetStartLine(child) - 1
                    WriteStreamLines(streamWriter, VbCode, iStartLine, iStopLine)

                    Dim strNodename As String = GetNodeName(child)
                    Select Case strNodename
                        Case "vb-doc"
                            WriteStreamVbDoc(streamWriter, child)

                            iStartLine = GetLastEndLine(child) + 1

                        Case "imports"
                            WriteStreamBody(streamWriter, child, VbCode)

                            iStartLine = GetLastEndLine(child) + 1

                        Case "class"
                            WriteStreamBody(streamWriter, child, VbCode)

                            iStartLine = GetStopLine(child) + 1
                            ClassMerging(child, iStartLine, iStopLine, VbCode, streamWriter)

                        Case "region"
                            WriteStreamBody(streamWriter, child, VbCode)

                            iStartLine = GetStopLine(child) + 1
                            RegionMerging(child, iStartLine, iStopLine, VbCode, streamWriter)

                        Case "package"
                            WriteStreamBody(streamWriter, child, VbCode)

                            iStartLine = GetStopLine(child) + 1
                            PackageMerging(child, iStartLine, iStopLine, VbCode, streamWriter)


                        Case Else
                            WriteStreamBody(streamWriter, child, VbCode)

                            iStartLine = GetStopLine(child) + 1
                    End Select

                    If iStartLine = 0 Then
                        iStartLine = iStopLine
                    End If
                Next
                streamWriter.Close()
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Private Shared Sub RemoveFile(ByVal filename As String)
        Try
            If filename IsNot Nothing Then
                If My.Computer.FileSystem.FileExists(filename) _
                Then
                    My.Computer.FileSystem.DeleteFile(filename)
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Fails to remove file:" + filename, ex)
        End Try
    End Sub

    Private Shared Sub MoveFile(ByVal strSource As String, ByVal strDestination As String)
        Try
            My.Computer.FileSystem.MoveFile(strSource, strDestination, True)

        Catch ex As Exception
            Throw New Exception("Fails to move file:" + strSource + " to:" + strDestination, ex)
        End Try
    End Sub


    Private Shared Sub PackageMerging(ByVal node As XmlNode, ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                             ByVal VbCode As VbCodeAnalyser, ByVal streamWriter As StreamWriter)
        Try
            For Each child As XmlNode In node.SelectNodes("*")

                Dim strNodename As String = GetNodeName(child)
                Select Case strNodename
                    Case "vb-doc", "region", "class"
                        iStopLine = GetStartLine(child) - 1
                        WriteStreamLines(streamWriter, VbCode, iStartLine, iStopLine)
                End Select

                Select Case strNodename
                    Case "vb-doc"
                        WriteStreamVbDoc(streamWriter, child)
                        iStartLine = GetLastEndLine(child) + 1

                    Case "region"
                        WriteStreamBody(streamWriter, child, VbCode)
                        iStartLine = GetStopLine(child) + 1
                        RegionMerging(child, iStartLine, iStopLine, VbCode, streamWriter)

                    Case "class"
                        WriteStreamBody(streamWriter, child, VbCode)
                        iStartLine = GetStopLine(child) + 1
                        ClassMerging(child, iStartLine, iStopLine, VbCode, streamWriter)


                    Case "end-package"
                        iStopLine = GetLastEndLine(node)    ' node is used, see GetLastEndLine!
                        WriteStreamLines(streamWriter, VbCode, iStartLine, iStopLine)
                        iStartLine = iStopLine + 1

                    Case "body"
                        ' Ignore

                    Case Else
                        OtherNodeMerge(child, iStartLine, iStopLine, VbCode, streamWriter)
                End Select

                If iStartLine = 0 Then
                    iStartLine = iStopLine
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub RegionMerging(ByVal node As XmlNode, ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                             ByVal VbCode As VbCodeAnalyser, ByVal streamWriter As StreamWriter)
        Try
            If IsNotCheck(node) _
            Then
                iStartLine = GetStartLine(node)
                iStopLine = GetLastEndLine(node)
                WriteStreamLines(streamWriter, VbCode, iStartLine, iStopLine, True)
                iStartLine = iStopLine + 1
            Else
                For Each child As XmlNode In node.SelectNodes("*")

                    Dim strNodename As String = GetNodeName(child)

                    Select Case strNodename
                        Case "vb-doc", "region", "class", "typedef", "property", "get", "set", "method", "attribute"
                            iStopLine = GetStartLine(child) - 1
                            WriteStreamLines(streamWriter, VbCode, iStartLine, iStopLine)
                    End Select


                    Select Case strNodename
                        Case "vb-doc"
                            WriteStreamVbDoc(streamWriter, child)
                            iStartLine = GetLastEndLine(child) + 1

                        Case "class"
                            WriteStreamBody(streamWriter, child, VbCode)
                            iStartLine = GetStopLine(child) + 1
                            ClassMerging(child, iStartLine, iStopLine, VbCode, streamWriter)

                        Case "property"
                            If IsNotCheck(child) Then
                                OtherNodeMerge(child, iStartLine, iStopLine, VbCode, streamWriter)
                            Else
                                WriteStreamBody(streamWriter, child, VbCode)
                                iStartLine = GetStopLine(child) + 1
                                RegionMerging(child, iStartLine, iStopLine, VbCode, streamWriter)
                            End If

                        Case "body"
                            ' Ignore

                        Case "end-region", "end-property"
                            iStopLine = GetLastEndLine(node)    ' node is used, see GetLastEndLine!
                            WriteStreamLines(streamWriter, VbCode, iStartLine, iStopLine)
                            iStartLine = iStopLine + 1

                        Case Else
                            OtherNodeMerge(child, iStartLine, iStopLine, VbCode, streamWriter)
                    End Select

                    If iStartLine = 0 Then
                        iStartLine = iStopLine
                    End If
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub ClassMerging(ByVal node As XmlNode, ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                             ByVal VbCode As VbCodeAnalyser, ByVal streamWriter As StreamWriter)
        Try
            For Each child As XmlNode In node.SelectNodes("*")

                Dim strNodename As String = GetNodeName(child)
                'Debug.Print(child.OuterXml)

                Select Case strNodename
                    Case "vb-doc", "region", "class", "typedef", "method", "attribute"
                        iStopLine = GetStartLine(child) - 1
                        WriteStreamLines(streamWriter, VbCode, iStartLine, iStopLine)
                End Select

                Select Case strNodename
                    Case "vb-doc"
                        WriteStreamVbDoc(streamWriter, child)
                        iStartLine = GetLastEndLine(child) + 1

                    Case "region"
                        WriteStreamBody(streamWriter, child, VbCode)
                        iStartLine = GetStopLine(child) + 1
                        RegionMerging(child, iStartLine, iStopLine, VbCode, streamWriter)

                    Case "end-class"
                        iStopLine = GetLastEndLine(node)    ' node is used, see GetLastEndLine!
                        WriteStreamLines(streamWriter, VbCode, iStartLine, iStopLine)

                        iStartLine = iStopLine + 1

                    Case "body"
                        ' Ignore

                    Case Else
                        OtherNodeMerge(child, iStartLine, iStopLine, VbCode, streamWriter)
                End Select

                If iStartLine = 0 Then
                    iStartLine = iStopLine
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub OtherNodeMerge(ByVal child As XmlNode, ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                             ByVal VbCode As VbCodeAnalyser, ByVal streamWriter As StreamWriter)
        Try
            Dim strNodename As String = GetNodeName(child)

            Dim body As XmlNode = child.SelectSingleNode("body")

            If body IsNot Nothing _
            Then
                WriteStreamBody(streamWriter, child, VbCode)
                iStartLine = GetStopLine(child) + 1
            Else
                iStopLine = GetLastEndLine(child)
                If iStopLine = 0 Then
                    iStopLine = GetStopLine(child)
                    If iStopLine = 0 Then
                        Throw New Exception("Node '" + strNodename + "' has neither attribute 'end' nor child 'end-" + strNodename + "' or value is 0")
                    End If
                End If
                WriteStreamLines(streamWriter, VbCode, GetStartLine(child), iStopLine, IsNotCheck(child))
                iStartLine = iStopLine + 1
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function GetReferenceName(ByVal parent As XmlNode, ByVal currentNode As XmlNode) As XmlNode
        Dim result As XmlNode = Nothing
        Try
            Dim strName As String = GetName(currentNode)
            Dim strNodeName As String = GetNodeName(currentNode)
            Dim strQuery As String = strNodeName + "[@name='" + strName + "']"
            Dim list As XmlNodeList = Nothing

            Select Case strNodeName
                Case "method"
                    If strName = "New" Then
                        strQuery = "method[@name='New' and @params='" + GetParams(currentNode) + "']"
                        result = parent.SelectSingleNode(strQuery)

                        If result Is Nothing Then
                            strQuery = "method[@name='New' and @types='" + GetParamTypes(currentNode) + "']"
                            result = parent.SelectSingleNode(strQuery)
                        End If
                    Else
                        strQuery = "method[@name='" + strName + "' and @params='" + GetParams(currentNode) + "']"
                        result = parent.SelectSingleNode(strQuery)

                        If result Is Nothing Then
                            strQuery = "method[@name='" + strName + "' and @types='" + GetParamTypes(currentNode) + "']"
                            result = parent.SelectSingleNode(strQuery)
                        End If

                        If result Is Nothing Then
                            strQuery = "method[@name='" + strName + "']"
                            list = parent.SelectNodes(strQuery)
                            If list.Count > 0 Then
                                result = list.Item(0)
                                Dim i As Integer = 1

                                While i < list.Count
                                    If IsNotCheck(result) = False Then
                                        i += 1
                                        result = list.Item(i)
                                    Else
                                        Exit While
                                    End If
                                End While

                                If IsNotCheck(result) = False Then
                                    result = Nothing
                                End If
                            End If
                        End If
                    End If

                Case "get", "set"
                    strQuery = strNodeName + "[parent::property/@name='" + strName + "']"
                    result = parent.SelectSingleNode(strQuery)

                Case Else
                    strQuery = strNodeName + "[@name='" + strName + "']"
                    result = parent.SelectSingleNode(strQuery)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function

    Private Shared Function GetNodeName(ByVal node As XmlNode) As String
        If node IsNot Nothing Then
            Return node.Name
        End If
        Return ""
    End Function

    Private Shared Function GetName(ByVal node As XmlNode) As String
        Try
            Select Case node.Name
                Case "get", "set"
                    Return node.ParentNode.Attributes.ItemOf("name").Value
                Case Else
                    Return node.Attributes.ItemOf("name").Value
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function LoadLines(ByVal VbCode As VbCodeAnalyser, ByVal node As XmlNode) As String
        Dim iStop As Integer = GetLastEndLine(node)
        Dim iStart As Integer = GetStartLine(node)
        If iStop = 0 Then iStop = GetStopLine(node)
        If iStop = 0 Then Throw New Exception("Stop line index is null in node:" + vbCrLf + node.OuterXml)
        Return VbCode.LoadLines(iStart, iStop)
    End Function

    Private Shared Sub SetStopLine(ByVal node As XmlNode, ByVal index As Integer)
        SetAttributevalue(node, "end", index)
    End Sub

    Private Shared Sub SetStartLine(ByVal node As XmlNode, ByVal index As Integer)
        SetAttributevalue(node, "start", index)
    End Sub

    Private Shared Sub SetCheck(ByVal node As XmlNode, Optional ByVal value As Boolean = True)
        SetAttributevalue(node, "checked", value)
    End Sub

    Private Shared Sub SetAttributevalue(ByVal node As XmlNode, ByVal name As String, ByVal value As Object)
        Dim attrib As XmlAttribute = node.Attributes.ItemOf(name)
        If attrib Is Nothing Then
            attrib = node.Attributes.Append(node.OwnerDocument.CreateAttribute(name))
        End If
        attrib.Value = value.ToString
    End Sub

    Private Shared Function IsNotCheck(ByVal node As XmlNode) As Boolean
        Return (GetChecked(node) = False)
    End Function

    Private Shared Function GetStartLine(ByVal node As XmlNode) As Integer
        Return CInt(node.Attributes.ItemOf("start").Value)
    End Function

    Private Shared Function GetChecked(ByVal node As XmlNode) As Boolean
        If node.Attributes.ItemOf("checked") IsNot Nothing Then
            Return (node.Attributes.ItemOf("checked").Value = "True")
        End If
        Return True
    End Function

    Private Shared Function GetStopLine(ByVal node As XmlNode) As Integer
        If node.Attributes.ItemOf("end") IsNot Nothing Then
            Return CInt(node.Attributes.ItemOf("end").Value)
        End If
        Return 0
    End Function

    Private Shared Function GetParamTypes(ByVal node As XmlNode) As String
        If node.Attributes.ItemOf("types") IsNot Nothing Then
            Return node.Attributes.ItemOf("types").Value
        End If
        Return "False"
    End Function

    Private Shared Function GetParams(ByVal node As XmlNode) As String
        If node.Attributes.ItemOf("params") IsNot Nothing Then
            Return node.Attributes.ItemOf("params").Value
        End If
        Return "False"
    End Function

    Private Shared Sub SetInsertLine(ByVal before As XmlNode, ByVal iPos As Integer)
        SetStartLine(before, iPos)
        SetStopLine(before, iPos - 1)   ' To force insert
        If GetNodeName(before) <> "attribute" Then SetLastEndLine(before, iPos - 1) ' To force insert
    End Sub

    Private Shared Sub SetLastEndLine(ByVal node As XmlNode, ByVal index As Integer)
        Dim child As XmlNode = node.SelectSingleNode("end-" + node.Name)

        If child Is Nothing Then
            child = node.AppendChild(node.OwnerDocument.CreateNode(XmlNodeType.Element, "end-" + node.Name, ""))
        End If

        child.InnerText = index.ToString
    End Sub

    Private Shared Function GetLastEndLine(ByVal node As XmlNode) As Integer
        Dim last As XmlNode = node.SelectSingleNode("end-" + node.Name)
        If last IsNot Nothing Then
            Return CInt(last.InnerText)
        End If
        Return 0
    End Function

    Private Shared Sub SetPosition(ByVal node As XmlNode, ByVal index As Integer)
        SetAttributevalue(node, "pos", index.ToString)
    End Sub

    Private Shared Function GetPosition(ByVal node As XmlNode) As Integer
        If node.Attributes.ItemOf("pos") IsNot Nothing Then
            Return CInt(node.Attributes.ItemOf("pos").Value)
        End If
        Return 1
    End Function

    Private Shared Sub WriteStreamVbDoc(ByVal stream As StreamWriter, ByVal node As XmlNode)

        Dim iPos As Integer = GetPosition(node)
        Dim iStart As Integer = GetStartLine(node)

        If IsNotCheck(node) Then
            Dim body As String() = node.FirstChild.InnerText.Split(vbLf)
            Dim tempo As String = "'    " + body(0).Replace("'", " ")

            For i As Integer = 1 To body.Length - 1
                tempo += vbLf + "'    " + body(i).Replace("'", " ")
            Next

            WriteStream(stream, tempo)
        Else
            WriteStreamCode(stream, node.FirstChild, iPos, iStart)
        End If
    End Sub

    Private Shared Sub WriteStreamBody(ByVal stream As StreamWriter, ByVal node As XmlNode, ByVal VbCode As VbCodeAnalyser)

        Dim iPos As Integer = GetPosition(node)
        Dim iStart As Integer = GetStartLine(node)

        WriteStreamCode(stream, node.SelectSingleNode("body"), iPos, iStart, VbCode)
    End Sub

    Private Shared Sub WriteStreamCode(ByVal stream As StreamWriter, ByVal node As XmlNode, _
                                       ByVal iPos As Integer, ByVal iStart As Integer, _
                                       Optional ByVal VbCode As VbCodeAnalyser = Nothing)
        Try
            If node Is Nothing Then
                Exit Sub
            End If
            Dim body As String() = node.InnerText.Split(vbLf)
            Dim tempo As String = body(0)

            Dim strSpaces As String = ""

            If iPos > 1 Then
                ' Compute old indentation
                tempo = Strings.Space(iPos - 1) + LTrim(tempo)
                strSpaces = Strings.Space(tempo.Length - body(0).Length)

                If VbCode IsNot Nothing Then
                    ' Get back old attributelist declaration if exists
                    Dim oldLine As String = VbCode.LoadLines(iStart, iStart)
                    tempo = oldLine.Substring(0, iPos - 1) + LTrim(body(0))
                End If
            End If

            For i As Integer = 1 To body.Length - 1
                tempo += vbLf + strSpaces + body(i)
            Next

            WriteStream(stream, tempo)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub WriteStreamLines(ByVal stream As StreamWriter, ByVal VbCode As VbCodeAnalyser, _
                                 ByVal iStartLine As Integer, ByVal iStopLine As Integer, _
                                 Optional ByVal bIsNotChecked As Boolean = False)
        Try
            If iStartLine <= iStopLine Then

                Debug.Print("WriteStreamLines: start=" + iStartLine.ToString + " - stop=" + iStopLine.ToString)

                If bIsNotChecked Then
                    Dim body As String() = VbCode.LoadLines(iStartLine, iStopLine).Split(vbLf)
                    Dim tempo As String = "'    " + body(0)

                    For i As Integer = 1 To body.Length - 1
                        tempo += vbLf + "'    " + body(i)
                    Next

                    WriteStream(stream, tempo)
                Else
                    WriteStream(stream, VbCode.LoadLines(iStartLine, iStopLine))
                End If
                Debug.Print("WriteStreamLines ==========================================")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub WriteStream(ByVal stream As StreamWriter, ByVal text As String)
        stream.WriteLine(text)
        Debug.Print(text)
    End Sub
#End Region

End Class
