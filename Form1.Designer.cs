﻿namespace FilteringApp
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrid
            // 
            this.dataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid.BackgroundColor = System.Drawing.Color.White;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(12, 12);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new System.Drawing.Size(474, 222);
            this.dataGrid.TabIndex = 0;
            // 
            // FilterOr
            // 
            this.FilterOr.Location = new System.Drawing.Point(252, 240);
            this.FilterOr.Name = "FilterOr";
            this.FilterOr.Size = new System.Drawing.Size(234, 53);
            this.FilterOr.TabIndex = 1;
            this.FilterOr.Text = "Filter \"OR\"";
            this.FilterOr.UseVisualStyleBackColor = true;
            this.FilterOr.Click += new System.EventHandler(this.FilterOr_Click);
            // 
            // FilterAnd
            // 
            this.FilterAnd.Location = new System.Drawing.Point(12, 240);
            this.FilterAnd.Name = "FilterAnd";
            this.FilterAnd.Size = new System.Drawing.Size(234, 53);
            this.FilterAnd.TabIndex = 2;
            this.FilterAnd.Text = "Filter \"AND\"";
            this.FilterAnd.UseVisualStyleBackColor = true;
            this.FilterAnd.Click += new System.EventHandler(this.FilterAnd_Click);
            // 
            // FilteringApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 299);
            this.Controls.Add(this.FilterAnd);
            this.Controls.Add(this.FilterOr);
            this.Controls.Add(this.dataGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FilteringApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FilteringApp";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Tekla.Structures.Dialog.UIControls.DataGrid dataGrid;
        private System.Windows.Forms.Button FilterOr;
        private System.Windows.Forms.Button FilterAnd;
    }
}

