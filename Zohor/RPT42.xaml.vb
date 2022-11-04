Imports System.Data

Public Class RPT42
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Value", "@DownPayment", "Header"}
        rpt.paravalue = New String() {Val(Value.Text), Val(DownPayment.Text), CType(Parent, Page).Title}

        Select Case Flag
            Case 1
                rpt.Rpt = "InstallmentTest.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

    End Sub


    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")

    End Sub

End Class