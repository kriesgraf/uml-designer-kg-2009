Imports System
Imports System.Xml
Imports System.Collections
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class XmlNodeListView
    Inherits XmlComponent

    Private m_bInfo As Boolean = False
    Private m_bLocked As Boolean = False
    Private m_bChecked As Boolean = False

    Public Const cstNullElement As String = "0"

    Public Enum EComboList
        Type_index = -1
        Type_simple = 0
        Container_single = 1
        Container_indexed = 2
    End Enum

    Private m_strName As String

    Public Property Info() As Boolean
        Get
            Return m_bInfo
        End Get
        Set(ByVal value As Boolean)
            m_bInfo = value
        End Set
    End Property

    Public ReadOnly Property Implementation() As EImplementation
        Get
            Select Case Me.NodeName
                Case "property"
                    ' Not yet used? to be defined

                Case "method", "class"
                    Return ConvertDtdToEnumImpl(GetAttribute("implementation"))

                Case "reference"
                    Return EImplementation.Simple

                Case "interface"
                    If CheckAttribute("root", "yes", "no") Then
                        Return EImplementation.Root
                    Else
                        Return EImplementation.Interf
                    End If
            End Select
            Return EImplementation.Unknown
        End Get
    End Property

    Public ReadOnly Property Id() As String
        Get
            If m_strName = "" Then
                Dim tempo As String = GetAttribute("id")
                If tempo IsNot Nothing Then
                    Return tempo
                End If
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
                Dim eLang As ELanguage = CType(Me.Tag, ELanguage)
                Dim strResult As String = GetFullpathDescription(Me.Node, eLang)
                If DEBUG_COMMANDS_ACTIVE Then strResult += " (" + eLang.ToString + ")"
                Return strResult
            End If
            Return m_strName
        End Get
    End Property

    Public Property CheckLocked() As Boolean
        Get
            Return m_bLocked
        End Get
        Set(ByVal value As Boolean)
            m_bLocked = value
        End Set
    End Property

    Public Property CheckedView() As Boolean
        Get
            Return m_bChecked
        End Get
        Set(ByVal value As Boolean)
            If m_bLocked = False Then
                m_bChecked = value
            End If
        End Set
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
                    AddNodeList(document, myList, "ancestor::class/model")
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

    Public Shared Function AddComponentList(ByVal component As XmlComponent, ByRef myList As ArrayList) As XmlNodeListView
        Dim xmlcpnt As XmlNodeListView = New XmlNodeListView(component.Node)
        xmlcpnt.Tag = component.Tag
        'Debug.Print("(" + component.Node.ToString + ")" + xmlcpnt.NodeName + "=" + xmlcpnt.FullpathClassName)
        myList.Add(xmlcpnt)
        Return xmlcpnt
    End Function

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

    Public Shared Function InitInheritedListBox(ByVal component As XmlComponent, _
                                                 ByVal lsbControl As ListBox, _
                                                 ByVal eImplementation As EImplementation, _
                                                 ByVal bRestrictedVisibility As Boolean) As Boolean
        Dim strClassQuery As String = ""
        Dim strReferenceQuery As String = ""
        Dim strInterfaceQuery As String = ""

        ' Implementation can change in dlgClass, also we can't get property 'component.Implementation' that contains initial value !
        Select Case eImplementation
            Case eImplementation.Simple, _
                 eImplementation.Exception, _
                 eImplementation.Container
                strClassQuery = "@implementation='simple' or @implementation='final' or @implementation='exception' or @implementation='abstract'"
                strReferenceQuery = "reference[@type!='typedef']"
                strInterfaceQuery = "interface[property or method][@root='no']"

            Case eImplementation.Node, _
                 eImplementation.Leaf
                strClassQuery = "@implementation='virtual' or @implementation='root' or @implementation='abstract'"
                strInterfaceQuery = "interface[property or method]"

            Case eImplementation.Root, _
                 eImplementation.Interf
                strClassQuery = "@implementation='abstract'"
                strInterfaceQuery = "interface[property or method][@root='no']"

            Case Else
                Throw New Exception("'CurrentImplementation' property not yet assigned")
        End Select

        If strClassQuery = "" Then
            lsbControl.Items.Clear()
            lsbControl.Items.Add("Sorry no inheritance available")
            lsbControl.Enabled = False
            Exit Function
        End If

        Dim xmlList As XmlNodeList
        Dim strIgnoredClasses As String
        Dim bInherit As Boolean = (component.GetNode("id(inherited/@idref)[@implementation!='abstract']") IsNot Nothing)
        If component.GetNode("id(inherited/@idref)[self::interface][@root='yes']") IsNot Nothing Then
            bInherit = True
        End If
        If component.GetNode("id(inherited/@idref)[self::reference]") IsNot Nothing Then
            bInherit = True
        End If

        If bInherit Then
            strClassQuery = "@implementation='abstract'"
            strReferenceQuery = ""
            strInterfaceQuery = "interface[@root='no']"
        End If
        xmlList = component.SelectNodes("inherited")

        ' We ignore current class ID !
        strIgnoredClasses = component.GetAttribute("id") + ";"

        Dim iterator As IEnumerator = xmlList.GetEnumerator()

        ' We ignore class ID that are already in heritance
        While iterator.MoveNext
            strIgnoredClasses = strIgnoredClasses + GetCurrentIDREF(iterator) + ";"
        End While

        strIgnoredClasses = "not(contains('" + strIgnoredClasses + "',concat(@id,';')))"

        ' Query removes ignored ID
        strClassQuery = "class[" + strIgnoredClasses + "][" + strClassQuery + "]"

        Dim collList As New ArrayList

        Dim strTempo As String

        ' We ignore classes that are not in current package, if it's required
        If bRestrictedVisibility = False Then
            strTempo = "//" + strClassQuery + "[@visibility='package']"
            AddNodeList(component, collList, strTempo)
        Else
            strTempo = "parent::*/" + strClassQuery
            AddNodeList(component, collList, strTempo)
        End If

        If strReferenceQuery.Length > 0 Then
            strTempo = "//" + strReferenceQuery + "[" + strIgnoredClasses + "]"
            AddNodeList(component, collList, strTempo)
        End If

        If strInterfaceQuery.Length > 0 Then
            strTempo = "//" + strInterfaceQuery + "[" + strIgnoredClasses + "]"
            AddNodeList(component, collList, strTempo)
        End If

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
    End Function

    Public Overrides Function CompareComponent(ByVal x As Object, ByVal y As Object) As Integer
        Dim str1 As String = CType(x, XmlNodeListView).FullpathClassName
        Dim str2 As String = CType(y, XmlNodeListView).FullpathClassName
        Return Comparer.DefaultInvariant.Compare(str1, str2)
    End Function

    Public Shared Sub SortNodeList(ByVal myList As ArrayList)
        myList.Sort(New XmlNodeListView("_comparer"))
    End Sub

    Public Shared Function ShowNodeList(ByVal myList As ArrayList, ByVal title As String, ByVal lockedMessage As String) As Boolean
        Dim form As New dlgNodeListView
        With form
            .Text = title
            .LockedMessage = lockedMessage
            .NodeList = myList
            .DisplayMember = "FullpathClassName"
            If .ShowDialog = DialogResult.OK Then
                Return True
            End If
        End With
        Return False
    End Function

    Public Shared Function GetListReferences(ByVal component As XmlComponent, ByRef listResult As ArrayList) As Boolean
        Dim bResult As Boolean = False
        Try
            Dim strName As String = component.Name
            Dim strID As String = GetID(component.Node)
            Dim strQuery As String = "//*[@name='" + strName + "' and @id!='" + strID + "']"

            If listResult Is Nothing Then
                Dim listNode As XmlNodeList = component.SelectNodes(strQuery)
                Return (listNode.Count > 0)
            End If

            AddNodeList(component, listResult, strQuery)
            Return (listResult.Count > 0)

        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function

    Private Shared Sub AddSimpleTypesList(ByVal myList As ArrayList, ByVal eTag As ELanguage)
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
