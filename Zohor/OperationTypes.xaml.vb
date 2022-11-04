Imports System.Data
Public Class OperationTypes
    Public TableName As String = "OperationTypes"
    Public TableDetailsName0 As String = "OperationTypeDegrees"
    Public TableDetailsName As String = "OperationTypeRooms"
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

        bm.Fields = New String() {SubId, "Name", "IsStopped"} ', "RoomValue", "Dr1Value", "Dr2Value", "Dr3Value", "AnesthetistValue", "CsValue", "CoValue", "ConsumablesValue", "Value"
        bm.control = New Control() {txtID, txtName, IsStopped} ', RoomValue, Dr1Value, Dr2Value, Dr3Value, AnesthetistValue, CsValue, CoValue, ConsumablesValue, Value
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        LoadWFH0()
        LoadWFH()

        btnNew_Click(sender, e)
    End Sub


    Structure GC0
        Shared DegreeId As String = "DegreeId"
        Shared DegreeName As String = "DegreeName"
        Shared Dr1Value As String = "Dr1Value"
        Shared Dr2Value As String = "Dr2Value"
        Shared Dr3Value As String = "Dr3Value"
        Shared AnesthetistValue As String = "AnesthetistValue"
        Shared CsValue As String = "CsValue"
        Shared MedicalValue As String = "MedicalValue"
        Shared CoValue As String = "CoValue"
        Shared ConsumablesValue As String = "ConsumablesValue"
        Shared Value As String = "Value"
        Shared Perc0 As String = "Perc0"
        Shared Perc As String = "Perc"
        Shared Total As String = "Total"
    End Structure


    Private Sub LoadWFH0()
        WFH0.Child = G0

        G0.Columns.Clear()
        G0.ForeColor = System.Drawing.Color.DarkBlue

        G0.Columns.Add(GC0.DegreeId, "الكود")
        G0.Columns.Add(GC0.DegreeName, "الدرجة العلمية")
        G0.Columns.Add(GC0.Dr1Value, "طبيب 1")
        G0.Columns.Add(GC0.Dr2Value, "طبيب 2")
        G0.Columns.Add(GC0.Dr3Value, "طبيب 3")
        G0.Columns.Add(GC0.AnesthetistValue, "طبيب تخدير")
        G0.Columns.Add(GC0.CsValue, "الرعاية التمريضية")
        G0.Columns.Add(GC0.MedicalValue, "الإشراف الطبي")
        G0.Columns.Add(GC0.CoValue, "أتعاب المستشفي")
        G0.Columns.Add(GC0.ConsumablesValue, "حد المستهلكات")
        G0.Columns.Add(GC0.Value, "الإجمالي")
        G0.Columns.Add(GC0.Perc0, "الخدمة %")
        G0.Columns.Add(GC0.Perc, "قيمة الخدمة")
        G0.Columns.Add(GC0.Total, "الإجمالي")

        G0.Columns(GC0.DegreeName).FillWeight = 300

        G0.Columns(GC0.DegreeId).ReadOnly = True
        G0.Columns(GC0.DegreeName).ReadOnly = True
        G0.Columns(GC0.Value).ReadOnly = True
        G0.Columns(GC0.Perc).ReadOnly = True
        G0.Columns(GC0.Total).ReadOnly = True

        If Not Md.MyProjectType = ProjectType.X Then
            G0.Columns(GC0.Perc0).Visible = False
            G0.Columns(GC0.Perc).Visible = False
            G0.Columns(GC0.Total).Visible = False
        End If

        G0.AllowUserToAddRows = False
        G0.AllowUserToDeleteRows = False

        AddHandler G0.CellEndEdit, AddressOf G0_CellEndEdit
        AddHandler G0.RowsAdded, AddressOf G0_RowsAdded
    End Sub
    Structure GC
        Shared RoomTypeId As String = "RoomTypeId"
        Shared RoomTypeName As String = "RoomTypeName"
        Shared Price As String = "Price"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.RoomTypeId, "الكود")
        G.Columns.Add(GC.RoomTypeName, "نوع الغرفة")
        G.Columns.Add(GC.Price, "سعر فتح الغرفة")

        G.Columns(GC.RoomTypeName).FillWeight = 300
        G.Columns(GC.Price).FillWeight = 150

        G.Columns(GC.RoomTypeId).ReadOnly = True
        G.Columns(GC.RoomTypeName).ReadOnly = True

        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
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

        If Not bm.SaveGrid(G0, TableDetailsName0, New String() {"OperationTypeId"}, New String() {txtID.Text}, New String() {"DegreeId", "Dr1Value", "Dr2Value", "Dr3Value", "AnesthetistValue", "CsValue", "MedicalValue", "CoValue", "ConsumablesValue", "Value", "Perc0", "Perc", "Total"}, New String() {GC0.DegreeId, GC0.Dr1Value, GC0.Dr2Value, GC0.Dr3Value, GC0.AnesthetistValue, GC0.CsValue, GC0.MedicalValue, GC0.CoValue, GC0.ConsumablesValue, GC0.Value, GC0.Perc0, GC0.Perc, GC0.Total}, New VariantType() {VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal}, New String() {GC0.DegreeId}) Then Return

        If Not bm.SaveGrid(G, TableDetailsName, New String() {"OperationTypeId"}, New String() {txtID.Text}, New String() {"RoomTypeId", "Price"}, New String() {GC.RoomTypeId, GC.Price}, New VariantType() {VariantType.Integer, VariantType.Decimal}, New String() {GC.RoomTypeId}) Then Return

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
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"
        FillGrid0()
        FillGrid()
        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'    delete from " & TableDetailsName0 & " where " & "OperationTypeId" & "='" & txtID.Text.Trim & "'    delete from " & TableDetailsName & " where " & "OperationTypeId" & "='" & txtID.Text.Trim & "'")
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

        IsStopped.SetResourceReference(CheckBox.ContentProperty, "IsStopped")

    End Sub

    Dim lop As Boolean = False

    Private Sub FillGrid0()
        dt = bm.ExecuteAdapter("select R.Id,R.Name,isnull(T.Dr1Value,0)Dr1Value,isnull(T.Dr2Value,0)Dr2Value,isnull(T.Dr3Value,0)Dr3Value,isnull(T.AnesthetistValue,0)AnesthetistValue,isnull(T.CsValue,0)CsValue,isnull(T.MedicalValue,0)MedicalValue,isnull(T.CoValue,0)CoValue,isnull(T.ConsumablesValue,0)ConsumablesValue,isnull(T.Value,0)Value from Degrees R left join " & TableDetailsName0 & " T on(R.Id=T.DegreeId and T.OperationTypeId=" & Val(txtID.Text) & ")")
        G0.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G0.Rows.Add({dt.Rows(i)(0), dt.Rows(i)(1), dt.Rows(i)(2), dt.Rows(i)(3), dt.Rows(i)(4), dt.Rows(i)(5), dt.Rows(i)(6), dt.Rows(i)(7), dt.Rows(i)(8), dt.Rows(i)(9), dt.Rows(i)(10)})
            G0_CellEndEdit(Nothing, New Forms.DataGridViewCellEventArgs(G0.Columns(GC0.Perc0).Index, i))
        Next
    End Sub

    Private Sub FillGrid()
        dt = bm.ExecuteAdapter("select R.Id,R.Name,isnull(T.Price,0)Price from RoomTypes R left join " & TableDetailsName & " T on(R.Id=T.RoomTypeId and T.OperationTypeId=" & Val(txtID.Text) & ")")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add({dt.Rows(i)(0), dt.Rows(i)(1), dt.Rows(i)(2)})
        Next
    End Sub

    Private Sub G0_CellEndEdit(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Dim i As Integer = e.RowIndex
        Try
            G0.Rows(i).Cells(GC0.Value).Value = Val(G0.Rows(i).Cells(GC0.Dr1Value).Value) + Val(G0.Rows(i).Cells(GC0.Dr2Value).Value) + Val(G0.Rows(i).Cells(GC0.Dr3Value).Value) + Val(G0.Rows(i).Cells(GC0.AnesthetistValue).Value) + Val(G0.Rows(i).Cells(GC0.CsValue).Value) + Val(G0.Rows(i).Cells(GC0.MedicalValue).Value) + Val(G0.Rows(i).Cells(GC0.CoValue).Value) + Val(G0.Rows(i).Cells(GC0.ConsumablesValue).Value)

            G0.Rows(i).Cells(GC0.Perc).Value = Val(G0.Rows(i).Cells(GC0.Perc0).Value) / 100 * (Val(G0.Rows(i).Cells(GC0.Value).Value) - Val(G0.Rows(i).Cells(GC0.ConsumablesValue).Value))
            G0.Rows(i).Cells(GC0.Total).Value = Val(G0.Rows(i).Cells(GC0.Value).Value) + Val(G0.Rows(i).Cells(GC0.Perc).Value)
        Catch ex As Exception
            G0.Rows(i).Cells(GC0.Perc).Value = ""
            G0.Rows(i).Cells(GC0.Total).Value = ""
            G0.Rows(i).Cells(GC0.Value).Value = ""
        End Try
    End Sub

    Private Sub G0_RowsAdded(sender As Object, e As Forms.DataGridViewRowsAddedEventArgs)
        G0.Rows(e.RowIndex).Cells(GC0.Perc0).Value = Val(bm.ExecuteScalar("select top 1 CaseInvoicesPerc0 from Statics"))
    End Sub


End Class
