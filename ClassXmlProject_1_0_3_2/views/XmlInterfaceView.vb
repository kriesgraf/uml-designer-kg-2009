﻿Imports System
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlProjectTools

Public Class XmlInterfaceView
    Inherits XmlInterfaceSpec
    Implements InterfViewForm
    Implements InterfObject

    Private m_xmlBindingsList As XmlBindingsList
    Private m_grdMembers As XmlDataGridView
    Private WithEvents m_chkRoot As CheckBox
    Private m_bInitOk As Boolean = False

    Public ReadOnly Property Title() As String
        Get
            If Me.NodeName = "interface" Then
                Return "Interface " + Me.Name
            End If
            Return "Enum " + Me.Name
        End Get
    End Property

    Public Property InterfObject() As Object Implements InterfObject.InterfObject
        Get
            Return Me
        End Get
        Set(ByVal value As Object)
            ' Nothing to do
        End Set
    End Property

    Public Property ParentClass() As String
        Get
            Return GetAttribute("class")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("class")
            Else
                AddAttribute("class", value)
            End If
        End Set
    End Property

    Public ReadOnly Property CurrentClassImpl() As EImplementation
        Get
            If m_chkRoot.Checked Then
                Return EImplementation.Root
            End If
            Return EImplementation.Interf
        End Get
    End Property

    Public Sub New()
        m_xmlBindingsList = New XmlBindingsList
    End Sub

    Public Sub UpdateObject() Implements InterfObject.Update

    End Sub

    Public Sub LoadValues()
        m_xmlBindingsList.Init()
        m_xmlNodeManager = XmlNodeManager.GetInstance()
    End Sub

    Public Sub UpdateValues()
        m_xmlBindingsList.UpdateValues()
    End Sub

    Public Function SearchDependencies(ByVal component As XmlComponent) As Boolean
        Try
            If component Is Nothing Then Return False

            Dim bIsEmpty As Boolean = False
            If dlgDependencies.ShowDependencies(m_xmlReferenceNodeCounter, component, bIsEmpty) _
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
        Dim frmResult As New dlgInterface
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub InitMenuItems(ByVal AddEnumeration As ToolStripMenuItem, _
                             ByVal AddMethod As ToolStripMenuItem, _
                             ByVal AddProperty As ToolStripMenuItem)

        If Me.NodeName = "interface" Then
            AddMethod.Visible = True
            AddProperty.Visible = True
            AddEnumeration.Visible = False
        Else
            AddMethod.Visible = False
            AddProperty.Visible = False
            AddEnumeration.Visible = True
        End If
    End Sub

    Public Sub InitBindEnumClass(ByVal label As Label, ByVal text As TextBox)
        Try
            m_bInitOk = True

            If Me.NodeName = "reference" Then
                label.Visible = True
                text.Visible = True
                m_xmlBindingsList.AddBinding(text, Me, "ParentClass")
            Else
                label.Visible = False
                text.Visible = False
            End If
        Catch ex As Exception
            Throw ex
        Finally
            m_bInitOk = False
        End Try
    End Sub

    Public Sub InitBindingName(ByVal dataControl As Control)
        Try
            m_bInitOk = True
            m_xmlBindingsList.AddBinding(dataControl, Me, "Name")

        Catch ex As Exception
            Throw ex
        Finally
            m_bInitOk = False
        End Try
    End Sub

    Public Sub InitBindingRoot(ByVal dataControl As CheckBox)
        Try
            m_bInitOk = True
            m_chkRoot = dataControl

            If Me.NodeName = "interface" Then
                m_xmlBindingsList.AddBinding(dataControl, Me, "Root", "Checked")
            Else
                m_chkRoot.Visible = False
            End If

        Catch ex As Exception
            Throw ex
        Finally
            m_bInitOk = False
        End Try
    End Sub

    Public Sub InitBindingPackage(ByVal dataControl As Control)
        Try
            m_bInitOk = True
            m_xmlBindingsList.AddBinding(dataControl, Me, "Package")

        Catch ex As Exception
            Throw ex
        Finally
            m_bInitOk = False
        End Try
    End Sub

    Public Sub LoadMembers(ByVal dataControl As XmlDataGridView)
        Try
            m_bInitOk = True
            m_grdMembers = dataControl
            If Me.NodeName = "interface" Then
                dataControl.Binding.LoadXmlNodes(Me, "property | method", "interface_member_view", CType(Me, InterfObject))
            Else
                dataControl.Binding.LoadXmlNodes(Me, "enumvalue", "type_enumvalue_view")
            End If
            dataControl.Binding.NodeCounter = Nothing

        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bInitOk = False
        End Try
    End Sub

    Public Overrides Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim xmlcpnt As XmlComponent = CreateDocument(removeNode.Node)
            xmlcpnt.GenerationLanguage = Me.GenerationLanguage

            If MsgBox("Confirm to delete:" + vbCrLf + "Name: " + xmlcpnt.Name, _
                       cstMsgYesNoQuestion, _
                       xmlcpnt.Name) = MsgBoxResult.Yes _
            Then
                Dim strNodeName As String = removeNode.NodeName
                bResult = MyBase.RemoveComponent(removeNode)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Private Sub m_chkRoot_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_chkRoot.CheckedChanged
        If m_bInitOk Then Exit Sub
        m_grdMembers.Binding.ResetBindings()
    End Sub
End Class
