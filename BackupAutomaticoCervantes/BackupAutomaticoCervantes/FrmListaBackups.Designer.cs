namespace BackupAutomaticoCervantes
{
    partial class FrmListaBackups
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelDefault1 = new RognusFramework.Componentes.LabelDefault();
            this.textBoxDefault1 = new RognusFramework.Componentes.TextBoxDefault();
            this.labelDefault2 = new RognusFramework.Componentes.LabelDefault();
            this.textBoxDefault2 = new RognusFramework.Componentes.TextBoxDefault();
            this.dataGridViewBusca1 = new RognusFramework.Componentes.DataGridViewBusca();
            this.dataGridViewCheckBoxColumnSelect1 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect2 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.gbxResultadosBusca.SuspendLayout();
            this.gbxFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBusca1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxResultadosBusca
            // 
            this.gbxResultadosBusca.Controls.Add(this.dataGridViewBusca1);
            this.gbxResultadosBusca.Location = new System.Drawing.Point(7, 89);
            this.gbxResultadosBusca.Size = new System.Drawing.Size(368, 236);
            // 
            // gbxFiltros
            // 
            this.gbxFiltros.Controls.Add(this.textBoxDefault2);
            this.gbxFiltros.Controls.Add(this.textBoxDefault1);
            this.gbxFiltros.Controls.Add(this.labelDefault2);
            this.gbxFiltros.Controls.Add(this.labelDefault1);
            this.gbxFiltros.Location = new System.Drawing.Point(7, 3);
            this.gbxFiltros.Size = new System.Drawing.Size(368, 80);
            this.gbxFiltros.Controls.SetChildIndex(this.pnlBtnBuscar, 0);
            this.gbxFiltros.Controls.SetChildIndex(this.labelDefault1, 0);
            this.gbxFiltros.Controls.SetChildIndex(this.labelDefault2, 0);
            this.gbxFiltros.Controls.SetChildIndex(this.textBoxDefault1, 0);
            this.gbxFiltros.Controls.SetChildIndex(this.textBoxDefault2, 0);
            // 
            // pnlBtnBuscar
            // 
            this.pnlBtnBuscar.Location = new System.Drawing.Point(284, 49);
            // 
            // pnlDefault
            // 
            this.pnlDefault.Location = new System.Drawing.Point(0, 333);
            this.pnlDefault.Size = new System.Drawing.Size(387, 30);
            // 
            // labelDefault1
            // 
            this.labelDefault1.AutoSize = true;
            this.labelDefault1.AutoState_User = true;
            this.labelDefault1.BackColor = System.Drawing.Color.Transparent;
            this.labelDefault1.EnabledAnteriorBusca = false;
            this.labelDefault1.Location = new System.Drawing.Point(5, 26);
            this.labelDefault1.Name = "labelDefault1";
            this.labelDefault1.Size = new System.Drawing.Size(49, 13);
            this.labelDefault1.TabIndex = 2;
            this.labelDefault1.Text = "Servidor:";
            // 
            // textBoxDefault1
            // 
            this.textBoxDefault1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDefault1.AutoState_User = true;
            this.textBoxDefault1.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxDefault1.EnabledAnteriorBusca = false;
            this.textBoxDefault1.Location = new System.Drawing.Point(60, 23);
            this.textBoxDefault1.Name = "textBoxDefault1";
            this.textBoxDefault1.Size = new System.Drawing.Size(289, 20);
            this.textBoxDefault1.TabIndex = 3;
            this.textBoxDefault1.TipoValor_User = RognusFramework.Componentes.TextBoxDefault.TiposValor.Indefinido;
            this.textBoxDefault1.ValorPadrao_User = "0";
            // 
            // labelDefault2
            // 
            this.labelDefault2.AutoSize = true;
            this.labelDefault2.AutoState_User = true;
            this.labelDefault2.BackColor = System.Drawing.Color.Transparent;
            this.labelDefault2.EnabledAnteriorBusca = false;
            this.labelDefault2.Location = new System.Drawing.Point(19, 52);
            this.labelDefault2.Name = "labelDefault2";
            this.labelDefault2.Size = new System.Drawing.Size(35, 13);
            this.labelDefault2.TabIndex = 2;
            this.labelDefault2.Text = "Porta:";
            // 
            // textBoxDefault2
            // 
            this.textBoxDefault2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDefault2.AutoState_User = true;
            this.textBoxDefault2.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxDefault2.EnabledAnteriorBusca = false;
            this.textBoxDefault2.Location = new System.Drawing.Point(60, 49);
            this.textBoxDefault2.Name = "textBoxDefault2";
            this.textBoxDefault2.Size = new System.Drawing.Size(211, 20);
            this.textBoxDefault2.TabIndex = 3;
            this.textBoxDefault2.TipoValor_User = RognusFramework.Componentes.TextBoxDefault.TiposValor.Indefinido;
            this.textBoxDefault2.ValorPadrao_User = "0";
            // 
            // dataGridViewBusca1
            // 
            this.dataGridViewBusca1.AllowUserToAddRows = false;
            this.dataGridViewBusca1.AllowUserToDeleteRows = false;
            this.dataGridViewBusca1.AllowUserToResizeRows = false;
            this.dataGridViewBusca1.AutoState_User = true;
            this.dataGridViewBusca1.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dataGridViewBusca1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewBusca1.CmsConfigVisibleColumns = null;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBusca1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewBusca1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBusca1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumnSelect2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(44)))), ((int)(((byte)(104)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewBusca1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewBusca1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBusca1.EnabledAnteriorBusca = false;
            this.dataGridViewBusca1.Location = new System.Drawing.Point(10, 16);
            this.dataGridViewBusca1.Name = "dataGridViewBusca1";
            this.dataGridViewBusca1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataGridViewBusca1.RowHeadersVisible = false;
            this.dataGridViewBusca1.RowHeadersWidth = 25;
            this.dataGridViewBusca1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBusca1.Size = new System.Drawing.Size(348, 210);
            this.dataGridViewBusca1.StandardTab = true;
            this.dataGridViewBusca1.TabIndex = 0;
            // 
            // dataGridViewCheckBoxColumnSelect1
            // 
            this.dataGridViewCheckBoxColumnSelect1.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect1.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect1.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect1.Name = "dataGridViewCheckBoxColumnSelect1";
            this.dataGridViewCheckBoxColumnSelect1.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect1.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect1.Visible = false;
            this.dataGridViewCheckBoxColumnSelect1.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect2
            // 
            this.dataGridViewCheckBoxColumnSelect2.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect2.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect2.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect2.Name = "dataGridViewCheckBoxColumnSelect2";
            this.dataGridViewCheckBoxColumnSelect2.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect2.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect2.Visible = false;
            this.dataGridViewCheckBoxColumnSelect2.Width = 30;
            // 
            // FrmListaBackups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 363);
            this.MultiSelect_User = true;
            this.Name = "FrmListaBackups";
            this.Text = "FrmListaBackups";
            this.gbxResultadosBusca.ResumeLayout(false);
            this.gbxFiltros.ResumeLayout(false);
            this.gbxFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBusca1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private RognusFramework.Componentes.TextBoxDefault textBoxDefault2;
        private RognusFramework.Componentes.TextBoxDefault textBoxDefault1;
        private RognusFramework.Componentes.LabelDefault labelDefault2;
        private RognusFramework.Componentes.LabelDefault labelDefault1;
        private RognusFramework.Componentes.DataGridViewBusca dataGridViewBusca1;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect1;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect2;
    }
}