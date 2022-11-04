﻿Imports System.Data
Public Class Suppliers
    Public TableName As String = "Suppliers"
    Public SubId As String = "Id"
    Public SubName As String = "Name"



    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        If Not Md.ShowCurrency Then CurrencyId.Visibility = Visibility.Hidden
        LoadResource()
        bm.Fields = New String() {SubId, SubName, "EnName", "AccNo", "CountryId", "CityId", "AreaId", "Address", "HeadMaster", "Tel1", "Tel2", "Tel3", "Fax", "CommercialRegister", "TaxCard", "CurrencyId", "Exchange", "MainBal0", "Bal0", "ApplyCurrencyCycle"}
        bm.control = New Control() {txtID, txtName, txtEnName, AccNo, CountryId, CityId, AreaId, Address, HeadMaster, Tel1, Tel2, Tel3, Fax, CommercialRegister, TaxCard, CurrencyId, Exchange, MainBal0, Bal0, ApplyCurrencyCycle}
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
        CountryId_LostFocus(Nothing, Nothing)
        CityId_LostFocus(Nothing, Nothing)
        AreaId_LostFocus(Nothing, Nothing)
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then 'OrElse Not bm.TestNames(txtName, txtEnName)
            txtName.Focus()
            Return
        End If
        If Val(AccNo.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            AccNo.Focus()
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
        CountryName.Clear()
        CityName.Clear()
        AreaName.Clear()

        txtName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Val(bm.ExecuteScalar("select dbo.GetSubAccUsingCount(" & 2 & ",'" & txtID.Text.Trim & "')")) > 0 Then
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

    Private Sub CountryId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CountryId.KeyUp
        bm.ShowHelp("Countries", CountryId, CountryName, e, "select cast(Id as varchar(100)) Id,Name from Countries", "Countries")
    End Sub

    Private Sub CityId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CityId.KeyUp
        bm.ShowHelp("Cities", CityId, CityName, e, "select cast(Id as varchar(100)) Id,Name from Cities where CountryId=" & CountryId.Text.Trim, "Cities", {"CountryId"}, {Val(CountryId.Text)})
    End Sub

    Private Sub AreaId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AreaId.KeyUp
        bm.ShowHelp("Areas", AreaId, AreaName, e, "select cast(Id as varchar(100)) Id,Name from Areas where CountryId=" & CountryId.Text.Trim & " and CityId=" & CityId.Text, "Areas", {"CountryId", "CityId"}, {Val(CountryId.Text), Val(CityId.Text)})
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, CountryId.KeyDown, CityId.KeyDown, AreaId.KeyDown, AccNo.KeyDown
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



    Private Sub CountryId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId.LostFocus
        bm.LostFocus(CountryId, CountryName, "select Name from Countries where Id=" & CountryId.Text.Trim())
        CityId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CityId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CityId.LostFocus
        bm.LostFocus(CityId, CityName, "select Name from Cities where CountryId=" & CountryId.Text.Trim & " and Id=" & CityId.Text.Trim())
    End Sub

    Private Sub AreaId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AreaId.LostFocus
        bm.LostFocus(AreaId, AreaName, "select Name from Areas where CountryId=" & CountryId.Text.Trim & " and CityId=" & CityId.Text.Trim & " and Id=" & AreaId.Text.Trim())
    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , 2, )
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , 2, )
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
        lblAccNo.SetResourceReference(ContentProperty, "AccNo")
        lblAddress.SetResourceReference(ContentProperty, "Address")
        lblBal0.SetResourceReference(ContentProperty, "Bal0")
        lblCountryId.SetResourceReference(ContentProperty, "CountryId")
        lblCityId.SetResourceReference(ContentProperty, "CityId")
        lblAreaId.SetResourceReference(ContentProperty, "AreaId")
        
    End Sub

    Private Sub Name_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles txtName.LostFocus
        txtName.Text = txtName.Text.Trim
        txtEnName.Text = bm.GetEnName(txtName.Text.Trim)
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
