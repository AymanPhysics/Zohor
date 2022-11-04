Imports System.Data

Public Class OperationMotions
    Public TableName As String = "OperationMotions"
    Public TableDetailsName As String = "OperationMotionsOperationTypes"
    Public SubId As String = "InvoiceNo"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    WithEvents G As New MyGrid

    Dim IsLoaded As Boolean = False
    Public Sub OperationMotions_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        If IsLoaded OrElse bm.TestIsLoaded(Me) Then Return
        IsLoaded = True

        bm.TestSecurity(Me, {btnSaveWithoutPrint}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnSave})
        LoadResource()

        bm.FillCombo("Companies", CompanyId, "")

        bm.FillCombo("Rooms", RoomId, "")

        bm.FillCombo("Employees", EmpIdReservation, "")
        bm.FillCombo("Employees", EmpIdRemaining, "")
        bm.FillCombo("Employees", EmpIdCanceled, "")
        bm.FillCombo("Employees", EmpIdReturned, "")

        bm.FillCombo("Employees", DrId1, " where Doctor=1")
        bm.FillCombo("Employees", DrId2, " where Doctor=1")
        bm.FillCombo("Employees", DrId3, " where Doctor=1")
        bm.FillCombo("Employees", AnesthetistId, " where Doctor=1")

        bm.Fields = New String() {SubId, "DayDate", "Canceled", "RoomId", "CaseId", "DrId1", "DrId2", "DrId3", "AnesthetistId", "RoomValue", "RoomValue2", "Dr1Value", "Dr2Value", "Dr3Value", "AnesthetistValue", "CsValue", "MedicalValue", "CoValue", "ConsumablesValue", "Value", "Notes", "SerialId", "SerialId2", "Payed", "Done", "Remaining", "RemainingDate", "EmpIdReservation", "EmpIdRemaining", "EmpIdCanceled", "CanceledDate", "Returned", "ReturnedDate", "EmpIdReturned", "RefereId", "CaseInvoiceNo", "NoOfDays", "Perc0", "Perc", "Total", "CompanyId", "Dr1Perc", "Dr2Perc", "Dr3Perc", "AnesthetistPerc"}
        bm.control = New Control() {txtID, DayDate, Canceled, RoomId, CaseId, DrId1, DrId2, DrId3, AnesthetistId, RoomValue, RoomValue2, Dr1Value, Dr2Value, Dr3Value, AnesthetistValue, CsValue, MedicalValue, CoValue, ConsumablesValue, Value, Notes, SerialId, SerialId2, Payed, Done, Remaining, RemainingDate, EmpIdReservation, EmpIdRemaining, EmpIdCanceled, CanceledDate, Returned, ReturnedDate, EmpIdReturned, RefereId, CaseInvoiceNo, NoOfDays, Perc0, Perc, Total, CompanyId, Dr1Perc, Dr2Perc, Dr3Perc, AnesthetistPerc}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName

        LoadWFH()
        btnNew_Click(sender, e)
        Done_Unchecked(Nothing, Nothing)
        Canceled_Unchecked(Nothing, Nothing)
        Returned_Unchecked(Nothing, Nothing)

        Done.Visibility = Visibility.Hidden
        Returned.Visibility = Visibility.Hidden
        lblPayed.Visibility = Visibility.Hidden
        Payed.Visibility = Visibility.Hidden
        lblSerialId.Visibility = Visibility.Hidden
        SerialId.Visibility = Visibility.Hidden
        lblRemaining.Visibility = Visibility.Hidden
        Remaining.Visibility = Visibility.Hidden

        lblNoOfDays.Visibility = Visibility.Hidden
        NoOfDays.Visibility = Visibility.Hidden

        If Not Md.MyProjectType = ProjectType.X Then
            '    btnPackage.Visibility = Visibility.Hidden
            '    lblRoomValue2.Visibility = Visibility.Hidden
            '    RoomValue2.Visibility = Visibility.Hidden
            '    lblCsValue.Visibility = Visibility.Hidden
            '    CsValue.Visibility = Visibility.Hidden
            '    lblMedicalValue.Visibility = Visibility.Hidden
            '    MedicalValue.Visibility = Visibility.Hidden
            '    lblCoValue.Visibility = Visibility.Hidden
            '    CoValue.Visibility = Visibility.Hidden

            lblPayed.Content = "التأمين"
            lblPayed.Visibility = Visibility.Visible
            Payed.Visibility = Visibility.Visible
        End If


        If Md.MyProjectType = ProjectType.Zohor AndAlso Md.Receptionist Then
            Dr1Value.Visibility = Visibility.Hidden
            Dr2Value.Visibility = Visibility.Hidden
            Dr3Value.Visibility = Visibility.Hidden
            AnesthetistValue.Visibility = Visibility.Hidden
            lblRoomValue.Visibility = Visibility.Hidden
            RoomValue.Visibility = Visibility.Hidden
            lblConsumablesValue.Visibility = Visibility.Hidden
            ConsumablesValue.Visibility = Visibility.Hidden
        End If
        If Md.MyProjectType = ProjectType.Zohor Then
            lblRefereId.Visibility = Visibility.Hidden
            RefereId.Visibility = Visibility.Hidden
            RefereName.Visibility = Visibility.Hidden
        End If
    End Sub

    Structure GC
        Shared OperationTypeId As String = "OperationTypeId"
        Shared OperationTypeName As String = "OperationTypeName"
        Shared Perc As String = "Perc"
    End Structure

    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.OperationTypeId, "كود العملية")
        G.Columns.Add(GC.OperationTypeName, "اسم العملية")
        G.Columns.Add(GC.Perc, "النسبة")

        G.Columns(GC.OperationTypeName).FillWeight = 300

        G.Columns(GC.OperationTypeName).ReadOnly = True

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.RowsAdded, AddressOf GridRowsAdded

    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            Dim i As Integer = e.RowIndex

            If G.Rows(i).Cells(GC.OperationTypeId).Value Is Nothing OrElse G.Rows(i).Cells(GC.OperationTypeId).Value.ToString = "" Then
                ClearRow(i)
                Return
            End If

            For x As Integer = 0 To G.Rows.Count - 1
                If x <> i AndAlso Not G.Rows(x).Cells(GC.OperationTypeId).Value Is Nothing AndAlso G.Rows(x).Cells(GC.OperationTypeId).Value.ToString = G.Rows(i).Cells(GC.OperationTypeId).Value.ToString Then
                    bm.ShowMSG("تم تكرار هذه العملية بالسطر رقم " + (x + 1).ToString)
                    ClearRow(i)
                    Return
                End If
            Next


            If G.Columns(e.ColumnIndex).Name = GC.OperationTypeId Then
                dt = bm.ExecuteAdapter("select Name from OperationTypes where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.OperationTypeId).Value))
                If dt.Rows.Count > 0 Then
                    G.Rows(i).Cells(GC.OperationTypeName).Value = dt.Rows(0)(0)
                Else
                    ClearRow(i)
                    Return
                End If
            End If
            If Not sender Is Nothing Then RoomId_LostFocus(Nothing, Nothing)
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        Catch ex As Exception
        End Try
    End Sub


    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.OperationTypeId).Value = Nothing
        G.Rows(i).Cells(GC.OperationTypeName).Value = Nothing
        G.Rows(i).Cells(GC.Perc).Value = Nothing
    End Sub


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.OperationTypeId).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.OperationTypeName).Index Then
                Dim str As String = "select cast(Id as varchar(100)) Id,Name from OperationTypes where IsStopped=0"
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.OperationTypeId), G.CurrentRow.Cells(GC.OperationTypeName), e, str) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.OperationTypeId).Index, G.CurrentCell.RowIndex))
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Perc)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim lop As Boolean = False
    Sub FillControls()
        bm.FillControls(Me)
        lop = True
        bm.FillControls(Me)
        FillGrid()
        CaseID_LostFocus(Nothing, Nothing)
        RefereId_LostFocus(Nothing, Nothing)

        If Canceled.IsChecked Then
            Canceled_Checked(Nothing, Nothing)
        Else
            Canceled_Unchecked(Nothing, Nothing)
        End If

        If Returned.IsChecked Then
            Returned_Checked(Nothing, Nothing)
        Else
            Returned_Unchecked(Nothing, Nothing)
        End If

        'If DayDate.SelectedDate = bm.MyGetDate() Then
        '    Returned.IsEnabled = False
        'Else
        '    Canceled.IsEnabled = False
        'End If

        lop = False
        UndoNewId()
        DayDate.Focus()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim lop2 As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveWithoutPrint.Click

        G.EndEdit()

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If
        If Val(CaseId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المريض")
            CaseId.Focus()
            Return
        End If
        
        For i As Integer = 0 To G.Rows.Count - 1
            If Val(G.Rows(i).Cells(GC.OperationTypeId).Value) > 0 Then Exit For
            If i = G.Rows.Count - 1 Then
                bm.ShowMSG("برجاء تحديد العملية")
                G.Focus()
                Return
            End If
        Next

        If Val(Dr1Value.Text) <> 0 AndAlso DrId1.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد الطبيب 1")
            DrId1.Focus()
            Return
        End If
        If Val(Dr2Value.Text) <> 0 AndAlso DrId2.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد الطبيب 2")
            DrId2.Focus()
            Return
        End If
        If Val(Dr3Value.Text) <> 0 AndAlso DrId3.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد الطبيب 3")
            DrId3.Focus()
            Return
        End If
        If Val(AnesthetistValue.Text) <> 0 AndAlso AnesthetistId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد الطبيب")
            AnesthetistId.Focus()
            Return
        End If
        'If Val(CsValue.Text) <> 0 AndAlso CsId.SelectedIndex < 1 Then
        '    bm.ShowMSG("برجاء تحديد الممرض")
        '    CsId.Focus()
        '    Return
        'End If

        lop2 = True
        RoomValue.Text = Val(RoomValue.Text)
        RoomValue2.Text = Val(RoomValue2.Text)
        Dr1Value.Text = Val(Dr1Value.Text)
        Dr2Value.Text = Val(Dr2Value.Text)
        Dr3Value.Text = Val(Dr3Value.Text)
        AnesthetistValue.Text = Val(AnesthetistValue.Text)
        
        CsValue.Text = Val(CsValue.Text)
        MedicalValue.Text = Val(MedicalValue.Text)
        CoValue.Text = Val(CoValue.Text)
        ConsumablesValue.Text = Val(ConsumablesValue.Text)
        Value.Text = Val(Value.Text)
        Payed.Text = Val(Payed.Text)
        Remaining.Text = Val(Remaining.Text)
        lop2 = False

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If txtID.Text.Trim = "" Then
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If txtID.Text = "" Then txtID.Text = "1"
            LastEntry.Text = txtID.Text
            State = BasicMethods.SaveState.Insert
        End If


        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                txtID.Text = ""
                LastEntry.Text = ""
                SerialId.Text = ""
                LastSerialId.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G, TableDetailsName, New String() {SubId}, New String() {txtID.Text}, New String() {"OperationTypeId", "Perc"}, New String() {GC.OperationTypeId, GC.Perc}, New VariantType() {VariantType.Integer, VariantType.Decimal}, New String() {GC.OperationTypeId}) Then Return

        If Val(SerialId.Text) = 0 AndAlso Val(Payed.Text) > 0 Then
            LastSerialId.Text = SerialId.Text
        End If

        If SerialId2.IsVisible AndAlso Val(SerialId2.Text) = 0 AndAlso Val(Remaining.Text) > 0 Then
            LastSerialId.Text = SerialId2.Text
        End If

        If Not Canceled.IsChecked AndAlso Not Returned.IsChecked AndAlso sender Is btnSave Then ShowRPT()
        btnNew_Click(sender, e)
        
    End Sub

    Private Sub ShowRPT()
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Flag", "@Id", "Header"}
        rpt.paravalue = New String() {1, txtID.Text, CType(Parent, Page).Title}
        rpt.Rpt = "ServicesONE.rpt"
        rpt.Print()
    End Sub


    Sub NewId()
        txtID.Clear()
        'txtID.IsEnabled = False

        DayDate.IsEnabled = Md.Manager
        
        'DrId.IsEnabled = True
        CaseId.IsEnabled = True

        'OperationTypeId.IsEnabled = True
        'OperationTypeId2.IsEnabled = True
        'OperationTypeId3.IsEnabled = True
        
        'DrValue.IsEnabled = True
        CsValue.IsEnabled = True
        MedicalValue.IsEnabled = True
        CoValue.IsEnabled = True
        ConsumablesValue.IsEnabled = True
        Payed.IsEnabled = True

        btnSave.IsEnabled = True
        btnSaveWithoutPrint.IsEnabled = True
        btnDelete.IsEnabled = True

    End Sub

    Sub UndoNewId()
        txtID.IsEnabled = True

        DayDate.IsEnabled = Md.Manager
        
        CaseId.IsEnabled = Md.Manager
        'DrId.IsEnabled = Md.Manager
        'CsId.IsEnabled = Md.Manager

        'OperationTypeId.IsEnabled = Md.Manager
        'OperationTypeId2.IsEnabled = Md.Manager
        'OperationTypeId3.IsEnabled = Md.Manager


        'DrValue.IsEnabled = Md.Manager
        CsValue.IsEnabled = Md.Manager
        MedicalValue.IsEnabled = Md.Manager
        CoValue.IsEnabled = Md.Manager
        ConsumablesValue.IsEnabled = Md.Manager
        Payed.IsEnabled = Md.Manager

        If Val(CaseInvoiceNo.Text) = 0 Then
            btnSave.IsEnabled = True
            btnSaveWithoutPrint.IsEnabled = True
            btnDelete.IsEnabled = True
        Else
            btnSave.IsEnabled = False
            btnSaveWithoutPrint.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

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
        bm.ClearControls(False)

        Dr1Perc.Text = 100
        Dr2Perc.Text = 100
        Dr3Perc.Text = 100
        AnesthetistPerc.Text = 100

        Perc0.Text = Val(bm.ExecuteScalar("select top 1 CaseInvoicesPerc0 from Statics"))

        G.Rows.Clear()
        Canceled.IsEnabled = True
        DayDate.SelectedDate = bm.MyGetDate() 
        CaseID_LostFocus(Nothing, Nothing)
        RefereId_LostFocus(Nothing, Nothing)

        'txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        'If txtID.Text = "" Then txtID.Text = "1"
        EmpIdReservation.SelectedValue = Md.UserName
        NewId()
        If Not lv Then DayDate.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Public lv As Boolean = False

    Public Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, RoomId.KeyDown, CaseId.KeyDown, DrId1.KeyDown, DrId2.KeyDown, DrId3.KeyDown, AnesthetistId.KeyDown, EmpIdReservation.KeyDown, EmpIdRemaining.KeyDown, EmpIdCanceled.KeyDown, RefereId.KeyDown, CaseInvoiceNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown, Remaining.KeyDown, RoomValue.KeyDown, RoomValue2.KeyDown, Dr1Value.KeyDown, Dr2Value.KeyDown, Dr3Value.KeyDown, AnesthetistValue.KeyDown, CsValue.KeyDown, MedicalValue.KeyDown, CoValue.KeyDown, ConsumablesValue.KeyDown, Value.KeyDown, Payed.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

     

    Private Sub Canceled_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Checked
        Canceled.IsEnabled = Md.Manager
        Returned.IsEnabled = Md.Manager
        Returned.IsChecked = False
         
        CsValue.IsEnabled = False
        MedicalValue.IsEnabled = False
        CoValue.IsEnabled = False
        ConsumablesValue.IsEnabled = False

        Payed.IsEnabled = False
        Done.IsEnabled = False

        CanceledDate.Visibility = Visibility.Visible
        CanceledDate.IsEnabled = Md.Manager
        EmpIdCanceled.Visibility = Visibility.Visible
        If EmpIdCanceled.SelectedIndex <= 0 Then
            EmpIdCanceled.SelectedValue = Md.UserName
            CanceledDate.SelectedDate = bm.MyGetDate()
        End If
    End Sub


    Private Sub Returned_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Returned.Checked
        Canceled.IsEnabled = Md.Manager
        Returned.IsEnabled = Md.Manager
        Canceled.IsChecked = False
         
        CsValue.IsEnabled = False
        MedicalValue.IsEnabled = False
        CoValue.IsEnabled = False
        ConsumablesValue.IsEnabled = False

        Payed.IsEnabled = False
        Done.IsEnabled = False

        ReturnedDate.Visibility = Visibility.Visible
        ReturnedDate.IsEnabled = Md.Manager
        EmpIdReturned.Visibility = Visibility.Visible
        If EmpIdReturned.SelectedIndex <= 0 Then
            EmpIdReturned.SelectedValue = Md.UserName
            ReturnedDate.SelectedDate = bm.MyGetDate()
        End If
    End Sub

    Private Sub Canceled_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Unchecked

        If Not Returned.IsChecked Then
            Canceled.IsEnabled = True
            Returned.IsEnabled = True
             
            CsValue.IsEnabled = True
            MedicalValue.IsEnabled = True
            CoValue.IsEnabled = True
            ConsumablesValue.IsEnabled = True
             

            Payed.IsEnabled = True
            Done.IsEnabled = True
        End If

        CanceledDate.Visibility = Visibility.Hidden
        CanceledDate.SelectedDate = Nothing
        EmpIdCanceled.Visibility = Visibility.Hidden
        EmpIdCanceled.SelectedIndex = 0
    End Sub


    Private Sub Returned_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Returned.Unchecked

        If Not Canceled.IsChecked Then
            Returned.IsEnabled = True

            'DrValue.IsEnabled = True
            CsValue.IsEnabled = True
            MedicalValue.IsEnabled = True
            CoValue.IsEnabled = True
            ConsumablesValue.IsEnabled = True
             
            Payed.IsEnabled = True
            Done.IsEnabled = True
        End If

        ReturnedDate.Visibility = Visibility.Hidden
        ReturnedDate.SelectedDate = Nothing
        EmpIdReturned.Visibility = Visibility.Hidden
        EmpIdReturned.SelectedIndex = 0
    End Sub

    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp
        If bm.ShowHelpCases(CaseId, CaseName, e) Then
            CaseID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select " & Resources.Item("CboName") & " Name from Cases where Id=" & CaseId.Text.Trim())
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        CompanyId.SelectedIndex = 0
        Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile,CompanyId from Cases where Id=" & CaseId.Text.Trim())
        If dt.Rows.Count > 0 Then
            CompanyId.SelectedValue = Val(dt.Rows(0)("CompanyId").ToString)
            CaseId.ToolTip = Resources.Item("Id") & ": " & CaseId.Text & vbCrLf & Resources.Item("Name") & ": " & CaseName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
            CaseName.ToolTip = CaseId.ToolTip
        End If

        If Val(txtID.Text) = 0 Then
            dt = bm.ExecuteAdapter("select top 1 * from ReservationsOperations where CaseId='" & Val(CaseId.Text) & "' and Canceled=0 order by  DayDate desc")
            G.Rows.Clear()
            If dt.Rows.Count > 0 Then
                If Val(dt.Rows(0)("OperationType").ToString) > 0 Then
                    G.Rows.Add({dt.Rows(0)("OperationType").ToString, "-", 100})
                    GridCalcRow(Nothing, New Forms.DataGridViewCellEventArgs(G.Columns(GC.OperationTypeId).Index, 0))
                End If
                If Val(dt.Rows(0)("OperationType").ToString) > 0 Then
                    G.Rows.Add({dt.Rows(0)("OperationType2").ToString, "-", 0})
                    GridCalcRow(Nothing, New Forms.DataGridViewCellEventArgs(G.Columns(GC.OperationTypeId).Index, 1))
                End If
                If Val(dt.Rows(0)("OperationType").ToString) > 0 Then
                    G.Rows.Add({dt.Rows(0)("OperationType3").ToString, "-", 0})
                    GridCalcRow(Nothing, New Forms.DataGridViewCellEventArgs(G.Columns(GC.OperationTypeId).Index, 2))
                End If

                DrId1.SelectedValue = dt.Rows(0)("SurgeonId").ToString
                DrId2.SelectedValue = dt.Rows(0)("SurgeonId2").ToString
                DrId3.SelectedValue = dt.Rows(0)("SurgeonId3").ToString
                AnesthetistId.SelectedValue = dt.Rows(0)("AnesthetistId").ToString
            End If

            RoomId.SelectedValue = Val(bm.ExecuteScalar("select top 1 RoomId from RoomsData where CaseId=" & Val(CaseId.Text) & " and IsCurrent=1"))

            If Not sender Is Nothing Then RoomId_LostFocus(Nothing, Nothing)
        End If


    End Sub

    Private Sub RefereId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles RefereId.KeyUp
        If bm.ShowHelp("ExternalDoctors", RefereId, RefereName, e, "select cast(Id as varchar(100)) Id,Name from ExternalDoctors") Then
            RefereId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub RefereId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RefereId.LostFocus
        bm.LostFocus(RefereId, RefereName, "select Name from ExternalDoctors where Id=" & RefereId.Text.Trim())
    End Sub


    Sub FillDoctors()
        Try
            'Dim sd As String = DrId1.SelectedValue
            'Dim dt As DataTable = bm.ExecuteAdapter("select DepartmentId from ServiceTypes where ServiceGroupId=" & ServiceGroupId.SelectedValue.ToString)
            'Dim s As String = 0
            'If dt.Rows.Count > 0 Then
            '    s = Val(dt.Rows(0)("DepartmentId").ToString)
            'End If
            'bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Doctor='1' and (DepartmentId=" & s & " or " & s & "=0) and Stopped='0' union select 0 Id,'-' Name order by Name", DrId1)
            'If lop Then DrId1.SelectedValue = sd
        Catch ex As Exception
        End Try

    End Sub


    Private Sub LoadResource()
        btnSaveWithoutPrint.SetResourceReference(ContentProperty, "Save")
        btnSave.SetResourceReference(ContentProperty, "Print")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")


        Canceled.SetResourceReference(CheckBox.ContentProperty, "Cancel")
        Returned.SetResourceReference(CheckBox.ContentProperty, "Returned")
        Done.SetResourceReference(CheckBox.ContentProperty, "Done Remaining")

        lblId.SetResourceReference(ContentProperty, "Id")
        lblCaseId.SetResourceReference(ContentProperty, "CaseId")
        'lblCoValue.SetResourceReference(ContentProperty, "CoValue")
        'lblCsValue.SetResourceReference(ContentProperty, "CsValue")
        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblDrValue.SetResourceReference(ContentProperty, "DrValue")
        lblLastEntry.SetResourceReference(ContentProperty, "LastEntry")
        lblNotes.SetResourceReference(ContentProperty, "Notes")

        lblValue.SetResourceReference(ContentProperty, "Value")
        lblPayed.SetResourceReference(ContentProperty, "Payed")
        lblRemaining.SetResourceReference(ContentProperty, "Remaining")
        lblSerialId.SetResourceReference(ContentProperty, "Serial")
        lblSerialId2.SetResourceReference(ContentProperty, "Serial")
        lblLastSerialId.SetResourceReference(ContentProperty, "LastSerial")

    End Sub

    Private Sub Value_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Value.TextChanged, Payed.TextChanged, Perc0.TextChanged
        If lop2 Then Return
        Remaining.Text = Val(Value.Text) - Val(Payed.Text)

        Perc.Text = bm.Round(Val(Perc0.Text) / 100 * (Val(Value.Text) - Val(ConsumablesValue.Text)))
        Total.Text = Val(Value.Text) + Val(Perc.Text)
    End Sub



    Private Sub Done_Checked(sender As Object, e As RoutedEventArgs) Handles Done.Checked
        Done.IsEnabled = Md.Manager

        lblSerialId2.Visibility = Visibility.Visible
        SerialId2.Visibility = Visibility.Visible

        RemainingDate.Visibility = Visibility.Visible
        RemainingDate.SelectedDate = bm.MyGetDate()
        RemainingDate.IsEnabled = Md.Manager
        EmpIdRemaining.Visibility = Visibility.Visible
        EmpIdRemaining.SelectedValue = Md.UserName
    End Sub
    Private Sub Done_Unchecked(sender As Object, e As RoutedEventArgs) Handles Done.Unchecked
        Done.IsEnabled = True

        lblSerialId2.Visibility = Visibility.Hidden
        SerialId2.Visibility = Visibility.Hidden

        RemainingDate.Visibility = Visibility.Hidden
        RemainingDate.SelectedDate = Nothing
        EmpIdRemaining.Visibility = Visibility.Hidden
        EmpIdRemaining.SelectedIndex = 0
    End Sub


    Private Sub RoomId_LostFocus(sender As Object, e As RoutedEventArgs) Handles RoomId.LostFocus, DrId1.LostFocus
        RoomValue.Clear()
        RoomValue2.Clear()
        CsValue.Clear()
        MedicalValue.Clear()
        Dr1Value.Clear()
        Dr2Value.Clear()
        Dr3Value.Clear()
        AnesthetistValue.Clear()
        'CsValue.Clear()
        'MedicalValue.Clear()
        CoValue.Clear()
        ConsumablesValue.Clear()
        Payed.Clear()
        NoOfDays.Clear()
        For i As Integer = 0 To G.Rows.Count - 1
            Dim OperationTypeId As Integer = Val(G.Rows(i).Cells(GC.OperationTypeId).Value)
            Dim Perc As Integer = Val(G.Rows(i).Cells(GC.Perc).Value)
            If OperationTypeId = 0 Then Continue For

            Dim OperationDescriptionId As Integer = Val(bm.ExecuteScalar("select OperationDescriptionId from OperationTypes where Id=" & OperationTypeId))

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.Zohor Then

                RoomValue.Text = Val(RoomValue.Text) + Val(bm.ExecuteScalar("select isnull(T.Price,0) from OperationTypeRooms T where RoomTypeId=(select R.RoomTypeId from Rooms R where Id=" & Val(RoomId.SelectedValue) & ") and OperationTypeId=" & OperationTypeId)) * Perc / 100
                RoomValue2.Text = Val(RoomValue2.Text) + Val(bm.ExecuteScalar("select isnull(Living,0) from RoomTypes where Id=(select R.RoomTypeId from Rooms R where Id=" & Val(RoomId.SelectedValue) & ")")) * Perc / 100
                CsValue.Text = Val(CsValue.Text) + Val(bm.ExecuteScalar("select isnull(Care,0) from RoomTypes where Id=(select R.RoomTypeId from Rooms R where Id=" & Val(RoomId.SelectedValue) & ")")) * Perc / 100
                MedicalValue.Text = Val(MedicalValue.Text) + Val(bm.ExecuteScalar("select isnull(supervision,0) from RoomTypes where Id=(select R.RoomTypeId from Rooms R where Id=" & Val(RoomId.SelectedValue) & ")")) * Perc / 100

                dt = bm.ExecuteAdapter("select * from OperationTypeDegrees where OperationTypeId=" & OperationTypeId & " and DegreeId=dbo.GetEmpDegreeId(" & Val(DrId1.SelectedValue) & ")")
                If dt.Rows.Count > 0 Then
                    Dr1Value.Text = Val(Dr1Value.Text) + Val(dt.Rows(0)("Dr1Value").ToString) * Perc / 100
                    Dr2Value.Text = Val(Dr2Value.Text) + Val(dt.Rows(0)("Dr2Value").ToString) * Perc / 100
                    Dr3Value.Text = Val(Dr3Value.Text) + Val(dt.Rows(0)("Dr3Value").ToString) * Perc / 100
                    AnesthetistValue.Text = Val(AnesthetistValue.Text) + Val(dt.Rows(0)("AnesthetistValue").ToString) * Perc / 100
                    'CsValue.Text = Val(CsValue.Text )+Val(dt.Rows(0)("CsValue").ToString)*Perc /100
                    'MedicalValue.Text = Val(MedicalValue.Text )+Val(dt.Rows(0)("MedicalValue").ToString)*Perc /100
                    CoValue.Text = Val(CoValue.Text) + Val(dt.Rows(0)("CoValue").ToString) * Perc / 100
                    ConsumablesValue.Text = Val(ConsumablesValue.Text) + Val(dt.Rows(0)("ConsumablesValue").ToString) * Perc / 100
                End If

            Else
                If OperationDescriptionId = 0 Then
                    dt = bm.ExecuteAdapter("select T.*,TT.CalcAfter CalcAfter from OperationPackageDescriptions T left join OperationTypes TT on(T.OperationTypeId=TT.Id) where T.OperationTypeId=" & OperationTypeId & " and T.DegreeId=dbo.GetEmpDegreeId(" & Val(DrId1.SelectedValue) & ") and T.RoomTypeId=(select RR.RoomTypeId from Rooms RR where RR.Id='" & Val(RoomId.SelectedValue) & "')")
                Else
                    'dt = bm.ExecuteAdapter("select T.*,TT.CalcAfter from OperationDescriptions T left join OperationTypes TT on(T.Id=TT.Id)where T.Id=" & OperationDescriptionId & " and T.DegreeId=dbo.GetEmpDegreeId(" & Val(DrId1.SelectedValue) & ")")
                    dt = bm.ExecuteAdapter("select *,0 CalcAfter from OperationDescriptions where Id=" & OperationDescriptionId & " and DegreeId=dbo.GetEmpDegreeId(" & Val(DrId1.SelectedValue) & ") and RoomTypeId=(select RR.RoomTypeId from Rooms RR where RR.Id='" & Val(RoomId.SelectedValue) & "')")
                End If
                If dt.Rows.Count > 0 Then
                    Dim MyValue As Decimal = Val(dt.Rows(0)("Value").ToString)
                    If Val(dt.Rows(0)("CalcAfter").ToString) = 1 Then
                        MyValue -= Val(dt.Rows(0)("ConsumablesValue").ToString)
                    End If

                    RoomValue.Text = Val(RoomValue.Text) + Val(dt.Rows(0)("RoomValue").ToString) * Perc / 100
                    'RoomValue2.Text = Val(RoomValue2.Text) + Val(bm.ExecuteScalar("select isnull(Living,0) from RoomTypes where Id=(select R.RoomTypeId from Rooms R where Id=" & Val(RoomId.SelectedValue) & ")")) * Perc / 100
                    'CsValue.Text = Val(CsValue.Text) + Val(bm.ExecuteScalar("select isnull(Care,0) from RoomTypes where Id=(select R.RoomTypeId from Rooms R where Id=" & Val(RoomId.SelectedValue) & ")")) * Perc / 100
                    'MedicalValue.Text = Val(MedicalValue.Text) + Val(bm.ExecuteScalar("select isnull(supervision,0) from RoomTypes where Id=(select R.RoomTypeId from Rooms R where Id=" & Val(RoomId.SelectedValue) & ")")) * Perc / 100


                    Dr1Value.Text = Val(Dr1Value.Text) + MyValue * Val(dt.Rows(0)("Dr1Perc").ToString) / 100 * Perc / 100
                    Dr2Value.Text = Val(Dr2Value.Text) + MyValue * Val(dt.Rows(0)("Dr2Perc").ToString) / 100 * Perc / 100
                    Dr3Value.Text = Val(Dr3Value.Text) + MyValue * Val(dt.Rows(0)("Dr3Perc").ToString) / 100 * Perc / 100
                    AnesthetistValue.Text = Val(AnesthetistValue.Text) + MyValue * Val(dt.Rows(0)("AnesthetistPerc").ToString) / 100 * Perc / 100
                    'CsValue.Text = Val(CsValue.Text )+Val(dt.Rows(0)("CsValue").ToString)*Perc /100
                    'MedicalValue.Text = Val(MedicalValue.Text )+Val(dt.Rows(0)("MedicalValue").ToString)*Perc /100
                    '''''''CoValue.Text = Val(CoValue.Text) + Val(dt.Rows(0)("CoValue").ToString) * Perc / 100
                    ConsumablesValue.Text = Val(ConsumablesValue.Text) + Val(dt.Rows(0)("ConsumablesValue").ToString) * Perc / 100
                    Payed.Text = Val(Payed.Text) + Val(dt.Rows(0)("InsAmount").ToString) * Perc / 100
                    NoOfDays.Text = IIf(Val(dt.Rows(0)("NoOfDays").ToString) > Val(NoOfDays.Text), Val(dt.Rows(0)("NoOfDays").ToString), NoOfDays.Text)
                End If

            End If


        Next

    End Sub

    Private Sub RoomValue_TextChanged(sender As Object, e As EventArgs) Handles RoomValue.TextChanged, RoomValue2.TextChanged, Dr1Value.TextChanged, Dr2Value.TextChanged, Dr3Value.TextChanged, AnesthetistValue.TextChanged, CsValue.TextChanged, MedicalValue.TextChanged, CoValue.TextChanged, ConsumablesValue.TextChanged
        If lop2 Then Return
        Value.Text = Val(RoomValue.Text) + Val(RoomValue2.Text) + Val(Dr1Value.Text) + Val(Dr2Value.Text) + Val(Dr3Value.Text) + Val(AnesthetistValue.Text) + Val(CsValue.Text) + Val(MedicalValue.Text) + Val(CoValue.Text) + Val(ConsumablesValue.Text)
        'Payed.Text = Value.Text
    End Sub

    Private Sub btnPackage_Click(sender As Object, e As RoutedEventArgs) Handles btnPackage.Click
        If bm.ShowDeleteMSG("هل تريد تحويل الحالة إلي حالة خاصة") Then
            Dr1Value.Clear()
            Dr2Value.Clear()
            Dr3Value.Clear()
            AnesthetistValue.Clear()
            MedicalValue.Clear()
            CoValue.Clear()
            If Md.MyProjectType = ProjectType.X Then ConsumablesValue.Clear()
        End If
    End Sub

    Private Sub FillGrid()
        dt = bm.ExecuteAdapter("select OperationTypeId,dbo.GetOperationTypesName(OperationTypeId)OperationTypeName,Perc from OperationMotionsOperationTypes where " & SubId & "=" & Val(txtID.Text) & "")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add({dt.Rows(i)(0), dt.Rows(i)(1), dt.Rows(i)(2)})
        Next
    End Sub

    Private Sub GridRowsAdded(sender As Object, e As Forms.DataGridViewRowsAddedEventArgs)
        Try
            If e.RowIndex = 0 Then G.Rows(e.RowIndex).Cells(GC.Perc).Value = 100
        Catch ex As Exception
        End Try
    End Sub

End Class
