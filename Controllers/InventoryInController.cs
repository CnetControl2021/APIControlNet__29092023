using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InventoryInController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public string RPT { get; private set; }

        public InventoryInController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpPost("{storeId}")]
        //[AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] InventoryIn oInventoryIn, Guid storeId, Guid idGuid)
        {
            if (idGuid != Guid.Empty)
            {
                var existeid = await context.InventoryIns.AnyAsync(x => x.InventoryInId == idGuid);
            }

            try
            {
                InventoryIn newIi = new InventoryIn();
                newIi.InventoryInId = Guid.NewGuid();
                newIi.StoreId = storeId;
                newIi.InventoryInNumber = oInventoryIn.InventoryInNumber; //capturado
                newIi.StartDate = oInventoryIn.StartDate;
                newIi.EndDate = oInventoryIn.EndDate;
                newIi.Date = DateTime.Now;
                newIi.TankIdi = oInventoryIn.TankIdi;
                newIi.ProductId = oInventoryIn.ProductId;
                newIi.StartVolume = oInventoryIn.StartVolume;
                newIi.Volume = oInventoryIn.Volume;
                newIi.EndVolume = oInventoryIn.EndVolume;
                newIi.StartTemperature = oInventoryIn.StartTemperature;
                newIi.EndTemperature = oInventoryIn.StartTemperature;
                newIi.AbsolutePressure = oInventoryIn.AbsolutePressure;
                newIi.CalorificPower = oInventoryIn.CalorificPower;
                newIi.Updated = DateTime.Now;
                newIi.Active = true;
                newIi.Locked = false;
                newIi.Deleted = false;

                context.InventoryIns.Add(newIi);
                context.SaveChanges();

                return Ok();
                // }
            }
            catch (Exception)
            {
                return BadRequest("Revisar captura");
            }
        }

        //[HttpPost("{storeId}")]
        ////[AllowAnonymous]
        //public async Task<int> Save([FromBody] invInDoc_Invoice invInDoc_Invoice, Guid storeId, Guid idGuid)
        //{
        //    if (idGuid != Guid.Empty)
        //    {
        //        var existeid = await context.InventoryIns.AnyAsync(x => x.InventoryInId == idGuid);
        //    }

        //    int rpta = 0;
        //    try
        //    {
        //        //using (var transaccion = new TransactionScope())
        //        using (var transaccion = await context.Database.BeginTransactionAsync())
        //        {
        //            InventoryIn oInventoryIn = new InventoryIn();
        //            oInventoryIn.InventoryInId = Guid.NewGuid();
        //            oInventoryIn.StoreId = storeId;
        //            oInventoryIn.InventoryInNumber = invInDoc_Invoice.inventoryInDTO.InventoryInNumber; //capturado
        //            oInventoryIn.Date = DateTime.Now;
        //            oInventoryIn.TankIdi = invInDoc_Invoice.inventoryInDTO.TankIdi;
        //            oInventoryIn.ProductId = invInDoc_Invoice.inventoryInDTO.ProductId;
        //            oInventoryIn.StartDate = invInDoc_Invoice.inventoryInDTO.StartDate;
        //            oInventoryIn.EndDate = invInDoc_Invoice.inventoryInDTO.EndDate;
        //            oInventoryIn.StartVolume = invInDoc_Invoice.inventoryInDTO.StartVolume;
        //            oInventoryIn.Volume = invInDoc_Invoice.inventoryInDTO.Volume;
        //            oInventoryIn.EndVolume = invInDoc_Invoice.inventoryInDTO.EndVolume;
        //            oInventoryIn.StartTemperature = invInDoc_Invoice.inventoryInDTO.StartTemperature;
        //            oInventoryIn.EndTemperature = oInventoryIn.StartTemperature;
        //            oInventoryIn.AbsolutePressure = invInDoc_Invoice.inventoryInDTO.AbsolutePressure;
        //            oInventoryIn.CalorificPower = invInDoc_Invoice.inventoryInDTO.CalorificPower;
        //            oInventoryIn.Updated = invInDoc_Invoice.inventoryInDTO.Updated;
        //            oInventoryIn.Active = invInDoc_Invoice.inventoryInDTO.Active;
        //            oInventoryIn.Locked = invInDoc_Invoice.inventoryInDTO.Locked;
        //            oInventoryIn.Deleted = invInDoc_Invoice.inventoryInDTO.Deleted;

        //            context.InventoryIns.Add(oInventoryIn);

        //            InventoryInDocument oInventoryInDocument = new InventoryInDocument();

        //            oInventoryInDocument.InventoryInId = oInventoryIn.InventoryInId;
        //            oInventoryInDocument.StoreId = oInventoryIn.StoreId;
        //            oInventoryInDocument.InventoryInIdi = 1;
        //            oInventoryInDocument.Date = DateTime.Now;
        //            oInventoryInDocument.Type = "RPT";
        //            oInventoryInDocument.Volume = oInventoryIn.Volume;
        //            oInventoryInDocument.Price = invInDoc_Invoice.inventoryInDocumentDTO.Price;
        //            oInventoryInDocument.Updated = invInDoc_Invoice.inventoryInDocumentDTO.Updated;
        //            oInventoryInDocument.Active = invInDoc_Invoice.inventoryInDocumentDTO.Active;
        //            oInventoryInDocument.Locked = invInDoc_Invoice.inventoryInDocumentDTO.Locked;
        //            oInventoryInDocument.Deleted = invInDoc_Invoice.inventoryInDocumentDTO.Deleted;

        //            context.InventoryInDocuments.Add(oInventoryInDocument);
        //            context.SaveChanges();

        //            await transaccion.CommitAsync();
        //            rpta = 1;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        rpta = 0;
        //    }
        //    return rpta; 
        //}


        //[HttpPut("update")]
        //public async Task<IActionResult> Put(Guid idGuid, invInDoc_Invoice invInDoc_Invoice)
        //{
        //    var inv = await context.InventoryIns.FirstOrDefaultAsync(x => x.InventoryInId == idGuid);
        //    if (inv == null)
        //    {
        //        return NotFound(" no encontrado");
        //    }

        //    var invDoc = await context.InventoryInDocuments.FirstOrDefaultAsync(x => x.InventoryInId == idGuid);
        //    if (invDoc == null)
        //    {
        //        return NotFound(" no encontrado");
        //    }

        //    inv.InventoryInId = idGuid;
        //    inv.TankIdi = invInDoc_Invoice.inventoryInDTO.TankIdi;
        //    inv.ProductId = invInDoc_Invoice.inventoryInDTO.ProductId;
        //    inv.StartDate = invInDoc_Invoice.inventoryInDTO.StartDate;
        //    inv.EndDate = invInDoc_Invoice.inventoryInDTO.EndDate;
        //    inv.StartVolume = invInDoc_Invoice.inventoryInDTO.StartVolume;
        //    inv.Volume = invInDoc_Invoice.inventoryInDTO?.Volume;
        //    inv.EndVolume = invInDoc_Invoice?.inventoryInDTO?.EndVolume;
        //    inv.StartTemperature = invInDoc_Invoice.inventoryInDTO.StartTemperature;
        //    inv.AbsolutePressure = invInDoc_Invoice.inventoryInDTO.AbsolutePressure;

        //    var upd1 = new InventoryInDTO
        //    {
        //        InventoryInId = inv.InventoryInId,
        //        StartVolume = inv.StartVolume,
        //    };
        //    ////
        //    invDoc.InventoryInId = idGuid;
        //    invDoc.Price = invInDoc_Invoice.inventoryInDocumentDTO.Price;

        //    var upd2 = new InventoryInDocumentDTO
        //    {
        //        InventoryInId = invDoc.InventoryInId,
        //        Price = invDoc.Price,
        //    };

        //    await context.SaveChangesAsync();

        //    //return Ok(new { InventoryInDTO = upd1, InventoryInDocumentDTO = upd2} );
        //    return Ok();
        //}

        [HttpGet("byIdGuid/{idGuid}")] 
        [AllowAnonymous]
        public async Task<ActionResult<InventoryInDTO>> Get([FromRoute] Guid idGuid)
        {
            if (idGuid == Guid.Empty) {  return BadRequest("Id no valido"); }
            var resp = await context.InventoryIns.FirstAsync(e => e.InventoryInId.Equals(idGuid));
            if (resp == null) { return NotFound("No encontrado"); }
            return mapper.Map<InventoryInDTO>(resp);
        }

        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(InventoryInDTO inventoryInDTO, Guid storeId)
        {
            var db = await context.InventoryIns.FirstOrDefaultAsync(c => c.InventoryInId == inventoryInDTO.InventoryInId);

            if (db is null)
            {
                return NotFound();
            }
            try
            {
                db = mapper.Map(inventoryInDTO, db);
                db.Updated = DateTime.Now;

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = db.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
                return Ok(db);

            }
            catch
            {
                return BadRequest($"Revisar datos");
            }

        }


        //[HttpGet("empaquetada")]
        //[AllowAnonymous]
        //public async Task<ActionResult<invInDoc_Invoice>> GetClaseEmpaquetada(Guid idGuid)
        //{
        //    var iI = await context.InventoryIns.FirstOrDefaultAsync(c => c.InventoryInId == idGuid);
        //    var iIDoc = await context.InventoryInDocuments.FirstOrDefaultAsync(c => c.InventoryInId == idGuid);
        //    //chema
        //    var claseEmpaquetada = new invInDoc_Invoice
        //    {
        //        inventoryInDTO = new InventoryInDTO
        //        {
        //            InventoryInId = iI.InventoryInId,
        //            TankIdi = iI.TankIdi,
        //            ProductId = iI.ProductId,
        //            StartDate = iI.StartDate,
        //            EndDate = iI.EndDate,
        //            StartVolume = iI.StartVolume,
        //            Volume = iI.Volume,
        //            EndVolume = iI.EndVolume,
        //            StartTemperature = iI.StartTemperature,
        //            AbsolutePressure = iI.AbsolutePressure
        //        },
        //        inventoryInDocumentDTO = new InventoryInDocumentDTO { Price = iIDoc.Price }
        //    };
        //    return claseEmpaquetada;
        //}


        //[HttpGet("informacion")] ///example chatgpt openia
        //public async Task<ActionResult<IEnumerable<InformacionOrdenDTO>>> GetInformacionOrdenes()
        //{
        //    var informacionOrdenes = await (
        //        from p in _context.Productos
        //        from c in _context.Clientes
        //        select new InformacionOrdenDTO
        //        {
        //            Producto = p,
        //            Cliente = c
        //        }).ToListAsync();

        //    var productos = informacionOrdenes.Select(io => io.Producto).ToList();
        //    var clientes = informacionOrdenes.Select(io => io.Cliente).ToList();

        //    return Ok(new { Productos = productos, Clientes = clientes });
        //}


        //[HttpPut("{storeId}")]
        //[AllowAnonymous]
        //public async Task<ActionResult<InventoryInDTO>> Put(InventoryInDTO inventoryInDTO, Guid storeId)
        //{
        //    //var existeid = await context.InventoryIns.AnyAsync(x => x.InventoryInId == idGuid);

        //    try
        //    {
        //        using (var transaccion = await context.Database.BeginTransactionAsync())
        //        {
        //            InventoryIn oInventoryIn = context.InventoryIns.FirstOrDefault(x => x.InventoryInId == inventoryInDTO.InventoryInId);

        //            oInventoryIn.InventoryInId = inventoryInDTO.InventoryInId;
        //            oInventoryIn.TankIdi = inventoryInDTO.TankIdi;
        //            oInventoryIn.ProductId = inventoryInDTO.ProductId;
        //            oInventoryIn.StartDate = inventoryInDTO.StartDate;
        //            oInventoryIn.EndDate = inventoryInDTO.EndDate;
        //            oInventoryIn.StartVolume = inventoryInDTO.StartVolume;
        //            oInventoryIn.Volume = inventoryInDTO.Volume;
        //            oInventoryIn.EndVolume = inventoryInDTO.EndVolume;
        //            oInventoryIn.StartTemperature = inventoryInDTO.StartTemperature;
        //            oInventoryIn.EndTemperature = inventoryInDTO.EndTemperature;
        //            oInventoryIn.AbsolutePressure = inventoryInDTO.AbsolutePressure;
        //            oInventoryIn.CalorificPower = inventoryInDTO.CalorificPower;
        //            oInventoryIn.Updated = DateTime.Now;
        //            context.Update(oInventoryIn);

        //            InventoryInDocument oInventoryInDocument = context.InventoryInDocuments.FirstOrDefault(x => x.InventoryInId == inventoryInDTO.InventoryInId);

        //            //oInventoryInDocument.InventoryInDocumentIdx = oInventoryInDocument.InventoryInDocumentIdx;
        //            oInventoryInDocument.InventoryInId = oInventoryIn.InventoryInId;
        //            oInventoryInDocument.StoreId = oInventoryInDocument.StoreId;
        //            oInventoryInDocument.SupplierTransportRegisterId = oInventoryInDocument.SupplierTransportRegisterId;
        //            oInventoryInDocument.Uuid = oInventoryInDocument.Uuid;

        //            oInventoryInDocument.Price = inventoryInDTO.Price;
        //            oInventoryInDocument.Updated = DateTime.Now;

        //            context.Update(oInventoryInDocument);
        //            context.SaveChanges();

        //            var supTraRegId = await context.SupplierTransportRegisters.AnyAsync
        //                    (x => x.SupplierTransportRegisterId == oInventoryInDocument.SupplierTransportRegisterId);

        //            if (supTraRegId is false && inventoryInDTO.AmountPerFee != 0 && inventoryInDTO.SupplierId != Guid.Empty)
        //            {
        //                SupplierTransportRegister stg = new();

        //                stg.SupplierTransportRegisterId = Guid.NewGuid();

        //                stg.SupplierId = inventoryInDTO.SupplierId;
        //                stg.AmountPerFee = inventoryInDTO.AmountPerFee;
        //                stg.AmountPerCapacity = inventoryInDTO.AmountPerCapacity;
        //                stg.AmountPerUse = inventoryInDTO.AmountPerUse;
        //                stg.AmountPerVolume = inventoryInDTO.AmountPerVolume;
        //                stg.AmountPerService = inventoryInDTO.AmountPerService;
        //                stg.AmountOfDiscount = inventoryInDTO.AmountOfDiscount;
        //                stg.Date = inventoryInDTO.Date;
        //                stg.Updated = inventoryInDTO.Updated;
        //                stg.Active = 1;
        //                stg.Locked = 0;
        //                stg.Deleted = 0;
        //                oInventoryInDocument.SupplierTransportRegisterId = stg.SupplierTransportRegisterId;

        //                context.SupplierTransportRegisters.Add(stg);
        //                context.SaveChanges();
        //            }
        //            else
        //            {
        //                SupplierTransportRegister stg = context.SupplierTransportRegisters.FirstOrDefault(x => x.SupplierTransportRegisterId == oInventoryInDocument.SupplierTransportRegisterId);

        //                stg.SupplierTransportRegisterId = stg.SupplierTransportRegisterId;

        //                stg.SupplierId = inventoryInDTO.SupplierId;
        //                stg.AmountPerFee = inventoryInDTO.AmountPerFee;
        //                stg.AmountPerCapacity = inventoryInDTO.AmountPerCapacity;
        //                stg.AmountPerUse = inventoryInDTO.AmountPerUse;
        //                stg.AmountPerVolume = inventoryInDTO.AmountPerVolume;
        //                stg.AmountPerService = inventoryInDTO.AmountPerService;
        //                stg.AmountOfDiscount = inventoryInDTO.AmountOfDiscount;
        //                stg.Date = inventoryInDTO.Date;
        //                stg.Updated = inventoryInDTO.Updated;
        //                stg.Active = 1;
        //                stg.Locked = 0;
        //                stg.Deleted = 0;
        //                oInventoryInDocument.SupplierTransportRegisterId = stg.SupplierTransportRegisterId;

        //                context.SupplierTransportRegisters.Update(stg);
        //                context.SaveChanges();

        //            }

        //            var invId = await context.Invoices.AnyAsync
        //                    (x => x.Uuid == inventoryInDTO.Uuid.ToString());

        //            if (invId is false)
        //            {
        //                Invoice inv = new();

        //                inv.InvoiceId = Guid.NewGuid();
        //                inv.StoreId = storeId;
        //                inv.InvoiceSerieId = inventoryInDTO.InvoiceSerieId;
        //                inv.Folio = inventoryInDTO.Folio;
        //                inv.Date = inventoryInDTO.Date;
        //                inv.CustomerId = inventoryInDTO.CustomerId;
        //                inv.Amount = inventoryInDTO.Amount; //precio
        //                inv.Subtotal = ((inv.Amount) / ((decimal)1.16));
        //                inv.AmountTax = ((inv.Subtotal) * ((decimal)0.16));
        //                inv.SatTipoComprobanteId = inventoryInDTO.SatTipoComprobanteId;
        //                inv.Updated = DateTime.Now;
        //                inv.Uuid = inventoryInDTO.Uuid.ToString();
        //                inv.Active = true;
        //                inv.Locked = false;
        //                inv.Deleted = false;
        //                Guid myGuid = new Guid(inv.Uuid);
        //                oInventoryInDocument.Uuid = myGuid;

        //                context.Invoices.Add(inv);
        //                context.SaveChanges();
        //            }
        //            else
        //            {
        //                Invoice inv = context.Invoices.FirstOrDefault(x => x.Uuid == oInventoryInDocument.Uuid.ToString());

        //                //inv.InvoiceId = inv.InvoiceId;

        //                inv.InvoiceSerieId = inventoryInDTO.InvoiceSerieId;
        //                inv.Folio = inventoryInDTO.Folio;
        //                inv.Date = inventoryInDTO.Date;
        //                inv.CustomerId = inventoryInDTO.CustomerId;
        //                inv.Amount = inventoryInDTO.Amount; //precio
        //                inv.Subtotal = ((inv.Amount) / ((decimal)1.16));
        //                inv.AmountTax = ((inv.Subtotal) * ((decimal)0.16));
        //                inv.SatTipoComprobanteId = inventoryInDTO.SatTipoComprobanteId;
        //                inv.Updated = DateTime.Now;
        //                inv.Uuid = inventoryInDTO.Uuid.ToString();
        //                inv.Active = true;
        //                inv.Locked = false;
        //                inv.Deleted = false;
        //                Guid myGuid = new Guid(inv.Uuid);
        //                oInventoryInDocument.Uuid = myGuid;

        //                context.Invoices.Update(inv);
        //                context.SaveChanges();
        //            }


        //            await transaccion.CommitAsync();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return NoContent();
        //    }
        //    return Ok();

        //}


        //[HttpPost("{storeId?}")]
        //[AllowAnonymous]
        //public async Task<int> Save([FromBody] invInDoc_Invoiceum invInDoc_Invoiceum, Guid storeId, Guid idGuid)
        //{
        //    var existeid = await context.InventoryIns.AnyAsync(x => x.InventoryInId == idGuid);

        //    int rpta = 0;
        //    try
        //    {
        //        //using (var transaccion = new TransactionScope())
        //        using (var transaccion = await context.Database.BeginTransactionAsync())
        //        {
        //            if (!existeid)
        //            {
        //                InventoryIn oInventoryIn = new InventoryIn();
        //                oInventoryIn.InventoryInId = invInDoc_Invoiceum.InventoryInDTO.InventoryInId;  //se genera en clase es llave para document
        //                oInventoryIn.StoreId = storeId;
        //                oInventoryIn.InventoryInNumber = invInDoc_Invoiceum.InventoryInDTO.InventoryInNumber; //capturado
        //                oInventoryIn.Date = DateTime.Now;
        //                oInventoryIn.TankIdi = invInDoc_Invoiceum.InventoryInDTO.TankIdi;
        //                oInventoryIn.ProductId = invInDoc_Invoiceum.InventoryInDTO.ProductId;
        //                oInventoryIn.StartDate = invInDoc_Invoiceum.InventoryInDTO.StartDate;
        //                oInventoryIn.EndDate = invInDoc_Invoiceum.InventoryInDTO.EndDate;
        //                oInventoryIn.StartVolume = invInDoc_Invoiceum.InventoryInDTO.StartVolume;
        //                oInventoryIn.Volume = invInDoc_Invoiceum.InventoryInDTO.Volume;
        //                oInventoryIn.EndVolume = invInDoc_Invoiceum.InventoryInDTO.EndVolume;
        //                oInventoryIn.StartTemperature = invInDoc_Invoiceum.InventoryInDTO.StartTemperature;
        //                oInventoryIn.EndTemperature = oInventoryIn.StartTemperature;
        //                oInventoryIn.AbsolutePressure = invInDoc_Invoiceum.InventoryInDTO.AbsolutePressure;
        //                oInventoryIn.CalorificPower = invInDoc_Invoiceum.InventoryInDTO.CalorificPower;
        //                oInventoryIn.Updated = invInDoc_Invoiceum.InventoryInDTO.Updated;
        //                oInventoryIn.Active = invInDoc_Invoiceum.InventoryInDTO?.Active;
        //                oInventoryIn.Locked = invInDoc_Invoiceum.InventoryInDTO?.Locked;
        //                oInventoryIn.Deleted = invInDoc_Invoiceum.InventoryInDTO.Deleted;

        //                context.InventoryIns.Add(oInventoryIn);               //Agrego o marco
        //                context.SaveChanges();                                //Guardo
        //                Guid iInventoryInId = oInventoryIn.InventoryInId;    //obtuve id creado en tabla1

        //                InventoryInDocument oInventoryInDocument = new InventoryInDocument();
        //                oInventoryInDocument.InventoryInDocumentIdx = oInventoryInDocument.InventoryInDocumentIdx;
        //                oInventoryInDocument.InventoryInId = iInventoryInId;
        //                oInventoryInDocument.StoreId = oInventoryIn.StoreId;
        //                oInventoryInDocument.InventoryInIdi = 1;
        //                oInventoryInDocument.Date = DateTime.Now;
        //                oInventoryInDocument.Type = "RPT";
        //                oInventoryInDocument.Volume = invInDoc_Invoiceum.InventoryInDTO.Volume;
        //                oInventoryInDocument.Price = invInDoc_Invoiceum.InventoryInDocumentDTO.Price;
        //                oInventoryInDocument.Updated = invInDoc_Invoiceum.InventoryInDocumentDTO.Updated;
        //                oInventoryInDocument.Active = invInDoc_Invoiceum.InventoryInDocumentDTO?.Active;
        //                oInventoryInDocument.Locked = invInDoc_Invoiceum.InventoryInDocumentDTO?.Locked;
        //                oInventoryInDocument.Deleted = invInDoc_Invoiceum.InventoryInDocumentDTO?.Deleted;

        //                context.InventoryInDocuments.Add(oInventoryInDocument);                  //Agrego o marco
        //                context.SaveChanges();                                                   //Guardo
        //                int iidInventoryInDoc = oInventoryInDocument.InventoryInDocumentIdx;    //obtuve id creado en tabla1

        //                SupplierTransportRegister oSupplierTransportRegister = new SupplierTransportRegister();
        //                oSupplierTransportRegister.SupplierTransportRegisterId = Guid.NewGuid();
        //                oSupplierTransportRegister.SupplierId = oInventoryIn.StoreId;
        //                oSupplierTransportRegister.AmountPerFee = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountPerFee;
        //                oSupplierTransportRegister.AmountPerCapacity = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountPerCapacity;
        //                oSupplierTransportRegister.AmountPerUse = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountPerUse;
        //                oSupplierTransportRegister.AmountPerVolume = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountPerVolume;
        //                oSupplierTransportRegister.AmountPerService = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountPerService;
        //                oSupplierTransportRegister.AmountOfDiscount = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountOfDiscount;
        //                oSupplierTransportRegister.Date = invInDoc_Invoiceum.SupplierTransportRegisterDTO.Date;
        //                oSupplierTransportRegister.Updated = invInDoc_Invoiceum.SupplierTransportRegisterDTO.Updated;
        //                oSupplierTransportRegister.Active = invInDoc_Invoiceum.SupplierTransportRegisterDTO.Active;
        //                oSupplierTransportRegister.Locked = invInDoc_Invoiceum.SupplierTransportRegisterDTO.Locked;
        //                oSupplierTransportRegister.Deleted = invInDoc_Invoiceum.SupplierTransportRegisterDTO.Deleted;
        //                oInventoryInDocument.SupplierTransportRegisterId = oSupplierTransportRegister.SupplierTransportRegisterId; //tabla atras

        //                context.SupplierTransportRegisters.Add(oSupplierTransportRegister);
        //                context.SaveChanges();

        //                Invoice oInvInvoice = new Invoice();
        //                oInvInvoice.InvoiceId = Guid.NewGuid();
        //                oInvInvoice.StoreId = storeId;
        //                oInvInvoice.InvoiceSerieId = invInDoc_Invoiceum.InvoiceDTO.InvoiceSerieId;
        //                oInvInvoice.Folio = invInDoc_Invoiceum.InvoiceDTO.Folio;
        //                oInvInvoice.Date = invInDoc_Invoiceum.InvoiceDTO.Date;
        //                oInvInvoice.CustomerId = invInDoc_Invoiceum.InvoiceDTO.CustomerId;
        //                oInvInvoice.Amount = invInDoc_Invoiceum.InvoiceDTO.Amount; //precio
        //                oInvInvoice.Subtotal = ((oInvInvoice.Amount) / ((decimal)1.16));
        //                oInvInvoice.AmountTax = ((oInvInvoice.Subtotal) * ((decimal)0.16));
        //                oInvInvoice.SatTipoComprobanteId = invInDoc_Invoiceum.InvoiceDTO.SatTipoComprobanteId;
        //                oInvInvoice.Updated = invInDoc_Invoiceum.InvoiceDTO.Updated;
        //                oInvInvoice.Active = invInDoc_Invoiceum.InvoiceDTO.Active;
        //                oInvInvoice.Locked = invInDoc_Invoiceum.InvoiceDTO.Locked;
        //                oInvInvoice.Deleted = invInDoc_Invoiceum.InvoiceDTO.Deleted;
        //                oInvInvoice.Uuid = invInDoc_Invoiceum.InvoiceDTO.Uuid.ToString();
        //                oInventoryInDocument.Uuid = invInDoc_Invoiceum.InvoiceDTO.Uuid;

        //                context.Invoices.Add(oInvInvoice);
        //                context.SaveChanges();

        //                //transaccion.Complete();    //Trasaccion completa
        //                await transaccion.CommitAsync();
        //                rpta = 1;                  //Si respuesta 1 esta ok
        //            }
        //            else
        //            {
        //                InventoryIn oInventoryIn = new InventoryIn();
        //                oInventoryIn.InventoryInId = invInDoc_Invoiceum.InventoryInDTO.InventoryInId;  //se genera en clase es llave para document
        //                oInventoryIn.StoreId = storeId;
        //                oInventoryIn.InventoryInNumber = invInDoc_Invoiceum.InventoryInDTO.InventoryInNumber; //capturado
        //                oInventoryIn.Date = DateTime.Now;
        //                oInventoryIn.TankIdi = invInDoc_Invoiceum.InventoryInDTO.TankIdi;
        //                oInventoryIn.ProductId = invInDoc_Invoiceum.InventoryInDTO.ProductId;
        //                oInventoryIn.StartDate = invInDoc_Invoiceum.InventoryInDTO.StartDate;
        //                oInventoryIn.EndDate = invInDoc_Invoiceum.InventoryInDTO.EndDate;
        //                oInventoryIn.StartVolume = invInDoc_Invoiceum.InventoryInDTO.StartVolume;
        //                oInventoryIn.Volume = invInDoc_Invoiceum.InventoryInDTO.Volume;
        //                oInventoryIn.EndVolume = invInDoc_Invoiceum.InventoryInDTO.EndVolume;
        //                oInventoryIn.StartTemperature = invInDoc_Invoiceum.InventoryInDTO.StartTemperature;
        //                oInventoryIn.EndTemperature = oInventoryIn.StartTemperature;
        //                oInventoryIn.AbsolutePressure = invInDoc_Invoiceum.InventoryInDTO.AbsolutePressure;
        //                oInventoryIn.CalorificPower = invInDoc_Invoiceum.InventoryInDTO.CalorificPower;
        //                oInventoryIn.Updated = invInDoc_Invoiceum.InventoryInDTO.Updated;
        //                oInventoryIn.Active = invInDoc_Invoiceum.InventoryInDTO?.Active;
        //                oInventoryIn.Locked = invInDoc_Invoiceum.InventoryInDTO?.Locked;
        //                oInventoryIn.Deleted = invInDoc_Invoiceum.InventoryInDTO.Deleted;

        //                context.InventoryIns.Update(oInventoryIn);               //Agrego o marco
        //                context.SaveChanges();                                //Guardo
        //                Guid iInventoryInId = oInventoryIn.InventoryInId;    //obtuve id creado en tabla1

        //                InventoryInDocument oInventoryInDocument = new InventoryInDocument();
        //                oInventoryInDocument.InventoryInDocumentIdx = oInventoryInDocument.InventoryInDocumentIdx;
        //                oInventoryInDocument.InventoryInId = iInventoryInId;
        //                oInventoryInDocument.StoreId = oInventoryIn.StoreId;
        //                oInventoryInDocument.InventoryInIdi = 1;
        //                oInventoryInDocument.Date = DateTime.Now;
        //                oInventoryInDocument.Type = "RPT";
        //                oInventoryInDocument.Volume = invInDoc_Invoiceum.InventoryInDTO.Volume;
        //                oInventoryInDocument.Price = invInDoc_Invoiceum.InventoryInDocumentDTO.Price;
        //                oInventoryInDocument.Updated = invInDoc_Invoiceum.InventoryInDocumentDTO.Updated;
        //                oInventoryInDocument.Active = invInDoc_Invoiceum.InventoryInDocumentDTO?.Active;
        //                oInventoryInDocument.Locked = invInDoc_Invoiceum.InventoryInDocumentDTO?.Locked;
        //                oInventoryInDocument.Deleted = invInDoc_Invoiceum.InventoryInDocumentDTO?.Deleted;

        //                context.InventoryInDocuments.Update(oInventoryInDocument);                  //Agrego o marco
        //                context.SaveChanges();                                                   //Guardo
        //                int iidInventoryInDoc = oInventoryInDocument.InventoryInDocumentIdx;    //obtuve id creado en tabla1

        //                SupplierTransportRegister oSupplierTransportRegister = new SupplierTransportRegister();
        //                oSupplierTransportRegister.SupplierTransportRegisterId = Guid.NewGuid();
        //                oSupplierTransportRegister.SupplierId = oInventoryIn.StoreId;
        //                oSupplierTransportRegister.AmountPerFee = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountPerFee;
        //                oSupplierTransportRegister.AmountPerCapacity = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountPerCapacity;
        //                oSupplierTransportRegister.AmountPerUse = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountPerUse;
        //                oSupplierTransportRegister.AmountPerVolume = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountPerVolume;
        //                oSupplierTransportRegister.AmountPerService = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountPerService;
        //                oSupplierTransportRegister.AmountOfDiscount = invInDoc_Invoiceum.SupplierTransportRegisterDTO.AmountOfDiscount;
        //                oSupplierTransportRegister.Date = invInDoc_Invoiceum.SupplierTransportRegisterDTO.Date;
        //                oSupplierTransportRegister.Updated = invInDoc_Invoiceum.SupplierTransportRegisterDTO.Updated;
        //                oSupplierTransportRegister.Active = invInDoc_Invoiceum.SupplierTransportRegisterDTO.Active;
        //                oSupplierTransportRegister.Locked = invInDoc_Invoiceum.SupplierTransportRegisterDTO.Locked;
        //                oSupplierTransportRegister.Deleted = invInDoc_Invoiceum.SupplierTransportRegisterDTO.Deleted;
        //                oInventoryInDocument.SupplierTransportRegisterId = oSupplierTransportRegister.SupplierTransportRegisterId; //tabla atras

        //                context.SupplierTransportRegisters.Update(oSupplierTransportRegister);
        //                context.SaveChanges();

        //                Invoice oInvInvoice = new Invoice();
        //                oInvInvoice.InvoiceId = Guid.NewGuid();
        //                oInvInvoice.StoreId = storeId;
        //                oInvInvoice.InvoiceSerieId = invInDoc_Invoiceum.InvoiceDTO.InvoiceSerieId;
        //                oInvInvoice.Folio = invInDoc_Invoiceum.InvoiceDTO.Folio;
        //                oInvInvoice.Date = invInDoc_Invoiceum.InvoiceDTO.Date;
        //                oInvInvoice.CustomerId = invInDoc_Invoiceum.InvoiceDTO.CustomerId;
        //                oInvInvoice.Amount = invInDoc_Invoiceum.InvoiceDTO.Amount; //precio
        //                oInvInvoice.Subtotal = ((oInvInvoice.Amount) / ((decimal)1.16));
        //                oInvInvoice.AmountTax = ((oInvInvoice.Subtotal) * ((decimal)0.16));
        //                oInvInvoice.SatTipoComprobanteId = invInDoc_Invoiceum.InvoiceDTO.SatTipoComprobanteId;
        //                oInvInvoice.Updated = invInDoc_Invoiceum.InvoiceDTO.Updated;
        //                oInvInvoice.Active = invInDoc_Invoiceum.InvoiceDTO.Active;
        //                oInvInvoice.Locked = invInDoc_Invoiceum.InvoiceDTO.Locked;
        //                oInvInvoice.Deleted = invInDoc_Invoiceum.InvoiceDTO.Deleted;
        //                oInvInvoice.Uuid = invInDoc_Invoiceum.InvoiceDTO.Uuid.ToString();
        //                oInventoryInDocument.Uuid = invInDoc_Invoiceum.InvoiceDTO.Uuid;

        //                context.Invoices.Update(oInvInvoice);
        //                context.SaveChanges();

        //                //transaccion.Complete();    //Trasaccion completa
        //                await transaccion.CommitAsync();

        //                rpta = 1;                  //Si respuesta 1 esta ok
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        rpta = 0;   // algo esta mal
        //    }
        //    return rpta;
        //}


        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<ActionResult<InventoryInDTO>> Get2([FromQuery] Guid storeId, DateTime dateIni, DateTime dateFin)
        {
            List<InventoryInDTO> listSO = new List<InventoryInDTO>();

            listSO = await (from iI in context.InventoryIns
                            where iI.Active == true
                            //join pd in context.Products on iI.ProductId equals pd.ProductId
                            //join tk in context.Tanks on pd.ProductId equals tk.ProductId
                            //join iIdoc in context.InventoryInDocuments on iI.InventoryInId equals iIdoc.InventoryInId
                            && iI.Date >= (dateIni) && iI.Date <= dateFin
                            && iI.StoreId == storeId
                            //&& tk.StoreId == storeId
                            orderby iI.InventoryInIdx descending

                            select new InventoryInDTO
                            {
                                InventoryInIdx = iI.InventoryInIdx,
                                InventoryInId = iI.InventoryInId,
                                StoreId = iI.StoreId,
                                Date = iI.Date,
                                StartDate = iI.StartDate,
                                Volume = iI.Volume,

                                //ProductName = pd.Name,
                                //TankName = tk.Name,
                                //Volume = iI.Volume,

                                //Price = iIdoc.Price

                            }).AsNoTracking().ToListAsync();
            return Ok(listSO);
        }


        //[HttpGet("{idGuid}/{storeId}")]
        //[AllowAnonymous]
        //public async Task<ActionResult<InventoryInDTO>> Get(Guid idGuid, Guid storeId)
        //{
        //    var registro = await context.InventoryInDocuments.FirstOrDefaultAsync(x => x.InventoryInId == idGuid && x.StoreId == storeId);
        //    var supTransReg = registro.SupplierTransportRegisterId;


        //    try
        //    {
        //        var data = await (from iI in context.InventoryIns
        //                          where iI.InventoryInId == idGuid

        //                          join iIdoc in context.InventoryInDocuments on iI.InventoryInId equals iIdoc.InventoryInId
        //                          join streg in context.SupplierTransportRegisters on iIdoc.SupplierTransportRegisterId equals streg.SupplierTransportRegisterId
        //                          into joinedEntities
        //                          from streg in joinedEntities.DefaultIfEmpty()

        //                          join inv in context.Invoices on iIdoc.Uuid.ToString() equals inv.Uuid
        //                          into joinedEntities2
        //                          from inv in joinedEntities2.DefaultIfEmpty()

        //                          select new InventoryInDTO
        //                          {
        //                              InventoryInIdx = iI.InventoryInIdx,
        //                              InventoryInId = iI.InventoryInId,
        //                              StoreId = iI.StoreId,
        //                              TankIdi = iI.TankIdi,
        //                              ProductId = iI.ProductId,
        //                              StartDate = iI.StartDate,
        //                              EndDate = iI.EndDate,
        //                              StartVolume = iI.StartVolume,
        //                              Volume = iI.Volume,
        //                              EndVolume = iI.EndVolume,
        //                              StartTemperature = iI.StartTemperature,
        //                              AbsolutePressure = iI.AbsolutePressure,
        //                              Price = iIdoc.Price,
        //                              SupplierId = streg.SupplierId,
        //                              AmountPerFee = streg != null ? streg.AmountPerFee : 0,
        //                              AmountPerCapacity = streg != null ? streg.AmountPerCapacity : 0,
        //                              AmountPerUse = streg != null ? streg.AmountPerUse : 0,
        //                              AmountPerVolume = streg != null ? streg.AmountPerVolume : 0,
        //                              AmountPerService = streg != null ? streg.AmountPerService : 0,
        //                              AmountOfDiscount = streg != null ? streg.AmountOfDiscount : 0,
        //                              SupplierTransportRegisterId = streg != null ? streg.SupplierTransportRegisterId : Guid.Empty,
        //                              CustomerId = inv != null ? inv.CustomerId : Guid.Empty,
        //                              SatTipoComprobanteId = inv != null ? inv.SatTipoComprobanteId : "",
        //                              InvoiceSerieId = inv != null ? inv.InvoiceSerieId : "",
        //                              Folio = inv != null ? inv.Folio : "",
        //                              Date2 = inv != null ? inv.Date : DateTime.Now,
        //                              Amount = inv != null ? inv.Amount : 0,
        //                              Uuid = inv != null ? (Guid)iIdoc.Uuid : Guid.Empty
        //                          }).FirstOrDefaultAsync();
        //        return Ok(data);
        //    }
        //    catch
        //    {

        //    }
        //    return Ok();
        //}


        //[HttpPut("{storeId?}")]
        //public async Task<IActionResult> Put(InventoryInDTO inventoryInDTO, Guid storeId)
        //{
        //    var findDb = await context.InventoryIns.FirstOrDefaultAsync(c => c.InventoryInIdx == inventoryInDTO.InventoryInIdx);

        //    if (findDb is null)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {
        //        findDb = mapper.Map(inventoryInDTO, findDb);

        //        var storeId2 = storeId;
        //        var usuarioId = obtenerUsuarioId();
        //        var ipUser = obtenetIP();
        //        var tableName = findDb.InventoryInNumber;
        //        await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName.ToString(), storeId2);
        //        await context.SaveChangesAsync();
        //        return Ok(findDb);

        //    }
        //    catch
        //    {
        //        return BadRequest($"Ya existe compra {inventoryInDTO.InventoryInNumber} en esa sucursal ");
        //    }

        //}


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        //[AllowAnonymous]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.InventoryIns.AnyAsync(x => x.InventoryInIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.InventoryIns.FirstOrDefaultAsync(x => x.InventoryInIdx == id);
            name2.Active = false;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.InventoryInNumber;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name.ToString(), storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        //[AllowAnonymous]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            try
            {
                var existe = await context.InventoryIns.AnyAsync(x => x.InventoryInIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.InventoryIns.FirstOrDefaultAsync(x => x.InventoryInIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.InventoryInNumber;
                var storeId2 = storeId;
                await servicioBinnacle.deleteBinnacle(usuarioId, ipUser, name.ToString(), storeId2);

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return BadRequest("ERROR DE DATOS RELACIONADOS");
            }

        }


        //[HttpGet("sinPag2")]
        ////[AllowAnonymous]
        //public async Task<IEnumerable<InventoryInDTO>> Get([FromQuery] Guid storeId, DateTime dateIni, DateTime dateFin, string nombre)
        //{
        //    //var queryable = await context.InventoryIns.Join(context.Products, ii => ii.ProductId, p => p.ProductId, (ii, prod)

        //    var queryable = context.InventoryIns.Where(x => x.Date <= dateFin && x.Date >= dateIni).AsQueryable();

        //    var inventoryin = await queryable.OrderByDescending(x => x.InventoryInIdx)
        //    .Include(x => x.InventoryInDocuments)
        //    .AsNoTracking()
        //    .ToListAsync();
        //    return mapper.Map<List<InventoryInDTO>>(inventoryin);

        //    //var queryable = context.InventoryIns.Where(x => x.Date <= dateFin && x.Date >= dateIni)
        //    //    .Join(context.Products,
        //    //        t1 => t1.ProductId,
        //    //        t2 => t2.ProductId,
        //    //        (t1, t2) => new { t1, t2 })
        //    //    .Join(context.Tanks,
        //    //        tt => tt.t2.ProductId,
        //    //        t3 => t3.ProductId,
        //    //        (tt, t3) => new { tt.t1, tt.t2, t3 })
        //    //    .Select(ttt => new InventoryInDTO
        //    //    {
        //    //        InventoryInIdx = ttt.t1.InventoryInIdx,
        //    //        InventoryInId = ttt.t1.InventoryInId,
        //    //        ProductName = ttt.t2.Name,
        //    //        TankName = ttt.t3.Name
        //    //    }).AsQueryable();

        //    //var inventoruIn = await queryable.OrderByDescending(x => x.InventoryInIdx)
        //    //    .AsNoTracking()
        //    //    .ToListAsync();
        //    //return mapper.Map<List<InventoryInDTO>>(inventoruIn);
        //}
    }


}

