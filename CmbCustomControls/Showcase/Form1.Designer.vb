<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.CmbTrackBar1 = New CmbCustomControls.CmbTrackBar()
        Me.SuspendLayout()
        '
        'CmbTrackBar1
        '
        Me.CmbTrackBar1.BackColor = System.Drawing.SystemColors.Control
        Me.CmbTrackBar1.ButtonColor = System.Drawing.Color.DodgerBlue
        Me.CmbTrackBar1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.CmbTrackBar1.Location = New System.Drawing.Point(285, 174)
        Me.CmbTrackBar1.MaximumValue = 50
        Me.CmbTrackBar1.MinimumSize = New System.Drawing.Size(128, 55)
        Me.CmbTrackBar1.MinimumValue = 0
        Me.CmbTrackBar1.Name = "CmbTrackBar1"
        Me.CmbTrackBar1.ShowScale = False
        Me.CmbTrackBar1.ShowTicks = CmbCustomControls.Enums.Ticks.None
        Me.CmbTrackBar1.ShowTitle = False
        Me.CmbTrackBar1.Size = New System.Drawing.Size(672, 55)
        Me.CmbTrackBar1.TabIndex = 0
        Me.CmbTrackBar1.Text = "CmbTrackBar1"
        Me.CmbTrackBar1.Value = 25
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1180, 610)
        Me.Controls.Add(Me.CmbTrackBar1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CmbTrackBar1 As CmbCustomControls.CmbTrackBar
End Class
