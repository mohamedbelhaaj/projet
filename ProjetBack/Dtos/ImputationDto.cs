using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Dtos
{
    public class ImputationDto
    {
        public Imputation imputation { get; set; }
        public List<string> deletedTask { get; set; }

    }
}
