Imports System.Xml
Imports System.IO
Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Imports System.Security.Permissions
Imports System.Net

Public Class Frm_Principal

    Private ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.Database
    Private Conexion As New OleDbConnection(ConexionString)

    Dim gblPathAppl As String
    Dim gblPathLog As String
    Dim gblPathTmpCloud As String
    Dim gblSession As String
    Dim gblTimePing As Integer = 2000
    Dim indexXML_Cloud As Integer = 0
    Dim codError As Integer = 0
    Dim numFile As Integer = 1
    Dim numFileLog As Integer = 4

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

    Public WithEvents FSW As FileSystemWatcher

    'Folders
    Dim Desktop As String = My.Computer.FileSystem.SpecialDirectories.Desktop
    Dim InputFolderPath As String = Desktop & My.Settings.InputFolder
    Dim ErrorFolderPath As String = Desktop & My.Settings.ErrorFolder
    Dim LogsFolderPath As String = Desktop & My.Settings.LogsFolder
    Dim OutputFolderPath As String = Desktop & My.Settings.OutputFolder
    Dim BrsResponseFolderPath As String = Desktop & My.Settings.BrsResponseFolder

    Dim FileName As String
    Dim foundFile As String

    Private Sub For1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        First_Interface()

        CheckForIllegalCrossThreadCalls = True

        FSW = New FileSystemWatcher(InputFolderPath, "*.csv")
        FSW.IncludeSubdirectories = True
        FSW.EnableRaisingEvents = True

        gblPathAppl = My.Application.Info.DirectoryPath & My.Settings.PathAppl
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom
        gblPathLog = My.Application.Info.DirectoryPath & My.Settings.PathLog
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\log
        gblPathTmpCloud = My.Application.Info.DirectoryPath & My.Settings.PathTmpCloud
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp_cloud

        'Dim file_permissions As New FileIOPermission(FileIOPermissionAccess.Read, gblPathTmpCloud)

        'file_permissions.PermitOnly()

    End Sub

    Private Sub FSW_Created(sender As Object, e As FileSystemEventArgs) Handles FSW.Created

        'If btn_report_cloudpbx.InvokeRequired Then

        '    btn_report_cloudpbx.BeginInvoke(Sub() btn_report_cloudpbx.Enabled = True)
        '    My.Application.DoEvents()
        'Else
        '    btn_report_cloudpbx.Enabled = True
        '    My.Application.DoEvents()
        'End If

        System.Threading.Thread.Sleep(3000)

        Dim di As DirectoryInfo = New DirectoryInfo(InputFolderPath)

        For Each file In di.GetFiles("*.csv")

            FileName = file.Name
            foundFile = file.FullName

            If tb_file_name.InvokeRequired Then
                tb_file_name.BeginInvoke(Sub() tb_file_name.Text = file.FullName)
                'tb_file_name.Invoke(Sub() tb_file_name.Text = file.FullName)
            Else
                tb_file_name.Text = file.FullName
            End If

            Validate_File()
        Next

    End Sub

    Private Sub FSW_Changed(sender As Object, e As FileSystemEventArgs) Handles FSW.Changed

    End Sub

    Private Sub FSW_Error(sender As Object, e As ErrorEventArgs) Handles FSW.[Error]

    End Sub

    Private Sub FSW_Renamed(sender As Object, e As RenamedEventArgs) Handles FSW.Renamed

    End Sub

    Private Sub FSW_Disposed(sender As Object, e As EventArgs) Handles FSW.Disposed

    End Sub

    Private Sub FSW_Deleted(sender As Object, e As FileSystemEventArgs) Handles FSW.Deleted

    End Sub

    Private Sub In_Case_Error(Optional ByVal closeFile As Integer = 0)

        If lbl_wait.InvokeRequired Then

            lbl_wait.BeginInvoke(Sub() lbl_wait.Visible = False)
            My.Application.DoEvents()
        Else
            lbl_wait.Visible = False
            My.Application.DoEvents()
        End If

        'Se cierra el archivo de los logs
        FileClose(numFileLog)

        My.Computer.FileSystem.MoveFile(foundFile, ErrorFolderPath & "\" & FileName, FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)

        If closeFile = 1 Then
            FileClose(numFile)
            FileClose(1)
        End If
    End Sub

    Private Sub Validate_File()

        If My.Computer.Network.Ping(My.Settings.Host, gblTimePing) Then
            'MsgBox("Server pinged successfully.")
        Else
            grabaLog(3, 3, 1)
            In_Case_Error()
            Exit Sub
        End If

        If lbl_wait.InvokeRequired Then

            lbl_wait.BeginInvoke(Sub() lbl_wait.Visible = True)
            My.Application.DoEvents()
        Else
            lbl_wait.Visible = True
            My.Application.DoEvents()
        End If

        'En el siguiente método, se valida que:

        'El archivo no se encuentre en uso
        'El archivo no este vacio
        'El archivo posea 26 columnas por cada fila, sin excepción

        Try
            FileOpen(1, foundFile, OpenMode.Input)
        Catch ex As Exception
            grabaLog(1, 4, 2, ex.ToString)
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
                Dim mensaje As String = "El archivo cargado no cuenta con 26 columnas separadas por el caracter ';', como se esperaba."
                grabaLog(1, 4, 3, mensaje)
                FileClose(1)
                In_Case_Error()
                Exit Sub
            End If
        End While

        If controlEmptyFile = 0 Then
            Dim mensaje As String = "El archivo cargado se encuentra vacío"
            grabaLog(1, 4, 3, mensaje)
            FileClose(1)
            In_Case_Error()
            Exit Sub
        End If

        FileClose(1)
        Save_Data_Access()
    End Sub

    Private Sub Save_Data_Access()

        Try
            FileOpen(1, foundFile, OpenMode.Input)
        Catch ex As Exception
            grabaLog(1, 4, 2, ex.ToString)
            FileClose(1)
            In_Case_Error()
            Exit Sub
        End Try

        'Se eliminan los datos antiguos de la tabla brs_cloudpbx
        Dim cmd As New OleDbCommand
        cmd.Connection = Conexion
        Dim instructionSQL As String = "delete * from brs_cloudpbx"
        cmd.CommandText = instructionSQL
        Try
            Conexion.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            grabaLog(1, 1, 7, ex.ToString)
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
            cadenaSQL += " values ( '" & Dominio & "',"
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
                grabaLog(1, 1, 4, ex.ToString)
                FileClose(1)
                Conexion.Close()
                In_Case_Error()
                Exit Sub
            End Try
            Conexion.Close()
        End While

        FileClose(1)

        If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
            lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "")
            My.Application.DoEvents()
        Else
            lbl_state_cloud.Text = ""
            My.Application.DoEvents()
        End If

        If ProgressBar1.ProgressBar.InvokeRequired Then
            ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = 0)
            My.Application.DoEvents()
        Else
            ProgressBar1.Value = 0
            My.Application.DoEvents()
        End If

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
            grabaLog(1, 1, 4, ex.ToString)
            Conexion.Close()
            In_Case_Error()
            Exit Sub
        End Try
        Conexion.Close()

        If DataGridView1.InvokeRequired Then

            'Task.Run(Function() Sub()

            DataGridView1.Invoke(
                    Sub()

                        DataGridView1.DataSource = dt

                        'Se evita que el usuario pueda reordenar la grilla
                        For j = 0 To DataGridView1.ColumnCount - 1
                            DataGridView1.Columns(j).SortMode = DataGridViewColumnSortMode.NotSortable
                        Next
                    End Sub
                    )

            'DataGridView1.BeginInvoke(Sub() DataGridView1.Refresh())

        Else
            DataGridView1.DataSource = dt
            DataGridView1.Refresh()
        End If

        If lbl_wait.InvokeRequired Then

            lbl_wait.BeginInvoke(Sub() lbl_wait.Visible = False)
            My.Application.DoEvents()
        Else
            lbl_wait.Visible = False
            My.Application.DoEvents()
        End If

        Validate_Data()
    End Sub

    Public Sub Validate_Data()

        'Esta variable se usa para controlar que la data supere las pruebas de validación
        estadoCeldas = 0

        Dim prohibited As String
        Dim pattern As String

        'Validación del dominio//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        domain = DataGridView1.Rows(0).Cells(0).Value.ToString.ToLower 'domain = dt.Rows(0)(0).ToString.ToLower

        pattern = "\A[a-no-z0-9.]{1,76}\.(cl|com|org){1}\Z"
        prohibited = "El campo del 'dominio' o 'domain' puede contener hasta 80 caracteres, que pueden ser alfanuméricos (obviando la 'ñ' y los espacios) incluyendo obligatoriamente '.cl' o '.com' o '.org' al final de la expresión."

        If Regex.IsMatch(domain, pattern) Then
            DataGridView1.Rows(0).Cells(0).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(0).ToolTipText = ""
        Else
            DataGridView1.Rows(0).Cells(0).Style.BackColor = Color.FromArgb(254, 84, 97)
            DataGridView1.Rows(0).Cells(0).ToolTipText = prohibited
            estadoCeldas = 1
            grabaLog(1, 4, 3, prohibited)
        End If

        'Validación numeración//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        For j = 0 To DataGridView1.Rows.Count - 1  'dt.Rows.Count - 1

            phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString

            pattern = "\A[0-9]{9}\Z"
            prohibited = "El campo de los 'números de teléfono' o 'numbers' solo aceptan caracteres númericos con un largo de 9 dígitos"

            If Regex.IsMatch(phoneNumber, pattern) Then
                DataGridView1.Rows(j).Cells(1).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(1).ToolTipText = ""

                'validar que no existan datos de ciertas columnas repetidas
                For i = 0 To DataGridView1.RowCount - 1

                    Dim valorComparado As String = DataGridView1.Rows(i).Cells(1).Value.ToString
                    Dim valores As String

                    For k = 0 To DataGridView1.RowCount - 1

                        valores = DataGridView1.Rows(k).Cells(1).Value.ToString

                        If valorComparado.Equals(valores) And i <> k Then

                            prohibited = "El número " & valorComparado & " esta repetido."
                            DataGridView1.Rows(k).Cells(1).Style.BackColor = Color.FromArgb(182, 15, 196)
                            DataGridView1.Rows(k).Cells(1).ToolTipText = prohibited
                            estadoCeldas = 1
                            grabaLog(1, 4, 3, prohibited)
                        End If
                    Next
                Next

            Else
                DataGridView1.Rows(j).Cells(1).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(1).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)

            End If
        Next

        'Validación de información del grupo//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        group_id = DataGridView1.Rows(0).Cells(2).Value.ToString
        group_name = DataGridView1.Rows(0).Cells(3).Value.ToString
        address = DataGridView1.Rows(0).Cells(6).Value.ToString
        city = DataGridView1.Rows(0).Cells(7).Value.ToString

        pattern = "\A[\w]{1,19}(_cloudpbx){1}\Z"
        prohibited = "El campo de 'identificación de grupo' o 'group_id' puede contener hasta 28 caracteres, que pueden ser alfanuméricos (obviando la 'ñ' y los espacios) incluyendo obligatoriamente '_cloudpbx' al final de la expresión."

        If Regex.IsMatch(group_id, pattern) Then
            DataGridView1.Rows(0).Cells(2).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(2).ToolTipText = ""
        Else
            DataGridView1.Rows(0).Cells(2).Style.BackColor = Color.FromArgb(254, 84, 97)
            DataGridView1.Rows(0).Cells(2).ToolTipText = prohibited
            estadoCeldas = 1
            grabaLog(1, 4, 3, prohibited)
        End If

        pattern = "\A(([a-no-zA-NO-Z0-9_]\.{0,1}\s{0,1})){1,80}\Z"
        prohibited = "El campo de 'nombre de grupo' o 'group_name' puede contener hasta 80 caracteres, que pueden ser alfanuméricos (obviando la 'ñ'), puntos, comas y no mas de un espacio consecutivo."

        If Regex.IsMatch(group_name, pattern) Then
            DataGridView1.Rows(0).Cells(3).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(3).ToolTipText = ""
        Else
            DataGridView1.Rows(0).Cells(3).Style.BackColor = Color.FromArgb(254, 84, 97)
            DataGridView1.Rows(0).Cells(3).ToolTipText = prohibited
            estadoCeldas = 1
            grabaLog(1, 4, 3, prohibited)
        End If

        pattern = "\A([\w\,]\s{0,1}){1,80}\Z"
        prohibited = "El campo de 'dirección de empresa' o 'address' puede contener hasta 80 caracteres, que pueden ser alfanuméricos (incluyendo la 'ñ'), comas y no mas de un espacio consecutivo."

        If Regex.IsMatch(address, pattern) Then
            DataGridView1.Rows(0).Cells(6).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(6).ToolTipText = ""
        Else
            DataGridView1.Rows(0).Cells(6).Style.BackColor = Color.FromArgb(254, 84, 97)
            DataGridView1.Rows(0).Cells(6).ToolTipText = prohibited
            estadoCeldas = 1
            grabaLog(1, 4, 3, prohibited)
        End If

        pattern = "\A([\w]\s{0,1}){1,80}\Z"
        prohibited = "El campo 'ciudad' o 'city' puede contener hasta 80 caracteres, que pueden ser alfanuméricos (incluyendo la 'ñ') y no mas de un espacio consecutivo."

        If Regex.IsMatch(city, pattern) Then
            DataGridView1.Rows(0).Cells(7).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(7).ToolTipText = ""
        Else
            DataGridView1.Rows(0).Cells(7).Style.BackColor = Color.FromArgb(254, 84, 97)
            DataGridView1.Rows(0).Cells(7).ToolTipText = prohibited
            estadoCeldas = 1
            grabaLog(1, 4, 3, prohibited)
        End If

        'validar información de los dispositivos//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        'La columna 'device_type' es la referencia para las demas, por ello se valida lo sigte:
        'No puede estar vacia la primera celda
        'No puede haber celdas vacias entremedio

        filasValidas = 0
        'For para saber cantidad de filas no vacias de la columna device_type
        For j = 0 To DataGridView1.Rows.Count - 2
            If DataGridView1.Rows(j).Cells(8).Value.ToString.Length > 0 Then
                filasValidas += 1
            End If
        Next

        For j = 0 To filasValidas - 1
            device_type = DataGridView1.Rows(j).Cells(8).Value.ToString
            mac = DataGridView1.Rows(j).Cells(9).Value.ToString
            serial_number = DataGridView1.Rows(j).Cells(10).Value.ToString
            physical_location = DataGridView1.Rows(j).Cells(11).Value.ToString

            'Si se compara con el signo = un string, no importaran las mayusculas o minusculas
            'If device_type.Equals("Yealink-T19xE2") Or device_type.Equals("Yealink-T21xE2") Or device_type.Equals("Yealink-T27G") Then
            pattern = "\A((Yealink-T19xE2)|(Yealink-T21xE2)|(Yealink-T27G)){1}\Z"
            prohibited = "El campo de 'tipo de dispositivo' o device_type' solo puede contener uno de los siguientes tipos de teléfonos: Yealink-T19xE2 o Yealink-T21xE2 o Yealink-T27G"

            If Regex.IsMatch(device_type, pattern) Then
                DataGridView1.Rows(j).Cells(8).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(8).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(8).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(8).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            pattern = "\A[a-fA-F0-9]{12}\Z"
            prohibited = "El campo de 'dirección mac' o 'mac' debe contener exactamente 12 caracteres, que deben pertenecer al sistema hexadecimal."

            If Regex.IsMatch(mac, pattern) Then
                DataGridView1.Rows(j).Cells(9).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(9).ToolTipText = ""

                'validar que no existan datos de ciertas columnas repetidas
                For i = 0 To filasValidas - 1

                    Dim valorComparado As String = DataGridView1.Rows(i).Cells(9).Value.ToString
                    Dim valores As String
                    'Dim control As Integer = 0

                    For k = 0 To filasValidas - 1

                        valores = DataGridView1.Rows(k).Cells(9).Value.ToString

                        If valorComparado.Equals(valores) And i <> k Then

                            prohibited = "El valor " & valorComparado & " esta repetido."
                            DataGridView1.Rows(k).Cells(9).Style.BackColor = Color.FromArgb(182, 15, 196)
                            DataGridView1.Rows(k).Cells(9).ToolTipText = prohibited
                            estadoCeldas = 1
                            grabaLog(1, 4, 3, prohibited)
                        End If
                    Next
                Next

            Else
                DataGridView1.Rows(j).Cells(9).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(9).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            pattern = "\A[a-no-zA-NO-Z0-9]{16}\Z"
            prohibited = "El campo de 'número de serie' o 'serial_number' debe contener exactamente 16 caracteres que solo pueden ser alfanuméricos (obviando la 'ñ')."

            If Regex.IsMatch(serial_number, pattern) Then
                DataGridView1.Rows(j).Cells(10).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(10).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(10).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(10).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            pattern = "\A[a-no-zA-NO-Z0-9_]{1,12}\Z"
            prohibited = "El campo de 'locación física' o 'physical_location' puede contener hasta 12 caracteres que solo pueden ser alfanuméricos (obviando la 'ñ')."

            If Regex.IsMatch(physical_location, pattern) Then
                DataGridView1.Rows(j).Cells(11).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(11).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(11).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(11).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If
        Next

        'validar información de los usuarios//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
            prohibited = "El campo 'departamento' o 'department' puede contener hasta 80 caracteres, que pueden ser alfanuméricos (incluyendo la 'ñ'), guiones bajos y medios, comas, puntos y no mas de un espacio consecutivo."

            If Regex.IsMatch(department, pattern) Then
                DataGridView1.Rows(j).Cells(12).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(12).ToolTipText = ""

            ElseIf Not Regex.IsMatch(department, pattern) And department.Length = 0 Then
                DataGridView1.Rows(j).Cells(12).Style.BackColor = Color.FromArgb(255, 255, 255)
                DataGridView1.Rows(j).Cells(12).ToolTipText = ""

            ElseIf Not Regex.IsMatch(department, pattern) And department.Length > 0 Then
                DataGridView1.Rows(j).Cells(12).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(12).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
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
            prohibited = "El campo de 'nombre de usuario' o 'first_name' puede contener hasta 30 caracteres, que pueden ser alfanuméricos (obviando la 'ñ'), guiones bajos y medios, puntos y no mas de un espacio consecutivo."

            If Regex.IsMatch(first_name, pattern) Then
                DataGridView1.Rows(j).Cells(13).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(13).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(13).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(13).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            prohibited = "El campo de 'apellido de usuario' o 'last_name' puede contener hasta 30 caracteres, que pueden ser alfanuméricos (obviando la 'ñ'), guiones bajos y medios, puntos y no mas de un espacio consecutivo."

            If Regex.IsMatch(last_name, pattern) Then
                DataGridView1.Rows(j).Cells(14).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(14).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(14).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(14).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            pattern = "\A([\w\,]\s{0,1}){1,80}\Z"
            prohibited = "El campo de 'dirección de usuario' o 'user_address' puede contener hasta 80 caracteres, que pueden ser alfanuméricos (incluyendo la 'ñ'), comas y no mas de un espacio consecutivo."

            If Regex.IsMatch(user_address, pattern) Then
                DataGridView1.Rows(j).Cells(16).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(16).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(16).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(16).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            prohibited = "El campo de 'ciudad de usuario' o 'user_city' puede contener hasta 80 caracteres, que pueden ser alfanuméricos (incluyendo la 'ñ'), comas y no mas de un espacio consecutivo."

            If Regex.IsMatch(user_city, pattern) Then
                DataGridView1.Rows(j).Cells(17).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(17).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(17).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(17).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            pattern = "\A[0-9]{2,5}\Z"
            prohibited = "El campo de 'extensiones' o 'extensions' solo puede contener numeros del 0 a 9 con un largo de 2 a 5 cifras y sin espacios."

            If Regex.IsMatch(extensions, pattern) Then
                DataGridView1.Rows(j).Cells(19).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(19).ToolTipText = ""

                'validar que no existan datos de ciertas columnas repetidas
                For i = 0 To filasValidas - 1

                    Dim valorComparado As String = DataGridView1.Rows(i).Cells(19).Value.ToString
                    Dim valores As String
                    'Dim control As Integer = 0

                    For k = 0 To filasValidas - 1

                        valores = DataGridView1.Rows(k).Cells(19).Value.ToString

                        If valorComparado.Equals(valores) And i <> k Then

                            prohibited = "El valor " & valorComparado & " esta repetido."
                            DataGridView1.Rows(k).Cells(19).Style.BackColor = Color.FromArgb(182, 15, 196)
                            DataGridView1.Rows(k).Cells(19).ToolTipText = prohibited
                            estadoCeldas = 1
                            grabaLog(1, 4, 3, prohibited)
                        End If
                    Next
                Next

            Else
                DataGridView1.Rows(j).Cells(19).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(19).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            pattern = "\A(b|B)loqueado\Z|\A(BLOQUEADO)\Z|\A(d|D)esbloqueado\Z|\A(DESBLOQUEADO)\Z"
            prohibited = "Los campos de 'ocp' u 'outgoing calling plan' solo pueden contener uno de los siguientes textos: bloqueado o Bloqueado o BLOQUEADO o desbloqueado o Desbloqueado o DESBLOQUEADO"

            If Regex.IsMatch(ocp_local, pattern) Then
                DataGridView1.Rows(j).Cells(20).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(20).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(20).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(20).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            If Regex.IsMatch(ocp_tollFree, pattern) Then
                DataGridView1.Rows(j).Cells(21).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(21).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(21).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(21).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            If Regex.IsMatch(ocp_internacional, pattern) Then
                DataGridView1.Rows(j).Cells(22).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(22).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(22).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(22).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            If Regex.IsMatch(ocp_special1, pattern) Then
                DataGridView1.Rows(j).Cells(23).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(23).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(23).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(23).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            If Regex.IsMatch(ocp_special2, pattern) Then
                DataGridView1.Rows(j).Cells(24).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(24).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(24).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(24).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            If Regex.IsMatch(ocp_premium1, pattern) Then
                DataGridView1.Rows(j).Cells(25).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(25).ToolTipText = ""
            Else
                DataGridView1.Rows(j).Cells(25).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(25).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If
        Next

        'INFORMACION OPCIONAL

        contact_name = DataGridView1.Rows(0).Cells(4).Value.ToString
        contact_number = DataGridView1.Rows(0).Cells(5).Value.ToString

        pattern = "\A([\w\.\,\-\/]\s{0,1}){1,30}\Z"
        prohibited = "Los campos 'nombre de contacto' o 'contact_name' y 'número de contacto' o 'contact_number' puede contener hasta 30 caracteres cada uno, que pueden ser alfanuméricos (incluyendo la 'ñ'), puntos, comas, guiones, barras y no mas de un espacio consecutivo."

        'Se llenan los campos y ambos cumplen
        If Regex.IsMatch(contact_name, pattern) And Regex.IsMatch(contact_number, pattern) Then
            DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(4).ToolTipText = ""
            DataGridView1.Rows(0).Cells(5).ToolTipText = ""
            infoContact = 1

            'Se llenan los campos y Niguno cumple
        ElseIf Not Regex.IsMatch(contact_name, pattern) And Not Regex.IsMatch(contact_number, pattern) Then

            'Ninguno cumple (apropósito)
            If contact_name.Length = 0 And contact_number.Length = 0 Then
                DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(255, 255, 255)
                DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(255, 255, 255)
                DataGridView1.Rows(0).Cells(4).ToolTipText = ""
                DataGridView1.Rows(0).Cells(5).ToolTipText = ""
                infoContact = 0

                'Ninguno cumple (Intento fallido)
            Else
                DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(0).Cells(4).ToolTipText = prohibited
                DataGridView1.Rows(0).Cells(5).ToolTipText = prohibited
                infoContact = 0
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)
            End If

            'Solo el primero cumple
        ElseIf Regex.IsMatch(contact_name, pattern) And Not Regex.IsMatch(contact_number, pattern) Then
            DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(254, 84, 97)
            DataGridView1.Rows(0).Cells(4).ToolTipText = ""
            DataGridView1.Rows(0).Cells(5).ToolTipText = prohibited
            infoContact = 0
            estadoCeldas = 1
            grabaLog(1, 4, 3, prohibited)

            'Solo el segundo cumple
        ElseIf Not Regex.IsMatch(contact_name, pattern) And Regex.IsMatch(contact_number, pattern) Then
            DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(254, 84, 97)
            DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(4).ToolTipText = prohibited
            DataGridView1.Rows(0).Cells(5).ToolTipText = ""
            infoContact = 0
            estadoCeldas = 1
            grabaLog(1, 4, 3, prohibited)
        End If

        pattern = "^([a-no-zA-NO-Z0-9_\.\-]+)@([a-no-zA-NO-Z0-9\-]+)((\.[a-no-zA-NO-Z0-9]{2,3})+)$"
        prohibited = "El campo de 'correo electrónico de usuario' o 'user_email' puede contener hasta 30 caracteres, que pueden ser alfanuméricos (obviando la 'ñ'), puntos y guiones."

        For j = 0 To filasValidas - 1

            user_email = DataGridView1.Rows(j).Cells(15).Value.ToString

            If Regex.IsMatch(user_email, pattern) Then
                DataGridView1.Rows(j).Cells(15).Style.BackColor = Color.FromArgb(0, 247, 0)
                DataGridView1.Rows(j).Cells(15).ToolTipText = ""

            ElseIf user_email.Length > 0 Then
                DataGridView1.Rows(j).Cells(15).Style.BackColor = Color.FromArgb(254, 84, 97)
                DataGridView1.Rows(j).Cells(15).ToolTipText = prohibited
                estadoCeldas = 1
                grabaLog(1, 4, 3, prohibited)

            ElseIf user_email.Length = 0 Then
                DataGridView1.Rows(j).Cells(15).Style.BackColor = Color.FromArgb(255, 255, 255)
                DataGridView1.Rows(j).Cells(15).ToolTipText = ""
            End If
        Next

        proxy = DataGridView1.Rows(0).Cells(18).Value.ToString
        pattern = "^(?:(?:[1-9]?[0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}(?:[1-9]?[0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"
        prohibited = "El campo 'proxy' debe cumplir con el formato de una dirección IPv4"

        If Regex.IsMatch(proxy, pattern) Then
            DataGridView1.Rows(0).Cells(18).Style.BackColor = Color.FromArgb(0, 247, 0)
            DataGridView1.Rows(0).Cells(18).ToolTipText = ""
            proxyInfo = 1

        ElseIf proxy.Length > 0 Then
            DataGridView1.Rows(0).Cells(18).Style.BackColor = Color.FromArgb(254, 84, 97)
            DataGridView1.Rows(0).Cells(18).ToolTipText = prohibited
            proxyInfo = 0
            estadoCeldas = 1
            grabaLog(1, 4, 3, prohibited)

        ElseIf proxy.Length = 0 Then
            DataGridView1.Rows(0).Cells(18).Style.BackColor = Color.FromArgb(255, 255, 255)
            DataGridView1.Rows(0).Cells(18).ToolTipText = ""
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

        If estadoCeldas = 0 Then
            CloudPBX()
        Else
            In_Case_Error()
            Exit Sub
        End If
    End Sub

    'Se esta validando nuevamente el codigo se va aqui desde donde se llama al metodo por primera vez
    Private Sub CloudPBX()

        indexXML_Cloud = 0

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

        'definición por código de los valores de la barra de progreso
        'ProgressBar1.Minimum = 0
        'ProgressBar1.Value = 0
        'ProgressBar1.Maximum = 100

        'SearchAllSubDirectories
        Try
            For Each document As String In My.Computer.FileSystem.GetFiles(gblPathTmpCloud, FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(document)
            Next
        Catch ex As Exception
            grabaLog(1, 2, 7, ex.ToString)
            In_Case_Error()
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
            grabaLog(1, 2, 5, ex.ToString)
            FileClose(1)
            In_Case_Error()
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA ASIGNAR EL DOMINIO CREADO----------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA CREAR NUMERACIÓN------------------------------------------------------------------------------
        Try
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
            'DataGridView1.Rows.Count - 1 cuando se muestra el header de la columna
            'DataGridView1.Rows.Count - 2 cuando se muestra el header de la columna y la fila de agregar columna
            For j = 0 To DataGridView1.Rows.Count - 1
                phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
                c_6 = "<phoneNumber>" & phoneNumber & "</phoneNumber>"
                WriteLine(numFile, c_6.ToCharArray)
            Next
            WriteLine(numFile, c_7.ToCharArray)
            WriteLine(numFile, finalLine.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            WriteLine(1, lineConfigFile.ToCharArray)
        Catch ex As Exception
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA CREAR PERFIL DE GRUPO-------------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA MODIFICAR EL LARGO DE LAS EXTENSIONES DE GRUPO--------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA SELECCIONAR SERVICIOS DE GRUPO (ARCHIVO EXTERNO)--------------------------------------------------------------
        indexXML_Cloud += 1
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA ASIGNAR LOS SERVICIOS---------------------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA ASIGNAR LA NUMERACIÓN AL GRUPO----------------------------------------------------------------------------------------
        Try
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
            For j = 0 To DataGridView1.Rows.Count - 2
                phoneNumber = DataGridView1.Rows(j).Cells(1).Value.ToString
                f_7 = "<phoneNumber>+56-" & phoneNumber & "</phoneNumber>"
                WriteLine(numFile, f_7.ToCharArray)
            Next
            WriteLine(numFile, f_8.ToCharArray)
            WriteLine(numFile, finalLine.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            WriteLine(1, lineConfigFile.ToCharArray)
        Catch ex As Exception
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA CREAR LOS DISPOSITIVOS--------------------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA CREAR LOS DEPARTAMENTOS---------------------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA CREAR LOS USUARIOS---------------------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA EL PROXY-----------------------------------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA ASIGNAR DISPOSITIVOS A USUARIOS---------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA ASIGNAR PACK DE SERVICIOS---------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA OCP OUTGOING-CALLING-PLAN------------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA ASIGNAR CONTRASEÑA SIP------------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA ACTIVAR LOS NUMEROS------------------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        'XML PARA ACTIVAR LA MUSICA EN ESPERA DEL GRUPO--------------------------------------------------------------
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
            grabaLog(1, 2, 6, ex.ToString)
            In_Case_Error(1)
            Exit Sub
        End Try

        FileClose(1)

        If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
            lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Processing XML Files...")
            My.Application.DoEvents()
        Else
            lbl_state_cloud.Text = "Processing XML Files..."
            My.Application.DoEvents()
        End If

        If ProgressBar1.ProgressBar.InvokeRequired Then
            ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = 30)
            My.Application.DoEvents()
        Else
            ProgressBar1.Value = 30
            My.Application.DoEvents()
        End If

        ExecuteShellBulk(multipleInputFile, 1)

        If codError = 0 Then

            grabaLog(2, 4, , "Se procesó correctamente el archivo " & FileName.ToString)
            FileClose(numFileLog)
            My.Computer.FileSystem.MoveFile(foundFile, OutputFolderPath & "\" & FileName & " SuccessfullyProcessed " & Format(Now(), "dd-MM-yyyy_hhmmss"), FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)

            If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
                lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Finished")
                My.Application.DoEvents()
            Else
                lbl_state_cloud.Text = "Finished"
                My.Application.DoEvents()
            End If

            If ProgressBar1.ProgressBar.InvokeRequired Then
                ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = 100)
                My.Application.DoEvents()
            Else
                ProgressBar1.Value = 100
                My.Application.DoEvents()
            End If

            ParseXML_cloudPBX()
        End If

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
            grabaLog(1, 6, 9, ex.ToString)
            FileClose(numFile)

            If numberSubroutine = 1 Then
                codError = 1
                In_Case_Error()

                If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
                    lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Error File ociclient.config")
                    My.Application.DoEvents()
                Else
                    lbl_state_cloud.Text = "Error File ociclient.config"
                    My.Application.DoEvents()
                End If

                If ProgressBar1.ProgressBar.InvokeRequired Then
                    ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = 100)
                    My.Application.DoEvents()
                Else
                    ProgressBar1.Value = 100
                    My.Application.DoEvents()
                End If
            End If
            Exit Sub
        End Try

        If numberSubroutine = 1 Then

            If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
                lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Executing App Voxcom...")
                My.Application.DoEvents()
            Else
                lbl_state_cloud.Text = "Executing App Voxcom..."
                My.Application.DoEvents()
            End If

            If ProgressBar1.ProgressBar.InvokeRequired Then
                ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = 50)
                My.Application.DoEvents()
            Else
                ProgressBar1.Value = 50
                My.Application.DoEvents()
            End If
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

            If proceso.HasExited = True And proceso.ExitCode <> 0 Then
                'MsgBox("el usuario cerro la ventana antes de tiempo")
                Dim mensaje As String = "El Shell de windows se cerró antes de tiempo"
                grabaLog(1, 7, 10, mensaje)

                If numberSubroutine = 1 Then
                    codError = 1
                    In_Case_Error()

                    If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
                        lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Error to the execute startASOCIClient.bat")
                        My.Application.DoEvents()
                    Else
                        lbl_state_cloud.Text = "Error to the execute startASOCIClient.bat"
                        My.Application.DoEvents()
                    End If

                    If ProgressBar1.ProgressBar.InvokeRequired Then
                        ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = 100)
                        My.Application.DoEvents()
                    Else
                        ProgressBar1.Value = 100
                        My.Application.DoEvents()
                    End If
                End If

                Exit Sub
            End If

            proceso.Close()

        Catch ex As Exception
            grabaLog(1, 7, 10, ex.ToString & strArguments)

            If numberSubroutine = 1 Then
                codError = 1
                In_Case_Error()

                If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
                    lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Error to the execute startASOCIClient.bat")
                    My.Application.DoEvents()
                Else
                    lbl_state_cloud.Text = "Error to the execute startASOCIClient.bat"
                    My.Application.DoEvents()
                End If

                If ProgressBar1.ProgressBar.InvokeRequired Then
                    ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = 100)
                    My.Application.DoEvents()
                Else
                    ProgressBar1.Value = 100
                    My.Application.DoEvents()
                End If
            End If
            Exit Sub
        End Try
    End Sub

    Private Sub ParseXML_cloudPBX()

        'Se habilita el boton que permite ver el reporte en cualquier momento

        If btn_report_cloudpbx.InvokeRequired Then
            btn_report_cloudpbx.Invoke(Sub() btn_report_cloudpbx.Enabled = True)
            My.Application.DoEvents()
        Else
            btn_report_cloudpbx.Enabled = True
            My.Application.DoEvents()
        End If

        If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
            lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Generating Report...")
            My.Application.DoEvents()
        Else
            lbl_state_cloud.Text = "Generating Report..."
            My.Application.DoEvents()
        End If

        If ProgressBar1.ProgressBar.InvokeRequired Then
            ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = 100)
            My.Application.DoEvents()
        Else
            ProgressBar1.Value = 100
            My.Application.DoEvents()
        End If

        Dim reader As XmlTextReader
        Dim parseXML As String
        Dim response As String = ""
        Dim lineReport As String = ""

        numFile = 5

        Dim name() As String = Split(FileName, ".")

        FileOpen(numFile, BrsResponseFolderPath & "\" & name(0) & "_report.txt", OpenMode.Output, OpenAccess.Write)

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
                    lineReport = response
                    WriteLine(numFile, lineReport.ToCharArray)
                End If

            Catch ex As Exception
                grabaLog(1, 8, , ex.ToString & "Ha ocurrido un error con el archivo respuesta " & gblPathTmpCloud & "\" & num & "_cloudpbx_response.xml")

                If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
                    lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Error to generate report")
                    My.Application.DoEvents()
                Else
                    lbl_state_cloud.Text = "Error to generate report"
                    My.Application.DoEvents()
                End If

                If ProgressBar1.ProgressBar.InvokeRequired Then
                    ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = 100)
                    My.Application.DoEvents()
                Else
                    ProgressBar1.Value = 100
                    My.Application.DoEvents()
                End If
                Exit Sub
            End Try
        Next

        FileClose(numFile)

        If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
            lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Finished")
            My.Application.DoEvents()
        Else
            lbl_state_cloud.Text = lbl_state_cloud.Text = "Finished"
            My.Application.DoEvents()
        End If

        If ProgressBar1.ProgressBar.InvokeRequired Then
            ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = ProgressBar1.Maximum)
            My.Application.DoEvents()
        Else
            ProgressBar1.Value = ProgressBar1.Maximum
            My.Application.DoEvents()
        End If

        System.Threading.Thread.Sleep(1000)
    End Sub


    Public Sub grabaLog(ByVal tipo As Integer, ByVal subtipo As Integer, Optional ByVal tipoMensaje As Integer = 0, Optional ByVal aditionalMessage As String = "")

        If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
            lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Saving log...")
            My.Application.DoEvents()
        Else
            lbl_state_cloud.Text = "Saving log..."
            My.Application.DoEvents()
        End If

        If ProgressBar1.ProgressBar.InvokeRequired Then
            ProgressBar1.ProgressBar.Invoke(Sub() ProgressBar1.Value = 100)
            My.Application.DoEvents()
        Else
            ProgressBar1.Value = 100
            My.Application.DoEvents()
        End If

        Dim message As String = ""
        Dim fileLog As String = ""
        Dim linerr As String = ""

        'tipo de indicación
        If tipo = 1 Then
            linerr += " Error>"

        ElseIf tipo = 2 Then
            linerr += " INFO>"

        ElseIf tipo = 3 Then
            linerr += " WARNING>"

        End If

        'subtipo de indicación
        Select Case subtipo

            Case 1
                linerr += " DB> "
            Case 2
                linerr += " XML> "
            Case 3
                linerr += " CNX> "
            Case 4
                linerr += " CSV> "
            Case 5
                linerr += " MultipleInputFile> "
            Case 6
                linerr += " ociclient.config> "
            Case 7
                linerr += " ExecuteShellBulk> "
            Case 8
                linerr += " BroadsoftResponseReport> "
        End Select

        'tipo de mensaje
        Select Case tipoMensaje

            Case 1
                'My.Computer.FileSystem.MoveFile(foundFile, ErrorFolderPath & "\" & FileName & "_[communication error with the server]_" & Format(Now(), "dd-MM-yyyy_hhmmss"), FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                message = "communication error with the server "
            Case 2
                message = "error trying to open the file "
            Case 3
                message = "file validation error "
            Case 4
                message = "database error "
            Case 5
                message = "data validation error "
            Case 6
                message = "Internal error trying to create xml file "
            Case 7
                message = "Internal error trying to delete old responses from Broadsoft "
            Case 8
                message = "Internal error trying to open multipleInputFile "
            Case 9
                message = "Internal error trying to create file ociclient.config "
            Case 10
                message = "Internal error trying to execute startASOCIClient.bat "
        End Select

        linerr += DateAndTime.Now & " -> " & message & vbNewLine & aditionalMessage

        fileLog = LogsFolderPath & "\" & FileName & "_" & Format(Now(), "dd-MM-yyyy_hhmmss") & ".log"

        'System.Threading.Thread.Sleep(500)

        Try
            FileOpen(numFileLog, fileLog, OpenMode.Append, OpenAccess.Write)

        Catch ex As Exception

        End Try

        WriteLine(numFileLog, linerr.ToCharArray)

        If lbl_state_cloud.GetCurrentParent.InvokeRequired Then
            lbl_state_cloud.GetCurrentParent.Invoke(Sub() lbl_state_cloud.Text = "Log saved")
            My.Application.DoEvents()
        Else
            lbl_state_cloud.Text = "Log saved"
            My.Application.DoEvents()
        End If

    End Sub

    Private Sub Btn_report_cloudpbx_Click(sender As Object, e As EventArgs) Handles btn_report_cloudpbx.Click

        ParseXML_cloudPBX()
        Process.Start("explorer.exe", Desktop & "\brs-response")
    End Sub

    Private Sub Btn_mode_auto_Click(sender As Object, e As EventArgs) Handles btn_fileWatcher.Click

        If FSW.EnableRaisingEvents = False Then
            FSW.EnableRaisingEvents = True
            lbl_status_fileWatcher.Text = "System File Watcher ON"
            My.Application.DoEvents()
        ElseIf FSW.EnableRaisingEvents = True Then
            FSW.EnableRaisingEvents = False
            lbl_status_fileWatcher.Text = "System File Watcher OFF"
            My.Application.DoEvents()
        End If

    End Sub

    Private Sub First_Interface()

        lbl_status_fileWatcher.Text = "System File Watcher ON"
        lbl_wait.Visible = False
        btn_report_cloudpbx.Enabled = False
        lbl_state_cloud.Text = ""
    End Sub

    Public Sub Tooltip_Help_Buttons(ByVal TooltipAyuda As ToolTip, ByVal Boton As Button, ByVal mensaje As String)

        ToolTipHelpButtons.RemoveAll()
        ToolTipHelpButtons.SetToolTip(Boton, mensaje)
        ToolTipHelpButtons.InitialDelay = 500
        ToolTipHelpButtons.IsBalloon = False
    End Sub

    Private Sub Btn_files_processed_Click(sender As Object, e As EventArgs) Handles btn_files_processed.Click
        Process.Start("explorer.exe", Desktop & "\output-csv")
    End Sub

    Private Sub Btn_errors_Click(sender As Object, e As EventArgs) Handles btn_errors.Click
        Process.Start("explorer.exe", Desktop & "\error")
    End Sub

    Private Sub Btn_input_csv_Click(sender As Object, e As EventArgs) Handles btn_input_csv.Click
        Process.Start("explorer.exe", Desktop & "\input-csv")
    End Sub

    Private Sub Btn_show_logs_Click(sender As Object, e As EventArgs) Handles btn_show_logs.Click
        Process.Start("explorer.exe", Desktop & "\logs")
    End Sub

    Private Sub Btn_show_report_Click(sender As Object, e As EventArgs) Handles btn_show_report.Click
        Process.Start("explorer.exe", Desktop & "\brs-response")
    End Sub

    Private Sub Btn_fileWatcher_MouseEnter(sender As Object, e As EventArgs) Handles btn_fileWatcher.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_fileWatcher, "Activar o desactivar la vigilancia del directorio Input-csv")
    End Sub

    Private Sub Btn_input_csv_MouseEnter(sender As Object, e As EventArgs) Handles btn_input_csv.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_input_csv, "Abrir directorio Input-csv")
    End Sub

    Private Sub Btn_errors_MouseEnter(sender As Object, e As EventArgs) Handles btn_errors.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_errors, "Abrir directorio Error")
    End Sub

    Private Sub Btn_files_processed_MouseEnter(sender As Object, e As EventArgs) Handles btn_files_processed.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_files_processed, "Abrir directorio Output-csv")
    End Sub

    Private Sub Btn_show_logs_MouseEnter(sender As Object, e As EventArgs) Handles btn_show_logs.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_show_logs, "Abrir directorio Logs")
    End Sub

    Private Sub Btn_show_report_MouseEnter(sender As Object, e As EventArgs) Handles btn_show_report.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_show_report, "Abrir directorio Report")
    End Sub

    Private Sub Btn_report_cloudpbx_MouseEnter(sender As Object, e As EventArgs) Handles btn_report_cloudpbx.MouseEnter
        Tooltip_Help_Buttons(ToolTipHelpButtons, btn_fileWatcher, "Generar reporte de un CloudPBX provisionado correctamente")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        'My.Computer.Network.DownloadFile("https://i.stack.imgur.com/grqbA.jpg", "C:\Users\cs\Downloads", "anonymous", "")
        'My.Computer.Network.DownloadFile("https://via.placeholder.com/600/92c952", "C:\Users\cs\Downloads", )

        'Dim fileReader As New WebClient()
        'Dim fileAddress = "https://via.placeholder.com/150/92c952.jpg"
        'fileReader.DownloadFile(fileAddress, "C:\Users\cs\Downloads")


        'Dim Data As String
        'Dim Address As String = "https://www.uv.mx/personal/cblazquez/files/2012/01/Sistema-Oseo.pdf"
        'Dim Client1 As WebClient = New WebClient()
        'Dim reader As StreamReader = New StreamReader(Client1.OpenRead(Address))
        'Data = reader.ReadToEnd
        'Client1.Dispose()
        'MsgBox(Data.ToString)


        'My.Computer.Network.DownloadFile("https://via.placeholder.com/600/aa8f2e", "C:\Users\cs\Desktop\foto.jpg", "", "", True, 500, True)
    End Sub

End Class