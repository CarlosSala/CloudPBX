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
        Me.btn_validate_data = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.lbl_state_cloud = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btn_report_cloudpbx = New System.Windows.Forms.Button()
        Me.lbl_wait = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btn_procesar = New System.Windows.Forms.Button()
        Me.tb_file_name = New System.Windows.Forms.TextBox()
        Me.btn_browse_CSV = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.StatusStrip2 = New System.Windows.Forms.StatusStrip()
        Me.ProgressBar2 = New System.Windows.Forms.ToolStripProgressBar()
        Me.lbl_state_proxy = New System.Windows.Forms.ToolStripStatusLabel()
        Me.cb_add_proxy = New System.Windows.Forms.CheckBox()
        Me.cb_modify_proxy = New System.Windows.Forms.CheckBox()
        Me.btn_process_proxy = New System.Windows.Forms.Button()
        Me.tb_write_proxy = New System.Windows.Forms.TextBox()
        Me.lbl_proxy = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.listbox_proxy = New System.Windows.Forms.ListBox()
        Me.lbl_cloudpbx_proxy = New System.Windows.Forms.Label()
        Me.tb_groupId_proxy = New System.Windows.Forms.TextBox()
        Me.lbl_groupId_proxy = New System.Windows.Forms.Label()
        Me.btn_search_proxy = New System.Windows.Forms.Button()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.btn_process_userLicense = New System.Windows.Forms.Button()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.lbl_numUser = New System.Windows.Forms.Label()
        Me.StatusStrip3 = New System.Windows.Forms.StatusStrip()
        Me.ProgressBar3 = New System.Windows.Forms.ToolStripProgressBar()
        Me.lbl_state_userLicense = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tb_groupId_UserGetList = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.openFileDialogCSV = New System.Windows.Forms.OpenFileDialog()
        Me.ToolTipHelpButtons = New System.Windows.Forms.ToolTip(Me.components)
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.StatusStrip2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip3.SuspendLayout()
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
        Me.TabPage1.Controls.Add(Me.btn_validate_data)
        Me.TabPage1.Controls.Add(Me.StatusStrip1)
        Me.TabPage1.Controls.Add(Me.btn_report_cloudpbx)
        Me.TabPage1.Controls.Add(Me.lbl_wait)
        Me.TabPage1.Controls.Add(Me.DataGridView1)
        Me.TabPage1.Controls.Add(Me.btn_procesar)
        Me.TabPage1.Controls.Add(Me.tb_file_name)
        Me.TabPage1.Controls.Add(Me.btn_browse_CSV)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(751, 514)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Create CloudPBX"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'btn_validate_data
        '
        Me.btn_validate_data.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_validate_data.Location = New System.Drawing.Point(610, 390)
        Me.btn_validate_data.Name = "btn_validate_data"
        Me.btn_validate_data.Size = New System.Drawing.Size(132, 75)
        Me.btn_validate_data.TabIndex = 53
        Me.btn_validate_data.Text = "Validate Data"
        Me.btn_validate_data.UseVisualStyleBackColor = True
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
        Me.btn_report_cloudpbx.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn_report_cloudpbx.Location = New System.Drawing.Point(144, 390)
        Me.btn_report_cloudpbx.MaximumSize = New System.Drawing.Size(132, 75)
        Me.btn_report_cloudpbx.MinimumSize = New System.Drawing.Size(132, 75)
        Me.btn_report_cloudpbx.Name = "btn_report_cloudpbx"
        Me.btn_report_cloudpbx.Size = New System.Drawing.Size(132, 75)
        Me.btn_report_cloudpbx.TabIndex = 4
        Me.btn_report_cloudpbx.Text = "Show Report"
        Me.btn_report_cloudpbx.UseVisualStyleBackColor = True
        '
        'lbl_wait
        '
        Me.lbl_wait.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.lbl_wait.AutoSize = True
        Me.lbl_wait.BackColor = System.Drawing.Color.White
        Me.lbl_wait.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_wait.ForeColor = System.Drawing.Color.Black
        Me.lbl_wait.Location = New System.Drawing.Point(250, 233)
        Me.lbl_wait.Name = "lbl_wait"
        Me.lbl_wait.Size = New System.Drawing.Size(267, 31)
        Me.lbl_wait.TabIndex = 0
        Me.lbl_wait.Text = "Hold on a second..."
        Me.lbl_wait.Visible = False
        '
        'DataGridView1
        '
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
        Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DataGridView1.Size = New System.Drawing.Size(734, 300)
        Me.DataGridView1.TabIndex = 51
        '
        'btn_procesar
        '
        Me.btn_procesar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn_procesar.Location = New System.Drawing.Point(6, 390)
        Me.btn_procesar.MaximumSize = New System.Drawing.Size(132, 75)
        Me.btn_procesar.MinimumSize = New System.Drawing.Size(132, 75)
        Me.btn_procesar.Name = "btn_procesar"
        Me.btn_procesar.Size = New System.Drawing.Size(132, 75)
        Me.btn_procesar.TabIndex = 3
        Me.btn_procesar.Text = "Process"
        Me.btn_procesar.UseVisualStyleBackColor = True
        '
        'tb_file_name
        '
        Me.tb_file_name.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tb_file_name.Location = New System.Drawing.Point(6, 57)
        Me.tb_file_name.MaximumSize = New System.Drawing.Size(1225, 20)
        Me.tb_file_name.MinimumSize = New System.Drawing.Size(650, 20)
        Me.tb_file_name.Name = "tb_file_name"
        Me.tb_file_name.Size = New System.Drawing.Size(659, 20)
        Me.tb_file_name.TabIndex = 2
        '
        'btn_browse_CSV
        '
        Me.btn_browse_CSV.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_browse_CSV.BackColor = System.Drawing.Color.Transparent
        Me.btn_browse_CSV.BackgroundImage = CType(resources.GetObject("btn_browse_CSV.BackgroundImage"), System.Drawing.Image)
        Me.btn_browse_CSV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_browse_CSV.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_browse_CSV.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_browse_CSV.Location = New System.Drawing.Point(672, 7)
        Me.btn_browse_CSV.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_browse_CSV.MaximumSize = New System.Drawing.Size(70, 70)
        Me.btn_browse_CSV.MinimumSize = New System.Drawing.Size(70, 70)
        Me.btn_browse_CSV.Name = "btn_browse_CSV"
        Me.btn_browse_CSV.Size = New System.Drawing.Size(70, 70)
        Me.btn_browse_CSV.TabIndex = 1
        Me.btn_browse_CSV.UseVisualStyleBackColor = False
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.StatusStrip2)
        Me.TabPage2.Controls.Add(Me.cb_add_proxy)
        Me.TabPage2.Controls.Add(Me.cb_modify_proxy)
        Me.TabPage2.Controls.Add(Me.btn_process_proxy)
        Me.TabPage2.Controls.Add(Me.tb_write_proxy)
        Me.TabPage2.Controls.Add(Me.lbl_proxy)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.listbox_proxy)
        Me.TabPage2.Controls.Add(Me.lbl_cloudpbx_proxy)
        Me.TabPage2.Controls.Add(Me.tb_groupId_proxy)
        Me.TabPage2.Controls.Add(Me.lbl_groupId_proxy)
        Me.TabPage2.Controls.Add(Me.btn_search_proxy)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(792, 519)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Rebuild the Proxy File"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'StatusStrip2
        '
        Me.StatusStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProgressBar2, Me.lbl_state_proxy})
        Me.StatusStrip2.Location = New System.Drawing.Point(3, 494)
        Me.StatusStrip2.Name = "StatusStrip2"
        Me.StatusStrip2.Size = New System.Drawing.Size(786, 22)
        Me.StatusStrip2.TabIndex = 12
        Me.StatusStrip2.Text = "StatusStrip2"
        '
        'ProgressBar2
        '
        Me.ProgressBar2.Name = "ProgressBar2"
        Me.ProgressBar2.Size = New System.Drawing.Size(100, 16)
        '
        'lbl_state_proxy
        '
        Me.lbl_state_proxy.Name = "lbl_state_proxy"
        Me.lbl_state_proxy.Size = New System.Drawing.Size(85, 17)
        Me.lbl_state_proxy.Text = "lbl_state_proxy"
        '
        'cb_add_proxy
        '
        Me.cb_add_proxy.AutoSize = True
        Me.cb_add_proxy.Location = New System.Drawing.Point(258, 368)
        Me.cb_add_proxy.Name = "cb_add_proxy"
        Me.cb_add_proxy.Size = New System.Drawing.Size(74, 17)
        Me.cb_add_proxy.TabIndex = 5
        Me.cb_add_proxy.Text = "Add Proxy"
        Me.cb_add_proxy.UseVisualStyleBackColor = True
        '
        'cb_modify_proxy
        '
        Me.cb_modify_proxy.AutoSize = True
        Me.cb_modify_proxy.Location = New System.Drawing.Point(130, 368)
        Me.cb_modify_proxy.Name = "cb_modify_proxy"
        Me.cb_modify_proxy.Size = New System.Drawing.Size(86, 17)
        Me.cb_modify_proxy.TabIndex = 4
        Me.cb_modify_proxy.Text = "Modify Proxy"
        Me.cb_modify_proxy.UseVisualStyleBackColor = True
        '
        'btn_process_proxy
        '
        Me.btn_process_proxy.Location = New System.Drawing.Point(130, 432)
        Me.btn_process_proxy.Name = "btn_process_proxy"
        Me.btn_process_proxy.Size = New System.Drawing.Size(75, 23)
        Me.btn_process_proxy.TabIndex = 7
        Me.btn_process_proxy.Text = "Process"
        Me.btn_process_proxy.UseVisualStyleBackColor = True
        '
        'tb_write_proxy
        '
        Me.tb_write_proxy.Location = New System.Drawing.Point(130, 406)
        Me.tb_write_proxy.Name = "tb_write_proxy"
        Me.tb_write_proxy.Size = New System.Drawing.Size(202, 20)
        Me.tb_write_proxy.TabIndex = 6
        '
        'lbl_proxy
        '
        Me.lbl_proxy.AutoSize = True
        Me.lbl_proxy.Location = New System.Drawing.Point(61, 409)
        Me.lbl_proxy.Name = "lbl_proxy"
        Me.lbl_proxy.Size = New System.Drawing.Size(33, 13)
        Me.lbl_proxy.TabIndex = 7
        Me.lbl_proxy.Text = "Proxy"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(38, 103)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 13)
        Me.Label2.TabIndex = 6
        '
        'listbox_proxy
        '
        Me.listbox_proxy.FormattingEnabled = True
        Me.listbox_proxy.Location = New System.Drawing.Point(130, 127)
        Me.listbox_proxy.Name = "listbox_proxy"
        Me.listbox_proxy.Size = New System.Drawing.Size(202, 225)
        Me.listbox_proxy.TabIndex = 3
        '
        'lbl_cloudpbx_proxy
        '
        Me.lbl_cloudpbx_proxy.AutoSize = True
        Me.lbl_cloudpbx_proxy.Enabled = False
        Me.lbl_cloudpbx_proxy.Location = New System.Drawing.Point(338, 43)
        Me.lbl_cloudpbx_proxy.Name = "lbl_cloudpbx_proxy"
        Me.lbl_cloudpbx_proxy.Size = New System.Drawing.Size(56, 13)
        Me.lbl_cloudpbx_proxy.TabIndex = 4
        Me.lbl_cloudpbx_proxy.Text = "_cloudpbx"
        '
        'tb_groupId_proxy
        '
        Me.tb_groupId_proxy.Location = New System.Drawing.Point(130, 40)
        Me.tb_groupId_proxy.Name = "tb_groupId_proxy"
        Me.tb_groupId_proxy.Size = New System.Drawing.Size(202, 20)
        Me.tb_groupId_proxy.TabIndex = 1
        '
        'lbl_groupId_proxy
        '
        Me.lbl_groupId_proxy.AutoSize = True
        Me.lbl_groupId_proxy.Location = New System.Drawing.Point(61, 43)
        Me.lbl_groupId_proxy.Name = "lbl_groupId_proxy"
        Me.lbl_groupId_proxy.Size = New System.Drawing.Size(45, 13)
        Me.lbl_groupId_proxy.TabIndex = 1
        Me.lbl_groupId_proxy.Text = "GroupId"
        '
        'btn_search_proxy
        '
        Me.btn_search_proxy.Location = New System.Drawing.Point(130, 66)
        Me.btn_search_proxy.Name = "btn_search_proxy"
        Me.btn_search_proxy.Size = New System.Drawing.Size(75, 23)
        Me.btn_search_proxy.TabIndex = 2
        Me.btn_search_proxy.Text = "Search"
        Me.btn_search_proxy.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.btn_process_userLicense)
        Me.TabPage3.Controls.Add(Me.DataGridView2)
        Me.TabPage3.Controls.Add(Me.lbl_numUser)
        Me.TabPage3.Controls.Add(Me.StatusStrip3)
        Me.TabPage3.Controls.Add(Me.Label6)
        Me.TabPage3.Controls.Add(Me.tb_groupId_UserGetList)
        Me.TabPage3.Controls.Add(Me.Label7)
        Me.TabPage3.Controls.Add(Me.Button4)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(792, 519)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Assign User Licenses"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'btn_process_userLicense
        '
        Me.btn_process_userLicense.Location = New System.Drawing.Point(80, 457)
        Me.btn_process_userLicense.Name = "btn_process_userLicense"
        Me.btn_process_userLicense.Size = New System.Drawing.Size(75, 23)
        Me.btn_process_userLicense.TabIndex = 25
        Me.btn_process_userLicense.Text = "Process"
        Me.btn_process_userLicense.UseVisualStyleBackColor = True
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToResizeColumns = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4})
        Me.DataGridView2.Location = New System.Drawing.Point(80, 130)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DataGridView2.Size = New System.Drawing.Size(617, 321)
        Me.DataGridView2.TabIndex = 3
        '
        'Column1
        '
        Me.Column1.HeaderText = "UserId"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.HeaderText = "Basic"
        Me.Column2.Name = "Column2"
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column2.Text = ""
        '
        'Column3
        '
        Me.Column3.HeaderText = "Standard"
        Me.Column3.Name = "Column3"
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'Column4
        '
        Me.Column4.HeaderText = "Advanced"
        Me.Column4.Name = "Column4"
        Me.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'lbl_numUser
        '
        Me.lbl_numUser.AutoSize = True
        Me.lbl_numUser.Location = New System.Drawing.Point(77, 97)
        Me.lbl_numUser.Name = "lbl_numUser"
        Me.lbl_numUser.Size = New System.Drawing.Size(0, 13)
        Me.lbl_numUser.TabIndex = 24
        '
        'StatusStrip3
        '
        Me.StatusStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProgressBar3, Me.lbl_state_userLicense})
        Me.StatusStrip3.Location = New System.Drawing.Point(3, 494)
        Me.StatusStrip3.Name = "StatusStrip3"
        Me.StatusStrip3.Size = New System.Drawing.Size(786, 22)
        Me.StatusStrip3.TabIndex = 22
        Me.StatusStrip3.Text = "StatusStrip3"
        '
        'ProgressBar3
        '
        Me.ProgressBar3.Name = "ProgressBar3"
        Me.ProgressBar3.Size = New System.Drawing.Size(100, 16)
        '
        'lbl_state_userLicense
        '
        Me.lbl_state_userLicense.Name = "lbl_state_userLicense"
        Me.lbl_state_userLicense.Size = New System.Drawing.Size(116, 17)
        Me.lbl_state_userLicense.Text = "lbl_state_userLicense"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Enabled = False
        Me.Label6.Location = New System.Drawing.Point(355, 53)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 13)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "_cloudpbx"
        '
        'tb_groupId_UserGetList
        '
        Me.tb_groupId_UserGetList.Location = New System.Drawing.Point(146, 51)
        Me.tb_groupId_UserGetList.Name = "tb_groupId_UserGetList"
        Me.tb_groupId_UserGetList.Size = New System.Drawing.Size(202, 20)
        Me.tb_groupId_UserGetList.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(77, 54)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(45, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "GroupId"
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(146, 77)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 2
        Me.Button4.Text = "Search"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'openFileDialogCSV
        '
        Me.openFileDialogCSV.FileName = "OpenFileDialog1"
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
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.StatusStrip2.ResumeLayout(False)
        Me.StatusStrip2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip3.ResumeLayout(False)
        Me.StatusStrip3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents btn_browse_CSV As Button
    Friend WithEvents openFileDialogCSV As OpenFileDialog
    Friend WithEvents tb_file_name As TextBox
    Friend WithEvents btn_procesar As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents lbl_wait As Label
    Friend WithEvents ToolTipHelpButtons As ToolTip
    Friend WithEvents tb_groupId_proxy As TextBox
    Friend WithEvents lbl_groupId_proxy As Label
    Friend WithEvents btn_search_proxy As Button
    Friend WithEvents lbl_cloudpbx_proxy As Label
    Friend WithEvents listbox_proxy As ListBox
    Friend WithEvents Label2 As Label
    Friend WithEvents tb_write_proxy As TextBox
    Friend WithEvents lbl_proxy As Label
    Friend WithEvents btn_process_proxy As Button
    Friend WithEvents btn_report_cloudpbx As Button
    Friend WithEvents cb_add_proxy As CheckBox
    Friend WithEvents cb_modify_proxy As CheckBox
    Friend WithEvents StatusStrip2 As StatusStrip
    Friend WithEvents ProgressBar2 As ToolStripProgressBar
    Friend WithEvents lbl_state_proxy As ToolStripStatusLabel
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ProgressBar1 As ToolStripProgressBar
    Friend WithEvents lbl_state_cloud As ToolStripStatusLabel
    Friend WithEvents StatusStrip3 As StatusStrip
    Friend WithEvents ProgressBar3 As ToolStripProgressBar
    Friend WithEvents lbl_state_userLicense As ToolStripStatusLabel
    Friend WithEvents Label6 As Label
    Friend WithEvents tb_groupId_UserGetList As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Button4 As Button
    Friend WithEvents lbl_numUser As Label
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents btn_validate_data As Button
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewButtonColumn
    Friend WithEvents Column3 As DataGridViewButtonColumn
    Friend WithEvents Column4 As DataGridViewButtonColumn
    Friend WithEvents btn_process_userLicense As Button
End Class
