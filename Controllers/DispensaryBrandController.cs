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
    public class DispensaryBrandController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public DispensaryBrandController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IEnumerable<DispensaryBrandDTO>> Get()
        {
            var companies = await context.DispensaryBrands.ToListAsync();
            return mapper.Map<List<DispensaryBrandDTO>>(companies);

        }

        [HttpGet("Active")]
        public async Task<IEnumerable<DispensaryBrandDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.DispensaryBrands.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var dispensaryBrand = await queryable.Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<DispensaryBrandDTO>>(dispensaryBrand);
        }

        [HttpGet("{id:int}", Name = "obtenerDispensaryBrand")]
        public async Task<ActionResult<DispensaryBrandDTO>> Get(int id)
        {
            var DispensaryBrand = await context.DispensaryBrands.FirstOrDefaultAsync(x => x.DispensaryBrandIdx == id);

            if (DispensaryBrand == null)
            {
                return NotFound();
            }

            return mapper.Map<DispensaryBrandDTO>(DispensaryBrand);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<DispensaryBrandDTO>>> Get([FromRoute] string nombre)
        {
            var RSs = await context.DispensaryBrands.Where(dispensaryBrand => dispensaryBrand.Name.Contains(nombre)).ToListAsync();

            return mapper.Map<List<DispensaryBrandDTO>>(RSs);
        }

        [HttpPost]
        public async Task<ActionResult> Post(DispensaryBrandDTO DispensaryBrandDTO)
        {
            var existe = await context.DispensaryBrands.AnyAsync(x => x.DispensaryBrandIdx == DispensaryBrandDTO.DispensaryBrandIdx);

            var dispensaryBrand = mapper.Map<Company>(DispensaryBrandDTO);

            if (existe)
            {
                context.Update(dispensaryBrand);
                await context.SaveChangesAsync();
            }
            else
            {
                var existeIDoNombre = await context.DispensaryBrands.AnyAsync(x => x.DispensaryBrandId == (DispensaryBrandDTO.DispensaryBrandId) | x.Name == DispensaryBrandDTO.Name);
                if (existeIDoNombre)
                {
                    return BadRequest($"Ya existe {DispensaryBrandDTO.DispensaryBrandId} {DispensaryBrandDTO.Name} ");
                }
                else
                {
                    context.Add(dispensaryBrand);
                    await context.SaveChangesAsync();
                }
            }
            var DispensaryBrandDTO2 = mapper.Map<DispensaryBrandDTO>(dispensaryBrand);
            return CreatedAtRoute("obtenerDispensaryBrand", new { id = dispensaryBrand.CompanyIdx }, DispensaryBrandDTO2);
        }


        [HttpPut]
        public async Task<IActionResult> Put(DispensaryBrandDTO dispensaryBrandDTO)
        {

            var DispensaryBrand = mapper.Map<DispensaryBrand>(dispensaryBrandDTO);
            context.Update(DispensaryBrand).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<DispensaryBrandDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var CompanyDB = await context.DispensaryBrands.FirstOrDefaultAsync(x => x.DispensaryBrandIdx == id);

            if (CompanyDB == null)
            {
                return NotFound();
            }

            var DispensaryBrandDTO = mapper.Map<DispensaryBrandDTO>(CompanyDB);

            patchDocument.ApplyTo(DispensaryBrandDTO, ModelState);

            var esValido = TryValidateModel(DispensaryBrandDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(DispensaryBrandDTO, CompanyDB);
            await context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("eliminar/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existe = await context.Companies.AnyAsync(x => x.CompanyIdx == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Company { CompanyIdx = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
