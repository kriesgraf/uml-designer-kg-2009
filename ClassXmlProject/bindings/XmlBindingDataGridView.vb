Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports System.Collections
Imports ClassXmlProject.XmlProjectTools
Imports Microsoft.VisualBasic

Public Interface InterfGridViewNotifier

    ' Please return False when column is a checkbox
    Function EventClick(ByVal dataMember As String) As Boolean
    Function EventDoubleClick(ByVal dataMember As String) As Boolean
    Function CanDropItem(ByVal component As XmlComponent) As Boolean
    Function CanDragItem() As Boolean

End Interface

Public Class XmlBindingDataGridView
    Implements InterfNodeCounter

    Private m_dataControl As XmlDataGridView
    Private m_strViewName As String

    Private m_xmlParentNode As XmlComposite
    Private m_xmlXpathQuery As String
    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter

    Private m_bForbiddenCellContentDoubleClick As Boolean
    Private m_bRaiseDataError As Boolean
    Private m_bTemporaryForbiddenUserToAddRows As Boolean
    Private m_bAllowUserToAddRows As Boolean

    Private m_refObject As InterfObject = Nothing

    Private WithEvents m_BindingSource As BindingSource

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

    Public WriteOnly Property TemporaryForbiddenUserToAddRows() As Boolean
        Set(ByVal value As Boolean)
            m_bTemporaryForbiddenUserToAddRows = value
        End Set
    End Property

    Public WriteOnly Property ForbiddenCellContentDoubleClick() As Boolean
        Set(ByVal value As Boolean)
            m_bForbiddenCellContentDoubleClick = value
        End Set
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return TypeName(Me)
        End Get
    End Property

    Public WriteOnly Property CatchDataError() As Boolean
        Set(ByVal value As Boolean)
            m_bRaiseDataError = value
        End Set
    End Property

    Public Sub New(ByVal dataControl As XmlDataGridView)
        m_BindingSource = New BindingSource
        m_dataControl = dataControl
        m_strViewName = ""
        m_BindingSource.AllowNew = True
        m_bRaiseDataError = False
        m_bTemporaryForbiddenUserToAddRows = True   ' set to False when reentrance bug will be fixed
        m_bForbiddenCellContentDoubleClick = False   ' set to False, when special double click is required
    End Sub

    Public Sub EndEdit()
        m_BindingSource.EndEdit()
    End Sub

    Public Function LoadXmlNodes(ByVal composite As XmlComposite, ByVal xpath As String, _
                                 ByVal strViewName As String, Optional ByVal refobj As InterfObject = Nothing) As Boolean
        Dim bResult As Boolean = False
        Try
            If composite Is Nothing Then
                Throw New Exception("Node to load must not be null in call of " + Me.ToString() + _
                                    ".LoadXmlNodes(Nothing," + strViewName + ")")
            End If

            m_xmlParentNode = composite
            m_xmlXpathQuery = xpath
            m_strViewName = strViewName
            m_refObject = refobj
            InitControl()
            Refresh()

        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function CellContentClick(ByVal sender As DataGridView, ByVal e As DataGridViewCellEventArgs, ByVal bDoubleClick As Boolean) As Boolean

        If e.RowIndex < 0 Then Exit Function

        Dim bResult As Boolean = False

        Try
            Dim InterfNotifier As InterfGridViewNotifier = TryCast(sender.Rows(e.RowIndex).DataBoundItem, InterfGridViewNotifier)

            If InterfNotifier IsNot Nothing _
            Then
                If bDoubleClick _
                Then
                    If m_bForbiddenCellContentDoubleClick = False _
                    Then
                        Dim InterfCounter As InterfNodeCounter = TryCast(InterfNotifier, InterfNodeCounter)
                        If InterfCounter IsNot Nothing Then InterfCounter.NodeCounter = m_xmlReferenceNodeCounter

                        bResult = InterfNotifier.EventDoubleClick(sender.Columns(e.ColumnIndex).DataPropertyName)
                    End If
                Else
                    bResult = InterfNotifier.EventClick(sender.Columns(e.ColumnIndex).DataPropertyName)
                End If
            End If
            If bResult Then ResetBindings(True)
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Public Function RowHeaderClick(ByVal sender As DataGridView, ByVal e As DataGridViewCellMouseEventArgs, _
                                   ByVal bDoubleClick As Boolean) As Boolean

        If e.RowIndex < 0 Then
            Return False
        End If

        Dim bResult As Boolean = False

        Try
            Dim InterfNotifier As InterfGridViewNotifier = TryCast(sender.Rows(e.RowIndex).DataBoundItem, InterfGridViewNotifier)

            If InterfNotifier IsNot Nothing _
            Then
                Dim InterfCounter As InterfNodeCounter = TryCast(InterfNotifier, InterfNodeCounter)
                If InterfCounter IsNot Nothing Then InterfCounter.NodeCounter = m_xmlReferenceNodeCounter

                If bDoubleClick Then
                    bResult = InterfNotifier.EventDoubleClick("")
                Else
                    bResult = InterfNotifier.EventClick("")
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return bResult
    End Function

    Private Sub Refresh()
        Try
            If m_xmlParentNode Is Nothing Then
                Throw New Exception("Property m_xmlParentNode is null")
            End If
            Dim list As XmlNodeList = m_xmlParentNode.SelectNodes(m_xmlXpathQuery)

            Dim iterator As IEnumerator = list.GetEnumerator()
            iterator.Reset()
            If m_BindingSource.List.Count > 0 Then m_BindingSource.List.Clear()

            Dim component As XmlComponent = Nothing

            ' To avoid the typical bug of datagridview
            m_dataControl.AllowUserToAddRows = False

            While iterator.MoveNext()
                component = XmlNodeManager.GetInstance().CreateView(CType(iterator.Current, XmlNode), m_strViewName, m_xmlParentNode.Node.OwnerDocument)
                component.Tag = m_xmlParentNode.Tag
                component.Document = m_xmlParentNode.Document

                If m_refObject IsNot Nothing _
                Then
                    CType(component, InterfObject).InterfObject = m_refObject.InterfObject
                End If

                Dim interf As InterfNodeCounter = TryCast(component, InterfNodeCounter)
                If interf IsNot Nothing _
                Then
                    interf.NodeCounter = m_xmlReferenceNodeCounter
                End If

                m_BindingSource.Add(component)
            End While

            If m_BindingSource.Count > 0 Then
                m_dataControl.AllowUserToAddRows = m_bAllowUserToAddRows
                m_dataControl.ShowCellToolTips = True

            Else
                m_dataControl.ShowCellToolTips = False

                If m_bTemporaryForbiddenUserToAddRows _
                Then
                    m_dataControl.AllowUserToAddRows = False
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub m_BindingSource_AddingNew(ByVal sender As Object, ByVal e As System.ComponentModel.AddingNewEventArgs) Handles m_BindingSource.AddingNew
        Try
            Dim strNodeName As String = m_xmlParentNode.AddNewComponent(TryCast(m_dataControl.Tag, String))
            e.NewObject = AddItem(strNodeName)

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Function AddItem(ByVal strNodeName As String) As XmlComponent
        Dim xmlComponent As XmlComponent = Nothing
        Dim xmlView As XmlComponent = Nothing
        Try
            If m_xmlParentNode Is Nothing Then
                Throw New Exception("m_xmlParentNode property is null")
            Else
                If strNodeName Is Nothing Then
                    strNodeName = CType(m_dataControl.Tag, String)
                End If
                ' Create an adapter that build xml nodes and attributes
                xmlComponent = XmlNodeManager.GetInstance().CreateDocument(strNodeName, m_xmlParentNode.Node.OwnerDocument, False)
                xmlComponent.Tag = m_xmlParentNode.Tag
                xmlComponent.SetIdReference(m_xmlReferenceNodeCounter)

                Dim child As XmlNode = m_xmlParentNode.AppendComponent(xmlComponent)
                If child IsNot Nothing Then
                    xmlView = XmlNodeManager.GetInstance().CreateView(child, m_strViewName, m_xmlParentNode.Node.OwnerDocument)

                    If m_refObject IsNot Nothing Then
                        With CType(xmlView, InterfObject)
                            .InterfObject = m_refObject
                            .Update()
                        End With
                    End If

                    Refresh()

                    If m_dataControl.AllowUserToAddRows = False Then
                        m_BindingSource.ResetBindings(True)
                        m_dataControl.AllowUserToAddRows = m_bAllowUserToAddRows
                    End If
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try

        Return xmlView
    End Function

    Public Function CanDragItem(ByVal component As Object) As Boolean
        Dim interf As InterfGridViewNotifier = TryCast(component, InterfGridViewNotifier)
        If interf IsNot Nothing Then
            Return interf.CanDragItem()
        End If
        Return False
    End Function

    Public Function CanDropItem(ByVal before As Object, ByVal component As Object) As Boolean
        Dim interf As InterfGridViewNotifier = TryCast(before, InterfGridViewNotifier)
        If interf IsNot Nothing Then
            If interf.CanDropItem(CType(component, XmlComponent)) Then
                ResetBindings(True)
                Return True
            End If
        End If
        Return False
    End Function

    Public Function DuplicateItem(ByVal component As XmlComponent) As Boolean
        Try
            If m_xmlParentNode Is Nothing Then
                Throw New Exception("m_xmlParentNode property is null")
            Else
                Dim xmlComponent As XmlComponent = m_xmlParentNode.DuplicateComponent(component)
                If xmlComponent IsNot Nothing Then
                    xmlComponent.Tag = m_xmlParentNode.Tag
                    xmlComponent.Name = component.Name + "_copy"
                    xmlComponent.SetIdReference(m_xmlReferenceNodeCounter, True)

                    Dim child As XmlNode = m_xmlParentNode.AppendComponent(xmlComponent)
                    If child IsNot Nothing Then
                        Dim xmlView As XmlComponent = XmlNodeManager.GetInstance().CreateView(child, m_strViewName, m_xmlParentNode.Node.OwnerDocument)

                        Refresh()
                        Return True
                    Else
                        MsgBox("Sorry can't dupplicate node '" + component.NodeName + "'!", MsgBoxStyle.Exclamation)
                    End If
                End If
            End If

        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
        Return False
    End Function

    Public Function DeleteItem(ByVal component As XmlComponent) As Boolean
        If component IsNot Nothing Then
            If m_xmlParentNode.RemoveComponent(component) Then
                If m_BindingSource.List.Count < 1 Then m_dataControl.AllowUserToAddRows = False
                Return True
            End If
        End If
        Return False
    End Function

    Public Function CutItem(ByVal component As XmlComponent) As Boolean
        Try
            ' Store in a specific clipboard shared by all projects
            XmlComponent.Clipboard.SetData(component, False)

        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Public Function CopyItem(ByVal component As XmlComponent) As Boolean
        Try
            ' Store in a specific clipboard shared by all projects
            XmlComponent.Clipboard.SetData(component, True)

        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Public Function PasteItem() As Boolean
        Try
            If m_xmlParentNode Is Nothing Then
                Throw New Exception("m_xmlParentNode property is null")
            Else
                ' Get back from the specific clipboard shared by all projects
                Dim bCopy As Boolean
                Dim component As XmlComponent = XmlComponent.Clipboard.GetData(bCopy)

                If m_xmlParentNode.CanPasteItem(component) _
                Then
                    ' Method XmlNode.AppendChild make a cut/paste if node is not cloned.
                    If bCopy = True Then
                        component = m_xmlParentNode.DuplicateComponent(component)
                        component.SetIdReference(m_xmlReferenceNodeCounter, True)
                    End If


                    If component IsNot Nothing Then
                        Dim child As XmlNode = m_xmlParentNode.AppendComponent(component)
                        If child IsNot Nothing _
                        Then
                            Refresh()
                            Return True
                        Else
                            MsgBox("Sorry can't paste node '" + component.NodeName + "' on '" + m_xmlParentNode.NodeName + "' !", MsgBoxStyle.Exclamation)
                        End If
                    End If
                Else
                    MsgBox("Sorry can't paste node '" + component.NodeName + "' on '" + m_xmlParentNode.NodeName + "' !", MsgBoxStyle.Exclamation)
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return False
    End Function
    Private Sub m_BindingSource_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.BindingManagerDataErrorEventArgs) Handles m_BindingSource.DataError
        If m_bRaiseDataError Then
            If MsgBox(e.Exception.ToString + vbCrLf + vbCrLf + "Please press Cancel if you have notice the reason of this error.", _
                      cstMsgOkCancelCritical) _
                        = MsgBoxResult.Cancel Then

                m_bRaiseDataError = False
            End If
        End If
    End Sub

    Public Sub ResetBindings(Optional ByVal bChanged As Boolean = False)
        Try
            If bChanged Then
                Refresh()
            Else
                m_BindingSource.ResetBindings(False)
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try
    End Sub

    Public Sub UpdateRows()
        For Each element As Object In m_BindingSource.List
            Dim interf As InterfObject = TryCast(element, InterfObject)
            If interf IsNot Nothing Then
                interf.Update()
            End If
        Next
    End Sub

    Private Sub InitControl()
        Try
            If m_strViewName.Length = 0 Then
                Throw New Exception("Method 'LoadXmlNodes' must be called previously")
            End If
            Dim component As XmlComponent = XmlNodeManager.GetInstance().CreateView(m_strViewName)
            Dim document As XmlDocument = m_xmlParentNode.Node.OwnerDocument

            Debug.Print("grid view '" + m_strViewName + "': " + GetName(document.DocumentElement))

            component.Document = document
            component.Tag = m_xmlParentNode.Tag

            CType(component, InterfViewControl).InitControl(m_dataControl)

            m_bAllowUserToAddRows = m_dataControl.AllowUserToAddRows

            m_dataControl.DataSource = Nothing
            m_dataControl.DataSource = m_BindingSource

            If m_BindingSource.Count > 0 Then
                m_dataControl.AllowUserToAddRows = m_bAllowUserToAddRows
            Else
                m_dataControl.AllowUserToAddRows = False
            End If

            m_dataControl.AutoResizeColumns()
            m_dataControl.AutoResizeRows()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
