Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6

Public Class XmlPropertyView
    Inherits XmlPropertySpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_bInitOk As Boolean = False
    Private m_ArrayButton As RadioButtonArray
    Private m_eClassImplementation As EImplementation = EImplementation.Unknown

    Private m_cmbRange, m_cmbSetAccess, m_cmbGetAccess As ComboBox
    Private m_chkMember, m_chkGetInline, m_chkSetInline As CheckBox

    Private m_cmdBehaviour As New ComboCommand

    Private WithEvents m_chkAttribute As New CheckBox
    Private WithEvents m_chkOverridable As New CheckBox

    Public WriteOnly Property ClassImpl() As EImplementation
        Set(ByVal value As EImplementation)
            m_eClassImplementation = value
        End Set
    End Property

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As Form = Nothing
        If document.GetNode("descendant::list") IsNot Nothing Then
            frmResult = New dlgContainer
        ElseIf document.GetNode("descendant::element") IsNot Nothing Then
            frmResult = New dlgStructure
        Else
            frmResult = New dlgProperty
        End If
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance()
    End Sub

    Public Function UpdateValues(ByVal cmbGet As ComboBox, ByVal cmbSet As ComboBox, ByVal chkAttribute As CheckBox) As Boolean

        If chkAttribute.Checked = False _
        Then
            If CType(cmbGet.SelectedItem, String) = "no" And CType(cmbSet.SelectedItem, String) = "no" _
            Then
                MsgBox("Please choose an 'Attribute range' or a getter and/or setter", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        m_xmlBindingsList.UpdateValues()
        Return True
    End Function

    Public Sub InitBindingOption(ByVal control As RadioButtonArray)
        Try
            m_ArrayButton = control
            Select Case Me.TypeVarDefinition.Kind
                Case XmlTypeVarSpec.EKindDeclaration.EK_Container
                    control.Item(1).Checked = True
                Case XmlTypeVarSpec.EKindDeclaration.EK_Structure
                    control.Item(2).Checked = True
                Case XmlTypeVarSpec.EKindDeclaration.EK_Union
                    control.Item(2).Checked = True
                Case Else
                    control.Item(0).Checked = True
            End Select

            If OverridesProperty <> "" Or Me.Node.ParentNode.Name = "interface" Then
                For i As Short = 0 To CType(control.Count - 1, Short)
                    control.Item(i).Enabled = False
                Next i
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateOption(ByVal control As RadioButtonArray)
        If Me.Tag = ELanguage.Language_Vbasic Then
            control.Item(0).Text = "Simple"
            control.Item(1).Text = "Container"
            control.Item(2).Visible = False
        Else
            control.Item(2).Visible = True
        End If
    End Sub

    Public Function CheckOption() As Boolean
        Dim bChanged As Boolean = True

        Select Case TypeVarDefinition.Kind
            Case XmlTypeVarSpec.EKindDeclaration.EK_Container
                If m_ArrayButton.Item(1).Checked Then bChanged = False
            Case XmlTypeVarSpec.EKindDeclaration.EK_Structure
                If m_ArrayButton.Item(2).Checked Then bChanged = False
            Case XmlTypeVarSpec.EKindDeclaration.EK_Union
                If m_ArrayButton.Item(2).Checked Then bChanged = False
            Case Else
                If m_ArrayButton.Item(0).Checked Then bChanged = False
        End Select

        Return bChanged
    End Function

    Public Sub ConfirmOption()
        If m_ArrayButton.Item(0).Checked Then
            Me.TypeVarDefinition.Kind = XmlTypeVarSpec.EKindDeclaration.EK_SimpleType
        ElseIf m_ArrayButton.Item(1).Checked Then
            Me.TypeVarDefinition.Kind = XmlTypeVarSpec.EKindDeclaration.EK_Container
        Else
            Me.TypeVarDefinition.Kind = XmlTypeVarSpec.EKindDeclaration.EK_Structure
        End If
    End Sub

    Public Sub CancelOption()
        InitBindingOption(m_ArrayButton)
    End Sub

    Public Sub InitComplete()
        HandlingAttribute(m_chkAttribute, Nothing)
        HandlingOverridable(m_chkAttribute, Nothing)
    End Sub

    Public Sub InitBindingName(ByVal dataControl As TextBox, ByVal label As Label)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")

            If OverridesProperty <> "" Then
                label.Enabled = False
                dataControl.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingBriefComment(ByVal dataControl As TextBox)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingRange(ByVal dataControl As ComboBox)
        Try
            m_cmbRange = dataControl
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"private", "protected", "public"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "Range")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingMember(ByVal dataControl As CheckBox)
        Try
            m_chkMember = dataControl
            m_xmlBindingsList.AddBinding(dataControl, Me, "Member", "Checked")

            If Me.OverridesProperty <> "" _
            Then
                dataControl.Checked = False
                dataControl.Enabled = False
                dataControl.Visible = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingAttribute(ByVal dataControl As CheckBox)
        Try
            m_bInitOk = False
            m_chkAttribute = dataControl
            m_xmlBindingsList.AddBinding(dataControl, Me, "MemberAttribute", "Checked")
            Select m_eClassImplementation
                Case EImplementation.Interf
                    m_chkAttribute.Checked = False
                    m_chkAttribute.Enabled = False

                Case EImplementation.Root
                    If Me.Node.ParentNode.Name = "interface" Then
                        m_chkAttribute.Checked = False
                        m_chkAttribute.Enabled = False
                    End If
            End Select
        Catch ex As Exception
            Throw ex
        Finally
            m_bInitOk = True
        End Try
    End Sub

    Public Sub InitBindingGetModifier(ByVal dataControl As CheckBox)
        Try
            dataControl.ThreeState = False
            m_xmlBindingsList.AddBinding(dataControl, Me, "AccessGetModifier", "Checked")

            If Me.Tag = ELanguage.Language_Vbasic _
            Then
                dataControl.Checked = False
                dataControl.Enabled = False
                dataControl.Visible = False

            ElseIf Me.OverridesProperty <> "" _
            Then
                dataControl.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingGetInline(ByVal dataControl As CheckBox)
        Try
            m_chkGetInline = dataControl
            dataControl.ThreeState = False
            m_xmlBindingsList.AddBinding(dataControl, Me, "AccessGetInline", "Checked")

            If Me.Tag = ELanguage.Language_Vbasic _
            Then
                dataControl.Checked = False
                dataControl.Enabled = False
                dataControl.Visible = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingSetInline(ByVal dataControl As CheckBox)
        Try
            m_chkSetInline = dataControl
            dataControl.ThreeState = False
            m_xmlBindingsList.AddBinding(dataControl, Me, "AccessSetInline", "Checked")

            If Me.Tag = ELanguage.Language_Vbasic _
            Then
                dataControl.Checked = False
                dataControl.Enabled = False
                dataControl.Visible = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingOverridable(ByVal control As CheckBox)
        Try
            m_bInitOk = False
            m_chkOverridable = control

            If m_eClassImplementation = EImplementation.Unknown Then
                m_eClassImplementation = ConvertDtdToEnumImpl(GetAttribute("implementation", "parent::class"))
            End If

            m_xmlBindingsList.AddBinding(control, Me, "OverridableProperty", "Checked")

            Select Case m_eClassImplementation
                Case EImplementation.Interf
                    control.Checked = True
                    control.Enabled = False

                Case EImplementation.Root
                    If Me.Node.ParentNode.Name = "interface" Then
                        control.Checked = True
                        control.Enabled = False
                    End If
                    ' Ignore

                Case EImplementation.Node
                    ' Ignore

                Case EImplementation.Unknown
                    Throw New Exception("Current class implementation is not provided")

                Case Else
                    control.Checked = False
                    control.Enabled = False
            End Select

        Catch ex As Exception
            Throw ex
        Finally
            m_bInitOk = True
        End Try
    End Sub

    Public Sub InitBindingGetAccess(ByVal dataControl As ComboBox, ByVal label As Label)
        Try
            m_bInitOk = False
            m_cmbGetAccess = dataControl
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"no", "protected", "public"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "AccessGetRange", "SelectedItem")

            If Me.OverridesProperty <> "" _
            Then
                dataControl.Enabled = False
                label.Enabled = False
            End If

        Catch ex As Exception
            Throw ex
        Finally
            m_bInitOk = True
        End Try
    End Sub

    Public Sub InitBindingGetBy(ByVal dataControl As ComboBox, ByVal label As Label)
        Try
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"ref", "val"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "AccessGetBy", "SelectedItem")

            If Me.Tag <> ELanguage.Language_CplusPlus _
            Then
                dataControl.Enabled = False
                dataControl.Visible = False
                label.Visible = False

            ElseIf Me.OverridesProperty <> "" _
            Then
                dataControl.Enabled = False
                label.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingSetAccess(ByVal dataControl As ComboBox, ByVal label As Label)
        Try
            m_bInitOk = False
            m_cmbSetAccess = dataControl
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"no", "protected", "public"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "AccessSetRange", "SelectedItem")

            If Me.OverridesProperty <> "" _
            Then
                dataControl.Enabled = False
                label.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
        Finally
            m_bInitOk = True
        End Try
    End Sub

    Public Sub InitBindingSetby(ByVal dataControl As ComboBox, ByVal label As Label)
        Try
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"ref", "val"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "AccessSetBy", "SelectedItem")

            If Me.Tag <> ELanguage.Language_CplusPlus _
            Then
                dataControl.Enabled = False
                dataControl.Visible = False
                label.Visible = False

            ElseIf Me.OverridesProperty <> "" _
            Then
                dataControl.Enabled = False
                label.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingBehaviour(ByVal dataControl As ComboBox, ByVal label As Label)
        Try
            m_cmdBehaviour.Combo = dataControl
            m_cmdBehaviour.Title = label

            With m_cmdBehaviour
                .Combo.DropDownStyle = ComboBoxStyle.DropDownList

                .Combo.Items.AddRange(New Object() {"Normal", "Default", "WithEvents"})
                m_xmlBindingsList.AddBinding(dataControl, Me, "Behaviour", "SelectedItem")

                If Me.Tag <> ELanguage.Language_Vbasic _
                Then
                    .Combo.SelectedIndex = 0
                    .Enabled = False
                    .Visible = False

                ElseIf m_eClassImplementation = EImplementation.Interf _
                Then
                    .Combo.SelectedIndex = 0
                    .Enabled = False

                ElseIf m_eClassImplementation = EImplementation.Root And Me.Node.ParentNode.Name = "interface" _
                Then
                    .Combo.SelectedIndex = 0
                    .Enabled = False

                ElseIf Me.OverridesProperty <> "" _
                Then
                    .Combo.SelectedIndex = 0
                    .Enabled = False
                End If
            End With
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub HandlingAttribute(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_chkAttribute.CheckedChanged
        If m_bInitOk = False Then Exit Sub

        If m_chkAttribute.Checked _
        Then
            m_cmbRange.Enabled = True
            m_chkGetInline.Enabled = (CType(m_cmbGetAccess.SelectedItem, String) <> "no")
            m_chkSetInline.Enabled = (CType(m_cmbSetAccess.SelectedItem, String) <> "no")
        Else
            m_cmbRange.SelectedIndex = 0
            m_cmbRange.Enabled = False
            m_chkGetInline.Enabled = False
            m_chkSetInline.Enabled = False
            m_chkGetInline.Checked = (CType(m_cmbGetAccess.SelectedItem, String) <> "no")
            m_chkSetInline.Checked = (CType(m_cmbSetAccess.SelectedItem, String) <> "no")
        End If
    End Sub

    Private Sub HandlingOverridable(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_chkOverridable.CheckedChanged
        If m_bInitOk = False Then Exit Sub

        If m_chkOverridable.Checked _
        Then
            If m_eClassImplementation = EImplementation.Interf _
            Then
                m_chkAttribute.Checked = False
                m_chkAttribute.Enabled = False
                m_cmbRange.SelectedIndex = 0
                m_cmbRange.Enabled = False
            End If

            m_chkMember.Checked = False
            m_chkMember.Enabled = False
        Else
            m_chkAttribute.Enabled = True
            m_chkMember.Enabled = True
        End If
    End Sub
End Class
