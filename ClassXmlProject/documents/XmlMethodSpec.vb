Imports System
Imports ClassXmlProject.XmlReferenceNodeCounter
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools
Imports System.ComponentModel
Imports System.Xml


Public Class XmlMethodSpec
    Inherits XmlComposite

#Region "Class declarations"

    Public Const cstOperator As String = "operator"

    Public Enum EKindMethod
        EK_Unknown
        EK_Method
        EK_Constructor
        EK_Operator
    End Enum

    Private m_xmlReturnValue As XmlTypeVarSpec = Nothing

#End Region

#Region "Properties"

    <CategoryAttribute("Code generation"), _
    DescriptionAttribute("Project language")> _
    Public ReadOnly Property GenerationLanguage() As ELanguage
        Get
            Return CType(MyBase.Tag, ELanguage)
        End Get
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("VB.NET method declaration")> _
    Public Property Behaviour() As String
        Get
            Dim strResult As String = GetAttribute("behaviour")
            If strResult Is Nothing Then
                strResult = "Normal"
            End If
            Return strResult
        End Get
        Set(ByVal value As String)
            If value <> "Normal" Then
                AddAttribute("behaviour", value)
            Else
                RemoveAttribute("behaviour")
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Component name")> _
    Public Overrides Property Name() As String
        Get
            Select Case Me.Kind
                Case EKindMethod.EK_Constructor
                    Return "Constructor"
                Case EKindMethod.EK_Operator
                    Return "operator(" + Me.OperatorName + ")"
                Case Else
                    Return MyBase.Name
            End Select
        End Get
        Set(ByVal value As String)
            MyBase.Name = value
        End Set
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Operator declaration")> _
    Public Property OperatorName() As String
        Get
            If Me.Kind = EKindMethod.EK_Constructor Then
                Return ""
            End If
            Return GetAttribute("operator")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("operator")
            Else
                AddAttribute("operator", value)
            End If
        End Set
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Component return value")> _
    Public ReadOnly Property ReturnValue() As XmlTypeVarSpec
        Get
            If m_xmlReturnValue IsNot Nothing Then
                m_xmlReturnValue.Tag = Me.Tag
            End If
            Return m_xmlReturnValue
        End Get
    End Property

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("Return value comment")> _
    Public Property ReturnComment() As String
        Get
            Return GetNodeString("return/comment")
        End Get
        Set(ByVal value As String)
            SetNodeString("return/comment", value)
        End Set
    End Property

    Protected Friend Property Kind() As EKindMethod
        Get
            Dim eKind As EKindMethod
            Dim strKind = GetAttribute("constructor")

            If strKind <> "" And strKind <> "no" _
            Then
                eKind = EKindMethod.EK_Constructor

            ElseIf GetAttribute("operator") IsNot Nothing _
            Then
                eKind = EKindMethod.EK_Operator
            Else
                eKind = EKindMethod.EK_Method
            End If
            Return eKind
        End Get
        Set(ByVal value As EKindMethod)
            UpdateNodes(value)
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Const method")> _
    Public Property Modifier() As Boolean
        Get
            Return (CheckAttribute("modifier", "const", "var"))
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("modifier", "const")
            Else
                SetAttribute("modifier", "var")
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("C++ custom inline code")> _
    Public Property Inline() As Boolean
        Get
            Return (CheckAttribute("inline", "yes", "no"))
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("inline", "yes")
            Else
                SetAttribute("inline", "no")
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Overrides method")> _
    Public Property OverridesMethod() As String
        Get
            Return GetAttribute("overrides")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                RemoveAttribute("overrides")
            Else
                AddAttribute("overrides", CStr(value))
            End If
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberClass)), _
    CategoryAttribute("UML design"), _
   DescriptionAttribute("Class member")> _
    Public Property Member() As String
        Get
            Return GetAttribute("member")
        End Get
        Set(ByVal value As String)
            SetAttribute("member", value)
        End Set
    End Property

    <TypeConverter(GetType(UmlMemberVisibility)), _
    CategoryAttribute("UML design"), _
    DescriptionAttribute("Method visibility")> _
    Public Property Range() As String
        Get
            If Me.Kind = EKindMethod.EK_Constructor Then

                Return GetAttribute("constructor")
            Else
                Return Me.ReturnValue.Range
            End If
        End Get
        Set(ByVal value As String)

            If Me.Kind = EKindMethod.EK_Constructor Then

                SetAttribute("constructor", value)
            Else
                Me.ReturnValue.Range = value
            End If
        End Set
    End Property

    <CategoryAttribute("UML design"), _
   DescriptionAttribute("Implementation")> _
    Public Property Implementation() As EImplementation
        Get
            Return ConvertDtdToEnumImpl(GetAttribute("implementation"))
        End Get
        Set(ByVal value As EImplementation)
            Dim strValue As String = ConvertEnumImplToDtd(value)
            SetAttribute("implementation", strValue)
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

    <CategoryAttribute("UML design"), _
    DescriptionAttribute("detail comment")> _
    Public Property Comment() As String
        Get
            Return GetNodeString("comment")
        End Get
        Set(ByVal value As String)
            SetNodeString("comment", value)
        End Set
    End Property

    <CategoryAttribute("XmlComponent"), _
    DescriptionAttribute("Component member id")> _
    Public Property NumId() As String
        Get
            Return GetAttribute("num-id")
        End Get
        Set(ByVal value As String)
            SetAttribute("num-id", value)
        End Set
    End Property
