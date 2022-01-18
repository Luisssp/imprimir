using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_Sqlite.Models
{
    class Produto
    {
        public int Id { get; set; }
        public string Carregamento  { get; set; }
        public string codigo { get; set; }
        public string produto { get; set; }
        public int QtdePed { get; set; }
        public string Peso { get; set; }
        public int QtdeConf { get; set; }
        public int QtdeAberto { get; set; }
        public string Peso_Unit { get; set; }
        public int QtdeTotal { get; set; }
        
    }
}
