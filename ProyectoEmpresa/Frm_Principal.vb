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

        'My.Application.DoEvents()
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
        'My.Application.DoEvents()
    End Sub


    Sub parseXML_update_CMM(ByRef codError As Integer, ByRef msgError As String)



        'Dim Xml As XmlDocument
        'Dim NodeList As XmlNodeList
        'Dim Node As XmlNode
        'Try
        '    Xml = New XmlDocument()
        '    Xml.Load(gblSetPathTmp & "\CMM_response_tmp_1.xml")
        '    NodeList = doc.GetElementsByTagName("FileContent")
        '    'NodeList = Xml.SelectNodes("/BroadsoftDocument/sessionId/command/summary")
        '    'NodeList = Xml.SelectNodes("/songs/song")

        '    MsgBox("Nodos por Leer:  " & NodeList.Count & vbNewLine)

        '    'MsgBox(NodeList.GetNamedItem("id").Value)

        '    'For Each Node In NodeList
        '    '    With Node.Attributes
        '    '        Console.WriteLine("ID: " & .GetNamedItem("id").Value)
        '    '        Console.WriteLine("Artist: " & .GetNamedItem("artist").Value)
        '    '        Console.WriteLine("Title: " & .GetNamedItem("title").Value)
        '    '        Console.Write(vbNewLine)
        '    '    End With
        '    'Next

        'Catch ex As Exception
        '    MsgBox(ex.GetType.ToString & vbNewLine & ex.Message.ToString)
        'Finally
        '    'Console.Read()
        'End Try












        'Dim Xml As XmlDocument
        'Dim NodeList As XmlNodeList
        'Dim Node As XmlNode

        'Try
        '    Xml = New XmlDocument()
        '    Xml.Load(gblSetPathTmp & "\CMM_response_tmp_1.xml")
        '    NodeList = Xml.SelectNodes("/BroadsoftDocument/command/summary")

        '    MsgBox(NodeList.Count)
        '    'Console.WriteLine("Nodos por Leer: " & NodeList.Count & vbNewLine)

        '    For Each Node In NodeList
        '        With Node.Attributes

        '            'Console.Write("ID: " & .GetNamedItem("id").Value)
        '            'Console.WriteLine("Artist: " & .GetNamedItem("artist").Value)
        '            'Console.WriteLine("Title: " & .GetNamedItem("title").Value)
        '            'Console.Write(vbNewLine)
        '            'Console.ReadLine()
        '        End With
        '    Next

        'Catch ex As Exception
        '    'Console.WriteLine(ex.GetType.ToString & vbNewLine & ex.Message.ToString)
        'Finally
        '    'Console.Read()
        'End Try




        'Try

        '    Dim documentoXML As XmlDocument
        '    Dim nodelist As XmlNodeList
        '    Dim nodo As XmlNode
        '    documentoXML = New XmlDocument
        '    documentoXML.Load(gblSetPathTmp & "\CMM_response_tmp_1.xml")
        '    nodelist = documentoXML.SelectNodes("/BroadsoftDocument/command/summary")

        '    For Each nodo In nodelist
        '        MsgBox(nodelist.ToString)
        '        Dim minodo = nodo.ChildNodes(0).InnerText
        '        MsgBox(minodo)
        '    Next

        'Catch ex As Exception
        '    MsgBox(ex.ToString)
        'End Try



        ''MsgBox("dentro del parse")
        'Dim reader As XmlTextReader
        'reader = New XmlTextReader(gblSetPathTmp & "\CMM_response_tmp_1.xml")
        'Dim SultimaEtiqueta As String = ""

        'While (reader.Read())

        '    If (reader.NodeType = XmlNodeType.Element) Then

        '        If reader.Name = "command" Then
        '            'i += 1
        '            If reader.HasAttributes Then 'If attributes exist
        '                While reader.MoveToNextAttribute()
        '                    'Display attribute name and value.
        '                    Console.Write(" {0}='{1}'", reader.Name, reader.Value)
        '                    If reader.Name = "xsi:type" Then
        '                    End If
        '                    If reader.Value = "c:SuccessResponse" Then
        '                        MsgBox("exito")
        '                    ElseIf reader.Value = "c:ErrorResponse" Then
        '                        MsgBox("error")
        '                    End If
        '                End While
        '            End If
        '        End If
        '    ElseIf reader.NodeType = XmlNodeType.Text Then
        '        MsgBox(reader.Name.ToString)
        '    Else
        '        MsgBox(reader.Name)
        '    End If
        'End While




















        'Dim reader As XmlTextReader
        'Dim swCol As Boolean = False
        'Dim exito As Boolean = False
        'Dim parseXMl As String
        'Dim i As Integer = 0
        'Dim iSql As String = ""
        'Dim iXml As Integer = 0
        'Dim topeXml As Integer = 0
        'topeXml = gblUpdTotaliXML
        'Conexion.Open()
        'Dim dcUser = New OleDb.OleDbCommand()
        'dcUser.Connection = Conexion
        'Dim fileNameTmp As String = ""
        'For iXml = 1 To topeXml
        '    exito = False
        '    Try
        '        parseXMl = gblSetPathTmp & "\CMM_response_tmp_" & iXml & ".xml"
        '        reader = New XmlTextReader(parseXMl)
        '        Do While (reader.Read())
        '            Select Case reader.NodeType
        '                Case XmlNodeType.Element 'Display beginning of element.
        '                    '                    Console.Write("<" + reader.Name)
        '                    If reader.Name = "command" Then
        '                        i += 1
        '                        If reader.HasAttributes Then 'If attributes exist
        '                            While reader.MoveToNextAttribute()
        '                                'Display attribute name and value.
        '                                Console.Write(" {0}='{1}'", reader.Name, reader.Value)
        '                                If reader.Name = "xsi:type" Then
        '                                    If reader.Value = "c:SuccessResponse" Then
        '                                        'Try
        '                                        '    iSql = "UPDATE brs_user set brs_user_number_activation = '" & Trim(dataNAGrdUpdate.Rows(i - 1).Cells(3).Value) & "' WHERE brs_user_userId = '" & dataNAGrdUpdate.Rows(i - 1).Cells(4).Value & "';"
        '                                        '    dcUser.CommandText = iSql
        '                                        '    dcUser.ExecuteNonQuery()
        '                                        Try
        '                                            iSql = "delete from brs_update_tmp where brs_udet_userId = '" & DataGridView1.Rows(i - 1).Cells(4).Value & "';"
        '                                            dcUser.CommandText = iSql
        '                                            'dc = New OleDb.OleDbCommand(iSql, MyConn)
        '                                            dcUser.ExecuteNonQuery()
        '                                            'MessageBox.Show("Access created Succesfully for brs_user " + fila)
        '                                        Catch ex As Exception
        '                                            'MessageBox.Show(ex.Message)
        '                                            'codError = 2
        '                                            'msgError = "No actualizado en BD"
        '                                        End Try
        '                                        'Catch ex As Exception
        '                                        '    'MessageBox.Show(ex.Message)
        '                                        '    'codError = 2
        '                                        '    'msgError = "No actualizado en BD"
        '                                        'End Try
        '                                    Else
        '                                        'dataGrdUpdate.Rows(i - 1).Cells("Column2").Value = imgListUpdate.Images(2)
        '                                    End If
        '                                End If
        '                            End While
        '                        End If
        '                    End If
        '                    'Case XmlNodeType.Text 'Display the text in each element.
        '                    '    'Console.WriteLine(reader.Value)
        '                    'Case XmlNodeType.EndElement 'Display end of element.
        '                    '    If reader.Name = "command" Then
        '                    '        swCol = False
        '                    '    End If
        '            End Select
        '        Loop
        '        reader.Close()
        '    Catch ex As Exception
        '        'MsgBox("Archivo de Respuesta no ha sido encontrado!", vbExclamation, "Error")
        '        grabaLog(1, 2, "Error al leer archivo XML>" & gblSetPathTmp & "\CMM_response_tmp_" & iXml & ".xml")
        '        codError = 1
        '        msgError = "Respuesta No Generada"
        '    End Try
        'Next
        'LblEstado.Text = ""
        'Conexion.Close()
        ''actualizaCMMGrillaUpdate(0)
        'My.Application.DoEvents()

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




        'Ultima linea de todos los XML
        Dim lineaFinal As String = "</BroadsoftDocument>"

        gblUpdTotalReg = 0
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
        LblEstado.Text = "Generando XML..."
        ProgressBar1.Value = ProgressBar1.Value + 10

        'My.Application.DoEvents()
        Dim multipleInputFile As String = gblSetPathTmp & "\multipleInputFile.txt"
        Dim lineConfigFile As String = ""
        FileOpen(1, multipleInputFile, OpenMode.Output, OpenAccess.Write)


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
        'My.Application.DoEvents()
        WriteLine(1, lineConfigFile.ToCharArray)

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
        'My.Application.DoEvents()
        WriteLine(1, lineConfigFile.ToCharArray)

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
        WriteLine(1, lineConfigFile.ToCharArray)

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
        'My.Application.DoEvents()
        WriteLine(1, lineConfigFile.ToCharArray)

        'XML PARA SELECCIONAR SERVICIOS DE GRUPO (ARCHIVO EXTERNO)--------------------------------------------------------------
        indiceXML += 1
        fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
        fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
        lineConfigFile = fileIXML & ";" & fileOXML
        'My.Application.DoEvents()
        WriteLine(1, lineConfigFile.ToCharArray)
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
        WriteLine(1, lineConfigFile.ToCharArray)

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
        'My.Application.DoEvents()
        WriteLine(1, lineConfigFile.ToCharArray)

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
            'My.Application.DoEvents()
            WriteLine(1, lineConfigFile.ToCharArray)
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
            'My.Application.DoEvents()
            WriteLine(1, lineConfigFile.ToCharArray)
        Next

        'XML PARA CREAR LOS USUARIOS---------------------------------------------------------------------------------
        For j = 0 To DataGridView1.RowCount - 2
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
            fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, i_1.ToCharArray)
            WriteLine(numFile, i_2.ToCharArray)
            WriteLine(numFile, i_3.ToCharArray)
            WriteLine(numFile, i_4.ToCharArray)
            WriteLine(numFile, i_5.ToCharArray)
            i_6 = "<groupId>" & group_id & "</groupId>"
            WriteLine(numFile, i_6.ToCharArray)
            phoneNumber = DataGridView1.Rows(j).Cells(1).Value
            i_7 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
            WriteLine(numFile, i_7.ToCharArray)
            last_name = DataGridView1.Rows(j).Cells(13).Value
            i_8 = "<lastName>" & last_name & "</lastName>"
            WriteLine(numFile, i_8.ToCharArray)
            first_name = DataGridView1.Rows(j).Cells(14).Value
            i_9 = "<firstName>" & first_name & "</firstName>"
            WriteLine(numFile, i_9.ToCharArray)
            i_10 = "<callingLineIdLastName>" & last_name & "</callingLineIdLastName>"
            WriteLine(numFile, i_10.ToCharArray)
            i_11 = "<callingLineIdFirstName>" & first_name & "</callingLineIdFirstName>"
            WriteLine(numFile, i_11.ToCharArray)
            WriteLine(numFile, i_12.ToCharArray)
            department = DataGridView1.Rows(j).Cells(12).Value
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
            user_email = DataGridView1.Rows(j).Cells(15).Value
            If user_email <> "" And user_email.Length <> 0 And user_email <> Nothing Then
                i_20 = "<emailAddress>" & user_email & "</emailAddress>"
                WriteLine(numFile, i_20.ToCharArray)
            End If
            WriteLine(numFile, i_21.ToCharArray)
            user_address = DataGridView1.Rows(j).Cells(16).Value
            i_22 = "<addressLine1>" & user_address & "</addressLine1>"
            WriteLine(numFile, i_22.ToCharArray)
            user_city = DataGridView1.Rows(j).Cells(17).Value
            i_23 = "<city>" & user_city & "</city>"
            WriteLine(numFile, i_23.ToCharArray)
            WriteLine(numFile, i_24.ToCharArray)
            WriteLine(numFile, i_25.ToCharArray)
            WriteLine(numFile, lineaFinal.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            'My.Application.DoEvents()
            WriteLine(1, lineConfigFile.ToCharArray)
        Next


        'XML PARA EL PROXY---------------------------------------------------------------------------------
        For j = 0 To DataGridView1.RowCount - 2
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
            fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, j_1.ToCharArray)
            WriteLine(numFile, j_2.ToCharArray)
            WriteLine(numFile, j_3.ToCharArray)
            WriteLine(numFile, j_4.ToCharArray)
            WriteLine(numFile, j_5.ToCharArray)
            j_6 = "<groupId>" & group_id & "</groupId>"
            WriteLine(numFile, j_6.ToCharArray)
            mac = DataGridView1.Rows(j).Cells(9).Value
            j_7 = "<deviceName>DV_" & mac & "</deviceName>"
            WriteLine(numFile, j_7.ToCharArray)
            WriteLine(numFile, j_8.ToCharArray)
            proxy = DataGridView1.Rows(0).Cells(18).Value
            j_9 = "<tagValue>" & proxy & "</tagValue>"
            WriteLine(numFile, j_9.ToCharArray)
            WriteLine(numFile, j_10.ToCharArray)
            WriteLine(numFile, lineaFinal.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            'My.Application.DoEvents()
            WriteLine(1, lineConfigFile.ToCharArray)
        Next

        'XML PARA ASIGNAR DISPOSITIVOS A USUARIOS---------------------------------------------------------------------
        For j = 0 To DataGridView1.RowCount - 2
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
            fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, k_1.ToCharArray)
            WriteLine(numFile, k_2.ToCharArray)
            WriteLine(numFile, k_3.ToCharArray)
            WriteLine(numFile, k_4.ToCharArray)
            phoneNumber = DataGridView1.Rows(j).Cells(1).Value
            k_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
            WriteLine(numFile, k_5.ToCharArray)
            k_6 = "<phoneNumber>" & phoneNumber & "</phoneNumber>"
            WriteLine(numFile, k_6.ToCharArray)
            extensions = DataGridView1.Rows(j).Cells(19).Value
            k_7 = "<extension>" & extensions & "</extension>"
            WriteLine(numFile, k_7.ToCharArray)
            WriteLine(numFile, k_8.ToCharArray)
            WriteLine(numFile, k_9.ToCharArray)
            WriteLine(numFile, k_10.ToCharArray)
            WriteLine(numFile, k_11.ToCharArray)
            WriteLine(numFile, k_12.ToCharArray)
            mac = DataGridView1.Rows(j).Cells(9).Value
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
            'My.Application.DoEvents()
            WriteLine(1, lineConfigFile.ToCharArray)
        Next

        'XML PARA ASIGNAR PACK DE SERVICIOS---------------------------------------------------------------------
        For j = 0 To DataGridView1.RowCount - 2
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
            fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, l_1.ToCharArray)
            WriteLine(numFile, l_2.ToCharArray)
            WriteLine(numFile, l_3.ToCharArray)
            WriteLine(numFile, l_4.ToCharArray)
            phoneNumber = DataGridView1.Rows(j).Cells(1).Value
            l_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
            WriteLine(numFile, l_5.ToCharArray)

            If DataGridView1.Rows(j).Cells(8).Value = "Yealink-T19xE2" Then
                l_6 = "<servicePackName>Pack_Basico</servicePackName>"

            ElseIf DataGridView1.Rows(j).Cells(8).Value = "Yealink-T21xE2" Then
                l_6 = "<servicePackName>Pack_Estandar</servicePackName>"

            ElseIf DataGridView1.Rows(j).Cells(8).Value = "Yealink-T27G" Then
                l_6 = "<servicePackName>Pack_Avanzado</servicePackName>"

            Else
                l_6 = "<servicePackName>Pack_Basico</servicePackName>"
            End If
            WriteLine(numFile, l_6.ToCharArray)
            WriteLine(numFile, l_7.ToCharArray)
            WriteLine(numFile, lineaFinal.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            'My.Application.DoEvents()
            WriteLine(1, lineConfigFile.ToCharArray)
        Next

        'XML PARA OCP OUTGOING-CALLING-PLAN------------------------------------------------------------------------
        For j = 0 To DataGridView1.RowCount - 2
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
            fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, m_1.ToCharArray)
            WriteLine(numFile, m_2.ToCharArray)
            WriteLine(numFile, m_3.ToCharArray)
            WriteLine(numFile, m_4.ToCharArray)
            phoneNumber = DataGridView1.Rows(j).Cells(1).Value
            m_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
            WriteLine(numFile, m_5.ToCharArray)
            WriteLine(numFile, m_6.ToCharArray)
            WriteLine(numFile, m_7.ToCharArray)
            WriteLine(numFile, m_8.ToCharArray)
            ocp_local = DataGridView1.Rows(j).Cells(20).Value
            If ocp_local = "bloqueado" Or ocp_local = "Bloqueado" Then
                m_9 = "<local>Disallow</local>"
            ElseIf ocp_local = "desbloqueado" Or ocp_local = "Desbloqueado" Then
                m_9 = "<local>Allow</local>"
            End If
            WriteLine(numFile, m_9.ToCharArray)
            ocp_tollFree = DataGridView1.Rows(j).Cells(21).Value
            If ocp_tollFree = "bloqueado" Or ocp_tollFree = "Bloqueado" Then
                m_10 = "<tollFree>Disallow</tollFree>"
            ElseIf ocp_tollFree = "desbloqueado" Or ocp_tollFree = "Desbloqueado" Then
                m_10 = "<tollFree>Allow</tollFree>"
            End If
            WriteLine(numFile, m_10.ToCharArray)
            WriteLine(numFile, m_11.ToCharArray)
            ocp_internacional = DataGridView1.Rows(j).Cells(22).Value
            If ocp_internacional = "bloqueado" Or ocp_internacional = "Bloqueado" Then
                m_12 = "<international>Disallow</international>"
            ElseIf ocp_internacional = "desbloqueado" Or ocp_internacional = "Desbloqueado" Then
                m_12 = "<international>Allow</international>"
            End If
            WriteLine(numFile, m_12.ToCharArray)
            WriteLine(numFile, m_13.ToCharArray)
            WriteLine(numFile, m_14.ToCharArray)
            ocp_special1 = DataGridView1.Rows(j).Cells(23).Value
            If ocp_special1 = "bloqueado" Or ocp_special1 = "Bloqueado" Then
                m_15 = "<specialServicesI>Disallow</specialServicesI>"
            ElseIf ocp_special1 = "desbloqueado" Or ocp_special1 = "Desbloqueado" Then
                m_15 = "<specialServicesI>Allow</specialServicesI>"
            End If
            WriteLine(numFile, m_15.ToCharArray)
            ocp_special2 = DataGridView1.Rows(j).Cells(24).Value
            If ocp_special2 = "bloqueado" Or ocp_special2 = "Bloqueado" Then
                m_16 = "<specialServicesII>Disallow</specialServicesII>"
            ElseIf ocp_special2 = "desbloqueado" Or ocp_special2 = "Desbloqueado" Then
                m_16 = "<specialServicesII>Allow</specialServicesII>"
            End If
            WriteLine(numFile, m_16.ToCharArray)
            ocp_premium1 = DataGridView1.Rows(j).Cells(25).Value
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
            'My.Application.DoEvents()
            WriteLine(1, lineConfigFile.ToCharArray)
        Next


        'XML PARA ASIGNAR CONTRASEÑA SIP------------------------------------------------------------------------
        For j = 0 To DataGridView1.RowCount - 2
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
            fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, n_1.ToCharArray)
            WriteLine(numFile, n_2.ToCharArray)
            WriteLine(numFile, n_3.ToCharArray)
            WriteLine(numFile, n_4.ToCharArray)
            phoneNumber = DataGridView1.Rows(j).Cells(1).Value
            n_5 = "<userId>" & phoneNumber & "@" & domain & "</userId>"
            WriteLine(numFile, n_5.ToCharArray)
            n_6 = "<userName>" & phoneNumber & "</userName>"
            WriteLine(numFile, n_6.ToCharArray)
            'Dim domi As String = Mid(domain.ToString, 0, 4)
            Dim letras As String = domain.Substring(0, 4)
            n_7 = "<newPassword>" & letras & phoneNumber & "</newPassword>"
            WriteLine(numFile, n_7.ToCharArray)
            WriteLine(numFile, n_8.ToCharArray)
            WriteLine(numFile, lineaFinal.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            'My.Application.DoEvents()
            WriteLine(1, lineConfigFile.ToCharArray)
        Next

        'XML PARA ACTIVAR LOS NUMEROS------------------------------------------------------------------------
        For j = 0 To DataGridView1.RowCount - 2
            numFile += 1
            indiceXML += 1
            fileIXML = gblSetPathTmp & "\" & "CMM_request_tmp_" & indiceXML & ".xml"
            fileOXML = gblSetPathTmp & "\" & "CMM_response_tmp_" & indiceXML & ".xml"
            FileOpen(numFile, fileIXML, OpenMode.Output)
            WriteLine(numFile, o_1.ToCharArray)
            WriteLine(numFile, o_2.ToCharArray)
            WriteLine(numFile, o_3.ToCharArray)
            WriteLine(numFile, o_4.ToCharArray)
            WriteLine(numFile, o_5.ToCharArray)
            o_6 = "<groupId>" & group_id & "</groupId>"
            WriteLine(numFile, o_6.ToCharArray)
            phoneNumber = DataGridView1.Rows(j).Cells(1).Value
            o_7 = "<phoneNumber>+56-" & phoneNumber & "</phoneNumber>"
            WriteLine(numFile, o_7.ToCharArray)
            WriteLine(numFile, o_8.ToCharArray)
            WriteLine(numFile, lineaFinal.ToCharArray)
            FileClose(numFile)
            lineConfigFile = fileIXML & ";" & fileOXML
            'My.Application.DoEvents()
            WriteLine(1, lineConfigFile.ToCharArray)
        Next

        FileClose(1)
        gblUpdTotaliXML = indiceXML

        LblEstado.Text = "Creación de archivos finalizada"
        ProgressBar1.Value = ProgressBar1.Value + 30
        'Exit Sub

        'MsgBox(My.Settings.gblCMMIdCluster.ToString())

        'parseXML_update_CMM(codError, msgError)

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