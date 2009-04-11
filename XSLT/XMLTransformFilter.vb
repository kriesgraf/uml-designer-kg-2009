Imports System.Xml
Imports System.Xml.Xsl
Imports System.Xml.Schema
Imports System.IO
Imports System.Collections.Generic
Imports System.Text

Friend Class XMLTransformFilter
    Inherits Form

#Region "Class declarations"

    Private Enum eOuputFormat
        NONE
        HTML
        XML
        TEXT
    End Enum

    Private m_styleXSL As New XslSimpleTransform(True)

    Private m_strFileXSL As String
    Private m_strFileXML As String
    Private m_strFolderXML As String

    Private m_dicoParamList As New Dictionary(Of String, String)

    Private m_strFileName As String
    Private m_strMedia As String
    Private m_strPath As String

    Private m_bXmlSelected As Boolean
    Private m_bSauverVerrou As Boolean
    Private m_bXSLChange As Boolean
    Private m_bInitializeComponent As Boolean = False
    Private m_bValidTransform As Boolean

    Private m_eType As eOuputFormat

    Private Const cstElmtFinal As String = "FINAL"
    Private Const cstVersionMethodesXSL As String = "1.0"
    Private Const cstAttSource As String = "src"
    Private Const cstElmtParamTransformation As String = "PARAM"
    Private Const cstNameParam As String = "name"
    Private Const cstParamValue As String = "value"
    Private Const cstMaxTempoFile As Short = 20
    Private Const cstFormatTempoFile As String = "000"
    Private Const cstPrefixTempoFile As String = "XFER-TMP"

    Private Const cstDefaultFile As String = "__webBrowser"
    Private Const cstXMLFilter As String = "eXtended Markup Language (*.xml)|*.xml|eXtended Style Sheet (*.xsl)|*.xsl|XML Schema Definitions (*.xsd)|*.xsd"
    Private Const cstHTMLFilter As String = "HTML Language (*.html)|*.html"
#End Region

#Region "Methods"

    Private Function LoadParams() As Boolean
        Dim bNodes As Boolean = False
        Try
            Dim doc As New XmlDocument
            Dim node As XmlNode

            doc.Load(m_strFileXSL)

            Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform")

            If CompareParams(doc, nsmgr, bNodes) = False Then

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
                        m_bValidTransform = True
                        m_eType = eOuputFormat.XML

                        node = doc.SelectSingleNode("//xsl:output/@media-type", nsmgr)

                        If Not node Is Nothing Then
                            m_strMedia = "." + node.Value
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
                        Else
                            m_strMedia = ".txt"
                        End If
                        m_eType = eOuputFormat.TEXT
                End Select
            Else
                m_eType = eOuputFormat.XML
                m_strMedia = ".xml"
            End If


        Catch ex As Exception
            Throw ex
        End Try
        Return bNodes
    End Function

    Private Function CompareParams(ByVal doc As XmlDocument, ByVal nsmgr As XmlNamespaceManager, ByRef bNodes As Boolean) As Boolean
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

    Private Sub Transformer()
        If Len(LabelXSL.Text) = 0 Or Len(LabelXML.Text) = 0 Then Exit Sub
        If m_bXmlSelected = False Then Exit Sub

        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            If m_bXSLChange Then
                LoadXSL()
                'If cmdParams.Enabled = True Then
                '    cmdParams_Click(Me, Nothing)
                'End If
                m_bXSLChange = False
            End If

            Dim strFullPathFilename As String = My.Computer.FileSystem.CombinePath(m_strPath, m_strFileName + m_strMedia)
            Dim doc As New XmlDocument
            doc.Load(m_strFileXML)
            m_styleXSL.Transform(doc.DocumentElement, strFullPathFilename, m_dicoParamList)

            Afficher()
            cmdSave.Enabled = True

        Catch ex As Exception
            Throw ex
        Finally
            Me.Cursor = oldCursor
        End Try
    End Sub

    Private Sub Sauver()
        Try
            'If m_bSauverVerrou Then Exit Sub

            AfficherSauvegarde()

            Dim strFullPathFilename As String = My.Computer.FileSystem.CombinePath(m_strPath, m_strFileName + m_strMedia)
            WebBrowser.Navigate(New System.Uri(strFullPathFilename))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub SauvegarderSous(ByRef StrCompleteFileName As String)
        Try
            Dim tempo As String
            Dim i As Integer

            tempo = RetirerChemin(StrCompleteFileName)

            i = InStr(tempo, ".")

            If i > 0 Then
                m_strFileName = Strings.Left(tempo, i - 1)
            Else
                m_strFileName = tempo
            End If

            i = InStr(StrCompleteFileName, m_strFileName)

            If i > 0 Then
                m_strPath = Strings.Left(StrCompleteFileName, i - 1)
            Else
                m_strPath = ""
            End If
            AfficherSauvegarde()
            Transformer()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub LoadXSL()
        Try
            If Len(LabelXSL.Text) = 0 Then
                MsgBox("Veuillez sélectionner un fichier XSL", MsgBoxStyle.Critical)
            Else
                m_styleXSL.Load(m_strFileXSL)
                cmdParams.Enabled = LoadParams()
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub Afficher()
        Try
            Dim doc As New XmlDocument
            If m_strFileName = "" Then
                Throw New Exception("Filename is empty")
            End If

            Dim strFullPathFilename As String = My.Computer.FileSystem.CombinePath(m_strPath, m_strFileName + m_strMedia)
            TextEditBox.LoadFile(strFullPathFilename, RichTextBoxStreamType.PlainText)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AfficherSauvegarde()
        lblTransform.Text = CompactPath(lblTransform, m_strPath + m_strFileName + m_strMedia)
    End Sub

    Private Function CompactPath(ByVal control As Control, ByVal strFullPathFilename As String) As String
        Dim strTempo As New StringBuilder(strFullPathFilename)
        shlwapi.PathCompactPath(user32.GetWindowDC(control.Handle), strTempo, control.ClientSize.Width)
        Return strTempo.ToString
    End Function

    Private Function RetirerChemin(ByRef strFullPathName As String) As String
        Dim strResult As String
        Try
            strResult = My.Computer.FileSystem.GetName(strFullPathName)

        Catch ex As Exception
            Throw ex
        End Try
        Return (strResult)
    End Function

