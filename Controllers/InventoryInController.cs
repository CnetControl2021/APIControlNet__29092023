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
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using System.Xml;

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


        //[HttpPost("{storeId}")]
        ////[AllowAnonymous]
        //public async Task<ActionResult> Post([FromBody] InventoryIn oInventoryIn, Guid storeId, Guid idGuid)
        //{
        //    if (idGuid != Guid.Empty)
        //    {
        //        var existeid = await context.InventoryIns.AnyAsync(x => x.InventoryInId == idGuid);
        //    }

        //    try
        //    {
        //        InventoryIn newIi = new InventoryIn();
        //        newIi.InventoryInId = Guid.NewGuid();
        //        newIi.StoreId = storeId;
        //        newIi.InventoryInNumber = oInventoryIn.InventoryInNumber; //capturado
        //        newIi.StartDate = oInventoryIn.StartDate;
        //        newIi.EndDate = oInventoryIn.EndDate;
        //        newIi.TankIdi = oInventoryIn.TankIdi; 
        //        newIi.Date = DateTime.Now;              
        //        newIi.ProductId = oInventoryIn.ProductId;
        //        newIi.StartVolume = oInventoryIn.StartVolume;
        //        newIi.Volume = oInventoryIn.Volume;
        //        newIi.EndVolume = oInventoryIn.EndVolume;
        //        newIi.StartTemperature = oInventoryIn.StartTemperature;
        //        newIi.EndTemperature = oInventoryIn.StartTemperature;
        //        newIi.AbsolutePressure = oInventoryIn.AbsolutePressure;
        //        newIi.CalorificPower = oInventoryIn.CalorificPower;
        //        newIi.Updated = DateTime.Now;
        //        newIi.Active = true;
        //        newIi.Locked = false;
        //        newIi.Deleted = false;

        //        context.InventoryIns.Add(newIi);
        //        context.SaveChanges();

        //        return Ok();
        //        // }
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Revisar captura");
        //    }
        //}

        [HttpPost("{storeId}")]
        //[AllowAnonymous]
        public async Task<ActionResult> Save([FromBody] InvIn_InvInDoc invInDoc_Invoice, Guid storeId)
        {
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    InventoryIn oInventoryIn = new()
                    {
                        InventoryInId = Guid.NewGuid(),
                        StoreId = storeId,
                        InventoryInNumber = invInDoc_Invoice.NInventoryInDTO.InventoryInNumber, //capturado
                        TankIdi = invInDoc_Invoice.NInventoryInDTO.TankIdi,
                        ProductId = invInDoc_Invoice.NInventoryInDTO.ProductId,
                        StartVolume = invInDoc_Invoice.NInventoryInDTO.StartVolume,
                        Volume = invInDoc_Invoice.NInventoryInDTO.Volume,
                        EndVolume = invInDoc_Invoice.NInventoryInDTO.EndVolume,
                        StartTemperature = invInDoc_Invoice.NInventoryInDTO.StartTemperature,
                        AbsolutePressure = invInDoc_Invoice.NInventoryInDTO.AbsolutePressure,
                        CalorificPower = invInDoc_Invoice.NInventoryInDTO.CalorificPower,
                        StartDate = invInDoc_Invoice.NInventoryInDTO.StartDate,
                        EndDate = invInDoc_Invoice.NInventoryInDTO.EndDate,
                        Date = DateTime.Now,
                        Updated = DateTime.Now,
                        Active = true,
                        Locked = false,
                        Deleted = false
                    };

                    context.InventoryIns.Add(oInventoryIn);
                    var id = oInventoryIn.InventoryInId;

                    var findTk = await context.Tanks.FirstOrDefaultAsync
                        (x => x.StoreId == storeId && x.TankIdi == invInDoc_Invoice.NInventoryInDTO.TankIdi);
                    var uMedtank = findTk.SatDescriptionMeasurement;


                    InventoryInDocument oInventoryInDocument = new()
                    {
                        InventoryInId = id, //clase1
                        StoreId = oInventoryIn.StoreId, //clase1
                        InventoryInIdi = 1,
                        Date = DateTime.Now,
                        Type = "RPT",
                        Folio = (int)oInventoryIn.InventoryInNumber, // de calse 1
                        Volume = oInventoryIn.Volume,              // clase1
                        Price = invInDoc_Invoice.NInventoryInDocumentDTO.Price,   
                        Amount = invInDoc_Invoice.NInventoryInDocumentDTO.Amount,
                        JsonClaveUnidadMedidaId = uMedtank,
                        Updated = DateTime.Now,
                        Active = true,
                        Locked = false,
                        Deleted = false
                    };

                    context.InventoryInDocuments.Add(oInventoryInDocument);
                    context.SaveChanges();

                    await transaccion.CommitAsync();
                    return Ok($"Registro correcto {oInventoryIn.InventoryInNumber}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


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
        public async Task<ActionResult<InvIn_InvInDoc>> Get([FromRoute] Guid idGuid)
        {
            if (idGuid == Guid.Empty) {  return BadRequest("Id no valido"); }
            var resp = await context.InventoryIns.FirstAsync(e => e.InventoryInId.Equals(idGuid));
            if (resp == null) { return NotFound("No encontrado"); }
            var resp2 = await context.InventoryInDocuments.FirstAsync(e => e.InventoryInId.Equals(idGuid));
            if (resp2 == null) { return NotFound("No encontrado"); }

            var empaquetada = new InvIn_InvInDoc
            {
                NInventoryInDTO = mapper.Map<InventoryInDTO>(resp),
                NInventoryInDocumentDTO = mapper.Map<InventoryInDocumentDTO>(resp2)
                
            };
            return Ok(empaquetada);
        }


        [HttpPut("{storeId?}")]
        public async Task<IActionResult> Put(InvIn_InvInDoc invInDoc_Invoice, Guid storeId)
        {
            var db = await context.InventoryIns.FirstOrDefaultAsync(c => c.InventoryInId == invInDoc_Invoice.NInventoryInDTO.InventoryInId);
            var db2 = await context.InventoryInDocuments.FirstOrDefaultAsync(c => c.InventoryInId == invInDoc_Invoice.NInventoryInDocumentDTO.InventoryInId);

            if (db is null)
            {
                return NotFound();
            }
            try
            {
                db = mapper.Map(invInDoc_Invoice.NInventoryInDTO, db);            
                db.Updated = DateTime.Now;
                db2 = mapper.Map(invInDoc_Invoice.NInventoryInDocumentDTO, db2);
                db2.Updated = DateTime.Now; 


                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = db.Name;
                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
                //return Ok(db);
                var empaquetada = new InvIn_InvInDoc
                {
                    NInventoryInDTO = mapper.Map<InventoryInDTO>(db),
                    NInventoryInDocumentDTO = mapper.Map<InventoryInDocumentDTO>(db2)

                };
                return Ok(empaquetada);
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

            if (dateIni.ToString() is null || dateIni.ToString() is "" || dateIni <= DateTime.MinValue)
            { dateIni = DateTime.Now.AddDays(-30); }

            if (dateFin.ToString() is null || dateFin.ToString() is "" || dateFin <= DateTime.MinValue) 
            { dateFin = DateTime.Now; }

            if (storeId == Guid.Empty) { return BadRequest("Sucursal no valida"); }

            listSO = await (from iI in context.InventoryIns
                            where iI.Active == true
                            && iI.Date >= (dateIni) && iI.Date <= dateFin
                            && iI.StoreId == storeId
                            join pd in context.Products on iI.ProductId equals pd.ProductId
                            //join tk in context.Tanks on pd.ProductId equals tk.ProductId
                            //where iI.StoreId == tk.StoreId
                            //join iIdoc in context.InventoryInDocuments on iI.InventoryInId equals iIdoc.InventoryInId
                            //where iI.StoreId == iIdoc.StoreId
                            //orderby iI.InventoryInIdx descending

                            select new InventoryInDTO
                            {
                                InventoryInIdx = iI.InventoryInIdx,
                                InventoryInId = iI.InventoryInId,
                                InventoryInNumber = iI.InventoryInNumber,
                                TankIdi = iI.TankIdi,
                                StoreId = iI.StoreId,
                                Date = iI.Date,
                                StartDate = iI.StartDate,
                                Volume = iI.Volume,
                                VolumeTc = iI.VolumeTc,
                                ProductName = pd.Name
                                //TankName = tk.Name,
                                //Price = iIdoc.Price

                            }).OrderByDescending(x => x.Date).AsNoTracking().ToListAsync();
            return Ok(listSO);
        }


        //[HttpGet("takeLast")]
        ////[AllowAnonymous]
        //public async Task<ActionResult<List<InventoryInDTO>>> Get3( Guid storeId)
        //{
        //    if (storeId == Guid.Empty) { return BadRequest("Sucursal no valida"); }

        //    var list = await context.InventoryIns.Where(x => x.StoreId == storeId )
        //        .OrderByDescending(x => x.InventoryInIdx)
        //        .ToListAsync();
        //    var lisTake = list.Take(25);
        //    return mapper.Map<List<InventoryInDTO>>(lisTake);
        //}

        //[HttpGet("takeLast")]
        //[AllowAnonymous]
        //public async Task<ActionResult<List<InvInDoc_InvoiceDTO>>> Get3(Guid storeId)
        //{
        //    if (storeId == Guid.Empty) { return BadRequest("Sucursal no valida"); }

        //    var list = await context.InventoryIns.Where(x => x.StoreId == storeId)
        //        .OrderByDescending(x => x.InventoryInIdx)
        //        .ToListAsync();
        //    var lisTake = list.Take(3);

        //    var list2 = await context.InventoryInDocuments.Where(x => x.StoreId == storeId)
        //        .OrderByDescending(x => x.InventoryInDocumentIdx)
        //        .ToListAsync();
        //    var lisTake2 = list.Take(3);

        //    //var claseEmpaquetada = new InvInDoc_InvoiceDTO
        //    //{
        //    //    inventoryInDTO = mapper.Map<InventoryInDTO>(lisTake),

        //    //    inventoryInDocumentDTO = mapper.Map<InventoryInDocumentDTO>(lisTake2)
        //    //};
        //    //return Ok( new { claseEmpaquetada } );

        //    return Ok( new { lisTake, lisTake2 } );

        //}


        //[HttpGet("empaquetada/{id2}/{storeId}")]
        ////[AllowAnonymous]
        //public async Task<ActionResult<InvInDoc_InvoiceDTO>> GetId(int id2, Guid storeId)
        //{
        //    var iIDoc = await context.InventoryInDocuments.FirstOrDefaultAsync(c => c.InventoryInDocumentIdx == id2 && c.StoreId == storeId);
        //    var invoice2 = await context.Invoices.FirstOrDefaultAsync(x => x.InvoiceId == iIDoc.InvoiceId);
        //    //chema
        //    var claseEmpaquetada = new InvInDoc_InvoiceDTO
        //    {
        //        inventoryInDocumentDTO = mapper.Map<InventoryInDocumentDTO>(iIDoc),
        //        invoiceDTO = mapper.Map<InvoiceDTO>(invoice2)
        //    };
        //    return claseEmpaquetada;
        //}


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

