Imports System.Data

Public Class OperationDescriptions
    Public TableName As String = "OperationDescriptions"
    Public SubId As String = "Id"
    Public SubName As String = "Name"
     


    Dim dt As New DataTable
    Dim bm As New BasicMethods


    Public Flag As Integer = 0
    WithEvents G0 As New MyGrid

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnPrintAll})
        LoadResource()
        btnPrintAll.Visibility = Visibility.Hidden
         
        bm.Fields = {SubId, SubName} ', "Value", "Perc"
        bm.control = {txtID, txtName} ', Value, Perc
        bm.KeyFields = {SubId}

        bm.Table_Name = TableName
        LoadWFH0()

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

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        FillGrid0()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If
        G0.EndEdit()

        bm.DefineValues()
        
        If Not bm.SaveGrid(G0, TableName, New String() {"Id"}, New String() {txtID.Text}, New String() {"DegreeId", "RoomTypeId", "Value", "ConsumablesValue", "NoOfDays", "Dr1Perc", "Dr2Perc", "Dr3Perc", "AnesthetistPerc", "RoomValue", "InsAmount"}, New String() {GC0.DegreeId, GC0.RoomTypeId, GC0.Value, GC0.ConsumablesValue, GC0.NoOfDays, GC0.Dr1Perc, GC0.Dr2Perc, GC0.Dr3Perc, GC0.AnesthetistPerc, GC0.RoomValue, GC0.InsAmount}, New VariantType() {VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal}, New String() {GC0.DegreeId, GC0.RoomTypeId}) Then Return

        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        btnNew_Click(sender, e)
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls()
        FillGrid0()

        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG("MsgDelete") Then
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

    Private Sub txtID_KeyUp(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyUp
        bm.ShowHelp(CType(Parent, Page).Title, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName)
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")
        btnPrintAll.SetResourceReference(ContentProperty, "Print All")

        lblId.SetResourceReference(ContentProperty, "Id")
        LblName.SetResourceReference(ContentProperty, "Name")
        
    End Sub

    Private Sub btnPrintAll_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintAll.Click
        bm.PrintTbl(CType(Parent, Page).Title, TableName)
    End Sub

    Private Sub FillGrid0()
        dt = bm.ExecuteAdapter("select R.Id,R.Name,RT.Id Id2,RT.Name Name2,isnull(T.Value,0)Value,isnull(T.ConsumablesValue,0)ConsumablesValue,isnull(T.NoOfDays,0)NoOfDays,isnull(T.Dr1Perc,0)Dr1Perc,isnull(T.Dr2Perc,0)Dr2Perc,isnull(T.Dr3Perc,0)Dr3Perc,isnull(T.AnesthetistPerc,0)AnesthetistPerc,isnull(T.RoomValue,0)RoomValue,isnull(T.InsAmount,0)InsAmount from Degrees R cross join RoomTypes RT left join OperationDescriptions T on(R.Id=T.DegreeId and RT.Id=T.RoomTypeId and T.Id=" & Val(txtID.Text) & ")")
        G0.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G0.Rows.Add({dt.Rows(i)(0), dt.Rows(i)(1), dt.Rows(i)(2), dt.Rows(i)(3), dt.Rows(i)(4), dt.Rows(i)(5), dt.Rows(i)(6), dt.Rows(i)(7), dt.Rows(i)(8), dt.Rows(i)(9), dt.Rows(i)(10), dt.Rows(i)(11), dt.Rows(i)(12)})
        Next
    End Sub

End Class
