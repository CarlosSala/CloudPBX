Imports System.Data.OleDb
Public Class Frm_Report

    Dim ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
    Dim Conexion As New OleDbConnection(ConexionString)

    Dim gblSession As String = ""
    Dim gblUpdTotalReg As Integer = 0
    Dim gblUpdTotaliXML As Integer = 0
    Dim indiceXML As Integer = 0
    Dim gblSetPathTmp As String
    Dim gblSetPathAppl As String
    Dim gblSetPathLog As String
    Dim gblTimePing As Integer = 2000
    Private Sub Frm_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        actualizarGrilla()
    End Sub


    Public Sub actualizarGrilla()

        Dim iSql As String = "select * from broadsoft_response_error"
        Dim cmd As New OleDbCommand
        Dim dt As New DataTable
        Dim da As New OleDbDataAdapter

        Try
            Conexion.Open()
            cmd.Connection = Conexion
            cmd.CommandText = iSql
            cmd.CommandType = CommandType.TableDirect
            da.SelectCommand = cmd
            da.Fill(dt)
            'Se muestran los datos en el datagridview 
            DataGridView2.DataSource = dt
            DataGridView2.Refresh()
        Catch ex As Exception
            MsgBox("Can not open connection ! , " & ex.Message)
        End Try
        Conexion.Close()

        'DataGridView2.CurrentCell = DataGridView2.Rows(0).Cells(0)
        'lblCMMUpdCurrentRow.Text = DataGridView2.CurrentCell.RowIndex + 1
        'lblCMMUpdTotalRows.Text = DataGridView2.RowCount

        'Lab_wait.Visible = False
        'Me.Cursor = Cursors.Default
        'Interface_Salida()
    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub
End Class