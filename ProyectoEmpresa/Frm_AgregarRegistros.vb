
Imports System.Data.OleDb ' manejo de BD Access
Public Class Frm_AgregarRegistros

    'El método load se carga primero que nada y automaticamente
    'Private Sub Frm_AgregarRegistros_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    Se llama al metodo Interface_Entrada()
    '    Interface_Entrada()
    '    MsgBox(My.Application.Info.DirectoryPath & My.Settings.SetDatabase)
    'End Sub

    'Private Sub GroupBox1_Enter(sender As Object, e As EventArgs)

    'End Sub

    'Private Sub Interface_Entrada()
    '    Se ejecuta cuando se carga el formulario
    '    Lab_Id.Enabled = True
    '    Text_Id.Enabled = True
    '    btn_search.Enabled = True

    '    Lab_Name.Enabled = False
    '    Text_Name.Enabled = True
    '    Lab_Address.Enabled = False
    '    Text_Address.Enabled = False
    '    Lab_Age.Enabled = False
    '    Text_Age.Enabled = False
    '    btn_save.Enabled = True
    'End Sub

    'Private Sub Interface_Datos()
    '    Se ejecuta cuando se ingresan nuevos datos
    '    Lab_Id.Enabled = False
    '    Text_Id.Enabled = False
    '    btn_search.Enabled = False

    '    Lab_Name.Enabled = True
    '    Text_Name.Enabled = True
    '    Lab_Address.Enabled = True
    '    Text_Address.Enabled = True
    '    Lab_Age.Enabled = True
    '    Text_Age.Enabled = True
    '    btn_save.Enabled = True
    'End Sub

    'Private Sub Btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
    '    If Buscar_Registro(Text_Id.Text) = True Then
    '        'Mostrar mensaje que diga que el registro existe
    '        MsgBox("El registro existe")
    '        Text_Id.Focus()
    '    Else
    '        Interface_Datos()
    '        Limpiar_Formulario()
    '        Text_Name.Focus()
    '    End If
    'End Sub

    'Buscar registro en la base de datos
    'Function Buscar_Registro(ByVal xId As String) As Boolean

    '    'Convertir cadena a numero
    '    Dim Id As String
    '    Id = xId.ToString()

    '    'Conexión, el puente entre la BD y el software
    '    Dim Conexion As New OleDbConnection
    '    Conexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase

    '    'C:\Users\cs\Desktop\VisualStudioProjects\CloudPBX\ProyectoEmpresa\bin\Proyecto_Empresa_BBDD\Database1.accdb' 

    '    'cadena SQL, mensajero o cadena de conexion
    '    Dim CadenaSql As String = "SELECT * FROM brs_create_group WHERE dominio = " & Id

    '    'Adaptador, es una vacija que recibe todo tipo de datos, ej xml
    '    Dim Adaptador As New OleDbDataAdapter(CadenaSql, Conexion)

    '    'Data set, es el esqueleto de una base de datos
    '    Dim Ds As New DataSet

    '    'Llamar al Data Set
    '    Conexion.Open() 'se abre la conexion
    '    Adaptador.Fill(Ds) 'El adaptador llena con datos al data set o esqueleto vacio
    '    Conexion.Close()

    '    'Contar registro
    '    If (Ds.Tables(0).Rows.Count = 0) Then
    '        'No se encontró el registro
    '        Return False
    '    Else
    '        'se encontró el registro
    '        'cargar los textBox del formulario con la informacion del registro encontrado
    '        Text_Name.Text = Ds.Tables(0).Rows(0)("numbers").ToString()
    '        Text_Address.Text = Ds.Tables(0).Rows(0)("direccion").ToString()
    '        Text_Age.Text = Ds.Tables(0).Rows(0)("edad").ToString()
    '        Ds.Dispose()
    '        Return True
    '    End If
    'End Function

    'Function GuardarDatos() As Boolean

    '    Se abre el archivo CSV en modo lectura y se le asigna un id
    '    FileOpen(1, "C:\Users\cs\Downloads\primera_parte.csv", OpenMode.Input)

    '    Dim regLine As String = ""
    '    Dim arrLine() As String

    '    Se crean variables que contendran las valores que luego se guardaran en access
    '    Convertir al tipo de dato que espera recibir la BD
    '    Dim Dominio As String = ""
    '    Dim Numeros As Integer

    '    Dim totalReg As Integer = 0
    '    Dim ierr = 0

    '    Dim letras(10) As String
    '    Dim contador As Integer = 0


    '    letras(0) = "s"
    '    letras(1) = "a"
    '    letras(2) = "l"
    '    letras(3) = "u"
    '    letras(4) = "d"
    '    letras(5) = "o"
    '    letras(6) = "t"
    '    letras(7) = "i"
    '    letras(8) = "m"
    '    letras(9) = "e"

    '    Conexión, el puente entre la BD y el software
    '    Dim Conexion As New OleDbConnection
    '    Conexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase

    '    While Not EOF(1)
    '        totalReg += 1
    '        regLine = LineInput(1)
    '        arrLine = Split(regLine, ";")
    '        Dominio = arrLine(0).ToString()
    '        Numeros = Convert.ToInt32(arrLine(1))

    '        MsgBox(arrLine(0) & " " & arrLine(1))

    '        Instruccion Sql 
    '        Se insertan datos en la tabla Personal, el nombre de la tabla va en minusculas
    '        Dim cadenaSQl As String = "INSERT INTO brs_create_group (dominio, numbers)"
    '        cadenaSQl = cadenaSQl + " VALUES ( '" & Dominio & "',"
    '        cadenaSQl = cadenaSQl + "         " & Numeros & ")"

    '        Crear un comando
    '        Dim Comando As OleDbCommand = Conexion.CreateCommand()
    '        Comando.CommandText = cadenaSQl

    '        Ejecutar la consulta de accion (agregan registros)

    '        Conexion.Open()
    '        Try

    '            MsgBox("se abrio correctamente la bd")
    '            Comando.ExecuteNonQuery()
    '        Catch ex As Exception
    '            MsgBox(ex.ToString)
    '        End Try
    '        contador += 1
    '        Conexion.Close()
    '    End While







    '    Return True
    'End Function
    'Private Sub Btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
    '    Se guarda informacion en la base de datos
    '    If Text_Id.Text.Length = 0 Or Text_Name.Text.Length = 0 Then
    '        MsgBox("Todos los campos son obligatorios", MsgBoxStyle.Exclamation)
    '    Else
    '        GuardarDatos()
    '        Interface_Entrada()
    '        Se limpian los campos
    '    Limpiar_Formulario()
    '        Text_Id.Focus()
    '    End If
    'End Sub

    'Private Sub Limpiar_Formulario()
    '    Text_Name.Clear()
    '    Text_Address.Clear()
    '    Text_Age.Clear()
    'End Sub
    'Private Sub Btn_canceled_Click(sender As Object, e As EventArgs)
    '    Me.Close()
    'End Sub

    'Private Sub Btn_search_Click(sender As Object, e As EventArgs)

    'End Sub
End Class