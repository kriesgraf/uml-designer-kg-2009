Imports System
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic

Public Class VbCodeMerger
    Public Function Merge(ByVal strFolder As String, ByVal strFilename As String) As Boolean
        Try
            Dim PreviousModule As New VbCodeAnalyser
            Dim NextModule As New VbCodeAnalyser

            PreviousModule.Analyse(strFolder, strFilename, ".prev", ".umlexp")
            Dim prevDocXml As XmlDocument = PreviousModule.Document

            NextModule.Analyse(My.Computer.FileSystem.CombinePath(strFolder, ".umlexp"), strFilename, ".next")
            Dim nextDocXml As XmlDocument = NextModule.Document

            Dim replaceChild As XmlNode
            Dim newDocXml As New XmlDocument
            Dim clone, current, body As XmlNode

            newDocXml.AppendChild(newDocXml.CreateNode(XmlNodeType.Element, "root", ""))
            current = newDocXml.DocumentElement
            
            ' Add immediately "Imports" if not exists in previous source
            If prevDocXml.SelectSingleNode("/root/imports") Is Nothing Then
                replaceChild = nextDocXml.SelectSingleNode("/root/imports")

                If replaceChild IsNot Nothing Then
                    clone = newDocXml.ImportNode(replaceChild, False)
                    current.AppendChild(clone)
                    body = newDocXml.CreateNode(XmlNodeType.Element, "body", "")
                    clone.AppendChild(body)
                    body.InnerText = NextModule.LoadLines(GetStartLine(replaceChild), GetLastEndLine(replaceChild))
                    SetStartLine(clone, 1)
                    SetLastEndLine(clone, "import", 0)  ' We set to 0 to force insert line
                End If
            End If

            If prevDocXml.SelectSingleNode("/root/package") Is Nothing Then
                replaceChild = nextDocXml.SelectSingleNode("/root/package")

                If replaceChild IsNot Nothing Then
                    clone = newDocXml.ImportNode(replaceChild, False)
                    current.AppendChild(clone)
                    body = newDocXml.CreateNode(XmlNodeType.Element, "body", "")
                    clone.AppendChild(body)
                    body.InnerText = NextModule.LoadLines(GetStartLine(replaceChild), GetStopLine(replaceChild))

                    ParsePackageDeclaration(prevDocXml.DocumentElement, replaceChild, clone, NextModule)

                    'SetStartLine(clone, 1)
                    'SetLastEndLine(clone, "import", 0)  ' We set to 0 to force insert line
                Else
                    ParsePackageDeclaration(prevDocXml.DocumentElement, nextDocXml.DocumentElement, current, NextModule)
                End If
            Else
                ParsePackageDeclaration(prevDocXml.DocumentElement, nextDocXml.DocumentElement, current, NextModule)
            End If

            newDocXml.Save(My.Computer.FileSystem.CombinePath(strFolder, ".umlexp\test.xml"))
            ProcessMerging(newDocXml, PreviousModule, strFolder)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Sub ParsePackageDeclaration(ByVal PrevPackageNode As XmlNode, ByVal NextPackageNode As XmlNode, ByVal NewPackageNode As XmlNode, ByVal NextModule As VbCodeAnalyser)
        Dim vbdocPrev, vbdocNext, body, clone As XmlNode
        Dim replaceChild As XmlNode

        For Each child As XmlNode In PrevPackageNode.SelectNodes("*")
            Dim strNodeName As String = GetNodeName(child)
            If strNodeName <> "vb-doc" _
            Then
                replaceChild = NextPackageNode.SelectSingleNode(strNodeName + "[@name='" + GetName(child) + "']")

                If replaceChild Is Nothing _
                Then
                    vbdocNext = Nothing

                ElseIf GetReferenceName(replaceChild.PreviousSibling, child) = "vb-doc" _
                Then
                    vbdocNext = replaceChild.PreviousSibling
                Else
                    vbdocNext = Nothing
                End If

                Select Case strNodeName
                    Case "imports"
                        If replaceChild IsNot Nothing Then
                            clone = NewPackageNode.OwnerDocument.ImportNode(child, False)
                            NewPackageNode.AppendChild(clone)

                            body = NewPackageNode.OwnerDocument.CreateNode(XmlNodeType.Element, "body", "")
                            clone.AppendChild(body)
                            body.InnerText = NextModule.LoadLines(GetStartLine(replaceChild), GetLastEndLine(replaceChild))
                            SetLastEndLine(clone, "imports", GetLastEndLine(child))
                        End If

                    Case "class"
                        If replaceChild IsNot Nothing _
                        Then
                            ' Clone only 'class' node and replace each declaration
                            If vbdocPrev IsNot Nothing Then
                                clone = NewPackageNode.OwnerDocument.ImportNode(vbdocPrev, True)
                                NewPackageNode.AppendChild(clone)
                                If vbdocNext IsNot Nothing Then
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
                            ' Clone 'class' node deeply to see in next pass
                            clone = NewPackageNode.OwnerDocument.ImportNode(child, True)
                            NewPackageNode.AppendChild(clone)
                        End If
                    Case "vb-doc"
                        ' Continue...
                End Select

                vbdocPrev = Nothing
            Else
                vbdocPrev = child
                vbdocNext = Nothing
                replaceChild = Nothing
            End If

        Next
    End Sub

    Private Sub ParseClassDeclaration(ByVal PrevClassNode As XmlNode, ByVal NextClassNode As XmlNode, ByVal NextModule As VbCodeAnalyser, ByVal NewClassNode As XmlNode)

        Dim vbdocPrev As XmlNode = Nothing

        For Each child As XmlNode In PrevClassNode.SelectNodes("*")

            Dim replaceChild, vbdocNext, clone, body As XmlNode
            Dim strNodeName As String = GetNodeName(child)

            If strNodeName <> "vb-doc" _
            Then
                Select Case strNodeName
                    Case "attribute", "method", "property", "typedef", "class"
                        replaceChild = GetReferenceName(NextClassNode, child)

                        If replaceChild Is Nothing _
                        Then
                            vbdocNext = Nothing

                        ElseIf GetReferenceName(replaceChild.PreviousSibling, child) = "vb-doc" _
                        Then
                            vbdocNext = replaceChild.PreviousSibling
                        Else
                            vbdocNext = Nothing
                        End If

                        If replaceChild IsNot Nothing _
                        Then
                            ' Clone only 'class' node and replace each declaration
                            If vbdocPrev IsNot Nothing Then
                                clone = NewClassNode.OwnerDocument.ImportNode(vbdocPrev, True)
                                NewClassNode.AppendChild(clone)
                                If vbdocNext IsNot Nothing Then
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
                                Case "class", "typedef"
                                    SetStopLine(clone, GetLastEndLine(child))
                                    strNewLine += LoadNestedClass(replaceChild, NextModule)
                                Case Else
                                    strNewLine += NextModule.LoadLines(GetStartLine(replaceChild), GetStopLine(replaceChild))
                            End Select

                            body.InnerText = strNewLine
                        Else
                            ' Clone 'class' node deeply to see in next pass
                            clone = NewClassNode.OwnerDocument.ImportNode(child, True)
                            NewClassNode.AppendChild(clone)
                        End If

                    Case Else
                        ' Clone 'class' node deeply to see in next pass
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

    Private Function LoadNestedClass(ByVal node As XmlNode, ByVal VbCode As VbCodeAnalyser)
        Return VbCode.LoadLines(GetStartLine(node), GetLastEndLine(node))
    End Function

    Private Sub ProcessMerging(ByVal docXml As XmlDocument, ByVal VbCode As VbCodeAnalyser, ByVal strFolder As String)
        Dim iStartLine As Integer = 1
        Dim iStopLine As Integer = 1
        Using streamWriter As StreamWriter = New StreamWriter(My.Computer.FileSystem.CombinePath(strFolder, "TestFile.vb"))

            For Each child As XmlNode In docXml.SelectNodes("/root/*")

                iStopLine = GetStartLine(child) - 1
                streamWriter.Write(VbCode.LoadLines(iStartLine, iStopLine))

                Dim strNodename As String = GetNodeName(child)
                Select Case strNodename
                    Case "vb-doc"
                        streamWriter.Write(child.FirstChild.InnerText)
                        iStartLine = GetLastEndLine(child) + 1

                    Case "imports"
                        streamWriter.Write(child.SelectSingleNode("body").InnerText)
                        iStartLine = GetLastEndLine(child) + 1

                    Case "class"
                        streamWriter.Write(child.SelectSingleNode("body").InnerText)
                        iStartLine = GetStopLine(child) + 1
                        ClassMerging(child, iStartLine, iStopLine, VbCode, streamWriter)

                    Case Else
                        streamWriter.Write(child.SelectSingleNode("body").InnerText)
                        iStartLine = GetStopLine(child) + 1
                End Select

                If iStartLine = 0 Then
                    iStartLine = iStopLine
                End If
            Next
            streamWriter.Close()
        End Using
    End Sub

    Private Sub ClassMerging(ByVal node As XmlNode, ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                             ByVal VbCode As VbCodeAnalyser, ByVal streamWriter As StreamWriter)

        For Each child As XmlNode In node.SelectNodes("*")
            Dim body As XmlNode
            Dim strNodename As String = GetNodeName(child)
            If strNodename <> "body" Then

                Select Case strNodename
                    Case "vb-doc"
                        iStopLine = GetStartLine(child) - 1
                        streamWriter.Write(VbCode.LoadLines(iStartLine, iStopLine))

                        streamWriter.Write(child.FirstChild.InnerText)
                        iStartLine = GetLastEndLine(child) + 1

                    Case "end-class"
                        iStopLine = GetLastEndLine(node)    ' node is used, see GetLastEndLine!
                        streamWriter.Write(VbCode.LoadLines(iStartLine, iStopLine))
                        iStartLine = iStopLine + 1

                    Case "body"
                        ' Ignore

                    Case Else
                        iStopLine = GetStartLine(child) - 1
                        streamWriter.Write(VbCode.LoadLines(iStartLine, iStopLine))

                        body = child.SelectSingleNode("body")

                        If body IsNot Nothing _
                        Then
                            streamWriter.Write(body.InnerText)
                            iStartLine = GetStopLine(child) + 1
                        Else
                            iStopLine = GetLastEndLine(child)
                            streamWriter.Write(VbCode.LoadLines(GetStartLine(child), iStopLine))
                            iStartLine = iStopLine + 1
                        End If
                End Select

                If iStartLine = 0 Then
                    iStartLine = iStopLine
                End If
            End If
        Next
    End Sub

    Private Shared Function GetReferenceName(ByVal node As XmlNode, ByVal Child As XmlNode)
        Dim strName As String = GetName(Child)
        Dim strNodeName As String = GetNodeName(Child)
        If strNodeName = "method" And strName = "New" Then
            Return node.SelectSingleNode("method[@name='New' and @params='" + GetParams(Child) + "']")
        End If
        Return node.SelectSingleNode(strNodeName + "[@name='" + strName + "']")
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

    Private Sub SetStopLine(ByVal node As XmlNode, ByVal index As Integer)
        Dim attrib As XmlAttribute = node.Attributes.GetNamedItem("end")
        If attrib Is Nothing Then
            attrib = node.Attributes.Append(node.OwnerDocument.CreateAttribute("end"))
        End If
        attrib.Value = index.ToString
    End Sub

    Private Sub SetStartLine(ByVal node As XmlNode, ByVal index As Integer)
        Dim attrib As XmlAttribute = node.Attributes.GetNamedItem("start")
        If attrib Is Nothing Then
            attrib = node.Attributes.Append(node.OwnerDocument.CreateAttribute("start"))
        End If
        attrib.Value = index.ToString
    End Sub

    Private Shared Function GetStartLine(ByVal node As XmlNode) As Integer
        Return Val(node.Attributes.GetNamedItem("start").Value)
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

    Private Shared Sub SetLastEndLine(ByVal node As XmlNode, ByVal strElement As String, ByVal index As Integer)
        Dim child As XmlNode = node.LastChild

        If child IsNot Nothing Then
            If child.Name <> "end-" + strElement Then
                child = Nothing
            End If
        End If

        If child Is Nothing Then
            child = node.AppendChild(node.OwnerDocument.CreateNode(XmlNodeType.Element, "end-" + strElement, ""))
        End If

        child.InnerText = index.ToString
    End Sub

    Private Shared Function GetLastEndLine(ByVal node As XmlNode) As Integer
        If node.LastChild IsNot Nothing Then
            Return Val(node.LastChild.InnerText)
        End If
        Return 0
    End Function
End Class
