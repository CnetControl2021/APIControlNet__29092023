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
        //public async Task<int> Save([FromBody] InvIn_InvInDoc invIn_InvInDoc, Guid storeId, Guid idGuid)
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
        //            oInventoryIn.InventoryInNumber = invIn_InvInDoc.inventoryInDTO.InventoryInNumber; //capturado
        //            oInventoryIn.Date = DateTime.Now;
        //            oInventoryIn.TankIdi = invIn_InvInDoc.inventoryInDTO.TankIdi;
        //            oInventoryIn.ProductId = invIn_InvInDoc.inventoryInDTO.ProductId;
        //            oInventoryIn.StartDate = invIn_InvInDoc.inventoryInDTO.StartDate;
        //            oInventoryIn.EndDate = invIn_InvInDoc.inventoryInDTO.EndDate;
        //            oInventoryIn.StartVolume = invIn_InvInDoc.inventoryInDTO.StartVolume;
        //            oInventoryIn.Volume = invIn_InvInDoc.inventoryInDTO.Volume;
        //            oInventoryIn.EndVolume = invIn_InvInDoc.inventoryInDTO.EndVolume;
        //            oInventoryIn.StartTemperature = invIn_InvInDoc.inventoryInDTO.StartTemperature;
        //            oInventoryIn.EndTemperature = oInventoryIn.StartTemperature;
        //            oInventoryIn.AbsolutePressure = invIn_InvInDoc.inventoryInDTO.AbsolutePressure;
        //            oInventoryIn.CalorificPower = invIn_InvInDoc.inventoryInDTO.CalorificPower;
        //            oInventoryIn.Updated = invIn_InvInDoc.inventoryInDTO.Updated;
        //            oInventoryIn.Active = invIn_InvInDoc.inventoryInDTO.Active;
        //            oInventoryIn.Locked = invIn_InvInDoc.inventoryInDTO.Locked;
        //            oInventoryIn.Deleted = invIn_InvInDoc.inventoryInDTO.Deleted;

        //            context.InventoryIns.Add(oInventoryIn);

        //            InventoryInDocument oInventoryInDocument = new InventoryInDocument();

        //            oInventoryInDocument.InventoryInId = oInventoryIn.InventoryInId;
        //            oInventoryInDocument.StoreId = oInventoryIn.StoreId;
        //            oInventoryInDocument.InventoryInIdi = 1;
        //            oInventoryInDocument.Date = DateTime.Now;
        //            oInventoryInDocument.Type = "RPT";
        //            oInventoryInDocument.Volume = oInventoryIn.Volume;
        //            oInventoryInDocument.Price = invIn_InvInDoc.inventoryInDocumentDTO.Price;
        //            oInventoryInDocument.Updated = invIn_InvInDoc.inventoryInDocumentDTO.Updated;
        //            oInventoryInDocument.Active = invIn_InvInDoc.inventoryInDocumentDTO.Active;
        //            oInventoryInDocument.Locked = invIn_InvInDoc.inventoryInDocumentDTO.Locked;
        //            oInventoryInDocument.Deleted = invIn_InvInDoc.inventoryInDocumentDTO.Deleted;

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
        //public async Task<IActionResult> Put(Guid idGuid, InvIn_InvInDoc invIn_InvInDoc)
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
        //    inv.TankIdi = invIn_InvInDoc.inventoryInDTO.TankIdi;
        //    inv.ProductId = invIn_InvInDoc.inventoryInDTO.ProductId;
        //    inv.StartDate = invIn_InvInDoc.inventoryInDTO.StartDate;
        //    inv.EndDate = invIn_InvInDoc.inventoryInDTO.EndDate;
        //    inv.StartVolume = invIn_InvInDoc.inventoryInDTO.StartVolume;
        //    inv.Volume = invIn_InvInDoc.inventoryInDTO?.Volume;
        //    inv.EndVolume = invIn_InvInDoc?.inventoryInDTO?.EndVolume;
        //    inv.StartTemperature = invIn_InvInDoc.inventoryInDTO.StartTemperature;
        //    inv.AbsolutePressure = invIn_InvInDoc.inventoryInDTO.AbsolutePressure;

        //    var upd1 = new InventoryInDTO
        //    {
        //        InventoryInId = inv.InventoryInId,
        //        StartVolume = inv.StartVolume,
        //    };
        //    ////
        //    invDoc.InventoryInId = idGuid;
        //    invDoc.Price = invIn_InvInDoc.inventoryInDocumentDTO.Price;

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
        //public async Task<ActionResult<InvIn_InvInDoc>> GetClaseEmpaquetada(Guid idGuid)
        //{
        //    var iI = await context.InventoryIns.FirstOrDefaultAsync(c => c.InventoryInId == idGuid);
        //    var iIDoc = await context.InventoryInDocuments.FirstOrDefaultAsync(c => c.InventoryInId == idGuid);
        //    //chema
        //    var claseEmpaquetada = new InvIn_InvInDoc
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
        //public async Task<int> Save([FromBody] InvIn_InvInDocum invIn_InvInDocum, Guid storeId, Guid idGuid)
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
        //                oInventoryIn.InventoryInId = invIn_InvInDocum.InventoryInDTO.InventoryInId;  //se genera en clase es llave para document
        //                oInventoryIn.StoreId = storeId;
        //                oInventoryIn.InventoryInNumber = invIn_InvInDocum.InventoryInDTO.InventoryInNumber; //capturado
        //                oInventoryIn.Date = DateTime.Now;
        //                oInventoryIn.TankIdi = invIn_InvInDocum.InventoryInDTO.TankIdi;
        //                oInventoryIn.ProductId = invIn_InvInDocum.InventoryInDTO.ProductId;
        //                oInventoryIn.StartDate = invIn_InvInDocum.InventoryInDTO.StartDate;
        //                oInventoryIn.EndDate = invIn_InvInDocum.InventoryInDTO.EndDate;
        //                oInventoryIn.StartVolume = invIn_InvInDocum.InventoryInDTO.StartVolume;
        //                oInventoryIn.Volume = invIn_InvInDocum.InventoryInDTO.Volume;
        //                oInventoryIn.EndVolume = invIn_InvInDocum.InventoryInDTO.EndVolume;
        //                oInventoryIn.StartTemperature = invIn_InvInDocum.InventoryInDTO.StartTemperature;
        //                oInventoryIn.EndTemperature = oInventoryIn.StartTemperature;
        //                oInventoryIn.AbsolutePressure = invIn_InvInDocum.InventoryInDTO.AbsolutePressure;
        //                oInventoryIn.CalorificPower = invIn_InvInDocum.InventoryInDTO.CalorificPower;
        //                oInventoryIn.Updated = invIn_InvInDocum.InventoryInDTO.Updated;
        //                oInventoryIn.Active = invIn_InvInDocum.InventoryInDTO?.Active;
        //                oInventoryIn.Locked = invIn_InvInDocum.InventoryInDTO?.Locked;
        //                oInventoryIn.Deleted = invIn_InvInDocum.InventoryInDTO.Deleted;

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
        //                oInventoryInDocument.Volume = invIn_InvInDocum.InventoryInDTO.Volume;
        //                oInventoryInDocument.Price = invIn_InvInDocum.InventoryInDocumentDTO.Price;
        //                oInventoryInDocument.Updated = invIn_InvInDocum.InventoryInDocumentDTO.Updated;
        //                oInventoryInDocument.Active = invIn_InvInDocum.InventoryInDocumentDTO?.Active;
        //                oInventoryInDocument.Locked = invIn_InvInDocum.InventoryInDocumentDTO?.Locked;
        //                oInventoryInDocument.Deleted = invIn_InvInDocum.InventoryInDocumentDTO?.Deleted;

        //                context.InventoryInDocuments.Add(oInventoryInDocument);                  //Agrego o marco
        //                context.SaveChanges();                                                   //Guardo
        //                int iidInventoryInDoc = oInventoryInDocument.InventoryInDocumentIdx;    //obtuve id creado en tabla1

        //                SupplierTransportRegister oSupplierTransportRegister = new SupplierTransportRegister();
        //                oSupplierTransportRegister.SupplierTransportRegisterId = Guid.NewGuid();
        //                oSupplierTransportRegister.SupplierId = oInventoryIn.StoreId;
        //                oSupplierTransportRegister.AmountPerFee = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountPerFee;
        //                oSupplierTransportRegister.AmountPerCapacity = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountPerCapacity;
        //                oSupplierTransportRegister.AmountPerUse = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountPerUse;
        //                oSupplierTransportRegister.AmountPerVolume = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountPerVolume;
        //                oSupplierTransportRegister.AmountPerService = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountPerService;
        //                oSupplierTransportRegister.AmountOfDiscount = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountOfDiscount;
        //                oSupplierTransportRegister.Date = invIn_InvInDocum.SupplierTransportRegisterDTO.Date;
        //                oSupplierTransportRegister.Updated = invIn_InvInDocum.SupplierTransportRegisterDTO.Updated;
        //                oSupplierTransportRegister.Active = invIn_InvInDocum.SupplierTransportRegisterDTO.Active;
        //                oSupplierTransportRegister.Locked = invIn_InvInDocum.SupplierTransportRegisterDTO.Locked;
        //                oSupplierTransportRegister.Deleted = invIn_InvInDocum.SupplierTransportRegisterDTO.Deleted;
        //                oInventoryInDocument.SupplierTransportRegisterId = oSupplierTransportRegister.SupplierTransportRegisterId; //tabla atras

        //                context.SupplierTransportRegisters.Add(oSupplierTransportRegister);
        //                context.SaveChanges();

        //                Invoice oInvInvoice = new Invoice();
        //                oInvInvoice.InvoiceId = Guid.NewGuid();
        //                oInvInvoice.StoreId = storeId;
        //                oInvInvoice.InvoiceSerieId = invIn_InvInDocum.InvoiceDTO.InvoiceSerieId;
        //                oInvInvoice.Folio = invIn_InvInDocum.InvoiceDTO.Folio;
        //                oInvInvoice.Date = invIn_InvInDocum.InvoiceDTO.Date;
        //                oInvInvoice.CustomerId = invIn_InvInDocum.InvoiceDTO.CustomerId;
        //                oInvInvoice.Amount = invIn_InvInDocum.InvoiceDTO.Amount; //precio
        //                oInvInvoice.Subtotal = ((oInvInvoice.Amount) / ((decimal)1.16));
        //                oInvInvoice.AmountTax = ((oInvInvoice.Subtotal) * ((decimal)0.16));
        //                oInvInvoice.SatTipoComprobanteId = invIn_InvInDocum.InvoiceDTO.SatTipoComprobanteId;
        //                oInvInvoice.Updated = invIn_InvInDocum.InvoiceDTO.Updated;
        //                oInvInvoice.Active = invIn_InvInDocum.InvoiceDTO.Active;
        //                oInvInvoice.Locked = invIn_InvInDocum.InvoiceDTO.Locked;
        //                oInvInvoice.Deleted = invIn_InvInDocum.InvoiceDTO.Deleted;
        //                oInvInvoice.Uuid = invIn_InvInDocum.InvoiceDTO.Uuid.ToString();
        //                oInventoryInDocument.Uuid = invIn_InvInDocum.InvoiceDTO.Uuid;

        //                context.Invoices.Add(oInvInvoice);
        //                context.SaveChanges();

        //                //transaccion.Complete();    //Trasaccion completa
        //                await transaccion.CommitAsync();
        //                rpta = 1;                  //Si respuesta 1 esta ok
        //            }
        //            else
        //            {
        //                InventoryIn oInventoryIn = new InventoryIn();
        //                oInventoryIn.InventoryInId = invIn_InvInDocum.InventoryInDTO.InventoryInId;  //se genera en clase es llave para document
        //                oInventoryIn.StoreId = storeId;
        //                oInventoryIn.InventoryInNumber = invIn_InvInDocum.InventoryInDTO.InventoryInNumber; //capturado
        //                oInventoryIn.Date = DateTime.Now;
        //                oInventoryIn.TankIdi = invIn_InvInDocum.InventoryInDTO.TankIdi;
        //                oInventoryIn.ProductId = invIn_InvInDocum.InventoryInDTO.ProductId;
        //                oInventoryIn.StartDate = invIn_InvInDocum.InventoryInDTO.StartDate;
        //                oInventoryIn.EndDate = invIn_InvInDocum.InventoryInDTO.EndDate;
        //                oInventoryIn.StartVolume = invIn_InvInDocum.InventoryInDTO.StartVolume;
        //                oInventoryIn.Volume = invIn_InvInDocum.InventoryInDTO.Volume;
        //                oInventoryIn.EndVolume = invIn_InvInDocum.InventoryInDTO.EndVolume;
        //                oInventoryIn.StartTemperature = invIn_InvInDocum.InventoryInDTO.StartTemperature;
        //                oInventoryIn.EndTemperature = oInventoryIn.StartTemperature;
        //                oInventoryIn.AbsolutePressure = invIn_InvInDocum.InventoryInDTO.AbsolutePressure;
        //                oInventoryIn.CalorificPower = invIn_InvInDocum.InventoryInDTO.CalorificPower;
        //                oInventoryIn.Updated = invIn_InvInDocum.InventoryInDTO.Updated;
        //                oInventoryIn.Active = invIn_InvInDocum.InventoryInDTO?.Active;
        //                oInventoryIn.Locked = invIn_InvInDocum.InventoryInDTO?.Locked;
        //                oInventoryIn.Deleted = invIn_InvInDocum.InventoryInDTO.Deleted;

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
        //                oInventoryInDocument.Volume = invIn_InvInDocum.InventoryInDTO.Volume;
        //                oInventoryInDocument.Price = invIn_InvInDocum.InventoryInDocumentDTO.Price;
        //                oInventoryInDocument.Updated = invIn_InvInDocum.InventoryInDocumentDTO.Updated;
        //                oInventoryInDocument.Active = invIn_InvInDocum.InventoryInDocumentDTO?.Active;
        //                oInventoryInDocument.Locked = invIn_InvInDocum.InventoryInDocumentDTO?.Locked;
        //                oInventoryInDocument.Deleted = invIn_InvInDocum.InventoryInDocumentDTO?.Deleted;

        //                context.InventoryInDocuments.Update(oInventoryInDocument);                  //Agrego o marco
        //                context.SaveChanges();                                                   //Guardo
        //                int iidInventoryInDoc = oInventoryInDocument.InventoryInDocumentIdx;    //obtuve id creado en tabla1

        //                SupplierTransportRegister oSupplierTransportRegister = new SupplierTransportRegister();
        //                oSupplierTransportRegister.SupplierTransportRegisterId = Guid.NewGuid();
        //                oSupplierTransportRegister.SupplierId = oInventoryIn.StoreId;
        //                oSupplierTransportRegister.AmountPerFee = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountPerFee;
        //                oSupplierTransportRegister.AmountPerCapacity = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountPerCapacity;
        //                oSupplierTransportRegister.AmountPerUse = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountPerUse;
        //                oSupplierTransportRegister.AmountPerVolume = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountPerVolume;
        //                oSupplierTransportRegister.AmountPerService = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountPerService;
        //                oSupplierTransportRegister.AmountOfDiscount = invIn_InvInDocum.SupplierTransportRegisterDTO.AmountOfDiscount;
        //                oSupplierTransportRegister.Date = invIn_InvInDocum.SupplierTransportRegisterDTO.Date;
        //                oSupplierTransportRegister.Updated = invIn_InvInDocum.SupplierTransportRegisterDTO.Updated;
        //                oSupplierTransportRegister.Active = invIn_InvInDocum.SupplierTransportRegisterDTO.Active;
        //                oSupplierTransportRegister.Locked = invIn_InvInDocum.SupplierTransportRegisterDTO.Locked;
        //                oSupplierTransportRegister.Deleted = invIn_InvInDocum.SupplierTransportRegisterDTO.Deleted;
        //                oInventoryInDocument.SupplierTransportRegisterId = oSupplierTransportRegister.SupplierTransportRegisterId; //tabla atras

        //                context.SupplierTransportRegisters.Update(oSupplierTransportRegister);
        //                context.SaveChanges();

        //                Invoice oInvInvoice = new Invoice();
        //                oInvInvoice.InvoiceId = Guid.NewGuid();
        //                oInvInvoice.StoreId = storeId;
        //                oInvInvoice.InvoiceSerieId = invIn_InvInDocum.InvoiceDTO.InvoiceSerieId;
        //                oInvInvoice.Folio = invIn_InvInDocum.InvoiceDTO.Folio;
        //                oInvInvoice.Date = invIn_InvInDocum.InvoiceDTO.Date;
        //                oInvInvoice.CustomerId = invIn_InvInDocum.InvoiceDTO.CustomerId;
        //                oInvInvoice.Amount = invIn_InvInDocum.InvoiceDTO.Amount; //precio
        //                oInvInvoice.Subtotal = ((oInvInvoice.Amount) / ((decimal)1.16));
        //                oInvInvoice.AmountTax = ((oInvInvoice.Subtotal) * ((decimal)0.16));
        //                oInvInvoice.SatTipoComprobanteId = invIn_InvInDocum.InvoiceDTO.SatTipoComprobanteId;
        //                oInvInvoice.Updated = invIn_InvInDocum.InvoiceDTO.Updated;
        //                oInvInvoice.Active = invIn_InvInDocum.InvoiceDTO.Active;
        //                oInvInvoice.Locked = invIn_InvInDocum.InvoiceDTO.Locked;
        //                oInvInvoice.Deleted = invIn_InvInDocum.InvoiceDTO.Deleted;
        //                oInvInvoice.Uuid = invIn_InvInDocum.InvoiceDTO.Uuid.ToString();
        //                oInventoryInDocument.Uuid = invIn_InvInDocum.InvoiceDTO.Uuid;

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
                            join pd in context.Products on iI.ProductId equals pd.ProductId
                            join tk in context.Tanks on pd.ProductId equals tk.ProductId
                            join iIdoc in context.InventoryInDocuments on iI.InventoryInId equals iIdoc.InventoryInId
                            where iI.Date >= (dateIni) && iI.Date <= dateFin
                            && iI.StoreId == storeId
                            && tk.StoreId == storeId
                            orderby iI.InventoryInIdx descending

                            select new InventoryInDTO
                            {
                                InventoryInIdx = iI.InventoryInIdx,
                                InventoryInId = iI.InventoryInId,
                                StoreId = iI.StoreId,
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

