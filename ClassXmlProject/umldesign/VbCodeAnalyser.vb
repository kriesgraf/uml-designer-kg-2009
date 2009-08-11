Imports System
Imports System.Xml
Imports System.IO
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic

Public Class VbCodeAnalyser
    Implements IDisposable

#Region "Constants"

    ' General declarations


    Private Const cstAccessModifier As String = "\b(Protected Friend |Protected |Public |Private |)"  ' Nothing for interface declaration
    Private Const cstAccessModifier2 As String = "\b(Dim |Protected Friend |Protected Friend |Protected |Public |Private |)"  ' 
    Private Const cstVarTypeFuncClassName As String = "(\b[a-zA-Z_][a-zA-Z0-9_]{0,}\b)"
    Private Const cstNamespace As String = "([a-zA-Z0-9_\.]{1,})"
    Private Const cstMustOverride As String = "MustOverride"
    Private Const cstAllText As String = "(.*.|)"
    Private Const cstArraySize As String = "(\(.*.\)|\(\)|)"
    Private Const cstEvent As String = "Event"
    Private Const cstOperators As String = "(-|&\b|\*\b|/\b|\\\b|\^\b|\+\b|<\b|<<|<=|<>|=|>\b|>=|>>|And|CType|IsFalse|IsTrue|Like|Mod|Not|Or|Xor)"
    Private Const cstStringDeclaration As String = Chr(34) + "[^" + Chr(34) + "\\]*(?:\\.[^" + Chr(34) + "\\]*)*" + Chr(34)
    Private Const cstStringReplace As String = Chr(34) + "[REPLACE]" + Chr(34)
    Private Const cstInOutParam As String = "\b(ByVal |ByRef )"
    Private Const cstParameter As String = cstVarTypeFuncClassName + cstArraySize + " As " + cstNamespace + cstArraySize
    Private Const cstParameter2 As String = cstVarTypeFuncClassName + cstArraySize + " As (New |)" + cstNamespace + cstArraySize
    Private Const cstEndStatement As String = "\b(End )({0})"
    Private Const cstEnumDeclaration As String = "(\b\w+)( = (\d+))?"

    Private Const cstImportsDeclaration As String = "\bImports " + cstNamespace
    Private Const cstRegionDeclaration As String = "\#Region (" + cstStringDeclaration + ")"
    Private Const cstAttributeValue As String = "( = (.*.))?"

    ' Class & Package declarations
    Private Const cstClassDeclaration As String = cstAccessModifier + cstAllText + "(Class |Interface )" + cstVarTypeFuncClassName
    Private Const cstPackageDeclaration As String = "\bNamespace " + cstVarTypeFuncClassName
    Private Const cstInheritsDeclaration As String = "\b(Inherits |Implements )" + cstNamespace
    ' Members
    Private Const cstAttributeDeclaration As String = cstAccessModifier + cstAllText + cstParameter2
    Private Const cstStructDeclaration As String = cstAccessModifier2 + cstAllText + cstParameter2
    Private Const cstFunction As String = "Function"
    Private Const cstAbstractMethod As String = "\b(" + cstFunction + " |Sub |" + cstEvent + " )" + cstVarTypeFuncClassName
    Private Const cstMethodDeclaration2 As String = "\(" + cstAllText + "\)"
    Private Const cstMethodDeclaration1 As String = "\(" + cstAllText + "\) As " + cstNamespace + cstArraySize
    Private Const cstMethodArgument As String = "(Optional |)" + cstInOutParam + cstParameter + cstAttributeValue
    Private Const cstMethodDeclaration As String = cstAccessModifier + cstAllText + "(" + cstMustOverride + " |)" + cstAllText + cstAbstractMethod
    Private Const cstTypedef As String = cstAccessModifier + cstAllText + "(Structure |Enum )" + cstVarTypeFuncClassName
    Private Const cstOperatorDeclaration As String = cstAccessModifier + cstAllText + "Shared " + cstAllText + "(" + cstMustOverride + " |)" + "Operator " + cstOperators + "\(" + cstAllText + "\) As " + cstNamespace + cstArraySize

    ' Properties
    Private Const cstAbstractProperty As String = cstAllText + "Property " + cstVarTypeFuncClassName + "\(\) As " + cstNamespace + cstArraySize
    Private Const cstPropertyDeclaration As String = cstAccessModifier + cstAbstractProperty
    Private Const cstAccessorGetDeclaration As String = "Get"
    Private Const cstAccessorSetDeclaration As String = "Set"

#End Region

#Region "Shared class members"

    Private Shared regImportsDeclaration As New Regex(cstImportsDeclaration)
    Private Shared regInheritsDeclaration As New Regex(cstInheritsDeclaration)
    Private Shared regPackageDeclaration As New Regex(cstPackageDeclaration)
    Private Shared regClassDeclaration As New Regex(cstClassDeclaration)
    Private Shared regStringDeclaration As New Regex(cstStringDeclaration)
    Private Shared regAttributeDeclaration As New Regex(cstAttributeDeclaration)
    Private Shared regPropertyDeclaration As New Regex(cstPropertyDeclaration)
    Private Shared regAbstractProperty As New Regex(cstAbstractProperty)
    Private Shared regEndProperty As New Regex(Strings.Format(cstEndStatement, "Property"))
    Private Shared regAccessorGetDeclaration As New Regex(cstAccessorGetDeclaration)
    Private Shared regAccessorSetDeclaration As New Regex(cstAccessorSetDeclaration)
    Private Shared regMethodDeclaration As New Regex(cstMethodDeclaration)
    Private Shared regMethodDeclaration1 As New Regex(cstMethodDeclaration1)
    Private Shared regMethodDeclaration2 As New Regex(cstMethodDeclaration2)
    Private Shared regOperatorDeclaration As New Regex(cstOperatorDeclaration)
    Private Shared regAbstractMethod As New Regex(cstAbstractMethod)
    Private Shared regTypedefDeclaration As New Regex(cstTypedef)
    Private Shared regParamDeclaration As New Regex(cstMethodArgument)
    Private Shared regRegionDeclaration As New Regex(cstRegionDeclaration)
    Private Shared regEnumDeclaration As New Regex(cstEnumDeclaration)
    Private Shared regStructDeclaration As New Regex(cstStructDeclaration)

