Imports System
Imports System.Windows.Forms

Public Class dlgException
    Implements InterfFormDocument

    Private m_xmlView As XmlMethodExceptionView = Nothing

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

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
        End Set
    End Property

    Private Sub chkVisibility_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVisibility.Click
        m_xmlView.InitAvailableClassesList(chkVisibility.Checked, lstClasses)
    End Sub

    Private Sub dlgException_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            With m_xmlView
                .InitAvailableClassesList(chkVisibility.Checked, lstClasses)
                Me.Text = "Add exceptions"
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView("method_exception_view")
    End Sub
End Class