#End Region

#Region "Events"

    Private Sub chkSynchronize_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkSynchronize.CheckedChanged
        Try
            '            If m_bInitializeComponent Then Exit Sub

            If chkSynchronize.Checked Then
                DriveXSL.Enabled = False
                DirXSL.Enabled = False
                DirXSL.Path = DirXML.Path
                SplitFrameXSL.IsSplitterFixed = True
                SplitFrameXSL.SplitterDistance = 0
            Else
                DriveXSL.Enabled = True
                DirXSL.Enabled = True
                SplitFrameXSL.IsSplitterFixed = False
                SplitFrameXSL.SplitterDistance = SplitFrameXSL.Height / 2
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNew.Click
        Try
            m_strPath = Application.StartupPath + "\"
            m_strFileName = cstDefaultFile

            Sauver()

            cmdNew.Enabled = False
            cmdSave.Enabled = True

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub cmdParams_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdParams.Click
        Try
            Dim fen As New dlgParams
            With fen
                .ParamList = m_dicoParamList
                .CurrentFolder = DirXSL.Path
                .XmlFolder = DirXML.Path
                .ShowDialog()
            End With

            Call Transformer()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub cmdRechargerXML_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRechargerXML.Click
        Try
            Call Transformer()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub cmdRechargerXSL_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRechargerXSL.Click
        Try
            m_bXSLChange = True
            Call Transformer()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
        Try
            Dim SaveDialogBoxSave As New SaveFileDialog
            SaveDialogBoxSave.Title = "Enregistrer sous..."
            SaveDialogBoxSave.CheckPathExists = True

            Select Case m_eType
                Case eOuputFormat.XML
                    SaveDialogBoxSave.Filter = cstXMLFilter
                Case eOuputFormat.HTML
                    SaveDialogBoxSave.Filter = cstHTMLFilter
                Case Else
                    SaveDialogBoxSave.Filter = "Specific Languages (*" & m_strMedia & ")|*" & m_strMedia
            End Select

            SaveDialogBoxSave.InitialDirectory = FileXSL.Path

            If SaveDialogBoxSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
                SauvegarderSous(SaveDialogBoxSave.FileName)
                cmdNew.Enabled = True
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub DirXML_Change(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles DirXML.Change
        Try
            If m_bInitializeComponent = False Then
                My.Settings.CurrentFolderXML = DirXML.Path
            End If
            FileXML.Path = DirXML.Path
            m_strFolderXML = DirXML.Path

            If chkSynchronize.CheckState Then
                DirXSL.Path = DirXML.Path
            End If

            cmdRechargerXML.Enabled = False
            cmdRechargerXSL.Enabled = False
            TextEditBox.Text = ""
            cmdSave.Enabled = False

            cmdParams.Enabled = False

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub DirXSL_Change(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles DirXSL.Change
        Try
            If m_bInitializeComponent = False Then
                My.Settings.CurrentFolderXSL = DirXSL.Path
            End If
            FileXSL.Path = DirXSL.Path

            cmdRechargerXML.Enabled = False
            cmdRechargerXSL.Enabled = False
            m_bSauverVerrou = True
            TextEditBox.Text = ""
            m_bSauverVerrou = False
            cmdSave.Enabled = False

            cmdParams.Enabled = False

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub DriveXML_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles DriveXML.SelectedIndexChanged
        Try
            DirXML.Path = Strings.Left(DriveXML.Drive, 2) + "\"

            If chkSynchronize.Checked Then
                DriveXSL.Drive = DriveXML.Drive
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub DriveXSL_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles DriveXSL.SelectedIndexChanged
        Try
            DirXSL.Path = Strings.Left(DriveXSL.Drive, 2) & "\"

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub FileXML_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles FileXML.SelectedIndexChanged
        Try
            Dim tempo As String

            m_eType = eOuputFormat.NONE

            If FileXML.SelectedIndex > -1 Then
                tempo = My.Computer.FileSystem.CombinePath(FileXML.Path, FileXML.Items(FileXML.SelectedIndex))

                If m_strFileXML <> tempo Then

                    m_strFileXML = tempo
                    m_bXmlSelected = True

                    LabelXML.Text = CompactPath(LabelXML, m_strFileXML)
                    cmdRechargerXML.Enabled = True
                    cmdRechargerXSL.Enabled = True

                    If FileXSL.SelectedIndex > -1 Then
                        Transformer()
                    End If
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub FileXSL_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles FileXSL.SelectedIndexChanged
        Try
            Dim tempo As String

            If FileXSL.SelectedIndex > -1 Then

                tempo = My.Computer.FileSystem.CombinePath(FileXSL.Path, FileXSL.Items(FileXSL.SelectedIndex))

                If m_strFileXSL <> tempo Then

                    m_strFileXSL = tempo
                    m_bXSLChange = True

                    LabelXSL.Text = CompactPath(LabelXSL, m_strFileXSL)
                    cmdRechargerXML.Enabled = True
                    cmdRechargerXSL.Enabled = True
                    Transformer()
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub XMLTransformFilter_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Try
            cmdNew.Enabled = False
            cmdSave.Enabled = False

            If Deployment.Application.ApplicationDeployment.IsNetworkDeployed Then
                Me.Text = "XSL Transform - " + My.Application.Deployment.CurrentVersion.ToString
            Else
                Me.Text = "XSL Transform - Version not published"
            End If
            txtXMLFilter.Text = My.Settings.CurrentExtension

            m_strMedia = ".xml"

            m_bInitializeComponent = True

            Dim tempo As String = My.Settings.CurrentFolderXML
            DriveXML.Drive = Strings.Left(tempo, 2)
            DirXML.Path = tempo

            tempo = My.Settings.CurrentFolderXSL
            DriveXSL.Drive = Strings.Left(tempo, 2)
            DirXSL.Path = tempo

            If DirXML.Path = DirXSL.Path Then
                chkSynchronize.Checked = True
            End If

            m_bInitializeComponent = False

            m_strFolderXML = tempo

            m_bXmlSelected = False
            m_bXSLChange = False

            m_bSauverVerrou = False

            cmdNew_Click(eventSender, eventArgs)

            tempo = Command()

            If Len(tempo) > 0 Then

                Dim i As Integer = InStr(tempo, ".xml")

                If i > 0 Then

                    m_strFileXML = Trim(Strings.Left(tempo, i + 4))
                    m_strFileXSL = Trim(Mid(tempo, i + 5))

                    If InStr(m_strFileXML, "\") = 0 Then
                        LabelXML.Text = m_strFileXML

                        m_strFileXML = My.Computer.FileSystem.CombinePath(Application.StartupPath, m_strFileXML)

                    ElseIf InStr(m_strFileXML, "..") > 0 Then
                        LabelXML.Text = RetirerChemin(m_strFileXML)
                        m_strFileXML = My.Computer.FileSystem.CombinePath(Application.StartupPath, m_strFileXML)
                    Else
                        LabelXML.Text = RetirerChemin(m_strFileXML)
                    End If

                    If InStr(m_strFileXSL, "\") = 0 Then
                        LabelXSL.Text = m_strFileXSL
                        m_strFileXSL = My.Computer.FileSystem.CombinePath(Application.StartupPath, m_strFileXSL)

                    ElseIf InStr(m_strFileXSL, "..") > 0 Then
                        LabelXML.Text = RetirerChemin(m_strFileXSL)
                        m_strFileXSL = My.Computer.FileSystem.CombinePath(Application.StartupPath, m_strFileXSL)
                    Else
                        LabelXML.Text = RetirerChemin(m_strFileXSL)
                    End If


                    m_bXmlSelected = True
                    m_bXSLChange = True

                    Transformer()

                ElseIf Len(tempo) > 0 Then

                    m_strFileXML = tempo

                    If InStr(m_strFileXML, "\") = 0 Then
                        m_strFileXML = My.Computer.FileSystem.CombinePath(Application.StartupPath, m_strFileXML)
                    End If

                    m_bXmlSelected = True

                    Transformer()
                End If

            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub txtXMLFilter_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtXMLFilter.TextChanged
        FileXML.Pattern = txtXMLFilter.Text
        If m_bInitializeComponent = False Then
            My.Settings.CurrentExtension = txtXMLFilter.Text
        End If
    End Sub

    Private Sub txtMaskXSL_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaskXSL.TextChanged
        FileXSL.Pattern = txtMaskXSL.Text
        If m_bInitializeComponent = False Then
            '            My.Settings.CurrentExtension = txtMaskXSL.Text
        End If
    End Sub

    Private Sub TextEditBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextEditBox.TextChanged
        Try
            If Me.m_bInitializeComponent Then Exit Sub
            'If m_bSauverVerrou = True Then Exit Sub

            Sauver()

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub LabelXML_ClientSizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelXML.ClientSizeChanged
        LabelXML.Text = CompactPath(LabelXML, m_strFileXML)
    End Sub

    Private Sub LabelXSL_ClientSizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LabelXSL.ClientSizeChanged
        LabelXSL.Text = CompactPath(LabelXSL, m_strFileXSL)
    End Sub

    Private Sub lblTransform_ClientSizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblTransform.ClientSizeChanged
        lblTransform.Text = CompactPath(LabelXSL, m_strPath + m_strFileName + m_strMedia)
    End Sub

#End Region

    Public Class shlwapi
        <System.Runtime.InteropServices.DllImport("shlwapi.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
        Shared Function PathCompactPath(ByVal hDC As IntPtr, ByVal lpszPath As StringBuilder, ByVal dx As Integer) As Boolean
        End Function
    End Class

    Public Class user32
        <System.Runtime.InteropServices.DllImport("user32")> _
        Shared Function GetWindowDC(ByVal hWnd As IntPtr) As IntPtr
        End Function
    End Class

    Private Sub cmdValidate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdValidate.Click
        Try
            LoadDocument(My.Computer.FileSystem.CombinePath(m_strPath, m_strFileName + m_strMedia))

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub LoadDocument(ByVal strFilename As String)
        Try
            ' the following to Validate succeeds.
            Dim settings As XmlReaderSettings = New XmlReaderSettings()
            settings.ProhibitDtd = False
            settings.ValidationType = System.Xml.ValidationType.DTD

            ' Using avoid to remain the file locked for another process
            Dim strDTD As String
            Dim document As New XmlDocument
            Using reader As XmlReader = XmlReader.Create(strFilename, settings)
                document.Load(reader)
                strDTD = document.DocumentType.SystemId
            End Using

            MsgBox("Validation complete with DTD:" + vbCrLf + vbCrLf + strDTD, MsgBoxStyle.Information)

        Catch ex As XmlSchemaException

            Dim lines As String() = TextEditBox.Lines()
            Dim location As String = ""

            If ex.LineNumber - 2 > -1 Then
                location += vbCrLf + "l" + CStr(ex.LineNumber - 1) + "=" + lines(ex.LineNumber - 2)
            End If
            If ex.LineNumber - 1 > 0 And ex.LineNumber - 1 < lines.Length Then
                location += vbCrLf + "l" + CStr(ex.LineNumber) + "=" + lines(ex.LineNumber - 1)
            End If

            If ex.LineNumber < lines.Length Then
                location += vbCrLf + "l" + CStr(ex.LineNumber + 1) + "=" + lines(ex.LineNumber)
            End If
            Dim message As String = "LineNumber=" + ex.LineNumber.ToString + vbCrLf + _
                                "LinePosition=" + ex.LinePosition.ToString + vbCrLf + _
                                "Message=" + ex.Message + vbCrLf + _
                                "SourceUri=" + ex.SourceUri + vbCrLf + vbCrLf + location 
            Throw New Exception(message, ex)
        End Try
    End Sub
End Class