Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class dlgProperty
    Implements InterfFormDocument

    Private m_xmlView As XmlPropertyView
    Private m_bChangeCombo As Boolean = False

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

    Private Sub dlgMember_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With m_xmlView
            .LoadValues()
            .InitBindingName(txtName)
            .InitBindingOption(optTypeArray)
            .InitBindingBriefComment(txtBrief)
            .InitBindingRange(cmbRange)
            .InitBindingMember(cmbMember)
            .InitBindingGetAccess(cmbGetAccess)
            .InitBindingGetBy(cmbGetBy, lblGetBy)
            .InitBindingGetModifier(chkModifier)
            .InitBindingSetAccess(cmbSetAccess)
            .InitBindingSetby(cmbSetby, lblSetBy)
            .InitBindingBehaviour(cmbBehaviour, lblBehaviour)

            .UpdateOption(optTypeArray)

            cmdType.Text = .FullpathTypeDescription
            Me.Text = .Name
        End With
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            m_xmlView.UpdateValues()
            Me.Tag = True

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ChangeCombo(ByVal bGetChange As Boolean)
        If m_bChangeCombo = True Then Exit Sub

        If m_xmlView.Tag = ELanguage.Language_Vbasic _
        Then
            m_bChangeCombo = True

            If bGetChange Then
                If cmbGetAccess.Text <> "no" Then
                    If cmbSetAccess.Text <> "no" And cmbSetAccess.Text <> cmbGetAccess.Text Then
                        cmbSetAccess.Text = cmbGetAccess.Text
                    End If
                End If
            Else
                If cmbSetAccess.Text <> "no" Then
                    If cmbGetAccess.Text <> "no" And cmbSetAccess.Text <> cmbGetAccess.Text Then
                        cmbGetAccess.Text = cmbSetAccess.Text
                    End If
                End If
            End If

            m_bChangeCombo = False
        Else
            If cmbGetAccess.Text = "no" Then
                lblGetBy.Enabled = False
                cmbGetBy.Enabled = False
                chkModifier.Enabled = False
            Else
                cmbGetBy.Enabled = True
                chkModifier.Enabled = True
                lblGetBy.Enabled = True
            End If

            If cmbSetAccess.Text = "no" Then
                lblSetBy.Enabled = False
                cmbSetby.Enabled = False
            Else
                lblSetBy.Enabled = True
                cmbSetby.Enabled = True
            End If
        End If
    End Sub

    Private Sub cmbGetAccess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbGetAccess.TextChanged
        ChangeCombo(True)
    End Sub

    Private Sub cmbSetAccess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSetAccess.TextChanged
        ChangeCombo(False)
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "property")
    End Sub

    Private Sub cmdType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdType.Click
        Dim fen As Form = m_xmlView.TypeVarDefinition.CreateDialogBox()
        fen.ShowDialog()
        If CType(fen.Tag, Boolean) = True Then
            m_xmlView.Updated = True
            cmdType.Text = m_xmlView.FullpathTypeDescription
        End If
    End Sub

    Private Sub optTypeArray_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles optTypeArray.Click

        If m_xmlView.CheckOption() Then
            If MsgBox("This operation is irreversible, would you want to continue ?" _
                      , cstMsgYesNoExclamation) _
                            = MsgBoxResult.No _
            Then
                m_xmlView.CancelOption()
            Else
                m_xmlView.ConfirmOption()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox(m_xmlView.Name + " will be deleted, please confirm ?", cstMsgYesNoQuestion) _
            = MsgBoxResult.Yes Then
            If m_xmlView.RemoveMe() Then
                Me.Tag = True
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
End Class
