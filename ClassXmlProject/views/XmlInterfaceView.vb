Imports System
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

    Public Property InterfObject() As Object Implements InterfObject.InterfObject
        Get
            Return Me
        End Get
        Set(ByVal value As Object)
            ' Nothing to do
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

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgInterface
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

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
            m_xmlBindingsList.AddBinding(dataControl, Me, "Root", "Checked")

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
            dataControl.Binding.LoadXmlNodes(Me, "property | method", "interface_member_view", CType(Me, InterfObject))
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
            If MsgBox("Confirm to delete:" + vbCrLf + "Name: " + xmlcpnt.Name, _
                       cstMsgYesNoQuestion, _
                       xmlcpnt.Name) = MsgBoxResult.Yes _
            Then
                Dim strNodeName As String = removeNode.NodeName
                bResult = MyBase.RemoveComponent(removeNode)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Overrides Function CanAddComponent(ByVal nodeXml As XmlComponent) As Boolean
        Select Case nodeXml.NodeName
            Case "property"
                CType(nodeXml, XmlPropertySpec).OverridableProperty = True

            Case "method"
                If m_chkRoot.Checked Then
                    CType(nodeXml, XmlMethodSpec).Implementation = XmlProjectTools.EImplementation.Root
                Else
                    CType(nodeXml, XmlMethodSpec).Implementation = XmlProjectTools.EImplementation.Interf
                End If
        End Select
        Return True
    End Function

    Private Sub m_chkRoot_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_chkRoot.CheckedChanged
        If m_bInitOk Then Exit Sub
        m_grdMembers.Binding.ResetBindings()
    End Sub
End Class
