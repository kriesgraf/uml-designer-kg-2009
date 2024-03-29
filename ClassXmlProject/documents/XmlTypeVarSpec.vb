﻿Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports ClassXmlProject.UmlCodeGenerator
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Class XmlTypeVarSpec
    Inherits XmlComposite

#Region "Class declarations"

    Public Const cstFullpathTypeDescription As String = "FullpathTypeDescription"
    Private m_bEnumWrong As Boolean = False
    Private m_bStaticProperty As Boolean = False

    Public Enum EKindDeclaration
        EK_Unknown
        EK_SimpleType
        EK_Structure
        EK_Enumeration
        EK_Union
        EK_Container
    End Enum

    Private m_xmlVariable As XmlVariableSpec = Nothing

#End Region

#Region "Properties"

    Public ReadOnly Property VariableDefinition() As XmlVariableSpec
        Get
            If m_xmlVariable IsNot Nothing Then m_xmlVariable.GenerationLanguage = Me.GenerationLanguage
            Return m_xmlVariable
        End Get
    End Property

    Public ReadOnly Property ParentName() As String
        Get
            Return GetAttribute("name", "parent::*")
        End Get
    End Property

    Public ReadOnly Property ParentNodeName() As String
        Get
            Return Me.Node.ParentNode.Name
        End Get
    End Property

    Public ReadOnly Property IsEnumWrong() As Boolean
        Get
            Return m_bEnumWrong
        End Get
    End Property

    Public ReadOnly Property IsNotOverriden() As Boolean
        Get
            Return String.IsNullOrEmpty(GetAttribute("overrides", "parent::property"))
        End Get
    End Property

    Public Property IsStaticProperty() As Boolean
        Get
            Return m_bStaticProperty
        End Get
        Set(ByVal value As Boolean)
            m_bStaticProperty = value
        End Set
    End Property

    Public ReadOnly Property FullpathTypeDescription() As String
        Get
            Dim strResult As String = ""

            If Me.Node IsNot Nothing Then
                Select Case Kind
                    Case XmlTypeVarSpec.EKindDeclaration.EK_Union
                        strResult = "union {" + GetStructureName(True) + "}"

                    Case XmlTypeVarSpec.EKindDeclaration.EK_Structure
                        strResult = strResult + GetBeginStruct(Me.GenerationLanguage) + GetStructureName(True) + GetEndStruct(Me.GenerationLanguage)

                    Case XmlTypeVarSpec.EKindDeclaration.EK_Container
                        strResult = GetContainer(GetTypeName())

                    Case Else
                        strResult = GetTypeName(True)
                End Select
            End If

            Return strResult
        End Get
    End Property

    Public ReadOnly Property DetailedDescription() As String
        Get
            Dim strResult As String = ""

            If Me.Node IsNot Nothing Then
                Select Case Kind
                    Case XmlTypeVarSpec.EKindDeclaration.EK_Union
                        strResult = "union {" + GetStructureName() + "}"

                    Case XmlTypeVarSpec.EKindDeclaration.EK_Structure
                        strResult = strResult + GetBeginStruct(Me.GenerationLanguage) + GetStructureName() + GetEndStruct(Me.GenerationLanguage)

                    Case XmlTypeVarSpec.EKindDeclaration.EK_Container
                        strResult = GetContainer(GetTypeName())

                    Case Else
                        strResult = GetTypeName()
                End Select
            End If

            Return strResult
        End Get
    End Property

    Public Property Kind() As EKindDeclaration
        Get
            Dim eKind As EKindDeclaration

            If TestNode("enumvalue") Then

                eKind = XmlTypeVarSpec.EKindDeclaration.EK_Enumeration

            ElseIf TestNode("element") Then

                If GetAttribute("struct") = "union" Then
                    eKind = XmlTypeVarSpec.EKindDeclaration.EK_Union
                Else
                    eKind = XmlTypeVarSpec.EKindDeclaration.EK_Structure
                End If
            ElseIf TestNode("list") Then

                eKind = XmlTypeVarSpec.EKindDeclaration.EK_Container
            Else
                eKind = XmlTypeVarSpec.EKindDeclaration.EK_SimpleType
            End If
            Return eKind
        End Get
        Set(ByVal value As EKindDeclaration)
            UpdateNodes(value, Me.Kind)
        End Set
    End Property

    Public Property Reference() As String
        Get
            Return GetAttribute("idref")
        End Get
        Set(ByVal value As String)
            ValidateIdReference(value, m_bCreateNodeNow)
            ReplaceAttribute("desc", "idref")
            SetAttribute("idref", value)
        End Set
    End Property

    Public Property Descriptor() As String
        Get
            Return GetAttribute("desc")
        End Get
        Set(ByVal value As String)
            ReplaceAttribute("idref", "desc")
            SetAttribute("desc", value)
        End Set
    End Property

    Public Property Indexed() As Boolean
        Get
            Return (CheckAttribute("type", "indexed", "simple", "list"))
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("type", "indexed", "list")
            Else
                SetAttribute("type", "simple", "list")
            End If
        End Set
    End Property

    Public Property IndexRef() As String
        Get
            Return GetAttribute("index-idref", "list")
        End Get
        Set(ByVal value As String)
            ValidateIdReference(value, m_bCreateNodeNow)
            ReplaceAttribute("index-desc", "index-idref", "list")
            SetAttribute("index-idref", value, "list")
            SetAttribute("type", "indexed", "list")
        End Set
    End Property

    Public Property IndexDesc() As String
        Get
            Return GetAttribute("index-desc", "list")
        End Get
        Set(ByVal value As String)
            ReplaceAttribute("index-idref", "index-desc", "list")
            SetAttribute("index-desc", value, "list")
            SetAttribute("type", "indexed", "list")
        End Set
    End Property

    Public Property IndexLevel() As Integer
        Get
            Dim s As String = GetAttribute("level", "list")
            If s = "" Then
                Return 0
            End If
            Return CInt(s)
        End Get
        Set(ByVal value As Integer)
            AddAttribute("level", CStr(value), "list")
        End Set
    End Property

    Public Property Iterator() As Boolean
        Get
            Return (CheckAttribute("iterator", "yes", "no", "list"))
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("iterator", "yes", "list")
            Else
                SetAttribute("iterator", "no", "list")
            End If
        End Set
    End Property

    Public Property Range() As String
        Get
            Return m_xmlVariable.GetAttribute("range")
        End Get
        Set(ByVal value As String)
            m_xmlVariable.SetAttribute("range", value)
        End Set
    End Property

    Public Property Value() As String
        Get
            Return m_xmlVariable.GetAttribute("value")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                m_xmlVariable.RemoveAttribute("valref")
                m_xmlVariable.RemoveAttribute("value")
            Else
                m_xmlVariable.ReplaceAttribute("valref", "value")
                m_xmlVariable.AddAttribute("value", value)
            End If
        End Set
    End Property

    Public Property ValRef() As String
        Get
            Return m_xmlVariable.GetAttribute("valref")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                m_xmlVariable.RemoveAttribute("valref")
                m_xmlVariable.RemoveAttribute("value")
            Else
                m_xmlVariable.ReplaceAttribute("value", "valref")
                m_xmlVariable.AddAttribute("valref", value)
            End If
        End Set
    End Property

    Public Property VarSize() As String
        Get
            Return m_xmlVariable.GetAttribute("size")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                m_xmlVariable.RemoveAttribute("size")
                m_xmlVariable.RemoveAttribute("sizeref")
            Else
                m_xmlVariable.ReplaceAttribute("sizeref", "size")
                m_xmlVariable.AddAttribute("size", value)
            End If
        End Set
    End Property

    Public Property SizeRef() As String
        Get
            Return m_xmlVariable.GetAttribute("sizeref")
        End Get
        Set(ByVal value As String)
            If value = "" Then
                m_xmlVariable.RemoveAttribute("size")
                m_xmlVariable.RemoveAttribute("sizeref")
            Else
                m_xmlVariable.ReplaceAttribute("size", "sizeref")
                m_xmlVariable.AddAttribute("sizeref", value)
            End If
        End Set
    End Property

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

    Public Property By() As Boolean
        Get
            Return (CheckAttribute("by", "ref", "val"))
        End Get
        Set(ByVal value As Boolean)
            If value Then
                SetAttribute("by", "ref")
            Else
                SetAttribute("by", "val")
            End If
        End Set
    End Property

    Public Property Level() As Integer
        Get
            Return CInt(GetAttribute("level"))
        End Get
        Set(ByVal value As Integer)
            SetAttribute("level", CStr(value))
        End Set
    End Property

    Public Property ContainerDesc() As String
        Get
            Return GetAttribute("desc", "list")
        End Get
        Set(ByVal value As String)
            ReplaceAttribute("idref", "desc", "list")
            SetAttribute("desc", value, "list")
        End Set
    End Property

    Public Property ContainerType() As String
        Get
            Return GetAttribute("type", "list")
        End Get
        Set(ByVal value As String)
            If value = "simple" Then
                RemoveAttribute("index-idref", "list")
                RemoveAttribute("index-desc", "list")
                AddAttribute("type", value, "list")
            Else
                AddAttribute("index-desc", "list")
                AddAttribute("type", value)
            End If
        End Set
    End Property

    Public Property ContainerRef() As String
        Get
            Return GetAttribute("idref", "list")
        End Get
        Set(ByVal value As String)
            ValidateIdReference(value, m_bCreateNodeNow)
            ReplaceAttribute("desc", "idref", "list")
            SetAttribute("idref", value, "list")
        End Set
    End Property
