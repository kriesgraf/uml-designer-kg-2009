Imports System
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlProjectTools

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

    Public Function UpdateValues() As Boolean
        If My.Computer.FileSystem.DirectoryExists(m_controlPath.Text) = False Then
            MsgBox("Sources location folder doesn't exist !", MsgBoxStyle.Exclamation)
            Return False
        Else
            Me.GenerationFolder = m_controlPath.Text
        End If
        m_xmlBindingsList.UpdateValues()
        Return True
    End Function

    Public Sub InitBindingName(ByVal dataControl As Control)
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
        cmbLanguage.Items.AddRange(New String() _
                                   {GetLanguage(CType(0, ELanguage)), _
                                    GetLanguage(CType(1, ELanguage)), _
                                    GetLanguage(CType(2, ELanguage))})
        m_xmlBindingsList.AddBinding(cmbLanguage, Me, "ValueLanguage", "SelectedIndex")
    End Sub

    Public Sub InitBindingFolder(ByVal control As TextBox)
        m_controlPath = control
        control.Text = Me.GenerationFolder
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
