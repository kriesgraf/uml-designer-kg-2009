Imports System
Imports System.Xml
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlNodeCounter

Public Class XmlReferenceNodeCounter

#Region "ID/IDREF counter/generator"

    Private m_PackageCounter As XmlNodeCounter
    Private m_ClassCounter As XmlNodeCounter
    Private m_RelationCounter As XmlNodeCounter

    Private Const cstPrefixClass As String = "class"
    Private Const cstPrefixPackage As String = "package"
    Private Const cstPrefixRelation As String = "relation"

    Public ReadOnly Property CurrentClassId() As String
        Get
            Return m_ClassCounter.CurrentId
        End Get
    End Property

    Public ReadOnly Property CurrentPackageId() As String
        Get
            Return m_PackageCounter.CurrentId
        End Get
    End Property

    Public ReadOnly Property CurrentRelationId() As String
        Get
            Return m_RelationCounter.CurrentId
        End Get
    End Property

    Public Function GetMaxClassId() As Integer
        Try
            Return m_ClassCounter.GetMaxId

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetMaxPackageId() As Integer
        Return m_PackageCounter.GetMaxId
    End Function

    Public Function GetMaxRelationId() As Integer
        Try
            Return m_RelationCounter.GetMaxId

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function IsClassIdRecycled() As Boolean
        Return m_ClassCounter.IsIdRecycled
    End Function

    Public Function IsPackageIdRecycled() As Boolean
        Return m_PackageCounter.IsIdRecycled
    End Function

    Public Function IsRelationIdRecycled() As Boolean
        Return m_RelationCounter.IsIdRecycled
    End Function

    Public Function GetNewClassId(Optional ByRef oCounter As XmlReferenceNodeCounter = Nothing) As String
        Dim strResult As String = cstPrefixClass
        Try
            If oCounter Is Nothing Then
                strResult = strResult + CStr(m_ClassCounter.GetNewId())
            Else
                strResult = strResult + CStr(m_ClassCounter.GetNewId(oCounter.m_ClassCounter))
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Function GetNewPackageId(Optional ByRef oCounter As XmlReferenceNodeCounter = Nothing) As String
        Dim strResult As String = cstPrefixPackage
        Try
            If oCounter Is Nothing Then
                strResult = strResult + CStr(m_PackageCounter.GetNewId())
            Else
                strResult = strResult + CStr(m_PackageCounter.GetNewId(oCounter.m_PackageCounter))
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Function GetNewRelationId(Optional ByRef oCounter As XmlReferenceNodeCounter = Nothing) As String
        Dim strResult As String = cstPrefixRelation
        Try
            If oCounter Is Nothing Then
                strResult = strResult + CStr(m_RelationCounter.GetNewId())
            Else
                strResult = strResult + CStr(m_RelationCounter.GetNewId(oCounter.m_RelationCounter))
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Sub Recycle(ByVal node As XmlNode)
        Try
            Select Case node.Name
                Case "reference", "interface"
                    m_ClassCounter.Recycle(node)
                Case "class"
                    m_ClassCounter.Recycle(node)
                Case "package"
                    m_PackageCounter.Recycle(node)
                Case "typedef"
                    m_ClassCounter.Recycle(node)
                Case "relationship"
                    m_RelationCounter.Recycle(node)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitItemCounters(ByVal docXML As XmlDocument)
        Try
            m_ClassCounter.Init(docXML, "//*[contains(@id,'class')]")
            m_PackageCounter.Init(docXML, "//package")
            m_RelationCounter.Init(docXML, "//relationship")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "NUM-ID generator"

    Public Shared Function GenerateNumericId(ByVal node As XmlNode, ByVal xpath As String, _
                                             Optional ByVal prefix As String = "", _
                                             Optional ByVal attribute As String = "num-id") As String
        Dim iResult As Integer = 0

        Try
            Dim child As XmlNode
            Dim Id As Integer
            Dim list As XmlNodeList
            Dim tempo As String

            If prefix <> "" Then
                list = node.SelectNodes(xpath + "[contains(@" + attribute + ",'" + prefix + "')]/@" + attribute + "")
            Else
                list = node.SelectNodes(xpath + "[@" + attribute + "]/@" + attribute + "")
            End If

            For Each child In list

                If prefix <> "" Then
                    If child.Value.StartsWith(prefix) = False Then
                        Throw New Exception("In Node: " + node.OuterXml + vbCrLf + vbCrLf + "One child node with attribute '" + attribute + "' does not start with prefix '" + prefix + "'")
                    End If
                    tempo = AfterStr(child.Value, prefix)
                    If IsNumeric(tempo) Then
                        Id = CInt(tempo)
                    Else
                        Id = 0
                    End If

                ElseIf child.Value.StartsWith("CONST") = False _
                Then
                    If IsNumeric(child.Value) = False Then
                        Throw New Exception("In Node: " + node.OuterXml + vbCrLf + vbCrLf + "One child node with attribute '" + attribute + "' is not numeric")
                    End If

                    Id = CInt(child.Value)
                End If

                If Id > iResult Then
                    iResult = Id
                End If
            Next child

            iResult = iResult + 1

        Catch ex As Exception
            Throw ex
        End Try
        Return prefix + CStr(iResult)
    End Function

#End Region

    Public Sub New(ByVal docXML As XmlDocument)
        Try
            m_PackageCounter = New XmlNodeCounter(cstPrefixPackage)
            m_ClassCounter = New XmlNodeCounter(cstPrefixClass)
            m_RelationCounter = New XmlNodeCounter(cstPrefixRelation)
            InitItemCounters(docXML)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
