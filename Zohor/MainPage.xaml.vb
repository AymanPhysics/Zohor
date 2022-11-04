' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Animation
Imports System.IO
Imports System.Windows.Threading
Imports System.Data
Imports System.Xml
Imports System.IO.Ports
Imports System.Threading

Partial Public Class MainPage
    Inherits Page
    Public NLevel As Boolean = False
    Dim m As MainWindow = Application.Current.MainWindow
    Dim bm As New BasicMethods
    WithEvents t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 1)}

    Private sampleGridOpacityAnimation As DoubleAnimation
    Private sampleGridTranslateTransformAnimation As DoubleAnimation
    Private borderTranslateDoubleAnimation As DoubleAnimation

    Public Sub New()
        InitializeComponent()

        Dim widthBinding As New Binding("ActualWidth")
        widthBinding.Source = Me

        sampleGridOpacityAnimation = New DoubleAnimation()
        sampleGridOpacityAnimation.To = 0
        sampleGridOpacityAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        sampleGridTranslateTransformAnimation = New DoubleAnimation()
        sampleGridTranslateTransformAnimation.BeginTime = TimeSpan.FromSeconds(0.15)
        sampleGridTranslateTransformAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        borderTranslateDoubleAnimation = New DoubleAnimation()
        borderTranslateDoubleAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.3))
        borderTranslateDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0)

    End Sub
    Private Shared _packUri As New Uri("pack://application:,,,/")

    Private Sub btnBack_Click(sender As Object, e As RoutedEventArgs) Handles btnBack.Click
        borderTranslateDoubleAnimation.From = 0
        borderTranslateDoubleAnimation.To = -ActualWidth
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
        GridSampleViewer_Loaded(Nothing, Nothing)
        Md.Currentpage = ""
    End Sub

    Private Sub selectedSampleChanged(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If TypeOf args.Source Is RadioButton Then
            Dim theButton As RadioButton = CType(args.Source, RadioButton)

            Dim theFrame
            If TypeOf theButton.Tag Is MyPage Then
                theFrame = CType(theButton.Tag, MyPage)
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    theFrame.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                If Not Md.MyProjectType = ProjectType.PCs Then
                    CType(theButton.Tag, MyPage).MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowEdit") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowDelete") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowNavigate") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowPrint") = 1
                End If
            ElseIf TypeOf theButton.Tag Is Window Then
                theFrame = CType(theButton.Tag, MyWindow)
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    theFrame.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                If Not Md.MyProjectType = ProjectType.PCs Then
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowEdit") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowDelete") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowNavigate") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowPrint") = 1
                End If
            End If

            theButton.IsTabStop = False
            CType(args.Source, RadioButton).IsChecked = False

            If TypeOf theButton.Tag Is Window Then
                CType(theFrame, Window).Show()
                CType(theFrame, Window).WindowState = WindowState.Minimized
                CType(theFrame, Window).WindowState = WindowState.Maximized
            ElseIf m.layoutSwitcher.SelectedIndex = 1 Then
                Dim frm As New MyWindow
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    frm.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    frm.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                frm.Content = theButton.Tag
                frm.WindowState = WindowState.Maximized
                frm.Show()
                frm.WindowState = WindowState.Minimized
                frm.WindowState = WindowState.Maximized
            Else
                SampleDisplayFrame.Content = theButton.Tag
                SampleDisplayBorder.Visibility = Visibility.Visible
                Try
                    theFrame.Tag = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Catch ex As Exception
                End Try
                sampleDisplayFrameLoaded(theFrame, args)
            End If
        End If

    End Sub

    Private Sub sampleDisplayFrameLoaded(ByVal sender As Object, ByVal args As EventArgs)
        If TypeOf sender Is MyWindow Then
            Try
                If Resources.Item(CType(sender, MyWindow).Tag) = "" Then
                    CType(sender, MyWindow).Title = CType(sender, MyWindow).Tag
                Else
                    CType(sender, MyWindow).Title = Resources.Item(CType(sender, MyWindow).Tag)
                End If
                Md.Currentpage = CType(sender, MyWindow).Title
            Catch ex As Exception
            End Try
        ElseIf TypeOf sender Is Page Then
            Try
                If Resources.Item(CType(sender, Page).Tag) = "" Then
                    CType(sender, Page).Title = CType(sender, Page).Tag
                Else
                    CType(sender, Page).Title = Resources.Item(CType(sender, Page).Tag)
                End If
                Md.Currentpage = CType(sender, Page).Title
            Catch ex As Exception
            End Try
        ElseIf Not sender Is Nothing AndAlso TypeOf CType(sender, Frame).Content Is Page Then
            Try
                If Resources.Item(CType(CType(sender, Frame).Content, Page).Tag) = "" Then
                    CType(CType(sender, Frame).Content, Page).Title = CType(CType(sender, Frame).Content, Page).Tag
                Else
                    CType(CType(sender, Frame).Content, Page).Title = Resources.Item(CType(CType(sender, Frame).Content, Page).Tag)
                End If
                Md.Currentpage = CType(CType(sender, Frame).Content, Page).Title
            Catch ex As Exception
            End Try
            Try
                If Resources.Item(CType(sender, Page).Tag) = "" Then
                    CType(sender, Page).Title = CType(sender, Page).Tag
                Else
                    CType(sender, Page).Title = Resources.Item(CType(sender, Page).Tag)
                End If
                Md.Currentpage = CType(sender, Page).Title
            Catch ex As Exception
            End Try
        End If

        sampleGridTranslateTransformAnimation.To = -ActualWidth
        borderTranslateDoubleAnimation.From = -ActualWidth
        borderTranslateDoubleAnimation.To = 0

        SampleDisplayBorder.Visibility = Visibility.Visible
        SampleGrid.BeginAnimation(Grid.OpacityProperty, sampleGridOpacityAnimation)
        SampleGridTranslateTransform.BeginAnimation(TranslateTransform.XProperty, sampleGridTranslateTransformAnimation)
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
    End Sub

    Private Sub galleryLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If bm.TestIsLoaded(Me, True) Then Return
        tab.Margin = New Thickness(0)
        tab.HorizontalAlignment = HorizontalAlignment.Stretch
        tab.VerticalAlignment = VerticalAlignment.Stretch
        'tab.Style = FindResource("TabControlLeftStyle")
        'tab.Style = FindResource("OutlookTabControlStyle")

        Load()

        SampleDisplayBorderTranslateTransform.X = -ActualWidth
        SampleDisplayBorder.Visibility = Visibility.Hidden
    End Sub

    Private Sub pageSizeChanged(ByVal sender As Object, ByVal args As SizeChangedEventArgs)
        SampleDisplayBorderTranslateTransform.X = Me.ActualWidth
    End Sub

    Dim DynamicMenuitem As Integer = 0
    Dim DtCurrentMenuitem As New DataTable With {.TableName = "T"}
    Sub TestCurrentMenuitem(CurrentMenuitem As Integer)
        If DtCurrentMenuitem.Columns.Count = 0 Then DtCurrentMenuitem.Columns.Add("C")
        If DtCurrentMenuitem.Select("C=" & CurrentMenuitem).Length > 0 AndAlso Not Md.MyProjectType = ProjectType.PCs Then MessageBox.Show(CurrentMenuitem)
        DtCurrentMenuitem.Rows.Add(CurrentMenuitem)
    End Sub
    Sub LoadLabel(CurrentMenuitem As Integer, ByVal G As WrapPanel, Ttl As String)
        TestCurrentMenuitem(CurrentMenuitem)

        For i As Integer = 0 To m.langSwitcher.Items.Count - 1
            Try
                If TryCast(TryCast(m.langSwitcher.Items(i), XmlElement).Attributes("Visibility"), XmlAttribute).Value = "2" Then Continue For
                Dim rd As ResourceDictionary = Md.MyDictionaries.Items(i)
                If rd.Item(Ttl) Is Nothing Then
                    rd.Item(Ttl) = Ttl
                Else
                    While rd.Item(Ttl).Length < 16
                        rd.Item(Ttl) = " " & rd.Item(Ttl) & " "
                    End While
                End If
            Catch ex As Exception
            End Try
        Next

        Dim lbl0 As New Label With {.Height = ActualHeight, .Margin = New Thickness(24, 0, 0, 0)}
        G.Children.Add(lbl0)

        Dim lbl As New Label With {.Name = "menuitem" & CurrentMenuitem, .FontFamily = New System.Windows.Media.FontFamily("khalaad al-arabeh 2"), .FontSize = 30, .HorizontalContentAlignment = HorizontalAlignment.Center, .Foreground = New SolidColorBrush(Color.FromArgb(255, 9, 103, 168)), .FontWeight = FontWeight.FromOpenTypeWeight(1), .Height = 90}
        lbl.SetResourceReference(ContentProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            lbl.Content = Ttl
        End If

        G.Children.Add(lbl)
        'If Md.MyProjectType = ProjectType.NawarGroup OrElse Md.MyProjectType = ProjectType.Hamido Then
        lbl.Width = 240
        lbl.Height = 70
        lbl.FontSize = 24
        'End If

        If Ttl = "" Then lbl.Height = 0


        If Not Lvl Then
            Dim it As New MenuItem With {.Header = "-----------------", .Name = "NewMenuItemSub" & CurrentMenuitem}
            it.Visibility = Visibility.Collapsed
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it)

            Dim it2 As New MenuItem With {.Header = "-----------------", .Name = "NewMenuItemSub" & CurrentMenuitem}
            it2.IsEnabled = False
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it2)
        End If
    End Sub

    Function LoadRadio(CurrentMenuitem As Integer, ByVal G As WrapPanel, ByVal Ttl As String) As RadioButton
        TestCurrentMenuitem(CurrentMenuitem)

        For i As Integer = 0 To m.langSwitcher.Items.Count - 1
            Try
                If TryCast(TryCast(m.langSwitcher.Items(i), XmlElement).Attributes("Visibility"), XmlAttribute).Value = "2" Then Continue For
                Dim rd As ResourceDictionary = Md.MyDictionaries.Items(i)
                If rd.Item(Ttl) Is Nothing Then
                    rd.Item(Ttl) = Ttl
                Else
                    While rd.Item(Ttl).Length < 16
                        rd.Item(Ttl) = " " & rd.Item(Ttl) & " "
                    End While
                End If
            Catch ex As Exception
            End Try
        Next

        Dim RName As String = "menuitem" & CurrentMenuitem
        Dim r As New RadioButton With {.Name = RName, .Style = Application.Current.FindResource("GlassRadioButtonStyle"), .Width = 180, .Height = 90}
        'r.Tag = New Page With {.Content = frm}
        'If Md.MyProjectType = ProjectType.NawarGroup OrElse Md.MyProjectType = ProjectType.Hamido Then
        If Md.MyProjectType <> ProjectType.X Then
            r.Width = 140
            r.Height = 70
        End If

        Dim t As New TranslateTextAnimationExample
        t.RealText.Tag = Ttl
        t.RealText.SetResourceReference(TextBlock.TextProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            t.RealText.Text = Ttl
        End If

        r.SetResourceReference(RadioButton.BackgroundProperty, "SC")
        t.SetResourceReference(RadioButton.BackgroundProperty, "SC")

        r.Content = t
        G.Children.Add(r)

        r.SetResourceReference(RadioButton.ToolTipProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            r.ToolTip = Ttl
        End If


        If Not Lvl Then
            Dim it As New MenuItem With {.Header = Ttl, .Name = "NewMenuItemSub" & CurrentMenuitem}
            it.Tag = r
            it.SetResourceReference(MenuItem.HeaderProperty, Ttl)
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it)
            AddHandler it.Click, AddressOf it_Click
        End If
        Return r
    End Function

    Private Sub it_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim x As RadioButton = CType(sender.Tag, RadioButton)
            x.RaiseEvent(New RoutedEventArgs(RadioButton.CheckedEvent))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GridSampleViewer_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        bm.TestIsLoaded(Me)
    End Sub

    Private Sub ResizeHeader(G As WrapPanel)
        If Lvl Then Return
        Dim Ttl As String = CType(CType(G.Parent, ScrollViewer).Parent, TabItem).Header
        Try
            While Md.DictionaryCurrent.Item(Ttl).Length < 16
                Md.DictionaryCurrent.Item(Ttl) = " " & Md.DictionaryCurrent.Item(Ttl) & " "
            End While
        Catch
        End Try
    End Sub


    Public Lvl As Boolean = False
    Public Sub Load()

        If MyProjectType = ProjectType.PCs Then
            LoadGPCs(0)
            Return
        End If

        LoadTabs()

        If Not Lvl Then
            dtLevelsMenuitems = bm.ExecuteAdapter("select * from LevelsMenuitems where LevelId=" & Md.LevelId)
            Dim dtLevelsTabs As DataTable = bm.ExecuteAdapter("select * from LevelsTabs where LevelId=" & Md.LevelId)
            If dtLevelsMenuitems.Rows.Count = 0 Then Application.Current.Shutdown()

            For i As Integer = 0 To tab.Items.Count - 1
                Dim item As TabItem = CType(tab.Items(i), TabItem)

                If dtLevelsTabs.Select("Id=" & tab.Items(i).Name.ToString.Replace("tab", "")).Length = 0 Then
                    item.Visibility = Visibility.Collapsed
                    CType(m.MyMenu.Items(i), MenuItem).Visibility = Visibility.Collapsed
                End If
                item.Content.Visibility = item.Visibility

                For x As Integer = 0 To CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children.Count - 1
                    If CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(RadioButton) Then
                        Dim t As RadioButton = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), RadioButton)
                        If dtLevelsMenuitems.Select("Id=" & t.Name.ToString.Replace("menuitem", "")).Length = 0 Then
                            t.Visibility = Visibility.Collapsed
                            CType(CType(m.MyMenu.Items(i), MenuItem).Items(x), MenuItem).Visibility = Visibility.Collapsed
                        End If
                    ElseIf CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(Label) Then
                        Dim t As Label = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), Label)
                        If t.Name = "" Then
                            t.Visibility = Visibility.Visible
                        ElseIf dtLevelsMenuitems.Select("Id=" & t.Name.ToString.Replace("menuitem", "")).Length = 0 Then
                            t.Visibility = Visibility.Collapsed
                            CType(CType(m.MyMenu.Items(i), MenuItem).Items(x), MenuItem).Visibility = Visibility.Collapsed
                        End If
                    End If
                Next
            Next

            For i As Integer = 0 To tab.Items.Count - 1
                If CType(tab.Items(i), TabItem).Visibility = Visibility.Visible Then
                    CType(tab.Items(i), TabItem).IsSelected = True
                    Exit For
                End If
            Next

        End If

    End Sub

    Function MakePanel(CurrentTab As Integer, MyHeader As String, ImagePath As String) As WrapPanel
        Dim SV As New MyScrollViewer
        bm.SetImage(SV.Img, ImagePath)
        Dim t As New TabItem With {.Content = SV, .Name = "tab" & CurrentTab, .Header = MyHeader, .Tag = MyHeader}

        'Template.ControlTemplate().Grid().Border().TextBlock()
        'FontFamily="khalaad al-arabeh 2" FontSize="12

        t.Style = FindResource("MyTabItem")
        't.Style = FindResource("OutlookTabItemStyle")
        't.Background = FindResource("OutlookButtonBackground")
        't.Foreground = FindResource("OutlookButtonForeground")

        tab.Items.Add(t)
        Dim G As WrapPanel = SV.MyWrapPanel
        G.Name = "MyWrapPanel" & CurrentTab
        G.AddHandler(System.Windows.Controls.Primitives.ToggleButton.CheckedEvent, New System.Windows.RoutedEventHandler(AddressOf Me.selectedSampleChanged))

        ResizeHeader(G)
        t.SetResourceReference(TabItem.HeaderProperty, t.Header)

        If Not Lvl Then
            Dim it As New MenuItem With {.Header = MyHeader, .MaxWidth = 150, .Name = "NewMenuItem" & CurrentTab}
            it.Tag = t
            it.SetResourceReference(MenuItem.HeaderProperty, MyHeader)
            m.MyMenu.Items.Add(it)
            AddHandler it.MouseEnter, AddressOf itm_Click
        End If

        Return G
    End Function

    Private Sub itm_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim x As TabItem = CType(sender.Tag, TabItem)
            x.Focus()
            x.IsSelected = True
            x.BringIntoView()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadGPCs(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, "File", "Omega.jpg")

        AddHandler LoadRadio(0, G, "PCs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       Dim frm As New BasicForm With {.TableName = "PCs"}
                                                       bm.SetImage(CType(frm, BasicForm).Img, "password.jpg")
                                                       frm.txtName.MaxLength = 1000
                                                       m.TabControl1.Items.Clear()
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub


        AddHandler LoadRadio(0, G, "فتح سنة مالية جديدة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       Dim frm As New CalcSalary With {.Flag = 11}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub



    End Sub

    Private Sub LoadGFile(CurrentTab As Integer)
        Dim s As String = "buttonscreen.jpg"
        Select Case Md.MyProjectType
            Case ProjectType.Zohor, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X
                s = "buttonscreen.jpg"
            Case ProjectType.X
                s = "buttonscreen3.jpg"
            Case ProjectType.X
                s = "buttonscreen3.jpg"
            Case ProjectType.X
                s = "Build2.jpg"
            Case Else
                s = "MainOMEGA.jpg"
        End Select

        Dim G As WrapPanel = MakePanel(CurrentTab, "File", s)
        Dim frm As UserControl

        AddHandler LoadRadio(101, G, "Employees").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               sender.Tag = New MyPage With {.Content = New Employees}
                                                           End Sub

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(102, G, "Drivers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New DriversSellers With {.TableName = "Drivers"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        If Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(103, G, "Countries").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BasicForm With {.TableName = "Countries"}
                                                                   If Md.MyProjectType = ProjectType.X Then bm.SetImage(CType(frm, BasicForm).Img, "CustomerInvoicesItems.Jpg")
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

            AddHandler LoadRadio(104, G, "Cities").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm2 With {.MainTableName = "Countries", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Country", .TableName = "Cities", .MainId = "CountryId", .SubId = "Id", .SubName = "Name"}

                                                                Select Case Md.MyProjectType
                                                                    Case ProjectType.X
                                                                        bm.SetImage(CType(frm, BasicForm2).Img, "CustomerInvoicesItems.Jpg")
                                                                    Case Else
                                                                        bm.SetImage(CType(frm, BasicForm2).Img, "MainOMEGA.jpg")
                                                                End Select
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub


            AddHandler LoadRadio(105, G, "Areas").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm3 With {.MainTableName = "Countries", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Country", .Main2TableName = "Cities", .Main2MainId = "CountryId", .Main2SubId = "Id", .Main2SubName = "Name", .lblMain2_Content = "City", .TableName = "Areas", .MainId = "CountryId", .MainId2 = "CityId", .SubId = "Id", .SubName = "Name"}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        End If

        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(106, G, "Drugs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm With {.TableName = "Drugs"}
                                                               bm.SetImage(CType(frm, BasicForm).Img, "drugs.jpg")
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(107, G, "Doses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm With {.TableName = "Doses"}
                                                               bm.SetImage(CType(frm, BasicForm).Img, "doses.jpg")
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(108, G, "Imaging").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm With {.TableName = "Rays"}
                                                                 bm.SetImage(CType(frm, BasicForm).Img, "ray.jpg")
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

            If Not Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(109, G, "Patient Jobs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New BasicForm With {.TableName = "Jobs"}
                                                                          bm.SetImage(CType(frm, BasicForm).Img, "jobs.jpg")
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
            End If

        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(110, G, "DrugsDoses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New DrugsDoses
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

            AddHandler LoadRadio(111, G, "DiagnoseGroups").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "DiagnoseGroups"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(112, G, "Diagnoses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BasicForm2 With {.TableName = "Diagnoses", .MainTableName = "DiagnoseGroups", .MainId = "GroupId", .lblMain_Content = "GroupId"}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(113, G, "MainJobs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New BasicForm With {.TableName = "MainJobs"}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

            AddHandler LoadRadio(114, G, "SubJobs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm2 With {.MainTableName = "MainJobs", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "MainJob", .TableName = "SubJobs", .MainId = "MainJobId", .SubId = "Id", .SubName = "Name"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        If Not Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(115, G, "Departments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BasicForm With {.TableName = "Departments"}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        End If

        AddHandler LoadRadio(116, G, "Attachment Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm With {.TableName = "AttachmentTypes"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then

            AddHandler LoadRadio(117, G, "Degrees").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New Degrees
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

            If Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(118, G, "Service Groups").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New ServiceGroups
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

                AddHandler LoadRadio(119, G, "Service Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New ServiceTypes
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

                If Md.MyProjectType = ProjectType.Zohor Then
                    AddHandler LoadRadio(120, G, "Visiting Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New BasicForm With {.TableName = "VisitingTypes"}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
                End If
            End If
            If Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(121, G, "Symptoms of Disease").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New BasicForm With {.TableName = "Cbo1"}
                                                                                 bm.SetImage(CType(frm, BasicForm).Img, "symptomsofdiseases.jpg")
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

                AddHandler LoadRadio(122, G, "Disease Diagnoses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New BasicForm With {.TableName = "Cbo2"}
                                                                               bm.SetImage(CType(frm, BasicForm).Img, "diseasesdiagnosis.jpg")
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub


            End If

            If Not Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(123, G, "Laboratory Test Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New BasicForm With {.TableName = "LaboratoryTestTypes", .Flag = 3}
                                                                                   bm.SetImage(CType(frm, BasicForm).Img, "LaboratoryTest.jpg")
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

                'AddHandler LoadRadio(124,G, "Test Type").Checked, Sub(sender As Object, e As RoutedEventArgs)
                '                                                  frm = New BasicForm2 With {.MainTableName = "LaboratoryTestTypes", .MainSubId = "Id", .MainSubName = "Name", .TableName = "LaboratoryTests", .MainId = "TestId", .SubId = "Id", .SubName = "Name", .lblMain_Content = "Test Type"}
                '                                                  bm.SetImage(CType(frm, BasicForm2).Img, "LaboratoryTest.jpg")
                '                                                  sender.Tag = New MyPage With {.Content = frm}
                '                                              End Sub

                AddHandler LoadRadio(125, G, "Test Type").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New LaboratoryTestTypes With {.MainTableName = "LaboratoryTestTypes", .MainSubId = "Id", .MainSubName = "Name", .TableName = "LaboratoryTests", .MainId = "TestId", .SubId = "Id", .SubName = "Name", .lblMain_Content = "Test Type"}
                                                                       bm.SetImage(CType(frm, LaboratoryTestTypes).Img, "LaboratoryTest.jpg")
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
            End If
        End If
        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(126, G, "Laboratory Test Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New LabTestItems
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub
        End If
        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(127, G, "FinishingTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "FinishingTypes"}
                                                                        bm.SetImage(CType(frm, BasicForm).Img, "Build11.jpg")
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Md.ShowShifts Then
            AddHandler LoadRadio(128, G, "Shifts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "Shifts"}
                                                                bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub
        End If

        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(129, G, "Companies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Companies
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        AddHandler LoadRadio(130, G, "Statics").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New Statics
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub


        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X Then

            AddHandler LoadRadio(131, G, "ContactGroups").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New BasicForm With {.TableName = "ContactGroups"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            AddHandler LoadRadio(132, G, "ContactTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm2 With {.MainTableName = "ContactGroups", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Group", .TableName = "ContactTypes", .MainId = "ContactGroupId", .SubId = "Id", .SubName = "Name"}
                                                                      bm.SetImage(CType(frm, BasicForm2).Img, "MainOMEGA.jpg")
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(133, G, "Contacts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New Contacts
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        End If

        If Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(134, G, "CallCenterCategories").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New BasicForm With {.TableName = "CallCenterCategories"}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            AddHandler LoadRadio(135, G, "CallCenter").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New CallCenter
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If


        If Md.MyProjectType = ProjectType.X Then

            LoadLabel(141, G, "Online")

            AddHandler LoadRadio(136, G, "Infertirity").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BasicForm With {.TableName = "Infertirity", .IsMultiLine = True, .WithImage = True}
                                                                     bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

            AddHandler LoadRadio(137, G, "Pregnancy").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BasicForm With {.TableName = "Pregnancy", .IsMultiLine = True, .WithImage = True}
                                                                   bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

            AddHandler LoadRadio(138, G, "Survey").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "Survey", .IsMultiLine = True}
                                                                bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

            AddHandler LoadRadio(139, G, "Welcome Images").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "GalleryMain", .WithImage = True}
                                                                        bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(140, G, "Gallery").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm With {.TableName = "Gallery", .WithImage = True}
                                                                 bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If







        If Md.MyProjectType = ProjectType.X Then
            LoadLabel(148, G, "")

            AddHandler LoadRadio(147, G, "Conferences").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New ConferencesTypes
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

            AddHandler LoadRadio(155, G, "Workshops").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BasicForm2 With {.MainTableName = "Conferences", .lblMain_Content = "Conference", .TableName = "Workshops", .MainId = "ConferenceId"}
                                                                   bm.SetImage(CType(frm, BasicForm2).Img, "MainOMEGA.jpg")
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

            AddHandler LoadRadio(142, G, "Universities").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm With {.TableName = "Universities"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(143, G, "Specialties").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BasicForm With {.TableName = "Specialties"}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

            AddHandler LoadRadio(144, G, "Sub Specialties").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BasicForm2 With {.MainTableName = "Specialties", .lblMain_Content = "Specialty", .TableName = "SubSpecialties", .MainId = "SpecialtyId"}
                                                                         bm.SetImage(CType(frm, BasicForm2).Img, "MainOMEGA.jpg")
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(145, G, "Degrees").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm With {.TableName = "Degrees"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

            AddHandler LoadRadio(146, G, "Titles").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "Titles"}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

            AddHandler LoadRadio(149, G, "Registration Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New BasicForm With {.TableName = "RegistrationTypes"}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

            AddHandler LoadRadio(150, G, "Attendance Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New BasicForm With {.TableName = "AttendanceTypes"}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            AddHandler LoadRadio(151, G, "Sponsors").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New BasicForm With {.TableName = "Sponsors"}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

            AddHandler LoadRadio(152, G, "Hotels").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "Hotels"}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

            AddHandler LoadRadio(153, G, "Room Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm With {.TableName = "RoomTypes"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

            AddHandler LoadRadio(154, G, "Room Upgrade Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New BasicForm With {.TableName = "RoomUpgradeTypes"}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

            LoadLabel(156, G, "")

            AddHandler LoadRadio(157, G, "Registration").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New Conferences
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(158, G, "Check In").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New ConferencesCheckInOut With {.Flag = 1}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

            AddHandler LoadRadio(159, G, "Check Out").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New ConferencesCheckInOut With {.Flag = 2}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

            AddHandler LoadRadio(160, G, "Print Certificate").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New ConferencesCheckInOut With {.Flag = 3}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(161, G, "Print All Certificates").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPTFromIdToId With {.Flag = 1}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub


        End If


        If Md.MyProjectType = ProjectType.X Then

            AddHandler LoadRadio(162, G, "مندوبي التسليم").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "DeliveryPersons"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If
    End Sub

    Private Sub LoadGClinics(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, IIf(Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X, "Department", "Clinics"), "reservation.jpg")
        Dim frm As UserControl

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(201, G, "ExternalDoctors").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New ExternalDoctors
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(202, G, "Patients").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New Cases2
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        ElseIf Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(203, G, "Patients Data").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New Cases4
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        ElseIf Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(204, G, "Patients Data").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New Cases5
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        Else
            AddHandler LoadRadio(205, G, "Patients Data").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New Cases With {.IsSub = IIf(Md.MyProjectType = ProjectType.X, True, False)}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        End If


        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            If Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(206, G, "Clinics Data").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New Clinics
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
            End If


            If Md.MyProjectType <> ProjectType.Zohor Then
                AddHandler LoadRadio(207, G, "OperationsRoomsData").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New Clinics With {.TableName = "OperationsRooms", .Flag = 2}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            End If

            AddHandler LoadRadio(208, G, "InpatientDepartments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New BasicForm With {.TableName = "InpatientDepartments"}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub


            AddHandler LoadRadio(249, G, "InpatientDepartmentsSub").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New BasicForm2 With {.MainTableName = "InpatientDepartments", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "الأقسام", .TableName = "InpatientDepartmentsSub", .MainId = "InpatientDepartmentId", .SubId = "Id", .SubName = "Name"}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub




            AddHandler LoadRadio(209, G, "RoomTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Clinics With {.TableName = "RoomTypes", .Flag = 5}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

            AddHandler LoadRadio(210, G, "Inpatient Data").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New Clinics With {.TableName = "Rooms", .Flag = 3, .ShowInpatientDepartmentId = True}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(211, G, "Visiting Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "VisitingTypes"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(212, G, "Operation Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New OperationTypes
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(213, G, "Visiting Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "VisitingTypes"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X Then
            If Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(214, G, "Services").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New Services
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
            Else
                AddHandler LoadRadio(215, G, "Services").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New Services_G
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

                If Md.MyProjectType = ProjectType.Zohor Then

                    AddHandler LoadRadio(250, G, "ServicesLab").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New Services_G With {.IsServicesLab = True}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

                    AddHandler LoadRadio(251, G, "ServicesIsLabToLab").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New ServicesIsLabToLab
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub
                End If
            End If
        End If

        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(216, G, "Inpatient Services").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New Services_G With {.Flag = 2}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

            AddHandler LoadRadio(217, G, "Operation Motions").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New OperationMotions
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        End If


        LoadLabel(218, G, "")


        Select Case Md.MyProjectType
            Case ProjectType.X
                AddHandler LoadRadio(219, G, "ReservationsClinics").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New ReservationsClinics
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            Case Else
                AddHandler LoadRadio(220, G, "Reservations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New Reservations
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
        End Select

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then 'Md.MyProjectType = ProjectType.Zohor OrElse 

            AddHandler LoadRadio(221, G, "ReservationsOperations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New ReservationsOperations
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.Zohor Then

            AddHandler LoadRadio(248, G, "ReservationsRoomsPre").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New ReservationsRoomsPre
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub


            AddHandler LoadRadio(222, G, "Inpatient").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New ReservationsRooms
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If


        Select Case Md.MyProjectType
            Case ProjectType.X
                AddHandler LoadRadio(223, G, "Clinics List").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New ReservationDetailsClinics
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            Case ProjectType.X
                AddHandler LoadRadio(224, G, "Patients Visiting").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New ReservationDetails2
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub
            Case ProjectType.X
                AddHandler LoadRadio(225, G, "Patients Visiting").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New ReservationDetails3
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub
            Case Else
                AddHandler LoadRadio(225, G, "Patients Visiting").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New ReservationDetails
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub
        End Select

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then 'Md.MyProjectType = ProjectType.Zohor OrElse 

            AddHandler LoadRadio(226, G, "Operations List").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New ReservationDetailsOperations
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(227, G, "Inpatient List").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New ReservationDetailsRooms
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(228, G, "Clinics Show").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New ReservationDetailsClinicsShows
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If

        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(229, G, "LabTests").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New LabTests
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        End If

        If Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(230, G, "OutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New EmpOutcome
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

            AddHandler LoadRadio(231, G, "CloseShift").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New EmpCloseShift
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

            AddHandler LoadRadio(232, G, "EditServices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New Services_G With {.IsEditing = True}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(233, G, "EditReservations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New Cancelation With {.Flag = 3}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        End If

        LoadLabel(234, G, "")

        If Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.Zohor Then

            AddHandler LoadRadio(235, G, "Invoices Cancelation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New Cancelation
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            AddHandler LoadRadio(236, G, "Invoices Refund").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New Cancelation With {.Flag = 2}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            LoadLabel(237, G, "")
        End If


        If Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(238, G, "EmpShifts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New EmpShifts
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(239, G, "Nurse Shift Summary").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New NurseShiftSummary
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(240, G, "Set Doctor Shifts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New PlanSub With {.Alone = True, .CurrentDay = bm.MyGetDate.Date}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(241, G, "Doctor Shifts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT25 With {.Flag = 11}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then 'Md.MyProjectType = ProjectType.Zohor OrElse
            AddHandler LoadRadio(242, G, "Patient Arrival").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT33 With {.Flag = 3}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub
        End If


        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(243, G, "Patient Leaving").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT33 With {.Flag = 4}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub
        End If



        If Md.MyProjectType = ProjectType.X Then 'Md.MyProjectType = ProjectType.Zohor OrElse
            AddHandler LoadRadio(244, G, "CalcClinicsHistory").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New CalcDayDate With {.Flag = 1}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.Zohor Then

            AddHandler LoadRadio(245, G, "CaseInvoices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New CaseInvoices
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub



        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(246, G, "Display").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 Dim w = New Display2
                                                                 w.Title = ""
                                                                 w.WindowStyle = WindowStyle.None
                                                                 w.WindowState = WindowState.Maximized
                                                                 'w.Topmost = True
                                                                 w.ShowDialog()
                                                             End Sub

        End If


        If Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(247, G, "Open New Year").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPTToDate With {.Flag = 1}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        End If

        '251
    End Sub

    Sub LoadGInPatient(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, "InPatient", "reservation.jpg")
        Dim frm As UserControl
        AddHandler LoadRadio(301, G, "Patients Data").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Cases4
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(302, G, "OperationsRoomsData").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New Clinics With {.TableName = "OperationsRooms", .Flag = 2}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        AddHandler LoadRadio(303, G, "InpatientDepartments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New BasicForm With {.TableName = "InpatientDepartments"}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        AddHandler LoadRadio(304, G, "RoomTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Clinics With {.TableName = "RoomTypes", .Flag = 5}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(305, G, "Inpatient Data").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New Clinics With {.TableName = "Rooms", .Flag = 3, .ShowInpatientDepartmentId = True}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(306, G, "OperationDescriptions").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New OperationDescriptions
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        AddHandler LoadRadio(307, G, "Operation Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New OperationTypes2
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub



        AddHandler LoadRadio(308, G, "Inpatient Services").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New Services_G With {.Flag = 2}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(309, G, "Operation Motions").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New OperationMotions
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        LoadLabel(310, G, "")


        AddHandler LoadRadio(311, G, "ReservationsOperations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New ReservationsOperations
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

        AddHandler LoadRadio(312, G, "ReservationsRoomsPre").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New ReservationsRoomsPre
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        AddHandler LoadRadio(313, G, "Inpatient").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New ReservationsRooms
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub




        AddHandler LoadRadio(314, G, "Operations List").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New ReservationDetailsOperations
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(315, G, "Inpatient List").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New ReservationDetailsRooms
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        LoadLabel(316, G, "")

        AddHandler LoadRadio(317, G, "Patient Arrival").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT33 With {.Flag = 3}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(318, G, "Patient Leaving").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT33 With {.Flag = 4}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(319, G, "CalcClinicsHistory").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New CalcDayDate With {.Flag = 1}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(320, G, "CaseInvoices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New CaseInvoices
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub


    End Sub

    Private Sub LoadGKidneysWash(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Kidneys Wash", s)
        Dim frm As UserControl

        AddHandler LoadRadio(401, G, "Patients").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New Cases3
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(402, G, "KidneysWashMotion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New KidneysWashMotion
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

    End Sub

    Private Sub LoadGStores(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Stores", s)
        Dim frm As UserControl

        AddHandler LoadRadio(501, G, "Groups").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Groups
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub


        AddHandler LoadRadio(502, G, "Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           'frm = New BasicForm2 With {.MainTableName = "Groups", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Group", .TableName = "Types", .MainId = "GroupId", .SubId = "Id", .SubName = "Name"}
                                                           frm = New Types
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        If Md.ShowPriceLists OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(503, G, "PriceLists").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm With {.TableName = "PriceLists"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(577, G, "مندوبي التسليم").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "DeliveryPersons"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Not Md.ShowQtySub Then
            If Not Md.MyProjectType = ProjectType.X AndAlso Not Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(563, G, "Itemunits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New BasicForm With {.TableName = "Itemunits"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
            End If
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(563, G, "UnitsWeights").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm1_2 With {.TableName = "UnitsWeights", .lblName2_text = "الوزن", .SubName2 = "Weights"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If


        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(505, G, "Colors").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New ColorsSizes
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

            AddHandler LoadRadio(506, G, "Sizes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New ColorsSizes With {.TableName = "Sizes", .SubTableName = "SizesDetails", .SubTableNameField = "SizeId"}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(507, G, "Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New ItemsClothes
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(508, G, "Print Barcode").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New PrintParcodeGrid
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            AddHandler LoadRadio(509, G, "Occasion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New Occasion
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        Else

            AddHandler LoadRadio(510, G, "Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Items
                                                               If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X Then 'OrElse Md.MyProjectType = ProjectType.NawarGroup 
                                                                   CType(frm, Items).AllowGenerateItemId = True
                                                                   CType(frm, Items).PadLeftCount = 5
                                                               End If
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            If Md.ShowBarcode Then
                AddHandler LoadRadio(511, G, "Print Barcode").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT18 With {.Flag = 1}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

                AddHandler LoadRadio(508, G, "Print Barcode Grid").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New PrintParcodeGrid
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

            End If

        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(570, G, "ItemsUpdate").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New ItemsUpdate
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(512, G, "Statics").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New Statics
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(513, G, "ItemComponants").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New ItemComponants
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(571, G, "Print Barcode").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT18 With {.Flag = 1}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub


            AddHandler LoadRadio(572, G, "Print Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT18 With {.Flag = 2}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        End If

        'If Md.MyProjectType = ProjectType.Zohor Then
        '    AddHandler LoadRadio(803, G, "CostCenters").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                             frm = New CostCenters
        '                                                             sender.Tag = New MyPage With {.Content = frm}
        '                                                         End Sub
        'End If

        AddHandler LoadRadio(514, G, "Stores").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Stores
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        LoadLabel(515, G, "Stores Motion")

        AddHandler LoadRadio(516, G, "Starting balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New Sales With {.Flag = Sales.FlagState.أرصدة_افتتاحية}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        Dim str As String = "Adding"
        If Md.MyProjectType = ProjectType.X Then str = "Donations"
        If Md.MyProjectType = ProjectType.X Then str = "InventoryAdding"
        AddHandler LoadRadio(517, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New Sales With {.Flag = Sales.FlagState.إضافة}
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

        str = "Exchange"
        If Md.MyProjectType = ProjectType.X Then str = "InventoryExchange"
        AddHandler LoadRadio(518, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New Sales With {.Flag = Sales.FlagState.صرف}
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

        If Not Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(519, G, "Gifts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Sales With {.Flag = Sales.FlagState.هدايا}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(520, G, "Loses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Sales With {.Flag = Sales.FlagState.هالك}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub
        End If

        AddHandler LoadRadio(521, G, "Transfer to a Store").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New Sales With {.Flag = Sales.FlagState.تحويل_إلى_مخزن}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(522, G, "Manual Receive").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New Sales With {.Flag = Sales.FlagState.تحويل_إلى_مخزن, .Receive = True}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If


        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(523, G, "Separate and Collect Orders").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New ItemCollectionMotion
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub
        End If

        AddHandler LoadRadio(524, G, "Inventory").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Inventory
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            LoadLabel(525, G, "Imports")

            AddHandler LoadRadio(526, G, "Shippers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New BasicForm With {.TableName = "Shippers"}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

            AddHandler LoadRadio(527, G, "ShippingLines").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New BasicForm With {.TableName = "ShippingLines"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            AddHandler LoadRadio(528, G, "ShippingCompanies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New BasicForm With {.TableName = "ShippingCompanies"}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(529, G, "ContainerSizes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "ContainerSizes"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(530, G, "PaymentMethods").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "PaymentMethods"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(531, G, "ShippingMethods").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BasicForm With {.TableName = "ShippingMethods"}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(532, G, "Imports").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New Sales With {.Flag = Sales.FlagState.الاستيراد}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

            AddHandler LoadRadio(533, G, "Imports Returns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New Sales With {.Flag = Sales.FlagState.مردودات_الاستيراد}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(534, G, "ImportMessages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New ImportMessages
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        LoadLabel(535, G, "Purchases")

        Dim s1 As String = "Purchases"
        Dim s2 As String = "Purchase Returns"
        If Md.MyProjectType = ProjectType.X Then
            s1 = "مشتريات داخلية"
            s2 = "مردود مشتريات داخلية"
        End If

        AddHandler LoadRadio(536, G, s1).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                      frm = New Sales With {.Flag = Sales.FlagState.مشتريات}
                                                      If Md.MyProjectType = ProjectType.X Then
                                                          frm = New Sales2 With {.Flag = Sales.FlagState.مشتريات}
                                                      End If
                                                      sender.Tag = New MyPage With {.Content = frm}
                                                  End Sub

        AddHandler LoadRadio(537, G, s2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                      frm = New Sales With {.Flag = Sales.FlagState.مردودات_مشتريات}
                                                      If Md.MyProjectType = ProjectType.X Then
                                                          frm = New Sales2 With {.Flag = Sales.FlagState.مردودات_مشتريات}
                                                      End If
                                                      sender.Tag = New MyPage With {.Content = frm}
                                                  End Sub

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            Dim f As String = "Outer Purchases"
            Dim f2 As String = "Outer Purchase Returns"
            If Md.MyProjectType = ProjectType.X Then
                f = "Direct Purchases"
                f2 = "Direct Purchases Returns"
                LoadLabel(569, G, f)
            End If
            AddHandler LoadRadio(564, G, f).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                         frm = New Sales With {.Flag = Sales.FlagState.مشتريات_خارجية}
                                                         sender.Tag = New MyPage With {.Content = frm}
                                                     End Sub

            AddHandler LoadRadio(565, G, f2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                          frm = New Sales With {.Flag = Sales.FlagState.مردودات_مشتريات_خارجية}
                                                          sender.Tag = New MyPage With {.Content = frm}
                                                      End Sub

        End If


        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(579, G, "إذن تسليم مورد").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New DeliveryOrder With {.Flag = 1}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        End If


        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(538, G, "Purchase Order").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New Sales With {.Flag = Sales.FlagState.أمر_شراء}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(576, G, "Supplier Quotation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New Sales With {.Flag = Sales.FlagState.عرض_أسعار_مورد}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub
        End If

        LoadLabel(539, G, "Sales")

        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then

            If Md.MyProjectType = ProjectType.Zohor Then
                AddHandler LoadRadio(540, G, "المبيعات نقدي").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New Sales With {.Flag = Sales.FlagState.المستهلكات}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

                AddHandler LoadRadio(541, G, "مردادات المبيعات نقدي").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New Sales With {.Flag = Sales.FlagState.مردودات_المستهلكات}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

            Else

                AddHandler LoadRadio(540, G, "Consumables").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New Sales With {.Flag = Sales.FlagState.المستهلكات}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

                AddHandler LoadRadio(541, G, "Consumables Returns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New Sales With {.Flag = Sales.FlagState.مردودات_المستهلكات}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            End If


            If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(542, G, "Internal Consumables").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New Sales With {.Flag = Sales.FlagState.مستهلكات_الداخلي}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

                AddHandler LoadRadio(543, G, "Internal Consumables Returns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                          frm = New Sales With {.Flag = Sales.FlagState.مردودات_مستهلكات_الداخلي}
                                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                                      End Sub

                AddHandler LoadRadio(544, G, "Operation Consumables").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New Sales With {.Flag = Sales.FlagState.مستهلكات_العمليات}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

                AddHandler LoadRadio(545, G, "Operation Consumables Returns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                           frm = New Sales With {.Flag = Sales.FlagState.مردودات_مستهلكات_العمليات}
                                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                                       End Sub
            End If

            If Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(546, G, "DocNo Calc").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT33 With {.Flag = 5}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
            End If

        ElseIf Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(547, G, "Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New SalesRed With {.Flag = Sales.FlagState.المبيعات}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(548, G, "Sales Returns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New SalesRed With {.Flag = Sales.FlagState.مردودات_المبيعات}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        Else
            AddHandler LoadRadio(549, G, "Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Sales With {.Flag = Sales.FlagState.المبيعات}
                                                               If Md.MyProjectType = ProjectType.X Then
                                                                   frm = New Sales2 With {.Flag = Sales2.FlagState.المبيعات}
                                                               End If
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(550, G, "Sales Returns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New Sales With {.Flag = Sales.FlagState.مردودات_المبيعات}
                                                                       If Md.MyProjectType = ProjectType.X Then
                                                                           frm = New Sales2 With {.Flag = Sales2.FlagState.مردودات_المبيعات}
                                                                       End If
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            Dim f As String = "Exports"
            Dim f2 As String = "Exports Returns"
            If Md.MyProjectType = ProjectType.X Then
                f = "Direct Sales"
                f2 = "Direct Sales Returns"
            End If
            LoadLabel(551, G, f)

            AddHandler LoadRadio(552, G, f).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                         frm = New Sales With {.Flag = Sales.FlagState.التصدير}
                                                         sender.Tag = New MyPage With {.Content = frm}
                                                     End Sub

            AddHandler LoadRadio(553, G, f2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                          frm = New Sales With {.Flag = Sales.FlagState.مردودات_التصدير}
                                                          sender.Tag = New MyPage With {.Content = frm}
                                                      End Sub

        End If

        If Md.MyProjectType = ProjectType.X Then
            LoadLabel(573, G, "مبيعات لمستثمر")

            AddHandler LoadRadio(574, G, "مبيعات لمستثمر").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New Sales With {.Flag = Sales.FlagState.مبيعات_لمستثمر}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(575, G, "مردودات مبيعات لمستثمر").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New Sales With {.Flag = Sales.FlagState.مردودات_مبيعات_لمستثمر}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

        End If

        If 1 = 2 Then
            AddHandler LoadRadio(554, G, "SalesDelivery").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New Sales With {.Flag = Sales.FlagState.مبيعات_التوصيل}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            AddHandler LoadRadio(555, G, "SalesDeliveryReturns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New Sales With {.Flag = Sales.FlagState.مردودات_مبيعات_التوصيل}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(580, G, "أمر توريد").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Sales With {.Flag = Sales.FlagState.أمر_توريد}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(556, G, "Quotation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Sales With {.Flag = Sales.FlagState.عرض_أسعار}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(557, G, "ItemsDelivery").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New ItemsDelivery
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(558, G, "إذن تسليم عميل").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New DeliveryOrder
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub


            AddHandler LoadRadio(578, G, "ItemCollectionMaintenance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New ItemCollectionMaintenance
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

        ElseIf Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(558, G, "إذن تسليم عميل").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New DeliveryOrder_O
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If




        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(559, G, "Wholesales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New Sales With {.Flag = Sales.FlagState.مبيعات_الجملة}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

            AddHandler LoadRadio(560, G, "WholesalesReturns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New Sales With {.Flag = Sales.FlagState.مردودات_مبيعات_الجملة}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(566, G, "Cashier").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New Cashier
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(567, G, "Tenders").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New Tenders
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        '580
    End Sub

    Private Sub LoadGProduction(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Production", s)
        Dim frm As UserControl

        LoadLabel(601, G, "البيانات الأساسية")

        AddHandler LoadRadio(602, G, "Machines").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              'frm = New BasicForm With {.TableName = "Machines"}
                                                              frm = New BasicForm1_3 With {.TableName = "Machines", .CboTableName = "CostCenters", .lblName2_text = "CostCenter", .SubName2 = "CostCenterId"}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(603, G, "MachinesOutcomeTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New BasicForm With {.TableName = "MachinesOutcomeTypes"}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        AddHandler LoadRadio(604, G, "MachinesMotion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New MachinesMotion
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        LoadLabel(605, G, "")


        Dim FlagsDt As DataTable = bm.ExecuteAdapter("select * From ProductionItemCollectionMotionFlags where Flag in(1,5)")
        For i As Integer = 0 To FlagsDt.Rows.Count - 1
            If FlagsDt.Rows(i)("Id") = 1 Then
                'DynamicMenuitem += 1
                LoadLabel(DynamicMenuitem, G, "أوامر التشغيل")
            ElseIf FlagsDt.Rows(i)("Id") = 41 Then
                'DynamicMenuitem += 1
                LoadLabel(DynamicMenuitem, G, "أوامر الانتاج")
            End If
            DynamicMenuitem += 1

            Dim r As RadioButton = LoadRadio(DynamicMenuitem, G, FlagsDt.Rows(i)("Name"))
            frm = New ProductionItemCollectionMotion With {.Flag = FlagsDt.Rows(i)("Id")}
            r.Tag = New MyPage With {.Content = frm}
        Next

        AddHandler LoadRadio(606, G, "ProductionPlan").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New ProductionPlan
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub


        LoadLabel(607, G, "الحسابات")

        AddHandler LoadRadio(608, G, "ProfitRatio").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New ProfitRatio
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        AddHandler LoadRadio(609, G, "مدفوعات المدرسين").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New TeachersPayments
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        'AddHandler LoadRadio(610,G, "ProductionOrderCreation1").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                                 frm = New OrderStatus With {.Flag = 1}
        '                                                                 sender.Tag = New MyPage With {.Content = frm}
        '                                                             End Sub

        'AddHandler LoadRadio(611,G, "ProductionOrderCreation2").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                                 frm = New OrderStatus With {.Flag = 2}
        '                                                                 sender.Tag = New MyPage With {.Content = frm}
        '                                                             End Sub

        'AddHandler LoadRadio(612,G, "ProductionOrderCreation3").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                                 frm = New OrderStatus With {.Flag = 3}
        '                                                                 sender.Tag = New MyPage With {.Content = frm}
        '                                                             End Sub

        'AddHandler LoadRadio(613,G, "ProductionOrderCreation4").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                                 frm = New OrderStatus With {.Flag = 4}
        '                                                                 sender.Tag = New MyPage With {.Content = frm}
        '                                                             End Sub

        AddHandler LoadRadio(614, G, "ProductionOrderCreation3").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New OrderStatus With {.Flag = 1}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        AddHandler LoadRadio(615, G, "ProductionOrderCreation4").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New OrderStatus With {.Flag = 4}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

    End Sub

    Private Sub LoadGSalaries(CurrentTab As Integer)
        Dim s As String = ""
        Select Case Md.MyProjectType
            Case ProjectType.X, ProjectType.X, ProjectType.X
                s = "reservation.jpg"
            Case Else
                s = "MainOMEGA.jpg"
        End Select

        Dim G As WrapPanel = MakePanel(CurrentTab, "Salaries", s)
        Dim frm As UserControl

        AddHandler LoadRadio(701, G, "OfficialHolidays").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New OfficialHolidays
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(702, G, "Import Attendance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New CalcSalary With {.Flag = 4}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(703, G, "Edit Attendance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New EditAttendance
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(704, G, "Loans").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Loans
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        LoadLabel(705, G, "Employees Motion")

        AddHandler LoadRadio(706, G, "DirectBonus").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New DirectBonusCut With {.TableName = "DirectBonus"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        AddHandler LoadRadio(707, G, "DirectCut").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New DirectBonusCut With {.TableName = "DirectCut"}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(708, G, "LeaveRequests").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New LeaveRequests
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(709, G, "LeaveRequests2").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New LeaveRequests2 With {.TableName = "LeaveRequests2"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        LoadLabel(710, G, "Calculation")

        AddHandler LoadRadio(711, G, "Calc Salary").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New CalcSalary With {.Flag = 1}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub


    End Sub

    Private Sub LoadGAccountants(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Accounts", s)
        Dim frm As UserControl

        AddHandler LoadRadio(801, G, "Chart").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Chart
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        If Md.ShowAnalysis Then
            AddHandler LoadRadio(802, G, "Analysis").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New BasicForm With {.TableName = "Analysis"}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        End If

        If Md.ShowCostCenter Then
            AddHandler LoadRadio(803, G, "CostCenters").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New CostCenters
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        End If

        If Md.ShowCostCenterSub Then
            AddHandler LoadRadio(804, G, "CostCenterSubs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New CostCenters With {.TableName = "CostCenterSubs", .MyHeader = "عناصر التكلفة"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Md.ShowCurrency Then
            AddHandler LoadRadio(805, G, "Currencies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm1_2 With {.TableName = "Currencies", .lblName2_text = "الرمز", .SubName2 = "Sign"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(806, G, "CheckBanks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm With {.TableName = "CheckBanks"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

            AddHandler LoadRadio(807, G, "Income Daily Motion Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New BankCash_G2Types With {.Flag = 1}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

            AddHandler LoadRadio(808, G, "Outcome Daily Motion Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New BankCash_G2Types With {.Flag = 2}
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub
        End If



        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(809, G, "Adjustments Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New BankCash_G2Types With {.Flag = 3}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        End If
        AddHandler LoadRadio(810, G, "Entry Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BankCash_G2Types With {.Flag = 4}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub


        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(811, G, "FinalReportsMain").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New BasicForm With {.TableName = "FinalReportsMain"}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            AddHandler LoadRadio(812, G, "FinalReportsSub").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BasicForm2 With {.MainTableName = "FinalReportsMain", .lblMain_Content = "FinalReportsMain", .TableName = "FinalReportsSub", .MainId = "FinalReportsMainId"}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(813, G, "FinalReportsSub2").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New FinalReportsSubDetails
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        End If


        If Md.ShowCurrency Then
            AddHandler LoadRadio(857, G, "CurrencyExchangeByDate").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New CurrencyExchangeByDate
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
        End If

        LoadLabel(814, G, "File")

        AddHandler LoadRadio(815, G, "Assets").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Assets
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        If Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X AndAlso Md.MyProjectType <> ProjectType.X Then
            AddHandler LoadRadio(816, G, "Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Customers
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        AddHandler LoadRadio(817, G, "Suppliers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Suppliers
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(818, G, "Debits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New CreditsDebits With {.TableName = "Debits", .MyLinkFile = 3}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(819, G, "Credits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New CreditsDebits With {.TableName = "Credits", .MyLinkFile = 4}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        AddHandler LoadRadio(820, G, "Saves").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New CreditsDebits With {.TableName = "Saves", .MyLinkFile = 5}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        If Md.ShowBanks Then
            AddHandler LoadRadio(821, G, "Banks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New CreditsDebits With {.TableName = "Banks", .MyLinkFile = 6}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(822, G, "Sellers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New CreditsDebits With {.TableName = "Sellers", .MyLinkFile = 7}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(823, G, "MoneyChangers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New CreditsDebits With {.TableName = "MoneyChangers", .MyLinkFile = 8}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        End If

        AddHandler LoadRadio(824, G, "OutComeTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New CreditsDebits With {.TableName = "OutComeTypes", .MyLinkFile = 9}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(825, G, "InComeTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New CreditsDebits With {.TableName = "InComeTypes", .MyLinkFile = 10}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(826, G, "OrderTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New OrderTypes
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(827, G, "Teachers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New CreditsDebits With {.TableName = "Teachers", .MyLinkFile = 14}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(862, G, "Investors").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New CreditsDebits With {.TableName = "Investors", .MyLinkFile = 15}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        LoadLabel(828, G, "Daily Motion")

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(829, G, "OutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BankCash2 With {.Flag = 2, .MyLinkFile = 5}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(830, G, "Currency Purchase").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New BankCash4 With {.Flag = 1, .MyLinkFile = 5}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(831, G, "Currency Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BankCash4 With {.Flag = 2, .MyLinkFile = 5}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(832, G, "Adjustments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New Entry2
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        End If

        Dim str As String = "Entry"
        If Md.MyProjectType = ProjectType.X Then
            str = "التسويات"
        End If
        AddHandler LoadRadio(833, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New Entry
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

        LoadLabel(834, G, "Income and Outcome")

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(835, G, "Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BankCash_G2 With {.Flag = 1}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

            AddHandler LoadRadio(836, G, "Outcome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BankCash_G2 With {.Flag = 2}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
            If Md.ShowBanks Then
                AddHandler LoadRadio(837, G, "Bank Transfer").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New BankCash_G3
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub
            End If

        ElseIf Md.ShowBankCash_G Then
            AddHandler LoadRadio(838, G, "Safe Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BankCash_G With {.Flag = 1, .MyLinkFile = 5}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

            AddHandler LoadRadio(839, G, "Safe Outcome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BankCash_G With {.Flag = 2, .MyLinkFile = 5}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            If Md.ShowBanks Then
                AddHandler LoadRadio(840, G, "Bank Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BankCash_G With {.Flag = 3, .MyLinkFile = 6}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

                AddHandler LoadRadio(841, G, "Bank Outcome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New BankCash_G With {.Flag = 4, .MyLinkFile = 6}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
            End If
        Else
            AddHandler LoadRadio(842, G, "Safe Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BankCash With {.Flag = 1, .MyLinkFile = 5}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

            AddHandler LoadRadio(843, G, "Safe Outcome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BankCash With {.Flag = 2, .MyLinkFile = 5}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub


            If Md.ShowBanks Then
                AddHandler LoadRadio(844, G, "Bank Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BankCash With {.Flag = 3, .MyLinkFile = 6}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

                AddHandler LoadRadio(845, G, "Bank Outcome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New BankCash With {.Flag = 4, .MyLinkFile = 6}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
            End If

        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(846, G, "Checks Tracing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New ChecksTracingNew
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(847, G, "CasesHistoryAll").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New CasesHistoryAll
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub
        End If


        If Md.MyProjectType = ProjectType.X Then

            LoadLabel(856, G, "Posting")

            AddHandler LoadRadio(848, G, "PostCaseInvoices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT25 With {.Flag = 24, .Tbl = "CaseInvoices", .IsPosted = 1}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            AddHandler LoadRadio(849, G, "UnPostCaseInvoices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT25 With {.Flag = 24, .Tbl = "CaseInvoices", .IsPosted = 0}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

            AddHandler LoadRadio(850, G, "PostSalesMaster").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT25 With {.Flag = 24, .Tbl = "SalesMaster", .IsPosted = 1}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(851, G, "UnPostSalesMaster").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT25 With {.Flag = 24, .Tbl = "SalesMaster", .IsPosted = 0}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(852, G, "PostEntry").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT25 With {.Flag = 24, .Tbl = "Entry", .IsPosted = 1}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

            AddHandler LoadRadio(853, G, "UnPostEntry").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT25 With {.Flag = 24, .Tbl = "Entry", .IsPosted = 0}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

            AddHandler LoadRadio(854, G, "PostBankCash_G").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT25 With {.Flag = 24, .Tbl = "BankCash_G", .IsPosted = 1}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(855, G, "UnPostBankCash_G").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT25 With {.Flag = 24, .Tbl = "BankCash_G", .IsPosted = 0}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            AddHandler LoadRadio(858, G, "PostServices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT25 With {.Flag = 24, .Tbl = "Services", .IsPosted = 1}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(859, G, "UnPostServices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT25 With {.Flag = 24, .Tbl = "Services", .IsPosted = 0}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(860, G, "CloseCases").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT41
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        End If


        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(861, G, "DeficitAndIncrease").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New DeficitAndIncrease
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub
        End If



        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            LoadLabel(10107, G, "Investors")

            AddHandler LoadRadio(10108, G, "InvestorsProfits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New InvestorsProfits
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub


            AddHandler LoadRadio(10110, G, "IncomeFromInvestors").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New BankCash5 With {.Flag = 1, .MyLinkFile = 5}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(10111, G, "OutComeToInvestors").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New BankCash5 With {.Flag = 2, .MyLinkFile = 5}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(10109, G, "InstallmentAlterts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New InstallmentAlerts
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(10110, G, "عمولات البائعين").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New SellersProfits
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(10111, G, "توزيعات مدفوعات الموردين").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New SupliersPayments
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub
        End If


        If Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(10112, G, "استلام الحوافظ").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New CashierZ
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
        End If
    End Sub

    Private Sub LoadGOperations(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, "Operations", "Build1.jpg")
        Dim frm As UserControl

        AddHandler LoadRadio(901, G, "OutComeTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New BasicForm With {.TableName = "OutComeTypes"}
                                                                  bm.SetImage(CType(frm, BasicForm).Img, "CustomerInvoicesItems.Jpg")
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(902, G, "Buildings").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Buildings
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(903, G, "UnitsTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New UnitsTypes
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(904, G, "BuildingsPlan").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BuildingsPlan
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(905, G, "OutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New BankCash2 With {.Flag = 2, .MyLinkFile = 5}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

    End Sub

    Sub LoadDailyMotion(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, "Daily Motion", "buttonscreen2.jpg")
        Dim frm As UserControl

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(100, G, "CustomerCompanies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New CustomerCompanies()
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(1001, G, "Invoices Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BasicForm With {.TableName = "InvoicesTypes"}
                                                                         bm.SetImage(CType(frm, BasicForm).Img, "InvoicesTypes.Jpg")
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(1002, G, "SuppPersons").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm With {.TableName = "SuppPersons"}
                                                                      bm.SetImage(CType(frm, BasicForm).Img, "InvoicesTypes.Jpg")
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1003, G, "InvoicesItems").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "CustomerInvoicesItems"}
                                                                        bm.SetImage(CType(frm, BasicForm).Img, "CustomerInvoicesItems.Jpg")
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(1004, G, "Nolon Prices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New NolonPriceTypes With {.TableName = "NolonPrices", .lblName_text = "UpLoad", .lblName2_text = "DownLoad"}
                                                                       bm.SetImage(CType(frm, NolonPriceTypes).Img, "InvoicesTypes.Jpg")
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            LoadLabel(1005, G, "Daily Motion")

            AddHandler LoadRadio(1006, G, "Invoices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Invoices
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

            AddHandler LoadRadio(1007, G, "Nolon").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New Nolon
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

            AddHandler LoadRadio(1008, G, "NolonRemaining").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New NolonRemaining
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(1009, G, "CustomerInvoices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New CustomerInvoices
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(1010, G, "DailyMotion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New DailyMotion
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        End If
    End Sub


    Sub LoadGInstallments(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, "Installments", "MainOMEGA.jpg")
        Dim frm As UserControl

        LoadLabel(10101, G, "Installment")

        AddHandler LoadRadio(10102, G, "PaymentDays").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BasicForm With {.TableName = "PaymentDays"}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(10103, G, "InstallmentCounts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BasicForm1_2 With {.TableName = "InstallmentCounts", .lblName2_text = "النسبة %", .SubName2 = "Perc"}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        AddHandler LoadRadio(10104, G, "Installment").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Installment With {.Flag = 13}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub


        AddHandler LoadRadio(10105, G, "Investors Installment").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New Installment With {.Flag = 35}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub


        AddHandler LoadRadio(10106, G, "طلب حصول على نظام تقسيط").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New CustomersTemp
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub


    End Sub


    Private Sub LoadGSecurity(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Options", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1101, G, "Change Password").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New ChangePassword
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1102, G, "Levels").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New Levels
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        AddHandler LoadRadio(1103, G, "Attachement").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New Attachments
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        If Md.ShowShifts Then
            AddHandler LoadRadio(1104, G, "Close Shift").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New CalcSalary With {.Flag = 6}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1105, G, "Schedule").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Schedule
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        If Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(1109, G, "Tasks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New Tasks
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub


            'AddHandler LoadRadio(1110, G, "TasksDetails").Checked, Sub(sender As Object, e As RoutedEventArgs)
            '                                                           frm = New TasksDetails
            '                                                           sender.Tag = New MyPage With {.Content = frm}
            '                                                       End Sub


            Dim w As New MyWindow With {.Content = New TasksDetails, .PreventClosing = True}

            w.Show()
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1106, G, "SHUTDOWN").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New CalcSalary With {.Flag = 10}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then


            AddHandler LoadRadio(1107, G, "فتح سنة مالية جديدة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New CalcSalary With {.Flag = 11}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub


            AddHandler LoadRadio(1108, G, "بدء سنة مالية جديدة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New CalcSalary With {.Flag = 12}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        End If


        If Md.ShowChat Then
            'AddHandler LoadRadio(1111, G, "المحادثات").Checked, Sub(sender As Object, e As RoutedEventArgs)
            '                                                        frm = New Chat
            '                                                        sender.Tag = New MyPage With {.Content = frm}
            '                                                    End Sub


            Dim w As New MyWindow With {.Content = New Chat, .PreventClosing = True, .Title = "Chat"}

            w.Show()
        End If

    End Sub

    Private Sub LoadGClinicsReports(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, IIf(Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X, "Department Reports", "Clinics Reports"), "report.jpg")
        Dim frm As UserControl

        If Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(1201, G, "Doctors").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT25 With {.Flag = 15}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

            AddHandler LoadRadio(1247, G, "Doctor Prices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT25 With {.Flag = 27}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(1248, G, "Services Prices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT25 With {.Flag = 28}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(1202, G, "Patients Data").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT35 With {.Flag = 1}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(1203, G, "ActiveCases").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT25 With {.Flag = 19}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1238, G, "ActiveCasesData").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT25 With {.Flag = 23}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
        End If

        AddHandler LoadRadio(1204, G, "Daily Reservations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT1 With {.Flag = 1}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        AddHandler LoadRadio(1205, G, "Daily Reservations Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT1 With {.Flag = 2}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

        If Not Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1206, G, "Services Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT25 With {.Flag = 14}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub


            AddHandler LoadRadio(1207, G, "Services").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT3 With {.Flag = 1}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

            AddHandler LoadRadio(1208, G, "Doctors Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT1 With {.Flag = 3}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(1209, G, "Doctors Income Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT1 With {.Flag = 4}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
        End If

        AddHandler LoadRadio(1210, G, "Daily Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT5 With {.Flag = 1}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1211, G, "Daily Income Summarized").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT5 With {.Flag = 4}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
        End If

        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1212, G, "CompanyDemand").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT5 With {.Flag = 5}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            If Md.MyProjectType = ProjectType.Zohor Then
                AddHandler LoadRadio(1213, G, "CompanyDemandSummarized").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT5 With {.Flag = 5, .Summarized = True}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub

            End If
            AddHandler LoadRadio(1214, G, "CompanyDemandAll").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT5 With {.Flag = 5, .All = True}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub
        End If


        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then 'Md.MyProjectType = ProjectType.Zohor OrElse 
            AddHandler LoadRadio(1215, G, "ReservationsOperations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT37 With {.Flag = 1}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

        End If

        If Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(1244, G, "ReservationsRoomsPreTotal").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New RPTFromDateToDate With {.Flag = 2}
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub

            AddHandler LoadRadio(1249, G, "ReservationsRoomsPreTotalCanceled").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                            frm = New RPTFromDateToDate With {.Flag = 7}
                                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                                        End Sub

            AddHandler LoadRadio(1246, G, "ReservationsRoomsPre").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPTFromDateToDate With {.Flag = 4}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1245, G, "Inpatient").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPTFromDateToDate With {.Flag = 3}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1216, G, "Lab Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT5 With {.Flag = 3}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        End If


        AddHandler LoadRadio(1217, G, "Monthly Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT1 With {.Flag = 5}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        LoadLabel(1218, G, "")

        AddHandler LoadRadio(1219, G, "Patient Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT5 With {.Flag = 2}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1220, G, "Shift Salary").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New CalcSalary With {.Flag = 2, .Hdr = "Shift Salary"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            AddHandler LoadRadio(1221, G, "Doctor Salary").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New CalcSalary With {.Flag = 3, .Hdr = "Doctor Salary"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(1222, G, "Salary Taxes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New CalcSalary With {.Flag = 5}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        End If
        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then

            AddHandler LoadRadio(1223, G, "LabTests").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT17 With {.Flag = 1}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        End If

        LoadLabel(1224, G, "")

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then

            AddHandler LoadRadio(1225, G, "Doctors Shifts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT25 With {.Flag = 7}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub
        End If









        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1226, G, "Nurse Shift Summary").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT25 With {.Flag = 9}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            AddHandler LoadRadio(1227, G, "Statistics Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT29 With {.Flag = 1}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            AddHandler LoadRadio(1228, G, "Statistics Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT29 With {.Flag = 2}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        End If


        If Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(1229, G, "OutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT25 With {.Flag = 12}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

            AddHandler LoadRadio(1230, G, "CloseShift").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT25 With {.Flag = 13}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1231, G, "Statistics Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT1 With {.Flag = 6}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(1234, G, "Statistics Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT1 With {.Flag = 7}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            AddHandler LoadRadio(1235, G, "Statistics Month").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT1 With {.Flag = 8}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(1236, G, "Statistics Department").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT1 With {.Flag = 9}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1237, G, "External Doctor Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT34 With {.Flag = 1}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
        End If


        If Md.MyProjectType = ProjectType.X Then 'Md.MyProjectType = ProjectType.Zohor OrElse 
            AddHandler LoadRadio(1239, G, "Patients Balances2").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT35 With {.Flag = 2}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

            AddHandler LoadRadio(1243, G, "Patients Balances Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT35 With {.Flag = 6}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

            AddHandler LoadRadio(1240, G, "Patients AccountMotion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT35 With {.Flag = 3}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            AddHandler LoadRadio(1241, G, "Bonus Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT35 With {.Flag = 4}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(1242, G, "Bonus Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT35 With {.Flag = 5}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1243, G, "CaseInvoices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT25 With {.Flag = 26}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        End If
        '1238
        '1249
    End Sub

    Sub LoadGInPatientReports(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, "InPatient Reports", "report.jpg")
        Dim frm As UserControl

        AddHandler LoadRadio(1301, G, "Patients Data").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT35 With {.Flag = 1}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(1302, G, "ActiveCases").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT25 With {.Flag = 19}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(1303, G, "ReservationsOperations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT37 With {.Flag = 1}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub


        LoadLabel(1304, G, "")

        AddHandler LoadRadio(1305, G, "Patient Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT5 With {.Flag = 2}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

    End Sub

    Private Sub LoadGKidneysWashReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Kidneys Wash Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1401, G, "Qumision Orders").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT22 With {.Flag = 1}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1402, G, "Insurance Orders").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT23 With {.Flag = 1}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(1403, G, "Qumision Order Renew").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT24 With {.Flag = 1}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        AddHandler LoadRadio(1404, G, "KidneysWashMotion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT33 With {.Flag = 1}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(1405, G, "KidneysWashMotionItems").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT33 With {.Flag = 2}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

    End Sub

    Private Sub LoadGStoresReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Stores Reports", s)
        Dim frm As UserControl

        If Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(1872, G, "Calc Store Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT25 With {.Flag = 8}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
        End If

        AddHandler LoadRadio(1501, G, "Items Printing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT13 With {.Flag = 1}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(1502, G, "Items Printing With Images").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT13 With {.Flag = 2}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

        LoadLabel(1503, G, "Stores Motion")

        AddHandler LoadRadio(1504, G, "Stores Motions Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT6 With {.Flag = 1, .Detail = 1}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        AddHandler LoadRadio(1505, G, "Stores Motions Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT6 With {.Flag = 1, .Detail = 0}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        If Md.ShowStoresMotionsEditing Then
            AddHandler LoadRadio(1506, G, "Stores Motions Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 1, .Detail = 2}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1507, G, "Separate and Collect Orders").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT6 With {.Flag = 11, .Detail = 16}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1508, G, "Separate and Collect Orders Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                                  frm = New RPT6 With {.Flag = 11, .Detail = 17}
                                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                                              End Sub
            End If
        End If

        Dim s1 As String = "Purchase Invoices Detailed"
        Dim s2 As String = "Purchase Invoices Total"
        Dim s3 As String = "Purchase Invoices Editing"
        If Md.MyProjectType = ProjectType.X Then
            s1 = "مشتريات داخلية تفصيلي"
            s2 = "مشتريات داخلية إجمالي"
            s3 = "تعديلات مشتريات داخلية"
        End If


        AddHandler LoadRadio(1509, G, s1).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New RPT6 With {.Flag = 2, .Detail = 1}
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

        AddHandler LoadRadio(1510, G, s2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New RPT6 With {.Flag = 2, .Detail = 0}
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

        If Md.ShowStoresMotionsEditing Then
            AddHandler LoadRadio(1511, G, s3).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 2, .Detail = 2}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub
        End If


        Dim f1 As String = "Outer Purchases Invoices Detailed"
        Dim f2 As String = "Outer Purchases Invoices Total"
        Dim f3 As String = "Outer Purchases Invoices Editing"
        If Md.MyProjectType = ProjectType.X Then
            f1 = "Direct Purchases Detailed"
            f2 = "Direct Purchases Total"
            f3 = "Direct Purchases Editing"
        End If


        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1571, G, f1).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 21, .Detail = 1}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

            AddHandler LoadRadio(1572, G, f2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 21, .Detail = 0}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1573, G, f3).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT6 With {.Flag = 21, .Detail = 2}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub
            End If

        End If


        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1585, G, "Supplier Quotation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 28, .Detail = 1}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub
        End If




        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1512, G, "Imports Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT6 With {.Flag = 7, .Detail = 1}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(1513, G, "Imports Not In Messages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT6 With {.Flag = 10, .Detail = 1}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub


            AddHandler LoadRadio(1514, G, "Imports Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT6 With {.Flag = 7, .Detail = 0}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1515, G, "Imports Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT6 With {.Flag = 7, .Detail = 2}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
            End If

            AddHandler LoadRadio(1516, G, "ImportMessagePacking").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT32 With {.Flag = 3}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1517, G, "ImportMessagePackingImages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT32 With {.Flag = 4}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub

            AddHandler LoadRadio(1596, G, "عقود استيراد تحت التنفيذ").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New RPT6 With {.Flag = 7, .Detail = 22}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

        End If

        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            Dim str As String = "Consumables Detailed"
            If Md.MyProjectType = ProjectType.Zohor Then
                str = "المبيعات نقدي تفصيلي"
            End If

            AddHandler LoadRadio(1518, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New RPT6 With {.Flag = 4, .Detail = 1}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

            str = "Consumables Total"
            If Md.MyProjectType = ProjectType.Zohor Then
                str = "المبيعات نقدي إجمالي"
            End If
            AddHandler LoadRadio(1519, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New RPT6 With {.Flag = 4, .Detail = 0}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

            If Md.ShowStoresMotionsEditing Then
                str = "Consumables Editing"
                If Md.MyProjectType = ProjectType.Zohor Then
                    str = "تعديلات المبيعات نقدي"
                End If
                AddHandler LoadRadio(1520, G, "Consumables Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT6 With {.Flag = 4, .Detail = 2}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
            End If

            If Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1584, G, "Consumables Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 4, .Detail = 20}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            End If
        Else
            AddHandler LoadRadio(1521, G, "Sales Invoices Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT6 With {.Flag = 3, .Detail = 1}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

            AddHandler LoadRadio(1522, G, "Sales Invoices Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT6 With {.Flag = 3, .Detail = 0}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1524, G, "Sales Invoices Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT6 With {.Flag = 3, .Detail = 2}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub
            End If
        End If

        f1 = "Exports Detailed"
        f2 = "Exports Total"
        f3 = "Exports Editing"
        If Md.MyProjectType = ProjectType.X Then
            f1 = "Direct Sales Detailed"
            f2 = "Direct Sales Total"
            f3 = "Direct Sales Editing"
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1525, G, f1).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 8, .Detail = 1}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

            AddHandler LoadRadio(1526, G, f2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 8, .Detail = 0}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1527, G, f3).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT6 With {.Flag = 8, .Detail = 2}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub
            End If

        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1581, G, "Investors Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 11, .Detail = 1}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

            AddHandler LoadRadio(1582, G, "Investors Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT6 With {.Flag = 11, .Detail = 0}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1583, G, "Investors Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT6 With {.Flag = 11, .Detail = 2}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
            End If

        End If

        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
        Else
            AddHandler LoadRadio(1523, G, "Stores Sales Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 3, .Detail = 9}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1528, G, "ItemsDelivery").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT36 With {.Flag = 2}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        End If


        If Md.MyProjectType = ProjectType.X Then

            AddHandler LoadRadio(1593, G, "إذن تسليم مورد").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPTFromDateToDate With {.Flag = 8, .MyFlag = 1}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(1591, G, "إذن تسليم عميل").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPTFromDateToDate With {.Flag = 8}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1595, G, "أمر توريد").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT6 With {.Flag = 31, .Detail = 21}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If


        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1529, G, "Quotation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT6 With {.Flag = 9, .Detail = 1}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1530, G, "ImportMessagesDetailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT32 With {.Flag = 1}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            AddHandler LoadRadio(1531, G, "ImportMessagesTotal").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT32 With {.Flag = 2}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then

                AddHandler LoadRadio(1532, G, "ImportMessageItemMotionCostPrice").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                               frm = New RPT32 With {.Flag = 5}
                                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                                           End Sub

                AddHandler LoadRadio(1533, G, "ImportMessageItemMotionSalesPrice").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                                frm = New RPT32 With {.Flag = 8}
                                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                                            End Sub

                AddHandler LoadRadio(1589, G, "ImportMessageItemMotionQty").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                         frm = New RPT32 With {.Flag = 10}
                                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                                     End Sub

                AddHandler LoadRadio(1597, G, "مبيعات أصناف من رسالة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New RPT32 With {.Flag = 12}
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub

                AddHandler LoadRadio(1534, G, "PrintSalesPone32_N").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT30 With {.Flag = 3}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

                AddHandler LoadRadio(1594, G, "أصناف موردين غير مستلمة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT32 With {.Flag = 9, .PRTFlag = 1}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub

                AddHandler LoadRadio(1535, G, "أصناف عملاء غير مستلمة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT32 With {.Flag = 9, .PRTFlag = 0}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub

            End If



            If Md.MyProjectType = ProjectType.X Then

                AddHandler LoadRadio(1592, G, "أصناف تحت الصيانة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT32 With {.Flag = 11}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

            End If


        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1580, G, "SalesDeliveryDates").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT25 With {.Flag = 25}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        End If


        If Md.MyProjectType = ProjectType.X Then

            LoadLabel(1586, G, "Attachments")

            AddHandler LoadRadio(1587, G, "Purchase Attachments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPTFromDateToDate With {.Flag = 5, .MyFlag = 9}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1588, G, "Sales Attachments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPTFromDateToDate With {.Flag = 5, .MyFlag = 13}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

        End If


        LoadLabel(1536, G, "Items Motion")

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1537, G, "Print Pones").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 3, .Detail = 5}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1538, G, "Sales Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT6 With {.Flag = 3, .Detail = 4}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        Else
            If Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1539, G, "Items Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT13 With {.Flag = 4}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub
            End If

            AddHandler LoadRadio(1540, G, "Sales Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT6 With {.Flag = 30, .Detail = 4}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub


            AddHandler LoadRadio(1541, G, "Items Sales Price Avg").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT6 With {.Flag = 3, .Detail = 10}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

            AddHandler LoadRadio(1542, G, "Item Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT6 With {.Flag = 3, .Detail = 11}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(1543, G, "Most Sales Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT6 With {.Flag = 3, .Detail = 12}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1544, G, "Most Profit Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT6 With {.Flag = 3, .Detail = 13}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

            AddHandler LoadRadio(1545, G, "Items Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 3, .Detail = 3}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1546, G, "Items Sales Summarized").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 3, .Detail = 15}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            If Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1545, G, "Direct Items Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 8, .Detail = 3}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

                AddHandler LoadRadio(1546, G, "Direct Items Sales Summarized").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                            frm = New RPT6 With {.Flag = 8, .Detail = 15}
                                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                                        End Sub

            End If
            AddHandler LoadRadio(1547, G, "Best-Selling Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 3, .Detail = 14}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

            AddHandler LoadRadio(1548, G, "Items Sales All Stores").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 3, .Detail = 6}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            If Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1549, G, "StagnantItems").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT25 With {.Flag = 17}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

                AddHandler LoadRadio(1550, G, "StagnantCustomers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT25 With {.Flag = 18}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
            End If

        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1574, G, "Sales Mobile").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT6 With {.Flag = 3, .Detail = 9}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            AddHandler LoadRadio(1575, G, "Sales Notes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 3, .Detail = 5}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1576, G, "Car Withdrawals").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT6 With {.Flag = 4, .Detail = 3}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        End If
        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1573, G, "*ItemCard*").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT12 With {.Flag = 9}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        End If

        AddHandler LoadRadio(1551, G, "ItemCard").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT12 With {.Flag = 1}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(1552, G, "Item Motion With Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT6 With {.Flag = 5, .Detail = 7}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1553, G, "ItemsCards").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New ItemsCard
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

            AddHandler LoadRadio(1554, G, "Store Bal Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 6, .Detail = 8}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1555, G, "ItemsPackages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT13 With {.Flag = 3}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(1556, G, "Item Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 5, .Detail = 18}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1557, G, "ItemsCardSalesPrice").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT6 With {.Flag = 5, .Detail = 19}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1556, G, "Item Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 5, .Detail = 18}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If

        LoadLabel(1558, G, "Items Balances")

        AddHandler LoadRadio(1559, G, "Items Balance In Store").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT12 With {.Flag = 2}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1560, G, "Items Balance In Store Sizes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                       frm = New RPT12 With {.Flag = 8}
                                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                                   End Sub
        End If

        AddHandler LoadRadio(1561, G, "Store Balance with Purchase Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                        frm = New RPT12 With {.Flag = 5}
                                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                                    End Sub

        AddHandler LoadRadio(1562, G, "Store Balance with Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT12 With {.Flag = 51, .ReportFlag = 1}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        AddHandler LoadRadio(1563, G, "Store Balance with Sales Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT12 With {.Flag = 52, .ReportFlag = 2}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub

        AddHandler LoadRadio(1564, G, "Items Balance In All Stores").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT12 With {.Flag = 3}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

        AddHandler LoadRadio(1565, G, "All Stores Balance with Purchase Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                             frm = New RPT12 With {.Flag = 6}
                                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                                         End Sub

        AddHandler LoadRadio(1566, G, "All Stores Balance with Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New RPT12 With {.Flag = 61, .ReportFlag = 1}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

        AddHandler LoadRadio(1567, G, "All Stores Balance with Sales Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                          frm = New RPT12 With {.Flag = 62, .ReportFlag = 2}
                                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                                      End Sub

        AddHandler LoadRadio(1568, G, "Items exceeded limit demand").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT12 With {.Flag = 4}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

        If Md.MyProjectType = ProjectType.Zohor Then
            AddHandler LoadRadio(1590, G, "Expired Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT12 With {.Flag = 10}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        End If


        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1569, G, "Items exceeded limit demand Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                               frm = New RPT12 With {.Flag = 7}
                                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                                           End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1570, G, "Reservation Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT6 With {.Flag = 5, .Detail = 1}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then

            AddHandler LoadRadio(1577, G, "Target Comparison").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT3 With {.Flag = 1}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

            AddHandler LoadRadio(1578, G, "Daily Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 4, .Detail = 4}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1579, G, "Sales Bonus").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 4, .Detail = 7}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        End If

        '1597
    End Sub

    Sub LoadGProductionReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Production Reports", s)
        Dim frm As UserControl

        LoadLabel(1601, G, "Machines")

        AddHandler LoadRadio(1602, G, "مصروفات الماكينات تفصيلي").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT39 With {.Flag = 1}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

        AddHandler LoadRadio(1603, G, "مصروفات الماكينات إجمالي").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT39 With {.Flag = 2}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

        LoadLabel(1604, G, "حركات أوامر الشغل")

        AddHandler LoadRadio(1605, G, "أوامر الشغل").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT60 With {.MainFlag = 1}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        'AddHandler LoadRadio(1606,G, "أوامر التشغيل - التصوير").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                                frm = New RPT60 With {.MainFlag = 2}
        '                                                                sender.Tag = New MyPage With {.Content = frm}
        '                                                            End Sub

        'AddHandler LoadRadio(1607,G, "أوامر التشغيل - التخريم والتغليف").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                                         frm = New RPT60 With {.MainFlag = 3}
        '                                                                         sender.Tag = New MyPage With {.Content = frm}
        '                                                                     End Sub

        'AddHandler LoadRadio(1608,G, "أوامر التشغيل - المراجعة").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                                 frm = New RPT60 With {.MainFlag = 4}
        '                                                                 sender.Tag = New MyPage With {.Content = frm}
        '                                                             End Sub

        AddHandler LoadRadio(1609, G, "أوامر الانتاج").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT60 With {.MainFlag = 5}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        LoadLabel(1610, G, "الحسابات")
        AddHandler LoadRadio(1611, G, "ProfitRatioTeachers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT01 With {.Flag = 1}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        AddHandler LoadRadio(1612, G, "ProfitRatioAll").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT01 With {.Flag = 2}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(1613, G, "أرصدة المدرسين").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT01 With {.Flag = 3}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(1614, G, "Store Balance with Sales Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT02 With {.Flag = 1}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub

        AddHandler LoadRadio(1615, G, "StoreDailyReport").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT03 With {.Flag = 1}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

    End Sub

    Private Sub LoadGSalaryReports(CurrentTab As Integer)
        Dim s As String = ""
        Select Case Md.MyProjectType
            Case ProjectType.X, ProjectType.X, ProjectType.X
                s = "reservation.jpg"
            Case Else
                s = "MainOMEGA.jpg"
        End Select


        Dim G As WrapPanel = MakePanel(CurrentTab, "Salary Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1701, G, "Employees").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New CalcSalary With {.Flag = 8}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(1702, G, "Salary Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT9 With {.Flag = 1}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1703, G, "Salary Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT9 With {.Flag = 2}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(1704, G, "Attendance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New RPT9 With {.Flag = 3}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        AddHandler LoadRadio(1705, G, "Loans").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New RPT25 With {.Flag = 1}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(1706, G, "Loans Status").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT25 With {.Flag = 6}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        LoadLabel(1707, G, "Employees Motion")

        AddHandler LoadRadio(1708, G, "DirectBonus").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT25 With {.Flag = 2}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(1709, G, "DirectCut").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New RPT25 With {.Flag = 3}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(1710, G, "LeaveRequests").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT25 With {.Flag = 4}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(1711, G, "LeaveRequests2").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT25 With {.Flag = 5}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

    End Sub

    Private Sub LoadGAccountsReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Accounts Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1801, G, "Chart").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New RPT26 With {.Flag = 1}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(1802, G, "Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New RPT38 With {.Flag = 1}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1893, G, "عملاء تحت التحصيل").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT38 With {.Flag = 2}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub
        End If

        Dim str As String = "Account Motion"
        If Md.MyProjectType = ProjectType.X Then
            str = "حساب الأستاذ"
        End If
        AddHandler LoadRadio(1803, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                        frm = New RPT2 With {.Flag = 1, .MyLinkFile = -1}
                                                        sender.Tag = New MyPage With {.Content = frm}
                                                    End Sub

        If Not Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1804, G, "EntryView").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT36 With {.Flag = 1}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

            If Md.ShowBankCash_G Then
                AddHandler LoadRadio(1805, G, "Income View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT21 With {.Flag = 1}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

                If Md.ShowStoresMotionsEditing Then
                    AddHandler LoadRadio(1806, G, "Income Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT21 With {.Flag = 1, .Detailed = 2}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
                End If

                AddHandler LoadRadio(1807, G, "Outcome View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT21 With {.Flag = 2}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

                If Md.ShowStoresMotionsEditing Then
                    AddHandler LoadRadio(1808, G, "Outcome Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT21 With {.Flag = 2, .Detailed = 2}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
                End If

            Else
                AddHandler LoadRadio(1809, G, "Safe Income View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT4 With {.Flag = 1, .MyLinkFile = 5}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

                AddHandler LoadRadio(1810, G, "Safe Outcome View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT4 With {.Flag = 2, .MyLinkFile = 5}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

                AddHandler LoadRadio(1811, G, "Bank Income View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT4 With {.Flag = 3, .MyLinkFile = 6}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

                AddHandler LoadRadio(1812, G, "Bank Outcome View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT4 With {.Flag = 4, .MyLinkFile = 6}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
            End If

            If Md.ShowCostCenter Then
                'AddHandler LoadRadio(1813,G, "CostCenterOutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                '                                                          frm = New RPT14 With {.Flag = 1}
                '                                                          sender.Tag = New MyPage With {.Content = frm}
                '                                                      End Sub

                'AddHandler LoadRadio(1814,G, "CostCenterOutComeToTal").Checked, Sub(sender As Object, e As RoutedEventArgs)
                '                                                               frm = New RPT14 With {.Flag = 2}
                '                                                               sender.Tag = New MyPage With {.Content = frm}
                '                                                           End Sub

                AddHandler LoadRadio(1815, G, "CostCenterAccountMotion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT14 With {.Flag = 3}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If
        End If

        AddHandler LoadRadio(1816, G, "Save Daily Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT2 With {.Flag = 2, .MyLinkFile = 5}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub


        If Md.ShowCurrency Then
            AddHandler LoadRadio(1817, G, "Currency Basket").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT28 With {.Flag = 1}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
        End If


        AddHandler LoadRadio(1818, G, "AllEntries").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New RPT25 With {.Flag = 22}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        If Md.MyProjectType = ProjectType.X Then

            AddHandler LoadRadio(1891, G, "الأقساط").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT45 With {.Flag = 1}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1819, G, "Invoice Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT30 With {.Flag = 1}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

                AddHandler LoadRadio(1820, G, "Invoices Outcome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT30 With {.Flag = 2}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

                AddHandler LoadRadio(1821, G, "Outcome Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT31 With {.Flag = 1}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            End If

            AddHandler LoadRadio(1822, G, "Statement of account").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT2 With {.Flag = 1}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1823, G, "Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT11 With {.Flag = 1}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub


            AddHandler LoadRadio(1824, G, "Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT2 With {.Flag = 3}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        Else

            LoadLabel(1825, G, "Account Motion")

            AddHandler LoadRadio(1826, G, "Asset Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT2 With {.Flag = 1, .MyLinkFile = 12}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1827, G, "Customer Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT2 With {.Flag = 1, .MyLinkFile = 13}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            Else
                AddHandler LoadRadio(1828, G, "Customer Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT2 With {.Flag = 1, .MyLinkFile = 1}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If

            AddHandler LoadRadio(1829, G, "Supplier Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT2 With {.Flag = 1, .MyLinkFile = 2}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

            AddHandler LoadRadio(1830, G, "Debit Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT2 With {.Flag = 1, .MyLinkFile = 3}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1831, G, "Credit Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT2 With {.Flag = 1, .MyLinkFile = 4}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

            AddHandler LoadRadio(1832, G, "Save Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT2 With {.Flag = 1, .MyLinkFile = 5}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            If Md.ShowBanks Then
                AddHandler LoadRadio(1833, G, "Bank Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT2 With {.Flag = 1, .MyLinkFile = 6}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
            End If

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1834, G, "Seller Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New RPT2 With {.Flag = 1, .MyLinkFile = 7}
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub
            End If

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1835, G, "MoneyChanger Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                          frm = New RPT2 With {.Flag = 1, .MyLinkFile = 8}
                                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                                      End Sub
            End If

            AddHandler LoadRadio(1836, G, "OutCome Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT2 With {.Flag = 1, .MyLinkFile = 9}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            If Md.MyProjectType <> ProjectType.X Then
                AddHandler LoadRadio(1837, G, "InCome Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT2 With {.Flag = 1, .MyLinkFile = 10}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub
            End If

            If Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1838, G, "Teacher Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT2 With {.Flag = 1, .MyLinkFile = 14}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub
            End If

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1884, G, "Investor Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT2 With {.Flag = 1, .MyLinkFile = 15}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If

            If Md.ShowSalaries Then
                AddHandler LoadRadio(1887, G, "Employee Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT2 With {.Flag = 1, .MyLinkFile = 16}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If

            LoadLabel(1839, G, "Balances")

            AddHandler LoadRadio(1840, G, "Assets Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT11 With {.Flag = 1, .MyLinkFile = 12}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1841, G, "Customers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT11 With {.Flag = 1, .MyLinkFile = 13}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
            Else
                AddHandler LoadRadio(1842, G, "Customers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT11 With {.Flag = 1, .MyLinkFile = 1}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
            End If

            AddHandler LoadRadio(1843, G, "Suppliers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT11 With {.Flag = 1, .MyLinkFile = 2}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

            AddHandler LoadRadio(1844, G, "Debits Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT11 With {.Flag = 1, .MyLinkFile = 3}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            AddHandler LoadRadio(1845, G, "Credits Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT11 With {.Flag = 1, .MyLinkFile = 4}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(1846, G, "Saves Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT11 With {.Flag = 1, .MyLinkFile = 5}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            If Md.ShowBanks Then
                AddHandler LoadRadio(1847, G, "Banks Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT11 With {.Flag = 1, .MyLinkFile = 6}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub
            End If

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1848, G, "Sellers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT11 With {.Flag = 1, .MyLinkFile = 7}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub
            End If

            If Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1849, G, "MoneyChangers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT11 With {.Flag = 1, .MyLinkFile = 8}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub
            End If

            AddHandler LoadRadio(1850, G, "OutCome Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT11 With {.Flag = 1, .MyLinkFile = 9}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            If Md.MyProjectType <> ProjectType.X Then
                AddHandler LoadRadio(1851, G, "InCome Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT11 With {.Flag = 1, .MyLinkFile = 10}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
            End If

            If Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1852, G, "Teachers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT11 With {.Flag = 1, .MyLinkFile = 14}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
            End If

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1885, G, "Investors Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT11 With {.Flag = 1, .MyLinkFile = 15}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
            End If

            If Md.ShowSalaries Then
                AddHandler LoadRadio(1888, G, "Employees Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT11 With {.Flag = 1, .MyLinkFile = 16}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
            End If

            If Md.MyProjectType = ProjectType.X Then Return

            LoadLabel(1853, G, "Assistant")

            AddHandler LoadRadio(1854, G, "Assets Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT2 With {.Flag = 3, .MyLinkFile = 12}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1855, G, "Customers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT2 With {.Flag = 3, .MyLinkFile = 13}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
            Else
                AddHandler LoadRadio(1856, G, "Customers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT2 With {.Flag = 3, .MyLinkFile = 1}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
            End If

            AddHandler LoadRadio(1857, G, "Suppliers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT2 With {.Flag = 3, .MyLinkFile = 2}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            AddHandler LoadRadio(1858, G, "Debits Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT2 With {.Flag = 3, .MyLinkFile = 3}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(1859, G, "Credits Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT2 With {.Flag = 3, .MyLinkFile = 4}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

            AddHandler LoadRadio(1860, G, "Saves Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT2 With {.Flag = 3, .MyLinkFile = 5}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub


            If Md.ShowBanks Then
                AddHandler LoadRadio(1861, G, "Banks Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT2 With {.Flag = 3, .MyLinkFile = 6}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
            End If


            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1862, G, "Sellers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT2 With {.Flag = 3, .MyLinkFile = 7}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
            End If

            If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
                AddHandler LoadRadio(1863, G, "MoneyChangers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT2 With {.Flag = 3, .MyLinkFile = 8}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If

            AddHandler LoadRadio(1864, G, "OutCome Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT2 With {.Flag = 3, .MyLinkFile = 9}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

            AddHandler LoadRadio(1865, G, "InCome Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT2 With {.Flag = 3, .MyLinkFile = 10}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1866, G, "Teachers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT2 With {.Flag = 3, .MyLinkFile = 14}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1886, G, "Investors Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT2 With {.Flag = 3, .MyLinkFile = 15}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
        End If

        If Md.ShowSalaries Then
            AddHandler LoadRadio(1889, G, "Employees Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT2 With {.Flag = 3, .MyLinkFile = 16}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
        End If

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1867, G, "AdministrativeExpensesDistribution").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                             frm = New RPT32 With {.Flag = 6}
                                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                                         End Sub


            AddHandler LoadRadio(1892, G, "توزيعات مدفوعات الموردين").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New RPT25 With {.Flag = 29}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

        End If

        LoadLabel(1868, G, "Final Accounts")

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1869, G, "CalcAllImportMessages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT25 With {.Flag = 21}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1870, G, "Calc Openned Messages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT25 With {.Flag = 10}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
        End If

        AddHandler LoadRadio(1871, G, "CalcAssetsDepreciation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New CalcSalary With {.Flag = 9}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        AddHandler LoadRadio(1872, G, "Calc Store Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT25 With {.Flag = 8}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1873, G, "Account Balance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT20 With {.Flag = 1}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1883, G, "Account Balance Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New RPT20 With {.Flag = 2}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

        End If

        AddHandler LoadRadio(1874, G, "Operating").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New RPT27 With {.Flag = 1, .MyEndType = 0}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(1875, G, "Trading").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New RPT27 With {.Flag = 1, .MyEndType = 1}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(1876, G, "Gains and losses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT27 With {.Flag = 1, .MyEndType = 2}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(1877, G, "Balance Sheet").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT27 With {.Flag = 1, .MyEndType = 3}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(1878, G, "Income Statement").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT27 With {.Flag = 2, .MyEndType = 2, .IsIncomeStatement = 1}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(1879, G, "Financial Position").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT27 With {.Flag = 3, .MyEndType = 3}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub



        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1880, G, "FinalReports").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT40 With {.Flag = 1, .MainTableName = "FinalReportsMain", .lblMain_Content = "FinalReportsMain", .Main2TableName = "FinalReportsSub", .Main2MainId = "FinalReportsMainId", .lblMain2_Content = "FinalReportsSub"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        End If





        If Md.MyProjectType = ProjectType.X Then
            AddHandler LoadRadio(1881, G, "DeficitAndIncrease").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT7 With {.Flag = 2}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

            AddHandler LoadRadio(1882, G, "DeficitAndIncreaseComparison").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                       frm = New RPT7 With {.Flag = 3}
                                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                                   End Sub


        End If


        AddHandler LoadRadio(1890, G, "Zakat").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Zakat
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        '1893
    End Sub

    Private Sub LoadGOperationsReports(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, "Operations Reports", "Build1.jpg")
        Dim frm As UserControl

        AddHandler LoadRadio(1901, G, "CostCenterOutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT14 With {.Flag = 1}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(1902, G, "CostCenterOutComeToTal").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT14 With {.Flag = 2}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        AddHandler LoadRadio(1903, G, "BuildingIncome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT15 With {.Flag = 1}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

    End Sub

    Private Sub LoadDailyMotionReports(CurrentTab As Integer)
        Dim s As String = "IMG46.Jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Daily Motion Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(2001, G, "CustomerCompanies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT10 With {.Flag = 1}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(2002, G, "DriverCalculations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT8 With {.Flag = 1}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        AddHandler LoadRadio(2003, G, "DailyMotion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT7 With {.Flag = 1}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(2004, G, "CarMovement").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT16 With {.Flag = 1}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

    End Sub


    Private Sub LoadGInstallmentsReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.Jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Installments Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(20101, G, "InstallmentTest").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT42 With {.Flag = 1}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(20102, G, "Installments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT43 With {.Flag = 1}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(20103, G, "تحصيل الأقساط").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPTFromDateToDate With {.Flag = 1}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(20105, G, "أرباح تحصيل الأقساط").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPTFromDateToDate With {.Flag = 6}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        AddHandler LoadRadio(20104, G, "أعذار عميل").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT8 With {.Flag = 2}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        'AddHandler LoadRadio(20105, G, "طلب حصول على نظام تقسيط").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                                       frm = New RPT44 With {.Flag = 1}
        '                                                                       sender.Tag = New MyPage With {.Content = frm}
        '                                                                   End Sub

        '20105
    End Sub

    Private Sub LoadGEventsReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.Jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(20201, G, "Printed IDs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPTFromIdToId With {.Flag = 2}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(20202, G, "Printed Certificates").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPTFromIdToId With {.Flag = 3}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

        AddHandler LoadRadio(20203, G, "Attendance Periods").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPTFromIdToId With {.Flag = 4}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        AddHandler LoadRadio(20204, G, "Attendance Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPTFromIdToId With {.Flag = 5}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        AddHandler LoadRadio(20205, G, "Attendance Per Day").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPTFromIdToId With {.Flag = 6}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        AddHandler LoadRadio(20206, G, "Attended Clients").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPTFromIdToId With {.Flag = 7}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(20207, G, "All Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPTCustomers With {.Flag = 1}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub


    End Sub

    Private Sub LoadAbout(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "About", s)
        Dim wb As New WebBrowser With {.Margin = New Thickness(0)}
        wb.Navigate("http://omegaapp.blogspot.com.eg/")
        G.Children.Add(wb)
        wb.Width = tab.ActualWidth - 20
        wb.Height = tab.ActualHeight - 60
    End Sub



    Private Sub LoadTabs()
        LoadGFile(1)
        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            LoadGClinics(2)
        End If

        'If Md.MyProjectType = ProjectType.Zohor Then
        '    LoadGInPatient(3)
        'End If

        If Md.MyProjectType = ProjectType.X Then
            LoadGKidneysWash(4)
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X Then
            LoadGStores(5)
        End If

        If Md.MyProjectType = ProjectType.X Then
            LoadGProduction(6)
        End If

        If Md.ShowSalaries Then
            LoadGSalaries(7)
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.Zohor Then
            LoadGAccountants(8)
        End If

        If Md.MyProjectType = ProjectType.X Then
            LoadGOperations(9)
        End If

        If Md.MyProjectType = ProjectType.X Then
            LoadDailyMotion(10)
        End If

        If Md.MyProjectType = ProjectType.X Then
            LoadGInstallments(101)
        End If

        LoadGSecurity(11)

        If Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            LoadGClinicsReports(12)
        End If

        'If Md.MyProjectType = ProjectType.Zohor Then
        '    LoadGInPatientReports(13)
        'End If

        If Md.MyProjectType = ProjectType.X Then
            LoadGKidneysWashReports(14)
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.Zohor OrElse Md.MyProjectType = ProjectType.X Then
            LoadGStoresReports(15)
        End If

        If Md.MyProjectType = ProjectType.X Then
            LoadGProductionReports(16)
        End If

        If Md.ShowSalaries Then
            LoadGSalaryReports(17)
        End If

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.Zohor Then
            LoadGAccountsReports(18)
        End If

        If Md.MyProjectType = ProjectType.X Then
            LoadGOperationsReports(19)
        End If

        If Md.MyProjectType = ProjectType.X Then
            LoadDailyMotionReports(20)
        End If

        If Md.MyProjectType = ProjectType.X Then
            LoadGInstallmentsReports(201)
        End If

        If Md.MyProjectType = ProjectType.X Then
            LoadGEventsReports(202)
        End If


        'LoadAbout(21)

        'bm.SetModem()



    End Sub



End Class

