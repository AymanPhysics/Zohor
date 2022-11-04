Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management
Imports System.ComponentModel
Imports System.IO

Public Class Tenders
    Public TableName As String = "TendersMaster"
    Public TableDetailsName As String = "TendersDetails"
    Public SubId As String = "InvoiceNo"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid

    Dim WithEvents BackgroundWorker1 As New BackgroundWorker

    Sub NewId()
        InvoiceNo.Clear()
        InvoiceNo.IsEnabled = Md.Manager
    End Sub

    Sub UndoNewId()
        InvoiceNo.IsEnabled = True
    End Sub

    Public Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()

        TabItem1.Height = 0
        DayDate.SelectedDate = Nothing
        DayDate.SelectedDate = bm.MyGetDate() 'Md.CurrentDate

        LoadWFH()


        bm.Fields = New String() {SubId, "DayDate", "ToId", "ReservToId", "Notes", "Total", "Total2", "Net", "Temp"}
        bm.control = New Control() {InvoiceNo, DayDate, ToId, ReservToId, Notes, Total, Total2, Net, Temp}
        bm.KeyFields = New String() {SubId}

        bm.Table_Name = TableName

        btnNew_Click(Nothing, Nothing)
    End Sub


    Structure GC
        Shared Barcode As String = "Barcode"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared UnitId As String = "UnitId"
        Shared UnitQty As String = "UnitQty"
        Shared Qty As String = "Qty"
        Shared Price As String = "Price"
        Shared Value As String = "Value"
        Shared UnitId2 As String = "UnitId2"
        Shared UnitQty2 As String = "UnitQty2"
        Shared Qty2 As String = "Qty2"
        Shared Price2 As String = "Price2"
        Shared Value2 As String = "Value2"
        Shared Line As String = "Line"
        Shared SD_Notes As String = "SD_Notes"
        Shared IsValid As String = "IsValid"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

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
        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.Price, "السعر")
        G.Columns.Add(GC.Value, "القيمة")


        Dim GCUnitId2 As New Forms.DataGridViewComboBoxColumn
        GCUnitId2.HeaderText = "الوحدة"
        GCUnitId2.Name = GC.UnitId2
        bm.FillCombo("select 0 Id,'' Name", GCUnitId2)
        G.Columns.Add(GCUnitId2)

        G.Columns.Add(GC.UnitQty2, "عدد الفرعى")
        G.Columns.Add(GC.Qty2, "الكمية")
        G.Columns.Add(GC.Price2, "السعر")
        G.Columns.Add(GC.Value2, "القيمة")


        G.Columns.Add(GC.Line, "Line")
        G.Columns.Add(GC.SD_Notes, "ملاحظات")

        Dim GCIsValid As New Forms.DataGridViewCheckBoxColumn
        GCIsValid.HeaderText = "اختيار"
        GCIsValid.Name = GC.IsValid
        G.Columns.Add(GCIsValid)


        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.Id).FillWeight = 110
        G.Columns(GC.Name).FillWeight = 280
        G.Columns(GC.SD_Notes).FillWeight = 280

        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.UnitQty).ReadOnly = True
        G.Columns(GC.UnitQty2).ReadOnly = True
        G.Columns(GC.Value).ReadOnly = True
        G.Columns(GC.Value2).ReadOnly = True
        G.Columns(GC.UnitQty).Visible = False
        G.Columns(GC.UnitQty2).Visible = False
        G.Columns(GC.Line).Visible = False

        G.BarcodeIndex = G.Columns(GC.Barcode).Index
        If Not Md.ShowBarcode Then
            G.Columns(GC.Barcode).Visible = False
            'btnPrint.Visibility = Visibility.Hidden
        End If

        If Md.MyProjectType = ProjectType.X Then
            G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
            For i As Integer = 0 To G.ColumnCount - 1
                G.Columns(i).Width = G.Columns(i).FillWeight
            Next
        End If

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
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
            G.Rows(i).Cells(GC.Qty2).Value = Val(G.Rows(i).Cells(GC.Qty2).Value)
            
            G.Rows(i).Cells(GC.Price).Value = Val(G.Rows(i).Cells(GC.Price).Value)
            G.Rows(i).Cells(GC.Price2).Value = Val(G.Rows(i).Cells(GC.Price2).Value)
            G.Rows(i).Cells(GC.Value).Value = Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.Price).Value)
            G.Rows(i).Cells(GC.Value2).Value = Val(G.Rows(i).Cells(GC.Qty2).Value) * Val(G.Rows(i).Cells(GC.Price2).Value)
        Catch ex As Exception
            ClearRow(i)
        End Try
        CalcTotal()
    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Barcode).Value = Nothing
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.UnitId).Value = Nothing
        G.Rows(i).Cells(GC.UnitQty).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.Price).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.UnitId2).Value = Nothing
        G.Rows(i).Cells(GC.UnitQty2).Value = Nothing
        G.Rows(i).Cells(GC.Qty2).Value = Nothing
        G.Rows(i).Cells(GC.Price2).Value = Nothing
        G.Rows(i).Cells(GC.Value2).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
        G.Rows(i).Cells(GC.SD_Notes).Value = Nothing
        G.Rows(i).Cells(GC.IsValid).Value = Nothing

    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(e.ColumnIndex).Name = GC.Barcode AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value Is Nothing Then
                Dim BC As String = G.Rows(e.RowIndex).Cells(GC.Barcode).Value.ToString
                If (Md.MyProjectType = ProjectType.X) AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                    If BC.Length > 12 AndAlso Val(BC.Substring(0, 1)) > 0 Then BC = BC.Substring(0, 12)
                    BC = BC.Substring(1)
                    G.Rows(e.RowIndex).Cells(GC.Id).Value = Val(Mid(BC, 1, BC.Length - 4))
                    LoadItemPrice(e.RowIndex)
                    Exit Sub
                ElseIf Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                    'G.Rows(e.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select Id from Items where IsStopped=0 and left(Barcode,12)='" & Val(BC) & "'")
                    G.Rows(e.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select Id from Items where IsStopped=0 and Barcode='" & BC & "'")
                    Exit Sub
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.Id Then
                AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
            End If
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            CalcRow(e.RowIndex)
        Catch ex As Exception
        End Try

    End Sub

    Sub AddItem(ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1)
        Try
            G.EndEdit()
            If Not TabControl1.SelectedIndex = 0 Then TabControl1.SelectedIndex = 0
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            'G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                For x As Integer = 0 To G.Rows.Count - 1
                    If Not G.Rows(x).Cells(GC.Id).Value Is Nothing AndAlso G.Rows(x).Cells(GC.Id).Value.ToString = Id.ToString AndAlso Not G.Rows(x).ReadOnly Then
                        If Md.MyProjectType = ProjectType.X Then
                            i = x
                            Exists = True
                            GoTo Br
                        Else
                            bm.ShowMSG("تم تكرار هذا الصنف بالسطر رقم " + (x + 1).ToString)
                        End If
                        Exit For
                    End If
                Next
                i = G.Rows.Add()
                G.CurrentCell = G.Rows(i).Cells(GC.Name)

Br:
            End If

            GetItemNameAndBal(i, Id)
            LoadItemUint(i)


            If Val(G.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            If Add + Val(G.Rows(i).Cells(GC.Qty).Value) > 1 Then Add = 0
            G.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Rows(i).Cells(GC.Qty).Value)

            LoadItemPrice(i)
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


    Private Sub ToId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ToId.KeyUp
        Dim Title, tbl As String
        If ReservToId.IsChecked Then
            tbl = "Suppliers"
            Title = "الموردين"
            If bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl, IIf(Md.MyProjectType = ProjectType.X, tbl, "")) Then
                ToId_LostFocus(sender, Nothing)
            End If
        Else
            If bm.ShowHelpCustomers(ToId, ToName, e) Then
                ToId_LostFocus(sender, Nothing)
            End If
        End If
    End Sub

    Private Sub ToId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToId.LostFocus
        'If GroupBoxPaymentType.Visibility = Visibility.Hidden Then Return
        Dim tbl As String
        If ReservToId.IsChecked Then
            tbl = "Suppliers"
        Else
            tbl = "Customers"
        End If
        bm.LostFocus(ToId, ToName, "select Name from " & tbl & " where Id=" & ToId.Text.Trim(), , , True)

        If lop Or Val(InvoiceNo.Text) > 0 Then Return

    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True


        UndoNewId()
        G.Rows.Clear()
        bm.FillControls(Me)
        ToId_LostFocus(Nothing, Nothing)


        Dim dt As DataTable = bm.ExecuteAdapter("select SD.* ,It.Name It_Name from TendersDetails SD left join Items It on(SD.ItemId=It.Id) where SD.InvoiceNo=" & InvoiceNo.Text)

        If dt.Rows.Count > 0 Then G.Rows.Add(dt.Rows.Count)
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.Barcode).Value = dt.Rows(i)("Barcode").ToString
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("It_Name").ToString
            GetItemNameAndBal(i, dt.Rows(i)("ItemId").ToString)
            LoadItemUint(i)
            G.Rows(i).Cells(GC.UnitId).Value = dt.Rows(i)("UnitId")
            G.Rows(i).Cells(GC.UnitQty).Value = dt.Rows(i)("UnitQty").ToString
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString

            G.Rows(i).Cells(GC.UnitId2).Value = dt.Rows(i)("UnitId2")
            G.Rows(i).Cells(GC.UnitQty2).Value = dt.Rows(i)("UnitQty2").ToString
            G.Rows(i).Cells(GC.Qty2).Value = dt.Rows(i)("Qty2").ToString
            G.Rows(i).Cells(GC.Price2).Value = dt.Rows(i)("Price2").ToString
            G.Rows(i).Cells(GC.Value2).Value = dt.Rows(i)("Value2").ToString

            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            G.Rows(i).Cells(GC.SD_Notes).Value = dt.Rows(i)("SD_Notes").ToString
            G.Rows(i).Cells(GC.IsValid).Value = dt.Rows(i)("IsValid")
            CalcRow(i)
        Next
        G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Id)
        CalcTotal()
        Notes.Focus()
        G.RefreshEdit()
        lop = False


        If Val(CaseInvoiceNo.Text) > 0 Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {InvoiceNo.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Md.Manager OrElse Md.MyProjectType = ProjectType.X Then DontClear = True
        btnSave_Click(sender, e)
        DontClear = False
    End Sub

    Dim AllowSave As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False

        If Not CType(sender, Button).IsEnabled Then Return

        Select Case Md.MyProjectType
            Case ProjectType.X
            Case Else
                For i As Integer = 0 To G.Rows.Count - 1
                    If Val(G.Rows(i).Cells(GC.Id).Value) > 0 Then
                        Exit For
                    End If
                    If i = G.Rows.Count - 1 Then Return
                Next
        End Select

        If DayDate.SelectedDate Is Nothing Then
            bm.ShowMSG("برجاء تحديد التاريخ ")
            DayDate.Focus()
            Return
        End If

        If Not bm.TestDateValidity(DayDate) Then Return
        If ToId.Visibility = Visibility.Visible AndAlso ToId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد " & lblToId.Content)
            ToId.Focus()
            Return
        End If

        G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Name)
        G.EndEdit()
        Try
            CalcRow(G.CurrentRow.Index)
        Catch ex As Exception
        End Try

        ToId.Text = Val(ToId.Text)
        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.All
        If btnSave.IsEnabled Then
            bm.DefineValues()
            If Not bm.Save(New String() {SubId}, New String() {InvoiceNo.Text.Trim}, State) Then
                If State = BasicMethods.SaveState.Insert Then
                    InvoiceNo.Text = ""
                End If
                Return
            End If

            If Not bm.SaveGrid(G, "TendersDetails", New String() {"InvoiceNo"}, New String() {InvoiceNo.Text}, New String() {"Barcode", "ItemId", "ItemName", "UnitId", "UnitQty", "Qty", "Price", "Value", "UnitId2", "UnitQty2", "Qty2", "Price2", "Value2", "SD_Notes", "IsValid"}, New String() {GC.Barcode, GC.Id, GC.Name, GC.UnitId, GC.UnitQty, GC.Qty, GC.Price, GC.Value, GC.UnitId2, GC.UnitQty2, GC.Qty2, GC.Price2, GC.Value2, GC.SD_Notes, GC.IsValid}, New VariantType() {VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.String, VariantType.Boolean}, New String() {GC.Id}, "Line", "Line") Then Return

        End If

        AllowSave = True
        If sender Is Nothing Then
            'PrintPone(sender, 0)
        End If

        If Not DontClear Then btnNew_Click(sender, e)

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Dim SalesSerialNoCount As Integer = 0
    Sub ClearControls()
        Try
            If looop Then Return
            NewId()
            Dim d As DateTime = bm.MyGetDate
            Try
                d = DayDate.SelectedDate
            Catch ex As Exception
            End Try

            bm.ClearControls(False)
            DayDate.SelectedDate = d

            ToId_LostFocus(Nothing, Nothing)
            Temp.Content = "ملغى"

            G.Rows.Clear()
            G.Focus()
            CalcTotal()
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "'")

            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "'")

            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {InvoiceNo.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub InvoiceNo_KeyUp(sender As Object, e As KeyEventArgs) Handles InvoiceNo.KeyUp
        If bm.ShowHelpMultiColumns(CType(Parent, Page).Title, InvoiceNo, InvoiceNo, e, "select cast(InvoiceNo as nvarchar(100))'رقم الفاتورة',cast(ToId as nvarchar(100))'كود العميل',dbo.GetCustomerName(ToId)'اسم العميل',dbo.ToStrDate(DayDate)'التاريخ' from TendersMaster") Then
            'InvoiceNo.Text = bm.SelectedRow(0)
            InvoiceNo_Leave(Nothing, Nothing)
        End If

    End Sub
    Public Sub InvoiceNo_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {InvoiceNo.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles InvoiceNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
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
        str = "Sales" & str

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
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "@InvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "Header"}
        rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, InvoiceNo.Text, InvoiceNo.Text, InvoiceNo.Text, NewItemsOnly, 0, 0, 0, CType(Parent, Page).Title}
        rpt.Rpt = ".rpt"
        rpt.Show()

    End Sub



    Dim LopCalc As Boolean = False
    Private Sub CalcTotal()
        If LopCalc Or lop Then Return
        Try
            LopCalc = True
            Total.Text = Math.Round(0, 2)
            Total2.Text = Math.Round(0, 2)
            For i As Integer = 0 To G.Rows.Count - 1
                Total.Text += Val(G.Rows(i).Cells(GC.Value).Value)
                Total2.Text += Val(G.Rows(i).Cells(GC.Value2).Value)
            Next
            Net.Text = Val(Total2.Text) - Val(Total.Text)
            LopCalc = False
        Catch ex As Exception
        End Try
    End Sub

    Dim DontClear As Boolean = False

    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Name).Index Then
                Dim str As String = "select cast(Id as varchar(100)) Id,Name," & PriceFieldName(GC.Price, 0) & " 'السعر' from Items where IsStopped=0 "
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, str) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId)
                    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                    End If

                End If
            End If


            If bm.ShowHelpGridItemBal(G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "GetItemCurrentBal " & Val(G.CurrentRow.Cells(GC.Id).Value)) Then
                GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                If G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId).Visible Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId)
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

    Private Sub LoadItemUint(i As Integer)
        Dim Id As Integer = Val(G.Rows(i).Cells(GC.Id).Value)

        bm.FillCombo("select 0 Id,Unit Name From Items where Id='" & Id & "'  union select 1 Id,UnitSub Name From Items where Id='" & Id & "'  union select 2 Id,UnitSub2 Name From Items where Id='" & Id & "' ", G.Rows(i).Cells(GC.UnitId))
        bm.FillCombo("select 0 Id,Unit Name From Items where Id='" & Id & "'  union select 1 Id,UnitSub Name From Items where Id='" & Id & "'  union select 2 Id,UnitSub2 Name From Items where Id='" & Id & "' ", G.Rows(i).Cells(GC.UnitId2))


        G.Rows(i).Cells(GC.UnitId).Value = 0
    End Sub

    Private Sub LoadItemPrice(i As Integer)
        Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items_View where Id='" & G.Rows(i).Cells(GC.Id).Value & "' ")


        If dt.Rows.Count = 0 Then Return
        G.Rows(i).Cells(GC.UnitQty).Value = UnitCount(dt, G.Rows(i).Cells(GC.UnitId).Value)

        'If dt.Rows(0)("AllowEditSalesPrice") = 1 Then
        '    G.Rows(i).Cells(GC.Price).ReadOnly = False
        'Else
        '    G.Rows(i).Cells(GC.Price).ReadOnly = True
        'End If


    End Sub

    Dim looop As Boolean = False

    Private Sub LoadVisibility()
        btnDelete.Visibility = Visibility.Visible
        btnFirst.Visibility = Visibility.Visible
        btnLast.Visibility = Visibility.Visible
        btnNext.Visibility = Visibility.Visible
        btnPrevios.Visibility = Visibility.Visible
        DayDate.Visibility = Visibility.Visible
        lblDayDate.Visibility = Visibility.Visible
        ReservToId.Visibility = Visibility.Hidden
        lblToId.Visibility = Visibility.Hidden
        ToId.Visibility = Visibility.Hidden
        ToName.Visibility = Visibility.Hidden
        btnAddCustomer.Visibility = Visibility.Hidden


        Temp.IsEnabled = Md.Manager

        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, InvoiceNo}, {})

    End Sub

    Private Sub Hide()
        ReservToId.Visibility = Visibility.Hidden
    End Sub

    Private Sub GetItemNameAndBal(i As Integer, Id As String)
        Dim str As String = "IsStopped=0 and"
        If lop Then str = ""

        Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items_View  where " & str & " Id='" & Id & "' ")
        Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
        If dr.Length = 0 Then
            If Not G.Rows(i).Cells(GC.Id).Value Is Nothing Or G.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
            ClearRow(i)
            CalcTotal()
            Return
        End If
        G.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id)
        G.Rows(i).Cells(GC.Name).Value = dr(0)(GC.Name)

    End Sub

    Private Sub btnAddCustomer_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCustomer.Click
        Dim frm As New MyWindow With {.Title = "Customers", .WindowState = WindowState.Maximized}
        bm.SetMySecurityType(frm, 816)
        frm.Content = New Customers With {.MyId = Val(ToId.Text)}
        frm.Show()
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
        ElseIf e.Key = Key.F8 Then
            btnNew_Click(Nothing, Nothing)
        End If
    End Sub


End Class
