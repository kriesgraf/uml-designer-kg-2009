Imports System
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public NotInheritable Class dlgAboutBox

    Private m_strMessage As String

    Public WriteOnly Property Message() As String
        Set(ByVal value As String)
            m_strMessage = value
        End Set
    End Property

    Private Sub dlgAboutBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Définissez le titre du formulaire.
        'Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            Me.Text = My.Application.Info.Title
        Else
            Me.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If

        Me.LabelProductName.Text = Application.ProductName

        If Deployment.Application.ApplicationDeployment.IsNetworkDeployed Then
            Me.LabelVersion.Text = My.Application.Deployment.CurrentVersion.ToString
        Else
            Me.LabelVersion.Text = "not published"
        End If

        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = My.Application.Info.Description + vbCrLf + m_strMessage
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

    Private Sub LabelCompanyName_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LabelCompanyName.LinkClicked

        ' Specify that the link was visited.
        Me.LabelCompanyName.LinkVisited = True

        ' Navigate to a URL.
        System.Diagnostics.Process.Start("http://code.google.com/p/uml-designer-kg-2009/")
    End Sub

    Private Sub TextBoxDescription_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxDescription.Click
        Clipboard.SetText(m_strMessage)
        MsgBox("Message copied in clipboard", MsgBoxStyle.Information)
    End Sub
End Class
