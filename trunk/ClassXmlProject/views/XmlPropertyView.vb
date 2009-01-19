Imports ClassXmlProject.UmlCodeGenerator
Imports Microsoft.VisualBasic.Compatibility.VB6

Public Class XmlPropertyView
    Inherits XmlPropertySpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_xmlNodeManager As XmlNodeManager
    Private m_ArrayButton As RadioButtonArray

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

    Public Sub UpdateValues()
        m_xmlBindingsList.UpdateValues()
    End Sub

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

    Public Sub InitBindingName(ByVal dataControl As Windows.Forms.TextBox)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingBriefComment(ByVal dataControl As Windows.Forms.TextBox)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingRange(ByVal dataControl As Windows.Forms.ComboBox)
        Try
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"private", "protected", "public"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "Range")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingMember(ByVal dataControl As Windows.Forms.ComboBox)
        Try
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"object", "class"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "Member")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingGetModifier(ByVal dataControl As Windows.Forms.CheckBox)
        Try
            If Me.Tag <> ELanguage.Language_CplusPlus Then
                dataControl.Enabled = False
                dataControl.Visible = False
            Else
                dataControl.ThreeState = False
                m_xmlBindingsList.AddBinding(dataControl, Me, "AccessGetModifier", "Checked")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingGetAccess(ByVal dataControl As Windows.Forms.ComboBox)
        Try
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"no", "protected", "public"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "AccessGetRange", "SelectedItem")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingGetBy(ByVal dataControl As Windows.Forms.ComboBox, ByVal label As Label)
        Try
            If Me.Tag <> ELanguage.Language_CplusPlus Then
                dataControl.Enabled = False
                dataControl.Visible = False
                label.Visible = False
            Else
                dataControl.DropDownStyle = ComboBoxStyle.DropDownList
                dataControl.Items.AddRange(New Object() {"ref", "val"})
                m_xmlBindingsList.AddBinding(dataControl, Me, "AccessGetBy", "SelectedItem")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingSetAccess(ByVal dataControl As Windows.Forms.ComboBox)
        Try
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"no", "protected", "public"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "AccessSetRange", "SelectedItem")
        Catch ex As Exception
            Throw ex
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
                dataControl.Items.AddRange(New Object() {"Normal", "Default", "WithEvents"})
                m_xmlBindingsList.AddBinding(dataControl, Me, "Behaviour", "SelectedItem")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingSetby(ByVal dataControl As Windows.Forms.ComboBox, ByVal label As Label)
        Try
            If Me.Tag <> ELanguage.Language_CplusPlus Then
                dataControl.Enabled = False
                dataControl.Visible = False
                label.Visible = False
            Else
                dataControl.DropDownStyle = ComboBoxStyle.DropDownList
                dataControl.Items.AddRange(New Object() {"ref", "val"})
                m_xmlBindingsList.AddBinding(dataControl, Me, "AccessSetBy", "SelectedItem")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
