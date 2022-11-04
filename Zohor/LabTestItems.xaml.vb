Imports System.Data

Public Class LabTestItems
    Public MainTableName As String = "LaboratoryTestTypes"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"


    Public Main2TableName As String = "LaboratoryTests"
    Public Main2MainId As String = "TestId"
    Public Main2SubId As String = "Id"
    Public Main2SubName As String = "Name"


    Public TableName As String = "LabTestItems"
    Public MainId As String = "TestId"
    Public MainId2 As String = "SubTestId"
    Public SubId As String = "Id"
    Public SubName As String = "Name"

    Public lblMain_Content As String = "TestId"
    Public lblMain2_Content As String = "SubTestId"


    Dim dt As New DataTable
    Dim bm As New BasicMethods
    WithEvents G0 As New MyGrid
    WithEvents G As New MyGrid


    Private Sub LabTestItems_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()
        LoadWFH0()
        LoadWFH()
        bm.FillCombo(MainTableName, CboMain, "")
        bm.Fields = New String() {MainId, MainId2, SubId, SubName, "Unit", "RefrenceRange"}
        bm.control = New Control() {CboMain, CboMain2, txtID, txtName, Unit, RefrenceRange}
        bm.KeyFields = New String() {MainId, MainId2, SubId}

        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub
    Structure GC0
        Shared NormalValue As String = "NormalValue"
    End Structure
    Structure GC
        Shared Result As String = "Result"
    End Structure


    Private Sub LoadWFH0()
        WFH0.Child = G0
        G0.Columns.Clear()
        G0.ForeColor = System.Drawing.Color.DarkBlue
        G0.Columns.Add(GC0.NormalValue, "Normal Value")

        G0.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G0.AllowUserToDeleteRows = True
        G0.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G0.TabStop = False

    End Sub

    Private Sub LoadWFH()
        WFH.Child = G
        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Result, "Result")

        G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G.AllowUserToDeleteRows = True
        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G.TabStop = False

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, MainId2, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Or CboMain.SelectedValue.ToString = 0 Or CboMain2.SelectedValue.ToString = 0 Then
            Return
        End If
        G0.EndEdit()
        G.EndEdit()
        DelivaryCost.Text = Val(DelivaryCost.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text.Trim}) Then Return


        Dim str0 As String = "delete LabTestItemsResults where TestId='" & Val(CboMain.SelectedValue) & "' and SubTestId='" & Val(CboMain2.SelectedValue) & "' and Id='" & Val(txtID.Text) & "'    delete LabTestItemsNormalValues where TestId='" & Val(CboMain.SelectedValue) & "' and SubTestId='" & Val(CboMain2.SelectedValue) & "' and Id='" & Val(txtID.Text) & "'"

        If Not bm.ExecuteNonQuery(str0) Then Return

        Dim str As String = "Insert LabTestItemsNormalValues(TestId,SubTestId,Id,NormalValue) values "
        For i As Integer = 0 To G0.Rows.Count - 1
            Try
                If G0.Rows(i).Cells(GC0.NormalValue).Value.ToString.Trim = "" Then Continue For
                str &= "('" & Val(CboMain.SelectedValue) & "','" & Val(CboMain2.SelectedValue) & "','" & Val(txtID.Text) & "','" & G0.Rows(i).Cells(GC0.NormalValue).Value.ToString & "'),"
            Catch ex As Exception
            End Try
        Next
        str = str.Substring(0, str.Length - 1)
        bm.ExecuteNonQuery(str)

        str = "Insert LabTestItemsResults(TestId,SubTestId,Id,Result) values "
        For i As Integer = 0 To G.Rows.Count - 1
            Try
                If G.Rows(i).Cells(GC.Result).Value.ToString.Trim = "" Then Continue For
                str &= "('" & Val(CboMain.SelectedValue) & "','" & Val(CboMain2.SelectedValue) & "','" & Val(txtID.Text) & "','" & G.Rows(i).Cells(GC.Result).Value.ToString & "'),"
            Catch ex As Exception
            End Try
        Next
        str = str.Substring(0, str.Length - 1)
        bm.ExecuteNonQuery(str)


        btnNew_Click(sender, e)
        
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {MainId, MainId2, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        lop = True
        Try
            G0.Rows.Clear()
            G.Rows.Clear()
            txtName.Clear()
            RefrenceRange.IsChecked = False
            Unit.Clear()
            DelivaryCost.Clear()

            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "='" & CboMain2.SelectedValue.ToString & "'")
            If txtID.Text = "" Then txtID.Text = "1"

            txtName.Focus()
        Catch
        End Try
        lop = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "' and " & MainId & " ='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & " ='" & CboMain2.SelectedValue.ToString & "'    delete LabTestItemsResults where TestId='" & Val(CboMain.SelectedValue) & "' and SubTestId='" & Val(CboMain2.SelectedValue) & "' and Id='" & Val(txtID.Text) & "'    delete LabTestItemsNormalValues where TestId='" & Val(CboMain.SelectedValue) & "' and SubTestId='" & Val(CboMain2.SelectedValue) & "' and Id='" & Val(txtID.Text) & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_KeyUp(sender As Object, e As KeyEventArgs) Handles txtID.KeyUp
        Try
            bm.ShowHelp("Laboratory Test Items", txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from LabTestItems where TestId='" & CboMain.SelectedValue.ToString & "' and SubTestId='" & CboMain2.SelectedValue.ToString & "'")
            txtName.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text.Trim}, dt)
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

    Private Sub DelivaryCost_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles DelivaryCost.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub CboMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain.SelectionChanged
        If lop Then Return
        Dim s As String = ""
        Try
            s = CboMain.SelectedValue.ToString
        Catch ex As Exception
        End Try
        bm.FillCombo(Main2TableName, CboMain2, " where " & Main2MainId & "='" & s & "'")
        ClearControls()
    End Sub

    Private Sub CboMain2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain2.SelectionChanged
        ClearControls()
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

        lblMain.SetResourceReference(ContentProperty, lblMain_Content)
        lblMain2.SetResourceReference(ContentProperty, lblMain2_Content)
        LblId.SetResourceReference(ContentProperty, "Id")
        LblName.SetResourceReference(ContentProperty, "Name")
        LblRefrenceRange.SetResourceReference(ContentProperty, "RefrenceRange")
        RefrenceRange.SetResourceReference(CheckBox.ContentProperty, "All Normal Values")
        LblUnit.SetResourceReference(ContentProperty, "Unit")
        TabItem0.SetResourceReference(TabItem.HeaderProperty, "Normal Value")
        TabItem1.SetResourceReference(TabItem.HeaderProperty, "Default Result")

    End Sub

    Dim lop As Boolean = False
    Private Sub FillControls()
        bm.FillControls(Me)
        CboMain_SelectedIndexChanged(Nothing, Nothing)
        lop = True
        bm.FillControls(Me)


        Dim dt As DataTable = bm.ExecuteAdapter("select * from LabTestItemsResults where TestId='" & Val(CboMain.SelectedValue) & "' and SubTestId='" & Val(CboMain2.SelectedValue) & "' and Id='" & Val(txtID.Text) & "'")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Result).Value = dt.Rows(i)("Result").ToString
        Next
        G.RefreshEdit()



        Dim dt0 As DataTable = bm.ExecuteAdapter("select * from LabTestItemsNormalValues where TestId='" & Val(CboMain.SelectedValue) & "' and SubTestId='" & Val(CboMain2.SelectedValue) & "' and Id='" & Val(txtID.Text) & "'")
        G0.Rows.Clear()
        For i As Integer = 0 To dt0.Rows.Count - 1
            G0.Rows.Add()
            G0.Rows(i).Cells(GC0.NormalValue).Value = dt0.Rows(i)("NormalValue").ToString
        Next
        G0.RefreshEdit()


        lop = False
    End Sub

End Class
