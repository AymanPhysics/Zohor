Imports System.Data

Public Class Installment
    Dim bm As New BasicMethods
    Public Flag As Integer
    Public MyStoreId As Integer = 0
    Public MyToId As Integer = 0
    Public MyInvoiceNo As Integer = 0
    WithEvents G As New MyGrid
    WithEvents G1 As New MyGrid
    WithEvents G2 As New MyGrid
    Dim dt As DataTable
    Dim CustTbl As String = "Customers"

    Private Sub Installment_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me, True) Then Return

        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {}, {})

        bm.Addcontrol_MouseDoubleClick({ToId, StoreId, SaveId})
        bm.FillCombo("PaymentDays", PaymentDay, "", , True)
        bm.FillCombo("InstallmentCounts", InstallCount, "", , True)

        LoadWFH()
        LoadWFH1()
        LoadWFH2()

        If Flag = 35 Then
            CustTbl = "Investors"
            Panel1.Visibility = Visibility.Hidden
        End If

        StoreId.Text = MyStoreId
        If MyStoreId = 0 Then
            StoreId.Text = Md.DefaultStore
        End If
        StoreId_LostFocus(Nothing, Nothing)
        ToId.Text = MyToId
        ToId_LostFocus(Nothing, Nothing)
        InvoiceNo.SelectedValue = MyInvoiceNo
        InvoiceNo_LostFocus(Nothing, Nothing)


        DayDate.SelectedDate = Now.Date
        SaveId.Text = Md.DefaultSave
        SaveId_LostFocus(Nothing, Nothing)



    End Sub

    Structure GC
        Shared Id As String = "Id"
        Shared TypeName As String = "TypeName"
        Shared MainDayDate As String = "MainDayDate"
        Shared DayDate As String = "DayDate"
        Shared Value As String = "Value"
        Shared Payed As String = "Payed"
        Shared Remaining As String = "Remaining"
        Shared PaymentDay As String = "PaymentDay"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Id, "المسلسل")
        G.Columns.Add(GC.TypeName, "النوع")
        G.Columns.Add(GC.MainDayDate, "التاريخ")
        G.Columns.Add(GC.DayDate, "تاريخ الاستحقاق")
        G.Columns.Add(GC.Value, "القيمة")
        G.Columns.Add(GC.Payed, "المدفوع")
        G.Columns.Add(GC.Remaining, "المتبقي")
        bm.MakeGridCombo(G, "اليوم", GC.PaymentDay, "select Id,Name from PaymentDays union all select 0 Id,'-' Name order by Id", 100)


        G.Columns(GC.Id).ReadOnly = True
        G.Columns(GC.TypeName).ReadOnly = True
        G.Columns(GC.MainDayDate).ReadOnly = True
        G.Columns(GC.DayDate).ReadOnly = True
        G.Columns(GC.Value).ReadOnly = True
        G.Columns(GC.Payed).ReadOnly = True
        G.Columns(GC.Remaining).ReadOnly = True
        G.Columns(GC.PaymentDay).ReadOnly = True

        G.RowHeadersVisible = False
        G.AllowUserToAddRows = False

        AddHandler G.SelectionChanged, AddressOf G_SelectionChanged
    End Sub


    Structure GC1
        Shared Name As String = "Name"
        Shared NationalID As String = "NationalID"
        Shared Mobile As String = "Mobile"
        Shared Job As String = "Job"
        Shared Address As String = "Address"
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH1()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH1.Child = G1

        G1.Columns.Clear()
        G1.ForeColor = System.Drawing.Color.DarkBlue

        G1.Columns.Add(GC1.Name, "الاسم")
        G1.Columns.Add(GC1.NationalID, "الرقم القومي")
        G1.Columns.Add(GC1.Mobile, "الموبيل")
        G1.Columns.Add(GC1.Job, "الوظيفة")
        G1.Columns.Add(GC1.Address, "العنوان")
        G1.Columns.Add(GC1.Notes, "ملاحظات")
        
    End Sub


    Structure GC2
        Shared PaymentsInvoiceNo As String = "PaymentsInvoiceNo"
        Shared DayDate As String = "DayDate"
        Shared Value As String = "Value"
        Shared SaveName As String = "SaveName"
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH2()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH2.Child = G2

        G2.Columns.Clear()
        G2.ForeColor = System.Drawing.Color.DarkBlue

        G2.Columns.Add(GC2.PaymentsInvoiceNo, "المسلسل")
        G2.Columns.Add(GC2.DayDate, "التاريخ")
        G2.Columns.Add(GC2.Value, "القيمة")
        G2.Columns.Add(GC2.SaveName, "الخزنة")
        G2.Columns.Add(GC2.Notes, "الملاحظات")

        G2.Columns(GC2.PaymentsInvoiceNo).ReadOnly = True
        G2.Columns(GC2.DayDate).ReadOnly = True
        G2.Columns(GC2.Value).ReadOnly = True
        G2.Columns(GC2.SaveName).ReadOnly = True
        G2.Columns(GC2.Notes).ReadOnly = True

        G2.RowHeadersVisible = False
        G2.AllowUserToAddRows = False

        AddHandler G2.SelectionChanged, AddressOf G2_SelectionChanged
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ToId.KeyDown, StoreId.KeyDown, SaveId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub ToId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToId.LostFocus
        bm.LostFocus(ToId, ToName, "select Name from " & CustTbl & " where Id=" & ToId.Text.Trim())
        LoadGrid1()
        FillCombo()
    End Sub

    Private Sub ToId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ToId.KeyUp
       If Flag = 35 Then
            If bm.ShowHelp("المستثمرين", ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from Investors", "") Then
                ToId_LostFocus(sender, Nothing)
            End If
        Else
            If bm.ShowHelpCustomers(ToId, ToName, e) Then
                ToId_LostFocus(sender, Nothing)
            End If
        End If
    End Sub

    Public Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
        FillCombo()
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub


    Private Sub btnAddCustomer_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCustomer.Click
        If Flag = 35 Then
            Dim frm As New MyWindow With {.Title = CustTbl, .WindowState = WindowState.Maximized}
            bm.SetMySecurityType(frm, 862)
            frm.Content = New CreditsDebits With {.TableName = "Investors", .MyLinkFile = 15, .MyId = Val(ToId.Text)}
            frm.Show()
        Else
            Dim frm As New MyWindow With {.Title = "Customers", .WindowState = WindowState.Maximized}
            bm.SetMySecurityType(frm, 816)
            frm.Content = New Customers With {.MyId = Val(ToId.Text)}
            frm.Show()
        End If

    End Sub

    Private Sub FillCombo()
        bm.FillCombo("Select 0 Id,'-' Name union select InvoiceNo Id,cast(InvoiceNo as nvarchar(100))+' - '+dbo.ToStrDate(DayDate)+' - '+cast(TotalAfterDiscount as nvarchar(100))+' LE' Name from SalesMaster where Flag=" & Flag & " and StoreId='" & StoreId.Text & "' and ToId='" & ToId.Text & "'", InvoiceNo)
    End Sub

    Private Sub G_SelectionChanged(sender As Object, e As EventArgs)
        If lop Then Return
        Try
            PaymentsInvoiceNo.SelectedValue = G.CurrentRow.Cells(GC.Id).Value
            PaymentsInvoiceNo_LostFocus(Nothing, Nothing)
        Catch
        End Try
    End Sub

    Private Sub SaveId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.KeyUp
        If bm.ShowHelp("Saves", SaveId, SaveName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(5," & Md.UserName & ")") Then
            SaveId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SaveId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.LostFocus
        bm.LostFocus(SaveId, SaveName, "select Name from Fn_EmpPermissions(5," & Md.UserName & ") where Id=" & SaveId.Text.Trim(), True)
    End Sub



    Private Sub Button2_Copy_Click(sender As Object, e As RoutedEventArgs) Handles Button2_Copy.Click
        If G.Rows.Count > 0 Then
            If Not bm.ShowDeleteMSG("سيتم مسح الأقساط الحالية وإعادة تجهيزها .. استمرار؟") Then
                Return
            End If
        End If

        If InstallCount.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد عدد الأقساط")
            InstallCount.Focus()
            Return
        End If


        If PaymentDay.SelectedIndex < 1 OrElse Val(PaymentDay.Text) > 30 Then
            bm.ShowMSG("برجاء تحديد يوم سداد مناسب")
            PaymentDay.Focus()
            Return
        End If

        DownPayment.Text = Val(DownPayment.Text)
        Commission.Text = Val(Commission.Text)

        Dim dd As DateTime = bm.ExecuteScalar("select DayDate from SalesMaster where Flag=" & Flag & " and StoreId='" & StoreId.Text & "' and ToId='" & ToId.Text & "' and InvoiceNo=" & InvoiceNo.SelectedValue)



        Dim vTotal As Decimal = (Val(TotalValue.Text) - Val(DownPayment.Text) + Val(Commission.Text)) * (Val(bm.ExecuteScalar("select 1 + (Perc*0.01) from InstallmentCounts where Id=" & InstallCount.SelectedValue))) + Val(DownPayment.Text)

        Dim v As Decimal = Math.Round((Val(TotalValue.Text) - Val(DownPayment.Text) + Val(Commission.Text)) * (Val(bm.ExecuteScalar("select 1 + (Perc*0.01) from InstallmentCounts where Id=" & InstallCount.SelectedValue))) / Val(InstallCount.Text), 2, MidpointRounding.AwayFromZero)

        v = bm.Rnd5LE(v)

        If Val(InstVal.Text) > 0 Then
            v = Val(InstVal.Text)
        End If

        G.Rows.Clear()
        For i As Integer = 0 To Val(InstallCount.Text)
            Dim x As Integer = i
            If IsThisMonth.IsChecked Then
                x -= 1
            End If
            If i = 0 Then
                G.Rows.Add({0, "مقدم", bm.ToStrDate(dd), bm.ToStrDate(dd), Val(DownPayment.Text), 0, Val(DownPayment.Text), "0"})
            Else
                Try
                    G.Rows.Add({i, "قسط", bm.ToStrDate(New Date(dd.AddMonths(x).Year, dd.AddMonths(x).Month, Val(PaymentDay.Text))), bm.ToStrDate(New Date(dd.AddMonths(x).Year, dd.AddMonths(x).Month, Val(PaymentDay.Text))), v, 0, v, PaymentDay.SelectedValue.ToString})
                Catch
                    G.Rows.Add({i, "قسط", bm.ToStrDate(New Date(dd.AddMonths(x).Year, dd.AddMonths(x).Month, 28)), bm.ToStrDate(New Date(dd.AddMonths(x).Year, dd.AddMonths(x).Month, 28)), v, 0, v, PaymentDay.SelectedValue.ToString})
                End Try
            End If
        Next

        NetValue.Text = v * Val(InstallCount.Text) + Val(DownPayment.Text)
        AddedValue.Text = bm.Rnd(Val(NetValue.Text) - vTotal)

        If Not bm.ExecuteNonQuery("delete InstallmentInvoicesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue & "   insert InstallmentInvoicesMaster(Flag,StoreId,ToId,InvoiceNo,TotalValue,DownPayment,InstallCount,InstVal,PaymentDay,IsThisMonth,DocNo,AddedValue,NetValue,Commission) select " & Flag & "," & StoreId.Text & "," & ToId.Text & "," & InvoiceNo.SelectedValue & "," & TotalValue.Text & "," & DownPayment.Text & "," & InstallCount.Text & "," & Val(InstVal.Text) & "," & PaymentDay.SelectedValue & "," & IIf(IsThisMonth.IsChecked, 1, 0) & ",'" & DocNo.Text & "','" & AddedValue.Text & "','" & NetValue.Text & "','" & Commission.Text & "'") Then Return


        If Not bm.SaveGrid(G, "InstallmentInvoicesDateils", New String() {"Flag", "StoreId", "ToId", "InvoiceNo"}, New String() {Flag, StoreId.Text, ToId.Text, InvoiceNo.SelectedValue}, New String() {"Id", "TypeName", "MainDayDate", "DayDate", "Value", "MainValue", "PaymentDay"}, New String() {GC.Id, GC.TypeName, GC.MainDayDate, GC.DayDate, GC.Value, GC.Value, GC.PaymentDay}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.Date, VariantType.Date, VariantType.Decimal, VariantType.Decimal, VariantType.Integer}, New String() {GC.Id}, "", "") Then Return


        LoadGrid()
    End Sub

    Private Sub InvoiceNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles InvoiceNo.LostFocus
        LoadMaster()
    End Sub

    Private Sub LoadGrid()
        lop = True
        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+TypeName+' - '+dbo.ToStrDate(DayDate)+' - '+cast((Value-Payed) as nvarchar(100))+' LE' Name from (select *,(select isnull(SUM(TT.Value),0) from InstallmentInvoicesDateilsPayments TT where TT.Id=T.Id and TT.Flag=" & Flag & " and TT.StoreId=" & StoreId.Text & " and TT.ToId=" & ToId.Text & " and TT.InvoiceNo=" & InvoiceNo.SelectedValue & ") Payed from InstallmentInvoicesDateils T where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and ToId=" & ToId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue & ")Tbl", PaymentsInvoiceNo)

        dt = bm.ExecuteAdapter("select *,Value-Payed Remaining from (select *,(select isnull(SUM(TT.Value),0) from InstallmentInvoicesDateilsPayments TT where TT.Id=T.Id and TT.Flag=" & Flag & " and TT.StoreId=" & StoreId.Text & " and TT.ToId=" & ToId.Text & " and TT.InvoiceNo=" & InvoiceNo.SelectedValue & ") Payed from InstallmentInvoicesDateils T where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and ToId=" & ToId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue & ")Tbl")

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add({dt.Rows(i)("Id"), dt.Rows(i)("TypeName"), bm.ToStrDate(dt.Rows(i)("MainDayDate")), bm.ToStrDate(dt.Rows(i)("DayDate")), dt.Rows(i)("Value"), dt.Rows(i)("Payed"), dt.Rows(i)("Remaining"), dt.Rows(i)("PaymentDay").ToString})
            If dt.Rows(i)("Remaining") = 0 Then
                G.Rows(i).DefaultCellStyle.BackColor = System.Drawing.Color.Silver
            End If
        Next

        PaymentsInvoiceNo_LostFocus(Nothing, Nothing)
        lop = False
    End Sub
    Dim lop As Boolean = False

    Private Sub LoadMaster()
        dt = bm.ExecuteAdapter("select * from InstallmentInvoicesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and ToId=" & ToId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue)

        Try
            TotalValue.Text = Val(bm.ExecuteScalar("select TotalAfterDiscount from SalesMaster where Flag=" & Flag & " and StoreId='" & StoreId.Text & "' and ToId='" & ToId.Text & "' and InvoiceNo=" & InvoiceNo.SelectedValue))
            DocNo.Text = bm.ExecuteScalar("select DocNo from SalesMaster where Flag=" & Flag & " and StoreId='" & StoreId.Text & "' and ToId='" & ToId.Text & "' and InvoiceNo=" & InvoiceNo.SelectedValue)
        Catch ex As Exception
        End Try

        If dt.Rows.Count = 0 Then

            DownPayment.Clear()
            InstallCount.SelectedIndex = 0
            PaymentDay.SelectedIndex = 0
            IsThisMonth.IsChecked = False
            InstVal.Clear()

            AddedValue.Clear()
            NetValue.Clear()
            Commission.Clear()

            G.Rows.Clear()

            Return
        End If
        'TotalValue.Text = dt.Rows(0)("TotalValue")
        'DocNo.Text = dt.Rows(0)("DocNo").ToString
        DownPayment.Text = dt.Rows(0)("DownPayment")
        InstallCount.Text = dt.Rows(0)("InstallCount")
        PaymentDay.SelectedValue = dt.Rows(0)("PaymentDay")
        AddedValue.Text = dt.Rows(0)("AddedValue").ToString
        NetValue.Text = dt.Rows(0)("NetValue").ToString
        Commission.Text = dt.Rows(0)("Commission").ToString
        InstVal.Text = dt.Rows(0)("InstVal").ToString

        IsThisMonth.IsChecked = IIf(dt.Rows(0)("IsThisMonth") = 1, True, False)

        LoadGrid()
    End Sub


    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If Val(Value.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المبلغ")
            Value.Focus()
            Return
        End If

        If DayDate.SelectedDate Is Nothing Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If

        If Val(SaveId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الخزنة")
            SaveId.Focus()
            Return
        End If

        If Not bm.ShowDeleteMSG("هل أنت متأكد من السداد؟") Then Return

        If Not bm.ExecuteNonQuery("insert InstallmentInvoicesDateilsPayments(Flag,StoreId,ToId,InvoiceNo,Id,PaymentsInvoiceNo,DayDate,Value,Notes,SaveId) select " & Flag & "," & StoreId.Text & "," & ToId.Text & "," & InvoiceNo.SelectedValue & "," & PaymentsInvoiceNo.SelectedValue & ",(select ISNULL((select max(PaymentsInvoiceNo) from InstallmentInvoicesDateilsPayments where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and ToId=" & ToId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue & " and Id=" & PaymentsInvoiceNo.SelectedValue & "),0)+1),'" & bm.ToStrDate(DayDate.SelectedDate) & "'," & Val(Value.Text) & ",'" & Notes.Text.Trim.Replace("'", "''") & "'," & Val(SaveId.Text)) Then Return

        Value.Clear()
        DayDate.SelectedDate = Now.Date
        SaveId.Text = Md.DefaultSave
        Notes.Clear()

        Dim x As Integer = PaymentsInvoiceNo.SelectedValue
        LoadGrid()
        PaymentsInvoiceNo.SelectedValue = x
        LoadGrid2()

    End Sub

    Private Sub LoadGrid2()
        dt = bm.ExecuteAdapter("select *,dbo.GetSubAccNameLink(5,SaveId)SaveName from InstallmentInvoicesDateilsPayments T where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and ToId=" & ToId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue & " and Id=" & PaymentsInvoiceNo.SelectedValue & " ")

        G2.Rows.Clear()

        For i As Integer = 0 To dt.Rows.Count - 1
            G2.Rows.Add({dt.Rows(i)("PaymentsInvoiceNo"), bm.ToStrDate(dt.Rows(i)("DayDate")), dt.Rows(i)("Value"), dt.Rows(i)("SaveName"), dt.Rows(i)("Notes")})
        Next

    End Sub

    Private Sub PaymentsInvoiceNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles PaymentsInvoiceNo.LostFocus
        Try
            If PaymentsInvoiceNo.SelectedValue Is Nothing Then
                PaymentsInvoiceNo.SelectedIndex = 0
            End If
            Value.Text = Val(bm.ExecuteScalar("select Value-Payed Remaining from (select *,(select isnull(SUM(TT.Value),0) from InstallmentInvoicesDateilsPayments TT where TT.Id=T.Id and TT.Flag=" & Flag & " and TT.StoreId=" & StoreId.Text & " and TT.ToId=" & ToId.Text & " and TT.InvoiceNo=" & InvoiceNo.SelectedValue & ") Payed from InstallmentInvoicesDateils T where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and ToId=" & ToId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue & " and Id=" & PaymentsInvoiceNo.SelectedValue & ")Tbl"))
            LoadGrid2()
        Catch
        End Try
    End Sub

    Private Sub G2_SelectionChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click
        If G2.CurrentRow Is Nothing Then Return
        If bm.ShowDeleteMSG("هل أنت متأكد من المسح") Then
            bm.ExecuteNonQuery("delete InstallmentInvoicesDateilsPayments where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue & " and Id=" & PaymentsInvoiceNo.SelectedValue & " and PaymentsInvoiceNo=" & G2.CurrentRow.Cells(GC2.PaymentsInvoiceNo).Value)

            Dim x As Integer = PaymentsInvoiceNo.SelectedValue
            InvoiceNo_LostFocus(Nothing, Nothing)
            PaymentsInvoiceNo.SelectedValue = x
            LoadGrid2()
        End If
    End Sub

    Private Sub btnSave1_Click(sender As Object, e As RoutedEventArgs) Handles btnSave1.Click
        G1.EndEdit()
        If Not bm.SaveGrid(G1, "CustomerInsures", New String() {"CustomerId"}, New String() {ToId.Text}, New String() {"Name", "NationalID", "Mobile", "Job", "Address", "Notes"}, New String() {GC1.Name, GC1.NationalID, GC1.Mobile, GC1.Job, GC1.Address, GC1.Notes}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String}, New String() {GC1.Name}, "", "") Then Return
        bm.ShowMSG("تم الحفظ")
        LoadGrid1()
    End Sub

    Private Sub LoadGrid1()
        dt = bm.ExecuteAdapter("select * from CustomerInsures where CustomerId=" & ToId.Text)

        G1.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G1.Rows.Add({dt.Rows(i)("Name"), dt.Rows(i)("NationalID"), dt.Rows(i)("Mobile"), dt.Rows(i)("Job"), dt.Rows(i)("Address"), dt.Rows(i)("Notes")})
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As RoutedEventArgs) Handles Button2.Click
        Try
            If G1.Rows.Count = 0 Then
                G1.Rows.Add()
            End If
            Dim rpt As New ReportViewer
            rpt.paraname = New String() {"Header", "P0", "P1", "P2", "P3", "P4", "P5", "P6", "ItemName", "Total", "DayDate", "DownPayment", "InstallVal", "InstallCount"}
            rpt.paravalue = New String() {"طلب حصول على نظام تقسيط من شركة " & Md.CompanyName, StoreName.Text, ToName.Text, bm.ExecuteScalar("select Mobile from " & CustTbl & " where Id=" & ToId.Text), bm.ExecuteScalar("select Address from " & CustTbl & " where Id=" & ToId.Text), G1.Rows(0).Cells(GC1.Name).Value, G1.Rows(0).Cells(GC1.Mobile).Value, G1.Rows(0).Cells(GC1.Address).Value, bm.ExecuteScalar("select top 1 ItemName from SalesDetails where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue), TotalValue.Text, G.Rows(1).Cells(GC.DayDate).Value, DownPayment.Text, G.Rows(1).Cells(GC.Value).Value, InstallCount.Text}
            rpt.Rpt = "SalesTemp.rpt"
            rpt.Show()
        Catch
        End Try
    End Sub

    Private Sub btnChange_Click(sender As Object, e As RoutedEventArgs) Handles btnChange.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من تغيير يوم السداد؟") Then
            bm.ExecuteNonQuery("update T set PaymentDay=" & PaymentDay.SelectedValue & " from InstallmentInvoicesDateils T where Id>0 and Flag=" & Flag & " and StoreId=" & StoreId.Text & " and ToId=" & ToId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue & " and not exists(select * from InstallmentInvoicesDateilsPayments TT where TT.Id=T.Id and TT.Flag=T.Flag and TT.StoreId=T.StoreId and TT.ToId=T.ToId and TT.InvoiceNo=T.InvoiceNo)")
            LoadGrid()
        End If
    End Sub

    Private Sub Button2_Copy1_Click(sender As Object, e As RoutedEventArgs) Handles Button2_Copy1.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"Header", "P0", "P1", "P2", "P3", "P4", "P5", "P6", "ItemName", "Total", "DayDate", "DownPayment", "InstallVal", "InstallCount"}
        rpt.paravalue = New String() {"طلب حصول على نظام تقسيط من شركة " & Md.CompanyName, StoreName.Text, "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ", "ــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ"}
        rpt.Rpt = "SalesTemp.rpt"
        rpt.Show()
    End Sub

    Private Sub btnChange_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnChange_Copy.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من ترحيل الأقساط لمدة شهر؟") Then
            bm.ExecuteNonQuery("update T set DayDate=dateAdd(Month,1,DayDate) from InstallmentInvoicesDateils T where Id>0 and Flag=" & Flag & " and StoreId=" & StoreId.Text & " and ToId=" & ToId.Text & " and InvoiceNo=" & InvoiceNo.SelectedValue & " and not exists(select * from InstallmentInvoicesDateilsPayments TT where TT.Id=T.Id and TT.Flag=T.Flag and TT.StoreId=T.StoreId and TT.ToId=T.ToId and TT.InvoiceNo=T.InvoiceNo)")
            LoadGrid()
        End If
    End Sub
End Class