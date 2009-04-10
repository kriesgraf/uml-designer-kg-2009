Imports System
Imports System.Xml
Imports System.ComponentModel
Imports System.Collections.Generic
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.UmlCodeGenerator
Imports Microsoft.VisualBasic

Public Interface InterfFormClass
    Property ClassImpl() As EImplementation
End Interface

Public Class XmlClassSpec
    Inherits XmlComposite
    Implements InterfNodeCounter

    Protected m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

#Region "Properties"

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("VB.NET class declaration")> _
    Public Property PartialDecl() As Boolean
        Get
            Dim strResult As String = GetAttribute("behaviour")
            If strResult Is Nothing Then
                Return False
            End If
            Return True
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                AddAttribute("behaviour", "Partial")
            Else
                RemoveAttribute("behaviour")
            End If
        End Set
    End Property

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

    <CategoryAttribute("Code generation"), _
    DescriptionAttribute("Project language")> _
    Public ReadOnly Property GenerationLanguage() As ELanguage
        Get
            Return CType(MyBase.Tag, ELanguage)
        End Get
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("C++ class declaration")> _
    Public ReadOnly Property FullpathClassName() As String
        Get
            Return GetFullpathDescription(Me.Node, CType(Me.Tag, ELanguage))
        End Get
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Implementation")> _
    Public Property Implementation() As EImplementation
        Get
            Return ConvertDtdToEnumImpl(GetAttribute("implementation"))
        End Get
        Set(ByVal value As EImplementation)
            SetAttribute("implementation", ConvertEnumImplToDtd(value))
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Brief comment")> _
    Public Property BriefComment() As String
        Get
            Return GetAttribute("brief", "comment")
        End Get
        Set(ByVal value As String)
            SetAttribute("brief", value, "comment")
        End Set
    End Property

    <TypeConverter(GetType(UmlClassVisibility)), _
    CategoryAttribute("UML design"), _
   DescriptionAttribute("Package visibility")> _
  Public Property Visibility() As String
        Get
            Return GetAttribute("visibility")
        End Get
        Set(ByVal value As String)
            SetAttribute("visibility", value)
        End Set
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Component id")> _
    Public Property Id() As String
        Get
            Return GetAttribute("id")
        End Get
        Set(ByVal value As String)
            SetAttribute("id", value)
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Detail comment")> _
    Public Property Comment() As String
        Get
            Return GetNodeString("comment")
        End Get
        Set(ByVal value As String)
            SetNodeString("comment", value)
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberVisibility)), _
    CategoryAttribute("UML design"), _
    DescriptionAttribute("Constructor visibility")> _
    Public Property Constructor() As String
        Get
            Return GetAttribute("constructor")
        End Get
        Set(ByVal value As String)
            SetAttribute("constructor", value)
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("C++ Constructor/destructor implementation")> _
    Public Property Inline() As String
        Get
            Return GetAttribute("inline")
        End Get
        Set(ByVal value As String)
            SetAttribute("inline", value)
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Param template model count")> _
    Public Property ModelCount() As Integer
        Get
            Return SelectNodes("model").Count - 1
        End Get
        Set(ByVal value As Integer)
            ' todo
            AddModelCount(value + 1)
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberVisibility)), _
    CategoryAttribute("UML design"), _
    DescriptionAttribute("Destructor visibility")> _
    Public Property Destructor() As String
        Get
            Return GetAttribute("destructor")
        End Get
        Set(ByVal value As String)
            SetAttribute("destructor", value)
        End Set
    End Property
#End Region

