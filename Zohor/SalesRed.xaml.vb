Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class SalesRed

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
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

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

        Shared مبيعات_الجملة As Integer = 21
        Shared مردودات_مبيعات_الجملة As Integer = 22
        Shared مبيعات_نصف_الجملة As Integer = 23
        Shared مردودات_مبيعات_نصف_الجملة As Integer = 24

    End Structure



    Sub NewId()
        InvoiceNo.Clear()
        InvoiceNo.IsEnabled = Md.Manager
    End Sub

    Sub UndoNewId()
        InvoiceNo.IsEnabled = True
    End Sub

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Temp_Checked(Nothing, Nothing)
        RdoCash_Checked(Nothing, Nothing)
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnPrint, btnPrint2, btnPrint3, btnPrint4, btnPrint5})
        LoadResource()
        bm.SetImage(Image1, "Clothes9.jpg")


        bm.FillCombo("Shifts", Shift, "")
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
        LoadCbo()

        'lblSaveId.Visibility = Visibility.Hidden
        'SaveId.Visibility = Visibility.Hidden
        'SaveName.Visibility = Visibility.Hidden

        StaticsDt = bm.ExecuteAdapter("select top 1 S_AccNo,R_S_AccNo,P_AccNo,R_P_AccNo,S_AccNo1,S_Per1,S_AccType1,S_AccNo2,S_Per2,S_AccType2,S_AccNo3,S_Per3,S_AccType3,S_AccNo4,S_Per4,S_AccType4,P_AccNo1,P_Per1,P_AccType1,P_AccNo2,P_Per2,P_AccType2,P_AccNo3,P_Per3,P_AccType3,P_AccNo4,P_Per4,P_AccType4 from Statics")

        bm.FillCombo("AccTypes", AccType1, "")
        bm.FillCombo("AccTypes", AccType2, "")
        bm.FillCombo("AccTypes", AccType3, "")
        bm.FillCombo("AccTypes", AccType4, "")

        RdoGrouping_Checked(Nothing, Nothing)

        TabItem1.Header = "" ' TryCast(TryCast(Me.Parent, TabItem).Header, TabsHeader).MyTabHeader

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
        End If

        If TestPurchaseAndReturn() Then
            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible
            lblCashier.Content = "الطالب"

            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "المورد"

            GroupBoxPaymentType.Visibility = Visibility.Visible

            'lblDiscount.Visibility = Visibility.Visible
            'lblPerc.Visibility = Visibility.Visible
            'lblLE.Visibility = Visibility.Visible
            'DiscountPerc.Visibility = Visibility.Visible
            'DiscountValue.Visibility = Visibility.Visible

            lblTotalAfterDiscount.Visibility = Visibility.Visible
            TotalAfterDiscount.Visibility = Visibility.Visible

        ElseIf TestSalesAndReturn() Then
            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible
            lblCashier.Content = "البائع"

            GroupBoxPaymentType.Visibility = Visibility.Visible

            'lblDiscount.Visibility = Visibility.Visible
            'lblPerc.Visibility = Visibility.Visible
            'lblLE.Visibility = Visibility.Visible
            'DiscountPerc.Visibility = Visibility.Visible
            'DiscountValue.Visibility = Visibility.Visible

            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "العميل"

            lblTotalAfterDiscount.Visibility = Visibility.Visible
            TotalAfterDiscount.Visibility = Visibility.Visible

        ElseIf Flag = FlagState.المستهلكات Then
            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible
            lblCashier.Content = "البائع"

            GroupBoxPaymentType.Visibility = Visibility.Visible

            'lblDiscount.Visibility = Visibility.Visible
            'lblPerc.Visibility = Visibility.Visible
            'lblLE.Visibility = Visibility.Visible
            'DiscountPerc.Visibility = Visibility.Visible
            'DiscountValue.Visibility = Visibility.Visible

            lblToId.Visibility = Visibility.Visible
            ToId.Visibility = Visibility.Visible
            ToName.Visibility = Visibility.Visible
            lblToId.Content = "المريض"

            lblTotalAfterDiscount.Visibility = Visibility.Visible
            TotalAfterDiscount.Visibility = Visibility.Visible
            HideAcc()
        Else
            HideAcc()
        End If

        bm.Fields = New String() {"Flag", MainId, SubId, "DayDate", "Shift", "ToId", "WaiterId", "TableId", "TableSubId", "NoOfPersons", "WithTax", "Taxvalue", "WithService", "ServiceValue", "CancelMinPerPerson", "MinPerPerson", "PaymentType", "CashValue", "DiscountPerc", "DiscountValue", "Notes", "IsClosed", "IsCashierPrinted", "Cashier", "DeliverymanId", "Total", "TotalAfterDiscount", "DocNo", "AccNo1", "AccNo2", "AccNo3", "AccNo4", "AccType1", "AccType2", "AccType3", "AccType4", "Per1", "Per2", "Per3", "Per4", "Val1", "Val2", "Val3", "Val4", "SaveId", "Temp", "AccNo"}
        bm.control = New Control() {txtFlag, StoreId, InvoiceNo, DayDate, Shift, ToId, WaiterId, TableId, TableSubId, NoOfPersons, WithTax, Taxvalue, WithService, ServiceValue, CancelMinPerPerson, MinPerPerson, PaymentType, CashValue, DiscountPerc, DiscountValue, Notes, IsClosed, IsCashierPrinted, CashierId, DeliverymanId, Total, TotalAfterDiscount, DocNo, AccNo1, AccNo2, AccNo3, AccNo4, AccType1, AccType2, AccType3, AccType4, Per1, Per2, Per3, Per4, Val1, Val2, Val3, Val4, SaveId, Temp, AccNo}
        bm.KeyFields = New String() {"Flag", MainId, SubId}

        bm.Table_Name = TableName

        LoadGroups()
        LoadAllItems()
        RdoCash_Checked(Nothing, Nothing)
        txtFlag.Text = Flag
        StoreId.Text = Md.DefaultStore
        StoreId_LostFocus(Nothing, Nothing)

        If Md.MyProjectType = ProjectType.X Then
            GAcc.Visibility = Visibility.Hidden
            PanelGroups.Visibility = Visibility.Hidden
            PanelTypes.Visibility = Visibility.Hidden
            PanelItems.Visibility = Visibility.Hidden
            HelpGD.Visibility = Visibility.Hidden
            WP.Visibility = Visibility.Hidden

            DocNo.Visibility = Visibility.Hidden
            lblDocNo.Visibility = Visibility.Hidden

            'TabControl1.Margin = New Thickness(0)
            'TabControl1.BringIntoView()
            PanelGroups.Visibility = Visibility.Hidden
            WP.Visibility = Visibility.Hidden
            Panelx.BringIntoView()
            If TestSalesAndReturn() Then
                btnPrint.Visibility = Visibility.Hidden
                btnPrint3.Visibility = Visibility.Hidden
                btnPrint5.Visibility = Visibility.Hidden
            End If

        End If

        If Not Flag = FlagState.مشتريات OrElse Not Md.MyProjectType = ProjectType.X Then
            btnPrint4.Visibility = Visibility.Hidden
        End If
        'CashierId.IsEnabled = Md.Manager = 1
        If Not (Md.MyProjectType = ProjectType.X) Then
            ComboBox1.Visibility = Visibility.Hidden
            lblComboBox1.Visibility = Visibility.Hidden
        End If
        ComboBox1.SelectedValue = Flag

        If Not Md.Manager Then
            'btnDelete.Visibility = Visibility.Hidden
            'btnFirst.Visibility = Visibility.Hidden
            'btnPrevios.Visibility = Visibility.Hidden
            'btnNext.Visibility = Visibility.Hidden
            'btnLast.Visibility = Visibility.Hidden

            DayDate.IsEnabled = False
            Shift.IsEnabled = False
            If Md.DefaultStore > 0 Then
                StoreId.IsEnabled = False
            End If
        End If

        If Md.MyProjectType = ProjectType.X Then
            GroupBoxPayment.Visibility = Visibility.Hidden
            GroupBoxPaymentType.Visibility = Visibility.Hidden
            Notes.Visibility = Visibility.Hidden
            lblNotes.Visibility = Visibility.Hidden
        End If
    End Sub


    Structure GC
        Shared SalesInvoiceNo As String = "SalesInvoiceNo"
        Shared Barcode As String = "Barcode"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Color As String = "Color"
        Shared Size As String = "Size"
        Shared UnitId As String = "UnitId"
        Shared UnitQty As String = "UnitQty"
        Shared Qty As String = "Qty"
        Shared Price As String = "Price"
        Shared UnitSub As String = "UnitSub"
        Shared QtySub As String = "QtySub"
        Shared PriceSub As String = "PriceSub"
        Shared Value As String = "Value"
        Shared IsPrinted As String = "IsPrinted"
        Shared SalesPrice As String = "SalesPrice"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.SalesInvoiceNo, "رقم الفاتورة")
        G.Columns.Add(GC.Barcode, "الباركود")
        G.Columns.Add(GC.Id, "كود الصنف")
        G.Columns.Add(GC.Name, "اسم الصنف")

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

        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.Price, "السعر")
        G.Columns.Add(GC.UnitSub, "الوحدة (فرعى)")
        G.Columns.Add(GC.QtySub, "الكمية (فرعى)")
        G.Columns.Add(GC.PriceSub, "السعر (فرعى)")
        G.Columns.Add(GC.Value, "القيمة")
        G.Columns.Add(GC.IsPrinted, "طباعة للمطبخ")
        G.Columns.Add(GC.SalesPrice, "سعر البيع")

        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.Id).FillWeight = 110
        G.Columns(GC.Name).FillWeight = 300
        G.Columns(GC.Color).FillWeight = 100
        G.Columns(GC.Size).FillWeight = 100
        G.Columns(GC.UnitId).FillWeight = 100
        G.Columns(GC.UnitQty).FillWeight = 100
        G.Columns(GC.Qty).FillWeight = 100
        G.Columns(GC.Price).FillWeight = 100
        G.Columns(GC.UnitSub).FillWeight = 100
        G.Columns(GC.QtySub).FillWeight = 100
        G.Columns(GC.PriceSub).FillWeight = 100
        G.Columns(GC.Value).FillWeight = 100
        G.Columns(GC.IsPrinted).FillWeight = 100
        G.Columns(GC.SalesPrice).FillWeight = 100

        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.UnitQty).ReadOnly = True
        G.Columns(GC.Price).ReadOnly = ReadOnlyState()
        G.Columns(GC.UnitSub).ReadOnly = True
        G.Columns(GC.PriceSub).ReadOnly = ReadOnlyState()
        G.Columns(GC.Value).ReadOnly = True

        G.Columns(GC.UnitQty).Visible = False
        G.Columns(GC.UnitSub).Visible = False
        G.Columns(GC.QtySub).Visible = False
        G.Columns(GC.PriceSub).Visible = False
        G.Columns(GC.IsPrinted).Visible = False

        If Not ((Md.MyProjectType = ProjectType.X) AndAlso TestPurchaseOnly()) Then
            G.Columns(GC.SalesPrice).Visible = False
        End If

        Select Case Flag
            Case FlagState.مردودات_المبيعات, FlagState.مردودات_مبيعات_التوصيل, FlagState.مردودات_مبيعات_الجملة, FlagState.مردودات_مبيعات_الصالة, FlagState.مردودات_مبيعات_نصف_الجملة, FlagState.مردودات_مشتريات
            Case Else
                G.Columns(GC.SalesInvoiceNo).Visible = False
        End Select

        If Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.UnitId).Visible = False
        Else
            G.Columns(GC.Color).Visible = False
            G.Columns(GC.Size).Visible = False
        End If

        If Md.ShowBarcode Then
            G.BarcodeIndex = G.Columns(GC.Barcode).Index
        Else
            G.Columns(GC.Barcode).Visible = False
        End If


        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Function ReadOnlyState() As Boolean
        If Md.Manager Then
            Return False
        ElseIf Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            Return False
        Else
            Return False
        End If
    End Function

    Function Fm() As Integer
        Select Case Flag
            Case FlagState.مبيعات_الصالة, FlagState.مردودات_مبيعات_الصالة
                Return 1
            Case FlagState.المبيعات, FlagState.مردودات_المبيعات
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
            HelpDt = bm.ExecuteAdapter("Select Id,Name," & PriceFieldName(GC.Price, 0) & " Price From Items  where IsStopped=0 " & ItemWhere())
            HelpDt.TableName = "tbl"
            HelpDt.Columns(0).ColumnName = FirstColumn
            HelpDt.Columns(1).ColumnName = SecondColumn
            HelpDt.Columns(2).ColumnName = ThirdColumn

            dv.Table = HelpDt
            HelpGD.ItemsSource = dv
            HelpGD.Columns(0).Width = 75
            HelpGD.Columns(1).Width = 220
            HelpGD.Columns(2).Width = 75
            If Md.Receptionist AndAlso Md.MyProjectType = ProjectType.X Then
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
            dv.RowFilter = " [" & FirstColumn & "] >" & IIf(txtID.Text.Trim = "", 0, txtID.Text) & " and [" & SecondColumn & "] like '" & txtName.Text & "%' and [" & ThirdColumn & "] >=" & IIf(txtPrice.Text.Trim = "", 0, txtPrice.Text) & ""
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
        Return st
    End Function
    Private Sub LoadItems(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WItems.Tag = xx.Tag
            WItems.Children.Clear()

            TabItems.Header = It & " - " & xx.Content.ToString

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items  where IsStopped=0 " & ItemWhere() & " and GroupId=" & WTypes.Tag.ToString & " and TypeId=" & xx.Tag.ToString)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x)
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

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items  where IsStopped=0 and Id='" & Id & "' " & ItemWhere())
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Rows(i).Cells(GC.Id).Value Is Nothing Or G.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
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
            LoadItemPrice(i)
            G.Rows(i).Cells(GC.UnitSub).Value = 0 'dr(0)(GC.UnitSub)
            G.Rows(i).Cells(GC.QtySub).Value = 0
            G.Rows(i).Cells(GC.PriceSub).Value = 0 'dr(0)(PriceFieldName(GC.PriceSub))
            If G.Rows(i).Cells(GC.IsPrinted).Value <> 1 Then G.Rows(i).Cells(GC.IsPrinted).Value = 0

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

            'G.Rows(i).Cells(GC.Value).Value = Math.Round(Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.Price).Value) + Val(G.Rows(i).Cells(GC.QtySub).Value) * Val(G.Rows(i).Cells(GC.PriceSub).Value), 2)
            G.Rows(i).Cells(GC.Value).Value = Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.Price).Value) + Val(G.Rows(i).Cells(GC.QtySub).Value) * Val(G.Rows(i).Cells(GC.PriceSub).Value)
        Catch ex As Exception
            ClearRow(i)
        End Try
        CalcTotal()
    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Barcode).Value = Nothing
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.Color).Value = Nothing
        G.Rows(i).Cells(GC.Size).Value = Nothing
        G.Rows(i).Cells(GC.UnitId).Value = Nothing
        G.Rows(i).Cells(GC.UnitQty).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.Price).Value = Nothing
        G.Rows(i).Cells(GC.UnitSub).Value = Nothing
        G.Rows(i).Cells(GC.QtySub).Value = Nothing
        G.Rows(i).Cells(GC.PriceSub).Value = Nothing
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

    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(e.ColumnIndex).Name = GC.Barcode AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value Is Nothing Then
                Dim BC As String = Val(G.Rows(e.RowIndex).Cells(GC.Barcode).Value.ToString)
                If BC.Length > 12 AndAlso Val(BC.Substring(0, 1)) > 0 Then BC = BC.Substring(0, 12)
                If (Md.MyProjectType = ProjectType.X) AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                    BC = BC.Substring(1)
                    G.Rows(e.RowIndex).Cells(GC.Id).Value = Val(Mid(BC, 1, BC.Length - 4))
                    AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
                    G.Rows(e.RowIndex).Cells(GC.Color).Value = Integer.Parse(Val(Mid(BC, BC.Length - 3, 2)))
                    G.Rows(e.RowIndex).Cells(GC.Size).Value = Integer.Parse(Val(Mid(BC, BC.Length - 1, 2)))
                    LoadItemPrice(e.RowIndex)
                    Exit Sub
                ElseIf Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                    G.Rows(e.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select Id from Items where IsStopped=0 and left(Barcode,12)='" & Val(BC) & "'")
                    AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
                    Exit Sub
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.Id Then
                AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.UnitId OrElse G.Columns(e.ColumnIndex).Name = GC.Size Then
                LoadItemPrice(e.RowIndex)
            End If
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            CalcRow(e.RowIndex)
        Catch ex As Exception
        End Try
    End Sub


    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        Dim str As String = ""
        If Flag = FlagState.المستهلكات AndAlso Not Md.Manager Then
            str = " where Flag=1"
        End If
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")" & str) Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        Dim str As String = ""
        If Flag = FlagState.المستهلكات AndAlso Not Md.Manager Then
            str = " and Flag=1"
        End If
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim() & str)
        ClearControls()
        If Md.ShowShiftForEveryStore Then
            dt = bm.ExecuteAdapter("select CurrentDate,CurrentShift from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
            If dt.Rows.Count > 0 Then
                DayDate.SelectedDate = dt.Rows(0)("CurrentDate")
                Shift.SelectedValue = dt.Rows(0)("CurrentShift")
            End If
        End If
    End Sub

    Private Sub SaveId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.LostFocus
        bm.LostFocus(SaveId, SaveName, "select Name+' [ '+cast(dbo.Bal0(AccNo,Id,getdate(),0,0,0) as nvarchar(100))+' ]' Name from Fn_EmpPermissions(5," & Md.UserName & ") where Id=" & SaveId.Text.Trim(), True)
    End Sub

    Private Sub SaveId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.KeyUp
        If bm.ShowHelp("Saves", SaveId, SaveName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(5," & Md.UserName & ")") Then
            SaveId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub ToId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ToId.KeyUp
        Dim Title, tbl As String
        If Flag = FlagState.تحويل_إلى_مخزن Then
            tbl = "Stores"
            Title = "المخازن"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
        ElseIf TestPurchaseAndReturn() Then
            tbl = "Suppliers"
            Title = "الموردين"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
        ElseIf TestSalesAndReturn() Then
            If bm.ShowHelpCustomers(ToId, ToName, e) Then
                ToId_LostFocus(sender, Nothing)
            End If
        ElseIf Flag = FlagState.المستهلكات Then
            If bm.ShowHelpCases(ToId, ToName, e) Then
                ToId_LostFocus(sender, Nothing)
            End If
        End If
    End Sub

    Function TestSalesAndReturn() As Boolean
        Return (Flag = FlagState.مبيعات_الصالة OrElse Flag = FlagState.مردودات_مبيعات_الصالة _
                OrElse Flag = FlagState.المبيعات OrElse Flag = FlagState.مردودات_المبيعات _
                OrElse Flag = FlagState.مبيعات_التوصيل OrElse Flag = FlagState.مردودات_مبيعات_التوصيل _
            OrElse Flag = FlagState.مبيعات_الجملة OrElse Flag = FlagState.مردودات_مبيعات_الجملة _
            OrElse Flag = FlagState.مبيعات_نصف_الجملة OrElse Flag = FlagState.مردودات_مبيعات_نصف_الجملة)
    End Function

    Function TestSalesOnly() As Boolean
        Return (Flag = FlagState.مبيعات_الصالة _
                OrElse Flag = FlagState.المبيعات _
                OrElse Flag = FlagState.مبيعات_التوصيل _
                OrElse Flag = FlagState.مبيعات_الجملة _
                OrElse Flag = FlagState.مبيعات_نصف_الجملة)
    End Function

    Function TestPurchaseAndReturn() As Boolean
        Return (Flag = FlagState.مشتريات OrElse Flag = FlagState.مردودات_مشتريات)
    End Function

    Function TestPurchaseOnly() As Boolean
        Return (Flag = FlagState.مشتريات)
    End Function

    Function TestDelivary() As Boolean
        Return (Flag = FlagState.مبيعات_التوصيل OrElse Flag = FlagState.مردودات_مبيعات_التوصيل)
    End Function

    Private Sub ToId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToId.LostFocus
        Dim tbl As String
        If Flag = FlagState.تحويل_إلى_مخزن Then
            tbl = "Stores"
        ElseIf TestPurchaseAndReturn() Then
            tbl = "Suppliers"
        ElseIf TestSalesAndReturn() Then
            tbl = "Customers"
        ElseIf Flag = FlagState.المستهلكات Then
            bm.LostFocus(ToId, ToName, "select " & Resources.Item("CboName") & " Name from Cases where Id=" & ToId.Text.Trim())
            ToId.ToolTip = ""
            ToName.ToolTip = ""
            Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile from Cases where Id=" & ToId.Text.Trim())
            If dt.Rows.Count > 0 Then
                ToId.ToolTip = Resources.Item("Id") & ": " & ToId.Text & vbCrLf & Resources.Item("Name") & ": " & ToName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
                ToName.ToolTip = ToId.ToolTip
            End If
            Return
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
    End Sub

    Private Sub WaiterId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles WaiterId.KeyUp
        bm.ShowHelp("الويترز", WaiterId, WaiterName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Waiter=1 and Stopped=0")
    End Sub

    Private Sub WaiterId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles WaiterId.LostFocus
        bm.LostFocus(WaiterId, WaiterName, "select EnName Name from Employees where Waiter=1 and Id=" & WaiterId.Text.Trim() & " and Stopped=0")
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


    Sub FillControls()
        If lop Then Return
        lop = True
        UndoNewId()
        bm.FillControls(Me)

        PaymentType_TextChanged(Nothing, Nothing)
        ToId_LostFocus(Nothing, Nothing)
        CashierId_LostFocus(Nothing, Nothing)
        WaiterId_LostFocus(Nothing, Nothing)
        DeliverymanId_LostFocus(Nothing, Nothing)
        TId_LostFocus(TableId, Nothing)
        TId_LostFocus(TableSubId, Nothing)
        TId_LostFocus(NoOfPersons, Nothing)

        AccNo1_LostFocus(Nothing, Nothing)
        AccNo2_LostFocus(Nothing, Nothing)
        AccNo3_LostFocus(Nothing, Nothing)
        AccNo4_LostFocus(Nothing, Nothing)

        Dim dt As DataTable = bm.ExecuteAdapter("select SD.* /*,It.Unit,It.UnitSub*/ from SalesDetails SD left join Items It on(SD.ItemId=It.Id) where SD.StoreId=" & StoreId.Text & " and SD.InvoiceNo=" & InvoiceNo.Text & " and SD.Flag=" & Flag)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.SalesInvoiceNo).Value = dt.Rows(i)("SalesInvoiceNo").ToString
            G.Rows(i).Cells(GC.Barcode).Value = dt.Rows(i)("Barcode").ToString
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("ItemName").ToString
            LoadItemUint(i)
            G.Rows(i).Cells(GC.Color).Value = dt.Rows(i)("Color")
            G.Rows(i).Cells(GC.Size).Value = dt.Rows(i)("Size")
            G.Rows(i).Cells(GC.UnitId).Value = dt.Rows(i)("UnitId")
            G.Rows(i).Cells(GC.UnitQty).Value = dt.Rows(i)("UnitQty").ToString
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
            G.Rows(i).Cells(GC.UnitSub).Value = "" 'dt.Rows(i)("UnitSub").ToString
            G.Rows(i).Cells(GC.QtySub).Value = 0 ' 'dt.Rows(i)("QtySub").ToString
            G.Rows(i).Cells(GC.PriceSub).Value = dt.Rows(i)("PriceSub").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Rows(i).Cells(GC.IsPrinted).Value = dt.Rows(i)("IsPrinted").ToString
            G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(i)("SalesPrice").ToString
            CalcRow(i)
            'If Not Md.Manager AndAlso TestSalesAndReturn() Then
            '    G.Rows(i).ReadOnly = True
            '    G.Rows(i).DefaultCellStyle.BackColor = System.Drawing.Color.PeachPuff
            '    btnDelete.IsEnabled = False
            'End If
        Next
        CalcTotal()
        Notes.Focus()
        G.RefreshEdit()
        lop = False
        CalcTotalEnd()
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

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click, btnPrint2.Click, btnPrint3.Click
        If Md.MyProjectType = ProjectType.X Then
            Dim c As New SalesRedFuckingPayment

            c.RdoCash.IsChecked = RdoCash.IsChecked
            c.RdoVisa.IsChecked = RdoVisa.IsChecked
            c.RdoCashVisa.IsChecked = RdoCashVisa.IsChecked
            c.RdoFuture.IsChecked = RdoFuture.IsChecked
            c.RdoCashFuture.IsChecked = RdoCashFuture.IsChecked
            c.RdoEmployees.IsChecked = RdoEmployees.IsChecked

            c.PaymentType.Text = PaymentType.Text
            c.CashValue.Text = CashValue.Text
            c.Total.Text = Total.Text
            c.DiscountPerc.Text = DiscountPerc.Text
            c.TotalAfterDiscount.Text = TotalAfterDiscount.Text
            c.DiscountValue.Text = DiscountValue.Text
            c.Payed.Text = Payed.Text
            c.Remaining.Text = Remaining.Text

            Dim frm As New MyWindow With {.Title = "الدفع", .SizeToContent = SizeToContent.WidthAndHeight}
            frm.Content = c
            frm.ShowDialog()
            If Not c.AllowPrint Then Return

            RdoCash.IsChecked = c.RdoCash.IsChecked
            RdoVisa.IsChecked = c.RdoVisa.IsChecked
            RdoCashVisa.IsChecked = c.RdoCashVisa.IsChecked
            RdoFuture.IsChecked = c.RdoFuture.IsChecked
            RdoCashFuture.IsChecked = c.RdoCashFuture.IsChecked
            RdoEmployees.IsChecked = c.RdoEmployees.IsChecked

            PaymentType.Text = c.PaymentType.Text
            CashValue.Text = c.CashValue.Text
            Total.Text = c.Total.Text
            DiscountPerc.Text = c.DiscountPerc.Text
            TotalAfterDiscount.Text = c.TotalAfterDiscount.Text
            DiscountValue.Text = c.DiscountValue.Text
            Payed.Text = c.Payed.Text
            Remaining.Text = c.Remaining.Text

        End If

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


        If ToId.Visibility = Visibility.Visible AndAlso ToId.Text.Trim = "" AndAlso Not TestSalesAndReturn() Then
            bm.ShowMSG("برجاء تحديد " & lblToId.Content)
            ToId.Focus()
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

        'If Not Md.Manager Then
        'DayDate.SelectedDate = bm.MyGetDate() 'Md.CurrentDate
        'Shift.SelectedValue = Md.CurrentShiftId
        'End If
        If Md.ShowShifts AndAlso Not Md.Manager Then
            DayDate.SelectedDate = Md.CurrentDate
            Shift.SelectedValue = Md.CurrentShiftId
        End If


        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & StoreId.Text & "'" & " and Flag=" & Flag)
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text
            
            
            
            State = BasicMethods.SaveState.Insert
        End If

        MinPerPerson.Text = Val(MinPerPerson.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                InvoiceNo.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G, "SalesDetails", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text}, New String() {"SalesInvoiceNo", "Barcode", "ItemId", "ItemName", "Color", "Size", "UnitId", "UnitQty", "Qty", "Price", "QtySub", "PriceSub", "Value", "IsPrinted", "SalesPrice"}, New String() {GC.SalesInvoiceNo, GC.Barcode, GC.Id, GC.Name, GC.Color, GC.Size, GC.UnitId, GC.UnitQty, GC.Qty, GC.Price, GC.QtySub, GC.PriceSub, GC.Value, GC.IsPrinted, GC.SalesPrice}, New VariantType() {VariantType.String, VariantType.String, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.Decimal}, New String() {GC.Id}) Then Return

        If State = BasicMethods.SaveState.Insert AndAlso TestPurchaseOnly() Then
            bm.ExecuteNonQuery("UpdateItemPurchasePrice", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text})
        End If

        Select Case CType(sender, Button).Name
            Case btnPrint.Name, btnPrint2.Name, btnPrint3.Name, btnPrint4.Name, btnPrint5.Name
                State = BasicMethods.SaveState.Print
            Case btnCloseTable.Name
                State = BasicMethods.SaveState.Close
        End Select

        TraceInvoice(State.ToString)

        'If TestSalesAndOnly() Then PrintPone(sender, 1)
        If sender Is btnPrint OrElse sender Is btnPrint2 OrElse sender Is btnPrint3 OrElse sender Is btnPrint4 OrElse sender Is btnPrint5 OrElse (sender Is btnCloseTable And btnPrint.IsEnabled) Then
            PrintPone(sender, 0)
            'txtID_Leave(Nothing, Nothing)
            '
            'Return
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

    Sub ClearControls()
        Try
            If looop Then Return
            NewId()
            Dim d As DateTime = Nothing
            Try
                If d.Year = 1 Then d = bm.MyGetDate
                d = DayDate.SelectedDate
            Catch ex As Exception
            End Try

            Dim s As String = Shift.SelectedValue.ToString
            Dim st As String = StoreId.Text
            Dim sv As String = SaveId.Text

            bm.ClearControls(False)

            Payed.Clear()
            Remaining.Clear()
            
            SaveId.Text = Md.DefaultSave
            If sv <> "" Then SaveId.Text = sv

            If Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X Then CashierId.Text = Md.UserName

            Dim dt As DataTable = bm.ExecuteAdapter("select S_AccNo,R_S_AccNo,P_AccNo,R_P_AccNo from Statics")
            Select Case Flag
                Case FlagState.مشتريات
                    AccNo.Text = dt.Rows(0)("P_AccNo")
                Case FlagState.مردودات_مشتريات
                    AccNo.Text = dt.Rows(0)("R_P_AccNo")
                Case FlagState.المبيعات, FlagState.مبيعات_التوصيل, FlagState.مبيعات_الصالة, FlagState.مبيعات_الجملة, FlagState.مبيعات_نصف_الجملة, FlagState.المستهلكات
                    AccNo.Text = dt.Rows(0)("S_AccNo")
                Case FlagState.مردودات_المبيعات, FlagState.مردودات_مبيعات_التوصيل, FlagState.مردودات_مبيعات_الصالة, FlagState.مردودات_مبيعات_الجملة, FlagState.مردودات_مبيعات_نصف_الجملة
                    AccNo.Text = dt.Rows(0)("R_S_AccNo")
            End Select


            SaveId_LostFocus(Nothing, Nothing)
            CashierId_LostFocus(Nothing, Nothing)
            ToId_LostFocus(Nothing, Nothing)
            AccNo_LostFocus(Nothing, Nothing)
            WaiterId_LostFocus(Nothing, Nothing)
            DeliverymanId_LostFocus(Nothing, Nothing)
            TId_LostFocus(TableId, Nothing)
            TId_LostFocus(TableSubId, Nothing)
            TId_LostFocus(NoOfPersons, Nothing)

            If TestPurchaseAndReturn() Then

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

            ElseIf TestSalesAndReturn() Then

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

            End If

            AccNo1_LostFocus(Nothing, Nothing)
            AccNo2_LostFocus(Nothing, Nothing)
            AccNo3_LostFocus(Nothing, Nothing)
            AccNo4_LostFocus(Nothing, Nothing)


            Temp.Visibility = Visibility.Visible
            If (Md.MyProjectType = ProjectType.X) AndAlso TestSalesOnly() Then
                Temp.Content = "فاتورة حجز"
                Temp.Visibility = Visibility.Hidden
            ElseIf (Md.MyProjectType = ProjectType.X) AndAlso TestPurchaseOnly() Then
                Temp.Content = "فاتورة معلقة"
                If Not Md.Manager Then Temp.IsEnabled = False
                If Md.Receptionist AndAlso Md.MyProjectType = ProjectType.X Then
                    G.Columns(GC.Price).Visible = False
                    G.Columns(GC.Value).Visible = False
                    G.Columns(GC.SalesPrice).Visible = False

                End If

            Else
                Temp.Visibility = Visibility.Hidden
            End If

            PaymentType.Text = 1

            If Not Md.Manager Then
                DayDate.SelectedDate = bm.MyGetDate() 'Md.CurrentDate
                Shift.SelectedValue = Md.CurrentShiftId
                If Md.ShowShifts Then
                    DayDate.SelectedDate = Md.CurrentDate
                    Shift.SelectedValue = Md.CurrentShiftId
                End If


                CashierId.Text = Md.UserName
                CashierId_LostFocus(Nothing, Nothing)
            Else
                DayDate.SelectedDate = d
                Shift.SelectedValue = s
            End If

            'DayDate.SelectedDate = bm.MyGetDate()
            'Shift.SelectedValue = Md.CurrentShiftId

            StoreId.Text = st

            txtFlag.Text = Flag

            G.Rows.Clear()
            CalcTotal()
            'InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & StoreId.Text & "'" & " and Flag=" & Flag)
            'If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"

            If TableSubId.Visibility = Visibility.Visible Then TableSubId.Text = 1
            If NoOfPersons.Visibility = Visibility.Visible Then NoOfPersons.Text = 1

            WithService.IsChecked = (WithService.Visibility = Visibility.Visible)
            WithTax.IsChecked = (WithTax.Visibility = Visibility.Visible)
        Catch
        End Try
        If Flag = FlagState.مبيعات_الصالة Then TabControl1.SelectedItem = TabItemTables


        If (Md.MyProjectType = ProjectType.X) AndAlso TestPurchaseOnly() AndAlso Not Md.Manager Then Temp.IsChecked = True
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            TraceInvoice("Deleted")
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)

            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)

            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , 0, True)
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , 0, True)
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
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyDown, InvoiceNo.KeyDown, ToId.KeyDown, WaiterId.KeyDown, TableId.KeyDown, TableSubId.KeyDown, NoOfPersons.KeyDown, txtID.KeyDown, CashierId.KeyDown, DeliverymanId.KeyDown, AccNo1.KeyDown, AccNo2.KeyDown, AccNo3.KeyDown, AccNo4.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Taxvalue.KeyDown, ServiceValue.KeyDown, MinPerPerson.KeyDown, CashValue.KeyDown, DiscountPerc.KeyDown, DiscountValue.KeyDown, txtPrice.KeyDown, Per1.KeyDown, Per2.KeyDown, Per3.KeyDown, Per4.KeyDown, Val1.KeyDown, Val2.KeyDown, Val3.KeyDown, Val4.KeyDown
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
        If ForceSales OrElse TestSalesAndReturn() Then
            str = "Sales" & str
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
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Shift", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "@InvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "Header", "Remaining", "Payed"}

        If NewItemsOnly = 0 Then
            rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, Shift.SelectedValue.ToString, Flag, StoreId.Text, InvoiceNo.Text, InvoiceNo.Text, InvoiceNo.Text, NewItemsOnly, 0, 0, 0, CType(Parent, Page).Title, Val(Remaining.Text), Val(Payed.Text)}
            rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "SalesPone_N.rpt", "SalesPone.rpt")
            'If sender Is btnPrint2 Then rpt.Rpt = "SalesPone2.rpt"
            If sender Is btnPrint3 Then rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "SalesPone3_N.rpt", "SalesPone3.rpt")
            If sender Is btnPrint4 Then rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "SalesPone4_N.rpt", "SalesPone4.rpt")

            If sender Is btnPrint2 Then
                Dim i As Integer = 1
                rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "SalesPone2_N.rpt", "SalesPone2.rpt")
                If Md.MyProjectType = ProjectType.X Then
                    'rpt.Rpt = "SalesPone2.rpt"
                    i = 2
                End If
                rpt.Print(".", Md.PonePrinter, i)
                'rpt.Show()
                Return
            End If

            If sender Is btnPrint5 Then
                rpt.Rpt = "PrintBarcodeFromSalesDetails.rpt"
                If Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "PrintBarcodeFromSalesDetailsSizeColor.rpt"
                End If
                rpt.Print(".", Md.BarcodePrinter, 1)
                'rpt.Show()
                Return
            End If

            If TestSalesOnly() OrElse TestPurchaseOnly() Then
                rpt.Print(, , 1)
            Else
                rpt.Print(, , 2)
            End If

            'rpt.Show()
        ElseIf Not TestSalesAndReturn() Then
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
            If Md.Receptionist AndAlso Md.MyProjectType = ProjectType.X Then
                txtPrice.Visibility = Visibility.Hidden
            End If
        Catch
        End Try
    End Sub


    Dim LopCalc As Boolean = False
    Private Sub CalcTotal() Handles Total.TextChanged, DiscountPerc.TextChanged, DiscountValue.TextChanged, Taxvalue.TextChanged, ServiceValue.TextChanged, MinPerPerson.TextChanged, NoOfPersons.TextChanged, WithTax.Checked, WithTax.Unchecked, WithService.Checked, WithService.Unchecked, CancelMinPerPerson.Checked, CancelMinPerPerson.Unchecked, ToId.LostFocus
        If LopCalc Or lop Then Return
        Try
            LopCalc = True
            MinPerPerson.Text = Math.Round(0, 2)
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

    Sub CalcTotalEnd() Handles Per1.TextChanged, Per2.TextChanged, Per3.TextChanged, Per4.TextChanged, AccType1.SelectionChanged, AccType2.SelectionChanged, AccType3.SelectionChanged, AccType4.SelectionChanged
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
        TotalAfterDiscount.Text = Val(Total.Text) - Val(DiscountValue.Text) + Val(Taxvalue.Text) + Val(ServiceValue.Text) + d1 + d2 + d3 + d4

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

    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Name).Index Then
                Dim str As String = "select cast(Id as varchar(100)) Id,Name," & PriceFieldName(GC.Price, 0) & " 'السعر' from Items where IsStopped=0 " & ItemWhere()
                If Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.SalesInvoiceNo).Value) > 0 Then
                    str = "select cast(ItemId as varchar(100)) Id,ItemName Name,Price 'السعر' from SalesDetails where StoreId=" & StoreId.Text & " and Flag=" & Flag - 1 & " and InvoiceNo=" & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.SalesInvoiceNo).Value)
                End If
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, str) Then
                    AddItem(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value, G.CurrentCell.RowIndex, 0)
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId)
                    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color)
                    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                    End If

                End If
            End If

            If bm.ShowHelpGridItemBal(G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "GetItemCurrentBal " & Val(G.CurrentRow.Cells(GC.Id).Value)) Then
                AddItem(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value, G.CurrentCell.RowIndex, 0)
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
        PanelItems.Margin = New Thickness(PanelItems.Margin.Left, PanelItems.Margin.Top, PanelItems.Margin.Right, 8)
        HelpGD.Margin = New Thickness(HelpGD.Margin.Left, HelpGD.Margin.Top, HelpGD.Margin.Right, 8)
        GAcc.Visibility = Visibility.Hidden

        GroupBoxPaymentType.Visibility = Visibility.Hidden
    End Sub

    Private Sub AccNo1_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo1.KeyUp
        bm.AccNoShowHelp(AccNo1, AccName1, e, , , )
    End Sub
    Private Sub AccNo2_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo2.KeyUp
        bm.AccNoShowHelp(AccNo2, AccName2, e, , , )
    End Sub
    Private Sub AccNo3_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo3.KeyUp
        bm.AccNoShowHelp(AccNo3, AccName3, e, , , )
    End Sub
    Private Sub AccNo4_KeyUp(sender As Object, e As KeyEventArgs) Handles AccNo4.KeyUp
        bm.AccNoShowHelp(AccNo4, AccName4, e, , , )
    End Sub

    Private Sub AccNo1_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo1.LostFocus
        bm.AccNoLostFocus(AccNo1, AccName1, , , )
    End Sub
    Private Sub AccNo2_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo2.LostFocus
        bm.AccNoLostFocus(AccNo2, AccName2, , , )
    End Sub
    Private Sub AccNo3_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo3.LostFocus
        bm.AccNoLostFocus(AccNo3, AccName3, , , )
    End Sub
    Private Sub AccNo4_LostFocus(sender As Object, e As RoutedEventArgs) Handles AccNo4.LostFocus
        bm.AccNoLostFocus(AccNo4, AccName4, , , )
    End Sub

    Private Sub LoadItemUint(i As Integer)
        Dim Id As Integer = Val(G.Rows(i).Cells(GC.Id).Value)
        'Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items where Id='" & Id & "' and " & ItemWhere() & "")

        bm.FillCombo("select 0 Id,Unit Name From Items where Id='" & Id & "' " & ItemWhere() & " union select 1 Id,UnitSub Name From Items where Id='" & Id & "' " & ItemWhere() & " union select 2 Id,UnitSub2 Name From Items where Id='" & Id & "' " & ItemWhere() & "", G.Rows(i).Cells(GC.UnitId))

        bm.FillCombo("select 0 Id,'-' Name union select Id,Name from ColorsDetails where ColorId=(select It.ColorId from Items It where It.Id='" & Id & "' " & ItemWhere() & ") order by Id", G.Rows(i).Cells(GC.Color))

        bm.FillCombo("select 0 Id,'-' Name union select Id,Name from SizesDetails where SizeId=(select It.SizeId from Items It where It.Id='" & Id & "' " & ItemWhere() & ") order by Id", G.Rows(i).Cells(GC.Size))


        If G.Rows(i).Cells(GC.UnitId).Value Is Nothing Then G.Rows(i).Cells(GC.UnitId).Value = 0
        If G.Rows(i).Cells(GC.Color).Value Is Nothing Then G.Rows(i).Cells(GC.Color).Value = 0
        If G.Rows(i).Cells(GC.Size).Value Is Nothing Then G.Rows(i).Cells(GC.Size).Value = 0
    End Sub

    Private Sub LoadItemPrice(i As Integer)
        Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items where Id='" & G.Rows(i).Cells(GC.Id).Value & "' " & ItemWhere())
        If dt.Rows.Count = 0 Then Return
        G.Rows(i).Cells(GC.Price).Value = dt.Rows(0)(PriceFieldName(GC.Price, G.Rows(i).Cells(GC.UnitId).Value))
        G.Rows(i).Cells(GC.UnitQty).Value = UnitCount(dt, G.Rows(i).Cells(GC.UnitId).Value)
        G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(0)(PriceFieldName(GC.Price, G.Rows(i).Cells(GC.UnitId).Value, True))
        If TestSalesAndReturn() AndAlso Val(G.Rows(i).Cells(GC.Size).Value) > 0 Then
            dt = bm.ExecuteAdapter("Select * From ItemSizes where ItemId='" & G.Rows(i).Cells(GC.Id).Value & "' and Id='" & G.Rows(i).Cells(GC.Size).Value & "'")
            If dt.Rows.Count = 0 Then Return
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(0)("SalesPrice")
        End If
        If TestPurchaseOnly() AndAlso Val(G.Rows(i).Cells(GC.Size).Value) > 0 Then
            dt = bm.ExecuteAdapter("Select * From ItemSizes where ItemId='" & G.Rows(i).Cells(GC.Id).Value & "' and Id='" & G.Rows(i).Cells(GC.Size).Value & "'")
            If dt.Rows.Count = 0 Then Return
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(0)("PurchasePrice")
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

        If Val(G.Rows(i).Cells(GC.SalesInvoiceNo).Value) > 0 Then
            Dim s As String = ""
            Select Case Flag
                Case FlagState.مردودات_المبيعات
                    s = FlagState.المبيعات
                Case FlagState.مردودات_مبيعات_التوصيل
                    s = FlagState.مبيعات_التوصيل
                Case FlagState.مردودات_مبيعات_الجملة
                    s = FlagState.مبيعات_الجملة
                Case FlagState.مردودات_مبيعات_الصالة
                    s = FlagState.مبيعات_الصالة
                Case FlagState.مردودات_مبيعات_نصف_الجملة
                    s = FlagState.مبيعات_نصف_الجملة
                Case FlagState.مردودات_مشتريات
                    s = FlagState.مشتريات
            End Select

            dt = bm.ExecuteAdapter("select Price from SalesDetails where StoreId=" & StoreId.Text & " and Flag=" & s & " and InvoiceNo=" & G.Rows(i).Cells(GC.SalesInvoiceNo).Value & " and ItemId=" & G.Rows(i).Cells(GC.Id).Value)
            If dt.Rows.Count = 0 Then
                bm.ShowMSG("هذا الصنف غير موجود بالفاتورة")
                ClearRow(i)
                Return
            End If
            Dim x As Decimal = Val(dt.Rows(0)(0))
            If x > 0 Then G.Rows(i).Cells(GC.Price).Value = x
        End If

    End Sub


    Private Sub btnPrint4_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint4.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub ComboBox1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ComboBox1.SelectionChanged
        If ComboBox1.SelectedIndex = -1 Then Return
        Flag = ComboBox1.SelectedValue
        txtFlag.Text = ComboBox1.SelectedValue
        CType(Parent, Page).Title = CType(ComboBox1.ItemsSource, DataView).Item(ComboBox1.SelectedIndex)("Name")
        Md.Currentpage = CType(ComboBox1.ItemsSource, DataView).Item(ComboBox1.SelectedIndex)("Name")
        btnNew_Click(Nothing, Nothing)
    End Sub

    Dim looop As Boolean = False
    Private Sub LoadCbo()
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

    Private Sub btnPrint5_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint5.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub Payed_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Payed.TextChanged, TotalAfterDiscount.TextChanged
        Remaining.Clear()
        If Val(Payed.Text) = 0 Then Return
        Remaining.Text = Val(Payed.Text) - IIf(Val(CashValue.Text) > 0, Val(CashValue.Text), Val(TotalAfterDiscount.Text))
    End Sub

    Private Sub btnRes_Click(sender As Object, e As RoutedEventArgs) Handles btnRes.Click
        ComboBox1.SelectedValue = FlagState.المبيعات
        Flag = ComboBox1.SelectedValue
        txtFlag.Text = ComboBox1.SelectedValue
        LoadWFH()
        CType(Parent, Page).Title = sender.Content
        Md.Currentpage = sender.Content
        btnNew_Click(Nothing, Nothing)
        Temp.IsChecked = True
        RdoCashFuture.IsChecked = True
    End Sub

    Private Sub btnSalesReturn_Click(sender As Object, e As RoutedEventArgs) Handles btnSalesReturn.Click
        ComboBox1.SelectedValue = FlagState.مردودات_المبيعات
        Flag = ComboBox1.SelectedValue
        txtFlag.Text = ComboBox1.SelectedValue
        LoadWFH()
        CType(Parent, Page).Title = CType(ComboBox1.ItemsSource, DataView).Item(ComboBox1.SelectedIndex)("Name")
        Md.Currentpage = CType(ComboBox1.ItemsSource, DataView).Item(ComboBox1.SelectedIndex)("Name")
        btnNew_Click(Nothing, Nothing)
        Temp.IsChecked = False
    End Sub

    Dim lopNew As Boolean = False
    Private Sub btnSales_Click(sender As Object, e As RoutedEventArgs) Handles btnSales.Click
        If lopNew Then Return
        lopNew = True
        ComboBox1.SelectedValue = FlagState.المبيعات
        Flag = ComboBox1.SelectedValue
        txtFlag.Text = ComboBox1.SelectedValue
        LoadWFH()
        CType(Parent, Page).Title = CType(ComboBox1.ItemsSource, DataView).Item(ComboBox1.SelectedIndex)("Name")
        Md.Currentpage = CType(ComboBox1.ItemsSource, DataView).Item(ComboBox1.SelectedIndex)("Name")
        btnNew_Click(Nothing, Nothing)
        Temp.IsChecked = False
        lopNew = False
    End Sub

    Private Sub btnResSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnResSearch.Click
        If bm.ShowHelp("الفواتير المحجوزة", InvoiceNo, InvoiceNo, Nothing, "select cast(InvoiceNo as varchar(100))InvoiceNo,dbo.GetCustomerName(ToId)CustomerName from SalesMaster where StoreId='" & StoreId.Text & "' and Flag=13 and Temp=1 and (ToId=" & Val(ToId.Text) & " or " & Val(ToId.Text) & "=0)") Then
            txtID_Leave(Nothing, Nothing)
        End If
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

    Private Sub Temp_Checked(sender As Object, e As RoutedEventArgs) Handles Temp.Checked, Temp.Unchecked
        If Temp.IsChecked Then
            btnResSearch.Visibility = Visibility.Visible
            btnResEnd.Visibility = Visibility.Visible
            btnDelete.Visibility = Visibility.Visible
        Else
            btnResSearch.Visibility = Visibility.Hidden
            btnResEnd.Visibility = Visibility.Hidden
            If Not Md.Manager Then
                btnDelete.Visibility = Visibility.Hidden
            End If
        End If
    End Sub

    Private Sub btnResEnd_Click(sender As Object, e As RoutedEventArgs) Handles btnResEnd.Click
        Temp.IsChecked = False
        RdoCash.IsChecked = True
        btnPrint_Click(btnPrint2, e)
    End Sub

    Private Sub btnCloseShift_Click(sender As Object, e As RoutedEventArgs) Handles btnCloseShift.Click
        If bm.ShowDeleteMSG("إغلاق الوردية لا يمكنك من إعادة فتحها مرة أخرى" & vbCrLf & vbCrLf & "هل أنت متأكد من إغلاق الوردية؟") Then

            If Md.ShowShiftForEveryStore Then
                dt = bm.ExecuteAdapter("CloseShiftForEveryStore", New String() {"StoreId"}, New String() {StoreId.Text})
            Else
                bm.ExecuteNonQuery("exec CloseShift")
            End If

            'bm.ShowMSG("تم إغلاق الوردية")


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


End Class
