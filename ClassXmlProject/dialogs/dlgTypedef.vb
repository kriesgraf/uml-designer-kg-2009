Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgTypedef
    Implements InterfFormDocument

    Private m_xmlView As XmlTypedefView

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
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "typedef")
    End Sub

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
        Debug.Print("Cancel_Button_Click")
        txtName.CausesValidation = False
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgMember_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With m_xmlView
            .LoadValues()
            .InitBindingRange(Me.cmbRange)
            .InitBindingBrief(Me.txtBrief)
            .InitBindingName(Me.txtName)
            cmdType.Text = .FullpathTypeDescription

            Me.Text = .Name
        End With
    End Sub

    Private Sub cmdType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdType.Click
        Debug.Print(cmdType.CausesValidation.ToString)
        Debug.Print("cmdType_Click")
        Dim fen As Form = m_xmlView.TypeVarDefinition.CreateDialogBox()
        fen.ShowDialog()
        If CType(fen.Tag, Boolean) = True Then
            m_xmlView.Updated = True
            cmdType.Text = m_xmlView.FullpathTypeDescription
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Debug.Print(btnDelete.CausesValidation.ToString)
        Debug.Print("btnDelete_Click")
        If MsgBox(m_xmlView.Name + " will be deleted, please confirm ?", cstMsgYesNoQuestion, "'Delete' command") _
            = MsgBoxResult.Yes Then
            If m_xmlView.RemoveMe() Then
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub txtName_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.Validated
        Debug.Print("txtName_Validated")
        Me.errorProvider.SetError(txtName, "")
    End Sub

    Private Sub txtName_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtName.Validating
        Debug.Print(Cancel_Button.CausesValidation.ToString)
        Debug.Print("txtName_Validating")
        e.Cancel = IsInvalidVariableName(txtName, Me.errorProvider)
    End Sub
End Class
