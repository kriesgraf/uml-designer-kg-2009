Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class dlgRelationParent
    Implements InterfFormDocument

    Private m_xmlView As XmlRelationParentView

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
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

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

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
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "father")
    End Sub

    Private Sub dlgRelationParent_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            With m_xmlView
                .LoadValues()
                .InitBindingOption(optTypeArray)
                .InitBindingIterator(Me.chkIterator)
                .InitBindingCheckIndex(Me.chkIndex)
                .InitBindingContainer(Me.cmbContainer)
                .InitBindingArraySize(cmbArraySize)
                .InitBindingComboIndex(Me.cmbIndex)
                .InitBindingIndexLevel(Me.cmbIndexLevel, Me.lblIndexLevel)

                Me.Text = .Name
            End With
            ChangeContainer(optTypeArray.Item(1).Checked)
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub optTypeArray_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles optTypeArray.Click
        Try
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

            ChangeContainer(optTypeArray.Item(1).Checked)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub chkIndex_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIndex.Click
        Try
            ChangeComboIndex(chkIndex.Checked)
            m_xmlView.RefreshComboContainer(chkIndex.Checked)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub ChangeContainer(ByVal bChecked As Boolean)
        If bChecked Then
            cmbArraySize.Enabled = False
            lblContainer.Enabled = True
            cmbContainer.Enabled = True
            chkIterator.Enabled = True
            chkIndex.Enabled = True
            ChangeComboIndex(chkIndex.Checked)
        Else
            cmbArraySize.Enabled = True
            lblContainer.Enabled = False
            cmbContainer.Enabled = False
            chkIterator.Enabled = False
            chkIndex.Enabled = False
            ChangeComboIndex(False)
        End If

    End Sub

    Private Sub ChangeComboIndex(ByVal bChecked As Boolean)
        If bChecked Then
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
End Class
