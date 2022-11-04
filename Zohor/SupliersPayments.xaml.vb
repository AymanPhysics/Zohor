Imports System.Data
Imports System.Windows.Forms

Public Class SupliersPayments

    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Dim dv As New DataView
    Dim dv2 As New DataView
    Dim dv3 As New DataView
    WithEvents G As New MyGrid

    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles FromDate.SelectedDateChanged, ToDate.SelectedDateChanged
        If Val(ToId.Text) = 0 OrElse FromDate.SelectedDate Is Nothing OrElse ToDate.SelectedDate Is Nothing Then
            Return
        End If

        Dim dt As DataTable = bm.ExecuteAdapter("GetBankCash_G2_Data", New String() {"ToId", "FromDate", "ToDate"}, {Val(ToId.Text), bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate)})
        dt.TableName = "tbl"

        dv.Table = dt
        G1.ItemsSource = dv

        G1.IsReadOnly = True
        G1.RowHeaderWidth = 0

        G1.MinColumnWidth = 0
        G1.Columns(dt.Columns("Line").Ordinal).Visibility = Visibility.Hidden
        G1.Columns(dt.Columns("BankCash_G2TypeName").Ordinal).Header = "اليومية"
        G1.Columns(dt.Columns("InvoiceNo").Ordinal).Header = "المسلسل"
        G1.Columns(dt.Columns("DayDate").Ordinal).Header = "التاريخ"
        G1.Columns(dt.Columns("DocNo").Ordinal).Header = "رقم المستند"
        G1.Columns(dt.Columns("MainLinkFileName").Ordinal).Header = "الجهة"
        G1.Columns(dt.Columns("MainAccName").Ordinal).Header = "الحساب"
        G1.Columns(dt.Columns("MainValue2").Ordinal).Header = "القيمة"
        G1.Columns(dt.Columns("CurrencyName").Ordinal).Header = "العملة"
        G1.Columns(dt.Columns("Value").Ordinal).Header = "المعادل"
        G1.Columns(dt.Columns("Notes").Ordinal).Header = "ملاحظات"
        G1.Columns(dt.Columns("OrderTypeName").Ordinal).Header = "الطلبية"
        G1.Columns(dt.Columns("ImportMessageId").Ordinal).Header = "الرسالة"
        G1.Columns(dt.Columns("StoreName").Ordinal).Header = "المخزن"
        G1.Columns(dt.Columns("StoreInvoiceNo").Ordinal).Header = "الفاتورة"

        G1.Focus()
        G1_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({ToId})

        LoadWFH()

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

    End Sub
    Structure GC
        Shared StoreId As String = "StoreId"
        Shared StoreName As String = "StoreName"
        Shared InvoiceNo As String = "InvoiceNo"
        Shared Daydate As String = "Daydate"
        Shared TotalAfterDiscount As String = "TotalAfterDiscount"
        Shared Value As String = "Value"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.StoreId, "كود المخزن")
        G.Columns.Add(GC.StoreName, "اسم المخزن")
        G.Columns.Add(GC.InvoiceNo, "مسلسل الفاتورة")
        G.Columns.Add(GC.Daydate, "التاريخ")
        G.Columns.Add(GC.TotalAfterDiscount, "قيمة العقد")
        G.Columns.Add(GC.Value, "المدفوع")

        G.Columns(GC.StoreName).ReadOnly = True
        G.Columns(GC.Daydate).ReadOnly = True
        G.Columns(GC.TotalAfterDiscount).ReadOnly = True

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.SelectionChanged, AddressOf G_SelectionChanged
    End Sub

    Dim lop As Boolean = False
    Private Sub G1_SelectionChanged(sender As Object, e As EventArgs) Handles G1.SelectionChanged
        If G1.SelectedItem Is Nothing Then
            Total0.Clear()
            Return
        End If
        Total0.Text = G1.SelectedItem("MainValue2")
        Dim dt As DataTable = bm.ExecuteAdapter("select * from SupliersPayments where MainLine=" & G1.SelectedItem(0))
        lop = True
        G.Rows.Clear()
        lop = False
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.StoreId).Value = dt.Rows(i)("StoreId")
            GridCalcRow(Nothing, New DataGridViewCellEventArgs(G.Columns(GC.StoreId).Index, i))
            G.Rows(i).Cells(GC.InvoiceNo).Value = dt.Rows(i)("InvoiceNo")
            GridCalcRow(Nothing, New DataGridViewCellEventArgs(G.Columns(GC.InvoiceNo).Index, i))
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value")
        Next
        Calc0()
        G.Focus()
        G_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub G_SelectionChanged(sender As Object, e As EventArgs)
        If lop Then Return
        If G.CurrentRow Is Nothing Then
            G3.ItemsSource = Nothing
            Return
        End If

        Dim dt As DataTable = bm.ExecuteAdapter("GetBankCash_G2_Data2", New String() {"StoreId", "InvoiceNo"}, {Val(G.CurrentRow.Cells(GC.StoreId).Value), Val(G.CurrentRow.Cells(GC.InvoiceNo).Value)})
        dt.TableName = "tbl"

        dv3.Table = dt
        G3.ItemsSource = dv3

        G3.IsReadOnly = True
        G3.RowHeaderWidth = 0

        G3.MinColumnWidth = 0
        G3.Columns(dt.Columns("Line").Ordinal).Visibility = Visibility.Hidden
        G3.Columns(dt.Columns("BankCash_G2TypeName").Ordinal).Header = "اليومية"
        G3.Columns(dt.Columns("InvoiceNo").Ordinal).Header = "المسلسل"
        G3.Columns(dt.Columns("DayDate").Ordinal).Header = "التاريخ"
        G3.Columns(dt.Columns("DocNo").Ordinal).Header = "رقم المستند"
        G3.Columns(dt.Columns("MainLinkFileName").Ordinal).Header = "الجهة"
        G3.Columns(dt.Columns("MainAccName").Ordinal).Header = "الحساب"
        G3.Columns(dt.Columns("MainValue2").Ordinal).Header = "القيمة"
        G3.Columns(dt.Columns("CurrencyName").Ordinal).Header = "العملة"
        G3.Columns(dt.Columns("Value").Ordinal).Header = "المعادل"
        G3.Columns(dt.Columns("Notes").Ordinal).Header = "ملاحظات"
        'G3.Columns(dt.Columns("OrderTypeName").Ordinal).Header = "الطلبية"
        'G3.Columns(dt.Columns("ImportMessageId").Ordinal).Header = "الرسالة"
        'G3.Columns(dt.Columns("StoreName").Ordinal).Header = "المخزن"
        'G3.Columns(dt.Columns("StoreInvoiceNo").Ordinal).Header = "الفاتورة"
        G3.Columns(dt.Columns("TT_Value").Ordinal).Header = "المدفوع"

        G3.Focus()


        Total.Text = G.CurrentRow.Cells(GC.TotalAfterDiscount).Value
        Dim x As Decimal = 0
        For i As Integer = 0 To dt.Rows.Count - 1
            x += Val(dt.Rows(i)("TT_Value"))
        Next
        Paid.Text = x
        Remaining.Text = Val(Total.Text) - Val(x)

    End Sub


    Private Sub GridKeyDown(sender As Object, e As KeyEventArgs)
        If G.CurrentCell Is Nothing Then Return
        If G.CurrentCell.ColumnIndex = G.Columns(GC.StoreId).Index Then
            If bm.ShowHelpGrid("Stores", G.CurrentRow.Cells(GC.StoreId), G.CurrentRow.Cells(GC.StoreName), e, "Select Id,name From Stores") Then
                G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.StoreName)
            End If
        ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.InvoiceNo).Index Then
            If bm.ShowHelpGridMultiColumns("Invoices", G.CurrentRow.Cells(GC.InvoiceNo), G.CurrentRow.Cells(GC.InvoiceNo), e, "select distinct M.InvoiceNo 'الفاتورة',dbo.ToStrDate(M.DayDate) 'التاريخ',dbo.GetOrderTypes(OrderTypeId)'الطلبية',M.TotalAfterDiscount 'القيمة',dbo.GetCurrencyName(CurrencyId)'العملة' from SalesMaster M where M.Temp=0 and M.StoreId=" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.StoreId).Value & " and M.Flag=19 and M.ToId=" & Val(ToId.Text)) Then
                G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Value)

                G.Rows(G.CurrentCell.RowIndex).Cells(GC.InvoiceNo).Value = bm.SelectedRow(0)
                G.Rows(G.CurrentCell.RowIndex).Cells(GC.Daydate).Value = bm.SelectedRow(1)
                G.Rows(G.CurrentCell.RowIndex).Cells(GC.TotalAfterDiscount).Value = bm.SelectedRow(2)

            End If

        End If
    End Sub

    Private Sub GridCalcRow(sender As Object, e As DataGridViewCellEventArgs)
        If e.ColumnIndex = G.Columns(GC.StoreId).Index Then
            G.Rows(e.RowIndex).Cells(GC.StoreId).Value = Val(G.Rows(e.RowIndex).Cells(GC.StoreId).Value)
            G.Rows(e.RowIndex).Cells(GC.StoreName).Value = bm.ExecuteScalar("select Name from Stores where Id=" & G.Rows(e.RowIndex).Cells(GC.StoreId).Value)
            If Val(G.Rows(e.RowIndex).Cells(GC.StoreId).Value) = 0 Then
                G.Rows(e.RowIndex).Cells(GC.StoreId).Value = ""
                G.Rows(e.RowIndex).Cells(GC.StoreName).Value = ""
                G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value = ""
                G.Rows(e.RowIndex).Cells(GC.Daydate).Value = ""
                G.Rows(e.RowIndex).Cells(GC.TotalAfterDiscount).Value = ""
                G.Rows(e.RowIndex).Cells(GC.Value).Value = ""
            End If
        ElseIf e.ColumnIndex = G.Columns(GC.InvoiceNo).Index Then
            G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value = Val(G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value)
            Dim dt As DataTable = bm.ExecuteAdapter("select dbo.ToStrDate(Daydate),TotalAfterDiscount from SalesMaster where Flag=19 and StoreId=" & G.Rows(e.RowIndex).Cells(GC.StoreId).Value & " and InvoiceNo=" & G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value)
            If dt.Rows.Count = 0 Then
                G.Rows(e.RowIndex).Cells(GC.Daydate).Value = ""
                G.Rows(e.RowIndex).Cells(GC.TotalAfterDiscount).Value = ""
            Else
                G.Rows(e.RowIndex).Cells(GC.Daydate).Value = dt.Rows(0)(0)
                G.Rows(e.RowIndex).Cells(GC.TotalAfterDiscount).Value = dt.Rows(0)(1)
            End If
        ElseIf e.ColumnIndex = G.Columns(GC.Value).Index Then
            G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value)
        End If
        Calc0()

    End Sub

    Sub Calc0()
        Dim x As Decimal = 0
        For i As Integer = 0 To G.Rows.Count - 1
            x += Val(G.Rows(i).Cells(GC.Value).Value)
        Next
        Paid0.Text = x
        Remaining0.Text = Val(Total0.Text) - x
    End Sub

    Private Sub LoadResource()
        'Button2.SetResourceReference(ContentProperty, "View Report")
        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")

    End Sub


    Private Sub ToId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ToId.KeyUp
        Dim dt As DataTable = bm.ExecuteAdapter("select * from LinkFile where Id=" & 2)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & 2 & "," & Md.UserName & ")") Then
            ToId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub ToId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToId.LostFocus
        bm.LostFocus(ToId, ToName, "select Name from Fn_EmpPermissions(" & 2 & "," & Md.UserName & ") where Id=" & ToId.Text.Trim())
        Button2_Click(Nothing, Nothing)
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ToId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If G1.SelectedItem Is Nothing Then
            Return
        End If
        G.EndEdit()
        Dim str As String = "delete SupliersPayments where MainLine=" & G1.SelectedItem(0)
        For i As Integer = 0 To G.Rows.Count - 1
            If Val(G.Rows(i).Cells(GC.StoreId).Value) = 0 Then Continue For
            If Val(G.Rows(i).Cells(GC.InvoiceNo).Value) = 0 Then Continue For
            str &= " insert SupliersPayments(MainLine,StoreId,InvoiceNo,Value,UserName,MyGetDate) select " & G1.SelectedItem(0) & "," & Val(G.Rows(i).Cells(GC.StoreId).Value) & "," & Val(G.Rows(i).Cells(GC.InvoiceNo).Value) & "," & Val(G.Rows(i).Cells(GC.Value).Value) & "," & Md.UserName & ",getdate() "
        Next
        If bm.ExecuteNonQuery(str) Then
            bm.ShowMSG("تمت العملية بنجاح")
        End If
    End Sub
End Class