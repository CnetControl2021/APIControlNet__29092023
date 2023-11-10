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
    public class ProductSatController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public ProductSatController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("{storeId?}")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductSatDTO>> Get5(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.ProductSats.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.ClaveProducto.ToString().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var productsat = await queryable.Paginar(paginacionDTO)
                .Include(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<ProductSatDTO>>(productsat);
        }


        [HttpGet("Active")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductSatDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.ProductSats.Where(x => x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.ClaveProducto.ToString().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var productsat = await queryable.Paginar(paginacionDTO)
                .Include(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<ProductSatDTO>>(productsat);
        }

        //[HttpGet("byStore/{id2}")] //por store
        //public async Task<ActionResult<List<ProductSatDTO>>> Get2([FromRoute] Guid id2)
        //{
        //    var RSs = await context.Tanks.Where(e => e.StoreId.Equals((Guid)id2))
        //        .ToListAsync();
        //    return mapper.Map<List<ProductSatDTO>>(RSs);
        //}


        [HttpGet("byid/{idx}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductSatDTO>> Get4(int idx)
        {
            var productSats = await context.ProductSats.FirstOrDefaultAsync(x => x.ProductSatIdx == idx);

            if (productSats == null)
            {
                return NotFound();
            }
            return mapper.Map<ProductSatDTO>(productSats);
        }


        [HttpGet("byGuid/{idGuid}", Name = "obtenerProductSat")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductSatDTO>> Get(Guid idGuid)
        {
            //var productSats = await context.ProductSats.FirstOrDefaultAsync(x => x.ProductId == idGuid);
            var productSats = await context.ProductSats.Include(p => p.Product).FirstOrDefaultAsync(x => x.ProductId == idGuid);

            if (productSats == null)
            {
                return NotFound();
            }
            return mapper.Map<ProductSatDTO>(productSats);
        }


        //[HttpGet("{nombre}")]
        //public async Task<ActionResult<List<ProductSatDTO>>> Get([FromRoute] string nombre)
        //{
        //    var RSs = await context.Tanks.Where(TankDB => TankDB.Name.Contains(nombre)).ToListAsync();
        //    return mapper.Map<List<ProductSatDTO>>(RSs);
        //}


        [HttpPost("{storeId?}")]
        public async Task<ActionResult> Post([FromBody] ProductSatDTO ProductSatDTO, Guid storeId)
        {
            var existeid = await context.ProductSats.AnyAsync(x => x.ProductId == ProductSatDTO.ProductId && x.StoreId == ProductSatDTO.StoreId);

            var productSatMap = mapper.Map<ProductSat>(ProductSatDTO);
            productSatMap.StoreId = storeId;

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = productSatMap.ClaveProducto.ToString();
            var storeId2 = storeId;

            if (existeid)
            {
                //return BadRequest($"Ya existe {productSatMap.ProductId} en esa sucursal ");
                return BadRequest($"Ya existe este producto en sucursal");
                //context.Update(productSatMap);
                //await context.SaveChangesAsync();
            }
            else
            {
                //var existe = await context.ProductSats.AnyAsync(x => x.ProductId == (productSatMap.ProductId) && x.StoreId == productSatMap.StoreId);

                //if (existe)
                //{
                //    return BadRequest($"Ya existe {productSatMap.ProductId} en esa sucursal ");
                //}
                //else
                //{
                    context.Add(productSatMap);
                    await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                    await context.SaveChangesAsync();
                //}
            }
            var ProductSatDTO2 = mapper.Map<ProductSat>(productSatMap);
            //return CreatedAtRoute("obtenerProductSat", new { id = productSatMap.ProductId }, ProductSatDTO2);
            return Ok(ProductSatDTO2);
        }


        [HttpPut("{storeId?}")]
        [AllowAnonymous]
        public async Task<IActionResult> Put(ProductSatDTO productSat, Guid storeId)
        {
            var db = await context.ProductSats.FirstOrDefaultAsync(c => c.ProductSatIdx == productSat.ProductSatIdx);

            if (db is null)
            {
                return NotFound();
            }
            try
            {
                db = mapper.Map(productSat, db);
                db.Updated = DateTime.Now;

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = db.SatProductKey;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);

                await context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return BadRequest($"Revisar datos");
            }
        }




        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.ProductSats.AnyAsync(x => x.ProductSatIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.ProductSats.FirstOrDefaultAsync(x => x.ProductSatIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.ClaveProducto.ToString();
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
                var existe = await context.ProductSats.AnyAsync(x => x.ProductSatIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.ProductSats.FirstOrDefaultAsync(x => x.ProductSatIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.ClaveProducto.ToString();
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

        [HttpGet("JsonClaveProducto/ActiveNotPage")]
        //[AllowAnonymous]
        public async Task<IEnumerable<JsonClaveProductoDTO>> Get3()
        {
            var queryable = context.JsonClaveProductos.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            var data = await queryable
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<JsonClaveProductoDTO>>(data);
        }

        [HttpGet("JsonSubClaveProducto/ActiveNotPage")]
        //[AllowAnonymous]
        public async Task<IEnumerable<JsonSubclaveProductoDTO>> Get6()
        {
            var queryable = context.JsonSubclaveProductos.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            var data = await queryable
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<JsonSubclaveProductoDTO>>(data);
        }

    }
}
