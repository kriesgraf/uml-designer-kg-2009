Imports System
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class dlgAlertException

    Private m_Exception As Exception
    Private m_strIssueMessage As String = ""
    Private m_isize As Integer = 450

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            ' No Exception re-entrance
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()

        Catch ex As Exception
            ' No Exception re-entrance
        End Try
    End Sub

    Private Sub dlgAlertException_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Text = TypeName(m_Exception)
            Me.Opacity = 0.8
            If m_strIssueMessage <> "" Then
                Me.lblComment.Text = m_strIssueMessage
            End If
            lblMessage.Text = m_Exception.Message
            txtStackTrace.Text = m_Exception.StackTrace
            Me.Width = m_isize

        Catch ex As Exception
            ' No Exception re-entrance
        End Try
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
        Try
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
                MsgBox("No inner exception", MsgBoxStyle.Exclamation, "Inner exception")
            End If
        Catch ex As Exception
            ' No Exception re-entrance
        End Try
    End Sub

    Private Sub lblMessage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMessage.Click
        Try
            Clipboard.SetText(m_Exception.ToString)
            MsgBox("Message copied in clipboard", MsgBoxStyle.Information, "Clipboard")

        Catch ex As Exception
            ' No Exception re-entrance
        End Try
    End Sub

    Private Sub LinkLabelIssue_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabelIssue.LinkClicked

        ' Specify that the link was visited.
        Me.LinkLabelIssue.LinkVisited = True

        ' Navigate to a URL.
        Try
            System.Diagnostics.Process.Start("http://code.google.com/p/uml-designer-kg-2009/issues/entry")

        Catch ex As Exception
            ' No Exception re-entrance
        End Try

    End Sub
End Class

Module MsgException
    Public Sub MsgExceptionBox(ByVal ex As Exception, Optional ByVal strIssueMessage As String = "", Optional ByVal size As Integer = 600)
        Dim alert As New dlgAlertException(ex, strIssueMessage, size)
        alert.ShowDialog()
    End Sub
End Module

