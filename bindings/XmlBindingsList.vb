Imports System
Imports System.Collections
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class XmlBindingsList

    Private m_BindingManager As ArrayList

    Public Sub Init()
        m_BindingManager = New ArrayList
    End Sub

    Public Sub UpdateValues()
        Try
            Dim iterator As IEnumerator = m_BindingManager.GetEnumerator()
            iterator.Reset()
            While iterator.MoveNext
                Dim binding As Binding = CType(iterator.Current, Binding)
                'Debug.Print("Control=" + binding.Control.Name + ";property=" + binding.PropertyName + ";Data=" + binding.BindingMemberInfo.BindingField)
                binding.WriteValue()
            End While

        Catch ex As Exception
            Throw ex
            'Finally
            '   m_BindingManager = Nothing
        End Try
    End Sub

    Public Sub ResetValues()
        Try
            Dim iterator As IEnumerator = m_BindingManager.GetEnumerator()
            iterator.Reset()
            While iterator.MoveNext
                Dim binding As Binding = CType(iterator.Current, Binding)
                '                Debug.Print("Control=" + binding.Control.Name + ";property=" + binding.PropertyName + ";Data=" + binding.BindingMemberInfo.BindingField)
                binding.ReadValue()
            End While

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function AddBinding(ByVal dataControl As Control, _
                                ByVal dataSource As Object, _
                                ByVal strDataMember As String, _
                                Optional ByVal strProperty As String = "Text", _
                                Optional ByVal updateMode As DataSourceUpdateMode = DataSourceUpdateMode.Never) _
                    As Binding

        Dim bindingResult As Binding = Nothing

        Try

#If DEBUG Then
            'Dim bds As New BindingSource
            'bds.DataSource = CType(dataSource, XmlComponent)
            'bds.DataMember = strDataMember
            'If dataSource IsNot Nothing Then Debug.Print(dataSource.ToString())
            'Dim list As System.ComponentModel.PropertyDescriptorCollection = bds.GetItemProperties(Nothing)
            'For Each pe As System.ComponentModel.PropertyDescriptor In list
            '    Debug.Print(pe.DisplayName)
            'Next
#End If
            bindingResult = dataControl.DataBindings.Add(strProperty, dataSource, strDataMember, False, updateMode)
            m_BindingManager.Add(bindingResult)
            If updateMode = DataSourceUpdateMode.Never Then bindingResult.ControlUpdateMode = ControlUpdateMode.Never
            bindingResult.DataSourceNullValue = ""
            bindingResult.NullValue = ""

        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + vbCrLf + ex.StackTrace)
        End Try
        Return bindingResult
    End Function

    Public Sub RemoveBinding(ByVal dataControl As Control, Optional ByVal strProperty As String = "")
        Dim binding As Binding = Nothing
        If dataControl.DataBindings.Count > 0 Then
            If strProperty = "" Then
                binding = dataControl.DataBindings(0)
                m_BindingManager.Remove(binding)
                dataControl.DataBindings.Remove(binding)
            Else
                For Each binding In dataControl.DataBindings
                    If binding.BindingMemberInfo.BindingMember = strProperty Then
                        m_BindingManager.Remove(binding)
                        dataControl.DataBindings.Remove(binding)
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub
End Class
