Imports System
Imports ClassXmlProject.XmlProjectTools
Imports System.ComponentModel
Imports System.Xml

Public Class XmlExceptionSpec
    Inherits XmlComponent

    <CategoryAttribute("UML design"), _
    [ReadOnly](True), _
    DescriptionAttribute("Component name")> _
    Public Overrides Property Name() As String
        Get
            Return MyBase.Name
        End Get
        Set(ByVal value As String)
            ' No update
        End Set
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Class id reference")> _
    Public Property Idref() As String
        Get
            If MyBase.NodeName = "exception" Then
                Return GetAttribute("idref")
            End If
            ' Used in dlgException to display node class and reference
            Return GetAttribute("id")
        End Get
        Set(ByVal value As String)
            ' Used in dlgException to append a node exception to node method
            If MyBase.NodeName = "exception" Then
                ValidateIdReference(value, m_bCreateNodeNow)
                SetAttribute("idref", value)
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("C++ class declaration")> _
    Public ReadOnly Property FullpathClassName() As String
        Get
            Dim nodeClass As XmlNode

            If MyBase.NodeName = "exception" Then
                nodeClass = MyBase.GetElementById(Me.Idref)
            Else
                nodeClass = MyBase.Node
            End If

            Dim eLang As ELanguage = CType(Me.Tag, ELanguage)
            Dim strResult As String = GetFullpathDescription(nodeClass, eLang)
            If DEBUG_COMMANDS_ACTIVE Then strResult += " (" + eLang.ToString + ")"
            Return strResult
        End Get
    End Property

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode)
        Try
            ChangeReferences(bLoadChildren)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()
            Idref = "class0"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub
End Class