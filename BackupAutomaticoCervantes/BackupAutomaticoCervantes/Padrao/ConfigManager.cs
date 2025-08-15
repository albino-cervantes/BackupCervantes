using BackupAutomaticoCervantes.DestinoBackup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.Padrao
{
    public sealed class ConfigManager
    {
        // 1) Instância única, inicialização lazy e thread-safe via Lazy<T>
        private static readonly Lazy<ConfigManager> _instance =
            new Lazy<ConfigManager>(() => new ConfigManager());  // :contentReference[oaicite:0]{index=0}

        public static ConfigManager Instance => _instance.Value;

        private const string ConfigFileName = "ConfigBackupAutomaticoCervantes.json";
        private readonly string _configPath;

        // 1a) path override opcional
        private static string _overrideConfigPath;

        // 2) Objeto que representa toda a configuração
        public AppConfigModel Config { get; private set; }

        // Construtor privado: carrega ou inicializa arquivo JSON
        private ConfigManager()
        {
            // se foi definido um override, usa-o; senão cai no BaseDirectory
            if (!string.IsNullOrEmpty(_overrideConfigPath))
                _configPath = _overrideConfigPath;
            else
                _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);

            LoadConfig();
        }

        /// <summary>
        /// Permite informar explicitamente um arquivo de configuração antes de usar Instance.
        /// Deve ser chamado _antes_ de qualquer acesso a ConfigManager.Instance.
        /// </summary>
        public static void SetConfigPath(string fullPath)
        {
            _overrideConfigPath = fullPath;
        }

        private void LoadConfig()
        {
            if (!File.Exists(_configPath))
            {
                // Se não existir, cria configuração padrão e salva
                Config = new AppConfigModel { ListaDeParamentos = new List<ParametrosBackupModel>() };
                SaveConfig();
            }
            else
            {
                string json = File.ReadAllText(_configPath);

                var settings = new JsonSerializerSettings
                {
                    // registra globalmente o conversor para IDestinoConfig
                    Converters = { new DestinoConfigConverter() }
                };

                // desserializa AppConfigModel inteiro usando esses settings
                Config = JsonConvert.DeserializeObject<AppConfigModel>(json, settings);
            }
        }

        public void SaveConfig()
        {
            string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(_configPath, json);
        }
    }
}
