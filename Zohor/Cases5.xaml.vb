Imports System.Data
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms.Integration

Public Class Cases5
    Public TableName As String = "Cases"
    Public SubId As String = "Id"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Public Sub Cases2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        If bm.TestIsLoaded(Me, True) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()

        bm.FillCombo("Gender", Gender, "", , True)

        bm.Fields = New String() {SubId, "Name", "SSN", "CityId", "DateOfBirth", "Notes", "Manager", "BankAccount", "HomePhone", "Mobile", "Password", "EnName", "mm", "GeneralManager", "HasAttendance", "Accountant", "Board", "Cashier", "Waiter", "Deliveryman"}
        bm.control = New Control() {txtID, ArName, SSN, CityId, DateOfBirth, Notes, Manager, BankAccount, HomePhone, Mobile, Password, EnName, mm, GeneralManager, HasAttendance, Accountant, Board, Cashier, Waiter, Deliveryman}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(Nothing, Nothing)
        
         
    End Sub 

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        CityId_LostFocus(Nothing, Nothing)
        DateOfBirth_SelectedDateChanged(Nothing, Nothing)
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
        CityId.Text = Val(CityId.Text) 

        If ArName.Text.Trim = "" Then Return

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
         
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
        CityName.Clear()
        DateOfBirth_SelectedDateChanged(Nothing, Nothing)
         
        ArName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        ArName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'") 
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

    Private Sub CityId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CityId.KeyUp
        bm.ShowHelp("Cities", CityId, CityName, e, "select cast(Id as varchar(100)) Id,Name from Cities where CountryId=1", "Cities", {"CountryId"}, {1})
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

    Private Sub CityId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CityId.LostFocus
        bm.LostFocus(CityId, CityName, "select Name from Cities where CountryId=1 and Id=" & CityId.Text.Trim())
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
        lblArName.SetResourceReference(ContentProperty, "ArName")
        lblDateOfBirth.SetResourceReference(ContentProperty, "DateOfBirth")
        lblEnName.SetResourceReference(ContentProperty, "EnName")
        lblHomePhone.SetResourceReference(ContentProperty, "HomePhone")
        lblCityId.SetResourceReference(ContentProperty, "CityId")
        lblMobile.SetResourceReference(ContentProperty, "Mobile")
        lblNotes.SetResourceReference(ContentProperty, "Notes")

    End Sub
      
 


    Dim lopD As Boolean = False
    Private Sub DateOfBirth_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DateOfBirth.SelectedDateChanged
        If lopD Then Return
        lopD = True
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
End Class
