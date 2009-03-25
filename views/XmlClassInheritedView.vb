Imports System.Xml
Imports System.Windows.Forms
Imports ClassXmlProject.XmlProjectTools

Public Class XmlClassInheritedView
    Inherits XmlInheritSpec
    Implements InterfGridViewNotifier
    Implements InterfViewControl
    Implements InterfObject

    Private m_xmlClassView As XmlClassGlobalView
    Protected Const cstFullpathClassName As String = "ClassName"

    Public ReadOnly Property ClassName() As String
        Get
            Return MyBase.Name
        End Get
    End Property

    Public ReadOnly Property ImplementationView() As String
        Get
            Return ConvertEnumImplToView(MyBase.Implementation)
        End Get
    End Property

    Public Property InterfObject() As Object Implements InterfObject.InterfObject
        Get
            Return m_xmlClassView
        End Get
        Set(ByVal value As Object)
            m_xmlClassView = TryCast(value, XmlClassGlobalView)
        End Set
    End Property

    Public Function EventClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventClick
        Return False
    End Function

    Public Function EventDoubleClick(ByVal dataMember As String) As Boolean Implements InterfGridViewNotifier.EventDoubleClick
        Dim xmlcpnt As XmlSuperClassView = CType(XmlNodeManager.GetInstance().CreateView(m_xmlClassView.Node, "class_superclass_view", m_xmlClassView.Document), XmlSuperClassView)
        xmlcpnt.CurrentImplementation = m_xmlClassView.CurrentClassImpl()

        Dim fen As Form = xmlcpnt.CreateForm(xmlcpnt)
        fen.ShowDialog()
        Return CType(fen.Tag, Boolean)
    End Function

    Public Sub InitControl(ByVal control As System.Windows.Forms.Control) Implements InterfViewControl.InitControl
        Dim data As XmlDataGridView = CType(control, XmlDataGridView)

        With data
            .Columns.Clear()
            .AllowDrop = True
            .ColumnDragStart = 0   ' Name column
            .AutoGenerateColumns = False
            .AllowUserToDeleteRows = False
            .AllowUserToAddRows = False
            .AllowUserToResizeRows = True
            .AllowUserToOrderColumns = True
        End With

        Dim col1 As DataGridViewColumn = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .ReadOnly = True
            .DataPropertyName = cstFullpathClassName
            .HeaderText = "Name"
            .Name = "ControlName_Name"
        End With
        data.Columns.Add(col1)

        col1 = New DataGridViewTextBoxColumn
        With col1
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .ReadOnly = True
            .DataPropertyName = "ImplementationView"
            .HeaderText = "Implementation"
            .Name = "ControlName_Implementation"
        End With
        data.Columns.Add(col1)

        Dim col2 As DataGridViewComboBoxColumn = New DataGridViewComboBoxColumn
        With col2
            .DisplayStyleForCurrentCellOnly = True
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .DataPropertyName = "Range"
            .HeaderText = "Visibility"
            .Name = "ControlName_Range"
            .Items.AddRange(New Object() {"private", "protected", "public"})
        End With
        data.Columns.Add(col2)
    End Sub

    Public Function Compare(ByVal nodeName As String) As Integer Implements InterfViewControl.Compare
        Return 0
    End Function

    Public Function CanDropItem(ByVal component As XmlComponent) As Boolean Implements InterfGridViewNotifier.CanDropItem
        If component.NodeName = "inherited" Then
            Me.DropInsertComponent(component)
            Return True
        End If
        Return False
    End Function

    Public Function CanDragItem() As Boolean Implements InterfGridViewNotifier.CanDragItem
        Return True
    End Function
End Class
