Imports System.Data
Public Class OperationTypes2
    Public TableName As String = "OperationTypes"
    Public TableDetailsName As String = "OperationPackageDescriptions"
    Public TableDetailsName2 As String = "OperationItems"
    Public SubId As String = "Id"



    Dim dt As New DataTable
    Dim bm As New BasicMethods
    WithEvents G0 As New MyGrid
    WithEvents G As New MyGrid

    Public Flag As Integer = 0

    Private Sub Clinics_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        bm.FillCombo("OperationDescriptions", OperationDescriptionId, "", "** Package **", True)

        bm.Fields = New String() {SubId, "Name", "OperationDescriptionId", "CalcBefore", "CalcAfter", "CalcConsumablesValue", "CalcItems"}
        bm.control = New Control() {txtID, txtName, OperationDescriptionId, CalcBefore, CalcAfter, CalcConsumablesValue, CalcItems}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        LoadWFH0()
        LoadWFH()

        btnNew_Click(sender, e)
    End Sub

    Structure GC0
        Shared DegreeId As String = "DegreeId"
        Shared DegreeName As String = "DegreeName"
        Shared RoomTypeId As String = "RoomTypeId"
        Shared RoomTypeName As String = "RoomTypeName"
        Shared Value As String = "Value"
        Shared ConsumablesValue As String = "ConsumablesValue"
        Shared NoOfDays As String = "NoOfDays"
        Shared Dr1Perc As String = "Dr1Perc"
        Shared Dr2Perc As String = "Dr2Perc"
        Shared Dr3Perc As String = "Dr3Perc"
        Shared AnesthetistPerc As String = "AnesthetistPerc"
        Shared RoomValue As String = "RoomValue"
        Shared InsAmount As String = "InsAmount"
    End Structure


    Private Sub LoadWFH0()
        WFH0.Child = G0

        G0.Columns.Clear()
        G0.ForeColor = System.Drawing.Color.DarkBlue

        G0.Columns.Add(GC0.DegreeId, "الكود")
        G0.Columns.Add(GC0.DegreeName, "الدرجة العلمية")
        G0.Columns.Add(GC0.RoomTypeId, "الكود")
        G0.Columns.Add(GC0.RoomTypeName, "درجة الغرفة")
        G0.Columns.Add(GC0.Value, "السعر")
        G0.Columns.Add(GC0.ConsumablesValue, "حد المستلزمات")
        G0.Columns.Add(GC0.NoOfDays, "مدة الإقامة")
        G0.Columns.Add(GC0.Dr1Perc, "نسبة الطبيب 1")
        G0.Columns.Add(GC0.Dr2Perc, "نسبة الطبيب 2")
        G0.Columns.Add(GC0.Dr3Perc, "نسبة الطبيب 3")
        G0.Columns.Add(GC0.AnesthetistPerc, "نسبة طبيب التخدير")
        G0.Columns.Add(GC0.RoomValue, "فتح غرفة العمليات")
        G0.Columns.Add(GC0.InsAmount, "مبلغ التأمين")

        G0.Columns(GC0.DegreeName).FillWeight = 300
        G0.Columns(GC0.RoomTypeName).FillWeight = 300

        G0.Columns(GC0.DegreeId).ReadOnly = True
        G0.Columns(GC0.DegreeName).ReadOnly = True
        G0.Columns(GC0.RoomTypeId).ReadOnly = True
        G0.Columns(GC0.RoomTypeName).ReadOnly = True

        G0.AllowUserToAddRows = False
        G0.AllowUserToDeleteRows = False

    End Sub
    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Qty As String = "Qty"
    End Structure

    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Id, "كود الصنف")
        G.Columns.Add(GC.Name, "اسم الصنف")
        G.Columns.Add(GC.Qty, "الكمية")

        G.Columns(GC.Name).FillWeight = 300

        G.Columns(GC.Name).ReadOnly = True

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown

    End Sub

    Dim RowLop As Boolean = False
    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        If RowLop Then Return
        RowLop = True
        Try
            If G.Columns(e.ColumnIndex).Name = GC.Id Then
                AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
            End If
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        Catch ex As Exception
        End Try
        RowLop = False
    End Sub


    Sub AddItem(ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1)
        Try
            G.EndEdit()
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If True Then 'i = -1
                For x As Integer = 0 To G.Rows.Count - 1
                    If x <> i AndAlso Not G.Rows(x).Cells(GC.Id).Value Is Nothing AndAlso G.Rows(x).Cells(GC.Id).Value.ToString = Id.ToString AndAlso Not G.Rows(x).ReadOnly Then
                        bm.ShowMSG("تم تكرار هذا الصنف بالسطر رقم " + (x + 1).ToString)
                        ClearRow(i)
                        Return
                    End If
                Next
                'i = G.Rows.Add()
                'G.CurrentCell = G.Rows(i).Cells(GC.Name)

