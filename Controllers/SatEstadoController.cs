using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
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
    public class SatEstadoController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public SatEstadoController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle=servicioBinnacle;
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("sinpag")]
        public async Task<IEnumerable<SatEstadoDTO>> Get()
        {
            var queryable = context.SatEstados.AsQueryable();

            var list = await queryable
                .AsNoTracking().ToListAsync();
            return mapper.Map<List<SatEstadoDTO>>(list);
        }
    }
}
