using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.backgroudservice
{
    public class ChangeTTM
    {
        private readonly PilotageDBContext _context;


        public ChangeTTM(PilotageDBContext context)
        {
            _context = context;
        }
        //public async Task<bool> ChangeTTMProjetLivraisonAsync()
        //{
        //    var projets = _context.ProjetLivraisons.Where(x => x.TTMId == null && x.StatusId == "Running" || x.StatusId == "Delivered").ToList();
        //    foreach (var item in projets)
        //    {
        //        if ((item.InitialPlannedDate.Date < DateTime.Now.Date && item.StatusId == "Running") || item.DeliveryDate > item.InitialPlannedDate&& item.StatusId == "Delivered")
        //        {
        //            item.TTMId = "late";
        //        }
        //        else if (item.DeliveryDate.Date == item.InitialPlannedDate.Date && item.StatusId == "Delivered")
        //        {
        //            item.TTMId = "ONTIME";
        //        }
        //        _context.Entry(item).State = EntityState.Modified;

        //    }
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //        return true;

        //    }
        //    catch (DbUpdateConcurrencyException e)
        //    {
        //        return false;
        //    }



        //}


        //public async Task<bool> ChangeTTMDetailProjetLivraisonAsync()
        //{
        //    var projets = _context.DetailLivraisons.Where(x => x.TTMId == null && x.StatusId == "Running" || x.StatusId == "Delivered").ToList();
        //    foreach (var item in projets)
        //    {
        //        if ((item.InitialPlannedDate.Date < DateTime.Now.Date && item.StatusId == "Running") || (item.DeliveryDate> item.InitialPlannedDate && item.StatusId == "Delivered"))
        //        {
        //            item.TTMId = "late";
        //        }
        //        else if (item.DeliveryDate.Date == item.InitialPlannedDate.Date && item.StatusId == "Delivered")
        //        {
        //            item.TTMId = "ONTIME";
        //        }
        //        _context.Entry(item).State = EntityState.Modified;

        //    }
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //        return true;

        //    }
        //    catch (DbUpdateConcurrencyException e)
        //    {
        //        return false;
        //    }

        //}



    }
}
