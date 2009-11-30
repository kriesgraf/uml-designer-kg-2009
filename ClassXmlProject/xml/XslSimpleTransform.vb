Imports System
Imports System.Xml
Imports System.Net
Imports System.IO
Imports System.Xml.Xsl
Imports System.Collections.Generic
Imports System.Xml.XPath
Imports Microsoft.VisualBasic

Public Class XslSimpleTransform

    Public Class Arguments
        Inherits Dictionary(Of String, String)
    End Class

    Private m_oStylesheet As XslCompiledTransform
    Private m_oResolver As XmlResolver

    ''' <summary>
    ''' Loads and compiles the style sheet located at the specified URI. 
    ''' </summary>
    ''' <param name="stylesheetUri">The URI of the style sheet. </param>
    ''' <param name="bRedirectException">Ask to encapsulate exception inside new one. </param>
    ''' <param name="resolver">The XmlResolver used to resolve specific style sheet URI. This argument is optional.</param>
    ''' <remarks>This class supports the XSLT 1.0/2.0 syntax. The XSLT style sheet must use the http://www.w3.org/1999/XSL/Transform namespace. 
    ''' Supports XSLT import, include elements, the XSLT document() function and embedded script blocks.</remarks>
    Public Sub Load(ByVal stylesheetUri As String, Optional ByVal bRedirectException As Boolean = True, _
                    Optional ByVal resolver As XmlResolver = Nothing)
        Try

            If resolver IsNot Nothing Then
                m_oResolver = resolver
            Else
                m_oResolver = New XmlUrlResolver
                m_oResolver.Credentials = System.Net.CredentialCache.DefaultCredentials
            End If
            m_oStylesheet.Load(stylesheetUri, New XsltSettings(True, True), m_oResolver)


        Catch ex1 As Xsl.XsltCompileException
            If bRedirectException Then
                Throw ex1
            ElseIf ex1.InnerException IsNot Nothing Then
                Throw ex1.InnerException
            Else
                Throw ex1
            End If
        Catch ex2 As Xsl.XsltException
            If bRedirectException Then
                Throw ex2
            ElseIf ex2.InnerException IsNot Nothing Then
                Throw ex2.InnerException
            Else
                Throw ex2
            End If
        Catch ex As Exception
            If bRedirectException Then
                Throw New Exception("Can't load XSL stylesheet: " + vbCrLf + stylesheetUri, ex)
            Else
                Throw ex
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Executes the transform using the input document specified by the URI and outputs the results to a file. 
    ''' </summary>
    ''' <param name="inputUri">The URI of the input document. </param>
    ''' <param name="resultsFile">The URI of the output file. </param>
    ''' <param name="arguments">A Dictionary list containing the namespace-qualified arguments used as input to the transform. This argument is optional. </param>
    ''' <remarks>This method uses a specific XmlUrlResolver with a default user credentials to resolve the input and output 
    ''' documents. An XmlReader with default settings is used to load the input document, the DTD processing is enabled.  
    '''</remarks>
    Public Overloads Sub Transform(ByVal inputUri As String, _
                         ByVal resultsFile As String, _
                         Optional ByVal arguments As Arguments = Nothing)
        Try
            Dim argList As XsltArgumentList = Nothing
            If arguments IsNot Nothing Then
                If arguments.Count > 0 Then
                    argList = New XsltArgumentList

                    For Each dico As KeyValuePair(Of String, String) In arguments
                        argList.AddParam(dico.Key, "", dico.Value)
                    Next dico
                End If
            End If

            Using fo As FileStream = File.Open(resultsFile, FileMode.Create)
                Dim settings As New XmlReaderSettings
                settings.ProhibitDtd = False

                Dim fileXML As XmlReader = XmlReader.Create(inputUri, settings)

                m_oStylesheet.Transform(fileXML, argList, fo)

                fo.Close()
            End Using
        Catch ex As Exception
            Throw New Exception("Can't apply stylesheet to file: " + vbCrLf + inputUri, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Executes the transform using the input document specified by an XmlNode object and outputs the results to a file. 
    ''' </summary>
    ''' <param name="input">An XmlNode object</param>
    ''' <param name="resultsFile">The URI of the output file. </param>
    ''' <param name="arguments">A Dictionary list containing the namespace-qualified arguments used as input to the transform. This argument is optional. </param>
    ''' <remarks>This method uses a specific XmlUrlResolver with a default user credentials to resolve the input and output 
    ''' documents. An XmlReader with default settings is used to load the input document, the DTD processing is enabled. if 
    ''' you pass in a node other than the document root node, this does not prevent the transformation process from accessing all nodes in the loaded document 
    '''</remarks>
    Public Overloads Sub Transform(ByVal input As XmlNode, _
                         ByVal resultsFile As String, _
                         Optional ByVal arguments As Arguments = Nothing)
        Try
            Dim argList As XsltArgumentList = Nothing

            If arguments.Count > 0 Then
                argList = New XsltArgumentList

                For Each dico As KeyValuePair(Of String, String) In arguments
                    argList.AddParam(dico.Key, "", dico.Value)
                Next dico
            End If

            Using fs As New FileStream(resultsFile, FileMode.Create)
                m_oStylesheet.Transform(input.CreateNavigator(), argList, fs)
                fs.Close()
            End Using
        Catch ex1 As XsltException
            If ex1.InnerException IsNot Nothing Then
                Throw ex1.InnerException
            ElseIf ex1.LineNumber > 0 And ex1.LinePosition > 0 Then
                Dim message As String = "LineNumber=" + ex1.LineNumber.ToString + vbCrLf + _
                                    "LinePosition=" + ex1.LinePosition.ToString + vbCrLf + _
                                    "Message=" + ex1.Message + vbCrLf + _
                                    "SourceUri=" + ex1.SourceUri
                Throw New Exception(message, ex1)
            Else
                Throw ex1
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Initializes a new instance with the specified debug setting. 
    ''' </summary>
    ''' <param name="enableDebug">true to generate debug information; otherwise false. Setting this to true enables you to debug the style sheet with the Microsoft Visual Studio Debugger. 
    ''' </param>
    ''' <remarks></remarks>
    Public Sub New(ByVal enableDebug As Boolean)
        m_oStylesheet = New XslCompiledTransform(enableDebug)
    End Sub
End Class

