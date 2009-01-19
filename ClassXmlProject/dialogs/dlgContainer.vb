Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class dlgContainer
    Implements InterfFormDocument

    Private m_xmlView As XmlContainerView

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
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
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
