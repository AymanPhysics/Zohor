﻿Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports CrystalDecisions.Shared

Public Class ReportViewer

    Public WithEvents ReportDoc As New ReportDocument
    Dim RptPath As String = ""
    Public Header As String = ""
    Public Rpt
    Public paraname() As String = {}
    Public paravalue() As String = {}
    Dim MyPageContentWidth As Integer = 0
    Dim PS As PaperSize

    Public Sub ReportViewer_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If Not ReportDoc Is Nothing Then
                ReportDoc.Close()
                ReportDoc.Dispose()
                Me.CrystalReportViewer1.ReportSource = Nothing
                GC.Collect()
            End If
            ReportDoc.Dispose()

            Dim bm As New BasicMethods
            bm.EmptyTemp()
        Catch
        End Try
    End Sub


    Public Sub ReportViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Md.MyProjectType = ProjectType.X Then CrystalReportViewer1.AllowedExportFormats = 3

            CrystalReportViewer1.ShowExportButton = (Md.Manager OrElse Md.UserCanRptExportButton)

            MinimizeBox = True
            Text = Rpt.Replace(".rpt", "")
            CrystalReportViewer1.ShowRefreshButton = False
            CrystalReportViewer1.ShowLogo = False
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None

            'RptPath = bm.GetNewTempName("dll")
            'IO.File.WriteAllBytes(RptPath, Rpt)
            RptPath = System.Windows.Forms.Application.StartupPath & "\RPTs\" & Rpt

            Dim ServerName As String = con.DataSource
            Dim DataBase As String = con.Database

            ReportDoc.Load(RptPath)

            Dim stb As New SqlClient.SqlConnectionStringBuilder
            stb.ConnectionString = con.ConnectionString
            ReportDoc.SetDatabaseLogon(stb.UserID, stb.Password, ServerName, DataBase)

            Dim Table_LogOn_Info As New TableLogOnInfo()

            Table_LogOn_Info.ConnectionInfo.UserID = stb.UserID
            Table_LogOn_Info.ConnectionInfo.Password = stb.Password

            Table_LogOn_Info.ConnectionInfo.ServerName = ServerName
            Table_LogOn_Info.ConnectionInfo.DatabaseName = DataBase

            Dim TableServer As String
            For Each Table_In_Report As CrystalDecisions.CrystalReports.Engine.Table In ReportDoc.Database.Tables
                Table_In_Report.ApplyLogOnInfo(Table_LogOn_Info)
                TableServer = Table_In_Report.Location.Split(".").Last
                Table_In_Report.Location = DataBase & ".dbo." & TableServer
            Next

            For i As Integer = 0 To ReportDoc.Subreports.Count - 1
                Try
                    For Each Table_In_Report As CrystalDecisions.CrystalReports.Engine.Table In ReportDoc.Subreports(i).Database.Tables
                        Table_In_Report.ApplyLogOnInfo(Table_LogOn_Info)
                        TableServer = Table_In_Report.Location.Split(".").Last
                        Table_In_Report.Location = DataBase & ".dbo." & TableServer
                    Next
                Catch
                End Try
            Next

            SetParamValue("DataBase", DataBase)
            SetParamValue("CompanyName", Md.CompanyName)
            SetParamValue("CompanyTel", Md.CompanyTel)
            SetParamValue("UserName", Md.UserName)
            SetParamValue("@UserName", Md.UserName)
            SetParamValue("ArName", Md.ArName)
            SetParamValue("EnName", Md.EnName)
            SetParamValue("MyProject", Md.MyProjectType.ToString)
            'SetParamValue("Header", IIf(Header = "", CType(CType(CType(Application.Current.Windows(0), MainWindow).TabControl1.SelectedItem, TabItem).Header, TabsHeader).MyTabHeader, Header))

            For i As Integer = 0 To paraname.Length - 1
                SetParamValue(paraname(i), paravalue(i))
            Next

            CrystalReportViewer1.ReportSource = ReportDoc
        Catch ex As Exception
        End Try

        Try

            If MyPageContentWidth = 0 Then
                ComboBox1.Items.Clear()
                Dim MPS As New System.Drawing.Printing.PageSettings
                Dim dt As New DataTable With {.TableName = "tbl"}
                dt.Columns.Add("Kind")
                dt.Columns.Add("RawKind")
                For Each p In MPS.PrinterSettings.PaperSizes
                    dt.Rows.Add({p.Kind, p.RawKind})
                Next
                ComboBox1.DataSource = dt
                ComboBox1.DisplayMember = "Kind"
                ComboBox1.ValueMember = "RawKind"
                ComboBox1.SelectedValue = Val(ReportDoc.PrintOptions.PaperSize.GetTypeCode())

                PS = ReportDoc.PrintOptions.PaperSize

                For i As Integer = 0 To ComboBox1.Items.Count - 1
                    If ComboBox1.Items(i)(0).ToString = PS.ToString.Replace("Paper", "") Then
                        ComboBox1.SelectedIndex = i
                    End If
                Next

                MyPageContentWidth = ReportDoc.PrintOptions.PageContentWidth

            End If
        Catch
        End Try


        'Try
        '    CType(ReportDoc.ReportDefinition.ReportObjects.Item("Logo"), CrystalDecisions.CrystalReports.ViewerObjectModel.FlashObjectInstance).
        '    ReportDoc.ReportDefinition.ReportObjects.Item("Logo").ObjectFormat = (New BasicMethods).ExecuteAdapter("select * from statics")("Logo")
        'Catch ex As Exception
        'End Try
    End Sub

    Public Sub Print(Optional ByVal ServerName As String = "", Optional ByVal PrinterName As String = "", Optional ByVal NoOfCopies As Integer = 1, Optional ClearRPT As Boolean = True)
        ReportViewer_Load(Nothing, Nothing)
        'Show()
        Try
            If PrinterName <> "" Then ReportDoc.PrintOptions.PrinterName = PrinterName '"\\" & ServerName & "\" & PrinterName 
            'For i As Integer = 1 To NoOfCopies
            '    CrystalReportViewer1.ShowLastPage()
            '    ReportDoc.PrintToPrinter(1, False, 1, CrystalReportViewer1.GetCurrentPageNumber)
            'Next

            CrystalReportViewer1.ShowLastPage()
            ReportDoc.PrintToPrinter(NoOfCopies, False, 1, CrystalReportViewer1.GetCurrentPageNumber)

        Catch ex As Exception
            Try
                'ReportDoc.PrintToPrinter(1, False, 1, 1)
                ReportDoc.PrintToPrinter(NoOfCopies, False, 1, 1)
            Catch
                Dim bm As New BasicMethods
                bm.ShowMSG(ex.Message & vbCrLf & "PrinterName: " & PrinterName)
            End Try
        End Try

        If ClearRPT Then ReportViewer_FormClosing(Nothing, Nothing)
    End Sub
    Public Sub SetParamValue(ByVal paramName As String, ByVal paramValue As String)

        For i As Integer = 0 To ReportDoc.DataDefinition.ParameterFields.Count - 1
            If ReportDoc.DataDefinition.ParameterFields(i).ParameterFieldName = paramName Then
                Dim PFD As ParameterFieldDefinition = ReportDoc.DataDefinition.ParameterFields(i)
                Dim PValues As New ParameterValues()
                Dim Parm As New ParameterDiscreteValue()
                Parm.Value = IIf(paramValue Is Nothing, "", paramValue)
                PValues.Add(Parm)
                Try
                    PFD.ApplyCurrentValues(PValues)
                Catch ex As Exception
                End Try
                'Exit For
            End If
        Next

        For i As Integer = 0 To ReportDoc.Subreports.Count - 1
            Try
                For i2 As Integer = 0 To ReportDoc.Subreports(i).DataDefinition.ParameterFields.Count - 1
                    If (ReportDoc.Subreports(i).DataDefinition.ParameterFields(i2).ParameterFieldName.ToLower() = paramName.ToLower()) Then
                        Dim PFD As ParameterFieldDefinition = ReportDoc.Subreports(i).DataDefinition.ParameterFields(i2)
                        Dim PValues As ParameterValues = New ParameterValues()
                        Dim Parm As ParameterDiscreteValue = New ParameterDiscreteValue()
                        Parm.Value = paramValue.Trim()
                        PValues.Add(Parm)
                        PFD.ApplyCurrentValues(PValues)
                        'Exit For
                    End If
                Next
            Catch
            End Try
        Next
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub ReportViewer_KeyDown(sender As Object, e As Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Forms.Keys.Escape Then
            Close()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If MyPageContentWidth = 0 Then Return
        Try
            Dim r As Decimal = MyPageContentWidth / ReportDoc.PrintOptions.PageContentWidth
            ReportDoc.PrintOptions.PaperSize = PS
            For Each c As ReportObject In ReportDoc.ReportDefinition.ReportObjects
                Try
                    c.Left *= r
                    'c.Height *= r

                    If TypeOf (c) Is CrystalDecisions.CrystalReports.Engine.LineObject Then
                        CType(c, CrystalDecisions.CrystalReports.Engine.LineObject).Right *= r
                    ElseIf TypeOf (c) Is CrystalDecisions.CrystalReports.Engine.BoxObject Then
                        'CType(c, CrystalDecisions.CrystalReports.Engine.BoxObject).Right *= r
                        'CType(c, CrystalDecisions.CrystalReports.Engine.BoxObject).Right = CType(c, CrystalDecisions.CrystalReports.Engine.BoxObject).Left + c.Width * r

                        'Dim x As CrystalDecisions.CrystalReports.Engine.BoxObject = c
                        ''x.FillColor = System.Drawing.Color.Red
                        'x.Left = c.Left * r
                        'x.Right = (c.Left + c.Width) * r
                        c.ObjectFormat.EnableSuppress = True
                    Else
                        c.Width *= r
                    End If

                    If TypeOf (c) Is FieldHeadingObject Then
                        Dim cc As FieldHeadingObject = c
                        Dim f As New System.Drawing.Font(cc.Font.FontFamily, cc.Font.Size * r, cc.Font.Style)
                        CType(c, FieldHeadingObject).ApplyFont(f)
                    ElseIf TypeOf (c) Is FieldObject Then
                        Dim cc As FieldObject = c
                        Dim f As New System.Drawing.Font(cc.Font.FontFamily, cc.Font.Size * r, cc.Font.Style)
                        CType(c, FieldObject).ApplyFont(f)
                    ElseIf TypeOf (c) Is textObject Then
                        Dim cc As TextObject = c
                        Dim f As New System.Drawing.Font(cc.Font.FontFamily, cc.Font.Size * r, cc.Font.Style)
                        CType(c, TextObject).ApplyFont(f)
                    End If

                Catch ex As Exception
                    ex = ex
                End Try
            Next

            Dim MPS As New System.Drawing.Printing.PageSettings
            ReportDoc.PrintOptions.PaperSize = MPS.PrinterSettings.PaperSizes(ComboBox1.SelectedIndex).Kind


            r = ReportDoc.PrintOptions.PageContentWidth / MyPageContentWidth
            For Each c As ReportObject In ReportDoc.ReportDefinition.ReportObjects
                Try
                    c.Left *= r
                    'c.Height *= r

                    If TypeOf (c) Is CrystalDecisions.CrystalReports.Engine.LineObject Then
                        CType(c, CrystalDecisions.CrystalReports.Engine.LineObject).Right *= r
                    ElseIf TypeOf (c) Is CrystalDecisions.CrystalReports.Engine.BoxObject Then
                        'CType(c, CrystalDecisions.CrystalReports.Engine.BoxObject).Right *= r
                        'CType(c, CrystalDecisions.CrystalReports.Engine.BoxObject).Right = CType(c, CrystalDecisions.CrystalReports.Engine.BoxObject).Left + c.Width * r

                        'Dim x As CrystalDecisions.CrystalReports.Engine.BoxObject = c
                        ''x.FillColor = System.Drawing.Color.Red
                        'x.Left = c.Left * r
                        'x.Right = (c.Left + c.Width) * r

                        c.ObjectFormat.EnableSuppress = True

                    Else
                        c.Width *= r
                    End If

                    If TypeOf (c) Is FieldHeadingObject Then
                        Dim cc As FieldHeadingObject = c
                        Dim f As New System.Drawing.Font(cc.Font.FontFamily, cc.Font.Size * r, cc.Font.Style)
                        CType(c, FieldHeadingObject).ApplyFont(f)
                    ElseIf TypeOf (c) Is FieldObject Then
                        Dim cc As FieldObject = c
                        Dim f As New System.Drawing.Font(cc.Font.FontFamily, cc.Font.Size * r, cc.Font.Style)
                        CType(c, FieldObject).ApplyFont(f)
                    ElseIf TypeOf (c) Is TextObject Then
                        Dim cc As TextObject = c
                        Dim f As New System.Drawing.Font(cc.Font.FontFamily, cc.Font.Size * r, cc.Font.Style)
                        CType(c, TextObject).ApplyFont(f)
                    End If

                Catch ex As Exception
                    ex = ex
                End Try
            Next
            ReportViewer_Load(Nothing, Nothing)



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


End Class