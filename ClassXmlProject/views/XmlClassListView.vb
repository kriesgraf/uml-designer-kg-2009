Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports System.Collections
Imports ClassXmlProject.XmlProjectTools

Public Class XmlClassListView
    Inherits XmlComponent
    Implements IComparer

    Public Const cstValueMember As String = "Id"
    Public Const cstDisplayMember As String = "FullpathClassName"


    Public Property Id() As String
        Get
            Return GetAttribute("id")
        End Get
        Set(ByVal value As String)
            SetAttribute("id", value)
        End Set
    End Property

    Public ReadOnly Property FullpathClassName() As String
        Get
            Dim eLang As ELanguage = Me.GenerationLanguage
            Dim strResult As String = GetFullpathDescription(Me.Node, eLang)
            If DEBUG_COMMANDS_ACTIVE Then strResult += " (" + eLang.ToString + ")"
            Return strResult
        End Get
    End Property

    Public Sub New(ByVal nodeXml As XmlNode)
        MyBase.New(nodeXml)
    End Sub

    Public Shared Sub AddListComboBoxColumn(ByVal document As XmlComponent, _
                                            ByVal DataPropertyName As String, _
                                            ByVal HeaderText As String, _
                                            ByVal column As DataGridViewColumn)

        Dim myList As New ArrayList
        ' Only kind of classes
        AddNodeList(document, myList, "//class[@implementation!='container']")
        AddNodeList(document, myList, "//interface")
        AddNodeList(document, myList, "//reference[@type!='typedef'][@container='0' or not(@container)]")

        If myList.Count = 0 Then
            myList = Nothing
        End If
        With CType(column, DataGridViewComboBoxColumn)
            'Debug.Print("DataPropertyName=" + DataPropertyName + ";ValueMember=" + XmlClassListView.cstValueMember)
            .Name = "ControlName_" + DataPropertyName

            .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            .DisplayStyleForCurrentCellOnly = True
            .DataPropertyName = DataPropertyName
            .HeaderText = HeaderText
            .MaxDropDownItems = 10
            .FlatStyle = FlatStyle.Flat

            If myList.Count > 0 Then
                .DataSource = myList
            Else
                .DataSource = Nothing
            End If
            .ValueMember = cstValueMember
            .DisplayMember = cstDisplayMember
        End With
    End Sub

    Public Shared Sub InitTypedefCombo(ByVal document As XmlComponent, ByVal control As ComboBox)

        Dim myList As New ArrayList

        AddNodeList(document, myList, "//class[@implementation!='container']")
        AddNodeList(document, myList, "//interface")
        AddNodeList(document, myList, "//reference[@type!='typedef'][@container='0' or not(@container)]")

        With control
            .DropDownStyle = ComboBoxStyle.DropDownList

            If myList.Count > 0 Then
                .DataSource = myList
            Else
                .DataSource = Nothing
            End If
            .ValueMember = cstValueMember
            .DisplayMember = cstDisplayMember
        End With
    End Sub

    Public Overrides Function CompareComponent(ByVal x As Object, ByVal y As Object) As Integer
        Dim str1 As String = CType(x, XmlClassListView).FullpathClassName
        Dim str2 As String = CType(y, XmlClassListView).FullpathClassName
        Return Comparer.DefaultInvariant.Compare(str1, str2)
    End Function

    Private Shared Sub AddNodeList(ByVal document As XmlComponent, ByRef myList As ArrayList, ByVal xpath As String)
        Dim iterator As IEnumerator = document.Document.SelectNodes(xpath).GetEnumerator
        iterator.Reset()

        'Debug.Print("xPath=" + xpath)

        While iterator.MoveNext
            Dim nodeXml As XmlNode = CType(iterator.Current, XmlNode)
            Dim xmlcpnt As XmlNodeListView = New XmlNodeListView(nodeXml)
            xmlcpnt.GenerationLanguage = document.GenerationLanguage
            'Debug.Print("(" + node.ToString + ")" + xmlcpnt.NodeName + "=" + xmlcpnt.FullpathClassName)
            myList.Add(xmlcpnt)
        End While
    End Sub
End Class
