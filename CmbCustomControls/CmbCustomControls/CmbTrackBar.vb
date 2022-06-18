Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Public Class CmbTrackBar

    Public Event OnValueChanged(sender As Object, e As CmbControlEventArgs)

    Private Delegate Sub OnKeyDownEventHandlerDelegate(sender As Object, e As KeyEventArgs)

    Private IsDragging As Boolean
    Private IsPressed As Boolean
    Private IsHot As Boolean


    Private _orientation As Orientation

    <Browsable(True), DefaultValue(Orientation.Horizontal), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property Orientation() As Orientation
        Get
            Return _orientation
        End Get
        Set(val As Orientation)
            _orientation = val

            If _orientation = Orientation.Horizontal Then
                Width = 128
                Height = 75
                MinimumSize = New Size(128, 75)
            Else
                Width = 55
                Height = 128
                MinimumSize = New Size(55, 128)
            End If
            Invalidate()
        End Set
    End Property

    Private _min As Integer
    <Browsable(True), DefaultValue(-100), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property MinimumValue() As Integer
        Get
            Return _min
        End Get
        Set(val As Integer)
            If val > _max Then
                Throw New ArgumentOutOfRangeException("Minimum", val, "Value greater then maximum")
            Else
                _min = val
                Invalidate()
            End If
        End Set
    End Property

    Private _max As Integer
    <Browsable(True), DefaultValue(100), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property MaximumValue() As Integer
        Get
            Return _max
        End Get
        Set(val As Integer)
            If val < _min Then
                Throw New ArgumentOutOfRangeException("Maximum", Value, "Value less then minimum")
            Else
                _max = val
                Invalidate()
            End If
        End Set
    End Property

    Private _value As Integer
    <Browsable(True), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property Value() As Integer
        Get
            Return _value
        End Get
        Set(val As Integer)
            _value = ConstrainValue(val)
            Invalidate()
            RaiseEvent OnValueChanged(Me, New CmbControlEventArgs(0, _value, 0))
        End Set
    End Property

    Private _incSmall As Integer
    <Browsable(True), DefaultValue(1), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property IncrementSmall() As Integer
        Get
            Return _incSmall
        End Get
        Set(val As Integer)
            _incSmall = val
        End Set
    End Property

    Private _incBig As Integer
    <Browsable(True), DefaultValue(10), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property IncrementBig() As Integer
        Get
            Return _incBig
        End Get
        Set(val As Integer)
            _incBig = val
        End Set
    End Property

    Private _ticks As Integer
    <Browsable(True), DefaultValue(20), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property Ticks() As Integer
        Get
            Return _ticks
        End Get
        Set(val As Integer)
            _ticks = val
            Invalidate(_sliderTickRect)
        End Set
    End Property

    Private _incSmallKey As Keys
    <Browsable(True), DefaultValue(Keys.Add), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property IncrementSmallKey() As Keys
        Get
            Return _incSmallKey
        End Get
        Set(val As Keys)
            _incSmallKey = val
        End Set
    End Property

    Private _decSmallKey As Keys
    <Browsable(True), DefaultValue(Keys.Subtract), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property DecrementSmallKey() As Keys
        Get
            Return _decSmallKey
        End Get
        Set(val As Keys)
            _decSmallKey = val
        End Set
    End Property

    Private _incBigKey As Keys
    <Browsable(True), DefaultValue(Keys.PageUp), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property IncrementBigKey() As Keys
        Get
            Return _incBigKey
        End Get
        Set(val As Keys)
            _incBigKey = val
        End Set
    End Property

    Private _decBigKey As Keys
    <Browsable(True), DefaultValue(Keys.PageDown), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property DecrementBigKey() As Keys
        Get
            Return _decBigKey
        End Get
        Set(val As Keys)
            _decBigKey = val
        End Set
    End Property

    Private Function ConstrainValue(val As Integer) As Integer
        Return Math.Min(Math.Max(val, MinimumValue), MaximumValue)
    End Function

    Private _showValue As Boolean
    <Browsable(True), DefaultValue(True), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property ShowValue() As Boolean
        Get
            Return _showValue
        End Get
        Set(val As Boolean)
            _showValue = val
            Invalidate()
        End Set
    End Property

    Private _showTitle As Boolean
    <Browsable(True), DefaultValue(True), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property ShowTitle() As Boolean
        Get
            Return _showTitle
        End Get
        Set(val As Boolean)
            _showTitle = val
            Invalidate()
        End Set
    End Property

    Private _showScale As Boolean
    <Browsable(True), DefaultValue(True), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property ShowScale() As Boolean
        Get
            Return _showScale
        End Get
        Set(val As Boolean)
            _showScale = val
            Invalidate()
        End Set
    End Property

    Private _showTicks As Enums.Ticks = Enums.Ticks.Both
    <Browsable(True), DefaultValue(Enums.Ticks.Both), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property ShowTicks() As Enums.Ticks
        Get
            Return _showTicks
        End Get
        Set(val As Enums.Ticks)
            _showTicks = val
            Invalidate()
        End Set
    End Property

    Private _backgroundColorLeft As Color = Color.LightGray
    <Browsable(True), DefaultValue(GetType(Color), "LightGray"), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property BackgroundColorLeft() As Color
        Get
            Return _backgroundColorLeft
        End Get
        Set(val As Color)
            _backgroundColorLeft = val
            Invalidate()
        End Set
    End Property

    Private _backgroundColorCenter As Color = Color.DimGray
    <Browsable(True), DefaultValue(GetType(Color), "DimGray"), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property BackgroundColorCenter() As Color
        Get
            Return _backgroundColorCenter
        End Get
        Set(val As Color)
            _backgroundColorCenter = val
            Invalidate()
        End Set
    End Property

    Private _backgroundColorRight As Color = Color.LightGray
    <Browsable(True), DefaultValue(GetType(Color), "LightGray"), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property BackgroundColorRight() As Color
        Get
            Return _backgroundColorRight
        End Get
        Set(val As Color)
            _backgroundColorRight = val
            Invalidate()
        End Set
    End Property

    Private _backgroundHeight As Integer = 3
    <Browsable(True), DefaultValue(3), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property BackgroundHeight() As Integer
        Get
            Return _backgroundHeight
        End Get
        Set(val As Integer)
            _backgroundHeight = Math.Min(Math.Max(val, 2), 20)
            Invalidate()
        End Set
    End Property

    Private _buttonColor As Color
    <Browsable(True), DefaultValue(GetType(Color), "Gray"), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property ButtonColor() As Color
        Get
            Return _buttonColor
        End Get
        Set(val As Color)
            _buttonColor = val
            Invalidate()
        End Set
    End Property

    Private _buttonStyle As Enums.TrackButtonStyle = Enums.TrackButtonStyle.Button
    <Browsable(True), DefaultValue(Enums.TrackButtonStyle.Button), Category("CmbTrackBar"), EditorBrowsable(EditorBrowsableState.Always)>
    Public Property ButtonStyle() As Enums.TrackButtonStyle
        Get
            Return _buttonStyle
        End Get
        Set(val As Enums.TrackButtonStyle)
            _buttonStyle = val
            Invalidate()
        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        SuspendLayout()

        ' Add any initialization after the InitializeComponent() call.
        _orientation = Orientation.Horizontal

        _titleRect = New Rectangle(0, 0, 128, 20)
        _scaleRect = New Rectangle(0, 55, 128, 20)
        _trackBarRect = New Rectangle(0, 20, 128, 35)

        _min = 0
        _max = 50
        _value = 0

        _incSmall = 1
        _incBig = 10
        _ticks = 20

        _backgroundColorLeft = Color.LightGray
        _backgroundColorCenter = Color.DimGray
        _backgroundColorRight = Color.LightGray
        _backgroundHeight = 3
        _buttonColor = Color.FromArgb(219, 219, 219)
        _buttonStyle = Enums.TrackButtonStyle.Button

        _showTitle = True
        _showValue = True
        _showScale = True
        _showTicks = Enums.Ticks.Both

        Text = "CmbTrackBar"

        Width = 128
        Height = 75
        MinimumSize = New Size(Width, Height)

        DoubleBuffered = True
        BackColor = SystemColors.Control
        ForeColor = SystemColors.WindowText

        _incSmallKey = Keys.Add
        _decSmallKey = Keys.Subtract
        _incBigKey = Keys.PageUp
        _decBigKey = Keys.PageDown

        ResumeLayout()
        Invalidate()
    End Sub

    Public Overrides Property MinimumSize As Size
        Get
            Return MyBase.MinimumSize
        End Get
        Set(val As Size)
            If _orientation = Orientation.Horizontal Then
                Dim y As Integer = 35
                If _showTitle Then y += 20
                If _showValue Then y += 20

                MyBase.MinimumSize = New Size(128, y)
            Else
                Dim y As Integer = 128
                Dim x As Integer = 35
                If _showTitle Then y += 20
                If _showValue Then x += 20

                MyBase.MinimumSize = New Size(x, y)
            End If
        End Set
    End Property

    Public Sub OnKeyDownEventHandler(sender As Object, e As KeyEventArgs)
        If InvokeRequired Then
            Dim d As New OnKeyDownEventHandlerDelegate(AddressOf OnKeyDownEventHandler)

            Invoke(d, Me, e)
        Else
            OnKeyDown(e)
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        Select Case e.KeyCode
            Case _incSmallKey
                Value += _incSmall
            Case _incBigKey
                Value += _incBig
            Case _decSmallKey
                Value -= _incSmall
            Case _decBigKey
                Value -= _incBig
        End Select

        e.Handled = True
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnBackColorChanged(e As EventArgs)
        MyBase.OnBackColorChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnForeColorChanged(e As EventArgs)
        MyBase.OnForeColorChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)
        MyBase.OnLeave(e)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        If _orientation = Orientation.Horizontal Then
            _titleRect = New Rectangle(0, 0, Width, 20)
            _trackBarRect = New Rectangle(0, 20, Width, 35)
            _scaleRect = New Rectangle(0, 55, Width, 20)
        Else
            _titleRect = New Rectangle(0, 0, Width, 20)
            _trackBarRect = New Rectangle(0, 20, 35, Height - 20)
            _scaleRect = New Rectangle(35, 20, 20, Height - 20)
        End If

        Invalidate()
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H2000000
            ' Turn on WS_EX_COMPOSITED
            Return cp
        End Get
    End Property

    Protected Overrides Sub OnMouseCaptureChanged(e As EventArgs)
        MyBase.OnMouseCaptureChanged(e)
        IsDragging = False
        IsHot = False
        IsPressed = False
        Invalidate()
    End Sub

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        MyBase.OnGotFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnLostFocus(e As EventArgs)
        MyBase.OnLostFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        IsDragging = e.Button = MouseButtons.Left

        If e.Button = MouseButtons.Left Then
            If _orientation = Orientation.Horizontal Then
                Value = GetTrackBarValue(e.X)
            Else
                Value = GetTrackBarValue(e.Y)
            End If
        Else
            If _sliderRect.Contains(e.X, e.Y) Then
                IsHot = True And Not IsPressed
            Else
                IsHot = False
            End If
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        If Enabled AndAlso Not Focused Then
            Focus()
        End If

        MyBase.OnMouseDown(e)

        If e.Button = MouseButtons.Left AndAlso _sliderRect.Contains(e.X, e.Y) Then
            IsPressed = True
            IsHot = False
        Else
            IsPressed = False
        End If

        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If e.Button = MouseButtons.Left Then
            If _orientation = Orientation.Horizontal Then
                Value = GetTrackBarValue(e.X)
            Else
                Value = GetTrackBarValue(e.Y)
            End If
        ElseIf e.Button = MouseButtons.Right Then
            ResetToZero()
        End If

        Invalidate(Rectangle.Round(_sliderTrackRect))
    End Sub

    Private Function GetTrackBarValue(position As Integer) As Integer
        Dim num As Single
        Dim num2 As Integer
        If _orientation = Orientation.Horizontal Then
            num = (CSng((position - _sliderTrackRect.X)) / CSng((_sliderTrackRect.Right - _sliderTrackRect.X)))
            num2 = (_min + CInt(((_max - _min) * num)))
        Else
            num = (1.0! - (CSng((position - _sliderTrackRect.Y)) / CSng((_sliderTrackRect.Bottom - _sliderTrackRect.Y))))
            num2 = (_min + CInt(((_max - _min) * num)))
        End If

        Return num2
    End Function

    Private Function GetSliderX(bound As RectangleF) As Single
        Dim actualValue As Integer
        Dim steps As Double
        Dim center As Single
        If _orientation = Orientation.Horizontal Then
            actualValue = Math.Abs(_min) + _value
            steps = bound.Width / (_max - _min)
            center = CSng(steps * actualValue) + bound.X
        Else
            actualValue = (_min + CInt((_max - _min) - _value))
            steps = bound.Height / (_max - _min)
            center = CSng(steps * actualValue) + bound.Y
        End If

        Return center
    End Function

    Private Sub ResetToZero()
        Value = Math.Max(_min, 0)
    End Sub

    Public Sub GoCenter()
        ResetToZero()
        Invalidate()
    End Sub

