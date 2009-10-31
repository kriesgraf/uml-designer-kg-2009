Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgSimpleTypes

    Private m_xmlDocument As New XmlDocument
    Private m_bValide As Boolean = False
    Private m_bContentChanged As Boolean = False
    Private m_strFilename As String
    Private m_eLang As ELanguage

    Private Class XmlSimpleType
        Inherits XmlComponent

        Public Property Prefix() As String
            Get
                Return GetAttribute("prefix")
            End Get
            Set(ByVal value As String)
                SetAttribute("prefix", value)
            End Set
        End Property

        Public Property Implementation() As String
            Get
                Return GetAttribute("implementation")
            End Get
            Set(ByVal value As String)
                SetAttribute("implementation", value)
            End Set
        End Property

        Public Property Import() As String
            Get
                Return GetAttribute("import")
            End Get
            Set(ByVal value As String)
                If value = "" Then
                    RemoveAttribute("import")
                Else
                    AddAttribute("import", value)
                End If
            End Set
        End Property

        Public Sub New(Optional ByVal node As XmlNode = Nothing)
            MyBase.New(node)
        End Sub
    End Class

    Public WriteOnly Property CodeLanguage() As ELanguage
        Set(ByVal value As ELanguage)
            m_eLang = value
        End Set
    End Property

    Public WriteOnly Property Filename() As String
        Set(ByVal value As String)
            m_strFilename = value
        End Set
    End Property

    Private Sub dlgSimpleTypes_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            Dim filename As String = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath, m_strFilename)

            If m_bValide And m_bContentChanged Then
                If MsgBox("Do you want to save change?", cstMsgYesNoQuestion, "Language prefix names") = MsgBoxResult.Yes Then
                    m_xmlDocument.Save(filename)
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub dlgSimpleTypes_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        grdSimpleTypeList.EndEdit()
    End Sub

    Private Sub dlgSimpleTypes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Text = "Update simple types (" + GetLanguage(m_eLang) + ")"
            Dim filename As String = My.Computer.FileSystem.CombinePath(Application.LocalUserAppDataPath, m_strFilename)

            m_xmlDocument.Load(filename)

            Dim MyList As New ArrayList

            For Each node As XmlNode In m_xmlDocument.SelectNodes("//root/*")
                MyList.Add(New XmlSimpleType(node))
            Next

            '            Dim col As DataGridViewTextBoxColumn

            With grdSimpleTypeList
                .AutoGenerateColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .AllowUserToOrderColumns = False

                Dim col As DataGridViewTextBoxColumn = New DataGridViewTextBoxColumn
                With col
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .ReadOnly = False
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

                col = New DataGridViewTextBoxColumn
                With col
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .ReadOnly = False
                    .DataPropertyName = "Implementation"
                    .HeaderText = "Implementation"
                    .Name = "ControlName_Implementation"
                End With
                .Columns.Add(col)

                col = New DataGridViewTextBoxColumn
                With col
                    .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .ReadOnly = False
                    .DataPropertyName = "Import"
                    .HeaderText = "Import"
                    .Name = "ControlName_Import"
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

    Private Sub grdSimpleTypeList_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdSimpleTypeList.CellValueChanged
        m_bContentChanged = True
    End Sub
End Class
