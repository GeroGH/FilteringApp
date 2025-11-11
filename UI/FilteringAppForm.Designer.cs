namespace FilteringApp.UI
{
    partial class FilteringApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGrid = new Tekla.Structures.Dialog.UIControls.DataGrid();
            this.FilterOr = new System.Windows.Forms.Button();
            this.FilterAnd = new System.Windows.Forms.Button();
            this.ReuseAppViewFilter = new System.Windows.Forms.Button();
            this.TexBoxUserInput = new System.Windows.Forms.TextBox();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ReuseUserViewFilter = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.statusBar.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGrid
            // 
            this.dataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid.BackgroundColor = System.Drawing.Color.White;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(6, 19);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new System.Drawing.Size(405, 314);
            this.dataGrid.TabIndex = 0;
            this.dataGrid.Click += new System.EventHandler(this.DataGrid_Click);
            // 
            // FilterOr
            // 
            this.FilterOr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilterOr.ForeColor = System.Drawing.Color.Firebrick;
            this.FilterOr.Location = new System.Drawing.Point(11, 92);
            this.FilterOr.Name = "FilterOr";
            this.FilterOr.Size = new System.Drawing.Size(146, 68);
            this.FilterOr.TabIndex = 1;
            this.FilterOr.Text = "OR";
            this.FilterOr.UseVisualStyleBackColor = true;
            this.FilterOr.Click += new System.EventHandler(this.FilterOr_Click);
            // 
            // FilterAnd
            // 
            this.FilterAnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilterAnd.ForeColor = System.Drawing.Color.DodgerBlue;
            this.FilterAnd.Location = new System.Drawing.Point(11, 19);
            this.FilterAnd.Name = "FilterAnd";
            this.FilterAnd.Size = new System.Drawing.Size(146, 68);
            this.FilterAnd.TabIndex = 2;
            this.FilterAnd.Text = "AND";
            this.FilterAnd.UseVisualStyleBackColor = true;
            this.FilterAnd.Click += new System.EventHandler(this.FilterAnd_Click);
            // 
            // ReuseAppViewFilter
            // 
            this.ReuseAppViewFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReuseAppViewFilter.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.ReuseAppViewFilter.Location = new System.Drawing.Point(11, 19);
            this.ReuseAppViewFilter.Name = "ReuseAppViewFilter";
            this.ReuseAppViewFilter.Size = new System.Drawing.Size(146, 68);
            this.ReuseAppViewFilter.TabIndex = 3;
            this.ReuseAppViewFilter.Text = "Application";
            this.ReuseAppViewFilter.UseVisualStyleBackColor = true;
            this.ReuseAppViewFilter.Click += new System.EventHandler(this.ReuseOldFilter_Click);
            // 
            // TexBoxUserInput
            // 
            this.TexBoxUserInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TexBoxUserInput.Location = new System.Drawing.Point(6, 19);
            this.TexBoxUserInput.Name = "TexBoxUserInput";
            this.TexBoxUserInput.Size = new System.Drawing.Size(568, 22);
            this.TexBoxUserInput.TabIndex = 4;
            this.TexBoxUserInput.TextChanged += new System.EventHandler(this.TexBoxUserInput_TextChanged);
            // 
            // statusBar
            // 
            this.statusBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarLabel});
            this.statusBar.Location = new System.Drawing.Point(5, 420);
            this.statusBar.Name = "statusBar";
            this.statusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusBar.Size = new System.Drawing.Size(588, 22);
            this.statusBar.TabIndex = 5;
            this.statusBar.Text = "statusBar";
            // 
            // statusBarLabel
            // 
            this.statusBarLabel.Name = "statusBarLabel";
            this.statusBarLabel.Size = new System.Drawing.Size(135, 17);
            this.statusBarLabel.Text = "Application Ready";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TexBoxUserInput);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(580, 49);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter by text:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGrid);
            this.groupBox2.Location = new System.Drawing.Point(8, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(411, 340);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Table values:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ReuseUserViewFilter);
            this.groupBox3.Controls.Add(this.ReuseAppViewFilter);
            this.groupBox3.Location = new System.Drawing.Point(425, 72);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(163, 167);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Representations:";
            // 
            // ReuseUserViewFilter
            // 
            this.ReuseUserViewFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReuseUserViewFilter.ForeColor = System.Drawing.Color.DarkGreen;
            this.ReuseUserViewFilter.Location = new System.Drawing.Point(11, 93);
            this.ReuseUserViewFilter.Name = "ReuseUserViewFilter";
            this.ReuseUserViewFilter.Size = new System.Drawing.Size(146, 68);
            this.ReuseUserViewFilter.TabIndex = 4;
            this.ReuseUserViewFilter.Text = "User";
            this.ReuseUserViewFilter.UseVisualStyleBackColor = true;
            this.ReuseUserViewFilter.Click += new System.EventHandler(this.ReuseUserViewFilter_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.FilterOr);
            this.groupBox4.Controls.Add(this.FilterAnd);
            this.groupBox4.Location = new System.Drawing.Point(425, 245);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(163, 167);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Logic operators:";
            // 
            // FilteringApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 447);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FilteringApp";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FilteringApp 10.11.2025e";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FilerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tekla.Structures.Dialog.UIControls.DataGrid dataGrid;
        private System.Windows.Forms.Button FilterOr;
        private System.Windows.Forms.Button FilterAnd;
        private System.Windows.Forms.Button ReuseAppViewFilter;
        private System.Windows.Forms.TextBox TexBoxUserInput;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusBarLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button ReuseUserViewFilter;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}

