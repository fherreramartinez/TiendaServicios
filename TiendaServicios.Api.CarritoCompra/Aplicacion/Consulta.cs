using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Dto;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }
        }

        public class manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            public readonly CarritoContexto _contexto;
            public readonly ILibrosService _libroService;

            public manejador(CarritoContexto contexto, ILibrosService libroService)
            {
                _contexto = contexto;
                _libroService = libroService;
            }


            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = await _contexto.CarritoSesion.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
                var carritoSesionDetalle = await _contexto.CarritoSesionDetalle.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                var listaCarritoDto = new List<CarritoDetalleDto>();

                foreach(var libro in carritoSesionDetalle)
                {
                    var response = await _libroService.GetLibro(new Guid(libro.ProductoSeleccionado));
                    if (response.resultado)
                    {
                        var objetoLibro = response.Libro;
                        var carritoDetalle = new CarritoDetalleDto
                        {
                            TituloLibro = objetoLibro.Titulo,
                            FechaPublicacion = objetoLibro.FechaPublicacion,
                             LibroId = objetoLibro.LibreriaMaterialId
                        };

                        listaCarritoDto.Add(carritoDetalle);

                    }
                }

                var carritoSesionDto = new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProducto = listaCarritoDto
                };

                return carritoSesionDto;

            }
        }

    }
}
