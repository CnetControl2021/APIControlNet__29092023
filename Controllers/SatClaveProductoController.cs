using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SatClaveProductoController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public SatClaveProductoController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("buscar/{textoBusqueda}")]
        ////[AllowAnonymous]
        public async Task<ActionResult<List<SatClaveProductoServicioDTO>>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda)) { return new List<SatClaveProductoServicioDTO>(); }
            else
            {
                textoBusqueda = textoBusqueda.ToLower();
                var RSs = await context.SatClaveProductoServicios.Where(storedb => storedb.Descripcion.ToLower().Contains(textoBusqueda)
                | storedb.SatClaveProductoServicioId.ToLower().Contains(textoBusqueda))
                    .AsNoTracking()
                    .ToListAsync();
                return mapper.Map<List<SatClaveProductoServicioDTO>>(RSs);
            }
        }
    }
}
