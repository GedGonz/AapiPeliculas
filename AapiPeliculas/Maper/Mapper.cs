using AapiPeliculas.Models;
using AapiPeliculas.Models.DTOS;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Maper
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Categoria, Categoriadto>().ReverseMap();
        }
    }
}
