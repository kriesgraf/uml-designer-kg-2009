Imports System
Imports System.ComponentModel
Imports System.Xml
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlNodeCounter
Imports ClassXmlProject.XmlReferenceNodeCounter

Public Class XmlEnumSpec
    Inherits XmlComponent

    Private Const cstInitialName As String = "NEW_ENUMVALUE"

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

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Fixed value")> _
    Public Property Value() As String
        Get
            Return GetAttribute("value")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("value")
            Else
                m_bCreateNodeNow = True
                SetAttribute("value", value)
                m_bCreateNodeNow = False
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Brief comment")> _
    Public Property Comment() As String
        Get
            If MyBase.Node IsNot Nothing Then
                Return MyBase.Node.InnerText
            End If
            Return ""
        End Get
        Set(ByVal value As String)
            MyBase.Node.InnerText = value
        End Set
    End Property

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing)
        MyBase.New(xmlNode)
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            ' We use MyBase to avoid overrided property that forbid change with cstInitialName value
            MyBase.Name = cstInitialName
            ChangeReferences()
            Id = "enum0"
            Comment = "Brief comment"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Sub NotifyInsert(Optional ByVal before As XmlComponent = Nothing)

        Dim strPrefix As String = ""
        Dim nodeXml As XmlNode = GetNode("ancestor::typedef/@id")
        If nodeXml Is Nothing Then
            nodeXml = GetNode("ancestor::property/@num-id")
            strPrefix = nodeXml.Value + "_"
            nodeXml = GetNode("ancestor::class/@id")
            strPrefix = AfterStr(nodeXml.Value, "class") + "_" + strPrefix
        Else
            strPrefix += AfterStr(nodeXml.Value, "class") + "_"
        End If
        strPrefix = "enum" + strPrefix

        Me.Id = GenerateNumericId(Me.Node.ParentNode, "enumvalue", strPrefix, "id")
        Me.Name = cstInitialName + AfterStr(Me.Id, strPrefix)
    End Sub

    Protected Friend Function CheckName() As Boolean
        Return (Me.Name <> cstInitialName)
    End Function
End Class
