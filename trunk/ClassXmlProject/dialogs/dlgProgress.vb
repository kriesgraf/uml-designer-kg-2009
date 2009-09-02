Imports System.Windows.Forms

Public Class dlgProgress
    Implements InterfProgression

    Private m_bLoaded As Boolean = False
    Private m_bVisible As Boolean = False

    Public Sub Increment(ByVal value As Integer) Implements InterfProgression.Increment
        Me.ProgressBar1.Increment(value)
        Application.DoEvents()  ' To lose time to dispatch event
        Debug.Print("Step=" + Me.ProgressBar1.Value.ToString)
        System.Threading.Thread.Sleep(100)
    End Sub

    Public WriteOnly Property Log() As String Implements InterfProgression.Log
        Set(ByVal value As String)
            Label1.Text = value
        End Set
    End Property

    Public WriteOnly Property Maximum() As Integer Implements InterfProgression.Maximum
        Set(ByVal value As Integer)
            Me.ProgressBar1.Maximum = value
            Debug.Print("Maximum=" + value.ToString)
        End Set
    End Property

    Public WriteOnly Property Minimum() As Integer Implements InterfProgression.Minimum
        Set(ByVal value As Integer)
            Me.ProgressBar1.Minimum = value
            Me.ProgressBar1.Value = value
            Debug.Print("Minimum=" + value.ToString)
        End Set
    End Property

    Public WriteOnly Property ProgressBarVisible() As Boolean Implements InterfProgression.ProgressBarVisible
        Set(ByVal value As Boolean)
            If m_bLoaded = False Then
                Return
            End If
            If m_bVisible And value = False Then
                Me.Close()
            ElseIf value = True Then
                m_bVisible = True
            End If
        End Set
    End Property

    Private Sub dlgProgress_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_bLoaded = True
    End Sub
End Class