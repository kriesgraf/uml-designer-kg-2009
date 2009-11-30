Imports System
Imports System.Xml

Public Class XmlContainerSpec
    Inherits XmlTypedefSpec

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode)

        ChangeReferences(bLoadChildren)

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
            Me.TypeVarDefinition.SetContainerValues(bCreateNodeNow)

            ' Range is initialized in class XmlTypeVarSpec, see m_xmlType member
            Id = "class0"
            Comment = "Insert here a comment"

        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Protected Friend Overrides Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, _
                                                    Optional ByVal eRename As ENameReplacement = ENameReplacement.NewName, _
                                                    Optional ByVal bSetIdrefChildren As Boolean = False)
        If xmlRefNodeCounter Is Nothing Then
            Throw New Exception("Argument 'xmlRefNodeCounter' is null")
        End If

        Me.Id = xmlRefNodeCounter.GetNewClassId()
        Dim strId As String = XmlNodeCounter.AfterStr(Me.Id, "class")

        Select Case eRename
            Case ENameReplacement.NewName
                Name = "New_container_" + strId
            Case ENameReplacement.AddCopyName
                ' Name is set by caller
                Name = Name + "_" + strId
        End Select
    End Sub
End Class
