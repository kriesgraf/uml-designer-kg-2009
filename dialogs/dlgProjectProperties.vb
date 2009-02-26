Imports System.Windows.Forms
Imports System.Text
Imports System

Public Class dlgProjectProperties
    Implements InterfFormDocument

    Private m_xmlView As XmlProjectPropertiesView
    Private m_bDriveLock As Boolean = False

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        m_xmlView.UpdateValues()
        Me.Tag = True
        Me.DialogResult = DialogResult.OK
        Me.Close()
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
        FolderBrowserDialog1.SelectedPath = fileListBox.Path
        If FolderBrowserDialog1.ShowDialog() Then
            txtPath.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Public Class shlwapi
        <System.Runtime.InteropServices.DllImport("shlwapi.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
        Shared Function PathCompactPath(ByVal hDC As IntPtr, ByVal lpszPath As StringBuilder, ByVal dx As Integer) As Boolean
        End Function
    End Class

    Public Class user32
        <System.Runtime.InteropServices.DllImport("user32")> _
        Shared Function GetWindowDC(ByVal hWnd As IntPtr) As IntPtr
        End Function
    End Class

    Private Function CompactPath(ByVal control As Control, ByVal strFullPathFilename As String) As String
        Dim strTempo As New StringBuilder(strFullPathFilename)
        shlwapi.PathCompactPath(user32.GetWindowDC(control.Handle), strTempo, control.ClientSize.Width)
        Return strTempo.ToString
    End Function

    Private Sub txtPath_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPath.TextChanged
        Try
            fileListBox.Path = txtPath.Text
            lblPath.Text = CompactPath(lblPath, txtPath.Text)
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub
End Class
