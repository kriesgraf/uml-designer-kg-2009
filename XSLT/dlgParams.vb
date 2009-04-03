Imports System
Imports System.Collections.Generic
Imports System.Xml

Friend Class dlgParams
    Inherits System.Windows.Forms.Form

    Private m_strCurrentFolder As String
    Private m_strXmlFolder As String
    Private m_strCurrentParam As String
    Private m_arrayList As Object
    Private m_bInitializeComponent As Boolean = False
    Private m_bListIndexLock As Boolean = False
    Private m_bValeurUpdated As Boolean = False

    Public WriteOnly Property CurrentFolder() As String
        Set(ByVal value As String)
            m_strCurrentFolder = value
        End Set
    End Property

    Public WriteOnly Property XmlFolder() As String
        Set(ByVal value As String)
            m_strXmlFolder = value
        End Set
    End Property

    Public WriteOnly Property ParamList() As Object
        Set(ByVal value As Object)
            m_arrayList = value
        End Set
    End Property

    Private Sub btnValider_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnValider.Click
        SaveCurrent()
    End Sub

    Private Sub cmbListeParams_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbListeParams.SelectedIndexChanged
        Try
            If Me.m_bInitializeComponent Then Exit Sub
            m_bValeurUpdated = True

            m_strCurrentParam = cmbListeParams.SelectedItem
            txtValeur.Text = m_arrayList(m_strCurrentParam)

            If InStr(txtValeur.Text, "<xsl:") <> 0 _
            Then
                cmdPath.Enabled = False
                cmdFile.Enabled = False
                chkSeparator.Checked = False
                chkSeparator.Enabled = False
                txtValeur.ReadOnly = True
            Else
                txtValeur.ReadOnly = False

                If m_strCurrentParam.Contains("Folder") _
                Then
                    cmdPath.Enabled = True
                    cmdFile.Enabled = False
                Else
                    cmdPath.Enabled = True
                    cmdFile.Enabled = True
                End If
                chkSeparator.Checked = (Strings.Right(txtValeur.Text, 1) = "\")
                txtValeur.SelectionStart = 0
                txtValeur.SelectionLength = Len(txtValeur.Text)

                If m_bListIndexLock = False Then
                    txtValeur.Focus()
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bValeurUpdated = False
        End Try
    End Sub

    Private Sub cmdFile_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFile.Click
        Try
            Dim dlg As New OpenFileDialog

            If txtValeur.Text = "" Then
                dlg.InitialDirectory = m_strCurrentFolder
            Else
                dlg.InitialDirectory = txtValeur.Text
            End If
            If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtValeur.Text = dlg.FileName
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdPath_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPath.Click
        Try
            Dim dlg As New FolderBrowserDialog

            If txtValeur.Text = "" Then
                dlg.SelectedPath = m_strCurrentFolder
            Else
                dlg.SelectedPath = txtValeur.Text
            End If

            If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtValeur.Text = dlg.SelectedPath
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub dlgParams_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Try
            For Each dico As KeyValuePair(Of String, String) In m_arrayList
                cmbListeParams.Items.Add(dico.Key)
            Next dico

            If m_arrayList.Count > 0 Then
                m_bListIndexLock = True
                cmbListeParams.SelectedIndex = 0
                m_bListIndexLock = False
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub SaveCurrent()
        Try
            Debug.Print(m_strCurrentParam + "=" + txtValeur.Text)
            m_arrayList(m_strCurrentParam) = txtValeur.Text
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub txtValeur_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtValeur.TextChanged
        If m_bValeurUpdated Then Exit Sub
        chkSeparator_CheckedChanged(sender, e)
    End Sub

    Private Sub chkSeparator_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeparator.CheckedChanged
        m_bValeurUpdated = True
        Try
            If chkSeparator.Checked Then
                If Strings.Right(txtValeur.Text, 1) <> "\" Then
                    txtValeur.Text = txtValeur.Text + "\"
                End If
            Else
                If Strings.Right(txtValeur.Text, 1) = "\" Then
                    txtValeur.Text = Strings.Left(txtValeur.Text, txtValeur.Text.Length - 1)
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            m_bValeurUpdated = False
        End Try
    End Sub

    Private Sub btnXmlFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXmlFolder.Click
        txtValeur.Text = m_strXmlFolder
    End Sub
End Class