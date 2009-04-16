Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgStructure
    Implements InterfFormDocument

    Private m_xmlView As XmlStructureView
    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
        txtName.Enabled = False
        txtBrief.Enabled = False
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

    Private Sub dlgStructure_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If DEBUG_COMMANDS_ACTIVE Then
            mnuProperties.Visible = True
        End If

        With m_xmlView
            .LoadValues()
            .InitBindingRange(cmbRange)
            .InitBindingBrief(txtBrief)
            .InitBindingName(txtName)
            .InitBindingUnion(chkUnion)
            .LoadElementMembers(grdElements)

            Me.Text = .Name
        End With
        mnuPaste.Enabled = XmlComponent.Clipboard.CanPaste
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "structure_view")
    End Sub

    Private Sub mnuAddElement_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAddElement.Click
        grdElements.AddItem()
    End Sub

    Private Sub mnuDeleteElement_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDeleteElement.Click
        grdElements.DeleteSelectedItems(1)
            End Sub

    Private Sub mnuEditElement_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEditElement.Click
        grdElements.EditCurrentItem()
    End Sub

    Private Sub mnuProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuProperties.Click
        dlgXmlNodeProperties.DisplayProperties(grdElements.SelectedItem)
        m_xmlView.Updated = True
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

    Private Sub grdElements_RowValuesChanged(ByVal sender As Object) Handles grdElements.RowValuesChanged
        ' TODO: for future use
    End Sub

    Private Sub mnuCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopy.Click
        mnuPaste.Enabled = grdElements.CopySelectedItem()
    End Sub

    Private Sub mnuPaste_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuPaste.Click
        mnuPaste.Enabled = Not (grdElements.PasteItem())
    End Sub

    Private Sub mnuDuplicate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDuplicate.Click
        grdElements.DuplicateSelectedItem()
            End Sub
End Class
