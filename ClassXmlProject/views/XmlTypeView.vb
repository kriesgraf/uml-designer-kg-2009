Imports System.Xml
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports ClassXmlProject.UmlNodesManager

Public Class XmlTypeView
    Inherits XmlTypeVarSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_xmlNodeManager As XmlNodeManager
    Private m_xmlComboTypedef As XmlBindingCombo
    Private m_xmlComboValue As XmlBindingCombo
    Private m_xmlComboSize As XmlBindingCombo
    Private m_SizeCheckBox As CheckBox
    Private m_ArrayRadioButtons As RadioButtonArray

    Public Overrides Property Name() As String
        Get
            If MyBase.Node.ParentNode.Name = "return" Then
                Return MyBase.GetAttribute("name", "ancestor::method")
            Else
                Return MyBase.GetAttribute("name", "parent::*")
            End If
        End Get
        Set(ByVal value As String)
            ' Read only here !
        End Set
    End Property

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
    End Sub

    Public Function ConfirmCancel(ByRef bOk As Boolean) As Boolean
        If MyBase.IsEnumWrong Then
            If MsgBox("Enumeration will be destroyed. Do you want to continue ?", _
                      cstMsgOkCancelCritical) = MsgBoxResult.Ok _
            Then
                MyBase.Kind = EKindDeclaration.EK_SimpleType
                bOk = True
                Return True
            Else
                Return False
            End If
        End If
        Return True
    End Function

    Public Function UpdateValues() As Boolean
        Try
            If MyBase.Kind = EKindDeclaration.EK_Enumeration Then
                If MyBase.CheckEnumeration() = False Then
                    Return False
                End If
            End If
            m_xmlBindingsList.UpdateValues()
            m_xmlComboTypedef.Update()
            If m_ArrayRadioButtons.Item(0).Checked Then
                Me.Kind = EKindDeclaration.EK_SimpleType
            Else
                Me.Kind = EKindDeclaration.EK_Enumeration
            End If
            If m_SizeCheckBox.Enabled = False Then
                Me.Kind = EKindDeclaration.EK_Enumeration
            ElseIf m_SizeCheckBox.Checked = False Then
                Me.VarSize = ""
                If Me.Node.ParentNode.Name = "property" Or Me.Node.ParentNode.Name = "param" Then
                    m_xmlComboValue.Update()
                End If
            Else
                Me.Value = ""
                m_xmlComboSize.Update()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
            Return False
        End Try
        Return True
    End Function

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgTypeVar
        CType(frmResult, InterfFormDocument).Document = document
        If document.Node.ParentNode.Name = "typedef" Then
            CType(frmResult, InterfFormDocument).DisableMemberAttributes()
        End If
        Return frmResult
    End Function

    Public Sub InitCheckBoxArray(ByVal control As CheckBox)
        m_SizeCheckBox = control
        If MyBase.SizeRef Is Nothing And MyBase.VarSize Is Nothing Then
            control.Checked = False
        Else
            control.Checked = True
        End If
    End Sub

    Public Sub InitBindingOption(ByVal control As RadioButtonArray)
        Try
            m_ArrayRadioButtons = control
            If TestNode("enumvalue") Then
                control.Item(1).Checked = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function UpdateOption(ByVal radioControl As RadioButtonArray, ByVal dataControl As XmlDataGridView, ByVal fen As Form) As Boolean
        Dim bLocked As Boolean = False
        If Me.Tag = ELanguage.Language_Vbasic _
        Then
            radioControl.Item(0).Visible = False
            radioControl.Item(1).Visible = False

            If GetNode("ancestor::typedef") Is Nothing _
            Then
                radioControl.Item(0).Checked = True
                dataControl.Visible = False
                fen.Height = 250
                bLocked = True
            Else
                radioControl.Item(1).Checked = True
            End If
        Else
            radioControl.Item(0).Visible = True
            radioControl.Item(1).Visible = True
        End If
        Return bLocked
    End Function

    Public Sub InitBindingReference(ByVal control As Control)
        Try
            Dim bIsNotParam = (GetNode("parent::param") Is Nothing)

            If bIsNotParam And Me.Tag <> ELanguage.Language_CplusPlus Then
                control.Enabled = False
                control.Visible = False
            Else
                m_xmlBindingsList.AddBinding(control, Me, "By", "Checked")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingModifier(ByVal control As Control)
        Try
            Dim bIsNotProperty = (GetNode("parent::property") Is Nothing)

            If bIsNotProperty And Me.Tag <> ELanguage.Language_CplusPlus Then
                control.Enabled = False
                control.Visible = False
            Else
                m_xmlBindingsList.AddBinding(control, Me, "Modifier", "Checked")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AddModelList(ByRef myList As ArrayList)
        If GetNode("ancestor::class[@implementation='container']") IsNot Nothing Then
            Debug.Print("Insert model")
            Dim iterator As IEnumerator = MyBase.SelectNodes("ancestor::class/model").GetEnumerator
            iterator.Reset()
            While iterator.MoveNext
                Dim xmlcpnt As XmlNodeListView = New XmlNodeListView(CType(iterator.Current, XmlNode))
                xmlcpnt.Tag = Me.Tag
                Debug.Print(xmlcpnt.FullpathClassName)
                myList.Add(xmlcpnt)
            End While
        End If
    End Sub

    Private Sub AddNodeList(ByRef myList As ArrayList, ByVal xpath As String)
        Dim iterator As IEnumerator = MyBase.SelectNodes(xpath).GetEnumerator
        iterator.Reset()
        'Debug.Print("xpath=" + xpath)
        While iterator.MoveNext
            Dim xmlcpnt As XmlNodeListView = New XmlNodeListView(CType(iterator.Current, XmlNode))
            xmlcpnt.Tag = Me.Tag
            'Debug.Print(xmlcpnt.FullpathClassName)
            myList.Add(xmlcpnt)
        End While
    End Sub

    Public Sub InitBindingTypedefs(ByVal control As ComboBox)
        Try
            Dim bIsNotTypedef = (GetNode("parent::typedef") Is Nothing)
            Dim myList As New ArrayList

            AddModelList(myList)
            AddNodeList(myList, "//class[@implementation!='container']")

            If Me.Tag <> ELanguage.Language_CplusPlus Then
                If bIsNotTypedef Then AddNodeList(myList, "//typedef")
            Else
                AddNodeList(myList, "//typedef")
            End If

            AddNodeList(myList, "//reference[@container='0' or not(@container)]")

            AddSimpleTypesList(myList, CType(Me.Tag, ELanguage))

            myList.Sort(New XmlNodeListView("_comparer"))

            With control
                .DropDownStyle = ComboBoxStyle.DropDown
                .DisplayMember = "FullpathClassName"
                .ValueMember = "Id"
                .DataSource = myList
            End With
            m_xmlComboTypedef = New XmlBindingCombo(control, Me, "Descriptor", "Reference")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingLevel(ByVal control As ComboBox, ByVal label As Label)
        Try
            If Me.Tag <> ELanguage.Language_CplusPlus Then
                control.Enabled = False
                control.Visible = False
                label.Visible = False
            Else
                control.DropDownStyle = ComboBoxStyle.DropDownList
                control.Items.AddRange(New Object() {"Value", "Pointer", "Handler"})

                m_xmlBindingsList.AddBinding(control, Me, "Level", "SelectedIndex")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingSize(ByVal control As ComboBox)
        Try
            Dim myList As New ArrayList

            AddNodeList(myList, "//enumvalue")
            With control
                .DropDownStyle = ComboBoxStyle.DropDown
                .DisplayMember = "FullpathClassName"
                .ValueMember = "Id"
                .DataSource = myList
            End With
            m_xmlComboSize = New XmlBindingCombo(control, Me, "VarSize", "SizeRef")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingValue(ByVal control As ComboBox, Optional ByVal bValueOk As Boolean = False)
        Try
            If MyBase.ParentNodeName = "typedef" Then
                control.Enabled = False
            Else

                Dim myList As New ArrayList

                AddNodeList(myList, "//enumvalue")

                With control
                    .DropDownStyle = ComboBoxStyle.DropDown
                    .DisplayMember = "FullpathClassName"
                    .ValueMember = "Id"
                    .DataSource = myList
                End With
                m_xmlComboValue = New XmlBindingCombo(control, Me, "Value", "ValRef")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub LoadEnumeration(ByVal grid As XmlDataGridView)
        Try
            grid.Binding.LoadXmlNodes(Me, "enumvalue", "type_enumvalue_view")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub New(Optional ByRef xmlnode As XmlNode = Nothing)
        MyBase.New(xmlnode, False)
        m_xmlBindingsList = New XmlBindingsList
        Try
            m_xmlNodeManager = XmlNodeManager.GetInstance()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
