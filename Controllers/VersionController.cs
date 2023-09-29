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
    public class VersionController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public VersionController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<VersionDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.Versions.Where(x => x.Active == true && x.Deleted == false && x.SystemId == 3).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Description.ToLower().Contains(nombre));
            }
            //if (storeId != Guid.Empty)
            //{
            //    queryable = queryable.Where(x => x.StoreId == storeId);
            //}
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var version = await queryable.Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<VersionDTO>>(version);
        }

        [HttpGet("routeHashFDll")]
        [AllowAnonymous]
        public async Task<IActionResult> Get5(string hashFDll)
        {
            var hashdb = await context.Versions.FirstOrDefaultAsync(x => x.Hash512 == hashFDll);
            //var hashsb2 = hashdb.Hash512;


            var db1 = await context.Versions.FirstOrDefaultAsync(y => y.VersionId == "1.0");
            if (db1 is null)
            {
                await context.Database.ExecuteSqlInterpolatedAsync
                ($@"INSERT INTO version (system_id, version_id, revision_id, user_name, user_name_check, description, version_date, updated, active, locked, deleted) 
                VALUES({"3"}, {"1.0"}, {"1.0"}, {"Control Volumetrico"}, {"ControlNet"}, {"Version inicial"}, {"2023-05-01"}, {"2023-05-01"}, {true}, {false}, {false})");
            }
            var db2 = await context.Versions.FirstOrDefaultAsync(y => y.VersionId == "1.1");
            if (db2 is null)
            {
                await context.Database.ExecuteSqlInterpolatedAsync
                ($@"INSERT INTO version (system_id, version_id, revision_id, user_name, user_name_check, description, version_date, updated, active, locked, deleted) 
                VALUES({"3"}, {"1.1"}, {"1.1"}, {"Control Volumetrico"}, {"ControlNet"}, {"Mejoras en reportes"}, {"2023-05-12"}, {"2023-05-01"}, {true}, {false}, {false})");
            }
            var db3 = await context.Versions.FirstOrDefaultAsync(y => y.VersionId == "2.0");
            if (db3 is null)
            {
                await context.Database.ExecuteSqlInterpolatedAsync
                ($@"INSERT INTO version (system_id, version_id, revision_id, user_name, user_name_check, description, version_date, updated, active, locked, deleted) 
                VALUES({"3"}, {"2.0"}, {"2.0"}, {"Control Volumetrico"}, {"ControlNet"}, {"Cambios en campos de medicion TC"}, {"2023-06-12"}, {"2023-06-12"}, {true}, {false}, {false})");
            }
            var db4 = await context.Versions.FirstOrDefaultAsync(y => y.VersionId == "2.1");
            if (db4 is null)
            {
                await context.Database.ExecuteSqlInterpolatedAsync
                ($@"INSERT INTO version (system_id, version_id, revision_id, user_name, user_name_check, description, version_date, updated, active, locked, deleted) 
                VALUES({"3"}, {"2.1"}, {"2.1"}, {"Control Volumetrico"}, {"ControlNet"}, {"Adicion de pagina para revisar version he historial"}, {"2023-06-20"}, {"2023-06-20"}, {true}, {false}, {false})");
            }
            var db5 = await context.Versions.FirstOrDefaultAsync(y => y.VersionId == "2.2");
            if (db5 is null)
            {
                await context.Database.ExecuteSqlInterpolatedAsync
                ($@"INSERT INTO version (system_id, version_id, revision_id, user_name, user_name_check, description, hash_512, version_date, updated, active, locked, deleted) 
                VALUES({"3"}, {"2.2"}, {"2.2"}, {"Control Volumetrico"}, {"ControlNet"}, 
                {"Se agrego identificador unico, (hash-512) para comprobar  autenticidad e integridad de la version, encabezado de empresa, y version, logo controlnet. Se agrego en las ventas, la inofrmacion con coeficiente de temperatura. "}, 
                {"98f518677af7e918bcbc36d31de6a6790c5f71548c4f33e82341e125effd623ca8d07f22a9621c0ba7441d8b576439e3e5e4dc90e0d2a6e94d5e43207e0aec17"},
                {"2023-07-11"}, {"2023-07-11"}, {true}, {false}, {false})");
            }


            if (hashdb == null)
            {
                return NotFound();
            }
            return Ok(hashdb);
        }
    }
}
