Imports System.Data
Imports System.IO

Public Class RPTToDate
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Flag = 1 Then
            If bm.ShowDeleteMSG("سيتم مسح كافة البيانات حتى تاريخ " & bm.ToStrDate(ToDate.SelectedDate) & " .. استمرار؟") Then
                If bm.ExecuteNonQuery("exec bk", False) AndAlso bm.ExecuteNonQuery("delete Reservations where DayDate<='" & bm.ToStrDate(ToDate.SelectedDate) & "' delete Services where DayDate<='" & bm.ToStrDate(ToDate.SelectedDate) & "' delete EmpCloseShift where DayDate<='" & bm.ToStrDate(ToDate.SelectedDate) & "' delete EmpOutCome where DayDate<='" & bm.ToStrDate(ToDate.SelectedDate) & "'  exec SetBalances '" & bm.ToStrDate(ToDate.SelectedDate) & "'", False) Then
                    bm.ShowMSG("تمت العملية بنجاح")
                Else
                    bm.ShowMSG("عملية فاشلة !!!")
                End If
            End If
            Return
        End If

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@ToDate", "Header"}
        rpt.paravalue = New String() {ToDate.SelectedDate, CType(Parent, Page).Title.Trim}
        Select Case Flag
            Case 2
                rpt.Rpt = ".rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        If Flag = 1 Then
            lblToDate.Content = "حتى تاريخ"
        ElseIf Flag = 2 Then
            lblToDate.Visibility = Visibility.Hidden
            ToDate.Visibility = Visibility.Hidden
        Else

        End If
        Dim MyNow As DateTime = bm.MyGetDate()
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Dim lop As Boolean = False



    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "Start")
        lblToDate.SetResourceReference(ContentProperty, "DayDate")

    End Sub


End Class