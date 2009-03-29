Imports System
Imports ClassXmlProject.XmlProjectTools
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class dlgRelation
    Implements InterfFormDocument
    Implements InterfNodeCounter
    Implements InterfFormCollaboration

    Private m_xmlView As XmlRelationView
    Private m_bEnableFather As Boolean = True
    Private m_bEnableChild As Boolean = True
    Private m_bChildCardinalChange As Boolean = False
    Private m_bFatherCardinalChange As Boolean = False
    Private m_bInit As Boolean = False
    Private m_strClassCollaborationtID As String

    Public WriteOnly Property ClassID() As String Implements InterfFormCollaboration.ClassID
        Set(ByVal value As String)
            m_strClassCollaborationtID = value
        End Set
    End Property

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

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
        If m_xmlView.Father.Idref = m_strClassCollaborationtID _
        Then
            m_bEnableFather = False
        ElseIf m_xmlView.Child.Idref = m_strClassCollaborationtID _
        Then
            m_bEnableChild = False
        End If
    End Sub

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            m_xmlView.Node = value.Node
            ' get a useful tag that transmit generation language ID
            m_xmlView.Tag = value.Tag
        End Set
    End Property

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView(Nothing, "relationship")
    End Sub

    Private Sub dlgRelation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            With m_xmlView
                m_bInit = True
                .LoadValues()

                ' Relationship attributes
                .InitBindingAction(txtAction)
                .InitBindingTypeComposition(cmbTypeComposition)

                ' Child attributes
                m_bFatherCardinalChange = True
                .InitBindingFatherName(txtFatherName)
                .InitBindingFatherCardinal(cmbFatherCardinal) ' Cardinal msut call before Range
                .InitBindingFatherRange(cmbFatherRange)
                .InitBindingFatherMember(chkFatherMember)
                .InitBindingFatherLevel(cmbFatherLevel, lblFatherLevel)
                .InitBindingFatherClassList(cmbFatherClass)
                .InitBindingFatherAccessors(chkGetFather, chkSetFather)
                m_bFatherCardinalChange = False

                ' Child attributes
                m_bChildCardinalChange = True
                .InitBindingChildName(txtChildName)
                .InitBindingChildCardinal(cmbChildCardinal) ' Cardinal msut call before Range
                .InitBindingChildRange(cmbChildRange)
                .InitBindingChildMember(chkChildMember)
                .InitBindingChildLevel(cmbChildLevel, lblChildLevel)
                .InitBindingChildClassList(cmbChildClass)
                .InitBindingChildAccessors(chkGetChild, chkSetChild)

                Me.Text = .Name

                m_bChildCardinalChange = False

                cmbChildClass.Enabled = m_bEnableChild
                cmbFatherClass.Enabled = m_bEnableFather
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bInit = False
        End Try
    End Sub

    Private Sub cmbFatherCardinal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFatherCardinal.SelectedIndexChanged
        If cmbFatherCardinal.Enabled = False Or m_bInit Then
            ' Nothing to do
        Else
            ChangeFatherCardinal()

            Me.btnFatherType.Text = m_xmlView.FatherType

            If cmbFatherCardinal.SelectedIndex > ECardinal.EVariable Then
                lblAccessor.Enabled = False
                chkGetFather.Enabled = False
                chkSetFather.Enabled = False
                btnFatherType.Enabled = True
                lblFatherType.Enabled = True
            Else
                lblAccessor.Enabled = True
                chkGetFather.Enabled = True
                chkSetFather.Enabled = True
                btnFatherType.Enabled = False
                lblFatherType.Enabled = False
            End If
        End If
    End Sub

    Private Sub cmbChildCardinal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbChildCardinal.SelectedIndexChanged
        ChangeChildCardinal()

        Me.btnChildType.Text = m_xmlView.ChildType

        If cmbChildCardinal.SelectedIndex > ECardinal.EVariable Then
            chkGetChild.Enabled = False
            chkSetChild.Enabled = False
            btnChildType.Enabled = True
            lblChildType.Enabled = True
        Else
            chkGetChild.Enabled = True
            chkSetChild.Enabled = True
            btnChildType.Enabled = False
            lblChildType.Enabled = False
        End If
    End Sub

    Private Sub cmbFatherRange_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFatherRange.SelectedIndexChanged
        If CType(cmbFatherRange.SelectedItem, String) = "no" Then
            cmbFatherClass.Enabled = False
            lblFatherLevel.Enabled = False
            cmbFatherLevel.Enabled = False
            lblFatherCardinal.Enabled = False
            cmbFatherCardinal.Enabled = False
            chkFatherMember.Enabled = False
            ' to disable action of cmbFatherCardinal_SelectedIndexChanged
            chkGetFather.Enabled = False
            chkSetFather.Enabled = False
            lblAccessor.Enabled = False
            btnFatherType.Enabled = False
            lblFatherType.Enabled = False
        Else
            cmbFatherClass.Enabled = m_bEnableFather
            lblFatherLevel.Enabled = True
            cmbFatherLevel.Enabled = True
            lblFatherCardinal.Enabled = True
            cmbFatherCardinal.Enabled = True
            chkFatherMember.Enabled = True
            cmbFatherCardinal_SelectedIndexChanged(sender, e)
        End If
    End Sub

    Private Sub cmbParent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
                                    cmbFatherClass.SelectedIndexChanged _
                                    , cmbFatherLevel.SelectedIndexChanged _
                                    , cmbChildClass.SelectedIndexChanged _
                                    , cmbChildLevel.SelectedIndexChanged
        If m_bInit Then Exit Sub

        Me.btnFatherType.Text = m_xmlView.FatherType
        Me.btnChildType.Text = m_xmlView.ChildType
    End Sub

    Private Sub btnChildType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChildType.Click
        Try
            Dim fen As Form = m_xmlView.Child.CreateDialogBox()
            fen.ShowDialog()
            If CType(fen.Tag, Boolean) = True Then
                m_xmlView.Updated = True
                btnChildType.Text = m_xmlView.ChildType
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub btnFatherType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFatherType.Click
        Try
            Dim fen As Form = m_xmlView.Father.CreateDialogBox()
            fen.ShowDialog()
            If CType(fen.Tag, Boolean) = True Then
                m_xmlView.Updated = True
                btnFatherType.Text = m_xmlView.FatherType
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub ChangeFatherCardinal()
        If m_bFatherCardinalChange Then Exit Sub
        Try
            m_bFatherCardinalChange = True
            m_xmlView.ChangeFatherCardinal()

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bFatherCardinalChange = False
        End Try
    End Sub

    Private Sub ChangeChildCardinal()
        If m_bChildCardinalChange Then Exit Sub
        Try
            m_bChildCardinalChange = True
            m_xmlView.ChangeChildCardinal()

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bChildCardinalChange = False
        End Try
    End Sub

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            ' Not used !
        End Set
    End Property

    Private Sub btnDelete_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox("This relation will be deleted, please confirm ?", cstMsgYesNoQuestion) _
            = MsgBoxResult.Yes Then
            If m_xmlView.RemoveMe() Then
                Me.Tag = True
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
End Class
