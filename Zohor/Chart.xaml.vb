Imports System.Data

Public Class Chart

    Public TableName As String = "Chart"
    Public SubId As String = "Id"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub Chart_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        bm.FillCombo("LinkFile", LinkFile, "", , True)
        Dim v() As String = {SubId, "Name", "MainAccId", "MotionType", "SubType", "LinkFile", "Level", "EndType", "Bal0", "Id2", "HideFromAccountEnd", "IsZakat"}
        bm.Fields = v
        Dim c() As Control = {txtID, txtName, AccNo, MotionType, SubType, LinkFile, Level, EndType, Bal0, Id2, HideFromAccountEnd, IsZakat}
        bm.control = c

        Dim k() As String = {SubId}
        bm.KeyFields = k

        bm.Table_Name = TableName

        'If Md.MyProjectType = ProjectType.SonacAlex Then
        '    PanelGroups.Visibility = Visibility.Hidden
        'End If

        LoadTree()
        btnNew_Click(sender, e)
    End Sub


    Sub LoadTree()
        'If Md.MyProjectType = ProjectType.SonacAlex Then Return

        TreeView1.Items.Clear()
        Dim dt As DataTable = bm.ExecuteAdapter("Select * from Chart order by Id")
        TreeView1.Items.Add(New TreeViewItem With {.Header = "دليل الحسابات"})
        TreeView1.Items(0).Tag = ""
        TreeView1.Items(0).FontSize = 20
        Dim dr() As DataRow = dt.Select("MainAccId=''")
        For i As Integer = 0 To dr.Length - 1
            Try
                Dim nn As New TreeViewItem
                nn.Foreground = Brushes.DarkRed
                nn.FontSize = 18
                'nn.Name = "Node_" & dr(i)("Id")
                nn.Tag = dr(i)("Id")
                nn.Header = dr(i)("Id") & "          " & dr(i)("Name")
                TreeView1.Items(0).Items.Add(nn)
                loadNode(dt, nn)
            Catch
            End Try
        Next
        CType(TreeView1.Items(0), TreeViewItem).IsExpanded = True
    End Sub

    Sub loadNode(ByVal dt As DataTable, ByVal nn As TreeViewItem)
        Dim dr() As DataRow = dt.Select("MainAccId='" & nn.Tag & "'")
        For i As Integer = 0 To dr.Length - 1
            Try
                Dim nn2 As New TreeViewItem
                nn2.Foreground = Brushes.DarkBlue
                nn2.FontSize = nn.FontSize - 1
                'nn2.Name = "Node_" & dr(i)("Id")
                nn2.Tag = dr(i)("Id")
                nn2.Header = dr(i)("Id") & "          " & dr(i)("Name")
                nn.Items.Add(nn2)
                loadNode(dt, nn2)
            Catch
            End Try
        Next
        nn.IsExpanded = True
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
        AccNo_LostFocus(Nothing, Nothing)
        Id2_LostFocus(Nothing, Nothing)
        bm.FillControls(Me)
        txtName.Focus()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If
        Bal0.Text = Val(Bal0.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        UpdateMotionType(txtID.Text, Val(Level.Text))
        bm.ExecuteNonQuery("exec UpdateChartBal0")
        LoadTree()
        btnNew_Click(sender, e)

    End Sub

    Sub UpdateMotionType(ByVal id As String, ByVal lvl As Integer)
        Dim dt As DataTable = bm.ExecuteAdapter("select Id from Chart where MainAccId='" & id & "'")
        For i As Integer = 0 To dt.Rows.Count - 1
            bm.ExecuteNonQuery("update Chart set MotionType=" & MotionType.SelectedIndex & ",EndType=" & EndType.SelectedIndex & ",Level=" & lvl + 1 & " where Id='" & dt.Rows(i)(0) & "'")
            UpdateMotionType(dt.Rows(i)(0), lvl + 1)
        Next
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
        txtID.Clear()
        Dim s As String = ""
        If Md.MyProjectType = ProjectType.X Then
            s = AccNo.Text
        End If
        bm.ClearControls()
        SubType.SelectedIndex = 1
        AccNo.Text = s
        AccNo_LostFocus(Nothing, Nothing)
        Id2_LostFocus(Nothing, Nothing)
        txtID.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            LoadTree()
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_KeyUp(sender As Object, e As KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp("Chart", txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from Chart") Then
            txtName.Focus()
        End If
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
        txtName.Focus()
        lv = False
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus

        AccName.Clear()
        MotionType.IsEnabled = True
        EndType.IsEnabled = True

        Level.Text = 1
        MotionType.SelectedIndex = 0
        EndType.SelectedIndex = 0

        If Val(AccNo.Text) = 0 Then
            Return
        End If

        bm.AccNoLostFocus(AccNo, AccName, 0, , )
        If AccNo.Text = "" Then Return

        If AccNo.Text = txtID.Text Then
            bm.ShowMSG("Main Account couldn't be Equal to Sub Account")
            AccNo.Clear()
            AccName.Clear()
        End If

        MotionType.IsEnabled = False
        EndType.IsEnabled = False

        Dim dt As DataTable = bm.ExecuteAdapter("select MotionType,EndType,Level from Chart where Id='" & AccNo.Text & "'")
        If dt.Rows.Count > 0 Then
            MotionType.SelectedIndex = Val(dt.Rows(0)("MotionType"))
            EndType.SelectedIndex = Val(dt.Rows(0)("EndType"))
            Level.Text = 1 + Val(dt.Rows(0)("Level"))
        End If

    End Sub
    Private Sub Id2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Id2.LostFocus
        Name2.Clear()
        If Val(Id2.Text) = 0 Then
            Return
        End If
        bm.AccNoLostFocus(Id2, Name2, -1, , , False)
    End Sub
    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, 0, , )
    End Sub

    Private Sub Id2_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Id2.KeyUp
        bm.AccNoShowHelp(Id2, Name2, e, 1, , , False)
    End Sub


    Private Sub SubType_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles SubType.SelectionChanged
        If SubType.SelectedIndex = 1 Then
            LinkFile.IsEnabled = True
            Id2.IsEnabled = True
        Else
            LinkFile.SelectedIndex = 0
            LinkFile.IsEnabled = False
            Id2.Clear()
            Id2.IsEnabled = False
        End If
        SetBal0()
    End Sub

    Private Sub TreeView1_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Object)) Handles TreeView1.SelectedItemChanged
        If TreeView1.SelectedItem Is Nothing Then Return
        txtID.Text = TreeView1.SelectedItem.Tag
        txtID_LostFocus(Nothing, Nothing)
        TreeView1.Focus()
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
        lblAccNo.SetResourceReference(ContentProperty, "AccNo")
        lblLinkFile.SetResourceReference(ContentProperty, "LinkFile")
        lblMotionType.SetResourceReference(ContentProperty, "MotionType")
        lblName.SetResourceReference(ContentProperty, "Name")
        lblSubType.SetResourceReference(ContentProperty, "SubType")


        bm.ResetComboboxContent(SubType)
        bm.ResetComboboxContent(MotionType)
    End Sub

    Private Sub SetBal0()
        If EndType.SelectedIndex = 3 AndAlso SubType.SelectedIndex = 1 AndAlso LinkFile.SelectedIndex = 0 Then
            Bal0.IsEnabled = True
        Else
            Bal0.IsEnabled = False
            'Bal0.Text = Val(bm.ExecuteScalar("select SUM(Bal0) from chart where MainAccId='" & txtID.Text & "'"))
            Bal0.Clear()
        End If

        If SubType.SelectedIndex = 1 AndAlso EndType.SelectedIndex = 3 Then
            IsZakat.IsEnabled = True
        Else
            IsZakat.IsEnabled = False
            IsZakat.IsChecked = False
        End If

    End Sub

    Private Sub LinkFile_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles LinkFile.SelectionChanged
        SetBal0()
    End Sub

    Private Sub EndType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles EndType.SelectionChanged
        SetBal0()


    End Sub
End Class

