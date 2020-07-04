using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AapiPeliculas.Models;
using AapiPeliculas.Models.DTOS;
using AapiPeliculas.Repositorios.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AapiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IMapper mapper;
        public UsuarioController(IUsuarioRepositorio _usuarioRepositorio, IMapper mapper)
        {
            this._usuarioRepositorio = _usuarioRepositorio;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _usuarioRepositorio.GetUsuarios();

            var listaUsuariosdto = mapper.Map<List<Usuariodto>>(listaUsuarios);

            return Ok(listaUsuariosdto);
        }

        [HttpGet("{IdUsuario:int}", Name = "GetUsuario")]
        public IActionResult GetUsuario(int IdUsuario)
        {
            var usuario = _usuarioRepositorio.GetUsuario(IdUsuario);

            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioDTO = mapper.Map<Usuariodto>(usuario);

            return Ok(usuarioDTO);
        }

        [HttpPost("Registro")]
        public IActionResult Registro(UsuarioAuthdto usuarioAuthdto) 
        {

            if (_usuarioRepositorio.ExistUsuario(usuarioAuthdto.Usuario.ToLower())) 
            {
                return BadRequest("El usuario ya existe");
            }

            var usuarioACrear = mapper.Map<Usuario>(usuarioAuthdto);

            var usuarioCreado = _usuarioRepositorio.Registro(usuarioACrear, usuarioAuthdto.Passwrod);

            return Ok(usuarioCreado);
        }
    }
}
