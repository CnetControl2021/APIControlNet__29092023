using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NetgroupCustomerController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public NetgroupCustomerController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NetgroupCustomerDTO>>> Get(int skip, int take, Guid companyId, string user, string searchTerm = "")
        {

            var dbngu = await context.NetgroupUsers!.FirstOrDefaultAsync(x => x.Name == user);

            if (dbngu is null)
            {
                var data = await (from cc in context.NetgroupCustomers
                                  join c in context.Customers on cc.CustomerId equals c.CustomerId
                                  select new NetgroupCustomerDTO
                                  {
                                      NetgroupCustomerIdx = cc.NetgroupCustomerIdx,
                                      //CompanyId = cc.CustomerId,
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
            else if (dbngu.NetgroupUserTypeId != 3 && dbngu is not null)
            {
                var data = await (from cc in context.NetgroupCustomers
                                  join c in context.Customers on cc.CustomerId equals c.CustomerId
                                  select new NetgroupCustomerDTO
                                  {
                                      NetgroupCustomerIdx = cc.NetgroupCustomerIdx,
                                      //CompanyId = cc.CustomerId,
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
            else
            {
                var custId = dbngu.CustomerId;

                var data = await (from cc in context.NetgroupCustomers
                                  where cc.CustomerId == custId
                                  join c in context.Customers on cc.CustomerId equals c.CustomerId
                                  select new NetgroupCustomerDTO
                                  {
                                      NetgroupCustomerIdx = cc.NetgroupCustomerIdx,
                                      //CompanyId = cc.CustomerId,
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
        public async Task<ActionResult<NetgroupCustomerDTO>> Get(int id, Guid idGuid)
        {
            var compCust = await context.NetgroupCustomers.FirstOrDefaultAsync(x => x.NetgroupCustomerIdx == id);
            var dbcustomer = await context.Customers.FirstOrDefaultAsync(x => x.CustomerId == compCust.CustomerId);

            if (compCust == null)
            {
                return NotFound();
            }

            var customer = mapper.Map<NetgroupCustomerDTO>(compCust);
            customer.CustomerName = dbcustomer.Name;

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NetgroupCustomerDTO netgroupCustDTO, Guid netgroupId)
        {

            var dbNetGcustomer = await context.NetgroupCustomers.AnyAsync(x => x.CustomerId == netgroupCustDTO.CustomerId
            & x.NetgroupId == netgroupId);

            var netgroupCust = mapper.Map<NetgroupCustomer>(netgroupCustDTO);
            netgroupCust.NetgroupId = netgroupId;

            if (dbNetGcustomer)
            {
                return BadRequest($"Ya existe en cliente en esta red");
            }
            else
            {
                context.Add(netgroupCust);

                var dbCl = await context.CustomerLimits.FirstOrDefaultAsync(x => x.CustomerId == netgroupCustDTO.CustomerId);
                try
                {
                    if (dbCl is null)
                    {
                        CustomerLimit cl = new CustomerLimit();
                        cl.CustomerId = netgroupCust.CustomerId;
                        cl.AmountCreditLimit = 0;
                        cl.CreditDays = 0;
                        cl.Date = DateTime.Now;
                        cl.Updated = DateTime.Now;
                        cl.Active = true;
                        cl.Locked = false; cl.Deleted = false; cl.FolioOdrNumber = 0;
                        context.Add(cl);

                        await context.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {

                        dbCl.CustomerId = netgroupCust.CustomerId;
                        dbCl.AmountCreditLimit = 0;
                        dbCl.CreditDays = 0;
                        dbCl.FolioOdrNumber = 0;
                        //dbCl.Date = DateTime.Now;
                        dbCl.Updated = DateTime.Now;
                        dbCl.Active = true;
                        dbCl.Locked = false; dbCl.Deleted = false;
                        context.Update(dbCl);

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
            var existe = await context.NetgroupCustomers.FirstOrDefaultAsync(x => x.NetgroupCustomerIdx == id);
            if (existe is null) { return NotFound(); }

            var dbVehicCust = await context.Vehicles.FirstOrDefaultAsync(x => x.CustomerId == existe.CustomerId);
            if (dbVehicCust?.CustomerId != null) { return BadRequest("Vehiculo relacionado"); }
            context.Remove(existe);
            await context.SaveChangesAsync();
            return NoContent();
        }


        //[HttpGet("byName/{textSearch}")] // BlazoredTypeahead
        //[AllowAnonymous]
        //public async Task<ActionResult<List<NetgroupCustomerDTO>>> Get3(string textSearch, Guid netgroupId)
        //{

        //    var data = await (from ngc in context.NetgroupCustomers where ngc.NetgroupId == netgroupId
        //                      join c in context.Customers on ngc.CustomerId equals c.CustomerId
        //                      select new NetgroupCustomerDTO
        //                      {
        //                          NetgroupCustomerIdx = ngc.NetgroupCustomerIdx,
        //                          //CompanyId = cc.CustomerId,
        //                          CustomerId = ngc.CustomerId,
        //                          CustomerName = c.Name,
        //                          CustomerRFC = c.Rfc,
        //                          CustomerEmail = c.Email
        //                      }).AsNoTracking().ToListAsync();

        //    var queryable = data.AsQueryable();

        //    if (!string.IsNullOrEmpty(textSearch))
        //    {
        //        queryable = queryable.Where(x => x.CustomerName.ToLower().Contains(textSearch) );

        //    }
        //    var netgroupCus = await queryable
        //       .AsNoTracking().ToListAsync();
        //    return mapper.Map<List<NetgroupCustomerDTO>>(netgroupCus);
        //}

        [HttpGet("byName/{textSearch}")] // BlazoredTypeahead
        [AllowAnonymous]
        public async Task<ActionResult<List<NetgroupCustomerDTO>>> Get3(string textSearch, Guid netgroupId)
        {
            var queryable = from ngc in context.NetgroupCustomers
                            join c in context.Customers on ngc.CustomerId equals c.CustomerId
                            where ngc.NetgroupId == netgroupId
                            select new NetgroupCustomerDTO
                            {
                                NetgroupCustomerIdx = ngc.NetgroupCustomerIdx,
                                CustomerId = ngc.CustomerId,
                                CustomerName = c.Name,
                                CustomerRFC = c.Rfc,
                                CustomerEmail = c.Email
                            };

            if (!string.IsNullOrEmpty(textSearch))
            {
                queryable = queryable.Where(x => x.CustomerName.ToLower().Contains(textSearch.ToLower()));
            }

            var netgroupCus = await queryable.AsNoTracking().ToListAsync();
            return mapper.Map<List<NetgroupCustomerDTO>>(netgroupCus);
        }


    }
}
