Imports System.Data
Imports Microsoft.Office.Interop
Imports System.IO

Public Class NurseShiftSummary
    Dim bm As New BasicMethods
    Dim dt As New DataTable

    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Remarks1b.Text = Remarks1b.Text.Replace("'", "''")
        bm.ExecuteNonQuery("delete NurseShiftSummary where EmpId='" & EmpId.Text & "' and DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "'   insert NurseShiftSummary(EmpId,DayDate,Name) select '" & EmpId.Text & "','" & bm.ToStrDate(DayDate.SelectedDate) & "','" & Remarks1b.Text & "' ")
        bm.ShowMSG("Done Successfuly")
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({EmpId})
        DayDate.SelectedDate = bm.MyGetDate()
        EmpId.Text = Md.UserName
        EmpId_LostFocus(Nothing, Nothing)

        DayDate.IsEnabled = False
        EmpId.IsEnabled = False

        Remarks1b.Text = bm.ExecuteScalar("select Name from NurseShiftSummary where EmpId='" & EmpId.Text & "' and DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "'")
    End Sub
    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "Save")
        lblEmpId.SetResourceReference(ContentProperty, "Employee")
        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub EmpId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId.KeyUp
        If bm.ShowHelp("Employees", EmpId, EmpName, e, "Select cast(Id as varchar(10))Id," & Resources.Item("CboName") & " Name from Employees") Then
            EmpId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub EmpId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId.LostFocus
        If Val(EmpId.Text.Trim) = 0 Then
            EmpId.Clear()
            EmpName.Clear()
            Return
        End If
        bm.LostFocus(EmpId, EmpName, "select Name from Employees where Id=" & EmpId.Text.Trim())
    End Sub



End Class