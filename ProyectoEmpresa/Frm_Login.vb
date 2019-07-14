Imports System.Data.OleDb ' manejo de BD Access
Public Class Frm_Login

    Dim conexion As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.Database)
    Private Sub Frm_Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            conexion.Open()
            lab_connectionState.Text = "Estas Actualmente conectado a la BD"
            conexion.Close()
        Catch ex As Exception
            MsgBox("La ruta a la BD no se encuentra", vbCritical)
            conexion.Close()
        End Try
    End Sub

    'Buscar registro en la base de datos
    Private Sub Btn_login_Click(sender As Object, e As EventArgs) Handles Btn_login.Click

        If validar() = True Then
            Limpiar()
            Dim FMP As New Frm_Principal
            FMP.Show()
            FMP.BringToFront()
            FMP.WindowState = FormWindowState.Normal
            Me.Close()
        Else
            MsgBox("Los datos no son correctos")
            Limpiar()
        End If
    End Sub

    Sub Limpiar()
        Text_user.Clear()
        Text_password.Clear()
    End Sub

    Public Function validar()
        Dim dt As New DataTable
        Dim ds As New DataSet
        ds.Tables.Add(dt)

        Dim da As New OleDbDataAdapter("SELECT * from Login", conexion)
        da.Fill(dt)

        For Each DataRow In dt.Rows
            If Text_user.Text = DataRow.item(0) And Text_password.Text = DataRow(1) Then
                conexion.Close()
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub Btn_canceled_Click(sender As Object, e As EventArgs) Handles Btn_canceled.Click
        Me.Close()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles lab_connectionState.Click

    End Sub
End Class