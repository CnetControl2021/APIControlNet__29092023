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
using System;

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


        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IEnumerable<SaleOrderDTO>> Get([FromQuery] Guid storeId, DateTime dateIni, DateTime dateFin)
        //{
        //    var queryable = context.SaleOrders.Where(x => x.StoreId.ToString().Contains(storeId.ToString())
        //    && x.Active == true).AsQueryable();
        //    var data = await queryable
        //        .Include(x => x.SaleSuborders)
        //        .AsNoTracking()
        //        .ToListAsync();
        //    return mapper.Map<List<SaleOrderDTO>>(data);

        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ActionResult> Get([FromQuery] Guid storeId, DateTime dateIni, DateTime dateFin)
        //{
        //    var so = await context.SaleOrders.Where(c => c.StoreId == storeId && c.Date >= (dateIni) && c.Date <= dateFin).ToListAsync();

        //    var ssub = context.SaleSuborders.ToList().Where(x => so.Any
        //            (p => p.SaleOrderId.ToString().Contains(x.SaleOrderId.ToString()))).ToList();

        //    var claseEmpaquetada = new
        //    {
        //        SaleOrderDTO = so,
        //        SaleSuborderDTO = ssub,
        //    };
        //    return Ok(claseEmpaquetada);
        //}


        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<ActionResult<SaleOrderDTO>> Get2([FromQuery] Guid storeId, DateTime dateIni, DateTime dateFin)
        {
            List<SaleOrderDTO> listSO = new();

            listSO = await (from so in context.SaleOrders
                            where storeId == so.StoreId
                            join ssub in context.SaleSuborders on so.SaleOrderId equals ssub.SaleOrderId
                            //join pd in context.Products on iI.ProductId equals pd.ProductId
                            //join tk in context.Tanks on pd.ProductId equals tk.ProductId
                            //join iIdoc in context.InventoryInDocuments on iI.InventoryInId equals iIdoc.InventoryInId
                            where so.Date >= (dateIni) && so.Date <= dateFin
                            //&& iI.StoreId == storeId
                            //&& tk.StoreId == storeId
                            orderby so.SaleOrderIdx descending

                            select new SaleOrderDTO
                            {
                                SaleOrderId = so.SaleOrderId,
                                SaleOrderNumber = so.SaleOrderNumber,
                                Date = so.Date,
                                Quantity =ssub.Quantity,
                                Price = ssub.Price,
                                TotalAmount = ssub.TotalAmount

                            }).AsNoTracking().ToListAsync();

            return Ok(listSO);
        }

        [HttpGet("wrapper")]
        [AllowAnonymous]
        public async Task<ActionResult<SoSsubDTO>> Get(Guid idGuid)
        {
            var so = await context.SaleOrders.FirstOrDefaultAsync(c => c.SaleOrderId == idGuid);
            var ssub = await context.SaleSuborders.FirstOrDefaultAsync(c => c.SaleOrderId == idGuid);

            var wrapeerClass = new SoSsubDTO
            {
                saleOrderDTO = new SaleOrderDTO
                {
                    SaleOrderId = so.SaleOrderId,
                    SaleOrderNumber= so.SaleOrderNumber,
                    StartDate = so.StartDate,
                    Date = so.Date,
                },
                saleSuborderDTO = new SaleSuborderDTO
                { 
                    ProductId = ssub.ProductId,
                    StartQuantity = ssub.StartQuantity,
                    Quantity=ssub.Quantity,
                    EndQuantity = ssub.EndQuantity,
                    Price = ssub.Price,
                    TotalAmount = ssub.TotalAmount,
                    Temperature = ssub.Temperature,
                    AbsolutePressure = ssub.AbsolutePressure,
                    CalorificPower = ssub.CalorificPower            
                }
            };
            return wrapeerClass;
        }


        [HttpPost("new/{storeId}")]
        [AllowAnonymous]
        public async Task<int> Save([FromBody] SoSsubDTO soSsub, Guid storeId, Guid inventoryinid)
        {
            //encontrar compra  // mandar idInvIn desde frontend ventana de venta dropdown
            var dataDB = await context.InventoryIns.FirstOrDefaultAsync(x => x.InventoryInId == inventoryinid);
            var starVolDB = dataDB.Volume;

            int rpta = 0;
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    SaleOrder so = new();

                    so.SaleOrderId = Guid.NewGuid();
                    so.StoreId = storeId;
                    so.StartDate = soSsub.saleOrderDTO.StartDate;
                    so.Date = soSsub.saleOrderDTO.Date;
                    so.Updated = so.Date;
                    so.SaleOrderNumber = soSsub.saleOrderDTO.SaleOrderNumber;
                    so.Name = so.SaleOrderNumber.ToString();
                    so.TankIdi = soSsub.saleOrderDTO.TankIdi; //mandar tanlidi
                    so.Active = true;
                    so.Locked = false;
                    so.Deleted = false;

                    context.SaleOrders.Add(so);
                    context.SaveChanges();
                    //Guid isSaorderNumber = so.SaleOrderId;

                    SaleSuborder sso = new();
                    sso.SaleOrderId = so.SaleOrderId;
                    sso.Name = so.SaleOrderNumber.ToString();
                    sso.ProductId = soSsub.saleSuborderDTO.ProductId;
                    sso.StartQuantity = soSsub.saleSuborderDTO.StartQuantity; //vol ini
                    sso.Quantity = soSsub.saleSuborderDTO.Quantity;  //volumen entregado
                    sso.EndQuantity = soSsub.saleSuborderDTO.EndQuantity; //vol fin 
                    sso.Price = soSsub.saleSuborderDTO.Price;   //Precio
                    sso.TotalAmount = soSsub.saleSuborderDTO.TotalAmount; //importe
                    sso.Temperature = soSsub?.saleSuborderDTO?.Temperature;
                    sso.AbsolutePressure = soSsub?.saleSuborderDTO?.AbsolutePressure;  //presion atmosferica
                    sso.CalorificPower = soSsub?.saleSuborderDTO?.CalorificPower;
                    sso.Date = so.Date;

                    context.SaleSuborders.Add(sso);
                    context.SaveChanges();

                    InventoryInSaleOrder invInSaOrder = new ();
                    invInSaOrder.InventoryInSaleOrderId = Guid.NewGuid();
                    invInSaOrder.StoreId = storeId;
                    invInSaOrder.Date = DateTime.Now;
                    invInSaOrder.TankIdi = so.TankIdi;  //desde clase1
                    invInSaOrder.ProductId = sso.ProductId;
                    invInSaOrder.StartVolume = starVolDB; // desde compra
                    invInSaOrder.StartDate = DateTime.Now;
                    invInSaOrder.Volume = sso.Quantity; //desde clase 1
                    invInSaOrder.EndVolume = starVolDB-sso.Quantity; // volumen desde compra menos volumen vemndido
                    invInSaOrder.Updated = DateTime.Now;
                    invInSaOrder.Active = true;
                    invInSaOrder.Deleted = false;
                    invInSaOrder.Locked = false;

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
                    so.StartDate = soSsub.saleOrderDTO.StartDate;
                    so.Date = soSsub.saleOrderDTO.Date;
                    so.Updated = so.Date;
                    so.SaleOrderNumber = soSsub.saleOrderDTO.SaleOrderNumber;
                    so.Name = so.SaleOrderNumber.ToString();
                    so.Active = true;
                    so.Locked = false;
                    so.Deleted = false;

                    context.SaleOrders.Add(so);             
                    context.SaveChanges();
                    //Guid isSaorderNumber = so.SaleOrderId;

                    SaleSuborder sso = new();
                    sso.SaleOrderId = so.SaleOrderId;
                    sso.Name = so.SaleOrderNumber.ToString(); 
                    sso.ProductId = soSsub.saleSuborderDTO.ProductId;
                    sso.StartQuantity = soSsub.saleSuborderDTO.StartQuantity; //vol ini
                    sso.Quantity = soSsub.saleSuborderDTO.Quantity;  //volumen entregado
                    sso.EndQuantity = soSsub.saleSuborderDTO.EndQuantity; //vol fin 
                    sso.Price = soSsub.saleSuborderDTO.Price;   //Precio
                    sso.TotalAmount = soSsub.saleSuborderDTO.TotalAmount; //importe
                    sso.Temperature = soSsub?.saleSuborderDTO?.Temperature;
                    sso.AbsolutePressure = soSsub?.saleSuborderDTO?.AbsolutePressure;  //presion atmosferica
                    sso.CalorificPower = soSsub?.saleSuborderDTO?.CalorificPower;
                    sso.Date = so.Date;

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


        [HttpPut("update")]
        public async Task<IActionResult> Put(Guid idGuid, SoSsubDTO so_SsubDTO)
        {
            var so = await context.SaleOrders.FirstOrDefaultAsync(x => x.SaleOrderId == idGuid);
            if (so == null)
            {
                return NotFound(" no encontrado");
            }

            var ssub = await context.SaleSuborders.FirstOrDefaultAsync(x => x.SaleOrderId == idGuid);
            if (ssub == null)
            {
                return NotFound(" no encontrado");
            }

            so.SaleOrderId = idGuid;
            so.SaleOrderNumber = so_SsubDTO.saleOrderDTO.SaleOrderNumber;
            so.StartDate = so_SsubDTO.saleOrderDTO.StartDate;
            so.Date = so.Date;
            so.Updated = DateTime.Now;

            ////
            ssub.SaleOrderId = idGuid;
            ssub.ProductId = so_SsubDTO.saleSuborderDTO.ProductId;
            ssub.StartQuantity = so_SsubDTO.saleSuborderDTO.StartQuantity;
            ssub.Quantity = so_SsubDTO.saleSuborderDTO.Quantity;
            ssub.EndQuantity = so_SsubDTO.saleSuborderDTO.EndQuantity;
            ssub.Price = so_SsubDTO.saleSuborderDTO.Price;
            ssub.TotalAmount = so_SsubDTO.saleSuborderDTO.TotalAmount;
            ssub.Date = so_SsubDTO.saleSuborderDTO.Date;
            ssub.Temperature = so_SsubDTO.saleSuborderDTO.Temperature;
            ssub.AbsolutePressure = so_SsubDTO.saleSuborderDTO.AbsolutePressure;
            ssub.CalorificPower = so_SsubDTO.saleSuborderDTO.CalorificPower;
            ssub.Updated = DateTime.Now;

            await context.SaveChangesAsync();

            //return Ok(new { InventoryInDTO = upd1, InventoryInDocumentDTO = upd2} );
            return Ok();
        }

    }
}
