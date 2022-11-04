Imports System.Data

Public Class RPT43
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    WithEvents G As New MyGrid
    Dim dt As DataTable
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click, Button3.Click, Button4.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Ban", "Header", "@PaymentDay", "@IsDelayed", "@All", "@Flag"}
        rpt.paravalue = New String() {bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate), IIf(Ban.IsChecked, 1, 0), CType(Parent, Page).Title, PaymentDay.SelectedValue, IIf(IsDelayed.IsChecked, 1, 0), 0, 3}

        Select Case Flag
            Case 1
                rpt.Rpt = "InstallmentInvoicesDateils.rpt"
                If sender Is Button3 Then rpt.Rpt = "InstallmentInvoicesDateils3.rpt"
                If sender Is Button4 Then rpt.Rpt = "InstallmentInvoicesDateils4.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

        bm.FillCombo("PaymentDays", PaymentDay, "", , True)

        LoadWFH()

    End Sub

    Structure GC
        Shared Flag As String = "Flag"
        Shared StoreId As String = "StoreId"
        Shared ToId As String = "ToId"
        Shared InvoiceNo As String = "InvoiceNo"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Mobile As String = "Mobile"
        Shared Insures_Name As String = "Insures_Name"
        Shared Insures_Mobile As String = "Insures_Mobile"
        Shared StoreName As String = "StoreName"
        Shared P_Name As String = "P_Name"
        Shared MainDayDate As String = "MainDayDate"
        Shared DayDate As String = "DayDate"
        Shared Value As String = "Value"
        Shared Payed As String = "Payed"
        Shared Remaining As String = "Remaining"
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Flag, "Flag")
        G.Columns.Add(GC.StoreId, "StoreId")
        G.Columns.Add(GC.ToId, "ToId")
        G.Columns.Add(GC.InvoiceNo, "InvoiceNo")
        G.Columns.Add(GC.Id, "Id")
        G.Columns.Add(GC.Name, "الاسم")
        G.Columns.Add(GC.Mobile, "الموبيل")
        G.Columns.Add(GC.Insures_Name, "اسم الضامن")
        G.Columns.Add(GC.Insures_Mobile, "موبيل الضامن")
        G.Columns.Add(GC.StoreName, "الفرع")
        G.Columns.Add(GC.P_Name, "يوم السداد")
        G.Columns.Add(GC.MainDayDate, "التاريخ")
        G.Columns.Add(GC.DayDate, "تاريخ الاستحقاق")
        G.Columns.Add(GC.Value, "القيمة")
        G.Columns.Add(GC.Payed, "المدفوع")
        G.Columns.Add(GC.Remaining, "المتبقي")
        G.Columns.Add(GC.Notes, "ملاحظات")

        G.Columns(GC.Flag).Visible = False
        G.Columns(GC.StoreId).Visible = False
        G.Columns(GC.ToId).Visible = False
        G.Columns(GC.InvoiceNo).Visible = False
        G.Columns(GC.Id).Visible = False

        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Mobile).ReadOnly = True
        G.Columns(GC.Insures_Name).ReadOnly = True
        G.Columns(GC.Insures_Mobile).ReadOnly = True
        G.Columns(GC.StoreName).ReadOnly = True
        G.Columns(GC.P_Name).ReadOnly = True
        G.Columns(GC.MainDayDate).ReadOnly = True
        G.Columns(GC.DayDate).ReadOnly = True
        G.Columns(GC.Value).ReadOnly = True
        G.Columns(GC.Payed).ReadOnly = True
        G.Columns(GC.Remaining).ReadOnly = True
        G.Columns(GC.Notes).ReadOnly = True

        G.RowHeadersVisible = False
        G.AllowUserToAddRows = False

        AddHandler G.SelectionChanged, AddressOf G_SelectionChanged
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")


        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

    End Sub

    Private Sub IsLate_Checked(sender As Object, e As RoutedEventArgs) Handles IsLate.Checked
        Ban.IsChecked = False
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(2000, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0).AddDays(-1)
    End Sub

    Private Sub IsLate_Unchecked(sender As Object, e As RoutedEventArgs) Handles IsLate.Unchecked
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Private Sub Ban_Checked(sender As Object, e As RoutedEventArgs) Handles Ban.Checked
        IsLate.IsChecked = False
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(2000, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year + 10, MyNow.Month, MyNow.Day, 0, 0, 0).AddDays(-1)
    End Sub

    Private Sub Ban_Unchecked(sender As Object, e As RoutedEventArgs) Handles Ban.Unchecked
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub



    Private Sub FromDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles FromDate.SelectedDateChanged
        'ToDate.SelectedDate = FromDate.SelectedDate
    End Sub

    Private Sub G_SelectionChanged(sender As Object, e As EventArgs)
        Try
            DayDate.SelectedDate = CType(G.CurrentRow.Cells(GC.DayDate).Value, DateTime)
            Notes.Text = G.CurrentRow.Cells(GC.Notes).Value.ToString
            Value.Text = G.CurrentRow.Cells(GC.Value).Value.ToString
        Catch ex As Exception
            DayDate.SelectedDate = Nothing
            Notes.Clear()
            Value.Clear()
        End Try
    End Sub

    Private Sub Button2_Copy_Click(sender As Object, e As RoutedEventArgs) Handles Button2_Copy.Click
        GetData()
    End Sub


    Private Sub Button2_Copy1_Click(sender As Object, e As RoutedEventArgs) Handles Button2_Copy1.Click
        If Val(Value.Text) <> Val(G.CurrentRow.Cells(GC.Value).Value) Then
            If Not bm.ShowDeleteMSG("سيتم تعديل قيمة القسط من " & Val(G.CurrentRow.Cells(GC.Value).Value) & " إلى " & Val(Value.Text)) Then
                Return
            End If
        End If
        If bm.ExecuteNonQuery("update InstallmentInvoicesDateils set DelayedDate=getdate(),IsDelayed=1,DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "',Notes='" & Notes.Text.Replace("'", "''") & "',Value='" & Val(Value.Text) & "' where Flag='" & G.CurrentRow.Cells(GC.Flag).Value & "' and StoreId='" & G.CurrentRow.Cells(GC.StoreId).Value & "' and ToId='" & G.CurrentRow.Cells(GC.ToId).Value & "' and InvoiceNo='" & G.CurrentRow.Cells(GC.InvoiceNo).Value & "' and Id='" & G.CurrentRow.Cells(GC.Id).Value & "'") Then
            bm.ShowMSG("تم الحفظ")
            GetData()
        End If
    End Sub

    Private Sub GetData()
        dt = bm.ExecuteAdapter("GetInstallmentInvoicesDateils", {"FromDate", "ToDate", "Ban", "PaymentDay", "IsDelayed", "All", "Flag"}, {bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate), IIf(Ban.IsChecked, 1, 0), PaymentDay.SelectedValue, IIf(IsDelayed.IsChecked, 1, 0), 0, 3})

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Flag).Value = dt.Rows(i)("Flag")
            G.Rows(i).Cells(GC.StoreId).Value = dt.Rows(i)("StoreId")
            G.Rows(i).Cells(GC.ToId).Value = dt.Rows(i)("ToId")
            G.Rows(i).Cells(GC.InvoiceNo).Value = dt.Rows(i)("InvoiceNo")
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("Id")
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("Name")
            G.Rows(i).Cells(GC.Mobile).Value = dt.Rows(i)("Mobile")
            G.Rows(i).Cells(GC.Insures_Name).Value = dt.Rows(i)("Insures_Name")
            G.Rows(i).Cells(GC.Insures_Mobile).Value = dt.Rows(i)("Insures_Mobile")
            G.Rows(i).Cells(GC.StoreName).Value = dt.Rows(i)("StoreName")
            G.Rows(i).Cells(GC.P_Name).Value = dt.Rows(i)("P_Name")
            G.Rows(i).Cells(GC.MainDayDate).Value = bm.ToStrDate(dt.Rows(i)("MainDayDate"))
            G.Rows(i).Cells(GC.DayDate).Value = bm.ToStrDate(dt.Rows(i)("DayDate"))
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value")
            G.Rows(i).Cells(GC.Payed).Value = dt.Rows(i)("Payed")
            G.Rows(i).Cells(GC.Remaining).Value = dt.Rows(i)("Remaining")
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes")
        Next

        Try
            G.CurrentCell = G.Rows(0).Cells(GC.Name)
            G_SelectionChanged(Nothing, Nothing)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button2_Copy2_Click(sender As Object, e As RoutedEventArgs) Handles Button2_Copy2.Click
        If bm.ShowDeleteMSG("هل أنت متأكد من التنازل عن القسط؟") Then
            If bm.ExecuteNonQuery("update InstallmentInvoicesDateils set IsCanceled=1,UserCanceled=" & Md.UserName & ",CanceledDate=getDate() where Flag='" & G.CurrentRow.Cells(GC.Flag).Value & "' and StoreId='" & G.CurrentRow.Cells(GC.StoreId).Value & "' and ToId='" & G.CurrentRow.Cells(GC.ToId).Value & "' and InvoiceNo='" & G.CurrentRow.Cells(GC.InvoiceNo).Value & "' and Id='" & G.CurrentRow.Cells(GC.Id).Value & "'") Then
                bm.ShowMSG("تم الحفظ")
                GetData()
            End If
        End If
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub


End Class