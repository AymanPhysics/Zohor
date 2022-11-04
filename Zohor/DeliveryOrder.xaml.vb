Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class DeliveryOrder

    Public MainTableName As String = "Stores"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"

    Public TableName As String = "DeliveryOrderMaster"
    Public TableDetailsName As String = "DeliveryOrderDetails"

    Public MainId As String = "StoreId"
    Public SubId As String = "InvoiceNo"

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim StaticsDt As New DataTable
    WithEvents G As New MyGrid
    Public Flag As Integer = 0
    Public Receive As Boolean = False
    Public FirstColumn As String = "الكـــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "السعــــر", Statement As String = ""
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"


    Sub NewId()
        InvoiceNo.Clear()
        InvoiceNo.IsEnabled = Md.Manager
        DocNo.IsEnabled = True
    End Sub

    Sub UndoNewId()
        InvoiceNo.IsEnabled = True
        DocNo.IsEnabled = False
    End Sub

    Private Sub DeliveryOrder_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, InvoiceNo}, {btnPrint, btnPrint2})
        LoadResource()
        TabItem1.Height = 0

        bm.FillCombo("SalesFlags", SalesFlagId, " where Id in " & IIf(Flag = 0, "(13)", "(10,20,30)"))

        DayDate.SelectedDate = Nothing
        DayDate.SelectedDate = bm.MyGetDate() 'Md.CurrentDate
        LoadWFH()
        TabItem1.Header = ""
        LoadVisibility()

        bm.Fields = New String() {"Flag", MainId, SubId, "DayDate", "ToId", "Notes", "Cashier", "DocNo", "Temp", "SalesInvoiceNo", "DeliveryPersonId", "SalesFlagId"}
        bm.control = New Control() {txtFlag, StoreId, InvoiceNo, DayDate, ToId, Notes, CashierId, DocNo, Temp, SalesInvoiceNo, DeliveryPersonId, SalesFlagId}
        bm.KeyFields = New String() {"Flag", MainId, SubId}

        bm.Table_Name = TableName

        txtFlag.Text = Flag

        StoreId.Text = Md.DefaultStore
        looop = True
        StoreId_LostFocus(Nothing, Nothing)
        looop = False

        If Md.MyProjectType = ProjectType.X Then
            SalesInvoiceNo.IsEnabled = False
        Else
            DocNo.Visibility = Visibility.Collapsed
            lblDocNo.Visibility = Visibility.Collapsed
        End If
        If Flag = 1 Then
            lblToId.Content = "المورد"
        End If

        btnNew_Click(Nothing, Nothing)
    End Sub

    Private Sub InvoiceNo_KeyUp(sender As Object, e As KeyEventArgs) Handles InvoiceNo.KeyUp, DocNo.KeyUp
        Select Case Flag
            Case 0
                If bm.ShowHelpMultiColumns(CType(Parent, Page).Title, InvoiceNo, InvoiceNo, e, "select cast(InvoiceNo as nvarchar(100))'رقم المسلسل',cast(ToId as nvarchar(100))'كود العميل',dbo.GetCustomerName(ToId)'اسم العميل',dbo.ToStrDate(DayDate)'التاريخ',DocNo 'رقم أمر الصرف',SalesInvoiceNo 'رقم كشف الحساب' from DeliveryOrderMaster where StoreId=" & Val(StoreId.Text) & " and Flag=" & Flag.ToString) Then
                    'InvoiceNo.Text = bm.SelectedRow(0)
                    InvoiceNo_Leave(Nothing, Nothing)
                End If
            Case 1
                If bm.ShowHelpMultiColumns(CType(Parent, Page).Title, InvoiceNo, InvoiceNo, e, "select cast(InvoiceNo as nvarchar(100))'رقم المسلسل',cast(ToId as nvarchar(100))'كود المورد',dbo.GetSupplierName(ToId)'اسم المورد',dbo.ToStrDate(DayDate)'التاريخ',DocNo 'رقم أمر الصرف',SalesInvoiceNo 'رقم كشف الحساب' from DeliveryOrderMaster where StoreId=" & Val(StoreId.Text) & " and Flag=" & Flag.ToString) Then
                    'InvoiceNo.Text = bm.SelectedRow(0)
                    InvoiceNo_Leave(Nothing, Nothing)
                End If
        End Select

    End Sub


    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Qty As String = "Qty"
        Shared Qty2 As String = "Qty2"
        Shared SD_Qty2 As String = "SD_Qty2"
        Shared SD_Qty3 As String = "SD_Qty3"
        Shared Qty3 As String = "Qty3"
        Shared Line As String = "Line"
        Shared SD_Notes As String = "SD_Notes"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Id, "كود الصنف")
        G.Columns.Add(GC.Name, "اسم الصنف")

        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.Qty2, "الكمية المستلمة")
        G.Columns.Add(GC.SD_Qty2, "عبوة / كرتونة")
        G.Columns.Add(GC.SD_Qty3, "عدد الكراتين")
        G.Columns.Add(GC.Qty3, "المتبقي")
        
        G.Columns.Add(GC.Line, "Line")
        G.Columns.Add(GC.SD_Notes, "ملاحظات")

        G.Columns(GC.Id).FillWeight = 110
        G.Columns(GC.Name).FillWeight = 280
        G.Columns(GC.SD_Notes).FillWeight = 280

        G.Columns(GC.Id).ReadOnly = True
        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Qty).ReadOnly = True
        G.Columns(GC.SD_Qty2).ReadOnly = True
        G.Columns(GC.SD_Qty3).ReadOnly = True
        G.Columns(GC.Qty3).ReadOnly = True

        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False
        
        If Not Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.SD_Qty2).Visible = False
            G.Columns(GC.SD_Qty3).Visible = False
        End If

        G.Columns(GC.Line).Visible = False

        If Not Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.SD_Notes).Visible = False
        End If

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
    End Sub


    Dim lop As Boolean = False


    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.SD_Qty2).Value = Nothing
        G.Rows(i).Cells(GC.SD_Qty3).Value = Nothing
        G.Rows(i).Cells(GC.Qty2).Value = Nothing
        G.Rows(i).Cells(GC.Qty3).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
        G.Rows(i).Cells(GC.SD_Notes).Value = Nothing
    End Sub
    

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If e.ColumnIndex = G.Columns(GC.SD_Qty2).Index OrElse e.ColumnIndex = G.Columns(GC.Qty2).Index Then
                G.Rows(e.RowIndex).Cells(GC.SD_Qty2).Value = Val(G.Rows(e.RowIndex).Cells(GC.SD_Qty2).Value)
                G.Rows(e.RowIndex).Cells(GC.SD_Qty3).Value = (Val(G.Rows(e.RowIndex).Cells(GC.Qty2).Value) - (Val(G.Rows(e.RowIndex).Cells(GC.Qty2).Value) Mod Val(G.Rows(e.RowIndex).Cells(GC.SD_Qty2).Value))) / Val(G.Rows(e.RowIndex).Cells(GC.SD_Qty2).Value) + IIf(Val(G.Rows(e.RowIndex).Cells(GC.Qty2).Value) Mod Val(G.Rows(e.RowIndex).Cells(GC.SD_Qty2).Value) > 0, 1, 0)
                G.Rows(e.RowIndex).Cells(GC.Qty2).Value = Val(G.Rows(e.RowIndex).Cells(GC.Qty2).Value)
                G.Rows(e.RowIndex).Cells(GC.Qty3).Value = Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value) - Val(G.Rows(e.RowIndex).Cells(GC.Qty2).Value)
                'ElseIf G.Rows(e.RowIndex ).ColumnIndex = G.Columns(GC.Qty2).Index Then
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        Dim str As String = " where 1=1 "
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")" & str) Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Dim StoreUnitId As Integer = 0
    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        Dim str As String = ""
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim() & str)

        If Md.ShowQtySub Then
            StoreUnitId = Val(bm.ExecuteScalar("select StoreUnitId from Stores where Id=" & StoreId.Text))
        End If
        ClearControls()
        If Md.ShowShiftForEveryStore Then
            dt = bm.ExecuteAdapter("select CurrentDate,CurrentShift from Stores where Id=" & StoreId.Text.Trim())
            If dt.Rows.Count > 0 Then
                DayDate.SelectedDate = dt.Rows(0)("CurrentDate")
            End If
        End If

    End Sub


    Private Sub ToId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToId.LostFocus
        Dim tbl As String = "Customers"
        If Flag = 1 Then tbl = "Suppliers"
        bm.LostFocus(ToId, ToName, "select Name from " & tbl & " where Id=" & ToId.Text.Trim())
    End Sub
    Private Sub DeliveryPersonId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DeliveryPersonId.LostFocus
        Dim tbl As String = "DeliveryPersons"
        bm.LostFocus(DeliveryPersonId, DeliveryPersonName, "select Name from " & tbl & " where Id=" & DeliveryPersonId.Text.Trim())
    End Sub
    Private Sub DeliveryPersonId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DeliveryPersonId.KeyUp
        bm.ShowHelp("DeliveryPersons", DeliveryPersonId, DeliveryPersonName, e, "select cast(Id as varchar(100)) Id,Name from DeliveryPersons", "DeliveryPersons")
    End Sub

    Private Sub CashierId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CashierId.KeyUp
        bm.ShowHelp("Employees", CashierId, CashierName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Stopped=0")
    End Sub

    Private Sub CashierId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CashierId.LostFocus
        bm.LostFocus(CashierId, CashierName, "select " & Resources.Item("CboName") & " Name from Employees where Doctor=0 and Id=" & CashierId.Text.Trim() & " and Stopped=0 ")
    End Sub


    Sub FillControls()
        If lop Then Return
        lop = True
        UndoNewId()
        G.Rows.Clear()
        bm.FillControls(Me)
        ToId_LostFocus(Nothing, Nothing)
        DeliveryPersonId_LostFocus(Nothing, Nothing)
        CashierId_LostFocus(Nothing, Nothing)

        Dim dt As DataTable = bm.ExecuteAdapter("select SD.* ,It.Name It_Name from DeliveryOrderDetails SD left join Items It on(SD.ItemId=It.Id) where SD.StoreId=" & StoreId.Text & " and SD.InvoiceNo=" & InvoiceNo.Text & " and SD.Flag=" & Flag)

        If dt.Rows.Count > 0 Then G.Rows.Add(dt.Rows.Count)
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("It_Name").ToString
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            G.Rows(i).Cells(GC.SD_Qty2).Value = dt.Rows(i)("SD_Qty2").ToString
            G.Rows(i).Cells(GC.SD_Qty3).Value = dt.Rows(i)("SD_Qty3").ToString
            G.Rows(i).Cells(GC.Qty2).Value = dt.Rows(i)("Qty2").ToString
            G.Rows(i).Cells(GC.Qty3).Value = dt.Rows(i)("Qty3").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            G.Rows(i).Cells(GC.SD_Notes).Value = dt.Rows(i)("SD_Notes").ToString
        Next
        If G.Rows.Count > 0 Then G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Id)
        Notes.Focus()
        G.RefreshEdit()
        lop = False

        If Val(CaseInvoiceNo.Text) > 0 Then
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

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click, btnPrint2.Click
        DontClear = True
        btnSave_Click(sender, e)
        DontClear = False
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If StoreId.Text.Trim = "" Then Return
        If Not CType(sender, Button).IsEnabled Then Return

        If DeliveryPersonId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد مندوب التسليم")
            DeliveryPersonId.Focus()
            Return
        End If

        If SalesFlagId.SelectedIndex = 0 Then
            bm.ShowMSG("برجاء تحديد النوع")
            SalesFlagId.Focus()
            Return
        End If






        Try
            If DayDate.SelectedDate < bm.ExecuteScalar("Select DayDate from SalesMaster where Flag=" & SalesFlagId.SelectedValue & " and StoreId=" & Val(StoreId.Text) & " and InvoiceNo=" & Val(SalesInvoiceNo.Text)) Then
                bm.ShowMSG("برجاء تحديد تاريخ صحيح")
                DayDate.Focus()
                Return
            End If
        Catch
        End Try


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

        G.EndEdit()

        If Not G.CurrentRow Is Nothing AndAlso G.CurrentRow.Index >= 0 Then GridCalcRow(Nothing, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Qty2).Index, G.CurrentRow.Index))


        ToId.Text = Val(ToId.Text)
        If Md.ShowShifts AndAlso Not Md.Manager Then
            DayDate.SelectedDate = Md.CurrentDate
        End If


        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & StoreId.Text & "'" & " and Flag=" & Flag)
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text
            State = BasicMethods.SaveState.Insert
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

            bm.SaveGrid(G, "DeliveryOrderDetails", New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text}, New String() {"ItemId", "ItemName", "Qty", "SD_Qty2", "SD_Qty3", "Qty2", "Qty3", "SD_Notes"}, New String() {GC.Id, GC.Name, GC.Qty, GC.SD_Qty2, GC.SD_Qty3, GC.Qty2, GC.Qty3, GC.SD_Notes}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.String}, New String() {GC.Id}, "Line", "Line")

        End If

        Select Case CType(sender, Button).Name
            Case btnPrint.Name, btnPrint2.Name
                State = BasicMethods.SaveState.Print
        End Select

        TraceInvoice(State.ToString)
        If sender Is btnPrint OrElse sender Is btnPrint2 Then
            PrintPone(sender, 0)
        End If

        btnNew_Click(sender, e)

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {"Flag", MainId, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        Try
            If looop Then Return
            NewId()
            Dim d As DateTime = bm.MyGetDate
            Try
                d = DayDate.SelectedDate
            Catch ex As Exception
            End Try

            DayDate.DisplayDateStart = "2000-1-1"
            Dim st As String = StoreId.Text

            bm.ClearControls(False)
            DeliveryPersonId_LostFocus(Nothing, Nothing)
            If Flag = 0 Then SalesFlagId.SelectedIndex = 1

            txtFlag.Text = Flag
            If Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X Then CashierId.Text = Md.UserName


            CashierId_LostFocus(Nothing, Nothing)
            ToId_LostFocus(Nothing, Nothing)

            Temp.Visibility = Visibility.Visible
            Temp.Content = "ملغى"
            

            If Not Md.Manager Then
                DayDate.SelectedDate = bm.MyGetDate() 'Md.CurrentDate
                If Md.ShowShifts Then
                    DayDate.SelectedDate = Md.CurrentDate
                End If

                CashierId.Text = Md.UserName
                CashierId_LostFocus(Nothing, Nothing)
            Else
                DayDate.SelectedDate = d
                DayDate.SelectedDate = bm.MyGetDate()
            End If

            'DayDate.SelectedDate = bm.MyGetDate()
            'Shift.SelectedValue = Md.CurrentShiftId

            StoreId.Text = st

            txtFlag.Text = Flag

            G.Rows.Clear()
            
        Catch
        End Try
        
        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            TraceInvoice("Deleted")
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)

            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)

            btnNew_Click(sender, e)
        End If
    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExecuteNonQuery("BeforeDeleteDeliveryOrder", New String() {"Flag", "StoreId", "InvoiceNo", "UserDelete", "State"}, New String() {Flag, StoreId.Text, InvoiceNo.Text, Md.UserName, State})
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub InvoiceNo_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyDown, InvoiceNo.KeyDown, ToId.KeyDown, CashierId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
            End If
        Catch ex As Exception
        End Try
    End Sub

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
        rpt.paraname = New String() {"@Flag", "@StoreId", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {Flag, StoreId.Text, InvoiceNo.Text, CType(sender, Button).Content & IIf(Flag = 1, " مورد", " عميل")}
        rpt.Rpt = "DeliveryOrder.rpt"
        If sender Is btnPrint2 Then rpt.Rpt = "DeliveryOrder2.rpt"
        rpt.Show()
    End Sub


    Dim DontClear As Boolean = False

    Private Sub btnDeliveryClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        InvoiceNo.Text = x.Tag
        InvoiceNo_Leave(Nothing, Nothing)
        TabControl1.SelectedItem = TabItem1
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As RoutedEventArgs) Handles btnRefresh.Click
        If bm.ShowDeleteMSG("سيتم مسح المستند وإعادة إنشائه .. استمرار؟") Then
            TraceInvoice("Deleted")
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)

            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'" & " and Flag=" & Flag)


            Dim ss As String = DocNo.Text
            btnNew_Click(sender, e)
            DocNo.Text = ss
            DocNo_LostFocus(Nothing, Nothing)

            DontClear = True
            btnSave_Click(btnSave, Nothing)
            DontClear = False
        End If
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


    Dim looop As Boolean = False

    Private Sub LoadVisibility()

        btnDelete.Visibility = Visibility.Visible
        btnFirst.Visibility = Visibility.Visible
        btnLast.Visibility = Visibility.Visible
        btnNext.Visibility = Visibility.Visible
        btnPrevios.Visibility = Visibility.Visible
        btnPrint.Visibility = Visibility.Visible
        btnPrint2.Visibility = Visibility.Visible
        CashierId.Visibility = Visibility.Hidden
        CashierName.Visibility = Visibility.Hidden
        DayDate.Visibility = Visibility.Visible
        lblCashier.Visibility = Visibility.Hidden
        lblDayDate.Visibility = Visibility.Visible
        DocNo.Visibility = Visibility.Visible
        lblDocNo.Visibility = Visibility.Visible
        

        lblCashier.Visibility = Visibility.Visible
        CashierId.Visibility = Visibility.Visible
        CashierName.Visibility = Visibility.Visible
        lblCashier.Content = "المستخدم"

        
    End Sub

    Private Sub DocNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles DocNo.LostFocus
        If DocNo.Text.Trim = "" Then Return

        Dim dt2 As DataTable = bm.ExecuteAdapter("GetSalesSerialDetails", {"Flag", "StoreId", "DocNo", "SalesTypeIdState"}, {Flag, Val(StoreId.Text), Val(DocNo.Text), IIf(Md.MyProjectType = ProjectType.X, 0, 1)})
        G.Rows.Clear()
        If dt2.Rows.Count = 0 Then
            bm.ShowMSG("تم استلام الكميات بالكامل")
            Return
        End If

        ToId.Text = dt2.Rows(0)("ToId").ToString
        ToName.Text = dt2.Rows(0)("ToName").ToString
        SalesInvoiceNo.Text = dt2.Rows(0)("InvoiceNo").ToString
        SalesFlagId.SelectedValue = dt2.Rows(0)("SM_Flag")

        DayDate.DisplayDateStart = dt2.Rows(0)("DayDate")

        For i = 0 To dt2.Rows.Count - 1
            If Val(dt2.Rows(i)("Qty3").ToString) = 0 Then Continue For
            Dim x As Integer = G.Rows.Add()
            G.Rows(x).HeaderCell.Value = (x + 1).ToString
            G.Rows(x).Cells(GC.Id).Value = dt2.Rows(i)("ItemId").ToString
            G.Rows(x).Cells(GC.Name).Value = dt2.Rows(i)("It_Name").ToString
            G.Rows(x).Cells(GC.Qty).Value = dt2.Rows(i)("Qty3").ToString
            G.Rows(x).Cells(GC.SD_Qty2).Value = dt2.Rows(i)("SD_Qty2").ToString
            G.Rows(x).Cells(GC.SD_Qty3).Value = 0
            G.Rows(x).Cells(GC.Qty2).Value = dt2.Rows(i)("Qty3").ToString
            G.Rows(x).Cells(GC.Qty3).Value = 0
            G.Rows(x).Cells(GC.Line).Value = 0
            G.Rows(x).Cells(GC.SD_Notes).Value = ""
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SD_Qty2).Index, x))
        Next

    End Sub

    Private Sub SalesInvoiceNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles SalesInvoiceNo.LostFocus
        If SalesInvoiceNo.Text.Trim = "" Then Return

        If SalesFlagId.SelectedIndex = 0 Then
            bm.ShowMSG("برجاء تحديد النوع")
            SalesFlagId.Focus()
            Return
        End If

        Dim dt2 As DataTable = bm.ExecuteAdapter("GetSalesSerialDetailsByInvoiceNo", {"Flag", "StoreId", "InvoiceNo", "SalesFlagId"}, {Flag, Val(StoreId.Text), Val(SalesInvoiceNo.Text), SalesFlagId.SelectedValue})
        G.Rows.Clear()
        If dt2.Rows.Count = 0 Then Return

        ToId.Text = dt2.Rows(0)("ToId").ToString
        ToName.Text = dt2.Rows(0)("ToName").ToString
        SalesInvoiceNo.Text = dt2.Rows(0)("InvoiceNo").ToString

        DayDate.DisplayDateStart = dt2.Rows(0)("DayDate")

        For i = 0 To dt2.Rows.Count - 1
            If Val(dt2.Rows(i)("Qty3").ToString) = 0 Then Continue For
            Dim x As Integer = G.Rows.Add()
            G.Rows(x).HeaderCell.Value = (x + 1).ToString
            G.Rows(x).Cells(GC.Id).Value = dt2.Rows(i)("ItemId").ToString
            G.Rows(x).Cells(GC.Name).Value = dt2.Rows(i)("It_Name").ToString
            G.Rows(x).Cells(GC.Qty).Value = dt2.Rows(i)("Qty3").ToString
            G.Rows(x).Cells(GC.SD_Qty2).Value = dt2.Rows(i)("SD_Qty2").ToString
            G.Rows(x).Cells(GC.SD_Qty3).Value = 0
            G.Rows(x).Cells(GC.Qty2).Value = dt2.Rows(i)("Qty3").ToString
            G.Rows(x).Cells(GC.Qty3).Value = 0
            G.Rows(x).Cells(GC.Line).Value = 0
            G.Rows(x).Cells(GC.SD_Notes).Value = ""
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SD_Qty2).Index, x))
        Next

    End Sub
End Class
