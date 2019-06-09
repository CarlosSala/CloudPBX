Imports System.Data.OleDb 'manejo de BD Access
Public Class Frm_Principal

    Dim ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
    Dim Conexion As New OleDbConnection(ConexionString)

    Private Sub For1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Interface_Entrada()
    End Sub

    Function GuardarDatosEnAccess() As Boolean

        'validar que se escogio un archivo
        If TextBox_FileName.Text = "" Then
            Lab_wait.Visible = False
            Me.Cursor = Cursors.Default
            Exit Function
        End If
        'Se abre el archivo CSV selccionado en modo lectura y se le asigna un id
        FileOpen(1, TextBox_FileName.Text, OpenMode.Input)

        Dim readLine As String = ""
        Dim arrayLine() As String

        'Variables que contendrán las valores a guardar en access
        'Convertir al tipo de dato que espera recibir la BD
        Dim Dominio As String = ""
        Dim Numeros As Long

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
            'Se debe modificar el tipo de dato, de numero entero largo a -> doble
            'Access no redondea cifras automaticamente si estas estan en formato general y si no superan los 16 caracteres
            Numeros = Convert.ToInt64(arrayLine(1))

            'MsgBox(Dominio & " " & Numeros.ToString())

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
        openFileDialogCSV.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop
        'MsgBox(My.Application.Info.DirectoryPath & Environment.GetFolderPath.DesktopDirectory)
        openFileDialogCSV.FileName = ""
        openFileDialogCSV.Filter = "Text files (*.csv)|*.csv"
        openFileDialogCSV.ShowDialog()
        TextBox_FileName.Text = openFileDialogCSV.FileName
        Lab_wait.Visible = True
        Me.Cursor = Cursors.WaitCursor
        GuardarDatosEnAccess()
    End Sub

    Public Sub actualizarGrilla()

        Conexion.Open()
        Dim iSql As String
        Dim cmd As New OleDbCommand
        Dim dt As New DataTable
        Dim da As New OleDbDataAdapter

        iSql = "select * from brs_create_group"

        Try
            cmd.Connection = Conexion
            cmd.CommandText = iSql
            cmd.CommandType = CommandType.TableDirect
            da.SelectCommand = cmd
            da.Fill(dt)
            'Se muestran los datos en el datagridview 
            DataGridView1.DataSource = dt
            DataGridView1.Refresh()
        Catch ex As Exception
            MsgBox("Can not open connection ! , " & ex.Message)
        End Try

        Conexion.Close()
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
