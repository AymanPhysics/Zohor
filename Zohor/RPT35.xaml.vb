Imports System.Data
Imports Microsoft.Office.Interop
Imports System.IO

Public Class RPT35
    Dim bm As New BasicMethods
    Dim dt As New DataTable

    Public Flag As Integer = 0

    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        Select Case Flag
            Case 1
                rpt.Rpt = "CasesAll.rpt"
            Case 2
                rpt.Rpt = "AssistantCases.rpt"
            Case 3
                rpt.Rpt = "AccountMotionCases.rpt"
            Case 4
                rpt.Rpt = "BonusDetailed.rpt"
            Case 5
                rpt.Rpt = "BonusTotal.rpt"
            Case 6
                rpt.Rpt = "AssistantCasesTotal.rpt"
        End Select

        rpt.paraname = New String() {"@CaseTypeId", "@DoctorId", "@CompanyId", "@FromDate", "@ToDate", "Header", "@MyId"}
        rpt.paravalue = New String() {Val(CaseTypeId.SelectedValue), Val(DoctorId.Text), Val(CompanyId.Text), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title, 0}
        rpt.Show()

    End Sub

    Public Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({DoctorId, CompanyId})

        bm.FillCombo("CaseTypes", CaseTypeId, "", , True)

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        
        Select Case Flag
            Case 1
                FromDate.Visibility = Visibility.Hidden
                lblFromDate.Visibility = Visibility.Hidden
                ToDate.Visibility = Visibility.Hidden
                lblToDate.Visibility = Visibility.Hidden
            Case 2
                lblDoctorId.Visibility = Visibility.Hidden
                DoctorId.Visibility = Visibility.Hidden
                DoctorName.Visibility = Visibility.Hidden
                lblCompanyId.Visibility = Visibility.Hidden
                CompanyId.Visibility = Visibility.Hidden
                CompanyName.Visibility = Visibility.Hidden
            Case 4, 5
                lblCaseOutcomeId_Copy.Visibility = Visibility.Hidden
                CaseTypeId.Visibility = Visibility.Hidden
        End Select

    End Sub
    Private Sub LoadResource()


        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")
        Button2.SetResourceReference(ContentProperty, "View Report")


    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DoctorId.KeyDown, CompanyId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    
    Private Sub DoctorId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DoctorId.KeyUp
        bm.ShowHelp("Doctors", DoctorId, DoctorName, e, "select cast(Id as varchar(100)) Id,Name Name from Employees where Doctor=1")
    End Sub

    Private Sub CompanyId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CompanyId.KeyUp
        bm.ShowHelp("Companies", CompanyId, CompanyName, e, "select cast(Id as varchar(100)) Id,Name from Companies")
    End Sub

    Private Sub DoctorId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DoctorId.LostFocus
        bm.LostFocus(DoctorId, DoctorName, "select Name Name from Employees where Doctor=1 and Id=" & DoctorId.Text.Trim())
    End Sub

    Private Sub CompanyId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CompanyId.LostFocus
        bm.LostFocus(CompanyId, CompanyName, "select Name from Companies where Id=" & CompanyId.Text.Trim())
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

            Case 1
                lblDoctorId.Visibility = Visibility.Visible
                DoctorId.Visibility = Visibility.Visible
                DoctorName.Visibility = Visibility.Visible

                lblCompanyId.Visibility = Visibility.Hidden
                CompanyId.Visibility = Visibility.Hidden
                CompanyName.Visibility = Visibility.Hidden

            Case 2, 3
                lblDoctorId.Visibility = Visibility.Visible
                DoctorId.Visibility = Visibility.Visible
                DoctorName.Visibility = Visibility.Visible

                lblCompanyId.Visibility = Visibility.Hidden
                CompanyId.Visibility = Visibility.Hidden
                CompanyName.Visibility = Visibility.Hidden

            Case 4
                lblDoctorId.Visibility = Visibility.Hidden
                DoctorId.Visibility = Visibility.Hidden
                DoctorName.Visibility = Visibility.Hidden

                lblCompanyId.Visibility = Visibility.Visible
                CompanyId.Visibility = Visibility.Visible
                CompanyName.Visibility = Visibility.Visible

            Case 5
                lblDoctorId.Visibility = Visibility.Visible
                DoctorId.Visibility = Visibility.Visible
                DoctorName.Visibility = Visibility.Visible

                lblCompanyId.Visibility = Visibility.Visible
                CompanyId.Visibility = Visibility.Visible
                CompanyName.Visibility = Visibility.Visible

        End Select
    End Sub

End Class