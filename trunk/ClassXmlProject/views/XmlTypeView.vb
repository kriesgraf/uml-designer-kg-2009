Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports System.Collections
Imports ClassXmlProject.XmlNodeListView
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports Microsoft.VisualBasic

Public Class XmlTypeView
    Inherits XmlTypeVarSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_xmlComboTypedef As XmlBindingCombo
    Private m_xmlComboValue As XmlBindingCombo
    Private m_xmlComboSize As XmlBindingCombo
    Private m_SizeCheckBox As CheckBox
    Private m_ArrayRadioButtons As RadioButtonArray

    Public Overrides Property Name() As String
        Get
            If MyBase.Node.ParentNode.Name = "return" Then
                Return MyBase.GetAttribute("name", "ancestor::method")
            Else
                Return MyBase.GetAttribute("name", "parent::*")
            End If
        End Get
        Set(ByVal value As String)
            ' Read only here !
        End Set
    End Property

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
    End Sub

    Public Function ConfirmCancel(ByRef bOk As Boolean) As Boolean
        If MyBase.IsEnumWrong Then
            If MsgBox("Enumeration will be destroyed. Do you want to continue ?", _
                      cstMsgOkCancelCritical, "Simple type conversion") = MsgBoxResult.Ok _
            Then
                MyBase.Kind = EKindDeclaration.EK_SimpleType
                bOk = True
                Return True
            Else
                Return False
            End If
        End If
        Return True
    End Function

    Public Function UpdateValues() As Boolean
        Try
            m_xmlBindingsList.UpdateValues()
            m_xmlComboTypedef.Update()
            If m_ArrayRadioButtons.Item(0).Checked Then
                Me.Kind = EKindDeclaration.EK_SimpleType
            Else
                Me.Kind = EKindDeclaration.EK_Enumeration
            End If
            If m_SizeCheckBox.Enabled = False Then
                Me.Kind = EKindDeclaration.EK_Enumeration
            ElseIf m_SizeCheckBox.Checked = False Then
                Me.VarSize = ""
                If Me.Node.ParentNode.Name = "property" Or Me.Node.ParentNode.Name = "param" Then
                    m_xmlComboValue.Update()
                End If
            Else
                Me.Value = ""
                m_xmlComboSize.Update()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
            Return False
        End Try
        Return True
    End Function

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgTypeVar
        CType(frmResult, InterfFormDocument).Document = document
        If document.Node.ParentNode.Name = "typedef" Then
            CType(frmResult, InterfFormDocument).DisableMemberAttributes()
        End If
        Return frmResult
    End Function

    Public Sub InitCheckBoxArray(ByVal control As CheckBox)
        m_SizeCheckBox = control
        If MyBase.SizeRef Is Nothing And MyBase.VarSize Is Nothing Then
            control.Checked = False
        Else
            control.Checked = True
        End If
    End Sub

    Public Sub InitBindingOption(ByVal control As RadioButtonArray)
        Try
            m_ArrayRadioButtons = control
            If TestNode("enumvalue") Then
                control.Item(1).Checked = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function CheckOption(ByVal bSimpleType As Boolean, ByVal group As GroupBox) As Boolean
        If bSimpleType And Kind = EKindDeclaration.EK_Enumeration _
        Then
            If MsgBox("This operation is irreversible, would you want to continue ?" _
                      , cstMsgYesNoExclamation, "Delete 'Enumeration'") _
                            = MsgBoxResult.No _
            Then
                Return True
            End If
        End If

        If (bSimpleType And Kind = EKindDeclaration.EK_Enumeration) Or _
            (bSimpleType = False And Kind = EKindDeclaration.EK_SimpleType) _
            Then
            Me.Updated = True
        End If

        If bSimpleType _
        Then
            group.Enabled = True
            m_SizeCheckBox.Enabled = True

        ElseIf GetNode("parent::property") IsNot Nothing _
        Then
            group.Enabled = True
            m_SizeCheckBox.Enabled = False
        Else
            group.Enabled = False
        End If
        Return False
    End Function

    Public Sub UpdateOption(ByVal radioControl As RadioButtonArray, ByVal dataControl As XmlDataGridView, ByVal fen As Form)
        If Me.Tag = ELanguage.Language_Vbasic _
        Then
            If GetNode("ancestor::typedef") IsNot Nothing _
            Then
                radioControl.Item(0).Visible = False
                radioControl.Item(1).Visible = False
                radioControl.Item(1).Checked = True
            End If
        End If
    End Sub

    Public Sub InitBindingReference(ByVal control As Control)
        Try
            Dim bIsNotParam = (GetNode("parent::param") Is Nothing)

            If bIsNotParam And Me.Tag <> ELanguage.Language_CplusPlus Then
                control.Enabled = False
                control.Visible = False
            Else
                m_xmlBindingsList.AddBinding(control, Me, "By", "Checked")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingModifier(ByVal control As Control)
        Try
            Dim bIsNotProperty = (GetNode("parent::property") Is Nothing)

            If bIsNotProperty And Me.Tag <> ELanguage.Language_CplusPlus Then
                control.Enabled = False
                control.Visible = False
            Else
                m_xmlBindingsList.AddBinding(control, Me, "Modifier", "Checked")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingTypedefs(ByVal control As ComboBox)
        Try
            InitTypedefCombo(Me, control)
            m_xmlComboTypedef = New XmlBindingCombo(control, Me, "Descriptor", "Reference")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingLevel(ByVal control As ComboBox, ByVal label As Label)
        Try
            If Me.Tag <> ELanguage.Language_CplusPlus Then
                control.Enabled = False
                control.Visible = False
                label.Visible = False
            Else
                control.DropDownStyle = ComboBoxStyle.DropDownList
                control.Items.AddRange(New Object() {"Value", "Pointer", "Handler"})

                m_xmlBindingsList.AddBinding(control, Me, "Level", "SelectedIndex")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingSize(ByVal control As ComboBox)
        Try
            InitValueCombo(Me, control)

            m_xmlComboSize = New XmlBindingCombo(control, Me, "VarSize", "SizeRef")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingValue(ByVal control As ComboBox, Optional ByVal bValueOk As Boolean = False)
        Try
            If MyBase.ParentNodeName = "typedef" Then
                control.Enabled = False
            Else
                InitValueCombo(Me, control, (Me.GetNode("parent::property") IsNot Nothing))

                m_xmlComboValue = New XmlBindingCombo(control, Me, "Value", "ValRef")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub LoadEnumeration(ByVal grid As XmlDataGridView)
        Try
            grid.Binding.LoadXmlNodes(Me, "enumvalue", "type_enumvalue_view")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub New(Optional ByRef xmlnode As XmlNode = Nothing)
        MyBase.New(xmlnode, False)
        m_xmlBindingsList = New XmlBindingsList
        Try
            m_xmlNodeManager = XmlNodeManager.GetInstance()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
