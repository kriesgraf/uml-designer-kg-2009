Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class dlgImport
    Implements InterfFormDocument
    Implements InterfNodeCounter
    Implements InterfProgression

#Region "Class declarations"

    Private m_xmlView As XmlImportView

#End Region

#Region "Properties"

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

    Public WriteOnly Property Maximum() As Integer Implements InterfProgression.Maximum
        Set(ByVal value As Integer)
            Me.pgbLoadImport.Maximum = value
        End Set
    End Property

    Public WriteOnly Property Minimum() As Integer Implements InterfProgression.Minimum
        Set(ByVal value As Integer)
            Me.pgbLoadImport.Minimum = value
            Me.pgbLoadImport.Value = value
        End Set
    End Property

    Public WriteOnly Property ProgressBarVisible() As Boolean Implements InterfProgression.ProgressBarVisible
        Set(ByVal value As Boolean)
            Me.pgbLoadImport.Visible = value
        End Set
    End Property

#End Region

#Region "Public methods"

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "import")
    End Sub

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

    Public Sub Increment(ByVal value As Integer) Implements InterfProgression.Increment
        Me.pgbLoadImport.Increment(value)
        Application.DoEvents()  ' To ose time to dispatch event
        System.Threading.Thread.Sleep(50)
    End Sub

#End Region

#Region "Private methods"

    Private Sub dlgImport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If XmlProjectTools.DEBUG_COMMANDS_ACTIVE Then
            mnuImportParameters.Visible = True
        End If
        Try
            With m_xmlView
                .LoadValues()

                .InitBindingName(txtName)
                .InitBindingFullpathName(txtParam)
                .InitBindingVisibility(cmbVisibility)
                .InitBindingInterface(chkInterface)
                .InitBindingListReferences(lsbReferences)
                .InitBindingBodyInterface(txtInterface)

                Me.Text = .Name

                tblDeclaration.ColumnStyles.Item(0).SizeType = SizeType.Percent
                tblDeclaration.ColumnStyles.Item(1).SizeType = SizeType.Percent

                If .ClassImport() Then
                    txtInterface.Visible = True
                    txtInterface.Enabled = True
                    lsbReferences.Visible = False
                    lsbReferences.Enabled = False
                    chkInterface.Visible = True
                    chkInterface.Enabled = True
                    lblVisibility.Visible = False
                    cmbVisibility.Enabled = False
                    cmbVisibility.Visible = False
                    tblDeclaration.ColumnStyles.Item(0).Width = 0
                    tblDeclaration.ColumnStyles.Item(1).Width = 100
                Else
                    txtInterface.Visible = False
                    txtInterface.Enabled = False
                    lsbReferences.Visible = True
                    lsbReferences.Enabled = True
                    chkInterface.Visible = False
                    chkInterface.Enabled = False
                    lblVisibility.Visible = True
                    cmbVisibility.Enabled = True
                    cmbVisibility.Visible = True
                    tblDeclaration.ColumnStyles.Item(0).Width = 100
                    tblDeclaration.ColumnStyles.Item(1).Width = 0
                End If

                mnuPaste.Enabled = XmlComponent.Clipboard.CanPaste
                chkInterface_Click(sender, e)
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            m_xmlView.UpdateValues()
            Me.Tag = True

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        txtParam.CausesValidation = False
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub chkInterface_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInterface.Click
        txtInterface.Enabled = chkInterface.Checked
        txtParam.Enabled = (chkInterface.Checked = False)
        lblParam.Enabled = (chkInterface.Checked = False)
    End Sub

    Private Sub lsbReferences_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lsbReferences.DoubleClick
        If lsbReferences.SelectedItem IsNot Nothing Then
            m_xmlView.Edit(lsbReferences)
        End If
    End Sub

    Private Sub AddNode_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
    Handles NewReference.Click, NewInterface.Click

        m_xmlView.AddNew(lsbReferences, CType(sender.Tag, String))
    End Sub

    Private Sub DeleteReference_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteReference.Click
        m_xmlView.Delete(lsbReferences)
    End Sub

    Private Sub EditReference_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditReference.Click
        m_xmlView.Edit(lsbReferences)
    End Sub

    Private Sub RemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveAll.Click
        m_xmlView.RemoveAllReferences(lsbReferences)
    End Sub

    Private Sub AddReferences_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
                Handles mnuReplace.Click, mnuMerge.Click, mnuConfirm.Click

        m_xmlView.AddReferences(Me, CType(sender.Tag, XmlImportSpec.EImportMode), lsbReferences)
    End Sub

    Private Sub RemoveRedundant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveRedundant.Click
        Dim bImportRemoved As Boolean = m_xmlView.RemoveRedundantReference(lsbReferences)
        If bImportRemoved Then
            Me.Tag = True
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub mnuRefDependencies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRefDependencies.Click
        If m_xmlView.SearchDependencies(CType(lsbReferences.SelectedItem, XmlComponent)) Then
            m_xmlView.InitBindingListReferences(lsbReferences, True)
        End If
    End Sub

    Private Sub DuplicateReference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DuplicateReference.Click
        m_xmlView.DuplicateReference(lsbReferences)
    End Sub

    Private Sub mnuCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopy.Click
        If lsbReferences.SelectedItem IsNot Nothing Then
            XmlComponent.Clipboard.SetData(CType(lsbReferences.SelectedItem, XmlComponent))
        End If
    End Sub

    Private Sub mnuPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPaste.Click
        mnuPaste.Enabled = Not (m_xmlView.PasteReference(lsbReferences))
    End Sub

    Private Sub mnuImportParameters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuImportParameters.Click
        dlgXmlNodeProperties.DisplayProperties(lsbReferences.SelectedItem)
        m_xmlView.Updated = True
    End Sub

    Private Sub txtParam_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtParam.Validated
        m_xmlView.InitBindingListReferences(lsbReferences, True)
    End Sub

    Private Sub mnuRenamePackage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRenamePackage.Click
        m_xmlView.RenamePackage(lsbReferences)
    End Sub

    Private Sub txtPackage_Validated(ByVal sender As TextBox, ByVal e As System.EventArgs) _
            Handles txtParam.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub txtPackage_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) _
            Handles txtParam.Validating
        e.Cancel = IsInvalidPackageName(sender, Me.errorProvider, m_xmlView.Tag)
    End Sub

    Private Sub mnuMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMoveUp.Click
        m_xmlView.MoveUpReference(lsbReferences)
    End Sub

    Private Sub mnuConvertToReference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuConvertToReference.Click
        m_xmlView.ConvertComponent(lsbReferences.SelectedItem)
        m_xmlView.InitBindingListReferences(lsbReferences, True)
    End Sub
#End Region
End Class
