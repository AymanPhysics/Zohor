﻿Imports System.Data
Imports Microsoft.Office.Interop
Imports System.IO

Public Class RPT23
    Dim bm As New BasicMethods
    Dim dt As New DataTable

    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Val(txtMonth.Text) = 0 OrElse Val(txtYear.Text) = 0 Then Return

        Dim rpt As New ReportViewer
        Select Case Flag
            Case 1
                rpt.Rpt = "KidneysWashMotion2.rpt"
        End Select

        rpt.paraname = New String() {"@Month", "@Year", "@Case2TypeId", "Header"}
        rpt.paravalue = New String() {txtMonth.Text, txtYear.Text, Case2TypeId.SelectedValue.ToString, CType(Parent, Page).Title}
        rpt.Show()

    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.FillCombo("Case2Types", Case2TypeId, "", , True)
        LoadResource()
        Dim MyNow As DateTime = bm.MyGetDate()
        txtMonth.Text = MyNow.Month
        txtYear.Text = MyNow.Year
    End Sub
    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")

        lblFromDate.SetResourceReference(ContentProperty, "Month")
        lblFromDate_Copy.SetResourceReference(ContentProperty, "Year")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles txtMonth.KeyDown, txtYear.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub



End Class