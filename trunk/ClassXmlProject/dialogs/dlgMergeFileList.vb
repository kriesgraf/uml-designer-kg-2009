Imports System.Collections
Imports ClassXmlProject.UmlCodeGenerator

Public Class dlgMergeFileList

    Private m_strFolder As String
    Private m_lstFilesToMerge As ArrayList
    Private m_strExternalMerger As String
    Private m_strArguments As String

    Public WriteOnly Property ProjectFolder() As String
        Set(ByVal value As String)
            m_strFolder = value
        End Set
    End Property

    Public WriteOnly Property FileList() As ArrayList
        Set(ByVal value As ArrayList)
            m_lstFilesToMerge = value
        End Set
    End Property

    Public WriteOnly Property ExternalMerger() As String
        Set(ByVal value As String)
            m_strExternalMerger = value
        End Set
    End Property

    Public WriteOnly Property Arguments() As String
        Set(ByVal value As String)
            m_strArguments = value
        End Set
    End Property

    Private Sub dlgMergeFileList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lsbFileList.DisplayMember = "Filename"
        Me.lsbFileList.DataSource = m_lstFilesToMerge
        dlgMergeFileList_Resize(sender, e)
    End Sub

    Private Sub dlgMergeFileList_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Me.lblFolder.Text = XmlProjectTools.CompactPath(Me.lblFolder, m_strFolder)
    End Sub

    Private Sub lsbFileList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsbFileList.DoubleClick
        Dim fileInfo As CodeInfo = CType(lsbFileList.SelectedItem, CodeInfo)
        CompareAndMergeFiles(m_strExternalMerger, m_strArguments, fileInfo.strTempFile, fileInfo.strReleaseFile)
    End Sub
End Class