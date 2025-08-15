namespace BackupAutomaticoCervantes
{
    partial class FrmADM
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.ribbon1 = new System.Windows.Forms.Ribbon();
            this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
            this.rbbConfigParametros = new System.Windows.Forms.RibbonButton();
            this.ribbonTab2 = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
            this.rbbInstalarServico = new System.Windows.Forms.RibbonButton();
            this.rbbConfigServico = new System.Windows.Forms.RibbonButton();
            this.SuspendLayout();
            // 
            // ribbon1
            // 
            this.ribbon1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ribbon1.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.Minimized = false;
            this.ribbon1.Name = "ribbon1";
            // 
            // 
            // 
            this.ribbon1.OrbDropDown.BorderRoundness = 8;
            this.ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.OrbDropDown.Name = "";
            this.ribbon1.OrbDropDown.Size = new System.Drawing.Size(527, 447);
            this.ribbon1.OrbDropDown.TabIndex = 0;
            this.ribbon1.OrbVisible = false;
            this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
            this.ribbon1.Size = new System.Drawing.Size(925, 151);
            this.ribbon1.TabIndex = 1;
            this.ribbon1.Tabs.Add(this.ribbonTab1);
            this.ribbon1.Tabs.Add(this.ribbonTab2);
            this.ribbon1.Text = "ribbon1";
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Panels.Add(this.ribbonPanel1);
            this.ribbonTab1.Text = "Gerenciar Parametros";
            // 
            // ribbonPanel1
            // 
            this.ribbonPanel1.Image = global::BackupAutomaticoCervantes.Properties.Resources.bkp_32;
            this.ribbonPanel1.Items.Add(this.rbbConfigParametros);
            this.ribbonPanel1.Name = "ribbonPanel1";
            this.ribbonPanel1.Text = "Parâmetros";
            // 
            // rbbConfigParametros
            // 
            this.rbbConfigParametros.Image = global::BackupAutomaticoCervantes.Properties.Resources.bkp_32;
            this.rbbConfigParametros.LargeImage = global::BackupAutomaticoCervantes.Properties.Resources.bkp_32;
            this.rbbConfigParametros.Name = "rbbConfigParametros";
            this.rbbConfigParametros.SmallImage = global::BackupAutomaticoCervantes.Properties.Resources.bkp_16;
            this.rbbConfigParametros.Text = "Config Parâmetros";
            this.rbbConfigParametros.Click += new System.EventHandler(this.rbbConfigParametros_Click);
            // 
            // ribbonTab2
            // 
            this.ribbonTab2.Name = "ribbonTab2";
            this.ribbonTab2.Panels.Add(this.ribbonPanel2);
            this.ribbonTab2.Text = "Gerenciar Serviço";
            // 
            // ribbonPanel2
            // 
            this.ribbonPanel2.Items.Add(this.rbbInstalarServico);
            this.ribbonPanel2.Items.Add(this.rbbConfigServico);
            this.ribbonPanel2.Name = "ribbonPanel2";
            this.ribbonPanel2.Text = "Serviço";
            // 
            // rbbInstalarServico
            // 
            this.rbbInstalarServico.Image = global::BackupAutomaticoCervantes.Properties.Resources.tools;
            this.rbbInstalarServico.LargeImage = global::BackupAutomaticoCervantes.Properties.Resources.tools;
            this.rbbInstalarServico.Name = "rbbInstalarServico";
            this.rbbInstalarServico.SmallImage = global::BackupAutomaticoCervantes.Properties.Resources.tools_16;
            this.rbbInstalarServico.Text = "Instalar Serviço";
            this.rbbInstalarServico.Click += new System.EventHandler(this.ribbonButton2_Click);
            // 
            // rbbConfigServico
            // 
            this.rbbConfigServico.Image = global::BackupAutomaticoCervantes.Properties.Resources.process_32;
            this.rbbConfigServico.LargeImage = global::BackupAutomaticoCervantes.Properties.Resources.process_32;
            this.rbbConfigServico.Name = "rbbConfigServico";
            this.rbbConfigServico.SmallImage = global::BackupAutomaticoCervantes.Properties.Resources.process_16;
            this.rbbConfigServico.Text = "Config Servico";
            // 
            // FrmADM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 453);
            this.Controls.Add(this.ribbon1);
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "FrmADM";
            this.Text = "FrmADM";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Ribbon ribbon1;
        private System.Windows.Forms.RibbonTab ribbonTab1;
        private System.Windows.Forms.RibbonPanel ribbonPanel1;
        private System.Windows.Forms.RibbonButton rbbConfigParametros;
        private System.Windows.Forms.RibbonTab ribbonTab2;
        private System.Windows.Forms.RibbonPanel ribbonPanel2;
        private System.Windows.Forms.RibbonButton rbbConfigServico;
        private System.Windows.Forms.RibbonButton rbbInstalarServico;
    }
}



