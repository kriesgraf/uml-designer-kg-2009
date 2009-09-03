Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.UmlNodesManager
Imports Microsoft.VisualBasic

Public Class dlgPackage
    Implements InterfFormDocument
    Implements InterfNodeCounter
    Implements InterfProgression

    Private m_xmlView As XmlPackageView
    Private m_strProjectFolder As String
    Private m_bInvalideCell As Boolean = False

    Public WriteOnly Property Log() As String Implements InterfProgression.Log
        Set(ByVal value As String)
        End Set
    End Property

    Public WriteOnly Property Maximum() As Integer Implements InterfProgression.Maximum
        Set(ByVal value As Integer)
            Me.strpProgressBar.Maximum = value
            Debug.Print("Maximum=" + value.ToString)
        End Set
    End Property

    Public WriteOnly Property Minimum() As Integer Implements InterfProgression.Minimum
        Set(ByVal value As Integer)
            Me.strpProgressBar.Minimum = value
            Me.strpProgressBar.Value = value
            Debug.Print("Minimum=" + value.ToString)
        End Set
    End Property

    Public WriteOnly Property ProgressBarVisible() As Boolean Implements InterfProgression.ProgressBarVisible
        Set(ByVal value As Boolean)
            Me.strpProgressBar.Visible = value
            Application.DoEvents()  ' To ose time to dispatch event
        End Set
    End Property

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

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "package")
    End Sub

    Public Sub Increment(ByVal value As Integer) Implements InterfProgression.Increment
        Me.strpProgressBar.Increment(value)
        Application.DoEvents()  ' To lose time to dispatch event
        Debug.Print("Step=" + Me.strpProgressBar.Value.ToString)
        System.Threading.Thread.Sleep(50)
    End Sub

#Region "Private methods"

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            txtFolder.Text = ComputeRelativePath(m_strProjectFolder, txtFolder.Text)
            m_xmlView.UpdateValues()
            Me.DialogResult = DialogResult.OK
            Me.Tag = True
            Me.Close()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        If m_bInvalideCell = False Then
            txtName.CausesValidation = False
            Me.Tag = m_xmlView.Updated
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub dlgPackage_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If XmlProjectTools.DEBUG_COMMANDS_ACTIVE Then
            mnuProperties.Visible = True
        End If

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

    Private Sub mnuAdd_Click(ByVal sender As ToolStripItem, ByVal e As System.EventArgs) _
        Handles mnuAddPackage.Click, mnuAddClass.Click, mnuAddImport.Click

        gridClasses.AddItem(CType(sender.Tag, String))
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        gridClasses.DeleteSelectedItems()
            End Sub

    Private Sub mnuEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEdit.Click
        gridClasses.EditCurrentItem()
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

    Private Sub mnuExportNodes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportNodes.Click
        Try
            m_xmlView.ExportNodes(Me, CType(gridClasses.SelectedItem, XmlComponent))

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuImportNodes_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
                Handles mnuImportNodes.Click, mnuUpdateNodes.Click
        Try
            m_xmlView.ImportNodes(Me, CType(sender.Tag, Boolean))

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub gridClasses_CellValidated(ByVal sender As XmlDataGridView, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridClasses.CellValidated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub gridClasses_CellValidating(ByVal sender As XmlDataGridView, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles gridClasses.CellValidating
        e.Cancel = IsInvalidVariableName(sender, e, Me.errorProvider)
        m_bInvalideCell = e.Cancel
    End Sub

    Private Sub gridClasses_RowValuesChanged(ByVal sender As Object) Handles gridClasses.RowValuesChanged
        ' TODO: for future use
    End Sub

    Private Sub btnFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFolder.Click
        Try
            Dim absolutePath As String
            If txtFolder.Text = "" Then
                absolutePath = m_strProjectFolder
            Else
                absolutePath = My.Computer.FileSystem.CombinePath(m_strProjectFolder, txtFolder.Text)
            End If

            FolderBrowserDialog1.Description = "Select the folder where you want to deposit generated code...."
            FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyDocuments
            FolderBrowserDialog1.SelectedPath = absolutePath
            If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
                txtFolder.Text = FolderBrowserDialog1.SelectedPath
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopy.Click
        mnuPaste.Enabled = gridClasses.CopySelectedItem()
    End Sub

    Private Sub mnuPaste_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuPaste.Click
        mnuPaste.Enabled = Not (gridClasses.PasteItem())
    End Sub

    Private Sub mnuDuplicate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDuplicate.Click
        gridClasses.DuplicateSelectedItem()
    End Sub

    Private Sub mnuDependencies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDependencies.Click
        Try
            m_xmlView.SearchDependencies(CType(gridClasses.SelectedItem, XmlComponent))

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuRedundancies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRedundancies.Click
        Try
            If m_xmlView.RemoveRedundant(CType(gridClasses.SelectedItem, XmlComponent)) Then
                gridClasses.Binding.ResetBindings(True)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub txtName_Validated(ByVal sender As TextBox, ByVal e As System.EventArgs) Handles txtName.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub txtName_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtName.Validating
        e.Cancel = IsInvalidPackageName(sender, Me.errorProvider, CType(m_xmlView.Tag, ELanguage), True)
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox(m_xmlView.Name + " will be deleted, please confirm ?", cstMsgYesNoQuestion, "'Delete' command") _
            = MsgBoxResult.Yes Then
            If m_xmlView.RemoveMe() Then
                Me.Tag = True
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub mnuImportReferences_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuImportReferences.Click
        Try
            m_xmlView.ImportReferences(Me)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuExportReferences_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportReferences.Click
        Try
            m_xmlView.ExportReferences(Me, gridClasses.SelectedItem)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuExchangeImports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExchangeImports.Click
        Try
            m_xmlView.ExchangeImports(gridClasses.SelectedItem)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub
#End Region

    Private Sub gridClasses_CellValidating(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles gridClasses.CellValidating

    End Sub

    Private Sub gridClasses_CellValidated(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridClasses.CellValidated

    End Sub
End Class
