Imports System.Data.OleDb
Public Class Frm_ListarRegistros


    Dim conexion As New OleDbConnection
    Private Sub Frm_ListarRegistros_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            conexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
            conexion.Open()

            MsgBox("Estas Actualmente conectado a la BD")

        Catch ex As Exception
            MsgBox("La ruta a la BD no se encuentra", vbCritical)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Mostrar()

        Dim oda As New OleDbDataAdapter
        Dim ods As New DataSet
        Dim consulta As String

        consulta = "Select * From Personal"
        oda = New OleDbDataAdapter(consulta, conexion)
        ods.Tables.Add("Personal")
        oda.Fill(ods.Tables("Personal"))
        DataGridView1.DataSource = ods.Tables("Personal")

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Mostrar()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class