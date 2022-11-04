Imports System.Data

Public Class RPTFromDateToDate
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public MyFlag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer

        rpt.paraname = New String() {"@StoreId", "StoreName", "@FromDate", "@ToDate", "Header", "@Flag"}
        rpt.paravalue = New String() {Val(StoreId.Text), StoreName.Text, FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title, MyFlag}
        Select Case Flag
            Case 1
                rpt.Rpt = "InstallmentInvoicesDateilsPayments.rpt"
            Case 2
                rpt.Rpt = "ReservationsRoomsDataAll.rpt"
            Case 3
                rpt.Rpt = "RoomsDataAllAll_Z.rpt"
            Case 4
                rpt.Rpt = "ReservationsRoomsDataAll2.rpt"
            Case 5
                rpt.Rpt = "SalesAttachments.rpt"
            Case 6
                rpt.Rpt = "InstallmentInvoicesDateilsPaymentsProfit.rpt"
            Case 7
                rpt.Rpt = "ReservationsRoomsDataAllCanceled.rpt"
            Case 8
                rpt.Rpt = "DeliveryOrderAll.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.Addcontrol_MouseDoubleClick({StoreId})

        LoadResource()
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

        If Flag <> 1 Then
            lblStoreId.Visibility = Visibility.Hidden
            StoreId.Visibility = Visibility.Hidden
            StoreName.Visibility = Visibility.Hidden
        End If

        If Flag = 4 Then
            lblFromDate.Visibility = Visibility.Hidden
            FromDate.Visibility = Visibility.Hidden
            lblToDate.Visibility = Visibility.Hidden
            ToDate.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")
        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub StoreId_KeyUp(sender As Object, e As KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
        End If
    End Sub

    Private Sub StoreId_LostFocus(sender As Object, e As RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text)
    End Sub

End Class