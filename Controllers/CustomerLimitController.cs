using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerLimitController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public CustomerLimitController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("{customerId}")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerLimitDTO>> Get(Guid customerId)
        {
            try 
            {
                var data = await context.CustomerLimits.FirstOrDefaultAsync(x => x.CustomerId == customerId);

                if (data == null)
                {
                    return NotFound();
                }
                return mapper.Map<CustomerLimitDTO>(data);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
