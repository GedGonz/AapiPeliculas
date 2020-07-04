using AapiPeliculas.Models;
using AapiPeliculas.Models.DTOS;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Maper
{
    public class ProfileMappers:Profile
    {
        public ProfileMappers()
        {
            CreateMap<Categoria, Categoriadto>().ReverseMap();

            CreateMap<Pelicula, Peliculadto>().ReverseMap();
            CreateMap<Pelicula, PeliculaCreatedto>().ReverseMap();
            CreateMap<Pelicula, PeliculaUpdatedto>().ReverseMap();
            CreateMap<Usuario, Usuariodto>().ReverseMap();
            CreateMap<Usuario, UsuarioAuthdto>().ReverseMap();
            CreateMap<Usuario, UsuarioAuthLogindto>().ReverseMap();
        }
    }
}
