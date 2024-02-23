using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;

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

        [HttpPut]
        public async Task<IActionResult> Put(CustomerLimitDTO customerLimitDTO, Guid storeId)
        {
            var db = await context.CustomerLimits.FirstOrDefaultAsync(x => x.CustomerId == customerLimitDTO.CustomerId);
            var tabla = context.Model.FindEntityType(typeof(CustomerLimit)).GetTableName();

            if (db is null)
            {
                return NotFound();
            }
            try
            {
                db = mapper.Map(customerLimitDTO, db);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = customerLimitDTO.AmountCreditLimit.ToString();
                var storeId2 = storeId;
                var Table = tabla;
                await servicioBinnacle.EditBinnacle2(usuarioId, ipUser, name, storeId2, tabla);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe {db.CustomerId} ");
            }
            return NoContent();
        }


    }
}
