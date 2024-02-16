using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
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
    public class OdrController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public OdrController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OdrDTO>>> Get(int skip, int take, Guid customerId, string searchTerm = "")
        {
            var data = await (from o in context.Odrs
                              where o.CustomerId == customerId
                              select o).AsNoTracking().ToListAsync();

            var query = data.Skip(skip).Take(take).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.CardOdr.ToLower().Contains(searchTerm)
                || c.OdrNumber.ToString().ToLower().Contains(searchTerm) );
            }

            var ntotal = query.Count();
            return Ok(new { query, ntotal });
        }

        [HttpGet("presetType")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PresetTypeDTO>>> Get()
        {
            var queryable = context.SatEstados.AsQueryable();
            var list = await queryable
                .AsNoTracking().ToListAsync();
            return mapper.Map<List<PresetTypeDTO>>(list);
        }


        [HttpGet("ValidateType")]
        [AllowAnonymous]
        public async Task<IEnumerable<OdrDTO>> Get4()
        {
            var data = await context.Odrs.ToListAsync();
            return mapper.Map<IEnumerable<OdrDTO>>(data);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] OdrDTO odrDTO, Guid customerId, Guid storeId)
        {
            var db = await context.Odrs.FirstOrDefaultAsync(x => x.CustomerId == odrDTO.CustomerId && x.CustomerId == odrDTO.CustomerId);
            var tabla = context.Model.FindEntityType(typeof(Odr)).GetTableName();

            var data = mapper.Map<Odr>(odrDTO);

            if (db is not null)
            {
                return BadRequest($"Ya existe {db.OdrNumber} ");
            }
            else
            {
                context.Add(data);
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = odrDTO.OdrNumber.ToString();
                var storeId2 = storeId;
                var Table = tabla;
                await servicioBinnacle.AddBinnacle2(usuarioId, ipUser, name, storeId2, Table);

                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
