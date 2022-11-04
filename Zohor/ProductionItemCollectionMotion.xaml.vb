Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class ProductionItemCollectionMotion

    Public MainTableName As String = "Stores"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"

    Public TableName As String = "ProductionItemCollectionMotionMaster"
    Public TableDetailsName2 As String = "ProductionItemCollectionMotionDetailsTo"

    Public MainId As String = "StoreId"
    Public SubId As String = "InvoiceNo"

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim StaticsDt As New DataTable
    WithEvents G As New MyGrid
    WithEvents MyTimer As New Threading.DispatcherTimer
    Public FirstColumn As String = "الكـــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "السعــــر", Statement As String = ""
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Public Flag As Integer

    Sub NewId()
        InvoiceNo.Clear()
        InvoiceNo.IsEnabled = False
    End Sub

    Sub UndoNewId()
        InvoiceNo.IsEnabled = True
    End Sub

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnPrint})
        txtFlag.Visibility = Visibility.Hidden
        If Not Md.Manager Then
            btnPrint2.Visibility = Visibility.Hidden
        End If

        LoadResource()
        DayDate.SelectedDate = Nothing
        DayDate.SelectedDate = bm.MyGetDate()

        RdoGrouping_Checked(Nothing, Nothing)
        LoadWFH(WFHTo, G)

        bm.FillCombo("Employees", SystemUser, "", , True)
        bm.FillCombo("Employees", EntryUser, "", , True)

        TabItem1.Header = "" ' TryCast(TryCast(Me.Parent, TabItem).Header, TabsHeader).MyTabHeader

        bm.Fields = {"Flag", MainId, SubId, "DayDate", "Notes", "DocNo", "MotionTypeId", "Temp", "ItemId", "Qty", "EmpId1", "EmpId2", "EmpId3", "EmpId4", "EntryUser", "EntryDate", "SystemUser", "UpdateDate", "NewInvoiceNo", "OreStoreId", "MachineId", "IsType1", "IsType2"}
        bm.control = {txtFlag, StoreId, InvoiceNo, DayDate, Notes, DocNo, MotionTypeId, Temp, ItemId, Qty, EmpId1, EmpId2, EmpId3, EmpId4, EntryUser, EntryDate, SystemUser, UpdateDate, NewInvoiceNo, OreStoreId, MachineId, IsType1, IsType2}
        bm.KeyFields = {"Flag", MainId, SubId}

        bm.Table_Name = TableName

        lblEntryUser.Visibility = Visibility.Hidden
        EntryUser.Visibility = Visibility.Hidden
        EntryDate.Visibility = Visibility.Hidden
        lblSystemUser.Visibility = Visibility.Hidden
        SystemUser.Visibility = Visibility.Hidden
        UpdateDate.Visibility = Visibility.Hidden
        NewInvoiceNo.Visibility = Visibility.Hidden

        LoadGroups()
        LoadAllItems()

        StoreId.Text = Md.DefaultStore
        StoreId_LostFocus(Nothing, Nothing)

        If Md.MyProjectType = ProjectType.X Then
            OreStoreId.Text = 3
            OreStoreId_LostFocus(Nothing, Nothing)
        End If
        btnNew_Click(Nothing, Nothing)
    End Sub


    Structure GC
        Shared Barcode As String = "Barcode"
        Shared ItemId As String = "ItemId"
        Shared ItemName As String = "ItemName"
        Shared Qty As String = "Qty"
        Shared TotalQty As String = "TotalQty"
        Shared Line As String = "Line"
    End Structure

    Private Sub LoadWFH(WFH As Forms.Integration.WindowsFormsHost, G As MyGrid)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Barcode, "الباركود")
        G.Columns.Add(GC.ItemId, "كود الصنف")
        G.Columns.Add(GC.ItemName, "اسم الصنف")

        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.TotalQty, "إجمالى الكمية")
        G.Columns.Add(GC.Line, "Line")

        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.ItemId).FillWeight = 110
        G.Columns(GC.ItemName).FillWeight = 300

        G.Columns(GC.ItemName).ReadOnly = True
        G.Columns(GC.TotalQty).ReadOnly = True

        G.BarcodeIndex = G.Columns(GC.Barcode).Index
        If Not Md.ShowBarcode Then
            G.Columns(GC.Barcode).Visible = False
        End If
        G.Columns(GC.Line).Visible = False

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.GotFocus, AddressOf GridGotFocus
    End Sub
    Function Fm() As Integer
        Return 0
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
            HelpDt = bm.ExecuteAdapter("Select cast(Id as nvarchar(100))Id,Name,SalesPrice Price From Items  where IsStopped=0 " & ItemWhere())
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
        Catch ex As Exception
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
            dv.RowFilter = " [" & FirstColumn & "] like '" & txtID.Text.Trim & "%' and [" & SecondColumn & "] like '%" & txtName.Text & "%' and [" & ThirdColumn & "] >=" & IIf(txtPrice.Text.Trim = "", 0, txtPrice.Text) & ""
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
        If IsCurrentMain Then
            ItemId.Text = Id
            ItemId_LostFocus(Nothing, Nothing)
            Return
        End If

        Try
            If Not TabControl1.SelectedIndex = 0 Then TabControl1.SelectedIndex = 0
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                For x As Integer = 0 To G.Rows.Count - 1
                    If Not G.Rows(x).Cells(GC.ItemId).Value Is Nothing AndAlso G.Rows(x).Cells(GC.ItemId).Value.ToString.Trim = Id.ToString.Trim AndAlso Not G.Rows(x).ReadOnly Then
                        i = x
                        Exists = True
                        GoTo Br
                    End If
                Next
                i = G.Rows.Add()
                G.CurrentCell = G.Rows(i).Cells(GC.ItemName)
