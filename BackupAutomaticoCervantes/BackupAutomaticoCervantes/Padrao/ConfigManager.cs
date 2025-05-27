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

        // 2) Objeto que representa toda a configuração
        public AppConfigModel Config { get; private set; }

        // Construtor privado: carrega ou inicializa arquivo JSON
        private ConfigManager()
        {
            _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
            LoadConfig();
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
                Config = JsonConvert.DeserializeObject<AppConfigModel>(json);  // :contentReference[oaicite:1]{index=1}
            }
        }

        public void SaveConfig()
        {
            string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(_configPath, json);
        }
    }
}
