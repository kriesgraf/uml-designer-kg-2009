Imports System
Imports System.Xml
Imports System.IO
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic

#If _APP_UML = "1" Then
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.XmlReferenceNodeCounter
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlNodeCounter
#End If


Public Enum ELanguage
    Language_CplusPlus = 0
    Language_Java = 1
    Language_Vbasic = 2
    Language_Tools = 3
End Enum

Public Class UmlNodesManager

    Public Const cstXmlFileHeader As String = "<?xml version='1.0' encoding='iso-8859-1'?>"

#Region "Public Methods"
#If _APP_UML = "1" Then

    Public Shared Function ImportNodes(ByVal form As Form, _
                                       ByVal component As XmlComposite, _
                                       ByVal strFilename As String, _
                                       ByVal NodeCounter As XmlReferenceNodeCounter, _
                                       ByVal bUpdateOnly As Boolean) As Boolean

        Dim observer As InterfProgression = CType(form, InterfProgression)
        Try
            Dim source As New XmlDocument
            Dim import As XmlComponent
            Dim child As XmlNode

            If bUpdateOnly Then
                MsgBox("Not yet implemented !", MsgBoxStyle.Exclamation)
                Return False
            End If

            observer.Minimum = 0
            observer.Maximum = 5
            observer.ProgressBarVisible = True

            If LoadDocument(form, source, strFilename) = EResult.Failed Then
                observer.ProgressBarVisible = False
                Return False
            End If

            observer.Increment(1)

            import = New XmlComponent(source.DocumentElement)
            import.Tag = component.Tag

            StartInsertion(source, NodeCounter)
            observer.Increment(1)

            Dim list As XmlNodeList

            ' Remove redundant imported references and classes
            list = import.SelectNodes("//import/export/* | //class")

            Dim name As String = My.Computer.FileSystem.GetName(strFilename)

            For Each child In list
                ' Cross control between component (Project node) and import (external file)
                Select Case dlgRedundancy.VerifyRedundancy(component, _
                                                           "Find redundancies in file '" + name + "' with project ...", _
                                                           child, name, False, True)
                    Case dlgRedundancy.EResult.RedundancyIgnoredAll
                        Exit For

                    Case Else
                        ' Ok, Ignore
                End Select
            Next child
            observer.Increment(1)

            For Each child In import.Node.ChildNodes
                Select Case child.Name
                    Case "relationship"
                        component.Document.DocumentElement.AppendChild(component.Document.ImportNode(child, True))

                    Case "generation", "comment"
                        ' Nothing todo 

                    Case Else
                        Dim xmlcpnt As XmlComponent = XmlNodeManager.GetInstance().CreateDocument(component.Document.ImportNode(child, True))
                        xmlcpnt.Tag = component.Tag
                        component.AppendComponent(xmlcpnt)
                End Select
            Next child
            observer.Increment(1)
            Return True

        Catch ex As Exception
            Throw ex
        Finally
            observer.ProgressBarVisible = False
        End Try
        Return False
    End Function

    Public Shared Sub ExportNodes(ByVal fen As Form, ByVal node As XmlNode, ByVal strFilename As String, _
                                  ByVal strFullpathPackage As String, ByVal eLang As ELanguage)

        Dim observer As InterfProgression = CType(fen, InterfProgression)
        Dim oldCursor As Cursor = fen.Cursor
        Try
            Dim source As New XmlDocument
            Dim strXML As String
            Dim strComment As String

            fen.Cursor = Cursors.WaitCursor
            observer.Minimum = 0
            observer.Maximum = 8
            observer.ProgressBarVisible = True

            strComment = GetNodeString(node, "comment/@brief")

            Dim strPath As String = GetProjectPath(strFilename)
            If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
            observer.Increment(1)

            strXML = cstXmlFileHeader
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
            observer.Increment(1)

            Dim child As XmlNode
            Dim listID As New SortedList

            child = node.CloneNode(True)
            observer.Increment(1)

            GetListDescendant(child, listID, "collaboration/@idref")
            observer.Increment(1)

            ExportRelationships(source, node, listID)
            observer.Increment(1)

            AddNodesAndLinkUnreferencedNodes(source, node, child, eLang)
            observer.Increment(1)

            child = GetNode(source, "/root/import/export")
            If child.HasChildNodes = False Then
                RemoveNode(child.ParentNode)
            End If
            observer.Increment(1)

            source.Save(strFilename)
            observer.Increment(1)

        Catch ex As Exception
            Throw ex
        Finally
            fen.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try
    End Sub

    Public Shared Sub ExportClassReferences(ByVal fen As Form, ByVal node As XmlNode, ByVal strFilename As String, _
                                            ByVal strFullpathPackage As String, ByVal eLang As ELanguage)
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        Try
            Dim source As New XmlDocument
            Dim strXML As String
            Dim index As Integer = 1
            Dim strPath As String = GetProjectPath(strFilename)

            fen.Cursor = Cursors.WaitCursor
            observer.ProgressBarVisible = True
            observer.Minimum = 0
            observer.Maximum = 6

            If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
            observer.Increment(1)

            strXML = cstXmlFileHeader
            strXML += GetDtdDeclaration("export")
            strXML += vbCrLf + "<export name='" + GetName(node) + "' source='" + strFullpathPackage + "'>" + vbCrLf
            strXML += vbCrLf + ExportElementClass(node, "")
            strXML += vbCrLf + "</export>"
            observer.Increment(1)

            source.LoadXml(strXML)
            observer.Increment(1)

            LinkUnreferencedNodes(source, node, eLang)
            observer.Increment(1)

            CheckExportedReferences(source)
            observer.Increment(1)

            source.Save(strFilename)
            observer.Increment(1)

        Catch ex As Exception
            Throw ex
        Finally
            fen.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try
    End Sub

    Public Shared Sub ExportRootReferences(ByVal fen As Form, ByVal node As XmlNode, ByVal strFilename As String, _
                                              ByVal eLang As ELanguage)
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)
        Dim strFullpathPackage As String = ""
        Try
            Dim source As New XmlDocument
            Dim strXML As String
            Dim index As Integer = 1
            Dim strPath As String = GetProjectPath(strFilename)

            fen.Cursor = Cursors.WaitCursor
            observer.ProgressBarVisible = True
            observer.Minimum = 0
            observer.Maximum = 6 + node.SelectNodes("descendant::package | descendant::class").Count

            If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
            observer.Increment(1)

            strXML = cstXmlFileHeader
            strXML += GetDtdDeclaration("export")

            strXML += vbCrLf + "<export name='" + GetName(node) + "' source='" + GetName(node) + "'>" + vbCrLf
            strXML += vbCrLf + ExportElementPackage(observer, node, "", eLang, True)    ' Increment observer
            strXML += vbCrLf + "</export>"

            source.LoadXml(strXML)
            observer.Increment(1)

            LinkUnreferencedNodes(source, node, eLang)
            observer.Increment(1)

            CheckExportedReferences(source)
            observer.Increment(1)

            source.Save(strFilename)
            observer.Increment(1)

        Catch ex As Exception
            Throw ex
        Finally
            fen.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try
    End Sub

    Public Shared Sub ExportPackageReferences(ByVal fen As Form, ByVal node As XmlNode, ByVal strFilename As String, _
                                              ByVal strFullpathPackage As String, ByVal eLang As ELanguage)
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        Try
            Dim source As New XmlDocument
            Dim strXML As String
            Dim index As Integer = 1
            Dim strPath As String = GetProjectPath(strFilename)

            fen.Cursor = Cursors.WaitCursor
            observer.ProgressBarVisible = True
            observer.Minimum = 0
            observer.Maximum = 6 + node.SelectNodes("descendant::package | descendant::class").Count

            If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
            observer.Increment(1)

            strXML = cstXmlFileHeader
            strXML += GetDtdDeclaration("export")

            strXML += vbCrLf + "<export name='" + GetName(node) + "' source='" + strFullpathPackage + "'>" + vbCrLf
            strXML += vbCrLf + ExportElementPackage(observer, node, "", eLang)    ' Increment observer
            strXML += vbCrLf + "</export>"

            source.LoadXml(strXML)
            observer.Increment(1)

            LinkUnreferencedNodes(source, node, eLang)
            observer.Increment(1)

            CheckExportedReferences(source)
            observer.Increment(1)

            source.Save(strFilename)
            observer.Increment(1)

        Catch ex As Exception
            Throw ex
        Finally
            fen.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try
    End Sub

    Public Shared Sub ExportTypedefReferences(ByVal fen As Form, ByVal node As XmlNode, ByVal strFilename As String, _
                                              ByVal strFullpathPackage As String)
        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        Try
            Dim source As New XmlDocument
            Dim strXML As String
            Dim strPath As String = GetProjectPath(strFilename)

            fen.Cursor = Cursors.WaitCursor
            observer.ProgressBarVisible = True
            observer.Minimum = 0
            observer.Maximum = 5

            If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
            observer.Increment(1)

            strXML = cstXmlFileHeader
            strXML += GetDtdDeclaration("export")

            strXML += vbCrLf + "<export name='" + GetName(node) + "' source='" + strFullpathPackage + "'>"
            strXML += vbCrLf + ExportElementTypedef(node.ParentNode, node, "")
            strXML += vbCrLf + "</export>"
            observer.Increment(1)

            'Debug.Print(strXML)
            source.LoadXml(strXML)
            observer.Increment(1)

            CheckExportedReferences(source)
            observer.Increment(1)

            source.Save(strFilename)
            observer.Increment(1)

        Catch ex As Exception
            Throw ex
        Finally
            fen.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try
    End Sub

    Public Shared Sub ReExport(ByVal fen As Form, ByVal node As XmlNode, ByVal strFilename As String, ByVal strFullpathPackage As String)

        Dim oldCursor As Cursor = fen.Cursor
        Dim observer As InterfProgression = CType(fen, InterfProgression)

        Try
            Dim source As New XmlDocument
            'Dim typedef As XmlNode
            Dim strXML As String
            Dim strPath As String = GetProjectPath(strFilename)

            fen.Cursor = Cursors.WaitCursor
            observer.ProgressBarVisible = True
            observer.Minimum = 0
            observer.Maximum = 5

            If CopyDocTypeDeclarationFile(strPath) = False Then Exit Sub
            observer.Increment(1)

            strXML = cstXmlFileHeader
            strXML += GetDtdDeclaration("export")
            strXML += vbCrLf + "<export name='" + GetName(node) + "' source='" + strFullpathPackage + "'>"

            For Each ref As XmlNode In node.ChildNodes
                strXML += vbCrLf + ref.OuterXml
            Next ref
            strXML += vbCrLf + "</export>"
            observer.Increment(1)

            'Debug.Print(strXML)
            source.LoadXml(strXML)
            observer.Increment(1)

            Dim list As XmlNodeList = source.SelectNodes("//collaboration")
            For Each child As XmlNode In list
                child.ParentNode.RemoveChild(child)
            Next child
            observer.Increment(1)

            source.Save(strFilename)
            observer.Increment(1)

        Catch ex As Exception
            Throw ex
        Finally
            fen.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try
    End Sub

    Public Shared Function ExtractReferences(ByVal fen As Form, ByVal node As XmlNode, _
                                             ByVal strFullpathPackage As String, ByVal eLang As ELanguage) As Boolean

        Dim observer As InterfProgression = CType(fen, InterfProgression)
        Dim oldCursor As Cursor = fen.Cursor
        Dim bResult As Boolean = False
        Try
            Dim parent As XmlNode = node.ParentNode
            Dim xmlParent As XmlComponent = XmlNodeManager.GetInstance().CreateDocument(parent)

            observer.ProgressBarVisible = True
            observer.Minimum = 0

            Dim strXML As String = "<export name='" + GetName(node) + "' source='" + strFullpathPackage + "'>"
            Select Case node.Name
                Case "class"
                    observer.Maximum = 2
                    strXML += vbCrLf + ExportElementClass(node, "")
                    observer.Increment(1)

                Case "package"
                    observer.Maximum = 1 + node.SelectNodes("descendant::package | descendant::class").Count
                    strXML += vbCrLf + ExportElementPackage(observer, node, "", eLang)
            End Select
            strXML += vbCrLf + "</export>"

            Dim importXML As XmlImportSpec = XmlNodeManager.GetInstance().CreateDocument("import", parent.OwnerDocument)
            If importXML.LoadXml(strXML) Then
                xmlParent.AppendComponent(importXML)
                parent.RemoveChild(node)
                bResult = True
            End If
            observer.Increment(1)

        Catch ex As Exception
            Throw ex
        Finally
            fen.Cursor = oldCursor
            observer.ProgressBarVisible = False
        End Try
        Return bResult
    End Function

    Public Shared Sub UpdatePrefixNames()
        Dim fen As New dlgPrefixNames
        fen.ShowDialog()
    End Sub

    Public Shared Sub UpdateSimpleTypes(ByVal eLang As ELanguage)
        Dim fen As New dlgSimpleTypes
        fen.Filename = GetSimpleTypesFilename(eLang)
        fen.CodeLanguage = eLang
        fen.ShowDialog()
    End Sub
