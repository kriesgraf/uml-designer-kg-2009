Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class MenuItemCommand
    Implements IEnumerator

    Public Const cstElement As String = "menuitem"

    Public Class MenuItemNode
        Inherits XmlComponent

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



        Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
            Try
                m_bCreateNodeNow = bCreateNodeNow
                ChangeReferences()
                Me.Name = "Menu item name"
                Me.Stylesheet = "<Please select a XSL style sheet>"
                Me.Tool = ""
                Me.ToolArguments = ""
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
    End Function

    Public Function CreateCommand() As MenuItemNode
        Dim xmlResult As MenuItemNode = Nothing
        Try
            xmlResult = New MenuItemNode
            xmlResult.Document = m_xmlDocument
            xmlResult.Node = xmlResult.CreateNode(MenuItemCommand.cstElement)

            xmlResult.SetDefaultValues(True)

            m_xmlDocument.DocumentElement.AppendChild(xmlResult.Node)
            xmlResult.Tag = m_xmlNodeList.Add(xmlResult)
            xmlResult.Name += xmlResult.Tag.ToString

        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Public Sub AddCommand(ByVal itemNode As MenuItemNode)
        Try
            m_xmlDocument.DocumentElement.AppendChild(itemNode.Node)
            itemNode.Tag = m_xmlNodeList.Add(itemNode)

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
            newItem.Tag = element.Tag
            m_mnuParent.DropDownItems.Insert(index, newItem)

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

    Public Sub RefreshList(ByVal list As ListBox)
        Try
            list.DataSource = Nothing
            list.Items.Clear()

            'Second overload member
            RefreshList()

            If m_xmlNodeList.Count > 0 Then
                list.DisplayMember = "Name"
                list.DataSource = m_xmlNodeList
            End If
        Catch ex As Exception
            Throw ex
        End Try
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
                itemNode.Tag = m_xmlNodeList.Add(itemNode)
            Next
            m_xmlNodeEnumerator = m_xmlNodeList.GetEnumerator

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub RefreshMenu()
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
End Class
