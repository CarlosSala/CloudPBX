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
        Me.btn_input_csv = New System.Windows.Forms.Button()
        Me.btn_errors = New System.Windows.Forms.Button()
        Me.btn_files_processed = New System.Windows.Forms.Button()
        Me.btn_show_logs = New System.Windows.Forms.Button()
        Me.btn_show_report = New System.Windows.Forms.Button()
        Me.btn_fileWatcher = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.lbl_state_cloud = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btn_report_cloudpbx = New System.Windows.Forms.Button()
        Me.lbl_wait = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.tb_file_name = New System.Windows.Forms.TextBox()
        Me.openFileDialogCSV = New System.Windows.Forms.OpenFileDialog()
        Me.ToolTipHelpButtons = New System.Windows.Forms.ToolTip(Me.components)
        Me.lbl_status_fileWatcher = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.MaximumSize = New System.Drawing.Size(1325, 690)
        Me.TabControl1.MinimumSize = New System.Drawing.Size(750, 540)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(759, 540)
        Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lbl_status_fileWatcher)
        Me.TabPage1.Controls.Add(Me.btn_input_csv)
        Me.TabPage1.Controls.Add(Me.btn_errors)
        Me.TabPage1.Controls.Add(Me.btn_files_processed)
        Me.TabPage1.Controls.Add(Me.btn_show_logs)
        Me.TabPage1.Controls.Add(Me.btn_show_report)
        Me.TabPage1.Controls.Add(Me.btn_fileWatcher)
        Me.TabPage1.Controls.Add(Me.StatusStrip1)
        Me.TabPage1.Controls.Add(Me.btn_report_cloudpbx)
        Me.TabPage1.Controls.Add(Me.lbl_wait)
        Me.TabPage1.Controls.Add(Me.DataGridView1)
        Me.TabPage1.Controls.Add(Me.tb_file_name)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(751, 514)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Create CloudPBX"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'btn_input_csv
        '
        Me.btn_input_csv.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_input_csv.Location = New System.Drawing.Point(110, 398)
        Me.btn_input_csv.Name = "btn_input_csv"
        Me.btn_input_csv.Size = New System.Drawing.Size(100, 58)
        Me.btn_input_csv.TabIndex = 59
        Me.btn_input_csv.Text = "Input CSV"
        Me.btn_input_csv.UseVisualStyleBackColor = True
        '
        'btn_errors
        '
        Me.btn_errors.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_errors.Location = New System.Drawing.Point(216, 398)
        Me.btn_errors.Name = "btn_errors"
        Me.btn_errors.Size = New System.Drawing.Size(100, 58)
        Me.btn_errors.TabIndex = 58
        Me.btn_errors.Text = "Files CSV No Processed"
        Me.btn_errors.UseVisualStyleBackColor = True
        '
        'btn_files_processed
        '
        Me.btn_files_processed.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_files_processed.Location = New System.Drawing.Point(322, 398)
        Me.btn_files_processed.Name = "btn_files_processed"
        Me.btn_files_processed.Size = New System.Drawing.Size(100, 58)
        Me.btn_files_processed.TabIndex = 57
        Me.btn_files_processed.Text = "Files CSV Processed"
        Me.btn_files_processed.UseVisualStyleBackColor = True
        '
        'btn_show_logs
        '
        Me.btn_show_logs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_show_logs.Location = New System.Drawing.Point(428, 398)
        Me.btn_show_logs.Name = "btn_show_logs"
        Me.btn_show_logs.Size = New System.Drawing.Size(100, 58)
        Me.btn_show_logs.TabIndex = 56
        Me.btn_show_logs.Text = "Logs"
        Me.btn_show_logs.UseVisualStyleBackColor = True
        '
        'btn_show_report
        '
        Me.btn_show_report.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_show_report.Location = New System.Drawing.Point(534, 398)
        Me.btn_show_report.Name = "btn_show_report"
        Me.btn_show_report.Size = New System.Drawing.Size(100, 58)
        Me.btn_show_report.TabIndex = 55
        Me.btn_show_report.Text = "Reports"
        Me.btn_show_report.UseVisualStyleBackColor = True
        '
        'btn_fileWatcher
        '
        Me.btn_fileWatcher.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_fileWatcher.BackColor = System.Drawing.Color.Transparent
        Me.btn_fileWatcher.BackgroundImage = CType(resources.GetObject("btn_fileWatcher.BackgroundImage"), System.Drawing.Image)
        Me.btn_fileWatcher.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_fileWatcher.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_fileWatcher.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_fileWatcher.Location = New System.Drawing.Point(670, 7)
        Me.btn_fileWatcher.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_fileWatcher.MaximumSize = New System.Drawing.Size(70, 70)
        Me.btn_fileWatcher.MinimumSize = New System.Drawing.Size(70, 70)
        Me.btn_fileWatcher.Name = "btn_fileWatcher"
        Me.btn_fileWatcher.Size = New System.Drawing.Size(70, 70)
        Me.btn_fileWatcher.TabIndex = 54
        Me.btn_fileWatcher.UseVisualStyleBackColor = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProgressBar1, Me.lbl_state_cloud})
        Me.StatusStrip1.Location = New System.Drawing.Point(3, 489)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(745, 22)
        Me.StatusStrip1.TabIndex = 52
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'lbl_state_cloud
        '
        Me.lbl_state_cloud.Name = "lbl_state_cloud"
        Me.lbl_state_cloud.Size = New System.Drawing.Size(85, 17)
        Me.lbl_state_cloud.Text = "lbl_state_cloud"
        '
        'btn_report_cloudpbx
        '
        Me.btn_report_cloudpbx.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_report_cloudpbx.Location = New System.Drawing.Point(640, 398)
        Me.btn_report_cloudpbx.MaximumSize = New System.Drawing.Size(132, 75)
        Me.btn_report_cloudpbx.MinimumSize = New System.Drawing.Size(100, 58)
        Me.btn_report_cloudpbx.Name = "btn_report_cloudpbx"
        Me.btn_report_cloudpbx.Size = New System.Drawing.Size(100, 58)
        Me.btn_report_cloudpbx.TabIndex = 4
        Me.btn_report_cloudpbx.Text = "Generate Report"
        Me.btn_report_cloudpbx.UseVisualStyleBackColor = True
        '
        'lbl_wait
        '
        Me.lbl_wait.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_wait.AutoSize = True
        Me.lbl_wait.BackColor = System.Drawing.Color.White
        Me.lbl_wait.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_wait.ForeColor = System.Drawing.Color.Black
        Me.lbl_wait.Location = New System.Drawing.Point(246, 249)
        Me.lbl_wait.Name = "lbl_wait"
        Me.lbl_wait.Size = New System.Drawing.Size(267, 31)
        Me.lbl_wait.TabIndex = 0
        Me.lbl_wait.Text = "Hold on a second..."
        Me.lbl_wait.Visible = False
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.DarkGray
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.DataGridView1.GridColor = System.Drawing.Color.DimGray
        Me.DataGridView1.Location = New System.Drawing.Point(6, 84)
        Me.DataGridView1.MaximumSize = New System.Drawing.Size(1300, 440)
        Me.DataGridView1.MinimumSize = New System.Drawing.Size(727, 300)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DataGridView1.Size = New System.Drawing.Size(734, 300)
        Me.DataGridView1.TabIndex = 51
        '
        'tb_file_name
        '
        Me.tb_file_name.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tb_file_name.Location = New System.Drawing.Point(6, 57)
        Me.tb_file_name.MaximumSize = New System.Drawing.Size(1225, 20)
        Me.tb_file_name.MinimumSize = New System.Drawing.Size(657, 20)
        Me.tb_file_name.Name = "tb_file_name"
        Me.tb_file_name.Size = New System.Drawing.Size(657, 20)
        Me.tb_file_name.TabIndex = 2
        '
        'openFileDialogCSV
        '
        Me.openFileDialogCSV.FileName = "OpenFileDialog1"
        '
        'lbl_status_fileWatcher
        '
        Me.lbl_status_fileWatcher.AutoSize = True
        Me.lbl_status_fileWatcher.Location = New System.Drawing.Point(6, 25)
        Me.lbl_status_fileWatcher.Name = "lbl_status_fileWatcher"
        Me.lbl_status_fileWatcher.Size = New System.Drawing.Size(127, 13)
        Me.lbl_status_fileWatcher.TabIndex = 60
        Me.lbl_status_fileWatcher.Text = "System File Watcher OFF"
        '
        'Frm_Principal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(1366, 768)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
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
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents openFileDialogCSV As OpenFileDialog
    Friend WithEvents tb_file_name As TextBox
    Friend WithEvents btn_procesar As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents lbl_wait As Label
    Friend WithEvents ToolTipHelpButtons As ToolTip
    Friend WithEvents btn_report_cloudpbx As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ProgressBar1 As ToolStripProgressBar
    Friend WithEvents lbl_state_cloud As ToolStripStatusLabel
    Friend WithEvents btn_validate_data As Button
    Friend WithEvents btn_fileWatcher As Button
    Friend WithEvents btn_browse_CSV As Button
    Friend WithEvents btn_errors As Button
    Friend WithEvents btn_files_processed As Button
    Friend WithEvents btn_show_logs As Button
    Friend WithEvents btn_show_report As Button
    Friend WithEvents btn_input_csv As Button
    Friend WithEvents lbl_status_fileWatcher As Label
End Class
