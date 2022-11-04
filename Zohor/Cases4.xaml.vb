Imports System.Data
Imports System.ComponentModel
Imports System.IO

Public Class Cases4
    Public TableName As String = "Cases"
    Public SubId As String = "Id"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    Dim WithEvents BackgroundWorker1 As New BackgroundWorker
     
    Public MyId As Integer = 0
    Public MyLinkFlie As Integer = 13
    WithEvents G As New MyGrid

    Sub NewId()
        txtID.Clear()
        'txtID.IsEnabled = Md.Manager
    End Sub

    Sub UndoNewId()
        'txtID.IsEnabled = True
    End Sub


    Public Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        'lblAccNo.Visibility = Visibility.Hidden
        'AccNo.Visibility = Visibility.Hidden
        'AccName.Visibility = Visibility.Hidden

        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnPrint, btnPrint_Copy, btnApproval1, btnApproval2, btnApproval3, btnApproval4, btnApproval5})
        bm.FillCombo("Gender", Gender, "", , True)
        bm.FillCombo("CaseTypes", CaseTypeId, "", , True)
        bm.FillCombo("Employees", SystemUser, "", , True)
        bm.FillCombo("Employees", EntryUser, "", , True)
        LoadResource()

        LoadWFH()
        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            lblId.Content = "MRN"
            lblJobId.Content = "وظيفة ولي لأمر"
            lblEnName.Visibility = Visibility.Hidden
            EnName.Visibility = Visibility.Hidden
        Else
            lblCityId.Visibility = Visibility.Hidden
            CityId.Visibility = Visibility.Hidden
            CityName.Visibility = Visibility.Hidden
        End If

        If Md.MyProjectType = ProjectType.X Then
            lblCaseTypeId.Visibility = Visibility.Hidden
            CaseTypeId.Visibility = Visibility.Hidden
            lblDoctorId.Visibility = Visibility.Hidden
            DoctorId.Visibility = Visibility.Hidden
            DoctorName.Visibility = Visibility.Hidden
            lblCompanyId.Visibility = Visibility.Hidden
            CompanyId.Visibility = Visibility.Hidden
            CompanyName.Visibility = Visibility.Hidden
            lblMemberName.Visibility = Visibility.Hidden
            MemberName.Visibility = Visibility.Hidden
            lblRelationId.Visibility = Visibility.Hidden
            RelationId.Visibility = Visibility.Hidden
            RelationName.Visibility = Visibility.Hidden
            lblBrokerCoId.Visibility = Visibility.Hidden
            BrokerCoId.Visibility = Visibility.Hidden
            BrokerCoName.Visibility = Visibility.Hidden
            lblMembershipNo.Visibility = Visibility.Hidden
            MembershipNo.Visibility = Visibility.Hidden
        Else
            btnApproval1.Visibility = Visibility.Hidden
            btnApproval2.Visibility = Visibility.Hidden
            btnApproval3.Visibility = Visibility.Hidden
            btnApproval4.Visibility = Visibility.Hidden
            btnApproval5.Visibility = Visibility.Hidden

            G.Visible = False
            WFH.Visibility = Visibility.Hidden
        End If

        bm.Fields = New String() {SubId, "Name", "D", "M", "Y", "CityId", "Gender", "SSN", "Address", "DateOfBirth", "JobId", "Notes", "BankAccount", "NationalId", "HomePhone", "Mobile", "Password", "EnName", "mm", "Occupation", "MenstrualH", "ObstetricH", "FamilyH", "OvarianFactor", "UtrineFactor", "TubalAndPeritoneal", "ObstHx", "HusbandName", "HusbandAge", "HusbandOccupation", "Risk", "GynEx", "MaleFactor", "PreviousART", "Email", "Address2", "OOR", "M2", "Sperm", "MaleCount", "Moitlity", "G3", "AbnormalForm", "Fertilization", "Empbryos", "ClassA", "Cryo", "Pregnancy", "DateOnAdmission", "Weight", "OperatedBefore", "CaseTypeId", "DoctorId", "CompanyId", "AccNo", "SystemUser", "MemberName", "RelationId", "RelationId2", "ExitTypeId", "BrokerCoId", "MembershipNo", "UpdateDate", "EntryUser", "EntryDate", "Barcode", "RecordCivilian", "Code"}
        bm.control = New Control() {txtID, txtName, D, M, Y, CityId, Gender, SSN, Address, DateOfBirth, JobId, Notes, BankAccount, NationalId, HomePhone, Mobile, Password, EnName, mm, Occupation, MenstrualH, ObstetricH, FamilyH, OvarianFactor, UtrineFactor, TubalAndPeritoneal, ObstHx, HusbandName, HusbandAge, HusbandOccupation, Risk, GynEx, MaleFactor, PreviousART, Email, Address2, OOR, M2, Sperm, MaleCount, Moitlity, G3, AbnormalForm, Fertilization, Empbryos, ClassA, Cryo, Pregnancy, DateOnAdmission, Weight, OperatedBefore, CaseTypeId, DoctorId, CompanyId, AccNo, SystemUser, MemberName, RelationId, RelationId2, ExitTypeId, BrokerCoId, MembershipNo, UpdateDate, EntryUser, EntryDate, Barcode, RecordCivilian, Code}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(Nothing, Nothing)
        If Not Md.MyProjectType = ProjectType.X Then
            TabControl1.Visibility = Visibility.Hidden
        End If
        If MyId > 0 Then
            txtID.Text = MyId
            txtID_LostFocus(Nothing, Nothing)
        End If
        If Not Md.Manager Then
            btnDelete.IsEnabled = False
            btnPrintEditing.Visibility = Visibility.Hidden
        End If
    End Sub


    Structure GC
        Shared TypeId As String = "TypeId"
        Shared CompanyId As String = "CompanyId"
        Shared DoctorId As String = "DoctorId"
        Shared OperationId As String = "OperationId"
        Shared VisitingTypeId As String = "VisitingTypeId"
        Shared UserName As String = "UserName"
        Shared MyGetDate As String = "MyGetDate"
        Shared Line As String = "Line"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        Dim GCTypeId As New Forms.DataGridViewComboBoxColumn
        GCTypeId.HeaderText = "النوع"
        GCTypeId.Name = GC.TypeId
        bm.FillCombo("select 0 Id,'-' Name union all select Id,Name From CaseTypes", GCTypeId)
        G.Columns.Add(GCTypeId)

        Dim GCCompanyId As New Forms.DataGridViewComboBoxColumn
        GCCompanyId.HeaderText = "الشركة"
        GCCompanyId.Name = GC.CompanyId
        bm.FillCombo("select 0 Id,'-' Name union all select Id,Name From Companies", GCCompanyId)
        G.Columns.Add(GCCompanyId)

        Dim GCDoctorId As New Forms.DataGridViewComboBoxColumn
        GCDoctorId.HeaderText = "الطبيب"
        GCDoctorId.Name = GC.DoctorId
        bm.FillCombo("select 0 Id,'-' Name union all select Id,Name From Employees where Doctor=1", GCDoctorId)
        G.Columns.Add(GCDoctorId)

        Dim GCOperationId As New Forms.DataGridViewComboBoxColumn
        GCOperationId.HeaderText = "العملية"
        GCOperationId.Name = GC.OperationId
        bm.FillCombo("select 0 Id,'-' Name union all select Id,Name From OperationTypes where IsStopped=0", GCOperationId)
        G.Columns.Add(GCOperationId)

        Dim GCVisitingTypeId As New Forms.DataGridViewComboBoxColumn
        GCVisitingTypeId.HeaderText = "العيادات الخارجية"
        GCVisitingTypeId.Name = GC.VisitingTypeId
        bm.FillCombo("select 0 Id,'-' Name union all select Id,Name From VisitingTypes", GCVisitingTypeId)
        G.Columns.Add(GCVisitingTypeId)

        Dim GCUserName As New Forms.DataGridViewComboBoxColumn
        GCUserName.HeaderText = "المستخدم"
        GCUserName.Name = GC.UserName
        bm.FillCombo("select 0 Id,'-' Name union all select Id,Name From Employees where SystemUser=1", GCUserName)
        G.Columns.Add(GCUserName)

        G.Columns.Add(GC.MyGetDate, "التاريخ")
        G.Columns.Add(GC.Line, "Line")
        
        'G.Columns(GC.MyGetDate).FillWeight = 280
        
        G.Columns(GC.UserName).ReadOnly = True
        G.Columns(GC.MyGetDate).ReadOnly = True

        G.Columns(GC.Line).Visible = False

        G.AutoCompleteMode = True
        AddHandler G.RowsAdded, AddressOf G_RowsAdded
    End Sub


    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        lop = True

        UndoNewId()

        bm.FillControls(Me)

        G.Rows.Clear()
        Dim dt As DataTable = bm.ExecuteAdapter("select * from CasesDetails where CaseId=" & txtID.Text & " order by Line")
        If dt.Rows.Count > 0 Then G.Rows.Add(dt.Rows.Count)
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.TypeId).Value = dt.Rows(i)("TypeId").ToString
            G.Rows(i).Cells(GC.CompanyId).Value = dt.Rows(i)("CompanyId").ToString
            G.Rows(i).Cells(GC.DoctorId).Value = dt.Rows(i)("DoctorId").ToString
            G.Rows(i).Cells(GC.OperationId).Value = dt.Rows(i)("OperationId").ToString
            G.Rows(i).Cells(GC.VisitingTypeId).Value = dt.Rows(i)("VisitingTypeId").ToString
            G.Rows(i).Cells(GC.UserName).Value = dt.Rows(i)("UserName").ToString
            G.Rows(i).Cells(GC.MyGetDate).Value = bm.ToStrDateTimeFormated(dt.Rows(i)("MyGetDate"))
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            If i > 0 Then
                G.Rows(i - 1).ReadOnly = True
            End If
        Next

        bm.GetImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        AccNo_LostFocus(Nothing, Nothing)
        CityId_LostFocus(Nothing, Nothing)
        JobId_LostFocus(Nothing, Nothing)
        DoctorId_LostFocus(Nothing, Nothing)
        CompanyId_LostFocus(Nothing, Nothing)
        RelationId_LostFocus(Nothing, Nothing)
        RelationId2_LostFocus(Nothing, Nothing)
        ExitTypeId_LostFocus(Nothing, Nothing)
        BrokerCoId_LostFocus(Nothing, Nothing)
        LoadTree()
        lop = False
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPrint.Click, btnApproval1.Click, btnApproval2.Click, btnApproval3.Click, btnApproval4.Click, btnApproval5.Click
        AllowSave = False
        If txtName.Text.Trim = "" Or Not TestNames() Then
            txtName.Focus()
            Return
        End If
        If Md.MyProjectType = ProjectType.X AndAlso DateOfBirth.SelectedDate Is Nothing Then
            bm.ShowMSG("برجاء تحديد تاريخ الميلاد")
            DateOfBirth.Focus()
            Return
        End If
        If AccNo.Visibility = Visibility.Visible AndAlso Val(AccNo.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد رقم الحساب")
            AccNo.Focus()
            Return
        End If
        Dim x As String = bm.ExecuteScalar("select Id from " & TableName & " where Name='" & txtName.Text.Trim & "' and Id<>" & txtID.Text)
        If x <> "" Then
            bm.ShowMSG("هذا المريض موجود بالفعل برقم " & x)
            txtName.Focus()
            Return
        End If


        If txtName.Text.Trim.Split(" ").Length < 4 Then
            bm.ShowMSG("برجاء تحديد اسم رباعي")
            txtName.Focus()
            Return
        End If

        If Mobile.Text.Trim.Length < 11 Then
            bm.ShowMSG("برجاء تحديد موبيل    صحيح")
            Mobile.Focus()
            Return
        End If

        If Not Mobile.Text.Trim.StartsWith("01") Then
            bm.ShowMSG("برجاء تحديد موبيل صحيح")
            Return
        End If


        JobId.Text = Val(JobId.Text)

        SystemUser.SelectedValue = Md.UserName
        UpdateDate.Text = bm.ExecuteScalar("Select GETDATE()")

        If EntryUser.SelectedValue = 0 Then EntryUser.SelectedValue = Md.UserName
        If EntryDate.Text.Trim = "" Then EntryDate.Text = bm.ExecuteScalar("Select GETDATE()")


        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If txtID.Text.Trim = "" Then
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If txtID.Text = "" Then txtID.Text = "1"
            lblLastEntry.Text = txtID.Text
            State = BasicMethods.SaveState.Insert
        End If

        G.EndEdit()

        If G.Visible Then
            Dim i As Integer = G.Rows.Count - 1
            While i >= 0
                If Val(G.Rows(i).Cells(GC.TypeId).Value) > 0 AndAlso Val(G.Rows(i).Cells(GC.DoctorId).Value) > 0 AndAlso Val(G.Rows(i).Cells(GC.OperationId).Value) > 0 Then
                    CaseTypeId.SelectedValue = G.Rows(i).Cells(GC.TypeId).Value
                    CompanyId.Text = G.Rows(i).Cells(GC.CompanyId).Value
                    DoctorId.Text = G.Rows(i).Cells(GC.DoctorId).Value
                    Exit While
                End If
                i -= 1
            End While

            If G.Rows.Count > 1 Then
                CompanyId.Text = G.Rows(G.Rows.Count - 2).Cells(GC.CompanyId).Value
            End If
        End If

        D.Text = Val(D.Text)
        M.Text = Val(M.Text)
        Y.Text = Val(Y.Text)

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                txtID.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If


        If Not bm.SaveGrid(G, "CasesDetails", New String() {"CaseId"}, New String() {txtID.Text}, New String() {"TypeId", "CompanyId", "DoctorId", "OperationId", "VisitingTypeId"}, New String() {GC.TypeId, GC.CompanyId, GC.DoctorId, GC.OperationId, GC.VisitingTypeId}, New VariantType() {VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer}, New String() {GC.TypeId}, "Line", "Line") Then Return

        bm.SaveImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        If sender Is btnPrint OrElse sender Is btnPrintEditing OrElse sender Is btnApproval1 OrElse sender Is btnApproval2 OrElse sender Is btnApproval3 OrElse sender Is btnApproval4 OrElse sender Is btnApproval5 Then
            State = BasicMethods.SaveState.Print
        End If

        TraceInvoice(State.ToString)

        If sender Is btnPrint OrElse sender Is btnApproval1 OrElse sender Is btnApproval2 OrElse sender Is btnApproval3 OrElse sender Is btnApproval4 OrElse sender Is btnApproval5 Then
            PrintPone(sender)
            Return
        End If

        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True

    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExecuteNonQuery("BeforeDeleteCases", New String() {"Id", "UserDelete", "State"}, New String() {txtID.Text, Md.UserName, State})
    End Sub


    Function TestNames() As Boolean
        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then Return True

        txtName.Text = txtName.Text.Trim
        EnName.Text = EnName.Text.Trim
        While txtName.Text.Contains("  ")
            txtName.Text = txtName.Text.Replace("  ", " ")
        End While
        While EnName.Text.Contains("  ")
            EnName.Text = EnName.Text.Replace("  ", " ")
        End While

        Dim Ar() As String
        Ar = txtName.Text.Split(" ")
        Dim En() As String
        En = EnName.Text.Split(" ")
        If Ar.Length <> En.Length Then
            bm.ShowMSG("Arabic Name Length must be EQUALE English Name Length")
            txtName.Focus()
            Return False
        End If

        Dim x As Integer = 0
        For i As Integer = 0 To Ar.Length - 1
            If Ar(i) = En(i) And Not IsNumeric(Ar(i)) Then
                bm.ShowMSG("Arabic Name could not be EQUALE English Name")
                EnName.Select(x, En(i).Length)
                EnName.Focus()
                Return False
            End If
            x += En(i).Length + 1
        Next


        For i As Integer = 0 To Ar.Length - 1
            Dim a As String = bm.ExecuteScalar("delete from Names  where Arabic_Name='" & Ar(i) & "' insert into Names (Arabic_Name,English_Name) values ('" & Ar(i) & "','" & En(i) & "')")
        Next

        Return True
    End Function
    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        NewId()

        TreeView1.Items.Clear()
        bm.ClearControls(Not DontFocus)

        G.Rows.Clear()

        SystemUser.SelectedValue = Md.UserName
        EntryUser.SelectedValue = Md.UserName

        'bm.FillCombo(TableName, CboArName, "", "", , )

        AccNo.Text = bm.ExecuteScalar("select top 1 Id from Chart where LinkFile=" & MyLinkFlie)
        AccNo_LostFocus(Nothing, Nothing)
        Barcode.Text = bm.ean13(txtID.Text)

        DateOnAdmission.SelectedDate = bm.MyGetDate
        bm.SetNoImage(Image1, True)
        CityId_LostFocus(Nothing, Nothing)
        JobId_LostFocus(Nothing, Nothing)
        DoctorId_LostFocus(Nothing, Nothing)
        CompanyId_LostFocus(Nothing, Nothing)
        RelationId_LostFocus(Nothing, Nothing)
        RelationId2_LostFocus(Nothing, Nothing)
        ExitTypeId_LostFocus(Nothing, Nothing)
        BrokerCoId_LostFocus(Nothing, Nothing)
        'txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        'If txtID.Text = "" Then txtID.Text = "1"

        If Not DontFocus Then txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            TraceInvoice("Deleted")
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from CaseAttachments where CaseId='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Public Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            'Dim s As String = txtID.Text
            ClearControls()
            'txtID.Text = s
            If Not DontFocus Then txtName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        If Not DontFocus Then
            txtName.SelectAll()
            txtName.Focus()
            txtName.SelectAll()
            txtName.Focus()
        End If
    End Sub

    Private Sub JobId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles JobId.KeyUp
        bm.ShowHelp("Jobs", JobId, JobName, e, "select cast(Id as varchar(100)) Id,Name from Jobs", "Jobs")
    End Sub

    Private Sub CityId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CityId.KeyUp
        bm.ShowHelp("Cities", CityId, CityName, e, "select cast(Id as varchar(100)) Id,Name from Cities", "Cities", {"CountryId"}, {1})
    End Sub

    Private Sub DoctorId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DoctorId.KeyUp
        bm.ShowHelp("Doctors", DoctorId, DoctorName, e, "select cast(Id as varchar(100)) Id,Name Name from Employees where Doctor=1")
    End Sub

    Private Sub CompanyId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CompanyId.KeyUp
        bm.ShowHelp("Companies", CompanyId, CompanyName, e, "select cast(Id as varchar(100)) Id,Name from Companies")
    End Sub

    Private Sub RelationId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles RelationId.KeyUp
        bm.ShowHelp("Relations", RelationId, RelationName, e, "select cast(Id as varchar(100)) Id,Name from Relations", "Relations")
    End Sub

    Private Sub RelationId2_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles RelationId2.KeyUp
        bm.ShowHelp("Relations", RelationId2, RelationName2, e, "select cast(Id as varchar(100)) Id,Name from Relations", "Relations")
    End Sub

    Private Sub ExitTypeId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ExitTypeId.KeyUp
        bm.ShowHelp("ExitTypes", ExitTypeId, ExitTypeName, e, "select cast(Id as varchar(100)) Id,Name from ExitTypes", "ExitTypes")
    End Sub

    Private Sub BrokerCoId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles BrokerCoId.KeyUp
        bm.ShowHelp("BrokerCos", BrokerCoId, BrokerCoName, e, "select cast(Id as varchar(100)) Id,Name from BrokerCos", "BrokerCos")
    End Sub

    Private Sub txtID_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelpCases(txtID, txtName, e, , , True) Then
            txtID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub


    

    Dim lop As Boolean = False

    Private Sub JobId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles JobId.LostFocus
        bm.LostFocus(JobId, JobName, "select Name from Jobs where Id=" & JobId.Text.Trim())
    End Sub

    Private Sub CityId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CityId.LostFocus
        bm.LostFocus(CityId, CityName, "select Name from Cities where CountryId=1 and Id=" & CityId.Text.Trim())
    End Sub

    Private Sub DoctorId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DoctorId.LostFocus
        bm.LostFocus(DoctorId, DoctorName, "select Name Name from Employees where Doctor=1 and Id=" & DoctorId.Text.Trim())
    End Sub

    Private Sub CompanyId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CompanyId.LostFocus
        bm.LostFocus(CompanyId, CompanyName, "select Name from Companies where Id=" & CompanyId.Text.Trim())
    End Sub

    Private Sub RelationId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RelationId.LostFocus
        bm.LostFocus(RelationId, RelationName, "select Name from Relations where Id=" & RelationId.Text.Trim())
    End Sub

    Private Sub RelationId2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RelationId2.LostFocus
        bm.LostFocus(RelationId2, RelationName2, "select Name from Relations where Id=" & RelationId2.Text.Trim())
    End Sub

    Private Sub ExitTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ExitTypeId.LostFocus
        bm.LostFocus(ExitTypeId, ExitTypeName, "select Name from ExitTypes where Id=" & ExitTypeId.Text.Trim())
    End Sub

    Private Sub BrokerCoId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BrokerCoId.LostFocus
        bm.LostFocus(BrokerCoId, BrokerCoName, "select Name from BrokerCos where Id=" & BrokerCoId.Text.Trim())
    End Sub

    Private Sub btnSetImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetImage.Click
        DontClear = True
        btnSave_Click(btnSave, Nothing)
        DontClear = False
        If Not AllowSave Then Return
        bm.SetImage(Image1)
    End Sub

    Private Sub btnSetNoImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetNoImage.Click
        bm.SetNoImage(Image1, True, True)
    End Sub

    Dim lop2 As Boolean = False

    Private Sub txtName_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles txtName.LostFocus
        If lop2 Then Return
        lop2 = True
        txtName.Text = txtName.Text.Trim
        While txtName.Text.Contains("  ")
            txtName.Text = txtName.Text.Replace("  ", " ")
        End While
        Dim s() As String
        s = txtName.Text.Split(" ")
        EnName.Clear()
        For i As Integer = 0 To s.Length - 1
            Dim a As String = bm.ExecuteScalar("select top 1 English_Name from Names where Arabic_Name='" & s(i) & "'")
            If a = "" Then
                EnName.Text &= s(i)
            Else
                EnName.Text &= a
            End If
            EnName.Text &= " "
        Next
        EnName.Text = EnName.Text.Trim
        lop2 = False
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDownload.Click
        Try
            MyImagedata = Nothing
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize <> 18 Then Return
            Dim s As New Forms.SaveFileDialog With {.Filter = "All files (*.*)|*.*"}
            s.Filter = "All files (*.*)|*.*"
            s.FileName = CType(TreeView1.SelectedItem, TreeViewItem).Header

            If IsNothing(sender) Then
                MyBath = bm.GetNewTempName(s.FileName)
            Else
                If Not s.ShowDialog = Forms.DialogResult.OK Then Return
                MyBath = s.FileName
            End If

            btnDownload.IsEnabled = False
            F1 = txtID.Text
            F2 = CType(TreeView1.SelectedItem, TreeViewItem).Tag
            BackgroundWorker1.RunWorkerAsync()
        Catch ex As Exception
        End Try
    End Sub
    Dim F2 As String = "", F1 As String = ""
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim myCommand As SqlClient.SqlCommand
            myCommand = New SqlClient.SqlCommand("select Image from CaseAttachments where CaseId='" & F1 & "' and AttachedName='" & F2 & "'" & bm.AppendWhere, con)
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
                    bm.ExecuteNonQuery("delete from CaseAttachments where CaseId='" & txtID.Text & "' and AttachedName='" & TreeView1.SelectedItem.Header & "'" & bm.AppendWhere)
                    LoadTree()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadTree()
        Dim dt As DataTable = bm.ExecuteAdapter("select AttachedName from CaseAttachments where CaseId=" & txtID.Text & bm.AppendWhere)
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
        DontClear = True
        btnSave_Click(btnSave, Nothing)
        DontClear = False
        If Not AllowSave Then Return

        Dim o As New Forms.OpenFileDialog
        o.Multiselect = True
        If o.ShowDialog = Forms.DialogResult.OK Then
            For i As Integer = 0 To o.FileNames.Length - 1
                bm.SaveFile("CaseAttachments", "CaseId", txtID.Text, "AttachedName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "Image", o.FileNames(i))
            Next
        End If
        LoadTree()
    End Sub


    Private Sub TreeView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TreeView1.MouseDoubleClick
        Button4_Click(Nothing, Nothing)
    End Sub



    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        btnSetImage.SetResourceReference(ContentProperty, "Change")
        btnSetNoImage.SetResourceReference(ContentProperty, "Cancel")
        btnDownload.SetResourceReference(ContentProperty, "Download")
        btnDeleteFile.SetResourceReference(ContentProperty, "Delete")
        btnAttach.SetResourceReference(ContentProperty, "Attach")

        lblId.SetResourceReference(ContentProperty, "Id")
        lblCityId.SetResourceReference(ContentProperty, "City")
        lblAddress.SetResourceReference(ContentProperty, "Address")
        lblArName.SetResourceReference(ContentProperty, "ArName")
        lblDateOfBirth.SetResourceReference(ContentProperty, "DateOfBirth")
        lblEnName.SetResourceReference(ContentProperty, "EnName")
        lblHomePhone.SetResourceReference(ContentProperty, "HomePhone")
        lblMobile.SetResourceReference(ContentProperty, "Mobile")
        lblNationalId.SetResourceReference(ContentProperty, "NationalId")
        lblNotes.SetResourceReference(ContentProperty, "Notes")
        lblGender.SetResourceReference(ContentProperty, "Gender")

        lblJobId.SetResourceReference(ContentProperty, "JobId")
        lblCompanyId.SetResourceReference(ContentProperty, "Company")

    End Sub

   
    Dim lopD As Boolean = False
    Private Sub DateOfBirth_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DateOfBirth.SelectedDateChanged
        If lopD Then Return
        lopD = True
        Try
            Dim dd As Date = bm.MyGetDate.Date
            Dim DOB As Date = New DateTime(DateOfBirth.SelectedDate.Value.Year, DateOfBirth.SelectedDate.Value.Month, DateOfBirth.SelectedDate.Value.Day)
            Dim tday As TimeSpan = dd.Subtract(DOB)
            Dim years As Integer, months As Integer, days As Integer
            months = 12 * (dd.Year - DOB.Year) + (dd.Month - DOB.Month)

            If dd.Day < DOB.Day Then
                months -= 1
                days = DateTime.DaysInMonth(DOB.Year, DOB.Month) - DOB.Day + dd.Day
            Else
                days = dd.Day - DOB.Day
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

    Private Sub ViewHistory_Click(sender As Object, e As RoutedEventArgs) Handles ViewHistory.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@CaseId", "CaseName", "@Flag", "@MainId", "@DayDate", "@Id", "Header"}
        rpt.paravalue = New String() {Val(txtID.Text), txtName.Text, -2, 0, bm.ToStrDate(Now.Date), 0, "Patient History"}
        rpt.Rpt = "CaseAllDetails.rpt"
        rpt.Show()
    End Sub


    Private Sub CaseTypeId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CaseTypeId.SelectionChanged
        If CaseTypeId.SelectedIndex < 0 Then Return
        Select Case CaseTypeId.Items(CaseTypeId.SelectedIndex)(0)
            Case 0
                lblDoctorId.Visibility = Visibility.Hidden
                DoctorId.Visibility = Visibility.Hidden
                DoctorName.Visibility = Visibility.Hidden

                lblCompanyId.Visibility = Visibility.Hidden
                CompanyId.Visibility = Visibility.Hidden
                CompanyName.Visibility = Visibility.Hidden

                lblMemberName.Visibility = Visibility.Hidden
                MemberName.Visibility = Visibility.Hidden
                lblRelationId.Visibility = Visibility.Hidden
                RelationId.Visibility = Visibility.Hidden
                RelationName.Visibility = Visibility.Hidden
                lblBrokerCoId.Visibility = Visibility.Hidden
                BrokerCoId.Visibility = Visibility.Hidden
                BrokerCoName.Visibility = Visibility.Hidden
                lblMembershipNo.Visibility = Visibility.Hidden
                MembershipNo.Visibility = Visibility.Hidden
            Case 1
                lblDoctorId.Visibility = Visibility.Visible
                DoctorId.Visibility = Visibility.Visible
                DoctorName.Visibility = Visibility.Visible

                lblCompanyId.Visibility = Visibility.Hidden
                CompanyId.Visibility = Visibility.Hidden
                CompanyName.Visibility = Visibility.Hidden

                lblMemberName.Visibility = Visibility.Hidden
                MemberName.Visibility = Visibility.Hidden
                lblRelationId.Visibility = Visibility.Hidden
                RelationId.Visibility = Visibility.Hidden
                RelationName.Visibility = Visibility.Hidden
                lblBrokerCoId.Visibility = Visibility.Hidden
                BrokerCoId.Visibility = Visibility.Hidden
                BrokerCoName.Visibility = Visibility.Hidden
                lblMembershipNo.Visibility = Visibility.Hidden
                MembershipNo.Visibility = Visibility.Hidden
            Case 2, 3
                lblDoctorId.Visibility = Visibility.Visible
                DoctorId.Visibility = Visibility.Visible
                DoctorName.Visibility = Visibility.Visible

                lblCompanyId.Visibility = Visibility.Hidden
                CompanyId.Visibility = Visibility.Hidden
                CompanyName.Visibility = Visibility.Hidden

                lblMemberName.Visibility = Visibility.Hidden
                MemberName.Visibility = Visibility.Hidden
                lblRelationId.Visibility = Visibility.Hidden
                RelationId.Visibility = Visibility.Hidden
                RelationName.Visibility = Visibility.Hidden
                lblBrokerCoId.Visibility = Visibility.Hidden
                BrokerCoId.Visibility = Visibility.Hidden
                BrokerCoName.Visibility = Visibility.Hidden
                lblMembershipNo.Visibility = Visibility.Hidden
                MembershipNo.Visibility = Visibility.Hidden
            Case 4
                lblDoctorId.Visibility = Visibility.Hidden
                DoctorId.Visibility = Visibility.Hidden
                DoctorName.Visibility = Visibility.Hidden

                lblCompanyId.Visibility = Visibility.Visible
                CompanyId.Visibility = Visibility.Visible
                CompanyName.Visibility = Visibility.Visible

                lblMemberName.Visibility = Visibility.Visible
                MemberName.Visibility = Visibility.Visible
                lblRelationId.Visibility = Visibility.Visible
                RelationId.Visibility = Visibility.Visible
                RelationName.Visibility = Visibility.Visible
                lblBrokerCoId.Visibility = Visibility.Visible
                BrokerCoId.Visibility = Visibility.Visible
                BrokerCoName.Visibility = Visibility.Visible
                lblMembershipNo.Visibility = Visibility.Visible
                MembershipNo.Visibility = Visibility.Visible
            Case 5
                lblDoctorId.Visibility = Visibility.Visible
                DoctorId.Visibility = Visibility.Visible
                DoctorName.Visibility = Visibility.Visible

                lblCompanyId.Visibility = Visibility.Visible
                CompanyId.Visibility = Visibility.Visible
                CompanyName.Visibility = Visibility.Visible

                lblMemberName.Visibility = Visibility.Visible
                MemberName.Visibility = Visibility.Visible
                lblRelationId.Visibility = Visibility.Visible
                RelationId.Visibility = Visibility.Visible
                RelationName.Visibility = Visibility.Visible
                lblBrokerCoId.Visibility = Visibility.Visible
                BrokerCoId.Visibility = Visibility.Visible
                BrokerCoName.Visibility = Visibility.Visible
                lblMembershipNo.Visibility = Visibility.Visible
                MembershipNo.Visibility = Visibility.Visible
        End Select
    End Sub


    Private Sub PrintPone(ByVal sender As System.Object)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Id", "@CaseId", "Header"}
        If TypeOf (Parent) Is Page Then
            rpt.paravalue = New String() {Val(txtID.Text), Val(txtID.Text), CType(Parent, Page).Title}
        ElseIf TypeOf (Parent) Is Window Then
            rpt.paravalue = New String() {Val(txtID.Text), Val(txtID.Text), CType(Parent, Window).Title}
        End If

        Select Case CType(sender, Button).Name
            Case btnPrint.Name
                rpt.Rpt = "Cases4.rpt"
            Case btnPrintEditing.Name
                rpt.Rpt = "DeletedCases4.rpt"
            Case btnApproval1.Name
                rpt.paraname = New String() {"@Id", "@CaseId", "@Line", "Header"}
                rpt.paravalue = New String() {Val(txtID.Text), Val(txtID.Text), bm.GetAltScreenshot(MainGrid), CType(Parent, Page).Title}
                rpt.Rpt = "CasesApproval1.rpt"
            Case btnApproval2.Name
                rpt.Rpt = "CasesApproval2.rpt"
            Case btnApproval3.Name
                rpt.Rpt = "CasesApproval3.rpt"
            Case btnApproval4.Name
                rpt.Rpt = "CasesApproval4.rpt"
            Case btnApproval5.Name
                rpt.Rpt = "CasesApproval5.rpt"
        End Select

        If sender Is btnPrint Then
            rpt.Show()
        Else
            rpt.Show() 'rpt.Print()
        End If
    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , MyLinkFlie, ,, True)
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , MyLinkFlie,,, True)
    End Sub

    Dim DontFocus As Boolean = False

    Private Sub txtName_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtName.TextChanged
        If looop OrElse lop Then Return
        If txtName.Text.Trim = "" Then
            CboArName.ItemsSource = Nothing
            CboArName.IsDropDownOpen = False
            Return
        End If
        bm.FillCombo("select top 10 Id,Name from " & TableName & " where Name like '" & txtName.Text & "%'", CboArName)
        CboArName.IsDropDownOpen = CboArName.Items.Count > 0
    End Sub

    Private Sub txtName_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles txtName.PreviewKeyDown
        Try
            If e.Key = Key.Down Then
                CboArName.SelectedIndex += 1
            ElseIf e.Key = Key.Up Then
                CboArName.SelectedIndex -= 1
            ElseIf e.Key = Key.Enter Then
                If CboArName.Text.Length > 0 Then txtName.Text = CboArName.Text
                CboArName.IsDropDownOpen = False
            End If
        Catch
        End Try
    End Sub

    Dim looop As Boolean = False
    Private Sub CboArName_DropDownClosed(sender As Object, e As EventArgs) Handles CboArName.DropDownClosed
        If CboArName.Items.Count = 0 Then Return

        looop = True
        txtName.Text = CboArName.Text
        looop = False

        DontFocus = True
        txtID.Text = CboArName.SelectedValue
        txtID_LostFocus(Nothing, Nothing)
        DontFocus = False
    End Sub
    Private Sub CboArName_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CboArName.SelectionChanged
        
    End Sub

    Private Sub btnPrint_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint_Copy.Click
        Try
            Dim rpt As New ReportViewer
            rpt.Rpt = "CaseBarcode.rpt"
            rpt.paraname = New String() {"@Id", "@Count"}
            rpt.paravalue = New String() {Val(txtID.Text), Val(Count.Text)}
            rpt.Print(".", Md.BarcodePrinter, 1)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub G_RowsAdded(sender As Object, e As Forms.DataGridViewRowsAddedEventArgs)
        'If lop Then Return
        'Dim i As Integer = e.RowIndex
        'G.Rows(i).HeaderCell.Value = (i + 1).ToString
        'G.Rows(i).Cells(GC.UserName).Value = Md.UserName
        'G.Rows(i).Cells(GC.MyGetDate).Value = bm.MyGetTime
        'G.Rows(i).Cells(GC.Line).Value = 0
    End Sub


    Private Sub btnPrintEditing_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintEditing.Click
        PrintPone(sender)
    End Sub

    Private Sub Code_LostFocus(sender As Object, e As RoutedEventArgs) Handles Code.LostFocus
        Code.Text = Code.Text.Trim
        Dim str As String = bm.ExecuteScalar("select Id from cases where Code='" & Code.Text & "'")
        If str <> txtID.Text Then
            txtID.Text = str
            txtID_LostFocus(Nothing, Nothing)
        End If
    End Sub

End Class
