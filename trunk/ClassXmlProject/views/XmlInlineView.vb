Public Class XmlInlineView
    Inherits XmlCodeInline
    Implements InterfViewForm

    Private m_Control As TextBox

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgInlineCode
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingCode(ByVal control As TextBox)
        control.Text = Me.CodeSource
        m_Control = control
    End Sub

    Public Function LoadValues() As Boolean
    End Function

    Public Function UpdateValues() As Boolean
        Me.CodeSource = m_Control.Text
    End Function
End Class
