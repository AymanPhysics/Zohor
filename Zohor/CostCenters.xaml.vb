Imports System.Data

Public Class CostCenters

    Public TableName As String = "CostCenters"
    Public SubId As String = "Id"
    Public MyHeader As String = "مراكز التكلفة"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Private Sub CostCenters_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()

        Dim v() As String = {SubId, "Name", "MainCostCenterId", "SubType"}
        bm.Fields = v
        Dim c() As Control = {txtID, txtName, MainCostCenterId, SubType}
        bm.control = c

        Dim k() As String = {SubId}
        bm.KeyFields = k

        bm.Table_Name = TableName
        LoadTree()
        btnNew_Click(sender, e)
    End Sub


    Sub LoadTree()
        TreeView1.Items.Clear()
        Dim dt As DataTable = bm.ExecuteAdapter("Select * from " & TableName & " order by Id")
        TreeView1.Items.Add(New TreeViewItem With {.Header = MyHeader})
        TreeView1.Items(0).Tag = ""
        TreeView1.Items(0).FontSize = 20
        Dim dr() As DataRow = dt.Select("MainCostCenterId=0")
        For i As Integer = 0 To dr.Length - 1
            Try
                Dim nn As New TreeViewItem
                nn.Foreground = Brushes.DarkRed
                nn.FontSize = 18
                nn.Name = "Node_" & dr(i)("Id")
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
        Dim dr() As DataRow = dt.Select("MainCostCenterId=" & nn.Tag)
        For i As Integer = 0 To dr.Length - 1
            Try
                Dim nn2 As New TreeViewItem
                nn2.Foreground = Brushes.DarkBlue
                nn2.FontSize = nn.FontSize - 1
                nn2.Name = "Node_" & dr(i)("Id")
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
        MainCostCenterId_LostFocus(Nothing, Nothing)
        txtName.Focus()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        LoadTree()
        btnNew_Click(sender, e)
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
        bm.ClearControls()
        MainCostCenterId_LostFocus(Nothing, Nothing)
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
        If bm.ShowHelp(TableName, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName) Then
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


    Private Sub MainCostCenterId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainCostCenterId.LostFocus

        bm.CostCenterIdLostFocus(MainCostCenterId, MainCostCenterName, 0, TableName)
        If MainCostCenterId.Text = "" Then Return

        If MainCostCenterId.Text = txtID.Text Then
            bm.ShowMSG("Main Cost Center couldn't be Equal to Sub Cost Center")
            MainCostCenterId.Clear()
            MainCostCenterName.Clear()
        End If


    End Sub

    Private Sub MainCostCenterId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles MainCostCenterId.KeyUp
        bm.CostCenterIdShowHelp(MainCostCenterId, MainCostCenterName, e, 0, TableName)
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
        lblMainCostCenterId.SetResourceReference(ContentProperty, "MainCostCenterId")
        lblName.SetResourceReference(ContentProperty, "Name")
        lblSubType.SetResourceReference(ContentProperty, "CostType")


        bm.ResetComboboxContent(SubType)
    End Sub
End Class

