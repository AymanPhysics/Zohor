Imports System.Data

Public Class NolonPriceTypes
    Public TableName As String = ""
    Public lblName_text As String = ""
    Public lblName2_text As String = ""

    Public SubId As String = "Id"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()

        bm.Fields = {SubId, "CountryId", "CityId", "AreaId", "CountryId1", "CityId1", "AreaId1", "Price1", "Price2", "Price3", "Price4", "Price5"}
        bm.control = {txtID, CountryId, CityId, AreaId, CountryId1, CityId1, AreaId1, Price1, Price2, Price3, Price4, Price5}
        bm.KeyFields = {SubId}

        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CType(Application.Current.MainWindow, MainWindow).TabControl1.Items.Remove(Me.Parent)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If AreaId.Text.Trim = "" Then
            AreaId.Focus()
            Return
        End If
        If AreaId1.Text.Trim = "" Then
            AreaId1.Focus()
            Return
        End If

        Price1.Text = Val(Price1.Text)
        Price2.Text = Val(Price2.Text)
        Price3.Text = Val(Price3.Text)
        Price4.Text = Val(Price4.Text)
        Price5.Text = Val(Price5.Text)

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
        CountryName.Clear()
        CityName.Clear()
        AreaName.Clear()
        CountryName1.Clear()
        CityName1.Clear()
        AreaName1.Clear()

        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        CountryId.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG("MsgDelete") Then
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
            ClearControls()
            CountryId.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        CountryId.SelectAll()
        CountryId.Focus()
        CountryId.SelectAll()
        CountryId.Focus()
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, CountryId.KeyDown, CityId.KeyDown, AreaId.KeyDown, CountryId1.KeyDown, CityId1.KeyDown, AreaId1.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID2_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Price1.KeyDown, Price2.KeyDown, Price3.KeyDown, Price4.KeyDown, Price5.KeyDown
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


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblId.SetResourceReference(ContentProperty, "Id")

    End Sub


    Private Sub CountryId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId.LostFocus
        bm.LostFocus(CountryId, CountryName, "select Name from Countries where Id=" & CountryId.Text.Trim())
        CityId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CityId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CityId.LostFocus
        bm.LostFocus(CityId, CityName, "select Name from Cities where CountryId=" & CountryId.Text.Trim & " and Id=" & CityId.Text.Trim())
        AreaId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub AreaId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AreaId.LostFocus
        bm.LostFocus(AreaId, AreaName, "select Name from Areas where CountryId=" & CountryId.Text.Trim & " and CityId=" & CityId.Text.Trim & " and Id=" & AreaId.Text.Trim())
    End Sub

    Private Sub CountryId1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId1.LostFocus
        bm.LostFocus(CountryId1, CountryName1, "select Name from Countries where Id=" & CountryId1.Text.Trim())
        CityId1_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CityId1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CityId1.LostFocus
        bm.LostFocus(CityId1, CityName1, "select Name from Cities where CountryId=" & CountryId1.Text.Trim & " and Id=" & CityId1.Text.Trim())
        AreaId1_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub AreaId1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AreaId1.LostFocus
        bm.LostFocus(AreaId1, AreaName1, "select Name from Areas where CountryId=" & CountryId1.Text.Trim & " and CityId=" & CityId1.Text.Trim & " and Id=" & AreaId1.Text.Trim())
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

    Private Sub CountryId1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CountryId1.KeyUp
        bm.ShowHelp("Countries", CountryId1, CountryName1, e, "select cast(Id as varchar(100)) Id,Name from Countries")
    End Sub

    Private Sub CityId1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CityId1.KeyUp
        bm.ShowHelp("Cities", CityId1, CityName1, e, "select cast(Id as varchar(100)) Id,Name from Cities where CountryId=" & CountryId1.Text.Trim)
    End Sub

    Private Sub AreaId1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AreaId1.KeyUp
        bm.ShowHelp("Areas", AreaId1, AreaName1, e, "select cast(Id as varchar(100)) Id,Name from Areas where CountryId=" & CountryId1.Text.Trim & " and CityId=" & CityId1.Text)
    End Sub

    Private Sub FillControls()
        bm.FillControls(Me)
        CountryId_LostFocus(Nothing, Nothing)
        CityId_LostFocus(Nothing, Nothing)
        AreaId_LostFocus(Nothing, Nothing)
        CountryId1_LostFocus(Nothing, Nothing)
        CityId1_LostFocus(Nothing, Nothing)
        AreaId1_LostFocus(Nothing, Nothing)
    End Sub


End Class
