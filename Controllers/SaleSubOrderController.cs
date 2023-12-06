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
                          join so in context.SaleOrders on ss.SaleOrderId equals so.SaleOrderId

                          select new SaleSuborderDTO
                          {
                              SaleSuborderIdx = ss.SaleSuborderIdx,
                              SaleOrderId = ss.SaleOrderId,
                              Name = so.SaleOrderNumber.ToString(),
                              ProductId = ss.ProductId,
                              Quantity = ss.Quantity,
                              Price = ss.Price,
                              TotalAmount = ss.TotalAmount,
                              Date = so.Date,
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
                    var saleSubDB = await context.SaleSuborders.FirstOrDefaultAsync(x => x.SaleOrderId == saleSubOrder_Invoice.NSaleSuborderDTO.SaleOrderId);
                    var dataDB = await context.Invoices.FirstOrDefaultAsync(x => x.Uuid == saleSubOrder_Invoice.NInvoiceDTO.Uuid);

                    if (saleSubOrder_Invoice.NInvoiceDTO.InvoiceId == Guid.Empty && dataDB == null)
                    {
                        Invoice oInvoice = new();
                        oInvoice.InvoiceId = Guid.NewGuid();
                        oInvoice.StoreId = storeId;
                        oInvoice.InvoiceSerieId = saleSubOrder_Invoice.NInvoiceDTO.InvoiceSerieId;
                        oInvoice.Folio = saleSubOrder_Invoice.NInvoiceDTO.Folio;
                        oInvoice.Date = saleSubOrder_Invoice.NInvoiceDTO.Date;
                        oInvoice.CustomerId = saleSubOrder_Invoice.NInvoiceDTO.CustomerId;
                        oInvoice.Amount = saleSubOrder_Invoice.NInvoiceDTO.Amount;
                        oInvoice.Subtotal = ((oInvoice.Amount) / ((decimal)1.16));
                        oInvoice.AmountTax = ((oInvoice.Subtotal) * ((decimal)0.16));
                        oInvoice.Uuid = saleSubOrder_Invoice.NInvoiceDTO.Uuid;
                        oInvoice.SatTipoComprobanteId = saleSubOrder_Invoice.NInvoiceDTO.SatTipoComprobanteId;
                        oInvoice.Updated = DateTime.Now;
                        oInvoice.Active = true;
                        oInvoice.Locked = false;
                        oInvoice.Deleted = false;

                        context.Invoices.Add(oInvoice);

                        InvoiceDetail invoiceDetail = new();
                        invoiceDetail.InvoiceId = oInvoice.InvoiceId;
                        invoiceDetail.InvoiceDetailIdi = 1;
                        invoiceDetail.ProductId = saleSubDB.ProductId;
                        invoiceDetail.Quantity = saleSubDB.Quantity;
                        invoiceDetail.Price = saleSubDB?.Price;
                        invoiceDetail.Amount = saleSubOrder_Invoice.NInvoiceDTO.Amount;
                        invoiceDetail.Subtotal = ((invoiceDetail.Amount) / ((decimal)1.16));
                        invoiceDetail.AmountTax = ((invoiceDetail.Amount) * ((decimal)0.16));
                        invoiceDetail.Date = DateTime.Now;
                        invoiceDetail.Updated = DateTime.Now;
                        invoiceDetail.Active = 1;
                        invoiceDetail.Locked = 0;
                        invoiceDetail.Deleted = 0;
                        context.InvoiceDetails.Add(invoiceDetail);

                        var saSub = await context.SaleSuborders.FirstOrDefaultAsync
                            (x => x.SaleOrderId == saleSubOrder_Invoice.NSaleSuborderDTO.SaleOrderId);

                        saSub.InvoiceId = oInvoice.InvoiceId; 
                        saSub.Updated = DateTime.Now;
                        saSub.Active = true;
                        saSub.Deleted = false;
                        saSub.Locked = false;

                        context.SaleSuborders.Update(saSub);
                        context.SaveChanges();
                    }
                    else
                    {
                        var oInvoice = await context.Invoices.FirstOrDefaultAsync(x => x.InvoiceId == dataDB.InvoiceId);
                        var oInvDetail = await context.InvoiceDetails.Where(x => x.InvoiceId == dataDB.InvoiceId)
                            .OrderBy(x => x.InvoiceDetailIdi).LastOrDefaultAsync();
                        var idi = oInvDetail.InvoiceDetailIdi++;

                        InvoiceDetail invoiceDetail = new();
                        invoiceDetail.InvoiceId = oInvoice.InvoiceId;
                        invoiceDetail.InvoiceDetailIdi = idi;
                        invoiceDetail.ProductId = saleSubDB.ProductId;
                        invoiceDetail.Quantity = saleSubDB.Quantity;
                        invoiceDetail.Price = saleSubDB?.Price;
                        invoiceDetail.Amount = saleSubOrder_Invoice.NInvoiceDTO.Amount;
                        invoiceDetail.Subtotal = ((invoiceDetail.Amount) / ((decimal)1.16));
                        invoiceDetail.AmountTax = ((invoiceDetail.Amount) * ((decimal)0.16));
                        invoiceDetail.Date = oInvoice.Date;
                        invoiceDetail.Updated = DateTime.Now;
                        invoiceDetail.Active = 1;
                        invoiceDetail.Locked = 0;
                        invoiceDetail.Deleted = 0;
                        context.InvoiceDetails.Add(invoiceDetail);

                        var listInvoiceDet = await context.InvoiceDetails.Where(x => x.InvoiceId == dataDB.InvoiceId).ToListAsync(); //update invoice Amount
                        var sumInvDet = listInvoiceDet.Sum(x => x.Amount.GetValueOrDefault());
                        oInvoice.Amount = sumInvDet + invoiceDetail.Amount;
                        oInvoice.Updated = DateTime.Now;
                        context.Invoices.Update(oInvoice);

                        var saSub = await context.SaleSuborders.FirstOrDefaultAsync
                            (x => x.SaleOrderId == saleSubOrder_Invoice.NSaleSuborderDTO.SaleOrderId);

                        saSub.InvoiceId = oInvoice.InvoiceId;
                        saSub.Updated = DateTime.Now;
                        saSub.Active = true;
                        saSub.Deleted = false;
                        saSub.Locked = false;

                        context.SaleSuborders.Update(saSub);
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
