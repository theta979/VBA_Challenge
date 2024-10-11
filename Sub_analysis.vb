Sub analyse_stocks():

    'Create loop to cycle through workheets
    Dim ws As Worksheet
    For Each ws In ThisWorkbook.Worksheets
     'Activate each worksheet
        ws.Activate
        
        'Set row headers
        ws.Range("I1").Value = "Ticker"
        ws.Range("J1").Value = "Quarterly Change"
        ws.Range("K1").Value = "Percent Change"
        ws.Range("L1").Value = "Total Stock Volume"
        
        'Set rows for comparison
        ws.Range("N2").Value = "Greatest % Increase"
        ws.Range("N3").Value = "Greatest % Decrease"
        ws.Range("N4").Value = "Total Stock Volume"
        
        'Comparison headers
        ws.Range("O1").Value = "Ticker"
        ws.Range("P1").Value = "Value"
        
        
        ' Setting The Dimensions
        '-------------------------------
        Dim total As Double
        Dim i As Long
        Dim change As Double
        Dim j As Integer
        Dim start As Long
        Dim rowCount As Long
        Dim percentChange As Double
        Dim averageChange As Double
        
        'Set & correct the initial values
        j = 0
        total = 0
        change = 0
        start = 2

        ' Get Row Count Value
        '-------------------------------
        rowCount = ws.Cells(ws.Rows.Count, 1).End(xlUp).Row
    
        For i = 2 To rowCount

            'Look ahead for ticker change
            '------------------------------------------
            If ws.Cells(i + 1, 1).Value <> ws.Cells(i, 1).Value Then

                'Store results in variables
                total = total + ws.Cells(i, 7).Value

                'Handle zero total volume
                If total = 0 Then
                    'Print the results
                    ws.Range("I" & 2 + j).Value = ws.Cells(i, 1).Value
                    ws.Range("J" & 2 + j).Value = 0
                    ws.Range("K" & 2 + j).Value = "%" & 0
                    ws.Range("L" & 2 + j).Value = 0

                Else
                    'Find First non-zero starting value
                    If ws.Cells(start, 3) = 0 Then
                        For find_value = start To i
                            If ws.Cells(find_value, 3).Value <> 0 Then
                                start = find_value
                                Exit For
                            End If
                        Next find_value
                    End If

                    'Calculate Change
                    change = (ws.Cells(i, 6) - ws.Cells(start, 3))
                    percentChange = change / ws.Cells(start, 3)

                    'Start next ticker
                    start = i + 1

                    'Print the result
                    ws.Range("I" & 2 + j).Value = ws.Cells(i, 1).Value
                    ws.Range("J" & 2 + j).Value = change
                    ws.Range("J" & 2 + j).NumberFormat = "0.00"
                    ws.Range("K" & 2 + j).Value = percentChange
                    ws.Range("K" & 2 + j).NumberFormat = "0.00%"
                    ws.Range("L" & 2 + j).Value = total

                    'Color code Positives green & negatives red
                    Select Case change
                        Case Is > 0
                            ws.Range("J" & 2 + j).Interior.ColorIndex = 4
                        Case Is < 0
                            ws.Range("J" & 2 + j).Interior.ColorIndex = 3
                        Case Else
                            ws.Range("J" & 2 + j).Interior.ColorIndex = 0
                    End Select

                End If

                    'Reset Variables for new ticker
                    total = 0
                    change = 0
                    j = j + 1

                'If ticker is still the same, add results
                Else
                    total = total + ws.Cells(i, 7).Value

                End If

    
         Next i

         'Take Max & Min and place them in a separate part in the worksheet
         ws.Range("P2") = "%" & WorksheetFunction.Max(ws.Range("K2:K" & rowCount)) * 100
         ws.Range("P3") = "%" & WorksheetFunction.Min(ws.Range("K2:K" & rowCount)) * 100
         ws.Range("P4") = WorksheetFunction.Max(ws.Range("L2:L" & rowCount))

         'Returns one less to exclude header
         increase_number = WorksheetFunction.Match(WorksheetFunction.Max(ws.Range("K2:K" & rowCount)), ws.Range("K2:K" & rowCount), 0)
         decrease_number = WorksheetFunction.Match(WorksheetFunction.Min(ws.Range("K2:K" & rowCount)), ws.Range("K2:K" & rowCount), 0)
         volume_number = WorksheetFunction.Match(WorksheetFunction.Max(ws.Range("L2:L" & rowCount)), ws.Range("L2:L" & rowCount), 0)

         'Final ticker symbol for total, greatest increase, decrease and total volume
         ws.Range("O2") = ws.Cells(increase_number + 1, 9)
         ws.Range("O3") = ws.Cells(decrease_number + 1, 9)
         ws.Range("O4") = ws.Cells(volume_number + 1, 9)

    Next ws

End Sub

