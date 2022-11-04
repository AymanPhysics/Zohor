﻿Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Drawing

Module Md
    Public LastVersion As Integer = 206
    Public MyProjectType As ProjectType = ProjectType.Zohor

    'Install-Package NavigationPane

    Public AllowPreviousYearsForNonManager = True
    Public ShowItemExpireDate As Boolean = False
    Public StopProfiler As Boolean = False
    Public HideSubAccNo As Boolean = False
    Public ShowSalaries As Boolean = False
    Public ShowCostCenter As Boolean = False
    Public ShowCostCenterSub As Boolean = False
    Public ShowAnalysis As Boolean = False
    Public ShowBanks As Boolean = True
    Public ShowBarcode As Boolean = False
    Public ShowItemSerialNo As Boolean = False
    Public ShowColorAndSize As Boolean = False
    Public ShowGridAccCombo As Boolean = False
    Public ShowShifts As Boolean = False
    Public ShowShiftForEveryStore As Boolean = False
    Public ShowQtySub As Boolean = True
    Public ShowPriceLists As Boolean = False
    Public ShowCurrency As Boolean = False
    Public AllowMultiCurrencyPerSubAcc As Boolean = False
    Public ShowBankCash_G As Boolean = False
    Public ShowStoresMotionsEditing As Boolean = False
    Public ShowBankCashMotionsEditing As Boolean = False
    Public ShowGridAccNames As Boolean = True
    Public ShowBankCash_GAccNo_Not_LinkFile As Boolean = False
    Public EditPrices As Boolean = False
    Public UserSeeSalesPrice As Boolean = False
    Public UserSeePurchasesPrice As Boolean = False
    Public UserSeeImportPrice As Boolean = False
    Public UserCanRptExportButton As Boolean = False
    Public RptFromToday As Boolean = False
    Public UserCanRecieve1 As Boolean = False
    Public UserCanRecieve2 As Boolean = False
    Public ShowChat As Boolean = False
    Public Demo As Boolean = False

    Public con As New SqlConnection
    Public SqlConStrBuilder As New SqlClient.SqlConnectionStringBuilder
    Public FourceExit As Boolean = False
    Public HasLeft As Boolean = False

    Public UserName, ArName, LevelId, Password, CompanyName, CompanyTel, Nurse, WhatsAppLink As String
    Public Manager, Receptionist As Boolean
    Public DefaultStore, DefaultSave, DefaultBank, CostCenterId As Integer
    Public EnName As String = "Please, Login", Currentpage As String = ""

    Public dtLevelsMenuitems As DataTable
    Public CurrentDate As DateTime
    Public CurrentShiftId As Integer = 0
    Public CurrentShiftName As String = ""
    Public Cashier As String = "0"
    Public UdlName As String = "" '"Connect"
    Public IsLogedIn As Boolean = False

    Public BarcodePrinter As String = ""
    Public PonePrinter As String = ""

    Public DictionaryCurrent As New ResourceDictionary()
    Public MyDictionaries As New ListBox

    Enum ProjectType
        PCs
        Zohor
        X
    End Enum

    
End Module
