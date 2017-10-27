Imports System.Configuration
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading
Imports System.Net.Mail
Imports System.Windows.Forms
Imports System.Collections.Specialized
Imports System.Security.Principal
Imports System.Xml

Public Class BittrexAPI



    Public Shared Function APIRequest(method As String, Optional currency As String = "BTC")
        Dim ResponseAsString As String = "Error"
        '   Try

        Dim APIkey As String = "5ff88e9b616541a4ae3f1187a5a0cc52"
        Dim Secret As String = "0fe32a2372e44a01b049d177598f8222"

        Dim URI As String


        If method.Contains("public") Then
            URI = "https://bittrex.com/api/v1.1/" & method
        Else
            URI = "https://bittrex.com/api/v1.1/account/" & method & "?apikey=" & APIkey & "&nonce=" & Environment.TickCount & "&currency=" & currency

        End If

        Dim request = WebRequest.Create("https://bittrex.com/api/v1.1/")
        Dim hmAcSha = New HMACSHA512(Encoding.ASCII.GetBytes(Secret))
        Dim URIbyte = Encoding.ASCII.GetBytes(URI)
        Dim hashmessage = hmAcSha.ComputeHash(URIbyte)
        Dim sign = BitConverter.ToString(hashmessage)
        Dim client As New WebClient()

        sign = sign.Replace("-", "")
        client.Headers.Add("apisign", sign)
        ResponseAsString = client.DownloadString(URI)
        ResponseAsString = ResponseAsString.Replace("""", "")
        Form1.ResponseBox.Text = ResponseAsString


        Return ResponseAsString

    End Function

    Public Shared Sub account_getbalance()

        Form1.ResponseBox.Text = BittrexAPI.APIRequest("getbalance", "ETH")
    End Sub

    Public Shared Sub account_getorderhistory()

        Form1.ResponseBox.Text = BittrexAPI.APIRequest("getorderhistory", "ETH")
        Parse.ParseText(BittrexAPI.APIRequest("getorderhistory", "ETH"))
    End Sub

    Public Shared Sub market_getmarkets()

        Form1.ResponseBox.Text = BittrexAPI.APIRequest("public/getmarkets")
    End Sub
    Public Shared Sub market_getTicker()

        Form1.ResponseBox.Text = BittrexAPI.APIRequest("public/getticker")
    End Sub

    Public Shared Sub market_getmarketsummaries()


        Try
            Collections.MarketSummaryList.RemoveRange(0, Collections.MarketSummaryList.Count)

            For Each item As String In Parse.ParseText(BittrexAPI.APIRequest("public/getmarketsummaries"))
                Dim NewMarketSummary As New MarketSummary
                With NewMarketSummary
                    .MarketName = Parse.GetBetween(item, "MarketName:", ",")
                    .High = Parse.GetBetween(item, "High:", ",")
                    .Low = Parse.GetBetween(item, "Low:", ",")
                    .Volume = Parse.GetBetween(item, "Volume:", ",")
                    .Last = Parse.GetBetween(item, "Last:", ",")
                    .BaseVolume = Parse.GetBetween(item, "BaseVolume:", ",")
                    .Bid = Parse.GetBetween(item, "Bid:", ",")
                    .Ask = Parse.GetBetween(item, "Ask:", ",")
                    .OpenBuyOrders = Parse.GetBetween(item, "OpenBuyOrders:", ",")
                    .OpenSellOrders = Parse.GetBetween(item, "OpenSellOrders:", ",")
                    .PrevDay = Parse.GetBetween(item, "PrevDay:", ",")
                End With
                Collections.MarketSummaryList.Add(NewMarketSummary)


            Next

            If Collections.OldMarketSummaryList.Count = 0 Then


                Form1.DataGridView1.RowsDefaultCellStyle.BackColor = Color.Black ' Color.FromArgb(64, 64, 64)
                Form1.DataGridView1.RowsDefaultCellStyle.ForeColor = Color.Lime

                Form1.DataGridView1.ColumnCount = 8
                Form1.DataGridView1.Columns(0).Name = "Market"

                Form1.DataGridView1.Columns(1).Name = "High"
                Form1.DataGridView1.Columns(2).Name = "Low"
                Form1.DataGridView1.Columns(3).Name = "Volume"
                Form1.DataGridView1.Columns(4).Name = "Last"
                Form1.DataGridView1.Columns(5).Name = "BaseVolume"
                Form1.DataGridView1.Columns(6).Name = "Bid"
                Form1.DataGridView1.Columns(7).Name = "Ask"

                Dim y As Integer = 0
                For Each item As MarketSummary In Collections.MarketSummaryList

                    Dim row As String() = New String() {item.MarketName, item.High, item.Low, item.Volume, item.Last, item.BaseVolume, item.Bid, item.Ask}
                    Form1.DataGridView1.Rows.Add(row)

                Next

            Else
                Collections.HistoricalMarketSummaries.Add(Collections.OldMarketSummaryList)

                For Each item As MarketSummary In Collections.MarketSummaryList


                    If item.Last = Collections.OldMarketSummaryList(Collections.MarketSummaryList.IndexOf(item)).Last And item.Ask = Collections.OldMarketSummaryList(Collections.MarketSummaryList.IndexOf(item)).Ask And item.Bid = Collections.OldMarketSummaryList(Collections.MarketSummaryList.IndexOf(item)).Bid Then  'Dont forget to add additional qualifiers
                        Form1.DataGridView1.Rows.RemoveAt(Collections.MarketSummaryList.IndexOf(item))

                        Dim row As String() = New String() {item.MarketName, item.High, item.Low, item.Volume, item.Last, item.BaseVolume, item.Bid, item.Ask}
                        Form1.DataGridView1.Rows.Insert(Collections.MarketSummaryList.IndexOf(item), row)




                    Else
                        MsgBox("HMM")

                    End If
                Next


            End If

            If Collections.MarketSummaryList.Count > 0 Then
                Collections.OldMarketSummaryList = Collections.MarketSummaryList
            End If
            Form1.ConnectionLabel.Text = "Connected"
            Form1.ConnectionLabel.ForeColor = Color.Lime

            Form1.DataGridView1.Visible = True


        Catch
            Form1.ConnectionLabel.Text = "Disconnected"
            Form1.ConnectionLabel.ForeColor = Color.Red
        End Try

    End Sub
    Public Shared Sub GetBalances()

        Collections.BalanceList.RemoveRange(0, Collections.BalanceList.Count)

        For Each item As String In Parse.ParseText(BittrexAPI.APIRequest("getbalances"))
            Dim NewWallet As New Wallet
            With NewWallet
                .Available = Parse.GetBetween(item, "Available:", ",")
                .Balance = Parse.GetBetween(item, "Balance:", ",")
                .CryptoAddress = Parse.GetBetween(item, "CryptoAddress:", "}")
                .Currency = Collections.CurrencyList.Find(Function(moo) moo.ID.Contains(Parse.GetBetween(item, "Currency:", ",")))
                .Pending = Parse.GetBetween(item, "Pending:", ",")
                .Requested = Convert.ToBoolean(Parse.GetBetween(item, "Requested:", ","))
                .UUID = Parse.GetBetween(item, "UUID:", ",")

            End With

            Collections.BalanceList.Add(NewWallet)

        Next

    End Sub

    Public Shared Sub PlaceOrder(Market As String, Price As Double, Amount As Double, IsBuy As Boolean)
        Dim NewOrder As New Order
        Dim arrayforsplit() As String = Market.Split(New Char() {"-"c})

        With NewOrder
            .Amount = Amount
            .Price = Price
            .Currency = Collections.CurrencyList.Find(Function(item) item.ID.Contains(arrayforsplit(1)))
            .Market = Market
            .IsTest = True
            .TimeStamp = My.Computer.Clock.LocalTime
            .TotalPaid = Price * Amount
            .IsBuy = IsBuy
        End With
        Collections.OrderList.Add(NewOrder)

    End Sub

    Public Shared Sub market_getCurrencies()

        For Each item As String In Parse.ParseText(BittrexAPI.APIRequest("public/getcurrencies"))
            Dim NewCurrency As New Currency
            With NewCurrency
                .ID = Parse.GetBetween(item, "Currency:", ",")
                .Name = Parse.GetBetween(item, "CurrencyLong:", ",")
                .TxFee = Parse.GetBetween(item, "TxFee:", ",")
                .IsActive = Parse.GetBetween(item, "IsActive:", ",")
                .MinConfirmations = Parse.GetBetween(item, "MinConfirmation:", ",")
                .BaseAddress = Parse.GetBetween(item, "BaseAddress:", ",")
                .CoinType = Parse.GetBetween(item, "CoinType:", ",")
            End With
            Collections.CurrencyList.Add(NewCurrency)


        Next


    End Sub

End Class