#End Region

#Region "Class members"

    Private m_iNbLine As Integer = 0
    Private m_textWriter As XmlTextWriter = Nothing
    Private m_streamReader As StreamReader = Nothing
    Private m_lineReader As StreamReader = Nothing
    Private m_xmlDocument As XmlDocument = Nothing
    '    Private m_listLines As New SortedList(Of Integer, Long)
    Private m_strVbSourceName As String
    Private m_bParseInheritsDeclaration As Boolean = False
    Private m_bParseTypedefsDeclaration As Boolean = False

#End Region

#Region "Properties"

    Public ReadOnly Property Document() As XmlDocument
        Get
            Return m_xmlDocument
        End Get
    End Property

    Public ReadOnly Property Filename() As String
        Get
            Return m_strVbSourceName
        End Get
    End Property

    Public WriteOnly Property InheritsDeclaration() As Boolean
        Set(ByVal value As Boolean)
            m_bParseInheritsDeclaration = value
        End Set
    End Property

    Public WriteOnly Property TypedefsDeclaration() As Boolean
        Set(ByVal value As Boolean)
            m_bParseTypedefsDeclaration = value
        End Set
    End Property

#End Region

#Region "Type definitions"

    Private Enum ClassMember
        UnknownElt
        AttributeElt
        TypedefElt
        GetterElt
        SetterElt
        PropertyElt
        ImplementedMethod
        AbstractMethod
        NestedClass
    End Enum

    Private Enum ProcessState
        StartOfInstruction
        EndOfFile
        Instruction
        WaitData
    End Enum

#End Region

#Region "Public methods"
    Public Function Analyse(ByVal strName As String) As String
        Return Analyse(Path.GetDirectoryName(strName), Path.GetFileName(strName), "", "")
    End Function

    Public Function Analyse(ByVal strFolder As String, ByVal strName As String, _
                            Optional ByVal strSuffix As String = "", Optional ByVal strTempFolder As String = "") As String
        Dim strXmlBase As String = My.Computer.FileSystem.CombinePath(strFolder, strTempFolder)
        strXmlBase = My.Computer.FileSystem.CombinePath(strXmlBase, strName + strSuffix + ".xml")
        Try
            Using textWriter As XmlTextWriter = New XmlTextWriter(strXmlBase, Nothing)
                textWriter.Formatting = Formatting.Indented
                textWriter.Indentation = 4
                textWriter.IndentChar = Chr(32)

                textWriter.WriteStartDocument()
                textWriter.WriteStartElement("root")
                textWriter.WriteAttributeString("file", strName)
                textWriter.WriteAttributeString("folder", strFolder)

                Dim strComment As String = vbCrLf + "cstClassDeclaration=" + cstClassDeclaration _
                                            + vbCrLf + "cstStringDeclaration=" + cstStringDeclaration _
                                            + vbCrLf + "cstAttributeDeclaration=" + cstAttributeDeclaration _
                                            + vbCrLf + "cstPropertyDeclaration=" + cstPropertyDeclaration _
                                            + vbCrLf + "cstMethodDeclaration=" + cstMethodDeclaration _
                                            + vbCrLf + "cstMethodDeclaration1=" + cstMethodDeclaration1 _
                                            + vbCrLf + "cstMethodDeclaration2=" + cstMethodDeclaration2 _
                                            + vbCrLf + "cstPropertyDeclaration=" + cstPropertyDeclaration _
                                            + vbCrLf + "cstAbstractMethod=" + cstAbstractMethod _
                                            + vbCrLf + "cstTypedef=" + cstTypedef _
                                            + vbCrLf + "cstOperatorDeclaration=" + cstOperatorDeclaration _
                                            + vbCrLf + "cstImportsDeclaration=" + cstImportsDeclaration _
                                            + vbCrLf + "cstRegionDeclaration=" + cstRegionDeclaration _
                                            + vbCrLf + "cstMethodArgument=" + cstMethodArgument + vbCrLf

                textWriter.WriteComment(strComment)

                m_strVbSourceName = My.Computer.FileSystem.CombinePath(strFolder, strName)

                Using streamReader As StreamReader = New StreamReader(m_strVbSourceName)
                    m_textWriter = textWriter
                    m_streamReader = streamReader
                    m_iNbLine = 0
                    Dim iStartLine As Integer = 0
                    Dim iStopLine As Integer = 0
                    Dim strReadLine As String = Nothing
                    Dim strInstruction As String = ""
                    Dim strStatement As String = ""
                    Dim eState As ProcessState = ProcessState.StartOfInstruction
                    Dim iStopDeclaration As Integer = 0
                    Do
                        m_iNbLine += 1
                        'm_listLines.Add(m_iNbLine, streamReader.BaseStream.Position)
                        strReadLine = streamReader.ReadLine()
                        'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

                        If ParseLine(eState, strReadLine, strInstruction, iStartLine, iStopLine, iStopDeclaration) = ProcessState.Instruction _
                        Then
                            If CheckClassInstruction(iStartLine, strInstruction, strStatement) _
                            Then
                                ParseClassDeclaration(iStopLine, strStatement)

                            ElseIf CheckPackageInstruction(iStartLine, iStopLine, strInstruction) _
                            Then
                                ParsePackageDeclaration()

                            ElseIf CheckRegionInstruction(iStartLine, iStopLine, strInstruction) _
                            Then
                                ParseRegionDeclaration(False, False)

                            ElseIf CheckImportsInstruction(iStartLine, iStopLine, strInstruction) _
                            Then
                                ParseImportsDeclaration()
                            End If

                            strInstruction = ""
                            iStartLine = 0
                            iStopLine = 0
                        End If
                    Loop Until strReadLine Is Nothing

                    streamReader.Close()
                End Using

                textWriter.Flush()
                textWriter.WriteString(vbCrLf)
                textWriter.WriteEndElement()
                textWriter.WriteEndDocument()
                textWriter.Close()
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            m_textWriter = Nothing
            m_streamReader = Nothing
            m_iNbLine = 0
        End Try

        Try
            m_xmlDocument = New XmlDocument
            m_xmlDocument.Load(strXmlBase)

        Catch ex As Exception
            Throw ex
        End Try
        Return strXmlBase
    End Function

    Public Function LoadLines(ByVal iStartLine As Integer, ByVal iStopLine As Integer) As String
        Dim strResult As String = ""
        Try
            If iStopLine < iStartLine Then
                Return ""
            End If

            Dim strReadLine As String

            If iStartLine < m_iNbLine Then
                m_lineReader.Close()
                m_lineReader = Nothing
            End If

            If m_lineReader Is Nothing Then
                m_iNbLine = 0
                m_lineReader = New StreamReader(m_strVbSourceName)
            End If

            Do
                m_iNbLine += 1
                strReadLine = m_lineReader.ReadLine()
                'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

                If strReadLine Is Nothing Then
                    m_lineReader.Close()
                    m_lineReader = Nothing
                    Exit Do
                Else
                    If m_iNbLine = iStartLine _
                    Then
                        strResult = strReadLine

                    ElseIf m_iNbLine > iStartLine _
                    Then
                        strResult += vbCrLf + strReadLine
                    End If
                End If
            Loop Until strReadLine Is Nothing Or m_iNbLine >= iStopLine

        Catch ex As Exception
            Throw ex
        End Try
        Return strResult
    End Function

