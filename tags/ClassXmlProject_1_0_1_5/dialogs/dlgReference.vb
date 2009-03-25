Imports System
Imports System.Windows.Forms

Public Class dlgReference
    Implements InterfFormDocument

    Private m_xmlView As XmlReferenceView

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
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
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = DialogResult.Cancel
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

            Me.Text = .Name
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

            Case Else
                cmbContainer.Enabled = True
        End Select
    End Sub
End Class
