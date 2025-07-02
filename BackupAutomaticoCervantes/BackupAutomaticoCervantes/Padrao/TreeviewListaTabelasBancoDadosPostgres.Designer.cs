namespace BackupAutomaticoCervantes.Padrao
{
    partial class TreeviewListaTabelasBancoDadosPostgres
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

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeViewDefault1 = new RognusFramework.Componentes.TreeViewDefault();
            this.SuspendLayout();
            // 
            // treeViewDefault1
            // 
            this.treeViewDefault1.AutoState_User = true;
            this.treeViewDefault1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDefault1.EnabledAnteriorBusca = false;
            this.treeViewDefault1.Location = new System.Drawing.Point(0, 0);
            this.treeViewDefault1.Name = "treeViewDefault1";
            this.treeViewDefault1.Size = new System.Drawing.Size(150, 150);
            this.treeViewDefault1.TabIndex = 0;
            // 
            // TreeviewListaTabelasBancoDadosPostgres
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewDefault1);
            this.Name = "TreeviewListaTabelasBancoDadosPostgres";
            this.ResumeLayout(false);

        }

        #endregion

        private RognusFramework.Componentes.TreeViewDefault treeViewDefault1;
    }
}
