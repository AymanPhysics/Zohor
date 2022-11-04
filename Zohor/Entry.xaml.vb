Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management
Imports System.ComponentModel
Imports System.IO

Public Class Entry

    Public TableName As String = "Entry"
    Public TableDetailsName As String = "EntryDt"
    Public SubId As String = "EntryTypeId"
    Public SubId2 As String = "InvoiceNo"

    Dim dv As New DataView
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim StaticsDt As New DataTable
    WithEvents G As New MyGrid
    WithEvents MyTimer As New Threading.DispatcherTimer

    Dim WithEvents BackgroundWorker1 As New BackgroundWorker


    Sub NewId()
        'InvoiceNo.Clear()
        'InvoiceNo.IsEnabled = Md.Manager
    End Sub

    Sub UndoNewId()
        'InvoiceNo.IsEnabled = True
    End Sub

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, InvoiceNo}, {btnPrint})
        LoadResource()

        bm.Fields = New String() {SubId, SubId2, "DayDate", "Notes", "IsPosted"}
        bm.control = New Control() {EntryTypeId, InvoiceNo, DayDate, Notes, IsPosted}
        bm.KeyFields = New String() {SubId, SubId2}
        bm.Table_Name = TableName

        Dim x As Integer = Val(GetSetting("OMEGA", "Entry", "EntryTypeId"))
        bm.FillCombo("GetEmpEntryTypes @Flag=" & 4 & ",@EmpId=" & Md.UserName & "", EntryTypeId)
        EntryTypeId.SelectedValue = x

        LoadWFH()
        btnNew_Click(Nothing, Nothing)
        SaveSetting("OMEGA", "Entry", "EntryTypeId", x)
        If Md.MyProjectType = ProjectType.X Then
            EntryTypeId.SelectedValue = x
            btnLast_Click(Nothing, Nothing)
        Else
            btnEdit.Visibility = Visibility.Hidden
        End If
    End Sub


    Structure GC
        Shared MainDebit As String = "MainDebit"
        Shared MainCredit As String = "MainCredit"
        Shared Exchange As String = "Exchange"
        Shared Debit As String = "Debit"
        Shared Credit As String = "Credit"
        Shared MainAccNo As String = "MainAccNo"
        Shared MainAccName As String = "MainAccName"
        Shared SubAccNo As String = "SubAccNo"
        Shared SubAccName As String = "SubAccName"
        Shared CurrencyId As String = "CurrencyId"
        Shared AnalysisId As String = "AnalysisId"
        Shared AnalysisName As String = "AnalysisName"
        Shared CostCenterId As String = "CostCenterId"
        Shared CostCenterName As String = "CostCenterName"
        Shared CostCenterSubId As String = "CostCenterSubId"
        Shared CostCenterSubName As String = "CostCenterSubName"
        Shared Notes As String = "Notes"
        Shared DocNo As String = "DocNo"

        Shared CostTypeId As String = "CostTypeId"
        Shared PurchaseAccNo As String = "PurchaseAccNo"
        Shared ImportMessageId As String = "ImportMessageId"
        Shared StoreId As String = "StoreId"
        Shared StoreInvoiceNo As String = "StoreInvoiceNo"
        Shared Line As String = "Line"
        Shared IsDocumented As String = "IsDocumented"
        Shared IsNotDocumented As String = "IsNotDocumented"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.MainDebit, "مدين عملة")
        G.Columns.Add(GC.MainCredit, "دائن عملة")

        Dim GCCurrencyId As New Forms.DataGridViewComboBoxColumn
        GCCurrencyId.HeaderText = "العملة"
        GCCurrencyId.Name = GC.CurrencyId
        bm.FillCombo("select Id,Name from Currencies order by Id", GCCurrencyId)
        If Md.MyProjectType = ProjectType.X Then
            G.Columns.Add(GCCurrencyId)
            G.RowHeadersWidth = 4
            G.RowHeadersWidthSizeMode = Forms.DataGridViewRowHeadersWidthSizeMode.EnableResizing
        End If

        G.Columns.Add(GC.Exchange, "المعامل")


        G.Columns.Add(GC.Debit, "مدين")
        G.Columns.Add(GC.Credit, "دائن")

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

        If Not Md.MyProjectType = ProjectType.X Then
            G.Columns.Add(GCCurrencyId)
        End If


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

        G.Columns.Add(GC.Notes, "البيان")
        G.Columns.Add(GC.DocNo, "رقم المستند")


        Dim GCCostTypeId As New Forms.DataGridViewComboBoxColumn
        GCCostTypeId.HeaderText = "نوع التكلفة"
        GCCostTypeId.Name = GC.CostTypeId
        bm.FillCombo("select Id,Name from CostTypes union all select 0 Id,'-' Name order by Id", GCCostTypeId)
        G.Columns.Add(GCCostTypeId)

        G.Columns.Add(GC.PurchaseAccNo, "الطلبية")
        G.Columns.Add(GC.ImportMessageId, "الرسالة")
        G.Columns.Add(GC.StoreId, "المخزن")
        G.Columns.Add(GC.StoreInvoiceNo, "مسلسل الفاتورة")
        G.Columns.Add(GC.Line, "Line")
        G.Columns.Add(GC.IsDocumented, "مؤيد")
        G.Columns.Add(GC.IsNotDocumented, "غير مؤيد")

        G.Columns(GC.MainDebit).FillWeight = 80
        G.Columns(GC.MainCredit).FillWeight = 80
        G.Columns(GC.Exchange).FillWeight = 50
        G.Columns(GC.Debit).FillWeight = 80
        G.Columns(GC.Credit).FillWeight = 80
        If Md.ShowGridAccCombo Then
            G.Columns(GC.MainAccNo).FillWeight = 200
            G.Columns(GC.AnalysisId).FillWeight = 120
            G.Columns(GC.CostCenterId).FillWeight = 120
            G.Columns(GC.CostCenterSubId).FillWeight = 120
        Else
            G.Columns(GC.MainAccNo).FillWeight = 80
            G.Columns(GC.SubAccNo).FillWeight = 80
            G.Columns(GC.AnalysisId).FillWeight = 80
            G.Columns(GC.CostCenterId).FillWeight = 80
            G.Columns(GC.CostCenterSubId).FillWeight = 80
        End If

        G.Columns(GC.MainAccName).FillWeight = 150
        G.Columns(GC.SubAccName).FillWeight = 150
        G.Columns(GC.CurrencyId).FillWeight = 100
        G.Columns(GC.AnalysisName).FillWeight = 120
        G.Columns(GC.CostCenterName).FillWeight = 120
        G.Columns(GC.CostCenterSubName).FillWeight = 120
        G.Columns(GC.Notes).FillWeight = 200
        G.Columns(GC.DocNo).FillWeight = 60
        G.Columns(GC.IsDocumented).FillWeight = 80
        G.Columns(GC.IsNotDocumented).FillWeight = 80

        G.Columns(GC.MainAccName).ReadOnly = True
        G.Columns(GC.SubAccName).ReadOnly = True
        G.Columns(GC.AnalysisName).ReadOnly = True
        G.Columns(GC.CostCenterName).ReadOnly = True
        G.Columns(GC.CostCenterSubName).ReadOnly = True

        If Not Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.MainDebit).Visible = False
            G.Columns(GC.MainCredit).Visible = False
            G.Columns(GC.Exchange).Visible = False
            G.Columns(GC.CurrencyId).Visible = False

            G.Columns(GC.IsDocumented).Visible = False
            G.Columns(GC.IsNotDocumented).Visible = False
        End If

        If Not (Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X) Then
            G.Columns(GC.CostTypeId).Visible = False
            G.Columns(GC.PurchaseAccNo).Visible = False
            G.Columns(GC.ImportMessageId).Visible = False
            G.Columns(GC.StoreId).Visible = False
            G.Columns(GC.StoreInvoiceNo).Visible = False

            PurchaseAccName.Visibility = Visibility.Hidden
            ImportMessageName.Visibility = Visibility.Hidden
            StoreName.Visibility = Visibility.Hidden
        End If
        'G.Columns(GC.Name).ReadOnly = True

        'G.Columns(GC.UnitQty).Visible = False

        If Md.HideSubAccNo Then
            G.Columns(GC.SubAccNo).Visible = False
        End If

        If Md.MyProjectType = ProjectType.X Then
            G.Columns(GC.DocNo).Visible = False
        End If

        If Md.ShowGridAccNames Then
            G.Columns(GC.MainAccName).Visible = True
            G.Columns(GC.SubAccName).Visible = True
            MainAccName.Visibility = Visibility.Hidden
            SubAccName.Visibility = Visibility.Hidden
        Else
            G.Columns(GC.MainAccName).Visible = False
            G.Columns(GC.SubAccName).Visible = False
            MainAccName.Visibility = Visibility.Visible
            SubAccName.Visibility = Visibility.Visible
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

        If Md.ShowGridAccCombo Then
            G.Columns(GC.MainAccName).Visible = False
            G.Columns(GC.SubAccName).Visible = False
            G.Columns(GC.AnalysisName).Visible = False
            G.Columns(GC.CostCenterName).Visible = False
            G.Columns(GC.CostCenterSubName).Visible = False
        End If

        G.Columns(GC.Line).Visible = False

        If Md.MyProjectType = ProjectType.X Then
            'G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
            'For i As Integer = 0 To G.ColumnCount - 1
            'G.Columns(i).Width = G.Columns(i).FillWeight
            'Next
            bm.AddThousandSeparator(G.Columns(GC.MainDebit))
            bm.AddThousandSeparator(G.Columns(GC.MainCredit))
            bm.AddThousandSeparator(G.Columns(GC.Debit))
            bm.AddThousandSeparator(G.Columns(GC.Credit))
            bm.AddThousandSeparator(G.Columns(GC.IsDocumented))
            bm.AddThousandSeparator(G.Columns(GC.IsNotDocumented))
        End If

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.CellBeginEdit, AddressOf G_CellBeginEdit
        AddHandler G.SelectionChanged, AddressOf G_SelectionChanged

    End Sub

    Dim lop As Boolean = False

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.MainDebit).Value = Nothing
        G.Rows(i).Cells(GC.MainCredit).Value = Nothing
        G.Rows(i).Cells(GC.Exchange).Value = Nothing
        G.Rows(i).Cells(GC.Debit).Value = Nothing
        G.Rows(i).Cells(GC.Credit).Value = Nothing
        G.Rows(i).Cells(GC.MainAccNo).Value = Nothing
        G.Rows(i).Cells(GC.MainAccName).Value = Nothing
        G.Rows(i).Cells(GC.SubAccNo).Value = Nothing
        G.Rows(i).Cells(GC.SubAccName).Value = Nothing
        G.Rows(i).Cells(GC.CurrencyId).Value = Nothing
        G.Rows(i).Cells(GC.AnalysisId).Value = Nothing
        G.Rows(i).Cells(GC.AnalysisName).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterId).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterName).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterSubId).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterSubName).Value = Nothing
        G.Rows(i).Cells(GC.Notes).Value = Nothing
        G.Rows(i).Cells(GC.DocNo).Value = Nothing
        G.Rows(i).Cells(GC.CostTypeId).Value = Nothing
        G.Rows(i).Cells(GC.PurchaseAccNo).Value = Nothing
        G.Rows(i).Cells(GC.ImportMessageId).Value = Nothing
        G.Rows(i).Cells(GC.StoreId).Value = Nothing
        G.Rows(i).Cells(GC.StoreInvoiceNo).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing

        G.Rows(i).Cells(GC.IsDocumented).Value = Nothing
        G.Rows(i).Cells(GC.IsNotDocumented).Value = Nothing
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(e.ColumnIndex).Name = GC.MainDebit Then
                G.Rows(e.RowIndex).Cells(GC.MainDebit).Value = Val(G.Rows(e.RowIndex).Cells(GC.MainDebit).Value)
                If Val(G.Rows(e.RowIndex).Cells(GC.MainDebit).Value) <> 0 Then
                    G.Rows(e.RowIndex).Cells(GC.MainCredit).Value = 0
                    G.Rows(e.RowIndex).Cells(GC.MainCredit).ReadOnly = True
                Else
                    G.Rows(e.RowIndex).Cells(GC.MainCredit).ReadOnly = False
                End If
                G.Rows(e.RowIndex).Cells(GC.Debit).Value = Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) * Val(G.Rows(e.RowIndex).Cells(GC.MainDebit).Value)
                If Md.MyProjectType = ProjectType.X Then
                    G.Rows(e.RowIndex).Cells(GC.Debit).Value = Math.Round(Val(G.Rows(e.RowIndex).Cells(GC.Debit).Value), 2, MidpointRounding.AwayFromZero)
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.MainCredit Then
                G.Rows(e.RowIndex).Cells(GC.MainCredit).Value = Val(G.Rows(e.RowIndex).Cells(GC.MainCredit).Value)
                If Val(G.Rows(e.RowIndex).Cells(GC.MainCredit).Value) <> 0 Then
                    G.Rows(e.RowIndex).Cells(GC.MainDebit).Value = 0
                    G.Rows(e.RowIndex).Cells(GC.MainDebit).ReadOnly = True
                Else
                    G.Rows(e.RowIndex).Cells(GC.MainDebit).ReadOnly = False
                End If
                G.Rows(e.RowIndex).Cells(GC.Credit).Value = Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) * Val(G.Rows(e.RowIndex).Cells(GC.MainCredit).Value)
                If Md.MyProjectType = ProjectType.X Then
                    G.Rows(e.RowIndex).Cells(GC.Credit).Value = Math.Round(Val(G.Rows(e.RowIndex).Cells(GC.Credit).Value), 2, MidpointRounding.AwayFromZero)
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.CurrencyId Then
                G.Rows(e.RowIndex).Cells(GC.Exchange).Value = bm.ExecuteScalar("select dbo.GetCurrencyExchangeMain(" & Val(G.Rows(e.RowIndex).Cells(GC.CurrencyId).Value) & ",'" & bm.ToStrDate(DayDate.SelectedDate) & "')")
                If Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) = 0 Then
                    G.Rows(e.RowIndex).Cells(GC.Exchange).Value = 1
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.Exchange Then
                G.Rows(e.RowIndex).Cells(GC.Debit).Value = Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) * Val(G.Rows(e.RowIndex).Cells(GC.MainDebit).Value)
                If Md.MyProjectType = ProjectType.X Then
                    G.Rows(e.RowIndex).Cells(GC.Debit).Value = Math.Round(Val(G.Rows(e.RowIndex).Cells(GC.Debit).Value), 2, MidpointRounding.AwayFromZero)
                End If
                G.Rows(e.RowIndex).Cells(GC.Credit).Value = Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) * Val(G.Rows(e.RowIndex).Cells(GC.MainCredit).Value)
                If Md.MyProjectType = ProjectType.X Then
                    G.Rows(e.RowIndex).Cells(GC.Credit).Value = Math.Round(Val(G.Rows(e.RowIndex).Cells(GC.Credit).Value), 2, MidpointRounding.AwayFromZero)
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.Debit Then
                G.Rows(e.RowIndex).Cells(GC.Exchange).Value = Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value)
                If Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) = 0 Then G.Rows(e.RowIndex).Cells(GC.Exchange).Value = 1

                G.Rows(e.RowIndex).Cells(GC.Debit).Value = Val(G.Rows(e.RowIndex).Cells(GC.Debit).Value)
                G.Rows(e.RowIndex).Cells(GC.MainDebit).Value = Val(G.Rows(e.RowIndex).Cells(GC.Debit).Value) / Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value)
                If Md.MyProjectType = ProjectType.X Then
                    G.Rows(e.RowIndex).Cells(GC.MainDebit).Value = Math.Round(Val(G.Rows(e.RowIndex).Cells(GC.MainDebit).Value), 2, MidpointRounding.AwayFromZero)
                End If

                If Val(G.Rows(e.RowIndex).Cells(GC.Debit).Value) <> 0 Then
                    G.Rows(e.RowIndex).Cells(GC.Credit).Value = 0
                    G.Rows(e.RowIndex).Cells(GC.Credit).ReadOnly = True
                    G.CurrentCell = G.Rows(e.RowIndex).Cells(GC.MainAccNo)
                Else
                    G.Rows(e.RowIndex).Cells(GC.Credit).ReadOnly = False
                    G.CurrentCell = G.Rows(e.RowIndex).Cells(GC.Credit)
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.Credit Then
                G.Rows(e.RowIndex).Cells(GC.Exchange).Value = Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value)
                If Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) = 0 Then G.Rows(e.RowIndex).Cells(GC.Exchange).Value = 1

                G.Rows(e.RowIndex).Cells(GC.Credit).Value = Val(G.Rows(e.RowIndex).Cells(GC.Credit).Value)
                G.Rows(e.RowIndex).Cells(GC.MainCredit).Value = Val(G.Rows(e.RowIndex).Cells(GC.Credit).Value) / Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value)
                If Md.MyProjectType = ProjectType.X Then
                    G.Rows(e.RowIndex).Cells(GC.MainCredit).Value = Math.Round(Val(G.Rows(e.RowIndex).Cells(GC.MainCredit).Value), 2, MidpointRounding.AwayFromZero)
                End If

                If Val(G.Rows(e.RowIndex).Cells(GC.Credit).Value) <> 0 Then
                    G.Rows(e.RowIndex).Cells(GC.Debit).Value = 0
                    G.Rows(e.RowIndex).Cells(GC.Debit).ReadOnly = True
                Else
                    G.Rows(e.RowIndex).Cells(GC.Debit).ReadOnly = False
                End If
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
                dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "')")
                If dt.Rows.Count > 0 Then
                    'bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.SubAccNo), SubAccName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value) & " and AccNo='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "'")
                    bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.SubAccNo), G.Rows(e.RowIndex).Cells(GC.SubAccName), "select Name from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value) & " and AccNo='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "'")
                    SubAccName.Content = G.Rows(e.RowIndex).Cells(GC.SubAccName).Value
                    'G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId)
                    G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = False
                Else
                    G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value = ""
                    G.Rows(e.RowIndex).Cells(GC.SubAccName).Value = ""
                    G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = True
                    SubAccName.Content = ""
                End If
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
                If Not G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).Value = Nothing AndAlso Not bm.IF_Exists("select InvoiceNo from SalesMaster where StoreId=" & G.CurrentRow.Cells(GC.StoreId).Value & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & G.CurrentRow.Cells(GC.StoreInvoiceNo).Value) Then
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
                G.Rows(e.RowIndex).Cells(GC.IsNotDocumented).Value = Val(G.Rows(e.RowIndex).Cells(GC.Debit).Value) + Val(G.Rows(e.RowIndex).Cells(GC.Credit).Value) - Val(G.Rows(e.RowIndex).Cells(GC.IsDocumented).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.IsNotDocumented Then
                G.Rows(e.RowIndex).Cells(GC.IsDocumented).Value = Val(G.Rows(e.RowIndex).Cells(GC.Debit).Value) + Val(G.Rows(e.RowIndex).Cells(GC.Credit).Value) - Val(G.Rows(e.RowIndex).Cells(GC.IsNotDocumented).Value)
            End If
            If G.Columns(e.ColumnIndex).Name = GC.MainDebit OrElse G.Columns(e.ColumnIndex).Name = GC.MainCredit OrElse G.Columns(e.ColumnIndex).Name = GC.Exchange OrElse G.Columns(e.ColumnIndex).Name = GC.Debit OrElse G.Columns(e.ColumnIndex).Name = GC.Credit Then
                CalcTotal()
            End If
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        Catch ex As Exception
        End Try
    End Sub



    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True

        G.ReadOnly = False

        UndoNewId()
        bm.FillControls(Me)

        Dim dt As DataTable = bm.ExecuteAdapter("select *,dbo.GetAccName(MainAccNo)MainAccName,dbo.GetSubAccName(MainAccNo,SubAccNo)SubAccName,dbo.GetAnalysisName(Analysisid)AnalysisName,dbo.GetCostCenterName(CostCenterId)CostCenterName,dbo.GetCostCenterSubName(CostCenterSubId)CostCenterSubName from " & TableDetailsName & " where " & SubId & "=" & EntryTypeId.SelectedValue.ToString & " and InvoiceNo=" & InvoiceNo.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.MainDebit).Value = dt.Rows(i)("MainDebit")
            G.Rows(i).Cells(GC.MainCredit).Value = dt.Rows(i)("MainCredit")
            G.Rows(i).Cells(GC.Exchange).Value = dt.Rows(i)("Exchange")
            G.Rows(i).Cells(GC.Debit).Value = dt.Rows(i)("Debit")
            G.Rows(i).Cells(GC.Credit).Value = dt.Rows(i)("Credit")
            G.Rows(i).Cells(GC.MainAccNo).Value = dt.Rows(i)("MainAccNo").ToString
            G.Rows(i).Cells(GC.MainAccName).Value = dt.Rows(i)("MainAccName").ToString
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.MainAccNo).Index, i))
            G.Rows(i).Cells(GC.SubAccNo).Value = dt.Rows(i)("SubAccNo") '.ToString
            G.Rows(i).Cells(GC.SubAccName).Value = dt.Rows(i)("SubAccName").ToString
            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, i))
            G.Rows(i).Cells(GC.CurrencyId).Value = dt.Rows(i)("CurrencyId").ToString
            G.Rows(i).Cells(GC.AnalysisId).Value = dt.Rows(i)("AnalysisId").ToString
            G.Rows(i).Cells(GC.AnalysisName).Value = dt.Rows(i)("AnalysisName").ToString
            G.Rows(i).Cells(GC.CostCenterId).Value = dt.Rows(i)("CostCenterId").ToString
            G.Rows(i).Cells(GC.CostCenterName).Value = dt.Rows(i)("CostCenterName").ToString
            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.CostCenterId).Index, i))
            G.Rows(i).Cells(GC.CostCenterSubId).Value = dt.Rows(i)("CostCenterSubId").ToString
            G.Rows(i).Cells(GC.CostCenterSubName).Value = dt.Rows(i)("CostCenterSubName").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
            G.Rows(i).Cells(GC.DocNo).Value = dt.Rows(i)("DocNo").ToString

            G.Rows(i).Cells(GC.CostTypeId).Value = dt.Rows(i)("CostTypeId").ToString
            G.Rows(i).Cells(GC.PurchaseAccNo).Value = dt.Rows(i)("PurchaseAccNo").ToString
            G.Rows(i).Cells(GC.ImportMessageId).Value = dt.Rows(i)("ImportMessageId").ToString
            G.Rows(i).Cells(GC.StoreId).Value = dt.Rows(i)("StoreId").ToString
            G.Rows(i).Cells(GC.StoreInvoiceNo).Value = dt.Rows(i)("StoreInvoiceNo").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString

            G.Rows(i).Cells(GC.IsDocumented).Value = dt.Rows(i)("IsDocumented")
            G.Rows(i).Cells(GC.IsNotDocumented).Value = dt.Rows(i)("IsNotDocumented")
        Next
        DayDate.Focus()
        G.RefreshEdit()
        lop = False
        CalcTotal()
        LoadTree()


        If Md.MyProjectType = ProjectType.X Then
            G.ReadOnly = True
        End If

        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId, SubId2}, New String() {Val(EntryTypeId.SelectedValue), InvoiceNo.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click
        DontClear = True
        btnSave_Click(sender, e)
        DontClear = False

        PrintPone(sender)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not btnSave.IsEnabled Then Return

        AllowSave = False
        If Not CType(sender, Button).IsEnabled Then Return

        If Not bm.TestDateValidity(DayDate) Then Return

        G.EndEdit()
        If Not G.CurrentCell Is Nothing Then
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.CurrentCell.ColumnIndex, G.CurrentCell.RowIndex))
        End If

        CalcTotal()

        For i As Integer = 0 To G.Rows.Count - 1
            If Val(G.Rows(i).Cells(GC.Debit).Value) = 0 AndAlso Val(G.Rows(i).Cells(GC.Credit).Value) = 0 Then
                Continue For
            End If
            If Val(G.Rows(i).Cells(GC.MainAccNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد الحساب بالسطر " & (i + 1).ToString)
                G.Focus()
                G.CurrentCell = G.Rows(i).Cells(GC.MainAccNo)
                Return
            ElseIf Not Md.HideSubAccNo AndAlso Not G.Rows(i).Cells(GC.SubAccNo).ReadOnly AndAlso Val(G.Rows(i).Cells(GC.SubAccNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد الفرعى بالسطر " & (i + 1).ToString)
                G.Focus()
                G.CurrentCell = G.Rows(i).Cells(GC.SubAccNo)
                Return
            End If

            If Val(G.Rows(i).Cells(GC.CostTypeId).Value) = 0 AndAlso Val(G.Rows(i).Cells(GC.PurchaseAccNo).Value) > 0 AndAlso Val(G.Rows(i).Cells(GC.ImportMessageId).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد الرسالة")
                G.Focus()
                G.CurrentCell = G.Rows(i).Cells(GC.ImportMessageId)
                Return
            End If
            If Val(G.Rows(i).Cells(GC.CostTypeId).Value) = 0 AndAlso Val(G.Rows(i).Cells(GC.StoreId).Value) > 0 AndAlso Val(G.Rows(i).Cells(GC.StoreInvoiceNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد مسلسل الفاتورة")
                G.Focus()
                G.CurrentCell = G.Rows(i).Cells(GC.StoreInvoiceNo)
                Return
            End If

        Next

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        ElseIf Val(Diff.Text) <> 0 Then
            bm.ShowMSG("المدين لا يساوى الدائن")
            Return
        End If


        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & SubId & "=" & EntryTypeId.SelectedValue.ToString)
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text
            State = BasicMethods.SaveState.Insert
        End If

        If Md.MyProjectType = ProjectType.X Then
            State = BasicMethods.SaveState.All
        End If

        bm.DefineValues()
        If Not bm.Save(New String() {SubId, SubId2}, New String() {Val(EntryTypeId.SelectedValue), InvoiceNo.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                InvoiceNo.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G, TableDetailsName, New String() {SubId, SubId2}, New String() {Val(EntryTypeId.SelectedValue), InvoiceNo.Text}, New String() {"MainDebit", "MainCredit", "Exchange", "Debit", "Credit", "MainAccNo", "SubAccNo", "CurrencyId", "AnalysisId", "CostCenterId", "CostCenterSubId", "Notes", "DocNo", "CostTypeId", "PurchaseAccNo", "ImportMessageId", "StoreId", "StoreInvoiceNo", "IsDocumented", "IsNotDocumented"}, New String() {GC.MainDebit, GC.MainCredit, GC.Exchange, GC.Debit, GC.Credit, GC.MainAccNo, GC.SubAccNo, GC.CurrencyId, GC.AnalysisId, GC.CostCenterId, GC.CostCenterSubId, GC.Notes, GC.DocNo, GC.CostTypeId, GC.PurchaseAccNo, GC.ImportMessageId, GC.StoreId, GC.StoreInvoiceNo, GC.IsDocumented, GC.IsNotDocumented}, New VariantType() {VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.String, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.Decimal}, New String() {GC.MainAccNo}, "Line", "Line") Then Return
        AllowSave = True

        If Not DontClear Then
            If Md.MyProjectType = ProjectType.X Then
                bm.ShowMSG("تم الحفظ", True)
                G.ReadOnly = True
            Else
                btnNew_Click(sender, e)
            End If
        End If

        SaveSetting("OMEGA", "Entry", "EntryTypeId", EntryTypeId.SelectedValue)
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId, SubId2}, "Min", dt)
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

            G.ReadOnly = False

            If EntryTypeId.SelectedIndex < 1 Then
                G.Enabled = False
            Else
                G.Enabled = True
            End If

            Dim d As DateTime = Nothing
            Try
                If d.Year = 1 Then d = bm.MyGetDate
                d = DayDate.SelectedDate
            Catch ex As Exception
            End Try

            If Md.MyProjectType = ProjectType.X Then
                For i As Integer = 0 To G.Rows.Count - 1
                    bm.ExecuteNonQuery("updateCustomersTempBal0", {"Id"}, {Val(G.Rows(i).Cells(GC.SubAccNo).Value)})
                Next
            End If

            Dim s As String = InvoiceNo.Text
            bm.ClearControls(False)
            If Md.MyProjectType = ProjectType.X Then
                InvoiceNo.Text = s
            Else
                InvoiceNo.Clear()
            End If

            MainAccName.Content = ""
            SubAccName.Content = ""
            AnalysisName.Content = ""
            CostCenterName.Content = ""
            CostCenterSubName.Content = ""

            DayDate.SelectedDate = bm.MyGetDate()
            If Md.MyProjectType = ProjectType.X Then
                DayDate.SelectedDate = d
            End If

            G.Rows.Clear()
            CalcTotal()
            LoadTree()
        Catch
        End Try

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "=" & EntryTypeId.SelectedValue.ToString & " and " & SubId2 & "='" & InvoiceNo.Text.Trim & "'")

            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where " & SubId & "=" & EntryTypeId.SelectedValue.ToString & " and " & SubId2 & "='" & InvoiceNo.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from EntryAttachments where " & SubId & "=" & EntryTypeId.SelectedValue.ToString & " and InvoiceNo='" & InvoiceNo.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId, SubId2}, New String() {Val(EntryTypeId.SelectedValue), InvoiceNo.Text}, "Back", dt)
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
        bm.RetrieveAll(New String() {SubId, SubId2}, New String() {Val(EntryTypeId.SelectedValue), InvoiceNo.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles InvoiceNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub

    

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
                CalcTotal()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub PrintPone(ByVal sender As System.Object)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@EntryTypeId", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {Val(EntryTypeId.SelectedValue), InvoiceNo.Text, CType(Parent, Page).Title}
        If G.Columns(GC.CostTypeId).Visible Then
            rpt.Rpt = "EntryOne.rpt"
        Else
            rpt.Rpt = "EntryOneMain.rpt"
        End If

        rpt.Show()
    End Sub


    Dim LopCalc As Boolean = False
    Private Sub CalcTotal()
        If LopCalc Or lop Then Return
        LopCalc = True
        Try
            Debit.Text = Math.Round(0, 2)
            Credit.Text = Math.Round(0, 2)
            Diff.Text = Math.Round(0, 2)
            For i As Integer = 0 To G.Rows.Count - 1
                Debit.Text += Val(G.Rows(i).Cells(GC.Debit).Value)
                Credit.Text += Val(G.Rows(i).Cells(GC.Credit).Value)
            Next
            Diff.Text = Val(Debit.Text) - Val(Credit.Text)
        Catch ex As Exception
        End Try


        Debit.Text = String.Format("{0:0,0.00}", Debit.Text)
        Credit.Text = String.Format("{0:0,0.00}", Credit.Text)
        Diff.Text = String.Format("{0:0,0.00}", Diff.Text)

        LopCalc = False
    End Sub

    Dim DontClear As Boolean = False

    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        'e.Handled = True
        If Md.ShowGridAccCombo Then Return
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
                dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.MainAccNo).Value & "')")
                If dt.Rows.Count > 0 AndAlso bm.ShowHelpGrid(dt.Rows(0)("TableName"), G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccNo), G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccName), e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.MainAccNo).Value & "'") Then
                    SubAccName.Content = G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccName).Value
                    'GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, G.CurrentCell.RowIndex))
                    If G.Columns(GC.AnalysisId).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisId)
                    ElseIf G.Columns(GC.CostCenterId).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId)
                    Else
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
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
                If bm.ShowHelpGridMultiColumns("الفواتير", G.CurrentRow.Cells(GC.StoreInvoiceNo), G.CurrentRow.Cells(GC.StoreInvoiceNo), e, "select cast(InvoiceNo as varchar(100)) 'الفاتورة',dbo.GetSupplierName(ToId) 'المورد',DocNo 'رقم عقد المورد',cast(TotalAfterDiscount as nvarchar(100)) 'إجمالي الفاتورة',cast(OrderTypeId as nvarchar(100)) 'مسلسل الطلبية',dbo.GetOrderTypes(OrderTypeId) 'اسم الطلبية' from SalesMaster where StoreId=" & G.CurrentRow.Cells(GC.StoreId).Value & " and Flag=" & Sales.FlagState.الاستيراد & " and ToId=" & G.CurrentRow.Cells(GC.SubAccNo).Value) Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")
    End Sub

    Private Sub G_CellBeginEdit(sender As Object, e As Forms.DataGridViewCellCancelEventArgs)
        If e.ColumnIndex = G.Columns(GC.MainAccNo).Index Then
            G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = False
        End If

        If CType(G.Rows(e.RowIndex).Cells(GC.CostTypeId), System.Windows.Forms.DataGridViewComboBoxCell).Value Is Nothing Then
            CType(G.Rows(e.RowIndex).Cells(GC.CostTypeId), System.Windows.Forms.DataGridViewComboBoxCell).Value = "0"
        End If

        If Val(G.Rows(G.CurrentRow.Index).Cells(GC.Debit).Value) + Val(G.Rows(G.CurrentRow.Index).Cells(GC.Credit).Value) <> 0 AndAlso G.CurrentRow.Index > 0 Then
            If G.Rows(G.CurrentRow.Index).Cells(GC.DocNo).Value Is Nothing OrElse G.Rows(G.CurrentRow.Index).Cells(GC.DocNo).Value = "" Then
                G.Rows(G.CurrentRow.Index).Cells(GC.DocNo).Value = G.Rows(G.CurrentRow.Index - 1).Cells(GC.DocNo).Value
            End If
            If G.Rows(G.CurrentRow.Index).Cells(GC.Notes).Value Is Nothing OrElse G.Rows(G.CurrentRow.Index).Cells(GC.Notes).Value = "" Then
                G.Rows(G.CurrentRow.Index).Cells(GC.Notes).Value = G.Rows(G.CurrentRow.Index - 1).Cells(GC.Notes).Value
            End If
        End If
    End Sub

    Private Sub G_SelectionChanged(sender As Object, e As EventArgs)
        Try
            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.MainAccNo).Index, G.CurrentRow.Index))
            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, G.CurrentRow.Index))
            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.AnalysisId).Index, G.CurrentRow.Index))
            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.CostCenterId).Index, G.CurrentRow.Index))
            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.CostCenterSubId).Index, G.CurrentRow.Index))

            MainAccName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.MainAccName).Value
            SubAccName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.SubAccName).Value
            AnalysisName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.AnalysisName).Value
            CostCenterName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.CostCenterName).Value
            CostCenterSubName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.CostCenterSubName).Value

            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.PurchaseAccNo).Index, G.CurrentRow.Index))
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ImportMessageId).Index, G.CurrentRow.Index))
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.StoreId).Index, G.CurrentRow.Index))
        Catch
        End Try
    End Sub





    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDownload.Click
        Try
            MyImagedata = Nothing
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize <> 18 Then Return
            Dim s As New Forms.SaveFileDialog With {.Filter = "All files (*.*)|*.*"}
            s.FileName = CType(TreeView1.SelectedItem, TreeViewItem).Header

            If IsNothing(sender) Then
                MyBath = bm.GetNewTempName(s.FileName)
            Else
                If Not s.ShowDialog = Forms.DialogResult.OK Then Return
                MyBath = s.FileName
            End If

            btnDownload.IsEnabled = False
            F0 = Val(EntryTypeId.SelectedValue)
            F1 = InvoiceNo.Text
            F2 = CType(TreeView1.SelectedItem, TreeViewItem).Tag
            BackgroundWorker1.RunWorkerAsync()
        Catch ex As Exception
        End Try
    End Sub
    Dim F2 As String = "", F1 As String = "", F0 As String = ""
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim myCommand As SqlClient.SqlCommand
            myCommand = New SqlClient.SqlCommand("select Image from EntryAttachments where " & SubId & "='" & F0 & "' and InvoiceNo='" & F1 & "' and AttachedName='" & F2 & "'" & bm.AppendWhere, con)
            If con.State <> ConnectionState.Open Then con.Open()
            MyImagedata = CType(myCommand.ExecuteScalar(), Byte())
        Catch ex As Exception
        End Try
        con.Close()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            File.WriteAllBytes(MyBath, MyImagedata)
            Process.Start(MyBath)
        Catch ex As Exception
        End Try
        btnDownload.IsEnabled = True
    End Sub

    Dim MyImagedata() As Byte
    Dim MyBath As String = ""
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteFile.Click
        Try
            If CType(TreeView1.SelectedItem, TreeViewItem).FontSize = 18 Then
                If bm.ShowDeleteMSG("MsgDeleteFile") Then
                    bm.ExecuteNonQuery("delete from EntryAttachments where " & SubId & "=" & EntryTypeId.SelectedValue.ToString & " and InvoiceNo='" & InvoiceNo.Text & "' and AttachedName='" & TreeView1.SelectedItem.Header & "'" & bm.AppendWhere)
                    LoadTree()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadTree()
        Dim dt As DataTable = bm.ExecuteAdapter("select AttachedName from EntryAttachments where " & SubId & "=" & EntryTypeId.SelectedValue.ToString & " and InvoiceNo=" & InvoiceNo.Text & bm.AppendWhere)
        TreeView1.Items.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim nn As New TreeViewItem
            nn.Foreground = Brushes.DarkRed
            nn.FontSize = 18
            nn.Tag = dt.Rows(i)(0).ToString
            nn.Header = dt.Rows(i)(0).ToString
            TreeView1.Items.Add(nn)
        Next
    End Sub

    Dim AllowSave As Boolean = False
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAttach.Click
        DontClear = True
        btnSave_Click(btnSave, Nothing)
        DontClear = False
        If Not AllowSave Then Return

        Dim o As New Forms.OpenFileDialog
        o.Multiselect = True
        If o.ShowDialog = Forms.DialogResult.OK Then
            For i As Integer = 0 To o.FileNames.Length - 1
                bm.SaveFile("EntryAttachments", "EntryTypeId", Val(EntryTypeId.SelectedValue), "InvoiceNo", InvoiceNo.Text, "AttachedName", (o.FileNames(i).Split("\"))(o.FileNames(i).Split("\").Length - 1), "Image", o.FileNames(i))
            Next
        End If
        LoadTree()
    End Sub


    Private Sub TreeView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TreeView1.MouseDoubleClick
        Button4_Click(Nothing, Nothing)
    End Sub


    Private Sub EntryTypeId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles EntryTypeId.SelectionChanged
        If lv OrElse lop Then
            Return
        End If
        btnNew_Click(Nothing, Nothing)
        txtID_Leave(Nothing, Nothing)
        'SaveSetting("OMEGA", "Entry", "EntryTypeId", EntryTypeId.SelectedValue)
    End Sub

    Dim SearchLop As Boolean = False
    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnSearch.Click
        SearchLop = True
        bm.DefineValues()
        bm.SearchTable({SubId, SubId2}, {EntryTypeId.SelectedValue, InvoiceNo.Text}, cboSearch, "", {DayDate}, , True)
        SearchLop = False
    End Sub

    Private Sub cboSearch_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSearch.SelectionChanged
        If SearchLop Then Return
        InvoiceNo.Text = cboSearch.SelectedValue.ToString
        txtID_Leave(Nothing, Nothing)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As RoutedEventArgs) Handles btnEdit.Click
        G.ReadOnly = False
    End Sub


End Class
