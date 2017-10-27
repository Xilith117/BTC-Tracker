Public Class splash
    Public BuildNumber As Double = 1.09

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Close()
    End Sub

    Private Sub splash_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If My.Application.Info.Title <> "" Then
            ApplicationTitle.Text = My.Application.Info.Title
        Else
            ApplicationTitle.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If

        Version.Text = "Version: " & BuildNumber.ToString
        Copyright.Text = My.Application.Info.Copyright


        Form1.Show()
        Timer1.Enabled = True
        Form1.Opacity = 100
    End Sub

End Class