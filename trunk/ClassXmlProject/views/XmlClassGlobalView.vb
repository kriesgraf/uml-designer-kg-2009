Imports System
Imports System.Windows.Forms
Imports System.Xml
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class XmlClassGlobalView
    Inherits XmlClassSpec
    Implements InterfViewForm
    Implements InterfObject

    Private m_bInitProcessing As Boolean = False
    Private m_xmlBindingsList As XmlBindingsList
    Private m_mnuConstructor As ToolStripMenuItem

    Private WithEvents m_cmbImplementation As ComboBox
    Private m_cmbParamInline As ComboBox

    Private m_gridMembers As XmlDataGridView
    Private m_gridInherited As XmlDataGridView
    Private m_cmdImplementation As New ComboCommand
    Private m_cmdParamInline As New ComboCommand
    Private m_cmdConstructor As New ComboCommand
    Private m_cmdDestructor As New ComboCommand

    Public ReadOnly Property CurrentClassImpl() As EImplementation
        Get
            Return ConvertViewToEnumImpl(CType(m_cmbImplementation.SelectedItem, String))
        End Get
    End Property

    Public Property ClassImpl() As String
        Get
            Return ConvertEnumImplToView(MyBase.Implementation)
        End Get
        Set(ByVal value As String)
            MyBase.Implementation = ConvertViewToEnumImpl(value)
        End Set
    End Property

    Public Property InterfObject() As Object Implements InterfObject.InterfObject
        Get
            Return Me
        End Get
        Set(ByVal value As Object)
            ' Nothing to do
        End Set
    End Property

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
    End Sub

    Public Sub UpdateObject() Implements InterfObject.Update

    End Sub

    Public Function UpdateValues() As Boolean
        Dim bResult As Boolean = False
        Try
            If MustCheckTemplate() _
            Then
                If Me.ModelCount <> m_cmbParamInline.SelectedIndex _
                Then
                    If CheckTemplate(True) _
                    Then
                        Me.ModelCount = m_cmbParamInline.SelectedIndex
                        bResult = True
                    End If
                Else
                    bResult = True
                End If
            ElseIf CheckTemplate(True) = True _
            Then
                Me.Inline = CType(m_cmbParamInline.SelectedItem, String)
                bResult = True
            End If

            If bResult Then
                m_gridMembers.Binding.UpdateRows()   ' We must change implementation of members if necessary
                m_xmlBindingsList.UpdateValues()
            End If

            ' Check data changed in grids
            If Me.Updated Then
                bResult = True
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Sub UpdateMenuClass(ByVal menuitem As ToolStripItem)
        If MyBase.GenerationLanguage <> ELanguage.Language_Vbasic Then
            menuitem.Text = "Typedef"
        Else
            menuitem.Text = "Enumeration"
        End If
    End Sub

    Public Sub InitBindingName(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingBriefComment(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "BriefComment")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingComment(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingConstructor(ByVal titleControl As Label, ByVal dataControl As ComboBox)
        Try
            m_cmdConstructor.Combo = dataControl
            m_cmdConstructor.Title = titleControl

            Select Case MyBase.GenerationLanguage
                Case ELanguage.Language_CplusPlus
                    titleControl.Text = "Default constructor:"

                Case ELanguage.Language_Vbasic
                    titleControl.Text = "Default New method:"

                Case ELanguage.Language_Java
                    titleControl.Text = "Default constructor:"
            End Select

            With dataControl
                .DropDownStyle = ComboBoxStyle.DropDownList
                .Items.AddRange(New Object() {"no", "private", "protected", "public"})
            End With
            m_xmlBindingsList.AddBinding(dataControl, Me, "Constructor", "SelectedItem")

            UpdateConstructorControl()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingDestructor(ByVal titleControl As Label, ByVal dataControl As ComboBox)
        Try
            m_cmdDestructor.Combo = dataControl
            m_cmdDestructor.Title = titleControl

            With dataControl
                .DropDownStyle = ComboBoxStyle.DropDownList
                .Items.AddRange(New Object() {"no", "private", "protected", "public"})
            End With
            m_xmlBindingsList.AddBinding(dataControl, Me, "Destructor", "SelectedItem")

            UpdateDestructorControl()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub InitBindingImplementation(ByVal titleControl As Label, ByVal dataControl As ComboBox)
        Try
            m_bInitProcessing = True
            m_cmbImplementation = dataControl
            m_cmdDestructor.Combo = dataControl
            m_cmdDestructor.Title = titleControl

            With dataControl
                .DropDownStyle = ComboBoxStyle.DropDownList
                .Sorted = True
                .Items.Add(ConvertEnumImplToView(EImplementation.Interf))
                .Items.Add(ConvertEnumImplToView(EImplementation.Container))

                If Me.Tag <> ELanguage.Language_Vbasic Then
                    .Items.Add(ConvertEnumImplToView(EImplementation.Exception))
                End If

                .Items.Add(ConvertEnumImplToView(EImplementation.Leaf))
                .Items.Add(ConvertEnumImplToView(EImplementation.Simple))
                .Items.Add(ConvertEnumImplToView(EImplementation.Root))
                .Items.Add(ConvertEnumImplToView(EImplementation.Node))
            End With

            m_cmbImplementation = dataControl
            m_xmlBindingsList.AddBinding(dataControl, Me, "ClassImpl", "SelectedItem")

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bInitProcessing = False
        End Try
    End Sub

    Public Sub InitBindingVisibility(ByVal dataControl As ComboBox)
        Try
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"package", "common"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "Visibility", "SelectedItem")

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub InitBindingInline(ByVal titleControl As Label, ByVal dataControl As ComboBox)
        Try

            m_cmdParamInline.Combo = dataControl
            m_cmdParamInline.Title = titleControl

            m_cmbParamInline = dataControl
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList

            UpdateInlineControl()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub InitBindingPartial(ByVal chkPartial As CheckBox)
        If Me.Tag = ELanguage.Language_Vbasic Then
            chkPartial.Visible = True
            chkPartial.Enabled = True
            m_xmlBindingsList.AddBinding(chkPartial, Me, "PartialDecl", "Checked")
        Else
            chkPartial.Visible = False
            chkPartial.Enabled = False
        End If
    End Sub

    Public Sub UpdateMenuConstructor(ByVal menu As ToolStripMenuItem)

        m_mnuConstructor = menu

        If CurrentClassImpl = EImplementation.Interf _
        Then
            m_mnuConstructor.Visible = False
        Else
            m_mnuConstructor.Visible = False
        End If
    End Sub

    Public Sub LoadMembers(ByVal dataControl As XmlDataGridView)
        Try
            m_gridMembers = dataControl
            dataControl.Binding.LoadXmlNodes(Me, "typedef | property | method", "class_member_view", CType(Me, InterfObject))
            dataControl.Binding.NodeCounter = m_xmlReferenceNodeCounter

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub LoadInheritedMembers(ByVal dataControl As XmlDataGridView)
        Try
            m_gridInherited = dataControl
            dataControl.Binding.LoadXmlNodes(Me, "inherited", "class_inherited_view", CType(Me, InterfObject))

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub LoadDependencies(ByVal dataControl As XmlDataGridView)
        Try
            dataControl.Binding.LoadXmlNodes(Me, "dependency", "class_dependency_view")

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub LoadRelations(ByVal dataControl As XmlDataGridView)
        Try
            dataControl.Binding.LoadXmlNodes(Me, "collaboration", "class_relation_view")
            dataControl.Binding.NodeCounter = m_xmlReferenceNodeCounter

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Overrides Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim xmlcpnt As XmlComponent = CreateDocument(removeNode.Node)
            If removeNode.NodeName = "typedef" Then

                If SelectNodes(dlgDependencies.GetQuery(removeNode)).Count > 0 Then

                    If MsgBox("Some elements reference this, you can dereference them and then this will be deleted." + _
                              vbCrLf + "Do you want to proceed", _
                                cstMsgYesNoQuestion, _
                                xmlcpnt.Name) = MsgBoxResult.Yes _
                    Then
                        Dim bIsEmpty As Boolean = False
                        bResult = dlgDependencies.ShowDependencies(xmlcpnt, bIsEmpty, "Remove references to " + xmlcpnt.Name)
                        If bIsEmpty = False Then
                            Return bResult
                        End If
                    Else
                        Return False
                    End If
                End If
            End If
            Dim strName As String = xmlcpnt.Name
            If MsgBox("Confirm to delete:" + vbCrLf + "Name: " + strName, _
                       cstMsgYesNoQuestion) = MsgBoxResult.Yes _
            Then
                Dim strNodeName As String = removeNode.NodeName
                If MyBase.RemoveComponent(removeNode) Then
                    If strNodeName = "inherited" Then
                        m_gridMembers.Binding.ResetBindings(True)
                    End If
                    bResult = True
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Sub New(Optional ByRef xmlnode As XmlNode = Nothing)
        MyBase.New(xmlnode, False)
        Try
            m_xmlBindingsList = New XmlBindingsList
            m_xmlNodeManager = XmlNodeManager.GetInstance()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgClass
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Function MustCheckTemplate() As Boolean
        Return (CType(m_cmbImplementation.SelectedItem, String) = ConvertEnumImplToView(EImplementation.Container))
    End Function

    Private Sub Implementation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_cmbImplementation.SelectedIndexChanged
        If m_bInitProcessing Then Exit Sub
        Try
            m_cmbParamInline.Items.Clear()
            UpdateInlineControl()
            UpdateConstructorControl()
            UpdateDestructorControl()
            m_gridInherited.Binding.UpdateRows()   ' We must checked inherited class with new implementation

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub UpdateConstructorControl()
        With m_cmdConstructor
            If Me.CurrentClassImpl = EImplementation.Interf Then
                m_mnuConstructor.Visible = False
                .Combo.SelectedIndex = 0
                .Enabled = False
            Else
                m_mnuConstructor.Visible = True
                .Enabled = True
            End If
        End With
    End Sub

    Private Sub UpdateDestructorControl()
        With m_cmdDestructor
            If Me.CurrentClassImpl = EImplementation.Interf Then
                If MyBase.GenerationLanguage = ELanguage.Language_CplusPlus _
                Then
                    .Combo.SelectedIndex = 0
                    .Enabled = False
                    .Visible = True
                Else
                    .Combo.SelectedIndex = 0
                    .Enabled = False
                    .Visible = False
                End If
            ElseIf MyBase.GenerationLanguage = ELanguage.Language_CplusPlus _
            Then
                .Enabled = True
                .Visible = True
            End If
        End With
    End Sub

    Private Sub UpdateInlineControl()
        With m_cmdParamInline
            If MustCheckTemplate() _
            Then
                .Title.Text = "Model parameter:"
                .Combo.Items.AddRange(New Object() {"A", "A, B"})
                Dim iCount As Integer = Me.ModelCount
                If iCount < 0 Then
                    .Combo.SelectedIndex = 0
                Else
                    .Combo.SelectedIndex = iCount
                End If

                .Visible = True
                .Enabled = True

            ElseIf MyBase.GenerationLanguage = ELanguage.Language_CplusPlus _
            Then
                If Me.CurrentClassImpl = EImplementation.Interf _
                Then
                    .Enabled = False
                    .Visible = True
                    With .Combo
                        .Items.AddRange(New Object() {"none"})
                        .SelectedIndex = 0
                    End With
                Else
                    .Enabled = True
                    .Visible = True
                    .Title.Text = "Inline:"
                    With .Combo
                        .DropDownStyle = ComboBoxStyle.DropDownList
                        .Items.AddRange(New Object() {"none", "constructor", "destructor", "both"})
                        .SelectedItem = Me.Inline
                    End With
                End If
            Else
                .Enabled = False
                .Visible = False
                .Combo.Items.AddRange(New Object() {"none"})
                .Combo.SelectedIndex = 0
            End If
        End With
    End Sub
End Class
