Imports System.Data

Public Class RPT6
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Detail As Integer = 0
    Dim IsCalculated = False

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        If ComboBox1.SelectedIndex < 0 Then ComboBox1.SelectedIndex = 0
        If Detail = 7 OrElse Detail = 8 OrElse Detail = 18 OrElse Detail = 19 Then
            If StoreId.Visibility = Visibility.Visible AndAlso StoreId.Text.Trim = "" Then
                bm.ShowMSG("برجاء تحديد المخزن")
                StoreId.Focus()
                Return
            End If
            If ItemId.Visibility = Visibility.Visible AndAlso ItemId.Text.Trim = "" Then
                bm.ShowMSG("برجاء تحديد الصنف")
                ItemId.Focus()
                Return
            End If
        End If

        If Not IsCalculated AndAlso BtnCalc.Visibility = Visibility.Visible AndAlso bm.ShowDeleteMSG("هل تريد احتساب تكلفة الأصناف؟") Then
            BtnCalc_Click(BtnCalc, Nothing)
            IsCalculated = True
            Return
        End If

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@ToDate2", "@Shift", "ShiftName", "@Flag", "@StoreId", "StoreName", "@FromInvoiceNo", "@ToInvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "@CashierId", "@IsClosedOnly", "Header", "@ToId", "@ItemId", "@ColorId", "ColorName", "@SizeId", "SizeName", "@SaveId", "ItemName", "@CountryId", "CountryName", "@GroupId", "GroupName", "@TypeId", "TypeName", "@WaiterId", "@SalesTypeId", "@Canceled", "@IsService", "@CostCenterId", "@ItemSerialNo"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, ToDate.SelectedDate, Shift.SelectedValue.ToString, Shift.Text, ComboBox1.SelectedValue.ToString, Val(StoreId.Text), StoreName.Text, Val(FromInvoice.Text), Val(ToInvoice.Text), 0, Flag, ComboBox1.SelectedValue.ToString, 0, Val(CashierId.Text), IIf(IsClosedOnly.IsChecked, 1, 0), CType(Parent, Page).Title, Val(ToId.Text), Val(ItemId.Text), 0, "", 0, "", 0, ItemName.Text, Val(CountryId.Text), CountryName.Text, Val(GroupId.Text), GroupName.Text, Val(TypeId.Text), TypeName.Text, Val(WaiterId.Text), SalesTypeId.SelectedValue, Canceled.SelectedIndex.ToString, IsService.SelectedIndex.ToString, CostCenterId.SelectedValue, ItemSerialNo.Text.Trim}

        Select Case Detail
            Case 0
                rpt.Rpt = "Sales2.rpt"
                If Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "Sales22.rpt"
                ElseIf Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "Sales2_O.rpt"
                ElseIf Md.MyProjectType = ProjectType.Zohor Then
                    rpt.Rpt = "Sales2_Z.rpt"
                ElseIf Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "Sales2_P.rpt"
                    If Flag = 8 OrElse Flag = 21 Then
                        rpt.Rpt = "Sales2.rpt"
                    End If
                End If
            Case 1
                rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "Sales_N.rpt", "Sales.rpt")
                If Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "SalesColorSize.rpt"
                ElseIf Md.MyProjectType = ProjectType.Zohor Then
                    rpt.Rpt = "Sales_Z.rpt"
                ElseIf Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "Sales_P.rpt"
                    If Flag = 8 OrElse Flag = 21 Then
                        rpt.Rpt = "Sales_P2.rpt"
                    End If
                End If
            Case 2
                rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "DeletedSales_N.rpt", "DeletedSales.rpt")
            Case 3
                rpt.Rpt = "ItemsSales5.rpt"
            Case 4
                rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "SalesProfit_N.rpt", "SalesProfit.rpt")
                rpt.Rpt = IIf(Md.MyProjectType = ProjectType.X, "SalesProfit_R.rpt", "SalesProfit.rpt")
            Case 5
                rpt.Rpt = "SalesPone22.rpt"
            Case 6
                rpt.Rpt = "ItemsSales4.rpt"
            Case 7
                rpt.Rpt = "ItemMotion2.rpt"
                If Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "ItemMotion2ColorSize.rpt"
                ElseIf Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "ItemMotion2_N.rpt"
                End If
            Case 8
                rpt.Rpt = "ItemMotion3.rpt"
            Case 9
                rpt.Rpt = "Sales3.rpt"
            Case 10
                rpt.Rpt = "ItemsSales6.rpt"
            Case 11
                rpt.Rpt = "ItemsSales7.rpt"
            Case 12
                rpt.Rpt = "ItemsSales8.rpt"
            Case 13
                rpt.Rpt = "ItemsSales9.rpt"
            Case 14
                rpt.Rpt = "ItemsSales51.rpt"
            Case 15
                rpt.Rpt = "ItemsSales52.rpt"
            Case 16
                rpt.Rpt = "ItemCollectionMotion1.rpt"
            Case 17
                rpt.Rpt = "ItemCollectionMotion2.rpt"
            Case 18
                rpt.Rpt = "ItemMotion22.rpt"
            Case 19
                rpt.Rpt = "ItemMotion23.rpt"
            Case 20
                rpt.Rpt = "Sales_APS.rpt"
            Case 21
                rpt.Rpt = "Sales4.rpt"
            Case 22
                rpt.Rpt = "Sales_N2.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.FillCombo("Shifts", Shift, "")
        bm.FillCombo("SalesTypes", SalesTypeId, "", , True)

        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from CostCenters where SubType=1 union all select 0 Id,'-' Name order by Id", CostCenterId)

        lblCostCenterId.Visibility = IIf(Md.ShowCostCenter, Visibility.Visible, Visibility.Hidden)
        CostCenterId.Visibility = IIf(Md.ShowCostCenter, Visibility.Visible, Visibility.Hidden)

        Canceled.SelectedIndex = 0
        lblCanceled.Visibility = Visibility.Hidden
        Canceled.Visibility = Visibility.Hidden

        IsService.SelectedIndex = 2

        If Not (Md.MyProjectType = ProjectType.Zohor AndAlso (Detail = 0 OrElse Detail = 1)) Then
            lblIsService.Visibility = Visibility.Hidden
            IsService.Visibility = Visibility.Hidden
        End If

        'Select Case Detail
        '    Case 0
        '    Case Else
        '        lblCanceled.Visibility = Visibility.Hidden
        '        Canceled.Visibility = Visibility.Hidden
        'End Select

        bm.Addcontrol_MouseDoubleClick({CountryId, GroupId, TypeId, ItemId, StoreId, ToId, CashierId, WaiterId})

        lblCashier.Visibility = Visibility.Hidden
        CashierId.Visibility = Visibility.Hidden
        CashierName.Visibility = Visibility.Hidden

        lblWaiter.Visibility = Visibility.Hidden
        WaiterId.Visibility = Visibility.Hidden
        WaiterName.Visibility = Visibility.Hidden

        SalesTypeId.Visibility = Visibility.Hidden

        If (Flag = 3 OrElse Flag = 30 OrElse Flag = 9 OrElse Flag = 31 OrElse Flag = 28) AndAlso (Detail = 0 OrElse Detail = 1) Then
            lblCashier.Visibility = Visibility.Visible
            CashierId.Visibility = Visibility.Visible
            CashierName.Visibility = Visibility.Visible

            lblWaiter.Visibility = Visibility.Visible
            WaiterId.Visibility = Visibility.Visible
            WaiterName.Visibility = Visibility.Visible

            If Md.MyProjectType = ProjectType.X AndAlso (Flag = 3 OrElse Flag = 30) Then
                SalesTypeId.Visibility = Visibility.Visible
            End If

        End If

        If Flag = 5 OrElse Flag = 6 Then
            Label2.Visibility = Visibility.Hidden
            ComboBox1.Visibility = Visibility.Hidden
            IsClosedOnly.Visibility = Visibility.Hidden
        End If

        If Flag = 6 Then
            lblItemId.Visibility = Visibility.Hidden
            ItemId.Visibility = Visibility.Hidden
            ItemName.Visibility = Visibility.Hidden
            Image1.Visibility = Visibility.Hidden
        End If

        lblShift.Visibility = Visibility.Hidden
        Shift.Visibility = Visibility.Hidden

        LoadCbo()

        Select Case Detail
            Case 11
                lblToId.Visibility = Visibility.Hidden
                ToId.Visibility = Visibility.Hidden
                ToName.Visibility = Visibility.Hidden

            Case 12, 13
                lblItemId.Visibility = Visibility.Hidden
                ItemId.Visibility = Visibility.Hidden
                ItemName.Visibility = Visibility.Hidden
                Image1.Visibility = Visibility.Hidden

                lblToId.Visibility = Visibility.Hidden
                ToId.Visibility = Visibility.Hidden
                ToName.Visibility = Visibility.Hidden
            Case 16, 17
                lblCountryId.Visibility = Visibility.Hidden
                CountryId.Visibility = Visibility.Hidden
                CountryName.Visibility = Visibility.Hidden

                lblGroupId.Visibility = Visibility.Hidden
                GroupId.Visibility = Visibility.Hidden
                GroupName.Visibility = Visibility.Hidden

                lblTypeId.Visibility = Visibility.Hidden
                TypeId.Visibility = Visibility.Hidden
                TypeName.Visibility = Visibility.Hidden

                lblItemId.Visibility = Visibility.Hidden
                ItemId.Visibility = Visibility.Hidden
                ItemName.Visibility = Visibility.Hidden
                Image1.Visibility = Visibility.Hidden

                lblToId.Visibility = Visibility.Hidden
                ToId.Visibility = Visibility.Hidden
                ToName.Visibility = Visibility.Hidden

                Label2.Visibility = Visibility.Hidden
                ComboBox1.Visibility = Visibility.Hidden

            Case 22
                lblDayDate.Visibility = Visibility.Hidden
                FromDate.Visibility = Visibility.Hidden
                Label1.Visibility = Visibility.Hidden
                ToDate.Visibility = Visibility.Hidden

        End Select

        Dim MyNow As DateTime = bm.MyGetDate()
        Shift.SelectedValue = 0
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate

        StoreId.Text = ""
        StoreId_LostFocus(Nothing, Nothing)

        If Not (Detail = 4 OrElse Detail = 7 OrElse Detail = 13 OrElse Detail = 18) Then
            BtnCalc.Visibility = Visibility.Hidden
        End If
        If Not (Flag = 10 And Detail = 1) Then
            ToDate2.Visibility = Visibility.Hidden
            lblToDate2.Visibility = Visibility.Hidden
        End If

        If Not Md.ShowItemSerialNo Then
            lblItemSerialNo.Visibility = Visibility.Hidden
            ItemSerialNo.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub


    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub

    Private Sub LoadCbo()
        Dim dt As New DataTable("tbl")
        dt.Columns.Add("Id")
        dt.Columns.Add("Name")
        dt.Rows.Add(New String() {0, "الكل"})
        Select Case Flag
            Case 1
                dt.Rows.Add(New String() {1, "أرصدة افتتاحية"})
                dt.Rows.Add(New String() {2, "إضافة"})
                dt.Rows.Add(New String() {3, "تسوية إضافة"})
                dt.Rows.Add(New String() {4, "صرف"})
                dt.Rows.Add(New String() {5, "تسوية صرف"})
                dt.Rows.Add(New String() {6, "هدايا"})
                dt.Rows.Add(New String() {7, "هالك"})
                dt.Rows.Add(New String() {8, "تحويل إلى مخزن"})

                lblToId.Visibility = Visibility.Hidden
                ToId.Visibility = Visibility.Hidden
                ToName.Visibility = Visibility.Hidden
                lblToId.Content = "المخزن المحول إليه"

                lblCashier.Content = "المستلم"

            Case 2, 21

                If Flag = 2 Then
                    dt.Rows.Add(New String() {9, "مشتريات"})
                    dt.Rows.Add(New String() {10, "مردودات مشتريات"})
                Else
                    If Md.MyProjectType = ProjectType.X Then
                        dt.Rows.Add(New String() {29, "مشتريات مباشرة"})
                        dt.Rows.Add(New String() {30, "مردودات مشتريات مباشرة"})
                    Else
                        dt.Rows.Add(New String() {29, "مشتريات خارجية"})
                        dt.Rows.Add(New String() {30, "مردودات مشتريات خارجية"})
                    End If
                End If


                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "المورد"

                lblCashier.Content = "الطالب"

            Case 7, 10
                dt.Rows.Add(New String() {19, "الاستيراد"})
                dt.Rows.Add(New String() {20, "مردودات الاستيراد"})

                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "المورد"

                lblCashier.Content = "الطالب"

            Case 3, 30
                'dt.Rows.Add(New String() {11, "مبيعات الصالة"})
                'dt.Rows.Add(New String() {12, "مردودات مبيعات الصالة"})
                'dt.Rows.Add(New String() {13, "مبيعات التيك أواى"})
                'dt.Rows.Add(New String() {14, "مردودات مبيعات التيك أواى"})
                'dt.Rows.Add(New String() {15, "مبيعات التوصيل"})
                'dt.Rows.Add(New String() {16, "مردودات مبيعات التوصيل"})
                'IsClosedOnly.Visibility = Visibility.Visible
                dt.Rows.Add(New String() {13, "المبيعات"})
                dt.Rows.Add(New String() {14, "مردودات المبيعات"})

                If Md.MyProjectType = ProjectType.X Then
                    dt.Rows.Add(New String() {21, "مبيعات الجملة"})
                    dt.Rows.Add(New String() {22, "مردودات مبيعات الجملة"})
                End If


                If 1 = 2 Then
                    dt.Rows.Add(New String() {23, "مبيعات نصف الجملة"})
                    dt.Rows.Add(New String() {24, "مردودات مبيعات نصف الجملة"})
                End If

                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "العميل"

                lblCashier.Content = "البائع"

            Case 4
                dt.Rows.Add(New String() {17, "المستهلكات"})
                dt.Rows.Add(New String() {18, "مردودات المستهلكات"})
                dt.Rows.Add(New String() {37, "مستهلكات الداخلي"})
                dt.Rows.Add(New String() {38, "مردودات مستهلكات الداخلي"})
                dt.Rows.Add(New String() {47, "مستهلكات العمليات"})
                dt.Rows.Add(New String() {48, "مردودات مستهلكات العمليات"})
                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "المريض"

                lblCashier.Content = "الممرضة"

            Case 5
                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "العميل"

                lblCashier.Content = "البائع"

            Case 8

                If Md.MyProjectType = ProjectType.X Then
                    dt.Rows.Add(New String() {33, "مبيعات مباشرة"})
                    dt.Rows.Add(New String() {34, "مردودات مبيعات مباشرة"})
                Else
                    dt.Rows.Add(New String() {33, "التصدير"})
                    dt.Rows.Add(New String() {34, "مردودات التصدير"})
                End If

            Case 9
                dt.Rows.Add(New String() {26, "عرض أسعار"})

                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "العميل"

                lblCashier.Content = "البائع"

            Case 11
                If Md.MyProjectType = ProjectType.X Then
                    dt.Rows.Add(New String() {35, "مبيعات لمستثمر"})
                    dt.Rows.Add(New String() {36, "مردودات مبيعات لمستثمر"})
                End If

            Case 21
                If Md.MyProjectType = ProjectType.X Then
                    dt.Rows.Add(New String() {29, "مشتريات مباشرة"})
                    dt.Rows.Add(New String() {30, "مردودات مشتريات مباشرة"})
                End If


            Case 28
                dt.Rows.Add(New String() {28, "عرض أسعار مورد"})

                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "المورد"

                lblCashier.Content = "الطالب"

            Case 31
                dt.Rows.Add(New String() {31, "أمر توريد"})

                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "العميل"

                lblCashier.Content = "البائع"
        End Select


        Select Case Flag
            Case 2, 3, 30, 9, 28, 4, 10, 8, 21, 31, 7
            Case Else
                lblToId.Visibility = Visibility.Hidden
                ToId.Visibility = Visibility.Hidden
                ToName.Visibility = Visibility.Hidden
        End Select

        Dim dv As New DataView
        dv.Table = dt
        ComboBox1.ItemsSource = dv
        ComboBox1.SelectedValuePath = "Id"
        ComboBox1.DisplayMemberPath = "Name"
        ComboBox1.SelectedIndex = 0
    End Sub

    Private Sub ComboBox1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ComboBox1.SelectionChanged
        If Flag = 1 Then
            If ComboBox1.SelectedValue = 8 Then
                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible

                lblToId.Content = "المخزن المحول إليه"

                'ElseIf Md.ShowCostCenter AndAlso (ComboBox1.SelectedValue = 2 OrElse ComboBox1.SelectedValue = 3 OrElse ComboBox1.SelectedValue = 4 OrElse ComboBox1.SelectedValue = 5) Then

                '    lblToId.Visibility = Visibility.Visible
                '    ToId.Visibility = Visibility.Visible
                '    ToName.Visibility = Visibility.Visible

                '    lblToId.Content = "مركز التكلفة"
            Else
                lblToId.Visibility = Visibility.Hidden
                ToId.Visibility = Visibility.Hidden
                ToName.Visibility = Visibility.Hidden

                ToId.Clear()
                ToName.Clear()
            End If

        End If
    End Sub


    Private Sub ToId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ToId.KeyUp
        Dim Title, tbl As String
        If Flag = 1 Then
            tbl = "Stores"
            Title = "المخازن"

            If Md.ShowCostCenter AndAlso (ComboBox1.SelectedValue = 2 OrElse ComboBox1.SelectedValue = 3 OrElse ComboBox1.SelectedValue = 4 OrElse ComboBox1.SelectedValue = 5) Then
                bm.CostCenterIdShowHelp(ToId, ToName, e, )
            Else
                bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
            End If

        ElseIf Flag = 2 OrElse Flag = 7 OrElse Flag = 10 OrElse Flag = 21 OrElse Flag = 28 Then
            tbl = "Suppliers"
            Title = "الموردين"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl & " where (CountryId=" & Val(CountryId.Text) & " or " & Val(CountryId.Text) & "=0)")
        ElseIf Flag = 3 OrElse Flag = 8 OrElse Flag = 5 OrElse Flag = 30 OrElse Flag = 9 OrElse Flag = 31 Then
            tbl = "Customers"
            Title = "العملاء"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
        ElseIf Flag = 11 Then
            tbl = "Investors"
            Title = "المستثمرين"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
        ElseIf Flag = 4 Then
            bm.ShowHelpCases(ToId, ToName, e)
        End If
    End Sub

    Private Sub ToId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToId.LostFocus
        Dim tbl As String
        If Flag = 1 Then

            If Md.ShowCostCenter AndAlso (ComboBox1.SelectedValue = 2 OrElse ComboBox1.SelectedValue = 3 OrElse ComboBox1.SelectedValue = 4 OrElse ComboBox1.SelectedValue = 5) Then
                bm.CostCenterIdLostFocus(ToId, ToName, )
                Return
            Else
                tbl = "Stores"
            End If

        ElseIf Flag = 2 OrElse Flag = 7 OrElse Flag = 10 OrElse Flag = 21 OrElse Flag = 28 Then
            tbl = "Suppliers"
        ElseIf Flag = 11 Then
            tbl = "Investors"
        ElseIf Flag = 3 OrElse Flag = 8 OrElse Flag = 5 OrElse Flag = 30 OrElse Flag = 9 OrElse Flag = 31 Then
            tbl = "Customers"
        ElseIf Flag = 4 Then
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
    End Sub


    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ItemId.KeyUp
        Dim str As String = ""
        If Detail = 7 Then str = " ItemType<>3 and "
        If bm.ShowHelp("Items", ItemId, ItemName, e, "select cast(Id as varchar(100)) Id,Name from Items where " & str & " (GroupId='" & Val(GroupId.Text) & "' or '" & Val(GroupId.Text) & "'=0) and (TypeId='" & Val(TypeId.Text) & "' or '" & Val(TypeId.Text) & "'=0)") Then
            ItemId_LostFocus(ItemId, Nothing)
        End If
    End Sub

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId.LostFocus
        bm.LostFocus(ItemId, ItemName, "select Name from Items where Id=" & ItemId.Text.Trim())
        bm.GetImage("Items", New String() {"Id"}, New String() {ItemId.Text.Trim()}, "Image", Image1)
    End Sub

    Private Sub GroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles GroupId.LostFocus
        bm.LostFocus(GroupId, GroupName, "select Name from Groups where Id=" & GroupId.Text.Trim())
        TypeId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub TypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TypeId.LostFocus
        bm.LostFocus(TypeId, TypeName, "select Name from Types where GroupId=" & GroupId.Text.Trim & " and Id=" & TypeId.Text.Trim())
    End Sub

    Private Sub GroupId_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles GroupId.KeyUp
        If bm.ShowHelp("Groups", GroupId, GroupName, e, "select cast(Id as varchar(100)) Id,Name from Groups") Then
            GroupId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub TypeId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles TypeId.KeyUp
        bm.ShowHelp("Types", TypeId, TypeName, e, "select cast(Id as varchar(100)) Id,Name from Types where GroupId=" & GroupId.Text.Trim)
    End Sub

    Private Sub CountryId_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles CountryId.KeyUp
        If bm.ShowHelp("Countries", CountryId, CountryName, e, "select cast(Id as varchar(100)) Id,Name from Countries") Then
            CountryId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CountryId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId.LostFocus
        bm.LostFocus(CountryId, CountryName, "select Name from Countries where Id=" & CountryId.Text.Trim())
    End Sub


    Private Sub BtnCalc_Click(sender As Object, e As RoutedEventArgs) Handles BtnCalc.Click
        Dim f As New RPT25 With {.Flag = 20}
        f.UserControl_Loaded(Nothing, Nothing)
        f.FromDate.SelectedDate = FromDate.SelectedDate
        f.ToDate.SelectedDate = ToDate.SelectedDate
        f.Button2_Click(BtnCalc, Nothing)
    End Sub

    Private Sub CashierId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CashierId.KeyUp
        bm.ShowHelp("Employees", CashierId, CashierName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Stopped=0") '& IIf(TestConsumablesAndReturn() AndAlso Md.MyProjectType = ProjectType.X, " and Nurse=1", "")
    End Sub

    Private Sub CashierId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CashierId.LostFocus
        bm.LostFocus(CashierId, CashierName, "select " & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Id=" & CashierId.Text.Trim() & " and Stopped=0 ") '& IIf(TestConsumablesAndReturn() AndAlso Md.MyProjectType = ProjectType.X, " and Nurse=1", "")
    End Sub

    Private Sub WaiterId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles WaiterId.KeyUp
        bm.ShowHelp("المندوبين", WaiterId, WaiterName, e, "select cast(Id as varchar(100)) Id,Name from Sellers")
    End Sub

    Private Sub WaiterId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles WaiterId.LostFocus
        bm.LostFocus(WaiterId, WaiterName, "select Name from Sellers where Id=" & WaiterId.Text.Trim())
    End Sub

End Class
