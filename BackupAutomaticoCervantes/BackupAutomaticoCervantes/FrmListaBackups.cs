using BackupAutomaticoCervantes.repositorios;
using Microsoft.Graph.Identity.B2xUserFlows.Item.Languages.Item.OverridesPages;
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
    public partial class FrmListaBackups : FrmModeloBusca
    {
        private readonly AppConfigRepositorio _repo;
        private bool _isClosing = false;

        public FrmListaBackups()
        {
            InitializeComponent();

            _repo = new AppConfigRepositorio();


        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _isClosing = true;
            base.OnFormClosing(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {

            if (!_isClosing && Visible)
                bdsListaParametrosDeBackup.DataSource = _repo.GetAll();
        }
    }
}
