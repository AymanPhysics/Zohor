Imports System
Imports System.Threading
Imports System.Windows.Threading
Imports System.Data

Public Class Display2
    Dim bm As New BasicMethods
    Public WithEvents WMP As New WMPLib.WindowsMediaPlayer

    Dim t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 0, 3)}
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.GetImage("Statics", New String() {}, New String() {}, "Logo", Image1)
        AddHandler t.Tick, AddressOf Tick

        'Dim dt As DataTable = bm.ExecuteAdapter("select Id,Name from tt")
        'For Each r In dt.Rows
        '    bm.Download(r(0))
        '    bm.Download(r(1))
        'Next
    End Sub

    Dim CurrentDisplayRoomId As Integer = 0
    Dim CurrentDisplayCaseId As Integer = 0

    Public Sub Tick()
        Dim dt As DataTable = bm.ExecuteAdapter("select DisplayRoomId,DisplayCaseId,DisplayCaseName from statics")
        If dt.Rows.Count > 0 Then
            DisplayRoomId.Content = dt.Rows(0)("DisplayRoomId")
            DisplayCaseId.Content = dt.Rows(0)("DisplayCaseId")
            DisplayCaseName.Content = dt.Rows(0)("DisplayCaseName")

            If Not (CurrentDisplayRoomId = DisplayRoomId.Content AndAlso CurrentDisplayCaseId = DisplayCaseId.Content) Then
                CurrentDisplayRoomId = DisplayRoomId.Content
                CurrentDisplayCaseId = DisplayCaseId.Content

                'bm.TextToSpeech("مَرِيضْ رَقَمْ " & DisplayCaseId.Content & " - " & DisplayCaseName.Content & " - غُرْفَهْ رَقَمْ - " & DisplayRoomId.Content)
                'bm.TextToSpeech({"مَرِيضْ رَقَمْ " & DisplayCaseId.Content & " - " & DisplayCaseName.Content & " - غُرْفَهْ رَقَمْ - " & DisplayRoomId.Content})
                bm.TextToSpeech({"مَرِيضْ رَقَمْ ", DisplayCaseId.Content, " غُرْفَهْ رَقَمْ ", DisplayRoomId.Content}) ' DisplayCaseName.Content, 
            End If

        End If
    End Sub

End Class
