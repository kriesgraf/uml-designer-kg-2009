Imports System
Imports System.Windows.Forms
Imports System.Xml
Imports System.Collections
Imports Microsoft.VisualBasic

Public Class XmlRefRedundancyView
    Inherits XmlComponent

    Private m_xmlProjectNode As XmlComponent = Nothing
    Private m_strImportName As String = ""

    Public WriteOnly Property ProjectNode() As XmlComponent
        Set(ByVal value As XmlComponent)
            m_xmlProjectNode = value
        End Set
    End Property

    Public WriteOnly Property Redundant() As XmlNode
        Set(ByVal value As XmlNode)
            Me.Node = value
        End Set
    End Property

    Public WriteOnly Property ImportName() As String
        Set(ByVal value As String)
            m_strImportName = value
        End Set
    End Property

    Public Function UpdateRemainingList(ByVal listRemoved As ListBox, ByVal listRemained As ListBox, _
                                        ByVal strDisplayMember As String, ByVal ok_button As Button) As Boolean

        Dim RemovedArray As ArrayList = listRemoved.DataSource

        If RemovedArray Is Nothing Then Return False

        Dim RemainedArray As New ArrayList
        Dim copy As XmlNodeListView
        For Each child As XmlNodeListView In RemovedArray
            If child.CheckedView Then
                copy = New XmlNodeListView(child.Node)
                copy.GenerationLanguage = Me.GenerationLanguage
                RemainedArray.Add(copy)
            End If
        Next
        If RemovedArray.Count = RemainedArray.Count Or RemainedArray.Count = 0 Then
            listRemained.DataSource = Nothing
            listRemained.Items.Clear()
            listRemained.Items.Add("No replacement.")
            listRemained.Enabled = False
            ok_button.Enabled = False
        Else
            listRemained.Enabled = True
            listRemained.DataSource = Nothing
            listRemained.Items.Clear()
            listRemained.SelectionMode = SelectionMode.One
            listRemained.DisplayMember = strDisplayMember
            listRemained.ValueMember = "Id"
            listRemained.DataSource = RemainedArray
            ok_button.Enabled = True
        End If
    End Function

    Public Function UpdateValues(ByVal listRemoved As ListBox, ByVal listRemained As ListBox) As Boolean
        Dim bResult As Boolean = False

        If listRemained.SelectedIndex <> -1 Then
            With CType(listRemained.SelectedItem, XmlNodeListView)
                Dim myList As ArrayList = CType(listRemoved.DataSource, ArrayList)

                For Each child As XmlNodeListView In myList
                    If child.CheckedView = False Then
                        Select Case child.NodeName
                            Case "typedef", "property", "return"
                                If XmlProjectTools.ChangeTypeDesc(child.Node, .Id) Then
                                    bResult = True
                                End If
                            Case Else
                                Dim bHashTypedef As Boolean = (child.SelectNodes("typedef").Count > 0)
                                If .NodeName = "reference" And .IsClassNode And bHashTypedef Then
                                    If MsgBox("Can't replace object: '" + child.FullpathClassName + "'" + vbCrLf + _
                                              "With remaining reference: '" + .FullpathClassName + "'" + vbCrLf + _
                                              "Because '" + child.FullpathClassName + "' has 'typedef' children." + vbCrLf + _
                                              "Would you want to continue?", XmlProjectTools.cstMsgYesNoQuestion, "Remove redundancies") _
                                              = MsgBoxResult.No Then
                                        Exit For
                                    End If
                                ElseIf XmlProjectTools.ChangeClassIDs(child.Node, .Node) Then
                                    bResult = True
                                End If
                                If child.RemoveMe() Then
                                    bResult = True
                                End If
                        End Select
                    End If
                Next
            End With
        Else
            MsgBox("No replaced object selected, please select one or press 'Ignore'", MsgBoxStyle.Exclamation)
        End If

        Return bResult
    End Function

    Public Function LoadNodes(ByVal listbox As ListBox, Optional ByVal strDisplayMember As String = XmlNodeListView.cstFullUmlPathName, _
                              Optional ByVal bClear As Boolean = False) As Boolean

        Dim listResult As New ArrayList

        If bClear Then
            listbox.DataSource = Nothing
            listbox.Items.Clear()
        End If

        If XmlNodeListView.GetListRedundancies(m_xmlProjectNode, Me.Node, listResult) _
        Then
            ' We add current redundant node to propose user to choose wide list
            Dim xmlcpnt As XmlNodeListView = Nothing
            If m_strImportName = "" Then
                xmlcpnt = New XmlNodeListView(Me.Node)
            Else
                xmlcpnt = New XmlNodeListView("; file -->" + m_strImportName)
                xmlcpnt.Node = Me.Node
            End If

            xmlcpnt.GenerationLanguage = Me.GenerationLanguage
            listResult.Add(xmlcpnt)

            XmlNodeListView.SortNodeList(listResult)

            listbox.SelectionMode = SelectionMode.MultiSimple
            listbox.DisplayMember = strDisplayMember
            listbox.ValueMember = "Id"
            listbox.DataSource = listResult
            listbox.SetSelected(0, False)
            Return True
        Else
            listbox.Items.Add("No redundancy detected.")
            listbox.Enabled = False
        End If

        Return False
    End Function
End Class
