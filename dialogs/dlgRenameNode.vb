Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class dlgRenameNode

    Public WriteOnly Property Title() As String
        Set(ByVal value As String)
            Me.Text = value
        End Set
    End Property

    Public WriteOnly Property Label() As String
        Set(ByVal value As String)
            Me.lblLabel.Text = value
        End Set
    End Property

    Public ReadOnly Property Result() As String
        Get
            Return Me.txtResult.Text
        End Get
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        txtResult.CausesValidation = False
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtPackage_Validated(ByVal sender As TextBox, ByVal e As System.EventArgs) _
            Handles txtResult.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub txtPackage_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) _
            Handles txtResult.Validating
        e.Cancel = IsInvalidPackageName(sender, Me.errorProvider, CInt(Me.Tag), True)
    End Sub

End Class
