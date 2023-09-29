using APIControlNet.Controllers;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.DTOs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CardTypeController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public CardTypeController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<CardTypeDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.CardTypes.AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            //if (storeId != Guid.Empty)
            //{
            //    queryable = queryable.Where(x => x.StoreId == storeId);
            //}
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var cardtype = await queryable.OrderByDescending(x => x.CardTypeIdx).Paginar(paginacionDTO)
                //.Include(x => x.CardType)
                //.Include(x => x.ProductStore).ThenInclude(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<CardTypeDTO>>(cardtype);
        }


        [HttpGet("Active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<CardTypeDTO>> Get2(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.CardTypes.Where(x => x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var cardtype = await queryable.OrderByDescending(x => x.CardTypeIdx).Paginar(paginacionDTO)
                //.Include(x => x.CardType)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<CardTypeDTO>>(cardtype);
        }


        [HttpGet("{id:int}", Name = "getCardType")]
        public async Task<ActionResult<CardTypeDTO>> Get(int id)
        {
            var cardType = await context.CardTypes.FirstOrDefaultAsync(x => x.CardTypeIdx == id);

            if (cardType == null)
            {
                return NotFound();
            }

            return mapper.Map<CardTypeDTO>(cardType);
        }


        //[HttpPost("{storeId?}")]
        //public async Task<ActionResult> Post([FromBody] CardTypeDTO CardTypeDTO, Guid? storeId)
        //{
        //    var existeid = await context.Cards.AnyAsync(x => x.CardId == CardTypeDTO.CardId && x.CardTypeId == CardTypeDTO.CardTypeId);

        //    var cardMap = mapper.Map<Card>(CardTypeDTO);

        //    var usuarioId = obtenerUsuarioId();
        //    var ipUser = obtenetIP();
        //    var name = cardMap.Name;
        //    var storeId2 = storeId;

        //    if (existeid)
        //    {
        //        return BadRequest($"Ya existe {CardTypeDTO.CardId} ");
        //    }
        //    else
        //    {
        //        context.Add(cardMap);

        //        await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
        //        await context.SaveChangesAsync();
        //    }
        //    return Ok();
        //}


        //[HttpPut("{storeId?}")]
        //public async Task<IActionResult> Put(CardTypeDTO CardTypeDTO, Guid storeId)
        //{
        //    var cardDB = await context.Cards.FirstOrDefaultAsync(c => c.CardIdx == CardTypeDTO.CardIdx);

        //    if (cardDB is null)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {
        //        cardDB = mapper.Map(CardTypeDTO, cardDB);

        //        var storeId2 = storeId;
        //        var usuarioId = obtenerUsuarioId();
        //        var ipUser = obtenetIP();
        //        var tableName = cardDB.Name;
        //        await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
        //        await context.SaveChangesAsync();
        //    }
        //    catch
        //    {
        //        return BadRequest($"Ya existe {CardTypeDTO.Name} ");
        //    }
        //    return NoContent();
        //}


        //[HttpDelete("logicDelete/{id}/{storeId?}")]
        //public async Task<IActionResult> logicDelete(int id, Guid storeId)
        //{
        //    var existe = await context.Cards.AnyAsync(x => x.CardIdx == id);
        //    if (!existe) { return NotFound(); }

        //    var name2 = await context.Cards.FirstOrDefaultAsync(x => x.CardIdx == id);
        //    name2.Active = false;
        //    context.Update(name2);

        //    var usuarioId = obtenerUsuarioId();
        //    var ipUser = obtenetIP();
        //    var name = name2.Name;
        //    var storeId2 = storeId;
        //    await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name, storeId2);

        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}


        //[HttpDelete("{id}/{storeId?}")]
        //public async Task<IActionResult> Delete(int id, Guid storeId)
        //{
        //    try
        //    {
        //        var existe = await context.Cards.AnyAsync(x => x.CardIdx == id);
        //        if (!existe) { return NotFound(); }

        //        var name2 = await context.Cards.FirstOrDefaultAsync(x => x.CardIdx == id);
        //        context.Remove(name2);

        //        var usuarioId = obtenerUsuarioId();
        //        var ipUser = obtenetIP();
        //        var name = name2.Name;
        //        var storeId2 = storeId;
        //        await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2);

        //        await context.SaveChangesAsync();
        //        return NoContent();
        //    }
        //    catch
        //    {
        //        return BadRequest("ERROR DE DATOS RELACIONADOS");
        //    }
        //}
    }
}
