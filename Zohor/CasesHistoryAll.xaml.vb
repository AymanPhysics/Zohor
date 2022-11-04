Imports System.Data
Imports System.ComponentModel
Imports System.IO

Public Class CasesHistoryAll
    Public TableName As String = "CasesHistoryAll"
    Public SubId As String = "InvoiceNo"

    Dim dt As New DataTable
    Dim dv As New DataView
    Dim bm As New BasicMethods
    Dim MyTextBoxes() As TextBox = {}

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnGet}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnSave})
        
        LoadResource()

        bm.Fields = New String() {SubId, "FromDate", "ToDate"}
        bm.control = New Control() {txtID, FromDate, ToDate}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)

        t_Tick()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()

        bm.FillControls(Me)

        FillGrid()

        DayDate.Focus()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Save()
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@InvoiceNo", "Header"}
        rpt.paravalue = New String() {Val(txtID.Text), CType(Parent, Page).Title.Trim}
        rpt.Rpt = "CasesHistoryAll.rpt"
        rpt.Show()

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()

        bm.ClearControls()
        txtID.Clear()

        FillGrid()

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        ToDate.SelectedDate = MyNow
        'txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        'If txtID.Text = "" Then txtID.Text = "1"
        DayDate.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub
     
    Private Sub LoadResource()
        Return
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblId.SetResourceReference(ContentProperty, "Id")

    End Sub

    Private Sub btnGet_Click(sender As Object, e As RoutedEventArgs) Handles btnGet.Click
        Dim FD As String = "1900/01/01"
        If Not FromDate.SelectedDate Is Nothing Then FD = bm.ToStrDate(FromDate.SelectedDate)
        Dim TD As String = "1900/01/01"
        If Not ToDate.SelectedDate Is Nothing Then TD = bm.ToStrDate(ToDate.SelectedDate)

        txtID.Text = bm.ExecuteScalar("CreateCasesHistoryAll", {"InvoiceNo", "FromDate", "ToDate"}, {Val(txtID.Text), FD, TD})
        txtID_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub FillGrid()
        Try
            If Val(txtID.Text) = 0 Then Return
            dt = bm.ExecuteAdapter("select cast(0 as bit)IsSelected,cast(InvoiceNo as nvarchar(100))InvoiceNo,cast(CaseId as nvarchar(100))CaseId,CaseName,cast(TypeId as nvarchar(100))TypeId,dbo.ToStrDateTime(EntryDate)EntryDate,isnull(dbo.ToStrDateTime(ExitGetDate),'')ExitGetDate,CaseTypeName,isnull(cast(Total as nvarchar(100)),'')Total,isnull(cast(Discount as nvarchar(100)),'')Discount,isnull(cast(Value as nvarchar(100)),'')Value,isnull(cast(Payed as nvarchar(100)),'')Payed,isnull(cast(Remaining as nvarchar(100)),'')Remaining,isnull(cast(SurgeonId as nvarchar(100)),'0')SurgeonId,T5_SurgeonName,isnull(cast(SurgeonId2 as nvarchar(100)),'0')SurgeonId2,T10_SurgeonName2,isnull(cast(SurgeonId3 as nvarchar(100)),'0')SurgeonId3,T10_SurgeonName3,isnull(cast(AnesthetistId as nvarchar(100)),'0')AnesthetistId,T10_AnesthetistName,isnull(OperationTypeName,'')OperationTypeName,isnull(RoomName,'')RoomName,dbo.ToStrDate(FromDate)FromDate,dbo.ToStrDate(ToDate)ToDate,cast(Line as nvarchar(100))Line from " & TableName & " where InvoiceNo=" & Val(txtID.Text))
            dv.Table = dt
            DG.ItemsSource = dv

            DG.Columns(dt.Columns("IsSelected").Ordinal).Header = "تحديد"
            DG.Columns(dt.Columns("InvoiceNo").Ordinal).Header = "المسلسل"
            DG.Columns(dt.Columns("CaseId").Ordinal).Header = "MRN"
            DG.Columns(dt.Columns("CaseName").Ordinal).Header = "الاسم"
            DG.Columns(dt.Columns("EntryDate").Ordinal).Header = "تاريخ الدخول"
            DG.Columns(dt.Columns("ExitGetDate").Ordinal).Header = "تاريخ الخروج"
            DG.Columns(dt.Columns("CaseTypeName").Ordinal).Header = "النوع"
            DG.Columns(dt.Columns("Total").Ordinal).Header = "المبلغ"
            DG.Columns(dt.Columns("Discount").Ordinal).Header = "الخصم"
            DG.Columns(dt.Columns("Value").Ordinal).Header = "المدفوع"
            DG.Columns(dt.Columns("Payed").Ordinal).Header = "السداد"
            DG.Columns(dt.Columns("Remaining").Ordinal).Header = "المتبقي"
            DG.Columns(dt.Columns("T5_SurgeonName").Ordinal).Header = "الطبيب 1"
            DG.Columns(dt.Columns("T10_SurgeonName2").Ordinal).Header = "الطبيب 2"
            DG.Columns(dt.Columns("T10_SurgeonName3").Ordinal).Header = "الطبيب 3"
            DG.Columns(dt.Columns("T10_AnesthetistName").Ordinal).Header = "التخدير"
            DG.Columns(dt.Columns("OperationTypeName").Ordinal).Header = "البيان"
            DG.Columns(dt.Columns("RoomName").Ordinal).Header = "جهة التسكين"

            DG.Columns(dt.Columns("Payed").Ordinal).Visibility = Visibility.Hidden
            DG.Columns(dt.Columns("TypeId").Ordinal).Visibility = Visibility.Hidden
            DG.Columns(dt.Columns("SurgeonId").Ordinal).Visibility = Visibility.Hidden
            DG.Columns(dt.Columns("SurgeonId2").Ordinal).Visibility = Visibility.Hidden
            DG.Columns(dt.Columns("SurgeonId3").Ordinal).Visibility = Visibility.Hidden
            DG.Columns(dt.Columns("AnesthetistId").Ordinal).Visibility = Visibility.Hidden
            DG.Columns(dt.Columns("FromDate").Ordinal).Visibility = Visibility.Hidden
            DG.Columns(dt.Columns("ToDate").Ordinal).Visibility = Visibility.Hidden
            DG.Columns(dt.Columns("Line").Ordinal).Visibility = Visibility.Hidden

            For index = 0 To DG.Columns.Count - 1
                DG.Columns(index).IsReadOnly = True
            Next
            DG.Columns(dt.Columns("IsSelected").Ordinal).IsReadOnly = False
            DG.Columns(dt.Columns("Value").Ordinal).IsReadOnly = False

            DG.SelectionUnit = DataGridSelectionUnit.CellOrRowHeader
            DG.RowHeaderWidth = 0
            DG.MinColumnWidth = 0
            DG.Columns(dt.Columns("InvoiceNo").Ordinal).Visibility = Visibility.Hidden
            DG.Columns(dt.Columns("FromDate").Ordinal).Visibility = Visibility.Hidden
            DG.Columns(dt.Columns("ToDate").Ordinal).Visibility = Visibility.Hidden

        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try

        t_Tick()

    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As RoutedEventArgs) Handles btnSelectAll.Click
        If btnSelectAll.Tag Is Nothing Then btnSelectAll.Tag = False
        btnSelectAll.Tag = Not btnSelectAll.Tag
        For index = 0 To DG.Items.Count - 1
            DG.Items(index)("IsSelected") = btnSelectAll.Tag
        Next
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

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        Save()
        Dim Str As String = "0"
        For index = 0 To DG.Items.Count - 1
            If DG.Items(index)("IsSelected") Then
                Str &= "," & vbCrLf & DG.Items(index)("Line")
            End If
        Next

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@InvoiceNo", "@Str", "Header"}
        rpt.paravalue = New String() {Val(txtID.Text), Str, CType(Parent, Page).Title.Trim}
        rpt.Rpt = "CasesHistoryAll2.rpt"
        rpt.Show()
    End Sub

    Dim MyRow As Integer = -1
    Private Sub DG_BeginningEdit(sender As Object, e As DataGridBeginningEditEventArgs) Handles DG.BeginningEdit
        MyRow = e.Row.GetIndex
    End Sub

    Private Sub DG_CurrentCellChanged(sender As Object, e As EventArgs) Handles DG.CurrentCellChanged
        If MyRow < 0 Then Return
        DG.Items(MyRow)("Remaining") = bm.Round(DG.Items(MyRow)("Total")) - bm.Round(DG.Items(MyRow)("Discount")) - bm.Round(DG.Items(MyRow)("Value"))
    End Sub 

    Private Sub Save()
        Dim str As String = ""
        For i As Integer = 0 To DG.Items.Count - 1
            str &= " update " & TableName & " set Value=" & bm.Round(DG.Items(i)("Value")) & " where Line=" & DG.Items(i)("Line") & vbCrLf
        Next
        str &= " update " & TableName & " set Remaining=Total-Discount-Value where InvoiceNo=" & (txtID.Text)
        bm.ExecuteNonQuery(str, False)
    End Sub

End Class
