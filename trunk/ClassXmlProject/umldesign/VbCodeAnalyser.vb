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
    Private Const cstLineFeed As String = "(_\r\n\s{0,}|)"
    Private Const cstLineFeedWithBlank As String = "( _\r\n\s{0,}|)"
    Private Const cstAccessModifier As String = "(Protected Friend |Protected |Public |Private |)"  ' Nothing for intertface declaration
    Private Const cstVarTypeFuncClassName As String = "(\b[a-zA-Z_][a-zA-Z0-9_]{0,}\b)"
    Private Const cstNamespace As String = "([a-zA-Z09_\.]{1,})"
    Private Const cstMustOverride As String = "MustOverride"
    Private Const cstAllText As String = "(.*.|)"
    Private Const cstArraySize As String = "(\(.*.\)|\(\)|)"
    Private Const cstEvent As String = "Event"
    Private Const cstOperators As String = "(-|&\b|\*\b|/\b|\\\b|\^\b|\+\b|<\b|<<|<=|<>|=|>\b|>=|>>|And|CType|IsFalse|IsTrue|Like|Mod|Not|Or|Xor)"
    Private Const cstStringDeclaration As String = Chr(34) + "[^" + Chr(34) + "\\]*(?:\\.[^" + Chr(34) + "\\]*)*" + Chr(34)
    Private Const cstStringReplace As String = Chr(34) + "[REPLACE]" + Chr(34)
    Private Const cstInOutParam As String = "(ByVal |ByRef )"
    Private Const cstParameter As String = cstVarTypeFuncClassName + cstLineFeedWithBlank + cstArraySize + " " + cstLineFeed + "As " + cstLineFeed + cstNamespace + cstLineFeedWithBlank + cstArraySize
    Private Const cstEndStatement As String = "(End )" + cstLineFeed + "({0})"
    Private Const cstImportsDeclaration As String = "(Imports )" + cstLineFeed + cstNamespace
    Private Const cstRegionDeclaration As String = "(\#Region )" + cstLineFeed + "(" + cstStringDeclaration + ")"

    ' Class & Package declarations
    Private Const cstClassDeclaration As String = cstAccessModifier + cstAllText + "(Class |Interface )" + cstLineFeed + cstVarTypeFuncClassName
    Private Const cstPackageDeclaration As String = "(Namespace )" + cstLineFeed + cstVarTypeFuncClassName
    ' Members
    Private Const cstAttributeDeclaration As String = cstAccessModifier + cstLineFeed + cstParameter
    Private Const cstAbstractMethod As String = "(Function |Sub |" + cstEvent + " )" + cstLineFeed + cstVarTypeFuncClassName
    Private Const cstMethodDeclaration As String = cstAccessModifier + cstAllText + "(" + cstMustOverride + " |)" + cstAllText + cstAbstractMethod + cstLineFeed + "\(" + cstAllText + "\)"
    Private Const cstMethodArgument As String = cstInOutParam + cstLineFeed + cstParameter
    Private Const cstTypedef As String = cstAccessModifier + cstAllText + "(Structure |Enum )" + cstLineFeed + cstVarTypeFuncClassName
    Private Const cstOperatorDeclaration As String = cstAccessModifier + cstAllText + "Shared " + cstAllText + "Operator " + cstLineFeed + cstOperators

    ' Properties
    Private Const cstPropertyDeclaration As String = cstAccessModifier + cstAllText + "Property " + cstLineFeed + cstVarTypeFuncClassName + cstLineFeedWithBlank + "(\(\)) " + cstLineFeed + "As " + cstLineFeed + cstNamespace + cstLineFeedWithBlank + cstArraySize
    Private Const cstAccessorGetDeclaration As String = "Get"
    Private Const cstAccessorSetDeclaration As String = "Set"

#End Region

