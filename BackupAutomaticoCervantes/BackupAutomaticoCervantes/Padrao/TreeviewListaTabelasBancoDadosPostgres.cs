using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RognusFramework.Componentes;

namespace BackupAutomaticoCervantes.Padrao
{
    public partial class TreeviewListaTabelasBancoDadosPostgres : UserControlDefault
    {
        #region Campos Privados
        private TreeViewDefault treeViewTables;
        private string connectionString;
        private bool isLoading = false;
        private bool _suppressEvents = false; // Para controlar eventos durante atualizações
        private readonly object _lockObject = new object(); // Para thread safety
        private Dictionary<string, bool> _previousSelectionState; // Cache do estado anterior
        #endregion

        #region Eventos Públicos
        /// <summary>
        /// Evento disparado quando a seleção de tabelas é alterada
        /// </summary>
        public event EventHandler<SelectedTablesChangedEventArgs> SelectedTablesChanged;

        /// <summary>
        /// Evento disparado quando o status da conexão muda
        /// </summary>
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;

        /// <summary>
        /// Evento disparado quando o carregamento das tabelas é iniciado
        /// </summary>
        public event EventHandler LoadingStarted;

        /// <summary>
        /// Evento disparado quando o carregamento das tabelas é finalizado
        /// </summary>
        public event EventHandler LoadingFinished;
        #endregion

        #region Propriedades Públicas
        /// <summary>
        /// String de conexão com o banco PostgreSQL
        /// </summary>
        [Category("Database")]
        [Description("String de conexão com o banco PostgreSQL")]
        public string ConnectionString
        {
            get { return connectionString; }
            set
            {
                if (connectionString != value)
                {
                    connectionString = value;
                    OnConnectionStringChanged();
                }
            }
        }

        /// <summary>
        /// Indica se os checkboxes devem ser exibidos
        /// </summary>
        [Category("Appearance")]
        [Description("Indica se os checkboxes devem ser exibidos")]
        [DefaultValue(true)]
        public bool ShowCheckBoxes
        {
            get { return treeViewTables?.CheckBoxes ?? true; }
            set
            {
                if (treeViewTables != null)
                    treeViewTables.CheckBoxes = value;
            }
        }

        /// <summary>
        /// Lista das tabelas atualmente selecionadas
        /// </summary>
        [Browsable(false)]
        public List<string> SelectedTables => GetSelectedTables();

        /// <summary>
        /// Indica se o controle está carregando dados
        /// </summary>
        [Browsable(false)]
        public bool IsLoading => isLoading;

        /// <summary>
        /// Indica se há uma conexão válida estabelecida
        /// </summary>
        [Browsable(false)]
        public bool IsConnected { get; private set; }
        #endregion

        #region Construtor
        public TreeviewListaTabelasBancoDadosPostgres()
        {
            this.Name = "DatabaseTableSelector";
            this.BackColor = SystemColors.Control;

            CreateTreeView();
            InitializeComponent();

            _previousSelectionState = new Dictionary<string, bool>();
        }
        #endregion

        #region Inicialização
        private void CreateTreeView()
        {
            treeViewTables = new TreeViewDefault();
            treeViewTables.Name = "treeViewTables";
            treeViewTables.CheckBoxes = true;
            treeViewTables.FullRowSelect = true;
            treeViewTables.HideSelection = false;
            treeViewTables.ShowLines = true;
            treeViewTables.ShowPlusMinus = true;
            treeViewTables.ShowRootLines = true;
            treeViewTables.Dock = DockStyle.Fill;
            treeViewTables.AfterCheck += TreeViewTables_AfterCheck;
            treeViewTables.NodeMouseDoubleClick += TreeViewTables_NodeMouseDoubleClick;

            this.Controls.Add(treeViewTables);
        }

        private void OnConnectionStringChanged()
        {
            // Limpar dados quando a connection string mudar
            ClearTreeView();
            IsConnected = false;
        }
        #endregion

        #region Eventos do TreeView
        private void TreeViewTables_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown || isLoading || _suppressEvents)
                return;

            // Suprimir eventos temporariamente para evitar recursão
            //_suppressEvents = true;

            // Evitar recursão infinita
            treeViewTables.AfterCheck -= TreeViewTables_AfterCheck;