#Region "Public methods"

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode, bLoadChildren)
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            ChangeReferences()

            Name = "New_class"
            Implementation = EImplementation.Simple
            Visibility = "package"
            Constructor = "public"
            Destructor = "public"
            Inline = "both"
            Id = "class0"
            Comment = "Insert here details"
            BriefComment = "Insert here a brief comment"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Sub LoadChildrenList(Optional ByVal strViewName As String = "")
        Try
            AddChildren(SelectNodes("*[name()!='comment' and name()!='inline']"), strViewName)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overrides Function AddNewComponent(ByVal strDocName As String) As String
        Try
            If strDocName = "" Then
                Dim child As XmlNode = Me.Node.LastChild
                strDocName = child.Name
                If strDocName = "method" Then
                    If GetAttributeValue(child, "constructor") <> "no" Then
                        strDocName = "constructor_doc"
                    Else
                        strDocName = MyBase.AddNewComponent(strDocName)
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return strDocName
    End Function

    Public Overrides Function RemoveComponent(ByVal removeNode As XmlComponent) As Boolean
        Dim bResult As Boolean = False
        Try
            Select Case removeNode.NodeName
                Case "property"
                    If RemoveOverridedProperty(Me, removeNode) = False Then
                        Return False
                    End If

                Case "method"
                    If RemoveOverridedMethod(Me, removeNode) = False Then
                        Return False
                    End If

                Case "inherited"
                    RemoveInheritedProperties(Me, removeNode)
                    RemoveInheritedMethods(Me, removeNode)
            End Select

            bResult = MyBase.RemoveComponent(removeNode)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Public Function OverrideProperties(ByVal eCurrent As EImplementation, Optional ByVal strIgnoredClasses As String = "", _
                                       Optional ByVal bIgnoreNoMembers As Boolean = False) As Boolean
        Try
            Dim fen As New dlgOverrideProperties(Me, eCurrent)

            Select Case fen.OverrideProperties(strIgnoredClasses)
                Case dlgOverrideProperties.EAnswer.SomeError
                    Return False

                Case dlgOverrideProperties.EAnswer.NoMembers
                    If bIgnoreNoMembers Then
                        Return True
                    End If
                    Return False

                Case Else
                    Return True
            End Select
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return False
    End Function

    Public Function OverrideMethods(ByVal eCurrent As EImplementation, Optional ByVal strIgnoredClasses As String = "", _
                                    Optional ByVal bIgnoreNoMembers As Boolean = False) As Boolean
        Try
            Dim fen As New dlgOverrideMethods(Me, eCurrent)

            Select Case fen.OverrideMethods(strIgnoredClasses)
                Case dlgOverrideProperties.EAnswer.SomeError
                    Return False

                Case dlgOverrideProperties.EAnswer.NoMembers
                    If bIgnoreNoMembers Then
                        Return True
                    End If
                    Return False

                Case Else
                    Return True
            End Select
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return False
    End Function

    Public Overrides Function CanPasteItem(ByVal child As XmlComponent) As Boolean
        Select Case child.NodeName
            Case "property", "typedef", "method"
                Return True

            Case Else
                Return MyBase.CanPasteItem(child)
        End Select
        Return MyBase.CanPasteItem(child)
    End Function

    Public Overrides Function CanAddComponent(ByVal nodeXml As XmlComponent) As Boolean
        Select nodeXml.NodeName
            Case "dependency"
                If GetFirstClassId(Me, Me.Id).Length = 0 Then
                    MsgBox("Sorry but only one class declared yet!", MsgBoxStyle.Exclamation)
                    Return False
                End If

            Case "typedef"
                Dim eLang As ELanguage = CType(Me.Tag, ELanguage)
                With CType(nodeXml, XmlTypedefSpec)
                    If eLang = ELanguage.Language_Vbasic _
                    Then
                        .TypeVarDefinition.Kind = XmlTypeVarSpec.EKindDeclaration.EK_Enumeration

                        Dim element As XmlEnumSpec = CreateDocument("enumvalue", Me.Document)
                        .AppendComponent(element)
                    End If
                End With
            Case Else
        End Select
        Return True
    End Function

#End Region

