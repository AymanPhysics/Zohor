Imports System.Data

Public Class OrderTypes
    Public TableName As String = "OrderTypes"
    Public SubId As String = "Id"
    Public SubName As String = "Name"



    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    Public MyLinkFile As Integer = 11
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        If Not Md.ShowCurrency Then CurrencyId.Visibility = Visibility.Hidden
        bm.Fields = New String() {SubId, SubName, "AccNo", "AccNo1", "AccNo2", "AccNo3", "CurrencyId", "ApplyCurrencyCycle", "Exchange", "MainBal0", "Bal0"}
        bm.control = New Control() {txtID, txtName, AccNo, AccNo1, AccNo2, AccNo3, CurrencyId, ApplyCurrencyCycle, Exchange, MainBal0, Bal0}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        
        If Not Md.ShowCurrency Then
            CurrencyId.Visibility = Visibility.Hidden
            lblExchange.Visibility = Visibility.Hidden
            Exchange.Visibility = Visibility.Hidden
            lblBal0.Visibility = Visibility.Hidden
            Bal0.Visibility = Visibility.Hidden
            ApplyCurrencyCycle.Visibility = Visibility.Hidden
        End If

        btnNew_Click(sender, e)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        AccNo_LostFocus(Nothing, Nothing)
        AccNo1_LostFocus(Nothing, Nothing)
        AccNo2_LostFocus(Nothing, Nothing)
        AccNo3_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If
        If Val(AccNo.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            AccNo.Focus()
            Return
        End If
        If Val(AccNo1.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            AccNo1.Focus()
            Return
        End If
        If Val(AccNo2.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            AccNo2.Focus()
            Return
        End If
        If Val(AccNo3.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            AccNo3.Focus()
            Return
        End If
        Bal0.Text = Val(Bal0.Text.Trim)
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls()
        CurrencyId_LostFocus(Nothing, Nothing)

        AccName.Clear()
        AccName1.Clear()
        AccName2.Clear()
        AccName3.Clear()
        txtName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Val(bm.ExecuteScalar("select dbo.GetSubAccUsingCount(" & MyLinkFile & ",'" & txtID.Text.Trim & "')")) > 0 Then
                bm.ShowMSG("غير مسموح بمسح حساب عليه حركات")
                Exit Sub
            End If
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
            txtName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        txtName.SelectAll()
        txtName.Focus()
        txtName.SelectAll()
        txtName.Focus()
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, AccNo.KeyDown, AccNo1.KeyDown, AccNo2.KeyDown, AccNo3.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyUp(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp(CType(Parent, Page).Title, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName, TableName) Then
            txtName.Focus()
        End If
    End Sub


    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Bal0.KeyDown, MainBal0.KeyDown, Exchange.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub


    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , MyLinkFile, )
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , MyLinkFile, )
    End Sub

    Private Sub AccNo1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo1.LostFocus
        bm.AccNoLostFocus(AccNo1, AccName1, , , True)
    End Sub

    Private Sub AccNo1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo1.KeyUp
        bm.AccNoShowHelp(AccNo1, AccName1, e, , , True)
    End Sub

    Private Sub AccNo2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo2.LostFocus
        bm.AccNoLostFocus(AccNo2, AccName2, , , True)
    End Sub

    Private Sub AccNo2_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo2.KeyUp
        bm.AccNoShowHelp(AccNo2, AccName2, e, , , True)
    End Sub

    Private Sub AccNo3_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo3.LostFocus
        bm.AccNoLostFocus(AccNo3, AccName3, , , True)
    End Sub

    Private Sub AccNo3_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo3.KeyUp
        bm.AccNoShowHelp(AccNo3, AccName3, e, , , True)
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblId.SetResourceReference(ContentProperty, "Id")
        lblName.SetResourceReference(ContentProperty, "Name")
        'lblAccNo.SetResourceReference(ContentProperty, "AccNo")

    End Sub

    Private Sub CurrencyId_LostFocus(sender As Object, e As RoutedEventArgs) Handles CurrencyId.LostFocus
        Try
            Exchange.Text = bm.ExecuteScalar("select dbo.GetCurrencyExchange(0,0," & CurrencyId.SelectedValue.ToString & ",0,getdate())")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub MainBal0_TextChanged(sender As Object, e As TextChangedEventArgs) Handles MainBal0.TextChanged, Exchange.TextChanged
        Bal0.Text = Val(MainBal0.Text) * Val(Exchange.Text)
    End Sub
End Class