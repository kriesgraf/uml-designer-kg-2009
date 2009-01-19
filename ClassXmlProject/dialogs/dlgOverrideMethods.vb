Imports System.Windows.Forms

Public Class dlgOverrideMethods

    Private m_xmlView As XmlClassOverrideMethodsView

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If m_xmlView.AddMethods(lstOverrideMethods) _
        Then
            Me.Tag = True
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New(ByVal document As XmlComponent)

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(document.Node, "class_override_methods")
    End Sub

    Private Sub dlgOverrideMethods_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_xmlView.InitListMethods(lstOverrideMethods)
        Me.Text = "Add overrided methods to class " + m_xmlView.Name
    End Sub
End Class
