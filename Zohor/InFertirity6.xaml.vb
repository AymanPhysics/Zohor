Imports System.Data

Public Class InFertility6

    Public MyCase As Integer = 0
    Public MyCaseName As String
    WithEvents G0 As New MyGrid
    Dim bm As New BasicMethods

    Private Sub InFertility_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LoadWFH0()

        Dim dt As DataTable = bm.ExecuteAdapter("select DayDate,Center,Doctor,Protocol,Dose,OOR,Embryo,Cryo,Pregnancy,Remarks from CaseICSI2 where CaseId=" & MyCase)
        G0.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G0.Rows.Add()
            G0.Rows(i).Cells(GC0.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G0.Rows(i).Cells(GC0.Center).Value = dt.Rows(i)("Center").ToString
            G0.Rows(i).Cells(GC0.Doctor).Value = dt.Rows(i)("Doctor").ToString
            G0.Rows(i).Cells(GC0.Protocol).Value = dt.Rows(i)("Protocol").ToString
            G0.Rows(i).Cells(GC0.Dose).Value = dt.Rows(i)("Dose").ToString
            G0.Rows(i).Cells(GC0.OOR).Value = dt.Rows(i)("OOR").ToString
            G0.Rows(i).Cells(GC0.Embryo).Value = dt.Rows(i)("Embryo").ToString
            G0.Rows(i).Cells(GC0.Cryo).Value = dt.Rows(i)("Cryo").ToString
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
        Shared Center As String = "Center"
        Shared Doctor As String = "Doctor"
        Shared Protocol As String = "Protocol"
        Shared Dose As String = "Dose"
        Shared OOR As String = "OOR"
        Shared Embryo As String = "Embryo"
        Shared Cryo As String = "Cryo"
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
        G0.Columns.Add(GC0.Center, "Center")
        G0.Columns.Add(GC0.Doctor, "Doctor")
        G0.Columns.Add(GC0.Protocol, "Protocol")
        G0.Columns.Add(GC0.Dose, "Dose")
        G0.Columns.Add(GC0.OOR, "OR")
        G0.Columns.Add(GC0.Embryo, "Embryo")
        G0.Columns.Add(GC0.Cryo, "Cryo")
        G0.Columns.Add(GC0.Pregnancy, "Pregnancy")
        G0.Columns.Add(GC0.Remarks, "Remarks")

        G0.Columns(GC0.DayDate).FillWeight = 100
        G0.Columns(GC0.Center).FillWeight = 200
        G0.Columns(GC0.Doctor).FillWeight = 200
        G0.Columns(GC0.OOR).FillWeight = 100
        G0.Columns(GC0.Embryo).FillWeight = 100
        G0.Columns(GC0.Cryo).FillWeight = 100
        G0.Columns(GC0.Pregnancy).FillWeight = 100
        G0.Columns(GC0.Remarks).FillWeight = 300

    End Sub



    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        G0.EndEdit()
        bm.SaveGrid(G0, "CaseICSI2", New String() {"CaseId"}, New String() {MyCase}, New String() {"DayDate", "Center", "Doctor", "Protocol", "Dose", "OOR", "Embryo", "Cryo", "Pregnancy", "Remarks"}, New String() {GC0.DayDate, GC0.Center, GC0.Doctor, GC0.Protocol, GC0.Dose, GC0.OOR, GC0.Embryo, GC0.Cryo, GC0.Pregnancy, GC0.Remarks}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String}, New String() {})


        Try
            CType(Me.Parent, Window).Close()
        Catch ex As Exception

        End Try
    End Sub



End Class
