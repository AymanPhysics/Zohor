Imports System.Data

Public Class RPT3
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        If EmpId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        If CsId.SelectedValue Is Nothing Then CsId.SelectedIndex = 0
        If SerialType.SelectedValue Is Nothing Then SerialType.SelectedIndex = 0
        If ServiceGroupId.SelectedValue Is Nothing Then ServiceGroupId.SelectedIndex = 0
        If ServiceTypeId.SelectedValue Is Nothing Then ServiceTypeId.SelectedIndex = 0
        If UserId.SelectedValue Is Nothing Then UserId.SelectedIndex = 0

        rpt.paraname = New String() {"@EmpId", "@CaseId", "@FromDate", "@ToDate", "@FromInvoice", "@ToInvoice", "@SerialType", "@ServiceGroupId", "@ServiceTypeId", "@CsId", "@FromSerialId", "@ToSerialId", "@Canceled", "@UserId", "Header", "@Returned"}
        rpt.paravalue = New String() {Val(EmpId.SelectedValue), Val(CaseId.Text), FromDate.SelectedDate, ToDate.SelectedDate, Val(FromInvoice.Text), Val(ToInvoice.Text), SerialType.SelectedValue.ToString, ServiceGroupId.SelectedValue.ToString(), ServiceTypeId.SelectedValue.ToString(), CsId.SelectedValue.ToString(), Val(FromSerial.Text), Val(ToSerial.Text), Canceled.SelectedIndex.ToString(), (UserId.SelectedValue), CType(Parent, Page).Title, Returned.SelectedIndex.ToString}
        Select Case Flag
            Case 1
                rpt.Rpt = "Services.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({CaseId})
        Canceled.SelectedIndex = 2
        Returned.SelectedIndex = 2
        bm.FillCombo("SerialTypes", SerialType, "")

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where SystemUser='1' and Stopped='0' union select 0 Id,'-' Name order by Name", UserId)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Doctor='1' and Stopped='0' union select 0 Id,'-' Name order by Name", EmpId)
        
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Nurse='1' and Stopped='0' union select 0 Id,'-' Name order by Name", CsId)

        EmpId.SelectedValue = Md.UserName
        If EmpId.SelectedIndex < 0 Then EmpId.SelectedIndex = 0
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

        If Flag = 1 Then
            Canceled.Visibility = Visibility.Hidden
            lblCanceled.Visibility = Visibility.Hidden
            Returned.Visibility = Visibility.Hidden
            lblReturned.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp
        If bm.ShowHelpCases(CaseId, CaseName, e) Then
            CaseID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select EnName Name from Cases where Id=" & CaseId.Text.Trim())
    End Sub

    Private Sub SerialType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles SerialType.SelectionChanged
        Try
            bm.FillCombo("ServiceGroups", ServiceGroupId, "where (SerialId=" & SerialType.SelectedValue.ToString & " or " & SerialType.SelectedValue.ToString & "=0)")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ServiceGroupId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ServiceGroupId.SelectionChanged
        Try
            bm.FillCombo("ServiceTypes", ServiceTypeId, " Where ServiceGroupId=" & Val(ServiceGroupId.SelectedValue.ToString))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")
        lblServiceGroup.SetResourceReference(ContentProperty, "ServiceGroupId")
        lblServiceType.SetResourceReference(ContentProperty, "ServiceTypeId")
        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")
        lblDoctor.SetResourceReference(ContentProperty, "Doctor")
        lblCS.SetResourceReference(ContentProperty, "C. S.")
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
