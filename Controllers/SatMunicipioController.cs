using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SatMunicipioController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public SatMunicipioController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("buscar/{textoBusqueda}")]
        //[AllowAnonymous]
        public async Task<ActionResult<List<SatMunicipioDTO>>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda)) { return new List<SatMunicipioDTO>(); }
            else
            {
                textoBusqueda = textoBusqueda.ToLower();
                var RSs = await context.SatMunicipios.Where(x => x.Descripcion.ToLower().Contains(textoBusqueda)).Take(15)
                    .AsNoTracking()
                    .ToListAsync();
                return mapper.Map<List<SatMunicipioDTO>>(RSs);
            }
        }

    }
}
