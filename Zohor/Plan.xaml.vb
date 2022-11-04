Imports System.Data
Imports System.Windows.Media.Animation

Public Class Plan


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Dim TblBuildings As New DataTable
    Dim TblBuildingsState As New DataTable
    Dim Flag As Integer = 1
    Dim CurrentWeek As Integer = 1
    Public MyFromDate As DateTime
    Public MyToDate As DateTime

    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        bm.TestIsLoaded(Me)
        LoadBuildings1()

        lblState1.Visibility = Visibility.Hidden
        lblState2.Visibility = Visibility.Hidden
        lblState3.Visibility = Visibility.Hidden
        lblState4.Visibility = Visibility.Hidden
        lblState5.Visibility = Visibility.Hidden
    End Sub

    Private Sub LoadBuildings1()
        Try
            Dim MyWidth As Double = (Wbuilding.ActualWidth - 160) / 24
            Dim DaysCount As Integer = DateDiff(DateInterval.Day, MyFromDate, MyToDate) + 1
            Dim MyHeight As Double = (Wbuilding.ActualHeight - 160) / DaysCount
            Wbuilding.Children.Clear()

            Animate2(AddControl(New Label, "", Md.CompanyName, Wbuilding.ActualWidth - 160, 100, 160, 0, 80, System.Windows.Media.Brushes.Silver))


            Dim wdt As Double = 0
            For i As Integer = 0 To 23
                Dim wd As Double = MyWidth
                Dim i2 As Integer = i + 1
                Dim Str As String = IIf(i > 12, i - 12, IIf(i = 0, i + 12, i)).ToString & " " & IIf(i < 12, "ص", "م")
                Str &= vbCrLf & "-" & vbCrLf & IIf(i2 > 12, i2 - 12, IIf(i2 = 0, i2 + 12, i2)).ToString & " " & IIf(i2 < 12, "ص", "م")
                AddControl(New Label, "", Str, wd, 60, 160 + wdt, 100, 14, System.Windows.Media.Brushes.Silver)
                wdt += wd
            Next

            Dim TblPlanSubFlags As DataTable = bm.ExecuteAdapter("select * from PlanSubFlags")

            Dim MyDate As DateTime = MyFromDate
            Dim x As Integer = 0
            While MyDate <= MyToDate
                Dim lbl As Label = AddControl(New Label, "DayLbl_1", bm.ToStrDate(MyDate), 100, MyHeight, 0, MyHeight * x + 160, 16, System.Windows.Media.Brushes.Silver)
                lbl.Tag = MyDate
                'lbl1.LayoutTransform = New RotateTransform(270)
                'lbl1.UpdateLayout()
                For xx As Integer = 0 To TblPlanSubFlags.Rows.Count - 1
                    Dim Mylbl As Label = AddControl(New Label, "MyLbl_" & xx, TblPlanSubFlags.Rows(xx)("Name"), 60, MyHeight / 3, 100, MyHeight * x + 160 + xx * MyHeight / 3, 16, System.Windows.Media.Brushes.Silver)
                Next
                x += 1
                MyDate = DateAdd(DateInterval.Day, 1, MyDate)
            End While

            MyHeight /= 3

            TblBuildingsState = bm.ExecuteAdapter("select * from PlanSub where DayDate between '" & bm.ToStrDate(MyFromDate) & "' and '" & bm.ToStrDate(MyToDate) & "' order by DayDate")
            For i As Integer = 0 To TblBuildingsState.Rows.Count - 1
                Dim DayDate As DateTime = TblBuildingsState.Rows(i)("DayDate")
                Dim FromHour As Integer = IIf(Val(TblBuildingsState.Rows(i)("FromHour")) = 12, 0, Val(TblBuildingsState.Rows(i)("FromHour"))) + Val(TblBuildingsState.Rows(i)("FromHourIndexId")) * 12
                Dim ToHour As Integer = IIf(Val(TblBuildingsState.Rows(i)("ToHour")) = 12, 0, Val(TblBuildingsState.Rows(i)("ToHour"))) + Val(TblBuildingsState.Rows(i)("ToHourIndexId")) * 12
                If FromHour = 24 Then ToHour = 0
                If ToHour = 0 Then ToHour = 24

                Dim Str As String = "   (" & IIf(FromHour > 12, FromHour - 12, IIf(FromHour = 0, FromHour + 12, FromHour)).ToString & " " & IIf(FromHour < 12, "ص", "م")
                Str &= " - " & IIf(ToHour > 12, ToHour - 12, IIf(ToHour = 0, ToHour + 12, ToHour)).ToString & " " & IIf(ToHour < 12, "ص", "م") & ")"

                Dim MyLbl As Label = AddControl(New Label, "lbl_" & TblBuildingsState.Rows(i)("Id") & "_" & FromHour.ToString & "_" & ToHour.ToString & "_", TblBuildingsState.Rows(i)("Name") & IIf(TblBuildingsState.Rows(i)("Name2") = "", "", " / " & TblBuildingsState.Rows(i)("Name2")) & vbCrLf & vbCrLf & Str, MyWidth * (ToHour - FromHour), MyHeight, MyWidth * FromHour + 160, 160 + MyHeight * (Val(TblBuildingsState.Rows(i)("Flag")) - 1) + MyHeight * DateDiff(DateInterval.Day, MyFromDate, DayDate), 18, IIf(1 = 2, System.Windows.Media.Brushes.LimeGreen, System.Windows.Media.Brushes.LightYellow))
                'If TblBuildingsState.Rows(i)("Crnt") = 1 Then Animate(MyLbl)

            Next

        Catch ex As Exception
            bm.ShowMSG(ex.Message)
        End Try

    End Sub

    Private Sub LoadBuildings2()
        Try

            Animate2(AddControl(New Label, "", Md.CompanyName & "                                                                                    " & "مدخل مدينة الإسكندرية الزراعي ""منطقة أبيس""", Wbuilding.ActualWidth - 60, Wbuilding.ActualHeight / 11, 60, 0, 60, System.Windows.Media.Brushes.Silver))

            Wbuilding.Children.Clear()
            dt = bm.ExecuteAdapter("GetFloors", {}, {})
            TblBuildingsState = bm.ExecuteAdapter("GetRooms", {}, {})

            Dim MyWidth As Double = Wbuilding.ActualWidth - 60
            Dim MyHeight As Double = Wbuilding.ActualHeight / (dt.Rows.Count * 4 + 1)

            Dim wdt As Double = 0
            For i As Integer = 0 To dt.Rows.Count - 1

                Dim MyRows As DataRow() = TblBuildingsState.Select("FloorId=" & dt.Rows(i)("Id"))
                If MyRows.Length = 0 Then Continue For
                Dim width As Double = MyWidth / MyRows.Length

                AddControl(New Label, "", dt.Rows(i)("Name"), MyWidth, MyHeight, 60 + wdt, (4 * i + 1) * MyHeight, 50, System.Windows.Media.Brushes.Silver)

                For i2 As Integer = 0 To MyRows.Length - 1
                    Dim dr As DataRow = MyRows(i2)
                    AddControl(New Label, "", MyRows(i2)("Name"), width, MyHeight, 60 + width * i2, (4 * i + 1) * MyHeight * 1 + MyHeight, 40, System.Windows.Media.Brushes.Silver)

                    Dim x As Integer = (2 * CurrentWeek - 1)
                    Dim MyLbl1 As Label = AddControl(New Label, "lbl_" & MyRows(i2)("FloorId") & "_" & MyRows(i2)("Id") & "_" & x, MyRows(i2)("Note" & x), width, MyHeight, 60 + width * i2, (4 * i + 1) * MyHeight * 1 + 2 * MyHeight, 22, IIf(MyRows(i2)("Crnt") = x, System.Windows.Media.Brushes.LimeGreen, System.Windows.Media.Brushes.LightYellow))
                    If MyRows(i2)("Crnt") = 1 Then Animate(MyLbl1)

                    Dim MyLbl2 As Label = AddControl(New Label, "lbl_" & MyRows(i2)("FloorId") & "_" & MyRows(i2)("Id") & "_" & (2 * CurrentWeek), MyRows(i2)("Note" & (x + 1)), width, MyHeight, 60 + width * i2, (4 * i + 1) * MyHeight * 1 + 3 * MyHeight, 22, IIf(MyRows(i2)("Crnt") = (x + 1), System.Windows.Media.Brushes.LimeGreen, System.Windows.Media.Brushes.LightYellow))
                    If MyRows(i2)("Crnt") = 2 Then Animate(MyLbl2)
                Next


            Next


        Catch ex As Exception
            bm.ShowMSG(ex.Message)
        End Try

    End Sub


    Function AddControl(x As Label, Myname As Object, c As Object, w As Integer, h As Integer, left As Decimal, top As Decimal, fz As Integer, Optional bg As Brush = Nothing) As Label
        Try
            x.Name = Myname
            x.Content = c
            x.HorizontalAlignment = HorizontalAlignment.Left
            x.VerticalAlignment = VerticalAlignment.Top
            x.HorizontalContentAlignment = HorizontalAlignment.Center
            x.Margin = New Thickness(left, top, 0, 0)
            x.BorderThickness = Mylbl.BorderThickness
            x.BorderBrush = Mylbl.BorderBrush
            x.Foreground = System.Windows.Media.Brushes.Black
            x.Height = h
            x.Width = w
            x.FontFamily = New FontFamily("Times New Roman")
            x.FontWeight = FontWeights.Bold
            x.FontSize = fz
            x.VerticalContentAlignment = VerticalAlignment.Center
            If bg Is Nothing Then
                TestColor(x)
            Else
                x.Background = bg
            End If

            Wbuilding.Children.Add(x)
            AddHandler x.PreviewMouseDoubleClick, AddressOf MyPreviewMouseDoubleClick
        Catch ex As Exception
            bm.ShowMSG(ex.Message)
        End Try
        Return x
    End Function

    Private Sub TestColor(x As Label, Optional Refresh As Boolean = False)
        Dim s As Integer

        Select Case s
            Case 1
                x.Background = lblState1.Background
            Case 2
                x.Background = lblState2.Background
            Case 3
                x.Background = lblState3.Background
            Case 4
                x.Background = lblState4.Background
            Case 5
                x.Background = lblState5.Background
        End Select
    End Sub

    Private Sub MyPreviewMouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        Try
            If sender.Name.ToString.StartsWith("lbl_") Then
                bm.ExecuteNonQuery("update Rooms set Crnt =" & sender.Name.ToString.Replace("lbl_", "").Split("_")(2) & " where FloorId=" & sender.Name.ToString.Replace("lbl_", "").Split("_")(0) & " and Id=" & sender.Name.ToString.Replace("lbl_", "").Split("_")(1))
                Select Case Flag
                    Case 1
                        LoadBuildings1()
                    Case 2
                        LoadBuildings2()
                End Select
            ElseIf sender.Name.ToString.StartsWith("DayLbl_") Then
                Dim x As Integer = Val(sender.Name.ToString.Replace("DayLbl_", ""))
                Dim Myfrm As New PlanSub With {.CurrentDay = sender.Tag}
                Dim frm As New Window With {.WindowState = WindowState.Maximized, .WindowStyle = WindowStyle.None, .Content = Myfrm}
                frm.ShowDialog()
                LoadBuildings1()
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub Animate(label As Label)
        Dim MyAnimation As DoubleAnimation
        MyAnimation = New DoubleAnimation()
        MyAnimation.From = 0.5
        MyAnimation.To = 1
        MyAnimation.AccelerationRatio = 0.0000000001
        MyAnimation.DecelerationRatio = 0.9
        'MyAnimation.SpeedRatio = 2
        MyAnimation.Duration = New Duration(TimeSpan.FromSeconds(1))
        MyAnimation.RepeatBehavior = Animation.RepeatBehavior.Forever
        MyAnimation.AutoReverse = True
        label.BeginAnimation(label.OpacityProperty, MyAnimation)
    End Sub


    Private Sub Animate2(label As Label)
        tbmarquee.Text = label.Content
        can.Width = label.Width
        can.Margin = label.Margin
        canMain.Width = label.Width
        canMain.Background = label.Background
        tbmarquee.Background = label.Background
        tbmarquee.Height = label.Height
        can.Height = label.Height
        canMain.Height = label.Height
        'tbmarquee.Width = label.Width

        tbmarquee.FontFamily = label.FontFamily
        tbmarquee.FontSize = label.FontSize
        
        label.Visibility = Visibility.Hidden
        Dim MyAnimation As New DoubleAnimation
        MyAnimation.From = canMain.Width
        MyAnimation.To = -canMain.Width / 4
        'MyAnimation.AccelerationRatio = 0.9999
        'MyAnimation.DecelerationRatio = 0.0001
        MyAnimation.RepeatBehavior = RepeatBehavior.Forever
        MyAnimation.Duration = New Duration(TimeSpan.Parse("0:00:20"))
        tbmarquee.BeginAnimation(Canvas.LeftProperty, MyAnimation)

       
    End Sub

End Class
