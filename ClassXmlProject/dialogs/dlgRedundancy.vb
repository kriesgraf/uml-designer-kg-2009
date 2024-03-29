﻿Imports System.Windows.Forms
Imports System.Xml
Imports System
Imports Microsoft.VisualBasic
Imports ClassXmlProject.XmlNodeListView

Public Class dlgRedundancy

    Public Enum EResult
        RedundancyChanged
        RedundancyIgnored
        RedundancyIgnoredAll
    End Enum

    Private m_xmlView As XmlRefRedundancyView = Nothing
    Private m_strDisplayMember As String = Nothing

    Public WriteOnly Property Node() As XmlNode
        Set(ByVal value As XmlNode)
            m_xmlView.Redundant = value
        End Set
    End Property

    Public WriteOnly Property Document() As XmlComponent
        Set(ByVal value As XmlComponent)
            m_xmlView.ProjectNode = value
            ' get a useful tag that transmit generation language ID
            m_xmlView.GenerationLanguage = value.GenerationLanguage
        End Set
    End Property

    Public WriteOnly Property ListToCheck() As Boolean
        Set(ByVal value As Boolean)
            Me.Ignore_All.Enabled = value
        End Set
    End Property

    Public WriteOnly Property ImportName() As String
        Set(ByVal value As String)
            m_xmlView.ImportName = value
        End Set
    End Property

    Public Shared Function VerifyRedundancy(ByVal projectNode As XmlComponent, ByVal strMessage1 As String, _
                                            ByVal child As XmlNode, _
                                            Optional ByVal strImportName As String = "", _
                                            Optional ByVal bDisplayEmptyList As Boolean = True, _
                                            Optional ByVal bListToCheck As Boolean = False) As EResult
        If bDisplayEmptyList = False Then
            If GetListRedundancies(projectNode, child, Nothing) = False Then
                Return EResult.RedundancyIgnored
            End If
        End If
        Dim fen As New dlgRedundancy
        fen.Document = projectNode
        fen.Node = child
        fen.ImportName = strImportName
        fen.Text = strMessage1
        fen.ListToCheck = bListToCheck

        Select Case fen.ShowDialog()
            Case System.Windows.Forms.DialogResult.Ignore
                Return EResult.RedundancyIgnoredAll

            Case System.Windows.Forms.DialogResult.OK
                If CType(fen.Tag, Boolean) Then
                    Return EResult.RedundancyChanged
                End If
        End Select

        Return EResult.RedundancyIgnored
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            If m_xmlView.UpdateValues(lsbRedundantClasses, lsbRemainClasses) Then
                Me.Tag = True
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
            Me.Tag = False
            Me.DialogResult = System.Windows.Forms.DialogResult.Abort
            Me.Close()
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Tag = False
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Ignore_All_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ignore_All.Click
        Me.Tag = False
        Me.DialogResult = System.Windows.Forms.DialogResult.Ignore
        Me.Close()
    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        m_xmlView = XmlNodeManager.GetInstance().CreateView("reference_redundancy")
    End Sub

    Private Sub dlgRedundancy_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If m_xmlView.LoadNodes(lsbRedundantClasses) Then
                lblMessage.Text = "'" + XmlProjectTools.GetName(m_xmlView.Node) + "' found several times!"
            Else
                lblMessage.Text = "..."
            End If

            m_strDisplayMember = cstFullUmlPathName

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub lsbRedundantClasses_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lsbRedundantClasses.MouseClick
        Dim index As Integer = lsbRedundantClasses.IndexFromPoint(e.X, e.Y)
        If index > -1 Then
            With CType(lsbRedundantClasses.Items(index), XmlNodeListView)
                If lsbRedundantClasses.SelectedIndices().Contains(index) Then
                    .CheckedView = True
                Else
                    .CheckedView = False
                End If
            End With
            m_xmlView.UpdateRemainingList(lsbRedundantClasses, lsbRemainClasses, m_strDisplayMember, OK_Button)
        End If
    End Sub

    Private Sub optDisplay_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles optDisplay_1.CheckedChanged, optDisplay_1.CheckedChanged, optDisplay_2.CheckedChanged

        If optDisplay_0.Checked _
        Then
            m_strDisplayMember = cstFullUmlPathName

        ElseIf optDisplay_1.Checked _
        Then
            m_strDisplayMember = cstFullpathClassName
        Else
            m_strDisplayMember = cstFullInfo
        End If

        m_xmlView.LoadNodes(lsbRedundantClasses, m_strDisplayMember, True)
        m_xmlView.UpdateRemainingList(lsbRedundantClasses, lsbRemainClasses, m_strDisplayMember, OK_Button)
    End Sub
End Class
