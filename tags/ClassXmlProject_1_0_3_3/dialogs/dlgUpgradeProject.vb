Imports System
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class dlgUpgradeProject

    Private m_strFilename As String
    Private m_exception As Exception

    Public WriteOnly Property Filename() As String
        Set(ByVal value As String)
            m_strFilename = value
        End Set
    End Property

    Public WriteOnly Property Warning() As Exception
        Set(ByVal value As Exception)
            m_exception = value
        End Set
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgUpgradeProject_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMessage.Text = "Failed to open file:" + vbCrLf + m_strFilename + vbCrLf + vbCrLf _
                          + "You can upgrade version and patch some mistakes (Ok) or leave (Cancel) ?"
    End Sub

    Private Sub btnInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInfo.Click
        MsgExceptionBox(m_exception)
    End Sub
End Class
