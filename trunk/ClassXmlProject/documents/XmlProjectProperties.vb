Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools
Imports System.ComponentModel
Imports System.Xml

Public Class XmlProjectProperties
    Inherits XmlComposite

    Public Function Edit() As Boolean
        Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(Me)
        fen.ShowDialog()
        Return CType(fen.Tag, Boolean)
    End Function

    <CategoryAttribute("Code generation"), _
    DescriptionAttribute("Project workspace")> _
    Public Property GenerationFolder() As String
        Get
            Return GetAttribute("destination", "generation")
        End Get
        Set(ByVal value As String)
            SetAttribute("destination", value, "generation")
        End Set
    End Property

    <CategoryAttribute("Code generation"), _
    DescriptionAttribute("Project language ID")> _
    Public Property GenerationLanguage() As Integer
        Get
            Dim value As Integer = CInt(GetAttribute("language", "generation"))
            Return value
        End Get
        Set(ByVal value As Integer)
            SetAttribute("language", CStr(value), "generation")
        End Set
    End Property


    Public ReadOnly Property Language() As String
        Get
            Return GetLanguage(GenerationLanguage)
        End Get
    End Property


    <CategoryAttribute("XmlComponent"), _
    Browsable(False), _
    DescriptionAttribute("Simple type list")> _
    Public ReadOnly Property SimpleTypesFilename() As String
        Get
            Return GetSimpleTypesFilename(Me.GenerationLanguage)
        End Get
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Brief comment")> _
    Public Property BriefComment() As String
        Get
            Return GetAttribute("brief", "comment")
        End Get
        Set(ByVal value As String)
            SetAttribute("brief", value, "comment")
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("detailed comment")> _
    Public Property Comment() As String
        Get
            Return GetNodeString("comment")
        End Get
        Set(ByVal value As String)
            SetNodeString("comment", value)
        End Set
    End Property

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode, bLoadChildren)
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()

            ' Order of initialization is important, xml node order is done here 
            Name = "New_project"
            GenerationFolder = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            GenerationLanguage = 0
            Comment = "Insert here details"
            BriefComment = "Insert here a brief comment"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Sub LoadChildrenList(Optional ByVal strViewName As String = "")
        Try
            AddChildren(SelectNodes("import | class | package | relationship"), strViewName)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Overrides Function AppendNode(ByVal child As XmlNode) As XmlNode
        Dim before As XmlNode = Nothing
        Select Case child.Name
            Case "import"
                before = GetNode("class")

                If before Is Nothing Then
                    before = GetNode("package")
                End If

                If before Is Nothing Then
                    before = GetNode("relationship")
                End If

            Case "class"
                before = GetNode("package")

                If before Is Nothing Then
                    before = GetNode("relationship")
                End If

            Case "package"
                before = GetNode("relationship")
        End Select

        If before Is Nothing Then
            Return Me.Node.AppendChild(child)
        Else
            Return Me.Node.InsertBefore(child, before)
        End If
    End Function
End Class

