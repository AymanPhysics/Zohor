Imports System.Data
Imports System.Windows.Controls.Primitives

Public Class Login

    Dim bm As New BasicMethods
    Public Flag As Integer = 1
    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnLogin.Click

        If Username.Text.Trim = "" Or Username.Text.Trim = "-" Or Username.SelectedIndex < 0 Then
            Username.Focus()
            Return
        End If

        If Password.Password.Trim = "" Then
            Password.Focus()
            Return
        End If

        Dim dt As DataTable

        If Not bm.StopPro() Then Return
        Dim paraname() As String = {"Id", "Password"}
        Dim paravalue() As String = {Username.SelectedValue.ToString, bm.Encrypt(Password.Password)}
        dt = bm.ExecuteAdapter("TestLogin", paraname, paravalue)
        If dt.Rows.Count = 0 Then
            bm.ShowMSG("Invalid Password ...")
            Password.Focus()
            Password.SelectAll()
            Exit Sub
        End If
        Md.UserName = Username.SelectedValue.ToString
        Md.ArName = dt.Rows(0)("Name").ToString
        Md.EnName = dt.Rows(0)("EnName").ToString
        Md.LevelId = dt.Rows(0)("LevelId").ToString
        Md.Password = bm.Decrypt(dt.Rows(0)("Password").ToString)
        Md.CompanyName = dt.Rows(0)("CompanyName").ToString
        Md.CompanyTel = dt.Rows(0)("CompanyTel").ToString
        Md.Manager = IIf(dt.Rows(0)("Manager").ToString() = "1", True, False)
        Md.Nurse = dt.Rows(0)("Nurse").ToString
        Md.Receptionist = IIf(dt.Rows(0)("Receptionist").ToString() = "1", True, False)
        Md.DefaultStore = dt.Rows(0)("DefaultStore")
        Md.DefaultSave = dt.Rows(0)("DefaultSave")
        Md.DefaultBank = dt.Rows(0)("DefaultBank")
        Md.CostCenterId = dt.Rows(0)("CostCenterId")
        Md.EditPrices = IIf(dt.Rows(0)("EditPrices").ToString() = "1", True, False)
        Md.UserSeeSalesPrice = IIf(dt.Rows(0)("SalesPrice").ToString() = "1", True, False)
        Md.UserSeePurchasesPrice = IIf(dt.Rows(0)("PurchasesPrice").ToString() = "1", True, False)
        Md.UserSeeImportPrice = IIf(dt.Rows(0)("ImportPrice").ToString() = "1", True, False)
        Md.UserCanRptExportButton = IIf(dt.Rows(0)("UserCanRptExportButton").ToString() = "1", True, False)
        Md.UserCanRecieve1 = IIf(dt.Rows(0)("UserCanRecieve1").ToString() = "1", True, False)
        Md.UserCanRecieve2 = IIf(dt.Rows(0)("UserCanRecieve2").ToString() = "1", True, False)

        Select Case Md.MyProjectType
            Case ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X, ProjectType.X
            Case Else
                'bm.TextToSpeech(Md.ArName)
        End Select

        If Md.Manager Then
            'bm.BackupAndSendEMail()
        End If

        Dim m As MainWindow = Application.Current.MainWindow
        m.LoadTabs(New MainPage)
        IsLogedIn = True

        SaveSetting("OMEGA", "Login", "AccYear", AccYear.Text)
        SaveSetting("OMEGA", "Login", "Username", Username.Text)
    End Sub

    Private Sub Login_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()

        If Md.MyProjectType = ProjectType.X OrElse Md.MyProjectType = ProjectType.X Then
            bm.GetImage("Statics", New String() {}, New String() {}, "Logo", CompanyLogo)
        Else
            CompanyLogo.Visibility = Visibility.Hidden
        End If

        Dim dt As New DataTable("ddtt")
        dt.Columns.Add("Id")
        dt.Columns.Add("Name")
        'For Each file In IO.Directory.GetFiles(System.Windows.Forms.Application.StartupPath)
        '    If file.ToLower.EndsWith(".udl") AndAlso Not file.ToLower.EndsWith("connect.udl") Then
        '        dt.Rows.Add(file.ToLower.Split("\").Last.Replace(".udl", ""), file.ToLower.Split("\").Last.Replace(".udl", ""))
        '    End If
        'Next
        For Each file In IO.Directory.GetFiles(System.Windows.Forms.Application.StartupPath)
            If file.ToLower.EndsWith(".udldll") Then
                dt.Rows.Add(file.ToLower.Split("\").Last.Replace(".udldll", ""), file.ToLower.Split("\").Last.Replace(".udldll", ""))
            End If
        Next
        bm.FillCombo(dt, AccYear)

        If dt.Rows.Count <= 1 Then
            lblAccYear.Visibility = Visibility.Hidden
            AccYear.Visibility = Visibility.Hidden
            LoadEmployees()
        Else
            lblAccYear.Visibility = Visibility.Visible
            AccYear.Visibility = Visibility.Visible

            Dim MyAccYear As String = GetSetting("OMEGA", "Login", "AccYear")
            If MyAccYear = "" Then
                AccYear.SelectedIndex = AccYear.Items.Count - 1
            Else
                AccYear.Text = MyAccYear
            End If
        End If


        LoadResource()
    End Sub

    Private Sub LoadResource()
        btnLogin.SetResourceReference(ContentProperty, "Login")
        lblUsername.SetResourceReference(ContentProperty, "Username")
        lblPassword.SetResourceReference(ContentProperty, "Password")
        lblAccYear.SetResourceReference(ContentProperty, "AccYear")
    End Sub

    Private Sub LoadEmployees()
        Dim CboName As String = Resources.Item("CboName")
        If Md.MyProjectType = ProjectType.X Then CboName = "EnName"
        bm.FillCombo("select Id," & CboName & " Name from Employees where SystemUser='1' and Stopped='0' union select 0 Id,'-' Name order by Name", Username)


        Md.CompanyName = bm.ExecuteScalar("select CompanyName from Statics")
        Md.CompanyTel = bm.ExecuteScalar("select CompanyTel from Statics")
        Md.WhatsAppLink = bm.ExecuteScalar("select WhatsAppLink from Statics")


        Username.Text = GetSetting("OMEGA", "Login", "Username")
        If Username.Text.Trim = "" Then
            Username.Focus()
        Else
            Password.Focus()
        End If
    End Sub


    Private Sub AccYear_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles AccYear.SelectionChanged
        Try
            Dim m As MainWindow = Application.Current.MainWindow
            Md.UdlName = AccYear.SelectedValue
            If con.State = ConnectionState.Open Then con.Close()
            If Not m.LoadConnection() Then
                Return
            End If

            'Dim x As Integer = 0
            'While Not m.LoadConnection()
            '    If x > 0 Then
            '        Application.Current.Shutdown()
            '        Return
            '    End If
            '    Dim frm As New EditConnection
            '    frm.Show()
            '    frm.Hide()
            '    frm.AccYear.Text = Md.UdlName
            '    If Not Md.MyProjectType = ProjectType.NawarGroup Then
            '        frm.ServerName.Text = con.DataSource
            '        frm.Database.Text = con.Database
            '    End If
            '    frm.ShowDialog()
            '    x += 1
            'End While
            
            bm = New BasicMethods
            LoadEmployees()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnEditCurrentConnection_Click(sender As Object, e As RoutedEventArgs) Handles btnEditCurrentConnection.Click
        Dim frm As New EditConnection
        frm.Show()
        frm.Hide()
        frm.AccYear.Text = Md.UdlName
        frm.Database.Text = con.Database
        If Not Md.MyProjectType = ProjectType.X Then
            frm.ServerName.Text = con.DataSource
        End If
        frm.ShowDialog()
        Login_Loaded(Nothing, Nothing)
        AccYear.Text = Md.UdlName
        AccYear_SelectionChanged(Nothing, Nothing)
    End Sub

End Class
