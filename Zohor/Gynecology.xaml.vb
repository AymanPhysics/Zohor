Imports System.Data

Public Class Gynecology

    Public TableName As String = "Gynecology"
    Public MainId As String = "CaseId"
    Public SubId As String = "Id"

    Public lblMain_Content As String


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Public WithImage As Boolean = False
    Public ReLoadMenue As Boolean = False

    Public MyCase As Integer
    Public MyCaseName As String

    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {}, {}, {}, {})
        'bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        If WithImage Then
            btnSetImage.Visibility = Visibility.Visible
            btnSetNoImage.Visibility = Visibility.Visible
            Image1.Visibility = Visibility.Visible
        End If

        bm.FillCombo("select Id,Name from Cases union select 0 Id,'-' Name order by Name", CboMain)

        Dim v() As String = {MainId, SubId, "DayDate", "Cervix", "Utras", "Ovaries", "Valva", "Vagina", "Complaint"}
        bm.Fields = v

        Dim c() As Control = {CboMain, txtID, DayDate, Cervix, Uterus, Ovaries, Valva, Vagina, Complaint}
        bm.control = c

        Dim k() As String = {MainId, SubId}
        bm.KeyFields = k

        bm.Table_Name = TableName
        btnNew_Click(sender, e)
        btnComplaint.Visibility = Visibility.Hidden
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        If WithImage Then bm.GetImage(TableName, New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}, "Image", Image1)
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

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CboMain.SelectedValue.ToString = 0 Then
            Return
        End If
        bm.DefineValues()
        If Not bm.Save(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}) Then Return
        If WithImage Then bm.SaveImage(TableName, New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}, "Image", Image1)
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
            bm.SetNoImage(Image1)
            bm.ClearControls()
            DayDate.SelectedDate = Now.Date
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & CboMain.SelectedValue.ToString & "'")
            If txtID.Text = "" Then txtID.Text = "1"

            Cervix.Focus()
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

    Private Sub btnSetImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetImage.Click
        bm.SetImage(Image1)
    End Sub

    Private Sub btnSetNoImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetNoImage.Click
        bm.SetNoImage(Image1, False, True)
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
        Dim rpt As New ReportViewer
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
End Class
