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
using System;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SupplierTransRegController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public SupplierTransRegController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        [HttpGet("byGuid/Sale/{idGuid?}")]
        [AllowAnonymous]
        public async Task<IEnumerable<SupplierTransportRegisterDTO>> Get3(Guid idGuid)
        {
            var findDB = await context.SaleSuborders.Where(x => x.SaleOrderId == idGuid).FirstOrDefaultAsync();
            var data = await context.SupplierTransportRegisters.Where(x => x.SupplierTransportRegisterId == findDB.SupplierTransportRegisterId).ToListAsync();

            return mapper.Map<List<SupplierTransportRegisterDTO>>(data);
        }


        [HttpGet("byGuid/{idGuid?}")]
        [AllowAnonymous]
        public async Task<IEnumerable<SupplierTransportRegisterDTO>> Get4(Guid idGuid)
        {
            var findDB = await context.InventoryInDocuments.Where(x => x.InventoryInId == idGuid).FirstOrDefaultAsync();
            var data = await context.SupplierTransportRegisters.Where(x => x.SupplierTransportRegisterId == findDB.SupplierTransportRegisterId).ToListAsync();

            return mapper.Map<List<SupplierTransportRegisterDTO>>(data);
        }


        [HttpGet("{id2:int}", Name = "obtenerStr")]
        //[AllowAnonymous]
        public async Task<ActionResult<SupplierTransportRegisterDTO>> Get(int id2)
        {
            var data = await context.SupplierTransportRegisters.FirstOrDefaultAsync(x => x.SupplierTransportRegisterIdx == id2);
            if (data == null)
            {
                return NotFound();
            }
            return mapper.Map<SupplierTransportRegisterDTO>(data);
        }

        [HttpPut("{storeId}")]
        public async Task<ActionResult> Put(Guid storeId, SupplierTransportRegisterDTO str)
        {
            var dataDB = await context.SupplierTransportRegisters.FirstOrDefaultAsync
                (c => c.SupplierTransportRegisterIdx == str.SupplierTransportRegisterIdx);

            if (dataDB is null)
            {
                return NotFound();
            }
            try
            {
                dataDB = mapper.Map(str, dataDB);

                var storeId2 = storeId;
                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var tableName = "SupplierTransportResgister";

                await servicioBinnacle.EditBinnacle(usuarioId, ipUser, tableName, storeId2);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest($"Ya existe {str.SupplierTransportRegisterIdx} en esa sucursal ");
            }
        }


        [HttpPost("{idGuid}")]
        [AllowAnonymous]
        public async Task<int> Post([FromBody] SupplierTransportRegisterDTO supplierTR, Guid idGuid)
        {
            int rpta = 0;
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    SupplierTransportRegister str = new();

                    str.SupplierTransportRegisterId = Guid.NewGuid();
                    str.SupplierId = supplierTR.SupplierId;
                    str.AmountPerFee = supplierTR.AmountPerFee; //importe por tarifa
                    str.AmountPerCapacity = supplierTR.AmountPerCapacity;
                    str.AmountPerUse = supplierTR.AmountPerUse;
                    str.AmountPerVolume = supplierTR.AmountPerVolume;
                    str.AmountPerService = supplierTR.AmountPerService;
                    str.AmountOfDiscount = supplierTR.AmountOfDiscount;
                    str.Date = DateTime.Now;
                    str.Updated = DateTime.Now;
                    str.Active = 1;
                    str.Locked = 0;
                    str.Deleted = 0;

                    context.SupplierTransportRegisters.Add(str);
                    //context.SaveChanges();

                    InventoryInDocument iiDoc = await context.InventoryInDocuments.Where(x => x.InventoryInId == idGuid).FirstOrDefaultAsync();

                    //iiDoc.InventoryInId = idGuid;
                    iiDoc.SupplierTransportRegisterId = str.SupplierTransportRegisterId;

                    context.InventoryInDocuments.Update(iiDoc);
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

        [HttpPost("sale/{idGuid}")]
        [AllowAnonymous]
        public async Task<int> PostSale([FromBody] SupplierTransportRegisterDTO supplierTR, Guid idGuid)
        {
            int rpta = 0;
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    SupplierTransportRegister str = new();

                    str.SupplierTransportRegisterId = Guid.NewGuid();
                    str.SupplierId = supplierTR.SupplierId;
                    str.AmountPerFee = supplierTR.AmountPerFee; //importe por tarifa
                    str.AmountPerCapacity = supplierTR.AmountPerCapacity;
                    str.AmountPerUse = supplierTR.AmountPerUse;
                    str.AmountPerVolume = supplierTR.AmountPerVolume;
                    str.AmountPerService = supplierTR.AmountPerService;
                    str.AmountOfDiscount = supplierTR.AmountOfDiscount;
                    str.Date = DateTime.Now;
                    str.Updated = DateTime.Now;
                    str.Active = 1;
                    str.Locked = 0;
                    str.Deleted = 0;

                    context.SupplierTransportRegisters.Add(str);
                    //context.SaveChanges();

                    SaleSuborder saSub = await context.SaleSuborders.Where(x => x.SaleOrderId == idGuid).FirstOrDefaultAsync();

                    saSub.SupplierTransportRegisterId = str.SupplierTransportRegisterId;

                    context.SaleSuborders.Update(saSub);
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


        [HttpDelete("logicDelete/{id}/{storeId?}")]
        public async Task<IActionResult> logicDelete(int id, Guid storeId)
        {
            var existe = await context.SupplierTransportRegisters.AnyAsync(x => x.SupplierTransportRegisterIdx == id);
            if (!existe) { return NotFound(); }

            var name2 = await context.SupplierTransportRegisters.FirstOrDefaultAsync(x => x.SupplierTransportRegisterIdx == id);
            name2.Active = 0;
            context.Update(name2);

            var usuarioId = obtenerUsuarioId();
            var ipUser = obtenetIP();
            var name = name2.SupplierTransportRegisterId;
            var storeId2 = storeId;
            await servicioBinnacle.deleteLogicBinnacle(usuarioId, ipUser, name.ToString(), storeId2);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}/{storeId?}")]
        public async Task<IActionResult> Delete(int id, Guid storeId)
        {
            try
            {
                var existe = await context.SupplierTransportRegisters.AnyAsync(x => x.SupplierTransportRegisterIdx == id);
                if (!existe) { return NotFound(); }

                var name2 = await context.SupplierTransportRegisters.FirstOrDefaultAsync(x => x.SupplierTransportRegisterIdx == id);
                context.Remove(name2);

                var usuarioId = obtenerUsuarioId();
                var ipUser = obtenetIP();
                var name = name2.SupplierTransportRegisterIdx;
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

        [HttpPost("{storeId}/{idGuid}")]
        //[AllowAnonymous]
        public async Task<int> Save([FromBody] SupplierTransportRegisterDTO suppTrRegDTO, Guid storeId, Guid idGuid)
        {
            int rpta = 0;
            try
            {
                using (var transaccion = await context.Database.BeginTransactionAsync())
                {
                    SupplierTransportRegister oSuppTR = new();

                    oSuppTR.SupplierId = Guid.NewGuid();
                    oSuppTR.AmountPerFee = suppTrRegDTO.AmountPerFee;
                    oSuppTR.AmountPerCapacity = suppTrRegDTO.AmountPerCapacity;
                    oSuppTR.AmountPerUse = suppTrRegDTO.AmountPerUse;
                    oSuppTR.AmountPerVolume = suppTrRegDTO.AmountPerVolume;
                    oSuppTR.AmountPerService = suppTrRegDTO.AmountPerService;
                    oSuppTR.AmountOfDiscount = suppTrRegDTO.AmountOfDiscount;
                    oSuppTR.Date = DateTime.Now;
                    oSuppTR.Updated = DateTime.Now;
                    oSuppTR.Active = 1;
                    oSuppTR.Locked = 0;
                    oSuppTR.Deleted = 0;

                    context.SupplierTransportRegisters.Add(oSuppTR);

                    InventoryInDocument oInvoiceDTODocument = context.InventoryInDocuments.First(x => x.InventoryInId == idGuid);
                    //Guid myGuid = new Guid(oSuppTR.SupplierId);
                    oInvoiceDTODocument.SupplierTransportRegisterId = oSuppTR.SupplierId;

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

