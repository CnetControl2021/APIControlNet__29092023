using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


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

        //[HttpGet]
        //public async Task<ActionResult<CustomerDTO>> Get([FromQuery] string nombre)
        //{
        //    var queryable = context.Customers.OrderByDescending(x => x.CustomerIdx).AsQueryable();
        //    if (!string.IsNullOrEmpty(nombre))
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) || x.CustomerNumber.ToString().ToLower().Contains(nombre));
        //    }
        //    //await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var customers = await queryable
        //        .Include(x => x.CustomerType)
        //        .OrderByDescending(x => x.CustomerIdx)
        //        .AsNoTracking().ToListAsync();
        //    return Ok(customers);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult GetData(int pageNumber = 1, int pageSize = 10)
        //{
        //    var totalItems = context.Customers.Count();
        //    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        //    var items = context.Customers
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    return Ok(new { Items = items, TotalPages = totalPages });
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetItems(int pageIndex = 0, int pageSize = 10)
        //{
        //    try
        //    {
        //        // Calcular la posición de inicio
        //        int skip = pageIndex * pageSize;

        //        // Consultar la base de datos para obtener solo la página específica
        //        var items = await context.Customers
        //            .OrderBy(i => i.CustomerIdx)
        //            .Skip(skip)
        //            .Take(pageSize)
        //            .ToListAsync();

        //        return Ok(items);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetItems(int pageNumber = 1, int pageSize = 10)
        //{
        //    //var Customer = await context.Customers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        //    var queryable = context.Customers.Skip((pageNumber -1) * pageSize).Take(pageSize);
        //    //var totalItems = context.Customers.Count();
        //    //return Ok(items);


        //    var customers = await queryable.ToListAsync();

        //    return mapper.Map<CustomerDTO>(customers);
        //}

        ////Para Virtualize
        //[HttpGet("total")]
        //[AllowAnonymous]
        //public async Task<ActionResult<int>> TotalItems()
        //{
        //    var total = await context.Customers.CountAsync();
        //    return Ok(total);
        //}


        ////Para Virtualize
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IEnumerable<CustomerDTO>> GetItems([FromQuery] string nombre, int pageNumber = 1, int pageSize = 10)
        //{
        //    var queryable = context.Customers.Where(x => x.Active == true && x.Deleted == false).AsQueryable();

        //    if (!string.IsNullOrEmpty(nombre))
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
        //    }
        //    var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize)
        //        .ToListAsync();
        //    return mapper.Map<List<CustomerDTO>>(items);
        //}

        //private readonly List<Cliente> _clientes = ObtenerClientes(); // Implementa ObtenerClientes según tus necesidades

        //[HttpGet] //okok
        //[AllowAnonymous]
        //public async Task<ActionResult> GetClientes([FromQuery] int skip, [FromQuery] int take, string searchTerm = "")
        //{
        //    var clientes = await context.Customers.Skip(skip).Take(take).AsNoTracking().ToListAsync();
        //    var totalClientes = await context.Customers.CountAsync();
        //    return Ok(new { Total = totalClientes, Clientes = clientes });
        //    ////chema
        //    //var claseEmpaquetada = new ApiResponse
        //    //{
        //    //    Clientes = mapper.Map<List<CustomerDTO>>(clientes),
        //    //    Total = totalClientes
        //    //};
        //    //return (IEnumerable<CustomerDTO>)Ok(claseEmpaquetada);
        //}


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> GetClientes(int skip, int take, string searchTerm = "")
        {
            try
            {
                var query = context.Customers.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(c => c.Name.ToLower().Contains(searchTerm) || c.CustomerNumber.ToString().ToLower().Contains(searchTerm));
                }

                var nclientes = await query.Skip(skip).Take(take).ToListAsync();
                var ntotal = await query.CountAsync();

                return new ApiResponse
                {
                    NTotal = ntotal,
                    NClientes = mapper.Map<IEnumerable<CustomerDTO>>(nclientes)
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ActionResult<CustomerDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
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
        //            //PaymentMode = x.PaymentMode.Name,
        //            x.Rfc
        //        }).Paginar(paginacionDTO).AsNoTracking()
        //        .ToListAsync();
        //    return Ok(customers);
        //}


        //[HttpGet]
        ////[AllowAnonymous]
        //public async Task<IEnumerable<CustomerDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        //{
        //    var queryable = context.Customers.OrderByDescending(x => x.CustomerIdx).AsQueryable();
        //    if (!string.IsNullOrEmpty(nombre))
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) || x.CustomerNumber.ToString().ToLower().Contains(nombre));
        //    }
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var customers = await queryable.Paginar(paginacionDTO)
        //        .Include(x => x.CustomerType)
        //        .OrderByDescending(x => x.CustomerIdx)
        //        .AsNoTracking()
        //        .ToListAsync();
        //    return mapper.Map<List<CustomerDTO>>(customers);
        //}


        //[HttpGet("active")]
        ////[AllowAnonymous]
        //public async Task<IEnumerable<CustomerDTO>> Get4([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        //{
        //    var queryable = context.Customers.Where(x => x.Active == true).OrderByDescending(x => x.CustomerIdx).AsQueryable();
        //    if (!string.IsNullOrEmpty(nombre))
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) || x.CustomerNumber.ToString().ToLower().Contains(nombre));
        //    }

        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var customers = await queryable.Paginar(paginacionDTO)
        //        .Include(x => x.CustomerType)
        //        .AsNoTracking().ToListAsync();
        //    return mapper.Map<List<CustomerDTO>>(customers);
        //}


        [HttpGet("{id:int?}", Name = "obtenerCustomer")]
        public async Task<ActionResult<CustomerDTO>> Get3(int id)
        {
            var Customer = await context.Customers.FirstOrDefaultAsync
                (x => x.CustomerIdx == id);

            if (Customer == null)
            {
                return NotFound();
            }

            return mapper.Map<CustomerDTO>(Customer);
        }

        [HttpGet("byCustomerId/{custommerId}", Name = "byGui")]
        //[AllowAnonymous]
        public async Task<ActionResult<CustomerDTO>> Get4(Guid custommerId)
        {
            var Customer = await context.Customers.FirstOrDefaultAsync
                (x => x.CustomerId == custommerId);

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


        [HttpGet("byName/{textSearch}")] // BlazoredTypeahead
        [AllowAnonymous]
        public async Task<ActionResult<List<CustomerDTO>>> Get2(string textSearch)
        {
            var queryable = context.Customers.OrderByDescending(x => x.CustomerIdx).AsQueryable();
            if (!string.IsNullOrEmpty(textSearch))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(textSearch) || x.Rfc.ToLower().Contains(textSearch));

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
