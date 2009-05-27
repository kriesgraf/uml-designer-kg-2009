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

    Public ReadOnly Property CurrentContext() As String Implements InterfListViewContext.CurrentContext
        Get
            Return "project"
        End Get
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

    Public Overrides Function CanRemove(ByVal removedNode As XmlComponent) As Boolean
        Try
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
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Public Overrides Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim strName As String = removeNode.Name
            If MsgBox("Confirm to delete:" + vbCrLf + "Name: " + strName, _
                       cstMsgYesNoQuestion, "'Delete' command") = MsgBoxResult.Yes _
            Then
                Dim strNodeName As String = removeNode.NodeName
                If MyBase.RemoveComponent(removeNode) _
                Then
                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

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

