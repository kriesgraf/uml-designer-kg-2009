Imports System.Xml
Imports ClassXmlProject.XmlProjectTools
Imports ClassXmlProject.UmlCodeGenerator

Public Class XmlClassListView
    Inherits XmlComponent
    Implements IComparer

    Public Const cstValueMember As String = "Id"
    Public Const cstDisplayMember As String = "FullpathClassName"


    Public Property Id() As String
        Get
            Return GetAttribute("id")
        End Get
        Set(ByVal value As String)
            SetAttribute("id", value)
        End Set
    End Property

    Public ReadOnly Property FullpathClassName() As String
        Get
            Return GetFullpathDescription(Me.Node, CType(Me.Tag, ELanguage))
        End Get
    End Property

    Public Sub New(ByVal nodeXml As XmlNode)
        MyBase.New(nodeXml)
    End Sub

    Public Shared Sub AddListComboBoxColumn(ByVal DataPropertyName As String, _
                                          ByVal HeaderText As String, _
                                          ByVal column As DataGridViewColumn, _
                                          ByVal xmlDocument As XmlDocument, _
                                          ByVal eTag As ELanguage)
        Try
            With CType(column, DataGridViewComboBoxColumn)
                'Debug.Print("DataPropertyName=" + DataPropertyName + ";ValueMember=" + XmlClassListView.cstValueMember)
                .Name = "ControlName_" + DataPropertyName

                .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
                .DisplayStyleForCurrentCellOnly = True
                .DataPropertyName = DataPropertyName
                .HeaderText = HeaderText
                .MaxDropDownItems = 10
                .FlatStyle = FlatStyle.Flat

                .DataSource = GetListComponents(xmlDocument, eTag)
                .ValueMember = XmlClassListView.cstValueMember
                .DisplayMember = XmlClassListView.cstDisplayMember
            End With

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Shared Function GetListComponents(ByVal xmlDocument As XmlDocument, ByVal eTag As ELanguage) As ArrayList
        Dim listComponents As ArrayList = Nothing
        Try
            Dim list As XmlNodeList = xmlDocument.SelectNodes("//class | //reference[@type!='typedef']")

            If list Is Nothing Then
                Throw New Exception("Class list is empty in method AddClassList")
            End If

            Dim iterator As IEnumerator = list.GetEnumerator
            Dim nodeXml As XmlNode
            Dim xmlcpnt As XmlClassListView
            iterator.Reset()

            listComponents = New ArrayList

            While iterator.MoveNext
                nodeXml = CType(iterator.Current, XmlNode)
                xmlcpnt = New XmlClassListView(nodeXml)
                xmlcpnt.Tag = eTag
                'Debug.Print("GetListComponents:=" + xmlcpnt.Name + "(" + xmlcpnt.Id)
                listComponents.Add(xmlcpnt)
            End While

            listComponents.Sort(New XmlClassListView(Nothing))

        Catch ex As Exception
            Throw ex
        End Try
        Return listComponents
    End Function

    Public Shared Sub AddListComboBoxControl(ByVal control As ComboBox, ByVal xmlDocument As XmlDocument, ByVal eTag As ELanguage)
        Try
            With control
                .DropDownStyle = ComboBoxStyle.DropDownList

                .DataSource = GetListComponents(xmlDocument, eTag)
                .ValueMember = cstValueMember
                .DisplayMember = cstDisplayMember
            End With
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim str1 As String = CType(x, XmlClassListView).FullpathClassName
        Dim str2 As String = CType(y, XmlClassListView).FullpathClassName
        Return Comparer.DefaultInvariant.Compare(str1, str2)
    End Function
End Class
