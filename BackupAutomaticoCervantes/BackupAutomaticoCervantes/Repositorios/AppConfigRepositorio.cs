using BackupAutomaticoCervantes.Padrao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.repositorios
{
    internal class AppConfigRepositorio
    {
        private readonly ConfigManager _cfg = ConfigManager.Instance;

        public IEnumerable<ParametrosBackupModel> GetAll() =>
            _cfg.Config.ListaDeParamentos;

        public ParametrosBackupModel GetById(Guid id) =>
            _cfg.Config.ListaDeParamentos.FirstOrDefault(t => t.Id == id);  // :contentReference[oaicite:2]{index=2}

        public void Add(ParametrosBackupModel task)
        {

            if (_cfg.Config.ListaDeParamentos.FindAll(item => item.Servidor == task.Servidor && item.NomebancoBancoDados == task.NomebancoBancoDados).Count > 0)
                throw new InvalidOperationException("Tarefa já existe com o mesmo servidor e banco de dados.");

            task.Id = Guid.NewGuid();
            _cfg.Config.ListaDeParamentos.Add(task);
            _cfg.SaveConfig();
        }

        public void Update(ParametrosBackupModel task)
        {
            var existing = GetById(task.Id);
            if (existing == null)
                throw new KeyNotFoundException("Tarefa não encontrada");

            // Atualiza somente os campos desejados
            existing.Servidor = task.Servidor;
            existing.Porta = task.Porta;
            existing.UsuarioBancoDados = task.UsuarioBancoDados;
            existing.SenhaUsuario = task.SenhaUsuario;
            existing.ListaTabelasIgnoradas = task.ListaTabelasIgnoradas;
            existing.NomebancoBancoDados = task.NomebancoBancoDados;
            existing.NomeArquivoDeBackup = task.NomeArquivoDeBackup;
            existing.CaminhoPgDump = task.CaminhoPgDump;
            existing.CaminhoPastaSalvarBackup = task.CaminhoPastaSalvarBackup;

            _cfg.SaveConfig();
        }

        public void Delete(Guid id)
        {
            var task = GetById(id);
            if (task != null)
            {
                _cfg.Config.ListaDeParamentos.Remove(task);
                _cfg.SaveConfig();
            }
        }
    }
}
