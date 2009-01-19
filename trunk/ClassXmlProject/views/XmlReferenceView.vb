Public Class XmlReferenceView
    Inherits XmlReferenceSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_xmlNodeManager As XmlNodeManager

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance()
    End Sub

    Public Sub UpdateValues()
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgReference
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingName(ByVal dataControl As Windows.Forms.Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingParentClass(ByVal dataControl As Windows.Forms.Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "ParentClass")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingPackage(ByVal dataControl As Windows.Forms.Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Package")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingContainer(ByVal combo As Windows.Forms.ComboBox)
        Try
            combo.DropDownStyle = ComboBoxStyle.DropDownList
            combo.Items.AddRange(New Object() {"none", "simple", "indexed"})

            m_xmlBindingsList.AddBinding(combo, Me, "Container", "SelectedIndex")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingType(ByVal combo As Windows.Forms.ComboBox)
        Try
            combo.DropDownStyle = ComboBoxStyle.DropDownList
            combo.Sorted = True
            combo.Items.AddRange(New Object() {"class", "typedef", "exception"})

            m_xmlBindingsList.AddBinding(combo, Me, "Kind", "SelectedItem")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