#End Region

#Region "Public methods"

    Public Overrides Function CanPasteItem(ByVal child As XmlComponent) As Boolean
        Select Case child.NodeName
            Case "param"
                Return True

            Case Else
                Return MyBase.CanPasteItem(child)
        End Select
        Return MyBase.CanPasteItem(child)
    End Function

    Public Overrides Sub LoadChildrenList(Optional ByVal strViewName As String = "")
        Try
            AddChildren(SelectNodes("exception | param"), strViewName)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode, bLoadChildren)
    End Sub

    Public Overrides Sub NotifyInsert(Optional ByVal before As XmlComponent = Nothing)
        NumId = GenerateNumericId(Me.Node.ParentNode, "method")
    End Sub

    Protected Friend Overrides Sub SetIdReference(ByVal xmlRefNodeCounter As XmlReferenceNodeCounter, Optional ByVal eRename As XmlComponent.ENameReplacement = XmlComponent.ENameReplacement.NewName, Optional ByVal bSetIdrefChildren As Boolean = False)
        Name = Name + "_" + CStr(NumId)
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            ' This method reset m_bCreateNodeNow !
            MyBase.SetDefaultValues(bCreateNodeNow)

            m_bCreateNodeNow = bCreateNodeNow

            Kind = EKindMethod.EK_Method

            ChangeReferences()

            Me.ReturnValue.SetDefaultValues(bCreateNodeNow)
            Me.ReturnValue.Descriptor = "void"

            Dim xmlParam As XmlParamSpec = CreateDocument(GetNode("param"))
            xmlParam.Tag = Me.Tag
            xmlParam.SetDefaultValues(bCreateNodeNow)
            xmlParam.NumId = "1"
            xmlParam = Nothing

            Name = "New_method"
            Inline = False
            Modifier = False
            Member = "object"
            Implementation = EImplementation.Simple
            NumId = "0"
            Comment = "Insert here a comment"
            BriefComment = "Brief comment"
            ReturnComment = "Return comment"
            Range = "public"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Function Clone(ByVal nodeXml As XmlNode, Optional ByVal bLoadChildren As Boolean = False) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            If nodeXml Is Nothing _
            Then
                xmlResult = MyBase.Clone(Nothing, bLoadChildren)
            Else
                Dim xmlAttribut As XmlNode = nodeXml.SelectSingleNode("@constructor")
                If xmlAttribut Is Nothing _
                Then
                    xmlResult = MyBase.Clone(nodeXml, bLoadChildren)
                ElseIf xmlAttribut.Value <> "no" _
                Then
                    xmlResult = New XmlConstructorSpec(nodeXml, bLoadChildren)
                Else
                    xmlResult = MyBase.Clone(nodeXml, bLoadChildren)
                End If
            End If

            If xmlResult IsNot Nothing Then xmlResult.Tag = Me.Tag

        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

#End Region

#Region "Protected & private methods"

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        Try
            MyBase.ChangeReferences(bLoadChildren)

            Dim nodeXml As XmlNode = Nothing

            If TestNode("return") Then
                Dim nodeType As XmlNode = GetNode("return/type")
                m_xmlReturnValue = TryCast(CreateDocument(nodeType, bLoadChildren), XmlTypeVarSpec)
                If m_xmlReturnValue IsNot Nothing Then m_xmlReturnValue.Tag = Me.Tag
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Overrides Function AppendNode(ByVal nodeXml As XmlNode, Optional ByVal observer As Object = Nothing) As XmlNode
        Dim before As XmlNode = Nothing
        ' exception*,return?,comment,param*,inline
        Select Case nodeXml.Name
            Case "comment"
                before = GetNode("param")

            Case "exception"
                before = GetNode("return")

                If before Is Nothing Then
                    before = GetNode("comment")
                End If

                If before Is Nothing Then
                    before = GetNode("param")
                End If

            Case "return"
                before = GetNode("comment")

                If before Is Nothing Then
                    before = GetNode("param")
                End If

            Case "param"
                before = Nothing
        End Select

        If before Is Nothing Then
            Return Me.Node.AppendChild(nodeXml)
        Else
            Return Me.Node.InsertBefore(nodeXml, before)
        End If
    End Function

    Private Sub UpdateNodes(ByVal eValue As EKindMethod)
        Try
            Dim nodeXml As XmlNode
            Select Case eValue
                Case EKindMethod.EK_Constructor
                    RemoveSingleNode("return")
                    SetAttribute("constructor", "public")
                    RemoveAttribute("operator")

                Case EKindMethod.EK_Method
                    If TestNode("return") = False Then
                        nodeXml = CreateAppendNode("return")
                        nodeXml.AppendChild(CreateNode("type"))
                        nodeXml.AppendChild(CreateNode("variable"))
                    End If
                    SetAttribute("constructor", "no")
                    RemoveAttribute("operator")

                Case EKindMethod.EK_Operator
                    If TestNode("return") = False Then
                        nodeXml = CreateAppendNode("return")
                        nodeXml.AppendChild(CreateNode("type"))
                        nodeXml.AppendChild(CreateNode("variable"))
                    End If
                    SetAttribute("constructor", "no")
                    SetAttribute("name", cstOperator)
                    SetAttribute("operator", "Operand")

                Case Else
                    Throw New Exception("Unknown method")
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region
End Class

