Public Class UmlMemberValue : Inherits System.ComponentModel.StringConverter

    Dim m_astrValue As String() = New String() {"val", "ref"}

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
        Return New StandardValuesCollection(m_astrValue)
    End Function

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function
End Class
