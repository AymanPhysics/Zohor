Imports System.Data
Imports System.IO

Public Class Statics

    Dim bm As New BasicMethods

    Private Sub Statics_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return

        'If Not Md.Manager Then
        lblPoneComment.Visibility = Visibility.Hidden
        PoneComment.Visibility = Visibility.Hidden
        lblPoneHeader.Visibility = Visibility.Hidden
        PoneHeader.Visibility = Visibility.Hidden
        'End If

        Dim dt As DataTable = bm.ExecuteAdapter("select PoneComment,PoneHeader,CompanyName,CompanyTel,WhatsAppLink from Statics")
        If dt.Rows.Count > 0 Then
            PoneComment.Text = dt.Rows(0)("PoneComment").ToString
            PoneHeader.Text = dt.Rows(0)("PoneHeader").ToString
            CompanyName.Text = dt.Rows(0)("CompanyName").ToString
            CompanyTel.Text = dt.Rows(0)("CompanyTel").ToString
            WhatsAppLink.Text = dt.Rows(0)("WhatsAppLink").ToString
        End If

        bm.GetImage("Statics", New String() {}, New String() {}, "Logo", Image1)

        BarcodePrinter.Items.Clear()
        PonePrinter.Items.Clear()
        For i As Integer = 0 To System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count - 1
            BarcodePrinter.Items.Add(System.Drawing.Printing.PrinterSettings.InstalledPrinters(i))
            PonePrinter.Items.Add(System.Drawing.Printing.PrinterSettings.InstalledPrinters(i))
        Next

        BarcodePrinter.Text = Md.BarcodePrinter
        PonePrinter.Text = Md.PonePrinter

    End Sub


    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click

        If Md.Manager Then
            bm.ExecuteNonQuery("update Statics set PoneComment='" & PoneComment.Text.Replace("'", "''") & "',PoneHeader='" & PoneHeader.Text.Replace("'", "''") & "',CompanyName='" & CompanyName.Text.Replace("'", "''") & "',CompanyTel='" & CompanyTel.Text.Replace("'", "''") & "',WhatsAppLink='" & WhatsAppLink.Text.Replace("'", "''") & "'")
        End If

        bm.SaveImage("Statics", New String() {}, New String() {}, "Logo", Image1)

        Try
            Md.BarcodePrinter = BarcodePrinter.Text
            SaveSetting("OMEGA", "Settings", "BarcodePrinter", BarcodePrinter.Text)
            Dim st As New StreamWriter("BarcodePrinter.dll")
            st.WriteLine(BarcodePrinter.Text)
            st.Flush()
            st.Close()
        Catch ex As Exception
        End Try

        Try
            Md.PonePrinter = PonePrinter.Text
            SaveSetting("OMEGA", "Settings", "PonePrinter", PonePrinter.Text)
            Dim st As New StreamWriter("PonePrinter.dll")
            st.WriteLine(PonePrinter.Text)
            st.Flush()
            st.Close()
        Catch ex As Exception
        End Try

        bm.ShowMSG("تم الحفظ بنجاح")
    End Sub

    Private Sub btnSetImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetImage.Click
        bm.SetImage(Image1, False)
    End Sub

    Private Sub btnSetNoImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetNoImage.Click
        bm.SetNoImage(Image1, False, True)
    End Sub


End Class
