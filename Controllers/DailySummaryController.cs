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
using System.Linq;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DailySummaryController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public DailySummaryController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        //[HttpGet]
        //[Route("sinPag")]
        //[AllowAnonymous]
        //public async Task<IEnumerable<DailySummaryDTO>> Get5(Guid storeId, DateTime dateIni, DateTime dateFin)
        //{
        //    //var queryable = context.DailySummaries.Where(x => x.Date <= dateFin && x.Date >= dateIni && x.Active ==true)
        //    //    .OrderByDescending(x => x.DailySummaryIdx).AsQueryable().AsNoTracking();
        //    var queryable = context.DailySummaries.Where(x => x.Date <= dateFin && x.Date >= dateIni)
        //        .OrderByDescending(x => x.Date).AsQueryable().AsNoTracking();
        //    if (storeId != Guid.Empty)
        //    {
        //        queryable = queryable.Where(x => x.StoreId == storeId);
        //    }
        //    var dailySum = await queryable
        //        .Include(x => x.Product.ProductStores).ThenInclude(x => x.Color)
        //        .AsNoTracking().ToListAsync();
        //    return mapper.Map<List<DailySummaryDTO>>(dailySum);
        //}

        [HttpGet]
        [Route("sinPag")]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DailySummaryDTO>>> Get5(Guid storeId, DateTime dateIni, DateTime dateFin)
        {
             var list = await (from ds in context.DailySummaries
                               where ds.StoreId == storeId 
                               join pd in context.Products on ds.ProductId equals pd.ProductId
                               join pds in context.ProductStores on pd.ProductId equals pds.ProductId
                               where pds.StoreId == storeId
                               && ds.Date >= (dateIni) && ds.Date <= dateFin

                              select new DailySummaryDTO
                              {
                                  DailySummaryIdx = ds.DailySummaryIdx,
                                  ProductId = ds.ProductId,
                                  StoreId = ds.StoreId,
                                  Date = ds.Date,                               
                                  ProductName = pd.Name,
                                  StartInventoryQuantity = ds.StartInventoryQuantity,
                                  InventoryInQuantity = ds.InventoryInQuantity,
                                  SaleSample = ds.SaleSample,
                                  TheoryInventoryQuantity = ds.TheoryInventoryQuantity,
                                  EndInventoryQuantity = ds.EndInventoryQuantity,
                                  InventoryDifference = ds.InventoryDifference,
                                  InventoryDifferencePercentage = ds.InventoryDifferencePercentage,
                                  SaleQuantity = ds.SaleQuantity,
                                  ProductColor = pds.Color
                              }).AsNoTracking().OrderByDescending(x => x.Date).ToListAsync();
            return list;
        }



        //[HttpGet("{id:int}", Name = "getPointsale")]
        //public async Task<ActionResult<DailySummaryDTO>> Get(int id)
        //{
        //    var pointsale = await context.PointSales.FirstOrDefaultAsync(x => x.PointSaleIdx == id);

        //    if (pointsale == null)
        //    {
        //        return NotFound();
        //    }

        //    return mapper.Map<DailySummaryDTO>(pointsale);
        //}


        //[HttpPost("{storeId?}")]
        //public async Task<ActionResult> Post([FromBody] DailySummaryDTO DailySummaryDTO, Guid? storeId)
        //{
        //    var existeid = await context.PointSales.AnyAsync(x => x.PointSaleIdi == DailySummaryDTO.PointSaleIdi && x.StoreId == DailySummaryDTO.StoreId);

        //    var pointsaleMap = mapper.Map<PointSale>(DailySummaryDTO);

        //    var usuarioId = obtenerUsuarioId();
        //    var ipUser = obtenetIP();
        //    var name = pointsaleMap.Name;
        //    var storeId2 = storeId;

        //    if (existeid)
        //    {
        //        return BadRequest($"Ya existe {DailySummaryDTO.StoreId} en esa empresa");
        //    }
        //    else
        //    {
        //        context.Add(pointsaleMap);

        //        await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
        //        await context.SaveChangesAsync();
        //    }
        //    return Ok();
        //    //var storeDTO2 = mapper.Map<DailySummaryDTO>(employeeMap);
        //    //return CreatedAtRoute("getEmployee", new { id = employeeMap.EmployeeId }, storeDTO2);
        //}


        //[HttpPut("{storeId?}")]
        //public async Task<IActionResult> Put(DailySummaryDTO DailySummaryDTO, Guid storeId)
        //{
        //    var pointsaleDB = await context.PointSales.FirstOrDefaultAsync(c => c.PointSaleIdx == DailySummaryDTO.PointSaleIdx);

        //    if (pointsaleDB is null)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {
        //        pointsaleDB = mapper.Map(DailySummaryDTO, pointsaleDB);

        //        var storeId2 = storeId;
        //        var usuarioId = obtenerUsuarioId();
        //        var ipUser = obtenetIP();
        //        var tableName = pointsaleDB.Name;
        //        await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
        //        await context.SaveChangesAsync();
        //    }
        //    catch
        //    {
        //        return BadRequest($"Ya existe {DailySummaryDTO.Name} ");
        //    }
        //    return NoContent();
        //}


        //[HttpDelete("logicDelete/{id}/{storeId?}")]
        //public async Task<IActionResult> logicDelete(int id, Guid storeId)
        //{
        //    var existe = await context.PointSales.AnyAsync(x => x.PointSaleIdx == id);
        //    if (!existe) { return NotFound(); }

        //    var name2 = await context.PointSales.FirstOrDefaultAsync(x => x.PointSaleIdx == id);
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
        //        var existe = await context.PointSales.AnyAsync(x => x.PointSaleIdx == id);
        //        if (!existe) { return NotFound(); }

        //        var name2 = await context.PointSales.FirstOrDefaultAsync(x => x.PointSaleIdx == id);
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
