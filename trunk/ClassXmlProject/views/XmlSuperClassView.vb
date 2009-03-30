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

    Private Sub AppendArrayList(ByVal xmlList As XmlNodeList, ByRef list As ArrayList)
        If list Is Nothing Then
            list = New ArrayList
        End If

        Dim iterator As IEnumerator = xmlList.GetEnumerator()

        While iterator.MoveNext
            ' TODO : create a prototype in class XmlNodeManager
            Dim xmlcpnt As XmlNodeListView = New XmlNodeListView(CType(iterator.Current, XmlNode))
            xmlcpnt.Tag = Me.Tag
            list.Add(xmlcpnt)
        End While
    End Sub

    Public Sub UpdateValues()
        Try
            For Each xmlview As XmlNodeListView In m_ListBox.SelectedItems

                Dim xmlcpnt As XmlInheritSpec = MyBase.CreateDocument("inherited", MyBase.Document)
                xmlcpnt.Idref = xmlview.Id

                MyBase.AppendNode(xmlcpnt.Node)
            Next xmlview
            ' Overrides some methods
            Dim xmlOverrides As XmlClassOverrideMethodsView = XmlNodeManager.GetInstance().CreateView(Me.Node, "class_override_methods")
            xmlOverrides.OverrideMethods(m_eImplementation)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function InitAvailableClassesList(ByVal bRestrictedVisibility As Boolean, ByVal lsbControl As ListBox) As Boolean
        Dim strQuery As String = ""

        ' Implementation can change in dlgClass, also we can't get property 'MyBase.Implementation' that contains initial value !
        Select Case m_eImplementation
            Case EImplementation.Simple
                strQuery = "@implementation='simple' or @implementation='exception' or @implementation='abstract'"
            Case EImplementation.Exception
                strQuery = "@implementation='simple' or @implementation='exception' or @implementation='abstract'"
            Case EImplementation.Container
                strQuery = "@implementation='simple' or @implementation='exception' or @implementation='abstract'"
            Case EImplementation.Node
                strQuery = "@implementation='virtual' or @implementation='root' or @implementation='abstract'"
            Case EImplementation.Leaf
                strQuery = "@implementation='virtual' or @implementation='root' or @implementation='abstract'"
            Case EImplementation.Root
                strQuery = "@implementation='abstract'"
            Case Else
                Throw New Exception("'CurrentImplementation' property not yet assigned")
        End Select

        If strQuery = "" Then
            lsbControl.Items.Clear()
            lsbControl.Items.Add("Sorry no inheritance available")
            lsbControl.Enabled = False
            Exit Function
        End If

        Dim xmlList As XmlNodeList
        Dim strIgnoredClasses As String

        xmlList = MyBase.SelectNodes("inherited")

        ' We ignore current class ID !
        strIgnoredClasses = MyBase.Id + ";"

        Dim iterator As IEnumerator = xmlList.GetEnumerator()

        ' We ignore class ID that are already in heritance
        While iterator.MoveNext
            strIgnoredClasses = strIgnoredClasses + GetCurrentIDREF(iterator) + ";"
        End While

        strIgnoredClasses = "not(contains('" + strIgnoredClasses + "',concat(@id,';')))"

        ' Query removes ignored ID
        strQuery = "class[" + strIgnoredClasses + "][" + strQuery + "]"

        Dim collList As ArrayList = Nothing

        Dim strTempo As String

        ' We ignore classes that are not in current package, if it's required
        If bRestrictedVisibility = False Then
            strTempo = "//" + strQuery + "[@visibility='package']"
            xmlList = MyBase.SelectNodes(strTempo)
            AppendArrayList(xmlList, collList)
        Else
            strTempo = "parent::*/" + strQuery
            xmlList = MyBase.SelectNodes(strTempo)
            AppendArrayList(xmlList, collList)
        End If

        strTempo = "//reference[@type!='typedef'][" + strIgnoredClasses + "]"
        xmlList = MyBase.SelectNodes(strTempo)
        AppendArrayList(xmlList, collList)

        If collList.Count = 0 Then
            lsbControl.Items.Clear()
            lsbControl.Items.Add("Sorry no inheritance available")
            lsbControl.Enabled = False
            Exit Function
        End If

        collList.Sort(New XmlNodeListView("_comparer"))

        lsbControl.Enabled = True
        lsbControl.DataSource = collList
        lsbControl.DisplayMember = "FullpathClassName"

        m_ListBox = lsbControl
        m_ListBox.SelectedIndex = -1
    End Function

End Class
