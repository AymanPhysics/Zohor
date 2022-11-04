Imports System.Data
Imports System.ComponentModel
Imports System.IO

Public Class ReservationDetailsClinicsShows
    Dim bm As New BasicMethods
    Dim bf As New BasicForm

    WithEvents G0 As New MyGrid
    WithEvents G1 As New MyGrid
    WithEvents G2 As New MyGrid
    WithEvents G3 As New MyGrid
    WithEvents G4 As New MyGrid
    WithEvents G5 As New MyGrid

    Dim WithEvents BackgroundWorker1 As New BackgroundWorker
    WithEvents t As New Forms.Timer With {.Interval = 2000}
    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded 
        If bm.TestIsLoaded(Me) Then Return
        Dim CurrentClinicId As Integer = GetSetting("OMEGA", "Show", "First", 0)
        Dim CurrentClinicId2 As Integer = GetSetting("OMEGA", "Show", "Second", 0)
        bm.FillCombo("select Id,Name from Clinics union select 0 Id,'-' Name order by Name", ClinicId)
        bm.FillCombo("select Id,Name from Clinics union select 0 Id,'-' Name order by Name", ClinicId2)
        ClinicId.SelectedValue = CurrentClinicId
        ClinicId2.SelectedValue = CurrentClinicId2
        ClinicId_SelectionChanged(Nothing, Nothing)
        ClinicId2_SelectionChanged(Nothing, Nothing)
        t.Start()
    End Sub
    Dim CurrentId As Integer = 0
    Dim CurrentId2 As Integer = 0
    Private Sub t_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles t.Tick
        GetButton(WR, ClinicId, CurrentId)
        GetButton(WR2, ClinicId2, CurrentId2)
    End Sub
       
    Private Sub ClinicId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ClinicId.SelectionChanged
        Try
            SaveSetting("OMEGA", "Show", "First", ClinicId.SelectedValue.ToString)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ClinicId2_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ClinicId2.SelectionChanged
        Try
            SaveSetting("OMEGA", "Show", "Second", ClinicId2.SelectedValue.ToString)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GetButton(G As Grid, CId As ComboBox, ByRef CurrentId As Integer)
        Try
            G.Children.Clear()
            If CId.SelectedIndex < 1 Then Return

            Dim dt As DataTable = bm.ExecuteAdapter("LoadReservationsClinicsShow", New String() {"ClinicId"}, New String() {Val(CId.SelectedValue.ToString)})

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & dt.Rows(i)("ReservId").ToString
                x.Tag = dt.Rows(i)("ReservId").ToString

                If CurrentId <> dt.Rows(i)("ReservId").ToString Then
                    'bm.TextToSpeech(CId.Text & " - " & dt.Rows(i)("CaseName").ToString)
                End If
                CurrentId = dt.Rows(i)("ReservId").ToString

                x.VerticalContentAlignment = VerticalAlignment.Center

                x.Margin = New Thickness(10, 10, 10, 10)
                x.HorizontalAlignment = HorizontalAlignment.Stretch
                x.VerticalAlignment = VerticalAlignment.Stretch
                x.FontSize = 74
                x.Cursor = Input.Cursors.Pen
                x.Content = dt.Rows(i)("ReservId").ToString.Replace(vbCrLf, " ") & "            " & dt.Rows(i)("CaseName").ToString
                x.ToolTip = x.Content
                x.ToolTip = x.Content
                x.Background = bf.btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                x.Background = bf.btnSave.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                G.Children.Add(x)
            Next
        Catch ex As Exception
        End Try
    End Sub

End Class
