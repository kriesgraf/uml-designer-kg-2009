Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgSimpleTypes

    Private m_strFilename As String
    Private m_eLang As ELanguage = ELanguage.Language_CplusPlus
    Private m_bContentChanged As Boolean = False

    Public WriteOnly Property Filename() As String
        Set(ByVal value As String)
            m_strFilename = value
        End Set
    End Property

    Public WriteOnly Property CodeLanguage() As ELanguage
        Set(ByVal value As ELanguage)
            m_eLang = value
        End Set
    End Property

    Private Sub dlgSimpleTypes_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If m_bContentChanged Then
            If MsgBox("Do you want to save change?", cstMsgYesNoQuestion, "Language simple types") = MsgBoxResult.Yes Then
                dtsSimpleTypesList.WriteXml(m_strFilename)
            End If
        End If

    End Sub

    Private Sub dlgSimpleTypes_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        grdSimpleTypeList.EndEdit()
    End Sub

    Private Sub dlgSimpleTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Text = "Update simple types (" + XmlProjectTools.GetLanguage(m_eLang) + ")"
            dtsSimpleTypesList.ReadXml(m_strFilename)

            Dim col As DataGridViewTextBoxColumn

            With grdSimpleTypeList
                .AutoGenerateColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .AllowUserToOrderColumns = False

                col = New DataGridViewTextBoxColumn
                With col
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .ReadOnly = False
                    .DataPropertyName = "name"
                    .HeaderText = "Name"
                    .Name = "ControlName_Name"
                End With
                .Columns.Add(col)

                col = New DataGridViewTextBoxColumn
                With col
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .ReadOnly = False
                    .DataPropertyName = "prefix"
                    .HeaderText = "Prefix"
                    .Name = "ControlName_Prefix"
                End With
                .Columns.Add(col)

                col = New DataGridViewTextBoxColumn
                With col
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .ReadOnly = False
                    .DataPropertyName = "implementation"
                    .HeaderText = "Implementation"
                    .Name = "ControlName_Implementation"
                End With
                .Columns.Add(col)

                col = New DataGridViewTextBoxColumn
                With col
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .ReadOnly = False
                    .DataPropertyName = "import"
                    .HeaderText = "Import"
                    .Name = "C"
                End With
                .Columns.Add(col)

                .DataSource = dtsSimpleTypesList.Tables("type")
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub AddToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddToolStripMenuItem.Click
        dtsSimpleTypesList.Tables("type").Rows().Add(New String() {"name1", "tt1", "type1"})
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        If grdSimpleTypeList.SelectedRows.Count > 0 Then
            Dim row As DataGridViewRow = grdSimpleTypeList.SelectedRows(0)
            If MsgBox("Please confirm delete '" + row.Cells(0).Value.ToString + "'", cstMsgYesNoQuestion, "'Delete' command") _
                     = MsgBoxResult.Yes _
            Then
                grdSimpleTypeList.Rows.Remove(row)
            End If
        End If
    End Sub

    Private Sub grdSimpleTypeList_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdSimpleTypeList.CellValueChanged
        m_bContentChanged = True
    End Sub
End Class
