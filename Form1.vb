Imports System.IO
Imports System.Windows.Forms.DataVisualization

Public Class Form1
    Dim sr As System.IO.StreamReader
    Dim ResponseAsString As String
    Dim APIkey As String = "112dd3b194de4fe799b5417705b154ba"
    Public TestWalletValues As New List(Of Currency)
    ' Dim elapsedTicks As Long = currentDate.Ticks - centuryBegin.Ticks
    ' Dim elapsedSpan As New TimeSpan(elapsedTicks)
    Dim WithEvents mPrintDocument As New Printing.PrintDocument
    Dim mPrintBitMap As Bitmap
    Dim timestart As DateTime
    Dim elapsedtime As TimeSpan
    Dim SelectedInterval As String = "2.5Sec"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TableLayoutPanel1.Top = Me.Top + 32
        TableLayoutPanel1.Height = Me.Height - 84
        TableLayoutPanel1.Width = Me.Width - 16
        timestart = My.Computer.Clock.LocalTime
    End Sub

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel1.Paint

    End Sub

    Private Sub UpdateValueGrid()
        'Try
        Dim j As Integer = 3
        If DataGridView2.Columns.Count = 0 Then

            DataGridView2.ColumnCount = j
            DataGridView2.Columns(0).Name = "Actual"

            DataGridView2.Columns(1).Name = "Ѻ"

            ' DataGridView2.Columns(2).Name = "40"
            DataGridView2.Columns(2).Name = "AllMAs"

            Dim row As String() = New String() {PriceGraph.Actual.Points.Last.YValues(0), PriceGraph.MA2.Points.Last.YValues(0), PriceGraph.AllMAs.Points.Last.YValues(0)}
            DataGridView2.Rows.Add(row)

        Else
            Dim row As String() = New String() {PriceGraph.Actual.Points.Last.YValues(0), PriceGraph.MA2.Points.Last.YValues(0), PriceGraph.AllMAs.Points.Last.YValues(0)}


            DataGridView2.Rows.Add(row)

        End If




        For Each moo As System.Windows.Forms.DataVisualization.Charting.Series In PriceGraph.AdditionalSeries
            If PriceGraph.AdditionalSeries.Count > DataGridView2.ColumnCount + j Then
                DataGridView2.ColumnCount += 1
                DataGridView2.Columns(j).Name = moo.Name
            Else


            End If
        Next
        '   Catch 
        ErrorLabel.Text = "UpdateValueGrid"

        ' End Try
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs)
        '   BittrexAPI.market_getmarketsummaries()
        Timer1.Enabled = True



    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        BittrexAPI.market_getmarketsummaries()
        Try
            PriceGraph.Addpoint(PriceGraph.Actual, My.Computer.Clock.TickCount, Collections.MarketSummaryList.Find(Function(item) item.MarketName.Contains(TextBox2.Text)).Last)
            Chart1.Titles(2).Text = "[" & PriceGraph.Actual.Points.Count & "]"


            If ((Timer1.Interval * PriceGraph.Actual.Points.Count) - 500).ToString.Contains("500") Then

                Chart1.Titles(1).Text = TimeSpan.FromMilliseconds(Timer1.Interval * PriceGraph.Actual.Points.Count).ToString & " @ " & SelectedInterval
            End If
        Catch
        End Try

        UpdateValueGrid()
    End Sub



    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        TextBox2.Text = DataGridView1.CurrentCell.Value.ToString

        PriceGraph.Actual.Points.Clear()
        PriceGraph.MA.Points.Clear()
        PriceGraph.MA2.Points.Clear()
        For Each item As MarketSummary In Collections.MarketSummaryList
            If item.MarketName = DataGridView1.CurrentCell.Value.ToString Then
                PriceTextBox.Text = item.Last

            End If
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        BittrexAPI.market_getmarkets()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        ActionsBox.Text += "Placed TEST BUY order for " & TextBox4.Text & " " & TextBox2.Text & " @ " & PriceTextBox.Text & " " & My.Computer.Clock.LocalTime & vbNewLine

        BittrexAPI.PlaceOrder(TextBox2.Text, PriceTextBox.Text, TextBox4.Text, True)


    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        BittrexAPI.market_getCurrencies()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '  currentDate = Date.Now

        BittrexAPI.market_getCurrencies()
        BittrexAPI.GetBalances()
        PriceGraph.LoadChart()

        Chart1.Top = Me.Height - Chart1.Height - StatusStrip1.Height - 45
        DataGridView1.Height = Chart1.Top - 35
        ResponseBox.Top = Chart1.Top
        '  ActionsBox.Height = (DataGridView1.Height / 2) - 20
        '  ActionsBox.Width = Me.Width - DataGridView1.Width - 50
        ' ActionsBox.Left = Me.Width - ActionsBox.Width - 30
        ResponseBox.Width = Me.Width - DataGridView1.Width - 50
        ResponseBox.Left = Me.Width - ActionsBox.Width - 30


    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        ActionsBox.Text += "Placed TEST Sell order for " & TextBox4.Text & " " & TextBox2.Text & " @ " & PriceTextBox.Text & " " & My.Computer.Clock.LocalTime & vbNewLine

        BittrexAPI.PlaceOrder(TextBox2.Text, PriceTextBox.Text, TextBox4.Text, False)
    End Sub


    Private Sub Button8_Click(sender As Object, e As EventArgs)
        PriceTextBox.Text = Collections.MarketSummaryList.Find(Function(item) item.MarketName.Contains(TextBox2.Text)).Last
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        BittrexAPI.GetBalances()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs)

        PriceGraph.Addpoint(PriceGraph.Actual, My.Computer.Clock.TickCount, 7)
    End Sub

    Private Sub SecToolStripMenuItem3_Click(sender As Object, e As EventArgs)
        Timer1.Interval = 1500
    End Sub

    Private Sub SecToolStripMenuItem2_Click(sender As Object, e As EventArgs)
        Timer1.Interval = 2500
    End Sub

    Private Sub SecToolStripMenuItem1_Click(sender As Object, e As EventArgs)
        Timer1.Interval = 5000
    End Sub

    Private Sub SecToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Timer1.Interval = 30000
    End Sub

    Private Sub MinToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Timer1.Interval = 60000
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs)
        Timer1.Interval = 600000
    End Sub

    Private Sub MAIntervaL_TextChanged(sender As Object, e As EventArgs)
        PriceGraph.MA.Points.Clear()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click

        Dim pr As Printing.PrintDocument = Chart1.Printing.PrintDocument
        Chart1.BackColor = Color.White
        Chart1.ChartAreas.First.BackColor = Color.White
        Chart1.ChartAreas.First.BorderColor = Color.Black
        Chart1.BorderlineColor = Color.Black

        Chart1.ChartAreas.First.BackSecondaryColor = Color.White
        'Chart1.Printing.PrintPreview()
        pr.DefaultPageSettings.Landscape = True



        Dim printpreviewdialog1 As New PrintPreviewDialog
        printpreviewdialog1.Document = pr

        ' printpreviewdialog1.ShowDialog()
        'Chart1.Printing.Print(True)
        'pr.DefaultPageSettings.Landscape = True
        pr.Print()
        Chart1.BackColor = Color.FromArgb(64, 64, 64)
        Chart1.ChartAreas.First.BackColor = Color.Black
        Chart1.ChartAreas.First.BackSecondaryColor = Color.FromArgb(0, 45, 0)



    End Sub


    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        PriceGraph.AddMA(TextBox5.Text)
        DataGridView2.ColumnCount += 1
        DataGridView2.Columns(DataGridView2.Columns.Count - 1).Name = TextBox5.Text
    End Sub

    Private Sub Chart2_Click(sender As Object, e As EventArgs)

    End Sub


    Private Sub SecToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles SecToolStripMenuItem.Click
        Timer1.Interval = 30000
        SelectedInterval = "30Sec"
    End Sub

    Private Sub DataGridView1_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

        timestart = My.Computer.Clock.LocalTime

        TextBox2.Text = DataGridView1.CurrentCell.Value.ToString

        ClearAll()

        For Each item As MarketSummary In Collections.MarketSummaryList
            If item.MarketName = DataGridView1.CurrentCell.Value.ToString Then
                PriceTextBox.Text = item.Last

            End If
        Next
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles TestBuybtn.Click
        ActionsBox.Text += "Placed TEST BUY order for " & TextBox4.Text & " " & TextBox2.Text & " @ " & PriceTextBox.Text & " " & My.Computer.Clock.LocalTime & vbNewLine

        BittrexAPI.PlaceOrder(TextBox2.Text, PriceTextBox.Text, TextBox4.Text, True)
    End Sub

    Private Sub TestSellbtn_Click(sender As Object, e As EventArgs) Handles TestSellbtn.Click
        ActionsBox.Text += "Placed TEST Sell order for " & TextBox4.Text & " " & TextBox2.Text & " @ " & PriceTextBox.Text & " " & My.Computer.Clock.LocalTime & vbNewLine

        BittrexAPI.PlaceOrder(TextBox2.Text, PriceTextBox.Text, TextBox4.Text, False)
    End Sub

    Private Sub Lastbtn_Click(sender As Object, e As EventArgs) Handles Lastbtn.Click
        PriceTextBox.Text = Collections.MarketSummaryList.Find(Function(item) item.MarketName.Contains(TextBox2.Text)).Last
    End Sub

    Private Sub Chart1_Click_1(sender As Object, e As EventArgs) Handles Chart1.Click

    End Sub

    Private Sub B_print_Click(sender As Object, e As EventArgs) Handles B_print.Click
        OFD1.ShowDialog()
        If File.Exists(OFD1.FileName) Then
            OpenCSV(OFD1.FileName)

        End If
    End Sub
    Private Sub OpenCSV(FileName As String)
        Dim sData() As String
        Dim High, Low As New List(Of Decimal)()
        Dim time As New List(Of String)
        Using sr As New StreamReader(FileName)
            While Not sr.EndOfStream
                sData = sr.ReadLine().Split(","c)

                High.Add(sData(1).Trim())
                Low.Add(sData(2).Trim())
                time.Add(sData(5).Trim())
            End While
        End Using

        Timer1.Enabled = False
        ClearAll()



        ProgressBar1.Maximum = High.Count



        Dim j As Integer = 0
        For Each item As String In High
            ProgressBar1.Value = j
            Chart1.Titles(2).Text = "[" & j & "]"
            Dim average As Decimal = (High(j) + (Low(j)) / 2)
            PriceGraph.Addpoint(PriceGraph.Actual, j, average)
            j += 1

            If j / 100 = Int(j / 100) Then
                Me.Refresh()
            Else
                'x is not an Integer!'
            End If


        Next



        '   HScrollBar1.Maximum = PriceGraph.Actual.Points.Count - 1
        ' HScrollBar1.Width = Chart1.ChartAreas(0).Position.Width
    End Sub

    Private Sub ClearAll()
        PriceGraph.Actual.Points.Clear()
        PriceGraph.MA.Points.Clear()
        PriceGraph.MA2.Points.Clear()
        PriceGraph.bars.Points.Clear()
        PriceGraph.SMI.Points.Clear()
        PriceGraph.SMI2.Points.Clear()
        PriceGraph.AllMAs.Points.Clear()


        'For Each potato As DataGridViewRow In DataGridView2.Rows
        '    DataGridView1.Rows.RemoveAt(0)
        'Next
        For Each item As DataVisualization.Charting.Series In PriceGraph.AdditionalSeries
            item.Points.Clear()
        Next
    End Sub


    'Private Sub graphMax_TextChanged(sender As Object, e As EventArgs) Handles graphMax.TextChanged
    '    Dim newseries As Charting.Series = PriceGraph.Actual

    '    For x As Integer = 0 To graphMax.Text - 1
    '        newseries.Points.RemoveAt(x)
    '    Next

    '    PriceGraph.Actual.Enabled = False
    '    PriceGraph.AdditionalSeries.Add(newseries)
    'End Sub

    Private Sub Button2_Click_2(sender As Object, e As EventArgs) Handles Button2.Click
        ExportDGVToCSV(False)
    End Sub

    Private Sub ExportDGVToCSV(Optional ByVal blnWriteColumnHeaderNames As Boolean = False, Optional ByVal strDelimiterType As String = ",")
        SaveFileDialog1.ShowDialog()
        Dim strexportfilename As String = SaveFileDialog1.FileName
        Dim sr As StreamWriter = File.CreateText(strExportFileName)
        Dim strDelimiter As String = strDelimiterType
        Dim intColumnCount As Integer = DataGridView2.Columns.Count - 1
        Dim strRowData As String = ""

        If blnWriteColumnHeaderNames Then
            For intX As Integer = 0 To intColumnCount
                strRowData += Replace(DataGridView2.Columns(intX).Name, strDelimiter, "") & IIf(intX < intColumnCount, strDelimiter, "")
            Next intX
            sr.WriteLine(strRowData)
        End If

        For intX As Integer = 0 To DataGridView2.Rows.Count - 1
            strRowData = ""
            For intRowData As Integer = 0 To intColumnCount
                strRowData += Replace(DataGridView2.Rows(intX).Cells(intRowData).Value, strDelimiter, "") & IIf(intRowData < intColumnCount, strDelimiter, "") '''''''''highlights this row
            Next intRowData
            sr.WriteLine(strRowData)
        Next intX
        sr.Close()
        MsgBox("ok")
    End Sub
End Class
