using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSNivelAcademico.Models
{
    public class NivelAcademico
    {
        public string sCodigo { get; set; }
        public string sNombre { get; set; }
        public string sDescripcion { get; set; }
        public DateTime? dFechaInicio { get; set; }
        public DateTime? dFechaFin { get; set; }
        public decimal? idNivelEducativo { get; set; }

    }
}
