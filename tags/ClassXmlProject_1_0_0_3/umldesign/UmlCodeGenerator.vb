﻿Imports System
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic

Public Class UmlCodeGenerator

#Region "Class declarations"

    Private Const cstFolderElement As String = "package"
    Private Const cstFileElement As String = "code"
    Private Const cstUpdate As String = "_codegen"
    Private Const cstCodeSourceHeaderCppStyleSheet As String = "uml2cpp-h.xsl"
    Private Const cstCodeSourceVbDotNetStyleSheet As String = "uml2vbnet.xsl"

    Private Shared m_bTransformActive As Boolean = False
#End Region

#Region "Public shared methods"

    Public Shared Function Generate(ByVal fen As System.Windows.Forms.Form, ByVal node As XmlNode, ByVal strClassId As String, _
                               ByVal strPackageId As String, ByVal eLanguage As ELanguage, _
                               ByVal strProgramFolder As String, ByRef strTransformation As String) As Boolean

        Dim bResult As Boolean = False

        Try
            If Right(strProgramFolder, 1) <> "\" Then
                strProgramFolder = strProgramFolder + "\"
            End If
            Select Case eLanguage
                Case eLanguage.Language_CplusPlus
                    If GenerateSourceHeader(fen, node.OwnerDocument.DocumentElement, _
                                            strClassId, strPackageId, strProgramFolder, _
                                            strTransformation) _
                    Then
                        bResult = True
                    End If
                Case eLanguage.Language_Vbasic
                    If GenerateClassModule(fen, node.OwnerDocument.DocumentElement, _
                                            strClassId, strPackageId, strProgramFolder, _
                                            strTransformation) _
                    Then
                        bResult = True
                    End If
                Case Else
                    Throw New Exception("Macro for " + eLanguage.ToString + " not yet implemented")
            End Select
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function
#End Region

