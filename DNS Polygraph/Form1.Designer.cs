namespace DnsPolygraph
{
    partial class MainWin
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            this.label6 = new System.Windows.Forms.Label();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flushDNSCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.domainCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbScroll = new System.Windows.Forms.CheckBox();
            this.cboResolvers = new System.Windows.Forms.ComboBox();
            this.cboInterfaces = new System.Windows.Forms.ComboBox();
            this.cbPassive = new System.Windows.Forms.CheckBox();
            this.numRows = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.cbWhois = new System.Windows.Forms.CheckBox();
            this.cboFilters = new System.Windows.Forms.ComboBox();
            this.cbMatched = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(819, 383);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 20);
            this.label6.TabIndex = 7;
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCapture.Enabled = false;
            this.btnCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapture.Location = new System.Drawing.Point(760, 29);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(68, 23);
            this.btnCapture.TabIndex = 15;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(834, 29);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(68, 23);
            this.btnStop.TabIndex = 16;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(918, 28);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::DNS_Polygraph.Properties.Resources.save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.saveToolStripMenuItem.Text = "Export to CSV";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearGridToolStripMenuItem,
            this.flushDNSCacheToolStripMenuItem,
            this.domainCountToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // clearGridToolStripMenuItem
            // 
            this.clearGridToolStripMenuItem.Image = global::DNS_Polygraph.Properties.Resources.del;
            this.clearGridToolStripMenuItem.Name = "clearGridToolStripMenuItem";
            this.clearGridToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.clearGridToolStripMenuItem.Text = "Clear Data                  (F4)";
            this.clearGridToolStripMenuItem.Click += new System.EventHandler(this.ClearGridToolStripMenuItem_Click);
            // 
            // flushDNSCacheToolStripMenuItem
            // 
            this.flushDNSCacheToolStripMenuItem.Name = "flushDNSCacheToolStripMenuItem";
            this.flushDNSCacheToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.flushDNSCacheToolStripMenuItem.Text = "Flush DNS Cache      (F5)";
            this.flushDNSCacheToolStripMenuItem.Click += new System.EventHandler(this.FlushDNSCacheToolStripMenuItem_Click);
            // 
            // domainCountToolStripMenuItem
            // 
            this.domainCountToolStripMenuItem.Name = "domainCountToolStripMenuItem";
            this.domainCountToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.domainCountToolStripMenuItem.Text = "Domain Counter       (F6)";
            this.domainCountToolStripMenuItem.Click += new System.EventHandler(this.DomainCountToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // cbScroll
            // 
            this.cbScroll.AutoSize = true;
            this.cbScroll.Checked = true;
            this.cbScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbScroll.Location = new System.Drawing.Point(17, 61);
            this.cbScroll.Name = "cbScroll";
            this.cbScroll.Size = new System.Drawing.Size(98, 21);
            this.cbScroll.TabIndex = 19;
            this.cbScroll.Text = "Auto Scroll";
            this.cbScroll.UseVisualStyleBackColor = true;
            // 
            // cboResolvers
            // 
            this.cboResolvers.BackColor = System.Drawing.SystemColors.Window;
            this.cboResolvers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboResolvers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboResolvers.ForeColor = System.Drawing.SystemColors.MenuText;
            this.cboResolvers.FormattingEnabled = true;
            this.cboResolvers.Location = new System.Drawing.Point(588, 30);
            this.cboResolvers.Name = "cboResolvers";
            this.cboResolvers.Size = new System.Drawing.Size(160, 23);
            this.cboResolvers.TabIndex = 20;
            this.cboResolvers.Text = "Resolvers";
            this.cboResolvers.SelectedIndexChanged += new System.EventHandler(this.CboResolvers_SelectedIndexChanged);
            // 
            // cboInterfaces
            // 
            this.cboInterfaces.BackColor = System.Drawing.SystemColors.Window;
            this.cboInterfaces.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboInterfaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboInterfaces.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboInterfaces.FormattingEnabled = true;
            this.cboInterfaces.Location = new System.Drawing.Point(15, 30);
            this.cboInterfaces.Name = "cboInterfaces";
            this.cboInterfaces.Size = new System.Drawing.Size(560, 23);
            this.cboInterfaces.TabIndex = 14;
            this.cboInterfaces.Text = "Capture Interfaces";
            this.cboInterfaces.SelectedIndexChanged += new System.EventHandler(this.cboInterfaces_SelectedIndexChanged);
            // 
            // cbPassive
            // 
            this.cbPassive.AutoSize = true;
            this.cbPassive.Location = new System.Drawing.Point(100, 61);
            this.cbPassive.Name = "cbPassive";
            this.cbPassive.Size = new System.Drawing.Size(111, 21);
            this.cbPassive.TabIndex = 18;
            this.cbPassive.Text = "Pasive mode";
            this.cbPassive.UseVisualStyleBackColor = true;
            // 
            // numRows
            // 
            this.numRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numRows.BackColor = System.Drawing.Color.Transparent;
            this.numRows.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRows.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numRows.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.numRows.Location = new System.Drawing.Point(820, 382);
            this.numRows.Name = "numRows";
            this.numRows.Size = new System.Drawing.Size(82, 20);
            this.numRows.TabIndex = 24;
            this.numRows.Text = "0";
            this.numRows.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFilter.Location = new System.Drawing.Point(311, 383);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(225, 22);
            this.txtFilter.TabIndex = 17;
            this.txtFilter.Click += new System.EventHandler(this.TxtFilter_Click);
            this.txtFilter.TextChanged += new System.EventHandler(this.TxtFilter_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(264, 389);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 40;
            this.label1.Text = "Search";
            // 
            // dgvMain
            // 
            this.dgvMain.AllowDrop = true;
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMain.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvMain.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.DarkRed;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMain.ColumnHeadersHeight = 22;
            this.dgvMain.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvMain.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvMain.Location = new System.Drawing.Point(17, 87);
            this.dgvMain.Margin = new System.Windows.Forms.Padding(0);
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dgvMain.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.RowHeadersWidth = 18;
            this.dgvMain.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvMain.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvMain.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Black;
            this.dgvMain.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvMain.RowTemplate.Height = 18;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Size = new System.Drawing.Size(885, 281);
            this.dgvMain.TabIndex = 38;
            this.dgvMain.TabStop = false;
            this.dgvMain.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvMain_DataBindingComplete);
            this.dgvMain.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DgvMain_RowPostPaint);
            this.dgvMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DgvMain_MouseClick);
            // 
            // cbWhois
            // 
            this.cbWhois.AutoSize = true;
            this.cbWhois.Location = new System.Drawing.Point(346, 61);
            this.cbWhois.Name = "cbWhois";
            this.cbWhois.Size = new System.Drawing.Size(222, 21);
            this.cbWhois.TabIndex = 46;
            this.cbWhois.Text = "Automatic Whois for Unrelated";
            this.cbWhois.UseVisualStyleBackColor = true;
            // 
            // cboFilters
            // 
            this.cboFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilters.FormattingEnabled = true;
            this.cboFilters.Location = new System.Drawing.Point(17, 383);
            this.cboFilters.Name = "cboFilters";
            this.cboFilters.Size = new System.Drawing.Size(224, 24);
            this.cboFilters.TabIndex = 18;
            this.cboFilters.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CboFiltersDrawItem);
            // 
            // cbMatched
            // 
            this.cbMatched.AutoSize = true;
            this.cbMatched.Location = new System.Drawing.Point(194, 61);
            this.cbMatched.Name = "cbMatched";
            this.cbMatched.Size = new System.Drawing.Size(187, 21);
            this.cbMatched.TabIndex = 47;
            this.cbMatched.Text = "Hide matched responses";
            this.cbMatched.UseVisualStyleBackColor = true;
            this.cbMatched.CheckedChanged += new System.EventHandler(this.CbMatched_CheckedChanged);
            // 
            // MainWin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(918, 415);
            this.Controls.Add(this.cbMatched);
            this.Controls.Add(this.cboFilters);
            this.Controls.Add(this.cbWhois);
            this.Controls.Add(this.dgvMain);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.numRows);
            this.Controls.Add(this.cboResolvers);
            this.Controls.Add(this.cbScroll);
            this.Controls.Add(this.cbPassive);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.cboInterfaces);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWin";
            this.Text = "DNS Polygraph v0.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWin_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbScroll;
        private System.Windows.Forms.ComboBox cboResolvers;
        private System.Windows.Forms.ComboBox cboInterfaces;
        private System.Windows.Forms.ToolStripMenuItem clearGridToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbPassive;
        private System.Windows.Forms.Label numRows;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flushDNSCacheToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbWhois;
        private System.Windows.Forms.ComboBox cboFilters;
        private System.Windows.Forms.CheckBox cbMatched;
        private System.Windows.Forms.ToolStripMenuItem domainCountToolStripMenuItem;

        public struct Row
        {
            public string Date { get; set; }
            public string Resolver { get; set; }
            public string Domain { get; set; }
            public string ResponseUntrust { get; set; }
            public string ResponseTrust { get; set; }
            public string R { get; set; }
            public string Info { get; set; }
            public bool Diferent { get; set; }
        }

    }



}

