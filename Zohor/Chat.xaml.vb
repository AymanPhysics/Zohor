Imports System.ComponentModel
Imports System.Data
Imports System.Timers
Imports System.Windows.Threading

Public Class Chat
    Dim bm As New BasicMethods
    Dim dt As New DataTable

    Dim dv As New DataView
    Dim dv2 As New DataView
    WithEvents t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 5)}
    WithEvents GetNewChat As New BackgroundWorker
    WithEvents GetChatOne As New BackgroundWorker
    WithEvents work As New BackgroundWorker

    Private Sub Chat_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        GetNewChat.WorkerSupportsCancellation = True
        GetChatOne.WorkerSupportsCancellation = True
        work.WorkerSupportsCancellation = True


        SetChatListViewItemsSource()


        AddHandler t.Tick, AddressOf t_Tick
        AddHandler work.DoWork, AddressOf work_DoWork
        AddHandler work.RunWorkerCompleted, AddressOf work_RunWorkerCompleted
        AddHandler GetNewChat.DoWork, AddressOf GetNewChat_DoWork
        AddHandler GetNewChat.RunWorkerCompleted, AddressOf GetNewChat_RunWorkerCompleted
        AddHandler GetChatOne.DoWork, AddressOf GetChatOne_DoWork
        AddHandler GetChatOne.RunWorkerCompleted, AddressOf GetChatOne_RunWorkerCompleted
        FillList()
        newMessageTxt_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub SetChatListViewItemsSource()
        chatListView.ItemsSource = dv2
        Dim view As CollectionView = CollectionViewSource.GetDefaultView(dv2)
        Dim GroupDescription As New PropertyGroupDescription("MyDate")
        view.GroupDescriptions.Add(GroupDescription)


        If chatListView.Items.Count > 0 Then
            chatListView.SelectedItem = chatListView.Items(chatListView.Items.Count - 1)
            UpdateLayout()
            chatListView.ScrollIntoView(chatListView.Items(chatListView.Items.Count - 1))
        End If

    End Sub

    Dim CurrentToId As Integer = 0
    Dim CurrentToName As String = 0
    Private Sub t_Tick(sender As Object, e As EventArgs)
        FillChatList()

        If GetNewChat.IsBusy Then Return
        GetNewChat.RunWorkerAsync()
    End Sub

    Private Sub GetNewChat_DoWork(sender As Object, e As DoWorkEventArgs)
        If Val(bm.ExecuteScalar("GetNewChat", {"UserFromId"}, {Md.UserName})) = 0 Then
            e.Cancel = True
        End If
    End Sub

    Private Sub GetNewChat_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        If e.Cancelled Then Return

        btn1_Click(Nothing, Nothing)
        CType(Parent, Window).WindowState = WindowState.Maximized
        CType(Parent, Window).Activate()
        CType(Parent, Window).BringIntoView()

        FillList()

    End Sub

    Private Sub FillList()
        work.CancelAsync()
        If Not work.IsBusy Then
            work.RunWorkerAsync()
        End If

        FillChatList()
    End Sub

    Private Sub work_DoWork(sender As Object, e As DoWorkEventArgs)
        If work.CancellationPending Then
            e.Cancel = True
            Return
        End If

        dv = bm.ExecuteAdapter("GetChatList", {"UserToId", "IsGrouping"}, {Md.UserName, IIf(IsGrouping, 1, 0)}).DefaultView
        If Val(dv.Table.Rows(0)("Line").ToString) > 0 Then
            bm.ExecuteNonQuery("SetNewChatIsDelivered", {"UserFromId", "Line"}, {Md.UserName, dv.Table.Rows(0)("Line")})
        End If
    End Sub

    Private Sub work_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        mainListView.ItemsSource = dv
    End Sub

    Private Sub FillChatList()
        If GetChatOne.IsBusy OrElse CurrentToId = 0 Then
            GetChatOne_DoWork()
            Return
        End If
        t.Stop()
        GetChatOne.RunWorkerAsync()
        t.Start()
    End Sub

    Private Sub GetChatOne_DoWork()
        Try
            If CurrentToId = 0 Then Return
            dv2 = bm.ExecuteAdapter("GetChatOne", {"UserFromId", "UserToId", "IsGrouping"}, {Md.UserName, CurrentToId, IIf(IsGrouping, 1, 0)}).DefaultView

            If dv2.Table.Rows.Count > 0 AndAlso Val(dv2.Table.Rows(0)("Line").ToString) > 0 Then
                bm.ExecuteNonQuery("SetNewChatIsDelivered", {"UserFromId", "Line"}, {Md.UserName, dv2.Table.Rows(0)("Line")})
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub GetChatOne_RunWorkerCompleted()
        If dv2.Table.Rows.Count = 0 Then
            chatListView.ItemsSource = Nothing
            'SetChatListViewItemsSource()
            Return
        End If
        Dim Line As Integer = dv2.Table.Rows(dv2.Table.Rows.Count - 1)("Line")
        Dim DeliveredDate = dv2.Table.Rows(dv2.Table.Rows.Count - 1)("DeliveredDate")
        Dim SeenDate = dv2.Table.Rows(dv2.Table.Rows.Count - 1)("SeenDate")
        If chatListView.Items.Count = 0 Then
            SetChatListViewItemsSource()
        Else
            Dim Line2 As Integer = chatListView.Items(chatListView.Items.Count - 1)("Line")
            Dim DeliveredDate2 = chatListView.Items(chatListView.Items.Count - 1)("DeliveredDate")
            Dim SeenDate2 = chatListView.Items(chatListView.Items.Count - 1)("SeenDate")
            If Line <> Line2 OrElse (Not IsDBNull(DeliveredDate) AndAlso Not IsDBNull(DeliveredDate2) AndAlso DeliveredDate <> DeliveredDate2) OrElse (Not IsDBNull(SeenDate) AndAlso Not IsDBNull(SeenDate2) AndAlso SeenDate <> SeenDate2) Then
                SetChatListViewItemsSource()
            End If
        End If



        SetIsSeen()
    End Sub


    Private Sub searchText_TextChanged(sender As Object, e As TextChangedEventArgs) Handles searchText.TextChanged
        dv.RowFilter = "Name like '%" & searchText.Text.Trim & "%'"
    End Sub

    Private Sub sendBtn_Click(sender As Object, e As RoutedEventArgs) Handles sendBtn.Click
        If CurrentToId = 0 OrElse newMessageTxt.Text.Trim.Length = 0 Then Return
        Dim UserToIds As New DataTable
        UserToIds.Columns.Add("Id")
        If IsGrouping = 0 Then
            UserToIds.Rows.Add({CurrentToId})
        Else
            dt = bm.ExecuteAdapter("select Id from Employees where LevelId=" & CurrentToId)
            For i As Integer = 0 To dt.Rows.Count - 1
                UserToIds.Rows.Add({dt.Rows(i)(0)})
            Next
        End If

        If bm.ExecuteNonQuery("SendChat", {"UserFromId", "Msg", "ToLevelId", "UserToIds"}, {Val(Md.UserName), newMessageTxt.Text.Trim, IIf(IsGrouping, CurrentToId, 0), UserToIds}, {DbType.Int64, DbType.String, DbType.Int64, DbType.Object}) Then
            newMessageTxt.Clear()
            FillList()
            'GetChatOne.RunWorkerAsync()
            newMessageTxt.Focus()
        End If
    End Sub

    Private Sub newMessageTxt_TextChanged(sender As Object, e As TextChangedEventArgs) Handles newMessageTxt.TextChanged
        TestEnabled
    End Sub

    Private Sub mainListView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles mainListView.SelectionChanged
        If Not mainListView.SelectedItem Is Nothing Then
            CurrentToId = mainListView.SelectedItem("Id")
            CurrentToName = mainListView.SelectedItem("Name")
            ToName.Text = CurrentToName
            newMessageTxt.Clear()
            SetIsSeen
            FillList()
        End If
        TestEnabled()
    End Sub

    Private Sub SetIsSeen()
        If chatListView.Items.Count = 0 OrElse CurrentToId = 0 Then Return
        bm.ExecuteNonQuery("SetNewChatIsSeen", {"UserFromId", "UserToId", "Line"}, {Md.UserName, CurrentToId, chatListView.Items(chatListView.Items.Count - 1)("Line")})
    End Sub

    Sub TestEnabled()
        sendBtn.IsEnabled = newMessageTxt.Text.Length > 0 AndAlso CurrentToId > 0
    End Sub

    Private Sub newMessageTxt_KeyDown(sender As Object, e As KeyEventArgs) Handles newMessageTxt.KeyDown
        If e.Key = Key.Tab Then
            e.Handled = True
            sendBtn_Click(Nothing, Nothing)
        End If
    End Sub

    Dim IsGrouping As Boolean = False
    Private Sub btn1_Click(sender As Object, e As RoutedEventArgs) Handles btn1.Click
        IsGrouping = False
        work_DoWork(Nothing, Nothing)
        work_RunWorkerCompleted(Nothing, Nothing)
    End Sub

    Private Sub btn2_Click(sender As Object, e As RoutedEventArgs) Handles btn2.Click
        IsGrouping = True
        work_DoWork(Nothing, Nothing)
        work_RunWorkerCompleted(Nothing, Nothing)
    End Sub

End Class
