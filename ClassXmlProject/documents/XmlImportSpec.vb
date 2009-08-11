Imports System
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.XmlNodeListView
Imports System.ComponentModel
Imports System.Collections
Imports System.Windows.Forms
Imports System.Xml
Imports Microsoft.VisualBasic

Public Class XmlImportSpec
    Inherits XmlComposite
    Implements InterfNodeCounter

#Region "Class declarations"
    ''' <summary>
    ''' Mode of importation
    ''' </summary>
    Public Enum EImportMode
        ''' <summary>
        ''' Remove old nodes and insert new references if old nodes are not linked, oherwise aborts
        ''' </summary>
        ReplaceReferences
        ''' <summary>
        ''' Check old nodes and remove some not found in new import, 
        ''' add new references, don't remove/rename linked old nodes, whole done without confirmation
        ''' </summary>
        MergeReferences
        ''' <summary>
        ''' Merge nodes with confirmation, user could abort process or check/unckeck new nodes to add, 
        ''' old nodes to remove or rename.
        ''' </summary>
        CheckReferences
    End Enum

    Private m_xmlInline As XmlCodeInline = Nothing
    Private m_xmlExport As XmlExportSpec
    Protected m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

#End Region

#Region "Properties"

    <CategoryAttribute("XmlComponent"), _
    Browsable(False), _
    DescriptionAttribute("Node counter")> _
    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
            If m_xmlExport IsNot Nothing Then
                Me.ChildExportNode.NodeCounter = value
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Comment")> _
    Public ReadOnly Property Comment() As String
        Get
            If TestNode("body") Then
                Return "Module interface declaration"
            Else
                Dim strImport As String = Me.Parameter
                If strImport = "" Then
                    strImport = GetReferences()
                End If
                Return "Imports " + strImport
            End If
        End Get
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Inline Body")> _
    Public Property InlineBody() As XmlCodeInline
        Get
            If m_xmlInline IsNot Nothing Then
                m_xmlInline.Tag = Me.Tag
            End If
            Return m_xmlInline
        End Get
        Set(ByVal value As XmlCodeInline)
            m_xmlInline = value
            If m_xmlInline IsNot Nothing Then
                m_xmlInline.Tag = Me.Tag
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("C++ class declaration")> _
    Public Property Parameter() As String
        Get
            Return GetAttribute("param")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("param")
            Else
                AddAttribute("param", value)
            End If
        End Set
    End Property

    <TypeConverter(GetType(UmlClassVisibility)), _
    CategoryAttribute("UML design"), _
   DescriptionAttribute("Package visibility")> _
    Public Property Visibility() As String
        Get
            Return GetAttribute("visibility")
        End Get
        Set(ByVal value As String)
            SetAttribute("visibility", value)
        End Set
    End Property

    Public ReadOnly Property ChildExportNode() As XmlExportSpec
        Get
            If m_xmlInline IsNot Nothing Then
                Throw New Exception("Can't get 'export' node when 'inline' is active")
            End If

            If m_xmlExport Is Nothing _
            Then
                If TestNode("export") Then
                    m_xmlExport = TryCast(CreateDocument(GetNode("export")), XmlExportSpec)
                    m_xmlExport.NodeCounter = m_xmlReferenceNodeCounter
                Else
                    m_xmlExport = CreateDocument("export", Me.Document)
                    m_xmlExport.NodeCounter = m_xmlReferenceNodeCounter
                    AppendComponent(m_xmlExport)
                End If
            End If
            If m_xmlExport IsNot Nothing Then m_xmlExport.Tag = Me.Tag

            Return m_xmlExport
        End Get
    End Property
#End Region

