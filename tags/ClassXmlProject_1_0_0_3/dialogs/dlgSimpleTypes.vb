Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgSimpleTypes

    Private m_strFilename As String

    Public WriteOnly Property Filename() As String
        Set(ByVal value As String)
            m_strFilename = value
        End Set
    End Property

    Private Sub dlgSimpleTypes_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dtsSimpleTypesList.WriteXml(m_strFilename)
    End Sub

    Private Sub dlgSimpleTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
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
        Dim row As DataGridViewRow = grdSimpleTypeList.SelectedRows(0)
        If MsgBox("Please confirm delete '" + row.Cells(0).Value.ToString + "'", cstMsgYesNoQuestion) _
                 = MsgBoxResult.Yes _
        Then
            grdSimpleTypeList.Rows.Remove(row)
        End If
    End Sub
End Class
