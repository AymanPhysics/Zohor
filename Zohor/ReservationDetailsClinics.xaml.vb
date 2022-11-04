Imports System.Data
Imports System.ComponentModel
Imports System.IO

Public Class ReservationDetailsClinics
    Dim bm As New BasicMethods
    Dim bf As New BasicForm

    WithEvents G0 As New MyGrid
    WithEvents G1 As New MyGrid
    WithEvents G2 As New MyGrid
    WithEvents G3 As New MyGrid
    WithEvents G4 As New MyGrid
    WithEvents G5 As New MyGrid
    WithEvents G6 As New MyGrid

    Dim WithEvents BackgroundWorker1 As New BackgroundWorker
    Dim t As New Forms.Timer With {.Interval = 1500}
    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        TabControl3.SelectedIndex = 0
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {}, {btnPrint})


        If Val(bm.ExecuteScalar("select Doctor from Employees where Id=" & Md.UserName)) = 0 Then
            AimOfTheDay.IsEnabled = False
            DatePicker1.IsEnabled = False
            Add3.IsEnabled = False
            btnDelete6.IsEnabled = False
            btnReNew.IsEnabled = False
        End If

        If Md.MyProjectType = ProjectType.X Then
            Value.Visibility = Visibility.Hidden
            Payed.Visibility = Visibility.Hidden
            Remaining.Visibility = Visibility.Hidden
            lblValue.Visibility = Visibility.Hidden
            lblPayed.Visibility = Visibility.Hidden
            lblRemaining.Visibility = Visibility.Hidden
        End If

        LoadResource()
        For i As Integer = 0 To TabControl3.Items.Count - 1
            TabControl3.SelectedIndex = i
        Next
        TabControl3.SelectedIndex = 0

        bm.FillCombo("VisitingTypes", VisitingType, "")
        t_Tick(Nothing, Nothing)
        'AddHandler t.Tick, AddressOf t_Tick
        't.Start()

        TabItem08.Visibility = Visibility.Collapsed
        TabItem09.Visibility = Visibility.Collapsed
        TabItem10.Visibility = Visibility.Collapsed
        TabItem11.Visibility = Visibility.Collapsed

    End Sub

    Structure GC0
        Shared Notes As String = "Notes"
    End Structure

    Private Sub LoadWFH0()
        WFH0.Child = G0

        G0.Columns.Clear()
        G0.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G0.ForeColor = System.Drawing.Color.DarkBlue
        G0.ColumnHeadersVisible = False

        G0.Columns.Add(GC0.Notes, "")
        G0.Columns(GC0.Notes).FillWeight = 100

        G0.AllowUserToDeleteRows = True
        G0.EditMode = Forms.DataGridViewEditMode.EditOnEnter
    End Sub

    Structure GC1
        Shared Notes As String = "Notes"
    End Structure

    Private Sub LoadWFH1()
        WFH1.Child = G1

        G1.Columns.Clear()
        G1.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G1.ForeColor = System.Drawing.Color.DarkBlue
        G1.ColumnHeadersVisible = False

        G1.Columns.Add(GC1.Notes, "")
        G1.Columns(GC1.Notes).FillWeight = 100

        G1.AllowUserToDeleteRows = True
        G1.EditMode = Forms.DataGridViewEditMode.EditOnEnter
    End Sub

    Structure GC2
        Shared Notes As String = "Notes"
    End Structure

    Private Sub LoadWFH2()
        WFH2.Child = G2

        G2.Columns.Clear()
        G2.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G2.ForeColor = System.Drawing.Color.DarkBlue
        G2.ColumnHeadersVisible = False

        G2.Columns.Add(GC2.Notes, "")
        G2.Columns(GC2.Notes).FillWeight = 100

        G2.AllowUserToDeleteRows = True
        G2.EditMode = Forms.DataGridViewEditMode.EditOnEnter
    End Sub

    Structure GC3
        Shared Notes1 As String = "Notes1"
        Shared Notes2 As String = "Notes2"
    End Structure

    Private Sub LoadWFH3()
        WFH3.Child = G3

        G3.Columns.Clear()
        G3.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G3.ForeColor = System.Drawing.Color.DarkBlue
        G3.ColumnHeadersVisible = False

        G3.Columns.Add(GC3.Notes1, "")
        G3.Columns.Add(GC3.Notes2, "")
        G3.Columns(GC3.Notes1).FillWeight = 100
        G3.Columns(GC3.Notes2).FillWeight = 200
        G3.Columns(GC3.Notes2).CellTemplate.Style.Alignment = Forms.DataGridViewContentAlignment.MiddleRight

        G3.AllowUserToDeleteRows = True
        G3.EditMode = Forms.DataGridViewEditMode.EditOnEnter
    End Sub

    Structure GC4
        Shared Notes As String = "Notes"
    End Structure

    Private Sub LoadWFH4()
        WFH4.Child = G4

        G4.Columns.Clear()
        G4.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G4.ForeColor = System.Drawing.Color.DarkBlue
        G4.ColumnHeadersVisible = False

        G4.Columns.Add(GC4.Notes, "")
        G4.Columns(GC4.Notes).FillWeight = 100

        G4.AllowUserToDeleteRows = True
        G4.EditMode = Forms.DataGridViewEditMode.EditOnEnter
    End Sub

    Structure GC5
        Shared Notes1 As String = "Notes1"
        Shared Notes2 As String = "Notes2"
    End Structure

    Private Sub LoadWFH5()
        WFH5.Child = G5

        G5.Columns.Clear()
        G5.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G5.ForeColor = System.Drawing.Color.DarkBlue
        G5.ColumnHeadersVisible = False

        G5.Columns.Add(GC5.Notes1, "")
        G5.Columns.Add(GC5.Notes2, "")
        G5.Columns(GC5.Notes1).FillWeight = 100
        G5.Columns(GC5.Notes2).FillWeight = 200

        G5.AllowUserToDeleteRows = True
        G5.EditMode = Forms.DataGridViewEditMode.EditOnEnter
    End Sub

    Structure GC6
        Shared DoctorId As String = "DoctorId"
        Shared Time As String = "Time"
        Shared Problem As String = "Problem"
        Shared CasePlan As String = "CasePlan"
        Shared Note As String = "Note"
    End Structure


    Private Sub LoadWFH6()
        WFH6.Child = G6
        G6.AllowUserToAddRows = False

        G6.Columns.Clear()
        G6.ForeColor = System.Drawing.Color.DarkBlue
        G6.Columns.Add(GC6.DoctorId, "Doctor")
        G6.Columns.Add(GC6.Time, "Time")
        G6.Columns.Add(GC6.Problem, "Problem")
        G6.Columns.Add(GC6.CasePlan, "Plan")
        G6.Columns.Add(GC6.Note, "Note")


        If Val(bm.ExecuteScalar("select Doctor from Employees where Id=" & Md.UserName)) = 0 Then
            G6.Columns(GC6.Problem).ReadOnly = True
            G6.Columns(GC6.CasePlan).ReadOnly = True
            G6.Columns(GC6.Note).ReadOnly = True
        End If
        G6.Columns(GC6.DoctorId).ReadOnly = True
        G6.Columns(GC6.Time).ReadOnly = True

        G6.Columns(GC6.Time).FillWeight = 50
    End Sub



    Private Sub btnReservClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        btnNew_Click(Nothing, Nothing)
        Dim btn As Button = sender
        lblReservId.Content = btn.Tag.ToString
        lblTime.Content = btn.Content.ToString.Split(vbCrLf)(0)
        Dim dt As DataTable = bm.ExecuteAdapter("select * from ReservationsClinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        If dt.Rows.Count > 0 Then
            lblTime.Content = dt.Rows(0)("Time").ToString
            CaseId.Text = dt.Rows(0)("CaseId").ToString
            CaseName.Text = dt.Rows(0)("CaseName").ToString
            VisitingType.SelectedValue = dt.Rows(0)("VisitingType").ToString
            Value.Text = dt.Rows(0)("Value").ToString
            Payed.Text = dt.Rows(0)("Payed").ToString
            Remaining.Text = dt.Rows(0)("Remaining").ToString

            Notes.Text = dt.Rows(0)("Notes").ToString

            Utras.Text = dt.Rows(0)("Utras").ToString
            Cervix.Text = dt.Rows(0)("Cervix").ToString
            Ovaries.Text = dt.Rows(0)("Ovaries").ToString
            Valva.Text = dt.Rows(0)("Valva").ToString
            Vagina.Text = dt.Rows(0)("Vagina").ToString
            E2.Text = dt.Rows(0)("E2").ToString
            HMG.Text = dt.Rows(0)("HMG").ToString
            FSH.Text = dt.Rows(0)("FSH").ToString
            Against.Text = dt.Rows(0)("Against").ToString
            Antagonist.Text = dt.Rows(0)("Antagonist").ToString
            RT.Text = dt.Rows(0)("RT").ToString
            LT.Text = dt.Rows(0)("LT").ToString
            EEnd.Text = dt.Rows(0)("EEnd").ToString
            RRemarks.Text = dt.Rows(0)("RRemarks").ToString
            PB.Text = dt.Rows(0)("PB").ToString
            Remarks1.Text = dt.Rows(0)("Remarks1").ToString
            Remarks2.Text = dt.Rows(0)("Remarks2").ToString
            Investigations.Text = dt.Rows(0)("Investigations").ToString
            NextVisitDate.SelectedDate = IIf(IsDate(dt.Rows(0)("NextVisitDate")), dt.Rows(0)("NextVisitDate"), Nothing)

        End If

        dt = bm.ExecuteAdapter("select * from Cases where Id='" & CaseId.Text & "'")
        If dt.Rows.Count > 0 Then
            Protocole.Text = dt.Rows(0)("Protocole").ToString
            Remarks.Text = dt.Rows(0)("Remarks").ToString
            LMP.SelectedDate = IIf(IsDate(dt.Rows(0)("LMP")), dt.Rows(0)("LMP"), Nothing)
            EDD.SelectedDate = IIf(IsDate(dt.Rows(0)("EDD")), dt.Rows(0)("EDD"), Nothing)
            G.Text = dt.Rows(0)("G").ToString
            P.Text = dt.Rows(0)("P").ToString
            A.Text = dt.Rows(0)("A").ToString
            Other.Text = dt.Rows(0)("Other").ToString
            SurgicalHistory.Text = dt.Rows(0)("SurgicalHistory").ToString
            ObstetricalHistory.Text = dt.Rows(0)("ObstetricalHistory").ToString
        End If

        dt = bm.ExecuteAdapter("select * from ReservationCbo0Clinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        G0.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G0.Rows.Add()
            G0.Rows(i).Cells(GC0.Notes).Value = dt.Rows(i)("Notes").ToString
        Next
        G0.RefreshEdit()

        dt = bm.ExecuteAdapter("select * from ReservationCbo1Clinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        G1.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G1.Rows.Add()
            G1.Rows(i).Cells(GC1.Notes).Value = dt.Rows(i)("Notes").ToString
        Next
        G1.RefreshEdit()

        dt = bm.ExecuteAdapter("select * from ReservationCbo2Clinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        G2.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G2.Rows.Add()
            G2.Rows(i).Cells(GC2.Notes).Value = dt.Rows(i)("Notes").ToString
        Next
        G2.RefreshEdit()

        dt = bm.ExecuteAdapter("select * from ReservationCbo3Clinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        G3.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G3.Rows.Add()
            G3.Rows(i).Cells(GC3.Notes1).Value = dt.Rows(i)("Notes1").ToString
            G3.Rows(i).Cells(GC3.Notes2).Value = dt.Rows(i)("Notes2").ToString
        Next
        G3.RefreshEdit()

        dt = bm.ExecuteAdapter("select * from ReservationRaysClinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        G4.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G4.Rows.Add()
            G4.Rows(i).Cells(GC4.Notes).Value = dt.Rows(i)("Notes").ToString
        Next
        G4.RefreshEdit()

        dt = bm.ExecuteAdapter("select * from ReservationTestsClinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        G5.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G5.Rows.Add()
            G5.Rows(i).Cells(GC5.Notes1).Value = dt.Rows(i)("Notes1").ToString
            G5.Rows(i).Cells(GC5.Notes2).Value = dt.Rows(i)("Notes2").ToString
        Next
        G5.RefreshEdit()



        AimOfTheDay.Clear()
        G6.Rows.Clear()
        If Not DatePicker1.SelectedDate Is Nothing Then
            dt = bm.ExecuteAdapter("select * from ProgressNote where CaseId=" & Val(CaseId.Text) & " and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "'")
            If dt.Rows.Count > 0 Then
                AimOfTheDay.Text = dt.Rows(0)("AimOfTheDay").ToString
            End If

            For i As Integer = 0 To dt.Rows.Count - 1
                G6.Rows.Add()
                G6.Rows(i).Cells(GC6.Problem).Value = dt.Rows(i)("Problem").ToString
                G6.Rows(i).Cells(GC6.CasePlan).Value = dt.Rows(i)("CasePlan").ToString
                G6.Rows(i).Cells(GC6.Note).Value = dt.Rows(i)("Note").ToString
                G6.Rows(i).Cells(GC6.DoctorId).Value = dt.Rows(i)("DoctorId").ToString
                G6.Rows(i).Cells(GC6.Time).Value = dt.Rows(i)("Time").ToString
            Next
        End If

        LoadTree()

    End Sub


    Private Sub DatePicker1_SelectedDatesChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles DatePicker1.SelectedDateChanged, ClinicId.SelectionChanged
        LoadReservations()
    End Sub


    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNew.Click
        lblReservId.Content = ""
        lblTime.Content = ""
        CaseId.Clear()
        CaseName.Clear()
        VisitingType.SelectedIndex = 0
        Value.Clear()
        Payed.Clear()
        Remaining.Clear()


        Notes.Clear()

        Protocole.Clear()
        Remarks.Clear()
        LMP.SelectedDate = Nothing
        EDD.SelectedDate = Nothing
        G.Clear()
        P.Clear()
        A.Clear()
        Other.Clear()
        SurgicalHistory.Clear()
        ObstetricalHistory.Clear()

        Utras.Clear()
        Cervix.Clear()
        Ovaries.Clear()
        Valva.Clear()
        Vagina.Clear()
        E2.Clear()
        HMG.Clear()
        FSH.Clear()
        Against.Clear()
        Antagonist.Clear()
        RT.Clear()
        LT.Clear()
        EEnd.Clear()
        RRemarks.Clear()
        PB.Clear()
        Remarks1.Clear()
        Remarks2.Clear()
        Investigations.Clear()
        NextVisitDate.SelectedDate = Nothing

        AimOfTheDay.Clear()
        Problem.Clear()
        CasePlan.Clear()
        Note.Clear()

        G0.Rows.Clear()
        G1.Rows.Clear()
        G2.Rows.Clear()
        G3.Rows.Clear()
        G4.Rows.Clear()
        G5.Rows.Clear()
        G6.Rows.Clear()


        TreeView1.Items.Clear()
    End Sub

    Dim AllowPrint As Boolean = False
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click
        btnSave_Click(sender, e)
        If Not AllowPrint Then Return
        Dim rpt As New ReportViewer
        If ClinicId.SelectedValue Is Nothing Then ClinicId.SelectedIndex = 0
        rpt.paraname = New String() {"@CaseId", "CaseName", "@Flag", "@MainId", "@DayDate", "@Id", "Header"}
        rpt.paravalue = New String() {Val(CaseId.Text), CaseName.Text, 1, Val(ClinicId.SelectedValue), bm.ToStrDate(DatePicker1.SelectedDate), lblReservId.Content.ToString, "Patient History"}
        rpt.Rpt = "CaseAllDetails.rpt"
        rpt.Show()
    End Sub

    Function ch(ByVal c As CheckBox)
        If c.IsChecked Then
            Return "1"
        Else
            Return "0"
        End If
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        AllowPrint = False
        If Not Valid() Then Return

        Post(1)

        G0.EndEdit()
        G1.EndEdit()
        G2.EndEdit()
        G3.EndEdit()
        G4.EndEdit()
        G5.EndEdit()
        G6.EndEdit()

        bm.SaveGrid(G0, "ReservationCbo0Clinics", New String() {"ClinicId", "DayDate", "ReservId"}, New String() {ClinicId.SelectedValue.ToString, bm.ToStrDate(DatePicker1.SelectedDate), lblReservId.Content.ToString}, New String() {"Notes"}, New String() {GC0.Notes}, New VariantType() {VariantType.String}, New String() {GC0.Notes})

        bm.SaveGrid(G1, "ReservationCbo1Clinics", New String() {"ClinicId", "DayDate", "ReservId"}, New String() {ClinicId.SelectedValue.ToString, bm.ToStrDate(DatePicker1.SelectedDate), lblReservId.Content.ToString}, New String() {"Notes"}, New String() {GC1.Notes}, New VariantType() {VariantType.String}, New String() {GC1.Notes})

        bm.SaveGrid(G2, "ReservationCbo2Clinics", New String() {"ClinicId", "DayDate", "ReservId"}, New String() {ClinicId.SelectedValue.ToString, bm.ToStrDate(DatePicker1.SelectedDate), lblReservId.Content.ToString}, New String() {"Notes"}, New String() {GC2.Notes}, New VariantType() {VariantType.String}, New String() {GC2.Notes})

        bm.SaveGrid(G3, "ReservationCbo3Clinics", New String() {"ClinicId", "DayDate", "ReservId"}, New String() {ClinicId.SelectedValue.ToString, bm.ToStrDate(DatePicker1.SelectedDate), lblReservId.Content.ToString}, New String() {"Notes1", "Notes2"}, New String() {GC3.Notes1, GC3.Notes2}, New VariantType() {VariantType.String, VariantType.String}, New String() {GC3.Notes1})

        bm.SaveGrid(G4, "ReservationRaysClinics", New String() {"ClinicId", "DayDate", "ReservId"}, New String() {ClinicId.SelectedValue.ToString, bm.ToStrDate(DatePicker1.SelectedDate), lblReservId.Content.ToString}, New String() {"Notes"}, New String() {GC4.Notes}, New VariantType() {VariantType.String}, New String() {GC4.Notes})

        bm.SaveGrid(G5, "ReservationTestsClinics", New String() {"ClinicId", "DayDate", "ReservId"}, New String() {ClinicId.SelectedValue.ToString, bm.ToStrDate(DatePicker1.SelectedDate), lblReservId.Content.ToString}, New String() {"Notes1", "Notes2"}, New String() {GC5.Notes1, GC5.Notes2}, New VariantType() {VariantType.String, VariantType.String}, New String() {GC5.Notes1})

        bm.SaveGrid(G6, "ProgressNote", New String() {"CaseId", "DayDate"}, New String() {Val(CaseId.Text), bm.ToStrDate(DatePicker1.SelectedDate)}, New String() {"Problem", "CasePlan", "Note", "DoctorId", "Time"}, New String() {GC6.Problem, GC6.CasePlan, GC6.Note, GC6.DoctorId, GC6.Time}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String}, New String() {})

        bm.ExecuteNonQuery("update ProgressNote set AimOfTheDay='" & AimOfTheDay.Text.Replace("'", "''") & "' where CaseId=" & Val(CaseId.Text) & " and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "'")

        AllowPrint = True
        If sender Is btnPrint Then Return

        btnNew_Click(Nothing, Nothing)
        LoadReservations()
    End Sub

    Function Delete() As Boolean
        If ClinicId.SelectedIndex < 1 OrElse DatePicker1.SelectedDate Is Nothing OrElse lblReservId.Content.ToString = "" Then
            Return False
        End If

        Post(0)
        bm.ExecuteNonQuery("Delete ReservationCbo0Clinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        bm.ExecuteNonQuery("Delete ReservationCbo1Clinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        bm.ExecuteNonQuery("Delete ReservationCbo2Clinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        bm.ExecuteNonQuery("Delete ReservationCbo3Clinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        bm.ExecuteNonQuery("Delete ReservationRaysClinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
        bm.ExecuteNonQuery("Delete ReservationTestsClinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")
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
        If ClinicId.SelectedIndex < 1 OrElse DatePicker1.SelectedDate Is Nothing OrElse lblReservId.Content.ToString = "" Then
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
            If ClinicId.SelectedIndex < 1 Then Return

            Dim dt As DataTable = bm.ExecuteAdapter("LoadReservationsClinics", New String() {"ClinicId", "Daydate"}, New String() {Val(ClinicId.SelectedValue.ToString), bm.ToStrDate(DatePicker1.SelectedDate)})

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 100
                x.Height = 50
                x.Margin = New Thickness(10, 10 + i * 60, 0, 0)
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

                If CurrentReservId = x.Tag Then
                    btnReservClick(x, Nothing)
                End If
            Next
        Catch ex As Exception
        End Try

    End Sub


    Private Sub Add0_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add0.Click
        If cbo0.Text.Trim = "" Then Return
        G0.Rows.Add(cbo0.Text)
        bm.AddItemToTable("Cbo0", cbo0.Text)
        cbo0.Text = ""
        'LoadCbo0()
    End Sub

    Private Sub Add1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add1.Click
        If cbo1.Text.Trim = "" Then Return
        G1.Rows.Add(cbo1.Text)
        bm.AddItemToTable("Cbo1", cbo1.Text)
        cbo1.Text = ""
        'LoadCbo1()
    End Sub

    Private Sub Add2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add2.Click
        If cbo2.Text.Trim = "" Then Return
        G2.Rows.Add(cbo2.Text)
        bm.AddItemToTable("Cbo2", cbo2.Text)
        cbo2.Text = ""
        'LoadCbo2()
    End Sub

    Private Sub Add3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add3.Click
        If cbo31.Text.Trim = "" Then Return
        G3.Rows.Add(cbo31.Text, cbo32.Text)
        bm.AddItemToTable("Drugs", cbo31.Text)
        bm.AddItemToTable("Doses", cbo32.Text)
        cbo31.Text = ""
        cbo32.Text = ""
        'LoadDrugs()
        'LoadDoses()
        cbo31.Focus()
    End Sub

    Private Sub Add4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add4.Click
        If Rays.Text.Trim = "" Then Return
        G4.Rows.Add(Rays.Text)
        bm.AddItemToTable("Rays", Rays.Text)
        Rays.Text = ""
        'LoadRays()
    End Sub

    Private Sub Add5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add5.Click
        If LaboratoryTestTypes.Text.Trim = "" Then Return
        G5.Rows.Add(LaboratoryTestTypes.Text, LaboratoryTests.Text)
        bm.AddItemToTable("LaboratoryTestTypes", LaboratoryTestTypes.Text)
        Dim s As String = LaboratoryTestTypes.Text
        Dim s2 As String = LaboratoryTests.Text
        LoadLaboratoryTestTypes()
        LaboratoryTestTypes.Text = s
        LaboratoryTests.Text = s2
        bm.AddItemToTable("LaboratoryTests", LaboratoryTests.Text, New String() {"TestId"}, New String() {LaboratoryTestTypes.SelectedValue.ToString})
        LaboratoryTestTypes.Text = ""
        LaboratoryTests.Text = ""
        'LoadLaboratoryTestTypes()
        'LaboratoryTestTypes.Focus()
    End Sub


    Private Sub Add6_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add6.Click
        G6.Rows.Add(Md.EnName, Now.TimeOfDay.ToString.Substring(0, 5), Problem.Text, CasePlan.Text, Note.Text)
        Problem.Text = ""
        CasePlan.Text = ""
        Note.Text = ""
    End Sub

    Private Sub LoadCbos()
        LoadCbo0()
        LoadCbo1()
        LoadCbo2()
        LoadDrugs()
        LoadDoses()
        LoadRays()
        LoadLaboratoryTestTypes()
    End Sub

    Private Sub cbo0_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles cbo0.PreviewKeyDown
        If e.Key = Key.Enter Then Add0_Click(Nothing, Nothing)
    End Sub

    Private Sub cbo1_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles cbo1.PreviewKeyDown
        If e.Key = Key.Enter Then Add1_Click(Nothing, Nothing)
    End Sub

    Private Sub cbo2_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles cbo2.PreviewKeyDown
        If e.Key = Key.Enter Then Add2_Click(Nothing, Nothing)
    End Sub

    Private Sub cbo31_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles cbo31.PreviewKeyDown
        If e.Key = Key.Enter Then cbo32.Focus()
    End Sub

    Private Sub cbo32_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles cbo32.PreviewKeyDown
        If e.Key = Key.Enter Then Add3_Click(Nothing, Nothing)
    End Sub

    Private Sub Rays_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles Rays.PreviewKeyDown
        If e.Key = Key.Enter Then Add4_Click(Nothing, Nothing)
    End Sub

    Private Sub LaboratoryTestTypes_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles LaboratoryTestTypes.PreviewKeyDown
        If e.Key = Key.Enter Then LaboratoryTests.Focus()
    End Sub

    Private Sub LaboratoryTests_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles LaboratoryTests.PreviewKeyDown
        If e.Key = Key.Enter Then Add5_Click(Nothing, Nothing)
    End Sub


    Private Sub Post(ByVal pp As Integer)
        If pp = 0 Then
            Notes.Clear()

            Protocole.Clear()
            Remarks.Clear()
            LMP.SelectedDate = Nothing
            EDD.SelectedDate = Nothing
            G.Clear()
            P.Clear()
            A.Clear()
            Other.Clear()
            SurgicalHistory.Clear()
            ObstetricalHistory.Clear()

            Utras.Clear()
            Cervix.Clear()
            Ovaries.Clear()
            Valva.Clear()
            Vagina.Clear()
            E2.Clear()
            HMG.Clear()
            FSH.Clear()
            Against.Clear()
            Antagonist.Clear()
            RT.Clear()
            LT.Clear()
            EEnd.Clear()
            RRemarks.Clear()
            PB.Clear()
            Remarks1.Clear()
            Remarks2.Clear()
            Investigations.Clear()
        End If

        If NextVisitDate.SelectedDate Is Nothing Then
            NextVisitDate.SelectedDate = Now.Date
        End If
        If LMP.SelectedDate Is Nothing Then
            LMP.SelectedDate = Now.Date
        End If
        If EDD.SelectedDate Is Nothing Then
            EDD.SelectedDate = Now.Date
        End If

        bm.ExecuteNonQuery("update ReservationsClinics set Posted=" & pp & ",Notes='" & Notes.Text.Replace("'", "''") & "',Utras='" & Utras.Text.Replace("'", "''") & "',Cervix='" & Cervix.Text.Replace("'", "''") & "',Ovaries='" & Ovaries.Text.Replace("'", "''") & "',Valva='" & Valva.Text.Replace("'", "''") & "',Vagina='" & Vagina.Text.Replace("'", "''") & "',E2='" & E2.Text.Replace("'", "''") & "',HMG='" & HMG.Text.Replace("'", "''") & "',FSH='" & FSH.Text.Replace("'", "''") & "',Against='" & Against.Text.Replace("'", "''") & "',Antagonist='" & Antagonist.Text.Replace("'", "''") & "',RT='" & RT.Text.Replace("'", "''") & "',LT='" & LT.Text.Replace("'", "''") & "',EEnd='" & EEnd.Text.Replace("'", "''") & "',RRemarks='" & RRemarks.Text.Replace("'", "''") & "',PB='" & PB.Text.Replace("'", "''") & "',Remarks1='" & Remarks1.Text.Replace("'", "''") & "',Remarks2='" & Remarks2.Text.Replace("'", "''") & "',Investigations='" & Investigations.Text.Replace("'", "''") & "',NextVisitDate='" & bm.ToStrDate(NextVisitDate.SelectedDate) & "' where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "'")

        bm.ExecuteNonQuery("update Cases set Protocole='" & Protocole.Text.Replace("'", "''") & "',Remarks='" & Remarks.Text.Replace("'", "''") & "',LMP='" & bm.ToStrDate(LMP.SelectedDate) & "',EDD='" & bm.ToStrDate(EDD.SelectedDate) & "',G='" & G.Text.Replace("'", "''") & "',P='" & P.Text.Replace("'", "''") & "',A='" & A.Text.Replace("'", "''") & "',Other='" & Other.Text.Replace("'", "''") & "',SurgicalHistory='" & SurgicalHistory.Text.Replace("'", "''") & "',ObstetricalHistory='" & ObstetricalHistory.Text.Replace("'", "''") & "' where Id='" & CaseId.Text & "'")
    End Sub

    Private Sub LaboratoryTestTypes_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles LaboratoryTestTypes.LostFocus
        Try
            bm.FillCombo("LaboratoryTests", LaboratoryTests, " Where TestId=" & Val(LaboratoryTestTypes.SelectedValue.ToString), "")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadLaboratoryTestTypes()
        bm.FillCombo("LaboratoryTestTypes", LaboratoryTestTypes, "", "")
        LaboratoryTestTypes_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub LoadRays()
        bm.FillCombo("Rays", Rays, "", "")
    End Sub

    Private Sub LoadDoses()
        bm.FillCombo("Doses", cbo32, "", "")
    End Sub

    Private Sub LoadDrugs()
        bm.FillCombo("Drugs", cbo31, "", "")
    End Sub

    Private Sub LoadCbo2()
        bm.FillCombo("Cbo2", cbo2, "", "")
    End Sub

    Private Sub LoadCbo1()
        bm.FillCombo("Cbo1", cbo1, "", "")
    End Sub

    Private Sub LoadCbo0()
        bm.FillCombo("Cbo0", cbo0, "", "")
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

    Sub UpdateEmployees(ByVal sender As Object, ByVal e As EventArgs) Handles C1.Checked, C1.Unchecked, C2.Checked, C2.Unchecked, C3.Checked, C3.Unchecked, C4.Checked, C4.Unchecked, C5.Checked, C5.Unchecked, C6.Checked, C6.Unchecked, C7.Checked, C7.Unchecked, C8.Checked, C8.Unchecked, C9.Checked, C9.Unchecked
        Try
            bm.ExecuteNonQuery("update Employees set " & sender.Name & "=" & ch(sender) & " where Id=" & Md.UserName)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub t_Tick(ByVal sender As Object, ByVal e As EventArgs)
        t.Stop()
        TabControl3.SelectedIndex = 1
        TabControl3.SelectedIndex = 0
        bm.FillCombo("select Id,Name from Clinics union select 0 Id,'-' Name order by Name", ClinicId)
        ClinicId.SelectedValue = Md.UserName
        Dim dt As New DataTable
        If ClinicId.SelectedIndex > 0 Then
            dt = bm.ExecuteAdapter("Select isnull(C1,0) C1,isnull(C2,0) C2,isnull(C3,0) C3,isnull(C4,0) C4,isnull(C5,0) C5,isnull(C6,0) C6,isnull(C7,0) C7,isnull(C8,0) C8,isnull(C9,0) C9 from Employees where Id=" & Md.UserName)
        End If

        If dt.Rows.Count > 0 Then
            C1.IsChecked = IIf(dt.Rows(0)("C1") = 1, True, False)
            C2.IsChecked = IIf(dt.Rows(0)("C2") = 1, True, False)
            C3.IsChecked = IIf(dt.Rows(0)("C3") = 1, True, False)
            C4.IsChecked = IIf(dt.Rows(0)("C4") = 1, True, False)
            C5.IsChecked = IIf(dt.Rows(0)("C5") = 1, True, False)
            C6.IsChecked = IIf(dt.Rows(0)("C6") = 1, True, False)
            C7.IsChecked = IIf(dt.Rows(0)("C7") = 1, True, False)
            C8.IsChecked = IIf(dt.Rows(0)("C8") = 1, True, False)
            C9.IsChecked = IIf(dt.Rows(0)("C9") = 1, True, False)
        End If

        LoadCbos()
        DatePicker1.SelectedDate = bm.MyGetDate()

        LoadWFH0()
        LoadWFH1()
        LoadWFH2()
        LoadWFH3()
        LoadWFH4()
        LoadWFH5()
        LoadWFH6()

        LoadReservations()
        bm.ApplyOpenCombobox(New ComboBox() {cbo0, cbo1, cbo2, cbo32, Rays, LaboratoryTests, LaboratoryTestTypes}) 'cbo31,
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnRefresh.Click
        LoadCbos()
        t_Tick(Nothing, Nothing)
    End Sub



    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        'lblName.SetResourceReference(ContentProperty, "Name")
    End Sub


    Private Sub ViewHistory_Copy_Click(sender As Object, e As RoutedEventArgs) Handles ViewHistory_Copy.Click
        Try
            EDD.SelectedDate = LMP.SelectedDate.Value.AddMonths(9).AddDays(7)
        Catch ex As Exception
        End Try
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
            F1 = ClinicId.SelectedValue.ToString
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
            myCommand = New SqlClient.SqlCommand("select Image from CaseAttachments2Clinics where ClinicId='" & F1 & "' and DayDate='" & F12 & "' and ReservId='" & F13 & "' and AttachedName='" & F2 & "'" & bm.AppendWhere, con)
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
                    bm.ExecuteNonQuery("delete from CaseAttachments2Clinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "' and AttachedName='" & TreeView1.SelectedItem.Header & "'" & bm.AppendWhere)
                    LoadTree()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadTree()
        Dim dt As DataTable = bm.ExecuteAdapter("select AttachedName from CaseAttachments2Clinics where ClinicId='" & ClinicId.SelectedValue.ToString & "' and DayDate='" & bm.ToStrDate(DatePicker1.SelectedDate) & "' and ReservId='" & lblReservId.Content.ToString & "' " & bm.AppendWhere)
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
                bm.SaveFile("CaseAttachments2", "ClinicId", ClinicId.SelectedValue.ToString, "DayDate", bm.ToStrDate(DatePicker1.SelectedDate), "ReservId", lblReservId.Content.ToString, "AttachedName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "Image", o.FileNames(i))
            Next
        End If
        LoadTree()
    End Sub


    Private Sub TreeView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TreeView1.MouseDoubleClick
        Button4_Click(Nothing, Nothing)
    End Sub

    Private Sub ViewHistory_Click(sender As Object, e As RoutedEventArgs) Handles ViewHistory.Click
        Dim rpt As New ReportViewer
        If ClinicId.SelectedValue Is Nothing Then ClinicId.SelectedIndex = 0
        rpt.paraname = New String() {"@CaseId", "CaseName", "@Flag", "@MainId", "@DayDate", "@Id", "Header"}
        rpt.paravalue = New String() {Val(CaseId.Text), CaseName.Text, -2, Val(ClinicId.SelectedValue), bm.ToStrDate(DatePicker1.SelectedDate), lblReservId.Content.ToString, "Patient History"}
        rpt.Rpt = "CaseAllDetails.rpt"
        rpt.Show()
    End Sub

    Private Sub btnAddCase_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCase.Click
        Dim frm As New MyWindow With {.Title = "Patients", .WindowState = WindowState.Maximized}
        frm.Content = New Cases With {.MyId = Val(CaseId.Text), .IsSub = IIf(Md.Nurse, True, False)}
        frm.Show()
    End Sub

    Private Sub btnClinics_Click(sender As Object, e As RoutedEventArgs) Handles btnClinics.Click
        Dim frm As New MyWindow With {.Title = "ReservationsClinics", .WindowState = WindowState.Maximized}
        frm.Content = New ReservationsClinics
        frm.Show()
    End Sub

    Private Sub btnOperations_Click(sender As Object, e As RoutedEventArgs) Handles btnOperations.Click
        Dim frm As New MyWindow With {.Title = "ReservationsOperations", .WindowState = WindowState.Maximized}
        frm.Content = New ReservationsOperations
        frm.Show()
    End Sub

    Private Sub btnInpatient_Click(sender As Object, e As RoutedEventArgs) Handles btnInpatient.Click
        Dim frm As New MyWindow With {.Title = "Inpatient", .WindowState = WindowState.Maximized}
        frm.Content = New ReservationsRooms
        frm.Show()
    End Sub

    Private Sub ReNew_Click(sender As Object, e As RoutedEventArgs) Handles btnReNew.Click
        Dim dt As DataTable = bm.ExecuteAdapter("select * from ProgressNote where CaseId=" & Val(CaseId.Text) & " and DayDate=(select max(DayDate) from ProgressNote where CaseId=" & Val(CaseId.Text) & " and DayDate<'" & bm.ToStrDate(DatePicker1.SelectedDate) & "')")
        AimOfTheDay.Clear()
        If dt.Rows.Count > 0 Then
            AimOfTheDay.Text = dt.Rows(0)("AimOfTheDay").ToString
        End If

        G6.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G6.Rows.Add()
            G6.Rows(i).Cells(GC6.Problem).Value = dt.Rows(i)("Problem").ToString
            G6.Rows(i).Cells(GC6.CasePlan).Value = dt.Rows(i)("CasePlan").ToString
            G6.Rows(i).Cells(GC6.Note).Value = dt.Rows(i)("Note").ToString
            G6.Rows(i).Cells(GC6.DoctorId).Value = dt.Rows(i)("DoctorId").ToString
            G6.Rows(i).Cells(GC6.Time).Value = Now.TimeOfDay.ToString.Substring(0, 5)
        Next

    End Sub

    Private Sub btnDelete6_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete6.Click
        Try
            G6.Rows.RemoveAt(G6.CurrentRow.Index)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CaseId_TextChanged(sender As Object, e As TextChangedEventArgs) Handles CaseId.TextChanged
        If CurrentReservId <> 0 Then Return
        bm.Table_Name = "ReservationsClinics"
        bm.SearchTable(New String() {"ClinicId", "CaseId", "ReservId"}, New String() {ClinicId.SelectedValue.ToString, CaseId.Text, lblReservId.Content.ToString}, cboSearch, "dbo.ToStrDate(DayDate)+' - '+cast(ReservId as varchar(100))", New Control() {CaseId}, "order by Name")
    End Sub

    Dim CurrentReservId As Integer = 0
    Private Sub cboSearch_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSearch.SelectionChanged
        Try
            If CurrentReservId <> 0 Then Return
            CurrentReservId = Val(cboSearch.Items(cboSearch.SelectedIndex)(0).ToString)
            DatePicker1.SelectedDate = CType(cboSearch.Items(cboSearch.SelectedIndex)(1).ToString.Substring(0, 10), DateTime)
            CurrentReservId = 0
        Catch ex As Exception
        End Try
    End Sub

End Class
