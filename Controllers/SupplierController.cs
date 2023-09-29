using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
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
    public class SupplierController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public SupplierController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("sinPag")]
        //[AllowAnonymous]
        public async Task<IEnumerable<SupplierDTO>> Get2([FromQuery] string nombre)
        {
            var queryable = context.Suppliers.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            var suppliers = await queryable.AsNoTracking().ToListAsync();
            return mapper.Map<List<SupplierDTO>>(suppliers);
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<SupplierDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Suppliers.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            //if (storeId != Guid.Empty)
            //{
            //    queryable = queryable.Where(x => x.StoreId == storeId);
            //}
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var suppliers = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
            return mapper.Map<List<SupplierDTO>>(suppliers);
        }


        [HttpGet("{id:int}", Name = "obtenerSuppliers")]
        public async Task<ActionResult<SupplierDTO>> Get(int id)
        {
            var suppliers = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierIdx == id);

            if (suppliers == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierDTO>(suppliers);
        }

        [HttpGet("byguid/{id2}", Name = "obtenerSuppliersbyGuid")]
        //[AllowAnonymous]
        public async Task<ActionResult<SupplierDTO>> Get(Guid id2)
        {
            var suppliers = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id2);

            if (suppliers == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierDTO>(suppliers);
        }

        [HttpPost]
        //[AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] SupplierDTO supplierDTO, Guid storeId)
        {

            var existe = await context.Suppliers.AnyAsync(x => x.SupplierIdx == supplierDTO.SupplierIdx);

            var supplier = mapper.Map<Supplier>(supplierDTO);

            //var supplierDB = await context.Suppliers.FirstOrDefaultAsync(x => x.StoreId == storeId);
            //storeAddress.StoreId = storeId;

            if (existe)
            {
                return BadRequest($"Ya existe {supplierDTO.Name} ");
            }
            else
            {
                context.Add(supplier);

                //var queryable = context.StoreAddresses.AsQueryable();

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = supplier.Name;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
                var storeAddressDTO2 = mapper.Map<SupplierDTO>(supplier);
                return CreatedAtRoute("obtenerSuppliers", new { id = supplierDTO.SupplierIdx }, storeAddressDTO2);

            }
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(SupplierDTO supplierDTO, Guid storeId)
        {
            var supplierDB = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierIdx == supplierDTO.SupplierIdx);

            if (supplierDB is null)
            {
                return NotFound();
            }
            supplierDB = mapper.Map(supplierDTO, supplierDB);

            var storeId2 = storeId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = supplierDB.Name;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Suppliers.AnyAsync(x => x.SupplierIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierIdx == id);
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
            var existe = await context.Suppliers.AnyAsync(x => x.SupplierIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierIdx == id);
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