#End If

    Public Shared Sub RenumberProject(ByRef node As XmlNode, Optional ByVal bChangeRelation As Boolean = False)
        Try
            Dim listID As XmlNodeList
            Dim child As XmlNode
            Dim strID As String

            Dim iClassIndex As Integer = 1
            listID = node.SelectNodes("//*[@id]")

            For Each child In listID
                Select Case child.Name
                    Case "class", "typedef", "reference", "interface", "model"
                        RenumberElement(child, iClassIndex, "class")
                        iClassIndex += 1
                End Select
            Next child

            Dim iPackage As Integer = 1
            listID = node.SelectNodes("//package")

            For Each child In listID
                SetID(child, "package" + CStr(iPackage))
                iPackage += 1
            Next child


            listID = node.SelectNodes("//param | // method | //property | //element")

            For Each child In listID
                If child.ParentNode Is Nothing Then
                    Throw New Exception("Node 'enumvalue' with name '" + GetName(child) + "' has not parent.")
                End If
                strID = GenerateNumericId(child.ParentNode, child.Name, "", "num-id", False)
                AddAttributeValue(child, "num-id", strID)
            Next

            listID = node.SelectNodes("//*[enumvalue or type/enumvalue]")

            For Each child In listID
                Dim strPrefix As String = ""
                Select Case child.Name
                    Case "reference"
                        strPrefix = GetID(child)

                    Case "property"
                        If child.ParentNode Is Nothing Then
                            Throw New Exception("Node 'property' with name '" + GetName(child) + "' has not parent.")
                        End If
                        strPrefix = GetAttributeValue(child, "num-id")
                        strPrefix = GetID(child.ParentNode) + "_" + strPrefix
                    Case Else
                        strPrefix = GetID(child)
                End Select

                Dim szOldID As String
                strPrefix = "enum" + AfterStr(strPrefix, "class")

                For Each enumalue As XmlNode In child.SelectNodes("descendant::enumvalue")
                    szOldID = GetID(enumalue)
                    strID = GenerateNumericId(child, "descendant::enumvalue", strPrefix, "id", False)
                    AddAttributeValue(enumalue, "id", strID)
                    RenumberRefElement(node, "valref", szOldID, strID)
                    RenumberRefElement(node, "sizeref", szOldID, strID)
                Next
            Next child


            If bChangeRelation Then
                listID = node.SelectNodes("//relationship")
                iClassIndex = 1

                For Each child In listID
                    RenumberElement(child, iClassIndex, "relation")
                    iClassIndex += 1
                Next child
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Private methods"

