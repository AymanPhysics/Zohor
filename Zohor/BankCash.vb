Imports System.Data

Public Class BankCash
    Public TableName As String = "BankCash"
    Public MainId As String = "BankId"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    Public MyLinkFile As Integer = 0
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        If Not Md.ShowCurrency Then CurrencyId.Visibility = Visibility.Hidden
        If Not Md.ShowCostCenter Then
            lblCostCenterId.Visibility = Visibility.Hidden
            CostCenterId.Visibility = Visibility.Hidden
            CostCenterName.Visibility = Visibility.Hidden
        End If
        LoadResource()
        bm.Fields = New String() {MainId, SubId, SubId2, "DayDate", "Value", "MainAccNo", "SubAccNo", "Notes", "Canceled", "Cash", "SheekNo", "SheekPerson", "SheekDate", "CostCenterId", "CurrencyId"}
        bm.control = New Control() {BankId, txtFlag, txtID, DayDate, Value, MainAccNo, SubAccNo, Notes, Canceled, Cash, SheekNo, SheekPerson, SheekDate, CostCenterId, CurrencyId}
        bm.KeyFields = New String() {MainId, SubId, SubId2}
        bm.Table_Name = TableName
        RdoCash_Checked(Nothing, Nothing)
        btnNew_Click(sender, e)
        BankId.Focus()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        'BankId_LostFocus(Nothing, Nothing)
        MainAccNo_LostFocus(Nothing, Nothing)
        SubAccNo_LostFocus(Nothing, Nothing)
        CostCenterId_LostFocus(Nothing, Nothing)
        DayDate.Focus()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Val(BankId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblBank.Content)
            BankId.Focus()
            Return
        End If
        If Val(MainAccNo.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الحساب العام")
            MainAccNo.Focus()
            Return
        End If
        If Val(SubAccNo.Text) = 0 And SubAccNo.IsEnabled Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo.Focus()
            Return
        End If
        Value.Text = Val(Value.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text.Trim, txtID.Text}) Then Return
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Dim lop As Boolean = False
    Sub ClearControls()
        If lop OrElse lv Then Return
        lop = True

        bm.ClearControls()
        BankId_LostFocus(Nothing, Nothing)

        'BankName.Clear()
        MainAccName.Clear()
        SubAccName.Clear()
        CostCenterName.Clear()
        RdoCash.IsChecked = True
        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtFlag.Text = Flag
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & MainId & "=" & BankId.Text & " and " & SubId & "=" & txtFlag.Text)
        If txtID.Text = "" Then txtID.Text = "1"
        'DayDate.Focus()
        txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & MainId & "=" & BankId.Text & " and " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text, txtID.Text}, "Back", dt)
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
        bm.RetrieveAll(New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text.Trim, txtID.Text}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, MainAccNo.KeyDown, SubAccNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus

        If Val(BankId.Text.Trim) = 0 Then
            BankId.Clear()
            BankName.Clear()
            Return
        End If

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
        bm.LostFocus(BankId, BankName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim())
        CurrencyId.SelectedValue = bm.ExecuteScalar("select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim())
        If lop Then Return
        btnNew_Click(Nothing, Nothing)
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName")) Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SubAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubAccNo.LostFocus
        If Val(MainAccNo.Text) = 0 Or Not SubAccNo.IsEnabled Then
            SubAccNo.Clear()
            SubAccName.Clear()
            Return
        End If

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')")
        bm.LostFocus(SubAccNo, SubAccName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo.Text.Trim() & " and AccNo='" & MainAccNo.Text & "'")
    End Sub

    Private Sub SubAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SubAccNo.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')")
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & MainAccNo.Text & "'") Then
            SubAccNo_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub MainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo.LostFocus
        bm.AccNoLostFocus(MainAccNo, MainAccName, , , )

        SubAccNo.IsEnabled = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')").Rows.Count > 0
        SubAccNo_LostFocus(Nothing, Nothing)
        If SubAccNo.IsEnabled Then
            SubAccNo.Focus()
        Else
            If CostCenterId.Visibility = Visibility.Visible Then
                CostCenterId.Focus()
            Else
                RdoCash.Focus()
            End If
        End If
    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles MainAccNo.KeyUp
        If bm.AccNoShowHelp(MainAccNo, MainAccName, e, , , ) Then
            MainAccNo_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub RdoCash_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoCash.Checked, RdoCheck.Checked
        Try
            Cash.Text = 0
            If RdoCash.IsChecked Then
                Cash.Text = 1
            ElseIf RdoCheck.IsChecked Then
                Cash.Text = 2
            End If
        Catch ex As Exception
        End Try

        Try
            If RdoCheck.IsChecked Then
                SheekNo.Visibility = Visibility.Visible
                lblSheekNo.Visibility = Visibility.Visible

                SheekPerson.Visibility = Visibility.Visible
                lblSheekPerson.Visibility = Visibility.Visible

                SheekDate.Visibility = Visibility.Visible
                lblSheekDate.Visibility = Visibility.Visible
            Else
                SheekNo.Visibility = Visibility.Hidden
                lblSheekNo.Visibility = Visibility.Hidden

                SheekPerson.Visibility = Visibility.Hidden
                lblSheekPerson.Visibility = Visibility.Hidden

                SheekDate.Visibility = Visibility.Hidden
                lblSheekDate.Visibility = Visibility.Hidden
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub Cash_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Cash.TextChanged
        Try
            If Cash.Text = 1 Then
                RdoCash.IsChecked = True
            ElseIf Cash.Text = 2 Then
                RdoCheck.IsChecked = True
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Canceled_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Checked
        Value.Text = ""
        Value.IsEnabled = False
    End Sub

    Private Sub Canceled_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Unchecked
        Value.IsEnabled = True
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblID.SetResourceReference(ContentProperty, "Id")

        lblBank.SetResourceReference(ContentProperty, "Bank")
        If MyLinkFile = 5 Then lblBank.SetResourceReference(ContentProperty, "Safe")

        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblValue.SetResourceReference(ContentProperty, "Value")
        lblNotes.SetResourceReference(ContentProperty, "Notes")
        lblMainAccNo.SetResourceReference(ContentProperty, "MainAccNo")
        lblSubAccNo.SetResourceReference(ContentProperty, "SubAccNo")
        lblSheekDate.SetResourceReference(ContentProperty, "SheekDate")
        lblSheekNo.SetResourceReference(ContentProperty, "SheekNo")
        lblSheekPerson.SetResourceReference(ContentProperty, "SheekPerson")
    End Sub

    Private Sub CostCenterId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CostCenterId.LostFocus
        bm.CostCenterIdLostFocus(CostCenterId, CostCenterName, )
    End Sub

    Private Sub CostCenterId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CostCenterId.KeyUp
        bm.CostCenterIdShowHelp(CostCenterId, CostCenterName, e, )
    End Sub


End Class
