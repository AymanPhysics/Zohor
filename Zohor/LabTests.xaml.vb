Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class LabTests

    Public TableName As String = "LabTests"
    Public TableDetailsName As String = "LabTestsDt"

    Public SubId As String = "InvoiceNo"

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    WithEvents MyTimer As New Threading.DispatcherTimer
    Public Flag As Integer
    Dim Gp As String = "التحاليل الرئيسية", Tp As String = "التحاليل الفرعية", It As String = "بنود التحاليل"

    Public MyCase As Integer = 0

    Sub NewId()
        InvoiceNo.Clear()
        InvoiceNo.IsEnabled = Md.Manager
    End Sub

    Sub UndoNewId()
        InvoiceNo.IsEnabled = True
    End Sub

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, InvoiceNo}, {btnPrint, btnPrint2, btnPrint3})
        LoadResource()
        bm.FillCombo("Gender", Gender, "", , True)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpId)

        TabItem1.Header = ""
        bm.Fields = New String() {SubId, "DayDate", "CaseId", "ReferredBy", "FinalStudy", "RInvoiceNo", "EmpId", "CBCId"}
        bm.control = New Control() {InvoiceNo, DayDate, CaseId, ReferredBy, FinalStudy, RInvoiceNo, EmpId, CBCId}
        bm.KeyFields = New String() {SubId}

        bm.Table_Name = TableName

        If Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X Then
            lblRInvoiceNo.Visibility = Visibility.Hidden
            RInvoiceNo.Visibility = Visibility.Hidden
            EmpId.Visibility = Visibility.Hidden
            lblReferredBy.Visibility = Visibility.Hidden
            ReferredBy.Visibility = Visibility.Hidden
        End If

        If Md.MyProjectType = ProjectType.X Then
            GUpdate.Visibility = Visibility.Hidden
            PanelResult.Visibility = Visibility.Hidden
        End If

        LoadGroups()
        LoadWFH()
        DayDate.SelectedDate = Nothing
        DayDate.SelectedDate = bm.MyGetDate() 'Md.CurrentDate
        btnNew_Click(sender, e)
        If MyCase > 0 Then
            CaseId.Text = MyCase
            CaseID_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Structure GC
        Shared TestId As String = "TestId"
        Shared TestName As String = "TestName"
        Shared SubTestId As String = "SubTestId"
        Shared SubTestName As String = "SubTestName"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Result As String = "Result"
        Shared Unit As String = "Unit"
        Shared NormalValue As String = "NormalValue"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.TestId, GC.TestId)
        G.Columns.Add(GC.TestName, "التحليل الرئيسى")
        G.Columns.Add(GC.SubTestId, GC.SubTestId)
        G.Columns.Add(GC.SubTestName, "التحليل الفرعى")
        G.Columns.Add(GC.Id, GC.Id)
        G.Columns.Add(GC.Name, "البند")
        G.Columns.Add(GC.Result, "النتيجة")
        G.Columns.Add(GC.Unit, "الوحدة")
        G.Columns.Add(GC.NormalValue, "NormalValue")

        G.Columns(GC.TestName).FillWeight = 200
        G.Columns(GC.SubTestName).FillWeight = 200
        G.Columns(GC.Name).FillWeight = 400
        G.Columns(GC.Result).FillWeight = 200 '400
        G.Columns(GC.Unit).FillWeight = 200
        G.Columns(GC.NormalValue).FillWeight = 200 '400

        G.Columns(GC.TestId).Visible = False
        G.Columns(GC.TestName).Visible = False
        G.Columns(GC.SubTestId).Visible = False
        G.Columns(GC.SubTestName).Visible = False
        G.Columns(GC.Id).Visible = False
        G.Columns(GC.TestName).ReadOnly = True
        G.Columns(GC.SubTestName).ReadOnly = True
        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Unit).Visible = False
        G.Columns(GC.Unit).ReadOnly = True
        G.Columns(GC.NormalValue).ReadOnly = False
        AddHandler G.CellEndEdit, AddressOf Grid_CellEndEdit
        AddHandler G.SelectionChanged, AddressOf Grid_SelectionChanged
    End Sub

    Sub LoadGroups()
        Try
            WGroups.Children.Clear()
            WTypes.Children.Clear()
            WItems.Children.Clear()
            TabGroups.Header = Gp
            TabTypes.Header = Tp
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("LoadLabTestItems")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x)
                x.Name = "TabItem_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WGroups.Children.Add(x)
                AddHandler x.Click, AddressOf LoadTypes
            Next
        Catch
        End Try
    End Sub

    Dim CurrentGroup As String = ""
    Dim CurrentGroupName As String = ""
    Dim CurrentType As String = ""
    Dim CurrentTypeName As String = ""
    Private Sub LoadTypes(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WTypes.Tag = xx.Tag
            CurrentGroup = xx.Tag
            CurrentGroupName = xx.Content
            WTypes.Children.Clear()
            WItems.Children.Clear()

            TabTypes.Header = Tp & " - " & xx.Content.ToString
            TabItems.Header = It

            If Val(bm.ExecuteScalar("Select AllTests From LaboratoryTestTypes where Id=" & xx.Tag.ToString)) = 1 Then
                AllTests = True
            End If


            Dim dt As DataTable = bm.ExecuteAdapter("LoadLaboratoryTests", New String() {"TestId"}, New String() {xx.Tag.ToString})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x)
                x.Name = "TabItem_" & xx.Tag.ToString & "_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WTypes.Children.Add(x)
                AddHandler x.Click, AddressOf LoadItems
                If AllTests Then
                    LoadItems(x, Nothing)
                End If
            Next
        Catch
        End Try
        AllTests = False
    End Sub

    Dim AllTests As Boolean = False
    Private Sub LoadItems(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WItems.Tag = xx.Tag
            CurrentType = xx.Tag
            CurrentTypeName = xx.Content
            WItems.Children.Clear()

            TabItems.Header = It & " - " & xx.Content.ToString


            Dim TypeAllTests As Boolean = False
            If Val(bm.ExecuteScalar("Select AllTests From LaboratoryTests where TestId=" & WTypes.Tag.ToString & " and Id=" & xx.Tag.ToString)) = 1 Then
                TypeAllTests = True
            End If


            Dim dt As DataTable = bm.ExecuteAdapter("Select * From LabTestItems where TestId=" & WTypes.Tag.ToString & " and SubTestId=" & xx.Tag.ToString & " order by Id")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x, 370)
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WItems.Children.Add(x)
                AddHandler x.Click, AddressOf TabItem
                If AllTests OrElse TypeAllTests Then
                    TabItem(x, Nothing)
                End If
            Next
        Catch
        End Try
    End Sub

    Private Sub TabItem(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        AddItem(x.Tag, x.Content)
    End Sub

    Sub AddItem(ByVal Id As String, Nam As String)
        Try
            Dim Exists As Boolean = False
            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            Dim i As Integer = -1
            For x As Integer = 0 To G.Rows.Count - 1
                If Not G.Rows(x).Cells(GC.TestId).Value Is Nothing AndAlso G.Rows(x).Cells(GC.TestId).Value.ToString = CurrentGroup AndAlso Not G.Rows(x).Cells(GC.SubTestId).Value Is Nothing AndAlso G.Rows(x).Cells(GC.SubTestId).Value.ToString = CurrentType AndAlso Not G.Rows(x).Cells(GC.Id).Value Is Nothing AndAlso G.Rows(x).Cells(GC.Id).Value.ToString = Id.ToString Then
                    i = x
                    Exists = True
                    Exit For
                End If
            Next
            If i = -1 Then i = G.Rows.Add()

            G.Rows(i).Cells(GC.TestId).Value = CurrentGroup
            G.Rows(i).Cells(GC.TestName).Value = CurrentGroupName
            G.Rows(i).Cells(GC.SubTestId).Value = CurrentType
            G.Rows(i).Cells(GC.SubTestName).Value = CurrentTypeName
            G.Rows(i).Cells(GC.Id).Value = Id
            G.Rows(i).Cells(GC.Name).Value = Nam
            G.Rows(i).Cells(GC.Unit).Value = bm.ExecuteScalar("select dbo.GetLabTestItemsUnit('" & CurrentGroup & "','" & CurrentType & "','" & Id & "')")

            G.Focus()
            G.Rows(i).Selected = True
            G.FirstDisplayedScrollingRowIndex = i
            G.CurrentCell = G.Rows(i).Cells(GC.Result)
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            G.BeginEdit(True)
        Catch
        End Try
        Grid_SelectionChanged(Nothing, Nothing)
    End Sub

    Dim lop As Boolean = False
    Sub FillControls()
        If lop Then Return
        lop = True
        UndoNewId()
        bm.FillControls(Me)
        CaseID_LostFocus(Nothing, Nothing)
        If Not Md.MyProjectType = ProjectType.X Then RInvoiceNo_LostFocus(Nothing, Nothing)
        Dim dt As DataTable = bm.ExecuteAdapter("select *,dbo.GetLaboratoryTestTypesName(TestId) TestName,dbo.GetLaboratoryTestsName(TestId,SubTestId) SubTestName,dbo.GetLabTestItemsName(TestId,SubTestId,Id) ItemName,dbo.GetLabTestItemsUnit(TestId,SubTestId,Id)Unit from LabTestsDt where InvoiceNo=" & InvoiceNo.Text & " order by TestId,SubTestId,Id")

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.TestId).Value = dt.Rows(i)("TestId").ToString
            G.Rows(i).Cells(GC.TestName).Value = dt.Rows(i)("TestName").ToString
            G.Rows(i).Cells(GC.SubTestId).Value = dt.Rows(i)("SubTestId").ToString
            G.Rows(i).Cells(GC.SubTestName).Value = dt.Rows(i)("SubTestName").ToString
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("Id").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("ItemName").ToString
            G.Rows(i).Cells(GC.Result).Value = dt.Rows(i)("Result").ToString
            G.Rows(i).Cells(GC.Unit).Value = dt.Rows(i)("Unit").ToString
            G.Rows(i).Cells(GC.NormalValue).Value = dt.Rows(i)("NormalValue").ToString
        Next
        Grid_SelectionChanged(Nothing, Nothing)
        FinalStudy.Focus()
        G.RefreshEdit()
        lop = False
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {InvoiceNo.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPrint.Click, btnPrint2.Click, btnPrint3.Click

        If ReferredBy.Visibility = Visibility.Visible AndAlso ReferredBy.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد الجهة")
            ReferredBy.Focus()
            Return
        End If
        If CaseId.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد المريض")
            CaseId.Focus()
            Return
        End If
        If RInvoiceNo.Visibility = Visibility.Visible AndAlso Val(InvoiceNo.Text) = 0 And Val(RInvoiceNo.Text) = 0 And Not Md.Manager Then
            bm.ShowMSG("برجاء تحديد رقم الإيصال")
            RInvoiceNo.Focus()
            Return
        End If


        G.EndEdit()

        If Not sender Is btnPrint3 Then
            For x As Integer = 0 To G.Rows.Count - 1
                If Not G.Rows(x).Cells(GC.TestId).Value Is Nothing AndAlso G.Rows(x).Cells(GC.Result).Value = "" Then
                    bm.ShowMSG("برجاء تحديد النتيجة بالسطر رقم " & (x + 1).ToString)
                    G.CurrentCell = G.Rows(x).Cells(GC.Result)
                    G.BeginEdit(True)
                    G.Focus()
                    Return
                End If
            Next
        End If



        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text
            
            
            
            State = BasicMethods.SaveState.Insert
        End If

        EmpId.SelectedValue = Md.UserName
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {InvoiceNo.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                InvoiceNo.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G, "LabTestsDt", New String() {"InvoiceNo"}, New String() {InvoiceNo.Text}, New String() {"TestId", "SubTestId", "Id", "Result", "NormalValue"}, New String() {GC.TestId, GC.SubTestId, GC.Id, GC.Result, GC.NormalValue}, New VariantType() {VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.String}, New String() {GC.TestId, GC.SubTestId, GC.Id}) Then Return

        If sender Is btnPrint Then
            PrintPone()
        ElseIf sender Is btnPrint2 Then
            PrintPone(True)
            Return
        ElseIf sender Is btnPrint3 Then
            PrintPone(False, True)
            Return
        End If

        If Not DontClear Then btnNew_Click(sender, e)
        
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
        DayDate.Focus()
    End Sub

    Sub ClearControls()
        Try
            NewId()
            bm.ClearControls(False)
            EmpId.SelectedValue = Md.UserName

            Dim MyNow As DateTime = bm.MyGetDate()
            DayDate.Text = MyNow.Date

            CaseID_LostFocus(Nothing, Nothing)
            CaseId.IsEnabled = True
            G.Rows.Clear()
            Grid_SelectionChanged(Nothing, Nothing)
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "'   delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub


    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {InvoiceNo.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {InvoiceNo.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyDown, InvoiceNo.KeyDown, RInvoiceNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    
    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub PrintPone(Optional View As Boolean = False, Optional Blank As Boolean = False)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@InvoiceNo"}
        rpt.Header = Md.MyProjectType.ToString

        rpt.paravalue = New String() {InvoiceNo.Text}
        rpt.Rpt = "LabTests.rpt"
        If View Then
            rpt.Show()
        ElseIf Blank Then
            rpt.Rpt = "LabTestsBlank.rpt"
            rpt.Print(, , 1)
        Else
            rpt.Print(, , 1)
        End If

    End Sub

    Dim DontClear As Boolean = False

    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        RInvoiceNo.SetResourceReference(ContentProperty, "R. Invoice No")
    End Sub

    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CaseId.KeyUp
        If bm.ShowHelpCases(CaseId, CaseName, e, True, False) Then
            CaseID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select " & Resources.Item("CboName") & " Name from Cases where Id=" & CaseId.Text.Trim() & " and InOut=1")
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile,Gender,DateOfBirth from Cases where Id=" & CaseId.Text.Trim() & " and InOut=1")
        If dt.Rows.Count > 0 Then
            CaseId.ToolTip = Resources.Item("Id") & ": " & CaseId.Text & vbCrLf & Resources.Item("Name") & ": " & CaseName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
            CaseName.ToolTip = CaseId.ToolTip
            Gender.SelectedIndex = 0
            DateOfBirth.SelectedDate = Nothing
            Try
                Gender.SelectedValue = Val(dt.Rows(0)("Gender"))
                DateOfBirth.SelectedDate = dt.Rows(0)("DateOfBirth")
            Catch
            End Try
        End If
    End Sub

    Private Sub Grid_CellEndEdit(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(GC.Result).Index = e.ColumnIndex Then
                If G.Rows(e.RowIndex).Cells(GC.TestId).Value Is Nothing Then
                    G.Rows.RemoveAt(e.RowIndex)
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub Grid_SelectionChanged(sender As Object, e As EventArgs)
        Try
            WResult.Children.Clear()
            WNormalValue.Children.Clear()
            Dim gr As String = G.CurrentRow.Cells(GC.TestId).Value
            Dim Tp As String = G.CurrentRow.Cells(GC.SubTestId).Value
            Dim It As String = G.CurrentRow.Cells(GC.Id).Value

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From LabTestItemsResults where TestId=" & gr & " and SubTestId=" & Tp & " and Id=" & It)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x, 95, 30)
                x.Content = dt.Rows(i)("Result").ToString
                x.ToolTip = dt.Rows(i)("Result").ToString
                WResult.Children.Add(x)
                AddHandler x.Click, AddressOf WResult_Click
            Next
            G.CurrentRow.Cells(GC.NormalValue).ReadOnly = False
            WNormalValue.IsEnabled = True
            Dim dt0 As DataTable = bm.ExecuteAdapter("Select * From LabTestItemsNormalValues where TestId=" & gr & " and SubTestId=" & Tp & " and Id=" & It)
            For i As Integer = 0 To dt0.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x, 95, 30)
                x.Content = dt0.Rows(i)("NormalValue").ToString
                x.ToolTip = dt0.Rows(i)("NormalValue").ToString
                WNormalValue.Children.Add(x)
                AddHandler x.Click, AddressOf WNormalValue_Click
            Next
            Dim ss As String = bm.ExecuteScalar("Select RefrenceRange From LabTestItems where TestId=" & gr & " and SubTestId=" & Tp & " and Id=" & It)
            If ss = 1 Then
                G.CurrentRow.Cells(GC.NormalValue).ReadOnly = True
                WNormalValue.IsEnabled = False
            End If

        Catch
        End Try
    End Sub

    Private Sub WResult_Click(sender As Object, e As RoutedEventArgs)
        Try
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            G.CurrentRow.Cells(GC.Result).Value = CType(sender, Button).Content
            G.RefreshEdit()
            Dim i As Integer = G.CurrentRow.Index
            G.Focus()
            G.CurrentCell = G.Rows(i).Cells(GC.Name)
            G.CurrentCell = G.Rows(i).Cells(GC.Result)
            G.BeginEdit(True)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub WNormalValue_Click(sender As Object, e As RoutedEventArgs)
        Try
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            G.CurrentRow.Cells(GC.NormalValue).Value = CType(sender, Button).Content
            G.RefreshEdit()
            Dim i As Integer = G.CurrentRow.Index
            G.Focus()
            G.CurrentCell = G.Rows(i).Cells(GC.Name)
            G.CurrentCell = G.Rows(i).Cells(GC.NormalValue)
            G.BeginEdit(True)
        Catch ex As Exception
        End Try
    End Sub

    Dim SearchLop As Boolean = False
    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnSearch.Click
        SearchLop = True
        bm.DefineValues()
        bm.SearchTable(New String() {SubId}, New String() {InvoiceNo.Text.Trim}, cboSearch, "'( '+cast(InvoiceNo as varchar(100))+' )    '+dbo.ToStrDate(DayDate)", New Control() {CaseId})
        SearchLop = False
    End Sub

    Private Sub cboSearch_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSearch.SelectionChanged
        If SearchLop Then Return
        InvoiceNo.Text = cboSearch.SelectedValue.ToString
        txtID_Leave(Nothing, Nothing)
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As RoutedEventArgs) Handles btnUpdate.Click
        bm.ExecuteNonQuery("update Cases set Gender='" & Gender.SelectedValue.ToString & "',DateOfBirth='" & bm.ToStrDate(DateOfBirth.SelectedDate) & "' where Id='" & CaseId.Text & "'")
        bm.ShowMSG("تم التعديل بنجاح")
    End Sub

    Private Sub RInvoiceNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles RInvoiceNo.LostFocus
        If lop Then Return
        RInvoiceNo.ToolTip = ""
        If RInvoiceNo.Text.Trim = "" Then
            CaseId.IsEnabled = True
            Return
        End If

        Dim dt As DataTable = bm.ExecuteAdapter("GetLabService", New String() {"InvoiceNo", "SerialId"}, New String() {Val(InvoiceNo.Text), Val(RInvoiceNo.Text)})
        If dt.Rows.Count = 0 Then
            RInvoiceNo.Clear()
            CaseId.IsEnabled = True
            CaseId.Clear()
            CaseID_LostFocus(Nothing, Nothing)
            Return
        End If
        CaseId.IsEnabled = Md.Manager
        CaseId.Text = dt.Rows(0)(0).ToString
        RInvoiceNo.ToolTip = "القيمة : " & dt.Rows(0)(1).ToString
        CaseID_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub ViewHistory_Click(sender As Object, e As RoutedEventArgs) Handles ViewHistory.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@CaseId", "CaseName", "@Flag", "@MainId", "@DayDate", "@Id", "Header"}
        rpt.paravalue = New String() {Val(CaseId.Text), CaseName.Text, -2, 0, bm.ToStrDate(Now.Date), 0, "Patient History"}
        rpt.Rpt = "CaseAllDetails.rpt"
        rpt.Show()
    End Sub

End Class
