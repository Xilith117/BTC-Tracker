Imports System.Windows.Forms.DataVisualization.Charting
Public Class Algorithms
    Public Shared Function MA(n As Integer)

        Dim Prices As New List(Of Decimal)
        Dim ReturnedMA As Decimal

        If PriceGraph.Actual.Points.Count < n Then
            n = PriceGraph.Actual.Points.Count
        End If



        For b As Integer = 0 To n - 1
            Prices.Add(PriceGraph.Actual.Points.Item(PriceGraph.Actual.Points.IndexOf(PriceGraph.Actual.Points.Last) - b).YValues.First)
        Next

        For i = 0 To Prices.Count - 1
                ReturnedMA += Prices.Item(i)
            Next

        ReturnedMA /= n

        ' Form2.TextBox1.Text = (SMI(n))



        Return ReturnedMA

        '     MsgBox("")

    End Function



    Public Shared Function SMI(n As Integer)


        If PriceGraph.Actual.Points.Count < n Then
            n = PriceGraph.Actual.Points.Count
        End If

        Dim ma2() As Double = PriceGraph.MA2.Points.Last.YValues
        Dim ReturnedSMI As Decimal
        Dim Prices As New List(Of Decimal)
        Dim MidpointDelta As Decimal
        '  Dim NSeries As New Series
        Dim HighestHigh As Decimal
        Dim LowestLow As Decimal
        Dim Close As Decimal
        Dim DEMA As Decimal 'double exponential moving average
        Dim DHL As Decimal

        For b As Integer = 0 To n - 1
            Prices.Add(PriceGraph.Actual.Points.Item(PriceGraph.Actual.Points.IndexOf(PriceGraph.Actual.Points.Last) - b).YValues.First)

        Next

        If HighestHigh = Nothing Or LowestLow = Nothing Then
            HighestHigh = Prices.First
            LowestLow = Prices.First
        End If
        For Each item As Double In Prices
            If item > HighestHigh Then
                HighestHigh = item
            End If

            If item < LowestLow Then
                LowestLow = item
            End If
        Next
        For Each item As MarketSummary In Collections.MarketSummaryList
            If item.MarketName = Form1.TextBox2.Text Then
                Close = item.PrevDay

            End If
        Next

        MidpointDelta = Math.Round(Close - (HighestHigh + LowestLow) / 2, 8)


        DEMA = ma2(0) * ((ma2(0) * MidpointDelta))

        DHL = ma2(0) * (ma2(0) * (HighestHigh - LowestLow))


        ReturnedSMI = 2 * (DEMA / DHL)




        '  Form1.TextBox1.Text = ReturnedSMI

        Return ReturnedSMI
     
    End Function


End Class