#Region "Title"
    Private _titleRect As Rectangle

    Private Sub DrawTitle(g As Graphics)
        If Not _showTitle Then Exit Sub
        Dim fnt As New Font(Font.FontFamily, 9, FontStyle.Bold, GraphicsUnit.Point)
        Dim t = g.MeasureString(Text, fnt)

        g.DrawString(Text, fnt, New SolidBrush(ForeColor), New PointF(CSng((_titleRect.Width / 2) - (t.Width / 2)), CSng((_titleRect.Height / 2) - (t.Height / 2))))
    End Sub
#End Region

    Private _trackBarRect As Rectangle
    Private _sliderTickRect As Rectangle
    Private _sliderTrackRect As RectangleF
    Private _sliderRect As RectangleF

#Region "Trackbar Horizontal"
    Private Sub DrawTrackbarH(g As Graphics, bound As Rectangle)
        Dim center As New PointF(CSng((bound.Width) / 2), CSng((bound.Height) / 2))

        g.FillRectangle(New SolidBrush(BackColor), bound)

        ' Init drawing parts
        ' Tick Rect
        Dim tRect As Rectangle = bound
        tRect.Inflate(-6, -2)
        _sliderTickRect = tRect

        If _showTicks <> Enums.Ticks.None Then
            ' Top/bottom ticks
            Dim numTicks As Integer = CInt(_ticks / 2)
            Dim num As Single = CSng(((tRect.Width / 2) / (numTicks - 1.0!)))

            Do While (numTicks > 0)
                Dim num2 As Single = (tRect.X + ((numTicks - 1) * num))
                ' Top ticks, left of center mirrored to right of center
                If _showTicks = Enums.Ticks.Both Or _showTicks = Enums.Ticks.Top Then
                    g.DrawLine(Pens.DarkGray, num2, tRect.Y, num2, tRect.Y + 2)
                    g.DrawLine(Pens.DarkGray, num2 + CSng(tRect.Width / 2), tRect.Y, num2 + CSng(tRect.Width / 2), tRect.Y + 2)
                End If

                ' Bottom ticks, left of center mirrored to right of center
                If _showTicks = Enums.Ticks.Both Or _showTicks = Enums.Ticks.Bottom Then
                    g.DrawLine(Pens.DarkGray, num2, tRect.Bottom - 2, num2, tRect.Bottom)
                    g.DrawLine(Pens.DarkGray, num2 + CSng(tRect.Width / 2), tRect.Bottom - 2, num2 + CSng(tRect.Width / 2), tRect.Bottom)
                End If

                numTicks -= 1
            Loop
        End If

        ' Top marker bounds
        If _showTicks = Enums.Ticks.Both Or _showTicks = Enums.Ticks.Top Then
            g.DrawLine(Pens.DarkGray, tRect.X, tRect.Y, tRect.X, tRect.Y + 3)
            g.DrawLine(Pens.DarkGray, center.X, tRect.Y, center.X, tRect.Y + 3)
            g.DrawLine(Pens.DarkGray, tRect.Right, tRect.Y, tRect.Right, tRect.Y + 3)
        End If

        ' Bottom marker bounds
        If _showTicks = Enums.Ticks.Both Or _showTicks = Enums.Ticks.Bottom Then
            g.DrawLine(Pens.DarkGray, tRect.X, tRect.Bottom - 3, tRect.X, tRect.Bottom)
            g.DrawLine(Pens.DarkGray, center.X, tRect.Bottom - 3, center.X, tRect.Bottom)
            g.DrawLine(Pens.DarkGray, tRect.Right, tRect.Bottom - 3, tRect.Right, tRect.Bottom)
        End If

        ' Center rect
        Dim cRect As Rectangle = bound
        cRect.Inflate(-6, -CInt((center.Y - CSng(_backgroundHeight / 2))))

        Dim gb As New LinearGradientBrush(cRect, BackColor, BackColor, 0, False)
        Dim cb As New ColorBlend With {
            .Positions = {0.0F, 0.5F, 1.0F},
            .Colors = {_backgroundColorLeft, _backgroundColorCenter, _backgroundColorRight}
        }
        gb.InterpolationColors = cb
        g.FillRectangle(gb, cRect)
        g.DrawLine(New Pen(BackColor), cRect.X, cRect.Y, cRect.Right, cRect.Bottom) ' TODO: check this, it was crect.x instead of crect.right

        ' SliderTrack rect
        _sliderTrackRect = bound
        _sliderTrackRect.Inflate(-6, -(center.Y - 11))
        Dim sCenter As Single = GetSliderX(_sliderTrackRect)
        _sliderRect = New RectangleF(sCenter - 5, _sliderTrackRect.Y, 10, _sliderTrackRect.Height)

        If ButtonStyle = Enums.TrackButtonStyle.Button Then
            RenderSliderButtonH(g, _sliderRect)
        Else
            RenderSliderArrowH(g, _sliderRect)
        End If
    End Sub

    Private Sub RenderSliderButtonH(g As Graphics, bound As RectangleF)
        ' Button Shades
        Dim b1 As Color = _buttonColor
        Dim borderColor As Color = SystemColors.ControlDarkDark

        If Focused Then
            borderColor = Color.Maroon
        End If

        If IsDragging Or IsPressed Then
            b1 = SystemColors.HotTrack
            borderColor = ControlPaint.Light(b1, -1)
        ElseIf IsHot Then
            b1 = SystemColors.Highlight
        End If

        Dim b2 As Color = ControlPaint.Light(b1, -0.7)
        Dim b3 As Color = ControlPaint.Light(b1, 0.95)
        Dim b4 As Color = ControlPaint.Light(b3, 0.75)

        Dim c1 As Color = ControlPaint.Light(b4, 0.7)
        Dim c2 As Color = ControlPaint.Light(c1, -0.8)
        Dim c3 As Color = ControlPaint.Light(c1, 1)
        Dim c4 As Color = ControlPaint.Light(c3, 1)

        Dim smoothingOrg As SmoothingMode = g.SmoothingMode
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Outer border
        Dim obRect As RectangleF = bound

        Dim r As Single = 3.0!
        Dim obtlArc As New RectangleF(obRect.X, obRect.Y, r, r)
        Dim obtrArc As New RectangleF(obRect.Right - r, obRect.Y, r, r)
        Dim obbrArc As New RectangleF(obRect.Right - r, obRect.Bottom - r, r, r)
        Dim obblArc As New RectangleF(obRect.X, obRect.Bottom - r, r, r)

        Dim obArc As New GraphicsPath
        obArc.AddArc(obtlArc, 180.0!, 90.0!) ' TopLeft
        obArc.AddArc(obtrArc, -90.0!, 90.0!) ' TopRight
        obArc.AddArc(obbrArc, 0.0!, 90.0!) ' BottomRight
        obArc.AddArc(obblArc, 90.0!, 90.0!) ' BottomLeft

        Dim obBrush As New LinearGradientBrush(obRect, borderColor, borderColor, 90, False)

        g.FillPath(obBrush, obArc)

        ' Inner border
        Dim ibRect As RectangleF = bound
        ibRect.Inflate(-1, -1)

        Dim ibtlArc As New RectangleF(ibRect.X, ibRect.Y, r, r)
        Dim ibtrArc As New RectangleF(ibRect.Right - r, ibRect.Y, r, r)
        Dim ibbrArc As New RectangleF(ibRect.Right - r, ibRect.Bottom - r, r, r)
        Dim ibblArc As New RectangleF(ibRect.X, ibRect.Bottom - r, r, r)

        Dim ibArc As New GraphicsPath
        ibArc.AddArc(ibtlArc, 180.0!, 90.0!) ' TopLeft
        ibArc.AddArc(ibtrArc, -90.0!, 90.0!) ' TopRight
        ibArc.AddArc(ibbrArc, 0.0!, 90.0!) ' BottomRight
        ibArc.AddArc(ibblArc, 90.0!, 90.0!) ' BottomLeft

        Dim ibBrush As New LinearGradientBrush(ibRect, BackColor, BackColor, 90, False)
        Dim ibc As New ColorBlend
        ibc.Positions = {0.0!, 0.5!, 0.51!, 1.0!}
        ibc.Colors = {c4, c3, c1, c2}

        ibBrush.InterpolationColors = ibc

        g.FillPath(ibBrush, ibArc)


        ' Background

        Dim bbRect As RectangleF = bound
        bbRect.Inflate(-2, -2)

        Dim bbtlArc As New RectangleF(bbRect.X, bbRect.Y, r, r)
        Dim bbtrArc As New RectangleF(bbRect.Right - r, bbRect.Y, r, r)
        Dim bbbrArc As New RectangleF(bbRect.Right - r, bbRect.Bottom - r, r, r)
        Dim bbblArc As New RectangleF(bbRect.X, bbRect.Bottom - r, r, r)

        Dim bbArc As New GraphicsPath
        bbArc.AddArc(bbtlArc, 180.0!, 90.0!) ' TopLeft
        bbArc.AddArc(bbtrArc, -90.0!, 90.0!) ' TopRight
        bbArc.AddArc(bbbrArc, 0.0!, 90.0!) ' BottomRight
        bbArc.AddArc(bbblArc, 90.0!, 90.0!) ' BottomLeft

        Dim bbBrush As New LinearGradientBrush(bbRect, BackColor, BackColor, 90, False)
        Dim bbc As New ColorBlend
        bbc.Positions = {0.0!, 0.5!, 0.51!, 1.0!}
        bbc.Colors = {b4, b3, b1, b2}

        bbBrush.InterpolationColors = bbc

        g.FillPath(bbBrush, bbArc)

        If _showValue Then
            Dim fnt As New Font("Segoe UI", 7, FontStyle.Regular, GraphicsUnit.Point, Nothing, True)
            Dim tcur As SizeF = g.MeasureString(Value.ToString, fnt)
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault
            Dim p As New GraphicsPath
            bbRect.Offset(-2.5!, 1)

            p.AddString(Value.ToString, New FontFamily("Segoe UI"), FontStyle.Regular, 7, bbRect, New StringFormat(StringFormatFlags.DirectionVertical))

            g.FillPath(Brushes.Black, p)
        End If

        g.SmoothingMode = smoothingOrg
    End Sub

    Private Sub RenderSliderArrowH(g As Graphics, bound As RectangleF)
        ' Button Shades
        Dim b1 As Color = _buttonColor

        Dim smoothingOrg As SmoothingMode = g.SmoothingMode
        g.SmoothingMode = SmoothingMode.AntiAlias

        Dim arrow As New GraphicsPath
        arrow.AddLine(bound.Left, bound.Top, bound.Right, bound.Top)
        arrow.AddLine(bound.Right, bound.Top, bound.Right, bound.Top + CInt(bound.Height / 2))
        arrow.AddLine(bound.Right, bound.Top + CInt(bound.Height / 2), bound.Right - CInt(bound.Width / 2), bound.Bottom)
        arrow.AddLine(bound.Right - CInt(bound.Width / 2), bound.Bottom, bound.Left, bound.Top + CInt(bound.Height / 2))
        arrow.AddLine(bound.Left, bound.Top + CInt(bound.Height / 2), bound.Left, bound.Top)

        Dim obBrush As New SolidBrush(ButtonColor)

        g.FillPath(obBrush, arrow)

        If _showValue Then
            Dim fnt As New Font(Font.FontFamily, 8, FontStyle.Bold, GraphicsUnit.Point, Nothing, True)
            Dim tcur = g.MeasureString(Value.ToString, fnt)
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault

            Dim p As New GraphicsPath
            bound.Offset(-2.5!, 1)
            p.AddString(Value.ToString, fnt.FontFamily, fnt.Style, fnt.Size, bound, New StringFormat(StringFormatFlags.DirectionVertical))

            g.FillPath(New SolidBrush(ForeColor), p)
        End If

        g.SmoothingMode = smoothingOrg
    End Sub
