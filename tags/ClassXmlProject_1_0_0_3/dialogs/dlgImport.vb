Imports System
Imports System.Windows.Forms

Public Class dlgImport
    Implements InterfFormDocument
    Implements InterfNodeCounter

    Private m_xmlView As XmlImportView

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque apr�s l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "import")
    End Sub

    Private Sub dlgImport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            With m_xmlView
                .LoadValues()

                .InitBindingName(txtName)
                .InitBindingFullpathName(txtParam)
                .InitBindingVisibility(cmbVisibility)
                .InitBindingInterface(chkInterface)
                .InitBindingReferences(lsbReferences)
                .InitBindingBodyInterface(txtInterface)

                Me.Text = .Name

                tblDeclaration.ColumnStyles.Item(0).SizeType = SizeType.Percent
                tblDeclaration.ColumnStyles.Item(1).SizeType = SizeType.Percent

                If .ClassImport() Then
                    txtInterface.Visible = True
                    txtInterface.Enabled = True
                    lsbReferences.Visible = False
                    lsbReferences.Enabled = False
                    chkInterface.Visible = True
                    chkInterface.Enabled = True
                    lblVisibility.Visible = False
                    cmbVisibility.Enabled = False
                    cmbVisibility.Visible = False
                    tblDeclaration.ColumnStyles.Item(0).Width = 0
                    tblDeclaration.ColumnStyles.Item(1).Width = 100
                Else
                    txtInterface.Visible = False
                    txtInterface.Enabled = False
                    lsbReferences.Visible = True
                    lsbReferences.Enabled = True
                    chkInterface.Visible = False
                    chkInterface.Enabled = False
                    lblVisibility.Visible = True
                    cmbVisibility.Enabled = True
                    cmbVisibility.Visible = True
                    tblDeclaration.ColumnStyles.Item(0).Width = 100
                    tblDeclaration.ColumnStyles.Item(1).Width = 0
                End If

                mnuPaste.Enabled = XmlComponent.Clipboard.CanPaste
                chkInterface_Click(sender, e)
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
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

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub chkInterface_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInterface.Click
        txtInterface.Enabled = chkInterface.Checked
        txtParam.Enabled = (chkInterface.Checked = False)
        lblParam.Enabled = (chkInterface.Checked = False)
    End Sub

    Private Sub lsbReferences_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lsbReferences.DoubleClick
        If lsbReferences.SelectedItem IsNot Nothing Then
            m_xmlView.Edit(lsbReferences)
        End If
    End Sub

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlView.NodeCounter = value
        End Set
    End Property

    Private Sub NewReference_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NewReference.Click
        m_xmlView.AddNew(lsbReferences)
    End Sub

    Private Sub DeleteReference_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteReference.Click
        m_xmlView.Delete(lsbReferences)
    End Sub

    Private Sub EditReference_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditReference.Click
        m_xmlView.Edit(lsbReferences)
    End Sub

    Private Sub RemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveAll.Click
        m_xmlView.RemoveAllReferences(lsbReferences)
    End Sub

    Private Sub AddReferences_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
                Handles mnuReplace.Click, mnuMerge.Click, mnuConfirm.Click

        m_xmlView.AddReferences(lsbReferences, sender.Tag)
    End Sub

    Private Sub RemoveRedundant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveRedundant.Click
        Dim bImportRemoved As Boolean = m_xmlView.RemoveRedundant(lsbReferences)
        If bImportRemoved Then
            Me.Tag = True
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub mnuRefDependencies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRefDependencies.Click
        If lsbReferences.SelectedItem() IsNot Nothing Then
            Dim fen As New dlgDependencies
            fen.Document = lsbReferences.SelectedItem()
            fen.ShowDialog()
            If CType(fen.Tag, Boolean) = True Then
                m_xmlView.Updated = True
            End If
        End If
    End Sub

    Private Sub DuplicateReference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DuplicateReference.Click
        m_xmlView.Duplicate(lsbReferences)
    End Sub

    Private Sub mnuCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopy.Click
        If lsbReferences.SelectedItem() IsNot Nothing Then
            XmlComponent.Clipboard.SetData(lsbReferences.SelectedItem())
        End If
    End Sub

    Private Sub mnuPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopy.Click
        If m_xmlView.PasteReference() Then
            m_xmlView.Updated = True
        End If
        mnuPaste.Enabled = False
    End Sub
End Class