Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class Occasion

    Public TableName As String = "Occasion"
    Public TableDetailsName As String = "OccasionDt"

    Public SubId As String = "InvoiceNo"

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    WithEvents MyTimer As New Threading.DispatcherTimer
    Public Flag As Integer
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Sub NewId()
        InvoiceNo.Clear()
        InvoiceNo.IsEnabled = Md.Manager
    End Sub

    Sub UndoNewId()
        InvoiceNo.IsEnabled = True
    End Sub

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, InvoiceNo}, {btnPrint, btnPrint2})
        LoadResource()
        
        TabItem1.Header = ""
        bm.Fields = New String() {SubId, "FromDate", "FromHour", "FromMinute", "ToDate", "ToHour", "ToMinute"}
        bm.control = New Control() {InvoiceNo, FromDate, FromHour, FromMinute, ToDate, ToHour, ToMinute}
        bm.KeyFields = New String() {SubId}

        bm.Table_Name = TableName

        LoadGroups()
        LoadWFH()
        FromDate.SelectedDate = Nothing
        FromDate.SelectedDate = bm.MyGetDate() 'Md.CurrentDate
        ToDate.SelectedDate = FromDate.SelectedDate
        If Md.MyProjectType = ProjectType.X Then
            PanelGroups.Visibility = Visibility.Hidden
            PanelTypes.Visibility = Visibility.Hidden
            PanelItems.Visibility = Visibility.Hidden

            TabControl1.Margin = New Thickness(0)
            TabControl1.BringIntoView()
        End If

        btnNew_Click(sender, e)
    End Sub


    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared SalesPrice As String = "SalesPrice"
        Shared DiscPerc As String = "DiscPerc"
        Shared DiscValue As String = "DiscValue"
        Shared SalesPriceAfter As String = "SalesPriceAfter"
        Shared Line As String = "Line"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Id, "الكود")
        G.Columns.Add(GC.Name, "الاسم")
        G.Columns.Add(GC.SalesPrice, "سعر البيع")
        G.Columns.Add(GC.DiscPerc, "نسبة الخصم")
        G.Columns.Add(GC.DiscValue, "قيمة الخصم")
        G.Columns.Add(GC.SalesPriceAfter, "السعر النهائى")
        G.Columns.Add(GC.Line, "Line")

        G.Columns(GC.Name).FillWeight = 400

        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.SalesPriceAfter).ReadOnly = True

        G.Columns(GC.SalesPrice).Visible = False
        G.Columns(GC.SalesPriceAfter).Visible = False
        G.Columns(GC.Line).Visible = False

        AddHandler G.CellEndEdit, AddressOf Grid_CellEndEdit
    End Sub

    Sub LoadGroups()
        Try
            WGroups.Children.Clear()
            WTypes.Children.Clear()
            WItems.Children.Clear()
            TabGroups.Header = Gp
            TabTypes.Header = Tp
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("LoadGroups2", New String() {"Form"}, New String() {0})
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

    Dim CurrentGroup As String = ""
    Dim CurrentGroupName As String = ""
    Dim CurrentType As String = ""
    Dim CurrentTypeName As String = ""
    Private Sub LoadTypes(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WTypes.Tag = xx.Tag
            WTypes.Children.Clear()
            WItems.Children.Clear()

            TabTypes.Header = Tp & " - " & xx.Content.ToString
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("LoadTypes2", New String() {"GroupId", "Form"}, New String() {xx.Tag.ToString, 0})
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

    Private Sub LoadItems(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WItems.Tag = xx.Tag
            WItems.Children.Clear()

            TabItems.Header = It & " - " & xx.Content.ToString

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items where " & ItemWhere() & " and GroupId=" & WTypes.Tag.ToString & " and TypeId=" & xx.Tag.ToString)
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
                i = G.Rows.Add()
Br:
            End If

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items where Id='" & Id & "' and " & ItemWhere())
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Rows(i).Cells(GC.Id).Value Is Nothing Or G.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
                ClearRow(i)
                Return
            End If
            G.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id)
            G.Rows(i).Cells(GC.Name).Value = dr(0)(GC.Name)

            LoadItemPrice(i)
            G.Rows(i).Cells(GC.DiscPerc).Value = Val(G.Rows(i).Cells(GC.DiscPerc).Value)
            G.Rows(i).Cells(GC.DiscValue).Value = Val(G.Rows(i).Cells(GC.DiscValue).Value)

            CalcRow(i)
            If Move Then
                G.Focus()
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.DiscPerc)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
            If Exists Then
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.DiscValue)
                G.CurrentCell = G.Rows(i).Cells(GC.DiscPerc)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
        Catch
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try
    End Sub


    Private Sub LoadItemPrice(i As Integer)
        Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items where Id='" & G.Rows(i).Cells(GC.Id).Value & "' and " & ItemWhere())
        If dt.Rows.Count = 0 Then Return
        G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(0)("SalesPrice")
    End Sub


    Function ItemWhere() As String
        Dim st As String = ""
        st = " ItemType in(0,1,2,3) "
        Return st
    End Function

    Dim lop As Boolean = False
    Sub FillControls()
        If lop Then Return
        lop = True
        UndoNewId()
        bm.FillControls(Me)
        Dim dt As DataTable = bm.ExecuteAdapter("select * from OccasionDt where InvoiceNo=" & InvoiceNo.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("ItemName").ToString
            G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(i)("SalesPrice").ToString
            G.Rows(i).Cells(GC.DiscPerc).Value = dt.Rows(i)("DiscPerc").ToString
            G.Rows(i).Cells(GC.DiscValue).Value = dt.Rows(i)("DiscValue").ToString
            G.Rows(i).Cells(GC.SalesPriceAfter).Value = dt.Rows(i)("SalesPriceAfter").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
        Next
        G.RefreshEdit()
        lop = False
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

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub btnPrint2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint2.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click


        G.EndEdit()

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text
            State = BasicMethods.SaveState.Insert
        End If

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {InvoiceNo.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                InvoiceNo.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G, "OccasionDt", New String() {"InvoiceNo"}, New String() {InvoiceNo.Text}, New String() {"ItemId", "ItemName", "SalesPrice", "DiscPerc", "DiscValue", "SalesPriceAfter"}, New String() {GC.Id, GC.Name, GC.SalesPrice, GC.DiscPerc, GC.DiscValue, GC.SalesPriceAfter}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal}, New String() {GC.Id}, "Line", "Line") Then Return

        If sender Is btnPrint Then
            PrintPone()
        ElseIf sender Is btnPrint2 Then
            PrintPone(True)
            Return
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
        FromDate.Focus()
    End Sub

    Sub ClearControls()
        Try
            NewId()
            bm.ClearControls(False)

            Dim MyNow As DateTime = bm.MyGetDate()
            FromDate.Text = MyNow.Date
            ToDate.Text = MyNow.Date
            FromHour.SelectedIndex = 0
            ToHour.SelectedIndex = 0
            FromMinute.SelectedIndex = 0
            ToMinute.SelectedIndex = 0

            G.Rows.Clear()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "'   delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub


    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {InvoiceNo.Text}, "Back", dt)
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

    
    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub PrintPone(Optional View As Boolean = False)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@InvoiceNo"}
        rpt.Header = Md.MyProjectType.ToString

        rpt.paravalue = New String() {InvoiceNo.Text}
        rpt.Rpt = "Occasion.rpt"
        If View Then
            rpt.Show()
        Else
            rpt.Print(, , 1)
        End If

    End Sub

    Dim DontClear As Boolean = False
    Sub CalcRow(ByVal i As Integer)
        Try
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value.ToString = "" Then
                ClearRow(i)
                Return
            End If
            G.Rows(i).Cells(GC.SalesPrice).Value = Val(G.Rows(i).Cells(GC.SalesPrice).Value)
            G.Rows(i).Cells(GC.DiscPerc).Value = Val(G.Rows(i).Cells(GC.DiscPerc).Value)
            G.Rows(i).Cells(GC.DiscValue).Value = Val(G.Rows(i).Cells(GC.DiscValue).Value)
            G.Rows(i).Cells(GC.SalesPriceAfter).Value = Val(G.Rows(i).Cells(GC.SalesPriceAfter).Value)
            G.Rows(i).Cells(GC.Line).Value = Val(G.Rows(i).Cells(GC.Line).Value)


            'If Val(G.Rows(i).Cells(GC.DiscValue).Value) = 0 AndAlso Val(G.Rows(i).Cells(GC.DiscPerc).Value) > 0 Then
            '    G.Rows(i).Cells(GC.DiscValue).Value = Val(G.Rows(i).Cells(GC.SalesPrice).Value) * Val(G.Rows(i).Cells(GC.DiscPerc).Value) / 100
            'End If

            'G.Rows(i).Cells(GC.SalesPriceAfter).Value = Val(G.Rows(i).Cells(GC.SalesPrice).Value) - Val(G.Rows(i).Cells(GC.DiscValue).Value)
        Catch ex As Exception
            ClearRow(i)
        End Try
    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.SalesPrice).Value = Nothing
        G.Rows(i).Cells(GC.DiscPerc).Value = Nothing
        G.Rows(i).Cells(GC.DiscValue).Value = Nothing
        G.Rows(i).Cells(GC.SalesPriceAfter).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
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

    Private Sub Grid_CellEndEdit(sender As Object, e As Forms.DataGridViewCellEventArgs)
        If G.Columns(e.ColumnIndex).Name = GC.Id Then
            AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
        End If
        G.Rows(e.RowIndex).Cells(GC.DiscPerc).Value = Val(G.Rows(e.RowIndex).Cells(GC.DiscPerc).Value)
        G.Rows(e.RowIndex).Cells(GC.DiscValue).Value = Val(G.Rows(e.RowIndex).Cells(GC.DiscValue).Value)
        G.Rows(e.RowIndex).Cells(GC.DiscPerc).Value = Val(G.Rows(e.RowIndex).Cells(GC.DiscPerc).Value)

        'If Val(G.Rows(e.RowIndex).Cells(GC.DiscValue).Value) = 0 Then
        '    G.Rows(e.RowIndex).Cells(GC.DiscValue).Value = Val(G.Rows(e.RowIndex).Cells(GC.SalesPrice).Value) * Val(G.Rows(e.RowIndex).Cells(GC.DiscPerc).Value) / 100
        'End If

        'G.Rows(e.RowIndex).Cells(GC.SalesPriceAfter).Value = Val(G.Rows(e.RowIndex).Cells(GC.SalesPrice).Value) - Val(G.Rows(e.RowIndex).Cells(GC.DiscValue).Value)


        If G.Columns(e.ColumnIndex).Name = GC.DiscValue AndAlso Val(G.Rows(e.RowIndex).Cells(GC.DiscValue).Value) <> 0 Then
            G.Rows(e.RowIndex).Cells(GC.DiscPerc).Value = 0
        End If

        If G.Columns(e.ColumnIndex).Name = GC.DiscPerc AndAlso Val(G.Rows(e.RowIndex).Cells(GC.DiscPerc).Value) <> 0 Then
            G.Rows(e.RowIndex).Cells(GC.DiscValue).Value = 0
        End If

        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        CalcRow(e.RowIndex)
    End Sub


End Class
