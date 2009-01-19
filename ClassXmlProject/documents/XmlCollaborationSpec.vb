Imports System.ComponentModel
Imports System.Xml

Public Class XmlCollaborationSpec
    Inherits XmlComponent

    Private m_xmlRelation As XmlRelationSpec

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Parent name")> _
    Public Overrides Property Name() As String
        Get
            Return Me.RelationParent.Name
        End Get
        Set(ByVal value As String)
            Me.RelationParent.Name = value
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Class member")> _
    Public Property Member() As Boolean
        Get
            Return Me.RelationParent.Member
        End Get
        Set(ByVal value As Boolean)
            Me.RelationParent.Member = value
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("C++ parent class declaration")> _
    Public ReadOnly Property FullpathTypeDescription() As String
        Get
            Return Me.RelationParent.FullpathTypeDescription
        End Get
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Relation component id")> _
    Public Property RelationId() As String
        Get
            Return MyBase.GetAttribute("idref")
        End Get
        Set(ByVal value As String)
            ValidateIdReference(value, m_bCreateNodeNow)
            MyBase.SetAttribute("idref", value)
        End Set
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Parent component id")> _
    Public ReadOnly Property ClassId() As String
        Get
            Return MyBase.GetAttribute("id", "parent::class")
        End Get
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Relation component")> _
    Public ReadOnly Property RelationSpec() As XmlRelationSpec
        Get
            If m_xmlRelation IsNot Nothing Then
                m_xmlRelation.Tag = Me.Tag
            End If
            Return m_xmlRelation
        End Get
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Relation parent component")> _
    Public ReadOnly Property RelationParent() As XmlRelationParentSpec
        Get
            Dim xmlResult As XmlRelationParentSpec = m_xmlRelation.Reference(Me.ClassId)
            xmlResult.Tag = Me.Tag
            Return xmlResult
        End Get
    End Property

    <TypeConverter(GetType(UmlMemberVisibility)), _
    CategoryAttribute("UML design"), _
    DescriptionAttribute("Relation visibility")> _
    Public Property Range() As String
        Get
            Return Me.RelationParent.Range
        End Get
        Set(ByVal value As String)
            Me.RelationParent.Range = value
        End Set
    End Property

    Public Sub New(Optional ByRef xmlNode As Xml.XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode)
        Try
            ChangeReferences(bLoadChildren)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()

            m_xmlRelation.SetDefaultValues(bCreateNodeNow)
            RelationId = m_xmlRelation.Id

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        Try
            MyBase.ChangeReferences(bLoadChildren)

            Dim nodeXml As XmlNode = GetNodeRef()

            If nodeXml Is Nothing Then
                If m_bCreateNodeNow Then
                    nodeXml = Me.Document.DocumentElement.AppendChild(CreateNode("relationship"))
                    m_xmlRelation = TryCast(CreateDocument(nodeXml, bLoadChildren), XmlRelationSpec)
                End If
            Else
                m_xmlRelation = TryCast(CreateDocument(nodeXml, bLoadChildren), XmlRelationSpec)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Overrides Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, Optional ByVal bParam As Boolean = False)
        If xmlRefNodeCounter Is Nothing Then
            Throw New Exception("Argument 'xmlRefNodeCounter' is null")
        End If

        m_xmlRelation.SetIdReference(xmlRefNodeCounter, True)
        RelationId = m_xmlRelation.Id
    End Sub

    Public Overrides Sub NotifyInsert(Optional ByVal before As XmlComponent = Nothing)
        Me.RelationSpec.Father.Idref = Me.ClassId
        Me.RelationSpec.Child.Idref = MyBase.GetFirstClassId(Me.ClassId)
        ' No update father because first created collaboration owns father
        Me.RelationSpec.Child.UpdateRelation("")
    End Sub

    Protected Friend Overrides Function RemoveMe() As Boolean
        ' Remove Cascading with relationship node
        Return Me.RelationSpec.RemoveMe()
    End Function
End Class
