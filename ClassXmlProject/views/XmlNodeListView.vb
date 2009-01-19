Imports System.Xml
Imports ClassXmlProject.XmlProjectTools

Public Class XmlNodeListView
    Inherits XmlComponent
    Implements IComparer

    Public Const cstNullElement As String = "0"

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
                Return GetFullpathDescription(Me.Node, Me.Tag)
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

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim str1 As String = CType(x, XmlNodeListView).FullpathClassName
        Dim str2 As String = CType(y, XmlNodeListView).FullpathClassName
        Return Comparer.DefaultInvariant.Compare(str1, str2)
    End Function
End Class
