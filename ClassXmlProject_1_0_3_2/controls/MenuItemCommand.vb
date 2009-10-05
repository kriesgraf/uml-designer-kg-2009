Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class MenuItemCommand
    Implements IEnumerator

    Public Const cstElement As String = "menuitem"
    Public Const cstProjectFolder As String = "{$ProjectFolder}"

    Public Class MenuItemNode
        Inherits XmlComponent

        Private m_RefIndex As Integer

        Property RefIndex() As Integer
            Get
                Return m_RefIndex
            End Get
            Set(ByVal value As Integer)
                m_RefIndex = value
            End Set
        End Property

        Property DiffTool() As String
            Get
                Return GetAttribute("difftool")
            End Get
            Set(ByVal value As String)
                SetAttribute("difftool", value)
            End Set
        End Property

        Property DiffArguments() As String
            Get
                Return GetAttribute("diffarg")
            End Get
            Set(ByVal value As String)
                SetAttribute("diffarg", value)
            End Set
        End Property

        Property Tool() As String
            Get
                Return GetAttribute("tool")
            End Get
            Set(ByVal value As String)
                SetAttribute("tool", value)
            End Set
        End Property

        Property ToolArguments() As String
            Get
                Return GetAttribute("args")
            End Get
            Set(ByVal value As String)
                SetAttribute("args", value)
            End Set
        End Property

        Property Stylesheet() As String
            Get
                Return GetAttribute("sheet")
            End Get
            Set(ByVal value As String)
                SetAttribute("sheet", value)
            End Set
        End Property

        Property XslParams() As String
            Get
                Return GetAttribute("params")
            End Get
            Set(ByVal value As String)
                SetAttribute("params", value)
            End Set
        End Property

        Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
            Try
                m_bCreateNodeNow = bCreateNodeNow
                ChangeReferences()
                Me.Name = "Menu item name"
                Me.Stylesheet = "<Left button to create/edit one, right to find it>"
                Me.XslParams = "-Param=value"
                Me.Tool = "C:\WINDOWS\system32\cmd.exe"
                Me.ToolArguments = "/k dir " + cstProjectFolder
                Me.DiffTool = My.Settings.DiffTool
                Me.DiffArguments = My.Settings.DiffToolArguments

            Catch ex As Exception
                Throw ex
            Finally
                m_bCreateNodeNow = False
            End Try
        End Sub

        Public Sub New(Optional ByVal node As XmlNode = Nothing)
            MyBase.New(node)
        End Sub
    End Class

    Private m_mnuParent As ToolStripMenuItem
    Private m_mnuCustomize As ToolStripMenuItem
    Private m_xmlDocument As XmlDocument
    Private m_xmlNodeList As ArrayList
    Private m_xmlNodeEnumerator As IEnumerator
    Private m_lstMenuItems As New ArrayList

    Private Const cstExternalToolsFile As String = "ExternalToolsFile.xml"

    Public Sub New(ByVal parent As ToolStripMenuItem, ByVal item As ToolStripMenuItem)
        m_mnuParent = parent
        m_mnuCustomize = item
        m_xmlNodeList = Nothing
        m_xmlDocument = Nothing
        m_xmlNodeEnumerator = Nothing
    End Sub

    Public Function LoadTools() As Boolean
        Try
            m_xmlDocument = New XmlDocument

            Dim filename As String = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath, cstExternalToolsFile)
            If My.Computer.FileSystem.FileExists(filename) Then
                m_xmlDocument.Load(filename)
                If GetAttributeValue(m_xmlDocument.DocumentElement, "version") <> cstExternalToolsFileVersion _
                Then
                    m_xmlDocument.LoadXml(CreateToolsFile())
                    m_xmlDocument.Save(filename)
                End If
            Else
                m_xmlDocument.LoadXml(CreateToolsFile())
                m_xmlDocument.Save(filename)
            End If

            RefreshList()

        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Public Function SaveTools() As Boolean
        Try
            Dim filename As String = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath, cstExternalToolsFile)
            m_xmlDocument.Save(filename)

        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Public Function CreateCommand() As MenuItemNode
        Dim xmlResult As MenuItemNode = Nothing
        Try
            xmlResult = New MenuItemNode
            xmlResult.Document = m_xmlDocument
            xmlResult.Node = xmlResult.CreateNode(MenuItemCommand.cstElement)

            xmlResult.SetDefaultValues(True)

            m_xmlDocument.DocumentElement.AppendChild(xmlResult.Node)
            xmlResult.RefIndex = m_xmlNodeList.Add(xmlResult)
            xmlResult.Name += xmlResult.RefIndex.ToString

        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Public Sub AddCommand(ByVal itemNode As MenuItemNode)
        Try
            m_xmlDocument.DocumentElement.AppendChild(itemNode.Node)
            itemNode.RefIndex = m_xmlNodeList.Add(itemNode)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub DeleteCommand(ByVal itemNode As MenuItemNode)
        Try
            itemNode.RemoveMe()
            m_xmlNodeList.Remove(itemNode)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function InsertCommand(ByVal node As Object) As ToolStripMenuItem
        Dim newItem As ToolStripMenuItem = Nothing
        Try
            Dim element As MenuItemNode = CType(node, MenuItemNode)
            Dim index As Integer = m_mnuParent.DropDownItems.IndexOf(m_mnuCustomize)

            newItem = New ToolStripMenuItem(element.Name)
            newItem.Tag = element.RefIndex

            m_mnuParent.DropDownItems.Insert(index, newItem)
            m_lstMenuItems.Add(newItem) ' To remove item later

        Catch ex As Exception
            Throw ex
        End Try
        Return newItem
    End Function

    Public Function Find(ByVal tag As Integer) As MenuItemNode
        If tag < m_xmlNodeList.Count Then
            Return m_xmlNodeList.Item(tag)
        End If
        Return Nothing
    End Function

    Public Sub RefreshList(ByVal list As ListBox, Optional ByVal bSelectLastItem As Boolean = False)
        Try
            list.DataSource = Nothing
            list.Items.Clear()

            'Second overload member
            RefreshList()

            If m_xmlNodeList.Count > 0 Then
                list.DisplayMember = "Name"
                list.DataSource = m_xmlNodeList
                If bSelectLastItem Then
                    list.SelectedIndex = list.Items.Count - 1
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub RefreshMenu()
        RemoveMenuItems()
        LoadTools()
    End Sub

    Public ReadOnly Property Current() As Object Implements System.Collections.IEnumerator.Current
        Get
            If m_xmlNodeEnumerator IsNot Nothing Then
                Return m_xmlNodeEnumerator.Current
            End If
            Return Nothing
        End Get
    End Property

    Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
        If m_xmlNodeEnumerator IsNot Nothing Then
            Return m_xmlNodeEnumerator.MoveNext()
        End If
        Return False
    End Function

    Public Sub Reset() Implements System.Collections.IEnumerator.Reset
        If m_xmlNodeEnumerator IsNot Nothing Then
            m_xmlNodeEnumerator.Reset()
        End If
    End Sub

    Private Sub RemoveMenuItems()
        For Each item As ToolStripMenuItem In m_lstMenuItems
            m_mnuParent.DropDownItems.Remove(item)
        Next
        m_lstMenuItems.Clear()
    End Sub

    Private Sub RefreshList()
        Try
            If m_xmlNodeList IsNot Nothing Then
                m_xmlNodeList.Clear()
            End If

            m_xmlNodeList = New ArrayList
            m_xmlNodeEnumerator = Nothing

            For Each node As XmlNode In m_xmlDocument.SelectNodes("//menuitem")
                Dim itemNode As MenuItemNode = New MenuItemNode(node)
                itemNode.RefIndex = m_xmlNodeList.Add(itemNode)
            Next
            m_xmlNodeEnumerator = m_xmlNodeList.GetEnumerator

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
