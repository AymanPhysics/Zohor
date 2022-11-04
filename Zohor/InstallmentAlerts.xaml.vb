Imports System.Data
Imports System.Threading.Tasks
Imports System.Threading

Public Class InstallmentAlerts
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Dim dt1 As New DataTable
    Dim dt2 As New DataTable
    Dim dt5 As New DataTable
    Dim dt6 As New DataTable
    Dim dtValues As New DataTable
    Dim dtG1 As New DataTable
    Dim dtG11 As New DataTable
    Dim dtG2 As New DataTable
    Dim dtG22 As New DataTable
    Public Flag As Integer = 0



    Dim MyFromDate As DateTime
    Dim MyToDate As DateTime


    Dim IsLoaded As Boolean = False
    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = MyNow.Date
        ToDate.SelectedDate = MyNow.Date

        MyFromDate = FromDate.SelectedDate
        MyToDate = ToDate.SelectedDate

        IsLoaded = True
        LoadAllData()

    End Sub
    Sub LoadAllData()
        Task.Factory.StartNew(AddressOf GetBal1)
        Task.Factory.StartNew(AddressOf GetBal2)
        Task.Factory.StartNew(AddressOf GetBal5)
        Task.Factory.StartNew(AddressOf GetBal6)


        Value1_X.Text = ""
        'Value2_X.Text = ""
        Value3_X.Text = ""
        Value4_X.Text = ""
        Task.Factory.StartNew(AddressOf GetValues_1)

        Value1.Text = ""
        'Value2.Text = ""
        Value3.Text = ""
        Value4.Text = ""
        Value41.Text = ""
        Task.Factory.StartNew(AddressOf GetValues_2)


        SalesMasterNo.Clear()
        Task.Factory.StartNew(AddressOf GetSalesMasterNo)


        GetG1()
        GetG2()

    End Sub

    Dim lop As Boolean = False

    Dim C1 As Integer = 0
    Dim C2 As Integer = 0
    Dim C3 As Integer = 0
    Dim C4 As Integer = 0
    Dim MyBal1 As Decimal = 0
    Sub GetBal1()
        C1 = bm.ExecuteScalar("select count(Id) from Customers where Ban2=0")
        C3 = bm.ExecuteScalar("select count(Id) from Customers where Ban=1")
        C4 = bm.ExecuteScalar("select count(Id) from Customers where Ban2=1")

        dt1 = bm.ExecuteAdapter("GetAllBal", {"MainAccNo", "LinkFile", "DayDate", "UserName", "P", "Z", "N"}, {"", 1, bm.ToStrDate(Now.Date), Md.UserName, "1", "1", "1"})

        MyBal1 = 0
        C2 = 0
        For i As Integer = 0 To dt1.Rows.Count - 1
            MyBal1 += Val(dt1.Rows(i)("MainBal0").ToString)
            If Val(dt1.Rows(i)("MainBal0").ToString) > 0 Then
                C2 += 1
            End If
        Next

        Dispatcher.BeginInvoke(Sub()
                                   Bal1.Text = MyBal1

                                   Customer1.Text = C1 - C2
                                   Customer2.Text = C2
                                   Customer3.Text = C3
                                   Customer4.Text = C4
                               End Sub)
    End Sub

    Dim MyBal2 As Decimal = 0
    Sub GetBal2()
        dt2 = bm.ExecuteAdapter("GetAllBal", {"MainAccNo", "LinkFile", "DayDate", "UserName", "P", "Z", "N"}, {"", 2, bm.ToStrDate(Now.Date), Md.UserName, "1", "1", "1"})

        MyBal2 = 0
        For i As Integer = 0 To dt2.Rows.Count - 1
            MyBal2 += Val(dt2.Rows(i)("MainBal0").ToString)
        Next
        Dispatcher.BeginInvoke(Sub()
                                   Bal2.Text = MyBal2
                               End Sub)
    End Sub

    Dim MyBal5 As Decimal = 0
    Sub GetBal5()
        dt5 = bm.ExecuteAdapter("GetAllBal", {"MainAccNo", "LinkFile", "DayDate", "UserName", "P", "Z", "N"}, {"", 5, bm.ToStrDate(Now.Date), Md.UserName, "1", "1", "1"})

        MyBal5 = 0
        For i As Integer = 0 To dt5.Rows.Count - 1
            MyBal5 += Val(dt5.Rows(i)("MainBal0").ToString)
        Next

        Dispatcher.BeginInvoke(Sub()
                                   Bal5.Text = MyBal5
                               End Sub)
    End Sub

    Dim MyBal6 As Decimal = 0
    Sub GetBal6()
        dt6 = bm.ExecuteAdapter("GetAllBal", {"MainAccNo", "LinkFile", "DayDate", "UserName", "P", "Z", "N"}, {"", 6, bm.ToStrDate(Now.Date), Md.UserName, "1", "1", "1"})

        MyBal6 = 0
        For i As Integer = 0 To dt6.Rows.Count - 1
            MyBal6 += Val(dt6.Rows(i)("MainBal0").ToString)
        Next

        Dispatcher.BeginInvoke(Sub()
                                   Bal6.Text = MyBal6
                               End Sub)
    End Sub




    Sub GetValues_1()
        dtValues = bm.ExecuteAdapter("GetInstallmentInvoicesDateils2", {"FromDate", "ToDate", "Ban", "PaymentDay", "IsDelayed", "All", "Flag"}, {bm.ToStrDate(MyFromDate), bm.ToStrDate(MyToDate), "0", "0", "0", 1, 1})
        Dispatcher.BeginInvoke(Sub()
                                   Try

                                       Value1_X.Text = "0"
                                       'Value2_X.Text = "0"
                                       Value3_X.Text = "0"
                                       Value4_X.Text = "0"


                                       If Not dtValues Is Nothing Then
                                           For i As Integer = 0 To dtValues.Rows.Count - 1
                                               Value1_X.Text += Val(dtValues.Rows(i)("Value").ToString)
                                               Value3_X.Text += Val(dtValues.Rows(i)("Payed").ToString)
                                               Value4_X.Text += Val(dtValues.Rows(i)("Remaining").ToString)
                                           Next
                                       End If
                                   Catch ex As Exception
                                   End Try
                               End Sub)
    End Sub
    Sub GetValues_2()
        dtValues = bm.ExecuteAdapter("GetInstallmentInvoicesDateils2", {"FromDate", "ToDate", "Ban", "PaymentDay", "IsDelayed", "All", "Flag"}, {bm.ToStrDate(MyFromDate), bm.ToStrDate(MyToDate), "0", "0", "0", 1, 2})
        Dispatcher.BeginInvoke(Sub()
                                   Try

                                       Value1.Text = "0"
                                       'Value2.Text = "0"
                                       Value3.Text = "0"
                                       Value4.Text = "0"
                                       Value41.Text = "0"
                                       If Not dtValues Is Nothing Then
                                           For i As Integer = 0 To dtValues.Rows.Count - 1
                                               Value1.Text += Val(dtValues.Rows(i)("Value").ToString)
                                               Value3.Text += Val(dtValues.Rows(i)("Payed").ToString)

                                               If Val(dtValues.Rows(i)("Ban").ToString) = 1 Then
                                                   Value41.Text += Val(dtValues.Rows(i)("Remaining").ToString)
                                               Else
                                                   Value4.Text += Val(dtValues.Rows(i)("Remaining").ToString)
                                               End If
                                           Next
                                       End If
                                   Catch ex As Exception
                                   End Try
                               End Sub)
    End Sub



    Dim SalesMasterCount As Integer = 0
    Sub GetSalesMasterNo()
        SalesMasterCount = 0
        SalesMasterCount = bm.ExecuteScalar("select count(InvoiceNo) from SalesMaster where Flag=13 and DayDate between '" & bm.ToStrDate(MyFromDate) & "' and '" & bm.ToStrDate(MyToDate) & "'")
        Dispatcher.BeginInvoke(Sub()
                                   Try
                                       SalesMasterNo.Text = SalesMasterCount
                                   Catch ex As Exception
                                   End Try
                               End Sub)
    End Sub

    Private Sub btnBal1_Click(sender As Object, e As RoutedEventArgs) Handles btnBal1.Click
        ShowBal(1)
    End Sub

    Private Sub btnBal2_Click(sender As Object, e As RoutedEventArgs) Handles btnBal2.Click
        ShowBal(2)
    End Sub

    Private Sub btnBal5_Click(sender As Object, e As RoutedEventArgs) Handles btnBal5.Click
        ShowBal(5)
    End Sub

    Private Sub btnBal6_Click(sender As Object, e As RoutedEventArgs) Handles btnBal6.Click
        ShowBal(6)
    End Sub

    Sub ShowBal(LinkFile As Integer)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@MainAccNo", "MainAccName", "@DayDate", "@ToDate", "@LinkFile", "Header", "IsGroupped", "@P", "@Z", "@N"}
        rpt.paravalue = New String() {"", "", bm.ToStrDate(ToDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate), LinkFile, "الأرصدة", 0, 1, 1, 1}
        rpt.Rpt = "AllBalTel.rpt"
        rpt.Show()
    End Sub

    Private Sub btnValue1_X_Click(sender As Object, e As RoutedEventArgs) Handles btnValue1_X.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Ban", "Header", "@PaymentDay", "@IsDelayed", "@All", "@Flag"}
        rpt.paravalue = New String() {bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate), 0, "الأقساط", 0, 0, 1, 1}
        rpt.Rpt = "InstallmentInvoicesDateils.rpt"
        rpt.Show()
    End Sub

    Private Sub btnValue1_Click(sender As Object, e As RoutedEventArgs) Handles btnValue1.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Ban", "Header", "@PaymentDay", "@IsDelayed", "@All", "@Flag"}
        rpt.paravalue = New String() {bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate), 0, "الأقساط", 0, 0, 1, 2}
        rpt.Rpt = "InstallmentInvoicesDateils.rpt"
        rpt.Show()
    End Sub

    Private Sub FromDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles FromDate.SelectedDateChanged, ToDate.SelectedDateChanged
        If Not IsLoaded Then Return
        MyFromDate = FromDate.SelectedDate
        MyToDate = ToDate.SelectedDate
        LoadAllData()
    End Sub

    Sub GetG1()
        Try
            dtG1 = bm.ExecuteAdapter("select G.LinkFile,L.Name 'الجهة',SUM(G.Value) 'القيمة' from BankCash_G G left join LinkFile L on(L.Id=G.LinkFile) where G.Flag=1 and G.DayDate between '" & bm.ToStrDate(MyFromDate) & "' and '" & bm.ToStrDate(MyToDate) & "' group by G.LinkFile,L.Name")
            dtG1.TableName = "tbl"
            Dim dv1 As New DataView
            dv1.Table = dtG1
            G1.ItemsSource = dv1
            G1.Columns(0).Visibility = Visibility.Hidden
        Catch ex As Exception
        End Try
    End Sub

    Private Sub G1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles G1.SelectionChanged
        Try
            dtG11 = bm.ExecuteAdapter("select A.Name 'الجهة',SUM(G.Value) 'القيمة' from BankCash_G G left join AllSub A on(A.LinkFile=G.LinkFile and A.Id=G.SubAccNo) where G.Flag=1 and G.DayDate between '" & bm.ToStrDate(MyFromDate) & "' and '" & bm.ToStrDate(MyToDate) & "' and G.LinkFile='" & G1.SelectedItem("LinkFile") & "' and A.Id<>0 group by G.LinkFile,A.Name")
            dtG11.TableName = "tbl"
            Dim dv11 As New DataView
            dv11.Table = dtG11
            G11.ItemsSource = dv11
        Catch ex As Exception
        End Try
    End Sub

    Sub GetG2()
        Try
            dtG2 = bm.ExecuteAdapter("select G.LinkFile,L.Name 'الجهة',SUM(G.Value) 'القيمة' from BankCash_G G left join LinkFile L on(L.Id=G.LinkFile) where G.Flag=2 and G.DayDate between '" & bm.ToStrDate(MyFromDate) & "' and '" & bm.ToStrDate(MyToDate) & "' group by G.LinkFile,L.Name")
            dtG2.TableName = "tbl"
            Dim dv2 As New DataView
            dv2.Table = dtG2
            G2.ItemsSource = dv2
            G2.Columns(0).Visibility = Visibility.Hidden
        Catch ex As Exception
        End Try
    End Sub

    Private Sub G2_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles G2.SelectionChanged
        Try
            dtG22 = bm.ExecuteAdapter("select A.Name 'الجهة',SUM(G.Value) 'القيمة' from BankCash_G G left join AllSub A on(A.LinkFile=G.LinkFile and A.Id=G.SubAccNo) where G.Flag=2 and G.DayDate between '" & bm.ToStrDate(MyFromDate) & "' and '" & bm.ToStrDate(MyToDate) & "' and G.LinkFile='" & G2.SelectedItem("LinkFile") & "' and A.Id<>0 group by G.LinkFile,A.Name")
            dtG22.TableName = "tbl"
            Dim dv22 As New DataView
            dv22.Table = dtG22
            G22.ItemsSource = dv22
        Catch ex As Exception
        End Try
    End Sub

    Private Sub BtnSalesMaster_Click(sender As Object, e As RoutedEventArgs) Handles btnSalesMaster.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@ToDate2", "@Shift", "ShiftName", "@Flag", "@StoreId", "StoreName", "@FromInvoiceNo", "@ToInvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "@CashierId", "@IsClosedOnly", "Header", "@ToId", "@ItemId", "@ColorId", "ColorName", "@SizeId", "SizeName", "@SaveId", "ItemName", "@CountryId", "CountryName", "@GroupId", "GroupName", "@TypeId", "TypeName", "@WaiterId", "@SalesTypeId", "@Canceled"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, ToDate.SelectedDate, 0, "", 0, 0, "", 0, 0, 0, 3, 0, 0, 0, 0, "المبيعات", 0, 0, 0, "", 0, "", 0, "", 0, "", 0, "", 0, "", 0, 0, 0}

        rpt.Rpt = "Sales2.rpt"
        rpt.Show()
    End Sub
End Class