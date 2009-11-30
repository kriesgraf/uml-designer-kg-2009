Imports System
Imports System.IO
Imports System.Xml
Imports System.Windows.Forms
Imports System.Collections
Imports ClassXmlProject.XmlNodeListView
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class XmlImportView
    Inherits XmlImportSpec
    Implements InterfViewForm

#Region "Class declarations"

    Private m_txtInterface, m_txtParam As TextBox
    Private m_chkInterface As CheckBox
    Private m_bClassInterface As Boolean
    Private m_xmlBindingsList As XmlBindingsList

#End Region

#Region "Public properties"

    Public ReadOnly Property ClassImport() As Boolean
        Get
            Dim xmlNode As XmlNode = GetNode("parent::*")
            If xmlNode IsNot Nothing Then
                Return (xmlNode.Name = "class")
            End If
            Return False
        End Get
    End Property

    Public Property ClassInterface() As Boolean
        Get
            Dim xmlNode As XmlNode = GetNode("body/line")
            If xmlNode IsNot Nothing Then
                m_bClassInterface = True
                Return True
            End If
            m_bClassInterface = False
            Return False
        End Get
        Set(ByVal value As Boolean)
            m_bClassInterface = value
        End Set
    End Property

    Public ReadOnly Property CurrentParameter() As String
        Get
            Return m_txtParam.Text.Trim()
        End Get
    End Property
#End Region

