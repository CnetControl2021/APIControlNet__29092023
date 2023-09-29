using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class SaleOrderTranspController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public string RPT { get; private set; }

        public SaleOrderTranspController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<SaleOrderDTO>> Get([FromQuery] Guid storeId, string nombre, DateTime dateIni, DateTime dateFin)
        {
            var queryable = context.SaleOrders.Where(x => x.Active == true && x.Date >= (dateIni) && x.Date <= dateFin).AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var data = await queryable.OrderByDescending(x => x.SaleOrderIdx)
                .Include(x => x.SaleSuborders)
                .AsNoTracking()
                .ToListAsync();
            return Ok(data);
        }



        [HttpPost("{storeId}")]
        [AllowAnonymous]
        public async Task<int> Save([FromBody] SoSsubDTO soSsub, Guid storeId)
        {
            int rpta = 0;
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    SaleOrder so = new();
                    
                    so.SaleOrderId = Guid.NewGuid();
                    so.StoreId = storeId;
                    so.Date = soSsub.saleOrderDTO.Date;
                    so.SaleOrderNumber = soSsub.saleOrderDTO.SaleOrderNumber;
                    so.Name = so.SaleOrderNumber.ToString();

                    context.SaleOrders.Add(so);             
                    context.SaveChanges();
                    //Guid isSaorderNumber = so.SaleOrderId;

                    SaleSuborder sso = new();
                    sso.SaleOrderId = so.SaleOrderId;
                    sso.Name = so.SaleOrderNumber.ToString(); 
                    sso.ProductId = soSsub.saleSuborderDTO.ProductId;
                    sso.Date = soSsub.saleOrderDTO.Date;
                    sso.Quantity = soSsub.saleSuborderDTO.Quantity;  //volumen entregado
                    sso.Price = soSsub.saleSuborderDTO.Price;   //Precio
                    sso.TotalQuantity = soSsub.saleSuborderDTO.Amount; //importe
                    sso.Temperature = soSsub?.saleSuborderDTO?.Temperature;
                    sso.AbsolutePressure = soSsub?.saleSuborderDTO?.AbsolutePressure;  //presion atmosferica
                    sso.CalorificPower = soSsub?.saleSuborderDTO?.CalorificPower;

                    context.SaleSuborders.Add(sso);
                    context.SaveChanges();

                    await transaccion.CommitAsync();
                    rpta = 1;                  //Si respuesta 1 esta ok
                }
            }
            catch (Exception)
            {
                rpta = 0;   // algo esta mal
            }
            return rpta;
        }

    }
}
