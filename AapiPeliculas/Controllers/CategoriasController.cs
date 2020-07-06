using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AapiPeliculas.Models;
using AapiPeliculas.Models.DTOS;
using AapiPeliculas.Repositorios.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AapiPeliculas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiPeliculasCategorias")]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IMapper mapper;
        public CategoriasController(ICategoriaRepositorio _categoriaRepositorio, IMapper mapper)
        {
            this._categoriaRepositorio = _categoriaRepositorio;
            this.mapper = mapper;
        }
        /// <summary>
        /// Obtener las Categorias
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200,Type = typeof(List<Categoriadto>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategorias() 
        {
            var listaCategorias = _categoriaRepositorio.GetCategorias();

            var listaCategoriasdto = mapper.Map<List<Categoriadto>>(listaCategorias);

            return Ok(listaCategoriasdto);
        }

        /// <summary>
        /// Obtener una Categoria
        /// </summary>
        /// <param name="IdCategoria">Id de la categoria</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{IdCategoria:int}",Name = "GetCategoria")]
        [ProducesResponseType(200, Type = typeof(Categoriadto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCategoria(int IdCategoria) 
        {
            var categoria = _categoriaRepositorio.GetCategoria(IdCategoria);

            if (categoria==null) 
            {
                return NotFound();
            }

            var categoriaDTO = mapper.Map<Categoriadto>(categoria);

            return Ok(categoriaDTO);
        }

        /// <summary>
        /// Crear una Categoria
        /// </summary>
        /// <param name="categoriadto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Categoriadto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCtaegoria([FromBody] Categoriadto categoriadto) 
        {
            if (categoriadto==null) 
            {
                return BadRequest(ModelState);
            }

            if (_categoriaRepositorio.ExistCategoria(categoriadto.Nombre)) 
            {
                ModelState.AddModelError("", "La Categoria ya existe");

                return StatusCode(404,ModelState);
            }
            var categoria = mapper.Map<Categoria>(categoriadto);

            if (!_categoriaRepositorio.CrearCategoria(categoria)) 
            {
                ModelState.AddModelError("", $"Algo salio mal al crear el registro {categoria.Nombre}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { IdCategoria = categoria.Id},categoria);

        }
        /// <summary>
        /// Actualiza una Categoria existente
        /// </summary>
        /// <param name="IdCategoria"></param>
        /// <param name="categoriadto"></param>
        /// <returns></returns>
        
        [HttpPatch("{IdCategoria:int}",Name = "ActualizarCategoria")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCategoria(int IdCategoria, [FromBody] Categoriadto categoriadto) 
        {

            if (categoriadto==null || IdCategoria!=categoriadto.Id)
            {
                return BadRequest(ModelState);
            }

            var categoria = mapper.Map<Categoria>(categoriadto);

            if (!_categoriaRepositorio.ActualizarCategoria(categoria)) 
            {
                ModelState.AddModelError("", $"Algo salio mal al actualizar el registro {categoria.Nombre}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Eliminar una Categoria
        /// </summary>
        /// <param name="IdCategoria">Id de la Categoria</param>
        /// <returns></returns>
        [HttpDelete("{IdCategoria:int}", Name = "EliminarCategoria")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EliminarCategoria(int IdCategoria)
        {


           //var categoria = mapper.Map<Categoria>(categoriadto);

            if (!_categoriaRepositorio.ExistCategoria(IdCategoria))
            {
                return NotFound();
            }

            var categoria = _categoriaRepositorio.GetCategoria(IdCategoria);



            if (!_categoriaRepositorio.EliminarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal al eliminar el registro {categoria.Nombre}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
