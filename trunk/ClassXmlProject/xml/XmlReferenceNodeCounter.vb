Imports System
Imports System.Xml
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.XmlNodeCounter

Public Class XmlReferenceNodeCounter

#Region "ID/IDREF counter/generator"

    Private m_PackageCounter As Integer = 0
    Private m_RelationCounter As Integer = 0

    Private m_ClassCounter As XmlNodeCounter
    Private Const cstPrefixClass As String = "class"

    Public Function GetNewClassId(Optional ByRef oCounter As XmlReferenceNodeCounter = Nothing) As String
        Dim strResult As String = cstPrefixClass

        If oCounter Is Nothing Then
            strResult = strResult + CStr(m_ClassCounter.GetNewId())
        Else
            strResult = strResult + CStr(m_ClassCounter.GetNewId(oCounter.m_ClassCounter))
        End If

        Return strResult
    End Function

    Public Function GetNewPackageId() As String
        m_PackageCounter += 1
        Return m_PackageCounter.ToString
    End Function

    Public Function GetNewRelationId() As String
        m_RelationCounter += 1
        Return m_RelationCounter.ToString
    End Function

    Public Sub InitItemCounters(ByVal docXML As XmlDocument)
        m_ClassCounter.Init(docXML, "//*[contains(@id,'class')]")
    End Sub
#End Region

#Region "NUM-ID generator"

    Public Shared Function GenerateNumericId(ByVal node As XmlNode, ByVal xpath As String, _
                                             Optional ByVal prefix As String = "", _
                                             Optional ByVal attribute As String = "num-id", _
                                             Optional ByVal bRaiseException As Boolean = True) As String
        Dim iResult As Integer = 0

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
                    If bRaiseException Then
                        Throw New Exception("In Node '" + node.Name + "' with name '" + GetName(node) + "'" + vbCrLf + vbCrLf + "has a child node with attribute '" + attribute + "' that does not start with prefix '" + prefix + "'")
                    Else
                        Id = 0
                    End If
                Else
                    tempo = AfterStr(child.Value, prefix)
                    If IsNumeric(tempo) Then
                        Id = CInt(tempo)
                    Else
                        Id = 0
                    End If
                End If

            ElseIf child.Value.StartsWith("CONST") = False _
            Then
                If IsNumeric(child.Value) = False Then
                    If bRaiseException Then
                        Throw New Exception("In Node '" + node.Name + "' with name '" + GetName(node) + "'" + vbCrLf + vbCrLf + "has a child node with attribute '" + attribute + "' that is not numeric")
                    Else
                        Id = 0
                    End If
                Else
                    Id = CInt(child.Value)
                End If
            End If

            If Id > iResult Then
                iResult = Id
            End If
        Next child

        iResult = iResult + 1
        Return prefix + CStr(iResult)
    End Function

#End Region

    Public Sub New(ByVal docXML As XmlDocument)
        m_ClassCounter = New XmlNodeCounter(cstPrefixClass)
        InitItemCounters(docXML)
    End Sub
End Class
