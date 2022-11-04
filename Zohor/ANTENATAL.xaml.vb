Imports System.Data

Public Class ANTENATAL

    Public MyCase As Integer = 0
    Public MyCaseName As String

    Dim bm As New BasicMethods



    Public TableName As String = "ANTENATAL"
    Public MainId As String = "CaseId"
    Public SubId As String = "Id"

    Public lblMain_Content As String


    Dim dt As New DataTable
    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Public WithImage As Boolean = False
    Public ReLoadMenue As Boolean = False
    WithEvents G3 As New MyGrid

    Private Sub ViewHistory_Copy_Click(sender As Object, e As RoutedEventArgs) Handles ViewHistory_Copy.Click
        Try
            EDD.SelectedDate = LMP.SelectedDate.Value.AddMonths(9).AddDays(7)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub InFertility_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        If bm.TestIsLoaded(Me) Then Return
        'bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnPrint})
        bm.TestSecurity(Me, {}, {}, {}, {})

        LoadResource()


        btnComplaint.Visibility = Visibility.Hidden
        'btnDrugsAndDoses.Visibility = Visibility.Hidden
        btnPrint.Visibility = Visibility.Hidden
        GDrugs.Visibility = Visibility.Hidden
        btnSave_Copy.Visibility = Visibility.Hidden

        Dim dt As DataTable = bm.ExecuteAdapter("select * from Cases where Id=" & MyCase)
        If dt.Rows.Count > 0 Then
            Try
                G.Text = dt.Rows(0)("G").ToString
                P.Text = dt.Rows(0)("P").ToString
                A.Text = dt.Rows(0)("A").ToString
                Other.Text = dt.Rows(0)("Other").ToString
                SurgicalHistory.Text = dt.Rows(0)("SurgicalHistory").ToString
                ObstetricalHistory.Text = dt.Rows(0)("ObstetricalHistory").ToString
                MedicallHistory.Text = dt.Rows(0)("MedicallHistory").ToString
            Catch ex As Exception
            End Try

            Try
                LMP.SelectedDate = dt.Rows(0)("LMP").ToString
                EDD.SelectedDate = dt.Rows(0)("EDD").ToString
            Catch ex As Exception
                LMP.SelectedDate = Now.Date
                EDD.SelectedDate = Now.Date
            End Try
        End If


        LoadWFH3()
        LoadDoses()
        LoadDrugs()

        bm.FillCombo("select Id,Name from Cases union select 0 Id,'-' Name order by Name", CboMain)
        CboMain.SelectedValue = MyCase
        Dim v() As String = {MainId, SubId, "PB", "Remarks1", "Remarks2", "NextVisitDate", "DayDate", "Remarks3", "Remarks4"}
        bm.Fields = v

        Dim c() As Control = {CboMain, txtID, PB, Remarks1, Remarks2, NextVisitDate, DayDate, Remarks3, Remarks4}
        bm.control = c

        Dim k() As String = {MainId, SubId}
        bm.KeyFields = k

        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub


    Structure GC3
        Shared Notes1 As String = "Notes1"
        Shared Notes2 As String = "Notes2"
    End Structure

    Private Sub LoadWFH3()
        WFH3.Child = G3

        G3.Columns.Clear()
        G3.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G3.ForeColor = System.Drawing.Color.DarkBlue
        G3.ColumnHeadersVisible = False

        G3.Columns.Add(GC3.Notes1, "")
        G3.Columns.Add(GC3.Notes2, "")
        G3.Columns(GC3.Notes1).FillWeight = 100
        G3.Columns(GC3.Notes2).FillWeight = 200
        G3.Columns(GC3.Notes2).CellTemplate.Style.Alignment = Forms.DataGridViewContentAlignment.MiddleRight

        G3.AllowUserToDeleteRows = True
        G3.EditMode = Forms.DataGridViewEditMode.EditOnEnter
    End Sub


    Sub FillControls()
        bm.FillControls(Me)


        dt = bm.ExecuteAdapter("select * from CasesComplaintDt where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "=0 and " & MainId3 & "='" & TableName & "' and " & SubId & "='" & txtID.Text.Trim & "'")
        G3.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G3.Rows.Add()
            G3.Rows(i).Cells(GC3.Notes1).Value = dt.Rows(i)("Notes1").ToString
            G3.Rows(i).Cells(GC3.Notes2).Value = dt.Rows(i)("Notes2").ToString
        Next
        G3.RefreshEdit()

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub



    Dim IsPrinting As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CboMain.SelectedValue.ToString = 0 Then
            Return
        End If

        bm.ExecuteNonQuery("update Cases set G='" & G.Text.Replace("'", "''") & "', P='" & P.Text.Replace("'", "''") & "', A='" & A.Text.Replace("'", "''") & "', Other='" & Other.Text.Replace("'", "''") & "', LMP='" & bm.ToStrDate(LMP.SelectedDate) & "', EDD='" & bm.ToStrDate(EDD.SelectedDate) & "', SurgicalHistory='" & SurgicalHistory.Text.Replace("'", "''") & "', ObstetricalHistory='" & ObstetricalHistory.Text.Replace("'", "''") & "', MedicallHistory='" & MedicallHistory.Text.Replace("'", "''") & "' where Id=" & MyCase)

        bm.DefineValues()
        If Not bm.Save(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}) Then Return

        bm.SaveGrid(G3, "CasesComplaintDt", New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, 0, TableName, txtID.Text}, New String() {"Notes1", "Notes2"}, New String() {GC3.Notes1, GC3.Notes2}, New VariantType() {VariantType.String, VariantType.String}, New String() {GC3.Notes1})

        If IsPrinting Then Return

        btnNew_Click(sender, e)
        
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {MainId, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        Try
            CboMain.SelectedValue = MyCase
            G3.Rows.Clear()

            bm.ClearControls()
            DayDate.SelectedDate = Now.Date

            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & CboMain.SelectedValue.ToString & "'")
            If txtID.Text = "" Then txtID.Text = "1"

            DayDate.Focus()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "' and " & MainId & " ='" & CboMain.SelectedValue.ToString & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}, dt)
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

    Private Sub CboMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain.SelectionChanged
        ClearControls()
    End Sub

    
    'Private Sub MyBase_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    '    If Not btnSave.Enabled Then Exit Sub
    '    Select Case bm.RequestDelete
    '        Case BasicMethods.CloseState.Yes
    '            
    '            btnSave_Click(Nothing, Nothing)
    '            If Not AllowClose Then e.Cancel = True
    '        Case BasicMethods.CloseState.No

    '        Case BasicMethods.CloseState.Cancel
    '            e.Cancel = True
    '    End Select
    'End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

    End Sub


    Private Sub MainWindow_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.Key = Key.Enter Then
                'e.Handled = True
                If FocusManager.GetFocusedElement(Me.Parent).GetType = GetType(Button) Then Return
                If FocusManager.GetFocusedElement(Me.Parent).GetType = GetType(Forms.Integration.WindowsFormsHost) Then Return
                If FocusManager.GetFocusedElement(Me.Parent).GetType = GetType(TextBox) Then
                    If CType(FocusManager.GetFocusedElement(Me.Parent), TextBox).VerticalScrollBarVisibility = ScrollBarVisibility.Visible Then Return
                End If
                InputManager.Current.ProcessInput(New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) With {.RoutedEvent = Keyboard.KeyDownEvent})
                If FocusManager.GetFocusedElement(Me.Parent).GetType = GetType(TextBox) AndAlso Not CType(FocusManager.GetFocusedElement(Me.Parent), TextBox).VerticalScrollBarVisibility = ScrollBarVisibility.Visible Then CType(FocusManager.GetFocusedElement(Me.Parent), TextBox).SelectAll()
            End If
        Catch
        End Try
    End Sub


    Private Sub btnComplaint_Click(sender As Object, e As RoutedEventArgs) Handles btnComplaint.Click
        Dim frm As New MyWindow With {.Title = "Complaint", .WindowState = WindowState.Maximized}
        Dim c As New Complaint
        c.MyCase = MyCase
        c.MyFlag = TableName
        c.MyKey = Val(txtID.Text)
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnInvestigations_Click(sender As Object, e As RoutedEventArgs) Handles btnInvestigations.Click
        'Dim frm As New MyWindow With {.Title = "Investigations", .WindowState = WindowState.Maximized}
        'Dim c As New Complaint With {.TableName = "Investigations"}
        'c.MyCase = MyCase
        'c.MyFlag = TableName
        'c.MyKey = Val(txtID.Text)
        'frm.Content = c
        'frm.ShowDialog()
        Dim rpt As New ReportViewer With {.TopMost = True}
        rpt.paraname = New String() {"@CaseId"}
        rpt.Header = Md.MyProjectType.ToString
        rpt.paravalue = New String() {MyCase}
        rpt.Rpt = "LabTests2.rpt"
        rpt.Show()
    End Sub

    Private Sub btnUltraSound_Click(sender As Object, e As RoutedEventArgs) Handles btnUltraSound.Click
        Dim frm As New MyWindow With {.Title = "Ultra Sound", .WindowState = WindowState.Maximized}
        Dim c As New MyImages
        c.v1 = MyCase
        c.v2 = TableName
        c.v3 = Val(txtID.Text)
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnDrugsAndDoses_Click(sender As Object, e As RoutedEventArgs) Handles btnDrugsAndDoses.Click
        Dim frm As New MyWindow With {.Title = "Drugs And Doses", .WindowState = WindowState.Maximized}
        Dim c As New DrugsAndDoses
        c.EmpId = Md.UserName
        c.DatePicker1 = DayDate.SelectedDate
        c.ReservId = Val(txtID.Text)
        c.CaseId = MyCase
        c.CaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnNeededLabTests_Click(sender As Object, e As RoutedEventArgs) Handles btnNeededLabTests.Click
        Dim frm As New MyWindow With {.Title = "Investigations Request", .WindowState = WindowState.Maximized}
        Dim c As New NeededLabTests
        c.EmpId = Md.UserName
        c.DatePicker1 = DayDate.SelectedDate
        c.ReservId = Val(txtID.Text)
        c.CaseId = MyCase
        c.CaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnInvestigationsEntry_Click(sender As Object, e As RoutedEventArgs) Handles btnInvestigationsEntry.Click
        Dim frm As New MyWindow With {.Title = "Investigations Entry", .WindowState = WindowState.Maximized}
        Dim c As New LabTests
        c.MyCase = MyCase
        frm.Content = c
        frm.ShowDialog()
    End Sub






    Public MainId2 As String = "Mykey"
    Public MainId3 As String = "MyFlag"

    Private Sub Add3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add3.Click
        If cbo31.Text.Trim = "" Then Return
        G3.Rows.Add(cbo31.Text, cbo32.Text)
        bm.AddItemToTable("Drugs", cbo31.Text)
        bm.AddItemToTable("Doses", cbo32.Text)
        cbo31.Text = ""
        cbo32.Text = ""
        'LoadDrugs()
        'LoadDoses()
        cbo31.Focus()
    End Sub

    Private Sub cbo31_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles cbo31.PreviewKeyDown
        If e.Key = Key.Enter Then cbo32.Focus()
    End Sub

    Private Sub cbo32_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles cbo32.PreviewKeyDown
        If e.Key = Key.Enter Then Add3_Click(Nothing, Nothing)
    End Sub


    Private Sub LoadDoses()
        bm.FillCombo("Doses", cbo32, "", "")
    End Sub

    Private Sub LoadDrugs()
        bm.FillCombo("Drugs", cbo31, "", "")
    End Sub

    Private Sub btnSave_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy.Click

        dt = bm.ExecuteAdapter("select * from CasesComplaintDt where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "=0 and " & MainId3 & "='" & TableName & "' and " & SubId & "=(select max(" & SubId & ") from CasesComplaintDt where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "=0 and " & MainId3 & "='" & TableName & "' and " & SubId & "<" & Val(txtID.Text.Trim) & ")")
        G3.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G3.Rows.Add()
            G3.Rows(i).Cells(GC3.Notes1).Value = dt.Rows(i)("Notes1").ToString
            G3.Rows(i).Cells(GC3.Notes2).Value = dt.Rows(i)("Notes2").ToString
        Next
        G3.RefreshEdit()

    End Sub


    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        IsPrinting = True
        btnSave_Click(Nothing, Nothing)
        IsPrinting = False

        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Window).Title
        rpt.paraname = New String() {"@CaseId", "@Mykey", "@MyFlag", "@Id", "CaseName", "NextVisitDate"}
        rpt.paravalue = New String() {CboMain.SelectedValue.ToString, 0, TableName, txtID.Text.Trim, MyCaseName, bm.ToStrDate(NextVisitDate.SelectedDate)}
        rpt.Rpt = "cbo4.rpt"
        'rpt.Show()
        rpt.Print()
    End Sub

    Private Sub LMP_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles LMP.SelectedDateChanged
        Dim i As Integer = CType(Now.Date - LMP.SelectedDate, TimeSpan).Days
        Weeks.Text = (i - (i Mod 7)) / 7
        Days.Text = i Mod 7
    End Sub
End Class
