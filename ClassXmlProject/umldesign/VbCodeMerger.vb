Imports System
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic

Public Class VbCodeMerger

#Region "Public methods"

    Public Shared Function Merge(ByVal strFolder As String, ByVal strFilename As String, _
                                 ByVal strTempFolder As String) As Boolean
        Try
            Using PreviousModule As New VbCodeAnalyser
                Using NextModule As New VbCodeAnalyser

                    PreviousModule.Analyse(strFolder, strFilename, ".prev", strTempFolder)
                    NextModule.Analyse(My.Computer.FileSystem.CombinePath(strFolder, strTempFolder), strFilename, ".next")

                    Dim newDocXml As New XmlDocument
                    newDocXml.AppendChild(newDocXml.CreateNode(XmlNodeType.Element, "root", ""))

                    CompareFiles(PreviousModule, NextModule, newDocXml)

                    ' Only for debug version
                    NextModule.Document.Save(My.Computer.FileSystem.CombinePath(strFolder, ".umlexp\test2.xml"))
                    newDocXml.Save(My.Computer.FileSystem.CombinePath(strFolder, ".umlexp\test.xml"))

                    MergeFiles(NextModule, newDocXml)

                    ' Only for debug version
                    newDocXml.Save(My.Computer.FileSystem.CombinePath(strFolder, ".umlexp\test.xml"))

                    ProcessMerging(newDocXml, PreviousModule, strFolder, strFilename)
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

#End Region

