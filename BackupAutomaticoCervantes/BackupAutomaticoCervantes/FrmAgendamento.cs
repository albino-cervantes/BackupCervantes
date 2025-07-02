using BackupAutomaticoCervantes.Models;
using BackupAutomaticoCervantes.Padrao;
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
    public partial class FrmAgendamento : FrmModeloSimple
    {
        public HorarioAgendamentoModel Resultado { get; private set; }

        public FrmAgendamento(HorarioAgendamentoModel agendamentoModel = null)
            : base(Eoperacao.Ok)
        {
            InitializeComponent();

            bdsHorarioAgendamento.DataSource = new HorarioAgendamentoModel();

            dtpHora.Format = DateTimePickerFormat.Time;
            dtpHora.ShowUpDown = true;

            chkDias.Items.AddRange(Enum.GetNames(typeof(DiasDaSemana)));

            if (agendamentoModel != null)
            {
                dtpHora.Value = DateTime.Today + agendamentoModel.Hora;
                chkTodosOsDias.Checked = agendamentoModel.ExecutarTodosOsDias;

                foreach (var dia in agendamentoModel.DiasDaSemana)
                    chkDias.SetItemChecked((int)dia, true);
            }
        }

        public override void btnInAlt_Click(object sender, EventArgs e)
        {

            foreach (var item in chkDias.CheckedItems)
            {
                ((HorarioAgendamentoModel)bdsHorarioAgendamento.Current).DiasDaSemana.Add((DiasDaSemana)Enum.Parse(typeof(DiasDaSemana), item.ToString()));
            }

            if (bdsHorarioAgendamento.Current != null)
                base.btnInAlt_Click((HorarioAgendamentoModel)bdsHorarioAgendamento.Current, e);

            this.Close();
        }

        private void chkTodosOsDias_CheckedChanged(object sender, EventArgs e)
        {
            if(chkTodosOsDias.Checked)
                chkDias.Enabled = false;
            else
                chkDias.Enabled = true;
        }
    }
}
