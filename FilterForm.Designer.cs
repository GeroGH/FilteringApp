namespace FilteringApp
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
            this.ReuseOldFilter = new System.Windows.Forms.Button();
            this.TexBoxUserInput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrid
            // 
            this.dataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid.BackgroundColor = System.Drawing.Color.White;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(12, 38);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new System.Drawing.Size(393, 280);
            this.dataGrid.TabIndex = 0;
            this.dataGrid.Click += new System.EventHandler(this.DataGrid_Click);
            // 
            // FilterOr
            // 
            this.FilterOr.Location = new System.Drawing.Point(411, 265);
            this.FilterOr.Name = "FilterOr";
            this.FilterOr.Size = new System.Drawing.Size(135, 53);
            this.FilterOr.TabIndex = 1;
            this.FilterOr.Text = "Filter \"OR\"";
            this.FilterOr.UseVisualStyleBackColor = true;
            this.FilterOr.Click += new System.EventHandler(this.FilterOr_Click);
            // 
            // FilterAnd
            // 
            this.FilterAnd.Location = new System.Drawing.Point(411, 206);
            this.FilterAnd.Name = "FilterAnd";
            this.FilterAnd.Size = new System.Drawing.Size(135, 53);
            this.FilterAnd.TabIndex = 2;
            this.FilterAnd.Text = "Filter \"AND\"";
            this.FilterAnd.UseVisualStyleBackColor = true;
            this.FilterAnd.Click += new System.EventHandler(this.FilterAnd_Click);
            // 
            // ReuseOldFilter
            // 
            this.ReuseOldFilter.Location = new System.Drawing.Point(411, 12);
            this.ReuseOldFilter.Name = "ReuseOldFilter";
            this.ReuseOldFilter.Size = new System.Drawing.Size(135, 53);
            this.ReuseOldFilter.TabIndex = 3;
            this.ReuseOldFilter.Text = "Reuse Old Filter";
            this.ReuseOldFilter.UseVisualStyleBackColor = true;
            this.ReuseOldFilter.Click += new System.EventHandler(this.ReuseOldFilter_Click);
            // 
            // TexBoxUserInput
            // 
            this.TexBoxUserInput.Location = new System.Drawing.Point(12, 12);
            this.TexBoxUserInput.Name = "TexBoxUserInput";
            this.TexBoxUserInput.Size = new System.Drawing.Size(393, 20);
            this.TexBoxUserInput.TabIndex = 4;
            this.TexBoxUserInput.TextChanged += new System.EventHandler(this.TexBoxUserInput_TextChanged);
            // 
            // FilteringApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 326);
            this.Controls.Add(this.TexBoxUserInput);
            this.Controls.Add(this.ReuseOldFilter);
            this.Controls.Add(this.FilterAnd);
            this.Controls.Add(this.FilterOr);
            this.Controls.Add(this.dataGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FilteringApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FilteringApp 2024.06.12a";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FilerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tekla.Structures.Dialog.UIControls.DataGrid dataGrid;
        private System.Windows.Forms.Button FilterOr;
        private System.Windows.Forms.Button FilterAnd;
        private System.Windows.Forms.Button ReuseOldFilter;
        private System.Windows.Forms.TextBox TexBoxUserInput;
    }
}

