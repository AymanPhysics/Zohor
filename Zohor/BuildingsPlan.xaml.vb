Imports System.Data

Public Class BuildingsPlan
    

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Dim TblBuildings As New DataTable
    Dim TblBuildingsState As New DataTable
    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadBuildings()


    End Sub

    Sub LoadBuildings()
        Try
            WGroups.Children.Clear()
            
            TblBuildings = bm.ExecuteAdapter("Select * from Buildings order by Id")
            For i As Integer = 0 To TblBuildings.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x, 140, 50)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & TblBuildings.Rows(i)("Id").ToString
                x.Tag = TblBuildings.Rows(i)("Id").ToString
                x.Content = TblBuildings.Rows(i)("Name").ToString
                x.ToolTip = TblBuildings.Rows(i)("Name").ToString
                WGroups.Children.Add(x)
                AddHandler x.Click, AddressOf btn_Click
            Next
        Catch
        End Try
    End Sub

    Private Sub btn_Click(sender As Object, e As RoutedEventArgs)
        Try

            TblBuildingsState = bm.ExecuteAdapter("GetAppartementStateAll", {"BuildingId"}, {sender.Tag})

            Dim MyWidth As Double = Wbuilding.ActualWidth - 140
            Dim dr As DataRow = TblBuildings.Select("Id=" & sender.Tag)(0)
            TabBuildings1.Tag = sender.Tag
            TabBuildings1.Header = dr("Name")
            Wbuilding.Children.Clear()
            dt = bm.ExecuteAdapter("select * from UnitsTypes where BuildingId=" & sender.Tag)


            AddControl(New Label, "", "المساحة", 100, 10, 60, System.Windows.Media.Brushes.Yellow)
            AddControl(New Label, "", "السعر", 100, 10, 100, System.Windows.Media.Brushes.Yellow)

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim wd As Double = MyWidth / dt.Rows.Count
                AddControl(New Label, "", dt.Rows(i)("Name"), wd, 120 + i * wd, 20, System.Windows.Media.Brushes.Silver)
                AddControl(New Label, "lblArea_" & (i + 1).ToString, dt.Rows(i)("Area"), wd, 120 + i * wd, 60, System.Windows.Media.Brushes.SkyBlue)
                AddControl(New Label, "lblPrice_" & (i + 1).ToString, dt.Rows(i)("Price"), wd, 120 + i * wd, 100, System.Windows.Media.Brushes.SkyBlue)
            Next

            For i As Integer = dr("FloorCount") To 1 Step -1
                AddControl(New Label, "", "الدور" & i, 100, 10, 140 + 40 * (dr("FloorCount") - i), System.Windows.Media.Brushes.Silver)
                For i2 As Integer = 0 To dt.Rows.Count - 1
                    Dim wd As Double = MyWidth / dt.Rows.Count
                    Dim x As Label = AddControl(New Label, "lbl_" & sender.Tag.ToString & "_" & i.ToString & "_" & (i2 + 1).ToString, sender.Tag.ToString & "/" & i.ToString & "/" & (i2 + 1).ToString, wd, 120 + i2 * wd, 140 + 40 * (dr("FloorCount") - i))
                    'x.Background = lblState1.Background
                    AddHandler x.MouseDoubleClick, AddressOf x_MouseDoubleClick
                Next
            Next

            For i As Integer = 0 To dr("BalanceCount") - 1
                If i = 0 Then AddControl(New Label, "", "الميزانين", 100, 10, 140 + 40 * dr("FloorCount"), System.Windows.Media.Brushes.Silver)
                Dim wd As Double = MyWidth / dr("BalanceCount")
                Dim x As Label = AddControl(New Label, "lbl_" & sender.Tag.ToString & "_0_" & (i + 1).ToString, sender.Tag.ToString & "/0/" & (i + 1).ToString, wd, 120 + i * wd, 140 + 40 * dr("FloorCount"))
                AddHandler x.MouseDoubleClick, AddressOf x_MouseDoubleClick
            Next

            For i As Integer = 0 To dr("ShopCount") - 1
                If i = 0 Then AddControl(New Label, "", "المحلات", 100, 10, 180 + 40 * dr("FloorCount"), System.Windows.Media.Brushes.Silver)
                Dim wd As Double = MyWidth / dr("ShopCount")
                Dim x As Label = AddControl(New Label, "lbl_" & sender.Tag.ToString & "_00_" & (i + 1).ToString, sender.Tag.ToString & "/00/" & (i + 1).ToString, wd, 120 + i * wd, 180 + 40 * dr("FloorCount"))
                AddHandler x.MouseDoubleClick, AddressOf x_MouseDoubleClick
            Next

            For i As Integer = 0 To dr("GarageCount") - 1
                If i = 0 Then AddControl(New Label, "", "الجراجات", 100, 10, 220 + 40 * dr("FloorCount"), System.Windows.Media.Brushes.Silver)
                Dim wd As Double = MyWidth / dr("GarageCount")
                Dim x As Label = AddControl(New Label, "lbl_" & sender.Tag.ToString & "_000_" & (i + 1).ToString, sender.Tag.ToString & "/000/" & (i + 1).ToString, wd, 120 + i * wd, 220 + 40 * dr("FloorCount"))
                AddHandler x.MouseDoubleClick, AddressOf x_MouseDoubleClick
            Next
        Catch ex As Exception
            bm.ShowMSG(ex.Message)
        End Try

    End Sub


    Function AddControl(x As Label, Myname As Object, c As Object, w As Integer, left As Decimal, top As Decimal, Optional bg As Brush = Nothing) As Label
        Try
            x.Name = Myname
            x.Content = c
            x.ToolTip = c
            x.HorizontalAlignment = HorizontalAlignment.Left
            x.VerticalAlignment = VerticalAlignment.Top
            x.HorizontalContentAlignment = HorizontalAlignment.Center
            x.Margin = New Thickness(left, top, 0, 0)
            x.BorderThickness = Mylbl.BorderThickness
            x.BorderBrush = Mylbl.BorderBrush
            x.Foreground = System.Windows.Media.Brushes.Black
            x.Height = 28
            x.Width = w
            If bg Is Nothing Then
                TestColor(x)
            Else
                x.Background = bg
            End If

            Wbuilding.Children.Add(x)
        Catch ex As Exception
            bm.ShowMSG(ex.Message)
        End Try
        Return x
    End Function

    Private Sub x_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        If Rdo1.IsChecked Then
            Try
                Dim frm As New MyDialog
                Dim c As New AppartementsSales
                frm.Content = c
                frm.SizeToContent = SizeToContent.WidthAndHeight

                c.txtID.Text = sender.Name.ToString.Replace("lbl_", "").Replace("_", "/")
                c.BuildingName.Text = TabBuildings1.Header
                c.Floor.Text = c.txtID.Text.Split("/")(1)
                c.UnitNo.Text = c.txtID.Text.Split("/")(2)

                If Val(c.Floor.Text) > 0 Then
                    dt = bm.ExecuteAdapter("select * from UnitsTypes where BuildingId=" & TabBuildings1.Tag & " and Id=" & c.UnitNo.Text)
                    c.Sample.Text = dt.Rows(0)("Name")
                    c.PriceBefore.Text = dt.Rows(0)("Price")
                    c.Area.Text = dt.Rows(0)("Area")
                End If

                Dim MyNow As DateTime = bm.MyGetDate()
                c.DayDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

                frm.ShowDialog()
                TestColor(sender, True)
            Catch
            End Try
        ElseIf Rdo2.IsChecked Then
            Try
                Dim frm As New MyDialog
                Dim c As New AppartementInstallments
                frm.Content = c
                frm.SizeToContent = SizeToContent.WidthAndHeight

                c.txtID.Text = sender.Name.ToString.Replace("lbl_", "").Replace("_", "/")
                c.BuildingName.Text = TabBuildings1.Header
                c.Floor.Text = c.txtID.Text.Split("/")(1)
                c.UnitNo.Text = c.txtID.Text.Split("/")(2)

                If Val(c.Floor.Text) > 0 Then
                    dt = bm.ExecuteAdapter("select * from UnitsTypes where BuildingId=" & TabBuildings1.Tag & " and Id=" & c.UnitNo.Text)
                    c.Sample.Text = dt.Rows(0)("Name")
                    c.PriceBefore.Text = dt.Rows(0)("Price")
                    c.Area.Text = dt.Rows(0)("Area")
                End If

                Dim MyNow As DateTime = bm.MyGetDate()
                c.DayDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

                frm.ShowDialog()
                TestColor(sender, True)
            Catch
            End Try
        Else
            Dim str As String = sender.Name.ToString.Replace("lbl_", "").Replace("_", "/")
            Dim rpt As New ReportViewer
            rpt.paraname = New String() {"@Id", "Header"}
            rpt.paravalue = New String() {str, "كشف حساب عميل"}
            rpt.Rpt = "GetAppartementSalesState.rpt"
            rpt.Show()
        End If
        
    End Sub

    Private Sub TestColor(x As Label, Optional Refresh As Boolean = False)
        Dim s As Integer
        If Refresh Then
            s = Val(bm.ExecuteScalar("select dbo.GetAppartementState('" & x.Content & "')"))
        Else
            s = Val(TblBuildingsState.Select("AppartementId='" & x.Content & "'")(0)("State"))
        End If

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


End Class
