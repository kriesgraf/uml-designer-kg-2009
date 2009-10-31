Imports System
Imports System.Xml
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class XmlNodeCounter

    Private m_iCounter As Integer
    Private m_CountDeletedList As Collection
    Private m_strPrefix As String

    Public Sub New(ByVal value As String)
        m_strPrefix = value
        m_CountDeletedList = New Collection
    End Sub

    Private Sub Clear()
        Try
            m_CountDeletedList.Clear()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public WriteOnly Property Prefix() As String
        Set(ByVal value As String)
            m_strPrefix = value
        End Set
    End Property

    Public ReadOnly Property CurrentId() As String
        Get
            Return CStr(m_iCounter - 1)
        End Get
    End Property

    Public Function GetMaxId() As Integer
        GetMaxId = m_iCounter
        m_iCounter = m_iCounter + 1
    End Function

    Public Function IsIdRecycled() As Boolean
        Return m_CountDeletedList.Count > 0
    End Function

    Public Function GetNewId(Optional ByRef oCounter As XmlNodeCounter = Nothing) As Integer

        Dim iResult As Integer = -1

        ' Quand on chercher a faire fusionner deux arbres il faut comparer
        ' simutanément les ID des deux arbres et toujours prendre le plus grand
        ' pour être sûr de ne pas rependre la référence d'un élément existant dans
        ' l'un des deux arbres
        Try

            If Not oCounter Is Nothing Then
                iResult = GetMaxId()
                If iResult <= CInt(oCounter.CurrentId) Then
                    iResult = oCounter.GetMaxId()
                    m_iCounter = iResult + 1
                End If
            Else
                If m_CountDeletedList.Count > 0 Then

                    ' Si il y a des "trous" dans la liste des ID, on en prend dans cette collection
                    iResult = CInt(m_CountDeletedList.Item(1))
                    m_CountDeletedList.Remove(1)
                Else
                    ' Sinon on prend l'index suivant...
                    iResult = GetMaxId()
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return iResult
    End Function

    Public Sub Init(ByVal docXML As XmlDocument, ByVal strQuery As String)
        Try
            Clear()
            m_iCounter = GetNextId(docXML, m_CountDeletedList, docXML.SelectNodes(strQuery), m_strPrefix)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function GetNextId(ByVal docXML As XmlDocument, ByRef deleteList As Collection, ByRef list As XmlNodeList, ByVal m_strPrefix As String) As Integer

        Dim child As XmlNode
        Dim node As XmlNode
        Dim tempo As String
        Dim Id As Integer
        Dim iResult As Integer

        Try
            iResult = 0

            For Each child In list
                Dim strID As String = GetID(child)

                If strID.StartsWith(m_strPrefix) = False Then
                    Throw New Exception("In Node: " + child.OuterXml + vbCrLf + vbCrLf + "Attribute 'id' does not start with prefix '" + m_strPrefix + "'")
                End If

                tempo = AfterStr(strID, m_strPrefix)
                If IsNumeric(tempo) Then
                    Id = CInt(tempo)
                Else
                    Id = 0
                End If

                If Id > iResult Then
                    iResult = Id
                End If
            Next child

            If list.Count > 0 Then

                node = list.Item(0)

                For Id = 1 To iResult
                    child = docXML.GetElementById(m_strPrefix + CStr(Id))

                    If child Is Nothing Then
                        deleteList.Add(Id, CStr(Id))
                    End If
                Next Id
            End If
            iResult = iResult + 1

        Catch ex As Exception
            Throw ex
        End Try

        Return iResult
    End Function

    Public Shared Function AfterStr(ByVal strText As String, ByVal strSearch As String) As String
        Dim i As Integer
        Dim strResult As String = ""

        i = InStr(strText, strSearch)

        If i > 0 Then
            strResult = Mid(strText, i + Len(strSearch))
        End If
        Return strResult
    End Function

    Public Shared Function BeforeStr(ByVal strText As String, ByVal strSearch As String) As String
        Dim i As Integer
        Dim strResult As String = ""

        i = InStr(strText, strSearch)

        If i > 1 Then
            strResult = Left(strText, i - 1)
        End If
        Return strResult
    End Function
End Class
