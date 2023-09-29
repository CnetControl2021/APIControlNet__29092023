using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LoadPosResController : CustomBaseController
    {
        private readonly CnetCoreContext context;

        private readonly IMapper mapper;

        public LoadPosResController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        //[HttpGet]
        //public async Task<ActionResult<LoadPositionResponseDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre, Guid storeId)
        //{
        //    var queryable = context.LoadPositionResponses.AsQueryable();
        //    if (!string.IsNullOrEmpty(nombre))
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
        //    }
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var loadPositionResponses = await queryable.Paginar(paginacionDTO)
        //        .AsNoTracking().ToListAsync();
        //    return Ok(loadPositionResponses);
        //}


        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<List<LoadPositionResponseDTO>> Get(Guid? storeId)
        //{
        //    var queryable = context.LoadPositionResponses.AsQueryable().AsNoTracking();
        //    if (storeId != Guid.Empty)
        //    {
        //        queryable = queryable.Where(x => x.StoreId == storeId);
        //    }
        //    var lpr = await queryable
        //        .Include(x => x.StatusDispenserIdiNavigation).AsNoTracking().ToListAsync();
        //    return mapper.Map<List<LoadPositionResponseDTO>>(lpr);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ActionResult<LoadPositionResponseDTO>> Get(Guid? storeId)
        //{
        //    var datos2 = await context.Products.Select(pd => new { pd = pd }).ToListAsync();
        //    var datos = await context.LoadPositionResponses.Select(p => 
        //    new
        //    {
        //        idx = p.LoadPositionResponseIdx,
        //        p.LoadPositionIdi,
        //        p.HoseIdi,
        //        p.Quantity,
        //        amount = p.Amount,
        //        stdisp = p.StatusDispenserIdiNavigation.Description,
        //        p.StatusDispenserIdiNavigation.ColorStatus
        //    }).ToListAsync();
        //    return Ok(datos);
        //}

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<LoadPositionResponseDTO>> Get5(Guid storeId)
        {
            List<LoadPositionResponseDTO> list = new List<LoadPositionResponseDTO>();
            if (storeId != Guid.Empty)
            {
                list = await (from lpr in context.LoadPositionResponses
                              join pd in context.Products on lpr.ProductId equals pd.ProductId
                              join sd in context.StatusDispensers on lpr.StatusDispenserIdi equals sd.StatusDispenserIdi
                              where lpr.StoreId == storeId

                              select new LoadPositionResponseDTO
                              {
                                  LoadPositionResponseIdx = lpr.LoadPositionResponseIdx,
                                  LoadPositionIdi = lpr.LoadPositionIdi,
                                  HoseIdi = lpr.HoseIdi,
                                  Quantity = lpr.Quantity,
                                  Amount = lpr.Amount,
                                  Price = lpr.Price,
                                  Description = sd.Description,
                                  ColorStatus = sd.ColorStatus,
                                  Productname = pd.Name
                              }).AsNoTracking().ToListAsync();
            }
            else
            {
                list = await (from lpr in context.LoadPositionResponses
                              join pd in context.Products on lpr.ProductId equals pd.ProductId
                              join sd in context.StatusDispensers on lpr.StatusDispenserIdi equals sd.StatusDispenserIdi

                              select new LoadPositionResponseDTO
                              {
                                  LoadPositionResponseIdx = lpr.LoadPositionResponseIdx,
                                  LoadPositionIdi = lpr.LoadPositionIdi,
                                  HoseIdi = lpr.HoseIdi,
                                  Quantity = lpr.Quantity,
                                  Amount = lpr.Amount,
                                  Price = lpr.Price,
                                  Description = sd.Description,
                                  ColorStatus = sd.ColorStatus,
                                  Productname = pd.Name
                              }).AsNoTracking().ToListAsync();
            }
            return list;
            //return mapper.Map<IEnumerable<LoadPositionResponseDTO>>(list);
        }


        //[HttpGet("{id:int}", Name = "obtenerLoadPositionsRes")]
        //public async Task<ActionResult<LoadPositionResponseDTO>> Get(int id)
        //{
        //    var loadPositionResponses = await context.LoadPositionResponses.FirstOrDefaultAsync(x => x.LoadPositionResponseIdx == id);

        //    if (loadPositionResponses == null)
        //    {
        //        return NotFound();
        //    }
        //    return mapper.Map<LoadPositionResponseDTO>(loadPositionResponses);
        //}


        //[HttpPost]
        //public async Task<ActionResult> Post([FromBody] LoadPositionResponseDTO LoadPositionResponseDTO)
        //{
        //    var existeid = await context.LoadPositionResponses.AnyAsync(x => x.LoadPositionResponseIdx == LoadPositionResponseDTO.LoadPositionResponseIdx);

        //    var loadPositionResponses = mapper.Map<LoadPositionResponse>(LoadPositionResponseDTO);

        //    if (existeid)
        //    {
        //        context.Update(loadPositionResponses);
        //        await context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        var existe = await context.LoadPositionResponses.AnyAsync(x => x.LoadPositionResponseIdx == LoadPositionResponseDTO.LoadPositionIdi);

        //        if (existe)
        //        {
        //            return BadRequest($"Ya existe {LoadPositionResponseDTO.LoadPositionIdi} en esa sucursal ");
        //        }
        //        else
        //        {
        //            context.Add(loadPositionResponses);
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //    var LoadPositionResponseDTO2 = mapper.Map<LoadPositionResponseDTO>(loadPositionResponses);
        //    return CreatedAtRoute("obtenerLoadPositionsRes", new { id = loadPositionResponses.LoadPositionResponseIdx }, LoadPositionResponseDTO2);
        //}


        //[HttpPut]
        //public async Task<IActionResult> Put(LoadPositionResponseDTO LoadPositionResponseDTO)
        //{
        //    var loadPosResDB = await context.LoadPositionResponses.FirstOrDefaultAsync(c => c.LoadPositionResponseIdx == LoadPositionResponseDTO.LoadPositionResponseIdx);

        //    if (loadPosResDB is null)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {
        //        loadPosResDB = mapper.Map(LoadPositionResponseDTO, loadPosResDB);
        //        await context.SaveChangesAsync();

        //    }
        //    catch
        //    {
        //        return BadRequest($"Ya existe puerto {LoadPositionResponseDTO.LoadPositionIdi} en esa sucursal ");
        //    }
        //    return NoContent();
        //}


        //[HttpGet("byStore/{id2}")] //por store
        //public async Task<ActionResult<List<LoadPositionResponseDTO>>> Get([FromRoute] Guid id2)
        //{
        //    var RSs = await context.LoadPositionResponses.Where(e => e.StoreId.Equals((Guid)id2))
        //        //.Include(x => x.Store)
        //        .ToListAsync();
        //    return mapper.Map<List<LoadPositionResponseDTO>>(RSs);
        //}


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var existe = await context.LoadPositionResponses.AnyAsync(x => x.LoadPositionResponseIdx == id);

        //    if (!existe)
        //    {
        //        return NotFound();
        //    }

        //    context.Remove(new LoadPositionResponse { LoadPositionResponseIdx = id });
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}

    }
}
