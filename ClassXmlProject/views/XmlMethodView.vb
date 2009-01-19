Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class XmlMethodView
    Inherits XmlMethodSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_xmlNodeManager As XmlNodeManager
    Private m_txtName As TextBox
    Private m_txtOperator As TextBox
    Private m_bInitProceed As Boolean = False

    Private cstTextBoxNameSize As Integer = 247

    Private WithEvents m_chkOperator As CheckBox

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

    Public Function ShowDialogCodeInline() As Boolean
        Dim bResult As Boolean = False
        Try
            If Me.CodeInline Is Nothing Then
                m_bCreateNodeNow = True
                ChangeReferences()
                m_bCreateNodeNow = False
                Me.CodeInline.SetDefaultValues()
            End If

            Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(Me.CodeInline)
            fen.Text = "Inline code"
            fen.ShowDialog()
            If CType(fen.Tag, Boolean) = True Then
                Me.Updated = True
                bResult = True
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Sub InitBindingName(ByVal dataControl As Windows.Forms.TextBox)
        m_txtName = dataControl
        dataControl.Width = cstTextBoxNameSize

        If Me.IsOperator = False Then
            dataControl.Visible = True
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")
        Else
            dataControl.Visible = False
        End If
    End Sub
    Public Sub InitBindingOperator(ByVal dataControl As Windows.Forms.TextBox)
        m_txtOperator = dataControl
        dataControl.Width = cstTextBoxNameSize

        If Me.IsOperator Then
            dataControl.Visible = True
            m_xmlBindingsList.AddBinding(dataControl, Me, "OperatorName")
        Else
            dataControl.Visible = False
        End If
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

    Public Sub InitBindingCheckOperator(ByVal dataControl As Windows.Forms.CheckBox)
        Try
            m_chkOperator = dataControl
            m_bInitProceed = True
            dataControl.Checked = Me.IsOperator

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

    Public Sub InitBindingRange(ByVal dataControl As ComboBox)

        dataControl.DropDownStyle = ComboBoxStyle.DropDownList
        UpdateRange(dataControl, False)
        m_xmlBindingsList.AddBinding(dataControl, Me.ReturnValue, "Range")
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

    Public Sub InitBindingCheckInline(ByVal dataControl As CheckBox, ByVal btnControl As Button)
        If Me.Tag <> ELanguage.Language_CplusPlus Then
            dataControl.Enabled = False
            dataControl.Visible = False
            btnControl.Enabled = False
            btnControl.Visible = False
        Else
            m_xmlBindingsList.AddBinding(dataControl, Me, "Inline", "Checked")
        End If
    End Sub

    Public Sub InitBindingImplementation(ByVal eCurrentClassImplementation As EImplementation, ByVal dataControl As ComboBox)
        Try
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Sorted = True

            If eCurrentClassImplementation = EImplementation.Unknown Then
                AddImplementation(-1, dataControl)
            Else
                AddImplementation(eCurrentClassImplementation, dataControl)
            End If

            m_xmlBindingsList.AddBinding(dataControl, Me, "ImplementationView", "SelectedItem")

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub InitBindingBehaviour(ByVal dataControl As Windows.Forms.ComboBox, ByVal label As Label)
        Try
            If Me.Tag <> ELanguage.Language_Vbasic Then
                dataControl.Enabled = False
                dataControl.Visible = False
                label.Visible = False
            Else
                dataControl.DropDownStyle = ComboBoxStyle.DropDownList

                dataControl.Items.AddRange(New Object() {"Normal", "Widening", "Narrowing", "Event", "Overloads", _
                                                         "Delegate", "Partial", "Shadows", "Overloads"})

                m_xmlBindingsList.AddBinding(dataControl, Me, "Behaviour", "SelectedItem")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateMember(ByVal dataControl As ComboBox, ByVal bChecked As Boolean)
        Try
            dataControl.Items.Clear()

            If Me.Tag = ELanguage.Language_Vbasic And bChecked Then

                dataControl.Items.Add("class")
            Else
                dataControl.Items.AddRange(New Object() {"object", "class"})
            End If

            dataControl.SelectedIndex = 0

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub


    Public Sub InitBindingMember(ByVal dataControl As ComboBox)
        Try
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList

            UpdateMember(dataControl, False)

            m_xmlBindingsList.AddBinding(dataControl, Me, "Member")

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub InitBindingModifier(ByVal dataControl As CheckBox)
        Try
            If Me.Tag <> ELanguage.Language_CplusPlus Then
                dataControl.Enabled = False
                dataControl.Visible = False
            Else
                m_xmlBindingsList.AddBinding(dataControl, Me, "Modifier", "Checked")
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

    Private Sub AddImplementation(ByVal iCurrent As Integer, ByVal combo As ComboBox)
        Dim eCurrentClassImpl As EImplementation
        If iCurrent = -1 Then
            eCurrentClassImpl = ConvertDtdToEnumImpl(GetAttribute("implementation", "parent::class"))
        Else
            eCurrentClassImpl = iCurrent
        End If
        Select Case eCurrentClassImpl
            Case EImplementation.Interf
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Interf, True))

            Case EImplementation.Node
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Interf, True))
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Root, True))
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Node, True))
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Leaf, True))
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Simple, True))

            Case EImplementation.Root
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Interf, True))
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Root, True))
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Simple, True))

            Case EImplementation.Leaf
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Interf, True))
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Leaf, True))
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Simple, True))

            Case EImplementation.Container
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Simple, True))

            Case Else
                combo.Items.Add(ConvertEnumImplToView(EImplementation.Simple, True))
        End Select
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub

    Private Sub m_chkOperator_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_chkOperator.Click

    End Sub
End Class
