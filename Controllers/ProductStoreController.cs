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
using System.Linq;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductStoreController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public ProductStoreController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("{storeId?}")]
        public async Task<IEnumerable<ProductStoreDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.ProductStores.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.ProductPemex.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var producStore = await queryable.Paginar(paginacionDTO)
                //.Include(x => x.Store).ThenInclude(h => h.Hoses)
                .Include(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<ProductStoreDTO>>(producStore);
        }


        [HttpGet("active/{storeId?}")]
        //[AllowAnonymous]
        public async Task<IEnumerable<ProductStoreDTO>> Get2(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.ProductStores.Where(x => x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.ProductPemex.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var producStore = await queryable.Paginar(paginacionDTO)
                //.Include(x => x.Store).ThenInclude(h => h.Hoses)
                .Include(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<ProductStoreDTO>>(producStore);
        }

        [HttpGet("activeSinPag")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductStoreDTO>> Get3([FromQuery] Guid storeId)
        {
            var queryable = context.ProductStores.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var producStore = await queryable
                .Include(x => x.Product)
                //.AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<ProductStoreDTO>>(producStore);
        }


        [HttpGet("{id:int}", Name = "obtenerProductStore")]
        public async Task<ActionResult<ProductStoreDTO>> Get(int id)
        {
            var productStore = await context.ProductStores.FirstOrDefaultAsync(x => x.ProductStoreIdx == id);

            if (productStore == null)
            {
                return NotFound();
            }

            return mapper.Map<ProductStoreDTO>(productStore);
        }


        [HttpGet("byStore/{id2}")] //por store 
        ////[AllowAnonymous]
        public async Task<ActionResult<List<ProductStoreDTO>>> Get([FromRoute] Guid id2)
        {
            var RSs = await context.ProductStores.Where(e => e.StoreId.Equals((Guid)id2) && e.Active == true)
                .Include(x => x.Product)
                .ToListAsync();
            return mapper.Map<List<ProductStoreDTO>>(RSs);
        }


        //[HttpGet("byStoreAndGui/{id2}")] //por store y gio
        ////[AllowAnonymous]
        //public async Task<ActionResult<List<ProductStoreDTO>>> Get([FromRoute] Guid id2, Guid id)
        //{
        //    var RSs = await context.ProductStores.Where(e => e.StoreId.Equals((Guid)id2)&& e.ProductId.Equals((Guid)id))
        //        .Include(x => x.Store)
        //        .ToListAsync();
        //    return mapper.Map<List<ProductStoreDTO>>(RSs);
        //}

        //[HttpGet("buscar/{textoBusqueda}")]
        ////[AllowAnonymous]
        //public async Task<ActionResult<List<ProductStoreDTO>>> Get4(string textoBusqueda)
        //{
        //    if (string.IsNullOrWhiteSpace(textoBusqueda)) { return new List<ProductStoreDTO>(); }
        //    textoBusqueda = textoBusqueda.ToLower();
        //    var RSs = await context.ProductStores.Where(storedb => storedb.Product.Name.ToLower().Contains(textoBusqueda))
        //        .AsNoTracking()
        //        .ToListAsync();
        //    return mapper.Map<List<ProductStoreDTO>>(RSs);
        //}


        [HttpPost("{storeId?}")]
        public async Task<ActionResult> Post([FromBody] ProductStoreDTO productStoreDTO, Guid storeId)
        {
            var existeid = await context.ProductStores.AnyAsync(x => x.ProductStoreIdx == productStoreDTO.ProductStoreIdx);

            var productStore = mapper.Map<ProductStore>(productStoreDTO);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = productStore.ProductUce;
            var storeId2 = storeId;

            if (existeid)
            {
                context.Update(productStore);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.ProductStores.AnyAsync(x => x.ProductId == (productStore.ProductId) && x.StoreId == productStore.StoreId);

                if (existe)
                {
                    return BadRequest($"Ya existe {productStore.ProductUce} en esa sucursal ");
                }
                else
                {
                    context.Add(productStore);
                    await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                    await context.SaveChangesAsync();
                }
            }
            var ProductStoreDTO2 = mapper.Map<ProductStoreDTO>(productStore);
            return CreatedAtRoute("obtenerProductStore", new { id = productStore.ProductStoreIdx }, ProductStoreDTO2);
        }


        
        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(ProductStoreDTO productStoreDTO, Guid storeId)
        {
            var productStoreDB = await context.ProductStores.FirstOrDefaultAsync(c => c.ProductStoreIdx == productStoreDTO.ProductStoreIdx);

            if (productStoreDB is null)
            {
                return NotFound();
            }
         
            productStoreDB = mapper.Map(productStoreDTO, productStoreDB);

            var storeId2 = storeId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var tableName = productStoreDB.ProductUce;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
            await context.SaveChangesAsync();
            
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.ProductStores.AnyAsync(x => x.ProductStoreIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.ProductStores.FirstOrDefaultAsync(x => x.ProductStoreIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.ProductUce;
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
                var existe = await context.ProductStores.AnyAsync(x => x.ProductStoreIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.ProductStores.FirstOrDefaultAsync(x => x.ProductStoreIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.ProductUce;
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
