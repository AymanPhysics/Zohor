Imports System.Data

Public Class Services
    Public TableName As String = "Services"
    Public SubId As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    Dim LabServiceGroupId As Integer = 0
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSaveWithoutPrint}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnSave})
        LoadResource()

        LabServiceGroupId = bm.ExecuteScalar("select top 1 LabServiceGroupId from Statics")

        bm.FillCombo("ServiceGroups", ServiceGroupId, "")
        LoadServiceGroups()

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Nurse='1' and Stopped='0' union select 0 Id,'-' Name order by Name", CsId)

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReservation)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdRemaining)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdCanceled)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReturned)

        bm.FillCombo("Companies", CompanyId, "")

        bm.Fields = New String() {SubId, "DayDate", "Canceled", "ServiceGroupId", "ServiceTypeId", "CaseId", "CompanyId", "DrId", "CsId", "DrValue", "CsValue", "CoValue", "Value", "Notes", "SerialId", "SerialId2", "Payed", "Done", "Remaining", "RemainingDate", "EmpIdReservation", "EmpIdRemaining", "EmpIdCanceled", "CanceledDate", "Returned", "ReturnedDate", "EmpIdReturned", "CaseInvoiceNo"}
        bm.control = New Control() {txtID, DayDate, Canceled, ServiceGroupId, ServiceTypeId, CaseId, CompanyId, DrId, CsId, DrValue, CsValue, CoValue, Value, Notes, SerialId, SerialId2, Payed, Done, Remaining, RemainingDate, EmpIdReservation, EmpIdRemaining, EmpIdCanceled, CanceledDate, Returned, ReturnedDate, EmpIdReturned, CaseInvoiceNo}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)
        Done_Unchecked(Nothing, Nothing)
        Canceled_Unchecked(Nothing, Nothing)
        Returned_Unchecked(Nothing, Nothing)
    End Sub

    Sub LoadServiceGroups()
        Try
            WR1.Children.Clear()
            For i As Integer = 0 To ServiceGroupId.Items.Count - 1
                If ServiceGroupId.Items(i)("Id").ToString = 0 Then Continue For
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & ServiceGroupId.Items(i)("Id").ToString
                x.Tag = ServiceGroupId.Items(i)("Id").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 110
                x.Height = 30
                x.Margin = New Thickness(5, 5, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = ServiceGroupId.Items(i)("Name").ToString.Replace(vbCrLf, " ")
                x.ToolTip = x.Content
                x.Background = btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR1.Children.Add(x)
                AddHandler x.Click, AddressOf btnServiceGroupClick
            Next
        Catch ex As Exception
        End Try
    End Sub


    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim lop As Boolean = False
    Sub FillControls()
        bm.FillControls(Me)
        ServiceGroupId_SelectionChanged(Nothing, Nothing)
        lop = True
        bm.FillControls(Me)
        ServiceTypeId_SelectionChanged(Nothing, Nothing)
        CaseID_LostFocus(Nothing, Nothing)

        If Canceled.IsChecked Then
            Canceled_Checked(Nothing, Nothing)
        Else
            Canceled_Unchecked(Nothing, Nothing)
        End If

        If Returned.IsChecked Then
            Returned_Checked(Nothing, Nothing)
        Else
            Returned_Unchecked(Nothing, Nothing)
        End If

        'If DayDate.SelectedDate = bm.MyGetDate() Then
        '    Returned.IsEnabled = False
        'Else
        '    Canceled.IsEnabled = False
        'End If

        lop = False
        UndoNewId()
        DayDate.Focus()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim lop2 As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveWithoutPrint.Click
        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If
        If ServiceGroupId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد مجموعة الخدمة")
            ServiceGroupId.Focus()
            Return
        End If
        If Val(DrValue.Text) <> 0 AndAlso DrId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد الطبيب")
            DrId.Focus()
            Return
        End If
        If Val(CsValue.Text) <> 0 AndAlso CsId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد الممرض")
            CsId.Focus()
            Return
        End If

        lop2 = True
        DrValue.Text = Val(DrValue.Text)
        CsValue.Text = Val(CsValue.Text)
        CoValue.Text = Val(CoValue.Text)
        Value.Text = Val(Value.Text)
        Payed.Text = Val(Payed.Text)
        Remaining.Text = Val(Remaining.Text)
        lop2 = False

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If txtID.Text.Trim = "" Then
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If txtID.Text = "" Then txtID.Text = "1"
            LastEntry.Text = txtID.Text
            State = BasicMethods.SaveState.Insert
        End If


        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                txtID.Text = ""
                LastEntry.Text = ""
                SerialId.Text = ""
                LastSerialId.Text = ""
            End If
            Return
        End If

        If Val(SerialId.Text) = 0 AndAlso Val(Payed.Text) > 0 Then
            If Md.MyProjectType = ProjectType.Zohor Then
                SerialId.Text = bm.ExecuteScalar("updateServicesSerialIdCo", {"ServiceGroupId", "Flag", "InvoiceNo"}, {ServiceGroupId.SelectedValue.ToString, 1, Val(txtID.Text)})
            Else
                SerialId.Text = bm.ExecuteScalar("updateServicesSerialId", {"ServiceGroupId", "Flag", "InvoiceNo"}, {ServiceGroupId.SelectedValue.ToString, 1, Val(txtID.Text)})
            End If
            LastSerialId.Text = SerialId.Text
        End If

        If SerialId2.IsVisible AndAlso Val(SerialId2.Text) = 0 AndAlso Val(Remaining.Text) > 0 Then
            If Md.MyProjectType = ProjectType.Zohor Then
                SerialId2.Text = bm.ExecuteScalar("updateServicesSerialId2", {"ServiceGroupId", "Flag", "InvoiceNo"}, {ServiceGroupId.SelectedValue.ToString, 1, Val(txtID.Text)})
            Else
                SerialId2.Text = bm.ExecuteScalar("updateServicesSerialId2", {"ServiceGroupId", "Flag", "InvoiceNo"}, {ServiceGroupId.SelectedValue.ToString, 1, Val(txtID.Text)})
            End If
            LastSerialId.Text = SerialId2.Text
        End If

        If Not Canceled.IsChecked AndAlso Not Returned.IsChecked AndAlso sender Is btnSave Then
            ShowRPT()
            If Val(ServiceGroupId.SelectedValue) = LabServiceGroupId Then ShowRPT(True)
        End If
        btnNew_Click(sender, e)
        
    End Sub

    Private Sub ShowRPT(Optional IsLabServiceGroupId As Boolean = False)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Flag", "@Id", "Header"}
        rpt.paravalue = New String() {1, txtID.Text, CType(Parent, Page).Title}
        rpt.Rpt = "ServicesONE.rpt"
        If IsLabServiceGroupId Then rpt.Rpt = "ServicesONELab.rpt"
        rpt.Print()
    End Sub


    Sub NewId()
        txtID.Clear()
        'txtID.IsEnabled = False

        DayDate.IsEnabled = Md.Manager
        WR1.IsEnabled = True
        WR2.IsEnabled = True

        DrId.IsEnabled = True
        CaseId.IsEnabled = True
        CsId.IsEnabled = True

        ServiceGroupId.IsEnabled = True
        ServiceTypeId.IsEnabled = True

        DrValue.IsEnabled = True
        CsValue.IsEnabled = True
        CoValue.IsEnabled = True
        Payed.IsEnabled = True

    End Sub

    Sub UndoNewId()
        txtID.IsEnabled = True

        DayDate.IsEnabled = Md.Manager
        WR1.IsEnabled = Md.Manager
        WR2.IsEnabled = Md.Manager

        CaseId.IsEnabled = Md.Manager
        DrId.IsEnabled = Md.Manager
        CsId.IsEnabled = Md.Manager

        ServiceGroupId.IsEnabled = Md.Manager
        ServiceTypeId.IsEnabled = Md.Manager

        DrValue.IsEnabled = Md.Manager
        CsValue.IsEnabled = Md.Manager
        CoValue.IsEnabled = Md.Manager
        Payed.IsEnabled = Md.Manager

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls(False)
        Canceled.IsEnabled = True
        DayDate.SelectedDate = bm.MyGetDate()
        ServiceGroupId_SelectionChanged(Nothing, Nothing)
        ServiceTypeId_SelectionChanged(Nothing, Nothing)
        CaseID_LostFocus(Nothing, Nothing)

        'txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        'If txtID.Text = "" Then txtID.Text = "1"
        EmpIdReservation.SelectedValue = Md.UserName
        NewId()
        DayDate.Focus()
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

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub


    
    'Private Sub MyBase_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    '    If Not btnSave.Enabled Then Exit Sub
    '    Select Case bm.RequestDelete
    '        Case BasicMethods.CloseState.Yes
    '            
    '            btnSave_Click(Nothing, Nothing)
    '            If Not AllowClose Then e.Cancel = True
    '        Case BasicMethods.CloseState.No

    '        Case BasicMethods.CloseState.Cancel
    '            e.Cancel = True
    '    End Select
    'End Sub


    Private Sub Canceled_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Checked
        Canceled.IsEnabled = Md.Manager
        Returned.IsEnabled = Md.Manager
        Returned.IsChecked = False

        'DrValue.Text = ""
        'CsValue.Text = ""
        'CoValue.Text = ""
        'Value.Text = ""
        'Payed.Text = ""
        'Remaining.Text = ""

        'DrId.SelectedIndex = 0
        'CsId.SelectedIndex = 0
        'Done.IsChecked = False

        DrId.IsEnabled = False
        CsId.IsEnabled = False

        DrValue.IsEnabled = False
        CsValue.IsEnabled = False
        CoValue.IsEnabled = False

        Payed.IsEnabled = False
        Done.IsEnabled = False

        CanceledDate.Visibility = Visibility.Visible
        CanceledDate.IsEnabled = Md.Manager
        EmpIdCanceled.Visibility = Visibility.Visible
        If EmpIdCanceled.SelectedIndex <= 0 Then
            EmpIdCanceled.SelectedValue = Md.UserName
            CanceledDate.SelectedDate = bm.MyGetDate()
        End If
    End Sub


    Private Sub Returned_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Returned.Checked
        Canceled.IsEnabled = Md.Manager
        Returned.IsEnabled = Md.Manager
        Canceled.IsChecked = False

        'DrValue.Text = ""
        'CsValue.Text = ""
        'CoValue.Text = ""
        'Value.Text = ""
        'Payed.Text = ""
        'Remaining.Text = ""

        'DrId.SelectedIndex = 0
        'CsId.SelectedIndex = 0
        'Done.IsChecked = False

        DrId.IsEnabled = False
        CsId.IsEnabled = False

        DrValue.IsEnabled = False
        CsValue.IsEnabled = False
        CoValue.IsEnabled = False

        Payed.IsEnabled = False
        Done.IsEnabled = False

        ReturnedDate.Visibility = Visibility.Visible
        ReturnedDate.IsEnabled = Md.Manager
        EmpIdReturned.Visibility = Visibility.Visible
        If EmpIdReturned.SelectedIndex <= 0 Then
            EmpIdReturned.SelectedValue = Md.UserName
            ReturnedDate.SelectedDate = bm.MyGetDate()
        End If
    End Sub

    Private Sub Canceled_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Unchecked

        If Not Returned.IsChecked Then
            Canceled.IsEnabled = True
            Returned.IsEnabled = True

            DrValue.IsEnabled = True
            CsValue.IsEnabled = True
            CoValue.IsEnabled = True

            DrId.IsEnabled = True
            CsId.IsEnabled = True

            Payed.IsEnabled = True
            Done.IsEnabled = True
        End If

        CanceledDate.Visibility = Visibility.Hidden
        CanceledDate.SelectedDate = Nothing
        EmpIdCanceled.Visibility = Visibility.Hidden
        EmpIdCanceled.SelectedIndex = 0
    End Sub


    Private Sub Returned_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Returned.Unchecked

        If Not Canceled.IsChecked Then
            Returned.IsEnabled = True

            DrValue.IsEnabled = True
            CsValue.IsEnabled = True
            CoValue.IsEnabled = True

            DrId.IsEnabled = True
            CsId.IsEnabled = True

            Payed.IsEnabled = True
            Done.IsEnabled = True
        End If

        ReturnedDate.Visibility = Visibility.Hidden
        ReturnedDate.SelectedDate = Nothing
        EmpIdReturned.Visibility = Visibility.Hidden
        EmpIdReturned.SelectedIndex = 0
    End Sub

    Private Sub DrValue_TextChanged(sender As Object, e As TextChangedEventArgs) Handles DrValue.TextChanged, CsValue.TextChanged, CoValue.TextChanged
        If lop2 Then Return
        Value.Text = Val(DrValue.Text) + Val(CsValue.Text) + Val(CoValue.Text)
        Payed.Text = Value.Text
    End Sub

    Private Sub ServiceGroupId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ServiceGroupId.SelectionChanged
        Dim s As String = ServiceTypeId.SelectedValue
        Try
            If Not lop Then
                bm.FillCombo("ServiceTypes", ServiceTypeId, " Where IsStopped=0 and ServiceGroupId = " & Val(ServiceGroupId.SelectedValue.ToString))
                ServiceTypeId.SelectedValue = s
            End If
        Catch ex As Exception
        End Try
        TabItem1.Header = ""
        If ServiceGroupId.SelectedIndex > 0 Then TabItem1.Header = ServiceGroupId.Items(ServiceGroupId.SelectedIndex)("Name")
        LoadServiceTypes()

    End Sub

    Sub LoadServiceTypes()
        Try
            WR2.Children.Clear()
            For i As Integer = 0 To ServiceTypeId.Items.Count - 1
                If ServiceTypeId.Items(i)("Id").ToString = 0 Then Continue For
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & ServiceTypeId.Items(i)("Id").ToString
                x.Tag = ServiceTypeId.Items(i)("Id").ToString
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 110
                x.Height = 30
                x.Margin = New Thickness(5, 5, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = ServiceTypeId.Items(i)("Name").ToString.Replace(vbCrLf, " ")
                x.ToolTip = x.Content
                x.Background = btnDelete.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR2.Children.Add(x)
                AddHandler x.Click, AddressOf btnServiceTypeClick
            Next
        Catch ex As Exception
        End Try
    End Sub


    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp
        If bm.ShowHelpCases(CaseId, CaseName, e) Then
            CaseID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select " & Resources.Item("CboName") & " Name from Cases where Id=" & CaseId.Text.Trim())
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        CompanyId.SelectedIndex = 0
        Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile,CompanyId from Cases where Id=" & CaseId.Text.Trim())
        If dt.Rows.Count > 0 Then
            CompanyId.SelectedValue = dt.Rows(0)("CompanyId")
            CaseId.ToolTip = Resources.Item("Id") & ": " & CaseId.Text & vbCrLf & Resources.Item("Name") & ": " & CaseName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
            CaseName.ToolTip = CaseId.ToolTip
        End If
        ServiceTypeId_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub ServiceTypeId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ServiceTypeId.SelectionChanged
        FillDoctors()
        If lop Then Return
        DrValue.Clear()
        CsValue.Clear()
        CoValue.Clear()
        Try
            Dim dt As DataTable = bm.ExecuteAdapter("select * from ServiceTypes where IsStopped=0 and ServiceGroupId=" & ServiceGroupId.SelectedValue.ToString & " and Id=" & ServiceTypeId.SelectedValue.ToString)
            If dt.Rows.Count > 0 Then
                'Dim Perc As Decimal = Val(bm.ExecuteScalar("select (100-isnull((select Perc from Companies where Id=(select T.CompanyId from Cases T where T.Id=" & Val(CaseId.Text) & ")),0))/100."))

                'Dim MyVal As Decimal = Val(bm.ExecuteScalar("select Price from ServiceCompanies where ServiceGroupId=" & ServiceGroupId.SelectedValue.ToString & " and Id=" & ServiceTypeId.SelectedValue.ToString & " and CompanyId=(select T.CompanyId from Cases T where T.Id=" & Val(CaseId.Text) & ")"))
                'Dim MyRemaining As Decimal = Val(bm.ExecuteScalar("select Remaining from ServiceCompanies where ServiceGroupId=" & ServiceGroupId.SelectedValue.ToString & " and Id=" & ServiceTypeId.SelectedValue.ToString & " and CompanyId=(select T.CompanyId from Cases T where T.Id=" & Val(CaseId.Text) & ")"))
                
                Dim Total As Decimal = Val(dt.Rows(0)("DrValue").ToString) + Val(dt.Rows(0)("CsValue").ToString) + Val(dt.Rows(0)("CoValue").ToString)

                DrValue.Text = Val(dt.Rows(0)("DrValue").ToString)
                CsValue.Text = Val(dt.Rows(0)("CsValue").ToString)
                CoValue.Text = Val(dt.Rows(0)("CoValue").ToString)

                CompanyId.ToolTip = Nothing
                If CompanyId.SelectedIndex > 0 Then
                    Dim dt2 As DataTable = bm.ExecuteAdapter("GetserviceCompaniesData", {"CompanyId", "ServiceGroupId", "Id"}, {CompanyId.SelectedValue.ToString, ServiceGroupId.SelectedValue, ServiceTypeId.SelectedValue})
                    DrValue.Text = 0
                    CsValue.Text = 0
                    CoValue.Text = dt2.Rows(0)("Price").ToString

                    Payed.Text = dt2.Rows(0)("Payed").ToString
                    CompanyId.ToolTip = dt2.Rows(0)("Notes").ToString
                    bm.ShowMsgOnScreen(dt2.Rows(0)("Notes").ToString)
                End If

                'If MyVal > 0 And Total > 0 Then
                '    DrValue.Text = Math.Round(Val(dt.Rows(0)("DrValue").ToString) * MyVal / Total, 2, MidpointRounding.AwayFromZero)
                '    CsValue.Text = Math.Round(Val(dt.Rows(0)("CsValue").ToString) * MyVal / Total, 2, MidpointRounding.AwayFromZero)
                '    CoValue.Text = MyVal - Val(DrValue.Text) - Val(CsValue.Text)
                'Else
                '    DrValue.Text = Math.Round(Val(dt.Rows(0)("DrValue").ToString) * Perc, 2, MidpointRounding.AwayFromZero)
                '    CsValue.Text = Math.Round(Val(dt.Rows(0)("CsValue").ToString) * Perc, 2, MidpointRounding.AwayFromZero)
                '    CoValue.Text = Math.Round(Val(dt.Rows(0)("CoValue").ToString) * Perc, 2, MidpointRounding.AwayFromZero)
                'End If

                'Payed.Text = Val(Value.Text) - MyRemaining

            End If
        Catch ex As Exception
        End Try

    End Sub

    Sub FillDoctors()
        Try
            Dim sd As String = DrId.SelectedValue
            Dim dt As DataTable = bm.ExecuteAdapter("select DepartmentId from ServiceTypes where ServiceGroupId=" & ServiceGroupId.SelectedValue.ToString & " and Id=" & ServiceTypeId.SelectedValue.ToString)
            Dim s As String = 0
            If dt.Rows.Count > 0 Then
                s = Val(dt.Rows(0)("DepartmentId").ToString)
            End If
            bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Doctor='1' and (DepartmentId=" & s & " or " & s & "=0) and Stopped='0' union select 0 Id,'-' Name order by Name", DrId)
            If lop Then DrId.SelectedValue = sd
        Catch ex As Exception
        End Try

    End Sub

    Private Sub btnServiceGroupClick(sender As Object, e As RoutedEventArgs)
        ServiceGroupId.SelectedValue = sender.Tag
        ServiceGroupId_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub btnServiceTypeClick(sender As Object, e As RoutedEventArgs)
        ServiceTypeId.SelectedValue = sender.Tag
        ServiceTypeId_SelectionChanged(Nothing, Nothing)
        CaseId.Focus()
    End Sub

    Private Sub LoadResource()
        btnSaveWithoutPrint.SetResourceReference(ContentProperty, "Save")
        btnSave.SetResourceReference(ContentProperty, "Print")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")


        Canceled.SetResourceReference(CheckBox.ContentProperty, "Cancel")
        Returned.SetResourceReference(CheckBox.ContentProperty, "Returned")
        Done.SetResourceReference(CheckBox.ContentProperty, "Done Remaining")

        lblId.SetResourceReference(ContentProperty, "Id")
        lblCaseId.SetResourceReference(ContentProperty, "CaseId")
        lblCoValue.SetResourceReference(ContentProperty, "CoValue")
        lblCsValue.SetResourceReference(ContentProperty, "CsValue")
        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblDrValue.SetResourceReference(ContentProperty, "DrValue")
        lblLastEntry.SetResourceReference(ContentProperty, "LastEntry")
        lblNotes.SetResourceReference(ContentProperty, "Notes")
        lblServiceGroupId.SetResourceReference(ContentProperty, "ServiceGroupId")
        lblServiceTypeId.SetResourceReference(ContentProperty, "ServiceTypeId")
        
        lblValue.SetResourceReference(ContentProperty, "Value")
        lblPayed.SetResourceReference(ContentProperty, "Payed")
        lblRemaining.SetResourceReference(ContentProperty, "Remaining")
        lblSerialId.SetResourceReference(ContentProperty, "Serial")
        lblSerialId2.SetResourceReference(ContentProperty, "Serial")
        lblLastSerialId.SetResourceReference(ContentProperty, "LastSerial")

    End Sub

    Private Sub Value_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Value.TextChanged, Payed.TextChanged
        If lop2 Then Return
        Remaining.Text = Val(Value.Text) - Val(Payed.Text)
    End Sub



    Private Sub Done_Checked(sender As Object, e As RoutedEventArgs) Handles Done.Checked
        Done.IsEnabled = Md.Manager

        lblSerialId2.Visibility = Visibility.Visible
        SerialId2.Visibility = Visibility.Visible

        RemainingDate.Visibility = Visibility.Visible
        RemainingDate.SelectedDate = bm.MyGetDate()
        RemainingDate.IsEnabled = Md.Manager
        EmpIdRemaining.Visibility = Visibility.Visible
        EmpIdRemaining.SelectedValue = Md.UserName
    End Sub
    Private Sub Done_Unchecked(sender As Object, e As RoutedEventArgs) Handles Done.Unchecked
        Done.IsEnabled = True

        lblSerialId2.Visibility = Visibility.Hidden
        SerialId2.Visibility = Visibility.Hidden

        RemainingDate.Visibility = Visibility.Hidden
        RemainingDate.SelectedDate = Nothing
        EmpIdRemaining.Visibility = Visibility.Hidden
        EmpIdRemaining.SelectedIndex = 0
    End Sub


    Private Sub btnAddCase_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCase.Click
        Dim frm As New MyWindow With {.Title = "Patients", .WindowState = WindowState.Maximized}
        frm.Content = New Cases4 With {.MyId = Val(CaseId.Text)}
        frm.Show()
    End Sub

End Class