#Region "Public methods"

    Public Sub LoadValues()
        m_xmlBindingsList = New XmlBindingsList
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance
    End Sub

    Public Sub UpdateValues()
        If m_chkInterface.Checked Then
            If Me.InlineBody Is Nothing Then
                Me.InlineBody = CreateDocument("body", Me.Document)
                AppendNode(Me.InlineBody.Node)
            End If
            Me.InlineBody.CodeSource = m_txtInterface.Text
        Else
            If Me.InlineBody IsNot Nothing Then Me.InlineBody.RemoveMe()
        End If
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Function SearchDependencies(ByVal component As XmlComponent) As Boolean

        If component Is Nothing Then Return False

        Dim bIsEmpty As Boolean = False
        If dlgDependencies.ShowDependencies(m_xmlReferenceNodeCounter, component, bIsEmpty) _
        Then
            Me.Updated = True
            Return True
        End If

        Return False
    End Function

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgImport
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingName(ByVal dataControl As Control)

        m_xmlBindingsList.AddBinding(dataControl, Me, "Name")

    End Sub

    Public Sub InitBindingFullpathName(ByVal dataControl As TextBox)

        m_txtParam = dataControl
        m_xmlBindingsList.AddBinding(dataControl, Me, "Parameter")

    End Sub

    Public Sub InitBindingVisibility(ByVal dataControl As ComboBox)

        dataControl.DropDownStyle = ComboBoxStyle.DropDownList
        dataControl.Items.AddRange(New Object() {"common", "package"})
        m_xmlBindingsList.AddBinding(dataControl, Me, "Visibility", "SelectedItem")

    End Sub

    Public Sub InitBindingInterface(ByVal dataControl As CheckBox)
        dataControl.Checked = Me.ClassInterface
        m_chkInterface = dataControl
    End Sub

    Public Sub InitBindingBodyInterface(ByVal dataControl As TextBox)
        If Me.InlineBody IsNot Nothing Then dataControl.Text = Me.InlineBody.CodeSource()
        m_txtInterface = dataControl
    End Sub

    Public Sub InitBindingListReferences(ByVal listbox As ListBox, Optional ByVal bClear As Boolean = False)

        Dim listNode As New ArrayList
        AddNodeList(Me, listNode, "descendant::reference | descendant::interface", Me)
        SortNodeList(listNode)

        listbox.DataSource = listNode
        listbox.DisplayMember = cstFullpathClassName
        listbox.SelectionMode = SelectionMode.MultiExtended

    End Sub

    Public Sub AddNew(ByVal list As ListBox, ByVal name As String)

        Dim xmlcpnt As XmlComponent = CreateDocument(name, Me.Document)
        xmlcpnt.GenerationLanguage = Me.GenerationLanguage
        xmlcpnt.SetIdReference(m_xmlReferenceNodeCounter)
        AppendComponent(xmlcpnt)

        InitBindingListReferences(list, True)
        Me.Updated = True

    End Sub

    Public Function RenamePackage(ByVal list As ListBox) As Boolean
        Dim bResult As Boolean = False

        Dim fen As New dlgRenameNode
        fen.Title = "Rename package"
        fen.Tag = Me.GenerationLanguage
        fen.Label = "Enter new package label"

        If fen.ShowDialog() = DialogResult.OK Then
            For Each element As XmlComponent In list.SelectedItems
                If fen.Result = "" Then
                    XmlProjectTools.RemoveAttribute(element.Node, "package")
                Else
                    XmlProjectTools.AddAttributeValue(element.Node, "package", fen.Result)
                End If
                Me.Updated = True
                bResult = True
            Next
        End If

        If bResult Then
            InitBindingListReferences(list, True)
        End If

        Return bResult
    End Function

    Public Overloads Function AddReferences(ByVal form As Form, ByVal eMode As EImportMode) As Boolean
        Dim bResult As Boolean = False

        If LoadImport(form, eMode) Then
            Me.Updated = True
            bResult = True
        End If

        Return bResult
    End Function

    Public Overloads Function AddReferences(ByVal form As Form, ByVal eMode As EImportMode, ByVal list As ListBox) As Boolean
        Dim bResult As Boolean = False

        If LoadImport(form, eMode) Then
            InitBindingListReferences(list, True)
            Me.Updated = True
            bResult = True
        End If

        Return bResult
    End Function

    Public Function RemoveRedundantReference(ByVal list As ListBox) As Boolean
        Dim bResult As Boolean = False

        If RemoveRedundant(TryCast(list.SelectedItem, XmlComponent)) Then
            InitBindingListReferences(list, True)
            Me.Updated = True
            If Me.GetNode("descendant::reference | descendant::interface") Is Nothing Then
                If MsgBox("Do you want to remove the import " + Me.Name + " too ?", _
                          cstMsgYesNoQuestion, "'Remove' command") _
                          = MsgBoxResult.Yes Then
                    bResult = Me.RemoveMe()
                End If
            End If
        End If

        Return bResult
    End Function

    Public Function PasteReference(ByVal list As ListBox) As Boolean

        ' Get back from the specific clipboard shared by all projects
        Dim bCopy As Boolean
        Dim bImportData As Boolean = XmlComponent.Clipboard.CheckData(Me, bCopy)  ' Check if data comme from other document
        Dim component = XmlComponent.Clipboard.Data
        If bCopy = False And bImportData _
        Then
            MsgBox("Sorry, can't cut object from one project and paste to another!", MsgBoxStyle.Exclamation, "'Paste' command")

        ElseIf Me.ChildExportNode.CanPasteItem(XmlComponent.Clipboard.Data) _
        Then
            If PasteOrDuplicate(component, bCopy) Then
                InitBindingListReferences(list, True)
                Me.Updated = True
                Return True
            Else
                MsgBox("Sorry can't paste node '" + component.NodeName + "' !", MsgBoxStyle.Exclamation, "'Paste' command")
            End If
        Else
            MsgBox("Sorry can't paste node '" + component.NodeName + "' !", MsgBoxStyle.Exclamation, "'Paste' command")
        End If

        Return False
    End Function

    Public Function RemoveAllReferences() As Boolean
        Dim bResult As Boolean = False

        If RemoveComponent(Me.ChildExportNode) Then
            bResult = True
            Me.Updated = True
        End If

        Return bResult
    End Function

    Public Function RemoveAllReferences(ByVal list As ListBox) As Boolean
        Dim bResult As Boolean = False
        Try
            If RemoveComponent(Me.ChildExportNode) Then
                list.DataSource = Nothing
                list.Items.Clear()
                Me.Updated = True
                bResult = True
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Sub Delete(ByVal list As ListBox)
        Try
            If list.SelectedItem IsNot Nothing Then
                Dim document As XmlComponent = CType(list.SelectedItem, XmlComponent)
                If MyBase.CanRemove(document) Then
                    If MyBase.RemoveComponent(document) Then
                        InitBindingListReferences(list, True)
                        Me.Updated = True
                    End If
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub DuplicateReference(ByVal list As ListBox)
        Try
            If list.SelectedItem IsNot Nothing Then
                Dim component As XmlComponent = CType(list.SelectedItem, XmlComponent)
                If PasteOrDuplicate(component) Then
                    InitBindingListReferences(list, True)
                    Me.Updated = True
                Else
                    MsgBox("Sorry can't duplicate node '" + component.NodeName + "' !", MsgBoxStyle.Exclamation, "'Duplicate' command")
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub Edit(ByVal list As ListBox)
        Try
            If list.SelectedItem IsNot Nothing _
            Then
                Dim fen As Form = m_xmlNodeManager.CreateForm(CType(list.SelectedItem, XmlComponent))
                Dim InterfCounter As InterfNodeCounter = TryCast(fen, InterfNodeCounter)
                If InterfCounter IsNot Nothing Then InterfCounter.NodeCounter = m_xmlReferenceNodeCounter

                fen.ShowDialog()
                If CType(fen.Tag, Boolean) Then
                    InitBindingListReferences(list, True)
                    Me.Updated = True
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub MoveUpReference(ByVal list As ListBox)
        Try
            If list.SelectedItem IsNot Nothing Then
                If MoveUpComponent(CType(list.SelectedItem, XmlComponent)) Then
                    InitBindingListReferences(list, True)
                    Me.Updated = True
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub
#End Region

