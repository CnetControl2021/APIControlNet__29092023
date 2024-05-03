using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompanyCustomerController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public CompanyCustomerController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CompanyCustomerDTO>>> Get(int skip, int take, Guid companyId, string searchTerm = "")
        {
            var data = await (from cc in context.CompanyCustomers
                              join c in context.Customers on cc.CustomerId equals c.CustomerId
                              select new CompanyCustomerDTO
                              {
                                  CompanyCustomerIdx = cc.CompanyCustomerIdx,
                                  CompanyId = cc.CustomerId,
                                  CustomerId = cc.CustomerId,
                                  CustomerName = c.Name,
                                  CustomerRFC = c.Rfc,
                                  CustomerEmail = c.Email
                              }).AsNoTracking().ToListAsync();

            var query = data.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.CustomerName.ToLower().Contains(searchTerm));
            }

            var ntotal = query.Count();
            return Ok(new { query, ntotal });
        }

        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(int id, Guid? storeId)
        {
            var existe = await context.CompanyCustomers.FirstOrDefaultAsync(x => x.CompanyCustomerIdx == id);

            if (existe is null) { return NotFound(); }

            var name2 = await context.Customers.FirstOrDefaultAsync(x => x.CustomerId == existe.CustomerId);
            context.Remove(existe);
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Name;
            var storeId2 = storeId;
            var tabla = "Customers";
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2, tabla);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
