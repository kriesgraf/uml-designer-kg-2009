﻿Imports System
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

    Private m_txtInterface As TextBox
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
        Try
            If component Is Nothing Then Return False

            Dim bIsEmpty As Boolean = False
            If dlgDependencies.ShowDependencies(component, bIsEmpty) _
            Then
                Me.Updated = True
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgImport
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitBindingName(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingFullpathName(ByVal dataControl As Control)
        Try
            m_xmlBindingsList.AddBinding(dataControl, Me, "Parameter")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingVisibility(ByVal dataControl As ComboBox)
        Try
            dataControl.DropDownStyle = ComboBoxStyle.DropDownList
            dataControl.Items.AddRange(New Object() {"common", "package"})
            m_xmlBindingsList.AddBinding(dataControl, Me, "Visibility", "SelectedItem")
        Catch ex As Exception
            Throw ex
        End Try
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
        Try
            Dim listNode As New ArrayList
            AddNodeList(Me, listNode, "descendant::reference | descendant::interface")
            SortNodeList(listNode)

            listbox.DataSource = listNode
            listbox.DisplayMember = cstFullpathClassName
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub AddNew(ByVal list As ListBox, ByVal name As String)
        Try
            Dim xmlcpnt As XmlComponent = CreateDocument(name, Me.Document)
            xmlcpnt.Tag = Me.Tag
            xmlcpnt.SetIdReference(m_xmlReferenceNodeCounter)
            AppendComponent(xmlcpnt)

            InitBindingListReferences(list, True)
            Me.Updated = True

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overloads Function AddReferences(ByVal form As Form, ByVal eMode As EImportMode) As Boolean
        Dim bResult As Boolean = False
        Try
            If LoadImport(form, eMode) Then
                Me.Updated = True
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Overloads Function AddReferences(ByVal form As Form, ByVal eMode As EImportMode, ByVal list As ListBox) As Boolean
        Dim bResult As Boolean = False
        Try
            If LoadImport(form, eMode) Then
                InitBindingListReferences(list, True)
                Me.Updated = True
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function RemoveRedundantReference(ByVal list As ListBox) As Boolean
        Dim bResult As Boolean = False
        Try
            If RemoveRedundant(TryCast(list.SelectedItem, XmlComponent)) Then
                InitBindingListReferences(list, True)
                Me.Updated = True
                If Me.GetNode("descendant::reference | descendant::interface") Is Nothing Then
                    If MsgBox("Do you want to remove the import " + Me.Name + " too ?", _
                              cstMsgYesNoQuestion) _
                              = MsgBoxResult.Yes Then
                        bResult = Me.RemoveMe()
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function PasteReference(ByVal list As ListBox) As Boolean
        Try
            ' Get back from the specific clipboard shared by all projects
            Dim bCopy As Boolean
            Dim component As XmlComponent = XmlComponent.Clipboard.GetData(bCopy)

            If Me.ChildExportNode.CanPasteItem(component) _
            Then
                If PasteOrDuplicate(component, bCopy) Then
                    InitBindingListReferences(list, True)
                    Me.Updated = True
                    Return True
                Else
                    MsgBox("Sorry can't paste " + component.NodeName + " '" + component.Name + "' !", MsgBoxStyle.Exclamation)
                End If
            Else
                MsgBox("Sorry can't paste " + component.NodeName + " '" + component.Name + "' !", MsgBoxStyle.Exclamation)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Public Overrides Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Select Case removeNode.NodeName
                Case "reference", "interface"
                    If SelectNodes(GetQueryListDependencies(removeNode)).Count > 0 Then

                        If MsgBox("Some elements reference this, you can dereference them and then this will be deleted." + _
                                  vbCrLf + "Do you want to proceed", _
                                    cstMsgYesNoQuestion, _
                                    removeNode.Name) = MsgBoxResult.Yes _
                        Then
                            Dim bIsEmpty As Boolean = False

                            If dlgDependencies.ShowDependencies(removeNode, bIsEmpty, "Remove references to " + removeNode.Name) Then
                                bResult = True
                            End If

                            If bIsEmpty = False Then
                                Return bResult
                            End If
                        Else
                            Return False
                        End If
                    End If
                    bResult = MyBase.RemoveComponent(removeNode)

                Case "export"
                    bResult = MyBase.RemoveComponent(removeNode)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function RemoveAllReferences() As Boolean
        Dim bResult As Boolean = False
        Try
            If RemoveComponent(Me.ChildExportNode) Then
                bResult = True
                Me.Updated = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
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
                If RemoveComponent(CType(list.SelectedItem, XmlComponent)) Then
                    InitBindingListReferences(list, True)
                    Me.Updated = True
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
                    MsgBox("Sorry can't duplicate " + component.NodeName + " '" + component.Name + "' !", MsgBoxStyle.Exclamation)
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub Edit(ByVal list As ListBox)
        Try
            If list.SelectedItem IsNot Nothing Then
                Dim fen As Form
                fen = m_xmlNodeManager.CreateForm(CType(list.SelectedItem, XmlComponent))
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
#End Region

#Region "Private methods"

    Private Function LoadImport(ByVal form As Form, ByVal eMode As EImportMode) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If eMode = EImportMode.MergeReferences _
                And SelectNodes("descendant::reference|descendant::interface").Count = 0 _
            Then
                MsgBox("Import is empty, use quite 'replace' or 'confirm' command!", MsgBoxStyle.Exclamation)
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
                Dim FileName As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(dlgOpenFile.FileName)
                LoadDocument(form, FileName, eMode)
                ' This class is used both in dlgImport and in frmProject
                If m_xmlBindingsList IsNot Nothing Then m_xmlBindingsList.ResetValues()
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Private Function PasteOrDuplicate(ByVal component As XmlComponent, Optional ByVal bDuplicate As Boolean = True) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim xmlComponent As XmlComponent = component

            If bDuplicate Then
                xmlComponent = Me.DuplicateComponent(component)
            End If

            If xmlComponent IsNot Nothing Then
                xmlComponent.Tag = Me.Tag
                xmlComponent.SetIdReference(m_xmlReferenceNodeCounter, True)

                If bDuplicate Then
                    xmlComponent.Name = component.Name + "_copy"
                    If Me.ChildExportNode.AppendComponent(xmlComponent) IsNot Nothing Then
                        bResult = True
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function
#End Region
End Class
