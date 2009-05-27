Imports System
Imports System.ComponentModel
Imports System.Xml
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlNodeListView
Imports ClassXmlProject.XmlProjectTools

Public Class XmlPackageSpec
    Inherits XmlComposite
    Implements InterfNodeCounter

    Protected m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

#Region "Properties"

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    Browsable(False), _
   DescriptionAttribute("Check location folder")> _
    Public ReadOnly Property IsFolder() As Boolean
        Get
            Return (GetAttribute("folder") <> "")
        End Get
    End Property


    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Source location folder. When empty same as name.")> _
    Public Property Folder() As String
        Get
            Dim strResult As String = GetAttribute("folder")
            'If strResult = "" Then
            '    strResult = Me.Name
            'End If
            Return strResult
        End Get
        Set(ByVal value As String)
            If value = "" Or value = Me.Name Then
                RemoveAttribute("folder")
            Else
                AddAttribute("folder", value)
            End If
        End Set
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
    DescriptionAttribute("Detail comment")> _
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

#End Region

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode, bLoadChildren)
    End Sub

    Public Overrides Function CanPasteItem(ByVal child As XmlComponent) As Boolean
        Select Case child.NodeName
            Case "class", "import", "package"
                Return True

            Case Else
                Return MyBase.CanPasteItem(child)
        End Select
        Return MyBase.CanPasteItem(child)
    End Function

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()
            Name = "New_package"
            Id = "package0"
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
            AddChildren(SelectNodes("*[name()!='comment']"), strViewName)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overrides Function CanRemove(ByVal removeNode As XmlComponent) As Boolean
        Try
            Select Case removeNode.NodeName

                Case "package", "import"
                    ' Search children from removed node
                    If removeNode.SelectNodes(GetQueryListDependencies(removeNode)).Count > 0 Then
                        MsgBox("This element is not empty", MsgBoxStyle.Exclamation, removeNode.Name)
                    Else
                        Return True
                    End If

                Case Else
                    If SelectNodes(GetQueryListDependencies(removeNode)).Count > 0 _
                    Then
                        If MsgBox("Some elements reference this, you can dereference them and then this will be deleted." + _
                                  vbCrLf + "Do you want to proceed", _
                                    cstMsgYesNoQuestion, _
                                    removeNode.Name) = MsgBoxResult.Yes _
                        Then
                            Dim bIsEmpty As Boolean = False

                            If dlgDependencies.ShowDependencies(m_xmlReferenceNodeCounter, removeNode, bIsEmpty, "Remove references to " + removeNode.Name) Then
                                Me.Updated = True
                            End If

                            Return bIsEmpty
                        End If
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
                If MyBase.RemoveComponent(removeNode) Then
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

            Case "class"
                before = GetNode("package")
        End Select

        If before Is Nothing Then
            Return Me.Node.AppendChild(child)
        Else
            Return Me.Node.InsertBefore(child, before)
        End If
    End Function

    Protected Friend Overrides Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, _
                                                    Optional ByVal eRename As ENameReplacement = ENameReplacement.NewName, _
                                                    Optional ByVal bSetIdrefChildren As Boolean = False)
        Id = xmlRefNodeCounter.GetNewPackageId()
        Select Case eRename
            Case ENameReplacement.NewName
                Name = "New_" + Me.Id
            Case ENameReplacement.AddCopyName
                ' Name is set by caller
                Name = Name + "_" + Me.Id
        End Select

        Me.LoadChildrenList()

        For Each child As XmlComponent In Me.ChildrenList
            child.SetIdReference(xmlRefNodeCounter, ENameReplacement.NoReplacement, bSetIdrefChildren)
        Next
    End Sub
End Class

