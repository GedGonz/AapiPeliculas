using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AapiPeliculas.Models;
using AapiPeliculas.Models.DTOS;
using AapiPeliculas.Repositorios.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AapiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        public UsuarioController(IUsuarioRepositorio _usuarioRepositorio, IMapper mapper, IConfiguration configuration)
        {
            this._usuarioRepositorio = _usuarioRepositorio;
            this.mapper = mapper;
            this.configuration = configuration;
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

        [HttpPost("Login")]
        public IActionResult Login(UsuarioAuthLogindto usuarioAuthdLogindto)
        {

            var usuarioLogin = _usuarioRepositorio.Login(usuarioAuthdLogindto.Usuario, usuarioAuthdLogindto.Passwrod);

            if (usuarioLogin==null) 
            {
                return Unauthorized();
            }

            var claims = new[] 
            {
                new Claim(ClaimTypes.NameIdentifier,usuarioLogin.Id.ToString()),
                new Claim(ClaimTypes.Name,usuarioLogin.UsuarioA.ToString())
            };

            //Gereracion de Token

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var credenciales = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credenciales
            };


            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokenDescriptor);

            return Ok(new 
            {
                token = tokenhandler.WriteToken(token)
            });

        }
    }
}
