using AapiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Repositorios.IRepository
{
    public interface ICategoriaRepositorio
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int IdCategoria);
        bool ExistCategoria(string nombre);
        bool ExistCategoria(int IdCategoria);
        bool CrearCategoria(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);
        bool EliminarCategoria(Categoria categoria);
        bool Guardar();
    }
}