#Region "Protected methods"

    Protected Friend Overrides Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, Optional ByVal bParam As Boolean = False)
        Id = xmlRefNodeCounter.GetNewClassId()
        If bParam = False Then
            Name = "New_" + Id
        Else
            ' Name is set by caller
            Name = Name + "_" + Id

            For Each child As XmlNode In SelectNodes("typedef")
                Dim strId As String = xmlRefNodeCounter.GetNewClassId()
                SetID(child, strId)

                For Each enumvalue As XmlNode In XmlProjectTools.SelectNodes(child, "descendant::enumvalue")
                    Dim strId2 As String = GetID(enumvalue)
                    SetID(enumvalue, "enum" + XmlNodeCounter.AfterStr(strId, "class") + "_" + XmlNodeCounter.AfterStr(strId2, "_"))
                Next
            Next
        End If
    End Sub

    Protected Friend Overrides Function AppendNode(ByVal child As XmlNode, Optional ByVal observer As Object = Nothing) As XmlNode
        Dim before As XmlNode = Nothing
        Select Case child.Name
            Case "model"
                before = GetNode("inherited")

                If before Is Nothing Then
                    before = GetNode("dependency")
                End If

                If before Is Nothing Then
                    before = GetNode("collaboration")
                End If

                If before Is Nothing Then
                    before = GetNode("comment")
                End If

            Case "dependency"
                before = GetNode("collaboration")

                If before Is Nothing Then
                    before = GetNode("comment")
                End If

            Case "collaboration"
                before = GetNode("comment")

            Case "inherited"
                before = GetNode("dependency")

                If before Is Nothing Then
                    before = GetNode("collaboration")
                End If

                If before Is Nothing Then
                    before = GetNode("comment")
                End If

            Case "import"
                before = GetNode("typedef")

                If before Is Nothing Then
                    before = GetNode("property")
                End If
                If before Is Nothing Then
                    before = GetNode("method")
                End If

            Case "typedef"
                before = GetNode("property")

                If before Is Nothing Then
                    before = GetNode("method")
                End If

            Case "property"
                before = GetNode("method")
        End Select

        If before Is Nothing Then
            Return Me.Node.AppendChild(child)
        Else
            Return Me.Node.InsertBefore(child, before)
        End If
    End Function

    Protected Function CheckModel(ByVal strID As String) As XmlNode
        Try
            Return GetNode("descendant::*[@*='" + strID + "' and not(self::model)]")

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Protected Function CheckTemplate(Optional ByVal bAlertUser As Boolean = False) As Boolean
        Dim list As XmlNodeList = SelectNodes("model")
        If list.Count > 0 Then
            For Each model As XmlNode In list
                If CheckModel(GetID(model)) IsNot Nothing _
                Then
                    If bAlertUser Then MsgBox("At least one model parameter is in use.", MsgBoxStyle.Exclamation)
                    Return False
                End If
            Next
        End If
        Return True
    End Function

    Protected Sub AddModelCount(ByVal iCount As Integer)
        Try
            Dim j As Integer = 0
            Dim strID As String = ""
            Dim list As XmlNodeList = SelectNodes("model")

            If list.Count > iCount _
            Then
                If iCount = 0 Then
                    RemoveModel("A")
                End If
                RemoveModel("B")
            ElseIf list.Count < iCount _
            Then
                If list.Count = 0 Then
                    AddModel("A")
                End If
                If iCount = 2 Then
                    AddModel("B")
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AddModel(ByVal strlabel As String)
        Try
            Dim model As XmlNode = MyBase.Document.CreateNode(XmlNodeType.Element, "model", "")

            Dim attrib As XmlAttribute = MyBase.Document.CreateAttribute("id")
            attrib.Value = m_xmlReferenceNodeCounter.GetNewClassId()
            model.Attributes.Append(attrib)

            attrib = MyBase.Document.CreateAttribute("name")
            attrib.Value = strlabel
            model.Attributes.Append(attrib)

            Me.AppendNode(model)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub RemoveModel(ByVal strlabel As String)
        Try
            RemoveSingleNode("model[@name='" + strlabel + "']")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region
End Class
