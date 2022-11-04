﻿Imports System.Data
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Windows.Forms

Public Class CalcSalary
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Hdr As String = ""
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Val(txtMonth.Text) = 0 OrElse Val(txtYear.Text) = 0 Then Return

        Dim rpt As New ReportViewer
        Select Case Flag
            Case 1
                bm.ExecuteNonQuery("CalcSalary", New String() {"Month", "Year"}, New String() {txtMonth.Text, txtYear.Text})
                bm.ShowMSG("Done Successfuly")
                Return
            Case 2
                rpt.Rpt = "SalaryHistoryShifts.rpt"
            Case 3
                rpt.Rpt = "SalaryHistoryNotShifts.rpt"
            Case 4
                LoadLog()
                Return
            Case 5
                rpt.Rpt = "SalaryHistoryAllTax.rpt"
            Case 6
                If bm.ShowDeleteMSG("إغلاق الوردية لا يمكنك من إعادة فتحها مرة أخرى" & vbCrLf & vbCrLf & "هل أنت متأكد من إغلاق الوردية؟") Then

                    If Md.ShowShiftForEveryStore Then
                        dt = bm.ExecuteAdapter("CloseShiftForEveryStore", New String() {"StoreId"}, New String() {Md.DefaultStore})
                    Else
                        bm.ExecuteNonQuery("exec CloseShift")
                    End If

                    bm.ShowMSG("تم إغلاق الوردية")
                    Try
                        Forms.Application.Restart()
                        Application.Current.Shutdown()
                    Catch ex As Exception
                    End Try
                End If
                Return
            Case 7
                Dim maintbl As String = "ServiceGroups"
                rpt.Rpt = IIf(maintbl = "", "PrintTbl.rpt", "PrintTbl2.rpt")
                rpt.paraname = {"Header", "@tbl", "@maintbl", "@mainfield"}
                rpt.paravalue = {CType(Parent, Page).Title, "ServiceTypes", maintbl, "ServiceGroupId"}
                rpt.Show()
                Return
            Case 8
                rpt.Rpt = "Employees.rpt"
            Case 9
                bm.ExecuteNonQuery("CalcAssetsDepreciation", New String() {"Month", "Year"}, New String() {txtMonth.Text, txtYear.Text})
                bm.ShowMSG("Done Successfuly")
                Return
            Case 10
                bm.ExecuteNonQuery("SHUTDOWN ", False)
                bm.ShowMSG("Done Successfuly")
                Return
            Case 11
                bm.ExecuteNonQuery("OpenNewYear", New String() {"NewYear"}, New String() {Val(Md.UdlName.Substring(Len(Md.UdlName) - 4, 4)) + 1})
                CopyUdl()
                bm.ShowMSG("تم فتح سنة مالية جديدة")
                Return
            Case 12
                CopyUdl()
                bm.ShowMSG("تم بدء مالية جديدة")
                Return
        End Select

        rpt.paraname = New String() {"@AccNo", "@Month", "@Period", "@Year", "Header"}
        rpt.paravalue = New String() {TaxAccNo.Text.Trim, Val(txtMonth.Text), Val(txtMonth.Text), Val(txtYear.Text), CType(Parent, Page).Title}
        rpt.Show()

    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.Addcontrol_MouseDoubleClick({TaxAccNo})
        LoadResource()
        Dim MyNow As DateTime = bm.MyGetDate()
        txtMonth.Text = MyNow.Month
        txtYear.Text = MyNow.Year

        If Flag = 6 Then
            GG.Children.Clear()
            Button2.Content = "إغلاق الوردية"
        ElseIf Flag = 7 Then
            GG.Children.Clear()
        ElseIf Flag = 8 Then
            GG.Children.Clear()
            Button2.Content = "عرض التقرير"
        ElseIf Flag = 10 Then
            GG.Children.Clear()
            Button2.Content = "SHUTDOWN"
        ElseIf Flag = 11 OrElse Flag = 12 Then
            GG.Children.Clear()
            Button2.Content = "بدء"
        End If


        If Flag = 5 Then
            TaxAccNo.Text = bm.ExecuteScalar("select dbo.GetTaxAcc()")
            TaxAccNo_LostFocus(Nothing, Nothing)
            Select Case MyNow.Month
                Case Is <= 3
                    txtMonth.Text = 1
                Case Is <= 6
                    txtMonth.Text = 2
                Case Is <= 9
                    txtMonth.Text = 3
                Case Else
                    txtMonth.Text = 4
            End Select
        End If
    End Sub
    Private Sub LoadResource()

        lblTaxAcc.SetResourceReference(ContentProperty, "TaxAcc")
        lblFromDate.SetResourceReference(ContentProperty, "Month")
        lblFromDate_Copy.SetResourceReference(ContentProperty, "Year")

        Select Case Flag
            Case 1
                Button2.SetResourceReference(ContentProperty, "Calculate")
            Case 2
                Button2.SetResourceReference(ContentProperty, "View Report")
            Case 3
                Button2.SetResourceReference(ContentProperty, "View Report")
            Case 4
                Button2.SetResourceReference(ContentProperty, "Import Attendance")
            Case 5
                Button2.SetResourceReference(ContentProperty, "View Report")
                lblFromDate.SetResourceReference(ContentProperty, "Period")
        End Select

        If Flag <> 5 Then
            lblTaxAcc.Visibility = Visibility.Hidden
            TaxAccNo.Visibility = Visibility.Hidden
            TaxAccName.Visibility = Visibility.Hidden
        End If
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtMonth.KeyDown, txtYear.KeyDown, TaxAccNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub LoadLog()
        Dim oo As New OpenFileDialog
        oo.Filter = "1_attlog.dat|1_attlog.dat"
        oo.FileName = "1_attlog.dat"
        If oo.ShowDialog() = DialogResult.Cancel Then Return
        Dim path As String = oo.FileName
        If Not File.Exists(path) Then
            bm.ShowMSG("Invalid path")
            Return
        End If

        Dim st As New StreamReader(path)
        Dim s As String = ""

        Dim AttendanceLogDT As New DataTable
        AttendanceLogDT.Columns.Add("EmpId")
        AttendanceLogDT.Columns.Add("DayDate")
        AttendanceLogDT.Columns.Add("State")

        Try
            While True
                s = st.ReadLine()
                If Val(s.Substring(10, 4)) = Val(txtYear.Text) AndAlso Val(s.Substring(15, 2)) = Val(txtMonth.Text) Then
                    AttendanceLogDT.Rows.Add({s.Substring(1, 8), s.Substring(10, 19), s.Substring(32, 1)})
                End If
            End While
        Catch ex As Exception
        End Try
        If bm.ExecuteNonQuery("SaveAttandanceLog", {"AttendanceLog"}, {AttendanceLogDT}, {DbType.Object}) Then
            bm.ShowMSG("Saved Successfuly")
        Else
            bm.ShowMSG("Faild to be Saved")
        End If

    End Sub

    Private Sub TaxAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TaxAccNo.LostFocus
        bm.AccNoLostFocus(TaxAccNo, TaxAccName, , , )
    End Sub

    Private Sub TaxAccNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles TaxAccNo.KeyUp
        bm.AccNoShowHelp(TaxAccNo, TaxAccName, e, , , )
    End Sub

    Private Sub CopyUdl()
        Dim NewConnect As String = System.Windows.Forms.Application.StartupPath & "\" & Md.UdlName.Substring(0, Len(Md.UdlName) - 4) & (Val(Md.UdlName.Substring(Len(Md.UdlName) - 4, 4)) + 1).ToString & ".udldll"
        IO.File.Copy(System.Windows.Forms.Application.StartupPath & "\" & Md.UdlName & ".udldll", NewConnect, True)

        Dim st As New StreamReader(NewConnect, True)
        Dim s As String = bm.Encrypt(bm.Decrypt(st.ReadToEnd).Replace(Md.con.Database, Md.con.Database.Substring(0, Len(Md.con.Database) - 4) & (Val(Md.con.Database.Substring(Len(Md.con.Database) - 4, 4)) + 1).ToString))
        st.Close()

        File.WriteAllText(NewConnect, s, Text.Encoding.Unicode)
    End Sub

End Class