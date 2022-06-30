Public Class CmbMultiBar
    Implements IDisposable

    Private ProgressBars As New List(Of KeyValuePair(Of Integer, CmbProgressBar))
    Private disposedValue As Boolean
    Public Shared Property OutputClaim As New Object

    Public Function Add(Title As String, UseColor As Boolean, ShowPercentage As Boolean) As Integer
        Dim fixedRow As Integer = Console.CursorTop
        If ProgressBars.Count > 0 Then
            fixedRow = ProgressBars.Last.Value.CurrentRow + 1
        End If

        Dim pb As New CmbProgressBar(fixedRow) With {
            .ShowTitle = Title.Length > 0,
            .ShowPercentage = ShowPercentage,
            .ColoredOutput = UseColor,
            .Title = Title
        }

        ProgressBars.Add(New KeyValuePair(Of Integer, CmbProgressBar)(ProgressBars.Count + 1, pb))

        Return ProgressBars.Count
    End Function

    Public ReadOnly Property MaxRow As Integer
        Get
            Return ProgressBars.Max(Function(c) c.Value.CurrentRow)
        End Get
    End Property

    Public Sub Report(value As Integer, key As Integer)
        Dim pb = ProgressBars.FirstOrDefault(Function(c) c.Key = key).Value

        If pb IsNot Nothing Then
            pb.Report(value)
        End If
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                For Each itm In ProgressBars
                    itm.Value.Dispose()
                Next
                ProgressBars.Clear()
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
End Class