#Region "Shared class members"

    Private Shared regImportsDeclaration As New Regex(cstImportsDeclaration)
    Private Shared regPackageDeclaration As New Regex(cstPackageDeclaration)
    Private Shared regClassDeclaration As New Regex(cstClassDeclaration)
    Private Shared regStringDeclaration As New Regex(cstStringDeclaration)
    Private Shared regAttributeDeclaration As New Regex(cstAttributeDeclaration)
    Private Shared regPropertyDeclaration As New Regex(cstPropertyDeclaration)
    Private Shared regEndProperty As New Regex(Strings.Format(cstEndStatement, "Property"))
    Private Shared regAccessorGetDeclaration As New Regex(cstAccessorGetDeclaration)
    Private Shared regAccessorSetDeclaration As New Regex(cstAccessorSetDeclaration)
    Private Shared regMethodDeclaration As New Regex(cstMethodDeclaration)
    Private Shared regOperatorDeclaration As New Regex(cstOperatorDeclaration)
    Private Shared regAbstractMethod As New Regex(cstAbstractMethod)
    Private Shared regTypedefDeclaration As New Regex(cstTypedef)
    Private Shared regParamDeclaration As New Regex(cstMethodArgument)
    Private Shared regRegionDeclaration As New Regex(cstRegionDeclaration)

#End Region

#Region "Class members"

    Private m_iNbLine As Integer = 0
    Private m_textWriter As XmlTextWriter = Nothing
    Private m_streamReader As StreamReader = Nothing
    Private m_lineReader As StreamReader = Nothing
    Private m_xmlDocument As XmlDocument = Nothing
    '    Private m_listLines As New SortedList(Of Integer, Long)
    Private m_strVbSourceName As String

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
        EndOfFile
        Instruction
        WaitData
    End Enum

#End Region

