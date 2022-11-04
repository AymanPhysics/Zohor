Imports System.Data

Public Class InFertility

    Public MyCase As Integer = 0
    Public MyCaseName As String
    Dim bm As New BasicMethods

    Private Sub InFertility_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        Dim dt As DataTable = bm.ExecuteAdapter("select DiagnosisAndDecisions,Remarks1a,Remarks1b,Y1,Y2,Y3,Y4,Y5,Y6,Y7,Y8,Y9,Y10,Y11,Y12,Y13 from Cases where Id=" & MyCase)
        If dt.Rows.Count > 0 Then
            Try
                DiagnosisAndDecisions.Text = dt.Rows(0)("DiagnosisAndDecisions").ToString
                Remarks1a.Text = dt.Rows(0)("Remarks1a").ToString
                Remarks1b.Text = dt.Rows(0)("Remarks1b").ToString
                Y1.IsChecked = IIf(dt.Rows(0)("Y1") = 1, True, False)
                Y2.IsChecked = IIf(dt.Rows(0)("Y2") = 1, True, False)
                Y3.IsChecked = IIf(dt.Rows(0)("Y3") = 1, True, False)
                Y4.IsChecked = IIf(dt.Rows(0)("Y4") = 1, True, False)
                Y5.IsChecked = IIf(dt.Rows(0)("Y5") = 1, True, False)
                Y6.IsChecked = IIf(dt.Rows(0)("Y6") = 1, True, False)
                Y7.IsChecked = IIf(dt.Rows(0)("Y7") = 1, True, False)
                Y8.IsChecked = IIf(dt.Rows(0)("Y8") = 1, True, False)
                Y9.IsChecked = IIf(dt.Rows(0)("Y9") = 1, True, False)
                Y10.IsChecked = IIf(dt.Rows(0)("Y10") = 1, True, False)
                Y11.IsChecked = IIf(dt.Rows(0)("Y11") = 1, True, False)
                Y12.IsChecked = IIf(dt.Rows(0)("Y12") = 1, True, False)
                Y13.IsChecked = IIf(dt.Rows(0)("Y13") = 1, True, False)
            Catch ex As Exception
            End Try
        End If

    End Sub




    Private Sub btnComplaint_Click(sender As Object, e As RoutedEventArgs) Handles btnComplaint.Click
        Dim frm As New MyWindow With {.Title = "Complaint", .WindowState = WindowState.Maximized}
        Dim c As New Complaint
        c.MyCase = MyCase
        c.MyFlag = "InFertility"
        c.MyKey = 0
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnInvestigations_Click(sender As Object, e As RoutedEventArgs) Handles btnInvestigations.Click
        'Dim frm As New MyWindow With {.Title = "Investigations", .WindowState = WindowState.Maximized}
        'Dim c As New Complaint With {.TableName = "Investigations"}
        'c.MyCase = MyCase
        'c.MyFlag = "InFertility"
        'c.MyKey = 0
        'frm.Content = c
        'frm.ShowDialog()
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@CaseId"}
        rpt.Header = Md.MyProjectType.ToString
        rpt.paravalue = New String() {MyCase}
        rpt.Rpt = "LabTests2.rpt"
        rpt.Show()
    End Sub

    Private Sub btnUltraSound_Click(sender As Object, e As RoutedEventArgs) Handles btnUltraSound.Click
        Dim frm As New MyWindow With {.Title = "Ultra Sound", .WindowState = WindowState.Maximized}
        Dim c As New MyImages
        c.v1 = MyCase
        c.v2 = "InFertility"
        c.v3 = 0
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnDrugsAndDoses_Click(sender As Object, e As RoutedEventArgs) Handles btnDrugsAndDoses.Click
        Dim frm As New MyWindow With {.Title = "Drugs And Doses", .WindowState = WindowState.Maximized}
        Dim c As New DrugsAndDoses
        c.EmpId = Md.UserName
        c.DatePicker1 = bm.MyGetDate 'DayDate.SelectedDate
        c.ReservId = 1 'Val(txtID.Text)
        c.CaseId = MyCase
        c.CaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnNeededLabTests_Click(sender As Object, e As RoutedEventArgs) Handles btnNeededLabTests.Click
        Dim frm As New MyWindow With {.Title = "Investigations Request", .WindowState = WindowState.Maximized}
        Dim c As New NeededLabTests
        c.EmpId = Md.UserName
        c.DatePicker1 = bm.MyGetDate 'DayDate.SelectedDate
        c.ReservId = 1 'Val(txtID.Text)
        c.CaseId = MyCase
        c.CaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnICSI_Click(sender As Object, e As RoutedEventArgs) Handles btnICSI.Click
        Dim frm As New MyWindow With {.Title = CType(sender, Button).Content, .WindowState = WindowState.Maximized}
        Dim c As New InFertility1
        c.MyCase = MyCase
        c.MyCaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnIVFLab_Click(sender As Object, e As RoutedEventArgs) Handles btnIVFLab.Click
        Dim frm As New MyWindow With {.Title = CType(sender, Button).Content, .WindowState = WindowState.Maximized}
        Dim c As New InFertility2
        c.MyCase = MyCase
        c.MyCaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnFollicularScaning_Click(sender As Object, e As RoutedEventArgs) Handles btnFollicularScaning.Click
        Dim frm As New MyWindow With {.Title = CType(sender, Button).Content, .WindowState = WindowState.Maximized}
        Dim c As New InFertility3
        c.MyCase = MyCase
        c.MyCaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnOtherInfo_Click(sender As Object, e As RoutedEventArgs) Handles btnOtherInfo.Click
        Dim frm As New MyWindow With {.Title = CType(sender, Button).Content, .WindowState = WindowState.Maximized}
        Dim c As New InFertility5
        c.MyCase = MyCase
        c.MyCaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnOvarianFactor_Click(sender As Object, e As RoutedEventArgs) Handles btnOvarianFactor.Click
        Dim frm As New MyWindow With {.Title = "Ovarian Factor", .WindowState = WindowState.Maximized}
        Dim c As New OvarianFactor
        c.EmpId = Md.UserName
        c.DatePicker1 = New DateTime(1900, 1, 1)
        c.ReservId = 1 'Val(txtID.Text)
        c.CaseId = MyCase
        c.CaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnMaleFactor_Click(sender As Object, e As RoutedEventArgs) Handles btnMaleFactor.Click
        Dim frm As New MyWindow With {.Title = CType(sender, Button).Content, .WindowState = WindowState.Maximized}
        Dim c As New InFertility4
        c.MyCase = MyCase
        c.MyCaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnUtrineFactorUltraSound_Click(sender As Object, e As RoutedEventArgs) Handles btnUtrineFactorUltraSound.Click
        Dim frm As New MyWindow With {.Title = "Ultra Sound", .WindowState = WindowState.Maximized}
        Dim c As New CaseData
        c.MyCase = MyCase
        c.MyFlag = "UtrineFactorUltraSound"
        c.MyKey = 0
        c.lbl1.Content = "2 D"
        c.lbl2.Content = "3 D"
        c.lbl3.Content = "Doppler"
        c.ShowSecondName = True
        c.ShowThirdName = True
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnOffice_Click(sender As Object, e As RoutedEventArgs) Handles btnOffice.Click
        Dim frm As New MyWindow With {.Title = "Office", .WindowState = WindowState.Maximized}
        Dim c As New CaseData
        c.MyCase = MyCase
        c.MyFlag = "Office"
        c.MyKey = 0
        c.lbl1.Content = "Cavity"
        c.lbl2.Content = "Endometrium"
        c.lbl3.Content = "Ostea"
        c.lbl4.Content = "Pathology"
        c.ShowSecondName = True
        c.ShowThirdName = True
        c.ShowForthName = True
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnLaparoscope_Click(sender As Object, e As RoutedEventArgs) Handles btnLaparoscope.Click
        Dim frm As New MyWindow With {.Title = "Laparoscope", .WindowState = WindowState.Maximized}
        Dim c As New CaseData2
        c.txtName.VerticalAlignment = VerticalAlignment.Top
        c.txtName.AutoWordSelection = False
        c.txtName.AcceptsReturn = False
        c.txtName.TextWrapping = TextWrapping.NoWrap
        c.txtName.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled
        c.txtName.Height = c.DayDate.Height

        c.MyCase = MyCase
        c.MyFlag = "Laparoscope"
        c.MyKey = 0
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnHSG_Click(sender As Object, e As RoutedEventArgs) Handles btnHSG.Click
        Dim frm As New MyWindow With {.Title = "HSG", .WindowState = WindowState.Maximized}
        Dim c As New CaseData
        c.MyCase = MyCase
        c.MyFlag = "HSG"
        c.MyKey = 0
        c.lbl1.Content = "Diagnosis"
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub InFertility_Unloaded(sender As Object, e As RoutedEventArgs) Handles Me.Unloaded
        bm.ExecuteNonQuery("update cases set DiagnosisAndDecisions='" & DiagnosisAndDecisions.Text.Replace("'", "''") & "',Remarks1a='" & Remarks1a.Text.Replace("'", "''") & "',Remarks1b='" & Remarks1b.Text.Replace("'", "''") & "',Y1=" & IIf(Y1.IsChecked, 1, 0) & ",Y2=" & IIf(Y2.IsChecked, 1, 0) & ",Y3=" & IIf(Y3.IsChecked, 1, 0) & ",Y4=" & IIf(Y4.IsChecked, 1, 0) & ",Y5=" & IIf(Y5.IsChecked, 1, 0) & ",Y6=" & IIf(Y6.IsChecked, 1, 0) & ",Y7=" & IIf(Y7.IsChecked, 1, 0) & ",Y8=" & IIf(Y8.IsChecked, 1, 0) & ",Y9=" & IIf(Y9.IsChecked, 1, 0) & ",Y10=" & IIf(Y10.IsChecked, 1, 0) & ",Y11=" & IIf(Y11.IsChecked, 1, 0) & ",Y12=" & IIf(Y12.IsChecked, 1, 0) & ",Y13=" & IIf(Y13.IsChecked, 1, 0) & " where Id='" & MyCase & "'")
    End Sub

    Private Sub btnICSI2_Click(sender As Object, e As RoutedEventArgs) Handles btnICSI2.Click
        Dim frm As New MyWindow With {.Title = CType(sender, Button).Content, .WindowState = WindowState.Maximized}
        Dim c As New InFertility6
        c.MyCase = MyCase
        c.MyCaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnIUI_Click(sender As Object, e As RoutedEventArgs) Handles btnIUI.Click
        Dim frm As New MyWindow With {.Title = CType(sender, Button).Content, .WindowState = WindowState.Maximized}
        Dim c As New InFertility7
        c.MyCase = MyCase
        c.MyCaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnRisk_Click(sender As Object, e As RoutedEventArgs) Handles btnRisk.Click
        Dim frm As New MyWindow With {.Title = CType(sender, Button).Content, .WindowState = WindowState.Maximized}
        Dim c As New InFertirity8
        c.MyCase = MyCase
        c.MyCaseName = MyCaseName
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Private Sub btnInvestigationsEntry_Click(sender As Object, e As RoutedEventArgs) Handles btnInvestigationsEntry.Click
        Dim frm As New MyWindow With {.Title = "Investigations Entry", .WindowState = WindowState.Maximized}
        Dim c As New LabTests
        c.MyCase = MyCase
        frm.Content = c
        frm.ShowDialog()
    End Sub

    Public Sub Click(sender As Object, e As RoutedEventArgs) Handles Y1.Checked, Y1.Unchecked, Y2.Checked, Y2.Unchecked, Y3.Checked, Y3.Unchecked, Y4.Checked, Y4.Unchecked, Y5.Checked, Y5.Unchecked, Y6.Checked, Y6.Unchecked, Y7.Checked, Y7.Unchecked, Y8.Checked, Y8.Unchecked, Y9.Checked, Y9.Unchecked, Y10.Checked, Y10.Unchecked, Y11.Checked, Y11.Unchecked, Y12.Checked, Y12.Unchecked, Y13.Checked, Y13.Unchecked
        If sender.IsChecked Then
            sender.Foreground = System.Windows.Media.Brushes.Red
            sender.FontWeight = FontWeights.ExtraBold
        Else
            sender.Foreground = System.Windows.Media.Brushes.Black
            sender.FontWeight = FontWeights.Normal
        End If
    End Sub

End Class
