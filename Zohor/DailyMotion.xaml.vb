Imports System.Data

Public Class DailyMotion
    Public TableName As String = "DailyMotion"
    Public SubId As String = "InvoiceNo"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        bm.FillCombo("NolonPriceTypes", NolonPriceTypeId, "")
        bm.FillCombo("PaymentTypes", PaymentTypeId, "")
        bm.Fields = New String() {SubId, "DayDate", "CustomerId", "SuppPersonId", "PaymentTypeId", "SellerId", "NolonPriceId", "NolonPriceTypeId", "Notes", "DueDate", "PoliceNo"}
        bm.control = New Control() {txtID, DayDate, CustomerId, SuppPersonId, PaymentTypeId, SellerId, NolonPricesId, NolonPriceTypeId, Notes, DueDate, PoliceNo}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim lop2 As Boolean = False
    Sub FillControls()
        lop2 = True
        bm.FillControls(Me)
        CustomerId_LostFocus(Nothing, Nothing)
        SellerId_LostFocus(Nothing, Nothing)
        SuppPersonId_LostFocus(Nothing, Nothing)
        NolonPricesId_LostFocus(Nothing, Nothing)
        DayDate.Focus()
        lop2 = False
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Val(CustomerId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد العميل")
            CustomerId.Focus()
            Return
        End If
        If Val(SellerId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المندوب")
            SellerId.Focus()
            Return
        End If
        If Val(SuppPersonId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد التوكيل")
            SuppPersonId.Focus()
            Return
        End If
        If Val(NolonPricesId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الجهة")
            NolonPricesId.Focus()
            Return
        End If
        If NolonPriceTypeId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد مقاس الحاوية")
            NolonPriceTypeId.Focus()
            Return
        End If
        If PaymentTypeId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد حالة التنقيذ")
            PaymentTypeId.Focus()
            Return
        End If

        CustomerId.Text = Val(CustomerId.Text)
        NolonPricesId.Text = Val(NolonPricesId.Text)

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        btnNew_Click(sender, e)
        
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
        bm.ClearControls()
        CustomerId_LostFocus(Nothing, Nothing)
        SellerId_LostFocus(Nothing, Nothing)
        SuppPersonId_LostFocus(Nothing, Nothing)
        NolonPricesId_LostFocus(Nothing, Nothing)
        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        DueDate.SelectedDate = MyNow
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"
        DayDate.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
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
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, CustomerId.KeyDown, NolonPricesId.KeyDown, SuppPersonId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub


    

    Private Sub CustomerId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CustomerId.LostFocus
        bm.LostFocus(CustomerId, CustomerName, "select Name from Customers where Id=" & CustomerId.Text.Trim())
        Dim s As String = ""
        Dim dt As DataTable = bm.ExecuteAdapter("GetCustomerData", New String() {"Id"}, New String() {Val(CustomerId.Text)})
        CustomerId.ToolTip = ""
        CustomerName.ToolTip = ""
        If dt.Rows.Count = 0 Then Return
        For i As Integer = 0 To dt.Columns.Count - 2
            s &= dt.Rows(0)(i).ToString & IIf(i = dt.Columns.Count - 1, "", vbCrLf)
        Next
        CustomerId.ToolTip = s
        CustomerName.ToolTip = s
    End Sub

    Private Sub CustomerId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CustomerId.KeyUp
        'bm.ShowHelp("Customers", CustomerId, CustomerName, e, "select cast(Id as varchar(100)) Id,Name from Customers")
        bm.ShowHelpCustomers(CustomerId, CustomerName, e)
    End Sub


    Private Sub SellerId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SellerId.LostFocus
        If Val(SellerId.Text.Trim) = 0 Then
            SellerId.Clear()
            SellerName.Clear()
            Return
        End If

        bm.LostFocus(SellerId, SellerName, "select Name from Sellers where Id=" & SellerId.Text.Trim())
    End Sub
    Private Sub SellerId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SellerId.KeyUp
        If bm.ShowHelp("Sellers", SellerId, SellerName, e, "select cast(Id as varchar(100)) Id,Name from Sellers") Then
            SellerId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SuppPersonId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SuppPersonId.LostFocus
        If Val(SuppPersonId.Text.Trim) = 0 Then
            SuppPersonId.Clear()
            SuppPersonName.Clear()
            Return
        End If

        bm.LostFocus(SuppPersonId, SuppPersonName, "select Name from SuppPersons where Id=" & SuppPersonId.Text.Trim())
    End Sub
    Private Sub SuppPersonId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SuppPersonId.KeyUp
        If bm.ShowHelp("SuppPersons", SuppPersonId, SuppPersonName, e, "select cast(Id as varchar(100)) Id,Name from SuppPersons", "SuppPersons") Then
            SuppPersonId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub NolonPricesId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles NolonPricesId.LostFocus, NolonPriceTypeId.LostFocus, NolonPriceTypeId.SelectionChanged
        If Val(NolonPricesId.Text.Trim) = 0 Then
            NolonPricesId.Clear()
            NolonPricesName.Clear()
            Return
        End If
        bm.LostFocus(NolonPricesId, NolonPricesName, "select dbo.GetNolonPricesName(Id) from NolonPrices where Id=" & NolonPricesId.Text.Trim())
    End Sub
    Private Sub NolonPricesId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles NolonPricesId.KeyUp
        If bm.ShowHelp("NolonPrices", NolonPricesId, NolonPricesName, e, "select cast(Id as varchar(100)) Id,dbo.GetNolonPricesName(Id) Name from NolonPrices") Then
            NolonPricesId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub LoadResource()
        Return
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblId.SetResourceReference(ContentProperty, "Id")
        lblMainAccNo.SetResourceReference(ContentProperty, "MainAccNo")
        lblNotes.SetResourceReference(ContentProperty, "Notes")
        lblNolonPriceId.SetResourceReference(ContentProperty, "Direction")

        lblDueDate.SetResourceReference(CheckBox.ContentProperty, "DueDate")

    End Sub


    Private Sub DayDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DayDate.SelectedDateChanged
        Try
            'DueDate.SelectedDate = DateAdd(DateInterval.Day, 7, DayDate.SelectedDate.Value)
        Catch ex As Exception
        End Try
    End Sub


    Dim SearchLop As Boolean = False
    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnSearch.Click
        SearchLop = True
        bm.DefineValues()
        bm.SearchTable(New String() {SubId}, New String() {txtID.Text.Trim}, cboSearch)
        SearchLop = False
    End Sub

    Private Sub cboSearch_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSearch.SelectionChanged
        If SearchLop Then Return
        txtID.Text = cboSearch.SelectedValue.ToString
        txtID_LostFocus(Nothing, Nothing)
    End Sub
End Class
