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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelDefault1 = new RognusFramework.Componentes.LabelDefault();
            this.textBoxDefault1 = new RognusFramework.Componentes.TextBoxDefault();
            this.labelDefault2 = new RognusFramework.Componentes.LabelDefault();
            this.textBoxDefault2 = new RognusFramework.Componentes.TextBoxDefault();
            this.dataGridViewCheckBoxColumnSelect11 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect10 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect9 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect8 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect6 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect5 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect4 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect3 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect2 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect1 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewCheckBoxColumnSelect7 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.dataGridViewBusca1 = new RognusFramework.Componentes.DataGridViewBusca();
            this.bdsListaParametrosDeBackup = new RognusFramework.Componentes.BindingSourceDefault(this.components);
            this.dataGridViewCheckBoxColumnSelect12 = new RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect();
            this.servidorDataGridViewTextBoxColumn = new RognusFramework.Componentes.DataGridViewTextBoxColumnDefault();
            this.nomebancoBancoDadosDataGridViewTextBoxColumn = new RognusFramework.Componentes.DataGridViewTextBoxColumnDefault();
            this.gbxResultadosBusca.SuspendLayout();
            this.gbxFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBusca1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsListaParametrosDeBackup)).BeginInit();
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
            // dataGridViewCheckBoxColumnSelect11
            // 
            this.dataGridViewCheckBoxColumnSelect11.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect11.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect11.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect11.Name = "dataGridViewCheckBoxColumnSelect11";
            this.dataGridViewCheckBoxColumnSelect11.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect11.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect11.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect11.Visible = false;
            this.dataGridViewCheckBoxColumnSelect11.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect10
            // 
            this.dataGridViewCheckBoxColumnSelect10.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect10.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect10.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect10.Name = "dataGridViewCheckBoxColumnSelect10";
            this.dataGridViewCheckBoxColumnSelect10.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect10.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect10.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect10.Visible = false;
            this.dataGridViewCheckBoxColumnSelect10.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect9
            // 
            this.dataGridViewCheckBoxColumnSelect9.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect9.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect9.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect9.Name = "dataGridViewCheckBoxColumnSelect9";
            this.dataGridViewCheckBoxColumnSelect9.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect9.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect9.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect9.Visible = false;
            this.dataGridViewCheckBoxColumnSelect9.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect8
            // 
            this.dataGridViewCheckBoxColumnSelect8.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect8.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect8.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect8.Name = "dataGridViewCheckBoxColumnSelect8";
            this.dataGridViewCheckBoxColumnSelect8.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect8.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect8.Visible = false;
            this.dataGridViewCheckBoxColumnSelect8.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect6
            // 
            this.dataGridViewCheckBoxColumnSelect6.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect6.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect6.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect6.Name = "dataGridViewCheckBoxColumnSelect6";
            this.dataGridViewCheckBoxColumnSelect6.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect6.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect6.Visible = false;
            this.dataGridViewCheckBoxColumnSelect6.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect5
            // 
            this.dataGridViewCheckBoxColumnSelect5.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect5.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect5.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect5.Name = "dataGridViewCheckBoxColumnSelect5";
            this.dataGridViewCheckBoxColumnSelect5.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect5.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect5.Visible = false;
            this.dataGridViewCheckBoxColumnSelect5.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect4
            // 
            this.dataGridViewCheckBoxColumnSelect4.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect4.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect4.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect4.Name = "dataGridViewCheckBoxColumnSelect4";
            this.dataGridViewCheckBoxColumnSelect4.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect4.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect4.Visible = false;
            this.dataGridViewCheckBoxColumnSelect4.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect3
            // 
            this.dataGridViewCheckBoxColumnSelect3.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect3.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect3.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect3.Name = "dataGridViewCheckBoxColumnSelect3";
            this.dataGridViewCheckBoxColumnSelect3.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect3.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect3.Visible = false;
            this.dataGridViewCheckBoxColumnSelect3.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect2
            // 
            this.dataGridViewCheckBoxColumnSelect2.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect2.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect2.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect2.Name = "dataGridViewCheckBoxColumnSelect2";
            this.dataGridViewCheckBoxColumnSelect2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect2.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect2.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect1
            // 
            this.dataGridViewCheckBoxColumnSelect1.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect1.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect1.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect1.Name = "dataGridViewCheckBoxColumnSelect1";
            this.dataGridViewCheckBoxColumnSelect1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect1.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect1.Width = 30;
            // 
            // dataGridViewCheckBoxColumnSelect7
            // 
            this.dataGridViewCheckBoxColumnSelect7.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect7.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect7.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect7.Name = "dataGridViewCheckBoxColumnSelect7";
            this.dataGridViewCheckBoxColumnSelect7.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect7.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect7.UsuarioPodeAlterarVisible = false;
            this.dataGridViewCheckBoxColumnSelect7.Visible = false;
            this.dataGridViewCheckBoxColumnSelect7.Width = 30;
            // 
            // dataGridViewBusca1
            // 
            this.dataGridViewBusca1.AllowUserToAddRows = false;
            this.dataGridViewBusca1.AllowUserToDeleteRows = false;
            this.dataGridViewBusca1.AllowUserToResizeRows = false;
            this.dataGridViewBusca1.AutoGenerateColumns = false;
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
            this.dataGridViewCheckBoxColumnSelect12,
            this.servidorDataGridViewTextBoxColumn,
            this.nomebancoBancoDadosDataGridViewTextBoxColumn});
            this.dataGridViewBusca1.DataSource = this.bdsListaParametrosDeBackup;
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
            this.dataGridViewBusca1.MultiSelect = false;
            this.dataGridViewBusca1.Name = "dataGridViewBusca1";
            this.dataGridViewBusca1.ReadOnly = true;
            this.dataGridViewBusca1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataGridViewBusca1.RowHeadersVisible = false;
            this.dataGridViewBusca1.RowHeadersWidth = 25;
            this.dataGridViewBusca1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBusca1.Size = new System.Drawing.Size(348, 210);
            this.dataGridViewBusca1.StandardTab = true;
            this.dataGridViewBusca1.TabIndex = 0;
            // 
            // bdsListaParametrosDeBackup
            // 
            this.bdsListaParametrosDeBackup.DataSource = typeof(BackupAutomaticoCervantes.ParametrosBackupModel);
            this.bdsListaParametrosDeBackup.SupportsSorting_User = true;
            // 
            // dataGridViewCheckBoxColumnSelect12
            // 
            this.dataGridViewCheckBoxColumnSelect12.ExportarParaExcel = false;
            this.dataGridViewCheckBoxColumnSelect12.Frozen = true;
            this.dataGridViewCheckBoxColumnSelect12.HeaderText = "";
            this.dataGridViewCheckBoxColumnSelect12.Name = "dataGridViewCheckBoxColumnSelect12";
            this.dataGridViewCheckBoxColumnSelect12.ReadOnly = true;
            this.dataGridViewCheckBoxColumnSelect12.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumnSelect12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumnSelect12.ToolTipText = "Selecionado";
            this.dataGridViewCheckBoxColumnSelect12.Visible = false;
            this.dataGridViewCheckBoxColumnSelect12.Width = 30;
            // 
            // servidorDataGridViewTextBoxColumn
            // 
            this.servidorDataGridViewTextBoxColumn.DataPropertyName = "Servidor";
            this.servidorDataGridViewTextBoxColumn.ExportarParaExcel = true;
            this.servidorDataGridViewTextBoxColumn.HeaderText = "Servidor";
            this.servidorDataGridViewTextBoxColumn.Name = "servidorDataGridViewTextBoxColumn";
            this.servidorDataGridViewTextBoxColumn.ReadOnly = true;
            this.servidorDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // nomebancoBancoDadosDataGridViewTextBoxColumn
            // 
            this.nomebancoBancoDadosDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nomebancoBancoDadosDataGridViewTextBoxColumn.DataPropertyName = "NomebancoBancoDados";
            this.nomebancoBancoDadosDataGridViewTextBoxColumn.ExportarParaExcel = true;
            this.nomebancoBancoDadosDataGridViewTextBoxColumn.HeaderText = "Banco de Dados";
            this.nomebancoBancoDadosDataGridViewTextBoxColumn.Name = "nomebancoBancoDadosDataGridViewTextBoxColumn";
            this.nomebancoBancoDadosDataGridViewTextBoxColumn.ReadOnly = true;
            this.nomebancoBancoDadosDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // FrmListaBackups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 363);
            this.Name = "FrmListaBackups";
            this.Text = "FrmListaBackups";
            this.gbxResultadosBusca.ResumeLayout(false);
            this.gbxFiltros.ResumeLayout(false);
            this.gbxFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBusca1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsListaParametrosDeBackup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private RognusFramework.Componentes.TextBoxDefault textBoxDefault2;
        private RognusFramework.Componentes.TextBoxDefault textBoxDefault1;
        private RognusFramework.Componentes.LabelDefault labelDefault2;
        private RognusFramework.Componentes.LabelDefault labelDefault1;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect1;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect2;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect3;
        private RognusFramework.Componentes.BindingSourceDefault bdsListaParametrosDeBackup;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect4;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect5;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect7;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect6;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect8;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect9;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect10;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect11;
        private RognusFramework.Componentes.DataGridViewBusca dataGridViewBusca1;
        private RognusFramework.Componentes.DataGridViewCheckBoxColumnSelect dataGridViewCheckBoxColumnSelect12;
        private RognusFramework.Componentes.DataGridViewTextBoxColumnDefault servidorDataGridViewTextBoxColumn;
        private RognusFramework.Componentes.DataGridViewTextBoxColumnDefault nomebancoBancoDadosDataGridViewTextBoxColumn;
    }
}