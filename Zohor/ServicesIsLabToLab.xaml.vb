Imports System.Data

Public Class ServicesIsLabToLab

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Dim dv As New DataView



    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)



        dt = bm.ExecuteAdapter("getServicesIsLabToLab", {}, {})

        If dt.Rows.Count = 0 Then
            Return
        End If
        dt.TableName = "tbl"

        dv.Table = dt
        DataGridView1.ItemsSource = dv

        'DataGridView1.IsReadOnly = True
        DataGridView1.RowHeaderWidth = 0
        DataGridView1.SelectionMode = DataGridSelectionMode.Single
        DataGridView1.SelectionUnit = DataGridSelectionUnit.Cell


        DataGridView1.MinColumnWidth = 0
        DataGridView1.Columns(dt.Columns("MyLine").Ordinal).Visibility = Visibility.Hidden
        DataGridView1.Columns(dt.Columns("ServiceGroupId").Ordinal).Visibility = Visibility.Hidden
        DataGridView1.Columns(dt.Columns("ServiceGroupName").Ordinal).Visibility = Visibility.Hidden
        DataGridView1.Columns(dt.Columns("ServiceTypeId").Ordinal).Visibility = Visibility.Hidden
        DataGridView1.Columns(dt.Columns("InvoiceNo").Ordinal).Header = "المسلسل"
        DataGridView1.Columns(dt.Columns("DayDate").Ordinal).Header = "التاريخ"
        DataGridView1.Columns(dt.Columns("Value").Ordinal).Header = "المبلغ"
        DataGridView1.Columns(dt.Columns("Cost").Ordinal).Header = "التكلفة"
        DataGridView1.Columns(dt.Columns("ServiceTypeName").Ordinal).Header = "التحليل"
        DataGridView1.Columns(dt.Columns("CaseId").Ordinal).Header = "الكود المريض"
        DataGridView1.Columns(dt.Columns("CaseName").Ordinal).Header = "اسم المريض"
        DataGridView1.Columns(dt.Columns("Notes").Ordinal).Header = "ملاحظات"
        DataGridView1.Columns(dt.Columns("IsSelected").Ordinal).Header = "اختيار"

        For i As Integer = 0 To DataGridView1.Columns.Count - 1
            If DataGridView1.Columns(i).Header = DataGridView1.Columns(dt.Columns("Cost").Ordinal).Header Then
            ElseIf DataGridView1.Columns(i).Header = DataGridView1.Columns(dt.Columns("IsSelected").Ordinal).Header Then
            Else
                DataGridView1.Columns(i).IsReadOnly = True
            End If
        Next
    End Sub

    Private Sub P_Checked(sender As Object, e As RoutedEventArgs) Handles P.Checked, P.Unchecked
        For i As Integer = 0 To DataGridView1.Items.Count - 1
            DataGridView1.Items(i)(dt.Columns("IsSelected").Ordinal) = P.IsChecked
        Next

        CalcTotal
    End Sub

    Sub CalcTotal()
        DataGridView1.CommitEdit()
        Value.Text = "0"
        Cost.Text = "0"
        For i As Integer = 0 To DataGridView1.Items.Count - 1
            If DataGridView1.Items(i)(dt.Columns("IsSelected").Ordinal) Then
                Value.Text += Val(DataGridView1.Items(i)(dt.Columns("Value").Ordinal))
                Cost.Text += Val(DataGridView1.Items(i)(dt.Columns("Cost").Ordinal))
            End If
        Next
    End Sub


    Private Sub DataGridView1_CurrentCellChanged(sender As Object, e As EventArgs) Handles DataGridView1.CurrentCellChanged
        CalcTotal()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If Not bm.ShowDeleteMSG("هل أنت متأكد من السداد؟") Then
            Return
        End If
        CalcTotal()

        Dim str As String = ""
        For i As Integer = 0 To DataGridView1.Items.Count - 1
            If DataGridView1.Items(i)(dt.Columns("IsSelected").Ordinal) Then
                str += " update Services set IsLabToLabDone=-1,IsLabToLabDoneEmpId=" & Md.UserName & ",IsLabToLabDoneGetDate=GETDATE(),IslabToLabPrice=" & Val(DataGridView1.Items(i)(dt.Columns("Cost").Ordinal)) & "    where MyLine=" & DataGridView1.Items(i)(dt.Columns("MyLine").Ordinal) & " and IsLabToLab=1 AND IsLabToLabDone=0 and Canceled=0 and Returned=0"
            End If
        Next

        If bm.ExecuteNonQuery(str) Then
            'bm.ShowMSG("تم العملية بنجاح")
        Else
            bm.ShowMSG("فشلت العملية")
            Return
        End If


        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "Header", "@Current"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title, 1}
        rpt.Rpt = "ServicesIsLabToLab.rpt"
        rpt.Show()

        str = ""
        For i As Integer = 0 To DataGridView1.Items.Count - 1
            If DataGridView1.Items(i)(dt.Columns("IsSelected").Ordinal) Then
                str += " update Services set IsLabToLabDone=1,IsLabToLabDoneEmpId=" & Md.UserName & ",IsLabToLabDoneGetDate=GETDATE(),IslabToLabPrice=" & Val(DataGridView1.Items(i)(dt.Columns("Cost").Ordinal)) & "    where MyLine=" & DataGridView1.Items(i)(dt.Columns("MyLine").Ordinal) & " and IsLabToLab=1 AND IsLabToLabDone=-1 and Canceled=0 and Returned=0"
            End If
        Next

        bm.ExecuteNonQuery(str)

        UserControl_Loaded(Nothing, Nothing)
    End Sub

    Private Sub BtnSave_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "Header", "@Current"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title, 0}
        rpt.Rpt = "ServicesIsLabToLab.rpt"
        rpt.Show()
    End Sub
End Class