#End Region

#Region "Trackbar Vertical"
    Private Sub DrawTrackbarV(g As Graphics, bound As Rectangle)
        Dim center As New PointF(CSng((bound.Width) / 2), CSng((bound.Height) / 2))

        g.FillRectangle(New SolidBrush(BackColor), bound)

        ' Init drawing parts
        ' Tick Rect
        Dim tRect As Rectangle = bound
        tRect.Inflate(-2, -6)
        _sliderTickRect = tRect

        If _showTicks <> Enums.Ticks.None Then
            ' Top/bottom ticks
            Dim numTicks As Integer = CInt(_ticks / 2)
            Dim num As Single = CSng(((tRect.Height / 2) / (numTicks - 1.0!)))
            Do While (numTicks > 0)
                Dim num2 As Single = (tRect.Y + ((numTicks - 1) * num))
                ' Top ticks, left of center mirrored to right of center
                If _showTicks = Enums.Ticks.Both Or _showTicks = Enums.Ticks.Top Then
                    g.DrawLine(Pens.DarkGray, tRect.X, num2, tRect.X + 2, num2)
                    g.DrawLine(Pens.DarkGray, tRect.X, num2 + CSng(tRect.Height / 2), tRect.X + 2, num2 + CSng(tRect.Height / 2))
                End If

                ' Bottom ticks, left of center mirrored to right of center
                If _showTicks = Enums.Ticks.Both Or _showTicks = Enums.Ticks.Bottom Then
                    g.DrawLine(Pens.DarkGray, tRect.Right - 2, num2, tRect.Right, num2)
                    g.DrawLine(Pens.DarkGray, tRect.Right - 2, num2 + CSng(tRect.Height / 2), tRect.Right, num2 + CSng(tRect.Height / 2))
                End If
                numTicks -= 1
            Loop
        End If

        ' Top marker bounds
        If _showTicks = Enums.Ticks.Both Or _showTicks = Enums.Ticks.Top Then
            g.DrawLine(Pens.DarkGray, tRect.X, tRect.Y, tRect.X + 3, tRect.Y)
            g.DrawLine(Pens.DarkGray, tRect.X, CSng(tRect.Height / 2) + tRect.Y, tRect.X + 3, CSng(tRect.Height / 2) + tRect.Y)
            g.DrawLine(Pens.DarkGray, tRect.X, tRect.Bottom, tRect.X + 3, tRect.Bottom)
        End If

        ' Bottom marker bounds
        If _showTicks = Enums.Ticks.Both Or _showTicks = Enums.Ticks.Bottom Then
            g.DrawLine(Pens.DarkGray, tRect.Right - 3, tRect.Y, tRect.Right, tRect.Y)
            g.DrawLine(Pens.DarkGray, tRect.Right - 3, CSng(tRect.Height / 2) + tRect.Y, tRect.Right, CSng(tRect.Height / 2) + tRect.Y)
            g.DrawLine(Pens.DarkGray, tRect.Right - 3, tRect.Bottom, tRect.Right, tRect.Bottom)
        End If

        ' Center rect
        Dim cRect As Rectangle = bound
        cRect.Inflate(-CInt((center.X - CSng(_backgroundHeight / 2))), -6)

        Dim gb As New LinearGradientBrush(cRect, BackColor, BackColor, 90, False)
        Dim cb As New ColorBlend

        cb.Positions = {0.0F, 0.5F, 1.0F}
        cb.Colors = {_backgroundColorRight, _backgroundColorCenter, _backgroundColorLeft}
        gb.InterpolationColors = cb
        g.FillRectangle(gb, cRect)
        g.DrawLine(New Pen(BackColor), cRect.X, cRect.Y, cRect.Right, cRect.Y)

        ' SliderTrack rect
        _sliderTrackRect = bound
        _sliderTrackRect.Inflate(-(center.X - 11), -6)
        Dim sCenter As Single = GetSliderX(_sliderTrackRect)
        _sliderRect = New RectangleF(_sliderTrackRect.X, sCenter - 5, _sliderTrackRect.Width, 10)

        If ButtonStyle = Enums.TrackButtonStyle.Button Then
            RenderSliderButtonV(g, _sliderRect)
        Else
            RenderSliderArrowV(g, _sliderRect)
        End If
    End Sub

    Private Sub RenderSliderButtonV(g As Graphics, bound As RectangleF)
        ' Button Shades
        Dim b1 As Color = _buttonColor
        Dim borderColor As Color = SystemColors.ControlDarkDark

        If Focused Then
            borderColor = Color.Maroon
        End If

        If IsDragging Or IsPressed Then
            b1 = SystemColors.HotTrack
            borderColor = ControlPaint.Light(b1, -1)
        ElseIf IsHot Then
            b1 = SystemColors.Highlight
        End If

        Dim b2 As Color = ControlPaint.Light(b1, -0.7)
        Dim b3 As Color = ControlPaint.Light(b1, 0.95)
        Dim b4 As Color = ControlPaint.Light(b3, 0.75)

        Dim c1 As Color = ControlPaint.Light(b4, 0.7)
        Dim c2 As Color = ControlPaint.Light(c1, -0.8)
        Dim c3 As Color = ControlPaint.Light(c1, 1)
        Dim c4 As Color = ControlPaint.Light(c3, 1)

        Dim smoothingOrg As SmoothingMode = g.SmoothingMode
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Outer border
        Dim obRect As RectangleF = bound

        Dim r As Single = 3.0!
        Dim obtlArc As New RectangleF(obRect.X, obRect.Y, r, r)
        Dim obtrArc As New RectangleF(obRect.Right - r, obRect.Y, r, r)
        Dim obbrArc As New RectangleF(obRect.Right - r, obRect.Bottom - r, r, r)
        Dim obblArc As New RectangleF(obRect.X, obRect.Bottom - r, r, r)

        Dim obArc As New GraphicsPath
        obArc.AddArc(obtlArc, 180.0!, 90.0!) ' TopLeft
        obArc.AddArc(obtrArc, -90.0!, 90.0!) ' TopRight
        obArc.AddArc(obbrArc, 0.0!, 90.0!) ' BottomRight
        obArc.AddArc(obblArc, 90.0!, 90.0!) ' BottomLeft

        Dim obBrush As New LinearGradientBrush(obRect, borderColor, borderColor, 0, False)

        g.FillPath(obBrush, obArc)

        ' Inner border
        Dim ibRect As RectangleF = bound
        ibRect.Inflate(-1, -1)

        Dim ibtlArc As New RectangleF(ibRect.X, ibRect.Y, r, r)
        Dim ibtrArc As New RectangleF(ibRect.Right - r, ibRect.Y, r, r)
        Dim ibbrArc As New RectangleF(ibRect.Right - r, ibRect.Bottom - r, r, r)
        Dim ibblArc As New RectangleF(ibRect.X, ibRect.Bottom - r, r, r)

        Dim ibArc As New GraphicsPath
        ibArc.AddArc(ibtlArc, 180.0!, 90.0!) ' TopLeft
        ibArc.AddArc(ibtrArc, -90.0!, 90.0!) ' TopRight
        ibArc.AddArc(ibbrArc, 0.0!, 90.0!) ' BottomRight
        ibArc.AddArc(ibblArc, 90.0!, 90.0!) ' BottomLeft

        Dim ibBrush As New LinearGradientBrush(ibRect, BackColor, BackColor, 0, False)
        Dim ibc As New ColorBlend
        ibc.Positions = {0.0!, 0.5!, 0.51!, 1.0!}
        ibc.Colors = {c4, c3, c1, c2}

        ibBrush.InterpolationColors = ibc

        g.FillPath(ibBrush, ibArc)


        ' Background

        Dim bbRect As RectangleF = bound
        bbRect.Inflate(-2, -2)

        Dim bbtlArc As New RectangleF(bbRect.X, bbRect.Y, r, r)
        Dim bbtrArc As New RectangleF(bbRect.Right - r, bbRect.Y, r, r)
        Dim bbbrArc As New RectangleF(bbRect.Right - r, bbRect.Bottom - r, r, r)
        Dim bbblArc As New RectangleF(bbRect.X, bbRect.Bottom - r, r, r)

        Dim bbArc As New GraphicsPath
        bbArc.AddArc(bbtlArc, 180.0!, 90.0!) ' TopLeft
        bbArc.AddArc(bbtrArc, -90.0!, 90.0!) ' TopRight
        bbArc.AddArc(bbbrArc, 0.0!, 90.0!) ' BottomRight
        bbArc.AddArc(bbblArc, 90.0!, 90.0!) ' BottomLeft

        Dim bbBrush As New LinearGradientBrush(bbRect, BackColor, BackColor, 0, False)
        Dim bbc As New ColorBlend
        bbc.Positions = {0.0!, 0.5!, 0.51!, 1.0!}
        bbc.Colors = {b4, b3, b1, b2}

        bbBrush.InterpolationColors = bbc

        g.FillPath(bbBrush, bbArc)

        If _showValue Then
            Dim fnt As New Font("Segoe UI", 6, FontStyle.Regular, GraphicsUnit.Point, Nothing, True)
            Dim tcur As SizeF = g.MeasureString(Value.ToString, fnt)
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            g.DrawString(Value.ToString, fnt, Brushes.Black, New PointF(CSng(bbRect.X + ((bbRect.Width / 2) - (tcur.Width / 2))), CSng(bbRect.Y + ((bbRect.Height / 2) - (tcur.Height / 2)) + 1)))
        End If
        g.SmoothingMode = smoothingOrg
    End Sub

    Private Sub RenderSliderArrowV(g As Graphics, bound As RectangleF)
        ' Button Shades
        Dim b1 As Color = _buttonColor

        Dim smoothingOrg As SmoothingMode = g.SmoothingMode
        g.SmoothingMode = SmoothingMode.AntiAlias

        Dim arrow As New GraphicsPath
        arrow.AddLine(bound.Left, bound.Top, bound.Right, bound.Top)
        arrow.AddLine(bound.Right, bound.Top, bound.Right, bound.Top + CInt(bound.Height / 2))
        arrow.AddLine(bound.Right, bound.Top + CInt(bound.Height / 2), bound.Right - CInt(bound.Width / 2), bound.Bottom)
        arrow.AddLine(bound.Right - CInt(bound.Width / 2), bound.Bottom, bound.Left, bound.Top + CInt(bound.Height / 2))
        arrow.AddLine(bound.Left, bound.Top + CInt(bound.Height / 2), bound.Left, bound.Top)

        Dim rotateMatrix As New Matrix
        rotateMatrix.RotateAt(-90, New PointF(bound.Left, bound.Top))
        arrow.Transform(rotateMatrix)

        Dim obBrush As New SolidBrush(ButtonColor)

        g.FillPath(obBrush, arrow)

        If _showValue Then
            Dim fnt As New Font(Font.FontFamily, 8, FontStyle.Bold, GraphicsUnit.Point, Nothing, True)
            Dim tcur = g.MeasureString(Value.ToString, fnt)
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            g.DrawString(Value.ToString, fnt, New SolidBrush(ForeColor), New PointF(CSng(bound.X + ((bound.Width / 2) - (tcur.Width / 2))), CSng(bound.Y + ((bound.Height / 2) - (tcur.Height / 2)) + 1)))
        End If

        g.SmoothingMode = smoothingOrg
    End Sub
