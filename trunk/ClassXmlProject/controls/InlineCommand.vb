Imports System.Windows.Forms

Public Class InlineCommand
    Public WithEvents m_chkInline As CheckBox
    Public m_btnInline As Button

    Public WriteOnly Property Visible() As Boolean
        Set(ByVal value As Boolean)
            m_chkInline.Visible = value
            m_btnInline.Visible = value
        End Set
    End Property

    Public Property Inline() As CheckBox
        Get
            Return m_chkInline
        End Get
        Set(ByVal value As CheckBox)
            m_chkInline = value
        End Set
    End Property

    Public Property Body() As Button
        Get
            Return m_btnInline
        End Get
        Set(ByVal value As Button)
            m_btnInline = value
            m_btnInline.Enabled = m_chkInline.Checked
        End Set
    End Property

    Private Sub CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_chkInline.CheckedChanged
        m_btnInline.Enabled = m_chkInline.Checked
    End Sub
End Class
