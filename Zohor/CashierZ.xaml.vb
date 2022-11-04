Imports System.Data
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Threading

Public Class CashierZ

    Dim dt As New DataTable
    Dim dv As New DataView
    Dim bm As New BasicMethods
    Dim MyTextBoxes() As TextBox = {}
    Dim t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 0, 5)}

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.Addcontrol_MouseDoubleClick({SaveId})
        DayDate.SelectedDate = bm.MyGetDate()
        SaveId.Text = Md.DefaultSave
        SaveId_LostFocus(Nothing, Nothing)
        AddHandler t.Tick, AddressOf FillGrid
        t_Tick()
    End Sub


    Private Sub FillGrid()
        Try
            If DayDate.SelectedDate Is Nothing Then Return
            dt = bm.ExecuteAdapter("select T.EmpId,E.Name,T.CurrentShift,T.DayDate,T.Value from EmpCloseShift T left join Employees E on(E.Id=T.EmpId) where T.SaveId=0")
            dv.Table = dt
            DG.ItemsSource = dv

            DG.SelectionUnit = DataGridSelectionUnit.FullRow
            DG.IsReadOnly = True
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try

        Dim x As Decimal = 0
        For i As Integer = 0 To dt.Rows.Count - 1
            x += Val(dt.Rows(i)(4))
        Next
        Total.Text = x

        txt_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub txt_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.Sort = DG.Columns(sender.Tag).Header
        Catch
        End Try
    End Sub

    Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.RowFilter = " 1=1"
            For i As Integer = 0 To dt.Columns.Count - 1
                If dt.Columns(i).ColumnName = "IsSelected" Then Continue For
                dv.RowFilter &= " and [" & dt.Columns(i).ColumnName & "] like '%" & MyTextBoxes(i).Text & "%' "
            Next
        Catch
        End Try
    End Sub

    Private Sub t_Tick()
        Try
            SC.Children.Clear()
            For i As Integer = 0 To dt.Columns.Count - 1
                Dim txt As New TextBox With {.Height = 30, .Margin = New Thickness(0, 0, 0, 10)}
                ReDim Preserve MyTextBoxes(MyTextBoxes.Length + 1)
                MyTextBoxes(i) = txt
                Dim binding As New Binding("ActualWidth")
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                binding.Source = DG.Columns(i)
                txt.SetBinding(TextBox.WidthProperty, binding)
                txt.Tag = i
                txt.TabIndex = i
                txt.HorizontalAlignment = HorizontalAlignment.Left
                txt.VerticalAlignment = VerticalAlignment.Top
                SC.Children.Add(txt)
                AddHandler txt.GotFocus, AddressOf txt_Enter
                AddHandler txt.TextChanged, AddressOf txt_TextChanged
            Next
            DG.SelectedIndex = 0
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    Private Sub DG_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles DG.MouseDoubleClick
        t.Stop()
        If DG.SelectedIndex < 0 Then
            t.Start()
            Return
        End If
        If Val(SaveId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الخزنة")
            Return
        End If
        If bm.ShowDeleteMSG("هل أنت متأكد من استلام الحافظة من الموظف " & DG.SelectedItems(0)(1) & " بمبلغ " & DG.SelectedItems(0)(4)) Then ', Value
            If bm.ExecuteNonQuery("update EmpCloseShift set SaveId='" & Val(SaveId.Text) & "',SaveGetDate=getdate(),SaveDate=CAST(GETDATE() AS DATE),SaveUsername=" & Md.UserName & " where  EmpId='" & DG.SelectedItems(0)(0) & "' and CurrentShift='" & DG.SelectedItems(0)(2) & "'  ") Then
                PrintPone(DG.SelectedItems(0)(0), DG.SelectedItems(0)(2))
                'bm.ShowMSG("تمت العملية بنجاح")
                FillGrid()
            End If
        End If
        t.Start()
    End Sub

    Private Sub PrintPone(EmpId As Integer, CurrentShift As String)
        Dim rpt As New ReportViewer
        rpt.Rpt = "EmpCloseShiftOne3.rpt"
        rpt.paraname = New String() {"@EmpId", "@CurrentShift", "Header"}
        rpt.paravalue = New String() {EmpId, CurrentShift, CType(Parent, Page).Title}
        rpt.Print(".", Md.PonePrinter, 1)
    End Sub


    Private Sub SaveId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.LostFocus
        bm.LostFocus(SaveId, SaveName, "select Name from Fn_EmpPermissions(5," & Md.UserName & ") where Id=" & SaveId.Text.Trim(), True)
    End Sub

    Private Sub SaveId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.KeyUp
        If bm.ShowHelp("Saves", SaveId, SaveName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(5," & Md.UserName & ")") Then
            SaveId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub DayDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DayDate.SelectedDateChanged
        FillGrid()
    End Sub
End Class
