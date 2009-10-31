Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgProperty
    Implements InterfFormDocument
    Implements InterfFormClass

    Private m_xmlView As XmlPropertyView
    Private m_eCurrentClassImplementation As EImplementation = EImplementation.Unknown
    Private m_bChangeCombo As Boolean = False

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
            m_xmlView.GenerationLanguage = value.GenerationLanguage
        End Set
    End Property

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "property")
    End Sub

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
        ' Nothing
    End Sub

    Private Sub dlgMember_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With m_xmlView
            .LoadValues()
            .ClassImpl = m_eCurrentClassImplementation
            .InitBindingName(txtName, lblName)
            '  Order is required, because some initialization are used by following methods
            .InitBindingRange(cmbRange)
            .InitBindingOverridable(chkOverridable)
            .InitBindingOption(optTypeArray)
            .InitBindingBriefComment(txtBrief)
            .InitBindingMember(chkMember)
            .InitBindingAttribute(chkAttribute)
            .InitBindingGetAccess(cmbGetAccess, lblGetAccess)
            .InitBindingGetBy(cmbGetBy, lblGetBy)
            .InitBindingGetModifier(chkModifier)
            .InitBindingGetInline(chkGetInline)
            .InitBindingSetAccess(cmbSetAccess, lblSetAccess)
            .InitBindingSetby(cmbSetby, lblSetBy)
            .InitBindingSetInline(chkSetInline)
            .InitBindingBehaviour(cmbBehaviour, lblBehaviour)

            .UpdateOption(optTypeArray)
            .InitComplete()

            DisableMemberAttributes()
            cmdType.Text = .FullpathTypeDescription

            If .OverridesProperty <> "" Then
                Me.Text = .Name + " (Overrides)"
            Else
                Me.Text = .Name
            End If
        End With
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            If m_xmlView.UpdateValues(cmbGetAccess, cmbSetAccess, chkAttribute, chkSetInline) _
            Then
                Me.Tag = True
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ChangeCombo(ByVal bGetChange As Boolean)
        If m_bChangeCombo = True Then Exit Sub

        If m_xmlView.GenerationLanguage = ELanguage.Language_Vbasic _
        Then
            m_bChangeCombo = True

            If bGetChange Then
                If CType(cmbGetAccess.SelectedItem, String) <> "no" Then
                    If CType(cmbSetAccess.SelectedItem, String) <> "no" And CType(cmbSetAccess.SelectedItem, String) <> CType(cmbGetAccess.SelectedItem, String) Then
                        cmbSetAccess.SelectedItem = CType(cmbGetAccess.SelectedItem, String)
                    End If
                End If
            Else
                If CType(cmbSetAccess.SelectedItem, String) <> "no" Then
                    If CType(cmbGetAccess.SelectedItem, String) <> "no" And CType(cmbSetAccess.SelectedItem, String) <> CType(cmbGetAccess.SelectedItem, String) Then
                        cmbGetAccess.SelectedItem = CType(cmbSetAccess.SelectedItem, String)
                    End If
                End If
            End If

            m_bChangeCombo = False
        Else
            If CType(cmbGetAccess.SelectedItem, String) = "no" _
            Then
                lblGetBy.Enabled = False
                cmbGetBy.Enabled = False
                chkModifier.Enabled = False

            ElseIf m_xmlView.OverridesProperty <> "" _
            Then
                cmbGetBy.Enabled = False
                chkModifier.Enabled = False
                lblGetBy.Enabled = False
            Else
                cmbGetBy.Enabled = True
                chkModifier.Enabled = True
                lblGetBy.Enabled = True
            End If

            If CType(cmbSetAccess.SelectedItem, String) = "no" _
            Then
                lblSetBy.Enabled = False
                cmbSetby.Enabled = False

            ElseIf m_xmlView.OverridesProperty <> "" _
            Then
                lblSetBy.Enabled = False
                cmbSetby.Enabled = False
            Else
                lblSetBy.Enabled = True
                cmbSetby.Enabled = True
            End If
        End If
    End Sub

    Private Sub cmbGetAccess_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbGetAccess.SelectedIndexChanged
        ChangeCombo(True)
        m_xmlView.HandlingAttribute()
    End Sub

    Private Sub cmbSetAccess_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSetAccess.SelectedIndexChanged
        ChangeCombo(False)
        m_xmlView.HandlingAttribute()
    End Sub

    Private Sub cmdType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdType.Click
        Dim fen As Form = m_xmlView.TypeVarDefinition.CreateDialogBox(Me.chkMember.Checked)
        fen.ShowDialog()
        If CType(fen.Tag, Boolean) = True Then
            m_xmlView.Updated = True
        End If
        cmdType.Text = m_xmlView.FullpathTypeDescription
    End Sub

    Private Sub optTypeArray_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles optTypeArray.Click

        If m_xmlView.CheckOption() Then
            If MsgBox("This operation is irreversible, would you want to continue ?" _
                      , cstMsgYesNoExclamation, "Array conversion") _
                            = MsgBoxResult.No _
            Then
                m_xmlView.CancelOption()
            Else
                m_xmlView.ConfirmOption()
                ' Hide this current window
                Me.Hide()
                ' To confirm update
                Me.Tag = True
                ' And open new one
                Dim fen As Form = m_xmlView.CreateForm(m_xmlView)
                ' Change dialog result close automatically previous window
                Me.DialogResult = fen.ShowDialog(Me)
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
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

    Private Sub cmbSetAccess_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSetAccess.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub cmbSetAccess_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmbSetAccess.Validating
        e.Cancel = False
        If m_xmlView.TypeVarDefinition.Modifier Then
            If cmbSetAccess.SelectedItem.ToString <> "no" Then
                ' Set the ErrorProvider error with the text to display. 
                Me.errorProvider.SetIconPadding(cmbSetAccess, 0)
                Me.errorProvider.SetIconAlignment(cmbSetAccess, ErrorIconAlignment.TopLeft)
                Me.errorProvider.SetError(cmbSetAccess, "'Constant' property can't have setter!")

                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub chkMember_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMember.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub chkMember_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles chkMember.Validating
        e.Cancel = False
        If m_xmlView.TypeVarDefinition.Modifier Then
            If m_xmlView.GenerationLanguage = ELanguage.Language_Vbasic And chkMember.Checked Then
                ' Set the ErrorProvider error with the text to display. 
                Me.errorProvider.SetIconPadding(cmbSetAccess, 0)
                Me.errorProvider.SetIconAlignment(chkMember, ErrorIconAlignment.TopLeft)
                Me.errorProvider.SetError(chkMember, "'Constant' property can't be 'Shared'!")

                e.Cancel = True
            End If
        End If
    End Sub
End Class
