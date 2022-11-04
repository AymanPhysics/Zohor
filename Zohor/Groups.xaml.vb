﻿Imports System.Data

Public Class Groups
    Public TableName As String = "Groups"
    Public SubId As String = "Id"
    Public SubName As String = "Name"



    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Public WithImage As Boolean = False

    Public Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If Not Md.MyProjectType = ProjectType.PCs Then
            txtName2.Visibility = Visibility.Hidden
        End If

        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        If WithImage Then
            btnSetImage.Visibility = Visibility.Visible
            btnSetNoImage.Visibility = Visibility.Visible
            Image1.Visibility = Visibility.Visible
        End If
        bm.Fields = New String() {SubId, SubName, "TradingFirstAccNo", "TradingLastAccNo", "BudgetAccNo"}
        bm.control = New Control() {txtID, txtName, TradingFirstAccNo, TradingLastAccNo, BudgetAccNo}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)
        If Md.MyProjectType = ProjectType.X Then
            CheckBox1.Visibility = Visibility.Hidden
        ElseIf Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            CheckBox1.Content = "مستهلكات"
        End If

        If Not Md.MyProjectType = ProjectType.Zohor AndAlso Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X Then bm.SetImage(Img, "attchmentfile.jpg")

        If Md.MyProjectType = ProjectType.X Then
            lblTradingFirstAccNo.Visibility = Visibility.Hidden
            TradingFirstAccNo.Visibility = Visibility.Hidden
            TradingFirstAccName.Visibility = Visibility.Hidden

            lblTradingLastAccNo.Visibility = Visibility.Hidden
            TradingLastAccNo.Visibility = Visibility.Hidden
            TradingLastAccName.Visibility = Visibility.Hidden

            lblBudgetAccNo.Visibility = Visibility.Hidden
            BudgetAccNo.Visibility = Visibility.Hidden
            BudgetAccName.Visibility = Visibility.Hidden
        End If
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        If WithImage Then bm.GetImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        TradingFirstAccNo_LostFocus(Nothing, Nothing)
        TradingLastAccNo_LostFocus(Nothing, Nothing)
        BudgetAccNo_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub TradingFirstAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TradingFirstAccNo.LostFocus
        bm.AccNoLostFocus(TradingFirstAccNo, TradingFirstAccName)
    End Sub

    Private Sub TradingFirstAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles TradingFirstAccNo.KeyUp
        bm.AccNoShowHelp(TradingFirstAccNo, TradingFirstAccName, e)
    End Sub

    Private Sub TradingLastAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TradingLastAccNo.LostFocus
        bm.AccNoLostFocus(TradingLastAccNo, TradingLastAccName)
    End Sub

    Private Sub TradingLastAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles TradingLastAccNo.KeyUp
        bm.AccNoShowHelp(TradingLastAccNo, TradingLastAccName, e)
    End Sub

    Private Sub BudgetAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BudgetAccNo.LostFocus
        bm.AccNoLostFocus(BudgetAccNo, BudgetAccName)
    End Sub

    Private Sub BudgetAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles BudgetAccNo.KeyUp
        bm.AccNoShowHelp(BudgetAccNo, BudgetAccName, e)
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
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If

        If TradingFirstAccNo.Visibility = Visibility.Visible AndAlso Val(TradingFirstAccNo.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            TradingFirstAccNo.Focus()
            Return
        End If
        If TradingLastAccNo.Visibility = Visibility.Visible AndAlso Val(TradingLastAccNo.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            TradingLastAccNo.Focus()
            Return
        End If
        If BudgetAccNo.Visibility = Visibility.Visible AndAlso Val(BudgetAccNo.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            BudgetAccNo.Focus()
            Return
        End If


        Dim x As Integer = Val(bm.ExecuteScalar("select Id from " & TableName & " where Name='" & txtName.Text.Trim & "' and Id<>" & Val(txtID.Text)))
        If x > 0 Then
            bm.ShowMSG("تم تكرار الاسم بمسلسل رقم " & x)
            Return
        End If

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        If WithImage Then bm.SaveImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
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
        bm.SetNoImage(Image1)
        bm.ClearControls(False)

        TradingFirstAccName.Clear()
        TradingLastAccName.Clear()
        BudgetAccName.Clear()

        CheckBox2.IsChecked = True
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
        bm.ShowHelp(CType(Parent, Page).Title, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName, TableName)
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


    Private Sub btnSetImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetImage.Click
        bm.SetImage(Image1)
    End Sub

    Private Sub btnSetNoImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetNoImage.Click
        bm.SetNoImage(Image1, False, True)
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
    End Sub

    Private Sub txtName_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtName.TextChanged
        txtName2.Text = bm.Encrypt(txtName.Text)
    End Sub
End Class
