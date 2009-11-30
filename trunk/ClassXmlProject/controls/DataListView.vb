Imports System
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Collections

''' <summary>
''' Displays data in a customizable listview. 
''' </summary>
''' <remarks></remarks>
Public Class DataListView
    Inherits ListView

#Region "Events"
    ''' <summary>
    ''' Occurs when an item is clicked.
    ''' </summary>
    ''' <param name="sender">The DataListView control</param>
    ''' <param name="e">A DataListViewEventArgs object</param>
    Public Event ItemClick(ByVal sender As DataListView, ByVal e As DataListViewEventArgs)

    ''' <summary>
    ''' Mouse DoubleClick on current selected item
    ''' </summary>
    ''' <param name="sender">The DataListView control</param>
    ''' <param name="e">A DataListViewEventArgs object</param>
    Public Event ItemDoubleClick(ByVal sender As DataListView, ByVal e As DataListViewEventArgs)

    ''' <summary>
    ''' Occurs when mouse is clicked in empty zone
    ''' </summary>
    ''' <param name="sender">The DataListView control</param>
    ''' <param name="e">A System.EventArgs object</param>
    Public Event EmptyZoneClick(ByVal sender As DataListView, ByVal e As System.EventArgs)

#End Region

#Region "Class declarations"
    Private m_bindingSource As BindingSource
    Private m_strValueMember As String
    Private m_strDisplayMember As String
    Private components As System.ComponentModel.IContainer
    Friend WithEvents TimerEmptyZone As System.Windows.Forms.Timer
    Private m_oDataSource As Object

#End Region

#Region "Properties"
    ''' <summary>
    ''' Data source to bind
    ''' </summary>
    Public Property DataSource() As IEnumerable
        Get
            Return CType(m_oDataSource, IEnumerable)
        End Get
        Set(ByVal value As IEnumerable)
            m_oDataSource = value
            If m_bindingSource IsNot Nothing Then
                m_bindingSource.DataSource = m_oDataSource
            End If
            If m_oDataSource IsNot Nothing Then InitBinding()
        End Set
    End Property

    ''' <summary>
    ''' BindingSource to replace
    ''' </summary>
    Public Property BindingSource() As BindingSource
        Get
            Return m_bindingSource
        End Get
        Set(ByVal value As BindingSource)
            m_bindingSource = value
            If value IsNot Nothing And m_oDataSource IsNot Nothing Then
                InitBinding()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Property to bind Text to display
    ''' </summary>
    ''' <remarks>Mandatory</remarks>
    Public Property DisplayMember() As String
        Get
            Return m_strDisplayMember
        End Get
        Set(ByVal value As String)
            m_strDisplayMember = value
        End Set
    End Property

    ''' <summary>
    ''' Property to bind Icon to display
    ''' </summary>
    ''' <remarks>Optional</remarks>
    Public Property ValueMember() As String
        Get
            Return m_strValueMember
        End Get
        Set(ByVal value As String)
            m_strValueMember = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the data-bound object that populated the selected item
    ''' </summary>
    Public ReadOnly Property SelectedItem() As Object
        Get
            Dim index As Integer = Me.SelectedIndex
            If index > -1 Then
                Return DataBoundItem(index)
            End If
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Gets the items that are selected in the control. 
    ''' </summary>
    ''' <returns>Null-base index</returns>
    Public ReadOnly Property SelectedIndex() As Integer
        Get
            If MyBase.SelectedIndices.Count > 0 Then
                Return SelectedIndices.Item(0)
            End If
            Return -1
        End Get
    End Property

    ''' <summary>
    ''' Gets the data-bound object that populated the item
    ''' </summary>
    ''' <param name="index">Null-base index</param>
    ''' <returns>The data-bound object</returns>
    Public ReadOnly Property DataBoundItem(ByVal index As Integer) As Object
        Get
            If MyBase.Items.Count > index And index > -1 Then
                Return Items(index).Tag
            End If
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Select one item
    ''' </summary>
    ''' <param name="index">Null index</param>
    ''' <remarks></remarks>
    Public Sub SelectItem(ByVal index As Integer)
        If MyBase.Items.Count > 0 Then
            If Items.Count > index And index > 0 Then
                MyBase.Items(index).Selected = True
            End If
        End If
    End Sub

#End Region

