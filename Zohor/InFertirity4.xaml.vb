Imports System.Data

Public Class InFertility4

    Public MyCase As Integer = 0
    Public MyCaseName As String
    WithEvents G As New MyGrid
    Dim bm As New BasicMethods

    Private Sub InFertility_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LoadWFH()

        Dim dt As DataTable = bm.ExecuteAdapter("select Medical,Surgical,Smoker,Others from Cases where Id=" & MyCase)
        If dt.Rows.Count > 0 Then
            Medical.Text = dt.Rows(0)("Medical").ToString
            Surgical.Text = dt.Rows(0)("Surgical").ToString
            Smoker.Text = dt.Rows(0)("Smoker").ToString
            Others.Text = dt.Rows(0)("Others").ToString
        End If

        dt = bm.ExecuteAdapter("select * from CaseInFertility4 where CaseId=" & MyCase)
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Daydate).Value = dt.Rows(i)("Daydate").ToString
            G.Rows(i).Cells(GC.Lab).Value = dt.Rows(i)("Lab").ToString
            G.Rows(i).Cells(GC.C).Value = dt.Rows(i)("C").ToString
            G.Rows(i).Cells(GC.M).Value = dt.Rows(i)("M").ToString
            G.Rows(i).Cells(GC.G3).Value = dt.Rows(i)("G3").ToString
            G.Rows(i).Cells(GC.Abn).Value = dt.Rows(i)("Abn").ToString
            G.Rows(i).Cells(GC.Pus).Value = dt.Rows(i)("Pus").ToString
            G.Rows(i).Cells(GC.Type).Value = dt.Rows(i)("Type").ToString
        Next

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
        Shared Daydate As String = "Daydate"
        Shared Lab As String = "Lab"
        Shared C As String = "C"
        Shared M As String = "M"
        Shared G3 As String = "G3"
        Shared Abn As String = "Abn"
        Shared Pus As String = "Pus"
        Shared Type As String = "Type"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Daydate, "Date")
        G.Columns.Add(GC.Lab, "Lab")
        G.Columns.Add(GC.C, "Count")
        G.Columns.Add(GC.M, "Motility")
        G.Columns.Add(GC.G3, "G3")
        G.Columns.Add(GC.Abn, "Abn")
        G.Columns.Add(GC.Pus, "Pus")
        G.Columns.Add(GC.Type, "Type")

    End Sub


    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        bm.ExecuteNonQuery("update cases set Medical='" & Medical.Text & "',Surgical='" & Surgical.Text & "',Smoker='" & Smoker.Text & "',Others='" & Others.Text & "' where Id='" & MyCase & "'")
        G.EndEdit()
        bm.SaveGrid(G, "CaseInFertility4", New String() {"CaseId"}, New String() {MyCase}, New String() {"Daydate", "Lab", "C", "M", "G3", "Abn", "Pus", "Type"}, New String() {GC.Daydate, GC.Lab, GC.C, GC.M, GC.G3, GC.Abn, GC.Pus, GC.Type}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String}, New String() {})

        Try
            CType(Me.Parent, Window).Close()
        Catch ex As Exception

        End Try
    End Sub



End Class
