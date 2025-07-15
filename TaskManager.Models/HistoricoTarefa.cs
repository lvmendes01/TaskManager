using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class HistoricoTarefa
    {
        public int Id { get; set; }
        public int TarefaId { get; set; }
        public string Alteracao { get; set; } = string.Empty;
        public DateTime DataAlteracao { get; set; }
        public string Usuario { get; set; } = string.Empty;
    }
}
