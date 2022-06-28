Imports System

Module Program
    Sub Main(args As String())
        Using pb As New CmbConsoleControls.CmbMultiBar
            Dim alltasks(3) As Task
            For y As Integer = 0 To 3
                Dim pbx = pb.Add($"Progressbar {y}", True, True)
                Dim sleeptime As Integer = Random.Shared.Next(50, 500)
                alltasks(y) = Task.Run(Sub()

                                           Threading.Thread.Sleep(100)
                                           Dim localSleeptime = sleeptime
                                           For i As Integer = 0 To 100
                                               pb.Report(i, pbx)
                                               Threading.Thread.Sleep(localSleeptime)
                                           Next
                                       End Sub)
            Next

            Task.WaitAll(alltasks)
        End Using
    End Sub
End Module
