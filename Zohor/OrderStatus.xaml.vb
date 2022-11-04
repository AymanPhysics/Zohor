Imports System.Data

Public Class OrderStatus


    Dim dt As New DataTable
    Dim dv As New DataView
    Dim bm As New BasicMethods
    Dim MyTextBoxes() As TextBox = {}

    Dim m As MainWindow = Application.Current.MainWindow
    Public WithImage As Boolean = False
    Public ReLoadMenue As Boolean = False
    Public Flag As Integer = 0

    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        
        FillData(0)
        bm.FillCombo("ProductionItemCollectionMotionFlags", CboFlag, " Where Flag=" & Flag, , True)
        If bm.TestIsLoaded(Me) Then Return
        bm.Addcontrol_MouseDoubleClick({EmpId1, EmpId2, EmpId3, EmpId4})
        Dim t As New System.Windows.Threading.DispatcherTimer With {.IsEnabled = True, .Interval = New System.TimeSpan(0, 0, 1)}
        AddHandler t.Tick, AddressOf t_Tick


        Select Case Flag
            Case 1
                '''''''''''''
            Case 1
                lblEmpId2.Visibility = Visibility.Hidden
                EmpId2.Visibility = Visibility.Hidden
                EmpName2.Visibility = Visibility.Hidden

                lblEmpId3.Visibility = Visibility.Hidden
                EmpId3.Visibility = Visibility.Hidden
                EmpName3.Visibility = Visibility.Hidden

                lblEmpId4.Visibility = Visibility.Hidden
                EmpId4.Visibility = Visibility.Hidden
                EmpName4.Visibility = Visibility.Hidden
            Case 2
                lblEmpId1.Visibility = Visibility.Hidden
                EmpId1.Visibility = Visibility.Hidden
                EmpName1.Visibility = Visibility.Hidden

                lblEmpId4.Visibility = Visibility.Hidden
                EmpId4.Visibility = Visibility.Hidden
                EmpName4.Visibility = Visibility.Hidden
            Case 3
                lblEmpId1.Visibility = Visibility.Hidden
                EmpId1.Visibility = Visibility.Hidden
                EmpName1.Visibility = Visibility.Hidden

                lblEmpId2.Visibility = Visibility.Hidden
                EmpId2.Visibility = Visibility.Hidden
                EmpName2.Visibility = Visibility.Hidden

                lblEmpId3.Visibility = Visibility.Hidden
                EmpId3.Visibility = Visibility.Hidden
                EmpName3.Visibility = Visibility.Hidden
            Case 4
                lblEmpId1.Visibility = Visibility.Hidden
                EmpId1.Visibility = Visibility.Hidden
                EmpName1.Visibility = Visibility.Hidden

                lblEmpId2.Visibility = Visibility.Hidden
                EmpId2.Visibility = Visibility.Hidden
                EmpName2.Visibility = Visibility.Hidden

                lblEmpId3.Visibility = Visibility.Hidden
                EmpId3.Visibility = Visibility.Hidden
                EmpName3.Visibility = Visibility.Hidden

                lblEmpId4.Visibility = Visibility.Hidden
                EmpId4.Visibility = Visibility.Hidden
                EmpName4.Visibility = Visibility.Hidden

        End Select

        lblEmpId1.Visibility = Visibility.Hidden
        EmpId1.Visibility = Visibility.Hidden
        EmpName1.Visibility = Visibility.Hidden

        lblEmpId2.Visibility = Visibility.Hidden
        EmpId2.Visibility = Visibility.Hidden
        EmpName2.Visibility = Visibility.Hidden

        lblEmpId3.Visibility = Visibility.Hidden
        EmpId3.Visibility = Visibility.Hidden
        EmpName3.Visibility = Visibility.Hidden

        lblEmpId4.Visibility = Visibility.Hidden
        EmpId4.Visibility = Visibility.Hidden
        EmpName4.Visibility = Visibility.Hidden

    End Sub

    Private Sub FillData(Flg As Integer)
        dt = bm.ExecuteAdapter("exec GetPreProductionOrder " & Flg & "," & Flag)
        dt.TableName = "tbl"

        dv.Table = dt
        DataGridView1.ItemsSource = dv
        'DataGridView1.IsReadOnly = True
        DataGridView1.Columns(1).Visibility = Visibility.Collapsed
        DataGridView1.Columns(3).Visibility = Visibility.Collapsed
        DataGridView1.Columns(10).Visibility = Visibility.Collapsed
        DataGridView1.Columns(11).Visibility = Visibility.Collapsed
        DataGridView1.RowHeaderWidth = 0
        DataGridView1.MinColumnWidth = 100
        'DataGridView1.SelectedIndex = 0 

        For i As Integer = 0 To DataGridView1.Columns.Count - 1
            DataGridView1.Columns(i).IsReadOnly = True
        Next
        DataGridView1.Columns(8).IsReadOnly = False
        DataGridView1.Columns(9).IsReadOnly = False
    End Sub

    Function MyMsg() As String
        Select Case Flag
            Case 1
                Return "تمت المراجعة؟"


            Case 1
                Return "تم التصوير؟"
            Case 2
                Return "تم التخريم والتغليف؟"
            Case 3
                Return "تمت المراجعة؟"
            Case 4
                Return "تحويل إلى أمر انتاج ..؟"
            Case Else
                Return ""
        End Select
    End Function

    Private Sub DataGridView1_PreviewMouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles DataGridView1.PreviewMouseDoubleClick
        If DataGridView1.CurrentColumn Is Nothing Then Return

        If Not DataGridView1.CurrentColumn.IsReadOnly Then Return
        If DataGridView1.Items.Count = 0 Then Return

        If EmpId1.Visibility = Visibility.Visible AndAlso Val(EmpId1.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد موظف ال" & lblEmpId1.Content)
            EmpId1.Focus()
            Return
        End If
        If EmpId2.Visibility = Visibility.Visible AndAlso Val(EmpId2.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد موظف ال" & lblEmpId2.Content)
            EmpId2.Focus()
            Return
        End If
        If EmpId3.Visibility = Visibility.Visible AndAlso Val(EmpId3.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد موظف ال" & lblEmpId3.Content)
            EmpId3.Focus()
            Return
        End If
        If EmpId4.Visibility = Visibility.Visible AndAlso Val(EmpId4.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد موظف ال" & lblEmpId4.Content)
            EmpId4.Focus()
            Return
        End If

        If bm.ShowDeleteMSG(MyMsg) Then
            Dim str As String = bm.ExecuteScalar("exec GenerateProductionOrder " & DataGridView1.CurrentItem("Flag") & ",'" & DataGridView1.CurrentItem("StoreId") & "'," & DataGridView1.CurrentItem("المسلسل") & "," & DataGridView1.CurrentItem("الكمية") & "," & Val(EmpId1.Text) & "," & Val(EmpId2.Text) & "," & Val(EmpId3.Text) & "," & Val(EmpId4.Text) & ",'" & DataGridView1.CurrentItem("ملاحظات").Trim & "'")
            'If Flag = 4 Then
            If Flag = 4 Then
                bm.ShowMSG("تم إنشاء أمر انتاج برقم " & str)
            Else
                bm.ShowMSG("تم إنشاء أمر مراجعة برقم " & str)
            End If

            Flag_SelectionChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub t_Tick(sender As Object, e As EventArgs)
        If Not sender Is Nothing Then CType(sender, System.Windows.Threading.DispatcherTimer).Stop()

        'Dim CurrentActualWidth As Integer = 0
        Try
            For i As Integer = 0 To dt.Columns.Count - 1
                Dim txt As New TextBox With {.Height = 30, .Margin = New Thickness(0, 0, 0, 10)}
                ReDim Preserve MyTextBoxes(MyTextBoxes.Length + 1)
                MyTextBoxes(i) = txt
                Dim binding As New Binding("ActualWidth")
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                binding.Source = DataGridView1.Columns(i)
                txt.SetBinding(TextBox.WidthProperty, binding)


                txt.Visibility = DataGridView1.Columns(i).Visibility
                txt.Tag = i
                txt.TabIndex = i
                txt.HorizontalAlignment = HorizontalAlignment.Left
                txt.VerticalAlignment = VerticalAlignment.Top
                SC.Children.Add(txt)
                AddHandler txt.GotFocus, AddressOf txt_Enter
                AddHandler txt.TextChanged, AddressOf txt_TextChanged
            Next
            DataGridView1.SelectedIndex = 0
        Catch
        End Try
        
    End Sub


    Private Sub txt_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.Sort = DataGridView1.Columns(sender.Tag).Header
        Catch
        End Try
    End Sub

    Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.RowFilter = " 1=1"
            For i As Integer = 0 To dt.Columns.Count - 1
                dv.RowFilter &= " and [" & dt.Columns(i).ColumnName & "] like '%" & MyTextBoxes(i).Text & "%' "
            Next
        Catch
        End Try
    End Sub

    Private Sub Flag_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CboFlag.SelectionChanged
        Try
            FillData(CboFlag.SelectedValue)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Flag", "@FlagFlag", "Header"}
        rpt.paravalue = New String() {Val(CboFlag.SelectedValue), Flag, CType(Parent, Page).Title}
        rpt.Rpt = "PreProductionOrder.rpt"
        rpt.Show()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs) Handles btnCancel.Click
        If DataGridView1.SelectedItem Is Nothing Then
            bm.ShowMSG("برجاء تحديد الصنف")
            Return
        End If
        If bm.ShowDeleteMSG() AndAlso bm.ExecuteNonQuery("update ProductionItemCollectionMotionMaster set NewInvoiceNo=-1 where Flag=" & DataGridView1.SelectedItem("Flag") & " and StoreId='" & DataGridView1.SelectedItem("StoreId") & "' and InvoiceNo=" & DataGridView1.SelectedItem("المسلسل")) Then
            Flag_SelectionChanged(Nothing, Nothing)
        End If
    End Sub


    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId1.KeyDown, EmpId2.KeyDown, EmpId3.KeyDown, EmpId4.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub EmpId1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId1.KeyUp
        bm.ShowHelp("Employees", EmpId1, EmpName1, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId2_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId2.KeyUp
        bm.ShowHelp("Employees", EmpId2, EmpName2, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId3_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId3.KeyUp
        bm.ShowHelp("Employees", EmpId3, EmpName3, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId4_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles EmpId4.KeyUp
        bm.ShowHelp("Employees", EmpId4, EmpName4, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId1.LostFocus
        bm.LostFocus(EmpId1, EmpName1, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId1.Text.Trim() & " and Stopped=0 ")
    End Sub

    Private Sub EmpId2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId2.LostFocus
        bm.LostFocus(EmpId2, EmpName2, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId2.Text.Trim() & " and Stopped=0 ")
    End Sub

    Private Sub EmpId3_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId3.LostFocus
        bm.LostFocus(EmpId3, EmpName3, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId3.Text.Trim() & " and Stopped=0 ")
    End Sub

    Private Sub EmpId4_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId4.LostFocus
        bm.LostFocus(EmpId4, EmpName4, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId4.Text.Trim() & " and Stopped=0 ")
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DataGridView1.SelectionChanged
        Return
        If DataGridView1.SelectedItem Is Nothing Then Return
        If DataGridView1.SelectedItem("IsType1") Then
            lblEmpId2.Visibility = Visibility.Visible
            EmpId2.Visibility = Visibility.Visible
            EmpName2.Visibility = Visibility.Visible

            lblEmpId3.Content = "تغليف"
        Else
            lblEmpId2.Visibility = Visibility.Hidden
            EmpId2.Visibility = Visibility.Hidden
            EmpName2.Visibility = Visibility.Hidden

            lblEmpId3.Content = "تجليد"
        End If

    End Sub
End Class
