Imports System.Windows.Forms
Imports System.Text
Imports System
Imports ClassXmlProject.XmlProjectTools

Public Class dlgProjectProperties
    Implements InterfFormDocument

    Private m_xmlView As XmlProjectPropertiesView
    Private m_bDriveLock As Boolean = False

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If m_xmlView.UpdateValues() Then
            Me.Tag = True
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        txtName.CausesValidation = False
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            ' this line updates all values of XmlProjectPropertiesView object
            m_xmlView.Node = value.Node
        End Set
    End Property

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView("root")
    End Sub

    Private Sub dlgProjectProperties_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Text = m_xmlView.Name

            With m_xmlView
                .LoadValues()

                ' Bind controls and document values
                .InitBindingName(txtName)
                .InitBindingBriefComment(txtBrief)
                .InitBindingComment(txtDetails)
                .InitBindingLanguage(cmbLanguage)
                .InitBindingFolder(txtPath)
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub btnPath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPath.Click
        FolderBrowserDialog1.Description = "Select the project root folder..."
        FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop
        FolderBrowserDialog1.SelectedPath = txtPath.Text
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtPath.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub txtPath_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPath.TextChanged
        Try
            If My.Computer.FileSystem.DirectoryExists(txtPath.Text) Then
                fileListBox.Path = txtPath.Text
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub txtName_Validated(ByVal sender As TextBox, ByVal e As System.EventArgs) Handles txtName.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub txtName_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtName.Validating
        e.Cancel = IsInvalidProjectName(sender, Me.errorProvider, Me.cmbLanguage.SelectedIndex)
    End Sub
End Class