#Region "Private methods"

    Private Sub InitBinding()

        If m_strDisplayMember = "" Then
            If m_oDataSource IsNot Nothing Then
                Throw New Exception("DisplayMember is not defined")
            Else
                Exit Sub ' A la construction il ne faut lever d'exception
            End If
        End If

        Dim lvitem As ListViewItem

        If m_bindingSource Is Nothing _
        Then
            m_bindingSource = New BindingSource
            m_bindingSource.DataSource = m_oDataSource

        ElseIf m_bindingSource.DataSource Is Nothing _
        Then
            m_bindingSource.DataSource = m_oDataSource
        End If

        '  If m_bindingSource.Count = 0 Then Exit Sub

        Dim list As PropertyDescriptorCollection = m_bindingSource.GetItemProperties(Nothing)

#If DEBUG Then
        'If m_oDataSource IsNot Nothing Then Debug.Print(m_oDataSource.ToString())
        'For Each pe As System.ComponentModel.PropertyDescriptor In list
        'Debug.Print(pe.DisplayName)
        'Next
#End If
        Me.Items.Clear()

        If list.Count > 0 Then
            ' If descValueProperty = Nothing We can't continue
            Dim descDisplayProperty As PropertyDescriptor = list(m_strDisplayMember)

            If descDisplayProperty Is Nothing Then
                Throw New Exception("DisplayMember '" + m_strDisplayMember + "' is unknown")
            End If
            ' If descValueProperty = Nothing We don't manage icons
            Dim descValueProperty As PropertyDescriptor = list(m_strValueMember)

            ' If descValueProperty = Nothing We don't manage icons
            Dim descToolTipProperty As PropertyDescriptor = list("ToolTipText")

            m_bindingSource.Position = 0

            Dim bContinue As Boolean = (m_bindingSource.Count > 0)
            While bContinue
                Dim strName As String() = CType(descDisplayProperty.GetValue(m_bindingSource.Current), String())
                Dim iIcon As Integer = 0

                If descValueProperty IsNot Nothing Then
                    iIcon = CType(descValueProperty.GetValue(m_bindingSource.Current), Integer)
                End If
                lvitem = New ListViewItem(strName, iIcon)
                lvitem.Tag = m_bindingSource.Current

                'Dim xkm As XmlProjectMemberView = CType(m_bindingSource.Current, XmlProjectMemberView)
                'Debug.Print(xkm.ToString + ":=" + Str(xkm.Tag))

                If descToolTipProperty IsNot Nothing Then
                    lvitem.ToolTipText = CType(descToolTipProperty.GetValue(m_bindingSource.Current), String)
                End If

                Items.Add(lvitem)
                'Items.Add(New ListViewItem(New String() {"titre1", "moi", "2208"}, 0))

                If m_bindingSource.Position + 1 < m_bindingSource.Count Then
                    m_bindingSource.Position = m_bindingSource.Position + 1
                Else
                    bContinue = False
                End If
            End While
        End If
    End Sub

    Private Sub DataListView_MouseDoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseDoubleClick
        If Me.SelectedItem IsNot Nothing Then
            OnItemClick(True)
        End If
    End Sub

    Private Sub DataListView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SelectedIndexChanged
        If Me.SelectedItem IsNot Nothing Then
            OnItemClick(False)
        Else
            Me.TimerEmptyZone.Start()
        End If
    End Sub

    Private Sub OnItemClick(ByVal bDoubleClick As Boolean)
        If bDoubleClick Then
            RaiseEvent ItemDoubleClick(Me, New DataListViewEventArgs(Me.SelectedIndex))
        Else
            RaiseEvent ItemClick(Me, New DataListViewEventArgs(Me.SelectedIndex))
        End If
    End Sub

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.TimerEmptyZone = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'TimerEmptyZone
        '
        Me.TimerEmptyZone.Interval = 10
        '
        'DataListView
        '
        Me.AllowDrop = True
        Me.LabelEdit = True
        Me.MultiSelect = False
        Me.ResumeLayout(False)

    End Sub

    Private Sub TimerEmptyZone_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TimerEmptyZone.Tick

        Me.TimerEmptyZone.Stop()

        If Me.SelectedItem Is Nothing Then
            OnEmptyZoneClick()
        End If
    End Sub
#End Region

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
    End Sub

    Protected Sub OnEmptyZoneClick()
        RaiseEvent EmptyZoneClick(Me, Nothing)
    End Sub
End Class

''' <summary>
''' Provides data for DataListView events related to item operations. 
''' </summary>
''' <remarks></remarks>
Public Class DataListViewEventArgs
    Inherits LabelEditEventArgs

    Public Sub New(Optional ByVal value As Integer = -1)
        MyBase.New(value)
    End Sub

    Public Sub New(ByVal value As LabelEditEventArgs)
        MyBase.New(value.Item, value.Label)
    End Sub
End Class

