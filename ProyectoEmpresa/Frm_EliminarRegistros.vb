Imports System.Data.OleDb
Public Class Frm_EliminarRegistros
    Private Sub Text_Age_TextChanged(sender As Object, e As EventArgs) Handles Text_Age.TextChanged

    End Sub

    Private Sub Frm_EliminarRegistros_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Interface_Entrada()
    End Sub

    Private Sub Interface_Entrada()
        'se ejecuta cuando se carga el formulario
        Lab_Id.Enabled = True
        Text_Id.Enabled = True
        btn_search.Enabled = True

        Lab_Name.Enabled = False
        Text_Name.Enabled = False
        Lab_Address.Enabled = False
        Text_Address.Enabled = False
        Lab_Age.Enabled = False
        Text_Age.Enabled = False


    End Sub

    Private Sub Interface_Datos()
        'se ejecuta cuando se ingresan nuervos datos
        Lab_Id.Enabled = False
        Text_Id.Enabled = False
        btn_search.Enabled = False


        Lab_Name.Enabled = True
        Text_Name.Enabled = True
        Lab_Address.Enabled = True
        Text_Address.Enabled = True
        Lab_Age.Enabled = True
        Text_Age.Enabled = True


    End Sub

    Function Buscar_Registro(ByVal xId As String) As Boolean

        'convertir cadena a numero
        Dim Id As Double
        Id = Convert.ToInt32(xId)

        'conexión, el puente entre la BD y el software
        Dim Conexion As New OleDbConnection
        Conexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase
        'cadena SQL, mensajero o cadena de conexion
        Dim CadenaSql As String = "SELECT * FROM Personal WHERE id = " & Id

        'Adaptador, es una vacija recibe todo tipo de datos, ej xml
        Dim Adaptador As New OleDbDataAdapter(CadenaSql, Conexion)

        'Data set, es un esquelo de una base de datos
        Dim Ds As New DataSet

        'Llamar al Data Set
        Conexion.Open() 'se abre la conexion
        Adaptador.Fill(Ds) 'El adaptador de con datos al data set o esqueleto vacio
        Conexion.Close()

        'contar registro
        If (Ds.Tables(0).Rows.Count = 0) Then
            'no se encontró el registro
            Return False
        Else
            'se encontró el registro
            'cargar los textBox del formulario con la informacion del registro encontrado
            Text_Name.Text = Ds.Tables(0).Rows(0)("nombre").ToString()
            Text_Address.Text = Ds.Tables(0).Rows(0)("direccion").ToString()
            Text_Age.Text = Ds.Tables(0).Rows(0)("edad").ToString()
            Ds.Dispose()
            Return True
        End If
    End Function

    Function Eliminar_Registro(ByVal Id As String) As Boolean
        'Convertir
        Dim Cod As Integer = Convert.ToInt32(Id)

        'conexión, el puente entre la BD y el software
        Dim Conexion As New OleDbConnection
        Conexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & My.Application.Info.DirectoryPath & My.Settings.SetDatabase

        'Instruccion SQL 
        'Modifica datos en la tabla Personal 
        Dim cadenaSQl As String = "DELETE FROM personal WHERE id = " & Cod

        'Crear un comando
        Dim Comando As OleDbCommand = Conexion.CreateCommand()
        Comando.CommandText = cadenaSQl

        'Ejecutar la consulta de accion (agregan registros)
        Conexion.Open()
        Comando.ExecuteNonQuery()
        Conexion.Close()
        Return True
    End Function

    Private Sub Limpiar_Formulario()
        Text_Name.Clear()
        Text_Address.Clear()
        Text_Age.Clear()
    End Sub

    Private Sub Btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        'buscar el registro que se desea eliminar
        If Buscar_Registro(Text_Id.Text) = True Then

            'Esta seguro de eliminar el registro actual
            Dim Respuesta As DialogResult = MessageBox.Show("¿Esta seguro que desea eliminar este registro", "Pregunta Importante", MessageBoxButtons.YesNo)

            'Evalua la respuesta del usuario en el msgbox
            If Respuesta = DialogResult.Yes Then
                'Eliminar registro
                Eliminar_Registro(Text_Id.Text)
                Limpiar_Formulario()
                Text_Id.Focus()
            Else
                'No eliminar registro
                Limpiar_Formulario()
                Text_Id.Focus()
            End If
        Else
            'No existe el registro
            MsgBox("El registro no existe")
            Text_Id.Focus()
        End If

    End Sub

    Private Sub Btn_canceled_Click(sender As Object, e As EventArgs) Handles btn_canceled.Click
        Me.Close()
    End Sub
End Class