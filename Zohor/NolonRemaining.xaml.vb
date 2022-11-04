Imports System.Data

Public Class NolonRemaining
    Dim bm As New BasicMethods
    
    Dim dt As New DataTable
    Dim dv As New DataView
    Public Header As String = ""

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        DueDate.SelectedDate = bm.MyGetDate()
        DueDate_SelectedDateChanged(Nothing, Nothing)
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        Banner1.StopTimer = True
        Banner1.Header = Header
    End Sub

    Private Sub DataGridView1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.Key = Input.Key.Up Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex - 1
            ElseIf e.Key = Input.Key.Down Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex + 1
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)(6) Then
                bm.ExecuteNonQuery("update Nolon set Done=1 where Done=0 and InvoiceNo=" & dt.Rows(i)(0))
            End If
        Next
        DueDate_SelectedDateChanged(Nothing, Nothing)
    End Sub

    Private Sub LoadResource()
        'lblName.SetResourceReference(ContentProperty, "Name")
    End Sub

    Private Sub DueDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DueDate.SelectedDateChanged

        Try
            dt = bm.ExecuteAdapter("GetNolonRemaining", {"DueDate"}, {bm.ToStrDate(DueDate.SelectedDate)})
            dt.TableName = "tbl"

            dv.Table = dt
            DataGridView1.ItemsSource = dv
            
            DataGridView1.Columns(0).Header = "المسلسل"
            DataGridView1.Columns(1).Header = "السائق"
            DataGridView1.Columns(2).Header = "النولون"
            DataGridView1.Columns(3).Header = "العهدة"
            DataGridView1.Columns(4).Header = "باقى النولون"
            DataGridView1.Columns(5).Header = "ملاحظات"
            DataGridView1.Columns(6).Header = "تم السداد"

            DataGridView1.Columns(0).IsReadOnly = True
            DataGridView1.Columns(1).IsReadOnly = True
            DataGridView1.Columns(2).IsReadOnly = True
            DataGridView1.Columns(3).IsReadOnly = True
            DataGridView1.Columns(4).IsReadOnly = True
            DataGridView1.Columns(5).IsReadOnly = True

            DataGridView1.SelectedIndex = 0
        Catch
        End Try

    End Sub

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        btnSave_Click(Nothing, Nothing)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@DueDate", "Header"}
        rpt.paravalue = New String() {DueDate.SelectedDate, CType(Parent, Page).Title}
        rpt.Rpt = "NolonRemaining.rpt"
        rpt.Show()
    End Sub

    Private Sub btnPrint2_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint2.Click
        btnSave_Click(Nothing, Nothing)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@DueDate", "Header"}
        rpt.paravalue = New String() {DueDate.SelectedDate, CType(Parent, Page).Title}
        rpt.Rpt = "NolonRemaining2.rpt"
        rpt.Show()
    End Sub
End Class