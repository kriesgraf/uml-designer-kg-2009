Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class dlgAlertWarning
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Me.chkConfirm.Checked = False Then
            MsgBox("Have you read the warning message and check to agree operation ?", MsgBoxStyle.Exclamation)
        Else
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class

Module MsgWarning
    Public Function MsgWarningBox(ByVal strMessage As String, Optional ByVal strTitle As String = "Caution !") As DialogResult
        Dim fen As New dlgAlertWarning
        fen.Text = strTitle
        fen.lblWarningMessage.Text = strMessage
        Return fen.ShowDialog()
    End Function
End Module
