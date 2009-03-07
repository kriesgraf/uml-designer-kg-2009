Imports System
Imports System.Collections.Generic
Imports ClassXmlProject.UmlCodeGenerator
Imports System.Windows.Forms
Imports System.Xml.Serialization
Imports System.IO
Imports System.Xml
Imports Microsoft.Win32

Public Enum XmlDocumentViewMode
    Unknown
    Database
    UmlView
    CodeSource
End Enum

Public Class XmlDocumentView
    Inherits WebBrowser

#Region "Class declarations"

    Private m_xslStylesheet As XslSimpleTransform
    Private m_currentNode As XmlNode
    Private m_eCurrentView As XmlDocumentViewMode = XmlDocumentViewMode.Unknown
    Private m_eLanguage As ELanguage = ELanguage.Language_CplusPlus
    Private m_bTransformActive As Boolean = False
    Private m_strTransformation As String = ""
    Private m_iTextSize As Integer = 100

    Private Const cstUpdate As String = "__umlTempleFile"
    Private Const cstDatabaseStyleSheet As String = "xsl-idle.xsl"
    Private Const cstUmlViewStyleSheet As String = "package.xsl"
    Private Const cstCodeSourceHeaderCppStyleSheet As String = "uml2cpp-h.xsl"
    Private Const cstCodeSourceVbDotNetStyleSheet As String = "uml2vbnet.xsl"

    ' Internet Explorer page setup  is used for printing document
    Private Const cstPrintSetupKey As String = "Software\Microsoft\Internet Explorer\PageSetup"
    Private Const cstPrintSetupHeader As String = "&w&b&d"
    Private Const cstPrintSetupFooter As String = "{0}&bPage &p sur &P"

#End Region

#Region "Properties"

    Public Property Display() As String
        Get
            Return m_strTransformation
        End Get
        Set(ByVal value As String)
            If value <> "" Then
                MyBase.Navigate(New System.Uri(value))
            End If
        End Set
    End Property

    Public Property DataSource() As XmlNode
        Get
            Return m_currentNode
        End Get
        Set(ByVal value As XmlNode)
            If value IsNot Nothing Then
                UpdateXslTransformation(value)
                m_currentNode = value
            End If
        End Set
    End Property

    Public Property Language() As Integer
        Get
            Return m_eLanguage
        End Get
        Set(ByVal value As Integer)
            m_eLanguage = CType(value, ELanguage)
            UpdatXmlDocumentViewMode(m_currentNode, m_eLanguage)
        End Set
    End Property

    Public Property View() As XmlDocumentViewMode
        Get
            Return m_eCurrentView
        End Get
        Set(ByVal value As XmlDocumentViewMode)
            m_eCurrentView = value
            UpdatXmlDocumentViewMode(m_currentNode, m_eLanguage)
        End Set
    End Property

#End Region

