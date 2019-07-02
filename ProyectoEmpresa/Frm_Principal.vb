Imports System.Xml
Imports System.Data.OleDb
Imports System
Imports System.IO
Imports System.Collections

Public Class Frm_Principal

    Dim ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
    Dim Conexion As New OleDbConnection(ConexionString)

    Dim indiceXML As Integer = 0
    Dim gblSetPathTmp As String
    Dim gblSetPathAppl As String
    Dim gblSetPathLog As String
    Dim gblTimePing As Integer = 2000
    Dim gblSession As String = ""

    Private Sub For1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Interface_Entrada()

        gblSetPathTmp = My.Application.Info.DirectoryPath & My.Settings.SetPathTmp
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp
        gblSetPathAppl = My.Application.Info.DirectoryPath & My.Settings.SetPathAppl
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom
        gblSetPathLog = My.Application.Info.DirectoryPath & My.Settings.SetPathLog
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\log
    End Sub

    Private Sub Interface_Entrada()
        btn_procesar.Enabled = False
        btn_BrowseCSV.Enabled = True
        Lab_wait.Visible = False
        'contabilización de filas y colummnas
        lblCMMUpdCurrentRow.Visible = False
        lblCMMUpdTotalRows.Visible = False
        LblEstado.Text = ""
    End Sub

    Public Sub TooltipAyudaBotones(ByVal TooltipAyuda As ToolTip, ByVal Boton As Button, ByVal mensaje As String)
        ToolTipHelpButtons.RemoveAll()
        ToolTipHelpButtons.SetToolTip(Boton, mensaje)
        ToolTipHelpButtons.InitialDelay = 500
        ToolTipHelpButtons.IsBalloon = False
    End Sub

    Private Sub btn_BrowseCSV_Click(sender As Object, e As EventArgs) Handles btn_BrowseCSV.Click

        'Se le pasan algunos parametros al openFileDialog
        openFileDialogCSV.Title = "Seleccione un archivo de extensión .CSV"
        openFileDialogCSV.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop
        openFileDialogCSV.FileName = ""
        'openFileDialogCSV.Filter = "Text files (*.csv)|*.csv|Text files (*.txt)|*.txt"
        openFileDialogCSV.Filter = "Text files (*.csv; *.txt)|*.csv; *.txt"
        openFileDialogCSV.Multiselect = False
        openFileDialogCSV.CheckFileExists = True
        openFileDialogCSV.ShowDialog()
        TextBox_FileName.Text = openFileDialogCSV.FileName
        'Lab_wait.Visible = True
        Me.Cursor = Cursors.WaitCursor
        GuardarDatosEnAccess()
    End Sub

    Sub GuardarDatosEnAccess()

        'Si no se escogió ningun archivo, se cancela la llamada al metodo
        If TextBox_FileName.Text = "" Then
            Lab_wait.Visible = False
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        Try
            'Se abre el archivo CSV selccionado en modo lectura y se le asigna un id
            FileOpen(1, TextBox_FileName.Text, OpenMode.Input)
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Exclamation)
            MsgBox("Asegurese de que el archivo no este siendo utlizado por otro proceso", MsgBoxStyle.Information)
            Me.Cursor = Cursors.Default
            FileClose(1)
            Exit Sub
        End Try

        Dim readLine As String = ""
        Dim arrayLine() As String

        'Variables que contendrán las valores a guardar en access
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

        'Se eliminan los datos antiguos de la tabla broadsoft_cloudPBX
        Dim cmd As New OleDbCommand()
        cmd.Connection = Conexion
        Dim instruccionSql As String = "DELETE * FROM broadsoft_cloudPBX"
        cmd.CommandText = instruccionSql

        Try
            Conexion.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Conexion.Close()

        'Validar el formato del archivo
        Dim controlNumColummnas As Integer = 0
        Dim controlArchivoVacio As Integer = 0

        'Se lee linea por linea el archivo con id = 1, hasta que este acabe, EndOfFile
        While Not EOF(1)

            'Lee una linea del archivo
            readLine = LineInput(1)
            arrayLine = Split(readLine, ";")

            'Comprueba que la primera linea del archivo contenga 26 columnas
            If arrayLine.Length <> 26 And controlNumColummnas = 0 Then
                MsgBox("Revisa el número de columnas del archivo cargado", MsgBoxStyle.Exclamation)
                FileClose(1)
                Me.Cursor = Cursors.Default
                Exit Sub
            End If

            'Si el programa modifica la variable controlArchivoVacio a 1, significa
            'que ingreso al while donde se lee el archivo y por ende este no esta vacio
            controlArchivoVacio = 1

            'La matriz debe ser cudrada
            Dominio = arrayLine(0).ToString()
            Numeros = arrayLine(1).ToString()
            Nombre_grupo = arrayLine(2).ToString()
            Nombre_empresa = arrayLine(3).ToString()
            Nombre_contacto = arrayLine(4).ToString()
            Telefono_contacto = arrayLine(5).ToString()
            Direccion_empresa = arrayLine(6).ToString()
            Ciudad = arrayLine(7).ToString()
            Tipo_dispositivo = arrayLine(8).ToString()
            Mac = arrayLine(9).ToString()
            Numero_serie = arrayLine(10).ToString()
            Locacion_fisica = arrayLine(11).ToString()
            Departamento = arrayLine(12).ToString()
            Nombre_usuario = arrayLine(13).ToString()
            Apellido_usuario = arrayLine(14).ToString()
            Correo_usuario = arrayLine(15).ToString()
            Direccion_usuario = arrayLine(16).ToString()
            Ciudad_usuario = arrayLine(17).ToString()
            Proxy = arrayLine(18).ToString()
            Extensiones = arrayLine(19).ToString()
            OCP_local = arrayLine(20).ToString()
            OCP_linea_gratis = arrayLine(21).ToString()
            OCP_internacional = arrayLine(22).ToString()
            OCP_especial1 = arrayLine(23).ToString()
            OCP_especial2 = arrayLine(24).ToString()
            OCP_premium1 = arrayLine(25).ToString()

            'Se modifica el valor de esta variable para que no se realice una validacion del numero de columnas
            'a partir de la segunda linea del archivo
            controlNumColummnas = 1

            'Se debe modificar el tipo de dato, de numero entero largo a -> doble
            'Access no redondea cifras automaticamente si estas estan en formato general y si no superan los 16 caracteres
            'Numeros = Convert.ToInt64(arrayLine(1))
            'MsgBox(Dominio & " " & Numeros.ToString())

            'Instrucción SQL
            'Se insertan datos en los campos domain y numbers de la tabla brs_create_group
            Dim cadenaSql As String = "INSERT INTO broadsoft_cloudPBX ([domain], numbers, group_id, group_name, contact_name, contact_phone, enterprise_address, city,
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

            'Crear un comando
            Dim Comando As OleDbCommand = Conexion.CreateCommand()
            Comando.CommandText = cadenaSql

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

        'Si el valor de controlArchivoVacio no cambio a 1, quiere decir que se salto el bucle While
        'debido a que el archivo estaba vacio
        If controlArchivoVacio = 0 Then
            MsgBox("El archivo esta vacio", MsgBoxStyle.Exclamation)
            FileClose(1)
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        FileClose(1)
        LblEstado.Text = ""
        ProgressBar1.Value = 0
        actualizarGrilla()
    End Sub


    'Se utilizan como variables globales para trabajar con access
    Dim da As New OleDbDataAdapter
    Dim cmd As New OleDbCommand
    Dim dt As New DataTable 'para trabajar con una tabla DataTAble y para trabajar con un conjunto de tablas DataSet

    Public Sub actualizarGrilla()

        Dim iSql As String = "select * from broadsoft_cloudPBX"
        Try
            Conexion.Open()
            cmd.Connection = Conexion
            cmd.CommandText = iSql
            da.SelectCommand = cmd
            da.Fill(dt)

            'Se muestran los datos en el datagridview 
            DataGridView1.DataSource = dt
            DataGridView1.Refresh()
        Catch ex As Exception
            MsgBox("Can not open connection ! , " & ex.Message)
        End Try
        Conexion.Close()

        'DataGridView1.CurrentCell = DataGridView1.Rows(0).Cells(0)
        'lblCMMUpdCurrentRow.Text = DataGridView1.CurrentCell.RowIndex + 1
        'lblCMMUpdTotalRows.Text = DataGridView1.RowCount

        Lab_wait.Visible = False
        Me.Cursor = Cursors.Default
        Interface_Salida()
    End Sub

    Private Sub BtnCMMOpenFile_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Interface_Salida()
        btn_procesar.Enabled = True
        btn_BrowseCSV.Enabled = True
    End Sub

    Private Sub btn_procesar_Click(sender As Object, e As EventArgs) Handles btn_procesar.Click

        Dim mensaje As String = ""
        Dim j As Integer = 0

        'If My.Computer.Network.Ping(My.Settings.SetHost, gblTimePing) Then
        '    'MsgBox("Server pinged successfully.")
        'Else
        '    MsgBox("Servidor fuera de Linea, favor verifique conexion!!!", vbOKOnly, "Error de Comunicación")
        '    Exit Sub
        'End If

        'Val("    38205 (Distrito Norte)")devuelve 38205 como valor numérico. Los espacios y el resto de cadena
        'a partir de donde no se puede reconocer un valor numérico se ignora, Si la cadena empieza con contenido no numérico Val devuelve cero.
        'If Val(lblCMMUpdTotalRows.Text) = 0 Then
        '    mensaje = "No existen datos en el archivo ..."
        '    Exit Sub
        'Else
        '    mensaje = lblCMMUpdTotalRows.Text & " registros a actualizar ..."
        'End If
        ''vbCrLf para un salto de linea
        'mensaje &= vbCrLf & vbCrLf & "Presione Aceptar para confirmar actualización..."
        'If MsgBox(mensaje, MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton2, "Confirmación") = MsgBoxResult.Cancel Then
        '    Exit Sub
        'End If


        'FASE 1

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para SystemDomainAddRequest                    |
        '\\\\\\\\\\\\\\\\\\\\/////////////////////////////////
        Dim a_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim a_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim a_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim a_4 As String = "<command xsi:type=" & Chr(34) & "SystemDomainAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim a_5 As String = "<domain>pruebacarlos.cl</domain>"
        Dim a_6 As String = "</command>"

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para ServiceProviderDomainAssignListRequest    |
        '\\\\\\\\\\\\\\\\\\\\/////////////////////////////////
        Dim b_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim b_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim b_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim b_4 As String = "<command xsi:type=" & Chr(34) & "ServiceProviderDomainAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim b_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim b_6 As String = "<domain>pruebacarlos.cl</domain>"
        Dim b_7 As String = "</command>"

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para ServiceProviderDnAddListRequest           |
        '\\\\\\\\\\\\\\\\\\\\/////////////////////////////////
        Dim c_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim c_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim c_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim c_4 As String = "<command xsi:type=" & Chr(34) & "ServiceProviderDnAddListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim c_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim c_6 As String = "<phoneNumber>232781567</phoneNumber>"
        Dim c_7 As String = "</command>"

        '/////////////////////\\\\\\\\\\\\\\\\
        '| XML para GroupAddRequest           |
        '\\\\\\\\\\\\\\\\\\\\/////////////////
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

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para GroupServiceAssignListRequest     |
        '\\\\\\\\\\\\\\\\\\\\/////////////////////////
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

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para GroupDnAssignListRequest          |
        '\\\\\\\\\\\\\\\\\\\\/////////////////////////
        Dim f_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim f_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim f_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim f_4 As String = "<command xsi:type=" & Chr(34) & "GroupDnAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim f_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim f_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim f_7 As String = "<phoneNumber>+56-232781566</phoneNumber>"
        Dim f_8 As String = "</command>"

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para GroupAccessDeviceAddRequest14     |
        '\\\\\\\\\\\\\\\\\\\\/////////////////////////
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

        '/////////////////////\\\\\\\\\\\\\\\\\\\
        '| XML para GroupDepartmentAddRequest    |
        '\\\\\\\\\\\\\\\\\\\\////////////////////
        Dim h_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim h_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim h_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim h_4 As String = "<command xsi:type=" & Chr(34) & "GroupDepartmentAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim h_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim h_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim h_7 As String = "<departmentName>Administracion</departmentName>"
        Dim h_8 As String = "</command>"

        '/////////////////////\\\\\\\\\\\\\\\
        '| XML para "UserAddRequest17sp4"    |
        '\\\\\\\\\\\\\\\\\\\\////////////////
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


        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para "GroupAccessDeviceCustomTagAddRequest"     |
        '\\\\\\\\\\\\\\\\\\\\//////////////////////////////////
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


        '/////////////////////\\\\\\\\\\\\\\\\\\\
        '| XML para "UserModifyRequest17sp4"     |
        '\\\\\\\\\\\\\\\\\\\\////////////////////
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


        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para "UserServiceAssignListRequest"     |
        '\\\\\\\\\\\\\\\\\\\\//////////////////////////
        Dim l_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim l_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim l_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim l_4 As String = "<command xsi:type=" & Chr(34) & "UserServiceAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim l_5 As String = "<userId>226337160@autopro.cl</userId>"
        Dim l_6 As String = "<servicePackName>Pack_Basico</servicePackName>"
        Dim l_7 As String = "</command>"


        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para "UserOutgoingCallingPlanOriginatingModifyRequest"    |
        '\\\\\\\\\\\\\\\\\\\\////////////////////////////////////////////
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


        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para "UserAuthenticationModifyRequest"   |
        '\\\\\\\\\\\\\\\\\\\\///////////////////////////
        Dim n_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim n_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim n_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim n_4 As String = "<command xsi:type=" & Chr(34) & "UserAuthenticationModifyRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim n_5 As String = "<userId>226337160@autopro.cl</userId>"
        Dim n_6 As String = "<userName>226337160</userName>"
        Dim n_7 As String = "<newPassword>XXXXX</newPassword>"
        Dim n_8 As String = "</command>"


        '/////////////////////\\\\\\\\\\\\\\\\\\\\\
        '| XML para "GroupDnActivateListRequest"   |
        '\\\\\\\\\\\\\\\\\\\\//////////////////////
        Dim o_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim o_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim o_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim o_4 As String = "<command xsi:type=" & Chr(34) & "GroupDnActivateListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim o_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim o_6 As String = "<groupId>AUTOPRO_cloudpbx</groupId>"
        Dim o_7 As String = "<phoneNumber>+56-226337160</phoneNumber>"
        Dim o_8 As String = "</command>"


        'FASE 2

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para "GroupExtensionLengthModifyRequest17"   |
        '\\\\\\\\\\\\\\\\\\\\///////////////////////////////
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

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para "GroupMusicOnHoldModifyInstanceRequest20"   |
        '\\\\\\\\\\\\\\\\\\\\///////////////////////////////////
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
        'ProgressBar1.Maximum = Val(lblCMMUpdTotalRows.Text)

        'SearchAllSubDirectories
        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblSetPathTmp, FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                My.Computer.FileSystem.DeleteFile(foundFile)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try


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

        LblEstado.Text = "Generando archivos XML..."
        ProgressBar1.Value = ProgressBar1.Value + 10
        My.Application.DoEvents()

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim estadoArchivo As Integer = 0
        Dim codError As Integer
        Dim msgError As String = ""
        Dim multipleInputFile As String = gblSetPathTmp & "\multipleInputFile.txt"
        Dim lineConfigFile As String = ""
        Dim numFile As Integer = 1


        FileOpen(1, multipleInputFile, OpenMode.Output, OpenAccess.Write)

        'validar la informacion obligatoria

        'validar dominio---------------------------------------------------------------------------------------------------
        domain = dt.Rows(0)(0).ToString
        If domain = "" Or domain.Length = 0 Then
            MsgBox("Revise el campo 'domain'", MsgBoxStyle.Exclamation)
        End If

        'validar numeración-------------------------------------------------------------------------------------------------
        phoneNumber = dt.Rows(j)(1).ToString
        For j = 0 To dt.Rows.Count - 1
            phoneNumber = dt.Rows(j)(1).ToString
            If phoneNumber = "" Or phoneNumber.Length <= 8 Then
                MsgBox("Revise el campo 'numbers'", MsgBoxStyle.Exclamation)
            End If
        Next

        'validar información del grupo------------------------------------------------------------------------------------------
        group_id = dt.Rows(0)(2).ToString
        group_name = dt.Rows(0)(3).ToString
        address = dt.Rows(0)(6).ToString
        city = dt.Rows(0)(7).ToString

        If group_id = "" Or group_id.Length = 0 Then
            MsgBox("Revise el campo 'group_id'", MsgBoxStyle.Exclamation)
        End If

        If group_name = "" Or group_name.Length = 0 Then
            MsgBox("Revise el campo 'group_name", MsgBoxStyle.Exclamation)
        End If

        If address = "" Or address.Length = 0 Then
            MsgBox("Revise el campo 'enterprise_adress'", MsgBoxStyle.Exclamation)
        End If

        If city = "" Or city.Length = 0 Then
            MsgBox("Revise el campo 'city'", MsgBoxStyle.Exclamation)
        End If


        'validar información de los dispositivos------------------------------------------------------------------------------------------
        Dim contadorFilas As Integer = 0
        'For para saber cantidad de filas desde device_type hacia adelante
        For j = 0 To dt.Rows.Count - 1
            If dt.Rows(j)(8).ToString.Length > 0 Then
                contadorFilas += 1
            Else
                Exit For
            End If
        Next

        For j = 0 To contadorFilas - 1
            mac = dt.Rows(j)(9).ToString
            device_type = dt.Rows(j)(8).ToString
            serial_number = dt.Rows(j)(10).ToString
            physical_location = dt.Rows(j)(11).ToString

            If mac = "" Or mac.Length = 0 Then
                MsgBox("Revise el campo 'mac'", MsgBoxStyle.Exclamation)
            End If
            If device_type = "" Or device_type.Length = 0 Then
                MsgBox("Revise el campo 'device_type'", MsgBoxStyle.Exclamation)
            End If
            If serial_number = "" Or serial_number.Length = 0 Then
                MsgBox("Revise el campo 'serial_number'", MsgBoxStyle.Exclamation)
            End If
            If physical_location = "" Or physical_location.Length = 0 Then
                MsgBox("Revise el campo 'physical_location'", MsgBoxStyle.Exclamation)
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
                If indice = -1 And arreglo(k) <> "" And arreglo(k).Length > 0 Then
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
        End Try


        For j = 0 To contadorFilas - 1
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

            If first_name = "" Or first_name.Length = 0 Then
                MsgBox("Revise el campo 'first_name'", MsgBoxStyle.Exclamation)
            End If
            If last_name = "" Or last_name.Length = 0 Then
                MsgBox("Revise el campo 'last_name'", MsgBoxStyle.Exclamation)
            End If
            If user_address = "" Or user_address.Length = 0 Then
                MsgBox("Revise el campo 'user_address'", MsgBoxStyle.Exclamation)
            End If
            If user_city = "" Or user_city.Length = 0 Then
                MsgBox("Revise el campo 'user_city'", MsgBoxStyle.Exclamation)
            End If
            If extensions = "" Or extensions.Length = 0 Then
                MsgBox("Revise el campo 'extensions'", MsgBoxStyle.Exclamation)
            End If
            If ocp_local = "" Or ocp_local.Length = 0 Then
                MsgBox("Revise el campo 'ocp_local'", MsgBoxStyle.Exclamation)
            End If
            If ocp_tollFree = "" Or ocp_tollFree.Length = 0 Then
                MsgBox("Revise el campo 'ocp_tollFree'", MsgBoxStyle.Exclamation)
            End If
            If ocp_internacional = "" Or ocp_internacional.Length = 0 Then
                MsgBox("Revise el campo 'ocp_internacional'", MsgBoxStyle.Exclamation)
            End If
            If ocp_special1 = "" Or ocp_special1.Length = 0 Then
                MsgBox("Revise el campo 'ocp_special1'", MsgBoxStyle.Exclamation)
            End If
            If ocp_special2 = "" Or ocp_special2.Length = 0 Then
                MsgBox("Revise el campo 'ocp_special2'", MsgBoxStyle.Exclamation)
            End If
            If ocp_premium1 = "" Or ocp_premium1.Length = 0 Then
                MsgBox("Revise el campo 'ocp_premium1'", MsgBoxStyle.Exclamation)
            End If
        Next


        'XML PARA CREAR UN DOMINIO-----------------------------------------------------------------------------------------------------------
        Try
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & indiceXML & "_CreateDomain_request_tmp.xml"
            fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
            FileClose(1)
            Exit Sub
        End Try
            estadoArchivo = 1


        'XML PARA ASIGNAR EL DOMINIO CREADO----------------------------------------------------------------------
        If estadoArchivo = 1 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmp & "\" & indiceXML & "_AssignDomain_request_tmp.xml"
                fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 2
        End If


        'XML PARA CREAR NUMERACIÓN------------------------------------------------------------------------------
        If estadoArchivo = 2 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmp & "\" & indiceXML & "_CreateNumbers_request_tmp.xml"
                fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 3
        End If


        'XML PARA CREAR PERFIL DE GRUPO-------------------------------------------------------------------------
        If estadoArchivo = 3 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmp & "\" & indiceXML & "_CreateProfileGroup_request_tmp.xml"
                fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                'WriteLine(numFile, d_11.ToCharArray)
                contact_name = dt.Rows(0)(4).ToString
                contact_number = dt.Rows(0)(5).ToString
                If contact_name <> "" And contact_name.Length > 0 And contact_number <> "" And contact_number.Length > 0 Then
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 4
        End If


        'XML PARA MODIFICAR EL LARGO DE LAS EXTENSIONES DE GRUPO--------------------------------------------------------------
        If estadoArchivo = 4 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmp & "\" & indiceXML & "_ExtensionsLength_request_tmp.xml"
                fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 5
        End If


        'XML PARA SELECCIONAR SERVICIOS DE GRUPO (ARCHIVO EXTERNO)--------------------------------------------------------------
        numFile += 1
        indiceXML += 1
        If estadoArchivo = 5 Then
            Try
                'Lee un archivo, modifica la linea 6
                Dim Lines_Array() As String = IO.File.ReadAllLines(gblSetPathAppl & "\arch_permanent\" & "5_SelectServices_request_tmp.xml")
                Lines_Array(5) = " <groupId>" & group_id & "</groupId>"

                'Se reescribe el archivo con la linea 6 ya editada
                IO.File.WriteAllLines(gblSetPathAppl & "\arch_permanent\" & indiceXML & "_SelectServices_request_tmp.xml", Lines_Array)

                fileIXML = gblSetPathAppl & "\arch_permanent\" & indiceXML & "_SelectServices_request_tmp.xml"
                fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
                lineConfigFile = fileIXML & ";" & fileOXML
                WriteLine(1, lineConfigFile.ToCharArray)

            Catch ex As Exception
                MsgBox(ex.ToString)
                MsgBox("Error al modificar el archivo " & indiceXML & "_SelectServices_request_tmp.xml", MsgBoxStyle.Exclamation)
                Exit Sub
            End Try
            estadoArchivo = 6
        End If


        'XML PARA ASIGNAR LOS SERVICIOS---------------------------------------------------------------------------------
        If estadoArchivo = 6 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmp & "\" & indiceXML & "_AssignServices_request_tmp.xml"
                fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 7
        End If

        'XML PARA ASIGNAR LA NUMERACIÓN----------------------------------------------------------------------------------------
        If estadoArchivo = 7 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmp & "\" & indiceXML & "_AssignNumber_request_tmp.xml"
                fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 8
        End If


        'XML PARA CREAR LOS DISPOSITIVOS---------------------------------------------------------------------------------
        If estadoArchivo = 8 Then
            Try
                For j = 0 To contadorFilas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmp & "\" & indiceXML & "_CreateDevice_request_tmp.xml"
                    fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
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
                    fileIXML = gblSetPathTmp & "\" & indiceXML & "_CreateDepartment_request_tmp.xml"
                    fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 10
        End If


        'XML PARA CREAR LOS USUARIOS---------------------------------------------------------------------------------
        If estadoArchivo = 10 Then
            Try
                For j = 0 To contadorFilas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmp & "\" & indiceXML & "_CreateUser_request_tmp.xml"
                    fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                    department = dt.Rows(j)(12)
                    If department <> "" And department.Length <> 0 And department <> Nothing Then
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
                    user_email = dt.Rows(j)(15)
                    If user_email <> "" And user_email.Length <> 0 And user_email <> Nothing Then
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 11
        End If


        'XML PARA EL PROXY-----------------------------------------------------------------------------------------------
        If estadoArchivo = 11 Then
            Try
                proxy = dt.Rows(0)(18).ToString
                If proxy <> "" And proxy.Length >= 8 Then
                    For j = 0 To contadorFilas - 1
                        numFile += 1
                        indiceXML += 1
                        fileIXML = gblSetPathTmp & "\" & indiceXML & "_CreateProxy_request_tmp.xml"
                        fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                        proxy = dt.Rows(0)(18)
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 12
        End If


        'XML PARA ASIGNAR DISPOSITIVOS A USUARIOS---------------------------------------------------------------------
        If estadoArchivo = 12 Then
            Try
                For j = 0 To contadorFilas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmp & "\" & indiceXML & "_AssignUser_request_tmp.xml"
                    fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 13
        End If


        'XML PARA ASIGNAR PACK DE SERVICIOS---------------------------------------------------------------------
        If estadoArchivo = 13 Then
            Try
                For j = 0 To contadorFilas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmp & "\" & indiceXML & "_AssignServices_request_tmp.xml"
                    fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 14
        End If


        'XML PARA OCP OUTGOING-CALLING-PLAN------------------------------------------------------------------------
        If estadoArchivo = 14 Then
            Try
                For j = 0 To contadorFilas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmp & "\" & indiceXML & "_OCP_request_tmp.xml"
                    fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 15
        End If


        'XML PARA ASIGNAR CONTRASEÑA SIP------------------------------------------------------------------------
        If estadoArchivo = 15 Then
            Try
                For j = 0 To contadorFilas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmp & "\" & indiceXML & "_AssignPasswordSIP_request_tmp.xml"
                    fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 16
        End If


        'XML PARA ACTIVAR LOS NUMEROS------------------------------------------------------------------------
        If estadoArchivo = 16 Then
            Try
                For j = 0 To contadorFilas - 1
                    numFile += 1
                    indiceXML += 1
                    fileIXML = gblSetPathTmp & "\" & indiceXML & "_ActivateNumber_request_tmp.xml"
                    fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
                Exit Sub
            End Try
            estadoArchivo = 17
        End If


        'XML PARA ACTIVAR LA MUSICA EN ESPERA DEL GRUPO--------------------------------------------------------------
        If estadoArchivo = 17 Then
            Try
                numFile += 1
                indiceXML += 1
                fileIXML = gblSetPathTmp & "\" & indiceXML & "_GroupMusicOnHold_request_tmp.xml"
                fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
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
                FileClose(1)
            End Try
        End If

        FileClose(1)

        'Exit Sub

        'MsgBox(My.Settings.gblCMMIdCluster.ToString())

        'parseXML_update_CMM(codError, msgError)

        executeShellBulk(multipleInputFile, My.Settings.gblCMMIdCluster, codError, msgError)
        If codError = 0 Then
            parseXML_update_CMM(codError, msgError)
            'My.Application.DoEvents()
        End If
    End Sub


    Public Sub ModifyProxy()

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para "GroupAccessDeviceGetListRequest"   |
        '\\\\\\\\\\\\\\\\\\\\///////////////////////////
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
        Dim estadoArchivo As Integer = 0
        Dim codError As Integer
        Dim msgError As String = ""
        Dim multipleInputFile As String = gblSetPathTmp & "\multipleInputFile1.txt"
        Dim lineConfigFile As String = ""
        Dim numFile As Integer = 1

        FileOpen(1, multipleInputFile, OpenMode.Output, OpenAccess.Write)

        'XML PARA ACTIVAR LA MUSICA EN ESPERA DEL GRUPO--------------------------------------------------------------
        If TextBox1.Text <> "" Then
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & indiceXML & "_DeviceGetList_request_tmp.xml"
            fileOXML = gblSetPathTmp & "\" & indiceXML & "_Broadsoft_response_tmp.xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, r_1.ToCharArray)
            WriteLine(numFile, r_2.ToCharArray)
            WriteLine(numFile, r_3.ToCharArray)
            WriteLine(numFile, r_4.ToCharArray)
            WriteLine(numFile, r_5.ToCharArray)
            r_6 = "<groupId>" & TextBox1.Text.ToString & "_cloudpbx" & "</groupId>"
            WriteLine(numFile, r_6.ToCharArray)
            WriteLine(numFile, r_7.ToCharArray)
            WriteLine(numFile, r_8.ToCharArray)
            WriteLine(numFile, lineaFinal.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            WriteLine(1, lineConfigFile.ToCharArray)

            FileClose(1)
        Else
            MsgBox("Campo de groupId inválido", MsgBoxStyle.Exclamation)
            Exit Sub
        End If


        executeShellBulk(multipleInputFile, My.Settings.gblCMMIdCluster, codError, msgError)
        If codError = 0 Then
            parseXML_update_CMM(codError, msgError)
            'My.Application.DoEvents()
        End If

        'LblEstado.Text = "Creación de archivos finalizada"
        'ProgressBar1.Value = ProgressBar1.Value + 40
        'My.Application.DoEvents()
    End Sub



    Public Sub executeShellBulk(ByVal fileMIF As String, ByVal cluster As Integer, ByVal codError As Integer, ByVal msgError As String)
        Dim fileConfig As String = ""
        Dim linregConfig As String = ""
        Dim strArguments As String = ""

        'My.Application.DoEvents()
        fileConfig = gblSetPathTmp & "\ociclient.config"
        FileOpen(1, fileConfig, OpenMode.Output, OpenAccess.Write)

        linregConfig = "userId = " & My.Settings.SetUser
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "password = " & My.Settings.SetPassword
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "hostname = " & My.Settings.SetHost
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "port = " & My.Settings.SetPort
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "sessionID = " & gblSession
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "connectionMode = " & My.Settings.SetMode
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "runMode =  Multiple"
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "multipleInputFile = " & fileMIF
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "pauseTimeBeforeRunStart = 3"
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "numberOfRuns = 1"
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "quietMode = " & My.Settings.SetModeQuit
        'linregConfig = "quietMode = " & "False"
        WriteLine(1, linregConfig.ToCharArray)

        linregConfig = "resultOutputFile = " & gblSetPathLog & "\voxTool_UserExtract_" & Format(Now(), "ddMMyyyy_hhmmss") & ".log"
        WriteLine(1, linregConfig.ToCharArray)

        FileClose(1)
        strArguments &= fileConfig
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp\ociclient.config

        btn_procesar.Enabled = False
        btn_BrowseCSV.Enabled = False
        LblEstado.Text = "Ejecutando aplicación Voxcom..."
        ProgressBar1.Value = ProgressBar1.Value + 15
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
            'My.Application.DoEvents()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            MsgBox("Archivo no ha sido generado " & ex.ToString)
            grabaLog(1, 3, "Error al ejecutar Shell>" & strArguments)
            codError = 1
            msgError = "Archivo no ha sido generado"
            LblEstado.Text = "Error"
            Exit Sub
        End Try

        LblEstado.Text = "Esperando reporte"
        ProgressBar1.Value = ProgressBar1.Value + 20
        My.Application.DoEvents()

    End Sub

    Sub parseXML_update_CMM(ByRef codError As Integer, ByRef msgError As String)



        '        <?xml version="1.0" encoding="ISO-8859-1"?> -----------------------------------------------------nodo tipo declaracion
        '<BroadsoftDocument protocol = "OCI" xmlns="C" xmlns:xsi = "http://www.w3.org/2001/XMLSchema-instance" >--nodo tipo element
        '<sessionId xmlns="">10.184.67.132,312714112,1561598861624</sessionId>
        '<command type = "Error" echo="" xsi:type = "c:ErrorResponse" xmlns:c = "C" xmlns="">
        '<summary>[Error 4267] Error assigning domain since the domain Is already assigned: felipe.cl</summary>
        '<summaryEnglish>[Error 4267] Error assigning domain since the domain Is already assigned: felipe.cl</summaryEnglish>
        '</command>
        '</BroadsoftDocument>


        '        <?xml version="1.0" encoding="ISO-8859-1"?>
        '<BroadsoftDocument protocol = "OCI" xmlns="C" xmlns:xsi = "http://www.w3.org/2001/XMLSchema-instance" >
        '<sessionId xmlns="">10.184.67.129,312714112,1561578139714</sessionId>
        '<command echo = "" xsi:Type = "c:SuccessResponse" xmlns:c = "C" xmlns=""/>
        '</BroadsoftDocument>


        Dim reader As XmlTextReader
        Dim swCol As Boolean = False
        Dim exito As Boolean = False
        Dim parseXMl As String
        Dim i As Integer = 0
        Dim iSql As String = ""
        Dim iXml As Integer = 1
        Dim topeXml As Integer = 0
        Dim response As String = ""

        Dim comando1 As New OleDbCommand()
        comando1.Connection = Conexion
        Dim Sql As String = "DELETE * FROM broadsoft_response_error"
        comando1.CommandText = Sql

        Try
            Conexion.Open()
            comando1.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Conexion.Close()

        'MyConn.Open()
        'dcUser = New OleDb.OleDbCommand()
        'dcUser.Connection = MyConn
        Dim fileNameTmp As String = ""
        For num = 1 To indiceXML

            exito = False
            Try
                parseXMl = gblSetPathTmp & "\" & num & "_Broadsoft_response_tmp.xml"
                reader = New XmlTextReader(parseXMl)
                Do While (reader.Read())

                    Select Case reader.NodeType

                        Case XmlNodeType.Element
                            If reader.Name = "command" Then

                                i += 1
                                If reader.HasAttributes Then 'If attributes exist
                                    While reader.MoveToNextAttribute()
                                        'Display attribute name and value.
                                        'MsgBox(reader.Name.ToString & reader.Value.ToString)
                                        If reader.Name = "xsi:type" Then
                                            If reader.Value = "c:SuccessResponse" Then
                                                'MsgBox("comando exitoso")
                                            ElseIf reader.Value = "c:ErrorResponse" Then

                                                'MsgBox("Error en el comando")
                                            End If
                                        End If
                                    End While
                                End If
                            End If
                            If reader.Name = "summary" Then

                                'MsgBox(reader.ReadString.ToString)
                                response = reader.ReadString & "_[File:" & num & "_Broadsoft_response_tmp.xml]"

                                Dim cadenaSql As String = "INSERT INTO broadsoft_response_error ([error]) VALUES ( '" & response & "')"

                                'Crear un comando
                                Dim Comando As OleDbCommand = Conexion.CreateCommand()
                                Comando.CommandText = cadenaSql

                                'Ejecutar la consulta de accion (agregan registros)
                                Try
                                    Conexion.Open()
                                    Comando.ExecuteNonQuery()
                                    'MsgBox("Se agregó correctamente el registro")
                                Catch ex As Exception
                                    MsgBox(" errorcito " & ex.ToString())
                                End Try
                                Conexion.Close()
                            End If
                            'Case XmlNodeType.XmlDeclaration
                    End Select
                Loop
                reader.Close()
            Catch ex As Exception
                'MsgBox("Archivo de Respuesta no ha sido encontrado!", vbExclamation, "Error")
                grabaLog(1, 2, "Error al leer archivo XML>" & gblSetPathTmp & "\CMM_response_tmp_" & iXml & ".xml")
                codError = 1
                msgError = "Respuesta No Generada"
            End Try
        Next
        indiceXML = 0
        btn_procesar.Enabled = False
        btn_BrowseCSV.Enabled = Enabled
        ProgressBar1.Value = ProgressBar1.Maximum
        LblEstado.Text = "Finalizado"
        Dim FMP As New Frm_Report
        FMP.Show()
        FMP.BringToFront()
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
        LblEstado.Text = "Guardando log"
        ProgressBar1.Value = ProgressBar1.Maximum
        FileOpen(2, fileLog, OpenMode.Append, OpenAccess.Write)
        WriteLine(2, linerr.ToCharArray)
        FileClose(2)
    End Sub


    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub Btn_BrowseCSV_MouseEnter(sender As Object, e As EventArgs) Handles btn_BrowseCSV.MouseEnter
        TooltipAyudaBotones(ToolTipHelpButtons, btn_BrowseCSV, "Seleccione un archivo")
    End Sub

    Private Sub Btn_procesar_MouseEnter(sender As Object, e As EventArgs) Handles btn_procesar.MouseEnter
        TooltipAyudaBotones(ToolTipHelpButtons, btn_procesar, "Procesar y enviar la información")
    End Sub

    Private Sub LblEstado_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TabPage2_Click(sender As Object, e As EventArgs) Handles TabPage2.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ModifyProxy()
    End Sub
End Class