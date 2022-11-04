Imports System.Data

Public Class RPT36
    Dim bm As New BasicMethods
    Public Flag As Integer = 1
    Dim dt As New DataTable

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@EntryTypeId", "@StoreId", "@FromDate", "@ToDate", "@FromInvoiceNo", "@ToInvoiceNo", "Header", "@UnDeliveredOnly"}
        rpt.paravalue = New String() {Val(EntryTypeId.SelectedValue), Val(StoreId.Text), FromDate.SelectedDate, ToDate.SelectedDate, Val(FromInvoice.Text), Val(ToInvoice.Text), CType(Parent, Page).Title, IIf(UnDeliveredOnly.IsChecked, 1, 0)}

        Select Case Flag
            Case 1
                rpt.Rpt = "EntryMainAll.rpt"
                If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                    rpt.Rpt = "EntryAll.rpt"
                End If
            Case 2
                rpt.Rpt = "DeliveryCustomers3.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        bm.Addcontrol_MouseDoubleClick({StoreId})

        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

        If Flag = 1 Then
            lblStoreId.Visibility = Visibility.Hidden
            StoreId.Visibility = Visibility.Hidden
            StoreName.Visibility = Visibility.Hidden

            UnDeliveredOnly.Visibility = Visibility.Hidden
        Else
            lblMain.Visibility = Visibility.Hidden
            EntryTypeId.Visibility = Visibility.Hidden
        End If

        bm.FillCombo("GetEmpEntryTypes @Flag=" & 4 & ",@EmpId=" & Md.UserName & "", EntryTypeId)



        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate

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

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub


End Class