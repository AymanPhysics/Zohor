Imports System.Data

Public Class ServiceGroups

    Public CboTableName As String = "SerialTypes"
    Public lblName2_text As String = "Serial Type"

    Public TableName As String = "ServiceGroups"
    Public SubId As String = "Id"
    Public SubName As String = "Name"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()

        Select Case Md.MyProjectType
            Case ProjectType.X, ProjectType.X
            Case Else
                lblMainAcc.Visibility = Visibility.Hidden
                MainAccNo.Visibility = Visibility.Hidden
                MainAccName.Visibility = Visibility.Hidden
                lblSubAcc.Visibility = Visibility.Hidden
                SubAccNo.Visibility = Visibility.Hidden
                SubAccName.Visibility = Visibility.Hidden
        End Select

        bm.FillCombo(CboTableName, CboMain, "")
        bm.Fields = {SubId, SubName, "SerialId", "MainAccNo", "SubAccNo", "IsTotal", "IsService1", "IsService2"}
        bm.control = {txtID, txtName, CboMain, MainAccNo, SubAccNo, IsTotal, IsService1, IsService2}
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

    Sub FillControls()
        bm.FillControls(Me)
        MainAccNo_LostFocus(Nothing, Nothing)
        SubAccNo_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If

        If MainAccNo.Visibility = Visibility.Visible AndAlso Val(MainAccNo.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الحساب العام")
            MainAccNo.Focus()
            Return
        End If
        If SubAccNo.Visibility = Visibility.Visible AndAlso Val(SubAccNo.Text) = 0 AndAlso SubAccNo.IsEnabled Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo.Focus()
            Return
        End If

        SubAccNo.Text = Val(SubAccNo.Text)
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
        IsService1.IsChecked = True
        IsService2.IsChecked = True
        MainAccNo_LostFocus(Nothing, Nothing)
        SubAccNo_LostFocus(Nothing, Nothing)
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        txtName.Focus()
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

    Private Sub txtID_KeyUp(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyUp
        bm.ShowHelp(CType(Parent, Page).Title, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName)
    End Sub

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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
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

        LblId.SetResourceReference(ContentProperty, "Id")
        LblName.SetResourceReference(ContentProperty, "Name")
        lblName2.SetResourceReference(ContentProperty, lblName2_text)

    End Sub

    Dim lop As Boolean = False
    Private Sub SubAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubAccNo.LostFocus
        If lop OrElse SubAccNo.Visibility = Visibility.Hidden Then Return
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
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & dt.Rows(0)("Id") & "," & Md.UserName & ") where AccNo='" & MainAccNo.Text & "'") Then
            SubAccNo_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub MainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo.LostFocus
        If MainAccNo.Visibility = Visibility.Hidden Then Return

        bm.AccNoLostFocus(MainAccNo, MainAccName)

        SubAccNo.IsEnabled = MainAccNo.Visibility <> Visibility.Visible OrElse bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')").Rows.Count > 0
        SubAccNo_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles MainAccNo.KeyUp
        bm.AccNoShowHelp(MainAccNo, MainAccName, e)
    End Sub

    Private Sub IsService1_Unchecked(sender As Object, e As RoutedEventArgs) Handles IsService1.Unchecked
        IsService2.IsChecked = True
    End Sub

    Private Sub IsService2_Unchecked(sender As Object, e As RoutedEventArgs) Handles IsService2.Unchecked
        IsService1.IsChecked = True
    End Sub

End Class
