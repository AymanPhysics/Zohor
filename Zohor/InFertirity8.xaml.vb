Imports System.Data

Public Class InFertirity8

    Public MyCase As Integer = 0
    Public MyCaseName As String
    WithEvents G0 As New MyGrid
    Dim bm As New BasicMethods

    Private Sub InFertility_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LoadWFH0()

        Dim dt As DataTable = bm.ExecuteAdapter("select * from CaseRisk where CaseId=" & MyCase)
        G0.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G0.Rows.Add()
            G0.Rows(i).Cells(GC0.Notes).Value = dt.Rows(i)("Notes").ToString
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
        Shared Notes As String = "Notes"
    End Structure

    Private Sub LoadWFH0()
        'WFH0.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH0.Foreground = New SolidColorBrush(Colors.Red)
        WFH0.Child = G0

        
        G0.Columns.Clear()
        G0.ForeColor = System.Drawing.Color.DarkBlue
        G0.Columns.Add(GC0.Notes, "Risk")

    End Sub


    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        G0.EndEdit()
        If Not bm.SaveGrid(G0, "CaseRisk", New String() {"CaseId"}, New String() {MyCase}, New String() {"Notes"}, New String() {GC0.Notes}, New VariantType() {VariantType.String}, New String() {}) Then Return

        Try
            CType(Me.Parent, Window).Close()
        Catch ex As Exception

        End Try
    End Sub

End Class
