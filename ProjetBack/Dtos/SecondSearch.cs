using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Dtos
{
    public class SecondSearch
    {
        public List<string> idClients { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string type { get; set; }
        public string client { get; set; }

    }
}
