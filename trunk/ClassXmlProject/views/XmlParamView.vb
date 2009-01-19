Public Class XmlParamView
    Inherits XmlParamSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_xmlNodeManager As XmlNodeManager

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
    End Sub

    Public Sub UpdateValues()
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As Form = New dlgParam
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingName(ByVal dataControl As Windows.Forms.TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Name")
    End Sub

    Public Sub InitBindingComment(ByVal dataControl As Windows.Forms.TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
