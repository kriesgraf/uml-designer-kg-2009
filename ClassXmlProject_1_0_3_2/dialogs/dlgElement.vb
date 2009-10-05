Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgElement
    Implements InterfFormDocument
    Private m_xmlView As XmlElementView

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.GenerationLanguage = value.GenerationLanguage
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
        Me.txtBrief.Enabled = False
        Me.txtName.Enabled = False
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

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView("element")
    End Sub

    Private Sub dlgElement_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With m_xmlView
            .LoadValues()
            .InitBindingLevel(Me.cmbLevel, Me.lblLevel)
            .InitBindingBrief(Me.txtBrief)
            .InitBindingName(Me.txtName)
            .InitBindingType(Me.cmbType)
            .InitBindingModifier(Me.chkModifier)
            .InitBindingSize(Me.cmbSize)

            Me.Text = .Name
        End With

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

    Private Sub txtName_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtName.Validating
        e.Cancel = IsInvalidVariableName(sender, Me.errorProvider)
    End Sub

    Private Sub cmbType_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub cmbType_Validating(ByVal sender As ComboBox, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmbType.Validating
        e.Cancel = IsInvalidType(sender, Me.errorProvider)
    End Sub
End Class
