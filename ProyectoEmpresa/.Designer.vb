<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_Principal
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_Principal))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btn_procesar = New System.Windows.Forms.Button()
        Me.TextBox_FileName = New System.Windows.Forms.TextBox()
        Me.btn_BrowseCSV = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.openFileDialogCSV = New System.Windows.Forms.OpenFileDialog()
        Me.Lab_wait = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(902, 420)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Lab_wait)
        Me.TabPage1.Controls.Add(Me.DataGridView1)
        Me.TabPage1.Controls.Add(Me.btn_procesar)
        Me.TabPage1.Controls.Add(Me.TextBox_FileName)
        Me.TabPage1.Controls.Add(Me.btn_BrowseCSV)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(894, 394)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Crear Grupo"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(6, 47)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(829, 280)
        Me.DataGridView1.TabIndex = 51
        '
        'btn_procesar
        '
        Me.btn_procesar.Location = New System.Drawing.Point(747, 333)
        Me.btn_procesar.Name = "btn_procesar"
        Me.btn_procesar.Size = New System.Drawing.Size(88, 55)
        Me.btn_procesar.TabIndex = 50
        Me.btn_procesar.Text = "Procesar"
        Me.btn_procesar.UseVisualStyleBackColor = True
        '
        'TextBox_FileName
        '
        Me.TextBox_FileName.Location = New System.Drawing.Point(6, 21)
        Me.TextBox_FileName.Name = "TextBox_FileName"
        Me.TextBox_FileName.Size = New System.Drawing.Size(829, 20)
        Me.TextBox_FileName.TabIndex = 49
        '
        'btn_BrowseCSV
        '
        Me.btn_BrowseCSV.BackColor = System.Drawing.Color.Transparent
        Me.btn_BrowseCSV.BackgroundImage = CType(resources.GetObject("btn_BrowseCSV.BackgroundImage"), System.Drawing.Image)
        Me.btn_BrowseCSV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_BrowseCSV.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_BrowseCSV.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_BrowseCSV.Location = New System.Drawing.Point(842, 4)
        Me.btn_BrowseCSV.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_BrowseCSV.Name = "btn_BrowseCSV"
        Me.btn_BrowseCSV.Size = New System.Drawing.Size(45, 45)
        Me.btn_BrowseCSV.TabIndex = 48
        Me.btn_BrowseCSV.UseVisualStyleBackColor = False
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(894, 394)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Crear Dispositivos"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(894, 394)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Crear Usuarios"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(894, 394)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Crear Servicios"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(894, 394)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Asignar Dispositivos"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'openFileDialogCSV
        '
        Me.openFileDialogCSV.FileName = "OpenFileDialog1"
        '
        'Lab_wait
        '
        Me.Lab_wait.AutoSize = True
        Me.Lab_wait.BackColor = System.Drawing.Color.White
        Me.Lab_wait.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lab_wait.ForeColor = System.Drawing.Color.Black
        Me.Lab_wait.Location = New System.Drawing.Point(275, 166)
        Me.Lab_wait.Name = "Lab_wait"
        Me.Lab_wait.Size = New System.Drawing.Size(300, 31)
        Me.Lab_wait.TabIndex = 0
        Me.Lab_wait.Text = "Espere un momento..."
        Me.Lab_wait.Visible = False
        '
        'Frm_Principal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(926, 444)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "Frm_Principal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CloudPBX"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents TabPage5 As TabPage
    Friend WithEvents btn_BrowseCSV As Button
    Friend WithEvents openFileDialogCSV As OpenFileDialog
    Friend WithEvents TextBox_FileName As TextBox
    Friend WithEvents btn_procesar As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Lab_wait As Label
End Class
