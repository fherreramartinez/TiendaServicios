using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Dto;
using TiendaServicios.Api.Autor.Modelo;
using Swashbuckle.AspNetCore.Swagger;

namespace TiendaServicios.Api.Autor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutorController(IMediator mediator)
        {
            _mediator = mediator;


        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        /// <summary>
        ///El método obtiene autores.        
        /// </summary>        
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>         
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]       
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            return await _mediator.Send(new Consulta.ListaAutor());    
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<AutorDto>> GetAutorLibro(string id)
        {
            return await _mediator.Send(new ConsultaFiltro.AutorUnico{ AutorLibroGuid = id  });
        }

    }
}
