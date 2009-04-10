Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class XmlMethodView
    Inherits XmlMethodSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    'Private m_xmlNodeManager As XmlNodeManager
    Private m_txtName As TextBox
    Private m_txtOperator As TextBox
    Private m_bInitProceed As Boolean = False
    Private m_eClassImplementation As EImplementation = EImplementation.Unknown

    Private cstTextBoxNameSize As Integer = 247

    Private m_cmdMember As New ComboCommand
    Private m_cmdBehaviour As New ComboCommand

    Private WithEvents m_cmbImplementation As ComboBox
    Private WithEvents m_chkOperator As CheckBox

    Public Property ClassImpl() As EImplementation
        Get
            Return m_eClassImplementation
        End Get
        Set(ByVal value As EImplementation)
            m_eClassImplementation = value
        End Set
    End Property

    Public Property ImplementationView() As String
        Get
            Return ConvertEnumImplToView(MyBase.Implementation, True)
        End Get
        Set(ByVal value As String)
            MyBase.Implementation = ConvertViewToEnumImpl(value)
        End Set
    End Property

    Public ReadOnly Property IsOperator() As Boolean
        Get
            Return (MyBase.OperatorName <> "")
        End Get
    End Property

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
    End Sub

    Public Sub UpdateValues()
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As Form = Nothing
        If document.GetAttribute("constructor") <> "no" Then
            frmResult = New dlgConstructor
        Else
            frmResult = New dlgMethod
        End If
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingName(ByVal dataControl As TextBox, ByVal label As Label)
        m_txtName = dataControl
        dataControl.Width = cstTextBoxNameSize

        If Me.IsOperator = False Then
            dataControl.Visible = True
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")
        Else
            dataControl.Visible = False
        End If

        If Me.OverridesMethod <> "" Then
            dataControl.Enabled = False
            label.Enabled = False
        End If
    End Sub

    Public Sub InitBindingOperator(ByVal dataControl As TextBox)
        m_txtOperator = dataControl
        dataControl.Width = cstTextBoxNameSize

        If Me.IsOperator Then
            dataControl.Visible = True
            m_xmlBindingsList.AddBinding(dataControl, Me, "OperatorName")
        Else
            dataControl.Visible = False
        End If

        If Me.OverridesMethod <> "" Then
            dataControl.Enabled = False
        End If
    End Sub

    Private Sub HandleBindingImplementation(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_cmbImplementation.SelectedIndexChanged
        If m_bInitProceed Then Exit Sub

        Dim strImplementation As String = CType(m_cmbImplementation.SelectedItem, String)

        Select Case ConvertViewToEnumImpl(strImplementation)

            Case EImplementation.Interf, EImplementation.Leaf, EImplementation.Node, EImplementation.Root
                With m_cmdMember
                    .Combo.SelectedIndex = 0
                    .Enabled = False
                End With

                If Me.Tag = ELanguage.Language_Vbasic Then
                    With m_cmdBehaviour
                        .Combo.SelectedIndex = 0
                        .Enabled = False
                    End With
                End If

            Case Else
                If OverridesMethod = "" Then
                    m_cmdMember.Enabled = True
                    m_cmdBehaviour.Enabled = True
                Else
                    m_cmdMember.Enabled = False
                    m_cmdBehaviour.Enabled = False
                End If
        End Select
    End Sub

    Private Sub HandleBindingOperator(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_chkOperator.Click
        If m_bInitProceed Then Exit Sub

        If m_chkOperator.Checked Then
            m_txtName.Visible = False
            m_xmlBindingsList.RemoveBinding(m_txtName)
            m_txtOperator.Visible = True
            m_xmlBindingsList.AddBinding(m_txtOperator, Me, "OperatorName")
        Else
            m_txtOperator.Visible = False
            m_xmlBindingsList.RemoveBinding(m_txtOperator)
            m_txtName.Visible = True
            m_xmlBindingsList.AddBinding(m_txtName, Me, "Name")
        End If
    End Sub

    Public Sub InitBindingCheckOperator(ByVal dataControl As CheckBox)
        Try
            m_chkOperator = dataControl
            m_bInitProceed = True
            dataControl.Checked = Me.IsOperator

            If Me.OverridesMethod <> "" Then
                dataControl.Enabled = False
            End If

        Catch ex As Exception
            Throw ex
        Finally
            m_bInitProceed = False
        End Try
    End Sub

    Public Sub UpdateOperatorDisplay(ByVal txtName As TextBox, ByVal txtOperator As TextBox, _
                                     ByVal chkBox As CheckBox, ByVal FlowLayout As FlowLayoutPanel, _
                                     ByVal bInitProceed As Boolean)

        Dim iWidth As Integer = FlowLayout.ClientSize.Width - 10 - FlowLayout.Margin.Size.Width

        If chkBox.Checked Then

            txtName.Text = "operator"
            txtName.Visible = False
            txtOperator.Visible = True

        ElseIf bInitProceed = False Then

            txtName.Width = iWidth - chkBox.Width
            txtName.Text = "to_complete"
            txtName.Visible = True
            txtOperator.Visible = False
        End If
    End Sub

    Public Sub UpdateMenu(ByVal item As ToolStripItem)
        If Me.Tag = ELanguage.Language_Vbasic Then
            item.Visible = False
        End If
    End Sub

    Public Sub UpdateRange(ByVal dataControl As ComboBox, ByVal bChecked As Boolean)
        Try
            dataControl.Items.Clear()

            If Me.Tag = ELanguage.Language_Vbasic And bChecked Then
                dataControl.Items.Add("public")
            Else
                dataControl.Items.AddRange(New Object() {"private", "protected", "public"})
            End If

            dataControl.SelectedIndex = 0

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub InitBindingRange(ByVal dataControl As ComboBox, ByVal label As Label)
        Try

            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            UpdateRange(dataControl, False)
            m_xmlBindingsList.AddBinding(dataControl, Me.ReturnValue, "Range")

            If OverridesMethod <> "" Then
                dataControl.Enabled = False
                label.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingBrief(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "BriefComment")
    End Sub

    Public Sub InitBindingReturnComment(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "ReturnComment")
    End Sub

    Public Sub InitBindingComment(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")
    End Sub

    Public Sub InitBindingCheckInline(ByVal dataControl As CheckBox)

        If Me.Tag <> ELanguage.Language_CplusPlus _
            Or m_eClassImplementation = EImplementation.Interf _
            Or Me.Node.ParentNode.Name = "interface" _
        Then
            dataControl.Visible = False
        Else
            m_xmlBindingsList.AddBinding(dataControl, Me, "Inline", "Checked")
        End If
    End Sub

    Public Sub InitBindingImplementation(ByVal dataControl As ComboBox, ByVal label As Label)
        Try
            m_cmbImplementation = dataControl

            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Sorted = True

            AddImplementation(dataControl)
            m_xmlBindingsList.AddBinding(dataControl, Me, "ImplementationView", "SelectedItem")

            If dataControl.Items.Count = 1 Then
                dataControl.SelectedIndex = 0
                dataControl.Enabled = False
                label.Enabled = False
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub InitBindingBehaviour(ByVal dataControl As ComboBox, ByVal label As Label)
        Try
            m_cmdBehaviour.Combo = dataControl
            m_cmdBehaviour.Title = label

            If Me.Tag <> ELanguage.Language_Vbasic Then
                m_cmdBehaviour.Enabled = False
                m_cmdBehaviour.Visible = False
            Else
                dataControl.DropDownStyle = ComboBoxStyle.DropDownList

                dataControl.Items.AddRange(New Object() {"Normal", "Widening", "Narrowing", "Event", "Overloads", _
                                                         "Delegate", "Partial", "Shadows"})

                m_xmlBindingsList.AddBinding(dataControl, Me, "Behaviour", "SelectedItem")

                If OverridesMethod <> "" Then
                    m_cmdBehaviour.Enabled = False
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingMember(ByVal dataControl As ComboBox, ByVal label As Label)
        Try
            m_cmdMember.Combo = dataControl
            m_cmdMember.Title = label

            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"object", "class"})

            m_xmlBindingsList.AddBinding(dataControl, Me, "Member")

            If OverridesMethod <> "" Then
                m_cmdMember.Enabled = False
                dataControl.SelectedIndex = 0
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub InitBindingModifier(ByVal dataControl As CheckBox)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Modifier", "Checked")

            If Me.Tag <> ELanguage.Language_CplusPlus Then
                dataControl.Checked = False
                dataControl.Enabled = False
                dataControl.Visible = False
            End If

            If OverridesMethod <> "" Then
                dataControl.Enabled = False
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub LoadParamMembers(ByVal dataControl As XmlDataGridView)
        Try
            If MyBase.Kind = EKindMethod.EK_Constructor Then
                dataControl.Binding.LoadXmlNodes(Me, "param", "method_member_view")
            Else
                dataControl.Binding.LoadXmlNodes(Me, "exception | param", "method_member_view")
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub AddImplementation(ByVal combo As ComboBox)

        If m_eClassImplementation = EImplementation.Unknown Then
            m_eClassImplementation = ConvertDtdToEnumImpl(GetAttribute("implementation", "parent::class"))
        End If

        Select Case m_eClassImplementation
            Case EImplementation.Interf
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Interf, True))

            Case EImplementation.Node
                If OverridesMethod = "" Then
                    combo.Items.Add(ConvertEnumImplToView(EImplementation.Interf, True))
                    combo.Items.Add(ConvertEnumImplToView(EImplementation.Root, True))
                End If

                combo.Items.Add(ConvertEnumImplToView(EImplementation.Node, True))
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Leaf, True))

                If OverridesMethod = "" Then
                    combo.Items.Add(ConvertEnumImplToView(EImplementation.Simple, True))
                End If

            Case EImplementation.Root
                If OverridesMethod = "" Then
                    combo.Items.Add(ConvertEnumImplToView(EImplementation.Interf, True))
                End If

                combo.Items.Add(ConvertEnumImplToView(EImplementation.Root, True))

                If OverridesMethod <> "" _
                Then
                    combo.Items.Add(ConvertEnumImplToView(EImplementation.Node, True))
                    combo.Items.Add(ConvertEnumImplToView(EImplementation.Leaf, True))

                ElseIf Me.Node.ParentNode.Name <> "interface" _
                Then
                    combo.Items.Add(ConvertEnumImplToView(EImplementation.Simple, True))
                End If

            Case EImplementation.Leaf
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Leaf, True))

                If OverridesMethod = "" Then
                    combo.Items.Add(ConvertEnumImplToView(EImplementation.Simple, True))
                End If

            Case EImplementation.Container
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Simple, True))
                combo.SelectedIndex = 0

            Case EImplementation.Unknown
                Throw New Exception("Current class implementation is not provided")

            Case Else
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Simple, True))
        End Select
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
