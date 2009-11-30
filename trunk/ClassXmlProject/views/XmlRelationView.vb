Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlClassListView
Imports ClassXmlProject.XmlProjectTools
Imports System.Xml
Imports Microsoft.VisualBasic

Public Class XmlRelationView
    Inherits XmlRelationSpec
    Implements InterfViewForm

    Class CheckAccessorCommand
        Public GetAccessor As CheckBox
        Public SetAccessor As CheckBox

        Public WriteOnly Property Enabled() As Boolean
            Set(ByVal value As Boolean)
                GetAccessor.Enabled = value
                SetAccessor.Enabled = value
            End Set
        End Property
    End Class

#Region "Member attributes"

    Private m_xmlBindingsList As XmlBindingsList

    Private m_strOldFather As String
    Private m_cmdFatherClass As New ComboCommand
    Private m_cmdFatherLevel As New ComboCommand
    Private m_cmdFatherCardinal As New ComboCommand
    Private m_bFatherDisabled As Boolean = False
    Private m_chkFatherAccessors As New CheckAccessorCommand
    Private m_chkFatherMember As New CheckBox

    Private m_strOldChild As String
    Private m_cmdChildClass As New ComboCommand
    Private m_cmdChildLevel As New ComboCommand
    Private m_cmdChildCardinal As New ComboCommand
    Private m_bChildDisabled As Boolean = False
    Private m_chkChildAccessors As New CheckAccessorCommand

    Private WithEvents m_btnFatherType As Button
    Private WithEvents m_btnChildType As Button
    Private WithEvents m_cmbChildCardinal As ComboBox
    Private WithEvents m_cmbFatherCardinal As ComboBox
    Private WithEvents m_cmbFatherRange As ComboBox

#End Region

#Region "Public Properties and Methods"

    Public Property KindIndex() As Integer
        Get
            Return CType(Me.Kind, Integer)
        End Get
        Set(ByVal value As Integer)
            Me.Kind = CType(value, EKindRelation)
        End Set
    End Property

    Public ReadOnly Property FatherType() As String
        Get
            Dim strId As String = Father.Idref
            Dim iLevel As Integer = Father.Level
            Return GetParentType(Father, m_cmdFatherClass, m_cmdFatherLevel)
        End Get
    End Property

    Public ReadOnly Property ChildType() As String
        Get
            Dim strId As String = Child.Idref
            Dim iLevel As Integer = Child.Level
            Return GetParentType(Child, m_cmdChildClass, m_cmdChildLevel)
        End Get
    End Property

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub

    Public Sub DisableFather()
        m_bFatherDisabled = True
    End Sub

    Public Sub DisableChild()
        m_bChildDisabled = True
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgRelation
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_strOldFather = Me.Father.Idref
        m_strOldChild = Me.Child.Idref
    End Sub

    Public Sub UpdateValues()

        m_xmlBindingsList.UpdateValues()

        ' To avoid an instability
        Me.Father.Cardinal = CType(Me.m_cmbFatherCardinal.SelectedIndex, ECardinal)
        Me.Child.Cardinal = CType(Me.m_cmbChildCardinal.SelectedIndex, ECardinal)

        Dim bParentDifferent As Boolean = (Me.Child.Idref <> Me.Father.Idref)
        Dim bFatherChanged As Boolean = (Me.Father.Idref <> m_strOldFather)
        Dim bChildChanged As Boolean = (Me.Child.Idref <> m_strOldChild)

        If bFatherChanged Then
            XmlProjectTools.UpdateOneCollaboration(Me.Document, m_strOldFather)
            XmlProjectTools.UpdateOneCollaboration(Me.Document, Me.Father.Idref)
        End If

        If bChildChanged And m_strOldChild <> m_strOldFather Then
            XmlProjectTools.UpdateOneCollaboration(Me.Document, m_strOldChild)
        End If

        If bChildChanged And bParentDifferent Then
            XmlProjectTools.UpdateOneCollaboration(Me.Document, Me.Child.Idref)
        End If
    End Sub

    Public Sub InitBindingAction(ByVal dataControl As Control)

        m_xmlBindingsList.AddBinding(dataControl, Me, "Action")

    End Sub

    Public Sub InitBindingTypeComposition(ByVal control As ComboBox)

        control.DropDownStyle = ComboBoxStyle.DropDownList
        control.Items.AddRange(New Object() {"Composition", "Aggregation", "Assembly"})

        m_xmlBindingsList.AddBinding(control, Me, "KindIndex", "SelectedIndex")

    End Sub

