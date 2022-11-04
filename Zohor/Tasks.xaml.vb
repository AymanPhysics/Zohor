Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.ComponentModel
Imports System.Windows.Threading

Public Class Tasks
    Public Flag As Integer = 1
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Dim dt2 As New DataTable

    Dim t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 30)}

    Private Sub Schedule_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return

        AddHandler t.Tick, AddressOf Tick
        LoadAll()
    End Sub

    Private Sub LoadAll() 
        dt2 = bm.ExecuteAdapter("select Id,dbo.GetEmpArName(EmpId)EmpName,Notes,MyLine,dbo.ToStrDate(Daydate)Daydate,MyGetDate,PreventDone from Tasks where IsDeleted=0 and isnull(DidEmpId,0)=0 order by MyGetDate desc")
        dt2.TableName = "tbl"

        Dim dv2 As New DataView
        dv2.Table = dt2
        DataGridView2.ItemsSource = dv2
        Try

            DataGridView2.Columns(dt2.Columns("Id").Ordinal).Visibility = Visibility.Hidden
            DataGridView2.Columns(dt2.Columns("MyLine").Ordinal).Visibility = Visibility.Hidden
            DataGridView2.Columns(dt2.Columns("PreventDone").Ordinal).Visibility = Visibility.Hidden
            DataGridView2.Columns(dt2.Columns("Notes").Ordinal).Width = 200
            DataGridView2.Columns(dt2.Columns("EmpName").Ordinal).Header = "المستخدم"
            DataGridView2.Columns(dt2.Columns("Notes").Ordinal).Header = "البيان"
            DataGridView2.Columns(dt2.Columns("Daydate").Ordinal).Header = "اليوم"
            DataGridView2.Columns(dt2.Columns("MyGetDate").Ordinal).Header = "وقت التحديث"

        Catch ex As Exception

        End Try

        If Calendar1.SelectedDate Is Nothing Then Calendar1.SelectedDate = bm.MyGetDate
        Calendar1_SelectedDatesChanged(Nothing, Nothing)
         
    End Sub

    Private Sub Calendar1_SelectedDatesChanged(sender As Object, e As SelectionChangedEventArgs) Handles Calendar1.SelectedDatesChanged
        dt = bm.ExecuteAdapter("select Id,dbo.GetEmpArName(EmpId)EmpName,Notes,MyLine,MyGetDate,DidEmpId,dbo.GetEmpArName(DidEmpId)DidEmpName,DidDatetime,DidNotes,PreventDone from Tasks where IsDeleted=0 and DayDate='" & bm.ToStrDate(Calendar1.SelectedDate) & "' order by MyGetDate desc")
        dt.TableName = "tbl"

        Dim dv As New DataView
        dv.Table = dt
        DataGridView1.ItemsSource = dv

        Try

            DataGridView1.Columns(dt.Columns("Id").Ordinal).Visibility = Visibility.Hidden
            DataGridView1.Columns(dt.Columns("MyLine").Ordinal).Visibility = Visibility.Hidden
            DataGridView1.Columns(dt.Columns("DidEmpId").Ordinal).Visibility = Visibility.Hidden
            DataGridView1.Columns(dt.Columns("PreventDone").Ordinal).Visibility = Visibility.Hidden
            
            DataGridView1.Columns(dt.Columns("Notes").Ordinal).Width = 300
            DataGridView1.Columns(dt.Columns("DidNotes").Ordinal).Width = 300

            DataGridView1.Columns(dt.Columns("EmpName").Ordinal).Header = "المستخدم"
            DataGridView1.Columns(dt.Columns("Notes").Ordinal).Header = "البيان"
            DataGridView1.Columns(dt.Columns("MyGetDate").Ordinal).Header = "الوقت"

            DataGridView1.Columns(dt.Columns("DidEmpName").Ordinal).Header = "المنفذ"
            DataGridView1.Columns(dt.Columns("DidDatetime").Ordinal).Header = "وقت التنفيذ"
            DataGridView1.Columns(dt.Columns("DidNotes").Ordinal).Header = "بيان التنفيذ"
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DataGridView1.SelectionChanged
        If DataGridView1.SelectedItem Is Nothing Then
            MyLine = 0
            Notes.Clear()
            PreventDone.IsChecked = False
            Return
        End If
        MyLine = DataGridView1.SelectedItem("MyLine")
        Notes.Text = DataGridView1.SelectedItem("Notes")
        PreventDone.IsChecked = IIf(DataGridView1.SelectedItem("PreventDone") = 1, True, False)
    End Sub

    Private Sub DataGridView2_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DataGridView2.SelectionChanged
        If DataGridView2.SelectedItem Is Nothing Then
            MyLine = 0
            Notes.Clear()
            PreventDone.IsChecked = False
            Return
        End If
        Calendar1.SelectedDate = DateTime.Parse(DataGridView2.SelectedItem("Daydate"))
        MyLine = DataGridView2.SelectedItem("MyLine")
        Notes.Text = DataGridView2.SelectedItem("Notes")
        PreventDone.IsChecked = IIf(DataGridView2.SelectedItem("PreventDone") = 1, True, False)
    End Sub

    Dim MyLine As Integer = 0
    Private Sub btnNew_Click(sender As Object, e As RoutedEventArgs) Handles btnNew.Click
        LoadAll()

        MyLine = 0
        Notes.Clear()
        Notes.Focus()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG("MsgDelete") Then
            'bm.ExecuteNonQuery("delete Tasks where DayDate='" & bm.ToStrDate(Calendar1.SelectedDate) & "' and MyLine=" & MyLine)
            bm.ExecuteNonQuery("update Tasks set IsDeleted=1,DeletedDate=Getdate(),DeletedUser=" & Md.UserName & " where DayDate='" & bm.ToStrDate(Calendar1.SelectedDate) & "' and MyLine=" & MyLine)
            btnNew_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If Notes.Text.Trim = "" Then Return

        If MyLine = 0 Then
            bm.ExecuteNonQuery("insert Tasks(DayDate,Id,EmpId,UserName,MyGetDate,Notes,PreventDone) select '" & bm.ToStrDate(Calendar1.SelectedDate) & "',(select isnull(MAX(Id),0)+1 from Tasks where DayDate='" & bm.ToStrDate(Calendar1.SelectedDate) & "')," & Md.UserName & "," & Md.UserName & ",GetDate(),'" & Notes.Text.Trim.Replace("'", "''") & "'," & IIf(PreventDone.IsChecked, 1, 0))
        Else
            bm.ExecuteNonQuery("update Tasks set EmpId=" & Md.UserName & ",UserName=" & Md.UserName & ",MyGetDate=GetDate(),Notes='" & Notes.Text.Trim.Replace("'", "''") & "',PreventDone=" & IIf(PreventDone.IsChecked, 1, 0) & " where DayDate='" & bm.ToStrDate(Calendar1.SelectedDate) & "' and MyLine=" & MyLine)
        End If
        btnNew_Click(Nothing, Nothing)
    End Sub

    Private Sub Tick(sender As Object, e As EventArgs)
        LoadAll()
    End Sub

    Private Sub btnSave_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {bm.ToStrDate(Calendar1.SelectedDate), bm.ToStrDate(Calendar1.SelectedDate), CType(Parent, Page).Title}
        rpt.Rpt = "Tasks.rpt"
        rpt.Show()
    End Sub

    Private Sub btnPrint2_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@MyLine", "Notes", "Header"}
        rpt.paravalue = New String() {MyLine, Notes.Text, CType(Parent, Page).Title}
        rpt.Rpt = "TasksUsers.rpt"
        rpt.Show()
    End Sub
End Class
