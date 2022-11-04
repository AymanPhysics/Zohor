Imports System.Data
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class ReservationsRooms
    Dim bm As New BasicMethods
    Dim bf As New BasicForm
    WithEvents G As New MyGrid
    Dim dt As DataTable

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({SurgeonId, CaseId})

        bm.FillCombo("OperationTypes", OperationType, " where IsStopped=0")
        bm.FillCombo("InpatientDepartments", InpatientDepartmentId, "")


        LoadReservationsRooms()

        If Md.MyProjectType = ProjectType.X Then btnAddCase.Visibility = Visibility.Hidden

        LoadWFH()

        If Md.MyProjectType = ProjectType.Zohor Then
            btnDoctorInstruction.Visibility = Visibility.Hidden
            btnInpatientList.Visibility = Visibility.Hidden

            'OperationType.Visibility = Visibility.Hidden
            'lblOperationType.Visibility = Visibility.Hidden

            lblDiagnoseOnAdmission.Content = "ملاحظات"
            lblOperationType.Content = "العملية"
            lblSurgeon.Content = "الطبيب"

            'If Not Md.Manager Then btnExit.Visibility = Visibility.Hidden
        Else
            Grid1Sub.Visibility = Visibility.Hidden
        End If
    End Sub

    Structure GC
        Shared Value As String = "Value"
        Shared UserNameId As String = "UserNameId"
        Shared UserFullName As String = "UserFullName"
        Shared MyGetDate As String = "MyGetDate"
        Shared CurrentShift As String = "CurrentShift"
        Shared IsClosed As String = "IsClosed"
        Shared Line As String = "Line"
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Value, "المبلغ")
        G.Columns.Add(GC.UserNameId, "UserNameId")
        G.Columns.Add(GC.UserFullName, "الموظف")
        G.Columns.Add(GC.MyGetDate, "التاريخ والوقت")
        G.Columns.Add(GC.CurrentShift, "CurrentShift")
        G.Columns.Add(GC.IsClosed, "IsClosed")
        G.Columns.Add(GC.Line, "السيريال")
        G.Columns.Add(GC.Notes, "البيان")

        G.Columns(GC.UserNameId).Visible = False
        G.Columns(GC.CurrentShift).Visible = False
        G.Columns(GC.IsClosed).Visible = False
        'G.Columns(GC.Line).Visible = False

        G.Columns(GC.Value).Width = 20
        G.Columns(GC.UserFullName).Width = 30

        G.Columns(GC.Line).ReadOnly = True
        G.Columns(GC.UserFullName).ReadOnly = True
        G.Columns(GC.MyGetDate).ReadOnly = True

        AddHandler G.CellEndEdit, AddressOf G_CellEndEdit
    End Sub

    Private Sub G_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        If Val(G.CurrentRow.Cells(GC.CurrentShift).Value) = 0 Then
            G.CurrentRow.Cells(GC.UserNameId).Value = Md.UserName
            G.CurrentRow.Cells(GC.UserFullName).Value = Md.ArName
            G.CurrentRow.Cells(GC.CurrentShift).Value = Val(bm.ExecuteScalar("select CurrentShift from Employees where Id='" & Md.UserName & "'"))
        End If
        CalcTotal()
    End Sub

    Dim IsNew As Boolean = False
    Dim MyLine As Integer = 0
    Private Sub btnReservClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If IsMoving Then
            MoveItem(sender)
            Return
        End If

        btnNew_Click(Nothing, Nothing)
        Dim btn As Controls.Button = sender
        lblRoomId.Content = btn.Name.Split("_")(1)
        lblId.Content = btn.Name.Split("_")(2)
        DayDate.SelectedDate = bm.MyGetDate
        ExitDate.SelectedDate = bm.MyGetDate
        lblTime.Content = Now.TimeOfDay.ToString.Substring(0, 5)
        CaseID_LostFocus(Nothing, Nothing)

        dt = bm.ExecuteAdapter("select * from RoomsData where RoomId='" & CType(sender, Controls.Button).Name.ToString.Split("_")(1) & "' and Id='" & CType(sender, Controls.Button).Name.ToString.Split("_")(2) & "' and IsCurrent=1")
        EmpIdReservation.SelectedValue = Md.UserName
        IsNew = True
        If dt.Rows.Count > 0 Then
            IsNew = False

            DayDate.SelectedDate = dt.Rows(0)("DayDate").ToString
            ExitDate.SelectedDate = dt.Rows(0)("ExitDate").ToString
            lblTime.Content = dt.Rows(0)("Time").ToString

            CaseId.Text = dt.Rows(0)("CaseId").ToString
            CaseID_LostFocus(Nothing, Nothing)
            CaseId.IsEnabled = Md.Manager

            OperationType.SelectedValue = dt.Rows(0)("OperationType").ToString
            SurgeonId.Text = dt.Rows(0)("SurgeonId").ToString
            SurgeonId_LostFocus(Nothing, Nothing)

            GetRoomsDataPayments(CType(sender, Controls.Button).Name.ToString.Split("_")(1), CType(sender, Controls.Button).Name.ToString.Split("_")(2))

            If Not Md.Manager Then
                CaseId.IsEnabled = Md.Manager
            End If
        End If
        CaseId.Focus()
    End Sub

    Sub GetRoomsDataPayments(MyRoomId As String, MyId As String)

        Dim MyCurrentShift As Integer = Val(bm.ExecuteScalar("select CurrentShift from Employees where Id='" & Md.UserName & "'"))

        dt = bm.ExecuteAdapter("select * from RoomsDataPayments where RoomId='" & MyRoomId & "' and Id='" & MyId & "' and IsCurrent=1")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)(GC.Value)
            G.Rows(i).Cells(GC.UserNameId).Value = dt.Rows(i)(GC.UserNameId)
            G.Rows(i).Cells(GC.UserFullName).Value = dt.Rows(i)(GC.UserFullName)
            G.Rows(i).Cells(GC.CurrentShift).Value = dt.Rows(i)(GC.CurrentShift)
            G.Rows(i).Cells(GC.IsClosed).Value = dt.Rows(i)(GC.IsClosed)
            G.Rows(i).Cells(GC.MyGetDate).Value = bm.ToStrDateTimeFormated(dt.Rows(i)(GC.MyGetDate))
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)(GC.Line)
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)(GC.Notes)
            If MyCurrentShift <> dt.Rows(i)(GC.CurrentShift) Then
                G.Rows(i).ReadOnly = True
            End If
            G.CurrentCell = G.Rows(i).Cells(GC.UserFullName)
        Next

        CalcTotal()

    End Sub

    Sub CalcTotal()
        Total.Clear()
        Dim x As Decimal = 0
        For i As Integer = 0 To G.Rows.Count - 1
            x += Val(G.Rows(i).Cells(GC.Value).Value)
        Next
        Total.Text = x
    End Sub

    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp
        If bm.ShowHelpCases(CaseId, CaseName, e, False, False) Then
            CaseID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select Code+' - '+" & Resources.Item("CboName") & " Name from Cases where Id=" & CaseId.Text.Trim())
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        DateOfBirth.SelectedDate = Nothing
        DiagnoseOnAdmission.Text = ""
        Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile,DateOfBirth,DiagnoseOnAdmission from Cases where Id=" & CaseId.Text.Trim())
        If dt.Rows.Count > 0 Then
            CaseId.ToolTip = Resources.Item("Id") & ": " & CaseId.Text & vbCrLf & Resources.Item("Name") & ": " & CaseName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
            CaseName.ToolTip = CaseId.ToolTip
            DateOfBirth.SelectedDate = IIf(dt.Rows(0)("DateOfBirth").ToString = "", Nothing, dt.Rows(0)("DateOfBirth"))
            DiagnoseOnAdmission.Text = dt.Rows(0)("DiagnoseOnAdmission").ToString
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNew.Click
        lblTime.Content = ""

        G.Rows.Clear()

        lblRoomId.Content = ""
        lblId.Content = ""

        CaseId.Clear()
        CaseName.Clear()
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        CaseID_LostFocus(Nothing, Nothing)
        SurgeonId.Clear()
        SurgeonId_LostFocus(Nothing, Nothing)


        DayDate.SelectedDate = bm.MyGetDate
        ExitDate.SelectedDate = bm.MyGetDate

        EmpIdReservation.SelectedIndex = 0
        CaseId.IsEnabled = True

        OperationType.SelectedIndex = 0

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click, btnSaveWithoutPrint.Click
        Try
            If Val(CaseId.Text) = 0 Then
                Return
            End If
            'If Val(OperationType.SelectedValue) = 0 Then
            '    bm.ShowMSG("برجاء تحديد نوع العملية")
            '    Return
            'End If

            Dim Cnt As Integer = Val(bm.ExecuteScalar("select count(RoomId) from ReservationsRooms where RoomId='" & lblRoomId.Content.ToString & "' and DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "' and CaseId<>'" & CaseId.Text.ToString & "'"))
            If Cnt > 0 Then
                If Not bm.ShowDeleteMSG("يوجد عدد " & Cnt & " حجوزات أخرى اليوم لنفس الغرفة. هل تريد الاستمرار؟") Then
                    Return
                End If
            End If


            Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update

            Dim str = "delete RoomsData where RoomId='" & lblRoomId.Content.ToString & "' and Id='" & lblId.Content.ToString & "' and IsCurrent=1  insert RoomsData(RoomId,Id,CaseId,CaseName,DayDate,ExitDate,IsCurrent,Time,UserName,MyGetDate,OperationType,SurgeonId) select '" & lblRoomId.Content.ToString & "','" & lblId.Content.ToString & "','" & CaseId.Text.ToString & "','" & CaseName.Text.ToString & "','" & bm.ToStrDate(DayDate.SelectedDate) & "','" & bm.ToStrDate(ExitDate.SelectedDate) & "',1,'" & lblTime.Content.ToString & "'," & Md.UserName & ",GetDate()," & Val(OperationType.SelectedValue) & "," & Val(SurgeonId.Text)

            If Not bm.ExecuteNonQuery(str) Then
                bm.ShowMSG("لم يتم الحفظ")
                Return
            End If

            bm.ExecuteNonQuery("update Cases set InOut=1,DiagnoseOnAdmission='" & DiagnoseOnAdmission.Text.Replace("'", "''") & "' where Id=" & CaseId.Text.Trim() & "     insert CaseStatus(CaseId,UserName,MyGetDate,InOut) select " & Val(CaseId.Text) & "," & Md.UserName & ",GetDate()," & 1)




            G.EndEdit()



            If Not bm.SaveGrid(G, "RoomsDataPayments", New String() {"RoomId", "Id", "IsCurrent"}, New String() {lblRoomId.Content.ToString, lblId.Content.ToString, 1}, New String() {"Value", "UserNameId", "UserFullName", "CurrentShift", "Notes", "IsClosed"}, New String() {GC.Value, GC.UserNameId, GC.UserFullName, GC.CurrentShift, GC.Notes, GC.IsClosed}, New VariantType() {VariantType.Decimal, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.String, VariantType.Integer}, New String() {GC.Value}, "Line", "Line") Then Return

            MyLine = Val(bm.ExecuteScalar("select MyLine from RoomsData where RoomId='" & lblRoomId.Content.ToString & "' and Id='" & lblId.Content.ToString & "' and IsCurrent=1  "))
            If MyLine = 0 Then Return
            bm.ExecuteNonQuery("update RoomsDataPayments set MyMainLine=" & MyLine & " where RoomId='" & lblRoomId.Content.ToString & "' and Id='" & lblId.Content.ToString & "' and IsCurrent=1  ")




            If Md.MyProjectType = ProjectType.Zohor Then
                For i As Integer = 0 To G.Rows.Count - 1
                    Try
                        If G.Rows(i).Cells(GC.Value).Value > 0 AndAlso G.Rows(i).Cells(GC.Line).Value = 0 Then
                            PrintOne()
                            Exit For
                        End If
                    Catch ex As Exception
                    End Try
                Next
            End If


            btnNew_Click(Nothing, Nothing)
            LoadReservationsRooms()
        Catch
        End Try
    End Sub

    Function Delete() As Boolean
        If lblId.Content.ToString = "" Then
            Return False
        End If

        Return bm.ExecuteNonQuery("delete RoomsData where RoomId='" & lblRoomId.Content.ToString & "' and Id='" & lblId.Content.ToString & "'")
    End Function

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Not Delete() Then Return
            btnNew_Click(Nothing, Nothing)
            LoadReservationsRooms()
        End If
    End Sub

    Private Sub btnClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim ToId, ToName As New Controls.TextBox

        bm.ShowHelp("", ToId, ToName, New Input.KeyEventArgs(Nothing, Nothing, Nothing, Key.F1), "select cast(r.Id as varchar(100)) Id,Name+' - '+c.Mobile+' - '+c.HomePhone Name from RoomsData r left join Cases c on(r.CaseId=c.Id) where r.RoomId='" & CType(sender, Button).Name.Split("_")(1) & "' and IsCurrent=1")



        Dim s As String = "lbl_" & CType(sender, Button).Name.Split("_")(1) & "_" & ToId.Text
        For Each b As Button In WR.Children
            If b.Name = s Then
                btnReservClick(b, Nothing)
                Exit Sub
            End If
        Next
    End Sub

    Sub LoadReservationsRooms()
        Try
            WR.Children.Clear()
            btnNew_Click(Nothing, Nothing)

            Dim dt As DataTable = bm.ExecuteAdapter("select Id,Name,Cnt from Rooms where (InpatientDepartmentId='" & Val(InpatientDepartmentId.SelectedValue) & "' or '" & Val(InpatientDepartmentId.SelectedValue) & "'=0) and (InpatientDepartmentsSubId='" & Val(InpatientDepartmentSubId.SelectedValue) & "' or '" & Val(InpatientDepartmentSubId.SelectedValue) & "'=0)")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Controls.Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "lbl_" & dt.Rows(i)("Id").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 160
                x.Height = 50
                x.Margin = New Thickness(10, 60 + i * 60, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = dt.Rows(i)("Name")
                x.ToolTip = x.Content
                x.Background = bf.btnNew.Background 'System.Windows.Media.Brushes.SkyBlue
                x.Foreground = System.Windows.Media.Brushes.Black
                AddHandler x.Click, AddressOf btnClick
                WR.Children.Add(x)

                Dim dt2 As DataTable = bm.ExecuteAdapter("select * from RoomsData where RoomId=" & dt.Rows(i)("Id").ToString & " and IsCurrent=1")
                Dim dt3 As DataTable = bm.ExecuteAdapter("select * from RoomsNames where MainId=" & dt.Rows(i)("Id").ToString)
                For i2 As Integer = 0 To Val(dt.Rows(i)("Cnt")) - 1
                    Dim x2 As New Controls.Button
                    x2.Style = Application.Current.FindResource("GlossyCloseButton")
                    x2.FontSize -= 1
                    x2.Name = "lbl_" & dt.Rows(i)("Id").ToString & "_" & (i2 + 1).ToString
                    x2.Content = i2 + 1
                    If dt3.Select("Id=" & (i2 + 1).ToString).Length > 0 Then
                        x2.Content &= vbCrLf & dt3.Rows(i2)("Name")
                    End If
                    x2.Tag = x2.Name
                    x2.Width = 50
                    x2.Height = 50
                    x2.Margin = New Thickness(180 + i2 * 60, 60 + i * 60, 0, 0)
                    x2.HorizontalAlignment = HorizontalAlignment.Left
                    x2.VerticalAlignment = VerticalAlignment.Top
                    x2.Cursor = Input.Cursors.Pen
                    x2.ToolTip = x2.Content
                    x2.Background = bf.btnNew.Background 'System.Windows.Media.Brushes.Blue
                    x2.Foreground = System.Windows.Media.Brushes.Black
                    If dt2.Select("Id=" & (i2 + 1).ToString).Length > 0 Then
                        x2.Background = bf.btnSave.Background
                        x2.Foreground = System.Windows.Media.Brushes.Black
                    End If
                    WR.Children.Add(x2)
                    AddHandler x2.Click, AddressOf btnReservClick
                Next
            Next

        Catch ex As Exception
        End Try

    End Sub

    Private Sub LoadResource()
        btnSaveWithoutPrint.SetResourceReference(ContentProperty, "Save")
        btnSave.SetResourceReference(ContentProperty, "Print")
        btnExit.SetResourceReference(ContentProperty, "Patient Leaving")

        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        TabItemCurrentReservation.SetResourceReference(TabItem.HeaderProperty, "Current Room")

        lblDate.SetResourceReference(ContentProperty, "Date")
        lblReserv.SetResourceReference(ContentProperty, "Room")
        lblTim.SetResourceReference(ContentProperty, "Time")
        lblPatient.SetResourceReference(ContentProperty, "Patient")

    End Sub

    Private Sub btnExit_Click(sender As Object, e As RoutedEventArgs) Handles btnExit.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من خروج المريض؟") Then
            If Not bm.ExecuteNonQuery("ExitCase", {"UserName", "RoomId", "Id"}, {Md.UserName, lblRoomId.Content.ToString, lblId.Content.ToString}) Then
                bm.ShowMSG("لم يتم الحفظ")
                Return
            End If
            'btnAddCase_Click(Nothing, Nothing)
            LoadReservationsRooms()
        End If
    End Sub

    Private Sub btnDoctorInstruction_Click(sender As Object, e As RoutedEventArgs) Handles btnDoctorInstruction.Click
        Dim frm As New MyWindow With {.Title = "Doctor Instruction", .WindowState = WindowState.Maximized}
        Dim c As New DoctorInstruction
        c.MyCase = Val(CaseId.Text)
        c.MyCaseName = CaseName.Text
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnInpatientList_Click(sender As Object, e As RoutedEventArgs) Handles btnInpatientList.Click
        Dim frm As New MyWindow With {.Title = "Inpatient List", .WindowState = WindowState.Maximized}
        Dim c As New ReservationDetailsRooms
        c.MyRoomId = Val(lblRoomId.Content)
        c.MyId = Val(lblId.Content)
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnAddCase_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCase.Click
        Dim frm As New MyWindow With {.Title = "Patients", .WindowState = WindowState.Maximized}
        frm.Content = New Cases4 With {.MyId = Val(CaseId.Text)}
        frm.Show()
    End Sub

    Dim lopD As Boolean = False
    Private Sub DateOfBirth_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DateOfBirth.SelectedDateChanged
        If lopD Then Return
        lopD = True
        Try
            D.Clear()
            M.Clear()
            Y.Clear()
            Dim DOB As Date = New DateTime(DateOfBirth.SelectedDate.Value.Year, DateOfBirth.SelectedDate.Value.Month, DateOfBirth.SelectedDate.Value.Day)
            Dim tday As TimeSpan = DateTime.Now.Subtract(DOB)
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
        lopD = False
    End Sub
    Private Sub DMY_SelectedDateChanged(sender As Object, e As TextChangedEventArgs) Handles D.TextChanged, M.TextChanged, Y.TextChanged
        If lopD Then Return
        lopD = True
        Try
            DateOfBirth.SelectedDate = DateAdd(DateInterval.Day, -Val(D.Text), DateAdd(DateInterval.Month, -Val(M.Text), DateAdd(DateInterval.Year, -Val(Y.Text), Now.Date)))
        Catch ex As Exception
        End Try
        lopD = False
    End Sub

    Dim IsMoving As Boolean = False
    Private Sub btnMove_Click(sender As Object, e As RoutedEventArgs) Handles btnMove.Click
        IsMoving = True
        Dim dt As DataTable = bm.ExecuteAdapter("select RoomId,Id from RoomsData where IsCurrent=1")
        For Each btn As Controls.Button In WR.Children
            If btn.Name.Length = btn.Name.Replace("_", "").Length + 1 Then
                btn.IsEnabled = False
            Else
                If dt.Select("RoomId=" & btn.Name.ToString.Split("_")(1) & " and Id=" & btn.Name.ToString.Split("_")(2)).Length > 0 Then
                    btn.IsEnabled = False
                End If
            End If
        Next
    End Sub

    Private Sub MoveItem(ByVal sender As Button)
        bm.ExecuteNonQuery("update RoomsData set IsCurrent=0,ExitDate=getdate() where RoomId='" & lblRoomId.Content.ToString & "' and Id='" & lblId.Content.ToString & "' update ReservationCbo0Rooms set IsCurrent=0 where RoomId='" & lblRoomId.Content.ToString & "' and ReservId='" & lblId.Content.ToString & "' update CaseAttachments2Rooms set IsCurrent=0 where RoomId='" & lblRoomId.Content.ToString & "' and ReservId='" & lblId.Content.ToString & "' ")

        lblRoomId.Content = sender.Name.ToString.Split("_")(1)
        lblId.Content = sender.Name.ToString.Split("_")(2)
        'DayDate.SelectedDate = bm.MyGetDate
        'ExitDate.SelectedDate = bm.MyGetDate
        'lblTime.Content = Now.TimeOfDay.ToString.Substring(0, 5)
        btnSave_Click(Nothing, Nothing)

        IsMoving = False
        LoadReservationsRooms()
        btnReservClick(sender, Nothing)
    End Sub

    Private Sub btnCancle_Click(sender As Object, e As RoutedEventArgs) Handles btnCancle.Click
        IsMoving = False
        LoadReservationsRooms()
    End Sub

    Private Sub SurgeonId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SurgeonId.KeyUp
        If bm.ShowHelp("Doctors", SurgeonId, SurgeonName, e, "select cast(Id as varchar(100)) Id,Name from Employees where Doctor=1") Then
            SurgeonId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub SurgeonId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SurgeonId.LostFocus
        bm.LostFocus(SurgeonId, SurgeonName, "select Name from Employees where Doctor=1 and Id=" & SurgeonId.Text.Trim())
    End Sub

    Private Sub btnPrintAll_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintAll.Click, btnPrintAll2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"Header"}
        rpt.paravalue = New String() {CType(Parent, Page).Title}
        rpt.Rpt = "RoomsDataAll.rpt"
        If Md.MyProjectType = ProjectType.Zohor Then
            rpt.Rpt = "RoomsDataAll_Z.rpt"
            If sender Is btnPrintAll2 Then
                rpt.Rpt = "RoomsDataAll_Z2.rpt"
            End If
        End If
        rpt.Show()
    End Sub

    Private Sub btnPrintOne_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintOne.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@RoomId", "@Id", "Header"}
        rpt.paravalue = New String() {Val(lblRoomId.Content), Val(lblId.Content), CType(Parent, Page).Title}
        rpt.Rpt = "RoomsDataOne.rpt"
        rpt.Show()
    End Sub


    Private Sub PrintOne()
        Dim rpt As New ReportViewer

        Dim MyCurrentShift As Integer = Val(bm.ExecuteScalar("select CurrentShift from Employees where Id='" & Md.UserName & "'"))

        rpt.paraname = New String() {"@RoomId", "@Id", "@CurrentShift", "@UserNameId", "Header"}
        rpt.paravalue = New String() {Val(lblRoomId.Content), Val(lblId.Content), MyCurrentShift, Md.UserName, CType(Parent, Page).Title}
        rpt.Rpt = "RoomsDataOne3.rpt"
        rpt.Print(".", Md.PonePrinter, 1)
    End Sub

    Private Sub btnPrintOne2_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintOne2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@RoomId", "@Id", "Header"}
        rpt.paravalue = New String() {Val(lblRoomId.Content), Val(lblId.Content), CType(Parent, Page).Title}
        rpt.Rpt = "RoomsDataOne2.rpt"
        rpt.Show()
    End Sub

    Private Sub InpatientDepartmentId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles InpatientDepartmentId.SelectionChanged
        Try
            bm.FillCombo("InpatientDepartmentsSub", InpatientDepartmentSubId, " where InpatientDepartmentId=" & Val(InpatientDepartmentId.SelectedValue))
            LoadReservationsRooms()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub InpatientDepartmentSubId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles InpatientDepartmentSubId.SelectionChanged
        Try
            LoadReservationsRooms()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        If G.CurrentRow Is Nothing OrElse G.CurrentRow.Cells(GC.Line).Value Is Nothing Then
            bm.ShowMSG("برجاء تحديد السيريال")
            Return
        End If

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Line", "Header"}
        rpt.paravalue = New String() {G.CurrentRow.Cells(GC.Line).Value, CType(Parent, Page).Title}
        rpt.Rpt = "RoomsDataOne4.rpt"
        rpt.Print(".", Md.PonePrinter, 1)

    End Sub
End Class
