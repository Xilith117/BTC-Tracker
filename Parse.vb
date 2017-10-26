Public Class Parse
    Public Shared ParsedList As New List(Of String)

    Public Shared Function ParseText(text As String)

        ParsedList.RemoveRange(0, ParsedList.Count)
        Dim levelsarray As String()
        levelsarray = text.Split(New Char() {"{"c})
        For Each item As String In levelsarray

            ParsedList.Add(item)
        Next
        ParsedList.RemoveAt(0)

        If ParsedList(0).Contains("false") Then
            MsgBox("Server returned 'False'")
        Else
            ParsedList.RemoveAt(0)

        End If


        Return ParsedList
    End Function

    Public Shared Function GetBetween(ByRef sSearch As String, ByRef sStart As String, ByRef sStop As String, _
                                                   Optional ByRef lSearch As Integer = 1) As String


        lSearch = InStr(lSearch, sSearch, sStart) 'Usage:Text1.Text =  Getbetween("123 get this 321","123","321")
        If lSearch > 0 Then
            lSearch = lSearch + Len(sStart)
            Dim lTemp As Long
            lTemp = InStr(lSearch, sSearch, sStop)
            If lTemp > lSearch Then
                GetBetween = Mid$(sSearch, lSearch, lTemp - lSearch)
      
            End If
        End If
        Return GetBetween
    End Function
End Class
