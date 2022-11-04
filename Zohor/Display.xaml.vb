Imports System
Imports System.Threading
Imports System.Windows.Threading

Public Class Display
    Dim bm As New BasicMethods
    Public Ok As Boolean
    Public lblYes As String = "Yes"
    Public lblNo As String = "No"

    Public DisplayCaseId As Integer = 0
    Public DisplayCaseName As String = ""
    Dim t As New DispatcherTimer With {.IsEnabled = False, .Interval = New TimeSpan(0, 0, 0, 0, 100)}
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        Ok = False
    
    End Sub

    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNo.Click
        Ok = False
        Close()
    End Sub

    Private Sub LoadResource()
        btnNo.SetResourceReference(ContentProperty, lblNo)
    End Sub

    Function Save(DisplayRoomId As Integer) As Boolean
        Return bm.ExecuteNonQuery("update statics set DisplayRoomId='" & DisplayRoomId & "',DisplayCaseId='" & DisplayCaseId & "',DisplayCaseName='" & DisplayCaseName & "'")
    End Function

    Private Sub btn1_Click(sender As Object, e As RoutedEventArgs) Handles btn1.Click
        Ok = True
        Save(1)
        Close()
    End Sub

    Private Sub btn2_Click(sender As Object, e As RoutedEventArgs) Handles btn2.Click
        Ok = True
        Save(2)
        Close()
    End Sub

    Private Sub btn3_Click(sender As Object, e As RoutedEventArgs) Handles btn3.Click
        Ok = True
        Save(3)
        Close()
    End Sub

    Private Sub btn4_Click(sender As Object, e As RoutedEventArgs) Handles btn4.Click
        Ok = True
        Save(4)
        Close()
    End Sub

End Class
