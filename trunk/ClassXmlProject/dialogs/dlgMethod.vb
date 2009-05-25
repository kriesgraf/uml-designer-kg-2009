Imports System
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgMethod
    Implements InterfFormDocument
    Implements InterfFormClass

    Private m_xmlView As XmlMethodView
    Private m_eCurrentClassImplementation As EImplementation = EImplementation.Unknown
    Private m_bInvalideCell As Boolean = False

    'Private m_bInitProceed As Boolean = False

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Property ClassImpl() As EImplementation Implements InterfFormClass.ClassImpl
        Get
            Return m_eCurrentClassImplementation
        End Get
        Set(ByVal value As EImplementation)
            m_eCurrentClassImplementation = value
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
        If m_xmlView.OverridesMethod <> "" Then
            cmdType.Enabled = False
            grdParams.Enabled = False
        End If
    End Sub

    Private Sub dlgMethod_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If XmlProjectTools.DEBUG_COMMANDS_ACTIVE Then
            mnuProperties.Visible = True
        End If

        With m_xmlView
            .LoadValues()

            .ClassImpl = m_eCurrentClassImplementation
            .InitBindingName(txtName, lblName)
            .InitBindingOperator(txtOperator)
            .InitBindingCheckOperator(chkOperator)
            .InitBindingBrief(txtBrief)
            .InitBindingComment(txtComment)
            .InitBindingReturnComment(txtReturnComments)
            .InitBindingRange(cmbRange, lblRange)
            .InitBindingMember(cmbMember, lblMember)
            .InitBindingModifier(chkConst)
            .InitBindingCheckInline(chkInline)
            .InitBindingBehaviour(cmbBehaviour, lblBehaviour)
            ' Must be initialized at the end because updates others controls
            .InitBindingImplementation(cmbImplementation, lblImplementation)
            .LoadParamMembers(grdParams)

            chkOperator_CheckedChanged(sender, e)
            cmdType.Text = .ReturnValue.FullpathTypeDescription()

            If .OverridesMethod <> "" Then
                Text = .Name + " (Overrides)"
            Else
                Text = .Name
            End If
            .UpdateMenu(mnuAddException)
            DisableMemberAttributes()

        End With
        mnuPaste.Enabled = XmlComponent.Clipboard.CanPaste
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

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        If m_bInvalideCell = False Then
            txtName.CausesValidation = False
            Me.Tag = m_xmlView.Updated
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        'm_bInitProceed = True
        InitializeComponent()
        'm_bInitProceed = False

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "method")
    End Sub

    Private Sub btnType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdType.Click
        Dim fen As Form = m_xmlView.ReturnValue.CreateDialogBox()
        fen.ShowDialog()
        If CType(fen.Tag, Boolean) = True Then
            m_xmlView.Updated = True
        End If
        cmdType.Text = m_xmlView.ReturnValue.FullpathTypeDescription
    End Sub

    Private Sub mnuAddException_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAddException.Click
        Dim xmlcpnt As XmlComponent = New XmlComponent(m_xmlView.Node)
        xmlcpnt.Document = m_xmlView.Document

        Dim fen As Form = New dlgException
        CType(fen, InterfFormDocument).Document = xmlcpnt
        fen.ShowDialog()
        If CType(fen.Tag, Boolean) = True Then
            m_xmlView.Updated = True
            grdParams.Binding.ResetBindings(True)
        End If
    End Sub

    Private Sub mnuAddParam_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAddParam.Click
        grdParams.AddItem("param")
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        grdParams.DeleteSelectedItems()
    End Sub

    Private Sub mnuEditParam_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditParam.Click
        grdParams.EditCurrentItem()
    End Sub

    Private Sub mnuProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuProperties.Click
        dlgXmlNodeProperties.DisplayProperties(grdParams.SelectedItem)
    End Sub

    Private Sub chkOperator_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOperator.CheckedChanged
        'm_xmlView.UpdateOperatorDisplay(txtName, txtOperator, chkOperator, FlowLayoutPanel3, m_bInitProceed)
        'If m_bInitProceed = False Then
        '    m_xmlView.UpdateRange(cmbRange, chkOperator.Checked)
        '    m_xmlView.UpdateMember(cmbMember, chkOperator.Checked)
        'End If
    End Sub

    Private Sub dlgMethod_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        'If m_bInitProceed = False Then m_xmlView.UpdateOperatorDisplay(txtName, txtOperator, chkOperator, FlowLayoutPanel3, False)
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

    Private Sub grdParams_CellValidated(ByVal sender As XmlDataGridView, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdParams.CellValidated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub grdParams_CellValidating(ByVal sender As XmlDataGridView, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles grdParams.CellValidating
        e.Cancel = IsInvalidVariableName(sender, e, Me.errorProvider)
        m_bInvalideCell = e.Cancel
    End Sub

    Private Sub grdParams_RowValuesChanged(ByVal sender As Object) Handles grdParams.RowValuesChanged
        ' TODO: for future use
    End Sub

    Private Sub mnuCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopy.Click
        mnuPaste.Enabled = grdParams.CopySelectedItem()
    End Sub

    Private Sub mnuPaste_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuPaste.Click
        mnuPaste.Enabled = Not (grdParams.PasteItem())
    End Sub

    Private Sub mnuDuplicate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDuplicate.Click
        grdParams.DuplicateSelectedItem()
    End Sub

    Private Sub txtName_Validated(ByVal sender As TextBox, ByVal e As System.EventArgs) Handles txtName.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub txtName_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtName.Validating
        e.Cancel = IsInvalidVariableName(sender, Me.errorProvider, ErrorIconAlignment.BottomRight)
    End Sub

    Private Sub txtOperator_Validated(ByVal sender As TextBox, ByVal e As System.EventArgs) Handles txtOperator.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub txtOperator_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtOperator.Validating
        e.Cancel = IsInvalidOperator(sender, Me.errorProvider, CType(m_xmlView.Tag, ELanguage))
    End Sub
End Class