#End Region

#Region "Constructor/Destructor"

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode, bLoadChildren)
    End Sub
#End Region

#Region "Public methods"

    Public Function CreateDialogBox(Optional ByVal bIsStaticProperty As Boolean = False) As Form
        Dim fen As dlgTypeVar = CType(XmlNodeManager.GetInstance().CreateForm(Me), dlgTypeVar)
        fen.IsStaticProperty = bIsStaticProperty
        Return fen
    End Function

    Public Overrides Function CanPasteItem(ByVal child As XmlComponent) As Boolean
        If child.NodeName = "element" Then
            Return True
        End If
        Return MyBase.CanPasteItem(child)
    End Function

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            Kind = EKindDeclaration.EK_SimpleType
            m_bCreateNodeNow = bCreateNodeNow
            ChangeReferences()
            Me.VariableDefinition.SetDefaultValues(bCreateNodeNow)
            Descriptor = "void"
            Level = 0
            By = False
            Modifier = False

        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub
#End Region

#Region "Protected methods"

    Protected Friend Sub SetContainerValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            Kind = EKindDeclaration.EK_Container
            m_bCreateNodeNow = bCreateNodeNow
            ChangeReferences()
            Me.VariableDefinition.SetDefaultValues(bCreateNodeNow)
            Descriptor = "int16"
            Level = 0
            By = False
            Modifier = False
            ContainerType = "simple"
            ContainerDesc = "undef_template"
            Iterator = False

        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Protected Friend Sub SetStructureValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            Kind = EKindDeclaration.EK_Structure
            m_bCreateNodeNow = bCreateNodeNow
            ChangeReferences()
            Me.VariableDefinition.SetDefaultValues(bCreateNodeNow)

        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Protected Friend Overrides Sub ChangeReferences(Optional ByVal bLoadChildren As Boolean = False)
        Dim nodeXml As XmlNode
        MyBase.ChangeReferences(bLoadChildren)
        If TestNode("following-sibling::variable") = False And m_bCreateNodeNow Then
            nodeXml = CreateAppendNode("following-sibling::variable")
        Else
            nodeXml = GetNode("following-sibling::variable")
        End If

        m_xmlVariable = CreateDocument(nodeXml)
        If m_xmlVariable IsNot Nothing Then m_xmlVariable.GenerationLanguage = Me.GenerationLanguage
    End Sub

    Public Overrides Sub LoadChildrenList(Optional ByVal strViewName As String = "")

        AddChildren(SelectNodes("enumvalue | element"), strViewName)

    End Sub
