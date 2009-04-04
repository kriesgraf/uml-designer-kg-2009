Imports System
Imports System.IO
Imports System.Xml
Imports System.Windows.Forms
Imports System.Collections
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

#Region "Public methods"

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

    Public Sub LoadValues()
        m_xmlBindingsList = New XmlBindingsList
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance
    End Sub

    Public Sub UpdateValues()
        If m_chkInterface.Checked Then
            If Me.InlineBody Is Nothing Then
                Me.InlineBody = CreateDocument("body", MyBase.Document)
                MyBase.AppendNode(Me.InlineBody.Node)
            End If
            Me.InlineBody.CodeSource = m_txtInterface.Text
        Else
            If Me.InlineBody IsNot Nothing Then Me.InlineBody.RemoveMe()
        End If
        m_xmlBindingsList.UpdateValues()
    End Sub

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

    Public Sub InitBindingReferences(ByVal listbox As ListBox)
        Try
            Dim listNode As New ArrayList
            Dim iterator As IEnumerator = MyBase.SelectNodes("descendant::reference | descendant::interface").GetEnumerator
            iterator.Reset()
            While iterator.MoveNext
                Dim xmlNode As XmlNode = TryCast(iterator.Current, XmlNode)
                listNode.Add(m_xmlNodeManager.CreateView(xmlNode, xmlNode.Name, MyBase.Document))
            End While
            listbox.DataSource = listNode
            listbox.DisplayMember = "Name"
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub AddNew(ByVal list As ListBox, ByVal name As String)
        Try
            If m_xmlExport Is Nothing Then
                m_xmlExport = CreateDocument("export", MyBase.Document)
                MyBase.AppendNode(m_xmlExport.Node)
            End If
            Dim xmlcpnt As XmlComponent = CreateDocument(name, MyBase.Document)

            m_xmlExport.AppendNode(xmlcpnt.Node)
            xmlcpnt.SetIdReference(m_xmlReferenceNodeCounter)

            InitBindingReferences(list)
            Me.Updated = True

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overloads Function AddReferences(ByVal eMode As EImportMode) As Boolean
        Dim bResult As Boolean = False
        Try
            If LoadImport(eMode) Then
                Me.Updated = True
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Overloads Function AddReferences(ByVal list As ListBox, ByVal eMode As EImportMode) As Boolean
        Dim bResult As Boolean = False
        Try
            If LoadImport(eMode) Then
                InitBindingReferences(list)
                Me.Updated = True
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function RemoveRedundant(ByVal component As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim fen As New dlgRedundancy
            fen.Document = Me
            fen.Node = component.Node
            If CType(fen.Document, XmlRefRedundancyView).GetListReferences IsNot Nothing Then
                fen.Text = "Check redundancies..."
                fen.Message = component.Name + " is redundant with:"
                fen.ShowDialog()
                If CType(fen.Tag, Boolean) = True Then
                    CheckReferences()
                    bResult = True
                End If
            Else
                MsgBox("No redundancies", MsgBoxStyle.Exclamation)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function RemoveRedundant(ByVal list As ListBox) As Boolean
        Dim bResult As Boolean = False
        Try
            If RemoveRedundant(TryCast(list.SelectedItem, XmlComponent)) Then
                InitBindingReferences(list)
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

    Public Overloads Function RemoveAllReferences() As Boolean
        Dim bResult As Boolean = False
        Try
            If RemoveComponent(m_xmlExport) Then
                bResult = True
                Me.Updated = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Overloads Function RemoveAllReferences(ByVal list As ListBox) As Boolean
        Dim bResult As Boolean = False
        Try
            If RemoveComponent(m_xmlExport) Then
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
                    InitBindingReferences(list)
                    Me.Updated = True
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub Duplicate(ByVal list As ListBox)
        Try
            If list.SelectedItem IsNot Nothing Then
                If MyBase.DuplicateReference(CType(list.SelectedItem, XmlComponent)) Then
                    InitBindingReferences(list)
                    Me.Updated = True
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
                    InitBindingReferences(list)
                    Me.Updated = True
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

#End Region

#Region "Private methods"

    Private Function LoadImport(ByVal eMode As EImportMode) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim dlgOpenFile As New OpenFileDialog
            If My.Settings.CurrentFolder = "." + Path.DirectorySeparatorChar.ToString Then
                dlgOpenFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            Else
                dlgOpenFile.InitialDirectory = My.Settings.CurrentFolder
            End If

            dlgOpenFile.Title = "Select a package references file..."
            dlgOpenFile.Filter = "Package references (*.ximp)|*.ximp|Doxygen TAG file (*.tag)|*.tag"

            If (dlgOpenFile.ShowDialog() = DialogResult.OK) _
            Then
                Dim FileName As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(dlgOpenFile.FileName)
                MyBase.LoadDocument(FileName, eMode)
                ' This class is used both in dlgImport and in frmProject
                If m_xmlBindingsList IsNot Nothing Then m_xmlBindingsList.ResetValues()
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

#End Region
End Class
