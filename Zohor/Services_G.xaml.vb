Imports System.Data

Public Class Services_G
    Public TableName As String = "Services"
    Public SubId As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Public Flag As Integer = 1

    Public IsEditing As Boolean = False
    Public IsServicesLab As Boolean = False

    Dim IsLoaded As Boolean = False
    Public Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If IsLoaded OrElse bm.TestIsLoaded(Me) Then Return
        IsLoaded = True

        bm.TestSecurity(Me, {btnSaveWithoutPrint}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnSave})
        LoadResource()
        CurrentShift.Visibility = Visibility.Hidden
        InsertedDate.Visibility = Visibility.Hidden
        IsClosed.Visibility = Visibility.Hidden
        IsAutomatic.Visibility = Visibility.Hidden

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReservation)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdRemaining)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdCanceled)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReturned)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", LabEmpIdReservation)

        bm.FillCombo("Companies", CompanyId, "")

        bm.Fields = New String() {"Flag", SubId, "DayDate", "Canceled", "CaseId", "CompanyId", "Notes", "SerialId", "SerialId2", "Done", "RemainingDate", "EmpIdReservation", "EmpIdRemaining", "EmpIdCanceled", "CanceledDate", "Returned", "ReturnedDate", "EmpIdReturned", "CurrentShift", "InsertedDate", "IsClosed", "RefereId", "CaseInvoiceNo", "IsAutomatic", "IsPosted", "LabEmpIdReservation", "LabDateTime"}
        bm.control = New Control() {txtFlag, txtID, DayDate, Canceled, CaseId, CompanyId, Notes, SerialId, SerialId2, Done, RemainingDate, EmpIdReservation, EmpIdRemaining, EmpIdCanceled, CanceledDate, Returned, ReturnedDate, EmpIdReturned, CurrentShift, InsertedDate, IsClosed, RefereId, CaseInvoiceNo, IsAutomatic, IsPosted, LabEmpIdReservation, LabDateTime}
        bm.KeyFields = New String() {"Flag", SubId}
        bm.Table_Name = TableName
        LoadWFH()
        LoadServiceGroups()

        btnNew_Click(sender, e)
        Done_Unchecked(Nothing, Nothing)
        Canceled_Unchecked(Nothing, Nothing)
        Returned_Unchecked(Nothing, Nothing)

        If Not Md.Manager AndAlso Not IsServicesLab AndAlso Md.MyProjectType = ProjectType.Zohor Then
            bm.AppendWhere = " and (username=" & Md.UserName & " or EmpIdReservation=0)"
        End If

        If Not Md.Manager AndAlso Md.MyProjectType = ProjectType.Zohor Then
            btnFirst.Visibility = Visibility.Hidden
            btnPrevios.Visibility = Visibility.Hidden
            btnNext.Visibility = Visibility.Hidden
            btnLast.Visibility = Visibility.Hidden
        End If

        If Not Md.MyProjectType = ProjectType.X Then
            lblRefereId.Visibility = Visibility.Hidden
            RefereId.Visibility = Visibility.Hidden
            RefereName.Visibility = Visibility.Hidden
        End If

        If Md.MyProjectType = ProjectType.X Then
            lblPayed.IsReadOnly = False
        End If

        If Flag = 2 Then
            lbllblPayed.Visibility = Visibility.Hidden
            lblPayed.Visibility = Visibility.Hidden
            Done.Visibility = Visibility.Hidden
            Returned.Visibility = Visibility.Hidden
        End If

        If IsEditing Then
            TabControl1.Visibility = Visibility.Hidden
            TabControl2.Visibility = Visibility.Hidden

            btnFirst.Visibility = Visibility.Hidden
            btnNext.Visibility = Visibility.Hidden
            btnPrevios.Visibility = Visibility.Hidden
            btnLast.Visibility = Visibility.Hidden

            btnSaveWithoutPrint.Visibility = Visibility.Hidden
            'btnNew.Visibility = Visibility.Hidden
            btnDelete.Visibility = Visibility.Hidden
        Else
            btnEdit.Visibility = Visibility.Hidden
        End If
    End Sub


    Structure GC
        Shared ServiceGroupId As String = "ServiceGroupId"
        Shared MyServiceTypeId As String = "MyServiceTypeId"
        Shared ServiceTypeId As String = "ServiceTypeId"
        Shared DrId As String = "DrId"
        Shared CsId As String = "CsId"
        Shared DrValue As String = "DrValue"
        Shared CsValue As String = "CsValue"
        Shared CoValue As String = "CoValue"
        Shared PreValue As String = "PreValue"
        Shared PrePayed As String = "PrePayed"
        Shared PreRemaining As String = "PreRemaining"
        Shared Qty As String = "Qty"
        Shared Value As String = "Value"
        Shared Payed As String = "Payed"
        Shared Remaining As String = "Remaining"
        Shared IsLabToLab As String = "Lab To Lab"
        Shared MyLine As String = "MyLine"
        Shared IsLabToLabDone As String = "IsLabToLabDone"
        Shared IsLabToLabDoneEmpId As String = "IsLabToLabDoneEmpId"
        Shared IsLabToLabDoneGetdate As String = "IsLabToLabDoneGetdate"
        Shared IsLabToLabPrice As String = "IsLabToLabPrice"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        Dim GCServiceGroupId As New Forms.DataGridViewComboBoxColumn
        GCServiceGroupId.HeaderText = "المجموعة"
        GCServiceGroupId.Name = GC.ServiceGroupId
        If IsServicesLab Then
            bm.FillCombo("select Id,Name from ServiceGroups where Id=(select LabServiceGroupId from Statics) union all select 0 Id,'-' Name order by Id", GCServiceGroupId)
        Else
            bm.FillCombo("select Id,Name from ServiceGroups where " & IIf(Flag = 1, "IsService1", "IsService2") & "=1 union all select 0 Id,'-' Name order by Id", GCServiceGroupId)
        End If
        G.Columns.Add(GCServiceGroupId)

        G.Columns.Add(GC.MyServiceTypeId, "كود النوع")

        Dim GCServiceTypeId As New Forms.DataGridViewComboBoxColumn
        GCServiceTypeId.HeaderText = "النوع"
        GCServiceTypeId.Name = GC.ServiceTypeId
        bm.FillCombo("select 0 Id,'-' Name", GCServiceTypeId)
        G.Columns.Add(GCServiceTypeId)

        Dim GCDrId As New Forms.DataGridViewComboBoxColumn
        GCDrId.HeaderText = "الطبيب"
        GCDrId.Name = GC.DrId
        bm.FillCombo("select 0 Id,'-' Name", GCDrId)
        G.Columns.Add(GCDrId)

        Dim GCCsId As New Forms.DataGridViewComboBoxColumn
        GCCsId.HeaderText = "التمريض"
        GCCsId.Name = GC.CsId
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Nurse='1' and Stopped='0' union select 0 Id,'-' Name", GCCsId)
        G.Columns.Add(GCCsId)

        G.Columns.Add(GC.DrValue, "DrValue")
        G.Columns.Add(GC.CsValue, "CsValue")
        G.Columns.Add(GC.CoValue, "CoValue")
        G.Columns.Add(GC.PreValue, "السعر")
        G.Columns.Add(GC.PrePayed, "حصة المريض")
        G.Columns.Add(GC.PreRemaining, "حصة الشركة")
        G.Columns.Add(GC.Qty, "العدد")
        G.Columns.Add(GC.Value, "السعر")
        G.Columns.Add(GC.Payed, "حصة المريض")
        G.Columns.Add(GC.Remaining, "حصة الشركة")


        Dim GCIsLabToLab As New Forms.DataGridViewComboBoxColumn
        GCIsLabToLab.HeaderText = "Lab To Lab"
        GCIsLabToLab.Name = GC.IsLabToLab
        bm.FillCombo("select Id,Name from LabToLab union select 0 Id,'-' Name", GCIsLabToLab)
        G.Columns.Add(GCIsLabToLab)
        G.Columns.Add(GC.MyLine, "MyLine")
        G.Columns.Add(GC.IsLabToLabDone, "IsLabToLabDone")
        G.Columns.Add(GC.IsLabToLabDoneEmpId, "IsLabToLabDoneEmpId")
        G.Columns.Add(GC.IsLabToLabDoneGetdate, "IsLabToLabDoneGetdate")
        G.Columns.Add(GC.IsLabToLabPrice, "IsLabToLabPrice")

        'G.Columns(GC.DrId).Visible = False
        G.Columns(GC.CsId).Visible = False
        G.Columns(GC.DrValue).Visible = False
        G.Columns(GC.CsValue).Visible = False
        G.Columns(GC.CoValue).Visible = False
        G.Columns(GC.PreValue).Visible = False
        G.Columns(GC.PrePayed).Visible = False
        G.Columns(GC.PreRemaining).Visible = False
        G.Columns(GC.MyLine).Visible = False

        If Md.MyProjectType = ProjectType.Zohor Then
            G.Columns(GC.Qty).Visible = False
            If Md.Nurse Then
                G.Columns(GC.Value).Visible = False
                G.Columns(GC.Payed).Visible = False
                G.Columns(GC.Remaining).Visible = False
            End If
        End If
        If Not Md.Manager AndAlso Md.MyProjectType = ProjectType.Zohor Then
            G.Columns(GC.Value).ReadOnly = True
        End If

        G.Columns(GC.IsLabToLab).Visible = IsServicesLab
        G.Columns(GC.IsLabToLabDone).Visible = False
        G.Columns(GC.IsLabToLabDoneEmpId).Visible = False
        G.Columns(GC.IsLabToLabDoneGetdate).Visible = False
        G.Columns(GC.IsLabToLabPrice).Visible = False

        G.Columns(GC.Remaining).ReadOnly = True
        G.Columns(GC.ServiceGroupId).FillWeight = 200
        G.Columns(GC.ServiceTypeId).FillWeight = 300
        G.Columns(GC.DrId).FillWeight = 200

        If IsEditing Then
            G.Columns(GC.ServiceGroupId).ReadOnly = True
            G.Columns(GC.MyServiceTypeId).ReadOnly = True
            G.Columns(GC.ServiceTypeId).ReadOnly = True
            G.Columns(GC.DrId).ReadOnly = True
            G.Columns(GC.Qty).ReadOnly = True
        End If

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
    End Sub


    Sub LoadServiceGroups()
        Try
            WR1.Children.Clear()
            For i As Integer = 0 To CType(G.Columns(GC.ServiceGroupId), Forms.DataGridViewComboBoxColumn).Items.Count - 1
                If CType(G.Columns(GC.ServiceGroupId), Forms.DataGridViewComboBoxColumn).Items(i)("Id").ToString = 0 Then Continue For
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & CType(G.Columns(GC.ServiceGroupId), Forms.DataGridViewComboBoxColumn).Items(i)("Id").ToString
                x.Tag = CType(G.Columns(GC.ServiceGroupId), Forms.DataGridViewComboBoxColumn).Items(i)("Id")
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 110
                x.Height = 30
                x.Margin = New Thickness(5, 5, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = CType(G.Columns(GC.ServiceGroupId), Forms.DataGridViewComboBoxColumn).Items(i)("Name").ToString.Replace(vbCrLf, " ")
                x.ToolTip = x.Content
                x.Background = btnNew.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR1.Children.Add(x)
                AddHandler x.Click, AddressOf btnServiceGroupClick
            Next
        Catch ex As Exception
        End Try
    End Sub


    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {"Flag", SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim lop As Boolean = False
    Sub FillControls()
        bm.FillControls(Me)
        lop = True
        bm.FillControls(Me)

        If Not InsertedDate.Text = "" Then InsertedDate.Text = bm.ToStrDateTimeFormated(InsertedDate.Text)

        LabDateTime.Text = bm.ToStrDateTimeFormated(LabDateTime.Text)

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


        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & TableName & " where Flag=" & Flag & " and " & SubId & "=" & txtID.Text)
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            If G.Columns(GC.Remaining).Visible Then
                G.CurrentCell = G.Rows(i).Cells(GC.Remaining)
            End If
            G.Rows(i).Cells(GC.ServiceGroupId).Value = dt.Rows(i)("ServiceGroupId").ToString
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ServiceGroupId).Index, i))
            G.Rows(i).Cells(GC.ServiceTypeId).Value = dt.Rows(i)("ServiceTypeId")
            G.Rows(i).Cells(GC.MyServiceTypeId).Value = dt.Rows(i)("ServiceTypeId")
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ServiceTypeId).Index, i))
            G.Rows(i).Cells(GC.DrId).Value = dt.Rows(i)("DrId")
            G.Rows(i).Cells(GC.CsId).Value = dt.Rows(i)("CsId")
            G.Rows(i).Cells(GC.DrValue).Value = dt.Rows(i)("DrValue")
            G.Rows(i).Cells(GC.CsValue).Value = dt.Rows(i)("CsValue")
            G.Rows(i).Cells(GC.CoValue).Value = dt.Rows(i)("CoValue")
            G.Rows(i).Cells(GC.PreValue).Value = dt.Rows(i)("PreValue")
            G.Rows(i).Cells(GC.PrePayed).Value = dt.Rows(i)("PrePayed")
            G.Rows(i).Cells(GC.PreRemaining).Value = dt.Rows(i)("PreRemaining")
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty")
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value")
            G.Rows(i).Cells(GC.Payed).Value = dt.Rows(i)("Payed")
            G.Rows(i).Cells(GC.Remaining).Value = dt.Rows(i)("Remaining")
            G.Rows(i).Cells(GC.IsLabToLab).Value = dt.Rows(i)("IsLabToLab").ToString
            G.Rows(i).Cells(GC.MyLine).Value = dt.Rows(i)("MyLine")
            G.Rows(i).Cells(GC.IsLabToLabDone).Value = dt.Rows(i)("IsLabToLabDone")
            G.Rows(i).Cells(GC.IsLabToLabDoneEmpId).Value = dt.Rows(i)("IsLabToLabDoneEmpId")
            G.Rows(i).Cells(GC.IsLabToLabDoneGetdate).Value = bm.ToStrDateTimeFormated(dt.Rows(i)("IsLabToLabDoneGetdate"))
            G.Rows(i).Cells(GC.IsLabToLabPrice).Value = dt.Rows(i)("IsLabToLabPrice")
            If G.Columns(GC.Remaining).Visible Then
                G.CurrentCell = G.Rows(i).Cells(GC.Remaining)
            End If
        Next
        DayDate.Focus()
        G.RefreshEdit()

        lop = False
        GridCalcRow(Nothing, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Remaining).Index, 0))
        UndoNewId()


        btnSave.IsEnabled = True
        btnSaveWithoutPrint.IsEnabled = True
        btnDelete.IsEnabled = True

        If Val(CaseInvoiceNo.Text) <> 0 Then
            btnSave.IsEnabled = False
            btnSaveWithoutPrint.IsEnabled = False
            btnDelete.IsEnabled = False
        End If
        If Val(CurrentShift.Text) > 0 Then
            btnSaveWithoutPrint.IsEnabled = Md.Manager
        End If


        Notes.Focus()


        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnSaveWithoutPrint.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

        If IsServicesLab And EmpIdReservation.SelectedIndex > 0 Then
            WR1.IsEnabled = False
            WR2.IsEnabled = False

            G.Columns(GC.ServiceGroupId).ReadOnly = True
            G.Columns(GC.ServiceTypeId).ReadOnly = True
            G.Columns(GC.MyServiceTypeId).ReadOnly = True
            G.Columns(GC.DrId).ReadOnly = True
        End If

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {"Flag", SubId}, New String() {Flag, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim lop2 As Boolean = False
    Dim IsNew As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveWithoutPrint.Click, btnEdit.Click
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

        G.EndEdit()
        If Not G.CurrentCell Is Nothing Then
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.CurrentCell.ColumnIndex, G.CurrentCell.ColumnIndex))
        End If

        For i As Integer = 0 To G.Rows.Count - 1
            Try
                If Not G.Rows(i).Cells(GC.ServiceGroupId).Value Is Nothing Then
                    If Val(G.Rows(i).Cells(GC.MyServiceTypeId).Value) = 0 AndAlso Val(G.Rows(i).Cells(GC.Value).Value) <> 0 Then
                        bm.ShowMSG("برجاء تحديد النوع بالسطر " & (i + 1))
                        G.CurrentCell = G.Rows(i).Cells(GC.MyServiceTypeId)
                        Return
                    End If
                End If
            Catch ex As Exception
            End Try
        Next


        If IsEditing Then
            Edit()
        End If

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If txtID.Text.Trim = "" Then
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where Flag=" & Flag)
            If txtID.Text = "" Then txtID.Text = "1"
            LastEntry.Text = txtID.Text
            State = BasicMethods.SaveState.Insert
        End If

        If InsertedDate.Text.Trim = "" Then InsertedDate.Text = bm.ExecuteScalar("select dbo.MyGetDateTime()")
        If InsertedDate.Text.Trim = "" Then Return

        If Not IsServicesLab AndAlso EmpIdReservation.SelectedIndex < 1 Then
            EmpIdReservation.SelectedValue = Md.UserName
        End If

        If btnSaveWithoutPrint.IsEnabled Then
            ' Must DefineValues to allow to save Grid
            bm.DefineValues()

            'If Not bm.Save(New String() {"Flag", SubId}, New String() {Flag, txtID.Text.Trim},State ) Then
            '    If State = BasicMethods.SaveState.Insert Then
            '        txtID.Text = ""
            '        LastEntry.Text = ""
            '        SerialId.Text = ""
            '        LastSerialId.Text = ""
            '    End If
            '    Return
            'End If


            If Not bm.SaveGrid(G, TableName, New String() {"Flag", SubId}, New String() {Flag, txtID.Text}, New String() {"ServiceGroupId", "ServiceTypeId", "CoValue", "CsId", "CsValue", "DrId", "DrValue", "PrePayed", "PreRemaining", "PreValue", "Qty", "Payed", "Remaining", "Value", "IsLabToLab", "IsLabToLabDone", "IsLabToLabDoneEmpId", "IsLabToLabDoneGetdate", "IsLabToLabPrice"}, New String() {GC.ServiceGroupId, GC.ServiceTypeId, GC.CoValue, GC.CsId, GC.CsValue, GC.DrId, GC.DrValue, GC.PrePayed, GC.PreRemaining, GC.PreValue, GC.Qty, GC.Payed, GC.Remaining, GC.Value, GC.IsLabToLab, GC.IsLabToLabDone, GC.IsLabToLabDoneEmpId, GC.IsLabToLabDoneGetdate, GC.IsLabToLabPrice}, New VariantType() {VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.Integer, VariantType.Decimal, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Date, VariantType.Decimal}, New String() {GC.ServiceTypeId}, GC.MyLine, , True) Then Return
            'If Not bm.Save(New String() {"Flag", SubId}, New String() {Flag, txtID.Text.Trim}) Then Return

            If Val(SerialId.Text) = 0 Then 'AndAlso Val(Payed.Text) > 0
                If Md.MyProjectType = ProjectType.Zohor Then
                    SerialId.Text = bm.ExecuteScalar("updateServicesSerialIdCo", {"ServiceGroupId", "Flag", "InvoiceNo"}, {0, Flag, Val(txtID.Text)})
                Else
                    SerialId.Text = bm.ExecuteScalar("updateServicesSerialId", {"ServiceGroupId", "Flag", "InvoiceNo"}, {0, Flag, Val(txtID.Text)})
                End If
                LastSerialId.Text = SerialId.Text
            End If

            If SerialId2.IsVisible AndAlso Val(SerialId2.Text) = 0 Then 'AndAlso Val(Remaining.Text) > 0 
                If Md.MyProjectType = ProjectType.Zohor Then
                    SerialId2.Text = bm.ExecuteScalar("updateServicesSerialIdCo2", {"ServiceGroupId", "Flag", "InvoiceNo"}, {0, Flag, Val(txtID.Text)})
                Else
                    SerialId2.Text = bm.ExecuteScalar("updateServicesSerialId2", {"ServiceGroupId", "Flag", "InvoiceNo"}, {0, Flag, Val(txtID.Text)})
                End If
                LastSerialId.Text = SerialId2.Text
            End If

        End If


        TraceInvoice(State.ToString)

        If Not Canceled.IsChecked AndAlso Not Returned.IsChecked AndAlso sender Is btnSave Then ShowRPT()
        If IsEditing Then
            bm.ShowMSG("تم تسجيل إيصال برقم " & txtID.Text)
        End If
        btnNew_Click(sender, e)

    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExecuteNonQuery("BeforeDeleteServices", New String() {"Flag", "InvoiceNo", "UserDelete", "State"}, New String() {Flag, txtID.Text, Md.UserName, State})
    End Sub

    Private Sub ShowRPT()
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Flag", "@Id", "Header", "IsNew"}
        rpt.paravalue = New String() {Flag, txtID.Text, CType(Parent, Page).Title, IIf(IsNew, 1, 0)}
        rpt.Rpt = "ServicesONEG.rpt"
        If Md.MyProjectType = ProjectType.Zohor Then
            rpt.Rpt = "ServicesONEGZohor.rpt"
            If IsServicesLab Then
                rpt.Rpt = "ServicesONEGZohorLab.rpt"
            End If
        End If
        rpt.Print()
        'rpt.Show()
    End Sub

    Sub NewId()
        txtFlag.Text = Flag
        txtID.Clear()
        'txtID.IsEnabled = False

        DayDate.IsEnabled = Md.Manager
        G.ReadOnly = False
        WR1.IsEnabled = True
        WR2.IsEnabled = True

        CaseId.IsEnabled = True
        IsNew = True
    End Sub

    Sub UndoNewId()
        txtID.IsEnabled = True

        'DayDate.IsEnabled = Md.Manager
        'G.ReadOnly = Not Md.Manager
        'WR1.IsEnabled = Md.Manager
        'WR2.IsEnabled = Md.Manager

        'CaseId.IsEnabled = Md.Manager
        IsNew = False
        If EmpIdReservation.SelectedIndex < 1 Then
            IsNew = True
        End If

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {"Flag", SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls(False)

        WR1.IsEnabled = True
        WR2.IsEnabled = True

        G.Columns(GC.ServiceGroupId).ReadOnly = False
        G.Columns(GC.ServiceTypeId).ReadOnly = False
        G.Columns(GC.MyServiceTypeId).ReadOnly = False
        G.Columns(GC.DrId).ReadOnly = False


        G.Rows.Clear()
        lblTotal.Clear()
        lblPayed.Clear()
        Canceled.IsEnabled = True
        DayDate.SelectedDate = bm.MyGetDate()
        CaseID_LostFocus(Nothing, Nothing)
        RefereId_LostFocus(Nothing, Nothing)



        If IsServicesLab Then
            LabEmpIdReservation.SelectedValue = Md.UserName
            LabDateTime.Text = bm.MyGetDateTime()
        Else
            EmpIdReservation.SelectedValue = Md.UserName
        End If


        btnSave.IsEnabled = True
        btnSaveWithoutPrint.IsEnabled = True
        btnDelete.IsEnabled = True
        NewId()
        DayDate.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where Flag=" & Flag & " and " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {"Flag", SubId}, New String() {Flag, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Public Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {"Flag", SubId}, New String() {Flag, txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            ClearControls()
            If Md.Manager Then txtID.Text = s
            lv = False
            Return
        End If
        FillControls()
        lv = False

        If IsEditing Then
            If Canceled.IsChecked OrElse Returned.IsChecked Then
                btnEdit.Visibility = Visibility.Hidden
            Else
                btnEdit.Visibility = Visibility.Visible
                G.ReadOnly = False
                'G.Columns(GC.ServiceGroupId).ReadOnly = True
                'G.Columns(GC.ServiceTypeId).ReadOnly = True
            End If
        End If

        If IsServicesLab Then

        End If
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub


    
    

    Private Sub Canceled_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Checked
        Canceled.IsEnabled = Md.Manager
        Returned.IsEnabled = Md.Manager
        Returned.IsChecked = False

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
            Done.IsEnabled = True
        End If

        ReturnedDate.Visibility = Visibility.Hidden
        ReturnedDate.SelectedDate = Nothing
        EmpIdReturned.Visibility = Visibility.Hidden
        EmpIdReturned.SelectedIndex = 0
    End Sub

    Sub LoadServiceTypes()
        Try
            WR2.Children.Clear()
            WR2.Tag = G.CurrentRow.Cells(GC.ServiceGroupId).Value
            For i As Integer = 0 To CType(G.CurrentRow.Cells(GC.ServiceTypeId), Forms.DataGridViewComboBoxCell).Items.Count - 1
                If CType(G.CurrentRow.Cells(GC.ServiceTypeId), Forms.DataGridViewComboBoxCell).Items(i)("Id").ToString = 0 Then Continue For
                Dim x As New Button
                x.Style = Application.Current.FindResource("GlossyCloseButton")
                x.Name = "R" & CType(G.CurrentRow.Cells(GC.ServiceTypeId), Forms.DataGridViewComboBoxCell).Items(i)("Id").ToString
                x.Tag = CType(G.CurrentRow.Cells(GC.ServiceTypeId), Forms.DataGridViewComboBoxCell).Items(i)("Id")
                x.VerticalContentAlignment = VerticalAlignment.Center
                x.Width = 110
                x.Height = 30
                x.Margin = New Thickness(5, 5, 0, 0)
                x.HorizontalAlignment = HorizontalAlignment.Left
                x.VerticalAlignment = VerticalAlignment.Top
                x.Cursor = Input.Cursors.Pen
                x.Content = CType(G.CurrentRow.Cells(GC.ServiceTypeId), Forms.DataGridViewComboBoxCell).Items(i)("Name").ToString.Replace(vbCrLf, " ")
                x.ToolTip = x.Content
                x.Background = btnDelete.Background
                x.Foreground = System.Windows.Media.Brushes.Black
                WR2.Children.Add(x)
                AddHandler x.Click, AddressOf btnServiceTypeClick
            Next
        Catch ex As Exception
        End Try
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
            Dim sd As String = G.CurrentRow.Cells(GC.DrId).Value
            Dim dt As DataTable = bm.ExecuteAdapter("select DepartmentId from ServiceTypes where ServiceGroupId=" & Val(G.CurrentRow.Cells(GC.ServiceGroupId).Value) & " and Id=" & Val(G.CurrentRow.Cells(GC.ServiceTypeId).Value))
            Dim s As String = 0
            If dt.Rows.Count > 0 Then
                s = Val(dt.Rows(0)("DepartmentId").ToString)
            End If
            bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Doctor='1' and (DepartmentId=" & s & " or " & s & "=0) and Stopped='0' union select 0 Id,'-' Name order by Name", G.CurrentRow.Cells(GC.DrId))
            If lop Then G.CurrentRow.Cells(GC.DrId).Value = sd
        Catch ex As Exception
        End Try

    End Sub

    Private Sub btnServiceGroupClick(sender As Object, e As RoutedEventArgs)
        If G.CurrentRow Is Nothing OrElse G.CurrentRow.Index = G.Rows.Count - 1 Then
            G.CurrentCell = G.Rows(G.Rows.Add()).Cells(GC.ServiceGroupId)
        End If
        If Not G.CurrentRow.Cells(GC.ServiceTypeId).Value Is Nothing AndAlso Val(G.CurrentRow.Cells(GC.ServiceTypeId).Value.ToString) > 0 Then
            G.CurrentCell = G.Rows(G.Rows.Add()).Cells(GC.ServiceGroupId)
        End If
        G.CurrentRow.Cells(GC.ServiceGroupId).Value = sender.Tag
        If G.Columns(GC.Remaining).Visible Then
            G.CurrentCell = G.CurrentRow.Cells(GC.Remaining)
        End If
        G.EndEdit()
        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ServiceGroupId).Index, G.CurrentRow.Index))
    End Sub

    Private Sub btnServiceTypeClick(sender As Object, e As RoutedEventArgs)
        If G.CurrentRow Is Nothing Then
            G.CurrentCell = G.Rows(G.Rows.Add()).Cells(GC.ServiceGroupId)
        End If
        If Val(G.CurrentRow.Cells(GC.ServiceTypeId).Value) > 0 Then
            G.CurrentCell = G.Rows(G.Rows.Add()).Cells(GC.ServiceGroupId)
        End If
        G.CurrentRow.Cells(GC.ServiceGroupId).Value = WR2.Tag
        G.EndEdit()
        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ServiceGroupId).Index, G.CurrentRow.Index))
        G.CurrentRow.Cells(GC.ServiceTypeId).Value = sender.Tag
        If G.Columns(GC.Remaining).Visible Then G.CurrentCell = G.CurrentRow.Cells(GC.Remaining)
        G.EndEdit()
        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ServiceTypeId).Index, G.CurrentRow.Index))
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
        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblLastEntry.SetResourceReference(ContentProperty, "LastEntry")
        lblNotes.SetResourceReference(ContentProperty, "Notes")
        
        lblSerialId.SetResourceReference(ContentProperty, "Serial")
        lblSerialId2.SetResourceReference(ContentProperty, "Serial")
        lblLastSerialId.SetResourceReference(ContentProperty, "LastSerial")

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

    Private Sub GridCalcRow(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            Select Case e.ColumnIndex
                Case G.Columns(GC.ServiceGroupId).Index
                    If G.Rows(e.RowIndex).Index > 0 AndAlso Val(G.Rows(e.RowIndex).Cells(GC.ServiceGroupId).Value) = 0 Then
                        G.Rows(e.RowIndex).Cells(GC.ServiceGroupId).Value = G.Rows(G.Rows(e.RowIndex).Index - 1).Cells(GC.ServiceGroupId).Value
                    End If
                    Dim s As Integer = 0
                    Try
                        s = Val(G.Rows(e.RowIndex).Cells(GC.ServiceTypeId).Value)
                    Catch ex As Exception
                    End Try
                    Try
                        bm.FillCombo("select 0 Id,'-' Name union select Id,Name from ServiceTypes Where IsStopped=0 and ServiceGroupId = " & Val(G.Rows(e.RowIndex).Cells(GC.ServiceGroupId).Value.ToString), CType(G.Rows(e.RowIndex).Cells(GC.ServiceTypeId), Forms.DataGridViewComboBoxCell))
                        G.Rows(e.RowIndex).Cells(GC.ServiceTypeId).Value = s
                    Catch ex As Exception
                    End Try
                    TabItem1.Header = ""
                    'If Val(G.Rows(e.RowIndex ).Cells(GC.ServiceGroupId).Value) > 0 Then TabItem1.Header = CType(CType(G.Columns(GC.ServiceGroupId), Forms.DataGridViewComboBoxColumn).DataSource, DataTable).Rows(G.Rows(e.RowIndex ).Index)(GC.ServiceGroupId).ToString


                    LoadServiceTypes()

                Case G.Columns(GC.MyServiceTypeId).Index
                    If G.Rows(e.RowIndex).Index > 0 AndAlso Val(G.Rows(e.RowIndex).Cells(GC.ServiceGroupId).Value) = 0 Then
                        G.Rows(e.RowIndex).Cells(GC.ServiceGroupId).Value = G.Rows(G.Rows(e.RowIndex).Index - 1).Cells(GC.ServiceGroupId).Value
                        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ServiceGroupId).Index, G.Rows(e.RowIndex).Index))
                    End If
                    G.Rows(e.RowIndex).Cells(GC.ServiceTypeId).Value = CType(Val(G.Rows(e.RowIndex).Cells(GC.MyServiceTypeId).Value), Integer)
                    GoTo A
                Case G.Columns(GC.ServiceTypeId).Index
