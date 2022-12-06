Imports System.Data
Imports System.ComponentModel

Public Class Reservations
    Dim bm As New BasicMethods
    Dim bf As New BasicForm
    Dim dtDoctors As New DataTable

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSaveWithoutPrint}, {btnDelete}, {}, {btnSave})
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({CaseId, RefereId})
        CurrentShift.Visibility = Visibility.Hidden
        InsertedDate.Visibility = Visibility.Hidden
        IsClosed.Visibility = Visibility.Hidden

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReservation)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdRemaining)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdCanceled)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReturned)


        dtDoctors = bm.ExecuteAdapter("select Id," & Resources.Item("CboName") & " Name,dbo.LoadEmpschedule(Id) Empschedule from Employees where Doctor='1' and Stopped='0'")
        LoadDepartments()
        LoadDoctors(0)
        bm.FillCombo("VisitingTypes", VisitingType, "")
        bm.FillCombo("Companies", CompanyId, "")
        Calendar1.SelectedDate = bm.MyGetDate()
        LoadReservations()
        Done_Unchecked(Nothing, Nothing)
        Canceled_Unchecked(Nothing, Nothing)
        Returned_Unchecked(Nothing, Nothing)
        Home_Click(Nothing, Nothing)

        If EmpId.Items.Count = 2 Then
            EmpId.SelectedIndex = 1
        End If

        If Md.MyProjectType = ProjectType.X Then btnAddCase.Visibility = Visibility.Hidden

        If Not Md.MyProjectType = ProjectType.X Then
            lblRefereId.Visibility = Visibility.Hidden
            RefereId.Visibility = Visibility.Hidden
            RefereName.Visibility = Visibility.Hidden
            lblPerc1.Visibility = Visibility.Hidden
            lblPerc2.Visibility = Visibility.Hidden
            Perc.Visibility = Visibility.Hidden
        End If
    End Sub

    Sub LoadDepartments()
        Try
            WR0.Children.Clear()
            Dim dt As DataTable = bm.ExecuteAdapter("select 0 Id,'كل التخصصات' Name,0 OrderBy from Departments union select Id,Name,1 OrderBy from Departments Order By OrderBy,Name")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "D" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 80
                x.Height = 35
                x.Margin = New Thickness(5, 5, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Background = btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR0.Children.Add(x)
                AddHandler x.Click, AddressOf btnDeptClick
            Next
        Catch ex As Exception
        End Try
    End Sub

    Sub LoadDoctors(DepartmentId As Integer)
        Try

            bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Doctor='1' and Stopped='0' and (DepartmentId=" & DepartmentId & " or " & DepartmentId & "=0) union select 0 Id,'-' Name order by Name", EmpId)

            dtDoctors = bm.ExecuteAdapter("select Id," & Resources.Item("CboName") & " Name,dbo.LoadEmpschedule(Id) Empschedule from Employees where Doctor='1' and Stopped='0' and (DepartmentId=" & DepartmentId & " or " & DepartmentId & "=0)")

            WR1.Children.Clear()
            For i As Integer = 0 To EmpId.Items.Count - 1
                If EmpId.Items(i)("Id").ToString = 0 Then Continue For
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & EmpId.Items(i)("Id").ToString
                x.Tag = EmpId.Items(i)("Id").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 115
                x.Height = 35
                x.Margin = New Thickness(5, 5, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = EmpId.Items(i)("Name").ToString.Replace(vbCrLf, " ")
                x.ToolTip = EmpId.Items(i)("Id").ToString & vbCrLf & x.Content & vbCrLf & vbCrLf & dtDoctors.Select("Id=" & EmpId.Items(i)("Id").ToString)(0)("Empschedule").ToString
                x.Background = btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR1.Children.Add(x)
                AddHandler x.Click, AddressOf btnEmpIdClick
            Next
        Catch ex As Exception
        End Try
    End Sub

    Dim IsNew As Boolean = False
    Private Sub btnReservClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Clear()
        Dim btn As Button = sender
        lblReservId.Content = btn.Tag.ToString.Split(",")(1)
        lblTime.Content = btn.Tag.ToString.Split(",")(2).Replace(vbCrLf, " ")
        DayDate.Content = New DateTime(CInt(btn.Tag.ToString.Split(",")(0).Split("_")(0)), CInt(btn.Tag.ToString.Split(",")(0).Split("_")(1)), CInt(btn.Tag.ToString.Split(",")(0).Split("_")(2))).ToShortDateString()
        Dim dt As DataTable = bm.ExecuteAdapter("select * from Reservations where EmpId='" & EmpId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "' and ReservId='" & lblReservId.Content.ToString & "'")
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

            RefereId.Text = dt.Rows(0)("RefereId").ToString
            RefereId_LostFocus(Nothing, Nothing)

            MyNotes.Text = dt.Rows(0)("MyNotes").ToString

            VisitingType.SelectedValue = dt.Rows(0)("VisitingType").ToString
            Value.Text = dt.Rows(0)("Value").ToString
            Perc.Text = dt.Rows(0)("Perc").ToString
            Payed.Text = dt.Rows(0)("Payed").ToString
            Remaining.Text = dt.Rows(0)("Remaining").ToString
            SerialId.Text = dt.Rows(0)("SerialId").ToString
            SerialId2.Text = dt.Rows(0)("SerialId2").ToString

            EmpIdReservation.SelectedValue = dt.Rows(0)("EmpIdReservation").ToString
            EmpIdRemaining.SelectedValue = dt.Rows(0)("EmpIdRemaining").ToString
            EmpIdCanceled.SelectedValue = dt.Rows(0)("EmpIdCanceled").ToString
            EmpIdReturned.SelectedValue = dt.Rows(0)("EmpIdReturned").ToString

            CurrentShift.Text = dt.Rows(0)("CurrentShift").ToString
            If Not dt.Rows(0)("InsertedDate").ToString = "" Then InsertedDate.Text = bm.ToStrDateTimeFormated(dt.Rows(0)("InsertedDate"))
            IsClosed.IsChecked = bm.IIf(dt.Rows(0)("IsClosed") = 1, True, False)

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

        'If DateTime.Parse(DayDate.Content.ToString) = bm.MyGetDate() Then
        '    Returned.IsEnabled = False
        'Else
        '    Canceled.IsEnabled = False
        'End If

        CaseId.Focus()
    End Sub

    Private Sub EmpId_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles EmpId.SelectionChanged
        LoadReservations()
    End Sub

    Private Sub Calendar1_SelectedDatesChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles Calendar1.SelectedDatesChanged
        LoadReservations()
    End Sub

    Private Sub ViewByMonth_Checked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles ViewByWeek.Checked, ViewByMonth.Checked
        LoadReservations()
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
        CompanyId.SelectedIndex = 0
        Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile,CompanyId from Cases where Id=" & CaseId.Text.Trim())
        If dt.Rows.Count > 0 Then
            CompanyId.SelectedValue = Val(dt.Rows(0)("CompanyId").ToString)
            If CompanyId.SelectedValue Is Nothing Then
                CompanyId.SelectedValue = 0
            End If
            CaseId.ToolTip = Resources.Item("Id") & ": " & CaseId.Text & vbCrLf & Resources.Item("Name") & ": " & CaseName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
            CaseName.ToolTip = CaseId.ToolTip
        End If
        VisitingType_SelectionChanged(Nothing, Nothing)
    End Sub


    Private Sub RefereId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles RefereId.KeyUp
        If bm.ShowHelp("ExternalDoctors", RefereId, RefereName, e, "select cast(Id as varchar(100)) Id,Name from ExternalDoctors") Then
            RefereId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub RefereId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RefereId.LostFocus
        bm.LostFocus(RefereId, RefereName, "select Name from ExternalDoctors where Id=" & RefereId.Text.Trim())
        VisitingType_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub Value_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Value.TextChanged, Payed.TextChanged
        Remaining.Text = Val(Value.Text) - Val(Payed.Text)
    End Sub

    Private Sub Perc_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Perc.TextChanged
        VisitingType_SelectionChanged(Nothing, Nothing)
    End Sub


    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown, Payed.KeyDown, Remaining.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNew.Click
        Clear()
        LoadDoctors(0)
    End Sub

    Sub Clear()
        If btnSave Is Nothing Then Return
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
        RefereId.Clear()
        RefereName.Clear()

        CompanyId.SelectedIndex = 0
        MyNotes.Clear()

        VisitingType.SelectedIndex = 0
        Value.Clear()
        Perc.Clear()
        Payed.Clear()
        Remaining.Clear()
        SerialId.Clear()
        SerialId2.Clear()
        Done.IsChecked = False


        CurrentShift.Clear()
        InsertedDate.Clear()
        IsClosed.IsChecked = False

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

        If Not Md.MyProjectType = ProjectType.Zohor AndAlso Val(SerialId.Text) > 0 And Val(Payed.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المدفوع")
            Payed.Focus()
            Return
        End If

        If Val(SerialId2.Text) > 0 And Val(Remaining.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المتبقي")
            Remaining.Focus()
            Return
        End If

        If InsertedDate.Text.Trim = "" Then InsertedDate.Text = bm.ExecuteScalar("select dbo.MyGetDateTime()")
        If InsertedDate.Text.Trim = "" Then Return

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If SerialId.Text = "" Then State = BasicMethods.SaveState.Insert

        Dim str As String
        If IsNew Then
            If bm.IF_Exists("select * from Reservations where EmpId='" & EmpId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "' and ReservId='" & lblReservId.Content.ToString & "'") Then
                bm.ShowMSG("لم يتم الحفظ " & vbCrLf & " برجاء التحديث وإعادة المحاولة")
                If bm.ShowDeleteMSG("هل تريد التحديث؟") Then
                    EmpId_SelectionChanged(Nothing, Nothing)
                End If
                Return
            End If

            str = "insert Reservations(EmpId,DayDate,ReservId,Time,CaseId,CaseName,RefereId,CompanyId,MyNotes,VisitingType,Value,Perc,Payed,Remaining,SerialId,UserName,MyGetDate,ReservationDate,Done,Canceled,Returned,RemainingDate,EmpIdReservation,EmpIdRemaining,EmpIdCanceled,CanceledDate,EmpIdReturned,ReturnedDate,CurrentShift,InsertedDate,IsClosed) select '" & EmpId.SelectedValue.ToString & "','" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "','" & lblReservId.Content.ToString & "','" & lblTime.Content.ToString & "','" & CaseId.Text & "','" & CaseName.Text & "','" & Val(RefereId.Text) & "','" & CompanyId.SelectedValue.ToString & "','" & MyNotes.Text.Trim.Replace("'", "''") & "','" & VisitingType.SelectedValue.ToString & "','" & Val(Value.Text) & "','" & Val(Perc.Text) & "','" & Val(Payed.Text) & "','" & Val(Remaining.Text) & "','" & SerialId.Text & "','" & Md.UserName & "',GetDate(),GetDate(),'" & bm.IIf(Done.IsChecked, 1, 0) & "','" & bm.IIf(Canceled.IsChecked, 1, 0) & "','" & bm.IIf(Returned.IsChecked, 1, 0) & "','" & bm.ToStrDate(RemainingDate.SelectedDate) & "','" & EmpIdReservation.SelectedValue.ToString & "','" & EmpIdRemaining.SelectedValue.ToString & "','" & EmpIdCanceled.SelectedValue.ToString & "','" & bm.ToStrDate(CanceledDate.SelectedDate) & "','" & EmpIdReturned.SelectedValue.ToString & "','" & bm.ToStrDate(ReturnedDate.SelectedDate) & "','" & CurrentShift.Text & "','" & InsertedDate.Text & "','" & bm.IIf(IsClosed.IsChecked, 1, 0) & "'"
        Else
            str = "update Reservations set CaseId='" & CaseId.Text & "',CaseName='" & CaseName.Text & "',RefereId='" & Val(RefereId.Text) & "',CompanyId='" & CompanyId.SelectedValue.ToString & "',MyNotes='" & MyNotes.Text.Trim.Replace("'", "''") & "',VisitingType='" & VisitingType.SelectedValue.ToString & "',Value='" & Val(Value.Text) & "',Perc='" & Val(Perc.Text) & "',Payed='" & Val(Payed.Text) & "',Remaining='" & Val(Remaining.Text) & "',Done='" & bm.IIf(Done.IsChecked, 1, 0) & "',Canceled='" & bm.IIf(Canceled.IsChecked, 1, 0) & "',Returned='" & bm.IIf(Returned.IsChecked, 1, 0) & "',RemainingDate='" & bm.ToStrDate(RemainingDate.SelectedDate) & "',SerialId='" & SerialId.Text & "',EmpIdReservation='" & EmpIdReservation.SelectedValue.ToString & "',EmpIdRemaining='" & EmpIdRemaining.SelectedValue.ToString & "',EmpIdCanceled='" & EmpIdCanceled.SelectedValue.ToString & "',CanceledDate='" & bm.ToStrDate(CanceledDate.SelectedDate) & "',EmpIdReturned='" & EmpIdReturned.SelectedValue.ToString & "',ReturnedDate='" & bm.ToStrDate(ReturnedDate.SelectedDate) & "',CurrentShift='" & CurrentShift.Text & "',InsertedDate='" & InsertedDate.Text & "',IsClosed='" & bm.IIf(IsClosed.IsChecked, 1, 0) & "',UserName='" & Md.UserName & "',MyGetDate=GetDate() where EmpId='" & EmpId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "' and ReservId='" & lblReservId.Content.ToString & "'"
        End If


        If btnSaveWithoutPrint.IsEnabled Then

            If Not bm.ExecuteNonQuery(str) Then
                bm.ShowMSG("لم يتم الحفظ")
                If State = BasicMethods.SaveState.Insert Then
                    SerialId.Text = ""
                    LastSerialId.Text = ""
                End If
                LoadReservations()
                Return
            End If


            If Val(SerialId.Text) = 0 Then
                If Md.MyProjectType = ProjectType.Zohor Then
                    SerialId.Text = bm.ExecuteScalar("updateReservationsSerialIdCo", {"VisitingType", "EmpId", "DayDate", "ReservId"}, {VisitingType.SelectedValue.ToString, EmpId.SelectedValue.ToString, bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)), lblReservId.Content.ToString})
                ElseIf Val(Payed.Text) > 0 Then
                    SerialId.Text = bm.ExecuteScalar("updateReservationsSerialId", {"VisitingType", "EmpId", "DayDate", "ReservId"}, {VisitingType.SelectedValue.ToString, EmpId.SelectedValue.ToString, bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)), lblReservId.Content.ToString})
                End If
                LastSerialId.Text = SerialId.Text
            End If

            If SerialId2.IsVisible AndAlso Val(SerialId2.Text) = 0 Then
                If Md.MyProjectType = ProjectType.Zohor Then
                    SerialId2.Text = bm.ExecuteScalar("updateReservationsSerialIdCo2", {"VisitingType", "EmpId", "DayDate", "ReservId"}, {VisitingType.SelectedValue.ToString, EmpId.SelectedValue.ToString, bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)), lblReservId.Content.ToString})
                ElseIf Val(Remaining.Text) > 0 Then
                    SerialId2.Text = bm.ExecuteScalar("updateReservationsSerialId2", {"VisitingType", "EmpId", "DayDate", "ReservId"}, {VisitingType.SelectedValue.ToString, EmpId.SelectedValue.ToString, bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)), lblReservId.Content.ToString})
                End If
                LastSerialId.Text = SerialId2.Text
            End If

            If sender Is btnSave Then
                bm.ExecuteNonQuery("update Cases set InOut=1 where Id=" & CaseId.Text.Trim() & "     insert CaseStatus(CaseId,UserName,MyGetDate,InOut) select " & Val(CaseId.Text) & "," & Md.UserName & ",GetDate()," & 1)
            End If

        End If

        TraceInvoice(State.ToString)


        If Not MyProjectType = ProjectType.X AndAlso Not MyProjectType = ProjectType.X AndAlso Not Canceled.IsChecked AndAlso Not Returned.IsChecked AndAlso sender Is btnSave Then ShowRPT()

        Home_Click(Nothing, Nothing)
        Clear()
        LoadReservations()
    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExecuteNonQuery("BeforeDeleteReservations", New String() {"EmpId", "DayDate", "ReservId", "UserDelete", "State"}, New String() {EmpId.SelectedValue.ToString, bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)), lblReservId.Content.ToString, Md.UserName, State})
    End Sub

    Function Delete() As Boolean
        If EmpId.SelectedIndex < 1 OrElse DayDate.Content.ToString = "" OrElse lblReservId.Content.ToString = "" Then
            Return False
        End If

        Return bm.ExecuteNonQuery("delete Reservations where EmpId='" & EmpId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "' and ReservId='" & lblReservId.Content.ToString & "'")
    End Function

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Not Delete() Then Return
            Clear()
            LoadReservations()
        End If
    End Sub

    Function Valid() As Boolean
        If EmpId.SelectedIndex < 1 OrElse DayDate.Content.ToString = "" OrElse lblReservId.Content.ToString = "" Then
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

        bm.ShowHelp("", ToId, ToName, New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.F1), "select cast(ReservId as varchar(100)) Id,CaseName+' - '+isnull(c.Mobile,'')+' - '+isnull(c.HomePhone,'') Name from Reservations r left join Cases c on(r.CaseId=c.Id) where r.EmpId='" & EmpId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(Calendar1.SelectedDate) & "'")


        Dim s As String = ("Reservation_" & CType(sender, Button).Content.ToString.Substring(0, 10) & "_" & ToId.Text).Replace("/", "_").Replace(" ", "_").Replace(":", "_").Replace(vbCrLf, "_")

        For Each b As Button In WR.Children
            If b.Name = s Then
                btnReservClick(b, Nothing)
                Exit Sub
            End If
        Next
    End Sub

    Sub LoadReservations()
        Try
            WR.Children.Clear()
            Clear()
            If EmpId.SelectedIndex < 1 Then Return

            Dim dt As DataTable
            If ViewByWeek.IsChecked Then
                dt = bm.ExecuteAdapter("WeekFirstDay", New String() {"DayDate"}, New String() {bm.ToStrDate(Calendar1.SelectedDate.Value)})
            Else
                dt = bm.ExecuteAdapter("GetAllDaysInMonth", New String() {"Year", "Month"}, New String() {Calendar1.SelectedDate.Value.Year, Calendar1.SelectedDate.Value.Month})
            End If

            Dim DaysDt As DataTable = bm.ExecuteAdapter("select Saturday,Sunday,Monday,Tuesday,Wednesday,Thursday,Friday from Employees where Id=" & EmpId.SelectedValue.ToString)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "lbl_" & dt.Rows(i)("Line").ToString
                'x.Tag = dt.Rows(i)("Date").Substring(0, 10)
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

                Dim dt2 As DataTable = bm.ExecuteAdapter("LoadReservations", New String() {"EmpId", "Daydate"}, New String() {Val(EmpId.SelectedValue.ToString), bm.ToStrDate(bm.FormatDate(dt.Rows(i)("Date")))})
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
                    'If dt2.Rows(i2)("IsExists") = 1 Then
                    '    Select Case dt2.Rows(i2)("VisitingType")
                    '        Case 1
                    '            x2.Background = bf.btnSave.Background
                    '        Case 2
                    '            x2.Background = bf.btnFirst.Background
                    '        Case 3
                    '            x2.Background = bf.btnDelete.Background
                    '    End Select
                    '    x2.Background = bf.btnSave.Background
                    '    x2.Foreground = System.Windows.Media.Brushes.Black
                    'ElseIf dt2.Rows(i2)("IsExists") = 2 Then
                    '    x2.Background = bf.btnDelete.Background
                    '    x2.Foreground = System.Windows.Media.Brushes.Black
                    'End If
                    If dt2.Rows(i2)("IsExists") = 1 OrElse dt2.Rows(i2)("IsExists") = 2 Then
                        Select Case dt2.Rows(i2)("VisitingType")
                            Case 1
                                x2.Background = bf.btnSave.Background
                            Case 2
                                x2.Background = bf.btnDelete.Background
                            Case Else
                                x2.Background = System.Windows.Media.Brushes.Red
                        End Select
                        x2.Foreground = System.Windows.Media.Brushes.Black
                    End If
                    If Val(dt2.Rows(i2)("Canceled").ToString) = 1 OrElse Val(dt2.Rows(i2)("Returned").ToString) = 1 Then
                        x2.Background = System.Windows.Media.Brushes.Silver
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
            If Not VisitingType.IsEnabled Then Return
            Value.Text = Val(bm.ExecuteScalar("select Price from VisitingTypeEmployees where EmpId='" & EmpId.SelectedValue.ToString & "' and VisitingTypeId='" & VisitingType.SelectedValue.ToString & "'")) * (1 - Val(Perc.Text) / 100)
            'Value.Text = Val(bm.ExecuteScalar("select isnull(C." & s & ",E." & s & ") from Employees E left join DoctorsCompanies C on(C.DoctorId=E.Id and C.CompanyId=(select T.CompanyId from Cases T where T.Id=" & Val(CaseId.Text) & ")) where E.Id='" & EmpId.SelectedValue.ToString & "'"))
            Payed.Text = Value.Text
            CompanyId.ToolTip = Nothing
            If CompanyId.SelectedIndex > 0 Then
                Dim dt As DataTable = bm.ExecuteAdapter("GetDoctorsCompaniesData", {"CompanyId", "VisitingTypeId", "DoctorId"}, {CompanyId.SelectedValue.ToString, VisitingType.SelectedValue, EmpId.SelectedValue})
                Value.Text = (dt.Rows(0)("Price").ToString) * (1 - Val(Perc.Text) / 100)
                Payed.Text = dt.Rows(0)("Payed").ToString
                CompanyId.ToolTip = dt.Rows(0)("Notes").ToString
                bm.ShowMsgOnScreen(dt.Rows(0)("Notes").ToString)
            ElseIf Val(RefereId.Text) > 0 Then
                Dim DescPerc As Decimal = Val(bm.ExecuteScalar("select DescPerc from ExternalDoctors where Id=" & RefereId.Text.Trim() & " and Type=0"))
                Value.Text = Val(Value.Text) * (100 - DescPerc) / 100
                Payed.Text = Value.Text
            End If
        Catch
        End Try
    End Sub

    Private Sub btnEmpIdClick(sender As Object, e As RoutedEventArgs)
        EmpId.SelectedValue = sender.Tag
        EmpId_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub ShowRPT()
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@EmpId", "@Date", "@Id", "Header", "IsNew"}
        rpt.paravalue = New String() {Val(EmpId.SelectedValue), DayDate.Content, Val(lblReservId.Content), CType(Parent, Page).Title, IIf(IsNew, 1, 0)}
        rpt.Rpt = "ReservationONE.rpt"
        If Md.MyProjectType = ProjectType.Zohor Then
            rpt.Rpt = "ReservationONEZohor.rpt"
        End If
        rpt.Print(".", Md.PonePrinter, 1)
    End Sub


    Private Sub LoadResource()
        btnSaveWithoutPrint.SetResourceReference(ContentProperty, "Save")
        btnSave.SetResourceReference(ContentProperty, "Print")

        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")

        Home.SetResourceReference(ContentProperty, "Today")

        TabItemReservations.SetResourceReference(TabItem.HeaderProperty, "Reservations")
        TabItemCurrentReservation.SetResourceReference(TabItem.HeaderProperty, "Current Reservation")

        lblEmpId.SetResourceReference(ContentProperty, "Doctor")
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
        btnPrintSchedule.SetResourceReference(ContentProperty, "Print Schedule")
        btnPrintPatients.SetResourceReference(ContentProperty, "Print Patients")
        btnPrintPatientsWithTel.SetResourceReference(ContentProperty, "Print Patients With Tel")

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
            If Not Done.IsChecked Then Done.IsEnabled = True
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

            'If DayDate.Content.ToString <> "" AndAlso DateTime.Parse(DayDate.Content.ToString) <> bm.MyGetDate() AndAlso Not Md.Manager Then
            '    Canceled.IsEnabled = False
            'Else
            '    Canceled.IsEnabled = True
            'End If

            Value.IsEnabled = True
            Payed.IsEnabled = True
            If Not Done.IsChecked Then Done.IsEnabled = True
        End If

        ReturnedDate.Visibility = Visibility.Hidden
        ReturnedDate.SelectedDate = Nothing
        EmpIdReturned.Visibility = Visibility.Hidden
        EmpIdReturned.SelectedIndex = 0
    End Sub


    Private Sub btnPrintSchedule_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintSchedule.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@EmpId"}
        rpt.paravalue = New String() {Val(EmpId.SelectedValue)}
        rpt.Rpt = "Empschedule.rpt"
        rpt.Print()
    End Sub

    Private Sub btnPrintPatients_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintPatients.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@EmpId", "@Daydate"}
        rpt.paravalue = New String() {Val(EmpId.SelectedValue), Calendar1.SelectedDate.Value}
        rpt.Rpt = "PrintReservations.rpt"
        rpt.Print()
    End Sub

    Private Sub btnPrintPatientsWithTel_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintPatientsWithTel.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@EmpId", "@Daydate"}
        rpt.paravalue = New String() {Val(EmpId.SelectedValue), Calendar1.SelectedDate.Value}
        rpt.Rpt = "PrintReservationsWithTel.rpt"
        rpt.Print()
    End Sub

    Private Sub btnAddCase_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCase.Click
        Dim frm As New MyWindow With {.Title = "Patients", .WindowState = WindowState.Maximized}
        frm.Content = New Cases4 With {.MyId = Val(CaseId.Text)}
        frm.MySecurityType.AllowEdit = True
        frm.Show()
    End Sub

    Private Sub btnDeptClick(sender As Object, e As RoutedEventArgs)
        LoadDoctors(sender.Tag)
    End Sub

    Private Sub CurrentShift_TextChanged(sender As Object, e As TextChangedEventArgs) Handles CurrentShift.TextChanged
        If Not Md.MyProjectType = ProjectType.Zohor Then Return
        If Val(CurrentShift.Text) > 0 Then
            btnSaveWithoutPrint.IsEnabled = Md.Manager
        Else
            btnSaveWithoutPrint.IsEnabled = True
        End If
    End Sub

    Private Sub ViewHistory_Click(sender As Object, e As RoutedEventArgs) Handles ViewHistory.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@CaseId", "CaseName", "@Flag", "@MainId", "@DayDate", "@Id", "Header"}
        rpt.paravalue = New String() {Val(CaseId.Text), CaseName.Text, -2, 0, bm.ToStrDate(Now.Date), 0, "Patient History"}
        rpt.Rpt = "CaseAllDetails.rpt"
        rpt.Show()
    End Sub

    Private Sub btnDisplay_Click(sender As Object, e As RoutedEventArgs) Handles btnDisplay.Click
        Dim frm As New Display With {.Title = "Display", .WindowState = WindowState.Maximized}
        frm.DisplayCaseId = Val(lblReservId.Content)
        frm.DisplayCaseName = CaseName.Text
        frm.ShowDialog()
    End Sub

End Class
