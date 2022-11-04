Imports System.Data

Public Class InFertility2

    Public MyCase As Integer = 0
    Public MyCaseName As String
    WithEvents G0 As New MyGrid
    Dim bm As New BasicMethods

    Private Sub InFertility_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LoadWFH0()
        
        Dim dt As DataTable = bm.ExecuteAdapter("select OOR,M2,Sperm,MaleCount,Moitlity,G3,AbnormalForm,Fertilization,Empbryos,ClassA,Cryo,Pregnancy from CaseIVF where CaseId=" & MyCase)
        G0.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G0.Rows.Add()
            G0.Rows(i).Cells(GC0.OOR).Value = dt.Rows(i)("OOR").ToString
            G0.Rows(i).Cells(GC0.M2).Value = dt.Rows(i)("M2").ToString
            G0.Rows(i).Cells(GC0.Sperm).Value = dt.Rows(i)("Sperm").ToString
            G0.Rows(i).Cells(GC0.MaleCount).Value = dt.Rows(i)("MaleCount").ToString
            G0.Rows(i).Cells(GC0.Moitlity).Value = dt.Rows(i)("Moitlity").ToString
            G0.Rows(i).Cells(GC0.G3).Value = dt.Rows(i)("G3").ToString
            G0.Rows(i).Cells(GC0.AbnormalForm).Value = dt.Rows(i)("AbnormalForm").ToString
            G0.Rows(i).Cells(GC0.Fertilization).Value = dt.Rows(i)("Fertilization").ToString
            G0.Rows(i).Cells(GC0.Empbryos).Value = dt.Rows(i)("Empbryos").ToString
            G0.Rows(i).Cells(GC0.ClassA).Value = dt.Rows(i)("ClassA").ToString
            G0.Rows(i).Cells(GC0.Cryo).Value = dt.Rows(i)("Cryo").ToString
            G0.Rows(i).Cells(GC0.Pregnancy).Value = dt.Rows(i)("Pregnancy").ToString
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
        Shared OOR As String = "OOR"
        Shared M2 As String = "M2"
        Shared Sperm As String = "Sperm"
        Shared MaleCount As String = "MaleCount"
        Shared Moitlity As String = "Moitlity"
        Shared G3 As String = "G3"
        Shared AbnormalForm As String = "AbnormalForm"
        Shared Fertilization As String = "Fertilization"
        Shared Empbryos As String = "Empbryos"
        Shared ClassA As String = "ClassA"
        Shared Cryo As String = "Cryo"
        Shared Pregnancy As String = "Pregnancy"
    End Structure


    Private Sub LoadWFH0()
        'WFH0.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH0.Foreground = New SolidColorBrush(Colors.Red)
        WFH0.Child = G0

        G0.Columns.Clear()
        G0.ForeColor = System.Drawing.Color.DarkBlue

        G0.Columns.Add(GC0.OOR, "OOR")
        G0.Columns.Add(GC0.M2, "M2")
        G0.Columns.Add(GC0.Sperm, "Sperm")
        G0.Columns.Add(GC0.MaleCount, "Count")
        G0.Columns.Add(GC0.Moitlity, "Motility")
        G0.Columns.Add(GC0.G3, "G3")
        G0.Columns.Add(GC0.AbnormalForm, "Abnormal Form")
        G0.Columns.Add(GC0.Fertilization, "Fertilization")
        G0.Columns.Add(GC0.Empbryos, "Empbryos")
        G0.Columns.Add(GC0.ClassA, "ClassA")
        G0.Columns.Add(GC0.Cryo, "Cryo")
        G0.Columns.Add(GC0.Pregnancy, "Pregnancy")

        G0.Columns(GC0.OOR).FillWeight = 100
        G0.Columns(GC0.M2).FillWeight = 100
        G0.Columns(GC0.Sperm).FillWeight = 100
        G0.Columns(GC0.MaleCount).FillWeight = 100
        G0.Columns(GC0.Moitlity).FillWeight = 100
        G0.Columns(GC0.G3).FillWeight = 100
        G0.Columns(GC0.AbnormalForm).FillWeight = 100
        G0.Columns(GC0.Fertilization).FillWeight = 100
        G0.Columns(GC0.Empbryos).FillWeight = 100
        G0.Columns(GC0.ClassA).FillWeight = 100
        G0.Columns(GC0.Cryo).FillWeight = 100
        G0.Columns(GC0.Pregnancy).FillWeight = 100

    End Sub



    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        G0.EndEdit()
        bm.SaveGrid(G0, "CaseIVF", New String() {"CaseId"}, New String() {MyCase}, New String() {"OOR", "M2", "Sperm", "MaleCount", "Moitlity", "G3", "AbnormalForm", "Fertilization", "Empbryos", "ClassA", "Cryo", "Pregnancy"}, New String() {GC0.OOR, GC0.M2, GC0.Sperm, GC0.MaleCount, GC0.Moitlity, GC0.G3, GC0.AbnormalForm, GC0.Fertilization, GC0.Empbryos, GC0.ClassA, GC0.Cryo, GC0.Pregnancy}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String}, New String() {})


        Try
            CType(Me.Parent, Window).Close()
        Catch ex As Exception

        End Try
    End Sub



End Class
