Imports System
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class XmlPackageView
    Inherits XmlPackageSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_dataControl As XmlDataGridView
    Private m_chkFolder As CheckBox
    Private m_txtFolder As TextBox

    Public ReadOnly Property ProjectFolder() As String
        Get
            Return GetAttribute("destination", "//generation")
        End Get
    End Property

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As Form = New dlgPackage
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
    End Sub

    Public Sub UpdateValues()
        Try
            If m_chkFolder.Checked = False Then
                m_txtFolder.Text = ""
            End If
            m_xmlBindingsList.UpdateValues()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingName(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingCheckFolder(ByVal dataControl As CheckBox)
        Try
            m_chkFolder = dataControl
            m_xmlBindingsList.AddBinding(dataControl, Me, "IsFolder", "Checked")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingTextFolder(ByVal dataControl As TextBox)
        Try
            m_txtFolder = dataControl
            m_xmlBindingsList.AddBinding(dataControl, Me, "Folder")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingBriefComment(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "BriefComment")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingComment(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub LoadClasses(ByVal dataControl As XmlDataGridView)
        Try
            m_dataControl = dataControl
            dataControl.Binding.LoadXmlNodes(Me, "import | class | package", "package_class_view")
            dataControl.Binding.NodeCounter = m_xmlReferenceNodeCounter

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub ImportNodes(ByVal form As Form, Optional ByVal bUpdateOnly As Boolean = False)
        Dim bResult As Boolean = False
        Try
            Dim dlgOpenFile As New OpenFileDialog

            If My.Settings.ImportFolder = "." + Path.DirectorySeparatorChar.ToString Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.ImportFolder
            End If

            dlgOpenFile.Title = "Select a project file..."
            dlgOpenFile.Filter = "UML project (*.xprj)|*.xprj"

            If dlgOpenFile.ShowDialog() = DialogResult.OK Then

                Dim FileName As String = dlgOpenFile.FileName
                Dim i As Integer = InStrRev(FileName, Path.DirectorySeparatorChar.ToString)

                If i > 0 Then
                    My.Settings.ImportFolder = XmlProjectTools.GetProjectPath(FileName)
                Else
                    My.Settings.ImportFolder = dlgOpenFile.InitialDirectory
                End If

                If UmlNodesManager.ImportNodes(form, Me, FileName, m_xmlReferenceNodeCounter, bUpdateOnly) Then
                    m_dataControl.Binding.ResetBindings(True)
                    Me.Updated = True
                    bResult = True
                End If

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
