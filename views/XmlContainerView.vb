Imports System
Imports System.Windows.Forms
Imports System.Xml
Imports System.Collections
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.UmlNodesManager

Public Class XmlContainerView
    Inherits XmlContainerSpec
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    'Private m_xmlNodeManager As XmlNodeManager
    Private m_xmlComboTypedef As XmlBindingCombo
    Private m_xmlComboContainer As XmlBindingCombo
    Private m_xmlComboIndex As XmlBindingCombo

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As Form = Nothing
        If document.GetNode("descendant::list") IsNot Nothing Then
            frmResult = New dlgContainer
        ElseIf document.GetNode("descendant::element") IsNot Nothing Then
            frmResult = New dlgStructure
        Else
            frmResult = New dlgTypedef
        End If
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance()
    End Sub

    Public Sub LoadElementMembers(ByVal dataControl As XmlDataGridView)
        Try
            dataControl.Binding.LoadXmlNodes(Me.TypeVarDefinition, "element", "type_element_view")
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub UpdateValues()
        If m_xmlComboTypedef IsNot Nothing Then m_xmlComboTypedef.Update()
        If m_xmlComboContainer IsNot Nothing Then m_xmlComboContainer.Update()
        If m_xmlComboIndex IsNot Nothing Then m_xmlComboIndex.Update()
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Sub InitBindingRange(ByVal dataControl As ComboBox)
        dataControl.DropDownStyle = ComboBoxStyle.DropDownList
        dataControl.Items.AddRange(New Object() {"private", "protected", "public"})
        m_xmlBindingsList.AddBinding(dataControl, Me, "Range")
    End Sub

    Public Sub InitBindingBrief(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")
    End Sub

    Public Sub InitBindingName(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Name")
    End Sub

    Public Sub InitBindingType(ByVal dataControl As ComboBox)
        InitTypedefCombo(dataControl)
        m_xmlComboTypedef = New XmlBindingCombo(dataControl, Me.TypeVarDefinition, "Descriptor", "Reference")
    End Sub

    Public Sub InitBindingContainer(ByVal dataControl As ComboBox)
        Dim iContainer As Integer = 1
        If Me.TypeVarDefinition.Indexed Then iContainer = 2
        InitTypedefCombo(dataControl, iContainer)
        m_xmlComboContainer = New XmlBindingCombo(dataControl, Me.TypeVarDefinition, "ContainerDesc", "ContainerRef")
    End Sub

    Public Sub RefreshComboContainer(ByVal bIndexed As Boolean)
        Dim iContainer As Integer = 1
        If bIndexed Then iContainer = 2
        InitTypedefCombo(m_xmlComboContainer.Control, iContainer, True)
    End Sub

    Public Sub InitBindingComboIndex(ByVal dataControl As ComboBox)
        InitTypedefCombo(dataControl, -1)
        m_xmlComboIndex = New XmlBindingCombo(dataControl, Me.TypeVarDefinition, "IndexDesc", "IndexRef")
    End Sub

    Public Sub InitBindingLevel(ByVal dataControl As ComboBox, ByVal label As Label)
        If Me.Tag <> ELanguage.Language_CplusPlus Then
            dataControl.Enabled = False
            dataControl.Visible = False
            label.Visible = False
        Else
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"Value", "Pointer", "Handle"})
            m_xmlBindingsList.AddBinding(dataControl, Me.TypeVarDefinition, "Level", "SelectedIndex")
        End If
    End Sub

    Public Sub InitBindingIndexLevel(ByVal dataControl As ComboBox, ByVal label As Label)
        If Me.Tag <> ELanguage.Language_CplusPlus Then
            dataControl.Enabled = False
            dataControl.Visible = False
            label.Visible = False
        Else
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"Value", "Pointer", "Handle"})
            m_xmlBindingsList.AddBinding(dataControl, Me.TypeVarDefinition, "IndexLevel", "SelectedIndex")
        End If
    End Sub

    Public Sub InitBindingIterator(ByVal dataControl As CheckBox)
        If Me.Tag <> ELanguage.Language_CplusPlus Then
            dataControl.Enabled = False
            dataControl.Visible = False
        Else
            m_xmlBindingsList.AddBinding(dataControl, Me.TypeVarDefinition, "Iterator", "Checked")
        End If
    End Sub

    Public Sub InitBindingCheckIndex(ByVal dataControl As CheckBox)
        m_xmlBindingsList.AddBinding(dataControl, Me.TypeVarDefinition, "Indexed", "Checked")
    End Sub

    Public Sub InitBindingModifier(ByVal dataControl As CheckBox)
        If Me.Tag <> ELanguage.Language_CplusPlus Then
            dataControl.Enabled = False
            dataControl.Visible = False
        Else
            m_xmlBindingsList.AddBinding(dataControl, Me.TypeVarDefinition, "Modifier", "Checked")
        End If
    End Sub

    Private Sub InitTypedefCombo(ByVal dataControl As ComboBox, Optional ByVal iContainer As Integer = 0, Optional ByVal bClear As Boolean = False)
        Try
            Dim myList As New ArrayList

            If bClear Then
                dataControl.DataSource = Nothing
                dataControl.Items().Clear()
            End If

            If iContainer < 0 _
            Then
                ' Specific index type
                AddNodeList(myList, "//class[@implementation!='container']")
                AddNodeList(myList, "//typedef[@id!='" + Me.Id + "' and (type[@desc and not(list)] or type/enumvalue)]")

                AddSimpleTypesList(myList, Me.Tag)

            ElseIf iContainer > 0 _
            Then
                ' Specific container type
                AddNodeList(myList, "//class[@implementation='container' and model[last()=" + CStr(iContainer) + "]]")
                AddNodeList(myList, "//reference[@container='" + CStr(iContainer) + "']")
            Else
                ' Simple value type
                AddNodeList(myList, "//class[@implementation!='container']")
                AddNodeList(myList, "//typedef[@id!='" + Me.Id + "']")
                AddNodeList(myList, "//reference[@container='0' or not(@container)]")

                AddSimpleTypesList(myList, Me.Tag)
            End If

            myList.Sort(New XmlNodeListView("_comparer"))

            With dataControl
                .DropDownStyle = ComboBoxStyle.DropDown
                .DisplayMember = "FullpathClassName"
                .ValueMember = "Id"
                .DataSource = myList
            End With
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AddNodeList(ByRef myList As ArrayList, ByVal xpath As String)
        Dim iterator As IEnumerator = MyBase.SelectNodes(xpath).GetEnumerator
        iterator.Reset()

        'Debug.Print("xPath=" + xpath)

        While iterator.MoveNext
            Dim xmlcpnt As XmlNodeListView = New XmlNodeListView(CType(iterator.Current, XmlNode))
            xmlcpnt.Tag = Me.Tag
            'Debug.Print(xmlcpnt.FullpathClassName)
            myList.Add(xmlcpnt)
        End While
    End Sub

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub
End Class
