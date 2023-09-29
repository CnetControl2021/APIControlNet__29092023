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
    public class CustomerAddressController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public CustomerAddressController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet]
        public async Task<ActionResult<CustomerAddressDTO>> Get()
        {
            var queryable = context.CustomerAddresses.AsQueryable();
            var regimenfiscal = await queryable
                .AsNoTracking().ToListAsync();
            return Ok(regimenfiscal);
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<ActionResult<CustomerAddressDTO>> Get2()
        {
            var queryable = context.CustomerAddresses.Where(x => x.Active == true).AsQueryable();
            var regimenfiscal = await queryable
                .AsNoTracking().ToListAsync();
            return Ok(regimenfiscal);
        }

    }
}
