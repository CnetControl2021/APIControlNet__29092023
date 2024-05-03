using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompanyController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public CompanyController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle=servicioBinnacle;
        }

        public const string gEmpty = "00000000-0000-0000-0000-000000000000";
        string myGuidString = gEmpty.ToString();

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<CompanyDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Companies.Where(x => x.CompanyId.ToString() != myGuidString).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var companies = await queryable.Paginar(paginacionDTO)
                .AsNoTracking().ToListAsync();
            return mapper.Map<List<CompanyDTO>>(companies);
        }


        [HttpGet("active")]
        ////[AllowAnonymous]
        public async Task<IEnumerable<CompanyDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Companies.Where(x => x.Active == true && x.Deleted == false && x.CompanyId.ToString() != myGuidString).AsQueryable();
            //var queryable = context.Companies.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var companies = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
            return mapper.Map<List<CompanyDTO>>(companies);
        }

        [HttpGet("active/notPag")]
        [AllowAnonymous]
        public async Task<IEnumerable<CompanyDTO>> Get3([FromQuery] string nombre)
        {
            var queryable = context.Companies.Where(x => x.Active == true && x.Deleted == false && x.CompanyId.ToString() != myGuidString).AsQueryable();
            //var queryable = context.Companies.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            //await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var companies = await queryable.AsNoTracking().ToListAsync();
            return mapper.Map<List<CompanyDTO>>(companies);
        }


        //[HttpGet]
        //public async Task<IEnumerable<CompanyDTO>> Get()
        //{
        //    //var queryable = context.Companies.AsQueryable();
        //    //await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    //var companies = await queryable.Paginar(paginacionDTO).Where(x => x.Active==1).ToListAsync();
        //    var companies = await context.Companies.ToListAsync();
        //    return mapper.Map<List<CompanyDTO>>(companies);

        //}


        [HttpGet("{id:int}", Name = "obtenerCompany")]
        public async Task<ActionResult<CompanyDTO>> Get(int id)
        {
            var Company = await context.Companies.FirstOrDefaultAsync(x => x.CompanyIdx == id);

            if (Company == null)
            {
                return NotFound();
            }
            return mapper.Map<CompanyDTO>(Company);
        }



        [HttpGet("guid/{id2:guid}", Name = "obtenerCompanyGuid")]
        ////[AllowAnonymous]
        public async Task<ActionResult<CompanyDTO>> Get(Guid id2)
        {
            var Company = await context.Companies.FirstOrDefaultAsync(x => x.CompanyId == id2);

            if (Company == null)
            {
                return NotFound();
            }
            return mapper.Map<CompanyDTO>(Company);
        }


        //[HttpGet("{buscar/nombre}")] 
        //public async Task<ActionResult<List<CompanyDTO>>> Get([FromRoute] string nombre) 
        //{
        //    var RSs = await context.Companies.Where(empresadb => empresadb.Name.Contains(nombre)).ToListAsync();

        //    return mapper.Map<List<CompanyDTO>>(RSs);
        //}

        [HttpGet("buscar/{nombre}")]
        public async Task<ActionResult<List<CompanyDTO>>> Get(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) { return new List<CompanyDTO>(); }
            nombre = nombre.ToLower();
            var RSs = await context.Companies.Where(empresadb => empresadb.Name.ToLower().Contains(nombre)).ToListAsync();
            return mapper.Map<List<CompanyDTO>>(RSs);
        }


        [HttpPost("{storeId?}")]
        public async Task<ActionResult> Post([FromBody]CompanyDTO companyDTO, Guid? storeId)
        {
            //var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            //var email = emailClaim.Value;

            var existe = await context.Companies.AnyAsync(x => x.CompanyIdx == companyDTO.CompanyIdx);
            var company = mapper.Map<Company>(companyDTO);

            if (existe)
            {
                context.Update(company);
                await context.SaveChangesAsync();
            }
            else
            {
                var existeRFCoNombre = await context.Companies.AnyAsync(x => x.Rfc == (companyDTO.Rfc) | x.Name == companyDTO.Name);
                if (existeRFCoNombre)
                {
                    return BadRequest($"Ya existe {companyDTO.Rfc} {companyDTO.Name} ");
                }
                else
                {
                    context.Add(company);

                    var queryable = context.Stores.AsQueryable();
                    
                    //if (storeId!=Guid.Empty)
                    //{
                    //    queryable = queryable.Where(x => x.StoreId==storeId);
                    //}
                    //else
                    //{
                    //    return NotFound();
                    //}
                    //await context.SaveChangesAsync();

                    //await context.Database.ExecuteSqlInterpolatedAsync
                    //($@"INSERT INTO binnacle (binnacle_id, name, user_id, binnacle_type_id, description, value_name, ip_address, mac_address, date, active, locked, deleted) 
                    // VALUES({Guid.NewGuid()}, {company.Name}, {email}, {6}, {"controlVolumetrico"}, {"edit"}, {Ip}, {addr}, {DateTime.Now}, {true}, {false}, {false})");

                    var usuarioId = obtenerUsuarioId();
                    var ipUser = obtenetIP();
                    var name = company.Name;
                    var storeId2 = storeId;
                    await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                    await context.SaveChangesAsync();
                }
            }
            var companyDTO2 = mapper.Map<CompanyDTO>(company);
            return CreatedAtRoute("obtenerCompany", new { id = company.CompanyIdx }, companyDTO2);
        }


        [HttpPut("desconectado/{id:int}")]
        public async Task<IActionResult> PutDesconectado(int id, CompanyDTO CompanyDTO)
        {
            var existe = await context.Companies.AnyAsync(x => x.CompanyIdx == id);

            if (!existe)
            {
                return NotFound();
            }
            
            var Company = mapper.Map<Company>(CompanyDTO);
            Company.CompanyIdx = id;
            context.Update(Company);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(CompanyDTO companyDTO, Guid storeId)
        {
            var companyDB = await context.Companies.FirstOrDefaultAsync(c => c.CompanyIdx == companyDTO.CompanyIdx);

            //var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            //var email = emailClaim.Value;

            ////var ClientIPAddr = HttpContext.Connection.RemoteIpAddress?.ToString();
            ////var ClientIPAddr = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4();
            //var Ip = HttpContext.Connection.RemoteIpAddress;
            //var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

            //var addr = "";
            //foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            //{
            //    //if (n.OperationalStatus == OperationalStatus.Up)
            //    if (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
            //        n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && n.OperationalStatus == OperationalStatus.Up)
            //    {
            //        addr += n.GetPhysicalAddress().ToString();
            //        break;
            //    }
            //}

            if (companyDB is null)
            {
                return NotFound();
            }
            companyDB = mapper.Map(companyDTO, companyDB);
            //await context.SaveChangesAsync();

            //await context.Database.ExecuteSqlInterpolatedAsync
            // ($@"INSERT INTO binnacle (binnacle_id, name, user_id, binnacle_type_id, description, value_name, ip_address, mac_address, updated, active, locked, deleted) 
            //  VALUES({Guid.NewGuid()}, {companyDB.Name}, {email}, {6}, {"controlVolumetrico"}, {"edit"}, {Ip}, {addr}, {DateTime.Now}, {true}, {false}, {false})");

            var storeId2 = storeId;
            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = companyDB.Name;
            await servicioBinnacle.EditBinnacle(usuarioId, ipUser, name, storeId2);
            await context.SaveChangesAsync();
            return NoContent();
        }


        //[HttpPut] //marcando objeto completo
        //public async Task<IActionResult> Put(CompanyDTO CompanyDTO)
        //{

        //    var Company = mapper.Map<Company>(CompanyDTO);
        //    context.Update(Company).State = EntityState.Modified;
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}


        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<CompanyDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var CompanyDB = await context.Companies.FirstOrDefaultAsync(x => x.CompanyIdx == id);

            if (CompanyDB == null)
            {
                return NotFound();
            }

            var CompanyDTO = mapper.Map<CompanyDTO>(CompanyDB);

            patchDocument.ApplyTo(CompanyDTO, ModelState);

            var esValido = TryValidateModel(CompanyDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(CompanyDTO, CompanyDB);
            await context.SaveChangesAsync();
            return NoContent();
        }


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var existe = await context.Companies.AnyAsync(x => x.CompanyIdx == id);
        //    if (!existe) { return NotFound(); }
        //    context.Remove(new Company { CompanyIdx = id });
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}

        [HttpDelete("logicDelete/{id}/{storeId?}")]
        ////[AllowAnonymous]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Companies.AnyAsync(x => x.CompanyIdx == id);
            if (!existe) { return NotFound(); }

            var queryable = context.Stores.AsQueryable();
            //if (storeId!=Guid.Empty)
            //{
            //    queryable = queryable.Where(x => x.StoreId==storeId);
            //}

            var name2 = await context.Companies.FirstOrDefaultAsync(x => x.CompanyIdx == id);
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
        ////[AllowAnonymous]
        public async Task<IActionResult> Delete(int id, Guid? storeId)
        {
            var existe = await context.Companies.AnyAsync(x => x.CompanyIdx == id);
            if (!existe) { return NotFound(); }

            var queryable = context.Stores.AsQueryable();
            if (storeId!=Guid.Empty || storeId is not null)
            {
                queryable = queryable.Where(x => x.StoreId==storeId);
            }
            var name2  = await context.Companies.FirstOrDefaultAsync(x => x.CompanyIdx == id);
            context.Remove(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Name;
            var storeId2 = storeId;
            var tabla = "Companies";
            await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2, tabla);

            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
