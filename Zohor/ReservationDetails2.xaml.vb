Imports System.Data
Imports System.ComponentModel
Imports System.IO

Public Class ReservationDetails2
    Dim bm As New BasicMethods
    Dim bf As New BasicForm 
    Dim WithEvents BackgroundWorker1 As New BackgroundWorker

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        TabControl3.SelectedIndex = 0
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {}, {btnPrint})
        LoadResource()
        For i As Integer = 0 To TabControl3.Items.Count - 1
            TabControl3.SelectedIndex = i
        Next
        TabControl3.SelectedIndex = 0

        t_Tick(Nothing, Nothing)
        EmpId.IsEnabled = True

        LoadDrugsDoses()
        LoadDiagnoseGroups()
    End Sub

    Sub LoadDrugsDoses()
        Try
            WR2.Children.Clear()
            Dim dt As DataTable = bm.ExecuteAdapter("Select Id,Name,Name2 from DrugsDoses")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "DD" & dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.Tag = dt.Rows(i)("Name2").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 100
                x.Height = 50
                x.Margin = New Thickness(10, 10, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.ToolTip = x.Tag
                x.Background = bf.btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR2.Children.Add(x)
                AddHandler x.Click, AddressOf btnDrugsDoses
            Next
        Catch ex As Exception
        End Try
    End Sub

    Sub LoadDiagnoseGroups()
        Try
            WR22.Children.Clear()
            Dim dt As DataTable = bm.ExecuteAdapter("Select Id,Name from DiagnoseGroups")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "DD" & dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 100
                x.Height = 50
                x.Margin = New Thickness(10, 10, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.ToolTip = x.Tag
                x.Background = bf.btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR22.Children.Add(x)
                AddHandler x.Click, AddressOf btnDiagnoseGroups
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnDiagnoseGroups(sender As Object, e As RoutedEventArgs)
        Try
            WR1.Children.Clear()
            Dim dt As DataTable = bm.ExecuteAdapter("Select Id,Name from Diagnoses where GroupId=" & CType(sender, Button).Name.Replace("DD", ""))
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "DD" & dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.Tag = dt.Rows(i)("Name").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 100
                x.Height = 50
                x.Margin = New Thickness(10, 10, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.ToolTip = x.Tag
                x.Background = bf.btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR1.Children.Add(x)
                AddHandler x.Click, AddressOf btnDiagnoses
            Next
        Catch ex As Exception
        End Try
    End Sub



    Private Sub btnReservClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        btnNew_Click(Nothing, Nothing)
        Dim btn As Button = sender
        lblReservId.Content = btn.Tag.ToString
        lblTime.Content = btn.Content.ToString.Split(vbCrLf)(0)
        Dim dt As DataTable = bm.ExecuteAdapter("select * from Reservations where EmpId='" & EmpId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        If dt.Rows.Count > 0 Then
            lblTime.Content = dt.Rows(0)("Time").ToString
            CaseId.Text = dt.Rows(0)("CaseId").ToString
            CaseName.Text = dt.Rows(0)("CaseName").ToString
            VisitingType.SelectedValue = dt.Rows(0)("VisitingType").ToString
            Details.Text = dt.Rows(0)("Details").ToString
            Details2.Text = dt.Rows(0)("Details2").ToString
            Details3.Text = dt.Rows(0)("Details3").ToString
            Details4.Text = dt.Rows(0)("Details4").ToString
        End If

        dt = bm.ExecuteAdapter("select Weight,OperatedBefore,DateOfBirth,HomePhone,Mobile from Cases where Id='" & CaseId.Text & "'")
        If dt.Rows.Count > 0 Then
            Weight.Text = dt.Rows(0)("Weight").ToString

            Try : OperatedBefore.IsChecked = IIf(dt.Rows(0)("OperatedBefore").ToString, True, False) : Catch : End Try

            DateOfBirth.Text = dt.Rows(0)("DateOfBirth").ToString
            HomePhone.Text = dt.Rows(0)("HomePhone").ToString
            Mobile.Text = dt.Rows(0)("Mobile").ToString

            Try
                Dim DOB As Date = New DateTime(DateOfBirth.SelectedDate.Value.Year, DateOfBirth.SelectedDate.Value.Month, DateOfBirth.SelectedDate.Value.Day)
                Dim tday As TimeSpan = DateTime.Now.Subtract(dob)
                Dim years As Integer, months As Integer, days As Integer
                months = 12 * (DateTime.Now.Year - DOB.Year) + (DateTime.Now.Month - DOB.Month)

                If DateTime.Now.Day < DOB.Day Then
                    months -= 1
                    days = DateTime.DaysInMonth(DOB.Year, DOB.Month) - DOB.Day + DateTime.Now.Day
                Else
                    days = DateTime.Now.Day - DOB.Day
                End If
                years = Math.Floor(months / 12)
                months -= years * 12

                D.Text = days
                M.Text = months
                Y.Text = years
            Catch ex As Exception
            End Try

        End If

        LoadTree()
    End Sub


    Private Sub DatePicker1_SelectedDatesChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles DatePicker1.SelectedDateChanged, EmpId.SelectionChanged
        LoadReservations()
    End Sub


    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNew.Click
        lblReservId.Content = ""
        lblTime.Content = ""
        CaseId.Clear()
        CaseName.Clear()


        Weight.Clear()
        OperatedBefore.IsChecked = False
        D.Clear()
        M.Clear()
        Y.Clear()
        HomePhone.Clear()
        Mobile.Clear()

        VisitingType.SelectedIndex = 0
        Details.Clear()
        Details2.Clear()
        Details3.Clear()
        Details4.Clear()

        TreeView1.Items.Clear()

    End Sub

    Dim AllowPrint As Boolean = False
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click
        btnSave_Click(sender, e)
        If Not AllowPrint Then Return
        Dim rpt As New ReportViewer
        If EmpId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        Dim MyNow As DateTime = bm.MyGetDate()
        rpt.paraname = New String() {"@EmpId", "@CaseId", "@FromDate", "@ToDate", "@FromId", "@ToId", "@All", "Header"}
        rpt.paravalue = New String() {Val(EmpId.SelectedValue), Val(CaseId.Text), bm.ToStrDate(DatePicker1.SelectedDate), bm.ToStrDate(DatePicker1.SelectedDate), lblReservId.Content.ToString, lblReservId.Content.ToString, 0, "Patient History"}
        rpt.Rpt = "DailyReservationsOne2.rpt"
        'rpt.Show()
        rpt.Print()
        LoadReservations()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        AllowPrint = False
        If Not Valid() Then Return

        Post(1)

        AllowPrint = True
        If sender Is btnPrint Then Return

        btnNew_Click(Nothing, Nothing)
        LoadReservations()
    End Sub

    Function Delete() As Boolean
        If EmpId.SelectedIndex < 1 OrElse DatePicker1.SelectedDate Is Nothing OrElse lblReservId.Content.ToString = "" Then
            Return False
        End If

        Post(0)

        Return True
    End Function

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Not Delete() Then Return
            btnNew_Click(Nothing, Nothing)
            LoadReservations()
        End If
    End Sub

    Function Valid() As Boolean
        If EmpId.SelectedIndex < 1 OrElse DatePicker1.SelectedDate Is Nothing OrElse lblReservId.Content.ToString = "" Then
            Return False
        End If

        If CaseId.Text.Trim = "" Then
            bm.ShowMSG("Please, Select a Patient")
            CaseId.Focus()
            Return False
        End If
        If VisitingType.SelectedIndex < 1 Then
            bm.ShowMSG("Please, Select a Visiting Type")
            VisitingType.Focus()
            Return False
        End If

        Return True
    End Function


    Sub LoadReservations()
        Try
            WR.Children.Clear()
            btnNew_Click(Nothing, Nothing)
            If EmpId.SelectedIndex < 1 Then Return

            Dim dt As DataTable = bm.ExecuteAdapter("LoadReservations", New String() {"EmpId", "Daydate"}, New String() {Val(EmpId.SelectedValue.ToString), bm.ToStrDate(DatePicker1.SelectedDate)})

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 100
                x.Height = 50
                x.Margin = New Thickness(10, 10 + i * 50, 0, 0)

                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
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
                WR.Children.Add(x)
                AddHandler x.Click, AddressOf btnReservClick

            Next
        Catch ex As Exception
        End Try

    End Sub


    Private Sub LoadCbos()
        bm.FillCombo("VisitingTypes", VisitingType, "")

    End Sub

    Private Sub Post(ByVal pp As Integer)
        If pp = 0 Then
            Details.Clear()
            Details2.Clear()
            Details3.Clear()
            Details4.Clear()
        End If

        bm.ExecuteNonQuery("update Reservations set Posted=" & pp & ",Details='" & Details.Text.Replace("'", "''") & "',Details2='" & Details2.Text.Replace("'", "''") & "',Details3='" & Details3.Text.Replace("'", "''") & "',Details4='" & Details4.Text.Replace("'", "''") & "' where EmpId='" & EmpId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
    End Sub

    Private Sub MyLinkLabelClick(ByVal sender As Object, ByVal e As EventArgs) Handles ViewHistory.Click
        If Val(CaseId.Text) = 0 Then Return
        Dim rpt As New ReportViewer
        If EmpId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        Dim MyNow As DateTime = bm.MyGetDate()
        rpt.paraname = New String() {"@EmpId", "@CaseId", "@FromDate", "@ToDate", "@FromId", "@ToId", "@All", "Header"}
        rpt.paravalue = New String() {Val(EmpId.SelectedValue), Val(CaseId.Text), MyNow.Date.ToString, MyNow.Date.ToString, 0, 0, 1, "Patient History"}
        rpt.Rpt = "DailyReservationsDetailed2.rpt"
        rpt.Show()
    End Sub

    Private Sub TabControl3_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles TabControl3.SelectionChanged
        Try
            CType(CType(CType(e.AddedItems(0), TabItem).Header, Grid).Children(0), TextBlock).Foreground = New SolidColorBrush(Colors.LightGray)
        Catch ex As Exception
        End Try
        Try
            CType(CType(CType(e.AddedItems(0), TabItem).Header, Grid).Children(1), TextBlock).Foreground = New SolidColorBrush(Colors.LightGray)
        Catch ex As Exception
        End Try
        Try
            CType(CType(CType(e.RemovedItems(0), TabItem).Header, Grid).Children(0), TextBlock).Foreground = New SolidColorBrush(Colors.Black)
        Catch ex As Exception
        End Try
        Try
            CType(CType(CType(e.RemovedItems(0), TabItem).Header, Grid).Children(1), TextBlock).Foreground = New SolidColorBrush(Colors.Black)
        Catch ex As Exception
        End Try
    End Sub


    Private Sub t_Tick(ByVal sender As Object, ByVal e As EventArgs)
        TabControl3.SelectedIndex = 1
        TabControl3.SelectedIndex = 0
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Doctor='1' and Stopped='0' union select 0 Id,'-' Name order by Name", EmpId)
        EmpId.SelectedValue = Md.UserName
        Dim dt As New DataTable

        LoadCbos()
        DatePicker1.SelectedDate = bm.MyGetDate()

        LoadReservations()
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnRefresh.Click
        t_Tick(Nothing, Nothing)
    End Sub



    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        'lblName.SetResourceReference(ContentProperty, "Name")
    End Sub


    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDownload.Click
        Try
            MyImagedata = Nothing
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize <> 18 Then Return
            Dim s As New Forms.SaveFileDialog With {.Filter = "All files (*.*)|*.*"}
            s.FileName = CType(TreeView1.SelectedItem, TreeViewItem).Header

            If IsNothing(sender) Then
                MyBath = bm.GetNewTempName(s.FileName)
            Else
                If Not s.ShowDialog = Forms.DialogResult.OK Then Return
                MyBath = s.FileName
            End If

            btnDownload.IsEnabled = False
            F1 = EmpId.SelectedValue.ToString
            F12 = bm.ToStrDate(DatePicker1.SelectedDate)
            F13 = lblReservId.Content.ToString
            F2 = CType(TreeView1.SelectedItem, TreeViewItem).Tag
            BackgroundWorker1.RunWorkerAsync()
        Catch ex As Exception
        End Try
    End Sub
    Dim F2 As String = "", F1 As String = "", F12 As String = "", F13 As String = ""
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim myCommand As SqlClient.SqlCommand
            myCommand = New SqlClient.SqlCommand("select Image from CaseAttachments2 where EmpId='" & F1 & "' and DayDate='" & F12 & "' and ReservId='" & F13 & "' and AttachedName='" & F2 & "'" & bm.AppendWhere, con)
            If con.State <> ConnectionState.Open Then con.Open()
            MyImagedata = CType(myCommand.ExecuteScalar(), Byte())
        Catch ex As Exception
        End Try
        con.Close()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            File.WriteAllBytes(MyBath, MyImagedata)
            Process.Start(MyBath)
        Catch ex As Exception
        End Try
        btnDownload.IsEnabled = True
    End Sub

    Dim MyImagedata() As Byte
    Dim MyBath As String = ""
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteFile.Click
        Try
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize = 18 Then
                If bm.ShowDeleteMSG("MsgDeleteFile") Then
                    bm.ExecuteNonQuery("delete from CaseAttachments2 where EmpId='" & EmpId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "' and AttachedName='" & TreeView1.SelectedItem.Header & "'" & bm.AppendWhere)
                    LoadTree()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadTree()
        Dim dt As DataTable = bm.ExecuteAdapter("select AttachedName from CaseAttachments2 where EmpId='" & EmpId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "' " & bm.AppendWhere)
        TreeView1.Items.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim nn As New TreeViewItem
            nn.Foreground = Brushes.DarkRed
            nn.FontSize = 18
            nn.Tag = dt.Rows(i)(0).ToString
            nn.Header = dt.Rows(i)(0).ToString
            TreeView1.Items.Add(nn)
        Next
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAttach.Click
        Dim o As New Forms.OpenFileDialog
        o.Multiselect = True
        If o.ShowDialog = Forms.DialogResult.OK Then
            For i As Integer = 0 To o.FileNames.Length - 1
                bm.SaveFile("CaseAttachments2", "EmpId", EmpId.SelectedValue.ToString, "DayDate", bm.ToStrDate(DatePicker1.SelectedDate), "ReservId", lblReservId.Content.ToString, "AttachedName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "Image", o.FileNames(i))
            Next
        End If
        LoadTree()
    End Sub


    Private Sub TreeView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TreeView1.MouseDoubleClick
        Button4_Click(Nothing, Nothing)
    End Sub

    Private Sub btnDrugsDoses(sender As Object, e As RoutedEventArgs)
        Details.Text = sender.Tag
    End Sub

    Private Sub btnDiagnoses(sender As Object, e As RoutedEventArgs)
        Details2.Text &= sender.Tag & vbCrLf
    End Sub


End Class
