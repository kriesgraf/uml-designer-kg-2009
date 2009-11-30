Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.XmlNodeListView
Imports Microsoft.VisualBasic
Imports System.ComponentModel
Imports System.Xml

Public Class XmlProjectProperties
    Inherits XmlComposite
    Implements InterfListViewContext
    Implements InterfNodeCounter

    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

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
    Public NotOverridable Overrides Property GenerationLanguage() As ELanguage
        Get
            Dim value As ELanguage = CType(CInt(GetAttribute("language", "generation")), ELanguage)
            m_iTag = CInt(value)
            Return value
        End Get
        Set(ByVal value As ELanguage)
            m_iTag = CInt(value)
            SetAttribute("language", CStr(m_iTag), "generation")
        End Set
    End Property

    <CategoryAttribute("Code generation"), _
    Browsable(False), _
    DescriptionAttribute("Integer value")> _
    Public Property ValueLanguage() As Integer
        Get
            Return CInt(Me.GenerationLanguage)
        End Get
        Set(ByVal value As Integer)
            Me.GenerationLanguage = CType(value, ELanguage)
        End Set
    End Property

    <CategoryAttribute("Code generation"), _
    Browsable(False), _
    DescriptionAttribute("String value")> _
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

    Public ReadOnly Property CurrentContext() As String Implements InterfListViewContext.CurrentContext
        Get
            Return "project"
        End Get
    End Property

    Public Function Edit() As Boolean
        Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(Me)
        fen.ShowDialog()
        Return CType(fen.Tag, Boolean)
    End Function

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

        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Sub LoadChildrenList(Optional ByVal strViewName As String = "")

        AddChildren(SelectNodes("import | class | package | relationship"), strViewName)

    End Sub

    Public Overrides Function CanRemove(ByVal removedNode As XmlComponent) As Boolean

        Select Case removedNode.NodeName
            Case "class"
                ' Search link from parent node
                If SelectNodes(GetQueryListDependencies(removedNode)).Count > 0 Then

                    If MsgBox("Some elements reference this, you can dereference them and then this will be deleted." + _
                          vbCrLf + "Do you want to proceed ?", _
                        cstMsgYesNoQuestion, _
                        removedNode.Name) = MsgBoxResult.Yes _
                    Then
                        Dim bIsEmpty As Boolean = False

                        If dlgDependencies.ShowDependencies(m_xmlReferenceNodeCounter, removedNode, bIsEmpty, "Remove references to " + removedNode.Name) Then
                            Me.Updated = True
                        End If

                        Return bIsEmpty
                    End If
                Else
                    Return True
                End If

            Case "package", "import"
                ' Search children from removed node
                If removedNode.SelectNodes(GetQueryListDependencies(removedNode)).Count > 0 Then
                    MsgBox("This element is not empty", MsgBoxStyle.Exclamation, removedNode.Name)
                Else
                    Return True
                End If
            Case "relationship"
                Return True
        End Select

        Return False
    End Function

    Public Overrides Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False

        Dim strName As String = removeNode.Name

        If removeNode.NodeName = "relationship" Then
            Dim element As XmlProjectMemberView = TryCast(removeNode, XmlProjectMemberView)
            If element IsNot Nothing Then
                strName = "Relation '" + element.Label(0) + "'" + vbCrLf + element.ToolTipText
            End If
        End If

        If MsgBox("Confirm to delete:" + vbCrLf + "Name: " + strName, _
                   cstMsgYesNoQuestion, "'Delete' command") = MsgBoxResult.Yes _
        Then
            Dim strNodeName As String = removeNode.NodeName
            If MyBase.RemoveComponent(removeNode) _
            Then
                bResult = True
            End If
        End If

        Return bResult
    End Function

    Public Sub TrimComments()
        XmlProjectTools.TrimComments(Me.Document.DocumentElement)
    End Sub

    Protected Friend Overrides Function RemoveRedundant(ByVal component As XmlComponent) As Boolean
        If component IsNot Nothing Then
            Select Case component.NodeName
                Case "class"
                    If dlgRedundancy.VerifyRedundancy(Me, "Check redundancies...", component.Node) _
                        = dlgRedundancy.EResult.RedundancyChanged _
                    Then
                        Me.Updated = True
                        Return True
                    End If

                Case Else
                    MsgBox("This node contains children !", MsgBoxStyle.Exclamation, "'Search dependencies' command")
                    Return False
            End Select
        End If
        Return False
    End Function

    Public Overrides Function CanAddComponent(ByVal nodeXml As XmlComponent) As Boolean
        Select Case nodeXml.NodeName
            Case "import", "class", "package", "relationship"
                Return True
        End Select
        Return False
    End Function

    Protected Friend Overrides Function AppendNode(ByVal child As XmlNode, Optional ByVal observer As Object = Nothing) As XmlNode
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

