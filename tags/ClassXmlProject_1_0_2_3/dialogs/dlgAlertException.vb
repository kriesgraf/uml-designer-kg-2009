Imports System
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class dlgAlertException

    Private m_Exception As Exception
    Private m_strIssueMessage As String = ""
    Private m_isize As Integer = 450

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
        If m_strIssueMessage = "" Then
            Me.lblComment.Text = "Click on text message to copy 'error stacktrace' to clipboard." + vbCrLf _
                                + "Click on link to send a new issue and paste 'stracktrace'."
        Else
            Me.lblComment.Text = m_strIssueMessage
        End If
        lblMessage.Text = m_Exception.Message
        txtStackTrace.Text = m_Exception.StackTrace
        Me.Width = m_isize
    End Sub

    Public Sub New(ByVal ex As Exception, Optional ByVal strIssueMessage As String = "", Optional ByVal size As Integer = 450)

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_Exception = ex
        m_strIssueMessage = strIssueMessage
        m_isize = size
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

    Private Sub LinkLabelIssue_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabelIssue.LinkClicked

        ' Specify that the link was visited.
        Me.LinkLabelIssue.LinkVisited = True

        ' Navigate to a URL.
        System.Diagnostics.Process.Start("http://code.google.com/p/uml-designer-kg-2009/issues/entry")
    End Sub
End Class

Module MsgException
    Public Sub MsgExceptionBox(ByVal ex As Exception, Optional ByVal strIssueMessage As String = "", Optional ByVal size As Integer = 600)
        Dim alert As New dlgAlertException(ex, strIssueMessage, size)
        alert.ShowDialog()
    End Sub
End Module

