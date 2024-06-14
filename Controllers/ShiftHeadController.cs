using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Net;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using APIControlNet.DTOs;
using APIControlNet.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ShiftHeadController : Controller
    {
        private readonly CnetCoreContext objContext;
        private readonly IMapper objMapper;
        private readonly ServicioBinnacle objServicioBinnacle;
        private readonly UserManager<IdentityUser> objUserManager;

        #region Constructor.
        public ShiftHeadController(CnetCoreContext viContext, IMapper viMapper, ServicioBinnacle viServicioBinnacle)
        {
            this.objContext = viContext;
            this.objMapper = viMapper;
            this.objServicioBinnacle = viServicioBinnacle;
        }
        #endregion

        [HttpGet("ObtenerCorteNuevo")]
        [AllowAnonymous]
        public async Task<ShiftHeadDTO> ObtenerCorteNuevoDatos(Guid viStoreId, int viIslandId)
        {
            ShiftHeadDTO objCorteDatos = new ShiftHeadDTO();
            Guid gShiftHeadId = Guid.Empty,
                 gEmployeeId = Guid.Empty;

            #region Ejemplos.
            //* Un solo parametro
            // join pd in objContext.Products on p.ProductId equals pd.ProductId

            //* Varios parametros
            // join ps in objContext.ProductSats on new { p1 = p.StoreId, p2 = p.ProductId } equals new { p1 = ps.StoreId, p2 = ps.ProductId }

            //* LEFT JOIN
            //join ps in objContext.ProductSats on new { f1 = th.StoreId, f2 = td.ProductId } equals new { f1 = ps.StoreId, f2 = ps.ProductId } into psf
            //                   from ps in psf.DefaultIfEmpty()

            //* GROUP BY 
            // group p by new { p.ProductId, p.ProductCode, p.Name, ps.SatProductKey, ps.TurbosinaConCombustibleNoFosil } into ProdDatos

            // * Foreach
            //foreach (var vProd in vQProductoDatos)
            //{
            //    //objCorteDatos.ShiftDate = vCorteDato.S


            //}
            #endregion

            #region Corte: Encabezado.
            #region Consulta: Corte Encabezado.
            var vQCorteHdrDatos = (from c in objContext.Shifts
                                   join ch in objContext.ShiftHeads on new { p1 = c.StoreId, p2 = c.ShiftHeadId } equals new { p1 = ch.StoreId, p2 = ch.ShiftHeadId }
                                   join m in objContext.Hoses on new { p1 = viStoreId, p2 = c.HoseIdi } equals new { p1 = m.StoreId, p2 = m.HoseIdi }
                                   join pc in objContext.LoadPositions on new { p1 = viStoreId, p2 = m.LoadPositionIdi } equals new { p1 = pc.StoreId, p2 = pc.LoadPositionIdi }
                                   join e in objContext.Employees on c.EmployeeId equals e.EmployeeId into ef
                                   from e in ef.DefaultIfEmpty()
                                   orderby ch.ShiftDate, ch.ShiftDay
                                   where c.StoreId == viStoreId
                                         && pc.IslandIdi == viIslandId
                                   //&& c.ShiftHeadId == viShiftHead
                                   group c by new { ch.ShiftHeadId, ch.ShiftNumber, ch.ShiftDate, ch.ShiftDay, c.EmployeeId, e.EmployeeNumber, e.Name } into CorteDatos
                                   select new
                                   {

                                       ShiftHeadId = CorteDatos.Key.ShiftHeadId,
                                       ShiftNumber = CorteDatos.Key.ShiftNumber,
                                       ShiftDate = CorteDatos.Key.ShiftDate,
                                       ShiftDay = CorteDatos.Key.ShiftDay,
                                       EmplyeeID = CorteDatos.Key.EmployeeId,
                                       EmployeeNumber = CorteDatos.Key.EmployeeNumber,
                                       EmployeeName = CorteDatos.Key.Name

                                   }).FirstOrDefault();
            #endregion

            #region Llenado: Corte Encabezado.
            gShiftHeadId = vQCorteHdrDatos.ShiftHeadId.GetValueOrDefault();
            gEmployeeId = vQCorteHdrDatos.EmplyeeID.GetValueOrDefault();

            objCorteDatos.ShiftNumber = vQCorteHdrDatos.ShiftNumber;
            objCorteDatos.ShiftDate = vQCorteHdrDatos.ShiftDate;
            objCorteDatos.ShiftDay = vQCorteHdrDatos.ShiftDay;
            objCorteDatos.EmployeeIdi = vQCorteHdrDatos.EmployeeNumber;
            objCorteDatos.EmployeeName = vQCorteHdrDatos.EmployeeName;
            #endregion
            #endregion;

            #region Corte: Combustibles.
            #region Consulta: Corte Combustibles.
            var vQCorteFuelDatos = (from c in objContext.Shifts
                                    join m in objContext.Hoses on new { p1 = viStoreId, p2 = c.HoseIdi } equals new { p1 = m.StoreId, p2 = m.HoseIdi }
                                    join p in objContext.Products on m.ProductId equals p.ProductId
                                    orderby c.HoseIdi
                                    where c.StoreId == viStoreId &&
                                          c.ShiftHeadId == gShiftHeadId
                                    select new
                                    {
                                        c.ShiftId,
                                        c.HoseIdi,
                                        p.ProductId,
                                        p.ProductCode,
                                        ProductName = p.Name,
                                        c.Price,
                                        c.StartQuantity,
                                        c.EndQuantity,
                                        JarQuantity = (from sh in objContext.SaleOrders
                                                       join sd in objContext.SaleSuborders on sh.SaleOrderId equals sd.SaleOrderId
                                                       join cd in objContext.Customers on sh.CustomerId equals cd.CustomerId
                                                       where sh.StoreId == viStoreId &&
                                                             sh.ShiftId == c.ShiftId &&
                                                             cd.CustomerNumber == 2004
                                                       select new { sd.Quantity }).Sum(j => j.Quantity),
                                        JarAmount = (from sh in objContext.SaleOrders
                                                     join cd in objContext.Customers on sh.CustomerId equals cd.CustomerId
                                                     where sh.StoreId == viStoreId &&
                                                           sh.ShiftId == c.ShiftId &&
                                                           cd.CustomerNumber == 2004
                                                     select new { sh.Amount }).Sum(j => j.Amount),
                                        ConsignmentQuantity = (from sh in objContext.SaleOrders
                                                               join sd in objContext.SaleSuborders on sh.SaleOrderId equals sd.SaleOrderId
                                                               join cd in objContext.Customers on sh.CustomerId equals cd.CustomerId
                                                               where sh.StoreId == viStoreId &&
                                                                     sh.ShiftId == c.ShiftId &&
                                                                     cd.SatConsignmentSale == "Y"
                                                               select new { sd.Quantity }).Sum(j => j.Quantity),
                                        ConsignmentAmount = (from sh in objContext.SaleOrders
                                                             join cd in objContext.Customers on sh.CustomerId equals cd.CustomerId
                                                             where sh.StoreId == viStoreId &&
                                                                   sh.ShiftId == c.ShiftId &&
                                                                   cd.SatConsignmentSale == "Y"
                                                             select new { sh.Amount }).Sum(j => j.Amount),
                                        c.TotalQuantity,
                                        c.TotalAmount
                                    });
            #endregion

            #region Llenado: Corte Combustibles.
            List<ShiftDTO> lstCorteCombustibles = new List<ShiftDTO>();

            foreach (var vFuel in vQCorteFuelDatos)
            {
                Decimal dCantidad = vFuel.TotalQuantity.GetValueOrDefault() - (vFuel.JarQuantity.GetValueOrDefault() + vFuel.ConsignmentQuantity.GetValueOrDefault()),
                        dImporte = vFuel.TotalAmount.GetValueOrDefault() - (vFuel.JarAmount.GetValueOrDefault() + vFuel.ConsignmentAmount.GetValueOrDefault());

                lstCorteCombustibles.Add(new ShiftDTO
                {
                    StoreId = viStoreId,
                    ShiftHeadId = gShiftHeadId,
                    ShiftId = vFuel.ShiftId,
                    HoseIdi = vFuel.HoseIdi,
                    ProductId = vFuel.ProductId,
                    ProductCode = vFuel.ProductCode,
                    ProductName = vFuel.ProductName,
                    Price = vFuel.Price,
                    StartQuantity = vFuel.StartQuantity,
                    EndQuantity = vFuel.EndQuantity,
                    JarQuantity = vFuel.JarQuantity,
                    JarAmount = vFuel.JarAmount,
                    ConsignmentQuantity = vFuel.ConsignmentQuantity,
                    ConsignmentAmount = vFuel.ConsignmentAmount,
                    TotalQuantity = dCantidad,
                    TotalAmount = dImporte
                });
            }

            objCorteDatos.Fuels = lstCorteCombustibles;
            #endregion
            #endregion

            #region Corte: Productos.
            #region Consulta: Productos.
            var vQCorteProductDatos = (from p in objContext.ProductStores
                                       join pd in objContext.Products on p.ProductId equals pd.ProductId
                                       join pt in objContext.ProductCategories on pd.ProductCategoryId equals pt.ProductCategoryId
                                       orderby pd.Name
                                       where p.StoreId == viStoreId && pd.IsFuel == false
                                       select new
                                       {
                                           p.ProductId,
                                           TypeName = pt.Name,
                                           pd.ProductCode,
                                           ProductName = pd.Name,
                                           p.Price,
                                           ProductTax = p.Tax,
                                           StartQuantity = 0,
                                           DeliveredQuantity = 0,
                                           ReceivedQuantity = 0,
                                           EndQuantity = (from c in objContext.Shifts
                                                          join vh in objContext.SaleOrders on c.ShiftId equals vh.ShiftId
                                                          join vd in objContext.SaleSuborders on vh.SaleOrderId equals vd.SaleOrderId
                                                          where c.StoreId == p.StoreId &&
                                                                c.ShiftHeadId == gShiftHeadId &&
                                                                vd.ProductId == p.ProductId
                                                          select new { vd.Quantity }).Sum(q => q.Quantity)
                                       });
            #endregion

            #region Llenado: Corte Productos.
            List<ShiftDTO> lstCorteProductos = new List<ShiftDTO>();

            foreach (var vProd in vQCorteProductDatos)
            {
                //if (vProd.EndQuantity <= 0)
                //    continue;

                Decimal dPrecioIVA = vProd.Price.GetValueOrDefault() * ((vProd.ProductTax.GetValueOrDefault() / 100) + 1),
                        dCantInicial = vProd.StartQuantity,
                        dCantRecibida = vProd.DeliveredQuantity,
                        dCantidad = 0, dImporte = 0;

                if ((dCantInicial + dCantRecibida) <= 0)
                    dCantidad = vProd.EndQuantity.GetValueOrDefault();
                else
                    dCantidad = (dCantInicial + dCantRecibida) - vProd.EndQuantity.GetValueOrDefault();

                dImporte = dCantidad * dPrecioIVA;

                lstCorteProductos.Add(new ShiftDTO
                {
                    StoreId = viStoreId,
                    ShiftHeadId = gShiftHeadId,
                    ProductId = vProd.ProductId,
                    ProductCode = vProd.ProductCode,
                    ProductName = vProd.ProductName,
                    ProductTypeName = vProd.TypeName,
                    Price = vProd.Price,
                    ProductTax = vProd.ProductTax,
                    StartQuantity = 0,
                    DeliveredQuantity = 0,
                    ReceivedQuantity = 0,
                    EndQuantity = vProd.EndQuantity,
                    TotalQuantity = dCantidad,
                    TotalAmount = dImporte
                });
            }

            objCorteDatos.Products = lstCorteProductos;
            #endregion
            #endregion

            #region Corte: Clientes.
            #region Consulta: Clientes diferentes a Efectivo.
            var vQCorteClientes = (from c in objContext.Shifts
                                   join v in objContext.SaleOrders on new { p1 = viStoreId, p2 = c.ShiftId } equals new { p1 = v.StoreId, p2 = v.ShiftId }
                                   join cd in objContext.Customers on v.CustomerId equals cd.CustomerId
                                   where c.StoreId == viStoreId &&
                                         c.ShiftHeadId == gShiftHeadId &&
                                         cd.CustomerTypeId != 1 &&
                                         cd.SatConsignmentSale != "Y"
                                   select new
                                   {
                                       Status = "A",
                                       ShiftPaymentTypeIdi = 2,
                                       ShiftPaymentTypeDescription = (from tp in objContext.ShiftPaymentTypes
                                                                      where tp.ShiftPaymentTypeId == 2
                                                                      select tp.Description).FirstOrDefault(),
                                       v.SaleOrderNumber,
                                       cd.CustomerNumber,
                                       cd.Name,
                                       v.Amount
                                   });
            #endregion

            #region Llenado: Clientes diferentes a Efectivo.
            List<ShiftPaymentDetailDTO> lstClientes = new List<ShiftPaymentDetailDTO>();
            foreach (var vCliente in vQCorteClientes)
            {
                lstClientes.Add(new ShiftPaymentDetailDTO
                {
                    ShiftPaymentTypeIdi = 2,
                    ShiftPaymentTypeDescription = vCliente.ShiftPaymentTypeDescription,
                    ShiftPaymentNumberReference = vCliente.SaleOrderNumber.GetValueOrDefault(),
                    CustomerNumber = vCliente.CustomerNumber,
                    CustomerName = vCliente.Name,
                    Amount = vCliente.Amount
                });
            }

            objCorteDatos.Clients = lstClientes;
            #endregion
            #endregion

            #region Corte: Depositos.
            #region Consulta: Depositos del Despachador.
            var vQCorteDepositos = (from d in objContext.ShiftDeposits
                                    where d.StoreId == viStoreId &&
                                          d.ShiftHeadId == gShiftHeadId &&
                                          d.IslandIdi == viIslandId &&
                                          d.EmployeeId == gEmployeeId
                                    select new
                                    {
                                        d.ShiftDepositNumber,
                                        d.Amount
                                    });
            #endregion

            #region Llenado: Depositos del Despachador.
            List<ShiftDepositDTO> lstDepositos = new List<ShiftDepositDTO>();

            foreach (var vDeposito in vQCorteDepositos)
            {
                lstDepositos.Add(new ShiftDepositDTO
                {
                    StoreId = viStoreId,
                    ShiftHeadId = gShiftHeadId,
                    IslandIdi = viIslandId,
                    EmployeeId = gEmployeeId,
                    ShiftDepositNumber = vDeposito.ShiftDepositNumber,
                    Amount = vDeposito.Amount
                });
            }

            objCorteDatos.Deposits = lstDepositos;
            #endregion
            #endregion

            #region Corte: Gastos.
            #region Consulta: Corte Gastos.
            var vQCorteGastos = (from g in objContext.Spents
                                 where g.StoreId == viStoreId
                                 select new
                                 {
                                     g.SpentId,
                                     g.Description
                                 });
            #endregion

            #region Llenado: Corte Gastos.
            List<ShiftPaymentDetailDTO> lstGastos = new List<ShiftPaymentDetailDTO>();

            foreach (var vGasto in vQCorteGastos)
                lstGastos.Add(new ShiftPaymentDetailDTO
                {
                    ShiftPaymentTypeIdi = 7,
                    ShiftPaymentNumberReference = vGasto.SpentId,
                    Description = vGasto.Description,
                    Amount = 0
                });

            objCorteDatos.Spents = lstGastos;
            #endregion
            #endregion

            #region Corte: Vales.
            #region Consulta: Corte vales.
            var vQCorteVales = (from c in objContext.Shifts
                                join v in objContext.SaleOrders on c.ShiftId equals v.ShiftId
                                join mp in objContext.SaleOrderPayments on v.SaleOrderId equals mp.SaleOrderId
                                //join fp in objContext.FormaPagoCnetcores on mp.SatFormaPagoId equals fp.FormaPagoCnetcoreId
                                where c.StoreId == viStoreId &&
                                      c.ShiftHeadId == gShiftHeadId
                                //&& fp.is_voucher = 1
                                //  group new { eh, ed, p } by p.JsonClaveUnidadMedidaId into g
                                group new { v } by mp.SatFormaPagoId into ValeDato
                                select new
                                {
                                    ValeDato.Key,
                                    // fp.is_own_card,
                                    Cantidad = ValeDato.Count(),
                                    Importe = (ValeDato.Sum(e => e.v.Amount) ?? 0)
                                });
            #endregion

            #region Llenado: Corte Vales.
            List<ShiftPaymentDetailDTO> lstVales = new List<ShiftPaymentDetailDTO>();
            foreach (var vVale in vQCorteVales)
            {
                lstVales.Add(new ShiftPaymentDetailDTO
                {
                    ShiftPaymentTypeIdi = 5,//ShiftPaymentDetailDTO.ePaymentType.Vales,
                    ShiftPaymentTypeDescription = (from tp in objContext.ShiftPaymentTypes
                                                   where tp.ShiftPaymentTypeId == 5
                                                   select tp.Description).FirstOrDefault(),
                    Amount = vVale.Importe
                });
            }

            objCorteDatos.Vouchers = lstVales;
            #endregion
            #endregion

            #region Corte: Tarjetas.
            #region Consulta: Corte Tarjeta.
            var vQCorteTarjetas = (from c in objContext.Shifts
                                   join v in objContext.SaleOrders on c.ShiftId equals v.ShiftId
                                   join mp in objContext.SaleOrderPayments on v.SaleOrderId equals mp.SaleOrderId
                                   //join fp in objContext.FormaPagoCnetcores on mp.SatFormaPagoId equals fp.FormaPagoCnetcoreId
                                   where c.StoreId == viStoreId &&
                                         c.ShiftHeadId == gShiftHeadId
                                        //&& fp.is_voucher = 0
                                        && (mp.SatFormaPagoId == "04" || mp.SatFormaPagoId == "28")
                                   group new { v } by mp.SatFormaPagoId into TarjetaDato
                                   select new
                                   {
                                       TarjetaDato.Key,
                                       Importe = (TarjetaDato.Sum(e => e.v.Amount) ?? 0)
                                   });
            #endregion

            #region Llenado: Corte Tarjetas.
            List<ShiftPaymentDetailDTO> lstTarjetas = new List<ShiftPaymentDetailDTO>();
            foreach (var vTarjeta in vQCorteTarjetas)
            {
                lstTarjetas.Add(new ShiftPaymentDetailDTO
                {
                    ShiftPaymentTypeIdi = 6,//ShiftPaymentDetailDTO.ePaymentType.Vales,
                    ShiftPaymentTypeDescription = (from tp in objContext.ShiftPaymentTypes
                                                   where tp.ShiftPaymentTypeId == 6
                                                   select tp.Description).FirstOrDefault(),
                    Description = (from f in objContext.SatFormaPagos
                                   where f.SatFormaPagoId == vTarjeta.Key
                                   select f.Descripcion).FirstOrDefault(),
                    Amount = vTarjeta.Importe
                });
            }

            objCorteDatos.Cards = lstTarjetas;
            #endregion
            #endregion

            return objMapper.Map<ShiftHeadDTO>(objCorteDatos);
        }


        [HttpGet("EstacionCortes")]
        //[AllowAnonymous]
        public async Task<IEnumerable<ShiftHeadDTO>> ListaEstacionCortes(Guid viStoreId, DateTime viDStart, DateTime viDEnd, int viShiftNumber, [FromQuery] PaginacionDTO viPaginacionDTO)
        {
            var vQCortes = objContext.ShiftHeads.Where(c => c.StoreId.Equals(viStoreId) &&
                                                            c.ShiftDate >= viDStart &&
                                                            c.ShiftDate <= viDEnd).AsQueryable().OrderByDescending(c => c.ShiftDate);
            if (viShiftNumber > 0)
                vQCortes = objContext.ShiftHeads.Where(c => c.StoreId.Equals(viStoreId) && c.ShiftNumber.Equals(viShiftNumber)).OrderByDescending(c => c.ShiftDate);

            await HttpContext.InsertarParametrosPaginacionEnRespuesta(vQCortes, viPaginacionDTO.CantidadAMostrar);
            var vListaCortes = await vQCortes.Paginar(viPaginacionDTO).AsNoTracking().ToListAsync();

            return objMapper.Map<List<ShiftHeadDTO>>(vListaCortes);
        }


        [AllowAnonymous]
        [HttpGet("Consultas Test")]
        public ActionResult TestDev()
        {
            Guid gNEstacion = Guid.Parse("71930422-3B22-4ABA-8D49-5BC623067637"),
                 gShiftHeadID = Guid.Parse("9882248B-1B18-4EF3-9426-6C36931F6B6E");

            var vQuery = (from c in objContext.Shifts
                          join m in objContext.Hoses on new { p1 = gNEstacion, p2 = c.HoseIdi } equals new { p1 = m.StoreId, p2 = m.HoseIdi }
                          where c.StoreId == gNEstacion &&
                                c.ShiftHeadId == gShiftHeadID
                          select new { c.ShiftIdNumber, c.HoseIdi, m.ProductId });

            if (vQuery == null)
                return BadRequest("No se encontraron productos que reportar");

            //foreach (var vProd in vQProductos)
            //{
            //}

            return Ok(vQuery);
        }
    }
}