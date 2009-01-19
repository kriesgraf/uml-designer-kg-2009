Imports System.Windows.Forms

Public Class dlgSuperClass
    Implements InterfFormDocument

    Private m_xmlView As XmlSuperClassView = Nothing

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            m_xmlView.UpdateValues()
            Me.Tag = True

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            ' This line get reference of object in argument, neither view creation nor XML node copy !
            m_xmlView = CType(value, XmlSuperClassView)
        End Set
    End Property

    Private Sub dlgSuperClass_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With m_xmlView
            .InitAvailableClassesList(chkVisibility.Checked, lstClasses)
            Me.Text = "Add mother classes to " + .Name
        End With
    End Sub

    Private Sub chkVisibility_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVisibility.Click
        m_xmlView.InitAvailableClassesList(chkVisibility.Checked, lstClasses)
    End Sub
End Class
