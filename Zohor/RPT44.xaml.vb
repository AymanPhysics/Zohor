Imports System.Data

Public Class RPT44
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"Header", "P0", "P1", "P2", "P3", "P4", "P5", "P6", "ItemName", "Total", "DayDate", "DownPayment", "InstallVal", "InstallCount"}
        rpt.paravalue = New String() {"طلب حصول على نظام تقسيط من شركة " & Md.CompanyName, bm.ExecuteScalar("select Name from Stores where Id=" & Md.DefaultStore), P1.Text, P2.Text, P3.Text, P4.Text, P5.Text, P6.Text, ItemName.Text, Total.Text, bm.ToStrDate(DayDate.SelectedDate), DownPayment.Text, InstallValue.Text, InstallCount.Text}
        rpt.Rpt = "SalesTemp.rpt"
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        DayDate.SelectedDate = Now.Date
    End Sub


    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")

    End Sub

End Class