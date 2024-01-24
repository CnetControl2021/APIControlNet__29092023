using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //var dataDb = await context.Stores.FirstOrDefaultAsync(x => x.StoreId == storeId);
            //var companyId = dataDb.CompanyId;

            try 
            {
                var pp = mapper.Map<ProductPrice>(productPriceDTO);
                pp.StoreId = storeId;
                pp.Date = DateTime.Now;
                pp.Updated = DateTime.Now;



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
            

            //if (existeIDYNombre)
            //{
            //    return BadRequest($"Ya existe {IslandDTO.IslandIdi} en esa sucursal ");
            //}
            //else
            //{
            //    context.Add(island);
            //    await servicioBinnacle.AddBinnacle(usuarioId, ipUser, name, storeId2);
            //    await context.SaveChangesAsync();
            //}

            //var IslandDTO2 = mapper.Map<IslandDTO>(island);
            //return CreatedAtRoute("obtenerIsla", new { id = island.IslandIdx }, IslandDTO2);
        }

    }
}
