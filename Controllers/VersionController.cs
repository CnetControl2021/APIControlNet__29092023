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
using System;


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
            var db6 = await context.Versions.FirstOrDefaultAsync(y => y.VersionId == "2.3");

            if (db6 is null)
            {
                await context.Database.ExecuteSqlInterpolatedAsync
                ($@"INSERT INTO version (system_id, version_id, revision_id, user_name, user_name_check, description, hash_512, version_date, updated, active, locked, deleted) 
                VALUES({"3"}, {"2.3"}, {"2.3"}, {"Control Volumetrico"}, {"ControlNet"}, 
                {"Se mejoro el sistema de notificaciones de alarmas. Se adiciono modulo de compras y ventas transportistas"}, 
                {"147d01fffed9305cd31e1bf1d5c81f769a97386651d1c914e0863a0d31f74bdff60a1cdf21119ef086ad2763a723566f77d817a3ab44415866abb70006bcef85"},
                {"2023-11-29"}, {"2023-11-29"}, {true}, {false}, {false})");
            }

            var db7 = await context.Versions.FirstOrDefaultAsync(y => y.VersionId == "2.4");

            if (db7 is null)
            {
                await context.Database.ExecuteSqlInterpolatedAsync
                ($@"INSERT INTO version (system_id, version_id, revision_id, user_name, user_name_check, description, hash_512, version_date, updated, active, locked, deleted) 
                VALUES({"3"}, {"2.4"}, {"2.4"}, {"Control Volumetrico"}, {"ControlNet"}, 
                {"Se agregaron catalogos para el manejo de clientes y proveedores. Se agrego contol para complemento de Json SAT"}, 
                {"16c8aa26b872f23980d2ff47ea3c97eb04abeb1f1ff23a8ec977bf9b4fd46e55857ba59a606be2e724418aeb292bf9bf96f093b142ac9a581cea91ce2e087bc3"},
                {"2024-01-15"}, {"2024-01-15"}, {true}, {false}, {false})");
            }
            else
            {
                var db = await context.Versions.FirstOrDefaultAsync(x => x.VersionId == "2.4");
                db.Updated = DateTime.Now;
                db.Hash512 = "41e0b8e0ba9d49abeea38f194a41de7c4b11c19798fe4f8331bb3dd7d9be37ddacdd1033760e7f358fbeed70a21db7475f5be4b7e6b30af9be6527dd13e82e93";
                context.Update(db);
                context.SaveChanges();
            }

            var hashdb = await context.Versions.FirstOrDefaultAsync(x => x.Hash512 == hashFDll);

            if (hashdb == null)
            {
                return NotFound();
            }
            return Ok(hashdb);
        }
    }
}