#Region "Public methods"
    Public Function Analyse(ByVal strFolder As String, ByVal strName As String, Optional ByVal strSuffix As String = "", Optional ByVal strTempFolder As String = "") As String
        Dim strXmlBase As String = My.Computer.FileSystem.CombinePath(strFolder, strTempFolder)
        strXmlBase = My.Computer.FileSystem.CombinePath(strXmlBase, strName + strSuffix + ".xml")
        Try
            Using textWriter As XmlTextWriter = New XmlTextWriter(strXmlBase, Nothing)
                textWriter.Formatting = Formatting.Indented
                textWriter.Indentation = 4
                textWriter.IndentChar = Chr(32)

                textWriter.WriteStartDocument()
                textWriter.WriteStartElement("root")

                Dim strComment As String = vbCrLf + "cstClassDeclaration=" + cstClassDeclaration _
                                            + vbCrLf + "cstStringDeclaration=" + cstStringDeclaration _
                                            + vbCrLf + "cstAttributeDeclaration=" + cstAttributeDeclaration _
                                            + vbCrLf + "cstPropertyDeclaration=" + cstPropertyDeclaration _
                                            + vbCrLf + "cstMethodDeclaration=" + cstMethodDeclaration _
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
                    Do
                        m_iNbLine += 1
                        'm_listLines.Add(m_iNbLine, streamReader.BaseStream.Position)
                        strReadLine = streamReader.ReadLine()
                        'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

                        If ParseLine(strReadLine, strInstruction, iStartLine, iStopLine) = ProcessState.Instruction _
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

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If ParseLine(strReadLine, strInstruction, iStartLine, iStopLine) = ProcessState.Instruction _
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

        m_textWriter.Flush()
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-imports", CStr(m_iNbLine - 1))
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
    End Sub

    Private Sub ParsePackageDeclaration()
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim strStatement As String = ""
        Dim regex As New Regex("(End )" + cstLineFeed + "(Namespace)")

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If ParseLine(strReadLine, strInstruction, iStartLine, iStopLine) = ProcessState.Instruction _
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

        m_textWriter.Flush()
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-package", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
    End Sub

    Private Sub ParseRegionDeclaration(ByVal bInterface As Boolean, Optional ByVal bAcceptMembers As Boolean = True)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim strStatement As String = ""
        Dim regex As New Regex("(End )" + cstLineFeed + "(Region)")

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If ParseLine(strReadLine, strInstruction, iStartLine, iStopLine) = ProcessState.Instruction _
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

                ElseIf bAcceptMembers Then
                    ParseMemberInstruction(iStartLine, iStopLine, strInstruction, bInterface)
                End If
                strInstruction = ""
                iStartLine = 0
                iStopLine = 0
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.Flush()
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-region", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
    End Sub

    Private Sub ParseClassDeclaration(ByVal iStopClassDeclaration As Integer, ByVal strStatement As String)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim regex As New Regex("(End )" + cstLineFeed + "(" + strStatement + ")")
        Dim bInterface As Boolean = (strStatement = "Interface")

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If strReadLine IsNot Nothing Then
                If InStr(strReadLine, "Implements") > 0 And iStopClassDeclaration > 0 Then
                    iStopClassDeclaration = m_iNbLine

                ElseIf InStr(strReadLine, "Inherits") > 0 And iStopClassDeclaration > 0 Then
                    iStopClassDeclaration = m_iNbLine

                ElseIf ParseLine(strReadLine, strInstruction, iStartLine, iStopLine) = ProcessState.Instruction _
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

                    ElseIf CheckRegionInstruction(iStartLine, iStopLine, strInstruction) _
                    Then
                        ParseRegionDeclaration(bInterface)
                    Else
                        ParseMemberInstruction(iStartLine, iStopLine, strInstruction, bInterface)
                    End If
                    strInstruction = ""
                    iStartLine = 0
                    iStopLine = 0
                End If
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.Flush()
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-class", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
    End Sub

    Private Sub ParseMemberInstruction(ByVal iStartLine As Integer, ByVal iStopLine As Integer, _
                                       ByVal strInstruction As String, ByVal bInterface As Boolean)
        Dim strStatement As String = ""

        Select Case CheckMemberInstruction(iStartLine, iStopLine, strInstruction, strStatement, bInterface)
            Case ClassMember.PropertyElt
                ParsePropertyBody()

            Case ClassMember.ImplementedMethod
                ParseEndStatement(strStatement, "method", False)

            Case ClassMember.NestedClass
                ParseEndStatement(strStatement, "class", False)

            Case ClassMember.TypedefElt
                ParseEndStatement(strStatement, "typedef", False)
        End Select
    End Sub

    Private Sub ParseMethodBody(ByVal strStatement As String)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim regex As New Regex("(End )" + cstLineFeed + "(" + strStatement + ")")

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If ParseLine(strReadLine, strInstruction, iStartLine, iStopLine) = ProcessState.Instruction _
                Then
                If regex.IsMatch(strInstruction) Then
                    Exit Do
                End If

                strInstruction = ""
                iStartLine = 0
                iStopLine = 0
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.Flush()
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-method", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
    End Sub

    Private Sub ParsePropertyBody()
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If ParseLine(strReadLine, strInstruction, iStartLine, iStopLine) = ProcessState.Instruction _
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

        m_textWriter.Flush()
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-property", m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
    End Sub

    Private Sub ParseEndStatement(ByVal strStatement As String, ByVal strElement As String, Optional ByVal bCheckComment As Boolean = True)
        Dim iStartLine As Integer = 0
        Dim iStopLine As Integer = 0
        Dim strReadLine As String
        Dim strInstruction As String = ""
        Dim regex As New Regex("(End )" + cstLineFeed + "(" + strStatement + ")")

        Do
            m_iNbLine += 1
            'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
            strReadLine = m_streamReader.ReadLine()
            'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

            If ParseLine(strReadLine, strInstruction, iStartLine, iStopLine, bCheckComment) = ProcessState.Instruction _
                Then
                If regex.IsMatch(strInstruction) Then
                    Exit Do
                End If
            End If
        Loop Until strReadLine Is Nothing

        m_textWriter.Flush()
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteElementString("end-" + strElement, m_iNbLine.ToString)
        m_textWriter.WriteString(vbCrLf)
        m_textWriter.WriteEndElement()
    End Sub

    Private Function ParseLine(ByRef strReadLine As String, ByRef strInstruction As String, _
                               ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                               Optional ByVal bCheckComment As Boolean = True) As ProcessState

        If strReadLine IsNot Nothing Then
            If CheckComment(strReadLine, bCheckComment) = False Then
                Return ProcessState.EndOfFile

            ElseIf iStartLine > 0 Then

                If Strings.Right(strReadLine, 1) = "_" Then

                    strInstruction += strReadLine + vbCrLf
                Else
                    iStopLine = m_iNbLine
                    strInstruction += strReadLine
                    Return ProcessState.Instruction
                End If

            ElseIf Strings.Right(strReadLine, 1) = "_" Then

                iStartLine = m_iNbLine
                strInstruction += strReadLine + vbCrLf

            Else
                strInstruction = strReadLine
                iStartLine = m_iNbLine
                iStopLine = m_iNbLine
                Return ProcessState.Instruction
            End If
        Else
            Return ProcessState.EndOfFile
        End If

        Return ProcessState.WaitData
    End Function

    Private Function CheckImportsInstruction(ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                                             ByVal strInstruction As String) As Boolean

        If regImportsDeclaration.IsMatch(strInstruction) Then

            Dim iPos As Integer = GetPosition(regImportsDeclaration, strInstruction, iStartLine)

            Dim split As String() = regImportsDeclaration.Split(strInstruction)
            'Console.WriteLine(iStartLine.ToString + " (" + iPos.ToString + ")->Detect package: " + split(3).Trim())
            'Console.WriteLine("Imports name: " + split(3).Trim())

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("imports")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", "Imports")

            Return True
        End If
        Return False
    End Function

    Private Function CheckPackageInstruction(ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                                             ByVal strInstruction As String) As Boolean

        If regPackageDeclaration.IsMatch(strInstruction) Then

            Dim iPos As Integer = GetPosition(regPackageDeclaration, strInstruction, iStartLine)

            Dim split As String() = regPackageDeclaration.Split(strInstruction)
            'Console.WriteLine(iStartLine.ToString + " (" + iPos.ToString + ")->Detect package: " + split(3).Trim())
            'Console.WriteLine("Package name: " + split(3).Trim())

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("package")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", split(3).Trim())

            Return True
        End If
        Return False
    End Function

    Private Function CheckRegionInstruction(ByRef iStartLine As Integer, ByRef iStopLine As Integer, _
                                            ByVal strInstruction As String) As Boolean
        If regRegionDeclaration.IsMatch(strInstruction) Then

            Dim split As String() = regRegionDeclaration.Split(strInstruction)

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("region")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("name", split(3).Trim())

            Return True
        End If
        Return False
    End Function

    Private Function CheckClassInstruction(ByRef iStartLine As Integer, ByVal strInstruction As String, _
                                           ByRef strStatement As String) As Boolean
        strStatement = ""
        If regClassDeclaration.IsMatch(strInstruction) Then

            Dim iPos As Integer = GetPosition(regClassDeclaration, strInstruction, iStartLine)

            Dim split As String() = regClassDeclaration.Split(strInstruction)
            strStatement = split(3).Trim()
            'Console.WriteLine(iStartLine.ToString + " (" + iPos.ToString + ")->Detect class: " + split(5).Trim())
            'Console.WriteLine(strStatement + " name: " + split(5).Trim())

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("class")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", split(5).Trim())

            Return True
        End If
        Return False
    End Function

    Private Function CheckMemberInstruction(ByRef iStartLine As Integer, ByVal iStopLine As Integer, _
                                            ByVal strInstruction As String, ByRef strStatement As String, _
                                            ByVal bInterface As Boolean) As ClassMember

        ' The order of call regex is made to avoid complicated string
        If regPropertyDeclaration.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regPropertyDeclaration, strInstruction, iStartLine)

            Dim split As String() = regPropertyDeclaration.Split(strInstruction)
            'Console.WriteLine(iStartLine.ToString + "-" + iStopLine.ToString + " (" + iPos.ToString + ")->Property: " + strInstruction)
            'Console.WriteLine("Property name: " + split(4).Trim())

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("property")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", split(4).Trim())
            m_textWriter.WriteAttributeString("type", split(9).Trim() + split(11).Trim())

            Return ClassMember.PropertyElt

        ElseIf regTypedefDeclaration.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regTypedefDeclaration, strInstruction, iStartLine)

            Dim split As String() = regTypedefDeclaration.Split(strInstruction)
            'Console.WriteLine(iStartLine.ToString + "-" + iStopLine.ToString + " (" + iPos.ToString + ")->Typedef: " + strInstruction)
            'Console.WriteLine("Typedef name: " + split(5).Trim())

            strStatement = split(3).Trim()

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("typedef")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", split(5).Trim())

            Return ClassMember.TypedefElt

        ElseIf bInterface Then
            If regAbstractMethod.IsMatch(strInstruction) Then

                Dim iPos = GetPosition(regAbstractMethod, strInstruction, iStartLine)

                Dim split As String() = regAbstractMethod.Split(strInstruction)
                'Console.WriteLine(iStartLine.ToString + "-" + iStopLine.ToString + " (" + iPos.ToString + ")->Method: " + strInstruction)
                'Console.WriteLine("Method name: " + split(3).Trim())

                m_textWriter.Flush()
                m_textWriter.WriteString(vbCrLf)
                m_textWriter.WriteStartElement("method")
                m_textWriter.WriteAttributeString("checked", "False")
                m_textWriter.WriteAttributeString("start", iStartLine.ToString)
                m_textWriter.WriteAttributeString("end", iStopLine.ToString)
                m_textWriter.WriteAttributeString("pos", iPos.ToString)
                m_textWriter.WriteAttributeString("name", split(3).Trim())

                CheckParamsInstruction(split(4))

                m_textWriter.WriteEndElement()

                Return ClassMember.AbstractMethod

            ElseIf CheckClassInstruction(iStartLine, strInstruction, strStatement) Then

                Return ClassMember.NestedClass
            End If
        ElseIf regMethodDeclaration.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regMethodDeclaration, strInstruction, iStartLine)

            Dim split As String() = regMethodDeclaration.Split(strInstruction)
            'Console.WriteLine(iStartLine.ToString + "-" + iStopLine.ToString + " (" + iPos.ToString + ")->Method: " + strInstruction)
            'Console.WriteLine("Method name: " + split(7).Trim())
            strStatement = split(5).Trim()

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("method")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", split(7).Trim())

            If split(5).Trim() = cstEvent Then
                m_textWriter.WriteEndElement()
                Return ClassMember.AbstractMethod

            ElseIf split(4).Trim() = cstMustOverride Then
                m_textWriter.WriteEndElement()
                Return ClassMember.AbstractMethod
            End If
            CheckParamsInstruction(split(9))
            Return ClassMember.ImplementedMethod

        ElseIf regOperatorDeclaration.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regOperatorDeclaration, strInstruction, iStartLine)

            Dim split As String() = regOperatorDeclaration.Split(strInstruction)
            'Console.WriteLine(iStartLine.ToString + "-" + iStopLine.ToString + " (" + iPos.ToString + ")->Method: " + strInstruction)
            'Console.WriteLine("Operator name: " + split(5).Trim())
            strStatement = "Operator"

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("method")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", split(5).Trim())

            Return ClassMember.ImplementedMethod

        ElseIf regAttributeDeclaration.IsMatch(strInstruction) Then

            Dim iPos = GetPosition(regAttributeDeclaration, strInstruction, iStartLine)

            Dim split As String() = regAttributeDeclaration.Split(strInstruction)
            'Console.WriteLine(iStartLine.ToString + "-" + iStopLine.ToString + " (" + iPos.ToString + ")->Attribute: " + strInstruction)
            'Console.WriteLine("Attribute name: " + split(3).Trim())

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("attribute")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)
            m_textWriter.WriteAttributeString("name", split(3).Trim())
            m_textWriter.WriteAttributeString("size", split(5).Trim())
            m_textWriter.WriteAttributeString("type", split(8).Trim())
            m_textWriter.WriteEndElement()

            Return ClassMember.AttributeElt

        ElseIf CheckClassInstruction(iStartLine, strInstruction, strStatement) Then

            Return ClassMember.NestedClass
        End If

        Return ClassMember.UnknownElt
    End Function

    Private Sub CheckParamsInstruction(ByVal strListParams As String)
        Dim listParams As MatchCollection = regParamDeclaration.Matches(strListParams)
        Dim strParams As String = ""
        Dim strTypes As String = ""
        For Each param As Match In listParams
            If strParams.Length > 0 Then
                strParams += ", "
                strTypes += ", "
            End If
            strParams += param.Groups(3).ToString + param.Groups(5).ToString
            strTypes += param.Groups(8).ToString
        Next
        m_textWriter.WriteAttributeString("params", strParams)
        m_textWriter.WriteAttributeString("types", strTypes)
    End Sub

    Private Function CheckGetSetInstruction(ByVal iStartLine As Integer, ByVal iStopLine As Integer, _
                                            ByVal strInstruction As String) As ClassMember
        If regAccessorGetDeclaration.IsMatch(strInstruction) Then

            Dim iPos = InStr(strInstruction, "Get")
            'Console.WriteLine(iStartLine.ToString + "-" + iStopLine.ToString + " (" + iPos.ToString + ")->Method Get" + strInstruction)

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("get")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)

            Return ClassMember.GetterElt

        ElseIf regAccessorSetDeclaration.IsMatch(strInstruction) Then

            Dim iPos = InStr(strInstruction, "Set")
            'Console.WriteLine(iStartLine.ToString + "-" + iStopLine.ToString + " (" + iPos.ToString + ")->Method Set: " + strInstruction)

            m_textWriter.Flush()
            m_textWriter.WriteString(vbCrLf)
            m_textWriter.WriteStartElement("set")
            m_textWriter.WriteAttributeString("checked", "False")
            m_textWriter.WriteAttributeString("start", iStartLine.ToString)
            m_textWriter.WriteAttributeString("end", iStopLine.ToString)
            m_textWriter.WriteAttributeString("pos", iPos.ToString)

            Return ClassMember.SetterElt
        End If

        Return ClassMember.UnknownElt
    End Function

    Private Function CheckComment(ByRef strReadLine As String, Optional ByVal bCheckComment As Boolean = True) As Boolean

        Dim iPos As Integer = InStr(strReadLine, "'''")

        If iPos = 0 Then
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

            m_textWriter.Flush()

            If bCheckComment Then
                m_textWriter.WriteString(vbCrLf)
                m_textWriter.WriteStartElement("vb-doc")
                m_textWriter.WriteAttributeString("checked", "False")
                m_textWriter.WriteAttributeString("start", m_iNbLine.ToString)
                m_textWriter.WriteAttributeString("pos", iPos.ToString)
                m_textWriter.WriteString(strReadLine)
            End If

            Do
                m_iNbLine += 1
                'm_listLines.Add(m_iNbLine, m_streamReader.BaseStream.Position)
                strReadLine = m_streamReader.ReadLine()
                'Debug.Print(m_iNbLine.ToString + "-" + strReadLine)

                If strReadLine IsNot Nothing Then
                    If InStr(strReadLine, "'''") = 0 Then
                        m_textWriter.Flush()

                        If bCheckComment Then
                            m_textWriter.WriteElementString("end-vb-doc", CStr(m_iNbLine - 1))
                            m_textWriter.WriteString(vbCrLf)
                            m_textWriter.WriteEndElement()
                        End If

                        Return CheckComment(strReadLine, bCheckComment)
                    Else
                        ''Console.WriteLine("VB-Doc: " + strReadLine)
                        If bCheckComment Then m_textWriter.WriteString(vbCrLf + strReadLine)
                    End If
                End If
            Loop Until strReadLine Is Nothing

            If bCheckComment Then m_textWriter.WriteEndElement()

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