#Region "Private method"

    Private Sub UpdatXmlDocumentViewMode(ByVal navXml As XmlNode, ByVal eLanguage As ELanguage)
        Try
            If m_eCurrentView = XmlDocumentViewMode.Unknown Then Exit Sub

            Dim strStyleSheet As String
            Dim strUmlFolder = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)

            Select Case m_eCurrentView
                Case XmlDocumentViewMode.Database
                    strStyleSheet = My.Computer.FileSystem.CombinePath(strUmlFolder, cstDatabaseStyleSheet)

                Case XmlDocumentViewMode.CodeSource
                    Select Case eLanguage
                        Case eLanguage.Language_CplusPlus
                            strStyleSheet = My.Computer.FileSystem.CombinePath(strUmlFolder, cstCodeSourceHeaderCppStyleSheet)
                        Case eLanguage.Language_Vbasic
                            strStyleSheet = My.Computer.FileSystem.CombinePath(strUmlFolder, cstCodeSourceVbDotNetStyleSheet)
                        Case Else
                            strStyleSheet = My.Computer.FileSystem.CombinePath(strUmlFolder, cstDatabaseStyleSheet)
                    End Select

                Case Else
                    strStyleSheet = My.Computer.FileSystem.CombinePath(strUmlFolder, cstUmlViewStyleSheet)
            End Select

            m_xslStylesheet.Load(strStyleSheet)
            UpdateXslTransformation(navXml)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub UpdateXslTransformation(ByVal navXml As XmlNode)
        Try
            If m_bTransformActive Then Exit Sub

            m_bTransformActive = True

            Dim strUmlFolder As String = My.Computer.FileSystem.CombinePath(Application.StartupPath, My.Settings.ToolsFolder)
            Dim strTmpFolder As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData

            Dim argList As New Dictionary(Of String, String)

            Select Case m_eCurrentView
                Case XmlDocumentViewMode.Database
                    m_strTransformation = My.Computer.FileSystem.CombinePath(strTmpFolder, cstUpdate + ".xml")

                Case XmlDocumentViewMode.CodeSource
                    m_strTransformation = My.Computer.FileSystem.CombinePath(strTmpFolder, cstUpdate + ".xml")
                    argList.Add("UmlFolder", strUmlFolder + "\")

                Case Else
                    m_strTransformation = My.Computer.FileSystem.CombinePath(strTmpFolder, cstUpdate + ".html")
                    argList.Add("IconsFolder", strUmlFolder + "\")
            End Select

            If navXml IsNot Nothing Then
                m_xslStylesheet.Transform(navXml, m_strTransformation, argList)

                'Debug.Print(navXml.OuterXml)

                MyBase.Navigate(New System.Uri(m_strTransformation))
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        Finally
            m_bTransformActive = False
        End Try
    End Sub

    Private Sub CheckInternetExplorerPageSetup()
        Dim key As RegistryKey = Registry.CurrentUser.OpenSubKey(cstPrintSetupKey, True)
        Try
            If key IsNot Nothing Then
                key.SetValue("footer", String.Format(cstPrintSetupFooter, m_currentNode.OwnerDocument.BaseURI))
                key.SetValue("header", cstPrintSetupHeader)
            End If

            MyBase.ShowPageSetupDialog()

            If key Is Nothing Then
                key = Registry.CurrentUser.OpenSubKey(cstPrintSetupKey, True)

                If key IsNot Nothing Then
                    key.SetValue("footer", String.Format(cstPrintSetupFooter, m_currentNode.OwnerDocument.BaseURI))
                    key.SetValue("header", cstPrintSetupHeader)
                End If
            End If

        Catch ex As Exception
        Finally
            If key IsNot Nothing Then
                key.Close()
            End If
        End Try
    End Sub
#End Region

#Region "Public methods"

    Public Sub New()
        m_xslStylesheet = New XslSimpleTransform(True)
    End Sub

    Public Overloads Sub ShowPrintPreviewDialog()
        CheckInternetExplorerPageSetup()
        MyBase.ShowPrintPreviewDialog()
    End Sub

    Public Overloads Sub ShowPrintDialog()
        CheckInternetExplorerPageSetup()
        MyBase.ShowPrintDialog()
    End Sub

    Public Sub DicreaseTextSize()
        Try
            If m_iTextSize > 50 Then
                m_iTextSize -= 10
                Dim oWebControl As Object = MyBase.ActiveXInstance
                oWebControl.ExecWB(63, 2, CType(m_iTextSize, Object), Nothing)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub IncreaseTextSize()
        Try
            If m_iTextSize < 200 Then
                m_iTextSize += 10
                Dim oWebControl As Object = MyBase.ActiveXInstance
                oWebControl.ExecWB(63, 2, CType(m_iTextSize, Object), Nothing)
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

    Private Sub XmlDocumentView_Navigated(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles Me.Navigated
        Try
            Dim oWebControl As Object = MyBase.ActiveXInstance
            oWebControl.ExecWB(63, 2, CType(m_iTextSize, Object), Nothing)

        Catch ex As Exception
        End Try
    End Sub
End Class
