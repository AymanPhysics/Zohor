Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine

Public Class RPTFromIdToId
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public MyFlag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer

        If Flag = 1 Then
            dt = bm.ExecuteAdapter("Select Id,Name from Customers where Code between " & Val(FromId.Text) & " and " & Val(ToId.Text))

            For i As Integer = 0 To dt.Rows.Count - 1
                rpt = New ReportViewer
                rpt.Rpt = "Certificate.rpt"
                rpt.paraname = New String() {"CustomerId", "CustomerName", "AttendanceHours"}
                rpt.paravalue = New String() {Val(dt.Rows(i)(0)), dt.Rows(i)(1), bm.ExecuteScalar("select dbo.getAttendanceHours(" & Val(ConferenceId.Text) & "," & Val(dt.Rows(i)(0)) & ")")}
                rpt.ReportViewer_Load(Nothing, Nothing)
                For Each c As ReportObject In rpt.ReportDoc.ReportDefinition.ReportObjects
                    Try
                        If c.Name = "AttendanceHours" Then
                            c.Top = CertificateTop2
                            c.Left = CertificateLeft2
                        Else
                            c.Top = CertificateTop
                            c.Left = CertificateLeft
                        End If
                    Catch
                    End Try
                Next
                'rpt.ShowDialog()
                rpt.Print()
                bm.ExecuteNonQuery("insert PrintCertificatesHistory(ConferenceId,CustomerId) select '" & Val(ConferenceId.Text) & "','" & Val(dt.Rows(i)(0)) & "'")
            Next
            Return
        End If


        rpt.paraname = New String() {"@SponsorId", "@ConferenceId", "Header"}
        rpt.paravalue = New String() {Val(SponsorId.Text), Val(ConferenceId.Text), CType(Parent, Page).Title}
        If Flag = 2 Then
            rpt.Rpt = "PrintIDsHistory.rpt"
        ElseIf Flag = 3 Then
            rpt.Rpt = "PrintCertificatesHistory.rpt"
        ElseIf Flag = 4 Then
            rpt.Rpt = "PL_CheckInOut.rpt"
        ElseIf Flag = 5 Then
            rpt.Rpt = "PL_CheckInOut2.rpt"
        ElseIf Flag = 6 Then
            rpt.Rpt = "PL_CheckInOut1.rpt"
        ElseIf Flag = 7 Then
            rpt.Rpt = "ConferencesMaster.rpt"
        Else

        End If

        rpt.Show()


    End Sub


    Dim CertificateTop As Integer = 0
    Dim CertificateLeft As Integer = 0
    Dim CertificateTop2 As Integer = 0
    Dim CertificateLeft2 As Integer = 0

    Private Sub ConferenceId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ConferenceId.LostFocus

        If Val(ConferenceId.Text.Trim) = 0 Then
            ConferenceId.Clear()
            ConferenceName.Clear()
            Return
        End If

        bm.LostFocus(ConferenceId, ConferenceName, "select Name from Conferences where /*IsActive=1 and*/ Id=" & ConferenceId.Text.Trim())

        dt = bm.ExecuteAdapter("select CertificateTop,CertificateLeft,CertificateTop2,CertificateLeft2 from conferences where Id=" & ConferenceId.Text)
        If dt.Rows.Count = 1 Then
            CertificateTop = Val(dt.Rows(0)("CertificateTop")) * 100
            CertificateLeft = Val(dt.Rows(0)("CertificateLeft")) * 100
            CertificateTop2 = Val(dt.Rows(0)("CertificateTop2")) * 100
            CertificateLeft2 = Val(dt.Rows(0)("CertificateLeft2")) * 100
        End If

    End Sub

    Private Sub ConferenceId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ConferenceId.KeyUp
        If bm.ShowHelp("Conferences", ConferenceId, ConferenceName, e, "select cast(Id as varchar(100)) Id,Name from Conferences /*where IsActive=1*/") Then
            ConferenceId_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.Addcontrol_MouseDoubleClick({FromId, ToId, ConferenceId, SponsorId})

        dt = bm.ExecuteAdapter("select Id,name from Conferences where IsActive=1")
        Select Case dt.Rows.Count
            Case 0
                If Flag = 1 Then bm.ShowMSG("There is no active conference... please, contact your administator")
            Case 1
                ConferenceId.Text = dt.Rows(0)(0)
                ConferenceId_LostFocus(Nothing, Nothing)
            Case Else
                If Flag = 1 Then ConferenceId_KeyDown(Nothing, Nothing)
        End Select

        If Flag <> 1 Then
            lblFromDate.Visibility = Visibility.Hidden
            lblToDate.Visibility = Visibility.Hidden
            FromId.Visibility = Visibility.Hidden
            ToId.Visibility = Visibility.Hidden
        End If


        If Flag <> 2 AndAlso Flag <> 3 AndAlso Flag <> 7 Then
            lblSponsorId.Visibility = Visibility.Hidden
            SponsorId.Visibility = Visibility.Hidden
            SponsorName.Visibility = Visibility.Hidden
        End If

    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles FromId.KeyDown, ToId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub CustomerId_KeyUp(sender As Object, e As KeyEventArgs) Handles FromId.KeyUp, ToId.KeyUp
        If bm.ShowHelpMultiColumns("Customers", sender, sender, e, "select Code,Name,MobileNumber,WhatsappNumber from Customers") Then
            sender.Focus()
        End If

    End Sub


    Private Sub SponsorId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles SponsorId.KeyUp
        If bm.ShowHelp("Sponsors", SponsorId, SponsorName, e, "select cast(Id as varchar(100)) Id,Name from Sponsors", "Sponsors") Then
            SponsorId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SponsorId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SponsorId.LostFocus
        bm.LostFocus(SponsorId, SponsorName, "select Name from Sponsors where Id=" & SponsorId.Text.Trim())
    End Sub

End Class