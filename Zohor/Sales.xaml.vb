Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Managementydate
Imports System.ComponentModel
Imports System.IO

Public Class Sales

    Public MainTableName As String = "Stores"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"

    Public TableName As String = "SalesMaster"
    Public TableDetailsName As String = "SalesDetails"

    Public MainId As String = "StoreId"
    Public SubId As String = "InvoiceNo"
    Public MyToId As Integer = 0

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim StaticsDt As New DataTable
    WithEvents G As New MyGrid
    Public Flag As Integer
    Public Receive As Boolean = False
    Public FirstColumn As String = "الكـــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "السعــــر", Statement As String = ""
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Dim WithEvents BackgroundWorker1 As New BackgroundWorker


    Public Structure FlagState
        'Don't forget to edit RPTs and Stored Procedures after Editing this structure
        Shared أرصدة_افتتاحية As Integer = 1
        Shared إضافة As Integer = 2
        Shared تسوية_إضافة As Integer = 3
        Shared صرف As Integer = 4
        Shared تسوية_صرف As Integer = 5
        Shared هدايا As Integer = 6
        Shared هالك As Integer = 7
        Shared تحويل_إلى_مخزن As Integer = 8
        Shared مشتريات As Integer = 9
        Shared مردودات_مشتريات As Integer = 10
        Shared مبيعات_الصالة As Integer = 11
        Shared مردودات_مبيعات_الصالة As Integer = 12
        Shared المبيعات As Integer = 13 'مبيعات_التيك_أواى
        Shared مردودات_المبيعات As Integer = 14 'مردودات_مبيعات_التيك_أواى
        Shared مبيعات_التوصيل As Integer = 15
        Shared مردودات_مبيعات_التوصيل As Integer = 16
        Shared المستهلكات As Integer = 17
        Shared مردودات_المستهلكات As Integer = 18
        Shared الاستيراد As Integer = 19
        Shared مردودات_الاستيراد As Integer = 20
        Shared مبيعات_الجملة As Integer = 21
        Shared مردودات_مبيعات_الجملة As Integer = 22
        Shared مبيعات_نصف_الجملة As Integer = 23
        Shared مردودات_مبيعات_نصف_الجملة As Integer = 24
        Shared عينات As Integer = 25
        Shared عرض_أسعار As Integer = 26
        Shared أمر_شراء As Integer = 27
        Shared عرض_أسعار_مورد As Integer = 28

        Shared مشتريات_خارجية As Integer = 29
        Shared مردودات_مشتريات_خارجية As Integer = 30

        Shared أمر_توريد As Integer = 31

        Shared التصدير As Integer = 33
        Shared مردودات_التصدير As Integer = 34

        Shared مبيعات_لمستثمر As Integer = 35
        Shared مردودات_مبيعات_لمستثمر As Integer = 36

        Shared مستهلكات_الداخلي As Integer = 37
        Shared مردودات_مستهلكات_الداخلي As Integer = 38
        Shared مستهلكات_العمليات As Integer = 47
        Shared مردودات_مستهلكات_العمليات As Integer = 48

    End Structure

    Function MainFlag() As String
        Select Case Flag
            Case FlagState.مردودات_الاستيراد
                Return FlagState.الاستيراد
            Case FlagState.مردودات_التصدير
                Return FlagState.التصدير
            Case FlagState.مردودات_مبيعات_لمستثمر
                Return FlagState.مبيعات_لمستثمر
            Case FlagState.مردودات_المبيعات
                Return FlagState.المبيعات
            Case FlagState.تحويل_إلى_مخزن
                Return FlagState.مردودات_المبيعات
            Case FlagState.مردودات_المستهلكات
                Return FlagState.المستهلكات
            Case FlagState.مردودات_مستهلكات_الداخلي
                Return FlagState.مستهلكات_الداخلي
            Case FlagState.مردودات_مستهلكات_العمليات
                Return FlagState.مستهلكات_العمليات
            Case FlagState.مردودات_مبيعات_التوصيل
                Return FlagState.مبيعات_التوصيل
            Case FlagState.مردودات_مبيعات_الجملة
                Return FlagState.مبيعات_الجملة
            Case FlagState.مردودات_مبيعات_الصالة
                Return FlagState.مبيعات_الصالة
            Case FlagState.مردودات_مبيعات_نصف_الجملة
                Return FlagState.مبيعات_نصف_الجملة
            Case FlagState.مردودات_مشتريات
                Return FlagState.مشتريات
            Case FlagState.مردودات_مشتريات_خارجية
                Return FlagState.مشتريات_خارجية
            Case Else
                Return 0
        End Select
    End Function

    Sub NewId()
        InvoiceNo.Clear()
        'InvoiceNo.IsEnabled = Md.Manager
    End Sub

    Sub UndoNewId()
        'InvoiceNo.IsEnabled = True
    End Sub

    Public Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        LoadResource()

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            btnNew.Content &= " (F8)"
            btnPrint2.Content &= " (F5)"

            btnSave.ToolTip = btnSave.Content
            btnDelete.ToolTip = btnDelete.Content
            btnNew.ToolTip = btnNew.Content
            btnFirst.ToolTip = btnFirst.Content
            btnPrevios.ToolTip = btnPrevios.Content
            btnNext.ToolTip = btnNext.Content
            btnLast.ToolTip = btnLast.Content
            btnDeleteRow.ToolTip = btnDeleteRow.Content
            btnPrint.ToolTip = btnPrint.Content
            btnPrint2.ToolTip = btnPrint2.Content
            btnPrint3.ToolTip = btnPrint3.Content
            btnPrint4.ToolTip = btnPrint4.Content
            btnPrint5.ToolTip = btnPrint5.Content
            btnPrintSafe.ToolTip = btnPrintSafe.Content
            btnPrintImage.ToolTip = btnPrintImage.Content
        End If

        If Not Md.MyProjectType = ProjectType.X Then
            btnPrintSafe.Visibility = Visibility.Hidden
        End If

        TabItem1.Height = 0
        TabItemDelivery.Height = 0
        TabItemTables.Height = 0

        Hide()
        bm.FillCombo("Shifts", Shift, "")
        bm.FillCombo("PaymentMethods", PaymentMethodId, "")
        bm.FillCombo("ShippingMethods", ShippingMethodId, "")
        bm.FillCombo("PriceLists", PriceListId, "")
        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from CostCenters where SubType=1 union all select 0 Id,'-' Name order by Id", CostCenterId)

        Shift.SelectedValue = Md.CurrentShiftId
        DayDate.SelectedDate = Nothing
        DayDate.SelectedDate = bm.MyGetDate() 'Md.CurrentDate
        If Md.ShowShifts Then
            DayDate.SelectedDate = Md.CurrentDate
            Shift.SelectedValue = Md.CurrentShiftId
        Else
            lblShift.Visibility = Visibility.Hidden
            Shift.Visibility = Visibility.Hidden
        End If

        LoadWFH()
        lblSaveId.Visibility = Visibility.Hidden
        SaveId.Visibility = Visibility.Hidden
        SaveName.Visibility = Visibility.Hidden
        SaveId.IsEnabled = Md.Manager


        lblBankId.Visibility = Visibility.Hidden
        BankId.Visibility = Visibility.Hidden
        BankName.Visibility = Visibility.Hidden
        BankId.IsEnabled = Md.Manager

        StaticsDt = bm.ExecuteAdapter("select top 1 S_AccNo,R_S_AccNo,P_AccNo,R_P_AccNo,S_AccNo1,S_Per1,S_AccType1,S_AccNo2,S_Per2,S_AccType2,S_AccNo3,S_Per3,S_AccType3,S_AccNo4,S_Per4,S_AccType4,P_AccNo1,P_Per1,P_AccType1,P_AccNo2,P_Per2,P_AccType2,P_AccNo3,P_Per3,P_AccType3,P_AccNo4,P_Per4,P_AccType4,S_SubAccNo1,S_SubAccNo2,S_SubAccNo3,S_SubAccNo4,S_R_SubAccNo1,S_R_SubAccNo2,S_R_SubAccNo3,S_R_SubAccNo4,P_SubAccNo1,P_SubAccNo2,P_SubAccNo3,P_SubAccNo4,P_R_SubAccNo1,P_R_SubAccNo2,P_R_SubAccNo3,P_R_SubAccNo4 from Statics")

        bm.FillCombo("AccTypes", AccType1, "")
        bm.FillCombo("AccTypes", AccType2, "")
        bm.FillCombo("AccTypes", AccType3, "")
        bm.FillCombo("AccTypes", AccType4, "")

        bm.FillCombo("SalesTypes", SalesTypeId, "", , True, True)

        RdoGrouping_Checked(Nothing, Nothing)

        TabItem1.Header = "" ' TryCast(TryCast(Me.Parent, TabItem).Header, TabsHeader).MyTabHeader

        LoadVisibility()

        LoadCbo()

        bm.Fields = New String() {"Flag", MainId, SubId, "DayDate", "Shift", "ToId", "ReservToId", "WaiterId", "TableId", "TableSubId", "NoOfPersons", "WithTax", "Taxvalue", "WithService", "ServiceValue", "CancelMinPerPerson", "MinPerPerson", "PaymentType", "CashValue", "DiscountPerc", "DiscountValue", "Notes", "IsClosed", "IsCashierPrinted", "Cashier", "DeliverymanId", "Total", "TotalCount", "TotalAfterDiscount", "DocNo", "AccNo1", "AccNo2", "AccNo3", "AccNo4", "AccType1", "AccType2", "AccType3", "AccType4", "Per1", "Per2", "Per3", "Per4", "Val1", "Val2", "Val3", "Val4", "SaveId", "BankId", "Temp", "Temp2", "OrderTypeId", "AccNo", "CurrencyId", "Shipping", "Freight", "CustomClearance", "PaymentMethodId", "ShippingMethodId", "ContractTerms", "DeliveryDate", "VersionNo", "CaseInvoiceNo", "PurchaseOrder", "SalesTypeId", "PriceListId", "SubAccNo1", "SubAccNo2", "SubAccNo3", "SubAccNo4", "IsPosted", "SphR", "SphL", "CylR", "CylL", "AxisR", "AxisL", "DoctorId", "CostCenterId", "CurrentShift"}
        bm.control = New Control() {txtFlag, StoreId, InvoiceNo, DayDate, Shift, ToId, ReservToId, WaiterId, TableId, TableSubId, NoOfPersons, WithTax, Taxvalue, WithService, ServiceValue, CancelMinPerPerson, MinPerPerson, PaymentType, CashValue, DiscountPerc, DiscountValue, Notes, IsClosed, IsCashierPrinted, CashierId, DeliverymanId, Total, TotalCount, TotalAfterDiscount, DocNo, AccNo1, AccNo2, AccNo3, AccNo4, AccType1, AccType2, AccType3, AccType4, Per1, Per2, Per3, Per4, Val1, Val2, Val3, Val4, SaveId, BankId, Temp, Temp2, OrderTypeId, AccNo, CurrencyId, Shipping, Freight, CustomClearance, PaymentMethodId, ShippingMethodId, ContractTerms, DeliveryDate, VersionNo, CaseInvoiceNo, PurchaseOrder, SalesTypeId, PriceListId, SubAccNo1, SubAccNo2, SubAccNo3, SubAccNo4, IsPosted, SphR, SphL, CylR, CylL, AxisR, AxisL, DoctorId, CostCenterId, CurrentShift}
        bm.KeyFields = New String() {"Flag", MainId, SubId}

        bm.Table_Name = TableName

        LoadGroups()
        LoadAllItems()
        RdoCash_Checked(Nothing, Nothing)
        txtFlag.Text = Flag
        If Receive Then
            ToId.Text = Md.DefaultStore
            ToId_LostFocus(Nothing, Nothing)
        Else
            StoreId.Text = Md.DefaultStore
            looop = True
            StoreId_LostFocus(Nothing, Nothing)
            looop = False
        End If

        ComboBox1.SelectedValue = Flag

        If Not (Md.MyProjectType = ProjectType.X AndAlso Flag = FlagState.مشتريات) Then
            PurchaseOrder.Visibility = Visibility.Hidden
            lblPurchaseOrder.Visibility = Visibility.Hidden
            btnPurchaseOrder.Visibility = Visibility.Hidden
        End If
        If Md.MyProjectType = ProjectType.X AndAlso Flag = FlagState.المبيعات Then
            btnPrint.Content = "طباعة كشف وإذن صرف"
            btnPrint.Margin = New Thickness(btnPrint.Margin.Left - btnPrint.Width * 0.8, btnPrint.Margin.Top, btnPrint.Margin.Right, btnPrint.Margin.Bottom)
            btnPrint.Width *= 1.8
        End If
        If Md.MyProjectType = ProjectType.X Then
            CurrencyId.IsEnabled = True
        End If
        If Md.MyProjectType <> ProjectType.X OrElse (Flag <> FlagState.المبيعات AndAlso Flag <> FlagState.مبيعات_لمستثمر) Then
            btnInstallment.Visibility = Visibility.Hidden
        End If

        If Md.MyProjectType <> ProjectType.Zohor OrElse Flag <> FlagState.تحويل_إلى_مخزن Then
            btnReplacement.Visibility = Visibility.Hidden
        End If

        RdoFuture.IsChecked = True
        RdoCash.IsChecked = True
        btnNew_Click(Nothing, Nothing)
        If MyToId > 0 Then
            ToId.Text = MyToId
            ToId_LostFocus(Nothing, Nothing)
        End If

        If Md.MyProjectType = ProjectType.X Then
            CalcBalSub()
        End If
    End Sub


    Structure GC
        Shared SalesInvoiceNo As String = "SalesInvoiceNo"
        Shared Barcode As String = "Barcode"
        Shared ItemSerialNo As String = "ItemSerialNo"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared GroupName As String = "GroupName"
        Shared TypeName As String = "TypeName"
        Shared ExpDate As String = "ExpDate"
        Shared ExpireDate As String = "ExpireDate"
        Shared Color As String = "Color"
        Shared Size As String = "Size"
        Shared UnitId As String = "UnitId"
        Shared UnitQty As String = "UnitQty"
        Shared CurrentBal As String = "CurrentBal"
        Shared UnitsWeightId As String = "UnitsWeightId"
        Shared UnitsWeightQty As String = "UnitsWeightQty"
        Shared PreQty As String = "PreQty"
        Shared ConsumptionQty As String = "ConsumptionQty"
        Shared ConsumptionRemainingQty As String = "ConsumptionRemainingQty"
        Shared Qty As String = "Qty"
        Shared Qty2 As String = "Qty2"
        Shared Qty3 As String = "Qty3"
        Shared ReceivedQty As String = "ReceivedQty"
        Shared SalesPriceTypeId As String = "SalesPriceTypeId"
        Shared Price As String = "Price"
        Shared UnitSub As String = "UnitSub"
        Shared QtySub As String = "QtySub"
        Shared PriceSub As String = "PriceSub"
        Shared ItemDiscountPerc As String = "ItemDiscountPerc"
        Shared ItemDiscount As String = "ItemDiscount"
        Shared Value As String = "Value"
        Shared IsPrinted As String = "IsPrinted"
        Shared SalesPrice As String = "SalesPrice"
        Shared FlagType As String = "FlagType"
        Shared SerialNo As String = "SerialNo"
        Shared IsDelivered As String = "IsDelivered"
        Shared SalesManId As String = "SalesManId"
        Shared Line As String = "Line"
        Shared ProductionOrderFlag As String = "ProductionOrderFlag"
        Shared ProductionOrderNo As String = "ProductionOrderNo"
        Shared SD_Notes As String = "SD_Notes"
        Shared AvgCost As String = "AvgCost"
        Shared AvgCostOne As String = "AvgCostOne"
        Shared SalesReturnStateId As String = "SalesReturnStateId"
        Shared SalesReturnStateReason As String = "SalesReturnStateReason"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.SalesInvoiceNo, "رقم الفاتورة")
        G.Columns.Add(GC.Barcode, "الباركود")
        G.Columns.Add(GC.ItemSerialNo, "السيريال")
        G.Columns.Add(GC.Id, "كود الصنف")
        G.Columns.Add(GC.Name, "اسم الصنف")
        G.Columns.Add(GC.ExpDate, "تاريخ الصلاحية")
        G.Columns.Add(GC.ExpireDate, "ExpireDate")
        G.Columns.Add(GC.GroupName, "المجموعة")
        G.Columns.Add(GC.TypeName, "النوع")

        Dim GCColor As New Forms.DataGridViewComboBoxColumn
        GCColor.HeaderText = "اللون"
        GCColor.Name = GC.Color
        bm.FillCombo("select 0 Id,'' Name", GCColor)
        G.Columns.Add(GCColor)

        Dim GCSize As New Forms.DataGridViewComboBoxColumn
        GCSize.HeaderText = "المقاس"
        GCSize.Name = GC.Size
        bm.FillCombo("select 0 Id,'' Name", GCSize)
        G.Columns.Add(GCSize)

        Dim GCUnitId As New Forms.DataGridViewComboBoxColumn
        GCUnitId.HeaderText = "الوحدة"
        GCUnitId.Name = GC.UnitId
        bm.FillCombo("select 0 Id,'' Name", GCUnitId)
        G.Columns.Add(GCUnitId)

        G.Columns.Add(GC.UnitQty, "عدد الفرعى")

        Dim GCUnitsWeightId As New Forms.DataGridViewComboBoxColumn
        GCUnitsWeightId.HeaderText = "الوحدة"
        GCUnitsWeightId.Name = GC.UnitsWeightId
        bm.FillCombo("select 0 Id,'' Name union all select Id,Name from UnitsWeights where Id>0", GCUnitsWeightId)
        G.Columns.Add(GCUnitsWeightId)

        G.Columns.Add(GC.UnitsWeightQty, "عدد الفرعى")

        G.Columns.Add(GC.CurrentBal, "الرصيد")

        G.Columns.Add(GC.PreQty, "العدد")
        G.Columns.Add(GC.ConsumptionQty, " المطلوبةالكمية")
        G.Columns.Add(GC.ConsumptionRemainingQty, "الكمية المتبقية")
        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.Qty2, "العدد/عبوة")
        G.Columns.Add(GC.Qty3, "عدد العبوات")
        G.Columns.Add(GC.ReceivedQty, "الكمية المستلمة")

        Dim GCSalesPriceTypeId As New Forms.DataGridViewComboBoxColumn
        GCSalesPriceTypeId.HeaderText = "نوع السعر"
        GCSalesPriceTypeId.Name = GC.SalesPriceTypeId
        bm.FillCombo("select Id,Name from SalesPriceTypes", GCSalesPriceTypeId)
        G.Columns.Add(GCSalesPriceTypeId)

        G.Columns.Add(GC.Price, "السعر")
        G.Columns.Add(GC.UnitSub, "الوحدة (فرعى)")
        G.Columns.Add(GC.QtySub, "الكمية (فرعى)")
        G.Columns.Add(GC.PriceSub, "السعر (فرعى)")
        G.Columns.Add(GC.ItemDiscountPerc, "نسبة الخصم")
        G.Columns.Add(GC.ItemDiscount, "قيمة الخصم")
        G.Columns.Add(GC.Value, "القيمة")
        G.Columns.Add(GC.IsPrinted, "طباعة للمطبخ")
        G.Columns.Add(GC.SalesPrice, "سعر البيع")

        Dim GCFlagType As New Forms.DataGridViewComboBoxColumn
        GCFlagType.HeaderText = "النوع"
        GCFlagType.Name = GC.FlagType
        If Flag = FlagState.المبيعات OrElse Flag = FlagState.التصدير Then
            bm.FillCombo("select 0 Id,'-' Name union all select 13 Id,'المبيعات' Name union all select 6 Id,'هدايا' Name union all select 25 Id,'عينات' Name", GCFlagType)
        Else
            bm.FillCombo("select 0 Id,'-' Name union all select 6 Id,'هدايا' Name union all select 25 Id,'عينات' Name", GCFlagType)
        End If
        G.Columns.Add(GCFlagType)

        G.Columns.Add(GC.SerialNo, "رقم الإذن")
        G.Columns.Add(GC.IsDelivered, "تم التسليم")
        G.Columns.Add(GC.SalesManId, "المندوب")
        G.Columns.Add(GC.Line, "Line")


        Dim GCProductionOrderFlag As New Forms.DataGridViewComboBoxColumn
        GCProductionOrderFlag.HeaderText = "نوع أمر الانتاج"
        GCProductionOrderFlag.Name = GC.ProductionOrderFlag
        bm.FillCombo("select 0 Id,'-' Name union all select Id,Name from ProductionItemCollectionMotionFlags where Flag=5 Order by Id", GCProductionOrderFlag)
        G.Columns.Add(GCProductionOrderFlag)


        Dim GCSalesReturnStateId As New Forms.DataGridViewComboBoxColumn
        GCSalesReturnStateId.HeaderText = "الحالة"
        GCSalesReturnStateId.Name = GC.SalesReturnStateId
        bm.FillCombo("select 0 Id,'-' Name union all select Id,Name from SalesReturnStates Order by Id", GCSalesReturnStateId)
        G.Columns.Add(GCSalesReturnStateId)

        G.Columns.Add(GC.SalesReturnStateReason, "سبب الحالة")

        G.Columns.Add(GC.ProductionOrderNo, "رقم أمر الانتاج")
        G.Columns.Add(GC.SD_Notes, "ملاحظات")
        G.Columns.Add(GC.AvgCost, "التكلفة")
        G.Columns.Add(GC.AvgCostOne, "التكلفة")

        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.ItemSerialNo).FillWeight = 150
        G.Columns(GC.Id).FillWeight = 110
        G.Columns(GC.Name).FillWeight = 280
        G.Columns(GC.UnitsWeightId).FillWeight = 150
        G.Columns(GC.SalesPriceTypeId).FillWeight = 150
        G.Columns(GC.ProductionOrderFlag).FillWeight = 240
        G.Columns(GC.SD_Notes).FillWeight = 280
        G.Columns(GC.SalesReturnStateId).FillWeight = 240
        G.Columns(GC.SalesReturnStateReason).FillWeight = 240

        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.GroupName).ReadOnly = True
        G.Columns(GC.TypeName).ReadOnly = True
        G.Columns(GC.UnitQty).ReadOnly = True
        G.Columns(GC.Qty3).ReadOnly = True



        'If Not Md.Manager AndAlso TestConsumablesAndReturn() Then
        '    G.Columns(GC.Price).ReadOnly = True
        'End If

        'G.Columns(GC.Price).ReadOnly = ReadOnlyState()

        'If Not Md.Manager AndAlso Not Md.EditPrices AndAlso (Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X) AndAlso TestConsumablesAndReturn() Then
        '    'G.Columns(GC.UnitId).Visible = False
        '    G.Columns(GC.Price).ReadOnly = True
        'End If




        G.Columns(GC.UnitSub).ReadOnly = True
        G.Columns(GC.PriceSub).ReadOnly = ReadOnlyState()
        G.Columns(GC.ItemDiscountPerc).ReadOnly = ReadOnlyState()
        G.Columns(GC.ItemDiscount).ReadOnly = ReadOnlyState()
        G.Columns(GC.Value).ReadOnly = True
        'G.Columns(GC.SerialNo).ReadOnly = True
        G.Columns(GC.CurrentBal).ReadOnly = True

        G.Columns(GC.ExpireDate ).Visible = False
        G.Columns(GC.CurrentBal).Visible = False
        G.Columns(GC.UnitQty).Visible = False
        G.Columns(GC.UnitSub).Visible = False
        G.Columns(GC.QtySub).Visible = False
        G.Columns(GC.PriceSub).Visible = False
        G.Columns(GC.IsPrinted).Visible = False

        G.Columns(GC.UnitsWeightQty).Visible = False

        G.Columns(GC.ReceivedQty).Visible = False
        If Receive Then
            G.AllowUserToAddRows = False
            G.AllowUserToDeleteRows = False
            G.Columns(GC.ReceivedQty).Visible = True
            For i As Integer = 0 To G.Columns.Count - 1
                G.Columns(i).ReadOnly = True
            Next
            G.Columns(GC.ReceivedQty).ReadOnly = False
        End If

        If Receive OrElse Flag = FlagState.تحويل_إلى_مخزن OrElse TestImportAndReturn() Then
            G.Columns(GC.Qty2).Visible = False
            G.Columns(GC.Qty3).Visible = False
        End If


        If Flag <> FlagState.تحويل_إلى_مخزن OrElse Md.MyProjectType <> ProjectType.Zohor Then
            G.Columns(GC.ConsumptionQty).Visible = False
            G.Columns(GC.ConsumptionRemainingQty).Visible = False
        End If
        G.Columns(GC.ConsumptionQty).ReadOnly = True
        G.Columns(GC.ConsumptionRemainingQty).ReadOnly = True

        G.Columns(GC.UnitId).Visible = Md.ShowQtySub
        G.Columns(GC.Color).Visible = Md.ShowColorAndSize
        G.Columns(GC.Size).Visible = Md.ShowColorAndSize

        G.Columns(GC.FlagType).Visible = False
        G.Columns(GC.SerialNo).Visible = False
        G.Columns(GC.IsDelivered).Visible = False
        G.Columns(GC.SalesManId).Visible = False
        G.Columns(GC.ItemDiscountPerc).Visible = False
        G.Columns(GC.ItemDiscount).Visible = False

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            If TestSalesOnly() OrElse TestPurchaseOnly() OrElse TestImportOnly() Then
                G.Columns(GC.ItemDiscountPerc).Visible = True
                G.Columns(GC.ItemDiscount).Visible = True
            End If
        End If

        If Not TestPurchaseOnly() AndAlso Not TestImportOnly() Then
            G.Columns(GC.SalesPrice).Visible = False
        End If
        If Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.SalesPrice).Visible = False
        End If


        Select Case Flag
            Case FlagState.مردودات_الاستيراد, FlagState.مردودات_التصدير, FlagState.مردودات_المبيعات, FlagState.مردودات_المستهلكات, FlagState.مردودات_مبيعات_التوصيل, FlagState.مردودات_مبيعات_الجملة, FlagState.مردودات_مبيعات_الصالة, FlagState.مردودات_مبيعات_نصف_الجملة, FlagState.مردودات_مشتريات
            Case Else
                G.Columns(GC.SalesInvoiceNo).Visible = False
        End Select

        If Md.MyProjectType = ProjectType.X AndAlso Flag = FlagState.تحويل_إلى_مخزن Then
            G.Columns(GC.SalesInvoiceNo).Visible = True
        End If


        Select Case Flag
            Case FlagState.مردودات_المبيعات, FlagState.مردودات_مبيعات_التوصيل, FlagState.مردودات_مبيعات_الجملة, FlagState.مردودات_مبيعات_الصالة, FlagState.مردودات_مبيعات_نصف_الجملة
            Case Else
                G.Columns(GC.SalesReturnStateId).Visible = False
                G.Columns(GC.SalesReturnStateReason).Visible = False
        End Select

        G.BarcodeIndex = G.Columns(GC.Barcode).Index
        If Not Md.ShowBarcode Then
            G.Columns(GC.Barcode).Visible = False
            'btnPrint.Visibility = Visibility.Hidden
        End If
        If Not Md.ShowItemSerialNo Then
            G.Columns(GC.ItemSerialNo).Visible = False
        End If

        If Not Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.Qty2).Visible = False
            G.Columns(GC.Qty3).Visible = False
        End If

        If Md.MyProjectType = ProjectType.X Then
            Select Case Flag
                Case FlagState.المبيعات, FlagState.صرف
                    'G.Columns(GC.FlagType).Visible = True
            End Select
            Select Case Flag
                Case FlagState.أرصدة_افتتاحية, FlagState.الاستيراد, FlagState.مردودات_الاستيراد, FlagState.مردودات_مشتريات, FlagState.مردودات_مشتريات_خارجية, FlagState.التصدير, FlagState.مردودات_التصدير, FlagState.عرض_أسعار, FlagState.أمر_توريد, FlagState.أمر_شراء, FlagState.إضافة, FlagState.صرف, FlagState.تحويل_إلى_مخزن, FlagState.المبيعات, FlagState.عرض_أسعار_مورد
                Case Else
                    'G.Columns(GC.SerialNo).Visible = True
            End Select
        End If

        If Md.MyProjectType = ProjectType.X Then
            If (TestPurchaseAndReturn() AndAlso Not Md.UserSeePurchasesPrice) _
                OrElse (TestSalesAndReturn() AndAlso Not Md.UserSeeSalesPrice) _
                OrElse (TestImportAndReturn() AndAlso Not Md.UserSeeImportPrice) _
               OrElse Flag = FlagState.تحويل_إلى_مخزن Then
                G.Columns(GC.Price).Visible = False
                G.Columns(GC.Value).Visible = False
                G.Columns(GC.ItemDiscountPerc).Visible = False
                G.Columns(GC.ItemDiscount).Visible = False
            End If
        End If

        G.Columns(GC.Line).Visible = False

        If Not Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.ProductionOrderFlag).Visible = False
            G.Columns(GC.ProductionOrderNo).Visible = False
        End If

        If Not (Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X) Then
            G.Columns(GC.SD_Notes).Visible = False
        End If

        If Not Md.ShowItemExpireDate Then
            G.Columns(GC.ExpDate).Visible = False
        End If


        If Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.UnitId).Visible = False
            G.Columns(GC.TypeName).Visible = False
        Else
            G.Columns(GC.GroupName).Visible = False
            G.Columns(GC.TypeName).Visible = False
        End If

        If Not (Md.MyProjectType = ProjectType.X AndAlso TestImportOnly()) Then
            G.Columns(GC.AvgCostOne).Visible = False
        End If
        G.Columns(GC.AvgCost).Visible = False
        G.Columns(GC.AvgCost).ReadOnly = True
        G.Columns(GC.AvgCostOne).ReadOnly = True

        If Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.UnitId).Visible = False
            G.Columns(GC.Qty).ReadOnly = True
        Else
            G.Columns(GC.UnitsWeightId).Visible = False
            G.Columns(GC.UnitsWeightQty).Visible = False
            G.Columns(GC.PreQty).Visible = False
            G.Columns(GC.SalesPriceTypeId).Visible = False
        End If

        If Md.MyProjectType = ProjectType.X Then
            G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
            For i As Integer = 0 To G.ColumnCount - 1
                G.Columns(i).Width = G.Columns(i).FillWeight
            Next
        End If

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.SelectionChanged, AddressOf G_SelectionChanged
    End Sub

    Function ReadOnlyState() As Boolean
        If Md.MyProjectType = ProjectType.X AndAlso (Flag = FlagState.تسوية_صرف OrElse Flag = FlagState.صرف) Then
            Return True
        ElseIf Md.Manager OrElse Md.EditPrices Then
            Return False
        ElseIf Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            Return False
        ElseIf Not Md.Manager AndAlso TestConsumablesAndReturn() Then
            Return True
        ElseIf TestSalesAndReturn() OrElse TestConsumablesAndReturn() Then
            Return True
        Else
            Return False
        End If
    End Function

    Function Fm() As Integer
        Select Case Flag
            Case FlagState.مبيعات_الصالة, FlagState.مردودات_مبيعات_الصالة
                Return 1
            Case FlagState.المبيعات, FlagState.مردودات_المبيعات, FlagState.التصدير, FlagState.مردودات_التصدير, FlagState.مبيعات_لمستثمر, FlagState.مردودات_مبيعات_لمستثمر
                Return 2
            Case FlagState.مبيعات_التوصيل, FlagState.مردودات_مبيعات_التوصيل
                Return 3
            Case FlagState.المستهلكات, FlagState.مردودات_المستهلكات, FlagState.مستهلكات_الداخلي, FlagState.مردودات_مستهلكات_الداخلي, FlagState.مستهلكات_العمليات, FlagState.مردودات_مستهلكات_العمليات
                Return 4
            Case Else
                Return 0
        End Select
    End Function

    Sub LoadGroups()
        Try
            WGroups.Children.Clear()
            WTypes.Children.Clear()
            WItems.Children.Clear()
            TabGroups.Header = Gp
            TabTypes.Header = Tp
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("LoadGroups2", New String() {"Form"}, New String() {Fm()})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WGroups.Children.Add(x)
                AddHandler x.Click, AddressOf LoadTypes
            Next
        Catch
        End Try
    End Sub

    Sub LoadTables()
        Try
            WTables.Children.Clear()
            WSubTables.Children.Clear()
            Dim dt As DataTable = bm.ExecuteAdapter("LoadTables", New String() {"StoreId"}, New String() {StoreId.Text})
            Dim dtInv As DataTable = bm.ExecuteAdapter("select InvoiceNo,TableId,TableSubId,dbo.ToStrTime(OpennedDate) OpennedTime,NoOfPersons,IsCashierPrinted from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and IsClosed=0")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Name = "Table_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Width = 100
                x.Height = 100
                x.Cursor = Input.Cursors.Pen
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = x.Content
                WTables.Children.Add(x)

                If dtInv.Select("TableId=" & x.Tag & " and TableSubId>1").Length > 0 Then
                    x.Background = System.Windows.Media.Brushes.MediumSpringGreen
                    x.Content &= vbCrLf & "مائدة مقسمة"
                ElseIf dtInv.Select("TableId=" & x.Tag).Length > 0 Then
                    If dtInv.Select("TableId=" & x.Tag)(0)("IsCashierPrinted") = 1 Then
                        x.Background = System.Windows.Media.Brushes.Magenta
                    Else
                        x.Background = System.Windows.Media.Brushes.Red
                    End If
                    x.Content &= vbCrLf & dtInv.Select("TableId=" & x.Tag)(0).Item("OpennedTime").ToString & vbCrLf & "العدد: " & dtInv.Select("TableId=" & x.Tag)(0).Item("NoOfPersons").ToString
                Else
                    x.Background = System.Windows.Media.Brushes.LimeGreen
                End If

                AddHandler x.Click, AddressOf btnTableClick
            Next
        Catch
        End Try
    End Sub

    Sub LoadUnPaiedInvoices()
        Try
            WDelivery.Children.Clear()
            Dim dt As DataTable = bm.ExecuteAdapter("select InvoiceNo,dbo.ToStrTime(OpennedDate) OpennedTime from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and IsClosed=0")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Name = "Delivery_" & dt.Rows(i)("InvoiceNo").ToString
                x.Tag = dt.Rows(i)("InvoiceNo").ToString
                x.Width = 100
                x.Height = 100
                x.Cursor = Input.Cursors.Pen
                x.Content = dt.Rows(i)("InvoiceNo").ToString & vbCrLf & vbCrLf & dt.Rows(i)("OpennedTime").ToString
                x.ToolTip = x.Content
                WDelivery.Children.Add(x)
                x.Background = System.Windows.Media.Brushes.Red
                AddHandler x.Click, AddressOf btnDeliveryClick
            Next
        Catch
        End Try
    End Sub

    Private Sub LoadTypes(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WTypes.Tag = xx.Tag
            WTypes.Children.Clear()
            WItems.Children.Clear()

            TabTypes.Header = Tp & " - " & xx.Content.ToString
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("LoadTypes2", New String() {"GroupId", "Form"}, New String() {xx.Tag.ToString, Fm()})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & xx.Tag.ToString & "_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WTypes.Children.Add(x)
                AddHandler x.Click, AddressOf LoadItems
            Next
        Catch
        End Try
    End Sub


    Sub LoadAllItems()
        Try
            HelpDt = bm.ExecuteAdapter("Select cast(Id as nvarchar(100))Id," & Resources.Item("CboName") & " Name," & PriceFieldName(GC.Price, 0) & " Price From Items  where IsStopped=0 " & ItemWhere())
            HelpDt.TableName = "tbl"
            HelpDt.Columns(0).ColumnName = FirstColumn
            HelpDt.Columns(1).ColumnName = SecondColumn
            HelpDt.Columns(2).ColumnName = ThirdColumn

            dv.Table = HelpDt
            HelpGD.ItemsSource = dv
            HelpGD.Columns(0).Width = 75
            HelpGD.Columns(1).Width = 220
            HelpGD.Columns(2).Width = 75

            If (Md.Receptionist AndAlso Md.MyProjectType = ProjectType.X) OrElse (Not Md.Manager AndAlso Md.Nurse AndAlso Md.MyProjectType = ProjectType.Zohor) Then
                HelpGD.Columns(2).Visibility = Visibility.Hidden
            End If

            HelpGD.SelectedIndex = 0
        Catch
        End Try

    End Sub

    Private Sub txtId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.GotFocus
        Try
            dv.Sort = FirstColumn
        Catch
        End Try
    End Sub

    Private Sub txtName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.GotFocus
        Try
            dv.Sort = SecondColumn
        Catch
        End Try
    End Sub

    Private Sub txtPrice_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrice.GotFocus
        Try
            dv.Sort = ThirdColumn
        Catch
        End Try
    End Sub

    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtName.TextChanged, txtPrice.TextChanged
        Try
            'dv.RowFilter = " [" & FirstColumn & "] like '" & txtID.Text.Trim & "%' and [" & SecondColumn & "] like '%" & txtName.Text & "%' and [" & ThirdColumn & "] >=" & IIf(txtPrice.Text.Trim = "", 0, txtPrice.Text) & ""
            dv.RowFilter = " [" & FirstColumn & "] like '" & txtID.Text.Trim & "%' and [" & SecondColumn & "] like '%" & txtName.Text & "%' " & IIf(txtPrice.Text.Trim = "", "", " and [" & ThirdColumn & "] =" & txtPrice.Text.Trim)
        Catch
        End Try
    End Sub


    Private Sub HelpGD_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.PreviewKeyDown, txtName.PreviewKeyDown, txtPrice.PreviewKeyDown
        Try
            If e.Key = Input.Key.Up Then
                HelpGD.SelectedIndex = HelpGD.SelectedIndex - 1
            ElseIf e.Key = Input.Key.Down Then
                HelpGD.SelectedIndex = HelpGD.SelectedIndex + 1
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub HelpGD_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles HelpGD.MouseDoubleClick
        Try
            AddItem(HelpGD.Items(HelpGD.SelectedIndex)(0))
        Catch ex As Exception
        End Try
    End Sub



    Function ItemWhere() As String
        Dim st As String = ""
        st = " and ItemType in(0,1,2,3) "

        If Md.MyProjectType = ProjectType.X AndAlso TestSalesAndReturn() Then
            st = " and ItemType in(2,3) "
        End If

        If TestConsumablesAndReturn() AndAlso Not Md.MyProjectType = ProjectType.Zohor Then
            st &= " and (Flag=1 or IsService=1) "
        End If

        If Not TestSalesAndReturn() AndAlso Not TestConsumablesAndReturn() AndAlso Not Md.MyProjectType = ProjectType.Zohor Then
            st &= " and IsService=0 "
        End If
        Return st
    End Function
    Private Sub LoadItems(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WItems.Tag = xx.Tag
            WItems.Children.Clear()

            TabItems.Header = It & " - " & xx.Content.ToString

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items_View  where IsStopped=0 " & ItemWhere() & " and GroupId=" & WTypes.Tag.ToString & " and TypeId=" & xx.Tag.ToString & " order by " & IIf(Md.MyProjectType = ProjectType.X, "Id", "Name"))
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x, 370)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)(Resources.Item("CboName")).ToString
                x.ToolTip = dt.Rows(i)(Resources.Item("CboName")).ToString
                WItems.Children.Add(x)
                AddHandler x.Click, AddressOf TabItem
            Next
        Catch
        End Try
    End Sub

    Private Sub TabItem(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        AddItem(x.Tag)
    End Sub

    Dim LopItem As Boolean = False
    Function AddItem(ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1) As Integer
        Try
            If LopItem Then Return i
            G.EndEdit()
            If Not TabControl1.SelectedIndex = 0 Then TabControl1.SelectedIndex = 0
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            'G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                'For x As Integer = 0 To G.Rows.Count - 1
                '    If Not G.Rows(x).Cells(GC.Id).Value Is Nothing AndAlso G.Rows(x).Cells(GC.Id).Value.ToString = Id.ToString AndAlso Not G.Rows(x).ReadOnly AndAlso Not G.Rows(x).Cells(GC.IsPrinted).Value = 1 Then
                '        If Md.MyProjectType = ProjectType.X Then
                '            i = x
                '            Exists = True
                '            GoTo Br
                '        Else
                '            bm.ShowMSG("تم تكرار هذا الصنف بالسطر رقم " + (x + 1).ToString)
                '        End If
                '        Exit For
                '    End If
                'Next


                Dim xx As Integer = bm.GridExistsColumn(G, GC.Id, Id.ToString)
                If xx >= 0 Then
                    bm.ShowMSG("تم تكرار هذا الصنف بالسطر رقم " + (xx + 1).ToString)
                End If



                i = G.Rows.Add()
                LopItem = True
                G.CurrentCell = G.Rows(i).Cells(GC.Name)
                LopItem = False
Br:
            End If

            GetItemNameAndBal(i, Id, G.Rows(i).Cells(GC.ItemSerialNo).Value)
            If G.Rows(i).Cells(GC.SalesPriceTypeId).Value Is Nothing Then
                G.Rows(i).Cells(GC.SalesPriceTypeId).Value = "1"
            End If

            'G.Rows(i).Cells(GC.Unit).Value = dr(0)(GC.Unit)
            LoadItemUint(i)


            If Val(G.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            If Add + Val(G.Rows(i).Cells(GC.Qty).Value) > 1 Then Add = 0
            G.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Rows(i).Cells(GC.Qty).Value)

            If Val(G.Rows(i).Cells(GC.Qty2).Value) = 0 Then G.Rows(i).Cells(GC.Qty2).Value = 1
            If Val(G.Rows(i).Cells(GC.ReceivedQty).Value) = 0 Then G.Rows(i).Cells(GC.ReceivedQty).Value = 0

            LoadItemPrice(i)
            G.Rows(i).Cells(GC.UnitSub).Value = 0 'dr(0)(GC.UnitSub)
            G.Rows(i).Cells(GC.QtySub).Value = 0
            G.Rows(i).Cells(GC.PriceSub).Value = 0 'dr(0)(PriceFieldName(GC.PriceSub))
            If G.Rows(i).Cells(GC.IsPrinted).Value <> 1 Then G.Rows(i).Cells(GC.IsPrinted).Value = 0

            If Md.ShowItemExpireDate AndAlso ((Flag = FlagState.تحويل_إلى_مخزن OrElse Flag = FlagState.تسوية_صرف OrElse Flag = FlagState.صرف OrElse Flag = FlagState.مردودات_مشتريات) OrElse TestConsumablesAndReturn()) Then
                dt = bm.ExecuteAdapter("select top 1 ExpDate,ExpireDate,cast(isnull(sum(Qty),0) as nvarchar(100))Qty from dbo.Fn_AllItemMotion(" & Val(StoreId.Text) & "," & G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value.ToString & ",'" & bm.ToStrDate(DayDate.SelectedDate) & "') Group by ExpDate,ExpireDate having sum(Qty)>0 order by ExpireDate")
                If dt.Rows.Count > 0 Then
                    G.Rows(i).Cells(GC.ExpDate).Value = dt.Rows(0)(0)
                    GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ExpDate).Index, i))
                End If
            Else
                G.Rows(i).Cells(GC.ExpireDate).Value = "1900-01-01"
            End If

            If Md.MyProjectType = ProjectType.X AndAlso Flag = FlagState.المبيعات Then
                If G.Rows(i).Cells(GC.FlagType).Value Is Nothing Then G.Rows(i).Cells(GC.FlagType).Value = FlagState.المبيعات.ToString
            Else
                If G.Rows(i).Cells(GC.FlagType).Value Is Nothing Then G.Rows(i).Cells(GC.FlagType).Value = "0"
            End If

            If (Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X) AndAlso (Flag = FlagState.إضافة OrElse Flag = FlagState.مشتريات OrElse Flag = FlagState.الاستيراد OrElse Flag = FlagState.تحويل_إلى_مخزن OrElse Flag = FlagState.المبيعات) Then
                If i = 0 Then
                    Dim x As Integer = 1
                    Select Case Flag
                        Case FlagState.تحويل_إلى_مخزن
                            x = 2
                        Case FlagState.المبيعات, FlagState.صرف
                            x = 3
                        Case FlagState.مردودات_مشتريات
                            x = 4
                        Case FlagState.مردودات_المبيعات
                            x = 5
                    End Select
                    If G.Rows(i).Cells(GC.SerialNo).Visible Then
                        If Flag = FlagState.المبيعات Then
                            'DocNo.Text = Val(bm.ExecuteScalar("select dbo.GetSalesNewSerial(" & StoreId.Text & "," & x & ")"))
                        Else
                            G.Rows(i).Cells(GC.SerialNo).Value = Val(bm.ExecuteScalar("select dbo.GetSalesNewSerial(" & StoreId.Text & "," & x & ")"))
                        End If
                    End If
                ElseIf Val(G.Rows(i).Cells(GC.SerialNo).Value) = 0 Then
                    'G.Rows(i).Cells(GC.SerialNo).Value = Val(G.Rows(0).Cells(GC.SerialNo).Value) + (i - (i Mod SalesSerialNoCount)) / SalesSerialNoCount
                    G.Rows(i).Cells(GC.SerialNo).Value = Val(G.Rows(i - 1).Cells(GC.SerialNo).Value)
                End If
            End If

            CalcRow(i)
            If Move Then
                G.Focus()
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
            If Exists Then
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.Price)
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
        Catch ex As Exception
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try

        Return i
    End Function

    Dim lop As Boolean = False
    Sub CalcRow(ByVal i As Integer)
        Try
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value.ToString = "" Then
                ClearRow(i)
                CalcTotal()
                Return
            End If
            G.Rows(i).Cells(GC.Qty).Value = Val(G.Rows(i).Cells(GC.Qty).Value)

            If Not lop AndAlso (Flag = FlagState.المبيعات OrElse Flag = FlagState.مردودات_مشتريات OrElse Flag = FlagState.مردودات_مشتريات_خارجية OrElse Flag = FlagState.تحويل_إلى_مخزن OrElse Flag = FlagState.عرض_أسعار OrElse Flag = FlagState.أمر_توريد OrElse Flag = FlagState.صرف OrElse Flag = FlagState.تسوية_صرف) AndAlso Val(G.Rows(i).Cells(GC.Qty).Value) > Val(G.Rows(i).Cells(GC.CurrentBal).Value) Then
                If G.CurrentCell.ColumnIndex = G.Columns(GC.Qty).Index Then
                    bm.ShowMSG("رصيد الصنف لا يسمح")
                End If
                If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                    G.Rows(i).Cells(GC.Qty).Value = 0
                End If
            End If
            G.Rows(i).Cells(GC.Qty2).Value = Val(G.Rows(i).Cells(GC.Qty2).Value)
            'G.Rows(i).Cells(GC.Qty3).Value = Math.Round(Val(G.Rows(i).Cells(GC.Qty).Value) / Val(G.Rows(i).Cells(GC.Qty2).Value), 2, MidpointRounding.AwayFromZero)
            G.Rows(i).Cells(GC.Qty3).Value = (Val(G.Rows(i).Cells(GC.Qty).Value) - (Val(G.Rows(i).Cells(GC.Qty).Value) Mod Val(G.Rows(i).Cells(GC.Qty2).Value))) / Val(G.Rows(i).Cells(GC.Qty2).Value) + IIf(Val(G.Rows(i).Cells(GC.Qty).Value) Mod Val(G.Rows(i).Cells(GC.Qty2).Value) > 0, 1, 0)
            G.Rows(i).Cells(GC.Price).Value = Val(G.Rows(i).Cells(GC.Price).Value)
            G.Rows(i).Cells(GC.QtySub).Value = Val(G.Rows(i).Cells(GC.QtySub).Value)
            G.Rows(i).Cells(GC.PriceSub).Value = Val(G.Rows(i).Cells(GC.PriceSub).Value)
            G.Rows(i).Cells(GC.SalesPrice).Value = Val(G.Rows(i).Cells(GC.SalesPrice).Value)

            'G.Rows(i).Cells(GC.Value).Value = Math.Round(Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.Price).Value) + Val(G.Rows(i).Cells(GC.QtySub).Value) * Val(G.Rows(i).Cells(GC.PriceSub).Value), 2)

            If Val(G.Rows(i).Cells(GC.SalesPriceTypeId).Value) = 2 Then
                G.Rows(i).Cells(GC.Value).Value = Val(G.Rows(i).Cells(GC.PreQty).Value) * (Val(G.Rows(i).Cells(GC.Price).Value) - Val(G.Rows(i).Cells(GC.ItemDiscount).Value)) + Val(G.Rows(i).Cells(GC.QtySub).Value) * Val(G.Rows(i).Cells(GC.PriceSub).Value)
            Else
                G.Rows(i).Cells(GC.Value).Value = Val(G.Rows(i).Cells(GC.Qty).Value) * (Val(G.Rows(i).Cells(GC.Price).Value) - Val(G.Rows(i).Cells(GC.ItemDiscount).Value)) + Val(G.Rows(i).Cells(GC.QtySub).Value) * Val(G.Rows(i).Cells(GC.PriceSub).Value)
            End If

            G.Rows(i).Cells(GC.ConsumptionRemainingQty).Value = Val(G.Rows(i).Cells(GC.ConsumptionQty).Value) - Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.UnitQty).Value)

            ItemBal.Text = G.CurrentRow.Cells(GC.CurrentBal).Value
        Catch ex As Exception
            ClearRow(i)
        End Try
        CalcTotal()
    End Sub

    Sub ClearRow(ByVal i As Integer)
        'G.Rows(i).Cells(GC.SalesInvoiceNo).Value = Nothing
        G.Rows(i).Cells(GC.Barcode).Value = Nothing
        G.Rows(i).Cells(GC.ItemSerialNo).Value = Nothing
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.ExpDate).Value = Nothing
        G.Rows(i).Cells(GC.ExpireDate).Value = Nothing
        G.Rows(i).Cells(GC.GroupName).Value = Nothing
        G.Rows(i).Cells(GC.TypeName).Value = Nothing
        G.Rows(i).Cells(GC.Color).Value = Nothing
        G.Rows(i).Cells(GC.Size).Value = Nothing
        G.Rows(i).Cells(GC.UnitId).Value = Nothing
        G.Rows(i).Cells(GC.UnitQty).Value = Nothing
        G.Rows(i).Cells(GC.ConsumptionQty).Value = Nothing
        G.Rows(i).Cells(GC.ConsumptionRemainingQty).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.Qty2).Value = Nothing
        G.Rows(i).Cells(GC.Qty3).Value = Nothing
        G.Rows(i).Cells(GC.ReceivedQty).Value = Nothing
        G.Rows(i).Cells(GC.Price).Value = Nothing
        G.Rows(i).Cells(GC.UnitSub).Value = Nothing
        G.Rows(i).Cells(GC.QtySub).Value = Nothing
        G.Rows(i).Cells(GC.PriceSub).Value = Nothing
        G.Rows(i).Cells(GC.ItemDiscountPerc).Value = Nothing
        G.Rows(i).Cells(GC.ItemDiscount).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.IsPrinted).Value = Nothing
        G.Rows(i).Cells(GC.SalesPrice).Value = Nothing
        G.Rows(i).Cells(GC.FlagType).Value = Nothing
        G.Rows(i).Cells(GC.SerialNo).Value = Nothing
        G.Rows(i).Cells(GC.IsDelivered).Value = Nothing
        G.Rows(i).Cells(GC.SalesManId).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
        G.Rows(i).Cells(GC.ProductionOrderFlag).Value = Nothing
        G.Rows(i).Cells(GC.ProductionOrderNo).Value = Nothing
        G.Rows(i).Cells(GC.SD_Notes).Value = Nothing
        G.Rows(i).Cells(GC.AvgCost).Value = Nothing
        G.Rows(i).Cells(GC.AvgCostOne).Value = Nothing

        G.Rows(i).Cells(GC.UnitsWeightId).Value = Nothing
        G.Rows(i).Cells(GC.UnitsWeightQty).Value = Nothing
        G.Rows(i).Cells(GC.PreQty).Value = Nothing
        G.Rows(i).Cells(GC.SalesPriceTypeId).Value = Nothing
        G.Rows(i).Cells(GC.SalesReturnStateId).Value = Nothing
        G.Rows(i).Cells(GC.SalesReturnStateReason).Value = Nothing
        ItemBal.Clear()
    End Sub
    Private Sub RdoCash_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoCash.Checked, RdoVisa.Checked, RdoCashVisa.Checked, RdoFuture.Checked, RdoCashFuture.Checked, RdoEmployees.Checked
        Try
            lblGroupBoxPaymentType.Content = "طريقة الدفع : " & CType(sender, RadioButton).Content
            PaymentType.Text = 0
            If RdoCash.IsChecked Then
                PaymentType.Text = 1
            ElseIf RdoVisa.IsChecked Then
                PaymentType.Text = 2
            ElseIf RdoCashVisa.IsChecked Then
                PaymentType.Text = 3
            ElseIf RdoFuture.IsChecked Then
                PaymentType.Text = 4
            ElseIf RdoCashFuture.IsChecked Then
                PaymentType.Text = 5
            ElseIf RdoEmployees.IsChecked Then
                PaymentType.Text = 6
            End If
        Catch ex As Exception
        End Try

        If GroupBoxPaymentType.Visibility = Visibility.Hidden Then Return

        Try
            If RdoCashVisa.IsChecked OrElse RdoCashFuture.IsChecked Then
                CashValue.Visibility = Visibility.Visible
                lblCashValue.Visibility = Visibility.Visible
            Else
                CashValue.Visibility = Visibility.Hidden
                lblCashValue.Visibility = Visibility.Hidden
                CashValue.Text = 0
            End If
        Catch ex As Exception
        End Try

        Try
            If (RdoCash.IsChecked OrElse RdoCashFuture.IsChecked OrElse RdoCashVisa.IsChecked) AndAlso (TestPurchaseAndReturn() OrElse TestSalesAndReturn() OrElse TestExportAndReturn()) Then
                If Not Md.MyProjectType = ProjectType.X Then
                    lblSaveId.Visibility = Visibility.Visible
                    SaveId.Visibility = Visibility.Visible
                    SaveName.Visibility = Visibility.Visible
                End If
            Else
                lblSaveId.Visibility = Visibility.Hidden
                SaveId.Visibility = Visibility.Hidden
                SaveName.Visibility = Visibility.Hidden
            End If
        Catch ex As Exception
        End Try

        Try
            If (RdoVisa.IsChecked OrElse RdoCashVisa.IsChecked) AndAlso (TestPurchaseAndReturn() OrElse TestSalesAndReturn()) Then
                If Not Md.MyProjectType = ProjectType.X Then
                    lblBankId.Visibility = Visibility.Visible
                    BankId.Visibility = Visibility.Visible
                    BankName.Visibility = Visibility.Visible
                End If
            Else
                lblBankId.Visibility = Visibility.Hidden
                BankId.Visibility = Visibility.Hidden
                BankName.Visibility = Visibility.Hidden
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        If Lop3 Then Return
        Try
            If G.CurrentCell.ColumnIndex = G.Columns(GC.SalesInvoiceNo).Index Then
                If Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.SalesInvoiceNo).Value) > 0 AndAlso Flag <> FlagState.تحويل_إلى_مخزن Then
                    dt = bm.ExecuteAdapter("select ToId from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & MainFlag() & " and InvoiceNo=" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.SalesInvoiceNo).Value & " and ToId=" & Val(ToId.Text))
                    If dt.Rows.Count = 0 Then
                        bm.ShowMSG("هذه الفاتورة لا تخص هذا " & lblToId.Content)
                        G.Rows(G.CurrentCell.RowIndex).Cells(GC.SalesInvoiceNo).Value = ""
                    End If
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.ItemSerialNo AndAlso Not G.Rows(e.RowIndex).Cells(GC.ItemSerialNo).Value Is Nothing AndAlso Not G.Rows(e.RowIndex).Cells(GC.ItemSerialNo).Value = "" Then
                dt = bm.ExecuteAdapter("select top 1 ItemId from SalesDetails where ItemSerialNo='" & G.Rows(e.RowIndex).Cells(GC.ItemSerialNo).Value.ToString & "' and Flag=9 and not(StoreId=" & Val(StoreId.Text) & " and Flag=" & Flag & " and InvoiceNo=" & Val(InvoiceNo.Text) & ")")
                If dt.Rows.Count > 0 Then
                    If Md.MyProjectType = ProjectType.X AndAlso TestPurchaseOnly() Then
                        bm.ShowMSG("تم شراء هذا الصنف من قبل")
                        G.Rows(e.RowIndex).Cells(GC.ItemSerialNo).Value = ""
                    Else 'If G.Rows(e.RowIndex).Cells(GC.Id).Value = "" Then
                        G.Rows(e.RowIndex).Cells(GC.Id).Value = dt.Rows(0)("ItemId")
                        GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, e.RowIndex))
                    End If
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.Barcode AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value Is Nothing AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value = "" Then
                Dim BC As String = G.Rows(e.RowIndex).Cells(GC.Barcode).Value.ToString
                If (Md.MyProjectType = ProjectType.X) AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                    If BC.Length > 12 AndAlso Val(BC.Substring(0, 1)) > 0 Then BC = BC.Substring(0, 12)
                    BC = BC.Substring(1)
                    G.Rows(e.RowIndex).Cells(GC.Id).Value = Val(Mid(BC, 1, BC.Length - 4))
                    AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
                    G.Rows(e.RowIndex).Cells(GC.Color).Value = Integer.Parse(Val(Mid(BC, BC.Length - 3, 2)))
                    G.Rows(e.RowIndex).Cells(GC.Size).Value = Integer.Parse(Val(Mid(BC, BC.Length - 1, 2)))
                    LoadItemPrice(e.RowIndex)
                    Exit Sub
                ElseIf Md.ShowItemExpireDate Then
                    dt = bm.ExecuteAdapter("select top 1 ItemId,ExpDate,ExpireDate from SalesDetails where Barcode='" & Val(BC) & "'")
                    If dt.Rows.Count > 0 Then
                        G.Rows(e.RowIndex).Cells(GC.Id).Value = dt.Rows(0)("ItemId")
                        AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
                        G.Rows(e.RowIndex).Cells(GC.ExpDate).Value = dt.Rows(0)("ExpDate")
                        G.Rows(e.RowIndex).Cells(GC.ExpireDate).Value = dt.Rows(0)("ExpireDate").ToString.Substring(0, 10)
                        LoadItemPrice(e.RowIndex)
                        Exit Sub
                    End If
                ElseIf Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                    'G.Rows(e.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select Id from Items where IsStopped=0 and left(Barcode,12)='" & Val(BC) & "'")
                    G.Rows(e.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select Id from Items where IsStopped=0 and Barcode='" & BC & "'")
                    AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
                    Exit Sub
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.Id Then
                AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
                G_SelectionChanged(Nothing, Nothing)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.ExpDate Then
                If G.Rows(e.RowIndex).Cells(GC.ExpDate).Value.ToString.Trim <> "" Then
                    If G.Rows(e.RowIndex).Cells(GC.ExpDate).Value.ToString.Trim.Split("/").Length <> 2 Then
                        bm.ShowMSG("برجاء إدخال الصلاحية علي هيئة " & vbCrLf & Now.Month.ToString.PadLeft(2, "0") & "/" & Now.Year.ToString)
                        G.Rows(e.RowIndex).Cells(GC.ExpDate).Value = ""
                        G.Rows(e.RowIndex).Cells(GC.ExpireDate).Value = "1900-01-01"
                        Return
                    Else
                        Try
                            Dim ar() = G.Rows(e.RowIndex).Cells(GC.ExpDate).Value.ToString.Split("/")
                            G.Rows(e.RowIndex).Cells(GC.ExpireDate).Value = bm.ToStrDate(ar(1) & "-" & ar(0) & "-" & Date.DaysInMonth(ar(1), ar(0)))
                            G.Rows(e.RowIndex).Cells(GC.Barcode).Value = G.Rows(e.RowIndex).Cells(GC.Id).Value & ar(0).PadLeft(2, "0") & ar(1).Substring(ar(1).Length - 2, 2).PadLeft(2, "0")
                        Catch ex As Exception
                            bm.ShowMSG("برجاء إدخال الصلاحية علي هيئة " & vbCrLf & Now.Month.ToString.PadLeft(2, "0") & "/" & Now.Year.ToString)
                            G.Rows(e.RowIndex).Cells(GC.ExpireDate).Value = "1900-01-01"
                        End Try
                    End If
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.UnitsWeightId Then
                G.Rows(e.RowIndex).Cells(GC.UnitsWeightQty).Value = Val(bm.ExecuteScalar("select Weights from UnitsWeights where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.UnitsWeightId).Value)))
                G.Rows(e.RowIndex).Cells(GC.Qty).Value = Val(G.Rows(e.RowIndex).Cells(GC.UnitsWeightQty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PreQty).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.PreQty OrElse G.Columns(e.ColumnIndex).Name = GC.UnitsWeightQty Then
                G.Rows(e.RowIndex).Cells(GC.Qty).Value = Val(G.Rows(e.RowIndex).Cells(GC.UnitsWeightQty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PreQty).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.UnitId OrElse G.Columns(e.ColumnIndex).Name = GC.Size Then
                LoadItemPrice(e.RowIndex)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.ItemDiscountPerc AndAlso Val(G.Rows(e.RowIndex).Cells(GC.ItemDiscountPerc).Value) > 0 Then
                G.Rows(e.RowIndex).Cells(GC.ItemDiscount).Value = Val(G.Rows(e.RowIndex).Cells(GC.Price).Value) * Val(G.Rows(e.RowIndex).Cells(GC.ItemDiscountPerc).Value) / 100
            ElseIf G.Columns(e.ColumnIndex).Name = GC.ProductionOrderNo Then
                G.Rows(e.RowIndex).Cells(GC.ProductionOrderNo).Value = Val(G.Rows(e.RowIndex).Cells(GC.ProductionOrderNo).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.SerialNo Then
                If G.Rows(e.RowIndex).Cells(GC.SerialNo).Value <> "" Then
                    G.Rows(e.RowIndex).Cells(GC.SerialNo).Value = Val(G.Rows(e.RowIndex).Cells(GC.SerialNo).Value)
                End If
                Dim x As Integer = 1
                Select Case Flag
                    Case FlagState.تحويل_إلى_مخزن
                        x = 2
                    Case FlagState.المبيعات, FlagState.صرف
                        x = 3
                    Case FlagState.مردودات_مشتريات
                        x = 4
                    Case FlagState.مردودات_المبيعات
                        x = 5
                End Select
                Dim ss As Integer = Val(bm.ExecuteScalar("select dbo.GetSalesNewSerial(" & StoreId.Text & "," & x & ")"))
                If Val(G.Rows(e.RowIndex).Cells(GC.SerialNo).Value) > 0 AndAlso Val(G.Rows(e.RowIndex).Cells(GC.SerialNo).Value) < ss Then
                    If Val(bm.ExecuteScalar("select dbo.GetSalesNewSerial2(" & StoreId.Text & "," & InvoiceNo.Text & "," & x & "," & Val(G.Rows(e.RowIndex).Cells(GC.SerialNo).Value) & ")")) > 0 Then
                        bm.ShowMSG("يجب ألا يقل رقم الإذن عن " & ss)
                        G.Rows(e.RowIndex).Cells(GC.SerialNo).Value = ss
                    End If
                End If
            End If
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            CalcRow(e.RowIndex)
        Catch ex As Exception
        End Try

    End Sub


    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        Dim str As String = " where 1=1 "
        If TestConsumablesAndReturn() AndAlso Not Md.MyProjectType = ProjectType.Zohor Then
            str &= " and Flag=1 "
        End If
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")" & str) Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Dim StoreUnitId As Integer = 0
    Public Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        Dim str As String = ""
        If TestConsumablesAndReturn() AndAlso Not Md.MyProjectType = ProjectType.Zohor Then
            str = " and Flag=1"
        End If
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim() & str)

        If Md.ShowQtySub Then
            StoreUnitId = Val(bm.ExecuteScalar("select StoreUnitId from Stores where Id=" & StoreId.Text))
        End If
        ClearControls()
        If Md.ShowShiftForEveryStore Then
            dt = bm.ExecuteAdapter("select CurrentDate,CurrentShift from Stores where Id=" & StoreId.Text.Trim())
            If dt.Rows.Count > 0 Then
                DayDate.SelectedDate = dt.Rows(0)("CurrentDate")
                Shift.SelectedValue = dt.Rows(0)("CurrentShift")
            End If
        End If

    End Sub

    Private Sub SaveId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.LostFocus
        bm.LostFocus(SaveId, SaveName, "select Name from Fn_EmpPermissions(5," & Md.UserName & ") where Id=" & SaveId.Text.Trim(), True)
    End Sub

    Private Sub DoctorId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DoctorId.LostFocus
        bm.LostFocus(DoctorId, DoctorName, "select Name from Doctors where Id=" & DoctorId.Text.Trim(), True)
    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus
        bm.LostFocus(BankId, BankName, "select Name from Fn_EmpPermissions(6," & Md.UserName & ") where Id=" & BankId.Text.Trim(), True)
    End Sub

    Private Sub OrderTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles OrderTypeId.LostFocus
        bm.LostFocus(OrderTypeId, OrderTypeName, "select Name from OrderTypes where Id=" & OrderTypeId.Text.Trim(), True)
        If TestImportAndReturn() Then bm.LostFocus(OrderTypeId, AccNo, "select AccNo1 from OrderTypes where Id=" & OrderTypeId.Text.Trim(), True)
    End Sub

    Private Sub SaveId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.KeyUp
        If bm.ShowHelp("Saves", SaveId, SaveName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(5," & Md.UserName & ")") Then
            SaveId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub DoctorId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DoctorId.KeyUp
        If bm.ShowHelp("Doctors", DoctorId, DoctorName, e, "select cast(Id as varchar(100)) Id,Name from Doctors", "Doctors") Then
            DoctorId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub BankId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.KeyUp
        If bm.ShowHelp("Banks", BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(6," & Md.UserName & ")") Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub OrderTypeId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles OrderTypeId.KeyUp
        If bm.ShowHelp("OrderTypes", OrderTypeId, OrderTypeName, e, "select cast(Id as varchar(100)) Id,Name from OrderTypes") Then
            OrderTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub ToId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ToId.KeyUp
        Dim Title, tbl As String
        If Flag = FlagState.تحويل_إلى_مخزن Then
            tbl = "Stores"
            Title = "المخازن"
            If bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl) Then
                ToId_LostFocus(sender, Nothing)
            End If
        ElseIf Md.ShowCostCenter AndAlso (Flag = FlagState.تسوية_إضافة OrElse Flag = FlagState.تسوية_صرف OrElse Flag = FlagState.صرف OrElse Flag = FlagState.إضافة) Then
            If bm.CostCenterIdShowHelp(ToId, ToName, e, ) Then
                ToId_LostFocus(sender, Nothing)
            End If
        ElseIf ((TestPurchaseAndReturn() OrElse TestImportAndReturn()) AndAlso Not ReservToId.IsChecked) OrElse ((TestSalesAndReturn() OrElse TestExportAndReturn()) AndAlso ReservToId.IsChecked) Then
            tbl = "Suppliers"
            Title = "الموردين"
            If bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl, IIf(Md.MyProjectType = ProjectType.X, tbl, "")) Then
                ToId_LostFocus(sender, Nothing)
            End If
        ElseIf TestInvestorsAndReturn() Then
            tbl = "Investors"
            Title = "المستثمرين"
            If bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl, "") Then
                ToId_LostFocus(sender, Nothing)
            End If
        ElseIf ((TestSalesAndReturn() OrElse TestExportAndReturn()) AndAlso Not ReservToId.IsChecked) OrElse ((TestPurchaseAndReturn() OrElse TestImportAndReturn()) AndAlso ReservToId.IsChecked) Then
            If bm.ShowHelpCustomers(ToId, ToName, e) Then
                ToId_LostFocus(sender, Nothing)
            End If
        ElseIf TestConsumablesAndReturn() Then
            If bm.ShowHelpCases(ToId, ToName, e, IIf(Flag = FlagState.المستهلكات OrElse Flag = FlagState.مردودات_المستهلكات, False, True), IIf(Md.MyProjectType = ProjectType.X, False, True)) Then
                ToId_LostFocus(sender, Nothing)
            End If
        End If
    End Sub

    Function TestSalesAndReturn() As Boolean
        Return (Flag = FlagState.مبيعات_الصالة OrElse Flag = FlagState.مردودات_مبيعات_الصالة _
                OrElse Flag = FlagState.المبيعات OrElse Flag = FlagState.مردودات_المبيعات _
                OrElse Flag = FlagState.مبيعات_التوصيل OrElse Flag = FlagState.مردودات_مبيعات_التوصيل _
                OrElse Flag = FlagState.مبيعات_الجملة OrElse Flag = FlagState.مردودات_مبيعات_الجملة _
                OrElse Flag = FlagState.مبيعات_نصف_الجملة OrElse Flag = FlagState.مردودات_مبيعات_نصف_الجملة _
                OrElse Flag = FlagState.مبيعات_لمستثمر OrElse Flag = FlagState.مردودات_مبيعات_لمستثمر _
                OrElse Flag = FlagState.عرض_أسعار OrElse Flag = FlagState.أمر_توريد)
    End Function

    Function TestSalesOnly() As Boolean
        Return (Flag = FlagState.مبيعات_الصالة _
                OrElse Flag = FlagState.المبيعات _
                OrElse Flag = FlagState.مبيعات_التوصيل _
                OrElse Flag = FlagState.مبيعات_الجملة _
                OrElse Flag = FlagState.مبيعات_نصف_الجملة _
                OrElse Flag = FlagState.عرض_أسعار OrElse Flag = FlagState.أمر_توريد)
    End Function

    Function TestPurchaseAndReturn() As Boolean
        Return (Flag = FlagState.مشتريات OrElse Flag = FlagState.مردودات_مشتريات OrElse Flag = FlagState.مشتريات_خارجية OrElse Flag = FlagState.مردودات_مشتريات_خارجية OrElse Flag = FlagState.أمر_شراء OrElse Flag = FlagState.عرض_أسعار_مورد)
    End Function

    Function TestOuterPurchaseAndReturn() As Boolean
        Return (Flag = FlagState.مشتريات_خارجية OrElse Flag = FlagState.مردودات_مشتريات_خارجية)
    End Function

    Function TestImportAndReturn() As Boolean
        Return (Flag = FlagState.الاستيراد OrElse Flag = FlagState.مردودات_الاستيراد)
    End Function

    Function TestExportAndReturn() As Boolean
        Return (Flag = FlagState.التصدير OrElse Flag = FlagState.مردودات_التصدير)
    End Function

    Function TestInvestorsAndReturn() As Boolean
        Return (Flag = FlagState.مبيعات_لمستثمر OrElse Flag = FlagState.مردودات_مبيعات_لمستثمر)
    End Function

    Function TestInvestorsOnly() As Boolean
        Return (Flag = FlagState.مبيعات_لمستثمر)
    End Function

    Function TestConsumablesAndReturn() As Boolean
        Return (Flag = FlagState.المستهلكات OrElse Flag = FlagState.مردودات_المستهلكات OrElse Flag = FlagState.مستهلكات_الداخلي OrElse Flag = FlagState.مردودات_مستهلكات_الداخلي OrElse Flag = FlagState.مستهلكات_العمليات OrElse Flag = FlagState.مردودات_مستهلكات_العمليات)
    End Function

    Function TestPurchaseOnly() As Boolean
        Return (Flag = FlagState.مشتريات OrElse Flag = FlagState.مشتريات_خارجية OrElse Flag = FlagState.أمر_شراء OrElse Flag = FlagState.عرض_أسعار_مورد)
    End Function

    Function TestImportOnly() As Boolean
        Return (Flag = FlagState.الاستيراد)
    End Function

    Function TestExportOnly() As Boolean
        Return (Flag = FlagState.التصدير)
    End Function

    Function TestDelivary() As Boolean
        Return (Flag = FlagState.مبيعات_التوصيل OrElse Flag = FlagState.مردودات_مبيعات_التوصيل)
    End Function

    Private Sub ToId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToId.LostFocus
        'If GroupBoxPaymentType.Visibility = Visibility.Hidden Then Return
        Dim tbl As String
        Dim dt As DataTable
        If Flag = FlagState.تحويل_إلى_مخزن Then
            tbl = "Stores"
        ElseIf Md.ShowCostCenter AndAlso (Flag = FlagState.تسوية_إضافة OrElse Flag = FlagState.تسوية_صرف OrElse Flag = FlagState.صرف OrElse Flag = FlagState.إضافة) Then
            bm.CostCenterIdLostFocus(ToId, ToName, )
            Return
        ElseIf TestInvestorsAndReturn() Then
            tbl = "Investors"
        ElseIf ((TestPurchaseAndReturn() OrElse TestImportAndReturn()) AndAlso Not ReservToId.IsChecked) OrElse ((TestSalesAndReturn() OrElse TestExportAndReturn()) AndAlso ReservToId.IsChecked) Then
            tbl = "Suppliers"
        ElseIf ((TestSalesAndReturn() OrElse TestExportAndReturn()) AndAlso Not ReservToId.IsChecked) OrElse ((TestPurchaseAndReturn() OrElse TestImportAndReturn()) AndAlso ReservToId.IsChecked) Then
            tbl = "Customers"
        ElseIf TestConsumablesAndReturn() Then
            bm.LostFocus(ToId, ToName, "select " & Resources.Item("CboName") & " Name from Cases where Id=" & ToId.Text.Trim() & IIf(lop OrElse Flag = FlagState.المستهلكات OrElse Flag = FlagState.مردودات_المستهلكات, "", " and InOut=1"))
            ToId.ToolTip = ""
            ToName.ToolTip = ""
            dt = bm.ExecuteAdapter("select HomePhone,Mobile,Notes from Customers where Id=" & ToId.Text.Trim())
            If dt.Rows.Count > 0 Then
                ToId.ToolTip = Resources.Item("Id") & ": " & ToId.Text & vbCrLf & Resources.Item("CboName") & ": " & ToName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString & vbCrLf & Resources.Item("Notes") & ": " & dt.Rows(0)("Notes").ToString
                ToName.ToolTip = ToId.ToolTip
            End If
            Return
        Else
            Return
        End If
        bm.LostFocus(ToId, ToName, "select Name from " & tbl & " where Id=" & ToId.Text.Trim(), , , True)

        If Md.MyProjectType = ProjectType.X And TestSalesAndReturn() And Not lop Then
            CashierId.Text = bm.ExecuteScalar("select CashierId from Customers where Id=" & ToId.Text)
        End If
        CashierId_LostFocus(Nothing, Nothing)


        If ((TestSalesAndReturn() OrElse TestExportAndReturn()) AndAlso Not ReservToId.IsChecked) OrElse ((TestPurchaseAndReturn() OrElse TestImportAndReturn()) AndAlso ReservToId.IsChecked) Then
            Dim w As Integer = Val(bm.ExecuteScalar("select SellerId from " & tbl & " where Id=" & ToId.Text.Trim()))
            If w > 0 Then
                WaiterId.Text = w
                WaiterId_LostFocus(Nothing, Nothing)
            End If
        End If


        Dim s As String = ""
        dt = bm.ExecuteAdapter("GetCustomerData", New String() {"Id"}, New String() {Val(ToId.Text)})
        If dt.Rows.Count > 0 Then
            If Not lop Then DiscountPerc.Text = Val(dt.Rows(0)("DescPerc").ToString)
            For i As Integer = 0 To dt.Columns.Count - 2
                s &= dt.Rows(0)(i).ToString & IIf(i = dt.Columns.Count - 1, "", vbCrLf)
            Next
        End If
        ToId.ToolTip = s
        ToName.ToolTip = s


        If Md.MyProjectType = ProjectType.X AndAlso (TestSalesAndReturn() OrElse TestPurchaseAndReturn()) Then
            CalcBalSub()
        End If

        If lop Or Val(InvoiceNo.Text) > 0 Then Return

        If ((TestSalesAndReturn() OrElse TestExportAndReturn()) AndAlso Not ReservToId.IsChecked) OrElse ((TestPurchaseAndReturn()) AndAlso ReservToId.IsChecked) Then

            If InvoiceNo.Text.Trim = "" Then
                If bm.ExecuteScalar("select Type from Customers where Id=" & ToId.Text.Trim()) = "1" Then
                    RdoCash.IsChecked = True
                Else
                    RdoFuture.IsChecked = True
                End If
            End If


            If Md.ShowPriceLists AndAlso (TestSalesAndReturn() OrElse TestExportAndReturn()) Then
                PriceListId.SelectedValue = Val(bm.ExecuteScalar("select PriceListId from Customers where Id=" & ToId.Text.Trim()))
                PriceListId_SelectionChanged(Nothing, Nothing)
            End If

            dt = bm.ExecuteAdapter("GetCustomerData", New String() {"Id"}, New String() {Val(ToId.Text)})
            If dt.Rows.Count > 0 Then
                If Not lop Then DiscountPerc.Text = Val(dt.Rows(0)("DescPerc").ToString)
                s = ""
                For i As Integer = 0 To dt.Columns.Count - 2
                    s &= dt.Rows(0)(i).ToString & IIf(i = dt.Columns.Count - 1, "", vbCrLf)
                Next
            End If
        End If
        If TestImportAndReturn() OrElse TestExportAndReturn() OrElse (Md.MyProjectType = ProjectType.Zohor AndAlso TestPurchaseAndReturn()) Then
            RdoCash.IsChecked = True
            RdoFuture.IsChecked = True
        End If
        If CurrencyId.Visibility = Visibility.Visible Then
            CurrencyId.SelectedIndex = 0
            Dim x As Integer = Val(bm.ExecuteScalar("select CurrencyId from " & tbl & " where Id=" & ToId.Text.Trim()))
            If x > 0 Then
                CurrencyId.SelectedValue = x
            End If
        End If
        ToId.ToolTip = s
        ToName.ToolTip = s

    End Sub

    Private Sub WaiterId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles WaiterId.KeyUp
        bm.ShowHelp("البائعين", WaiterId, WaiterName, e, "select cast(Id as varchar(100)) Id,Name from Sellers")
    End Sub

    Private Sub WaiterId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles WaiterId.LostFocus
        bm.LostFocus(WaiterId, WaiterName, "select Name from Sellers where Id=" & WaiterId.Text.Trim())
    End Sub

    Private Sub DeliverymanId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DeliverymanId.KeyUp
        bm.ShowHelp("الطيارين", DeliverymanId, DeliverymanName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Deliveryman=1 and Stopped=0")
    End Sub

    Private Sub DeliverymanId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DeliverymanId.LostFocus
        bm.LostFocus(DeliverymanId, DeliverymanName, "select EnName Name from Employees where Deliveryman=1 and Id=" & DeliverymanId.Text.Trim() & " and Stopped=0")
    End Sub


    Private Sub CashierId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CashierId.KeyUp
        bm.ShowHelp("Employees", CashierId, CashierName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Stopped=0" & IIf(TestConsumablesAndReturn() AndAlso Md.MyProjectType = ProjectType.X, " and Nurse=1", " and Cashier=1"))
    End Sub

    Private Sub CashierId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CashierId.LostFocus
        bm.LostFocus(CashierId, CashierName, "select " & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Id=" & CashierId.Text.Trim() & " and Stopped=0 " & IIf(TestConsumablesAndReturn() AndAlso Md.MyProjectType = ProjectType.X, " and Nurse=1", " and Cashier=1"))
    End Sub


    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True


        UndoNewId()
        G.Rows.Clear()
        bm.FillControls(Me)

        LoadTree()

        PaymentType_TextChanged(Nothing, Nothing)
        ToId_LostFocus(Nothing, Nothing)
        SaveId_LostFocus(Nothing, Nothing)
        BankId_LostFocus(Nothing, Nothing)
        DoctorId_LostFocus(Nothing, Nothing)

        TId_LostFocus(TableId, Nothing)
        TId_LostFocus(TableSubId, Nothing)
        TId_LostFocus(NoOfPersons, Nothing)

        AccNo_LostFocus(Nothing, Nothing)
        AccNo1_LostFocus(Nothing, Nothing)
        AccNo2_LostFocus(Nothing, Nothing)
        AccNo3_LostFocus(Nothing, Nothing)
        AccNo4_LostFocus(Nothing, Nothing)

        CashierId_LostFocus(Nothing, Nothing)
        WaiterId_LostFocus(Nothing, Nothing)
        DeliverymanId_LostFocus(Nothing, Nothing)
        OrderTypeId_LostFocus(Nothing, Nothing)

        If Flag = FlagState.الاستيراد Then
            MessageId.Text = bm.ExecuteScalar("Select top 1 Id from ImportMessagesDetails where OrderTypeId='" & OrderTypeId.Text & "' and InvoiceNo='" & InvoiceNo.Text & "'")
        End If

        Dim dt As DataTable = bm.ExecuteAdapter("select SD.* ,It." & Resources.Item("CboName") & " It_Name,dbo.GetGroupName(It.GroupId)GroupName,dbo.GetTypeName(It.GroupId,It.TypeId)TypeName from SalesDetails SD left join Items It on(SD.ItemId=It.Id) where SD.StoreId=" & StoreId.Text & " and SD.InvoiceNo=" & InvoiceNo.Text & " and SD.Flag=" & Flag)

        If dt.Rows.Count > 0 Then G.Rows.Add(dt.Rows.Count)
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.SalesInvoiceNo).Value = dt.Rows(i)("SalesInvoiceNo").ToString
            G.Rows(i).Cells(GC.Barcode).Value = dt.Rows(i)("Barcode").ToString
            G.Rows(i).Cells(GC.ItemSerialNo).Value = dt.Rows(i)("ItemSerialNo").ToString
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("It_Name").ToString
            G.Rows(i).Cells(GC.ExpDate).Value = dt.Rows(i)(GC.ExpDate)
            G.Rows(i).Cells(GC.ExpireDate).Value = bm.ToStrDate(dt.Rows(i)(GC.ExpireDate)).ToString.Substring(0, 10)
            G.Rows(i).Cells(GC.GroupName).Value = dt.Rows(i)(GC.GroupName).ToString
            G.Rows(i).Cells(GC.TypeName).Value = dt.Rows(i)(GC.TypeName).ToString
            GetItemNameAndBal(i, dt.Rows(i)("ItemId").ToString, dt.Rows(i)("ItemSerialNo").ToString)
            LoadItemUint(i)
            G.Rows(i).Cells(GC.Color).Value = dt.Rows(i)("Color")
            G.Rows(i).Cells(GC.Size).Value = dt.Rows(i)("Size")
            G.Rows(i).Cells(GC.UnitId).Value = dt.Rows(i)("UnitId")
            G.Rows(i).Cells(GC.UnitQty).Value = dt.Rows(i)("UnitQty").ToString
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            G.Rows(i).Cells(GC.ConsumptionQty).Value = dt.Rows(i)("ConsumptionQty").ToString
            G.Rows(i).Cells(GC.ConsumptionRemainingQty).Value = dt.Rows(i)("ConsumptionRemainingQty").ToString

            G.Rows(i).Cells(GC.CurrentBal).Value += G.Rows(i).Cells(GC.Qty).Value

            G.Rows(i).Cells(GC.Qty2).Value = dt.Rows(i)("Qty2").ToString
            G.Rows(i).Cells(GC.Qty3).Value = dt.Rows(i)("Qty3").ToString
            G.Rows(i).Cells(GC.ReceivedQty).Value = dt.Rows(i)("ReceivedQty").ToString
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
            G.Rows(i).Cells(GC.UnitSub).Value = "" 'dt.Rows(i)("UnitSub").ToString
            G.Rows(i).Cells(GC.QtySub).Value = 0 ' 'dt.Rows(i)("QtySub").ToString
            G.Rows(i).Cells(GC.PriceSub).Value = dt.Rows(i)("PriceSub").ToString
            G.Rows(i).Cells(GC.ItemDiscountPerc).Value = dt.Rows(i)("ItemDiscountPerc").ToString
            G.Rows(i).Cells(GC.ItemDiscount).Value = dt.Rows(i)("ItemDiscount").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Rows(i).Cells(GC.IsPrinted).Value = dt.Rows(i)("IsPrinted").ToString
            G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(i)("SalesPrice").ToString
            G.Rows(i).Cells(GC.FlagType).Value = dt.Rows(i)("FlagType").ToString
            G.Rows(i).Cells(GC.SerialNo).Value = dt.Rows(i)("SerialNo").ToString
            G.Rows(i).Cells(GC.IsDelivered).Value = dt.Rows(i)("IsDelivered").ToString
            G.Rows(i).Cells(GC.SalesManId).Value = dt.Rows(i)("SalesManId").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            G.Rows(i).Cells(GC.ProductionOrderFlag).Value = dt.Rows(i)("ProductionOrderFlag").ToString
            G.Rows(i).Cells(GC.ProductionOrderNo).Value = dt.Rows(i)("ProductionOrderNo").ToString
            G.Rows(i).Cells(GC.SD_Notes).Value = dt.Rows(i)("SD_Notes").ToString
            G.Rows(i).Cells(GC.AvgCost).Value = dt.Rows(i)("AvgCost").ToString
            G.Rows(i).Cells(GC.SalesReturnStateId).Value = dt.Rows(i)("SalesReturnStateId").ToString
            G.Rows(i).Cells(GC.SalesReturnStateReason).Value = dt.Rows(i)("SalesReturnStateReason").ToString

            If Val(dt.Rows(i)("Qty").ToString) > 0 Then
                G.Rows(i).Cells(GC.AvgCostOne).Value = bm.Rnd(Val(dt.Rows(i)("AvgCost").ToString) / Val(dt.Rows(i)("Qty").ToString))
            Else
                G.Rows(i).Cells(GC.AvgCostOne).Value = 0
            End If

            G.Rows(i).Cells(GC.UnitsWeightId).Value = dt.Rows(i)("UnitsWeightId").ToString
            G.Rows(i).Cells(GC.UnitsWeightQty).Value = dt.Rows(i)("UnitsWeightQty").ToString
            G.Rows(i).Cells(GC.PreQty).Value = dt.Rows(i)("PreQty").ToString
            G.Rows(i).Cells(GC.SalesPriceTypeId).Value = dt.Rows(i)("SalesPriceTypeId").ToString
            'CalcRow(i)
            'If Not Md.Manager AndAlso TestSalesAndReturn() Then
            '    G.Rows(i).ReadOnly = True
            '    G.Rows(i).DefaultCellStyle.BackColor = System.Drawing.Color.PeachPuff
            '    btnDelete.IsEnabled = False
            'End If
        Next

        G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Name)
        CalcTotal()
        Notes.Focus()
        G.RefreshEdit()
        lop = False


        If Val(CaseInvoiceNo.Text) > 0 Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

        CalcTotalEnd()

        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {"Flag", MainId, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click, btnPrint2.Click, btnPrint3.Click, btnPrint4.Click, btnPrint5.Click, btnPrintImage.Click, btnReceiveSave.Click, btnPrintSafe.Click
        If Md.Manager OrElse Md.MyProjectType = ProjectType.X Then DontClear = True
        If Md.MyProjectType = ProjectType.X Then DontClear = False
        btnSave_Click(sender, e)
        DontClear = False
    End Sub

    Dim AllowSave As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False

        If StoreId.Text.Trim = "" Then Return
        If Not btnSave.IsEnabled Then GoTo Print


        If DayDate.SelectedDate Is Nothing Then
            bm.ShowMSG("برجاء تحديد التاريخ ")
            DayDate.Focus()
            Return
        End If

        If Not bm.TestDateValidity(DayDate) Then Return

        If SaveId.Visibility = Visibility.Visible AndAlso Val(SaveId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الخزنة")
            SaveId.Focus()
            Return
        End If
        If BankId.Visibility = Visibility.Visible AndAlso Val(BankId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد البنك")
            BankId.Focus()
            Return
        End If
        If AccNo.Visibility = Visibility.Visible AndAlso Val(AccNo.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الحساب ")
            AccNo.Focus()
            Return
        End If
        If ToId.Visibility = Visibility.Visible AndAlso ToId.Text.Trim = "" AndAlso Flag <> FlagState.أمر_شراء Then
            bm.ShowMSG("برجاء تحديد " & lblToId.Content)
            ToId.Focus()
            Return
        End If
        If OrderTypeId.Visibility = Visibility.Visible AndAlso OrderTypeId.Text.Trim = "" AndAlso TestImportAndReturn() Then
            bm.ShowMSG("برجاء تحديد " & lblOrderTypeId.Content)
            OrderTypeId.Focus()
            Return
        End If
        If TableId.Visibility = Visibility.Visible AndAlso TableId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد رقم المائدة")
            TableId.Focus()
            Return
        End If
        If TableSubId.Visibility = Visibility.Visible AndAlso TableSubId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد رقم الفرعى من المائدة")
            TableSubId.Focus()
            Return
        End If
        If NoOfPersons.Visibility = Visibility.Visible AndAlso NoOfPersons.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد عدد الأفراد")
            NoOfPersons.Focus()
            Return
        End If
        If CashierId.Visibility = Visibility.Visible AndAlso CashierId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد " & lblCashier.Content)
            CashierId.Focus()
            Return
        End If
        'If WaiterId.Visibility = Visibility.Visible AndAlso Val(WaiterId.Text) = 0 Then
        '    bm.ShowMSG("برجاء تحديد المندوب")
        '    WaiterId.Focus()
        '    Return
        'End If
        If DeliverymanId.Visibility = Visibility.Visible AndAlso DeliverymanId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد الطيار")
            DeliverymanId.Focus()
            Return
        End If
        If Flag = FlagState.تحويل_إلى_مخزن AndAlso ToId.Text.Trim = StoreId.Text Then
            bm.ShowMSG("لا يمكن التحويل لنفس المخزن")
            ToId.Focus()
            Return
        End If


        If AccNo1.Visibility = Visibility.Visible AndAlso AccNo1.Text.Trim = "" AndAlso Val(Val1.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب")
            AccNo1.Focus()
            Return
        End If
        If AccNo2.Visibility = Visibility.Visible AndAlso AccNo2.Text.Trim = "" AndAlso Val(Val2.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب")
            AccNo2.Focus()
            Return
        End If
        If AccNo3.Visibility = Visibility.Visible AndAlso AccNo3.Text.Trim = "" AndAlso Val(Val3.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب")
            AccNo3.Focus()
            Return
        End If
        If AccNo4.Visibility = Visibility.Visible AndAlso AccNo4.Text.Trim = "" AndAlso Val(Val4.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب")
            AccNo4.Focus()
            Return
        End If


        If Val(SubAccNo1.Text) = 0 AndAlso SubAccNo1.IsEnabled AndAlso Val(AccNo1.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo1.Focus()
            Return
        End If

        If Val(SubAccNo2.Text) = 0 AndAlso SubAccNo2.IsEnabled AndAlso Val(AccNo2.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo2.Focus()
            Return
        End If

        If Val(SubAccNo3.Text) = 0 AndAlso SubAccNo3.IsEnabled AndAlso Val(AccNo3.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo3.Focus()
            Return
        End If

        If Val(SubAccNo4.Text) = 0 AndAlso SubAccNo4.IsEnabled AndAlso Val(AccNo4.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo4.Focus()
            Return
        End If



        If AccType1.Visibility = Visibility.Visible AndAlso AccType1.SelectedIndex < 1 AndAlso Val(Val1.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد النوع")
            AccType1.Focus()
            Return
        End If
        If AccType2.Visibility = Visibility.Visible AndAlso AccType2.SelectedIndex < 1 AndAlso Val(Val2.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد النوع")
            AccType2.Focus()
            Return
        End If
        If AccType3.Visibility = Visibility.Visible AndAlso AccType3.SelectedIndex < 1 AndAlso Val(Val3.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد النوع")
            AccType3.Focus()
            Return
        End If
        If AccType4.Visibility = Visibility.Visible AndAlso AccType4.SelectedIndex < 1 AndAlso Val(Val4.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد النوع")
            AccType4.Focus()
            Return
        End If

        G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Name)
        G.EndEdit()
        Try
            CalcRow(G.CurrentRow.Index)
        Catch ex As Exception
        End Try


        Select Case Md.MyProjectType
            Case ProjectType.X
                If G.Columns(GC.SalesReturnStateId).Visible Then
                    For i As Integer = 0 To G.Rows.Count - 1
                        If Val(G.Rows(i).Cells(GC.Id).Value) = 0 Then Continue For
                        If Val(G.Rows(i).Cells(GC.SalesReturnStateId).Value) = 0 Then
                            bm.ShowMSG("برجاء تحديد الحالة بالسطر رقم " & (i + 1))
                            G.CurrentCell = G.Rows(i).Cells(GC.SalesReturnStateId)
                            Exit Sub
                        End If
                        If G.Rows(i).Cells(GC.SalesReturnStateReason).Value Is Nothing Then
                            G.Rows(i).Cells(GC.SalesReturnStateReason).Value = ""
                        End If
                        If Val(G.Rows(i).Cells(GC.SalesReturnStateId).Value) = 2 AndAlso G.Rows(i).Cells(GC.SalesReturnStateReason).Value.ToString.Trim = "" Then
                            bm.ShowMSG("برجاء تحديد سبب الحالة بالسطر رقم " & (i + 1))
                            G.CurrentCell = G.Rows(i).Cells(GC.SalesReturnStateReason)
                            Exit Sub
                        End If
                    Next
                End If
            Case Else
                Dim xx As Integer = bm.GridCountColumn(G, GC.Id)
                If xx = 0 Then Return

                'For i As Integer = 0 To G.Rows.Count - 1
                '    If Val(G.Rows(i).Cells(GC.Id).Value) > 0 Then
                '        Exit For
                '    End If
                '    If i = G.Rows.Count - 1 Then Return
                'Next
        End Select




        TableId.Text = Val(TableId.Text)
        TableSubId.Text = Val(TableSubId.Text)
        NoOfPersons.Text = Val(NoOfPersons.Text)
        MinPerPerson.Text = Val(MinPerPerson.Text)
        ServiceValue.Text = Val(ServiceValue.Text)
        Taxvalue.Text = Val(Taxvalue.Text)
        PaymentType.Text = Val(PaymentType.Text)
        CashValue.Text = Val(CashValue.Text)

        DiscountPerc.Text = Val(DiscountPerc.Text)
        DiscountValue.Text = Val(DiscountValue.Text)

        ToId.Text = Val(ToId.Text)
        WaiterId.Text = Val(WaiterId.Text)

        Per1.Text = Val(Per1.Text)
        Per2.Text = Val(Per2.Text)
        Per3.Text = Val(Per3.Text)
        Per4.Text = Val(Per4.Text)

        Val1.Text = Val(Val1.Text)
        Val2.Text = Val(Val2.Text)
        Val3.Text = Val(Val3.Text)
        Val4.Text = Val(Val4.Text)

        Shipping.Text = Val(Shipping.Text)
        Freight.Text = Val(Freight.Text)
        CustomClearance.Text = Val(CustomClearance.Text)

        'If Not Md.Manager Then
        'DayDate.SelectedDate = bm.MyGetDate() 'Md.CurrentDate
        'Shift.SelectedValue = Md.CurrentShiftId
        'End If
        If Md.ShowShifts AndAlso Not Md.Manager Then
            DayDate.SelectedDate = Md.CurrentDate
            Shift.SelectedValue = Md.CurrentShiftId
        End If

        If CurrencyId.SelectedValue < 1 Then
            CurrencyId.SelectedValue = 1
        End If


        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select isnull(max(" & SubId & "),0)+1 from " & TableName & " where " & MainId & "='" & StoreId.Text & "'" & " and Flag=" & Flag)
            If InvoiceNo.Text = "" Then Return 'InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text



            State = BasicMethods.SaveState.Insert
        End If

        MinPerPerson.Text = Val(MinPerPerson.Text)

        If Md.MyProjectType = ProjectType.X AndAlso DocNo.Text.Trim = "" Then
            DocNo.Text = Val(bm.ExecuteScalar("select dbo.GetSalesSerialNawar(" & StoreId.Text & "," & Flag & ")"))
        End If

        If btnSave.IsEnabled Then
            bm.DefineValues()
            If Not bm.Save(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text.Trim}, State) Then
                If State = BasicMethods.SaveState.Insert Then
                    InvoiceNo.Text = ""
                    lblLastEntry.Text = ""
                End If
                Return
            End If

            If Not bm.SaveGrid(G, "SalesDetails", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text}, New String() {"SalesInvoiceNo", "Barcode", "ItemSerialNo", "ItemId", "ItemName", "ExpDate", "ExpireDate", "Color", "Size", "UnitId", "UnitQty", "Qty", "Qty2", "Qty3", "ReceivedQty", "Price", "QtySub", "PriceSub", "ItemDiscountPerc", "ItemDiscount", "Value", "IsPrinted", "SalesPrice", "FlagType", "SerialNo", "IsDelivered", "SalesManId", "ProductionOrderFlag", "ProductionOrderNo", "SD_Notes", "UnitsWeightId", "UnitsWeightQty", "PreQty", "SalesPriceTypeId", "AvgCost", "SalesReturnStateId", "SalesReturnStateReason", "ConsumptionQty", "ConsumptionRemainingQty"}, New String() {GC.SalesInvoiceNo, GC.Barcode, GC.ItemSerialNo, GC.Id, GC.Name, GC.ExpDate, GC.ExpireDate, GC.Color, GC.Size, GC.UnitId, GC.UnitQty, GC.Qty, GC.Qty2, GC.Qty3, GC.ReceivedQty, GC.Price, GC.QtySub, GC.PriceSub, GC.ItemDiscountPerc, GC.ItemDiscount, GC.Value, GC.IsPrinted, GC.SalesPrice, GC.FlagType, GC.SerialNo, GC.IsDelivered, GC.SalesManId, GC.ProductionOrderFlag, GC.ProductionOrderNo, GC.SD_Notes, GC.UnitsWeightId, GC.UnitsWeightQty, GC.PreQty, GC.SalesPriceTypeId, GC.AvgCost, GC.SalesReturnStateId, GC.SalesReturnStateReason, GC.ConsumptionQty, GC.ConsumptionRemainingQty}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.String, VariantType.Integer, VariantType.String, VariantType.String, VariantType.Date, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.Decimal, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.Decimal, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Integer}, New String() {GC.Id}, "Line", "Line") Then Return

            If TestPurchaseOnly() Then 'State = BasicMethods.SaveState.Insert AndAlso
                bm.ExecuteNonQuery("UpdateItemPurchasePrice", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text})
            ElseIf TestImportOnly() Then 'State = BasicMethods.SaveState.Insert AndAlso
                bm.ExecuteNonQuery("UpdateItemImportPrice", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text})
            End If

            'If DocNo.Text.Trim = "" AndAlso State = BasicMethods.SaveState.Insert AndAlso Flag = FlagState.المستهلكات Then
            '    bm.ExecuteNonQuery("UpdateDocNo", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text})
            'End If

            If Md.MyProjectType = ProjectType.X AndAlso TestSalesAndReturn() Then
                bm.ExecuteNonQuery("UpdateCost", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text})
            End If

            bm.ExecuteNonQuery("UpdateSalesDetailsComponants", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text})

        End If


        If Md.MyProjectType = ProjectType.X AndAlso TestSalesAndReturn() Then
            SaveCustomerData()
        End If

Print:

        Select Case CType(sender, Button).Name
            Case btnPrint.Name, btnPrint2.Name, btnPrint3.Name, btnPrint4.Name, btnPrint5.Name, btnPrintSafe.Name
                State = BasicMethods.SaveState.Print
            Case btnCloseTable.Name
                State = BasicMethods.SaveState.Close
        End Select

        TraceInvoice(State.ToString)

        AllowSave = True
        'If TestSalesAndOnly() Then PrintPone(sender, 1)
        If sender Is btnPrint OrElse sender Is btnPrint2 OrElse sender Is btnPrint3 OrElse sender Is btnPrint4 OrElse sender Is btnPrint5 OrElse sender Is btnPrintImage OrElse sender Is btnPrintSafe OrElse (sender Is btnCloseTable And btnPrint.IsEnabled) Then
            PrintPone(sender, 0)
            If Md.MyProjectType = ProjectType.X AndAlso sender Is btnPrint AndAlso Flag = FlagState.المبيعات Then
                PrintPone(btnPrint3, 0)
            End If

            'txtID_Leave(Nothing, Nothing)
            '
            'Return
        End If

        If Md.WhatsAppLink <> "" AndAlso TestSalesOnly() Then
            'bm.SendWhatsApp(bm.ExecuteScalar("select Mobile from Customers where Id=" & ToId.Text), "صباحو يا عسل")
        End If

        If Md.MyProjectType = ProjectType.X Then
            CalcBalSub()
        End If

        If Not DontClear Then btnNew_Click(sender, e)

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {"Flag", MainId, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
        TableId.Focus()
    End Sub

    Dim IsClearing As Boolean = False
    Dim SalesSerialNoCount As Integer = 0
    Sub ClearControls()
        Try
            If looop Then Return
            IsClearing = True
            NewId()
            Dim d As DateTime = bm.MyGetDate
            Try
                d = DayDate.SelectedDate
            Catch ex As Exception
            End Try

            Dim s As String = 0
            Try
                s = Shift.SelectedValue.ToString()
            Catch ex As Exception
            End Try
            Dim st As String = StoreId.Text

            If Md.MyProjectType = ProjectType.X AndAlso TestSalesAndReturn() Then
                bm.ExecuteNonQuery("updateCustomersTempBal0", {"Id"}, {Val(ToId.Text)})
            End If

            bm.ClearControls(False)

            If CurrencyId.SelectedValue < 1 Then
                CurrencyId.SelectedValue = 1
            End If

            CalcBalSub()

            If SalesTypeId.Items.Count > 0 Then
                SalesTypeId.SelectedValue = 1
                If Md.MyProjectType = ProjectType.X AndAlso TestSalesOnly() Then
                    SalesTypeId.SelectedValue = 2
                End If
            End If

            LoadTree()

            CancelMinPerPerson.IsChecked = True

            SalesSerialNoCount = Val(bm.ExecuteScalar("Select top 1 SalesSerialNoCount from Statics"))
            Payed.Clear()
            Remaining.Clear()
            txtFlag.Text = Flag
            SaveId.Text = Md.DefaultSave
            BankId.Text = Md.DefaultBank
            If Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X Then CashierId.Text = Md.UserName

            Dim dt As DataTable = bm.ExecuteAdapter("select S_AccNo,R_S_AccNo,P_AccNo,R_P_AccNo,OP_AccNo,R_OP_AccNo from Statics")
            Select Case Flag
                Case FlagState.مشتريات
                    AccNo.Text = dt.Rows(0)("P_AccNo")
                Case FlagState.مردودات_مشتريات
                    AccNo.Text = dt.Rows(0)("R_P_AccNo")
                Case FlagState.مشتريات_خارجية
                    AccNo.Text = dt.Rows(0)("OP_AccNo")
                Case FlagState.مردودات_مشتريات_خارجية
                    AccNo.Text = dt.Rows(0)("R_OP_AccNo")
                Case FlagState.المبيعات, FlagState.مبيعات_التوصيل, FlagState.مبيعات_الصالة, FlagState.مبيعات_الجملة, FlagState.مبيعات_نصف_الجملة, FlagState.المستهلكات, FlagState.مستهلكات_الداخلي, FlagState.مستهلكات_العمليات, FlagState.التصدير, FlagState.مبيعات_لمستثمر, FlagState.مردودات_مبيعات_لمستثمر
                    AccNo.Text = dt.Rows(0)("S_AccNo")
                Case FlagState.مردودات_المبيعات, FlagState.مردودات_مبيعات_التوصيل, FlagState.مردودات_مبيعات_الصالة, FlagState.مردودات_مبيعات_الجملة, FlagState.مردودات_مبيعات_نصف_الجملة, FlagState.مردودات_المستهلكات, FlagState.مردودات_مستهلكات_الداخلي, FlagState.مردودات_مستهلكات_العمليات, FlagState.مردودات_التصدير
                    AccNo.Text = dt.Rows(0)("R_S_AccNo")
            End Select

            OrderTypeId_LostFocus(Nothing, Nothing)
            SaveId_LostFocus(Nothing, Nothing)
            BankId_LostFocus(Nothing, Nothing)
            DoctorId_LostFocus(Nothing, Nothing)


            CashierId_LostFocus(Nothing, Nothing)
            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                ToId.Text = 1
            End If
            ToId_LostFocus(Nothing, Nothing)
            WaiterId_LostFocus(Nothing, Nothing)
            AccNo_LostFocus(Nothing, Nothing)
            DeliverymanId_LostFocus(Nothing, Nothing)
            TId_LostFocus(TableId, Nothing)
            TId_LostFocus(TableSubId, Nothing)
            TId_LostFocus(NoOfPersons, Nothing)

            If TestPurchaseAndReturn() Then
                If Md.MyProjectType = ProjectType.X AndAlso TestOuterPurchaseAndReturn() Then
                Else

                    AccNo1.Text = StaticsDt.Rows(0)("P_AccNo1")
                    SubAccNo1.Text = StaticsDt.Rows(0)("P_SubAccNo1")
                    SubAccNo1_LostFocus(Nothing, Nothing)
                    Per1.Text = StaticsDt.Rows(0)("P_Per1")
                    AccType1.SelectedValue = StaticsDt.Rows(0)("P_AccType1")

                    AccNo2.Text = StaticsDt.Rows(0)("P_AccNo2")
                    SubAccNo2.Text = StaticsDt.Rows(0)("P_SubAccNo2")
                    SubAccNo2_LostFocus(Nothing, Nothing)
                    Per2.Text = StaticsDt.Rows(0)("P_Per2")
                    AccType2.SelectedValue = StaticsDt.Rows(0)("P_AccType2")

                    AccNo3.Text = StaticsDt.Rows(0)("P_AccNo3")
                    SubAccNo3.Text = StaticsDt.Rows(0)("P_SubAccNo3")
                    SubAccNo3_LostFocus(Nothing, Nothing)
                    Per3.Text = StaticsDt.Rows(0)("P_Per3")
                    AccType3.SelectedValue = StaticsDt.Rows(0)("P_AccType3")

                    AccNo4.Text = StaticsDt.Rows(0)("P_AccNo4")
                    SubAccNo4.Text = StaticsDt.Rows(0)("P_SubAccNo4")
                    SubAccNo4_LostFocus(Nothing, Nothing)
                    Per4.Text = StaticsDt.Rows(0)("P_Per4")
                    AccType4.SelectedValue = StaticsDt.Rows(0)("P_AccType4")

                End If
            ElseIf TestSalesAndReturn() Then

                AccNo1.Text = StaticsDt.Rows(0)("S_AccNo1")
                SubAccNo1.Text = StaticsDt.Rows(0)("S_SubAccNo1")
                SubAccNo1_LostFocus(Nothing, Nothing)
                Per1.Text = StaticsDt.Rows(0)("S_Per1")
                AccType1.SelectedValue = StaticsDt.Rows(0)("S_AccType1")

                AccNo2.Text = StaticsDt.Rows(0)("S_AccNo2")
                SubAccNo2.Text = StaticsDt.Rows(0)("S_SubAccNo2")
                SubAccNo2_LostFocus(Nothing, Nothing)
                Per2.Text = StaticsDt.Rows(0)("S_Per2")
                AccType2.SelectedValue = StaticsDt.Rows(0)("S_AccType2")

                AccNo3.Text = StaticsDt.Rows(0)("S_AccNo3")
                SubAccNo3_LostFocus(Nothing, Nothing)
                SubAccNo3.Text = StaticsDt.Rows(0)("S_SubAccNo3")
                Per3.Text = StaticsDt.Rows(0)("S_Per3")
                AccType3.SelectedValue = StaticsDt.Rows(0)("S_AccType3")

                AccNo4.Text = StaticsDt.Rows(0)("S_AccNo4")
                SubAccNo4.Text = StaticsDt.Rows(0)("S_SubAccNo4")
                SubAccNo4_LostFocus(Nothing, Nothing)
                Per4.Text = StaticsDt.Rows(0)("S_Per4")
                AccType4.SelectedValue = StaticsDt.Rows(0)("S_AccType4")

                'If Not Md.Manager Then
                '    AccNo1.IsEnabled = False
                '    AccNo2.IsEnabled = False
                '    AccNo3.IsEnabled = False
                '    AccNo4.IsEnabled = False
                'End If
            End If

            AccNo_LostFocus(Nothing, Nothing)

            AccNo1_LostFocus(Nothing, Nothing)
            AccNo2_LostFocus(Nothing, Nothing)
            AccNo3_LostFocus(Nothing, Nothing)
            AccNo4_LostFocus(Nothing, Nothing)


            If Md.MyProjectType = ProjectType.X AndAlso TestSalesOnly() Then
                Temp.Visibility = Visibility.Visible
                Temp.Content = "فاتورة حجز"
            ElseIf Md.MyProjectType = ProjectType.X AndAlso TestPurchaseOnly() Then
                Temp.Visibility = Visibility.Visible
                Temp.Content = "فاتورة معلقة"
                'If Not Md.Manager Then Temp.IsEnabled = False
            Else
                Temp.Visibility = Visibility.Visible
                Temp.Content = "ملغى"
            End If

            If Md.MyProjectType = ProjectType.X Then
                Temp.Visibility = Visibility.Hidden
            End If

            Temp2.Visibility = Visibility.Hidden
            Temp2.IsChecked = True
            If Md.MyProjectType = ProjectType.X And TestImportOnly() Then
                Temp2.Visibility = Visibility.Visible
                Temp2.IsChecked = False
            End If

            If (Md.Receptionist AndAlso Md.MyProjectType = ProjectType.X) OrElse (Not Md.Manager AndAlso Md.Nurse AndAlso Md.MyProjectType = ProjectType.Zohor) Then
                G.Columns(GC.Price).Visible = False
                G.Columns(GC.ItemDiscountPerc).Visible = False
                G.Columns(GC.ItemDiscount).Visible = False
                G.Columns(GC.Value).Visible = False
                'G.Columns(GC.SalesPrice).Visible = False
            End If

            PaymentType.Text = 1
            If TestPurchaseAndReturn() Then
                PaymentType.Text = 4
            End If

            If Not Md.Manager Then
                DayDate.SelectedDate = bm.MyGetDate() 'Md.CurrentDate
                Shift.SelectedValue = Md.CurrentShiftId
                If Md.ShowShifts Then
                    DayDate.SelectedDate = Md.CurrentDate
                    Shift.SelectedValue = Md.CurrentShiftId
                End If

                If Not Md.MyProjectType = ProjectType.X Then CashierId.Text = Md.UserName
                CashierId_LostFocus(Nothing, Nothing)
            Else
                DayDate.SelectedDate = d
                Shift.SelectedValue = s
                DayDate.SelectedDate = bm.MyGetDate()
                Shift.SelectedValue = Md.CurrentShiftId
            End If

            'DayDate.SelectedDate = bm.MyGetDate()
            'Shift.SelectedValue = Md.CurrentShiftId

            StoreId.Text = st

            txtFlag.Text = Flag

            G.Rows.Clear()
            G.Focus()
            G_SelectionChanged(Nothing, Nothing)
            'InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & StoreId.Text & "'" & " and Flag=" & Flag)
            'If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"

            If TableSubId.Visibility = Visibility.Visible Then TableSubId.Text = 1
            If NoOfPersons.Visibility = Visibility.Visible Then NoOfPersons.Text = 1

            WithService.IsChecked = (WithService.Visibility = Visibility.Visible)
            WithTax.IsChecked = (WithTax.Visibility = Visibility.Visible)
        Catch
        End Try
        If Flag = FlagState.مبيعات_الصالة Then TabControl1.SelectedItem = TabItemTables


        If TestSalesAndReturn() AndAlso Md.MyProjectType = ProjectType.X Then
            DeliveryDate.SelectedDate = DayDate.SelectedDate
        End If


        'If Md.MyProjectType = ProjectType.X AndAlso TestPurchaseOnly() AndAlso Not Md.Manager Then Temp.IsChecked = True

        'btnSave.IsEnabled = True
        'btnDelete.IsEnabled = True
        IsClearing = False
        CalcTotal()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If bm.IF_Exists("select * from InstallmentInvoicesMaster" & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag) Then
                If Not bm.ShowDeleteMSG("سيتم مسح الأقساط .. استمرار؟") Then
                    Return
                End If
            End If


            TraceInvoice("Deleted")
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)

            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)




            bm.ExecuteNonQuery("delete from InstallmentInvoicesMaster where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)

            bm.ExecuteNonQuery("delete from InstallmentInvoicesDateils where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)

            bm.ExecuteNonQuery("delete from InstallmentInvoicesDateilsPayments where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)



            btnNew_Click(sender, e)
        End If
    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExecuteNonQuery("BeforeDeleteSales", New String() {"Flag", "StoreId", "InvoiceNo", "UserDelete", "State"}, New String() {Flag, StoreId.Text, InvoiceNo.Text, Md.UserName, State})
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub InvoiceNo_KeyUp(sender As Object, e As KeyEventArgs) Handles InvoiceNo.KeyUp
        If TestImportAndReturn() Then
            If bm.ShowHelpMultiColumns("Header", InvoiceNo, InvoiceNo, e, "select cast(InvoiceNo as nvarchar(100))'رقم الفاتورة',cast(ToId as nvarchar(100))'كود المورد',dbo.GetSupplierName(ToId)'اسم المورد',dbo.GetOrderTypes(OrderTypeId)'رقم الطلبية',DocNo'رقم عقد المورد',dbo.ToStrDate(DayDate)'التاريخ',DocNo 'رقم المستند' from SalesMaster where StoreId=" & Val(StoreId.Text) & " and Flag=" & Flag.ToString) Then
                'InvoiceNo.Text = bm.SelectedRow(0)
                InvoiceNo_Leave(Nothing, Nothing)
            End If
        ElseIf TestPurchaseAndReturn() Then
            If bm.ShowHelpMultiColumns("Header", InvoiceNo, InvoiceNo, e, "select cast(InvoiceNo as nvarchar(100))'رقم الفاتورة',cast(ToId as nvarchar(100))'كود المورد',dbo.GetSupplierName(ToId)'رقم عقد المورد',dbo.ToStrDate(DayDate)'التاريخ',DocNo 'رقم المستند' from SalesMaster where StoreId=" & Val(StoreId.Text) & " and Flag=" & Flag.ToString) Then
                'InvoiceNo.Text = bm.SelectedRow(0)
                InvoiceNo_Leave(Nothing, Nothing)
            End If
        ElseIf TestSalesAndReturn() OrElse TestExportAndReturn() Then
            If bm.ShowHelpMultiColumns(CType(Parent, Page).Title, InvoiceNo, InvoiceNo, e, "select cast(InvoiceNo as nvarchar(100))'رقم الفاتورة',cast(ToId as nvarchar(100))'كود العميل',dbo.GetCustomerName(ToId)'اسم العميل',dbo.ToStrDate(DayDate)'التاريخ',DocNo 'رقم المستند' from SalesMaster where StoreId=" & Val(StoreId.Text) & " and Flag=" & Flag.ToString) Then
                'InvoiceNo.Text = bm.SelectedRow(0)
                InvoiceNo_Leave(Nothing, Nothing)
            End If
        End If

    End Sub
    Public Sub InvoiceNo_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyDown, InvoiceNo.KeyDown, ToId.KeyDown, WaiterId.KeyDown, TableId.KeyDown, TableSubId.KeyDown, NoOfPersons.KeyDown, txtID.KeyDown, CashierId.KeyDown, DeliverymanId.KeyDown, AccNo1.KeyDown, AccNo2.KeyDown, AccNo3.KeyDown, AccNo4.KeyDown, SubAccNo1.KeyDown, SubAccNo2.KeyDown, SubAccNo3.KeyDown, SubAccNo4.KeyDown, OrderTypeId.KeyDown, PurchaseOrder.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Taxvalue.KeyDown, ServiceValue.KeyDown, MinPerPerson.KeyDown, CashValue.KeyDown, DiscountPerc.KeyDown, DiscountValue.KeyDown, txtPrice.KeyDown, Per1.KeyDown, Per2.KeyDown, Per3.KeyDown, Per4.KeyDown, Val1.KeyDown, Val2.KeyDown, Val3.KeyDown, Val4.KeyDown, Shipping.KeyDown, Freight.KeyDown, CustomClearance.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub


    'Private Sub MyBase_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    '    If Not btnSave.Enabled Then Exit Sub
    '    Select Case bm.RequestDelete
    '        Case BasicMethods.CloseState.Yes
    '            
    '            btnSave_Click(Nothing, Nothing)
    '            If Not AllowClose Then e.Cancel = True
    '        Case BasicMethods.CloseState.No

    '        Case BasicMethods.CloseState.Cancel
    '            e.Cancel = True
    '    End Select
    'End Sub

    Private Sub PaymentType_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles PaymentType.TextChanged
        Try
            If PaymentType.Text = 1 Then
                RdoCash.IsChecked = True
            ElseIf PaymentType.Text = 2 Then
                RdoVisa.IsChecked = True
            ElseIf PaymentType.Text = 3 Then
                RdoCashVisa.IsChecked = True
            ElseIf PaymentType.Text = 4 Then
                RdoFuture.IsChecked = True
            ElseIf PaymentType.Text = 5 Then
                RdoCashFuture.IsChecked = True
            ElseIf PaymentType.Text = 6 Then
                RdoEmployees.IsChecked = True
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub TableId_KeyUp(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles TableId.KeyUp
        If bm.ShowHelp("الموائد", TableId, TableIdName, e, "select cast(Id as varchar(100)) Id,Name from Tables where StoreId='" & StoreId.Text & "'") Then
            TId_LostFocus(TableId, Nothing)
        End If
    End Sub



    Private Sub TId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TableId.LostFocus, TableSubId.LostFocus, NoOfPersons.LostFocus

        If CType(sender, TextBox).Text.Trim = "" Or CType(sender, TextBox).Text.Trim = "0" Then CType(sender, TextBox).Clear()

        If sender Is TableId AndAlso CType(sender, TextBox).Visibility = Visibility.Visible Then
            bm.LostFocus(TableId, TableIdName, "select Name from Tables where StoreId='" & StoreId.Text & "' and Id=" & TableId.Text.Trim())
            TestDoublicatinInTables(False)
        ElseIf sender Is TableSubId AndAlso CType(sender, TextBox).Visibility = Visibility.Visible Then
            Dim x As Integer = Val(bm.ExecuteScalar("select MaxSubTable from Statics"))
            If (x < Val(TableSubId.Text)) Then
                bm.ShowMSG("الحد الأقصى للفرعى هو " & x)
                TableSubId.Clear()
            End If
            TestDoublicatinInTables(True)
        End If
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
                CalcTotal()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Function PriceFieldName(ByVal str As String, i As Integer, Optional ForceSales As Boolean = False) As String
        If ForceSales OrElse TestSalesAndReturn() OrElse TestConsumablesAndReturn() OrElse TestExportAndReturn() Then
            str = "Sales" & str
        ElseIf TestImportAndReturn() Then
            str = "Import" & str
        Else
            str = "Purchase" & str
        End If

        Select Case i
            Case 1
                Return str & "Sub"
            Case 2
                Return str & "Sub2"
            Case Else
                Return str
        End Select
    End Function

    Function UnitCount(dt As DataTable, i As Integer) As String
        Select Case i
            Case 1
                Return dt.Rows(0)("UnitCount")
            Case 2
                Return dt.Rows(0)("UnitCount2")
            Case Else
                Return 1
        End Select
    End Function

    Private Sub PrintPone(ByVal sender As System.Object, ByVal NewItemsOnly As Integer)

        If Not bm.ExecuteNonQuery("UpdatePrintUser", New String() {"Flag", "StoreId", "InvoiceNo", "PrintUser"}, New String() {Flag, StoreId.Text, InvoiceNo.Text, Md.UserName}) Then Return

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Shift", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "@InvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "Header", "Remaining", "Payed", "Bal"}

        Dim MyBal As Decimal = 0
        Dim tbl As String = IIf(TestSalesAndReturn, IIf(ReservToId.IsChecked, "Suppliers", "Customers"), IIf(ReservToId.IsChecked, "Customers", "Suppliers"))
        If TestSalesAndReturn() OrElse TestPurchaseAndReturn() Then
            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X
                MyBal = Val(bm.ExecuteScalar("select dbo.Bal0(AccNo,Id,'" & bm.ToStrDate(DayDate.SelectedDate) & "',0,0,0) from " & tbl & " where Id=" & ToId.Text))
        End Select
        End If

        If NewItemsOnly = 0 Then
            rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, Val(Shift.SelectedValue), Flag, StoreId.Text, InvoiceNo.Text, InvoiceNo.Text, InvoiceNo.Text, NewItemsOnly, 0, 0, 0, CType(Parent, Page).Title, Val(Remaining.Text), Val(Payed.Text), MyBal}

            Select Case Md.MyProjectType
                Case ProjectType.X
                    rpt.Rpt = "SalesPone_N.rpt"
                    If Flag = FlagState.إضافة OrElse Flag = FlagState.صرف Then
                        rpt.Rpt = "SalesPone_N2.rpt"
                    ElseIf TestSalesAndReturn() Then
                        rpt.Rpt = "SalesPone_N3.rpt"
                    ElseIf Flag = FlagState.تحويل_إلى_مخزن Then
                        rpt.Rpt = "SalesPone_N4.rpt"
                    End If
                Case ProjectType.X
                    rpt.Rpt = "SalesPone_H.rpt"
                Case Else
                    rpt.Rpt = "SalesPone.rpt"
                    If Md.MyProjectType = ProjectType.X Then
                        rpt.Rpt = "SalesPone_P.rpt"
                        If Flag = FlagState.التصدير OrElse Flag = FlagState.مردودات_التصدير Then
                            rpt.Rpt = "SalesPone_P2.rpt"
                        End If
                    ElseIf Md.MyProjectType = ProjectType.X Then
                        rpt.Rpt = "SalesPone_O.rpt"
                    ElseIf Md.MyProjectType = ProjectType.Zohor Then
                        rpt.Rpt = "SalesPone_Z.rpt"
                    End If
            End Select

            'If sender Is btnPrint2 Then rpt.Rpt = "SalesPone2.rpt"
            If sender Is btnPrint3 Then
                rpt.Rpt = "SalesPone3.rpt"
                If Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "SalesPone3_N.rpt"
                    If G.Columns(GC.SalesReturnStateId).Visible Then
                        rpt.Rpt = "SalesPone3_N2.rpt"
                    End If
                    If Flag = FlagState.تحويل_إلى_مخزن Then
                        rpt.Rpt = "SalesPone3_N3.rpt"
                    End If
                End If
            End If

            If sender Is btnPrint3 AndAlso Md.MyProjectType = ProjectType.X Then rpt.Rpt = "SalesPone33.rpt"
            If sender Is btnPrint3 AndAlso Md.MyProjectType = ProjectType.X Then rpt.Rpt = "SalesPone3_H.rpt"
            If sender Is btnPrint3 AndAlso Md.MyProjectType = ProjectType.X Then rpt.Rpt = "SalesPone3_O.rpt"

            If sender Is btnPrint4 Then rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "SalesPone4_N.rpt", "SalesPone4.rpt")
            If sender Is btnPrintSafe Then rpt.Rpt = "SalesPonePrintSafe.rpt"

            If sender Is btnPrint2 Then
                Dim i As Integer = 1
                rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "SalesPone2_N.rpt", "SalesPone2.rpt")
                rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "SalesPone2_AZ.rpt", rpt.Rpt)

                'Return it for Toson
                'rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "SalesPone_N_A5.rpt", rpt.Rpt)

                If Md.MyProjectType = ProjectType.X Then
                    'rpt.Rpt = "SalesPone2.rpt"
                    i = 2
                End If
                If Md.MyProjectType = ProjectType.X Then
                    If sender Is btnPrint Then
                        rpt.ShowDialog()
                    Else
                        rpt.Show()
                    End If
                Else
                    rpt.Print(".", Md.PonePrinter, i)
                End If
                Return
            End If

            If sender Is btnPrint5 Then
                rpt.Rpt = "PrintBarcodeFromSalesDetails.rpt"
                If Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "PrintBarcodeFromSalesDetailsSizeColor.rpt"
                ElseIf Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "PrintBarcodeFromSalesDetails_AZ.rpt"
                End If

                If Md.MyProjectType = ProjectType.X Then
                    If sender Is btnPrint Then
                        rpt.ShowDialog()
                    Else
                        rpt.Show()
                    End If
                ElseIf Md.MyProjectType = ProjectType.X Then
                    If bm.ShowDeleteMSG("هل تريد طباعة المقاس الكبير؟") Then
                        rpt.Rpt = "PrintBarcodeFromSalesDetailsS2.rpt"
                    Else
                        rpt.Rpt = "PrintBarcodeFromSalesDetailsS1.rpt"
                    End If
                    rpt.Print(".", Md.BarcodePrinter, 1)
                Else
                    rpt.Print(".", Md.BarcodePrinter, 1)
                End If
                Return
            End If

            If sender Is btnPrintImage Then
                rpt.Rpt = "SalesPone_N_Image.rpt"
            End If

            If sender Is btnPrintSafe Then
                rpt.Print(".", Md.PonePrinter, 1)
                Return
            End If



            If Md.MyProjectType = ProjectType.X AndAlso sender Is btnPrint Then
                rpt.ShowDialog()
            Else
                rpt.Show()
            End If

        ElseIf Not TestSalesAndReturn() Then
            rpt.Rpt = "SalesPoneKitchen.rpt"
            For i As Integer = 0 To G.Rows.Count - 1
                Try
                    If G.Rows(i).Cells(GC.IsPrinted).Value.ToString = 0 Then
                        Dim dt As DataTable = bm.ExecuteAdapter("GetPrinters", New String() {"Shift", "Flag", "StoreId", "InvoiceNo"}, New String() {Shift.SelectedValue.ToString, Flag, StoreId.Text, InvoiceNo.Text})
                        For x As Integer = 0 To dt.Rows.Count - 1
                            rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, Shift.SelectedValue.ToString, Flag, StoreId.Text, InvoiceNo.Text, InvoiceNo.Text, NewItemsOnly, 0, 0, dt.Rows(x)("PrintingGroupId"), 0}

                            If Md.MyProjectType = ProjectType.X Then
                                rpt.Show()
                            Else
                                rpt.Print(dt.Rows(x)("ServerName"), dt.Rows(x)("PrinterName"))
                            End If
                        Next
                        Exit For
                    End If
                Catch
                End Try
            Next
        End If

    End Sub


    Private Sub RdoGrouping_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoGrouping.Checked, RdoSearch.Checked
        Try
            If RdoGrouping.IsChecked Then
                txtID.Visibility = Visibility.Hidden
                txtName.Visibility = Visibility.Hidden
                txtPrice.Visibility = Visibility.Hidden
                HelpGD.Visibility = Visibility.Hidden
                PanelGroups.Visibility = Visibility.Visible
                PanelTypes.Visibility = Visibility.Visible
                PanelItems.Visibility = Visibility.Visible
            ElseIf RdoSearch.IsChecked Then
                txtID.Visibility = Visibility.Visible
                txtName.Visibility = Visibility.Visible
                txtPrice.Visibility = Visibility.Visible
                HelpGD.Visibility = Visibility.Visible
                PanelGroups.Visibility = Visibility.Hidden
                PanelTypes.Visibility = Visibility.Hidden
                PanelItems.Visibility = Visibility.Hidden
            End If
            If (Md.Receptionist AndAlso Md.MyProjectType = ProjectType.X) OrElse (Not Md.Manager AndAlso Md.Nurse AndAlso Md.MyProjectType = ProjectType.Zohor) Then
                txtPrice.Visibility = Visibility.Hidden
            End If
        Catch
        End Try
    End Sub


    Dim LopCalc As Boolean = False
    Private Sub CalcTotal() Handles DiscountPerc.TextChanged, DiscountValue.TextChanged, Taxvalue.TextChanged, ServiceValue.TextChanged, MinPerPerson.TextChanged, NoOfPersons.TextChanged, WithTax.Checked, WithTax.Unchecked, WithService.Checked, WithService.Unchecked, CancelMinPerPerson.Checked, CancelMinPerPerson.Unchecked, ToId.LostFocus
        If LopCalc OrElse lop OrElse IsClearing Then Return
        Try
            LopCalc = True
            MinPerPerson.Text = Math.Round(0, 2)
            Total.Text = Math.Round(0, 2)
            TotalCount.Text = Math.Round(0, 2)
            Taxvalue.Text = Math.Round(0, 2)
            ServiceValue.Text = Math.Round(0, 2)

            If CancelMinPerPerson.IsChecked Then
                MinPerPerson.Text = Math.Round(0, 2)
            Else
                MinPerPerson.Text = Val(bm.ExecuteScalar("select dbo.GetMinValuePerPerson(" & StoreId.Text & ")"))
            End If



            'For i As Integer = 0 To G.Rows.Count - 1
            '    Total.Text += Val(G.Rows(i).Cells(GC.Value).Value)
            '    TotalCount.Text += Val(G.Rows(i).Cells(GC.Qty).Value)
            'Next

            Total.Text = bm.GridSumColumn(G, GC.Value)
            TotalCount.Text = bm.GridSumColumn(G, GC.Qty)


            If Val(DiscountPerc.Text) > 0 Then
                'DiscountValue.Text = Math.Round(Val(Total.Text) * Val(DiscountPerc.Text) / 100, 2)
                DiscountValue.Text = Val(Total.Text) * Val(DiscountPerc.Text) / 100
            End If


            If Not lop Or Not IsClosed.IsChecked Then

                If Val(Total.Text) < Val(MinPerPerson.Text) * Val(NoOfPersons.Text) Then
                    'Total.Text = Math.Round(Val(MinPerPerson.Text) * Val(NoOfPersons.Text), 2)
                    Total.Text = Val(MinPerPerson.Text) * Val(NoOfPersons.Text)
                End If

                If Val(DiscountPerc.Text) > 0 Then
                    'DiscountValue.Text = Math.Round(Val(Total.Text) * Val(DiscountPerc.Text) / 100, 2)
                    DiscountValue.Text = Val(Total.Text) * Val(DiscountPerc.Text) / 100
                End If


                If WithTax.IsChecked Then
                    Taxvalue.Text = 0.01 * (Val(Total.Text) - Val(DiscountValue.Text)) * Val(bm.ExecuteScalar("select dbo.GetTaxPerc(" & StoreId.Text & ")"))
                Else
                    Taxvalue.Text = Math.Round(0, 2)
                End If
                If WithService.IsChecked Then
                    If TestDelivary() Then
                        ServiceValue.Text = Val(bm.ExecuteScalar("select dbo.GetDelivaryCost(" & Val(StoreId.Text) & "," & Val(ToId.Text) & ")"))
                    Else
                        'ServiceValue.Text = Math.Round((Val(Total.Text) - Val(DiscountValue.Text)) * Val(bm.ExecuteScalar("select dbo.GetServicePerc(" & StoreId.Text & ")")) / 100, 2)
                        ServiceValue.Text = (Val(Total.Text) - Val(DiscountValue.Text)) * Val(bm.ExecuteScalar("select dbo.GetServicePerc(" & StoreId.Text & ")")) / 100
                    End If
                Else
                    ServiceValue.Text = Math.Round(0, 2)
                End If

            End If

            LopCalc = False
            CalcTotalEnd()
        Catch ex As Exception
        End Try
    End Sub

    Sub CalcTotalEnd() Handles Per1.TextChanged, Per2.TextChanged, Per3.TextChanged, Per4.TextChanged, Val1.LostFocus, Val2.LostFocus, Val3.LostFocus, Val4.LostFocus, AccType1.SelectionChanged, AccType2.SelectionChanged, AccType3.SelectionChanged, AccType4.SelectionChanged, Shipping.LostFocus, Freight.LostFocus, CustomClearance.LostFocus
        'Val1.Text = Math.Round(Val(Total.Text) * Val(Per1.Text) / 100, 2)
        'Val2.Text = Math.Round(Val(Total.Text) * Val(Per2.Text) / 100, 2)
        'Val3.Text = Math.Round(Val(Total.Text) * Val(Per3.Text) / 100, 2)
        'Val4.Text = Math.Round(Val(Total.Text) * Val(Per4.Text) / 100, 2)

        Val1.IsEnabled = Val(Per1.Text) = 0
        Val2.IsEnabled = Val(Per2.Text) = 0
        Val3.IsEnabled = Val(Per3.Text) = 0
        Val4.IsEnabled = Val(Per4.Text) = 0

        If Val(Per1.Text) <> 0 Then Val1.Text = (Val(Total.Text) - Val(DiscountValue.Text)) * Val(Per1.Text) / 100
        If Val(Per2.Text) <> 0 Then Val2.Text = (Val(Total.Text) - Val(DiscountValue.Text)) * Val(Per2.Text) / 100
        If Val(Per3.Text) <> 0 Then Val3.Text = (Val(Total.Text) - Val(DiscountValue.Text)) * Val(Per3.Text) / 100
        If Val(Per4.Text) <> 0 Then Val4.Text = (Val(Total.Text) - Val(DiscountValue.Text)) * Val(Per4.Text) / 100

        If Md.MyProjectType.X Then
            Val1.Text = Math.Round(Val(Val1.Text), 2, MidpointRounding.AwayFromZero)
            Val2.Text = Math.Round(Val(Val2.Text), 2, MidpointRounding.AwayFromZero)
            Val3.Text = Math.Round(Val(Val3.Text), 2, MidpointRounding.AwayFromZero)
            Val4.Text = Math.Round(Val(Val4.Text), 2, MidpointRounding.AwayFromZero)
        End If

        If Md.MyProjectType.X And TestSalesAndReturn() Then
            If (Val(Total.Text) - Val(DiscountValue.Text)) <= 300 Then
                Val2.Clear()
            End If
        End If


        Dim d1 As Decimal = Val(Val1.Text)
        Dim d2 As Decimal = Val(Val2.Text)
        Dim d3 As Decimal = Val(Val3.Text)
        Dim d4 As Decimal = Val(Val4.Text)

        If AccType1.SelectedValue = 1 Then d1 *= -1
        If AccType2.SelectedValue = 1 Then d2 *= -1
        If AccType3.SelectedValue = 1 Then d3 *= -1
        If AccType4.SelectedValue = 1 Then d4 *= -1

        If AccType1.SelectedIndex < 1 Then d1 = 0
        If AccType2.SelectedIndex < 1 Then d2 = 0
        If AccType3.SelectedIndex < 1 Then d3 = 0
        If AccType4.SelectedIndex < 1 Then d4 = 0

        'TotalAfterDiscount.Text = Math.Round(Val(Total.Text) - Val(DiscountValue.Text) + Val(Taxvalue.Text) + Val(ServiceValue.Text) + d1 + d2 + d3 + d4, 2)
        TotalAfterDiscount.Text = Val(Total.Text) - Val(DiscountValue.Text) + Val(Taxvalue.Text) + Val(ServiceValue.Text) + d1 + d2 + d3 + d4 + Val(Shipping.Text) + Val(Freight.Text) + Val(CustomClearance.Text)


    End Sub

    Dim DontClear As Boolean = False
    Private Sub btnCloseTable_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCloseTable.Click
        If btnPrint.IsEnabled Then

            DontClear = True
            btnSave_Click(btnCloseTable, e)
            DontClear = False

        End If
        'If Not bm.ExecuteNonQuery("update SalesMaster set IsClosed=1,ClosedDate=getdate(),DayDate='" & bm.ToStrDate(Md.CurrentDate) & "',Shift=" & Md.CurrentShiftId & " where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and InvoiceNo=" & InvoiceNo.Text) Then Return
        If Not bm.ExecuteNonQuery("update SalesMaster set IsClosed=1,ClosedDate=getdate(),DayDate='" & bm.ToStrDate(bm.MyGetDate()) & "',Shift=" & Md.CurrentShiftId & " where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and InvoiceNo=" & InvoiceNo.Text) Then Return
        btnNew_Click(sender, e)
    End Sub

    Private Sub IsClosed_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles IsClosed.Checked, IsClosed.Unchecked
        btnCloseTable.IsEnabled = Not IsClosed.IsChecked
    End Sub


    Private Sub IsCashierPrinted_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles IsCashierPrinted.Checked, IsCashierPrinted.Unchecked
        'btnSave.IsEnabled = Not (IsCashierPrinted.IsChecked And Not Md.Manager)
        'btnPrint.IsEnabled = Not (IsCashierPrinted.IsChecked And Not Md.Manager)
        'btnDelete.IsEnabled = Not (IsCashierPrinted.IsChecked And Not Md.Manager)
    End Sub

    Private Sub btnTableClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        If ChkSplite.IsChecked Then
            LoadSubTables(x.Tag)
        Else
            GetTable(x.Tag, 1)
        End If
    End Sub

    Sub GetTable(ByVal MainTable As Integer, ByVal SubTable As Integer)
        InvoiceNo.Text = bm.ExecuteScalar("select InvoiceNo from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and TableId=" & MainTable & " and TableSubId=" & SubTable & " and IsClosed=0")
        InvoiceNo_Leave(Nothing, Nothing)
        TableId.Text = MainTable
        TableSubId.Text = SubTable
        TId_LostFocus(TableId, Nothing)
        TabControl1.SelectedItem = TabItem1
    End Sub


    Private Sub TabControl1_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles TabControl1.SelectionChanged
        If e.AddedItems.Count = 0 Then Return
        If e.AddedItems(0) Is TabItemTables Then
            LoadTables()
        ElseIf e.AddedItems(0) Is TabItemDelivery Then
            LoadUnPaiedInvoices()
        End If
    End Sub

    Private Sub btnDeliveryClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        InvoiceNo.Text = x.Tag
        InvoiceNo_Leave(Nothing, Nothing)
        TId_LostFocus(TableId, Nothing)
        TabControl1.SelectedItem = TabItem1
    End Sub

    Private Sub TestDoublicatinInTables(ByVal msg As Boolean)
        If TableId.Text.Trim = "" Or IsClosed.IsChecked Then Return
        Dim s As String = bm.ExecuteScalar("select InvoiceNo from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and TableId=" & TableId.Text & " and TableSubId=" & TableSubId.Text & " and IsClosed=0")
        If s <> "" AndAlso s <> InvoiceNo.Text Then
            If msg Then bm.ShowMSG("هذه المائدة مفتوحة بمسلسل " & s)
            TableSubId.Clear()
        End If
    End Sub

    Private Sub ChkSplite_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ChkSplite.Checked
        SpliteScrollViewer.Visibility = Visibility.Visible
        WSubTables.Children.Clear()
    End Sub
    Private Sub ChkSplite_UnChecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ChkSplite.Unchecked
        SpliteScrollViewer.Visibility = Visibility.Hidden
        WSubTables.Children.Clear()
    End Sub

    Private Sub LoadSubTables(ByVal MyTag As Integer)
        WSubTables.Children.Clear()
        Dim z As Integer = Val(bm.ExecuteScalar("select top 1 MaxSubTable from Statics"))
        Dim dtInv As DataTable = bm.ExecuteAdapter("select InvoiceNo,TableId,TableSubId,dbo.ToStrTime(OpennedDate) OpennedTime,NoOfPersons,IsCashierPrinted from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and IsClosed=0")
        For i As Integer = 1 To z
            Try
                Dim x As New Button
                x.Name = "SubTable_" & i
                x.Tag = MyTag
                x.Width = 50
                x.Height = 50
                x.Cursor = Input.Cursors.Pen
                x.Content = i
                WSubTables.Children.Add(x)

                If dtInv.Select("TableId=" & x.Tag & " and TableSubId=" & i).Length > 0 Then
                    If dtInv.Select("TableId=" & x.Tag & " and TableSubId=" & i)(0)("IsCashierPrinted") = 1 Then
                        x.Background = System.Windows.Media.Brushes.Magenta
                    Else
                        x.Background = System.Windows.Media.Brushes.Red
                    End If
                Else
                    x.Background = System.Windows.Media.Brushes.LimeGreen
                End If

                AddHandler x.Click, AddressOf btnSubTableClick
            Catch
            End Try
        Next

    End Sub

    Private Sub btnSubTableClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = CType(sender, Button)
        GetTable(x.Tag, x.Name.Replace("SubTable_", ""))
    End Sub


    Dim Lop3 As Boolean = Focusable
    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        e.Handled = True
        If Receive Then Return
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                Lop3 = True
                G.Rows.Add()
                Lop3 = False
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.SalesInvoiceNo).Index Then
                If e.KeyCode = Forms.Keys.F1 AndAlso OrderTypeId.Visibility = Visibility.Visible AndAlso Val(OrderTypeId.Text) = 0 Then
                    bm.ShowMSG("برجاء تحديد الطلبية")
                    OrderTypeId.Focus()
                    Return
                End If

                'If bm.ShowHelpGrid("Invoices", G.CurrentRow.Cells(GC.SalesInvoiceNo), G.CurrentRow.Cells(GC.SalesInvoiceNo), e, "select distinct M.InvoiceNo,dbo.ToStrDate(M.DayDate) DayDate from SalesMaster M left join SalesDetails D on(M.Flag=D.Flag and M.StoreId=D.StoreId and M.InvoiceNo=D.InvoiceNo) where M.StoreId=" & StoreId.Text & " and M.Flag=" & MainFlag() & " and M.ToId=" & Val(ToId.Text) & " and M.OrderTypeId=" & Val(OrderTypeId.Text) & " and (D.ItemId=" & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value) & " or " & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value) & "=0)", , "الفاتورة", "التاريخ") Then
                '    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SalesInvoiceNo).Index, G.CurrentCell.RowIndex))
                '    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id)
                'End If

                If bm.ShowHelpGridMultiColumns("Invoices", G.CurrentRow.Cells(GC.SalesInvoiceNo), G.CurrentRow.Cells(GC.SalesInvoiceNo), e, "select distinct M.InvoiceNo 'الفاتورة',dbo.ToStrDate(M.DayDate) 'التاريخ',D.ItemId 'كود الصنف',D.ItemName 'اسم الصنف',D.Qty 'الكمية',D.Price -D.ItemDiscount 'السعر بعد الخصم',D.Value 'القيمة' from SalesMaster M left join SalesDetails D on(M.Flag=D.Flag and M.StoreId=D.StoreId and M.InvoiceNo=D.InvoiceNo) where M.Temp=0 and M.StoreId=" & StoreId.Text & " and M.Flag=" & MainFlag() & " and (M.ToId=" & Val(ToId.Text) & " or " & Val(ToId.Text) & "=0) and (M.OrderTypeId=" & Val(OrderTypeId.Text) & " or " & Val(OrderTypeId.Text) & "=0) and (D.ItemId=" & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value) & " or " & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value) & "=0)") Then
                    'GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SalesInvoiceNo).Index, G.CurrentCell.RowIndex))
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Name)

                    G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value = bm.SelectedRow(2)
                    G.Rows(G.CurrentCell.RowIndex).Cells(GC.Name).Value = bm.SelectedRow(3)
                    G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Value = bm.SelectedRow(4)
                    G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty2).Value = 1
                    G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty3).Value = bm.SelectedRow(4)
                    G.Rows(G.CurrentCell.RowIndex).Cells(GC.Price).Value = bm.SelectedRow(5)
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))

                End If

            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Name).Index Then
                Dim str As String = "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name," & PriceFieldName(GC.Price, 0) & " 'السعر' from Items where IsStopped=0 " & ItemWhere()
                If Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.SalesInvoiceNo).Value) > 0 Then
                    str = "select cast(ItemId as varchar(100)) Id,ItemName Name,Price-ItemDiscount 'السعر' from SalesDetails where StoreId=" & StoreId.Text & " and Flag=" & MainFlag() & " and InvoiceNo=" & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.SalesInvoiceNo).Value)
                    If Flag = FlagState.تحويل_إلى_مخزن Then
                        str = "select cast(D.ItemId as varchar(100)) Id,D.ItemName Name,SRS.Name 'الحالة' from SalesDetails D left join SalesReturnStates SRS on(D.SalesReturnStateId=SRS.Id) where D.StoreId=" & StoreId.Text & " and D.Flag=" & MainFlag() & " and D.InvoiceNo=" & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.SalesInvoiceNo).Value)
                    End If
                End If
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, str) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId)
                    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color)
                    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                    End If

                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.ExpDate).Index Then
                Dim str As String = "select ExpDate,cast(isnull(sum(Qty),0) as nvarchar(100))Qty from dbo.Fn_AllItemMotion(" & Val(StoreId.Text) & "," & G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value.ToString & ",'" & bm.ToStrDate(DayDate.SelectedDate) & "') Group by ExpDate having sum(Qty)>0"
                If bm.ShowHelpGrid("الرصيد", G.CurrentRow.Cells(GC.ExpDate), G.CurrentRow.Cells(GC.ExpDate), e, str,, "الصلاحية", "الرصيد") Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                End If
            End If


            If bm.ShowHelpGridItemBal(G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "GetItemCurrentBal " & Val(G.CurrentRow.Cells(GC.Id).Value)) Then
                GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                If G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId).Visible Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId)
                ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color).Visible Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color)
                ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")


    End Sub

    Private Sub HideAcc()
        Tab1.Visibility = Visibility.Collapsed
        GroupBoxPaymentType.Visibility = Visibility.Hidden

        If Md.MyProjectType = ProjectType.X Then
            PanelItems.Margin = New Thickness(PanelItems.Margin.Left, PanelItems.Margin.Top, PanelItems.Margin.Right, 8)
            HelpGD.Margin = New Thickness(HelpGD.Margin.Left, HelpGD.Margin.Top, HelpGD.Margin.Right, 8)
            PanelAcc.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub HideAcc2()
        Tab2.Visibility = Visibility.Collapsed
    End Sub

    Private Sub HideAcc3()
        Tab3.Visibility = Visibility.Collapsed
    End Sub

    Private Sub HideAcc4()
        Tab4.Visibility = Visibility.Collapsed
    End Sub

    Private Sub HideAcc5()
        Tab5.Visibility = Visibility.Collapsed
    End Sub

    Private Sub AccNo1_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo1.KeyUp
        If bm.AccNoShowHelp(AccNo1, AccName1, e, , , ) Then
            AccNo1_LostFocus(Nothing, Nothing)
        End If
    End Sub
    Private Sub AccNo2_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo2.KeyUp
        If bm.AccNoShowHelp(AccNo2, AccName2, e, , , ) Then
            AccNo2_LostFocus(Nothing, Nothing)
        End If
    End Sub
    Private Sub AccNo3_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo3.KeyUp
        If bm.AccNoShowHelp(AccNo3, AccName3, e, , , ) Then
            AccNo3_LostFocus(Nothing, Nothing)
        End If
    End Sub
    Private Sub AccNo4_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo4.KeyUp
        If bm.AccNoShowHelp(AccNo4, AccName4, e, , , ) Then
            AccNo4_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub AccNo1_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo1.LostFocus
        bm.AccNoLostFocus(AccNo1, AccName1, , , )

        SubAccNo1.IsEnabled = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & AccNo1.Text & "')").Rows.Count > 0
        SubAccNo1_LostFocus(Nothing, Nothing)
        If SubAccNo1.IsEnabled Then
            SubAccNo1.Focus()
        End If
    End Sub
    Private Sub AccNo2_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo2.LostFocus
        bm.AccNoLostFocus(AccNo2, AccName2, , , )

        SubAccNo2.IsEnabled = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & AccNo2.Text & "')").Rows.Count > 0
        SubAccNo2_LostFocus(Nothing, Nothing)
        If SubAccNo2.IsEnabled Then
            SubAccNo2.Focus()
        End If
    End Sub
    Private Sub AccNo3_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo3.LostFocus
        bm.AccNoLostFocus(AccNo3, AccName3, , , )

        SubAccNo3.IsEnabled = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & AccNo3.Text & "')").Rows.Count > 0
        SubAccNo3_LostFocus(Nothing, Nothing)
        If SubAccNo3.IsEnabled Then
            SubAccNo3.Focus()
        End If
    End Sub
    Private Sub AccNo4_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo4.LostFocus
        bm.AccNoLostFocus(AccNo4, AccName4, , , )

        SubAccNo4.IsEnabled = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & AccNo4.Text & "')").Rows.Count > 0
        SubAccNo4_LostFocus(Nothing, Nothing)
        If SubAccNo4.IsEnabled Then
            SubAccNo4.Focus()
        End If
    End Sub





    Private Sub SubAccNo1_KeyUp(sender As Object, e As KeyEventArgs) Handles SubAccNo1.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & AccNo1.Text & "')")
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo1, AccName1, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & AccNo1.Text & "'") Then
            SubAccNo1_LostFocus(Nothing, Nothing)
        End If
    End Sub
    Private Sub SubAccNo2_KeyUp(sender As Object, e As KeyEventArgs) Handles SubAccNo2.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & AccNo2.Text & "')")
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo2, AccName2, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & AccNo2.Text & "'") Then
            SubAccNo2_LostFocus(Nothing, Nothing)
        End If
    End Sub
    Private Sub SubAccNo3_KeyUp(sender As Object, e As KeyEventArgs) Handles SubAccNo3.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & AccNo3.Text & "')")
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo3, AccName3, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & AccNo3.Text & "'") Then
            SubAccNo3_LostFocus(Nothing, Nothing)
        End If
    End Sub
    Private Sub SubAccNo4_KeyUp(sender As Object, e As KeyEventArgs) Handles SubAccNo4.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & AccNo4.Text & "')")
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo4, AccName4, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & AccNo4.Text & "'") Then
            SubAccNo4_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SubAccNo1_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo1.LostFocus
        If Val(AccNo1.Text) > 0 AndAlso Val(SubAccNo1.Text) > 0 Then
            bm.SubAccNoLostFocus(AccNo1, SubAccNo1, AccName1)
        End If
    End Sub
    Private Sub SubAccNo2_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo2.LostFocus
        If Val(AccNo2.Text) > 0 AndAlso Val(SubAccNo2.Text) > 0 Then
            bm.SubAccNoLostFocus(AccNo2, SubAccNo2, AccName2)
        End If
    End Sub
    Private Sub SubAccNo3_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo3.LostFocus
        If Val(AccNo3.Text) > 0 AndAlso Val(SubAccNo3.Text) > 0 Then
            bm.SubAccNoLostFocus(AccNo3, SubAccNo3, AccName3)
        End If
    End Sub
    Private Sub SubAccNo4_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo4.LostFocus
        If Val(AccNo4.Text) > 0 AndAlso Val(SubAccNo4.Text) > 0 Then
            bm.SubAccNoLostFocus(AccNo4, SubAccNo4, AccName4)
        End If
    End Sub



    Private Sub LoadItemUint(i As Integer)
        Dim Id As Integer = Val(G.Rows(i).Cells(GC.Id).Value)
        'Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items where Id='" & Id & "' and " & ItemWhere() & "")

        If G.Columns(GC.UnitId).Visible Then bm.FillCombo("select 0 Id,Unit Name From Items where Id='" & Id & "' " & ItemWhere() & " union select 1 Id,UnitSub Name From Items where Id='" & Id & "' " & ItemWhere() & " union select 2 Id,UnitSub2 Name From Items where Id='" & Id & "' " & ItemWhere() & "", G.Rows(i).Cells(GC.UnitId))

        If G.Columns(GC.Color).Visible Then bm.FillCombo("select 0 Id,'-' Name union select Id,Name from ColorsDetails where ColorId=(select It.ColorId from Items It where It.Id='" & Id & "' " & ItemWhere() & ") order by Id", G.Rows(i).Cells(GC.Size))

        If G.Columns(GC.UnitId).Visible Then bm.FillCombo("select 0 Id,'-' Name union select Id,Name from SizesDetails where SizeId=(select It.SizeId from Items It where It.Id='" & Id & "' " & ItemWhere() & ") order by Id", G.Rows(i).Cells(GC.Size))


        If G.Rows(i).Cells(GC.UnitId).Value Is Nothing Then
            If Md.ShowQtySub Then
                G.Rows(i).Cells(GC.UnitId).Value = StoreUnitId
            Else
                G.Rows(i).Cells(GC.UnitId).Value = 0
            End If
        End If
        If G.Rows(i).Cells(GC.Color).Value Is Nothing Then G.Rows(i).Cells(GC.Color).Value = 0
        If G.Rows(i).Cells(GC.Size).Value Is Nothing Then G.Rows(i).Cells(GC.Size).Value = 0

        'If TestConsumablesAndReturn() Then
        '    'G.Rows(i).Cells(GC.UnitId).Value = 2
        'End If

    End Sub

    Private Sub LoadItemPrice(i As Integer)
        Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items_View where Id='" & G.Rows(i).Cells(GC.Id).Value & "' " & ItemWhere())

        Dim PLdt As DataTable
        If Val(PriceListId.SelectedValue) > 0 Then
            PLdt = bm.ExecuteAdapter("Select * From ItemPriceLists where PriceListId=" & Val(PriceListId.SelectedValue) & " and ItemId='" & G.Rows(i).Cells(GC.Id).Value & "' ")
        End If

        If dt.Rows.Count = 0 Then Return
        If LoadPriceList OrElse Val(G.Rows(i).Cells(GC.Price).Value) = 0 OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.UnitId).Index Then
            If Not PLdt Is Nothing AndAlso PLdt.Rows.Count > 0 Then
                G.Rows(i).Cells(GC.Price).Value = PLdt.Rows(0)(PriceFieldName(GC.Price, G.Rows(i).Cells(GC.UnitId).Value))
            Else
                G.Rows(i).Cells(GC.Price).Value = dt.Rows(0)(PriceFieldName(GC.Price, G.Rows(i).Cells(GC.UnitId).Value))
            End If
        End If
        G.Rows(i).Cells(GC.UnitQty).Value = UnitCount(dt, G.Rows(i).Cells(GC.UnitId).Value)

        If Not PLdt Is Nothing AndAlso PLdt.Rows.Count > 0 Then
            G.Rows(i).Cells(GC.SalesPrice).Value = PLdt.Rows(0)(PriceFieldName(GC.Price, G.Rows(i).Cells(GC.UnitId).Value, True))
        Else
            G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(0)(PriceFieldName(GC.Price, G.Rows(i).Cells(GC.UnitId).Value, True))
        End If
        If dt.Rows(0)("AllowEditSalesPrice") = 1 Then
            G.Rows(i).Cells(GC.Price).ReadOnly = False
        Else
            G.Rows(i).Cells(GC.Price).ReadOnly = ReadOnlyState()
        End If

        If TestSalesAndReturn() AndAlso Val(G.Rows(i).Cells(GC.Size).Value) > 0 Then
            dt = bm.ExecuteAdapter("Select * From ItemSizes where ItemId='" & G.Rows(i).Cells(GC.Id).Value & "' and Id='" & G.Rows(i).Cells(GC.Size).Value & "'")
            If dt.Rows.Count = 0 Then Return
            If Val(G.Rows(i).Cells(GC.Price).Value) = 0 Then
                G.Rows(i).Cells(GC.Price).Value = dt.Rows(0)("SalesPrice")
            End If
        End If
        If TestPurchaseOnly() AndAlso Val(G.Rows(i).Cells(GC.Size).Value) > 0 Then
            dt = bm.ExecuteAdapter("Select * From ItemSizes where ItemId='" & G.Rows(i).Cells(GC.Id).Value & "' and Id='" & G.Rows(i).Cells(GC.Size).Value & "'")
            If dt.Rows.Count = 0 Then Return
            If Val(G.Rows(i).Cells(GC.Price).Value) = 0 Then
                G.Rows(i).Cells(GC.Price).Value = dt.Rows(0)("PurchasePrice")
            End If
            G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(0)("SalesPrice")
        End If

        If Md.MyProjectType = ProjectType.X Then
            If TestSalesAndReturn() Then
                dt = bm.ExecuteAdapter("GetOccasionDisc", New String() {"ItemId"}, New String() {G.Rows(i).Cells(GC.Id).Value})
                If dt.Rows.Count > 0 Then
                    G.Rows(i).Cells(GC.Price).Value = Val(G.Rows(i).Cells(GC.Price).Value) - Val(dt.Rows(0)("Discvalue"))
                    G.Rows(i).Cells(GC.Price).Value -= Val(G.Rows(i).Cells(GC.Price).Value) * Val(dt.Rows(0)("DiscPerc")) / 100
                End If
            End If
        End If

        If Md.MyProjectType = ProjectType.X Then
            If Flag = FlagState.إضافة OrElse Flag = FlagState.تسوية_إضافة Then
                G.Rows(i).Cells(GC.Price).Value = Val(bm.ExecuteScalar("select dbo.GetItemLastInPrice('" & G.Rows(i).Cells(GC.Id).Value & "','" & bm.ToStrDate(DayDate.SelectedDate) & "')"))
            End If
        End If

        If Val(G.Rows(i).Cells(GC.SalesInvoiceNo).Value) > 0 Then
            dt = bm.ExecuteAdapter("select Price-ItemDiscount from SalesDetails where StoreId=" & StoreId.Text & " and Flag=" & MainFlag() & " and InvoiceNo=" & G.Rows(i).Cells(GC.SalesInvoiceNo).Value & " and ItemId=" & G.Rows(i).Cells(GC.Id).Value)
            If dt.Rows.Count = 0 Then
                bm.ShowMSG("هذا الصنف غير موجود بالفاتورة")
                ClearRow(i)
                Return
            End If
            Dim x As Decimal = Val(dt.Rows(0)(0))
            If x > 0 Then G.Rows(i).Cells(GC.Price).Value = x
        End If

    End Sub


    Private Sub ComboBox1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ComboBox1.SelectionChanged
        If ComboBox1.SelectedIndex = -1 Then Return
        Flag = ComboBox1.SelectedValue
        txtFlag.Text = ComboBox1.SelectedValue
        CType(Parent, Page).Title = CType(ComboBox1.ItemsSource, DataView).Item(ComboBox1.SelectedIndex)("Name")
        Md.Currentpage = CType(ComboBox1.ItemsSource, DataView).Item(ComboBox1.SelectedIndex)("Name")
        LoadVisibility()
        btnNew_Click(Nothing, Nothing)
    End Sub

    Dim looop As Boolean = False
    Private Sub LoadCbo()
        If ComboBox1.Visibility = Visibility.Hidden Then Return

        looop = True
        Dim FF = Flag
        Dim dt As New DataTable("tbl")
        dt.Columns.Add("Id")
        dt.Columns.Add("Name")
        Select Case Flag
            Case 1, 2, 3, 4, 5, 6, 7, 8
                dt.Rows.Add(New String() {1, "أرصدة افتتاحية"})
                dt.Rows.Add(New String() {2, "إضافة"})
                dt.Rows.Add(New String() {3, "تسوية إضافة"})
                dt.Rows.Add(New String() {4, "صرف"})
                dt.Rows.Add(New String() {5, "تسوية صرف"})
                dt.Rows.Add(New String() {6, "هدايا"})
                dt.Rows.Add(New String() {7, "هالك"})
                dt.Rows.Add(New String() {8, "تحويل إلى مخزن"})
            Case 9, 10, 29, 30
                dt.Rows.Add(New String() {9, "مشتريات"})
                dt.Rows.Add(New String() {10, "مردودات مشتريات"})
                dt.Rows.Add(New String() {29, "مشتريات خارجية"})
                dt.Rows.Add(New String() {30, "مردودات مشتريات خارجية"})
            Case 11, 12, 13, 14, 15, 16
                'dt.Rows.Add(New String() {11, "مبيعات الصالة"})
                'dt.Rows.Add(New String() {12, "مردودات مبيعات الصالة"})
                'dt.Rows.Add(New String() {13, "مبيعات التيك أواى"})
                'dt.Rows.Add(New String() {14, "مردودات مبيعات التيك أواى"})
                'dt.Rows.Add(New String() {15, "مبيعات التوصيل"})
                'dt.Rows.Add(New String() {16, "مردودات مبيعات التوصيل"})
                'IsClosedOnly.Visibility = Visibility.Visible
                dt.Rows.Add(New String() {13, "المبيعات"})
                dt.Rows.Add(New String() {14, "مردودات المبيعات"})
        End Select

        Dim dv As New DataView
        dv.Table = dt
        dv.Sort = "Id"
        ComboBox1.ItemsSource = dv
        ComboBox1.SelectedValuePath = "Id"
        ComboBox1.DisplayMemberPath = "Name"
        ComboBox1.SelectedIndex = 0

        ComboBox1.SelectedValue = FF
        ComboBox1_SelectionChanged(Nothing, Nothing)
        looop = False
    End Sub


    Private Sub Payed_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Payed.TextChanged, TotalAfterDiscount.TextChanged
        Remaining.Clear()
        If Val(Payed.Text) = 0 Then Return
        'Remaining.Text = Val(Payed.Text) - IIf(Val(CashValue.Text) > 0, Val(CashValue.Text), Val(TotalAfterDiscount.Text))
        Remaining.Text = Val(TotalAfterDiscount.Text) - Val(Payed.Text)
    End Sub

    Private Sub LoadVisibility()
        'Dim x As Integer = AccNo.Margin.Top
        'lblOrderTypeId.Margin = New Thickness(lblOrderTypeId.Margin.Left, x, lblOrderTypeId.Margin.Right, lblOrderTypeId.Margin.Bottom)
        'OrderTypeId.Margin = New Thickness(OrderTypeId.Margin.Left, x, OrderTypeId.Margin.Right, OrderTypeId.Margin.Bottom)
        'OrderTypeName.Margin = New Thickness(OrderTypeName.Margin.Left, x, OrderTypeName.Margin.Right, OrderTypeName.Margin.Bottom)

        'lblVersionNo.Margin = New Thickness(lblVersionNo.Margin.Left, x, lblVersionNo.Margin.Right, lblVersionNo.Margin.Bottom)
        'VersionNo.Margin = New Thickness(VersionNo.Margin.Left, x, VersionNo.Margin.Right, VersionNo.Margin.Bottom)

        'lblDeliveryDate.Margin = New Thickness(lblDeliveryDate.Margin.Left, x, lblDeliveryDate.Margin.Right, lblDeliveryDate.Margin.Bottom)
        'DeliveryDate.Margin = New Thickness(DeliveryDate.Margin.Left, x, DeliveryDate.Margin.Right, DeliveryDate.Margin.Bottom)

        btnImportFromExcel.Visibility = Visibility.Hidden

        lblOrderTypeId.Visibility = Visibility.Hidden
        OrderTypeId.Visibility = Visibility.Hidden
        OrderTypeName.Visibility = Visibility.Hidden

        btnDelete.Visibility = Visibility.Visible
        btnFirst.Visibility = Visibility.Visible
        btnLast.Visibility = Visibility.Visible
        btnNext.Visibility = Visibility.Visible
        btnPrevios.Visibility = Visibility.Visible
        btnPrint.Visibility = Visibility.Visible
        btnPrint3.Visibility = Visibility.Visible
        btnPrint4.Visibility = Visibility.Visible
        btnPrint5.Visibility = Visibility.Visible
        CashierId.Visibility = Visibility.Hidden
        CashierName.Visibility = Visibility.Hidden
        lblComboBox1.Visibility = Visibility.Hidden
        ComboBox1.Visibility = Visibility.Hidden
        lblSalesTypeId.Visibility = Visibility.Hidden
        SalesTypeId.Visibility = Visibility.Hidden
        DayDate.Visibility = Visibility.Visible
        DiscountPerc.Visibility = Visibility.Visible
        DiscountValue.Visibility = Visibility.Visible
        GroupBoxPaymentType.Visibility = Visibility.Visible
        lblCashier.Visibility = Visibility.Hidden
        lblDayDate.Visibility = Visibility.Visible
        lblDiscount.Visibility = Visibility.Visible
        lblDiscount_Copy.Visibility = Visibility.Visible
        lblDiscount_Copy1.Visibility = Visibility.Visible
        DocNo.Visibility = Visibility.Visible
        lblDocNo.Visibility = Visibility.Visible
        lblLE.Visibility = Visibility.Visible
        lblPayed.Visibility = Visibility.Visible
        lblPerc.Visibility = Visibility.Visible
        lblRemaining.Visibility = Visibility.Visible
        ReservToId.Visibility = Visibility.Hidden
        Payed.Visibility = Visibility.Visible
        Remaining.Visibility = Visibility.Visible
        lblToId.Visibility = Visibility.Hidden
        ToId.Visibility = Visibility.Hidden
        ToName.Visibility = Visibility.Hidden
        WP.Visibility = Visibility.Visible
        Tab2.Visibility = Visibility.Collapsed
        Tab3.Visibility = Visibility.Collapsed
        'Tab4.Visibility = Visibility.Collapsed
        Tab5.Visibility = Visibility.Collapsed

        lblDeliveryDate.Visibility = Visibility.Hidden
        DeliveryDate.Visibility = Visibility.Hidden
        lblVersionNo.Visibility = Visibility.Hidden
        VersionNo.Visibility = Visibility.Hidden
        lblMessageId.Visibility = Visibility.Hidden
        MessageId.Visibility = Visibility.Hidden
        btnAddCustomer.Visibility = Visibility.Hidden


        If Md.MyProjectType = ProjectType.X AndAlso (TestSalesAndReturn() OrElse TestPurchaseAndReturn()) Then
            Tab6.Visibility = Visibility.Visible
            Tab6.IsSelected = True
        Else
            Tab6.Visibility = Visibility.Hidden
        End If


        'Temp.IsEnabled = Md.Manager

        PriceListId.Visibility = Visibility.Hidden
        If Md.ShowPriceLists AndAlso (TestSalesAndReturn() OrElse TestExportAndReturn()) Then
            PriceListId.Visibility = Visibility.Visible
        End If

        If Md.MyProjectType = ProjectType.X AndAlso TestSalesAndReturn() Then
            btnAddCustomer.Visibility = Visibility.Visible
        End If

        btnPrint3.Content = "طباعة كميات"
        Select Case Flag
            Case FlagState.المبيعات, FlagState.الاستيراد, FlagState.مردودات_الاستيراد, FlagState.عرض_أسعار, FlagState.أمر_توريد, FlagState.أمر_شراء
                btnPrintImage.Visibility = Visibility.Visible
            Case Else
                btnPrintImage.Visibility = Visibility.Hidden
        End Select


        If Flag = FlagState.أرصدة_افتتاحية Then
            'lblDayDate.Visibility = Visibility.Hidden
            'DayDate.Visibility = Visibility.Hidden
            lblShift.Visibility = Visibility.Hidden
            Shift.Visibility = Visibility.Hidden

            lblDocNo.Visibility = Visibility.Hidden
            DocNo.Visibility = Visibility.Hidden


            'ElseIf Md.ShowCostCenter AndAlso (Flag = FlagState.تسوية_إضافة OrElse Flag = FlagState.تسوية_صرف OrElse Flag = FlagState.صرف OrElse Flag = FlagState.إضافة) Then

            'lblToId.Visibility = Visibility.Visible
            'ToId.Visibility = Visibility.Visible
            'ToName.Visibility = Visibility.Visible
            'lblToId.Content = "مركز التكلفة"


        ElseIf Flag = FlagState.تحويل_إلى_مخزن Then
            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible
            lblCashier.Content = "المستلم"

            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "المخزن المحول إليه"

            lblDocNo.Visibility = Visibility.Hidden
            DocNo.Visibility = Visibility.Hidden

            lblTotal.Visibility = Visibility.Hidden
            Total.Visibility = Visibility.Hidden

            btnPrint3.Content = "إذن صرف"
        End If

        If TestPurchaseAndReturn() Then

            If Flag <> FlagState.أمر_شراء Then
                lblCashier.Visibility = Visibility.Visible
                CashierId.Visibility = Visibility.Visible
                CashierName.Visibility = Visibility.Visible
                lblCashier.Content = "الطالب"

                GroupBoxPaymentType.Visibility = Visibility.Visible
                If Not Md.MyProjectType = ProjectType.X Then
                    lblSaveId.Visibility = Visibility.Visible
                    SaveId.Visibility = Visibility.Visible
                    SaveName.Visibility = Visibility.Visible
                End If
            Else
                lblCashier.Visibility = Visibility.Hidden
                CashierId.Visibility = Visibility.Hidden
                CashierName.Visibility = Visibility.Hidden

                GroupBoxPaymentType.Visibility = Visibility.Hidden

                lblSaveId.Visibility = Visibility.Hidden
                SaveId.Visibility = Visibility.Hidden
                SaveName.Visibility = Visibility.Hidden

                lblBankId.Visibility = Visibility.Hidden
                BankId.Visibility = Visibility.Hidden
                BankName.Visibility = Visibility.Hidden

                'btnPrint3.Visibility = Visibility.Hidden

            End If

            ReservToId.Visibility = Visibility.Visible
            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "المورد"

            If Md.ShowCurrency Then
                lblCurrencyId.Visibility = Visibility.Visible
                CurrencyId.Visibility = Visibility.Visible
            End If

            If Md.MyProjectType = ProjectType.X Then
                lblDocNo.Visibility = Visibility.Visible
                DocNo.Visibility = Visibility.Visible
                lblDocNo.Content = "أمر الشراء"

                'lblAccNo.Visibility = Visibility.Visible
                'AccNo.Visibility = Visibility.Visible
                'AccName.Visibility = Visibility.Visible
            End If


            btnPrint3.Content = "إذن إضافة"
            If Flag = FlagState.مردودات_مشتريات OrElse Flag = FlagState.مردودات_مشتريات_خارجية Then
                btnPrint3.Content = "إذن صرف"

                If Md.MyProjectType = ProjectType.X Then
                    lblDocNo.Content = "رقم إذن الصرف"
                End If
            End If




            If Flag = FlagState.عرض_أسعار_مورد Then
                lblCashier.Visibility = Visibility.Hidden
                CashierId.Visibility = Visibility.Hidden
                CashierName.Visibility = Visibility.Hidden

                GroupBoxPaymentType.Visibility = Visibility.Hidden

                lblSaveId.Visibility = Visibility.Hidden
                SaveId.Visibility = Visibility.Hidden
                SaveName.Visibility = Visibility.Hidden

                lblBankId.Visibility = Visibility.Hidden
                BankId.Visibility = Visibility.Hidden
                BankName.Visibility = Visibility.Hidden

                btnPrint3.Visibility = Visibility.Hidden

                lblDeliveryDate.Visibility = Visibility.Hidden
                DeliveryDate.Visibility = Visibility.Hidden

                btnGenerateSalesInvoiceFromQuotation.Visibility = Visibility.Visible
                btnGenerateSalesInvoiceFromQuotation.Content = btnGenerateSalesInvoiceFromQuotation.Content.ToString.Replace("مبيعات", "مشتريات")
            End If


            If Md.MyProjectType = ProjectType.Zohor Then
                lblDeliveryDate.Visibility = Visibility.Visible
                DeliveryDate.Visibility = Visibility.Visible
                lblDeliveryDate.Content = "ت فاتورة المورد"
            End If


        ElseIf TestImportAndReturn() Then
            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible
            lblCashier.Content = "الطالب"

            'ReservToId.Visibility = Visibility.Visible
            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "المورد"

            lblDocNo.Visibility = Visibility.Visible
            DocNo.Visibility = Visibility.Visible
            lblDocNo.Content = "رقم عقد المورد"

            If Flag = FlagState.مردودات_الاستيراد Then
                If Md.MyProjectType = ProjectType.X Then
                    lblDocNo.Content = "رقم إذن الصرف"
                End If
            End If

            'lblAccNo.Visibility = Visibility.Visible
            'AccNo.Visibility = Visibility.Visible
            'AccName.Visibility = Visibility.Visible

            lblOrderTypeId.Visibility = Visibility.Visible
            OrderTypeId.Visibility = Visibility.Visible
            OrderTypeName.Visibility = Visibility.Visible

            lblVersionNo.Visibility = Visibility.Visible
            VersionNo.Visibility = Visibility.Visible

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                lblDeliveryDate.Visibility = Visibility.Visible
                DeliveryDate.Visibility = Visibility.Visible
                lblDeliveryDate.Content = "تاريخ الاستلام"
            End If

            If Flag = FlagState.الاستيراد Then
                lblMessageId.Visibility = Visibility.Visible
                MessageId.Visibility = Visibility.Visible
            End If

            'Tab1.Visibility = Visibility.Collapsed
            Tab3.Visibility = Visibility.Visible
            PanelAcc.SelectedItem = Tab3
            If Md.ShowCurrency Then
                lblCurrencyId.Visibility = Visibility.Visible
                CurrencyId.Visibility = Visibility.Visible
            End If

            RdoFuture.IsChecked = True
            GroupBoxPaymentType.Visibility = Visibility.Hidden

            btnPrint3.Content = "إذن إضافة"
            'HideAcc()
        ElseIf TestSalesAndReturn() OrElse TestExportAndReturn() Then

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                lblDeliveryDate.Visibility = Visibility.Visible
                DeliveryDate.Visibility = Visibility.Visible
                If Md.MyProjectType = ProjectType.X Then
                    lblDeliveryDate.Content = "تاريخ التحميل"
                ElseIf Md.MyProjectType = ProjectType.X Then
                    lblDeliveryDate.Content = "تاريخ التحصيل"
                Else
                    lblDeliveryDate.Content = "تاريخ التسليم"
                End If
            End If


            If Flag <> FlagState.عرض_أسعار AndAlso Flag <> FlagState.أمر_توريد Then
                lblCashier.Visibility = Visibility.Visible
                CashierId.Visibility = Visibility.Visible
                CashierName.Visibility = Visibility.Visible
                lblCashier.Content = "البائع"

                GroupBoxPaymentType.Visibility = Visibility.Visible

                If Not Md.MyProjectType = ProjectType.X Then
                    lblSaveId.Visibility = Visibility.Visible
                    SaveId.Visibility = Visibility.Visible
                    SaveName.Visibility = Visibility.Visible
                End If
            Else
                lblCashier.Visibility = Visibility.Hidden
                CashierId.Visibility = Visibility.Hidden
                CashierName.Visibility = Visibility.Hidden

                GroupBoxPaymentType.Visibility = Visibility.Hidden

                lblSaveId.Visibility = Visibility.Hidden
                SaveId.Visibility = Visibility.Hidden
                SaveName.Visibility = Visibility.Hidden

                lblBankId.Visibility = Visibility.Hidden
                BankId.Visibility = Visibility.Hidden
                BankName.Visibility = Visibility.Hidden

                btnPrint3.Visibility = Visibility.Hidden

                lblDeliveryDate.Visibility = Visibility.Hidden
                DeliveryDate.Visibility = Visibility.Hidden

                btnGenerateSalesInvoiceFromQuotation.Visibility = Visibility.Visible
            End If


            ReservToId.Visibility = Visibility.Visible
            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "العميل"

            If Md.ShowCurrency Then
                lblCurrencyId.Visibility = Visibility.Visible
                CurrencyId.Visibility = Visibility.Visible
            End If

            If Not (Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X) Then
                lblWaiter.Visibility = Visibility.Visible
                WaiterId.Visibility = Visibility.Visible
                WaiterName.Visibility = Visibility.Visible
            End If

            btnPrint3.Content = "إذن صرف"
            If Flag = FlagState.مردودات_المبيعات OrElse Flag = FlagState.مردودات_التصدير Then
                btnPrint3.Content = "إذن إضافة"
            End If

            If TestInvestorsAndReturn() Then
                ReservToId.Visibility = Visibility.Hidden
            End If

            If Md.MyProjectType = ProjectType.X Then
                HideAcc()
                GroupBoxPaymentType.Visibility = Visibility.Visible
            End If


            If Md.MyProjectType = ProjectType.X Then
                Tab5.Visibility = Visibility.Visible
                PanelAcc.SelectedItem = Tab5
            End If
        ElseIf TestConsumablesAndReturn() Then
            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible
            lblCashier.Content = "الممرضة"

            GroupBoxPaymentType.Visibility = Visibility.Visible

            'lblDiscount.Visibility = Visibility.Visible b 
            'lblPerc.Visibility = Visibility.Visible
            'lblLE.Visibility = Visibility.Visible
            'DiscountPerc.Visibility = Visibility.Visible
            'DiscountValue.Visibility = Visibility.Visible

            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "المريض"

            'DocNo.IsEnabled = False

            btnPrint3.Content = "إذن صرف"
            If Flag = FlagState.مردودات_المستهلكات Then
                btnPrint3.Content = "إذن إضافة"
            End If

            If Not Md.Manager Then

                lblDiscount.Visibility = Visibility.Hidden
                lblDiscount_Copy.Visibility = Visibility.Hidden
                lblDiscount_Copy1.Visibility = Visibility.Hidden
                DiscountPerc.Visibility = Visibility.Hidden
                DiscountValue.Visibility = Visibility.Hidden

                btnPrint3.Visibility = Visibility.Hidden
                'btnPrint2.Visibility = Visibility.Hidden
                btnPrint5.Visibility = Visibility.Hidden

            End If
            HideAcc()
        Else
            HideAcc()
        End If

        If Md.MyProjectType = ProjectType.X Then
            PanelAcc.Visibility = Visibility.Collapsed
            Tab1.Visibility = Visibility.Collapsed
            'PanelGroups.Visibility = Visibility.Hidden
            'PanelTypes.Visibility = Visibility.Hidden
            'PanelItems.Visibility = Visibility.Hidden
            'HelpGD.Visibility = Visibility.Hidden
            'WP.Visibility = Visibility.Hidden

            DocNo.Visibility = Visibility.Hidden
            lblDocNo.Visibility = Visibility.Hidden

            'TabControl1.Margin = New Thickness(0)
            'TabControl1.BringIntoView()
            If TestSalesAndReturn() Then
                'btnPrint.Visibility = Visibility.Hidden
                btnPrint3.Visibility = Visibility.Hidden
                btnPrint5.Visibility = Visibility.Hidden
            End If
        Else
            btnItemsSearch.Visibility = Visibility.Hidden
            btnBalSearch.Visibility = Visibility.Hidden
        End If

        If Not Flag = FlagState.مشتريات OrElse Not Md.MyProjectType = ProjectType.X Then
            btnPrint4.Visibility = Visibility.Hidden
        End If

        'CashierId.IsEnabled = Md.Manager = 1


        If Md.MyProjectType = ProjectType.X Then
            lblComboBox1.Visibility = Visibility.Visible
            ComboBox1.Visibility = Visibility.Visible
        End If

        If (Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X) AndAlso TestSalesOnly() AndAlso Flag <> FlagState.عرض_أسعار AndAlso Flag <> FlagState.أمر_توريد Then
            lblSalesTypeId.Visibility = Visibility.Visible
            SalesTypeId.Visibility = Visibility.Visible
        End If


        If Not Md.Manager AndAlso Not Md.MyProjectType = ProjectType.X Then
            'btnDelete.Visibility = Visibility.Hidden

            If Flag = FlagState.تحويل_إلى_مخزن Then
                'btnPrint.Visibility = Visibility.Hidden
                btnPrint2.Visibility = Visibility.Hidden
            End If

            'DayDate.IsEnabled = False
            Shift.IsEnabled = False
            'If Md.DefaultStore > 0 Then
            '    StoreId.IsEnabled = False
            'End If
        End If

        If Not TestSalesAndReturn() OrElse Flag = FlagState.عرض_أسعار OrElse Flag = FlagState.أمر_توريد OrElse Flag = FlagState.أمر_شراء OrElse Flag = FlagState.عرض_أسعار_مورد Then
            Payed.Visibility = Visibility.Hidden
            lblPayed.Visibility = Visibility.Hidden
            Remaining.Visibility = Visibility.Hidden
            lblRemaining.Visibility = Visibility.Hidden
        End If

        If TestSalesAndReturn() OrElse TestPurchaseAndReturn() OrElse TestConsumablesAndReturn() OrElse TestImportAndReturn() OrElse TestExportAndReturn() Then
            lblDiscount.Visibility = Visibility.Visible
            lblDiscount_Copy.Visibility = Visibility.Visible
            DiscountPerc.Visibility = Visibility.Visible
            lblDiscount_Copy1.Visibility = Visibility.Visible
            DiscountValue.Visibility = Visibility.Visible
            lblTotalAfterDiscount.Visibility = Visibility.Visible
            TotalAfterDiscount.Visibility = Visibility.Visible
        Else
            lblDiscount.Visibility = Visibility.Hidden
            lblDiscount_Copy.Visibility = Visibility.Hidden
            DiscountPerc.Visibility = Visibility.Hidden
            lblDiscount_Copy1.Visibility = Visibility.Hidden
            DiscountValue.Visibility = Visibility.Hidden
            lblTotalAfterDiscount.Visibility = Visibility.Hidden
            TotalAfterDiscount.Visibility = Visibility.Hidden
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            Payed.Visibility = Visibility.Hidden
            lblPayed.Visibility = Visibility.Hidden
            Remaining.Visibility = Visibility.Hidden
            lblRemaining.Visibility = Visibility.Hidden
            If Not Md.Manager Then
                lblDiscount.Visibility = Visibility.Hidden
                lblDiscount_Copy.Visibility = Visibility.Hidden
                DiscountPerc.Visibility = Visibility.Hidden
                lblDiscount_Copy1.Visibility = Visibility.Hidden
                DiscountValue.Visibility = Visibility.Hidden
                'DayDate.IsEnabled = Not TestConsumablesAndReturn()
            End If
        End If


        If Md.MyProjectType = ProjectType.X OrElse Flag = FlagState.عرض_أسعار OrElse Flag = FlagState.أمر_توريد OrElse Flag = FlagState.أمر_شراء OrElse Flag = FlagState.عرض_أسعار_مورد Then
            lblWaiter.Visibility = Visibility.Hidden
            WaiterId.Visibility = Visibility.Hidden
            WaiterName.Visibility = Visibility.Hidden
        End If

        If (Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X) AndAlso Flag <> FlagState.مشتريات AndAlso Flag <> FlagState.مردودات_مشتريات AndAlso Flag <> FlagState.مشتريات_خارجية AndAlso Flag <> FlagState.مردودات_مشتريات_خارجية AndAlso Flag <> FlagState.الاستيراد AndAlso Flag <> FlagState.مردودات_الاستيراد Then
            DocNo.Visibility = Visibility.Hidden
            lblDocNo.Visibility = Visibility.Hidden
        End If

        If Md.MyProjectType = ProjectType.X AndAlso Flag = FlagState.المبيعات Then
            DocNo.Visibility = Visibility.Visible
            lblDocNo.Visibility = Visibility.Visible
            lblDocNo.Content = "رقم إذن الصرف"
        End If

        If Receive Then
            btnReceive.Visibility = Visibility.Visible
            btnReceiveAll.Visibility = Visibility.Visible
            btnReceiveSave.Visibility = Visibility.Visible
            StoreId.IsEnabled = False
            'DayDate.IsEnabled = False
            CashierId.IsEnabled = False

            btnSave.Visibility = Visibility.Hidden
            btnDelete.Visibility = Visibility.Hidden
            btnDeleteRow.Visibility = Visibility.Hidden
            btnFirst.Visibility = Visibility.Hidden
            btnNext.Visibility = Visibility.Hidden
            btnPrevios.Visibility = Visibility.Hidden
            btnLast.Visibility = Visibility.Hidden

            'btnPrint.Visibility = Visibility.Hidden
            btnPrint2.Visibility = Visibility.Hidden
            btnPrint3.Visibility = Visibility.Hidden
            btnPrint4.Visibility = Visibility.Hidden
            btnPrint5.Visibility = Visibility.Hidden

            lblLastEntry.Visibility = Visibility.Hidden
            Label1.Visibility = Visibility.Hidden
        End If


        If Md.MyProjectType = ProjectType.X Then
            Tab4.Visibility = Visibility.Collapsed

            If btnGenerateSalesInvoiceFromQuotation.Visibility <> Visibility.Visible Then
                btnImportFromExcel.Visibility = Visibility.Visible
            End If

        Else
            btnImportFromExcelBarcode.Visibility = Visibility.Hidden
        End If



        lblCostCenterId.Visibility = Visibility.Hidden
        CostCenterId.Visibility = Visibility.Hidden
        If Md.ShowCostCenter AndAlso (TestConsumablesAndReturn() OrElse Flag = FlagState.صرف OrElse Flag = FlagState.إضافة) Then
            lblCostCenterId.Visibility = Visibility.Visible
            CostCenterId.Visibility = Visibility.Visible
        End If

        bm.TestSecurity(Me, {btnSave, DayDate}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, InvoiceNo}, {btnPrint, btnPrint2, btnPrint3, btnPrint4, btnPrint5, btnPrintImage, btnPrintSafe}, {G})

    End Sub

    Private Sub btnItemsSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnItemsSearch.Click
        Try
            If G.CurrentRow Is Nothing Then G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Id)
            G.Focus()
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Id)
            GridKeyDown(G, New System.Windows.Forms.KeyEventArgs(Forms.Keys.F1))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnBalSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnBalSearch.Click
        Try
            If G.CurrentRow Is Nothing Then G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Id)
            G.Focus()
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Id)
            GridKeyDown(G, New System.Windows.Forms.KeyEventArgs(Forms.Keys.F12))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Hide()
        btnGenerateSalesInvoiceFromQuotation.Visibility = Visibility.Hidden
        lblTableId.Visibility = Visibility.Hidden
        TableId.Visibility = Visibility.Hidden
        lblTableSubId.Visibility = Visibility.Hidden
        TableSubId.Visibility = Visibility.Hidden
        lblNoOfPersons.Visibility = Visibility.Hidden
        NoOfPersons.Visibility = Visibility.Hidden
        lblMinPerPerson.Visibility = Visibility.Hidden
        MinPerPerson.Visibility = Visibility.Hidden
        CancelMinPerPerson.Visibility = Visibility.Hidden
        WithTax.Visibility = Visibility.Hidden
        WithService.Visibility = Visibility.Hidden
        ServiceValue.Visibility = Visibility.Hidden
        Taxvalue.Visibility = Visibility.Hidden
        RdoEmployees.Visibility = Visibility.Hidden
        PaymentType.Visibility = Visibility.Hidden
        ToName.Visibility = Visibility.Hidden
        ReservToId.Visibility = Visibility.Hidden
        lblToId.Visibility = Visibility.Hidden
        ToId.Visibility = Visibility.Hidden
        lblWaiter.Visibility = Visibility.Hidden
        WaiterId.Visibility = Visibility.Hidden
        WaiterName.Visibility = Visibility.Hidden
        txtFlag.Visibility = Visibility.Hidden
        TableIdName.Visibility = Visibility.Hidden
        btnCloseTable.Visibility = Visibility.Hidden
        IsClosed.Visibility = Visibility.Hidden
        IsCashierPrinted.Visibility = Visibility.Hidden
        CashierName.Visibility = Visibility.Hidden
        lblCashier.Visibility = Visibility.Hidden
        CashierId.Visibility = Visibility.Hidden
        lblPerc.Visibility = Visibility.Hidden
        lblLE.Visibility = Visibility.Hidden
        lblDeliveryman.Visibility = Visibility.Hidden
        DeliverymanId.Visibility = Visibility.Hidden
        DeliverymanName.Visibility = Visibility.Hidden
        TabItemTables.Visibility = Visibility.Hidden
        TabItemDelivery.Visibility = Visibility.Hidden

        lblAccNo.Visibility = Visibility.Hidden
        AccNo.Visibility = Visibility.Hidden
        AccName.Visibility = Visibility.Hidden

        CurrencyId.Visibility = Visibility.Hidden
        lblCurrencyId.Visibility = Visibility.Hidden

        IsCashierPrinted.Visibility = Visibility.Hidden
        btnReceive.Visibility = Visibility.Hidden
        btnReceiveAll.Visibility = Visibility.Hidden
        btnReceiveSave.Visibility = Visibility.Hidden

    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , 0, True)
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , 0, True)
    End Sub

    Private Sub btnReceive_Click(sender As Object, e As RoutedEventArgs) Handles btnReceive.Click
        StoreId.Focus()
        If bm.ShowHelp("التحويلات", StoreId, StoreId, Nothing, "select cast(StoreId as varchar(100)) 'كود المخزن',dbo.GetStoreName(StoreId) 'اسم المخزن',cast(InvoiceNo as varchar(100)) 'رقم الإذن' from SalesMaster where Flag=" & Flag & " and ToId=" & Val(ToId.Text), , , , "كود المخزن", "اسم المخزن") Then
            StoreId_LostFocus(Nothing, Nothing)
            ToId.Focus()
            InvoiceNo.Text = bm.SelectedRow(2)
            InvoiceNo_Leave(Nothing, Nothing)
            'InvoiceNo.IsEnabled = False
        End If
    End Sub

    Private Sub btnReceiveAll_Click(sender As Object, e As RoutedEventArgs) Handles btnReceiveAll.Click
        For i As Integer = 0 To G.Rows.Count - 1
            G.Rows(i).Cells(GC.ReceivedQty).Value = G.Rows(i).Cells(GC.Qty).Value
        Next
    End Sub

    Private Sub ReservToId_Checked(sender As Object, e As RoutedEventArgs) Handles ReservToId.Checked, ReservToId.Unchecked
        If ((TestPurchaseAndReturn() OrElse TestImportAndReturn()) AndAlso Not ReservToId.IsChecked) OrElse ((TestSalesAndReturn() OrElse TestExportAndReturn()) AndAlso ReservToId.IsChecked) Then
            lblToId.Content = "المورد"
        ElseIf ((TestSalesAndReturn() OrElse TestExportAndReturn()) AndAlso Not ReservToId.IsChecked) OrElse ((TestPurchaseAndReturn() OrElse TestImportAndReturn()) AndAlso ReservToId.IsChecked) Then
            lblToId.Content = "العميل"
        End If
        ToId_LostFocus(Nothing, Nothing)

    End Sub

    Private Sub G_SelectionChanged(sender As Object, e As EventArgs)
        If lop OrElse IsClearing Then Return

        If G.CurrentRow.Cells(GC.Id).Value Is Nothing Then
            ItemBal.Clear()
            bm.SetNoImage(Image1)
            Return
        End If
        Try
            ItemBal.Text = G.CurrentRow.Cells(GC.CurrentBal).Value
        Catch ex As Exception
            ItemBal.Clear()
        End Try
        Try
            bm.GetImage("Items", New String() {"Id"}, New String() {G.CurrentRow.Cells(GC.Id).Value}, "Image", Image1)
        Catch ex As Exception
            bm.SetNoImage(Image1)
        End Try
    End Sub

    Private Sub GetItemNameAndBal(i As Integer, Id As String, ItemSerialNo As String)
        Dim str As String = "IsStopped=0 and"
        If lop Then str = ""

        Dim dt As DataTable = bm.ExecuteAdapter("Select dbo.GetStoreItemBal('" & StoreId.Text & "','" & Id & "','" & ItemSerialNo & "',0,'" & G.Rows(i).Cells(GC.Size).Value & "','" & bm.ToStrDate(DayDate.SelectedDate) & "')Bal,*,dbo.GetGroupName(GroupId)GroupName,dbo.GetTypeName(GroupId,TypeId)TypeName From Items_View  where " & str & " Id='" & Id & "' " & ItemWhere())
        Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
        If dr.Length = 0 Then
            If Not G.Rows(i).Cells(GC.Id).Value Is Nothing Or G.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
            ClearRow(i)
            CalcTotal()
            Return
        End If
        G.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id)
        G.Rows(i).Cells(GC.Name).Value = dr(0)(Resources.Item("CboName"))
        G.Rows(i).Cells(GC.GroupName).Value = dr(0)(GC.GroupName)
        G.Rows(i).Cells(GC.TypeName).Value = dr(0)(GC.TypeName)
        G.Rows(i).Cells(GC.CurrentBal).Value = dr(0)("Bal")

        If Not Md.ShowItemExpireDate Then
            G.Rows(i).Cells(GC.Barcode).Value = dr(0)(GC.Barcode)
        End If

        G_SelectionChanged(Nothing, Nothing)

    End Sub

    Private Sub DocNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DocNo.KeyUp
        If Not (Md.MyProjectType = ProjectType.X AndAlso TestPurchaseOnly()) Then Return

        If bm.ShowHelp("أوامر الشراء", DocNo, DocNo, e, "select cast(InvoiceNo as varchar(100)) 'المسلسل',dbo.GetSubAccNameLink(2,ToId) 'المورد',dbo.ToStrDate(DayDate) 'التاريخ' from SalesMaster where Flag=" & 27 & " and (ToId=" & Val(ToId.Text) & " or " & Val(ToId.Text) & "=0)", , , , "المسلسل", "المورد") Then

            Dim MyFlag As Integer = Flag
            Dim MyInvoiceNo As String = InvoiceNo.Text
            Dim MyDate As DateTime = DayDate.SelectedDate
            Dim MyDocNo As String = DocNo.Text

            Flag = 27
            txtFlag.Text = Flag
            InvoiceNo.Text = DocNo.Text
            InvoiceNo_Leave(Nothing, Nothing)

            Flag = MyFlag
            txtFlag.Text = Flag
            InvoiceNo.Text = MyInvoiceNo
            DayDate.SelectedDate = MyDate
            DocNo.Text = MyDocNo

            'If InvoiceNo.Text.Trim = "" Then InvoiceNo.IsEnabled = False
        End If
    End Sub

    Private Sub btnPurchaseOrder_Click(sender As Object, e As RoutedEventArgs) Handles btnPurchaseOrder.Click
        If bm.ShowHelpMultiColumns("Header", PurchaseOrder, PurchaseOrder, Nothing, "select cast(InvoiceNo as nvarchar(100))'رقم الفاتورة',dbo.ToStrDate(DayDate)'التاريخ',TotalAfterDiscount 'قيمة الفاتورة' from SalesMaster where StoreId=" & Val(StoreId.Text) & " and Flag=" & 27 & " and ToId=" & Val(ToId.Text)) Then
            Dim dt As DataTable = bm.ExecuteAdapter("GetItemsFromPurchaseOrder", {"StoreId", "PurchaseOrder"}, {Val(StoreId.Text), Val(PurchaseOrder.Text)})
            G.Rows.Clear()
            If dt.Rows.Count > 0 Then G.Rows.Add(dt.Rows.Count)
            For i As Integer = 0 To dt.Rows.Count - 1
                G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
                GridCalcRow(Nothing, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, i))
                G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
                CalcRow(i)
            Next
            G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Id)
            CalcTotal()

        End If
    End Sub

    Private Sub btnAddCustomer_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCustomer.Click
        Dim frm As New MyWindow With {.Title = "Customers", .WindowState = WindowState.Maximized}
        bm.SetMySecurityType(frm, 816)
        frm.Content = New Customers With {.MyId = Val(ToId.Text)}
        frm.Show()
    End Sub

    Dim LoadPriceList As Boolean = False
    Private Sub PriceListId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles PriceListId.SelectionChanged
        LoadPriceList = True
        For i As Integer = 0 To G.Rows.Count - 1
            GridCalcRow(Nothing, New Forms.DataGridViewCellEventArgs(G.Columns(GC.UnitId).Index, i))
        Next
        LoadPriceList = False
    End Sub

    Private Sub btnGenerateSalesInvoiceFromQuotation_Click(sender As Object, e As RoutedEventArgs) Handles btnGenerateSalesInvoiceFromQuotation.Click
        If Flag = FlagState.عرض_أسعار_مورد Then
            If bm.ShowDeleteMSG("هل أنت متأكد من إنشاء فاتورة مشتريات؟") Then
                DontClear = True
                btnSave_Click(sender, e)
                DontClear = False
                bm.ShowMSG("تم إنشاء فاتورة مشتريات برقم " & bm.ExecuteScalar("GeneratePurchaseInvoiceFromQuotation", {"StoreId", "InvoiceNo"}, {Val(StoreId.Text), Val(InvoiceNo.Text)}))
            End If
        ElseIf Flag = FlagState.أمر_توريد Then
            If bm.ShowDeleteMSG("هل أنت متأكد من إنشاء فاتورة مبيعات؟") Then
                DontClear = True
                btnSave_Click(sender, e)
                DontClear = False
                bm.ShowMSG("تم إنشاء فاتورة مبيعات برقم " & bm.ExecuteScalar("GenerateSalesInvoiceFromQuotation2", {"StoreId", "InvoiceNo"}, {Val(StoreId.Text), Val(InvoiceNo.Text)}))
            End If
        Else
            If bm.ShowDeleteMSG("هل أنت متأكد من إنشاء فاتورة مبيعات؟") Then
                DontClear = True
                btnSave_Click(sender, e)
                DontClear = False
                bm.ShowMSG("تم إنشاء فاتورة مبيعات برقم " & bm.ExecuteScalar("GenerateSalesInvoiceFromQuotation", {"StoreId", "InvoiceNo"}, {Val(StoreId.Text), Val(InvoiceNo.Text)}))
            End If
        End If


    End Sub

    Private Sub Sales_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles Me.PreviewKeyDown
        If e.Key = Key.F12 Then
            If Not Md.ShowBarcode Then Return
            G.Focus()
            If G.CurrentRow Is Nothing Then
                Dim i As Integer = G.Rows.Add
                G.CurrentCell = G.Rows(i).Cells(GC.Barcode)
            End If
            G.CurrentCell = G.CurrentRow.Cells(GC.Barcode)
            G.CurrentRow.Cells(GC.Barcode).ReadOnly = True
            G.CurrentRow.Cells(GC.Barcode).ReadOnly = False
        ElseIf e.Key = Key.F5 Then
            If Not btnPrint2.Visibility = Visibility.Visible OrElse Not btnPrint2.IsEnabled Then Return
            btnPrint_Click(btnPrint2, Nothing)
        ElseIf e.Key = Key.F8 Then
            btnNew_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub btnInstallment_Click(sender As Object, e As RoutedEventArgs) Handles btnInstallment.Click
        DontClear = True

        If DocNo.Text.Trim = "" Then
            DocNo.Text = bm.ExecuteScalar("select dbo.getInstallNewDocNo()")
        End If
        RdoFuture.IsChecked = True
        btnSave_Click(sender, e)

        'If Not AllowSave Then Return

        Dim frm As New MyWindow With {.Title = "Installment", .WindowState = WindowState.Maximized}
        bm.SetMySecurityType(frm, 10102)
        frm.Content = New Installment With {.MyStoreId = Val(StoreId.Text), .MyToId = Val(ToId.Text), .MyInvoiceNo = Val(InvoiceNo.Text), .Flag = Flag}
        frm.Show()

        DontClear = False
    End Sub




    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDownload.Click
        Try
            MyImagedata = Nothing
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize <> 18 Then Return
            Dim s As New Forms.SaveFileDialog With {.Filter = "All files (*.*)|*.*"}
            s.FileName = CType(TreeView1.SelectedItem, TreeViewItem).Header

            If IsNothing(sender) Then
                MyBath = bm.GetNewTempName(s.FileName)
            Else
                If Not s.ShowDialog = Forms.DialogResult.OK Then Return
                MyBath = s.FileName
            End If

            btnDownload.IsEnabled = False
            F00 = Val(Flag)
            F0 = Val(StoreId.Text)
            F1 = InvoiceNo.Text
            F2 = CType(TreeView1.SelectedItem, TreeViewItem).Tag
            BackgroundWorker1.RunWorkerAsync()
        Catch ex As Exception
        End Try
    End Sub
    Dim F2 As String = "", F1 As String = "", F0 As String = "", F00 As String = ""
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim myCommand As SqlClient.SqlCommand
            myCommand = New SqlClient.SqlCommand("select Image from SalesAttachments where Flag='" & F00 & "' and StoreId='" & F0 & "' and InvoiceNo='" & F1 & "' and AttachedName='" & F2 & "'" & bm.AppendWhere, con)
            If con.State <> ConnectionState.Open Then con.Open()
            MyImagedata = CType(myCommand.ExecuteScalar(), Byte())
        Catch ex As Exception
        End Try
        con.Close()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            File.WriteAllBytes(MyBath, MyImagedata)
            Process.Start(MyBath)
        Catch ex As Exception
        End Try
        btnDownload.IsEnabled = True
    End Sub

    Dim MyImagedata() As Byte

    Private Sub BtnMonthlyPayment_Click(sender As Object, e As RoutedEventArgs) Handles btnMonthlyPayment.Click
        If Val(MonthlyPayment.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblMonthlyPayment.Content)
            Return
        End If

        If Val(MonthlyPaymentDay.Text) < 1 OrElse Val(MonthlyPaymentDay.Text) > 28 Then
            bm.ShowMSG("برجاء تحديد " & lblMonthlyPaymentDay.Content & " بين 1 و 28")
            Return
        End If
        SaveCustomerData()

        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@Id", "Header"}
        rpt.paravalue = New String() {Val(ToId.Text), CType(Parent, Page).Title}
        rpt.Rpt = "CustomerMonthlyPayment.rpt"
        rpt.Show()



    End Sub

    Sub SaveCustomerData()
        bm.ExecuteNonQuery("update Customers set MonthlyPayment='" & Val(MonthlyPayment.Text) & "',MonthlyPaymentDay='" & Val(MonthlyPaymentDay.Text) & "' where Id='" & Val(ToId.Text) & "'")
    End Sub

    Private Sub CalcBalSub()
        If TestPurchaseAndReturn() Then
            lblMonthlyPayment.Visibility = Visibility.Hidden
            MonthlyPayment.Visibility = Visibility.Hidden
            lblMonthlyPaymentDay.Visibility = Visibility.Hidden
            MonthlyPaymentDay.Visibility = Visibility.Hidden
            lblLastPayment.Visibility = Visibility.Hidden
            LastPayment.Visibility = Visibility.Hidden
            LastPaymentValue.Visibility = Visibility.Hidden
            btnMonthlyPayment.Visibility = Visibility.Hidden
            btn2.Visibility = Visibility.Hidden
            btns.Visibility = Visibility.Hidden
            btnCustomers.Visibility = Visibility.Hidden
        End If




        If Val(ToId.Text) > 0 Then
            If TestSalesAndReturn() Then

                MonthlyPayment.Text = bm.ExecuteScalar("select MonthlyPayment from Customers where Id='" & Val(ToId.Text) & "'")

                MonthlyPaymentDay.Text = bm.ExecuteScalar("select MonthlyPaymentDay from Customers where Id='" & Val(ToId.Text) & "'")

                LastPayment.Content = bm.ExecuteScalar("select dbo.getCustomerLastPayment('" & Val(ToId.Text) & "')")

                LastPaymentValue.Content = bm.ExecuteScalar("select dbo.getCustomerLastPaymentValue('" & Val(ToId.Text) & "')")

            Else
                MonthlyPayment.Visibility = Visibility.Hidden
                MonthlyPaymentDay.Visibility = Visibility.Hidden
                LastPayment.Visibility = Visibility.Hidden
                LastPaymentValue.Visibility = Visibility.Hidden
            End If

            CurrentBal.Content = bm.ExecuteScalar("select dbo.Bal0Link('" & IIf(TestSalesAndReturn, 1, 2) & "','" & Val(ToId.Text) & "','" & bm.ToStrDate(bm.MyGetDate()) & "',0)")
        Else
            CurrentBal.Content = ""
            LastPayment.Content = ""
            LastPaymentValue.Content = ""
        End If
    End Sub

    Private Sub BtnRpt_Click(sender As Object, e As RoutedEventArgs) Handles btnRpt.Click
        Dim rpt As New ReportViewer
        Dim RPTFlag1 As Integer = 3

        Dim MainAccNo As String = bm.ExecuteScalar("select AccNo from " & IIf(TestSalesAndReturn, "Customers", "Suppliers") & " where Id='" & Val(ToId.Text) & "'")

        rpt.paraname = New String() {"@MainAccNo", "MainAccName", "@SubAccNo", "SubAccName", "@FromDate", "@ToDate", "Header", "@Detailed", "@DetailedInvoice", "@LinkFile", "@ToId", "@RPTFlag1", "@RPTFlag2", "@ActiveOnly", "@HasBalOnly", "@WindowId", "@CostCenterId", "@CostCenterSubId", "@FromMainAccNo", "@ToMainAccNo"}
        rpt.paravalue = New String() {MainAccNo, "", Val(ToId.Text), ToName.Text, "2000-1-1", bm.MyGetDate(), "كشف حساب", 1, 0, IIf(TestSalesAndReturn, 1, 2), Val(ToId.Text), 3, 0, 1, 0, 0, 0, 0, MainAccNo, MainAccNo}
        rpt.Rpt = "AccountMotion.rpt"
        rpt.Show()
    End Sub

    Dim MyBath As String = ""

    Private Sub BtnReplacement_Click(sender As Object, e As RoutedEventArgs) Handles btnReplacement.Click
        If Val(ToId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المخزن المحول إليه")
            ToId.Focus()
            Return
        End If
        Dim mm As New Replacement
        mm.StoreId = Val(ToId.Text)
        mm.ShowDialog()
        If mm.Ok Then
            G.Rows.Clear()
            If mm.Result.Rows.Count = 0 Then Return
            G.Rows.Add(mm.Result.Rows.Count)
            For i As Integer = 0 To mm.Result.Rows.Count - 1
                G.Rows(i).Cells(GC.Id).Value = mm.Result.Rows(i)("ItemId").ToString
                GridCalcRow(Nothing, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, i))
                G.Rows(i).Cells(GC.ConsumptionQty).Value = mm.Result.Rows(i)("Qty").ToString
                G.Rows(i).Cells(GC.UnitId).Value = 1
                GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.UnitId).Index, i))
                G.Rows(i).Cells(GC.Qty).Value = bm.RndInt(Val(G.Rows(i).Cells(GC.ConsumptionQty).Value) / Val(G.Rows(i).Cells(GC.UnitQty).Value))
                CalcRow(i)

            Next
        End If
    End Sub

    Private Sub BtnCustomers_Click(sender As Object, e As RoutedEventArgs) Handles btnCustomers.Click
        If Val(ToId.Text) = 0 Then Return
        Dim frm As New MyWindow With {.Title = "Customers", .WindowState = WindowState.Maximized}
        bm.SetMySecurityType(frm, 816)
        frm.Content = New Customers With {.MyId = Val(ToId.Text)}
        frm.Show()
    End Sub

    Private Sub Btns_Click(sender As Object, e As RoutedEventArgs) Handles btns.Click
        If Val(ToId.Text) = 0 Then Return
        Dim frm As New MyWindow With {.Title = "أمر توريد", .WindowState = WindowState.Maximized}
        bm.SetMySecurityType(frm, 580)
        frm.Content = New Sales With {.Flag = Sales.FlagState.أمر_توريد, .MyToId = Val(ToId.Text)}
        frm.Show()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteFile.Click
        Try
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize = 18 Then
                If bm.ShowDeleteMSG("MsgDeleteFile") Then
                    bm.ExecuteNonQuery("delete from SalesAttachments where Flag=" & Flag & " and StoreId='" & StoreId.Text & "' and InvoiceNo='" & InvoiceNo.Text & "' and AttachedName='" & TreeView1.SelectedItem.Header & "'" & bm.AppendWhere)
                    LoadTree()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadTree()
        Dim dt As DataTable = bm.ExecuteAdapter("select AttachedName from SalesAttachments where Flag=" & Flag & " and StoreId='" & StoreId.Text & "' and InvoiceNo=" & InvoiceNo.Text & bm.AppendWhere)
        TreeView1.Items.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim nn As New TreeViewItem
            nn.Foreground = Brushes.DarkRed
            nn.FontSize = 18
            nn.Tag = dt.Rows(i)(0).ToString
            nn.Header = dt.Rows(i)(0).ToString
            TreeView1.Items.Add(nn)
        Next
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAttach.Click
        DontClear = True
        btnSave_Click(btnSave, Nothing)
        DontClear = False
        If Not AllowSave Then Return

        Dim o As New Forms.OpenFileDialog
        o.Multiselect = True
        If o.ShowDialog = Forms.DialogResult.OK Then
            For i As Integer = 0 To o.FileNames.Length - 1
                bm.SaveFile("SalesAttachments", "Flag", Val(Flag), "StoreId", Val(StoreId.Text), "InvoiceNo", InvoiceNo.Text, "AttachedName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "Image", o.FileNames(i))
            Next
        End If
        LoadTree()
    End Sub


    Private Sub TreeView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TreeView1.MouseDoubleClick
        Button4_Click(Nothing, Nothing)
    End Sub

    Private Sub CashValue_TextChanged(sender As Object, e As TextChangedEventArgs) Handles CashValue.TextChanged
        Payed.Text = CashValue.Text
        Payed_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub btnImportFromExcel_Click(sender As Object, e As RoutedEventArgs) Handles btnImportFromExcel.Click
        dt = bm.OpenExcel()
        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)(0).ToString.Trim = "" Then Continue For
            Dim Id As Integer = Val(bm.ExecuteScalar("select top 1 Id from Items where Name='" & dt.Rows(i)(0) & "'"))
            Dim x As Integer = 0
            Dim ShowError As Boolean = False
            If Id > 0 Then
                ShowError = True
            End If

            Dim GroupId As Integer = Val(bm.ExecuteScalar("select Id from Groups where Name='" & dt.Rows(i)(1) & "'"))
            If GroupId = 0 Then
                GroupId = bm.ExecuteScalar("declare @G int=(select isnull(max(Id),0)+1 from Groups) insert Groups(Id,Name) select @G,'" & dt.Rows(i)(1) & "' select @G")
            End If

            Dim TypeId As Integer = Val(bm.ExecuteScalar("select Id from Types where Name='" & dt.Rows(i)(2) & "' and GroupId=" & GroupId))
            If TypeId = 0 Then
                TypeId = Val(bm.ExecuteScalar("declare @T int=(select isnull(max(Id),0)+1 from Types  where GroupId=" & GroupId & ") insert Types(GroupId,Id,Name) select " & GroupId & ",@T,'" & dt.Rows(i)(2) & "' select @T"))
            End If

            Id = Val(bm.ExecuteScalar("select Id from Items where Name='" & dt.Rows(i)(0) & "'"))

            If Id = 0 Then
                Id = Val(bm.ExecuteScalar("declare @It int=(select isnull(max(Id),0)+1 from Items) INSERT Items (Id, Name, GroupId, TypeId, PrintingGroupId, StoreId, Unit, UnitCount, PurchasePrice, PurchasePriceSub, SalesPrice, SalesPriceSub, ItemType, Image, UnitSub, UserName, MyGetDate, Adding, IsTables, IsTakeAway, IsDelivary, UnitCount2, PurchasePriceSub2, SalesPriceSub2, UnitSub2, Limit, SizeId, ColorId, Barcode, IsStopped, Flag, EnName, ImportPrice, ImportPriceSub, ImportPriceSub2, IsKidneysWash, ItemUnitId, CodeOnPackage, IsService, CountryId, AllowEditSalesPrice, Maximum, MaximumSub, LimitSub, Bonus, Target, Stopped) select @It, '" & dt.Rows(i)(0) & "', " & GroupId & ", " & TypeId & ", 0, 0,'', 1, " & Val(dt.Rows(i)(4).ToString) & ", 0, " & Val(dt.Rows(i)(5).ToString) & ", 0, 2,null,'', 1, getdate(), 0, 1, 1, 1, 0, 0, 0, '', 0, 0, 0, dbo.ean13(@It), 0, 1, '', 0, 0, 0, 0, 1, '" & dt.Rows(i)(6) & "', 0, 0, 0, 0, 0, 0, 0, 0, 0 Select @It"))
            End If

            If Id = 0 Then
                bm.ShowMSG("يوجد خطأ في تسجيل الصنف " & dt.Rows(i)(0))
            End If
            If ShowError Then
                bm.ShowMSG("تم تكرار الصنف " & dt.Rows(i)(0) & " برقم " & Id)
            End If

            LopCalc = True
            x = AddItem(Id)
            G.Rows(i).Cells(GC.Price).Value = Val(dt.Rows(i)(4).ToString)
            G.Rows(i).Cells(GC.SalesPrice).Value = Val(dt.Rows(i)(5).ToString)
            LopCalc = False
            G.Rows(x).Cells(GC.Qty).Value = dt.Rows(i)(7)
            CalcRow(i)

        Next

        CalcTotal()

        If dt.Rows.Count > 0 Then
            bm.ShowMSG("تمت العملية بنجاح")
        Else
            bm.ShowMSG("لا توجد بيانات")
        End If

    End Sub


    Private Sub BtnImportFromExcelBarcode_Click(sender As Object, e As RoutedEventArgs) Handles btnImportFromExcelBarcode.Click
        dt = bm.OpenExcel()
        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)(0).ToString.Trim = "" Then Continue For
            LopCalc = True
            Dim x As Integer = G.Rows.Add
            G.Rows(x).Cells(GC.Barcode).Value = dt.Rows(i)(0).ToString
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Barcode).Index, i))
            LopCalc = False
        Next

        CalcTotal()
        bm.ShowMSG("تمت العملية بنجاح")

    End Sub


    Private Sub Btn2_Click(sender As Object, e As RoutedEventArgs) Handles btn2.Click

        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@ToDate2", "@Shift", "ShiftName", "@Flag", "@StoreId", "StoreName", "@FromInvoiceNo", "@ToInvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "@CashierId", "@IsClosedOnly", "Header", "@ToId", "@ItemId", "@ColorId", "ColorName", "@SizeId", "SizeName", "@SaveId", "ItemName", "@CountryId", "CountryName", "@GroupId", "GroupName", "@TypeId", "TypeName", "@WaiterId", "@SalesTypeId", "@Canceled", "@IsService", "@CostCenterId", "@ItemSerialNo"}
        rpt.paravalue = New String() {"2000-1-1", DayDate.SelectedDate, DayDate.SelectedDate, 0, "", 0, 0, "", 0, 0, 0, 31, 0, 0, 0, 0, btn2.Content, Val(ToId.Text), 0, 0, "", 0, "", 0, "", 0, "", 0, "", 0, "", 0, 0, 0, 0, 0, ""}
        rpt.Rpt = "Sales4.rpt"
        rpt.Show()

    End Sub

End Class
