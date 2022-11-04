Imports System.Data
Imports System.Windows.Forms

Public Class BankCash_G2
    Public TableName As String = "BankCash_G2"
    Public MainId As String = "BankCash_G2TypeId"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G1 As New MyGrid
    Public Flag As Integer = 0
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {btnPrint, btnPrint2, btnPrint3})
        If Not Md.Manager Then
            'btnDelete.Visibility = Visibility.Hidden
            'btnFirst.Visibility = Visibility.Hidden
            'btnPrevios.Visibility = Visibility.Hidden
            'btnNext.Visibility = Visibility.Hidden
            'btnLast.Visibility = Visibility.Hidden

            'btnPrint.Visibility = Visibility.Hidden
            'btnPrint2.Visibility = Visibility.Hidden

            'DayDate.IsEnabled = False
        End If

        lblNotes.Visibility = Visibility.Hidden
        Notes.Visibility = Visibility.Hidden
        GroupBoxDed.Visibility = Visibility.Hidden
        btnPrint2.Visibility = Visibility.Hidden
        bm.Addcontrol_MouseDoubleClick({CheckNo, CheckBankId})


        bm.Fields = {MainId, SubId, SubId2, "MainLinkFile", "BankId", "DayDate", "Canceled", "CurrencyId", "SystemUser", "IsPosted"}
        bm.control = {BankCash_G2TypeId, txtFlag, txtID, MainLinkFile, BankId, DayDate, Canceled, CurrencyId, SystemUser, IsPosted}
        bm.KeyFields = {MainId, SubId, SubId2}
        bm.Table_Name = TableName

        txtFlag.Text = Flag


        Dim x As Integer = Val(GetSetting("OMEGA", "BankCash_G2TypeId", Flag))
        bm.FillCombo("GetEmpBankCash_G2Types @Flag=" & Flag & ",@EmpId=" & Md.UserName & "", BankCash_G2TypeId)
        BankCash_G2TypeId.SelectedValue = x

        bm.FillCombo("CheckTypes", CheckTypeId, "", , True, True)
        bm.FillCombo("LinkFile", MainLinkFile, " where ShowInBankCash_G2=1", , True)
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        bm.FillCombo("Employees", SystemUser, "", , True)
        If Not Md.ShowCurrency Then
            lblCurrencyId.Visibility = Visibility.Hidden
            CurrencyId.Visibility = Visibility.Hidden
        End If
        If Not Md.ShowCostCenter Then
            CostCenterName.Visibility = Visibility.Hidden
        End If
        LoadResource()
        LoadWFH(WFH1, G1)

        SaveSetting("OMEGA", "BankCash_G2TypeId", Flag, x)
        btnNew_Click(sender, e)

        If Md.MyProjectType = ProjectType.X Then
            CurrencyId.IsEnabled = True
            btnLast_Click(sender, e)
        Else
            btnEdit.Visibility = Visibility.Hidden
        End If


        If Not Md.MyProjectType = ProjectType.X Then
            btnPrint3.Visibility = Visibility.Hidden
            btnIsSelected.Visibility = Visibility.Hidden
        End If

        BankId.Focus()
    End Sub



    Structure GC
        Shared IsSelected As String = "IsSelected"
        Shared DocNo As String = "DocNo"
        Shared MainValue As String = "MainValue"
        Shared Exchange As String = "Exchange"
        Shared Value As String = "Value"
        Shared LinkFile As String = "LinkFile"
        Shared MainAccNo As String = "MainAccNo"
        Shared MainAccName As String = "MainAccName"
        Shared SubAccNo As String = "SubAccNo"
        Shared SubAccName As String = "SubAccName"

        Shared CurrencyId2 As String = "CurrencyId2"
        Shared MainValue2 As String = "MainValue2"
        Shared Exchange2 As String = "Exchange2"

        Shared AnalysisId As String = "AnalysisId"
        Shared AnalysisName As String = "AnalysisName"
        Shared CostCenterId As String = "CostCenterId"
        Shared CostCenterName As String = "CostCenterName"
        Shared CostCenterSubId As String = "CostCenterSubId"
        Shared CostCenterSubName As String = "CostCenterSubName"

        Shared CostTypeId As String = "CostTypeId"
        Shared PurchaseAccNo As String = "PurchaseAccNo"
        Shared ImportMessageId As String = "ImportMessageId"
        Shared StoreId As String = "StoreId"
        Shared StoreInvoiceNo As String = "StoreInvoiceNo"

        Shared Notes As String = "Notes"
        Shared CheckTypeId As String = "CheckTypeId"
        Shared CheckNo As String = "CheckNo"
        Shared CheckDate As String = "CheckDate"
        Shared CheckBankId As String = "CheckBankId"
        Shared CheckPersonName As String = "CheckPersonName"

        Shared MainValue2Ded As String = "MainValue2Ded"
        Shared Value2Ded As String = "Value2Ded"
        Shared DedNotes As String = "DedNotes"
        Shared Line As String = "Line"
        Shared IsDocumented As String = "IsDocumented"
        Shared IsNotDocumented As String = "IsNotDocumented"
    End Structure


    Private Sub LoadWFH(WFH As Forms.Integration.WindowsFormsHost, G As MyGrid)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        Dim GCIsSelected As New Forms.DataGridViewCheckBoxColumn
        GCIsSelected.HeaderText = "تحديد"
        GCIsSelected.Name = GC.IsSelected
        G.Columns.Add(GCIsSelected)

        G.Columns.Add(GC.DocNo, "رقم المستند")
        G.Columns.Add(GC.MainValue, "القيمة")
        G.Columns.Add(GC.Exchange, "سعر الصرف")
        G.Columns.Add(GC.Value, "المعادل بالعملة المحلية")

        Dim GCLinkFile As New Forms.DataGridViewComboBoxColumn
        GCLinkFile.HeaderText = "الجهة"
        GCLinkFile.Name = GC.LinkFile
        bm.FillCombo("select Id,Name from LinkFile union all select 0 Id,'-' Name order by Id", GCLinkFile)
        G.Columns.Add(GCLinkFile)

        If Md.ShowGridAccCombo Then
            bm.MakeGridCombo(G, "الحساب", GC.MainAccNo, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from Chart where SubType=1 union all select 0 Id,'-' Name order by Id")
        Else
            G.Columns.Add(GC.MainAccNo, "الحساب")
        End If
        G.Columns.Add(GC.MainAccName, "اسم الحساب")

        If Md.ShowGridAccCombo Then
            bm.MakeGridCombo(G, "الفرعى", GC.SubAccNo, "select 0 Id,'-' Name order by Id")
        Else
            G.Columns.Add(GC.SubAccNo, "الفرعى")
        End If
        G.Columns.Add(GC.SubAccName, "اسم الفرعى")

        Dim GCCurrencyId2 As New Forms.DataGridViewComboBoxColumn
        GCCurrencyId2.HeaderText = "عملة الفرعي"
        GCCurrencyId2.Name = GC.CurrencyId2
        bm.FillCombo("select Id,Name from Currencies order by Id", GCCurrencyId2)
        G.Columns.Add(GCCurrencyId2)
        G.Columns.Add(GC.MainValue2, "القيمة لدى الفرعي")
        G.Columns.Add(GC.Exchange2, "سعر الصرف بالعملة المحلية")

        If Md.ShowGridAccCombo Then
            bm.MakeGridCombo(G, "التحليلي", GC.AnalysisId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from Analysis union all select 0 Id,'-' Name order by Id")
        Else
            G.Columns.Add(GC.AnalysisId, "التحليلي")
        End If
        G.Columns.Add(GC.AnalysisName, "اسم التحليلي")

        If Md.ShowGridAccCombo Then
            bm.MakeGridCombo(G, "م. التكلفة", GC.CostCenterId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from CostCenters where SubType=1 union all select 0 Id,'-' Name order by Id")
        Else
            G.Columns.Add(GC.CostCenterId, "م. التكلفة")
        End If
        G.Columns.Add(GC.CostCenterName, "اسم م. التكلفة")

        If Md.ShowGridAccCombo Then
            bm.MakeGridCombo(G, "عنصر التكلفة", GC.CostCenterSubId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from CostCenterSubs where SubType=1 union all select 0 Id,'-' Name order by Id")
        Else
            G.Columns.Add(GC.CostCenterSubId, "عنصر التكلفة")
        End If
        G.Columns.Add(GC.CostCenterSubName, "اسم عنصر التكلفة")

        Dim GCCostTypeId As New Forms.DataGridViewComboBoxColumn
        GCCostTypeId.HeaderText = "نوع التكلفة"
        GCCostTypeId.Name = GC.CostTypeId
        bm.FillCombo("select Id,Name from CostTypes union all select 0 Id,'-' Name order by Id", GCCostTypeId)
        G.Columns.Add(GCCostTypeId)

        G.Columns.Add(GC.PurchaseAccNo, "الطلبية")
        G.Columns.Add(GC.ImportMessageId, "الرسالة")
        G.Columns.Add(GC.StoreId, "المخزن")
        G.Columns.Add(GC.StoreInvoiceNo, "مسلسل الفاتورة")

        G.Columns.Add(GC.Notes, "البيان")

        G.Columns.Add(GC.CheckTypeId, "CheckTypeId")
        G.Columns.Add(GC.CheckNo, "CheckNo")
        G.Columns.Add(GC.CheckDate, "CheckDate")
        G.Columns.Add(GC.CheckBankId, "CheckBankId")
        G.Columns.Add(GC.CheckPersonName, "CheckPersonName")

        G.Columns.Add(GC.MainValue2Ded, bm.ExecuteScalar("select dbo.GetAccName((select BankCash_G2_Flag" & Flag & "_DedAcc from Statics))"))
        G.Columns.Add(GC.Value2Ded, "المعادل")
        G.Columns.Add(GC.DedNotes, "البيان")
        G.Columns.Add(GC.Line, "Line")
        G.Columns.Add(GC.IsDocumented, "مؤيد")
        G.Columns.Add(GC.IsNotDocumented, "غير مؤيد")

        G.Columns(GC.CheckTypeId).Visible = False
        G.Columns(GC.CheckNo).Visible = False
        G.Columns(GC.CheckDate).Visible = False
        G.Columns(GC.CheckBankId).Visible = False
        G.Columns(GC.CheckPersonName).Visible = False


        G.Columns(GC.MainValue2Ded).Visible = False
        G.Columns(GC.Value2Ded).Visible = False
        G.Columns(GC.DedNotes).Visible = False

        G.Columns(GC.Value).ReadOnly = True
        G.Columns(GC.SubAccName).ReadOnly = True

        If Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.Value).ReadOnly = False
        Else
            G.Columns(GC.IsDocumented).Visible = False
            G.Columns(GC.IsNotDocumented).Visible = False
        End If

        SetGCExchange(G)

        If Not Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.IsSelected).Visible = False
        End If

        If Not Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.CurrencyId2).ReadOnly = True
            G.Columns(GC.Exchange2).ReadOnly = True
        End If


        If Md.ShowGridAccCombo Then
        Else
            G.Columns(GC.MainAccNo).FillWeight = 80
            G.Columns(GC.SubAccNo).FillWeight = 80
            G.Columns(GC.AnalysisId).FillWeight = 80
            G.Columns(GC.CostCenterId).FillWeight = 80
            G.Columns(GC.CostCenterSubId).FillWeight = 80
        End If

        G.Columns(GC.MainAccName).FillWeight = 200
        G.Columns(GC.SubAccName).FillWeight = 200
        G.Columns(GC.AnalysisName).FillWeight = 200
        G.Columns(GC.CostCenterName).FillWeight = 200
        G.Columns(GC.CostCenterSubName).FillWeight = 200

        G.Columns(GC.CurrencyId2).FillWeight = 140

        G.Columns(GC.CostTypeId).FillWeight = 240
        G.Columns(GC.SubAccName).FillWeight = 240
        G.Columns(GC.Notes).FillWeight = 300
        G.Columns(GC.IsSelected).FillWeight = 80
        G.Columns(GC.DocNo).FillWeight = 100
        G.Columns(GC.DedNotes).FillWeight = 200

        G.Columns(GC.MainAccName).ReadOnly = True
        G.Columns(GC.SubAccName).ReadOnly = True
        G.Columns(GC.AnalysisName).ReadOnly = True
        G.Columns(GC.CostCenterName).ReadOnly = True
        G.Columns(GC.CostCenterSubName).ReadOnly = True
        G.Columns(GC.Value2Ded).ReadOnly = True

        If Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.DocNo).ReadOnly = True
        End If

        If Md.HideSubAccNo Then
            G.Columns(GC.SubAccNo).Visible = False
        End If

        If Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.DocNo).Visible = False
            G.Columns(GC.CurrencyId2).Visible = False
            G.Columns(GC.MainValue2).Visible = False
            G.Columns(GC.Exchange2).Visible = False
        End If

        If Md.ShowBankCash_GAccNo_Not_LinkFile Then
            G.Columns(GC.LinkFile).Visible = False
            G.Columns(GC.MainAccNo).Visible = True
            If Md.ShowGridAccNames Then
                G.Columns(GC.MainAccName).Visible = True
                MainAccName.Visibility = Visibility.Hidden
            Else
                G.Columns(GC.MainAccName).Visible = False
                MainAccName.Visibility = Visibility.Visible
            End If
        Else
            G.Columns(GC.LinkFile).Visible = True
            G.Columns(GC.MainAccNo).Visible = False
            G.Columns(GC.MainAccName).Visible = False
            MainAccName.Visibility = Visibility.Hidden
        End If

        If Md.ShowGridAccNames Then
            G.Columns(GC.SubAccName).Visible = True
            SubAccName.Visibility = Visibility.Hidden
        Else
            G.Columns(GC.SubAccName).Visible = False
            SubAccName.Visibility = Visibility.Visible
        End If


        If Not Md.ShowCurrency Then
            G.Columns(GC.CurrencyId2).Visible = False
            G.Columns(GC.Exchange).Visible = False
            G.Columns(GC.Exchange2).Visible = False
            G.Columns(GC.Value).Visible = False
            G.Columns(GC.MainValue2).Visible = False
        End If

        If Md.ShowAnalysis Then
            G.Columns(GC.AnalysisId).Visible = True
            G.Columns(GC.AnalysisName).Visible = Md.ShowGridAccNames
            AnalysisName.Visibility = IIf(Md.ShowGridAccNames, Visibility.Hidden, Visibility.Visible)
        Else
            G.Columns(GC.AnalysisId).Visible = False
            G.Columns(GC.AnalysisName).Visible = False
            AnalysisName.Visibility = Visibility.Hidden
        End If

        If Md.ShowCostCenter Then
            G.Columns(GC.CostCenterId).Visible = True
            G.Columns(GC.CostCenterName).Visible = Md.ShowGridAccNames
            CostCenterName.Visibility = IIf(Md.ShowGridAccNames, Visibility.Hidden, Visibility.Visible)
        Else
            G.Columns(GC.CostCenterId).Visible = False
            G.Columns(GC.CostCenterName).Visible = False
            CostCenterName.Visibility = Visibility.Hidden
        End If

        If Md.ShowCostCenterSub Then
            G.Columns(GC.CostCenterSubId).Visible = True
            G.Columns(GC.CostCenterSubName).Visible = Md.ShowGridAccNames
            CostCenterSubName.Visibility = IIf(Md.ShowGridAccNames, Visibility.Hidden, Visibility.Visible)
        Else
            G.Columns(GC.CostCenterSubId).Visible = False
            G.Columns(GC.CostCenterSubName).Visible = False
            CostCenterSubName.Visibility = Visibility.Hidden
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.CostTypeId).Visible = True
            G.Columns(GC.PurchaseAccNo).Visible = True
            G.Columns(GC.ImportMessageId).Visible = True
            G.Columns(GC.StoreId).Visible = True
            G.Columns(GC.StoreInvoiceNo).Visible = True
            PurchaseAccName.Visibility = Visibility.Visible
            ImportMessageName.Visibility = Visibility.Visible
            StoreName.Visibility = Visibility.Visible
        Else
            G.Columns(GC.CostTypeId).Visible = False
            G.Columns(GC.PurchaseAccNo).Visible = False
            G.Columns(GC.ImportMessageId).Visible = False
            G.Columns(GC.StoreId).Visible = False
            G.Columns(GC.StoreInvoiceNo).Visible = False
            PurchaseAccName.Visibility = Visibility.Hidden
            ImportMessageName.Visibility = Visibility.Hidden
            StoreName.Visibility = Visibility.Hidden
        End If

        If Md.ShowGridAccCombo Then
            G.Columns(GC.MainAccName).Visible = False
            G.Columns(GC.SubAccName).Visible = False
            G.Columns(GC.AnalysisName).Visible = False
            G.Columns(GC.CostCenterName).Visible = False
            G.Columns(GC.CostCenterSubName).Visible = False
        End If

        G.Columns(GC.Line).Visible = False

        If Md.MyProjectType = ProjectType.X Then
            G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
            For i As Integer = 0 To G.ColumnCount - 1
                G.Columns(i).Width = G.Columns(i).FillWeight
            Next
        End If

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.CellBeginEdit, AddressOf G_CellBeginEdit
        AddHandler G.SelectionChanged, AddressOf G_SelectionChanged
        AddHandler G.RowsAdded, AddressOf G_RowsAdded
    End Sub

    Private Sub G_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs)
        Try
            G1.CurrentRow.Cells(GC.CostCenterId).Value = Md.CostCenterId
        Catch
        End Try
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim IsNew As Boolean = False
    Sub FillControls()
        If lop Then Return
        lop = True

        If Md.MyProjectType = ProjectType.X Then
            G1.ReadOnly = True
        End If

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True

        bm.FillControls(Me)
        BankId_LostFocus(Nothing, Nothing)

        FillGrid(G1, TableName)

        DayDate.Focus()
        lop = False
        CalcTotal()

        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

    End Sub

    Sub FillGrid(G As MyGrid, MyTableName As String)

        Dim dt As DataTable = bm.ExecuteAdapter("select *,dbo.GetAccName(MainAccNo)MainAccName,dbo.GetAnalysisName(Analysisid)AnalysisName,dbo.GetCostCenterName(CostCenterId)CostCenterName,dbo.GetCostCenterSubName(CostCenterSubId)CostCenterSubName from " & MyTableName & " where " & MainId & "=" & BankCash_G2TypeId.SelectedValue.ToString & " and " & SubId & "=" & txtFlag.Text.Trim & " and " & SubId2 & "=" & txtID.Text)

        IsNew = True
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            IsNew = False
            G.Rows.Add()
            G.Rows(i).Cells(GC.MainValue).Value = dt.Rows(i)("MainValue").ToString
            G.Rows(i).Cells(GC.Exchange).Value = dt.Rows(i)("Exchange").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Rows(i).Cells(GC.LinkFile).Value = dt.Rows(i)("LinkFile").ToString
            G.Rows(i).Cells(GC.MainAccNo).Value = dt.Rows(i)("MainAccNo").ToString
            lop = False
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.MainAccNo).Index, i))
            G.Rows(i).Cells(GC.MainAccNo).Value = dt.Rows(i)("MainAccNo").ToString
            lop = True
            G.Rows(i).Cells(GC.MainAccName).Value = dt.Rows(i)("MainAccName").ToString
            G.Rows(i).Cells(GC.SubAccNo).Value = dt.Rows(i)("SubAccNo") '.ToString
            lop = False
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, i))
            lop = True
            G.Rows(i).Cells(GC.AnalysisId).Value = dt.Rows(i)("AnalysisId").ToString
            G.Rows(i).Cells(GC.AnalysisName).Value = dt.Rows(i)("AnalysisName").ToString
            G.Rows(i).Cells(GC.CostCenterId).Value = dt.Rows(i)("CostCenterId").ToString
            G.Rows(i).Cells(GC.CostCenterName).Value = dt.Rows(i)("CostCenterName").ToString
            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.CostCenterId).Index, i))
            G.Rows(i).Cells(GC.CostCenterSubId).Value = dt.Rows(i)("CostCenterSubId").ToString
            G.Rows(i).Cells(GC.CostCenterSubName).Value = dt.Rows(i)("CostCenterSubName").ToString
            G.Rows(i).Cells(GC.CostTypeId).Value = dt.Rows(i)("CostTypeId").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
            G.Rows(i).Cells(GC.IsSelected).Value = IIf(dt.Rows(i)("IsSelected") = 1, True, False)
            G.Rows(i).Cells(GC.DocNo).Value = dt.Rows(i)("DocNo").ToString
            G.Rows(i).Cells(GC.CheckTypeId).Value = dt.Rows(i)("CheckTypeId").ToString
            G.Rows(i).Cells(GC.CheckNo).Value = dt.Rows(i)("CheckNo").ToString
            G.Rows(i).Cells(GC.CheckDate).Value = dt.Rows(i)("CheckDate").ToString
            G.Rows(i).Cells(GC.CheckBankId).Value = dt.Rows(i)("CheckBankId").ToString
            G.Rows(i).Cells(GC.CheckPersonName).Value = dt.Rows(i)("CheckPersonName").ToString

            G.Rows(i).Cells(GC.CurrencyId2).Value = dt.Rows(i)("CurrencyId2").ToString
            G.Rows(i).Cells(GC.MainValue2).Value = dt.Rows(i)("MainValue2").ToString
            G.Rows(i).Cells(GC.Exchange2).Value = dt.Rows(i)("Exchange2").ToString

            G.Rows(i).Cells(GC.PurchaseAccNo).Value = dt.Rows(i)("PurchaseAccNo").ToString
            G.Rows(i).Cells(GC.ImportMessageId).Value = dt.Rows(i)("ImportMessageId").ToString
            G.Rows(i).Cells(GC.StoreId).Value = dt.Rows(i)("StoreId").ToString
            G.Rows(i).Cells(GC.StoreInvoiceNo).Value = dt.Rows(i)("StoreInvoiceNo").ToString

            G.Rows(i).Cells(GC.MainValue2Ded).Value = dt.Rows(i)("MainValue2Ded").ToString
            G.Rows(i).Cells(GC.Value2Ded).Value = dt.Rows(i)("Value2Ded").ToString
            G.Rows(i).Cells(GC.DedNotes).Value = dt.Rows(i)("DedNotes").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            G.Rows(i).Cells(GC.IsDocumented).Value = dt.Rows(i)("IsDocumented").ToString
            G.Rows(i).Cells(GC.IsNotDocumented).Value = dt.Rows(i)("IsNotDocumented").ToString


        Next
        Try
            If G.Rows(dt.Rows.Count - 1).Cells(GC.MainAccNo).Visible Then
                G.CurrentCell = G.Rows(dt.Rows.Count - 1).Cells(GC.MainAccNo)
                G.RefreshEdit()
            End If
        Catch ex As Exception
        End Try


    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not btnSave.IsEnabled Then Return

        AllowSave = False
        If BankCash_G2TypeId.SelectedValue Is Nothing OrElse BankCash_G2TypeId.SelectedValue < 1 Then
            bm.ShowMSG("برجاء تحديد اليومية")
            BankCash_G2TypeId.Focus()
            Return
        End If
        If Val(txtID.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المسلسل")
            txtID.Focus()
            Return
        End If
        If Val(BankId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblBank.Content)
            BankId.Focus()
            Return
        End If

        G1.EndEdit()

        For i As Integer = 0 To G1.Rows.Count - 1
            If Val(G1.Rows(i).Cells(GC.MainValue).Value) = 0 Then
                Continue For
            End If

            If Md.ShowBankCash_GAccNo_Not_LinkFile AndAlso Val(G1.Rows(i).Cells(GC.MainAccNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد الحساب بالسطر " & (i + 1).ToString)
                G1.Focus()
                G1.CurrentCell = G1.Rows(i).Cells(GC.MainAccNo)
                Return
            ElseIf Not Md.HideSubAccNo AndAlso Not G1.Rows(i).Cells(GC.SubAccNo).ReadOnly AndAlso Val(G1.Rows(i).Cells(GC.SubAccNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد الفرعى بالسطر " & (i + 1).ToString)
                G1.Focus()
                G1.CurrentCell = G1.Rows(i).Cells(GC.SubAccNo)
                Return
            End If
            If Val(G1.Rows(i).Cells(GC.CheckNo).Value) = 0 AndAlso Val(G1.Rows(i).Cells(GC.CheckTypeId).Value) <> 3 AndAlso Val(G1.Rows(i).Cells(GC.CheckTypeId).Value) > 1 Then
                bm.ShowMSG("برجاء تحديد رقم الشيك بالسطر " & (i + 1).ToString)
                G1.Focus()
                G1.CurrentCell = G1.Rows(i).Cells(GC.Notes)
                G1.CurrentCell = G1.Rows(i).Cells(GC.DocNo)
                Return
            End If

            If Val(G1.Rows(i).Cells(GC.CheckTypeId).Value) = 2 AndAlso bm.IF_Exists("select * from BankCash_G2 where CheckTypeId=2 and CheckNo='" & G1.Rows(i).Cells(GC.CheckNo).Value & "' and not(Flag=" & Flag & " and BankCash_G2TypeId=" & BankCash_G2TypeId.SelectedValue & " and InvoiceNo=" & Val(txtID.Text) & ")") Then
                bm.ShowMSG("برجاء اختيار رقم شيك آخر بالسطر " & (i + 1).ToString)
                G1.Focus()
                G1.CurrentCell = G1.Rows(i).Cells(GC.Notes)
                G1.CurrentCell = G1.Rows(i).Cells(GC.DocNo)
                Return
            End If

            If Md.MyProjectType = ProjectType.X AndAlso G1.Columns(GC.CostTypeId).Visible AndAlso Val(G1.Rows(i).Cells(GC.LinkFile).Value) = 9 AndAlso Val(G1.Rows(i).Cells(GC.CostTypeId).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد نوع التكلفة بالسطر " & (i + 1).ToString)
                G1.Focus()
                G1.CurrentCell = G1.Rows(i).Cells(GC.CostTypeId)
                Return
            End If

        Next

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If



        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        'If IsNew Then State = BasicMethods.SaveState.Insert

        SystemUser.SelectedValue = Md.UserName
        bm.DefineValues()

        If Not bm.SaveGrid(G1, TableName, New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text.Trim, txtID.Text}, New String() {"MainValue", "Exchange", "Value", "LinkFile", "MainAccNo", "SubAccNo", "CostCenterId", "CostCenterSubId", "CostTypeId", "Notes", "IsSelected", "DocNo", "CheckTypeId", "CheckNo", "CheckPersonName", "CheckDate", "CheckBankId", "PurchaseAccNo", "ImportMessageId", "StoreId", "StoreInvoiceNo", "CurrencyId2", "MainValue2", "Exchange2", "MainValue2Ded", "Value2Ded", "DedNotes", "IsDocumented", "IsNotDocumented"}, New String() {GC.MainValue, GC.Exchange, GC.Value, GC.LinkFile, GC.MainAccNo, GC.SubAccNo, GC.CostCenterId, GC.CostCenterSubId, GC.CostTypeId, GC.Notes, GC.IsSelected, GC.DocNo, GC.CheckTypeId, GC.CheckNo, GC.CheckPersonName, GC.CheckDate, GC.CheckBankId, GC.PurchaseAccNo, GC.ImportMessageId, GC.StoreId, GC.StoreInvoiceNo, GC.CurrencyId2, GC.MainValue2, GC.Exchange2, GC.MainValue2Ded, GC.Value2Ded, GC.DedNotes, GC.IsDocumented, GC.IsNotDocumented}, New VariantType() {VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.Boolean, VariantType.String, VariantType.Integer, VariantType.String, VariantType.String, VariantType.Date, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.String, VariantType.Decimal, VariantType.Decimal}, New String() {}, "Line", "Line") Then Return


        If Not bm.Save(New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text.Trim, txtID.Text}, State) Then Return

        Select Case CType(sender, Controls.Button).Name
            Case btnPrint.Name, btnPrint2.Name, btnPrint3.Name
                State = BasicMethods.SaveState.Print
        End Select

        TraceInvoice(State.ToString)

        If Not DontClear Then
            If Md.MyProjectType = ProjectType.X Then
                bm.ShowMSG("تم الحفظ", True)
            Else
                btnNew_Click(sender, e)
            End If
        End If
        AllowSave = True
        SaveSetting("OMEGA", "BankCash_G2TypeId", Flag, BankCash_G2TypeId.SelectedValue)
    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExecuteNonQuery("BeforeDeleteBankCash_G2", New String() {"Flag", "BankCash_G2TypeId", "InvoiceNo", "UserDelete", "State"}, New String() {Flag, BankCash_G2TypeId.SelectedValue, txtID.Text, Md.UserName, State})
        bm.ExecuteNonQuery("BeforeDeleteBankCash_G22", New String() {"Flag", "BankCash_G2TypeId", "InvoiceNo", "UserDelete", "State"}, New String() {Flag, BankCash_G2TypeId.SelectedValue, txtID.Text, Md.UserName, State})
    End Sub


    Dim lop As Boolean = False

    Sub ClearRow(G As MyGrid, ByVal i As Integer)
        G.Rows(i).Cells(GC.MainValue).Value = Nothing
        G.Rows(i).Cells(GC.Exchange).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.LinkFile).Value = Nothing
        G.Rows(i).Cells(GC.MainAccNo).Value = Nothing
        G.Rows(i).Cells(GC.MainAccName).Value = Nothing
        G.Rows(i).Cells(GC.SubAccNo).Value = Nothing
        G.Rows(i).Cells(GC.SubAccName).Value = Nothing
        G.Rows(i).Cells(GC.AnalysisId).Value = Nothing
        G.Rows(i).Cells(GC.AnalysisName).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterId).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterName).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterSubId).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterSubName).Value = Nothing
        G.Rows(i).Cells(GC.CostTypeId).Value = Nothing
        G.Rows(i).Cells(GC.Notes).Value = Nothing
        G.Rows(i).Cells(GC.IsSelected).Value = Nothing
        G.Rows(i).Cells(GC.DocNo).Value = Nothing
        G.Rows(i).Cells(GC.CheckTypeId).Value = 1
        G.Rows(i).Cells(GC.CheckNo).Value = Nothing
        G.Rows(i).Cells(GC.CheckDate).Value = Nothing
        G.Rows(i).Cells(GC.CheckBankId).Value = Nothing
        G.Rows(i).Cells(GC.CheckPersonName).Value = Nothing

        G.Rows(i).Cells(GC.CurrencyId2).Value = Nothing
        G.Rows(i).Cells(GC.MainValue2).Value = Nothing
        G.Rows(i).Cells(GC.Exchange2).Value = Nothing

        G.Rows(i).Cells(GC.PurchaseAccNo).Value = Nothing
        G.Rows(i).Cells(GC.ImportMessageId).Value = Nothing
        G.Rows(i).Cells(GC.StoreId).Value = Nothing
        G.Rows(i).Cells(GC.StoreInvoiceNo).Value = Nothing

        G.Rows(i).Cells(GC.MainValue2Ded).Value = Nothing
        G.Rows(i).Cells(GC.Value2Ded).Value = Nothing
        G.Rows(i).Cells(GC.DedNotes).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
        G.Rows(i).Cells(GC.IsDocumented).Value = Nothing
        G.Rows(i).Cells(GC.IsNotDocumented).Value = Nothing

    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Dim G As MyGrid = sender

        If lop Then Return
        Try
            If Md.MyProjectType = ProjectType.X AndAlso e.RowIndex < G.Rows.Count - 1 AndAlso Val(G.Rows(e.RowIndex).Cells(GC.DocNo).Value) = 0 Then
                G.Rows(e.RowIndex).Cells(GC.DocNo).Value = Val(txtID.Text) * 1000 + e.RowIndex + 1
            End If

            If G.Columns(e.ColumnIndex).Name = GC.MainValue OrElse G.Columns(e.ColumnIndex).Name = GC.Exchange Then

                If Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) = 0 Then
                    If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                        G.Rows(e.RowIndex).Cells(GC.Exchange).Value = bm.ExecuteScalar("select dbo.GetCurrencyExchange(" & Val(BankId.Text) & "," & MainLinkFile.SelectedValue & "," & CurrencyId.SelectedValue.ToString & ",0,'" & bm.ToStrDate(DayDate.SelectedDate) & "')")
                    Else
                        G.Rows(e.RowIndex).Cells(GC.Exchange).Value = bm.ExecuteScalar("select dbo.GetCurrencyExchangeMain(" & CurrencyId.SelectedValue.ToString & ",'" & bm.ToStrDate(DayDate.SelectedDate) & "')")
                    End If
                    If Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) = 0 Then
                        G.Rows(e.RowIndex).Cells(GC.Exchange).Value = 1
                    End If
                    If Val(G.Rows(e.RowIndex).Cells(GC.Exchange2).Value) = 0 Then
                        G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = 1
                    End If
                End If

                G.Rows(e.RowIndex).Cells(GC.MainValue).Value = Val(G.Rows(e.RowIndex).Cells(GC.MainValue).Value)
                G.Rows(e.RowIndex).Cells(GC.Value).Value = Math.Round(Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) * Val(G.Rows(e.RowIndex).Cells(GC.MainValue).Value), 4, MidpointRounding.AwayFromZero)

                GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, G.CurrentRow.Index))
                GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.MainValue2).Index, G.CurrentRow.Index))
            ElseIf G.Columns(e.ColumnIndex).Name = GC.MainAccNo Then
                'bm.AccNoLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.MainAccNo), MainAccName)
                bm.AccNoLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.MainAccNo), G.Rows(e.RowIndex).Cells(GC.MainAccName))
                MainAccName.Content = G.Rows(e.RowIndex).Cells(GC.MainAccName).Value
                dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "')")
                If dt.Rows.Count = 0 Then
                    G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = True
                Else
                    G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = False
                    If Md.ShowGridAccCombo Then
                        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from " & dt.Rows(0)("TableName") & " where AccNo='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "' union all select 0 Id,'-' Name order by Id", G.Rows(e.RowIndex).Cells(GC.SubAccNo))
                    End If
                End If
                GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, e.RowIndex))
                'G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccNo)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.SubAccNo Then
                If Md.ShowBankCash_GAccNo_Not_LinkFile Then
                    dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "')")
                    If dt.Rows.Count > 0 Then
                        'bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.SubAccNo), SubAccName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value) & " and AccNo='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "'")
                        bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.SubAccNo), G.Rows(e.RowIndex).Cells(GC.SubAccName), "select Name from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value) & " and AccNo='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "'")
                        SubAccName.Content = G.Rows(e.RowIndex).Cells(GC.SubAccName).Value
                        'G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId)
                        bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.SubAccNo), G.Rows(e.RowIndex).Cells(GC.CurrencyId2), "select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value))
                        If Val(G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value) = 0 Then
                            G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value = "1"
                        End If

                        If Md.MyProjectType = ProjectType.X Then
                            G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value = CurrencyId.SelectedValue
                        End If
                        G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = False
                    Else
                        G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value = ""
                        G.Rows(e.RowIndex).Cells(GC.SubAccName).Value = ""
                        G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = True
                        SubAccName.Content = ""
                        G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value = "1"
                        G.Rows(e.RowIndex).Cells(GC.MainValue2).Value = G.Rows(e.RowIndex).Cells(GC.MainValue).Value
                        G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = "1"

                        If Md.MyProjectType = ProjectType.X Then
                            G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value = CurrencyId.SelectedValue
                            G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = G.Rows(e.RowIndex).Cells(GC.Exchange).Value
                        End If
                    End If
                Else
                    G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = False
                    dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & G.Rows(e.RowIndex).Cells(GC.LinkFile).Value)
                    If dt.Rows.Count > 0 Then
                        bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.SubAccNo), G.Rows(e.RowIndex).Cells(GC.SubAccName), "select Name from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value))
                        SubAccName.Content = G.Rows(e.RowIndex).Cells(GC.SubAccName).Value
                        bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.SubAccNo), G.Rows(e.RowIndex).Cells(GC.CurrencyId2), "select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value))
                    Else
                        G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value = ""
                        G.Rows(e.RowIndex).Cells(GC.SubAccName).Value = ""
                        SubAccName.Content = ""
                        G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value = 1
                        G.Rows(e.RowIndex).Cells(GC.MainValue2).Value = G.Rows(e.RowIndex).Cells(GC.MainValue).Value
                        G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = "1"
                    End If
                End If

                If dt.Rows.Count > 0 Then
                    If Val(G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value) = Val(CurrencyId.SelectedValue) Then
                        G.Rows(e.RowIndex).Cells(GC.MainValue2).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = G.Rows(e.RowIndex).Cells(GC.Exchange).Value
                        G.Rows(e.RowIndex).Cells(GC.MainValue2).Value = G.Rows(e.RowIndex).Cells(GC.MainValue).Value
                    ElseIf Val(G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value) = 1 AndAlso Val(G.Rows(e.RowIndex).Cells(GC.LinkFile).Value) <> 8 Then
                        G.Rows(e.RowIndex).Cells(GC.MainValue2).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = 1
                        G.Rows(e.RowIndex).Cells(GC.MainValue2).Value = G.Rows(e.RowIndex).Cells(GC.Value).Value
                    Else
                        G.Rows(e.RowIndex).Cells(GC.MainValue2).ReadOnly = False
                        If Val(G.Rows(e.RowIndex).Cells(GC.Exchange2).Value) = 0 Or Val(G.Rows(e.RowIndex).Cells(GC.MainValue2).Value) = 0 Then

                            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                                G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = bm.ExecuteScalar("select dbo.GetCurrencyExchange(" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value) & "," & G.Rows(e.RowIndex).Cells(GC.LinkFile).Value & "," & G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value & ",0,'" & bm.ToStrDate(DayDate.SelectedDate) & "')")
                            Else
                                G.Rows(e.RowIndex).Cells(GC.Exchange).Value = bm.ExecuteScalar("select dbo.GetCurrencyExchangeMain(" & CurrencyId.SelectedValue.ToString & ",'" & bm.ToStrDate(DayDate.SelectedDate) & "')")
                            End If
                            If Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) = 0 Then
                                G.Rows(e.RowIndex).Cells(GC.Exchange).Value = 1
                            End If
                            If Val(G.Rows(e.RowIndex).Cells(GC.Exchange2).Value) = 0 Then
                                G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = 1
                            End If
                            G.Rows(e.RowIndex).Cells(GC.MainValue2).Value = Math.Round(G.Rows(e.RowIndex).Cells(GC.Value).Value / G.Rows(e.RowIndex).Cells(GC.Exchange2).Value, 4, MidpointRounding.AwayFromZero)
                        End If
                    End If
                End If

            ElseIf G.Columns(e.ColumnIndex).Name = GC.MainValue2 Then
                If Val(G.Rows(e.RowIndex).Cells(GC.LinkFile).Value) = 8 AndAlso Val(G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value) = 1 Then
                    G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.MainValue2).Value)
                    G.Rows(e.RowIndex).Cells(GC.Exchange).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value) / Val(G.Rows(e.RowIndex).Cells(GC.MainValue).Value)
                End If
                G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value) / Val(G.Rows(e.RowIndex).Cells(GC.MainValue2).Value)
                G.Rows(e.RowIndex).Cells(GC.Value2Ded).Value = Val(G.Rows(e.RowIndex).Cells(GC.MainValue2Ded).Value) * Val(G.Rows(e.RowIndex).Cells(GC.Exchange2).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.MainValue2Ded Then
                G.Rows(e.RowIndex).Cells(GC.Value2Ded).Value = Val(G.Rows(e.RowIndex).Cells(GC.MainValue2Ded).Value) * Val(G.Rows(e.RowIndex).Cells(GC.Exchange2).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.AnalysisId Then
                'bm.AnalysisIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.AnalysisId), AnalysisName)
                bm.AnalysisIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.AnalysisId), G.Rows(e.RowIndex).Cells(GC.AnalysisName))
                AnalysisName.Content = G.Rows(e.RowIndex).Cells(GC.AnalysisName).Value
            ElseIf G.Columns(e.ColumnIndex).Name = GC.CostCenterId Then
                'bm.CostCenterIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.CostCenterId), CostCenterName)
                bm.CostCenterIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.CostCenterId), G.Rows(e.RowIndex).Cells(GC.CostCenterName))
                CostCenterName.Content = G.Rows(e.RowIndex).Cells(GC.CostCenterName).Value
            ElseIf G.Columns(e.ColumnIndex).Name = GC.CostCenterSubId Then
                'bm.CostCenterSubIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.CostCenterSubId), CostCenterSubName)
                bm.CostCenterSubIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.CostCenterSubId), G.Rows(e.RowIndex).Cells(GC.CostCenterSubName))
                CostCenterSubName.Content = G.Rows(e.RowIndex).Cells(GC.CostCenterSubName).Value
            ElseIf G.Columns(e.ColumnIndex).Name = GC.PurchaseAccNo Then
                bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo), PurchaseAccName, "select Name from OrderTypes where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).Value))
                'bm.AccNoLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo), PurchaseAccName)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.ImportMessageId Then
                bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.ImportMessageId), ImportMessageName, "select dbo.GetAccName(AccNo) from ImportMessages  where OrderTypeId='" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.PurchaseAccNo).Value & "' and Id=" & Val(G.Rows(e.RowIndex).Cells(GC.ImportMessageId).Value))
            ElseIf G.Columns(e.ColumnIndex).Name = GC.StoreId Then
                bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.StoreId), StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.StoreId).Value))
            ElseIf G.Columns(e.ColumnIndex).Name = GC.StoreInvoiceNo Then
                If Not G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).Value = Nothing AndAlso Not bm.IF_Exists("select InvoiceNo from SalesMaster where Temp=0 and StoreId=" & G.CurrentRow.Cells(GC.StoreId).Value & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & G.CurrentRow.Cells(GC.StoreInvoiceNo).Value) Then
                    bm.ShowMSG("هذا الرقم غير صحيح")
                    G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).Value = Nothing
                    Exit Sub
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.CostTypeId Then
                Select Case G.Rows(e.RowIndex).Cells(GC.CostTypeId).Value
                    Case 1
                        G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly = True
                    Case 2
                        G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly = False
                        G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly = True
                    Case 3
                        G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly = False
                        G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly = False
                        G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly = True
                    Case 4
                        G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly = False
                        G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly = False
                    Case Else
                        G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly = False
                        G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly = False
                        G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly = False
                        G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly = False
                End Select

                If G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly Then G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).Value = ""
                If G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly Then G.Rows(e.RowIndex).Cells(GC.ImportMessageId).Value = ""
                If G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly Then G.Rows(e.RowIndex).Cells(GC.StoreId).Value = ""
                If G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly Then G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).Value = ""

            ElseIf G.Columns(e.ColumnIndex).Name = GC.IsDocumented Then
                G.Rows(e.RowIndex).Cells(GC.IsNotDocumented).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value) - Val(G.Rows(e.RowIndex).Cells(GC.IsDocumented).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.IsNotDocumented Then
                G.Rows(e.RowIndex).Cells(GC.IsDocumented).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value) - Val(G.Rows(e.RowIndex).Cells(GC.IsNotDocumented).Value)
            End If

            loplop = True
            Try
                If Val(G.Rows(e.RowIndex).Cells(GC.CheckTypeId).Value) < 1 Then G.Rows(e.RowIndex).Cells(GC.CheckTypeId).Value = 1
                CheckTypeId.SelectedValue = G.Rows(e.RowIndex).Cells(GC.CheckTypeId).Value
                CheckNo.Text = G.Rows(e.RowIndex).Cells(GC.CheckNo).Value
                CheckPersonName.Text = G.Rows(e.RowIndex).Cells(GC.CheckPersonName).Value
                MainValue2Ded.Text = G.Rows(e.RowIndex).Cells(GC.MainValue2Ded).Value
                Value2Ded.Text = G.Rows(e.RowIndex).Cells(GC.Value2Ded).Value
                DedNotes.Text = G.Rows(e.RowIndex).Cells(GC.DedNotes).Value

                CheckBankId.Text = G.Rows(e.RowIndex).Cells(GC.CheckBankId).Value
                CheckBankId_LostFocus(Nothing, Nothing)
                CheckDate.SelectedDate = Nothing
                If G.Rows(e.RowIndex).Cells(GC.CheckDate).Value Is Nothing Then
                    G.Rows(e.RowIndex).Cells(GC.CheckDate).Value = Nothing
                Else
                    CheckDate.SelectedDate = DateTime.Parse(G.Rows(e.RowIndex).Cells(GC.CheckDate).Value)
                End If
            Catch ex As Exception
            End Try
            loplop = False
            TestEnable()

            CalcTotal()
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        Catch ex As Exception
        End Try
    End Sub
    Dim loplop As Boolean = False

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        If lop Then Return
        lop = True

        If BankCash_G2TypeId.SelectedIndex < 1 Then
            G1.Enabled = False
        Else
            G1.Enabled = True
        End If

        IsNew = True

        G1.ReadOnly = False

        Dim d As Date = bm.MyGetDate()
        If Md.MyProjectType = ProjectType.X AndAlso Not DayDate.SelectedDate Is Nothing Then
            d = DayDate.SelectedDate
        End If
        bm.ClearControls(False)

        If Md.MyProjectType = ProjectType.X Then
            MainLinkFile.SelectedValue = 5
            BankId.Text = Md.DefaultSave
        End If

        DayDate.SelectedDate = d

        G1.Rows.Clear()

        CalcTotal()

        CheckTypeId.SelectedValue = 1
        CheckNo.Clear()
        CheckDate.SelectedDate = Nothing
        CheckBankId.Clear()
        CheckBankName.Clear()
        CheckPersonName.Clear()
        TestEnable()

        MainAccName.Content = ""
        SubAccName.Content = ""
        AnalysisName.Content = ""
        CostCenterName.Content = ""
        CostCenterSubName.Content = ""

        ImportMessageName.Content = ""
        StoreName.Content = ""
        Value.Clear()
        MainValue.Clear()
        BankId_LostFocus(Nothing, Nothing)

        txtFlag.Text = Flag
        SystemUser.SelectedValue = Md.UserName

        If BankCash_G2TypeId.SelectedIndex < 1 Then
            lop = False
            Return
        End If
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & MainId & "=" & BankCash_G2TypeId.SelectedValue.ToString & " and " & SubId & "=" & txtFlag.Text)
        dt = bm.ExecuteAdapter("select FromInvoiceNo,ToInvoiceNo from BankCash_G2Types where Flag=" & Flag & " and Id=" & BankCash_G2TypeId.SelectedValue.ToString)
        If dt.Rows.Count = 0 Then
            lop = False
            Return
        End If
        If txtID.Text = "" Then txtID.Text = dt.Rows(0)("FromInvoiceNo")
        If Val(txtID.Text) > dt.Rows(0)("ToInvoiceNo") Then
            'txtID.Text = dt.Rows(0)("ToInvoiceNo")
            txtID.Clear()
            bm.ShowMSG("لا توجد مسلسلات خالية في هذه اليومية")
        End If
        'DayDate.Focus()
        'txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            TraceInvoice("Deleted")
            bm.ExecuteNonQuery("delete from " & TableName & " where " & MainId & "=" & BankCash_G2TypeId.SelectedValue.ToString & " and " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text, txtID.Text}, "Back", dt)
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
        bm.RetrieveAll(New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text.Trim, txtID.Text}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()

            dt = bm.ExecuteAdapter("select FromInvoiceNo,ToInvoiceNo from BankCash_G2Types where Flag=" & Flag & " and Id=" & BankCash_G2TypeId.SelectedValue.ToString)
            If dt.Rows.Count > 0 Then
                If Val(txtID.Text) < dt.Rows(0)("FromInvoiceNo") OrElse Val(txtID.Text) > dt.Rows(0)("ToInvoiceNo") Then txtID.Text = ""
            End If

            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus
        Try
            If Val(BankId.Text.Trim) = 0 Then
                BankId.Clear()
                BankName.Clear()
                Return
            End If

            dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile.SelectedValue)
            bm.LostFocus(BankId, BankName, "select Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ") where Id=" & BankId.Text.Trim())
            CurrencyId.SelectedValue = bm.ExecuteScalar("select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim())
            CurrencyId_SelectionChanged(Nothing, Nothing)
        Catch
        End Try
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile.SelectedValue)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ")") Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub CheckBankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CheckBankId.LostFocus
        If Val(CheckBankId.Text.Trim) = 0 Then
            CheckBankId.Clear()
            CheckBankName.Clear()
            Return
        End If
        bm.LostFocus(CheckBankId, CheckBankName, "select Name from CheckBanks where Id=" & CheckBankId.Text.Trim())
    End Sub
    Private Sub CheckBankId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CheckBankId.KeyUp
        If bm.ShowHelp("Banks", CheckBankId, CheckBankName, e, "select cast(Id as varchar(100)) Id,Name from CheckBanks", "CheckBanks") Then
            CheckBankId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub CheckNo_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CheckNo.KeyUp
        Dim str As String = "GetCheckStates "
        If Flag = 2 Then
            str &= " @LinkFile=" & Val(MainLinkFile.SelectedValue) & ",@AccNo=" & Val(BankId.Text) & ",@LinkFile2=" & Val(G1.CurrentRow.Cells(GC.LinkFile).Value) & ",@AccNo2=" & Val(G1.CurrentRow.Cells(GC.SubAccNo).Value)
        Else
            str &= " @LinkFile2=" & Val(MainLinkFile.SelectedValue) & ",@AccNo2=" & Val(BankId.Text) & ",@LinkFile=" & Val(G1.CurrentRow.Cells(GC.LinkFile).Value) & ",@AccNo=" & Val(G1.CurrentRow.Cells(GC.SubAccNo).Value)
        End If

        If bm.ShowHelpMultiColumns("الشيكات", CheckNo, CheckNo, e, str) Then
            'CheckNo.Text = bm.SelectedRow(0)
            CheckNo_LostFocus(Nothing, Nothing)
            CheckBankId_LostFocus(Nothing, Nothing)

            Try
                G1.CurrentRow.Cells(GC.MainValue).Value = bm.SelectedRow("المتبقي")
                GridCalcRow(G1, New System.Windows.Forms.DataGridViewCellEventArgs(G1.Columns(GC.MainValue).Index, G1.CurrentRow.Index))
                G1.CurrentRow.Cells(GC.MainValue2).Value = 0
                GridCalcRow(G1, New System.Windows.Forms.DataGridViewCellEventArgs(G1.Columns(GC.SubAccNo).Index, G1.CurrentRow.Index))
            Catch
            End Try

        End If

    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblID.SetResourceReference(ContentProperty, "Id")

        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblNotes.SetResourceReference(ContentProperty, "Notes")
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G1.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G1.Rows.Remove(G1.CurrentRow)
                CalcTotal()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim LopCalc As Boolean = False
    Private Sub CalcTotal()
        If LopCalc Or lop Then Return
        LopCalc = True
        Try
            Value.Text = Math.Round(0, 4)
            MainValue.Text = Math.Round(0, 4)
            For i As Integer = 0 To G1.Rows.Count - 1
                Value.Text += Val(G1.Rows(i).Cells(GC.Value).Value)
                MainValue.Text += Val(G1.Rows(i).Cells(GC.MainValue).Value)
            Next
        Catch ex As Exception
        End Try
        LopCalc = False
    End Sub


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        Dim G As MyGrid = sender
        'e.Handled = True
        If Md.ShowGridAccCombo Then Return
        If G.CurrentCell Is Nothing OrElse G.CurrentCell.ReadOnly Then Return
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.MainAccNo).Index Then
                If bm.AccNoShowHelpGrid(G.CurrentRow.Cells(GC.MainAccNo), G.CurrentRow.Cells(GC.MainAccName), e, 1) Then
                    MainAccName.Content = G.CurrentRow.Cells(GC.MainAccNo).Value
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccNo)
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.SubAccNo).Index Then
                If Md.ShowBankCash_GAccNo_Not_LinkFile Then
                    dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.MainAccNo).Value & "')")
                    If dt.Rows.Count > 0 AndAlso bm.ShowHelpGrid(dt.Rows(0)("TableName"), G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccNo), G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccName), e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.MainAccNo).Value & "'") Then
                        SubAccName.Content = G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccName).Value
                        'GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, G.CurrentCell.RowIndex))
                        If G.Columns(GC.AnalysisId).Visible Then
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisId)
                        ElseIf G.Columns(GC.CostCenterId).Visible Then
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId)
                        ElseIf G.Columns(GC.MainValue2).Visible Then
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.MainValue2)
                        Else
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                        End If
                    End If
                Else
                    dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.LinkFile).Value)

                    If dt.Rows.Count > 0 AndAlso bm.ShowHelpGrid(dt.Rows(0)("TableName"), G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccNo), SubAccName, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName")) Then
                        SubAccName.Content = G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccName).Value
                        'GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, G.CurrentCell.RowIndex))
                        If G.Columns(GC.AnalysisId).Visible Then
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisId)
                        ElseIf G.Columns(GC.CostCenterId).Visible Then
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId)
                        ElseIf G.Columns(GC.MainValue2).Visible Then
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.MainValue2)
                        Else
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                        End If
                    End If
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.AnalysisId).Index Then
                If bm.ShowHelpGrid("Analysis", G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisId), G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisName), e, "select cast(Id as varchar(100)) Id,Name from Analysis") Then
                    AnalysisName.Content = G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisName).Value
                    If G.Columns(GC.CostCenterId).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId)
                    Else
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                    End If
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.CostCenterId).Index Then
                If bm.ShowHelpGrid("CostCenters", G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId), CostCenterName, e, "select cast(Id as varchar(100)) Id,Name from CostCenters where SubType=1") Then
                    If G.Columns(GC.CostCenterSubId).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterSubId)
                    Else
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                    End If
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.CostCenterSubId).Index Then
                If bm.ShowHelpGrid("CostCenterSubs", G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterSubId), G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterSubName), e, "select cast(Id as varchar(100)) Id,Name from CostCenterSubs") Then
                    CostCenterSubName.Content = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterSubName).Value
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.PurchaseAccNo).Index Then
                If bm.ShowHelpGrid("OrderTypes", G.Rows(G.CurrentCell.RowIndex).Cells(GC.PurchaseAccNo), PurchaseAccName, e, "select cast(Id as varchar(100)) Id,Name from OrderTypes") Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.ImportMessageId)
                End If

                'If bm.AccNoShowHelpGrid(G.CurrentRow.Cells(GC.PurchaseAccNo), PurchaseAccName, e, 1, , True) Then
                '    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.ImportMessageId)
                'End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.ImportMessageId).Index Then
                If bm.ShowHelpGrid("ImportMessages", G.Rows(G.CurrentCell.RowIndex).Cells(GC.ImportMessageId), ImportMessageName, e, "select cast(Id as varchar(100)) 'رقم الرسالة',dbo.GetShipperName(ShipperId) الشاحن from ImportMessages where OrderTypeId='" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.PurchaseAccNo).Value & "'", "", "رقم الرسالة", "الشاحن") Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.StoreId)
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.StoreId).Index Then
                If bm.ShowHelpGrid("Stores", G.Rows(G.CurrentCell.RowIndex).Cells(GC.StoreId), StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.StoreInvoiceNo)
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.StoreInvoiceNo).Index Then
                If bm.ShowHelpGridMultiColumns("الفواتير", G.CurrentRow.Cells(GC.StoreInvoiceNo), G.CurrentRow.Cells(GC.StoreInvoiceNo), e, "select cast(M.InvoiceNo as varchar(100)) 'الفاتورة',dbo.GetSupplierName(M.ToId) 'المورد',M.DocNo 'رقم عقد المورد',cast(TotalAfterDiscount as nvarchar(100)) 'إجمالي الفاتورة',cast(M.OrderTypeId as nvarchar(100)) 'مسلسل الطلبية',dbo.GetOrderTypes(M.OrderTypeId) 'اسم الطلبية',(case when isnull(MM.IsDelivered,0)=1 then 'تم الاستلام' else 'لم يتم الاستلام' end) 'الحالة' from SalesMaster M left join ImportMessagesDetails DD on(M.OrderTypeId=DD.OrderTypeId and M.StoreId=DD.StoreId and M.InvoiceNo=DD.InvoiceNo) left join ImportMessages MM on(MM.OrderTypeId=DD.OrderTypeId and MM.Id=DD.Id) where M.Temp=0 and M.StoreId=" & G.CurrentRow.Cells(GC.StoreId).Value & " and M.Flag=" & Sales.FlagState.الاستيراد & IIf(G.CurrentRow.Cells(GC.LinkFile).Value = 2, " and M.ToId=" & G.CurrentRow.Cells(GC.SubAccNo).Value, "")) Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                End If
            End If
        Catch ex As Exception
        End Try
        G.CommitEdit(Forms.DataGridViewDataErrorContexts.Commit)
    End Sub


    Private Sub G_CellBeginEdit(sender As Object, e As Forms.DataGridViewCellCancelEventArgs)
        Dim G As MyGrid = sender
        If CType(G.Rows(e.RowIndex).Cells(GC.LinkFile), System.Windows.Forms.DataGridViewComboBoxCell).Value Is Nothing Then
            CType(G.Rows(e.RowIndex).Cells(GC.LinkFile), System.Windows.Forms.DataGridViewComboBoxCell).Value = "0"
        End If
        If CType(G.Rows(e.RowIndex).Cells(GC.CostTypeId), System.Windows.Forms.DataGridViewComboBoxCell).Value Is Nothing Then
            CType(G.Rows(e.RowIndex).Cells(GC.CostTypeId), System.Windows.Forms.DataGridViewComboBoxCell).Value = "0"
        End If
    End Sub

    Private Sub G_SelectionChanged(sender As Object, e As EventArgs)
        Dim G As MyGrid = sender
        If G.CurrentRow Is Nothing Then Return

        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.LinkFile).Index, G.CurrentRow.Index))

        MainAccName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.MainAccName).Value
        SubAccName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.SubAccName).Value
        AnalysisName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.AnalysisName).Value
        CostCenterName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.CostCenterName).Value
        CostCenterSubName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.CostCenterSubName).Value

        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.PurchaseAccNo).Index, G.CurrentRow.Index))
        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ImportMessageId).Index, G.CurrentRow.Index))
        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.StoreId).Index, G.CurrentRow.Index))
    End Sub

    Private Sub BankCash_G2TypeId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles BankCash_G2TypeId.SelectionChanged
        btnNew_Click(Nothing, Nothing)
        txtID_LostFocus(Nothing, Nothing)
        'SaveSetting("OMEGA", "BankCash_G2TypeId", Flag, BankCash_G2TypeId.SelectedValue)
    End Sub

    Private Sub Exchange_TextChanged(sender As Object, e As TextChangedEventArgs) 'Handles Exchange.TextChanged
        For i As Integer = 0 To G1.Rows.Count - 1
            If Val(G1.Rows(i).Cells(GC.MainValue).Value) <> 0 Then
                GridCalcRow(G1, New Forms.DataGridViewCellEventArgs(G1.Columns(GC.MainValue).Index, i))
            End If
        Next
    End Sub

    Private Sub CheckTypeId_LostFocus(sender As Object, e As RoutedEventArgs) Handles CheckTypeId.LostFocus
        If loplop Then Return
        If G1.CurrentRow Is Nothing Then
            G1.CurrentCell = G1.Rows(G1.Rows.Add).Cells(GC.DocNo)
        End If

        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckTypeId).Value = CheckTypeId.SelectedValue
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckTypeId).Value = 1
        End Try
        TestEnable()
    End Sub

    Private Sub CheckNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles CheckNo.LostFocus
        Try
            dt = bm.ExecuteAdapter("select top 1 dbo.ToStrDate(CheckDate),CheckBankId from BankCash_G2 where CheckNo='" & CheckNo.Text & "' Order by Daydate")
            If dt.Rows.Count > 0 Then
                CheckDate.SelectedDate = CType(dt.Rows(0)(0), DateTime)
                CheckBankId.Text = dt.Rows(0)(1)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CheckNo_TextChanged(sender As Object, e As TextChangedEventArgs) Handles CheckNo.TextChanged
        If loplop OrElse G1.CurrentRow Is Nothing Then Return
        If G1.CurrentRow.Index = G1.NewRowIndex Then
            Dim i As Integer = G1.Rows.Add
            G1.CurrentCell = G1.Rows(i).Cells(G1.CurrentCell.ColumnIndex)
        End If
        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckNo).Value = CheckNo.Text
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckNo).Value = ""
        End Try
    End Sub


    Private Sub CheckPersonName_TextChanged(sender As Object, e As TextChangedEventArgs) Handles CheckPersonName.TextChanged
        If loplop OrElse G1.CurrentRow Is Nothing Then Return
        If G1.CurrentRow.Index = G1.NewRowIndex Then
            Dim i As Integer = G1.Rows.Add
            G1.CurrentCell = G1.Rows(i).Cells(G1.CurrentCell.ColumnIndex)
        End If
        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckPersonName).Value = CheckPersonName.Text
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckPersonName).Value = ""
        End Try
    End Sub

    Private Sub CheckDate_TextChanged(sender As Object, e As SelectionChangedEventArgs) Handles CheckDate.SelectedDateChanged
        If loplop OrElse G1.CurrentRow Is Nothing Then Return
        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckDate).Value = CheckDate.SelectedDate.Value.Date
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckDate).Value = Nothing
        End Try
    End Sub

    Private Sub CheckBankId_TextChanged(sender As Object, e As TextChangedEventArgs) Handles CheckBankId.TextChanged
        If loplop OrElse G1.CurrentRow Is Nothing Then Return
        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckBankId).Value = CheckBankId.Text
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckBankId).Value = ""
        End Try
        CheckBankId_LostFocus(Nothing, Nothing)
    End Sub


    Private Sub Ded_TextChanged(sender As Object, e As TextChangedEventArgs) Handles MainValue2Ded.TextChanged, Value2Ded.TextChanged, DedNotes.TextChanged
        If loplop OrElse G1.CurrentRow Is Nothing Then Return
        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.MainValue2Ded).Value = MainValue2Ded.Text
            G1.Rows(G1.CurrentRow.Index).Cells(GC.Value2Ded).Value = Value2Ded.Text
            G1.Rows(G1.CurrentRow.Index).Cells(GC.DedNotes).Value = DedNotes.Text
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.MainValue2Ded).Value = Nothing
            G1.Rows(G1.CurrentRow.Index).Cells(GC.Value2Ded).Value = Nothing
            G1.Rows(G1.CurrentRow.Index).Cells(GC.DedNotes).Value = Nothing
        End Try
        GridCalcRow(G1, New Forms.DataGridViewCellEventArgs(G1.Columns(GC.MainValue2Ded).Index, G1.CurrentRow.Index))
    End Sub


    Private Sub TestEnable()
        If CheckTypeId.SelectedValue = 1 Then
            CheckNo.IsReadOnly = False
            CheckNo.IsEnabled = False
            CheckDate.IsEnabled = False
            CheckBankId.IsEnabled = False
            CheckPersonName.IsEnabled = False
            CheckNo.Clear()
            CheckPersonName.Clear()
            CheckDate.SelectedDate = Nothing
            CheckBankId.Clear()
            CheckBankName.Clear()
        ElseIf CheckTypeId.SelectedValue = 2 OrElse CheckTypeId.SelectedValue = 3 OrElse CheckTypeId.SelectedValue = 11 Then
            CheckNo.IsReadOnly = False
            CheckNo.IsEnabled = True
            CheckDate.IsEnabled = True
            CheckBankId.IsEnabled = True
            CheckPersonName.IsEnabled = True
            If CheckNo.Text.Trim = "" AndAlso CheckTypeId.SelectedValue = 3 Then
                lop = True
                CheckNo.Text = Flag & "-" & BankCash_G2TypeId.SelectedValue & "-" & txtID.Text & "-" & (G1.CurrentRow.Index + 1)
                lop = False
            End If
        Else
            CheckNo.IsReadOnly = True
            CheckNo.IsEnabled = True
            CheckDate.IsEnabled = False
            CheckBankId.IsEnabled = False
            CheckPersonName.IsEnabled = False
        End If

    End Sub

    Private Sub CurrencyId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CurrencyId.SelectionChanged
        SetGCExchange(G1)
        If lop Then Return
        Try
            'G.Rows(e.RowIndex).Cells(GC.Exchange).Value= bm.ExecuteScalar("select dbo.GetCurrencyExchange(" & Val(BankId.Text) & "," & MainLinkFile.SelectedValue & "," & CurrencyId.SelectedValue.ToString & ",0,'"  & bm.ToStrDate(DayDate .SelectedDate ) & "')")
        Catch ex As Exception
        End Try
    End Sub

    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        DontClear = True
        btnSave_Click(sender, e)
        DontClear = False
        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@BankCash_G2TypeId", "@Flag", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {BankCash_G2TypeId.SelectedValue.ToString, Flag, txtID.Text, CType(Parent, Page).Title}
        rpt.Rpt = "BankCash_G21.rpt"
        rpt.Show()
    End Sub

    Private Sub btnPrint2_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint2.Click
        DontClear = True
        btnSave_Click(sender, e)
        DontClear = False
        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@BankCash_G2TypeId", "@Flag", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {BankCash_G2TypeId.SelectedValue.ToString, Flag, txtID.Text, G1.Columns(GC.MainValue2Ded).HeaderText}
        rpt.Rpt = "BankCash_G22.rpt"
        rpt.Show()
    End Sub

    Private Sub btnPrint3_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint3.Click
        DontClear = True
        btnSave_Click(sender, e)
        DontClear = False
        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@BankCash_G2TypeId", "@Flag", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {BankCash_G2TypeId.SelectedValue.ToString, Flag, txtID.Text, CType(Parent, Page).Title}
        rpt.Rpt = "BankCash_G23.rpt"
        rpt.Show()
    End Sub

    Private Sub btnChangeCheckNo_Click(sender As Object, e As RoutedEventArgs) Handles btnChangeCheckNo.Click
        Dim frm As New Window 'With {.SizeToContent = True}
        frm.Content = New ChangeCheckNo With {.MyCheckNo = CheckNo.Text, .txtCheck = CheckNo}
        frm.ShowDialog()
        If CType(frm.Content, ChangeCheckNo).AllowChange Then
            'CheckNo.Text = CType(frm.Content, ChangeCheckNo).MyCheckNo
            CheckNo_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub MainLinkFile_LostFocus(sender As Object, e As RoutedEventArgs) Handles MainLinkFile.LostFocus
        BankId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub SetGCExchange(G As MyGrid)
        If Md.MyProjectType = ProjectType.X Then Return
        Try
            If Flag = 2 Then
                G.Columns(GC.Exchange).ReadOnly = True
            ElseIf Val(CurrencyId.SelectedValue) = 1 Then
                G.Columns(GC.Exchange).ReadOnly = True
            Else
                G.Columns(GC.Exchange).ReadOnly = False
            End If
        Catch
        End Try
    End Sub


    Dim SearchLop As Boolean = False
    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnSearch.Click
        SearchLop = True
        bm.DefineValues()
        bm.SearchTable({MainId, SubId, SubId2}, {BankCash_G2TypeId.SelectedValue, Flag, txtID.Text}, cboSearch, "", {DayDate}, , True)
        SearchLop = False
    End Sub

    Private Sub cboSearch_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSearch.SelectionChanged
        If SearchLop Then Return
        txtID.Text = cboSearch.SelectedValue.ToString
        txtID_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As RoutedEventArgs) Handles btnEdit.Click
        G1.ReadOnly = False
    End Sub

    Dim IsSelected As Boolean = False
    Private Sub BtnIsSelected_Click(sender As Object, e As RoutedEventArgs) Handles btnIsSelected.Click
        G1.EndEdit()
        IsSelected = Not IsSelected
        For i As Integer = 0 To G1.Rows.Count - 1
            G1.Rows(i).Cells(GC.IsSelected).Value = IsSelected
        Next
        If IsSelected Then
            btnIsSelected.Content = "إلغاء تحديد الكل"
        Else
            btnIsSelected.Content = "تحديد الكل"
        End If
    End Sub
End Class
