Imports System
Imports System.Xml
Imports System.ComponentModel
Public Class XmlStructureSpec
    Inherits XmlTypedefSpec

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Union structure")> _
    Public Property Union() As Boolean
        Get
            Return (Me.TypeVarDefinition.CheckAttribute("struct", "union", "struct"))
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.TypeVarDefinition.SetAttribute("struct", "union")
            Else
                Me.TypeVarDefinition.SetAttribute("struct", "struct")
            End If
        End Set
    End Property

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode)
        Try
            ChangeReferences(bLoadChildren)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Overrides Function CreateNode(ByVal name As String) As XmlNode
        If name = "structure_doc" Then
            Return MyBase.CreateNode("typedef")
        End If
        Return MyBase.CreateNode(name)
    End Function

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            ' This method reset m_bCreateNodeNow !
            MyBase.SetDefaultValues(bCreateNodeNow)

            m_bCreateNodeNow = bCreateNodeNow
            Name = "New_structure"
            ChangeReferences()
            Me.TypeVarDefinition.SetStructureValues(bCreateNodeNow)

            ' Range is initialized in class XmlTypeVarSpec, see m_xmlType member
            Id = "class0"
            Comment = "Insert here a comment"
        Catch ex As Exception
            Throw ex
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
                Name = "New_structure_" + strId
            Case ENameReplacement.AddCopyName
                ' Name is set by caller
                Name = Name + "_" + strId
        End Select

        ' Use this option only to paste typedef from another project
        If bSetIdrefChildren Then
            ' Change idref for attributes: type/@idref or list/@idref and list/@index-ref
            For Each unref As XmlNode In Me.SelectNodes("descendant::*/@idref | descendant::*/@index-ref")
                unref.Value = Me.Id       ' We change to this arbitray ID to avoid error. We let user to change it himself
            Next
        End If
    End Sub
End Class
