using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceMDS.Models
{
    public class Pais
    {
        public string sCodigo { get; set; }
        public string sNombre { get; set; }
        public string sPais { get; set; }
        public DateTime? dFechaInicio { get; set; }
        public DateTime? dFechaFin { get; set; }

    }
}
