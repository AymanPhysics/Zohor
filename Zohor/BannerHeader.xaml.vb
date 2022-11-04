﻿' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Threading

Namespace EmployeeTracker
    ''' <summary>
    ''' Interaction logic for BannerHeader.xaml
    ''' </summary>
    Partial Public Class BannerHeader
        Inherits UserControl

        Dim bm As New BasicMethods
        Dim t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 0, 0, 100)}
        Dim t2 As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 0, 1)}

        Public Header As String = ""
        Public StopTimer As Boolean = False
        Public Sub New()
            InitializeComponent()
            AddHandler t.Tick, AddressOf Tick
            AddHandler t2.Tick, AddressOf Tick2
        End Sub

        Public Sub Tick()
            If StopTimer Then
                t.Stop()
                t2.Stop()
                lblMain.Text = Header
                Return
            End If
            Try
                lblMain.Text = "                " & Md.CompanyName & " " & Md.UdlName.Replace("Connect", "")

                If ShowShifts AndAlso Not ShowShiftForEveryStore AndAlso IsLogedIn Then
                    lblMain.Text &= "   |   " & Md.CurrentDate.Date.ToShortDateString & "  " & Md.CurrentShiftName & " "
                End If
                If Not IsNothing(Md.ArName) Then
                    lblMain.Text &= "   |   " & Md.Currentpage.Trim & " |  " & Resources.Item("Username") & ": " & IIf(Application.Current.MainWindow.FlowDirection = FlowDirection.LeftToRight, Md.EnName, Md.ArName)
                End If
                lblMain.FlowDirection = Application.Current.MainWindow.FlowDirection
                lblMain.HorizontalAlignment = HorizontalAlignment.Left
            Catch ex As Exception
            End Try
        End Sub

        Private Sub Tick2(sender As Object, e As EventArgs)
            bm.GetCurrent()
        End Sub

    End Class
End Namespace
