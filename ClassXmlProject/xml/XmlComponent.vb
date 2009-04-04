Imports ClassXmlProject.XmlProjectTools
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Xml
Imports System
Imports System.Collections
Imports Microsoft.VisualBasic

#Region "Interfaces"

Public Interface InterfObject
    Property InterfObject() As Object
    Sub Update()
End Interface

Public Interface InterfViewControl

    Sub InitControl(ByVal control As Control)
    Function Compare(ByVal nodeName As String) As Integer

End Interface

Public Interface InterfViewForm

    Function CreateForm(ByVal document As XmlComponent) As Form

End Interface

Public Interface InterfFormDocument

    WriteOnly Property Document() As XmlComponent
    Sub DisableMemberAttributes()

End Interface

Public Interface InterfNodeCounter

    WriteOnly Property NodeCounter() As XmlReferenceNodeCounter

End Interface

#End Region

Public Class XmlComponent
    Inherits Object
    Implements IComparer

#Region "Predefined types"

    Public Class XmlClipboard

        Private m_xmlComponent As XmlComponent = Nothing
        Private m_bCopy As Boolean = True

        Public ReadOnly Property CanPaste() As Boolean
            Get
                Return (m_xmlComponent IsNot Nothing)
            End Get
        End Property

        Public Sub SetData(ByVal component As Object, Optional ByVal bCopy As Boolean = True)
            m_bCopy = bCopy
            m_xmlComponent = TryCast(component, XmlComponent)
        End Sub

        Public Function GetData(ByRef bCopy As Boolean) As XmlComponent
            bCopy = m_bCopy
            m_bCopy = True
            Dim xmlResult As XmlComponent = m_xmlComponent
            m_xmlComponent = Nothing
            Return xmlResult
        End Function
    End Class

#End Region

#Region "Class declarations"

    Protected Friend m_bCreateNodeNow As Boolean
    Protected m_xmlNodeManager As XmlNodeManager

    Private Shared m_xmlClipBoardComponent As New XmlClipboard

    Private m_iTag As Integer
    Private m_xmlNode As XmlNode
    Private m_xmlDocument As XmlDocument
    Private m_bUpdated As Boolean = False
#End Region

#Region "Properties (Adapter pattern)"

    <CategoryAttribute("General"), _
    DescriptionAttribute("Name")> _
    Public Overridable Property Name() As String
        Get
            Dim strResult As String = GetAttribute("name")
            If strResult = "" Then
                If m_xmlNode IsNot Nothing Then
                    strResult = "#" + m_xmlNode.Name
                Else
                    strResult = "$Name=" + TypeName(Me)
                End If
            End If
            Return strResult
        End Get
        Set(ByVal value As String)
            SetAttribute("name", value)
        End Set
    End Property

    <CategoryAttribute("General"), _
    DescriptionAttribute("Integer tag")> _
    Public Overridable Property Tag() As Integer
        Get
            Return m_iTag
        End Get
        Set(ByVal value As Integer)
            m_iTag = value
        End Set
    End Property

    <CategoryAttribute("XmlNode"), _
    DescriptionAttribute("XMLDOM node")> _
    Public Property Node() As XmlNode
        Get
            Return m_xmlNode
        End Get
        Set(ByVal value As XmlNode)
            m_xmlNode = value
            If value IsNot Nothing Then
                If value.OwnerDocument IsNot Nothing Then
                    m_xmlDocument = value.OwnerDocument
                End If
            End If
            ChangeReferences(False)
        End Set
    End Property

    <CategoryAttribute("XmlNode"), _
    DescriptionAttribute("XML element name")> _
    Public ReadOnly Property NodeName() As String
        Get
            If m_xmlNode IsNot Nothing Then
                Return m_xmlNode.Name
            End If
            Return "Nothing"
        End Get
    End Property

    <CategoryAttribute("XmlNode"), _
    DescriptionAttribute("XML markup representing this node and all its child nodes")> _
    Public ReadOnly Property OuterXml() As String
        Get
            If m_xmlNode IsNot Nothing Then
                Return m_xmlNode.OuterXml
            End If
            Return "<Nothing/>"
        End Get
    End Property

    <CategoryAttribute("XmlNode"), _
    DescriptionAttribute("XML markup representing only the child nodes of this node")> _
    Public ReadOnly Property InnerXml() As String
        Get
            If m_xmlNode IsNot Nothing Then
                Return m_xmlNode.InnerXml
            End If
            Return "<Nothing/>"
        End Get
    End Property

    <CategoryAttribute("XmlNode"), _
    DescriptionAttribute("XMLDOM owner document")> _
    Public Property Document() As XmlDocument
        Get
            Return m_xmlDocument
        End Get
        Set(ByVal value As XmlDocument)
            If m_xmlDocument IsNot value And value IsNot Nothing Then
                m_xmlDocument = value
            End If
        End Set
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("A three bytes hashcode")> _
    Public ReadOnly Property Key() As String
        Get
            Return Hex(GetHashCode())
        End Get
    End Property

