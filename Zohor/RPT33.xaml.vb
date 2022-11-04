Imports System.Data

Public Class RPT33
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Dim dt As New DataTable
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@CaseId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(CaseId.Text), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}


        Select Case Flag
            Case 1
                rpt.Rpt = "KidneysWashMotion0.rpt"
            Case 2
                rpt.Rpt = "KidneysWashMotionItems.rpt"
            Case 3
                If Val(bm.ExecuteScalar("select dbo.GetCaseStatus(" & Val(CaseId.Text) & ")")) = 1 Then
                    bm.ShowMSG("هذا المريض موجود بالفعل")
                    Return
                End If
                bm.ExecuteNonQuery("SetCaseStatus", {"CaseId", "UserName", "InOut"}, {Val(CaseId.Text), Md.UserName, 1})
                bm.ShowMSG("تم تسجيل دخول المريض بنجاح")
                Return
            Case 4
                If Val(bm.ExecuteScalar("select dbo.GetCaseStatus(" & Val(CaseId.Text) & ")")) <> 1 Then
                    bm.ShowMSG("هذا المريض غير موجود بالمستشفى")
                    Return
                End If
                bm.ExecuteNonQuery("SetCaseStatus", {"CaseId", "UserName", "InOut"}, {Val(CaseId.Text), Md.UserName, 2})

                dt = bm.ExecuteAdapter("select RoomId,Id from RoomsData where CaseId=" & Val(CaseId.Text) & " and IsCurrent=1 ")
                If dt.Rows.Count > 0 Then
                    bm.ExecuteNonQuery("ExitCase", {"UserName", "RoomId", "Id"}, {Md.UserName, dt.Rows(0)("RoomId"), dt.Rows(0)("Id")})
                End If

                bm.ShowMSG("تم تسجيل خروج المريض بنجاح")
                Return
            Case 5
                bm.ExecuteNonQuery("UpdateDocNoAll", {"FromDate", "ToDate"}, {bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate)})
                bm.ShowMSG("تمت العملية بنجاح")
                Return
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({CaseId})

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate


        If Flag = 3 OrElse Flag = 4 Then
            lblFromDate.Visibility = Visibility.Hidden
            FromDate.Visibility = Visibility.Hidden
            lblToDate.Visibility = Visibility.Hidden
            ToDate.Visibility = Visibility.Hidden
        ElseIf Flag = 5 Then
            FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
            lblPatient.Visibility = Visibility.Hidden
            CaseId.Visibility = Visibility.Hidden
            CaseName.Visibility = Visibility.Hidden
        End If

        If Flag = 3 Then
            Button2.Content = "دخول المريض"
        ElseIf Flag = 4 Then
            Button2.Content = "خروج المريض"
        ElseIf Flag = 5 Then
            Button2.Content = "احتساب"
        End If
    End Sub

    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp

        Select Case Flag
            Case 1, 2
                If bm.ShowHelpCases2(CaseId, CaseName, e) Then
                    CaseID_LostFocus(sender, Nothing)
                End If
            Case Else
                If bm.ShowHelpCases(CaseId, CaseName, e, Md.MyProjectType = ProjectType.Zohor) Then
                    CaseID_LostFocus(sender, Nothing)
                End If
        End Select
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        Select Case Flag
            Case 1, 2
                bm.LostFocus(CaseId, CaseName, "select Name from Cases2 where Id=" & CaseId.Text.Trim())
            Case Else
                bm.LostFocus(CaseId, CaseName, "select Name from Cases where Id=" & CaseId.Text.Trim() & IIf(Md.MyProjectType = ProjectType.Zohor, " and InOut=1", ""))
        End Select
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")
        lblPatient.SetResourceReference(ContentProperty, "Patient")

    End Sub

End Class