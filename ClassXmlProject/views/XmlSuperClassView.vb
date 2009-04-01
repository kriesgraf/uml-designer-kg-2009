Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class XmlSuperClassView
    Inherits XmlClassSpec
    Implements InterfViewForm

    Private m_ListBox As ListBox
    Private m_eImplementation As EImplementation

    Public Property CurrentImplementation() As EImplementation
        Get
            Return m_eImplementation
        End Get
        Set(ByVal value As EImplementation)
            m_eImplementation = value
        End Set
    End Property

    Public Sub New(Optional ByRef xmlnode As XmlNode = Nothing)
        MyBase.New(xmlnode, False)
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgSuperClass
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Public Sub UpdateValues()
        Try
            For Each xmlview As XmlNodeListView In m_ListBox.SelectedItems

                Dim xmlcpnt As XmlInheritSpec = MyBase.CreateDocument("inherited", MyBase.Document)
                xmlcpnt.Idref = xmlview.Id

                MyBase.AppendNode(xmlcpnt.Node)
            Next xmlview

            ' Overrides some properties
            Dim xmlProperty As XmlClassOverridePropertiesView = XmlNodeManager.GetInstance().CreateView(Me.Node, "class_override_properties")
            xmlProperty.OverrideProperties(m_eImplementation)

            ' Overrides some methods
            Dim xmlMethod As XmlClassOverrideMethodsView = XmlNodeManager.GetInstance().CreateView(Me.Node, "class_override_methods")
            xmlMethod.OverrideMethods(m_eImplementation)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function InitAvailableClassesList(ByVal bRestrictedVisibility As Boolean, ByVal lsbControl As ListBox) As Boolean
        Try
            XmlNodeListView.InitInheritedListBox(Me, lsbControl, m_eImplementation, bRestrictedVisibility)
            m_ListBox = lsbControl
            m_ListBox.SelectedIndex = -1

        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