#End Region

#Region "Scale"
    Private _scaleRect As Rectangle

    Private Sub DrawScaleH(g As Graphics)
        If Not _showScale Then Exit Sub
        Dim fnt As New Font(Font.FontFamily, 8, FontStyle.Regular, GraphicsUnit.Point)

        Dim tmin As SizeF = g.MeasureString(_min.ToString, fnt)
        Dim tcur As SizeF = g.MeasureString(CInt((_min + _max) / 2).ToString, fnt)
        Dim tmax As SizeF = g.MeasureString(_max.ToString, fnt)

        g.DrawString(_min.ToString, fnt, New SolidBrush(ForeColor), New PointF(CSng((_scaleRect.X + 4)), CSng(_scaleRect.Y + ((_scaleRect.Height / 2) - (tmin.Height / 2)))))
        g.DrawString(CInt((_min + _max) / 2).ToString, fnt, New SolidBrush(ForeColor), New PointF(CSng((_scaleRect.Width / 2) - (tcur.Width / 2)), CSng(_scaleRect.Y + ((_scaleRect.Height / 2) - (tcur.Height / 2)))))
        g.DrawString(_max.ToString, fnt, New SolidBrush(ForeColor), New PointF(CSng(_scaleRect.Right - 4 - tmax.Width), CSng(_scaleRect.Y + ((_scaleRect.Height / 2) - (tmax.Height / 2)))))
    End Sub

    Private Sub DrawScaleV(g As Graphics)
        If Not _showScale Then Exit Sub
        Dim fnt As New Font("Segoe UI", 8, FontStyle.Regular, GraphicsUnit.Point)

        Dim tmax As SizeF = g.MeasureString(_max.ToString, fnt)

        g.DrawString(_max.ToString, fnt, New SolidBrush(ForeColor), New PointF(CSng(_scaleRect.X + 1), CSng(_scaleRect.Y)))
        g.DrawString(CInt((_min + _max) / 2).ToString, fnt, New SolidBrush(ForeColor), New PointF(CSng(_scaleRect.X + 1), CSng((_scaleRect.Height / 2) + (_scaleRect.Y / 2))))
        g.DrawString(_min.ToString, fnt, New SolidBrush(ForeColor), New PointF(CSng(_scaleRect.X + 1), CSng(_scaleRect.Bottom - tmax.Height)))
    End Sub
