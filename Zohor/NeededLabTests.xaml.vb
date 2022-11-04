Imports System.Data

Public Class NeededLabTests

    Public EmpId As Integer
    Public DatePicker1 As DateTime
    Public ReservId As Integer
    Public CaseId As Integer
    Public CaseName As String

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Public WithImage As Boolean = False
    WithEvents G5 As New MyGrid

    Public Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        LoadWFH5()

        LoadLaboratoryTestTypes()

        dt = bm.ExecuteAdapter("select * from ReservationTests where EmpId='" & EmpId & "' and DayDate='" & bm.ToStrDate(DatePicker1) & "' and ReservId='" & ReservId & "'")
        G5.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G5.Rows.Add()
            G5.Rows(i).Cells(GC5.Notes1).Value = dt.Rows(i)("Notes1").ToString
            G5.Rows(i).Cells(GC5.Notes2).Value = dt.Rows(i)("Notes2").ToString
            G5.Rows(i).Cells(GC5.Notes3).Value = dt.Rows(i)("Notes3").ToString
        Next
        G5.RefreshEdit()


        Try
            Visibility = Visibility.Hidden
            Visibility = Visibility.Visible

            Dim c = G5.Parent
            G5.Parent = Nothing
            G5.Parent = c
        Catch ex As Exception
        End Try
    End Sub

    Structure GC5
        Shared Notes1 As String = "Notes1"
        Shared Notes2 As String = "Notes2"
        Shared Notes3 As String = "Notes3"
    End Structure

    Private Sub LoadWFH5()
        WFH5.Child = G5

        G5.Columns.Clear()
        G5.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G5.ForeColor = System.Drawing.Color.DarkBlue
        G5.ColumnHeadersVisible = False

        G5.Columns.Add(GC5.Notes1, "")
        G5.Columns.Add(GC5.Notes2, "")
        G5.Columns.Add(GC5.Notes3, "")
        G5.Columns(GC5.Notes1).FillWeight = LaboratoryTestTypes.ActualWidth
        G5.Columns(GC5.Notes2).FillWeight = LaboratoryTests.ActualWidth
        G5.Columns(GC5.Notes3).FillWeight = LabTestItems.ActualWidth + Add5.ActualWidth

        G5.AllowUserToDeleteRows = True
        G5.EditMode = Forms.DataGridViewEditMode.EditOnEnter
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPrint.Click

        bm.SaveGrid(G5, "ReservationTests", New String() {"EmpId", "DayDate", "ReservId"}, New String() {EmpId, bm.ToStrDate(DatePicker1), ReservId}, New String() {"Notes1", "Notes2", "Notes3"}, New String() {GC5.Notes1, GC5.Notes2, GC5.Notes3}, New VariantType() {VariantType.String, VariantType.String, VariantType.String}, New String() {GC5.Notes1})


        If sender Is btnSave Then CType(Parent, Window).Close()
        If sender Is btnPrint Then
            Dim rpt As New ReportViewer
            rpt.Header = CType(Parent, Window).Title
            rpt.paraname = New String() {"@EmpId", "@CaseId", "CaseName", "@FromDate", "@ToDate", "@FromId", "@ToId", "@All", "Header", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
            rpt.paravalue = New String() {Val(EmpId), Val(CaseId), CaseName, bm.ToStrDate(DatePicker1), bm.ToStrDate(DatePicker1), ReservId, ReservId, 0, "Patient History", 1, 1, 1, 1, 1, 1, 1, 1, 1}
            rpt.Rpt = "cbo5.rpt"
            rpt.Show()
            'rpt.Print()

        End If
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")

    End Sub


    Private Sub Add5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add5.Click
        If LaboratoryTestTypes.Text.Trim = "" Then Return
        G5.Rows.Add(LaboratoryTestTypes.Text, LaboratoryTests.Text, LabTestItems.Text)
        bm.AddItemToTable("LaboratoryTestTypes", LaboratoryTestTypes.Text)
        Dim s As String = LaboratoryTestTypes.Text
        Dim s2 As String = LaboratoryTests.Text
        Dim s3 As String = LabTestItems.Text
        LoadLaboratoryTestTypes()
        LaboratoryTestTypes.Text = s
        LaboratoryTests.Text = s2
        LabTestItems.Text = s3
        bm.AddItemToTable("LaboratoryTests", LaboratoryTests.Text, New String() {"TestId"}, New String() {LaboratoryTestTypes.SelectedValue.ToString})

        LoadLaboratoryTestTypes()
        LaboratoryTestTypes.Text = s
        LaboratoryTestTypes_LostFocus(Nothing, Nothing)
        LaboratoryTests.Text = s2
        LaboratoryTests_LostFocus(Nothing, Nothing)
        LabTestItems.Text = s3
        bm.AddItemToTable("LabTestItems", LabTestItems.Text, New String() {"TestId", "SubTestId"}, New String() {LaboratoryTestTypes.SelectedValue.ToString, LaboratoryTests.SelectedValue.ToString})
        LaboratoryTestTypes.Text = ""
        LaboratoryTests.Text = ""
        LabTestItems.Text = ""
        'LoadLaboratoryTestTypes()
        'LaboratoryTestTypes.Focus()
    End Sub

    Private Sub LoadLaboratoryTestTypes()
        bm.FillCombo("LaboratoryTestTypes", LaboratoryTestTypes, "", "")
        LaboratoryTestTypes_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub LaboratoryTestTypes_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles LaboratoryTestTypes.LostFocus
        Try
            bm.FillCombo("LaboratoryTests", LaboratoryTests, " Where TestId=" & Val(LaboratoryTestTypes.SelectedValue.ToString), "")
            LaboratoryTests_LostFocus(Nothing, Nothing)
        Catch ex As Exception
        End Try
    End Sub


    Private Sub LaboratoryTests_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles LaboratoryTests.LostFocus
        Try
            bm.FillCombo("LabTestItems", LabTestItems, " Where TestId=" & Val(LaboratoryTestTypes.SelectedValue.ToString) & " and SubTestId=" & Val(LaboratoryTests.SelectedValue.ToString), "")
        Catch ex As Exception
        End Try
    End Sub


    Private Sub LaboratoryTests_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles LaboratoryTests.PreviewKeyDown
        If e.Key = Key.Enter Then LabTestItems.Focus()
    End Sub

    Private Sub LaboratoryTestTypes_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles LaboratoryTestTypes.PreviewKeyDown
        If e.Key = Key.Enter Then LaboratoryTests.Focus()
    End Sub


    Private Sub LabTestItems_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles LabTestItems.PreviewKeyDown
        If e.Key = Key.Enter Then Add5_Click(Nothing, Nothing)
    End Sub



End Class
