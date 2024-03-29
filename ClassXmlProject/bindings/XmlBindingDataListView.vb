﻿Imports System
Imports System.Xml
Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Generic
Imports Microsoft.VisualBasic

Public Interface InterfListViewContext

    ReadOnly Property CurrentContext() As String

End Interface

Public Interface InterfListViewNotifier

    Function EventDoubleClick(ByRef bDisplayChildren As Boolean, Optional ByVal bEditMode As Boolean = False) As Boolean
    Function EventClick() As Boolean
    Function CanDrag() As Boolean
    Function CanDropItem(ByRef child As XmlComponent, ByRef bImported As Boolean, Optional ByVal bCheckOnly As Boolean = True) As Boolean

End Interface

Public Interface InterfListViewControl

    Sub ChangeView(ByVal dataControl As DataListView)

End Interface

Public Class XmlBindingDataListView
    Implements InterfNodeCounter

    Private m_dataControl As XmlDataListView
    Private m_strViewName As String
    Private m_strFirstNodeName As String
    Private m_strPath As String

    Private m_xmlRootNode As XmlComposite
    Private m_xmlParentNode As XmlComposite
    Private m_xmlNodeHistory As ArrayList
    Private m_xmlReferenceNodeCounter As XmlReferenceNodeCounter
    Private m_xmlStack As List(Of XmlComposite)
    Private m_currentIndex As Integer

    Private m_bRaiseDataError As Boolean

    Public WriteOnly Property NodeCounter() As XmlReferenceNodeCounter Implements InterfNodeCounter.NodeCounter
        Set(ByVal value As XmlReferenceNodeCounter)
            m_xmlReferenceNodeCounter = value
        End Set
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return TypeName(Me)
        End Get
    End Property

    Public ReadOnly Property Path() As String
        Get
            Return m_strPath
        End Get
    End Property

    Public ReadOnly Property Parent() As XmlComposite
        Get
            Return m_xmlStack(m_currentIndex)
        End Get
    End Property

    Public ReadOnly Property Ancestor() As XmlComposite
        Get
            Return m_xmlStack(0)
        End Get
    End Property

    Public Sub New(ByVal dataControl As XmlDataListView)
        m_dataControl = dataControl
        m_strViewName = ""
        m_bRaiseDataError = True
    End Sub

    Public Function LoadItems(ByVal composite As XmlComposite, ByVal strViewName As String, ByVal strFirstNodeName As String) As Boolean
        Dim bResult As Boolean = False

        If composite Is Nothing Then
            Throw New Exception("Node to load must not be null in call of " + Me.ToString() + _
                                ".LoadXmlNodes(Nothing," + strViewName + ")")
        End If
        ' Remain reference of root node in its own original instance
        m_xmlRootNode = composite
        Dim view As XmlComponent = XmlNodeManager.GetInstance().CreateView(composite.Node, strViewName)
        InitStack(view)
        m_strFirstNodeName = strFirstNodeName
        m_strViewName = strViewName

        InitControl()
        Refresh()

        Return bResult
    End Function

    Public Sub UpdatePath()
        m_strPath = "/" + m_xmlStack(0).Name
        For i As Integer = 1 To m_xmlStack.Count - 1
            m_strPath = m_strPath + "/" + m_xmlStack(i).Name
        Next i
    End Sub

    Public Function CanDragItem(ByVal item As ListViewItem) As Boolean
        Dim interf As InterfListViewNotifier = TryCast(item.Tag, InterfListViewNotifier)
        If interf IsNot Nothing Then
            Return interf.CanDrag()
        End If
        Return False
    End Function

    Public Function CanDropItem(ByVal parent As ListViewItem, ByVal child As ListViewItem) As Boolean
        Debug.Print("XmlBindingDataListView.CanDropItem")
        Dim interf As InterfListViewNotifier = TryCast(parent.Tag, InterfListViewNotifier)
        If interf IsNot Nothing Then
            Dim component As XmlComponent = TryCast(child.Tag, XmlComponent)
            Dim bUnused As Boolean = False
            If interf.CanDropItem(component, bUnused) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function DropParentItem(ByVal child As ListViewItem) As Boolean
        Debug.Print("XmlBindingDataListView.DropParentItem")
        Dim interf As InterfListViewNotifier = TryCast(m_xmlParentNode, InterfListViewNotifier)

        If interf IsNot Nothing Then
            Dim component As XmlComponent = TryCast(child.Tag, XmlComponent)
            Dim bImportData As Boolean = False

            If interf.CanDropItem(component, bImportData, False) Then
                If bImportData Then
                    component.GenerationLanguage = m_xmlParentNode.GenerationLanguage
                    component.Name = component.Name + "_imported"
                    component.SetIdReference(m_xmlReferenceNodeCounter, XmlComponent.ENameReplacement.AddCopyName, True)
                End If
                m_xmlRootNode.Updated = True
                ResetBindings(True)
                Return True
            Else
                MsgBox("Sorry can't drop node '" + component.NodeName + "'!", MsgBoxStyle.Exclamation, "'Drop' command")
            End If
        End If
        Return False
    End Function

    Public Function DropItem(ByVal parent As ListViewItem, ByVal child As ListViewItem) As Boolean
        Debug.Print("XmlBindingDataListView.DropItem")
        Dim interf As InterfListViewNotifier = TryCast(parent.Tag, InterfListViewNotifier)
        If interf IsNot Nothing Then
            Dim component As XmlComponent = TryCast(child.Tag, XmlComponent)
            Dim bImportData As Boolean = False

            If CType(parent.Tag, XmlComponent).Document IsNot component.Document _
            Then
                MsgBox("Please open sub-level of the target and drop node in an empty zone!", MsgBoxStyle.Exclamation, "'Drop' command")

            ElseIf interf.CanDropItem(component, bImportData, False) Then
                m_xmlRootNode.Updated = True
                ResetBindings(True)
                Return True
            Else
                MsgBox("Sorry can't drop node '" + component.NodeName + "'!", MsgBoxStyle.Exclamation, "'Drop' command")
            End If
        End If
        Return False
    End Function

    Public Function IsNotHome() As Boolean
        Return (m_currentIndex > 0)
    End Function

    Public Function IsNotEnd() As Boolean
        Return (m_currentIndex < m_xmlStack.Count)
    End Function

    Public Function GoHomeNode() As Boolean
        If m_currentIndex > 0 Then
            m_currentIndex = 0
            m_xmlParentNode = m_xmlStack(m_currentIndex)
            m_xmlStack.Clear()
            m_xmlStack.Add(m_xmlParentNode)
            m_strPath = m_xmlParentNode.Name
            'Debug.Print("GoHomeNode:=(" + CStr(m_currentIndex) + ")" + m_xmlParentNode.ToString)
            Refresh()
            m_dataControl.CurrentContext = CType(m_xmlParentNode, InterfListViewContext).CurrentContext
        End If
        Return False
    End Function

    Private Sub InitStack(ByVal composite As XmlComposite)
        Dim InterfCounter As InterfNodeCounter = TryCast(composite, InterfNodeCounter)
        If InterfCounter IsNot Nothing Then InterfCounter.NodeCounter = m_xmlReferenceNodeCounter
        m_xmlStack = New List(Of XmlComposite)
        m_currentIndex = 0
        m_xmlStack.Add(composite)
        m_currentIndex = m_xmlStack.Count - 1
        m_xmlParentNode = composite
        m_strPath = m_xmlParentNode.Name
    End Sub

    Private Sub PushNode(ByVal composite As XmlComposite)
        Dim InterfCounter As InterfNodeCounter = TryCast(composite, InterfNodeCounter)
        If InterfCounter IsNot Nothing Then InterfCounter.NodeCounter = m_xmlReferenceNodeCounter
        m_xmlStack.Add(composite)
        m_currentIndex = m_xmlStack.Count - 1
        m_xmlParentNode = composite
        m_strPath = m_strPath + "/" + m_xmlParentNode.Name
        'Debug.Print("PushNode:=(" + CStr(m_currentIndex) + ")" + composite.ToString)
        Refresh()
        m_dataControl.CurrentContext = CType(composite, InterfListViewContext).CurrentContext
    End Sub

    Public Function PopNode() As Boolean
        If m_currentIndex > 0 Then
            m_xmlStack.RemoveAt(m_currentIndex)
            m_currentIndex = m_currentIndex - 1
            m_xmlParentNode = m_xmlStack(m_currentIndex)
            m_strPath = Left(m_strPath, InStrRev(m_strPath, "/") - 1)
            'Debug.Print("PopNode:=(" + CStr(m_currentIndex) + ")" + m_xmlParentNode.ToString)
            Refresh()
            m_dataControl.CurrentContext = CType(m_xmlParentNode, InterfListViewContext).CurrentContext
        End If
        Return (m_currentIndex > 0)
    End Function

    Public Sub ResetBindings(ByVal metaDataChanged As Boolean)
        ' use "metaDataChanged" for future use
        Refresh()
    End Sub

    Public Function AfterLabelEdit(ByVal sender As DataListView, ByVal e As DataListViewEventArgs) As Boolean

        Dim component As XmlComponent = TryCast(sender.DataBoundItem(e.Item), XmlComponent)
        If component IsNot Nothing And e.Label IsNot Nothing Then
            component.Name = e.Label
            m_xmlRootNode.Updated = True
            Return True
        End If
        Return False
    End Function

    Public Function ItemClick(ByRef bChanged As Boolean, ByVal sender As DataListView, ByVal e As DataListViewEventArgs, _
                              ByVal bDoubleClick As Boolean, ByVal bEditMode As Boolean) As Boolean
        Dim bDisplayChildren As Boolean = False

        bChanged = False

        Dim composite As XmlComposite = CType(sender.DataBoundItem(e.Item), XmlComposite)
            Dim InterfNotifier As InterfListViewNotifier = TryCast(composite, InterfListViewNotifier)

            If InterfNotifier IsNot Nothing Then
                Dim InterfCounter As InterfNodeCounter = TryCast(InterfNotifier, InterfNodeCounter)
                If InterfCounter IsNot Nothing Then InterfCounter.NodeCounter = m_xmlReferenceNodeCounter

                If bDoubleClick Then
                    bChanged = InterfNotifier.EventDoubleClick(bDisplayChildren, bEditMode)
                Else
                    InterfNotifier.EventClick()
                End If
            End If
            If bChanged _
            Then
                m_xmlRootNode.Updated = True
                Refresh()

            ElseIf bDisplayChildren _
            Then
                PushNode(composite)
            End If

        Return bDisplayChildren
    End Function

    Private Sub Refresh()

        If m_xmlParentNode Is Nothing Then
            Throw New Exception("Property m_xmlParentNode is null")
        End If

        ' Disconnect source to make update
        m_dataControl.DataSource = Nothing

        ' To enforce generation language propagation
        m_xmlParentNode.GenerationLanguage = m_xmlRootNode.GenerationLanguage

        With CType(m_xmlParentNode, XmlComposite)
            .LoadChildrenList(m_strViewName)
            m_dataControl.DataSource = .ChildrenList

            If .ChildrenList.Count > 0 Then
                m_dataControl.ShowItemToolTips = True
            Else
                m_dataControl.ShowItemToolTips = False
            End If
        End With
    End Sub

    Public Function AddItem(ByVal strNodeName As String) As XmlComponent
        Dim xmlComponent As XmlComponent = Nothing
        Dim xmlView As XmlComponent = Nothing
        Try
            If m_xmlParentNode Is Nothing Then
                Throw New Exception("parentNode property is null")
            Else
                Dim parentNode As XmlComposite = XmlNodeManager.GetInstance().CreateDocument(m_xmlParentNode.Node)
                parentNode.GenerationLanguage = m_xmlRootNode.GenerationLanguage

                If strNodeName Is Nothing Then
                    strNodeName = CType(m_dataControl.Tag, String)
                End If
                ' Create an adapter that build xml nodes and attributes
                xmlComponent = XmlNodeManager.GetInstance().CreateDocument(strNodeName, parentNode.Node.OwnerDocument, False)
                xmlComponent.GenerationLanguage = parentNode.GenerationLanguage
                xmlComponent.SetIdReference(m_xmlReferenceNodeCounter)

                Dim child As XmlNode = parentNode.AppendComponent(xmlComponent)
                If child IsNot Nothing Then
                    xmlView = XmlNodeManager.GetInstance().CreateView(child, m_strViewName, parentNode.Node.OwnerDocument)
                    xmlView.GenerationLanguage = xmlComponent.GenerationLanguage

                    With CType(xmlView, InterfObject)
                        .InterfObject = m_xmlParentNode
                        .Update()
                    End With

                    m_xmlRootNode.Updated = True
                    Refresh()
                End If
            End If
        Catch ex As Exception
            MsgExceptionBox(ex)
        End Try

        Return xmlView
    End Function

    Public Function CutItem(ByVal component As XmlComponent) As Boolean

        ' Store in a specific clipboard shared by all projects
        XmlComponent.Clipboard.SetData(component, False)

        Return True
    End Function

    Public Function CopyItem(ByVal component As XmlComponent) As Boolean

        ' Store in a specific clipboard shared by all projects
        XmlComponent.Clipboard.SetData(component, True)

        Return True
    End Function

    Public Function PasteItem() As Boolean
        Dim bResult As Boolean = False

        If m_xmlParentNode Is Nothing Then
            Throw New Exception("m_xmlParentNode property is null")
        Else
            ' Get back from the specific clipboard shared by all projects
            Dim bCopy As Boolean
            Dim bImportData As Boolean = XmlComponent.Clipboard.CheckData(m_xmlParentNode, bCopy)  ' Check if data comme from other document
            Dim component = XmlComponent.Clipboard.Data
            If bCopy = False And bImportData _
            Then
                MsgBox("Sorry, can't cut object from one project and paste to another!", MsgBoxStyle.Exclamation, "'Cut/paste' command")

            ElseIf m_xmlParentNode.CanPasteItem(XmlComponent.Clipboard.Data) _
            Then
                component = XmlComponent.Clipboard.GetData(bCopy)   ' method "Get" remove node from clipboard
                ' Refresh is done in method "DuplicateItem"
                If DuplicateOrPasteItem(component, bCopy, bImportData) Then
                    bResult = True
                Else
                    MsgBox("Sorry can't paste '" + component.NodeName + "' !", MsgBoxStyle.Exclamation, "'Paste' command")
                End If
            Else
                MsgBox("Sorry can't paste node '" + component.NodeName + "' !", MsgBoxStyle.Exclamation, "'Paste' command")
            End If
        End If

        Return bResult
    End Function

    Public Function DuplicateOrPasteItem(ByVal component As XmlComponent, _
                                         Optional ByVal bDuplicate As Boolean = True, _
                                         Optional ByVal bImportData As Boolean = False) As Boolean

        If m_xmlParentNode Is Nothing Then
            Throw New Exception("m_xmlParentNode property is null")
        Else
            Dim xmlComponent As XmlComponent = component

            If bDuplicate And bImportData = False _
            Then
                xmlComponent = m_xmlParentNode.DuplicateComponent(component)

            ElseIf bImportData _
            Then
                xmlComponent = m_xmlParentNode.ImportDocument(component)
            End If

            If xmlComponent IsNot Nothing Then
                xmlComponent.GenerationLanguage = m_xmlParentNode.GenerationLanguage

                ' Append a node not duplicated cause move node to new location.
                Dim child As XmlNode = m_xmlParentNode.AppendComponent(xmlComponent)
                If child IsNot Nothing Then

                    If bDuplicate And bImportData = False _
                    Then
                        xmlComponent.Name = component.Name + "_copy"
                        xmlComponent.SetIdReference(m_xmlReferenceNodeCounter, xmlComponent.ENameReplacement.AddCopyName)

                    ElseIf bImportData _
                    Then
                        xmlComponent.Name = component.Name + "_imported"
                        xmlComponent.SetIdReference(m_xmlReferenceNodeCounter, xmlComponent.ENameReplacement.AddCopyName, True)
                    End If

                    m_xmlRootNode.Updated = True
                    Refresh()
                    Return True
                End If
            End If
        End If

        Return False
    End Function

    Public Function DeleteItem(ByVal component As XmlComponent) As Boolean

        If component IsNot Nothing _
            Then
            Dim parent As XmlComposite = CType(XmlNodeManager.GetInstance().CreateDocument(m_xmlParentNode.Node), XmlComposite)
            parent.GenerationLanguage = m_xmlParentNode.GenerationLanguage

            If parent.CanRemove(component) _
            Then
                If parent.RemoveComponent(component) _
                Then
                    parent.Updated = True
                End If
            End If

            If parent.Updated Then
                ' Updated real document
                m_xmlRootNode.Updated = True
                Return True
            End If
        End If

        Return False
    End Function

    Private Sub InitControl()

        If m_strViewName = "" Then
            Throw New Exception("Method 'LoadItems' must be called previously")
        End If
        Dim component As XmlComponent = XmlNodeManager.GetInstance().CreateView(m_strViewName)
        CType(component, XmlComponent).Document = m_xmlParentNode.Node.OwnerDocument
        CType(component, InterfViewControl).InitControl(m_dataControl)

    End Sub

    Public Sub ChangeView()
        If m_strViewName = "" Then
            Throw New Exception("Method 'LoadItems' must be called previously")
        End If
        Dim component As XmlComponent = XmlNodeManager.GetInstance().CreateView(m_strViewName)
        CType(component, InterfListViewControl).ChangeView(m_dataControl)
    End Sub
End Class

