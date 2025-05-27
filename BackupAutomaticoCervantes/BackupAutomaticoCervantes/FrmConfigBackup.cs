using BackupAutomaticoCervantes.DestinoBackup;
using BackupAutomaticoCervantes.DestinoBackup.GoogleDrive;
using BackupAutomaticoCervantes.Models;
using BackupAutomaticoCervantes.repositorios;
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
    public partial class FrmConfigBackup : RognusFramework.FrmModeloFull
    {
        private ParametrosBackupModel _parametrosOld;
        private readonly AppConfigRepositorio _repo;

        public FrmConfigBackup()
        {
            InitializeComponent();

            _repo = new AppConfigRepositorio();

            bdsParametros.DataSource = new ParametrosBackupModel();
        }

        public FrmConfigBackup(ParametrosBackupModel pValue) 
        {
            InitializeComponent();
            bdsParametros.DataSource = pValue;
        }

        private void btnAbrirDiretorioSalvarBackupEm_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ((ParametrosBackupModel)bdsParametros.Current).CaminhoPastaSalvarBackup = dialog.SelectedPath;
            }

            Cursor.Current = Cursors.Default;

            bdsParametros.ResetBindings(false);
        }

        private void btnAbrirCaminhoPgDump_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Executável (*.exe)|*.exe";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ((ParametrosBackupModel)bdsParametros.Current).CaminhoPgDump = openFileDialog.FileName;
            }

            bdsParametros.ResetBindings(false);
            Cursor.Current = Cursors.Default;
        }

        private void frmBuscaListaBackpus_btnInAltClick_user(object sender, EventArgs e)
        {
            bdsParametros.DataSource = (ParametrosBackupModel)sender;
            EstadoBuscar();
        }

        public override void btnNovo_Click(object sender, EventArgs e)
        {
            bdsParametros.DataSource = new ParametrosBackupModel();
            base.btnNovo_Click(sender, e);
        }
        
        public override void btnAlterar_Click(object sender, EventArgs e)
        {
            _parametrosOld = ((ParametrosBackupModel)bdsParametros.Current).Clone();
            base.btnAlterar_Click(sender, e);
        }

        public override void btnExcluir_Click_Yes()
        {
            base.btnExcluir_Click_Yes();
            
            _repo.Delete(((ParametrosBackupModel)bdsParametros.Current).Id);

        }

        public override void btnSalvar_Click(object sender, EventArgs e)
        {
            switch (EstadoAtual)
            {
                case Estados.Inativo:
                    break;
                case Estados.Novo:
                    _repo.Add((ParametrosBackupModel)bdsParametros.Current);
                    break;
                case Estados.Buscar:
                    break;
                case Estados.Buscando:
                    break;
                case Estados.Alterar:
                    _repo.Update((ParametrosBackupModel)bdsParametros.Current);
                    break;
                case Estados.Salvo:
                    break;
                default:
                    break;
            }

            _parametrosOld = ((ParametrosBackupModel)bdsParametros.Current).Clone();

            base.btnSalvar_Click(_parametrosOld, e);
        }

        private void dtgAgendamentos_AddNewItem_User(object sender, EventArgs e)
        {
         
            FrmAgendamento frmAgendamento = new FrmAgendamento();
            frmAgendamento.btnInAltClick_user += FrmAgendamento_btnInAltClick_user;
            frmAgendamento.ShowDialog(this);

        }

        private void FrmAgendamento_btnInAltClick_user(object sender, EventArgs e)
        {
            ((ParametrosBackupModel)bdsParametros.Current).Agendamentos.Add((HorarioAgendamentoModel)sender);
        }

        private void dtgAgendamentos_RowRemoving_User(object sender, DataGridViewRowCancelEventArgs e)
        {

        }

        private void dtgAgendamentos_UpdateItem_User(object sender, EventArgs e)
        {

        }

        private async void dtgLocaisDeDestino_AddNewItem_User(object sender, EventArgs e)
        {
            var destino = new DestinoConfig();

            destino.Tipo = DestinoTipo.GoogleDrive;

            await GoogleDriveAuthManager.AutenticarAsync(destino.Id);

            ((ParametrosBackupModel)bdsParametros.Current).Destinos.Add(destino);
        }

        private void dtgLocaisDeDestino_RowRemoving_User(object sender, DataGridViewRowCancelEventArgs e)
        {

        }

        private void dtgLocaisDeDestino_UpdateItem_User(object sender, EventArgs e)
        {

        }
    }
}
