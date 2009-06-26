Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgPrefixNames

    Private m_xmlDocument As New XmlDocument
    Private m_bValide As Boolean = False
    Private m_bContentChanged As Boolean = False

    Private Class XmlPrefix

        Private m_XmlNode As XmlNode

        Public Sub New(ByVal node As XmlNode)
            m_XmlNode = node
        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return m_XmlNode.Name
            End Get
        End Property

        Public Property Prefix() As String
            Get
                Dim delim As String = Chr(10) + Chr(13) + Chr(9) + Chr(32)
                Return m_XmlNode.InnerText.Trim(delim)
            End Get
            Set(ByVal value As String)
                XmlProjectTools.SetNodeString(m_XmlNode, value)
            End Set
        End Property
    End Class

    Private Sub dlgPrefixNames_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            Dim filename As String = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath, cstPrefixNameFilename)

            If m_bValide And m_bContentChanged Then
                If MsgBox("Do you want to save change?", cstMsgYesNoQuestion, "Language prefix names") = MsgBoxResult.Yes Then
                    m_xmlDocument.Save(filename)
                    ReloadPrefixNameDocument(filename)
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub dlgPrefixNames_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        grdPrefixNameList.EndEdit()
    End Sub

    Private Sub dlgPrefixNames_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Text = "Update prefix names"
            Dim filename As String = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath, cstPrefixNameFilename)

            m_xmlDocument.Load(filename)

            Dim MyList As New ArrayList

            For Each node As XmlNode In m_xmlDocument.SelectNodes("//root/*")
                MyList.Add(New XmlPrefix(node))
            Next

            Dim col As DataGridViewTextBoxColumn

            With grdPrefixNameList
                .AutoGenerateColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .AllowUserToOrderColumns = False

                col = New DataGridViewTextBoxColumn
                With col
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .ReadOnly = True
                    .DataPropertyName = "Name"
                    .HeaderText = "Name"
                    .Name = "ControlName_Name"
                End With
                .Columns.Add(col)

                col = New DataGridViewTextBoxColumn
                With col
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .ReadOnly = False
                    .DataPropertyName = "Prefix"
                    .HeaderText = "Prefix"
                    .Name = "ControlName_Prefix"
                End With
                .Columns.Add(col)

                .DataSource = MyList
            End With
            m_bValide = True

        Catch ex As Exception
            MsgExceptionBox(ex)
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End Try
    End Sub

    Private Sub grdPrefixNameList_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdPrefixNameList.CellValueChanged
        m_bContentChanged = True
    End Sub
End Class
