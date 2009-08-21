Imports System

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

    Private Sub dlgImportExchange_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            With m_xmlView
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
End Class