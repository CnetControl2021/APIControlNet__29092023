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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompanyCustomerController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public CompanyCustomerController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
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
            return Ok(new { query, ntotal}); 
        }


        [HttpGet("GetOK")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiRespSupplierTransportDTO>> Get5(int skip, int take, Guid storeId, string searchTerm = "")
        {
            var query = context.SupplierTransports.Where(x => x.StoreId == storeId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.ToLower().Contains(searchTerm) || c.Rfc.ToLower().Contains(searchTerm));
            }

            var nsupplierT = await query.Skip(skip).Take(take).OrderByDescending(x => x.Date).ToListAsync();
            var ntotal = await query.CountAsync();

            return new ApiRespSupplierTransportDTO
            {
                NTotal = ntotal,
                NSupplierTransportDTOs = mapper.Map<IEnumerable<SupplierTransportDTO>>(nsupplierT)
            };
        }


        [HttpGet("{id:int}/{idGuid}", Name = "newCompCust")]
        public async Task<ActionResult<CompanyCustomerDTO>> Get(int id, Guid idGuid)
        {
            var compCust = await context.CompanyCustomers.FirstOrDefaultAsync(x => x.CompanyCustomerIdx == id);
            var dbcustomer = await context.Customers.FirstOrDefaultAsync(x => x.CustomerId == compCust.CustomerId);

            if (compCust == null)
            {
                return NotFound();
            }

            var customer = mapper.Map<CompanyCustomerDTO>(compCust);
            customer.CustomerName = dbcustomer.Name;

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CompanyCustomerDTO compCustDTO, Guid storeId)
        {
            var existe = await context.CompanyCustomers.AnyAsync
                (x => x.CompanyId == compCustDTO.CompanyId && x.CustomerId == compCustDTO.CustomerId);
            var dbcustomer = await context.Customers.FirstOrDefaultAsync(x => x.CustomerId == compCustDTO.CustomerId);

            var compCust = mapper.Map<CompanyCustomer>(compCustDTO);
            //suppTransp.StoreId = storeId;
            //suppTransp.Name = dbSupplier.Name;
            //suppTransp.Rfc = dbSupplier.Rfc;

            if (existe)
            {
                return BadRequest($"Ya existe {dbcustomer.Name} en cliente en empresa");
            }
            else
            {
                context.Add(compCust);

                var dbCl = await context.CustomerLimits.FirstOrDefaultAsync(x => x.CustomerId == compCustDTO.CustomerId);
                try
                {
                    if (dbCl is null)
                    {
                        CustomerLimit cl = new CustomerLimit();
                        cl.CustomerId = compCust.CustomerId;
                        cl.AmountCreditLimit = 0;
                        cl.CreditDays = 0;
                        cl.Date = DateTime.Now;
                        cl.Updated = DateTime.Now;
                        cl.Active = true;
                        cl.Locked = false; cl.Deleted = false; cl.FolioOdrNumber = 0;
                        context.Add(cl);

                        var usuarioId = obtenerUsuarioId();
                        var ipUser = obtenetIP();
                        var name = dbcustomer.Name;
                        var storeId2 = storeId;
                        await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);

                        await context.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {

                        dbCl.CustomerId = compCust.CustomerId;
                        dbCl.AmountCreditLimit = 0;
                        dbCl.CreditDays = 0;
                        dbCl.FolioOdrNumber = 0;
                        //dbCl.Date = DateTime.Now;
                        dbCl.Updated = DateTime.Now;
                        dbCl.Active = true;
                        dbCl.Locked = false; dbCl.Deleted = false;
                        context.Update(dbCl);

                        var usuarioId = obtenerUsuarioId();
                        var ipUser = obtenetIP();
                        var name = dbcustomer.Name;
                        var storeId2 = storeId;
                        await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);

                        await context.SaveChangesAsync();
                        return Ok();
                    }
                }
                catch (Exception ex) 
                {
                    return BadRequest(ex.Message + "Seleccione cliente");
                }                     
            }
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
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
