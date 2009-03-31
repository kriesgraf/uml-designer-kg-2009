Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class XmlNodeListView
    Inherits XmlComponent
    Implements IComparer

    Public Const cstNullElement As String = "0"

    Public Enum EComboList
        Type_index = -1
        Type_simple = 0
        Container_single = 1
        Container_indexed = 2
    End Enum

    Private m_strName As String

    Public ReadOnly Property Id() As String
        Get
            If m_strName = "" Then
                Return GetAttribute("id")
            End If
            Return cstNullElement
        End Get
    End Property

    Public ReadOnly Property MasterNode() As XmlNode
        Get
            Return GetMasterNode(Me.Node)
        End Get
    End Property

    Public ReadOnly Property FullUmlPathName() As String
        Get
            Return Me.Name + " (" + GetFullUmlPath(Me.Node.ParentNode) + ")"
        End Get
    End Property

    Public ReadOnly Property FullpathClassName() As String
        Get
            If m_strName = "" Then
                Return GetFullpathDescription(Me.Node, CType(Me.Tag, ELanguage))
            End If
            Return m_strName
        End Get
    End Property

    Public Sub New(ByVal _strName As String)
        m_strName = _strName
    End Sub

    Public Sub New(ByVal nodeXml As XmlNode)
        MyBase.New(nodeXml)
    End Sub

    Public Shared Sub InitContainerCombo(ByVal document As XmlComponent, ByVal dataControl As ComboBox, _
                                         Optional ByVal bIndexed As Boolean = False, _
                                         Optional ByVal bClear As Boolean = False)
        If bIndexed Then
            InitTypedefCombo(document, dataControl, 2, bClear)
        Else
            InitTypedefCombo(document, dataControl, 1, bClear)
        End If
    End Sub

    Public Shared Sub InitTypedefCombo(ByVal document As XmlComponent, ByVal dataControl As ComboBox, _
                                 Optional ByVal iContainer As Integer = 0, _
                                 Optional ByVal bClear As Boolean = False)
        Try
            Dim myList As New ArrayList
            Dim bIsNotTypedef = (document.GetNode("parent::typedef") Is Nothing)

            If document.Tag = ELanguage.Language_CplusPlus Then
                bIsNotTypedef = True
            End If

            If bClear Then
                dataControl.DataSource = Nothing
                dataControl.Items().Clear()
            End If

            Select Case (CType(iContainer, EComboList))

                Case EComboList.Type_index
                    ' Specific index type
                    AddNodeList(document, myList, "//class[@implementation!='container']")
                    If bIsNotTypedef Then AddNodeList(document, myList, "//typedef[type[@desc and not(list)] or type[@idref and not(id(@idref)/type/list)] or type/enumvalue]")
                    AddNodeList(document, myList, "//interface")
                    AddNodeList(document, myList, "//reference[@container='0' or not(@container)]")

                    AddSimpleTypesList(myList, CType(document.Tag, ELanguage))

                Case EComboList.Container_single
                    ' Specific container type
                    AddNodeList(document, myList, "//class[@implementation='container' and model[last()=1]]")
                    AddNodeList(document, myList, "//reference[@container='3' or @container='1']")

                Case EComboList.Container_indexed
                    ' Specific container type
                    AddNodeList(document, myList, "//class[@implementation='container' and model[last()=2]]")
                    AddNodeList(document, myList, "//reference[@container='2']")

                Case Else
                    ' Simple value type
                    AddNodeList(document, myList, "//class[@implementation!='container']")
                    If bIsNotTypedef Then AddNodeList(document, myList, "//typedef")
                    AddNodeList(document, myList, "//interface")
                    AddNodeList(document, myList, "//reference[@container='0' or not(@container)]")

                    AddSimpleTypesList(myList, CType(document.Tag, ELanguage))
            End Select

            SortNodeList(myList)

            With dataControl
                .DropDownStyle = ComboBoxStyle.DropDown
                .DisplayMember = "FullpathClassName"
                .ValueMember = "Id"
                .DataSource = myList
            End With
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub InitValueCombo(ByVal document As XmlComponent, ByVal control As ComboBox)
        Try
            Dim myList As New ArrayList

            AddNodeList(document, myList, "//enumvalue")

            With control
                .DropDownStyle = ComboBoxStyle.DropDown
                .DisplayMember = "FullpathClassName"
                .ValueMember = "Id"
                .DataSource = myList
            End With
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub AddNodeList(ByVal document As XmlComponent, ByRef myList As ArrayList, ByVal xpath As String)
        Dim iterator As IEnumerator = document.SelectNodes(xpath).GetEnumerator
        iterator.Reset()

        'Debug.Print("xPath=" + xpath)

        While iterator.MoveNext
            Dim node As XmlNode = CType(iterator.Current, XmlNode)
            Dim xmlcpnt As XmlNodeListView = New XmlNodeListView(node)
            xmlcpnt.Tag = document.Tag
            'Debug.Print("(" + node.ToString + ")" + xmlcpnt.NodeName + "=" + xmlcpnt.FullpathClassName)
            myList.Add(xmlcpnt)
        End While
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim str1 As String = CType(x, XmlNodeListView).FullpathClassName
        Dim str2 As String = CType(y, XmlNodeListView).FullpathClassName
        Return Comparer.DefaultInvariant.Compare(str1, str2)
    End Function

    Private Shared Sub SortNodeList(ByRef myList As ArrayList)
        myList.Sort(New XmlNodeListView("_comparer"))
    End Sub

    Private Shared Sub AddSimpleTypesList(ByRef myList As ArrayList, ByVal eTag As ELanguage)
        Try
            Dim doc As New XmlDocument
            LoadDocument(doc, GetSimpleTypesFilename(eTag))

            Dim iterator As IEnumerator = doc.SelectNodes("//type").GetEnumerator()
            iterator.Reset()

            While iterator.MoveNext()
                myList.Add(New XmlNodeListView(GetCurrentName(iterator)))
            End While
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function GetFullUmlPath(ByVal current As XmlNode) As String
        Try
            Select Case current.Name
                Case "package", "class", "import"
                    Return GetFullUmlPath(current.ParentNode) + "/" + GetName(current)

                Case "export"
                    Return GetFullUmlPath(current.ParentNode.ParentNode) + "/" + GetName(current)

                Case Else
                    Return "/" + GetName(current)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
