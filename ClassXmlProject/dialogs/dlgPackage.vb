Imports System
Imports System.Windows.Forms

Public Class dlgPackage
    Implements InterfFormDocument
    Implements InterfNodeCounter

    Private m_xmlView As XmlPackageView
    Private m_strProjectFolder As String

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlView.NodeCounter = value
        End Set
    End Property

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque apr�s l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "package")
    End Sub

#Region "Private methods"

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        m_xmlView.UpdateValues()
        Me.Tag = True
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgPackage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            With m_xmlView
                .LoadValues()

                m_strProjectFolder = .ProjectFolder

                ' Bind controls and document values
                .InitBindingName(txtName)
                .InitBindingCheckFolder(chkFolder)
                .InitBindingTextFolder(txtFolder)
                .InitBindingBriefComment(txtBrief)
                .InitBindingComment(txtDetails)

                ' Load document values into controls
                .LoadClasses(gridClasses)

                Me.Text = .Name
            End With

            chkFolder_CheckedChanged(sender, e)
            mnuPaste.Enabled = XmlComponent.Clipboard.CanPaste

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles mnuAddPackage.Click, mnuAddClass.Click, mnuAddImport.Click
        gridClasses.AddItem(CType(sender, ToolStripItem).Tag)
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        If gridClasses.DeleteSelectedItems() Then
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEdit.Click
        If gridClasses.EditCurrentItem() Then
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub chkFolder_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFolder.CheckedChanged
        If chkFolder.Checked Then
            txtFolder.Enabled = True
            btnFolder.Enabled = True
        Else
            txtFolder.Text = ""
            txtFolder.Enabled = False
            btnFolder.Enabled = False
        End If
    End Sub

    Private Sub mnuImportNodes_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
                Handles mnuImportNodes.Click, mnuUpdateNodes.Click
        Try
            m_xmlView.ImportNodes(CType(sender.Tag, Boolean))

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub gridClasses_RowValuesChanged(ByVal sender As Object) Handles gridClasses.RowValuesChanged
        m_xmlView.Updated = True
    End Sub

    Private Sub btnFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFolder.Click
        Try
            Dim dlg As New FolderBrowserDialog
            Dim absolutePath As String
            If txtFolder.Text = "" Then
                absolutePath = m_strProjectFolder
            Else
                absolutePath = My.Computer.FileSystem.CombinePath(m_strProjectFolder, txtFolder.Text)
            End If
            dlg.RootFolder = Environment.SpecialFolder.Desktop
            dlg.SelectedPath = absolutePath
            If dlg.ShowDialog() = DialogResult.OK Then
                txtFolder.Text = UmlNodesManager.ComputeRelativePath(m_strProjectFolder, dlg.SelectedPath)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopy.Click
        mnuPaste.Enabled = gridClasses.CopySelectedItem()
    End Sub

    Private Sub mnuPaste_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuPaste.Click
        If gridClasses.PasteItem() Then
            m_xmlView.Updated = True
        End If
        mnuPaste.Enabled = False
    End Sub

    Private Sub mnuDuplicate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDuplicate.Click
        If gridClasses.DuplicateSelectedItem() Then
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuDependencies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDependencies.Click
        If gridClasses.SelectedItem() IsNot Nothing Then
            Dim fen As New dlgDependencies
            fen.Document = gridClasses.SelectedItem()
            fen.ShowDialog()
            If CType(fen.Tag, Boolean) = True Then
                m_xmlView.Updated = True
            End If
        End If
    End Sub
#End Region
End Class