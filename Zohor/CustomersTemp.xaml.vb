Imports System.Data

Public Class CustomersTemp
    Public TableName As String = "CustomersTemp"
    Public SubId As String = "Id"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()

        bm.Fields = {SubId, "P1", "P2", "P3", "P4", "P5", "P6", "ItemName", "Total", "DayDate", "DownPayment", "InstallCount", "InstallValue", "Notes", "CustId"}
        bm.control = {txtID, P1, P2, P3, P4, P5, P6, ItemName, Total, DayDate, DownPayment, InstallCount, InstallValue, Notes, CustId}
        bm.KeyFields = {SubId}

        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CType(Application.Current.MainWindow, MainWindow).TabControl1.Items.Remove(Me.Parent)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        bm.FillControls(Me)
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        bm.FillControls(Me)
    End Sub

    Dim AddCust As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If P1.Text.Trim = "" Then
            P1.Focus()
            Return
        End If
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If Not AddCust OrElse DontClear Then btnNew_Click(sender, e)

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        bm.FillControls(Me)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        P1.Focus()
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
        bm.FillControls(Me)
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_KeyUp(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp(CType(Parent, Page).Title, txtID, P1, e, "select cast(Id as varchar(100)) Id,P1 Name from " & TableName) Then
            P1.Focus()
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
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            P1.Focus()
            lv = False
            Return
        End If
        bm.FillControls(Me)
        lv = False
        P1.SelectAll()
        P1.Focus()
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


        lblId.SetResourceReference(ContentProperty, "Id") 
    End Sub

    Dim DontClear As Boolean = False
    Private Sub Button2_Click(sender As Object, e As RoutedEventArgs) Handles Button2.Click
        DontClear = True
        btnSave_Click(sender, e)
        DontClear = False


        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"Header", "P0", "P1", "P2", "P3", "P4", "P5", "P6", "ItemName", "Total", "DayDate", "DownPayment", "InstallVal", "InstallCount"}
        rpt.paravalue = New String() {"طلب حصول على نظام تقسيط من شركة " & Md.CompanyName, bm.ExecuteScalar("select Name from Stores where Id=" & Md.DefaultStore), P1.Text, P2.Text, P3.Text, P4.Text, P5.Text, P6.Text, ItemName.Text, Total.Text, bm.ToStrDate(DayDate.SelectedDate), DownPayment.Text, InstallValue.Text, InstallCount.Text}
        rpt.Rpt = "SalesTemp.rpt"
        rpt.Show()
    End Sub

    Private Sub btnAddCustomer_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCustomer.Click
        Dim frm As New MyWindow With {.Title = "Customers", .WindowState = WindowState.Maximized}
        bm.SetMySecurityType(frm, 816)
        frm.Content = New Customers With {.MyId = Val(CustId.Text)}
        frm.Show()
    End Sub

    Private Sub Button2_Copy_Click(sender As Object, e As RoutedEventArgs) Handles Button2_Copy.Click
        If P1.Text.Trim = "" Then Return
        If Val(CustId.Text.Trim) > 0 Then
            btnAddCustomer_Click(Nothing, Nothing)
            Return
        End If

        CustId.Clear()

        If Not bm.AddItemToTable("Customers", {"Name", "Tel", "Mobile", "Address", "AccNo"}, {P1.Text, "", P2.Text, P3.Text, bm.ExecuteScalar("select min(Id) from Chart where LinkFile=1")}) Then Return

        CustId.Text = bm.ExecuteScalar("select max(Id) from Customers")

        AddCust = True
        btnSave_Click(Nothing, Nothing)
        AddCust = False

        btnAddCustomer_Click(Nothing, Nothing)
    End Sub
End Class
