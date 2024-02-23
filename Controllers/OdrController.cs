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
        public async Task<ActionResult<IEnumerable<OdrDTO>>> Get(int skip, int take, Guid customerId, Guid vehicleiD, string searchTerm = "")
        {
            var data = await (from o in context.Odrs
                              where o.CustomerId == customerId && o.VehicleId == vehicleiD
                              select o).AsNoTracking().ToListAsync();

            var query = data.Skip(skip).Take(take).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.CardOdr.ToLower().Contains(searchTerm)
                || c.OdrNumber.ToString().ToLower().Contains(searchTerm));
            }

            var ntotal = query.Count();
            return Ok(new { query, ntotal });
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] OdrDTO odrDTO, Guid storeId)
        {
            var db = await context.Odrs.FirstOrDefaultAsync(x => x.CustomerId == odrDTO.CustomerId && x.VehicleId == odrDTO.VehicleId
            && x.OdrNumber == odrDTO.OdrNumber);
            if (db is not null)
            {
                return BadRequest($"Ya existe {db.OdrNumber} ");
            }

            var tabla = context.Model.FindEntityType(typeof(Odr)).GetTableName();

            var cusNumDb = await context.Customers.FirstOrDefaultAsync(x => x.CustomerId == odrDTO.CustomerId);
            var customerNumber = cusNumDb.CustomerNumber;

            var productDb = await context.Products.FirstOrDefaultAsync(x => x.ProductId == odrDTO.ProductId);
            var productCode = productDb.ProductCode;

            var cuslimitDb = await context.CustomerLimits.Where(x => x.CustomerId == odrDTO.CustomerId).FirstOrDefaultAsync();
            var folio = cuslimitDb.FolioOdrNumber;
            if (folio is null) { folio = 0; }
            folio++;

            var data = mapper.Map<Odr>(odrDTO);
            //Odr data = new();
            data.OdrId = Guid.NewGuid();
            data.OdrNumber = folio++;
            data.CardOdr = "17" + customerNumber.ToString("00000000") + data.OdrNumber?.ToString("D7");
            data.NetgroupNetId = odrDTO.NetgroupNetId;
            data.CustomerId = odrDTO.CustomerId;
            data.VehicleId = odrDTO.VehicleId;
            data.ProductId = odrDTO.ProductId;
            data.ProductCode = productCode;
            data.PresetType = odrDTO.PresetType;
            data.PresetQuantity = odrDTO.PresetQuantity;
            context.Odrs.Add(data);

            cuslimitDb.FolioOdrNumber = data.OdrNumber;
            context.Update(cuslimitDb);

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
