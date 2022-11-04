Imports System.Data

Public Class PlanSub

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    WithEvents G As New MyGrid

    Public CurrentDay As DateTime
    Public Alone As Boolean = False
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        G.EndEdit()
        If Not bm.SaveGrid(G, "PlanSub", New String() {"DayDate"}, New String() {bm.ToStrDate(DayDate.SelectedDate)}, New String() {"Flag", "Id", "Name", "Id2", "Name2", "FromHour", "FromHourIndexId", "ToHour", "ToHourIndexId"}, New String() {GC.Flag, GC.Id, GC.Name, GC.Id2, GC.Name2, GC.FromHour, GC.FromHourIndexId, GC.ToHour, GC.ToHourIndexId}, New VariantType() {VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.String, VariantType.Decimal, VariantType.Integer, VariantType.Decimal, VariantType.Integer}, New String() {GC.Id}) Then Return

        If Alone Then
            bm.ShowMSG("Saved Successfuly")
            'DayDate.SelectedDate = Nothing
        Else
            CType(Parent, Window).Close()
        End If

    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadWFH()
        LoadResource()
        DayDate.SelectedDate = CurrentDay
        DayDate.IsEnabled = Alone
    End Sub

    Dim lop As Boolean = False

    Private Sub LoadResource()

    End Sub


    Structure GC
        Shared Flag As String = "Flag"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Id2 As String = "Id2"
        Shared Name2 As String = "Name2"
        Shared FromHour As String = "FromHour"
        Shared FromHourIndexId As String = "FromHourIndexId"
        Shared ToHour As String = "ToHour"
        Shared ToHourIndexId As String = "ToHourIndexId"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        Dim GCFlag As New Forms.DataGridViewComboBoxColumn
        GCFlag.HeaderText = "النوع"
        GCFlag.Name = GC.Flag
        bm.FillCombo("select Id,Name from PlanSubFlags", GCFlag)
        G.Columns.Add(GCFlag)

        G.Columns.Add(GC.Id, "كود الطبيب")
        G.Columns.Add(GC.Name, "اسم الطبيب")
        G.Columns.Add(GC.Id2, "كود الطبيب")
        G.Columns.Add(GC.Name2, "اسم الطبيب")
        G.Columns.Add(GC.FromHour, "من ساعة")

        Dim GCFromHourIndexId As New Forms.DataGridViewComboBoxColumn
        GCFromHourIndexId.HeaderText = "ص/م"
        GCFromHourIndexId.Name = GC.FromHourIndexId
        bm.FillCombo("select Id,Name from HourIndex", GCFromHourIndexId)
        G.Columns.Add(GCFromHourIndexId)

        G.Columns.Add(GC.ToHour, "إلى ساعة")

        Dim GCToHourIndexId As New Forms.DataGridViewComboBoxColumn
        GCToHourIndexId.HeaderText = "ص/م"
        GCToHourIndexId.Name = GC.ToHourIndexId
        bm.FillCombo("select Id,Name from HourIndex", GCToHourIndexId)
        G.Columns.Add(GCToHourIndexId)

        G.Columns(GC.Flag).FillWeight = 150
        G.Columns(GC.Name).FillWeight = 300
        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Name2).FillWeight = 300
        G.Columns(GC.Name2).ReadOnly = True

        G.AllowUserToDeleteRows = True

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        If G.Columns(e.ColumnIndex).Name = GC.Id Then
            AddItem(G.Rows(e.RowIndex).Cells(GC.Id), G.Rows(e.RowIndex).Cells(GC.Name), G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
        ElseIf G.Columns(e.ColumnIndex).Name = GC.Id2 Then
            AddItem(G.Rows(e.RowIndex).Cells(GC.Id2), G.Rows(e.RowIndex).Cells(GC.Name2), G.Rows(e.RowIndex).Cells(GC.Id2).Value, e.RowIndex, 0)
        End If
        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        CalcRow(e.RowIndex)
    End Sub

    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                Dim i As Integer = G.Rows.Add()
                G.CurrentCell = G.Rows(i).Cells(G.CurrentCell.ColumnIndex)
            End If

            If G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Name).Index Then
                If bm.ShowHelpGrid("Doctors", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "select cast(Id as varchar(100)) Id,Name from Employees where Doctor=1") Then
                    G.CurrentCell = G.CurrentRow.Cells(GC.Id2)
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.Id2).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Name2).Index Then
                If bm.ShowHelpGrid("Doctors", G.CurrentRow.Cells(GC.Id2), G.CurrentRow.Cells(GC.Name2), e, "select cast(Id as varchar(100)) Id,Name from Employees where Doctor=1") Then
                    G.CurrentCell = G.CurrentRow.Cells(GC.FromHour)
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id2).Index, G.CurrentCell.RowIndex))
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Sub AddItem(MyCellId As Forms.DataGridViewCell, MyCellName As Forms.DataGridViewCell, ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1)
        Try
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                i = G.Rows.Add()
Br:
            End If

            Dim dt As DataTable = bm.ExecuteAdapter("Select Id,Name From Employees where Doctor=1 and Id='" & Id & "'")
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not MyCellId.Value Is Nothing Or MyCellId.Value <> "" Then bm.ShowMSG("هذا الطبيب غير موجود")
                MyCellName.Value = Nothing
                ClearRow(i)
                Return
            End If
            MyCellId.Value = dr(0)(GC.Id)
            MyCellName.Value = dr(0)(GC.Name)
            
            If G.Rows(i).Cells(GC.Flag).Value Is Nothing Then G.Rows(i).Cells(GC.Flag).Value = "1"
            If G.Rows(i).Cells(GC.FromHourIndexId).Value Is Nothing Then G.Rows(i).Cells(GC.FromHourIndexId).Value = "0"
            If G.Rows(i).Cells(GC.ToHourIndexId).Value Is Nothing Then G.Rows(i).Cells(GC.ToHourIndexId).Value = "0"

            CalcRow(i)
            If Move Then
                G.Focus()
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.FromHour)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
            If Exists Then
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.Name)
                G.CurrentCell = G.Rows(i).Cells(GC.FromHour)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
        Catch
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try
    End Sub

    Sub CalcRow(ByVal i As Integer)
        Try
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value.ToString = "" Then
                G.Rows(i).Cells(GC.Name).Value = Nothing
                ClearRow(i)
                Return
            ElseIf G.Rows(i).Cells(GC.Id2).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id2).Value.ToString = "" Then
                G.Rows(i).Cells(GC.Name2).Value = Nothing
                ClearRow(i)
                Return
            End If
        Catch ex As Exception
            ClearRow(i)
        End Try
    End Sub

    Sub ClearRow(ByVal i As Integer)
        Return
        G.Rows(i).Cells(GC.Flag).Value = "1"
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.Id2).Value = Nothing
        G.Rows(i).Cells(GC.Name2).Value = Nothing
        G.Rows(i).Cells(GC.FromHour).Value = Nothing
        G.Rows(i).Cells(GC.FromHourIndexId).Value = "0"
        G.Rows(i).Cells(GC.ToHour).Value = Nothing
        G.Rows(i).Cells(GC.ToHourIndexId).Value = "0"
    End Sub

    Private Sub btnItemsSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnItemsSearch.Click
        Try
            If G.CurrentRow Is Nothing Then G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Id)
            G.Focus()
            'G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Id)
            GridKeyDown(G, New System.Windows.Forms.KeyEventArgs(Forms.Keys.F1))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DayDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DayDate.SelectedDateChanged
        G.Rows.Clear()
        If DayDate.SelectedDate Is Nothing Then Return
        Dim dt As DataTable = bm.ExecuteAdapter("select * from PlanSub where DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "'")
        For index = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(index).Cells(GC.Flag).Value = dt.Rows(index)(GC.Flag.ToString).ToString
            G.Rows(index).Cells(GC.Id).Value = dt.Rows(index)(GC.Id.ToString)
            G.Rows(index).Cells(GC.Name).Value = dt.Rows(index)(GC.Name.ToString)
            G.Rows(index).Cells(GC.Id2).Value = dt.Rows(index)(GC.Id2.ToString)
            G.Rows(index).Cells(GC.Name2).Value = dt.Rows(index)(GC.Name2.ToString)
            G.Rows(index).Cells(GC.FromHour).Value = dt.Rows(index)(GC.FromHour.ToString)
            G.Rows(index).Cells(GC.FromHourIndexId).Value = dt.Rows(index)(GC.FromHourIndexId.ToString).ToString
            G.Rows(index).Cells(GC.ToHour).Value = dt.Rows(index)(GC.ToHour.ToString)
            G.Rows(index).Cells(GC.ToHourIndexId).Value = dt.Rows(index)(GC.ToHourIndexId.ToString).ToString
        Next

    End Sub
End Class