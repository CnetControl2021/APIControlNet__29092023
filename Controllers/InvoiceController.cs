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

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InvoiceController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public string RPT { get; private set; }

        public InvoiceController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("Active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<InvoiceDTO>> Get2(Guid storeId, Guid idGuid, [FromQuery] string nombre)
        {
            var queryable = context.Invoices.Where(x => x.InvoiceId == idGuid && x.Active == true).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Folio.ToLower().Contains(nombre));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var data = await queryable.OrderByDescending(x => x.InvoiceIdx)
                //.Include(x => x.LoadPosition)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<InvoiceDTO>>(data);
        }

        [HttpPost("{storeId}/{idGuid}")]
        //[AllowAnonymous]
        public async Task<int> Save([FromBody] InvoiceDTO invoiceDTO, Guid storeId, Guid idGuid)
        {
            int rpta = 0;
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    Invoice oInvoiceDTO = new();
                    oInvoiceDTO.InvoiceId = Guid.NewGuid();
                    oInvoiceDTO.StoreId = storeId;
                    oInvoiceDTO.InvoiceSerieId = invoiceDTO.InvoiceSerieId;
                    oInvoiceDTO.Folio = invoiceDTO.Folio;
                    oInvoiceDTO.Date = invoiceDTO.Date;
                    oInvoiceDTO.CustomerId = invoiceDTO.CustomerId;
                    oInvoiceDTO.Amount = invoiceDTO.Amount;
                    oInvoiceDTO.Subtotal = ((oInvoiceDTO.Amount) / ((decimal)1.16));
                    oInvoiceDTO.AmountTax = ((oInvoiceDTO.Subtotal) * ((decimal)0.16));
                    oInvoiceDTO.Uuid = invoiceDTO.Uuid.ToString();
                    oInvoiceDTO.SatTipoComprobanteId = invoiceDTO.SatTipoComprobanteId;
                    oInvoiceDTO.Updated = DateTime.Now;
                    oInvoiceDTO.Active = true;
                    oInvoiceDTO.Locked = false;
                    oInvoiceDTO.Deleted = false;

                    context.Invoices.Add(oInvoiceDTO);

                    InventoryInDocument oInvoiceDTODocument = context.InventoryInDocuments.First(x => x.InventoryInId == idGuid);
                    Guid myGuid = new Guid(oInvoiceDTO.Uuid);
                    oInvoiceDTODocument.Uuid = myGuid;
                    //oInvoiceDTO.Uuid = oInvoiceDTO.Uuid;
                    
                    context.InventoryInDocuments.Update(oInvoiceDTODocument);
                    context.SaveChanges();

                    await transaccion.CommitAsync();
                    rpta = 1;
                }
            }
            catch (Exception)
            {
                rpta = 0;
            }
            return rpta;
        }

    }
}
