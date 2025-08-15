using BackupAutomaticoCervantes.Padrao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupAutomaticoCervantes
{
    public partial class FrmADM : RibbonForm
    {

        public FrmADM()
        {
            InitializeComponent();
        }

        private void rbbConfigParametros_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                var frm = new FrmConfigBackup();
                frm.MdiParent = this;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Cursor.Current = Cursors.Default;
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ClsStatica.CriarServico("BackupAutomaticoCervantes", "Backup Automático Cervantes",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BackupWindowsService.exe"));
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Cursor.Current = Cursors.Default;
        }
    }
}