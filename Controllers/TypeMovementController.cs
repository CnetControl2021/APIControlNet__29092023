using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeMovementController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public TypeMovementController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<TypeMovementDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.TypeMovements.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var typeMovements = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<TypeMovementDTO>>(typeMovements);
        }
    }
}
