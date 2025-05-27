using BackupAutomaticoCervantes.repositorios;
using RognusFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackupAutomaticoCervantes
{
    public partial class FrmADM : FrmDefault
    {
        private readonly AppConfigRepositorio _repo;

        public FrmADM()
        {
            InitializeComponent();

            _repo = new AppConfigRepositorio();

            bdsParametrosBackups.DataSource = _repo.GetAll();
        }

        private void dtgListaParametrosBackups_AddNewItem_User(object sender, EventArgs e)
        {

        }

        private void dtgListaParametrosBackups_UpdateItem_User(object sender, EventArgs e)
        {

        }

        private void dtgListaParametrosBackups_RowRemoving_User(object sender, DataGridViewRowCancelEventArgs e)
        {

        }
    }
}
