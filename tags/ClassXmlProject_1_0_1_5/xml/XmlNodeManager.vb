﻿Imports System
Imports System.Windows.Forms
Imports System.Xml
Imports System.Collections

Public Class XmlNodeManager
    Inherits Object

#Region "Class declarations"

    ' Object member
    Private m_xmlListDocuments As SortedList
    Private m_xmlListViews As SortedList
#End Region

#Region "Singleton pattern"

    ' Class member
    Private Shared instance As XmlNodeManager = Nothing

    Public Shared Function GetInstance() As XmlNodeManager

        If instance Is Nothing Then
            instance = New XmlNodeManager
        End If
        Return instance
    End Function
#End Region

#Region "Prototype pattern"

    Public Function AddView(ByVal strName As String, ByRef component As Object) As Boolean
        Try
            m_xmlListViews.Add(strName, component)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function AddDocument(ByVal strName As String, ByRef component As Object) As Boolean
        Try
            m_xmlListDocuments.Add(strName, component)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetView(ByVal strViewName As String) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            xmlResult = FindView(strViewName)
            If xmlResult Is Nothing Then
                Throw New Exception("Try to get an undefined view : " + strViewName)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Public Function CreateForm(ByVal document As XmlComponent) As Form
        Dim frmResult As Form = Nothing
        Try
            If document Is Nothing Then
                Throw New Exception("Document must not be null")
            End If
            Dim view As XmlComponent = GetView(document.Node.Name)
            frmResult = CType(view, InterfViewForm).CreateForm(document)

        Catch ex As Exception
            Throw ex
        End Try
        Return frmResult
    End Function

    Public Overloads Function CreateView(ByVal strViewName As String, Optional ByVal docXml As XmlDocument = Nothing) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            xmlResult = FindView(strViewName)
            If xmlResult IsNot Nothing Then
                xmlResult = xmlResult.Clone(Nothing)
                xmlResult.Document = docXml
            Else
                Throw New Exception("Try to create an undefined view : " + strViewName)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Public Overloads Function CreateView(ByVal xmlNode As XmlNode, ByVal strViewName As String, Optional ByVal docXml As XmlDocument = Nothing) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            xmlResult = FindView(strViewName)
            If xmlResult IsNot Nothing Then
                xmlResult = xmlResult.Clone(xmlNode)
                xmlResult.Document = docXml
            Else
                Throw New Exception("Try to create an undefined view : " + strViewName)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Public Overloads Function CreateDocument(ByVal strNodeName As String, Optional ByVal docXml As XmlDocument = Nothing, Optional ByVal bLoadChildren As Boolean = False) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            xmlResult = FindDocument(strNodeName)

            If xmlResult Is Nothing Then
                Throw New Exception("Try to create an undefined document : " + strNodeName)
            Else
                xmlResult = xmlResult.Clone(Nothing, bLoadChildren)
                xmlResult.Document = docXml
                xmlResult.Node = xmlResult.CreateNode(strNodeName)
            End If

            xmlResult.SetDefaultValues(True)

        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Public Overloads Function CreateDocument(ByVal xmlNode As XmlNode, Optional ByVal bLoadChildren As Boolean = False) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            If xmlNode IsNot Nothing Then

                xmlResult = FindDocument(xmlNode.Name)

                If xmlResult Is Nothing Then
                    Throw New Exception("Try to create an undefined document : " + xmlNode.Name)
                Else
                    xmlResult = xmlResult.Clone(xmlNode, bLoadChildren)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Private Function FindView(ByVal strName As String) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            If m_xmlListViews.Contains(strName) Then
                xmlResult = m_xmlListViews.Item(strName)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function

    Private Function FindDocument(ByVal strName As String) As XmlComponent
        Dim xmlResult As XmlComponent = Nothing
        Try
            If m_xmlListDocuments.Contains(strName) Then
                xmlResult = m_xmlListDocuments.Item(strName)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return xmlResult
    End Function
#End Region

    Public Sub New()
        m_xmlListDocuments = New SortedList
        m_xmlListViews = New SortedList
    End Sub

    Protected Overrides Sub Finalize()
        ' To break circular references
        m_xmlListDocuments = Nothing
        m_xmlListViews = Nothing
        MyBase.Finalize()
    End Sub

    Public Shared Sub Destroy()
        instance = Nothing
    End Sub
End Class
