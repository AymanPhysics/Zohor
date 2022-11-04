Imports System.Data
Imports System.Windows.Forms

Public Class SellersProfits
    Public TableName As String = "SellersProfits"
    Public SubId As String = "InvoiceNo"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        bm.Fields = {SubId, "DayDate", "FromDate", "ToDate", "CashierId"}
        bm.control = {txtID, DayDate, FromDate, ToDate, CashierId}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName

        LoadWFH()
        btnNew_Click(sender, e)
        DayDate.Focus()
    End Sub



    Structure GC
        Shared StoreId As String = "StoreId"
        Shared StoreName As String = "StoreName"
        Shared SalesFlagId As String = "SalesFlagId"
        Shared FlagName As String = "FlagName"
        Shared SalesInvoiceNo As String = "SalesInvoiceNo"

        Shared SalesDayDate As String = "SalesDayDate"
        Shared ToId As String = "ToId"
        Shared ToName As String = "ToName"

        Shared TotalAfterDiscount As String = "TotalAfterDiscount"
        Shared ReturnValue As String = "ReturnValue"
        Shared Payed As String = "Payed"
        Shared Remaining As String = "Remaining"
        Shared Perc As String = "Perc"
        Shared PercValue As String = "PercValue"

        Shared Notes As String = "Notes"
        Shared Line As String = "Line"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.StoreId, "StoreId")
        G.Columns.Add(GC.StoreName, "المخزن")
        G.Columns.Add(GC.SalesFlagId, "Flag")
        G.Columns.Add(GC.FlagName, "النوع")
        G.Columns.Add(GC.SalesInvoiceNo, "رقم الفاتورة")
        G.Columns.Add(GC.SalesDayDate, "التاريخ")
        G.Columns.Add(GC.ToId, "كود العميل")
        G.Columns.Add(GC.ToName, "اسم العميل")
        G.Columns.Add(GC.TotalAfterDiscount, "القيمة")
        G.Columns.Add(GC.ReturnValue, "المردود")
        G.Columns.Add(GC.Payed, "المدفوع")
        G.Columns.Add(GC.Remaining, "المتبقي")
        G.Columns.Add(GC.Perc, "النسبة")
        G.Columns.Add(GC.PercValue, "العمولة")

        G.Columns.Add(GC.Notes, "ملاحظات")
        G.Columns.Add(GC.Line, "Line")


        G.Columns(GC.ToName).FillWeight = 200
        G.Columns(GC.Notes).FillWeight = 300

        G.Columns(GC.StoreId).ReadOnly = True
        G.Columns(GC.StoreName).ReadOnly = True
        G.Columns(GC.SalesFlagId).ReadOnly = True
        G.Columns(GC.FlagName).ReadOnly = True
        G.Columns(GC.SalesInvoiceNo).ReadOnly = True
        G.Columns(GC.SalesDayDate).ReadOnly = True
        G.Columns(GC.ToId).ReadOnly = True
        G.Columns(GC.ToName).ReadOnly = True
        G.Columns(GC.TotalAfterDiscount).ReadOnly = True
        G.Columns(GC.ReturnValue).ReadOnly = True
        G.Columns(GC.Payed).ReadOnly = True
        G.Columns(GC.Remaining).ReadOnly = True


        G.Columns(GC.StoreId).Visible = False
        G.Columns(GC.SalesFlagId).Visible = False
        G.Columns(GC.Line).Visible = False

        G.AllowUserToAddRows = False

        AddHandler G.CellEndEdit, AddressOf G_CellEndEdit
    End Sub

    Private Sub G_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        If e.ColumnIndex = G.Columns(GC.Perc).Index Then
            Dim i As Integer = e.RowIndex
            G.Rows(i).Cells(GC.PercValue).Value = Val(G.Rows(i).Cells(GC.Perc).Value) * (G.Rows(i).Cells(GC.TotalAfterDiscount).Value - G.Rows(i).Cells(GC.ReturnValue).Value)
        End If

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True


        bm.FillControls(Me)
        CashierId_LostFocus(Nothing, Nothing)


        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & TableName & " where " & SubId & "=" & txtID.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()

            G.Rows(i).Cells(GC.StoreId).Value = dt.Rows(i)("StoreId").ToString
            G.Rows(i).Cells(GC.StoreName).Value = dt.Rows(i)("StoreName").ToString
            G.Rows(i).Cells(GC.SalesFlagId).Value = dt.Rows(i)("SalesFlagId").ToString
            G.Rows(i).Cells(GC.FlagName).Value = dt.Rows(i)("FlagName").ToString
            G.Rows(i).Cells(GC.SalesInvoiceNo).Value = dt.Rows(i)("SalesInvoiceNo").ToString
            G.Rows(i).Cells(GC.SalesDayDate).Value = bm.ToStrDate(dt.Rows(i)("SalesDayDate"))
            G.Rows(i).Cells(GC.ToId).Value = dt.Rows(i)("ToId").ToString
            G.Rows(i).Cells(GC.ToName).Value = dt.Rows(i)("ToName").ToString
            G.Rows(i).Cells(GC.TotalAfterDiscount).Value = dt.Rows(i)("TotalAfterDiscount").ToString
            G.Rows(i).Cells(GC.ReturnValue).Value = dt.Rows(i)("ReturnValue").ToString
            G.Rows(i).Cells(GC.Payed).Value = dt.Rows(i)("Payed").ToString
            G.Rows(i).Cells(GC.Remaining).Value = dt.Rows(i)("Remaining").ToString
            G.Rows(i).Cells(GC.Perc).Value = dt.Rows(i)("Perc").ToString
            G.Rows(i).Cells(GC.PercValue).Value = dt.Rows(i)("PercValue").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
        Next
        DayDate.Focus()
        G.RefreshEdit()
        lop = False

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False

        G.EndEdit()


        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If

        If Not IsDate(FromDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            FromDate.Focus()
            Return
        End If

        If Not IsDate(ToDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            ToDate.Focus()
            Return
        End If

        If Val(CashierId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد البائع")
            CashierId.Focus()
            Return
        End If

        bm.DefineValues()

        If Not bm.SaveGrid(G, TableName, New String() {SubId}, New String() {txtID.Text}, New String() {"StoreId", "StoreName", "SalesFlagId", "FlagName", "SalesInvoiceNo", "SalesDayDate", "ToId", "ToName", "TotalAfterDiscount", "ReturnValue", "Payed", "Remaining", "Perc", "PercValue", "Notes"}, New String() {GC.StoreId, GC.StoreName, GC.SalesFlagId, GC.FlagName, GC.SalesInvoiceNo, GC.SalesDayDate, GC.ToId, GC.ToName, GC.TotalAfterDiscount, GC.ReturnValue, GC.Payed, GC.Remaining, GC.Perc, GC.PercValue, GC.Notes}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Date, VariantType.Integer, VariantType.String, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.String}, New String() {}, "Line", "Line", True) Then Return




        'If Not bm.Save(New String() {SubId}, New String() {txtID.Text}) Then Return

        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True
    End Sub

    Dim lop As Boolean = False

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        If lop OrElse lv Then Return
        lop = True
        bm.ClearControls()
        CashierId_LostFocus(Nothing, Nothing)


        DayDate.SelectedDate = bm.MyGetDate()
        FromDate.SelectedDate = bm.MyGetDate()
        ToDate.SelectedDate = bm.MyGetDate()
        G.Rows.Clear()


        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"
        'DayDate.Focus()
        txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If sender Is Nothing OrElse bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "=" & txtID.Text)
            If Not sender Is Nothing Then
                btnNew_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblID.SetResourceReference(ContentProperty, "Id")

        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
    End Sub


    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) 'Handles btnPrint.Click, btnPrint2.Click, btnPrint3.Click
        DontClear = True
        If btnSave.IsEnabled AndAlso btnSave.Visibility = Visibility.Visible Then
            btnSave_Click(sender, e)
        Else
            AllowSave = True
        End If
        DontClear = False
        If Not AllowSave Then Return

        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@InvoiceNo", "Header"}
        rpt.paravalue = New String() {CType(Parent, Page).Title}
        rpt.Rpt = ".rpt"
        rpt.Show()
    End Sub


    Private Sub btncalc_Click(sender As Object, e As RoutedEventArgs) Handles btncalc.Click
        Dim CustDt As DataTable = bm.ExecuteAdapter("getSellerCustomersBal", {"ToDate", "CashierId"}, {bm.ToStrDate(ToDate.SelectedDate), Val(CashierId.Text)})

        Dim InvoicesDt As DataTable = bm.ExecuteAdapter("getSellerSalesInvoices", {"FromDate", "ToDate", "CashierId"}, {bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate), Val(CashierId.Text)})


        G.Rows.Clear()
        For i As Integer = 0 To InvoicesDt.Rows.Count - 1
            G.Rows.Add()

            G.Rows(i).Cells(GC.StoreId).Value = InvoicesDt.Rows(i)("StoreId").ToString
            G.Rows(i).Cells(GC.StoreName).Value = InvoicesDt.Rows(i)("StoreName").ToString
            G.Rows(i).Cells(GC.SalesFlagId).Value = InvoicesDt.Rows(i)("Flag").ToString
            G.Rows(i).Cells(GC.FlagName).Value = InvoicesDt.Rows(i)("FlagName").ToString
            G.Rows(i).Cells(GC.SalesInvoiceNo).Value = InvoicesDt.Rows(i)("InvoiceNo").ToString
            G.Rows(i).Cells(GC.SalesDayDate).Value = InvoicesDt.Rows(i)("DayDate")
            G.Rows(i).Cells(GC.ToId).Value = InvoicesDt.Rows(i)("ToId").ToString
            G.Rows(i).Cells(GC.ToName).Value = InvoicesDt.Rows(i)("ToName").ToString
            G.Rows(i).Cells(GC.TotalAfterDiscount).Value = InvoicesDt.Rows(i)("TotalAfterDiscount").ToString
            G.Rows(i).Cells(GC.ReturnValue).Value = InvoicesDt.Rows(i)("ReturnValue").ToString
            G.Rows(i).Cells(GC.Payed).Value = Val(G.Rows(i).Cells(GC.TotalAfterDiscount).Value) - Val(G.Rows(i).Cells(GC.ReturnValue).Value)
            G.Rows(i).Cells(GC.Remaining).Value = 0
            G.Rows(i).Cells(GC.Perc).Value = Nothing
            G.Rows(i).Cells(GC.PercValue).Value = Nothing
            G.Rows(i).Cells(GC.Notes).Value = ""
            G.Rows(i).Cells(GC.Line).Value = 0
        Next


        For i As Integer = 0 To CustDt.Rows.Count - 1
            For x As Integer = G.Rows.Count - 1 To 0 Step -1
                If Val(CustDt.Rows(i)("Id")) = Val(G.Rows(x).Cells(GC.ToId).Value) Then
                    If Val(CustDt.Rows(i)("MainBal0")) > 0 Then
                        If Val(G.Rows(x).Cells(GC.TotalAfterDiscount).Value) - Val(G.Rows(x).Cells(GC.ReturnValue).Value) < Val(CustDt.Rows(i)("MainBal0")) Then
                            G.Rows(x).Cells(GC.Remaining).Value = Val(G.Rows(x).Cells(GC.TotalAfterDiscount).Value) - Val(G.Rows(x).Cells(GC.ReturnValue).Value)
                            G.Rows(x).Cells(GC.Payed).Value = 0
                            CustDt.Rows(i)("MainBal0") -= G.Rows(x).Cells(GC.Remaining).Value
                        Else
                            G.Rows(x).Cells(GC.Remaining).Value = CustDt.Rows(i)("MainBal0")
                            G.Rows(x).Cells(GC.Payed).Value = Val(G.Rows(x).Cells(GC.TotalAfterDiscount).Value) - Val(G.Rows(x).Cells(GC.ReturnValue).Value) - G.Rows(x).Cells(GC.Remaining).Value
                            CustDt.Rows(i)("MainBal0") = 0
                        End If
                    End If
                End If
            Next
        Next


    End Sub

    Private Sub CashierId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CashierId.LostFocus
        bm.LostFocus(CashierId, CashierName, "select " & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Id=" & CashierId.Text.Trim() & " and Stopped=0 and Cashier=1")
    End Sub

    Private Sub CashierId_KeyUp(sender As Object, e As Input.KeyEventArgs) Handles CashierId.KeyUp
        bm.ShowHelp("Employees", CashierId, CashierName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Stopped=0 and Cashier=1")
    End Sub
End Class
