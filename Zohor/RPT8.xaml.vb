Imports System.Data

Public Class RPT8
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer

        rpt.paraname = New String() {"@CarNo", "@TrillaNo", "@CustomerId", "@DriverId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {CarNo.Text.Trim, TrillaNo.Text.Trim,
Val(CustomerId.Text), Val(DriverId.Text), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "CustomerInvoice3.rpt"
            Case 2
                rpt.Rpt = "InstallmentCustomerIsDelayed.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.Addcontrol_MouseDoubleClick({CustomerId, DriverId})
        LoadResource()
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1,1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

        If Flag = 2 Then
            lblDriver.Visibility = Visibility.Hidden
            lblSheekNo.Visibility = Visibility.Hidden
            lblSheekNo_Copy.Visibility = Visibility.Hidden
            lblFromDate.Visibility = Visibility.Hidden
            lblToDate.Visibility = Visibility.Hidden
            DriverId.Visibility = Visibility.Hidden
            DriverName.Visibility = Visibility.Hidden
            CarNo.Visibility = Visibility.Hidden
            TrillaNo.Visibility = Visibility.Hidden
            FromDate.Visibility = Visibility.Hidden
            ToDate.Visibility = Visibility.Hidden
        End If
    End Sub




    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")
        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CustomerId.KeyDown, DriverId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub CustomerId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CustomerId.LostFocus
        bm.LostFocus(CustomerId, CustomerName, "select Name from Customers where Id=" & CustomerId.Text.Trim())
        Dim s As String = ""
        Dim dt As DataTable = bm.ExecuteAdapter("GetCustomerData", New String() {"Id"}, New String() {Val(CustomerId.Text)})
        CustomerId.ToolTip = ""
        CustomerName.ToolTip = ""
        If dt.Rows.Count = 0 Then Return
        For i As Integer = 0 To dt.Columns.Count - 2
            s &= dt.Rows(0)(i).ToString & IIf(i = dt.Columns.Count - 1, "", vbCrLf)
        Next
        CustomerId.ToolTip = s
        CustomerName.ToolTip = s
    End Sub

    Private Sub CustomerId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CustomerId.KeyUp
        'bm.ShowHelp("Customers", CustomerId, CustomerName, e, "select cast(Id as varchar(100)) Id,Name from Customers")
        bm.ShowHelpCustomers(CustomerId, CustomerName, e)
    End Sub

    Private Sub DriverId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DriverId.LostFocus
        If Val(DriverId.Text.Trim) = 0 Then
            DriverId.Clear()
            DriverName.Clear()
            Return
        End If

        bm.LostFocus(DriverId, DriverName, "select Name from Drivers where Id=" & DriverId.Text.Trim())
    End Sub

    Private Sub DriverId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DriverId.KeyUp
        If bm.ShowHelp("Drivers", DriverId, DriverName, e, "select cast(Id as varchar(100)) Id,Name from Drivers", "Drivers") Then
            DriverId_LostFocus(Nothing, Nothing)
        End If
    End Sub


End Class