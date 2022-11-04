Imports System.Data

Public Class ProgressNote

    Public MyCase As Integer = 0
    Public MyCaseName As String
    WithEvents G As New MyGrid
    Dim bm As New BasicMethods

    Private Sub InFertility_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LoadWFH()
        DayDate.SelectedDate = bm.MyGetDate
        If Val(bm.ExecuteScalar("select Doctor from Employees where Id=" & Md.UserName)) = 0 Then
            AimOfTheDay.IsEnabled = False
            DayDate.IsEnabled = False
            Add3.IsEnabled = False
            btnDelete.IsEnabled = False
            btnReNew.IsEnabled = False
        End If

        'GetData()

        Try
            Visibility = Visibility.Hidden
            Visibility = Visibility.Visible

            Dim c = G.Parent
            G.Parent = Nothing
            G.Parent = c
        Catch ex As Exception
        End Try
    End Sub



    Structure GC
        Shared DoctorId As String = "DoctorId"
        Shared Time As String = "Time"
        Shared Problem As String = "Problem"
        Shared CasePlan As String = "CasePlan"
        Shared Note As String = "Note"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G
        G.AllowUserToAddRows = False

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.DoctorId, "Doctor")
        G.Columns.Add(GC.Time, "Time")
        G.Columns.Add(GC.Problem, "Problem")
        G.Columns.Add(GC.CasePlan, "Plan")
        G.Columns.Add(GC.Note, "Note")


        If Val(bm.ExecuteScalar("select Doctor from Employees where Id=" & Md.UserName)) = 0 Then
            G.Columns(GC.Problem).ReadOnly = True
            G.Columns(GC.CasePlan).ReadOnly = True
            G.Columns(GC.Note).ReadOnly = True
        End If
        G.Columns(GC.DoctorId).ReadOnly = True
        G.Columns(GC.Time).ReadOnly = True
        
        G.Columns(GC.Time).FillWeight = 50
    End Sub


    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click

        G.EndEdit()
        bm.SaveGrid(G, "ProgressNote", New String() {"CaseId", "DayDate"}, New String() {MyCase, bm.ToStrDate(DayDate.SelectedDate)}, New String() {"Problem", "CasePlan", "Note", "DoctorId", "Time"}, New String() {GC.Problem, GC.CasePlan, GC.Note, GC.DoctorId, GC.Time}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String}, New String() {})

        bm.ExecuteNonQuery("update ProgressNote set AimOfTheDay='" & AimOfTheDay.Text.Replace("'", "''") & "' where CaseId=" & MyCase & " and DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "'")

        Try
            CType(Me.Parent, Window).Close()
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Add3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add3.Click

        G.Rows.Add(Md.EnName, Now.TimeOfDay.ToString.Substring(0, 5), Problem.Text, CasePlan.Text, Note.Text)
        Problem.Text = ""
        CasePlan.Text = ""
        Note.Text = ""

    End Sub

    Private Sub GetData() Handles DayDate.SelectedDateChanged
        Dim dt As DataTable = bm.ExecuteAdapter("select * from ProgressNote where CaseId=" & MyCase & " and DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "'")
        AimOfTheDay.Clear()
        If dt.Rows.Count > 0 Then
            AimOfTheDay.Text = dt.Rows(0)("AimOfTheDay").ToString
            End If

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Problem).Value = dt.Rows(i)("Problem").ToString
            G.Rows(i).Cells(GC.CasePlan).Value = dt.Rows(i)("CasePlan").ToString
            G.Rows(i).Cells(GC.Note).Value = dt.Rows(i)("Note").ToString
            G.Rows(i).Cells(GC.DoctorId).Value = dt.Rows(i)("DoctorId").ToString
            G.Rows(i).Cells(GC.Time).Value = dt.Rows(i)("Time").ToString
        Next

    End Sub

    Private Sub ReNew_Click(sender As Object, e As RoutedEventArgs) Handles btnReNew.Click
        Dim dt As DataTable = bm.ExecuteAdapter("select * from ProgressNote where CaseId=" & MyCase & " and DayDate=(select max(DayDate) from ProgressNote where CaseId=" & MyCase & " and DayDate<'" & bm.ToStrDate(DayDate.SelectedDate) & "')")
        AimOfTheDay.Clear()
        If dt.Rows.Count > 0 Then
            AimOfTheDay.Text = dt.Rows(0)("AimOfTheDay").ToString
        End If

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Problem).Value = dt.Rows(i)("Problem").ToString
            G.Rows(i).Cells(GC.CasePlan).Value = dt.Rows(i)("CasePlan").ToString
            G.Rows(i).Cells(GC.Note).Value = dt.Rows(i)("Note").ToString
            G.Rows(i).Cells(GC.DoctorId).Value = dt.Rows(i)("DoctorId").ToString
            G.Rows(i).Cells(GC.Time).Value = Now.TimeOfDay.ToString.Substring(0, 5)
        Next

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click
        Try
            G.Rows.RemoveAt(G.CurrentRow.Index)
        Catch ex As Exception
        End Try
    End Sub
End Class
