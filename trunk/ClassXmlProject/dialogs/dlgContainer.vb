Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgContainer
    Implements InterfFormDocument
    Implements InterfFormClass

    Private m_xmlView As XmlContainerView
    Private m_eCurrentClassImplementation As EImplementation

    Public Property ClassImpl() As EImplementation Implements InterfFormClass.ClassImpl
        Get
            Return m_eCurrentClassImplementation
        End Get
        Set(ByVal value As EImplementation)
            m_eCurrentClassImplementation = value
        End Set
    End Property

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
        Me.txtBrief.Enabled = False
        Me.txtName.Enabled = False
        Me.cmbRange.Enabled = False
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
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
        txtName.CausesValidation = False
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgContainer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            With m_xmlView
                .LoadValues()
                .InitBindingName(Me.txtName)
                .InitBindingBrief(Me.txtBrief)
                .InitBindingRange(Me.cmbRange)
                .InitBindingType(Me.cmbType)
                .InitBindingLevel(Me.cmbLevel, Me.lblLevel)
                .InitBindingModifier(Me.chkModifier)
                .InitBindingIterator(Me.chkIterator)
                .InitBindingCheckIndex(Me.chkIndex)
                .InitBindingContainer(Me.cmbContainer)
                .InitBindingComboIndex(Me.cmbIndex)
                .InitBindingIndexLevel(Me.cmbIndexLevel, Me.lblIndexLevel)

                Me.Text = .Name
            End With

            ChangeComboIndex()
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "container_view")
    End Sub

    Private Sub chkIndex_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIndex.Click
        Try
            ChangeComboIndex()
            m_xmlView.RefreshComboContainer(chkIndex.Checked)
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub ChangeComboIndex()
        If chkIndex.Checked Then
            If cmbIndex.SelectedIndex = -1 Then
                If cmbIndex.Text = "" Then cmbIndex.SelectedIndex = 0
            End If
            cmbIndex.Enabled = True
            cmbIndexLevel.Enabled = True
            flpIndex.Enabled = True
            lblIndexLevel.Enabled = True
        Else
            cmbIndex.Enabled = False
            cmbIndexLevel.Enabled = False
            flpIndex.Enabled = False
            lblIndexLevel.Enabled = False
        End If
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

    Private Sub txtName_Validated(ByVal sender As TextBox, ByVal e As System.EventArgs) Handles txtName.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub combo_Validated(ByVal sender As ComboBox, ByVal e As System.EventArgs) _
            Handles cmbContainer.Validated, cmbIndex.Validated, cmbType.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub txtName_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtName.Validating
        e.Cancel = IsInvalidVariableName(sender, Me.errorProvider)
    End Sub

    Private Sub combo_Validating(ByVal sender As ComboBox, ByVal e As System.ComponentModel.CancelEventArgs) _
            Handles cmbContainer.Validating, cmbIndex.Validating, cmbType.Validating
        e.Cancel = IsInvalidType(sender, Me.errorProvider)
    End Sub
End Class
