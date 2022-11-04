Imports System.Data

Public Class ItemsUpdate

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid


    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        If bm.TestIsLoaded(Me) Then Return
        bm.Addcontrol_MouseDoubleClick({StoreId, GroupId, TypeId})
        LoadWFH()

    End Sub

    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Unit As String = "Unit"
        Shared PurchasePrice As String = "PurchasePrice"
        Shared SalesPrice As String = "SalesPrice"
        Shared UnitSub As String = "UnitSub"
        Shared PurchasePriceSub As String = "PurchasePriceSub"
        Shared SalesPriceSub As String = "SalesPriceSub"
        Shared Limit As String = "Limit"
        Shared LimitSub As String = "LimitSub"
        Shared Maximum As String = "Maximum"
        Shared MaximumSub As String = "MaximumSub"
        Shared Target As String = "Target"
        Shared Bonus As String = "Bonus"
    End Structure

    Private Sub LoadWFH()
        WFH0.Child = G
        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Id, "كود الصنف")
        G.Columns.Add(GC.Name, "اسم الصنف")
        G.Columns.Add(GC.Unit, "الوحدة الرئيسية")
        G.Columns.Add(GC.PurchasePrice, "سعر الشراء")
        G.Columns.Add(GC.SalesPrice, "سعر البيع")
        G.Columns.Add(GC.UnitSub, "وحدة الفرعى")
        G.Columns.Add(GC.PurchasePriceSub, "سعر شراء الفرعى")
        G.Columns.Add(GC.SalesPriceSub, "سعر بيع الفرعى")
        G.Columns.Add(GC.Limit, "حد الطلب للوحدة الرئيسية")
        G.Columns.Add(GC.LimitSub, "حد الطلب للوحدة الفرعية")
        G.Columns.Add(GC.Maximum, "الحد الأقصى للوحدة الرئيسية")
        G.Columns.Add(GC.MaximumSub, "الحد الأقصى للوحدة الفرعية")
        G.Columns.Add(GC.Target, "المستهدف")
        G.Columns.Add(GC.Bonus, "البونص ")

        G.Columns(GC.Name).FillWeight = 300

        G.Columns(GC.Id).ReadOnly = True
        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Unit).ReadOnly = True
        G.Columns(GC.UnitSub).ReadOnly = True

        G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False
        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G.TabStop = False

    End Sub

    Private Sub GetData()
        dt = bm.ExecuteAdapter("select Id,Name,Unit,PurchasePrice,SalesPrice,UnitSub,PurchasePriceSub,dbo.GetStoreItemPrice(Id,'" & Val(StoreId.Text) & "')SalesPriceSub,Limit,dbo.GetStoreItemLimit(Id,'" & Val(StoreId.Text) & "')LimitSub,Maximum,dbo.GetStoreItemMaximum(Id,'" & Val(StoreId.Text) & "')MaximumSub,Target,Bonus from Items where (GroupId='" & Val(GroupId.Text) & "' or '" & Val(GroupId.Text) & "'=0) and (TypeId='" & Val(TypeId.Text) & "' or '" & Val(TypeId.Text) & "'=0)")
        If dt.Rows.Count = 0 Then Return
        StoreId.IsEnabled = False
        GroupId.IsEnabled = False
        TypeId.IsEnabled = False
        G.Rows.Add(dt.Rows.Count)
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)(GC.Id)
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)(GC.Name)
            G.Rows(i).Cells(GC.Unit).Value = dt.Rows(i)(GC.Unit)
            G.Rows(i).Cells(GC.PurchasePrice).Value = dt.Rows(i)(GC.PurchasePrice)
            G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(i)(GC.SalesPrice)
            G.Rows(i).Cells(GC.UnitSub).Value = dt.Rows(i)(GC.UnitSub)
            G.Rows(i).Cells(GC.PurchasePriceSub).Value = dt.Rows(i)(GC.PurchasePriceSub)
            G.Rows(i).Cells(GC.SalesPriceSub).Value = dt.Rows(i)(GC.SalesPriceSub)
            G.Rows(i).Cells(GC.Limit).Value = dt.Rows(i)(GC.Limit)
            G.Rows(i).Cells(GC.LimitSub).Value = dt.Rows(i)(GC.LimitSub)
            G.Rows(i).Cells(GC.Maximum).Value = dt.Rows(i)(GC.Maximum)
            G.Rows(i).Cells(GC.MaximumSub).Value = dt.Rows(i)(GC.MaximumSub)
            G.Rows(i).Cells(GC.Target).Value = dt.Rows(i)(GC.Target)
            G.Rows(i).Cells(GC.Bonus).Value = dt.Rows(i)(GC.Bonus)
        Next
    End Sub

    Private Sub btnNew_Click(sender As Object, e As RoutedEventArgs) Handles btnNew.Click
        GroupId.IsEnabled = True
        TypeId.IsEnabled = True
        StoreId.IsEnabled = True

        GroupId.Clear()
        TypeId.Clear()
        StoreId.Clear()

        GroupName.Clear()
        TypeName.Clear()
        StoreName.Clear()

        G.Rows.Clear()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        G.EndEdit()
        Dim str As String = ""
        For i As Integer = 0 To G.Rows.Count - 1
            str &= " update Items set PurchasePrice='" & Val(G.Rows(i).Cells(GC.PurchasePrice).Value.ToString) & "',SalesPrice='" & Val(G.Rows(i).Cells(GC.SalesPrice).Value.ToString) & "',PurchasePriceSub='" & Val(G.Rows(i).Cells(GC.PurchasePriceSub).Value.ToString) & "',SalesPriceSub='" & Val(G.Rows(i).Cells(GC.SalesPriceSub).Value.ToString) & "',Limit='" & Val(G.Rows(i).Cells(GC.Limit).Value.ToString) & "',LimitSub='" & Val(G.Rows(i).Cells(GC.LimitSub).Value.ToString) & "',Maximum='" & Val(G.Rows(i).Cells(GC.Maximum).Value.ToString) & "',MaximumSub='" & Val(G.Rows(i).Cells(GC.MaximumSub).Value.ToString) & "',Target='" & Val(G.Rows(i).Cells(GC.Target).Value.ToString) & "',Bonus='" & Val(G.Rows(i).Cells(GC.Bonus).Value.ToString) & "' where Id='" & Val(G.Rows(i).Cells(GC.Id).Value.ToString) & "'  delete ItemsPrices where ItemId='" & Val(G.Rows(i).Cells(GC.Id).Value.ToString) & "' AND StoreId='" & Val(StoreId.Text) & "' insert ItemsPrices(ItemId,StoreId,Price,Limit,Maximum) values ('" & Val(G.Rows(i).Cells(GC.Id).Value.ToString) & "','" & Val(StoreId.Text) & "','" & Val(G.Rows(i).Cells(GC.SalesPriceSub).Value.ToString) & "','" & Val(G.Rows(i).Cells(GC.LimitSub).Value.ToString) & "','" & Val(G.Rows(i).Cells(GC.MaximumSub).Value.ToString) & "')"
        Next
        bm.ExecuteScalar(Str)
        btnNew_Click(Nothing, Nothing)
    End Sub

    Private Sub TypeId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles TypeId.KeyUp
        bm.ShowHelp("Types", TypeId, TypeName, e, "select cast(Id as varchar(100)) Id,Name from Types where GroupId=" & GroupId.Text.Trim)
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles GroupId.KeyDown, TypeId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub GroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles GroupId.LostFocus
        bm.LostFocus(GroupId, GroupName, "select Name from Groups where Id=" & GroupId.Text.Trim())
        'TypeId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub TypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TypeId.LostFocus
        bm.LostFocus(TypeId, TypeName, "select Name from Types where GroupId=" & GroupId.Text.Trim & " and Id=" & TypeId.Text.Trim())
        GetData()
    End Sub

    Private Sub GroupId_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles GroupId.KeyUp
        If bm.ShowHelp("Groups", GroupId, GroupName, e, "select cast(Id as varchar(100)) Id,Name from Groups") Then
            GroupId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Stores") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Stores where Id=" & StoreId.Text.Trim())
        'GetData()
    End Sub

End Class
