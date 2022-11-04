Imports System.Data

Public Class InFertility7

    Public MyCase As Integer = 0
    Public MyCaseName As String
    WithEvents G0 As New MyGrid
    Dim bm As New BasicMethods

    Private Sub InFertility_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LoadWFH0()

        Dim dt As DataTable = bm.ExecuteAdapter("select DayDate,Doctor,Count,G3,Abnormality,Pregnancy,Remarks from CaseIUI where CaseId=" & MyCase)
        G0.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G0.Rows.Add()
            G0.Rows(i).Cells(GC0.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G0.Rows(i).Cells(GC0.Doctor).Value = dt.Rows(i)("Doctor").ToString
            G0.Rows(i).Cells(GC0.Count).Value = dt.Rows(i)("Count").ToString
            G0.Rows(i).Cells(GC0.G3).Value = dt.Rows(i)("G3").ToString
            G0.Rows(i).Cells(GC0.Abnormality).Value = dt.Rows(i)("Abnormality").ToString
            G0.Rows(i).Cells(GC0.Pregnancy).Value = dt.Rows(i)("Pregnancy").ToString
            G0.Rows(i).Cells(GC0.Remarks).Value = dt.Rows(i)("Remarks").ToString
        Next

        Try
            Visibility = Visibility.Hidden
            Visibility = Visibility.Visible

            Dim c = G0.Parent
            G0.Parent = Nothing
            G0.Parent = c
        Catch ex As Exception
        End Try
    End Sub

    Structure GC0
        Shared DayDate As String = "DayDate"
        Shared Doctor As String = "Doctor"
        Shared Count As String = "Count"
        Shared G3 As String = "G3"
        Shared Abnormality As String = "Abnormality"
        Shared Pregnancy As String = "Pregnancy"
        Shared Remarks As String = "Remarks"
    End Structure

    Private Sub LoadWFH0()
        'WFH0.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH0.Foreground = New SolidColorBrush(Colors.Red)
        WFH0.Child = G0

        G0.Columns.Clear()
        G0.ForeColor = System.Drawing.Color.DarkBlue

        G0.Columns.Add(GC0.DayDate, "Date")
        G0.Columns.Add(GC0.Doctor, "Doctor")
        G0.Columns.Add(GC0.Count, "Count")
        G0.Columns.Add(GC0.G3, "G3")
        G0.Columns.Add(GC0.Abnormality, "Abnormality")
        G0.Columns.Add(GC0.Pregnancy, "Pregnancy")
        G0.Columns.Add(GC0.Remarks, "Remarks")

        G0.Columns(GC0.DayDate).FillWeight = 100
        G0.Columns(GC0.Doctor).FillWeight = 200
        G0.Columns(GC0.Pregnancy).FillWeight = 100
        G0.Columns(GC0.Remarks).FillWeight = 300

    End Sub


    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        G0.EndEdit()
        bm.SaveGrid(G0, "CaseIUI", New String() {"CaseId"}, New String() {MyCase}, New String() {"DayDate", "Doctor", "Count", "G3", "Abnormality", "Pregnancy", "Remarks"}, New String() {GC0.DayDate, GC0.Doctor, GC0.Count, GC0.G3, GC0.Abnormality, GC0.Pregnancy, GC0.Remarks}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String}, New String() {})

        Try
            CType(Me.Parent, Window).Close()
        Catch ex As Exception

        End Try
    End Sub

End Class
