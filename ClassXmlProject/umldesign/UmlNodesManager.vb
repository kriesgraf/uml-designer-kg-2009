Imports System
Imports System.Xml
Imports System.Collections
Imports ClassXmlProject.XmlProjectTools
Imports System.Text
Imports Microsoft.VisualBasic

#If _APP_UML = "1" Then
Imports ClassXmlProject.UmlCodeGenerator
#End If


Public Enum ELanguage
    Language_CplusPlus = 0
    Language_Java = 1
    Language_Vbasic = 2
End Enum

Public Class UmlNodesManager

#If _APP_UML = "1" Then

    Public Shared Sub AddSimpleTypesList(ByRef myList As ArrayList, ByVal eTag As ELanguage)
        Try
            Dim doc As New XmlDocument
            LoadDocument(doc, GetSimpleTypesFilename(eTag))

            Dim iterator As IEnumerator = doc.SelectNodes("//type").GetEnumerator()
            iterator.Reset()

            While iterator.MoveNext()
                myList.Add(New XmlNodeListView(GetCurrentName(iterator)))
            End While
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function ImportNodes(ByVal component As XmlComposite, _
                                       ByVal strFilename As String, _
                                       ByVal NodeCounter As XmlReferenceNodeCounter, _
                                       ByVal bUpdateOnly As Boolean) As Boolean
        Try
            Dim source As New XmlDocument
            Dim import As XmlComponent
            Dim child As XmlNode

            If bUpdateOnly Then
                MsgBox("Not yet implemented !", MsgBoxStyle.Exclamation)
                Return False
            End If

            LoadDocument(source, strFilename)

            import = New XmlComponent(source.DocumentElement)
            import.Tag = component.Tag

            StartInsertion(source, NodeCounter)
            Dim list As XmlNodeList

            ' Remove redundant imported references
            list = import.SelectNodes("//import/export/reference")

            For Each child In list
                VerifyRedundancy(component, "project/package " + component.Name, child, "file:" + vbCrLf + strFilename)
            Next child

            ' Remove redundant references in current project with imported classes
            list = component.SelectNodes("//import/export/reference")

            For Each child In list
                VerifyRedundancy(import, "file:" + vbCrLf + strFilename + vbCrLf, child, "project/package " + component.Name)
            Next child

            For Each child In import.Node.ChildNodes
                Select Case child.Name
                    Case "relationship"
                        component.Document.DocumentElement.AppendChild(component.Document.ImportNode(child, True))

                    Case "generation", "comment"
                        ' Nothing todo 

                    Case Else
                        Dim xmlcpnt As XmlComponent = XmlNodeManager.GetInstance().CreateDocument(component.Document.ImportNode(child, True))
                        component.AppendComponent(xmlcpnt)
                End Select
            Next child
            Return True

        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Public Shared Sub ExportNodes(ByVal node As XmlNode, ByVal strFilename As String, _
                                    ByVal strFullpathPackage As String)
        Dim source As New XmlDocument
        Dim strXML As String
        Dim strComment As String

        strComment = GetNodeString(node, "comment/@brief")

        Dim strPath As String = GetProjectPath(strFilename)
        If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
        strXML = "<?xml version='1.0' encoding='iso-8859-1'?>"
        strXML += GetDtdDeclaration("root")

        strXML += vbCrLf + "<root name='" + strFullpathPackage + "'>"
        strXML += vbCrLf + "<generation destination='c:\' language='0'/>"
        strXML += vbCrLf + "<comment brief='" + strComment + "'/>"
        strXML += vbCrLf + "<import name='" + GetName(node) + "_unreferenced' param='' visibility='package'>"
        strXML += vbCrLf + "<export name='" + GetName(node) + "_unreferenced'>"
        strXML += vbCrLf + "</export>"
        strXML += vbCrLf + "</import>"
        strXML += vbCrLf + "</root>" + vbCrLf

        source.LoadXml(strXML)

        Dim child As XmlNode
        Dim listID As New SortedList

        child = node.CloneNode(True)

        GetListDescendant(child, listID, "collaboration/@idref")
        ExportRelationships(source, node, listID)

        AddNodesAndLinkUnreferencedNodes(source, node, child)
        child = GetNode(source, "/root/import/export")
        If child.HasChildNodes = False Then
            RemoveNode(child.ParentNode)
        End If
        source.Save(strFilename)
    End Sub

    Public Shared Sub ExportClassReferences(ByVal node As XmlNode, ByVal strFilename As String, _
                                            ByVal strFullpathPackage As String)
        Try
            Dim source As New XmlDocument
            Dim strXML As String
            Dim index As Integer = 1
            Dim strPath As String = GetProjectPath(strFilename)

            If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
            strXML = "<?xml version='1.0' encoding='iso-8859-1'?>"
            strXML += GetDtdDeclaration("export")
            strXML += vbCrLf + "<export name='" + GetName(node) + "' source='" + strFullpathPackage + "'>" + vbCrLf
            strXML += vbCrLf + ExportElementClass(node, "")
            strXML += vbCrLf + "</export>"

            source.LoadXml(strXML)
            source.Save(strFilename)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub ExportPackageReferences(ByVal node As XmlNode, ByVal strFilename As String, _
                                              ByVal strFullpathPackage As String, ByVal strSeparator As String)
        Try
            Dim source As New XmlDocument
            Dim strXML As String
            Dim index As Integer = 1
            Dim strPath As String = GetProjectPath(strFilename)

            If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
            strXML = "<?xml version='1.0' encoding='iso-8859-1'?>"
            strXML += GetDtdDeclaration("export")

            strXML += vbCrLf + "<export name='" + GetName(node) + "' source='" + strFullpathPackage + "'>" + vbCrLf

            strXML += vbCrLf + ExportElementPackage(node, "", strSeparator)

            strXML += vbCrLf + "</export>"

            source.LoadXml(strXML)
            source.Save(strFilename)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub ExportTypedefReferences(ByVal node As XmlNode, ByVal strFilename As String, _
                                              ByVal strFullpathPackage As String)
        Try
            Dim source As New XmlDocument
            Dim strXML As String
            Dim strPath As String = GetProjectPath(strFilename)

            If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
            strXML = "<?xml version='1.0' encoding='iso-8859-1'?>"
            strXML += GetDtdDeclaration("export")

            strXML += vbCrLf + "<export name='" + GetName(node) + "' source='" + strFullpathPackage + "'>"
            strXML += vbCrLf + ExportElementTypedef(node.ParentNode, node, "")
            strXML += vbCrLf + "</export>"
            'Debug.Print(strXML)
            source.LoadXml(strXML)
            source.Save(strFilename)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub ReExport(ByVal node As XmlNode, ByVal strFilename As String, ByVal strFullpathPackage As String)
        Try
            Dim source As New XmlDocument
            'Dim typedef As XmlNode
            Dim strXML As String
            Dim strPath As String = GetProjectPath(strFilename)

            If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
            strXML = "<?xml version='1.0' encoding='iso-8859-1'?>"
            strXML += GetDtdDeclaration("export")
            strXML += vbCrLf + "<export name='" + GetName(node) + "' source='" + strFullpathPackage + "'>"

            For Each ref As XmlNode In node.ChildNodes
                Dim xmlcpnt As XmlReferenceSpec = XmlNodeManager.GetInstance().CreateDocument(ref)
                strXML += vbCrLf + xmlcpnt.OuterXml
            Next
            strXML += vbCrLf + "</export>"
            'Debug.Print(strXML)
            source.LoadXml(strXML)
            Dim list As XmlNodeList = source.SelectNodes("//collaboration")
            For Each child As XmlNode In list
                child.ParentNode.RemoveChild(child)
            Next child

            source.Save(strFilename)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function ExtractReferences(ByVal node As XmlNode, ByVal strFullpathPackage As String, _
                                             ByVal strSeparator As String) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim parent As XmlNode = node.ParentNode
            Dim xmlParent As XmlComponent = XmlNodeManager.GetInstance().CreateDocument(parent)

            Dim strXML As String = "<export name='" + GetName(node) + "' source='" + strFullpathPackage + "'>"
            Select Case node.Name
                Case "class"
                    strXML += vbCrLf + ExportElementClass(node, "")
                Case "package"
                    strXML += vbCrLf + ExportElementPackage(node, "", strSeparator)
            End Select
            strXML += vbCrLf + "</export>"

            Dim importXML As XmlImportSpec = XmlNodeManager.GetInstance().CreateDocument("import", parent.OwnerDocument)
            If importXML.LoadXml(strXML) Then
                xmlParent.AppendComponent(importXML)
                parent.RemoveChild(node)
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Function UpdateSimpleTypes(ByVal strFilename As String) As Boolean
        Dim fen As New dlgSimpleTypes
        fen.Filename = strFilename
        fen.ShowDialog()
        Return CType(fen.Tag, Boolean)
    End Function

    Private Shared Function ExportElementPackage(ByVal node As XmlNode, ByVal strCurrentPackage As String, _
                                              ByVal strSeparator As String) As String
        Dim strXML As String = ""
        Try
            If strCurrentPackage = "" Then
                strCurrentPackage = GetName(node)
            Else
                strCurrentPackage += strSeparator + GetName(node)
            End If
            For Each current As XmlNode In node.SelectNodes("class | package")
                If current.Name = "class" Then
                    strXML += vbCrLf + ExportElementClass(current, strCurrentPackage)
                Else
                    strXML += vbCrLf + ExportElementPackage(current, strCurrentPackage, strSeparator)
                End If
            Next current
        Catch ex As Exception
            Throw ex
        End Try
        Return strXML
    End Function

    Private Shared Function ExportElementClass(ByVal node As XmlNode, ByVal strCurrentPackage As String) As String
        Dim strXML As String = ""
        Try
            strXML += "<reference name='" + GetName(node) + "' type='class'"

            If strCurrentPackage <> "" Then strXML += " package='" + strCurrentPackage + "'"

            Dim iContainer = node.SelectNodes("model").Count
            strXML += " container='" + CStr(iContainer) + "' id='" + GetID(node) + "'/>"

            For Each typedef In SelectNodes(node, "typedef[variable/@range='public']")
                strXML += vbCrLf + ExportElementTypedef(node, typedef, strCurrentPackage)
            Next typedef

        Catch ex As Exception
            Throw ex
        End Try
        Return strXML
    End Function

    Private Shared Function ExportElementTypedef(ByVal node As XmlNode, ByVal typedef As XmlNode, _
                                              ByVal strCurrentPackage As String) As String
        Dim strXML As String = ""
        Try
            strXML += vbCrLf + "<reference name='" + GetName(typedef) + "' class='" + GetName(node) + "'"

            If strCurrentPackage <> "" Then strXML += " package='" + strCurrentPackage + "'"

            strXML += " type='typedef' container='0' id='" + GetID(typedef) + "'/>"
        Catch ex As Exception
            Throw ex
        End Try
        Return strXML
    End Function

    Private Shared Sub GetListDescendant(ByRef node As XmlNode, ByRef list As SortedList, ByVal strQuery As String, Optional ByVal strName As String = ".")
        Dim listNodes As XmlNodeList
        Dim child As XmlNode
        Dim strID As String

        listNodes = SelectNodes(node, strQuery)

        For Each child In listNodes
            If strName = "." Then
                strID = child.Value
            Else
                strID = GetNodeString(child, strName)
            End If
            If list.Contains(strID) = False Then list.Add(strID, strID)
        Next child

        listNodes = SelectNodes(node, "descendant::*/" + strQuery)

        For Each child In listNodes
            If strName = "." Then
                strID = child.Value
            Else
                strID = GetNodeString(child, strName)
            End If
            If list.Contains(strID) = False Then list.Add(strID, strID)
        Next child
    End Sub

    Private Shared Sub ExportRelationships(ByRef source As XmlDocument, ByVal treeNode As XmlNode, ByRef listID As SortedList)
        Dim relation As XmlNode

        For i As Integer = 0 To listID.Count - 1
            relation = SelectNodeStringId(treeNode, listID.GetByIndex(i).ToString)

            If Not relation Is Nothing Then
                ' Convert "document" context to "MyBase.Document" context
                source.DocumentElement.AppendChild(source.ImportNode(relation, True))
            End If
        Next
    End Sub

    Private Shared Sub AddNodesAndLinkUnreferencedNodes(ByVal source As XmlDocument, ByVal treeNode As XmlNode, ByVal exportNodes As XmlNode)
        Dim listID As New SortedList
        Dim child As XmlNode
        Dim unref As XmlNode
        Dim export As XmlNode
        Dim reference As XmlNode
        Dim strType As String
        Dim strID As String
        Dim strPackage As String

        child = source.ImportNode(exportNodes, True)
        Dim before As XmlNode = GetNode(source, "//relationship")

        If before IsNot Nothing Then
            source.DocumentElement.InsertBefore(child, before)
        Else
            source.DocumentElement.AppendChild(child)
        End If

        export = GetNode(source, "descendant::export")

        ' On remplace les ID des valeurs énumérés qui décrivent la taille d'un tableau 
        ' par leur descripteur texte (name), ça générera du code erroné, mais les références sont 
        ' épargnés et le projet reste lisible
        For Each child In SelectNodes(source, "//*[@sizeref and not(@sizeref=//*/@id)]")
            strID = GetIDREF(child, "sizeref")
            unref = SelectNodeStringId(treeNode, strID)
            RemoveAttribute(child, "sizeref")
            AddAttributeValue(child, "size", GetName(unref))
        Next child

        ' On remplace les ID des valeurs énumérés qui décrivent les valeurs par défaut de propriété et d'argument 
        ' par leur descripteur texte (name), ça générera du code erroné, mais les références sont 
        ' épargnés et le projet reste lisible
        For Each child In SelectNodes(source, "//*[@valref and not(@valref=//*/@id)]")
            strID = GetIDREF(child, "valref")
            unref = SelectNodeStringId(treeNode, strID)
            RemoveAttribute(child, "valref")
            AddAttributeValue(child, "value", GetName(unref))
        Next child

        ' On place les ID dans une collection pour supprimer les redondances
        For Each child In SelectNodes(source, "//*[@idref and not(@idref=//*/@id)]")
            strID = GetIDREF(child)
            If listID.Contains(strID) = False Then
                listID.Add(strID, strID)
            End If
        Next child

        For Each dico As DictionaryEntry In listID

            ' On récupère le vrai noeud de l'arbre original
            ' pour pouvoir nommer la référence dans le fichier d'export
            unref = SelectNodeStringId(treeNode, dico.Value.ToString)
            reference = CreateAppendNode(export, "reference")

            If unref Is Nothing Then
                strType = "class"
            ElseIf unref.Name = "typedef" Then
                strType = "typedef"
            Else
                strType = "class"
            End If

            If Not unref Is Nothing Then
                AddAttributeValue(reference, "name", GetName(unref))
                strPackage = GetPackage(unref)
                If strPackage <> "" Then AddAttributeValue(reference, "package", strPackage)
                AddAttributeValue(reference, "id", GetID(unref))
                AddAttributeValue(reference, "type", strType)
            End If
        Next
    End Sub

    Private Shared Function VerifyRedundancy(ByVal component As XmlComponent, ByVal strMessage1 As String, ByVal child As XmlNode, ByVal strMessage2 As String) As Boolean
        Dim bResult As Boolean = False
        Dim fen As New dlgRedundancy
        fen.Document = component
        fen.Node = child
        If CType(fen.Document, XmlRefRedundancyView).GetListReferences IsNot Nothing Then
            fen.Text = "Check redundancies..."
            fen.Message = "In " + strMessage1 + " found redundancy with " + strMessage2
            fen.ShowDialog()
            bResult = (CType(fen.Tag, Boolean))
        End If
        Return bResult
    End Function
