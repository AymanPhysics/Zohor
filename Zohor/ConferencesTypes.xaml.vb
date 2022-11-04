Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine

Public Class ConferencesTypes
    Public TableName As String = "Conferences"
    Public SubId As String = "Id"
    Public SubName As String = "Name"



    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow

    Public Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        If bm.TestIsLoaded(Me) Then Return

        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnPrintAll})

        LoadResource()


        bm.Fields = New String() {SubId, SubName, "IsActive", "IsFree", "CertificateLeft", "CertificateTop", "CertificateLeft2", "CertificateTop2", "IDLeft", "IDTop", "IsCalc", "AttendanceHours"}
        bm.control = New Control() {txtID, txtName, CheckBox1, IsFree, CertificateLeft, CertificateTop, CertificateLeft2, CertificateTop2, IDLeft, IDTop, IsCalc, AttendanceHours}
        bm.KeyFields = New String() {SubId}
        CheckBox1.Content = "IsActive"

        bm.Table_Name = TableName
        btnNew_Click(sender, e)

    End Sub

    Sub FillControls()
        bm.FillControls(Me)

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CType(Application.Current.MainWindow, MainWindow).TabControl1.Items.Remove(Me.Parent)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
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
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        btnNew_Click(sender, e)

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
        bm.SetNoImage(Image1)
        bm.ClearControls(False)
        CheckBox2.IsChecked = True
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        txtName.Focus()
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

    Private Sub txtID_KeyUp(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyUp
        bm.ShowHelp(CType(Parent, Page).Title, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName, TableName)
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, CertificateTop.KeyDown, CertificateLeft.KeyDown, CertificateTop2.KeyDown, CertificateLeft2.KeyDown, AttendanceHours.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

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
        btnPrintAll.SetResourceReference(ContentProperty, "Print All")

        lblId.SetResourceReference(ContentProperty, "Id")
        lblName.SetResourceReference(ContentProperty, "Name")
    End Sub

    Private Sub btnPrintAll_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintAll.Click
        bm.PrintTbl(CType(Parent, Page).Title, TableName)
    End Sub

    Private Sub BtnPrintAll_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintAll_Copy.Click
        Dim rpt As New ReportViewer
        rpt.Rpt = "Certificate.rpt"
        rpt.paraname = New String() {"CustomerId", "CustomerName", "AttendanceHours"}
        rpt.paravalue = New String() {0, "Client Name Will Appear Here", "10"}
        rpt.ReportViewer_Load(Nothing, Nothing)
        For Each c As ReportObject In rpt.ReportDoc.ReportDefinition.ReportObjects
            Try
                If c.Name = "AttendanceHours" Then
                    c.Top = Val(CertificateTop2.Text) * 100
                    c.Left = Val(CertificateLeft2.Text) * 100
                Else
                    c.Top = Val(CertificateTop.Text) * 100
                    c.Left = Val(CertificateLeft.Text) * 100
                End If
            Catch
            End Try
        Next

        rpt.Show()
    End Sub

    Private Sub BtnPrintAll_Copy1_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintAll_Copy1.Click
        Dim rpt As New ReportViewer
        rpt.Rpt = "ID.rpt"
        rpt.paraname = New String() {"CustomerId", "CustomerName"}
        rpt.paravalue = New String() {"1000000100013", "Client Name Will Appear Here"}
        rpt.ReportViewer_Load(Nothing, Nothing)
        For Each c As ReportObject In rpt.ReportDoc.ReportDefinition.ReportObjects
            Try
                If c.Name = "CustomerName" Then
                    c.Top = Val(IDTop.Text) * 100
                End If
                c.Left = Val(IDLeft.Text) * 100
            Catch
            End Try
        Next

        rpt.Show()

    End Sub

    Private Sub IsCalc_Checked(sender As Object, e As RoutedEventArgs) Handles IsCalc.Checked, IsCalc.Unchecked
        If IsCalc.IsChecked Then
            AttendanceHours.Clear()
            AttendanceHours.IsEnabled = False
        Else
            AttendanceHours.IsEnabled = True
        End If
    End Sub
End Class