#Region "Private methods"

    Private Function LoadImport(ByVal form As Form, ByVal eMode As EImportMode) As Boolean
        Dim bResult As Boolean = False

        Dim dlgOpenFile As New OpenFileDialog
        If eMode = EImportMode.MergeReferences _
            And SelectNodes("descendant::reference|descendant::interface").Count = 0 _
        Then
            MsgBox("Import is empty, use quite 'replace' or 'confirm' command!", MsgBoxStyle.Exclamation, "'Import' command")
            Return False

        ElseIf My.Settings.CurrentFolder = "." + Path.DirectorySeparatorChar.ToString Then
            dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        Else
            dlgOpenFile.InitialDirectory = My.Settings.CurrentFolder
        End If

        dlgOpenFile.Title = "Select a package references file..."
        dlgOpenFile.Filter = "Package references (*.ximp)|*.ximp|Doxygen TAG file (*.tag)|*.tag"

        If (dlgOpenFile.ShowDialog() = DialogResult.OK) _
        Then
            Dim FileName As FileInfo = My.Computer.FileSystem.GetFileInfo(dlgOpenFile.FileName)
            If LoadDocument(form, FileName, eMode) Then

                ExtractExternalReferences(Me.Node.ParentNode, ChildExportNode.Node)

                ' This class is used both in dlgImport and in frmProject
                If m_xmlBindingsList IsNot Nothing Then m_xmlBindingsList.ResetValues()
                bResult = True
            End If
        End If

        Return bResult
    End Function

    Private Function PasteOrDuplicate(ByVal component As XmlComponent, _
                                      Optional ByVal bDuplicate As Boolean = True, _
                                      Optional ByVal bImportData As Boolean = False) As Boolean
        Dim bResult As Boolean = False

        Dim xmlComponent As XmlComponent = component

        If bDuplicate And bImportData = False _
            Then
            xmlComponent = Me.DuplicateComponent(component)

        ElseIf bImportData _
        Then
            xmlComponent = Me.ImportDocument(component)
        End If

        If xmlComponent IsNot Nothing Then
            xmlComponent.GenerationLanguage = Me.GenerationLanguage
            xmlComponent.SetIdReference(m_xmlReferenceNodeCounter, ENameReplacement.AddCopyName)

            If bDuplicate And bImportData = False _
                Then
                xmlComponent.Name = component.Name + "_copy"
                xmlComponent.SetIdReference(m_xmlReferenceNodeCounter, ENameReplacement.AddCopyName)

            ElseIf bImportData _
            Then
                xmlComponent.Name = component.Name + "_imported"
                xmlComponent.SetIdReference(m_xmlReferenceNodeCounter, ENameReplacement.AddCopyName, True)
            End If

            If Me.ChildExportNode.AppendComponent(xmlComponent) IsNot Nothing Then
                bResult = True
            End If
        End If

        Return bResult
    End Function
#End Region
End Class