#Region "Child methods"

    Public Sub InitBindingChildName(ByVal dataControl As Control)

        InitBindingParentName(Me.Child, dataControl)

    End Sub

    Public Sub InitBindingChildRange(ByVal control As ComboBox, ByVal button As Button)

        m_btnChildType = button
        InitBindingParentRange(Me.Child, control)

    End Sub

    Public Sub InitBindingChildCardinal(ByVal control As ComboBox, ByVal label As Label)

        m_cmbChildCardinal = control
        m_cmdChildCardinal.Combo = control
        m_cmdChildCardinal.Title = label

        InitBindingParentCardinal(Me.Child, m_cmdChildCardinal)

        HandleParentCardinal(Me.Child, m_cmdChildCardinal, m_cmdChildClass, m_cmdChildLevel, m_btnChildType, m_chkChildAccessors)

    End Sub

    Public Sub InitBindingChildMember(ByVal dataControl As Control)

        InitBindingParentMember(Me.Child, dataControl)

    End Sub

    Public Sub InitBindingChildAccessors(ByVal dataControlGet As Control, ByVal dataControlSet As Control)

        m_chkChildAccessors.GetAccessor = dataControlGet
        m_chkChildAccessors.SetAccessor = dataControlSet

        InitBindingParentAccessors(Me.Child, m_chkChildAccessors)
    End Sub

    Public Sub InitBindingChildLevel(ByVal control As ComboBox, ByVal label As Label)
        m_cmdChildLevel.Combo = control
        m_cmdChildLevel.Title = label

        InitBindingParentLevel(Me.Child, m_cmdChildLevel)

    End Sub

    Public Sub InitBindingChildClassList(ByVal control As ComboBox, ByVal label As Label)

        m_cmdChildClass.Combo = control
        m_cmdChildClass.Title = label
        InitBindingParentClassList(Me.Child, m_cmdChildClass)

        If m_bChildDisabled Then
            m_cmdChildClass.Enabled = False
        End If
    End Sub

#End Region

#Region "Father methods"

    Public Sub InitBindingFatherName(ByVal dataControl As Control)

        InitBindingParentName(Me.Father, dataControl)

    End Sub

    Public Sub InitBindingFatherRange(ByVal control As ComboBox)

        m_cmbFatherRange = control
        InitBindingParentRange(Me.Father, control)

    End Sub

    Public Sub InitBindingFatherCardinal(ByVal control As ComboBox, ByVal label As Label, ByVal button As Button)

        m_btnFatherType = button
        m_cmbFatherCardinal = control
        m_cmdFatherCardinal.Combo = control
        m_cmdFatherCardinal.Title = label

        InitBindingParentCardinal(Me.Father, m_cmdFatherCardinal)

        HandleParentCardinal(Me.Father, m_cmdFatherCardinal, m_cmdFatherClass, m_cmdFatherLevel, m_btnFatherType, m_chkFatherAccessors)

    End Sub

    Public Sub InitBindingFatherMember(ByVal dataControl As Control)

        m_chkFatherMember = dataControl
        InitBindingParentMember(Me.Father, dataControl)

    End Sub

    Public Sub InitBindingFatherAccessors(ByVal dataControlGet As Control, ByVal dataControlSet As Control)

        m_chkFatherAccessors.GetAccessor = dataControlGet
        m_chkFatherAccessors.SetAccessor = dataControlSet

        InitBindingParentAccessors(Me.Father, m_chkFatherAccessors)
    End Sub

    Public Sub InitBindingFatherLevel(ByVal control As ComboBox, ByVal label As Label)

        m_cmdFatherLevel.Combo = control
        m_cmdFatherLevel.Title = label

        InitBindingParentLevel(Me.Father, m_cmdFatherLevel)

    End Sub

    Public Sub InitBindingFatherClassList(ByVal control As ComboBox, ByVal label As Label)

        m_cmdFatherClass.Combo = control
        m_cmdFatherClass.Title = label
        InitBindingParentClassList(Me.Father, m_cmdFatherClass)

        If m_bFatherDisabled Then
            m_cmdFatherClass.Enabled = False
        End If
    End Sub

