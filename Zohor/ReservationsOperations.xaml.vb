Imports System.Data
Imports System.ComponentModel

Public Class ReservationsOperations
    Dim bm As New BasicMethods
    Dim bf As New BasicForm
    Dim dtOperationsRooms As New DataTable

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSaveWithoutPrint}, {btnDelete}, {}, {btnSave})
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({CaseId, SurgeonId, SurgeonId2, SurgeonId3, AnesthetistId, NurseId})


        If Md.MyProjectType = ProjectType.X OrElse 1 = 1 Then
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
            'Canceled.Visibility = Visibility.Hidden
            Returned.Visibility = Visibility.Hidden
            LastSerialId.Visibility = Visibility.Hidden
            lblLastSerial.Visibility = Visibility.Hidden
            
            lblTime.Visibility = Visibility.Hidden
            lblTim.Visibility = Visibility.Hidden
        End If

        bm.FillCombo("select Id,Name from OperationsRooms union select 0 Id,'-' Name order by Name", OperationsRoomId)

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReservation)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdRemaining)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdCanceled)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReturned)

        bm.FillCombo("Hours", HH1, "", , True)
        bm.FillCombo("Minutes", MM1, "", , True)
        bm.FillCombo("HourIndex", Index1, "", , , True)
        bm.FillCombo("Hours", HH2, "", , True)
        bm.FillCombo("Minutes", MM2, "", , True)
        bm.FillCombo("HourIndex", Index2, "", , , True)

        dtOperationsRooms = bm.ExecuteAdapter("select Id,Name,dbo.LoadOperationsRoomschedule(Id) OperationsRoomschedule from OperationsRooms")
        LoadDoctors()
        bm.FillCombo("OperationTypes", OperationType, " where IsStopped=0")
        bm.FillCombo("OperationTypes", OperationType2, " where IsStopped=0")
        bm.FillCombo("OperationTypes", OperationType3, " where IsStopped=0")
        Calendar1.SelectedDate = bm.MyGetDate()
        LoadReservationsOperations()
        Done_Unchecked(Nothing, Nothing)
        Canceled_Unchecked(Nothing, Nothing)
        Returned_Unchecked(Nothing, Nothing)
        Home_Click(Nothing, Nothing)

        If Md.MyProjectType = ProjectType.X Then btnAddCase.Visibility = Visibility.Hidden
    End Sub

    Sub LoadDoctors()
        Try
            WR1.Children.Clear()
            For i As Integer = 0 To OperationsRoomId.Items.Count - 1
                If OperationsRoomId.Items(i)("Id").ToString = 0 Then Continue For
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & OperationsRoomId.Items(i)("Id").ToString
                x.Tag = OperationsRoomId.Items(i)("Id").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 115
                x.Height = 35
                x.Margin = New Thickness(5, 5, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = OperationsRoomId.Items(i)("Name").ToString.Replace(vbCrLf, " ")
                x.ToolTip = OperationsRoomId.Items(i)("Id").ToString & vbCrLf & x.Content & vbCrLf & vbCrLf & dtOperationsRooms.Select("Id=" & OperationsRoomId.Items(i)("Id").ToString)(0)("OperationsRoomschedule").ToString
                x.Background = btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR1.Children.Add(x)
                AddHandler x.Click, AddressOf btnOperationsRoomIdClick
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
        DayDate.Content = New DateTime(CInt(btn.Tag.ToString.Split(",")(0).Split("_")(0)), CInt(btn.Tag.ToString.Split(",")(0).Split("_")(1)), CInt(btn.Tag.ToString.Split(",")(0).Split("_")(2))).ToShortDateString()
        Dim dt As DataTable = bm.ExecuteAdapter("select * from ReservationsOperations where OperationsRoomId='" & OperationsRoomId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "' and ReservId='" & lblReservId.Content.ToString & "'")
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

            OperationType.SelectedValue = dt.Rows(0)("OperationType").ToString
            OperationType2.SelectedValue = dt.Rows(0)("OperationType2").ToString
            OperationType3.SelectedValue = dt.Rows(0)("OperationType3").ToString
            Value.Text = dt.Rows(0)("Value").ToString
            Payed.Text = dt.Rows(0)("Payed").ToString
            Remaining.Text = dt.Rows(0)("Remaining").ToString
            SerialId.Text = dt.Rows(0)("SerialId").ToString
            SerialId2.Text = dt.Rows(0)("SerialId2").ToString
            SurgeonId.Text = dt.Rows(0)("SurgeonId").ToString
            SurgeonId2.Text = dt.Rows(0)("SurgeonId2").ToString
            SurgeonId3.Text = dt.Rows(0)("SurgeonId3").ToString
            SurgeonId_LostFocus(Nothing, Nothing)
            SurgeonId2_LostFocus(Nothing, Nothing)
            SurgeonId3_LostFocus(Nothing, Nothing)
            AnesthetistId.Text = dt.Rows(0)("AnesthetistId").ToString
            AnesthetistId_LostFocus(Nothing, Nothing)
            NurseId.Text = dt.Rows(0)("NurseId").ToString
            NurseId_LostFocus(Nothing, Nothing)

            EmpIdReservation.SelectedValue = dt.Rows(0)("EmpIdReservation").ToString
            EmpIdRemaining.SelectedValue = dt.Rows(0)("EmpIdRemaining").ToString
            EmpIdCanceled.SelectedValue = dt.Rows(0)("EmpIdCanceled").ToString
            EmpIdReturned.SelectedValue = dt.Rows(0)("EmpIdReturned").ToString

            HH1.SelectedValue = dt.Rows(0)("HH1")
            MM1.SelectedValue = dt.Rows(0)("MM1")
            Index1.SelectedValue = dt.Rows(0)("Index1")
            HH2.SelectedValue = dt.Rows(0)("HH2")
            MM2.SelectedValue = dt.Rows(0)("MM2")
            Index2.SelectedValue = dt.Rows(0)("Index2")

            bm.SetPickerDate(CanceledDate, dt.Rows(0)("CanceledDate"))
            bm.SetPickerDate(ReturnedDate, dt.Rows(0)("ReturnedDate"))
            bm.SetPickerDate(RemainingDate, dt.Rows(0)("RemainingDate"))

            If Val(dt.Rows(0)("Posted").ToString) = 1 Then
                btnSave.Visibility = Visibility.Hidden
                btnSaveWithoutPrint.Visibility = Visibility.Hidden
                btnDelete.Visibility = Visibility.Hidden
            End If

            OperationType.IsEnabled = False
            OperationType2.IsEnabled = False
            OperationType3.IsEnabled = False

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

    Private Sub OperationsRoomId_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles OperationsRoomId.SelectionChanged
        LoadReservationsOperations()
    End Sub

    Private Sub Calendar1_SelectedDatesChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles Calendar1.SelectedDatesChanged
        LoadReservationsOperations()
    End Sub

    Private Sub ViewByMonth_Checked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles ViewByWeek.Checked, ViewByMonth.Checked
        LoadReservationsOperations()
    End Sub


    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp
        If bm.ShowHelpCases(CaseId, CaseName, e, False, False) Then
            CaseID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select " & Resources.Item("CboName") & " Name from Cases where Id=" & CaseId.Text.Trim())
        If Val(SurgeonId.Text) = 0 Then
            bm.LostFocus(CaseId, SurgeonId, "select DoctorId Name from Cases where Id=" & CaseId.Text.Trim())
            SurgeonId_LostFocus(Nothing, Nothing)
        End If
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile from Cases where Id=" & CaseId.Text.Trim())
        If dt.Rows.Count > 0 Then
            CaseId.ToolTip = Resources.Item("Id") & ": " & CaseId.Text & vbCrLf & Resources.Item("Name") & ": " & CaseName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
            CaseName.ToolTip = CaseId.ToolTip
        End If
    End Sub

    Private Sub SurgeonId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SurgeonId.KeyUp
        If bm.ShowHelp("Doctors", SurgeonId, SurgeonName, e, "select cast(Id as varchar(100)) Id,Name from Employees where Doctor=1") Then
            SurgeonId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub SurgeonId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SurgeonId.LostFocus
        bm.LostFocus(SurgeonId, SurgeonName, "select Name from Employees where Doctor=1 and Id=" & SurgeonId.Text.Trim())
    End Sub

    Private Sub SurgeonId2_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SurgeonId2.KeyUp
        If bm.ShowHelp("Doctors", SurgeonId2, SurgeonName2, e, "select cast(Id as varchar(100)) Id,Name from Employees where Doctor=1") Then
            SurgeonId2_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub SurgeonId2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SurgeonId2.LostFocus
        bm.LostFocus(SurgeonId2, SurgeonName2, "select Name from Employees where Doctor=1 and Id=" & SurgeonId2.Text.Trim())
    End Sub

    Private Sub SurgeonId3_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SurgeonId3.KeyUp
        If bm.ShowHelp("Doctors", SurgeonId3, SurgeonName3, e, "select cast(Id as varchar(100)) Id,Name from Employees where Doctor=1") Then
            SurgeonId3_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub SurgeonId3_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SurgeonId3.LostFocus
        bm.LostFocus(SurgeonId3, SurgeonName3, "select Name from Employees where Doctor=1 and Id=" & SurgeonId3.Text.Trim())
    End Sub

    Private Sub AnesthetistId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AnesthetistId.KeyUp
        If bm.ShowHelp("Doctors", AnesthetistId, AnesthetistName, e, "select cast(Id as varchar(100)) Id,Name from Employees where Doctor=1") Then
            AnesthetistId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub AnesthetistId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AnesthetistId.LostFocus
        bm.LostFocus(AnesthetistId, AnesthetistName, "select Name from Employees where Doctor=1 and Id=" & AnesthetistId.Text.Trim())
    End Sub

    Private Sub NurseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles NurseId.KeyUp
        If bm.ShowHelp("Nurses", NurseId, NurseName, e, "select cast(Id as varchar(100)) Id,Name from Employees where Nurse=1") Then
            NurseId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub NurseId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles NurseId.LostFocus
        bm.LostFocus(NurseId, NurseName, "select Name from Employees where Nurse=1 and Id=" & NurseId.Text.Trim())
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

        OperationType.SelectedIndex = 0
        OperationType2.SelectedIndex = 0
        OperationType3.SelectedIndex = 0

        Value.Clear()
        Payed.Clear()
        Remaining.Clear()
        SerialId.Clear()
        SerialId2.Clear()
        SurgeonId.Clear()
        SurgeonId2.Clear()
        SurgeonId3.Clear()
        SurgeonName.Clear()
        SurgeonName2.Clear()
        SurgeonName3.Clear()
        AnesthetistId.Clear()
        AnesthetistName.Clear()
        NurseId.Clear()
        NurseName.Clear()
        Done.IsChecked = False

        HH1.SelectedIndex = 0
        MM1.SelectedIndex = 0
        Index1.SelectedIndex = 0
        HH2.SelectedIndex = 0
        MM2.SelectedIndex = 0
        Index2.SelectedIndex = 0

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
        OperationType.IsEnabled = True
        OperationType2.IsEnabled = True
        OperationType3.IsEnabled = True
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
            str = "insert ReservationsOperations(OperationsRoomId,DayDate,ReservId,Time,CaseId,CaseName,OperationType,OperationType2,OperationType3,SurgeonId,SurgeonId2,SurgeonId3,AnesthetistId,NurseId,Value,Payed,Remaining,SerialId,UserName,MyGetDate,ReservationDate,Done,Canceled,Returned,RemainingDate,EmpIdReservation,EmpIdRemaining,EmpIdCanceled,CanceledDate,EmpIdReturned,ReturnedDate,HH1,MM1,Index1,HH2,MM2,Index2) select '" & OperationsRoomId.SelectedValue.ToString & "','" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "','" & lblReservId.Content.ToString & "','" & lblTime.Content.ToString & "','" & CaseId.Text & "','" & CaseName.Text & "','" & OperationType.SelectedValue.ToString & "','" & OperationType2.SelectedValue.ToString & "','" & OperationType3.SelectedValue.ToString & "','" & Val(SurgeonId.Text) & "','" & Val(SurgeonId2.Text) & "','" & Val(SurgeonId3.Text) & "','" & Val(AnesthetistId.Text) & "','" & Val(NurseId.Text) & "','" & Val(Value.Text) & "','" & Val(Payed.Text) & "','" & Val(Remaining.Text) & "','" & SerialId.Text & "','" & Md.UserName & "',GetDate(),GetDate(),'" & bm.IIf(Done.IsChecked, 1, 0) & "','" & bm.IIf(Canceled.IsChecked, 1, 0) & "','" & bm.IIf(Returned.IsChecked, 1, 0) & "','" & bm.ToStrDate(RemainingDate.SelectedDate) & "','" & EmpIdReservation.SelectedValue.ToString & "','" & EmpIdRemaining.SelectedValue.ToString & "','" & EmpIdCanceled.SelectedValue.ToString & "','" & bm.ToStrDate(CanceledDate.SelectedDate) & "','" & EmpIdReturned.SelectedValue.ToString & "','" & bm.ToStrDate(ReturnedDate.SelectedDate) & "'," & HH1.SelectedValue & "," & MM1.SelectedValue & "," & Index1.SelectedValue & "," & HH2.SelectedValue & "," & MM2.SelectedValue & "," & Index2.SelectedValue
        Else
            str = "update ReservationsOperations set CaseId='" & CaseId.Text & "',CaseName='" & CaseName.Text & "',OperationType='" & OperationType.SelectedValue.ToString & "',OperationType2='" & OperationType2.SelectedValue.ToString & "',OperationType3='" & OperationType3.SelectedValue.ToString & "',SurgeonId='" & Val(SurgeonId.Text) & "',SurgeonId2='" & Val(SurgeonId2.Text) & "',SurgeonId3='" & Val(SurgeonId3.Text) & "',AnesthetistId='" & Val(AnesthetistId.Text) & "',NurseId='" & Val(NurseId.Text) & "',Value='" & Val(Value.Text) & "',Payed='" & Val(Payed.Text) & "',Remaining='" & Val(Remaining.Text) & "',Done='" & bm.IIf(Done.IsChecked, 1, 0) & "',Canceled='" & bm.IIf(Canceled.IsChecked, 1, 0) & "',Returned='" & bm.IIf(Returned.IsChecked, 1, 0) & "',RemainingDate='" & bm.ToStrDate(RemainingDate.SelectedDate) & "',SerialId='" & SerialId.Text & "',EmpIdReservation='" & EmpIdReservation.SelectedValue.ToString & "',EmpIdRemaining='" & EmpIdRemaining.SelectedValue.ToString & "',EmpIdCanceled='" & EmpIdCanceled.SelectedValue.ToString & "',CanceledDate='" & bm.ToStrDate(CanceledDate.SelectedDate) & "',EmpIdReturned='" & EmpIdReturned.SelectedValue.ToString & "',ReturnedDate='" & bm.ToStrDate(ReturnedDate.SelectedDate) & "',UserName='" & Md.UserName & "',MyGetDate=GetDate(),HH1=" & HH1.SelectedValue & ",MM1=" & MM1.SelectedValue & ",Index1=" & Index1.SelectedValue & ",HH2=" & HH2.SelectedValue & ",MM2=" & MM2.SelectedValue & ",Index2=" & Index2.SelectedValue & " where OperationsRoomId='" & OperationsRoomId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "' and ReservId='" & lblReservId.Content.ToString & "'"
        End If


        If Not bm.ExecuteNonQuery(str) Then
            bm.ShowMSG("لم يتم الحفظ")
            If State = BasicMethods.SaveState.Insert Then
                SerialId.Text = ""
                LastSerialId.Text = ""
            End If
            Return
        End If


        If Not Canceled.IsChecked AndAlso Not Returned.IsChecked AndAlso sender Is btnSave Then ShowRPT()

        bm.ExecuteNonQuery("update Cases set InOut=1 where Id=" & CaseId.Text.Trim() & "     insert CaseStatus(CaseId,UserName,MyGetDate,InOut) select " & Val(CaseId.Text) & "," & Md.UserName & ",GetDate()," & 1)

        GenerateOperationMotion()

        Home_Click(Nothing, Nothing)
        btnNew_Click(Nothing, Nothing)
        LoadReservationsOperations()
    End Sub

    Function Delete() As Boolean
        If OperationsRoomId.SelectedIndex < 1 OrElse DayDate.Content.ToString = "" OrElse lblReservId.Content.ToString = "" Then
            Return False
        End If

        Return bm.ExecuteNonQuery("delete ReservationsOperations where OperationsRoomId='" & OperationsRoomId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DateTime.Parse(DayDate.Content.ToString)) & "' and ReservId='" & lblReservId.Content.ToString & "'")
    End Function

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Not Delete() Then Return
            btnNew_Click(Nothing, Nothing)
            LoadReservationsOperations()
        End If
    End Sub

    Function Valid() As Boolean
        If OperationsRoomId.SelectedIndex < 1 OrElse DayDate.Content.ToString = "" OrElse lblReservId.Content.ToString = "" Then
            Return False
        End If

        If CaseId.Text.Trim = "" Then
            bm.ShowMSG("Please, Select a Patient")
            CaseId.Focus()
            Return False
        End If
        If OperationType.SelectedIndex < 1 Then
            bm.ShowMSG("Please, Select a Operation Type")
            OperationType.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub btnClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Calendar1.SelectedDate = CType(sender, Button).Content.ToString.Substring(0, 10)

        Dim ToId, ToName As New TextBox

        bm.ShowHelp("", ToId, ToName, New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.F1), "select cast(ReservId as varchar(100)) Id,CaseName+' - '+c.Mobile+' - '+c.HomePhone Name from ReservationsOperations r left join Cases c on(r.CaseId=c.Id) where r.OperationsRoomId='" & OperationsRoomId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(Calendar1.SelectedDate) & "'")


        Dim s As String = ("Reservation_" & CType(sender, Button).Content.ToString.Substring(0, 10) & "_" & ToId.Text).Replace("/", "_").Replace(" ", "_").Replace(":", "_").Replace(vbCrLf, "_")

        For Each b As Button In WR.Children
            If b.Name = s Then
                btnReservClick(b, Nothing)
                Exit Sub
            End If
        Next
    End Sub

    Sub LoadReservationsOperations()
        Try
            WR.Children.Clear()
            btnNew_Click(Nothing, Nothing)
            If OperationsRoomId.SelectedIndex < 1 Then Return

            Dim dt As DataTable
            If ViewByWeek.IsChecked Then
                dt = bm.ExecuteAdapter("WeekFirstDay", New String() {"DayDate"}, New String() {bm.ToStrDate(Calendar1.SelectedDate.Value)})
            Else
                dt = bm.ExecuteAdapter("GetAllDaysInMonth", New String() {"Year", "Month"}, New String() {Calendar1.SelectedDate.Value.Year, Calendar1.SelectedDate.Value.Month})
            End If

            Dim DaysDt As DataTable = bm.ExecuteAdapter("select Saturday,Sunday,Monday,Tuesday,Wednesday,Thursday,Friday from OperationsRooms where Id=" & OperationsRoomId.SelectedValue.ToString)
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

                Dim dt2 As DataTable = bm.ExecuteAdapter("LoadReservationsOperations", New String() {"OperationsRoomId", "Daydate"}, New String() {Val(OperationsRoomId.SelectedValue.ToString), bm.ToStrDate(bm.FormatDate(dt.Rows(i)("Date")))})
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
                    x2.Content = dt2.Rows(i2)("Id").ToString '& vbCrLf & dt2.Rows(i2)("Time").ToString
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

    Private Sub btnOperationsRoomIdClick(sender As Object, e As RoutedEventArgs)
        OperationsRoomId.SelectedValue = sender.Tag
        OperationsRoomId_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub ShowRPT()
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@OperationsRoomId", "@Date", "@Id", "Header"}
        rpt.paravalue = New String() {Val(OperationsRoomId.SelectedValue), DayDate.Content, Val(lblReservId.Content), CType(Parent, Page).Title}
        rpt.Rpt = "ReservationsOperationsONE.rpt"
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

        TabItemReservationsOperations.SetResourceReference(TabItem.HeaderProperty, "ReservationsOperations")
        TabItemCurrentReservation.SetResourceReference(TabItem.HeaderProperty, "Current Reservation")

        lblOperationsRoomId.SetResourceReference(ContentProperty, "OperationsRoom")
        ViewByWeek.SetResourceReference(ContentProperty, "View By Week")
        ViewByMonth.SetResourceReference(ContentProperty, "View By Month")
        Canceled.SetResourceReference(CheckBox.ContentProperty, "Cancel")
        Returned.SetResourceReference(CheckBox.ContentProperty, "Returned")
        Done.SetResourceReference(CheckBox.ContentProperty, "Done Remaining")

        lblDate.SetResourceReference(ContentProperty, "Date")
        lblReserv.SetResourceReference(ContentProperty, "Reserve")
        lblTim.SetResourceReference(ContentProperty, "Time")
        lblPatient.SetResourceReference(ContentProperty, "Patient")
        lblOperationType.SetResourceReference(ContentProperty, "OperationType")
        lblValue.SetResourceReference(ContentProperty, "Value")
        lblPayed.SetResourceReference(ContentProperty, "Payed")
        lblRemaining.SetResourceReference(ContentProperty, "Remaining")
        lblSerial.SetResourceReference(ContentProperty, "Serial")
        lblSerialId2.SetResourceReference(ContentProperty, "Serial")
        lblLastSerial.SetResourceReference(ContentProperty, "LastSerial")
        'btnPrintSchedule.SetResourceReference(ContentProperty, "Print Schedule")
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
        'rpt.paraname = New String() {"@OperationsRoomId"}
        'rpt.paravalue = New String() {Val(OperationsRoomId.SelectedValue)}
        'rpt.Rpt = "OperationsRoomschedule.rpt"
        rpt.paraname = New String() {"Header"}
        rpt.paravalue = New String() {CType(Parent, Page).Title}
        rpt.Rpt = "OperationsRoomsAll.rpt"
        rpt.Show()
    End Sub

    Private Sub btnPrintPatients_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintPatients.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@OperationsRoomId", "@Daydate"}
        rpt.paravalue = New String() {Val(OperationsRoomId.SelectedValue), Calendar1.SelectedDate.Value}
        rpt.Rpt = "PrintReservationsOperations.rpt"
        rpt.Show()
    End Sub

    Private Sub btnAddCase_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCase.Click
        Dim frm As New MyWindow With {.Title = "Patients", .WindowState = WindowState.Maximized}
        frm.Content = New Cases4 With {.MyId = Val(CaseId.Text)}
        frm.Show()
    End Sub

    Private Sub GenerateOperationMotion()
        'Dim frm As New OperationMotions
        'frm.OperationMotions_Loaded(Nothing, Nothing)
        'frm.RoomId.SelectedValue = RoomId

        'frm.OperationTypeId, "")
        'frm.OperationTypeId2, "")
        'frm.OperationTypeId3, "")

        'frm.DrId1, " where Doctor=1")
        'frm.DrId2, " where Doctor=1")
        'frm.DrId3, " where Doctor=1")
        'frm.AnesthetistId, " where Doctor=1")

    End Sub

End Class
