using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Dtos
{
    public class FiltreProjetComptableDto
    {

        public string statut { get; set; }
        public string client { get; set; }
        public int annees { get; set; }
        public string commercialId { get; set; }
    }
}