#Region "Private methods"

    Private Shared Sub CompareFiles(ByVal PrevModule As VbCodeAnalyser, ByVal NextModule As VbCodeAnalyser, _
                             ByVal newDocXml As XmlDocument)

        Dim prevDocXml As XmlDocument = PrevModule.Document
        Dim nextDocXml As XmlDocument = NextModule.Document
        Dim clone, current, body As XmlNode

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
    End Sub

    Private Shared Sub MergeFiles(ByVal NextModule As VbCodeAnalyser, ByVal newDocXml As XmlDocument)
        ' Only one class in the "next" file
        Dim firstParent As XmlNode = NextModule.Document.SelectSingleNode("//class")
        ' Only the first class id generated
        Dim secondParent As XmlNode = newDocXml.DocumentElement.SelectSingleNode("//class")

        MergeNodes(firstParent, secondParent, NextModule, newDocXml)
    End Sub

    Private Shared Sub MergeNodes(ByVal firstParent As XmlNode, ByVal secondParent As XmlNode, _
                                  ByVal NextModule As VbCodeAnalyser, ByVal newDocXml As XmlDocument)

        Dim vbdoc As XmlNode = Nothing
        Dim previous As XmlNode = Nothing
        Dim body, clone As XmlNode

        For Each child In firstParent.ChildNodes()
            Select GetNodeName(child)
                Case "region"
                    Dim second As XmlNode = GetReferenceName(secondParent, child)
                    MergeNodes(child, second, NextModule, newDocXml)

                Case "vb-doc"
                    If IsNotCheck(child) Then
                        vbdoc = child
                    End If

                Case Else
                    If IsNotCheck(child) = False _
                    Then
                        previous = child
                    Else
                        clone = newDocXml.ImportNode(child, True)
                        Dim iPos As Integer = 0

                        If previous IsNot Nothing _
                        Then
                            Dim before As XmlNode = GetReferenceName(secondParent, previous)
                            If before IsNot Nothing Then
                                before = before.NextSibling
                            End If

                            If before Is Nothing Then
                                before = secondParent.SelectSingleNode("end-region")
                            End If

                            If before Is Nothing Then
                                before = secondParent.SelectSingleNode("end-class")
                            End If

                            Select GetNodeName(before)
                                Case "end-class", "end-region"
                                    iPos = GetLastEndLine(secondParent)
                                Case Else
                                    iPos = GetStartLine(before)
                            End Select

                            secondParent.InsertBefore(clone, before)
                        Else
                            Dim before As XmlNode = secondParent.SelectSingleNode("*[not(self::body)]")
                            iPos = GetStartLine(before)
                            before = secondParent.InsertBefore(clone, before)
                        End If

                        Dim strNewLines As String = ""

                        If vbdoc IsNot Nothing Then
                            strNewLines += vbdoc.FirstChild.InnerText
                        End If

                        strNewLines += LoadLines(NextModule, child)

                        body = newDocXml.CreateNode(XmlNodeType.Element, "body", "")
                        clone.InsertBefore(body, clone.FirstChild)
                        body.InnerText = strNewLines + vbCrLf

                        SetPosition(clone, 1)   ' To force write on first column
                        SetInsertLine(clone, iPos)

                        SetCheck(child)
                        previous = child
                        vbdoc = Nothing
                    End If
            End Select
        Next
    End Sub

    Private Shared Sub ParsePackageDeclaration(ByVal PrevPackageNode As XmlNode, ByVal NextPackageNode As XmlNode, _
                                        ByVal NewPackageNode As XmlNode, ByVal NextModule As VbCodeAnalyser, _
                                        ByRef iStopClassDeclaration As Integer)

        Dim vbdocPrev As XmlNode = Nothing
        Dim clone As XmlNode
        Dim iClassCounter As Integer = 0

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
    End Sub

    Private Shared Sub ParseClassDeclaration(ByVal PrevClassNode As XmlNode, ByVal NextClassNode As XmlNode, ByVal NextModule As VbCodeAnalyser, ByVal NewClassNode As XmlNode)

        Dim vbdocPrev As XmlNode = Nothing

        For Each child As XmlNode In PrevClassNode.SelectNodes("*")

            Dim replaceChild, vbdocNext, clone, body As XmlNode
            Dim strNodeName As String = GetNodeName(child)

            If strNodeName <> "vb-doc" _
            Then
                Select Case strNodeName
                    Case "attribute", "method", "property", "typedef", "class", "region"
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

                                Case Else
                                    strNewLine += NextModule.LoadLines(GetStartLine(replaceChild), GetStopLine(replaceChild))
                                    body.InnerText = strNewLine
                            End Select

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
    End Sub

    Private Shared Sub ProcessMerging(ByVal docXml As XmlDocument, ByVal VbCode As VbCodeAnalyser, _
                               ByVal strFolder As String, ByVal strFilename As String)
        Dim iStartLine As Integer = 1
        Dim iStopLine As Integer = 1
        Dim strTemp As String = My.Computer.FileSystem.CombinePath(strFolder, ".umlexp")
        Dim strPath As String = My.Computer.FileSystem.CombinePath(strTemp, strFilename + ".new")
        Try
            Using streamWriter As StreamWriter = New StreamWriter(strPath)

                For Each child As XmlNode In docXml.SelectNodes("/root/*")

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

            Dim strFinalPath As String = My.Computer.FileSystem.CombinePath(strFolder, strFilename)

            'My.Computer.FileSystem.MoveFile(strFinalPath, strFinalPath + ".bak")
            'My.Computer.FileSystem.MoveFile(strPath, strFinalPath)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Shared Sub PackageMerging(ByVal node As XmlNode, ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                             ByVal VbCode As VbCodeAnalyser, ByVal streamWriter As StreamWriter)

        For Each child As XmlNode In node.SelectNodes("*")

            Dim strNodename As String = GetNodeName(child)
            Select Case strNodename
                Case "vb-doc","region","class"
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

    End Sub

    Private Shared Sub RegionMerging(ByVal node As XmlNode, ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                             ByVal VbCode As VbCodeAnalyser, ByVal streamWriter As StreamWriter)

        For Each child As XmlNode In node.SelectNodes("*")

            Dim strNodename As String = GetNodeName(child)

            Select Case strNodename
                Case "vb-doc", "region", "class", "typedef", "method", "attribute"
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

                Case "body"
                    ' Ignore

                Case "end-region"
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
    End Sub

    Private Shared Sub ClassMerging(ByVal node As XmlNode, ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                             ByVal VbCode As VbCodeAnalyser, ByVal streamWriter As StreamWriter)

        For Each child As XmlNode In node.SelectNodes("*")

            Dim strNodename As String = GetNodeName(child)

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
    End Sub

    Private Shared Sub OtherNodeMerge(ByVal child As XmlNode, ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                             ByVal VbCode As VbCodeAnalyser, ByVal streamWriter As StreamWriter)

        Dim strNodename As String = GetNodeName(child)

        iStopLine = GetStartLine(child) - 1
        WriteStreamLines(streamWriter, VbCode, iStartLine, iStopLine)

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
            WriteStreamLines(streamWriter, VbCode, GetStartLine(child), iStopLine)
            iStartLine = iStopLine + 1
        End If
    End Sub

    Private Shared Function GetReferenceName(ByVal node As XmlNode, ByVal Child As XmlNode) As XmlNode
        Dim strName As String = GetName(Child)
        Dim strNodeName As String = GetNodeName(Child)
        Dim strQuery As String = strNodeName + "[@name='" + strName + "']"

        Select Case strNodeName
            Case "method"
                If strName = "New" Then
                    strQuery = "method[@name='New' and @params='" + GetParams(Child) + "']"
                End If

            Case "region"
                strQuery = strNodeName + "[@name='" + strName + "']"
        End Select
        Return node.SelectSingleNode(strQuery)
    End Function

    Private Shared Function GetNodeName(ByVal node As XmlNode) As String
        If node IsNot Nothing Then
            Return node.Name
        End If
        Return ""
    End Function

    Private Shared Function GetName(ByVal node As XmlNode) As String
        Return node.Attributes.GetNamedItem("name").Value
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
        Dim attrib As XmlAttribute = node.Attributes.GetNamedItem(name)
        If attrib Is Nothing Then
            attrib = node.Attributes.Append(node.OwnerDocument.CreateAttribute(name))
        End If
        attrib.Value = value.ToString
    End Sub

    Private Shared Function IsNotCheck(ByVal node As XmlNode) As Boolean
        Return (GetChecked(node) = False)
    End Function

    Private Shared Function GetStartLine(ByVal node As XmlNode) As Integer
        Return Val(node.Attributes.GetNamedItem("start").Value)
    End Function

    Private Shared Function GetChecked(ByVal node As XmlNode) As Boolean
        If node.Attributes.GetNamedItem("checked") IsNot Nothing Then
            Return (node.Attributes.GetNamedItem("checked").Value = "True")
        End If
        Return True
    End Function

    Private Shared Function GetStopLine(ByVal node As XmlNode) As Integer
        If node.Attributes.GetNamedItem("end") IsNot Nothing Then
            Return Val(node.Attributes.GetNamedItem("end").Value)
        End If
        Return 0
    End Function

    Private Shared Function GetParams(ByVal node As XmlNode) As String
        If node.Attributes.GetNamedItem("params") IsNot Nothing Then
            Return node.Attributes.GetNamedItem("params").Value
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
            Return Val(last.InnerText)
        End If
        Return 0
    End Function

    Private Shared Sub SetPosition(ByVal node As XmlNode, ByVal index As Integer)
        SetAttributevalue(node, "pos", index.ToString)
    End Sub

    Private Shared Function GetPosition(ByVal node As XmlNode) As Integer
        If node.Attributes.GetNamedItem("pos") IsNot Nothing Then
            Return Val(node.Attributes.GetNamedItem("pos").Value)
        End If
        Return 1
    End Function

    Private Shared Sub WriteStreamVbDoc(ByVal stream As StreamWriter, ByVal node As XmlNode)

        Dim iPos As Integer = GetPosition(node)
        Dim iStart As Integer = GetStartLine(node)

        WriteStreamCode(stream, node.FirstChild, iPos, iStart)
    End Sub

    Private Shared Sub WriteStreamBody(ByVal stream As StreamWriter, ByVal node As XmlNode, ByVal VbCode As VbCodeAnalyser)

        Dim iPos As Integer = GetPosition(node)
        Dim iStart As Integer = GetStartLine(node)

        WriteStreamCode(stream, node.SelectSingleNode("body"), iPos, iStart, VbCode)
    End Sub

    Private Shared Sub WriteStreamCode(ByVal stream As StreamWriter, ByVal node As XmlNode, _
                                       ByVal iPos As Integer, ByVal iStart As Integer, _
                                       Optional ByVal VbCode As VbCodeAnalyser = Nothing)

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

        WriteStream(stream, tempo + vbLf)

        Dim iStop As Integer = body.Length - 1
        If body(iStop) = "" Then iStop -= 1

        For i As Integer = 1 To iStop
            WriteStream(stream, strSpaces + body(i) + vbLf)
        Next
    End Sub

    Private Shared Sub WriteStreamLines(ByVal streamWriter As StreamWriter, ByVal VbCode As VbCodeAnalyser, _
                                 ByVal iStartLine As Integer, ByVal iStopLine As Integer)

        If iStartLine <= iStopLine Then
            WriteStream(streamWriter, VbCode.LoadLines(iStartLine, iStopLine))
        End If
    End Sub

    Private Shared Sub WriteStream(ByVal stream As StreamWriter, ByVal text As String)

        stream.Write(text)

        Dim tempo As String = text
        If Strings.Right(tempo, 2) = vbCrLf Then
            tempo = tempo.Substring(0, tempo.Length - 2)
        End If
        Debug.Print(tempo)
    End Sub
#End Region

End Class
