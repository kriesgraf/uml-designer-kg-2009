Imports System.Windows.Forms

Public Class ComboCommand

    Private m_combo As ComboBox
    Private m_label As Label

    Public WriteOnly Property Enabled() As Boolean
        Set(ByVal value As Boolean)
            m_combo.Enabled = value
            m_label.Enabled = value
        End Set
    End Property

    Public WriteOnly Property Visible() As Boolean
        Set(ByVal value As Boolean)
            m_combo.Visible = value
            m_label.Visible = value
        End Set
    End Property

    Public Property Combo() As ComboBox
        Get
            Return m_combo
        End Get
        Set(ByVal value As ComboBox)
            m_combo = value
        End Set
    End Property

    Public Property Title() As Label
        Get
            Return m_label
        End Get
        Set(ByVal value As Label)
            m_label = value
        End Set
    End Property
End Class
