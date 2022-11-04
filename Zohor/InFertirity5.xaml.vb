Imports System.Data

Public Class InFertility5

    Public MyCase As Integer = 0
    Public MyCaseName As String
    Dim bm As New BasicMethods

    Private Sub InFertility_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        
        Dim dt As DataTable = bm.ExecuteAdapter("select MenstrualH,ObstetricH,Occupation,FamilyH from Cases where Id=" & MyCase)
        If dt.Rows.Count > 0 Then
            MenstrualH.Text = dt.Rows(0)("MenstrualH").ToString
            ObstetricH.Text = dt.Rows(0)("ObstetricH").ToString
            Occupation.Text = dt.Rows(0)("Occupation").ToString
            FamilyH.Text = dt.Rows(0)("FamilyH").ToString
        End If

    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        bm.ExecuteNonQuery("update cases set MenstrualH='" & MenstrualH.Text.Replace("'", "''") & "',ObstetricH='" & ObstetricH.Text.Replace("'", "''") & "',Occupation='" & Occupation.Text.Replace("'", "''") & "',FamilyH='" & FamilyH.Text.Replace("'", "''") & "' where Id='" & MyCase & "'")

        Try
            CType(Me.Parent, Window).Close()
        Catch ex As Exception

        End Try
    End Sub



End Class
