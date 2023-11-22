using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SupplierFuelController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public SupplierFuelController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<SupplierFuelDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, 
            Guid storeId, Guid id2)
        {
            var queryable = context.SupplierFuels.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            if (id2 != Guid.Empty)
            {  
                queryable = queryable.Where(x => x.SupplierId == id2);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var suppliersFuel = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
            return mapper.Map<List<SupplierFuelDTO>>(suppliersFuel);
        }


        [HttpGet("{id:int}", Name = "newSuppFuel")]
        public async Task<ActionResult<SupplierFuelDTO>> Get(int id)
        {
            var suppliersFuel = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierFuelIdx == id);

            if (suppliersFuel == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierFuelDTO>(suppliersFuel);
        }


        [HttpGet("byguid/{id2?}/{id?}")]
        //[AllowAnonymous]
        public async Task<ActionResult<SupplierFuelDTO>> Get(Guid id2, int id)
        {
            var suppliersFuel = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierId == id2 & x.SupplierFuelIdi == id);

            if (suppliersFuel == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierFuelDTO>(suppliersFuel);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SupplierFuelDTO SupplierFuelDTO, Guid id2, Guid storeId)
        {
            var existe = await context.SupplierFuels.AnyAsync(x => x.SupplierId == SupplierFuelDTO.SupplierId && SupplierFuelDTO.StoreId == storeId);

            var suppFuel = mapper.Map<SupplierFuel>(SupplierFuelDTO);
            suppFuel.SupplierId = id2;
            suppFuel.StoreId = storeId;

            if (existe)
            {
                return BadRequest($"Ya existe {SupplierFuelDTO.SupplierFuelIdi} y {SupplierFuelDTO.Name} ");
            }
            else
            {
                context.Add(suppFuel);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = suppFuel.Name;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
                var storeAddressDTO2 = mapper.Map<SupplierFuelDTO>(suppFuel);
                return CreatedAtRoute("newSuppFuel", new { id = SupplierFuelDTO.SupplierFuelIdx }, storeAddressDTO2);

            }
        }


        [HttpPut]
        public async Task<IActionResult> Put(SupplierFuelDTO SupplierFuelDTO)
        {
            var supFuelDB = await context.SupplierFuels.FirstOrDefaultAsync(c => c.SupplierFuelIdx == SupplierFuelDTO.SupplierFuelIdx);
            if (supFuelDB is null)
            {
                return NotFound();
            }
            supFuelDB = mapper.Map(SupplierFuelDTO, supFuelDB);

            var storeId2 = supFuelDB.StoreId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = supFuelDB.Name;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.SupplierFuels.AnyAsync(x => x.SupplierFuelIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierFuelIdx == id);
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
        public async Task<IActionResult> Delete(int id, Guid? storeId)
        {
            var existe = await context.SupplierFuels.AnyAsync(x => x.SupplierFuelIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierFuels.FirstOrDefaultAsync(x => x.SupplierFuelIdx == id);
            context.Remove(name2);
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
