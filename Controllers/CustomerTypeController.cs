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
    public class CustomerTypeController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public CustomerTypeController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<CustomerTypeDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.CustomerTypes.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);

            var customersType = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<CustomerTypeDTO>>(customersType);
        }


        [HttpGet("{id:int}", Name = "obtenerCustomerType")]
        public async Task<ActionResult<CustomerTypeDTO>> Get(int id)
        {
            var customersType = await context.CustomerTypes.FirstOrDefaultAsync(x => x.CustomerTypeId == id);

            if (customersType == null)
            {
                return NotFound();
            }

            return mapper.Map<CustomerTypeDTO>(customersType);
        }


        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<CustomerTypeDTO>>> Get([FromRoute] string nombre)
        {
            var RSs = await context.CustomerTypes.Where(Customerdb => Customerdb.Name.Contains(nombre)).ToListAsync();

            return mapper.Map<List<CustomerTypeDTO>>(RSs);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CustomerTypeDTO CustomerTypeDTO)
        {

            var existeid = await context.CustomerTypes.AnyAsync(x => x.CustomerTypeIdx == CustomerTypeDTO.CustomerTypeIdx);

            var customersType = mapper.Map<Customer>(CustomerTypeDTO);

            if (existeid)
            {
                context.Update(customersType);
                await context.SaveChangesAsync();
            }
            else
            {
                var existe = await context.CustomerTypes.AnyAsync(x => x.CustomerTypeIdx == (CustomerTypeDTO.CustomerTypeIdx) | x.Name == CustomerTypeDTO.Name);

                if (existe)
                {
                    return BadRequest($"Ya existe {CustomerTypeDTO.CustomerTypeIdx} {CustomerTypeDTO.Name} ");
                }
                else
                {
                    context.Add(customersType);
                    await context.SaveChangesAsync();
                }
            }
            var CustomerTypeDTO2 = mapper.Map<CustomerTypeDTO>(customersType);
            return CreatedAtRoute("obtenerCustomerType", new { id = customersType.CustomerIdx }, CustomerTypeDTO2);
        }


        [HttpPut]
        public async Task<IActionResult> Put(CustomerTypeDTO CustomerTypeDTO)
        {

            var customersType = mapper.Map<Customer>(CustomerTypeDTO);
            context.Update(customersType).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<CustomerTypeDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var CustomerDB = await context.CustomerTypes.FirstOrDefaultAsync(x => x.CustomerTypeIdx == id);

            if (CustomerDB == null)
            {
                return NotFound();
            }

            var CustomerTypeDTO = mapper.Map<CustomerTypeDTO>(CustomerDB);

            patchDocument.ApplyTo(CustomerTypeDTO, ModelState);

            var esValido = TryValidateModel(CustomerTypeDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(CustomerTypeDTO, CustomerDB);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existe = await context.CustomerTypes.AnyAsync(x => x.CustomerTypeIdx == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Customer { CustomerIdx = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
