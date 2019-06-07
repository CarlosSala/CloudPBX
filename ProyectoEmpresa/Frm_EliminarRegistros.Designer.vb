<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_EliminarRegistros
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btn_canceled = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btn_search = New System.Windows.Forms.Button()
        Me.Text_Age = New System.Windows.Forms.TextBox()
        Me.Text_Address = New System.Windows.Forms.TextBox()
        Me.Text_Name = New System.Windows.Forms.TextBox()
        Me.Text_Id = New System.Windows.Forms.TextBox()
        Me.Lab_Age = New System.Windows.Forms.Label()
        Me.Lab_Address = New System.Windows.Forms.Label()
        Me.Lab_Name = New System.Windows.Forms.Label()
        Me.Lab_Id = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btn_canceled
        '
        Me.btn_canceled.Location = New System.Drawing.Point(448, 251)
        Me.btn_canceled.Name = "btn_canceled"
        Me.btn_canceled.Size = New System.Drawing.Size(93, 23)
        Me.btn_canceled.TabIndex = 6
        Me.btn_canceled.Text = "Cancelar"
        Me.btn_canceled.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btn_search)
        Me.GroupBox1.Controls.Add(Me.Text_Age)
        Me.GroupBox1.Controls.Add(Me.Text_Address)
        Me.GroupBox1.Controls.Add(Me.Text_Name)
        Me.GroupBox1.Controls.Add(Me.Text_Id)
        Me.GroupBox1.Controls.Add(Me.Lab_Age)
        Me.GroupBox1.Controls.Add(Me.Lab_Address)
        Me.GroupBox1.Controls.Add(Me.Lab_Name)
        Me.GroupBox1.Controls.Add(Me.Lab_Id)
        Me.GroupBox1.Location = New System.Drawing.Point(26, 28)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(515, 189)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        '
        'btn_search
        '
        Me.btn_search.Location = New System.Drawing.Point(176, 19)
        Me.btn_search.Name = "btn_search"
        Me.btn_search.Size = New System.Drawing.Size(93, 23)
        Me.btn_search.TabIndex = 1
        Me.btn_search.Text = "Buscar"
        Me.btn_search.UseVisualStyleBackColor = True
        '
        'Text_Age
        '
        Me.Text_Age.Location = New System.Drawing.Point(112, 134)
        Me.Text_Age.Name = "Text_Age"
        Me.Text_Age.Size = New System.Drawing.Size(58, 20)
        Me.Text_Age.TabIndex = 4
        '
        'Text_Address
        '
        Me.Text_Address.Location = New System.Drawing.Point(112, 94)
        Me.Text_Address.Name = "Text_Address"
        Me.Text_Address.Size = New System.Drawing.Size(310, 20)
        Me.Text_Address.TabIndex = 3
        '
        'Text_Name
        '
        Me.Text_Name.Location = New System.Drawing.Point(112, 56)
        Me.Text_Name.Name = "Text_Name"
        Me.Text_Name.Size = New System.Drawing.Size(310, 20)
        Me.Text_Name.TabIndex = 2
        '
        'Text_Id
        '
        Me.Text_Id.Location = New System.Drawing.Point(112, 19)
        Me.Text_Id.Name = "Text_Id"
        Me.Text_Id.Size = New System.Drawing.Size(58, 20)
        Me.Text_Id.TabIndex = 0
        '
        'Lab_Age
        '
        Me.Lab_Age.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lab_Age.Location = New System.Drawing.Point(6, 134)
        Me.Lab_Age.Name = "Lab_Age"
        Me.Lab_Age.Size = New System.Drawing.Size(100, 20)
        Me.Lab_Age.TabIndex = 3
        Me.Lab_Age.Text = "Edad"
        '
        'Lab_Address
        '
        Me.Lab_Address.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lab_Address.Location = New System.Drawing.Point(6, 94)
        Me.Lab_Address.Name = "Lab_Address"
        Me.Lab_Address.Size = New System.Drawing.Size(100, 20)
        Me.Lab_Address.TabIndex = 2
        Me.Lab_Address.Text = "Dirección"
        '
        'Lab_Name
        '
        Me.Lab_Name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lab_Name.Location = New System.Drawing.Point(6, 56)
        Me.Lab_Name.Name = "Lab_Name"
        Me.Lab_Name.Size = New System.Drawing.Size(100, 20)
        Me.Lab_Name.TabIndex = 1
        Me.Lab_Name.Text = "Nombre"
        '
        'Lab_Id
        '
        Me.Lab_Id.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lab_Id.Location = New System.Drawing.Point(6, 19)
        Me.Lab_Id.Name = "Lab_Id"
        Me.Lab_Id.Size = New System.Drawing.Size(100, 20)
        Me.Lab_Id.TabIndex = 0
        Me.Lab_Id.Text = "Id"
        '
        'Frm_EliminarRegistros
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 311)
        Me.Controls.Add(Me.btn_canceled)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "Frm_EliminarRegistros"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Eliminar Registros"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_canceled As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btn_search As Button
    Friend WithEvents Text_Age As TextBox
    Friend WithEvents Text_Address As TextBox
    Friend WithEvents Text_Name As TextBox
    Friend WithEvents Text_Id As TextBox
    Friend WithEvents Lab_Age As Label
    Friend WithEvents Lab_Address As Label
    Friend WithEvents Lab_Name As Label
    Friend WithEvents Lab_Id As Label
End Class
