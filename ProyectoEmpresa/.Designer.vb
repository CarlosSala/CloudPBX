<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm_Principal
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_Principal))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.btn_report_cloudpbx = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.Lbl_state = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Lbl_wait = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btn_procesar = New System.Windows.Forms.Button()
        Me.TextBox_FileName = New System.Windows.Forms.TextBox()
        Me.btn_Browse_CSV = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.openFileDialogCSV = New System.Windows.Forms.OpenFileDialog()
        Me.ToolTipHelpButtons = New System.Windows.Forms.ToolTip(Me.components)
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.MinimumSize = New System.Drawing.Size(800, 625)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(800, 625)
        Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.btn_report_cloudpbx)
        Me.TabPage1.Controls.Add(Me.StatusStrip1)
        Me.TabPage1.Controls.Add(Me.Lbl_wait)
        Me.TabPage1.Controls.Add(Me.DataGridView1)
        Me.TabPage1.Controls.Add(Me.btn_procesar)
        Me.TabPage1.Controls.Add(Me.TextBox_FileName)
        Me.TabPage1.Controls.Add(Me.btn_Browse_CSV)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(792, 599)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Create CloudPBX"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'btn_report_cloudpbx
        '
        Me.btn_report_cloudpbx.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_report_cloudpbx.Location = New System.Drawing.Point(508, 490)
        Me.btn_report_cloudpbx.MaximumSize = New System.Drawing.Size(132, 75)
        Me.btn_report_cloudpbx.MinimumSize = New System.Drawing.Size(132, 75)
        Me.btn_report_cloudpbx.Name = "btn_report_cloudpbx"
        Me.btn_report_cloudpbx.Size = New System.Drawing.Size(132, 75)
        Me.btn_report_cloudpbx.TabIndex = 53
        Me.btn_report_cloudpbx.Text = "Ver informe"
        Me.btn_report_cloudpbx.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.StatusStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProgressBar1, Me.Lbl_state})
        Me.StatusStrip1.Location = New System.Drawing.Point(6, 574)
        Me.StatusStrip1.MaximumSize = New System.Drawing.Size(400, 22)
        Me.StatusStrip1.MinimumSize = New System.Drawing.Size(269, 22)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(269, 22)
        Me.StatusStrip1.TabIndex = 52
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'Lbl_state
        '
        Me.Lbl_state.Name = "Lbl_state"
        Me.Lbl_state.Size = New System.Drawing.Size(119, 17)
        Me.Lbl_state.Text = "ToolStripStatusLabel1"
        '
        'Lbl_wait
        '
        Me.Lbl_wait.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.Lbl_wait.AutoSize = True
        Me.Lbl_wait.BackColor = System.Drawing.Color.White
        Me.Lbl_wait.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lbl_wait.ForeColor = System.Drawing.Color.Black
        Me.Lbl_wait.Location = New System.Drawing.Point(255, 288)
        Me.Lbl_wait.Name = "Lbl_wait"
        Me.Lbl_wait.Size = New System.Drawing.Size(300, 31)
        Me.Lbl_wait.TabIndex = 0
        Me.Lbl_wait.Text = "Espere un momento..."
        Me.Lbl_wait.Visible = False
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.DataGridView1.GridColor = System.Drawing.SystemColors.ActiveCaption
        Me.DataGridView1.Location = New System.Drawing.Point(6, 84)
        Me.DataGridView1.MaximumSize = New System.Drawing.Size(1307, 400)
        Me.DataGridView1.MinimumSize = New System.Drawing.Size(780, 400)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DataGridView1.Size = New System.Drawing.Size(780, 400)
        Me.DataGridView1.TabIndex = 51
        '
        'btn_procesar
        '
        Me.btn_procesar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_procesar.Location = New System.Drawing.Point(647, 490)
        Me.btn_procesar.MaximumSize = New System.Drawing.Size(132, 75)
        Me.btn_procesar.MinimumSize = New System.Drawing.Size(132, 75)
        Me.btn_procesar.Name = "btn_procesar"
        Me.btn_procesar.Size = New System.Drawing.Size(132, 75)
        Me.btn_procesar.TabIndex = 50
        Me.btn_procesar.Text = "Procesar"
        Me.btn_procesar.UseVisualStyleBackColor = True
        '
        'TextBox_FileName
        '
        Me.TextBox_FileName.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_FileName.Location = New System.Drawing.Point(6, 57)
        Me.TextBox_FileName.MaximumSize = New System.Drawing.Size(1230, 20)
        Me.TextBox_FileName.MinimumSize = New System.Drawing.Size(700, 20)
        Me.TextBox_FileName.Name = "TextBox_FileName"
        Me.TextBox_FileName.Size = New System.Drawing.Size(700, 20)
        Me.TextBox_FileName.TabIndex = 49
        '
        'btn_Browse_CSV
        '
        Me.btn_Browse_CSV.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_Browse_CSV.BackColor = System.Drawing.Color.Transparent
        Me.btn_Browse_CSV.BackgroundImage = CType(resources.GetObject("btn_Browse_CSV.BackgroundImage"), System.Drawing.Image)
        Me.btn_Browse_CSV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_Browse_CSV.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_Browse_CSV.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Browse_CSV.Location = New System.Drawing.Point(709, 7)
        Me.btn_Browse_CSV.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_Browse_CSV.MaximumSize = New System.Drawing.Size(70, 70)
        Me.btn_Browse_CSV.MinimumSize = New System.Drawing.Size(70, 70)
        Me.btn_Browse_CSV.Name = "btn_Browse_CSV"
        Me.btn_Browse_CSV.Size = New System.Drawing.Size(70, 70)
        Me.btn_Browse_CSV.TabIndex = 48
        Me.btn_Browse_CSV.UseVisualStyleBackColor = False
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.CheckBox2)
        Me.TabPage2.Controls.Add(Me.CheckBox1)
        Me.TabPage2.Controls.Add(Me.Button2)
        Me.TabPage2.Controls.Add(Me.TextBox2)
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.ListBox1)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Controls.Add(Me.TextBox1)
        Me.TabPage2.Controls.Add(Me.Label1)
        Me.TabPage2.Controls.Add(Me.Button1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(792, 599)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Rebuild the file"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(281, 402)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(74, 17)
        Me.CheckBox2.TabIndex = 11
        Me.CheckBox2.Text = "Add Proxy"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(153, 402)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(86, 17)
        Me.CheckBox1.TabIndex = 10
        Me.CheckBox1.Text = "Modify Proxy"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(153, 466)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "Process"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(153, 440)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(202, 20)
        Me.TextBox2.TabIndex = 8
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(84, 443)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(33, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Proxy"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(54, 139)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 13)
        Me.Label2.TabIndex = 6
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(153, 161)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(202, 225)
        Me.ListBox1.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Enabled = False
        Me.Label3.Location = New System.Drawing.Point(361, 77)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "_cloudpbx"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(153, 74)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(202, 20)
        Me.TextBox1.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(84, 77)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "GroupId"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(153, 100)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Search"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(792, 599)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Crear Usuarios"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(792, 599)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Crear Servicios"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(792, 599)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Asignar Dispositivos"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'openFileDialogCSV
        '
        Me.openFileDialogCSV.FileName = "OpenFileDialog1"
        '
        'Frm_Principal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(816, 647)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1366, 768)
        Me.MinimumSize = New System.Drawing.Size(832, 686)
        Me.Name = "Frm_Principal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Voxcom - CloudPBX"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents TabPage5 As TabPage
    Friend WithEvents btn_Browse_CSV As Button
    Friend WithEvents openFileDialogCSV As OpenFileDialog
    Friend WithEvents TextBox_FileName As TextBox
    Friend WithEvents btn_procesar As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Lbl_wait As Label
    Friend WithEvents ToolTipHelpButtons As ToolTip
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ProgressBar1 As ToolStripProgressBar
    Friend WithEvents Lbl_state As ToolStripStatusLabel
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents btn_report_cloudpbx As Button
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents CheckBox1 As CheckBox
End Class
