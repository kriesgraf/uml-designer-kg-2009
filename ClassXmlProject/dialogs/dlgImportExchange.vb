Imports System
Imports System.Windows.Forms

Public Class dlgImportExchange
    Implements InterfFormDocument

    Private m_xmlView As XmlExchangeImportsView = Nothing

    Public Sub DisableMemberAttributes() Implements InterfFormDocument.DisableMemberAttributes
    End Sub

    Public WriteOnly Property Document() As XmlComponent Implements InterfFormDocument.Document
        Set(ByVal value As XmlComponent)
            ' Becareful, use instance given by caller
            m_xmlView = value
        End Set
    End Property

    Private Sub dlgImportExchange_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        m_xmlView.CheckEmpty()
        Me.Tag = m_xmlView.Updated
    End Sub

    Private Sub dlgImportExchange_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            With m_xmlView
                .LoadValues()
                .InitBindingName(Me.Text)
                .InitBindingSource(Me.lsbSource)
                .InitBindingDestination(Me.lsbDestination)
                ' Order is important
                .InitBindingImports(Me.lsbImports)
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub lsbImports_KeyUp(ByVal sender As ListBox, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lsbImports.KeyUp
        Try
            If e.KeyCode = Keys.Insert And sender.SelectedItem IsNot Nothing Then
                m_xmlView.AddImport(lsbImports)
                lsbImports.Select()
            ElseIf e.KeyCode = Keys.Delete And sender.SelectedItem IsNot Nothing Then
                m_xmlView.Delete(lsbImports)
                lsbImports.Select()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub lsbImports_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsbImports.SelectedIndexChanged
        Try
            With m_xmlView
                .SelectDestination(lsbImports)
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub btnDestination_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDestination.Click
        Try
            With m_xmlView
                .MoveDestination()
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub btnSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSource.Click
        Try
            With m_xmlView
                .MoveSource()
            End With
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub listBox_DoubleClick(ByVal sender As ListBox, ByVal e As System.EventArgs) _
        Handles lsbSource.DoubleClick, lsbDestination.DoubleClick
        Try
            If sender.SelectedItem IsNot Nothing Then
                m_xmlView.Edit(sender)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub lsbSource_KeyUp(ByVal sender As ListBox, ByVal e As System.Windows.Forms.KeyEventArgs) _
        Handles lsbSource.KeyUp, lsbDestination.KeyUp
        Try
            If e.KeyCode = Keys.Enter And sender.SelectedItem IsNot Nothing Then
                m_xmlView.Edit(sender)
            ElseIf e.KeyCode = Keys.Left And sender Is lsbDestination Then
                m_xmlView.MoveSource()
            ElseIf e.KeyCode = Keys.Right And sender Is lsbSource Then
                m_xmlView.MoveDestination()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            If lsbImports.SelectedItem IsNot Nothing Then
                m_xmlView.AddImport(lsbImports)
                lsbImports.Select()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            If lsbImports.SelectedItem IsNot Nothing Then
                m_xmlView.Delete(lsbImports)
                lsbImports.Select()
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub
End Class