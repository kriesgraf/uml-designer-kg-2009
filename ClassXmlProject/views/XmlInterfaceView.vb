Imports System
Imports System.Windows.Forms

Public Class XmlInterfaceView
    Inherits XmlInterfaceSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance()
    End Sub

    Public Sub UpdateValues()
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgInterface
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingName(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingPackage(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Package")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub LoadMembers(ByVal dataControl As XmlDataGridView)
        Try
            dataControl.Binding.LoadXmlNodes(Me, "property | method", "interface_member_view")
            dataControl.Binding.NodeCounter = Nothing

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub
End Class
