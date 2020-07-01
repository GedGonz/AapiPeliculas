using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AapiPeliculas.Models.DTOS;
using AapiPeliculas.Repositorios.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AapiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IMapper mapper;
        public CategoriasController(ICategoriaRepositorio _categoriaRepositorio, IMapper mapper)
        {
            this._categoriaRepositorio = _categoriaRepositorio;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCategorias() 
        {
            var listaCategorias = _categoriaRepositorio.GetCategorias();

            var listaCategoriasdto = mapper.Map<List<Categoriadto>>(listaCategorias);

            return Ok(listaCategoriasdto);
        }


    }
}
