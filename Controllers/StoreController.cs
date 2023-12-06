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
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[AutoValidateAntiforgeryToken]
    public class StoreController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ServicioBinnacle servicioBinnacle;

        public StoreController(CnetCoreContext context, IMapper mapper, UserManager<IdentityUser> userManager,
                ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.servicioBinnacle = servicioBinnacle;
        }

        public const string gEmpty = "00000000-0000-0000-0000-000000000000";
        string myGuidString = gEmpty.ToString();

        [HttpGet("activeSinPag")]
        [AllowAnonymous]
        public async Task<IEnumerable<StoreDTO>> Get([FromQuery] string nombre, Guid storeId)
        {
            var queryable = context.Stores.Where(x => x.Active == true && x.Deleted == false && x.StoreId.ToString() != myGuidString).AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var stores = await queryable
                .Include(x => x.Company)
                .ToListAsync();
            return mapper.Map<List<StoreDTO>>(stores);
        }

        [HttpGet("hashFDll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHash512Async()
        {
            string inputok = @"C:\inetpub\appCnet\wwwroot\_framework\FrontCnet.dll";   //windows dll
            string inputok2 = "/var/www/blazorapp/wwwroot/_framework/FrontCnet.dll";     //linux dll 
            //string inputok = "C:\\inetpub\\appCnet\\wwwroot\\_framework\\FrontCnet.dll";
            //var inputok2 = inputok.ToString();

            if (System.IO.File.Exists(inputok))
            {
                // El archivo existe
                Console.WriteLine(inputok + "Si exite archivo en ruta");
                using (var fileStream = new FileStream(inputok, FileMode.Open))
                {
                    using (var sha512 = SHA512.Create())
                    {
                        byte[] hashBytes = sha512.ComputeHash(fileStream);
                        string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                        return await Task.FromResult<IActionResult>(Ok(hashString));
                    }
                }
            }
            if (System.IO.File.Exists(inputok2))
            {
                // El archivo existe
                Console.WriteLine(inputok2 + "Si exite archivo en ruta");
                using (var fileStream = new FileStream(inputok2, FileMode.Open))
                {
                    using (var sha512 = SHA512.Create())
                    {
                        byte[] hashBytes = sha512.ComputeHash(fileStream);
                        string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                        return await Task.FromResult<IActionResult>(Ok(hashString));
                    }
                }
            }
            return Ok();
        }


        [HttpGet("activeSinPag2/{idCom?}")]
        [AllowAnonymous]
        public async Task<IEnumerable<StoreDTO>> Get2([FromQuery] string nombre, Guid storeId, Guid idCom)
        {
            if (idCom != Guid.Empty)
            {
                var queryable = context.Stores.Where(x => x.Active == true && x.Deleted == false && x.StoreId.ToString() != myGuidString
                  && x.CompanyId == idCom).AsQueryable();

                if (!string.IsNullOrEmpty(nombre))
                {
                    queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
                }
                if (storeId != Guid.Empty)
                {
                    queryable = queryable.Where(x => x.StoreId == storeId);
                }
                var stores = await queryable
                    .Include(x => x.Company)
                    .ToListAsync();
                return mapper.Map<List<StoreDTO>>(stores);

            }
            else
            {
                var queryable = context.Stores.Where(x => x.Active == true && x.Deleted == false && x.StoreId.ToString() != myGuidString
                 ).AsQueryable();

               // var queryable = context.Stores.AsQueryable();

                var stores = await queryable
                    .Include(x => x.Company)
                    .ToListAsync();
                //return (IEnumerable<StoreDTO>)Ok(stores);
                return mapper.Map<List<StoreDTO>>(stores);
            }
        }


        [HttpGet("{storeId?}")]
        //[AllowAnonymous]
        public async Task<IEnumerable<StoreDTO>> Get2(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Stores.Where(x => x.StoreId.ToString() != myGuidString).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var stores = await queryable.Paginar(paginacionDTO)
                .Include(x => x.Company)
                .Include(x => x.StoreAddresses)
                .Include(x => x.StoreSat)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<StoreDTO>>(stores);
        }


        [HttpGet("firstList")]
        //[AllowAnonymous]
        public async Task<IEnumerable<StoreDTO>> Get4(Guid companyId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Stores.Where(x => x.StoreId.ToString() != myGuidString).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (companyId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.CompanyId.ToString().Contains(companyId.ToString()));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var stores = await queryable.Paginar(paginacionDTO)
                .Include(x => x.Company)
                .Include(x => x.StoreAddresses)
                .Include(x => x.StoreSat)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<StoreDTO>>(stores);
        }



        [HttpGet("active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<StoreDTO>> Get3(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Stores.Where(x => x.Active == true && x.StoreId.ToString() != myGuidString
            && x.StoreId == storeId).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var store = await queryable.OrderByDescending(x => x.StoreIdx).Paginar(paginacionDTO)
                .Include(x => x.Company)
                .Include(x => x.StoreAddresses)
                .Include(x => x.StoreSat)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<StoreDTO>>(store);
        }


        [HttpGet("{id:int}", Name = "obtenerStore")]
        ////[AllowAnonymous]
        public async Task<ActionResult<StoreDTO>> Get5(int id)
        {
            var Store = await context.Stores.FirstOrDefaultAsync(x => x.StoreIdx == id);

            if (Store == null)
            {
                return NotFound();
            }
            return mapper.Map<StoreDTO>(Store);
        }


        [HttpGet("byStore/{storeId?}")] //por store
        [AllowAnonymous]
        public async Task<ActionResult<StoreDTO>> Get4(Guid? storeId)
        {
            var Store = await context.Stores.FirstOrDefaultAsync(x => x.StoreId == storeId);
            //var Storedb = await context.Companies.FirstOrDefaultAsync(y => y.StoreId == storeId);

            if (Store == null)
            {
                return NotFound();
            }
            return mapper.Map<StoreDTO>(Store);
        }


        //[HttpGet("{nombre}")]
        //public async Task<ActionResult<List<StoreDTO>>> Get([FromRoute] string nombre)
        //{
        //    var RSs = await context.Stores.Where(storedb => storedb.Name.Contains(nombre)).ToListAsync();

        //    return mapper.Map<List<StoreDTO>>(RSs);
        //}


        [HttpPost("{storeId?}")]
        public async Task<ActionResult> Post([FromBody] StoreDTO storeDTO, Guid? storeId)
        {

            var existeid = await context.Stores.AnyAsync(x => x.StoreNumber == storeDTO.StoreNumber && x.CompanyId == storeDTO.CompanyId);
            var store = mapper.Map<Store>(storeDTO);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = store.Name;
            var storeId2 = storeId;

            if (existeid)
            {
                return BadRequest($"Ya existe sucursal {storeDTO.StoreNumber} en esa empresa ");
            }
            else
            {
                context.Add(store);

                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
            }
            var storeDTO2 = mapper.Map<StoreDTO>(store);
            return CreatedAtRoute("obtenerStore", new { id = store.StoreIdx }, storeDTO2);
        }


        [HttpPut("{storeId?}")]
        ////[AllowAnonymous]
        public async Task<ActionResult> Put(StoreDTO storeDTO, Guid storeId)
        {
            var storeDB = await context.Stores.FirstOrDefaultAsync(c => c.StoreIdx == storeDTO.StoreIdx);

            if (storeDB is null)
            {
                return NotFound();
            }
            try
            {
                storeDB = mapper.Map(storeDTO, storeDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = storeDB.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Ya existe sucursal {storeDTO.StoreNumber} en esa empresa ");
            }
        }


        //[HttpPatch("{id:int}")]
        //public async Task<IActionResult> Patch(int id, JsonPatchDocument<StoreDTO> patchDocument)
        //{
        //    if (patchDocument == null)
        //    {
        //        return BadRequest();
        //    }

        //    var StoreDB = await context.Stores.FirstOrDefaultAsync(x => x.StoreIdx == id);

        //    if (StoreDB == null)
        //    {
        //        return NotFound();
        //    }

        //    var StoreDTO = mapper.Map<StoreDTO>(StoreDB);

        //    patchDocument.ApplyTo(StoreDTO, ModelState);

        //    var esValido = TryValidateModel(StoreDTO);
        //    if (!esValido)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    mapper.Map(StoreDTO, StoreDB);
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.Stores.AnyAsync(x => x.StoreIdx == id);
            if (!existe) { return NotFound(); }

            var queryable = context.Stores.AsQueryable();
            //if (storeId!=Guid.Empty)
            //{
            //    queryable = queryable.Where(x => x.StoreId==storeId);
            //}

            var name2 = await context.Stores.FirstOrDefaultAsync(x => x.StoreIdx == id);
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

        //[HttpDelete("{id}/{storeId?}")]
        //////[AllowAnonymous]
        //public async Task<IActionResult> Delete(int id, Guid storeId)
        //{
        //    var existe = await context.Stores.AnyAsync(x => x.StoreIdx == id);
        //    if (!existe) { return NotFound(); }

        //    var queryable = context.Stores.AsQueryable();
        //    //if (storeId!=Guid.Empty)
        //    //{
        //    //    queryable = queryable.Where(x => x.StoreId==storeId);
        //    //}
        //    var name2 = await context.Stores.FirstOrDefaultAsync(x => x.StoreIdx == id);
        //    context.Remove(name2);

        //    var usuarioId = obtenerUsuarioId();
        //    var ipUser = obtenetIP();
        //    var name = name2.Name;
        //    var storeId = storeId;
        //    await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId);

        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}


        [HttpDelete("{id}/{storeId?}")]
        ////[AllowAnonymous]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            try
            {
                var existe = await context.Stores.AnyAsync(x => x.StoreIdx == id);
                if (!existe) { return NotFound(); }

                var queryable = context.Stores.AsQueryable();
                //if (storeId!=Guid.Empty)
                //{
                //    queryable = queryable.Where(x => x.StoreId==storeId);
                //}
                var name2 = await context.Stores.FirstOrDefaultAsync(x => x.StoreIdx == id);
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
                return NotFound("ERROR DE DATOS RELACIONADOS");
            }

        }
    }
}

