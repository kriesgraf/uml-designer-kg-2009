Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class dlgTypedef
    Implements InterfFormDocument

    Private m_xmlView As XmlTypedefView

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            m_xmlView.UpdateValues()
            Me.Tag = True

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Tag = m_xmlView.Updated
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub dlgMember_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With m_xmlView
            .LoadValues()
            .InitBindingRange(Me.cmbRange)
            .InitBindingBrief(Me.txtBrief)
            .InitBindingName(Me.txtName)
            cmdType.Text = .FullpathTypeDescription

            Me.Text = .Name
        End With
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "typedef")
    End Sub

    Private Sub cmdType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdType.Click
        Dim fen As Form = m_xmlView.TypeVarDefinition.CreateDialogBox()
        fen.ShowDialog()
        If CType(fen.Tag, Boolean) = True Then
            m_xmlView.Updated = True
            cmdType.Text = m_xmlView.FullpathTypeDescription
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox(m_xmlView.Name + " will be deleted, please confirm ?", cstMsgYesNoQuestion) _
            = MsgBoxResult.Yes Then
            If m_xmlView.RemoveMe() Then
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
End Class
