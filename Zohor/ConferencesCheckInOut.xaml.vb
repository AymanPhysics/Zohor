Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine

Public Class ConferencesCheckInOut

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0

    Dim CertificateTop As Integer = 0
    Dim CertificateLeft As Integer = 0
    Dim CertificateTop2 As Integer = 0
    Dim CertificateLeft2 As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        bm.Addcontrol_MouseDoubleClick({ConferenceId})

        If TypeOf (Parent) Is Page Then
            lblState.Content = CType(Parent, Page).Title
        ElseIf TypeOf (Parent) Is MyWindow Then
            lblState.Content = CType(Parent, MyWindow).Title
        End If

        If Not Md.MyProjectType = ProjectType.X Then
            dt = bm.ExecuteAdapter("select Id,name from Conferences where IsActive=1")
            Select Case dt.Rows.Count
                Case 0
                    bm.ShowMSG("There is no active conference... please, contact your administator")
                Case 1
                    ConferenceId.Text = dt.Rows(0)(0)
                    ConferenceId_LostFocus(Nothing, Nothing)
                Case Else
                    ConferenceId_KeyDown(Nothing, Nothing)
            End Select
        End If

        If Md.MyProjectType = ProjectType.X Then
            Button2.Visibility = Visibility.Hidden
            lblBank.Visibility = Visibility.Hidden
            ConferenceId.Visibility = Visibility.Hidden
            ConferenceName.Visibility = Visibility.Hidden
        End If

        If Flag = 3 Then
            Button2.Visibility = Visibility.Hidden
        End If

        Barcode.Focus()

    End Sub

    Private Sub ConferenceId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ConferenceId.LostFocus

        If Val(ConferenceId.Text.Trim) = 0 Then
            ConferenceId.Clear()
            ConferenceName.Clear()
            Return
        End If

        bm.LostFocus(ConferenceId, ConferenceName, "select Name from Conferences where IsActive=1 and Id=" & ConferenceId.Text.Trim())

        dt = bm.ExecuteAdapter("select CertificateTop,CertificateLeft,CertificateTop2,CertificateLeft2 from conferences where Id=" & ConferenceId.Text)
        If dt.Rows.Count = 1 Then
            CertificateTop = Val(dt.Rows(0)("CertificateTop")) * 100
            CertificateLeft = Val(dt.Rows(0)("CertificateLeft")) * 100
            CertificateTop2 = Val(dt.Rows(0)("CertificateTop2")) * 100
            CertificateLeft2 = Val(dt.Rows(0)("CertificateLeft2")) * 100
        End If


    End Sub
    Private Sub ConferenceId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles ConferenceId.KeyUp
        If bm.ShowHelp("Conferences", ConferenceId, ConferenceName, e, "select cast(Id as varchar(100)) Id,Name from Conferences where IsActive=1") Then
            ConferenceId_LostFocus(Nothing, Nothing)
        End If
    End Sub



    Private Sub Barcode_KeyUp(sender As Object, e As KeyEventArgs) Handles Barcode.KeyUp
        If Val(Barcode.Text) = 0 Then
            Barcode.Focus()
            Return
        End If

        If e.Key = Key.Enter Then

            If Md.MyProjectType = ProjectType.X Then
                bm.AppendStringToFile(CheckInOutFile, Flag & "," & Barcode.Text & "," & bm.ToStrDateTimeFormated(Now))
                Barcode.Clear()
                Barcode.Focus()
                Return
            End If



            If Val(ConferenceId.Text) = 0 Then
                bm.ShowMSG("Please, select a Conference")
                ConferenceId.Focus()
                Return
            End If
            If Not bm.ExecuteNonQuery("insert PL_CheckInOut(ConferenceId,Flag,CustomerId,DayDate) select '" & Val(ConferenceId.Text) & "','" & Flag & "','" & Val(Barcode.Text) & "',getdate()") Then

                If Not IO.Directory.Exists(Forms.Application.StartupPath & "\Data\" & Val(ConferenceId.Text) & "\" & Val(Flag) & "\" & Val(Barcode.Text)) Then
                    IO.Directory.CreateDirectory(Forms.Application.StartupPath & "\Data\" & Val(ConferenceId.Text) & "\" & Val(Flag) & "\" & Val(Barcode.Text))
                End If

                Try
                    IO.File.CreateText(Forms.Application.StartupPath & "\Data\" & Val(ConferenceId.Text) & "\" & Val(Flag) & "\" & Val(Barcode.Text) & "\" & bm.ToStrDateTimeFormated(Now).Replace(":", "--") & ".now")
                Catch ex As Exception
                    bm.ShowMSG(ex.Message)
                End Try

            End If




            If Flag = 3 Then

                Dim rpt As New ReportViewer
                rpt.Rpt = "Certificate.rpt"
                rpt.paraname = New String() {"CustomerId", "CustomerName", "AttendanceHours"}
                rpt.paravalue = New String() {Val(Barcode.Text), bm.ExecuteScalar("select Name from Customers where Id=" & Val(Barcode.Text)), bm.ExecuteScalar("select dbo.getAttendanceHours(" & Val(ConferenceId.Text) & "," & Val(Barcode.Text) & ")")}
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
                rpt.Print()

            End If

            Barcode.Clear()
            Barcode.Focus()
        End If
    End Sub

    Dim CheckInOutFile As String = Forms.Application.StartupPath & "\OfflineBarcodes.dll"
    Private Sub Button2_Click(sender As Object, e As RoutedEventArgs) Handles Button2.Click
        If Not IO.File.Exists(CheckInOutFile) Then
            bm.ShowMSG("System is up to date")
            Return
        End If

        Dim sql As String = ""
        Dim sr As StreamReader = New StreamReader(CheckInOutFile)


        Dim s As String
Repeat:
        s = sr.ReadLine()
        While s IsNot Nothing
            If s <> "" Then
                Dim ar() As String = s.Split(",")
                sql &= " insert PL_CheckInOut(ConferenceId,Flag,CustomerId,DayDate) select '" & Val(ConferenceId.Text) & "','" & ar(0) & "','" & ar(1) & "','" & ar(2) & "'"
            End If
            GoTo Repeat
        End While

        sr.Close()
        If Not bm.ExecuteNonQuery(sql) Then
            bm.ShowMSG("Failed")
        Else
            bm.ShowMSG("Done successfully")
            Dim dir As String = Forms.Application.StartupPath & "\OldOfflineBarcodes\" & bm.ToStrDateTimeFormated(Now).Replace(":", "")
            IO.Directory.CreateDirectory(dir)
            IO.File.Move(CheckInOutFile, dir & "\OfflineBarcodes.dll")
        End If

    End Sub
End Class
