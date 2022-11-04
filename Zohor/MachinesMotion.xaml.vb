Imports System.Data

Public Class MachinesMotion


    Dim dt As New DataTable
    Dim dv As New DataView
    Dim bm As New BasicMethods
    Dim MyTextBoxes() As TextBox = {}

    Dim m As MainWindow = Application.Current.MainWindow
    Public WithImage As Boolean = False
    Public ReLoadMenue As Boolean = False
    Public Flag As Integer = 0
    WithEvents G As New MyGrid

    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        If bm.TestIsLoaded(Me) Then Return
        LoadWFH()
        DayDate.SelectedDate = bm.MyGetDate().Date
        bm.FillCombo("Machines", MachineId, "")

    End Sub

    Structure GC
        Shared MachinesOutcomeTypeId As String = "MachinesOutcomeTypeId"
        Shared MachinesOutcomeTypeName As String = "MachinesOutcomeTypeName"
        Shared Qty As String = "Qty"
        Shared Price As String = "Price"
        Shared Value As String = "Value"
        Shared Notes As String = "Notes"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.MachinesOutcomeTypeId, "كود البند")
        G.Columns.Add(GC.MachinesOutcomeTypeName, "اسم البند")
        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.Price, "السعر")
        G.Columns.Add(GC.Value, "القيمة")
        G.Columns.Add(GC.Notes, "ملاحظات")



        G.Columns(GC.MachinesOutcomeTypeName).FillWeight = 300
        G.Columns(GC.Notes).FillWeight = 300

        G.Columns(GC.MachinesOutcomeTypeName).ReadOnly = True
        G.Columns(GC.Value).ReadOnly = True


        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.MachinesOutcomeTypeId).Value = Nothing
        G.Rows(i).Cells(GC.MachinesOutcomeTypeName).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.Price).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.Notes).Value = Nothing
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub FillData()
        If DayDate.SelectedDate Is Nothing OrElse MachineId.SelectedValue Is Nothing OrElse MachineId.SelectedIndex < 1 Then
            Value.Clear()
            G.Rows.Clear()
            Return
        End If

        G.Enabled = False
        Value.Text = Val(bm.ExecuteScalar("select Value from MachinesMotion where MachineId=" & Val(MachineId.SelectedValue.ToString) & " and DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "'"))
        
        Dim dt As DataTable = bm.ExecuteAdapter("select *,dbo.GetMachinesOutcomeTypeName(MachinesOutcomeTypeId)MachinesOutcomeTypeName from MachinesOutcomeMotion where DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "' and MachineId='" & Val(MachineId.SelectedValue.ToString) & "'")

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.MachinesOutcomeTypeId).Value = dt.Rows(i)("MachinesOutcomeTypeId").ToString
            G.Rows(i).Cells(GC.MachinesOutcomeTypeName).Value = dt.Rows(i)("MachinesOutcomeTypeName").ToString
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
        Next
        G.Enabled = True
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        If DayDate.SelectedDate Is Nothing OrElse MachineId.SelectedValue Is Nothing OrElse MachineId.SelectedIndex < 1 Then Return
        G.EndEdit()
        Try
            G.CurrentCell = G.CurrentRow.Cells(GC.MachinesOutcomeTypeName)
        Catch ex As Exception
        End Try


        Dim str As String = "delete MachinesMotion where DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "' insert MachinesMotion(DayDate,MachineId,Value,UserName,MyGetDate) values " & "('" & bm.ToStrDate(DayDate.SelectedDate) & "','" & Val(MachineId.SelectedValue.ToString) & "','" & Val(Value.Text) & "','" & Md.UserName & "',GetDate())"

        If bm.ExecuteNonQuery(str) Then

            If Not bm.SaveGrid(G, "MachinesOutcomeMotion", New String() {"DayDate", "MachineId"}, New String() {bm.ToStrDate(DayDate.SelectedDate), Val(MachineId.SelectedValue)}, New String() {"MachinesOutcomeTypeId ", "Qty", "Price", "Value", "Notes"}, New String() {GC.MachinesOutcomeTypeId, GC.Qty, GC.Price, GC.Value, GC.Notes}, New VariantType() {VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.String}, New String() {GC.MachinesOutcomeTypeId}) Then Return

            bm.ShowMSG("تم الحفظ بنجاح")
            DayDate_SelectedDateChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub DayDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DayDate.SelectedDateChanged
        FillData()
    End Sub

    Private Sub GridCalcRow(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(e.ColumnIndex).Name = GC.MachinesOutcomeTypeId Then
                bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.MachinesOutcomeTypeId), G.Rows(e.RowIndex).Cells(GC.MachinesOutcomeTypeName), "Select Name from MachinesOutcomeTypes where Id='" & Val(G.Rows(e.RowIndex).Cells(GC.MachinesOutcomeTypeId).Value) & "'")
            ElseIf G.Columns(e.ColumnIndex).Name = GC.Qty OrElse G.Columns(e.ColumnIndex).Name = GC.Price Then
                G.Rows(e.RowIndex).Cells(GC.Qty).Value = Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value)
                G.Rows(e.RowIndex).Cells(GC.Price).Value = Val(G.Rows(e.RowIndex).Cells(GC.Price).Value)
                G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.Price).Value)
            End If
        Catch

        End Try

    End Sub

    Private Sub GridKeyDown(sender As Object, e As Forms.KeyEventArgs)
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.MachinesOutcomeTypeId).Index Then
                If bm.ShowHelpGrid("MachinesOutcomeTypes", G.CurrentRow.Cells(GC.MachinesOutcomeTypeId), G.CurrentRow.Cells(GC.MachinesOutcomeTypeName), e, "select Cast(Id as nvarchar(100))Id,Name from MachinesOutcomeTypes") Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub MachineId_LostFocus(sender As Object, e As RoutedEventArgs) Handles MachineId.LostFocus
        FillData()
    End Sub
End Class
