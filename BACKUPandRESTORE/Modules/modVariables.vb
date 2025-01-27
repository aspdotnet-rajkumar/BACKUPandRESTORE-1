﻿Option Explicit On
#Region "Imports"
Imports System
Imports System.Data
Imports System.Data.OleDb

#End Region

Module modVariables

    Public mvarCompanyCode As String
    Public mvarCompanyName As String
    Public mvarUserID As String
    Public mvarFinYearCode As String
    Public mvarFinYearDate As String
    Public mvarLocationCode As String
    Public mvarLocationName As String
    Public mvarModuleName As String
    Public mvarServerDate As String
    Public mvarLanguageCode As String
    Public mvarLanguageDesc As String
    Public SSQL As String
    Public mvarProdLocation As String
    Public mvarProductCode As String
    Public mvarApplicationType As String
    Public mvarsublocationcode As String
    Public mvarIsAdmin As Boolean
    Public mvarFinYearStartDate As Date
    Public mvarFinYearEndDate As Date
    Public con As New OleDbConnection
    Public da As New OleDbDataAdapter
    Public com As New OleDbCommand
    Public ds As New DataSet
    Public ds1 As New DataSet
    Public ds2 As New DataSet
    Public dt As New DataTable
    Public dtPickList As New DataTable
    Public thisConnnection As New OleDbConnection
    Public thisDataAdapter As New OleDbDataAdapter
    Public thisCommand As New OleDbCommand
    Public thisTransaction As OleDbTransaction
    Public mDataRow As DataRow
    Public mRowCount As Double
    Public mvarargument As String
    Public locindex As Integer
    Public mvarmenu As String
    Public CmpPrefix As String
    Public mvarDbasename = "eGeneral"

    Public Declare Unicode Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringW" _
        (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String,
         ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32

    'Public mvarReportPath As String = "\\datasvrb\APPLICATIONS\TexOne\"
    ''   Public mvarReportPath As String = "D:\SpinOne"
    Public mvarReportPath As String = "\\ls-SPIN-DATA\APPLICATIONS\Spinone"

    'Public mvarReportPath As String = Application.StartupPath

End Module

