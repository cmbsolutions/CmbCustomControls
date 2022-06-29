Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Public Class CmbProgressBar
    Implements IDisposable
    Implements IProgress(Of Integer)

#Region "Properties"
    Private MaxLength As Integer
    Private Const MinLength As Integer = 30

    Public ReadOnly Property CurrentRow As Integer

    Private CurrentProgress As Integer
    Private internalBuilder As StringBuilder
    Private workingTitle As String

    Public Property ColoredOutput As Boolean = True
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
        MaxLength = Math.Min(70, Math.Max(MinLength, Console.WindowWidth))
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
        internalBuilder.Clear()
        Dim ignoreLength As Integer = 0

        If ShowTitle Then
            internalBuilder.Append($"{If(ColoredOutput, "<green>", "")}{workingTitle,-20} {If(ColoredOutput, "<yellow>", "")}[")
            If ColoredOutput Then ignoreLength = 15
        Else
            internalBuilder.Append($"{If(ColoredOutput, "<yellow>", "")}[")
            If ColoredOutput Then ignoreLength = 8
        End If

        Dim MaxCols As Integer = (MaxLength + ignoreLength) - internalBuilder.Length - 6
        Dim ColsFilled As Integer = Convert.ToInt32(Math.Min(((MaxCols / 100) * CurrentProgress), 100))
        Dim Spaces As Integer = MaxCols - ColsFilled

        internalBuilder.Append($"{New String("#"c, ColsFilled)}{New String(" "c, Spaces)}] ")

        If ShowPercentage Then
            internalBuilder.Append($"{If(ColoredOutput, "<cyan>", "")}{CurrentProgress,3}%{If(ColoredOutput, "<reset>", "")}")
        End If

        Dim morespaces As Integer = Math.Max(0, Console.WindowWidth - internalBuilder.Length)

        internalBuilder.Append($"{New String(" "c, morespaces)}")
    End Sub

    Private Sub WriteOutput()
        If ColoredOutput Then
            Dim RegexObj As New Regex("<(dark|)(red|green|blue|black|yellow|cyan|white|grey|magenta|reset)>([^<]+)", RegexOptions.IgnoreCase)
            Dim color As ConsoleColor
            SyncLock CmbMultiBar.OutputClaim
                Console.SetCursorPosition(0, CurrentRow)
                Dim MatchResults As Match = RegexObj.Match(internalBuilder.ToString)
                While MatchResults.Success
                    Dim i As Integer
                    For i = 1 To MatchResults.Groups.Count
                        Dim GroupObj As Group = MatchResults.Groups(i)
                        If GroupObj.Success Then
                            If GroupObj.Value = "reset" Then
                                Console.ResetColor()
                                Continue For
                            End If

                            If [Enum].TryParse(Of ConsoleColor)(GroupObj.Value, True, color) Then
                                Console.ForegroundColor = color
                                Continue For
                            End If

                            Console.Write(GroupObj.Value)
                        End If
                    Next
                    MatchResults = MatchResults.NextMatch()
                End While
            End SyncLock
        Else
            SyncLock CmbMultiBar.OutputClaim
                Console.SetCursorPosition(0, CurrentRow)
                Console.Write(internalBuilder.ToString)
            End SyncLock
        End If
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
