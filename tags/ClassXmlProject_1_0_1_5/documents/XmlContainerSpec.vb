Imports System
Imports System.Xml

Public Class XmlContainerSpec
    Inherits XmlTypedefSpec

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode)
        Try
            ChangeReferences(bLoadChildren)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Overrides Function CreateNode(ByVal name As String) As XmlNode
        If name = "container_doc" Then
            Return MyBase.CreateNode("typedef")
        End If
        Return MyBase.CreateNode(name)
    End Function

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            ' This method reset m_bCreateNodeNow !
            MyBase.SetDefaultValues(bCreateNodeNow)

            m_bCreateNodeNow = bCreateNodeNow
            Name = "New_container"
            ChangeReferences()
            m_xmlType.SetContainerValues(bCreateNodeNow)

            ' Range is initialized in class XmlTypeVarSpec, see m_xmlType member
            Id = "class0"
            Comment = "Insert here a comment"
        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Protected Friend Overrides Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, Optional ByVal bParam As Boolean = False)
        If xmlRefNodeCounter Is Nothing Then
            Throw New Exception("Argument 'xmlRefNodeCounter' is null")
        End If

        Id = xmlRefNodeCounter.GetNewClassId()
    End Sub
End Class
