Imports System.Data

Public Class KidneysWashMotion
    Public TableName As String = "KidneysWashMotion"
    Public TableNameDetails As String = "KidneysWashMotionDetails"
    Public SubId As String = "InvoiceNo"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Public Flag As Integer = 0
    Public MyLinkFile As Integer = 0
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        bm.FillCombo("CasePaymentTypes", CasePaymentTypeId, "", , True)

        LoadResource()
        bm.Fields = New String() {SubId, "DayDate", "Notes", "Canceled", "CaseId", "CasePaymentTypeId", "Value", "MM", "YY"}
        bm.control = New Control() {txtID, DayDate, Notes, Canceled, CaseId, CasePaymentTypeId, Value, MM, YY}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName

        LoadWFH()
        btnNew_Click(sender, e)
        txtID.Focus()
    End Sub



    Structure GC
        Shared ItemId As String = "ItemId"
        Shared ItemName As String = "ItemName"
        Shared Qty As String = "Qty"
        Shared Line As String = "Line"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.ItemId, "كود الصنف")
        G.Columns.Add(GC.ItemName, "اسم الصنف")
        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.Line, "Line")

        G.Columns(GC.ItemName).FillWeight = 300

        G.Columns(GC.ItemId).ReadOnly = True
        G.Columns(GC.ItemName).ReadOnly = True
        G.Columns(GC.Line).ReadOnly = True

        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False


    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True
        bm.FillControls(Me)
        CaseID_LostFocus(Nothing, Nothing)
        bm.FillControls(Me)

        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & TableNameDetails & " where " & SubId & "=" & txtID.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.ItemId).Value = dt.Rows(i)("ItemId").ToString
            G.Rows(i).Cells(GC.ItemName).Value = dt.Rows(i)("ItemName").ToString
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
        Next
        DayDate.Focus()
        G.RefreshEdit()
        lop = False
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Val(CaseId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblCaseId.Content)
            CaseId.Focus()
            Return
        End If

        G.EndEdit()

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If

        Value.Text = Val(Value.Text)
        MM.Text = Val(MM.Text)
        YY.Text = Val(YY.Text)
        bm.DefineValues()

        If Not bm.Save(New String() {SubId}, New String() {txtID.Text}) Then Return
        If Not bm.SaveGrid(G, TableNameDetails, New String() {SubId}, New String() {txtID.Text}, New String() {"ItemId", "ItemName", "Qty"}, New String() {GC.ItemId, GC.ItemName, GC.Qty}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.Decimal}, New String() {GC.ItemId}, "Line", "Line") Then Return

        btnNew_Click(sender, e)
    End Sub

    Dim lop As Boolean = False

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
        If lop OrElse lv Then Return
        lop = True
        bm.ClearControls()

        DayDate.SelectedDate = bm.MyGetDate()
        MM_LostFocus(Nothing, Nothing)
        YY_LostFocus(Nothing, Nothing)

        G.Rows.Clear()
        dt = bm.ExecuteAdapter("select Id,Name from Items where IsKidneysWash=1")
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.ItemId).Value = dt.Rows(i)("Id").ToString
            G.Rows(i).Cells(GC.ItemName).Value = dt.Rows(i)("Name").ToString
        Next

        CaseName.Text = ""

        Value.Clear()

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtFlag.Text = Flag
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"
        'DayDate.Focus()
        txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "=" & txtID.Text & " delete from " & TableNameDetails & " where " & SubId & "=" & txtID.Text)
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
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub Canceled_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Checked
        Value.Text = ""
        Value.IsEnabled = False
    End Sub

    Private Sub Canceled_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Unchecked
        Value.IsEnabled = True
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblID.SetResourceReference(ContentProperty, "Id")

        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblNotes.SetResourceReference(ContentProperty, "Notes")

        lblCaseId.SetResourceReference(ContentProperty, "CaseId")
    End Sub


    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp
        If bm.ShowHelp("Cases", CaseId, CaseName, e, "select cast(Id as varchar(100)) Id,Name from Cases2") Then
            CaseID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select Name from Cases2 where Id=" & CaseId.Text.Trim())
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile,CasePaymentTypeId,Value from Cases2 where Id=" & CaseId.Text.Trim())
        If dt.Rows.Count > 0 Then
            CasePaymentTypeId.SelectedValue = dt.Rows(0)("CasePaymentTypeId")
            Value.Text = dt.Rows(0)("Value").ToString

            CaseId.ToolTip = Resources.Item("Id") & ": " & CaseId.Text & vbCrLf & Resources.Item("Name") & ": " & CaseName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
            CaseName.ToolTip = CaseId.ToolTip
        End If
    End Sub

    Private Sub MM_LostFocus(sender As Object, e As RoutedEventArgs) Handles MM.LostFocus
        Try
            MM.Text = Val(MM.Text)
            If Val(MM.Text) = 0 OrElse Val(MM.Text) > 12 Then
                MM.Text = DayDate.SelectedDate.Value.Month
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub YY_LostFocus(sender As Object, e As RoutedEventArgs) Handles YY.LostFocus
        Try
            YY.Text = Val(YY.Text)
            If Val(YY.Text) = 0 Then
                YY.Text = DayDate.SelectedDate.Value.Year
            End If
        Catch ex As Exception
        End Try
    End Sub

End Class
