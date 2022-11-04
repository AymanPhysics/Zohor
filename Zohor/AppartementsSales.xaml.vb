Imports System.Data

Public Class AppartementsSales

    Public TableName As String = "AppartementsSales"
    Public SubId As String = "Id"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    WithEvents G As New MyGrid
    Public WithImage As Boolean = False
    Public ReLoadMenue As Boolean = False

    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {}, {})
        LoadResource()
        bm.FillCombo("FinishingTypes", FinishingTypeId, "", , True)
        
        bm.Fields = {SubId, "DayDate", "Price", "Finishing", "Transport", "ElectricityAndWater", "OtherCosts", "Total", "Payed", "Payed2", "Remaining", "CustName", "Floor", "Area", "BuildingName", "Sample", "UnitNo", "PriceBefore", "Tel", "Mobile", "Email", "FinishingTypeId"}
        bm.control = {txtID, DayDate, Price, Finishing, Transport, ElectricityAndWater, OtherCosts, Total, Payed, Payed2, Remaining, CustName, Floor, Area, BuildingName, Sample, UnitNo, PriceBefore, Tel, Mobile, Email, FinishingTypeId}
        bm.KeyFields = {SubId}

        LoadWFH()
        bm.Table_Name = TableName
        txtID_Leave(Nothing, Nothing)
    End Sub



    Structure GC
        Shared Id As String = "Id"
        Shared Value As String = "Value"
        Shared DueDate As String = "DueDate"
        Shared Notes As String = "Notes"
        Shared IsPayed As String = "IsPayed"
        Shared PayedDate As String = "PayedDate"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Id, "م")
        G.Columns.Add(GC.Value, "قيمة القسط")
        G.Columns.Add(GC.DueDate, "تاريخ الاستحقاق")
        G.Columns.Add(GC.Notes, "ملاحظات")
        G.Columns.Add(GC.IsPayed, "تم السداد")
        G.Columns.Add(GC.PayedDate, "تاريخ السداد")


        G.Columns(GC.Id).FillWeight = 60
        G.Columns(GC.Value).FillWeight = 100
        G.Columns(GC.DueDate).FillWeight = 100
        G.Columns(GC.Notes).FillWeight = 300
        G.Columns(GC.IsPayed).FillWeight = 100
        G.Columns(GC.PayedDate).FillWeight = 100

        G.Columns(GC.Id).ReadOnly = True

        G.Columns(GC.IsPayed).Visible = False
        G.Columns(GC.PayedDate).Visible = False

        G.AllowUserToDeleteRows = True
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.RowsRemoved, AddressOf G_RowsRemoved
        AddHandler G.EditingControlShowing, AddressOf G_EditingControlShowing
        AddHandler G.CellEndEdit, AddressOf G_CellEndEdit
    End Sub


    Sub FillControls()
        bm.FillControls(Me)
        Dim dt As DataTable = bm.ExecuteAdapter("select * from AppartementInstallments where AppartementId='" & txtID.Text & "'")
        For index = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(index).Cells(GC.Id).Value = dt.Rows(index)(GC.Id.ToString)
            G.Rows(index).Cells(GC.Value).Value = dt.Rows(index)(GC.Value.ToString)
            Try
                G.Rows(index).Cells(GC.DueDate).Value = dt.Rows(index)(GC.DueDate.ToString).ToString.Substring(0, 10)
            Catch ex As Exception
            End Try

            G.Rows(index).Cells(GC.Notes).Value = dt.Rows(index)(GC.Notes.ToString)
        Next
        G_RowsRemoved(Nothing, Nothing)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CustName.Text.Trim = "" Then
            Return
        End If

        Try
            For index = 0 To G.Rows.Count - 2
                If Val(G.Rows(index).Cells(GC.Value).Value) > 0 AndAlso Not IsDate(G.Rows(index).Cells(GC.DueDate).Value) Then
                    bm.ShowMSG("برجاء تحديد التاريخ بالسطر رقم " & (index + 1).ToString)
                    G.CurrentCell = G.Rows(index).Cells(GC.DueDate)
                    Return
                End If
            Next
        Catch
        End Try

        If Val(Remaining.Text) <> Val(GridTotal.Text) Then
            bm.ShowMSG("إجمالى الأقساط لا يساوى المتبقى")
            Return
        End If


        Area.Text = Val(Area.Text)
        G.EndEdit()
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If Not bm.SaveGrid(G, "AppartementInstallments", New String() {"AppartementId"}, New String() {txtID.Text}, New String() {"Id", "Value", "DueDate", "Notes"}, New String() {GC.Id, GC.Value, GC.DueDate, GC.Notes}, New VariantType() {VariantType.String, VariantType.Decimal, VariantType.Date, VariantType.String}, New String() {GC.Id}) Then Return

        CType(Parent, Window).Close()
    End Sub

    Sub ClearControls()
        Try
            txtID.Clear()
            bm.ClearControls()
            G.Rows.Clear()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            CType(Parent, Window).Close()
        End If
    End Sub

    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True
        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, Area.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID2_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Price.KeyDown, Finishing.KeyDown, Transport.KeyDown, ElectricityAndWater.KeyDown, OtherCosts.KeyDown, Total.KeyDown, Payed.KeyDown, Payed2.KeyDown, Remaining.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")

    End Sub

    Private Sub GridKeyDown(sender As Object, e As Forms.KeyEventArgs)
        e.Handled = True

    End Sub

    Dim WithEvents d As New Forms.DateTimePicker
    Private Sub G_EditingControlShowing(sender As Object, e As Forms.DataGridViewEditingControlShowingEventArgs)
        e.Control.Controls.Clear()
        If G.CurrentCell.ColumnIndex = G.Columns(GC.DueDate).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.PayedDate).Index Then
            d = New Forms.DateTimePicker
            e.Control.Controls.Add(d)
            d.Width = G.CurrentCell.OwningColumn.Width
            d.Location = New System.Drawing.Point(-2, -2)
            'AddHandler d.Validated, AddressOf d_ValueChanged
            AddHandler d.LostFocus, AddressOf d_ValueChanged
            Try
                d.Value = G.CurrentCell.Value
            Catch
                d.Value = Now.Date
            End Try
        End If

    End Sub

    Private Sub d_ValueChanged(sender As Object, e As EventArgs)
        Try
            G.CurrentCell.Value = d.Text.Substring(0, 10)
        Catch ex As Exception
        End Try
    End Sub


    Dim lloopp As Boolean = False
    Private Sub G_CellEndEdit(sender As Object, e As Forms.DataGridViewCellEventArgs)
        G_RowsRemoved(Nothing, Nothing)
    End Sub

    Sub Calc() Handles Price.TextChanged, Finishing.TextChanged, Transport.TextChanged, ElectricityAndWater.TextChanged, OtherCosts.TextChanged, Payed.TextChanged, Payed2.TextChanged

        Total.Text = Val(Price.Text) + Val(Finishing.Text) + Val(Transport.Text) + Val(ElectricityAndWater.Text) + Val(OtherCosts.Text)
        Remaining.Text = Val(Total.Text) - Val(Payed.Text) - Val(Payed2.Text)


    End Sub

    Private Sub G_RowsRemoved(sender As Object, e As Forms.DataGridViewRowsRemovedEventArgs)
        If lloopp Then Return
        lloopp = True
        Try
            For index = 0 To G.Rows.Count - 2
                G.Rows(index).Cells(GC.Id).Value = index + 1
            Next
        Catch
        End Try

        Dim ss As Decimal = 0
        Try
            For index = 0 To G.Rows.Count - 2
                ss += Val(G.Rows(index).Cells(GC.Value).Value)
            Next
        Catch
        End Try
        GridTotal.Text = ss

        If Not G.CurrentCell Is Nothing AndAlso G.CurrentCell.ColumnIndex = G.Columns(GC.Value).Index AndAlso Not G.CurrentCell.Value Is Nothing Then
            Try
                G.CurrentCell.Value = Val(G.CurrentCell.Value)
            Catch
                G.CurrentCell.Value = ""
            End Try
        End If

        If Not G.CurrentCell Is Nothing AndAlso G.CurrentCell.ColumnIndex = G.Columns(GC.DueDate).Index AndAlso Not G.CurrentCell.Value Is Nothing Then
            Try
                Dim dd As DateTime = DateTime.Parse(G.CurrentCell.Value)
            Catch
                G.CurrentCell.Value = ""
            End Try
        End If
        lloopp = False

    End Sub


End Class
