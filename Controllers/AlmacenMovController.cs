//using APIControlNet.DTOs;
//using APIControlNet.Models;
//using APIControlNet.Utilidades;
//using AutoMapper;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;

//namespace APIControlNet.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//    public class AlmacenMovController : ControllerBase
//    {
//        private readonly CnetCoreContext context;
//        private readonly IMapper mapper;

//        public AlmacenMovController(CnetCoreContext context, IMapper mapper)
//        {
//            this.context = context;
//            this.mapper = mapper;
//        }


//        [HttpGet]
//        //[AllowAnonymous]
//        public async Task<IEnumerable<AlmMovCrearDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
//        {
//            var queryable = context.StoreHouseMovements.AsQueryable();
//            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
//            //var storeHouseMovements = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
//            var storeHouseMovements = await queryable.Paginar(paginacionDTO)
//                //.Include(ald => ald.StoreHouseMovementDetails.OrderByDescending(q => q.Quantity))
//                .Include(tm => tm.TypeMovementIdxNavigation.N)
//                .ToListAsync();
//            return mapper.Map<List<AlmMovCrearDTO>>(storeHouseMovements);
//        }


//        [HttpPost]
//        public async Task<ActionResult> Post(AlmMovCrearDTO almMovCrearDTO)
//        {
//            var almMovCrear = mapper.Map<StoreHouseMovement>(almMovCrearDTO);

//            context.Add(almMovCrear);

//            foreach (var item in almMovCrear.StoreHouseMovementDetails)
//            {

//                await context.AddRangeAsync(new StoreHouseDetail()
//                {
//                    StoreHouseId = (Guid)almMovCrear.StoreHouseIdDestination,
//                    ProductId = item.ProductId,
//                    Quantity = item.Quantity
//                });
//            }



//            //var storeHouseMovDetailDb = context.StoreHouseMovementDetails.First();

//            //var listado = await context.StoreHouseDetails.AsQueryable(x => AlmMovCrearDTO { StoreHouseIdDestination = x.Store});
//            var almacendetalle = context.StoreHouseDetails.Select(p => new ComparaDTO { AlmacenDestino = p.StoreHouseId, Productoid = p.ProductId }).ToList();

//            //var listaenStock = almcendetalle.Select(p => new AlmacenDetalleDTO { StoreHouseId = p.StoreHouseId, ProductId = p.ProductId }).ToList();
//            //var listaenStock2 = almcendetalle.List;
//            //var listado2 = context.StoreHouseMovements.AsQueryable();

//            //var storeHouseMovement = almMovCrear.ToList();  // necesito propiedad de navegacion
//            //var storeHouseMovementDetails = almMovCrear.StoreHouseMovementDetails.ToList();
//            var listaInsertando = almMovCrear.StoreHouseMovementDetails.AsQueryable(); //    (p => new AlmMovCrearDTO{ StoreHouseIdDestination = p.ProductId  });
//            var listaInsertando2 = listaInsertando.Select(p => new ComparaDTO { AlmacenDestino = (Guid)p.StoreHouseMovement.StoreHouseIdDestination, Productoid = p.ProductId }).ToList();
//            var listaInsertando3 = listaInsertando.Select(p => new { StoreHouseIdDestination = (Guid)p.StoreHouseMovement.StoreHouseIdDestination, ProductId = p.ProductId, Quantity = p.Quantity }).ToList();
//            //var listaInsertando3 = listaInsertando.Select(p => new { Quantity = p.Quantity }).ToList();

//            IEnumerable<ComparaDTO> except = (IEnumerable<ComparaDTO>)listaInsertando2.Except(almacendetalle); //todos los nuevos menos los que estan en la bd
//                                                                                                               //IEnumerable<ComparaDTO> except2 = (IEnumerable<ComparaDTO>)listaInsertando2.Except(almacendetalle);  //todos la bd menos los que se van a insertar

