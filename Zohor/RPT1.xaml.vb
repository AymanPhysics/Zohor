Imports System.Data

Public Class RPT1
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        'If Flag = 5 AndAlso Val(CaseId.Text) = 0 Then
        'bm.ShowMSG("Please, Select a Patient")
        'CaseId.Focus()
        'Return
        'End If

        bm.Addcontrol_MouseDoubleClick({CaseId})

        If EmpId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        If VisitingType.SelectedValue Is Nothing Then VisitingType.SelectedIndex = 0
        If UserId.SelectedValue Is Nothing Then UserId.SelectedIndex = 0
        If SerialType.SelectedValue Is Nothing Then SerialType.SelectedIndex = 0


        Dim rpt As New ReportViewer
        If EmpId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        If UserId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        rpt.paraname = New String() {"@EmpId", "@CaseId", "@CsId", "@VisitingType", "@FromDate", "@ToDate", "@FromId", "@ToId", "Header", "@UserId", "@FromSerialId", "@ToSerialId", "@Canceled", "@SerialType", "@All", "@IsReservations", "@IsServices", "@ServiceGroupId", "@ServiceTypeId", "@Returned", "@CompanyId", "@DepartmentId", "@FromHH", "@FromMM", "@ToHH", "@ToMM", "@ShowZeroValues"}
        rpt.paravalue = New String() {Val(EmpId.SelectedValue), Val(CaseId.Text), Val(UserId.SelectedValue.ToString()), Val(VisitingType.SelectedValue.ToString), FromDate.SelectedDate, ToDate.SelectedDate, Val(FromInvoice.Text), Val(ToInvoice.Text), CType(Parent, Page).Title, Val(UserId.SelectedValue), Val(FromSerialId.Text), Val(ToSerialId.Text), Canceled.SelectedIndex.ToString, SerialType.SelectedValue.ToString, 0, 1, 1, 0, 0, Returned.SelectedIndex.ToString, 0, 0, 0, 0, 0, 0, 0}


        Select Case Flag
            Case 1
                rpt.Rpt = "DailyReservations.rpt"
            Case 2
                rpt.Rpt = "DailyReservationsDetailed.rpt"
            Case 3
                rpt.Rpt = "ReservationsDepartments.rpt"
            Case 4
                rpt.Rpt = "ReservationsDoctors.rpt"
            Case 5
                rpt.Rpt = "ReservationsDepartments2.rpt"
            Case 6
                rpt.Rpt = "ReservationsDepartments2S1.rpt"
            Case 7
                rpt.Rpt = "ReservationsDepartments2S2.rpt"
            Case 8
                rpt.Rpt = "ReservationsDepartments2S3.rpt"
            Case 9
                rpt.Rpt = "ReservationsDepartments2S4.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

        If Flag = 1 Then
            Canceled.Visibility = Visibility.Hidden
            lblCanceled.Visibility = Visibility.Hidden
            Returned.Visibility = Visibility.Hidden
            lblReturned.Visibility = Visibility.Hidden
        ElseIf Flag = 2 Then
            lblUsername.Visibility = Visibility.Hidden
            UserId.Visibility = Visibility.Hidden
            
            lblFromSerialId.Visibility = Visibility.Hidden
            FromSerialId.Visibility = Visibility.Hidden
            lblToSerialId.Visibility = Visibility.Hidden
            ToSerialId.Visibility = Visibility.Hidden

            lblSerialType.Visibility = Visibility.Hidden
            SerialType.Visibility = Visibility.Hidden
            lblVisitingType.Visibility = Visibility.Hidden
            VisitingType.Visibility = Visibility.Hidden

            Canceled.Visibility = Visibility.Hidden
            lblCanceled.Visibility = Visibility.Hidden
            Returned.Visibility = Visibility.Hidden
            lblReturned.Visibility = Visibility.Hidden
        ElseIf Flag = 3 OrElse Flag = 4 OrElse Flag = 6 OrElse Flag = 7 OrElse Flag = 8 OrElse Flag = 9 Then
            lblDoctor.Visibility = Visibility.Hidden
            EmpId.Visibility = Visibility.Hidden
            lblPatient.Visibility = Visibility.Hidden
            CaseId.Visibility = Visibility.Hidden
            CaseName.Visibility = Visibility.Hidden

            lblUsername.Visibility = Visibility.Hidden
            UserId.Visibility = Visibility.Hidden
            lblFromResId.Visibility = Visibility.Hidden
            FromInvoice.Visibility = Visibility.Hidden
            lblToResId.Visibility = Visibility.Hidden
            ToInvoice.Visibility = Visibility.Hidden

            lblFromSerialId.Visibility = Visibility.Hidden
            FromSerialId.Visibility = Visibility.Hidden
            lblToSerialId.Visibility = Visibility.Hidden
            ToSerialId.Visibility = Visibility.Hidden

            lblSerialType.Visibility = Visibility.Hidden
            SerialType.Visibility = Visibility.Hidden
            lblVisitingType.Visibility = Visibility.Hidden
            VisitingType.Visibility = Visibility.Hidden
        End If

        bm.FillCombo("SerialTypes", SerialType, "")

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where SystemUser='1' and Stopped='0' union select 0 Id,'-' Name order by Name", UserId)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Doctor='1' and Stopped='0' union select 0 Id,'-' Name order by Name", EmpId)
        EmpId.SelectedValue = Md.UserName
        If EmpId.SelectedIndex < 0 Then EmpId.SelectedIndex = 0
        Canceled.SelectedIndex = 2
        Returned.SelectedIndex = 2
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Private Sub SerialType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles SerialType.SelectionChanged
        Try
            bm.FillCombo("VisitingTypes", VisitingType, "where (SerialId=" & SerialType.SelectedValue.ToString & " or " & SerialType.SelectedValue.ToString & "=0)")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp
        If bm.ShowHelpCases(CaseId, CaseName, e) Then
            CaseID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select EnName Name from Cases where Id=" & CaseId.Text.Trim())
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")
        lblVisitingType.SetResourceReference(ContentProperty, "VisitingType")
        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")
        lblDoctor.SetResourceReference(ContentProperty, "Doctor")
        lblFromResId.SetResourceReference(ContentProperty, "From Res. Id")
        lblToResId.SetResourceReference(ContentProperty, "To Res. Id")
        lblPatient.SetResourceReference(ContentProperty, "Patient")
        lblUsername.SetResourceReference(ContentProperty, "Username")
        lblFromSerialId.SetResourceReference(ContentProperty, "From Serial Id")
        lblToSerialId.SetResourceReference(ContentProperty, "To Serial Id")
        lblCanceled.SetResourceReference(ContentProperty, "Canceled")
        lblReturned.SetResourceReference(ContentProperty, "Returned")
        lblSerialType.SetResourceReference(ContentProperty, "Serial Type")

        bm.ResetComboboxContent(Canceled)
        bm.ResetComboboxContent(Returned)
    End Sub

End Class