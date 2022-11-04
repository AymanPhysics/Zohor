Imports System.Data
Imports System.ComponentModel

Public Class ReservationsClinics
    Dim bm As New BasicMethods
    Dim bf As New BasicForm
    Dim dtClinics As New DataTable

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSaveWithoutPrint}, {btnDelete}, {}, {btnSave})
        LoadResource()

        If Md.MyProjectType = ProjectType.X Then
            Value.Visibility = Visibility.Hidden
            Payed.Visibility = Visibility.Hidden
            Remaining.Visibility = Visibility.Hidden
            SerialId.Visibility = Visibility.Hidden
            SerialId2.Visibility = Visibility.Hidden
            lblValue.Visibility = Visibility.Hidden
            lblPayed.Visibility = Visibility.Hidden
            lblRemaining.Visibility = Visibility.Hidden
            lblSerial.Visibility = Visibility.Hidden
            lblSerialId2.Visibility = Visibility.Hidden
            Done.Visibility = Visibility.Hidden
            Canceled.Visibility = Visibility.Hidden
            Returned.Visibility = Visibility.Hidden
            LastSerialId.Visibility = Visibility.Hidden
            lblLastSerial.Visibility = Visibility.Hidden
        End If

        bm.Addcontrol_MouseDoubleClick({CaseId})

        bm.FillCombo("select Id,Name from Clinics union select 0 Id,'-' Name order by Name", ClinicId)

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReservation)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdRemaining)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdCanceled)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReturned)


        dtClinics = bm.ExecuteAdapter("select Id,Name,dbo.LoadClinicschedule(Id) Clinicschedule from Clinics")
        LoadDoctors()
        bm.FillCombo("VisitingTypes", VisitingType, "")
        Calendar1.SelectedDate = bm.MyGetDate()
        LoadReservationsClinics()
        Done_Unchecked(Nothing, Nothing)
        Canceled_Unchecked(Nothing, Nothing)
        Returned_Unchecked(Nothing, Nothing)
        Home_Click(Nothing, Nothing)
    End Sub

    Sub LoadDoctors()
        Try
            WR1.Children.Clear()
            For i As Integer = 0 To ClinicId.Items.Count - 1
                If ClinicId.Items(i)("Id").ToString = 0 Then Continue For
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & ClinicId.Items(i)("Id").ToString
                x.Tag = ClinicId.Items(i)("Id").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 115
                x.Height = 35
                x.Margin = New Thickness(5, 5, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = ClinicId.Items(i)("Name").ToString.Replace(vbCrLf, " ")
                x.ToolTip = ClinicId.Items(i)("Id").ToString & vbCrLf & x.Content & vbCrLf & vbCrLf & dtClinics.Select("Id=" & ClinicId.Items(i)("Id").ToString)(0)("Clinicschedule").ToString
                x.Background = btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR1.Children.Add(x)
                AddHandler x.Click, AddressOf btnClinicIdClick
            Next
        Catch ex As Exception
        End Try
    End Sub

    Dim IsNew As Boolean = False
    Private Sub btnReservClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        btnNew_Click(Nothing, Nothing)
        Dim btn As Button = sender
        lblReservId.Content = btn.Tag.ToString.Split(",")(1)
        lblTime.Content = btn.Tag.ToString.Split(",")(2).Replace(vbCrLf, " ")
        DayDate.Content = New DateTime(CInt(btn.Tag.ToString.Split(",")(0).Split("_")(2)), CInt(btn.Tag.ToString.Split(",")(0).Split("_")(1)), CInt(btn.Tag.ToString.Split(",")(0).Split("_")(0))).ToShortDateString()
        Dim dt As DataTable = bm.ExecuteAdapter("select * from ReservationsClinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        EmpIdReservation.SelectedValue = Md.UserName
        IsNew = True
        If dt.Rows.Count > 0 Then
            IsNew = False
            Canceled.IsChecked = bm.IIf(dt.Rows(0)("Canceled") = 1, True, False)
            Returned.IsChecked = bm.IIf(dt.Rows(0)("Returned") = 1, True, False)
            Done.IsChecked = bm.IIf(dt.Rows(0)("Done") = 1, True, False)

            lblTime.Content = dt.Rows(0)("Time").ToString
            CaseId.Text = dt.Rows(0)("CaseId").ToString
            CaseName.Text = dt.Rows(0)("CaseName").ToString
            CaseID_LostFocus(Nothing, Nothing)

            VisitingType.SelectedValue = dt.Rows(0)("VisitingType").ToString
            Value.Text = dt.Rows(0)("Value").ToString
            Payed.Text = dt.Rows(0)("Payed").ToString
            Remaining.Text = dt.Rows(0)("Remaining").ToString
            SerialId.Text = dt.Rows(0)("SerialId").ToString
            SerialId2.Text = dt.Rows(0)("SerialId2").ToString

            EmpIdReservation.SelectedValue = dt.Rows(0)("EmpIdReservation").ToString
            EmpIdRemaining.SelectedValue = dt.Rows(0)("EmpIdRemaining").ToString
            EmpIdCanceled.SelectedValue = dt.Rows(0)("EmpIdCanceled").ToString
            EmpIdReturned.SelectedValue = dt.Rows(0)("EmpIdReturned").ToString

            bm.SetPickerDate(CanceledDate, dt.Rows(0)("CanceledDate"))
            bm.SetPickerDate(ReturnedDate, dt.Rows(0)("ReturnedDate"))
            bm.SetPickerDate(RemainingDate, dt.Rows(0)("RemainingDate"))

            If Val(dt.Rows(0)("Posted").ToString) = 1 Then
                btnSave.Visibility = Visibility.Hidden
                btnSaveWithoutPrint.Visibility = Visibility.Hidden
                btnDelete.Visibility = Visibility.Hidden
            End If

            VisitingType.IsEnabled = False
            CaseId.IsEnabled = Md.Manager
            Value.IsEnabled = Md.Manager
            Payed.IsEnabled = Md.Manager

            If Canceled.IsChecked Then
                Canceled_Checked(Nothing, Nothing)
            Else
                Canceled_Unchecked(Nothing, Nothing)
            End If

            If Returned.IsChecked Then
                Returned_Checked(Nothing, Nothing)
            Else
                Returned_Unchecked(Nothing, Nothing)
            End If

            If Not Md.Manager Then
                CaseId.IsEnabled = Md.Manager
                Value.IsEnabled = Md.Manager
                Payed.IsEnabled = Md.Manager
            End If
        End If
        CaseId.Focus()
    End Sub

    Private Sub ClinicId_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ClinicId.SelectionChanged
        LoadReservationsClinics()
    End Sub

    Private Sub Calendar1_SelectedDatesChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles Calendar1.SelectedDatesChanged
        LoadReservationsClinics()
    End Sub

    Private Sub ViewByMonth_Checked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles ViewByWeek.Checked, ViewByMonth.Checked
        LoadReservationsClinics()
    End Sub


    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp
        If bm.ShowHelpCases(CaseId, CaseName, e) Then
            CaseID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select " & Resources.Item("CboName") & " Name from Cases where Id=" & CaseId.Text.Trim())
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile from Cases where Id=" & CaseId.Text.Trim())
        If dt.Rows.Count > 0 Then
            CaseId.ToolTip = Resources.Item("Id") & ": " & CaseId.Text & vbCrLf & Resources.Item("Name") & ": " & CaseName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
            CaseName.ToolTip = CaseId.ToolTip
        End If
    End Sub

    Private Sub Value_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Value.TextChanged, Payed.TextChanged
        Remaining.Text = Val(Value.Text) - Val(Payed.Text)
    End Sub


    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown, Payed.KeyDown, Remaining.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNew.Click
        btnSave.Visibility = Visibility.Visible
        btnSaveWithoutPrint.Visibility = Visibility.Visible
        btnDelete.Visibility = Visibility.Hidden 'Visible
        DayDate.Content = ""
        lblReservId.Content = ""
        lblTime.Content = ""

        CaseId.Clear()
        CaseName.Clear()
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""

        VisitingType.SelectedIndex = 0
        Value.Clear()
        Payed.Clear()
        Remaining.Clear()
        SerialId.Clear()
        SerialId2.Clear()
        Done.IsChecked = False

        Canceled.IsChecked = False
        Returned.IsChecked = False
        EmpIdReservation.SelectedIndex = 0
        EmpIdRemaining.SelectedIndex = 0
        EmpIdCanceled.SelectedIndex = 0
        EmpIdReturned.SelectedIndex = 0
        CanceledDate.SelectedDate = Nothing
        ReturnedDate.SelectedDate = Nothing

        Canceled.IsEnabled = True
        Returned.IsEnabled = True
        Done.IsEnabled = True

        CaseId.IsEnabled = True
        VisitingType.IsEnabled = True
        Value.IsEnabled = True
        Payed.IsEnabled = True

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click, btnSaveWithoutPrint.Click
        If Not Valid() Then Return

        If RemainingDate.SelectedDate Is Nothing Then
            RemainingDate.SelectedDate = "1900-01-01"
        End If
        If CanceledDate.SelectedDate Is Nothing Then
            CanceledDate.SelectedDate = "1900-01-01"
        End If
        If ReturnedDate.SelectedDate Is Nothing Then
            ReturnedDate.SelectedDate = "1900-01-01"
        End If


        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If SerialId.Text = "" Then State = BasicMethods.SaveState.Insert

        Dim str As String
        If IsNew Then
            str = "insert ReservationsClinics(ClinicId,DayDate,ReservId,Time,CaseId,CaseName,VisitingType,Value,Payed,Remaining,SerialId,UserName,MyGetDate,ReservationDate,Done,Canceled,Returned,RemainingDate,EmpIdReservation,EmpIdRemaining,EmpIdCanceled,CanceledDate,EmpIdReturned,ReturnedDate) select '" & ClinicId.SelectedValue.ToString & "','" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "','" & lblReservId.Content.ToString & "','" & lblTime.Content.ToString & "','" & CaseId.Text & "','" & CaseName.Text & "','" & VisitingType.SelectedValue.ToString & "','" & Val(Value.Text) & "','" & Val(Payed.Text) & "','" & Val(Remaining.Text) & "','" & SerialId.Text & "','" & Md.UserName & "',GetDate(),GetDate(),'" & bm.IIf(Done.IsChecked, 1, 0) & "','" & bm.IIf(Canceled.IsChecked, 1, 0) & "','" & bm.IIf(Returned.IsChecked, 1, 0) & "','" & bm.ToStrDate(RemainingDate.SelectedDate) & "','" & EmpIdReservation.SelectedValue.ToString & "','" & EmpIdRemaining.SelectedValue.ToString & "','" & EmpIdCanceled.SelectedValue.ToString & "','" & bm.ToStrDate(CanceledDate.SelectedDate) & "','" & EmpIdReturned.SelectedValue.ToString & "','" & bm.ToStrDate(ReturnedDate.SelectedDate) & "'"
        Else
            str = "update ReservationsClinics set CaseId='" & CaseId.Text & "',CaseName='" & CaseName.Text & "',VisitingType='" & VisitingType.SelectedValue.ToString & "',Value='" & Val(Value.Text) & "',Payed='" & Val(Payed.Text) & "',Remaining='" & Val(Remaining.Text) & "',Done='" & bm.IIf(Done.IsChecked, 1, 0) & "',Canceled='" & bm.IIf(Canceled.IsChecked, 1, 0) & "',Returned='" & bm.IIf(Returned.IsChecked, 1, 0) & "',RemainingDate='" & bm.ToStrDate(RemainingDate.SelectedDate) & "',SerialId='" & SerialId.Text & "',EmpIdReservation='" & EmpIdReservation.SelectedValue.ToString & "',EmpIdRemaining='" & EmpIdRemaining.SelectedValue.ToString & "',EmpIdCanceled='" & EmpIdCanceled.SelectedValue.ToString & "',CanceledDate='" & bm.ToStrDate(CanceledDate.SelectedDate) & "',EmpIdReturned='" & EmpIdReturned.SelectedValue.ToString & "',ReturnedDate='" & bm.ToStrDate(ReturnedDate.SelectedDate) & "',UserName='" & Md.UserName & "',MyGetDate=GetDate() where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "' and ReservId='" & lblReservId.Content.ToString & "'"
        End If


        If Not bm.ExecuteNonQuery(str) Then
            bm.ShowMSG("لم يتم الحفظ")
            If State = BasicMethods.SaveState.Insert Then
                SerialId.Text = ""
                LastSerialId.Text = ""
            End If
            Return
        End If


        'If Val(SerialId.Text) = 0 AndAlso Val(Payed.Text) > 0 Then
        '    SerialId.Text = bm.ExecuteScalar("updateReservationsClinicsSerialId", {"VisitingType", "ClinicId", "DayDate", "ReservId"}, {VisitingType.SelectedValue.ToString, ClinicId.SelectedValue.ToString, bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)), lblReservId.Content.ToString})
        '    LastSerialId.Text = SerialId.Text
        'End If

        'If SerialId2.IsVisible AndAlso Val(SerialId2.Text) = 0 AndAlso Val(Remaining.Text) > 0 Then
        '    SerialId2.Text = bm.ExecuteScalar("updateReservationsClinicsSerialId2", {"VisitingType", "ClinicId", "DayDate", "ReservId"}, {VisitingType.SelectedValue.ToString, ClinicId.SelectedValue.ToString, bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)), lblReservId.Content.ToString})
        '    LastSerialId.Text = SerialId2.Text
        'End If

        If Not Md.MyProjectType = ProjectType.Zohor AndAlso Not MyProjectType = ProjectType.X AndAlso Not MyProjectType = ProjectType.X AndAlso Not MyProjectType = ProjectType.X AndAlso Not Canceled.IsChecked AndAlso Not Returned.IsChecked AndAlso sender Is btnSave Then ShowRPT()

        bm.ExecuteNonQuery("update Cases set InOut=1 where Id=" & CaseId.Text.Trim() & "     insert CaseStatus(CaseId,UserName,MyGetDate,InOut) select " & Val(CaseId.Text) & "," & Md.UserName & ",GetDate()," & 1)

        Home_Click(Nothing, Nothing)
        btnNew_Click(Nothing, Nothing)
        LoadReservationsClinics()
    End Sub

    Function Delete() As Boolean
        If ClinicId.SelectedIndex < 1 OrElse DayDate.Content.ToString = "" OrElse lblReservId.Content.ToString = "" Then
            Return False
        End If

        Return bm.ExecuteNonQuery("delete ReservationsClinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "' and ReservId='" & lblReservId.Content.ToString & "'")
    End Function

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Not Delete() Then Return
            btnNew_Click(Nothing, Nothing)
            LoadReservationsClinics()
        End If
    End Sub

    Function Valid() As Boolean
        If ClinicId.SelectedIndex < 1 OrElse DayDate.Content.ToString = "" OrElse lblReservId.Content.ToString = "" Then
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

    Private Sub btnClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Calendar1.SelectedDate = CType(sender, Button).Content.ToString.Substring(0, 10)

        Dim ToId, ToName As New TextBox

        bm.ShowHelp("", ToId, ToName, New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.F1), "select cast(ReservId as varchar(100)) Id,CaseName+' - '+c.Mobile+' - '+c.HomePhone Name from ReservationsClinics r left join Cases c on(r.CaseId=c.Id) where r.ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(Calendar1.SelectedDate) & "'")


        Dim s As String = ("Reservation_" & CType(sender, Button).Content.ToString.Substring(0, 10) & "_" & ToId.Text).Replace("/", "_").Replace(" ", "_").Replace(":", "_").Replace(vbCrLf, "_")

        For Each b As Button In WR.Children
            If b.Name = s Then
                btnReservClick(b, Nothing)
                Exit Sub
            End If
        Next
    End Sub

    Sub LoadReservationsClinics()
        Try
            WR.Children.Clear()
            btnNew_Click(Nothing, Nothing)
            If ClinicId.SelectedIndex < 1 Then Return

            Dim dt As DataTable
            If ViewByWeek.IsChecked Then
                dt = bm.ExecuteAdapter("WeekFirstDay", New String() {"DayDate"}, New String() {bm.ToStrDate(Calendar1.SelectedDate.Value)})
            Else
                dt = bm.ExecuteAdapter("GetAllDaysInMonth", New String() {"Year", "Month"}, New String() {Calendar1.SelectedDate.Value.Year, Calendar1.SelectedDate.Value.Month})
            End If

            Dim DaysDt As DataTable = bm.ExecuteAdapter("select Saturday,Sunday,Monday,Tuesday,Wednesday,Thursday,Friday from Clinics where Id=" & ClinicId.SelectedValue.ToString)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "lbl_" & dt.Rows(i)("Line").ToString
                x.Tag = bm.FormatDate(dt.Rows(i)("Date"))
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 160
                x.Height = 50
                x.Margin = New Thickness(10, 10 + i * 60, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = bm.FormatDate(dt.Rows(i)("Date")) & "     " & Resources.Item(dt.Rows(i)("Day").ToString)
                x.ToolTip = x.Content
                x.Background = bf.btnNew.Background 'System.Windows.Media.Brushes.SkyBlue
                x.Foreground = System.Windows.Media.Brushes.Black
                If dt.Rows(i)("Date") = DateTime.Parse(Calendar1.SelectedDate.Value.Date.ToString) Then
                    x.Background = bf.btnSave.Background 'System.Windows.Media.Brushes.Gold
                    x.Foreground = System.Windows.Media.Brushes.Black
                End If
                AddHandler x.Click, AddressOf btnClick
                WR.Children.Add(x)

                Dim dt2 As DataTable = bm.ExecuteAdapter("LoadReservationsClinics", New String() {"ClinicId", "Daydate"}, New String() {Val(ClinicId.SelectedValue.ToString), bm.ToStrDate(bm.FormatDate(dt.Rows(i)("Date")))})
                For i2 As Integer = 0 To dt2.Rows.Count - 1
                    If DaysDt.Rows(0)(dt.Rows(i)("Day").ToString) <> 1 Then Continue For
                    Dim x2 As New Button
                    x2.Style = Application.Current.FindResource("GlossyCloseButton")
                    x2.FontSize -= 1
                    x2.Name = ("Reservation_" & bm.FormatDate(dt.Rows(i)("Date")) & "_" & dt2.Rows(i2)("Id").ToString).Replace("/", "_").Replace(" ", "_").Replace(":", "_").Replace(vbCrLf, "_")
                    x2.Tag = bm.FormatDate(dt.Rows(i)("Date")).Replace("/", "_") & "," & dt2.Rows(i2)("Id").ToString & "," & dt2.Rows(i2)("Time").ToString
                    x2.Width = 50
                    x2.Height = 50
                    x2.Margin = New Thickness(180 + i2 * 60, 10 + i * 60, 0, 0)
                    x2.HorizontalAlignment = HorizontalAlignment.Left
                    x2.VerticalAlignment = VerticalAlignment.Top
                    x2.Cursor = Input.Cursors.Pen
                    x2.Content = dt2.Rows(i2)("Id").ToString & vbCrLf & dt2.Rows(i2)("Time").ToString
                    x2.ToolTip = x2.Content
                    x2.Background = bf.btnNew.Background 'System.Windows.Media.Brushes.Blue
                    x2.Foreground = System.Windows.Media.Brushes.Black
                    If dt2.Rows(i2)("IsExists") = 1 Then
                        x2.Background = bf.btnSave.Background
                        x2.Foreground = System.Windows.Media.Brushes.Black
                    ElseIf dt2.Rows(i2)("IsExists") = 2 Then
                        x2.Background = bf.btnDelete.Background
                        x2.Foreground = System.Windows.Media.Brushes.Black
                    End If
                    WR.Children.Add(x2)
                    AddHandler x2.Click, AddressOf btnReservClick
                Next
            Next
        Catch ex As Exception
        End Try

    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrevios.Click
        If ViewByWeek.IsChecked Then
            Calendar1.SelectedDate = Calendar1.SelectedDate.Value.AddDays(-7)
        Else
            Calendar1.SelectedDate = Calendar1.SelectedDate.Value.AddMonths(-1)
        End If
        Calendar1.DisplayDate = Calendar1.SelectedDate
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNext.Click
        If ViewByWeek.IsChecked Then
            Calendar1.SelectedDate = Calendar1.SelectedDate.Value.AddDays(7)
        Else
            Calendar1.SelectedDate = Calendar1.SelectedDate.Value.AddMonths(1)
        End If
        Calendar1.DisplayDate = Calendar1.SelectedDate
    End Sub

    Private Sub Home_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Home.Click
        Calendar1.SelectedDate = bm.MyGetDate()
        Calendar1.DisplayDate = Calendar1.SelectedDate
    End Sub

    Private Sub Done_Checked(sender As Object, e As RoutedEventArgs) Handles Done.Checked
        Done.IsEnabled = Md.Manager

        lblSerialId2.Visibility = Visibility.Visible
        SerialId2.Visibility = Visibility.Visible

        RemainingDate.Visibility = Visibility.Visible
        RemainingDate.SelectedDate = bm.MyGetDate()
        RemainingDate.IsEnabled = Md.Manager
        EmpIdRemaining.Visibility = Visibility.Visible
        EmpIdRemaining.SelectedValue = Md.UserName
    End Sub
    Private Sub Done_Unchecked(sender As Object, e As RoutedEventArgs) Handles Done.Unchecked
        Done.IsEnabled = True

        lblSerialId2.Visibility = Visibility.Hidden
        SerialId2.Visibility = Visibility.Hidden

        RemainingDate.Visibility = Visibility.Hidden
        RemainingDate.SelectedDate = Nothing
        EmpIdRemaining.Visibility = Visibility.Hidden
        EmpIdRemaining.SelectedIndex = 0
    End Sub

    Private Sub VisitingType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles VisitingType.SelectionChanged
        Try
            Dim s As String = ""
            If VisitingType.SelectedValue = 1 Then s = "DetectionPrice"
            If VisitingType.SelectedValue = 2 Then s = "ConsultationPrice"
            Value.Text = bm.ExecuteScalar("select " & s & " from Clinics where Id='" & ClinicId.SelectedValue.ToString & "'")
            Payed.Text = Value.Text
        Catch
        End Try
    End Sub

    Private Sub btnClinicIdClick(sender As Object, e As RoutedEventArgs)
        ClinicId.SelectedValue = sender.Tag
        ClinicId_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub ShowRPT()
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@ClinicId", "@Date", "@Id", "Header"}
        rpt.paravalue = New String() {Val(ClinicId.SelectedValue), DayDate.Content, Val(lblReservId.Content), CType(Parent, Page).Title}
        rpt.Rpt = "ReservationsClinicsONE.rpt"
        rpt.Print()
    End Sub


    Private Sub LoadResource()
        btnSaveWithoutPrint.SetResourceReference(ContentProperty, "Save")
        btnSave.SetResourceReference(ContentProperty, "Print")

        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")

        Home.SetResourceReference(ContentProperty, "Today")

        TabItemReservationsClinics.SetResourceReference(TabItem.HeaderProperty, "ReservationsClinics")
        TabItemCurrentReservation.SetResourceReference(TabItem.HeaderProperty, "Current Reservation")

        lblClinicId.SetResourceReference(ContentProperty, "Clinic")
        ViewByWeek.SetResourceReference(ContentProperty, "View By Week")
        ViewByMonth.SetResourceReference(ContentProperty, "View By Month")
        Canceled.SetResourceReference(CheckBox.ContentProperty, "Cancel")
        Returned.SetResourceReference(CheckBox.ContentProperty, "Returned")
        Done.SetResourceReference(CheckBox.ContentProperty, "Done Remaining")

        lblDate.SetResourceReference(ContentProperty, "Date")
        lblReserv.SetResourceReference(ContentProperty, "Reserve")
        lblTim.SetResourceReference(ContentProperty, "Time")
        lblPatient.SetResourceReference(ContentProperty, "Patient")
        lblVisitingType.SetResourceReference(ContentProperty, "VisitingType")
        lblValue.SetResourceReference(ContentProperty, "Value")
        lblPayed.SetResourceReference(ContentProperty, "Payed")
        lblRemaining.SetResourceReference(ContentProperty, "Remaining")
        lblSerial.SetResourceReference(ContentProperty, "Serial")
        lblSerialId2.SetResourceReference(ContentProperty, "Serial")
        lblLastSerial.SetResourceReference(ContentProperty, "LastSerial")
        btnPrintSchedule.SetResourceReference(ContentProperty, "Print Schedule")
        btnPrintPatients.SetResourceReference(ContentProperty, "Print Patients")

    End Sub

    Private Sub Canceled_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Checked
        Canceled.IsEnabled = Md.Manager
        Returned.IsEnabled = Md.Manager
        Returned.IsChecked = False

        'Value.Text = ""
        'Payed.Text = ""
        'Remaining.Text = ""
        'Done.IsChecked = False

        Value.IsEnabled = False
        Payed.IsEnabled = False
        Done.IsEnabled = False

        CanceledDate.Visibility = Visibility.Visible
        CanceledDate.IsEnabled = Md.Manager
        EmpIdCanceled.Visibility = Visibility.Visible
        If EmpIdCanceled.SelectedIndex <= 0 Then
            EmpIdCanceled.SelectedValue = Md.UserName
            CanceledDate.SelectedDate = bm.MyGetDate()
        End If
    End Sub

    Private Sub Returned_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Returned.Checked
        Canceled.IsEnabled = Md.Manager
        Returned.IsEnabled = Md.Manager
        Canceled.IsChecked = False

        'Value.Text = ""
        'Payed.Text = ""
        'Remaining.Text = ""
        'Done.IsChecked = False

        Value.IsEnabled = False
        Payed.IsEnabled = False
        Done.IsEnabled = False

        ReturnedDate.Visibility = Visibility.Visible
        ReturnedDate.IsEnabled = Md.Manager
        EmpIdReturned.Visibility = Visibility.Visible
        If EmpIdReturned.SelectedIndex <= 0 Then
            EmpIdReturned.SelectedValue = Md.UserName
            ReturnedDate.SelectedDate = bm.MyGetDate()
        End If
    End Sub

    Private Sub Canceled_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Unchecked

        If Not Returned.IsChecked Then
            Canceled.IsEnabled = True
            Returned.IsEnabled = True

            Value.IsEnabled = True
            Payed.IsEnabled = True
            Done.IsEnabled = True
        End If

        CanceledDate.Visibility = Visibility.Hidden
        CanceledDate.SelectedDate = Nothing
        EmpIdCanceled.Visibility = Visibility.Hidden
        EmpIdCanceled.SelectedIndex = 0
    End Sub

    Private Sub Returned_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Returned.Unchecked

        If Not Canceled.IsChecked Then
            Returned.IsEnabled = True
            Canceled.IsEnabled = True

            Value.IsEnabled = True
            Payed.IsEnabled = True
            Done.IsEnabled = True
        End If

        ReturnedDate.Visibility = Visibility.Hidden
        ReturnedDate.SelectedDate = Nothing
        EmpIdReturned.Visibility = Visibility.Hidden
        EmpIdReturned.SelectedIndex = 0
    End Sub


    Private Sub btnPrintSchedule_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintSchedule.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@ClinicId"}
        rpt.paravalue = New String() {Val(ClinicId.SelectedValue)}
        rpt.Rpt = "Clinicschedule.rpt"
        rpt.Print()
    End Sub

    Private Sub btnPrintPatients_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintPatients.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@ClinicId", "@Daydate"}
        rpt.paravalue = New String() {Val(ClinicId.SelectedValue), Calendar1.SelectedDate.Value}
        rpt.Rpt = "PrintReservationsClinics.rpt"
        rpt.Print()
    End Sub

    Private Sub btnAddCase_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCase.Click
        Dim frm As New MyWindow With {.Title = "Patients", .WindowState = WindowState.Maximized}
        frm.Content = New Cases With {.IsSub = True}
        frm.Show()
    End Sub
End Class
