Imports System
Imports System.ComponentModel
Imports ClassXmlProject.XmlProjectTools
Imports System.Xml

Public Class XmlTypedefSpec
    Inherits XmlComponent

    Private m_xmlType As XmlTypeVarSpec

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Full path type description")> _
    Public ReadOnly Property FullpathTypeDescription() As String
        Get
            Return Me.TypeVarDefinition.FullpathTypeDescription
        End Get
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Type node")> _
    Public ReadOnly Property TypeVarDefinition() As XmlTypeVarSpec
        Get
            If m_xmlType IsNot Nothing Then
                m_xmlType.Tag = Me.Tag
            End If
            Return m_xmlType
        End Get
    End Property

    'Public ReadOnly Property Member() As String
    '    Get
    '        Return ""
    '    End Get
    'End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Comment")> _
    Public Property Comment() As String
        Get
            Return GetNodeString("comment")
        End Get
        Set(ByVal value As String)
            SetNodeString("comment", value)
        End Set
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Component id")> _
    Public Property Id() As String
        Get
            Return GetAttribute("id")
        End Get
        Set(ByVal value As String)
            SetAttribute("id", value)
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberVisibility)), _
    CategoryAttribute("UML design"), _
    DescriptionAttribute("Constructor visibility")> _
    Public Property Range() As String
        Get
            Return Me.TypeVarDefinition.Range
        End Get
        Set(ByVal value As String)
            Me.TypeVarDefinition.Range = value
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

    Public Overrides Function Clone(ByVal nodeXml As XmlNode, Optional ByVal bLoadChildren As Boolean = False) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            If nodeXml Is Nothing _
            Then
                xmlResult = MyBase.Clone(Nothing, bLoadChildren)
            Else
                Dim xmlAttribut As XmlNode = nodeXml.SelectSingleNode("type/@struct")
                If xmlAttribut Is Nothing _
                Then
                    xmlResult = MyBase.Clone(nodeXml, bLoadChildren)
                Else
                    Select Case xmlAttribut.Value
                        Case "union"
                            xmlResult = New XmlStructureSpec(nodeXml, bLoadChildren)
                        Case "struct"
                            xmlResult = New XmlStructureSpec(nodeXml, bLoadChildren)
                        Case "container"
                            xmlResult = New XmlContainerSpec(nodeXml, bLoadChildren)
                    End Select
                End If
            End If

            If xmlResult IsNot Nothing Then xmlResult.Tag = Me.Tag

        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Public Overrides Function AppendComponent(ByVal nodeXml As XmlComponent, Optional ByVal observer As Object = Nothing) As System.Xml.XmlNode
        Return Me.TypeVarDefinition.AppendComponent(nodeXml, observer)
    End Function

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            ' This method reset m_bCreateNodeNow !
            MyBase.SetDefaultValues(bCreateNodeNow)

            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()
            Me.TypeVarDefinition.SetDefaultValues(bCreateNodeNow)
            Me.TypeVarDefinition.Descriptor = "int16"
            Me.TypeVarDefinition.Range = "public"

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
        Dim index As Integer = 1
        For Each enumvalue In Me.SelectNodes("descendant::enumvalue")
            SetID(enumvalue, "enum" + strId + "_" + index.ToString)
            index += 1
        Next

        Select Case eRename
            Case ENameReplacement.NewName
                Name = "New_typedef_" + strId
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

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        Try
            MyBase.ChangeReferences(bLoadChildren)

            Dim nodeXml As XmlNode
            If TestNode("type") = False And m_bCreateNodeNow Then
                nodeXml = CreateAppendNode("type")
            Else
                nodeXml = GetNode("type")
            End If
            m_xmlType = TryCast(CreateDocument(nodeXml, bLoadChildren), XmlTypeVarSpec)
            If m_xmlType IsNot Nothing Then m_xmlType.Tag = Me.Tag

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class

