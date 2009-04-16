Imports System
Imports ClassXmlProject.XmlProjectTools
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class dlgConstructor
    Implements InterfFormDocument

    Private m_xmlView As XmlConstructorView

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
        txtBrief.Enabled = False
        txtComment.Enabled = False
        cmbRange.Enabled = False
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
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "constructor_view")
    End Sub

    Private Sub dlgConstructor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If XmlProjectTools.DEBUG_COMMANDS_ACTIVE Then
            mnuProperties.Visible = True
        End If

        With m_xmlView

            .LoadValues()

            .InitBindingBrief(txtBrief)
            .InitBindingComment(txtComment)
            .InitBindingRange(cmbRange)
            .InitBindingCheckInline(chkInline)
            .InitBindingCheckCopy(chkCopy)

            .LoadParamMembers(grdParams)

            Text = .Name + " constructor"
        End With
        mnuPaste.Enabled = XmlComponent.Clipboard.CanPaste
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

    Private Sub chkCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCopy.Click
        If chkCopy.Checked = True Then
            If m_xmlView.CheckCopyConstructor() = False Then
                If MsgBox("This operation is irreversible, would you want to continue ?" _
                          , cstMsgYesNoExclamation, "'Copy' constructor") _
                                = MsgBoxResult.No _
                Then
                    chkCopy.Checked = False
                Else
                    m_xmlView.ReplaceCopyConstructor()
                    grdParams.Binding.ResetBindings(True)
                End If
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox("This constructor will be deleted, please confirm ?", cstMsgYesNoQuestion, "'Delete' command") _
            = MsgBoxResult.Yes Then
            If m_xmlView.RemoveMe() Then
                Me.Tag = True
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
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

    Private Sub mnuDuplicate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDuplicate.Click
        grdParams.DuplicateSelectedItem()
    End Sub
End Class
