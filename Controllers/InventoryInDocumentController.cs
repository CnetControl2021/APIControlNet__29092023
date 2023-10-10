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


        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<InventoryInDocumentDTO>> Get(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.InventoryInDocuments.AsQueryable();
            //if (!string.IsNullOrEmpty(nombre))
            //{
            //    queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            //}
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var invindocument = await queryable.OrderByDescending(x => x.InventoryInDocumentIdx).Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<InventoryInDocumentDTO>>(invindocument);
        }


        [HttpGet("Active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<InventoryInDocumentDTO>> Get2(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, Guid inventoryInId)
        {
            var queryable = context.InventoryInDocuments.Where(x => x.Active == true).AsQueryable();
            //if (!string.IsNullOrEmpty(nombre))
            //{
            //    queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
            //}
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            if (inventoryInId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.InventoryInId == inventoryInId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var invindocument = await queryable.OrderByDescending(x => x.InventoryInDocumentIdx).Paginar(paginacionDTO)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<InventoryInDocumentDTO>>(invindocument);
        }


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

        [HttpGet("empaquetada/{id2}")]
        //[AllowAnonymous]
        public async Task<ActionResult<InvInDoc_InvoiceDTO>> GetId(int id2)
        {
            var iIDoc = await context.InventoryInDocuments.FirstOrDefaultAsync(c => c.InventoryInDocumentIdx == id2);
            var invoice2 = await context.Invoices.FirstOrDefaultAsync(x => x.InvoiceId == iIDoc.InvoiceId);
            //chema
            var claseEmpaquetada = new InvInDoc_InvoiceDTO
            {
                inventoryInDocumentDTO = mapper.Map<InventoryInDocumentDTO>(iIDoc),
                invoiceDTO = mapper.Map<InvoiceDTO>(invoice2)
            };
            return claseEmpaquetada;
        }
        

        [HttpGet("{InventoryInId}/{storeId}")]
        //[AllowAnonymous]
        public async Task<ActionResult<InventoryInDocumentDTO>> Get(Guid InventoryInId, Guid storeId)
        {
            var list = await context.InventoryInDocuments.Where(x => x.InventoryInId == InventoryInId
            && x.StoreId == storeId ).ToListAsync();

            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }


        [HttpPost("{storeId}/{idGuid}")]
        //[AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] InvInDoc_InvoiceDTO invInDoc_Invoice, Guid storeId, Guid idGuid)
        {
            if (idGuid == Guid.Empty)
            {
                return BadRequest("Id de compra no valida");
            }
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    InventoryInDocument oInvInDoc = new();
                    oInvInDoc.InventoryInId = idGuid;
                    oInvInDoc.StoreId = storeId;
                    oInvInDoc.InventoryInIdi = invInDoc_Invoice.inventoryInDocumentDTO.InventoryInIdi;
                    oInvInDoc.Date = invInDoc_Invoice.inventoryInDocumentDTO.Date;
                    oInvInDoc.Type = "RPT";
                    oInvInDoc.Folio = invInDoc_Invoice.inventoryInDocumentDTO.Folio;
                    oInvInDoc.Vehicle = invInDoc_Invoice.inventoryInDocumentDTO.Vehicle;
                    oInvInDoc.Volume = invInDoc_Invoice.inventoryInDocumentDTO.Volume;  //litros
                    oInvInDoc.Amount = invInDoc_Invoice.inventoryInDocumentDTO.Amount;   // Total
                    oInvInDoc.Price = invInDoc_Invoice.inventoryInDocumentDTO.Price;
                    oInvInDoc.SatTipoComprobanteId = invInDoc_Invoice.inventoryInDocumentDTO.SatTipoComprobanteId;
                    oInvInDoc.InvoiceId = Guid.NewGuid();   //IdGuid de Invoice
                    oInvInDoc.Uuid = invInDoc_Invoice.inventoryInDocumentDTO.Uuid;       /// uuid de factura
                    oInvInDoc.Updated = DateTime.Now;
                    oInvInDoc.Active = true;
                    oInvInDoc.Locked = false;
                    oInvInDoc.Deleted = false;

                    context.InventoryInDocuments.Add(oInvInDoc);

                    Invoice oIvoice = new();
                    oIvoice.InvoiceId = (Guid)oInvInDoc.InvoiceId;
                    oIvoice.StoreId = storeId;
                    oIvoice.InvoiceSerieId = invInDoc_Invoice.invoiceDTO.InvoiceSerieId;

                    string folStr = oInvInDoc.Folio.ToString();
                    oIvoice.Folio = folStr;  //de clase 1

                    oIvoice.Date = oInvInDoc.Date; //de clase1
                    oIvoice.SupplierId = invInDoc_Invoice.invoiceDTO.SupplierId;

                    oIvoice.Amount = oInvInDoc.Amount;   //de clase1
                    oIvoice.Subtotal = ((oIvoice.Amount) / ((decimal)1.16));
                    oIvoice.AmountTax = ((oIvoice.Subtotal) * ((decimal)0.16));

                    oIvoice.Uuid = oInvInDoc.Uuid.ToString(); // de clase1
                    oIvoice.SatTipoComprobanteId = oInvInDoc.SatTipoComprobanteId;  // de clase 1

                    oIvoice.Updated = DateTime.Now;
                    oIvoice.Active = true;
                    oIvoice.Locked = false;
                    oIvoice.Deleted = false;

                    context.Invoices.Add(oIvoice);
                    context.SaveChanges();

                    await transaccion.CommitAsync();
                }
            }
            catch (Exception)
            {
                return BadRequest("Revisar datos");
            }
            return Ok();
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(InventoryInDocumentDTO InventoryInDocumentDTO, Guid storeId)
        {
            var invindocumentDB = await context.InventoryInDocuments.FirstOrDefaultAsync(c => c.InventoryInDocumentIdx == InventoryInDocumentDTO.InventoryInDocumentIdx);

            if (invindocumentDB is null)
            {
                return NotFound();
            }
            try
            {
                invindocumentDB = mapper.Map(InventoryInDocumentDTO, invindocumentDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = invindocumentDB.Folio.ToString();
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest($"Ya existe {InventoryInDocumentDTO.Folio} ");
            }
            return NoContent();
        }


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
