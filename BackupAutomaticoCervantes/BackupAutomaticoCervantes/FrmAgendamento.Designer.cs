namespace BackupAutomaticoCervantes
{
    partial class FrmAgendamento
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
            this.groupBoxDefault1 = new RognusFramework.Componentes.GroupBoxDefault();
            this.chkDias = new System.Windows.Forms.CheckedListBox();
            this.chkTodosOsDias = new RognusFramework.Componentes.CheckBoxDefault();
            this.labelDefault1 = new RognusFramework.Componentes.LabelDefault();
            this.dtpHora = new RognusFramework.Componentes.DateTimePickerDefault();
            this.horarioAgendamentoModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bdsHorarioAgendamento = new RognusFramework.Componentes.BindingSourceDefault(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).BeginInit();
            this.groupBoxDefault1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horarioAgendamentoModelBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsHorarioAgendamento)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDefault
            // 
            this.pnlDefault.Location = new System.Drawing.Point(0, 238);
            this.pnlDefault.Size = new System.Drawing.Size(161, 30);
            // 
            // groupBoxDefault1
            // 
            this.groupBoxDefault1.AutoState_User = true;
            this.groupBoxDefault1.Controls.Add(this.chkDias);
            this.groupBoxDefault1.Controls.Add(this.chkTodosOsDias);
            this.groupBoxDefault1.Controls.Add(this.labelDefault1);
            this.groupBoxDefault1.Controls.Add(this.dtpHora);
            this.groupBoxDefault1.EnabledAnteriorBusca = false;
            this.groupBoxDefault1.Location = new System.Drawing.Point(12, 12);
            this.groupBoxDefault1.Name = "groupBoxDefault1";
            this.groupBoxDefault1.Size = new System.Drawing.Size(138, 217);
            this.groupBoxDefault1.TabIndex = 18;
            this.groupBoxDefault1.TabStop = false;
            // 
            // chkDias
            // 
            this.chkDias.FormattingEnabled = true;
            this.chkDias.Location = new System.Drawing.Point(8, 42);
            this.chkDias.Name = "chkDias";
            this.chkDias.Size = new System.Drawing.Size(117, 139);
            this.chkDias.TabIndex = 3;
            // 
            // chkTodosOsDias
            // 
            this.chkTodosOsDias.AutoSize = true;
            this.chkTodosOsDias.AutoState_User = true;
            this.chkTodosOsDias.BackColor = System.Drawing.Color.Transparent;
            this.chkTodosOsDias.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.bdsHorarioAgendamento, "ExecutarTodosOsDias", true));
            this.chkTodosOsDias.EnabledAnteriorBusca = false;
            this.chkTodosOsDias.Location = new System.Drawing.Point(6, 19);
            this.chkTodosOsDias.Name = "chkTodosOsDias";
            this.chkTodosOsDias.Size = new System.Drawing.Size(92, 17);
            this.chkTodosOsDias.TabIndex = 2;
            this.chkTodosOsDias.Text = "Todos os dias";
            this.chkTodosOsDias.UseVisualStyleBackColor = false;
            this.chkTodosOsDias.CheckedChanged += new System.EventHandler(this.chkTodosOsDias_CheckedChanged);
            // 
            // labelDefault1
            // 
            this.labelDefault1.AutoSize = true;
            this.labelDefault1.AutoState_User = true;
            this.labelDefault1.BackColor = System.Drawing.Color.Transparent;
            this.labelDefault1.EnabledAnteriorBusca = false;
            this.labelDefault1.Location = new System.Drawing.Point(6, 190);
            this.labelDefault1.Name = "labelDefault1";
            this.labelDefault1.Size = new System.Drawing.Size(33, 13);
            this.labelDefault1.TabIndex = 1;
            this.labelDefault1.Text = "Hora:";
            // 
            // dtpHora
            // 
            this.dtpHora.AutoState_User = true;
            this.dtpHora.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bdsHorarioAgendamento, "Hora", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "t"));
            this.dtpHora.EnabledAnteriorBusca = false;
            this.dtpHora.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpHora.Location = new System.Drawing.Point(45, 187);
            this.dtpHora.Name = "dtpHora";
            this.dtpHora.Size = new System.Drawing.Size(80, 20);
            this.dtpHora.TabIndex = 0;
            this.dtpHora.Value = new System.DateTime(2025, 4, 29, 16, 19, 43, 510);
            // 
            // horarioAgendamentoModelBindingSource
            // 
            this.horarioAgendamentoModelBindingSource.DataSource = typeof(BackupAutomaticoCervantes.Models.HorarioAgendamentoModel);
            // 
            // bdsHorarioAgendamento
            // 
            this.bdsHorarioAgendamento.DataSource = typeof(BackupAutomaticoCervantes.Models.HorarioAgendamentoModel);
            this.bdsHorarioAgendamento.SupportsSorting_User = true;
            // 
            // FrmAgendamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(161, 268);
            this.Controls.Add(this.groupBoxDefault1);
            this.Location = new System.Drawing.Point(177, 307);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(177, 307);
            this.MinimizeBox = false;
            this.Name = "FrmAgendamento";
            this.Text = "Agendamento";
            this.ValidarControls = false;
            this.Controls.SetChildIndex(this.pnlDefault, 0);
            this.Controls.SetChildIndex(this.groupBoxDefault1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).EndInit();
            this.groupBoxDefault1.ResumeLayout(false);
            this.groupBoxDefault1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horarioAgendamentoModelBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsHorarioAgendamento)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RognusFramework.Componentes.GroupBoxDefault groupBoxDefault1;
        private RognusFramework.Componentes.DateTimePickerDefault dtpHora;
        private RognusFramework.Componentes.CheckBoxDefault chkTodosOsDias;
        private RognusFramework.Componentes.LabelDefault labelDefault1;
        private System.Windows.Forms.CheckedListBox chkDias;
        private RognusFramework.Componentes.BindingSourceDefault bdsHorarioAgendamento;
        private System.Windows.Forms.BindingSource horarioAgendamentoModelBindingSource;
    }
}