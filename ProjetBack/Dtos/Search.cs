using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Dtos
{
    public class Search
    {
        public int page { get; set; }
        public int size { get; set; }
        public int AssocitionProjetCompatble { get; set; }

        public string projetLivraisonId { get; set; }
        public string idUser { get; set; }
        public string detailLivraisonId { get; set; }
        public string status { get; set; }
        public string client { get; set; }
        public string type { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string clientId { get; set; }
        public string nature { get; set; }

        public string ManagerId { get; set; }
        public string commercialId { get; set; }
        public int annees { get; set; }

        public List<string> idClients { get; set; }

        public List<string> types { get; set; }
        public string dateDebut { get; set; }
        public string dateFin { get; set; }
    }
}
