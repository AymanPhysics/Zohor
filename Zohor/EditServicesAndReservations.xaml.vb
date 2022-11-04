Imports System.Data

Public Class EditServicesAndReservations
    
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Public Flag As Integer = 1

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave, btnSaveWithoutPrint}, {btnDelete}, {}, {})
        LoadResource()
        CurrentShift.Visibility = Visibility.Hidden
        InsertedDate.Visibility = Visibility.Hidden
    
        bm.FillCombo("Companies", CompanyId, "")


        btnNew_Click(sender, e)

        If Not Md.Manager Then
            bm.AppendWhere = " and username=" & Md.UserName
        End If

    End Sub


    Structure GC
        Shared MyLine As String = "MyLine"
        Shared ServiceGroupName As String = "ServiceGroupName"
        Shared ServiceTypeName As String = "ServiceTypeName"
        Shared Value As String = "Value"
        Shared Payed As String = "Payed"
        Shared Remaining As String = "Remaining"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.MyLine, "MyLine")
        G.Columns.Add(GC.ServiceGroupName, "المجموعة")
        G.Columns.Add(GC.ServiceTypeName, "النوع")

        G.Columns.Add(GC.Value, "السعر")
        G.Columns.Add(GC.Payed, "حصة المريض")
        G.Columns.Add(GC.Remaining, "حصة الشركة")

        If Not Md.Manager AndAlso Md.MyProjectType = ProjectType.Zohor Then
            G.Columns(GC.Value).ReadOnly = True
        End If

        G.Columns(GC.Remaining).ReadOnly = True
        G.Columns(GC.ServiceGroupName).FillWeight = 200
        G.Columns(GC.ServiceTypeName).FillWeight = 300

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
    End Sub


    Dim lop As Boolean = False
    Sub FillControls()
        bm.FillControls(Me)
        lop = True
        bm.FillControls(Me)
        If Not InsertedDate.Text = "" Then InsertedDate.Text = bm.ToStrDateTimeFormated(InsertedDate.Text)

        CaseID_LostFocus(Nothing, Nothing)


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

        DayDate.Focus()
    End Sub

    Dim lop2 As Boolean = False
    Dim IsNew As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveWithoutPrint.Click
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


        If InsertedDate.Text.Trim = "" Then InsertedDate.Text = bm.ExecuteScalar("select dbo.MyGetDateTime()")
        If InsertedDate.Text.Trim = "" Then Return





        btnNew_Click(sender, e)

    End Sub

    Private Sub ShowRPT()
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Flag", "@Id", "Header", "IsNew"}
        rpt.paravalue = New String() {Flag, txtID.Text, CType(Parent, Page).Title, IIf(IsNew, 1, 0)}
        rpt.Rpt = "ServicesONEG.rpt"
        If Md.MyProjectType = ProjectType.Zohor Then
            rpt.Rpt = "ServicesONEGZohor.rpt"
        End If
        rpt.Print()
    End Sub


    Sub NewId()
        txtFlag.Text = Flag
        txtID.Clear()
        'txtID.IsEnabled = False

        DayDate.IsEnabled = Md.Manager
        G.ReadOnly = False

        CaseId.IsEnabled = True
        IsNew = True
    End Sub

    Sub UndoNewId()
        txtID.IsEnabled = True

        DayDate.IsEnabled = Md.Manager
        G.ReadOnly = Not Md.Manager

        CaseId.IsEnabled = Md.Manager
        IsNew = False
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls(False)
        G.Rows.Clear()

        DayDate.SelectedDate = bm.MyGetDate()
        CaseID_LostFocus(Nothing, Nothing)

        btnSave.IsEnabled = True
        btnSaveWithoutPrint.IsEnabled = True
        btnDelete.IsEnabled = True
        NewId()
        DayDate.Focus()
    End Sub

    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        

    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
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



    Private Sub LoadResource()
        btnSaveWithoutPrint.SetResourceReference(ContentProperty, "Save")
        btnSave.SetResourceReference(ContentProperty, "Print")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")


        lblId.SetResourceReference(ContentProperty, "Id")
        lblCaseId.SetResourceReference(ContentProperty, "CaseId")
        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblNotes.SetResourceReference(ContentProperty, "Notes")
    End Sub

    Private Sub GridCalcRow(sender As Object, e As Forms.DataGridViewCellEventArgs)
        Try
            Select Case e.ColumnIndex

                Case G.Columns(GC.Payed).Index, G.Columns(GC.Value).Index
                    G.CurrentRow.Cells(GC.Remaining).Value = Val(G.CurrentRow.Cells(GC.Value).Value) - Val(G.CurrentRow.Cells(GC.Payed).Value)
            End Select
        Catch ex As Exception
        End Try

    End Sub


End Class
