Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports System.Collections
Imports ClassXmlProject.XmlProjectTools

Public Class XmlMethodExceptionView
    Inherits XmlMethodSpec
    Implements InterfViewForm

    Private m_strImplementation As String
    Private m_ListBox As ListBox

    Public Property CurrentImplementation() As String
        Get
            Return m_strImplementation
        End Get
        Set(ByVal value As String)
            m_strImplementation = value
        End Set
    End Property

    Public Sub New(Optional ByRef xmlnode As XmlNode = Nothing)
        MyBase.New(xmlnode, False)
    End Sub

    Public Function CreateForm(ByVal document As XmlComponent) As System.Windows.Forms.Form Implements InterfViewForm.CreateForm
        Dim frmResult As New dlgException
        CType(frmResult, InterfFormDocument).Document = document
        Return frmResult
    End Function

    Private Sub AppendListBox(ByVal xmlList As XmlNodeList, ByRef list As ArrayList)
        If list Is Nothing Then
            list = New ArrayList
        End If

        Dim iterator As IEnumerator = xmlList.GetEnumerator()

        While iterator.MoveNext
            ' TODO : create a prototype in class XmlNodeManager
            ' Note: this class is used as document and view !
            Dim xmlcpnt As XmlExceptionSpec = New XmlExceptionSpec(CType(iterator.Current, XmlNode))
            'xmlcpnt.Document = MyBase.Document
            Debug.Print(xmlcpnt.ToString + "(" + xmlcpnt.Idref + ")")
            list.Add(xmlcpnt)
        End While
    End Sub

    Public Sub UpdateValues()
        Try
            For Each xmlNode As XmlExceptionSpec In m_ListBox.SelectedItems

                Dim xmlcpnt As XmlExceptionSpec = MyBase.CreateDocument("exception", MyBase.Document)
                xmlcpnt.Idref = xmlNode.Idref

                MyBase.AppendNode(xmlcpnt.Node)

            Next xmlNode
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Function InitAvailableClassesList(ByVal bRestrictedVisibility As Boolean, ByVal lsbControl As ListBox) As Boolean

        Dim strQuery As String = "@implementation='exception'"
        Dim xmlList As XmlNodeList
        Dim strIgnoredClasses As String = ""

        xmlList = MyBase.SelectNodes("exception")

        Dim iterator As IEnumerator = xmlList.GetEnumerator()

        ' We ignore class ID that are already in exception list
        While iterator.MoveNext
            strIgnoredClasses = strIgnoredClasses + GetCurrentIDREF(iterator) + ";"
        End While

        If strIgnoredClasses <> "" Then
            strIgnoredClasses = "not(contains('" + strIgnoredClasses + "',concat(@id,';')))"
        End If

        ' Query removes ignored ID
        strQuery = "class[" + strQuery + "]"

        If strIgnoredClasses <> "" Then
            strQuery += "[" + strIgnoredClasses + "]"
        End If
        Dim collList As ArrayList = Nothing

        Dim strTempo As String

        ' We ignore classes that are not in current package, if it's required
        If bRestrictedVisibility = False Then
            strTempo = "//" + strQuery + "[@visibility='package']"
            xmlList = MyBase.SelectNodes(strTempo)
            AppendListBox(xmlList, collList)
        End If

        strTempo = "parent::*/" + strQuery
        xmlList = MyBase.SelectNodes(strTempo)
        AppendListBox(xmlList, collList)

        strTempo = "//reference[@type='exception']"
        If strIgnoredClasses <> "" Then
            strTempo += "[" + strIgnoredClasses + "]"
        End If

        xmlList = MyBase.SelectNodes(strTempo)
        AppendListBox(xmlList, collList)

        lsbControl.DataSource = collList
        lsbControl.DisplayMember = "FullpathClassName"
        m_ListBox = lsbControl
    End Function

End Class
