Imports System.Xml
Imports System.IO
Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Public Class Frm_Principal

    Private ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.Database
    Private Conexion As New OleDbConnection(ConexionString)

    Dim gblPathAppl As String
    Dim gblPathLog As String
    Dim gblPathTmpCloud As String
    Dim gblPathTmpProxy As String
    Dim gblPathTmpUserLicense As String
    Dim gblTimePing As Integer = 2000
    Dim gblSession As String = ""
    Dim indexXML_Cloud As Integer = 0
    Dim indexXML_Proxy_DVmac As Integer = 0
    Dim indexXML_Proxy As Integer = 0
    Dim indexXML_UsersLincense_Group As Integer = 0
    Dim indexXML_UsersLincense = 0
    Dim indexXML_UsersLicense_Assign As Integer = 0
    Dim indexXML_UsersLicense_UnAssign As Integer = 0
    Dim codError As Integer = 0
    Dim numFile As Integer = 1
    'Dim n_File As Integer = FreeFile()

    Dim domain As String = ""
    Dim phoneNumber As String = ""
    Dim group_id As String = ""
    Dim group_name As String = ""
    Dim contact_name As String = ""
    Dim contact_number As String = ""
    Dim address As String = ""
    Dim city As String = ""
    Dim device_type As String = ""
    Dim mac As String = ""
    Dim serial_number As String = ""
    Dim physical_location As String = ""
    Dim department As String = ""
    Dim first_name As String = ""
    Dim last_name As String = ""
    Dim user_email As String = ""
    Dim user_address As String = ""
    Dim user_city As String = ""
    Dim proxy As String = ""
    Dim extensions As String = ""
    Dim ocp_local As String = ""
    Dim ocp_tollFree As String = ""
    Dim ocp_internacional As String = ""
    Dim ocp_special1 As String = ""
    Dim ocp_special2 As String = ""
    Dim ocp_premium1 As String = ""

    Dim filasValidas As Integer
    Dim infoContact As Integer = 0
    Dim arregloDeptos() As String
    Dim proxyInfo As Integer = 0
    Dim estadoCeldas As Integer = 0
    Dim ArrayUserGetList() As String

    Private Sub For1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        First_Interface()
        Second_Interface()
        Third_Interface()

        gblPathAppl = My.Application.Info.DirectoryPath & My.Settings.PathAppl
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom
        gblPathLog = My.Application.Info.DirectoryPath & My.Settings.PathLog
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\log
        gblPathTmpCloud = My.Application.Info.DirectoryPath & My.Settings.PathTmpCloud
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp_cloud
        gblPathTmpProxy = My.Application.Info.DirectoryPath & My.Settings.PathTmpProxy
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp_proxy
        gblPathTmpUserLicense = My.Application.Info.DirectoryPath & My.Settings.PathTmpUserLicense
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp_userLicense
    End Sub

    Private Sub First_Interface()

        btn_browse_CSV.Enabled = True
        lbl_wait.Visible = False
        btn_report_cloudpbx.Enabled = False
        btn_show_report.Enabled = False
        btn_procesar.Enabled = False
        btn_validate_data.Enabled = False
        lbl_state_cloud.Text = ""
    End Sub

    Private Sub Second_Interface()

        listbox_proxy.Enabled = False
        cb_modify_proxy.Enabled = False
        cb_add_proxy.Enabled = False
        tb_write_proxy.Enabled = False
        btn_process_proxy.Enabled = False
        lbl_proxy.Enabled = False
        lbl_state_proxy.Text = ""
    End Sub

    Private Sub Third_Interface()

        lbl_state_userLicense.Text = ""
        DataGridView2.EnableHeadersVisualStyles = False
        DataGridView2.Enabled = False
        btn_process_userLicense.Enabled = False
    End Sub

    Public Sub Tooltip_Help_Buttons(ByVal TooltipAyuda As ToolTip, ByVal Boton As Button, ByVal mensaje As String)

        ToolTipHelpButtons.RemoveAll()
        ToolTipHelpButtons.SetToolTip(Boton, mensaje)
        ToolTipHelpButtons.InitialDelay = 500
        ToolTipHelpButtons.IsBalloon = False
    End Sub

    Private Sub Btn_browse_CSV_Click(sender As Object, e As EventArgs) Handles btn_browse_CSV.Click

        openFileDialogCSV.Title = "Seleccione un archivo de extensión .csv o .txt"
        openFileDialogCSV.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop
        openFileDialogCSV.FileName = ""
        'openFileDialogCSV.Filter = "Text files (*.csv)|*.csv|Text files (*.txt)|*.txt"
        openFileDialogCSV.Filter = "Text files (*.csv; *.txt)|*.csv; *.txt"
        openFileDialogCSV.Multiselect = False
        openFileDialogCSV.CheckFileExists = True
        openFileDialogCSV.ShowDialog()
        tb_file_name.Text = openFileDialogCSV.FileName
        Validate_File()
    End Sub

    Private Sub In_Case_Error()
        lbl_wait.Visible = False
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub Validate_File()

        lbl_wait.Visible = True
        Me.Cursor = Cursors.WaitCursor
        My.Application.DoEvents()

        'En el siguiente método, se valida que:
        'Se haya seleccionado un archivo
        'El archivo no se encuentre en uso
        'El archivo no este vacio
        'El archivo posea 26 columnas por cada fila, sin excepción

        'Si no se escogió ningun archivo se expulsa del metodo
        If tb_file_name.Text = "" Then
            In_Case_Error()
            Exit Sub
        End If

        'Se abre el archivo selccionado en modo lectura y se le asigna un Id
        Try
            FileOpen(1, tb_file_name.Text, OpenMode.Input)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo " & tb_file_name.Text.ToString & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(1)
            In_Case_Error()
            Exit Sub
        End Try

        'Se valida el formato del archivo
        Dim controlEmptyFile As Integer = 0

        'Se lee linea por linea el archivo con Id = 1, hasta que este acabe, EndOfFile
        While Not EOF(1)

            'Si controlEmptyFile cambia a 1, el archivo no esta vacio
            controlEmptyFile = 1

            'Se lee una linea del archivo en cada ejecución
            Dim readLine As String = ""
            Dim arrayLine() As String
            readLine = LineInput(1)
            arrayLine = Split(readLine, ";")

            'Se comprueba que cada linea del archivo contenga 26 columnas por fila
            If arrayLine.Length <> 26 Then
                MsgBox("Revise el número de columnas del archivo cargado", MsgBoxStyle.Exclamation, "Error en la estructura del archivo")
                FileClose(1)
                In_Case_Error()
                Exit Sub
            End If
        End While

        If controlEmptyFile = 0 Then
            MsgBox("El archivo esta vacio", MsgBoxStyle.Exclamation, "Error en la estructura del archivo")
            FileClose(1)
            In_Case_Error()
            Exit Sub
        End If

        FileClose(1)
        Save_Data_Access()
    End Sub

    Private Sub Save_Data_Access()

        'Se abre el archivo selccionado en modo lectura y se le asigna un id
        Try
            FileOpen(1, tb_file_name.Text, OpenMode.Input)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo " & tb_file_name.Text.ToString & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(1)
            In_Case_Error()
            Exit Sub
        End Try

        'Se eliminan los datos antiguos de la tabla brs_cloudpbx
        Dim cmd As New OleDbCommand()
        cmd.Connection = Conexion
        Dim instructionSQL As String = "delete * from brs_cloudpbx"
        cmd.CommandText = instructionSQL
        Try
            Conexion.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los datos antiguos de la tabla brs_cloudpbx", MsgBoxStyle.Exclamation, "Error con base de datos")
            FileClose(1)
            Conexion.Close()
            In_Case_Error()
            Exit Sub
        End Try
        Conexion.Close()

        'Variables que contendrán las valores a guardar en Access
        Dim Dominio As String = ""
        Dim Numeros As String = ""
        Dim Nombre_grupo As String = ""
        Dim Nombre_empresa As String = ""
        Dim Nombre_contacto As String = ""
        Dim Telefono_contacto As String = ""
        Dim Direccion_empresa As String = ""
        Dim Ciudad As String = ""
        Dim Tipo_dispositivo As String = ""
        Dim Mac As String = ""
        Dim Numero_serie As String = ""
        Dim Locacion_fisica As String = ""
        Dim Departamento As String = ""
        Dim Nombre_usuario As String = ""
        Dim Apellido_usuario As String = ""
        Dim Correo_usuario As String = ""
        Dim Direccion_usuario As String = ""
        Dim Ciudad_usuario As String = ""
        Dim Proxy As String = ""
        Dim Extensiones As String = ""
        Dim OCP_local As String = ""
        Dim OCP_linea_gratis As String = ""
        Dim OCP_internacional As String = ""
        Dim OCP_especial1 As String = ""
        Dim OCP_especial2 As String = ""
        Dim OCP_premium1 As String = ""

        Dim readLine As String = ""
        Dim arrayLine() As String

        'Se lee linea por linea el archivo con Id = 1, hasta que este acabe, EndOfFile
        While Not EOF(1)

            readLine = LineInput(1)
            arrayLine = Split(readLine, ";")
            Dominio = arrayLine(0).ToString
            Numeros = arrayLine(1).ToString
            Nombre_grupo = arrayLine(2).ToString
            Nombre_empresa = arrayLine(3).ToString
            Nombre_contacto = arrayLine(4).ToString
            Telefono_contacto = arrayLine(5).ToString
            Direccion_empresa = arrayLine(6).ToString
            Ciudad = arrayLine(7).ToString
            Tipo_dispositivo = arrayLine(8).ToString
            Mac = arrayLine(9).ToString
            Numero_serie = arrayLine(10).ToString
            Locacion_fisica = arrayLine(11).ToString
            Departamento = arrayLine(12).ToString
            Nombre_usuario = arrayLine(13).ToString
            Apellido_usuario = arrayLine(14).ToString
            Correo_usuario = arrayLine(15).ToString
            Direccion_usuario = arrayLine(16).ToString
            Ciudad_usuario = arrayLine(17).ToString
            Proxy = arrayLine(18).ToString
            Extensiones = arrayLine(19).ToString
            OCP_local = arrayLine(20).ToString
            OCP_linea_gratis = arrayLine(21).ToString
            OCP_internacional = arrayLine(22).ToString
            OCP_especial1 = arrayLine(23).ToString
            OCP_especial2 = arrayLine(24).ToString
            OCP_premium1 = arrayLine(25).ToString

            Dim cadenaSQL As String = "insert into brs_cloudpbx ([domain], numbers, group_id, group_name, contact_name, contact_phone, enterprise_address, city,
                                                                        device_type, mac, serial_number, physical_location, deparment_name,
                                                                        first_name, last_name, user_email, user_address, user_city,
                                                                        proxy, extensions,
                                                                        ocp_local, ocp_tollFree, ocp_international, specialServices1, specialServices2, premiumServices1)"
            cadenaSQL += " VALUES ( '" & Dominio & "',"
            cadenaSQL += "          '" & Numeros & "',"
            cadenaSQL += "          '" & Nombre_grupo & "',"
            cadenaSQL += "          '" & Nombre_empresa & "',"
            cadenaSQL += "          '" & Nombre_contacto & "',"
            cadenaSQL += "          '" & Telefono_contacto & "',"
            cadenaSQL += "          '" & Direccion_empresa & "',"
            cadenaSQL += "          '" & Ciudad & "',"
            cadenaSQL += "          '" & Tipo_dispositivo & "',"
            cadenaSQL += "          '" & Mac & "',"
            cadenaSQL += "          '" & Numero_serie & "',"
            cadenaSQL += "          '" & Locacion_fisica & "',"
            cadenaSQL += "          '" & Departamento & "',"
            cadenaSQL += "          '" & Nombre_usuario & "',"
            cadenaSQL += "          '" & Apellido_usuario & "',"
            cadenaSQL += "          '" & Correo_usuario & "',"
            cadenaSQL += "          '" & Direccion_usuario & "',"
            cadenaSQL += "          '" & Ciudad_usuario & "',"
            cadenaSQL += "          '" & Proxy & "',"
            cadenaSQL += "          '" & Extensiones & "',"
            cadenaSQL += "          '" & OCP_local & "',"
            cadenaSQL += "          '" & OCP_linea_gratis & "',"
            cadenaSQL += "          '" & OCP_internacional & "',"
            cadenaSQL += "          '" & OCP_especial1 & "',"
            cadenaSQL += "          '" & OCP_especial2 & "',"
            cadenaSQL += "          '" & OCP_premium1 & "')"

            'Se crea el comando
            Dim Comando As OleDbCommand = Conexion.CreateCommand()
            Comando.CommandText = cadenaSQL

            'Se Ejecuta la consulta de accion (agregar registros)
            Try
                Conexion.Open()
                Comando.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("No se pudieron insertar los nuevos datos en la tabla brs_cloudpbx", MsgBoxStyle.Exclamation, "Error con base de datos")
                FileClose(1)
                Conexion.Close()
                In_Case_Error()
                Exit Sub
            End Try
            Conexion.Close()
        End While

        FileClose(1)
        lbl_state_cloud.Text = ""
        ProgressBar1.Value = 0
        Update_Grid()
    End Sub

    Public Sub Update_Grid()

        Dim cmd As New OleDbCommand
        Dim dt As New DataTable
        Dim da As New OleDbDataAdapter
        Dim iSQL As String = "select * from brs_cloudpbx"

        'Se ejecuta la consulta para traer registros
        Try
            Conexion.Open()
            cmd.Connection = Conexion
            cmd.CommandText = iSQL
            da.SelectCommand = cmd
            da.Fill(dt)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudo traer la información desde la tabla brs_cloudpbx", MsgBoxStyle.Exclamation, "Error con base de datos")
            Conexion.Close()
            In_Case_Error()
            Exit Sub
        End Try
        Conexion.Close()

        'Se muestran los datos en el datagridview 
        DataGridView1.DataSource = dt
        DataGridView1.Refresh()

        'Se evita que el usuario pueda reordenar la grilla
        For j = 0 To DataGridView1.ColumnCount - 1
            DataGridView1.Columns(j).SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        'DataGridView1.CurrentCell = DataGridView1.Rows(0).Cells(0)
        'lblCMMUpdCurrentRow.Text = DataGridView1.CurrentCell.RowIndex + 1
        'lblCMMUpdTotalRows.Text = DataGridView1.RowCount

        lbl_wait.Visible = False
        Me.Cursor = Cursors.Default
        btn_procesar.Enabled = False
        btn_validate_data.Enabled = True
        My.Application.DoEvents()
        'Validate_Data()
    End Sub

    Public Function Validate_Data() As Integer

        'Esta variable se usa para controlar que la data supere las pruebas de validación
        estadoCeldas = 0

        'INFORMACION OBLIGATORIA. A CONTINUACION SE VALIDA QUE:

        'domain sea mayor a un largo 3
        'todos los phoneNumber tengan un largo mayor a 8
        'group_id sea mayor a un largo 3
        'group_name sea mayor a un largo 3
        'address sea mayor a un largo 3
        'city sea mayor a un largo 3
        'device_type se mayor a un largo 11
        'mac sea mayor a un largo 11
        'serial_number sea mayor a un largo 15
        'physical_location sea mayor a un largo 3
        'first_name sea mayor a un largo 1
        'last_name sea mayor a un largo 0
        'user_address sea mayor a un largo 3
        'user_city sea mayor a un largo 3
        'first_name sea mayor a un largo 1
        'extensions sea mayor a un largo 1
        'ocp_local sea mayor a un largo 8
        'ocp_tollFree sea mayor a un largo 8
        'ocp_internacional sea mayor a un largo 8
        'ocp_special1 sea mayor a un largo 8
        'ocp_special2 sea mayor a un largo 8
        'ocp_premium1 sea mayor a un largo 8

        'Dim mc As MatchCollection = Regex.Matches(domain, pattern)
        'Dim m As Match

        'Dim contador As Integer = 0

        'For Each m In mc
        '    contador += 1
        '    m.Value, m.Index
        'Next m

        ''Si la cantidad de matches es igual a al largo del dominio
        'If domain.Length = mc.Count Then


        'Validación del dominio----------------------------------------------------------------------------------------------------
        domain = DataGridView1.Rows(0).Cells(0).Value.ToString.ToLower 'domain = dt.Rows(0)(0).ToString.ToLower

        Dim pattern As String = "\A[a-no-z0-9\-\.\:]{1,76}\.(cl|com|org){1}\Z"

        If Regex.IsMatch(domain, pattern) Then
            DataGridView1.Rows(0).Cells(0).Style.BackColor = Color.FromArgb(0, 247, 0)
        Else
            DataGridView1.Rows(0).Cells(0).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas = 1
        End If

        'Validación numeración-------------------------------------------------------------------------------------------------
        For j = 0 To DataGridView1.Rows.Count - 2  'dt.Rows.Count - 1
            phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString

            pattern = "\A[0-9]{9}\Z"

            If Regex.IsMatch(phoneNumber, pattern) Then
                DataGridView1.Rows(j).Cells(1).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(1).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If
        Next

        'Validación de información del grupo---------------------------------------------------------------------------------------
        group_id = DataGridView1.Rows(0).Cells(2).Value.ToString
        group_name = DataGridView1.Rows(0).Cells(3).Value.ToString
        address = DataGridView1.Rows(0).Cells(6).Value.ToString
        city = DataGridView1.Rows(0).Cells(7).Value.ToString

        pattern = "\A[\w]{1,19}(_cloudpbx){1}\Z"
        If Regex.IsMatch(group_id, pattern) Then
            DataGridView1.Rows(0).Cells(2).Style.BackColor = Color.FromArgb(0, 247, 0)
        Else
            DataGridView1.Rows(0).Cells(2).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas = 1
        End If

        pattern = "\A(([a-no-zA-NO-Z0-9_]\.{0,1}\s{0,1})){1,80}\Z"
        If Regex.IsMatch(group_name, pattern) Then
            DataGridView1.Rows(0).Cells(3).Style.BackColor = Color.FromArgb(0, 247, 0)
        Else
            DataGridView1.Rows(0).Cells(3).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas = 1
        End If

        pattern = "\A([\w]\s{0,1}){1,80}\Z"
        If Regex.IsMatch(address, pattern) Then
            DataGridView1.Rows(0).Cells(6).Style.BackColor = Color.FromArgb(0, 247, 0)
        Else
            DataGridView1.Rows(0).Cells(6).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas = 1
        End If

        pattern = "\A([\w]\s{0,1}){1,80}\Z"
        If Regex.IsMatch(city, pattern) Then
            DataGridView1.Rows(0).Cells(7).Style.BackColor = Color.FromArgb(0, 247, 0)
        Else
            DataGridView1.Rows(0).Cells(7).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas = 1
        End If

        'pattern = "[a-no-zA-NO-Z0-9_\.\,\-\s]"
        Dim mc As MatchCollection = Regex.Matches(address, pattern)

        'Dim m As Match
        'Dim contador1 As Integer = 0

        'For Each m In mc
        '    contador1 += 1
        '    'm.Value, m.Index
        'Next m
        ''Si la cantidad de matches es igual a al largo del dominio

        'validar información de los dispositivos-----------------------------------------------------------------------------------
        'La columna 'device_type' es la referencia para las demas, por ello se valida lo sigte:
        'No puede estar vacia la primera celda
        'No puede haber celdas vacias entremedio

        If DataGridView1.Rows(0).Cells(8).Value.ToString.Length > 11 Then
            DataGridView1.Rows(0).Cells(8).Style.BackColor = Color.FromArgb(0, 247, 0)
        Else
            DataGridView1.Rows(0).Cells(8).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas = 1
        End If

        filasValidas = 0
        'For para saber cantidad de filas no vacias de la columna device_type
        For j = 0 To DataGridView1.Rows.Count - 2
            If DataGridView1.Rows(j).Cells(8).Value.ToString.Length > 0 Then
                filasValidas += 1
            End If
        Next

        For j = 0 To filasValidas - 1
            mac = DataGridView1.Rows(j).Cells(9).Value.ToString
            device_type = DataGridView1.Rows(j).Cells(8).Value.ToString
            serial_number = DataGridView1.Rows(j).Cells(10).Value.ToString
            physical_location = DataGridView1.Rows(j).Cells(11).Value.ToString

            pattern = "\A[a-fA-F0-9]{12}\Z"
            If Regex.IsMatch(mac, pattern) Then
                DataGridView1.Rows(j).Cells(9).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(9).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            'Si se compara con el signo = un string, no importaran las mayusculas o minusculas
            'If device_type.Equals("Yealink-T19xE2") Or device_type.Equals("Yealink-T21xE2") Or device_type.Equals("Yealink-T27G") Then
            pattern = "\A((Yealink-T19xE2)|(Yealink-T21xE2)|(Yealink-T27G)){1}\Z"
            If Regex.IsMatch(device_type, pattern) Then
                DataGridView1.Rows(j).Cells(8).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(8).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            pattern = "\A[0-9]{16}\Z"
            If Regex.IsMatch(serial_number, pattern) Then
                DataGridView1.Rows(j).Cells(10).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(10).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            pattern = "\A[a-no-zA-NO-Z0-9_]{1,12}\Z"
            If Regex.IsMatch(physical_location, pattern) Then
                DataGridView1.Rows(j).Cells(11).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(11).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If
        Next

        'validar información de los usuarios------------------------------------------------------------------------------------------
        'validar creación de los departamentos
        Dim varAcumulaDepto As String = ""
        Dim arreglo() As String
        ReDim arregloDeptos(DataGridView1.Rows.Count - 2)
        Dim index As Integer
        Dim numElementos As Integer = 0

        For i = 0 To DataGridView1.RowCount - 2
            varAcumulaDepto += DataGridView1.Rows(i).Cells(12).Value.ToString & ";"
        Next

        arreglo = Split(varAcumulaDepto, ";")

        For k = 0 To arreglo.Length - 1
            index = Array.IndexOf(arregloDeptos, arreglo(k))
            'Se guarda en arregloDeptos todo aquello que no este repetido y que tenga un largo mayor a cero
            If index = -1 And arreglo(k).Length > 0 Then
                arregloDeptos(numElementos) = arreglo(k)
                'MsgBox("Se guardó el elemento: " & arregloDeptos(numElementos) & " en arregloDeptos")
                numElementos += 1
            Else
                'MsgBox("Elemento repetido no se guardó")
            End If
        Next

        ReDim Preserve arregloDeptos(numElementos - 1)
        'MsgBox("cantidad de departamentos a crear " & arregloDeptos.Length)
        'For Each elemento As String In arregloDeptos
        '    MsgBox(" arreglo final con los deptos " & vbCrLf & elemento)
        'Next
        For j = 0 To filasValidas - 1
            department = DataGridView1.Rows(j).Cells(12).Value.ToString

            pattern = "\A([\w\.\,\-]\s{0,1}){1,80}\Z"

            If Regex.IsMatch(department, pattern) Then
                DataGridView1.Rows(j).Cells(12).Style.BackColor = Color.FromArgb(0, 247, 0)
            ElseIf Not Regex.IsMatch(department, pattern) And department.length = 0 Then
                DataGridView1.Rows(j).Cells(12).Style.BackColor = Color.FromArgb(255, 255, 255)
            ElseIf Not Regex.IsMatch(department, pattern) And department.length > 0 Then
                DataGridView1.Rows(j).Cells(12).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If
        Next

        For j = 0 To filasValidas - 1
            first_name = DataGridView1.Rows(j).Cells(13).Value.ToString
            last_name = DataGridView1.Rows(j).Cells(14).Value.ToString
            user_address = DataGridView1.Rows(j).Cells(16).Value.ToString
            user_city = DataGridView1.Rows(j).Cells(17).Value.ToString
            extensions = DataGridView1.Rows(j).Cells(19).Value.ToString
            ocp_local = DataGridView1.Rows(j).Cells(20).Value.ToString
            ocp_tollFree = DataGridView1.Rows(j).Cells(21).Value.ToString
            ocp_internacional = DataGridView1.Rows(j).Cells(22).Value.ToString
            ocp_special1 = DataGridView1.Rows(j).Cells(23).Value.ToString
            ocp_special2 = DataGridView1.Rows(j).Cells(24).Value.ToString
            ocp_premium1 = DataGridView1.Rows(j).Cells(25).Value.ToString

            pattern = "\A([a-no-zA-NO-Z0-9_\.\-]\s{0,1}){1,30}\Z"

            If Regex.IsMatch(first_name, pattern) Then
                DataGridView1.Rows(j).Cells(13).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(13).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            If Regex.IsMatch(last_name, pattern) Then
                DataGridView1.Rows(j).Cells(14).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(14).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            pattern = "\A([\w\,]\s{0,1}){1,80}\Z"

            If Regex.IsMatch(user_address, pattern) Then
                DataGridView1.Rows(j).Cells(16).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(16).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            If Regex.IsMatch(user_city, pattern) Then
                DataGridView1.Rows(j).Cells(17).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(17).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            pattern = "\A[0-9]{2,5}\Z"

            If Regex.IsMatch(extensions, pattern) Then
                DataGridView1.Rows(j).Cells(19).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(19).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            If ocp_local.Equals("bloqueado") Or ocp_local.Equals("Bloqueado") Or ocp_local.Equals("BLOQUEADO") Or ocp_local.Equals("desbloqueado") Or ocp_local.Equals("Desbloqueado") Or ocp_local.Equals("DESBLOQUEADO") Then
                DataGridView1.Rows(j).Cells(20).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(20).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            If ocp_tollFree.Equals("bloqueado") Or ocp_tollFree.Equals("Bloqueado") Or ocp_tollFree.Equals("BLOQUEADO") Or ocp_tollFree.Equals("desbloqueado") Or ocp_tollFree.Equals("Desbloqueado") Or ocp_tollFree.Equals("DESBLOQUEADO") Then
                DataGridView1.Rows(j).Cells(21).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(21).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            If ocp_internacional.Equals("bloqueado") Or ocp_internacional.Equals("Bloqueado") Or ocp_internacional.Equals("BLOQUEADO") Or ocp_internacional.Equals("desbloqueado") Or ocp_internacional.Equals("Desbloqueado") Or ocp_internacional.Equals("DESBLOQUEADO") Then
                DataGridView1.Rows(j).Cells(22).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(22).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            If ocp_special1.Equals("bloqueado") Or ocp_special1.Equals("Bloqueado") Or ocp_special1.Equals("BLOQUEADO") Or ocp_special1.Equals("desbloqueado") Or ocp_special1.Equals("Desbloqueado") Or ocp_special1.Equals("DESBLOQUEADO") Then
                DataGridView1.Rows(j).Cells(23).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(23).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            If ocp_special2.Equals("bloqueado") Or ocp_special2.Equals("Bloqueado") Or ocp_special2.Equals("BLOQUEADO") Or ocp_special2.Equals("desbloqueado") Or ocp_special2.Equals("Desbloqueado") Or ocp_special2.Equals("DESBLOQUEADO") Then
                DataGridView1.Rows(j).Cells(24).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(24).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If

            If ocp_premium1.Equals("bloqueado") Or ocp_premium1.Equals("Bloqueado") Or ocp_premium1.Equals("BLOQUEADO") Or ocp_premium1.Equals("desbloqueado") Or ocp_premium1.Equals("Desbloqueado") Or ocp_premium1.Equals("DESBLOQUEADO") Then
                DataGridView1.Rows(j).Cells(25).Style.BackColor = Color.FromArgb(0, 247, 0)
            Else
                DataGridView1.Rows(j).Cells(25).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            End If
        Next

        'INFORMACION OPCIONAL. A CONTINUACION SE VALIDA QUE:

        'Para añadir contact_name y conctact_number al archivo xml correspondiente:
        'contact_name debe ser mayor a un largo 3 y
        'contact_number deber ser mayor a un largo 8
        'user_email  debe ser mayor a un largo 10
        'proxy sea mayor a un largo 7

        contact_name = DataGridView1.Rows(0).Cells(4).Value.ToString
        contact_number = DataGridView1.Rows(0).Cells(5).Value.ToString

        pattern = "\A([\w\.\,\-]\s{0,1}){1,30}\Z"

        'mc = Regex.Matches(contact_name, pattern)
        'Dim mc1 As MatchCollection = Regex.Matches(contact_number, pattern)

        'Se llenan los campos y ambos cumplen
        If Regex.IsMatch(contact_name, pattern) And Regex.IsMatch(contact_number, pattern) Then
            DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(0, 247, 0)
            infoContact = 1

            'Se llenan los campos y Niguno cumple
        ElseIf Not Regex.IsMatch(contact_name, pattern) And Not Regex.IsMatch(contact_number, pattern) Then
            'Ninguno cumple (apropósito)
            If contact_name.Length = 0 And contact_number.Length = 0 Then
                DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(255, 255, 255)
                DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(255, 255, 255)
                infoContact = 0
            Else  ''Ninguno cumple (Intento fallido)
                DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(254, 84, 97)
                infoContact = 0
                estadoCeldas = 1
            End If

            'Solo el primero cumple
        ElseIf Regex.IsMatch(contact_name, pattern) And Not Regex.IsMatch(contact_number, pattern) Then
            DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(254, 84, 97)
            infoContact = 0
            estadoCeldas = 1

            'Solo el segundo cumple
        ElseIf Not Regex.IsMatch(contact_name, pattern) And Regex.IsMatch(contact_number, pattern) Then
            DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(254, 84, 97)
            DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(0, 247, 0)
            infoContact = 0
            estadoCeldas = 1
        End If

        pattern = "^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"

        For j = 0 To filasValidas - 1
            user_email = DataGridView1.Rows(j).Cells(15).Value.ToString

            If Regex.IsMatch(user_email, pattern) Then
                DataGridView1.Rows(j).Cells(15).Style.BackColor = Color.FromArgb(0, 247, 0)
            ElseIf Not Regex.IsMatch(user_email, pattern) And user_email.Length > 0 Then
                DataGridView1.Rows(j).Cells(15).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas = 1
            ElseIf Not Regex.IsMatch(user_email, pattern) And user_email.Length = 0 Then
                DataGridView1.Rows(j).Cells(15).Style.BackColor = Color.FromArgb(255, 255, 255)
            End If
        Next

        proxy = DataGridView1.Rows(0).Cells(18).Value.ToString
        pattern = "^(?:(?:[1-9]?[0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}(?:[1-9]?[0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$|^localhost$"

        If Regex.IsMatch(proxy, pattern) Then
            DataGridView1.Rows(0).Cells(18).Style.BackColor = Color.FromArgb(0, 247, 0)
            proxyInfo = 1
        ElseIf proxy.Length > 0 Then
            DataGridView1.Rows(0).Cells(18).Style.BackColor = Color.FromArgb(254, 84, 97)
            proxyInfo = 0
            estadoCeldas = 1
        ElseIf proxy.Length = 0 Then
            DataGridView1.Rows(0).Cells(18).Style.BackColor = Color.FromArgb(255, 255, 255)
            proxyInfo = 0
        End If

        For fila As Integer = 0 To DataGridView1.RowCount - 1
            For columna As Integer = 0 To DataGridView1.ColumnCount - 1

                'Se bloquean todas las filas superiores a la posicion 0, en las columnas 0,2,3,4,5,6,7 y 18
                If columna = 0 Or columna > 1 And columna < 8 Or columna = 18 Then
                    If fila > 0 Then
                        DataGridView1.Rows(fila).Cells(columna).ReadOnly = True
                        DataGridView1.Rows(fila).Cells(columna).Style.BackColor = Color.FromArgb(232, 232, 232)
                    End If
                Else
                    'filasValidas - 1 se iguala con fila, ya que esta parte en 0
                    If fila > filasValidas - 1 And columna <> 1 And columna <> 8 Then
                        DataGridView1.Rows(fila).Cells(columna).ReadOnly = True
                        DataGridView1.Rows(fila).Cells(columna).Style.BackColor = Color.FromArgb(232, 232, 232)
                    Else
                        DataGridView1.Rows(fila).Cells(columna).ReadOnly = False
                    End If
                End If
                'MsgBox(Me.DataGridView1.Item(Columnas, filas).Value)
            Next
        Next

        'validar que la columna de los numeros sea igual o mayor a la de los dispositivos

        If estadoCeldas = 0 Then
            btn_procesar.Enabled = True
            Return 0
        Else
            btn_procesar.Enabled = False
            'MsgBox("revise las celdas")
            Return 1
        End If
    End Function

    'Se esta validando nuevamente el codigo se va aqui desde donde se llama al metodo por primera vez
    Private Sub In_Case_Error1()
        FileClose(numFile)
        FileClose(1)
        Me.Cursor = Cursors.Default
        btn_procesar.Enabled = True
        btn_validate_data.Enabled = True
        btn_browse_CSV.Enabled = True
    End Sub

    Private Sub Btn_procesar_Click(sender As Object, e As EventArgs) Handles btn_procesar.Click

        'Para comprobar conexión con el servidor
        If My.Computer.Network.Ping(My.Settings.Host, gblTimePing) Then
            'MsgBox("Server pinged successfully.")
        Else
            MsgBox("Servidor fuera de Linea, favor verifique la conexion", MsgBoxStyle.Exclamation, "Error de Comunicación")
            Exit Sub
        End If

        'Se llama a la validación de la data por ultima vez
        Validate_Data()
        Dim estado = Validate_Data()
        If estado = 0 Then

        Else
            'Me.Cursor = Cursors.Default
            btn_procesar.Enabled = False
            'MsgBox("revise las celdas")
            Exit Sub
        End If

        indexXML_Cloud = 0
        Me.Cursor = Cursors.WaitCursor
        btn_procesar.Enabled = False
        btn_browse_CSV.Enabled = False
        btn_validate_data.Enabled = False
        My.Application.DoEvents()

        'FASE 1

        ' Chr(34) = " 
        Dim line1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim line2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim line3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"

        'XML PARA CREAR UN DOMINIO-----------------------------------------------------------------------------------------------------------
        Dim a_4 As String = "<command xsi:type=" & Chr(34) & "SystemDomainAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim a_5 As String = "<domain>pruebacarlos.cl</domain>"
        Dim a_6 As String = "</command>"

        'XML PARA ASIGNAR EL DOMINIO CREADO----------------------------------------------------------------------
        Dim b_4 As String = "<command xsi:type=" & Chr(34) & "ServiceProviderDomainAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim b_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim b_6 As String = "<domain>pruebacarlos.cl</domain>"
        Dim b_7 As String = "</command>"

        'XML PARA CREAR NUMERACIÓN------------------------------------------------------------------------------
        Dim c_4 As String = "<command xsi:type=" & Chr(34) & "ServiceProviderDnAddListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim c_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim c_6 As String = "<phoneNumber>232781567</phoneNumber>"
        Dim c_7 As String = "</command>"

        ' 'XML PARA CREAR PERFIL DE GRUPO-------------------------------------------------------------------------
        Dim d_4 As String = "<command xsi:type=" & Chr(34) & "GroupAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim d_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim d_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim d_7 As String = "<defaultDomain>pruebacarlos.cl</defaultDomain>"
        Dim d_8 As String = "<userLimit>100</userLimit>"
        Dim d_9 As String = "<groupName>Prueba Carlos cloud</groupName>"
        Dim d_10 As String = "<callingLineIdName>Prueba Carlos cloud</callingLineIdName>"
        Dim d_11 As String = "<timeZone>America/Santiago</timeZone>"
        Dim d_12 As String = "<contact>"
        Dim d_13 As String = "<contactName>carlos</contactName>"
        Dim d_14 As String = "<contactNumber>12345</contactNumber>"
        Dim d_15 As String = "</contact>"
        Dim d_16 As String = "<address>"
        Dim d_17 As String = "<addressLine1>Direccion de prueba 12</addressLine1>"
        Dim d_18 As String = "<city>Santiago</city>"
        Dim d_19 As String = "</address>"
        Dim d_20 As String = "</command>"

        'XML PARA ASIGNAR LOS SERVICIOS---------------------------------------------------------------------------------
        Dim e_4 As String = "<command xsi:type=" & Chr(34) & "GroupServiceAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim e_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim e_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim e_7 As String = "<serviceName>Account/Authorization Codes</serviceName>"
        Dim e_8 As String = "<serviceName>Call Capacity Management</serviceName>"
        Dim e_9 As String = "<serviceName>Call Park</serviceName>"
        Dim e_10 As String = "<serviceName>Call Pickup</serviceName>"
        Dim e_11 As String = "<serviceName>Custom Ringback Group</serviceName>"
        Dim e_12 As String = "<serviceName>Custom Ringback Group - Video</serviceName>"
        Dim e_13 As String = "<serviceName>Emergency Zones</serviceName>"
        Dim e_14 As String = "<serviceName>Enhanced Outgoing Calling Plan</serviceName>"
        Dim e_15 As String = "<serviceName>Group Paging</serviceName>"
        Dim e_16 As String = "<serviceName>Hunt Group</serviceName>"
        Dim e_17 As String = "<serviceName>Incoming Calling Plan</serviceName>"
        Dim e_18 As String = "<serviceName>Instant Group Call</serviceName>"
        Dim e_19 As String = "<serviceName>Intercept Group</serviceName>"
        Dim e_20 As String = "<serviceName>Inventory Report</serviceName>"
        Dim e_21 As String = "<serviceName>LDAP Integration</serviceName>"
        Dim e_22 As String = "<serviceName>Music On Hold</serviceName>"
        Dim e_23 As String = "<serviceName>Music On Hold - Video</serviceName>"
        Dim e_24 As String = "<serviceName>Outgoing Calling Plan</serviceName>"
        Dim e_25 As String = "<serviceName>Preferred Carrier Group</serviceName>"
        Dim e_26 As String = "<serviceName>Series Completion</serviceName>"
        Dim e_27 As String = "<serviceName>Service Scripts Group</serviceName>"
        Dim e_28 As String = "<serviceName>Voice Messaging Group</serviceName>"
        Dim e_29 As String = "</command>"

        'XML PARA ASIGNAR LA NUMERACIÓN AL GRUPO----------------------------------------------------------------------------------------
        Dim f_4 As String = "<command xsi:type=" & Chr(34) & "GroupDnAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim f_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim f_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim f_7 As String = "<phoneNumber>+56-232781566</phoneNumber>"
        Dim f_8 As String = "</command>"

        'XML PARA CREAR LOS DISPOSITIVOS---------------------------------------------------------------------------------
        Dim g_4 As String = "<command xsi:type=" & Chr(34) & "GroupAccessDeviceAddRequest14" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim g_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim g_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim g_7 As String = "<deviceName>DV_805EC0568966</deviceName>"
        Dim g_8 As String = "<deviceType>Yealink-T19xE2</deviceType>"
        Dim g_9 As String = "<protocol>SIP 2.0</protocol>"
        Dim g_10 As String = "<macAddress>805EC0568966</macAddress>"
        Dim g_11 As String = "<serialNumber>3127318120900584</serialNumber>"
        Dim g_12 As String = "<physicalLocation>ZONA_2_28</physicalLocation>"
        Dim g_13 As String = "<transportProtocol>Unspecified</transportProtocol>"
        Dim g_14 As String = "</command>"

        'XML PARA CREAR LOS DEPARTAMENTOS---------------------------------------------------------------------------------
        Dim h_4 As String = "<command xsi:type=" & Chr(34) & "GroupDepartmentAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim h_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim h_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim h_7 As String = "<departmentName>Administracion</departmentName>"
        Dim h_8 As String = "</command>"

        'XML PARA CREAR LOS USUARIOS---------------------------------------------------------------------------------
        Dim i_4 As String = "<command xsi:type=" & Chr(34) & "UserAddRequest17sp4" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim i_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim i_6 As String = "<groupId>AUTOPRO_cloudpbx</groupId>"
        Dim i_7 As String = "<userId>226337160@autopro.cl</userId>"
        Dim i_8 As String = "<lastName>GONZALEZ</lastName>"
        Dim i_9 As String = "<firstName>GLADIS</firstName>"
        Dim i_10 As String = "<callingLineIdLastName>GONZALEZ</callingLineIdLastName>"
        Dim i_11 As String = "<callingLineIdFirstName>GLADIS</callingLineIdFirstName>"
        Dim i_12 As String = "<password>a1234567</password>"
        Dim i_13 As String = "<department xsi:type=" & Chr(34) & "GroupDepartmentKey" & Chr(34) & ">"
        Dim i_14 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim i_15 As String = "<groupId>AUTOPRO_cloudpbx</groupId>"
        Dim i_16 As String = "<name>RECEPCION</name>"
        Dim i_17 As String = "</department>"
        Dim i_18 As String = "<language>Spanish</language>"
        Dim i_19 As String = "<timeZone>Chile/Continental</timeZone>"
        Dim i_20 As String = "<emailAddress>GGONZALEZ@PROSAL.CL</emailAddress>"
        Dim i_21 As String = "<address>"
        Dim i_22 As String = "<addressLine1>MONEDA 2387 PISO 1 , SANTIAGO</addressLine1>"
        Dim i_23 As String = "<city>Santiago</city>"
        Dim i_24 As String = "</address>"
        Dim i_25 As String = "</command>"

        'XML PARA EL PROXY-----------------------------------------------------------------------------------------------
        Dim j_4 As String = "<command xsi:type=" & Chr(34) & "GroupAccessDeviceCustomTagAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim j_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim j_6 As String = "<groupId>AUTOPRO_cloudpbx</groupId>"
        Dim j_7 As String = "<deviceName>DV_805EC02EC440</deviceName>"
        Dim j_8 As String = "<tagName>%SBC_ADDRESS%</tagName>"
        Dim j_9 As String = "<tagValue>172.24.16.211</tagValue>"
        Dim j_10 As String = "</command>"

        'XML PARA ASIGNAR DISPOSITIVOS A USUARIOS---------------------------------------------------------------------
        Dim k_4 As String = "<command xsi:type=" & Chr(34) & "UserModifyRequest17sp4" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim k_5 As String = "<userId>226337160@autopro.cl</userId>"
        Dim k_6 As String = "<phoneNumber>226337160</phoneNumber>"
        Dim k_7 As String = "<extension>7160</extension>"
        Dim k_8 As String = "<sipAliasList xsi:nil=" & Chr(34) & "true" & Chr(34) & "/>"
        Dim k_9 As String = "<endpoint>"
        Dim k_10 As String = "<accessDeviceEndpoint>"
        Dim k_11 As String = "<accessDevice>"
        Dim k_12 As String = "<deviceLevel>Group</deviceLevel>"
        Dim k_13 As String = "<deviceName>DV_805EC02EC440</deviceName>"
        Dim k_14 As String = "</accessDevice>"
        Dim k_15 As String = "<linePort>226337160@autopro.cl</linePort>"
        Dim k_16 As String = "<contactList xsi:nil=" & Chr(34) & "true" & Chr(34) & "/>"
        Dim k_17 As String = "</accessDeviceEndpoint>"
        Dim k_18 As String = "</endpoint>"
        Dim k_19 As String = "</command>"

        'XML PARA ASIGNAR PACK DE SERVICIOS---------------------------------------------------------------------
        Dim l_4 As String = "<command xsi:type=" & Chr(34) & "UserServiceAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim l_5 As String = "<userId>226337160@autopro.cl</userId>"
        Dim l_6 As String = "<servicePackName>Pack_Basico</servicePackName>"
        Dim l_7 As String = "</command>"

        'XML PARA OCP OUTGOING-CALLING-PLAN------------------------------------------------------------------------
        Dim m_4 As String = "<command xsi:type=" & Chr(34) & "UserOutgoingCallingPlanOriginatingModifyRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim m_5 As String = "<userId>226337160@autopro.cl</userId>"
        Dim m_6 As String = "<useCustomSettings>true</useCustomSettings>"
        Dim m_7 As String = "<userPermissions>"
        Dim m_8 As String = "<group>Allow</group>"
        Dim m_9 As String = "<local>Allow</local>"
        Dim m_10 As String = "<tollFree>Allow</tollFree>"
        Dim m_11 As String = "<toll>Allow</toll>"
        Dim m_12 As String = "<international>Disallow</international>"
        Dim m_13 As String = "<operatorAssisted>Allow</operatorAssisted>"
        Dim m_14 As String = "<chargeableDirectoryAssisted>Allow</chargeableDirectoryAssisted>"
        Dim m_15 As String = "<specialServicesI>Allow</specialServicesI>"
        Dim m_16 As String = "<specialServicesII>Allow</specialServicesII>"
        Dim m_17 As String = "<premiumServicesI>Disallow</premiumServicesI>"
        Dim m_18 As String = "<premiumServicesII>Disallow</premiumServicesII>"
        Dim m_19 As String = "<casual>Disallow</casual>"
        Dim m_20 As String = "<urlDialing>Allow</urlDialing>"
        Dim m_21 As String = "<unknown>Allow</unknown>"
        Dim m_22 As String = "</userPermissions>"
        Dim m_23 As String = "</command>"

        'XML PARA ASIGNAR CONTRASEÑA SIP------------------------------------------------------------------------
        Dim n_4 As String = "<command xsi:type=" & Chr(34) & "UserAuthenticationModifyRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim n_5 As String = "<userId>226337160@autopro.cl</userId>"
        Dim n_6 As String = "<userName>226337160</userName>"
        Dim n_7 As String = "<newPassword>XXXXX</newPassword>"
        Dim n_8 As String = "</command>"

        'XML PARA ACTIVAR LOS NUMEROS------------------------------------------------------------------------
        Dim o_4 As String = "<command xsi:type=" & Chr(34) & "GroupDnActivateListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim o_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim o_6 As String = "<groupId>AUTOPRO_cloudpbx</groupId>"
        Dim o_7 As String = "<phoneNumber>+56-226337160</phoneNumber>"
        Dim o_8 As String = "</command>"

        'FASE 2

        'XML PARA MODIFICAR EL LARGO DE LAS EXTENSIONES DE GRUPO--------------------------------------------------------------
        Dim p_4 As String = "<command xsi:type=" & Chr(34) & "GroupExtensionLengthModifyRequest17" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim p_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim p_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim p_7 As String = "<minExtensionLength>2</minExtensionLength>"
        Dim p_8 As String = "<maxExtensionLength>4</maxExtensionLength>"
        Dim p_9 As String = "<defaultExtensionLength>4</defaultExtensionLength>"
        Dim p_10 As String = "</command>"

        'XML PARA ACTIVAR LA MUSICA EN ESPERA DEL GRUPO--------------------------------------------------------------/
        Dim q_4 As String = "<command xsi:type=" & Chr(34) & "GroupMusicOnHoldModifyInstanceRequest20" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim q_5 As String = " <serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim q_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim q_7 As String = "<isActiveDuringCallHold>true</isActiveDuringCallHold>"
        Dim q_8 As String = "<isActiveDuringCallPark>true</isActiveDuringCallPark>"
        Dim q_9 As String = "<isActiveDuringBusyCampOn>true</isActiveDuringBusyCampOn>"
        Dim q_10 As String = "<source>"
        Dim q_11 As String = "<audioFilePreferredCodec>None</audioFilePreferredCodec>"
        Dim q_12 As String = "<messageSourceSelection>System</messageSourceSelection>"
        Dim q_13 As String = "<customSource>"
        Dim q_14 As String = "<audioFile xsi:nil=" & Chr(34) & "true" & Chr(34) & "/>"
        Dim q_15 As String = "<videoFile xsi:nil=" & Chr(34) & "true" & Chr(34) & "/>"
        Dim q_16 As String = "</customSource>"
        Dim q_17 As String = "<externalSource>"
        Dim q_18 As String = "<accessDeviceEndpoint xsi:nil=" & Chr(34) & "true" & Chr(34) & "/>"
        Dim q_19 As String = "</externalSource>"
        Dim q_20 As String = "</source>"
        Dim q_21 As String = "</command>"

        Dim finalLine As String = "</BroadsoftDocument>"

        ProgressBar1.Minimum = 0
        ProgressBar1.Value = 0
        ProgressBar1.Maximum = 100

        'SearchAllSubDirectories
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblPathTmpCloud, FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(foundFile)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los archivos antiguos de la carpeta " & gblPathTmpCloud &
                   ", verifique que los archivos no esten siendo utilizados por otro proceso", MsgBoxStyle.Exclamation, "Error al eliminar archivos")
            Me.Cursor = Cursors.Default
            btn_procesar.Enabled = True
            btn_browse_CSV.Enabled = True
            btn_validate_data.Enabled = True
            Exit Sub
        End Try

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim estadoArchivo As Integer = 0
        Dim multipleInputFile As String = gblPathTmpCloud & "\multipleInputFile.txt"
        Dim lineConfigFile As String = ""

        Try
            FileOpen(1, multipleInputFile, OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            FileClose(1)
            Me.Cursor = Cursors.Default
            btn_procesar.Enabled = True
            btn_browse_CSV.Enabled = True
            btn_validate_data.Enabled = True
            Exit Sub
        End Try

        'XML PARA CREAR UN DOMINIO-----------------------------------------------------------------------------------------------------------
        Try
            numFile = 2
            indexXML_Cloud += 1
            fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_CreateDomain_request.xml"
            fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, line1.ToCharArray)
            WriteLine(numFile, line2.ToCharArray)
            WriteLine(numFile, line3.ToCharArray)
            WriteLine(numFile, a_4.ToCharArray)
            a_5 = "<domain>" & domain & "</domain>"
            WriteLine(numFile, a_5.ToCharArray)
            WriteLine(numFile, a_6.ToCharArray)
            WriteLine(numFile, finalLine.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            WriteLine(1, lineConfigFile.ToCharArray)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
            In_Case_Error1()
            Exit Sub
        End Try
        estadoArchivo = 1

        'XML PARA ASIGNAR EL DOMINIO CREADO----------------------------------------------------------------------
        If estadoArchivo = 1 Then
            Try
                numFile = 2
                indexXML_Cloud += 1
                fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_AssignDomain_request.xml"
                fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, line1.ToCharArray)
                WriteLine(numFile, line2.ToCharArray)
                WriteLine(numFile, line3.ToCharArray)
                WriteLine(numFile, b_4.ToCharArray)
                WriteLine(numFile, b_5.ToCharArray)
                b_6 = "<domain>" & domain & "</domain>"
                WriteLine(numFile, b_6.ToCharArray)
                WriteLine(numFile, b_7.ToCharArray)
                WriteLine(numFile, finalLine.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 2
        End If

        ''XML PARA CREAR NUMERACIÓN------------------------------------------------------------------------------
        'If estadoArchivo = 2 Then
        '    Try
        '        numFile = 2
        '        indexXML_Cloud += 1
        '        fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_CreateNumbers_request.xml"
        '        fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
        '        FileOpen(numFile, fileIXML, OpenMode.Output)
        '        WriteLine(numFile, line1.ToCharArray)
        '        WriteLine(numFile, line2.ToCharArray)
        '        WriteLine(numFile, line3.ToCharArray)
        '        WriteLine(numFile, c_4.ToCharArray)
        '        WriteLine(numFile, c_5.ToCharArray)
        '        For j = 0 To DataGridView1.Rows.Count - 2
        '            phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
        '            c_6 = "<phoneNumber>" & phoneNumber & "</phoneNumber>"
        '            WriteLine(numFile, c_6.ToCharArray)
        '        Next
        '        WriteLine(numFile, c_7.ToCharArray)
        '        WriteLine(numFile, finalLine.ToCharArray)
        '        FileClose(numFile)
        '        lineConfigFile = fileIXML & ";" & fileOXML
        '        WriteLine(1, lineConfigFile.ToCharArray)
        '    Catch ex As Exception
        '        MsgBox(ex.ToString)
        '        MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
        '        In_Case_Error1()
        '        Exit Sub
        '    End Try
        '    estadoArchivo = 3
        'End If


        'XML PARA CREAR NUMERACIÓN------------------------------------------------------------------------------
        If estadoArchivo = 2 Then
            Try
                For j = 0 To DataGridView1.Rows.Count - 2
                    numFile = 2
                    indexXML_Cloud += 1
                    fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_CreateNumbers_request.xml"
                    fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, c_4.ToCharArray)
                    WriteLine(numFile, c_5.ToCharArray)
                    phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
                    c_6 = "<phoneNumber>" & phoneNumber & "</phoneNumber>"
                    WriteLine(numFile, c_6.ToCharArray)
                    WriteLine(numFile, c_7.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 3
        End If

        'XML PARA CREAR PERFIL DE GRUPO-------------------------------------------------------------------------
        If estadoArchivo = 3 Then
            Try
                numFile = 2
                indexXML_Cloud += 1
                fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_CreateProfileGroup_request.xml"
                fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, line1.ToCharArray)
                WriteLine(numFile, line2.ToCharArray)
                WriteLine(numFile, line3.ToCharArray)
                WriteLine(numFile, d_4.ToCharArray)
                WriteLine(numFile, d_5.ToCharArray)
                d_6 = "<groupId>" & group_id & "</groupId>"
                WriteLine(numFile, d_6.ToCharArray)
                d_7 = "<defaultDomain>" & domain & "</defaultDomain>"
                WriteLine(numFile, d_7.ToCharArray)

                Dim limit As Integer = DataGridView1.Rows.Count

                If limit < 25 Then
                    d_8 = "<userLimit>25</userLimit>"
                ElseIf limit >= 25 And limit < 50 Then
                    d_8 = "<userLimit>50</userLimit>"
                ElseIf limit >= 50 And limit < 100 Then
                    d_8 = "<userLimit>100</userLimit>"
                ElseIf limit >= 100 And limit < 200 Then
                    d_8 = "<userLimit>200</userLimit>"
                Else
                    d_8 = "<userLimit>1000</userLimit>"
                End If
                WriteLine(numFile, d_8.ToCharArray)

                d_9 = "<groupName>" & group_name & "</groupName>"
                WriteLine(numFile, d_9.ToCharArray)
                d_10 = "<callingLineIdName>" & group_name & "</callingLineIdName>"
                WriteLine(numFile, d_10.ToCharArray)
                'WriteLine(numFile, d_11.ToCharArray) "<timeZone>America/Santiago</timeZone>"
                If infoContact = 1 Then
                    WriteLine(numFile, d_12.ToCharArray)
                    d_13 = "<contactName>" & contact_name & "</contactName>"
                    WriteLine(numFile, d_13.ToCharArray)
                    d_14 = "<contactNumber>" & contact_number & "</contactNumber>"
                    WriteLine(numFile, d_14.ToCharArray)
                    WriteLine(numFile, d_15.ToCharArray)
                End If
                WriteLine(numFile, d_16.ToCharArray)
                d_17 = "<addressLine1>" & address & "</addressLine1>"
                WriteLine(numFile, d_17.ToCharArray)
                d_18 = "<city>" & city & "</city>"
                WriteLine(numFile, d_18.ToCharArray)
                WriteLine(numFile, d_19.ToCharArray)
                WriteLine(numFile, d_20.ToCharArray)
                WriteLine(numFile, finalLine.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 4
        End If

        'XML PARA MODIFICAR EL LARGO DE LAS EXTENSIONES DE GRUPO--------------------------------------------------------------
        If estadoArchivo = 4 Then
            Try
                numFile = 2
                indexXML_Cloud += 1
                fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_ExtensionsLength_request.xml"
                fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, line1.ToCharArray)
                WriteLine(numFile, line2.ToCharArray)
                WriteLine(numFile, line3.ToCharArray)
                WriteLine(numFile, p_4.ToCharArray)
                WriteLine(numFile, p_5.ToCharArray)
                p_6 = "<groupId>" & group_id & "</groupId>"
                WriteLine(numFile, p_6.ToCharArray)
                WriteLine(numFile, p_7.ToCharArray)
                WriteLine(numFile, p_8.ToCharArray)
                WriteLine(numFile, p_9.ToCharArray)
                WriteLine(numFile, p_10.ToCharArray)
                WriteLine(numFile, finalLine.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 5
        End If

        'XML PARA SELECCIONAR SERVICIOS DE GRUPO (ARCHIVO EXTERNO)--------------------------------------------------------------
        indexXML_Cloud += 1
        If estadoArchivo = 5 Then
            Try
                'Lee un archivo, modifica la linea 6
                Dim Lines_Array() As String = IO.File.ReadAllLines(gblPathAppl & "\servicesFile_cloud\" & "5_SelectServices_request.xml")
                Lines_Array(5) = " <groupId>" & group_id & "</groupId>"

                'Se reescribe el archivo con la linea 6 ya editada
                IO.File.WriteAllLines(gblPathAppl & "\servicesFile_cloud\" & indexXML_Cloud & "_SelectServices_request.xml", Lines_Array)

                fileIXML = gblPathAppl & "\servicesFile_cloud\" & indexXML_Cloud & "_SelectServices_request.xml"
                fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al modificar el archivo " & gblPathAppl & "\servicesFile_cloud\" & indexXML_Cloud & "_SelectServices_request.xml", MsgBoxStyle.Exclamation, "Error al crear el archivo")
                FileClose(1)
                Me.Cursor = Cursors.Default
                btn_browse_CSV.Enabled = True
                btn_procesar.Enabled = True
                btn_validate_data.Enabled = True
                Exit Sub
            End Try
            estadoArchivo = 6
        End If

        'XML PARA ASIGNAR LOS SERVICIOS---------------------------------------------------------------------------------
        If estadoArchivo = 6 Then
            Try
                numFile = 2
                indexXML_Cloud += 1
                fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_AssignServices_request.xml"
                fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, line1.ToCharArray)
                WriteLine(numFile, line2.ToCharArray)
                WriteLine(numFile, line3.ToCharArray)
                WriteLine(numFile, e_4.ToCharArray)
                WriteLine(numFile, e_5.ToCharArray)
                e_6 = "<groupId>" & group_id & "</groupId>"
                WriteLine(numFile, e_6.ToCharArray)
                WriteLine(numFile, e_7.ToCharArray)
                WriteLine(numFile, e_8.ToCharArray)
                WriteLine(numFile, e_9.ToCharArray)
                WriteLine(numFile, e_10.ToCharArray)
                WriteLine(numFile, e_11.ToCharArray)
                WriteLine(numFile, e_12.ToCharArray)
                WriteLine(numFile, e_13.ToCharArray)
                WriteLine(numFile, e_14.ToCharArray)
                WriteLine(numFile, e_15.ToCharArray)
                WriteLine(numFile, e_16.ToCharArray)
                WriteLine(numFile, e_17.ToCharArray)
                WriteLine(numFile, e_18.ToCharArray)
                WriteLine(numFile, e_19.ToCharArray)
                WriteLine(numFile, e_20.ToCharArray)
                WriteLine(numFile, e_21.ToCharArray)
                WriteLine(numFile, e_22.ToCharArray)
                WriteLine(numFile, e_23.ToCharArray)
                WriteLine(numFile, e_24.ToCharArray)
                WriteLine(numFile, e_25.ToCharArray)
                WriteLine(numFile, e_26.ToCharArray)
                WriteLine(numFile, e_27.ToCharArray)
                WriteLine(numFile, e_28.ToCharArray)
                WriteLine(numFile, e_29.ToCharArray)
                WriteLine(numFile, finalLine.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 7
        End If

        ''XML PARA ASIGNAR LA NUMERACIÓN AL GRUPO----------------------------------------------------------------------------------------
        'If estadoArchivo = 7 Then
        '    Try
        '        numFile = 2
        '        indexXML_Cloud += 1
        '        fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_AssignNumber_request.xml"
        '        fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
        '        FileOpen(numFile, fileIXML, OpenMode.Output)
        '        WriteLine(numFile, line1.ToCharArray)
        '        WriteLine(numFile, line2.ToCharArray)
        '        WriteLine(numFile, line3.ToCharArray)
        '        WriteLine(numFile, f_4.ToCharArray)
        '        WriteLine(numFile, f_5.ToCharArray)
        '        f_6 = "<groupId>" & group_id & "</groupId>"
        '        WriteLine(numFile, f_6.ToCharArray)
        '        For j = 0 To DataGridView1.Rows.Count - 2
        '            phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
        '            f_7 = "<phoneNumber>+56-" & phoneNumber & "</phoneNumber>"
        '            WriteLine(numFile, f_7.ToCharArray)
        '        Next
        '        WriteLine(numFile, f_8.ToCharArray)
        '        WriteLine(numFile, finalLine.ToCharArray)
        '        FileClose(numFile)
        '        lineConfigFile = fileIXML & ";" & fileOXML
        '        WriteLine(1, lineConfigFile.ToCharArray)
        '    Catch ex As Exception
        '        MsgBox(ex.ToString)
        '        MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
        '        In_Case_Error1()
        '        Exit Sub
        '    End Try
        '    estadoArchivo = 8
        'End If

        'XML PARA ASIGNAR LA NUMERACIÓN AL GRUPO----------------------------------------------------------------------------------------
        If estadoArchivo = 7 Then
            Try
                For j = 0 To DataGridView1.Rows.Count - 2
                    numFile = 2
                    indexXML_Cloud += 1
                    fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_AssignNumber_request.xml"
                    fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, f_4.ToCharArray)
                    WriteLine(numFile, f_5.ToCharArray)
                    f_6 = "<groupId>" & group_id & "</groupId>"
                    WriteLine(numFile, f_6.ToCharArray)
                    phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
                    f_7 = "<phoneNumber>+56-" & phoneNumber & "</phoneNumber>"
                    WriteLine(numFile, f_7.ToCharArray)
                    WriteLine(numFile, f_8.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 8
        End If

        'XML PARA CREAR LOS DISPOSITIVOS---------------------------------------------------------------------------------
        If estadoArchivo = 8 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile = 2
                    indexXML_Cloud += 1
                    fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_CreateDevice_request.xml"
                    fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, g_4.ToCharArray)
                    WriteLine(numFile, g_5.ToCharArray)
                    g_6 = "<groupId>" & group_id & "</groupId>"
                    WriteLine(numFile, g_6.ToCharArray)
                    mac = DataGridView1.Rows(j).Cells(9).Value.ToString
                    g_7 = "<deviceName>DV_" & mac & "</deviceName>"
                    WriteLine(numFile, g_7.ToCharArray)
                    device_type = DataGridView1.Rows(j).Cells(8).Value.ToString
                    g_8 = "<deviceType>" & device_type & "</deviceType>"
                    WriteLine(numFile, g_8.ToCharArray)
                    WriteLine(numFile, g_9.ToCharArray)
                    g_10 = "<macAddress>" & mac & "</macAddress>"
                    WriteLine(numFile, g_10.ToCharArray)
                    serial_number = DataGridView1.Rows(j).Cells(10).Value.ToString
                    g_11 = "<serialNumber>" & serial_number & "</serialNumber>"
                    WriteLine(numFile, g_11.ToCharArray)
                    physical_location = DataGridView1.Rows(j).Cells(11).Value.ToString
                    g_12 = "<physicalLocation>" & physical_location & "</physicalLocation>"
                    WriteLine(numFile, g_12.ToCharArray)
                    WriteLine(numFile, g_13.ToCharArray)
                    WriteLine(numFile, g_14.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 9
        End If

        'XML PARA CREAR LOS DEPARTAMENTOS---------------------------------------------------------------------------------
        If estadoArchivo = 9 Then
            Try
                For j = 0 To arregloDeptos.Length - 1
                    numFile = 2
                    indexXML_Cloud += 1
                    fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_CreateDepartment_request.xml"
                    fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, h_4.ToCharArray)
                    WriteLine(numFile, h_5.ToCharArray)
                    h_6 = "<groupId>" & group_id & "</groupId>"
                    WriteLine(numFile, h_6.ToCharArray)
                    department = arregloDeptos(j).ToString
                    h_7 = "<departmentName>" & department & "</departmentName>"
                    WriteLine(numFile, h_7.ToCharArray)
                    WriteLine(numFile, h_8.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 10
        End If

        'XML PARA CREAR LOS USUARIOS---------------------------------------------------------------------------------
        If estadoArchivo = 10 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile = 2
                    indexXML_Cloud += 1
                    fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_CreateUser_request.xml"
                    fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, i_4.ToCharArray)
                    WriteLine(numFile, i_5.ToCharArray)
                    i_6 = "<groupId>" & group_id & "</groupId>"
                    WriteLine(numFile, i_6.ToCharArray)
                    phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
                    i_7 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
                    WriteLine(numFile, i_7.ToCharArray)
                    last_name = DataGridView1.Rows(j).Cells(14).Value.ToString
                    i_8 = "<lastName>" & last_name & "</lastName>"
                    WriteLine(numFile, i_8.ToCharArray)
                    first_name = DataGridView1.Rows(j).Cells(13).Value.ToString
                    i_9 = "<firstName>" & first_name & "</firstName>"
                    WriteLine(numFile, i_9.ToCharArray)
                    i_10 = "<callingLineIdLastName>" & last_name & "</callingLineIdLastName>"
                    WriteLine(numFile, i_10.ToCharArray)
                    i_11 = "<callingLineIdFirstName>" & first_name & "</callingLineIdFirstName>"
                    WriteLine(numFile, i_11.ToCharArray)
                    WriteLine(numFile, i_12.ToCharArray)
                    department = DataGridView1.Rows(j).Cells(12).Value.ToString
                    If department.Length > 0 Then
                        WriteLine(numFile, i_13.ToCharArray)
                        WriteLine(numFile, i_14.ToCharArray)
                        i_15 = "<groupId>" & group_id & "</groupId>"
                        WriteLine(numFile, i_15.ToCharArray)
                        i_16 = "<name>" & department & "</name>"
                        WriteLine(numFile, i_16.ToCharArray)
                        WriteLine(numFile, i_17.ToCharArray)
                    End If
                    WriteLine(numFile, i_18.ToCharArray)
                    WriteLine(numFile, i_19.ToCharArray)
                    user_email = DataGridView1.Rows(j).Cells(15).Value.ToString
                    If user_email.Length > 0 Then
                        i_20 = "<emailAddress>" & user_email & "</emailAddress>"
                        WriteLine(numFile, i_20.ToCharArray)
                    End If
                    WriteLine(numFile, i_21.ToCharArray)
                    user_address = DataGridView1.Rows(j).Cells(16).Value.ToString
                    i_22 = "<addressLine1>" & user_address & "</addressLine1>"
                    WriteLine(numFile, i_22.ToCharArray)
                    user_city = DataGridView1.Rows(j).Cells(17).Value.ToString
                    i_23 = "<city>" & user_city & "</city>"
                    WriteLine(numFile, i_23.ToCharArray)
                    WriteLine(numFile, i_24.ToCharArray)
                    WriteLine(numFile, i_25.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 11
        End If

        'XML PARA EL PROXY-----------------------------------------------------------------------------------------------
        If estadoArchivo = 11 Then
            Try
                If proxyInfo = 1 Then
                    For j = 0 To filasValidas - 1
                        numFile = 2
                        indexXML_Cloud += 1
                        fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_CreateProxy_request.xml"
                        fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                        FileOpen(numFile, fileIXML, OpenMode.Output)
                        WriteLine(numFile, line1.ToCharArray)
                        WriteLine(numFile, line2.ToCharArray)
                        WriteLine(numFile, line3.ToCharArray)
                        WriteLine(numFile, j_4.ToCharArray)
                        WriteLine(numFile, j_5.ToCharArray)
                        j_6 = "<groupId>" & group_id & "</groupId>"
                        WriteLine(numFile, j_6.ToCharArray)
                        mac = DataGridView1.Rows(j).Cells(9).Value.ToString
                        j_7 = "<deviceName>DV_" & mac & "</deviceName>"
                        WriteLine(numFile, j_7.ToCharArray)
                        WriteLine(numFile, j_8.ToCharArray)
                        j_9 = "<tagValue>" & proxy & "</tagValue>"
                        WriteLine(numFile, j_9.ToCharArray)
                        WriteLine(numFile, j_10.ToCharArray)
                        WriteLine(numFile, finalLine.ToCharArray)
                        FileClose(numFile)
                        lineConfigFile = fileIXML & ";" & fileOXML
                        WriteLine(1, lineConfigFile.ToCharArray)
                    Next
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 12
        End If

        'XML PARA ASIGNAR DISPOSITIVOS A USUARIOS---------------------------------------------------------------------
        If estadoArchivo = 12 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile = 2
                    indexXML_Cloud += 1
                    fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_AssignUser_request.xml"
                    fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, k_4.ToCharArray)
                    phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
                    k_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
                    WriteLine(numFile, k_5.ToCharArray)
                    k_6 = "<phoneNumber>" & phoneNumber & "</phoneNumber>"
                    WriteLine(numFile, k_6.ToCharArray)
                    extensions = DataGridView1.Rows(j).Cells(19).Value.ToString
                    k_7 = "<extension>" & extensions & "</extension>"
                    WriteLine(numFile, k_7.ToCharArray)
                    WriteLine(numFile, k_8.ToCharArray)
                    WriteLine(numFile, k_9.ToCharArray)
                    WriteLine(numFile, k_10.ToCharArray)
                    WriteLine(numFile, k_11.ToCharArray)
                    WriteLine(numFile, k_12.ToCharArray)
                    mac = DataGridView1.Rows(j).Cells(9).Value.ToString
                    k_13 = "<deviceName>DV_" & mac & "</deviceName>"
                    WriteLine(numFile, k_13.ToCharArray)
                    WriteLine(numFile, k_14.ToCharArray)
                    k_15 = "<linePort>" & phoneNumber & "@" & domain & "</linePort>"
                    WriteLine(numFile, k_15.ToCharArray)
                    WriteLine(numFile, k_16.ToCharArray)
                    WriteLine(numFile, k_17.ToCharArray)
                    WriteLine(numFile, k_18.ToCharArray)
                    WriteLine(numFile, k_19.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 13
        End If

        'XML PARA ASIGNAR PACK DE SERVICIOS---------------------------------------------------------------------
        If estadoArchivo = 13 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile = 2
                    indexXML_Cloud += 1
                    fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_AssignServices_request.xml"
                    fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, l_4.ToCharArray)
                    phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
                    l_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
                    WriteLine(numFile, l_5.ToCharArray)
                    device_type = DataGridView1.Rows(j).Cells(8).Value.ToString
                    If device_type = "Yealink-T19xE2" Then
                        l_6 = "<servicePackName>Pack_Basico</servicePackName>"
                    ElseIf device_type = "Yealink-T21xE2" Then
                        l_6 = "<servicePackName>Pack_Estandar</servicePackName>"
                    ElseIf device_type = "Yealink-T27G" Then
                        l_6 = "<servicePackName>Pack_Avanzado</servicePackName>"
                    End If
                    WriteLine(numFile, l_6.ToCharArray)
                    WriteLine(numFile, l_7.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 14
        End If

        'XML PARA OCP OUTGOING-CALLING-PLAN------------------------------------------------------------------------
        If estadoArchivo = 14 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile = 2
                    indexXML_Cloud += 1
                    fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_OCP_request.xml"
                    fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, m_4.ToCharArray)
                    phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
                    m_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
                    WriteLine(numFile, m_5.ToCharArray)
                    WriteLine(numFile, m_6.ToCharArray)
                    WriteLine(numFile, m_7.ToCharArray)
                    WriteLine(numFile, m_8.ToCharArray)

                    ocp_local = DataGridView1.Rows(j).Cells(20).Value.ToString
                    If ocp_local = "bloqueado" Or ocp_local = "Bloqueado" Or ocp_local = "BLOQUEADO" Then
                        m_9 = "<local>Disallow</local>"
                    ElseIf ocp_local = "desbloqueado" Or ocp_local = "Desbloqueado" Or ocp_local = "DESBLOQUEADO" Then
                        m_9 = "<local>Allow</local>"
                    End If
                    WriteLine(numFile, m_9.ToCharArray)

                    ocp_tollFree = DataGridView1.Rows(j).Cells(21).Value.ToString
                    If ocp_tollFree = "bloqueado" Or ocp_tollFree = "Bloqueado" Or ocp_tollFree = "BLOQUEADO" Then
                        m_10 = "<tollFree>Disallow</tollFree>"
                    ElseIf ocp_tollFree = "desbloqueado" Or ocp_tollFree = "Desbloqueado" Or ocp_tollFree = "DESBLOQUEADO" Then
                        m_10 = "<tollFree>Allow</tollFree>"
                    End If
                    WriteLine(numFile, m_10.ToCharArray)
                    WriteLine(numFile, m_11.ToCharArray)

                    ocp_internacional = DataGridView1.Rows(j).Cells(22).Value.ToString
                    If ocp_internacional = "bloqueado" Or ocp_internacional = "Bloqueado" Or ocp_internacional = "BLOQUEADO" Then
                        m_12 = "<international>Disallow</international>"
                    ElseIf ocp_internacional = "desbloqueado" Or ocp_internacional = "Desbloqueado" Or ocp_internacional = "DESBLOQUEADO" Then
                        m_12 = "<international>Allow</international>"
                    End If
                    WriteLine(numFile, m_12.ToCharArray)
                    WriteLine(numFile, m_13.ToCharArray)
                    WriteLine(numFile, m_14.ToCharArray)

                    ocp_special1 = DataGridView1.Rows(j).Cells(23).Value.ToString
                    If ocp_special1 = "bloqueado" Or ocp_special1 = "Bloqueado" Or ocp_special1 = "BLOQUEADO" Then
                        m_15 = "<specialServicesI>Disallow</specialServicesI>"
                    ElseIf ocp_special1 = "desbloqueado" Or ocp_special1 = "Desbloqueado" Or ocp_special1 = "DESBLOQUEADO" Then
                        m_15 = "<specialServicesI>Allow</specialServicesI>"
                    End If
                    WriteLine(numFile, m_15.ToCharArray)

                    ocp_special2 = DataGridView1.Rows(j).Cells(24).Value.ToString
                    If ocp_special2 = "bloqueado" Or ocp_special2 = "Bloqueado" Or ocp_special2 = "BLOQUEADO" Then
                        m_16 = "<specialServicesII>Disallow</specialServicesII>"
                    ElseIf ocp_special2 = "desbloqueado" Or ocp_special2 = "Desbloqueado" Or ocp_special2 = "DESBLOQUEADO" Then
                        m_16 = "<specialServicesII>Allow</specialServicesII>"
                    End If
                    WriteLine(numFile, m_16.ToCharArray)

                    ocp_premium1 = DataGridView1.Rows(j).Cells(25).Value.ToString
                    If ocp_premium1 = "bloqueado" Or ocp_premium1 = "Bloqueado" Or ocp_premium1 = "BLOQUEADO" Then
                        m_17 = "<premiumServicesI>Disallow</premiumServicesI>"
                    ElseIf ocp_premium1 = "desbloqueado" Or ocp_premium1 = "Desbloqueado" Or ocp_premium1 = "DESBLOQUEADO" Then
                        m_17 = "<premiumServicesI>Allow</premiumServicesI>"
                    End If
                    WriteLine(numFile, m_17.ToCharArray)
                    WriteLine(numFile, m_18.ToCharArray)
                    WriteLine(numFile, m_19.ToCharArray)
                    WriteLine(numFile, m_20.ToCharArray)
                    WriteLine(numFile, m_21.ToCharArray)
                    WriteLine(numFile, m_22.ToCharArray)
                    WriteLine(numFile, m_23.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 15
        End If

        'XML PARA ASIGNAR CONTRASEÑA SIP------------------------------------------------------------------------
        If estadoArchivo = 15 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile = 2
                    indexXML_Cloud += 1
                    fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_AssignPasswordSIP_request.xml"
                    fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, n_4.ToCharArray)
                    phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
                    n_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
                    WriteLine(numFile, n_5.ToCharArray)
                    n_6 = "<userName>" & phoneNumber & "</userName>"
                    WriteLine(numFile, n_6.ToCharArray)
                    'Dim domi As String = Mid(domain.ToString, 0, 4)
                    Dim letras As String = group_id.Substring(0, 4)
                    n_7 = "<newPassword>" & letras & phoneNumber & "</newPassword>"
                    WriteLine(numFile, n_7.ToCharArray)
                    WriteLine(numFile, n_8.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 16
        End If

        'XML PARA ACTIVAR LOS NUMEROS------------------------------------------------------------------------
        If estadoArchivo = 16 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile = 2
                    indexXML_Cloud += 1
                    fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_ActivateNumber_request.xml"
                    fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, o_4.ToCharArray)
                    WriteLine(numFile, o_5.ToCharArray)
                    o_6 = "<groupId>" & group_id & "</groupId>"
                    WriteLine(numFile, o_6.ToCharArray)
                    phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
                    o_7 = "<phoneNumber>+56-" & phoneNumber & "</phoneNumber>"
                    WriteLine(numFile, o_7.ToCharArray)
                    WriteLine(numFile, o_8.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
            estadoArchivo = 17
        End If

        'XML PARA ACTIVAR LA MUSICA EN ESPERA DEL GRUPO--------------------------------------------------------------
        If estadoArchivo = 17 Then
            Try
                numFile = 2
                indexXML_Cloud += 1
                fileIXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_GroupMusicOnHold_request.xml"
                fileOXML = gblPathTmpCloud & "\" & indexXML_Cloud & "_cloudpbx_response.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, line1.ToCharArray)
                WriteLine(numFile, line2.ToCharArray)
                WriteLine(numFile, line3.ToCharArray)
                WriteLine(numFile, q_4.ToCharArray)
                WriteLine(numFile, q_5.ToCharArray)
                q_6 = "<groupId>" & group_id & "</groupId>"
                WriteLine(numFile, q_6.ToCharArray)
                WriteLine(numFile, q_7.ToCharArray)
                WriteLine(numFile, q_8.ToCharArray)
                WriteLine(numFile, q_9.ToCharArray)
                WriteLine(numFile, q_10.ToCharArray)
                WriteLine(numFile, q_11.ToCharArray)
                WriteLine(numFile, q_12.ToCharArray)
                WriteLine(numFile, q_13.ToCharArray)
                WriteLine(numFile, q_14.ToCharArray)
                WriteLine(numFile, q_15.ToCharArray)
                WriteLine(numFile, q_16.ToCharArray)
                WriteLine(numFile, q_17.ToCharArray)
                WriteLine(numFile, q_18.ToCharArray)
                WriteLine(numFile, q_19.ToCharArray)
                WriteLine(numFile, q_20.ToCharArray)
                WriteLine(numFile, q_21.ToCharArray)
                WriteLine(numFile, finalLine.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
                In_Case_Error1()
                Exit Sub
            End Try
        End If

        FileClose(1)

        lbl_state_cloud.Text = "Processing XML Files..."
        ProgressBar1.Value = ProgressBar1.Value = 30
        My.Application.DoEvents()

        ExecuteShellBulk(multipleInputFile, 1)
        If codError = 0 Then
            ParseXML_cloudPBX()
        End If
    End Sub

    Private Sub In_Case_Error2()
        btn_procesar.Enabled = True
        btn_validate_data.Enabled = True
        btn_browse_CSV.Enabled = True
    End Sub

    Public Sub ExecuteShellBulk(ByVal fileMIF As String, ByVal numberSubroutine As Integer)

        codError = 0
        Dim fileConfig As String = ""
        Dim linregConfig As String = ""
        Dim strArguments As String = ""

        Try
            numFile = 3
            fileConfig = gblPathAppl & "\ociclient.config"
            FileOpen(numFile, fileConfig, OpenMode.Output, OpenAccess.Write)
            linregConfig = "userId = " & My.Settings.User
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "password = " & My.Settings.Password
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "hostname = " & My.Settings.Host
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "port = " & My.Settings.Port
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "sessionID = " & gblSession
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "connectionMode = " & My.Settings.Mode
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "runMode = Multiple"
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "multipleInputFile = " & fileMIF
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "pauseTimeBeforeRunStart = 3"
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "numberOfRuns = 1"
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "quietMode = " & My.Settings.ModeQuit
            'linregConfig = "quietMode = " & "False"
            WriteLine(numFile, linregConfig.ToCharArray)
            linregConfig = "resultOutputFile = " & gblPathLog & "\voxcom_app_" & Format(Now(), "ddMMyyyy_hhmmss") & ".log"
            WriteLine(numFile, linregConfig.ToCharArray)
            FileClose(numFile)
            strArguments &= fileConfig 'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\ociclient.config
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Se produjo un error al crear el archivo" & fileConfig & " y los archivos XML no fueron enviados", MsgBoxStyle.Exclamation, "Error al crear archivo")
            FileClose(numFile)
            Me.Cursor = Cursors.Default
            If numberSubroutine = 1 Then
                codError = 1
                In_Case_Error2()
                lbl_state_cloud.Text = "Error File ociclient.config"
                ProgressBar1.Value = ProgressBar1.Maximum
                My.Application.DoEvents()
            ElseIf numberSubroutine = 2 Then
                codError = 2
                lbl_state_proxy.Text = "Error File archivo ociclient.config"
                ProgressBar2.Value = ProgressBar2.Maximum
                My.Application.DoEvents()
            ElseIf numberSubroutine = 3 Then
                codError = 3
                lbl_state_userLicense.Text = "Error en archivo ociclient.config"
                ProgressBar3.Value = ProgressBar3.Maximum
                My.Application.DoEvents()
            End If
            Exit Sub
        End Try

        If numberSubroutine = 1 Then
            lbl_state_cloud.Text = "Executing App Voxcom..."
            ProgressBar1.Value = 50
            My.Application.DoEvents()
        ElseIf numberSubroutine = 2 Then
            lbl_state_proxy.Text = "Executing App Voxcom..."
            ProgressBar2.Value = 50
            My.Application.DoEvents()
        ElseIf numberSubroutine = 3 Then
            lbl_state_userLicense.Text = "Executing App Voxcom..."
            ProgressBar3.Value = 50
            My.Application.DoEvents()
        End If

        Try
            Dim proceso As New Process()
            'StartInfo obtiene propiedades que luego se pasan al metodo Proceso.Start()
            proceso.StartInfo.WorkingDirectory = gblPathAppl
            proceso.StartInfo.FileName = "startASOCIClient.bat"
            proceso.StartInfo.Arguments = Chr(34) & strArguments & Chr(34)
            proceso.StartInfo.UseShellExecute = True
            proceso.Start()
            proceso.WaitForExit()
            proceso.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
            'grabaLog(1, 3, "Error al ejecutar Shell>" & strArguments)
            Me.Cursor = Cursors.Default
            If numberSubroutine = 1 Then
                codError = 1
                In_Case_Error2()
                lbl_state_cloud.Text = "Error to the execute startASOCIClient.bat"
                ProgressBar1.Value = ProgressBar1.Maximum
                My.Application.DoEvents()
            ElseIf numberSubroutine = 2 Then
                codError = 2
                lbl_state_proxy.Text = "Error to the execute startASOCIClient.bat"
                ProgressBar2.Value = ProgressBar2.Maximum
                My.Application.DoEvents()
            ElseIf numberSubroutine = 3 Then
                codError = 3
                lbl_state_userLicense.Text = "Error to the execute startASOCIClient.bat"
                ProgressBar3.Value = ProgressBar3.Maximum
                My.Application.DoEvents()
            End If
            Exit Sub
        End Try
    End Sub

    Private Sub ParseXML_cloudPBX()

        Me.Cursor = Cursors.WaitCursor
        lbl_state_cloud.Text = "Generating Report..."
        ProgressBar1.Value = 75
        My.Application.DoEvents()

        System.Threading.Thread.Sleep(500)

        btn_report_cloudpbx.Enabled = True 'Se habilita el boton que permite ver el reporte en cualquier momento
        btn_show_report.Enabled = True

        Dim reader As XmlTextReader
        Dim parseXML As String
        Dim response As String = ""
        Dim reportLine As String = ""
        Dim infoLine As String = ""
        Dim num As Integer = 0


        numFile = 4

        Try
            FileOpen(numFile, My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & group_id & "_report.txt", OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Se produjo un error al crear el archivo " & My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & group_id & "_report.txt" & " que genera el reporte", MsgBoxStyle.Exclamation, "Error al crear archivo")
            FileClose(numFile)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try


        For num = 1 To indexXML_Cloud
            Try
                parseXML = gblPathTmpCloud & "\" & num & "_cloudpbx_response.xml"
                reader = New XmlTextReader(parseXML)

                'Si el archivo no tiene el formato esperado o esta vacio se captura la excepción 
                Do While (reader.Read())
                    Select Case reader.NodeType
                        Case XmlNodeType.Element

                            'Existen dos posibles response a encontrar en el archivo

                            If reader.Name = "command" Then
                                If reader.HasAttributes Then 'If attributes exist
                                    While reader.MoveToNextAttribute()
                                        'MsgBox(reader.Name.ToString & reader.Value.ToString) 'Display attribute name and value.
                                        If reader.Name = "xsi:type" Then
                                            If reader.Value = "c:SuccessResponse" Then
                                                response = reader.Value.ToString
                                                'ElseIf reader.Value = "c:ErrorResponse" Then

                                            End If
                                        End If
                                    End While
                                End If
                            End If

                            If reader.Name = "summary" Then
                                response = reader.ReadString
                            End If

                            If reader.Name = "detail" Then
                                response += " " & reader.ReadString
                            End If
                            'Case XmlNodeType.XmlDeclaration
                    End Select
                Loop
                reader.Close()

                If response.Length > 0 Then
                    response += " [File:" & num & "_cloudpbx_response.xml]"
                    reportLine = response
                    WriteLine(numFile, reportLine.ToCharArray)
                End If

            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Ha ocurrido un error con el archivo respuesta " & gblPathTmpCloud & "\" & num & "_cloudpbx_response.xml", MsgBoxStyle.Exclamation, "Error al generar reporte")
                'grabaLog(1, 2, "Error al leer archivo XML>" & gblPathTmpCloud & "\" & num & "_cloudpbx_response.xml")
                lbl_state_cloud.Text = "Error to generate report"
                ProgressBar1.Value = ProgressBar1.Maximum
                Me.Cursor = Cursors.Default
                In_Case_Error2()
                Exit Sub
            End Try
        Next

        infoLine = "Se enviaron " & indexXML_Cloud & " archivos " & vbNewLine & "Se recibieron " & num - 1 & " archivos"

        WriteLine(numFile, infoLine.ToCharArray)

        FileClose(numFile)

        btn_procesar.Enabled = False
        btn_browse_CSV.Enabled = True
        btn_validate_data.Enabled = True

        Me.Cursor = Cursors.Default
        lbl_state_cloud.Text = "Finished"
        ProgressBar1.Value = ProgressBar1.Maximum
        My.Application.DoEvents()

        Process.Start("explorer.exe", My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & group_id & "_report.txt")

    End Sub


    'Public Sub grabaLog(ByVal tipo As Integer, ByVal subtipo As Integer, ByVal mensaje As String)
    '    Dim fileLog As String = ""
    '    Dim linerr As String = ""

    '    linerr = DateAndTime.Now & ">"
    '    'tipo -> 1=ERRO,2=INFO,3=WARN
    '    'subtipo -> 1=DB,2=XML,3=CNX
    '    If tipo = 1 Then
    '        linerr = linerr & "Error>"
    '    End If
    '    If tipo = 2 Then
    '        linerr = linerr & "INFO>"
    '    End If
    '    If tipo = 3 Then
    '        linerr = linerr & "WARNING>"
    '    End If
    '    If subtipo = 1 Then
    '        linerr = linerr & "DB>"
    '    End If
    '    If subtipo = 2 Then
    '        linerr = linerr & "XML>"
    '    End If
    '    If subtipo = 2 Then
    '        linerr = linerr & "CNX>"
    '    End If
    '    linerr = linerr & mensaje
    '    fileLog = gblPathLog & "\LOG_" & DateAndTime.DateString & ".log"

    '    'MsgBox(fileLog.ToString)
    '    lbl_state_cloud.Text = "Saving log"
    '    ProgressBar1.Value = ProgressBar1.Maximum
    '    My.Application.DoEvents()
    '    numFile = 4

    '    FileOpen(numFile, fileLog, OpenMode.Append, OpenAccess.Write)
    '    WriteLine(numFile, linerr.ToCharArray)
    '    FileClose(numFile)
    'End Sub


    '///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    'SEGUNDA INTERFAZ-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------/
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Public Sub getDeviceName()

        If My.Computer.Network.Ping(My.Settings.Host, gblTimePing) Then
            'MsgBox("Server pinged successfully.")
        Else
            MsgBox("Servidor fuera de Linea, favor verifique la conexion", MsgBoxStyle.Exclamation, "Error de Comunicación")
            Exit Sub
        End If

        If tb_groupId_proxy.Text.Length = 0 Then
            MsgBox("Campo de 'groupId' inválido", MsgBoxStyle.Exclamation, "Error campo de búsqueda")
            Exit Sub
        End If

        indexXML_Proxy_DVmac = 0
        Me.Cursor = Cursors.WaitCursor
        lbl_state_proxy.Text = ""
        ProgressBar2.Value = 0
        My.Application.DoEvents()

        'Se eliminan los archivos antiguos del directorio correspondiente
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblPathTmpProxy & "\getDeviceName", FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(foundFile)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los archivos antiguos de la carpeta " & gblPathTmpProxy & "\getDeviceName" & ", verifique que los archivos no esten siendo utilizados por otro proceso", MsgBoxStyle.Exclamation, "Error al eliminar archivos")
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        'XML PARA OBTENER LAS MAC DE LOS DISPOSITIVOS DE UN GRUPO
        Dim r_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim r_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim r_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim r_4 As String = "<command xsi:type=" & Chr(34) & "GroupAccessDeviceGetListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim r_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim r_6 As String = "<groupId>AGPRO_cloudpbx</groupId>"
        Dim r_7 As String = "<responseSizeLimit>1000</responseSizeLimit>"
        Dim r_8 As String = "</command>"

        Dim finalLine As String = "</BroadsoftDocument>"

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim multipleInputFile As String = gblPathTmpProxy & "\getDeviceName\multipleInputFileProxy.txt"
        Dim lineConfigFile As String = ""

        numFile = 5
        Dim numFileDVmac = numFile

        Try
            FileOpen(numFileDVmac, multipleInputFile, OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo" & multipleInputFile & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(numFileDVmac)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        'XML PARA OBTENER LA MAC DE LOS DISPOSITIVOS--------------------------------------------------------------
        Try
            numFile = 6
            indexXML_Proxy_DVmac += 1
            fileIXML = gblPathTmpProxy & "\getDeviceName\" & indexXML_Proxy_DVmac & "_DeviceGetList_request.xml"
            fileOXML = gblPathTmpProxy & "\getDeviceName\" & indexXML_Proxy_DVmac & "_cloudpbx_response.xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, r_1.ToCharArray)
            WriteLine(numFile, r_2.ToCharArray)
            WriteLine(numFile, r_3.ToCharArray)
            WriteLine(numFile, r_4.ToCharArray)
            WriteLine(numFile, r_5.ToCharArray)
            r_6 = "<groupId>" & tb_groupId_proxy.Text.ToString.ToUpper & "_cloudpbx" & "</groupId>"
            WriteLine(numFile, r_6.ToCharArray)
            WriteLine(numFile, r_7.ToCharArray)
            WriteLine(numFile, r_8.ToCharArray)
            WriteLine(numFile, finalLine.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            WriteLine(numFileDVmac, lineConfigFile.ToCharArray)
            FileClose(numFileDVmac)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
            FileClose(numFile)
            FileClose(numFileDVmac)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        ExecuteShellBulk(multipleInputFile, 2)
        If codError = 0 Then
            ParseXML_DVmac()
        End If
    End Sub

    Sub ParseXML_DVmac()

        lbl_state_proxy.Text = "Preparing List of Devices..."
        ProgressBar2.Value = 75
        My.Application.DoEvents()

        Dim comando As New OleDbCommand
        comando.Connection = Conexion
        Dim instruction As String = "delete * from brs_proxy_get_dvmac"
        comando.CommandText = instruction

        Try
            Conexion.Open()
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al acceder a la base de datos e intentar eliminar los elementos antiguos de la tabla 'brs_proxy_get_dvmac'", MsgBoxStyle.Exclamation, "Error al generar reporte")
            lbl_state_proxy.Text = "Error with Database"
            ProgressBar2.Value = ProgressBar2.Maximum
            Me.Cursor = Cursors.Default
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        Dim reader As XmlTextReader
        Dim parseXML As String
        Dim response As String = ""

        Try
            parseXML = gblPathTmpProxy & "\getDeviceName\1_cloudpbx_response.xml"
            reader = New XmlTextReader(parseXML)

            'Si el archivo no tiene el formato esperado o esta vacio se captura la excepción 
            Do While (reader.Read())
                Select Case reader.NodeType
                    Case XmlNodeType.Element

                        'Si no se encuentra el grupo buscado

                        If reader.Name = "summary" Then
                            response = reader.ReadString
                            MsgBox(response.ToString, MsgBoxStyle.Exclamation, "Broadsoft Response")
                            Me.Cursor = Cursors.Default
                            lbl_state_proxy.Text = response.ToString
                            ProgressBar2.Value = ProgressBar2.Maximum
                            reader.Close()
                            Exit Sub

                        ElseIf reader.Name = "col" Then
                            response = reader.ReadString.ToString
                            If response.Length = 15 Then
                                Dim deviceName As String = response.ToString.Substring(0, 2)

                                If deviceName = "DV" Or deviceName = "dv" Or deviceName = "Dv" Then

                                    Dim cadena As String = "insert into brs_proxy_get_dvmac (mac_address) values ( '" & response & "')"
                                    Dim Comando1 As OleDbCommand = Conexion.CreateCommand()
                                    Comando1.CommandText = cadena
                                    Try
                                        Conexion.Open()
                                        Comando1.ExecuteNonQuery()
                                    Catch ex As Exception
                                        MsgBox(ex.ToString)
                                        MsgBox("Error al acceder a la base de datos e intentar insertar nuevos elementos en la tabla 'brs_proxy_get_dvmac'", MsgBoxStyle.Exclamation, "Error to the generate report")
                                        lbl_state_proxy.Text = "Error with Database"
                                        ProgressBar2.Value = ProgressBar2.Maximum
                                        Me.Cursor = Cursors.Default
                                        Conexion.Close()
                                        reader.Close()
                                        Exit Sub
                                    End Try
                                    Conexion.Close()
                                End If
                            End If
                        End If

                        'Si no encuentra los nodos en el archivo, no inserta nada en el access

                        'Case XmlNodeType.XmlDeclaration
                End Select
            Loop
            reader.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Archivo de Respuesta no ha sido encontrado" & gblPathTmpProxy & "\getDeviceName\1_cloudpbx_response.xml", MsgBoxStyle.Exclamation, "Error file response")
            'grabaLog1(1, 2, "Error al leer archivo XML>" & gblPathTmpProxy & "\getDeviceName\" & num & "_cloudpbx_response.xml")
            'msgError = "Respuesta No Generada"
            lbl_state_proxy.Text = "Error file response"
            ProgressBar2.Value = ProgressBar2.Maximum
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try
        Update_ListBox()
    End Sub


    'DataTable utilizada para el rebuild de archivos
    Dim dt1 As New DataTable
    Public Sub Update_ListBox()

        Dim cmd As New OleDbCommand
        cmd.Connection = Conexion
        Dim instruction As String = "select * from brs_proxy_get_dvmac"
        cmd.CommandText = instruction

        Dim da As New OleDbDataAdapter
        Dim dtproxy As New DataTable
        dt1 = dtproxy

        Try
            Conexion.Open()
            da.SelectCommand = cmd
            da.Fill(dtproxy)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudo traer la información desde la tabla brs_proxy_get_dvmac", MsgBoxStyle.Exclamation, "Error con base de datos")
            lbl_state_proxy.Text = "Error with Database"
            ProgressBar2.Value = ProgressBar2.Maximum
            Me.Cursor = Cursors.Default
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        Label2.Text = "Se encontraron " + dtproxy.Rows.Count.ToString() + " dispositivos en el grupo " + tb_groupId_proxy.Text
        listbox_proxy.Enabled = True
        Me.listbox_proxy.Items.Clear()

        For j = 0 To dtproxy.Rows.Count - 1
            listbox_proxy.Items.Add(dtproxy.Rows.Item(j)(0).ToString)
        Next

        'listbox_proxy.Refresh()

        tb_write_proxy.Enabled = True
        btn_process_proxy.Enabled = True
        lbl_proxy.Enabled = True
        cb_modify_proxy.Enabled = True
        cb_add_proxy.Enabled = True
        cb_modify_proxy.Checked = True
        cb_add_proxy.Checked = False
        Me.Cursor = Cursors.Default
        lbl_state_proxy.Text = "Finished"
        ProgressBar2.Value = ProgressBar2.Maximum
    End Sub


    Public Sub ModifyProxy()

        If MsgBox("¿Está seguro que desea continuar?", vbOKCancel, "Confirmación") = MsgBoxResult.Cancel Then
            Exit Sub
        End If

        If tb_write_proxy.Text.Length >= 7 Then

        Else
            MsgBox("Campo de 'proxy' inválido", MsgBoxStyle.Exclamation, "Error campo de búsqueda")
            Exit Sub
        End If

        indexXML_Proxy = 0
        Me.Cursor = Cursors.WaitCursor
        lbl_state_proxy.Text = ""
        ProgressBar2.Value = 0
        My.Application.DoEvents()

        'Se eliminan los archivos antiguos del directorio correspondiente
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblPathTmpProxy & "\modifyProxy", FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(foundFile)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los archivos antiguos de la carpeta " & gblPathTmpProxy & "\modifyProxy" & ", verifique que los archivos no esten siendo utilizados por otro proceso", MsgBoxStyle.Exclamation, "Error al eliminar archivos")
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        Dim line1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim line2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim line3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"

        'XML PARA MODIFICAR PROXY
        Dim j_4 As String = "<command xsi:type=" & Chr(34) & "GroupAccessDeviceCustomTagModifyRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim j_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim j_6 As String = "<groupId>AUTOPRO_cloudpbx</groupId>"
        Dim j_7 As String = "<deviceName>DV_805EC02EC440</deviceName>"
        Dim j_8 As String = "<tagName>%SBC_ADDRESS%</tagName>"
        Dim j_9 As String = "<tagValue>172.24.16.211</tagValue>"
        Dim j_10 As String = "</command>"

        'XML PARA RECONSTRUIR LOS ARCHIVOS DE LOS DISPOSITIVOS
        Dim s_4 As String = "<command xsi:type=" & Chr(34) & "GroupCPEConfigRebuildDeviceConfigFileRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim s_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim s_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim s_7 As String = "<deviceName>DV_805EC0568966</deviceName>"
        Dim s_8 As String = "</command>"

        Dim finalLine As String = "</BroadsoftDocument>"

        If cb_modify_proxy.Checked = True Then
            j_4 = "<command xsi:type=" & Chr(34) & "GroupAccessDeviceCustomTagModifyRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        ElseIf cb_add_proxy.Checked = True Then
            j_4 = "<command xsi:type=" & Chr(34) & "GroupAccessDeviceCustomTagAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        End If

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim multipleInputFile As String = gblPathTmpProxy & "\modifyProxy\multipleInputFile.txt"
        Dim lineConfigFile As String = ""
        Dim proxy As String = ""
        Dim group_id As String = ""
        Dim dv_mac As String = ""

        numFile = 7
        Dim numFileProxy As Integer = numFile

        Try
            FileOpen(numFileProxy, multipleInputFile, OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo " & multipleInputFile & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(numFileProxy)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        Try
            For j = 0 To dt1.Rows.Count - 1
                numFile = 8
                indexXML_Proxy += 1
                fileIXML = gblPathTmpProxy & "\modifyProxy\" & indexXML_Proxy & "_CreateProxy_request.xml"
                fileOXML = gblPathTmpProxy & "\modifyProxy\" & indexXML_Proxy & "_cloudpbx_response.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, line1.ToCharArray)
                WriteLine(numFile, line2.ToCharArray)
                WriteLine(numFile, line3.ToCharArray)
                WriteLine(numFile, j_4.ToCharArray)
                WriteLine(numFile, j_5.ToCharArray)
                group_id = tb_groupId_proxy.Text.ToString
                j_6 = "<groupId>" & group_id.ToUpper & "_cloudpbx" & "</groupId>"
                WriteLine(numFile, j_6.ToCharArray)
                dv_mac = dt1.Rows(j)(0).ToString
                j_7 = "<deviceName>" & dv_mac & "</deviceName>"
                WriteLine(numFile, j_7.ToCharArray)
                WriteLine(numFile, j_8.ToCharArray)
                proxy = tb_write_proxy.Text.ToString
                j_9 = "<tagValue>" & proxy & "</tagValue>"
                WriteLine(numFile, j_9.ToCharArray)
                WriteLine(numFile, j_10.ToCharArray)
                WriteLine(numFile, finalLine.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(numFileProxy, lineConfigFile.ToCharArray)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation)
            FileClose(numFile)
            FileClose(numFileProxy)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        Try
            For j = 0 To dt1.Rows.Count - 1
                numFile = 9
                indexXML_Proxy += 1
                fileIXML = gblPathTmpProxy & "\modifyProxy\" & indexXML_Proxy & "_RebuildDevice_request.xml"
                fileOXML = gblPathTmpProxy & "\modifyProxy\" & indexXML_Proxy & "_cloudpbx_response.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, line1.ToCharArray)
                WriteLine(numFile, line2.ToCharArray)
                WriteLine(numFile, line3.ToCharArray)
                WriteLine(numFile, s_4.ToCharArray)
                WriteLine(numFile, s_5.ToCharArray)
                group_id = tb_groupId_proxy.Text.ToString
                s_6 = "<groupId>" & group_id.ToUpper & "_cloudpbx" & "</groupId>"
                WriteLine(numFile, s_6.ToCharArray)
                dv_mac = dt1.Rows(j)(0).ToString
                s_7 = "<deviceName>" & dv_mac & "</deviceName>"
                WriteLine(numFile, s_7.ToCharArray)
                WriteLine(numFile, s_8.ToCharArray)
                WriteLine(numFile, finalLine.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(numFileProxy, lineConfigFile.ToCharArray)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation)
            FileClose(numFile)
            FileClose(numFileProxy)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        FileClose(numFileProxy)

        ExecuteShellBulk(multipleInputFile, 2)
        If codError = 0 Then
            ParseXML_proxy()
        End If
    End Sub

    Private Sub ParseXML_proxy()

        lbl_state_proxy.Text = "Generating Report..."
        ProgressBar2.Value = 75
        My.Application.DoEvents()


        Dim comando As New OleDbCommand()
        comando.Connection = Conexion
        Dim instruction As String = "delete * from brs_proxy_response"
        comando.CommandText = instruction

        Try
            Conexion.Open()
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al acceder a la base de datos e intentar eliminar los elementos antiguos de la tabla 'brs_proxy_response'", MsgBoxStyle.Exclamation, "Error al generar reporte")
            lbl_state_proxy.Text = "Error with Database"
            ProgressBar2.Value = ProgressBar2.Maximum
            Me.Cursor = Cursors.Default
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        Dim reader As XmlTextReader
        Dim parseXML As String
        Dim response As String = ""

        For num = 1 To indexXML_Proxy
            Try
                parseXML = gblPathTmpProxy & "\modifyProxy\" & num & "_cloudpbx_response.xml"
                reader = New XmlTextReader(parseXML)

                'Si el archivo no tiene el formato esperado o esta vacio se captura la excepción 
                Do While (reader.Read())
                    Select Case reader.NodeType
                        Case XmlNodeType.Element

                            'Existen dos posibles response a encontrar en el archivo

                            If reader.Name = "command" Then
                                If reader.HasAttributes Then 'If attributes exist
                                    While reader.MoveToNextAttribute()
                                        'MsgBox(reader.Name.ToString & reader.Value.ToString) 'Display attribute name and value.
                                        If reader.Name = "xsi:type" Then
                                            If reader.Value = "c:SuccessResponse" Then
                                                response = reader.Value.ToString
                                                'ElseIf reader.Value = "c:ErrorResponse" Then

                                            End If
                                        End If
                                    End While
                                End If
                            End If

                            If reader.Name = "summary" Then
                                response = reader.ReadString
                            End If

                            If reader.Name = "detail" Then
                                response += reader.ReadString
                            End If
                            'Case XmlNodeType.XmlDeclaration
                    End Select
                Loop
                reader.Close()

                If response.Length > 0 Then
                    response += " [File:" & num & "_cloudpbx_response.xml]"
                    Dim cadena As String = "insert into brs_proxy_response (response) values ( '" & response & "')"
                    'Crear un comando
                    Dim Comando1 As OleDbCommand = Conexion.CreateCommand()
                    Comando1.CommandText = cadena
                    Try
                        Conexion.Open()
                        Comando1.ExecuteNonQuery()
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                        MsgBox("Error al acceder a la base de datos e intentar agregar registros a la tabla 'brs_proxy_response'", MsgBoxStyle.Exclamation, "Error al generar reporte")
                        lbl_state_proxy.Text = "Error with Database"
                        ProgressBar2.Value = ProgressBar2.Maximum
                        Me.Cursor = Cursors.Default
                        Conexion.Close()
                        Exit Sub
                    End Try
                    Conexion.Close()
                    response = ""
                End If

            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Archivo de Respuesta no ha sido encontrado" & gblPathTmpProxy & "\modifyProxy\" & num & "_cloudpbx_response.xml", MsgBoxStyle.Exclamation, "Error al generar reporte")
                'grabaLog(1, 2, "Error al leer archivo XML>" & gblPathTmpCloud & "\" & num & "_cloudpbx_response.xml")
                lbl_state_proxy.Text = "Error to generate report"
                ProgressBar2.Value = ProgressBar2.Maximum
                Me.Cursor = Cursors.Default
                Exit Sub
            End Try
        Next

        Dim FMP As New Frm_Report_Proxy
        FMP.Show()
        FMP.BringToFront()

        tb_groupId_proxy.Text = ""
        tb_write_proxy.Text = ""
        cb_modify_proxy.Enabled = False
        cb_add_proxy.Enabled = False
        tb_write_proxy.Enabled = False
        btn_process_proxy.Enabled = False
        lbl_proxy.Enabled = False
        Me.listbox_proxy.Items.Clear()
        listbox_proxy.Enabled = False
        Label2.Text = ""

        Me.Cursor = Cursors.Default
        lbl_state_proxy.Text = "Finished"
        ProgressBar2.Value = ProgressBar2.Maximum
        My.Application.DoEvents()
    End Sub


    '///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    'TERCERA INTERFAZ-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------/
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Public Sub GetUserListInGroup()

        If My.Computer.Network.Ping(My.Settings.Host, gblTimePing) Then
            'MsgBox("Server pinged successfully.")
        Else
            MsgBox("Servidor fuera de Linea, favor verifique la conexion", MsgBoxStyle.Exclamation, "Error de Comunicación")
            Exit Sub
        End If

        If tb_groupId_UserGetList.Text.Length = 0 Then
            MsgBox("Campo de 'groupId' inválido", MsgBoxStyle.Exclamation, "Error campo de búsqueda")
            Exit Sub
        End If

        indexXML_UsersLincense_Group = 0
        Me.Cursor = Cursors.WaitCursor
        lbl_state_userLicense.Text = ""
        ProgressBar3.Value = 0
        My.Application.DoEvents()

        'Se eliminan los archivos antiguos del directorio correspondiente
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblPathTmpUserLicense & "\GetUserListInGroup", FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(foundFile)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los archivos antiguos de la carpeta " & gblPathTmpUserLicense & "\GetUserListInGroup" & ", verifique que los archivos no esten siendo utilizados por otro proceso", MsgBoxStyle.Exclamation, "Error al eliminar archivos")
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try


        'XML PARA OBTENER LA LISTA DE USUARIOS DE UN GRUPO------------------------------------------------------------------------------------------------------
        Dim t_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim t_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim t_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim t_4 As String = "<command xsi:type=" & Chr(34) & "UserGetListInGroupRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim t_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim t_6 As String = "<GroupId>AGPRO_cloudpbx</GroupId>"
        Dim t_7 As String = "<responseSizeLimit>1000</responseSizeLimit>"
        Dim t_8 As String = "</command>"

        Dim finalLine As String = "</BroadsoftDocument>"

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim multipleInputFile As String = gblPathTmpUserLicense & "\GetUserListInGroup\multipleInputFile.txt"
        Dim lineConfigFile As String = ""

        numFile = 10
        Dim numFileUserGetList = numFile

        Try
            FileOpen(numFileUserGetList, multipleInputFile, OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo" & multipleInputFile & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(numFileUserGetList)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        'XML PARA OBTENER LA LISTA DE USUARIOS DE UN GRUPO-----------------------------------------------------------------------------------------------------
        Try
            numFile = 11
            indexXML_UsersLincense_Group += 1
            fileIXML = gblPathTmpUserLicense & "\GetUserListInGroup\" & indexXML_UsersLincense_Group & "_GetUserList_request.xml"
            fileOXML = gblPathTmpUserLicense & "\GetUserListInGroup\" & indexXML_UsersLincense_Group & "_cloudpbx_response.xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, t_1.ToCharArray)
            WriteLine(numFile, t_2.ToCharArray)
            WriteLine(numFile, t_3.ToCharArray)
            WriteLine(numFile, t_4.ToCharArray)
            WriteLine(numFile, t_5.ToCharArray)
            t_6 = "<GroupId>" & tb_groupId_UserGetList.Text.ToString.ToUpper & "_cloudpbx" & "</GroupId>"
            WriteLine(numFile, t_6.ToCharArray)
            WriteLine(numFile, t_7.ToCharArray)
            WriteLine(numFile, t_8.ToCharArray)
            WriteLine(numFile, finalLine.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            WriteLine(numFileUserGetList, lineConfigFile.ToCharArray)
            FileClose(numFileUserGetList)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation, "Error al crear el archivo")
            FileClose(numFile)
            FileClose(numFileUserGetList)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        ExecuteShellBulk(multipleInputFile, 3)
        If codError = 0 Then
            ParseXML_UserGetList()
        End If
    End Sub

    Private Sub ParseXML_UserGetList()

        lbl_state_userLicense.Text = "Getting Users..."
        ProgressBar3.Value = 50
        My.Application.DoEvents()

        Dim comando As New OleDbCommand
        comando.Connection = Conexion
        Dim instruction As String = "delete * from brs_get_user_license"
        comando.CommandText = instruction

        Try
            Conexion.Open()
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al acceder a la base de datos e intentar eliminar los elementos antiguos de la tabla 'brs_get_user_license'", MsgBoxStyle.Exclamation, "Error al generar reporte")
            lbl_state_userLicense.Text = "Error with Database"
            ProgressBar3.Value = ProgressBar3.Maximum
            Me.Cursor = Cursors.Default
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        Dim xmldoc As New XmlDocument
        Dim xmlnode As XmlNodeList
        Dim xmlnodeSummary As XmlNodeList
        Dim response As String = ""

        Try
            Dim fs As New FileStream(gblPathTmpUserLicense & "\GetUserListInGroup\1_cloudpbx_response.xml", FileMode.Open, FileAccess.Read)

            xmldoc.Load(fs)
            xmlnode = xmldoc.GetElementsByTagName("col")
            xmlnodeSummary = xmldoc.GetElementsByTagName("summary")

            If xmlnodeSummary.Count = 1 Then
                MsgBox(xmlnodeSummary(0).InnerText.ToString, MsgBoxStyle.Exclamation, "Broadsoft Response")
                lbl_state_userLicense.Text = "Error de Consulta"
                ProgressBar3.Value = ProgressBar3.Maximum
                Me.Cursor = Cursors.Default
                fs.Close()
                Exit Sub
            Else
                xmlnode = xmldoc.GetElementsByTagName("col")

                'xmlnode contiene todos los nodos "col" del unico archivo response posible y puede puede tener un valor minimo de 11 elementos
                ReDim ArrayUserGetList((xmlnode.Count / 11) - 1)
                Dim contador As Integer = 0

                For i = 0 To xmlnode.Count - 1
                    If i = 0 Or (i Mod 11) = 0 Then
                        response = xmlnode(i).InnerText.ToString
                        ArrayUserGetList(contador) = response.ToString
                        'MsgBox(ArrayUserGetList(contador).ToString)
                        contador += 1
                    End If
                Next
                fs.Close()
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Ha ocurrido un error con el archivo respuesta ", MsgBoxStyle.Exclamation, "Error al generar reporte")
            'grabaLog(1, 2, "Error al leer archivo XML>" & gblPathTmpCloud & "\" & num & "_cloudpbx_response.xml")
            lbl_state_userLicense.Text = "Error al generar reporte"
            ProgressBar3.Value = ProgressBar3.Maximum
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        GetUserLicense()
    End Sub

    'DataTable utilizada para el rebuild de archivos

    Public Sub GetUserLicense()

        indexXML_UsersLincense = 0

        'Se eliminan los archivos antiguos del directorio correspondiente
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblPathTmpUserLicense & "\GetUserLicense", FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(foundFile)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los archivos antiguos de la carpeta " & gblPathTmpUserLicense & "\GetUserLicense" & ", verifique que los archivos no esten siendo utilizados por otro proceso", MsgBoxStyle.Exclamation, "Error al eliminar archivos")
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        'XML PARA OBTENER LA LICENCIA DE CADA USUARIO DE UN GRUPO
        Dim u_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim u_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim u_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim u_4 As String = "<command xsi:type=" & Chr(34) & "UserServiceGetAssignmentListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim u_5 As String = "<userId>232780191@telefonicachile.cl</userId>"
        Dim u_6 As String = "</command>"

        Dim finalLine As String = "</BroadsoftDocument>"

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim multipleInputFile As String = gblPathTmpUserLicense & "\GetUserLicense\multipleInputFile.txt"
        Dim lineConfigFile As String = ""
        Dim userId As String = ""

        numFile = 12
        Dim numFileUserLicense As Integer = numFile

        Try
            FileOpen(numFileUserLicense, multipleInputFile, OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo " & multipleInputFile & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(numFileUserLicense)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        Try
            For j = 0 To ArrayUserGetList.Length - 1
                numFile = 13
                indexXML_UsersLincense += 1
                fileIXML = gblPathTmpUserLicense & "\GetUserLicense\" & indexXML_UsersLincense & "_GetUserLicense_request.xml"
                fileOXML = gblPathTmpUserLicense & "\GetUserLicense\" & indexXML_UsersLincense & "_cloudpbx_response.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, u_1.ToCharArray)
                WriteLine(numFile, u_2.ToCharArray)
                WriteLine(numFile, u_3.ToCharArray)
                WriteLine(numFile, u_4.ToCharArray)
                'userId = dtproxy.Rows(j)(0).ToString
                userId = ArrayUserGetList(j).ToString
                u_5 = "<userId>" & userId & "</userId>"
                WriteLine(numFile, u_5.ToCharArray)
                WriteLine(numFile, u_6.ToCharArray)
                WriteLine(numFile, finalLine.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(numFileUserLicense, lineConfigFile.ToCharArray)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al crear el archivo " & fileIXML, MsgBoxStyle.Exclamation)
            FileClose(numFile)
            FileClose(numFileUserLicense)
            Exit Sub
        End Try

        FileClose(numFileUserLicense)

        ExecuteShellBulk(multipleInputFile, 3)
        If codError = 0 Then
            ParseXML_UserGetLicense()
        End If

    End Sub

    Private Sub ParseXML_UserGetLicense()

        lbl_state_userLicense.Text = "Getting User Licenses..."
        ProgressBar3.Value = 75
        My.Application.DoEvents()

        Dim comando As New OleDbCommand
        comando.Connection = Conexion
        Dim instruction As String = "delete * from brs_get_user_license" 'Dim instruction As String = "DELETE * FROM brs_get_user_license WHERE user_id Is Null AND basic Is Null AND standard Is Null AND advanced is Null"
        comando.CommandText = instruction

        Try
            Conexion.Open()
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al acceder a la base de datos e intentar eliminar los elementos antiguos de la tabla 'brs_get_user_license'", MsgBoxStyle.Exclamation, "Error al generar reporte")
            lbl_state_userLicense.Text = "Error with Database"
            ProgressBar3.Value = ProgressBar3.Maximum
            Me.Cursor = Cursors.Default
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        Dim xmldoc As New XmlDocument
        Dim xmlnode As XmlNodeList
        Dim response As String = ""
        Dim response1 As String = ""
        Dim response2 As String = ""
        Dim response3 As String = ""

        For num = 1 To indexXML_UsersLincense
            Try
                Dim fs As New FileStream(gblPathTmpUserLicense & "\GetUserLicense\" & num & "_cloudpbx_response.xml", FileMode.Open, FileAccess.Read)

                xmldoc.Load(fs)
                xmlnode = xmldoc.GetElementsByTagName("col")

                For i = 0 To xmlnode.Count - 1
                    If xmlnode(i).InnerText.Equals("Pack_Basico") Then
                        response1 = xmlnode(i + 1).InnerText.ToString
                    End If
                    If xmlnode(i).InnerText.Equals("Pack_Estandar") Then
                        response2 = xmlnode(i + 1).InnerText.ToString
                    End If
                    If xmlnode(i).InnerText.Equals("Pack_Avanzado") Then
                        response3 = xmlnode(i + 1).InnerText.ToString
                    End If
                Next
                fs.Close()

                'Se obtiene el valor que se guarda en el campo user_id
                response = ArrayUserGetList(num - 1)

                If (response1.Length Or response2.Length Or response3.Length) > 0 Then

                    If response1 = "" Then
                        response1 = "false"
                    End If
                    If response2 = "" Then
                        response2 = "false"
                    End If
                    If response3 = "" Then
                        response3 = "false"
                    End If

                    Dim cadena As String = "insert into brs_get_user_license (user_id, basic, standard, advanced)  values (@value,  @value1, @value2, @value3)"
                    'Dim cadena As String = "insert into brs_get_user_license (basic, standard, advanced)  values (@value,  @value1, @value2)"
                    'Dim cadena As String = "update brs_get_user_license set basic = @value, standard = @value1, advanced = @value2"

                    Dim Comando1 As New OleDbCommand(cadena, Conexion)

                    Comando1.Parameters.AddWithValue("@value", response)
                    Comando1.Parameters.AddWithValue("@value1", response1)
                    Comando1.Parameters.AddWithValue("@value2", response2)
                    Comando1.Parameters.AddWithValue("@value3", response3)

                    Try
                        Conexion.Open()
                        Comando1.ExecuteNonQuery()
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                        MsgBox("Error al acceder a la base de datos e intentar insertar nuevos elementos en la tabla 'brs_get_user_license'", MsgBoxStyle.Exclamation, "Error al generar reporte")
                        lbl_state_userLicense.Text = "Error with Database"
                        ProgressBar3.Value = ProgressBar3.Maximum
                        Me.Cursor = Cursors.Default
                        Conexion.Close()
                        Exit Sub
                    End Try
                    Conexion.Close()
                    response = ""
                    response1 = ""
                    response2 = ""
                    response3 = ""

                Else
                    'En caso de que un usuario no posea ninguna licencia
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Ha ocurrido un error con el archivo respuesta", MsgBoxStyle.Exclamation, "Error al generar reporte")
                'grabaLog(1, 2, "Error al leer archivo XML>" & gblPathTmpCloud & "\" & num & "_cloudpbx_response.xml")
                lbl_state_userLicense.Text = "Error to generate report"
                ProgressBar3.Value = ProgressBar3.Maximum
                Me.Cursor = Cursors.Default
                Exit Sub
            End Try
        Next

        'xmlnode(i).ChildNodes.Item(0).InnerText.Trim()
        'str = xmlnode(i).ChildNodes.Item(0).InnerText.Trim() & "  " & xmlnode(i).ChildNodes.Item(1).InnerText.Trim() & "  " & xmlnode(i).ChildNodes.Item(2).InnerText.Trim()
        Update_Grid2()
    End Sub

    Public Sub Update_Grid2()

        Dim cmd As New OleDbCommand
        Dim da As New OleDbDataAdapter
        Dim dtproxy As New DataTable
        Dim instruction As String = "select * from brs_get_user_license"

        Try
            Conexion.Open()
            cmd.Connection = Conexion
            cmd.CommandText = instruction
            da.SelectCommand = cmd
            da.Fill(dtproxy)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudo traer la información desde la tabla brs_get_user_license", MsgBoxStyle.Exclamation, "Error con base de datos")
            lbl_state_userLicense.Text = "Error with Database"
            ProgressBar3.Value = ProgressBar3.Maximum
            Me.Cursor = Cursors.Default
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        Dim collectionRows As DataRowCollection
        Dim rows As DataRow
        collectionRows = dtproxy.Rows

        DataGridView2.Rows.Clear()
        DataGridView2.Refresh()

        'Se muestran los datos en el datagridview2
        For Each rows In collectionRows
            DataGridView2.Rows.Add(rows.ItemArray)
        Next

        DataGridView2.Enabled = True
        DataGridView2.EnableHeadersVisualStyles = True

        lbl_numUser.Text = "Se encontraron " + dtproxy.Rows.Count.ToString() + " usuarios en el grupo " + tb_groupId_UserGetList.Text

        lbl_state_userLicense.Text = "Finished"
        ProgressBar3.Value = ProgressBar3.Maximum
        Me.Cursor = Cursors.Default
        btn_process_userLicense.Enabled = True
        My.Application.DoEvents()

        'DataGridView2.DataSource = dtproxy
        'DataGridView2.Columns.Clear()

        'Assignment_UserLicensse()

        'Dim btn As New DataGridViewButtonColumn()
        'DataGridView2.Columns.Add(btn)
        'btn.HeaderText = "Click Data"
        'btn.Text = "Click Here"
        'btn.Name = "btn"
        'btn.UseColumnTextForButtonValue = True

        'para mostrar el listado de usuarios en el listbox
        'Me.ListBox2.Items.Clear()
        'For j = 0 To dtproxy.Rows.Count - 1
        '    ListBox2.Items.Add(dtproxy.Rows.Item(j)(0).ToString)
        'Next
        'ListBox2.Refresh()
    End Sub


    Private Sub Assignment_UserLicensse()

        If My.Computer.Network.Ping(My.Settings.Host, gblTimePing) Then
            'MsgBox("Server pinged successfully.")
        Else
            MsgBox("Servidor fuera de Linea, favor verifique la conexion", MsgBoxStyle.Exclamation, "Error de Comunicación")
            Exit Sub
        End If

        indexXML_UsersLicense_Assign = 0
        Me.Cursor = Cursors.WaitCursor
        lbl_state_userLicense.Text = ""
        ProgressBar3.Value = 0
        My.Application.DoEvents()

        'DataGridView2.EnableHeadersVisualStyles = False
        'DataGridView2.Enabled = False

        'Se eliminan los archivos antiguos del directorio correspondiente
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblPathTmpUserLicense & "\AssigmentUserLicense\AssignServices\", FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(foundFile)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los archivos antiguos de la carpeta " & gblPathTmpUserLicense & "\AssigmentUserLicense\AssignServices\" & ", verifique que los archivos no esten siendo utilizados por otro proceso", MsgBoxStyle.Exclamation, "Error al eliminar archivos")
            Me.Cursor = Cursors.Default
            'DataGridView2.Enabled = True
            'DataGridView2.EnableHeadersVisualStyles = False
            Exit Sub
        End Try

        'XML PARA ASIGNAR PACK DE SERVICIOS---------------------------------------------------------------------
        Dim line1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim line2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim line3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"

        Dim l_4 As String = "<command xsi:type=" & Chr(34) & "UserServiceAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim l_5 As String = "<userId>226337160@autopro.cl</userId>"
        Dim l_6 As String = "<servicePackName>Pack_Basico</servicePackName>"
        Dim l_7 As String = "</command>"

        Dim finalLine As String = "</BroadsoftDocument>"

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim multipleInputFile As String = gblPathTmpUserLicense & "\AssigmentUserLicense\AssignServices\multipleInputFile.txt"
        Dim lineConfigFile As String = ""
        Dim userId As String = ""
        Dim packBasic As String = ""
        Dim packStandard As String = ""
        Dim packAdvanced As String = ""
        Dim AssignState As Integer = 0
        Dim UnAssignState As Integer = 0

        numFile = 14
        Dim numFileUserLicense As Integer = numFile

        Try
            FileOpen(numFileUserLicense, multipleInputFile, OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo " & multipleInputFile & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(numFileUserLicense)
            Me.Cursor = Cursors.Default
            'DataGridView2.Enabled = True
            'DataGridView2.EnableHeadersVisualStyles = True
            Exit Sub
        End Try

        'XML PARA ASIGNAR PACK DE SERVICIOS---------------------------------------------------------------------
        For j = 0 To DataGridView2.RowCount - 1
            Try
                packBasic = DataGridView2.Rows(j).Cells(1).Value.ToString
                packStandard = DataGridView2.Rows(j).Cells(2).Value.ToString
                packAdvanced = DataGridView2.Rows(j).Cells(3).Value.ToString

                If packBasic.Equals("TRUE") Or packStandard.Equals("TRUE") Or packAdvanced.Equals("TRUE") Then

                    AssignState = 1

                    numFile = 15
                    indexXML_UsersLicense_Assign += 1
                    fileIXML = gblPathTmpUserLicense & "\AssigmentUserLicense\AssignServices\" & indexXML_UsersLicense_Assign & "_AssignServices_request.xml"
                    fileOXML = gblPathTmpUserLicense & "\AssigmentUserLicense\AssignServices\" & indexXML_UsersLicense_Assign & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, l_4.ToCharArray)

                    userId = DataGridView2.Rows(j).Cells(0).Value.ToString
                    l_5 = "<userId>" & userId & "</userId>"
                    WriteLine(numFile, l_5.ToCharArray)

                    If packBasic.Equals("TRUE") Then
                        l_6 = "<servicePackName>Pack_Basico</servicePackName>"
                        WriteLine(numFile, l_6.ToCharArray)
                    End If
                    If packStandard.Equals("TRUE") Then
                        l_6 = "<servicePackName>Pack_Estandar</servicePackName>"
                        WriteLine(numFile, l_6.ToCharArray)
                    End If
                    If packAdvanced.Equals("TRUE") Then
                        l_6 = "<servicePackName>Pack_Avanzado</servicePackName>"
                        WriteLine(numFile, l_6.ToCharArray)
                    End If

                    WriteLine(numFile, l_7.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(numFileUserLicense, lineConfigFile.ToCharArray)
                End If

                If packBasic.Equals("FALSE") Or packStandard.Equals("FALSE") Or packAdvanced.Equals("FALSE") Then

                    UnAssignState = 1

                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & gblPathTmpUserLicense & "\AssigmentUserLicense\AssignServices\" & indexXML_UsersLicense_Assign & "_AssignServices_request.xml", MsgBoxStyle.Exclamation)
                FileClose(numFile)
                FileClose(numFileUserLicense)
                Me.Cursor = Cursors.Default
                'DataGridView2.Enabled = True
                'DataGridView2.EnableHeadersVisualStyles = True
                Exit Sub
            End Try
        Next

        FileClose(numFileUserLicense)

        If AssignState = 0 And UnAssignState = 0 Then
            MsgBox("No se establecieron cambios en las asignacioens de icencias de usuarios", MsgBoxStyle.Exclamation, "Revise los campos 'TRUE' y 'FALSE'")
            Me.Cursor = Cursors.Default
            Exit Sub

        ElseIf AssignState = 1 And UnAssignState = 1 Then
            If MsgBox("¿Está seguro que desea continuar?", vbOKCancel, "Se asignarán y desasignarán licencias a usuarios") = MsgBoxResult.Cancel Then
                Me.Cursor = Cursors.Default
                Exit Sub
            End If

            ExecuteShellBulk(multipleInputFile, 3)
            If codError = 0 Then
                ParseXML_Assignment_UserLicense()
                Dim confirmacion As Integer = Validate_Response()
                If confirmacion = 1 Then
                    UnAssignment_UserLicensse()
                    GetUserListInGroup()
                End If
            End If

        ElseIf AssignState = 1 And UnAssignState = 0 Then
            If MsgBox("¿Está seguro que desea continuar?", vbOKCancel, "Se asignarán licencias a usuarios") = MsgBoxResult.Cancel Then
                Me.Cursor = Cursors.Default
                Exit Sub
            End If
            ExecuteShellBulk(multipleInputFile, 3)
            If codError = 0 Then
                ParseXML_Assignment_UserLicense()
                GetUserListInGroup()
            End If

        ElseIf AssignState = 0 And UnAssignState = 1 Then
            If MsgBox("¿Está seguro que desea continuar?", vbOKCancel, "Se quitarán licencias a usuarios") = MsgBoxResult.Cancel Then
                Me.Cursor = Cursors.Default
                Exit Sub
            End If

            UnAssignment_UserLicensse()
            GetUserListInGroup()
        End If

    End Sub

    Private Sub ParseXML_Assignment_UserLicense()

        lbl_state_userLicense.Text = "Getting new user licenses..."
        ProgressBar3.Value = 75
        My.Application.DoEvents()

        Dim comando As New OleDbCommand()
        comando.Connection = Conexion
        Dim instruction As String = "delete * from brs_user_license_assingment"
        comando.CommandText = instruction

        Try
            Conexion.Open()
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al acceder a la base de datos e intentar eliminar los elementos antiguos de la tabla 'brs_user_license_assingment'", MsgBoxStyle.Exclamation, "Error al generar reporte")
                lbl_state_userLicense.Text = "Error with Database"
            ProgressBar3.Value = ProgressBar3.Maximum
            Me.Cursor = Cursors.Default
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        Dim reader As XmlTextReader
        Dim parseXML As String
        Dim response As String = ""

        For num = 1 To indexXML_UsersLicense_Assign
            Try
                parseXML = gblPathTmpUserLicense & "\AssigmentUserLicense\AssignServices\" & num & "_cloudpbx_response.xml"
                reader = New XmlTextReader(parseXML)

                'Si el archivo no tiene el formato esperado o esta vacio se captura la excepción 
                Do While (reader.Read())
                    Select Case reader.NodeType
                        Case XmlNodeType.Element

                            'Existen dos posibles response a encontrar en el archivo

                            If reader.Name = "command" Then
                                If reader.HasAttributes Then 'If attributes exist
                                    While reader.MoveToNextAttribute()
                                        'MsgBox(reader.Name.ToString & reader.Value.ToString) 'Display attribute name and value.
                                        If reader.Name = "xsi:type" Then
                                            If reader.Value = "c:SuccessResponse" Then
                                                response = reader.Value.ToString
                                                'ElseIf reader.Value = "c:ErrorResponse" Then

                                            End If
                                        End If
                                    End While
                                End If
                            End If

                            If reader.Name = "summary" Then
                                response = reader.ReadString
                            End If

                            If reader.Name = "detail" Then
                                response += reader.ReadString
                            End If
                            'Case XmlNodeType.XmlDeclaration
                    End Select
                Loop
                reader.Close()

                If response.Length > 0 Then
                    response += " [File:" & num & "_cloudpbx_response.xml]"
                    Dim cadena As String = "insert into brs_user_license_assingment (response) VALUES ( '" & response & "')"
                    'Crear un comando
                    Dim Comando1 As OleDbCommand = Conexion.CreateCommand()
                    Comando1.CommandText = cadena
                    Try
                        Conexion.Open()
                        Comando1.ExecuteNonQuery()
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                        MsgBox("Error al acceder a la base de datos e intentar agregar registros a la tabla 'brs_proxy_response'", MsgBoxStyle.Exclamation, "Error al generar reporte")
                        lbl_state_userLicense.Text = "Error with Database"
                        ProgressBar3.Value = ProgressBar3.Maximum
                        Me.Cursor = Cursors.Default
                        Conexion.Close()
                        Exit Sub
                    End Try
                    Conexion.Close()
                    response = ""
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Archivo de Respuesta no ha sido encontrado", MsgBoxStyle.Exclamation, "Error al generar reporte")
                'grabaLog(1, 2, "Error al leer archivo XML>" & gblPathTmpCloud & "\" & num & "_cloudpbx_response.xml")
                lbl_state_userLicense.Text = "Error to generate report"
                ProgressBar3.Value = ProgressBar3.Maximum
                Me.Cursor = Cursors.Default
                Exit Sub
            End Try
        Next

        Me.Cursor = Cursors.Default
        lbl_state_userLicense.Text = "Finished"
        ProgressBar3.Value = ProgressBar3.Maximum
        My.Application.DoEvents()

        'GetUserListInGroup()
    End Sub

    Private Function Validate_Response() As Integer

        Dim cmd As New OleDbCommand
        cmd.Connection = Conexion
        Dim iSQL As String = "select * from brs_user_license_assingment"
        cmd.CommandText = iSQL

        Dim da As New OleDbDataAdapter
        Dim dt As New DataTable

        Try
            Conexion.Open()
            da.SelectCommand = cmd
            da.Fill(dt)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudo traer la información desde la tabla brs_cloudpbx", MsgBoxStyle.Exclamation, "Error con base de datos")
            lbl_state_userLicense.Text = "Error with Database"
            ProgressBar3.Value = ProgressBar3.Maximum
            Me.Cursor = Cursors.Default
            Conexion.Close()
            Exit Function
        End Try
        Conexion.Close()
        Dim contador As Integer = 0

        Dim rowCollection As DataRowCollection
        Dim row As DataRow
        rowCollection = dt.Rows

        'DataGridView2.DataSource = dtproxy
        'DataGridView2.Columns.Clear()
        'DataGridView2.Rows.Clear()
        'DataGridView2.Refresh()

        'MsgBox(filasss.ItemArray.ToString)

        'Se muestran los datos en el datagridview2
        For Each row In rowCollection
            If row.ItemArray(0).ToString.Substring(0, 17).Equals("c:SuccessResponse") Then
                'MsgBox(filasss.ItemArray(0).ToString)
                contador += 1
            End If
        Next

        If contador = indexXML_UsersLicense_Assign Then
            'MsgBox("todo correcto")
            Me.Cursor = Cursors.Default
            Return 1
        Else
            MsgBox("No se aplicaron los cambios de desasignación de licencias de usuario", MsgBoxStyle.Exclamation, "Error con base de datos")
            'llamar a un formulario y mostrar que se hizo mal
            Return 0
        End If

    End Function


    Private Sub UnAssignment_UserLicensse()

        If My.Computer.Network.Ping(My.Settings.Host, gblTimePing) Then
            'MsgBox("Server pinged successfully.")
        Else
            MsgBox("Servidor fuera de Linea, favor verifique la conexion", MsgBoxStyle.Exclamation, "Error de Comunicación")
            Me.Cursor = Cursors.Default
            DataGridView2.Enabled = True
            Exit Sub
        End If

        indexXML_UsersLicense_UnAssign = 0

        'indexXML_UsersLicense_Assign = 0
        'lbl_state_userLicense.Text = ""
        'ProgressBar3.Value = 0
        'My.Application.DoEvents()

        'Se eliminan los archivos antiguos del directorio correspondiente
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblPathTmpUserLicense & "\AssigmentUserLicense\UnAssignServices", FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(foundFile)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los archivos antiguos de la carpeta " & gblPathTmpUserLicense & "\AssigmentUserLicense\UnAssignServices" &
                   ", verifique que los archivos no esten siendo utilizados por otro proceso", MsgBoxStyle.Exclamation, "Error al eliminar archivos")
            Me.Cursor = Cursors.Default
            DataGridView2.Enabled = True
            Exit Sub
        End Try

        'XML PARA DESASIGNAR PACK DE SERVICIOS---------------------------------------------------------------------
        Dim line1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim line2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim line3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"

        Dim l_4 As String = "<command xsi:type=" & Chr(34) & "UserServiceUnassignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim l_5 As String = "<userId>226337160@autopro.cl</userId>"
        Dim l_6 As String = "<servicePackName>Pack_Basico</servicePackName>"
        Dim l_7 As String = "</command>"

        Dim finalLine As String = "</BroadsoftDocument>"

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim multipleInputFile As String = gblPathTmpUserLicense & "\AssigmentUserLicense\UnAssignServices\multipleInputFile.txt"
        Dim lineConfigFile As String = ""
        Dim userId As String = ""
        Dim packBasic As String = ""
        Dim packStandard As String = ""
        Dim packAdvanced As String = ""
        Dim UnAssignState As Integer = 0

        numFile = 16
        Dim numFileUserLicense As Integer = numFile

        Try
            FileOpen(numFileUserLicense, multipleInputFile, OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo " & multipleInputFile & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(numFileUserLicense)
            Me.Cursor = Cursors.Default
            DataGridView2.Enabled = True
            Exit Sub
        End Try

        'XML PARA ASIGNAR PACK DE SERVICIOS---------------------------------------------------------------------
        For j = 0 To DataGridView2.RowCount - 1
            Try
                packBasic = DataGridView2.Rows(j).Cells(1).Value.ToString
                packStandard = DataGridView2.Rows(j).Cells(2).Value.ToString
                packAdvanced = DataGridView2.Rows(j).Cells(3).Value.ToString

                If packBasic.Equals("FALSE") Or packStandard.Equals("FALSE") Or packAdvanced.Equals("FALSE") Then

                    UnAssignState = 1

                    numFile = 17
                    indexXML_UsersLicense_UnAssign += 1
                    fileIXML = gblPathTmpUserLicense & "\AssigmentUserLicense\UnAssignServices\" & indexXML_UsersLicense_UnAssign & "_AssignServices_request.xml"
                    fileOXML = gblPathTmpUserLicense & "\AssigmentUserLicense\UnAssignServices\" & indexXML_UsersLicense_UnAssign & "_cloudpbx_response.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, line1.ToCharArray)
                    WriteLine(numFile, line2.ToCharArray)
                    WriteLine(numFile, line3.ToCharArray)
                    WriteLine(numFile, l_4.ToCharArray)

                    userId = DataGridView2.Rows(j).Cells(0).Value.ToString
                    l_5 = "<userId>" & userId & "</userId>"
                    WriteLine(numFile, l_5.ToCharArray)

                    'Realizar prueba de asignar mas de una licencia al usuario

                    If packBasic.Equals("FALSE") Then
                        l_6 = "<servicePackName>Pack_Basico</servicePackName>"
                        WriteLine(numFile, l_6.ToCharArray)
                    End If
                    If packStandard.Equals("FALSE") Then
                        l_6 = "<servicePackName>Pack_Estandar</servicePackName>"
                        WriteLine(numFile, l_6.ToCharArray)
                    End If
                    If packAdvanced.Equals("FALSE") Then
                        l_6 = "<servicePackName>Pack_Avanzado</servicePackName>"
                        WriteLine(numFile, l_6.ToCharArray)
                    End If

                    WriteLine(numFile, l_7.ToCharArray)
                    WriteLine(numFile, finalLine.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(numFileUserLicense, lineConfigFile.ToCharArray)
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & gblPathTmpUserLicense & "\AssigmentUserLicense\UnAssignServices\" & indexXML_UsersLicense_UnAssign & "_AssignServices_request.xml", MsgBoxStyle.Exclamation)
                Me.Cursor = Cursors.Default
                DataGridView2.Enabled = True
                FileClose(numFileUserLicense)
                Exit Sub
            End Try
        Next

        'If UnAssignState = 0 Then
        '    MsgBox("No se establecieron cambios para desasignaar licencias de usuarios", MsgBoxStyle.Exclamation, "Revise los campos 'FALSE'")
        '    Me.Cursor = Cursors.Default
        '    DataGridView2.Enabled = True
        '    FileClose(numFileUserLicense)
        '    Exit Sub
        'End If

        FileClose(numFileUserLicense)

        ExecuteShellBulk(multipleInputFile, 3)

        Me.Cursor = Cursors.Default

        'If codError = 0 Then
        '    ParseXML_UnAssignment_UserLicense()
        'End If

        lbl_state_userLicense.Text = "Finished"
        ProgressBar3.Value = 100
        My.Application.DoEvents()

        'GetUserListInGroup()

    End Sub


    Private Sub ParseXML_UnAssignment_UserLicense()

    End Sub

    Private Sub Btn_BrowseCSV_MouseEnter(sender As Object, e As EventArgs) Handles btn_browse_CSV.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_browse_CSV, "Seleccione un archivo")
    End Sub

    Private Sub Btn_procesar_MouseEnter(sender As Object, e As EventArgs) Handles btn_procesar.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_procesar, "Se procesa y se envía la información")
    End Sub

    Private Sub Btn_report_cloudpbx_MouseEnter(sender As Object, e As EventArgs) Handles btn_report_cloudpbx.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_report_cloudpbx, "Se genera un informe del último CloudPBX procesado")
    End Sub

    Private Sub Btn_show_report_MouseEnter(sender As Object, e As EventArgs) Handles btn_show_report.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_show_report, "Se muestra el reporte del último CloudPBX procesado")
    End Sub
    Private Sub Btn_validate_data_MouseEnter(sender As Object, e As EventArgs) Handles btn_validate_data.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_validate_data, "Se valida localmente la información a procesar")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_search_proxy.Click
        getDeviceName()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btn_process_proxy.Click
        ModifyProxy()
    End Sub

    Private Sub Btn_report_cloudpbx_Click(sender As Object, e As EventArgs) Handles btn_report_cloudpbx.Click
        parseXML_cloudPBX()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles cb_modify_proxy.CheckedChanged

        If cb_modify_proxy.CheckState = 1 Then
            cb_add_proxy.Checked = 0
        ElseIf cb_modify_proxy.CheckState = 0 Then
            cb_add_proxy.Checked = 1
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles cb_add_proxy.CheckedChanged

        If cb_add_proxy.CheckState = 1 Then
            cb_modify_proxy.Checked = 0
        ElseIf cb_add_proxy.CheckState = 0 Then
            cb_modify_proxy.Checked = 1
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        GetUserListInGroup()
    End Sub

    Private Sub Btn_validate_data_Click(sender As Object, e As EventArgs) Handles btn_validate_data.Click
        Validate_Data()
    End Sub

    Private Sub Btn_process_userLicense_Click(sender As Object, e As EventArgs) Handles btn_process_userLicense.Click
        Assignment_UserLicensse()
    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick
        Dim currentCell As String = DataGridView2.CurrentCell.Value.ToString
        If currentCell.Equals("true") Then
            DataGridView2.CurrentCell.Value = "FALSE"
        ElseIf currentCell.Equals("FALSE") Then
            DataGridView2.CurrentCell.Value = "true"

        ElseIf currentCell.Equals("false") Then
            DataGridView2.CurrentCell.Value = "TRUE"
        ElseIf currentCell.Equals("TRUE") Then
            DataGridView2.CurrentCell.Value = "false"
        End If
        'MsgBox(currentCell & "hola")
        'MsgBox(DataGridView2.CurrentCell.Clone().ToString)
    End Sub

    Private Sub Btn_show_report_Click(sender As Object, e As EventArgs) Handles btn_show_report.Click

        If File.Exists(My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & group_id & "_report.txt") Then
            Process.Start("explorer.exe", My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & group_id & "_report.txt")
        Else
            Process.Start("explorer.exe", My.Computer.FileSystem.SpecialDirectories.Desktop)
        End If

    End Sub


End Class