#End Region

#Region "Public methods"

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing)
        m_iTag = 0
        m_bCreateNodeNow = False
        m_xmlNode = xmlNode
        If m_xmlNode IsNot Nothing Then
            If m_xmlNode.OwnerDocument IsNot Nothing Then
                m_xmlDocument = m_xmlNode.OwnerDocument
            End If
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return MyBase.ToString + " {#" + Me.NodeName + ", '" + Me.Name + "'}"
    End Function

    Public Overridable Function CompareComponent(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim str1 As String = CType(x, XmlComponent).Name
        Dim str2 As String = CType(y, XmlComponent).Name
        Return Comparer.DefaultInvariant.Compare(str1, str2)
    End Function
#End Region

#Region "XML queries"

    Protected Friend Overloads Function TestNode(ByVal xpath As String) As Boolean
        Dim bResult As Boolean = False
        Try
            If m_xmlNode IsNot Nothing Then
                bResult = (m_xmlNode.SelectSingleNode(xpath) IsNot Nothing)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Overloads Function GetNode(ByVal nodeXml As XmlNode, ByVal xpath As String) As XmlNode
        Dim xmlResult As XmlNode = Nothing
        Try
            If xpath = "" Then
                Throw New Exception("XPath query can't be empty")

            ElseIf nodeXml IsNot Nothing Then
                If m_bCreateNodeNow Then
                    xmlResult = nodeXml.SelectSingleNode(xpath)
                    If xmlResult Is Nothing Then xmlResult = CreateAppendNode(xpath)
                Else
                    xmlResult = nodeXml.SelectSingleNode(xpath)
                End If
            Else
                '                Throw New Exception("Node can't be null")
            End If
        Catch ex As Exception
            Throw New Exception("Can't get single node, see inner exception...", ex)
        End Try
        Return xmlResult
    End Function

    Protected Friend Overloads Function GetNode(ByVal xpath As String) As XmlNode
        Dim xmlResult As XmlNode = Nothing
        If m_bCreateNodeNow Then
            xmlResult = GetNode(m_xmlNode, xpath)
            If xmlResult Is Nothing Then xmlResult = CreateAppendNode(xpath)
        Else
            xmlResult = GetNode(m_xmlNode, xpath)
        End If
        Return xmlResult
    End Function

    Protected Friend Overloads Function SelectNodes(ByVal nodeXml As XmlNode, Optional ByVal xpath As String = "*") As XmlNodeList
        Dim xmlResult As XmlNodeList = Nothing
        Try
            If nodeXml IsNot Nothing Then
                xmlResult = nodeXml.SelectNodes(xpath)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Protected Friend Overloads Function SelectNodes(Optional ByVal xpath As String = "*") As XmlNodeList
        Dim xmlResult As XmlNodeList = Nothing
        Try
            If m_xmlNode IsNot Nothing Then
                xmlResult = m_xmlNode.SelectNodes(xpath)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Protected Friend Function GetNodeString(ByVal xpath As String) As String
        Dim strResult As String = ""
        Dim nodeXml As XmlNode
        Try
            nodeXml = GetNode(xpath)
            If nodeXml IsNot Nothing Then
                strResult = nodeXml.InnerText
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Protected Friend Function GetNodeRef(Optional ByVal name As String = "idref", Optional ByVal xpath As String = "") As XmlNode
        If m_xmlNode IsNot Nothing Then
            Dim strId As String = GetAttribute(name, xpath)
            If strId = "" Then
                Return Nothing
            End If
            Return GetElementById(strId)
        End If
        Return Nothing
    End Function

    Protected Friend Function GetElementById(ByVal strId As String) As XmlNode
        If Me.Document Is Nothing Then
            Throw New Exception("Property m_xmlDocument is null in component " + Me.ToString())
        End If
        If strId Is Nothing Then
            Throw New Exception("Argument strId is null in call of method " + Me.ToString + ".GetElementById")
        End If
        Return Me.Document.GetElementById(strId)
    End Function

    Protected Friend Function ValidateIdReference(ByVal IdRef As String, Optional ByVal bDoNotRaiseException As Boolean = False) As Boolean
        Dim bResult As Boolean = True
        If IdRef = "" _
        Then
            If bDoNotRaiseException = False Then Throw New Exception("Argument value is empty in call of method " + Me.ToString + "get_Idref")
            bResult = False

        ElseIf Me.Document IsNot Nothing _
        Then
            If GetElementById(IdRef) Is Nothing Then
                If bDoNotRaiseException = False Then Throw New Exception("Id '" + IdRef + "' is unknown in project")
                bResult = False
            End If
        Else
            If bDoNotRaiseException = False Then Throw New Exception("Property MyBase.Document is null in component " = Me.ToString)
            bResult = False
        End If
        Return bResult
    End Function

    Protected Friend Sub SetBooleanAttribute(ByVal name As String, ByVal value_bool As Boolean, _
                                             Optional ByVal value_true As String = "yes", Optional ByVal value_false As String = "no")
        Try
            If value_bool Then
                SetAttribute(name, value_true)
            Else
                SetAttribute(name, value_false)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Function CheckAttribute(ByVal name As String, ByVal to_check As String, _
                                             ByVal default_value As String, Optional ByVal xpath As String = "") As Boolean

        Dim strResult As String = Nothing
        Try
            strResult = GetAttribute(name, xpath)

            If strResult = Nothing Then
                Return (to_check = default_value)
            End If
            Return (to_check = strResult)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Protected Friend Function GetAttribute(ByVal name As String, Optional ByVal xpath As String = "") As String
        Dim strResult As String = Nothing
        Dim nodeXml As XmlNode
        Try
            If m_xmlNode IsNot Nothing Then
                If xpath = "" Then
                    nodeXml = m_xmlNode.Attributes.ItemOf(name)
                Else
                    nodeXml = GetNode(xpath)
                    If nodeXml IsNot Nothing Then
                        nodeXml = nodeXml.Attributes.ItemOf(name)
                    End If
                End If
                If nodeXml IsNot Nothing Then
                    strResult = nodeXml.Value
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

    Protected Friend Function SetNodeString(ByVal xpath As String, ByVal value As String) As Boolean
        Dim bResult As Boolean = False
        Dim nodeXml As XmlNode = Nothing
        Dim delim As String = Chr(10) + Chr(13) + Chr(9) + Chr(32)

        Try
            nodeXml = m_xmlNode.SelectSingleNode(xpath)

            If nodeXml Is Nothing And m_bCreateNodeNow Then
                nodeXml = CreateAppendNode(xpath, True, value.Trim(delim.ToCharArray()))
            ElseIf nodeXml IsNot Nothing Then
                nodeXml.InnerText = value.Trim(delim.ToCharArray())
            End If

            bResult = True

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function SetAttribute(ByVal name As String, ByVal value As String, Optional ByVal xpath As String = "") As Boolean
        Dim bResult As Boolean = False
        Dim nodeXml As XmlNode = m_xmlNode
        Try
            If xpath <> "" Then
                nodeXml = GetNode(xpath)
                If nodeXml Is Nothing And m_bCreateNodeNow Then
                    nodeXml = CreateAppendNode(xpath)
                End If
            Else
                nodeXml = m_xmlNode
            End If

            If nodeXml Is Nothing Then
                '                Throw New Exception("Current nodeXml is null")
                Return False
            End If

            Dim attrib As XmlAttribute = nodeXml.Attributes.ItemOf(name)

            If attrib Is Nothing And m_bCreateNodeNow Then
                attrib = nodeXml.Attributes.Append(CreateAttribute(name))
            End If

            If attrib IsNot Nothing Then
                attrib.Value = value
                bResult = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Function CreateElement(ByVal name As String) As XmlNode
        Return Me.Document.CreateNode(XmlNodeType.Element, name, "")
    End Function

    Protected Friend Overridable Function CreateNode(ByVal name As String) As XmlNode
        If Me.Document Is Nothing Then
            Throw New Exception("Property m_xmlDocument is null in component " + Me.ToString())
        End If
        Return CreateElement(name)
    End Function

    Protected Friend Overridable Function CreateTextNode(ByVal Text As String) As XmlText
        If Me.Document Is Nothing Then
            Throw New Exception("Property m_xmlDocument is null in component " + Me.ToString())
        End If
        Return Me.Document.CreateTextNode(Text)
    End Function

    Protected Friend Overridable Function CreateAttribute(ByVal name As String) As XmlAttribute
        If Me.Document Is Nothing Then
            Throw New Exception("Property m_xmlDocument is null in component " + Me.ToString())
        End If
        Return Me.Document.CreateAttribute(name)
    End Function

    Protected Friend Function CreateAppendNode(ByVal xpath As String, Optional ByVal bAddTextNode As Boolean = False, Optional ByVal value As String = "undefined") As XmlNode
        Dim xmlResult As XmlNode = Nothing
        Try
            If xpath.Contains("::") Then

                Dim StringArray() As String = xpath.Split("::")
                Select Case StringArray(0)
                    Case "following-sibling"
                        xmlResult = m_xmlNode.ParentNode.InsertAfter(CreateNode(StringArray(2)), m_xmlNode)
                    Case "preceding-sibling"
                        xmlResult = m_xmlNode.ParentNode.InsertBefore(CreateNode(StringArray(2)), m_xmlNode)
                    Case Else
                        Throw New Exception("axe-location " + StringArray(0) + " not yet supported")
                End Select

            ElseIf xpath.Contains("/") Then

                Dim StringArray() As String = xpath.Split("/")
                xmlResult = GetNode(StringArray(0))
                If xmlResult Is Nothing Then
                    xmlResult = CreateElement(StringArray(0))
                    AppendNode(xmlResult)
                End If
                xmlResult = xmlResult.AppendChild(CreateElement(StringArray(1)))

            Else
                ' We use method "CreateElement" because this is not Overridable
                xmlResult = CreateElement(xpath)
                AppendNode(xmlResult)
            End If
            If xmlResult IsNot Nothing And bAddTextNode Then
                xmlResult.AppendChild(CreateTextNode(value))
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Protected Friend Overridable Function RemoveMe() As Boolean
        Dim bResult As Boolean = False
        Dim parentNode As XmlNode
        Try
            If m_xmlNode IsNot Nothing Then
                parentNode = m_xmlNode.ParentNode
                If parentNode IsNot Nothing Then
                    NotifyRemove(m_xmlNode)
                    parentNode.RemoveChild(m_xmlNode)
                    m_xmlNode = Nothing
                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return bResult
    End Function

    Protected Friend Function RemoveSingleNode(ByVal xpath As String) As Boolean
        Dim bResult As Boolean = False
        Dim xmlRemovedNode As XmlNode
        Try
            xmlRemovedNode = m_xmlNode.SelectSingleNode(xpath)
            If xmlRemovedNode IsNot Nothing Then
                NotifyRemove(xmlRemovedNode)
                m_xmlNode.RemoveChild(xmlRemovedNode)
            End If
            bResult = True

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function RemoveAllNodes(Optional ByVal xpath As String = "*") As Boolean
        Dim bResult As Boolean = False

        Try
            Dim list As XmlNodeList

            list = m_xmlNode.SelectNodes(xpath)

            Dim iterator As IEnumerator = list.GetEnumerator()
            iterator.Reset()

            While iterator.MoveNext()
                NotifyRemove(iterator.Current)
                m_xmlNode.RemoveChild(iterator.Current)
            End While
            bResult = True

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Function ReplaceAttribute(ByVal Source As String, ByVal Dest As String, Optional ByVal xpath As String = "") As Boolean
        RemoveAttribute(Source, xpath)
        AddAttribute(Dest, "", xpath)
    End Function

    Protected Friend Function RemoveAttribute(ByVal name As String, Optional ByVal xpath As String = "") As Boolean
        Dim bResult As Boolean = False
        Try
            If xpath = "" Then
                If m_xmlNode.Attributes.ItemOf(name) IsNot Nothing Then
                    m_xmlNode.Attributes.RemoveNamedItem(name)
                End If
            Else
                If TestNode(xpath) Then
                    If GetNode(xpath).Attributes.ItemOf(name) IsNot Nothing Then
                        GetNode(xpath).Attributes.RemoveNamedItem(name)
                    End If
                End If
            End If
            bResult = True

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Overridable Function FindNode(ByVal child As XmlComponent) As XmlNode
        Return GetNode("*[@name='" + child.Name + "']")
    End Function

    Protected Friend Overridable Function AppendNode(ByVal nodeXml As XmlNode) As XmlNode
        ' TODO : surcharger dans les classes filles pour lesquel l'append ne sait fait pas aussi simplement qu'ici
        Return m_xmlNode.AppendChild(nodeXml)
    End Function

    Protected Friend Function AddAttribute(ByVal name As String, Optional ByVal value As String = "", Optional ByVal xpath As String = "") As XmlAttribute
        Dim attrib As XmlAttribute = Nothing
        Dim nodeXml As XmlNode = Nothing
        Try

            If xpath <> "" Then
                nodeXml = GetNode(xpath)
            Else
                nodeXml = m_xmlNode
            End If

            If nodeXml Is Nothing Then
                '                Throw New Exception("Current nodeXml is null")
                Return Nothing
            End If

            attrib = nodeXml.Attributes.ItemOf(name)

            If attrib Is Nothing Then
                attrib = CreateAttribute(name)
                nodeXml.Attributes.SetNamedItem(attrib)
            End If

            attrib.Value = value

        Catch ex As Exception
            Throw ex
        End Try
        Return attrib
    End Function

    Public Overridable Function CanAddComponent(ByVal nodeXml As XmlComponent) As Boolean
        Return True
    End Function

    Public Overridable Function AppendComponent(ByVal nodeXml As XmlComponent) As XmlNode
        ' This method is called when order is not defined, also AppendNode must do it
        If CanAddComponent(nodeXml) Then
            AppendNode(nodeXml.Node)
            nodeXml.NotifyInsert()
            Return nodeXml.Node
        End If
        Return Nothing
    End Function

    Public Overridable Function DropInsertComponent(ByVal component As XmlComponent) As XmlNode
        If Me.Node IsNot component.Node Then
            Me.Node.ParentNode.RemoveChild(component.Node)
            Me.Node.ParentNode.InsertBefore(component.Node, Me.Node)
            Return component.Node
        End If
        Return Nothing
    End Function

    Public Overridable Function InsertComponent(ByVal nodeXml As XmlComponent, ByVal before As XmlComponent) As XmlNode
        m_xmlNode.InsertBefore(nodeXml.Node, before.Node)
        nodeXml.NotifyInsert(before)
        Return nodeXml.Node
    End Function

    Public Overridable Sub NotifyInsert(Optional ByVal before As XmlComponent = Nothing)
        ' To be overrided if necessary
    End Sub

    Public Overridable Sub NotifyRemove(ByVal node As XmlNode)
        ' To be overrided if necessary
    End Sub


#End Region

#Region "Prototype pattern"

    Public Shared ReadOnly Property Clipboard() As XmlClipboard
        Get
            Return m_xmlClipBoardComponent
        End Get
    End Property

    Public Property Updated() As Boolean

        Get
            Dim bResult As Boolean = m_bUpdated
#If DEBUG Then
#Else
            m_bUpdated = False
#End If
            Return bResult
        End Get
        Set(ByVal value As Boolean)
            m_bUpdated = value
        End Set
    End Property

    Public Overridable Function Clone(ByVal nodeXml As XmlNode, Optional ByVal bLoadChildren As Boolean = False) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            xmlResult = MemberwiseClone()
            xmlResult.Node = nodeXml
            xmlResult.ChangeReferences(bLoadChildren)
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Protected Overloads Function CreateDocument(ByVal strNodeName As String, Optional ByVal docXml As XmlDocument = Nothing, Optional ByVal bLoadChildren As Boolean = False) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            xmlResult = XmlNodeManager.GetInstance().CreateDocument(strNodeName, docXml, bLoadChildren)
        Catch ex As Exception
            Throw (ex)
        End Try
        Return xmlResult
    End Function

    Protected Overloads Function CreateDocument(Optional ByVal nodeXml As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            If nodeXml IsNot Nothing Then
                xmlResult = XmlNodeManager.GetInstance().CreateDocument(nodeXml, bLoadChildren)
            End If
        Catch ex As Exception
            Throw (ex)
        End Try
        Return xmlResult
    End Function

    Protected Friend Overridable Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        ' To be overrided if necessary
    End Sub

    Protected Friend Overridable Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, Optional ByVal bParam As Boolean = False)
        ' To be overrided if necessary
    End Sub

    Public Overridable Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            ' m_xmlNode must be created at this step by the customize classes 
            ' that know which element to create.
            ' But attribute 'Name' will be create here if not exists
            m_bCreateNodeNow = bCreateNodeNow
            Name = "New_" + m_xmlNode.Name
        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub
#End Region
End Class

