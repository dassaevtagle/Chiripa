using System;
using System.Collections.Generic;
using ChiripaAPI.Models;

namespace ChiripaAPI.Services.Infrastructure
{
    public interface IHielito
    {
         Hielito AddNewHielito(Hielito hielito);

         void UpdateHielito(Hielito hielito);
         void DeleteHielito(int id);
         Hielito GetHielitoById(int id);
         List<Hielito> GetAllHielitos();
         
    }
}