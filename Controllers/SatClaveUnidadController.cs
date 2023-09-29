using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
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
    public class SatClaveUnidadController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public SatClaveUnidadController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("buscar/{textoBusqueda}")]
        ////[AllowAnonymous]
        public async Task<ActionResult<List<SatClaveUnidadDTO>>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda)) { return new List<SatClaveUnidadDTO>(); }
            else
            {
                textoBusqueda = textoBusqueda.ToLower();
                var RSs = await context.SatClaveUnidads.Where(storedb => storedb.Nombre.ToLower().Contains(textoBusqueda)
                | storedb.SatClaveUnidadId.ToLower().Contains(textoBusqueda))
                    .AsNoTracking()
                    .ToListAsync();
                return mapper.Map<List<SatClaveUnidadDTO>>(RSs);
            }
            
        }
    }
}
