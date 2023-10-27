using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public CustomerController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle=servicioBinnacle;
        }


        [HttpGet]
        public async Task<ActionResult<CustomerDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Customers.OrderByDescending(x => x.CustomerIdx).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) || x.CustomerNumber.ToString().ToLower().Contains(nombre));
            }            
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var customers = await queryable.Paginar(paginacionDTO)
                .Include(x => x.CustomerType)
                .OrderByDescending(x => x.CustomerIdx)
                .AsNoTracking().ToListAsync();
            return Ok(customers);
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<ActionResult<CustomerDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Customers.Where(x => x.Active == true).OrderByDescending(x => x.CustomerIdx).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) || x.CustomerNumber.ToString().ToLower().Contains(nombre));
            }
            
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var customers = await queryable.Paginar(paginacionDTO)
                .Include(x => x.CustomerType)      
                .AsNoTracking().ToListAsync();
            return Ok(customers);
        }


        //[HttpGet]
        ////[AllowAnonymous]
        //public async Task<ActionResult<CustomerDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        //{
        //    var queryable = context.Customers.AsQueryable();
        //    if (!string.IsNullOrEmpty(nombre))
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) | x.Rfc.ToLower().Contains(nombre) | x.CustomerNumber.ToString().ToLower().Contains(nombre));
        //    }
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var customers = await queryable.AsNoTracking().OrderByDescending(x => x.CustomerIdx)
        //        .Select(x => new
        //        {
        //            x.CustomerIdx,
        //            x.Name,
        //            x.CustomerNumber,
        //            CustomerType = x.CustomerType.Name,
        //            PaymentMode = x.PaymentMode.Name,
        //            x.Rfc
        //        }).Paginar(paginacionDTO).AsNoTracking()
        //        .ToListAsync();
        //    return Ok(customers);
        //}


        [HttpGet("{id:int}", Name = "obtenerCustomer")]
        public async Task<ActionResult<CustomerDTO>> Get(int id)
        {
            var Customer = await context.Customers.FirstOrDefaultAsync(x => x.CustomerIdx == id);

            if (Customer == null)
            {
                return NotFound();
            }

            return mapper.Map<CustomerDTO>(Customer);
        }

        [HttpGet("byGuid/{idPass:guid}", Name = "obtenerCustomer2")]
        //[AllowAnonymous]
        public async Task<ActionResult<CustomerDTO>> Get(Guid idPass)
        {
            var Customer = await context.Customers.FirstOrDefaultAsync(x => x.CustomerId == idPass);

            if (Customer == null)
            {
                return NotFound();
            }

            return mapper.Map<CustomerDTO>(Customer);
        }


        [HttpGet("byName/{textSearch}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CustomerDTO>>> Get(string textSearch)
        {
            var queryable = context.Customers.OrderByDescending(x => x.CustomerIdx).AsQueryable();
            if (!string.IsNullOrEmpty(textSearch))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(textSearch.ToLower()));
            }
            var customers = await queryable
               .AsNoTracking().ToListAsync();
            return mapper.Map<List<CustomerDTO>> (customers);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CustomerDTO CustomerDTO)
        {

            var existeid = await context.Customers.AnyAsync(x => x.CustomerIdx == CustomerDTO.CustomerIdx);

            var Customer = mapper.Map<Customer>(CustomerDTO);
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = Customer.Name;

            if (existeid)
            {
                context.Update(Customer);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.Customers.AnyAsync(x => x.CustomerNumber == (CustomerDTO.CustomerNumber) | x.Name == CustomerDTO.Name);

                if (existe)
                {
                    return BadRequest($"Ya existe {CustomerDTO.CustomerNumber} {CustomerDTO.Name} ");
                }
                else
                {
                    context.Add(Customer);
                    //await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name);
                    await context.SaveChangesAsync();
                }
            }
            var CustomerDTO2 = mapper.Map<CustomerDTO>(Customer);
            return CreatedAtRoute("obtenerCustomer", new { id = Customer.CustomerIdx }, CustomerDTO2);
        }


        [HttpPut]
        public async Task<IActionResult> Put(CustomerDTO customerDTO)
        {
            var customerDB = await context.Customers.FirstOrDefaultAsync(c => c.CustomerIdx == customerDTO.CustomerIdx);

            if (customerDB is null)
            {
                return NotFound();
            }
            try
            {
                customerDB = mapper.Map(customerDTO, customerDB);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = customerDB.Name;
                //await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe cliente {customerDTO.CustomerNumber} en esa sucursal ");
            }
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Customers.AnyAsync(x => x.CustomerIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Customers.FirstOrDefaultAsync(x => x.CustomerIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Name;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            try
            {
                var existe = await context.Customers.AnyAsync(x => x.CustomerIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.Customers.FirstOrDefaultAsync(x => x.CustomerIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.Name;
                var storeId2 = storeId;
                await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2);

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return BadRequest("ERROR DE DATOS RELACIONADOS");
            }
        }
    }
}
