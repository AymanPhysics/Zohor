Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine

Public Class RPTCustomers
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public MyFlag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer

        rpt.paraname = New String() {"@CityId", "@CountryId", "@DegreeId", "@SpecialtyId", "@SponsorId", "@SubSpecialtyId", "@TitleId", "@UniversityId", "Header"}
        rpt.paravalue = New String() {Val(CityId.Text), Val(CountryId.Text), Val(DegreeId.Text), Val(SpecialtyId.Text), Val(SponsorId.Text), Val(SubSpecialtyId.Text), Val(TitleId.Text), Val(UniversityId.Text), CType(Parent, Page).Title}
        If Flag = 1 Then
            rpt.Rpt = "PLCustomers.rpt"
        Else

        End If

        rpt.Show()


    End Sub


    Dim CertificateTop As Integer = 0
    Dim CertificateLeft As Integer = 0

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.Addcontrol_MouseDoubleClick({CityId, CountryId, DegreeId, SpecialtyId, SponsorId, SubSpecialtyId, TitleId, UniversityId})


    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CityId.KeyDown, CountryId.KeyDown, DegreeId.KeyDown, SpecialtyId.KeyDown, SponsorId.KeyDown, SubSpecialtyId.KeyDown, TitleId.KeyDown, UniversityId.KeyDown
        bm.MyKeyPress(sender, e)
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




    Private Sub SponsorId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SponsorId.KeyUp
        If bm.ShowHelp("Sponsors", SponsorId, SponsorName, e, "select cast(Id as varchar(100)) Id,Name from Sponsors", "Sponsors") Then
            SponsorId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SponsorId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SponsorId.LostFocus
        bm.LostFocus(SponsorId, SponsorName, "select Name from Sponsors where Id=" & SponsorId.Text.Trim())
    End Sub




End Class