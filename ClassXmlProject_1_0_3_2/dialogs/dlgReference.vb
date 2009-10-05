Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgReference
    Implements InterfFormDocument

    Private m_xmlView As XmlReferenceView

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.GenerationLanguage = value.GenerationLanguage
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

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
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "reference")
    End Sub

    Private Sub dlgReference_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With m_xmlView

            .LoadValues()

            .InitBindingName(txtName)
            .InitBindingParentClass(txtParentClass)
            .InitBindingPackage(txtPackage)
            .InitBindingContainer(cmbContainer)
            .InitBindingType(cmbType)

            Me.Text = .Title
        End With

    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged

        lblParentClass.Enabled = False
        txtParentClass.Enabled = False

        Select Case CType(cmbType.SelectedItem, String)
            Case "exception"
                cmbContainer.SelectedIndex = 0
                cmbContainer.Enabled = False

            Case "typedef"
                lblParentClass.Enabled = True
                txtParentClass.Enabled = True
                cmbContainer.SelectedIndex = 0
                cmbContainer.Enabled = False

            Case "enumeration"
                If MsgBox("This operation is irreversible, would you want to continue ?" _
                          , cstMsgYesNoExclamation, "Array conversion") _
                                = MsgBoxResult.No _
                Then
                    m_xmlView.CancelEnum(cmbType)
                Else
                    m_xmlView.ConfirmEnum()
                    Me.Hide()
                    ' To confirm update
                    Me.Tag = True
                    ' And open new one
                    Dim fen As Form = m_xmlView.CreateForm(m_xmlView)
                    ' Change dialog result close automatically previous window
                    Me.DialogResult = fen.ShowDialog(Me)
                End If

            Case Else
                cmbContainer.Enabled = True
        End Select
    End Sub

    Private Sub txtName_Validated(ByVal sender As TextBox, ByVal e As System.EventArgs) _
            Handles txtName.Validated, txtPackage.Validated, txtParentClass.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub txtName_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtName.Validating
        e.Cancel = IsInvalidVariableName(sender, Me.errorProvider)
    End Sub

    Private Sub txtParentClass_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtParentClass.Validating
        If sender.Text.Length > 0 Then
            e.Cancel = IsInvalidVariableName(sender, Me.errorProvider)
        Else
            e.Cancel = False
        End If
    End Sub

    Private Sub txtPackage_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) _
            Handles txtPackage.Validating
        e.Cancel = IsInvalidPackageName(sender, Me.errorProvider, m_xmlView.GenerationLanguage, True)
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
End Class
