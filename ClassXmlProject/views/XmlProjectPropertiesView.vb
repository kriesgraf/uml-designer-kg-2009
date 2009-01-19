Imports Microsoft.VisualBasic.Compatibility.VB6

Public Class XmlProjectPropertiesView
    Inherits XmlProjectProperties
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_controlPath As TextBox

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgProjectProperties
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
    End Sub

    Public Sub UpdateValues()
        Me.GenerationFolder = m_controlPath.Text
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Sub InitBindingName(ByVal dataControl As Windows.Forms.Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")

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

    Public Sub InitBindingLanguage(ByVal cmbLanguage As ComboBox)
        Dim list As New Dictionary(Of String, Integer)
        cmbLanguage.Items.AddRange(New String() {"C++", "Java", "VB.NET"})
        m_xmlBindingsList.AddBinding(cmbLanguage, Me, "GenerationLanguage", "SelectedIndex")
    End Sub

    Public Sub InitBindingFolder(ByVal control As TextBox)
        m_controlPath = control
        control.Text = Me.GenerationFolder
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
