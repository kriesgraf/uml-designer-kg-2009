﻿Imports System.ComponentModel

Public Class UmlMemberVisibility
    Inherits StringConverter

    Dim m_astrVisibility As String() = New String() {"no", "private", "protected", "public"}

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
        Return New StandardValuesCollection(m_astrVisibility)
    End Function

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function
End Class
