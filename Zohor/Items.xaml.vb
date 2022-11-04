Imports System.Data

Public Class Items
    Public TableName As String = "Items"
    Public SubId As String = "Id"
    Public SubName As String = "Name"
    Public TableDetailsName As String = "ItemPriceLists"


    WithEvents G As New MyGrid

    Dim dt As New DataTable
    Dim bm As New BasicMethods
    Public AllowGenerateItemId As Boolean = False
    Public PadLeftCount As Integer = 5

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast, txtID}, {})
        LoadResource()

        LoadWFH()
        

        lblStoreId.Visibility = Visibility.Hidden
        StoreId.Visibility = Visibility.Hidden
        StoreName.Visibility = Visibility.Hidden
        lblPrintingGroupId.Visibility = Visibility.Hidden
        PrintingGroupId.Visibility = Visibility.Hidden
        PrintingGroupName.Visibility = Visibility.Hidden

        ItemType.Visibility = Visibility.Hidden
        lblItemType.Visibility = Visibility.Hidden
        btnItemComponants.Visibility = Visibility.Hidden

        ImportPrice.Visibility = Visibility.Hidden
        ImportPriceSub.Visibility = Visibility.Hidden
        ImportPriceSub2.Visibility = Visibility.Hidden
        lblImportPrice.Visibility = Visibility.Hidden
        lblImportPriceSub.Visibility = Visibility.Hidden

        bm.FillCombo("ItemUnits", ItemUnitId, "")
        If Md.ShowQtySub Then
            ItemUnitId.Visibility = Visibility.Hidden
        Else
            Unit.Visibility = Visibility.Hidden
            
            lblUnitSub_Copy.Visibility = Visibility.Hidden
            lblUnitSub_Copy1.Visibility = Visibility.Hidden
            lblUnitSub.Visibility = Visibility.Hidden
            lblPurchasePriceSub.Visibility = Visibility.Hidden
            lblSalesPriceSub.Visibility = Visibility.Hidden
            lblUnitCount.Visibility = Visibility.Hidden

            UnitSub.Visibility = Visibility.Hidden
            PurchasePriceSub.Visibility = Visibility.Hidden
            SalesPriceSub.Visibility = Visibility.Hidden
            UnitCount.Visibility = Visibility.Hidden

            UnitSub2.Visibility = Visibility.Hidden
            PurchasePriceSub2.Visibility = Visibility.Hidden
            SalesPriceSub2.Visibility = Visibility.Hidden
            UnitCount2.Visibility = Visibility.Hidden
        End If

        bm.Fields = New String() {SubId, SubName, "EnName", "GroupId", "TypeId", "ItemUnitId", "PrintingGroupId", "StoreId", "PurchasePrice", "PurchasePriceSub", "PurchasePriceSub2", "SalesPrice", "SalesPriceSub", "SalesPriceSub2", "ItemType", "Unit", "UnitSub", "UnitSub2", "UnitCount", "UnitCount2", "Adding", "IsTables", "IsTakeAway", "IsDelivary", "Limit", "Barcode", "IsStopped", "Flag", "ImportPrice", "ImportPriceSub", "ImportPriceSub2", "IsKidneysWash", "CodeOnPackage", "IsService", "CountryId", "AllowEditSalesPrice", "IsHiddenFromLimit"}
        bm.control = New Control() {txtID, txtName, txtEnName, GroupId, TypeId, ItemUnitId, PrintingGroupId, StoreId, PurchasePrice, PurchasePriceSub, PurchasePriceSub2, SalesPrice, SalesPriceSub, SalesPriceSub2, ItemType, Unit, UnitSub, UnitSub2, UnitCount, UnitCount2, Adding, IsTables, IsTakeAway, IsDelivary, Limit, Barcode, IsStopped, Flag, ImportPrice, ImportPriceSub, ImportPriceSub2, IsKidneysWash, CodeOnPackage, IsService, CountryId, AllowEditSalesPrice, IsHiddenFromLimit}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName

        If Not Md.MyProjectType = ProjectType.X Then
            IsKidneysWash.Visibility = Visibility.Hidden
        End If

        If Not (Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.Zohor) Then
            Flag.Visibility = Visibility.Hidden
        End If

        If Md.ShowBarcode Then
            lblBarcode.Visibility = Visibility.Visible
            Barcode.Visibility = Visibility.Visible
        Else
            lblBarcode.Visibility = Visibility.Hidden
            Barcode.Visibility = Visibility.Hidden
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            ItemType.Visibility = Visibility.Visible
            lblItemType.Visibility = Visibility.Visible
            ComboBoxItem1.Content = "خام - أجزاء قابل للتركيب"
            ComboBoxItem2.Content = "نصف مصنع"
            ComboBoxItem3.Content = "تام - بيع"
            ComboBoxItem4.Content = "كومبو - بيع"

            ComboBoxItem2.Visibility = Visibility.Collapsed
            IsService.Visibility = Visibility.Collapsed
            'ImportPrice.Visibility = Visibility.Visible
            'lblImportPrice.Visibility = Visibility.Visible
        End If

        If Not Md.ShowPriceLists Then
            WFH.Visibility = Visibility.Hidden
        End If

        btnNew_Click(sender, e)
    End Sub

    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared SalesPrice As String = "SalesPrice"
        Shared SalesPriceSub As String = "SalesPriceSub"
        Shared SalesPriceSub2 As String = "SalesPriceSub2"
    End Structure

    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Id, "Id")
        G.Columns.Add(GC.Name, "قائمة الأسعار")
        G.Columns.Add(GC.SalesPrice, "وحدة فرعية")
        G.Columns.Add(GC.SalesPriceSub, "وحدة رئيسية 1")
        G.Columns.Add(GC.SalesPriceSub2, "وحدة رئيسية 2")

        G.Columns(GC.Name).FillWeight = 250
        G.Columns(GC.Id).Visible = False

        G.Columns(GC.Name).ReadOnly = True

        G.Columns(GC.SalesPriceSub).Visible = Md.ShowQtySub
        G.Columns(GC.SalesPriceSub2).Visible = Md.ShowQtySub

        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False

    End Sub

    Structure GC2
        Shared Barcode As String = "Barcode"
    End Structure

    Structure GC1
        Shared StoreId As String = "StoreId"
        Shared StoreName As String = "StoreName"
        Shared Price As String = "Price"
        Shared PriceSub As String = "PriceSub"
        'Shared PriceSub2 As String = "PriceSub2"
        Shared Limit As String = "Limit"
        Shared Maximum As String = "Maximum"
    End Structure


    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        IsNew = False
        bm.FillControls(Me)
        bm.GetImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)

        GetGrid()



        GroupId_LostFocus(Nothing, Nothing)
        TypeId_LostFocus(Nothing, Nothing)
        CountryId_LostFocus(Nothing, Nothing)
        PrintingGroupId_LostFocus(Nothing, Nothing)
        StoreId_LostFocus(Nothing, Nothing)
    End Sub

    Sub GetGrid()

        Dim dt As DataTable = bm.ExecuteAdapter("select T.Id,T.Name,T2.SalesPrice,T2.SalesPriceSub,T2.SalesPriceSub2 from PriceLists T left join ItemPriceLists T2 on(T.Id=T2.PriceListId and T2.ItemId=" & txtID.Text & ")")

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("Id").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("Name").ToString
            G.Rows(i).Cells(GC.SalesPrice).Value = dt.Rows(i)("SalesPrice").ToString
            G.Rows(i).Cells(GC.SalesPriceSub).Value = dt.Rows(i)("SalesPriceSub").ToString
            G.Rows(i).Cells(GC.SalesPriceSub2).Value = dt.Rows(i)("SalesPriceSub2").ToString
        Next
        G.RefreshEdit()

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowPrint = False
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If

        If Md.MyProjectType = ProjectType.X AndAlso ItemUnitId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد الوحدة")
            ItemUnitId.Focus()
            Return
        End If

        If Md.MyProjectType = ProjectType.X AndAlso Val(CountryId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblCountryId.Content)
            CountryId.Focus()
            Return
        End If

        G.EndEdit()

        UnitCount.Text = Val(UnitCount.Text)
        PurchasePrice.Text = Val(PurchasePrice.Text)
        PurchasePriceSub.Text = Val(PurchasePriceSub.Text)
        SalesPrice.Text = Val(SalesPrice.Text)
        SalesPriceSub.Text = Val(SalesPriceSub.Text)

        UnitCount2.Text = Val(UnitCount2.Text)
        PurchasePriceSub2.Text = Val(PurchasePriceSub2.Text)
        SalesPriceSub2.Text = Val(SalesPriceSub2.Text)
        Limit.Text = Val(Limit.Text)



        Dim x As Integer = Val(bm.ExecuteScalar("select Id from " & TableName & " where Name='" & txtName.Text.Trim & "' and Id<>" & Val(txtID.Text)))
        If x > 0 Then
            bm.ShowMSG("تم تكرار الاسم بمسلسل رقم " & x)
            Return
        End If



        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If Not bm.SaveGrid(G, TableDetailsName, New String() {"ItemId"}, New String() {txtID.Text}, New String() {"PriceListId", "SalesPrice", "SalesPriceSub", "SalesPriceSub2"}, New String() {GC.Id, GC.SalesPrice, GC.SalesPriceSub, GC.SalesPriceSub2}, New VariantType() {VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal}, New String() {GC.Id}) Then Return


        

        bm.SaveImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)

        If Not DontClear Then
            If ItemType.SelectedIndex = 3 AndAlso Not bm.IF_Exists("select * from ItemComponants where MainItemId='" & txtID.Text & "'") Then
                bm.ShowMSG("برجاء تحديد مكونات الصنف")
                Return
            End If
        End If

        If Not DontClear Then btnNew_Click(sender, e)
        AllowPrint = True
    End Sub

    Dim AllowPrint As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Dim IsNew As Boolean = True
    Sub ClearControls()
        IsNew = True
        bm.ClearControls()
        ItemType.SelectedIndex = 2

        bm.SetNoImage(Image1)

        GetGrid()

        

        GroupName.Clear()
        TypeName.Clear()
        CountryName.Clear()
        PrintingGroupName.Clear()
        StoreName.Clear()

        txtName.Clear()

        If AllowGenerateItemId Then
            txtID.Text = ""
        Else
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If txtID.Text = "" Then txtID.Text = "1"
        End If
        txtID_TextChanged(Nothing, Nothing)

        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Val(bm.ExecuteScalar("select dbo.GetItemUsingCount('" & txtID.Text.Trim & "')")) > 0 Then
                bm.ShowMSG("غير مسموح بمسح أصناف عليها حركات")
                Exit Sub
            End If
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where ItemId='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from ItemsBarcode where ItemId='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from ItemsPrices where ItemId='" & txtID.Text.Trim & "'")

            btnNew_Click(sender, e)
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
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            txtName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        txtName.SelectAll()
        txtName.Focus()
        txtName.SelectAll()
        txtName.Focus()
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles StoreId.KeyUp
        bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")")
    End Sub

    Private Sub TypeId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles TypeId.KeyUp
        bm.ShowHelp("Types", TypeId, TypeName, e, "select cast(Id as varchar(100)) Id,Name from Types where GroupId=" & GroupId.Text.Trim, "Types", {"GroupId"}, {Val(GroupId.Text)})
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown, GroupId.KeyDown, CountryId.KeyDown, TypeId.KeyDown, PrintingGroupId.KeyDown, StoreId.KeyDown, ItemType.KeyDown, Limit.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyUp(sender As Object, e As KeyEventArgs) Handles txtID.KeyUp
        Try
            Dim str As String = ""
            If Md.MyProjectType = ProjectType.Zohor Then
                str = " where (GroupId=" & Val(GroupId.Text) & " or " & Val(GroupId.Text) & "=0) and (TypeId=" & Val(TypeId.Text) & " or " & Val(TypeId.Text) & "=0)"
            End If
            If bm.ShowHelp("Items", txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from Items " & str) Then txtName.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub PrintingGroupId_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles PrintingGroupId.KeyUp
        bm.ShowHelp("PrintingGroups", PrintingGroupId, PrintingGroupName, e, "select cast(Id as varchar(100)) Id,Name from PrintingGroups")
    End Sub


    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles PurchasePrice.KeyDown, PurchasePriceSub.KeyDown, SalesPrice.KeyDown, SalesPriceSub.KeyDown, PurchasePriceSub2.KeyDown, SalesPriceSub2.KeyDown, UnitCount.KeyDown, UnitCount2.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub GroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles GroupId.LostFocus
        bm.LostFocus(GroupId, GroupName, "select Name from Groups where Id=" & GroupId.Text.Trim())
        TypeId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CountryId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId.LostFocus
        bm.LostFocus(CountryId, CountryName, "select Name from Countries where Id=" & CountryId.Text.Trim())
    End Sub

    Private Sub PrintingGroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles PrintingGroupId.LostFocus
        bm.LostFocus(PrintingGroupId, PrintingGroupName, "select Name from PrintingGroups where Id=" & PrintingGroupId.Text.Trim())
    End Sub

    Private Sub TypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TypeId.LostFocus

        bm.LostFocus(TypeId, TypeName, "select Name from Types where GroupId=" & GroupId.Text.Trim & " and Id=" & TypeId.Text.Trim())
        If Val(TypeId.Text) = 0 Then
            Return
        End If


        If IsNew AndAlso AllowGenerateItemId Then
            Dim str As String = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where GroupId=" & Val(GroupId.Text) & " and TypeId=" & Val(TypeId.Text))

            If str = "" Then
                str = GroupId.Text & TypeId.Text.PadLeft(2, "0") & "1".PadLeft(PadLeftCount, "0")
            End If


            If bm.IF_Exists("select Id from Items where Id=" & str) Then
                bm.ShowMSG("يوجد صنف بنفس المسلسل " & str)
                Return
            Else
                txtID.Text = str
            End If

        End If


    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub

    Private Sub GroupId_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles GroupId.KeyUp
        If bm.ShowHelp("Groups", GroupId, GroupName, e, "select cast(Id as varchar(100)) Id,Name from Groups") Then
            GroupId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CountryId_KeyDown(ByVal sender As System.Object, ByVal e As Input.KeyEventArgs) Handles CountryId.KeyUp
        If bm.ShowHelp("Countries", CountryId, CountryName, e, "select cast(Id as varchar(100)) Id,Name from Countries", "Countries") Then
            CountryId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub btnSetImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetImage.Click
        If Md.MyProjectType = ProjectType.X Then
            bm.SetImage(Image1, True, 600, 600)
        Else
            bm.SetImage(Image1)
        End If
    End Sub

    Private Sub btnSetNoImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetNoImage.Click
        bm.SetNoImage(Image1, False, True)
    End Sub

    Private Sub SalesUnitCount_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount.LostFocus
        Try
            If Val(UnitCount.Text) = 0 Then
                SalesPriceSub.Text = 0
            Else
                SalesPriceSub.Text = Val(SalesPrice.Text) * Val(UnitCount.Text)
            End If
        Catch ex As Exception
            SalesPriceSub.Text = 0
        End Try
    End Sub

    Private Sub SalesUnitCount2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount2.LostFocus
        Try
            If Val(UnitCount2.Text) = 0 Then
                SalesPriceSub2.Text = 0
            Else
                SalesPriceSub2.Text = Val(SalesPrice.Text) * Val(UnitCount2.Text)
            End If
        Catch ex As Exception
            SalesPriceSub2.Text = 0
        End Try
    End Sub

    Private Sub PurchaseUnitCount_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount.LostFocus
        Try
            If Val(UnitCount.Text) = 0 Then
                PurchasePriceSub.Text = 0
            Else
                PurchasePriceSub.Text = Val(PurchasePrice.Text) * Val(UnitCount.Text)
            End If
        Catch ex As Exception
            PurchasePriceSub.Text = 0
        End Try
    End Sub

    Private Sub PurchaseUnitCount2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount2.LostFocus
        Try
            If Val(UnitCount2.Text) = 0 Then
                PurchasePriceSub2.Text = 0
            Else
                PurchasePriceSub2.Text = Val(PurchasePrice.Text) * Val(UnitCount2.Text)
            End If
        Catch ex As Exception
            PurchasePriceSub2.Text = 0
        End Try
    End Sub

    Private Sub ItemType_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ItemType.SelectionChanged
        Select Case ItemType.SelectedIndex
            Case 0, 1
                IsDelivary.IsChecked = False
                IsTables.IsChecked = False
                IsTakeAway.IsChecked = False
            Case 2, 3
                IsDelivary.IsChecked = True
                IsTables.IsChecked = True
                IsTakeAway.IsChecked = True
        End Select

        If ItemType.SelectedIndex = 3 AndAlso Md.MyProjectType = ProjectType.X Then
            btnItemComponants.Visibility = Visibility.Visible
        Else
            btnItemComponants.Visibility = Visibility.Hidden
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


    End Sub


    Private Sub btnItemComponants_Click(sender As Object, e As RoutedEventArgs) Handles btnItemComponants.Click

        DontClear = True
        btnSave_Click(sender, Nothing)
        DontClear = False

        If Not AllowPrint Then Return

        Dim frm As New MyWindow With {.Title = CType(sender, Button).Content, .WindowState = WindowState.Maximized}
        Dim c As New ItemComponants With {.MyId = txtID.Text}
        frm.Content = c
        frm.Hide()
        frm.ShowDialog()
    End Sub

    Private Sub ItemUnitId_LostFocus(sender As Object, e As RoutedEventArgs) Handles ItemUnitId.LostFocus
        Unit.Text = ItemUnitId.Text
    End Sub

    Private Sub btnChangeItemId_Click(sender As Object, e As RoutedEventArgs) Handles btnChangeItemId.Click
        Dim frm As New Window 'With {.SizeToContent = True}
        frm.Content = New ChangeItemId With {.MyItemId = txtID.Text, .txtItemId = txtID}
        frm.ShowDialog()
        If CType(frm.Content, ChangeItemId).AllowChange Then
            txtID_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub txtID_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtID.TextChanged
        Barcode.Text = bm.ean13(txtID.Text)
    End Sub
End Class
