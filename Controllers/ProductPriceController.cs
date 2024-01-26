using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductPriceController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public ProductPriceController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid storeId)
        {
            try
            {
                var result = await (from p in context.Products
                                    join pp in context.ProductPrices on p.ProductId equals pp.ProductId
                                    join ps in context.ProductStores on p.ProductId equals ps.ProductId
                                    where p.IsFuel == true
                                          && pp.IsCancelled == false
                                          && pp.IsApplied == false
                                          && pp.StoreId == storeId
                                    orderby pp.ProgrammingDate descending
                                    //orderby ps.ProductUce

                                    select new 
                                    {
                                        pp.ProductPriceIdx,
                                        p.ProductCode,
                                        p.ProductId,
                                        pp.Ieps,
                                        pp.Price,
                                        pp.Date,
                                        pp.ProgrammingDate,
                                        pp.ApplicationDate
                                    }).ToListAsync();

                var conteo = result.Count();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        //[HttpPost("{storeId?}")]
        [HttpPost]
        public async Task<ActionResult> Post(ProductPriceDTO productPriceDTO, Guid storeId)
        {
            var dataDb = await context.ProductPrices.FirstOrDefaultAsync(x => x.StoreId == storeId
            && x.ProductId == productPriceDTO.ProductId && x.IsCancelled == false && x.IsApplied == false);
            if (dataDb != null) { dataDb.IsCancelled = true; context.Update(dataDb); }           
                     
            try 
            {
                var pp = mapper.Map<ProductPrice>(productPriceDTO);
                pp.StoreId = storeId;
                pp.Date = DateTime.Now;      
                pp.ApplicationDate = DateTime.Now;
                pp.IsCancelled = false;
                pp.IsApplied = false;
                pp.Updated = DateTime.Now;
                pp.Active = true;
                pp.Locked = false;
                pp.Deleted = false;

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = pp.Price.ToString();
                var storeId2 = storeId;


                context.Add(pp);
                await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                await context.SaveChangesAsync();
                return Ok();
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/datos/actualizar
        [HttpPost("list")]
        public async Task<IActionResult> ActualizarDatos([FromBody] List<ProductPriceDTO> datos, Guid storeId)
        {
            var queryable = context.ProductPrices.AsQueryable();

            var db = queryable.Where(x => x.StoreId == storeId && x.IsCancelled == false && x.IsApplied == false);

            if (db.Any())
            {

                foreach (var item in db)
                {
                    item.IsCancelled = true; 
                }
                context.SaveChanges();
            }

            try
            {
                if (datos == null || !datos.Any())
                {
                    return BadRequest("No se proporcionaron datos para actualizar.");
                }

                foreach (var item in datos)
                {
                    ProductPrice pp = new();
                    pp.StoreId = storeId;
                    pp.ProductId = item.ProductId;
                    pp.Date = DateTime.Now;
                    pp.ApplicationDate = DateTime.Now;
                    pp.IsCancelled = false;
                    pp.IsApplied = false;
                    pp.Updated = DateTime.Now;
                    pp.Active = true;
                    pp.Locked = false;
                    pp.Deleted = false;
                    pp.Price = item.Price;
                    pp.IsImmediate = item.IsImmediate;
                    pp.Ieps = item.Ieps;
                    pp.ProgrammingDate = item.ProgrammingDate;

                    var usuarioId = obtenerUsuarioId();
                    var ipUser = obtenetIP();
                    var name = pp.Price.ToString();
                    var storeId2 = storeId;

                    context.Add(pp);
                    await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Ok("Datos actualizados con éxito");
        }

    }
}
