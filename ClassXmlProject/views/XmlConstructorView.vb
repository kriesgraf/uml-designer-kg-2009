Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator

Public Class XmlConstructorView
    Inherits XmlConstructorSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList

    Public Overrides Property Name() As String
        Get
            Return MyBase.GetAttribute("name", "parent::class")
        End Get
        Set(ByVal value As String)
            ' Read only here !
        End Set
    End Property

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
    End Sub

    Public Sub UpdateValues()
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As Form = Nothing
        frmResult = New dlgConstructor
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingRange(ByVal dataControl As ComboBox)
        dataControl.DropDownStyle = ComboBoxStyle.DropDownList
        dataControl.Items.AddRange(New Object() {"private", "protected", "public"})
        m_xmlBindingsList.AddBinding(dataControl, Me, "Range")
    End Sub

    Public Sub InitBindingBrief(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "BriefComment")
    End Sub

    Public Sub InitBindingComment(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")
    End Sub

    Public Sub InitBindingCheckCopy(ByVal dataControl As CheckBox)
        If Me.GenerationLanguage <> ELanguage.Language_CplusPlus Then
            dataControl.Enabled = False
            dataControl.Visible = False
        Else
            dataControl.Checked = MyBase.CheckCopyConstructor()
        End If
    End Sub

    Public Sub InitBindingCheckInline(ByVal dataControl As CheckBox)
        If Me.GenerationLanguage <> ELanguage.Language_CplusPlus Then
            dataControl.Visible = False
        Else
            m_xmlBindingsList.AddBinding(dataControl, Me, "Inline", "Checked")
        End If
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

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