A:
                    FillDoctors()
                    If lop Then Return
                    G.Rows(e.RowIndex).Cells(GC.MyServiceTypeId).Value = CType(Val(G.Rows(e.RowIndex).Cells(GC.ServiceTypeId).Value), Integer)

                    G.Rows(e.RowIndex).Cells(GC.DrValue).Value = ""
                    G.Rows(e.RowIndex).Cells(GC.CsValue).Value = ""
                    G.Rows(e.RowIndex).Cells(GC.CoValue).Value = ""
                    Dim dt As DataTable = bm.ExecuteAdapter("select * from ServiceTypes where IsStopped=0 and ServiceGroupId=" & Val(G.Rows(e.RowIndex).Cells(GC.ServiceGroupId).Value) & " and Id=" & Val(G.Rows(e.RowIndex).Cells(GC.ServiceTypeId).Value))
                    If dt.Rows.Count > 0 Then



                        Dim Total As Decimal = Val(dt.Rows(0)("DrValue").ToString) + Val(dt.Rows(0)("CsValue").ToString) + Val(dt.Rows(0)("CoValue").ToString)

                        G.Rows(e.RowIndex).Cells(GC.DrValue).Value = Val(dt.Rows(0)("DrValue").ToString)
                        G.Rows(e.RowIndex).Cells(GC.CsValue).Value = Val(dt.Rows(0)("CsValue").ToString)
                        G.Rows(e.RowIndex).Cells(GC.CoValue).Value = Val(dt.Rows(0)("CoValue").ToString)

                        G.Rows(e.RowIndex).Cells(GC.PreValue).Value = Val(G.Rows(e.RowIndex).Cells(GC.DrValue).Value) + Val(G.Rows(e.RowIndex).Cells(GC.CsValue).Value) + Val(G.Rows(e.RowIndex).Cells(GC.CoValue).Value)
                        G.Rows(e.RowIndex).Cells(GC.PrePayed).Value = G.Rows(e.RowIndex).Cells(GC.PreValue).Value
                        If Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value) = 0 Then G.Rows(e.RowIndex).Cells(GC.Qty).Value = 1

                        CompanyId.ToolTip = Nothing
                        If CompanyId.SelectedIndex > 0 Then
                            Dim dt2 As DataTable = bm.ExecuteAdapter("GetserviceCompaniesData", {"CompanyId", "ServiceGroupId", "Id"}, {CompanyId.SelectedValue.ToString, Val(G.Rows(e.RowIndex).Cells(GC.ServiceGroupId).Value), Val(G.Rows(e.RowIndex).Cells(GC.ServiceTypeId).Value)})
                            G.Rows(e.RowIndex).Cells(GC.DrValue).Value = 0
                            G.Rows(e.RowIndex).Cells(GC.CsValue).Value = 0
                            G.Rows(e.RowIndex).Cells(GC.CoValue).Value = dt2.Rows(0)("Price").ToString

                            G.Rows(e.RowIndex).Cells(GC.PreValue).Value = dt2.Rows(0)("Price").ToString
                            G.Rows(e.RowIndex).Cells(GC.PrePayed).Value = dt2.Rows(0)("Payed").ToString
                            bm.ShowMsgOnScreen(dt2.Rows(0)("Notes").ToString)
                        ElseIf Val(RefereId.Text) > 0 Then
                            Dim DescPerc As Decimal = Val(bm.ExecuteScalar("select DescPerc from ExternalDoctors where Id=" & RefereId.Text.Trim() & " and Type=0"))

                            G.Rows(e.RowIndex).Cells(GC.DrValue).Value = 0
                            G.Rows(e.RowIndex).Cells(GC.CsValue).Value = 0
                            G.Rows(e.RowIndex).Cells(GC.CoValue).Value = Val(G.Rows(e.RowIndex).Cells(GC.PreValue).Value) * (100 - DescPerc) / 100

                            G.Rows(e.RowIndex).Cells(GC.PreValue).Value = G.Rows(e.RowIndex).Cells(GC.CoValue).Value
                            G.Rows(e.RowIndex).Cells(GC.PrePayed).Value = G.Rows(e.RowIndex).Cells(GC.CoValue).Value
                        End If

                        G.EndEdit()
                        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.PreValue).Index, G.Rows(e.RowIndex).Index))
                        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.PrePayed).Index, G.Rows(e.RowIndex).Index))
                    End If


                Case G.Columns(GC.Qty).Index
                    G.Rows(e.RowIndex).Cells(GC.Payed).Value = Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PrePayed).Value)
                    G.Rows(e.RowIndex).Cells(GC.Remaining).Value = Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PreRemaining).Value)
                    G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PreValue).Value)
                Case G.Columns(GC.PrePayed).Index
                    G.Rows(e.RowIndex).Cells(GC.PreRemaining).Value = Val(G.Rows(e.RowIndex).Cells(GC.PreValue).Value) - Val(G.Rows(e.RowIndex).Cells(GC.PrePayed).Value)
                    G.Rows(e.RowIndex).Cells(GC.Payed).Value = Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PrePayed).Value)
                    G.Rows(e.RowIndex).Cells(GC.PreRemaining).Value = Val(G.Rows(e.RowIndex).Cells(GC.PreValue).Value) - Val(G.Rows(e.RowIndex).Cells(GC.PrePayed).Value)
                    G.Rows(e.RowIndex).Cells(GC.Remaining).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value) - Val(G.Rows(e.RowIndex).Cells(GC.Payed).Value)
                Case G.Columns(GC.Payed).Index, G.Columns(GC.Value).Index
                    G.Rows(e.RowIndex).Cells(GC.Remaining).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value) - Val(G.Rows(e.RowIndex).Cells(GC.Payed).Value)
                Case G.Columns(GC.PreValue).Index, G.Columns(GC.Qty).Index
                    If Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value.ToString) = 0 Then G.Rows(e.RowIndex).Cells(GC.Qty).Value = 1
                    G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PreValue).Value)
            End Select
        Catch ex As Exception
        End Try

        Try
            lblTotal.Clear()
            lblPayed.Clear()
            For i As Integer = 0 To G.Rows.Count - 1
                lblTotal.Text = Val(lblTotal.Text) + Val(G.Rows(i).Cells(GC.Value).Value)
                lblPayed.Text = Val(lblPayed.Text) + Val(G.Rows(i).Cells(GC.Payed).Value)
            Next
        Catch ex As Exception
            lblTotal.Clear()
            lblPayed.Clear()
        End Try
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
                GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ServiceTypeId).Index, 0))
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub lblPayed_LostFocus(sender As Object, e As RoutedEventArgs) Handles lblPayed.LostFocus
        If lblPayed.IsReadOnly Then Return
        Try
            Dim MyPayed As Decimal = Val(lblPayed.Text)
            For i As Integer = 0 To G.Rows.Count - 1
                If MyPayed = 0 Then
                    G.Rows(i).Cells(GC.Payed).Value = 0
                ElseIf MyPayed >= Val(G.Rows(i).Cells(GC.Value).Value) Then
                    G.Rows(i).Cells(GC.Payed).Value = Val(G.Rows(i).Cells(GC.Value).Value)
                Else
                    G.Rows(i).Cells(GC.Payed).Value = MyPayed
                End If
                G.Rows(i).Cells(GC.Remaining).Value = Val(G.Rows(i).Cells(GC.Value).Value) - Val(G.Rows(i).Cells(GC.Payed).Value)
                MyPayed -= Val(G.Rows(i).Cells(GC.Payed).Value)

            Next
        Catch ex As Exception
        End Try
    End Sub

    Sub Edit()
        If txtID.Text.Trim = "" Then Return
        If Canceled.IsChecked OrElse Returned.IsChecked Then
            bm.ShowMSG("هذا الإيصال ملغي بالفعل")
            Return
        End If
        bm.ExecuteNonQuery("update Services set Canceled=1,EmpIdCanceled=" & Md.UserName & ",CanceledDate=GETDATE() where InvoiceNo=" & txtID.Text)
        txtID.Clear()
        SerialId.Clear()
        SerialId2.Clear()
        EmpIdReservation.SelectedValue = Md.UserName
        EmpIdRemaining.SelectedValue = 0
        EmpIdCanceled.SelectedValue = 0
        EmpIdReturned.SelectedValue = 0
    End Sub


    Private Sub BtnbtnSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnSearch.Click
        If bm.ShowHelp("Header", txtID, txtID, Nothing, "select distinct cast(InvoiceNo as nvarchar(100))'رقم الفاتورة',dbo.ToStrDate(DayDate)'التاريخ' from Services where CaseId=" & Val(CaseId.Text) & " and Flag=" & Flag.ToString,,,, "رقم الفاتورة", "التاريخ") Then
            'InvoiceNo.Text = bm.SelectedRow(0)
            txtID_LostFocus(Nothing, Nothing)
        End If
    End Sub
End Class
