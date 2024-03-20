using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
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
    public class ProductController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public ProductController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("viewPrice")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts(Guid storeId)
        {
            try
            {
                var result = await (from p in context.Products
                                    join ps in context.ProductStores on p.ProductId equals ps.ProductId
                                    where ps.StoreId == storeId && p.IsFuel == true
                                    orderby ps.ProductUce
                                    select new
                                    {
                                        p.ProductIdx,
                                        p.ProductId,
                                        p.Name,
                                        p.ProductCode,
                                        ps.Price,
                                        ps.Ieps,
                                    }).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var products = await queryable.OrderByDescending(x => x.ProductIdx).Paginar(paginacionDTO)
               // .Include(b => b.ProductCategory)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<ProductDTO>>(products);
        }

        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<ProductDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Products.Where(x => x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var products = await queryable.OrderByDescending(x => x.ProductIdx).Paginar(paginacionDTO)
                //.Include(b => b.ProductCategory)
                //.Include(c => c.SatClaveProductoServicioIdxNavigation)
                //.Include(c => c.SatClaveUnidad)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<ProductDTO>>(products);
        }


        [HttpGet("activeSinPag")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductDTO>> Get([FromQuery] string nombre, bool? isFuel)
        {
            var queryable = context.Products.Where(x => x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (isFuel is true)
            {
                queryable = queryable.Where(x => x.IsFuel == true);
            }
            var products = await queryable
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<ProductDTO>>(products);
        }

        [HttpGet("byproductid/{id2}")] //por producto gui
        //[AllowAnonymous]
        public async Task<ActionResult<List<ProductDTO>>> Get2([FromRoute] Guid id2)
        {
            var RSs = await context.Products.Where(e => e.ProductId.Equals((Guid)id2))
                .ToListAsync();
            return mapper.Map<List<ProductDTO>>(RSs);
        }


        [HttpGet("{id:int}", Name = "obtenerProduct")]
        //[AllowAnonymous]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var products = await context.Products.Where(x => x.ProductIdx == id)
                //.Include(c => c.SatClaveProductoServicioIdxNavigation)
                .AsNoTracking().FirstOrDefaultAsync();
            return mapper.Map<ProductDTO>(products);
        }


        [HttpGet("guid/{id}", Name = "obtenerProductGuid")]
        public async Task<ActionResult<ProductDTO>> Get(Guid id)
        {
            var Product = await context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            if (Product == null)
            {
                return NotFound();
            }
            return mapper.Map<ProductDTO>(Product);
        }


        [HttpGet("buscar/{textoBusqueda}")]
        //[AllowAnonymous]
        public async Task<ActionResult<List<ProductDTO>>> Get4(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda)) { return new List<ProductDTO>(); }
            textoBusqueda = textoBusqueda.ToLower();
            var RSs = await context.Products.Where(storedb => storedb.Name.ToLower().Contains(textoBusqueda))
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<ProductDTO>>(RSs);
        }


        [HttpPost("{storeId?}")]
        public async Task<ActionResult> Post(Guid storeId, [FromBody] ProductDTO productDTO)
        {
            var existeid = await context.Products.AnyAsync(x => x.ProductIdx == productDTO.ProductIdx);
            var Product = mapper.Map<Product>(productDTO);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = Product.Name;
            var storeId2 = storeId;

            if (existeid)
            {
                context.Update(Product);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.Products.AnyAsync(x => x.ProductId == (productDTO.ProductId));

                if (existe)
                {
                    return BadRequest($"Ya existe {productDTO.ProductId}");
                }
                else
                {
                    context.Add(Product);
                    await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                    await context.SaveChangesAsync();
                }
            }
            var ProducDTO2 = mapper.Map<ProductDTO>(Product);
            return CreatedAtRoute("obtenerProduct", new { id = Product.ProductIdx }, ProducDTO2);
        }


        [HttpPost("withUpdate/{id}/{selected?}")]
        //[AllowAnonymous]
        public async Task<ActionResult> Post2(ProductDTO productDTO, int id, string selected)
        {
            var existeid = await context.Products.AnyAsync(x => x.ProductIdx == id);

            var product = mapper.Map<Product>(productDTO);

            if (existeid)
            {
                context.Update(product);
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                context.Add(product);
                await context.SaveChangesAsync();
                return Ok();
            }
            //var ProducDTO2 = mapper.Map<ProductDTO>(Product);
            //return CreatedAtRoute("obtenerProduct", new { id = Product.ProductIdx }, ProducDTO2);
        }


        [HttpPut("{storeId?}")]
        //[AllowAnonymous]
        public async Task<IActionResult> Put(Guid storeId, ProductDTO productDTO)
        {
            var productDB = await context.Products.FirstOrDefaultAsync(x => x.ProductIdx == productDTO.ProductIdx);

            if (productDB is null) { return NotFound(); }

            productDB = mapper.Map(productDTO, productDB);

            var storeId2 = storeId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var tableName = productDB.Name;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("desconectado/{id}")]
        //[AllowAnonymous]
        public async Task<IActionResult> PutDesconectado(int id, ProductDTO productDTO)
        {
            var productoenbd = await context.Products.AnyAsync(x => x.ProductIdx == id);

            if (!productoenbd)
            {
                return NotFound();
            }

            context.Update(productoenbd);
            await context.SaveChangesAsync();
            return NoContent();

            //var existe = await context.Products.AnyAsync(x => x.ProductIdx == id);

            //if (!existe)
            //{
            //    return NotFound();
            //}
            //var ProducDB = mapper.Map<Product>(productDTO);

            //ProducDB.ProductIdx = id;
            //context.Update(ProducDB);
            //await context.SaveChangesAsync();
            //return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Products.AnyAsync(x => x.ProductIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Products.FirstOrDefaultAsync(x => x.ProductIdx == id);
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
                var existe = await context.Products.AnyAsync(x => x.ProductIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.Products.FirstOrDefaultAsync(x => x.ProductIdx == id);
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
