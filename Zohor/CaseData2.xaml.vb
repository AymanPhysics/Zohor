Imports System.Data
Imports System.ComponentModel
Imports System.IO

Public Class CaseData2

    Public TableName As String = "CaseFuckingData"
    Public MainId As String = "CaseId"
    Public MainId2 As String = "Mykey"
    Public MainId3 As String = "MyFlag"
    Public SubId As String = "Id"
    Public SubName As String = "Name"
    Public SubName2 As String = "Name2"
    Public SubName3 As String = "Name3"
    Public SubName4 As String = "Name4"
    Public SubName5 As String = "Name5"
    Public SubName6 As String = "Name6"
    Public SubName7 As String = "Name7"
    Public SubName8 As String = "Name8"
    Public lblMain_Content As String


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Public WithImage As Boolean = False
    Public ReLoadMenue As Boolean = False

    Public MyCase As Integer
    Public MyFlag As String
    Public MyKey As Integer

    Dim WithEvents BackgroundWorker1 As New BackgroundWorker

    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {}, {}, {}, {})
        'bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        If WithImage Then
            btnSetImage.Visibility = Visibility.Visible
            btnSetNoImage.Visibility = Visibility.Visible
            Image1.Visibility = Visibility.Visible
        End If

        bm.FillCombo("select Id,Name from Cases union select 0 Id,'-' Name order by Name", CboMain)

        Dim v() As String = {MainId, MainId2, MainId3, SubId, SubName, SubName2, SubName3, SubName4, SubName5, SubName6, SubName7, SubName8, "DayDate"}
        bm.Fields = v

        Dim c() As Control = {CboMain, txtMyKey, txtMyFlag, txtID, txtName, txtName2, txtName3, txtName4, txtName5, txtName6, txtName7, txtName8, DayDate}
        bm.control = c

        Dim k() As String = {MainId, MainId2, MainId3, SubId}
        bm.KeyFields = k

    
        bm.Table_Name = TableName
        btnNew_Click(sender, e)

        Try
            Visibility = Visibility.Hidden
            Visibility = Visibility.Visible

        Catch ex As Exception
        End Try
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        LoadTree()
        If WithImage Then bm.GetImage(TableName, New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text.Trim}, "Image", Image1)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, MainId2, MainId3, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" AndAlso txtName2.Text.Trim = "" AndAlso txtName3.Text.Trim = "" AndAlso txtName4.Text.Trim = "" Then
            'Return
        End If
        If CboMain.SelectedValue.ToString = 0 Then
            Return
        End If

        bm.DefineValues()
        If Not bm.Save(New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text.Trim}) Then Return
        If WithImage Then bm.SaveImage(TableName, New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text.Trim}, "Image", Image1)
        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {MainId, MainId2, MainId3, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        Try
            TreeView1.Items.Clear()
            CboMain.SelectedValue = MyCase
            txtMyKey.Text = MyKey
            txtMyFlag.Text = MyFlag
            bm.SetNoImage(Image1)
            txtName.Clear()
            txtName2.Clear()
            txtName3.Clear()
            txtName4.Clear()
            txtName5.Clear()
            txtName6.Clear()
            txtName7.Clear()
            txtName8.Clear()
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "='" & txtMyKey.Text & "' and " & MainId3 & "='" & txtMyFlag.Text & "'")
            If txtID.Text = "" Then txtID.Text = "1"
            DayDate.SelectedDate = Now.Date
            txtName.Focus()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "' and " & MainId & " ='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "='" & txtMyKey.Text & "' and " & MainId3 & "='" & txtMyFlag.Text & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub CboMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain.SelectionChanged
        ClearControls()
    End Sub

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

    End Sub


    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDownload.Click
        Try
            MyImagedata = Nothing
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize <> 18 Then Return
            Dim s As New Forms.SaveFileDialog With {.Filter = "All files (*.*)|*.*"}
            s.Filter = "All files (*.*)|*.*"
            s.FileName = CType(TreeView1.SelectedItem, TreeViewItem).Header

            If IsNothing(sender) Then
                MyBath = bm.GetNewTempName(s.FileName)
            Else
                If Not s.ShowDialog = Forms.DialogResult.OK Then Return
                MyBath = s.FileName
            End If

            btnDownload.IsEnabled = False
            F1 = CboMain.SelectedValue 'txtID.Text
            F2 = CType(TreeView1.SelectedItem, TreeViewItem).Tag
            BackgroundWorker1.RunWorkerAsync()
        Catch ex As Exception
        End Try
    End Sub
    Dim F2 As String = "", F1 As String = ""
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim myCommand As SqlClient.SqlCommand
            myCommand = New SqlClient.SqlCommand("select Image from CaseDataAttachments where CaseId='" & F1 & "' and AttachedName='" & F2 & "' and Mykey='" & MyKey & "' and MyFlag='" & MyFlag & "'" & bm.AppendWhere, con)
            If con.State <> ConnectionState.Open Then con.Open()
            MyImagedata = CType(myCommand.ExecuteScalar(), Byte())
        Catch ex As Exception
        End Try
        con.Close()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            File.WriteAllBytes(MyBath, MyImagedata)
            Process.Start(MyBath)
        Catch ex As Exception
        End Try
        btnDownload.IsEnabled = True
    End Sub

    Dim MyImagedata() As Byte
    Dim MyBath As String = ""
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteFile.Click
        Try
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize = 18 Then
                If bm.ShowDeleteMSG("MsgDeleteFile") Then
                    bm.ExecuteNonQuery("delete from CaseDataAttachments where CaseId='" & CboMain.SelectedValue & "' and Mykey='" & MyKey & "' and MyFlag='" & MyFlag & "' and AttachedName='" & TreeView1.SelectedItem.Header & "'" & bm.AppendWhere)
                    LoadTree()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadTree()
        Dim dt As DataTable = bm.ExecuteAdapter("select AttachedName from CaseDataAttachments where CaseId=" & CboMain.SelectedValue & "  and Mykey='" & MyKey & "' and MyFlag='" & MyFlag & "' " & bm.AppendWhere)
        TreeView1.Items.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim nn As New TreeViewItem
            nn.Foreground = Brushes.DarkRed
            nn.FontSize = 18
            nn.Tag = dt.Rows(i)(0).ToString
            nn.Header = dt.Rows(i)(0).ToString
            TreeView1.Items.Add(nn)
        Next
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAttach.Click
        DontClear = True
        btnSave_Click(btnSave, Nothing)
        DontClear = False
        If Not AllowSave Then Return

        Dim o As New Forms.OpenFileDialog
        o.Multiselect = True
        If o.ShowDialog = Forms.DialogResult.OK Then
            For i As Integer = 0 To o.FileNames.Length - 1
                bm.SaveFile("CaseDataAttachments", "CaseId", CboMain.SelectedValue, "AttachedName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "Mykey", MyKey, "MyFlag", MyFlag, "Image", o.FileNames(i))
            Next
        End If
        LoadTree()
    End Sub


    Private Sub TreeView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TreeView1.MouseDoubleClick
        Button4_Click(Nothing, Nothing)
    End Sub



End Class
