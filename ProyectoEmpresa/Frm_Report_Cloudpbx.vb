﻿Imports System.Data.OleDb
Public Class Frm_Report_Cloudpbx

    Dim ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.Database
    Dim Conexion As New OleDbConnection(ConexionString)

    Private Sub Frm_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Update_Grid()
    End Sub

    Public Sub Update_Grid()

        Dim iSql As String = "select * from brs_cloudpbx_response"
        Dim cmd As New OleDbCommand
        Dim dt As New DataTable
        Dim da As New OleDbDataAdapter

        Try
            Conexion.Open()
            cmd.Connection = Conexion
            cmd.CommandText = iSql
            da.SelectCommand = cmd
            da.Fill(dt)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al acceder a la base de datos para mostrar reporte", MsgBoxStyle.Exclamation, "Error al generar reporte")
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        'Se muestran los datos en el datagridview 
        DataGridView2.DataSource = dt
        DataGridView2.Refresh()

        For j = 0 To DataGridView2.ColumnCount - 1
            DataGridView2.Columns(j).SortMode = DataGridViewColumnSortMode.NotSortable
        Next

    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class