using AapiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Repositorios.IRepository
{
    public interface PeliculaRepositoriocs
    {
        ICollection<Pelicula> GetPeliculas();
        ICollection<Pelicula> GetPeliculasEnCategoria(int IdCategoria);
        Pelicula GetPelicula(int IdPelicula);
        bool ExistPelicula(string nombre);
        IEnumerable<Pelicula> BuscarPelicula(string nombre);
        bool ExistPelicula(int IdPelicula);
        bool CrearPelicula(Pelicula pelicula);
        bool ActualizarPelicula(Pelicula pelicula);
        bool EliminarPelicula(Pelicula pelicula);
        bool Guardar();
    }
}
