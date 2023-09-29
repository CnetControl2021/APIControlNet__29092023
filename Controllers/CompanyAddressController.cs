using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompanyAddressController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public CompanyAddressController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle=servicioBinnacle;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("sinpag")]
        public async Task<IEnumerable<CompanyAddressDTO>> Get()
        {
            var queryable = context.CompanyAddresses.AsQueryable();

            var companiesAddress = await queryable
                //.Include(a => a.SatCodigoPostal)
                .AsNoTracking().ToListAsync();
            return mapper.Map<List<CompanyAddressDTO>>(companiesAddress);
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IEnumerable<CompanyAddressDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId, Guid companyId)
        {
            var queryable = context.CompanyAddresses.Where(x => x.Active == true && x.Deleted == false).AsQueryable();
            //var queryable = context.Companies.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Description.ToLower().Contains(nombre));
            }
            if (companyId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.CompanyId == companyId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var companyAddr = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
            return mapper.Map<List<CompanyAddressDTO>>(companyAddr);
        }




        [HttpGet("{id:int}", Name = "obtenerCompanyAddress")]
        public async Task<ActionResult<CompanyAddressDTO>> Get(int id)
        {
            var companyAddr = await context.CompanyAddresses.FirstOrDefaultAsync(x => x.CompanyAddressIdx == id);

            if (companyAddr == null)
            {
                return NotFound();
            }
            return mapper.Map<CompanyAddressDTO>(companyAddr);
        }


        [HttpPost]
        //[AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] CompanyAddressDTO companyAddressDTO, Guid storeId, string companyId)
        {

            var existe = await context.CompanyAddresses.AnyAsync(x => x.CompanyAddressIdx == companyAddressDTO.CompanyAddressIdx);
            var companyAddress = mapper.Map<CompanyAddress>(companyAddressDTO);

            var companyidDb = await context.Companies.FirstOrDefaultAsync(x => x.CompanyId.ToString() == companyId);
            companyAddress.CompanyId = companyidDb.CompanyId;

            if (existe)
            {
                return BadRequest($"Ya existe {companyAddressDTO.Description} ");
            }
            else
            {
                 context.Add(companyAddress);

                    //var queryable = context.Stores.AsQueryable();

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = companyAddressDTO.Street;
                var storeId2 = storeId;
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
                var CompanyAddressDTO2 = mapper.Map<CompanyAddressDTO>(companyAddress);
                return CreatedAtRoute("obtenerCompanyAddress", new { id = companyAddress.CompanyAddressIdx }, CompanyAddressDTO2);

            }
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(CompanyAddressDTO companyAddressDTO, Guid storeId)
        {
            var companyAddrDB = await context.CompanyAddresses.FirstOrDefaultAsync(c => c.CompanyAddressIdx == companyAddressDTO.CompanyAddressIdx);

            if (companyAddrDB is null)
            {
                return NotFound();
            }
            companyAddrDB = mapper.Map(companyAddressDTO, companyAddrDB);

            var storeId2 = storeId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = companyAddrDB.Street;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        ////[AllowAnonymous]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.CompanyAddresses.AnyAsync(x => x.CompanyAddressIdx == id);
            if (!existe) { return NotFound(); }

            var queryable = context.Stores.AsQueryable();

            var name2 = await context.CompanyAddresses.FirstOrDefaultAsync(x => x.CompanyAddressIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Street;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        ////[AllowAnonymous]
        public async Task<IActionResult> Delete(int id, Guid? storeId)
        {
            var existe = await context.CompanyAddresses.AnyAsync(x => x.CompanyAddressIdx == id);
            if (!existe) { return NotFound(); }

            var queryable = context.Stores.AsQueryable();
            if (storeId!=Guid.Empty || storeId is not null)
            {
                queryable = queryable.Where(x => x.StoreId==storeId);
            }
            var name2 = await context.CompanyAddresses.FirstOrDefaultAsync(x => x.CompanyAddressIdx == id);
            context.Remove(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Street;
            var storeId2 = storeId;
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
