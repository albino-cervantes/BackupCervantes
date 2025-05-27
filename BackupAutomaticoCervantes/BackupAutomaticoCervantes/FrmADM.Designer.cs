namespace BackupAutomaticoCervantes
{
    partial class FrmADM
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
            this.bdsParametrosBackups = new RognusFramework.Componentes.BindingSourceDefault(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsParametrosBackups)).BeginInit();
            this.SuspendLayout();
            // 
            // bdsParametrosBackups
            // 
            this.bdsParametrosBackups.DataSource = typeof(BackupAutomaticoCervantes.ParametrosBackupModel);
            this.bdsParametrosBackups.SupportsSorting_User = true;
            // 
            // FrmADM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 667);
            this.Name = "FrmADM";
            this.ShowIcon = false;
            this.Text = " Backups";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderDefault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsParametrosBackups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private RognusFramework.Componentes.BindingSourceDefault bdsParametrosBackups;
    }
}