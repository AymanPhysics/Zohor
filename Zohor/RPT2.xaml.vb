Imports System.Data

Public Class RPT2
    Public MyLinkFile As Integer = 0
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Flag <> 3 AndAlso MainLinkFile.Visibility = Visibility.Visible AndAlso MainLinkFile.IsEnabled AndAlso MainLinkFile.SelectedIndex = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblMainLinkFile.Content)
            MainLinkFile.Focus()
            Return
        End If

        'If Not Md.HideSubAccNo AndAlso MyLinkFile = 0 AndAlso Val(SubAccNo.Text) = 0 AndAlso SubAccNo.IsEnabled Then
        '    bm.ShowMSG("برجاء تحديد الحساب الفرعى")
        '    SubAccNo.Focus()
        '    Return
        'End If
        If MyLinkFile > 0 AndAlso Val(SubAccNo.Text) = 0 AndAlso SubAccNo.Visibility = Visibility.Visible Then
            bm.ShowMSG("برجاء تحديد الكود")
            SubAccNo.Focus()
            Return
        End If

        If MainAccNo.Text.Trim <> "" AndAlso Val(MainAccNo.Text) = Val(bm.ExecuteScalar("select YearProfitAccNo from Statics")) Then
            CType(sender, Button).IsEnabled = False
            bm.ExecuteNonQuery("exec AccountEnd 2, 3, 0, '" & bm.ToStrDate(New DateTime(ToDate.SelectedDate.Value.Year, 1, 1, 0, 0, 0)) & "', '" & bm.ToStrDate(ToDate.SelectedDate) & "', 0, 0")
            CType(sender, Button).IsEnabled = True
        End If

        Dim rpt As New ReportViewer
        Dim RPTFlag1 As Integer = 2
        RPTFlag1 = IIf(MyLinkFile = 1, 3, RPTFlag1)
        RPTFlag1 = IIf(MyLinkFile = 13, 4, RPTFlag1)
        RPTFlag1 = IIf(MyLinkFile = 15, 11, RPTFlag1)

        If ToMainAccNo.Text.Trim = "" Then
            ToMainAccNo.Text = MainAccNo.Text
        End If


        rpt.paraname = New String() {"@MainAccNo", "MainAccName", "@SubAccNo", "SubAccName", "@FromDate", "@ToDate", "Header", "@Detailed", "@DetailedInvoice", "@LinkFile", "@ToId", "@RPTFlag1", "@RPTFlag2", "@ActiveOnly", "@HasBalOnly", "@WindowId", "@CostCenterId", "@CostCenterSubId", "@FromMainAccNo", "@ToMainAccNo"}
        rpt.paravalue = New String() {MainAccNo.Text, MainAccName.Text, Val(SubAccNo.Text), SubAccName.Text, FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title.Trim & " " & IIf(MainLinkFile.SelectedIndex > 0, MainLinkFile.Text, ""), IIf(Detailed.IsChecked, 1, 0), IIf(DetailedInvoice.IsChecked, 1, 0), MyLinkFile, Val(SubAccNo.Text), RPTFlag1, 0, ActiveOnly.SelectedIndex, HasBalOnly.SelectedIndex, Val(WindowId.SelectedValue), Val(CostCenterId.SelectedValue), Val(CostCenterSubId.SelectedValue), MainAccNo.Text, ToMainAccNo.Text}
        Select Case Flag
            Case 1
                rpt.Rpt = "AccountMotion.rpt"
                If Detailed.IsChecked AndAlso (MyLinkFile = 5 OrElse MyLinkFile = 6) Then rpt.Rpt = "AccountMotionBanks.rpt"
                If Detailed.IsChecked AndAlso Md.MyProjectType = ProjectType.X Then rpt.Rpt = "AccountMotion4.rpt"
                If Md.MyProjectType = ProjectType.X Then rpt.Rpt = "AccountMotionCurrency.rpt"
                If DetailedInvoice.IsChecked Then
                    If Md.MyProjectType = ProjectType.X Then
                        rpt.Rpt = "AccountMotion3.rpt"
                    Else
                        rpt.Rpt = "AccountMotion2.rpt"
                    End If
                End If
            Case 2
                rpt.Rpt = "AccountMotionBanks2.rpt"
            Case 3
                rpt.Rpt = "Assistant.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({MainAccNo, SubAccNo, ToMainAccNo})
        bm.FillCombo("LinkFile", MainLinkFile, "", , True)

        bm.FillCombo("Fn_AllWindows()", WindowId, "", , True)
        bm.FillCombo("CostCenters", CostCenterId, "WHERE SubType=1", , True)
        bm.FillCombo("CostCenterSubs", CostCenterSubId, "WHERE SubType=1", , True)

        ActiveOnly.SelectedIndex = 2
        HasBalOnly.SelectedIndex = 2

        lblActiveOnly.Visibility = Visibility.Hidden
        ActiveOnly.Visibility = Visibility.Hidden
        lblHasBalOnly.Visibility = Visibility.Hidden
        HasBalOnly.Visibility = Visibility.Hidden

        If Flag = 2 Then
            Detailed.Visibility = Visibility.Hidden
        ElseIf Flag = 3 Then
            Detailed.Visibility = Visibility.Hidden
            DetailedInvoice.Visibility = Visibility.Hidden

            lblActiveOnly.Visibility = Visibility.Visible
            ActiveOnly.Visibility = Visibility.Visible
            lblHasBalOnly.Visibility = Visibility.Visible
            HasBalOnly.Visibility = Visibility.Visible

            lblSubAcc.Visibility = Visibility.Collapsed
            SubAccNo.Visibility = Visibility.Collapsed
            SubAccName.Visibility = Visibility.Collapsed
        End If

        If MyLinkFile = -1 Then
            MyLinkFile = 0
            MainLinkFile.SelectedIndex = 0
            MainLinkFile.Visibility = Visibility.Hidden
            lblMainLinkFile.Visibility = Visibility.Hidden
        ElseIf MyLinkFile = 0 Then
            lblMainAcc.Visibility = Visibility.Hidden
            MainAccNo.Visibility = Visibility.Hidden
            MainAccName.Visibility = Visibility.Hidden

            MainLinkFile.SelectedIndex = 0
            MainLinkFile.Visibility = Visibility.Visible
            lblMainLinkFile.Visibility = Visibility.Visible
        Else
            MainLinkFile.SelectedValue = MyLinkFile
            MainLinkFile.Visibility = Visibility.Hidden
            lblMainLinkFile.Visibility = Visibility.Hidden
        End If

        'If Flag = 2 OrElse (Md.MyProjectType <> ProjectType.NawarGroup AndAlso Md.MyProjectType <> ProjectType.Hamido AndAlso Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X) Then
        '    MainLinkFile.Visibility = Visibility.Hidden
        '    lblMainLinkFile.Visibility = Visibility.Hidden
        'End If

        If MyLinkFile > 0 And Flag <> 3 Then
            lblMainAcc.Visibility = Visibility.Collapsed
            MainAccNo.Visibility = Visibility.Collapsed
            MainAccName.Visibility = Visibility.Collapsed
            'Select Case MyLinkFile
            '    Case 1
            '        lblSubAcc.SetResourceReference(ContentProperty, "Customer")
            '    Case 2
            '        lblSubAcc.SetResourceReference(ContentProperty, "Supplier")
            '    Case 3
            '        lblSubAcc.SetResourceReference(ContentProperty, "Debit")
            '    Case 4
            '        lblSubAcc.SetResourceReference(ContentProperty, "Credit")
            '    Case 5
            '        lblSubAcc.SetResourceReference(ContentProperty, "Safe")
            '    Case 6
            '        lblSubAcc.SetResourceReference(ContentProperty, "Bank")
            '    Case 7
            '        lblSubAcc.SetResourceReference(ContentProperty, "Seller")
            'End Select
        End If
        If Flag = 3 Then
            lblMainAcc.Visibility = Visibility.Visible
            MainAccNo.Visibility = Visibility.Visible
            MainAccName.Visibility = Visibility.Visible
        End If
        If Md.MyProjectType = ProjectType.X Then
            Detailed.IsChecked = True
        End If

        If Flag <> 1 AndAlso Flag <> 3 Then
            DetailedInvoice.Visibility = Visibility.Hidden
        End If

        If MyLinkFile = 5 Then
            SubAccNo.Text = Md.DefaultSave
            SubAccNo_LostFocus(Nothing, Nothing)
        ElseIf MyLinkFile = 6 Then
            SubAccNo.Text = Md.DefaultBank
            SubAccNo_LostFocus(Nothing, Nothing)
        End If


        If Flag = 1 AndAlso Md.MyProjectType = ProjectType.X Then
            lblMainAcc.Content = "من حساب"

            lblSubAcc.Visibility = Visibility.Hidden
            SubAccNo.Visibility = Visibility.Hidden
            SubAccName.Visibility = Visibility.Hidden
        Else
            lblToMainAcc.Visibility = Visibility.Hidden
            ToMainAccNo.Visibility = Visibility.Hidden
            ToMainAccName.Visibility = Visibility.Hidden

            WindowId.Visibility = Visibility.Hidden
            lblWindowId.Visibility = Visibility.Hidden
            CostCenterId.Visibility = Visibility.Hidden
            lblCostCenterId.Visibility = Visibility.Hidden
            CostCenterSubId.Visibility = Visibility.Hidden
            lblCostCenterSubId.Visibility = Visibility.Hidden
        End If



        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate

    End Sub

    Dim lop As Boolean = False
    Private Sub SubAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubAccNo.LostFocus
        If lop OrElse SubAccNo.Visibility <> Visibility.Visible Then Return
        If MyLinkFile = 0 Then
            If Val(MainAccNo.Text) = 0 Or Not SubAccNo.IsEnabled Then
                SubAccNo.Clear()
                SubAccName.Clear()
                Return
            End If
            dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')")
            bm.LostFocus(SubAccNo, SubAccName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo.Text.Trim() & " and AccNo='" & MainAccNo.Text & "'")
        Else
            If Val(SubAccNo.Text) = 0 Then
                SubAccNo.Clear()
                SubAccName.Clear()
                Return
            End If
            dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
            bm.LostFocus(SubAccNo, SubAccName, "select Name from Fn_EmpPermissions(" & MyLinkFile & "," & Md.UserName & ") where Id=" & SubAccNo.Text.Trim(), , True)
            If MyLinkFile > 0 Then
                bm.LostFocus(SubAccNo, MainAccNo, "select AccNo from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo.Text.Trim())
                lop = True
                MainAccNo_LostFocus(Nothing, Nothing)
                lop = False
            End If

        End If
    End Sub
    Private Sub SubAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SubAccNo.KeyUp
        If MyLinkFile = 0 Then
            dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')")
            If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & dt.Rows(0)("Id") & "," & Md.UserName & ") where AccNo='" & MainAccNo.Text & "'") Then
                SubAccNo_LostFocus(Nothing, Nothing)
            End If
        Else
            dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
            If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & MyLinkFile & "," & Md.UserName & ")") Then
                SubAccNo_LostFocus(Nothing, Nothing)
            End If
        End If
    End Sub


    Private Sub MainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo.LostFocus
        bm.AccNoLostFocus(MainAccNo, MainAccName, , MyLinkFile, ,, True)

        SubAccNo.IsEnabled = MainAccNo.Visibility <> Visibility.Visible OrElse MyLinkFile > 0 OrElse bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')").Rows.Count > 0
        SubAccNo_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles MainAccNo.KeyUp
        bm.AccNoShowHelp(MainAccNo, MainAccName, e, , MyLinkFile, ,, True)
    End Sub


    Private Sub ToMainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToMainAccNo.LostFocus
        bm.AccNoLostFocus(ToMainAccNo, ToMainAccName, , MyLinkFile, ,, True)
    End Sub

    Private Sub ToMainAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ToMainAccNo.KeyUp
        bm.AccNoShowHelp(ToMainAccNo, ToMainAccName, e, , MyLinkFile, ,, True)
    End Sub


    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")
        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")
        'lblMainAcc.SetResourceReference(ContentProperty, "Main AccNo")
        lblSubAcc.SetResourceReference(ContentProperty, "Sub AccNo")
        Detailed.SetResourceReference(CheckBox.ContentProperty, "Detailed")
        DetailedInvoice.SetResourceReference(CheckBox.ContentProperty, "Detailed With Invoice")
    End Sub

    Private Sub Detailed_Checked(sender As Object, e As RoutedEventArgs) Handles DetailedInvoice.Checked, Detailed.Checked
        If Flag = 3 Then Return
        If sender Is DetailedInvoice And Detailed.IsChecked = True Then Detailed.IsChecked = False
        If sender Is Detailed And DetailedInvoice.IsChecked = True Then DetailedInvoice.IsChecked = False
    End Sub

    Private Sub MainLinkFile_LostFocus(sender As Object, e As RoutedEventArgs) Handles MainLinkFile.LostFocus
        MyLinkFile = MainLinkFile.SelectedValue
        MainAccNo_LostFocus(Nothing, Nothing)
        SubAccNo_LostFocus(Nothing, Nothing)
    End Sub



End Class