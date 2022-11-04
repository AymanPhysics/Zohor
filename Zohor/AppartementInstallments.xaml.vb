Imports System.Data

Public Class AppartementInstallments

    Public TableName As String = "AppartementsSales"
    Public TableNameDt As String = "AppartementInstallmentsDt"
    Public SubId As String = "Id"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Dim LinkFile As Integer = 5
    Public WithImage As Boolean = False
    Public ReLoadMenue As Boolean = False

    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {}, {})
        LoadResource()
        Rdo_Checked(Nothing, Nothing)
        bm.Fields = {SubId, "DayDate", "CustName", "Floor", "Area", "BuildingName", "Sample", "UnitNo", "PriceBefore"}
        bm.control = {txtID, DayDate, CustName, Floor, Area, BuildingName, Sample, UnitNo, PriceBefore}
        bm.KeyFields = {SubId}

        bm.Table_Name = TableName
        txtID_Leave(Nothing, Nothing)
    End Sub




    Sub FillControls()
        bm.FillControls(Me)
        bm.FillCombo("select Id,'[ '+cast(Id as varchar(10))+' ]       '+dbo.ToStrDate(DueDate) Name from AppartementInstallments where AppartementId='" & txtID.Text & "' union select '0','-'", InstallmentDate)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CustName.Text.Trim = "" Then
            Return
        End If
        If Val(BankId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblBank.Content)
            BankId.Focus()
            Return
        End If
        Dim str As String = "delete from " & TableNameDt & " where AppartementId='" & txtID.Text.Trim & "' and PaymentFlag='" & PaymentFlag.Text & "'"
        If Rdo3.IsChecked Then
            str &= " and Id='" & InstallmentDate.SelectedValue & "'"
        End If
        str &= " insert AppartementInstallmentsDt(AppartementId,PaymentFlag,Id,SafeId,Value,DayDate) select '" & txtID.Text.Trim & "','" & PaymentFlag.Text & "','" & InstallmentDate.SelectedValue & "','" & Val(BankId.Text) & "','" & Val(Value.Text) & "','" & bm.ToStrDate(DayDate.SelectedDate) & "'"
        bm.ExecuteNonQuery(str)

        CType(Parent, Window).Close()
    End Sub

    Sub ClearControls()
        Try
            txtID.Clear()
            bm.ClearControls()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            Dim str As String = "delete from " & TableNameDt & " where AppartementId='" & txtID.Text.Trim & "' and PaymentFlag='" & PaymentFlag.Text & "'"
            If Rdo3.IsChecked Then
                str &= " and Id='" & InstallmentDate.SelectedValue & "'"
            End If
            bm.ExecuteNonQuery(Str)
            CType(Parent, Window).Close()
        End If
    End Sub

    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True
        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, Area.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID2_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")

    End Sub

    Private Sub Rdo_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Rdo1.Checked, Rdo2.Checked, Rdo3.Checked
        Try
            If Rdo3.IsChecked Then
                InstallmentDate.Visibility = Visibility.Visible
            Else
                InstallmentDate.Visibility = Visibility.Hidden
                InstallmentDate.SelectedIndex = 0
            End If
        Catch ex As Exception
        End Try

        Dim MyDt As DataTable = bm.ExecuteAdapter("Select * From AppartementsSales where Id='" & txtID.Text & "'")
        Try
            PaymentFlag.Text = 0
            If Rdo1.IsChecked Then
                PaymentFlag.Text = 1
                Value.Text = MyDt.Rows(0)("Payed")
            ElseIf Rdo2.IsChecked Then
                PaymentFlag.Text = 2
                Value.Text = MyDt.Rows(0)("Payed2")
            ElseIf Rdo3.IsChecked Then
                PaymentFlag.Text = 3
                InstallmentDate_SelectionChanged(Nothing, Nothing)
            End If
        Catch ex As Exception
        End Try
        GetData()
    End Sub

    Private Sub PaymentFlag_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles PaymentFlag.TextChanged
        Try
            If PaymentFlag.Text = 1 Then
                Rdo1.IsChecked = True
            ElseIf PaymentFlag.Text = 2 Then
                Rdo2.IsChecked = True
            ElseIf PaymentFlag.Text = 3 Then
                Rdo3.IsChecked = True
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus
        If Val(BankId.Text.Trim) = 0 Then
            BankId.Clear()
            BankName.Clear()
            Return
        End If

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & LinkFile)
        bm.LostFocus(BankId, BankName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim())
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & LinkFile)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName")) Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub InstallmentDate_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles InstallmentDate.SelectionChanged

        Value.Text = Val(bm.ExecuteScalar("Select Value From AppartementInstallments where AppartementId='" & txtID.Text & "' and Id='" & InstallmentDate.SelectedValue.ToString & "'"))
        GetData()

    End Sub

    Private Sub GetData()

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        BankId.Clear()
        BankName.Clear()

        Dim str As String = "select * from " & TableNameDt & " where AppartementId='" & txtID.Text.Trim & "' and PaymentFlag='" & PaymentFlag.Text & "'"
        If Rdo3.IsChecked Then
            str &= " and Id='" & InstallmentDate.SelectedValue & "'"
        End If
        Dim dtdt As DataTable = bm.ExecuteAdapter(str)
        If dtdt.Rows.Count > 0 Then
            DayDate.SelectedDate = dtdt.Rows(0)("DayDate")
            BankId.Text = dtdt.Rows(0)("SafeId")
            BankId_LostFocus(Nothing, Nothing)
        End If

    End Sub

End Class