Br:
            End If

            GetItemNameAndBal(i, Id)
            

            If Val(G.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            If Add + Val(G.Rows(i).Cells(GC.Qty).Value) > 1 Then Add = 0
            G.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Rows(i).Cells(GC.Qty).Value)

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

    Private Sub GetItemNameAndBal(i As Integer, Id As String)
        Dim dt As DataTable = bm.ExecuteAdapter("Select Id,Name From Items_View  where /*IsStopped=0 and*/ Id='" & Id & "' ")
        Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
        If dr.Length = 0 Then
            If Not G.Rows(i).Cells(GC.Id).Value Is Nothing Or G.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
            ClearRow(i)
            Return
        End If
        G.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id)
        G.Rows(i).Cells(GC.Name).Value = dr(0)(GC.Name)

    End Sub


    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
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
                Dim str As String = "select cast(Id as varchar(100)) Id,Name from Items where IsStopped=0 "
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, str) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        OperationDescriptionId_LostFocus(Nothing, Nothing)
        FillGrid0()
        FillGrid()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If

        G0.EndEdit()
        G.EndEdit()
        
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If Not bm.SaveGrid(G0, TableDetailsName, New String() {"OperationTypeId"}, New String() {txtID.Text}, New String() {"DegreeId", "RoomTypeId", "Value", "ConsumablesValue", "NoOfDays", "Dr1Perc", "Dr2Perc", "Dr3Perc", "AnesthetistPerc", "RoomValue", "InsAmount"}, New String() {GC0.DegreeId, GC0.RoomTypeId, GC0.Value, GC0.ConsumablesValue, GC0.NoOfDays, GC0.Dr1Perc, GC0.Dr2Perc, GC0.Dr3Perc, GC0.AnesthetistPerc, GC0.RoomValue, GC0.InsAmount}, New VariantType() {VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal}, New String() {GC0.DegreeId, GC0.RoomTypeId}) Then Return

        If Not bm.SaveGrid(G, TableDetailsName2, New String() {"OperationTypeId"}, New String() {txtID.Text}, New String() {"ItemId", "Qty"}, New String() {GC.Id, GC.Qty}, New VariantType() {VariantType.Integer, VariantType.Decimal}, New String() {GC.Id}) Then Return

        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True

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
        CalcBefore.IsChecked = True
        CalcConsumablesValue.IsChecked = True
        OperationDescriptionId_LostFocus(Nothing, Nothing)
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"
        FillGrid0()
        FillGrid()
        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'    delete from " & TableDetailsName & " where " & "OperationTypeId" & "='" & txtID.Text.Trim & "'    delete from " & TableDetailsName2 & " where " & "OperationTypeId" & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False


    Private Sub txtID_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp("OperationTypes", txtID, txtName, e, "Select cast(Id as varchar(10))Id," & Resources.Item("CboName") & " Name from OperationTypes where IsStopped=0") Then
            txtID_LostFocus(sender, Nothing)
        End If
    End Sub


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


    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
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
        lblArName.SetResourceReference(ContentProperty, "Name")


    End Sub

    Dim lop As Boolean = False
     

    Private Sub FillGrid0()
        dt = bm.ExecuteAdapter("select R.Id,R.Name,RT.Id Id2,RT.Name Name2,isnull(T.Value,0)Value,isnull(T.ConsumablesValue,0)ConsumablesValue,isnull(T.NoOfDays,0)NoOfDays,isnull(T.Dr1Perc,0)Dr1Perc,isnull(T.Dr2Perc,0)Dr2Perc,isnull(T.Dr3Perc,0)Dr3Perc,isnull(T.AnesthetistPerc,0)AnesthetistPerc,isnull(T.RoomValue,0)RoomValue,isnull(T.InsAmount,0)InsAmount from Degrees R cross join RoomTypes RT left join OperationPackageDescriptions T on(R.Id=T.DegreeId and RT.Id=T.RoomTypeId and T.OperationTypeId=" & Val(txtID.Text) & ")")
        G0.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G0.Rows.Add({dt.Rows(i)(0), dt.Rows(i)(1), dt.Rows(i)(2), dt.Rows(i)(3), dt.Rows(i)(4), dt.Rows(i)(5), dt.Rows(i)(6), dt.Rows(i)(7), dt.Rows(i)(8), dt.Rows(i)(9), dt.Rows(i)(10), dt.Rows(i)(11), dt.Rows(i)(12)})
        Next
    End Sub

    Private Sub FillGrid()
        dt = bm.ExecuteAdapter("select ItemId,dbo.GetItemName(ItemId)ItemName,Qty from OperationItems where OperationTypeId=" & Val(txtID.Text) & "")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add({dt.Rows(i)(0), dt.Rows(i)(1), dt.Rows(i)(2)})
        Next
    End Sub

    Private Sub OperationDescriptionId_LostFocus(sender As Object, e As RoutedEventArgs) Handles OperationDescriptionId.LostFocus
        If OperationDescriptionId.SelectedValue = 0 Then
            WFH0.Visibility = Visibility.Visible
            lblMotionType_Copy.Visibility = Visibility.Visible
            CalcBefore.Visibility = Visibility.Visible
            CalcAfter.Visibility = Visibility.Visible
        Else
            WFH0.Visibility = Visibility.Hidden
            lblMotionType_Copy.Visibility = Visibility.Hidden
            CalcBefore.Visibility = Visibility.Hidden
            CalcAfter.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Sub CalcAfter_Checked(sender As Object, e As RoutedEventArgs) Handles CalcAfter.Checked, CalcAfter.Unchecked
        CalcBefore.IsChecked = Not CalcAfter.IsChecked
    End Sub

    Private Sub CalcBefore_Checked(sender As Object, e As RoutedEventArgs) Handles CalcBefore.Checked, CalcBefore.Unchecked
        CalcAfter.IsChecked = Not CalcBefore.IsChecked
    End Sub

    Private Sub CalcConsumablesValue_Checked(sender As Object, e As RoutedEventArgs) Handles CalcConsumablesValue.Checked, CalcConsumablesValue.Unchecked
        CalcItems.IsChecked = Not CalcConsumablesValue.IsChecked
        If CalcItems.IsChecked Then
            WFH.Visibility = Visibility.Visible
        Else
            WFH.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Sub CalcItems_Checked(sender As Object, e As RoutedEventArgs) Handles CalcItems.Checked, CalcItems.Unchecked
        CalcConsumablesValue.IsChecked = Not CalcItems.IsChecked
        If CalcItems.IsChecked Then
            WFH.Visibility = Visibility.Visible
        Else
            WFH.Visibility = Visibility.Hidden
        End If
    End Sub
End Class
