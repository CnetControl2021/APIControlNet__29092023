using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SupplierTransRegController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public SupplierTransRegController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("{storeId?}")]
        public async Task<IEnumerable<SupplierTransportRegisterDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.SupplierTransportRegisters.AsQueryable();
            //if (!string.IsNullOrEmpty(nombre))
            //{
            //    queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            //}
            //if (storeId != Guid.Empty)
            //{
            //    queryable = queryable.Where(x => x.StoreId == storeId);
            //}
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var supTraReg = await queryable.Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<SupplierTransportRegisterDTO>>(supTraReg);
        }


        [HttpGet("Active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<SupplierTransportRegisterDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.SupplierTransportRegisters.Where(x => x.Active == 1).AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var supTraReg = await queryable.Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<SupplierTransportRegisterDTO>>(supTraReg);
        }

        [HttpGet("ActiveSinPag")]
        //[AllowAnonymous]
        public async Task<IEnumerable<SupplierTransportRegisterDTO>> Get3([FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.SupplierTransportRegisters.Where(x => x.Active == 1 && x.Deleted == 0).AsQueryable();
            var supTraReg = await queryable.OrderByDescending(x => x.SupplierTransportRegisterIdx)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<SupplierTransportRegisterDTO>>(supTraReg);
        }


        [HttpGet("{id:int}", Name = "obtSupTraReg")]
        public async Task<ActionResult<SupplierTransportRegisterDTO>> Get(int id)
        {
            var supTraReg = await context.SupplierTransportRegisters.FirstOrDefaultAsync(x => x.SupplierTransportRegisterIdx == id);

            if (supTraReg == null)
            {
                return NotFound();
            }

            return mapper.Map<SupplierTransportRegisterDTO>(supTraReg);
        }





        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.SupplierTransportRegisters.AnyAsync(x => x.SupplierTransportRegisterIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierTransportRegisters.FirstOrDefaultAsync(x => x.SupplierTransportRegisterIdx == id);
            name2.Active = 0;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.SupplierTransportRegisterId;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name.ToString(), storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            try
            {
                var existe = await context.SupplierTransportRegisters.AnyAsync(x => x.SupplierTransportRegisterIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.SupplierTransportRegisters.FirstOrDefaultAsync(x => x.SupplierTransportRegisterIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.SupplierTransportRegisterIdx;
                var storeId2 = storeId;
                await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name.ToString(), storeId2);

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
