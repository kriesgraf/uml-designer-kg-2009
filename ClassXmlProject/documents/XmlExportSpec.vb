Imports System
Imports ClassXmlProject.XmlProjectTools
Imports System.Xml
Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic

Public Class XmlExportSpec
    Inherits XmlComposite
    Implements InterfNodeCounter

#Region "Class declarations"

    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

#End Region

#Region "Properties"

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

    Public Property Source() As String
        Get
            Return GetAttribute("source")
        End Get
        Set(ByVal value As String)
            SetAttribute("source", value)
        End Set
    End Property
#End Region

#Region "Public methods"

    Public Overrides Sub LoadChildrenList(Optional ByVal strViewName As String = "")
        Try
            AddChildren(SelectNodes("reference | interface"), strViewName)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode, bLoadChildren)
    End Sub

    Public Function SearchRedundancies(ByVal projectNode As XmlComponent, ByVal name As String) As Boolean
        Dim child As XmlNode

        For Each child In SelectNodes("reference | interface")
            Select Case dlgRedundancy.VerifyRedundancy(projectNode, _
                                                       "Find redundancies in file '" + name + "' with project...", _
                                                       child, name, False, True)
                Case dlgRedundancy.EResult.RedundancyIgnoredAll
                    Exit For

                Case Else
                    ' Ok, Ignore
            End Select
        Next child
        Return True
    End Function

#End Region

#Region "Protected friend methods"

    Protected Friend Overrides Function AppendNode(ByVal nodeXml As XmlNode, Optional ByVal observer As Object = Nothing) As XmlNode
        Dim interf As InterfProgression = TryCast(observer, InterfProgression)
        Try
            Select Case nodeXml.Name
                Case "reference", "interface"
                    Return Me.Node.AppendChild(nodeXml)
                Case Else
                    Dim list As XmlNodeList = nodeXml.SelectNodes("descendant::reference | descendant::interface")
                    If interf IsNot Nothing Then
                        interf.Minimum = 0
                        interf.Maximum = list.Count
                        interf.ProgressBarVisible = True
                    End If
                    For Each child As XmlNode In list
                        Me.Node.AppendChild(child)
                        If interf IsNot Nothing Then interf.Increment(1)
                    Next
            End Select
        Catch ex As Exception
            Throw ex
        Finally
            If interf IsNot Nothing Then interf.ProgressBarVisible = False
        End Try
        Return Nothing
    End Function

    Protected Friend Sub ReplaceReference(ByVal destination As XmlNodeListView)
        Dim origin As XmlNode = FindNode(destination)   ' If origin is nothing append at the end of list
        Me.Node.InsertBefore(Me.Document.ImportNode(destination.Node, True), origin)
        If destination.Info Then
            RemoveNode(origin)
        End If
    End Sub

    Protected Friend Function RemoveReferences() As Boolean
        Dim bResult As Boolean = True
        Try
            Dim child As XmlNode
            Dim count As Integer
            Dim strList As String = ""

            If Node Is Nothing Then Exit Function

            For Each child In Node.ChildNodes
                count = GetNodeRefCount(child, strList, CType(Me.Tag, ELanguage))
                If count > 0 Then
                    MsgBox("Reference " + GetName(child) + " is used by " + CStr(count) + " element(s):" + vbCrLf + strList, vbExclamation)
                    bResult = False
                    Exit For
                End If
            Next child
            If bResult Then
                For Each child In Node.ChildNodes
                    m_xmlReferenceNodeCounter.Recycle(child)
                Next child
                RemoveMe()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Protected Friend Function LoadStringImport(ByVal strXML As String) As Boolean
        Dim bResult As Boolean = False
        Try
            ' the following call to Validate succeeds.
            Dim document As New XmlDocument
            document.LoadXml(strXML)

            ' Convert "document" context to "MyBase.Document" context
            MyBase.Node = MyBase.Document.ImportNode(document.DocumentElement, True)
            bResult = True

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Protected Friend Function LoadImport(ByVal form As Form, ByVal projectNode As XmlComponent, _
                                         ByVal strFilename As String, Optional ByVal bDoxygenTagFile As Boolean = False) As Boolean
        Dim bResult As Boolean = False
        Try
            ' the following call to Validate succeeds.
            Dim document As New XmlDocument
            Dim strSource As String = GetProjectPath(strFilename)

            If bDoxygenTagFile Then
                XmlProjectTools.ConvertDoxygenTagFile(form, document, strFilename)
            Else
                If UseDocTypeDeclarationFileForImport(strSource) = False Then
                    Return False
                End If
                If XmlProjectTools.LoadDocument(form, document, strFilename) = EResult.Failed Then
                    Return False
                End If
            End If
            ' Convert "document" context to "MyBase.Document" context
            MyBase.Node = Me.Document.ImportNode(document.DocumentElement, True)
            UpdateExportReferences()
            bResult = True

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Protected Friend Function GetMerge(ByVal component As XmlComponent) As XmlNode
        Return GetNode(component.NodeName + "[@name='" + component.Name + "']")
    End Function

#End Region

#Region "private methods"

    Private Sub UpdateExportReferences()
        Dim child As XmlNode
        Dim strID As String

        If Me.Node Is Nothing Then Exit Sub

        For Each child In SelectNodes("reference | interface")
            strID = m_xmlReferenceNodeCounter.GetNewClassId()
            ChangeID(child, Me.Node, strID)
            SetID(child, strID)

            Dim pos As Integer = 0
            Dim strEnumID As String
            For Each enumvalue As XmlNode In child.SelectNodes("enumvalue")
                pos += 1
                strEnumID = "enum" + XmlNodeCounter.AfterStr(strID, "class") + "_" + pos.ToString
                ChangeID(enumvalue, Me.Node, strEnumID)
                SetID(enumvalue, strEnumID)
            Next enumvalue
        Next child
    End Sub

#End Region

End Class
