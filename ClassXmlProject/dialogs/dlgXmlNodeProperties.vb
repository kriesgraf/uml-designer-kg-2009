Imports System
Imports System.Xml

Public Class dlgXmlNodeProperties
    Public Shared Function DisplayProperties(ByVal myobject As Object) As Boolean
        Dim bResult As Boolean = False
        Try
            If myobject IsNot Nothing Then
                Dim fen As New dlgXmlNodeProperties
                Dim node As XmlNode = CType(myobject, XmlComponent).Node
                Dim xmlcpnt As XmlComponent = XmlNodeManager.GetInstance().CreateDocument(node, True)
                fen.PropertyGrid1.SelectedObject = xmlcpnt
                fen.ShowDialog()
                bResult = True  '   CType(fen.Tag, Boolean)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return bResult
    End Function

    Private Sub dlgXmlNodeProperties_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Text = Me.PropertyGrid1.SelectedObject.ToString
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub
End Class
