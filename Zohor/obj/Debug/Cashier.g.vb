﻿#ExternalChecksum("..\..\Cashier.xaml","{8829d00f-11b8-4213-878b-770e8597ac16}","9775F3B40765937C6C786A6F7ED3CD8F8B9F8C62DD343B1E4AB09B43CA7B80FB")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Forms.Integration
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Shell


'''<summary>
'''Cashier
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class Cashier
    Inherits System.Windows.Controls.UserControl
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\Cashier.xaml",16)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents lblDayDate As System.Windows.Controls.Label
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",17)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents DayDate As System.Windows.Controls.DatePicker
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",21)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SC As System.Windows.Controls.WrapPanel
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",22)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents DG As System.Windows.Controls.DataGrid
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",25)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents StoreName As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",33)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents label3 As System.Windows.Controls.Label
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",34)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents StoreId As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",42)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SaveName As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",50)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents lblSaveId As System.Windows.Controls.Label
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",51)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SaveId As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",59)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents lblSaveId_Copy As System.Windows.Controls.Label
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\Cashier.xaml",60)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents Total As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    Private _contentLoaded As Boolean
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")>  _
    Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        Dim resourceLocater As System.Uri = New System.Uri("/OMEGA;component/cashier.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\Cashier.xaml",1)
        System.Windows.Application.LoadComponent(Me, resourceLocater)
        
        #End ExternalSource
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")>  _
    Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
        If (connectionId = 1) Then
            Me.lblDayDate = CType(target,System.Windows.Controls.Label)
            Return
        End If
        If (connectionId = 2) Then
            Me.DayDate = CType(target,System.Windows.Controls.DatePicker)
            Return
        End If
        If (connectionId = 3) Then
            Me.SC = CType(target,System.Windows.Controls.WrapPanel)
            Return
        End If
        If (connectionId = 4) Then
            Me.DG = CType(target,System.Windows.Controls.DataGrid)
            Return
        End If
        If (connectionId = 5) Then
            Me.StoreName = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 6) Then
            Me.label3 = CType(target,System.Windows.Controls.Label)
            Return
        End If
        If (connectionId = 7) Then
            Me.StoreId = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 8) Then
            Me.SaveName = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 9) Then
            Me.lblSaveId = CType(target,System.Windows.Controls.Label)
            Return
        End If
        If (connectionId = 10) Then
            Me.SaveId = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 11) Then
            Me.lblSaveId_Copy = CType(target,System.Windows.Controls.Label)
            Return
        End If
        If (connectionId = 12) Then
            Me.Total = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class
