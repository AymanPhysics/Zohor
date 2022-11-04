Imports System.Data

Public Class InFertility1

    Public MyCase As Integer = 0
    Public MyCaseName As String
    WithEvents G As New MyGrid
    Dim bm As New BasicMethods

    Private Sub InFertility_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LoadWFH()

        Dim dt As DataTable = bm.ExecuteAdapter("select Protocol,InfertirityRemarks from Cases where Id=" & MyCase)
        If dt.Rows.Count > 0 Then
            Protocol.Text = dt.Rows(0)("Protocol").ToString
            InfertirityRemarks.Text = dt.Rows(0)("InfertirityRemarks").ToString
        End If

        dt = bm.ExecuteAdapter("select * from CaseInFertility where CaseId=" & MyCase)
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Daydate).Value = dt.Rows(i)("Daydate").ToString
            G.Rows(i).Cells(GC.E2).Value = dt.Rows(i)("E2").ToString
            G.Rows(i).Cells(GC.HMG).Value = dt.Rows(i)("HMG").ToString
            G.Rows(i).Cells(GC.FSH).Value = dt.Rows(i)("FSH").ToString
            G.Rows(i).Cells(GC.against).Value = dt.Rows(i)("against").ToString
            G.Rows(i).Cells(GC.antagainst).Value = dt.Rows(i)("antagainst").ToString
            G.Rows(i).Cells(GC.RT).Value = dt.Rows(i)("RT").ToString
            G.Rows(i).Cells(GC.LT).Value = dt.Rows(i)("LT").ToString
            G.Rows(i).Cells(GC.Endd).Value = dt.Rows(i)("Endd").ToString
            G.Rows(i).Cells(GC.Remarks).Value = dt.Rows(i)("Remarks").ToString
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
        Shared E2 As String = "E2"
        Shared HMG As String = "HMG"
        Shared FSH As String = "FSH"
        Shared against As String = "against"
        Shared antagainst As String = "antagainst"
        Shared RT As String = "RT"
        Shared LT As String = "LT"
        Shared Endd As String = "End"
        Shared Remarks As String = "Remarks"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Daydate, "Date")
        G.Columns.Add(GC.E2, "E2")
        G.Columns.Add(GC.HMG, "HMG")
        G.Columns.Add(GC.FSH, "FSH")
        G.Columns.Add(GC.against, "agonist")
        G.Columns.Add(GC.antagainst, "antagonist")
        G.Columns.Add(GC.RT, "RT")
        G.Columns.Add(GC.LT, "LT")
        G.Columns.Add(GC.Endd, "End")
        G.Columns.Add(GC.Remarks, "Remarks")

        G.Columns(GC.Daydate).FillWeight = 100
        G.Columns(GC.E2).FillWeight = 100
        G.Columns(GC.HMG).FillWeight = 100
        G.Columns(GC.FSH).FillWeight = 100
        G.Columns(GC.against).FillWeight = 100
        G.Columns(GC.antagainst).FillWeight = 100
        G.Columns(GC.RT).FillWeight = 100
        G.Columns(GC.LT).FillWeight = 100
        G.Columns(GC.Endd).FillWeight = 100
        G.Columns(GC.Remarks).FillWeight = 400

    End Sub


    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        bm.ExecuteNonQuery("update cases set Protocol='" & Protocol.Text.Replace("'", "''") & "',InfertirityRemarks='" & InfertirityRemarks.Text.Replace("'", "''") & "' where Id='" & MyCase & "'")

        G.EndEdit()
        bm.SaveGrid(G, "CaseInFertility", New String() {"CaseId"}, New String() {MyCase}, New String() {"Daydate", "E2", "HMG", "FSH", "against", "antagainst", "RT", "LT", "Endd", "Remarks"}, New String() {GC.Daydate, GC.E2, GC.HMG, GC.FSH, GC.against, GC.antagainst, GC.RT, GC.LT, GC.Endd, GC.Remarks}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String, VariantType.String}, New String() {})

        Try
            CType(Me.Parent, Window).Close()
        Catch ex As Exception

        End Try
    End Sub

    

End Class
