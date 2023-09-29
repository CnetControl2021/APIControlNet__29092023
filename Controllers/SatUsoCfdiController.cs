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
    public class SatUsoCfdiController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public SatUsoCfdiController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet]
        public async Task<ActionResult<SatUsoCfdiDTO>> Get()
        {
            var queryable = context.SatUsoCfdis.AsQueryable();
            var regimenfiscal = await queryable
                .AsNoTracking().ToListAsync();
            return Ok(regimenfiscal);
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<ActionResult<CustomerDTO>> Get2()
        {
            var queryable = context.SatUsoCfdis.Where(x => x.Active == true).AsQueryable();
            var regimenfiscal = await queryable
                .AsNoTracking().ToListAsync();
            return Ok(regimenfiscal);
        }
    }
}
