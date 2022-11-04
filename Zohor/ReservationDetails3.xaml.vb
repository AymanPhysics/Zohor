Imports System.Data
Imports System.ComponentModel
Imports System.IO

Public Class ReservationDetails3
    Dim bm As New BasicMethods
    Dim bf As New BasicForm

    Dim WithEvents BackgroundWorker1 As New BackgroundWorker
    Dim t As New Forms.Timer With {.Interval = 1500}
    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Doctor='1' and Stopped='0' union select 0 Id,'-' Name order by Name", EmpId)
        EmpId.SelectedValue = Md.UserName

        EmpId.IsEnabled = True
        If Md.MyProjectType = ProjectType.X Then
            EmpId.IsEnabled = True
        End If

        DatePicker1.SelectedDate = Now.Date

    End Sub

    Private Sub btnReservClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If Md.MyProjectType = ProjectType.X Then
            Dim c As New Cases2
            Dim frm As New MyWindow With {.Content = c, .WindowState = WindowState.Maximized}
            c.Cases2_Loaded(Nothing, Nothing)
            c.txtID.Text = sender.Tag
            c.txtID_LostFocus(Nothing, Nothing)
            frm.ShowDialog()
            LoadReservations()
        End If
    End Sub


    Private Sub DatePicker1_SelectedDatesChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles DatePicker1.SelectedDateChanged, EmpId.SelectionChanged
        LoadReservations()
    End Sub


    Sub LoadReservations()
        Try
            WR.Children.Clear()
            WR1.Children.Clear()
            WR2.Children.Clear()
            If EmpId.SelectedIndex < 1 Then Return

            Dim dt As DataTable = bm.ExecuteAdapter("LoadReservations", New String() {"EmpId", "Daydate"}, New String() {Val(EmpId.SelectedValue.ToString), bm.ToStrDate(DatePicker1.SelectedDate)})

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("CaseId").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 100
                x.Height = 50
                'x.Margin = New Thickness(10, 10 + i * 60, 0, 0)
                x.Margin = New Thickness(10, 10, 10, 10)
                'x.HorizontalAlignment = HorizontalAlignment.Left
                'x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = dt.Rows(i)("Time").ToString.Replace(vbCrLf, " ") & vbCrLf & dt.Rows(i)("CaseName").ToString
                x.ToolTip = x.Content
                x.ToolTip = x.Content
                x.Background = bf.btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                If dt.Rows(i)("Posted") = 1 Then
                    x.Background = bf.btnSave.Background
                    x.Foreground = System.Windows.Media.Brushes.Black
                End If
                If dt.Rows(i)("IsExists") = 1 Then
                    x.Background = bf.btnSave.Background
                    x.Foreground = System.Windows.Media.Brushes.Black
                End If
                If dt.Rows(i)("VisitingType") = 4 Then
                    WR1.Children.Add(x)
                ElseIf dt.Rows(i)("VisitingType") = 5 Then
                    WR2.Children.Add(x)
                Else
                    WR.Children.Add(x)
                End If
                AddHandler x.Click, AddressOf btnReservClick

            Next
        Catch ex As Exception
        End Try

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As RoutedEventArgs) Handles btnRefresh.Click
        LoadReservations()
    End Sub
End Class
