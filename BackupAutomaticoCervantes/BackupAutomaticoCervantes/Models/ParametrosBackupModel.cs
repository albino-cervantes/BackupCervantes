using BackupAutomaticoCervantes.DestinoBackup;
using BackupAutomaticoCervantes.Models;
using RognusFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes
{
    public class ParametrosBackupModel : IFrmModeloBuscaItem
    {
        #region Atributos
        private Guid _id;
        private string _servidor;
        private string _porta;
        private string _usuarioBancoDados;
        private string _senhaUsuario;
        private List<string> _listaTabelasIgnoradas;
        private string _nomebancoBancoDados;
        private string _nomeArquivoDeBackup;
        private string _caminhoPastaSalvarBackup;
        private string _caminhoPgDump;
        private List<HorarioAgendamentoModel> _agendamentos = new List<HorarioAgendamentoModel>();
        private List<DestinoConfig> _destinos = new List<DestinoConfig>();

        #endregion

        #region Propriedades

        public string Servidor { get => _servidor; set => _servidor = value; }
        public string Porta { get => _porta; set => _porta = value; }
        public string UsuarioBancoDados { get => _usuarioBancoDados; set => _usuarioBancoDados = value; }
        public string SenhaUsuario { get => _senhaUsuario; set => _senhaUsuario = value; }
        public List<string> ListaTabelasIgnoradas { get => _listaTabelasIgnoradas; set => _listaTabelasIgnoradas = value; }
        public string NomebancoBancoDados { get => _nomebancoBancoDados; set => _nomebancoBancoDados = value; }
        public string NomeArquivoDeBackup { get => _nomeArquivoDeBackup; set => _nomeArquivoDeBackup = value; }
        public string CaminhoPgDump { get => _caminhoPgDump; set => _caminhoPgDump = value; }
        public string CaminhoPastaSalvarBackup { get => _caminhoPastaSalvarBackup; set => _caminhoPastaSalvarBackup = value; }
        public Guid Id { get => _id; set => _id = value; }
        public List<HorarioAgendamentoModel> Agendamentos { get => _agendamentos; set => _agendamentos = value; }
        public List<DestinoConfig> Destinos { get => _destinos; set => _destinos = value; } 


        #endregion

        #region Construtor
        public ParametrosBackupModel(string servidor,
                                   string porta,
                                   string usuarioBancoDados,
                                   string senhaUsuario,
                                   List<string> listaTabelasIgnoradas,
                                   string nomebancoBancoDados,
                                   string nomeArquivoDeBackup,
                                   string caminhoPgDump,
                                   string caminhoPastaParaSalvarBackup,
                                   Guid id,
                                   List<HorarioAgendamentoModel> agendamentos,
                                   List<DestinoConfig> destinos)
        {
            _servidor = servidor;
            _porta = porta;
            _usuarioBancoDados = usuarioBancoDados;
            _senhaUsuario = senhaUsuario;
            _listaTabelasIgnoradas = listaTabelasIgnoradas;
            _nomebancoBancoDados = nomebancoBancoDados;
            _nomeArquivoDeBackup = nomeArquivoDeBackup;
            _caminhoPgDump = caminhoPgDump;
            _caminhoPastaSalvarBackup = caminhoPastaParaSalvarBackup;
            _id = id;
            _agendamentos = agendamentos;
            _destinos = destinos;
        }

        public ParametrosBackupModel() { }

        public ParametrosBackupModel Clone()
        {
            return new ParametrosBackupModel(
                _servidor,
                _porta,
                _usuarioBancoDados,
                _senhaUsuario,
                _listaTabelasIgnoradas,
                _nomebancoBancoDados,
                _nomeArquivoDeBackup,
                _caminhoPgDump,
                _caminhoPastaSalvarBackup,
                _id,
                _agendamentos,
                _destinos);
        }


        RognusFramework.IFrmModeloBuscaItem RognusFramework.IFrmModeloBuscaItem.Clone()
        {
            return this.Clone();
        }

       public long GetId()
        {
            return (long)_id.GetHashCode();
        }

        #endregion

    }
}
