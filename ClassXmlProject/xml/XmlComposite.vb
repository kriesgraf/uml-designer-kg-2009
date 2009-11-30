Imports System
Imports System.Xml
Imports System.ComponentModel
Imports System.Collections

Public Class XmlComposite
    Inherits XmlComponent

#Region "Class declarations"

    Private m_xmlChildrenList As ArrayList
#End Region

#Region "Properties"

    <CategoryAttribute("XmlComponent"), _
    Browsable(False), _
    DescriptionAttribute("Children list")> _
    Public ReadOnly Property ChildrenList() As ArrayList
        Get
            Return m_xmlChildrenList
        End Get
    End Property

#End Region

#Region "Constructor/Destructor"

    Public Sub New(Optional ByVal xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode)

        ChangeReferences(bLoadChildren)
    End Sub
#End Region

#Region "Public functions"

    Public Overridable Function AddNewComponent(ByVal strNodeName As String) As XmlComponent
        Dim xmlResult As XmlComponent = CreateDocument(strNodeName)
        Me.AppendComponent(xmlResult)
        Return xmlResult
    End Function

    Public Overridable Function DropAppendComponent(ByRef child As XmlComponent, ByRef bImportData As Boolean) As Boolean

        If child.Document IsNot Me.Document Then
            ' We must call "ImportDocument" when dropped node comes from a different project
            child = Me.ImportDocument(child)
            bImportData = True
        Else
            ' Notionally, child will be removed from first location by command Append
            ' But is some case, "AppendComponent" moved only children and not current, 
            ' also, we must remove current node manually
            XmlProjectTools.RemoveNode(child.Node)
            bImportData = False
        End If

        Return (AppendComponent(child) IsNot Nothing)
    End Function

    Public Overridable Function DuplicateComponent(ByVal component As XmlComponent) As XmlComponent
        Return CreateDocument(component.Node.CloneNode(True))
    End Function

    Public Overridable Function CanRemove(ByVal removeNode As XmlComponent) As Boolean
        Return True
    End Function

    Public Overridable Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean

        Return removeNode.RemoveMe()

    End Function

    Public Overridable Function CanPasteItem(ByVal child As XmlComponent) As Boolean
        Return False
    End Function

    Public Overridable Function GetNameNewComponent(ByVal strDocName As String) As String

        If strDocName Is Nothing _
            Then
            strDocName = Me.Node.LastChild.Name

        ElseIf strDocName = "" _
        Then
            strDocName = Me.Node.LastChild.Name
        End If

        Return strDocName
    End Function

    Public Overridable Sub LoadChildrenList(Optional ByVal strViewName As String = "")
        AddChildren(SelectNodes(), strViewName)

    End Sub

#End Region

#Region "Protected functions"

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        MyBase.ChangeReferences(bLoadChildren)

        'm_xmlChildrenList.Clear()
        If bLoadChildren Then LoadChildrenList()

    End Sub

    Protected Sub AddChildren(ByVal list As XmlNodeList, ByVal strViewName As String)

        'Debug.Print("(AddChildren)" + Me.ToString + ":=" + Str(Me.Tag))
        m_xmlChildrenList = New ArrayList
        If list Is Nothing Then
            ' do nothing
        ElseIf list.Count > 0 Then
            Dim iterator As IEnumerator = list.GetEnumerator()
            Dim xcpnt As XmlComponent
            iterator.Reset()
            While iterator.MoveNext()
                If strViewName <> "" Then
                    xcpnt = CreateView(strViewName, TryCast(iterator.Current, XmlNode))
                Else
                    xcpnt = CreateDocument(TryCast(iterator.Current, XmlNode))
                End If
                xcpnt.GenerationLanguage = Me.GenerationLanguage
                'Debug.Print("(AddChildren,'" + strViewName + "')" + xcpnt.ToString + ":=" + Str(xcpnt.GenerationLanguage))
                m_xmlChildrenList.Add(xcpnt)
            End While
        End If
    End Sub

    Protected Function CreateView(ByVal strViewName As String, Optional ByVal nodeXml As XmlNode = Nothing, Optional ByVal docXml As XmlDocument = Nothing) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing

        If nodeXml IsNot Nothing Then
            xmlResult = XmlNodeManager.GetInstance().CreateView(nodeXml, strViewName, docXml)
        End If

        Return xmlResult
    End Function
#End Region
End Class