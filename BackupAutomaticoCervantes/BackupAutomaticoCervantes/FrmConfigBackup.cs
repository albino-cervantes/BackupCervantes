using BackupAutomaticoCervantes.DestinoBackup;
using BackupAutomaticoCervantes.DestinoBackup.GoogleDrive;
using BackupAutomaticoCervantes.Models;
using BackupAutomaticoCervantes.repositorios;
using BackupAutomaticoCervantes.Padrao;
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
        private bool _isUpdatingTreeView = false;

        public FrmConfigBackup()
        {
            InitializeComponent();

            _repo = new AppConfigRepositorio();

            bdsParametros.DataSource = new ParametrosBackupModel();

            ConfigurarTreeViewTabelasEventos();
            ConfigurarBindingSourceEventos();
        }

        public FrmConfigBackup(ParametrosBackupModel pValue)
        {
            InitializeComponent();
            bdsParametros.DataSource = pValue;

            ConfigurarTreeViewTabelasEventos();
            ConfigurarBindingSourceEventos();
        }

        #region Configuração de Eventos

        private void ConfigurarTreeViewTabelasEventos()
        {
            // Configurar eventos do TreeView de tabelas
            tvListaTabelas.SelectedTablesChanged += TvListaTabelas_SelectedTablesChanged;
            tvListaTabelas.ConnectionStatusChanged += TvListaTabelas_ConnectionStatusChanged;
        }

        private void ConfigurarBindingSourceEventos()
        {
            // Monitorar mudanças no BindingSource
            //bdsParametros.CurrentChanged += BdsParametros_CurrentChanged;
            bdsParametros.DataSourceChanged += BdsParametros_DataSourceChanged;
        }

        #endregion

        #region Eventos do BindingSource

        //private async void BdsParametros_CurrentChanged(object sender, EventArgs e)
        //{
        //    await AtualizarTreeViewTabelas();
        //}

        private async void BdsParametros_DataSourceChanged(object sender, EventArgs e)
        {
            await AtualizarTreeViewTabelas();
        }

        #endregion

        #region Eventos do TreeView de Tabelas

        private void TvListaTabelas_SelectedTablesChanged(object sender, SelectedTablesChangedEventArgs e)
        {
            if (_isUpdatingTreeView)
                return;

            var parametros = (ParametrosBackupModel)bdsParametros.Current;
            if (PossuiDadosConexaoCompletos(parametros))
            {
                // Atualizar a lista de tabelas ignoradas baseada na seleção
                // Assumindo que tabelas NÃO selecionadas são as ignoradas
                var todasTabelas = tvListaTabelas.GetAllAvailableTables();
                var tabelasSelecionadas = e.SelectedTables;

                parametros.ListaTabelasIgnoradas = todasTabelas
                    .Where(t => tabelasSelecionadas.Contains(t))
                    .ToList();

                // Notificar mudança no BindingSource
                bdsParametros.ResetBindings(false);
            }
        }

        private void TvListaTabelas_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e)
        {
            // Atualizar interface com status da conexão
            AtualizarStatusConexao(e.IsConnected, e.Message);
        }

        #endregion

        #region Métodos de Atualização do TreeView

        private async Task AtualizarTreeViewTabelas()
        {
            var parametros = (ParametrosBackupModel)bdsParametros.Current;

            if (parametros == null || !PossuiDadosConexaoCompletos(parametros))
            {
                LimparTreeViewTabelas();
                return;
            }

            try
            {
                _isUpdatingTreeView = true;

                // Construir string de conexão
                string connectionString = ConstruirConnectionString(parametros);

                // Carregar tabelas do banco
                await tvListaTabelas.LoadTablesAsync(connectionString);

                // Marcar tabelas que NÃO estão na lista de ignoradas
                var tabelasParaSelecionar = await ObterTabelasParaSelecionar(parametros, connectionString);
                tvListaTabelas.SetSelectedTables(tabelasParaSelecionar);

                AtualizarStatusConexao(true, "Tabelas carregadas com sucesso");
            }
            catch (Exception ex)
            {
                AtualizarStatusConexao(false, $"Erro ao carregar tabelas: {ex.Message}");
                MessageBox.Show($"Erro ao carregar tabelas: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isUpdatingTreeView = false;
            }
        }

        private bool PossuiDadosConexaoCompletos(ParametrosBackupModel parametros)
        {
            return !string.IsNullOrWhiteSpace(parametros.Servidor) &&
                   !string.IsNullOrWhiteSpace(parametros.Porta) &&
                   !string.IsNullOrWhiteSpace(parametros.UsuarioBancoDados) &&
                   !string.IsNullOrWhiteSpace(parametros.SenhaUsuario) &&
                   !string.IsNullOrWhiteSpace(parametros.NomebancoBancoDados);
        }

        private string ConstruirConnectionString(ParametrosBackupModel parametros)
        {
            return $"Host={parametros.Servidor};Database={parametros.NomebancoBancoDados};Username={parametros.UsuarioBancoDados};Password={parametros.SenhaUsuario};Port={parametros.Porta}";
        }

        private async Task<List<string>> ObterTabelasParaSelecionar(ParametrosBackupModel parametros, string connectionString)
        {
            // Se não há lista de tabelas ignoradas, selecionar todas
            //if (parametros.ListaTabelasIgnoradas == null || !parametros.ListaTabelasIgnoradas.Any())
            //{
            //    return await ObterTodasTabelasDisponiveis(connectionString);
            //}

            // Obter todas as tabelas e remover as ignoradas
            var todasTabelas = await ObterTodasTabelasDisponiveis(connectionString);
            return todasTabelas.Where(t => parametros.ListaTabelasIgnoradas?.Contains(t) ?? false).ToList();
        }

        private async Task<List<string>> ObterTodasTabelasDisponiveis(string connectionString)
        {
            // Criar uma instância temporária para obter as tabelas
            var tempTreeView = new TreeviewListaTabelasBancoDadosPostgres();
            try
            {
                await tempTreeView.LoadTablesAsync(connectionString);
                return tempTreeView.GetAllAvailableTables();
            }
            catch
            {
                return new List<string>();
            }
            finally
            {
                tempTreeView.Dispose();
            }
        }

        private void LimparTreeViewTabelas()
        {
            tvListaTabelas.ClearSelection();
            AtualizarStatusConexao(false, "Dados de conexão incompletos");
        }

        private void AtualizarStatusConexao(bool isConnected, string message)
        {
            // Atualizar interface com status da conexão
            // Você pode adicionar um label ou status bar para mostrar isso
            // Exemplo:
            // lblStatusConexao.Text = message;
            // lblStatusConexao.ForeColor = isConnected ? Color.Green : Color.Red;
        }

        #endregion

        #region Eventos de Botões

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

        #endregion

        #region Métodos Override

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

        #endregion

        #region Eventos de Grids

        private void dtgAgendamentos_AddNewItem_User(object sender, EventArgs e)
        {
            FrmAgendamento frmAgendamento = new FrmAgendamento();
            frmAgendamento.btnInAltClick_user += FrmAgendamento_btnInAltClick_user;
            frmAgendamento.ShowDialog(this);
        }

        private void FrmAgendamento_btnInAltClick_user(object sender, EventArgs e)
        {
            ((ParametrosBackupModel)bdsParametros.Current).Agendamentos.Add((HorarioAgendamentoModel)sender);

            bdsParametros.ResetBindings(false);
        }

        private void dtgAgendamentos_RowRemoving_User(object sender, DataGridViewRowCancelEventArgs e)
        {
            // Implementar lógica se necessário
        }

        private void dtgAgendamentos_UpdateItem_User(object sender, EventArgs e)
        {
            // Implementar lógica se necessário
        }

        private async void dtgLocaisDeDestino_AddNewItem_User(object sender, EventArgs e)
        {
            var destino = new DestinoConfig();

            destino.Tipo = DestinoTipo.GoogleDrive;

            await GoogleDriveAuthManager.AutenticarAsync(destino.Id);

            ((ParametrosBackupModel)bdsParametros.Current).Destinos.Add(destino);

            bdsParametros.ResetBindings(false);
        }

        private void dtgLocaisDeDestino_RowRemoving_User(object sender, DataGridViewRowCancelEventArgs e)
        {
            // Implementar lógica se necessário
        }

        private void dtgLocaisDeDestino_UpdateItem_User(object sender, EventArgs e)
        {
            // Implementar lógica se necessário
        }

        private void frmBuscaListaBackpus_btnInAltClick_user(object sender, EventArgs e)
        {
            bdsParametros.DataSource = (ParametrosBackupModel)sender;
            EstadoBuscar();
        }

        private void frmListaBackups1_btnInAltClick_user(object sender, EventArgs e)
        {
            bdsParametros.DataSource = (ParametrosBackupModel)sender;
            EstadoBuscar();
        }

        #endregion
        
        #region Cleanup

        private void LimparEventos()
        {
            // Remover eventos para evitar memory leaks
            if (tvListaTabelas != null)
            {
                tvListaTabelas.SelectedTablesChanged -= TvListaTabelas_SelectedTablesChanged;
                tvListaTabelas.ConnectionStatusChanged -= TvListaTabelas_ConnectionStatusChanged;
            }

            if (bdsParametros != null)
            {
                //bdsParametros.CurrentChanged -= BdsParametros_CurrentChanged;
                bdsParametros.DataSourceChanged -= BdsParametros_DataSourceChanged;
            }
        }

        // Chame este método quando o formulário for fechado
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            LimparEventos();
            base.OnFormClosed(e);
        }

        #endregion

        private async void btnCarregarListaTabelas_Click(object sender, EventArgs e)
        {
            await AtualizarTreeViewTabelas();
        }

    }
}