Imports System.Data

Public Class HelpCases
    Dim bm As New BasicMethods
    Public FirstColumn As String = "ID", SecondColumn As String = "Arabic Name", ThirdColumn As String = "English Name", FourthColumn As String = "National ID", FifthColumn As String = "Tel", SixthColumn As String = "Mobile", SeventhColumn As String = "Address"

    Dim dt As New DataTable
    Dim dv As New DataView
    Public Header As String = ""

    Public Proc As String = "CasesSearch"
    Public tbl As String = "cases"
    Public ExistsOnly As Boolean = False
    Public AllowAdd As Boolean = True
    Public MyLinkFlie As Integer = 13

    Private Sub Help_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        Banner1.StopTimer = True
        Banner1.Header = Header
        If Not AllowAdd Then btnSave.Visibility = Visibility.Hidden
        Try
            If Md.MyProjectType = ProjectType.X Then
                Proc = "CasesSearch2"
                SixthColumn = "Age"
            End If
            dt = bm.ExecuteAdapter(Proc, {"ExistsOnly"}, {IIf(ExistsOnly, 1, 0)})
            dt.TableName = "tbl"
            dt.Columns(0).ColumnName = FirstColumn
            dt.Columns(1).ColumnName = SecondColumn
            dt.Columns(2).ColumnName = ThirdColumn
            dt.Columns(3).ColumnName = FourthColumn
            dt.Columns(4).ColumnName = FifthColumn
            dt.Columns(5).ColumnName = SixthColumn
            dt.Columns(6).ColumnName = SeventhColumn

            dv.Table = dt
            DataGridView1.ItemsSource = dv
            DataGridView1.Columns(0).Width = 85
            DataGridView1.Columns(1).Width = 165
            DataGridView1.Columns(2).Width = 165
            DataGridView1.Columns(3).Width = 90
            DataGridView1.Columns(4).Width = 80
            DataGridView1.Columns(5).Width = 85
            DataGridView1.Columns(6).Width = 120

            DataGridView1.SelectedIndex = 0
        Catch
        End Try

        'If Md.MyProject = Client.ClothesRed Then
        '    Banner1.R.Fill = System.Windows.Media.Brushes.Red
        '    Banner1.lblMain.Foreground = System.Windows.Media.Brushes.White
        '    Banner1.Background = System.Windows.Media.Brushes.White
        '    Banner1.Foreground = System.Windows.Media.Brushes.Red
        'End If

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

    Private Sub txtEnName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEnName.GotFocus
        Try
            dv.Sort = ThirdColumn
        Catch
        End Try
    End Sub

    Private Sub NationalId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NationalId.GotFocus
        Try
            dv.Sort = FourthColumn
        Catch
        End Try
    End Sub

    Private Sub txtTel_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTel.GotFocus
        Try
            dv.Sort = FifthColumn
        Catch
        End Try
    End Sub

    Private Sub txtMob_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMob.GotFocus
        Try
            dv.Sort = SixthColumn
        Catch
        End Try
    End Sub

    Private Sub txtAddress_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAddress.GotFocus
        Try
            dv.Sort = SeventhColumn
        Catch
        End Try
    End Sub


    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtArName.TextChanged, txtEnName.TextChanged, NationalId.TextChanged, txtTel.TextChanged, txtMob.TextChanged, txtAddress.TextChanged
        Try
            dv.RowFilter = " [" & FirstColumn & "] like '%" & txtID.Text & "%' and [" & SecondColumn & "] like '%" & txtArName.Text & "%' and [" & ThirdColumn & "] like '%" & txtEnName.Text & "%' and [" & FourthColumn & "] like '%" & NationalId.Text & "%' and ([" & FifthColumn & "] like '%" & txtTel.Text & "%' or [" & SixthColumn & "] like '%" & txtTel.Text & "%') and ([" & FifthColumn & "] like '%" & txtMob.Text & "%' or [" & SixthColumn & "] like '%" & txtMob.Text & "%') and [" & SeventhColumn & "] like '%" & txtAddress.Text & "%'"
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



    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        If txtArName.Text.Trim = "" Then Return

        If txtArName.Text.Trim.Split(" ").Length < 4 Then
            bm.ShowMSG("برجاء تحديد اسم رباعي")
            Return
        End If

        If txtMob.Text.Trim.Length < 11 Then
            bm.ShowMSG("برجاء تحديد موبيل صحيح")
            Return
        End If

        If Not txtMob.Text.Trim.StartsWith("01") Then
            bm.ShowMSG("برجاء تحديد موبيل صحيح")
            Return
        End If


        txtID.Clear()


        txtArName.Text = txtArName.Text.Trim
        txtEnName.Text = txtEnName.Text.Trim
        NationalId.Text = NationalId.Text.Trim
        txtTel.Text = txtTel.Text.Trim
        txtMob.Text = txtMob.Text.Trim
        txtAddress.Text = txtAddress.Text.Trim

        If DataGridView1.Items.Count > 0 Then Return

        txtEnName.Text = bm.GetEnName(txtArName.Text.Trim)

        Dim StrAccNo As String = bm.ExecuteScalar("select top 1 Id from Chart where LinkFile=" & MyLinkFlie)

        If Not bm.AddItemToTable(tbl, {"Name", "EnName", "NationalId", "HomePhone", IIf(Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X, "Age", "Mobile"), "Address", "AccNo", "EntryUser", "EntryDate"}, {txtArName.Text, txtEnName.Text, NationalId.Text, txtTel.Text, txtMob.Text, txtAddress.Text, StrAccNo, Md.UserName, bm.MyGetDateTime}) Then Return


        Help_Load(Nothing, Nothing)
        DataGridView1.SelectedIndex = dv.Table.Rows.Count - 1

        txtId_TextChanged(Nothing, Nothing)

        DataGridView1.SelectedIndex = 0
        SelectedId = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
        SelectedName = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
        Close()
    End Sub

    Dim IsLoaded As Boolean = False
    Private Sub LoadResource()
        If IsLoaded Then Return
        IsLoaded = True
        'lblName.SetResourceReference(ContentProperty, "Name")

        FirstColumn = Application.Current.MainWindow.Resources.Item(FirstColumn)
        SecondColumn = Application.Current.MainWindow.Resources.Item(SecondColumn)
        ThirdColumn = Application.Current.MainWindow.Resources.Item(ThirdColumn)
        FourthColumn = Application.Current.MainWindow.Resources.Item(FourthColumn)
        FifthColumn = Application.Current.MainWindow.Resources.Item(FifthColumn)
        SixthColumn = Application.Current.MainWindow.Resources.Item(SixthColumn)
        SeventhColumn = Application.Current.MainWindow.Resources.Item(SeventhColumn)

    End Sub
End Class