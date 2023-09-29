using APIControlNet.DTOs;
using APIControlNet.Models;
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
    public class StoreHouseMovementController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public StoreHouseMovementController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<StoreHouseMovementDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.StoreHouseMovements.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var storeHouseMovements = await queryable.AsNoTracking().OrderByDescending(x => x.StoreHouseMovementIdx)
                .Select(x => new
                {
                    StoreHouseMovementIdx = x.StoreHouseMovementIdx,
                    StoreHouseMovementId = x.StoreHouseMovementId,
                    Name = x.TypeMovementIdxNavigation.Name,
                    Date = x.Date,
                    NameStoreHouse = x.StoreHouseIdDestinationNavigation.Name,
                }).Paginar(paginacionDTO).AsNoTracking()
                .ToListAsync();

            return Ok(storeHouseMovements);
            //return mapper.Map<List<StoreHouseMovementDTO>>(storeHouseMovements);
        }


        [HttpGet("agrupaAlmacenProducto")]
        public async Task<ActionResult> Get()
        {
            var movimientosAgrupados = await context.StoreHouseMovementDetails.GroupBy(p => new
            {
                StoreHouseIdDestination = (Guid)p.StoreHouseMovement.StoreHouseIdDestination,
                ProductId = (Guid)p.ProductId,

            })
                .Select(x => new
                {
                    StoreHouseIdDestination = x.Key,
                    ProducId = x.Key,
                    Quantity = x.Sum(p => p.QuantityEntry)
                })            
                .ToListAsync();
            return Ok(movimientosAgrupados);
        }


        [HttpPost]
        //[AllowAnonymous]
        public async Task<ActionResult> Post([FromBody]StoreHouseMovementDTO almMovCrearDTO)
        {
            var almMovCrear = mapper.Map<StoreHouseMovement>(almMovCrearDTO);

            context.Add(almMovCrear);

            //var productosAgrupados = context.StoreHouseMovementDetails.GroupBy(p => new
            //{
            //    StoreHouseIdDestination = (Guid)p.StoreHouseMovement.StoreHouseIdDestination,
            //    ProductId = (Guid)p.ProductId
            //})
            //    .Select(p => new
            //    {
            //        StoreHouseIdDestination = p.Key,
            //        ProductId = p.Key,
            //        Quantity = p.Sum(p => p.Quantity)
            //    }).ToList();

            ////var almacendetalle = context.StoreHouseDetails.Select(p => new { AlmacenDestino = p.StoreHouseIdDestination, Productoid = p.ProductId }).ToList();

            //foreach (var product in productosAgrupados)
            //{
            //    await context.AddRangeAsync(new StoreHouseDetail()
            //    {
            //        StoreHouseIdDestination = product.StoreHouseIdDestination.StoreHouseIdDestination.ToString(),
            //        ProductId = product.ProductId.ProductId.ToString(),
            //        Quantity = product.Quantity
            //    });
            //}

            //foreach (var product in productosAgrupados)
            //{
            //    await context.AddRangeAsync(new StoreHouseDetail()
            //    {
            //        StoreHouseIdDestination = product.StoreHouseIdDestination.StoreHouseIdDestination.ToString(),
            //        ProductId = product.ProductId.ProductId.ToString(),
            //        Quantity = product.Quantity
            //    });
            //    //var existeproduct = await context.StoreHouseDetails.AnyAsync(p => p.ProductId == product.ProductId.ProductId.ToString());
            //    //if (existeproduct)
            //    //{
            //    //    return BadRequest($"Ya existe producto con Guid { product.ProductId.ProductId.ToString()}");
            //    //}
            //    //else
            //    //{
            //    //    await context.AddRangeAsync(new StoreHouseDetail()
            //    //    {
            //    //        StoreHouseIdDestination = product.StoreHouseIdDestination.StoreHouseIdDestination.ToString(),
            //    //        ProductId = product.ProductId.ProductId.ToString(),
            //    //        Quantity = product.Quantity
            //    //    });
            //    //}
            //}

            await context.SaveChangesAsync();
            return Ok();
        }
    }
}

