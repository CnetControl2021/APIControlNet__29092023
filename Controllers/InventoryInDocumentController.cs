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
    public class InventoryInDocumentController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public InventoryInDocumentController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<InventoryInDocumentDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.InventoryInDocuments.AsQueryable();
            //if (!string.IsNullOrEmpty(nombre))
            //{
            //    queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            //}
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var invindocument = await queryable.OrderByDescending(x => x.InventoryInDocumentIdx).Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<InventoryInDocumentDTO>>(invindocument);
        }


        [HttpGet("Active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<InventoryInDocumentDTO>> Get2(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, Guid inventoryInId)
        {
            var queryable = context.InventoryInDocuments.Where(x => x.Active == true).AsQueryable();
            //if (!string.IsNullOrEmpty(nombre))
            //{
            //    queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            //}
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            if (inventoryInId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.InventoryInId == inventoryInId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var invindocument = await queryable.OrderByDescending(x => x.InventoryInDocumentIdx).Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<InventoryInDocumentDTO>>(invindocument);
        }


        //[HttpGet("sinPag/{nombre}")]
        //[AllowAnonymous]
        //public async Task<IEnumerable<InventoryInDocumentDTO>> Get(Guid storeId, Guid idGuid)
        //{
        //    var data = context.InventoryInDocuments.Where(x => x.InventoryInId == idGuid).FirstOrDefaultAsync();
        //    var uuid = data.Result.Uuid.ToString();

           
        //    return mapper.Map<List<InventoryInDocumentDTO>>(data);
        //}


        [HttpGet("{id:int}", Name = "getInventoryInDocument")]
        //[AllowAnonymous]
        public async Task<ActionResult<InventoryInDocumentDTO>> Get(int id)
        {
            var invindocument = await context.InventoryInDocuments.FirstOrDefaultAsync(x => x.InventoryInDocumentIdx == id);

            if (invindocument == null)
            {
                return NotFound();
            }

            return mapper.Map<InventoryInDocumentDTO>(invindocument);
        }

        [HttpGet("{InventoryInId}/{storeId}")]
        [AllowAnonymous]
        public async Task<ActionResult<InventoryInDocumentDTO>> Get(Guid InventoryInId, Guid storeId)
        {
            var list = await context.InventoryInDocuments.Where(x => x.InventoryInId == InventoryInId
            && x.StoreId == storeId ).ToListAsync();

            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }


        //[HttpPost]
        //public async Task<ActionResult> Post([FromBody] InventoryInDocumentDTO inventoryInDocumentDTO)
        //{
        //    var existeid = await context.InventoryInDocuments.AnyAsync
        //    (x => x.InventoryInId == inventoryInDocumentDTO.InventoryInId && x.StoreId == inventoryInDocumentDTO.StoreId && x.InventoryInIdi == inventoryInDocumentDTO.InventoryInIdi);

        //    var invindocument = mapper.Map<InventoryInDocument>(inventoryInDocumentDTO);
            

        //    var usuarioId = obtenerUsuarioId();
        //    var ipUser = obtenetIP();
        //    var name = invindocument.Folio.ToString();
        //    var storeId2 = inventoryInDocumentDTO.StoreId;

            
        //    if (existeid)
        //    {
        //        return BadRequest($"Ya existe {inventoryInDocumentDTO.StoreId} en esa empresa");
        //    }
        //    else
        //    {
        //        context.Add(invindocument);

        //        //await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
        //        await context.SaveChangesAsync();
        //    }
        //    return Ok();
        //    //var storeDTO2 = mapper.Map<InventoryInDocumentDTO>(employeeMap);
        //    //return CreatedAtRoute("getEmployee", new { id = employeeMap.EmployeeId }, storeDTO2);
        //}


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(InventoryInDocumentDTO InventoryInDocumentDTO, Guid storeId)
        {
            var invindocumentDB = await context.InventoryInDocuments.FirstOrDefaultAsync(c => c.InventoryInDocumentIdx == InventoryInDocumentDTO.InventoryInDocumentIdx);

            if (invindocumentDB is null)
            {
                return NotFound();
            }
            try
            {
                invindocumentDB = mapper.Map(InventoryInDocumentDTO, invindocumentDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = invindocumentDB.Folio.ToString();
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe {InventoryInDocumentDTO.Folio} ");
            }
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.InventoryInDocuments.AnyAsync(x => x.InventoryInDocumentIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.InventoryInDocuments.FirstOrDefaultAsync(x => x.InventoryInDocumentIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Folio.ToString();
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
                var existe = await context.InventoryInDocuments.AnyAsync(x => x.InventoryInDocumentIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.InventoryInDocuments.FirstOrDefaultAsync(x => x.InventoryInDocumentIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.Folio.ToString();
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