#End Region

#Region "Private methods"

    Private Sub UpdateNodes(ByVal eKind As EKindDeclaration, ByVal eOldKind As EKindDeclaration)

        Select Case eKind
            Case XmlTypeVarSpec.EKindDeclaration.EK_SimpleType
                Select Case eOldKind
                    Case XmlTypeVarSpec.EKindDeclaration.EK_Union Or XmlTypeVarSpec.EKindDeclaration.EK_Structure
                        RemoveAttribute("struct")
                        RemoveAllNodes("element")

                    Case XmlTypeVarSpec.EKindDeclaration.EK_Enumeration
                        RemoveAllNodes("enumvalue")
                        AddAttribute("desc", "int16")

                    Case XmlTypeVarSpec.EKindDeclaration.EK_Container
                        RemoveAttribute("struct")
                        RemoveSingleNode("list")
                End Select

            Case XmlTypeVarSpec.EKindDeclaration.EK_Container
                AddAttribute("struct", "container")
                AppendNode(CreateNode("list"))
                ContainerType = "simple"
                ContainerDesc = "undef_template"
                Value = ""
                VarSize = ""

                Select Case eOldKind
                    Case XmlTypeVarSpec.EKindDeclaration.EK_Enumeration
                        RemoveAllNodes("enumvalue")
                        AddAttribute("desc", "int16")

                    Case XmlTypeVarSpec.EKindDeclaration.EK_SimpleType
                        Value = ""
                        VarSize = ""

                    Case XmlTypeVarSpec.EKindDeclaration.EK_Structure Or XmlTypeVarSpec.EKindDeclaration.EK_Union
                        RemoveAllNodes("element")
                        AddAttribute("desc", "int16")
                End Select

            Case XmlTypeVarSpec.EKindDeclaration.EK_Structure
                AddAttribute("struct", "struct")

                If eOldKind <> XmlTypeVarSpec.EKindDeclaration.EK_Union Then
                    RemoveAttribute("desc")
                    RemoveAttribute("idref")
                    RemoveSingleNode("list")
                    RemoveAllNodes("enumvalue")
                    Value = ""
                    VarSize = ""
                    Dim xmlcpnt As XmlElementSpec = New XmlElementSpec(AppendNode(CreateNode("element")))
                    xmlcpnt.GenerationLanguage = Me.GenerationLanguage
                    xmlcpnt.SetDefaultValues(True)
                End If

            Case XmlTypeVarSpec.EKindDeclaration.EK_Union
                AddAttribute("struct", "union")

                If eOldKind <> XmlTypeVarSpec.EKindDeclaration.EK_Structure Then
                    RemoveAttribute("desc")
                    RemoveAttribute("idref")
                    RemoveSingleNode("list")
                    RemoveAllNodes("enumvalue")
                    Value = ""
                    VarSize = ""
                    Dim xmlcpnt As XmlElementSpec = New XmlElementSpec(AppendNode(CreateNode("element")))
                    xmlcpnt.GenerationLanguage = Me.GenerationLanguage
                    xmlcpnt.SetDefaultValues(True)
                End If

            Case XmlTypeVarSpec.EKindDeclaration.EK_Enumeration

                If eOldKind <> EKindDeclaration.EK_Enumeration Then
                    Dim element As XmlEnumSpec = CreateDocument("enumvalue", Me.Document)
                    element.GenerationLanguage = Me.GenerationLanguage
                    AppendComponent(element)
                End If

                Select Case eOldKind
                    Case XmlTypeVarSpec.EKindDeclaration.EK_Container
                        RemoveAttribute("struct")
                        RemoveSingleNode("list")

                    Case XmlTypeVarSpec.EKindDeclaration.EK_SimpleType
                        RemoveAttribute("desc")
                        RemoveAttribute("idref")
                        Value = ""
                        VarSize = ""

                    Case XmlTypeVarSpec.EKindDeclaration.EK_Structure Or XmlTypeVarSpec.EKindDeclaration.EK_Union
                        RemoveAttribute("struct")
                        RemoveAllNodes("element")
                End Select
        End Select
    End Sub

    Private Function GetStructureName(Optional ByVal bAbreviated As Boolean = False) As String
        Dim strResult As String = ""
        If bAbreviated Then
            strResult = GetAttribute("name", "element") + ",..."
        Else
            Dim list As XmlNodeList = SelectNodes("element")
            For Each child As XmlNode In list
                strResult += GetName(child)
                If child IsNot list.Item(list.Count - 1) Then
                    strResult += ", "
                End If
            Next
        End If
        Return strResult
    End Function

    Private Function GetTypeName(Optional ByVal bAbreviated As Boolean = False) As String
        Dim strResult As String = ""

        If Reference IsNot Nothing Then
            strResult = XmlTypeVarSpec.GetReferenceTypeDescription(Me.GetElementById(Reference), Me.GenerationLanguage, True)
        ElseIf Descriptor IsNot Nothing Then
            strResult = Descriptor
        End If

        If strResult = "" Then
            strResult += GetBeginEnum(Me.GenerationLanguage)
            If bAbreviated = False Then
                Dim list As XmlNodeList = SelectNodes("enumvalue")
                For Each child As XmlNode In list
                    strResult += GetName(child)
                    If child IsNot list.Item(list.Count - 1) Then
                        strResult += ", "
                    End If
                Next
            Else
                strResult += GetAttribute("name", "enumvalue") + ",..."
            End If
            strResult += GetEndStruct(Me.GenerationLanguage)
        End If

        If Modifier Then
            If Me.GenerationLanguage <> ELanguage.Language_CplusPlus Then
                strResult = "Const " + strResult
            Else
                strResult = "const " + strResult
            End If
        End If

        strResult = strResult + DisplayLevel(Level, Me.GenerationLanguage)

        If By Then
            If Me.GenerationLanguage <> ELanguage.Language_CplusPlus Then
                If Me.ParentNodeName = "param" Then
                    strResult = "ByRef As " + strResult
                End If
            Else
                strResult = strResult + Chr(38)
            End If
        Else
            If Me.GenerationLanguage <> ELanguage.Language_CplusPlus Then
                If Me.ParentNodeName = "param" Then
                    strResult = "ByVal As " + strResult
                End If
            End If
        End If

        If VarSize <> "" Or SizeRef <> "" _
        Then
            strResult = strResult + " " + GetArrayString(Me.GenerationLanguage)
        End If

        Return strResult
    End Function

    Private Function GetContainer(ByVal szFullpathTypeDescription As String) As String
        Dim strResult As String = ""

        strResult = GetBeginContainer(Me.GenerationLanguage)

        If ContainerType = "indexed" Then
            If IndexRef IsNot Nothing Then
                strResult = strResult + GetReferenceTypeDescription(Me.GetElementById(IndexRef), Me.GenerationLanguage, True)
            ElseIf IndexDesc IsNot Nothing Then
                strResult = strResult + IndexDesc
            End If
            strResult = strResult + DisplayLevel(Level, Me.GenerationLanguage) + ", "
        End If

        strResult = strResult + szFullpathTypeDescription + GetEndContainer(Me.GenerationLanguage)

        Return strResult
    End Function

    Public Shared Function GetReferenceTypeDescription(ByVal child As XmlNode, ByVal eTag As ELanguage, _
                                                       Optional ByVal bShorter As Boolean = False) As String
        Dim strResult As String = ""

        If child Is Nothing Then
            Return ""
        End If

        If bShorter Then
            strResult = GetName(child)

        Else
            strResult = GetFullpathDescription(child, eTag)
        End If

        Return strResult
    End Function
#End Region
End Class



