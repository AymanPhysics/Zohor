Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.ComponentModel
Imports System.Windows.Threading
Imports System.Threading.Tasks

Public Class TasksDetails
    Public Flag As Integer = 1
    Dim bm As New BasicMethods
    Dim mydt As New DataTable

    Dim t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 30)}

    Private Sub Schedule_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return

        AddHandler t.Tick, AddressOf Tick
        LoadAllData()
    End Sub

    Sub LoadAllData()
        t.Stop()
        Task.Factory.StartNew(AddressOf LoadAll)
    End Sub


    Dim MyGetDate As String = ""
    Private Sub LoadAll()
        Try

            mydt = bm.ExecuteAdapter("select Id,dbo.GetEmpArName(EmpId)EmpName,Notes,MyLine,dbo.ToStrDate(Daydate)Daydate,MyGetDate,PreventDone,(case when cast(MyGetDate as Date)=cast(GetDate() as Date) then 'Red' else 'Yellow' end) MyColor from Tasks where IsDeleted=0 and isnull(DidEmpId,0)=0 order by MyGetDate desc")

            Dispatcher.BeginInvoke(Sub()
                                       If Not mydt Is Nothing Then
                                           Dim dv2 As New DataView
                                           dv2.Table = mydt
                                           DataGridView2.ItemsSource = dv2
                                           Try
                                               DataGridView2.Columns(DataGridView2.Columns.Count - 2).Visibility = Visibility.Hidden
                                               DataGridView2.Columns(DataGridView2.Columns.Count - 1).Visibility = Visibility.Hidden
                                           Catch ex As Exception
                                           End Try

                                           Try

                                               DataGridView2.Columns(mydt.Columns("Id").Ordinal).Visibility = Visibility.Hidden
                                               DataGridView2.Columns(mydt.Columns("MyLine").Ordinal).Visibility = Visibility.Hidden
                                               DataGridView2.Columns(mydt.Columns("Notes").Ordinal).Width = 600
                                               DataGridView2.Columns(mydt.Columns("EmpName").Ordinal).Header = "المستخدم"
                                               DataGridView2.Columns(mydt.Columns("Notes").Ordinal).Header = "البيان"
                                               DataGridView2.Columns(mydt.Columns("Daydate").Ordinal).Header = "اليوم"
                                               DataGridView2.Columns(mydt.Columns("MyGetDate").Ordinal).Header = "وقت التحديث"
                                           Catch ex As Exception
                                           End Try

                                           Try
                                               If DataGridView2.Items.Count > 0 AndAlso (MyGetDate = "" OrElse MyGetDate <> bm.ToStrDateTimeFormated(DataGridView2.Items(0)("MyGetDate"))) Then
                                                   Dim w As MyWindow = Parent
                                                   w.PreventClosing = True
                                                   w.WindowState = WindowState.Maximized
                                                   w.Activate()
                                               End If
                                           Catch ex As Exception
                                           End Try

                                           t.Start()
                                           Try
                                               If DataGridView2.Items.Count > 0 Then
                                                   MyGetDate = bm.ToStrDateTimeFormated(DataGridView2.Items(0)("MyGetDate"))
                                               End If
                                           Catch ex As Exception
                                           End Try


                                       End If
                                   End Sub)

            bm.ExecuteNonQuery("InsertTasksUsers", {"UserName"}, {Md.UserName})

        Catch ex As Exception
        End Try
    End Sub


    Private Sub DataGridView2_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DataGridView2.SelectionChanged
        If DataGridView2.SelectedItem Is Nothing Then Return

        MyLine = DataGridView2.SelectedItem("MyLine")
        Notes.Text = DataGridView2.SelectedItem("Notes")
        If DataGridView2.SelectedItem("PreventDone").ToString = 1 Then
            btnSave.Visibility = Visibility.Hidden
        Else
            btnSave.Visibility = Visibility.Visible
        End If
    End Sub

    Dim MyLine As Integer = 0


    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If DataGridView2.SelectedItem Is Nothing Then Return
        If bm.ShowDeleteMSG("هل أنت  متأكد من إتمام المهمة؟") Then
            bm.ExecuteNonQuery("update Tasks set DidEmpId=" & Md.UserName & ",DidDatetime=GetDate(),DidNotes='" & DidNotes.Text.Trim.Replace("'", "''") & "' where MyLine=" & MyLine)
            LoadAll()
            DidNotes.Clear()
        End If

    End Sub

    Private Sub Tick(sender As Object, e As EventArgs)
        LoadAll()
    End Sub

End Class