#Region "Private shared methods"

    Private Shared Function GenerateSourceHeader(ByVal fen As System.Windows.Forms.Form, ByVal node As XmlNode, ByVal strClassId As String, _
                                                ByVal strPackageId As String, ByVal strPath As String, _
                                                ByRef strTransformation As String) As Boolean
        Dim bResult As Boolean = False
        Dim oldCursor As Cursor = fen.Cursor
        fen.Cursor = Cursors.WaitCursor
        Try
            If m_bTransformActive Then Return bResult

            m_bTransformActive = True

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim strStyleSheet As String = My.Computer.FileSystem.CombinePath(strUmlFolder, cstCodeSourceHeaderCppStyleSheet)
            strTransformation = My.Computer.FileSystem.CombinePath(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData, _
                                                                   cstUpdate + ".xml")

            Dim argList As New Dictionary(Of String, String)
            argList.Add("UmlFolder", strUmlFolder + "\")
            argList.Add("InputClass", strClassId)
            argList.Add("InputPackage", strPackageId)

            If node IsNot Nothing Then
                Dim xslStylesheet As XslSimpleTransform = New XslSimpleTransform(True)
                xslStylesheet.Load(strStyleSheet)
                xslStylesheet.Transform(node, strTransformation, argList)

                ExtractCode(strTransformation, strPath)
                bResult = True
            End If

            fen.Cursor = oldCursor

        Catch ex As Exception
            fen.Cursor = oldCursor
            MsgExceptionBox(ex)
        End Try

        m_bTransformActive = False

        Return bResult
    End Function

    Private Shared Function GenerateClassModule(ByVal fen As System.Windows.Forms.Form, ByVal node As XmlNode, ByVal strClassId As String, _
                                                ByVal strPackageId As String, ByVal strPath As String, _
                                                ByRef strTransformation As String) As Boolean
        Dim bResult As Boolean = False
        Dim oldCursor As Cursor = fen.Cursor
        fen.Cursor = Cursors.WaitCursor
        Try
            If m_bTransformActive Then Return bResult

            m_bTransformActive = True

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim strStyleSheet As String = My.Computer.FileSystem.CombinePath(strUmlFolder, cstCodeSourceVbDotNetStyleSheet)
            strTransformation = My.Computer.FileSystem.CombinePath(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData, _
                                                                   cstUpdate + ".xml")

            Dim argList As New Dictionary(Of String, String)
            argList.Add("UmlFolder", strUmlFolder + "\")
            argList.Add("InputClass", strClassId)
            argList.Add("InputPackage", strPackageId)

            If node IsNot Nothing Then
                Dim xslStylesheet As XslSimpleTransform = New XslSimpleTransform(True)
                xslStylesheet.Load(strStyleSheet)
                xslStylesheet.Transform(node, strTransformation, argList)

                ExtractCode(strTransformation, strPath)
                bResult = True
            End If

            fen.Cursor = oldCursor

        Catch ex As Exception
            fen.Cursor = oldCursor
            MsgExceptionBox(ex)
        End Try

        m_bTransformActive = False

        Return bResult
    End Function

    Private Shared Sub ExtractCode(ByRef strTransformation As String, ByVal strFolder As String)
        Dim reader As XmlTextReader = Nothing
        Try
            ' Load the reader with the data file and ignore all white space nodes.         
            reader = New XmlTextReader(strTransformation)
            reader.WhitespaceHandling = WhitespaceHandling.None

            ' Parse the file and display each of the nodes.
            While reader.Read()
                Select Case reader.NodeType
                    Case XmlNodeType.Element
                        Select Case reader.Name
                            Case cstFolderElement
                                ExtractPackage(strFolder, reader)

                            Case cstFileElement
                                ExtractClass(strFolder, reader)
                            Case Else
                                'Debug.Print("Node ignored:=" + reader.Name)
                        End Select
                    Case XmlNodeType.EndElement
                        'Debug.Print("End Node:=" + reader.Name)
                End Select
            End While
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try
    End Sub

    Private Shared Sub ExtractPackage(ByVal currentFolder As String, ByVal reader As XmlTextReader)

        reader.MoveToFirstAttribute()
        If reader.Name <> "name" Then
            Exit Sub
        End If

        Dim strNewFolder As String = reader.Value
        reader.MoveToElement()

        CreateBranch(currentFolder, strNewFolder)

        While reader.Read()
            Select Case reader.NodeType
                Case XmlNodeType.Element
                    Select Case reader.Name
                        Case cstFolderElement
                            ExtractPackage(currentFolder + strNewFolder + "\", reader)

                        Case cstFileElement
                            ExtractClass(currentFolder + strNewFolder + "\", reader)
                        Case Else
                            'Debug.Print("Node ignored:=" + reader.Name)
                    End Select
                Case XmlNodeType.EndElement
                    'Debug.Print("End Node:=" + reader.Name)
                    If reader.Name = cstFolderElement Then Exit While
            End Select
        End While
    End Sub

    Private Shared Sub ExtractClass(ByVal currentFolder As String, ByVal reader As XmlTextReader)

        reader.MoveToFirstAttribute()
        If reader.Name <> "name" Then
            Exit Sub
        End If

        Dim strNewFile As String = reader.Value
        reader.MoveToElement()

        'Debug.Print("ExtractClass:=" + currentFolder + strNewFile)

        Using sw As StreamWriter = New StreamWriter(currentFolder + strNewFile)
            While reader.Read()
                Select Case reader.NodeType
                    Case XmlNodeType.Element

                    Case XmlNodeType.CDATA
                        sw.Write(reader.Value)

                    Case XmlNodeType.EndElement
                        If reader.Name = "code" Then
                            sw.Close()
                            Exit While
                        End If
                End Select
            End While
        End Using
    End Sub

    Private Shared Sub CreateBranch(ByVal ExistingPath As String, ByVal NewBranch As String)
        Try

            If My.Computer.FileSystem.DirectoryExists(ExistingPath + NewBranch) = False _
            Then
                My.Computer.FileSystem.CreateDirectory(ExistingPath + NewBranch)
                'Debug.Print("CreateBranch:=" + ExistingPath + NewBranch)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
End Class