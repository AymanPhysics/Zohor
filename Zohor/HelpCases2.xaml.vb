Imports System.Data

Public Class HelpCases2
    Dim bm As New BasicMethods
    Public FirstColumn As String = "ID", SecondColumn As String = "Arabic Name", ThirdColumn As String = "Diagnose On Admission", FourthColumn As String = "Outcome", FifthColumn As String = "City"

    Dim dt As New DataTable
    Dim dv As New DataView
    Public Header As String = ""

    Public Proc As String = "CasesSearch2"
    Public tbl As String = "cases"
    Public MyLinkFlie As Integer = 13

    Private Sub Help_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()

        bm.FillCombo("CaseOutcome", CaseOutcomeId, "")
        bm.FillCombo("Cities", CityId, "where CountryId=1")

        Banner1.StopTimer = True
        Banner1.Header = Header
        Try 
            dt = bm.ExecuteAdapter(Proc)
            dt.TableName = "tbl"
            dt.Columns(0).ColumnName = FirstColumn
            dt.Columns(1).ColumnName = SecondColumn
            dt.Columns(2).ColumnName = ThirdColumn
            dt.Columns(3).ColumnName = FourthColumn
            dt.Columns(4).ColumnName = FifthColumn 

            dv.Table = dt
            DataGridView1.ItemsSource = dv
            DataGridView1.Columns(0).Width = 85
            DataGridView1.Columns(1).Width = 165
            DataGridView1.Columns(2).Width = 165
            DataGridView1.Columns(3).Width = 165
            DataGridView1.Columns(4).Width = 165
            DataGridView1.Columns(5).Width = 85
            DataGridView1.Columns(6).Width = 120

            DataGridView1.SelectedIndex = 0
        Catch
        End Try
         
        If Not _IsLoaded Then
            _IsLoaded = True
            txtArName.Focus()
        End If
    End Sub
    Dim _IsLoaded As Boolean = False


    Private Sub txtId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.GotFocus
        Try
            dv.Sort = FirstColumn
        Catch
        End Try
    End Sub

    Private Sub txtArName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtArName.GotFocus
        Try
            dv.Sort = SecondColumn
        Catch
        End Try
    End Sub

    Private Sub txtDiagnoseOnAdmission_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiagnoseOnAdmission.GotFocus
        Try
            dv.Sort = ThirdColumn
        Catch
        End Try
    End Sub

    Private Sub CaseOutcomeId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CaseOutcomeId.GotFocus
        Try
            dv.Sort = FourthColumn
        Catch
        End Try
    End Sub

    Private Sub CityId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CityId.GotFocus
        Try
            dv.Sort = FifthColumn
        Catch
        End Try
    End Sub  


    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtArName.TextChanged, txtDiagnoseOnAdmission.TextChanged, CaseOutcomeId.SelectionChanged, CityId.SelectionChanged
        Try
            Dim s1 As String = CaseOutcomeId.Items(CaseOutcomeId.SelectedIndex)("Name")
            Dim s2 As String = CityId.Items(CityId.SelectedIndex)("Name")
            dv.RowFilter = " [" & FirstColumn & "] like '%" & txtID.Text & "%' and [" & SecondColumn & "] like '%" & txtArName.Text & "%' and [" & ThirdColumn & "] like '%" & txtDiagnoseOnAdmission.Text & "%' and ([" & FourthColumn & "] like '%" & s1 & "%' or '" & s1 & "'='-') and ([" & FifthColumn & "] like '%" & s2 & "%' or '" & s2 & "'='-')"
        Catch ex As Exception
        End Try
    End Sub

    Public SelectedId As Integer = 0
    Public SelectedName As String = ""

    Private Sub DataGridView1_KeyDown(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.Key = Key.Enter Then
                SelectedId = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
                SelectedName = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
                Close()
            ElseIf e.Key = Input.Key.Escape Then
                Close()
            ElseIf e.Key = Input.Key.Up Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex - 1
                DataGridView1.ScrollIntoView(DataGridView1.SelectedItem)
            ElseIf e.Key = Input.Key.Down Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex + 1
                DataGridView1.ScrollIntoView(DataGridView1.SelectedItem)
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub DataGridView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles DataGridView1.MouseDoubleClick
        Try
            SelectedId = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
            SelectedName = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
            Close()
        Catch ex As Exception
        End Try
    End Sub

     
    Dim IsLoaded As Boolean = False
    Private Sub LoadResource()
        If IsLoaded Then Return
        IsLoaded = True
        Return
        'lblName.SetResourceReference(ContentProperty, "Name")

        FirstColumn = Application.Current.MainWindow.Resources.Item(FirstColumn)
        SecondColumn = Application.Current.MainWindow.Resources.Item(SecondColumn)
        ThirdColumn = Application.Current.MainWindow.Resources.Item(ThirdColumn)
        FourthColumn = Application.Current.MainWindow.Resources.Item(FourthColumn)
        FifthColumn = Application.Current.MainWindow.Resources.Item(FifthColumn)


    End Sub




    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If txtArName.Text.Trim = "" Then Return
        txtID.Clear()

        For i As Integer = 0 To dv.Table.Rows.Count - 1
            If txtArName.Text.Trim = dv.Table.Rows(i).Item(1).ToString Then Return
        Next

        txtArName.Text = txtArName.Text.Trim
        txtDiagnoseOnAdmission.Text = txtDiagnoseOnAdmission.Text.Trim

        Dim StrAccNo As String = bm.ExecuteScalar("select top 1 Id from Chart where LinkFile=" & MyLinkFlie)

        If Not bm.AddItemToTable(tbl, {"Name", "DiagnoseOnAdmission", "CaseOutcomeId", "CityId", "AccNo"}, {txtArName.Text, txtDiagnoseOnAdmission.Text, CaseOutcomeId.SelectedValue, CityId.SelectedValue, StrAccNo}) Then Return

        Help_Load(Nothing, Nothing)
        DataGridView1.SelectedIndex = dv.Table.Rows.Count - 1

        txtId_TextChanged(Nothing, Nothing)
    End Sub

End Class