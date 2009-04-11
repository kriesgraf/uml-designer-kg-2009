Imports System
Imports System.Windows.Forms

Public Class dlgInterface
    Implements InterfFormDocument

    Private m_xmlView As XmlInterfaceView

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

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
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "interface")
    End Sub

    Private Sub dlgInterface_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If XmlProjectTools.DEBUG_COMMANDS_ACTIVE Then
            MemberProperties.Visible = True
        End If

        With m_xmlView

            .LoadValues()

            .InitBindingName(txtName)
            .InitBindingPackage(txtPackage)
            .InitBindingRoot(chkRoot)

            .LoadMembers(gridMembers)

            Me.Text = .Name
        End With

    End Sub

    Private Sub mnuAddMember_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
                Handles AddProperty.Click, AddMethod.Click

        gridMembers.AddItem(CType(sender.Tag, String))
    End Sub

    Private Sub mnuEditMember_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditMember.Click
        gridMembers.EditCurrentItem()
    End Sub

    Private Sub mnuMemberDependencies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMemberDependencies.Click
        If gridMembers.SelectedItem IsNot Nothing Then
            Dim bIsEmpty As Boolean = False
            If dlgDependencies.ShowDependencies(CType(gridMembers.SelectedItem, XmlComponent), bIsEmpty) Then
                m_xmlView.Updated = True
            End If
        End If
    End Sub

    Private Sub mnuMemberProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemberProperties.Click
        dlgXmlNodeProperties.DisplayProperties(gridMembers.SelectedItem)
        m_xmlView.Updated = True
    End Sub

    Private Sub DuplicateMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DuplicateMember.Click
        gridMembers.DuplicateSelectedItem()
    End Sub

    Private Sub CopyMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyMember.Click
        PasteMember.Enabled = gridMembers.CopySelectedItem()
    End Sub

    Private Sub PasteMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteMember.Click
        PasteMember.Enabled = Not (gridMembers.PasteItem())
    End Sub

    Private Sub mnuDeleteMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMember.Click
        gridMembers.DeleteSelectedItems()
    End Sub

    Private Sub GridRowValuesChanged(ByVal sender As Object) Handles gridMembers.RowValuesChanged
        ' TODO: for future use
    End Sub
End Class
