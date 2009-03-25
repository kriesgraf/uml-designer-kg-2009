Imports System
Imports System.ComponentModel
Imports System.Xml
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlNodeCounter

Public Class XmlEnumSpec
    Inherits XmlComponent

    Private Const cstInitialName As String = "NEW_ENUMVALUE"

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Name. Must be different than " + cstInitialName)> _
    Public Overrides Property Name() As String
        Get
            Return MyBase.Name
        End Get
        Set(ByVal value As String)
            If MyBase.Name = cstInitialName Then
                ' If value is/contains  "cstInitialName" we refused to update "Name"
                ' Because use Name and typedef ID to make a unique ID
                If value.StartsWith(cstInitialName) = False Then
                    Me.Id = Me.Id + "_" + value
                    MyBase.Name = value
                Else
                    MsgBox("Please choose a name different than '" + cstInitialName + "'", MsgBoxStyle.Exclamation)
                End If
            Else
                MyBase.Name = value
            End If
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
        Dim node As XmlNode = GetNode("ancestor::typedef/@id")
        Dim strId As String = ""
        If node Is Nothing Then
            node = GetNode("ancestor::property/@num-id")
            strId = node.Value
            node = GetNode("ancestor::class/@id")
            strId = AfterStr(node.Value, "class") + "_" + strId + "_"
        Else
            strId = AfterStr(node.Value, "class")
        End If
        Id = "enum" + strId
    End Sub

    Protected Friend Function CheckName() As Boolean
        Return (Me.Name <> cstInitialName)
    End Function
End Class