Br:
            End If

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items_View  where /*IsStopped=0 and*/ Id='" & Id & "' " & ItemWhere())
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Rows(i).Cells(GC.ItemId).Value Is Nothing Or G.Rows(i).Cells(GC.ItemId).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
                ClearRow(i)

                Return
            End If
            G.Rows(i).Cells(GC.ItemId).Value = dr(0)("Id")
            G.Rows(i).Cells(GC.ItemName).Value = dr(0)("Name")


            If Val(G.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            G.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Rows(i).Cells(GC.Qty).Value)

            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Qty).Index, i))
            Try
                G.CurrentCell = G.Rows(i).Cells(GC.ItemName)
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
            Catch ex As Exception
            End Try

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
        If lop Then Return
        Try
            If G.Rows(i).Cells(GC.ItemId).Value Is Nothing OrElse G.Rows(i).Cells(GC.ItemId).Value.ToString.Trim = "" Then
                ClearRow(i)
                Return
            ElseIf G.Rows(i).Cells(GC.ItemId).Value Is Nothing OrElse G.Rows(i).Cells(GC.ItemId).Value.ToString.Trim = "" Then

            End If
            G.Rows(i).Cells(GC.Qty).Value = Val(G.Rows(i).Cells(GC.Qty).Value)
            G.Rows(i).Cells(GC.TotalQty).Value = Val(G.Rows(i).Cells(GC.Qty).Value) * Val(Qty.Text)


        Catch ex As Exception
            ClearRow(i)
        End Try

    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Barcode).Value = Nothing
        G.Rows(i).Cells(GC.ItemId).Value = Nothing
        G.Rows(i).Cells(GC.ItemName).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.TotalQty).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            Dim G As MyGrid = sender
            If G.Columns(e.ColumnIndex).Name = GC.ItemId Then
                AddItem(G.Rows(e.RowIndex).Cells(GC.ItemId).Value, e.RowIndex, 0)
            End If
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            CalcRow(e.RowIndex)
        Catch ex As Exception
        End Try
        'G.Rows(e.RowIndex).Cells(GC.TotalQty).Value = Val(MainQty.Text) * Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value)

    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
        If Not sender Is Nothing Then ClearControls()
    End Sub


    Private Sub MachineId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles MachineId.KeyUp
        If bm.ShowHelp("Machines", MachineId, MachineName, e, "select cast(Id as varchar(100)) Id,Name from Machines") Then
            MachineId_LostFocus(MachineId, Nothing)
        End If
    End Sub

    Private Sub MachineId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MachineId.LostFocus
        bm.LostFocus(MachineId, MachineName, "select Name from Machines where Id=" & MachineId.Text.Trim())
    End Sub


    Private Sub OreStoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles OreStoreId.KeyUp
        If bm.ShowHelp("Stores", OreStoreId, OreStoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            OreStoreId_LostFocus(OreStoreId, Nothing)
        End If
    End Sub

    Private Sub OreStoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles OreStoreId.LostFocus
        bm.LostFocus(OreStoreId, OreStoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & OreStoreId.Text.Trim())
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True
        UndoNewId()
        bm.FillControls(Me)
        OreStoreId_LostFocus(Nothing, Nothing)
        ItemId_LostFocus(Nothing, Nothing)
        EmpId1_LostFocus(Nothing, Nothing)
        EmpId2_LostFocus(Nothing, Nothing)
        EmpId3_LostFocus(Nothing, Nothing)
        EmpId4_LostFocus(Nothing, Nothing)
        MachineId_LostFocus(Nothing, Nothing)

        dt = bm.ExecuteAdapter("select SD.* from " & TableDetailsName2 & " SD where SD.Flag=" & Flag & " and SD.StoreId=" & StoreId.Text & " and SD.InvoiceNo=" & InvoiceNo.Text)
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Barcode).Value = dt.Rows(i)("Barcode").ToString.Trim
            G.Rows(i).Cells(GC.ItemId).Value = dt.Rows(i)("ItemId").ToString.Trim
            G.Rows(i).Cells(GC.ItemName).Value = dt.Rows(i)("ItemName").ToString.Trim
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString.Trim
            G.Rows(i).Cells(GC.TotalQty).Value = dt.Rows(i)("TotalQty").ToString.Trim
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            CalcRow(i)
        Next
        G.RefreshEdit()

        Notes.Focus()
        lop = False
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
        btnSave_Click(sender, e)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If StoreId.Text.Trim = "" Then Return
        If Not CType(sender, Button).IsEnabled Then Return

        If Val(ItemId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الصنف")
            ItemId.Focus()
            Return
        End If

        If Val(Qty.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد العدد")
            Qty.Focus()
            Return
        End If


        If Val(MachineId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الماكينة")
            MachineId.Focus()
            Return
        End If

        G.EndEdit()
        Try
            CalcRow(G.CurrentRow.Index)
        Catch ex As Exception
        End Try

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where Flag=" & Flag & " and " & MainId & "='" & StoreId.Text & "'")
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text
            'lblLastEntry.Foreground = System.Windows.Media.Brushes.Red
            'System.Threading.Thread.Sleep(500)
            'lblLastEntry.Foreground = System.Windows.Media.Brushes.Blue
            State = BasicMethods.SaveState.Insert
        End If


        SystemUser.SelectedValue = Md.UserName
        UpdateDate.Text = bm.ExecuteScalar("Select GETDATE()")

        If EntryUser.SelectedValue = 0 Then EntryUser.SelectedValue = Md.UserName
        If EntryDate.Text.Trim = "" Then EntryDate.Text = bm.ExecuteScalar("Select GETDATE()")


        bm.DefineValues()
        If Not bm.Save(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                InvoiceNo.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G, TableDetailsName2, New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text}, New String() {"Barcode", "ItemId", "ItemName", "Qty", "TotalQty"}, New String() {GC.Barcode, GC.ItemId, GC.ItemName, GC.Qty, GC.TotalQty}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal}, New String() {GC.ItemId}, "Line", "Line") Then Return

        If sender Is btnPrint OrElse sender Is btnPrint2 Then
            PrintPone(sender, 0)
            Return
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

            Dim st As String = StoreId.Text
            If st = "" Then st = "001"

            bm.ClearControls(False)
            txtFlag.Text = Flag
            ItemName.Clear()
            EmpName1.Clear()
            EmpName2.Clear()
            EmpName3.Clear()
            EmpName4.Clear()
            MachineName.Clear()

            MotionTypeId.SelectedIndex = 1
            IsType1.IsChecked = True

            DayDate.SelectedDate = bm.MyGetDate()

            SystemUser.SelectedValue = Md.UserName
            EntryUser.SelectedValue = Md.UserName

            StoreId.Text = st
            StoreId_LostFocus(Nothing, Nothing)
            OreStoreId_LostFocus(Nothing, Nothing)
            G.Rows.Clear()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where Flag=" & Flag & " and " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'            delete from " & TableDetailsName2 & " where Flag=" & Flag & " and " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'")

            btnNew_Click(sender, e)
        End If
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyDown, InvoiceNo.KeyDown, txtID.KeyDown, EmpId1.KeyDown, EmpId2.KeyDown, EmpId3.KeyDown, EmpId4.KeyDown, MachineId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtPrice.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub EmpId1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId1.KeyUp
        bm.ShowHelp("Employees", EmpId1, EmpName1, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId2_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId2.KeyUp
        bm.ShowHelp("Employees", EmpId2, EmpName2, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId3_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId3.KeyUp
        bm.ShowHelp("Employees", EmpId3, EmpName3, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId4_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId4.KeyUp
        bm.ShowHelp("Employees", EmpId4, EmpName4, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId1.LostFocus
        bm.LostFocus(EmpId1, EmpName1, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId1.Text.Trim() & " and Stopped=0 ")
    End Sub

    Private Sub EmpId2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId2.LostFocus
        bm.LostFocus(EmpId2, EmpName2, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId2.Text.Trim() & " and Stopped=0 ")
    End Sub

    Private Sub EmpId3_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId3.LostFocus
        bm.LostFocus(EmpId3, EmpName3, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId3.Text.Trim() & " and Stopped=0 ")
    End Sub

    Private Sub EmpId4_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId4.LostFocus
        bm.LostFocus(EmpId4, EmpName4, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId4.Text.Trim() & " and Stopped=0 ")
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

        rpt.paraname = New String() {"@FromDate", "@ToDate", "@MainFlag", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "Header", "@ItemId", "@TypeId", "@GroupId", "Manager", "@MachineId"}
        rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, 0, Flag, StoreId.Text, InvoiceNo.Text, InvoiceNo.Text, CType(Parent, Page).Title, ItemId.Text, 0, 0, IIf(Md.Manager, 1, 0), 0}
        rpt.Rpt = "ProductionItemCollectionMotion.rpt"
        If sender Is btnPrint2 Then
            rpt.Rpt = "ProductionItemCollectionMotion3.rpt"
        End If
        rpt.Show()

    End Sub


    Dim DontClear As Boolean = False


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        Dim G As MyGrid = sender
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.ItemId).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.ItemName).Index Then
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.ItemId), G.CurrentRow.Cells(GC.ItemName), e, "select cast(Id as varchar(100)) Id,Name,SalesPrice 'السعر' from Items where IsStopped=0 " & ItemWhere()) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemId).Index, G.CurrentCell.RowIndex))
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                    End If

                End If
            End If


            If bm.ShowHelpGridItemBal(G.CurrentRow.Cells(GC.ItemId), G.CurrentRow.Cells(GC.ItemName), e, "GetItemCurrentBal " & Val(G.CurrentRow.Cells(GC.ItemId).Value)) Then
                GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemId).Index, G.CurrentCell.RowIndex))
                If G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
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


    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ItemId.KeyUp
        If bm.ShowHelp("Items", ItemId, ItemName, e, "select cast(Id as varchar(100)) Id,Name from Items where IsStopped=0 " & ItemWhere()) Then
            ItemId_LostFocus(ItemId, Nothing)
        End If
    End Sub

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId.LostFocus
        bm.LostFocus(ItemId, ItemName, "select Name from Items where Id=" & ItemId.Text.Trim())
    End Sub


    Private Sub btnCalc_Click(sender As Object, e As RoutedEventArgs) Handles btnCalc.Click
        dt = bm.ExecuteAdapter("SELECT ItemId,ItemName,Qty FROM ItemComponants where MainItemId='" & ItemId.Text & "'")
        G.Rows.Clear()
        If dt.Rows.Count = 0 Then Return
        G.Rows.Add(dt.Rows.Count)
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows(i).Cells(GC.ItemId).Value = dt.Rows(i)("ItemId")
            G.Rows(i).Cells(GC.ItemName).Value = dt.Rows(i)("ItemName")
            G.Rows(i).Cells(GC.Qty).Value = Val(dt.Rows(i)("Qty"))
            CalcRow(i)
        Next
    End Sub

    Dim IsCurrentMain As Boolean = True


    Private Sub ItemId_GotFocus(sender As Object, e As RoutedEventArgs) Handles ItemId.GotFocus
        IsCurrentMain = True
    End Sub

    Private Sub GridGotFocus(sender As Object, e As EventArgs)
        IsCurrentMain = False
    End Sub

    Private Sub IsType1_Checked(sender As Object, e As RoutedEventArgs) Handles IsType1.Checked, IsType1.Unchecked
        IsType2.IsChecked = Not IsType1.IsChecked
        TestType()
    End Sub
    Private Sub IsType2_Checked(sender As Object, e As RoutedEventArgs) Handles IsType2.Checked, IsType2.Unchecked
        IsType1.IsChecked = Not IsType2.IsChecked
        TestType()
    End Sub

    Sub TestType()
        If IsType1.IsChecked Then
            lblEmpId2.Visibility = Visibility.Visible
            EmpId2.Visibility = Visibility.Visible
            EmpName2.Visibility = Visibility.Visible

            lblEmpId3.Content = "تغليف"
        Else
            lblEmpId2.Visibility = Visibility.Hidden
            EmpId2.Visibility = Visibility.Hidden
            EmpName2.Visibility = Visibility.Hidden

            lblEmpId3.Content = "تجليد"
        End If
    End Sub


End Class
