Imports System.Data

Public Class OvarianFactor

    Public EmpId As Integer
    Public DatePicker1 As DateTime
    Public ReservId As Integer
    Public CaseId As Integer
    Public CaseName As String

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Public WithImage As Boolean = False


    Public Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
    
        dt = bm.ExecuteAdapter("select * from ReservationCbo4 where EmpId='" & EmpId & "' /*and DayDate='" & bm.ToStrDate(DatePicker1) & "' and ReservId='" & ReservId & "'*/ and CaseId='" & CaseId & "'")
        If dt.Rows.Count > 0 Then
            Right.Text = dt.Rows(0)("Notes1").ToString
            Left.Text = dt.Rows(0)("Notes2").ToString
        End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        bm.ExecuteNonQuery("delete ReservationCbo4 where EmpId='" & EmpId & "' and CaseId='" & CaseId & "' insert ReservationCbo4(EmpId,CaseId,Notes1,Notes2) select '" & EmpId & "','" & CaseId & "','" & Right.Text.Replace("'", "''") & "','" & Left.Text.Replace("'", "''") & "'")


        If sender Is btnSave Then CType(Parent, Window).Close()

    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")

    End Sub


End Class
