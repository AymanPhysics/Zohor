Imports System.Data
Public Class Customers
    Public TableName As String = "Customers"
    Public SubId As String = "Id"
    Public SubName As String = "Name"



    Public MyId As Integer = 0
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        bm.FillCombo("PriceLists", PriceListId, "")
        PriceListId.Visibility = Visibility.Hidden
        If Md.ShowPriceLists OrElse Md.MyProjectType = ProjectType.X Then
            PriceListId.Visibility = Visibility.Visible
        End If


        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        If Not Md.ShowCurrency Then CurrencyId.Visibility = Visibility.Hidden
        LoadResource()
        bm.Fields = New String() {SubId, SubName, "AccNo", "CountryId", "CityId", "AreaId", "Address", "Floor", "Appartment", "Tel", "Mobile", "CurrencyId", "Exchange", "MainBal0", "Bal0", "DescPerc", "Type", "ApplyCurrencyCycle", "Ban", "PriceListId", "EmpId", "Notes", "NationalId", "Ban2", "BanNotes", "CashierId", "MonthlyPayment", "MonthlyPaymentDay"}
        bm.control = New Control() {txtID, txtName, AccNo, CountryId, CityId, AreaId, Address, Floor, Appartment, Tel, Mobile, CurrencyId, Exchange, MainBal0, Bal0, DescPerc, Type, ApplyCurrencyCycle, Ban, PriceListId, EmpId, Notes, NationalId, Ban2, BanNotes, CashierId, MonthlyPayment, MonthlyPaymentDay}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        If Not Md.ShowCurrency Then
            CurrencyId.Visibility = Visibility.Hidden
            lblExchange.Visibility = Visibility.Hidden
            Exchange.Visibility = Visibility.Hidden
            lblBal0.Visibility = Visibility.Hidden
            Bal0.Visibility = Visibility.Hidden
            ApplyCurrencyCycle.Visibility = Visibility.Hidden
        End If
        btnNew_Click(sender, e)
        If MyId > 0 Then
            txtID.Text = MyId
            txtID_LostFocus(Nothing, Nothing)
            If Not Md.Manager Then
                btnDelete.IsEnabled = False
                Bal0.IsEnabled = False
                MainBal0.IsEnabled = False
            End If
        End If

        If Md.MyProjectType = ProjectType.X Then
            Ban.Content = "قضية"
            lblEmpId.Content = "القائم بالاستعلام"
        Else
            Ban2.Visibility = Visibility.Hidden
            BanNotes.Visibility = Visibility.Hidden
            lblBanNotes.Visibility = Visibility.Hidden
        End If
        If Not Md.MyProjectType = ProjectType.X Then
            lblLastPayment.Visibility = Visibility.Hidden
            LastPayment.Visibility = Visibility.Hidden
            lblCurrentBal.Visibility = Visibility.Hidden
            CurrentBal.Visibility = Visibility.Hidden
            lblMonthlyPayment.Visibility = Visibility.Hidden
            MonthlyPayment.Visibility = Visibility.Hidden
            lblMonthlyPaymentDay.Visibility = Visibility.Hidden
            MonthlyPaymentDay.Visibility = Visibility.Hidden
            btnMonthlyPayment.Visibility = Visibility.Hidden
            btnRpt.Visibility = Visibility.Hidden
        End If

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        AccNo_LostFocus(Nothing, Nothing)
        CountryId_LostFocus(Nothing, Nothing)
        CityId_LostFocus(Nothing, Nothing)
        AreaId_LostFocus(Nothing, Nothing)
        EmpId_LostFocus(Nothing, Nothing)
        CashierId_LostFocus(Nothing, Nothing)

        If Md.MyProjectType = ProjectType.X Then
            CurrentBal.Content = bm.ExecuteScalar("select dbo.Bal0Link(1,'" & Val(txtID.Text) & "','" & bm.ToStrDate(bm.MyGetDate()) & "',0)")
            LastPayment.Content = bm.ExecuteScalar("select dbo.getCustomerLastPayment('" & Val(txtID.Text) & "')")
        End If
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If

        Dim x As Integer = Val(bm.ExecuteScalar("select Id from " & TableName & " where Name='" & txtName.Text.Trim & "' and Id<>" & Val(txtID.Text)))
        If x > 0 Then
            bm.ShowMSG("تم تكرار الاسم بمسلسل رقم " & x)
            Return
        End If


        If Val(AccNo.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            AccNo.Focus()
            Return
        End If
        Bal0.Text = Val(Bal0.Text.Trim)
        DescPerc.Text = Val(DescPerc.Text.Trim)
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        AllowSave = True
        If Not DontClear Then btnNew_Click(sender, e)

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()


        If Md.MyProjectType = ProjectType.X Then
            bm.ExecuteNonQuery("updateCustomersTempBal0", {"Id"}, {Val(txtID.Text)})
        End If


        bm.ClearControls()
        CurrencyId_LostFocus(Nothing, Nothing)

        AccName.Clear()
        CountryName.Clear()
        CityName.Clear()
        AreaName.Clear()
        EmpName.Clear()
        CashierId_LostFocus(Nothing, Nothing)

        txtName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        CurrentBal.Content = ""
        LastPayment.Content = ""

        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Val(bm.ExecuteScalar("select dbo.GetSubAccUsingCount(" & 1 & ",'" & txtID.Text.Trim & "')")) > 0 Then
                bm.ShowMSG("غير مسموح بمسح حساب عليه حركات")
                Exit Sub
            End If
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
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
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            txtName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        txtName.SelectAll()
        txtName.Focus()
        txtName.SelectAll()
        txtName.Focus()
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub CountryId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CountryId.KeyUp
        bm.ShowHelp("Countries", CountryId, CountryName, e, "select cast(Id as varchar(100)) Id,Name from Countries", "Countries")
    End Sub

    Private Sub CityId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CityId.KeyUp
        bm.ShowHelp("Cities", CityId, CityName, e, "select cast(Id as varchar(100)) Id,Name from Cities where CountryId=" & CountryId.Text.Trim, "Cities", {"CountryId"}, {Val(CountryId.Text)})
    End Sub

    Private Sub AreaId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AreaId.KeyUp
        bm.ShowHelp("Areas", AreaId, AreaName, e, "select cast(Id as varchar(100)) Id,Name from Areas where CountryId=" & CountryId.Text.Trim & " and CityId=" & CityId.Text, "Areas", {"CountryId", "CityId"}, {Val(CountryId.Text), Val(CityId.Text)})
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, CountryId.KeyDown, CityId.KeyDown, AreaId.KeyDown, AccNo.KeyDown, EmpId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Bal0.KeyDown, DescPerc.KeyDown, MainBal0.KeyDown, Exchange.KeyDown
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



    Private Sub CountryId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId.LostFocus
        bm.LostFocus(CountryId, CountryName, "select Name from Countries where Id=" & CountryId.Text.Trim())
        CityId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CityId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CityId.LostFocus
        bm.LostFocus(CityId, CityName, "select Name from Cities where CountryId=" & CountryId.Text.Trim & " and Id=" & CityId.Text.Trim())
        AreaId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub AreaId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AreaId.LostFocus
        bm.LostFocus(AreaId, AreaName, "select Name from Areas where CountryId=" & CountryId.Text.Trim & " and CityId=" & CityId.Text.Trim & " and Id=" & AreaId.Text.Trim())
    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , 1, ,, True)
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , 1, ,, True)
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblId.SetResourceReference(ContentProperty, "Id")
        lblName.SetResourceReference(ContentProperty, "Name")
        lblAccNo.SetResourceReference(ContentProperty, "AccNo")
        lblAddress.SetResourceReference(ContentProperty, "Address")
        lblAppartment.SetResourceReference(ContentProperty, "Appartment")
        lblBal0.SetResourceReference(ContentProperty, "Bal0")
        lblCountryId.SetResourceReference(ContentProperty, "CountryId")
        lblCityId.SetResourceReference(ContentProperty, "CityId")
        lblAreaId.SetResourceReference(ContentProperty, "AreaId")
        lblDescPerc.SetResourceReference(ContentProperty, "DescPerc")
        lblFloor.SetResourceReference(ContentProperty, "Floor")
        lblMobile.SetResourceReference(ContentProperty, "Mobile")
        lblTel.SetResourceReference(ContentProperty, "Tel")
    End Sub

    Private Sub txtID_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelpCustomers(txtID, txtName, e) Then
            txtID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CurrencyId_LostFocus(sender As Object, e As RoutedEventArgs) Handles CurrencyId.LostFocus
        Try
            Exchange.Text = bm.ExecuteScalar("select dbo.GetCurrencyExchange(0,0," & CurrencyId.SelectedValue.ToString & ",0,getdate())")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub MainBal0_TextChanged(sender As Object, e As TextChangedEventArgs) Handles MainBal0.TextChanged, Exchange.TextChanged
        Bal0.Text = Val(MainBal0.Text) * Val(Exchange.Text)
    End Sub

    Public Sub Ban_Checked(sender As Object, e As RoutedEventArgs) Handles Ban.Checked, Ban.Unchecked, Ban2.Checked, Ban2.Unchecked
        If sender.IsChecked Then
            sender.Foreground = System.Windows.Media.Brushes.Red
            sender.FontWeight = FontWeights.ExtraBold
        Else
            sender.Foreground = System.Windows.Media.Brushes.Black
            sender.FontWeight = FontWeights.Normal
        End If
    End Sub

    Private Sub EmpId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId.KeyUp
        bm.ShowHelp("الموظفين", EmpId, EmpName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId.LostFocus
        bm.LostFocus(EmpId, EmpName, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId.Text.Trim() & " and Stopped=0")
    End Sub

    Private Sub CashierId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CashierId.KeyUp
        bm.ShowHelp("Employees", CashierId, CashierName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Stopped=0 and Cashier=1")
    End Sub

    Private Sub CashierId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CashierId.LostFocus
        bm.LostFocus(CashierId, CashierName, "select " & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Id=" & CashierId.Text.Trim() & " and Stopped=0 and Cashier=1")
    End Sub

    Dim DontClear As Boolean = False
    Dim AllowSave As Boolean = False
    Private Sub BtnMonthlyPayment_Click(sender As Object, e As RoutedEventArgs) Handles btnMonthlyPayment.Click
        If Val(MonthlyPayment.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblMonthlyPayment.Content)
            Return
        End If

        If Val(MonthlyPaymentDay.Text) < 1 OrElse Val(MonthlyPaymentDay.Text) > 28 Then
            bm.ShowMSG("برجاء تحديد " & lblMonthlyPaymentDay.Content & " بين 1 و 28")
            Return
        End If

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
        rpt.paraname = New String() {"@Id", "Header"}
        rpt.paravalue = New String() {Val(txtID.Text), CType(Parent, Page).Title}
        rpt.Rpt = "CustomerMonthlyPayment.rpt"
        rpt.Show()
    End Sub

    Private Sub BtnRpt_Click(sender As Object, e As RoutedEventArgs) Handles btnRpt.Click
        Dim rpt As New ReportViewer
        Dim RPTFlag1 As Integer = 3
        rpt.paraname = New String() {"@MainAccNo", "MainAccName", "@SubAccNo", "SubAccName", "@FromDate", "@ToDate", "Header", "@Detailed", "@DetailedInvoice", "@LinkFile", "@ToId", "@RPTFlag1", "@RPTFlag2", "@ActiveOnly", "@HasBalOnly", "@WindowId", "@CostCenterId", "@CostCenterSubId", "@FromMainAccNo", "@ToMainAccNo"}
        rpt.paravalue = New String() {AccNo.Text, "", Val(txtID.Text), txtName.Text, "2000-1-1", bm.MyGetDate(), CType(Parent, Page).Title.Trim & " ", 1, 0, 1, Val(txtID.Text), 3, 0, 1, 0, 0, 0, 0, AccNo.Text, AccNo.Text}
        rpt.Rpt = "AccountMotion.rpt"
        rpt.Show()
    End Sub
End Class