            try
            {
                // Atualizar nós filhos baseado no nó pai
                UpdateChildNodes(e.Node, e.Node.Checked);

                // Atualizar nós pais baseado nos filhos
                UpdateParentNodes(e.Node);

                // Verificar se houve mudança real na seleção
                if (HasSelectionChanged())
                {
                    UpdatePreviousSelectionState();
                    OnSelectedTablesChanged();
                }
            }
            finally
            {
                //_suppressEvents = false;
                treeViewTables.AfterCheck += TreeViewTables_AfterCheck;
            }
        }

        private void TreeViewTables_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag.ToString().Contains(".") &&
                !e.Node.Tag.ToString().StartsWith("schema:"))
            {
                OnTableDoubleClicked(e.Node.Tag.ToString());
            }
        }
        #endregion

        #region Métodos de Atualização de Nós
        /// <summary>
        /// Atualiza todos os nós filhos com o mesmo estado do pai
        /// </summary>
        private void UpdateChildNodes(TreeNode parentNode, bool isChecked)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                childNode.Checked = isChecked;

                // Recursivamente atualizar filhos dos filhos
                if (childNode.Nodes.Count > 0)
                {
                    UpdateChildNodes(childNode, isChecked);
                }
            }
        }

        /// <summary>
        /// Atualiza o estado dos nós pais baseado no estado dos filhos
        /// </summary>
        private void UpdateParentNodes(TreeNode childNode)
        {
            TreeNode parentNode = childNode.Parent;

            if (parentNode != null)
            {
                // Verificar o estado de todos os filhos
                bool allChecked = true;
                bool anyChecked = false;

                foreach (TreeNode sibling in parentNode.Nodes)
                {
                    if (sibling.Checked)
                    {
                        anyChecked = true;
                    }
                    else
                    {
                        allChecked = false;
                    }
                }

                // Atualizar o pai baseado no estado dos filhos
                if (allChecked)
                {
                    parentNode.Checked = true;
                }
                else if (!anyChecked)
                {
                    parentNode.Checked = false;
                }
                // Se alguns estão marcados e outros não, deixar o pai desmarcado
                // Você pode implementar um estado "indeterminado" aqui se desejar

                // Recursivamente atualizar os pais
                UpdateParentNodes(parentNode);
            }
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Conecta ao banco e carrega a estrutura de tabelas de forma assíncrona
        /// </summary>
        /// <param name="connectionString">String de conexão (opcional)</param>
        public async Task LoadTablesAsync(string connectionString = null)
        {
            if (!string.IsNullOrEmpty(connectionString))
                this.connectionString = connectionString;

            if (string.IsNullOrEmpty(this.connectionString))
            {
                OnConnectionStatusChanged(false, "String de conexão não informada");
                return;
            }

            await LoadDatabaseStructureAsync();
        }

        /// <summary>
        /// Obtém todas as tabelas disponíveis no banco
        /// </summary>
        /// <returns>Lista com nomes completos das tabelas</returns>
        public List<string> GetAllAvailableTables()
        {
            var allTables = new List<string>();
            if (treeViewTables?.Nodes != null)
                GetAllTablesRecursive(treeViewTables.Nodes, allTables);
            return allTables;
        }

        /// <summary>
        /// Obtém a lista de tabelas selecionadas
        /// </summary>
        /// <returns>Lista com nomes completos das tabelas (schema.tabela)</returns>
        public List<string> GetSelectedTables()
        {
            var selectedTables = new List<string>();
            if (treeViewTables?.Nodes != null)
                GetSelectedTablesRecursive(treeViewTables.Nodes, selectedTables);
            return selectedTables;
        }

        /// <summary>
        /// Define quais tabelas devem estar selecionadas
        /// </summary>
        /// <param name="tableNames">Lista de nomes de tabelas (schema.tabela)</param>
        public void SetSelectedTables(List<string> tableNames)
        {
            if (tableNames == null || treeViewTables?.Nodes == null)
                return;

            lock (_lockObject)
            {
                _suppressEvents = true;

                try
                {
                    // Primeiro desmarcar todos
                    UncheckAllNodes(treeViewTables.Nodes);

                    // Marcar as tabelas específicas
                    CheckSpecificTables(treeViewTables.Nodes, tableNames);

                    // Atualizar estado dos pais baseado nos filhos selecionados
                    UpdateAllParentStates(treeViewTables.Nodes);

                    UpdatePreviousSelectionState();
                }
                finally
                {
                    _suppressEvents = false;
                }
            }

            // Disparar evento após atualização
            OnSelectedTablesChanged();
        }

        /// <summary>
        /// Limpa todas as seleções
        /// </summary>
        public void ClearSelection()
        {
            if (treeViewTables?.Nodes == null)
                return;

            lock (_lockObject)
            {
                _suppressEvents = true;

                try
                {
                    UncheckAllNodes(treeViewTables.Nodes);
                    treeViewTables.Nodes.Clear();
                    UpdatePreviousSelectionState();
                }
                finally
                {
                    _suppressEvents = false;
                }
            }

            OnSelectedTablesChanged();
        }

        /// <summary>
        /// Recarrega a estrutura do banco de dados
        /// </summary>
        public async Task RefreshAsync()
        {
            await LoadDatabaseStructureAsync();
        }

        /// <summary>
        /// Testa a conexão com o banco de dados
        /// </summary>
        /// <param name="connectionString">String de conexão (opcional)</param>
        /// <returns>True se a conexão for bem-sucedida</returns>
        public async Task<bool> TestConnectionAsync(string connectionString = null)
        {
            string connStr = connectionString ?? this.connectionString;

            if (string.IsNullOrEmpty(connStr))
                return false;

            try
            {
                using (var connection = new NpgsqlConnection(connStr))
                {
                    await connection.OpenAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Métodos Privados
        private async Task LoadDatabaseStructureAsync()
        {
            if (string.IsNullOrEmpty(connectionString))
                return;

            lock (_lockObject)
            {
                if (isLoading)
                    return; // Evitar múltiplas chamadas simultâneas

                isLoading = true;
            }

            OnLoadingStarted();

            try
            {
                // Salvar seleção atual
                var currentSelection = GetSelectedTables();

                await Task.Run(() => LoadDatabaseStructure());

                IsConnected = true;
                OnConnectionStatusChanged(true, "Conectado com sucesso");

                // Restaurar seleção anterior se possível
                if (currentSelection.Any())
                {
                    SetSelectedTables(currentSelection);
                }
            }
            catch (Exception ex)
            {
                IsConnected = false;
                OnConnectionStatusChanged(false, $"Erro de conexão: {ex.Message}");
                ClearTreeView();
                throw;
            }
            finally
            {
                isLoading = false;
                OnLoadingFinished();
            }
        }

        private void LoadDatabaseStructure()
        {
            InvokeIfRequired(() =>
            {
                _suppressEvents = true;
                treeViewTables.Nodes.Clear();
                _suppressEvents = false;
            });

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string databaseName = connection.Database;

                // Criar nó raiz do banco
                TreeNode databaseNode = new TreeNode($"Database [{databaseName}]")
                {
                    ImageIndex = 0,
                    Tag = "database",
                    Checked = false
                };

                // Obter todos os schemas
                var schemas = GetSchemas(connection);

                foreach (var schema in schemas)
                {
                    TreeNode schemaNode = new TreeNode(schema)
                    {
                        ImageIndex = 1,
                        Tag = $"schema:{schema}",
                        Checked = false
                    };
                    databaseNode.Nodes.Add(schemaNode);

                    // Obter tabelas do schema
                    var tables = GetTables(connection, schema);

                    foreach (var table in tables)
                    {
                        TreeNode tableNode = new TreeNode(table)
                        {
                            ImageIndex = 2,
                            Tag = $"{schema}.{table}",
                            Checked = false
                        };
                        schemaNode.Nodes.Add(tableNode);
                    }
                }

                // Adicionar ao TreeView na thread da UI
                InvokeIfRequired(() =>
                {
                    _suppressEvents = true;
                    treeViewTables.Nodes.Add(databaseNode);
                    databaseNode.Expand();

                    // Expandir todos os schemas por padrão
                    foreach (TreeNode schemaNode in databaseNode.Nodes)
                    {
                        schemaNode.Expand();
                    }
                    _suppressEvents = false;
                    UpdatePreviousSelectionState();
                });
            }
        }

        private List<string> GetSchemas(NpgsqlConnection connection)
        {
            var schemas = new List<string>();

            string query = @"
                SELECT schema_name 
                FROM information_schema.schemata 
                WHERE schema_name NOT ILIKE ('information_schema%') AND schema_name NOT ILIKE ('pg_%')
                ORDER BY schema_name";

            using (var command = new NpgsqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    schemas.Add(reader.Get<string>("schema_name"));
                }
            }

            return schemas;
        }

        private List<string> GetTables(NpgsqlConnection connection, string schemaName)
        {
            var tables = new List<string>();

            string query = @"
                SELECT table_name 
                FROM information_schema.tables 
                WHERE table_schema = @schema 
                AND table_type = 'BASE TABLE'
                ORDER BY table_name";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@schema", schemaName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.Get<string>("table_name"));
                    }
                }
            }

            return tables;
        }

        private void GetSelectedTablesRecursive(TreeNodeCollection nodes, List<string> selectedTables)
        {
            foreach (TreeNode node in nodes)
            {
                // Verificar se é um nó de tabela
                if (node.Checked && IsTableNode(node))
                {
                    selectedTables.Add(node.Tag.ToString());
                }

                if (node.Nodes.Count > 0)
                {
                    GetSelectedTablesRecursive(node.Nodes, selectedTables);
                }
            }
        }

        private void GetAllTablesRecursive(TreeNodeCollection nodes, List<string> allTables)
        {
            foreach (TreeNode node in nodes)
            {
                if (IsTableNode(node))
                {
                    allTables.Add(node.Tag.ToString());
                }

                if (node.Nodes.Count > 0)
                {
                    GetAllTablesRecursive(node.Nodes, allTables);
                }
            }
        }

        private bool IsTableNode(TreeNode node)
        {
            return node.Tag != null &&
                   node.Tag.ToString().Contains(".") &&
                   !node.Tag.ToString().StartsWith("schema:") &&
                   node.Tag.ToString() != "database";
        }

        private void UncheckAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
                if (node.Nodes.Count > 0)
                {
                    UncheckAllNodes(node.Nodes);
                }
            }
        }

        private void CheckSpecificTables(TreeNodeCollection nodes, List<string> tableNames)
        {
            foreach (TreeNode node in nodes)
            {
                if (IsTableNode(node) && tableNames.Contains(node.Tag.ToString()))
                {
                    node.Checked = true;
                }

                if (node.Nodes.Count > 0)
                {
                    CheckSpecificTables(node.Nodes, tableNames);
                }
            }
        }

        /// <summary>
        /// Atualiza o estado de todos os nós pais baseado nos filhos
        /// </summary>
        private void UpdateAllParentStates(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    // Primeiro atualizar recursivamente os filhos
                    UpdateAllParentStates(node.Nodes);

                    // Depois atualizar este nó baseado nos filhos
                    bool allChecked = true;
                    bool anyChecked = false;

                    foreach (TreeNode child in node.Nodes)
                    {
                        if (child.Checked)
                        {
                            anyChecked = true;
                        }
                        else
                        {
                            allChecked = false;
                        }
                    }

                    // Se todos os filhos estão marcados, marcar o pai
                    // Se nenhum filho está marcado, desmarcar o pai
                    if (allChecked && node.Nodes.Count > 0)
                    {
                        node.Checked = true;
                    }
                    else if (!anyChecked)
                    {
                        node.Checked = false;
                    }
                }
            }
        }

        private void ClearTreeView()
        {
            InvokeIfRequired(() =>
            {
                _suppressEvents = true;
                treeViewTables.Nodes.Clear();
                _suppressEvents = false;
                UpdatePreviousSelectionState();
            });
        }

        private void InvokeIfRequired(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private bool HasSelectionChanged()
        {
            var currentSelection = GetSelectedTables();
            var currentState = currentSelection.ToDictionary(t => t, t => true);

            // Comparar com estado anterior
            if (_previousSelectionState.Count != currentState.Count)
                return true;

            foreach (var kvp in currentState)
            {
                if (!_previousSelectionState.ContainsKey(kvp.Key))
                    return true;
            }

            return false;
        }

        private void UpdatePreviousSelectionState()
        {
            _previousSelectionState = GetSelectedTables().ToDictionary(t => t, t => true);
        }
        #endregion

        #region Eventos Virtuais
        protected virtual void OnSelectedTablesChanged()
        {
            if (_suppressEvents) return;

            var args = new SelectedTablesChangedEventArgs(GetSelectedTables());
            SelectedTablesChanged?.Invoke(this, args);
        }

        protected virtual void OnConnectionStatusChanged(bool isConnected, string message)
        {
            var args = new ConnectionStatusChangedEventArgs(isConnected, message);
            ConnectionStatusChanged?.Invoke(this, args);
        }

        protected virtual void OnTableDoubleClicked(string tableName)
        {
            // Pode ser sobrescrito por classes derivadas
        }

        protected virtual void OnLoadingStarted()
        {
            LoadingStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoadingFinished()
        {
            LoadingFinished?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Cleanup
        private void CleanupResources()
        {
            if (treeViewTables != null)
            {
                treeViewTables.AfterCheck -= TreeViewTables_AfterCheck;
                treeViewTables.NodeMouseDoubleClick -= TreeViewTables_NodeMouseDoubleClick;
            }

            _previousSelectionState?.Clear();
        }

        // Chamado automaticamente quando o controle é destruído
        ~TreeviewListaTabelasBancoDadosPostgres()
        {
            CleanupResources();
        }
        #endregion
    }

    #region Classes de EventArgs
    /// <summary>
    /// Argumentos do evento SelectedTablesChanged
    /// </summary>
    public class SelectedTablesChangedEventArgs : EventArgs
    {
        public List<string> SelectedTables { get; }

        public SelectedTablesChangedEventArgs(List<string> selectedTables)
        {
            SelectedTables = selectedTables ?? new List<string>();
        }
    }

    /// <summary>
    /// Argumentos do evento ConnectionStatusChanged
    /// </summary>
    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        public bool IsConnected { get; }
        public string Message { get; }

        public ConnectionStatusChangedEventArgs(bool isConnected, string message)
        {
            IsConnected = isConnected;
            Message = message;
        }
    }
    #endregion
}