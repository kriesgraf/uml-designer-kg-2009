Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class dlgTypeVar
    Implements InterfFormDocument

    Private m_xmlView As XmlTypeView
    Private m_bValueEnabled As Boolean = True
    Private m_bInvalideCell As Boolean = False

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
        cmbValue.Enabled = False
        m_bValueEnabled = False
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
            Dim bOk As Boolean = False
            If m_xmlView.ConfirmCancel(bOk) Then
                If bOk Then
                    Me.Tag = True
                    Me.DialogResult = DialogResult.OK
                Else
                    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                    Me.Tag = m_xmlView.Updated
                End If
                Me.Close()
            End If
        End If
    End Sub

    Private Sub dlgTypeVar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If XmlProjectTools.DEBUG_COMMANDS_ACTIVE Then
            mnuEnumProperties.Visible = True
        End If

        Try
            With m_xmlView
                ' Load document values
                .LoadValues()

                ' Bind controls and document values
                .InitCheckBoxArray(chkArray)
                .InitBindingOption(optTypeArray)
                .InitBindingReference(chkReference)
                .InitBindingModifier(chkTypeConst)
                .InitBindingSize(cmbSize)
                .InitBindingTypedefs(cmbTypedefs)
                .InitBindingLevel(cmbTypeLevel, lblTypeLevel)
                .InitBindingValue(cmbValue)

                .UpdateOption(optTypeArray, gridEnumeration, Me)

                ' Load document values into controls
                .LoadEnumeration(gridEnumeration)

                Me.Text = m_xmlView.Name
            End With

            ' Enable/Disable controls according to document values
            ArrayChange()
            TypeChange()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub mnuDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDelete.Click
        gridEnumeration.DeleteSelectedItems(1)
    End Sub

    Private Sub mnuAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAdd.Click
        gridEnumeration.AddItem()
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "type")
    End Sub

    Private Sub optType_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles optType_0.Click, optType_1.Click
        TypeChange()
    End Sub

    Private Sub chkArray_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkArray.Click
        ArrayChange()
    End Sub

    Private Sub ArrayChange()
        If chkArray.Checked Then
            cmbValue.SelectedIndex = -1
            cmbValue.Enabled = False
            cmbSize.Enabled = True
        Else
            cmbValue.Enabled = m_bValueEnabled
            cmbSize.SelectedIndex = -1
            cmbSize.Enabled = False
        End If
    End Sub

    Private Sub TypeChange()

        If m_xmlView.CheckOption(optType_0.Checked, GroupVariable) _
        Then
            optType_0.Checked = False
            optType_1.Checked = True

        ElseIf optType_0.Checked _
        Then
            m_xmlView.Kind = XmlTypeVarSpec.EKindDeclaration.EK_SimpleType
            gridEnumeration.Binding.ResetBindings(True)
            Me.Height = 316
            cmbTypedefs.Enabled = True
            cmbTypeLevel.Enabled = True
            gridEnumeration.Enabled = False
            chkReference.Enabled = True
            chkTypeConst.Enabled = True
        Else
            m_xmlView.Kind = XmlTypeVarSpec.EKindDeclaration.EK_Enumeration
            Me.Height = 552
            cmbTypedefs.Enabled = False
            cmbTypeLevel.Enabled = False
            gridEnumeration.Enabled = True
            gridEnumeration.Binding.ResetBindings(True)
            chkReference.Enabled = False
            chkTypeConst.Enabled = False
        End If
    End Sub

    Private Sub mnuEnumProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuEnumProperties.Click
        dlgXmlNodeProperties.DisplayProperties(gridEnumeration.SelectedItem)
        m_xmlView.Updated = True
    End Sub

    Private Sub gridEnumeration_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridEnumeration.CellValidated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub gridEnumeration_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles gridEnumeration.CellValidating
        e.Cancel = IsInvalidVariableName(sender, e, Me.errorProvider)
        m_bInvalideCell = e.Cancel
    End Sub

    Private Sub gridEnumeration_RowValuesChanged(ByVal sender As Object) Handles gridEnumeration.RowValuesChanged
        ' TODO: for future use
    End Sub

    Private Sub cmbTypedefs_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTypedefs.Validated
        Me.errorProvider.SetError(sender, "")
    End Sub

    Private Sub cmbTypedefs_Validating(ByVal sender As ComboBox, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmbTypedefs.Validating
        e.Cancel = IsInvalidType(sender, Me.errorProvider)
    End Sub
End Class
