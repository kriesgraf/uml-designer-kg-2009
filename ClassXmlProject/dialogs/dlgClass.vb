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
    Private m_bInvalideCell As Boolean = False

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

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView("class")
        m_toolTip = New ToolTip

    End Sub

#End Region

#Region "Private methods"

    Private Sub dlgClass_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If DEBUG_COMMANDS_ACTIVE Then
            InheritsProperties.Visible = True
            MemberProperties.Visible = True
            DependencyProperties.Visible = True
            RelationProperties.Visible = True
        End If
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
                ' Combo Implementation is used by several methods, reference must be set previously
                .InitBindingImplementation(lblImplementation, cmbImplementation)
                .UpdateMenuConstructor(AddConstructor)
                .InitBindingConstructor(lblConstructor, cmbConstructor)
                .InitBindingDestructor(lblDestructor, cmbDestructor)
                .InitBindingVisibility(cmbVisibility)
                .InitBindingInline(lblInline, cmbModelInline)
                .InitBindingPartial(chkPartial)
                ' Load document values into controls
                .LoadDependencies(gridDependencies)
                .LoadInheritedMembers(gridInherited)
                .LoadMembers(gridMembers)
                .LoadRelations(gridRelations)

                .UpdateMenuClass(AddTypedef)
            End With

            PasteMember.Enabled = XmlComponent.Clipboard.CanPaste
            Me.WindowState = FormWindowState.Maximized

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
        If m_bInvalideCell = False Then
            Me.Tag = m_xmlView.Updated
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox(m_xmlView.Name + " will be deleted, please confirm ?", cstMsgYesNoQuestion, "'Delete' command") _
            = MsgBoxResult.Yes Then
            If m_xmlView.RemoveMe() Then
                Me.Tag = True
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub cmbModelInline_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbModelInline.SelectedIndexChanged

        m_xmlView.MustCheckTemplate()
    End Sub

    Private Sub txtBrief_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBrief.TextChanged
        m_toolTip.SetToolTip(txtBrief, txtBrief.Text)
    End Sub

    Private Sub mnuDeleteSuperClass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteSuperClass.Click
        gridInherited.DeleteSelectedItems()
    End Sub

    Private Sub mnuDeleteMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMember.Click
        gridMembers.DeleteSelectedItems()
    End Sub

    Private Sub mnuDeleteDependency_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteDependency.Click
        gridDependencies.DeleteSelectedItems()
    End Sub

    Private Sub mnuDeleteRelation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteRelation.Click
        gridRelations.DeleteSelectedItems()
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
    End Sub

    Private Sub mnuAddRelation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddRelation.Click
        gridRelations.AddItem()
    End Sub

    Private Sub mnuAddMember_Click(ByVal sender As ToolStripMenuItem, ByVal e As System.EventArgs) _
                Handles AddTypedef.Click, AddProperty.Click, AddMethod.Click, AddConstructor.Click, _
                        AddStructure.Click, AddContainer.Click

        gridMembers.AddItem(CType(sender.Tag, String))
    End Sub

    Private Sub mnuEditMember_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditMember.Click
        gridMembers.EditCurrentItem()
    End Sub

    Private Sub mnuEditRelation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditRelation.Click
        gridRelations.EditCurrentItem()
    End Sub

    Private Sub mnuInheritsProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InheritsProperties.Click
        dlgXmlNodeProperties.DisplayProperties(gridInherited.SelectedItem)
        m_xmlView.Updated = True
        gridMembers.Binding.ResetBindings(False)
    End Sub

    Private Sub mnuDependencyProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DependencyProperties.Click
        dlgXmlNodeProperties.DisplayProperties(gridDependencies.SelectedItem)
        gridMembers.Binding.ResetBindings(False)
        m_xmlView.Updated = True
    End Sub

    Private Sub mnuMemberProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemberProperties.Click
        dlgXmlNodeProperties.DisplayProperties(CType(gridMembers.SelectedItem, XmlComponent))
        m_xmlView.Updated = True
        gridMembers.Binding.ResetBindings(False)
    End Sub

    Private Sub mnuRelationProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RelationProperties.Click
        dlgXmlNodeProperties.DisplayProperties(gridRelations.SelectedItem)
        m_xmlView.Updated = True
        gridMembers.Binding.ResetBindings(False)
    End Sub

    Private Sub mnuMemberDependencies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMemberDependencies.Click
        If m_xmlView.SearchDependencies(CType(gridMembers.SelectedItem, XmlComponent)) Then
            gridMembers.Binding.ResetBindings(True)
        End If
    End Sub

    Private Sub Grids_CellValidated(ByVal sender As XmlDataGridView, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
            Handles gridMembers.CellValidated, gridRelations.CellValidated

        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub Grids_CellValidating(ByVal sender As XmlDataGridView, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) _
            Handles gridMembers.CellValidating, gridRelations.CellValidating

        If sender.Name = "gridMembers" Then
            e.Cancel = IsInvalidVariableName(sender, e, Me.errorProvider)
        Else
            e.Cancel = IsInvalidVariableName(sender, e, Me.errorProvider, ErrorIconAlignment.TopRight)
        End If
        m_bInvalideCell = e.Cancel
    End Sub

    Private Sub GridRowValuesChanged(ByVal sender As Object) _
                Handles gridDependencies.RowValuesChanged, _
                gridInherited.RowValuesChanged, _
                gridMembers.RowValuesChanged, _
                gridRelations.RowValuesChanged

        ' TODO: for future use
    End Sub

    Private Sub mnuOverrideProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOverrideProperties.Click
        If m_xmlView.OverrideProperties(m_xmlView.CurrentClassImpl) _
        Then
            gridMembers.Binding.ResetBindings(True)
            m_xmlView.Updated = True
        Else
            MsgBox("No properties to override!", MsgBoxStyle.Exclamation, "'Override' command")
        End If
    End Sub

    Private Sub mnuOverrideMethods_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuOverrideMethods.Click
        If m_xmlView.OverrideMethods(m_xmlView.CurrentClassImpl) _
        Then
            gridMembers.Binding.ResetBindings(True)
            m_xmlView.Updated = True
        Else
            MsgBox("No methods to override!", MsgBoxStyle.Exclamation, "'Override' command")
        End If
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

    Private Sub txtName_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.Validated
        Me.errorProvider.SetError(txtName, "")
    End Sub

    Private Sub txtName_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtName.Validating
        e.Cancel = IsInvalidVariableName(txtName, errorProvider)
    End Sub

    Private Sub RemoveRedundancies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveRedundancies.Click
        m_xmlView.RemoveRedundant(CType(gridMembers.SelectedItem, XmlComponent))
    End Sub

    Private Sub mnuExportReferences_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportReferences.Click

    End Sub
#End Region
End Class
