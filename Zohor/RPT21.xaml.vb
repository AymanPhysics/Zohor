Imports System.Data

Public Class RPT21
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Detailed As Integer = 1
    Dim dt As New DataTable

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@BankCash_G2TypeId", "@MainLinkFile", "@BankId", "@FromDate", "@ToDate", "@LinkFile", "@SubAccNo", "@CostCenterId", "@FromInvoiceNo", "@ToInvoiceNo", "Header", "@Flag", "@CurrencyId", "@EmpId"}
        rpt.paravalue = New String() {BankCash_G2TypeId.SelectedValue, MainLinkFile.SelectedValue, Val(BankId.Text), FromDate.SelectedDate, ToDate.SelectedDate, LinkFile.SelectedValue, Val(SubAccNo.Text), Val(CostCenterId.Text), Val(FromInvoice.Text), Val(ToInvoice.Text), CType(Parent, Page).Title, Flag, Val(CurrencyId.SelectedValue), Md.UserName}
        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            rpt.Rpt = "BankCash_G2.rpt"
            If Detailed = 2 Then
                rpt.Rpt = "DeletedBankCash_G2.rpt"
            End If
        Else
            rpt.Rpt = "BankCash_G.rpt"
        End If

        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.FillCombo("Currencies", CurrencyId, "", , True)

        'bm.FillCombo("BankCash_G2Types", BankCash_G2TypeId, " where Flag=" & Flag, , True)
        bm.FillCombo("GetEmpBankCash_G2Types @Flag=" & Flag & ",@EmpId=" & Md.UserName & "", BankCash_G2TypeId)


        bm.FillCombo("LinkFile", MainLinkFile, "", , True)
        bm.FillCombo("LinkFile", LinkFile, "", , True)

        bm.Addcontrol_MouseDoubleClick({BankId, SubAccNo, CostCenterId})
        If Not Md.ShowCurrency Then
            lblCurrencyId.Visibility = Visibility.Hidden
            CurrencyId.Visibility = Visibility.Hidden
        End If
        If Not (Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X) Then
            lblBankCash_G2TypeId.Visibility = Visibility.Hidden
            BankCash_G2TypeId.Visibility = Visibility.Hidden
        End If
        If Not Md.ShowCostCenter Then
            lblCostCenterId.Visibility = Visibility.Hidden
            CostCenterId.Visibility = Visibility.Hidden
            CostCenterName.Visibility = Visibility.Hidden
        End If

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1,1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub
    Private Sub SubAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubAccNo.LostFocus

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & LinkFile.SelectedValue)
        If dt.Rows.Count = 0 Then
            SubAccNo.Clear()
            SubAccName.Clear()
            Return
        End If
        bm.LostFocus(SubAccNo, SubAccName, "select Name from Fn_EmpPermissions(" & LinkFile.SelectedValue & "," & Md.UserName & ") where Id=" & SubAccNo.Text.Trim())
    End Sub
    Private Sub SubAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SubAccNo.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & LinkFile.SelectedValue)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & LinkFile.SelectedValue & "," & Md.UserName & ")") Then
            SubAccNo_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub LinkFile_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles LinkFile.LostFocus
        SubAccNo_LostFocus(Nothing, Nothing)
    End Sub


    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")
        'lblFromId.SetResourceReference(ContentProperty, "From Id")
        'lblToId.SetResourceReference(ContentProperty, "To Id")
        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")
        'lblLinkFile.SetResourceReference(ContentProperty, "LinkFile")
        'lblSubAccNo.SetResourceReference(ContentProperty, "Sub AccNo")
        'lblBank.SetResourceReference(ContentProperty, "Bank") 

    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus, MainLinkFile.LostFocus
        If MainLinkFile.SelectedIndex = 0 OrElse Val(BankId.Text.Trim) = 0 Then
            BankId.Clear()
            BankName.Clear()
            Return
        End If

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile.SelectedValue)
        bm.LostFocus(BankId, BankName, "select Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ") where Id=" & BankId.Text.Trim())
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile.SelectedValue)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ")") Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub CostCenterId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CostCenterId.LostFocus
        bm.CostCenterIdLostFocus(CostCenterId, CostCenterName, )
    End Sub

    Private Sub CostCenterId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CostCenterId.KeyUp
        bm.CostCenterIdShowHelp(CostCenterId, CostCenterName, e, )
    End Sub

End Class