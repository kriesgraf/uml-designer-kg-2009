Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools
Imports System.Xml
Imports Microsoft.VisualBasic

Public Class XmlRelationView
    Inherits XmlRelationSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList

    Private m_strOldFather As String
    Private m_cmbFatherClass As ComboBox
    Private m_cmbFatherLevel As ComboBox
    Private m_cmbFatherCardinal As ComboBox
    Private WithEvents m_bindingComboFatherClass As Binding

    Private m_strOldChild As String
    Private m_cmbChildClass As ComboBox
    Private m_cmbChildLevel As ComboBox
    Private m_cmbChildCardinal As ComboBox
    Private WithEvents m_bindingComboChildClass As Binding

    Public Property KindIndex() As Integer
        Get
            Return CType(MyBase.Kind, Integer)
        End Get
        Set(ByVal value As Integer)
            MyBase.Kind = CType(value, EKindRelation)
        End Set
    End Property

    Public ReadOnly Property FatherType() As String
        Get
            Dim strId As String = Father.Idref
            Dim iLevel As Integer = Father.Level
            Return GetParentType(Father, strId, m_cmbFatherClass, iLevel, m_cmbFatherLevel)
        End Get
    End Property

    Public ReadOnly Property ChildType() As String
        Get
            Dim strId As String = Child.Idref
            Dim iLevel As Integer = Child.Level
            Return GetParentType(Child, strId, m_cmbChildClass, iLevel, m_cmbChildLevel)
        End Get
    End Property

    Private Function GetParentType(ByVal xmlParent As XmlRelationParentSpec, _
                                   ByVal strId As String, ByVal cmbClass As ComboBox, _
                                   ByVal iLevel As Integer, ByVal cmblevel As ComboBox) As String

        If xmlParent Is Nothing Then
            Throw New Exception("Argument(s) null in call of " + Me.ToString + ".GetParentType()")
        End If
        If cmbClass IsNot Nothing Then
            If cmbClass.SelectedValue IsNot Nothing Then
                Dim strTempo = TryCast(cmbClass.SelectedValue, String)
                If strTempo IsNot Nothing Then
                    strId = cmbClass.SelectedValue
                End If
            End If
        End If
        If cmblevel IsNot Nothing Then
            If cmblevel.SelectedIndex <> -1 Then
                iLevel = cmblevel.SelectedIndex
            End If
        End If

        Return xmlParent.GetFullpathTypeDescription(strId, iLevel)
    End Function

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgRelation
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_strOldFather = MyBase.Father.Idref
        m_strOldChild = MyBase.Child.Idref
    End Sub

    Public Sub UpdateValues()
        Try
            m_xmlBindingsList.UpdateValues()

            Dim bParentDifferent As Boolean = (MyBase.Child.Idref <> MyBase.Father.Idref)
            Dim bFatherChanged As Boolean = (MyBase.Father.Idref <> m_strOldFather)
            Dim bChildChanged As Boolean = (MyBase.Child.Idref <> m_strOldChild)

            If bFatherChanged Then
                XmlProjectTools.UpdateOneCollaboration(Me.Document, m_strOldFather)
                XmlProjectTools.UpdateOneCollaboration(Me.Document, MyBase.Father.Idref)
            End If

            If bChildChanged And m_strOldChild <> m_strOldFather Then
                XmlProjectTools.UpdateOneCollaboration(Me.Document, m_strOldChild)
            End If

            If bChildChanged And bParentDifferent Then
                XmlProjectTools.UpdateOneCollaboration(Me.Document, MyBase.Child.Idref)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingAction(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Action")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingTypeComposition(ByVal control As ComboBox)
        Try
            control.DropDownStyle = ComboBoxStyle.DropDownList
            control.Items.AddRange(New Object() {"Composition", "Aggregation", "Assembly"})

            m_xmlBindingsList.AddBinding(control, Me, "KindIndex", "SelectedIndex")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingChildName(ByVal dataControl As Control)
        Try
            InitBindingParentName(MyBase.Child, dataControl)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub InitBindingParentName(ByVal xmlParent As XmlRelationParentSpec, ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, xmlParent, "Name")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingChildRange(ByVal control As ComboBox)
        Try
            InitBindingParentRange(MyBase.Child, control)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub InitBindingParentRange(ByVal xmlParent As XmlRelationParentSpec, ByVal control As ComboBox)
        Try
            control.DropDownStyle = ComboBoxStyle.DropDownList
            control.Items.AddRange(New Object() {"no", "private", "protected", "public"})

            m_xmlBindingsList.AddBinding(control, xmlParent, "Range")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingChildCardinal(ByVal control As ComboBox)
        Try
            m_cmbChildCardinal = control
            InitBindingParentCardinal(MyBase.Child, control)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub InitBindingParentCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal control As ComboBox)
        Try
            control.DropDownStyle = ComboBoxStyle.DropDownList
            control.Items.AddRange(New Object() {"1", "0 or 1", "0 to n", "1 to n"})

            m_xmlBindingsList.AddBinding(control, xmlParent, "Cardinal", "SelectedIndex")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingChildMember(ByVal dataControl As Control)
        Try
            InitBindingParentMember(MyBase.Child, dataControl)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub InitBindingParentMember(ByVal xmlParent As XmlRelationParentSpec, ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, xmlParent, "Member", "Checked")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingChildAccessors(ByVal dataControlGet As Control, ByVal dataControlSet As Control)
        Try
            InitBindingParentAccessors(MyBase.Child, dataControlGet, dataControlSet)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub InitBindingParentAccessors(ByVal xmlParent As XmlRelationParentSpec, ByVal dataControlGet As Control, ByVal dataControlSet As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControlGet, xmlParent, "AccessorGet", "Checked")
            m_xmlBindingsList.AddBinding(dataControlSet, xmlParent, "AccessorSet", "Checked")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingChildLevel(ByVal control As ComboBox, ByVal label As Label)
        Try
            If Me.Tag <> ELanguage.Language_CplusPlus Then
                control.Enabled = False
                control.Visible = False
                label.Visible = False
            Else
                m_cmbChildLevel = control
                InitBindingParentLevel(MyBase.Child, control)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub InitBindingParentLevel(ByVal xmlParent As XmlRelationParentSpec, ByVal control As ComboBox)
        Try
            control.DropDownStyle = ComboBoxStyle.DropDownList
            control.Items.AddRange(New Object() {"Value", "Pointer", "Handler"})

            m_xmlBindingsList.AddBinding(control, xmlParent, "Level", "SelectedIndex")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingFatherName(ByVal dataControl As Control)
        Try
            InitBindingParentName(MyBase.Father, dataControl)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingFatherRange(ByVal control As ComboBox)
        Try
            InitBindingParentRange(MyBase.Father, control)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingFatherCardinal(ByVal control As ComboBox)
        Try
            m_cmbFatherCardinal = control
            InitBindingParentCardinal(MyBase.Father, control)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingFatherMember(ByVal dataControl As Control)
        Try
            InitBindingParentMember(MyBase.Father, dataControl)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingFatherAccessors(ByVal dataControlGet As Control, ByVal dataControlSet As Control)
        Try
            InitBindingParentAccessors(MyBase.Father, dataControlGet, dataControlSet)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingFatherLevel(ByVal control As ComboBox, ByVal label As Label)
        Try
            If Me.Tag <> ELanguage.Language_CplusPlus Then
                control.Enabled = False
                control.Visible = False
                label.Visible = False
            Else
                InitBindingParentLevel(MyBase.Father, control)
                m_cmbFatherLevel = control
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingFatherClassList(ByVal control As ComboBox)
        Try

            m_cmbFatherClass = control
            m_bindingComboFatherClass = InitBindingParentClassList(MyBase.Father, control)
            m_bindingComboFatherClass.FormattingEnabled = True
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingChildClassList(ByVal control As ComboBox)
        Try
            m_cmbChildClass = control
            m_bindingComboChildClass = InitBindingParentClassList(MyBase.Child, control)
            m_bindingComboChildClass.FormattingEnabled = True

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub HandleComboClassEvent(ByVal sender As Object, ByVal e As BindingCompleteEventArgs) _
                        Handles m_bindingComboChildClass.BindingComplete, _
                                m_bindingComboFatherClass.BindingComplete

        Dim combo As ComboBox = CType(e.Binding.BindableComponent, ComboBox)

        If e.BindingCompleteState = BindingCompleteState.DataError _
        Then
            MsgBox("Control " + combo.Name + vbCrLf + vbCrLf + e.ErrorText, MsgBoxStyle.Critical, Me.Name)

        ElseIf e.BindingCompleteState = BindingCompleteState.Exception _
        Then

            MsgBox("Control " + combo.Name + ", selected index (" + combo.SelectedIndex.ToString + ") is wrong" + vbCrLf + vbCrLf + e.Exception.StackTrace, MsgBoxStyle.Critical, Me.Name)
        End If
    End Sub

    Private Function InitBindingParentClassList(ByVal xmlParent As XmlRelationParentSpec, ByVal control As ComboBox) As Binding
        Dim bindingResult As Binding = Nothing
        Try
            XmlClassListView.AddListComboBoxControl(control, MyBase.Document, Me.Tag)

            bindingResult = m_xmlBindingsList.AddBinding(control, xmlParent, "Idref", "SelectedValue")

        Catch ex As Exception
            Throw ex
        End Try
        Return bindingResult
    End Function

    Public Function ChangeChildCardinal() As Boolean
        If m_cmbChildCardinal Is Nothing Then
            Throw New Exception("Property m_cmbChildCardinal is null, omit to call previously the method " + Me.ToString + ".InitBindingChildCardinal()")
        End If
        Return ChangeParentCardinal(MyBase.Child, m_cmbChildCardinal)
    End Function

    Public Function ChangeFatherCardinal() As Boolean
        If m_cmbFatherCardinal Is Nothing Then
            Throw New Exception("Property m_cmbFatherCardinal is null, omit to call previously the method " + Me.ToString + ".InitBindingFatherCardinal()")
        End If
        Return ChangeParentCardinal(MyBase.Father, m_cmbFatherCardinal)
    End Function

    Public Function ChangeParentCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal cmbCardinal As ComboBox) As Boolean
        Try
            If CheckCardinal(xmlParent, cmbCardinal) Then
                If MsgBox("This operation will remove accessors or container implementation, would you want to continue ?" _
                          , cstMsgYesNoExclamation) _
                                = MsgBoxResult.No _
                Then
                    CancelCardinal(xmlParent, cmbCardinal)
                Else
                    ConfirmCardinal(xmlParent, cmbCardinal)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function CheckCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal cmbCardinal As ComboBox) As Boolean

        Dim bChanged As Boolean
        Dim bMultiple As Boolean = (cmbCardinal.SelectedIndex = ECardinal.EEmptyList _
                                    Or cmbCardinal.SelectedIndex = ECardinal.EFullList)

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

    Private Sub CancelCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal cmbCardinal As ComboBox)
        cmbCardinal.SelectedIndex = xmlParent.Cardinal
    End Sub

    Private Sub ConfirmCardinal(ByVal xmlParent As XmlRelationParentSpec, ByVal cmbCardinal As ComboBox)

        xmlParent.Cardinal = cmbCardinal.SelectedIndex

        Select Case xmlParent.Cardinal
            Case ECardinal.EVariable
                xmlParent.Kind = EKindParent.Reference
            Case ECardinal.EFix
                xmlParent.Kind = EKindParent.Reference
            Case Else
                xmlParent.Kind = EKindParent.Container
        End Select
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
