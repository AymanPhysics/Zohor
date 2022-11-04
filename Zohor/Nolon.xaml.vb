Imports System.Data
Imports System.ComponentModel
Imports System.IO

Public Class Nolon
    Public TableName As String = "Nolon"
    Public SubId As String = "InvoiceNo"

    Dim dt As New DataTable
    Dim bm As New BasicMethods
    Dim IsNewPone As Boolean = False
    WithEvents BackgroundWorker1 As New BackgroundWorker


    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        bm.FillCombo("NolonPriceTypes", NolonPriceTypeId, "")
        bm.FillCombo("CustomerInvoicesTypes", CustomerInvoicesTypeId, "")
        bm.Fields = New String() {SubId, "DayDate", "CustomerId", "CustomerInvoicesTypeId", "SellerId", "DriverId", "CarNo", "TrillaNo", "PoliceNo", "SilNo", "NolonPriceId", "NolonPriceTypeId", "Value", "Value2", "Payed", "Remaining", "Tips", "Tips2", "Notes", "CustomerInvoiceNo", "DueDate", "Done", "DocNo", "Docdate", "Payments", "OtherPayments"}
        bm.control = New Control() {txtID, DayDate, CustomerId, CustomerInvoicesTypeId, SellerId, DriverId, CarNo, TrillaNo, PoliceNo, SilNo, NolonPricesId, NolonPriceTypeId, Value, Value2, Payed, Remaining, Tips, Tips2, Notes, CustomerInvoiceNo, DueDate, Done, DocNo, Docdate, Payments, OtherPayments}
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
        IsNewPone = False
        bm.FillControls(Me)
        CustomerId_LostFocus(Nothing, Nothing)
        SellerId_LostFocus(Nothing, Nothing)
        DriverId_LostFocus(Nothing, Nothing)
        NolonPricesId_LostFocus(Nothing, Nothing)
        LoadTree()
        DayDate.Focus()
        lop2 = False
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not btnSave.IsEnabled Then Return
        
        
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
        If Val(DriverId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد السائق")
            DriverId.Focus()
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
        If CustomerInvoicesTypeId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد الحالة")
            CustomerInvoicesTypeId.Focus()
            Return
        End If

        lop2 = True
        Value.Text = Val(Value.Text)
        Value2.Text = Val(Value2.Text)
        Payed.Text = Val(Payed.Text)
        Remaining.Text = Val(Remaining.Text)
        Tips.Text = Val(Tips.Text)
        Tips2.Text = Val(Tips2.Text)
        lop2 = False

        CustomerInvoiceNo.Text = Val(CustomerInvoiceNo.Text)
        CustomerId.Text = Val(CustomerId.Text)
        DriverId.Text = Val(DriverId.Text)
        NolonPricesId.Text = Val(NolonPricesId.Text)
        OtherPayments.Text = Val(OtherPayments.Text)

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If IsNewPone Then
            Dim Field As String = "Price1"
            Select Case NolonPriceTypeId.SelectedValue
                Case 1 : Field = "Price1"
                Case 2 : Field = "Price2"
                Case 3 : Field = "Price3"
                Case 4 : Field = "Price4"
                Case 5 : Field = "Price5"
            End Select
            bm.ExecuteNonQuery("update NolonPrices set " & Field & "=" & Val(Value.Text) & " where Id=" & NolonPricesId.Text.Trim())
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
        IsNewPone = True
        TreeView1.Items.Clear()
        bm.ClearControls()
        CustomerId_LostFocus(Nothing, Nothing)
        SellerId_LostFocus(Nothing, Nothing)
        DriverId_LostFocus(Nothing, Nothing)
        NolonPricesId_LostFocus(Nothing, Nothing)

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, CustomerId.KeyDown, DriverId.KeyDown, NolonPricesId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown, Value2.KeyDown, Tips.KeyDown, Tips2.KeyDown, Payed.KeyDown, Payments.KeyDown, OtherPayments.KeyDown
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



    Private Sub DriverId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DriverId.LostFocus
        If Val(DriverId.Text.Trim) = 0 Then
            DriverId.Clear()
            DriverName.Clear()
            Return
        End If

        bm.LostFocus(DriverId, DriverName, "select Name from Drivers where Id=" & DriverId.Text.Trim())
    End Sub
    Private Sub DriverId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DriverId.KeyUp
        If bm.ShowHelp("Drivers", DriverId, DriverName, e, "select cast(Id as varchar(100)) Id,Name from Drivers", "Drivers") Then
            DriverId_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub NolonPricesId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CustomerId.LostFocus, NolonPricesId.LostFocus, NolonPriceTypeId.LostFocus, NolonPriceTypeId.SelectionChanged
        If Val(NolonPricesId.Text.Trim) = 0 Then
            NolonPricesId.Clear()
            NolonPricesName.Clear()
            Return
        End If
        bm.LostFocus(NolonPricesId, NolonPricesName, "select dbo.GetNolonPricesName(Id) from NolonPrices where Id=" & NolonPricesId.Text.Trim())
        If lop2 Then Return

        Dim Field As String = "Price1"
        Select Case NolonPriceTypeId.SelectedValue
            Case 1 : Field = "Price1"
            Case 2 : Field = "Price2"
            Case 3 : Field = "Price3"
            Case 4 : Field = "Price4"
            Case 5 : Field = "Price5"
            Case Else : Return
        End Select
        bm.LostFocus(NolonPricesId, Value, "select " & Field & " from NolonPrices where Id=" & NolonPricesId.Text.Trim())
        Value2.Text = Val(bm.ExecuteScalar("select top 1 Value2 from Nolon where InvoiceNo<" & Val(txtID.Text) & " and CustomerId=" & Val(CustomerId.Text) & " and NolonPriceId=" & Val(NolonPricesId.Text) & " and NolonPriceTypeId=" & NolonPriceTypeId.SelectedValue & " order by InvoiceNo desc"))

        If Val(Value2.Text) = 0 Then
            Value2.Text = Value.Text
        End If
    End Sub
    Private Sub NolonPricesId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles NolonPricesId.KeyUp
        If bm.ShowHelp("NolonPrices", NolonPricesId, NolonPricesName, e, "select cast(Id as varchar(100)) Id,dbo.GetNolonPricesName(Id) Name from NolonPrices") Then
            NolonPricesId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub Value_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Value.TextChanged, Payed.TextChanged
        If lop2 Then Return
        Remaining.Text = Val(Value.Text) - Val(Payed.Text)
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
        lblValue.SetResourceReference(ContentProperty, "Value")
        lblDriver.SetResourceReference(ContentProperty, "Driver")
        lblNolonPriceId.SetResourceReference(ContentProperty, "Direction")
        ''

        lblDueDate.SetResourceReference(CheckBox.ContentProperty, "DueDate")
        Done.SetResourceReference(CheckBox.ContentProperty, "Done")

        lblPayments.SetResourceReference(ContentProperty, "Payments")
        lblOtherPayments.SetResourceReference(ContentProperty, "OtherPayments")

    End Sub

    Private Sub CustomerInvoiceNo_TextChanged(sender As Object, e As TextChangedEventArgs) Handles CustomerInvoiceNo.TextChanged
        CustomerId.IsEnabled = Val(CustomerInvoiceNo.Text) = 0
        'btnSave.IsEnabled = Val(CustomerInvoiceNo.Text) = 0
    End Sub

    Private Sub DayDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles Docdate.SelectedDateChanged
        Try
            DueDate.SelectedDate = DateAdd(DateInterval.Day, 7, Docdate.SelectedDate.Value)
        Catch ex As Exception
        End Try
    End Sub







    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button4.Click
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
            myCommand = New SqlClient.SqlCommand("select Image from NolonAttachments where InvoiceNo='" & F1 & "' and AttachedName='" & F2 & "'" & bm.AppendWhere, con)
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
                    bm.ExecuteNonQuery("delete from NolonAttachments where InvoiceNo='" & txtID.Text & "' and AttachedName='" & TreeView1.SelectedItem.Header & "'" & bm.AppendWhere)
                    LoadTree()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadTree()
        Dim dt As DataTable = bm.ExecuteAdapter("select AttachedName from NolonAttachments where InvoiceNo=" & txtID.Text & bm.AppendWhere)
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
                bm.SaveFile("NolonAttachments", "InvoiceNo", txtID.Text, "AttachedName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "Image", o.FileNames(i))
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
