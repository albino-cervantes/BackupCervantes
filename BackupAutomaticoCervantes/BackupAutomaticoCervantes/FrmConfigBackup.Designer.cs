namespace BackupAutomaticoCervantes
{
    partial class FrmConfigBackup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbxPropriedadesConexao = new RognusFramework.Componentes.GroupBoxDefault();
            this.btnAbrirCaminhoPgDump = new RognusFramework.Componentes.ButtonDefault();
            this.btnAbrirDiretorioSalvarBackupEm = new RognusFramework.Componentes.ButtonDefault();
            this.labelDefault7 = new RognusFramework.Componentes.LabelDefault();
            this.labelDefault6 = new RognusFramework.Componentes.LabelDefault();
            this.txtCaminhoPgDump = new RognusFramework.Componentes.TextBoxDefault();
            this.bdsParametros = new RognusFramework.Componentes.BindingSourceDefault(this.components);
            this.txtCaminhoSalvarBackup = new RognusFramework.Componentes.TextBoxDefault();
            this.txtBancoDados = new RognusFramework.Componentes.TextBoxDefault();
            this.labelDefault5 = new RognusFramework.Componentes.LabelDefault();
            this.txtSenhaBancoDados = new RognusFramework.Componentes.TextBoxDefault();
            this.labelDefault4 = new RognusFramework.Componentes.LabelDefault();
            this.txtUsuarioBancoDados = new RognusFramework.Componentes.TextBoxDefault();
            this.labelDefault3 = new RognusFramework.Componentes.LabelDefault();
            this.txtPorta = new RognusFramework.Componentes.TextBoxDefault();
            this.labelDefault2 = new RognusFramework.Componentes.LabelDefault();
            this.txtServidor = new RognusFramework.Componentes.TextBoxDefault();
            this.labelDefault1 = new RognusFramework.Componentes.LabelDefault();
            this.fbdCaminhoSalvarBackup = new System.Windows.Forms.FolderBrowserDialog();
            this.ofdCaminhoPgDump = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxDefault1 = new RognusFramework.Componentes.GroupBoxDefault();
            this.dtgAgendamentos = new RognusFramework.Componentes.DataGridViewChange();
            this.userControlInAltDel1 = new RognusFramework.Componentes.UserControlInAltDel();
            this.groupBoxDefault2 = new RognusFramework.Componentes.GroupBoxDefault();
            this.dtgLocaisDeDestino = new RognusFramework.Componentes.DataGridViewChange();
            this.groupBoxDefault3 = new RognusFramework.Componentes.GroupBoxDefault();
            this.userControlInAltDel2 = new RognusFramework.Componentes.UserControlInAltDel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).BeginInit();
            this.gbxPropriedadesConexao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsParametros)).BeginInit();
            this.groupBoxDefault1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgAgendamentos)).BeginInit();
            this.groupBoxDefault2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgLocaisDeDestino)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDefault
            // 
            this.pnlDefault.Location = new System.Drawing.Point(0, 440);
            this.pnlDefault.Size = new System.Drawing.Size(751, 30);
            // 
            // gbxPropriedadesConexao
            // 
            this.gbxPropriedadesConexao.AutoState_User = true;
            this.gbxPropriedadesConexao.Controls.Add(this.btnAbrirCaminhoPgDump);
            this.gbxPropriedadesConexao.Controls.Add(this.btnAbrirDiretorioSalvarBackupEm);
            this.gbxPropriedadesConexao.Controls.Add(this.labelDefault7);
            this.gbxPropriedadesConexao.Controls.Add(this.labelDefault6);
            this.gbxPropriedadesConexao.Controls.Add(this.txtCaminhoPgDump);
            this.gbxPropriedadesConexao.Controls.Add(this.txtCaminhoSalvarBackup);
            this.gbxPropriedadesConexao.Controls.Add(this.txtBancoDados);
            this.gbxPropriedadesConexao.Controls.Add(this.labelDefault5);
            this.gbxPropriedadesConexao.Controls.Add(this.txtSenhaBancoDados);
            this.gbxPropriedadesConexao.Controls.Add(this.labelDefault4);
            this.gbxPropriedadesConexao.Controls.Add(this.txtUsuarioBancoDados);
            this.gbxPropriedadesConexao.Controls.Add(this.labelDefault3);
            this.gbxPropriedadesConexao.Controls.Add(this.txtPorta);
            this.gbxPropriedadesConexao.Controls.Add(this.labelDefault2);
            this.gbxPropriedadesConexao.Controls.Add(this.txtServidor);
            this.gbxPropriedadesConexao.Controls.Add(this.labelDefault1);
            this.gbxPropriedadesConexao.EnabledAnteriorBusca = false;
            this.gbxPropriedadesConexao.Location = new System.Drawing.Point(12, 12);
            this.gbxPropriedadesConexao.Name = "gbxPropriedadesConexao";
            this.gbxPropriedadesConexao.Size = new System.Drawing.Size(401, 208);
            this.gbxPropriedadesConexao.TabIndex = 0;
            this.gbxPropriedadesConexao.TabStop = false;
            this.gbxPropriedadesConexao.Text = "Propriedades Conexão";
            // 
            // btnAbrirCaminhoPgDump
            // 
            this.btnAbrirCaminhoPgDump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbrirCaminhoPgDump.AutoState_User = true;
            this.btnAbrirCaminhoPgDump.EnabledAnteriorBusca = false;
            this.btnAbrirCaminhoPgDump.Location = new System.Drawing.Point(352, 175);
            this.btnAbrirCaminhoPgDump.Name = "btnAbrirCaminhoPgDump";
            this.btnAbrirCaminhoPgDump.Size = new System.Drawing.Size(43, 23);
            this.btnAbrirCaminhoPgDump.TabIndex = 8;
            this.btnAbrirCaminhoPgDump.Text = "...";
            this.btnAbrirCaminhoPgDump.UseVisualStyleBackColor = true;
            this.btnAbrirCaminhoPgDump.Click += new System.EventHandler(this.btnAbrirCaminhoPgDump_Click);
            // 
            // btnAbrirDiretorioSalvarBackupEm
            // 
            this.btnAbrirDiretorioSalvarBackupEm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbrirDiretorioSalvarBackupEm.AutoState_User = true;
            this.btnAbrirDiretorioSalvarBackupEm.EnabledAnteriorBusca = false;
            this.btnAbrirDiretorioSalvarBackupEm.Location = new System.Drawing.Point(352, 149);
            this.btnAbrirDiretorioSalvarBackupEm.Name = "btnAbrirDiretorioSalvarBackupEm";
            this.btnAbrirDiretorioSalvarBackupEm.Size = new System.Drawing.Size(43, 23);
            this.btnAbrirDiretorioSalvarBackupEm.TabIndex = 6;
            this.btnAbrirDiretorioSalvarBackupEm.Text = "...";
            this.btnAbrirDiretorioSalvarBackupEm.UseVisualStyleBackColor = true;
            this.btnAbrirDiretorioSalvarBackupEm.Click += new System.EventHandler(this.btnAbrirDiretorioSalvarBackupEm_Click);
            // 
            // labelDefault7
            // 
            this.labelDefault7.AutoSize = true;
            this.labelDefault7.AutoState_User = true;
            this.labelDefault7.BackColor = System.Drawing.Color.Transparent;
            this.labelDefault7.EnabledAnteriorBusca = false;
            this.labelDefault7.Location = new System.Drawing.Point(9, 180);
            this.labelDefault7.Name = "labelDefault7";
            this.labelDefault7.Size = new System.Drawing.Size(95, 13);
            this.labelDefault7.TabIndex = 0;
            this.labelDefault7.Text = "Caminho PgDump:";
            // 
            // labelDefault6
            // 
            this.labelDefault6.AutoSize = true;
            this.labelDefault6.AutoState_User = true;
            this.labelDefault6.BackColor = System.Drawing.Color.Transparent;
            this.labelDefault6.EnabledAnteriorBusca = false;
            this.labelDefault6.Location = new System.Drawing.Point(6, 154);
            this.labelDefault6.Name = "labelDefault6";
            this.labelDefault6.Size = new System.Drawing.Size(98, 13);
            this.labelDefault6.TabIndex = 0;
            this.labelDefault6.Text = "Salvar Backup Em:";
            // 
            // txtCaminhoPgDump
            // 
            this.txtCaminhoPgDump.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCaminhoPgDump.AutoState_User = true;
            this.txtCaminhoPgDump.BackColor = System.Drawing.SystemColors.Control;
            this.txtCaminhoPgDump.CampoObrigatorio = true;
            this.txtCaminhoPgDump.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bdsParametros, "CaminhoPgDump", true));
            this.txtCaminhoPgDump.EnabledAnteriorBusca = false;
            this.txtCaminhoPgDump.Location = new System.Drawing.Point(110, 177);
            this.txtCaminhoPgDump.Name = "txtCaminhoPgDump";
            this.txtCaminhoPgDump.ReadOnly = true;
            this.txtCaminhoPgDump.Size = new System.Drawing.Size(236, 20);
            this.txtCaminhoPgDump.TabIndex = 7;
            this.txtCaminhoPgDump.TipoValor_User = RognusFramework.Componentes.TextBoxDefault.TiposValor.Indefinido;
            this.txtCaminhoPgDump.ValorPadrao_User = "0";
            // 
            // bdsParametros
            // 
            this.bdsParametros.DataSource = typeof(BackupAutomaticoCervantes.ParametrosBackupModel);
            this.bdsParametros.SupportsSorting_User = true;
            // 
            // txtCaminhoSalvarBackup
            // 
            this.txtCaminhoSalvarBackup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCaminhoSalvarBackup.AutoState_User = true;
            this.txtCaminhoSalvarBackup.BackColor = System.Drawing.SystemColors.Control;
            this.txtCaminhoSalvarBackup.CampoObrigatorio = true;
            this.txtCaminhoSalvarBackup.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bdsParametros, "CaminhoPastaSalvarBackup", true));
            this.txtCaminhoSalvarBackup.EnabledAnteriorBusca = false;
            this.txtCaminhoSalvarBackup.Location = new System.Drawing.Point(110, 151);
            this.txtCaminhoSalvarBackup.Name = "txtCaminhoSalvarBackup";
            this.txtCaminhoSalvarBackup.ReadOnly = true;
            this.txtCaminhoSalvarBackup.Size = new System.Drawing.Size(236, 20);
            this.txtCaminhoSalvarBackup.TabIndex = 5;
            this.txtCaminhoSalvarBackup.TipoValor_User = RognusFramework.Componentes.TextBoxDefault.TiposValor.Indefinido;
            this.txtCaminhoSalvarBackup.ValorPadrao_User = "0";
            // 
            // txtBancoDados
            // 
            this.txtBancoDados.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBancoDados.AutoState_User = true;
            this.txtBancoDados.BackColor = System.Drawing.SystemColors.Control;
            this.txtBancoDados.CampoObrigatorio = true;
            this.txtBancoDados.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bdsParametros, "NomebancoBancoDados", true));
            this.txtBancoDados.EnabledAnteriorBusca = false;
            this.txtBancoDados.Location = new System.Drawing.Point(110, 125);
            this.txtBancoDados.Name = "txtBancoDados";
            this.txtBancoDados.ReadOnly = true;
            this.txtBancoDados.Size = new System.Drawing.Size(285, 20);
            this.txtBancoDados.TabIndex = 4;
            this.txtBancoDados.TipoValor_User = RognusFramework.Componentes.TextBoxDefault.TiposValor.Indefinido;
            this.txtBancoDados.ValorPadrao_User = "0";
            // 
            // labelDefault5
            // 
            this.labelDefault5.AutoSize = true;
            this.labelDefault5.AutoState_User = true;
            this.labelDefault5.BackColor = System.Drawing.Color.Transparent;
            this.labelDefault5.EnabledAnteriorBusca = false;
            this.labelDefault5.Location = new System.Drawing.Point(29, 128);
            this.labelDefault5.Name = "labelDefault5";
            this.labelDefault5.Size = new System.Drawing.Size(75, 13);
            this.labelDefault5.TabIndex = 0;
            this.labelDefault5.Text = "Banco Dados:";
            // 
            // txtSenhaBancoDados
            // 
            this.txtSenhaBancoDados.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSenhaBancoDados.AutoState_User = true;
            this.txtSenhaBancoDados.BackColor = System.Drawing.SystemColors.Control;
            this.txtSenhaBancoDados.CampoObrigatorio = true;
            this.txtSenhaBancoDados.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bdsParametros, "SenhaUsuario", true));
            this.txtSenhaBancoDados.EnabledAnteriorBusca = false;
            this.txtSenhaBancoDados.Location = new System.Drawing.Point(110, 99);
            this.txtSenhaBancoDados.Name = "txtSenhaBancoDados";
            this.txtSenhaBancoDados.ReadOnly = true;
            this.txtSenhaBancoDados.Size = new System.Drawing.Size(285, 20);
            this.txtSenhaBancoDados.TabIndex = 3;
            this.txtSenhaBancoDados.TipoValor_User = RognusFramework.Componentes.TextBoxDefault.TiposValor.Indefinido;
            this.txtSenhaBancoDados.ValorPadrao_User = "0";
            // 
            // labelDefault4
            // 
            this.labelDefault4.AutoSize = true;
            this.labelDefault4.AutoState_User = true;
            this.labelDefault4.BackColor = System.Drawing.Color.Transparent;
            this.labelDefault4.EnabledAnteriorBusca = false;
            this.labelDefault4.Location = new System.Drawing.Point(63, 102);
            this.labelDefault4.Name = "labelDefault4";
            this.labelDefault4.Size = new System.Drawing.Size(41, 13);
            this.labelDefault4.TabIndex = 0;
            this.labelDefault4.Text = "Senha:";
            // 
            // txtUsuarioBancoDados
            // 
            this.txtUsuarioBancoDados.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsuarioBancoDados.AutoState_User = true;
            this.txtUsuarioBancoDados.BackColor = System.Drawing.SystemColors.Control;
            this.txtUsuarioBancoDados.CampoObrigatorio = true;
            this.txtUsuarioBancoDados.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bdsParametros, "UsuarioBancoDados", true));
            this.txtUsuarioBancoDados.EnabledAnteriorBusca = false;
            this.txtUsuarioBancoDados.Location = new System.Drawing.Point(110, 73);
            this.txtUsuarioBancoDados.Name = "txtUsuarioBancoDados";
            this.txtUsuarioBancoDados.ReadOnly = true;
            this.txtUsuarioBancoDados.Size = new System.Drawing.Size(285, 20);
            this.txtUsuarioBancoDados.TabIndex = 2;
            this.txtUsuarioBancoDados.TipoValor_User = RognusFramework.Componentes.TextBoxDefault.TiposValor.Indefinido;
            this.txtUsuarioBancoDados.ValorPadrao_User = "0";
            // 
            // labelDefault3
            // 
            this.labelDefault3.AutoSize = true;
            this.labelDefault3.AutoState_User = true;
            this.labelDefault3.BackColor = System.Drawing.Color.Transparent;
            this.labelDefault3.EnabledAnteriorBusca = false;
            this.labelDefault3.Location = new System.Drawing.Point(58, 76);
            this.labelDefault3.Name = "labelDefault3";
            this.labelDefault3.Size = new System.Drawing.Size(46, 13);
            this.labelDefault3.TabIndex = 0;
            this.labelDefault3.Text = "Usuário:";
            // 
            // txtPorta
            // 
            this.txtPorta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPorta.AutoState_User = true;
            this.txtPorta.BackColor = System.Drawing.SystemColors.Control;
            this.txtPorta.CampoObrigatorio = true;
            this.txtPorta.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bdsParametros, "Porta", true));
            this.txtPorta.EnabledAnteriorBusca = false;
            this.txtPorta.Location = new System.Drawing.Point(110, 47);
            this.txtPorta.Name = "txtPorta";
            this.txtPorta.ReadOnly = true;
            this.txtPorta.Size = new System.Drawing.Size(285, 20);
            this.txtPorta.TabIndex = 1;
            this.txtPorta.TipoValor_User = RognusFramework.Componentes.TextBoxDefault.TiposValor.Indefinido;
            this.txtPorta.ValorPadrao_User = "0";
            // 
            // labelDefault2
            // 
            this.labelDefault2.AutoSize = true;
            this.labelDefault2.AutoState_User = true;
            this.labelDefault2.BackColor = System.Drawing.Color.Transparent;
            this.labelDefault2.EnabledAnteriorBusca = false;
            this.labelDefault2.Location = new System.Drawing.Point(69, 50);
            this.labelDefault2.Name = "labelDefault2";
            this.labelDefault2.Size = new System.Drawing.Size(35, 13);
            this.labelDefault2.TabIndex = 0;
            this.labelDefault2.Text = "Porta:";
            // 
            // txtServidor
            // 
            this.txtServidor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServidor.AutoState_User = true;
            this.txtServidor.BackColor = System.Drawing.SystemColors.Control;
            this.txtServidor.CampoObrigatorio = true;
            this.txtServidor.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bdsParametros, "Servidor", true));
            this.txtServidor.EnabledAnteriorBusca = false;
            this.txtServidor.Location = new System.Drawing.Point(110, 21);
            this.txtServidor.Name = "txtServidor";
            this.txtServidor.ReadOnly = true;
            this.txtServidor.Size = new System.Drawing.Size(285, 20);
            this.txtServidor.TabIndex = 0;
            this.txtServidor.TipoValor_User = RognusFramework.Componentes.TextBoxDefault.TiposValor.Indefinido;
            this.txtServidor.ValorPadrao_User = "0";
            // 
            // labelDefault1
            // 
            this.labelDefault1.AutoSize = true;
            this.labelDefault1.AutoState_User = true;
            this.labelDefault1.BackColor = System.Drawing.Color.Transparent;
            this.labelDefault1.EnabledAnteriorBusca = false;
            this.labelDefault1.Location = new System.Drawing.Point(34, 24);
            this.labelDefault1.Name = "labelDefault1";
            this.labelDefault1.Size = new System.Drawing.Size(70, 13);
            this.labelDefault1.TabIndex = 0;
            this.labelDefault1.Text = "Servidor / IP:";
            // 
            // fbdCaminhoSalvarBackup
            // 
            this.fbdCaminhoSalvarBackup.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // ofdCaminhoPgDump
            // 
            this.ofdCaminhoPgDump.FileName = "openFileDialog1";
            // 
            // groupBoxDefault1
            // 
            this.groupBoxDefault1.AutoState_User = true;
            this.groupBoxDefault1.Controls.Add(this.dtgAgendamentos);
            this.groupBoxDefault1.EnabledAnteriorBusca = false;
            this.groupBoxDefault1.Location = new System.Drawing.Point(12, 226);
            this.groupBoxDefault1.Name = "groupBoxDefault1";
            this.groupBoxDefault1.Size = new System.Drawing.Size(401, 178);
            this.groupBoxDefault1.TabIndex = 18;
            this.groupBoxDefault1.TabStop = false;
            this.groupBoxDefault1.Text = "Agendamentos";
            // 
            // dtgAgendamentos
            // 
            this.dtgAgendamentos.AllowUserToAddRows = false;
            this.dtgAgendamentos.AllowUserToAlterStructOnReadOnly_User = false;
            this.dtgAgendamentos.AllowUserToResizeRows = false;
            this.dtgAgendamentos.AutoState_User = true;
            this.dtgAgendamentos.BackColorEdit_User = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(236)))), ((int)(((byte)(221)))));
            this.dtgAgendamentos.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dtgAgendamentos.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dtgAgendamentos.CmsConfigVisibleColumns = null;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgAgendamentos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgAgendamentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgAgendamentos.CurrentBackColorEdit_User = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(213)))), ((int)(((byte)(179)))));
            this.dtgAgendamentos.CurrentBackColorNotEdit_User = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(225)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgAgendamentos.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtgAgendamentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgAgendamentos.EnabledAnteriorBusca = false;
            this.dtgAgendamentos.Location = new System.Drawing.Point(3, 16);
            this.dtgAgendamentos.Name = "dtgAgendamentos";
            this.dtgAgendamentos.ReadOnly = true;
            this.dtgAgendamentos.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dtgAgendamentos.RowHeadersWidth = 25;
            this.dtgAgendamentos.SelectionBackColorEdit_User = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(204)))));
            this.dtgAgendamentos.SelectionBackColorNotEdit_User = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            this.dtgAgendamentos.SelectionForeColor_User = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dtgAgendamentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtgAgendamentos.Size = new System.Drawing.Size(395, 159);
            this.dtgAgendamentos.StandardTab = true;
            this.dtgAgendamentos.TabIndex = 0;
            this.dtgAgendamentos.AddNewItem_User += new System.EventHandler(this.dtgAgendamentos_AddNewItem_User);
            this.dtgAgendamentos.UpdateItem_User += new System.EventHandler(this.dtgAgendamentos_UpdateItem_User);
            this.dtgAgendamentos.RowRemoving_User += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dtgAgendamentos_RowRemoving_User);
            // 
            // userControlInAltDel1
            // 
            this.userControlInAltDel1.AutoState_User = true;
            this.userControlInAltDel1.BackColor = System.Drawing.Color.Transparent;
            this.userControlInAltDel1.EnabledAnteriorBusca = false;
            this.userControlInAltDel1.Location = new System.Drawing.Point(327, 407);
            this.userControlInAltDel1.Name = "userControlInAltDel1";
            this.userControlInAltDel1.Owner_User = this.dtgAgendamentos;
            this.userControlInAltDel1.Size = new System.Drawing.Size(86, 24);
            this.userControlInAltDel1.TabIndex = 19;
            // 
            // groupBoxDefault2
            // 
            this.groupBoxDefault2.AutoState_User = true;
            this.groupBoxDefault2.Controls.Add(this.dtgLocaisDeDestino);
            this.groupBoxDefault2.EnabledAnteriorBusca = false;
            this.groupBoxDefault2.Location = new System.Drawing.Point(419, 226);
            this.groupBoxDefault2.Name = "groupBoxDefault2";
            this.groupBoxDefault2.Size = new System.Drawing.Size(329, 178);
            this.groupBoxDefault2.TabIndex = 20;
            this.groupBoxDefault2.TabStop = false;
            this.groupBoxDefault2.Text = "Locais de Destino ";
            // 
            // dtgLocaisDeDestino
            // 
            this.dtgLocaisDeDestino.AllowUserToAddRows = false;
            this.dtgLocaisDeDestino.AllowUserToAlterStructOnReadOnly_User = false;
            this.dtgLocaisDeDestino.AllowUserToResizeRows = false;
            this.dtgLocaisDeDestino.AutoState_User = true;
            this.dtgLocaisDeDestino.BackColorEdit_User = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(236)))), ((int)(((byte)(221)))));
            this.dtgLocaisDeDestino.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dtgLocaisDeDestino.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dtgLocaisDeDestino.CmsConfigVisibleColumns = null;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgLocaisDeDestino.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dtgLocaisDeDestino.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgLocaisDeDestino.CurrentBackColorEdit_User = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(213)))), ((int)(((byte)(179)))));
            this.dtgLocaisDeDestino.CurrentBackColorNotEdit_User = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(225)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgLocaisDeDestino.DefaultCellStyle = dataGridViewCellStyle4;
            this.dtgLocaisDeDestino.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgLocaisDeDestino.EnabledAnteriorBusca = false;
            this.dtgLocaisDeDestino.Location = new System.Drawing.Point(3, 16);
            this.dtgLocaisDeDestino.Name = "dtgLocaisDeDestino";
            this.dtgLocaisDeDestino.ReadOnly = true;
            this.dtgLocaisDeDestino.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dtgLocaisDeDestino.RowHeadersWidth = 25;
            this.dtgLocaisDeDestino.SelectionBackColorEdit_User = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(204)))));
            this.dtgLocaisDeDestino.SelectionBackColorNotEdit_User = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            this.dtgLocaisDeDestino.SelectionForeColor_User = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dtgLocaisDeDestino.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtgLocaisDeDestino.Size = new System.Drawing.Size(323, 159);
            this.dtgLocaisDeDestino.StandardTab = true;
            this.dtgLocaisDeDestino.TabIndex = 0;
            this.dtgLocaisDeDestino.AddNewItem_User += new System.EventHandler(this.dtgLocaisDeDestino_AddNewItem_User);
            this.dtgLocaisDeDestino.UpdateItem_User += new System.EventHandler(this.dtgLocaisDeDestino_UpdateItem_User);
            this.dtgLocaisDeDestino.RowRemoving_User += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dtgLocaisDeDestino_RowRemoving_User);
            // 
            // groupBoxDefault3
            // 
            this.groupBoxDefault3.AutoState_User = true;
            this.groupBoxDefault3.EnabledAnteriorBusca = false;
            this.groupBoxDefault3.Location = new System.Drawing.Point(422, 12);
            this.groupBoxDefault3.Name = "groupBoxDefault3";
            this.groupBoxDefault3.Size = new System.Drawing.Size(323, 208);
            this.groupBoxDefault3.TabIndex = 22;
            this.groupBoxDefault3.TabStop = false;
            this.groupBoxDefault3.Text = "Tabelas Excluidas do Backup";
            // 
            // userControlInAltDel2
            // 
            this.userControlInAltDel2.AutoState_User = true;
            this.userControlInAltDel2.BackColor = System.Drawing.Color.Transparent;
            this.userControlInAltDel2.EnabledAnteriorBusca = false;
            this.userControlInAltDel2.Location = new System.Drawing.Point(662, 407);
            this.userControlInAltDel2.Name = "userControlInAltDel2";
            this.userControlInAltDel2.Owner_User = this.dtgLocaisDeDestino;
            this.userControlInAltDel2.Size = new System.Drawing.Size(86, 24);
            this.userControlInAltDel2.TabIndex = 23;
            // 
            // FrmConfigBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 470);
            this.Controls.Add(this.userControlInAltDel2);
            this.Controls.Add(this.groupBoxDefault3);
            this.Controls.Add(this.groupBoxDefault2);
            this.Controls.Add(this.userControlInAltDel1);
            this.Controls.Add(this.groupBoxDefault1);
            this.Controls.Add(this.gbxPropriedadesConexao);
            this.MinimumSize = new System.Drawing.Size(439, 295);
            this.Name = "FrmConfigBackup";
            this.ShowInTaskbar = true;
            this.Text = "Parametros de Backup";
            this.Controls.SetChildIndex(this.pnlDefault, 0);
            this.Controls.SetChildIndex(this.gbxPropriedadesConexao, 0);
            this.Controls.SetChildIndex(this.groupBoxDefault1, 0);
            this.Controls.SetChildIndex(this.userControlInAltDel1, 0);
            this.Controls.SetChildIndex(this.groupBoxDefault2, 0);
            this.Controls.SetChildIndex(this.groupBoxDefault3, 0);
            this.Controls.SetChildIndex(this.userControlInAltDel2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).EndInit();
            this.gbxPropriedadesConexao.ResumeLayout(false);
            this.gbxPropriedadesConexao.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsParametros)).EndInit();
            this.groupBoxDefault1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgAgendamentos)).EndInit();
            this.groupBoxDefault2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgLocaisDeDestino)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RognusFramework.Componentes.GroupBoxDefault gbxPropriedadesConexao;
        private RognusFramework.Componentes.TextBoxDefault txtPorta;
        private RognusFramework.Componentes.LabelDefault labelDefault2;
        private RognusFramework.Componentes.TextBoxDefault txtServidor;
        private RognusFramework.Componentes.LabelDefault labelDefault1;
        private RognusFramework.Componentes.TextBoxDefault txtSenhaBancoDados;
        private RognusFramework.Componentes.LabelDefault labelDefault4;
        private RognusFramework.Componentes.TextBoxDefault txtUsuarioBancoDados;
        private RognusFramework.Componentes.LabelDefault labelDefault3;
        private RognusFramework.Componentes.TextBoxDefault txtBancoDados;
        private RognusFramework.Componentes.LabelDefault labelDefault5;
        private RognusFramework.Componentes.LabelDefault labelDefault6;
        private RognusFramework.Componentes.TextBoxDefault txtCaminhoSalvarBackup;
        private RognusFramework.Componentes.LabelDefault labelDefault7;
        private RognusFramework.Componentes.TextBoxDefault txtCaminhoPgDump;
        private RognusFramework.Componentes.ButtonDefault btnAbrirCaminhoPgDump;
        private RognusFramework.Componentes.ButtonDefault btnAbrirDiretorioSalvarBackupEm;
        private System.Windows.Forms.FolderBrowserDialog fbdCaminhoSalvarBackup;
        private System.Windows.Forms.OpenFileDialog ofdCaminhoPgDump;
        private RognusFramework.Componentes.BindingSourceDefault bdsParametros;
        private RognusFramework.Componentes.GroupBoxDefault groupBoxDefault1;
        private RognusFramework.Componentes.DataGridViewChange dtgAgendamentos;
        private RognusFramework.Componentes.UserControlInAltDel userControlInAltDel1;
        private RognusFramework.Componentes.GroupBoxDefault groupBoxDefault2;
        private RognusFramework.Componentes.DataGridViewChange dtgLocaisDeDestino;
        private RognusFramework.Componentes.GroupBoxDefault groupBoxDefault3;
        private RognusFramework.Componentes.UserControlInAltDel userControlInAltDel2;
    }
}