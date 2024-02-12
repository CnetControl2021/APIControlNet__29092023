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
    public class VehicleController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public VehicleController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle=servicioBinnacle;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<VehicleDTO>>> Get(int skip, int take, Guid customerId, string searchTerm = "")
        {
            var data = await (from v in context.Vehicles
                              where v.CustomerId == customerId
                              //join c in context.Customers on cc.CustomerId equals c.CustomerId
                              select new VehicleDTO
                              {
                                  VehicleIdx = v.VehicleIdx,
                                  CustomerId = v.CustomerId,
                                  Name = v.Name,
                                  VehicleNumber = v.VehicleNumber,
                                  Plate = v.Plate
                              }).AsNoTracking().ToListAsync();

            var query = data.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.VehicleNumber.ToLower().Contains(searchTerm)
                || c.Plate.ToLower().Contains(searchTerm) || c.Name.ToLower().Contains(searchTerm));
            }

            var ntotal = query.Count();
            return Ok(new { query, ntotal });
        }

        [HttpGet("ValidateType")]
        [AllowAnonymous]
        public async Task<IEnumerable<ValidateTypeDTO>> Get4()
        {
            var data = await context.ValidateTypes.ToListAsync();
            return mapper.Map<IEnumerable<ValidateTypeDTO>>(data);
        }


        //[HttpGet]
        ////[AllowAnonymous]
        //public async Task<ActionResult<VehicleDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid idGuid)
        //{
        //    var queryable = context.Vehicles.AsQueryable();
        //    if (!string.IsNullOrEmpty(nombre) || idGuid != Guid.Empty)
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) || x.CustomerId.Equals(idGuid));
        //    }
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var customers = await queryable.OrderByDescending(x => x.VehicleIdx).Paginar(paginacionDTO)
        //        //.Include(x => x.Customer)
        //        .AsNoTracking().ToListAsync();
        //    return Ok(customers);
        //}


        //[HttpGet("active")]
        //public async Task<ActionResult<VehicleDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid idGuid)
        //{
        //    var queryable = context.Vehicles.Where(x => x.Active == true).AsQueryable();
        //    if (!string.IsNullOrEmpty(nombre) || idGuid != Guid.Empty)
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) || x.CustomerId.Equals(idGuid));
        //    }
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var customers = await queryable.OrderByDescending(x => x.VehicleIdx).Paginar(paginacionDTO)
        //        //.Include(x => x.Customer)
        //        .AsNoTracking().ToListAsync();
        //    return Ok(customers);
        //}

        [HttpGet("sinPag/{nombre}")]
        //[AllowAnonymous]
        public async Task<ActionResult<VehicleDTO>> Get(string nombre, [FromQuery] Guid idGuid)
        {
            var queryable = context.Vehicles.AsQueryable();
            if (!string.IsNullOrEmpty(nombre) || idGuid != Guid.Empty)
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) || x.CustomerId.Equals(idGuid));
            }
            var customers = await queryable
                .AsNoTracking().ToListAsync();
            return Ok(customers);
        }


        [HttpGet("{id:int}", Name = "obtenerVehiculo")]
        //[AllowAnonymous]
        public async Task<ActionResult<VehicleDTO>> Get(int id)
        {
            var vehicle = await context.Vehicles.FirstOrDefaultAsync(x => x.VehicleIdx == id);

            if (vehicle == null)
            {
                return NotFound();
            }
            return mapper.Map<VehicleDTO>(vehicle);
        }


        [HttpGet("{nombre}")]
        
        public async Task<ActionResult<List<VehicleDTO>>> Get([FromRoute] string nombre)
        {
            var RSs = await context.Vehicles.Where(VehicleDB => VehicleDB.Name.Contains(nombre)).ToListAsync();

            return mapper.Map<List<VehicleDTO>>(RSs);
        }

        [HttpGet("byCustomer/{id2}")] //por store
        //[AllowAnonymous]
        public async Task<ActionResult<List<VehicleDTO>>> Get([FromRoute] Guid id2)
        {
            var RSs = await context.Vehicles.Where(e => e.CustomerId.Equals((Guid)id2))
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<VehicleDTO>>(RSs);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VehicleDTO vehicleDTO, Guid customerId, Guid storeId)
        {
            var existe = await context.Vehicles.AnyAsync
                (x => x.VehicleId == vehicleDTO.VehicleId && x.CustomerId == vehicleDTO.CustomerId);

            var dbveh = await context.Customers.FirstOrDefaultAsync(x => x.CustomerId == vehicleDTO.CustomerId);

            var vehicle = mapper.Map<Vehicle>(vehicleDTO);
            vehicle.CustomerId = customerId;

            if (existe)
            {
                return BadRequest($"Ya existe {dbveh.Name} ");
            }
            else
            {
                context.Add(vehicle);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = vehicleDTO.Name;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);

                await context.SaveChangesAsync();
                return Ok();
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put(VehicleDTO vehicleDTO)
        {
            var vehicleDB = await context.Vehicles.FirstOrDefaultAsync(c => c.VehicleIdx == vehicleDTO.VehicleIdx);

            if (vehicleDB is null)
            {
                return NotFound();
            }
            try
            {
                vehicleDB = mapper.Map(vehicleDTO, vehicleDB);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = vehicleDB.Name;
                //await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe vehiculo {vehicleDTO.VehicleNumber} ");
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            var existe = await context.Vehicles.AnyAsync(x => x.VehicleIdx == id);
            var tabla = context.Model.FindEntityType(typeof(Netgroup)).GetTableName();
            if (!existe) { return NotFound(); }

            var name2 = await context.Vehicles.FirstOrDefaultAsync(x => x.VehicleIdx == id);
            context.Remove(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Name;
            var storeId2 = storeId;
            var Table = tabla;
            await servicioBinnacle.deleteBinnacle2(usuarioId, ipUser, name, storeId2, tabla);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
