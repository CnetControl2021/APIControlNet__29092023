using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreHouseMovementDetailController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;

        public StoreHouseMovementDetailController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<StoreHouseMovementDetailDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.StoreHouseMovementDetails.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var storeHouseMovementsDetails = await queryable.Paginar(paginacionDTO)
                .ToListAsync();
            return mapper.Map<List<StoreHouseMovementDetailDTO>>(storeHouseMovementsDetails);
        }

        //[HttpGet]
        ////[AllowAnonymous]
        //public async Task<ActionResult<StoreHouseMovementDetailDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
        //{
        //    var queryable = context.StoreHouseMovementDetails.AsQueryable();
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var storeHouseMovementsDetails = await queryable
        //        .Select(x => new
        //        {
        //            StoreHouseMovementDetailIdx = x.StoreHouseMovementDetailIdx,
        //            StoreHouseMovementId = x.StoreHouseMovementId,
        //            ProductId = x.ProductId,
        //            ProductName = x.Product.Name,
        //            Quantity = x.Quantity,
        //            Location = x.Location
        //        }).Paginar(paginacionDTO)
        //        .ToListAsync();
        //    return Ok(storeHouseMovementsDetails);
        //}



        [HttpGet("inventory/{id}")]
        public async Task<ActionResult<StoreHouseMovementDetailDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, Guid id)
        {
            var queryable = context.StoreHouseMovementDetails.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var movimientosAgrupados = await context.StoreHouseMovementDetails.AsNoTracking()
                //.Include(db => db.Product)
                .Where(x => x.StoreHouseMovement.StoreHouseIdDestination == id)
            .GroupBy(p => p.ProductId)
            .Select(x => new
            {
                ProductId = x.Key,
                Quantity = x.Sum(p => p.QuantityEntry),
                //Name = x.Select(db => db.Product.Name).FirstOrDefault()
            }).Paginar(paginacionDTO).AsNoTracking()
            .ToListAsync();

            return Ok(movimientosAgrupados);
        }


        [HttpGet("detalleMov/{id2}")] //detalle de mov
        public async Task<ActionResult<StoreHouseMovementDetailDTO>> Get2(Guid id2)
        {
            var movimientosAgrupados2 = await context.StoreHouseMovementDetails.AsNoTracking()
                .Where(x => x.StoreHouseMovementId == id2)
            .Select(x => new
            {
                StoreHouseMovementId = x.StoreHouseMovementId,
                Product = x.ProductId,
                //Name = x.Product.Name,
                Quantity = x.QuantityEntry
            }).AsNoTracking()
            .ToListAsync();
            return Ok(movimientosAgrupados2);
        }

        //return mapper.Map<List<StoreHouseMovementDetailDTO>>(movimientosAgrupados);


        //[HttpGet("{id}")] // buscar las por id de almacen destino en otra entidad  asnotracking
        //public async Task<IEnumerable<StoreHouseMovementDetailDTO>> Get([FromQuery] PaginacionDTO paginacionDTO, Guid id)
        //{
        //    var queryable = context.StoreHouseMovementDetails.AsQueryable();
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var storeHouseMovementsDetails = await queryable.Where(x => x.StoreHouseMovement.StoreHouseIdDestination == id)
        //         .Paginar(paginacionDTO)
        //         .ToListAsync();
        //    return mapper.Map<List<StoreHouseMovementDetailDTO>>(storeHouseMovementsDetails);

        //    //var movimientosAgrupados = await queryable.GroupBy(p => new
        //    //{
        //    //    StoreHouseIdDestination = (Guid)p.StoreHouseMovement.StoreHouseIdDestination,
        //    //    ProductId = (Guid)p.ProductId,

        //    //})
        //    //    .Select(x => new
        //    //    {
        //    //        StoreHouseIdDestination = x.Key,
        //    //        ProducId = x.Key,
        //    //        Quantity = x.Sum(p => p.Quantity)
        //    //    })
        //    //    .ToListAsync();
        //    //return Ok(movimientosAgrupados);
        //}

    }
}
