Imports System.Data
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms.Integration

Public Class Cases2
    Public TableName As String = "Cases"
    Public SubId As String = "Id"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    Dim WithEvents BackgroundWorker1 As New BackgroundWorker
    WithEvents G0 As New MyGrid
    WithEvents G As New MyGrid
    WithEvents G2 As New MyGrid

    Public Sub Cases2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        If bm.TestIsLoaded(Me, True) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        LoadWFH0()
        LoadWFH()
        LoadWFH2()

        bm.Fields = New String() {SubId, "Name", "SSN", "Address", "DateOfBirth", "JobId", "Notes", "Manager", "BankAccount", "HomePhone", "Mobile", "Password", "EnName", "mm", "GeneralManager", "HasAttendance", "Accountant", "Board", "Cashier", "Waiter", "Deliveryman", "Occupation", "MenstrualH", "ObstetricH", "FamilyH", "HusbandName", "HusbandAge", "HusbandOccupation", "Email", "DateOfBirthCount", "MarriageDate", "MarriageDateCount"}
        bm.control = New Control() {txtID, ArName, SSN, Address, DateOfBirth, JobId, Notes, Manager, BankAccount, HomePhone, Mobile, Password, EnName, mm, GeneralManager, HasAttendance, Accountant, Board, Cashier, Waiter, Deliveryman, Occupation, MenstrualH, ObstetricH, FamilyH, HusbandName, HusbandAge, HusbandOccupation, Email, DateOfBirthCount, MarriageDate, MarriageDateCount}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(Nothing, Nothing)
        If Not Md.MyProjectType = ProjectType.X Then
            TabControl1.Visibility = Visibility.Hidden
        End If


        Try
            Visibility = Visibility.Hidden
            Visibility = Visibility.Visible
            
            Dim c0 = G0.Parent
            G0.Parent = Nothing
            G0.Parent = c0

            Dim c = G.Parent
            G.Parent = Nothing
            G.Parent = c

            Dim c2 = G2.Parent
            G2.Parent = Nothing
            G2.Parent = c2

        Catch ex As Exception
        End Try
    End Sub


    Structure GC0
        Shared Id As String = "Id"
        Shared Name As String = "Name"
    End Structure


    Private Sub LoadWFH0()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH0.Child = G0

        G0.Columns.Clear()
        G0.ForeColor = System.Drawing.Color.DarkBlue
        G0.Columns.Add(GC0.Id, "index")
        G0.Columns.Add(GC0.Name, "Notes")

        G0.Columns(GC0.Id).FillWeight = 70
        G0.Columns(GC0.Name).FillWeight = 300
        G0.Columns(GC0.Id).ReadOnly = True

        AddHandler G0.RowsAdded, AddressOf G0_RowsAdded
    End Sub


    Structure GC
        Shared Daydate As String = "Daydate"
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Daydate, "Daydate")
        G.Columns.Add(GC.Notes, "Notes")

        G.Columns(GC.Daydate).FillWeight = 70
        G.Columns(GC.Notes).FillWeight = 300

    End Sub

    Structure GC2
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH2()
        'WFH2.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH2.Foreground = New SolidColorBrush(Colors.Red)
        WFH2.Child = G2

        G2.Columns.Clear()
        G2.ForeColor = System.Drawing.Color.DarkBlue
        G2.Columns.Add(GC2.Notes, "Risk")

    End Sub


    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)

        'bm.GetImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        JobId_LostFocus(Nothing, Nothing)

        Dim dt As DataTable = bm.ExecuteAdapter("select * from Informations where CaseId=" & txtID.Text)
        G0.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G0.Rows.Add()
            G0.Rows(i).Cells(GC0.Id).Value = dt.Rows(i)("Id").ToString
            G0.Rows(i).Cells(GC0.Name).Value = dt.Rows(i)("Name").ToString
        Next
        G0.CurrentCell = G0.Rows(G0.Rows.Count - 1).Cells(0)

        dt = bm.ExecuteAdapter("select * from CaseDetails where CaseId=" & txtID.Text)
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Daydate).Value = dt.Rows(i)("Daydate").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
        Next
        G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(0)


        dt = bm.ExecuteAdapter("select * from CaseRisk where CaseId=" & txtID.Text)
        G2.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G2.Rows.Add()
            G2.Rows(i).Cells(GC2.Notes).Value = dt.Rows(i)("Notes").ToString
        Next
        G2.CurrentCell = G2.Rows(G2.Rows.Count - 1).Cells(0)

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False
        JobId.Text = Val(JobId.Text)
        G0.EndEdit()
        G.EndEdit()
        G2.EndEdit()

        If ArName.Text.Trim = "" Then Return

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        bm.SaveGrid(G0, "Informations", New String() {"CaseId"}, New String() {txtID.Text}, New String() {"Id", "Name"}, New String() {GC0.Id, GC0.Name}, New VariantType() {VariantType.String, VariantType.String}, New String() {})

        bm.SaveGrid(G, "CaseDetails", New String() {"CaseId"}, New String() {txtID.Text}, New String() {"DayDate", "Notes"}, New String() {GC.Daydate, GC.Notes}, New VariantType() {VariantType.String, VariantType.String}, New String() {})

        bm.SaveGrid(G2, "CaseRisk", New String() {"CaseId"}, New String() {txtID.Text}, New String() {"Notes"}, New String() {GC2.Notes}, New VariantType() {VariantType.String}, New String() {})


        'bm.SaveImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True
        
    End Sub
    Function TestNames() As Boolean

        ArName.Text = ArName.Text.Trim
        EnName.Text = EnName.Text.Trim
        While ArName.Text.Contains("  ")
            ArName.Text = ArName.Text.Replace("  ", " ")
        End While
        While EnName.Text.Contains("  ")
            EnName.Text = EnName.Text.Replace("  ", " ")
        End While

        Dim Ar() As String
        Ar = ArName.Text.Split(" ")
        Dim En() As String
        En = EnName.Text.Split(" ")
        If Ar.Length <> En.Length Then
            bm.ShowMSG("Arabic Name Length must be EQUALE English Name Length")
            ArName.Focus()
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
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        'TreeView1.Items.Clear()
        bm.ClearControls()
        'bm.SetNoImage(Image1, True)
        JobName.Clear()

        G0.Rows.Clear()
        G.Rows.Clear()
        G2.Rows.Clear()
        ArName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        ArName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from CaseAttachments where CaseId='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from Informations where CaseId='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from CaseDetails where CaseId='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from CaseRisk where CaseId='" & txtID.Text.Trim & "'")
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
            ClearControls()
            ArName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        ArName.SelectAll()
        ArName.Focus()
        ArName.SelectAll()
        ArName.Focus()
        'arName.Text = dt(0)("Name")
    End Sub

    Private Sub JobId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles JobId.KeyUp
        bm.ShowHelp("Jobs", JobId, JobName, e, "select cast(Id as varchar(100)) Id,Name from Jobs", "Jobs")
    End Sub

    Private Sub txtID_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelpCases(txtID, ArName, e) Then
            txtID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub


    

    Dim lop As Boolean = False

    Private Sub JobId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles JobId.LostFocus
        bm.LostFocus(JobId, JobName, "select Name from Jobs where Id=" & JobId.Text.Trim())
    End Sub

    Dim lop2 As Boolean = False
    Private Sub ArName_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ArName.LostFocus
        If lop2 Then Return
        lop2 = True
        ArName.Text = ArName.Text.Trim
        While ArName.Text.Contains("  ")
            ArName.Text = ArName.Text.Replace("  ", " ")
        End While
        Dim s() As String
        s = ArName.Text.Split(" ")
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


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblId.SetResourceReference(ContentProperty, "Id")
        lblAddress.SetResourceReference(ContentProperty, "Address")
        lblArName.SetResourceReference(ContentProperty, "ArName")
        lblDateOfBirth.SetResourceReference(ContentProperty, "DateOfBirth")
        lblEnName.SetResourceReference(ContentProperty, "EnName")
        lblHomePhone.SetResourceReference(ContentProperty, "HomePhone")
        lblJobId.SetResourceReference(ContentProperty, "JobId")
        lblMobile.SetResourceReference(ContentProperty, "Mobile")
        lblNotes.SetResourceReference(ContentProperty, "Notes")

    End Sub

    Private Sub btnSave_Copy2_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy2.Click
        DontClear = True
        btnSave_Click(Nothing, Nothing)
        DontClear = False
        If Not AllowSave Then Return

        Dim frm As New MyWindow With {.Title = "Gynecology", .WindowState = WindowState.Maximized}
        Dim c As New Gynecology
        c.MyCase = Val(txtID.Text)
        c.MyCaseName = EnName.Text
        frm.Content = c
        frm.ShowDialog()
    End Sub


    Private Sub btnSave_Copy3_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy3.Click
        DontClear = True
        btnSave_Click(Nothing, Nothing)
        DontClear = False
        If Not AllowSave Then Return

        Dim frm As New MyWindow With {.Title = "InFertility", .WindowState = WindowState.Maximized}
        Dim c As New InFertility
        c.MyCase = Val(txtID.Text)
        c.MyCaseName = EnName.Text
        frm.Content = c
        frm.ShowDialog()
        txtID_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub btnSave_Copy5_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy5.Click
        DontClear = True
        btnSave_Click(Nothing, Nothing)
        DontClear = False
        If Not AllowSave Then Return

        Dim frm As New MyWindow With {.Title = "ANTENATAL", .WindowState = WindowState.Maximized}
        Dim c As New ANTENATAL
        c.MyCase = Val(txtID.Text)
        c.MyCaseName = EnName.Text
        frm.Content = c
        frm.ShowDialog()
    End Sub


    Dim boolcalc As Boolean = False
    Private Sub DateOfBirthCount_TextChanged(sender As Object, e As TextChangedEventArgs) Handles DateOfBirthCount.TextChanged
        If boolcalc Then Return
        boolcalc = True
        DateOfBirth.SelectedDate = Now.Date.AddYears(-Val(DateOfBirthCount.Text))
        boolcalc = False
    End Sub
    Private Sub DateOfBirth_LostFocus(sender As Object, e As System.EventArgs) Handles DateOfBirth.LostFocus
        If boolcalc Then Return
        boolcalc = True
        DateOfBirthCount.Text = Now.Date.Year - DateOfBirth.SelectedDate.Value.Year
        boolcalc = False
    End Sub


    Private Sub MarriageDateCount_TextChanged(sender As Object, e As TextChangedEventArgs) Handles MarriageDateCount.TextChanged
        If boolcalc Then Return
        boolcalc = True
        MarriageDate.SelectedDate = Now.Date.AddYears(-Val(MarriageDateCount.Text))
        boolcalc = False
    End Sub
    Private Sub MarriageDate_LostFocus(sender As Object, e As System.EventArgs) Handles MarriageDate.LostFocus
        If boolcalc Then Return
        boolcalc = True
        MarriageDateCount.Text = Now.Date.Year - MarriageDate.SelectedDate.Value.Year
        boolcalc = False
    End Sub

    Private Sub G0_RowsAdded(sender As Object, e As Forms.DataGridViewRowsAddedEventArgs)
        For i As Integer = 0 To G0.Rows.Count - 1
            G0.Rows(i).Cells(GC0.Id).Value = i + 1
        Next
    End Sub


End Class
