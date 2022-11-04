Imports System.Data

Public Class InvestorsProfits
    Public TableName As String = "InvestorsProfits"
    Public SubId As String = "InvoiceNo"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        bm.Fields = New String() {SubId, "DayDate", "Perc1", "Perc2", "Val1", "Val2", "Total1", "Total2", "IsRound"}
        bm.control = New Control() {txtID, DayDate, Perc1, Perc2, Val1, Val2, Total1, Total2, IsRound}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName

        LoadWFH()
        btnNew_Click(sender, e)
        DayDate.Focus()
    End Sub



    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Bal As String = "Bal"
        Shared Perc As String = "Perc"
        Shared Value As String = "Value"
        Shared Ded As String = "Ded"
        Shared Ded2 As String = "Ded2"
        Shared Net As String = "Net"
        Shared Notes As String = "Notes"
        Shared Line As String = "Line"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Id, "الكود")
        G.Columns.Add(GC.Name, "الاسم")
        G.Columns.Add(GC.Bal, "الرصيد")
        G.Columns.Add(GC.Perc, "النسبة %")
        G.Columns.Add(GC.Value, "القيمة")
        G.Columns.Add(GC.Ded, "أقساط")
        G.Columns.Add(GC.Ded2, "منصرف من الأرباح الشهرية خلال العام")
        G.Columns.Add(GC.Net, "القيمة المستحقة")
        G.Columns.Add(GC.Notes, "ملاحظات")
        G.Columns.Add(GC.Line, "Line")
         
        G.Columns(GC.Name).FillWeight = 200
        G.Columns(GC.Notes).FillWeight = 300

        G.Columns(GC.Id).ReadOnly = True
        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Bal).ReadOnly = True
        G.Columns(GC.Perc).ReadOnly = True
        G.Columns(GC.Value).ReadOnly = True
        G.Columns(GC.Ded).ReadOnly = True
        G.Columns(GC.Ded2).ReadOnly = True
        G.Columns(GC.Net).ReadOnly = True


        G.Columns(GC.Line).Visible = False

        G.AllowUserToAddRows = False

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True


        bm.FillControls(Me) 

        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & TableName & " where " & SubId & "=" & txtID.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("Id").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("Name").ToString
            G.Rows(i).Cells(GC.Bal).Value = dt.Rows(i)("Bal").ToString
            G.Rows(i).Cells(GC.Perc).Value = dt.Rows(i)("Perc").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Rows(i).Cells(GC.Ded).Value = dt.Rows(i)("Ded").ToString
            G.Rows(i).Cells(GC.Ded2).Value = dt.Rows(i)("Ded2").ToString
            G.Rows(i).Cells(GC.Net).Value = dt.Rows(i)("Net").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
        Next
        DayDate.Focus()
        G.RefreshEdit()
        lop = False
        CalcTotal()
         
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False
     
        G.EndEdit()
         

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If


        bm.DefineValues()

        If Not bm.SaveGrid(G, TableName, New String() {SubId}, New String() {txtID.Text}, New String() {"Id", "Name", "Bal", "Perc", "Value", "Ded", "Ded2", "Net", "Notes"}, New String() {GC.Id, GC.Name, GC.Bal, GC.Perc, GC.Value, GC.Ded, GC.Ded2, GC.Net, GC.Notes}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.String}, New String() {}) Then Return

        If Not bm.Save(New String() {SubId}, New String() {txtID.Text}) Then Return

        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True
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

        DayDate.SelectedDate = bm.MyGetDate()
        DayDate2.SelectedDate = bm.MyGetDate()
        G.Rows.Clear()
        CalcTotal()

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow

        Perc1.IsChecked = True

        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"
        'DayDate.Focus()
        txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If sender Is Nothing OrElse bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "=" & txtID.Text)
            If Not sender Is Nothing Then
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

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Val1.KeyDown, Val2.KeyDown
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

        lblID.SetResourceReference(ContentProperty, "Id")

        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
    End Sub

    Dim LopCalc As Boolean = False
    Private Sub CalcTotal()
        If LopCalc Or lop Then Return
        Try
            LopCalc = True
            Total1.Text = Math.Round(0, 2)
            Total2.Text = Math.Round(0, 2)
            For i As Integer = 0 To G.Rows.Count - 1
                Total1.Text += Val(G.Rows(i).Cells(GC.Bal).Value)
                Total2.Text += Val(G.Rows(i).Cells(GC.Value).Value)
            Next

            LopCalc = False
        Catch ex As Exception
        End Try
    End Sub


    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) 'Handles btnPrint.Click, btnPrint2.Click, btnPrint3.Click
        DontClear = True
        If btnSave.IsEnabled AndAlso btnSave.Visibility = Visibility.Visible Then
            btnSave_Click(sender, e)
        Else
            AllowSave = True
        End If
        DontClear = False
        If Not AllowSave Then Return

        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@InvoiceNo", "Header"}
        rpt.paravalue = New String() {CType(Parent, Page).Title}
        rpt.Rpt = ".rpt"
        rpt.Show()
    End Sub


    Private Sub Perc1_Checked(sender As Object, e As RoutedEventArgs) Handles Perc1.Checked
        Perc2.IsChecked = False
    End Sub

    Private Sub Perc1_UnChecked(sender As Object, e As RoutedEventArgs) Handles Perc1.Unchecked
        Perc2.IsChecked = True
    End Sub

    Private Sub Perc2_Checked(sender As Object, e As RoutedEventArgs) Handles Perc2.Checked
        Perc1.IsChecked = False
    End Sub

    Private Sub Perc2_UnChecked(sender As Object, e As RoutedEventArgs) Handles Perc2.Unchecked
        Perc1.IsChecked = True
    End Sub

    Private Sub btncalc_Click(sender As Object, e As RoutedEventArgs) Handles btncalc.Click
        If G.Rows.Count = 0 OrElse bm.ShowDeleteMSG("سيتم مسح القيد وإعادة الاحتساب مرة أخري .. استمرار؟") Then
            btnDelete_Click(Nothing, Nothing)
            dt = bm.ExecuteAdapter("LoadInvestorsBal0", {"Month", "Year"}, {DayDate2.SelectedDate.Value.Month, DayDate2.SelectedDate.Value.Year})
            G.Rows.Clear()
            For i As Integer = 0 To dt.Rows.Count - 1
                G.Rows.Add()
                G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("Id").ToString
                G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("Name").ToString
                G.Rows(i).Cells(GC.Bal).Value = dt.Rows(i)("Bal0").ToString
                G.Rows(i).Cells(GC.Perc).Value = 0
                G.Rows(i).Cells(GC.Value).Value = 0
                G.Rows(i).Cells(GC.Ded).Value = dt.Rows(i)("Ded").ToString
                If Perc1.IsChecked Then
                    G.Rows(i).Cells(GC.Ded2).Value = 0
                Else
                    G.Rows(i).Cells(GC.Ded2).Value = dt.Rows(i)("Ded2").ToString
                End If
                G.Rows(i).Cells(GC.Net).Value = 0
                G.Rows(i).Cells(GC.Notes).Value = ""
                G.Rows(i).Cells(GC.Line).Value = 0
            Next

            CalcTotal()

            For i As Integer = 0 To G.Rows.Count - 1
                If Perc1.IsChecked Then
                    G.Rows(i).Cells(GC.Perc).Value = Val1.Text
                    G.Rows(i).Cells(GC.Value).Value = bm.Rnd(Val(G.Rows(i).Cells(GC.Perc).Value) / 100 * G.Rows(i).Cells(GC.Bal).Value)
                Else
                    G.Rows(i).Cells(GC.Perc).Value = 100 * Val(G.Rows(i).Cells(GC.Bal).Value) / Val(Total1.Text)
                    G.Rows(i).Cells(GC.Value).Value = bm.Rnd(Val(G.Rows(i).Cells(GC.Perc).Value) / 100 * Val(Val2.Text))
                    G.Rows(i).Cells(GC.Perc).Value = bm.Rnd(G.Rows(i).Cells(GC.Perc).Value)
                End If

                If IsRound.IsChecked Then
                    G.Rows(i).Cells(GC.Value).Value = bm.Rnd5LE(G.Rows(i).Cells(GC.Value).Value)
                End If

                G.Rows(i).Cells(GC.Net).Value = Val(G.Rows(i).Cells(GC.Value).Value) - Val(G.Rows(i).Cells(GC.Ded).Value) - Val(G.Rows(i).Cells(GC.Ded2).Value)
            Next

            CalcTotal()

        End If
    End Sub
End Class
