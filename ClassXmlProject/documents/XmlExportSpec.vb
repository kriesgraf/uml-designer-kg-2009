Imports ClassXmlProject.XmlProjectTools
Imports System.Xml
Imports System.IO

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
            AddChildren(SelectNodes("reference"), strViewName)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub New(Optional ByRef xmlNode As Xml.XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode, bLoadChildren)
    End Sub

    Protected Friend Overrides Function AppendNode(ByVal nodeXml As System.Xml.XmlNode) As System.Xml.XmlNode
        If nodeXml.Name = "reference" Then
            Return MyBase.AppendNode(nodeXml)
        End If
        For Each child As XmlNode In nodeXml.SelectNodes("descendant::reference")
            MyBase.AppendNode(child)
        Next
        Return Nothing
    End Function

#End Region

    Protected Friend Function RemoveReferences() As Boolean
        Dim bResult As String = True
        Try
            Dim child As XmlNode
            Dim count As Integer
            Dim strList As String = ""

            If Node Is Nothing Then Exit Function

            For Each child In Node.ChildNodes
                count = GetNodeRefCount(child, strList, Me.Tag)
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
        Dim bResult As String = False
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

    Protected Friend Function LoadImport(ByVal strFilename As String, Optional ByVal bDoxygenTagFile As Boolean = False) As Boolean
        Dim bResult As String = False
        Try
            ' the following call to Validate succeeds.
            Dim document As New XmlDocument
            Dim strSource As String = GetProjectPath(strFilename)

            If bDoxygenTagFile Then
                XmlProjectTools.ConvertDoxygenTagFile(document, strFilename)
            Else
                If UseDocTypeDeclarationFileForImport(strSource) = False Then
                    Return False
                End If
                XmlProjectTools.LoadDocument(document, strFilename)
            End If
            ' Convert "document" context to "MyBase.Document" context
            MyBase.Node = MyBase.Document.ImportNode(document.DocumentElement, True)
            UpdateExportReferences()
            bResult = True

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Protected Friend Function CheckMerge(ByVal component As XmlComponent, ByVal bAskReplace As Boolean) As Boolean
        Dim child As XmlNode = MyBase.GetNode("reference[@name='" + component.Name + "']")
        If child IsNot Nothing Then
            If bAskReplace Then
                Return False
            Else
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub UpdateExportReferences()
        Dim child As XmlNode

        If Node Is Nothing Then Exit Sub

        For Each child In MyBase.SelectNodes("descendant::reference")
            SetID(child, m_xmlReferenceNodeCounter.GetNewClassId())
        Next child
    End Sub
End Class
