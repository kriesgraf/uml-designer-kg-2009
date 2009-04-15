Imports System
Imports ClassXmlProject.XmlProjectTools
Imports System.Windows.Forms
Imports System.Xml
Imports Microsoft.VisualBasic

Public Class dlgClass
    Implements InterfFormDocument
    Implements InterfNodeCounter

#Region "Class declarations"

    Private m_toolTip As ToolTip
    Private m_xmlView As XmlClassGlobalView = Nothing

#End Region

#Region "Public methods"

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlView.NodeCounter = value
        End Set
    End Property

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            ' this line updates all values of XmlClassGlobalView object
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque apr�s l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView("class")
        m_toolTip = New ToolTip

    End Sub

#End Region

#Region "Private methods"

    Private Sub dlgClass_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            With m_toolTip
                .AutoPopDelay = 5000
                .InitialDelay = 1000
                .ReshowDelay = 500
                .ShowAlways = False
                .SetToolTip(txtBrief, txtBrief.Text)
            End With

            Me.Text = m_xmlView.Name

            With m_xmlView
                .LoadValues()

                ' Bind controls and document values
                .InitBindingName(txtName)
                .InitBindingBriefComment(txtBrief)
                .InitBindingComment(txtDetails)
                .InitBindingConstructor(lblConstructor, cmbConstructor, btnConstructor)
                .InitBindingDestructor(lblDestructor, cmbDestructor, btnDestructor)
                .InitBindingImplementation(cmbImplementation)
                .InitBindingVisibility(cmbVisibility)
                .InitBindingInline(lblInline, cmbModelInline)
                .InitBindingPartial(btnConstructor, btnDestructor, chkPartial)

                ' Load document values into controls
                .LoadDependencies(gridDependencies)
                .LoadInheritedMembers(gridInherited)
                .LoadMembers(gridMembers)
                .LoadRelations(gridRelations)

                .UpdateMenuClass(AddTypedef)
            End With

            PasteMember.Enabled = XmlComponent.Clipboard.CanPaste

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            If m_xmlView.UpdateValues() Then
                Me.Tag = True
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox(m_xmlView.Name + " will be deleted, please confirm ?", cstMsgYesNoQuestion) _
            = MsgBoxResult.Yes Then
            If m_xmlView.RemoveMe() Then
                Me.Tag = True
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub btnDestructor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDestructor.Click
        m_xmlView.ShowDialogDestructor()
    End Sub

    Private Sub btnConstructor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConstructor.Click
        m_xmlView.ShowDialogConstructor()
    End Sub

    Private Sub cmbModelInline_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbModelInline.SelectedIndexChanged

        If m_xmlView.MustCheckTemplate() = False _
        Then
            Select Case CType(CType(sender, ComboBox).SelectedItem, String)
                Case "constructor"
                    btnConstructor.Enabled = True
                    btnDestructor.Enabled = False

                Case "destructor"
                    btnConstructor.Enabled = False
                    btnDestructor.Enabled = True

                Case "both"
                    btnConstructor.Enabled = True
                    btnDestructor.Enabled = True

                Case "none"
                    btnConstructor.Enabled = False
                    btnDestructor.Enabled = False
            End Select
        End If
    End Sub

    Private Sub txtBrief_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBrief.TextChanged
        m_toolTip.SetToolTip(txtBrief, txtBrief.Text)
    End Sub

    Private Sub mnuDeleteSuperClass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteSuperClass.Click
        If gridInherited.DeleteSelectedItems() Then
            gridMembers.Binding.ResetBindings(True)
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuDeleteMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMember.Click
        If gridMembers.DeleteSelectedItems() Then
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuDeleteDependency_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteDependency.Click
        If gridDependencies.DeleteSelectedItems() Then
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuDeleteRelation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteRelation.Click
        If gridRelations.DeleteSelectedItems() Then
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuAddSuperClass_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddSuperClass.Click
        Dim xmlcpnt As XmlSuperClassView = CType(XmlNodeManager.GetInstance().CreateView(m_xmlView.Node, "class_superclass_view", m_xmlView.Document), XmlSuperClassView)
        xmlcpnt.CurrentImplementation = m_xmlView.CurrentClassImpl()
        xmlcpnt.Tag = m_xmlView.Tag

        Dim fen As Form = xmlcpnt.CreateForm(xmlcpnt)
        fen.ShowDialog()
        If CType(fen.Tag, Boolean) = True Then
            m_xmlView.Updated = True
            gridInherited.Binding.ResetBindings(True)
            gridMembers.Binding.ResetBindings(True)
        End If
    End Sub

    Private Sub mnuAddDependency_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddDependency.Click
        gridDependencies.AddItem()
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuAddRelation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddRelation.Click
        gridRelations.AddItem()
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuAddMember_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
                Handles AddTypedef.Click, AddProperty.Click, AddMethod.Click, AddConstructor.Click, _
                        AddStructure.Click, AddContainer.Click

        gridMembers.AddItem(sender.Tag)
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuEditMember_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditMember.Click
        If gridMembers.EditCurrentItem() Then
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuEditRelation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditRelation.Click
        If gridRelations.EditCurrentItem() Then
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuInheritsProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InheritsProperties.Click
        dlgXmlNodeProperties.DisplayProperties(gridInherited.SelectedItem)
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuDependencyProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DependencyProperties.Click
        dlgXmlNodeProperties.DisplayProperties(gridDependencies.SelectedItem)
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuMemberProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemberProperties.Click
        dlgXmlNodeProperties.DisplayProperties(gridMembers.SelectedItem)
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuRelationProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RelationProperties.Click
        dlgXmlNodeProperties.DisplayProperties(gridRelations.SelectedItem)
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuMemberDependencies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMemberDependencies.Click
        If gridMembers.SelectedItem() IsNot Nothing Then
            Dim fen As New dlgDependencies
            fen.Document = gridMembers.SelectedItem()
            fen.ShowDialog()
            If CType(fen.Tag, Boolean) = True Then
                m_xmlView.Updated = True
            End If
        End If
    End Sub

    Private Sub gridDependencies_RowValuesChanged(ByVal sender As Object) _
                Handles gridDependencies.RowValuesChanged, _
                gridInherited.RowValuesChanged, _
                gridMembers.RowValuesChanged, _
                gridRelations.RowValuesChanged

        m_xmlView.Updated = True
    End Sub

    Private Sub mnuOverrideProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOverrideProperties.Click
        If m_xmlView.OverrideProperties() Then
            gridMembers.Binding.ResetBindings(True)
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub mnuOverrideMethods_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuOverrideMethods.Click
        If m_xmlView.OverrideMethods() Then
            gridMembers.Binding.ResetBindings(True)
            m_xmlView.Updated = True
        End If
    End Sub

    Private Sub DuplicateMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DuplicateMember.Click
        If gridMembers.DuplicateSelectedItem() Then
            m_xmlView.Updated = True
        End If
    End Sub
#End Region

    Private Sub CopyMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyMember.Click
        PasteMember.Enabled = gridMembers.CopySelectedItem()
    End Sub

    Private Sub PasteMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteMember.Click
        If gridMembers.PasteItem() Then
            m_xmlView.Updated = True
        End If
        PasteMember.Enabled = False
    End Sub
End Class