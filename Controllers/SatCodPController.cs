using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SatCodPController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public SatCodPController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("buscar/{textoBusqueda}")]
        //[AllowAnonymous]
        public async Task<ActionResult<List<SatCodigoPostalDTO>>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda)) { return new List<SatCodigoPostalDTO>(); }
            else
            {
                textoBusqueda = textoBusqueda.ToLower();
                var RSs = await context.SatCodigoPostals.Where(x => x.SatCodigoPostalId.ToLower().Contains(textoBusqueda)).Take(15)
                    .AsNoTracking()
                    .ToListAsync();
                return mapper.Map<List<SatCodigoPostalDTO>>(RSs);
            }
        }
    }
}