#If _APP_UML = "1" Then
    Private Shared Function CheckExportedReferences(ByVal source As XmlDocument) As Boolean
        Dim bResult As Boolean = False
        Try

            Dim list As XmlNodeList = source.SelectNodes("//export/*")

            Dim component As XmlComponent = XmlNodeManager.GetInstance().CreateDocument(source.DocumentElement)

            For Each child In list
                ' Cross control between component (Project node) and import (external file)
                Select Case dlgRedundancy.VerifyRedundancy(component, "Find redundancies in exported file...", child, "", False, True)
                    Case dlgRedundancy.EResult.RedundancyIgnoredAll
                        Exit For

                    Case dlgRedundancy.EResult.RedundancyChanged
                        bResult = True

                    Case Else
                        ' Ignore
                End Select
            Next child
        Catch ex As Exception

        End Try
        Return bResult
    End Function

    Private Shared Function ExportElementPackage(ByVal observer As InterfProgression, ByVal node As XmlNode, ByVal strCurrentPackage As String, _
                                              ByVal eLang As ELanguage, Optional ByVal bRoot As Boolean = False) As String
        Dim strXML As String = ""
        Try
            observer.Increment(1)

            If bRoot = False Then
                If strCurrentPackage <> "" Then
                    strCurrentPackage += GetSeparator(eLang) + GetName(node)
                Else
                    strCurrentPackage = GetName(node)
                End If
            End If

                For Each current As XmlNode In node.SelectNodes("class | package")
                If current.Name = "class" Then
                    strXML += vbCrLf + ExportElementClass(current, strCurrentPackage)
                    observer.Increment(1)
                Else
                    strXML += vbCrLf + ExportElementPackage(observer, current, strCurrentPackage, eLang)
                End If
                Next current
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

            strXML += " type='typedef' container='0' id='" + GetID(typedef) + "'"
            If GetNode(typedef, "descendant::enumvalue") Is Nothing Then
                strXML += "/>"
            Else
                strXML += ">"
                For Each child As XmlNode In SelectNodes(typedef, "descendant::enumvalue")
                    strXML += vbCrLf + child.OuterXml
                Next
                strXML += vbCrLf + "</reference>"
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strXML
    End Function

    Private Shared Sub ExportElementClassMember(ByRef strXML As String, ByVal member As XmlOverrideMemberView)
        If member.InterfaceMember Then
            ' We clone node to remove unused attributes or nodes
            member.Node = member.Node.CloneNode(True)
            Dim attrib As XmlAttribute = member.Node.Attributes("overrides")
            If attrib IsNot Nothing Then
                member.Node.Attributes.Remove(attrib)
            End If
            member.InterfaceMember = True
            strXML += vbCrLf + member.OuterXml
        End If
    End Sub

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

    Private Shared Sub AddNodesAndLinkUnreferencedNodes(ByVal source As XmlDocument, ByVal treeNode As XmlNode, _
                                                        ByVal exportNodes As XmlNode, ByVal eLang As ELanguage)
        Dim listID As New SortedList
        Dim child As XmlNode

        child = source.ImportNode(exportNodes, True)
        Dim before As XmlNode = GetNode(source, "//relationship")

        If before IsNot Nothing Then
            source.DocumentElement.InsertBefore(child, before)
        Else
            source.DocumentElement.AppendChild(child)
        End If

        LinkUnreferencedNodes(source, treeNode, eLang, "no")
    End Sub

    Private Shared Sub LinkUnreferencedNodes(ByVal source As XmlDocument, ByVal treeNode As XmlNode, _
                                             ByVal eLang As ELanguage, Optional ByVal strExternal As String = "yes")
        Dim listID As New SortedList
        Dim child As XmlNode
        Dim unref As XmlNode
        Dim export As XmlNode
        Dim reference As XmlNode
        Dim strType As String
        Dim strID As String
        Dim strPackage As String

        ' We add unreferenced IDs to first import
        export = GetNode(source, "//export")

        ' We replace the ID of the enum values that describe the size of a table 
        ' by their text descriptor (name), it will generate the wrong code, but the references are 
        ' lost but the project remains readable
        For Each child In SelectNodes(source, "//*[@sizeref and not(@sizeref=//*/@id)]")
            strID = GetIDREF(child, "sizeref")
            unref = SelectNodeStringId(treeNode, strID)
            RemoveAttribute(child, "sizeref")
            AddAttributeValue(child, "size", GetName(unref))
        Next child

        For Each child In SelectNodes(source, "//*[@valref and not(@valref=//*/@id)]")
            strID = GetIDREF(child, "valref")
            unref = SelectNodeStringId(treeNode, strID)
            RemoveAttribute(child, "valref")
            AddAttributeValue(child, "value", GetName(unref))
        Next child

        ' ID are placed in a collection to remove redundancy
        For Each child In SelectNodes(source, "//*[@idref and not(@idref=//*/@id)]")
            strID = GetIDREF(child)
            If listID.Contains(strID) = False Then
                listID.Add(strID, strID)
            End If
        Next child

        For Each dico As DictionaryEntry In listID

            ' We recover the real node of the original tree 
            ' To rename the reference in the export file
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
                Select Case unref.Name
                    Case "typedef", "class"
                        strPackage = GetPackage(unref)
                    Case "reference", "interface"
                        strPackage = GetPackage(unref)
                        Dim tempo As String = GetNodeString(unref, "ancestor::import/@param")
                        If tempo.Length > 0 Then
                            If strPackage <> "" Then ' check nothing and empty in same time
                                strPackage = tempo + GetSeparator(eLang) + strPackage
                            Else
                                strPackage = tempo
                            End If
                        End If

                    Case Else
                        strPackage = GetPackage(unref)
                End Select
                If strPackage <> "" Then AddAttributeValue(reference, "package", strPackage)
                AddAttributeValue(reference, "external", strExternal)
                AddAttributeValue(reference, "id", GetID(unref))
                AddAttributeValue(reference, "type", strType)
                If strType = "typedef" Then AddAttributeValue(reference, "class", GetName(unref.ParentNode))
            End If
        Next
    End Sub

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

    Public Shared Function ExportElementClass(ByVal node As XmlNode, ByVal strCurrentPackage As String) As String

        Dim strXML As String = ""

        Try
            Dim strImplementation As String = GetAttributeValue(node, "implementation")
            Dim strName As String = GetName(node)

            Debug.Print("Name:=" + strName + " - " + strImplementation)

            Select Case strImplementation
                Case "simple", "final", "exception", "container"
                    strXML += "<reference name='" + GetName(node) + "' type='class'"

                    If strCurrentPackage <> "" Then strXML += " package='" + strCurrentPackage + "'"

                    Dim iContainer = node.SelectNodes("model").Count
                    strXML += " container='" + CStr(iContainer) + "' id='" + GetID(node) + "'/>"

                Case Else
                    strXML += "<interface name='" + GetName(node) + "'"
                    If strImplementation <> "abstract" Then
                        strXML += " root='yes'"
                    Else
                        strXML += " root='no'"
                    End If
                    If strCurrentPackage <> "" Then strXML += " package='" + strCurrentPackage + "'"
                    strXML += " id='" + GetID(node) + "'>"

                    Dim myList As New ArrayList

                    Dim iteration As Integer = 0
                    Dim eImplementation As EImplementation = ConvertDtdToEnumImpl(strImplementation)
                    SelectInheritedProperties(iteration, eImplementation, node, myList)

                    iteration = 0
                    SelectInheritedMethods(iteration, eImplementation, node, myList)

                    For Each member As XmlOverrideMemberView In myList
                        ExportElementClassMember(strXML, member)
                    Next

                    strXML += vbCrLf + "</interface>"
            End Select

            Dim list As XmlNodeList = SelectNodes(node, "typedef[variable/@range='public']")

            For Each typedef As XmlNode In list
                strXML += vbCrLf + ExportElementTypedef(node, typedef, strCurrentPackage)
            Next typedef

        Catch ex As Exception
            Throw ex
        End Try
        Return strXML
    End Function

    Public Shared Function ComputeRelativePath(ByVal strRootPath As String, _
                                               ByVal strCurrentPath As String) As String

        Dim strRelative As String = strCurrentPath
        Try
            If strCurrentPath.StartsWith(strRootPath) Then
                strRelative = strCurrentPath.Substring(strRootPath.Length)
                If strRelative.StartsWith(Path.DirectorySeparatorChar.ToString) Then
                    strRelative = strRelative.Substring(1)
                End If
            Else
                Dim strDisk As String = ""
                Dim i As Integer = InStr(strRootPath, Path.VolumeSeparatorChar)
                If i > 0 Then
                    strDisk = Left(strRootPath, i - 1)
                Else
                    i = InStr(strRootPath, "\\")    ' TODO: convet to special constant
                    If i > 0 Then
                        strDisk = Left(strRootPath, InStr(i + 2, strRootPath, Path.DirectorySeparatorChar) - 1)
                    End If
                End If
                If Left(strCurrentPath, strDisk.Length) = strDisk _
                Then
                    strRelative = GetRelativePath(strRootPath.Substring(strDisk.Length), strCurrentPath.Substring(strDisk.Length))
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strRelative
    End Function

    Public Shared Function GetRelativePath(ByVal strRoot As String, ByVal strFinal As String) As String

        Dim reg As New RegularExpressions.Regex("\" + Path.DirectorySeparatorChar, RegularExpressions.RegexOptions.Compiled)
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
            result += ".." + Path.DirectorySeparatorChar
            indexR += 1
        End While
        ' Complete final path
        While indexF < splitFinal.Length And splitFinal(indexF).Length > 0
            result += splitFinal(indexF) + Path.DirectorySeparatorChar
            indexF += 1
        End While
        Return result
    End Function

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
                            MsgBox("Sorry but, doesn't describe a C++ implemented template", MsgBoxStyle.Exclamation, "Convert Doxygen file index")
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
#End If

    Private Shared Sub RenumberElement(ByRef node As XmlNode, ByVal index As Integer, ByVal Prefix As String)
        Try
            Dim szOldID As String = GetID(node)
            Dim szNewID As String = Prefix + CStr(index)
            SetID(node, szNewID)

            RenumberRefElement(node, "overrides", szOldID, szNewID)
            RenumberRefElement(node, "idref", szOldID, szNewID)
            RenumberRefElement(node, "index-idref", szOldID, szNewID)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Sub RenumberRefElement(ByRef node As XmlNode, ByVal szIDREF As String, ByVal szOldID As String, ByVal szNewID As String)
        Try
            Dim listIDREF As XmlNodeList
            Dim child As XmlNode

            listIDREF = node.SelectNodes("//@" + szIDREF + "[.='" + szOldID + "']")
            For Each child In listIDREF
                child.Value = szNewID
            Next child
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
End Class
