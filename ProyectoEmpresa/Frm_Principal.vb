﻿Imports System.Data.OleDb 'manejo de BD Access
Public Class Frm_Principal

    Dim ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
    Dim Conexion As New OleDbConnection(ConexionString)

    Private Sub For1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Interface_Entrada()
    End Sub

    Function GuardarDatosEnAccess() As Boolean

        'Se abre el archivo CSV selccionado en modo lectura y se le asigna un id
        FileOpen(1, TextBox_FileName.Text, OpenMode.Input)

        Dim readLine As String = ""
        Dim arrayLine() As String

        'Variables que contendrán las valores a guardar en access
        'Convertir al tipo de dato que espera recibir la BD
        Dim Dominio As String = ""
        Dim Numeros As Integer

        Dim ierr = 0

        Conexion.Open()
        Dim dcUser As OleDbCommand
        dcUser = New OleDb.OleDbCommand()
        Dim cmd As New OleDbCommand
        dcUser.Connection = Conexion
        cmd.Connection = Conexion
        Try
            Dim iSql As String = "DELETE * FROM brs_create_group"
            cmd.CommandText = iSql
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            'Continue Do
        End Try
        Conexion.Close()
        'Se lee linea por linea el archivo hasta que este acabe, EndOfFile
        While Not EOF(1)

            'Lee una linea del archivo con id = 1
            readLine = LineInput(1)
            arrayLine = Split(readLine, ";")
            Dominio = arrayLine(0).ToString()
            Numeros = Convert.ToInt32(arrayLine(1))
            'MsgBox(Dominio & " " & Numeros)

            'Instrucción SQL
            'Se insertan datos en los campos dominio y numbers de la tabla brs_create_group
            Dim cadenaSQl As String = "INSERT INTO brs_create_group (dominio, numbers)"
            cadenaSQl = cadenaSQl + " VALUES ( '" & Dominio & "',"
            cadenaSQl = cadenaSQl + "         " & Numeros & ")"

            'Crear un comando
            Dim Comando As OleDbCommand = Conexion.CreateCommand()
            Comando.CommandText = cadenaSQl

            'Ejecutar la consulta de accion (agregan registros)
            Try
                Conexion.Open()
                Comando.ExecuteNonQuery()
                'MsgBox("Se agregó correctamente el registro")
            Catch ex As Exception
                MsgBox(ex.ToString())
            End Try
            Conexion.Close()
        End While
        FileClose(1)
        actualizarGrilla()
        Return True
    End Function

    Private Sub btn_BrowseCSV_Click(sender As Object, e As EventArgs) Handles btn_BrowseCSV.Click
        'Se le pasan algunos parametros al openFileDialog
        openFileDialogCSV.Title = "Seleccione un archivo de extensión .CSV"
        openFileDialogCSV.InitialDirectory = My.Application.Info.DirectoryPath
        'MsgBox(My.Application.Info.DirectoryPath)
        openFileDialogCSV.FileName = ""
        openFileDialogCSV.ShowDialog()
        TextBox_FileName.Text = openFileDialogCSV.FileName
        Lab_wait.Visible = True
        Me.Cursor = Cursors.WaitCursor
        GuardarDatosEnAccess()
    End Sub



    Public Sub actualizarGrilla()
        'If txtCMMFileCSV.Text = "" Then
        '    Exit Sub
        'End If

        Conexion.Open()
        Dim iSql As String
        Dim cmd As New OleDbCommand
        Dim dt As New DataTable
        Dim da As New OleDbDataAdapter
        'If cluster <> 0 Then
        iSql = "select * from brs_create_group"
        'Else
        'iSql = "select * from brs_update_cmm_tmp"
        'End If
        Try
            cmd.Connection = Conexion
            cmd.CommandText = iSql
            cmd.CommandType = CommandType.TableDirect
            da.SelectCommand = cmd
            da.Fill(dt)
            ' muestro los resultados en la datagridview 
            DataGridView1.DataSource = dt
            DataGridView1.Refresh()
        Catch ex As Exception
            MsgBox("Can not open connection ! , " & ex.Message)
        End Try
        'If DataGridView1.RowCount = 0 Then
        '    MsgBox("No se encontraron registros", MsgBoxStyle.Exclamation, "Aviso!")
        '    Conexion.Close()
        '    'grpCMMUpdRecord.Visible = False
        '    'BtnCMMUpdate.Enabled = False
        '    Me.Cursor = Cursors.Arrow
        '    Exit Sub
        'End If
        Conexion.Close()
        'grpCMMUpdRecord.Visible = True
        'dataCMMGrdUpdate.CurrentCell = dataCMMGrdUpdate.Rows(0).Cells(0)
        'lblCMMUpdCurrentRow.Text = dataCMMGrdUpdate.CurrentCell.RowIndex + 1
        'lblCMMUpdTotalRows.Text = dataCMMGrdUpdate.RowCount
        Lab_wait.Visible = False
        Me.Cursor = Cursors.Default
        Interface_Salida()
    End Sub

    Private Sub BtnCMMOpenFile_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub Interface_Entrada()
        'Se ejecuta cuando se carga el formulario
        btn_procesar.Enabled = False
        btn_BrowseCSV.Enabled = True
        Lab_wait.Visible = False
    End Sub

    Private Sub Interface_Salida()
        btn_procesar.Enabled = True
        btn_BrowseCSV.Enabled = True
    End Sub
    Private Sub btn_procesar_Click(sender As Object, e As EventArgs) Handles btn_procesar.Click

    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub
End Class
