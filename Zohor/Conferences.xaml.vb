Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine

Public Class Conferences
    Public TableName As String = "ConferencesMaster"
    Public TableNameDetails As String = "ConferencesDetails"
    Public SubId As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    Public MyLinkFile As Integer = 5
    WithEvents G As New MyGrid
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, InvoiceNo}, {})

        bm.Addcontrol_MouseDoubleClick({MobileNumber, WhatsappNumber, CountryId, CityId, UniversityId, SpecialtyId, SubSpecialtyId, DegreeId, TitleId})

        bm.Fields = New String() {"ConferenceId", SubId, "DayDate", "Value", "Notes", "Canceled", "CustomerId", "RegistrationTypeId", "AttendanceTypeId", "SponsorId", "Conference", "Sponsored", "Payment", "FacultyMember", "Nursing", "Student", "InvitedVIP", "Syndicate", "Lunch", "GalaDinner", "HotelId", "RoomTypeId", "RoomUpgradeTypeId", "CheckIn", "CheckOut", "Payment2", "EarlyCheckIn", "LateCheckOut", "TwoConnectedRooms", "SeaView", "PoolView", "GroundFloor", "ExtraBed", "ExtraMeal", "ExtraNight", "ShuttelBuss", "Transportation", "IsAttend"}
        bm.control = New Control() {ConferenceId, InvoiceNo, DayDate, Value, Notes, Canceled, CustomerId, RegistrationTypeId, AttendanceTypeId, SponsorId, Conference, Sponsored, Payment, FacultyMember, Nursing, Student, InvitedVIP, Syndicate, Lunch, GalaDinner, HotelId, RoomTypeId, RoomUpgradeTypeId, CheckIn, CheckOut, Payment2, EarlyCheckIn, LateCheckOut, TwoConnectedRooms, SeaView, PoolView, GroundFloor, ExtraBed, ExtraMeal, ExtraNight, ShuttelBuss, Transportation, IsAttend}
        bm.KeyFields = New String() {"ConferenceId", SubId}
        bm.Table_Name = TableName


        dt = bm.ExecuteAdapter("select Id, Name from Conferences where IsActive=1")
        Select Case dt.Rows.Count
            Case 0
                bm.ShowMSG("There Is no active conference... please, contact your administator")
            Case 1
                ConferenceId.Text = dt.Rows(0)(0)
                ConferenceId_LostFocus(Nothing, Nothing)
            Case Else
                ConferenceId_KeyDown(Nothing, Nothing)
        End Select


    End Sub

    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Line As String = "Line"
        Shared IsSelected As String = "IsSelected"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Id, "Id")
        G.Columns.Add(GC.Name, "Name")

        G.Columns.Add(GC.Line, "Line")

        Dim GCIsSelected As New Forms.DataGridViewCheckBoxColumn
        GCIsSelected.HeaderText = "Select"
        GCIsSelected.Name = GC.IsSelected
        G.Columns.Add(GCIsSelected)

        G.Columns(GC.Id).FillWeight = 110
        G.Columns(GC.Name).FillWeight = 280

        G.Columns(GC.Id).Visible = False
        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Line).Visible = False

        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False

        dt = bm.ExecuteAdapter("select Id,Name from Workshops where ConferenceId=" & ConferenceId.Text)
        G.EndEdit()
        G.Rows.Clear()

        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)(0)
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)(1)
            G.Rows(i).Cells(GC.IsSelected).Value = 0
        Next
    End Sub


    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {"ConferenceId", SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)



        dt = bm.ExecuteAdapter("select T.Id,T.Name,(case when TT.WorkShopId is null then 0 else 1 end)IsSelected from Workshops T left join ConferencesDetails TT on(T.ConferenceId=TT.ConferenceId and T.Id=TT.WorkShopId and TT.InvoiceNo='" & InvoiceNo.Text & "') where T.ConferenceId='" & ConferenceId.Text & "'")
        If dt.Rows.Count = 0 Then
            For i As Integer = 0 To G.Rows.Count - 1
                G.Rows(i).Cells(GC.IsSelected).Value = 0
            Next
        Else
            G.Rows.Clear()
            For i As Integer = 0 To dt.Rows.Count - 1
                G.Rows.Add()
                G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)(0)
                G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)(1)
                G.Rows(i).Cells(GC.IsSelected).Value = IIf(dt.Rows(i)(2) = 1, True, False)
            Next
        End If
        G.EndEdit()
        CustomerId_LostFocus(CustomerId, Nothing)

        RegistrationTypeId_LostFocus(Nothing, Nothing)
        AttendanceTypeId_LostFocus(Nothing, Nothing)
        SponsorId_LostFocus(Nothing, Nothing)
        HotelId_LostFocus(Nothing, Nothing)
        RoomTypeId_LostFocus(Nothing, Nothing)
        RoomUpgradeTypeId_LostFocus(Nothing, Nothing)

        Notes.Focus()


    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {"ConferenceId", SubId}, New String() {ConferenceId.Text, InvoiceNo.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Val(ConferenceId.Text) = 0 Then
            bm.ShowMSG("Please, select a Conference")
            ConferenceId.Focus()
            Return
        End If

        If CustomerName.Text.Trim = "" Then
            bm.ShowMSG("Please, select a Customer")
            CustomerName.Focus()
            Return
        End If




        SaveCustomer()


        Dim IsNew As Boolean = False
        If Val(InvoiceNo.Text) = 0 Then
            IsNew = True
            InvoiceNo.Text = bm.ExecuteScalar("select isnull(MAX(InvoiceNo),0)+1 from ConferencesMaster where ConferenceId=" & ConferenceId.Text)
        End If

        If Val(InvoiceNo.Text) = 0 Then
            Return
        End If

        Value.Text = Val(Value.Text)
        bm.DefineValues()

        If Not bm.Save(New String() {"ConferenceId", SubId}, New String() {ConferenceId.Text, InvoiceNo.Text}) Then
            InvoiceNo.Clear()
            Return
        End If



        G.EndEdit()
        Dim sql As String = ""
        For i As Integer = 0 To G.Rows.Count - 1
            If G.Rows(i).Cells(GC.IsSelected).Value IsNot Nothing AndAlso G.Rows(i).Cells(GC.IsSelected).Value Then
                sql &= " insert ConferencesDetails(ConferenceId,InvoiceNo,WorkShopId) select '" & Val(ConferenceId.Text) & "','" & Val(InvoiceNo.Text) & "','" & G.Rows(i).Cells(GC.Id).Value & "' "
            End If
        Next

        If Not bm.ExecuteNonQuery("delete ConferencesDetails where ConferenceId='" & Val(ConferenceId.Text) & "' and InvoiceNo='" & Val(InvoiceNo.Text) & "' " & sql) Then Return



        If Not DontClear Then
            btnNew_Click(sender, e)
        End If

    End Sub

    Private Sub SaveCustomer()
        If Val(CustomerId.Text) = 0 Then
            CustomerId.Text = bm.ExecuteScalar("declare @Code bigint=(select isnull(MAX(Code),10000)+1 from Customers)  declare @Id bigint=dbo.ean13(@Code) insert Customers(Code,Id,Name,MobileNumber,WhatsappNumber,CountryId,CityId,UniversityId,Email,WorkPlace,DateOfBirth,SpecialtyId,SubSpecialtyId,DegreeId,TitleId,SponsorId) select @Code,@Id,'" & CustomerName.Text.Trim.Replace("'", "''") & "','" & MobileNumber.Text.Trim.Replace("'", "''") & "','" & WhatsappNumber.Text.Trim.Replace("'", "''") & "','" & CountryId.Text.Trim.Replace("'", "''") & "','" & CityId.Text.Trim.Replace("'", "''") & "','" & UniversityId.Text.Trim.Replace("'", "''") & "','" & Email.Text.Trim.Replace("'", "''") & "','" & WorkPlace.Text.Trim.Replace("'", "''") & "','" & bm.ToStrDate(DateOfBirth.SelectedDate) & "','" & SpecialtyId.Text.Trim.Replace("'", "''") & "','" & SubSpecialtyId.Text.Trim.Replace("'", "''") & "','" & DegreeId.Text.Trim.Replace("'", "''") & "','" & TitleId.Text.Trim.Replace("'", "''") & "','" & SponsorId.Text.Trim.Replace("'", "''") & "' select @Id")
            InvoiceNo.Text = CustomerId.Text
        Else
            bm.ExecuteNonQuery("update Customers set Name='" & CustomerName.Text.Trim.Replace("'", "''") & "',MobileNumber='" & MobileNumber.Text.Trim.Replace("'", "''") & "',WhatsappNumber='" & WhatsappNumber.Text.Trim.Replace("'", "''") & "',CountryId='" & CountryId.Text.Trim.Replace("'", "''") & "',CityId='" & CityId.Text.Trim.Replace("'", "''") & "',UniversityId='" & UniversityId.Text.Trim.Replace("'", "''") & "',Email='" & Email.Text.Trim.Replace("'", "''") & "',WorkPlace='" & WorkPlace.Text.Trim.Replace("'", "''") & "',DateOfBirth='" & bm.ToStrDate(DateOfBirth.SelectedDate) & "',SpecialtyId='" & SpecialtyId.Text.Trim.Replace("'", "''") & "',SubSpecialtyId='" & SubSpecialtyId.Text.Trim.Replace("'", "''") & "',DegreeId='" & DegreeId.Text.Trim.Replace("'", "''") & "',TitleId='" & TitleId.Text.Trim.Replace("'", "''") & "',SponsorId='" & SponsorId.Text.Trim.Replace("'", "''") & "' where Id=" & CustomerId.Text)
        End If

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {"ConferenceId", SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        InvoiceNo.Clear()
        ClearControls()
    End Sub

    Dim lop As Boolean = False
    Sub ClearControls()
        TabControl2.SelectedIndex = 0

        If Not lop AndAlso Not IsCustLeave Then
            lop = True
            bm.ClearControls(False)
            CustomerId.Text = InvoiceNo.Text
        End If
        lop = True
        Conference.IsChecked = True

        For i As Integer = 0 To G.Rows.Count - 1
            G.Rows(i).Cells(GC.IsSelected).Value = 0
            G.CurrentCell = G.Rows(i).Cells(GC.Name)
        Next
        G.EndEdit()

        If Not IsCustLeave Then CustomerId_LostFocus(CustomerId, Nothing)
        RegistrationTypeId_LostFocus(Nothing, Nothing)
        AttendanceTypeId_LostFocus(Nothing, Nothing)
        SponsorId_LostFocus(Nothing, Nothing)
        HotelId_LostFocus(Nothing, Nothing)
        RoomTypeId_LostFocus(Nothing, Nothing)
        RoomUpgradeTypeId_LostFocus(Nothing, Nothing)


        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow

        If Not IsCustLeave Then MobileNumber.Focus()
        lop = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where  " & SubId & "=" & InvoiceNo.Text & " and ConferenceId=" & ConferenceId.Text)
            bm.ExecuteNonQuery("delete from " & TableNameDetails & " where  " & SubId & "=" & InvoiceNo.Text & " and ConferenceId=" & ConferenceId.Text)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {"ConferenceId", SubId}, New String() {ConferenceId.Text, InvoiceNo.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub InvoiceNo_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {"ConferenceId", SubId}, New String() {ConferenceId.Text, InvoiceNo.Text}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub InvoiceNo_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles InvoiceNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub InvoiceNo_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Dim CertificateTop As Integer = 0
    Dim CertificateLeft As Integer = 0
    Dim CertificateTop2 As Integer = 0
    Dim CertificateLeft2 As Integer = 0

    Dim IDTop As Integer = 0
    Dim IDLeft As Integer = 0

    Dim IsFree As Boolean = False
    Private Sub ConferenceId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ConferenceId.LostFocus

        If Val(ConferenceId.Text.Trim) = 0 Then
            ConferenceId.Clear()
            ConferenceName.Clear()
            Return
        End If

        bm.LostFocus(ConferenceId, ConferenceName, "select Name from Conferences where IsActive=1 and Id=" & ConferenceId.Text.Trim())

        dt = bm.ExecuteAdapter("select CertificateTop,CertificateLeft,CertificateTop2,CertificateLeft2,IDTop,IDLeft,IsFree from conferences where Id=" & ConferenceId.Text)
        If dt.Rows.Count = 1 Then
            CertificateTop = Val(dt.Rows(0)("CertificateTop")) * 100
            CertificateLeft = Val(dt.Rows(0)("CertificateLeft")) * 100
            CertificateTop2 = Val(dt.Rows(0)("CertificateTop2")) * 100
            CertificateLeft2 = Val(dt.Rows(0)("CertificateLeft2")) * 100
            IDTop = Val(dt.Rows(0)("IDTop")) * 100
            IDLeft = Val(dt.Rows(0)("IDLeft")) * 100
            IsFree = IIf(dt.Rows(0)("IsFree") = 1, True, False)
        End If


        LoadWFH()

        If lop Then Return
        btnNew_Click(Nothing, Nothing)
    End Sub
    Private Sub ConferenceId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ConferenceId.KeyUp
        If bm.ShowHelp("Conferences", ConferenceId, ConferenceName, e, "select cast(Id as varchar(100)) Id,Name from Conferences where IsActive=1") Then
            ConferenceId_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub Canceled_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Checked
        Value.Text = ""
        Value.IsEnabled = False
    End Sub

    Private Sub Canceled_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Unchecked
        Value.IsEnabled = True
    End Sub


    Private Sub CountryId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CountryId.KeyUp
        If bm.ShowHelp("Countries", CountryId, CountryName, e, "select cast(Id as varchar(100)) Id,Name from Countries", "Countries") Then
            CountryId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub CountryId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId.LostFocus
        bm.LostFocus(CountryId, CountryName, "select Name from Countries where Id=" & CountryId.Text.Trim())
        CityId_LostFocus(Nothing, Nothing)
    End Sub



    Private Sub UniversityId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles UniversityId.KeyUp
        If bm.ShowHelp("Universities", UniversityId, UniversityName, e, "select cast(Id as varchar(100)) Id,Name from Universities", "Universities") Then
            UniversityId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub UniversityId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UniversityId.LostFocus
        bm.LostFocus(UniversityId, UniversityName, "select Name from Universities where Id=" & UniversityId.Text.Trim())
    End Sub



    Private Sub SpecialtyId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SpecialtyId.KeyUp
        If bm.ShowHelp("Specialties", SpecialtyId, SpecialtyName, e, "select cast(Id as varchar(100)) Id,Name from Specialties", "Specialties") Then
            SpecialtyId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SpecialtyId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SpecialtyId.LostFocus
        bm.LostFocus(SpecialtyId, SpecialtyName, "select Name from Specialties where Id=" & SpecialtyId.Text.Trim())
    End Sub



    Private Sub DegreeId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DegreeId.KeyUp
        If bm.ShowHelp("Degrees", DegreeId, DegreeName, e, "select cast(Id as varchar(100)) Id,Name from Degrees", "Degrees") Then
            DegreeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub DegreeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DegreeId.LostFocus
        bm.LostFocus(DegreeId, DegreeName, "select Name from Degrees where Id=" & DegreeId.Text.Trim())
    End Sub



    Private Sub TitleId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles TitleId.KeyUp
        If bm.ShowHelp("Titles", TitleId, TitleName, e, "select cast(Id as varchar(100)) Id,Name from Titles", "Titles") Then
            TitleId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub TitleId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TitleId.LostFocus
        bm.LostFocus(TitleId, TitleName, "select Name from Titles where Id=" & TitleId.Text.Trim())
    End Sub

    Private Sub CityId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CityId.KeyUp
        If bm.ShowHelp("Cities", CityId, CityName, e, "select cast(Id as varchar(100)) Id,Name from Cities where CountryId=" & CountryId.Text.Trim, "Cities", {"CountryId"}, {Val(CountryId.Text)}) Then
            CityId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub CityId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CityId.LostFocus
        If CountryId.Text.Trim = "" Then
            CityId.Clear()
            CityName.Clear()
            Return
        End If
        bm.LostFocus(CityId, CityName, "select Name from Cities where CountryId='" & CountryId.Text.Trim & "' and Id=" & CityId.Text.Trim())
    End Sub


    Private Sub SubSpecialtyId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SubSpecialtyId.KeyUp
        If bm.ShowHelp("SubSpecialties", SubSpecialtyId, SubSpecialtyName, e, "select cast(Id as varchar(100)) Id,Name from SubSpecialties where SpecialtyId=" & SpecialtyId.Text.Trim, "SubSpecialties", {"SpecialtyId"}, {Val(SpecialtyId.Text)}) Then
            SubSpecialtyId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SubSpecialtyId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubSpecialtyId.LostFocus
        If SpecialtyId.Text.Trim = "" Then
            SubSpecialtyId.Clear()
            SubSpecialtyName.Clear()
            Return
        End If
        bm.LostFocus(SubSpecialtyId, SubSpecialtyName, "select Name from SubSpecialties where SpecialtyId=" & SpecialtyId.Text.Trim & " and Id=" & SubSpecialtyId.Text.Trim())
    End Sub

    Private Sub Payment2_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Payment.TextChanged, Payment2.TextChanged
        Value.Text = Val(Payment.Text) + Val(Payment2.Text)
    End Sub

    Dim DontClear As Boolean = False
    Private Sub BtnPrintId_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintId.Click

        If Not IsFree AndAlso Val(Payment.Text.Trim) = 0 AndAlso SponsorId.Text.Trim = "" Then
            bm.ShowMSG("برجاء السداد أو اختيار الشركة الراعية")
            SponsorId.Focus()
            Return
        End If


        DontClear = True
        btnSave_Click(Nothing, Nothing)
        DontClear = False


        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"CustomerId", "CustomerName"}
        rpt.paravalue = New String() {CustomerId.Text, CustomerName.Text}
        rpt.Rpt = "Id.rpt"
        rpt.ReportViewer_Load(Nothing, Nothing)
        For Each c As ReportObject In rpt.ReportDoc.ReportDefinition.ReportObjects
            Try
                If c.Name = "CustomerName" Then
                    c.Top = Val(IDTop)
                End If
                c.Left = Val(IDLeft)
            Catch
            End Try
        Next
        'rpt.Show()
        rpt.Print()
        bm.ExecuteNonQuery("insert PrintIDsHistory(ConferenceId,CustomerId) select '" & Val(ConferenceId.Text) & "','" & CustomerId.Text & "'")

        btnNew_Click(sender, e)

    End Sub

    Dim IsCustLeave As Boolean = False
    Private Sub CustomerId_LostFocus(sender As Object, e As RoutedEventArgs) Handles CustomerId.LostFocus, MobileNumber.LostFocus, WhatsappNumber.LostFocus
        If sender Is CustomerId OrElse sender Is InvoiceNo Then
            dt = bm.ExecuteAdapter("select * from Customers where Id='" & CustomerId.Text & "'")
            If dt.Rows.Count = 0 Then
                CustomerId.Clear()
                MobileNumber.Clear()
                WhatsappNumber.Clear()
                CustomerName.Clear()
                CountryId.Clear()
                CountryId_LostFocus(Nothing, Nothing)
                CityId.Clear()
                CityId_LostFocus(Nothing, Nothing)
                UniversityId.Clear()
                UniversityId_LostFocus(Nothing, Nothing)
                Email.Clear()
                WorkPlace.Clear()
                DateOfBirth.SelectedDate = Nothing
                SpecialtyId.Clear()
                SpecialtyId_LostFocus(Nothing, Nothing)
                SubSpecialtyId.Clear()
                SubSpecialtyId_LostFocus(Nothing, Nothing)
                DegreeId.Clear()
                DegreeId_LostFocus(Nothing, Nothing)
                TitleId.Clear()
                TitleId_LostFocus(Nothing, Nothing)

                Return
            End If
        Else
            dt = bm.ExecuteAdapter("select * from Customers where ('" & MobileNumber.Text.Trim & "'<>'' and '" & MobileNumber.Text.Trim & "' in(MobileNumber,WhatsappNumber))  or ('" & WhatsappNumber.Text.Trim & "'<>'' and '" & WhatsappNumber.Text.Trim & "' in(MobileNumber,WhatsappNumber))")
        End If

        If dt.Rows.Count > 0 Then
            CustomerId.Text = dt.Rows(0)("Id")

            InvoiceNo.Text = CustomerId.Text
            IsCustLeave = True
            InvoiceNo_LostFocus(Nothing, Nothing)
            IsCustLeave = False

            MobileNumber.Text = dt.Rows(0)("MobileNumber")
            WhatsappNumber.Text = dt.Rows(0)("WhatsappNumber")
            CustomerName.Text = dt.Rows(0)("Name")
            CountryId.Text = dt.Rows(0)("CountryId").ToString
            CountryId_LostFocus(Nothing, Nothing)
            CityId.Text = dt.Rows(0)("CityId").ToString
            CityId_LostFocus(Nothing, Nothing)
            UniversityId.Text = dt.Rows(0)("UniversityId").ToString
            UniversityId_LostFocus(Nothing, Nothing)
            Email.Text = dt.Rows(0)("Email").ToString
            WorkPlace.Text = dt.Rows(0)("WorkPlace").ToString
            DateOfBirth.Text = bm.ToStrDate(dt.Rows(0)("DateOfBirth"))
            SpecialtyId.Text = dt.Rows(0)("SpecialtyId").ToString
            SpecialtyId_LostFocus(Nothing, Nothing)
            SubSpecialtyId.Text = dt.Rows(0)("SubSpecialtyId").ToString
            SubSpecialtyId_LostFocus(Nothing, Nothing)
            DegreeId.Text = dt.Rows(0)("DegreeId").ToString
            DegreeId_LostFocus(Nothing, Nothing)
            TitleId.Text = dt.Rows(0)("TitleId").ToString
            TitleId_LostFocus(Nothing, Nothing)
        End If

    End Sub

    Private Sub CustomerId_KeyUp(sender As Object, e As KeyEventArgs) Handles CustomerId.KeyUp, MobileNumber.KeyUp, WhatsappNumber.KeyUp, InvoiceNo.KeyUp
        If bm.ShowHelpMultiColumns("Customers", CustomerId, CustomerName, e, "select cast(C.Id as varchar(100)) 'Id',C.Name,C.MobileNumber,C.WhatsappNumber,C.Code,C.Name SponsorName from Customers C left join Sponsors S on(C.SponsorId=S.Id)") Then
            CustomerId_LostFocus(sender, Nothing)
            CustomerName.Focus()
        End If

    End Sub



    Private Sub RegistrationTypeId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles RegistrationTypeId.KeyUp
        If bm.ShowHelp("RegistrationTypes", RegistrationTypeId, RegistrationTypeName, e, "select cast(Id as varchar(100)) Id,Name from RegistrationTypes", "RegistrationTypes") Then
            RegistrationTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub RegistrationTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RegistrationTypeId.LostFocus
        bm.LostFocus(RegistrationTypeId, RegistrationTypeName, "select Name from RegistrationTypes where Id=" & RegistrationTypeId.Text.Trim())
    End Sub


    Private Sub AttendanceTypeId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AttendanceTypeId.KeyUp
        If bm.ShowHelp("AttendanceTypes", AttendanceTypeId, AttendanceTypeName, e, "select cast(Id as varchar(100)) Id,Name from AttendanceTypes", "AttendanceTypes") Then
            AttendanceTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub AttendanceTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AttendanceTypeId.LostFocus
        bm.LostFocus(AttendanceTypeId, AttendanceTypeName, "select Name from AttendanceTypes where Id=" & AttendanceTypeId.Text.Trim())
    End Sub


    Private Sub SponsorId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SponsorId.KeyUp
        If bm.ShowHelp("Sponsors", SponsorId, SponsorName, e, "select cast(Id as varchar(100)) Id,Name from Sponsors", "Sponsors") Then
            SponsorId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SponsorId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SponsorId.LostFocus
        bm.LostFocus(SponsorId, SponsorName, "select Name from Sponsors where Id=" & SponsorId.Text.Trim())
    End Sub



    Private Sub HotelId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles HotelId.KeyUp
        If bm.ShowHelp("Hotels", HotelId, HotelName, e, "select cast(Id as varchar(100)) Id,Name from Hotels", "Hotels") Then
            HotelId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub HotelId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles HotelId.LostFocus
        bm.LostFocus(HotelId, HotelName, "select Name from Hotels where Id=" & HotelId.Text.Trim())
    End Sub



    Private Sub RoomTypeId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles RoomTypeId.KeyUp
        If bm.ShowHelp("RoomTypes", RoomTypeId, RoomTypeName, e, "select cast(Id as varchar(100)) Id,Name from RoomTypes", "RoomTypes") Then
            RoomTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub RoomTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RoomTypeId.LostFocus
        bm.LostFocus(RoomTypeId, RoomTypeName, "select Name from RoomTypes where Id=" & RoomTypeId.Text.Trim())
    End Sub



    Private Sub RoomUpgradeTypeId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles RoomUpgradeTypeId.KeyUp
        If bm.ShowHelp("RoomUpgradeTypes", RoomUpgradeTypeId, RoomUpgradeTypeName, e, "select cast(Id as varchar(100)) Id,Name from RoomUpgradeTypes", "RoomUpgradeTypes") Then
            RoomUpgradeTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub RoomUpgradeTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RoomUpgradeTypeId.LostFocus
        bm.LostFocus(RoomUpgradeTypeId, RoomUpgradeTypeName, "select Name from RoomUpgradeTypes where Id=" & RoomUpgradeTypeId.Text.Trim())
    End Sub



    Private Sub btnImportFromExcel_Click(sender As Object, e As RoutedEventArgs) Handles btnImportFromExcel.Click
        btnNew_Click(Nothing, Nothing)
        Dim dtAll As DataTable = bm.OpenExcel()
        If dtAll.Rows.Count = 0 Then Return

        Dim MyView As New DataView(dtAll)
        Dim DistinctDt As DataTable = MyView.ToTable(True, dtAll.Columns(2).ColumnName)

        For i As Integer = 0 To DistinctDt.Rows.Count - 1
            If DistinctDt.Rows(i)(0).ToString.Trim = "" Then Continue For
            bm.ExecuteNonQuery("if not exists(select Id from Sponsors where Name='" & DistinctDt.Rows(i)(0).ToString.Trim & "') begin  declare @Id int=(select isnull(max(Id),0)+1 from Sponsors) insert Sponsors(Id,Name)select @Id,'" & DistinctDt.Rows(i)(0).ToString.Trim & "' end")
        Next


        Dim sql As String = "declare @Code bigint declare @Id bigint"

        For i As Integer = 0 To dtAll.Rows.Count - 1
            If dtAll.Rows(i)(0).ToString.Trim = "" Then Continue For

            Dim MyMobileNumber As String = dtAll.Rows(i)(1).ToString.Trim.Replace("'", "''").Replace(" ", "''").Replace("-", "''")

            sql &= "  select @Id=null,@Code=null   select @Id=Id,@Code=Code from Customers where ('" & MyMobileNumber & "'<>'' and '" & MyMobileNumber & "' in(MobileNumber,WhatsappNumber))       if @Code is null begin  select @Code =(select isnull(MAX(Code),10000)+1 from Customers)  select @Id =dbo.ean13(@Code) insert Customers(Id,Name,MobileNumber,WhatsappNumber,code,SponsorId) select @Id,'" & dtAll.Rows(i)(0).ToString.Trim.Replace("'", "''") & "','" & dtAll.Rows(i)(1).ToString.Trim.Replace("'", "''") & "','" & dtAll.Rows(i)(1).ToString.Trim.Replace("'", "''") & "',@Code,(select Id from Sponsors where Name='" & dtAll.Rows(i)(2).ToString.Trim.Replace("'", "''") & "')  end  insert ConferencesMaster(ConferenceId,InvoiceNo,CustomerId,Conference) select '" & Val(ConferenceId.Text) & "',@Id,@Id,1 where not exists(select ConferenceId from ConferencesMaster where ConferenceId='" & Val(ConferenceId.Text) & "' and InvoiceNo=@Id ) "



            'MobileNumber.Text = dtAll.Rows(i)(1).ToString.Trim.Replace("'", "''")
            'WhatsappNumber.Text = dtAll.Rows(i)(1).ToString.Trim
            'CustomerId_LostFocus(Nothing, Nothing)
            'CustomerName.Text = dtAll.Rows(i)(0).ToString.Trim

            'SponsorId.Text = Val(bm.ExecuteScalar("select Id from Sponsors where Name='" & dtAll.Rows(i)(2).ToString.Trim & "'"))
            'If Val(SponsorId.Text) = 0 Then
            '    SponsorId.Text = bm.ExecuteScalar("declare @Id int=(select isnull(max(Id),0)+1 from Sponsors) insert Sponsors(Id,Name)select @Id,'" & dtAll.Rows(i)(2).ToString.Trim & "' select @Id")
            'End If

            'Conference.IsChecked = True

            'btnSave_Click(btnSave, Nothing)
        Next

        If bm.ExecuteNonQuery(sql) Then
            bm.ShowMSG("Done Successfully")
        Else
            bm.ShowMSG("Error")
        End If
    End Sub

    Private Sub MobileNumber_TextChanged(sender As Object, e As TextChangedEventArgs) Handles MobileNumber.TextChanged
        WhatsappNumber.Text = MobileNumber.Text
        WhatsappNumber.SelectAll()
    End Sub

    Private Sub BtnImportFromExcel2_Click(sender As Object, e As RoutedEventArgs) Handles btnImportFromExcel2.Click
        If G.SelectedRows.Count = 0 Then
            bm.ShowMSG("Please, select a Workshop")
            Return
        End If


        Dim index As Integer = G.SelectedRows(0).Index

        If Not bm.ShowDeleteMSG("You've selected " & G.Rows(index).Cells(GC.Name).Value & ", continue?") Then
            Return
        End If

        btnNew_Click(Nothing, Nothing)
        Dim dtAll As DataTable = bm.OpenExcel()
        If dtAll.Rows.Count = 0 Then Return

        Dim MyView As New DataView(dtAll)
        Dim DistinctDt As DataTable = MyView.ToTable(True, dtAll.Columns(2).ColumnName)

        For i As Integer = 0 To DistinctDt.Rows.Count - 1
            If DistinctDt.Rows(i)(0).ToString.Trim = "" Then Continue For
            bm.ExecuteNonQuery("if not exists(select Id from Sponsors where Name='" & DistinctDt.Rows(i)(0).ToString.Trim & "') begin  declare @Id int=(select isnull(max(Id),0)+1 from Sponsors) insert Sponsors(Id,Name)select @Id,'" & DistinctDt.Rows(i)(0).ToString.Trim & "' end")
        Next


        Dim sql As String = "declare @Code bigint declare @Id bigint"

        For i As Integer = 0 To dtAll.Rows.Count - 1
            If dtAll.Rows(i)(0).ToString.Trim = "" Then Continue For

            Dim MyMobileNumber As String = dtAll.Rows(i)(1).ToString.Trim.Replace("'", "''").Replace(" ", "''").Replace("-", "''")

            sql &= "     select @Id=Id,@Code=Code from Customers where ('" & MyMobileNumber & "'<>'' and '" & MyMobileNumber & "' in(MobileNumber,WhatsappNumber))       if @Code is null begin  select @Code =(select isnull(MAX(Code),10000)+1 from Customers)  select @Id =dbo.ean13(@Code) insert Customers(Id,Name,MobileNumber,WhatsappNumber,code,SponsorId) select @Id,'" & dtAll.Rows(i)(0).ToString.Trim.Replace("'", "''") & "','" & dtAll.Rows(i)(1).ToString.Trim.Replace("'", "''") & "','" & dtAll.Rows(i)(1).ToString.Trim.Replace("'", "''") & "',@Code,(select Id from Sponsors where Name='" & dtAll.Rows(i)(2).ToString.Trim.Replace("'", "''") & "')  end  delete ConferencesDetails where ConferenceId='" & Val(ConferenceId.Text) & "' and InvoiceNo=@Id and WorkShopId='" & G.Rows(index).Cells(GC.Id).Value.ToString & "' insert ConferencesDetails(ConferenceId,InvoiceNo,WorkShopId) select '" & Val(ConferenceId.Text) & "',@Id,'" & G.Rows(index).Cells(GC.Id).Value.ToString & "'  insert ConferencesMaster(ConferenceId,InvoiceNo,CustomerId,Conference) select '" & Val(ConferenceId.Text) & "',@Id,@Id,0 where not exists(select ConferenceId from ConferencesMaster where ConferenceId='" & Val(ConferenceId.Text) & "' and InvoiceNo=@Id ) "



            'MobileNumber.Text = dtAll.Rows(i)(1).ToString.Trim
            'WhatsappNumber.Text = dtAll.Rows(i)(1).ToString.Trim
            'CustomerId_LostFocus(Nothing, Nothing)
            'CustomerName.Text = dtAll.Rows(i)(0).ToString.Trim

            'G.Rows(index).Cells(GC.IsSelected).Value = True

            'btnSave_Click(btnSave, Nothing)
        Next

        If bm.ExecuteNonQuery(sql) Then
            bm.ShowMSG("Done Successfully")
        Else
            bm.ShowMSG("Error")
        End If

    End Sub

    Private Sub BtnPrintCertificate_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintCertificate.Click
        Dim rpt As New ReportViewer
        rpt.Rpt = "Certificate.rpt"
        rpt.paraname = New String() {"CustomerId", "CustomerName", "AttendanceHours"}
        rpt.paravalue = New String() {Val(CustomerId.Text), CustomerName.Text, bm.ExecuteScalar("select dbo.getAttendanceHours(" & Val(ConferenceId.Text) & "," & Val(CustomerId.Text) & ")")}
        rpt.ReportViewer_Load(Nothing, Nothing)
        For Each c As ReportObject In rpt.ReportDoc.ReportDefinition.ReportObjects
            Try
                If c.Name = "AttendanceHours" Then
                    c.Top = CertificateTop2
                    c.Left = CertificateLeft2
                Else
                    c.Top = CertificateTop
                    c.Left = CertificateLeft
                End If
            Catch
            End Try
        Next
        rpt.Print()
        bm.ExecuteNonQuery("insert PrintCertificatesHistory(ConferenceId,CustomerId) select '" & Val(ConferenceId.Text) & "','" & CustomerId.Text & "'")

    End Sub
End Class