#Region "Public methods"

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()

            Name = "New_import"
            Visibility = "package"
            Select Case CType(Me.Tag, ELanguage)
                Case ELanguage.Language_CplusPlus
                    Parameter = "Import.h"
                Case ELanguage.Language_Vbasic
                    Parameter = "Namespace1"
                Case ELanguage.Language_Java
                    Parameter = "Package1"
            End Select

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Protected Friend Overrides Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, Optional ByVal eRename As XmlComponent.ENameReplacement = XmlComponent.ENameReplacement.NewName, Optional ByVal bSetIdrefChildren As Boolean = False)
        Try
            If xmlRefNodeCounter Is Nothing Then
                Throw New Exception("Argument 'xmlRefNodeCounter' is null")
            End If
            Select Case eRename
                Case ENameReplacement.NewName
                    ' We call a second time this method to rename import according to language
                    SetDefaultValues(False)
                Case Else
            End Select
            If Me.ChildExportNode IsNot Nothing Then
                Me.ChildExportNode.SetIdReference(xmlRefNodeCounter, eRename, bSetIdrefChildren)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overrides Function AppendComponent(ByVal document As XmlComponent, Optional ByVal observer As Object = Nothing) As XmlNode
        If CanAddComponent(document) = False _
        Then
            Return Nothing

        ElseIf document.NodeName = "export" Then
            Return MyBase.AppendComponent(document, observer)
        Else
            Return ChildExportNode.AppendComponent(document, observer)
        End If
    End Function

    Protected Friend Overrides Function AppendNode(ByVal nodeXml As System.Xml.XmlNode, Optional ByVal observer As Object = Nothing) As System.Xml.XmlNode
        If nodeXml.Name = "export" Then
            Return MyBase.AppendNode(nodeXml, observer)
        End If
        Return ChildExportNode.AppendNode(nodeXml, observer)
    End Function

    Public Overrides Function InsertComponent(ByVal document As XmlComponent, ByVal before As XmlComponent) As XmlNode
        Return ChildExportNode.InsertComponent(document, before)
    End Function

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode)
        Try
            ChangeReferences(bLoadChildren)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function CanDropItem(ByVal child As XmlComponent, Optional ByVal bCheckOnly As Boolean = True) As Boolean
        Select Case child.NodeName
            Case "class"
                Return True

            Case Else
                Dim tempo As String = child.GetAttribute("param")
                Dim tempo2 As String = Me.Parameter
                If tempo Is Nothing Then
                    If tempo2 Is Nothing Then
                        Return True
                    End If
                Else
                    If tempo2 IsNot Nothing Then
                        Return (tempo = tempo2)
                    End If
                End If
                Return False
        End Select
        Return False
    End Function

    Public Overrides Function CanRemove(ByVal removeNode As XmlComponent) As Boolean
        Try
            Select Case removeNode.NodeName
                Case "reference", "interface"
                    If SelectNodes(GetQueryListDependencies(removeNode)).Count > 0 _
                    Then
                        If MsgBox("Some elements reference this, you can dereference them and then this will be deleted." + _
                                  vbCrLf + "Do you want to proceed", _
                                    cstMsgYesNoQuestion, _
                                    removeNode.Name) = MsgBoxResult.Yes _
                        Then
                            Dim bIsEmpty As Boolean = False

                            If dlgDependencies.ShowDependencies(m_xmlReferenceNodeCounter, removeNode, bIsEmpty, "Remove references to " + removeNode.Name) Then
                                Me.Updated = True
                            End If

                            Return bIsEmpty
                        End If
                    Else
                        Return True
                    End If

                Case Else
                    Return MyBase.CanRemove(removeNode)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Public Overrides Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim strName As String = removeNode.Name
            If MsgBox("Confirm to delete:" + vbCrLf + "Name: " + strName, _
                       cstMsgYesNoQuestion, "'Delete' command") = MsgBoxResult.Yes _
            Then
                Select Case removeNode.NodeName
                    Case "reference", "interface"
                        ' Don't use return value that returns "true" only when export is empty
                        RemoveReference(removeNode)
                        Return True

                    Case "export"
                        Return RemoveImport()
                End Select
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function MoveUpComponent(ByVal child As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim import As XmlImportSpec = CreateDocument("import")
            import.Tag = Me.Tag
            import.SetDefaultValues(False)
            import.Name = "MovedUp" + import.GetHashCode().ToString

            Dim parent As XmlComposite = CreateDocument(Me.Node.ParentNode)

            If parent.AppendComponent(import) IsNot Nothing Then
                If import.AppendComponent(child) IsNot Nothing Then
                    MsgBox("Moved into a new 'import' node named '" + import.Name + "'.", MsgBoxStyle.Information, "'Move up' command")
                    bResult = True
                Else
                    MsgBox("Failed to moved up node '" + child.Name + "' !", MsgBoxStyle.Information, "'Move up' command")
                End If

            Else
                MsgBox("Failed to moved up up node '" + child.Name + "' !", MsgBoxStyle.Information, "'Move up' command")
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Sub ConvertComponent(ByVal component As XmlComponent)
        Try
            If component Is Nothing Then Exit Sub

            Dim ref As XmlReferenceSpec
            Dim interf As XmlInterfaceSpec

            Select Case component.NodeName
                Case "reference"
                    ref = CreateDocument(component.Node)
                    interf = CreateDocument("interface")
                    interf.Name = ref.Name
                    interf.Id = ref.Id
                    interf.Package = ref.Package
                    interf.Tag = ref.Tag
                    ref.ReplaceMe(interf)
                    ref = Nothing

                Case "interface"
                    interf = CreateDocument(component.Node)
                    ref = CreateDocument("reference")
                    ref.Name = interf.Name
                    ref.Id = interf.Id
                    ref.Package = interf.Package
                    ref.Tag = interf.Tag
                    interf.ReplaceMe(ref)
                    interf = Nothing
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Protected methods"

    Protected Function GetReferences() As String
        Dim strResult As String = ""
        Try

            If Me.ChildExportNode Is Nothing Then
                Return ""
            End If

            Me.ChildExportNode.LoadChildrenList()

            If Me.ChildExportNode.ChildrenList.Count = 0 Then Return ""

            Dim iterator As IEnumerator = Me.ChildExportNode.ChildrenList.GetEnumerator()
            iterator.Reset()

            While iterator.MoveNext
                Dim xmlcpnt As XmlComponent = CType(iterator.Current, XmlComponent)
                If strResult <> "" Then strResult = strResult + ", "
                Select Case xmlcpnt.NodeName
                    Case "reference"
                        strResult = strResult + CType(xmlcpnt, XmlReferenceSpec).FullpathClassName
                    Case "interface"
                        strResult = strResult + CType(xmlcpnt, XmlInterfaceSpec).FullpathClassName
                End Select
            End While
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        Try
            MyBase.ChangeReferences(bLoadChildren)

            If TestNode("body") Then
                m_xmlInline = TryCast(CreateDocument(GetNode("body")), XmlCodeInline)
                If m_xmlInline IsNot Nothing Then m_xmlInline.Tag = Me.Tag

            ElseIf TestNode("export") Then
                m_xmlExport = TryCast(CreateDocument(GetNode("export")), XmlExportSpec)
                If m_xmlExport IsNot Nothing Then
                    m_xmlExport.Tag = Me.Tag
                    m_xmlExport.NodeCounter = m_xmlReferenceNodeCounter
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Protected Friends methods"

    Protected Friend Overrides Function RemoveRedundant(ByVal component As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            If component IsNot Nothing Then
                If dlgRedundancy.VerifyRedundancy(Me, "Check redundancies...", component.Node) _
                    = dlgRedundancy.EResult.RedundancyChanged _
                Then
                    Me.Updated = True
                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function LoadDocument(ByVal form As Form, ByVal Filename As System.IO.FileInfo, _
                                           Optional ByVal eMode As EImportMode = EImportMode.ReplaceReferences) As Boolean
        Dim bResult As Boolean = False
        Dim bDoxygenTagFile As Boolean = (Filename.Extension.ToLower = ".tag")
        Try
            Select Case eMode
                Case EImportMode.ReplaceReferences
                    bResult = ReplaceFileReferences(form, Filename.FullName, bDoxygenTagFile)
                Case EImportMode.MergeReferences
                    bResult = MergeFileReferences(form, Filename.FullName, False, bDoxygenTagFile)
                Case Else
                    bResult = MergeFileReferences(form, Filename.FullName, True, bDoxygenTagFile)
            End Select

            If bResult Then
                Me.Parameter = ChildExportNode.Source
                Me.Name = ChildExportNode.Name
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function LoadXml(ByVal strXML As String) As Boolean
        Dim bResult As Boolean = False
        Try
            bResult = ReplaceStringReferences(strXML)

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function CheckReferences() As Boolean
        Dim bResult As Boolean = False
        Try
            If Me.ChildExportNode.SelectNodes().Count = 0 Then
                Me.ChildExportNode.RemoveMe()
                m_xmlExport = Nothing
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

#End Region

#Region "Private methods"

    Private Function RemoveReference(ByVal component As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            component.RemoveMe()
            bResult = CheckReferences()

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Private Function ReplaceStringReferences(ByVal strXML As String) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim bImportOk As Boolean = True

            If m_xmlExport Is Nothing Then
                m_xmlExport = New XmlExportSpec()
                Me.ChildExportNode.Document = Me.Document
            Else
                bImportOk = Me.ChildExportNode.RemoveReferences()
            End If

            If bImportOk _
            Then
                Me.ChildExportNode.NodeCounter = m_xmlReferenceNodeCounter

                If Me.ChildExportNode.LoadStringImport(strXML) Then
                    Me.AppendComponent(Me.ChildExportNode)
                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Private Function MergeFileReferences(ByVal form As Form, ByVal strFilename As String, _
                                         ByVal bAskReplace As Boolean, _
                                         Optional ByVal bDoxygenTagFile As Boolean = False) As Boolean

        Dim observer As InterfProgression = TryCast(form, InterfProgression)
        Dim bResult As Boolean = False
        Try
            Dim bImportOk As Boolean = True

            If m_xmlExport Is Nothing Then
                MsgBox("Import empty, please use command 'Replace' !", MsgBoxStyle.Exclamation, "'Merge' command")
            Else
                Dim export As XmlExportSpec = New XmlExportSpec()   ' We don't use base method MyBase.CreateDocument, because don't want to create an XmlNode instance
                export.Document = Me.Document
                export.NodeCounter = m_xmlReferenceNodeCounter
                export.Tag = Me.Tag

                Dim myList As New ArrayList

                If export.LoadImport(form, Me, strFilename, bDoxygenTagFile) Then
                    export.LoadChildrenList()
                    For Each xmlcpnt As XmlComponent In export.ChildrenList
                        xmlcpnt.Tag = export.Tag
                        Dim added As XmlNodeListView = AddComponentList(xmlcpnt, myList)
                        Dim found As XmlNode = Me.ChildExportNode.FindNode(xmlcpnt)
                        Dim strID As String = Nothing


                        If found IsNot Nothing Then
                            Debug.Print("Name:=" + added.Name + "-Found:=" + found.OuterXml)
                            strID = GetID(found)
                            added.Info = True
                        Else
                            Debug.Print("Name:=" + added.Name + "-Content:=" + added.OuterXml)
                            strID = m_xmlReferenceNodeCounter.GetNewClassId()
                            added.CheckedView = True
                        End If

                        If ChangeID(added.Node, export.Node, strID) Then
                            added.CheckedView = True
                            added.CheckLocked = True
                        End If
                    Next

                    If myList.Count > 0 Then
                        SortNodeList(myList)

                        observer.Minimum = 0
                        observer.Maximum = myList.Count + 1

                        If bAskReplace = False _
                        Then
                            observer.ProgressBarVisible = True
                            For Each element As XmlNodeListView In myList
                                Debug.Print("Name:=" + element.Name + "-" + element.Id)
                                Me.ChildExportNode.ReplaceReference(element)
                                observer.Increment(1)
                            Next
                        ElseIf ShowNodeList(myList, "Select references/interfaces to replace", "Locked, because reference is used by an interface property or method") _
                        Then
                            observer.ProgressBarVisible = True
                            bResult = True
                            For Each element As XmlNodeListView In myList
                                Debug.Print("Name:=" + element.Name + "-" + element.Id + "-" + element.CheckedView.ToString)
                                If element.CheckedView Then
                                    Me.ChildExportNode.ReplaceReference(element)
                                End If
                                observer.Increment(1)
                            Next
                        End If

                        Me.ChildExportNode.SearchRedundancies(Me, My.Computer.FileSystem.GetName(strFilename))
                        observer.Increment(1)
                        bResult = True
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            observer.ProgressBarVisible = False
        End Try
        Return bResult
    End Function

    Private Function ReplaceFileReferences(ByVal form As Form, ByVal strFilename As String, Optional ByVal bDoxygenTagFile As Boolean = False) As Boolean

        Dim observer As InterfProgression = TryCast(form, InterfProgression)
        Dim bResult As Boolean = False
        Try
            Dim bImportOk As Boolean = True

            observer.Minimum = 0
            observer.Maximum = 4
            observer.ProgressBarVisible = True

            If m_xmlExport Is Nothing Then
                m_xmlExport = New XmlExportSpec()   ' Don't create at this step an Xml node!
                Me.ChildExportNode.Document = Me.Document
            Else
                bImportOk = Me.ChildExportNode.RemoveReferences()
            End If

            observer.Increment(1)

            If bImportOk Then

                Me.ChildExportNode.NodeCounter = m_xmlReferenceNodeCounter

                If Me.ChildExportNode.LoadImport(form, Me, strFilename, bDoxygenTagFile) Then    ' Clone node at this step
                    observer.Increment(1)

                    Me.AppendComponent(Me.ChildExportNode, observer)                             ' And append now
                    observer.Increment(1)

                    Me.ChildExportNode.SearchRedundancies(Me, My.Computer.FileSystem.GetName(strFilename))
                    observer.Increment(1)

                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            observer.ProgressBarVisible = False
        End Try
        Return bResult
    End Function

    Private Function RemoveImport() As Boolean
        Dim bResult As Boolean = False
        Try
            If Me.ChildExportNode.RemoveReferences() Then
                m_xmlExport = Nothing
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

#End Region
End Class

