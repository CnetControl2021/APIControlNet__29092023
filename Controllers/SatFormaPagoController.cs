using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SatFormaPagoController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public SatFormaPagoController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet]
        public async Task<ActionResult<SatFormaPagoDTO>> Get()
        {
            var queryable = context.SatFormaPagos.AsQueryable();
            var formaPago = await queryable
                .AsNoTracking().ToListAsync();
            return Ok(formaPago);
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<ActionResult<CustomerDTO>> Get2()
        {
            var queryable = context.SatFormaPagos.Where(x => x.Active == true).AsQueryable();
            var formaPago = await queryable
                .AsNoTracking().ToListAsync();
            return Ok(formaPago);
        }
    }
}
