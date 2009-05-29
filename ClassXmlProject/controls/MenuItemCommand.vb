Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class MenuItemCommand

    Private m_mnuParent As ToolStripMenuItem
    Private m_mnuCustomize As ToolStripMenuItem

    Public Sub New(ByVal parent As ToolStripMenuItem, ByVal item As ToolStripMenuItem)
        m_mnuParent = parent
        m_mnuCustomize = item
    End Sub

    Public Sub AddCommand(ByVal tag As Integer, ByVal label As String)
        Dim index As Integer = m_mnuParent.DropDownItems.IndexOf(m_mnuCustomize)
        Dim newItem As ToolStripMenuItem = New ToolStripMenuItem(label)
        newItem.Tag = tag
        AddHandler newItem.Click, AddressOf Me.Click
        m_mnuParent.DropDownItems.Insert(index, newItem)
    End Sub

    Private Sub AddMenuItem(ByVal node As XmlNode)
        Dim index As Integer = m_mnuParent.DropDownItems.IndexOf(m_mnuCustomize)
        Dim newItem As ToolStripMenuItem = New ToolStripMenuItem(GetAttributeValue(node, "label"))
        AddHandler newItem.Click, AddressOf Me.Click
        m_mnuParent.DropDownItems.Insert(index, newItem)
    End Sub

    Private Sub Click(ByVal sender As ToolStripMenuItem, ByVal e As EventArgs)
        Debug.Print(sender.Tag.ToString)
    End Sub
End Class
