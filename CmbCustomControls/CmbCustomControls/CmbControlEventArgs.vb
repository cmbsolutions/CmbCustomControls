Public Class CmbControlEventArgs
    Inherits EventArgs

    Public ReadOnly Property x As Integer
    Public ReadOnly Property y As Integer
    Public ReadOnly Property z As Integer

    Sub New(x As Integer, y As Integer, z As Integer)
        _x = x
        _y = y
        _z = z
    End Sub

    Public Overrides Function ToString() As String
        Return $"X: {x}, Y: {y}, Z: {z}"
    End Function
End Class

Public Class CmbButtonEventArgs
    Inherits CmbControlEventArgs

    Public Property ButtonValue As Boolean

    Sub New()
        MyBase.New(0, 0, 0)
    End Sub

    Sub New(b As Boolean)
        MyBase.New(0, 0, 0)

        _buttonValue = b
    End Sub

    Public Overrides Function ToString() As String
        Return $"B: {ButtonValue}"
    End Function
End Class

Public Class CmbMessageEventArgs
    Inherits CmbControlEventArgs

    Sub New()
        MyBase.New(0, 0, 0)
    End Sub

    Sub New(m As String)
        MyBase.New(0, 0, 0)

        _message = m
    End Sub

    Public ReadOnly Property Message() As String
End Class