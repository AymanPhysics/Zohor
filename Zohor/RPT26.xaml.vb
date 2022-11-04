Imports System.Data

Public Class RPT26

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromId", "@ToId", "Header"}
        rpt.paravalue = New String() {AccNo.Text, AccNo2.Text, CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "Chart.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({AccNo, AccNo2})
    End Sub

    Dim lop As Boolean = False
    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")

    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyDown, AccNo2.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, -1, , )
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, -1, , )
    End Sub

    Private Sub AccNo2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo2.LostFocus
        bm.AccNoLostFocus(AccNo2, AccName2, -1, , )
    End Sub

    Private Sub AccNo2_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles AccNo2.KeyUp
        bm.AccNoShowHelp(AccNo2, AccName2, e, -1, , )
    End Sub

End Class