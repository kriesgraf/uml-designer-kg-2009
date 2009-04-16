Imports System
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class dlgAlertException

    Private m_Exception As Exception

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgAlertException_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = TypeName(m_Exception)
        Me.Opacity = 0.8
        lblMessage.Text = m_Exception.Message
        txtStackTrace.Text = m_Exception.StackTrace
    End Sub

    Public Sub New(ByVal ex As Exception)

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_Exception = ex
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        If m_Exception.InnerException IsNot Nothing Then
            MsgBox(m_Exception.InnerException.Message + vbCrLf + vbCrLf + m_Exception.InnerException.StackTrace, MsgBoxStyle.Critical, TypeName(m_Exception))
        Else
            MsgBox("No inner exception", MsgBoxStyle.Exclamation, "Inner exception")
        End If
    End Sub

    Private Sub lblMessage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMessage.Click
        Clipboard.SetText(m_Exception.ToString)
        MsgBox("Message copied in clipboard", MsgBoxStyle.Information, "Clipboard")
    End Sub
End Class

Module MsgException
    Public Sub MsgExceptionBox(ByVal ex As Exception)
        Dim alert As New dlgAlertException(ex)
        alert.ShowDialog()
    End Sub
End Module

