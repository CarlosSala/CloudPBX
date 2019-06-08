Imports System.Data.OleDb ' manejo de BD Access
Public Class Frm_Principal


    Dim ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
    Dim Conexion As New OleDbConnection(ConexionString)
    Private Sub For1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Function GuardarDatos() As Boolean

        'Se abre el archivo CSV en modo lectura y se le asigna un id
        FileOpen(1, TextBox_FileName.Text, OpenMode.Input)

        Dim regLine As String = ""
        Dim arrLine() As String

        'Se crean variables que contendran las valores que luego se guardaran en access
        'Convertir al tipo de dato que espera recibir la BD
        Dim Dominio As String = ""
        Dim Numeros As Integer

        Dim totalReg As Integer = 0
        Dim ierr = 0

        'Dim letras(10) As String
        'Dim contador As Integer = 0


        'letras(0) = "s"
        'letras(1) = "a"
        'letras(2) = "l"
        'letras(3) = "u"
        'letras(4) = "d"
        'letras(5) = "o"
        'letras(6) = "t"
        'letras(7) = "i"
        'letras(8) = "m"
        'letras(9) = "e"

        'Conexión, el puente entre la BD y el software


        While Not EOF(1)
            totalReg += 1
            regLine = LineInput(1)
            arrLine = Split(regLine, ";")
            Dominio = arrLine(0).ToString()
            Numeros = Convert.ToInt32(arrLine(1))

            MsgBox(arrLine(0) & " " & arrLine(1))

            'Instruccion SQL 
            'Se insertan datos en la tabla Personal, el nombre de la tabla va en minusculas
            Dim cadenaSQl As String = "INSERT INTO brs_create_group (dominio, numbers)"
            cadenaSQl = cadenaSQl + " VALUES ( '" & Dominio & "',"
            cadenaSQl = cadenaSQl + "         " & Numeros & ")"

            'Crear un comando
            Dim Comando As OleDbCommand = Conexion.CreateCommand()
            Comando.CommandText = cadenaSQl

            'Ejecutar la consulta de accion (agregan registros)

            Conexion.Open()
            Try

                MsgBox("se abrio correctamente la bd")
                Comando.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            'contador += 1
            Conexion.Close()
        End While


        actualizarGrilla()




        Return True
    End Function
    Private Sub AgregarNuevoToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim FAR As New Frm_AgregarRegistros
        FAR.ShowDialog()
    End Sub

    Private Sub ModificarToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim FMR As New Frm_ModificarRegistros
        FMR.ShowDialog()
    End Sub

    Private Sub EliminarToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim FER As New Frm_EliminarRegistros
        FER.ShowDialog()
    End Sub

    Private Sub SalirDelSistemaToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub ListarRegistroToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim Frm As New Frm_ListarRegistros
        Frm_ListarRegistros.Show()
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub BtnCMMBrowseCSV_Click(sender As Object, e As EventArgs) Handles BtnCMMBrowseCSV.Click
        CDFileCSV.Title = "Seleccione Archivo CSV"
        CDFileCSV.InitialDirectory = My.Application.Info.DirectoryPath
        CDFileCSV.FileName = ""
        CDFileCSV.ShowDialog()
        TextBox_FileName.Text = CDFileCSV.FileName
        GuardarDatos()
    End Sub

    Public Sub actualizarGrilla()
        'If txtCMMFileCSV.Text = "" Then
        '    Exit Sub
        'End If

        Me.Cursor = Cursors.WaitCursor
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
        If DataGridView1.RowCount = 0 Then
            MsgBox("No se encontraron registros", MsgBoxStyle.Exclamation, "Aviso!")
            Conexion.Close()
            'grpCMMUpdRecord.Visible = False
            'BtnCMMUpdate.Enabled = False
            Me.Cursor = Cursors.Arrow
            Exit Sub
        End If
        Conexion.Close()
        'grpCMMUpdRecord.Visible = True
        'dataCMMGrdUpdate.CurrentCell = dataCMMGrdUpdate.Rows(0).Cells(0)
        'lblCMMUpdCurrentRow.Text = dataCMMGrdUpdate.CurrentCell.RowIndex + 1
        'lblCMMUpdTotalRows.Text = dataCMMGrdUpdate.RowCount
        Me.Cursor = Cursors.Arrow
    End Sub












    Private Sub BtnCMMOpenFile_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub Interface_Entrada()
        'Se ejecuta cuando se carga el formulario
        'Lab_Id.Enabled = True
        'Text_Id.Enabled = True
        'btn_search.Enabled = True

        'Lab_Name.Enabled = False
        'Text_Name.Enabled = True
        'Lab_Address.Enabled = False
        'Text_Address.Enabled = False
        'Lab_Age.Enabled = False
        'Text_Age.Enabled = False
        'btn_save.Enabled = True
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_procesar.Click
        'GuardarDatos()
        'Interface_Entrada()
    End Sub


    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub
End Class
