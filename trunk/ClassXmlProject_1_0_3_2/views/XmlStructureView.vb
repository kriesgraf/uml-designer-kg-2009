Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator

Public Class XmlStructureView
    Inherits XmlStructureSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_xmlComboTypedef As XmlBindingCombo
    Private m_xmlComboContainer As XmlBindingCombo
    Private m_xmlComboIndex As XmlBindingCombo

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As Form = Nothing
        If document.GetNode("descendant::list") IsNot Nothing Then
            frmResult = New dlgContainer
        ElseIf document.GetNode("descendant::element") IsNot Nothing Then
            frmResult = New dlgStructure
        Else
            frmResult = New dlgTypedef
        End If
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance()
    End Sub

    Public Sub LoadElementMembers(ByVal dataControl As XmlDataGridView)
        Try
            dataControl.Binding.LoadXmlNodes(Me.TypeVarDefinition, "element", "type_element_view")
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub UpdateValues()
        If m_xmlComboTypedef IsNot Nothing Then m_xmlComboTypedef.Update()
        If m_xmlComboContainer IsNot Nothing Then m_xmlComboContainer.Update()
        If m_xmlComboIndex IsNot Nothing Then m_xmlComboIndex.Update()
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Sub InitBindingRange(ByVal dataControl As ComboBox)
        dataControl.DropDownStyle = ComboBoxStyle.DropDownList
        dataControl.Items.AddRange(New Object() {"private", "protected", "public"})
        m_xmlBindingsList.AddBinding(dataControl, Me, "Range")
    End Sub

    Public Sub InitBindingBrief(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")
    End Sub

    Public Sub InitBindingName(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Name")
    End Sub

    Public Sub InitBindingUnion(ByVal dataControl As CheckBox)
        If Me.GenerationLanguage <> ELanguage.Language_CplusPlus Then
            dataControl.Enabled = False
            dataControl.Visible = False
        Else
            m_xmlBindingsList.AddBinding(dataControl, Me, "Union", "Checked")
        End If
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
