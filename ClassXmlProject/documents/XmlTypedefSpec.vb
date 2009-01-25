Imports System
Imports System.ComponentModel
Imports ClassXmlProject.XmlProjectTools
Imports System.Xml

Public Class XmlTypedefSpec
    Inherits XmlComponent

    Protected m_xmlType As XmlTypeVarSpec

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Full path type description")> _
    Public ReadOnly Property FullpathTypeDescription() As String
        Get
            Return m_xmlType.FullpathTypeDescription
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
            Return m_xmlType.Range
        End Get
        Set(ByVal value As String)
            m_xmlType.Range = value
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
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function


    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            ' This method reset m_bCreateNodeNow !
            MyBase.SetDefaultValues(bCreateNodeNow)

            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()
            m_xmlType.SetDefaultValues(bCreateNodeNow)
            m_xmlType.Descriptor = "int16"
            m_xmlType.Range = "public"

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

        For Each enumvalue In XmlProjectTools.SelectNodes(Me.Node, "descendant::enumvalue")
            Dim strId2 As String = GetID(enumvalue)
            SetID(enumvalue, "enum" + XmlNodeCounter.AfterStr(Me.Id, "class") + "_" + XmlNodeCounter.AfterStr(strId2, "_"))
        Next
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
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class

