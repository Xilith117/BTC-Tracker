Imports System.ComponentModel
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.IO
Imports System.Drawing.Printing


Public Class PriceGraph
    'This is a test
    Public Shared Actual As New Series
    Public Shared MA As New Series
    Public Shared MA2 As New Series
    Public Shared SMI As New Series
    Public Shared SMI2 As New Series
    Public Shared bars As New Series
    Public Shared AllMAs As New Series
    Public Shared barsnumber As Integer = 0
    Public Shared AdditionalSeries As New List(Of Series)
    Public Shared lastprice As Decimal
    Public Shared circles As New Series
    Dim pd As New System.Drawing.Printing.PrintDocument()
    ' Add the event handler, and then print 

    Public Shared Sub AddMA(value As String)
        Dim newseries As New Series
        newseries.Name = value
        newseries.Color = RandomColor()
        newseries.ChartType = SeriesChartType.Line
        newseries.BorderWidth = 2
        AdditionalSeries.Add(newseries)

        Form1.chart1.Series.Add(AdditionalSeries(AdditionalSeries.Count - 1))

    End Sub

    Public Shared Function RandomColor()
        Dim final As Color
        Dim rand As New Random

        final = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256))
        Return final
    End Function



    Public Shared Sub Addpoint(Series As Series, Position As Double, Price As Double)
        '     Try https://www.marketvolume.com/


        Series.Points.AddXY(Position, Price)
        Series.Points.Last.ToolTip = Series.Points.Last.YValues(0) & "-" & Series.Points.Last.XValue
        If Form1.CheckBox2.Checked = False Then
            Form1.graphMax.Text = Actual.Points.Count - 1

        End If
        If Price <> lastprice Then

            bars.Points.AddXY(barsnumber, Price)
            '  txbars.Points.AddXY(barsnumber, Price - (Price * 0.0025))
            barsnumber += 1
        End If
      

        '  MA.Points.AddXY(Position, Algorithms.MA(40))
        MA2.Points.AddXY(Position, Algorithms.MA(Actual.Points.Count))

        If Actual.Points.Count >= 40 Then
            ' Try
            SMI.Points.AddXY(Position, Algorithms.SMI(40) * -1)
            SMI.Points.Last.ToolTip = SMI.Points.Last.YValues(0) & "-" & SMI.Points.Last.XValue

            If SMI.Points.Last.YValues(0) >= 50 Then

                If SMI.Points.Last.YValues(0) <= 255 * 2 Then
                    Actual.Points.Last.MarkerColor = Color.FromArgb(255, 255, SMI.Points.Last.YValues(0) - 255, 0)
                    Actual.Points.Last.MarkerSize = 2
                    Actual.Points.Last.MarkerStyle = MarkerStyle.Circle

                Else

                    Actual.Points.Last.MarkerColor = Color.FromArgb(255, 255, 0, 0)
                    Actual.Points.Last.MarkerSize = 2
                    Actual.Points.Last.MarkerStyle = MarkerStyle.Circle

                End If
            End If
            If SMI.Points.Last.YValues(0) <= -50 Then
                Actual.Points.Last.MarkerColor = Color.FromArgb(255, 0, 0, 255)
                Actual.Points.Last.MarkerSize = 2
                Actual.Points.Last.MarkerStyle = MarkerStyle.Circle

            End If


            SMI2.Points.AddXY(Position, ((SMI.Points.Last.YValues(0) * MA2.Points.Last.YValues(0)) * -1) * 10)
            ' Form1.TextBox3.Text = (SMI.Points.Last.YValues(0) * MA.Points.Last.YValues(0)) * 10
            Form1.Chart2.Titles.First.Text = SMI.Points.Last.YValues(0) / 100 & "%"
            'Catch ex As Exception
            '    Form1.ErrorLabel.Text = ex.ToString
            'End Try
        End If


        If AdditionalSeries.Count > 0 Then
            For Each item As Series In AdditionalSeries
                item.Points.AddXY(Position, Algorithms.MA(item.Name))
            Next
        End If


        Dim n As Decimal = Actual.Points.Last.YValues(0) + MA2.Points.Last.YValues(0)
        Dim nCount As Integer = 2

        For Each item As Series In AdditionalSeries
            n += item.Points.Last.YValues(0)
            nCount += 1
        Next
        n = n / nCount


        AllMAs.Points.AddXY(Position, n)



        lastprice = Actual.Points.Last.YValues(0)
        Form1.Chart1.Titles.First.Text = Collections.MarketSummaryList.Find(Function(item) item.MarketName.Contains(Form1.TextBox2.Text)).MarketName & "@" & Collections.MarketSummaryList.Find(Function(item) item.MarketName.Contains(Form1.TextBox2.Text)).Last
        ' Catch
        ' End Try
    End Sub



    Public Shared Sub LoadChart()



        Actual.Name = "Actual"

        'Change to a line graph.
        Actual.ChartType = SeriesChartType.Area
        Actual.BorderWidth = 2
        Actual.Color = Color.FromArgb(100, Color.Green)
        '  For index As Integer = 1 To 10
        form1.chart1.Series.Add(Actual)



        MA2.Name = "Ѻ"

        'Change to a line graph.
        MA2.ChartType = SeriesChartType.Line
        MA2.Color = Color.Yellow
        MA2.BorderWidth = 2
        '  For index As Integer = 1 To 10
        Form1.Chart1.Series.Add(MA2)

        AllMAs.Name = "ALL MAs"

        'Change to a line graph.
        AllMAs.ChartType = SeriesChartType.Line
        AllMAs.Color = Color.Lime
        AllMAs.BorderWidth = 2
        '  For index As Integer = 1 To 10
        Form1.Chart1.Series.Add(AllMAs)


        'MA.Name = "40"

        ''Change to a line graph.
        'MA.ChartType = SeriesChartType.Line
        'MA.Color = Color.Purple
        'MA.BorderWidth = 2
        ''  For index As Integer = 1 To 10
        'form1.chart1.Series.Add(MA)

       

        SMI.Name = "SMI"

        'Change to a line graph.
        SMI.ChartType = SeriesChartType.Line
        SMI.Color = Color.Red
        SMI.BorderWidth = 2
        '  For index As Integer = 1 To 10
        Form1.Chart2.Series.Add(SMI)
        SMI2.Name = "SMI2"

        'Change to a line graph.
        SMI2.ChartType = SeriesChartType.Line
        SMI2.Color = Color.White
        SMI2.BorderWidth = 2
        '  For index As Integer = 1 To 10
        Form1.Chart2.Series.Add(SMI2)

        bars.Name = "bars"
        bars.CustomProperties = "PointWidth = 0.9"
        'Change to a line graph.
        bars.ChartType = SeriesChartType.Column
        bars.Color = Color.Green
        bars.BorderWidth = 8
        '  For index As Integer = 1 To 10
        Form1.Chart3.Series.Add(bars)


        'bars.Name = "circles"

        ''Change to a line graph.
        'circles.ChartType = SeriesChartType.ErrorBar
        'circles.Color = Color.Red
        'circles.BorderWidth = 2
        ''  For index As Integer = 1 To 10
        'Form1.Chart1.Series.Add(circles)
    End Sub






End Class