//            //if ((listaInsertando2 is not null) && (except.Any()))
//            //{
//            //    foreach (var item in almMovCrear.StoreHouseMovementDetails)
//            //    {
//            //        await context.AddRangeAsync(new StoreHouseDetail()
//            //        {
//            //            StoreHouseId = (Guid)almMovCrear.StoreHouseIdDestination,
//            //            ProductId = item.ProductId,
//            //            Quantity = item.Quantity
//            //        });
//            //    }

//            //}
//            //else
//            //{
//            //    var listaRemplazar = listaInsertando3;

//            //    context.RemoveRange(listaRemplazar);
//            //    context.AddRange(listaRemplazar);

//            //}


//            //foreach (var product in except)
//            //    Console.WriteLine(product.AlmacenDestino + " " + product.Productoid);
//            //if (except.Any())
//            //{
//            //    foreach (var item in except) //insertando los nuevos com quantity
//            //    {
//            //        await context.AddRangeAsync(new StoreHouseDetail()
//            //        {
//            //            StoreHouseId = item.AlmacenDestino,
//            //            ProductId = item.Productoid,
//            //            Quantity = item.Quantity
//            //        }); ;
//            //    }
//            //}
//            //else
//            //{
//            //    context.UpdateRange(listaInsertando2);
//            //}


//            //foreach (var item in except2) //insertando solo quantity
//            //{
//            //    await context.AddRangeAsync(new StoreHouseDetail()
//            //    {
//            //        Quantity = item.Quantity
//            //    }); ;
//            //}


//            //Enumerable.SequenceEqual(almacendetalle.OrderBy(t => t), listaInsertando2.OrderBy(t => t));


//            //var listIntersect = listaInsertando2.Intersect(almacendetalle).ToList();

//            //var soloEnPrimera = listaInsertando2.Except(almacendetalle).ToList(); 

//            //if (listaInsertando2 is ReferenceEquals almacendetalle)
//            //{
//            //    foreach (var item in almMovCrear.StoreHouseMovementDetails)
//            //    {

//            //        await context.AddRangeAsync(new StoreHouseDetail()
//            //        {
//            //            StoreHouseId = (Guid)almMovCrear.StoreHouseIdDestination,
//            //            ProductId = item.ProductId,
//            //            Quantity = item.Quantity
//            //        });
//            //    }
//            //}

//            //List<Object> firstNotSecond = almacendetalle.Except(listaInsertando2).ToList();

//            //var firstNotSecond = almacendetalle.Where(i => !listaInsertando2.Contains(i)).ToList();


//            //IEnumerable<Object> differenceQuery = listaInsertando2.Except(almacendetalle);



//            //string[] listaGuids = context.StoreHouseDetails.AsQueryable();
//            //var AlmYproductoNew = listado2.Select(p => new {
//            //    StoreHouseIdDestination = p.StoreHouseIdDestination,
//            //    product = item.ProductId
//            //}).ToList();

//            //var AlmYproducto = listado.Select(p => new { StoreHouseId = p.StoreHouseId, ProductId = p.ProductId}).ToList();
//            //var AlmYproductoCDTO = listado.Select(p => new AlmacenDetalleDTO { StoreHouseId = p.StoreHouseId, ProductId = p.ProductId }).ToList();

//            //Console.WriteLine(AlmYproducto.Count);
//            //Console.WriteLine(AlmYproductoCDTO.Count);
//            //var AlmYproductoCDTONew = listado2.Select(p => new { 
//            //                        StoreHouseIdDestination = p.StoreHouseIdDestination,}).ToList();



//            //foreach (var item in almMovCrear.StoreHouseMovementDetails)
//            //{

//            //    await context.AddRangeAsync(new StoreHouseDetail()
//            //    {
//            //        StoreHouseId = (Guid)almMovCrear.StoreHouseIdDestination,
//            //        ProductId = item.ProductId,
//            //        Quantity = item.Quantity
//            //    });
//            //}


//            await context.SaveChangesAsync();
//            return Ok();
//        }



//    }
//}

