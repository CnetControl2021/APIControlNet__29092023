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
    public class SupplierTransportController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public SupplierTransportController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<SupplierTransportDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre,
            Guid storeId, Guid id2)
        {
            var queryable = context.SupplierTransports.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
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
            var suppliersTrans = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
            return mapper.Map<List<SupplierTransportDTO>>(suppliersTrans);
        }


        [HttpGet("{id:int}", Name = "newSuppTrans")]
        public async Task<ActionResult<SupplierTransportDTO>> Get(int id)
        {
            var suppliersTrans = await context.SupplierTransports.FirstOrDefaultAsync(x => x.SupplierTransportIdx == id);

            if (suppliersTrans == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierTransportDTO>(suppliersTrans);
        }


        [HttpGet("byguid/{id2?}/{id?}")]
        //[AllowAnonymous]
        public async Task<ActionResult<SupplierTransportDTO>> Get(Guid id2, int id)
        {
            var suppliersTrans = await context.SupplierTransports.FirstOrDefaultAsync(x => x.SupplierId == id2 & x.SupplierTransportIdi == id);

            if (suppliersTrans == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierTransportDTO>(suppliersTrans);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SupplierTransportDTO SupplierTransportDTO, Guid id2, Guid storeId)
        {
            var existe = await context.SupplierTransports.AnyAsync(x => x.SupplierTransportIdi == SupplierTransportDTO.SupplierTransportIdi);

            var suppTrans = mapper.Map<SupplierTransport>(SupplierTransportDTO);
            suppTrans.SupplierId = id2;
            suppTrans.StoreId = storeId;

            if (existe)
            {
                return BadRequest($"Ya existe {SupplierTransportDTO.SupplierTransportIdi} y {SupplierTransportDTO.Name} ");
            }
            else
            {
                context.Add(suppTrans);

                //var queryable = context.StoreAddresses.AsQueryable();

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = suppTrans.Name;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
                var storeAddressDTO2 = mapper.Map<SupplierTransportDTO>(suppTrans);
                return CreatedAtRoute("newSuppTrans", new { id = SupplierTransportDTO.SupplierTransportIdx }, storeAddressDTO2);

            }
        }


        //[HttpPut("{storeId?}")]
        [HttpPut]
        //[AllowAnonymous]
        public async Task<IActionResult> Put(SupplierTransportDTO SupplierTransportDTO )
        {
            var supTransDB = await context.SupplierTransports.FirstOrDefaultAsync(c => c.SupplierTransportIdx == SupplierTransportDTO.SupplierTransportIdx);


            if (supTransDB is null)
            {
                return NotFound();
            }
            supTransDB = mapper.Map(SupplierTransportDTO, supTransDB);

            var storeId2 = supTransDB.StoreId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = supTransDB.Name;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.SupplierTransports.AnyAsync(x => x.SupplierTransportIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierTransports.FirstOrDefaultAsync(x => x.SupplierTransportIdx == id);
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
            var existe = await context.SupplierTransports.AnyAsync(x => x.SupplierTransportIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierTransports.FirstOrDefaultAsync(x => x.SupplierTransportIdx == id);
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
