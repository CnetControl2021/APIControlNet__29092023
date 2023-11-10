using APIControlNet.DTOs;
using APIControlNet.DTOs.Seeding;
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

    public class InventoryInDocumentController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        

        public InventoryInDocumentController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        //[HttpGet]
        ////[AllowAnonymous]
        //public async Task<IEnumerable<InventoryInDocumentDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        //{
        //    var queryable = context.InventoryInDocuments.AsQueryable();
        //    //if (!string.IsNullOrEmpty(nombre))
        //    //{
        //    //    queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
        //    //}
        //    if (storeId != Guid.Empty)
        //    {
        //        queryable = queryable.Where(x => x.StoreId == storeId);
        //    }
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var invindocument = await queryable.OrderByDescending(x => x.InventoryInDocumentIdx).Paginar(paginacionDTO)
        //        .AsNoTracking()
        //        .ToListAsync();
        //    return mapper.Map<List<InventoryInDocumentDTO>>(invindocument);
        //}


        //[HttpGet("Active")]
        ////[AllowAnonymous]
        //public async Task<IEnumerable<InventoryInDocumentDTO>> Get2(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, Guid inventoryInId)
        //{
        //    var queryable = context.InventoryInDocuments.Where(x => x.Active == true).AsQueryable();
        //    //if (!string.IsNullOrEmpty(nombre))
        //    //{
        //    //    queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
        //    //}
        //    if (storeId != Guid.Empty)
        //    {
        //        queryable = queryable.Where(x => x.StoreId == storeId);
        //    }
        //    if (inventoryInId != Guid.Empty)
        //    {
        //        queryable = queryable.Where(x => x.InventoryInId == inventoryInId);
        //    }
        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var invindocument = await queryable.OrderByDescending(x => x.InventoryInDocumentIdx).Paginar(paginacionDTO)
        //        .AsNoTracking()
        //        .ToListAsync();
        //    return mapper.Map<List<InventoryInDocumentDTO>>(invindocument);
        //}


        //[HttpGet("sinPag/{nombre}")]
        //[AllowAnonymous]
        //public async Task<IEnumerable<InventoryInDocumentDTO>> Get(Guid storeId, Guid idGuid)
        //{
        //    var data = context.InventoryInDocuments.Where(x => x.InventoryInId == idGuid).FirstOrDefaultAsync();
        //    var uuid = data.Result.Uuid.ToString();


        //    return mapper.Map<List<InventoryInDocumentDTO>>(data);
        //}


        //[HttpGet("{id2:int}", Name = "getInventoryInDocument")]
        //[AllowAnonymous]
        //public async Task<ActionResult<InventoryInDocumentDTO>> Get(int id2)
        //{
        //    var invindocument = await context.InventoryInDocuments.FirstOrDefaultAsync(x => x.InventoryInDocumentIdx == id2);

        //    if (invindocument == null)
        //    {
        //        return NotFound();
        //    }

        //    return mapper.Map<InventoryInDocumentDTO>(invindocument);
        //}


        [HttpGet("package/{id2}/{storeId}")]
        [AllowAnonymous]
        public async Task<ActionResult<InvInDoc_Invoice>> Get3(int id2, Guid storeId)
        {
            var iIDoc = await context.InventoryInDocuments.FirstOrDefaultAsync(c => c.InventoryInDocumentIdx == id2 && c.StoreId == storeId);
            //var paramUuid = iIDoc.Uuid.ToString();
            var invoice = await context.Invoices.FirstOrDefaultAsync(x => x.InvoiceId == iIDoc.InvoiceId);
            var invdet = await context.InvoiceDetails.FirstOrDefaultAsync(x => x.InvoiceId == iIDoc.InvoiceId);
            //chema
            var claseEmpaquetada = new InvInDoc_Invoice
            {
                NInventoryInDocumentDTO = mapper.Map<InventoryInDocumentDTO>(iIDoc),
                NInvoiceDTO = mapper.Map<InvoiceDTO>(invoice),
                NInvoiceDetailDTO = mapper.Map<InvoiceDetailDTO>(invdet)  
            };
            return Ok(claseEmpaquetada);


            //if (paramUuid != null)
            //{
            //    var invoice = await context.Invoices.FirstOrDefaultAsync(x => x.Uuid == paramUuid);
            //    var claseEmpaquetada = new InvInDoc_Invoice
            //    {
            //        NInventoryInDocumentDTO = mapper.Map<InventoryInDocumentDTO>(iIDoc),
            //        NInvoiceDTO = mapper.Map<InvoiceDTO>(invoice)
            //    };
            //    return Ok(claseEmpaquetada);
            //}
            //else
            //{
            //    var invoice = await context.Invoices.FirstOrDefaultAsync(x => x.Uuid == string.Empty);
            //    var claseEmpaquetada = new InvInDoc_Invoice
            //    {
            //        NInventoryInDocumentDTO = mapper.Map<InventoryInDocumentDTO>(iIDoc),
            //    };
            //    return Ok(claseEmpaquetada);
            //}

        }

        [HttpGet("active/byGuid/{idGuid}/{storeId}")]
        [AllowAnonymous]
        public async Task<ActionResult<InventoryInDocumentDTO>> Get2(Guid idGuid, Guid storeId)
        {
            List<InventoryInDocumentDTO> list = new List<InventoryInDocumentDTO>();

            if (storeId == Guid.Empty) { return BadRequest("Sucursal no valida"); }

            list = await (from iID in context.InventoryInDocuments 
                          where iID.InventoryInId == idGuid
                          && iID.StoreId == storeId

                            select new InventoryInDocumentDTO
                            {
                                InventoryInDocumentIdx = iID.InventoryInDocumentIdx,
                                InventoryInId = iID.InventoryInId,
                                Date = iID.Date,
                                Folio = iID.Folio,
                                Volume = iID.Volume,
                                Price = iID.Price,
                                Amount = iID.Amount

                            }).AsNoTracking().ToListAsync();
            return Ok(list);
        }



        //[HttpGet("{InventoryInId}/{storeId}")]
        ////[AllowAnonymous]
        //public async Task<ActionResult<InventoryInDocumentDTO>> Get(Guid InventoryInId, Guid storeId)
        //{
        //    var list = await context.InventoryInDocuments.Where(x => x.InventoryInId == InventoryInId
        //    && x.StoreId == storeId ).ToListAsync();

        //    if (list == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(list);
        //}


        [HttpPost("{storeId}")]
        //[AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] InvInDoc_Invoice invInDoc_Invoice, Guid storeId)
        {
            if (storeId == Guid.Empty)
            {
                return BadRequest("Sucursal no valida");
            }
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {

                    if (invInDoc_Invoice.NInvoiceDTO.InvoiceId == Guid.Empty)
                    {
                        Invoice oIvoice = new();
                        oIvoice.InvoiceId = Guid.NewGuid();
                        oIvoice.StoreId = storeId;
                        oIvoice.InvoiceSerieId = invInDoc_Invoice.NInvoiceDTO.InvoiceSerieId;
                        oIvoice.Folio = invInDoc_Invoice.NInvoiceDTO.Folio;
                        oIvoice.Date = invInDoc_Invoice.NInvoiceDTO.Date;
                        oIvoice.SupplierId = invInDoc_Invoice.NInvoiceDTO.SupplierId;
                        oIvoice.Amount = invInDoc_Invoice.NInvoiceDTO.Amount;
                        oIvoice.Subtotal = ((oIvoice.Amount) / ((decimal)1.16));
                        oIvoice.AmountTax = ((oIvoice.Subtotal) * ((decimal)0.16));
                        oIvoice.Uuid = invInDoc_Invoice.NInvoiceDTO.Uuid;
                        oIvoice.SatTipoComprobanteId = invInDoc_Invoice.NInvoiceDTO.SatTipoComprobanteId;
                        oIvoice.Updated = DateTime.Now;
                        oIvoice.Active = true;
                        oIvoice.Locked = false;
                        oIvoice.Deleted = false;

                        context.Invoices.Add(oIvoice);
                        var iiDoc = await context.InventoryInDocuments.FirstOrDefaultAsync
                        (x => x.InventoryInId == invInDoc_Invoice.NInventoryInDocumentDTO.InventoryInId);

                        iiDoc.InvoiceId = oIvoice.InvoiceId;

                        Guid strToGuid = new Guid(oIvoice.Uuid);
                        iiDoc.Uuid = strToGuid;

                        iiDoc.Updated = DateTime.Now;
                        iiDoc.Active = true;
                        iiDoc.Deleted = false;
                        iiDoc.Locked = false;

                        context.InventoryInDocuments.Update(iiDoc);
                        context.SaveChanges();
                    }
                    else
                    {
                        var oIvoice = await context.Invoices.FirstOrDefaultAsync (x => x.InvoiceId == invInDoc_Invoice.NInvoiceDTO.InvoiceId);
                        //oIvoice.InvoiceId = Guid.NewGuid();
                        //oIvoice.StoreId = storeId;
                        oIvoice.InvoiceSerieId = invInDoc_Invoice.NInvoiceDTO.InvoiceSerieId;
                        oIvoice.Folio = invInDoc_Invoice.NInvoiceDTO.Folio;
                        oIvoice.Date = invInDoc_Invoice.NInvoiceDTO.Date;
                        oIvoice.SupplierId = invInDoc_Invoice.NInvoiceDTO.SupplierId;
                        oIvoice.Amount = invInDoc_Invoice.NInvoiceDTO.Amount;
                        oIvoice.Subtotal = ((oIvoice.Amount) / ((decimal)1.16));
                        oIvoice.AmountTax = ((oIvoice.Subtotal) * ((decimal)0.16));
                        oIvoice.Uuid = invInDoc_Invoice.NInvoiceDTO.Uuid;
                        oIvoice.SatTipoComprobanteId = invInDoc_Invoice.NInvoiceDTO.SatTipoComprobanteId;
                        oIvoice.Updated = DateTime.Now;
                        oIvoice.Active = true;
                        oIvoice.Locked = false;
                        oIvoice.Deleted = false;

                        context.Invoices.Update(oIvoice);
                        var iiDoc = await context.InventoryInDocuments.FirstOrDefaultAsync
                        (x => x.InventoryInId == invInDoc_Invoice.NInventoryInDocumentDTO.InventoryInId);

                        iiDoc.InvoiceId = oIvoice.InvoiceId;

                        Guid strToGuid = new Guid(oIvoice.Uuid);
                        iiDoc.Uuid = strToGuid;

                        iiDoc.Updated = DateTime.Now;
                        iiDoc.Active = true;
                        iiDoc.Deleted = false;
                        iiDoc.Locked = false;

                        context.InventoryInDocuments.Update(iiDoc);
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


        //[HttpPut("{storeId?}")]
        //public async Task<IActionResult> Put(InvInDoc_Invoice inDoc_Invoice, Guid storeId)
        //{

        //    try
        //    {
        //        var inventoryInDocumentDTO = mapper.Map<InventoryInDocumentDTO>(inDoc_Invoice.NInventoryInDocumentDTO);
        //        var invoiceDTO = mapper.Map<InvoiceDTO>(inDoc_Invoice.NInvoiceDTO);

        //        var invInDocDB = await context.InventoryInDocuments.FirstOrDefaultAsync
        //        (c => c.InventoryInDocumentIdx == inDoc_Invoice.NInventoryInDocumentDTO.InventoryInDocumentIdx);

        //        var invoiceDB = await context.Invoices.FirstOrDefaultAsync
        //        (c => c.InvoiceId == invInDocDB.InvoiceId);

        //        if (invInDocDB == null || invoiceDB == null) { return NotFound(); }

        //        context.Entry(invInDocDB).CurrentValues.SetValues(inventoryInDocumentDTO);
        //        context.Entry(invoiceDB).CurrentValues.SetValues(invoiceDTO);
        //        await context.SaveChangesAsync();
        //        return Ok();

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.InventoryInDocuments.AnyAsync(x => x.InventoryInDocumentIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.InventoryInDocuments.FirstOrDefaultAsync(x => x.InventoryInDocumentIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.Folio.ToString();
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name, storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            try
            {
                var existe = await context.InventoryInDocuments.AnyAsync(x => x.InventoryInDocumentIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.InventoryInDocuments.FirstOrDefaultAsync(x => x.InventoryInDocumentIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.Folio.ToString();
                var storeId2 = storeId;
                await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name, storeId2);

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return BadRequest("ERROR DE DATOS RELACIONADOS");
            }
        }
    }
}
