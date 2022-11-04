Imports System.Data
Imports System.ComponentModel
Imports System.IO

Public Class CustomerInvoices
    Public TableName As String = "CustomerInvoices"
    Public SubId As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods
    WithEvents G As New MyGrid
    WithEvents G2 As New MyGrid
    WithEvents G3 As New MyGrid

    WithEvents Timr As New Timers.Timer(2000)
    WithEvents bg As New BackgroundWorker
    WithEvents BackgroundWorker1 As New BackgroundWorker


    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnPrint1, btnPrint2})
        TabControl2.SelectedIndex = -1
        LoadResource()
        LoadTypes()
        LoadWFH()
        LoadWFH2()
        LoadWFH3()
        bm.Fields = New String() {SubId, "DayDate", "CustomerId", "SuppPersonId", "CertificationNo", "CountryId", "CityId", "AreaId", "CountryId1", "CityId1", "AreaId1", "Notes"}
        bm.control = New Control() {txtID, DayDate, CustomerId, SuppPersonId, CertificationNo, CountryId, CityId, AreaId, CountryId1, CityId1, AreaId1, Notes}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)


        AddHandler bg.DoWork, AddressOf bg_DoWork
        AddHandler bg.RunWorkerCompleted, AddressOf bg_RunWorkerCompleted
        bg.RunWorkerAsync()
    End Sub



    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Value As String = "Value"
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Id, "كود البند")
        G.Columns.Add(GC.Name, "البند")
        G.Columns.Add(GC.Value, "القيمة")
        G.Columns.Add(GC.Notes, "البيان")

        G.Columns(GC.Name).FillWeight = 300
        G.Columns(GC.Value).FillWeight = 100
        G.Columns(GC.Notes).FillWeight = 300

        G.Columns(GC.Id).Visible = False
        G.Columns(GC.Name).ReadOnly = True

        G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G.AllowUserToDeleteRows = False
        AddHandler G.CellEndEdit, AddressOf GridCalcRow
    End Sub


    Structure GC2
        Shared Id As String = "Id"
        Shared Value As String = "Value"
        Shared IsNew As String = "IsNew"
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH2()
        WFH2.Child = G2

        G2.Columns.Clear()
        G2.ForeColor = System.Drawing.Color.DarkBlue
        G2.Columns.Add(GC2.Id, "مسلسل الإيصال")
        G2.Columns.Add(GC2.Value, "القيمة")
        G2.Columns.Add(GC2.IsNew, "IsNew")
        G2.Columns.Add(GC2.Notes, "نوع الإيصال")

        G2.Columns(GC2.Id).FillWeight = 100
        G2.Columns(GC2.Value).FillWeight = 100
        G2.Columns(GC2.Notes).FillWeight = 200

        G2.Columns(GC2.Id).ReadOnly = True
        G2.Columns(GC2.Value).ReadOnly = True
        G2.Columns(GC2.Notes).ReadOnly = True
        G2.Columns(GC2.IsNew).Visible = False
        G2.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G2.SelectionMode = Forms.DataGridViewSelectionMode.FullRowSelect
        G2.AllowUserToAddRows = False
        G2.AllowUserToDeleteRows = False
    End Sub

    Structure GC3
        Shared Id As String = "Id"
        Shared Value As String = "Value"
        Shared Tips As String = "Tips"
        Shared Tips2 As String = "Tips2"
        Shared Value2 As String = "Value2"
        Shared IsNew As String = "IsNew"
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH3()
        WFH3.Child = G3

        G3.Columns.Clear()
        G3.ForeColor = System.Drawing.Color.DarkBlue
        G3.Columns.Add(GC3.Id, "مسلسل النولون")
        G3.Columns.Add(GC3.Value, "القيمة سائقين")
        G3.Columns.Add(GC3.Value2, "القيمة عملاء")
        G3.Columns.Add(GC3.Tips, "الإكرامية سائقين")
        G3.Columns.Add(GC3.Tips2, "الإكرامية عملاء")
        G3.Columns.Add(GC3.IsNew, "IsNew")
        G3.Columns.Add(GC3.Notes, "الجهة")

        G3.Columns(GC3.Id).FillWeight = 100
        G3.Columns(GC3.Value).FillWeight = 100
        G3.Columns(GC3.Value2).FillWeight = 100
        G3.Columns(GC3.Tips).FillWeight = 100
        G3.Columns(GC3.Tips2).FillWeight = 100
        G3.Columns(GC3.Notes).FillWeight = 200

        G3.Columns(GC3.Id).ReadOnly = True
        G3.Columns(GC3.Value).ReadOnly = True
        G3.Columns(GC3.Value2).ReadOnly = True
        G3.Columns(GC3.Tips).ReadOnly = True
        G3.Columns(GC3.Tips2).ReadOnly = True
        G3.Columns(GC3.Notes).ReadOnly = True
        G3.Columns(GC3.IsNew).Visible = False
        G3.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G3.SelectionMode = Forms.DataGridViewSelectionMode.FullRowSelect
        G3.AllowUserToAddRows = False
        G3.AllowUserToDeleteRows = False
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        btnGetInvoices.IsEnabled = True
        btnGetNolon.IsEnabled = True

        CustomerId_LostFocus(Nothing, Nothing)
        SuppPersonId_LostFocus(Nothing, Nothing)
        CountryId_LostFocus(Nothing, Nothing)
        CityId_LostFocus(Nothing, Nothing)
        AreaId_LostFocus(Nothing, Nothing)
        CountryId1_LostFocus(Nothing, Nothing)
        CityId1_LostFocus(Nothing, Nothing)
        AreaId1_LostFocus(Nothing, Nothing)
        LoadTree()

        Dim dt As DataTable = bm.ExecuteAdapter("select * from CustomerInvoicesDt where InvoiceNo=" & txtID.Text)
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("ItemName").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
        Next
        G.RefreshEdit()

        G2.Rows.Clear()
        Dim dt2 As DataTable = bm.ExecuteAdapter("select InvoiceNo,Value,dbo.GetInvoicesTypeName(InvoicesTypeId)'InvoicesTypeName' from Invoices where CustomerInvoiceNo='" & txtID.Text & "'")
        For i As Integer = 0 To dt2.Rows.Count - 1
            G2.Rows.Add(dt2.Rows(i)("InvoiceNo"), dt2.Rows(i)("Value"), 1, dt2.Rows(i)("InvoicesTypeName"))
        Next
        G2.RefreshEdit()

        G3.Rows.Clear()
        Dim dt3 As DataTable = bm.ExecuteAdapter("select InvoiceNo,Value,Value2,Tips,Tips2,dbo.GetNolonPricesName(NolonPriceId)'NolonPricesName' from Nolon where CustomerInvoiceNo='" & txtID.Text & "'")
        For i As Integer = 0 To dt3.Rows.Count - 1
            G3.Rows.Add(dt3.Rows(i)("InvoiceNo"), dt3.Rows(i)("Value"), dt3.Rows(i)("Value2"), dt3.Rows(i)("Tips"), dt3.Rows(i)("Tips2"), 1, dt3.Rows(i)("NolonPricesName"))
        Next
        G3.RefreshEdit()

        DayDate.Focus()
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
        G.EndEdit()
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If Not bm.SaveGrid(G, "CustomerInvoicesDt", New String() {"InvoiceNo"}, New String() {txtID.Text}, New String() {"ItemId", "ItemName", "Value", "Notes"}, New String() {GC.Id, GC.Name, GC.Value, GC.Notes}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.Decimal, VariantType.String}, New String() {GC.Id}) Then Return

        SaveG2()
        SaveG3()

        If sender Is btnGetInvoices OrElse sender Is btnGetNolon Then Return

        If sender Is btnPrint1 Then
            Dim rpt As New ReportViewer
            rpt.paraname = New String() {"@InvoiceNo"}
            rpt.paravalue = New String() {txtID.Text}
            rpt.Rpt = "CustomerInvoice.rpt"
            rpt.Show()
            Return
        ElseIf sender Is btnPrint2 Then
            Dim rpt As New ReportViewer
            rpt.paraname = New String() {"@InvoiceNo"}
            rpt.paravalue = New String() {txtID.Text}
            rpt.Rpt = "CustomerInvoice2.rpt"
            rpt.Show()
            Return
        End If

        If Not sender Is Button1 Then btnNew_Click(sender, e)
        
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
        TreeView1.Items.Clear()
        bm.ClearControls()
        CustomerId_LostFocus(Nothing, Nothing)
        SuppPersonId_LostFocus(Nothing, Nothing)

        CountryName.Clear()
        CityName.Clear()
        AreaName.Clear()

        CountryName1.Clear()
        CityName1.Clear()
        AreaName1.Clear()

        G.Rows.Clear()
        G2.Rows.Clear()
        G3.Rows.Clear()

        btnGetInvoices.IsEnabled = True
        btnGetNolon.IsEnabled = True

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"
        DayDate.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from CustomerInvoicesDt where " & SubId & "='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from CustomerInvoiceAttachments where " & SubId & "='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("UPDATE Invoices set CustomerInvoiceNo=0 where CustomerInvoiceNo='" & txtID.Text & "'")
            bm.ExecuteNonQuery("UPDATE Nolon set CustomerInvoiceNo=0 where CustomerInvoiceNo='" & txtID.Text & "'")
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

    Private Sub CountryId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CountryId.KeyUp
        bm.ShowHelp("Countries", CountryId, CountryName, e, "select cast(Id as varchar(100)) Id,Name from Countries", "Countries")
    End Sub

    Private Sub CityId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CityId.KeyUp
        bm.ShowHelp("Cities", CityId, CityName, e, "select cast(Id as varchar(100)) Id,Name from Cities where CountryId=" & CountryId.Text.Trim, "Cities", {"CountryId"}, {Val(CountryId.Text)})
    End Sub

    Private Sub AreaId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AreaId.KeyUp
        bm.ShowHelp("Areas", AreaId, AreaName, e, "select cast(Id as varchar(100)) Id,Name from Areas where CountryId=" & CountryId.Text.Trim & " and CityId=" & CityId.Text, "Areas", {"CountryId", "CityId"}, {Val(CountryId.Text), Val(CityId.Text)})
    End Sub

    Private Sub CountryId1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CountryId1.KeyUp
        bm.ShowHelp("Countries", CountryId1, CountryName1, e, "select cast(Id as varchar(100)) Id,Name from Countries", "Countries")
    End Sub

    Private Sub CityId1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CityId1.KeyUp
        bm.ShowHelp("Cities", CityId1, CityName1, e, "select cast(Id as varchar(100)) Id,Name from Cities where CountryId=" & CountryId1.Text.Trim, "Cities", {"CountryId"}, {Val(CountryId1.Text)})
    End Sub

    Private Sub AreaId1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AreaId1.KeyUp
        bm.ShowHelp("Areas", AreaId1, AreaName1, e, "select cast(Id as varchar(100)) Id,Name from Areas where CountryId=" & CountryId1.Text.Trim & " and CityId=" & CityId1.Text, "Areas", {"CountryId", "CityId"}, {Val(CountryId1.Text), Val(CityId1.Text)})
    End Sub


    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, CustomerId.KeyDown, SuppPersonId.KeyDown, CountryId.KeyDown, CityId.KeyDown, AreaId.KeyDown, CountryId1.KeyDown, CityId1.KeyDown, AreaId1.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub


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

    Private Sub CountryId1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId1.LostFocus
        bm.LostFocus(CountryId1, CountryName1, "select Name from Countries where Id=" & CountryId1.Text.Trim())
        CityId1_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CityId1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CityId1.LostFocus
        bm.LostFocus(CityId1, CityName1, "select Name from Cities where CountryId=" & CountryId1.Text.Trim & " and Id=" & CityId1.Text.Trim())
        AreaId1_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub AreaId1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AreaId1.LostFocus
        bm.LostFocus(AreaId1, AreaName1, "select Name from Areas where CountryId=" & CountryId1.Text.Trim & " and CityId=" & CityId1.Text.Trim & " and Id=" & AreaId1.Text.Trim())
    End Sub

    Sub LoadTypes()
        Try
            WR1.Children.Clear()
            Dim dt As DataTable = bm.ExecuteAdapter("select * from CustomerInvoicesItems")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 120
                x.Height = 50
                x.Margin = New Thickness(10, 10, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = dt.Rows(i)("Name").ToString.Replace(vbCrLf, " ")
                x.ToolTip = x.Content
                x.Background = btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR1.Children.Add(x)
                AddHandler x.Click, AddressOf btnClick
            Next
        Catch ex As Exception
        End Try
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


    Private Sub SuppPersonId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SuppPersonId.LostFocus
        bm.LostFocus(SuppPersonId, SuppPersonName, "select Name from SuppPersons where Id=" & SuppPersonId.Text.Trim())
    End Sub

    Private Sub SuppPersonId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SuppPersonId.KeyUp
        bm.ShowHelp("SuppPersons", SuppPersonId, SuppPersonName, e, "select cast(Id as varchar(100)) Id,Name from SuppPersons", "SuppPersons")
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
        'lblSheekPerson.SetResourceReference(ContentProperty, "SheekPerson")


        lblCountryId.SetResourceReference(ContentProperty, "CountryId")
        lblCityId.SetResourceReference(ContentProperty, "CityId")
        lblAreaId.SetResourceReference(ContentProperty, "AreaId")

        lblCountryId1.SetResourceReference(ContentProperty, "CountryId")
        lblCityId1.SetResourceReference(ContentProperty, "CityId")
        lblAreaId1.SetResourceReference(ContentProperty, "AreaId")

    End Sub

    Private Sub btnClick(sender As Object, e As RoutedEventArgs)
        TabControl2.SelectedIndex = 0

        Dim i As Integer = G.Rows.Add()

        G.Rows(i).Cells(GC.Id).Value = CType(sender, Button).Tag
        G.Rows(i).Cells(GC.Name).Value = CType(sender, Button).Content
        G.Rows(i).Cells(GC.Value).Value = 0

        G.Focus()
        G.Rows(i).Selected = True
        G.FirstDisplayedScrollingRowIndex = i
        G.CurrentCell = G.Rows(i).Cells(GC.Value)
        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G.BeginEdit(True)

    End Sub

    Private Sub GridCalcRow(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value)
            If G.Rows(e.RowIndex).Cells(GC.Id).Value = Nothing Then
                G.CommitEdit(Forms.DataGridViewDataErrorContexts.RowDeletion)
                G.Rows.RemoveAt(e.RowIndex)
            End If
        Catch
        End Try
    End Sub

    Private Sub btnGetInvoices_Click(sender As Object, e As RoutedEventArgs) Handles btnGetInvoices.Click
        Dim dt As DataTable = bm.ExecuteAdapter("select InvoiceNo,Value,dbo.GetInvoicesTypeName(InvoicesTypeId)'InvoicesTypeName'  from Invoices where CustomerInvoiceNo=0 and CustomerId='" & CustomerId.Text & "'")
        For i As Integer = 0 To dt.Rows.Count - 1
            G2.Rows.Add(dt.Rows(i)("InvoiceNo"), dt.Rows(i)("Value"), 1, dt.Rows(i)("InvoicesTypeName"))
        Next
        btnSave_Click(sender, e)
        'btnGetInvoices.IsEnabled = False
    End Sub

    Private Sub btnGetNolon_Click(sender As Object, e As RoutedEventArgs) Handles btnGetNolon.Click
        Dim dt As DataTable = bm.ExecuteAdapter("select InvoiceNo, Value, Tips, Value2, Tips2, dbo.GetNolonPricesName(NolonPriceId)'NolonPricesName' from Nolon where CustomerInvoiceNo=0 and CustomerId='" & CustomerId.Text & "'")
        For i As Integer = 0 To dt.Rows.Count - 1
            G3.Rows.Add(dt.Rows(i)("InvoiceNo"), dt.Rows(i)("Value"), dt.Rows(i)("Value2"), dt.Rows(i)("Tips"), dt.Rows(i)("Tips2"), 1, dt.Rows(i)("NolonPricesName"))
        Next
        btnSave_Click(sender, e)
        ' btnGetNolon.IsEnabled = False
    End Sub

    Private Sub SaveG2()
        For i As Integer = 0 To G2.Rows.Count - 1
            If G2.Rows(i).Cells(GC2.IsNew).Value = 1 Then
                bm.ExecuteNonQuery("UPDATE Invoices set CustomerInvoiceNo='" & txtID.Text & "' where InvoiceNo='" & G2.Rows(i).Cells(GC2.Id).Value & "'")
            End If
        Next
    End Sub

    Private Sub SaveG3()
        For i As Integer = 0 To G3.Rows.Count - 1
            If G3.Rows(i).Cells(GC3.IsNew).Value = 1 Then
                bm.ExecuteNonQuery("UPDATE Nolon set CustomerInvoiceNo='" & txtID.Text & "' where InvoiceNo='" & G3.Rows(i).Cells(GC3.Id).Value & "'")
            End If
        Next
    End Sub

    Private Sub btnDeleteInvoice_Click(sender As Object, e As RoutedEventArgs) Handles btnDeleteInvoice.Click
        If G2.CurrentRow Is Nothing Then Return
        If bm.ShowDeleteMSG("هل أنت متأكد من حذف هذا الإيصال؟") Then
            bm.ExecuteNonQuery("UPDATE Invoices set CustomerInvoiceNo=0 where InvoiceNo='" & G2.Rows(G2.CurrentRow.Index).Cells(GC2.Id).Value & "' and CustomerInvoiceNo='" & txtID.Text & "'")
            G2.Rows.RemoveAt(G2.CurrentRow.Index)
        End If
    End Sub

    Private Sub btnDeleteNolon_Click(sender As Object, e As RoutedEventArgs) Handles btnDeleteNolon.Click
        If G3.CurrentRow Is Nothing Then Return
        If bm.ShowDeleteMSG("هل أنت متأكد من حذف هذا النولون؟") Then
            bm.ExecuteNonQuery("UPDATE Nolon set CustomerInvoiceNo=0 where InvoiceNo='" & G3.Rows(G3.CurrentRow.Index).Cells(GC3.Id).Value & "' and CustomerInvoiceNo='" & txtID.Text & "'")
            G3.Rows.RemoveAt(G3.CurrentRow.Index)
        End If
    End Sub

    Private Sub btnDeleteItem_Click(sender As Object, e As RoutedEventArgs) Handles btnDeleteItem.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من حذف هذا البند؟") Then
            G.Rows.RemoveAt(G.CurrentRow.Index)
        End If
    End Sub

    Private Sub btnPrint1_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint1.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub btnPrint2_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint2.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub bg_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        TabControl2.SelectedIndex = 0
    End Sub

    Private Sub Timr_Elapsed(sender As Object, e As Timers.ElapsedEventArgs)
        Timr.Stop()
    End Sub

    Private Sub bg_DoWork(sender As Object, e As DoWorkEventArgs)
        System.Threading.Thread.Sleep(500)
    End Sub






    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
        Try
            MyImagedata = Nothing
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize <> 18 Then Return
            Dim s As New Forms.SaveFileDialog With {.Filter = "All files (*.*)|*.*"}
            s.Filter = "All files (*.*)|*.*"
            s.FileName = CType(TreeView1.SelectedItem, TreeViewItem).Header

            If IsNothing(sender) Then
                MyBath = bm.GetNewTempName(s.FileName)
            Else
                If Not s.ShowDialog = Forms.DialogResult.OK Then Return
                MyBath = s.FileName
            End If

            Button4.IsEnabled = False
            F1 = txtID.Text
            F2 = CType(TreeView1.SelectedItem, TreeViewItem).Tag
            BackgroundWorker1.RunWorkerAsync()
        Catch ex As Exception
        End Try
    End Sub
    Dim F2 As String = "", F1 As String = ""
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim myCommand As SqlClient.SqlCommand
            myCommand = New SqlClient.SqlCommand("select Image from CustomerInvoiceAttachments where InvoiceNo='" & F1 & "' and AttachedName='" & F2 & "'" & bm.AppendWhere, con)
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
        Button4.IsEnabled = True
    End Sub

    Dim MyImagedata() As Byte
    Dim MyBath As String = ""
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button3.Click
        Try
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize = 18 Then
                If bm.ShowDeleteMSG("هل أنت متأكد من مسح الملف """ & TreeView1.SelectedItem.Header & """?") Then
                    bm.ExecuteNonQuery("delete from CustomerInvoiceAttachments where InvoiceNo='" & txtID.Text & "' and AttachedName='" & TreeView1.SelectedItem.Header & "'" & bm.AppendWhere)
                    LoadTree()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadTree()
        Dim dt As DataTable = bm.ExecuteAdapter("select AttachedName from CustomerInvoiceAttachments where InvoiceNo=" & txtID.Text & bm.AppendWhere)
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

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        btnSave_Click(sender, e)

        Dim o As New Forms.OpenFileDialog
        o.Multiselect = True
        If o.ShowDialog = Forms.DialogResult.OK Then
            For i As Integer = 0 To o.FileNames.Length - 1
                bm.SaveFile("CustomerInvoiceAttachments", "InvoiceNo", txtID.Text, "AttachedName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "Image", o.FileNames(i))
            Next
        End If
        LoadTree()
    End Sub


    Private Sub TreeView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TreeView1.MouseDoubleClick
        Button4_Click(Nothing, Nothing)
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
