Imports System
Imports ClassXmlProject.XmlReferenceNodeCounter
Imports System.ComponentModel
Imports System.Xml

Public Class XmlConstructorSpec
    Inherits XmlMethodSpec

    ' rend le champ ReadOnly pour tout binding !
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

    Public Sub New(Optional ByRef xmlNode As XmlNode = Nothing, Optional ByVal bLoadChildren As Boolean = False)
        MyBase.New(xmlNode)
        Try
            ChangeReferences(bLoadChildren)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Friend Function CheckCopyConstructor() As Boolean
        Dim bResult As Boolean = False
        Try
            Dim strClassId As String = MyBase.GetAttribute("id", "ancestor::class")

            If MyBase.SelectNodes("param").Count = 1 Then
                Dim xmlcpnt As XmlParamSpec = MyBase.CreateDocument(MyBase.GetNode("param"))
                xmlcpnt.Tag = Me.Tag
                If xmlcpnt.TypeVarDefinition.Modifier = True And _
                    xmlcpnt.TypeVarDefinition.By = True And _
                    xmlcpnt.TypeVarDefinition.Level = 0 And _
                    xmlcpnt.TypeVarDefinition.Value = "" And _
                    xmlcpnt.TypeVarDefinition.ValRef = "" And _
                    xmlcpnt.TypeVarDefinition.VarSize = "" And _
                    xmlcpnt.TypeVarDefinition.SizeRef = "" And _
                    xmlcpnt.TypeVarDefinition.Reference = strClassId _
                Then
                    bResult = True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Protected Friend Sub ReplaceCopyConstructor()
        Try
            Dim strClassId As String = MyBase.GetAttribute("id", "ancestor::class")
            Dim xmlcpnt As XmlParamSpec

            If MyBase.SelectNodes("param").Count = 1 Then
                xmlcpnt = MyBase.CreateDocument(MyBase.GetNode("param"))
                xmlcpnt.Tag = Me.Tag
            Else
                MyBase.RemoveAllNodes("param")
                xmlcpnt = CreateDocument(CreateAppendNode("param"))
                xmlcpnt.Tag = Me.Tag
                xmlcpnt.SetDefaultValues(True)
                xmlcpnt.NumId = "1"
            End If

            With xmlcpnt.TypeVarDefinition
                .Kind = XmlTypeVarSpec.EKindDeclaration.EK_SimpleType
                .Level = 0
                .Modifier = True
                .By = True
                .Reference = strClassId
                .Value = ""
                .VarSize = ""
            End With
            Me.Updated = True
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overrides Sub SetDefaultValues(Optional ByVal bCreateNodeNow As Boolean = True)
        Try
            m_bCreateNodeNow = bCreateNodeNow

            Kind = EKindMethod.EK_Constructor

            ChangeReferences()

            If bCreateNodeNow Then
                Dim xmlParam As XmlParamSpec = CreateDocument(CreateAppendNode("param"))
                xmlParam.Tag = Me.Tag
                xmlParam.SetDefaultValues(bCreateNodeNow)
                xmlParam.NumId = "1"
                xmlParam = Nothing
            End If
            Inline = False
            Modifier = False
            Member = "object"
            Implementation = XmlProjectTools.EImplementation.Simple
            NumId = "0"
            Comment = "Insert here a comment"
            BriefComment = "Brief comment"
            Range = "public"

        Catch ex As Exception
            Throw ex
        Finally
            m_bCreateNodeNow = False
        End Try
    End Sub

    Public Overrides Sub NotifyInsert(Optional ByVal before As XmlComponent = Nothing)
        NumId = GenerateNumericId(Me.Node.ParentNode, "method", "CONST")
    End Sub

    Protected Friend Overrides Function CreateNode(ByVal name As String) As XmlNode
        Return MyBase.CreateNode("method")
    End Function

End Class
