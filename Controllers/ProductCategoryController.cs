using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductCategoryController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public ProductCategoryController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<ProductCategoryDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.ProductCategories.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var productsCategory = await queryable.AsNoTracking()
                .ToListAsync();
            return Ok(productsCategory);
        }


        [HttpGet("{id:int}", Name = "obtenerProductCategory")]
        public async Task<ActionResult<ProductCategoryDTO>> Get(int id)
        {
            var productCategory = await context.ProductCategories.FirstOrDefaultAsync(x => x.ProductCategoryIdx == id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return mapper.Map<ProductCategoryDTO>(productCategory);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<ProductCategoryDTO>>> Get([FromRoute] string nombre)
        {
            var RSs = await context.ProductCategories.Where(Productdb => Productdb.Name.Contains(nombre)).ToListAsync();

            return mapper.Map<List<ProductCategoryDTO>>(RSs);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductCategoryDTO ProductCategoryDTO)
        {

            var existeid = await context.ProductCategories.AnyAsync(x => x.ProductCategoryIdx == ProductCategoryDTO.ProductCategoryIdx);

            var productCategory = mapper.Map<ProductCategory>(ProductCategoryDTO);

            if (existeid)
            {
                context.Update(productCategory);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.ProductCategories.AnyAsync(x => x.ProductCategoryIdx == (ProductCategoryDTO.ProductCategoryIdx) | x.Name == ProductCategoryDTO.Name);

                if (existe)
                {
                    return BadRequest($"Ya existe {ProductCategoryDTO.ProductCategoryIdx} {ProductCategoryDTO.Name} ");
                }
                else
                {
                    context.Add(productCategory);
                    await context.SaveChangesAsync();
                }
            }
            var ProductCategoryDTO2 = mapper.Map<ProductCategoryDTO>(productCategory);
            return CreatedAtRoute("obtenerProductCategory", new { id = productCategory.ProductCategoryIdx }, ProductCategoryDTO2);
        }


        [HttpPut]
        public async Task<IActionResult> Put(ProductCategoryDTO ProductCategoryDTO)
        {
            var productCategory = mapper.Map<ProductCategory>(ProductCategoryDTO);
            context.Update(productCategory).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }



        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<ProductCategoryDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var ProductDB = await context.ProductCategories.FirstOrDefaultAsync(x => x.ProductCategoryIdx == id);

            if (ProductDB == null)
            {
                return NotFound();
            }

            var ProductCategoryDTO = mapper.Map<ProductCategoryDTO>(ProductDB);

            patchDocument.ApplyTo(ProductCategoryDTO, ModelState);

            var esValido = TryValidateModel(ProductCategoryDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(ProductCategoryDTO, ProductDB);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existe = await context.ProductCategories.AnyAsync(x => x.ProductCategoryIdx == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new ProductCategory { ProductCategoryIdx = id });
            await context.SaveChangesAsync();
            return NoContent();
        }


    }
}
