Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management
Imports System.IO.Ports

Public Class Sales2

    Public MainTableName As String = "Stores"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"

    Public TableName As String = "SalesMaster"
    Public TableDetailsName As String = "SalesDetails"

    Public MainId As String = "StoreId"
    Public SubId As String = "InvoiceNo"

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim StaticsDt As New DataTable
    WithEvents G As New MyGrid
    WithEvents MyTimer As New Threading.DispatcherTimer
    Public Flag As Integer
    Public FirstColumn As String = "الكـــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "السعــــر", Statement As String = ""
    Dim Gp As String = "الأقسام الرئيسية", Tp As String = "الأقسام الفرعية", It As String = "الأصناف"

    Dim DO_CashCustPrevent As Boolean = False
    Public Save2Flag As Integer = 0

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
        Shared مشتريات_من_عملاء As Integer = 17
        Shared مبيعات_لموردين As Integer = 18
        Shared سلفة As Integer = 19
        Shared رد_سلفة As Integer = 20
        Shared مبيعات_الوردية As Integer = 99
    End Structure

    Sub NewId()
        InvoiceNo.Clear()
        InvoiceNo.IsEnabled = False
    End Sub

    Sub UndoNewId()
        InvoiceNo.IsEnabled = True
    End Sub

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        If Not Md.MyProjectType = ProjectType.X Then
            btnPrintA6.Visibility = Visibility.Hidden
        End If

        'If Md.MyProjectType = ProjectType.Shady Then
        '    btnNewWindow.Content = "تحويل لمخزن"
        'End If


        If Not Md.Manager Then
            SaveId.IsEnabled = False
            SaveName.IsEnabled = False
            BankId.IsEnabled = False
            BankName.IsEnabled = False
        End If

        StaticsDt = bm.ExecuteAdapter("select top 1 S_AccNo,R_S_AccNo,P_AccNo,R_P_AccNo,S_AccNo1,S_Per1,S_AccType1,S_AccNo2,S_Per2,S_AccType2,S_AccNo3,S_Per3,S_AccType3,S_AccNo4,S_Per4,S_AccType4,P_AccNo1,P_Per1,P_AccType1,P_AccNo2,P_Per2,P_AccType2,P_AccNo3,P_Per3,P_AccType3,P_AccNo4,P_Per4,P_AccType4 from Statics")

        bm.FillCombo("AccTypes", AccType1, "")
        bm.FillCombo("AccTypes", AccType2, "")
        bm.FillCombo("AccTypes", AccType3, "")
        bm.FillCombo("AccTypes", AccType4, "")

        RdoGrouping_Checked(Nothing, Nothing)
        bm.FillCombo("Shifts", Shift, "", , True)
        Shift.SelectedValue = Md.CurrentShiftId

        TabItem1.Header = "" ' TryCast(TryCast(Me.Parent, TabItem).Header, TabsHeader).MyTabHeader

        lblToId.Visibility = Visibility.Hidden
        ToId.Visibility = Visibility.Hidden
        ToName.Visibility = Visibility.Hidden


        lblCashier.Visibility = Visibility.Hidden
        CashierId.Visibility = Visibility.Hidden
        CashierName.Visibility = Visibility.Hidden

        lblSaveId.Visibility = Visibility.Hidden
        SaveId.Visibility = Visibility.Hidden
        SaveName.Visibility = Visibility.Hidden

        lblBankId.Visibility = Visibility.Hidden
        BankId.Visibility = Visibility.Hidden
        BankName.Visibility = Visibility.Hidden


        If Flag = FlagState.أرصدة_افتتاحية Then
            lblDayDate.Visibility = Visibility.Hidden
            DayDate.Visibility = Visibility.Hidden
            lblShift.Visibility = Visibility.Hidden
            Shift.Visibility = Visibility.Hidden

            lblDocNo.Visibility = Visibility.Hidden
            DocNo.Visibility = Visibility.Hidden
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

            btnPrint.Visibility = Visibility.Hidden
            btnPrint2.Visibility = Visibility.Hidden
            'btnPrint4.Visibility = Visibility.Hidden
            Label6.Visibility = Visibility.Hidden
            Total.Visibility = Visibility.Hidden
        End If

        If Flag = FlagState.مشتريات OrElse Flag = FlagState.مردودات_مشتريات OrElse Flag = FlagState.مبيعات_لموردين Then
            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible
            lblCashier.Content = "الطالب"
            If Flag = FlagState.مبيعات_لموردين Then lblCashier.Content = "البائع"

            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "المورد"

            GroupBoxPaymentType.Visibility = Visibility.Visible

            lblSaveId.Visibility = Visibility.Visible
            SaveId.Visibility = Visibility.Visible
            SaveName.Visibility = Visibility.Visible

            lblBankId.Visibility = Visibility.Visible
            BankId.Visibility = Visibility.Visible
            BankName.Visibility = Visibility.Visible

            'lblDiscount.Visibility = Visibility.Visible
            'lblPerc.Visibility = Visibility.Visible
            'lblLE.Visibility = Visibility.Visible
            'DiscountPerc.Visibility = Visibility.Visible
            'DiscountValue.Visibility = Visibility.Visible


        ElseIf Flag = FlagState.المبيعات OrElse Flag = FlagState.مردودات_المبيعات OrElse Flag = FlagState.مشتريات_من_عملاء OrElse Flag = FlagState.مبيعات_الوردية Then
            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible
            lblCashier.Content = "البائع"
            If Flag = FlagState.مشتريات_من_عملاء Then lblCashier.Content = "المستلم"

            lblSaveId.Visibility = Visibility.Visible
            SaveId.Visibility = Visibility.Visible
            SaveName.Visibility = Visibility.Visible

            lblBankId.Visibility = Visibility.Visible
            BankId.Visibility = Visibility.Visible
            BankName.Visibility = Visibility.Visible

            If Flag = FlagState.مبيعات_الوردية Then
                GroupBoxPaymentType.Visibility = Visibility.Hidden
                lblBankId.Visibility = Visibility.Hidden
                BankId.Visibility = Visibility.Hidden
                BankName.Visibility = Visibility.Hidden
            Else
                GroupBoxPaymentType.Visibility = Visibility.Visible
            End If


            'lblDiscount.Visibility = Visibility.Visible
            'lblPerc.Visibility = Visibility.Visible
            'lblLE.Visibility = Visibility.Visible
            'DiscountPerc.Visibility = Visibility.Visible
            'DiscountValue.Visibility = Visibility.Visible

            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "العميل"


        Else
            HideAcc()
            lblTotalAfterDiscount.Visibility = Visibility.Hidden
            TotalAfterDiscount.Visibility = Visibility.Hidden

            Remaining.Visibility = Visibility.Hidden
            Payed.Visibility = Visibility.Hidden
            lblRemaining.Visibility = Visibility.Hidden
            lblPayed.Visibility = Visibility.Hidden

        End If

        If Flag = FlagState.هدايا Then
            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "العميل"

        End If
        If Not (Flag = FlagState.المبيعات OrElse Flag = FlagState.مردودات_المبيعات OrElse Flag = FlagState.مشتريات_من_عملاء OrElse Flag = FlagState.مبيعات_الوردية) Then
            DriverName.Visibility = Visibility.Hidden
            CarNo.Visibility = Visibility.Hidden
            btnPrintCar.Visibility = Visibility.Hidden
            Distance.Visibility = Visibility.Hidden
            lblDriverName.Visibility = Visibility.Hidden
            lblCarNo.Visibility = Visibility.Hidden
            lblDistance.Visibility = Visibility.Hidden

        End If

        If False Then 'Md.ShopItemsOnly
            WP.Visibility = Visibility.Hidden
            GAcc.Visibility = Visibility.Hidden
            btnCalcAddDed.Visibility = Visibility.Hidden
            GNumbers.Visibility = Visibility.Visible
            btnDeleteRow.Visibility = Visibility.Hidden
            btnNewWindow.Visibility = Visibility.Hidden

            Dim x As Integer = 190
            HelpGD.Width -= x
            PanelGroups.Width -= x
            PanelTypes.Width -= x
            PanelItems.Width -= x
            GNumbers.Width -= x
            TabControl1.Margin = New Thickness(TabControl1.Margin.Left - x, TabControl1.Margin.Top, TabControl1.Margin.Right, TabControl1.Margin.Bottom)
        End If

        If Not (Flag = FlagState.المبيعات OrElse Flag = FlagState.مبيعات_الوردية) Then
            Tips.Visibility = Visibility.Hidden
            lblTips.Visibility = Visibility.Hidden
            lblEmpId.Visibility = Visibility.Hidden
            EmpId.Visibility = Visibility.Hidden
            EmpName.Visibility = Visibility.Hidden
        End If

        If Flag = FlagState.مردودات_المبيعات Then
            MyGrid.Background = New SolidColorBrush(Colors.LightGray)
        ElseIf Flag = FlagState.مبيعات_لموردين Then
            MyGrid.Background = New SolidColorBrush(Colors.Wheat)
        End If

        bm.Fields = New String() {MainId, SubId, "DayDate", "Shift", "Flag", "ToId", "WaiterId", "TableId", "TableSubId", "NoOfPersons", "WithTax", "Taxvalue", "WithService", "ServiceValue", "CancelMinPerPerson", "MinPerPerson", "PaymentType", "CashValue", "DiscountPerc", "DiscountValue", "Notes", "IsClosed", "IsCashierPrinted", "Cashier", "DeliverymanId", "Total", "TotalAfterDiscount", "DocNo", "AccNo1", "AccNo2", "AccNo3", "AccNo4", "AccType1", "AccType2", "AccType3", "AccType4", "Per1", "Per2", "Per3", "Per4", "Val1", "Val2", "Val3", "Val4", "SaveId", "DriverName", "CarNo", "Distance", "Tips", "Mobile", "BankId", "EmpId"}
        bm.control = New Control() {StoreId, InvoiceNo, DayDate, Shift, txtFlag, ToId, WaiterId, TableId, TableSubId, NoOfPersons, WithTax, Taxvalue, WithService, ServiceValue, CancelMinPerPerson, MinPerPerson, PaymentType, CashValue, DiscountPerc, DiscountValue, Notes, IsClosed, IsCashierPrinted, CashierId, DeliverymanId, Total, TotalAfterDiscount, DocNo, AccNo1, AccNo2, AccNo3, AccNo4, AccType1, AccType2, AccType3, AccType4, Per1, Per2, Per3, Per4, Val1, Val2, Val3, Val4, SaveId, DriverName, CarNo, Distance, Tips, Mobile, BankId, EmpId}
        bm.KeyFields = New String() {MainId, SubId}

        bm.Table_Name = TableName

        LoadGroups()
        LoadAllItems()
        RdoCash_Checked(Nothing, Nothing)
        LoadWFH()
        DayDate.SelectedDate = Nothing
        DayDate.SelectedDate = Md.CurrentDate
        Shift.SelectedValue = Md.CurrentShiftId
        StoreId.Text = Md.DefaultStore
        StoreId_LostFocus(Nothing, Nothing)



        'CashierId.IsEnabled = Md.Manager = 1

        ComboBox1.Visibility = Visibility.Hidden
        lblComboBox1.Visibility = Visibility.Hidden

        LoadCbo()
        ComboBox1.SelectedValue = Flag

        If Not Md.Manager Then
            btnDelete.Visibility = Visibility.Hidden
            btnFirst.Visibility = Visibility.Hidden
            btnPrevios.Visibility = Visibility.Hidden
            btnNext.Visibility = Visibility.Hidden
            btnLast.Visibility = Visibility.Hidden

            btnCustomers.Visibility = Visibility.Hidden
            btnItems.Visibility = Visibility.Hidden

            DayDate.IsEnabled = False
            Shift.IsEnabled = False
            If Md.DefaultStore > 0 Then
                StoreId.IsEnabled = False
            End If

            btnPrint.Visibility = Visibility.Hidden
            'btnPrint4.Visibility = Visibility.Hidden
            'btnPrint3.Visibility = Visibility.Hidden
            'btnSave.Visibility = Visibility.Hidden

            If Md.MyProjectType = ProjectType.X Then
                btnNew.Visibility = Visibility.Hidden
            End If

            If Md.MyProjectType = ProjectType.X OrElse Not Md.Manager Then
                btnProfit.Visibility = Visibility.Hidden
            End If
        End If

    End Sub


    Structure GC
        Shared Barcode As String = "Barcode"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared UnitId As String = "UnitId"
        Shared UnitQty As String = "UnitQty"
        Shared CurrentBal As String = "CurrentBal"
        Shared Qty As String = "Qty"
        Shared Price As String = "Price"
        Shared Weight As String = "Weight"
        Shared TotalWeight As String = "TotalWeight"
        Shared UnitSub As String = "UnitSub"
        Shared QtySub As String = "QtySub"
        Shared PriceSub As String = "PriceSub"
        Shared Service As String = "Service"
        Shared Bonus As String = "Bonus"
        Shared TotalBonus As String = "TotalBonus"
        Shared Value As String = "Value"
        Shared IsPrinted As String = "IsPrinted"
        Shared SalesPrice As String = "SalesPrice"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G
        WFH.TabIndex = 9
        G.TabIndex = 9
        G.TabStop = True

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Barcode, "الباركود")
        G.Columns.Add(GC.Id, "كود الصنف")
        G.Columns.Add(GC.Name, "اسم الصنف")

        Dim GCUnitId As New Forms.DataGridViewComboBoxColumn
        GCUnitId.HeaderText = "الوحدة"
        GCUnitId.Name = GC.UnitId
        bm.FillCombo("select 0 Id,'' Name", GCUnitId)
        G.Columns.Add(GCUnitId)

        G.Columns.Add(GC.UnitQty, "عدد الفرعى")

        G.Columns.Add(GC.CurrentBal, "الرصيد الحالى")

        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.Price, "السعر")
        G.Columns.Add(GC.Weight, "الوزن")
        G.Columns.Add(GC.TotalWeight, "إجمالى الوزن")
        G.Columns.Add(GC.UnitSub, "الوحدة (فرعى)")
        G.Columns.Add(GC.QtySub, "الكمية (فرعى)")
        G.Columns.Add(GC.PriceSub, "السعر (فرعى)")
        G.Columns.Add(GC.Service, "الخدمة")
        G.Columns.Add(GC.Bonus, "البونص")
        G.Columns.Add(GC.TotalBonus, "البونص")
        G.Columns.Add(GC.Value, "القيمة")
        G.Columns.Add(GC.IsPrinted, "طباعة للمطبخ")
        G.Columns.Add(GC.SalesPrice, "سعر البيع")

        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.Id).FillWeight = 110
        G.Columns(GC.Name).FillWeight = 300

        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.UnitQty).ReadOnly = True
        G.Columns(GC.CurrentBal).ReadOnly = True
        G.Columns(GC.Price).ReadOnly = ReadOnlyState()
        G.Columns(GC.UnitSub).ReadOnly = True
        G.Columns(GC.PriceSub).ReadOnly = ReadOnlyState()
        G.Columns(GC.Value).ReadOnly = True
        G.Columns(GC.Bonus).ReadOnly = True
        G.Columns(GC.TotalBonus).ReadOnly = True

        G.Columns(GC.Weight).Visible = False
        G.Columns(GC.TotalWeight).Visible = False
        G.Columns(GC.UnitQty).Visible = False
        G.Columns(GC.UnitSub).Visible = False
        G.Columns(GC.QtySub).Visible = False
        G.Columns(GC.PriceSub).Visible = False
        G.Columns(GC.IsPrinted).Visible = False


        G.Columns(GC.SalesPrice).Visible = False



        G.BarcodeIndex = G.Columns(GC.Barcode).Index
        'G.Columns(GC.UnitId).Visible = False


        If Not (Flag = FlagState.المبيعات OrElse Flag = FlagState.مردودات_المبيعات OrElse Flag = FlagState.مبيعات_لموردين OrElse Flag = FlagState.مبيعات_الوردية) Then
            G.Columns(GC.Service).Visible = False
        End If

        G.Columns(GC.Bonus).Visible = False
        If Not (Flag = FlagState.المبيعات OrElse Flag = FlagState.مبيعات_لموردين OrElse Flag = FlagState.مبيعات_الوردية) Then
            G.Columns(GC.TotalBonus).Visible = False
        End If

        If Flag = FlagState.تحويل_إلى_مخزن And Not Md.Manager Then
            G.Columns(GC.Price).Visible = False
            G.Columns(GC.Value).Visible = False
        End If

        If False Then 'Md.ShopItemsOnly
            G.Columns(GC.Barcode).Visible = False
            G.Columns(GC.CurrentBal).Visible = False
            G.Columns(GC.Service).Visible = False
            G.Columns(GC.TotalBonus).Visible = False
        End If

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Function ReadOnlyState() As Boolean
        If TestSalesAndReturn() Then 'And Flag <> FlagState.مشتريات_من_عملاء And Not Md.AllowCashierToEditPrice 
            Return True
        Else
            Return False
        End If
    End Function

    Function Fm() As Integer
        Select Case Flag
            Case FlagState.مبيعات_الصالة, FlagState.مردودات_مبيعات_الصالة
                Return 1
            Case FlagState.المبيعات, FlagState.مردودات_المبيعات, FlagState.مبيعات_الوردية
                Return 2
            Case FlagState.مبيعات_التوصيل, FlagState.مردودات_مبيعات_التوصيل
                Return 3
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

            Dim dt As DataTable = bm.ExecuteAdapter("LoadGroups2", New String() {"Form"}, New String() {Fm()}) 'Md.ShopItemsOnly
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
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
                SetStyle(x)
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


            Dim x As Integer = Val(bm.ExecuteScalar("select Flag from stores where ID='" & Val(StoreId.Text) & "'"))

            Dim Str As String = PriceFieldName(GC.Price, x)
            If Md.MyProjectType = ProjectType.X Then
                If Str = "SalesPriceSub" Then
                    Str = "dbo.GetStoreItemPriceSub(Id," & Val(StoreId.Text) & ")"
                Else
                    Str = "dbo.GetStoreItemPrice(Id," & Val(StoreId.Text) & ")"
                End If
            End If

            HelpDt = bm.ExecuteAdapter("Select Id,Name," & Str & " Price From Items where " & ItemWhere())

            HelpDt.TableName = "tbl"
            HelpDt.Columns(0).ColumnName = FirstColumn
            HelpDt.Columns(1).ColumnName = SecondColumn
            HelpDt.Columns(2).ColumnName = ThirdColumn

            dv.Table = HelpDt
            HelpGD.ItemsSource = dv
            HelpGD.Columns(0).Width = 75
            HelpGD.Columns(1).Width = 220
            HelpGD.Columns(2).Width = 75

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
            dv.RowFilter = " [" & FirstColumn & "] >" & IIf(txtID.Text.Trim = "", 0, txtID.Text) & " and [" & SecondColumn & "] like '%" & txtName.Text & "%' and [" & ThirdColumn & "] >=" & IIf(txtPrice.Text.Trim = "", 0, txtPrice.Text) & ""
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
        st = " ItemType in(0,1,2,3) and IsStopped=0 "
        If False Then 'Md.ShopItemsOnly = 1
            st &= " and (dbo.GetGroupIsShop(GroupId)=1 or " & 0 & "=0)" 'Md.ShopItemsOnly
        End If
        Return st
    End Function
    Private Sub LoadItems(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WItems.Tag = xx.Tag
            WItems.Children.Clear()

            TabItems.Header = It & " - " & xx.Content.ToString

            Dim dt As DataTable = bm.ExecuteAdapter("Select Id,Name From Items where " & ItemWhere() & " and GroupId=" & WTypes.Tag.ToString & " and TypeId=" & xx.Tag.ToString)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
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

    Sub AddItem(ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1)
        Try
            If Not TabControl1.SelectedIndex = 0 Then TabControl1.SelectedIndex = 0
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                'For x As Integer = 0 To G.Rows.Count - 1
                '    If Not G.Rows(x).Cells(GC.Id).Value Is Nothing AndAlso G.Rows(x).Cells(GC.Id).Value.ToString = Id.ToString AndAlso Not G.Rows(x).ReadOnly AndAlso Not G.Rows(x).Cells(GC.IsPrinted).Value = 1 Then
                '        i = x
                '        Exists = True
                '        GoTo Br
                '    End If
                'Next
                i = G.Rows.Add()
Br:
            End If

            Dim dt As DataTable = bm.ExecuteAdapter("Select Id,Name From Items where Id='" & Id & "' and " & ItemWhere())
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
                ClearRow(i)
                CalcTotal()
                Return
            End If
            G.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id)
            G.Rows(i).Cells(GC.Name).Value = dr(0)(GC.Name)

            'G.Rows(i).Cells(GC.Unit).Value = dr(0)(GC.Unit)
            LoadItemUint(i)

            If Val(G.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            G.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Rows(i).Cells(GC.Qty).Value)

            GridCalcRow(G, New System.Windows.Forms.DataGridViewCellEventArgs(G.Columns.IndexOf(G.Columns(GC.Qty)), i))

            LoadItemPrice(i)
            G.Rows(i).Cells(GC.UnitSub).Value = 0 'dr(0)(GC.UnitSub)
            G.Rows(i).Cells(GC.QtySub).Value = 0
            G.Rows(i).Cells(GC.PriceSub).Value = 0 'dr(0)(PriceFieldName(GC.PriceSub))
            If G.Rows(i).Cells(GC.IsPrinted).Value <> 1 Then G.Rows(i).Cells(GC.IsPrinted).Value = 0

            CalcRow(i)

            Validate_CashCustPrevent(i)

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
        Catch
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try
    End Sub

    Dim lop As Boolean = False
    Sub CalcRow(ByVal i As Integer)
        Try
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value.ToString = "" Then
                ClearRow(i)
                CalcTotal()
                Return
            End If
            G.Rows(i).Cells(GC.Qty).Value = Val(G.Rows(i).Cells(GC.Qty).Value)
            G.Rows(i).Cells(GC.Price).Value = Val(G.Rows(i).Cells(GC.Price).Value)
            G.Rows(i).Cells(GC.QtySub).Value = Val(G.Rows(i).Cells(GC.QtySub).Value)
            G.Rows(i).Cells(GC.PriceSub).Value = Val(G.Rows(i).Cells(GC.PriceSub).Value)
            G.Rows(i).Cells(GC.SalesPrice).Value = Val(G.Rows(i).Cells(GC.SalesPrice).Value)
            G.Rows(i).Cells(GC.Service).Value = Val(G.Rows(i).Cells(GC.Service).Value)
            G.Rows(i).Cells(GC.Bonus).Value = Val(G.Rows(i).Cells(GC.Bonus).Value)
            G.Rows(i).Cells(GC.TotalBonus).Value = Val(G.Rows(i).Cells(GC.TotalBonus).Value)
            G.Rows(i).Cells(GC.Weight).Value = Val(G.Rows(i).Cells(GC.Weight).Value)
            G.Rows(i).Cells(GC.TotalWeight).Value = Val(G.Rows(i).Cells(GC.TotalWeight).Value)

            'G.Rows(i).Cells(GC.Value).Value = Math.Round(Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.Price).Value) + Val(G.Rows(i).Cells(GC.QtySub).Value) * Val(G.Rows(i).Cells(GC.PriceSub).Value), 2)
            G.Rows(i).Cells(GC.Value).Value = Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.Price).Value) + Val(G.Rows(i).Cells(GC.QtySub).Value) * Val(G.Rows(i).Cells(GC.PriceSub).Value) + Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.Service).Value)
            G.Rows(i).Cells(GC.TotalBonus).Value = Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.Bonus).Value)
            G.Rows(i).Cells(GC.TotalWeight).Value = Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.Weight).Value)
        Catch ex As Exception
            ClearRow(i)
        End Try
        CalcTotal()
    End Sub

    Sub Validate_CashCustPrevent(ByVal i As Integer)
        Return
        If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value.ToString = "" Then
            Return
        ElseIf bm.ExecuteAdapter("Select G.CashCustPrevent From Groups G Left join Items It on(It.GroupId = G.Id) Where It.Id = '" & G.Rows(i).Cells(GC.Id).Value & "' and " & ItemWhere()).Rows(0).Item(0) = 1 Then
            DO_CashCustPrevent = True
            ToId.Text = ""
            ToId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Barcode).Value = Nothing
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.UnitId).Value = Nothing
        G.Rows(i).Cells(GC.UnitQty).Value = Nothing
        G.Rows(i).Cells(GC.CurrentBal).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.Price).Value = Nothing
        G.Rows(i).Cells(GC.Weight).Value = Nothing
        G.Rows(i).Cells(GC.TotalWeight).Value = Nothing
        G.Rows(i).Cells(GC.UnitSub).Value = Nothing
        G.Rows(i).Cells(GC.QtySub).Value = Nothing
        G.Rows(i).Cells(GC.PriceSub).Value = Nothing
        G.Rows(i).Cells(GC.Service).Value = Nothing
        G.Rows(i).Cells(GC.Bonus).Value = Nothing
        G.Rows(i).Cells(GC.TotalBonus).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.IsPrinted).Value = Nothing
        G.Rows(i).Cells(GC.SalesPrice).Value = Nothing
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
            If Md.MyProjectType = ProjectType.X Then
                If RdoVisa.IsChecked OrElse RdoCashVisa.IsChecked Then
                    AccType4.SelectedValue = 2
                    Per4.Text = 2.5
                Else
                    AccType4.SelectedValue = 0
                    Per4.Text = 0
                End If
            End If
        Catch ex As Exception
        End Try


    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        If G.Columns(e.ColumnIndex).Name = GC.Barcode AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value Is Nothing Then
            If G.Rows(e.RowIndex).Cells(GC.Barcode).Value.ToString.Length > 0 Then
                G.Rows(e.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select top 1 ItemId from ItemsBarcode where substring(Barcode,1,12)='" & G.Rows(e.RowIndex).Cells(GC.Barcode).Value.ToString.Substring(0, 12) & "'")
                GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, e.RowIndex))
                Exit Sub
            End If
        ElseIf G.Columns(e.ColumnIndex).Name = GC.Id Then
            AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
        ElseIf G.Columns(e.ColumnIndex).Name = GC.UnitId Then
            LoadItemPrice(e.RowIndex)
        ElseIf G.Columns(e.ColumnIndex).Name = GC.Qty Then
            'If G.Rows(e.RowIndex).Cells(GC.Qty).Value > G.Rows(e.RowIndex).Cells(GC.CurrentBal).Value Then
            '    bm.ShowMSG("رصيد الصنف لا يسمح")
            '    G.Rows(e.RowIndex).Cells(GC.Qty).Value = 0
            'End If
        End If
        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        CalcRow(e.RowIndex)
    End Sub

    Private Sub SaveId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SaveId.KeyUp
        If bm.ShowHelp("Saves", SaveId, SaveName, e, "select cast(Id as varchar(100)) Id,Name from Saves") Then
            SaveId_LostFocus(SaveId, Nothing)
        End If
    End Sub

    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles BankId.KeyUp
        If bm.ShowHelp("Banks", BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from Banks") Then
            BankId_LostFocus(BankId, Nothing)
        End If
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Stores") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        If Val(StoreId.Text.Trim()) > 0 Then
            Md.DefaultStore = StoreId.Text.Trim()
        End If
        bm.LostFocus(StoreId, StoreName, "select Name from Stores where Id=" & StoreId.Text.Trim())
        LoadAllItems()
        ClearControls()

    End Sub

    Private Sub SaveId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.LostFocus
        bm.LostFocus(SaveId, SaveName, "select Name from Saves where Id=" & SaveId.Text.Trim(), True)
    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus
        bm.LostFocus(BankId, BankName, "select Name from Banks where Id=" & BankId.Text.Trim(), True)
    End Sub

    Private Sub ToId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ToId.KeyUp
        Dim Title, tbl As String
        If Flag = FlagState.تحويل_إلى_مخزن Then
            tbl = "Stores"
            Title = "المخازن"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
        ElseIf Flag = FlagState.مشتريات OrElse Flag = FlagState.مردودات_مشتريات OrElse Flag = FlagState.مبيعات_لموردين Then
            tbl = "Suppliers"
            Title = "الموردين"
            Dim str As String = Val(bm.ExecuteScalar("select top 1 Id from chart where LinkFile=2"))
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl, "Suppliers", {"AccNo"}, {str})
        ElseIf Flag = FlagState.المبيعات OrElse Flag = FlagState.مردودات_المبيعات OrElse Flag = FlagState.مشتريات_من_عملاء OrElse Flag = FlagState.هدايا OrElse Flag = FlagState.مبيعات_الوردية Then
            If bm.ShowHelpCustomers(ToId, ToName, e) Then
                ToId_LostFocus(sender, Nothing)
            End If
        End If
    End Sub

    Function TestSalesAndReturn() As Boolean
        Return (Flag = FlagState.مبيعات_الصالة OrElse Flag = FlagState.مردودات_مبيعات_الصالة OrElse Flag = FlagState.المبيعات OrElse Flag = FlagState.مردودات_المبيعات OrElse Flag = FlagState.مبيعات_التوصيل OrElse Flag = FlagState.مردودات_مبيعات_التوصيل OrElse Flag = FlagState.مبيعات_الوردية)
    End Function

    Function TestSalesAndOnly() As Boolean
        'Return (Flag = FlagState.مبيعات_الصالة OrElse Flag = FlagState.المبيعات OrElse Flag = FlagState.مبيعات_التوصيل)
        Return (Flag = FlagState.مبيعات_الصالة OrElse Flag = FlagState.مبيعات_التوصيل OrElse Flag = FlagState.مبيعات_لموردين OrElse Flag = FlagState.مبيعات_الوردية OrElse Flag = FlagState.المبيعات)
    End Function

    Function TestPurchaseOnly() As Boolean
        Return (Flag = FlagState.مشتريات)
    End Function

    Function TestDelivary() As Boolean
        Return (Flag = FlagState.مبيعات_التوصيل OrElse Flag = FlagState.مردودات_مبيعات_التوصيل)
    End Function

    Private Sub ToId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToId.LostFocus, Mobile.LostFocus
        Dim tbl As String
        If Flag = FlagState.تحويل_إلى_مخزن Then
            tbl = "Stores"
        ElseIf Flag = FlagState.مشتريات OrElse Flag = FlagState.مردودات_مشتريات OrElse Flag = FlagState.مبيعات_لموردين Then
            tbl = "Suppliers"
        ElseIf TestSalesAndReturn() OrElse Flag = FlagState.مشتريات_من_عملاء OrElse Flag = FlagState.هدايا Then
            tbl = "Customers"

            If sender Is Mobile Then
                If Val(Mobile.Text) <> 0 Then
                    Dim ss As String = bm.ExecuteScalar("select max(ToId) from " & TableName & " where Mobile=" & Mobile.Text.Trim())
                    If ss <> "" Then
                        ToId.Text = ss
                    End If
                    ss = bm.ExecuteScalar("select max(DriverName) from " & TableName & " where Mobile=" & Mobile.Text.Trim())
                    If ss <> "" Then
                        DriverName.Text = ss
                    End If
                End If
            Else
                If Val(ToId.Text) <> 0 AndAlso Val(Mobile.Text) = 0 Then bm.LostFocus(ToId, Mobile, "select Mobile from " & tbl & " where Id=" & ToId.Text.Trim())
            End If
            If Not lop Then
                If bm.ExecuteScalar("select Type from Customers where Id=" & Val(ToId.Text)) = "0" Then
                    RdoFuture.IsChecked = True
                Else
                    RdoCash.IsChecked = True
                End If
            End If

        Else
            Return
        End If
        bm.LostFocus(ToId, ToName, "select Name from " & tbl & " where Id=" & ToId.Text.Trim())
        Dim s As String = ""
        If TestSalesAndReturn() Then
            Dim dt As DataTable = bm.ExecuteAdapter("GetCustomerData", New String() {"Id"}, New String() {Val(ToId.Text)})
            If dt.Rows.Count > 0 Then
                If Not lop Then DiscountPerc.Text = Val(dt.Rows(0)("DescPerc").ToString)
                For i As Integer = 0 To dt.Columns.Count - 2
                    s &= dt.Rows(0)(i).ToString & IIf(i = dt.Columns.Count - 1, "", vbCrLf)
                Next
            End If
        End If
        ToId.ToolTip = s
        ToName.ToolTip = s

        G.Focus()
        G.CurrentCell = G.Rows(0).Cells(GC.Id)

    End Sub

    Private Sub WaiterId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles WaiterId.KeyUp
        bm.ShowHelp("الويترز", WaiterId, WaiterName, e, "select cast(Id as varchar(100)) Id,Name from Sellers")
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
        bm.ShowHelp("Employees", CashierId, CashierName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Stopped=0")
    End Sub

    Private Sub CashierId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CashierId.LostFocus
        bm.LostFocus(CashierId, CashierName, "select " & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Id=" & CashierId.Text.Trim() & " and Stopped=0")
    End Sub

    Private Sub EmpId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId.KeyUp
        bm.ShowHelp("Employees", EmpId, EmpName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where SystemUser=0 and Stopped=0")
    End Sub

    Private Sub EmpId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId.LostFocus
        bm.LostFocus(EmpId, EmpName, "select " & Resources.Item("CboName") & " Name from Employees where SystemUser=0 and Id=" & EmpId.Text.Trim() & " and Stopped=0")
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True
        UndoNewId()
        bm.FillControls(Me)

        PaymentType_TextChanged(Nothing, Nothing)
        ToId_LostFocus(Nothing, Nothing)
        CashierId_LostFocus(Nothing, Nothing)
        EmpId_LostFocus(Nothing, Nothing)
        WaiterId_LostFocus(Nothing, Nothing)
        DeliverymanId_LostFocus(Nothing, Nothing)
        TId_LostFocus(TableId, Nothing)
        TId_LostFocus(TableSubId, Nothing)
        TId_LostFocus(NoOfPersons, Nothing)
        BankId_LostFocus(BankId, Nothing)

        AccNo1_LostFocus(Nothing, Nothing)
        AccNo2_LostFocus(Nothing, Nothing)
        AccNo3_LostFocus(Nothing, Nothing)
        AccNo4_LostFocus(Nothing, Nothing)

        Dim dt As DataTable = bm.ExecuteAdapter("select SD.* /*,It.Unit,It.UnitSub*/ from SalesDetails SD left join Items It on(SD.ItemId=It.Id) where SD.StoreId=" & StoreId.Text & " and SD.InvoiceNo=" & InvoiceNo.Text & bm.AppendWhere & " order by SD.Line")

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Barcode).Value = dt.Rows(i)("Barcode").ToString
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("ItemName").ToString
            LoadItemUint(i)
            G.Rows(i).Cells(GC.UnitId).Value = dt.Rows(i)("UnitId")
            G.Rows(i).Cells(GC.UnitQty).Value = dt.Rows(i)("UnitQty").ToString
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
            G.Rows(i).Cells(GC.Weight).Value = dt.Rows(i)("Weight").ToString
            G.Rows(i).Cells(GC.TotalWeight).Value = dt.Rows(i)("TotalWeight").ToString
            G.Rows(i).Cells(GC.UnitSub).Value = "" 'dt.Rows(i)("UnitSub").ToString
            G.Rows(i).Cells(GC.QtySub).Value = 0 ' 'dt.Rows(i)("QtySub").ToString
            G.Rows(i).Cells(GC.PriceSub).Value = dt.Rows(i)("PriceSub").ToString
            G.Rows(i).Cells(GC.Service).Value = dt.Rows(i)("Service").ToString
            G.Rows(i).Cells(GC.Bonus).Value = dt.Rows(i)("Bonus").ToString
            G.Rows(i).Cells(GC.TotalBonus).Value = dt.Rows(i)("TotalBonus").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Rows(i).Cells(GC.IsPrinted).Value = dt.Rows(i)("IsPrinted").ToString
            G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(i)("SalesPrice").ToString
            CalcRow(i)
            If Md.Cashier AndAlso TestSalesAndReturn() Then
                G.Rows(i).ReadOnly = True
                G.Rows(i).DefaultCellStyle.BackColor = System.Drawing.Color.PeachPuff
                btnDelete.IsEnabled = False
            End If
        Next
        CalcTotal()
        Notes.Focus()
        G.RefreshEdit()
        lop = False
        CalcTotalEnd()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click, btnPrint2.Click, btnPrint3.Click, btnProfit.Click, btnPrintA6.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If StoreId.Text.Trim = "" Then Return
        If Not CType(sender, Button).IsEnabled Then Return

        For i As Integer = 0 To G.Rows.Count - 1
            If Val(G.Rows(i).Cells(GC.Id).Value) > 0 Then
                Exit For
            End If
            If i = G.Rows.Count - 1 Then Return
        Next

        If Flag = FlagState.المبيعات AndAlso 0 = 1 AndAlso Mobile.Text.Trim = "" Then 'Md.IsCustTel
            bm.ShowMSG("برجاء تحديد " & lblTeleNo.Content)
            Mobile.Focus()
            Return
        End If

        If ToId.Visibility = Visibility.Visible AndAlso ToId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد " & lblToId.Content)
            ToId.Focus()
            Return
        End If
        If DO_CashCustPrevent And Val(ToId.Text.Trim) = 1 AndAlso TestSalesAndReturn() Then
            bm.ShowMSG("لا يمكن البيع للعميل النقدى")
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
        If WaiterId.Visibility = Visibility.Visible AndAlso WaiterId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد الويتر")
            WaiterId.Focus()
            Return
        End If
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

        G.EndEdit()
        Try
            CalcRow(G.CurrentRow.Index)
        Catch ex As Exception
        End Try

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

        Tips.Text = Val(Tips.Text)

        If Not Md.Manager Then
            DayDate.SelectedDate = Md.CurrentDate
            Shift.SelectedValue = Md.CurrentShiftId
        End If


        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & StoreId.Text & "'" & bm.AppendWhere)
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text
            'lblLastEntry.Foreground = System.Windows.Media.Brushes.Red
            'System.Threading.Thread.Sleep(500)
            'lblLastEntry.Foreground = System.Windows.Media.Brushes.Blue
            State = BasicMethods.SaveState.Insert
        End If

        MinPerPerson.Text = Val(MinPerPerson.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                InvoiceNo.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G, "SalesDetails", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {txtFlag.Text, StoreId.Text, InvoiceNo.Text}, New String() {"Barcode", "ItemId", "ItemName", "UnitId", "UnitQty", "Qty", "Price", "QtySub", "PriceSub", "Service", "Bonus", "TotalBonus", "Value", "IsPrinted", "SalesPrice", "Weight", "TotalWeight"}, New String() {GC.Barcode, GC.Id, GC.Name, GC.UnitId, GC.UnitQty, GC.Qty, GC.Price, GC.QtySub, GC.PriceSub, GC.Service, GC.Bonus, GC.TotalBonus, GC.Value, GC.IsPrinted, GC.SalesPrice, GC.Weight, GC.TotalWeight}, New VariantType() {VariantType.String, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal}, New String() {GC.Id}) Then Return

        'If /*State = BasicMethods.SaveState.Insert AndAlso*/ Flag = FlagState.مشتريات Then
        If Flag = FlagState.مشتريات Then
            bm.ExecuteNonQuery("UpdateItemPurchasePrice", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {txtFlag.Text, StoreId.Text, InvoiceNo.Text})
        End If

        Select Case CType(sender, Button).Name
            Case btnPrint.Name, btnPrint2.Name, btnPrint3.Name, btnPrint4.Name, btnPrintA6.Name
                State = BasicMethods.SaveState.Print
            Case btnCloseTable.Name
                State = BasicMethods.SaveState.Close
        End Select

        TraceInvoice(State.ToString)

        If TestSalesAndOnly() Then PrintPone(sender, 1)
        If sender Is btnPrint OrElse sender Is btnPrint2 OrElse sender Is btnPrint3 OrElse sender Is btnPrint4 OrElse sender Is btnProfit OrElse sender Is btnPrintA6 OrElse (sender Is btnCloseTable And btnPrint.IsEnabled) Then
            PrintPone(sender, 0)
            'txtID_Leave(Nothing, Nothing)
            'AllowClose = True
            'Return
        End If
        Display()
        If Not DontClear Then btnNew_Click(sender, e)
        AllowClose = True
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
        TableId.Focus()
    End Sub

    Sub ClearControls()
        Try
            NewId()
            Dim d As DateTime = Nothing
            Try
                If d.Year = 1 Then d = bm.MyGetDate
                d = DayDate.SelectedDate
            Catch ex As Exception
            End Try

            Dim s As String = Shift.SelectedValue.ToString
            Dim st As String = StoreId.Text

            Dim MySaveId As Integer = Val(SaveId.Text)
            bm.ClearControls(False)
            Payed.Clear()
            Remaining.Clear()

            If MySaveId = 0 Then
                SaveId.Text = Md.DefaultSave
            Else
                SaveId.Text = MySaveId
            End If

            CashierId.Text = Md.UserName
            BankId.Text = Md.DefaultBank

            SaveId_LostFocus(Nothing, Nothing)
            BankId_LostFocus(Nothing, Nothing)
            CashierId_LostFocus(Nothing, Nothing)
            EmpId_LostFocus(Nothing, Nothing)
            ToId_LostFocus(Nothing, Nothing)
            WaiterId_LostFocus(Nothing, Nothing)
            DeliverymanId_LostFocus(Nothing, Nothing)
            TId_LostFocus(TableId, Nothing)
            TId_LostFocus(TableSubId, Nothing)
            TId_LostFocus(NoOfPersons, Nothing)

            Select Case Flag
                Case FlagState.مشتريات, FlagState.مردودات_مشتريات

                    AccNo1.Text = StaticsDt.Rows(0)("P_AccNo1")
                    Per1.Text = StaticsDt.Rows(0)("P_Per1")
                    AccType1.SelectedValue = StaticsDt.Rows(0)("P_AccType1")

                    AccNo2.Text = StaticsDt.Rows(0)("P_AccNo2")
                    Per2.Text = StaticsDt.Rows(0)("P_Per2")
                    AccType2.SelectedValue = StaticsDt.Rows(0)("P_AccType2")

                    AccNo3.Text = StaticsDt.Rows(0)("P_AccNo3")
                    Per3.Text = StaticsDt.Rows(0)("P_Per3")
                    AccType3.SelectedValue = StaticsDt.Rows(0)("P_AccType3")

                    AccNo4.Text = StaticsDt.Rows(0)("P_AccNo4")
                    Per4.Text = StaticsDt.Rows(0)("P_Per4")
                    AccType4.SelectedValue = StaticsDt.Rows(0)("P_AccType4")

                Case FlagState.المبيعات, FlagState.مردودات_المبيعات, FlagState.مبيعات_الوردية

                    AccNo1.Text = StaticsDt.Rows(0)("S_AccNo1")
                    Per1.Text = StaticsDt.Rows(0)("S_Per1")
                    AccType1.SelectedValue = StaticsDt.Rows(0)("S_AccType1")

                    AccNo2.Text = StaticsDt.Rows(0)("S_AccNo2")
                    Per2.Text = StaticsDt.Rows(0)("S_Per2")
                    AccType2.SelectedValue = StaticsDt.Rows(0)("S_AccType2")

                    AccNo3.Text = StaticsDt.Rows(0)("S_AccNo3")
                    Per3.Text = StaticsDt.Rows(0)("S_Per3")
                    AccType3.SelectedValue = StaticsDt.Rows(0)("S_AccType3")

                    AccNo4.Text = StaticsDt.Rows(0)("S_AccNo4")
                    Per4.Text = StaticsDt.Rows(0)("S_Per4")
                    AccType4.SelectedValue = StaticsDt.Rows(0)("S_AccType4")

                    ToId.Text = 1
                    ToId_LostFocus(Nothing, Nothing)
            End Select
            AccNo1_LostFocus(Nothing, Nothing)
            AccNo2_LostFocus(Nothing, Nothing)
            AccNo3_LostFocus(Nothing, Nothing)
            AccNo4_LostFocus(Nothing, Nothing)

            PaymentType.Text = 1

            If Flag = FlagState.مشتريات OrElse Flag = FlagState.مردودات_مشتريات OrElse Flag = FlagState.مبيعات_لموردين Then
                RdoFuture.IsChecked = True
            End If

            If Md.Cashier = 1 Then
                CashierId.Text = Md.UserName
                CashierId_LostFocus(Nothing, Nothing)
            End If

            bm.GetCurrent()
            DayDate.SelectedDate = Md.CurrentDate
            Shift.SelectedValue = Md.CurrentShiftId

            StoreId.Text = st

            txtFlag.Text = Flag
            bm.AppendWhere = " and Flag=" & Flag
            G.Rows.Clear()
            CalcTotal()
            'InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & StoreId.Text & "'" & bm.AppendWhere)
            'If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"

            If TableSubId.Visibility = Visibility.Visible Then TableSubId.Text = 1
            If NoOfPersons.Visibility = Visibility.Visible Then NoOfPersons.Text = 1

            WithService.IsChecked = (WithService.Visibility = Visibility.Visible)
            WithTax.IsChecked = (WithTax.Visibility = Visibility.Visible)

            DO_CashCustPrevent = False

        Catch
        End Try
        If Flag = FlagState.مبيعات_الصالة Then TabControl1.SelectedItem = TabItemTables

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            TraceInvoice("Deleted")
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & bm.AppendWhere)

            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & bm.AppendWhere)

            btnNew_Click(sender, e)
        End If
    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExecuteNonQuery("BeforeDeleteSales", New String() {"Flag", "StoreId", "InvoiceNo", "UserDelete", "State"}, New String() {txtFlag.Text, StoreId.Text, InvoiceNo.Text, Md.UserName, State})
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyDown, InvoiceNo.KeyDown, ToId.KeyDown, WaiterId.KeyDown, TableId.KeyDown, TableSubId.KeyDown, NoOfPersons.KeyDown, txtID.KeyDown, CashierId.KeyDown, DeliverymanId.KeyDown, AccNo1.KeyDown, AccNo2.KeyDown, AccNo3.KeyDown, AccNo4.KeyDown, SaveId.KeyDown, BankId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Taxvalue.KeyDown, ServiceValue.KeyDown, MinPerPerson.KeyDown, CashValue.KeyDown, DiscountPerc.KeyDown, DiscountValue.KeyDown, txtPrice.KeyDown, Per1.KeyDown, Per2.KeyDown, Per3.KeyDown, Per4.KeyDown, Val1.KeyDown, Val2.KeyDown, Val3.KeyDown, Val4.KeyDown, Tips.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Dim AllowClose As Boolean = False
    'Private Sub MyBase_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    '    If Not btnSave.Enabled Then Exit Sub
    '    Select Case bm.RequestDelete
    '        Case BasicMethods.CloseState.Yes
    '            AllowClose = False
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
        If CType(sender, TextBox).Text.Trim = "" OrElse CType(sender, TextBox).Text.Trim = "0" Then CType(sender, TextBox).Clear()

        If sender Is TableId Then
            bm.LostFocus(TableId, TableIdName, "select Name from Tables where StoreId='" & StoreId.Text & "' and Id=" & TableId.Text.Trim())
            TestDoublicatinInTables(False)
        ElseIf sender Is TableSubId Then
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
        If ForceSales OrElse TestSalesAndReturn() OrElse Flag = FlagState.مبيعات_لموردين OrElse Flag = FlagState.تحويل_إلى_مخزن Then
            str = "Sales" & str
        Else
            str = "Purchase" & str
        End If

        Select Case i
            Case 1
                str &= "Sub"
            Case 2
                str &= "Sub2"
            Case Else
                str &= ""
        End Select
        Return str
    End Function

    Function PriceFieldName2(ByVal str As String, i As Integer, Optional ForceSales As Boolean = False) As String
        Select Case i
            Case 1
                Return str & "Sub"
            Case 2
                Return str & "Sub2"
            Case Else
                Return str
        End Select
    End Function

    Function ServiceFieldName(i As Integer) As String
        If i = 1 Then
            Return "Service2"
        Else
            Return "Service"
        End If
    End Function

    Function WeightFieldName(i As Integer) As String
        If i = 1 Then
            Return "WeightSub"
        Else
            Return "Weight"
        End If
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

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Shift", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "@InvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "Header", "Remaining", "Payed", "@GroupId", "@TypeId", "@ItemId", "@Mobile", "@CarNo"}

        If NewItemsOnly = 0 Then
            Dim ttl As String = ""
            Try
                ttl = CType(Parent, Page).Title
            Catch ex As Exception
                ttl = CType(Parent, Window).Title
            End Try
            rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, Shift.SelectedValue.ToString, Flag, StoreId.Text, InvoiceNo.Text, InvoiceNo.Text, InvoiceNo.Text, NewItemsOnly, 0, 0, 0, ttl, Val(Remaining.Text), Val(Payed.Text), 0, 0, 0, "", ""}
            If sender Is btnPrint4 Then
                rpt.Rpt = "PrintBarcodeFromSalesDetails.rpt"
                rpt.Print(".", Md.BarcodePrinter, 1)
                Return
            End If
            If sender Is btnProfit Then
                bm.ExecuteNonQuery("GetItemsSalesPre", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text})
                rpt.Rpt = "ItemsSales4.rpt"
                rpt.ShowDialog()
                Return
            End If

            rpt.Rpt = "SalesPone.rpt"
            If sender Is btnPrint2 Then rpt.Rpt = "SalesPone2.rpt"
            If sender Is btnPrint3 Then rpt.Rpt = "SalesPone3.rpt"
            If sender Is btnPrintA6 Then rpt.Rpt = "SalesPoneA6.rpt"
            Select Case Flag
                Case FlagState.المبيعات, FlagState.مردودات_المبيعات, FlagState.مشتريات, FlagState.مردودات_مشتريات, FlagState.مبيعات_لموردين, FlagState.مشتريات_من_عملاء, FlagState.مبيعات_الوردية
                    If sender Is btnPrint2 Then
                        rpt.Print(".", Md.PonePrinter, 1)
                    Else
                        rpt.Print()
                    End If
                Case Else
                    If sender Is btnPrint2 Then
                        rpt.Print(".", Md.PonePrinter, 2)
                    Else
                        rpt.Print()
                    End If
            End Select
            'rpt.Show()
        ElseIf Not (Flag = FlagState.المبيعات OrElse Flag = FlagState.مردودات_المبيعات OrElse Flag = FlagState.مبيعات_الوردية) Then
            rpt.Rpt = "SalesPoneKitchen.rpt"
            For i As Integer = 0 To G.Rows.Count - 1
                Try
                    If G.Rows(i).Cells(GC.IsPrinted).Value.ToString = 0 Then
                        Dim dt As DataTable = bm.ExecuteAdapter("GetPrinters", New String() {"Shift", "Flag", "StoreId", "InvoiceNo"}, New String() {Shift.SelectedValue.ToString, Flag, StoreId.Text, InvoiceNo.Text})
                        For x As Integer = 0 To dt.Rows.Count - 1
                            rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, Shift.SelectedValue.ToString, Flag, StoreId.Text, InvoiceNo.Text, InvoiceNo.Text, NewItemsOnly, 0, 0, dt.Rows(x)("PrintingGroupId")}
                            rpt.Print(dt.Rows(x)("ServerName"), dt.Rows(x)("PrinterName"))
                            'rpt.Show()
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
        Catch
        End Try
    End Sub


    Dim LopCalc As Boolean = False
    Private Sub CalcTotal() Handles CashValue.TextChanged, Total.TextChanged, DiscountPerc.TextChanged, DiscountValue.TextChanged, Taxvalue.TextChanged, ServiceValue.TextChanged, MinPerPerson.TextChanged, NoOfPersons.TextChanged, WithTax.Checked, WithTax.Unchecked, WithService.Checked, WithService.Unchecked, CancelMinPerPerson.Checked, CancelMinPerPerson.Unchecked, ToId.LostFocus
        If LopCalc OrElse lop Then Return
        Try
            LopCalc = True
            MinPerPerson.Text = Math.Round(0, 2)
            DiscountValue.Text = Math.Round(0, 2)
            Total.Text = Math.Round(0, 2)
            Taxvalue.Text = Math.Round(0, 2)
            ServiceValue.Text = Math.Round(0, 2)

            If CancelMinPerPerson.IsChecked Then
                MinPerPerson.Text = Math.Round(0, 2)
            Else
                MinPerPerson.Text = Val(bm.ExecuteScalar("select dbo.GetMinValuePerPerson(" & StoreId.Text & ")"))
            End If
            For i As Integer = 0 To G.Rows.Count - 1
                Total.Text += Val(G.Rows(i).Cells(GC.Value).Value)
            Next

            'DiscountValue.Text = Math.Round(Val(Total.Text) * Val(DiscountPerc.Text) / 100, 2)
            DiscountValue.Text = Val(Total.Text) * Val(DiscountPerc.Text) / 100

            If Not lop OrElse Not IsClosed.IsChecked Then

                If Val(Total.Text) < Val(MinPerPerson.Text) * Val(NoOfPersons.Text) Then
                    'Total.Text = Math.Round(Val(MinPerPerson.Text) * Val(NoOfPersons.Text), 2)
                    Total.Text = Val(MinPerPerson.Text) * Val(NoOfPersons.Text)
                End If

                'DiscountValue.Text = Math.Round(Val(Total.Text) * Val(DiscountPerc.Text) / 100, 2)
                DiscountValue.Text = Val(Total.Text) * Val(DiscountPerc.Text) / 100

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
        Catch ex As Exception
        End Try
        CalcTotalEnd()
    End Sub

    Sub CalcTotalEnd() Handles Per1.TextChanged, Per2.TextChanged, Per3.TextChanged, Per4.TextChanged, AccType1.SelectionChanged, AccType2.SelectionChanged, AccType3.SelectionChanged, AccType4.SelectionChanged, Tips.TextChanged, Val1.LostFocus, Val2.LostFocus, Val3.LostFocus, Val4.LostFocus
        'Val1.Text = Math.Round(Val(Total.Text) * Val(Per1.Text) / 100, 2)
        'Val2.Text = Math.Round(Val(Total.Text) * Val(Per2.Text) / 100, 2)
        'Val3.Text = Math.Round(Val(Total.Text) * Val(Per3.Text) / 100, 2)
        'Val4.Text = Math.Round(Val(Total.Text) * Val(Per4.Text) / 100, 2)

        Val1.IsEnabled = Val(Per1.Text) = 0
        Val2.IsEnabled = Val(Per2.Text) = 0
        Val3.IsEnabled = Val(Per3.Text) = 0
        Val4.IsEnabled = Val(Per4.Text) = 0

        If Val(Per1.Text) <> 0 Then Val1.Text = Val(Total.Text) * Val(Per1.Text) / 100
        If Val(Per2.Text) <> 0 Then Val2.Text = Val(Total.Text) * Val(Per2.Text) / 100
        If Val(Per3.Text) <> 0 Then Val3.Text = Val(Total.Text) * Val(Per3.Text) / 100
        If Val(Per4.Text) <> 0 Then Val4.Text = Val(Total.Text) * Val(Per4.Text) / 100

        If Md.MyProjectType = ProjectType.X Then
            Val4.Text = (Val(Total.Text) - Val(CashValue.Text)) * Val(Per4.Text) / 100
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
        TotalAfterDiscount.Text = Val(Total.Text) - Val(DiscountValue.Text) + Val(Taxvalue.Text) + Val(ServiceValue.Text) + d1 + d2 + d3 + d4 + Val(Tips.Text)

    End Sub

    Dim DontClear As Boolean = False
    Private Sub btnCloseTable_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCloseTable.Click
        If btnPrint.IsEnabled Then
            AllowClose = False
            DontClear = True
            btnSave_Click(btnCloseTable, e)
            DontClear = False
            If Not AllowClose Then Return
        End If
        'If Not bm.ExcuteNonQuery("update SalesMaster set IsClosed=1,ClosedDate=getdate(),DayDate='" & bm.ToStrDate(Md.CurrentDate) & "',Shift=" & Md.CurrentShiftId & " where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and InvoiceNo=" & InvoiceNo.Text) Then Return
        If Not bm.ExecuteNonQuery("update SalesMaster set IsClosed=1,ClosedDate=getdate(),DayDate='" & bm.ToStrDate(bm.MyGetDate()) & "',Shift=" & Md.CurrentShiftId & " where Flag=" & Flag & " and StoreId=" & StoreId.Text & " and InvoiceNo=" & InvoiceNo.Text) Then Return
        btnNew_Click(sender, e)
    End Sub

    Private Sub IsClosed_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles IsClosed.Checked, IsClosed.Unchecked
        btnCloseTable.IsEnabled = Not IsClosed.IsChecked
    End Sub


    Private Sub IsCashierPrinted_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles IsCashierPrinted.Checked, IsCashierPrinted.Unchecked
        btnSave.IsEnabled = Not (IsCashierPrinted.IsChecked And Md.Cashier = 1)
        btnPrint.IsEnabled = Not (IsCashierPrinted.IsChecked And Md.Cashier = 1)
        btnDelete.IsEnabled = Not (IsCashierPrinted.IsChecked And Md.Cashier = 1)
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
        txtID_Leave(Nothing, Nothing)
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
        txtID_Leave(Nothing, Nothing)
        TId_LostFocus(TableId, Nothing)
        TabControl1.SelectedItem = TabItem1
    End Sub

    Private Sub TestDoublicatinInTables(ByVal msg As Boolean)
        If TableId.Text.Trim = "" OrElse IsClosed.IsChecked Then Return
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

    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Name).Index Then
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "select cast(Id as varchar(100)) Id,Name from Items where " & ItemWhere()) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Sub SetStyle(ByVal x As Button)
        x.Style = Application.Current.FindResource("GlossyCloseButton")
        x.VerticalContentAlignment = VerticalAlignment.Center
        x.Width = 85
        x.Height = 35
        x.Margin = New Thickness(5, 5, 0, 0)
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblSaveId.SetResourceReference(ContentProperty, "Safe")
        lblBankId.SetResourceReference(ContentProperty, "Bank")
    End Sub

    Private Sub HideAcc()
        PanelItems.Margin = New Thickness(PanelItems.Margin.Left, PanelItems.Margin.Top, PanelItems.Margin.Right, 8)
        HelpGD.Margin = New Thickness(HelpGD.Margin.Left, HelpGD.Margin.Top, HelpGD.Margin.Right, 8)
        GAcc.Visibility = Visibility.Hidden
        btnCalcAddDed.Visibility = Visibility.Hidden
        GroupBoxPaymentType.Visibility = Visibility.Hidden
    End Sub

    Private Sub AccNo1_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo1.KeyUp
        bm.AccNoShowHelp(AccNo1, AccName1, e, , , True)
    End Sub
    Private Sub AccNo2_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo2.KeyUp
        bm.AccNoShowHelp(AccNo2, AccName2, e, , , True)
    End Sub
    Private Sub AccNo3_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo3.KeyUp
        bm.AccNoShowHelp(AccNo3, AccName3, e, , , True)
    End Sub
    Private Sub AccNo4_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo4.KeyUp
        bm.AccNoShowHelp(AccNo4, AccName4, e, , , True)
    End Sub

    Private Sub AccNo1_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo1.LostFocus
        bm.AccNoLostFocus(AccNo1, AccName1, , , True)
    End Sub
    Private Sub AccNo2_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo2.LostFocus
        bm.AccNoLostFocus(AccNo2, AccName2, , , True)
    End Sub
    Private Sub AccNo3_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo3.LostFocus
        bm.AccNoLostFocus(AccNo3, AccName3, , , True)
    End Sub
    Private Sub AccNo4_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo4.LostFocus
        bm.AccNoLostFocus(AccNo4, AccName4, , , True)
    End Sub

    Private Sub LoadItemUint(i As Integer)
        Dim Id As Integer = Val(G.Rows(i).Cells(GC.Id).Value)
        'Dim dt As DataTable = bm.ExcuteAdapter("Select * From Items where Id='" & Id & "' and " & ItemWhere() & "")

        bm.FillCombo("select 0 Id,Unit Name From Items where Id='" & Id & "' and " & ItemWhere() & " union select 1 Id,UnitSub Name From Items where Id='" & Id & "' and " & ItemWhere() & " /*union select 2 Id,UnitSub2 Name From Items where Id='" & Id & "' and " & ItemWhere() & "*/", G.Rows(i).Cells(GC.UnitId))

        'If Val(dt.Rows(0)("MainUnitOnly")) <> 1 AndAlso Val(bm.ExecuteScalar("select Flag from stores where ID='" & Val(StoreId.Text) & "'")) = 1 Then
        If Val(bm.ExecuteScalar("select Flag from stores where ID='" & Val(StoreId.Text) & "'")) = 1 Then
            G.Rows(i).Cells(GC.UnitId).Value = 1
        End If
        If G.Rows(i).Cells(GC.UnitId).Value Is Nothing Then G.Rows(i).Cells(GC.UnitId).Value = 0
    End Sub

    Private Sub LoadItemPrice(i As Integer)
        Try

            Dim Str As String = PriceFieldName(GC.Price, G.Rows(i).Cells(GC.UnitId).Value)
            If Md.MyProjectType = ProjectType.X Then
                If Str = "SalesPriceSub" Then
                    Str = "dbo.GetStoreItemPriceSub(Id," & Val(StoreId.Text) & ")"
                Else
                    Str = "dbo.GetStoreItemPrice(Id," & Val(StoreId.Text) & ")"
                End If
            End If

            Dim Str2 As String = PriceFieldName(GC.Price, G.Rows(i).Cells(GC.UnitId).Value, True)
            If Md.MyProjectType = ProjectType.X Then
                If Str2 = "SalesPriceSub" Then
                    Str2 = "dbo.GetStoreItemPriceSub(Id," & Val(StoreId.Text) & ")"
                Else
                    Str2 = "dbo.GetStoreItemPrice(Id," & Val(StoreId.Text) & ")"
                End If
            End If


            Dim dt As DataTable = bm.ExecuteAdapter("Select " & Str & " p1," & Str2 & " p2,UnitCount,UnitCount2," & ServiceFieldName(G.Rows(i).Cells(GC.UnitId).Value) & " s,Bonus," & WeightFieldName(G.Rows(i).Cells(GC.UnitId).Value) & " w From Items where Id='" & G.Rows(i).Cells(GC.Id).Value & "' and " & ItemWhere())
            If dt.Rows.Count = 0 Then Return
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(0)("p1")


            G.Rows(i).Cells(GC.UnitQty).Value = UnitCount(dt, G.Rows(i).Cells(GC.UnitId).Value)
            G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(0)("p2")

            If (Flag = FlagState.المبيعات OrElse Flag = FlagState.مبيعات_لموردين OrElse Flag = FlagState.مبيعات_الوردية) Then
                Dim DD As DataTable = bm.ExecuteAdapter("select * from CustomerItems where CustomerId=" & ToId.Text & " and ItemId=" & G.Rows(i).Cells(GC.Id).Value)
                If DD.Rows.Count > 0 Then
                    G.Rows(i).Cells(GC.Price).Value = DD.Rows(0)(PriceFieldName2(GC.Price, G.Rows(i).Cells(GC.UnitId).Value))
                End If
            End If

            If Flag = FlagState.مشتريات Then
                Dim DD As DataTable = bm.ExecuteAdapter("select top 1 SD.Price from SalesDetails SD left join SalesMaster SM on(SD.StoreId=SM.StoreId and SD.Flag=SM.Flag and SD.InvoiceNo=SM.InvoiceNo) where SM.Flag=" & Flag & " and SM.ToId=" & ToId.Text & " and SD.ItemId=" & G.Rows(i).Cells(GC.Id).Value & " and SD.UnitId=" & G.Rows(i).Cells(GC.UnitId).Value & " and SM.DayDate<='" & bm.ToStrDate(DayDate.SelectedDate) & "' order by SM.DayDate desc")
                If DD.Rows.Count > 0 Then
                    G.Rows(i).Cells(GC.Price).Value = DD.Rows(0)(0)
                End If
            End If


            G.Rows(i).Cells(GC.CurrentBal).Value = bm.ExecuteScalar("select dbo.FnStoreIetmBal(" & Val(StoreId.Text) & "," & G.Rows(i).Cells(GC.Id).Value & ",''," & G.Rows(i).Cells(GC.UnitId).Value & ",'" & bm.ToStrDate(DayDate.SelectedDate) & "')")

            If G.Columns(GC.Service).Visible Then
                G.Rows(i).Cells(GC.Service).Value = dt.Rows(0)("s")
            Else
                G.Rows(i).Cells(GC.Service).Value = 0
            End If

            G.Rows(i).Cells(GC.Weight).Value = dt.Rows(0)("w")

            If G.Columns(GC.TotalBonus).Visible Then
                G.Rows(i).Cells(GC.Bonus).Value = Val(dt.Rows(0)("Bonus"))
            Else
                G.Rows(i).Cells(GC.Bonus).Value = 0
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Sub ComboBox1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ComboBox1.SelectionChanged
        If ComboBox1.SelectedIndex = -1 Then Return
        Flag = ComboBox1.SelectedValue
        txtFlag.Text = ComboBox1.SelectedValue
        Try
            CType(Parent, Page).Title = CType(ComboBox1.ItemsSource, DataView).Item(ComboBox1.SelectedIndex)("Name")
        Catch ex As Exception
        End Try
        Md.Currentpage = CType(ComboBox1.ItemsSource, DataView).Item(ComboBox1.SelectedIndex)("Name")
        btnNew_Click(Nothing, Nothing)
    End Sub

    Private Sub LoadCbo()
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
            Case 9, 10
                dt.Rows.Add(New String() {9, "مشتريات"})
                dt.Rows.Add(New String() {10, "مردودات مشتريات"})
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
                dt.Rows.Add(New String() {99, "مبيعات الوردية"})
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
    End Sub

    Private Sub btnPrint4_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint4.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub Payed_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Payed.TextChanged, TotalAfterDiscount.TextChanged
        If Val(Payed.Text) > 0 Then
            Remaining.Text = Val(Payed.Text) - IIf(Val(CashValue.Text) > 0, Val(CashValue.Text), Val(TotalAfterDiscount.Text))
        Else
            Remaining.Clear()
        End If

    End Sub

    Private Sub btnCustomers_Click(sender As Object, e As RoutedEventArgs) Handles btnCustomers.Click
        Dim frm As New Window With {.Title = "العملاء", .WindowState = WindowState.Normal}
        bm.AddKeyDownToForm(frm)
        Dim c As New Customers
        frm.Content = c
        frm.SizeToContent = SizeToContent.WidthAndHeight
        frm.Show()
    End Sub

    Private Sub btnItems_Click(sender As Object, e As RoutedEventArgs) Handles btnItems.Click
        Dim frm As New Window With {.Title = "الأصناف", .WindowState = WindowState.Normal}
        bm.AddKeyDownToForm(frm)
        Dim c As New Items
        frm.Content = c
        frm.SizeToContent = SizeToContent.WidthAndHeight
        frm.Show()
    End Sub

    Private Sub btnNewWindow_Click(sender As Object, e As RoutedEventArgs) Handles btnNewWindow.Click

        'If Md.MyProjectType = ProjectType.Shady Then
        '    Dim frm As New Sales With {.Flag = Sales.FlagState.تحويل_إلى_مخزن}
        '    Dim W As New Window With {.Title = "تحويل إلى مخزن "}
        '    W.Content = frm
        '    W.Show()
        '    W.WindowState = WindowState.Maximized
        'Else
        '    Dim frm As New Sales With {.Flag = Flag}
        '    Dim W As New Window With {.Title = Md.Currentpage}
        '    W.Content = frm
        '    W.Show()
        '    W.WindowState = WindowState.Maximized
        'End If

        Dim frm As New Sales With {.Flag = Flag}
        Dim W As New Window With {.Title = Md.Currentpage}
        W.Content = frm
        W.Show()
        W.WindowState = WindowState.Maximized
    End Sub

    Private Sub btn1_Click(sender As Object, e As RoutedEventArgs) Handles btn1.Click, btn2.Click, btn3.Click, btn4.Click, btn5.Click, btn6.Click, btn7.Click, btn8.Click, btn9.Click
        Try
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Qty)
            G.Rows(G.CurrentRow.Index).Cells(GC.Qty).Value = sender.Content
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Value)
            CalcRow(G.CurrentRow.Index)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnPlus_Click(sender As Object, e As RoutedEventArgs) Handles btnPlus.Click
        Try
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Qty)
            G.Rows(G.CurrentRow.Index).Cells(GC.Qty).Value += 1
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Value)
            CalcRow(G.CurrentRow.Index)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btnMinus_Click(sender As Object, e As RoutedEventArgs) Handles btnMinus.Click
        Try
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Qty)
            If Val(G.Rows(G.CurrentRow.Index).Cells(GC.Qty).Value) > 0 Then
                G.Rows(G.CurrentRow.Index).Cells(GC.Qty).Value -= 1
            End If
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Value)
            CalcRow(G.CurrentRow.Index)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnPrintCar_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintCar.Click
        Dim rpt As New ReportViewer
        If Shift.SelectedIndex < 0 Then Shift.SelectedIndex = 0
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Shift", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "@CashierId", "@IsClosedOnly", "Header", "@ToId", "@GroupId", "GroupName", "@TypeId", "TypeName", "@ItemId", "@Barcode", "@AccNo", "@PaymentType", "@Mobile", "@CarNo"}
        rpt.paravalue = New String() {"1900-1-1", DayDate.SelectedDate, 0, 0, Val(StoreId.Text), 0, 0, 0, 4, ComboBox1.SelectedValue.ToString, 0, 0, 0, "مسحوبات سيارة", 0, 0, "", 0, "", 0, "", "", 0, "", CarNo.Text.Trim}

        rpt.Rpt = "CarSales.rpt"

        rpt.Show()
    End Sub

    Private Sub Display()
        Try
            Dim sp As New SerialPort
            sp.PortName = "COM1"
            sp.BaudRate = 9600
            sp.Parity = Parity.None
            sp.DataBits = 8
            sp.StopBits = StopBits.One
            sp.Open()
            sp.WriteLine(TotalAfterDiscount.Text)
            sp.Close()
            sp.Dispose()
            sp = Nothing
        Catch ex As Exception
        End Try
    End Sub



    Private Sub btnCloseShift_Click(sender As Object, e As RoutedEventArgs) Handles btnCloseShift.Click
        If bm.ShowDeleteMSG("إغلاق الوردية لا يمكنك من إعادة فتحها مرة أخرى" & vbCrLf & vbCrLf & "هل أنت متأكد من إغلاق الوردية؟") Then

            dt = bm.ExecuteAdapter("CloseShiftForEveryStore", New String() {"StoreId"}, New String() {StoreId.Text})

            Dim rpt As New ReportViewer
            rpt.paraname = New String() {"@FromDate", "@ToDate", "@Shift", "ShiftName", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "@CashierId", "@IsClosedOnly", "Header", "@ToId", "@ItemId", "@GroupId", "@TypeId", "@SaveId"}
            rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, Shift.SelectedValue.ToString, Shift.Text, 0, Val(StoreId.Text), 0, 0, 0, 6, ComboBox1.SelectedValue.ToString, 0, 0, 0, "تقرير المبيعات الإجمالى", Val(ToId.Text), 0, 0, 0, Val(SaveId.Text)}
            rpt.Rpt = "ItemsSales3.rpt"
            rpt.Show()

            Try
                Forms.Application.Restart()
                Application.Current.Shutdown()
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub CarNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles CarNo.LostFocus
        If CarNo.Text.Trim = "" Then Return
        ToId.Text = bm.ExecuteScalar("select top 1 ToId from " & TableName & " where CarNo='" & CarNo.Text & "'")
        ToId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub btnCalcAddDed_Click(sender As Object, e As RoutedEventArgs) Handles btnCalcAddDed.Click
        If Val(Total.Text) = 0 Then Return

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

        Dim Perc As Decimal = 1 + (d1 + d2 + d3 + d4) / Val(Total.Text)
        For i As Integer = 0 To G.Rows.Count - 1
            If Val(G.Rows(i).Cells(GC.Id).Value) > 0 Then
                G.Rows(i).Cells(GC.Price).Value = Math.Round(Val(G.Rows(i).Cells(GC.Price).Value) * Perc, 2, MidpointRounding.AwayFromZero)
                CalcRow(i)
            End If
        Next

        Per1.Clear()
        Per2.Clear()
        Per3.Clear()
        Per4.Clear()

        Val1.Clear()
        Val2.Clear()
        Val3.Clear()
        Val4.Clear()

        CalcTotal()
    End Sub
End Class