#End Region

#Region "Value"
    Private _valueRect As RectangleF

    Private Sub DrawValueH(g As Graphics)
        If Not _showValue Then Exit Sub
        Dim fnt As New Font("Segoe UI", 8, FontStyle.Regular, GraphicsUnit.Point)

        Dim tcur As SizeF = g.MeasureString(Value.ToString, fnt)
        _valueRect = _sliderRect

        _valueRect.Offset(_sliderRect.Width + 2, 0)

        g.DrawString(Value.ToString, fnt, New SolidBrush(ForeColor), New PointF(CSng((_valueRect.Width / 2) - (tcur.Width / 2)), CSng(_valueRect.Y + ((_valueRect.Height / 2) - (tcur.Height / 2)))))
    End Sub

    Private Sub DrawValueV(g As Graphics)
        If Not _showValue Then Exit Sub
        Dim fnt As New Font("Segoe UI", 8, FontStyle.Regular, GraphicsUnit.Point, Nothing, True)

        Dim tmax As SizeF = g.MeasureString(_max.ToString, fnt)
        _valueRect = _sliderRect

        _valueRect.Offset(0, _sliderRect.Height + 2)

        g.DrawString(Value.ToString, fnt, New SolidBrush(ForeColor), New PointF(CSng(_valueRect.X + 1), CSng((_valueRect.Height / 2) + (_valueRect.Y / 2))))

    End Sub
#End Region

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g = e.Graphics
        g.Clear(BackColor)

        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

        DrawTitle(g)

        If _orientation = Orientation.Horizontal Then
            DrawTrackbarH(g, _trackBarRect)
            DrawScaleH(g)
        Else
            DrawTrackbarV(g, _trackBarRect)
            DrawScaleV(g)
        End If
    End Sub

End Class
