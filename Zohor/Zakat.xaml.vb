Imports System.Data
Imports System.Threading.Tasks
Imports System.Threading
Imports System.Globalization
Imports Newtonsoft.Json

Public Class Zakat
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return

        Dim MyNow As DateTime = bm.MyGetDate()
        ToDate.SelectedDate = MyNow.Date

    End Sub

    Private Sub btnBal1_Click(sender As Object, e As RoutedEventArgs) Handles btnBal1.Click
        Dim webclient_server7 As New System.Net.WebClient
        Dim json_result As String = webclient_server7.DownloadString("https://goldpricez.today/api/?countires=eg&karat=21&measurement=g")
        Try
            GoldPrice.Text = json_result.Replace("{""response"":true,""lastupdate"":null,""measurement"":""g"",""karat"":21,""result"":{""eg"":", "").Replace("}}", "")
        Catch ex As Exception
            bm.ShowMSG("حدث خطأ: " & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub HypLink_RequestNavigate(sender As Object, e As RequestNavigateEventArgs) Handles HypLink.RequestNavigate
        System.Diagnostics.Process.Start(e.Uri.AbsoluteUri)
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If Val(GoldPrice.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد سعر الذهب")
            GoldPrice.Focus()
            Return
        End If
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@ToDate", "Header", "GoldPrice", "@IsDedCustomers"}
        rpt.paravalue = New String() {ToDate.SelectedDate, CType(Parent, Page).Title, Val(GoldPrice.Text), IIf(IsDedCustomers.IsChecked, 1, 0)}
        rpt.Rpt = "AccountEndZakat.rpt"
        rpt.Show()
    End Sub
End Class