Imports System.Xml
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools

Public Class XmlClassGlobalView
    Inherits XmlClassSpec
    Implements InterfViewForm
    Implements InterfObject

    Private m_bInitProcessing As Boolean = False
    Private m_xmlBindingsList As XmlBindingsList
    Private m_xmlNodeManager As XmlNodeManager

    Private WithEvents m_cmbImplementation As ComboBox
    Private m_lblInline As Label
    Private WithEvents m_cmbParamInline As ComboBox

    Public ReadOnly Property CurrentClassImpl() As EImplementation
        Get
            Return ConvertViewToEnumImpl(m_cmbImplementation.SelectedItem)
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
                If MyBase.GenerationLanguage = ELanguage.Language_CplusPlus _
                Then
                    Me.Inline = m_cmbParamInline.SelectedItem
                End If
                bResult = True
            End If

            If bResult Then
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

    Public Sub InitBindingName(ByVal dataControl As Windows.Forms.Control)
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

    Public Sub InitBindingConstructor(ByVal titleControl As Label, ByVal dataControl As ComboBox, ByVal btnControl As Button)
        Try
            Select Case MyBase.GenerationLanguage
                Case ELanguage.Language_CplusPlus
                    titleControl.Text = "Default constructor:"

                Case ELanguage.Language_Vbasic
                    titleControl.Text = "Default New method:"
                    btnControl.Visible = False

                Case ELanguage.Language_Java
                    titleControl.Text = "Default constructor:"
                    btnControl.Visible = False
            End Select

            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"no", "private", "protected", "public"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "Constructor", "SelectedItem")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingDestructor(ByVal titleControl As Label, ByVal dataControl As ComboBox, ByVal btnControl As Button)
        Try
            If MyBase.GenerationLanguage = ELanguage.Language_CplusPlus _
            Then
                dataControl.DropDownStyle = ComboBoxStyle.DropDownList
                dataControl.Items.AddRange(New Object() {"private", "protected", "public"})
                m_xmlBindingsList.AddBinding(dataControl, Me, "Destructor", "SelectedItem")
            Else
                titleControl.Visible = False
                dataControl.Visible = False
                dataControl.Enabled = False
                btnControl.Visible = False
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub InitBindingImplementation(ByVal dataControl As ComboBox)
        Try
            m_bInitProcessing = True

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
            m_lblInline = titleControl
            m_cmbParamInline = dataControl
            m_cmbParamInline.DropDownStyle = ComboBoxStyle.DropDownList
            UpdateInlineControl()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub InitBindingPartial(ByVal btn1 As Button, ByVal btn2 As Button, ByVal chkPartial As CheckBox)
        If Me.Tag = ELanguage.Language_Vbasic Then
            btn1.Visible = False
            btn2.Visible = False
            chkPartial.Visible = True
            chkPartial.Enabled = True
            m_xmlBindingsList.AddBinding(chkPartial, Me, "PartialDecl", "Checked")
        Else
            btn1.Visible = True
            btn2.Visible = True
            chkPartial.Visible = False
            chkPartial.Enabled = False
        End If
    End Sub

    Public Sub LoadMembers(ByVal dataControl As XmlDataGridView)
        Try
            dataControl.Binding.LoadXmlNodes(Me, "typedef | property | method", "class_member_view", CType(Me, InterfObject))
            dataControl.Binding.NodeCounter = m_xmlReferenceNodeCounter

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub LoadInheritedMembers(ByVal dataControl As XmlDataGridView)
        Try
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

                If SelectNodes("//*[@*[.='" + GetID(removeNode.Node) + "' and name()!='id']]").Count > 0 Then

                    If MsgBox("Some elements reference this, you can dereference them and then this will be deleted." + _
                              vbCrLf + "Do you want to proceed", _
                                cstMsgYesNoQuestion, _
                                xmlcpnt.Name) = MsgBoxResult.Yes _
                    Then
                        Dim fen As New dlgDependencies
                        fen.NoTitle = False
                        fen.Text = "Remove references to " + xmlcpnt.Name
                        fen.Document = xmlcpnt
                        fen.ShowDialog()
                        bResult = CType(fen.Tag, Boolean)
                        If fen.IsEmpty = False Then
                            Return bResult
                        End If
                    Else
                        Return False
                    End If
                End If
            End If
            If MsgBox("Confirm to delete " + xmlcpnt.Name, _
                       cstMsgYesNoQuestion, _
                       xmlcpnt.Name) = MsgBoxResult.Yes _
            Then
                Select Case removeNode.NodeName
                    Case "property"
                        RemoveOverridedProperty(removeNode)

                    Case "method"
                        RemoveOverridedMethod(removeNode)

                    Case "inherited"
                        RemoveInheritedProperties(removeNode)
                        RemoveInheritedMethods(removeNode)
                End Select

                If MyBase.RemoveComponent(removeNode) Then
                    bResult = True
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Function ShowDialogConstructor() As Boolean
        Dim bResult As Boolean = False
        Try
            If Me.InlineConstructor Is Nothing Then
                m_bCreateNodeNow = True
                ChangeReferences()
                m_bCreateNodeNow = False
                Me.InlineConstructor.SetDefaultValues()
            End If

            Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(Me.InlineConstructor)
            fen.Text = "Constructor"
            fen.ShowDialog()
            If CType(fen.Tag, Boolean) Then
                Me.Updated = True
                bResult = True
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Function ShowDialogDestructor() As Boolean
        Dim bResult As Boolean = False
        Try
            If Me.InlineDestructor Is Nothing Then
                m_bCreateNodeNow = True
                ChangeReferences()
                m_bCreateNodeNow = False
                Me.InlineDestructor.SetDefaultValues()
            End If

            Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(Me.InlineDestructor)
            fen.Text = "Destructor"
            fen.ShowDialog()
            If CType(fen.Tag, Boolean) Then
                Me.Updated = True
                bResult = True
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

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub UpdateInlineControl()
        If MustCheckTemplate() _
        Then
            m_cmbParamInline.Enabled = True
            m_lblInline.Text = "Model parameter:"
            m_cmbParamInline.Items.AddRange(New Object() {"A", "A, B"})
            Dim iCount As Integer = Me.ModelCount
            If iCount < 0 Then
                m_cmbParamInline.SelectedIndex = 0
            Else
                m_cmbParamInline.SelectedIndex = iCount
            End If

            m_cmbParamInline.Visible = True
            m_lblInline.Visible = True

        ElseIf MyBase.GenerationLanguage = ELanguage.Language_CplusPlus _
        Then
            m_cmbParamInline.Enabled = True
            m_lblInline.Text = "Inline:"
            m_cmbParamInline.DropDownStyle = ComboBoxStyle.DropDownList
            m_cmbParamInline.Items.AddRange(New Object() {"none", "constructor", "destructor", "both"})
            m_cmbParamInline.SelectedItem = Me.Inline

            m_cmbParamInline.Visible = True
            m_lblInline.Visible = True
        Else
            m_cmbParamInline.Enabled = False
            m_cmbParamInline.Visible = False
            m_lblInline.Visible = False
        End If
    End Sub

    Private Sub RemoveOverridedProperty(ByVal removeNode As XmlComponent)
        ''Throw New Exception("RemoveOverridedProperty not yet implemented")
    End Sub

    Private Sub RemoveOverridedMethod(ByVal removeNode As XmlComponent)
        ' Throw New Exception("RemoveOverridedMethod not yet implemented")
    End Sub

    Private Sub RemoveInheritedProperties(ByVal removeNode As XmlComponent)
        'Throw New Exception("RemoveInheritedProperties not yet implemented")
    End Sub

    Private Function RemoveInheritedMethods(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim listID As New List(Of String)
            LoadTreeInherited(removeNode.Node, listID)
            For Each strId As String In listID
                Dim listMethod As XmlNodeList = SelectNodes("method[@overrides='" + strId + "']")
                For Each method As XmlNode In listMethod
                    MyBase.Node.RemoveChild(method)
                    bResult = True
                Next
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return bResult
    End Function
End Class