#End Region

#End Region

#Region "Private Methods"
    Private Sub InitBindingParentName(ByVal xmlParent As XmlRelationParentSpec, ByVal dataControl As Control)

        m_xmlBindingsList.AddBinding(dataControl, xmlParent, "Name")

    End Sub

    Private Sub InitBindingParentRange(ByVal xmlParent As XmlRelationParentSpec, ByVal control As ComboBox)

        control.DropDownStyle = ComboBoxStyle.DropDownList
        If xmlParent.NodeName = "child" Then
            control.Items.AddRange(New Object() {"private", "protected", "public"})
        Else
            control.Items.AddRange(New Object() {"no", "private", "protected", "public"})
        End If

        m_xmlBindingsList.AddBinding(control, xmlParent, "Range")

    End Sub

    Private Sub InitBindingParentCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal control As ComboCommand)

        With control
            .Combo.DropDownStyle = ComboBoxStyle.DropDownList
            .Combo.Items.AddRange(New Object() {"1", "0 or 1", "0 to n", "1 to n"})

            m_xmlBindingsList.AddBinding(.Combo, xmlParent, "Cardinal", "SelectedIndex")
        End With

    End Sub

    Private Sub InitBindingParentAccessors(ByVal xmlParent As XmlRelationParentSpec, ByVal dataControl As CheckAccessorCommand)

        m_xmlBindingsList.AddBinding(dataControl.GetAccessor, xmlParent, "AccessorGet", "Checked")
        m_xmlBindingsList.AddBinding(dataControl.SetAccessor, xmlParent, "AccessorSet", "Checked")

    End Sub

    Private Function CheckCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal cmbCardinal As ComboCommand) As Boolean

        Dim bChanged As Boolean
        Dim bMultiple As Boolean = (cmbCardinal.Combo.SelectedIndex = ECardinal.EmptyList _
                                    Or cmbCardinal.Combo.SelectedIndex = ECardinal.FullList)

        Select Case xmlParent.Kind
            Case EKindParent.Array
                bChanged = (bMultiple = False)
            Case EKindParent.Container
                bChanged = (bMultiple = False)
            Case Else
                bChanged = (bMultiple = True)
        End Select

        Return bChanged
    End Function

    Private Function ChangeParentCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal cmbCardinal As ComboCommand) As Boolean

        If CheckCardinal(xmlParent, cmbCardinal) Then
            If MsgBox("This operation will remove accessors or container implementation, would you want to continue ?" _
                      , cstMsgYesNoExclamation, "Cardinal") _
                            = MsgBoxResult.No _
            Then
                CancelCardinal(xmlParent, cmbCardinal)
                Return False
            Else
                ConfirmCardinal(xmlParent, cmbCardinal)
            End If
        End If

        Return True
    End Function

    Private Sub CancelCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal cmbCardinal As ComboCommand)
        cmbCardinal.Combo.SelectedIndex = xmlParent.Cardinal
    End Sub

    Private Sub ConfirmCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal cmbCardinal As ComboCommand)

        xmlParent.Cardinal = CType(cmbCardinal.Combo.SelectedIndex, ECardinal)

        Select Case xmlParent.Cardinal
            Case ECardinal.Variable
                xmlParent.Kind = EKindParent.Reference
            Case ECardinal.Fix
                xmlParent.Kind = EKindParent.Reference
            Case Else
                xmlParent.Kind = EKindParent.Container
        End Select
    End Sub

    Private Function InitBindingParentClassList(ByVal xmlParent As XmlRelationParentSpec, ByVal control As ComboCommand) As Binding
        Dim bindingResult As Binding = Nothing

        With control
            InitTypedefCombo(Me, .Combo)
            bindingResult = m_xmlBindingsList.AddBinding(.Combo, xmlParent, "Idref", "SelectedValue")
        End With

        Return bindingResult
    End Function

    Private Function GetParentType(ByVal xmlParent As XmlRelationParentSpec, _
                                   ByVal cmbClass As ComboCommand, _
                                   ByVal cmblevel As ComboCommand) As String

        Dim strId As String = xmlParent.Idref
        Dim iLevel As Integer = xmlParent.Level

        If xmlParent Is Nothing Then
            Throw New Exception("Argument(s) null in call of " + Me.ToString + ".GetParentType()")
        End If
        With cmbClass.Combo
            If .SelectedValue IsNot Nothing Then
                Dim strTempo = TryCast(.SelectedValue, String)
                If strTempo IsNot Nothing Then
                    strId = CType(.SelectedValue, String)
                End If
            End If
        End With
        With cmblevel.Combo
            If .SelectedIndex <> -1 Then
                iLevel = .SelectedIndex
            End If
        End With
        Return xmlParent.GetFullpathTypeDescription(strId, iLevel)
    End Function

    Private Sub InitBindingParentMember(ByVal xmlParent As XmlRelationParentSpec, ByVal dataControl As Control)

        m_xmlBindingsList.AddBinding(dataControl, xmlParent, "Member", "Checked")

    End Sub

    Private Sub InitBindingParentLevel(ByVal xmlParent As XmlRelationParentSpec, ByVal control As ComboCommand)

        If Me.GenerationLanguage <> ELanguage.Language_CplusPlus Then
            control.Enabled = False
            control.Visible = False
        End If

        With control
            .Combo.DropDownStyle = ComboBoxStyle.DropDownList
            .Combo.Items.AddRange(New Object() {"Value", "Pointer", "Handler"})

            m_xmlBindingsList.AddBinding(.Combo, xmlParent, "Level", "SelectedIndex")
        End With
    End Sub

    Private Sub HandleParentCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal cardinal As ComboCommand, _
                                     ByVal cmbClass As ComboCommand, ByVal cmbLevel As ComboCommand, _
                                     ByVal button As Button, ByVal accessor As CheckAccessorCommand)

        If ChangeParentCardinal(xmlParent, cardinal) Then
            Select Case xmlParent.Kind
                Case EKindParent.Array, EKindParent.Container

                    accessor.Enabled = False
                    button.Enabled = True

                Case Else
                    accessor.Enabled = True
                    button.Enabled = False
            End Select
            button.Text = GetParentType(xmlParent, cmbClass, cmbLevel)
        End If
    End Sub

    Private Sub HandleFatherRange(ByVal dataControl As ComboBox)
        If CType(dataControl.SelectedItem, String) = "no" _
        Then
            m_cmdFatherCardinal.Enabled = False
            m_btnFatherType.Enabled = False
            m_chkFatherAccessors.Enabled = False
            m_cmdFatherLevel.Enabled = False
            m_chkFatherMember.Enabled = False
        Else
            m_cmdFatherCardinal.Enabled = True
            HandleParentCardinal(Me.Father, m_cmdFatherCardinal, m_cmdFatherClass, m_cmdFatherLevel, m_btnFatherType, m_chkFatherAccessors)
            m_cmdFatherLevel.Enabled = True
            m_chkFatherMember.Enabled = True
        End If
    End Sub

    Private Sub ChildCardinal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_cmbChildCardinal.SelectedIndexChanged
        HandleParentCardinal(Me.Child, m_cmdChildCardinal, m_cmdChildClass, m_cmdChildLevel, m_btnChildType, m_chkChildAccessors)
    End Sub

    Private Sub FatherCardinal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_cmbFatherCardinal.SelectedIndexChanged
        HandleParentCardinal(Me.Father, m_cmdFatherCardinal, m_cmdFatherClass, m_cmdFatherLevel, m_btnFatherType, m_chkFatherAccessors)
    End Sub

    Private Sub FatherRange_SelectedIndexChanged(ByVal sender As ComboBox, ByVal e As System.EventArgs) Handles m_cmbFatherRange.SelectedIndexChanged
        HandleFatherRange(sender)
    End Sub
#End Region
End Class
