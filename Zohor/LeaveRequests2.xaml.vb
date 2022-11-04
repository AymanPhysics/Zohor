Imports System.Data

Public Class LeaveRequests2
    Public TableName As String = ""
    Public SubId As String = "Id"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub DirectBonusCut_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()

        bm.Fields = New String() {SubId, "EmpId", "DayDate", "LeaveType", "HalfDay", "FromDate", "ToDate", "Reason"}
        bm.control = New Control() {txtID, EmpId, DayDate, LeaveType, HalfDay, FromDate, ToDate, Reason}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim lop As Boolean = False
    Sub FillControls()
        bm.FillControls(Me)
        lop = True
        bm.FillControls(Me)
        EmpId_LostFocus(Nothing, Nothing)
        lop = False
        UndoNewId()
        DayDate.Focus()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim lop2 As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Val(EmpId.Text) = 0 Then Return
        If FromDate.SelectedDate Is Nothing Then Return
        If ToDate.SelectedDate Is Nothing Then Return
        If LeaveType.SelectedIndex < 1 Then Return

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If txtID.Text.Trim = "" Then
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If txtID.Text = "" Then txtID.Text = "1"
            LastEntry.Text = txtID.Text
            State = BasicMethods.SaveState.Insert
        End If

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                txtID.Text = ""
                LastEntry.Text = ""
            End If
            Return
        End If

        btnNew_Click(sender, e)
    End Sub

    Sub NewId()
        txtID.Clear()
        'txtID.IsEnabled = False
    End Sub

    Sub UndoNewId()
        txtID.IsEnabled = True
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
        DayDate.SelectedDate = bm.MyGetDate()
        EmpId_LostFocus(Nothing, Nothing)
        NewId()
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub EmpId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId.KeyUp
        If bm.ShowHelp("Employees", EmpId, EmpName, e, "Select cast(Id as varchar(10))Id," & Resources.Item("CboName") & " Name from Employees") Then
            EmpId_LostFocus(sender, Nothing)
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

        lblId.SetResourceReference(ContentProperty, "Id")
        lblEmpId.SetResourceReference(ContentProperty, "Employee")
        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblLastEntry.SetResourceReference(ContentProperty, "LastEntry")
        lblNotes.SetResourceReference(ContentProperty, "Notes")
        lblLeaveType.SetResourceReference(ContentProperty, "LeaveType")
        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")
        HalfDay.SetResourceReference(CheckBox.ContentProperty, "HalfDay")


        lblAnnual.SetResourceReference(ContentProperty, "Annual")
        lblDayOff.SetResourceReference(ContentProperty, "DayOff")

        lblBal.SetResourceReference(ContentProperty, "Bal")
        lblUsed.SetResourceReference(ContentProperty, "Used")
        lblRemaining.SetResourceReference(ContentProperty, "Remaining")

        bm.ResetComboboxContent(LeaveType)
    End Sub



    Private Sub EmpId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId.LostFocus
        If Val(EmpId.Text.Trim) = 0 Then
            EmpId.Clear()
            EmpName.Clear()
            AnnualBal.Clear()
            AnnualRemaining.Clear()
            AnnualUsed.Clear()
            DayOffBal.Clear()
            DayOffRemainning.Clear()
            DayOffUsed.Clear()
            Return
        End If
        bm.LostFocus(EmpId, EmpName, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId.Text.Trim())


        Dim dt As DataTable = bm.ExecuteAdapter("select Annual,NoofDaysOff,dbo.GetEmpLeaveBal2(Id,1) AnnualUsed,dbo.GetEmpLeaveBal2(Id,2) DayOffUsed from Employees where Id=" & EmpId.Text.Trim())
        If dt.Rows.Count > 0 Then
            AnnualBal.Text = Val(dt.Rows(0)("Annual"))
            DayOffBal.Text = Val(dt.Rows(0)("NoofDaysOff"))

            AnnualUsed.Text = Val(dt.Rows(0)("AnnualUsed"))
            DayOffUsed.Text = Val(dt.Rows(0)("DayOffUsed"))

            AnnualRemaining.Text = Val(AnnualBal.Text) - Val(AnnualUsed.Text)
            DayOffRemainning.Text = Val(DayOffBal.Text) - Val(DayOffUsed.Text)
        End If
    End Sub

    Private Sub HalfDay_Checked(sender As Object, e As RoutedEventArgs) Handles HalfDay.Checked
        ToDate.IsEnabled = False
        ToDate.SelectedDate = FromDate.SelectedDate
    End Sub

    Private Sub HalfDay_Unchecked(sender As Object, e As RoutedEventArgs) Handles HalfDay.Unchecked
        ToDate.IsEnabled = True
    End Sub

    Private Sub FromDate_LostFocus(sender As Object, e As RoutedEventArgs) Handles FromDate.LostFocus, ToDate.LostFocus
        If ToDate.SelectedDate Is Nothing OrElse ToDate.SelectedDate < FromDate.SelectedDate Then
            ToDate.SelectedDate = FromDate.SelectedDate
        End If
    End Sub
End Class