#End If

    Public Shared Function RenameType(ByVal eLang As ELanguage, ByRef nodeDoxygen As XmlNode) As Boolean
        Dim bResult As Boolean = False
        Try

            Dim list As XmlNodeList
            Dim child As XmlNode
            Dim tData As New TSimpleDeclaration
            Dim tIndex As New TSimpleDeclaration
            Dim tContainer As New TSimpleDeclaration
            Dim strType As String = ""

            Debug.Print(nodeDoxygen.OuterXml)

            list = SelectNodes(nodeDoxygen, "type/ref")
            child = GetNode(nodeDoxygen, "type")

            If child.HasChildNodes = False Then
                ' UML Designer type, Nothing todo !
            ElseIf child.FirstChild.Name = "enumvalue" Then
                ' UML Designer type, Nothing todo !
            Else
                For Each node As XmlNode In child.ChildNodes
                    If node.NodeType = XmlNodeType.Element Then
                        strType += "refid:" + GetAttributeValue(node, "refid")
                    Else
                        strType += node.Value
                    End If
                Next

                If nodeDoxygen.Name = "element" Then

                    If AnalyzeSimpleType(eLang, strType, tData) Then

                        nodeDoxygen.RemoveChild(child)
                        child = CreateSimpletype(child, tData)
                        nodeDoxygen.InsertBefore(child, nodeDoxygen.FirstChild)
                        bResult = True
                    End If

                ElseIf AnalyzeContainerType(eLang, strType, tData, tIndex, tContainer, False) Then

                    nodeDoxygen.RemoveChild(child)
                    child = CreateNewContainer(nodeDoxygen, tData, tIndex, tContainer)
                    nodeDoxygen.InsertBefore(nodeDoxygen.OwnerDocument.ImportNode(child, True), nodeDoxygen.FirstChild)
                    bResult = True

                ElseIf AnalyzeSimpleType(eLang, strType, tData) Then

                    nodeDoxygen.RemoveChild(child)
                    child = CreateSimpletype(nodeDoxygen, tData)
                    nodeDoxygen.InsertBefore(nodeDoxygen.OwnerDocument.ImportNode(child, True), nodeDoxygen.FirstChild)
                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Shared Sub RenumberProject(ByRef node As XmlNode)
        Try
            Dim listID As XmlNodeList
            Dim child As XmlNode
            Dim szID As String

            Dim iClassIndex As Integer = 1
            listID = SelectNodes(node, "//*[@id]")

            For Each child In listID
                Select Case child.Name
                    Case "class", "typedef", "reference", "model"
                        RenumberElement(child, iClassIndex, "class")
                        iClassIndex += 1
                End Select
            Next child

            Dim iPackage As Integer = 1
            listID = SelectNodes(node, "//package")

            For Each child In listID
                SetID(child, "package" + CStr(iPackage))
                iPackage += 1
            Next child


            Dim szOldID As String
            listID = SelectNodes(node, "//enumvalue")

            For Each child In listID
                szOldID = GetID(child)
                szID = GetID(child.ParentNode.ParentNode)
                szID = szID.Substring(Len("class"))
                szID = "enum" + szID + "_" + GetName(child)
                AddAttributeValue(child, "id", szID)
                RenumberRefElement(node, "valref", szOldID, szID)
                RenumberRefElement(node, "sizeref", szOldID, szID)
            Next child

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub RenumberElement(ByRef node As XmlNode, ByVal index As Integer, ByVal Prefix As String)
        Try
            Dim szOldID As String = GetID(node)
            Dim szNewID As String = Prefix + CStr(index)
            SetID(node, szNewID)

            RenumberRefElement(node, "idref", szOldID, szNewID)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub RenumberRefElement(ByRef node As XmlNode, ByVal szIDREF As String, ByVal szOldID As String, ByVal szNewID As String)
        Try
            Dim listIDREF As XmlNodeList
            Dim child As XmlNode

            listIDREF = SelectNodes(node, "//@" + szIDREF + "[.='" + szOldID + "']")
            For Each child In listIDREF
                child.Value = szNewID
            Next child
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function AnalyzeSimpleType(ByVal eLang As ELanguage, ByVal strType As String, _
                                        ByRef tInfo As TSimpleDeclaration) As Boolean
        Try
            If Trim(strType) = "" Then

                ' Nothing to do

            ElseIf InStr(strType, "const") > 0 Then

                tInfo.bConst = True

                Dim reg As New RegularExpressions.Regex("const", RegularExpressions.RegexOptions.Compiled)

                AnalyzeSimpleType(eLang, Trim(reg.Split(strType)(0)), tInfo)
                AnalyzeSimpleType(eLang, Trim(reg.Split(strType)(1)), tInfo)

            Else
                Dim reg As New RegularExpressions.Regex("\*{1,2}", RegularExpressions.RegexOptions.Compiled)

                If reg.IsMatch(strType) Then

                    Dim value As RegularExpressions.Match = reg.Match(strType)
                    tInfo.ilevel = value.Groups(0).Value.Length
                    AnalyzeSimpleType(eLang, Trim(reg.Split(strType)(0)), tInfo)
                    AnalyzeSimpleType(eLang, Trim(reg.Split(strType)(1)), tInfo)

                ElseIf InStr(strType, Chr(38)) > 0 Then
                    tInfo.bReference = True
                    AnalyzeSimpleType(eLang, Trim(strType.Split(Chr(38))(0)), tInfo)

                ElseIf strType.StartsWith("refid:") Then
                    tInfo.strIdref = strType.Substring(Len("refid:"))
                    tInfo.strTypeName = Nothing
                Else
                    tInfo.strIdref = Nothing
                    tInfo.strTypeName = strType
                End If
            End If
            Return True

        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Private Shared Function AnalyzeContainerType(ByVal eLang As ELanguage, ByVal strType As String, _
                                        ByRef tData As TSimpleDeclaration, _
                                        ByRef tIndex As TSimpleDeclaration, _
                                        ByRef tContainer As TSimpleDeclaration, _
                                        Optional ByVal bAlertOk As Boolean = True) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim strSplit As String()
            Dim strContainer, strData, strIndex As String

            Select Case eLang

                Case ELanguage.Language_CplusPlus
                    Dim reg As New RegularExpressions.Regex("\<.*\>", RegularExpressions.RegexOptions.Compiled)

                    If reg.IsMatch(strType) = False _
                    Then
                        If bAlertOk Then
                            MsgBox("Sorry but, doesn't describe a C++ implemented template", MsgBoxStyle.Exclamation)
                        End If

                        bResult = False
                    Else
                        reg = New RegularExpressions.Regex("\<|\,|>$", RegularExpressions.RegexOptions.Compiled)

                        strSplit = reg.Split(strType)
                        strContainer = Trim(strSplit(0))

                        AnalyzeSimpleType(eLang, strContainer, tContainer)

                        If strSplit.Length = 4 Then
                            strIndex = Trim(strSplit(1))
                            strData = Trim(strSplit(2))
                            AnalyzeSimpleType(eLang, strIndex, tIndex)
                        Else
                            strData = Trim(strSplit(1))
                        End If

                        AnalyzeSimpleType(eLang, strData, tData)

                        bResult = True
                    End If

                Case Else
                    Throw New Exception("Sorry but, this function is not implemented for this language (" + eLang.ToString + ")")
            End Select

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Private Shared Function CreateSimpletype(ByVal treeNode As XmlNode, ByVal tData As TSimpleDeclaration, Optional ByVal bAddAttributes As Boolean = False) As XmlNode
        Dim source As New XmlDocument
        Try
            Dim strXML As String

            strXML = "<type "

            If tData.bReference Then
                strXML += " by='ref'"
            Else
                strXML += " by='val'"
            End If

            strXML += " level='" + CStr(tData.ilevel) + "'"

            If tData.bConst Then
                strXML += " modifier='const'"
            Else
                strXML += " modifier='var'"
            End If

            If tData.strTypeName <> "" Then
                strXML += " desc='" + tData.strTypeName + "'"
            Else
                strXML += " idref='" + tData.strIdref + "'"
            End If

            strXML += "/>"

            source.LoadXml(strXML)

        Catch ex As Exception
            Throw ex
        End Try
        Return source.FirstChild
    End Function

    Private Shared Function CreateNewContainer(ByVal treeNode As XmlNode, _
                                        ByVal tData As TSimpleDeclaration, _
                                        ByVal tIndex As TSimpleDeclaration, _
                                        ByVal tContainer As TSimpleDeclaration) As XmlNode
        Dim source As New XmlDocument
        Try
            Dim strXML As String

            strXML = "<type modifier='var' struct='container' by='val'"

            If tData.strIdref = "" Then
                strXML += " desc='" + tData.strTypeName + "'"
            Else
                strXML += " idref='" + tData.strIdref + "'"
            End If

            strXML += " level='" + CStr(tData.ilevel) + "'>" + vbCrLf

            strXML += "<list "

            If tContainer.strTypeName <> "" Then
                strXML += " desc='" + tContainer.strTypeName + "'"
            Else
                strXML += " idref='" + tContainer.strIdref + "'"
            End If

            If tIndex.strTypeName <> "" _
            Then
                strXML += " type='indexed'"
                strXML += " index-desc='" + tIndex.strTypeName + "'"
                strXML += " level='" + CStr(tIndex.ilevel) + "'"

            ElseIf tIndex.strIdref <> "" _
            Then
                strXML += " type='indexed'"
                strXML += " index-idref='" + tIndex.strIdref + "'"
                strXML += " level='" + CStr(tIndex.ilevel) + "'"
            Else
                strXML += " type='simple'"
            End If


            strXML += " iterator='no' />" + vbCrLf
            strXML += "</type>" + vbCrLf

            source.LoadXml(strXML)

        Catch ex As Exception
            Throw ex
        End Try
        Return source.FirstChild
    End Function

    Private Shared Function GetIdFromFullpathName(ByVal strFullpathName As String, ByVal treeNode As XmlNode) As String
        Dim strResult As String = Nothing
        Try
            Dim node As XmlNode
            Dim strName As String
            Dim iIndex As Integer

            iIndex = InStr(strFullpathName, "::")

            If iIndex = 0 Then
                node = GetNode(treeNode, "//class[@name='" + strFullpathName + "']")
            Else
                strName = Left(strFullpathName, iIndex - 1)
                strFullpathName = Mid(strFullpathName, iIndex + 2)

                node = GetNode(treeNode, "//package[@name='" + strName + "']")

                If node Is Nothing Then
                    node = GetNode(treeNode, "//class[@name='" + strName + "']/typedef[@name='" + strFullpathName + "']")

                Else
                    iIndex = InStr(strFullpathName, "::")

                    If iIndex = 0 Then
                        node = GetNode(node, "class[@name='" + strFullpathName + "']")
                    Else
                        strName = Left(strFullpathName, iIndex - 1)
                        strFullpathName = Mid(strFullpathName, iIndex + 2)

                        node = GetNode(node, "class[@name='" + strName + "']/typedef[@name='" + strFullpathName + "']")
                    End If
                End If

            End If
            If Not node Is Nothing Then
                strResult = GetID(node)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Shared Function ComputeRelativePath(ByVal strRootPath As String, _
                                               ByVal strCurrentPath As String) As String

        Dim strRelative As String = strCurrentPath
        Try
            If strCurrentPath.StartsWith(strRootPath) Then
                strRelative = strCurrentPath.Substring(strRootPath.Length)
            Else
                Dim strDisk As String = ""
                Dim i As Integer = InStr(strRootPath, System.IO.Path.VolumeSeparatorChar)
                If i > 0 Then
                    strDisk = Left(strRootPath, i - 1)
                Else
                    i = InStr(strRootPath, "\\")
                    If i > 0 Then
                        strDisk = Left(strRootPath, InStr(i + 2, strRootPath, System.IO.Path.VolumeSeparatorChar) - 1)
                    End If
                End If
                If Left(strCurrentPath, strDisk.Length) = strDisk _
                Then
                    strRelative = GetRelativePath(strRootPath.Substring(strDisk.Length + 2), strCurrentPath.Substring(strDisk.Length + 2))
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strRelative
    End Function

    Public Shared Function GetRelativePath(ByVal strRoot As String, ByVal strFinal As String) As String

        Dim reg As New RegularExpressions.Regex("\" + System.IO.Path.DirectorySeparatorChar, RegularExpressions.RegexOptions.Compiled)
        Dim splitRoot As String() = reg.Split(strRoot)
        Dim splitFinal As String() = reg.Split(strFinal)
        Dim result As String = ""
        Dim indexF As Integer = 0
        ' First seek path difference
        While indexF < splitRoot.Length And indexF < splitFinal.Length
            If splitFinal(indexF) <> splitRoot(indexF) Then
                Exit While
            End If
            indexF += 1
        End While
        ' Complete path with ".."
        Dim indexR As Integer = indexF
        While indexR < splitRoot.Length
            result += ".." + System.IO.Path.DirectorySeparatorChar
            indexR += 1
        End While
        ' Complete final path
        While indexF < splitFinal.Length
            result += splitFinal(indexF) + System.IO.Path.DirectorySeparatorChar
            indexF += 1
        End While
        Return result
    End Function

End Class
