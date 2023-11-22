using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
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

    public class SaleSubOrderController : ControllerBase
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;
        public SaleSubOrderController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet("byGuid/{idGuid}")]
        //[AllowAnonymous]
        public async Task<ActionResult<SaleSuborderDTO>> Get2(Guid idGuid)
        {
            List<SaleSuborderDTO> list = new();

            //if (storeId == Guid.Empty) { return BadRequest("Sucursal no valida"); }

            list = await (from ss in context.SaleSuborders
                          where ss.SaleOrderId == idGuid

                          select new SaleSuborderDTO
                          {
                              SaleSuborderIdx = ss.SaleSuborderIdx,
                              SaleOrderId = ss.SaleOrderId,
                              Name = ss.Name,
                              ProductId = ss.ProductId,
                              Quantity = ss.Quantity,
                              Price = ss.Price,
                              TotalAmount = ss.TotalAmount,
                              Date = ss.Date,
                              StartQuantity = ss.StartQuantity,
                              EndQuantity = ss.EndQuantity,
                              QuantityTc = ss.QuantityTc

                          }).AsNoTracking().ToListAsync();
            return Ok(list);
        }

        [HttpGet("package/{id2}/{storeId}")]
        [AllowAnonymous]
        public async Task<ActionResult<SaleSubOrder_Invoice>> Get3(int id2, Guid storeId)
        {
            var ssubO = await context.SaleSuborders.FirstOrDefaultAsync(c => c.SaleSuborderIdx == id2);
            var invoice = await context.Invoices.FirstOrDefaultAsync(x => x.InvoiceId == ssubO.InvoiceId && x.StoreId == storeId);
            //chema
            var claseEmpaquetada = new SaleSubOrder_Invoice
            {
                NSaleSuborderDTO = mapper.Map<SaleSuborderDTO>(ssubO),
                NInvoiceDTO = mapper.Map<InvoiceDTO>(invoice)
            };
            return Ok(claseEmpaquetada);
        }


        [HttpPost("{storeId}")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] SaleSubOrder_Invoice saleSubOrder_Invoice, Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
                return BadRequest("Sucursal no valida");
            }
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    if (saleSubOrder_Invoice.NInvoiceDTO.InvoiceId == Guid.Empty)
                    {
                        Invoice oIvoice = new();
                        oIvoice.InvoiceId = Guid.NewGuid();
                        oIvoice.StoreId = storeId;
                        oIvoice.InvoiceSerieId = saleSubOrder_Invoice.NInvoiceDTO.InvoiceSerieId;
                        oIvoice.Folio = saleSubOrder_Invoice.NInvoiceDTO.Folio;
                        oIvoice.Date = saleSubOrder_Invoice.NInvoiceDTO.Date;
                        oIvoice.CustomerId = saleSubOrder_Invoice.NInvoiceDTO.CustomerId;
                        oIvoice.Amount = saleSubOrder_Invoice.NInvoiceDTO.Amount;
                        oIvoice.Subtotal = ((oIvoice.Amount) / ((decimal)1.16));
                        oIvoice.AmountTax = ((oIvoice.Subtotal) * ((decimal)0.16));
                        oIvoice.Uuid = saleSubOrder_Invoice.NInvoiceDTO.Uuid;
                        oIvoice.SatTipoComprobanteId = saleSubOrder_Invoice.NInvoiceDTO.SatTipoComprobanteId;
                        oIvoice.Updated = DateTime.Now;
                        oIvoice.Active = true;
                        oIvoice.Locked = false;
                        oIvoice.Deleted = false;

                        context.Invoices.Add(oIvoice);

                        var saSub = await context.SaleSuborders.FirstOrDefaultAsync
                            (x => x.SaleOrderId == saleSubOrder_Invoice.NSaleSuborderDTO.SaleOrderId);

                        saSub.InvoiceId = oIvoice.InvoiceId; 
                        saSub.Updated = DateTime.Now;
                        saSub.Active = true;
                        saSub.Deleted = false;
                        saSub.Locked = false;

                        context.SaleSuborders.Update(saSub);
                        context.SaveChanges();
                    }
                    else
                    {
                        var oIvoice = await context.Invoices.FirstOrDefaultAsync(x => x.InvoiceId == saleSubOrder_Invoice.NInvoiceDTO.InvoiceId);
                        //oIvoice.InvoiceId = Guid.NewGuid();
                        //oIvoice.StoreId = storeId;
                        oIvoice.InvoiceSerieId = saleSubOrder_Invoice.NInvoiceDTO.InvoiceSerieId;
                        oIvoice.Folio = saleSubOrder_Invoice.NInvoiceDTO.Folio;
                        oIvoice.Date = saleSubOrder_Invoice.NInvoiceDTO.Date;
                        oIvoice.SupplierId = saleSubOrder_Invoice.NInvoiceDTO.SupplierId;
                        oIvoice.Amount = saleSubOrder_Invoice.NInvoiceDTO.Amount;
                        oIvoice.Subtotal = ((oIvoice.Amount) / ((decimal)1.16));
                        oIvoice.AmountTax = ((oIvoice.Subtotal) * ((decimal)0.16));
                        oIvoice.Uuid = saleSubOrder_Invoice.NInvoiceDTO.Uuid;
                        oIvoice.SatTipoComprobanteId = saleSubOrder_Invoice.NInvoiceDTO.SatTipoComprobanteId;
                        oIvoice.Updated = DateTime.Now;
                        oIvoice.Active = true;
                        oIvoice.Locked = false;
                        oIvoice.Deleted = false;

                        context.Invoices.Update(oIvoice);
                        context.SaveChanges();
                    }
                    await transaccion.CommitAsync();
                }
            }
            catch (Exception)
            {
                return BadRequest("Revisar datos");
            }
            return Ok();
        }

    }
}
