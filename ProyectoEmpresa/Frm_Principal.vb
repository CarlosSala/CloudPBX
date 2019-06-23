Imports System.Xml
Imports System.Data.OleDb

Public Class Frm_Principal

    Dim ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
    Dim Conexion As New OleDbConnection(ConexionString)

    Dim gblSession As String = ""
    Dim gblUpdTotalReg As Integer = 0
    Dim gblUpdTotaliXML As Integer = 0

    Dim gblSetPathTmp As String
    Dim gblSetPathAppl As String
    Dim gblSetPathLog As String
    Dim gblTimePing As Integer = 2000

    Private Sub For1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Interface_Entrada()
        gblSetPathTmp = My.Application.Info.DirectoryPath & My.Settings.SetPathTmp
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp
        gblSetPathAppl = My.Application.Info.DirectoryPath & My.Settings.SetPathAppl
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom
        gblSetPathLog = My.Application.Info.DirectoryPath & My.Settings.SetPathLog
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\log
    End Sub

    Public Sub executeShellBulk(ByVal fileMIF As String, ByVal cluster As Integer, ByVal codError As Integer, ByVal msgError As String)
        Dim fileConfig As String = ""
        Dim linregConfig As String = ""
        Dim strArguments As String = ""
        'Dim strUser As String = "siemens02"
        'Dim strPassword As String = "siemens.02"


        Dim i As Integer = 0
        'Dim regCluster() As String

        'For i = 0 To arrayCluster.Length
        '    regCluster = Split(arrayCluster(i), ";")
        '    If cluster = regCluster(0) Then
        '        strUser = regCluster(2)
        '        strPassword = regCluster(3)
        '        Exit For
        '    End If
        'Next

        'If cluster = 1 Then
        '    strUser = My.Settings.SetUserC1
        '    strPassword = My.Settings.SetPasswordC1
        'End If
        'If cluster = 2 Then
        '    strUser = My.Settings.SetUserC2
        '    strPassword = My.Settings.SetPasswordC2
        'End If
        'If cluster = 3 Then
        '    strUser = My.Settings.SetUserC3
        '    strPassword = My.Settings.SetPasswordC3
        'End If

        LblEstado.Text = "Creando archivo de configuración..."
        ProgressBar1.Value = ProgressBar1.Value + 30

        My.Application.DoEvents()
        fileConfig = gblSetPathTmp & "\ociclient.config"
        FileOpen(8, fileConfig, OpenMode.Output, OpenAccess.Write)
        linregConfig = "userId = " & My.Settings.SetUser

        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "password = " & My.Settings.SetPassword
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "hostname = " & My.Settings.SetHost
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "port = " & My.Settings.SetPort
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "sessionID = " & gblSession
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "connectionMode = " & My.Settings.SetMode
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "runMode =  Multiple"
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "multipleInputFile = " & fileMIF
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "pauseTimeBeforeRunStart = 3"
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "numberOfRuns = 1"
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "quietMode = " & My.Settings.SetModeQuit
        'linregConfig = "quietMode = " & "False"
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "resultOutputFile = " & gblSetPathLog & "\voxTool_UserExtract_" & Format(Now(), "ddMMyyyy_hhmmss") & ".log"
        WriteLine(8, linregConfig.ToCharArray)

        FileClose(8)
        strArguments &= fileConfig
        'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Debug\voxcom\tmp\ociclient.config

        LblEstado.Text = "Ejecutando aplicación Voxcom..."
        'My.Application.DoEvents()
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
        ProgressBar1.Value = ProgressBar1.Value + 30
        LblEstado.Text = "Finalizado"
        btn_procesar.Enabled = False
        My.Application.DoEvents()
    End Sub


    Sub parseXML_update_CMM(ByRef codError As Integer, ByRef msgError As String)
        Dim reader As XmlTextReader
        Dim swCol As Boolean = False
        Dim exito As Boolean = False
        Dim parseXMl As String
        Dim i As Integer = 0
        Dim iSql As String = ""
        Dim iXml As Integer = 0
        Dim topeXml As Integer = 0
        topeXml = gblUpdTotaliXML
        Conexion.Open()
        Dim dcUser = New OleDb.OleDbCommand()
        dcUser.Connection = Conexion
        Dim fileNameTmp As String = ""
        For iXml = 1 To topeXml
            exito = False
            Try
                parseXMl = gblSetPathTmp & "\CMM_response_tmp_" & iXml & ".xml"
                reader = New XmlTextReader(parseXMl)
                Do While (reader.Read())
                    Select Case reader.NodeType
                        Case XmlNodeType.Element 'Display beginning of element.
                            '                    Console.Write("<" + reader.Name)
                            If reader.Name = "command" Then
                                i += 1
                                If reader.HasAttributes Then 'If attributes exist
                                    While reader.MoveToNextAttribute()
                                        'Display attribute name and value.
                                        Console.Write(" {0}='{1}'", reader.Name, reader.Value)
                                        If reader.Name = "xsi:type" Then
                                            If reader.Value = "c:SuccessResponse" Then
                                                'Try
                                                '    iSql = "UPDATE brs_user set brs_user_number_activation = '" & Trim(dataNAGrdUpdate.Rows(i - 1).Cells(3).Value) & "' WHERE brs_user_userId = '" & dataNAGrdUpdate.Rows(i - 1).Cells(4).Value & "';"
                                                '    dcUser.CommandText = iSql
                                                '    dcUser.ExecuteNonQuery()
                                                Try
                                                    iSql = "delete from brs_update_tmp where brs_udet_userId = '" & DataGridView1.Rows(i - 1).Cells(4).Value & "';"
                                                    dcUser.CommandText = iSql
                                                    'dc = New OleDb.OleDbCommand(iSql, MyConn)
                                                    dcUser.ExecuteNonQuery()
                                                    'MessageBox.Show("Access created Succesfully for brs_user " + fila)
                                                Catch ex As Exception
                                                    'MessageBox.Show(ex.Message)
                                                    'codError = 2
                                                    'msgError = "No actualizado en BD"
                                                End Try
                                                'Catch ex As Exception
                                                '    'MessageBox.Show(ex.Message)
                                                '    'codError = 2
                                                '    'msgError = "No actualizado en BD"
                                                'End Try
                                            Else
                                                'dataGrdUpdate.Rows(i - 1).Cells("Column2").Value = imgListUpdate.Images(2)
                                            End If
                                        End If
                                    End While
                                End If
                            End If
                            'Case XmlNodeType.Text 'Display the text in each element.
                            '    'Console.WriteLine(reader.Value)
                            'Case XmlNodeType.EndElement 'Display end of element.
                            '    If reader.Name = "command" Then
                            '        swCol = False
                            '    End If
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
        LblEstado.Text = ""
        Conexion.Close()
        'actualizaCMMGrillaUpdate(0)
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
        FileOpen(7, fileLog, OpenMode.Append, OpenAccess.Write)
        WriteLine(7, linerr.ToCharArray)
        FileClose(7)
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
            MsgBox(ex.ToString())
            MsgBox("Asegurese de que el archivo no este siendo utlizado por otro proceso")
            Me.Cursor = Cursors.Default
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

        'Dim Numeros As Long
        'Dim ierr = 0

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

        ''Comprobar si el archivo esta vacio
        'Try
        '    readLine = LineInput(1)
        '    MsgBox("texto del archivo: " & readLine)
        'Catch ex As Exception
        '    MsgBox(ex.ToString)
        'End Try

        'If readLine.Length = 0 Then
        '    'If readLine = "" Then
        '    MsgBox("El archivo se encuentra vacio")
        '    FileClose(1)
        '    Exit Sub
        'Else
        '    MsgBox("El archivo no esta vacio")
        '    FileClose(1)
        '    Exit Sub
        'End If
        'Validar el formato del archivo

        'Se lee linea por linea el archivo con id = 1, hasta que este acabe, EndOfFile
        While Not EOF(1)

            'Lee una linea del archivo
            readLine = LineInput(1)
            arrayLine = Split(readLine, ";")
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

            'Se debe modificar el tipo de dato, de numero entero largo a -> doble
            'Access no redondea cifras automaticamente si estas estan en formato general y si no superan los 16 caracteres
            'Numeros = Convert.ToInt64(arrayLine(1))
            'MsgBox(Dominio & " " & Numeros.ToString())

            'Instrucción SQL
            'Se insertan datos en los campos domain y numbers de la tabla brs_create_group
            Dim cadenaSql As String = "INSERT INTO broadsoft_cloudPBX ([domain], numbers, group_id, group_name, contact_name, contact_phone, enterprise_address, city,
                                                                        device_type, mac, serial_number, physical_location, deparment_name)"
            cadenaSql = cadenaSql + " VALUES ( '" & Dominio & "',"
            cadenaSql = cadenaSql + "          '" & Numeros & "',"
            cadenaSql = cadenaSql + "          '" & Nombre_grupo & "',"
            cadenaSql = cadenaSql + "          '" & Nombre_empresa & "',"
            cadenaSql = cadenaSql + "          '" & Nombre_contacto & "',"
            cadenaSql = cadenaSql + "          '" & Telefono_contacto & "',"
            cadenaSql = cadenaSql + "          '" & Direccion_empresa & "',"
            cadenaSql = cadenaSql + "          '" & Ciudad & "',"
            cadenaSql = cadenaSql + "          '" & Tipo_dispositivo & "',"
            cadenaSql = cadenaSql + "          '" & Mac & "',"
            cadenaSql = cadenaSql + "          '" & Numero_serie & "',"
            cadenaSql = cadenaSql + "          '" & Locacion_fisica & "',"
            cadenaSql = cadenaSql + "          '" & Departamento & "')"

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
        FileClose(1)
        actualizarGrilla()
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

    Public Sub actualizarGrilla()

        Dim iSql As String = "select * from broadsoft_cloudPBX"
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
            DataGridView1.DataSource = dt
            DataGridView1.Refresh()
        Catch ex As Exception
            MsgBox("Can not open connection ! , " & ex.Message)
        End Try
        Conexion.Close()

        DataGridView1.CurrentCell = DataGridView1.Rows(0).Cells(0)
        lblCMMUpdCurrentRow.Text = DataGridView1.CurrentCell.RowIndex + 1
        lblCMMUpdTotalRows.Text = DataGridView1.RowCount

        Lab_wait.Visible = False
        Me.Cursor = Cursors.Default
        Interface_Salida()
    End Sub

    Private Sub BtnCMMOpenFile_Click(sender As Object, e As EventArgs)

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

    Private Sub Interface_Salida()
        btn_procesar.Enabled = True
        btn_BrowseCSV.Enabled = True
    End Sub
    Private Sub btn_procesar_Click(sender As Object, e As EventArgs) Handles btn_procesar.Click

        Dim mensaje As String = ""
        Dim j As Integer = 0

        'If My.Computer.Network.Ping(My.Settings.SetHost, gblTimePing) Then
        '    MsgBox("Server pinged successfully.")
        'Else
        '    MsgBox("Servidor fuera de Linea, favor verifique conexion!!!", vbOKOnly, "Error de Comunicación")
        '    Exit Sub
        'End If

        'Val("    38205 (Distrito Norte)")devuelve 38205 como valor numérico. Los espacios y el resto de cadena
        'a partir de donde no se puede reconocer un valor numérico se ignora, Si la cadena empieza con contenido no numérico Val devuelve cero.
        If Val(lblCMMUpdTotalRows.Text) = 0 Then
            mensaje = "No existen datos en el archivo ..."
            Exit Sub
        Else
            mensaje = lblCMMUpdTotalRows.Text & " registros a actualizar ..."
        End If
        'vbCrLf para un salto de linea
        mensaje &= vbCrLf & vbCrLf & "Presione Aceptar para confirmar actualización..."
        If MsgBox(mensaje, MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton2, "Confirmación") = MsgBoxResult.Cancel Then
            Exit Sub
        End If


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
        Dim d_8 As String = "<userLimit>25</userLimit>"
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

        Dim h_1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim h_2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        Dim h_3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim h_4 As String = "<command xsi:type=" & Chr(34) & "GroupDepartmentAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim h_5 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim h_6 As String = "<groupId>PRUEBACARLOS_cloudpbx</groupId>"
        Dim h_7 As String = "<departmentName>Administracion</departmentName>"
        Dim h_8 As String = "</command>"

        'ultima linea de todos los XML
        Dim lineaFinal As String = "</BroadsoftDocument>"

        gblUpdTotalReg = 0
        gblUpdTotaliXML = 0
        ProgressBar1.Minimum = 0
        ProgressBar1.Value = 0
        ProgressBar1.Maximum = 100
        'ProgressBar1.Maximum = Val(lblCMMUpdTotalRows.Text)

        For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblSetPathTmp, FileIO.SearchOption.SearchAllSubDirectories, "*.*")
            'My.Computer.FileSystem.DeleteFile(foundFile)
        Next

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim indiceXML As Integer = 0
        Dim codError As Integer
        Dim msgError As String = ""
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
        LblEstado.Text = "Generando XML..."
        ProgressBar1.Value = ProgressBar1.Value + 10

        My.Application.DoEvents()
        Dim multipleInputFile As String = gblSetPathTmp & "\multipleInputFile.txt"
        Dim lineConfigFile As String = ""
        FileOpen(50, multipleInputFile, OpenMode.Output, OpenAccess.Write)


        'XML PARA CREAR UN DOMINIO-----------------------------------------------------------------------------
        indiceXML += 1
        fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
        fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
        FileOpen(2, fileIXML, OpenMode.Output)
        WriteLine(2, a_1.ToCharArray)
        WriteLine(2, a_2.ToCharArray)
        WriteLine(2, a_3.ToCharArray)
        WriteLine(2, a_4.ToCharArray)
        domain = DataGridView1.Rows(j).Cells(0).Value
        a_5 = "<domain>" & domain & "</domain>"
        WriteLine(2, a_5.ToCharArray)
        WriteLine(2, a_6.ToCharArray)
        WriteLine(2, lineaFinal.ToCharArray)
        FileClose(2)
        lineConfigFile = fileIXML & ";" & fileOXML
        My.Application.DoEvents()
        WriteLine(50, lineConfigFile.ToCharArray)

        'XML PARA ASIGNAR EL DOMINIO CREADO----------------------------------------------------------------------
        indiceXML += 1
        fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
        fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
        FileOpen(3, fileIXML, OpenMode.Output)
        WriteLine(3, b_1.ToCharArray)
        WriteLine(3, b_2.ToCharArray)
        WriteLine(3, b_3.ToCharArray)
        WriteLine(3, b_4.ToCharArray)
        WriteLine(3, b_5.ToCharArray)
        b_6 = "<domain>" & domain & "</domain>"
        WriteLine(3, b_6.ToCharArray)
        WriteLine(3, b_7.ToCharArray)
        WriteLine(3, lineaFinal.ToCharArray)
        FileClose(3)
        lineConfigFile = fileIXML & ";" & fileOXML
        My.Application.DoEvents()
        WriteLine(50, lineConfigFile.ToCharArray)

        'XML PARA CREAR NUMERACIÓN------------------------------------------------------------------------------
        indiceXML += 1
        fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
        fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
        FileOpen(4, fileIXML, OpenMode.Output)
        WriteLine(4, c_1.ToCharArray)
        WriteLine(4, c_2.ToCharArray)
        WriteLine(4, c_3.ToCharArray)
        WriteLine(4, c_4.ToCharArray)
        WriteLine(4, c_5.ToCharArray)
        For j = 0 To DataGridView1.Rows.Count - 2
            phoneNumber = DataGridView1.Rows(j).Cells(1).Value
            c_6 = "<phoneNumber>" & phoneNumber & "</phoneNumber>"
            WriteLine(4, c_6.ToCharArray)
        Next
        WriteLine(4, c_7.ToCharArray)
        WriteLine(4, lineaFinal.ToCharArray)
        FileClose(4)
        lineConfigFile = fileIXML & ";" & fileOXML
        WriteLine(50, lineConfigFile.ToCharArray)

        'XML PARA CREAR PERFIL DE GRUPO-------------------------------------------------------------------------
        indiceXML += 1
        fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
        fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
        FileOpen(5, fileIXML, OpenMode.Output)
        WriteLine(5, d_1.ToCharArray)
        WriteLine(5, d_2.ToCharArray)
        WriteLine(5, d_3.ToCharArray)
        WriteLine(5, d_4.ToCharArray)
        WriteLine(5, d_5.ToCharArray)
        group_id = DataGridView1.Rows(0).Cells(2).Value
        d_6 = "<groupId>" & group_id & "</groupId>"
        WriteLine(5, d_6.ToCharArray)
        d_7 = "<defaultDomain>" & domain & "</defaultDomain>"
        WriteLine(5, d_7.ToCharArray)
        WriteLine(5, d_8.ToCharArray)
        group_name = DataGridView1.Rows(0).Cells(3).Value
        d_9 = "<groupName>" & group_name & "</groupName>"
        WriteLine(5, d_9.ToCharArray)
        d_10 = "<callingLineIdName>" & group_name & "</callingLineIdName>"
        WriteLine(5, d_10.ToCharArray)
        'WriteLine(5, d_11.ToCharArray)
        WriteLine(5, d_12.ToCharArray)
        contact_name = DataGridView1.Rows(0).Cells(4).Value
        d_13 = "<contactName>" & contact_name & "</contactName>"
        WriteLine(5, d_13.ToCharArray)
        contact_number = DataGridView1.Rows(0).Cells(5).Value
        d_14 = "<contactNumber>" & contact_number & "</contactNumber>"
        WriteLine(5, d_14.ToCharArray)
        WriteLine(5, d_15.ToCharArray)
        WriteLine(5, d_16.ToCharArray)
        address = DataGridView1.Rows(0).Cells(6).Value
        d_17 = "<addressLine1>" & address & "</addressLine1>"
        WriteLine(5, d_17.ToCharArray)
        city = DataGridView1.Rows(0).Cells(7).Value
        d_18 = "<city>" & city & "</city>"
        WriteLine(5, d_18.ToCharArray)
        WriteLine(5, d_19.ToCharArray)
        WriteLine(5, d_20.ToCharArray)
        WriteLine(5, lineaFinal.ToCharArray)
        FileClose(5)
        lineConfigFile = fileIXML & ";" & fileOXML
        My.Application.DoEvents()
        WriteLine(50, lineConfigFile.ToCharArray)

        'XML PARA SELECCIONAR SERVICIOS DE GRUPO (ARCHIVO EXTERNO)--------------------------------------------------------------
        indiceXML += 1
        fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
        fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
        lineConfigFile = fileIXML & ";" & fileOXML
        My.Application.DoEvents()
        WriteLine(50, lineConfigFile.ToCharArray)
        Try
            Dim Lines_Array() As String = IO.File.ReadAllLines(gblSetPathTmp & "\CMM_request_tmp_5.xml")
            Lines_Array(5) = " <groupId>" & group_id & "</groupId>"
            'MsgBox(Lines_Array(5).ToString())
            IO.File.WriteAllLines(gblSetPathTmp & "\CMM_request_tmp_5.xml", Lines_Array)
            'MsgBox("se reescribió correctamente el archivo de servicios de grupo")
        Catch ex As Exception
            MsgBox("error al modificar el archivo de servicios de grupo " & ex.ToString)
        End Try


        'XML PARA ASIGNAR LOS SERVICIOS---------------------------------------------------------------------------------
        indiceXML += 1
        fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
        fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
        FileOpen(7, fileIXML, OpenMode.Output)
        WriteLine(7, e_1.ToCharArray)
        WriteLine(7, e_2.ToCharArray)
        WriteLine(7, e_3.ToCharArray)
        WriteLine(7, e_4.ToCharArray)
        WriteLine(7, e_5.ToCharArray)
        e_6 = "<groupId>" & group_id & "</groupId>"
        WriteLine(7, e_6.ToCharArray)
        WriteLine(7, e_7.ToCharArray)
        WriteLine(7, e_8.ToCharArray)
        WriteLine(7, e_9.ToCharArray)
        WriteLine(7, e_10.ToCharArray)
        WriteLine(7, e_11.ToCharArray)
        WriteLine(7, e_12.ToCharArray)
        WriteLine(7, e_13.ToCharArray)
        WriteLine(7, e_14.ToCharArray)
        WriteLine(7, e_15.ToCharArray)
        WriteLine(7, e_16.ToCharArray)
        WriteLine(7, e_17.ToCharArray)
        WriteLine(7, e_18.ToCharArray)
        WriteLine(7, e_19.ToCharArray)
        WriteLine(7, e_20.ToCharArray)
        WriteLine(7, e_21.ToCharArray)
        WriteLine(7, e_22.ToCharArray)
        WriteLine(7, e_23.ToCharArray)
        WriteLine(7, e_24.ToCharArray)
        WriteLine(7, e_25.ToCharArray)
        WriteLine(7, e_26.ToCharArray)
        WriteLine(7, e_27.ToCharArray)
        WriteLine(7, e_28.ToCharArray)
        WriteLine(7, e_29.ToCharArray)
        WriteLine(7, lineaFinal.ToCharArray)
        FileClose(7)
        lineConfigFile = fileIXML & ";" & fileOXML
        WriteLine(50, lineConfigFile.ToCharArray)

        'XML PARA ASIGNAR LA NUMERACIÓN---------------------------------------------------------------------------------
        indiceXML += 1
        fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
        fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
        FileOpen(8, fileIXML, OpenMode.Output)
        WriteLine(8, f_1.ToCharArray)
        WriteLine(8, f_2.ToCharArray)
        WriteLine(8, f_3.ToCharArray)
        WriteLine(8, f_4.ToCharArray)
        WriteLine(8, f_5.ToCharArray)
        f_6 = "<groupId>" & group_id & "</groupId>"
        WriteLine(8, f_6.ToCharArray)
        For j = 0 To DataGridView1.Rows.Count - 2
            phoneNumber = DataGridView1.Rows(j).Cells(1).Value
            f_7 = "<phoneNumber>+56-" & phoneNumber & "</phoneNumber>"
            WriteLine(8, f_7.ToCharArray)
        Next
        WriteLine(8, f_8.ToCharArray)
        WriteLine(8, lineaFinal.ToCharArray)
        FileClose(8)
        lineConfigFile = fileIXML & ";" & fileOXML
        My.Application.DoEvents()
        WriteLine(50, lineConfigFile.ToCharArray)

        'XML PARA CREAR LOS DISPOSITIVOS---------------------------------------------------------------------------------
        Dim numFile As Integer = 8
        For j = 0 To DataGridView1.Rows.Count - 2
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
            fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, g_1.ToCharArray)
            WriteLine(numFile, g_2.ToCharArray)
            WriteLine(numFile, g_3.ToCharArray)
            WriteLine(numFile, g_4.ToCharArray)
            WriteLine(numFile, g_5.ToCharArray)
            group_id = DataGridView1.Rows(0).Cells(2).Value
            g_6 = "<groupId>" & group_id & "</groupId>"
            WriteLine(numFile, g_6.ToCharArray)
            mac = DataGridView1.Rows(j).Cells(9).Value
            g_7 = "<deviceName>DV_" & mac & "</deviceName>"
            WriteLine(numFile, g_7.ToCharArray)
            device_type = DataGridView1.Rows(j).Cells(8).Value
            g_8 = "<deviceType>" & device_type & "</deviceType>"
            WriteLine(numFile, g_8.ToCharArray)
            WriteLine(numFile, g_9.ToCharArray)
            g_10 = "<macAddress>" & mac & "</macAddress>"
            WriteLine(numFile, g_10.ToCharArray)
            serial_number = DataGridView1.Rows(j).Cells(10).Value
            g_11 = "<serialNumber>" & serial_number & "</serialNumber>"
            WriteLine(numFile, g_11.ToCharArray)
            physical_location = DataGridView1.Rows(j).Cells(11).Value
            g_12 = "<physicalLocation>" & physical_location & "</physicalLocation>"
            WriteLine(numFile, g_12.ToCharArray)
            WriteLine(numFile, g_13.ToCharArray)
            WriteLine(numFile, g_14.ToCharArray)
            WriteLine(numFile, lineaFinal.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            My.Application.DoEvents()
            WriteLine(50, lineConfigFile.ToCharArray)
        Next

        'XML PARA CREAR LOS DEPARTAMENTOS---------------------------------------------------------------------------------
        Dim varAcumulaDepto As String = ""
        Dim arreglo() As String
        Dim arregloDeptos(DataGridView1.Rows.Count - 2) As String
        Dim indice As Integer
        Dim numElementos As Integer = 0

        For i = 0 To DataGridView1.Rows.Count - 2
            varAcumulaDepto += DataGridView1.Rows(i).Cells(12).Value & ";"
            'MsgBox(Depto.ToString)
        Next

        arreglo = Split(varAcumulaDepto, ";")
        'MsgBox("Elementos en el arreglo: " & arreglo.Length)

        For k = 0 To arreglo.Length - 1
            Try
                indice = Array.IndexOf(arregloDeptos, arreglo(k))
                'MsgBox("El elemento " & arreglo(k) & " arroja: " & indice)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try

            If indice = -1 And arreglo(k) <> "" And arreglo(k).Length <> 0 Then
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

        For j = 0 To arregloDeptos.Length - 1
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
            fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
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
            My.Application.DoEvents()
            WriteLine(50, lineConfigFile.ToCharArray)
        Next

        FileClose(50)

        LblEstado.Text = "Creación de archivos finalizada"
        ProgressBar1.Value = ProgressBar1.Value + 30
        Exit Sub
        'MsgBox(My.Settings.gblCMMIdCluster.ToString())
        executeShellBulk(multipleInputFile, My.Settings.gblCMMIdCluster, codError, msgError)
        If codError = 0 Then
            parseXML_update_CMM(codError, msgError)
            'My.Application.DoEvents()
        End If
    End Sub

    Public Sub TooltipAyudaBotones(ByVal TooltipAyuda As ToolTip, ByVal Boton As Button, ByVal mensaje As String)
        ToolTipHelpButtons.RemoveAll()
        ToolTipHelpButtons.SetToolTip(Boton, mensaje)
        ToolTipHelpButtons.InitialDelay = 500
        ToolTipHelpButtons.IsBalloon = False
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

End Class