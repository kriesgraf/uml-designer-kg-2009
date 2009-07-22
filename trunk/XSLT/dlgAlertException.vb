Imports System.Windows.Forms

Public Class dlgAlertException

    Private m_Exception As Exception

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgAlertException_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = TypeName(m_Exception)
        'Me.Opacity = 0.8
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
            Dim current As Exception = m_Exception.InnerException
            Dim strMessage As String = ""
            While current IsNot Nothing
                strMessage += current.Message + vbCrLf + vbCrLf + current.StackTrace
                current = current.InnerException
                strMessage += vbCrLf + "------------------------------------------------------------" + vbCrLf
            End While
            MsgBox(strMessage, MsgBoxStyle.Critical, TypeName(m_Exception))
        Else
            MsgBox("No inner exception", MsgBoxStyle.Exclamation)
        End If
    End Sub
End Class

Module MsgException
    Public Sub MsgExceptionBox(ByVal ex As Exception)
        Dim alert As New dlgAlertException(ex)
        alert.ShowDialog()
    End Sub
End Module

