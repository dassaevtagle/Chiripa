using System;
using System.Collections.Generic;
using System.Linq;
using ChiripaAPI.Data;
using ChiripaAPI.Models;
using ChiripaAPI.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChiripaAPI.Services.Repositories
{
    public class HielitoRepo : IHielito
    {
        private readonly ChiripaDbContext db;

        public HielitoRepo(ChiripaDbContext db)
        {
            this.db = db;
        }
        public Hielito AddNewHielito(Hielito hielito)
        {
            // If a hielito already has this name
            if(db.Hielitos.FirstOrDefault(h => h.Name.ToLower() == hielito.Name.ToLower()) != null)
            {
                return null;
            }

            if(db.Hielitos.Find(1) == null)
            {
                hielito.Id = 1;
                db.Hielitos.Add(hielito);
                db.SaveChanges();
                return hielito;
            }
            else
            {
                var biggestId = db.Hielitos.OrderByDescending(h => h.Id).FirstOrDefault();

                hielito.Id = biggestId.Id + 1;
                db.Hielitos.Add(hielito);
                db.SaveChanges();
                return hielito;
            }  
        }

        public void DeleteHielito(int id)
        {
            var hielito = db.Hielitos.Single(h => h.Id == id);
            db.Hielitos.Remove(hielito);
            db.SaveChanges();
        }

        public List<Hielito> GetAllHielitos()
        {
            return db.Hielitos.Select(h => h).ToList();
        }

        public Hielito GetHielitoById(int id)
        {
            var hielito = db.Hielitos.Find(id);
            if(hielito == null)
            {
                return null;
            }
            return db.Hielitos.Single( h => h.Id == id);
        }

        public void UpdateHielito(Hielito hielito)
        {
            db.Update(hielito);
            db.SaveChanges();
        }
    }
}