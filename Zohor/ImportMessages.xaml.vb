Imports System.Data

Public Class ImportMessages

    Public TableNameDetails As String = "ImportMessagesDetails"
    Public TableNameDetailsSub As String = "ImportMessagesDetailsSub"
    Public TableNameDetailsSub2 As String = "ImportMessagesDetailsSub2"
    Public TableName As String = "ImportMessages"
    Public MainId As String = "OrderTypeId"
    Public SubId As String = "Id" 

    Dim dt As New DataTable
    Dim bm As New BasicMethods
    WithEvents G As New MyGrid
    WithEvents G1 As New MyGrid
    WithEvents G2 As New MyGrid

    Private Sub LabTestItems_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnPrint, btnPrint2, btnPrint2Images, btnPrint3, btnPrint3Images, btnPrintImages})

        bm.FillCombo("select Id,Name from Currencies order by Id", ContainerSizeCurrencyId)
        bm.FillCombo("Shippers", ShipperId, "")
        bm.FillCombo("ShippingLines", ShippingLineId, "")
        bm.FillCombo("ShippingCompanies", ShippingCompanyId, "")
        bm.FillCombo("ContainerSizes", ContainerSizeId, "")
        bm.FillCombo("Employees", EntryUser, "", , True)

        LoadResource()
        LoadWFH()
        LoadWFH1()
        LoadWFH2()
        bm.Fields = New String() {MainId, SubId, "AccNo", "DayDate", "IsDelivered", "DeliveredDate", "StoreId", "ShipperId", "ShippingLineId", "ShippingCompanyId", "ContainerSizeId", "ContainerPrice", "ContainerSizeCurrencyId", "PolisaData", "Perc", "Cost_InvoiceNo", "Cost_MessageId", "Cost_OrderTypeId", "Cost_Administrative", "Bal0", "val5", "CertificateNo", "EntryUser"}
        bm.control = New Control() {OrderTypeId, txtID, MainAccNo, DayDate, IsDelivered, DeliveredDate, StoreId, ShipperId, ShippingLineId, ShippingCompanyId, ContainerSizeId, ContainerPrice, ContainerSizeCurrencyId, PolisaData, Perc, Cost_InvoiceNo, Cost_MessageId, Cost_OrderTypeId, Cost_Administrative, Bal0, val5, CertificateNo, EntryUser}
        bm.KeyFields = New String() {MainId, SubId}

        If Not Md.ShowBarcode Then
            btnPrintBarcode.Visibility = Visibility.Hidden
            Barcode.Visibility = Visibility.Hidden
            lblBarcode.Visibility = Visibility.Hidden
        End If

        lblCost_Administrative.Visibility = Visibility.Hidden
        Cost_Administrative.Visibility = Visibility.Hidden
        btnCalc2.Visibility = Visibility.Hidden

        bm.Table_Name = TableName
        IsDelivered_UnChecked(Nothing, Nothing)
        btnNew_Click(sender, e)
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub

    Structure GC
        Shared InvoiceNo As String = "InvoiceNo"
        Shared SupplierId As String = "SupplierId"
        Shared SupplierName As String = "SupplierName"
        Shared DocNo As String = "DocNo"
        Shared DeliveryDate As String = "DeliveryDate"
        Shared TotalAfterDiscount As String = "TotalAfterDiscount"
        Shared CurrencyName As String = "CurrencyName"
        Shared Perc As String = "Perc"
        Shared Line As String = "Line"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G
        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.InvoiceNo, "رقم الفاتورة")
        G.Columns.Add(GC.SupplierId, "كود المورد")
        G.Columns.Add(GC.SupplierName, "اسم المورد")
        G.Columns.Add(GC.DocNo, "رقم عقد المورد")
        G.Columns.Add(GC.DeliveryDate, "تاريخ الاستلام")
        G.Columns.Add(GC.TotalAfterDiscount, "القيمة")
        G.Columns.Add(GC.CurrencyName, "العملة")
        G.Columns.Add(GC.Perc, "المعامل")
        G.Columns.Add(GC.Line, "Line")

        G.Columns(GC.SupplierName).FillWeight = 220
        G.Columns(GC.SupplierId).ReadOnly = True
        G.Columns(GC.SupplierName).ReadOnly = True
        G.Columns(GC.DocNo).ReadOnly = True
        G.Columns(GC.DeliveryDate).ReadOnly = True
        G.Columns(GC.TotalAfterDiscount).ReadOnly = True
        G.Columns(GC.CurrencyName).ReadOnly = True
        G.Columns(GC.Perc).ReadOnly = True
        G.Columns(GC.Line).Visible = False

        G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G.AllowUserToDeleteRows = True
        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G.TabStop = False

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.SelectionChanged, AddressOf GridSelectionChanged
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Structure GC1
        Shared InvoiceNo As String = "InvoiceNo"
        Shared ItemId As String = "ItemId"
        Shared ItemName As String = "ItemName"
        Shared ItemUnit As String = "ItemUnit"
        Shared Qty As String = "Qty"
        Shared Qty2 As String = "Qty2"
        Shared Qty3 As String = "Qty3"
        Shared ReturnQty As String = "ReturnQty"
        Shared NetQty As String = "NetQty"
        Shared QtyRecieved As String = "QtyRecieved"
        Shared Remaining As String = "Remaining"
        Shared Price As String = "Price"
        Shared Value As String = "Value"
        Shared SerialNo As String = "SerialNo"
    End Structure

    Private Sub LoadWFH1()
        WFH1.Child = G1
        G1.Columns.Clear()
        G1.ForeColor = System.Drawing.Color.DarkBlue
        G1.Columns.Add(GC1.InvoiceNo, "رقم الفاتورة")
        G1.Columns.Add(GC1.ItemId, "كود الصنف")
        G1.Columns.Add(GC1.ItemName, "اسم الصنف")
        G1.Columns.Add(GC1.ItemUnit, "الوحدة")
        G1.Columns.Add(GC1.Qty, "الكمية")
        G1.Columns.Add(GC1.ReturnQty, "المردود")
        G1.Columns.Add(GC1.Qty2, "العدد/عبوة")
        G1.Columns.Add(GC1.Qty3, "عدد العبوات")
        G1.Columns.Add(GC1.NetQty, "الصافي")
        G1.Columns.Add(GC1.QtyRecieved, "الكمية المستلمة")
        G1.Columns.Add(GC1.Remaining, "المتبقي")
        G1.Columns.Add(GC1.Price, "السعر")
        G1.Columns.Add(GC1.Value, "القيمة")
        G1.Columns.Add(GC1.SerialNo, "رقم الإذن")


        G1.Columns(GC1.ItemName).FillWeight = 300
        G1.Columns(GC1.InvoiceNo).ReadOnly = True
        G1.Columns(GC1.ItemId).ReadOnly = True
        G1.Columns(GC1.ItemName).ReadOnly = True
        G1.Columns(GC1.ItemUnit).ReadOnly = True
        G1.Columns(GC1.Qty).ReadOnly = True
        G1.Columns(GC1.Qty2).ReadOnly = True
        G1.Columns(GC1.Qty3).ReadOnly = True
        G1.Columns(GC1.ReturnQty).ReadOnly = True
        G1.Columns(GC1.NetQty).ReadOnly = True
        G1.Columns(GC1.Remaining).ReadOnly = True

        G1.Columns(GC1.Qty2).Visible = False
        G1.Columns(GC1.Qty3).Visible = False
        G1.Columns(GC1.QtyRecieved).Visible = False
        G1.Columns(GC1.Price).Visible = False
        G1.Columns(GC1.Value).Visible = False
        G1.Columns(GC1.SerialNo).Visible = False

        G1.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G1.AllowUserToAddRows = False
        G1.AllowUserToDeleteRows = False
        G1.TabStop = False

        AddHandler G1.CellDoubleClick, AddressOf G1_CellDoubleClick
        AddHandler G1.CellEndEdit, AddressOf G1_CellEndEdit
        AddHandler G1.RowsRemoved, AddressOf G1_RowsRemoved

    End Sub

    Structure GC2
        Shared InvoiceNo As String = "InvoiceNo"
        Shared ItemId As String = "ItemId"
        Shared ItemName As String = "ItemName"
        Shared ItemUnit As String = "ItemUnit"
        Shared Qty As String = "Qty"
        Shared Qty2 As String = "Qty2"
        Shared Qty3 As String = "Qty3"
        Shared ReturnQty As String = "ReturnQty"
        Shared NetQty As String = "NetQty"
        Shared QtyRecieved As String = "QtyRecieved"
        Shared Price As String = "Price"
        Shared Value As String = "Value"
        Shared AvgCost As String = "AvgCost"
        Shared SerialNo As String = "SerialNo"
        Shared Line As String = "Line"
    End Structure

    Private Sub LoadWFH2()
        WFH2.Child = G2
        G2.Columns.Clear()
        G2.ForeColor = System.Drawing.Color.DarkBlue
        G2.Columns.Add(GC2.InvoiceNo, "رقم الفاتورة")
        G2.Columns.Add(GC2.ItemId, "كود الصنف")
        G2.Columns.Add(GC2.ItemName, "اسم الصنف")
        G2.Columns.Add(GC2.ItemUnit, "الوحدة")
        G2.Columns.Add(GC2.Qty, "الكمية")
        G2.Columns.Add(GC2.Qty2, "العدد/عبوة")
        G2.Columns.Add(GC2.Qty3, "عدد العبوات")
        G2.Columns.Add(GC2.ReturnQty, "المردود")
        G2.Columns.Add(GC2.NetQty, "الصافي")
        G2.Columns.Add(GC2.QtyRecieved, "الكمية المستلمة")
        G2.Columns.Add(GC2.Price, "السعر")
        G2.Columns.Add(GC2.Value, "القيمة")
        G2.Columns.Add(GC2.AvgCost, "التكلفة")
        G2.Columns.Add(GC2.SerialNo, "رقم الإذن")
        G2.Columns.Add(GC2.Line, "Line")


        G2.Columns(GC2.ItemName).FillWeight = 300
        G2.Columns(GC2.InvoiceNo).ReadOnly = True
        G2.Columns(GC2.ItemId).ReadOnly = True
        G2.Columns(GC2.ItemName).ReadOnly = True
        G2.Columns(GC2.ItemUnit).ReadOnly = True
        'G2.Columns(GC2.Qty).ReadOnly = True
        'G2.Columns(GC2.Qty2).ReadOnly = True
        G2.Columns(GC2.Qty3).ReadOnly = True
        G2.Columns(GC2.ReturnQty).ReadOnly = True
        G2.Columns(GC2.NetQty).ReadOnly = True
        G2.Columns(GC2.QtyRecieved).Visible = False
        G2.Columns(GC2.Price).Visible = False
        G2.Columns(GC2.Value).Visible = False
        G2.Columns(GC2.AvgCost).Visible = False
        'G2.Columns(GC2.SerialNo).ReadOnly = True
        G2.Columns(GC2.ReturnQty).Visible = False
        G2.Columns(GC2.NetQty).Visible = False
        G2.Columns(GC2.Line).Visible = False

        G2.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G2.AllowUserToAddRows = False
        G2.AllowUserToDeleteRows = True
        G2.TabStop = False

        AddHandler G2.CellEndEdit, AddressOf G2_CellEndEdit
        AddHandler G2.RowsRemoved, AddressOf G2_RowsRemoved
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {OrderTypeId.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtID.Text.Trim = "" Or OrderTypeId.Text.Trim = "" Then
            Return
        End If


        If ShipperId.Visibility = Visibility.Visible AndAlso ShipperId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد " & lblShipperId.Content)
            ShipperId.Focus()
            Return
        End If
        If ShippingLineId.Visibility = Visibility.Visible AndAlso ShippingLineId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد " & lblShippingLineId.Content)
            ShippingLineId.Focus()
            Return
        End If
        If ShippingCompanyId.Visibility = Visibility.Visible AndAlso ShippingCompanyId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد " & lblShippingCompanyId.Content)
            ShippingCompanyId.Focus()
            Return
        End If
        If ContainerSizeId.Visibility = Visibility.Visible AndAlso ContainerSizeId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد " & lblContainerSizeId.Content)
            ContainerSizeId.Focus()
            Return
        End If



        G.EndEdit()
        G1.EndEdit()
        G2.EndEdit()

        ContainerPrice.Text = Val(ContainerPrice.Text)
        Perc.Text = Val(Perc.Text)
        Cost_InvoiceNo.Text = Val(Cost_InvoiceNo.Text)
        Cost_MessageId.Text = Val(Cost_MessageId.Text)
        Cost_OrderTypeId.Text = Val(Cost_OrderTypeId.Text)
        Cost_Administrative.Text = Val(Cost_Administrative.Text)
        val5.Text = Val(val5.Text)

        If EntryUser.SelectedValue = 0 Then EntryUser.SelectedValue = Md.UserName


        bm.DefineValues()
        If Not bm.Save(New String() {MainId, SubId}, New String() {OrderTypeId.Text, txtID.Text.Trim}) Then Return

        Dim str As String = "delete " & TableNameDetails & " where " & MainId & "=" & OrderTypeId.Text & " and " & SubId & "='" & Val(txtID.Text) & "' "
        bm.ExecuteNonQuery(str)

        str = ""
        For i As Integer = 0 To G.Rows.Count - 1
            If G.Rows(i).Cells(GC.InvoiceNo).Value Is Nothing OrElse G.Rows(i).Cells(GC.InvoiceNo).Value.ToString.Trim = "" Then Continue For
            Dim InsertIdentity As Boolean = False
            If Val(G.Rows(i).Cells("Line").Value) = 0 Then
                str &= " set identity_insert " & TableNameDetails & " off "
                InsertIdentity = True
            Else
                str &= " set identity_insert " & TableNameDetails & " on "
            End If

            str &= " Insert " & TableNameDetails & "(" & MainId & "," & SubId & ",StoreId,InvoiceNo,SupplierId,SupplierName,DocNo,DeliveryDate,Perc"
            If Not InsertIdentity Then str &= ",Line"
            str &= ") values "
            Try
                Dim x As String = "1900-1-1"
                If G.Rows(i).Cells(GC.DeliveryDate).Value.ToString <> "" Then
                    x = bm.ToStrDate(DateTime.Parse(G.Rows(i).Cells(GC.DeliveryDate).Value.ToString))
                End If
                str &= "('" & OrderTypeId.Text & "','" & Val(txtID.Text) & "','" & Val(StoreId.Text) & "','" & G.Rows(i).Cells(GC.InvoiceNo).Value.ToString & "','" & G.Rows(i).Cells(GC.SupplierId).Value.ToString & "','" & G.Rows(i).Cells(GC.SupplierName).Value.ToString & "','" & G.Rows(i).Cells(GC.DocNo).Value.ToString & "','" & x & "','" & G.Rows(i).Cells(GC.Perc).Value & "'"
                If Not InsertIdentity Then str &= "," & Val(G.Rows(i).Cells(GC.Line).Value)
                str &= ")"
            Catch ex As Exception
            End Try
        Next
        bm.ExecuteNonQuery(str)


        str = " delete " & TableNameDetailsSub & " where " & MainId & "=" & OrderTypeId.Text & " and " & SubId & "='" & Val(txtID.Text) & "' "
        bm.ExecuteNonQuery(str)
        str = " Insert " & TableNameDetailsSub & "(" & MainId & "," & SubId & ",StoreId,InvoiceNo,ItemId,ItemName,Qty,Qty2,Qty3,ReturnQty,NetQty,QtyRecieved,Price,Value,SerialNo) values "
        For i As Integer = 0 To G1.Rows.Count - 1
            Try
                str &= "('" & OrderTypeId.Text & "','" & Val(txtID.Text) & "','" & Val(StoreId.Text) & "','" & G1.Rows(i).Cells(GC1.InvoiceNo).Value.ToString & "','" & G1.Rows(i).Cells(GC1.ItemId).Value.ToString & "','" & G1.Rows(i).Cells(GC1.ItemName).Value.ToString & "','" & G1.Rows(i).Cells(GC1.Qty).Value.ToString & "','" & G1.Rows(i).Cells(GC1.Qty2).Value.ToString & "','" & G1.Rows(i).Cells(GC1.Qty3).Value.ToString & "','" & G1.Rows(i).Cells(GC1.ReturnQty).Value.ToString & "','" & G1.Rows(i).Cells(GC1.NetQty).Value.ToString & "','" & Val(G1.Rows(i).Cells(GC1.QtyRecieved).Value) & "','" & G1.Rows(i).Cells(GC1.Price).Value.ToString & "','" & G1.Rows(i).Cells(GC1.Value).Value.ToString & "','" & Val(G1.Rows(i).Cells(GC1.SerialNo).Value) & "'),"
            Catch ex As Exception
            End Try
        Next
        str = str.Substring(0, str.Length - 1)
        bm.ExecuteNonQuery(str)

        str = " delete " & TableNameDetailsSub2 & " where " & MainId & "=" & OrderTypeId.Text & " and " & SubId & "='" & Val(txtID.Text) & "' "
        bm.ExecuteNonQuery(str)

        str = ""
        For i As Integer = 0 To G2.Rows.Count - 1
            Dim InsertIdentity As Boolean = False
            If Val(G2.Rows(i).Cells("Line").Value) = 0 Then
                str &= " set identity_insert " & TableNameDetailsSub2 & " off "
                InsertIdentity = True
            Else
                str &= " set identity_insert " & TableNameDetailsSub2 & " on "
            End If

            str &= " Insert " & TableNameDetailsSub2 & "(" & MainId & "," & SubId & ",StoreId,InvoiceNo,ItemId,ItemName,Qty,Qty2,Qty3,ReturnQty,NetQty,QtyRecieved,Price,Value,AvgCost,SerialNo"
            If Not InsertIdentity Then str &= ",Line"
            str &= ") values "
            Try
                str &= "('" & OrderTypeId.Text & "','" & Val(txtID.Text) & "','" & Val(StoreId.Text) & "','" & G2.Rows(i).Cells(GC2.InvoiceNo).Value.ToString & "','" & G2.Rows(i).Cells(GC2.ItemId).Value.ToString & "','" & G2.Rows(i).Cells(GC2.ItemName).Value.ToString & "','" & G2.Rows(i).Cells(GC2.Qty).Value.ToString & "','" & G2.Rows(i).Cells(GC2.Qty2).Value.ToString & "','" & G2.Rows(i).Cells(GC2.Qty3).Value.ToString & "','" & G2.Rows(i).Cells(GC2.ReturnQty).Value.ToString & "','" & G2.Rows(i).Cells(GC2.NetQty).Value.ToString & "','" & Val(G2.Rows(i).Cells(GC2.QtyRecieved).Value) & "','" & G2.Rows(i).Cells(GC2.Price).Value.ToString & "','" & G2.Rows(i).Cells(GC2.Value).Value.ToString & "','" & G2.Rows(i).Cells(GC2.Value).Value.ToString & "','" & Val(G2.Rows(i).Cells(GC2.SerialNo).Value) & "'"
                If Not InsertIdentity Then str &= "," & Val(G2.Rows(i).Cells(GC2.Line).Value)
                str &= ")"
            Catch ex As Exception
            End Try
        Next
        bm.ExecuteNonQuery(str)

        'For i As Integer = 0 To G1.Rows.Count - 1
        '    Try
        '        str = " update SalesDetails set SerialNo='" & G1.Rows(i).Cells(GC1.SerialNo).Value.ToString & "',Qty3='" & G1.Rows(i).Cells(GC1.Qty3).Value.ToString & "',Qty2='" & G1.Rows(i).Cells(GC1.Qty2).Value.ToString & "' where StoreId='" & StoreId.Text & "' and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo='" & G1.Rows(i).Cells(GC1.InvoiceNo).Value.ToString & "' and ItemId='" & G1.Rows(i).Cells(GC1.ItemId).Value.ToString & "' "
        '    Catch ex As Exception
        '    End Try
        'Next
        'bm.ExecuteNonQuery(str)


        If sender Is btnCalc Then
            dt = bm.ExecuteAdapter("CalcImportMessageCost", New String() {"OrderTypeId", "ImportMessageId"}, New String() {Val(OrderTypeId.Text), Val(txtID.Text)})
        ElseIf IsDelivered.IsChecked AndAlso (sender Is btnSave OrElse sender Is btnCalc2) Then
            dt = bm.ExecuteAdapter("CalcImportMessageCost2", New String() {"OrderTypeId", "ImportMessageId"}, New String() {Val(OrderTypeId.Text), Val(txtID.Text)})
        End If


        bm.ExecuteNonQuery("update T set AvgCost=T.Value*TT.Perc from ImportMessagesDetailsSub2 T left join ImportMessagesDetails TT on(TT.StoreId=T.StoreId and TT.InvoiceNo=T.InvoiceNo) where T.OrderTypeId=" & Val(OrderTypeId.Text) & " and T.Id=" & Val(txtID.Text))

        CalcImportMessageCostSubData()

        If Not DontClear Then btnNew_Click(sender, e)
        'bm.ShowMSG("Saved Successfuly")

    End Sub
    Dim DontClear As Boolean = False
    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub


    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        txtID.Clear()
        ClearControls()
        StoreId.Focus()
        CalcImportMessageCostSubData
    End Sub

    Sub ClearControls()
        lop = True
        Try
            G.Rows.Clear()
            G1.Rows.Clear()
            G2.Rows.Clear()
            bm.ClearControls(False)

            bm.LostFocus(OrderTypeId, MainAccNo, "select AccNo from OrderTypes where Id=" & OrderTypeId.Text.Trim(), True)
            '''''''OrderTypeId_LostFocus(Nothing, Nothing)
            MainAccNo_LostFocus(Nothing, Nothing)
            StoreId_LostFocus(StoreId, Nothing)
            '''''txtID.Text = bm.ExecuteScalar("select dbo.GetSalesNewSerial(" & StoreId.Text & ",1)")
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & OrderTypeId.Text & "'")
            If txtID.Text = "" Then txtID.Text = "1"

            EntryUser.SelectedValue = Md.UserName

            DayDate.SelectedDate = bm.MyGetDate.Date
            DayDate.Focus()
        Catch
        End Try
        lop = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            dt = bm.ExecuteAdapter("select 'يومية:'+dbo.GetBankCash_G2TypeName(Flag,BankCash_G2TypeId)+';مسلسل:'+cast(InvoiceNo as nvarchar(100))+';المبلغ:'+cast(MainValue as nvarchar(100)) from BankCash_G2 where PurchaseAccNo='" & OrderTypeId.Text & "' and ImportMessageId='" & txtID.Text.Trim & "' union select 'تسوية مسلسل:'+cast(InvoiceNo as nvarchar(100))+';المبلغ:'+cast(Value as nvarchar(100)) from Entry2 where PurchaseAccNo='" & OrderTypeId.Text & "' and ImportMessageId='" & txtID.Text.Trim & "' union select 'قيد مسلسل:'+cast(InvoiceNo as nvarchar(100))+';المبلغ:'+cast(Debit+Credit as nvarchar(100)) from EntryDt where PurchaseAccNo='" & OrderTypeId.Text & "' and ImportMessageId='" & txtID.Text.Trim & "'")
            If dt.Rows.Count > 0 Then
                Dim Str As String = ""
                For i As Integer = 0 To dt.Rows.Count - 1
                    Str &= vbCrLf & dt.Rows(i)(0)
                Next
                bm.ShowMSG("غير مسموح بمسح رسائل عليها مصاريف" & Str)
                Exit Sub
            End If
            bm.ExecuteNonQuery("delete from " & TableName & " where " & MainId & "=" & OrderTypeId.Text & " and " & SubId & "='" & txtID.Text.Trim & "'     delete " & TableNameDetails & " where " & MainId & "=" & OrderTypeId.Text & " and " & SubId & "='" & txtID.Text.Trim & "'     delete " & TableNameDetailsSub & " where " & MainId & "=" & OrderTypeId.Text & " and " & SubId & "='" & txtID.Text.Trim & "'     delete " & TableNameDetailsSub2 & " where " & MainId & "=" & OrderTypeId.Text & " and " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {OrderTypeId.Text, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_KeyUp(sender As Object, e As KeyEventArgs) Handles txtID.KeyUp
        Try
            If bm.ShowHelpMultiColumns("رقم الرسالة", txtID, txtID, e, "select cast(T.Id as varchar(100)) المسلسل,dbo.GetShipperName(ShipperId) 'الشاحن',CertificateNo 'رقم الشهادة',TT.InvoiceNo 'رقم العقد',SP.Name 'المورد' from ImportMessages T left join ImportMessagesDetails TT on(T.OrderTypeId=TT.OrderTypeId and T.Id=TT.Id) left join Suppliers Sp on (SP.Id=TT.SupplierId) where T.OrderTypeId=" & OrderTypeId.Text) Then
                DayDate.Focus()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, SubId}, New String() {OrderTypeId.Text, txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ContainerPrice.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub


    Private Sub LoadResource()
        btnCalc.SetResourceReference(ContentProperty, "Calc Cost")
        btnCalc2.SetResourceReference(ContentProperty, "Calc Messages Costs")
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        'LblId.SetResourceReference(ContentProperty, "Id")
        lblShipperId.SetResourceReference(ContentProperty, "Shipper")
        lblShippingLineId.SetResourceReference(ContentProperty, "ShippingLine")
        lblShippingCompanyId.SetResourceReference(ContentProperty, "ShippingCompany")
        lblContainerSizeId.SetResourceReference(ContentProperty, "ContainerSize")
        lblContainerPrice.SetResourceReference(ContentProperty, "ContainerPrice")

    End Sub

    Dim lop As Boolean = False
    Private Sub FillControls()

        lop = True
        bm.FillControls(Me)
        '''''''''''''OrderTypeId_LostFocus(Nothing, Nothing)
        MainAccNo_LostFocus(Nothing, Nothing)
        StoreId_LostFocus(StoreId, Nothing)
        Dim dt As DataTable = bm.ExecuteAdapter("select T.*,SM.TotalAfterDiscount,dbo.GetCurrencyName(SM.CurrencyId)CurrencyName from " & TableNameDetails & " T left join SalesMaster SM on(SM.Flag=19 and SM.StoreId=" & Val(StoreId.Text) & " and SM.InvoiceNo=T.InvoiceNo) where T.OrderTypeId='" & OrderTypeId.Text & "' and T.Id='" & Val(txtID.Text) & "'")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G.Rows(i).Cells(GC.SupplierId).Value = dt.Rows(i)("SupplierId").ToString
            G.Rows(i).Cells(GC.SupplierName).Value = dt.Rows(i)("SupplierName").ToString
            G.Rows(i).Cells(GC.DocNo).Value = dt.Rows(i)("DocNo").ToString
            G.Rows(i).Cells(GC.DeliveryDate).Value = IIf(DateTime.Parse(dt.Rows(i)("DeliveryDate")) = DateTime.Parse("01/01/1900"), "", dt.Rows(i)("DeliveryDate").ToString.Substring(0, 10))
            G.Rows(i).Cells(GC.TotalAfterDiscount).Value = dt.Rows(i)("TotalAfterDiscount").ToString
            G.Rows(i).Cells(GC.CurrencyName).Value = dt.Rows(i)("CurrencyName").ToString
            G.Rows(i).Cells(GC.Perc).Value = dt.Rows(i)("Perc").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            GridCalcRow(Nothing, New Forms.DataGridViewCellEventArgs(G.Rows(i).Cells(GC.InvoiceNo).ColumnIndex, i))
        Next
        G.RefreshEdit()


        dt = bm.ExecuteAdapter("select *,dbo.GetItemName(ItemId)MyItemName,dbo.GetItemUnit(ItemId)MyItemUnit from " & TableNameDetailsSub2 & " where OrderTypeId='" & OrderTypeId.Text & "' and Id='" & Val(txtID.Text) & "'")
        G2.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G2.Rows.Add()
            G2.Rows(i).Cells(GC2.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G2.Rows(i).Cells(GC2.ItemId).Value = dt.Rows(i)("ItemId").ToString
            G2.Rows(i).Cells(GC2.ItemName).Value = dt.Rows(i)("MyItemName").ToString
            G2.Rows(i).Cells(GC2.ItemUnit).Value = dt.Rows(i)("MyItemUnit").ToString
            G2.Rows(i).Cells(GC2.Qty).Value = dt.Rows(i)("Qty").ToString
            G2.Rows(i).Cells(GC2.Qty2).Value = dt.Rows(i)("Qty2").ToString
            G2.Rows(i).Cells(GC2.Qty3).Value = dt.Rows(i)("Qty3").ToString
            G2.Rows(i).Cells(GC2.ReturnQty).Value = dt.Rows(i)("ReturnQty").ToString
            G2.Rows(i).Cells(GC2.NetQty).Value = dt.Rows(i)("NetQty").ToString
            G2.Rows(i).Cells(GC2.QtyRecieved).Value = dt.Rows(i)("QtyRecieved").ToString
            G2.Rows(i).Cells(GC2.Price).Value = dt.Rows(i)("Price").ToString
            G2.Rows(i).Cells(GC2.Value).Value = dt.Rows(i)("Value").ToString
            G2.Rows(i).Cells(GC2.AvgCost).Value = dt.Rows(i)("AvgCost").ToString
            G2.Rows(i).Cells(GC2.SerialNo).Value = dt.Rows(i)("SerialNo").ToString
            G2.Rows(i).Cells(GC2.Line).Value = dt.Rows(i)("Line").ToString
        Next
        G2.RefreshEdit()

        CalcAllQty()
        CalcImportMessageCostSubData()

        lop = False
    End Sub

    Private Sub MainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo.LostFocus
        bm.AccNoLostFocus(MainAccNo, MainAccName, , , , , True)

    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles MainAccNo.KeyUp
        bm.AccNoShowHelp(MainAccNo, MainAccName, e, , , , , True)
    End Sub

    Private Sub IsDelivered_Checked(sender As Object, e As RoutedEventArgs) Handles IsDelivered.Checked
        lblToDate.Visibility = Visibility.Visible
        DeliveredDate.Visibility = Visibility.Visible
        btnCalc.Visibility = Visibility.Visible
        'btnCalc2.Visibility = Visibility.Visible
        lblPerc.Visibility = Visibility.Visible
        Perc.Visibility = Visibility.Visible
        'btnCalc3.Visibility = Visibility.Visible
    End Sub

    Private Sub IsDelivered_UnChecked(sender As Object, e As RoutedEventArgs) Handles IsDelivered.Unchecked
        lblToDate.Visibility = Visibility.Hidden
        DeliveredDate.Visibility = Visibility.Hidden
        btnCalc.Visibility = Visibility.Hidden
        'btnCalc2.Visibility = Visibility.Hidden
        lblPerc.Visibility = Visibility.Hidden
        Perc.Visibility = Visibility.Hidden
        'btnCalc3.Visibility = Visibility.Hidden
    End Sub

    Private Sub GridCalcRow(sender As Object, e As Forms.DataGridViewCellEventArgs)
        If G.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Nothing Then
            G.Rows(e.RowIndex).Cells(GC.SupplierId).Value = Nothing
            G.Rows(e.RowIndex).Cells(GC.SupplierName).Value = Nothing
            G.Rows(e.RowIndex).Cells(GC.DocNo).Value = Nothing
            G.Rows(e.RowIndex).Cells(GC.DeliveryDate).Value = Nothing
            G.Rows(e.RowIndex).Cells(GC.TotalAfterDiscount).Value = Nothing
            G.Rows(e.RowIndex).Cells(GC.CurrencyName).Value = Nothing
            G.Rows(e.RowIndex).Cells(GC.Perc).Value = Nothing
            G.Rows(e.RowIndex).Cells(GC.Line).Value = Nothing
            'Return
        End If
        If G.Columns(e.ColumnIndex).Name = GC.InvoiceNo Then
            If Not G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value = Nothing AndAlso Not bm.IF_Exists("select InvoiceNo from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and OrderTypeId=" & OrderTypeId.Text & " and InvoiceNo=" & G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value) Then
                bm.ShowMSG("هذا الرقم غير صحيح")
                G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.SupplierId).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.SupplierName).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.DocNo).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.DeliveryDate).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.TotalAfterDiscount).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.CurrencyName).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.Perc).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.Line).Value = Nothing
                Exit Sub
            End If


            If Not G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value = Nothing Then
                Dim myid As Integer = Val(bm.ExecuteScalar("select Id from ImportMessagesDetails where OrderTypeId=" & Val(OrderTypeId.Text) & " and Id<>" & Val(txtID.Text) & " and InvoiceNo=" & G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value))
                If myid > 0 Then
                    bm.ShowMSG("تم تكرار هذه الفاتورة برسالة مسلسل " & myid)
                    G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.SupplierId).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.SupplierName).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.DocNo).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.DeliveryDate).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.TotalAfterDiscount).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.CurrencyName).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.Perc).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.Line).Value = Nothing
                    Exit Sub
                End If
            End If

            For i As Integer = 0 To G.Rows.Count - 1
                If (G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value) = 0 OrElse i = e.RowIndex Then Continue For
                If G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value = G.Rows(i).Cells(GC.InvoiceNo).Value Then
                    bm.ShowMSG("تم تكرار هذه الفاتورة")
                    G.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.SupplierId).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.SupplierName).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.DocNo).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.DeliveryDate).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.TotalAfterDiscount).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.CurrencyName).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.Perc).Value = Nothing
                    G.Rows(e.RowIndex).Cells(GC.Line).Value = Nothing
                    Exit For
                End If
            Next

            Dim dt As DataTable = bm.ExecuteAdapter("select AccNo,Id from ImportMessagesDetails where not(AccNo='" & MainAccNo.Text & "' and Id='" & txtID.Text & "') and StoreId='" & StoreId.Text & "' and InvoiceNo='" & G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value & "'")
            If dt.Rows.Count > 0 Then
                bm.ShowMSG("تم تسجيل هذه الفاتورة فى الحساب رقم " & dt.Rows(0)(0).ToString & " مسلسل رقم " & dt.Rows(0)(1).ToString)
                G.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.SupplierId).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.SupplierName).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.DocNo).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.DeliveryDate).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.TotalAfterDiscount).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.CurrencyName).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.Perc).Value = Nothing
                G.Rows(e.RowIndex).Cells(GC.Line).Value = Nothing
                Exit Sub
            End If



            dt = bm.ExecuteAdapter("select ToId,dbo.GetSupplierName(ToId)ToName,DocNo,dbo.ToStrDate(DeliveryDate)DeliveryDate,TotalAfterDiscount,dbo.GetCurrencyName(CurrencyId)CurrencyName,Perc from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value)
            If dt.Rows.Count > 0 Then
                G.Rows(e.RowIndex).Cells(GC.SupplierId).Value = dt.Rows(0)("ToId").ToString
                G.Rows(e.RowIndex).Cells(GC.SupplierName).Value = dt.Rows(0)("ToName").ToString
                G.Rows(e.RowIndex).Cells(GC.DocNo).Value = dt.Rows(0)("DocNo").ToString
                G.Rows(e.RowIndex).Cells(GC.DeliveryDate).Value = IIf(DateTime.Parse(dt.Rows(0)("DeliveryDate").ToString) = DateTime.Parse("01/01/1900"), "", dt.Rows(0)("DeliveryDate").ToString.Substring(0, 10))
                G.Rows(e.RowIndex).Cells(GC.TotalAfterDiscount).Value = dt.Rows(0)("TotalAfterDiscount").ToString
                G.Rows(e.RowIndex).Cells(GC.CurrencyName).Value = dt.Rows(0)("CurrencyName").ToString
                G.Rows(e.RowIndex).Cells(GC.Perc).Value = dt.Rows(0)("Perc").ToString
            End If

            For i As Integer = G1.Rows.Count - 1 To 0 Step -1
                If Val(G1.Rows(i).Cells(GC1.InvoiceNo).Value) = 0 Then Continue For
                For x As Integer = 0 To G.Rows.Count - 1
                    If G1.Rows(i).Cells(GC1.InvoiceNo).Value <> G.Rows(x).Cells(GC.InvoiceNo).Value Then Continue For
                    GoTo a
                Next
                G1.Rows.RemoveAt(i)
a:
            Next

            For i As Integer = 0 To G1.Rows.Count - 1
                If Val(G1.Rows(i).Cells(GC1.InvoiceNo).Value) > 0 AndAlso G1.Rows(i).Cells(GC1.InvoiceNo).Value = G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value Then
                    Return
                End If
            Next

            dt = bm.ExecuteAdapter("select InvoiceNo,ItemId,sum(Qty)Qty,max(Qty2)Qty2,sum(Qty3)Qty3,Price-ItemDiscount Price,sum(Value)Value,max(SerialNo)SerialNo,dbo.GetReturnQty(StoreId,Flag,InvoiceNo,ItemId)ReturnQty,sum(Qty)-dbo.GetReturnQty(StoreId,Flag,InvoiceNo,ItemId)NetQty,dbo.GetItemName(ItemId)MyItemName,dbo.GetItemUnit(ItemId)MyItemUnit from SalesDetails where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & G.Rows(e.RowIndex).Cells(GC.InvoiceNo).Value & " group by StoreId,Flag,InvoiceNo,ItemId,Price-ItemDiscount")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As Integer = G1.Rows.Add()
                G1.Rows(x).Cells(GC1.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
                G1.Rows(x).Cells(GC1.ItemId).Value = dt.Rows(i)("ItemId").ToString
                G1.Rows(x).Cells(GC1.ItemName).Value = dt.Rows(i)("MyItemName").ToString
                G1.Rows(x).Cells(GC1.ItemUnit).Value = dt.Rows(i)("MyItemUnit").ToString
                G1.Rows(x).Cells(GC1.Qty).Value = dt.Rows(i)("Qty").ToString
                G1.Rows(x).Cells(GC1.Qty2).Value = dt.Rows(i)("Qty2").ToString
                G1.Rows(x).Cells(GC1.Qty3).Value = dt.Rows(i)("Qty3").ToString
                G1.Rows(x).Cells(GC1.ReturnQty).Value = dt.Rows(i)("ReturnQty").ToString
                G1.Rows(x).Cells(GC1.NetQty).Value = dt.Rows(i)("NetQty").ToString
                G1.Rows(x).Cells(GC1.Price).Value = dt.Rows(i)("Price").ToString
                G1.Rows(x).Cells(GC1.Value).Value = dt.Rows(i)("Value").ToString
                G1.Rows(x).Cells(GC1.SerialNo).Value = dt.Rows(i)("SerialNo").ToString
            Next

        End If
    End Sub

    Private Sub GridKeyDown(sender As Object, e As Forms.KeyEventArgs)
        If e.Handled Then Return
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.InvoiceNo).Index Then
                bm.ShowHelpGrid("الفواتير", G.CurrentRow.Cells(GC.InvoiceNo), G.CurrentRow.Cells(GC.InvoiceNo), e, "select cast(InvoiceNo as varchar(100)) Id,dbo.GetSupplierName(ToId) Name from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and OrderTypeId=" & OrderTypeId.Text, , "الفاتورة", "المورد")
                G.CurrentCell = G.CurrentRow.Cells(GC.SupplierId)
                G.CurrentCell = G.CurrentRow.Cells(GC.InvoiceNo)
                GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.CurrentCell.ColumnIndex, G.CurrentCell.RowIndex))
                GridSelectionChanged(sender, New Forms.DataGridViewCellEventArgs(G.CurrentCell.ColumnIndex, G.CurrentCell.RowIndex))
            End If


        Catch ex As Exception
        End Try
    End Sub

    Private Sub GridSelectionChanged(sender As Object, e As EventArgs)
        Return
        Dim dt As DataTable = bm.ExecuteAdapter("select *,dbo.GetItemName(ItemId)MyItemName from " & TableNameDetailsSub & " where OrderTypeId='" & OrderTypeId.Text & "' and Id='" & Val(txtID.Text) & "' and InvoiceNo='" & G.CurrentRow.Cells(GC.InvoiceNo).Value & "'")
        G1.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G1.Rows.Add()
            G1.Rows(i).Cells(GC1.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G1.Rows(i).Cells(GC1.ItemId).Value = dt.Rows(i)("ItemId").ToString
            G1.Rows(i).Cells(GC1.ItemName).Value = dt.Rows(i)("MyItemName").ToString
            G1.Rows(i).Cells(GC1.Qty).Value = dt.Rows(i)("Qty").ToString
            G1.Rows(i).Cells(GC1.Qty2).Value = dt.Rows(i)("Qty2").ToString
            G1.Rows(i).Cells(GC1.Qty3).Value = dt.Rows(i)("Qty3").ToString
            G1.Rows(i).Cells(GC1.ReturnQty).Value = dt.Rows(i)("ReturnQty").ToString
            G1.Rows(i).Cells(GC1.NetQty).Value = dt.Rows(i)("NetQty").ToString
            G1.Rows(i).Cells(GC1.QtyRecieved).Value = dt.Rows(i)("QtyRecieved").ToString
            G1.Rows(i).Cells(GC1.Price).Value = dt.Rows(i)("Price").ToString
            G1.Rows(i).Cells(GC1.Value).Value = dt.Rows(i)("Value").ToString
            G1.Rows(i).Cells(GC1.SerialNo).Value = dt.Rows(i)("SerialNo").ToString
        Next
        G1.RefreshEdit()
        If dt.Rows.Count > 0 Then Return

        dt = bm.ExecuteAdapter("select *,dbo.GetReturnQty(StoreId,Flag,InvoiceNo,ItemId)ReturnQty,Qty-dbo.GetReturnQty(StoreId,Flag,InvoiceNo,ItemId)NetQty,dbo.GetItemName(ItemId)MyItemName from SalesDetails where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & G.CurrentRow.Cells(GC.InvoiceNo).Value)
        G1.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G1.Rows.Add()
            G1.Rows(i).Cells(GC1.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G1.Rows(i).Cells(GC1.ItemId).Value = dt.Rows(i)("ItemId").ToString
            G1.Rows(i).Cells(GC1.ItemName).Value = dt.Rows(i)("MyItemName").ToString
            G1.Rows(i).Cells(GC1.Qty).Value = dt.Rows(i)("Qty").ToString
            G1.Rows(i).Cells(GC1.Qty2).Value = dt.Rows(i)("Qty2").ToString
            G1.Rows(i).Cells(GC1.Qty3).Value = dt.Rows(i)("Qty3").ToString
            G1.Rows(i).Cells(GC1.ReturnQty).Value = dt.Rows(i)("ReturnQty").ToString
            G1.Rows(i).Cells(GC1.NetQty).Value = dt.Rows(i)("NetQty").ToString
            G1.Rows(i).Cells(GC1.Price).Value = dt.Rows(i)("Price").ToString
            G1.Rows(i).Cells(GC1.Value).Value = dt.Rows(i)("Value").ToString
            G1.Rows(i).Cells(GC1.SerialNo).Value = dt.Rows(i)("SerialNo").ToString

        Next
    End Sub

    Private Sub OrderTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles OrderTypeId.LostFocus
        bm.LostFocus(OrderTypeId, OrderTypeName, "select Name from OrderTypes where Id=" & OrderTypeId.Text.Trim(), True)
        btnNew_Click(Nothing, Nothing)
    End Sub

    Private Sub OrderTypeId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles OrderTypeId.KeyUp
        If bm.ShowHelp("OrderTypes", OrderTypeId, OrderTypeName, e, "select cast(Id as varchar(100)) Id,Name from OrderTypes") Then
            OrderTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub btnShipperId_Click(sender As Object, e As RoutedEventArgs) Handles btnShipperId.Click
        bm.ShowHelp("Shippers", ShipperId, "Shippers")
    End Sub

    Private Sub btnShippingLineId_Click(sender As Object, e As RoutedEventArgs) Handles btnShippingLineId.Click
        bm.ShowHelp("ShippingLines", ShippingLineId, "ShippingLines")
    End Sub

    Private Sub btnShippingCompanyId_Click(sender As Object, e As RoutedEventArgs) Handles btnShippingCompanyId.Click
        bm.ShowHelp("ShippingCompanies", ShippingCompanyId, "ShippingCompanies")
    End Sub

    Private Sub btnContainerSizeId_Click(sender As Object, e As RoutedEventArgs) Handles btnContainerSizeId.Click
        bm.ShowHelp("ContainerSizes", ContainerSizeId, "ContainerSizes")
    End Sub

    Private Sub btnCalc_Click(sender As Object, e As RoutedEventArgs) Handles btnCalc.Click, btnCalc2.Click
        If Val(Cost_Administrative0.Text) > 0 Then
            bm.ShowMSG("برجاء توزيع المصاريف الإدارية")
            Return
        End If
        If Val(Cost_OrderTypeId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد مصاريف الطلبية")
            Cost_OrderTypeId.Focus()
            Return
        End If
        If PolisaData.SelectedDate Is Nothing Then
            bm.ShowMSG("برجاء تسجيل " & lblPolisaData.Content)
            PolisaData.Focus()
            Return
        End If
        If DeliveredDate.SelectedDate Is Nothing Then
            DeliveredDate.SelectedDate = bm.MyGetDate.Date
        End If
        DontClear = True
        btnSave_Click(sender, Nothing)
        DontClear = False
        txtID_Leave(Nothing, Nothing)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click, btnPrintImages.Click
        DontClear = True
        btnSave_Click(sender, Nothing)
        DontClear = False
        Dim rpt As New ReportViewer
        rpt.Rpt = IIf(sender Is btnPrintImages, "ImportMessagesImages.rpt", "ImportMessages.rpt")
        rpt.paraname = New String() {"@OrderTypeId", "@Id", "Header"}
        rpt.paravalue = New String() {Val(OrderTypeId.Text), Val(txtID.Text), CType(Parent, Page).Title}
        rpt.Show()
    End Sub

    Private Sub btnPrint2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint2.Click, btnPrint2Images.Click
        DontClear = True
        btnSave_Click(sender, Nothing)
        DontClear = False
        Dim rpt As New ReportViewer
        rpt.Rpt = IIf(sender Is btnPrint2Images, "ImportMessages2Images.rpt", "ImportMessages2.rpt")
        rpt.paraname = New String() {"@OrderTypeId", "@Id", "Header"}
        rpt.paravalue = New String() {Val(OrderTypeId.Text), Val(txtID.Text), sender.Content.ToString.Replace(" بالصور", "")}
        rpt.Show()
    End Sub

    Private Sub btnPrint3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint3.Click, btnPrint3Images.Click
        DontClear = True
        btnSave_Click(sender, Nothing)
        DontClear = False
        Dim rpt As New ReportViewer
        rpt.Rpt = IIf(sender Is btnPrint3Images, "ImportMessages3Images.rpt", "ImportMessages3.rpt")
        rpt.paraname = New String() {"@OrderTypeId", "@Id", "Header"}
        rpt.paravalue = New String() {Val(OrderTypeId.Text), Val(txtID.Text), sender.Content.ToString.Replace(" بالصور", "")}
        rpt.Show()
    End Sub

    Private Sub G1_CellDoubleClick(sender As Object, e As Forms.DataGridViewCellEventArgs)
        G2.EndEdit()
        If e.RowIndex < 0 Then Return
        Dim count As Integer = Val(G1.Rows(e.RowIndex).Cells(GC1.NetQty).Value)
        For i As Integer = 0 To G2.Rows.Count - 1
            If G2.Rows(i).Cells(GC2.InvoiceNo).Value = G1.Rows(e.RowIndex).Cells(GC1.InvoiceNo).Value AndAlso G2.Rows(i).Cells(GC2.ItemId).Value = G1.Rows(e.RowIndex).Cells(GC1.ItemId).Value AndAlso G2.Rows(i).Cells(GC2.Price).Value = G1.Rows(e.RowIndex).Cells(GC1.Price).Value Then
                count -= Val(G2.Rows(i).Cells(GC2.Qty).Value)
            End If
        Next
        If count = 0 Then
            bm.ShowMSG("لا يوجد كمية متبقية")
        Else
            Dim i As Integer = G2.Rows.Add
            G2.Rows(i).Cells(GC2.InvoiceNo).Value = G1.Rows(e.RowIndex).Cells(GC1.InvoiceNo).Value
            G2.Rows(i).Cells(GC2.ItemId).Value = G1.Rows(e.RowIndex).Cells(GC1.ItemId).Value
            G2.Rows(i).Cells(GC2.ItemName).Value = G1.Rows(e.RowIndex).Cells(GC1.ItemName).Value
            G2.Rows(i).Cells(GC2.Qty).Value = count
            G2.Rows(i).Cells(GC2.Qty2).Value = G1.Rows(e.RowIndex).Cells(GC1.Qty2).Value
            G2.Rows(i).Cells(GC2.Qty3).Value = G1.Rows(e.RowIndex).Cells(GC1.Qty3).Value
            G2.Rows(i).Cells(GC2.ReturnQty).Value = G1.Rows(e.RowIndex).Cells(GC1.ReturnQty).Value
            G2.Rows(i).Cells(GC2.NetQty).Value = G1.Rows(e.RowIndex).Cells(GC1.NetQty).Value
            G2.Rows(i).Cells(GC2.QtyRecieved).Value = G1.Rows(e.RowIndex).Cells(GC1.QtyRecieved).Value
            G2.Rows(i).Cells(GC2.Price).Value = G1.Rows(e.RowIndex).Cells(GC1.Price).Value
            G2.Rows(i).Cells(GC2.Value).Value = G1.Rows(e.RowIndex).Cells(GC1.Value).Value
            If G2.Rows(i).Cells(GC2.AvgCost).Value Is Nothing Then
                G2.Rows(i).Cells(GC2.AvgCost).Value = 0
            End If

            'G2.Rows(i).Cells(GC2.SerialNo).Value = G1.Rows(e.RowIndex).Cells(GC1.SerialNo).Value
            If i = 0 Then
                G2.Rows(i).Cells(GC2.SerialNo).Value = Val(bm.ExecuteScalar("select dbo.GetSalesSerialNawar(" & StoreId.Text & ",-19)"))
            Else
                G2.Rows(i).Cells(GC2.SerialNo).Value = G2.Rows(i - 1).Cells(GC2.SerialNo).Value
            End If
            G2_CellEndEdit(sender, New Forms.DataGridViewCellEventArgs(G2.Columns(GC2.Qty).Index, i))
        End If
    End Sub

    Private Sub G1_CellEndEdit(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            If e.ColumnIndex = G1.Columns(GC1.Qty).Index OrElse e.ColumnIndex = G1.Columns(GC1.Qty2).Index Then
                G1.Rows(e.RowIndex).Cells(GC1.Qty3).Value = Math.Round(G1.Rows(e.RowIndex).Cells(GC1.NetQty).Value / G1.Rows(e.RowIndex).Cells(GC1.Qty2).Value, 0, MidpointRounding.AwayFromZero)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub G2_CellEndEdit(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            If e.ColumnIndex = G2.Columns(GC2.Qty).Index OrElse e.ColumnIndex = G2.Columns(GC2.Qty2).Index Then
                G2.Rows(e.RowIndex).Cells(GC2.Qty3).Value = Math.Round(G2.Rows(e.RowIndex).Cells(GC2.Qty).Value / G2.Rows(e.RowIndex).Cells(GC2.Qty2).Value, 2, MidpointRounding.AwayFromZero)
            End If
        Catch ex As Exception
        End Try

        Try
            For i As Integer = 0 To G1.Rows.Count - 1
                If G1.Rows(i).Cells(GC1.InvoiceNo).Value = G2.Rows(e.RowIndex).Cells(GC2.InvoiceNo).Value AndAlso G1.Rows(i).Cells(GC1.ItemId).Value = G2.Rows(e.RowIndex).Cells(GC2.ItemId).Value AndAlso G1.Rows(i).Cells(GC1.Price).Value = G2.Rows(e.RowIndex).Cells(GC2.Price).Value Then

                    Dim count As Integer = Val(G1.Rows(i).Cells(GC1.NetQty).Value)
                    For i2 As Integer = 0 To G2.Rows.Count - 1
                        If G2.Rows(i2).Cells(GC2.InvoiceNo).Value = G1.Rows(i).Cells(GC1.InvoiceNo).Value AndAlso G2.Rows(i2).Cells(GC2.ItemId).Value = G1.Rows(i).Cells(GC1.ItemId).Value AndAlso G2.Rows(i2).Cells(GC2.Price).Value = G1.Rows(i).Cells(GC1.Price).Value Then
                            count -= Val(G2.Rows(i2).Cells(GC2.Qty).Value)
                        End If
                    Next
                    G1.Rows(i).Cells(GC1.Remaining).Value = count

                End If
            Next
        Catch ex As Exception
        End Try

        Try
            G2.Rows(e.RowIndex).Cells(GC2.Value).Value = Val(G2.Rows(e.RowIndex).Cells(GC2.Qty).Value) * Val(G2.Rows(e.RowIndex).Cells(GC2.Price).Value)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CalcAllQty()
        Try
            For i As Integer = 0 To G1.Rows.Count - 1
                Dim count As Integer = Val(G1.Rows(i).Cells(GC1.NetQty).Value)
                For i2 As Integer = 0 To G2.Rows.Count - 1
                    If G2.Rows(i2).Cells(GC2.InvoiceNo).Value = G1.Rows(i).Cells(GC1.InvoiceNo).Value AndAlso G2.Rows(i2).Cells(GC2.ItemId).Value = G1.Rows(i).Cells(GC1.ItemId).Value AndAlso Math.Round(Val(G2.Rows(i2).Cells(GC2.Price).Value), 2, MidpointRounding.AwayFromZero) = Math.Round(Val(G1.Rows(i).Cells(GC1.Price).Value), 2, MidpointRounding.AwayFromZero) Then
                        count -= Val(G2.Rows(i2).Cells(GC2.Qty).Value)
                    End If
                Next
                G1.Rows(i).Cells(GC1.Remaining).Value = count
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Sub G1_RowsRemoved(sender As Object, e As Forms.DataGridViewRowsRemovedEventArgs)
        CalcAllQty()
    End Sub

    Private Sub G2_RowsRemoved(sender As Object, e As Forms.DataGridViewRowsRemovedEventArgs)
        CalcAllQty()
    End Sub

    Private Sub btnPrintBarcode_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintBarcode.Click

        G2.EndEdit()

        Dim rpt As New ReportViewer

        If Md.MyProjectType = ProjectType.X Then
            If bm.ShowDeleteMSG("هل تريد طباعة المقاس الكبير؟") Then
                rpt.Rpt = "PrintBarcodeS2.rpt"
            Else
                rpt.Rpt = "PrintBarcodeS1.rpt"
            End If
        Else
            rpt.Rpt = "PrintBarcode.rpt"
        End If
        rpt.paraname = New String() {"@InvoiceNo", "@ItemId", "@ColorId", "@SizeId", "@Count", "Header"}

        For i As Integer = 0 To G2.Rows.Count - 1
            Try
                If Val(G2.Rows(i).Cells(GC2.ItemId).Value) = 0 Then Continue For

                For x As Integer = 0 To Val(G2.Rows(i).Cells(GC2.Qty).Value) - 1
                    rpt.paravalue = New String() {0, Val(G2.Rows(i).Cells(GC2.ItemId).Value), 0, 0, 1, CType(Parent, Page).Title}
                    rpt.Print(".", Md.BarcodePrinter, 1, False)
                Next

            Catch ex As Exception
            End Try
        Next
        CalcAllQty()
    End Sub


    Private Sub Barcode_KeyDown(sender As Object, e As KeyEventArgs) Handles Barcode.KeyDown
        If e.Key = Key.Tab Then
            Dim IsExists As Boolean = False
            Dim MyId As Integer = Val(bm.ExecuteScalar("select Id from Items where Barcode='" & Barcode.Text & "'"))

            If MyId = 0 Then
                bm.ShowMSG("هذا الصنف غير موجود")
                Exit Sub
            End If

            For i As Integer = 0 To G2.Rows.Count - 1
                If G2.Rows(i).Cells(GC2.ItemId).Value = MyId Then
                    G2.Rows(i).Cells(GC2.Qty).Value += 1
                    G2_CellEndEdit(sender, New Forms.DataGridViewCellEventArgs(G2.Columns(GC2.Qty).Index, i))
                    IsExists = True
                    Exit For
                End If
            Next

            If Not IsExists Then
                For x As Integer = 0 To G1.Rows.Count - 1
                    If MyId = G1.Rows(x).Cells(GC1.ItemId).Value Then


                        Dim i As Integer = G2.Rows.Add
                        G2.Rows(i).Cells(GC2.InvoiceNo).Value = G1.Rows(x).Cells(GC1.InvoiceNo).Value
                        G2.Rows(i).Cells(GC2.ItemId).Value = G1.Rows(x).Cells(GC1.ItemId).Value
                        G2.Rows(i).Cells(GC2.ItemName).Value = G1.Rows(x).Cells(GC1.ItemName).Value
                        G2.Rows(i).Cells(GC2.Qty).Value = 1
                        G2.Rows(i).Cells(GC2.Qty2).Value = G1.Rows(x).Cells(GC1.Qty2).Value
                        G2.Rows(i).Cells(GC2.Qty3).Value = G1.Rows(x).Cells(GC1.Qty3).Value
                        G2.Rows(i).Cells(GC2.ReturnQty).Value = G1.Rows(x).Cells(GC1.ReturnQty).Value
                        G2.Rows(i).Cells(GC2.NetQty).Value = G1.Rows(x).Cells(GC1.NetQty).Value
                        G2.Rows(i).Cells(GC2.QtyRecieved).Value = G1.Rows(x).Cells(GC1.QtyRecieved).Value
                        G2.Rows(i).Cells(GC2.Price).Value = G1.Rows(x).Cells(GC1.Price).Value
                        G2.Rows(i).Cells(GC2.Value).Value = G1.Rows(x).Cells(GC1.Value).Value
                        If G2.Rows(i).Cells(GC2.AvgCost).Value Is Nothing Then
                            G2.Rows(i).Cells(GC2.AvgCost).Value = 0
                        End If

                        If i = 0 Then
                            G2.Rows(i).Cells(GC2.SerialNo).Value = Val(bm.ExecuteScalar("select dbo.GetSalesSerialNawar(" & StoreId.Text & ",-19)"))
                        Else
                            G2.Rows(i).Cells(GC2.SerialNo).Value = G2.Rows(i - 1).Cells(GC2.SerialNo).Value
                        End If
                        G2_CellEndEdit(sender, New Forms.DataGridViewCellEventArgs(G2.Columns(GC2.Qty).Index, i))



                        Exit For
                    End If
                Next
            End If

            Barcode.Clear()
            e.Handled = True
        End If

    End Sub

    Private Sub BtnCalc3_Click(sender As Object, e As RoutedEventArgs) Handles btnCalc3.Click
        'توزيع المصاريف الإدارية على الطلبيات المفتوحة
        If bm.ExecuteNonQuery("CalcImportMessageCostSub", {"OrderTypeId", "ImportMessageId", "DeliveredDate"}, {Val(OrderTypeId.Text), Val(txtID.Text), bm.ToStrDate(DeliveredDate.SelectedDate)}) Then
            CalcImportMessageCostSubData()

            DontClear = True
            btnSave_Click(Nothing, Nothing)
            DontClear = False

            val5.Text = bm.ExecuteScalar("select dbo.GetCalcImportMessageCostVal5('" & Val(OrderTypeId.Text) & "','" & Val(txtID.Text) & "','" & bm.ToStrDate(DeliveredDate.SelectedDate) & "')")

            bm.ShowMSG("تمت العملية بنجاح")
        End If
    End Sub

    Sub CalcImportMessageCostSubData()
        If DeliveredDate.SelectedDate Is Nothing Then
            Cost_Administrative0.Clear()
            Cost_OrderTypeId0.Clear()
            Return
        End If
        Cost_Administrative0.Text = Val(bm.ExecuteScalar("CalcImportMessageCostSubData", {"DeliveredDate"}, {bm.ToStrDate(DeliveredDate.SelectedDate)}))
        bm.ExecuteNonQuery("exec GenerateCurrencyExchangeByDateTemp null,'" & bm.ToStrDate(DeliveredDate.SelectedDate) & "'")
        Cost_OrderTypeId0.Text = Val(bm.ExecuteScalar("select dbo.Bal0(AccNo,Id,'" & bm.ToStrDate(DeliveredDate.SelectedDate) & "',0,0,0)*C.Exchange from OrderTypes T
left join CurrencyExchangeByDateTemp C on(C.CurrencyId=T.CurrencyId and C.DayDate='" & bm.ToStrDate(DeliveredDate.SelectedDate) & "') where Id=" & Val(OrderTypeId.Text)))

    End Sub

    Private Sub DeliveredDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DeliveredDate.SelectedDateChanged
        CalcImportMessageCostSubData()
    End Sub
End Class
