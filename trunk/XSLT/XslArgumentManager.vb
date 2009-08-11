Imports System
Imports System.Xml
Imports System.Net
Imports System.IO
Imports System.Xml.Xsl
Imports System.Collections.Generic
Imports System.Xml.XPath
Imports Microsoft.VisualBasic

Public Class XslArgumentManager

    Private m_strFileXSL As String

    Private m_dicoParamList As New XslSimpleTransform.Arguments
    Private m_eType As eOuputFormat
    Private m_strMedia As String

    Public Enum eOuputFormat
        NONE
        HTML
        XML
        TEXT
    End Enum

    Public Property FileXSL() As String
        Get
            Return m_strFileXSL
        End Get
        Set(ByVal value As String)
            m_strFileXSL = value
        End Set
    End Property

    Public Property MediaType() As eOuputFormat
        Get
            Return m_eType
        End Get
        Set(ByVal value As eOuputFormat)
            m_eType = value
        End Set
    End Property


    Public ReadOnly Property ParamList() As XslSimpleTransform.Arguments
        Get
            Return m_dicoParamList
        End Get
    End Property

    Public Property Media() As String
        Get
            Return m_strMedia
        End Get
        Set(ByVal value As String)
            m_strMedia = value
        End Set
    End Property

    Public Function LoadParams(Optional ByVal bClear As Boolean = False) As Boolean
        Dim bNodes As Boolean = False

        Dim doc As New XmlDocument
        Dim node As XmlNode

        doc.Load(m_strFileXSL)

        Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
        nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform")

        If CompareParams(doc, nsmgr, bNodes) = False Or bClear Then

            bNodes = False
            m_dicoParamList.Clear()

            For Each node In doc.SelectNodes("//xsl:stylesheet/xsl:param", nsmgr)
                bNodes = True
                m_dicoParamList.Add(node.Attributes.GetNamedItem("name").Value, node.InnerText)
            Next
        End If

        node = doc.SelectSingleNode("//xsl:output/@method", nsmgr)

        ' On regarde si le format de code généré est du xml/text/html
        If Not node Is Nothing Then
            Select Case node.Value
                Case "xml"
                    m_eType = eOuputFormat.XML

                    node = doc.SelectSingleNode("//xsl:output/@media-type", nsmgr)

                    If Not node Is Nothing Then
                        m_strMedia = "." + node.Value
                        m_eType = eOuputFormat.NONE
                    Else
                        m_strMedia = ".xml"
                    End If

                Case "html"
                    m_eType = eOuputFormat.HTML
                    m_strMedia = ".html"

                Case Else
                    node = doc.SelectSingleNode("//xsl:output/@media-type", nsmgr)

                    If Not node Is Nothing Then
                        m_strMedia = "." + node.Value
                        m_eType = eOuputFormat.NONE
                    Else
                        m_strMedia = ".txt"
                    End If
                    m_eType = eOuputFormat.TEXT
            End Select
        Else
            m_eType = eOuputFormat.XML
            m_strMedia = ".xml"
        End If

        Return bNodes
    End Function

    Public Function CompareParams(ByVal doc As XmlDocument, ByVal nsmgr As XmlNamespaceManager, ByRef bNodes As Boolean) As Boolean
        Dim bCompareOk As Boolean = True

        For Each node As XmlNode In doc.SelectNodes("//xsl:stylesheet/xsl:param", nsmgr)
            bNodes = True
            If m_dicoParamList.ContainsKey(node.Attributes.GetNamedItem("name").Value) = False Then
                bCompareOk = False
                Exit For
            End If
        Next

        If bCompareOk Then
            For Each Text As String In m_dicoParamList.Keys
                If doc.SelectNodes("//xsl:stylesheet/xsl:param[@name='" + Text + "']", nsmgr) Is Nothing Then
                    bCompareOk = False
                    Exit For
                End If
            Next
        End If
        Return bCompareOk
    End Function

End Class
