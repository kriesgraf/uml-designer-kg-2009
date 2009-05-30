Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms

Public Class MenuItemCommand
    Implements IEnumerator

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

        Property DiffArgument() As String
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

        Property ToolArgument() As String
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

        Public Overrides Function CompareComponent(ByVal x As Object, ByVal y As Object) As Integer
            Return MyBase.CompareComponent(x, y)
        End Function

        Public Sub New(ByVal node As XmlNode)
            MyBase.New(node)
        End Sub
    End Class

    Private m_mnuParent As ToolStripMenuItem
    Private m_mnuCustomize As ToolStripMenuItem
    Private m_xmlDocument As New XmlDocument
    Private m_xmlNodeList As ArrayList
    Private m_xmlNodeEnumerator As IEnumerator

    Private Const cstExternalToolsFile As String = "ExternalToolsFile.xml"

    Public Sub New(ByVal parent As ToolStripMenuItem, ByVal item As ToolStripMenuItem)
        m_mnuParent = parent
        m_mnuCustomize = item
        m_xmlNodeList = New ArrayList
        m_xmlNodeEnumerator = Nothing
    End Sub

    Public Function LoadTools() As Boolean
        Dim filename As String = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath, cstExternalToolsFile)
        If My.Computer.FileSystem.FileExists(filename) Then
            m_xmlDocument.Load(filename)
            For Each node As XmlNode In m_xmlDocument.SelectNodes("//menuitem")
                Dim itemNode As MenuItemNode = New MenuItemNode(node)
                itemNode.Tag = m_xmlNodeList.Add(itemNode)
            Next
            m_xmlNodeEnumerator = m_xmlNodeList.GetEnumerator
        Else
            Throw New Exception("no yet implemented")
        End If
    End Function

    Public Function AddCommand(ByVal node As Object) As ToolStripMenuItem
        Dim element As MenuItemNode = CType(node, MenuItemNode)
        Dim index As Integer = m_mnuParent.DropDownItems.IndexOf(m_mnuCustomize)
        Dim newItem As ToolStripMenuItem = New ToolStripMenuItem(element.Name)
        newItem.Tag = element.Tag
        m_mnuParent.DropDownItems.Insert(index, newItem)
        Return newItem
    End Function

    Public Function Find(ByVal tag As Integer) As MenuItemNode
        If tag < m_xmlNodeList.Count Then
            Return m_xmlNodeList.Item(tag)
        End If
        Return Nothing
    End Function

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
