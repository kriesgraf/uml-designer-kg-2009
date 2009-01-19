Public Class XmlBindingCombo

    Private m_Control As ComboBox
    Private m_strDataText As String
    Private m_strDataValue As String
    Private m_xmlDataSource As XmlComponent

    Public ReadOnly Property Control() As ComboBox
        Get
            Return m_Control
        End Get
    End Property

    Public Sub New(ByVal combo As ComboBox, ByVal _DataSource As Object, _
                   ByVal _strDataText As String, ByVal _strDataValue As String)

        m_Control = combo
        m_Control.DropDownStyle = ComboBoxStyle.DropDown
        m_strDataText = _strDataText
        m_strDataValue = _strDataValue
        m_xmlDataSource = _DataSource

        Dim bds As BindingSource = New BindingSource
        bds.DataSource = _DataSource
        Dim list As System.ComponentModel.PropertyDescriptorCollection = bds.GetItemProperties(Nothing)
#If DEBUG Then
        'For Each pd As System.ComponentModel.PropertyDescriptor In list
        '    Debug.Print(pd.DisplayName + ", " + pd.Description)
        'Next
#End If
        Dim propDescriptor As System.ComponentModel.PropertyDescriptor = list(m_strDataText)
        Dim strValue As String = Nothing
        If propDescriptor IsNot Nothing Then
            strValue = TryCast(propDescriptor.GetValue(bds.Current), String)
            If strValue IsNot Nothing Then
                m_Control.SelectedIndex = -1
                m_Control.Text = strValue
            Else
                propDescriptor = list(m_strDataValue)
                If propDescriptor IsNot Nothing Then
                    strValue = TryCast(propDescriptor.GetValue(bds.Current), String)
                    If strValue IsNot Nothing Then
                        m_Control.SelectedValue = strValue
                    Else
                        m_Control.Text = Nothing
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub Update()
        Try
            If m_Control.Enabled = False Then Exit Sub

            Dim bSelectedText As Boolean
            Debug.Print("-------------------------------")
            Debug.Print("SelectedIndex=" + m_Control.SelectedIndex.ToString)
            If m_Control.SelectedItem IsNot Nothing Then Debug.Print("SelectedItem=" + m_Control.SelectedItem.ToString)
            Debug.Print("SelectedText=" + m_Control.SelectedText)
            If m_Control.SelectedValue IsNot Nothing Then Debug.Print("SelectedValue=" + m_Control.SelectedValue.ToString)
            Debug.Print("Text=" + m_Control.Text)

            If m_Control.SelectedIndex = -1 _
            Then
                bSelectedText = True

            ElseIf m_Control.SelectedValue Is Nothing _
            Then
                bSelectedText = True

            ElseIf CType(m_Control.SelectedValue, String) = XmlNodeListView.cstNullElement _
            Then
                bSelectedText = True
            End If

            Dim bds As BindingSource = New BindingSource
            bds.DataSource = m_xmlDataSource
            Dim list As System.ComponentModel.PropertyDescriptorCollection = bds.GetItemProperties(Nothing)
#If DEBUG Then
            'Debug.Print("====================== " + m_xmlDataSource.ToString + " ==================================")
            'For Each pd As System.ComponentModel.PropertyDescriptor In list
            '    If pd.GetValue(bds.Current) IsNot Nothing Then
            '        Debug.Print(pd.DisplayName + ":=" + pd.GetValue(bds.Current).ToString + ", " + pd.Description)
            '    Else
            '        Debug.Print(pd.DisplayName + ":=Nothing, " + pd.Description)
            '    End If
            'Next
#End If
            Dim propDescriptor As System.ComponentModel.PropertyDescriptor = Nothing

            If bSelectedText _
            Then
                propDescriptor = list(m_strDataText)
                propDescriptor.SetValue(bds.Current, m_Control.Text)
            Else
                propDescriptor = list(m_strDataValue)
                propDescriptor.SetValue(bds.Current, m_Control.SelectedValue)
            End If
        Catch ex As Exception
            Throw New Exception("Update fails, see inner exception.", ex)
        End Try
    End Sub
End Class