#End Region

#Region "Private methods"

    Private Sub ParseImportsDeclaration()
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim eState As ProcessState = ProcessState.StartOfInstruction
        Dim iStopDeclaration As Integer = 0

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If ParseLine(eState, strReadLine, strInstruction, iStartLine, iStopLine, iStopDeclaration) = ProcessState.Instruction _
            Then
                If regImportsDeclaration.IsMatch(strInstruction) = False _
                Then
                    Exit Do

                ElseIf CheckRegionInstruction(iStartLine, iStopLine, strInstruction) _
                Then
                    Throw New Exception("Do not defined Region inside 'Imports' declarations")
                End If

                strInstruction = ""
                iStartLine = 0
                iStopLine = 0
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-imports", CStr(m_iNbLine - 1))
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
        m_textWriter.Flush()
    End Sub

    Private Sub ParsePackageDeclaration()
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim strStatement As String = ""
        Dim regex As New Regex("(End )(Namespace)")
        Dim eState As ProcessState = ProcessState.StartOfInstruction
        Dim iStopDeclaration As Integer = 0

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If ParseLine(eState, strReadLine, strInstruction, iStartLine, iStopLine, iStopDeclaration) = ProcessState.Instruction _
            Then
                If regex.IsMatch(strInstruction) _
                Then
                    Exit Do

                ElseIf CheckClassInstruction(iStartLine, strInstruction, strStatement) _
                Then
                    ParseClassDeclaration(iStopLine, strStatement)

                ElseIf CheckPackageInstruction(iStartLine, iStopLine, strInstruction) _
                Then
                    ParsePackageDeclaration()

                ElseIf CheckRegionInstruction(iStartLine, iStopLine, strInstruction) _
                Then
                    ParseRegionDeclaration(False, False)
                End If

                strInstruction = ""
                iStartLine = 0
                iStopLine = 0
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-package", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
        m_textWriter.Flush()
    End Sub

    Private Sub ParseRegionDeclaration(ByVal bInterface As Boolean, _
                                       Optional ByVal bAcceptMembers As Boolean = True)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim strStatement As String = ""
        Dim regex As New Regex("(End )(Region)")
        Dim eState As ProcessState = ProcessState.StartOfInstruction
        Dim iStopDeclaration As Integer = 0

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If ParseLine(eState, strReadLine, strInstruction, iStartLine, iStopLine, iStopDeclaration) = ProcessState.Instruction _
            Then
                If regex.IsMatch(strInstruction) Then
                    Exit Do

                ElseIf CheckClassInstruction(iStartLine, strInstruction, strStatement) _
                Then
                    ParseClassDeclaration(iStopLine, strStatement)

                ElseIf CheckPackageInstruction(iStartLine, iStopLine, strInstruction) _
                Then
                    Throw New Exception("Do not defined 'Namespace' inside 'Region' declarations")

                ElseIf CheckRegionInstruction(iStartLine, iStopLine, strInstruction) _
                Then
                    Throw New Exception("Do not defined interlocked 'Region' declarations")

                ElseIf CheckInheritsInstruction(iStartLine, iStopLine, strInstruction, strStatement) _
                Then
                    ParseInheritsDeclaration()

                ElseIf bAcceptMembers Then
                    ParseMemberInstruction(iStartLine, iStopLine, strInstruction, bInterface)
                End If
                strInstruction = ""
                iStartLine = 0
                iStopLine = 0
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-region", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
        m_textWriter.Flush()
    End Sub

    Private Sub ParseClassDeclaration(ByVal iStopClassDeclaration As Integer, _
                                      ByVal strStatement As String)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim regex As New Regex("(End )(" + strStatement.Trim() + ")")
        Dim bInterface As Boolean = (strStatement.Trim() = "Interface")
        Dim eState As ProcessState = ProcessState.StartOfInstruction

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If strReadLine IsNot Nothing Then
                If InStr(strReadLine, "Implements") > 0 And iStopClassDeclaration > 0 And m_bParseInheritsDeclaration = False Then
                    iStopClassDeclaration = m_iNbLine

                ElseIf InStr(strReadLine, "Inherits") > 0 And iStopClassDeclaration > 0 And m_bParseInheritsDeclaration = False Then
                    iStopClassDeclaration = m_iNbLine

                ElseIf ParseLine(eState, strReadLine, strInstruction, iStartLine, iStopLine, iStopClassDeclaration) = ProcessState.Instruction _
                Then
                    If iStopClassDeclaration > 0 Then
                        m_textWriter.WriteAttributeString("end", iStopClassDeclaration.ToString)
                        iStopClassDeclaration = 0
                    End If

                    If regex.IsMatch(strInstruction) Then
                        If iStopClassDeclaration > 0 _
                        Then
                            m_textWriter.WriteAttributeString("end", iStopClassDeclaration.ToString)
                        End If

                        Exit Do

                    ElseIf CheckInheritsInstruction(iStartLine, iStopLine, strInstruction, strStatement) _
                    Then
                        ParseInheritsDeclaration()

                    ElseIf CheckRegionInstruction(iStartLine, iStopLine, strInstruction) _
                    Then
                        ParseRegionDeclaration(bInterface)

                    ElseIf CheckClassInstruction(iStartLine, strInstruction, strStatement) _
                    Then
                        ParseClassDeclaration(iStopLine, strStatement)
                    Else
                        ParseMemberInstruction(iStartLine, iStopLine, strInstruction, bInterface)
                    End If
                    strInstruction = ""
                    iStartLine = 0
                    iStopLine = 0
                End If
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-class", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
        m_textWriter.Flush()
    End Sub

    Private Sub ParseInheritsDeclaration()

        If m_bParseInheritsDeclaration = False Then Exit Sub
    End Sub

    Private Sub ParseMemberInstruction(ByVal iStartLine As Integer, ByVal iStopLine As Integer, _
                                       ByVal strInstruction As String, ByVal bInterface As Boolean)

        Dim strStatement As String = ""

        Select Case CheckMemberInstruction(iStartLine, iStopLine, strInstruction, strStatement, bInterface)
            Case ClassMember.PropertyElt
                ParsePropertyBody(bInterface)

            Case ClassMember.ImplementedMethod
                ParseEndStatement(strStatement, "method", False)

            Case ClassMember.NestedClass
                If m_bParseTypedefsDeclaration Then
                    ParseClassDeclaration(0, strStatement)
                Else
                    ParseEndStatement(strStatement, "class", False)
                End If

            Case ClassMember.TypedefElt
                ParseTypedefBody(strStatement)
        End Select
    End Sub

    Private Sub ParseMethodBody(ByVal strStatement As String)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim regex As New Regex("(End )(" + strStatement + ")")
        Dim eState As ProcessState = ProcessState.StartOfInstruction
        Dim iStopDeclaration As Integer = 0

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If ParseLine(eState, strReadLine, strInstruction, iStartLine, iStopLine, iStopDeclaration) = ProcessState.Instruction _
                Then
                If regex.IsMatch(strInstruction) Then
                    Exit Do
                End If

                strInstruction = ""
                iStartLine = 0
                iStopLine = 0
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-method", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
        m_textWriter.Flush()
    End Sub

    Private Sub ParseTypedefBody(ByVal strStatement As String)

        If m_bParseTypedefsDeclaration = False Then
            ParseEndStatement(strStatement, "typedef", False)
        Else
            Select Case strStatement
                Case "Enum"
                    ParseEnumStatement()

                Case "Structure"
                    ParseStructStatement()
            End Select
        End If
    End Sub

    Private Sub ParsePropertyBody(ByVal bInterface As Boolean)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim eState As ProcessState = ProcessState.StartOfInstruction
        Dim iStopDeclaration As Integer = 0

        If bInterface = False Then
            Do
                m_iNbLine += 1
                'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
                strReadLine = m_streamReader.ReadLine()
                'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

                If ParseLine(eState, strReadLine, strInstruction, iStartLine, iStopLine, iStopDeclaration) = ProcessState.Instruction _
                    Then
                    If regEndProperty.IsMatch(strInstruction) Then
                        Exit Do
                    End If

                    Select Case CheckGetSetInstruction(iStartLine, iStopLine, strInstruction)
                        Case ClassMember.GetterElt
                            ParseEndStatement("Get", "get")
                        Case ClassMember.SetterElt
                            ParseEndStatement("Set", "set")
                    End Select
                    strInstruction = ""
                    iStartLine = 0
                    iStopLine = 0
                End If
            Loop Until strReadLine Is Nothing

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteElementString("end-property", m_iNbLine.ToString)
            m_textWriter.WriteString(vbCrLf)
        End If

        m_textWriter.WriteEndElement()
        m_textWriter.Flush()
    End Sub

    Private Sub ParseEndStatement(ByVal strStatement As String, ByVal strElement As String, Optional ByVal bCheckComment As Boolean = True)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim regex As New Regex("(End )(" + strStatement + ")")
        Dim eState As ProcessState = ProcessState.StartOfInstruction

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)
            Dim iStopClassDeclaration As Integer = 0
            If ParseLine(eState, strReadLine, strInstruction, iStartLine, iStopLine, iStopClassDeclaration, bCheckComment) = ProcessState.Instruction _
                Then
                If regex.IsMatch(strInstruction) Then
                    Exit Do
                End If
                iStartLine = 0
                iStopLine = 0
                strInstruction = ""
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-" + strElement, m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
        m_textWriter.Flush()
    End Sub

    Private Sub ParseEnumStatement(Optional ByVal bCheckComment As Boolean = True)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim regex As New Regex("(End )(Enum)")
        Dim eState As ProcessState = ProcessState.StartOfInstruction

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            Dim iStopClassDeclaration As Integer = 0
            If ParseLine(eState, strReadLine, strInstruction, iStartLine, iStopLine, iStopClassDeclaration, bCheckComment) = ProcessState.Instruction _
                Then
                If regex.IsMatch(strInstruction) Then
                    Exit Do
                ElseIf regEnumDeclaration.IsMatch(strInstruction) _
                Then
                    Dim groups As GroupCollection = regEnumDeclaration.Match(strInstruction).Groups

                    For i As Integer = 0 To groups.Count - 1
                        Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
                    Next
                    m_textWriter.WriteString(vbCrLf)
                    m_textWriter.WriteStartElement("enumvalue")
                    m_textWriter.WriteAttributeString("checked", "False")
                    m_textWriter.WriteAttributeString("start", iStartLine.ToString)
                    m_textWriter.WriteAttributeString("pos", iStopLine.ToString)
                    m_textWriter.WriteAttributeString("name", groups(1).ToString.Trim)
                    m_textWriter.WriteAttributeString("value", groups(3).ToString.Trim)
                    m_textWriter.Flush()
                End If
                iStartLine = 0
                iStopLine = 0
                strInstruction = ""
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-typedef", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
        m_textWriter.Flush()
    End Sub

    Private Sub ParseStructStatement(Optional ByVal bCheckComment As Boolean = True)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim regex As New Regex("(End )(Structure)")
        Dim eState As ProcessState = ProcessState.StartOfInstruction

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            Dim iStopClassDeclaration As Integer = 0
            If ParseLine(eState, strReadLine, strInstruction, iStartLine, iStopLine, iStopClassDeclaration, bCheckComment) = ProcessState.Instruction _
                Then
                If regex.IsMatch(strInstruction) Then
                    Exit Do
                ElseIf regStructDeclaration.IsMatch(strInstruction) _
                Then
                    Dim groups As GroupCollection = regStructDeclaration.Match(strInstruction).Groups

                    For i As Integer = 0 To groups.Count - 1
                        Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
                    Next
                    m_textWriter.WriteString(vbCrLf)
                    m_textWriter.WriteStartElement("element")
                    m_textWriter.WriteAttributeString("checked", "False")
                    m_textWriter.WriteAttributeString("start", iStartLine.ToString)
                    m_textWriter.WriteAttributeString("end", iStopLine.ToString)
                    m_textWriter.WriteAttributeString("visibility", groups(1).ToString.Trim)
                    m_textWriter.WriteAttributeString("name", groups(3).ToString.Trim)
                    m_textWriter.WriteAttributeString("other", groups(2).ToString.Trim)
                    m_textWriter.WriteAttributeString("size", groups(4).ToString.Trim + groups(7).ToString.Trim)
                    m_textWriter.WriteAttributeString("type", groups(6).ToString.Trim)
                    m_textWriter.WriteEndElement()
                    m_textWriter.Flush()
                End If
                iStartLine = 0
                iStopLine = 0
                strInstruction = ""
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-typedef", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
        m_textWriter.Flush()
    End Sub

    Private Function ParseLine(ByRef eStateResult As ProcessState, _
                               ByRef strReadLine As String, ByRef strInstruction As String, _
                               ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                               ByRef iStopClassDeclaration As Integer, _
                               Optional ByVal bCheckComment As Boolean = True) As ProcessState

        If strReadLine IsNot Nothing Then
            If eStateResult = ProcessState.WaitData Then
                strReadLine = Strings.LTrim(strReadLine)
            End If
            If CheckComment(strReadLine, iStopClassDeclaration, bCheckComment) = False Then

                eStateResult = ProcessState.EndOfFile

            ElseIf iStartLine > 0 Then

                If Strings.Right(strReadLine, 1) = "_" Then

                    strInstruction += strReadLine.Substring(0, strReadLine.Length - 1)
                    eStateResult = ProcessState.WaitData
                Else
                    iStopLine = m_iNbLine
                    strInstruction += strReadLine
                    Dim i1 As Integer = InStr(strInstruction, "<")
                    Dim i2 As Integer = InStr(strInstruction, ">")
                    If i2 > i1 And i1 > 0 Then
                        strInstruction = strInstruction.Substring(i2 + 1)
                        iStartLine = iStopLine
                    End If
                    eStateResult = ProcessState.Instruction
                End If

            ElseIf Strings.Right(strReadLine, 1) = "_" Then

                iStartLine = m_iNbLine
                strInstruction += strReadLine.Substring(0, strReadLine.Length - 1)
                eStateResult = ProcessState.WaitData

            Else
                strInstruction = strReadLine
                iStartLine = m_iNbLine
                iStopLine = m_iNbLine
                eStateResult = ProcessState.Instruction
            End If
        Else
            eStateResult = ProcessState.EndOfFile
        End If
        Return eStateResult
    End Function

    Private Function CheckImportsInstruction(ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                                             ByVal strInstruction As String) As Boolean

        If regImportsDeclaration.IsMatch(strInstruction) Then

            Dim iPos As Integer = GetPosition(regImportsDeclaration, strInstruction, iStartLine)

            Dim groups As GroupCollection = regImportsDeclaration.Match(strInstruction).Groups

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("imports")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", groups(1).ToString.Trim)
            m_textWriter.Flush()
            Return True
        End If
        Return False
    End Function

    Private Function CheckPackageInstruction(ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                                             ByVal strInstruction As String) As Boolean

        If regPackageDeclaration.IsMatch(strInstruction) Then

            Dim iPos As Integer = GetPosition(regPackageDeclaration, strInstruction, iStartLine)

            Dim groups As GroupCollection = regPackageDeclaration.Match(strInstruction).Groups

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("package")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", groups(1).ToString.Trim())
            m_textWriter.Flush()
            Return True
        End If
        Return False
    End Function

    Private Function CheckRegionInstruction(ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                                            ByVal strInstruction As String) As Boolean
        If regRegionDeclaration.IsMatch(strInstruction) Then

            Dim groups As GroupCollection = regRegionDeclaration.Match(strInstruction).Groups

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("region")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("name", groups(1).ToString.Trim)
            m_textWriter.Flush()
            Return True
        End If
        Return False
    End Function

    Private Function CheckClassInstruction(ByRef iStartLine As Integer, ByVal strInstruction As String, _
                                           ByRef strStatement As String) As Boolean
        strStatement = ""
        If regClassDeclaration.IsMatch(strInstruction) Then

            Dim iPos As Integer = GetPosition(regClassDeclaration, strInstruction, iStartLine)

            Dim groups As GroupCollection = regClassDeclaration.Match(strInstruction).Groups
            strStatement = groups(3).ToString.Trim

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("class")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", groups(4).ToString.Trim)
            m_textWriter.WriteAttributeString("visibility", groups(1).ToString.Trim)
            m_textWriter.WriteAttributeString("other", groups(2).ToString.Trim)
            m_textWriter.WriteAttributeString("kind", strStatement)
            m_textWriter.Flush()
            Return True
        End If
        Return False
    End Function

    Private Function CheckInheritsInstruction(ByRef iStartLine As Integer, ByVal iStopLine As Integer, _
                                            ByVal strInstruction As String, ByRef strStatement As String) As Boolean

        If regInheritsDeclaration.IsMatch(strInstruction) _
        Then
            Dim groups As GroupCollection = regInheritsDeclaration.Match(strInstruction).Groups

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            If regInheritsDeclaration.Split(strInstruction)(0).Trim.Length = 0 _
            Then
                strStatement = groups(1).ToString.Trim

                m_textWriter.WriteString(vbCrLf)
                m_textWriter.WriteStartElement("inherited")
                m_textWriter.WriteAttributeString("checked", "False")
                m_textWriter.WriteAttributeString("start", iStartLine.ToString)
                m_textWriter.WriteAttributeString("end", iStopLine.ToString)
                m_textWriter.WriteAttributeString("name", groups(2).ToString.Trim)
                m_textWriter.WriteAttributeString("other", groups(1).ToString.Trim)
                m_textWriter.WriteEndElement()
                m_textWriter.Flush()
                Return True
            End If
        End If

        Return False
    End Function

    Private Function CheckMemberInstruction(ByRef iStartLine As Integer, ByVal iStopLine As Integer, _
                                            ByVal strInstruction As String, ByRef strStatement As String, _
                                            ByVal bInterface As Boolean) As ClassMember

        ' The order of call regex is made to avoid complicated string
        If bInterface And regAbstractProperty.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regAbstractProperty, strInstruction, iStartLine)

            Dim groups As GroupCollection = regAbstractProperty.Match(strInstruction).Groups

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("property")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", groups(2).ToString.Trim)
            m_textWriter.WriteAttributeString("other", groups(1).ToString.Trim)
            m_textWriter.WriteAttributeString("type", groups(3).ToString.Trim())
            m_textWriter.WriteAttributeString("size", groups(4).ToString.Trim())
            m_textWriter.Flush()
            Return ClassMember.PropertyElt

        ElseIf regPropertyDeclaration.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regPropertyDeclaration, strInstruction, iStartLine)

            Dim groups As GroupCollection = regPropertyDeclaration.Match(strInstruction).Groups

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("property")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("visibility", groups(1).ToString.Trim)
            m_textWriter.WriteAttributeString("name", groups(3).ToString.Trim)
            m_textWriter.WriteAttributeString("other", groups(2).ToString.Trim)
            m_textWriter.WriteAttributeString("type", groups(4).ToString.Trim())
            m_textWriter.WriteAttributeString("size", groups(5).ToString.Trim())
            m_textWriter.Flush()
            Return ClassMember.PropertyElt

        ElseIf regTypedefDeclaration.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regTypedefDeclaration, strInstruction, iStartLine)

            Dim groups As GroupCollection = regTypedefDeclaration.Match(strInstruction).Groups

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            strStatement = groups(3).ToString.Trim()

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("typedef")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("visibility", groups(1).ToString.Trim)
            m_textWriter.WriteAttributeString("name", groups(4).ToString.Trim())
            m_textWriter.WriteAttributeString("type", groups(3).ToString.Trim())
            m_textWriter.WriteAttributeString("other", groups(2).ToString.Trim())
            m_textWriter.Flush()
            Return ClassMember.TypedefElt

        ElseIf bInterface Then
            If regAbstractMethod.IsMatch(strInstruction) Then

                Dim iPos = GetPosition(regAbstractMethod, strInstruction, iStartLine)

                Dim groups As GroupCollection = regAbstractMethod.Match(strInstruction).Groups

                For i = 0 To groups.Count - 1
                    Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
                Next

                strStatement = groups(1).ToString.Trim()

                m_textWriter.WriteString(vbCrLf)
                m_textWriter.WriteStartElement("method")
                m_textWriter.WriteAttributeString("checked", "False")
                m_textWriter.WriteAttributeString("start", iStartLine.ToString)
                m_textWriter.WriteAttributeString("end", iStopLine.ToString)
                m_textWriter.WriteAttributeString("pos", iPos.ToString)
                m_textWriter.WriteAttributeString("name", groups(2).ToString.Trim())
                m_textWriter.WriteAttributeString("type", strStatement)
                m_textWriter.Flush()
                Dim tempo As String

                If strStatement = cstFunction Then
                    groups = regMethodDeclaration1.Match(strInstruction).Groups
                    tempo = groups(1).ToString

                    m_textWriter.WriteAttributeString("return", groups(2).ToString.Trim())
                    m_textWriter.WriteAttributeString("size", groups(3).ToString.Trim())
                Else
                    groups = regMethodDeclaration2.Match(strInstruction).Groups
                    tempo = groups(1).ToString.Trim
                End If

                For i = 0 To groups.Count - 1
                    Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
                Next

                CheckParamsInstruction(tempo)

                m_textWriter.WriteEndElement()
                m_textWriter.Flush()

                Return ClassMember.AbstractMethod

            ElseIf CheckClassInstruction(iStartLine, strInstruction, strStatement) Then

                Return ClassMember.NestedClass
            End If
        ElseIf regMethodDeclaration.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regMethodDeclaration, strInstruction, iStartLine)
            Dim groups As GroupCollection = regMethodDeclaration.Match(strInstruction).Groups

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            strStatement = groups(5).ToString.Trim()
            Dim tempo As String = groups(2).ToString.Trim() + " " + groups(3).ToString.Trim() + " " + groups(4).ToString.Trim()

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("method")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("visibility", groups(1).ToString.Trim())
            m_textWriter.WriteAttributeString("other", tempo.Trim())
            m_textWriter.WriteAttributeString("type", strStatement)
            m_textWriter.WriteAttributeString("name", groups(6).ToString.Trim())
            m_textWriter.Flush()
            tempo = groups(3).ToString.Trim()

            If strStatement = cstFunction Then
                groups = regMethodDeclaration1.Match(strInstruction).Groups
                CheckParamsInstruction(groups(1).ToString.Trim)

                m_textWriter.WriteAttributeString("return", groups(2).ToString.Trim())
                m_textWriter.WriteAttributeString("size", groups(3).ToString.Trim())
            Else
                groups = regMethodDeclaration2.Match(strInstruction).Groups
                CheckParamsInstruction(groups(1).ToString.Trim)
            End If

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            If strStatement = cstEvent Then
                m_textWriter.WriteEndElement()
                m_textWriter.Flush()
                Return ClassMember.AbstractMethod

            ElseIf tempo = cstMustOverride Then
                m_textWriter.WriteEndElement()
                m_textWriter.Flush()
                Return ClassMember.AbstractMethod
            End If
            Return ClassMember.ImplementedMethod

        ElseIf regOperatorDeclaration.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regOperatorDeclaration, strInstruction, iStartLine)
            Dim groups As GroupCollection = regOperatorDeclaration.Match(strInstruction).Groups

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            strStatement = "Operator"
            Dim tempo As String = groups(2).ToString.Trim() + " " + groups(3).ToString.Trim() + " " + groups(4).ToString.Trim()

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("method")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", groups(5).ToString.Trim())
            m_textWriter.WriteAttributeString("return", groups(7).ToString.Trim())
            m_textWriter.WriteAttributeString("size", groups(8).ToString.Trim())
            m_textWriter.WriteAttributeString("visibility", groups(1).ToString.Trim())
            m_textWriter.WriteAttributeString("other", tempo.Trim())
            m_textWriter.Flush()
            If groups(3).ToString.Trim() = cstMustOverride Then
                m_textWriter.WriteEndElement()
                m_textWriter.Flush()
                Return ClassMember.AbstractMethod
            End If
            CheckParamsInstruction(groups(7).ToString.Trim)
            Return ClassMember.ImplementedMethod

        ElseIf regAttributeDeclaration.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regAttributeDeclaration, strInstruction, iStartLine)

            Dim groups As GroupCollection = regAttributeDeclaration.Match(strInstruction).Groups

            For i = 0 To groups.Count - 1
                Debug.Print(i.ToString + "-[" + groups(i).ToString + "]")
            Next

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("attribute")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("visibility", groups(1).ToString.Trim)
            m_textWriter.WriteAttributeString("name", groups(3).ToString.Trim)
            m_textWriter.WriteAttributeString("other", groups(2).ToString.Trim)
            m_textWriter.WriteAttributeString("size", groups(4).ToString.Trim + groups(7).ToString.Trim)
            m_textWriter.WriteAttributeString("type", groups(6).ToString.Trim)
            m_textWriter.WriteAttributeString("default", groups(9).ToString.Trim)
            m_textWriter.WriteEndElement()
            m_textWriter.Flush()

            Return ClassMember.AttributeElt

        ElseIf CheckClassInstruction(iStartLine, strInstruction, strStatement) Then

            Return ClassMember.NestedClass
        End If

        Return ClassMember.UnknownElt
    End Function

    Private Sub CheckParamsInstruction(ByVal strListParams As String)

        Dim strParams As String = ""
        Dim strTypes As String = ""
        Dim strValues As String = ""

        For Each brutParam As String In strListParams.Split(",")
            Dim group As GroupCollection = regParamDeclaration.Match(brutParam).Groups

            For i = 0 To group.Count - 1
                Debug.Print(i.ToString + "-[" + group(i).ToString + "]")
            Next

            If strParams.Length > 0 Then
                strParams += ","
                strTypes += ","
                strValues += ","
            End If
            Dim tempo As String
            tempo = group(1).ToString.Trim + " " + group(2).ToString.Trim + " " + group(3).ToString.Trim
            strParams += tempo.Trim
            tempo = group(5).ToString.Trim + group(6).ToString.Trim
            strTypes += tempo.Trim
            tempo = group(8).ToString.Trim
            strValues += tempo.Trim
        Next
        m_textWriter.WriteAttributeString("params", strParams.Trim)
        m_textWriter.WriteAttributeString("types", strTypes.Trim)
        m_textWriter.WriteAttributeString("values", strValues.Trim)
    End Sub

    Private Function CheckGetSetInstruction(ByVal iStartLine As Integer, ByVal iStopLine As Integer, _
                                            ByVal strInstruction As String) As ClassMember
        If regAccessorGetDeclaration.IsMatch(strInstruction) Then

            Dim iPos = InStr(strInstruction, "Get")

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("get")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.Flush()
            Return ClassMember.GetterElt

        ElseIf regAccessorSetDeclaration.IsMatch(strInstruction) Then

            Dim iPos = InStr(strInstruction, "Set")

            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("set")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.Flush()
            Return ClassMember.SetterElt
        End If

        Return ClassMember.UnknownElt
    End Function

    Private Function CheckComment(ByRef strReadLine As String, _
                                  ByRef iStopClassDeclaration As Integer, _
                                    ByVal bCheckComment As Boolean) As Boolean

        Dim iPos As Integer = InStr(strReadLine, "'''")
        Dim iComment As Integer = InStr(strReadLine, "''''")

        If iPos = 0 Or iComment > 0 Then
            If InStr(strReadLine, "'") > 0 Then
                If regStringDeclaration.IsMatch(strReadLine) Then
                    strReadLine = regStringDeclaration.Replace(strReadLine, cstStringReplace)
                    'Console.WriteLine("String detected: " + strReadLine)
                End If
                Dim i As Integer = InStr(strReadLine, "'")
                If i > 0 Then
                    strReadLine = Strings.Left(strReadLine, i - 1)
                End If
            End If
            Return True
        Else
            ''Console.WriteLine("VB-Doc: " + strReadLine)


            If bCheckComment Then
                If iStopClassDeclaration > 0 Then
                    m_textWriter.WriteAttributeString("end", iStopClassDeclaration.ToString)
                    iStopClassDeclaration = 0
                End If
                m_textWriter.WriteString(vbCrLf)
                m_textWriter.WriteStartElement("vb-doc")
                m_textWriter.WriteAttributeString("checked", "False")
                m_textWriter.WriteAttributeString("start", m_iNbLine.ToString)
                m_textWriter.WriteAttributeString("pos", iPos.ToString)
                m_textWriter.WriteString(strReadLine)
            End If
            m_textWriter.Flush()

            Do
                m_iNbLine += 1
                'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
                strReadLine = m_streamReader.ReadLine()
                'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

                If strReadLine IsNot Nothing Then
                    If InStr(strReadLine, "'''") = 0 Then

                        If bCheckComment Then
                            m_textWriter.WriteElementString("end-vb-doc", CStr(m_iNbLine - 1))
                            m_textWriter.WriteString(vbCrLf)
                            m_textWriter.WriteEndElement()
                            m_textWriter.Flush()
                        End If
                        m_textWriter.Flush()
                        Return CheckComment(strReadLine, iStopClassDeclaration, bCheckComment)
                    Else
                        ''Console.WriteLine("VB-Doc: " + strReadLine)
                        If bCheckComment Then m_textWriter.WriteString(vbCrLf + strReadLine)
                    End If
                End If
            Loop Until strReadLine Is Nothing

            If bCheckComment Then m_textWriter.WriteEndElement()
            m_textWriter.Flush()

            Return False
        End If
        Return True
    End Function

    Private Function GetPosition(ByVal regEx As Regex, ByVal strInstruction As String, _
                                 ByRef iStartLine As Integer) As Integer

        Dim groups As GroupCollection = regEx.Match(strInstruction).Groups
        Dim iPos = groups(0).Index
        Dim strPrecedingInstruction As String = Strings.Left(strInstruction, iPos)
        Dim split As String() = strPrecedingInstruction.Split(vbLf)

        If split.Length > 1 Then
            iStartLine += split.Length - 1
            iPos = split(split.Length - 1).Length
        End If
        Return iPos + 1
    End Function

#End Region

#Region " IDisposable Support "

    Private disposedValue As Boolean = False        ' Pour détecter les appels redondants

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_lineReader IsNot Nothing Then
                    m_lineReader.Close()
                    m_lineReader = Nothing
                End If
            End If
            m_xmlDocument = Nothing
        End If
        Me.disposedValue = True
    End Sub

    ' Ce code a été ajouté par Visual Basic pour permettre l'implémentation correcte du modèle pouvant être supprimé.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ne modifiez pas ce code. Ajoutez du code de nettoyage dans Dispose(ByVal disposing As Boolean) ci-dessus.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
