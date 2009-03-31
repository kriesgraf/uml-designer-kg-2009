Imports System
Imports System.Windows.Forms
Imports ClassXmlProject.XmlNodeListView
Imports ClassXmlProject.XmlProjectTools
Imports System.Xml
Imports System.Collections
Imports Microsoft.VisualBasic

Public Class XmlElementView
    Inherits XmlElementSpec
    Implements InterfGridViewNotifier
    Implements InterfViewControl
    Implements InterfViewForm

    Private m_xmlBindingsList As XmlBindingsList
    Private m_xmlComboTypedef As XmlBindingCombo
    Private m_xmlComboSize As XmlBindingCombo

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As dlgElement = New dlgElement
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance()
    End Sub

    Public Sub UpdateValues()
        m_xmlComboTypedef.Update()
        m_xmlComboSize.Update()
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Sub InitBindingBrief(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Comment")
    End Sub

    Public Sub InitBindingName(ByVal dataControl As TextBox)
        m_xmlBindingsList.AddBinding(dataControl, Me, "Name")
    End Sub

    Public Sub InitBindingType(ByVal dataControl As ComboBox)
        Try
            InitTypedefCombo(Me, dataControl)
            m_xmlComboTypedef = New XmlBindingCombo(dataControl, Me, "Descriptor", "Reference")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingSize(ByVal dataControl As ComboBox)
        Try
            InitValueCombo(Me, dataControl)

            m_xmlComboSize = New XmlBindingCombo(dataControl, Me, "VarSize", "SizeRef")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InitBindingModifier(ByVal control As Control)
        If Me.Tag <> ELanguage.Language_CplusPlus Then
            control.Enabled = False
            control.Visible = False
        Else
            m_xmlBindingsList.AddBinding(control, Me, "Modifier", "Checked")
        End If
    End Sub

    Public Sub InitBindingLevel(ByVal control As ComboBox, ByVal label As Label)
        If Me.Tag <> ELanguage.Language_CplusPlus Then
            control.Enabled = False
            control.Visible = False
            label.Visible = False
        Else
            control.DropDownStyle = ComboBoxStyle.DropDownList
            control.Items.AddRange(New Object() {"Value", "Pointer", "Handler"})
            m_xmlBindingsList.AddBinding(control, Me, "Level", "SelectedIndex")
        End If
    End Sub

    Public ReadOnly Property FullpathTypeDescription() As String
        Get
            Dim strResult As String = ""
            Try
                If Me.Node IsNot Nothing Then
                    strResult = MyBase.Descriptor + GetReferenceTypeDescription(MyBase.Reference, True)

                    If MyBase.Modifier Then strResult = "const " + strResult

                    strResult = strResult + DisplayLevel(MyBase.Level)

                    If MyBase.VarSize <> "" Or MyBase.SizeRef <> "" _
                    Then
                        strResult = strResult + " " + GetArrayString(CType(Me.Tag, ELanguage))
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return strResult
        End Get
    End Property

    Public Function EventClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventClick
        Try
            If dataMember = XmlTypeVarSpec.cstFullpathTypeDescription Then
                Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(Me)

                CType(fen, InterfFormDocument).DisableMemberAttributes()

                fen.ShowDialog()
                Return CType(fen.Tag, Boolean)
            End If
            Return False
        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Public Function EventDoubleClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventDoubleClick
        Try
            Dim fen As Form = XmlNodeManager.GetInstance().CreateForm(Me)
            fen.ShowDialog()
            Return CType(fen.Tag, Boolean)

        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Public Sub InitControl(ByVal control As System.Windows.Forms.Control) Implements InterfViewControl.InitControl
        Dim data As XmlDataGridView = CType(control, XmlDataGridView)

        With data
            .Columns.Clear()
            .AllowDrop = True
            .ColumnDragStart = 0   ' Name column
            .AutoGenerateColumns = False
            .AllowUserToDeleteRows = False
            .AllowUserToAddRows = True
            .AllowUserToResizeRows = True
            .AllowUserToOrderColumns = True
            .EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
        End With

        Dim col1 As DataGridViewColumn = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .DataPropertyName = "Name"
            .HeaderText = "Name"
            .Name = "ControlName_Name"
        End With
        data.Columns.Add(col1)

        Dim col2 As DataGridViewButtonColumn = New DataGridViewButtonColumn
        With col2
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .DataPropertyName = XmlTypeVarSpec.cstFullpathTypeDescription
            .HeaderText = "Type"
            .Name = "ControlName_Type"
            .ReadOnly = True
        End With
        data.Columns.Add(col2)

        col1 = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .DataPropertyName = "Comment"
            .HeaderText = "Comment"
            .Name = "ControlName_Comment"
        End With
        data.Columns.Add(col1)

    End Sub

    Public Function Compare(ByVal nodeName As String) As Integer Implements InterfViewControl.Compare
        Return 0
    End Function

    Private Function DisplayLevel(ByVal level As Integer) As String
        Dim strResult As String = ""
        Try
            Select Case level
                Case 1
                    strResult = "*"
                Case 2
                    strResult = "**"
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Private Function GetReferenceTypeDescription(ByVal szReferenceId As String, Optional ByVal bShorter As Boolean = False) As String
        Dim strResult As String = ""

        If szReferenceId Is Nothing Then
            Return ""
        ElseIf szReferenceId.Length = 0 Then
            Return ""
        End If

        Try
            Dim child As XmlNode = GetElementById(szReferenceId)
            strResult = GetName(child)

            Dim strSeparator As String = "::"
            If Me.Tag <> ELanguage.Language_CplusPlus Then
                strSeparator = "."
            End If

            If bShorter Then
                ' nothing to do 
            ElseIf child.Name = "reference" Or child.Name = "interface" Then
                strResult = GetPackage(child) + strSeparator + strResult
            Else
                If child.Name = "typedef" Then
                    child = child.ParentNode
                    strResult = GetName(child) + strSeparator + strResult
                    child = child.ParentNode
                Else
                    strResult = GetName(child)
                    child = child.ParentNode
                End If
                If child.Name <> "root" Then
                    strResult = GetName(child) + strSeparator + strResult
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub

    Public Function CanDropItem(ByVal component As XmlComponent) As Boolean Implements InterfGridViewNotifier.CanDropItem
        Me.DropInsertComponent(component)
        Return True
    End Function

    Public Function CanDragItem() As Boolean Implements InterfGridViewNotifier.CanDragItem
        Return True
    End Function
End Class
