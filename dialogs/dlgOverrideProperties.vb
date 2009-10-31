Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class dlgOverrideProperties

    Public Enum EAnswer
        MembersToOverride
        NoMembers
        SomeError
        MembersUpdated
    End Enum

    Private m_xmlView As XmlClassOverridePropertiesView

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If m_xmlView.AddProperties(lstOverrideProperties) _
        Then
            Me.Tag = True
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New(ByVal document As XmlComponent, ByVal eImplementation As EImplementation)

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(document.Node, "class_override_properties")
        m_xmlView.CurrentClassImpl = eImplementation
    End Sub

    Public Function OverrideProperties(Optional ByVal strIgnoredClasses As String = "") As EAnswer
        Dim eResult As EAnswer = EAnswer.NoMembers
        Try
            If m_xmlView.InitListProperties(strIgnoredClasses) Then
                Me.ShowDialog()
                If CType(Me.Tag, Boolean) Then
                    Return EAnswer.MembersUpdated
                End If
            End If
            Return EAnswer.NoMembers

        Catch ex As Exception
            eResult = EAnswer.SomeError
            MsgExceptionBox(ex)
        End Try
        Return eResult
    End Function

    Private Sub dlgOverrideProperties_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_xmlView.LoadProperties(lstOverrideProperties)
        Me.Text = "Add overridden Properties to class " + m_xmlView.Name
    End Sub
End Class
