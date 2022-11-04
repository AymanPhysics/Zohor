Imports System.Data

Public Class RPT45
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Val(Day.Text) = 0 Then
            Day.Text = 1
        End If
        If Val(Day2.Text) = 0 Then
            Day2.Text = Date.DaysInMonth(Val(Year.Text), Val(Month.Text))
        End If

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@AccNo", "@Year", "@Month", "@Day", "@Day2", "@IsDelayOnly", "Header"}
        rpt.paravalue = New String() {Val(AccNo.Text), Val(Year.Text), Val(Month.Text), Val(Day.Text), Val(Day2.Text), IIf(IsDelayOnly.IsChecked, 1, 0), CType(Parent, Page).Title}

        Select Case Flag
            Case 1
                rpt.Rpt = "MonthlyPayment.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return

        bm.Addcontrol_MouseDoubleClick({AccNo})
        LoadResource()

        Year.Text = bm.MyGetDate.Year
        Month.Text = bm.MyGetDate.Month
        Day.Text = bm.MyGetDate.Day
        Day2.Text = bm.MyGetDate.Day

    End Sub


    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")

    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , 1, )
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , 1, )
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

End Class