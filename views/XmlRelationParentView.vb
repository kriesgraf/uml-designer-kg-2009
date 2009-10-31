Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlNodeListView
Imports ClassXmlProject.XmlProjectTools
Imports System.Xml
Imports System.Collections
Imports Microsoft.VisualBasic.Compatibility.VB6

Public Class XmlRelationParentView
    Inherits XmlRelationParentSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_xmlComboContainer As XmlBindingCombo
    Private m_xmlComboIndex As XmlBindingCombo
    Private m_ArrayRadioButtons As RadioButtonArray
    Private m_xmlComboSize As XmlBindingCombo

    Public Function CreateForm(ByVal document As XmlComponent) As Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgRelationParent
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance()
    End Sub

    Public Sub UpdateValues()
        If m_ArrayRadioButtons.Item(0).Checked Then
            If m_xmlComboSize IsNot Nothing Then m_xmlComboSize.Update()
        Else
            If m_xmlComboContainer IsNot Nothing Then m_xmlComboContainer.Update()
            If m_xmlComboIndex IsNot Nothing Then m_xmlComboIndex.Update()
        End If
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Sub InitBindingOption(ByVal control As RadioButtonArray)
        Try
            m_ArrayRadioButtons = control
            Select Case MyBase.Kind
                Case EKindParent.Array
                    control.Item(0).Checked = True
                Case EKindParent.Container
                    control.Item(1).Checked = True
                Case Else
                    Throw New Exception("Kind:=" + MyBase.Kind.ToString + " not allowed")
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingContainer(ByVal dataControl As ComboBox)
        InitContainerCombo(Me, dataControl, Me.Indexed)
        m_xmlComboContainer = New XmlBindingCombo(dataControl, Me, "ContainerDesc", "ContainerRef")
    End Sub

    Public Sub RefreshComboContainer(ByVal bIndexed As Boolean)
        InitContainerCombo(Me, m_xmlComboContainer.Control, bIndexed, True)
    End Sub

    Public Sub InitBindingArraySize(ByVal control As ComboBox)
        Try
            InitValueCombo(Me, control)
            m_xmlComboSize = New XmlBindingCombo(control, Me, "VarSize", "SizeRef")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingComboIndex(ByVal dataControl As ComboBox)
        InitTypedefCombo(Me, dataControl, EComboList.Type_index)
        m_xmlComboIndex = New XmlBindingCombo(dataControl, Me, "IndexDesc", "IndexRef")
    End Sub

    Public Sub InitBindingIndexLevel(ByVal dataControl As ComboBox, ByVal label As Label)
        If Me.GenerationLanguage <> ELanguage.Language_CplusPlus Then
            dataControl.Enabled = False
            dataControl.Visible = False
            label.Visible = False
        Else
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"Value", "Pointer", "Handle"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "IndexLevel", "SelectedIndex")
        End If
    End Sub

    Public Sub InitBindingIterator(ByVal dataControl As CheckBox)
        If Me.GenerationLanguage <> ELanguage.Language_CplusPlus Then
            dataControl.Enabled = False
            dataControl.Visible = False
        Else
            m_xmlBindingsList.AddBinding(dataControl, Me, "Iterator", "Checked")
        End If
    End Sub

    Public Sub InitBindingCheckIndex(ByVal dataControl As CheckBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Indexed", "Checked")
    End Sub

    Public Function CheckOption() As Boolean
        Dim bChanged As Boolean = True
        Try
            Select Case MyBase.Kind
                Case EKindParent.Container
                    If m_ArrayRadioButtons.Item(1).Checked Then bChanged = False
                Case EKindParent.Array
                    If m_ArrayRadioButtons.Item(0).Checked Then bChanged = False
                Case Else
                    Throw New Exception("Kind:=" + MyBase.Kind.ToString + " not allowed")
            End Select
        Catch ex As Exception
            Throw ex
        End Try

        Return bChanged
    End Function

    Public Sub ConfirmOption()
        If m_ArrayRadioButtons.Item(0).Checked Then
            MyBase.Kind = EKindParent.Array
            m_xmlComboSize.Control.SelectedText = "9999"
        Else
            MyBase.Kind = EKindParent.Container
            m_xmlComboContainer.Control.SelectedIndex = 0
        End If
    End Sub

    Public Sub CancelOption()
        InitBindingOption(m_ArrayRadioButtons)
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
