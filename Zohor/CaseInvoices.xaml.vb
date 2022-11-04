Imports System.Data
Imports System.ComponentModel
Imports System.IO

Public Class CaseInvoices
    Public TableName As String = "CaseInvoices"
    Public SubId As String = "InvoiceNo"

    Dim dt As New DataTable
    Dim bm As New BasicMethods
    WithEvents G0 As New MyGrid
    WithEvents G1 As New MyGrid
    WithEvents G2 As New MyGrid
    WithEvents G3 As New MyGrid
    WithEvents G4 As New MyGrid

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnPrint, btnPrint2, btnPrint3, btnPrintWithoutSave})
        bm.FillCombo("select '' Id,'-' Name union select Id,Name from chart where LinkFile=3 order by Id", RemainingMainAccNo)
        RemainingMainAccNo_LostFocus(Nothing, Nothing)

        btnGetAllCases.Visibility = Visibility.Hidden

        LoadResource()
        LoadWFH0()
        LoadWFH1()
        LoadWFH2()
        LoadWFH3()
        LoadWFH4()

        bm.Fields = New String() {SubId, "DayDate", "CaseId", "Notes", "Total00", "Total0", "Total01", "Total1", "Total2", "Total3", "Total4", "Perc", "Total", "FromDate", "ToDate", "Discount", "Bal", "ConsumablesValue", "Remaining", "RemainingMainAccNo", "RemainingSubAccNo", "Perc0", "IsPosted"}
        bm.control = New Control() {txtID, DayDate, CaseId, Notes, Total00, Total0, Total01, Total1, Total2, Total3, Total4, Perc, Total, FromDate, ToDate, Discount, Bal, ConsumablesValue, Remaining, RemainingMainAccNo, RemainingSubAccNo, Perc0, IsPosted}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)

        btnExit.SetResourceReference(ContentProperty, "Patient Leaving")

        'FromDate.SelectedDate = Now.Date
        ToDate.SelectedDate = Now.Date
        If Md.MyProjectType = ProjectType.Zohor Then
            TabItem5.Visibility = Visibility.Hidden
            FromDate.SelectedDate = Now.Date
        End If
    End Sub

    Structure GC0
        Shared FlagId As String = "FlagId"
        Shared FlagName As String = "FlagName"
        Shared DayDate As String = "DayDate"
        Shared StoreId As String = "StoreId"
        Shared StoreName As String = "StoreName"
        Shared InvoiceNo As String = "InvoiceNo"
        Shared ItemId As String = "ItemId"
        Shared ItemName As String = "ItemName"
        Shared Qty As String = "Qty"
        Shared Value As String = "Value"
        Shared IsNew As String = "IsNew"
        Shared Notes As String = "Notes"
    End Structure

    Private Sub LoadWFH0() 'SalesMaster
        WFH0.Child = G0

        G0.Columns.Clear()
        G0.ForeColor = System.Drawing.Color.DarkBlue
        G0.Columns.Add(GC0.FlagId, "كود")
        G0.Columns.Add(GC0.FlagName, "النوع")
        G0.Columns.Add(GC0.DayDate, "التاريخ")
        G0.Columns.Add(GC0.StoreId, "كود المخزن")
        G0.Columns.Add(GC0.StoreName, "المخزن")
        G0.Columns.Add(GC0.InvoiceNo, "مسلسل")
        G0.Columns.Add(GC0.ItemId, "كود البند")
        G0.Columns.Add(GC0.ItemName, "البند")
        G0.Columns.Add(GC0.Qty, "الكمية")
        G0.Columns.Add(GC0.Value, "القيمة")
        G0.Columns.Add(GC0.IsNew, "IsNew")
        G0.Columns.Add(GC0.Notes, "ملاحظات")

        G0.Columns(GC0.ItemName).FillWeight = 300
        G0.Columns(GC0.Value).FillWeight = 100
        G0.Columns(GC0.Notes).FillWeight = 300

        G0.Columns(GC0.FlagId).Visible = False
        G0.Columns(GC0.FlagName).ReadOnly = True
        G0.Columns(GC0.DayDate).ReadOnly = True
        G0.Columns(GC0.StoreId).Visible = False
        G0.Columns(GC0.StoreName).ReadOnly = True
        G0.Columns(GC0.InvoiceNo).ReadOnly = True
        G0.Columns(GC0.ItemId).ReadOnly = True
        G0.Columns(GC0.ItemName).ReadOnly = True
        G0.Columns(GC0.Qty).ReadOnly = True
        G0.Columns(GC0.Value).ReadOnly = True
        G0.Columns(GC0.Notes).ReadOnly = True
        G0.Columns(GC0.IsNew).Visible = False

        G0.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G0.AllowUserToAddRows = False

    End Sub

    Structure GC1
        Shared FlagId As String = "FlagId"
        Shared FlagName As String = "FlagName"
        Shared DayDate As String = "DayDate"
        Shared StoreId As String = "StoreId"
        Shared StoreName As String = "StoreName"
        Shared InvoiceNo As String = "InvoiceNo"
        Shared ItemName As String = "ItemName"
        Shared Value As String = "Value"
        Shared IsNew As String = "IsNew"
    End Structure

    Private Sub LoadWFH1() 'SalesMaster
        WFH1.Child = G1

        G1.Columns.Clear()
        G1.ForeColor = System.Drawing.Color.DarkBlue
        G1.Columns.Add(GC1.FlagId, "كود")
        G1.Columns.Add(GC1.FlagName, "النوع")
        G1.Columns.Add(GC1.DayDate, "التاريخ")
        G1.Columns.Add(GC1.StoreId, "كود المخزن")
        G1.Columns.Add(GC1.StoreName, "المخزن")
        G1.Columns.Add(GC1.InvoiceNo, "مسلسل")
        G1.Columns.Add(GC1.ItemName, "البند")
        G1.Columns.Add(GC1.Value, "القيمة")
        G1.Columns.Add(GC1.IsNew, "IsNew")

        G1.Columns(GC1.ItemName).FillWeight = 300
        G1.Columns(GC1.Value).FillWeight = 100

        G1.Columns(GC1.FlagId).Visible = False
        G1.Columns(GC1.FlagName).ReadOnly = True
        G1.Columns(GC1.DayDate).ReadOnly = True
        G1.Columns(GC1.StoreId).Visible = False
        G1.Columns(GC1.StoreName).ReadOnly = True
        G1.Columns(GC1.InvoiceNo).ReadOnly = True
        G1.Columns(GC1.ItemName).ReadOnly = True
        G1.Columns(GC1.Value).ReadOnly = True
        G1.Columns(GC1.IsNew).Visible = False

        G1.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G1.AllowUserToAddRows = False

    End Sub

    Structure GC2
        Shared InvoiceNo As String = "InvoiceNo"
        Shared GroupName As String = "GroupName"
        Shared TypeName As String = "TypeName"
        Shared Value As String = "Value"
        Shared IsNew As String = "IsNew"
    End Structure

    Private Sub LoadWFH2() 'Services
        WFH2.Child = G2

        G2.Columns.Clear()
        G2.ForeColor = System.Drawing.Color.DarkBlue
        G2.Columns.Add(GC2.InvoiceNo, "مسلسل")
        G2.Columns.Add(GC2.GroupName, "المجموعة")
        G2.Columns.Add(GC2.TypeName, "النوع")
        G2.Columns.Add(GC2.Value, "القيمة")
        G2.Columns.Add(GC2.IsNew, "IsNew")

        G2.Columns(GC2.GroupName).FillWeight = 300
        G2.Columns(GC2.TypeName).FillWeight = 300
        G2.Columns(GC2.Value).FillWeight = 100

        G2.Columns(GC2.InvoiceNo).ReadOnly = True
        G2.Columns(GC2.GroupName).ReadOnly = True
        G2.Columns(GC2.TypeName).ReadOnly = True
        G2.Columns(GC2.Value).ReadOnly = True
        G2.Columns(GC2.IsNew).Visible = False

        G2.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G2.AllowUserToAddRows = False

        AddHandler G2.CellDoubleClick, AddressOf G2_CellDoubleClick
    End Sub

    Structure GC3
        Shared InvoiceNo As String = "InvoiceNo"
        Shared DayDate As String = "DayDate"
        Shared OperationTypeName As String = "OperationTypeName"
        Shared Dr1Name As String = "Dr1Name"
        Shared ConsumablesValue As String = "ConsumablesValue"
        Shared NoOfDays As String = "NoOfDays"
        Shared Value As String = "Value"
        Shared IsNew As String = "IsNew"
    End Structure

    Private Sub LoadWFH3() 'OperationMotions
        WFH3.Child = G3

        G3.Columns.Clear()
        G3.ForeColor = System.Drawing.Color.DarkBlue
        G3.Columns.Add(GC3.InvoiceNo, "مسلسل")
        G3.Columns.Add(GC3.DayDate, "التاريخ")
        G3.Columns.Add(GC3.OperationTypeName, "العملية")
        G3.Columns.Add(GC3.Dr1Name, "الطبيب")
        G3.Columns.Add(GC3.ConsumablesValue, "حد المستهلكات")
        G3.Columns.Add(GC3.NoofDays, "NoOfDays")
        G3.Columns.Add(GC3.Value, "القيمة")
        G3.Columns.Add(GC3.IsNew, "IsNew")

        G3.Columns(GC3.OperationTypeName).FillWeight = 300
        G3.Columns(GC3.Dr1Name).FillWeight = 300
        G3.Columns(GC3.Value).FillWeight = 100

        G3.Columns(GC3.InvoiceNo).ReadOnly = True
        G3.Columns(GC3.DayDate).ReadOnly = True
        G3.Columns(GC3.OperationTypeName).ReadOnly = True
        G3.Columns(GC3.Dr1Name).ReadOnly = True
        G3.Columns(GC3.ConsumablesValue).ReadOnly = True
        G3.Columns(GC3.NoOfDays).Visible = False
        G3.Columns(GC3.Value).ReadOnly = True
        G3.Columns(GC3.IsNew).Visible = False

        G3.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G3.AllowUserToAddRows = False

        AddHandler G3.CellDoubleClick, AddressOf G3_CellDoubleClick
    End Sub

    Structure GC4
        Shared DayDate As String = "DayDate"
        Shared Living As String = "Living"
        Shared Supervision As String = "Supervision"
        Shared Care As String = "Care"
        Shared Value As String = "Value"
        Shared IsNew As String = "IsNew"
    End Structure

    Private Sub LoadWFH4() 'ClinicsHistory
        WFH4.Child = G4

        G4.Columns.Clear()
        G4.ForeColor = System.Drawing.Color.DarkBlue
        G4.Columns.Add(GC4.DayDate, "التاريخ")
        G4.Columns.Add(GC4.Living, "إقامة")
        G4.Columns.Add(GC4.Supervision, "الإشراف الطبي")
        G4.Columns.Add(GC4.Care, "الرعاية التمريضية")
        G4.Columns.Add(GC4.Value, "الإجمالي")
        G4.Columns.Add(GC4.IsNew, "IsNew")

        G4.Columns(GC4.Value).FillWeight = 100

        G4.Columns(GC4.DayDate).ReadOnly = True
        'G4.Columns(GC4.Living).ReadOnly = True
        'G4.Columns(GC4.Supervision).ReadOnly = True
        'G4.Columns(GC4.Care).ReadOnly = True
        G4.Columns(GC4.Value).ReadOnly = True
        G4.Columns(GC4.IsNew).Visible = False

        G4.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G4.AllowUserToAddRows = False
        AddHandler G4.CellEndEdit, AddressOf G4_CellEndEdit
    End Sub


    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True

        btnPrintWithoutSave.Visibility = Visibility.Hidden
        bm.FillControls(Me)
        RemainingMainAccNo_LostFocus(Nothing, Nothing)
        bm.FillControls(Me)

        CaseId_LostFocus(Nothing, Nothing)

        dt = bm.ExecuteAdapter("select M.Flag,dbo.GetSalesFlagName(M.Flag)FlagName,dbo.ToStrDate(M.DayDate)DayDate,M.StoreId,M.InvoiceNo,D.ItemId,D.ItemName,D.Qty,(Case when M.Flag=47 then 1. else -1. end)*D.Value Value,dbo.GetStoreName(M.StoreId)StoreName,M.Notes from SalesDetails D left join SalesMaster M on(M.StoreId=D.StoreId and M.Flag=D.Flag and M.InvoiceNo=D.InvoiceNo) where M.Flag in(47,48) and M.ToId=" & Val(CaseId.Text) & " and M.CaseInvoiceNo=" & txtID.Text)
        G0.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G0.Rows.Add()
            G0.Rows(i).Cells(GC0.FlagId).Value = dt.Rows(i)("Flag").ToString
            G0.Rows(i).Cells(GC0.FlagName).Value = dt.Rows(i)("FlagName").ToString
            G0.Rows(i).Cells(GC0.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G0.Rows(i).Cells(GC0.StoreId).Value = dt.Rows(i)("StoreId").ToString
            G0.Rows(i).Cells(GC0.StoreName).Value = dt.Rows(i)("StoreName").ToString
            G0.Rows(i).Cells(GC0.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G0.Rows(i).Cells(GC0.ItemId).Value = dt.Rows(i)("ItemId").ToString
            G0.Rows(i).Cells(GC0.ItemName).Value = dt.Rows(i)("ItemName").ToString
            G0.Rows(i).Cells(GC0.Qty).Value = dt.Rows(i)("Qty").ToString
            G0.Rows(i).Cells(GC0.Value).Value = dt.Rows(i)("Value").ToString
            G0.Rows(i).Cells(GC0.IsNew).Value = 0
            G0.Rows(i).Cells(GC0.Notes).Value = dt.Rows(i)("Notes").ToString
        Next
        G0.RefreshEdit()

        dt = bm.ExecuteAdapter("select M.Flag,dbo.GetSalesFlagName(M.Flag)FlagName,dbo.ToStrDate(M.DayDate)DayDate,M.StoreId,M.InvoiceNo,D.ItemName,(Case when M.Flag=37 then 1. else -1. end)*D.Value Value,dbo.GetStoreName(M.StoreId)StoreName from SalesDetails D left join SalesMaster M on(M.StoreId=D.StoreId and M.Flag=D.Flag and M.InvoiceNo=D.InvoiceNo) where M.Flag in(37,38) and M.ToId=" & Val(CaseId.Text) & " and M.CaseInvoiceNo=" & txtID.Text)
        G1.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G1.Rows.Add()
            G1.Rows(i).Cells(GC1.FlagId).Value = dt.Rows(i)("Flag").ToString
            G1.Rows(i).Cells(GC1.FlagName).Value = dt.Rows(i)("FlagName").ToString
            G1.Rows(i).Cells(GC1.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G1.Rows(i).Cells(GC1.StoreId).Value = dt.Rows(i)("StoreId").ToString
            G1.Rows(i).Cells(GC1.StoreName).Value = dt.Rows(i)("StoreName").ToString
            G1.Rows(i).Cells(GC1.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G1.Rows(i).Cells(GC1.ItemName).Value = dt.Rows(i)("ItemName").ToString
            G1.Rows(i).Cells(GC1.Value).Value = dt.Rows(i)("Value").ToString
            G1.Rows(i).Cells(GC1.IsNew).Value = 0
        Next
        G1.RefreshEdit()

        dt = bm.ExecuteAdapter("select InvoiceNo,dbo.GetServiceGroupName(ServiceGroupId)GroupName,dbo.GetServiceTypeName(ServiceGroupId,ServiceTypeId)TypeName,Value from services where Flag=2 and CaseId=" & Val(CaseId.Text) & " and CaseInvoiceNo=" & txtID.Text)
        G2.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G2.Rows.Add()
            G2.Rows(i).Cells(GC2.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G2.Rows(i).Cells(GC2.GroupName).Value = dt.Rows(i)("GroupName").ToString
            G2.Rows(i).Cells(GC2.TypeName).Value = dt.Rows(i)("TypeName").ToString
            G2.Rows(i).Cells(GC2.Value).Value = dt.Rows(i)("Value").ToString
            G2.Rows(i).Cells(GC2.IsNew).Value = 0
        Next
        G2.RefreshEdit()

        dt = bm.ExecuteAdapter("select InvoiceNo,dbo.ToStrDate(DayDate)DayDate,/*dbo.GetOperationTypesName(OperationTypeId)+' - '+ dbo.GetOperationTypesName(OperationTypeId2)+' - '+ dbo.GetOperationTypesName(OperationTypeId3)*/dbo.GetOperationMotionsOperationTypesNameAll(InvoiceNo) OperationTypeName,dbo.GetEmpArName(DrId1)Dr1Name,ConsumablesValue,NoOfDays,Value from OperationMotions where CaseId=" & Val(CaseId.Text) & " and CaseInvoiceNo=" & txtID.Text)
        G3.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G3.Rows.Add()
            G3.Rows(i).Cells(GC3.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G3.Rows(i).Cells(GC3.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G3.Rows(i).Cells(GC3.OperationTypeName).Value = dt.Rows(i)("OperationTypeName").ToString
            G3.Rows(i).Cells(GC3.Dr1Name).Value = dt.Rows(i)("Dr1Name").ToString
            G3.Rows(i).Cells(GC3.ConsumablesValue).Value = dt.Rows(i)("ConsumablesValue").ToString
            G3.Rows(i).Cells(GC3.NoOfDays).Value = dt.Rows(i)("NoOfDays").ToString
            G3.Rows(i).Cells(GC3.Value).Value = dt.Rows(i)("Value").ToString
            G3.Rows(i).Cells(GC3.IsNew).Value = 0
        Next
        G3.RefreshEdit()

        dt = bm.ExecuteAdapter("select dbo.ToStrDate(DayDate)DayDate,Living,Supervision,Care,Living+Supervision+Care Value  from ClinicsHistory where CaseId=" & Val(CaseId.Text) & " and CaseInvoiceNo=" & txtID.Text)
        G4.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G4.Rows.Add()
            G4.Rows(i).Cells(GC4.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G4.Rows(i).Cells(GC4.Living).Value = dt.Rows(i)("Living").ToString
            G4.Rows(i).Cells(GC4.Supervision).Value = dt.Rows(i)("Supervision").ToString
            G4.Rows(i).Cells(GC4.Care).Value = dt.Rows(i)("Care").ToString
            G4.Rows(i).Cells(GC4.Value).Value = dt.Rows(i)("Value").ToString
            G4.Rows(i).Cells(GC4.IsNew).Value = 0
        Next
        G4.RefreshEdit()

        CalcAll()

        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

        DayDate.Focus()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Val(CaseId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد العميل")
            CaseId.Focus()
            Return
        End If

        If Not bm.TestDateValidity(DayDate) Then Return

        G0.EndEdit()
        G1.EndEdit()
        G2.EndEdit()
        G3.EndEdit()
        G4.EndEdit()


        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        SaveG0()
        SaveG1()
        SaveG2()
        SaveG3()
        SaveG4()

        If sender Is btnSave Then btnNew_Click(sender, e)

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
        btnPrintWithoutSave.Visibility = Visibility.Visible
        bm.ClearControls()

        Perc0.Text = Val(bm.ExecuteScalar("select top 1 CaseInvoicesPerc0 from Statics"))

        RemainingMainAccNo_LostFocus(Nothing, Nothing)

        CaseId_LostFocus(Nothing, Nothing)

        G0.Rows.Clear()
        G1.Rows.Clear()
        G2.Rows.Clear()
        G3.Rows.Clear()
        G4.Rows.Clear()

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        ToDate.SelectedDate = MyNow
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"
        CalcAll()
        DayDate.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click


        ' ''While Val(txtID.Text) > 1
        ' ''    btnSave_Click(Nothing, Nothing)
        ' ''    btnPrevios_Click(Nothing, Nothing)
        ' ''End While
        ' ''bm.ShowMSG("Done")
        ' ''Return

        If sender Is btnPrintWithoutSave OrElse bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")

            bm.ExecuteNonQuery("UPDATE SalesMaster set CaseInvoiceNo=0 where Flag in(47,48) and CaseInvoiceNo='" & txtID.Text & "'")
            bm.ExecuteNonQuery("UPDATE SalesMaster set CaseInvoiceNo=0 where Flag in(37,38) and CaseInvoiceNo='" & txtID.Text & "'")
            bm.ExecuteNonQuery("UPDATE services set CaseInvoiceNo=0 where Flag=2 and CaseInvoiceNo='" & txtID.Text & "'")
            bm.ExecuteNonQuery("UPDATE OperationMotions set CaseInvoiceNo=0 where CaseInvoiceNo='" & txtID.Text & "'")
            bm.ExecuteNonQuery("UPDATE ClinicsHistory set CaseInvoiceNo=0 where CaseInvoiceNo='" & txtID.Text & "'")

            If Not sender Is btnPrintWithoutSave Then
                btnNew_Click(sender, e)
            End If

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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, CaseId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub

    

    Private Sub CaseId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select Code+' - '+Name from Cases where Id=" & CaseId.Text.Trim())
        CalcBal()
        'Dim s As String = ""
        'Dim dt As DataTable = bm.ExecuteAdapter("GetCaseData", New String() {"Id"}, New String() {Val(CaseId.Text)})
        'CaseId.ToolTip = ""
        'CaseName.ToolTip = ""
        'If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return
        'For i As Integer = 0 To dt.Columns.Count - 2
        '    s &= dt.Rows(0)(i).ToString & IIf(i = dt.Columns.Count - 1, "", vbCrLf)
        'Next
        'CaseId.ToolTip = s
        'CaseName.ToolTip = s
    End Sub

    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp
        'bm.ShowHelp("Cases", CaseId, CaseName, e, "select cast(Id as varchar(100)) Id,Name from Cases")
        If bm.ShowHelpCases(CaseId, CaseName, e) Then
            CaseId_LostFocus(Nothing, Nothing)
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
        lblCaseId.SetResourceReference(ContentProperty, "CaseId")
        lblNotes.SetResourceReference(ContentProperty, "Notes")
        'lblSheekPerson.SetResourceReference(ContentProperty, "SheekPerson")

    End Sub

    Private Sub btnGet_Click(sender As Object, e As RoutedEventArgs) Handles btnGet.Click
        Dim FD As String = "1900/01/01"
        If Not FromDate.SelectedDate Is Nothing Then FD = bm.ToStrDate(FromDate.SelectedDate)
        Dim TD As String = "1900/01/01"
        If Not ToDate.SelectedDate Is Nothing Then TD = bm.ToStrDate(ToDate.SelectedDate)

        dt = bm.ExecuteAdapter("select M.Flag,dbo.GetSalesFlagName(M.Flag)FlagName,dbo.ToStrDate(M.DayDate)DayDate,M.StoreId,M.InvoiceNo,D.ItemId,D.ItemName,D.Qty,(Case when M.Flag=47 then 1. else -1. end)*D.Value Value,dbo.GetStoreName(M.StoreId)StoreName,M.Notes from SalesDetails D left join SalesMaster M on(M.StoreId=D.StoreId and M.Flag=D.Flag and M.InvoiceNo=D.InvoiceNo) where M.Temp=0 and M.Flag in(47,48) and M.ToId=" & Val(CaseId.Text) & " and M.CaseInvoiceNo=0 and M.Daydate between '" & FD & "' and '" & TD & "' order by M.InvoiceNo")
        For i As Integer = G0.Rows.Count - 1 To 0 Step -1
            If G0.Rows(i).Cells(GC0.IsNew).Value = 1 Then
                G0.Rows.RemoveAt(i)
            End If
        Next
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim x As Integer = G0.Rows.Add()
            G0.Rows(x).Cells(GC0.FlagId).Value = dt.Rows(i)("Flag").ToString
            G0.Rows(x).Cells(GC0.FlagName).Value = dt.Rows(i)("FlagName").ToString
            G0.Rows(x).Cells(GC0.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G0.Rows(x).Cells(GC0.StoreId).Value = dt.Rows(i)("StoreId").ToString
            G0.Rows(x).Cells(GC0.StoreName).Value = dt.Rows(i)("StoreName").ToString
            G0.Rows(x).Cells(GC0.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G0.Rows(x).Cells(GC0.ItemId).Value = dt.Rows(i)("ItemId").ToString
            G0.Rows(x).Cells(GC0.ItemName).Value = dt.Rows(i)("ItemName").ToString
            G0.Rows(x).Cells(GC0.Qty).Value = dt.Rows(i)("Qty").ToString
            G0.Rows(x).Cells(GC0.Value).Value = dt.Rows(i)("Value").ToString
            G0.Rows(x).Cells(GC0.IsNew).Value = 1
            G0.Rows(i).Cells(GC0.Notes).Value = dt.Rows(i)("Notes").ToString
        Next
        G0.RefreshEdit()

        dt = bm.ExecuteAdapter("select M.Flag,dbo.GetSalesFlagName(M.Flag)FlagName,dbo.ToStrDate(M.DayDate)DayDate,M.StoreId,M.InvoiceNo,D.ItemName,(Case when M.Flag=37 then 1. else -1. end)*D.Value Value,dbo.GetStoreName(M.StoreId)StoreName from SalesDetails D left join SalesMaster M on(M.StoreId=D.StoreId and M.Flag=D.Flag and M.InvoiceNo=D.InvoiceNo) where M.Temp=0 and M.Flag in(37,38) and M.ToId=" & Val(CaseId.Text) & " and M.CaseInvoiceNo=0 and M.Daydate between '" & FD & "' and '" & TD & "'")
        For i As Integer = G1.Rows.Count - 1 To 0 Step -1
            If G1.Rows(i).Cells(GC1.IsNew).Value = 1 Then
                G1.Rows.RemoveAt(i)
            End If
        Next
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim x As Integer = G1.Rows.Add()
            G1.Rows(x).Cells(GC1.FlagId).Value = dt.Rows(i)("Flag").ToString
            G1.Rows(x).Cells(GC1.FlagName).Value = dt.Rows(i)("FlagName").ToString
            G1.Rows(x).Cells(GC1.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G1.Rows(x).Cells(GC1.StoreId).Value = dt.Rows(i)("StoreId").ToString
            G1.Rows(x).Cells(GC1.StoreName).Value = dt.Rows(i)("StoreName").ToString
            G1.Rows(x).Cells(GC1.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G1.Rows(x).Cells(GC1.ItemName).Value = dt.Rows(i)("ItemName").ToString
            G1.Rows(x).Cells(GC1.Value).Value = dt.Rows(i)("Value").ToString
            G1.Rows(x).Cells(GC1.IsNew).Value = 1
        Next
        G1.RefreshEdit()

        dt = bm.ExecuteAdapter("select InvoiceNo,dbo.GetServiceGroupName(ServiceGroupId)GroupName,dbo.GetServiceTypeName(ServiceGroupId,ServiceTypeId)TypeName,Value from services where Canceled=0 and Flag=2 and CaseId=" & Val(CaseId.Text) & " and CaseInvoiceNo=0 and Daydate between '" & FD & "' and '" & TD & "'")
        For i As Integer = G2.Rows.Count - 1 To 0 Step -1
            If G2.Rows(i).Cells(GC2.IsNew).Value = 1 Then
                G2.Rows.RemoveAt(i)
            End If
        Next
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim x As Integer = G2.Rows.Add()
            G2.Rows(x).Cells(GC2.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G2.Rows(x).Cells(GC2.GroupName).Value = dt.Rows(i)("GroupName").ToString
            G2.Rows(x).Cells(GC2.TypeName).Value = dt.Rows(i)("TypeName").ToString
            G2.Rows(x).Cells(GC2.Value).Value = dt.Rows(i)("Value").ToString
            G2.Rows(x).Cells(GC2.IsNew).Value = 1
        Next
        G2.RefreshEdit()

        dt = bm.ExecuteAdapter("select InvoiceNo,dbo.ToStrDate(DayDate)DayDate,/*dbo.GetOperationTypesName(OperationTypeId)*/dbo.GetOperationMotionsOperationTypesNameAll(InvoiceNo)OperationTypeName,dbo.GetEmpArName(DrId1)Dr1Name,ConsumablesValue,NoOfDays,Value from OperationMotions where Canceled=0 and CaseId=" & Val(CaseId.Text) & " and CaseInvoiceNo=0 and Daydate between '" & FD & "' and '" & TD & "'")
        For i As Integer = G3.Rows.Count - 1 To 0 Step -1
            If G3.Rows(i).Cells(GC3.IsNew).Value = 1 Then
                G3.Rows.RemoveAt(i)
            End If
        Next
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim x As Integer = G3.Rows.Add()
            G3.Rows(x).Cells(GC3.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G3.Rows(x).Cells(GC3.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G3.Rows(x).Cells(GC3.OperationTypeName).Value = dt.Rows(i)("OperationTypeName").ToString
            G3.Rows(x).Cells(GC3.Dr1Name).Value = dt.Rows(i)("Dr1Name").ToString
            G3.Rows(x).Cells(GC3.ConsumablesValue).Value = dt.Rows(i)("ConsumablesValue").ToString
            G3.Rows(x).Cells(GC3.NoOfDays).Value = dt.Rows(i)("NoOfDays").ToString
            G3.Rows(x).Cells(GC3.Value).Value = dt.Rows(i)("Value").ToString
            G3.Rows(x).Cells(GC3.IsNew).Value = 1
        Next
        G3.RefreshEdit()

        dt = bm.ExecuteAdapter("select *,Living+Supervision+Care Value from ClinicsHistory where CaseId=" & Val(CaseId.Text) & " and CaseInvoiceNo=0 and Daydate between '" & FD & "' and '" & TD & "'")

        For i As Integer = G4.Rows.Count - 1 To 0 Step -1
            If G4.Rows(i).Cells(GC4.IsNew).Value = 1 Then
                G4.Rows.RemoveAt(i)
            End If
        Next
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim x As Integer = G4.Rows.Add()
            G4.Rows(x).Cells(GC4.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G4.Rows(x).Cells(GC4.Living).Value = dt.Rows(i)("Living").ToString
            G4.Rows(x).Cells(GC4.Supervision).Value = dt.Rows(i)("Supervision").ToString
            G4.Rows(x).Cells(GC4.Care).Value = dt.Rows(i)("Care").ToString
            G4.Rows(x).Cells(GC4.Value).Value = dt.Rows(i)("Value").ToString
            G4.Rows(x).Cells(GC4.IsNew).Value = 1
        Next
        G4.RefreshEdit()
        CalcAll()
    End Sub



    Private Sub SaveG0()
        bm.ExecuteNonQuery("UPDATE SalesMaster set CaseInvoiceNo=0 where Flag in(47,48) and CaseInvoiceNo='" & txtID.Text & "'")
        For i As Integer = 0 To G0.Rows.Count - 1
            bm.ExecuteNonQuery("UPDATE SalesMaster set CaseInvoiceNo='" & txtID.Text & "' where Flag=" & G0.Rows(i).Cells(GC0.FlagId).Value & " and StoreId='" & G0.Rows(i).Cells(GC0.StoreId).Value & "' and InvoiceNo='" & G0.Rows(i).Cells(GC0.InvoiceNo).Value & "'")
        Next
    End Sub

    Private Sub SaveG1()
        bm.ExecuteNonQuery("UPDATE SalesMaster set CaseInvoiceNo=0 where Flag in(37,38) and CaseInvoiceNo='" & txtID.Text & "'")
        For i As Integer = 0 To G1.Rows.Count - 1
            bm.ExecuteNonQuery("UPDATE SalesMaster set CaseInvoiceNo='" & txtID.Text & "' where Flag=" & G1.Rows(i).Cells(GC1.FlagId).Value & " and StoreId='" & G1.Rows(i).Cells(GC1.StoreId).Value & "' and InvoiceNo='" & G1.Rows(i).Cells(GC1.InvoiceNo).Value & "'")
        Next
    End Sub

    Private Sub SaveG2()
        bm.ExecuteNonQuery("UPDATE Services set CaseInvoiceNo=0 where Flag=2 and CaseInvoiceNo='" & txtID.Text & "'")
        For i As Integer = 0 To G2.Rows.Count - 1
            bm.ExecuteNonQuery("UPDATE Services set CaseInvoiceNo='" & txtID.Text & "' where Flag=2 and InvoiceNo='" & G2.Rows(i).Cells(GC2.InvoiceNo).Value & "'")
        Next
    End Sub

    Private Sub SaveG3()
        bm.ExecuteNonQuery("UPDATE OperationMotions set CaseInvoiceNo=0 where CaseInvoiceNo='" & txtID.Text & "'")
        For i As Integer = 0 To G3.Rows.Count - 1
            bm.ExecuteNonQuery("UPDATE OperationMotions set CaseInvoiceNo='" & txtID.Text & "' where InvoiceNo='" & G3.Rows(i).Cells(GC3.InvoiceNo).Value & "'")
        Next
    End Sub

    Private Sub SaveG4()
        bm.ExecuteNonQuery("UPDATE ClinicsHistory set CaseInvoiceNo=0 where CaseInvoiceNo='" & txtID.Text & "'")
        For i As Integer = 0 To G4.Rows.Count - 1
            bm.ExecuteNonQuery("UPDATE ClinicsHistory set CaseInvoiceNo='" & txtID.Text & "',Living=" & Val(G4.Rows(i).Cells(GC4.Living).Value) & ",Supervision=" & Val(G4.Rows(i).Cells(GC4.Supervision).Value) & ",Care=" & Val(G4.Rows(i).Cells(GC4.Care).Value) & " where CaseId=" & Val(CaseId.Text) & " and DayDate='" & bm.ToStrDate(CType(G4.Rows(i).Cells(GC4.DayDate).Value, DateTime)) & "'")
        Next
    End Sub

    Private Sub btnDelete0_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete0.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من حذف هذا البند؟") Then
            G0.Rows.RemoveAt(G0.CurrentRow.Index)
            CalcAll()
        End If
    End Sub

    Private Sub btnDelete1_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete1.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من حذف هذا البند؟") Then
            G1.Rows.RemoveAt(G1.CurrentRow.Index)
            CalcAll()
        End If
    End Sub

    Private Sub btnDelete2_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete2.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من حذف هذا البند؟") Then
            G2.Rows.RemoveAt(G2.CurrentRow.Index)
            CalcAll()
        End If
    End Sub

    Private Sub btnDelete3_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete3.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من حذف هذا البند؟") Then
            G3.Rows.RemoveAt(G3.CurrentRow.Index)
            CalcAll()
        End If
    End Sub

    Private Sub btnDelete4_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete4.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من حذف هذا البند؟") Then
            G4.Rows.RemoveAt(G4.CurrentRow.Index)
            CalcAll()
        End If
    End Sub

    Private Sub btnPrint1_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click, btnPrint2.Click, btnPrint3.Click, btnPrintWithoutSave.Click
        btnSave_Click(sender, e)

        If sender Is btnPrint OrElse sender Is btnPrint2 OrElse sender Is btnPrint3 OrElse sender Is btnPrintWithoutSave Then
            Dim rpt As New ReportViewer
            rpt.paraname = New String() {"@CaseInvoiceNo", "Header"}
            rpt.paravalue = New String() {Val(txtID.Text), "فاتورة"}
            rpt.Rpt = "CaseInvoice.rpt"
            If Md.MyProjectType = ProjectType.Zohor Then
                rpt.Rpt = "CaseInvoice_Z.rpt"
            End If
            If sender Is btnPrintWithoutSave Then
                rpt.ShowDialog()
                btnDelete_Click(sender, Nothing)
            Else
                If sender Is btnPrint2 Then
                    rpt.Rpt = "CaseInvoice2.rpt"
                ElseIf sender Is btnPrint3 Then
                    rpt.Rpt = "CaseInvoice3.rpt"
                End If
                rpt.Show()
            End If

            Return
        End If

    End Sub

    
    Dim SearchLop As Boolean = False
    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnSearch.Click
        SearchLop = True
        bm.DefineValues()
        bm.SearchTable({SubId}, {txtID.Text.Trim}, cboSearch, "", {CaseId})
        SearchLop = False
    End Sub

    Private Sub cboSearch_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSearch.SelectionChanged
        If SearchLop Then Return
        txtID.Text = cboSearch.SelectedValue.ToString
        txtID_LostFocus(Nothing, Nothing)
    End Sub

    Sub CalcAll()
        Dim x As Decimal = 0

        Dim dd As Date = DateTime.Now.Date
        Dim dd2 As Date = DateTime.Now.Date
        If G3.Rows.Count > 0 Then
            dd = DateTime.Parse(G3.Rows(0).Cells(GC3.DayDate).Value)
            Dim MyNoOfDays As Integer = Val(G3.Rows(0).Cells(GC3.NoOfDays).Value)
            If MyNoOfDays > 0 Then dd2 = dd.AddDays(MyNoOfDays - 1)
        End If


        'إجمالي مستهلكات العمليات
        For i As Integer = 0 To G0.Rows.Count - 1
            x += Val(G0.Rows(i).Cells(GC0.Value).Value)
        Next
        'x = bm.Round(x)
        Total00.Text = x
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim str As String = ""
        For i As Integer = 0 To G3.Rows.Count - 1
            str &= G3.Rows(i).Cells(GC3.InvoiceNo).Value & ","
        Next
        If str.Length > 0 Then str = str.Substring(0, str.Length - 1)
        Dim ItemDt As DataTable = bm.ExecuteAdapter("select T.ItemId,SUM(Qty)Qty from OperationItems T left join OperationMotionsOperationTypes TT on(T.OperationTypeId=TT.OperationTypeId) where TT.InvoiceNo in(" & str & ") group by T.ItemId")
        'إضافي مستهلكات العمليات
        Dim xx As Decimal = 0 ' x
        For i As Integer = 0 To G0.Rows.Count - 1
            Dim q As Decimal = Val(G0.Rows(i).Cells(GC0.Qty).Value)
            If ItemDt.Rows.Count > 0 AndAlso ItemDt.Select("ItemId=" & G0.Rows(i).Cells(GC0.ItemId).Value).Length > 0 AndAlso ItemDt.Select("ItemId=" & G0.Rows(i).Cells(GC0.ItemId).Value)(0)("Qty") > 0 AndAlso DateTime.Parse(G0.Rows(i).Cells(GC0.DayDate).Value) >= dd AndAlso DateTime.Parse(G0.Rows(i).Cells(GC0.DayDate).Value) <= dd2 Then
                Try
                    q -= ItemDt.Select("ItemId=" & G0.Rows(i).Cells(GC0.ItemId).Value)(0)("Qty")
                    ItemDt.Select("ItemId=" & G0.Rows(i).Cells(GC0.ItemId).Value)(0)("Qty") -= Val(G0.Rows(i).Cells(GC0.Qty).Value)
                Catch ex As Exception
                End Try
            End If

            If ItemDt.Rows.Count > 0 AndAlso ItemDt.Select("ItemId=" & G0.Rows(i).Cells(GC0.ItemId).Value).Length > 0 AndAlso q > 0 Then
                xx += Val(G0.Rows(i).Cells(GC0.Value).Value) * q / Val(G0.Rows(i).Cells(GC0.Qty).Value)
            End If
        Next
        'xx = bm.Round(xx)

        'حد المستهلكات
        Dim MyConsumablesValue As Decimal = 0
        For i As Integer = 0 To G3.Rows.Count - 1
            x -= Val(G3.Rows(i).Cells(GC3.ConsumablesValue).Value)
            MyConsumablesValue += Val(G3.Rows(i).Cells(GC3.ConsumablesValue).Value)
        Next
        'ConsumablesValue.Text = bm.Round(MyConsumablesValue)

        If Md.MyProjectType = ProjectType.X Then
            Total0.Text = IIf(x > 0, x, 0)
        Else
            Total0.Text = xx
        End If
        'Total0.Text = bm.Round(Total0.Text)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'إيرادات فرق المستهلكات
        'x = bm.Round(x)
        Total01.Text = 0
        If x < 0 Then
            Total01.Text = -x 'bm.Round(-x)
            x = 0
        End If
        'x = bm.Round(x)

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'مستهلكات الداخلي
        x = 0
        For i As Integer = 0 To G1.Rows.Count - 1
            x += Val(G1.Rows(i).Cells(GC1.Value).Value)
        Next
        'x = bm.Round(x)
        Total1.Text = x
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'خدمات الداخلي
        x = 0
        For i As Integer = 0 To G2.Rows.Count - 1
            x += Val(G2.Rows(i).Cells(GC2.Value).Value)
        Next
        'x = bm.Round(x)
        Total2.Text = x
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        'العمليات
        x = 0
        For i As Integer = 0 To G3.Rows.Count - 1
            x += Val(G3.Rows(i).Cells(GC3.Value).Value)
        Next
        'x = bm.Round(x)
        Total3.Text = x
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'الإقامة
        x = 0
        For i As Integer = 0 To G4.Rows.Count - 1
            x += Val(G4.Rows(i).Cells(GC4.Value).Value)
        Next
        'x = bm.Round(x)
        Total4.Text = x
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Perc.Text = bm.Round(Val(Perc0.Text) / 100 * (Val(Total2.Text) + Val(Total3.Text) + Val(Total4.Text) - Val(MyConsumablesValue)))
        Total.Text = Val(Total0.Text) + Val(Total1.Text) + Val(Total2.Text) + Val(Total3.Text) + Val(Total4.Text) + Val(Perc.Text) - Val(Discount.Text)
        Remaining.Text = Val(Total.Text) - Val(Bal.Text)


        'Round
        'Total1.Text = bm.Round(Total1.Text)
        'Total2.Text = bm.Round(Total2.Text)
        'Total3.Text = bm.Round(Total3.Text)
        'Total4.Text = bm.Round(Total4.Text)
        'Perc.Text = bm.Round(Perc.Text)
        'Total.Text = bm.Round(Total.Text)
        'Total0.Text = bm.Round(Total0.Text)
        'Discount.Text = bm.Round(Discount.Text)
        'Bal.Text = bm.Round(Bal.Text)
        'Total01.Text = bm.Round(Total01.Text)
        'ConsumablesValue.Text = bm.Round(ConsumablesValue.Text)
        'Remaining.Text = bm.Round(Remaining.Text)
        'Total00.Text = bm.Round(Total00.Text)
        'Perc0.Text = bm.Round(Perc0.Text)
    End Sub

    Private Sub G4_CellEndEdit(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            G4.Rows(e.RowIndex).Cells(GC4.Value).Value = Val(G4.Rows(e.RowIndex).Cells(GC4.Living).Value) + Val(G4.Rows(e.RowIndex).Cells(GC4.Supervision).Value) + Val(G4.Rows(e.RowIndex).Cells(GC4.Care).Value)
        Catch ex As Exception
            G4.Rows(e.RowIndex).Cells(GC4.Value).Value = 0
        End Try
    End Sub

    Private Sub CalcBal()
        Dim FD As String = "1900/01/01"
        If Not FromDate.SelectedDate Is Nothing Then FD = bm.ToStrDate(FromDate.SelectedDate)
        Dim TD As String = "1900/01/01"
        If Not ToDate.SelectedDate Is Nothing Then TD = bm.ToStrDate(ToDate.SelectedDate)

        'Bal.Text = Val(bm.ExecuteScalar("select SUM(Value) from BankCash_G where Flag=1 and LinkFile=13 and SubAccNo='" & Val(CaseId.Text) & "' and Daydate between '" & FD & "' and '" & TD & "'"))

        dt = bm.ExecuteAdapter("GetCasePayments", {"CaseId", "FromDate", "ToDate"}, {Val(CaseId.Text), FD, TD})

        Bal.Text = bm.DatatableSumColumn(dt, "Value")

    End Sub

    Private Sub FromDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles FromDate.SelectedDateChanged, ToDate.SelectedDateChanged
        CalcBal()
    End Sub

    Private Sub Discount_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Discount.TextChanged, Perc0.TextChanged
        CalcAll()
    End Sub

    Private Sub RemainingMainAccNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles RemainingMainAccNo.LostFocus
        Try
            bm.FillCombo("select 0 Id,'-' Name union select Id,Name from Debits where AccNo='" & RemainingMainAccNo.SelectedValue & "' order by Id", RemainingSubAccNo)
        Catch
        End Try
    End Sub

    Private Sub btnGetAllCases_Click(sender As Object, e As RoutedEventArgs) Handles btnGetAllCases.Click
        Dim d1 As String = bm.ToStrDate(FromDate.SelectedDate)
        Dim d2 As String = bm.ToStrDate(ToDate.SelectedDate)
        Dim Str As String = "select distinct Id from (select M.ToId Id from SalesDetails D left join SalesMaster M on(M.StoreId=D.StoreId and M.Flag=D.Flag and M.InvoiceNo=D.InvoiceNo) where M.Temp=0 and M.Flag in(37,38,47,48) and M.CaseInvoiceNo=0 and M.DayDate between '" & d1 & "' and '" & d2 & "'" & _
" union all select CaseId from services where Canceled=0 and Flag=2 and CaseInvoiceNo=0 and DayDate between '" & d1 & "' and '" & d2 & "'" & _
" union all select CaseId from OperationMotions where Canceled=0 and CaseInvoiceNo=0 and DayDate between '" & d1 & "' and '" & d2 & "'" & _
" union all select CaseId from ClinicsHistory where CaseInvoiceNo=0 and DayDate between '" & d1 & "' and '" & d2 & "'" & _
" union all select CaseId from Reservations where DayDate between '" & d1 & "' and '" & d2 & "'" & _
" ) Tbl"
        dt = bm.ExecuteAdapter(Str)
        Dim MyCases As String = ""
        For i As Integer = 0 To dt.Rows.Count - 1
            MyCases &= "," & dt.Rows(i)(0).ToString
        Next
        bm.PrintTbl(CType(sender, Button).Content, "Cases", , , "Where Id in(0" & MyCases & ")")
    End Sub

    Private Sub btnPrintWithoutSave_Copy_Click(sender As Object, e As RoutedEventArgs)
        'btnFirst_Click(Nothing, Nothing)
        Dim d As Integer = 0
        While d <> Val(txtID.Text)
            d = Val(txtID.Text)
            btnSave_Click(btnSave, Nothing)
            btnNext_Click(Nothing, Nothing)
        End While
        MessageBox.Show("Done")
    End Sub

    Private Sub G2_CellDoubleClick(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            Dim i As Integer = G2.CurrentRow.Cells(GC2.InvoiceNo).Value
            If i = 0 Then Return
            Dim m As New Services_G With {.Flag = 2}
            Dim w As New MyWindow With {.Content = m}
            m.BasicForm_Loaded(Nothing, Nothing)
            w.Show()
            lv = True
            m.txtID.Text = i
            m.txtID_LostFocus(Nothing, Nothing)
            lv = False
        Catch ex As Exception
        End Try
    End Sub

    Private Sub G3_CellDoubleClick(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            Dim i As Integer = G3.CurrentRow.Cells(GC3.InvoiceNo).Value
            If i = 0 Then Return
            Dim m As New OperationMotions
            Dim w As New MyWindow With {.Content = m}
            m.OperationMotions_Loaded(Nothing, Nothing)
            w.Show()
            lv = True
            m.txtID.Text = i
            m.txtID_LostFocus(Nothing, Nothing)
            lv = False
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As RoutedEventArgs) Handles btnExit.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من خروج المريض؟") Then
            If Val(bm.ExecuteScalar("select dbo.GetCaseStatus(" & Val(CaseId.Text) & ")")) <> 1 Then
                bm.ShowMSG("هذا المريض غير موجود بالمستشفى")
                Return
            End If
            bm.ExecuteNonQuery("SetCaseStatus", {"CaseId", "UserName", "InOut"}, {Val(CaseId.Text), Md.UserName, 2})

            dt = bm.ExecuteAdapter("select RoomId,Id from RoomsData where CaseId=" & Val(CaseId.Text) & " and IsCurrent=1 ")
            If dt.Rows.Count > 0 Then
                bm.ExecuteNonQuery("ExitCase", {"UserName", "RoomId", "Id"}, {Md.UserName, dt.Rows(0)("RoomId"), dt.Rows(0)("Id")})
            End If

            bm.ShowMSG("تم تسجيل خروج المريض بنجاح")
            Return
        End If
    End Sub

    Private Sub BtnPrint3_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint3_Copy.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@CaseId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(CaseId.Text), bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate), CaseName.Text}
        rpt.Rpt = "CasePayments.rpt"
        rpt.Show()
    End Sub
End Class
