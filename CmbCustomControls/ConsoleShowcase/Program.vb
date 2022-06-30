Imports System
Imports System.Threading

Module Program
    Sub Main(args As String())
        Console.WriteLine("A single progressbar without color...")
        Using p As New CmbConsoleControls.CmbProgressBar("Some process that is running")
            p.ColoredOutput = False
            For i As Integer = 0 To 100
                p.Report(i)
                Threading.Thread.Sleep(10)
            Next
        End Using
        Console.WriteLine()
        Console.WriteLine("A bunch of progressbars with color...")
        Console.WriteLine("Press C to cancel")

        Using pb As New CmbConsoleControls.CmbMultiBar
            Using cts As New CancellationTokenSource()
                Using cts2 As New CancellationTokenSource
                    Dim token As CancellationToken = cts.Token
                    Dim token2 As CancellationToken = cts2.Token
                    Dim alltasks(3) As Task
                    For y As Integer = 0 To alltasks.Length - 1
                        Dim pbx = pb.Add($"Progressbar {y}", True, True)
                        Dim sleeptime As Integer = Random.Shared.Next(10, 50)
                        alltasks(y) = Task.Run(Sub()
                                                   Dim localpbx = pbx
                                                   Dim localSleeptime = sleeptime
                                                   Threading.Thread.Sleep(100)
                                                   For i As Integer = 0 To 100
                                                       If token.IsCancellationRequested Then Exit Sub
                                                       pb.Report(i, localpbx)
                                                       Threading.Thread.Sleep(localSleeptime)
                                                   Next
                                               End Sub, token)
                    Next

                    Task.Run(Sub()
                                 Do
                                     While Not Console.KeyAvailable
                                         If token2.IsCancellationRequested Then Exit Sub
                                         Thread.Sleep(250)
                                     End While
                                 Loop While Console.ReadKey(True).KeyChar.ToString.ToUpperInvariant <> "C"
                                 cts.Cancel()
                             End Sub, token2)

                    Task.WaitAll(alltasks)
                    cts2.Cancel()

                    Console.SetCursorPosition(0, pb.MaxRow + 2)
                End Using
            End Using
        End Using
    End Sub
End Module
