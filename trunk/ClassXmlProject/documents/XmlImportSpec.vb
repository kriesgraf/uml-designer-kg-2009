Imports System
Imports ClassXmlProject.UmlCodeGenerator
Imports System.ComponentModel
Imports System.Collections
Imports System.Xml
Imports Microsoft.VisualBasic

Public Class XmlImportSpec
    Inherits XmlComponent
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

    Private m_xmlInline As XmlCodeInline
    Protected m_xmlExport As XmlExportSpec
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
                m_xmlExport.NodeCounter = value
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
            Return m_xmlInline
        End Get
        Set(ByVal value As XmlCodeInline)
            m_xmlInline = value
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

#End Region

#Region "Protected methods"

    Protected Function GetReferences() As String
        Dim strResult As String = ""
        Try

            If m_xmlExport Is Nothing Then
                Return ""
            End If

            m_xmlExport.LoadChildrenList()

            If m_xmlExport.ChildrenList.Count = 0 Then Return ""

            Dim iterator As IEnumerator = m_xmlExport.ChildrenList.GetEnumerator()
            iterator.Reset()

            While iterator.MoveNext
                Dim xmlcpnt As XmlReferenceSpec = CType(iterator.Current, XmlReferenceSpec)
                If strResult <> "" Then strResult = strResult + ", "
                xmlcpnt.Tag = Me.Tag
                strResult = strResult + xmlcpnt.FullpathClassName
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
            End If
            If TestNode("export") Then
                m_xmlExport = TryCast(CreateDocument(GetNode("export")), XmlExportSpec)
                m_xmlExport.NodeCounter = m_xmlReferenceNodeCounter
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Public methods"

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()

            Name = "New_import"
            Visibility = "package"
            Parameter = "import.h"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Function AppendComponent(ByVal document As XmlComponent) As XmlNode
        If CanAddComponent(document) = False _
        Then
            Return Nothing

        ElseIf document.NodeName = "export" Then
            Me.Parameter = CType(document, XmlExportSpec).Source
            Me.Name = CType(document, XmlExportSpec).Name
            Return MyBase.AppendComponent(document)
        Else
            Return GetExportNode().AppendComponent(document)
        End If
    End Function

    Public Overrides Function InsertComponent(ByVal document As XmlComponent, ByVal before As XmlComponent) As XmlNode
        Return GetExportNode().InsertComponent(document, before)
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
        Return (Me.Parameter = child.GetAttribute("param"))
    End Function

    Public Function PasteReference() As Boolean
        Try
            ' Get back from the specific clipboard shared by all projects
            Dim bCopy As Boolean
            Dim component As XmlComponent = XmlComponent.Clipboard.GetData(bCopy)
            Dim parent As XmlComposite = GetExportNode()

            If parent.CanPasteItem(component) _
            Then
                ' Method XmlNode.AppendChild make a cut/paste if node is not cloned.
                If bCopy = True Then
                    component = GetExportNode().DuplicateComponent(component)
                    component.SetIdReference(m_xmlReferenceNodeCounter, True)
                End If


                If component IsNot Nothing Then
                    parent.AppendComponent(component)
                    Return True
                End If
            Else
                MsgBox("Sorry can't paste node '" + component.NodeName + "' on '" + parent.NodeName + "' !", MsgBoxStyle.Exclamation)
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

#End Region

