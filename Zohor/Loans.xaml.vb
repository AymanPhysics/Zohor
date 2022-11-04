Imports System.Data

Public Class Loans
    Public TableName As String = "Loans"
    Public SubId As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub DirectBonusCut_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()

        bm.Fields = New String() {SubId, "EmpId", "Value", "DayDate", "DedStartMonth", "DedStartYear", "Notes", "DedCount", "SaveId"}
        bm.control = New Control() {txtID, EmpId, Value, DayDate, DedStartMonth, DedStartYear, Notes, DedCount, SaveId}
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
        SaveId_LostFocus(Nothing, Nothing)
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
        If Val(EmpId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblEmpId.Content)
            Return
        End If
        If Val(DedStartMonth.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblMonth.Content)
            Return
        End If
        If Val(DedStartYear.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblYear.Content)
            Return
        End If
        If Val(DedCount.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblDedCount.Content)
            Return
        End If
        If Val(Value.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblValue.Content)
            Return
        End If
        If Val(SaveId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblSaveId.Content)
            Return
        End If


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
        DedStartMonth.Text = DayDate.SelectedDate.Value.Month
        DedStartYear.Text = DayDate.SelectedDate.Value.Year
        EmpId_LostFocus(Nothing, Nothing)
        SaveId_LostFocus(Nothing, Nothing)
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, EmpId.KeyDown, SaveId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId.KeyDown, Value.KeyDown, DedStartMonth.KeyDown, DedStartYear.KeyDown, DedCount.KeyDown
        bm.MyKeyPress(sender, e, False)
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
        lblMonth.SetResourceReference(ContentProperty, "Month")
        lblYear.SetResourceReference(ContentProperty, "Year")

        lblDedCount.SetResourceReference(ContentProperty, "DedCount")
        lblDedStart.SetResourceReference(ContentProperty, "DedStart")

    End Sub



    Private Sub EmpId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId.LostFocus
        If Val(EmpId.Text.Trim) = 0 Then
            EmpId.Clear()
            EmpName.Clear()
            Return
        End If
        bm.LostFocus(EmpId, EmpName, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId.Text.Trim())
    End Sub


    Private Sub SaveId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.LostFocus
        If Val(SaveId.Text.Trim) = 0 Then
            SaveId.Clear()
            SaveName.Clear()
            Return
        End If
        bm.LostFocus(SaveId, SaveName, "select Name from Saves where Id=" & SaveId.Text.Trim())
    End Sub
    Private Sub SaveId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SaveId.KeyUp
        If bm.ShowHelp("Saves", SaveId, SaveName, e, "select cast(Id as varchar(100)) Id,Name from Saves") Then
            SaveId_LostFocus(Nothing, Nothing)
        End If
    End Sub

End Class
