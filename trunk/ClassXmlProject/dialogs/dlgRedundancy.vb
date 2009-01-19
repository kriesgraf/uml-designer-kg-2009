Imports System.Windows.Forms
Imports System.Xml

Public Class dlgRedundancy

    Private m_xmlView As XmlRefRedundancyView = Nothing

    Public WriteOnly Property Node() As XmlNode
        Set(ByVal value As XmlNode)
            m_xmlView.Redundant = value
        End Set
    End Property

    Public Property Document() As XmlComponent
        Get
            Return m_xmlView
        End Get
        Set(ByVal value As XmlComponent)
            ' this line updates all values of XmlClassGlobalView object
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public WriteOnly Property Message() As String
        Set(ByVal value As String)
            lblMessage.Text = value
        End Set
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If m_xmlView.UpdateValues(lsbRedundantClasses) Then
            Me.Tag = True
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView("reference_redundancy")
    End Sub

    Private Sub dlgRedundancy_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_xmlView.LoadNodes(lsbRedundantClasses)
    End Sub
End Class