#Region "Protected Friends methods"

    Protected Friend Function GetExportNode() As XmlExportSpec
        If m_xmlInline IsNot Nothing Then
            Throw New Exception("Can't get 'export' node when 'inline' is active")
        End If

        If m_xmlExport Is Nothing _
        Then
            If TestNode("export") Then
                m_xmlExport = TryCast(CreateDocument(GetNode("export")), XmlExportSpec)
                m_xmlExport.NodeCounter = m_xmlReferenceNodeCounter
            Else
                m_xmlExport = MyBase.CreateDocument("export", MyBase.Document)
                m_xmlExport.NodeCounter = m_xmlReferenceNodeCounter
                MyBase.AppendComponent(m_xmlExport)
            End If
        End If
        Return m_xmlExport
    End Function

    Protected Friend Function LoadDocument(ByVal Filename As System.IO.FileInfo, _
                                           Optional ByVal eMode As EImportMode = EImportMode.ReplaceReferences) As Boolean
        Dim bResult As Boolean = False
        Dim bDoxygenTagFile As Boolean = (Filename.Extension.ToLower = ".tag")
        Try
            Select Case eMode
                Case EImportMode.ReplaceReferences
                    bResult = ReplaceFileReferences(Filename.FullName, bDoxygenTagFile)
                Case EImportMode.MergeReferences
                    bResult = MergeFileReferences(Filename.FullName, bDoxygenTagFile)
                Case Else
                    MsgBox("Not yet implemented !", MsgBoxStyle.Exclamation)
            End Select
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

    Private Function ReplaceStringReferences(ByVal strXML As String) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim bImportOk As Boolean = True

            If m_xmlExport Is Nothing Then
                m_xmlExport = New XmlExportSpec()
                m_xmlExport.Document = MyBase.Document
            Else
                bImportOk = m_xmlExport.RemoveReferences()
            End If
            If bImportOk Then

                m_xmlExport.NodeCounter = m_xmlReferenceNodeCounter

                If m_xmlExport.LoadStringImport(strXML) Then
                    Me.AppendComponent(m_xmlExport)
                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Private Function MergeFileReferences(ByVal strFilename As String, ByVal bAskReplace As Boolean, _
                                         Optional ByVal bDoxygenTagFile As Boolean = False) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim bImportOk As Boolean = True

            If m_xmlExport Is Nothing Then
                MsgBox("Import empty, please use command 'Replace' !", MsgBoxStyle.Exclamation)
            Else
                Dim export As XmlExportSpec = New XmlExportSpec()
                export.Document = Me.Document
                export.NodeCounter = m_xmlReferenceNodeCounter

                If export.LoadImport(strFilename, bDoxygenTagFile) Then
                    export.LoadChildrenList()
                    For Each xmlcpnt As XmlComponent In export.ChildrenList
                        If m_xmlExport.CheckMerge(xmlcpnt, bAskReplace) Then
                            m_xmlExport.AppendComponent(xmlcpnt)
                            bResult = True
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Private Function ReplaceFileReferences(ByVal strFilename As String, Optional ByVal bDoxygenTagFile As Boolean = False) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim bImportOk As Boolean = True

            If m_xmlExport Is Nothing Then
                m_xmlExport = New XmlExportSpec()
                m_xmlExport.Document = MyBase.Document
            Else
                bImportOk = m_xmlExport.RemoveReferences()
            End If
            If bImportOk Then

                m_xmlExport.NodeCounter = m_xmlReferenceNodeCounter

                If m_xmlExport.LoadImport(strFilename, bDoxygenTagFile) Then
                    Me.AppendComponent(m_xmlExport)
                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function DuplicateReference(ByVal component As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim xmlComponent As XmlComponent = m_xmlExport.DuplicateComponent(component)
            If xmlComponent IsNot Nothing Then
                xmlComponent.Tag = m_xmlExport.Tag
                xmlComponent.Name = component.Name + "_copy"
                xmlComponent.SetIdReference(m_xmlReferenceNodeCounter, True)
                m_xmlExport.AppendComponent(xmlComponent)
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function RemoveReference(ByVal component As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            component.RemoveMe()
            bResult = CheckReferences()
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function CheckReferences() As Boolean
        Dim bResult As Boolean = False
        Try
            If m_xmlExport.SelectNodes().Count = 0 Then
                m_xmlExport.RemoveMe()
                m_xmlExport = Nothing
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function RemoveImport() As Boolean
        Dim bResult As Boolean = False
        Try
            If m_xmlExport.RemoveReferences() Then
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

