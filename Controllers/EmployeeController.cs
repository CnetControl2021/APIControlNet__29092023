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
    public class EmployeeController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public EmployeeController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<EmployeeDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Employees.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var employee = await queryable.OrderByDescending(x => x.EmployeeId).Paginar(paginacionDTO)
                //.Include(x => x.LoadPosition)
                //.Include(x => x.ProductStore).ThenInclude(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<EmployeeDTO>>(employee);
        }


        [HttpGet("Active")]
        ////[AllowAnonymous]
        public async Task<IEnumerable<EmployeeDTO>> Get2(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Employees.Where(x => x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var employee = await queryable.OrderByDescending(x => x.EmployeeIdx).Paginar(paginacionDTO)
                //.Include(x => x.LoadPosition)
                //.Include(x => x.ProductStore).ThenInclude(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<EmployeeDTO>>(employee);
        }

        [HttpGet("sinPag/{nombre}")]
        //[AllowAnonymous]
        public async Task<IEnumerable<EmployeeDTO>> Get(Guid storeId, string nombre)
        {
            var queryable = context.Employees.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var employee = await queryable
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<EmployeeDTO>>(employee);
        }


        [HttpGet("{id:int}", Name = "getEmployee")]
        public async Task<ActionResult<EmployeeDTO>> Get(int id)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(x => x.EmployeeIdx == id);

            if (employee == null)
            {
                return NotFound();
            }

            return mapper.Map<EmployeeDTO>(employee);
        }


        [HttpPost("{storeId?}")]
        public async Task<ActionResult> Post([FromBody] EmployeeDTO employeeDTO, Guid? storeId)
        {
            var existeid = await context.Employees.AnyAsync(x => x.EmployeeId == employeeDTO.EmployeeId && x.StoreId == employeeDTO.StoreId);

            var employeeMap = mapper.Map<Employee>(employeeDTO);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = employeeMap.Name;
            var storeId2 = storeId;

            if (existeid)
            {
                return BadRequest($"Ya existe {employeeDTO.StoreId} en esa empresa");
            }
            else
            {
                context.Add(employeeMap);

                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
            }
            return Ok();
            //var storeDTO2 = mapper.Map<EmployeeDTO>(employeeMap);
            //return CreatedAtRoute("getEmployee", new { id = employeeMap.EmployeeId }, storeDTO2);
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(EmployeeDTO EmployeeDTO, Guid storeId)
        {
            var employeeDB = await context.Employees.FirstOrDefaultAsync(c => c.EmployeeIdx == EmployeeDTO.EmployeeIdx);

            if (employeeDB is null)
            {
                return NotFound();
            }
            try
            {
                employeeDB = mapper.Map(EmployeeDTO, employeeDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = employeeDB.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe {EmployeeDTO.Name} ");
            }
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Employees.AnyAsync(x => x.EmployeeIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.Employees.FirstOrDefaultAsync(x => x.EmployeeIdx == id);
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
                var existe = await context.Employees.AnyAsync(x => x.EmployeeIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.Employees.FirstOrDefaultAsync(x => x.EmployeeIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.Name;
                var storeId2 = storeId;
                var tabla = "Employees";
                await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2, tabla);

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
