Imports System.Data

Public Class ServiceTypes
    Public MainTableName As String = "ServiceGroups"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"

    Public TableName As String = "ServiceTypes"
    Public MainId As String = "ServiceGroupId"
    Public SubId As String = "Id"
    Public SubName As String = "Name"
    Public lblMain_Content As String = "Service Group"

    Public TableDetailsName As String = "ServiceCompanies"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Public WithImage As Boolean = False
    Public ReLoadMenue As Boolean = False

    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        LoadWFH()

        If WithImage Then
            btnSetImage.Visibility = Visibility.Visible
            btnSetNoImage.Visibility = Visibility.Visible
            Image1.Visibility = Visibility.Visible
        End If

        bm.FillCombo(MainTableName, CboMain, "")
        bm.Fields = {MainId, SubId, SubName, "DepartmentId", "DrValue", "CsValue", "CoValue", "IsStopped", "IsDaily", "CompanyValue", "Cost"}
        bm.control = {CboMain, txtID, txtName, DepartmentId, DrValue, CsValue, CoValue, IsStopped, IsDaily, CompanyValue, Cost}
        bm.KeyFields = {MainId, SubId}

        bm.Table_Name = TableName
        btnNew_Click(sender, e)

        If Md.MyProjectType = ProjectType.X Then
            IsDaily.Visibility = Visibility.Hidden
        End If
    End Sub


    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Price As String = "Price"
        Shared Payed As String = "Payed"
        Shared Remaining As String = "Remaining"
        Shared DrValue As String = "DrValue"
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Id, "كود الشركة")
        G.Columns.Add(GC.Name, "الشركة")
        G.Columns.Add(GC.Price, "السعر")
        G.Columns.Add(GC.Payed, "حصة المريض")
        G.Columns.Add(GC.Remaining, "حصة الشركة")
        G.Columns.Add(GC.DrValue, "حصة الطبيب")
        G.Columns.Add(GC.Notes, "ملاحظات")

        G.Columns(GC.Name).FillWeight = 280

        G.Columns(GC.Id).ReadOnly = True
        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Id).Visible = False

        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False

         
        G.Columns(GC.Name).FillWeight = 300
        G.Columns(GC.Notes).FillWeight = 400
         
        G.Columns(GC.Id).ReadOnly = True 
        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Remaining).ReadOnly = True 
        G.Columns(GC.Id).Visible = False

        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False
        
        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.PreviewKeyDown, AddressOf G_PreviewKeyDown
    End Sub
    Private Sub GridCalcRow(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            If G.CurrentCell.ColumnIndex = G.Columns(GC.Price).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Payed).Index Then
                G.Rows(G.CurrentCell.RowIndex).Cells(GC.Remaining).Value = Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Price).Value) - Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Payed).Value)
            End If
        Catch
        End Try
    End Sub
    Private Sub G_PreviewKeyDown(sender As Object, e As Forms.PreviewKeyDownEventArgs)
        If e.KeyCode = Forms.Keys.Delete AndAlso G.CurrentRow.Index >= 0 AndAlso bm.ShowDeleteMSG() Then
            G.Rows.Remove(G.CurrentRow)
        End If
    End Sub

    Dim lop As Boolean = False
    Sub FillControls()
        lop = True
        bm.FillControls(Me)
        lop = False
        If WithImage Then bm.GetImage(TableName, New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}, "Image", Image1)
        DepartmentId_LostFocus(Nothing, Nothing)
        FillGrid()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Or CboMain.SelectedValue.ToString = 0 Then
            Return
        End If
        DrValue.Text = Val(DrValue.Text)
        CsValue.Text = Val(CsValue.Text)
        CoValue.Text = Val(CoValue.Text)

        G.EndEdit()

        bm.DefineValues()
        If Not bm.Save(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}) Then Return
        If WithImage Then bm.SaveImage(TableName, New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}, "Image", Image1)

        If Not bm.SaveGrid(G, TableDetailsName, New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text}, New String() {"CompanyId", "Price", "Payed", "Remaining", "DrValue", "Notes"}, New String() {GC.Id, GC.Price, GC.Payed, GC.Remaining, GC.DrValue, GC.Notes}, New VariantType() {VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.String}, New String() {GC.Id}) Then Return

        btnNew_Click(sender, e)
        
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'bm.ClearControls()
        ClearControls()
        FillGrid()
    End Sub

    Sub ClearControls()
        Try
            bm.ClearControls()
            bm.SetNoImage(Image1)
            DepartmentName.Clear()
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & CboMain.SelectedValue.ToString & "'")
            If txtID.Text = "" Then txtID.Text = "1"
            txtName.Focus()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "' and " & MainId & " ='" & CboMain.SelectedValue.ToString & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text}, "Back", dt)
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
        bm.RetrieveAll(New String() {MainId, SubId}, New String() {CboMain.SelectedValue.ToString, txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            btnNew_Click(Nothing, Nothing)
            txtID.Text = s
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyUp(sender As Object, e As KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp(TableName, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName & " where " & MainId & "=" & CboMain.SelectedValue.ToString) Then
            txtName.Focus()
        End If
    End Sub

    Private Sub CboMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain.SelectionChanged
        If lop Then Return
        btnNew_Click(Nothing, Nothing)
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

    Private Sub DrValue_TextChanged(sender As Object, e As TextChangedEventArgs) Handles DrValue.TextChanged, CsValue.TextChanged, CoValue.TextChanged
        Value.Text = Val(DrValue.Text) + Val(CsValue.Text) + Val(CoValue.Text)
    End Sub

    Private Sub DepartmentId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DepartmentId.KeyUp
        bm.ShowHelp("Departments", DepartmentId, DepartmentName, e, "select cast(Id as varchar(100)) Id,Name from Departments", "Departments")
    End Sub

    Private Sub DepartmentId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DepartmentId.LostFocus
        bm.LostFocus(DepartmentId, DepartmentName, "select Name from Departments where Id=" & DepartmentId.Text.Trim())
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblDepartmentId.SetResourceReference(ContentProperty, "Department")
        lblMain.SetResourceReference(ContentProperty, lblMain_Content)
        LblId.SetResourceReference(ContentProperty, "Id")
        LblName.SetResourceReference(ContentProperty, "Name")
        lblCoValue.SetResourceReference(ContentProperty, "CoValue")
        lblCsValue.SetResourceReference(ContentProperty, "CsValue")
        lblDrValue.SetResourceReference(ContentProperty, "DrValue")
        
        lblValue.SetResourceReference(ContentProperty, "Value")
        IsStopped.SetResourceReference(CheckBox.ContentProperty, "IsStopped")
        IsDaily.SetResourceReference(CheckBox.ContentProperty, "IsDaily")
        
    End Sub

    Private Sub FillGrid()
        dt = bm.ExecuteAdapter("select C.Id,C.Name,isnull(T.Price,0)Price,isnull(T.Payed,0)Payed,isnull(T.Remaining,0)Remaining,isnull(T.DrValue,0)DrValue,isnull(T.Notes,'')Notes from Companies C left join ServiceCompanies T on(T.CompanyId=C.Id and T." & SubId & "='" & txtID.Text.Trim & "' and T." & MainId & " ='" & CboMain.SelectedValue.ToString & "')")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add({dt.Rows(i)("Id"), dt.Rows(i)("Name"), dt.Rows(i)("Price"), dt.Rows(i)("Payed"), dt.Rows(i)("Remaining"), dt.Rows(i)("DrValue"), dt.Rows(i)("Notes")})
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
        Next
    End Sub

    Private Sub Value_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Value.TextChanged
        CompanyValue.Text = Value.Text
    End Sub
End Class
