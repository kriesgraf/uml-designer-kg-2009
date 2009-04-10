Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgMethod
    Implements InterfFormDocument
    Implements InterfFormClass


    Private m_xmlView As XmlMethodView
    Private m_eCurrentClassImplementation As EImplementation = EImplementation.Unknown

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
            btnType.Enabled = False
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
            btnType.Text = .ReturnValue.FullpathTypeDescription()

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
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        'm_bInitProceed = True
        InitializeComponent()
        'm_bInitProceed = False

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "method")
    End Sub

    Private Sub btnType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnType.Click
        Dim fen As Form = m_xmlView.ReturnValue.CreateDialogBox()
        fen.ShowDialog()
        If CType(fen.Tag, Boolean) = True Then
            m_xmlView.Updated = True
            btnType.Text = m_xmlView.ReturnValue.FullpathTypeDescription
        End If
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
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        If grdParams.DeleteSelectedItems() Then
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuEditParam_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditParam.Click
        If grdParams.EditCurrentItem() Then
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuProperties.Click
        dlgXmlNodeProperties.DisplayProperties(grdParams.SelectedItem)
        m_xmlView.Updated = True
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
        If MsgBox(m_xmlView.Name + " will be deleted, please confirm ?", cstMsgYesNoQuestion) _
            = MsgBoxResult.Yes Then
            If m_xmlView.RemoveMe() Then
                Me.Tag = True
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub grdParams_RowValuesChanged(ByVal sender As Object) Handles grdParams.RowValuesChanged
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopy.Click
        mnuPaste.Enabled = grdParams.CopySelectedItem()
    End Sub

    Private Sub mnuPaste_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuPaste.Click
        If grdParams.PasteItem() Then
            m_xmlView.Updated = True
        End If
        mnuPaste.Enabled = False
    End Sub

    Private Sub mnuDuplicate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDuplicate.Click
        If grdParams.DuplicateSelectedItem() Then
            m_xmlView.Updated = True
        End If
    End Sub
End Class

