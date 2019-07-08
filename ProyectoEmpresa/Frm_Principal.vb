Imports System.Xml
Imports System.Data.OleDb

Public Class Frm_Principal

    Dim ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
    Dim Conexion As New OleDbConnection(ConexionString)


    Dim gblSetPathTmp As String
    Dim gblSetPathAppl As String
    Dim gblSetPathLog As String
    Dim gblSetPathTmpCloud As String
    Dim gblSetPathTmpProxy As String
    Dim gblTimePing As Integer = 2000
    Dim gblSession As String = ""
    Dim codError As Integer = 0
    Dim indiceXML As Integer = 0
    Dim indiceXML_DVmac As Integer = 0
    Dim indiceXML_Proxy As Integer = 0
    Dim numFile As Integer = 1
    'Dim n_File As Integer = FreeFile()

    Private Sub For1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Interface_Entrada1()
        interface_entrada2()
        gblSetPathTmpProxy = My.Application.Info.DirectoryPath & My.Settings.SetPathTmpProxy
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp_proxy
        gblSetPathTmpCloud = My.Application.Info.DirectoryPath & My.Settings.SetPathTmpCloud
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp_cloud
        gblSetPathTmp = My.Application.Info.DirectoryPath & My.Settings.SetPathTmp
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp
        gblSetPathAppl = My.Application.Info.DirectoryPath & My.Settings.SetPathAppl
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom
        gblSetPathLog = My.Application.Info.DirectoryPath & My.Settings.SetPathLog
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\log
    End Sub

    Private Sub Interface_Entrada1()

        btn_procesar.Enabled = False
        btn_Browse_CSV.Enabled = True
        Lbl_wait.Visible = False
        Lbl_state.Text = ""
    End Sub

    Private Sub Interface_entrada2()
        TextBox2.Enabled = False
        Button2.Enabled = False
        Label5.Enabled = False
    End Sub


    Public Sub Tooltip_Ayuda_Botones(ByVal TooltipAyuda As ToolTip, ByVal Boton As Button, ByVal mensaje As String)

        ToolTipHelpButtons.RemoveAll()
        ToolTipHelpButtons.SetToolTip(Boton, mensaje)
        ToolTipHelpButtons.InitialDelay = 500
        ToolTipHelpButtons.IsBalloon = False
    End Sub

    Private Sub btn_Browse_CSV_Click(sender As Object, e As EventArgs) Handles btn_Browse_CSV.Click

        openFileDialogCSV.Title = "Seleccione un archivo de extensión .CSV"
        openFileDialogCSV.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop
        openFileDialogCSV.FileName = ""
        'openFileDialogCSV.Filter = "Text files (*.csv)|*.csv|Text files (*.txt)|*.txt"
        openFileDialogCSV.Filter = "Text files (*.csv; *.txt)|*.csv; *.txt"
        openFileDialogCSV.Multiselect = False
        openFileDialogCSV.CheckFileExists = True
        openFileDialogCSV.ShowDialog()
        TextBox_FileName.Text = openFileDialogCSV.FileName
        Lbl_wait.Visible = True
        Me.Cursor = Cursors.WaitCursor
        My.Application.DoEvents()
        validar_Archivo()
    End Sub

    'A continuación, se valida que:
    'Se haya seleccionado un archivo
    'El archivo no se encuentre en uso
    'El archivo no este vacio
    'El archivo posea 26 columnas por cada fila, sin excepción
    Private Sub validar_Archivo()
        'Si no se escogió ningun archivo, se expulsa del metodo validar_Archivo
        If TextBox_FileName.Text = "" Then
            Lbl_wait.Visible = False
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        'Se abre el archivo selccionado en modo lectura y se le asigna un id
        Try
            FileOpen(1, TextBox_FileName.Text, OpenMode.Input)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            Lbl_wait.Visible = False
            Me.Cursor = Cursors.Default
            FileClose(1)
            Exit Sub
        End Try

        'Se valida el formato del archivo
        Dim controlArchivoVacio As Integer = 0

        'Mientras no se llegue al final del archivo
        While Not EOF(1)

            'Si la variable controlArchivoVacio cambia a 1 es porque el while comenzo a recorrer el archivo y por ende este no esta vacio
            controlArchivoVacio = 1

            'Se lee una linea del archivo por cada ejecución
            Dim readLine As String = ""
            Dim arrayLine() As String
            readLine = LineInput(1)
            arrayLine = Split(readLine, ";")

            'Se comprueba que cada linea del archivo contenga 26 columnas por fila
            If arrayLine.Length <> 26 Then
                Lbl_wait.Visible = False
                Me.Cursor = Cursors.Default
                MsgBox("Revise el número de columnas del archivo cargado", MsgBoxStyle.Exclamation, "Error en la estructura del archivo")
                FileClose(1)
                Exit Sub
            End If
        End While

        If controlArchivoVacio = 0 Then
            Me.Cursor = Cursors.Default
            Lbl_wait.Visible = False
            MsgBox("El archivo esta vacio", MsgBoxStyle.Exclamation, "Error en la estructura del archivo")
            FileClose(1)
            Exit Sub
        End If

        FileClose(1)
        save_Data_Access()
    End Sub

    Private Sub save_Data_Access()

        'Se abre el archivo selccionado en modo lectura y se le asigna un id
        Try
            FileOpen(1, TextBox_FileName.Text, OpenMode.Input)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            Me.Cursor = Cursors.Default
            Lbl_wait.Visible = False
            FileClose(1)
            Exit Sub
        End Try

        'Se eliminan los datos antiguos de la tabla brs_cloudPBX
        Dim cmd As New OleDbCommand()
        cmd.Connection = Conexion
        Dim instruccionSql As String = "DELETE * FROM brs_cloudpbx"
        cmd.CommandText = instruccionSql
        Try
            Conexion.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los datos antiguos de la tabla brs_cloudpbx", MsgBoxStyle.Exclamation, "Error de conexión con base de datos")
            Me.Cursor = Cursors.Default
            Lbl_wait.Visible = False
            FileClose(1)
            Conexion.Close()
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

        'Se lee linea por linea el archivo con id = 1, hasta que este acabe, EndOfFile
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

            'Instrucción SQL
            Dim cadenaSql As String = "INSERT INTO brs_cloudpbx ([domain], numbers, group_id, group_name, contact_name, contact_phone, enterprise_address, city,
                                                                        device_type, mac, serial_number, physical_location, deparment_name,
                                                                        first_name, last_name, user_email, user_address, user_city,
                                                                        proxy, extensions,
                                                                        ocp_local, ocp_tollFree, ocp_international, specialServices1, specialServices2, premiumServices1)"
            cadenaSql += " VALUES ( '" & Dominio & "',"
            cadenaSql += "          '" & Numeros & "',"
            cadenaSql += "          '" & Nombre_grupo & "',"
            cadenaSql += "          '" & Nombre_empresa & "',"
            cadenaSql += "          '" & Nombre_contacto & "',"
            cadenaSql += "          '" & Telefono_contacto & "',"
            cadenaSql += "          '" & Direccion_empresa & "',"
            cadenaSql += "          '" & Ciudad & "',"
            cadenaSql += "          '" & Tipo_dispositivo & "',"
            cadenaSql += "          '" & Mac & "',"
            cadenaSql += "          '" & Numero_serie & "',"
            cadenaSql += "          '" & Locacion_fisica & "',"
            cadenaSql += "          '" & Departamento & "',"
            cadenaSql += "          '" & Nombre_usuario & "',"
            cadenaSql += "          '" & Apellido_usuario & "',"
            cadenaSql += "          '" & Correo_usuario & "',"
            cadenaSql += "          '" & Direccion_usuario & "',"
            cadenaSql += "          '" & Ciudad_usuario & "',"
            cadenaSql += "          '" & Proxy & "',"
            cadenaSql += "          '" & Extensiones & "',"
            cadenaSql += "          '" & OCP_local & "',"
            cadenaSql += "          '" & OCP_linea_gratis & "',"
            cadenaSql += "          '" & OCP_internacional & "',"
            cadenaSql += "          '" & OCP_especial1 & "',"
            cadenaSql += "          '" & OCP_especial2 & "',"
            cadenaSql += "          '" & OCP_premium1 & "')"

            'Se crea el comando
            Dim Comando As OleDbCommand = Conexion.CreateCommand()
            Comando.CommandText = cadenaSql

            'Se Ejecuta la consulta de accion (agregar registros)
            Try
                Conexion.Open()
                Comando.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("No se pudieron insertar los nuevos datos en la tabla brs_cloudpbx", MsgBoxStyle.Exclamation, "Error de conexión con base de datos")
                Me.Cursor = Cursors.Default
                Lbl_wait.Visible = False
                FileClose(1)
                Conexion.Close()
                Exit Sub
            End Try
            Conexion.Close()
        End While

        FileClose(1)
        Lbl_state.Text = ""
        ProgressBar1.Value = 0
        update_Grid()
    End Sub


    'dt o dataTable es una variable global para utilizarla desde otros metodos
    Dim dt As New DataTable
    Public Sub update_Grid()

        Dim iSql As String = "select * from brs_cloudpbx"
        Dim cmd As New OleDbCommand
        Dim dt1 As New DataTable
        Dim da As New OleDbDataAdapter
        dt = dt1

        'Se Ejecuta la consulta para traer registros
        Try
            Conexion.Open()
            cmd.Connection = Conexion
            cmd.CommandText = iSql
            da.SelectCommand = cmd
            da.Fill(dt1)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudo traer la información desde la tabla brs_cloudpbx", MsgBoxStyle.Exclamation, "Error de conexión con base de datos")
            Me.Cursor = Cursors.Default
            Lbl_wait.Visible = False
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        'Se muestran los datos en el datagridview 
        DataGridView1.DataSource = dt1
        DataGridView1.Refresh()

        For j = 0 To DataGridView1.ColumnCount - 1
            DataGridView1.Columns(j).SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        'DataGridView1.CurrentCell = DataGridView1.Rows(0).Cells(0)
        'lblCMMUpdCurrentRow.Text = DataGridView1.CurrentCell.RowIndex + 1
        'lblCMMUpdTotalRows.Text = DataGridView1.RowCount

        Lbl_wait.Visible = False
        Me.Cursor = Cursors.Default
        Interface_Salida()
    End Sub

    Private Sub Interface_Salida()
        btn_procesar.Enabled = True
        btn_Browse_CSV.Enabled = True
    End Sub

    Private Sub Error_File()
        FileClose(numFile)
        FileClose(1)
        Me.Cursor = Cursors.Default
        btn_Browse_CSV.Enabled = True
        indiceXML = 0
    End Sub

    Private Sub btn_procesar_Click(sender As Object, e As EventArgs) Handles btn_procesar.Click

        'If My.Computer.Network.Ping(My.Settings.SetHost, gblTimePing) Then
        '    MsgBox("Server pinged successfully.")
        'Else
        '    MsgBox("Servidor fuera de Linea, favor verifique la conexion", MsgBoxStyle.Exclamation, "Error de Comunicación")
        '    Exit Sub
        'End If
        Me.Cursor = Cursors.WaitCursor
        btn_procesar.Enabled = False
        btn_Browse_CSV.Enabled = False

        'FASE 1

        'XML PARA CREAR UN DOMINIO-----------------------------------------------------------------------------------------------------------
        Dim a_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim a_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim a_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim a_4 As String = "<command xsi:type=" & Chr(34) & "SystemDomainAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim a_5 As String = "<domain>pruebacarlos.cl</domain>"
        Dim a_6 As String = "</command>"

        'XML PARA ASIGNAR EL DOMINIO CREADO----------------------------------------------------------------------
        Dim b_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim b_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim b_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim b_4 As String = "<command xsi:type=" & Chr(34) & "ServiceProviderDomainAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim b_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim b_6 As String = "<domain>pruebacarlos.cl</domain>"
        Dim b_7 As String = "</command>"

        'XML PARA CREAR NUMERACIÓN------------------------------------------------------------------------------
        Dim c_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim c_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim c_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim c_4 As String = "<command xsi:type=" & Chr(34) & "ServiceProviderDnAddListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim c_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim c_6 As String = "<phoneNumber>232781567</phoneNumber>"
        Dim c_7 As String = "</command>"

        ' 'XML PARA CREAR PERFIL DE GRUPO-------------------------------------------------------------------------
        Dim d_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim d_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim d_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
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
        Dim e_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim e_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim e_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
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

        'XML PARA ASIGNAR LA NUMERACIÓN----------------------------------------------------------------------------------------
        Dim f_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim f_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim f_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim f_4 As String = "<command xsi:type=" & Chr(34) & "GroupDnAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim f_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim f_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim f_7 As String = "<phoneNumber>+56-232781566</phoneNumber>"
        Dim f_8 As String = "</command>"

        'XML PARA CREAR LOS DISPOSITIVOS---------------------------------------------------------------------------------
        Dim g_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim g_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim g_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
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
        Dim h_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim h_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim h_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim h_4 As String = "<command xsi:type=" & Chr(34) & "GroupDepartmentAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim h_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim h_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim h_7 As String = "<departmentName>Administracion</departmentName>"
        Dim h_8 As String = "</command>"

        'XML PARA CREAR LOS USUARIOS---------------------------------------------------------------------------------
        Dim i_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim i_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim i_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
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
        Dim j_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim j_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim j_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim j_4 As String = "<command xsi:type=" & Chr(34) & "GroupAccessDeviceCustomTagAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim j_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim j_6 As String = "<groupId>AUTOPRO_cloudpbx</groupId>"
        Dim j_7 As String = "<deviceName>DV_805EC02EC440</deviceName>"
        Dim j_8 As String = "<tagName>%SBC_ADDRESS%</tagName>"
        Dim j_9 As String = "<tagValue>172.24.16.211</tagValue>"
        Dim j_10 As String = "</command>"

        'XML PARA ASIGNAR DISPOSITIVOS A USUARIOS---------------------------------------------------------------------
        Dim k_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim k_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim k_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
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
        Dim l_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim l_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim l_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim l_4 As String = "<command xsi:type=" & Chr(34) & "UserServiceAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim l_5 As String = "<userId>226337160@autopro.cl</userId>"
        Dim l_6 As String = "<servicePackName>Pack_Basico</servicePackName>"
        Dim l_7 As String = "</command>"

        'XML PARA OCP OUTGOING-CALLING-PLAN------------------------------------------------------------------------
        Dim m_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim m_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim m_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
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
        Dim n_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim n_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim n_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim n_4 As String = "<command xsi:type=" & Chr(34) & "UserAuthenticationModifyRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim n_5 As String = "<userId>226337160@autopro.cl</userId>"
        Dim n_6 As String = "<userName>226337160</userName>"
        Dim n_7 As String = "<newPassword>XXXXX</newPassword>"
        Dim n_8 As String = "</command>"

        'XML PARA ACTIVAR LOS NUMEROS------------------------------------------------------------------------
        Dim o_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim o_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim o_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim o_4 As String = "<command xsi:type=" & Chr(34) & "GroupDnActivateListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim o_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim o_6 As String = "<groupId>AUTOPRO_cloudpbx</groupId>"
        Dim o_7 As String = "<phoneNumber>+56-226337160</phoneNumber>"
        Dim o_8 As String = "</command>"


        'FASE 2

        'XML PARA MODIFICAR EL LARGO DE LAS EXTENSIONES DE GRUPO--------------------------------------------------------------
        Dim p_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim p_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim p_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim p_4 As String = "<command xsi:type=" & Chr(34) & "GroupExtensionLengthModifyRequest17" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim p_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim p_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim p_7 As String = "<minExtensionLength>2</minExtensionLength>"
        Dim p_8 As String = "<maxExtensionLength>4</maxExtensionLength>"
        Dim p_9 As String = "<defaultExtensionLength>4</defaultExtensionLength>"
        Dim p_10 As String = "</command>"

        'XML PARA ACTIVAR LA MUSICA EN ESPERA DEL GRUPO--------------------------------------------------------------/
        Dim q_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim q_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim q_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
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

        'Ultima linea de todos los XML
        Dim lineaFinal As String = "</BroadsoftDocument>"

        ProgressBar1.Minimum = 0
        ProgressBar1.Value = 0
        ProgressBar1.Maximum = 100

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

        'Esta variable se usa para controlar que todas las validaciones se realizaron correctamente
        Dim estadoCeldas As Integer = 0

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

        'validar dominio-----------------------------------------------------------------------------------------------------
        domain = dt.Rows(0)(0).ToString.ToLower
        If domain.Length <= 3 Then
            DataGridView1.Rows(0).Cells(0).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas += 1
        Else
            DataGridView1.Rows(0).Cells(0).Style.BackColor = Nothing
        End If

        'validar numeración-------------------------------------------------------------------------------------------------
        For j = 0 To dt.Rows.Count - 1
            phoneNumber = dt.Rows(j)(1).ToString
            If phoneNumber.Length <= 8 Then
                DataGridView1.Rows(j).Cells(1).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(1).Style.BackColor = Nothing
            End If
        Next

        'validar información del grupo---------------------------------------------------------------------------------------
        group_id = dt.Rows(0)(2).ToString
        group_name = dt.Rows(0)(3).ToString
        address = dt.Rows(0)(6).ToString
        city = dt.Rows(0)(7).ToString

        If group_id.Length <= 3 Then
            DataGridView1.Rows(0).Cells(2).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas += 1
        Else
            DataGridView1.Rows(0).Cells(2).Style.BackColor = Nothing
        End If

        If group_name.Length <= 3 Then
            DataGridView1.Rows(0).Cells(3).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas += 1
        Else
            DataGridView1.Rows(0).Cells(3).Style.BackColor = Nothing
        End If

        If address.Length <= 3 Then
            DataGridView1.Rows(0).Cells(6).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas += 1
        Else
            DataGridView1.Rows(0).Cells(6).Style.BackColor = Nothing
        End If

        If city.Length <= 3 Then
            DataGridView1.Rows(0).Cells(7).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas += 1
        Else
            DataGridView1.Rows(0).Cells(7).Style.BackColor = Nothing
        End If

        'validar información de los dispositivos-----------------------------------------------------------------------------------

        'La columna 'device_type' es la referencia para las demas, por ello se valida lo sigte:
        'no puede estar vacia la primera celda
        'no puede haber celdas vacias entremedio

        If dt.Rows(0)(8).ToString.Length <= 11 Then
            DataGridView1.Rows(0).Cells(8).Style.BackColor = Color.FromArgb(254, 84, 97)
            estadoCeldas += 1
        Else
            DataGridView1.Rows(0).Cells(8).Style.BackColor = Nothing
        End If

        Dim filasValidas As Integer = 0
        'For para saber cantidad de filas no vacias de la columna device_type (utilizada como referencia)
        For j = 0 To dt.Rows.Count - 1
            If dt.Rows(j)(8).ToString.Length > 0 Then
                filasValidas += 1
            End If
        Next

        For j = 0 To filasValidas - 1
            mac = dt.Rows(j)(9).ToString
            device_type = dt.Rows(j)(8).ToString
            serial_number = dt.Rows(j)(10).ToString
            physical_location = dt.Rows(j)(11).ToString

            If mac.Length <= 11 Then
                DataGridView1.Rows(j).Cells(9).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(9).Style.BackColor = Nothing
            End If

            If device_type.Length <= 11 Then
                DataGridView1.Rows(j).Cells(8).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(8).Style.BackColor = Nothing
            End If

            If serial_number.Length <= 15 Then
                DataGridView1.Rows(j).Cells(10).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(10).Style.BackColor = Nothing
            End If

            If physical_location.Length <= 3 Then
                DataGridView1.Rows(j).Cells(11).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(11).Style.BackColor = Nothing
            End If
        Next


        'validar información de los usuarios------------------------------------------------------------------------------------------
        Dim varAcumulaDepto As String = ""
        Dim arreglo() As String
        Dim arregloDeptos(dt.Rows.Count - 1) As String
        Dim indice As Integer
        Dim numElementos As Integer = 0
        Try
            For i = 0 To dt.Rows.Count - 1
                varAcumulaDepto += dt.Rows(i)(12).ToString & ";"
                'MsgBox(Depto.ToString)
            Next
            arreglo = Split(varAcumulaDepto, ";")
            'MsgBox("Elementos en el arreglo: " & arreglo.Length)
            For k = 0 To arreglo.Length - 1
                indice = Array.IndexOf(arregloDeptos, arreglo(k))
                'MsgBox("El elemento " & arreglo(k) & " arroja: " & indice)
                'Se guarda en arregloDeptos todo aquello que no este repetido y que tenga un largo mayor a cero
                If indice = -1 And arreglo(k).Length > 0 Then
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
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error en el procedimiento de creación de los depatamentos", MsgBoxStyle.Exclamation, "Error de validación")
            Me.Cursor = Cursors.Default
            btn_procesar.Enabled = True
            btn_Browse_CSV.Enabled = True
            Exit Sub
        End Try

        For j = 0 To filasValidas - 1
            first_name = dt.Rows(j)(13).ToString
            last_name = dt.Rows(j)(14).ToString
            user_address = dt.Rows(j)(16).ToString
            user_city = dt.Rows(j)(17).ToString
            extensions = dt.Rows(j)(19).ToString
            ocp_local = dt.Rows(j)(20).ToString
            ocp_tollFree = dt.Rows(j)(21).ToString
            ocp_internacional = dt.Rows(j)(22).ToString
            ocp_special1 = dt.Rows(j)(23).ToString
            ocp_special2 = dt.Rows(j)(24).ToString
            ocp_premium1 = dt.Rows(j)(25).ToString

            If first_name.Length <= 1 Then
                DataGridView1.Rows(j).Cells(13).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(13).Style.BackColor = Nothing
            End If

            If last_name.Length < 1 Then
                DataGridView1.Rows(j).Cells(14).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(14).Style.BackColor = Nothing
            End If

            If user_address.Length <= 3 Then
                DataGridView1.Rows(j).Cells(16).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(16).Style.BackColor = Nothing
            End If

            If user_city.Length <= 3 Then
                DataGridView1.Rows(j).Cells(17).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(17).Style.BackColor = Nothing
            End If

            If extensions.Length <= 1 Then
                DataGridView1.Rows(j).Cells(19).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(19).Style.BackColor = Nothing
            End If

            If ocp_local.Length <= 8 Then
                DataGridView1.Rows(j).Cells(20).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(20).Style.BackColor = Nothing
            End If

            If ocp_tollFree.Length <= 8 Then
                DataGridView1.Rows(j).Cells(21).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(21).Style.BackColor = Nothing
            End If

            If ocp_internacional.Length <= 8 Then
                DataGridView1.Rows(j).Cells(22).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(22).Style.BackColor = Nothing
            End If
            If ocp_special1.Length <= 8 Then
                DataGridView1.Rows(j).Cells(23).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(23).Style.BackColor = Nothing
            End If
            If ocp_special2.Length <= 8 Then
                DataGridView1.Rows(j).Cells(24).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(24).Style.BackColor = Nothing
            End If
            If ocp_premium1.Length <= 8 Then
                DataGridView1.Rows(j).Cells(25).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            Else
                DataGridView1.Rows(j).Cells(25).Style.BackColor = Nothing
            End If
        Next


        'INFORMACION OPCIONAL. A CONTINUACION SE VALIDA QUE:

        'Para añadir contact_name y conctact_number al archivo xml correspondiente:
        'contact_name debe ser mayor a un largo 3 y
        'contact_number deber ser mayor a un largo 8
        'user_email  debe ser mayor a un largo 10
        'proxy sea mayor a un largo 7

        Dim infoContact As Integer = 0
        contact_name = dt.Rows(0)(4).ToString
        contact_number = dt.Rows(0)(5).ToString

        If contact_name.Length <= 3 And contact_number.Length <= 8 Then
            DataGridView1.Rows(0).Cells(4).Style.BackColor = Nothing
            DataGridView1.Rows(0).Cells(5).Style.BackColor = Nothing
            infoContact = 0

        ElseIf contact_name.Length > 3 And contact_number.Length > 8 Then
            DataGridView1.Rows(0).Cells(4).Style.BackColor = Nothing
            DataGridView1.Rows(0).Cells(5).Style.BackColor = Nothing
            infoContact = 1

        ElseIf contact_name.Length > 3 And contact_number.Length <= 8 Then
            DataGridView1.Rows(0).Cells(4).Style.BackColor = Nothing
            DataGridView1.Rows(0).Cells(5).Style.BackColor = Color.FromArgb(254, 84, 97)
            infoContact = 0
            estadoCeldas += 1

        ElseIf contact_name.Length <= 3 And contact_number.Length > 8 Then
            DataGridView1.Rows(0).Cells(4).Style.BackColor = Color.FromArgb(254, 84, 97)
            DataGridView1.Rows(0).Cells(5).Style.BackColor = Nothing
            infoContact = 0
            estadoCeldas += 1
        End If


        For j = 0 To filasValidas - 1
            user_email = dt.Rows(j)(15).ToString
            If user_email.Length = 0 Then
                DataGridView1.Rows(j).Cells(15).Style.BackColor = Nothing
            ElseIf user_email.Length > 10 Then
                DataGridView1.Rows(j).Cells(15).Style.BackColor = Nothing
            ElseIf user_email.Length <= 10 And user_email.Length > 0 Then
                DataGridView1.Rows(j).Cells(15).Style.BackColor = Color.FromArgb(254, 84, 97)
                estadoCeldas += 1
            End If
        Next

        Dim proxyInfo As Integer = 0
        proxy = dt.Rows(0)(18).ToString
        If proxy.Length = 0 Then
            DataGridView1.Rows(0).Cells(18).Style.BackColor = Nothing
            proxyInfo = 0
        ElseIf proxy.Length > 0 And proxy.Length < 7 Then
            DataGridView1.Rows(0).Cells(18).Style.BackColor = Color.FromArgb(254, 84, 97)
            proxyInfo = 0
            estadoCeldas += 1
        ElseIf proxy.Length > 7 Then
            DataGridView1.Rows(0).Cells(18).Style.BackColor = Nothing
            proxyInfo = 1
        End If

        If estadoCeldas <> 0 Then
            Me.Cursor = Cursors.Default
            btn_procesar.Enabled = True
            btn_Browse_CSV.Enabled = True
            'MsgBox("revise las celdas")
            Exit Sub
        End If

        'SearchAllSubDirectories
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblSetPathTmpCloud, FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(foundFile)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los archivos antiguos de la carpeta " & My.Application.Info.DirectoryPath & My.Settings.SetPathTmpCloud &
                   ", verifique que los archivos no esten siendo utilizados por otro proceso", MsgBoxStyle.Exclamation, "Error al eliminar archivos")
            Me.Cursor = Cursors.Default
            btn_Browse_CSV.Enabled = True
            Exit Sub
        End Try

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim estadoArchivo As Integer = 0
        Dim msgError As String = ""
        Dim multipleInputFile As String = gblSetPathTmpCloud & "\multipleInputFile.txt"
        Dim lineConfigFile As String = ""


        Try
            FileOpen(1, multipleInputFile, OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo " & gblSetPathTmp & "\multipleInputFile.txt" & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(1)
            Me.Cursor = Cursors.Default
            btn_Browse_CSV.Enabled = True
            Exit Sub
        End Try


        'XML PARA CREAR UN DOMINIO-----------------------------------------------------------------------------------------------------------
        Try
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_CreateDomain_request_tmp.xml"
            fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, a_1.ToCharArray)
            WriteLine(numFile, a_2.ToCharArray)
            WriteLine(numFile, a_3.ToCharArray)
            WriteLine(numFile, a_4.ToCharArray)
            a_5 = "<domain>" & domain & "</domain>"
            WriteLine(numFile, a_5.ToCharArray)
            WriteLine(numFile, a_6.ToCharArray)
            WriteLine(numFile, lineaFinal.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            WriteLine(1, lineConfigFile.ToCharArray)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al crear el archivo " & indiceXML & "_CreateDomain_request_tmp.xml", MsgBoxStyle.Exclamation)
            Error_File()
            Exit Sub
        End Try
        estadoArchivo = 1

        'XML PARA ASIGNAR EL DOMINIO CREADO----------------------------------------------------------------------
        If estadoArchivo = 1 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_AssignDomain_request_tmp.xml"
                fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, b_1.ToCharArray)
                WriteLine(numFile, b_2.ToCharArray)
                WriteLine(numFile, b_3.ToCharArray)
                WriteLine(numFile, b_4.ToCharArray)
                WriteLine(numFile, b_5.ToCharArray)
                b_6 = "<domain>" & domain & "</domain>"
                WriteLine(numFile, b_6.ToCharArray)
                WriteLine(numFile, b_7.ToCharArray)
                WriteLine(numFile, lineaFinal.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_AssignDomain_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 2
        End If

        'XML PARA CREAR NUMERACIÓN------------------------------------------------------------------------------
        If estadoArchivo = 2 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_CreateNumbers_request_tmp.xml"
                fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, c_1.ToCharArray)
                WriteLine(numFile, c_2.ToCharArray)
                WriteLine(numFile, c_3.ToCharArray)
                WriteLine(numFile, c_4.ToCharArray)
                WriteLine(numFile, c_5.ToCharArray)
                For j = 0 To dt.Rows.Count - 1
                    phoneNumber = dt.Rows(j)(1)
                    c_6 = "<phoneNumber>" & phoneNumber & "</phoneNumber>"
                    WriteLine(numFile, c_6.ToCharArray)
                Next
                WriteLine(numFile, c_7.ToCharArray)
                WriteLine(numFile, lineaFinal.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_CreateNumbers_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 3
        End If

        'XML PARA CREAR PERFIL DE GRUPO-------------------------------------------------------------------------
        If estadoArchivo = 3 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_CreateProfileGroup_request_tmp.xml"
                fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, d_1.ToCharArray)
                WriteLine(numFile, d_2.ToCharArray)
                WriteLine(numFile, d_3.ToCharArray)
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
                WriteLine(numFile, lineaFinal.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_CreateProfileGroup_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 4
        End If

        'XML PARA MODIFICAR EL LARGO DE LAS EXTENSIONES DE GRUPO--------------------------------------------------------------
        If estadoArchivo = 4 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_ExtensionsLength_request_tmp.xml"
                fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, p_1.ToCharArray)
                WriteLine(numFile, p_2.ToCharArray)
                WriteLine(numFile, p_3.ToCharArray)
                WriteLine(numFile, p_4.ToCharArray)
                WriteLine(numFile, p_5.ToCharArray)
                p_6 = "<groupId>" & group_id & "</groupId>"
                WriteLine(numFile, p_6.ToCharArray)
                WriteLine(numFile, p_7.ToCharArray)
                WriteLine(numFile, p_8.ToCharArray)
                WriteLine(numFile, p_9.ToCharArray)
                WriteLine(numFile, p_10.ToCharArray)
                WriteLine(numFile, lineaFinal.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_ExtensionsLength_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 5
        End If

        'XML PARA SELECCIONAR SERVICIOS DE GRUPO (ARCHIVO EXTERNO)--------------------------------------------------------------
        indiceXML += 1
        If estadoArchivo = 5 Then
            Try
                'Lee un archivo, modifica la linea 6
                Dim Lines_Array() As String = IO.File.ReadAllLines(gblSetPathAppl & "\servicesFile_cloud\" & "5_SelectServices_request_tmp.xml")
                Lines_Array(5) = " <groupId>" & group_id & "</groupId>"

                'Se reescribe el archivo con la linea 6 ya editada
                IO.File.WriteAllLines(gblSetPathAppl & "\servicesFile_cloud\" & indiceXML & "_SelectServices_request_tmp.xml", Lines_Array)

                fileIXML = gblSetPathAppl & "\servicesFile_cloud\" & indiceXML & "_SelectServices_request_tmp.xml"
                fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al modificar el archivo " & indiceXML & "_SelectServices_request_tmp.xml", MsgBoxStyle.Exclamation)
                FileClose(1)
                Me.Cursor = Cursors.Default
                btn_Browse_CSV.Enabled = True
                Exit Sub
            End Try
            estadoArchivo = 6
        End If

        'XML PARA ASIGNAR LOS SERVICIOS---------------------------------------------------------------------------------
        If estadoArchivo = 6 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_AssignServices_request_tmp.xml"
                fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, e_1.ToCharArray)
                WriteLine(numFile, e_2.ToCharArray)
                WriteLine(numFile, e_3.ToCharArray)
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
                WriteLine(numFile, lineaFinal.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_AssignServices_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 7
        End If

        'XML PARA ASIGNAR LA NUMERACIÓN----------------------------------------------------------------------------------------
        If estadoArchivo = 7 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_AssignNumber_request_tmp.xml"
                fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, f_1.ToCharArray)
                WriteLine(numFile, f_2.ToCharArray)
                WriteLine(numFile, f_3.ToCharArray)
                WriteLine(numFile, f_4.ToCharArray)
                WriteLine(numFile, f_5.ToCharArray)
                f_6 = "<groupId>" & group_id & "</groupId>"
                WriteLine(numFile, f_6.ToCharArray)
                For j = 0 To dt.Rows.Count - 1
                    phoneNumber = dt.Rows(j)(1)
                    f_7 = "<phoneNumber>+56-" & phoneNumber & "</phoneNumber>"
                    WriteLine(numFile, f_7.ToCharArray)
                Next
                WriteLine(numFile, f_8.ToCharArray)
                WriteLine(numFile, lineaFinal.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_AssignNumber_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 8
        End If

        'XML PARA CREAR LOS DISPOSITIVOS---------------------------------------------------------------------------------
        If estadoArchivo = 8 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_CreateDevice_request_tmp.xml"
                    fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, g_1.ToCharArray)
                    WriteLine(numFile, g_2.ToCharArray)
                    WriteLine(numFile, g_3.ToCharArray)
                    WriteLine(numFile, g_4.ToCharArray)
                    WriteLine(numFile, g_5.ToCharArray)
                    g_6 = "<groupId>" & group_id & "</groupId>"
                    WriteLine(numFile, g_6.ToCharArray)
                    mac = dt.Rows(j)(9).ToString
                    g_7 = "<deviceName>DV_" & mac & "</deviceName>"
                    WriteLine(numFile, g_7.ToCharArray)
                    device_type = dt.Rows(j)(8).ToString
                    g_8 = "<deviceType>" & device_type & "</deviceType>"
                    WriteLine(numFile, g_8.ToCharArray)
                    WriteLine(numFile, g_9.ToCharArray)
                    g_10 = "<macAddress>" & mac & "</macAddress>"
                    WriteLine(numFile, g_10.ToCharArray)
                    serial_number = dt.Rows(j)(10).ToString
                    g_11 = "<serialNumber>" & serial_number & "</serialNumber>"
                    WriteLine(numFile, g_11.ToCharArray)
                    physical_location = dt.Rows(j)(11).ToString
                    g_12 = "<physicalLocation>" & physical_location & "</physicalLocation>"
                    WriteLine(numFile, g_12.ToCharArray)
                    WriteLine(numFile, g_13.ToCharArray)
                    WriteLine(numFile, g_14.ToCharArray)
                    WriteLine(numFile, lineaFinal.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_CreateDevice_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 9
        End If

        'XML PARA CREAR LOS DEPARTAMENTOS---------------------------------------------------------------------------------
        If estadoArchivo = 9 Then
            Try
                For j = 0 To arregloDeptos.Length - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_CreateDepartment_request_tmp.xml"
                    fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, h_1.ToCharArray)
                    WriteLine(numFile, h_2.ToCharArray)
                    WriteLine(numFile, h_3.ToCharArray)
                    WriteLine(numFile, h_4.ToCharArray)
                    WriteLine(numFile, h_5.ToCharArray)
                    h_6 = "<groupId>" & group_id & "</groupId>"
                    WriteLine(numFile, h_6.ToCharArray)
                    department = arregloDeptos(j).ToString
                    h_7 = "<departmentName>" & department & "</departmentName>"
                    WriteLine(numFile, h_7.ToCharArray)
                    WriteLine(numFile, h_8.ToCharArray)
                    WriteLine(numFile, lineaFinal.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_CreateDepartment_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 10
        End If

        'XML PARA CREAR LOS USUARIOS---------------------------------------------------------------------------------
        If estadoArchivo = 10 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_CreateUser_request_tmp.xml"
                    fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, i_1.ToCharArray)
                    WriteLine(numFile, i_2.ToCharArray)
                    WriteLine(numFile, i_3.ToCharArray)
                    WriteLine(numFile, i_4.ToCharArray)
                    WriteLine(numFile, i_5.ToCharArray)
                    i_6 = "<groupId>" & group_id & "</groupId>"
                    WriteLine(numFile, i_6.ToCharArray)
                    phoneNumber = dt.Rows(j)(1)
                    i_7 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
                    WriteLine(numFile, i_7.ToCharArray)
                    last_name = dt.Rows(j)(13)
                    i_8 = "<lastName>" & last_name & "</lastName>"
                    WriteLine(numFile, i_8.ToCharArray)
                    first_name = dt.Rows(j)(14)
                    i_9 = "<firstName>" & first_name & "</firstName>"
                    WriteLine(numFile, i_9.ToCharArray)
                    i_10 = "<callingLineIdLastName>" & last_name & "</callingLineIdLastName>"
                    WriteLine(numFile, i_10.ToCharArray)
                    i_11 = "<callingLineIdFirstName>" & first_name & "</callingLineIdFirstName>"
                    WriteLine(numFile, i_11.ToCharArray)
                    WriteLine(numFile, i_12.ToCharArray)
                    department = dt.Rows(j)(12).ToString
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
                    user_email = dt.Rows(j)(15).ToString
                    If user_email.Length > 0 Then
                        i_20 = "<emailAddress>" & user_email & "</emailAddress>"
                        WriteLine(numFile, i_20.ToCharArray)
                    End If
                    WriteLine(numFile, i_21.ToCharArray)
                    user_address = dt.Rows(j)(16)
                    i_22 = "<addressLine1>" & user_address & "</addressLine1>"
                    WriteLine(numFile, i_22.ToCharArray)
                    user_city = dt.Rows(j)(17)
                    i_23 = "<city>" & user_city & "</city>"
                    WriteLine(numFile, i_23.ToCharArray)
                    WriteLine(numFile, i_24.ToCharArray)
                    WriteLine(numFile, i_25.ToCharArray)
                    WriteLine(numFile, lineaFinal.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_CreateUser_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 11
        End If

        'XML PARA EL PROXY-----------------------------------------------------------------------------------------------
        If estadoArchivo = 11 Then
            Try
                If proxyInfo = 1 Then
                    For j = 0 To filasValidas - 1
                        numFile += 1
                        indiceXML += 1
                        fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_CreateProxy_request_tmp.xml"
                        fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                        FileOpen(numFile, fileIXML, OpenMode.Output)
                        WriteLine(numFile, j_1.ToCharArray)
                        WriteLine(numFile, j_2.ToCharArray)
                        WriteLine(numFile, j_3.ToCharArray)
                        WriteLine(numFile, j_4.ToCharArray)
                        WriteLine(numFile, j_5.ToCharArray)
                        j_6 = "<groupId>" & group_id & "</groupId>"
                        WriteLine(numFile, j_6.ToCharArray)
                        mac = dt.Rows(j)(9)
                        j_7 = "<deviceName>DV_" & mac & "</deviceName>"
                        WriteLine(numFile, j_7.ToCharArray)
                        WriteLine(numFile, j_8.ToCharArray)
                        j_9 = "<tagValue>" & proxy & "</tagValue>"
                        WriteLine(numFile, j_9.ToCharArray)
                        WriteLine(numFile, j_10.ToCharArray)
                        WriteLine(numFile, lineaFinal.ToCharArray)
                        FileClose(numFile)
                        lineConfigFile = fileIXML & ";" & fileOXML
                        WriteLine(1, lineConfigFile.ToCharArray)
                    Next
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_CreateProxy_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 12
        End If

        'XML PARA ASIGNAR DISPOSITIVOS A USUARIOS---------------------------------------------------------------------
        If estadoArchivo = 12 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_AssignUser_request_tmp.xml"
                    fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, k_1.ToCharArray)
                    WriteLine(numFile, k_2.ToCharArray)
                    WriteLine(numFile, k_3.ToCharArray)
                    WriteLine(numFile, k_4.ToCharArray)
                    phoneNumber = dt.Rows(j)(1)
                    k_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
                    WriteLine(numFile, k_5.ToCharArray)
                    k_6 = "<phoneNumber>" & phoneNumber & "</phoneNumber>"
                    WriteLine(numFile, k_6.ToCharArray)
                    extensions = dt.Rows(j)(19)
                    k_7 = "<extension>" & extensions & "</extension>"
                    WriteLine(numFile, k_7.ToCharArray)
                    WriteLine(numFile, k_8.ToCharArray)
                    WriteLine(numFile, k_9.ToCharArray)
                    WriteLine(numFile, k_10.ToCharArray)
                    WriteLine(numFile, k_11.ToCharArray)
                    WriteLine(numFile, k_12.ToCharArray)
                    mac = dt.Rows(j)(9)
                    k_13 = "<deviceName>DV_" & mac & "</deviceName>"
                    WriteLine(numFile, k_13.ToCharArray)
                    WriteLine(numFile, k_14.ToCharArray)
                    k_15 = "<linePort>" & phoneNumber & "@" & domain & "</linePort>"
                    WriteLine(numFile, k_15.ToCharArray)
                    WriteLine(numFile, k_16.ToCharArray)
                    WriteLine(numFile, k_17.ToCharArray)
                    WriteLine(numFile, k_18.ToCharArray)
                    WriteLine(numFile, k_19.ToCharArray)
                    WriteLine(numFile, lineaFinal.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_CreateProxy_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 13
        End If

        'XML PARA ASIGNAR PACK DE SERVICIOS---------------------------------------------------------------------
        If estadoArchivo = 13 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_AssignServices_request_tmp.xml"
                    fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, l_1.ToCharArray)
                    WriteLine(numFile, l_2.ToCharArray)
                    WriteLine(numFile, l_3.ToCharArray)
                    WriteLine(numFile, l_4.ToCharArray)
                    phoneNumber = dt.Rows(j)(1)
                    l_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
                    WriteLine(numFile, l_5.ToCharArray)
                    If dt.Rows(j)(8) = "Yealink-T19xE2" Then
                        l_6 = "<servicePackName>Pack_Basico</servicePackName>"
                    ElseIf dt.Rows(j)(8) = "Yealink-T21xE2" Then
                        l_6 = "<servicePackName>Pack_Estandar</servicePackName>"
                    ElseIf dt.Rows(j)(8) = "Yealink-T27G" Then
                        l_6 = "<servicePackName>Pack_Avanzado</servicePackName>"
                    Else
                        l_6 = "<servicePackName>Pack_Basico</servicePackName>"
                    End If
                    WriteLine(numFile, l_6.ToCharArray)
                    WriteLine(numFile, l_7.ToCharArray)
                    WriteLine(numFile, lineaFinal.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_AssignServices_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 14
        End If

        'XML PARA OCP OUTGOING-CALLING-PLAN------------------------------------------------------------------------
        If estadoArchivo = 14 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_OCP_request_tmp.xml"
                    fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, m_1.ToCharArray)
                    WriteLine(numFile, m_2.ToCharArray)
                    WriteLine(numFile, m_3.ToCharArray)
                    WriteLine(numFile, m_4.ToCharArray)
                    phoneNumber = dt.Rows(j)(1)
                    m_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
                    WriteLine(numFile, m_5.ToCharArray)
                    WriteLine(numFile, m_6.ToCharArray)
                    WriteLine(numFile, m_7.ToCharArray)
                    WriteLine(numFile, m_8.ToCharArray)
                    ocp_local = dt.Rows(j)(20)
                    If ocp_local = "bloqueado" Or ocp_local = "Bloqueado" Then
                        m_9 = "<local>Disallow</local>"
                    ElseIf ocp_local = "desbloqueado" Or ocp_local = "Desbloqueado" Then
                        m_9 = "<local>Allow</local>"
                    End If
                    WriteLine(numFile, m_9.ToCharArray)
                    ocp_tollFree = dt.Rows(j)(21)
                    If ocp_tollFree = "bloqueado" Or ocp_tollFree = "Bloqueado" Then
                        m_10 = "<tollFree>Disallow</tollFree>"
                    ElseIf ocp_tollFree = "desbloqueado" Or ocp_tollFree = "Desbloqueado" Then
                        m_10 = "<tollFree>Allow</tollFree>"
                    End If
                    WriteLine(numFile, m_10.ToCharArray)
                    WriteLine(numFile, m_11.ToCharArray)
                    ocp_internacional = dt.Rows(j)(22)
                    If ocp_internacional = "bloqueado" Or ocp_internacional = "Bloqueado" Then
                        m_12 = "<international>Disallow</international>"
                    ElseIf ocp_internacional = "desbloqueado" Or ocp_internacional = "Desbloqueado" Then
                        m_12 = "<international>Allow</international>"
                    End If
                    WriteLine(numFile, m_12.ToCharArray)
                    WriteLine(numFile, m_13.ToCharArray)
                    WriteLine(numFile, m_14.ToCharArray)
                    ocp_special1 = dt.Rows(j)(23)
                    If ocp_special1 = "bloqueado" Or ocp_special1 = "Bloqueado" Then
                        m_15 = "<specialServicesI>Disallow</specialServicesI>"
                    ElseIf ocp_special1 = "desbloqueado" Or ocp_special1 = "Desbloqueado" Then
                        m_15 = "<specialServicesI>Allow</specialServicesI>"
                    End If
                    WriteLine(numFile, m_15.ToCharArray)
                    ocp_special2 = dt.Rows(j)(24)
                    If ocp_special2 = "bloqueado" Or ocp_special2 = "Bloqueado" Then
                        m_16 = "<specialServicesII>Disallow</specialServicesII>"
                    ElseIf ocp_special2 = "desbloqueado" Or ocp_special2 = "Desbloqueado" Then
                        m_16 = "<specialServicesII>Allow</specialServicesII>"
                    End If
                    WriteLine(numFile, m_16.ToCharArray)
                    ocp_premium1 = dt.Rows(j)(25)
                    If ocp_premium1 = "bloqueado" Or ocp_premium1 = "Bloqueado" Then
                        m_17 = "<premiumServicesI>Disallow</premiumServicesI>"
                    ElseIf ocp_premium1 = "desbloqueado" Or ocp_premium1 = "Desbloqueado" Then
                        m_17 = "<premiumServicesI>Allow</premiumServicesI>"
                    End If
                    WriteLine(numFile, m_17.ToCharArray)
                    WriteLine(numFile, m_18.ToCharArray)
                    WriteLine(numFile, m_19.ToCharArray)
                    WriteLine(numFile, m_20.ToCharArray)
                    WriteLine(numFile, m_21.ToCharArray)
                    WriteLine(numFile, m_22.ToCharArray)
                    WriteLine(numFile, m_23.ToCharArray)
                    WriteLine(numFile, lineaFinal.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_OCP_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 15
        End If

        'XML PARA ASIGNAR CONTRASEÑA SIP------------------------------------------------------------------------
        If estadoArchivo = 15 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_AssignPasswordSIP_request_tmp.xml"
                    fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, n_1.ToCharArray)
                    WriteLine(numFile, n_2.ToCharArray)
                    WriteLine(numFile, n_3.ToCharArray)
                    WriteLine(numFile, n_4.ToCharArray)
                    phoneNumber = dt.Rows(j)(1)
                    n_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
                    WriteLine(numFile, n_5.ToCharArray)
                    n_6 = "<userName>" & phoneNumber & "</userName>"
                    WriteLine(numFile, n_6.ToCharArray)
                    'Dim domi As String = Mid(domain.ToString, 0, 4)
                    Dim letras As String = group_id.Substring(0, 4)
                    n_7 = "<newPassword>" & letras & phoneNumber & "</newPassword>"
                    WriteLine(numFile, n_7.ToCharArray)
                    WriteLine(numFile, n_8.ToCharArray)
                    WriteLine(numFile, lineaFinal.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_AssignPasswordSIP_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 16
        End If

        'XML PARA ACTIVAR LOS NUMEROS------------------------------------------------------------------------
        If estadoArchivo = 16 Then
            Try
                For j = 0 To filasValidas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_ActivateNumber_request_tmp.xml"
                    fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                    FileOpen(numFile, fileIXML, OpenMode.Output)
                    WriteLine(numFile, o_1.ToCharArray)
                    WriteLine(numFile, o_2.ToCharArray)
                    WriteLine(numFile, o_3.ToCharArray)
                    WriteLine(numFile, o_4.ToCharArray)
                    WriteLine(numFile, o_5.ToCharArray)
                    o_6 = "<groupId>" & group_id & "</groupId>"
                    WriteLine(numFile, o_6.ToCharArray)
                    phoneNumber = dt.Rows(j)(1)
                    o_7 = "<phoneNumber>+56-" & phoneNumber & "</phoneNumber>"
                    WriteLine(numFile, o_7.ToCharArray)
                    WriteLine(numFile, o_8.ToCharArray)
                    WriteLine(numFile, lineaFinal.ToCharArray)
                    FileClose(numFile)
                    lineConfigFile = fileIXML & ";" & fileOXML
                    WriteLine(1, lineConfigFile.ToCharArray)
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_ActivateNumber_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
            estadoArchivo = 17
        End If

        'XML PARA ACTIVAR LA MUSICA EN ESPERA DEL GRUPO--------------------------------------------------------------
        If estadoArchivo = 17 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmpCloud & "\" & indiceXML & "_GroupMusicOnHold_request_tmp.xml"
                fileOXML = gblSetPathTmpCloud & "\" & indiceXML & "_cloudpbx_response_.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, q_1.ToCharArray)
                WriteLine(numFile, q_2.ToCharArray)
                WriteLine(numFile, q_3.ToCharArray)
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
                WriteLine(numFile, lineaFinal.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)
            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al crear el archivo " & indiceXML & "_GroupMusicOnHold_request_tmp.xml", MsgBoxStyle.Exclamation)
                Error_File()
                Exit Sub
            End Try
        End If

        FileClose(1)

        Lbl_state.Text = "Procesando el envío de los archivos XML"
        ProgressBar1.Value = ProgressBar1.Value + 30
        My.Application.DoEvents()

        'Exit Sub
        'parseXML_update_CMM(codError, msgError)

        executeShellBulk(multipleInputFile)
        If codError = 0 Then
            parseXML_cloudPBX()
            'My.Application.DoEvents()
        End If
    End Sub

    Public Sub executeShellBulk(ByVal fileMIF As String)

        Dim fileConfig As String = ""
        Dim linregConfig As String = ""
        Dim strArguments As String = ""
        Try
            numFile += 1
            fileConfig = gblSetPathAppl & "\ociclient.config"
            FileOpen(numFile, fileConfig, OpenMode.Output, OpenAccess.Write)

            linregConfig = "userId = " & My.Settings.SetUser
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "password = " & My.Settings.SetPassword
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "hostname = " & My.Settings.SetHost
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "port = " & My.Settings.SetPort
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "sessionID = " & gblSession
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "connectionMode = " & My.Settings.SetMode
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "runMode =  Multiple"
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "multipleInputFile = " & fileMIF
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "pauseTimeBeforeRunStart = 3"
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "numberOfRuns = 1"
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "quietMode = " & My.Settings.SetModeQuit
            'linregConfig = "quietMode = " & "False"
            WriteLine(numFile, linregConfig.ToCharArray)

            linregConfig = "resultOutputFile = " & gblSetPathLog & "\voxTool_UserExtract_" & Format(Now(), "ddMMyyyy_hhmmss") & ".log"
            WriteLine(numFile, linregConfig.ToCharArray)

            FileClose(numFile)
            strArguments &= fileConfig
            'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\ociclient.config
        Catch ex As Exception
            FileClose(numFile)
            MsgBox(ex.ToString)
            MsgBox("Se produjo un error al crear el archivo" & gblSetPathTmpCloud & "\ociclient.config" & " y los archivos XML no fueron enviados", MsgBoxStyle.Exclamation, "Error al crear archivo")
            indiceXML_DVmac = 0
            indiceXML = 0
            codError = 1
            Me.Cursor = Cursors.Default
            btn_Browse_CSV.Enabled = True
            Lbl_state.Text = "Error en archivo ociclient.config"
            ProgressBar1.Value = ProgressBar1.Maximum
            My.Application.DoEvents()
            Exit Sub
        End Try

        Lbl_state.Text = "Ejecutando aplicación Voxcom..."
        'ProgressBar1.Value += 20
        My.Application.DoEvents()

        Try
            Dim proceso As New Process()
            'StartInfo obtiene propiedades que luego se pasan al metodo Proceso.Start()
            proceso.StartInfo.WorkingDirectory = gblSetPathAppl
            proceso.StartInfo.FileName = "startASOCIClient.bat"
            proceso.StartInfo.Arguments = Chr(34) & strArguments & Chr(34)
            proceso.StartInfo.UseShellExecute = True
            proceso.Start()
            proceso.WaitForExit()
            proceso.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
            'grabaLog(1, 3, "Error al ejecutar Shell>" & strArguments)
            indiceXML_DVmac = 0
            indiceXML = 0
            codError = 1
            Me.Cursor = Cursors.Default
            btn_Browse_CSV.Enabled = True
            Lbl_state.Text = "Error al ejecutar startASOCIClient.bat"
            ProgressBar1.Value = ProgressBar1.Maximum
            My.Application.DoEvents()
            Exit Sub
        End Try
    End Sub

    Sub parseXML_cloudPBX()

        Lbl_state.Text = "Generando reporte"
        ProgressBar1.Value += 25
        My.Application.DoEvents()

        Dim reader As XmlTextReader
        Dim parseXMl As String
        Dim response As String = ""

        Dim comando As New OleDbCommand()
        comando.Connection = Conexion
        Dim Sql As String = "DELETE * FROM brs_cloudpbx_response_error"
        comando.CommandText = Sql

        Try
            Conexion.Open()
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al acceder a la base de datos e intentar eliminar los elementos antiguos de la tabla 'brs_cloudpbx_response_error'",
                            MsgBoxStyle.Exclamation, "Error al generar reporte")
            indiceXML = 0
            Lbl_state.Text = "Error al acceder a la base de datos"
            ProgressBar1.Value = ProgressBar1.Maximum
            Me.Cursor = Cursors.Default
            btn_Browse_CSV.Enabled = True
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        For num = 1 To indiceXML
            Try
                parseXMl = gblSetPathTmpCloud & "\" & num & "_cloudpbx_response_.xml"
                reader = New XmlTextReader(parseXMl)
                Do While (reader.Read())
                    Select Case reader.NodeType
                        Case XmlNodeType.Element
                            'If reader.Name = "command" Then
                            '    If reader.HasAttributes Then 'If attributes exist
                            '        While reader.MoveToNextAttribute()
                            '            'Display attribute name and value.
                            '            'MsgBox(reader.Name.ToString & reader.Value.ToString)
                            '            If reader.Name = "xsi:type" Then
                            '                If reader.Value = "c:SuccessResponse" Then
                            '                    'MsgBox("comando exitoso")
                            '                ElseIf reader.Value = "c:ErrorResponse" Then
                            '                    'MsgBox("Error en el comando")
                            '                End If
                            '            End If
                            '        End While
                            '    End If
                            'End If
                            If reader.Name = "summary" Then
                                'MsgBox(reader.ReadString.ToString)
                                response = reader.ReadString & "_[File:" & num & "_cloudpbx_response_.xml]"
                                Dim Sql1 As String = "INSERT INTO brs_cloudpbx_response_error ([error]) VALUES ( '" & response & "')"
                                'Crear un comando
                                Dim Comando1 As OleDbCommand = Conexion.CreateCommand()
                                Comando1.CommandText = Sql1
                                Try
                                    Conexion.Open()
                                    Comando1.ExecuteNonQuery()
                                Catch ex As Exception
                                    MsgBox(ex.ToString)
                                    MsgBox("Error al acceder a la base de datos e intentar agregar registros a la tabla 'brs_cloudpbx_response_error'",
                                                    MsgBoxStyle.Exclamation, "Error al generar reporte")
                                    indiceXML = 0
                                    Lbl_state.Text = "Error al acceder a la base de datos"
                                    ProgressBar1.Value = ProgressBar1.Maximum
                                    Me.Cursor = Cursors.Default
                                    btn_Browse_CSV.Enabled = True
                                    Conexion.Close()
                                    reader.Close()
                                    Exit Sub
                                End Try
                                Conexion.Close()
                            End If
                            'Case XmlNodeType.XmlDeclaration
                    End Select
                Loop
                reader.Close()
            Catch ex As Exception
                MsgBox("Archivo de Respuesta no ha sido encontrado", MsgBoxStyle.Exclamation, "Error al generar reporte")
                'grabaLog(1, 2, "Error al leer archivo XML>" & gblSetPathTmpCloud & "\" & num & "_cloudpbx_response_.xml")
                indiceXML = 0
                Lbl_state.Text = "Error al generar reporte"
                ProgressBar1.Value = ProgressBar1.Maximum
                Me.Cursor = Cursors.Default
                btn_Browse_CSV.Enabled = True
                Conexion.Close()
                Exit Sub
            End Try
        Next

        indiceXML = 0
        Dim FMP As New Frm_Report
        FMP.Show()
        FMP.BringToFront()
        My.Application.DoEvents()
        Me.Cursor = Cursors.Default
        btn_Browse_CSV.Enabled = Enabled
        Lbl_state.Text = "Finalizado"
        ProgressBar1.Value = ProgressBar1.Maximum
        My.Application.DoEvents()
    End Sub

    Public Sub grabaLog(ByVal tipo As Integer, ByVal subtipo As Integer, ByVal mensaje As String)
        Dim fileLog As String = ""
        Dim linerr As String = ""

        linerr = DateAndTime.Now & ">"
        'tipo -> 1=ERRO,2=INFO,3=WARN
        'subtipo -> 1=DB,2=XML,3=CNX
        If tipo = 1 Then
            linerr = linerr & "ERROR>"
        End If
        If tipo = 2 Then
            linerr = linerr & "INFO>"
        End If
        If tipo = 3 Then
            linerr = linerr & "WARNING>"
        End If
        If subtipo = 1 Then
            linerr = linerr & "DB>"
        End If
        If subtipo = 2 Then
            linerr = linerr & "XML>"
        End If
        If subtipo = 2 Then
            linerr = linerr & "CNX>"
        End If
        linerr = linerr & mensaje
        fileLog = gblSetPathLog & "\LOG_" & DateAndTime.DateString & ".log"

        'MsgBox(fileLog.ToString)
        Lbl_state.Text = "Guardando log"
        ProgressBar1.Value = ProgressBar1.Maximum
        My.Application.DoEvents()
        numFile += 1

        FileOpen(numFile, fileLog, OpenMode.Append, OpenAccess.Write)
        WriteLine(numFile, linerr.ToCharArray)
        FileClose(numFile)
    End Sub


    'SEGUNDA INTERFAZ---------------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub getDeviceName()

        If TextBox1.Text.Length > 0 Then
            'TextBox1.Enabled = False
            'Button1.Enabled = False
            Me.Cursor = Cursors.WaitCursor
        Else
            MsgBox("Campo de 'groupId' inválido", MsgBoxStyle.Exclamation, "Error campo de búsqueda")
            Exit Sub
        End If

        indiceXML_DVmac = 0

        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblSetPathTmpProxy & "\getDeviceName", FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(foundFile)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("No se pudieron eliminar los archivos antiguos de la carpeta " & My.Application.Info.DirectoryPath & My.Settings.SetPathTmpProxy & "\getDeviceName" &
                   ", verifique que los archivos no esten siendo utilizados por otro proceso", MsgBoxStyle.Exclamation, "Error al eliminar archivos")
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

        Dim lineaFinal As String = "</BroadsoftDocument>"

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim codError As Integer
        Dim msgError As String = ""
        Dim multipleInputFile As String = gblSetPathTmpProxy & "\getDeviceName\multipleInputFileProxy.txt"
        Dim lineConfigFile As String = ""

        numFile += 1
        Dim numeroArchivo = numFile

        Try
            FileOpen(numeroArchivo, multipleInputFile, OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo" & "gblSetPathTmpProxy" & "\getDeviceName\multipleInputFileProxy.txt" & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(numeroArchivo)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try

        'XML PARA OBTENER LA MAC DE LOS DISPOSITIVOS--------------------------------------------------------------
        Try
            numFile += 1
            indiceXML_DVmac += 1
            fileIXML = gblSetPathTmpProxy & "\getDeviceName\" & indiceXML_DVmac & "_DeviceGetList_request_tmp.xml"
            fileOXML = gblSetPathTmpProxy & "\getDeviceName\" & indiceXML_DVmac & "_cloudpbx_response_.xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, r_1.ToCharArray)
            WriteLine(numFile, r_2.ToCharArray)
            WriteLine(numFile, r_3.ToCharArray)
            WriteLine(numFile, r_4.ToCharArray)
            WriteLine(numFile, r_5.ToCharArray)
            r_6 = "<groupId>" & TextBox1.Text.ToString.ToUpper & "_cloudpbx" & "</groupId>"
            WriteLine(numFile, r_6.ToCharArray)
            WriteLine(numFile, r_7.ToCharArray)
            WriteLine(numFile, r_8.ToCharArray)
            WriteLine(numFile, lineaFinal.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            WriteLine(numeroArchivo, lineConfigFile.ToCharArray)
            FileClose(numeroArchivo)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al crear el archivo " & "\getDeviceName\" & indiceXML_DVmac & "_DeviceGetList_request_tmp.xml", MsgBoxStyle.Exclamation, "Error al crear el archivo")
            FileClose(numFile)
            FileClose(numeroArchivo)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try


        executeShellBulk(multipleInputFile)
        If codError = 0 Then
            parseXML_DvMac(codError, msgError)
            'My.Application.DoEvents()
        End If
    End Sub


    Sub parseXML_DvMac(ByRef codError As Integer, ByRef msgError As String)

        'Lbl_state.Text = "Generando reporte"
        'ProgressBar1.Value += 25
        'My.Application.DoEvents()

        Dim reader As XmlTextReader
        Dim parseXMl As String
        Dim response As String = ""
        Dim comando As New OleDbCommand()
        comando.Connection = Conexion
        Dim Sql As String = "DELETE * FROM brs_proxy_get_dvmac"
        comando.CommandText = Sql

        Try
            Conexion.Open()
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al acceder a la base de datos e intentar eliminar los elementos antiguos de la tabla 'brs_cloudpbx_response_error'",
                            MsgBoxStyle.Exclamation, "Error al generar reporte")
            indiceXML_DVmac = 0
            'Lbl_state.Text = "Error al acceder a la base de datos"
            'ProgressBar1.Value = ProgressBar1.Maximum
            Me.Cursor = Cursors.Default
            Conexion.Close()
        End Try
        Conexion.Close()


        For num = 1 To indiceXML_DVmac
            Try
                parseXMl = gblSetPathTmpProxy & "\getDeviceName\" & num & "_cloudpbx_response_.xml"
                reader = New XmlTextReader(parseXMl)
                Do While (reader.Read())
                    Select Case reader.NodeType
                        Case XmlNodeType.Element

                            'If reader.Name = "command" Then
                            '    If reader.HasAttributes Then 'If attributes exist
                            '        While reader.MoveToNextAttribute()
                            '            'Display attribute name and value.
                            '            'MsgBox(reader.Name.ToString & reader.Value.ToString)
                            '            'If reader.Name = "xsi:type" Then
                            '            If reader.Value = "GroupAccessDeviceGetListResponse" Then

                            '            ElseIf reader.Value = "c:ErrorResponse" Then
                            '                'MsgBox("Error en el comando")
                            '            End If
                            '        End While
                            '    End If
                            'End If

                            'Si no se encuentra el grupo buscado
                            If reader.Name = "summary" Then
                                'MsgBox(reader.ReadString.ToString)
                                response = reader.ReadString
                                MsgBox(response.ToString, MsgBoxStyle.Exclamation)
                                Me.Cursor = Cursors.Default
                                reader.Close()
                                Exit Sub

                            ElseIf reader.Name = "col" Then
                                'MsgBox(reader.ReadString.ToString)
                                response = reader.ReadString
                                If response.Length = 15 Then
                                    'Dim mac As String = response.Substring(3, 12)
                                    Dim cadenaSql As String = "INSERT INTO brs_proxy_get_dvmac (mac_address) VALUES ( '" & response & "')"
                                    Dim Comando1 As OleDbCommand = Conexion.CreateCommand()
                                    Comando1.CommandText = cadenaSql
                                    Try
                                        Conexion.Open()
                                        Comando1.ExecuteNonQuery()
                                    Catch ex As Exception
                                        MsgBox(ex.ToString)
                                        MsgBox("Error al acceder a la base de datos e intentar insertar nuevos elementos en la tabla 'brs_proxy_get_dvmac'",
                            MsgBoxStyle.Exclamation, "Error al generar reporte")
                                        indiceXML_DVmac = 0
                                        Me.Cursor = Cursors.Default
                                        reader.Close()
                                        'Lbl_state.Text = "Error al acceder a la base de datos"
                                        'ProgressBar1.Value = ProgressBar1.Maximum
                                        reader.Close()
                                        Conexion.Close()
                                    End Try
                                    Conexion.Close()
                                End If
                            End If
                            'Case XmlNodeType.XmlDeclaration
                    End Select
                Loop
                reader.Close()
            Catch ex As Exception
                'MsgBox("Archivo de Respuesta no ha sido encontrado!", vbExclamation, "Error")
                MsgBox(ex.ToString)
                grabaLog1(1, 2, "Error al leer archivo XML>" & gblSetPathTmpProxy & "\getDeviceName\" & num & "_cloudpbx_response_.xml")
                codError = 1
                msgError = "Respuesta No Generada"
            End Try
        Next

        indiceXML_DVmac = 0
        actualizarListBox()
        My.Application.DoEvents()
        'Lbl_state.Text = "Finalizado"
        'ProgressBar1.Value = ProgressBar1.Maximum
        'My.Application.DoEvents()
    End Sub

    Public Sub grabaLog1(ByVal tipo As Integer, ByVal subtipo As Integer, ByVal mensaje As String)
        Dim fileLog As String = ""
        Dim linerr As String = ""

        linerr = DateAndTime.Now & ">"
        'tipo -> 1=ERRO,2=INFO,3=WARN
        'subtipo -> 1=DB,2=XML,3=CNX
        If tipo = 1 Then
            linerr = linerr & "ERROR>"
        End If
        If tipo = 2 Then
            linerr = linerr & "INFO>"
        End If
        If tipo = 3 Then
            linerr = linerr & "WARNING>"
        End If
        If subtipo = 1 Then
            linerr = linerr & "DB>"
        End If
        If subtipo = 2 Then
            linerr = linerr & "XML>"
        End If
        If subtipo = 2 Then
            linerr = linerr & "CNX>"
        End If
        linerr = linerr & mensaje
        fileLog = gblSetPathLog & "\LOG_" & DateAndTime.DateString & ".log"


        'MsgBox(fileLog.ToString)
        Lbl_state.Text = "Guardando log"
        My.Application.DoEvents()
        numFile += 1
        Dim numFileLog2 As Integer = numFile
        FileOpen(numFileLog2, fileLog, OpenMode.Append, OpenAccess.Write)
        WriteLine(numFileLog2, linerr.ToCharArray)
        FileClose(numFileLog2)
    End Sub

    'DataTable utilizada para el rebuild de archivos
    Dim dt1 As New DataTable
    Public Sub actualizarListBox()

        Dim iSql As String = "select * from brs_proxy_get_dvmac"
        Dim cmd As New OleDbCommand

        Dim da As New OleDbDataAdapter
        Dim dtproxy As New DataTable
        dt1 = dtproxy

        Try
            Conexion.Open()
            cmd.Connection = Conexion
            cmd.CommandText = iSql
            'cmd.CommandType = CommandType.TableDirect
            da.SelectCommand = cmd
            da.Fill(dtproxy)

            'ListBox1.DataSource = Nothing
            Me.ListBox1.Items.Clear()

            For j = 0 To dtproxy.Rows.Count - 1

                ListBox1.Items.Add(dtproxy.Rows.Item(j)(0).ToString)
            Next

            ListBox1.Refresh()
        Catch ex As Exception
            MsgBox("Can not open connection ! , " & ex.Message)
            Conexion.Close()
        End Try
        Conexion.Close()

        'DataGridView1.CurrentCell = DataGridView1.Rows(0).Cells(0)
        ''lblCMMUpdCurrentRow.Text = DataGridView1.CurrentCell.RowIndex + 1
        ''lblCMMUpdTotalRows.Text = DataGridView1.RowCount
        Label2.Text = "Se encontraron " + dtproxy.Rows.Count.ToString() + " dispositivos" + " en el grupo " + TextBox1.Text

        TextBox2.Enabled = True
        Button2.Enabled = True
        Label5.Enabled = True

        Lbl_wait.Visible = False
        Me.Cursor = Cursors.Default
        Interface_Salida()
        Exit Sub
    End Sub



    Public Sub modificarProxy()

        If TextBox2.Text.Length >= 7 Then
            Me.Cursor = Cursors.WaitCursor
        Else
            MsgBox("Campo de 'proxy' inválido", MsgBoxStyle.Exclamation, "Error campo de búsqueda")
            Exit Sub
        End If

        'XML PARA MODIFICAR PROXY
        Dim j_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim j_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim j_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim j_4 As String = "<command xsi:type=" & Chr(34) & "GroupAccessDeviceCustomTagModifyRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim j_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim j_6 As String = "<groupId>AUTOPRO_cloudpbx</groupId>"
        Dim j_7 As String = "<deviceName>DV_805EC02EC440</deviceName>"
        Dim j_8 As String = "<tagName>%SBC_ADDRESS%</tagName>"
        Dim j_9 As String = "<tagValue>172.24.16.211</tagValue>"
        Dim j_10 As String = "</command>"

        'XML PARA RECONSTRUIR LOS ARCHIVOS DE LOS DISPOSITIVOS
        Dim s_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim s_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim s_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim s_4 As String = "<command xsi:type=" & Chr(34) & "GroupCPEConfigRebuildDeviceConfigFileRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim s_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim s_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim s_7 As String = "<deviceName>DV_805EC0568966</deviceName>"
        Dim s_8 As String = "</command>"

        Dim lineaFinal As String = "</BroadsoftDocument>"

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim estadoArchivo As Integer = 0
        Dim multipleInputFile As String = gblSetPathTmpProxy & "\modifyProxy\multipleInputFile2.txt"
        Dim lineConfigFile As String = ""
        Dim proxy As String = ""
        Dim group_id As String = ""
        Dim dv_mac As String = ""

        numFile += 1
        Dim numFileProxy As Integer = numFile

        Try
            FileOpen(numFileProxy, multipleInputFile, OpenMode.Output, OpenAccess.Write)
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Asegurese de que el archivo " & gblSetPathTmpProxy & "\modifyProxy\multipleInputFile2.txt" & " no este siendo utlizado por otro proceso", MsgBoxStyle.Exclamation, "Error al abrir el archivo")
            FileClose(1)
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try


        Try
            proxy = TextBox2.Text.ToString
            For j = 0 To dt1.Rows.Count - 1
                numFile += 1
                indiceXML_Proxy += 1
                fileIXML = gblSetPathTmpProxy & "\modifyProxy\" & indiceXML_Proxy & "_CreateProxy_request_tmp.xml"
                fileOXML = gblSetPathTmpProxy & "\modifyProxy\" & indiceXML_Proxy & "_cloudpbx_response_.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, j_1.ToCharArray)
                WriteLine(numFile, j_2.ToCharArray)
                WriteLine(numFile, j_3.ToCharArray)
                WriteLine(numFile, j_4.ToCharArray)
                WriteLine(numFile, j_5.ToCharArray)
                group_id = TextBox1.Text.ToString
                j_6 = "<groupId>" & group_id.ToUpper & "_cloudpbx" & "</groupId>"
                WriteLine(numFile, j_6.ToCharArray)
                dv_mac = dt1.Rows(j)(0).ToString
                j_7 = "<deviceName>" & dv_mac & "</deviceName>"
                WriteLine(numFile, j_7.ToCharArray)
                WriteLine(numFile, j_8.ToCharArray)
                proxy = TextBox2.Text.ToString
                j_9 = "<tagValue>" & proxy & "</tagValue>"
                WriteLine(numFile, j_9.ToCharArray)
                WriteLine(numFile, j_10.ToCharArray)
                WriteLine(numFile, lineaFinal.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(numFileProxy, lineConfigFile.ToCharArray)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al crear el archivo " & indiceXML_Proxy & "_CreateProxy_request_tmp.xml", MsgBoxStyle.Exclamation)
            FileClose(numFile)
            FileClose(numFileProxy)
            Exit Sub
        End Try

        Try
            For j = 0 To dt1.Rows.Count - 1
                numFile += 1
                indiceXML_Proxy += 1
                fileIXML = gblSetPathTmpProxy & "\modifyProxy\" & indiceXML_Proxy & "_RebuildDevice_request_tmp.xml"
                fileOXML = gblSetPathTmpProxy & "\modifyProxy\" & indiceXML_Proxy & "_cloudpbx_response_.xml"
                FileOpen(numFile, fileIXML, OpenMode.Output)
                WriteLine(numFile, s_1.ToCharArray)
                WriteLine(numFile, s_2.ToCharArray)
                WriteLine(numFile, s_3.ToCharArray)
                WriteLine(numFile, s_4.ToCharArray)
                WriteLine(numFile, s_5.ToCharArray)
                group_id = TextBox1.Text.ToString
                s_6 = "<groupId>" & group_id.ToUpper & "_cloudpbx" & "</groupId>"
                WriteLine(numFile, s_6.ToCharArray)
                dv_mac = dt1.Rows(j)(0).ToString
                s_7 = "<deviceName>" & dv_mac & "</deviceName>"
                WriteLine(numFile, s_7.ToCharArray)
                WriteLine(numFile, s_8.ToCharArray)
                WriteLine(numFile, lineaFinal.ToCharArray)
                FileClose(numFile)
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(numFileProxy, lineConfigFile.ToCharArray)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al crear el archivo " & indiceXML_Proxy & "_RebuildDevice_request_tmp.xml", MsgBoxStyle.Exclamation)
            FileClose(numFile)
            FileClose(numFileProxy)
            Exit Sub
        End Try


        FileClose(numFileProxy)

        'Exit Sub
        executeShellBulk(multipleInputFile)
        If codError = 0 Then
            parseXML_proxy()
            'My.Application.DoEvents()
        End If

    End Sub

    Private Sub parseXML_proxy()
        'Lbl_state.Text = "Generando reporte"
        'ProgressBar1.Value += 25
        'My.Application.DoEvents()

        Dim reader As XmlTextReader
        Dim parseXMl As String
        Dim response As String = ""

        Dim comando As New OleDbCommand()
        comando.Connection = Conexion
        Dim Sql As String = "DELETE * FROM brs_proxy_response_error"
        comando.CommandText = Sql

        Try
            Conexion.Open()
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
            MsgBox("Error al acceder a la base de datos e intentar eliminar los elementos antiguos de la tabla 'brs_proxy_response_error'",
                            MsgBoxStyle.Exclamation, "Error al generar reporte")
            indiceXML = 0
            Lbl_state.Text = "Error al acceder a la base de datos"
            'ProgressBar1.Value = ProgressBar1.Maximum
            Me.Cursor = Cursors.Default
            Conexion.Close()
            Exit Sub
        End Try
        Conexion.Close()

        For num = indiceXML_DVmac To indiceXML_DVmac + indiceXML_Proxy
            Try
                parseXMl = gblSetPathTmpProxy & "\modifyProxy\" & num & "_cloudpbx_response_.xml"
                reader = New XmlTextReader(parseXMl)
                Do While (reader.Read())
                    Select Case reader.NodeType
                        Case XmlNodeType.Element
                            'If reader.Name = "command" Then
                            '    If reader.HasAttributes Then 'If attributes exist
                            '        While reader.MoveToNextAttribute()
                            '            'Display attribute name and value.
                            '            'MsgBox(reader.Name.ToString & reader.Value.ToString)
                            '            If reader.Name = "xsi:type" Then
                            '                If reader.Value = "c:SuccessResponse" Then
                            '                    'MsgBox("comando exitoso")
                            '                ElseIf reader.Value = "c:ErrorResponse" Then
                            '                    'MsgBox("Error en el comando")
                            '                End If
                            '            End If
                            '        End While
                            '    End If
                            'End If
                            If reader.Name = "summary" Then
                                'MsgBox(reader.ReadString.ToString)
                                response = reader.ReadString & "_[File:" & num & "_cloudpbx_response_.xml]"
                                Dim Sql1 As String = "INSERT INTO brs_proxy_response_error ([error]) VALUES ( '" & response & "')"
                                'Crear un comando
                                Dim Comando1 As OleDbCommand = Conexion.CreateCommand()
                                Comando1.CommandText = Sql1
                                Try
                                    Conexion.Open()
                                    Comando1.ExecuteNonQuery()
                                Catch ex As Exception
                                    MsgBox(ex.ToString)
                                    MsgBox("Error al acceder a la base de datos e intentar agregar registros a la tabla 'brs_proxy_response_error'",
                                                    MsgBoxStyle.Exclamation, "Error al generar reporte")
                                    indiceXML_Proxy = 0
                                    Lbl_state.Text = "Error al acceder a la base de datos"
                                    'ProgressBar1.Value = ProgressBar1.Maximum
                                    Me.Cursor = Cursors.Default
                                    Conexion.Close()
                                    reader.Close()
                                    Exit Sub
                                End Try
                                Conexion.Close()
                            End If
                            'Case XmlNodeType.XmlDeclaration
                    End Select
                Loop
                reader.Close()
            Catch ex As Exception
                MsgBox("Archivo de Respuesta no ha sido encontrado", MsgBoxStyle.Exclamation, "Error al generar reporte")
                'grabaLog(1, 2, "Error al leer archivo XML>" & gblSetPathTmpCloud & "\" & num & "_cloudpbx_response_.xml")
                indiceXML = 0
                Lbl_state.Text = "Error al generar reporte"
                'ProgressBar1.Value = ProgressBar1.Maximum
                Me.Cursor = Cursors.Default
                Conexion.Close()
                Exit Sub
            End Try
        Next

        indiceXML_Proxy = 0
        'Dim FMP As New Frm_Report
        'FMP.Show()
        'FMP.BringToFront()
        'My.Application.DoEvents()
        Me.Cursor = Cursors.Default
        'Lbl_state.Text = "Finalizado"
        'ProgressBar1.Value = ProgressBar1.Maximum
        My.Application.DoEvents()
    End Sub




    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub Btn_BrowseCSV_MouseEnter(sender As Object, e As EventArgs) Handles btn_Browse_CSV.MouseEnter
        Tooltip_Ayuda_Botones(ToolTipHelpButtons, btn_Browse_CSV, "Seleccione un archivo")
    End Sub

    Private Sub Btn_procesar_MouseEnter(sender As Object, e As EventArgs) Handles btn_procesar.MouseEnter
        Tooltip_Ayuda_Botones(ToolTipHelpButtons, btn_procesar, "Procesar y enviar la información")
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        getDeviceName()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        modificarProxy()
    End Sub

    Private Sub TabPage2_Click(sender As Object, e As EventArgs) Handles TabPage2.Click

    End Sub

End Class