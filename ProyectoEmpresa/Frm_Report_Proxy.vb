Imports System.Data.OleDb
Public Class Frm_Report_Proxy

    Dim ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
    Dim Conexion As New OleDbConnection(ConexionString)

    Private Sub Frm_Report_Proxy_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Update_Grid()
    End Sub

    Public Sub Update_Grid()

        Dim iSql As String = "select * from brs_proxy_response_error"
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


End Class