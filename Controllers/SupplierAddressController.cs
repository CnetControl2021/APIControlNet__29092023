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
using System.Linq;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SupplierAddressController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public SupplierAddressController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<SupplierAddressDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.SupplierAddresses.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Street.ToLower().Contains(nombre));
            }
            //if (storeId != Guid.Empty)
            //{
            //    queryable = queryable.Where(x => x.StoreId == storeId);
            //}
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var suppliersAddr = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
            return mapper.Map<List<SupplierAddressDTO>>(suppliersAddr);
        }


        [HttpGet("{id:int}", Name = "obtenerSupplierAddr")]
        public async Task<ActionResult<SupplierAddressDTO>> Get(int id)
        {
            var suppliersAddr = await context.SupplierAddresses.FirstOrDefaultAsync(x => x.SupplierAddressIdx == id);

            if (suppliersAddr == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierAddressDTO>(suppliersAddr);
        }


        [HttpGet("byguid/{id2}", Name = "obtenerSuppliersAddrbyGuid")]
        ////[AllowAnonymous]
        public async Task<ActionResult<SupplierAddressDTO>> Get(Guid id2)
        {
            var suppliersAddr = await context.SupplierAddresses.FirstOrDefaultAsync(x => x.SupplierId == id2);

            if (suppliersAddr == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierAddressDTO>(suppliersAddr);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SupplierAddressDTO SupplierAddressDTO, Guid storeId, Guid id2)
        {
            var existe = await context.SupplierAddresses.AnyAsync(x => x.SupplierAddressIdi == SupplierAddressDTO.SupplierAddressIdi);

            var suppliersAddr = mapper.Map<SupplierAddress>(SupplierAddressDTO);
            suppliersAddr.SupplierId = id2;

            if (existe)
            {
                return BadRequest($"Ya existe {SupplierAddressDTO.SupplierAddressIdi} y {SupplierAddressDTO.Street} ");
            }
            else
            {
                context.Add(suppliersAddr);

                //var queryable = context.StoreAddresses.AsQueryable();

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = suppliersAddr.Street;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
                var storeAddressDTO2 = mapper.Map<SupplierAddressDTO>(suppliersAddr);
                return CreatedAtRoute("obtenerSupplierAddr", new { id = SupplierAddressDTO.SupplierAddressIdx }, storeAddressDTO2);

            }
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(SupplierAddressDTO SupplierAddressDTO, Guid storeId)
        {
            var supplierAddrDB = await context.SupplierAddresses.FirstOrDefaultAsync(c => c.SupplierAddressIdx == SupplierAddressDTO.SupplierAddressIdx);

            if (supplierAddrDB is null)
            {
                return NotFound();
            }
            supplierAddrDB = mapper.Map(SupplierAddressDTO, supplierAddrDB);

            var storeId2 = storeId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = supplierAddrDB.Street;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.SupplierAddresses.AnyAsync(x => x.SupplierAddressIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierAddresses.FirstOrDefaultAsync(x => x.SupplierAddressIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Street;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(int id, Guid? storeId)
        {
            var existe = await context.SupplierAddresses.AnyAsync(x => x.SupplierAddressIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierAddresses.FirstOrDefaultAsync(x => x.SupplierAddressIdx == id);
            context.Remove(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Street;
            var storeId2 = storeId;
            var tabla = "SupplierAddresses";
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2, tabla);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
