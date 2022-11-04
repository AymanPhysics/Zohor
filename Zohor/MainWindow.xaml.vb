Imports System.Drawing
Imports System.Data
Imports System.IO
Imports System.Windows.Controls.Primitives
Imports System.Xml

Class MainWindow
    Dim bm As New BasicMethods
    Public Nlvl As Boolean = False
    Dim bol As Boolean = False
    Dim Copy As Boolean = False

    Public WMP As New WMPLib.WindowsMediaPlayer
    Private Sub MainWindow_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        'Application.Current.MainWindow.SizeToContent = Windows.SizeToContent.WidthAndHeight
        'Application.Current.MainWindow.SizeToContent = Windows.SizeToContent.WidthAndHeight
        'Topmost = True
        BringIntoView()
        'Dim s As String = bm.Decrypt("otJ8kXBQS1NqkmfDT1lDNQ==")

        'bm.SetModemMessage("01000986186", "test")

    End Sub

    Private Sub MainWindow_Deactivated(sender As Object, e As EventArgs) Handles Me.Deactivated
        'Topmost = False
    End Sub

    Private Sub MainWindow_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        SaveSetting("OMEGA", "Lang", "LayoutType", layoutSwitcher.SelectedIndex)
        If Nlvl Or bol Then Return
        If Copy = True Then
            bol = True
            Application.Current.Shutdown()
            Exit Sub
        End If
        bm.ClearTemp()
        If bm.ShowDeleteMSG("MsgExit") Then
            bol = True
            Md.FourceExit = True
            Application.Current.Shutdown()
        Else
            e.Cancel = True
            'Me.BringIntoView()
        End If
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try

            LoadResource()

            'dotnet add package ExcelDataReader --version 3.6.0


            If Not Md.MyProjectType = ProjectType.X Then
                MyMenu.Visibility = Visibility.Collapsed
            End If




            Try
                Dim frm As New Window With {.WindowState = WindowState.Minimized, .ShowInTaskbar = False}
                frm.Show()
                frm.Content = WMP
                frm.Hide()
            Catch ex As Exception

            End Try


            Dim Lay As String = GetSetting("OMEGA", "Lang", "LayoutType")
            If Lay = "" Then
                Lay = 1
            End If
            layoutSwitcher.SelectedIndex = Val(Lay)
            SaveSetting("OMEGA", "Lang", "En", GetSetting("OMEGA", "Lang", "En"))
            'layoutSwitcher.Visibility = Visibility.Hidden

            If Md.MyProjectType = ProjectType.X Then

                For i As Integer = 0 To langSwitcher.Items.Count - 1
                    Try
                        Md.MyDictionaries.Items.Add(New ResourceDictionary With {.Source = New Uri(TryCast(TryCast(langSwitcher.Items(i), XmlElement).Attributes("Dic"), XmlAttribute).Value, UriKind.Relative)})
                    Catch ex As Exception
                    End Try
                Next

                langSwitcher_SelectionChanged(Nothing, Nothing)
                langSwitcher.SelectedIndex = 1


                Dim frm As New MyWindow With {.Title = "Check in", .WindowState = WindowState.Maximized}
                frm.Content = New ConferencesCheckInOut With {.Flag = 1}
                frm.Show()

                Dim frm2 As New MyWindow With {.Title = "Check out", .WindowState = WindowState.Maximized}
                frm2.Content = New ConferencesCheckInOut With {.Flag = 2}
                frm2.Show()

                WindowState = WindowState.Minimized
                WindowStyle = WindowStyle.ThreeDBorderWindow
                Return
            End If


            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.ShowGridAccCombo = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.RptFromToday = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.Zohor
                    Md.ShowCostCenter = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.AllowPreviousYearsForNonManager = False
            End Select


            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.ShowCostCenterSub = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.ShowAnalysis = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.Zohor
                    Md.ShowBanks = False
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.Zohor, ProjectType.X
                    Md.ShowBarcode = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.ShowItemSerialNo = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.ShowColorAndSize = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X, ProjectType.X
                    Md.ShowShifts = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X
                    Md.ShowShiftForEveryStore = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X
                    Md.ShowQtySub = False
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X
                    Md.ShowPriceLists = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X
                    Md.ShowCurrency = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.AllowMultiCurrencyPerSubAcc = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.Zohor
                    Md.ShowBankCash_G = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X
                    Md.ShowStoresMotionsEditing = True
                    Md.ShowBankCashMotionsEditing = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.PCs
                    Md.ShowGridAccNames = False
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.ShowBankCash_GAccNo_Not_LinkFile = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.HideSubAccNo = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X
                    Md.ShowSalaries = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.Zohor
                    Md.ShowItemExpireDate = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.X
                    Md.ShowChat = True
            End Select

            If Not LoadConnection() Then Return
            bm = New BasicMethods()
            LoadBarcodePrinter()
            LoadPonePrinter()

            If Not MyProjectType = ProjectType.PCs Then
                If Md.Demo Then
                    bm.TestDemo()
                Else
                    bm.TestProtection()
                End If
            End If

            Dim v As Integer = Val(bm.ExecuteScalar("select LastVersion from LastVersion"))
            If v > Md.LastVersion Or v = 0 Then
                bm.ShowMSG("MsgLastVersion")
                Application.Current.Shutdown()
            End If

            If Md.LastVersion > v Then
                bm.ExecuteNonQuery("delete from LastVersion insert into LastVersion (LastVersion) select " & Md.LastVersion)
            End If

            bm.ClearTemp()

            btnChangeLanguage.Visibility = Visibility.Hidden
            langSwitcher.Visibility = Visibility.Hidden
            Dim L As New Login
            Select Case Md.MyProjectType
                Case ProjectType.Zohor, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X
                    'btnChangeLanguage.Visibility = Visibility.Visible
                    langSwitcher.Visibility = Visibility.Visible
            End Select

            For i As Integer = 0 To langSwitcher.Items.Count - 1
                Try
                    Md.MyDictionaries.Items.Add(New ResourceDictionary With {.Source = New Uri(TryCast(TryCast(langSwitcher.Items(i), XmlElement).Attributes("Dic"), XmlAttribute).Value, UriKind.Relative)})
                Catch ex As Exception
                End Try
            Next

            langSwitcher_SelectionChanged(Nothing, Nothing)
            If Md.MyProjectType = ProjectType.X Then
                langSwitcher.SelectedIndex = 1
            End If

            Select Case Md.MyProjectType
                Case ProjectType.Zohor, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X
                Case ProjectType.X
                    bm.SetImage(L.Img, "buttonscreen2.jpg")
                Case Else
                    bm.SetImage(L.Img, "Login.jpg")
            End Select
            LoadTabs(L)
        Catch ex As Exception
        End Try
    End Sub

    Public Sub LoadTabs(G As Object)
        Try
            MainGrid.Children.Clear()
            MainGrid.Children.Add(New Frame With {.Content = G})
        Catch ex As Exception
        End Try
    End Sub

    Public Sub AddTabOLD(ByVal M As MenuItem, ByVal L As UserControl)
        Dim Tab As New TabItem
        Tab.Header = M.Header
        Tab.Name = "Tab" & M.Name
        Tab.Content = L
        For Each it As TabItem In TabControl1.Items
            If it.Name = Tab.Name Then
                Tab = it
                TabControl1.SelectedItem = Tab
                Return
            End If
        Next
        TabControl1.Items.Add(Tab)
        TabControl1.SelectedItem = Tab
    End Sub

    'Add new tab --> mahmoud
    Public Sub AddTAB(ByVal M As MenuItem, ByVal UserCtrl As UserControl, Optional ByVal HaveClose As Boolean = True)
        Dim TabName As String = M.Name
        Dim TabHeader As String = M.Header
        Dim MW As MainWindow = Application.Current.MainWindow
        Dim TI As TabItem
        For I As Integer = 0 To MW.TabControl1.Items.Count - 1
            TI = MW.TabControl1.Items(I)
            If TI.Name = TabName Then
                TI.Focus()
                Exit Sub
            End If
        Next
        TI = New TabItem
        If HaveClose Then
            TI.Header = New TabsHeader With {.MyTabHeader = TabHeader, .MyTabName = TabName, .WithClose = Visibility.Visible}
        Else
            TI.Header = New TabsHeader With {.MyTabHeader = TabHeader, .MyTabName = TabName, .WithClose = Visibility.Hidden}
        End If
        Try
            CType(TI.Header, TabsHeader).Grid1.Children.Add(M.Icon)
        Catch ex As Exception
        End Try
        TI.Name = TabName
        TI.Content = UserCtrl
        MW.TabControl1.Items.Add(TI)
        TI.Focus()
    End Sub

    Function LoadConnectionOLD() As Boolean
        If con.State = ConnectionState.Open Then Return True
        Dim st As New StreamReader(Md.UdlName & ".udl")
        Dim s As String = ""
        st.ReadLine()
        st.ReadLine()
        s += st.ReadLine
        con.ConnectionString = s.Substring(20)
        Dim cb As New SqlClient.SqlConnectionStringBuilder(con.ConnectionString)
        Dim f As New Form1
        'con.ConnectionString = "Data Source=" & cb.DataSource & ";Initial Catalog=" & cb.InitialCatalog & ";Persist Security Info=True;User ID=" & cb.UserID & ";Password=" & cb.Password 'f.Password 
        con.ConnectionString = cb.ConnectionString
        Try
            con.Open()
        Catch ex As Exception
            bm.ShowMSG("Connection failed")
            bol = True
            Md.FourceExit = True
            Application.Current.Shutdown()
            Return False
        End Try
        Return True
    End Function


    Function LoadConnection() As Boolean
        If con.State = ConnectionState.Open Then Return True

        If Md.UdlName = "" Then
            For Each myfile As String In Directory.GetFiles(System.Windows.Forms.Application.StartupPath)
                If myfile.ToLower.EndsWith(".udldll") Then
                    Md.UdlName = myfile.ToLower.Replace(".udldll", "").Split("\").Last
                End If
            Next
            If Md.UdlName = "" Then
                Dim frm As New EditConnection
                frm.Show()
                frm.Hide()
                frm.AccYear.Text = Md.UdlName
                If Not Md.MyProjectType = ProjectType.X Then
                    frm.ServerName.Text = con.DataSource
                    frm.Database.Text = con.Database
                End If
                frm.ShowDialog()
                If Not frm.Ok Then
                    Application.Current.Shutdown()
                    Return False
                End If
            End If
        End If

        Dim st As New StreamReader(Md.UdlName & ".udldll")
        Dim s As String = st.ReadLine
        st.Close()
        st.Dispose()
        If con.State = ConnectionState.Open Then con.Close()
        con.ConnectionString = bm.Decrypt(s)
        Dim cb As New SqlClient.SqlConnectionStringBuilder(con.ConnectionString)
        'Dim f As New Form1
        'con.ConnectionString = "Data Source=" & cb.DataSource & ";Initial Catalog=" & cb.InitialCatalog & ";Persist Security Info=True;User ID=" & cb.UserID & ";Password=" & cb.Password 'f.Password 
        con.ConnectionString = cb.ConnectionString
        con = New SqlClient.SqlConnection(cb.ConnectionString)

        Try
            con.Open()
            con.Close()
        Catch ex As Exception
            bm.ShowMSG("Connection failed")
            bol = True
            'Md.FourceExit = True
            'Application.Current.Shutdown()

            Dim frm As New EditConnection
            frm.Show()
            frm.Hide()
            frm.AccYear.Text = Md.UdlName
            If Not Md.MyProjectType = ProjectType.X Then
                frm.ServerName.Text = cb.DataSource
                frm.Database.Text = cb.InitialCatalog
            End If
            frm.ShowDialog()

            Return False
        End Try
        Return True
    End Function


    Sub LoadBarcodePrinter()
        Md.BarcodePrinter = GetSetting("OMEGA", "Settings", "BarcodePrinter")
        If Md.BarcodePrinter <> "" Then Return
        Try
            Dim st As New StreamReader("BarcodePrinter.dll")
            Md.BarcodePrinter = st.ReadLine
            st.Close()
        Catch ex As Exception
        End Try
    End Sub

    Sub LoadPonePrinter()
        Md.PonePrinter = GetSetting("OMEGA", "Settings", "PonePrinter")
        If Md.PonePrinter <> "" Then Return
        Try
            Dim st As New StreamReader("PonePrinter.dll")
            Md.PonePrinter = st.ReadLine
            st.Close()
        Catch ex As Exception
        End Try
    End Sub
    Public LogedIn As Boolean = False
    Public Flag As Integer = 1


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnExit.Click
        Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnLogout.Click
        Try
            If Not bm.ShowDeleteMSG("MsgExit") Then Return
            Forms.Application.Restart()
            Application.Current.Shutdown()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnChangeLanguage_Click(sender As Object, e As RoutedEventArgs) Handles btnChangeLanguage.Click
        'If GetSetting("OMEGA", "Lang", "En", True) = False Then
        '    FlowDirection = FlowDirection.LeftToRight
        '    Md.DictionaryCurrent = Md.DictionaryEn
        '    SaveSetting("OMEGA", "Lang", "En", True)
        'Else
        '    FlowDirection = FlowDirection.RightToLeft
        '    Md.DictionaryCurrent = Md.DictionaryAr
        '    SaveSetting("OMEGA", "Lang", "En", False)
        'End If
        Resources = Md.DictionaryCurrent
        Banner1.Resources = Md.DictionaryCurrent
        Banner2.Resources = Md.DictionaryCurrent
        If MainGrid.Children(0).GetType.ToString = "System.Windows.Controls.Frame" Then CType(MainGrid.Children(0), Frame).Refresh()

    End Sub

    Private Sub LoadResource()
        'Md.DictionaryAr.Source = New Uri("Dic_Ar.xaml", UriKind.Relative)
        'Md.DictionaryEn.Source = New Uri("Dic_En.xaml", UriKind.Relative)

        btnChangeLanguage.SetResourceReference(ContentProperty, "Ar-En")
        btnLogout.SetResourceReference(ContentProperty, "Logout")
        btnExit.SetResourceReference(ContentProperty, "ExitApp")
    End Sub


    Public Sub MainWindow_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.Key = Key.Enter Then
                'e.Handled = True
                If FocusManager.GetFocusedElement(Me).GetType = GetType(Button) Then Return
                If FocusManager.GetFocusedElement(Me).GetType = GetType(Forms.Integration.WindowsFormsHost) Then Return
                If FocusManager.GetFocusedElement(Me).GetType = GetType(TextBox) Then
                    If CType(FocusManager.GetFocusedElement(Me), TextBox).VerticalScrollBarVisibility = ScrollBarVisibility.Visible Then Return
                End If
                Dim c As Control = FocusManager.GetFocusedElement(Me)
                InputManager.Current.ProcessInput(New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) With {.RoutedEvent = Keyboard.KeyDownEvent})
                e.Handled = True
                'c.Focus()

                InputManager.Current.ProcessInput(New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter) With {.RoutedEvent = Keyboard.KeyDownEvent})
                'If FocusManager.GetFocusedElement(Me).GetType = GetType(TextBox) AndAlso Not CType(FocusManager.GetFocusedElement(Me), TextBox).VerticalScrollBarVisibility = ScrollBarVisibility.Visible Then CType(FocusManager.GetFocusedElement(Me), TextBox).SelectAll()
            End If
        Catch
        End Try
    End Sub

    Private Sub btnMinimize_Click(sender As Object, e As RoutedEventArgs) Handles btnMinimize.Click
        WindowState = WindowState.Minimized
    End Sub

    Private Sub langSwitcher_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles langSwitcher.SelectionChanged
        Try
            Dim e1 As XmlElement = TryCast(langSwitcher.SelectedItem, XmlElement)
            If e1 IsNot Nothing Then
                Md.DictionaryCurrent = Md.MyDictionaries.Items(langSwitcher.SelectedIndex)
                FlowDirection = TryCast(e1.Attributes("FlowDirection"), XmlAttribute).Value

                Resources = Md.DictionaryCurrent
                Banner1.Resources = Md.DictionaryCurrent
                Banner2.Resources = Md.DictionaryCurrent
                If MainGrid.Children(0).GetType.ToString = "System.Windows.Controls.Frame" Then CType(MainGrid.Children(0), Frame).Refresh()
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub layoutSwitcher_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles layoutSwitcher.SelectionChanged
        If layoutSwitcher.SelectedIndex = 1 Then
            WindowStyle = WindowStyle.ThreeDBorderWindow
        Else
            WindowStyle = WindowStyle.None
            WindowState = WindowState.Minimized
            WindowState = WindowState.Maximized
        End If
    End Sub
End Class
