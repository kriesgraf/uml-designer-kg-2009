Imports System
Imports ClassXmlProject.XmlProjectTools
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class dlgRelation
    Implements InterfFormDocument
    Implements InterfNodeCounter
    Implements InterfFormCollaboration

    Private m_xmlView As XmlRelationView
    Private m_bInit As Boolean = False
    Private m_strClassCollaborationtID As String

    Public WriteOnly Property ClassID() As String Implements InterfFormCollaboration.ClassID
        Set(ByVal value As String)
            m_strClassCollaborationtID = value
        End Set
    End Property

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "relationship")
    End Sub

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            ' Not used !
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
        If m_xmlView.Father.Idref = m_strClassCollaborationtID _
        Then
            m_xmlView.DisableFather()
        ElseIf m_xmlView.Child.Idref = m_strClassCollaborationtID _
        Then
            m_xmlView.DisableChild()
        End If
    End Sub

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.GenerationLanguage = value.GenerationLanguage
        End Set
    End Property

    Private Sub dlgRelation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            With m_xmlView
                m_bInit = True
                .LoadValues()

                ' Relationship attributes
                .InitBindingAction(txtAction)
                .InitBindingTypeComposition(cmbTypeComposition)

                ' Child attributes
                .InitBindingFatherName(txtFatherName)
                .InitBindingFatherMember(chkFatherMember)
                .InitBindingFatherLevel(cmbFatherLevel, lblFatherLevel)
                .InitBindingFatherClassList(cmbFatherClass, lblFatherClass)
                .InitBindingFatherAccessors(chkGetFather, chkSetFather)
                .InitBindingFatherCardinal(cmbFatherCardinal, lblFatherCardinal, btnFatherType)    ' Called at the end, because all references
                .InitBindingFatherRange(cmbFatherRange)                                            ' Called at the end, because all references

                ' Child attributes
                .InitBindingChildName(txtChildName)
                .InitBindingChildRange(cmbChildRange, btnChildType)
                .InitBindingChildMember(chkChildMember)
                .InitBindingChildLevel(cmbChildLevel, lblChildLevel)
                .InitBindingChildClassList(cmbChildClass, lblChildClass)
                .InitBindingChildAccessors(chkGetChild, chkSetChild)
                .InitBindingChildCardinal(cmbChildCardinal, lblChildCardinal)    ' Called at the end, because all references

                If cmbChildClass.Items.Count = 0 Or cmbFatherClass.Items.Count = 0 Then
                    MsgBox("The project doesn't not contain classes compatible with relationships", MsgBoxStyle.Exclamation, "Relationship")
                    Me.DialogResult = Windows.Forms.DialogResult.Cancel
                End If

                Me.Text = .Name
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bInit = False
        End Try
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            m_xmlView.UpdateValues()
            Me.Tag = True

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        txtFatherName.CausesValidation = False
        txtChildName.CausesValidation = False
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ChildType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChildType.Click
        Try
            Dim fen As Form = m_xmlView.Child.CreateDialogBox()
            fen.ShowDialog()
            If CType(fen.Tag, Boolean) = True Then
                m_xmlView.Updated = True
                btnChildType.Text = m_xmlView.ChildType
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub FatherType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFatherType.Click
        Try
            Dim fen As Form = m_xmlView.Father.CreateDialogBox()
            fen.ShowDialog()
            If CType(fen.Tag, Boolean) = True Then
                m_xmlView.Updated = True
                btnFatherType.Text = m_xmlView.FatherType
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox("This relation will be deleted, please confirm ?", cstMsgYesNoQuestion, "'Delete' command") _
            = MsgBoxResult.Yes Then
            If m_xmlView.RemoveMe() Then
                Me.Tag = True
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub cmbChildLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbChildLevel.SelectedIndexChanged
        If m_bInit Then Exit Sub
        btnChildType.Text = m_xmlView.ChildType
    End Sub

    Private Sub cmbFatherLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFatherLevel.SelectedIndexChanged
        If m_bInit Then Exit Sub
        btnFatherType.Text = m_xmlView.FatherType
    End Sub

    Private Sub txtName_Validated(ByVal sender As TextBox, ByVal e As System.EventArgs) _
            Handles txtFatherName.Validated, txtChildName.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub txtName_Validating(ByVal sender As TextBox, ByVal e As System.ComponentModel.CancelEventArgs) _
            Handles txtFatherName.Validating, txtChildName.Validating
        e.Cancel = IsInvalidVariableName(sender, Me.errorProvider)
    End Sub
End Class
