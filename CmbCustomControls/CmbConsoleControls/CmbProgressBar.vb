Imports System.Text
Imports System.Threading
Public Class CmbProgressBar
    Implements IDisposable
    Implements IProgress(Of Integer)

#Region "Properties"
    Private MaxLength As Integer
    Public ReadOnly Property CurrentRow As Integer

    Private CurrentProgress As Integer
    Private internalBuilder As StringBuilder
    Private workingTitle As String

    Public Property ShowPercentage As Boolean = True
    Public Property ShowTitle As Boolean = True
    Private _Title As String
    Public Property Title As String
        Set(value As String)
            _Title = value

            If value.Length > 20 Then
                workingTitle = $"{value.Substring(0, 17)}..."
            Else
                workingTitle = value
            End If
        End Set
        Get
            Return _Title
        End Get
    End Property
#End Region

#Region "Init class"
    Sub New()
        internalBuilder = New StringBuilder
        MaxLength = Math.Min(60, Console.WindowWidth)
        _CurrentRow = Console.CursorTop
    End Sub
    Sub New(Title As String)
        MyClass.New
        Me.Title = Title
    End Sub
    Sub New(FixedRow As Integer)
        MyClass.New
        _CurrentRow = FixedRow
    End Sub
    Sub New(Title As String, FixedRow As Integer)
        MyClass.New
        Me.Title = Title
        _CurrentRow = FixedRow
    End Sub
#End Region

    Private Sub Build()
        'SyncLock internalBuilder
        internalBuilder.Clear()

        If ShowTitle Then
            internalBuilder.Append($"{workingTitle,20} [")
        Else
            internalBuilder.Append("["c)
        End If

        Dim MaxCols As Integer = MaxLength - internalBuilder.Length - 6
        Dim ColsFilled As Integer = Convert.ToInt32(Math.Min(((MaxCols / 100) * CurrentProgress), 100))
        Dim Spaces As Integer = MaxCols - ColsFilled

        internalBuilder.Append($"{New String("#"c, ColsFilled)}{New String(" "c, Spaces)}] ")

        If ShowPercentage Then
            internalBuilder.Append($"{CurrentProgress,3}%")
        End If

        Dim morespaces As Integer = Math.Max(0, Console.WindowWidth - internalBuilder.Length)

        internalBuilder.Append($"{New String(" "c, morespaces)}")

        'End SyncLock
    End Sub

    Private Sub WriteOutput()
        SyncLock CmbMultiBar.OutputClaim
            Console.SetCursorPosition(0, CurrentRow)
            Console.Write(internalBuilder.ToString)
        End SyncLock
    End Sub

    Public Sub Report(value As Integer) Implements IProgress(Of Integer).Report
        ' min 0 and max 100
        Dim normalized As Integer = Math.Max(0, Math.Min(100, value))
        If normalized <> CurrentProgress Then
            Interlocked.Exchange(CurrentProgress, normalized)
            Build()
            WriteOutput()
        End If
    End Sub

#Region "Dispose code"
    Private disposedValue As Boolean

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                If internalBuilder IsNot Nothing Then
                    internalBuilder.Clear()
                End If
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            ' TODO: set large fields to null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    ' Protected Overrides Sub Finalize()
    '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
