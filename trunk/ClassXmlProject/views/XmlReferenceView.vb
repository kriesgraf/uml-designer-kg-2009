Imports System
Imports System.Windows.Forms

Public Class XmlReferenceView
    Inherits XmlReferenceSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance()
    End Sub

    Public Sub UpdateValues()
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm

        Dim frmResult As Form
        Dim reference As XmlReferenceSpec = CreateDocument(document.Node)

        If reference.RefKind = EReferenceKind.Enumeration _
        Then
            frmResult = New dlgInterface
        Else
            frmResult = New dlgReference
        End If

        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingName(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingParentClass(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "ParentClass")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingPackage(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Package")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingContainer(ByVal combo As ComboBox)
        Try
            combo.DropDownStyle = ComboBoxStyle.DropDownList
            combo.Items.AddRange(New Object() {"none", "simple", "indexed", "object-based"})

            m_xmlBindingsList.AddBinding(combo, Me, "Container", "SelectedIndex")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub CancelEnum(ByVal combo As ComboBox)
        combo.SelectedIndex = 0
    End Sub

    Public Sub ConfirmEnum()
        Me.Kind = "typedef"
        Dim element As XmlEnumSpec = CreateDocument("enumvalue", Me.Document)
        element.Tag = Me.Tag
        AppendComponent(element)
    End Sub

    Public Sub InitBindingType(ByVal combo As ComboBox)
        Try
            combo.DropDownStyle = ComboBoxStyle.DropDownList
            combo.Sorted = True

            If Me.GetNode("enumvalue") IsNot Nothing Then
                combo.Items.Add("enumeration")
                combo.Enabled = False
                combo.SelectedItem = "enumeration"
            Else
                combo.Items.AddRange(New Object() {"class", "typedef", "enumeration", "exception"})
                m_xmlBindingsList.AddBinding(combo, Me, "Kind", "SelectedItem")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
