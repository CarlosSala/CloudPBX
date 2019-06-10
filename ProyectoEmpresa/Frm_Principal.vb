Imports System.Data.OleDb
Public Class Frm_Principal

    Dim ConexionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
    Dim Conexion As New OleDbConnection(ConexionString)


    Dim gblTotUsers As Integer
    Dim gblIdCluster As Integer = 0
    Dim gblNameCluster As String = ""
    Dim gblSession As String = ""
    Dim gblUserLogin As String = ""
    Dim gblUserPassword As String = ""
    Dim arrayTotUserId(100000) As String
    Dim arrayTotDevice(100000) As String
    Dim gblTotUserId As Integer
    Dim gblTotUserDet As Integer = 0
    Dim gblTotUserDev As Integer
    Dim gblUpdTotalC1 As Integer = 0
    Dim gblUpdTotalC2 As Integer = 0
    Dim gblUpdTotalC3 As Integer = 0
    Dim gblUpdTotalReg As Integer = 0
    Dim gblUpdTotaliXML As Integer = 0
    Dim arrayCluster(10) As String
    Dim gblTope As Integer = 15
    Dim arrayServicios(22, 2) As String
    Dim gblNAUpdTotalC1 As Integer = 0
    Dim gblNAUpdTotalC2 As Integer = 0
    Dim gblNAUpdTotalC3 As Integer = 0
    Dim gblNAUpdTotalReg As Integer = 0
    Dim gblNAUpdTotaliXML As Integer = 0

    Dim gblSetPathTmp As String
    Dim gblSetPathAppl As String
    Dim gblSetPathLog As String
    Dim gblUserProfile As Integer = 0
    Dim gblCancelar As Boolean = False
    Dim gblTimePing As Integer = 2000
    Dim gblNumeros As TextBox
    Dim gblConsultaRemote As Boolean = False
    Dim gblCMMNameCluster As String
    Dim gblCMMIdCluster As String
    Dim gblCallManager As Integer = 0 '0=nada,1=Call Manager,2=Movil


    Private Sub For1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Interface_Entrada()
        gblSetPathTmp = My.Application.Info.DirectoryPath & My.Settings.SetPathTmp
        'MsgBox(gblSetPathTmp.ToString())
    End Sub

    Public Sub executeShellBulk(ByVal fileMIF As String, ByVal cluster As Integer, ByVal codError As Integer, ByVal msgError As String)
        Dim fileConfig As String = ""
        Dim linregConfig As String = ""
        Dim strArguments As String = ""
        Dim strUser As String = ""
        Dim strPassword As String = ""


        Dim i As Integer = 0
        Dim regCluster() As String

        For i = 0 To arrayCluster.Length
            regCluster = Split(arrayCluster(i), ";")
            If cluster = regCluster(0) Then
                strUser = regCluster(2)
                strPassword = regCluster(3)
                Exit For
            End If
        Next

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
        My.Application.DoEvents()
        fileConfig = gblSetPathTmp & "\ociclient.config"
        FileOpen(8, fileConfig, OpenMode.Output, OpenAccess.Write)
        linregConfig = "userId = " & strUser
        WriteLine(8, linregConfig.ToCharArray)

        linregConfig = "password = " & strPassword
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
        LblEstado.Text = "Ejecutando aplicación Voxcom..."
        'My.Application.DoEvents()
        Try
            Dim proceso As New Process()
            proceso.StartInfo.WorkingDirectory = gblSetPathAppl '"c:\asociclient\"
            proceso.StartInfo.FileName = "startVoxTool.bat"
            proceso.StartInfo.Arguments = Chr(34) & strArguments & Chr(34)
            proceso.StartInfo.UseShellExecute = True
            proceso.Start()
            proceso.WaitForExit()
            proceso.Close()
            'My.Application.DoEvents()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            MsgBox("Archivo no ha sido generado")
            grabaLog(1, 3, "Error al ejecutar Shell>" & strArguments)
            codError = 1
            msgError = "Archivo no ha sido generado"
            LblEstado.Text = ""
            Exit Sub
        End Try
        LblEstado.Text = ""
        My.Application.DoEvents()
    End Sub


    'Sub parseXML_update_CMM(ByRef codError As Integer, ByRef msgError As String)
    '    Dim reader As XmlTextReader
    '    Dim swCol As Boolean = False
    '    Dim exito As Boolean = False
    '    Dim parseXMl As String
    '    Dim i As Integer = 0
    '    Dim iSql As String = ""
    '    Dim iXml As Integer = 0
    '    Dim topeXml As Integer = 0
    '    topeXml = gblUpdTotaliXML
    '    MyConn.Open()
    '    dcUser = New OleDb.OleDbCommand()
    '    dcUser.Connection = MyConn
    '    Dim fileNameTmp As String = ""
    '    For iXml = 1 To topeXml
    '        exito = False
    '        Try
    '            parseXMl = gblSetPathTmp & "\" & "CMM_response_tmp_" & iXml & ".xml"
    '            reader = New XmlTextReader(parseXMl)
    '            Do While (reader.Read())
    '                Select Case reader.NodeType
    '                    Case XmlNodeType.Element 'Display beginning of element.
    '                        '                    Console.Write("<" + reader.Name)
    '                        If reader.Name = "command" Then
    '                            i += 1
    '                            If reader.HasAttributes Then 'If attributes exist
    '                                While reader.MoveToNextAttribute()
    '                                    'Display attribute name and value.
    '                                    Console.Write(" {0}='{1}'", reader.Name, reader.Value)
    '                                    If reader.Name = "xsi:type" Then
    '                                        If reader.Value = "c:SuccessResponse" Then
    '                                            'Try
    '                                            '    iSql = "UPDATE brs_user set brs_user_number_activation = '" & Trim(dataNAGrdUpdate.Rows(i - 1).Cells(3).Value) & "' WHERE brs_user_userId = '" & dataNAGrdUpdate.Rows(i - 1).Cells(4).Value & "';"
    '                                            '    dcUser.CommandText = iSql
    '                                            '    dcUser.ExecuteNonQuery()
    '                                            Try
    '                                                iSql = "delete from brs_update_tmp where brs_udet_userId = '" & dataCMMGrdUpdate.Rows(i - 1).Cells(4).Value & "';"
    '                                                dcUser.CommandText = iSql
    '                                                'dc = New OleDb.OleDbCommand(iSql, MyConn)
    '                                                dcUser.ExecuteNonQuery()
    '                                                'MessageBox.Show("Access created Succesfully for brs_user " + fila)
    '                                            Catch ex As Exception
    '                                                'MessageBox.Show(ex.Message)
    '                                                'codError = 2
    '                                                'msgError = "No actualizado en BD"
    '                                            End Try
    '                                            'Catch ex As Exception
    '                                            '    'MessageBox.Show(ex.Message)
    '                                            '    'codError = 2
    '                                            '    'msgError = "No actualizado en BD"
    '                                            'End Try
    '                                        Else
    '                                            'dataGrdUpdate.Rows(i - 1).Cells("Column2").Value = imgListUpdate.Images(2)
    '                                        End If
    '                                    End If
    '                                End While
    '                            End If
    '                        End If
    '                        'Case XmlNodeType.Text 'Display the text in each element.
    '                        '    'Console.WriteLine(reader.Value)
    '                        'Case XmlNodeType.EndElement 'Display end of element.
    '                        '    If reader.Name = "command" Then
    '                        '        swCol = False
    '                        '    End If
    '                End Select
    '            Loop
    '            reader.Close()
    '        Catch ex As Exception
    '            'MsgBox("Archivo de Respuesta no ha sido encontrado!", vbExclamation, "Error")
    '            grabaLog(1, 2, "Error al leer archivo XML>" & gblSetPathTmp & "\CMM_response_tmp_" & iXml & ".xml")
    '            codError = 1
    '            msgError = "Respuesta No Generada"
    '        End Try
    '    Next
    '    LblEstado.Text = ""
    '    MyConn.Close()
    '    actualizaCMMGrillaUpdate(0)
    '    My.Application.DoEvents()

    'End Sub


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
        'Se abre el archivo CSV selccionado en modo lectura y se le asigna un id
        FileOpen(1, TextBox_FileName.Text, OpenMode.Input)

        Dim readLine As String = ""
        Dim arrayLine() As String


        'Variables que contendrán las valores a guardar en access
        Dim Dominio As String = ""
        Dim Numeros As String = ""
        'Dim Numeros As Long
        'Dim ierr = 0

        Dim cmd As New OleDbCommand()
        cmd.Connection = Conexion
        Dim instruccionSql As String = "DELETE * FROM brs_create_group"
        cmd.CommandText = instruccionSql

        Try
            Conexion.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Conexion.Close()

        'Se lee linea por linea el archivo con id = 1, hasta que este acabe, EndOfFile
        While Not EOF(1)

            'Lee una linea del archivo
            readLine = LineInput(1)
            arrayLine = Split(readLine, ";")
            Dominio = arrayLine(0).ToString()
            Numeros = arrayLine(1).ToString()
            'Se debe modificar el tipo de dato, de numero entero largo a -> doble
            'Access no redondea cifras automaticamente si estas estan en formato general y si no superan los 16 caracteres
            'Numeros = Convert.ToInt64(arrayLine(1))
            'MsgBox(Dominio & " " & Numeros.ToString())

            'Instrucción SQL
            'Se insertan datos en los campos domain y numbers de la tabla brs_create_group
            Dim cadenaSql As String = "INSERT INTO brs_create_group ([domain], numbers)"
            cadenaSql = cadenaSql + " VALUES ( '" & Dominio & "',"
            cadenaSql = cadenaSql + "          '" & Numeros & "')"

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

        Dim iSql As String = "select * from brs_create_group"
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
        'Se ejecuta cuando se carga el formulario
        btn_procesar.Enabled = False
        btn_BrowseCSV.Enabled = True
        Lab_wait.Visible = False
        'contabilización de filas y colummnas
        lblCMMUpdCurrentRow.Visible = False
        lblCMMUpdTotalRows.Visible = False
    End Sub

    Private Sub Interface_Salida()
        btn_procesar.Enabled = True
        btn_BrowseCSV.Enabled = True
    End Sub
    Private Sub btn_procesar_Click(sender As Object, e As EventArgs) Handles btn_procesar.Click

        Dim swCluster As Boolean = False
        Dim mensaje As String = ""
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim fileNameTmp As String = ""

        'If My.Computer.Network.Ping(My.Settings.SetHost, gblTimePing) Then
        '    'MsgBox("Server pinged successfully.")
        'Else
        '    MsgBox("Servidor fuera de Linea, favor verifique conexion!!!", vbOKOnly, "Error de Comunicación")
        '    Exit Sub
        'End If

        'Val("    38205 (Distrito Norte)")devuelve 38205 como valor numérico. Los espacios y el resto de cadena
        'a partir de donde no se puede reconocer un valor numérico se ignora, Si la cadena empieza con contenido no numérico Val devuelve cero.
        If Val(lblCMMUpdTotalRows.Text) = 1 Then
            mensaje = "Un registro a actualizar ..."
        Else
            mensaje = lblCMMUpdTotalRows.Text & " registros a actualizar ..."
        End If
        'vbCrLf para un salto de linea
        mensaje &= vbCrLf & vbCrLf & "Presione Aceptar para confirmar actualización..."
        If MsgBox(mensaje, MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton2, "Confirmación") = MsgBoxResult.Cancel Then
            Exit Sub
        End If

        'Exit Sub

        'Encabezado de los xmls
        Dim linupd1 As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>"
        Dim linupd2 As String = "<BroadsoftDocument protocol=" & Chr(34) & "OCI" & Chr(34) & " xmlns=" & Chr(34) & "C" & Chr(34) & ">"
        'Dim linupd3 As String = "<sessionId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</sessionId>"
        Dim linupd3 As String = "<userId xmlns=" & Chr(34) & Chr(34) & ">%%%OSS_USER%%%</userId>"

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para SystemDomainAddRequest                    |
        '\\\\\\\\\\\\\\\\\\\\/////////////////////////////////
        Dim lineaUpddate1_2 As String = "<command xsi:Type=" & Chr(34) & "SystemDomainAddRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim lineaUpddate1_3 As String = "<domain>pruebacarlos.cl</domain>"
        Dim lineaUpddate1_4 As String = "</command>"

        '/////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        '| XML para ServiceProviderDomainAssignListRequest    |
        '\\\\\\\\\\\\\\\\\\\\/////////////////////////////////
        Dim lineaUpddate2_1 As String = "<command xsi : Type =" & Chr(34) & "ServiceProviderDomainAssignListRequest" & Chr(34) & " xmlns=" & Chr(34) & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & ">"
        Dim lineaUpddate2_2 As String = "<serviceProviderId>CloudPBX_SMB</serviceProviderId>"
        Dim lineaUpddate2_3 As String = "<domain>pruebacarlos.cl</domain>"
        Dim lineaUpddate2_4 As String = "</command>"

        Dim lineaUpdate3 As String = "</BroadsoftDocument>"

        gblUpdTotalReg = 0
        gblUpdTotaliXML = 0
        ProgressBar1.Minimum = 0
        ProgressBar1.Value = 0
        ProgressBar1.Maximum = Val(lblCMMUpdTotalRows.Text)

        'MsgBox("llegamos hasta aquí ")

        'Exit Sub

        Dim contador As Integer = 0



        For Each foundFile As String In My.Computer.FileSystem.GetFiles(gblSetPathTmp, FileIO.SearchOption.SearchAllSubDirectories, "*.*")

                My.Computer.FileSystem.DeleteFile(foundFile)
            Next

        'Try
        '        If foundFile.Count = 0 Then
        '            'tell user no files
        '            MsgBox("no hay archivos")
        '        Else
        '            MsgBox("hay archivos" & foundFile.Count.ToString)

        '            'For Each file In foundFile
        '            '    contador += 1
        '            '    MsgBox(contador.ToString())
        '            'Next
        '        End If
        '        'My.Computer.FileSystem.DeleteFile(foundFile)
        '    Next
        'Catch ex As Exception
        '    MsgBox(ex.ToString())
        'End Try


        'Exit Sub

        Dim fileIXML As String = ""
        Dim fileOXML As String = ""
        Dim codError As Integer
        Dim msgError As String = ""
        Dim regLine As String = ""
        'Dim arrLine() As String
        Dim domain As String
        Dim phoneNumber As String


        Dim accessDeviceLevel As String
        Dim accessDeviceName As String
        Dim linePort As String
        Dim contactList As String

        Dim iSql As String = ""
        Dim iXml As Integer = 0
        Dim vbrs_user_service_provider_id As String = ""
        Dim vbrs_user_group_id As String = ""
        Dim vbrs_user_phone_number As String = ""

        LblEstado.Text = "Generando XML..."
        My.Application.DoEvents()
        Dim multipleInputFile As String = gblSetPathTmp & "\multipleInputFile.txt"
        Dim linMIF As String = ""
        'FileOpen(2, multipleInputFile, OpenMode.Output, OpenAccess.Write)

        'For j = 0 To DataGridView1.Rows.Count - 1
        ProgressBar1.Value = ProgressBar1.Value + 1
        'If gblCallManager = 1 Then
        domain = DataGridView1.Rows(0).Cells(0).Value
        'MsgBox(domain)

        'Exit Sub
        gblUpdTotalReg += 1
        i = i + 1
        If i = 1 Then
            iXml += 1
            gblUpdTotaliXML += 1
            fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & iXml & ".xml"
            fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & iXml & ".xml"
            FileOpen(1, fileIXML, OpenMode.Output)
            WriteLine(1, linupd1.ToCharArray)
            WriteLine(1, linupd2.ToCharArray)
            WriteLine(1, linupd3.ToCharArray)
        End If

        'MsgBox("Se llegó hasta acá")
        'Exit Sub

        lineaUpddate1_3 = "<domain>" & domain & "</domain>"
        'lineaUpddate1_3 = "<phoneNumber>" & phoneNumber & "</phoneNumber>"
        'WriteLine(1, lineaUpdate3.ToCharArray)
        WriteLine(1, lineaUpddate1_2.ToCharArray)
        WriteLine(1, lineaUpddate1_3.ToCharArray)
        WriteLine(1, lineaUpddate1_4.ToCharArray)
        WriteLine(1, lineaUpdate3.ToCharArray)
        'WriteLine(1, linupd1_4.ToCharArray)


        MsgBox("Se llegó hasta acá")
        FileClose(1)
        Exit Sub


        '    linupd2_2 = "<serviceProviderId>" & serviceProviderId & "</serviceProviderId>"
        '    linupd2_3 = "<groupId>" & groupId & "</groupId>"
        '    linupd2_4 = "<phoneNumber>" & phoneNumberE & "</phoneNumber>"
        '    WriteLine(1, linupd2_1.ToCharArray)
        '    WriteLine(1, linupd2_2.ToCharArray)
        '    WriteLine(1, linupd2_3.ToCharArray)
        '    WriteLine(1, linupd2_4.ToCharArray)
        '    WriteLine(1, linupd2_5.ToCharArray)


        '    linupd3_2 = "<serviceProviderId>" & serviceProviderId & "</serviceProviderId>"
        '    linupd3_3 = "<groupId>" & groupId & "</groupId>"
        '    linupd3_4 = "<userId>" & userId & "</userId>"
        '    linupd3_5 = "<lastName>" & lastName & "</lastName>"
        '    linupd3_6 = "<firstName>" & firstName & "</firstName>"
        '    linupd3_7 = "<callingLineIdLastName>" & lastName & "</callingLineIdLastName>"
        '    linupd3_8 = "<callingLineIdFirstName>" & firstName & "</callingLineIdFirstName>"
        '    linupd3_10 = "<nameDialingLastName>" & lastName & "</nameDialingLastName>"
        '    linupd3_11 = "<nameDialingFirstName>" & firstName & "</nameDialingFirstName>"
        '    linupd3_13 = "<password>" & password & "</password>"

        '    WriteLine(1, linupd3_1.ToCharArray)
        '    WriteLine(1, linupd3_2.ToCharArray)
        '    WriteLine(1, linupd3_3.ToCharArray)
        '    WriteLine(1, linupd3_4.ToCharArray)
        '    WriteLine(1, linupd3_5.ToCharArray)
        '    WriteLine(1, linupd3_6.ToCharArray)
        '    WriteLine(1, linupd3_7.ToCharArray)
        '    WriteLine(1, linupd3_8.ToCharArray)
        '    WriteLine(1, linupd3_9.ToCharArray)
        '    WriteLine(1, linupd3_10.ToCharArray)
        '    WriteLine(1, linupd3_11.ToCharArray)
        '    WriteLine(1, linupd3_12.ToCharArray)
        '    WriteLine(1, linupd3_13.ToCharArray)
        '    WriteLine(1, linupd3_14.ToCharArray)
        '    WriteLine(1, linupd3_15.ToCharArray)
        '    WriteLine(1, linupd3_16.ToCharArray)
        '    WriteLine(1, linupd3_17.ToCharArray)




        '    linupd4_2 = "<userId>" & userId & "</userId>"
        '    linupd4_3 = "<phoneNumber>" & phoneNumber & "</phoneNumber>"
        '    linupd4_4 = "<extension>" & extension & "</extension>"
        '    WriteLine(1, linupd4_1.ToCharArray)
        '    WriteLine(1, linupd4_2.ToCharArray)
        '    WriteLine(1, linupd4_3.ToCharArray)
        '    WriteLine(1, linupd4_4.ToCharArray)
        '    WriteLine(1, linupd4_5.ToCharArray)
        '    WriteLine(1, linupd4_6.ToCharArray)

        '    If gblCallManager = 1 Then
        '        ' Solo para Call Manager
        '        linupd4_7CM3 = "<name>" & trunkGDEname & "</name>"
        '        linupd4_7CM4 = "<linePort>" & trunkGDElinePort & "</linePort>"
        '        WriteLine(1, linupd4_7CM1.ToCharArray)
        '        WriteLine(1, linupd4_7CM2.ToCharArray)
        '        WriteLine(1, linupd4_7CM3.ToCharArray)
        '        WriteLine(1, linupd4_7CM4.ToCharArray)
        '        WriteLine(1, linupd4_7CM5.ToCharArray)
        '        WriteLine(1, linupd4_7CM6.ToCharArray)
        '        WriteLine(1, linupd4_7CM7.ToCharArray)
        '        WriteLine(1, linupd4_7CM8.ToCharArray)
        '        WriteLine(1, linupd4_7CM9.ToCharArray)
        '    Else
        '        ' Solo para Moviles
        '        linupd4_7MV3 = "<deviceLevel>" & accessDeviceLevel & "</deviceLevel>"
        '        linupd4_7MV4 = "<deviceName>" & accessDeviceName & "</deviceName>"
        '        linupd4_7MV6 = "<linePort>" & linePort & "</linePort>"
        '        linupd4_7MV8 = "<contact>" & contactList & "</contact>"
        '        WriteLine(1, linupd4_7MV1.ToCharArray)
        '        WriteLine(1, linupd4_7MV2.ToCharArray)
        '        WriteLine(1, linupd4_7MV3.ToCharArray)
        '        WriteLine(1, linupd4_7MV4.ToCharArray)
        '        WriteLine(1, linupd4_7MV5.ToCharArray)
        '        WriteLine(1, linupd4_7MV6.ToCharArray)
        '        WriteLine(1, linupd4_7MV7.ToCharArray)
        '        WriteLine(1, linupd4_7MV8.ToCharArray)
        '        WriteLine(1, linupd4_7MV9.ToCharArray)
        '        WriteLine(1, linupd4_7MV10.ToCharArray)
        '    End If
        '    WriteLine(1, linupd4_8.ToCharArray)
        '    WriteLine(1, linupd4_9.ToCharArray)

        '    linupd5_2 = "<userId>" & userId & "</userId>"
        '    linupd5_3 = "<servicePackName>" & servicePackName & "</servicePackName>"

        '    WriteLine(1, linupd5_1.ToCharArray)
        '    WriteLine(1, linupd5_2.ToCharArray)
        '    WriteLine(1, linupd5_3.ToCharArray)
        '    WriteLine(1, linupd5_4.ToCharArray)

        '    If i = 3 Then
        '        WriteLine(1, linupd4.ToCharArray)
        '        FileClose(1)
        '        linMIF = fileIXML & ";" & fileOXML
        '        WriteLine(9, linMIF.ToCharArray)
        '        My.Application.DoEvents()
        '        i = 0
        '    End If
        'Next
        'If i > 0 Then
        '    WriteLine(1, linupd4.ToCharArray)
        '    FileClose(1)
        '    linMIF = fileIXML & ";" & fileOXML
        '    WriteLine(9, linMIF.ToCharArray)
        'End If
        ''ProgressBar1.Value = 0
        'FileClose(9)
        'My.Application.DoEvents()
        'executeShellBulk(multipleInputFile, gblCMMIdCluster, codError, msgError)
        'If codError = 0 Then

        '    'parseXML_update_CMM(codError, msgError)

        '    'My.Application.DoEvents()
        'End If
        'ProgressBar1.Minimum = 0
        'ProgressBar1.Maximum = 0
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

End Class
