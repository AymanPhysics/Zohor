Imports System
Imports System.Data
Imports System.Threading
Public Class Replacement
    Dim bm As New BasicMethods
    Public Ok As Boolean
    Public DelMsg As Boolean = False

    Public lblYes As String = "Yes"
    Public lblNo As String = "No"
    Public StoreId As Integer = 0
    Public Result As DataTable

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        Ok = False

        bm.FillCombo("Hours", HH1, "", , True)
        bm.FillCombo("Minutes", MM1, "", , True)
        bm.FillCombo("HourIndex", Index1, "", , , True)
        bm.FillCombo("Hours", HH2, "", , True)
        bm.FillCombo("Minutes", MM2, "", , True)
        bm.FillCombo("HourIndex", Index2, "", , , True)
        FromDate.SelectedDate = Now
        ToDate.SelectedDate = Now
    End Sub

    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNo.Click
        Ok = False
        Close()
    End Sub

    Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnYes.Click
        Ok = True
        Result = bm.ExecuteAdapter("getReplacement", {"StoreId", "FromDate", "ToDate"}, {StoreId, bm.ToStrDate(FromDate.SelectedDate) & " " & HH1.SelectedValue.ToString & ":" & MM1.SelectedValue.ToString & " " & Index1.Text, bm.ToStrDate(ToDate.SelectedDate) & " " & HH2.SelectedValue.ToString & ":" & MM2.SelectedValue.ToString & " " & Index2.Text})
        Close()
    End Sub

    Private Sub LoadResource()
        btnYes.SetResourceReference(ContentProperty, lblYes)
        btnNo.SetResourceReference(ContentProperty, lblNo)

    End Sub

End Class
