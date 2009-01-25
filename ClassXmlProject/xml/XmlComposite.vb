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
        Try
            ChangeReferences(bLoadChildren)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Public functions"

    Public Function DropAppendComponent(ByVal child As XmlComponent) As Boolean
        Try
            XmlProjectTools.RemoveNode(child.Node)
            AppendComponent(child)
            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function MoveUpComponent(ByVal child As XmlComponent) As Boolean
        Try
            Me.Node.RemoveChild(child.Node)
            Dim parent As XmlComposite = CreateDocument(Me.Node.ParentNode)
            parent.AppendComponent(child)
            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Overridable Function DuplicateComponent(ByVal component As XmlComponent) As XmlComponent
        Return CreateDocument(component.Node.CloneNode(True))
    End Function

    Public Overridable Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Try
            Return removeNode.RemoveMe()

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Overridable Function CanPasteItem(ByVal child As XmlComponent) As Boolean
        Return False
    End Function

    Public Overridable Function AddNewComponent(ByVal strDocName As String) As String
        Try
            If strDocName Is Nothing _
            Then
                strDocName = Me.Node.LastChild.Name

            ElseIf strDocName = "" _
            Then
                strDocName = Me.Node.LastChild.Name
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return strDocName
    End Function

    Public Overridable Sub LoadChildrenList(Optional ByVal strViewName As String = "")
        Try
            AddChildren(SelectNodes(), strViewName)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Protected functions"

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        MyBase.ChangeReferences(bLoadChildren)
        Try
            'm_xmlChildrenList.Clear()
            If bLoadChildren Then LoadChildrenList()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub AddChildren(ByVal list As XmlNodeList, ByVal strViewName As String)
        Try
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
                    xcpnt.Tag = Me.Tag
                    'Debug.Print("(AddChildren,'" + strViewName + "')" + xcpnt.ToString + ":=" + Str(xcpnt.Tag))
                    m_xmlChildrenList.Add(xcpnt)
                End While
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Function CreateView(ByVal strViewName As String, Optional ByVal nodeXml As XmlNode = Nothing, Optional ByVal docXml As XmlDocument = Nothing) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            If nodeXml IsNot Nothing Then
                xmlResult = XmlNodeManager.GetInstance().CreateView(nodeXml, strViewName, docXml)
            End If
        Catch ex As Exception
            Throw (ex)
        End Try
        Return xmlResult
    End Function
#End Region
End Class