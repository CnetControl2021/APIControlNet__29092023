﻿using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VolumetricController : CustomBaseController
    {
        private readonly CnetCoreContext objContext;
        private readonly IMapper objMapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public VolumetricController(CnetCoreContext viContext, IMapper viMapper, ServicioBinnacle servicioBinnacle)
        {
            this.objContext = viContext;
            this.objMapper = viMapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        //[AllowAnonymous]
        [HttpGet(Name = "CVolJSon")]
        public async Task<IActionResult> GenerarCVolJSon(Guid? viNCompania, Guid? viNEstacion, CVolJSonDTO.eTipoReporte viTipoReporte, Boolean viReporteEstacion, DateTime viFecha, Boolean viGenerarComplemento, Boolean viGenerarCompTransporte, Boolean viGenCompRecepcion, Boolean viGenComEntrega)
        {
            CVolJSonDTO objCVolDatos = new CVolJSonDTO();
            Dictionary<Guid, DailySummary> dictDailySummary = null;
            Dictionary<Guid, MonthlySummary> dictMonthlySummary = null;

            #region Inicializacion de Valores.
            String sNomClaveInstalacion = "", sClaveTipoReporte = "";
            String sTipoComplementoEstacion = "";
            String sRutaCarpetaRaiz = String.Empty;
            int iNStore = 0;
            int iNClaveInstalacion = 1, iNTransaccionNew = 1;
            int iDiferenciaHora = 0, iTotalTanques = 0, iTotalDispensarios = 0, iTotalDuctos = 0;

            DateTime dtFechaRegistro = DateTime.Now;
            DateTime vxFecha = viFecha, dtPerDateIni = new DateTime(), dtPerDateEnd = new DateTime();

            CVolJSonDTO.eTipoDistribucion objTipoDistribucion = CVolJSonDTO.eTipoDistribucion.Autotanques;
            CVolJSonDTO.eTipoComplemento objTipoComplemento = CVolJSonDTO.eTipoComplemento.Comercializadora;
            Boolean bCompRecep = viGenCompRecepcion, bCompEntregas = viGenComEntrega;
            #endregion

            switch (viTipoReporte)
            {
                #region Reporte: Diario.
                case CVolJSonDTO.eTipoReporte.Dia:
                    objCVolDatos.Version = "1.0";
                    dictDailySummary = new Dictionary<Guid, DailySummary>();

                    // Asignamos los Valores de las Fechas de Consult
                    dtPerDateIni = vxFecha.AddMinutes(0);
                    dtPerDateEnd = vxFecha.AddMinutes(1439);
                    dtPerDateEnd = dtPerDateEnd.AddSeconds(59);

                    objCVolDatos.FechaYHoraCorte = dtPerDateEnd.ToString("yyyy-MM-dd") + "T" +
                                                   dtPerDateEnd.ToString("HH:mm:ss") + "-" +
                                                   iDiferenciaHora.ToString("00") + ":00";
                    break;
                #endregion

                #region Reporte: Mensual.
                case CVolJSonDTO.eTipoReporte.Mes:
                    objCVolDatos.Version = "4.1";
                    dictMonthlySummary = new Dictionary<Guid, MonthlySummary>();

                    // Asignamos los Valores de las Fechas de Consulta.
                    dtPerDateIni = vxFecha.AddMinutes(0);
                    dtPerDateEnd = vxFecha.AddMonths(1);
                    dtPerDateEnd = dtPerDateEnd.AddSeconds(-1);

                    objCVolDatos.FechaYHoraReporteMes = dtPerDateEnd.ToString("yyyy-MM-dd") + "T" +
                                                        dtPerDateEnd.ToString("HH:mm:ss") + "-" +
                                                        iDiferenciaHora.ToString("00") + ":00";
                    break;
                    #endregion
            }

            #region Ruta Carpeta Archivo.
            var vQSPath = (from s in objContext.Settings
                           where s.StoreId == viNEstacion &&
                                 s.Field == "setting_volumetric_path"
                           select new { Path = (s.Value ?? "") }
                           );

            if (vQSPath != null)
            {
                foreach (var vPathDato in vQSPath)
                    sRutaCarpetaRaiz = vPathDato.Path;

                if (String.IsNullOrEmpty(sRutaCarpetaRaiz))
                    return BadRequest("No se encontro la ruta de la carpeta de alojamiento en la configuración.");
            }
            else
                return BadRequest("No se encontro la ruta de la carpeta de alojamiento en la configuración.");
            #endregion

            #region Datos: Compañia.
            #region Consulta: Datos de la Compañia.
            var vQCompania = (from c in objContext.Companies
                              where c.CompanyId == viNCompania
                              select new
                              {
                                  Nombre = (c.Name ?? ""),
                                  TradeNombre = (c.TradeName ?? ""),
                                  RFC = (c.Rfc ?? "")
                              });
            #endregion

            #region Lectura: Datos Compañia.
            if (vQCompania != null)
            {
                foreach (var vCompaniaDatos in vQCompania)
                {
                    #region Compañia: Asignamos Valores.
                    String sNombreCompania = vCompaniaDatos.Nombre,
                           sTradeCompania = vCompaniaDatos.TradeNombre,
                           sRfcCompania = vCompaniaDatos.RFC;
                    #endregion

                    #region Compañia: Validamos Valores.
                    if (String.IsNullOrEmpty(sRfcCompania))
                        return BadRequest("La Compania '" + sNombreCompania + "' no contiene su RFC. (company)");
                    #endregion

                    objCVolDatos.RfcContribuyente = sRfcCompania.Trim();
                }
            }
            else
                return BadRequest("No se encontro la Compania '" + viNCompania.ToString() + "' (Company)");
            #endregion
            #endregion

            // FISICO: 13 Caracteres | MORAL : 12 Caracteres
            objCVolDatos.RfcProveedor = "CCC93102878A";
            objCVolDatos.RfcProveedores = null;

            #region Caracteres (Permisos).
            String sCaracterValor = CVolJSonDTO.CARACTER_PERMISIONARIO,
                   sDescripInstalacion = String.Empty;
            Object oModalidadPermiso = null, oNumPermiso = null,
                   //oTipoCaracter = null, oCaracter = null,
                   oNumContratoOAsignacion = null, oInstalacionAlmacenGasNatural = null;
            int iNumeroPozos = 0;

            #region Caracter.
            switch (sCaracterValor)
            {
                #region Caracter: PERMISIONARIO.
                case CVolJSonDTO.CARACTER_PERMISIONARIO:
                    var vQStoreDatos = (from s in objContext.Stores
                                        join ss in objContext.StoreSats on s.StoreId equals ss.StoreId
                                        where s.CompanyId == viNCompania &&
                                              s.StoreId == viNEstacion
                                        select new
                                        {
                                            StoreNumber = s.StoreNumber,
                                            SatRfcSupplier = (ss.RfcSystemSupplier ?? ""),
                                            RfcLegalRepresentative = (ss.RfcLegalRepresentative ?? ""),
                                            SatInstallationKey = (ss.JsonClaveInstalacionId ?? ""),
                                            SatDescriptionInstallation = (ss.SystemDescriptionInstallation ?? ""),
                                            SatReportType = (ss.JsonTipoReporteId ?? ""),
                                            SatPermission = (ss.SatPermission ?? ""),
                                            CrePermission = (ss.CrePermission ?? ""),
                                            TimeDifference = s.TimeDifference.GetValueOrDefault(),
                                            DistributionType = (ss.JsonTipoDistribucionId ?? ""),
                                            ComplementType = (ss.JsonTipoComplementoId ?? "")
                                        });

                    if (vQStoreDatos != null)
                        foreach (var vStoreDatos in vQStoreDatos)
                        {
                            #region Asignación: Estacion Datos.
                            iNStore = vStoreDatos.StoreNumber;
                            String sRfcProveedor = vStoreDatos.SatRfcSupplier; //(F0403: TERfcProveedorSAT)
                            sNomClaveInstalacion = vStoreDatos.SatInstallationKey; //(F0403: TEClaveInstalacionSAT)
                            sDescripInstalacion = vStoreDatos.SatDescriptionInstallation; //(F0403: TEDescripInstalacionSAT)
                            sClaveTipoReporte = vStoreDatos.SatReportType; //(F0403: TEClaveTipoReporteSAT)
                            oNumPermiso = vStoreDatos.SatPermission; //(F0403: TEnumeroPermisoCRE)
                            oModalidadPermiso = vStoreDatos.CrePermission; //(F0403: TEClavePermisoCRE)
                            iNClaveInstalacion = vStoreDatos.StoreNumber; //(F0403: TECODSTA)
                            iDiferenciaHora = vStoreDatos.TimeDifference; //(F0403: TeDifHor)
                            String sRfcRepresentanteLegal = vStoreDatos.RfcLegalRepresentative, //(F0403: TERfcRepresentanteLegalSAT)
                                   sTipoDistribucion = vStoreDatos.DistributionType; //(F0403: TETipoDistribucion)
                            sTipoComplementoEstacion = vStoreDatos.ComplementType; //(F0403: TETipoComplementoCVol)
                            #endregion

                            #region Validamos valores requeridos.
                            if (String.IsNullOrEmpty(sRfcProveedor))
                                return BadRequest(iNStore + "|SatRfcSupplier");
                            else
                                objCVolDatos.RfcProveedor = sRfcProveedor;

                            if (String.IsNullOrEmpty(sNomClaveInstalacion))
                                return BadRequest(iNStore + "|SatInstallationKey");

                            if (String.IsNullOrEmpty(oModalidadPermiso.ToString()))
                                return BadRequest(iNStore + "|CrePermission");

                            if (String.IsNullOrEmpty(oNumPermiso.ToString()))
                                return BadRequest(iNStore + "|SatPermission");

                            if (String.IsNullOrEmpty(sClaveTipoReporte))
                                return BadRequest(iNStore + "|SatReportType");

                            if (String.IsNullOrEmpty(sDescripInstalacion))
                                return BadRequest(iNStore + "|SatDescriptionInstallation");

                            if (String.IsNullOrEmpty(sTipoComplementoEstacion))
                                return BadRequest(iNStore + "|ComplementType");
                            #endregion

                            String sTipoComplementoValido = String.Empty;
                            switch (sTipoComplementoEstacion.ToUpper())
                            {
                                case CVolJSonDTO.TIPO_COMPLEMENTO_DISTRIBUIDOR: objTipoComplemento = CVolJSonDTO.eTipoComplemento.Distribuidor; break;
                                case CVolJSonDTO.TIPO_COMPLEMENTO_TRANSPORTE: objTipoComplemento = CVolJSonDTO.eTipoComplemento.Transportista; break;
                                case CVolJSonDTO.TIPO_COMPLEMENTO_COMERCIALIZADOR: objTipoComplemento = CVolJSonDTO.eTipoComplemento.Comercializadora; break;
                                default:
                                    sTipoComplementoValido = "El Tipo de Complemento '" + sTipoComplementoEstacion + "' NO es valido.";
                                    break;
                            }

                            // ==> Tipo Distribución.
                            switch (sTipoDistribucion.ToUpper())
                            {
                                case "TRANSPORTE": objTipoDistribucion = CVolJSonDTO.eTipoDistribucion.Autotanques; break;
                                case "DUCTO": objTipoDistribucion = CVolJSonDTO.eTipoDistribucion.Ductos; break;
                            }

                            if (!String.IsNullOrEmpty(sTipoComplementoValido) && !viReporteEstacion)
                                return BadRequest(sTipoComplementoValido);

                            objCVolDatos.RfcRepresentanteLegal = sRfcRepresentanteLegal;
                        }
                    else
                        return BadRequest($"No se encontraron los datos de la Estación '{viNEstacion}'");
                    break;
                #endregion

                #region Caracter: USUARIO.
                case CVolJSonDTO.CARACTER_USUARIO: oInstalacionAlmacenGasNatural = ""; break;
                #endregion

                #region Caracter: DEFAULT.
                default: oNumContratoOAsignacion = ""; iNumeroPozos = 0; break;
                    #endregion
            }
            #endregion

            objCVolDatos.Caracter = sCaracterValor;
            objCVolDatos.ModalidadPermiso = oModalidadPermiso;
            objCVolDatos.NumPermiso = oNumPermiso;
            objCVolDatos.NumContratoOAsignacion = oNumContratoOAsignacion;
            objCVolDatos.InstalacionAlmacenGasNatural = oInstalacionAlmacenGasNatural;

            // PERSONA MORAL
            if (objCVolDatos.RfcContribuyente.Length.Equals(12))
            {
                if (objCVolDatos.RfcRepresentanteLegal == null || String.IsNullOrEmpty(objCVolDatos.RfcRepresentanteLegal.ToString()))
                    return BadRequest(iNStore + "|RfcRepresentanteLegal|NoConfig");
                else if (objCVolDatos.RfcRepresentanteLegal.ToString().Length < 13)
                    return BadRequest("RFC Representante Legal incorrecto '" + objCVolDatos.RfcRepresentanteLegal + "'");
            }
            else
                objCVolDatos.RfcRepresentanteLegal = null;
            #endregion

            #region Clave Instalación.
            objCVolDatos.ClaveInstalacion = sNomClaveInstalacion + "-" + iNClaveInstalacion.ToString().PadLeft(4, '0');
            if (objCVolDatos.ClaveInstalacion.ToString().Length < 8 || objCVolDatos.ClaveInstalacion.ToString().Length > 30)
                return BadRequest("La Clave de Instalación debe de ser entre 8 y 30 caracteres.");
            #endregion

            #region Descripción de Instalación.
            objCVolDatos.DescripcionInstalacion = sDescripInstalacion;
            if (objCVolDatos.DescripcionInstalacion.ToString().Length <= 4 || objCVolDatos.DescripcionInstalacion.ToString().Length > 250)
                return BadRequest("La Descripción de Instalación debe de ser entre 0 y 250 caracteres.");
            #endregion

            objCVolDatos.Geolocalizacion = null;
            objCVolDatos.NumeroPozos = iNumeroPozos;
            objCVolDatos.NumeroDuctosEntradaSalida = 0;
            objCVolDatos.NumeroDuctosTransporteDistribucion = 0;

            #region Datos: Productos.
            #region Productos que llevan clave de Producto.
            List<String> lstProductosConClave = new List<String>();
            lstProductosConClave.Add("PR03");
            lstProductosConClave.Add("PR07");
            lstProductosConClave.Add("PR08");
            lstProductosConClave.Add("PR09");
            lstProductosConClave.Add("PR11");
            lstProductosConClave.Add("PR13");
            lstProductosConClave.Add("PR15");
            lstProductosConClave.Add("PR16");
            lstProductosConClave.Add("PR17");
            lstProductosConClave.Add("PR18");
            lstProductosConClave.Add("PR19");
            #endregion

            Dictionary<String, String> dictProductosExis = new Dictionary<string, string>();
            dictProductosExis.Clear();

            objCVolDatos.Producto = new List<CVolJSonDTO.stProductoDato>();
            List<int> lstDispensariosExist = new List<int>();

            #region Consulta: Producto Datos.
            var vQProductos = (from th in objContext.SaleOrders
                               join td in objContext.SaleSuborders on th.SaleOrderId equals td.SaleOrderId
                               join p in objContext.Products on td.ProductId equals p.ProductId
                               join ps in objContext.ProductSats on new { f1 = th.StoreId, f2 = td.ProductId } equals new { f1 = ps.StoreId, f2 = ps.ProductId }
                               //join ps in objContext.ProductSats on new { f1 = th.StoreId, f2 = td.ProductId } equals new { f1 = ps.StoreId, f2 = ps.ProductId } into rf
                               //from ps in rf.DefaultIfEmpty()
                               where th.StoreId == viNEstacion &&
                                     th.StartDate >= dtPerDateIni && th.StartDate <= dtPerDateEnd
                                     && p.IsFuel == true
                               group p by new { p.ProductId, p.ProductCode, p.Name, ps.SatProductKey, ps.SatProductSubkey, ps.ComposOctanajeGasolina, ps.SatWithFossil, ps.SatPercentageWithFossil } into ProdDatos
                               select new
                               {
                                   ProductoID = ProdDatos.Key.ProductId,
                                   ProductCode = ProdDatos.Key.ProductCode,
                                   ProductName = ProdDatos.Key.Name,
                                   ClaveProducto = (ProdDatos.Key.SatProductKey ?? ""),
                                   ClaveSubProducto = (ProdDatos.Key.SatProductSubkey ?? ""),
                                   ComposOctanajeGasolina = (ProdDatos.Key.ComposOctanajeGasolina ?? 0),
                                   ConFosil = (ProdDatos.Key.SatWithFossil ?? 0),
                                   PorcentajeConFosil = (ProdDatos.Key.SatPercentageWithFossil ?? 0)
                               });

            if (vQProductos == null)
                return BadRequest("No se encontraron productos que reportar");
            #endregion

            foreach (var vProd in vQProductos)
            {
                #region Datos: Producto.
                CVolJSonDTO.stProductoDato objProductoDato = new CVolJSonDTO.stProductoDato();

                #region Producto: Asignamos Valores.
                String sNProducto = vProd.ProductCode,
                       sClaveProducto = vProd.ClaveProducto.ToString();
                Object oSubClaveProducto = null;
                int iConFosil = vProd.ConFosil;
                int iPorcentajeFosil = vProd.PorcentajeConFosil;
                #endregion

                #region Producto: Validamos datos.
                // Validamos que la Clave contenga SubClaveProducto.
                if (lstProductosConClave.Exists(p => p.Equals(sClaveProducto)))
                    oSubClaveProducto = vProd.ClaveSubProducto.ToString();

                if (String.IsNullOrEmpty(sClaveProducto))
                    return BadRequest("El producto '" + sNProducto + "' no contiene la Clave (Prod_SAT)");

                if (oSubClaveProducto == null)
                    return BadRequest("El producto '" + sNProducto + "' no contiene la SubClave (Prod_SAT)");
                #endregion

                // DATOS DEL PRODUCTO
                objProductoDato.ClaveProducto = sClaveProducto;
                objProductoDato.ClaveSubProducto = oSubClaveProducto;

                // DATOS DE ACUERDO AL TIPO DE COMBUSTIBLE.
                switch (sClaveProducto)
                {
                    #region ClaveProducto: PR07 (Gasolina).
                    case "PR07":
                        int iOctanajePR07 = vProd.ComposOctanajeGasolina;
                        objProductoDato.ComposOctanajeGasolina = iOctanajePR07;

                        if (iConFosil.Equals(1))
                        {
                            objProductoDato.GasolinaConCombustibleNoFosil = "Sí";
                            objProductoDato.ComposDeCombustibleNoFosilEnGasolina = iPorcentajeFosil;
                        }
                        else
                            objProductoDato.GasolinaConCombustibleNoFosil = "No";
                        break;
                    #endregion

                    #region ClaveProducto: PR03 (Diesel).
                    case "PR03":
                        if (iConFosil.Equals(1))
                        {
                            objProductoDato.DieselConCombustibleNoFosil = "Sí";
                            objProductoDato.ComposDeCombustibleNoFosilEnDiesel = iPorcentajeFosil;
                        }
                        else
                            objProductoDato.DieselConCombustibleNoFosil = "No";
                        break;
                    #endregion

                    #region ClaveProducto: PR11 (Turbosina).
                    case "PR11":
                        if (iConFosil.Equals(1))
                        {
                            objProductoDato.TurbosinaConCombustibleNoFosil = "Sí";
                            objProductoDato.ComposDeCombustibleNoFosilEnTurbosina = iPorcentajeFosil;
                        }
                        else
                            objProductoDato.TurbosinaConCombustibleNoFosil = "No";
                        break;
                        #endregion
                }

                if (objTipoDistribucion == CVolJSonDTO.eTipoDistribucion.Ductos)
                {
                    List<CVolJSonDTO.stDuctoDatos> lstDuctosNew = new List<CVolJSonDTO.stDuctoDatos>();
                    objProductoDato.Ducto = lstDuctosNew;
                }

                //objCVolDatos.Producto.Add(objProductoDato);
                //dictProductosExis.Add(sNProducto, oSubClaveProducto.ToString());
                #endregion

                #region Tipo de Reporte.
                switch (viTipoReporte)
                {
                    #region Tipo: Reporte Día.
                    case CVolJSonDTO.eTipoReporte.Dia:
                        // Agregamos el registro para generar el resumen en la tabla "Daily_Summary"
                        if (!dictDailySummary.ContainsKey(vProd.ProductoID))
                        {
                            dictDailySummary.Add(vProd.ProductoID, new DailySummary
                            {
                                StoreId = Guid.Parse(viNEstacion.ToString()),
                                Date = dtPerDateIni,
                                ProductId = vProd.ProductoID
                            });
                        }

                        if (viReporteEstacion)
                        {
                            #region Tanque Datos.
                            List<CVolJSonDTO.stTanqueDatos> lstTanques = new List<CVolJSonDTO.stTanqueDatos>();
                            CVolJSonDTO.stTanqueDatos objTanqueDatos;

                            DateTime dtPerDateEndExi = dtPerDateEnd.AddMinutes(7);

                            #region Consulta: Datos del Tanque.
                            var vQTanques = (from t in objContext.Tanks
                                             where t.StoreId == viNEstacion && t.ProductId == vProd.ProductoID
                                             select new
                                             {
                                                 NumeroTanque = t.TankIdi,
                                                 Nombre = (t.Name ?? ""),
                                                 FechaCalibracion = t.SatDateCalibration,
                                                 CapTotal = t.CapacityTotal,
                                                 CapOperativa = t.CapacityOperational,
                                                 CapUtil = t.CapacityUseful,
                                                 Fondaje = t.Fondage,
                                                 CapMinOperativa = t.CapacityMinimumOperating,
                                                 Estatus = "O",
                                                 Tipo = (t.SatTankType ?? ""),
                                                 TipoMedicion = (t.SatTypeMeasurement ?? ""),
                                                 TipoMedAlmacenamiento = (t.SatTypeMediumStorage ?? ""),
                                                 DescripMedicion = (t.SatDescriptionMeasurement ?? ""),
                                                 PorcentIncertMed = (t.SatPercentageUncertaintyMeasurement ?? 0),
                                                 VolumenExistenciasAnterior = (from ia in objContext.Inventories
                                                                               where ia.StoreId == viNEstacion &&
                                                                                     ia.TankIdi == t.TankIdi &&
                                                                                     ia.Date >= dtPerDateIni && ia.Date <= dtPerDateEnd.AddMinutes(7)
                                                                               orderby ia.Date
                                                                               select (from ian in objContext.Inventories
                                                                                       where ian.StoreId == t.StoreId &&
                                                                                             ian.TankIdi == t.TankIdi &&
                                                                                             ian.Date < ia.Date
                                                                                       orderby ian.Date descending
                                                                                       select ian.Volume ?? 0).FirstOrDefault()).FirstOrDefault(),
                                                 VolumenFinal = (from vf in objContext.Inventories
                                                                 orderby vf.Date descending
                                                                 where vf.Date >= dtPerDateIni && vf.Date <= dtPerDateEnd.AddMinutes(7) &&
                                                                       vf.StoreId == t.StoreId &&
                                                                       vf.TankIdi == t.TankIdi &&
                                                                       vf.ProductId == vProd.ProductoID
                                                                 select vf.Volume ?? 0).FirstOrDefault(),
                                                 //VolumenAcumOpsRecepcion = (from vr in objContext.InventoryIns
                                                 //                           where vr.StoreId == t.StoreId &&
                                                 //                                 vr.StartDate >= dtPerDateIni && vr.StartDate <= dtPerDateEnd &&
                                                 //                                 vr.TankIdi == t.TankIdi &&
                                                 //                                 vr.ProductId == vProd.ProductoID
                                                 //                           select (from id in objContext.InventoryInDocuments
                                                 //                                   where id.StoreId == t.StoreId &&
                                                 //                                         id.InventoryInId == vr.InventoryInId
                                                 //                                   select new { Volume = (id.Volume ?? 0) }).Sum(a => a.Volume)).FirstOrDefault()
                                                 VolumenAcumOpsRecepcion = (from vr in objContext.InventoryIns
                                                                            join id in objContext.InventoryInDocuments on new { f1 = vr.StoreId, f2 = vr.InventoryInId } equals new { f1 = id.StoreId, f2 = id.InventoryInId }
                                                                            where vr.StoreId == t.StoreId &&
                                                                                  vr.StartDate >= dtPerDateIni && vr.StartDate <= dtPerDateEnd &&
                                                                                  vr.TankIdi == t.TankIdi &&
                                                                                  vr.ProductId == vProd.ProductoID
                                                                            select new { Volume = (id.Volume ?? 0) }).Sum(a => a.Volume)
                                             });
                            #endregion

                            #region Lectura: Datos del Tanque.
                            if (vQTanques != null)
                                foreach (var vTanqDatos in vQTanques)
                                {
                                    #region Tanque: Asignamos Valores.
                                    int? iNTanque = vTanqDatos.NumeroTanque,
                                         iCapTotal = vTanqDatos.CapTotal,
                                         iCapOperativa = vTanqDatos.CapOperativa,
                                         iCapUtil = vTanqDatos.CapUtil,
                                         iCapFondaje = vTanqDatos.Fondaje,
                                         iVolMinOperacion = vTanqDatos.CapMinOperativa,
                                         iTanqueIncertidumbre = vTanqDatos.PorcentIncertMed;
                                    String sDescripcionSAT = vTanqDatos.Nombre;
                                    DateTime dtTanqueFCalibracion = Convert.ToDateTime(vTanqDatos.FechaCalibracion);
                                    String sTanqueUniMed = "UM03",
                                           sTanqueClaveSAT = vTanqDatos.Tipo,
                                           sTanqueStatus = vTanqDatos.Estatus,
                                           sTanqueTipoMedicion = vTanqDatos.TipoMedicion,
                                           sTanqueDescMedicion = vTanqDatos.DescripMedicion,
                                           sTanqueTipoMedAlmacenamiento = vTanqDatos.TipoMedAlmacenamiento;
                                    Decimal dVolumenExistenciasAnterior = vTanqDatos.VolumenExistenciasAnterior,
                                            dVolumenFinal = vTanqDatos.VolumenFinal,
                                            dVolRecepciones = Convert.ToDecimal(vTanqDatos.VolumenAcumOpsRecepcion),
                                            dVolEntrega = dVolumenExistenciasAnterior + (dVolRecepciones - dVolumenFinal),
                                            dVolExistencia = dVolumenExistenciasAnterior + (dVolRecepciones - dVolEntrega);
                                    #endregion

                                    #region Tanque: Validamos Valores.
                                    if (String.IsNullOrEmpty(sTanqueClaveSAT))
                                        return BadRequest("El tanque el Tipo de Tanque (Tank)");

                                    if (String.IsNullOrEmpty(sDescripcionSAT))
                                        return BadRequest("El tanque no contiene la Descripción (Tank)");

                                    if (String.IsNullOrEmpty(sTanqueStatus))
                                        return BadRequest("El tanque no contiene un Estatus (Tank)");

                                    if (String.IsNullOrEmpty(sTanqueTipoMedicion))
                                        return BadRequest("El tanque no contiene un Tipo de Medición (Tank)");

                                    if (String.IsNullOrEmpty(sTanqueTipoMedAlmacenamiento))
                                        return BadRequest("El tanque no contiene un Tipo de Medio de Almacenamiento (Tank)");
                                    else if (sTanqueTipoMedAlmacenamiento.Equals("0"))
                                        return BadRequest("El tanque contiene un Tipo de Medio de Almacenamiento incorrecto '" + sTanqueTipoMedAlmacenamiento + "' (Tank)");

                                    // sTanqueDescMedicion
                                    if (String.IsNullOrEmpty(sTanqueDescMedicion))
                                        return BadRequest("El tanque no contiene la Descripción en la Medición (Tank)");
                                    else if (sTanqueDescMedicion.Length < 2)
                                        return BadRequest("La Descripción de la Medición debe ser minimo de 2 caracteres (Tank)");

                                    // PONER EN 0 SI EL NUMERO ES MENOR A 0. (OCTAVIO)
                                    if (dVolEntrega < 0)
                                        dVolEntrega = 0;
                                    #endregion

                                    #region Tanque: Llenado de Estructura.
                                    objTanqueDatos = new CVolJSonDTO.stTanqueDatos();
                                    objTanqueDatos.ClaveIdentificacionTanque = sTanqueClaveSAT + "-" + sNomClaveInstalacion + "-" + iNTanque.ToString().PadLeft(4, '0');
                                    objTanqueDatos.LocalizacionYODescripcionTanque = sDescripcionSAT;
                                    objTanqueDatos.VigenciaCalibracionTanque = dtTanqueFCalibracion.ToString("yyyy-MM-dd");

                                    #region Capacidad Total Tanque.
                                    CVolJSonDTO.stCapacidadDato objCapacidadTotalDato = new CVolJSonDTO.stCapacidadDato();
                                    objCapacidadTotalDato.ValorNumerico = iCapTotal;
                                    objCapacidadTotalDato.UnidadDeMedida = sTanqueUniMed;
                                    objTanqueDatos.CapacidadTotalTanque = objCapacidadTotalDato;
                                    #endregion

                                    #region Capacidad Operativa.
                                    CVolJSonDTO.stCapacidadDato objCapOperativaDato = new CVolJSonDTO.stCapacidadDato();
                                    objCapOperativaDato.ValorNumerico = iCapOperativa;
                                    objCapOperativaDato.UnidadDeMedida = sTanqueUniMed;
                                    objTanqueDatos.CapacidadOperativaTanque = objCapOperativaDato;
                                    #endregion

                                    #region Capacidad Util.
                                    CVolJSonDTO.stCapacidadDato objCapUtilDato = new CVolJSonDTO.stCapacidadDato();
                                    objCapUtilDato.ValorNumerico = iCapUtil;
                                    objCapUtilDato.UnidadDeMedida = sTanqueUniMed;
                                    objTanqueDatos.CapacidadUtilTanque = objCapUtilDato;
                                    #endregion

                                    #region Capacidad Fondaje.
                                    CVolJSonDTO.stCapacidadDato objCapFondaje = new CVolJSonDTO.stCapacidadDato();
                                    objCapFondaje.ValorNumerico = iCapFondaje;
                                    objCapFondaje.UnidadDeMedida = sTanqueUniMed;
                                    objTanqueDatos.CapacidadFondajeTanque = objCapFondaje;
                                    #endregion

                                    #region Volumen Minimo Operación.
                                    CVolJSonDTO.stCapacidadDato objVolMinOperDato = new CVolJSonDTO.stCapacidadDato();
                                    objVolMinOperDato.ValorNumerico = iVolMinOperacion;
                                    objVolMinOperDato.UnidadDeMedida = sTanqueUniMed;
                                    objTanqueDatos.VolumenMinimoOperacion = objVolMinOperDato;
                                    #endregion

                                    #region Estado Tanque.
                                    objTanqueDatos.EstadoTanque = "F";
                                    if (sTanqueStatus.ToUpper().Equals("O"))
                                        objTanqueDatos.EstadoTanque = "O";
                                    #endregion

                                    #region Medición de Tanques.
                                    Decimal dTanqIncertMed = 0;
                                    if (iTanqueIncertidumbre > 0)
                                        dTanqIncertMed = Convert.ToDecimal(iTanqueIncertidumbre / 100);

                                    String sSistMedicion = "";
                                    switch (sTanqueTipoMedicion)
                                    {
                                        case "SMD": // SMD-ETA-TQS-USP-0026.
                                            sSistMedicion = sTanqueTipoMedicion + "-" +
                                                            sTanqueTipoMedAlmacenamiento + "-" +
                                                            sTanqueClaveSAT + "-" +
                                                            sNomClaveInstalacion + "-" +
                                                            iNTanque.ToString().PadLeft(4, '0');
                                            break;

                                        case "SME": // SME-STQ-EDS-0021.
                                            sSistMedicion = sTanqueTipoMedicion + "-" +
                                                            sTanqueClaveSAT + "-" +
                                                            sNomClaveInstalacion + "-" +
                                                            iNTanque.ToString().PadLeft(4, '0');
                                            break;

                                        default:
                                            sSistMedicion = sTanqueTipoMedicion + "-" +
                                                            sTanqueClaveSAT + "-" +
                                                            sNomClaveInstalacion + "-" +
                                                            iNTanque.ToString().PadLeft(4, '0');
                                            break;
                                    }

                                    List<CVolJSonDTO.stMedicionTanqueDato> lstMedidores = new List<CVolJSonDTO.stMedicionTanqueDato>();
                                    lstMedidores.Add(new CVolJSonDTO.stMedicionTanqueDato
                                    {
                                        SistemaMedicionTanque = sSistMedicion,
                                        LocalizODescripSistMedicionTanque = sTanqueDescMedicion,
                                        VigenciaCalibracionSistMedicionTanque = dtTanqueFCalibracion.ToString("yyyy-MM-dd"),
                                        IncertidumbreMedicionSistMedicionTanque = Math.Round(dTanqIncertMed, 3)
                                    });

                                    objTanqueDatos.Medidores = lstMedidores;
                                    #endregion

                                    #region Existencias.
                                    CVolJSonDTO.stExistenciaDato objExistenciaDato = new CVolJSonDTO.stExistenciaDato();

                                    objExistenciaDato.VolumenExistenciasAnterior = Math.Round(dVolumenExistenciasAnterior, 3);

                                    #region Porcentaje Recepción.
                                    CVolJSonDTO.stVolumenDato objVolRecepcionDato = new CVolJSonDTO.stVolumenDato();
                                    objVolRecepcionDato.ValorNumerico = Math.Round(dVolRecepciones, 3);
                                    objVolRecepcionDato.UnidadDeMedida = sTanqueUniMed;
                                    objExistenciaDato.VolumenAcumOpsRecepcion = objVolRecepcionDato;
                                    #endregion

                                    objExistenciaDato.HoraRecepcionAcumulado = dtFechaRegistro.ToString("HH:mm:ss") + "-" +
                                                                               iDiferenciaHora.ToString("00") + ":00";

                                    #region Volumen de Entrega.
                                    CVolJSonDTO.stCapacidadDato objVolEntregaDato = new CVolJSonDTO.stCapacidadDato();
                                    objVolEntregaDato.ValorNumerico = Math.Round(dVolEntrega, 3);
                                    objVolEntregaDato.UnidadDeMedida = sTanqueUniMed;
                                    objExistenciaDato.VolumenAcumOpsEntrega = objVolEntregaDato;
                                    #endregion

                                    objExistenciaDato.HoraEntregaAcumulado = dtFechaRegistro.ToString("HH:mm:ss") + "-" +
                                                                             iDiferenciaHora.ToString("00") + ":00";
                                    objExistenciaDato.VolumenExistencias = Math.Round(dVolExistencia, 3);
                                    objExistenciaDato.FechaYHoraEstaMedicion = dtPerDateEnd.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00";
                                    objExistenciaDato.FechaYHoraMedicionAnterior = dtPerDateIni.AddSeconds(-1).ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00";

                                    objTanqueDatos.Existencias = objExistenciaDato;
                                    #endregion

                                    #region Recepciones.
                                    #region Consulta: Recepciones (Compras).
                                    var vQRecepciones = (dynamic)null;

                                    if (viGenerarComplemento && bCompRecep)
                                        #region Consulta para Complemento.
                                        vQRecepciones = (from c in objContext.InventoryIns
                                                         join cd in objContext.InventoryInDocuments on new { f1 = c.StoreId, f2 = c.InventoryInId } equals new { f1 = cd.StoreId, f2 = cd.InventoryInId } into rf
                                                         from cd in rf.DefaultIfEmpty()
                                                         where c.StoreId == viNEstacion &&
                                                               c.EndDate >= dtPerDateIni && c.EndDate <= dtPerDateEnd &&
                                                               c.TankIdi == vTanqDatos.NumeroTanque &&
                                                               c.ProductId == vProd.ProductoID
                                                         select new
                                                         {
                                                             NumeroDocumento = c.InventoryInNumber,
                                                             VolInicial = c.StartVolume,
                                                             VolFinal = c.EndVolume,
                                                             TempFinal = c.EndTemperature,
                                                             FechaInicial = c.StartDate,
                                                             FechaFinal = c.EndDate,
                                                             ProvedorID = cd.SupplierFuelIdi ?? 0,
                                                             FechaDocumento = cd.Date,
                                                             Litros = (cd.Volume ?? 0),
                                                             Precio = (cd.Price ?? 0),
                                                             PrecioVta = (cd.SalePrice ?? 0),
                                                             PrecioVtaPublico = (cd.PublicSalePrice ?? 0),
                                                             TipoComplemento = (cd.JsonTipoComplementoId ?? String.Empty),
                                                             TipoCFDI = (cd.SatTipoComprobanteId ?? String.Empty),
                                                             CFDI = (cd.Uuid ?? Guid.Empty),//(cd.SatCfdi ?? ""),
                                                             UnidadMedida = (cd.JsonClaveUnidadMedidaId ?? String.Empty),//(cd.SatMeasureUnit ?? ""),
                                                             VolDescargado = (c.EndVolume ?? 0) - (c.StartVolume ?? 0),
                                                             FolioDocumento = (from d in objContext.InventoryInDocuments
                                                                               where d.StoreId == c.StoreId &&
                                                                                     d.InventoryInId == c.InventoryInId
                                                                               select d.Folio).FirstOrDefault()
                                                         });
                                    #endregion
                                    else
                                        #region Consulta sin Complemento.
                                        vQRecepciones = (from c in objContext.InventoryIns
                                                         where c.StoreId == viNEstacion &&
                                                               c.EndDate >= dtPerDateIni && c.EndDate <= dtPerDateEnd &&
                                                               c.TankIdi == vTanqDatos.NumeroTanque &&
                                                               c.ProductId == vProd.ProductoID
                                                         select new
                                                         {
                                                             NumeroDocumento = c.InventoryInNumber,
                                                             VolInicial = c.StartVolume,
                                                             VolFinal = c.EndVolume,
                                                             TempFinal = c.EndTemperature,
                                                             FechaInicial = c.StartDate,
                                                             FechaFinal = c.EndDate,
                                                             FolioDocumento = (from d in objContext.InventoryInDocuments
                                                                               where d.StoreId == c.StoreId &&
                                                                                     d.InventoryInId == c.InventoryInId
                                                                               select d.Folio).FirstOrDefault()
                                                         });
                                    #endregion
                                    #endregion

                                    #region Lectura: Datos de la Recepción.
                                    if (vQRecepciones != null)
                                    {
                                        CVolJSonDTO.stRecepcionDato objRecepcionDato = new CVolJSonDTO.stRecepcionDato();
                                        int iCantRecep = 0, iCantDocRecep = 0, iNRecepcion = 0;
                                        Decimal dSumaVolRecep = 0;
                                        List<CVolJSonDTO.stRecepcionIndividualDato> lstRecepciones = new List<CVolJSonDTO.stRecepcionIndividualDato>();

                                        foreach (var vRecepDato in vQRecepciones)
                                        {
                                            #region Asignación: Recepcion (Compra).
                                            int iNDocRecep = vRecepDato.NumeroDocumento;//.GetValueOrDefault(),
                                                                                        //iTemperaturaRecep = Convert.ToInt32(vRecepDato.TempFinal);//.GetValueOrDefault());
                                            DateTime dtFIniRecep = vRecepDato.FechaInicial,//.GetValueOrDefault(),
                                                     dtFFinRecep = vRecepDato.FechaFinal;//.GetValueOrDefault();
                                            Decimal dRecepVolIni = vRecepDato.VolInicial,//.GetValueOrDefault(),
                                                    dRecepVolFin = vRecepDato.VolFinal,//.GetValueOrDefault(),
                                                    dRecepVol = dRecepVolFin - dRecepVolIni;
                                            Decimal dTemperaturaRecep = Convert.ToDecimal(vRecepDato.TempFinal);

                                            dSumaVolRecep += dRecepVol;
                                            iCantRecep++;

                                            if (iNDocRecep > 0)
                                                iCantDocRecep++;

                                            if (iNRecepcion > 2000)
                                                iNRecepcion = 1;
                                            else
                                                iNRecepcion++;
                                            #endregion

                                            #region Tanque: Llenado de Estructura.
                                            #region Volumen Inicial Dato.
                                            CVolJSonDTO.stCapacidadDato objRecepVolIni = new();
                                            objRecepVolIni.ValorNumerico = Math.Round(dRecepVolIni, 3);
                                            objRecepVolIni.UnidadDeMedida = sTanqueUniMed;
                                            #endregion

                                            #region Volumen Final Dato.
                                            CVolJSonDTO.stCapacidadDato objRecepVolFin = new CVolJSonDTO.stCapacidadDato();
                                            objRecepVolFin.ValorNumerico = Math.Round(dRecepVolFin, 3);
                                            #endregion

                                            #region Volumen de Recepcion.
                                            CVolJSonDTO.stCapacidadDato objRecepVolumen = new CVolJSonDTO.stCapacidadDato();
                                            objRecepVolumen.ValorNumerico = Math.Round(dRecepVol, 3);
                                            objRecepVolumen.UnidadDeMedida = sTanqueUniMed;
                                            #endregion

                                            #region Complemento.
                                            object oCompRecepDato = null;
                                            if (viGenerarComplemento && bCompRecep)
                                            {
                                                CVolJSonDTO.stComplementoRecepcionDato objComplementoDato = new CVolJSonDTO.stComplementoRecepcionDato();

                                                #region Asignación: Recepcion (Compra).
                                                String sTipoComplemento = vRecepDato.TipoComplemento,// voHSql.GetValueToString_Default(recepcion, "TipoComplementoSAT", String.Empty).Trim(),
                                                           sNacCFDI = vRecepDato.CFDI,
                                                           sNacTipoCFDI = vRecepDato.TipoCFDI,
                                                           sNacUnidMedida = vRecepDato.UnidadMedida,
                                                           sNacDescProd = vProd.ProductName;
                                                Decimal dNacPrecioCompra = (Decimal)vRecepDato.Precio,
                                                        dNacPrecioVTAPub = (Decimal)vRecepDato.PrecioVtaPublic,
                                                        dNacPrecioVTA = (Decimal)vRecepDato.PrecioVta,
                                                        dNacLitros = (Decimal)vRecepDato.Litros,
                                                        dNacVolDescargado = (Decimal)vRecepDato.VolDescargado;
                                                DateTime dtNacFechaCompra = DateTime.Now;
                                                //Guid gProveedorID = vRecepDato.ProvedorID;
                                                int iNProveedor = (int)vRecepDato.ProvedorID;

                                                if (vRecepDato.FechaDocumento != null)
                                                    dtNacFechaCompra = Convert.ToDateTime(vRecepDato.FechaDocumento);

                                                Decimal dNacVolRestitucion = dNacVolDescargado - dNacLitros;
                                                #endregion

                                                if (!String.IsNullOrEmpty(sNacCFDI) && String.IsNullOrEmpty(sNacTipoCFDI))
                                                    return BadRequest("Recepción (Document): Tipo de CFDI sin definir.");

                                                #region Proveedor Datos.
                                                #region Consulta: Proveedor Datos.
                                                //var vQProveedor = new Object();
                                                var vQProveedor = (from p in objContext.SupplierFuels
                                                                   where p.StoreId == viNEstacion &&
                                                                         p.SupplierFuelIdi == iNProveedor
                                                                   //p.SupplierFuelId == gProveedorID
                                                                   select new
                                                                   {
                                                                       p.SupplierFuelIdi,
                                                                       SupplierType = "", //(p.SupplierType ?? ""),
                                                                       SupplierRfc = "", //(p.SupplierRfc ?? ""),
                                                                       Name = (p.Name ?? ""),
                                                                       SupplierPermission = "", //(p.SupplierPermission ?? ""),
                                                                       StorageAndDistributionPermission = (p.StorageAndDistributionPermission ?? ""),
                                                                       IsConsignment = (p.IsConsignment ?? "N")
                                                                   });
                                                #endregion

                                                #region Lectura: Proveedor Datos.
                                                if (vQProveedor != null)
                                                    foreach (var vProvDato in vQProveedor)
                                                    {
                                                        List<CVolJSonDTO.stNacionalDato> lstNacional = new List<CVolJSonDTO.stNacionalDato>();

                                                        #region Asignacion: Proveedor Datos.
                                                        String sTipoProveedor = vProvDato.SupplierType,
                                                               sRFC_Proveedor = vProvDato.SupplierRfc,
                                                               sNombreProveedor = vProvDato.Name,
                                                               sPermisoProveedor = vProvDato.SupplierPermission,
                                                               sPermisoAlmProveedor = vProvDato.StorageAndDistributionPermission,
                                                               sConsignacionProveedor = vProvDato.IsConsignment;
                                                        #endregion

                                                        #region Validacion: Proveedor Datos.
                                                        if (String.IsNullOrEmpty(sPermisoProveedor))
                                                            return BadRequest("Permiso del Proveedor sin definir. (SupFuel)");

                                                        if (String.IsNullOrEmpty(sRFC_Proveedor))
                                                            return BadRequest("RFC del Proveedor sin definir. (SupFuel)");

                                                        if (String.IsNullOrEmpty(sNombreProveedor))
                                                            return BadRequest("Nombre del Proveedor sin definir. (SupFuel)");
                                                        else if (sNombreProveedor.Length < 10)
                                                            return BadRequest("El Nombre del Proveedor ('" + sRFC_Proveedor + "':'" + sNombreProveedor + "') debe contener minimo de 10 caracteres.  (SupFuel)");
                                                        #endregion

                                                        #region Llenado: Proveedor Datos.
                                                        #region TerminalAlmYDist
                                                        CVolJSonDTO.stTerminalAlmDato objTerminalDato = new CVolJSonDTO.stTerminalAlmDato();
                                                        objTerminalDato.TerminalAlmYDist = "Terminal con permiso " + sPermisoAlmProveedor;
                                                        objTerminalDato.PermisoAlmYDist = sPermisoAlmProveedor;

                                                        CVolJSonDTO.stAlmacenamientoDato objAlmacenDato = new CVolJSonDTO.stAlmacenamientoDato();
                                                        objAlmacenDato.Almacenamiento = objTerminalDato;

                                                        objComplementoDato.TerminalAlmYDist = objAlmacenDato;
                                                        #endregion

                                                        #region Nacional.
                                                        CVolJSonDTO.stNacionalDato objNacionDato = new CVolJSonDTO.stNacionalDato();
                                                        objNacionDato.RfcClienteOProveedor = sRFC_Proveedor;
                                                        objNacionDato.NombreClienteOProveedor = sNombreProveedor;
                                                        objNacionDato.PermisoProveedor = sPermisoProveedor;

                                                        #region CFDI.
                                                        List<CVolJSonDTO.stCFDIDato> lstCfDIs = new List<CVolJSonDTO.stCFDIDato>();
                                                        CVolJSonDTO.stCFDIDato objCFDI_Dato = new CVolJSonDTO.stCFDIDato();
                                                        objCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                        objCFDI_Dato.Cfdi = sNacCFDI;
                                                        objCFDI_Dato.PrecioCompra = Math.Round(dNacPrecioCompra, 3);
                                                        objCFDI_Dato.PrecioVenta = 0;
                                                        objCFDI_Dato.PrecioDeVentaAlPublico = 0;
                                                        objCFDI_Dato.FechaYHoraTransaccion = dtNacFechaCompra.ToString("s") + "-" +
                                                                                             iDiferenciaHora.ToString("00") + ":00";

                                                        #region VolumenDocumentado.
                                                        CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                        objVolumenDocumentado.ValorNumerico = Math.Round(dNacLitros, 3);
                                                        objVolumenDocumentado.UnidadDeMedida = sNacUnidMedida;

                                                        objCFDI_Dato.VolumenDocumentado = objVolumenDocumentado;
                                                        #endregion

                                                        lstCfDIs.Add(objCFDI_Dato);
                                                        objNacionDato.CFDIs = lstCfDIs;
                                                        #endregion

                                                        lstNacional.Add(objNacionDato);
                                                        objComplementoDato.Nacional = lstNacional;
                                                        #endregion

                                                        #region Aclaracion.
                                                        if (sConsignacionProveedor.Equals("Y"))
                                                        {
                                                            CVolJSonDTO.stAclaracionDato objAclaracionDato = new CVolJSonDTO.stAclaracionDato();
                                                            objAclaracionDato.Aclaracion = "RECEPCION BAJO CONTRATO PEMEX. Restitución de combustible por " + Math.Round(dNacVolRestitucion, 3).ToString() + " L de " + sNacDescProd + ". " +
                                                                                           "Volumen descargado " + Math.Round(dNacVolDescargado, 3).ToString() + " L. " +
                                                                                           "Volumen facturado " + Math.Round(dNacLitros, 3).ToString() + " L";
                                                            objComplementoDato.ACLARACION = objAclaracionDato;
                                                        }
                                                        #endregion
                                                        #endregion

                                                        objComplementoDato.TipoComplemento = sTipoComplemento;
                                                    }

                                                oCompRecepDato = objComplementoDato;
                                                #endregion
                                                #endregion
                                            }
                                            #endregion
                                            #endregion

                                            lstRecepciones.Add(new CVolJSonDTO.stRecepcionIndividualDato
                                            {
                                                NumeroDeRegistro = iNRecepcion,//(int)voHSql.GetValueToDouble(recepcion, "NUMREG"),
                                                VolumenInicialTanque = objRecepVolIni,
                                                VolumenFinalTanque = Math.Round(dRecepVolFin, 3),//objRecepVolFin,
                                                VolumenRecepcion = objRecepVolumen,
                                                Temperatura = Convert.ToInt32(Math.Truncate(dTemperaturaRecep).ToString("00")),
                                                PresionAbsoluta = 0,
                                                Complemento = oCompRecepDato,
                                                FechaYHoraInicioRecepcion = dtFIniRecep.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00",
                                                FechaYHoraFinalRecepcion = dtFFinRecep.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00"
                                            });
                                        }

                                        objRecepcionDato.TotalRecepciones = iCantRecep;

                                        #region Suma Volumen Recepcion.
                                        CVolJSonDTO.stVolumenDato objSumaVolDato = new CVolJSonDTO.stVolumenDato();
                                        objSumaVolDato.ValorNumerico = Math.Round(dSumaVolRecep, 3);
                                        objSumaVolDato.UnidadDeMedida = sTanqueUniMed;
                                        objRecepcionDato.SumaVolumenRecepcion = objSumaVolDato;
                                        #endregion

                                        objRecepcionDato.TotalDocumentos = iCantDocRecep;
                                        objRecepcionDato.SumaCompras = null;

                                        if (objRecepcionDato.TotalRecepciones > 0)
                                            objRecepcionDato.Recepcion = lstRecepciones;

                                        objTanqueDatos.Recepciones = objRecepcionDato;
                                    }
                                    #endregion
                                    #endregion

                                    #region Entregas.
                                    CVolJSonDTO.stEntregaDato objEntregaDato = new CVolJSonDTO.stEntregaDato();
                                    objEntregaDato.TotalEntregas = 0;
                                    objEntregaDato.SumaVentas = 0;
                                    objEntregaDato.TotalDocumentos = 0;

                                    CVolJSonDTO.stVolumenDato objVolEntregasDato = new CVolJSonDTO.stVolumenDato();
                                    objVolEntregasDato.ValorNumerico = 0;
                                    objVolEntregasDato.UnidadDeMedida = sTanqueUniMed;
                                    objEntregaDato.SumaVolumenEntregado = objVolEntregasDato;

                                    if (objEntregaDato.TotalEntregas > 0)
                                        /// DE MOMENTO NO SE REGISTRAN ENTREGAS
                                        objEntregaDato.Entrega = null;

                                    objTanqueDatos.Entregas = objEntregaDato;
                                    #endregion

                                    objTanqueDatos.CapacidadGasTalon = null;

                                    lstTanques.Add(objTanqueDatos);
                                    iTotalTanques++;
                                    #endregion

                                    #region Resumen Producto.
                                    if (dictDailySummary.ContainsKey(vProd.ProductoID))
                                    {
                                        dictDailySummary[vProd.ProductoID].StartInventoryQuantity += dVolumenExistenciasAnterior;
                                        dictDailySummary[vProd.ProductoID].InventoryInQuantity += dVolRecepciones;
                                        dictDailySummary[vProd.ProductoID].EndInventoryQuantity += dVolumenFinal;
                                    }
                                    #endregion
                                }
                            #endregion

                            objProductoDato.Tanque = lstTanques;
                            #endregion

                            #region Dispensarios Datos.
                            //List<int> lstDispensariosExist = new List<int>();
                            List<CVolJSonDTO.stDispensarioDato> lstDispensario = new List<CVolJSonDTO.stDispensarioDato>();
                            CVolJSonDTO.stDispensarioDato objDispensarioDatos;

                            #region Consulta: Datos del Dispensario.
                            var vQDispensario = (from m in objContext.Hoses
                                                 join pc in objContext.LoadPositions on new { f1 = m.StoreId, f2 = m.LoadPositionIdi } equals new { f1 = pc.StoreId, f2 = pc.LoadPositionIdi }
                                                 join d in objContext.Dispensaries on new { f1 = m.StoreId, f2 = pc.DispensaryIdi } equals new { f1 = d.StoreId, f2 = d.DispensaryIdi }
                                                 where m.StoreId == viNEstacion && m.ProductId == vProd.ProductoID
                                                 select new
                                                 {
                                                     m.HoseIdi,
                                                     m.Position,
                                                     pc.DispensaryIdi,
                                                     SatMeasurementType = (d.SatMeasurementType ?? ""),
                                                     SatMeasurementDescription = (d.SatMeasurementDescription ?? ""),
                                                     SatCalibrationDate = (d.SatCalibrationDate ?? DateTime.Now),
                                                     SatMeasurementPercentageUncertainty = (d.SatMeasurementPercentageUncertainty ?? 0)
                                                 });
                            #endregion

                            #region Lectura: Datos del Dispensario.
                            if (vQDispensario != null)
                            {
                                foreach (var vDispDatos in vQDispensario)
                                {
                                    #region Dispensario: Asignamos Valores.
                                    int iNDispensario = vDispDatos.DispensaryIdi,
                                        iNManguera = vDispDatos.Position.GetValueOrDefault(), // (F0405: TsNumMan)
                                        iCManguera = vDispDatos.HoseIdi.GetValueOrDefault(); // (F0405: TsCodMan)
                                    String sClaveDisp = "DISP-" + iNDispensario.ToString("0000");
                                    String sTipoMedDisp = vDispDatos.SatMeasurementType,
                                           sDescripMedDisp = vDispDatos.SatMeasurementDescription;
                                    DateTime dtFCalibracionDisp = Convert.ToDateTime(vDispDatos.SatCalibrationDate);
                                    Decimal dIncertidumbreDisp = Convert.ToDecimal(vDispDatos.SatMeasurementPercentageUncertainty);
                                    #endregion

                                    #region Dispensario: Validamos Valores.
                                    if (String.IsNullOrEmpty(sTipoMedDisp))
                                        return BadRequest("El Dispensario '" + iNDispensario.ToString() + "' no contiene un Tipo de Medición. (dispensary)");

                                    if (!sTipoMedDisp.ToUpper().Equals("SMD"))
                                        return BadRequest("El Tipo de Medición '" + sTipoMedDisp + "' no es valido debe ser 'SMD'. (dispensary)");

                                    // Creamos una lista para saber la cantidad de dispensarios que se agregaron.
                                    if (!lstDispensariosExist.Exists(d => d == iNDispensario))
                                        lstDispensariosExist.Add(iNDispensario);
                                    #endregion

                                    #region Dispensario: Llenado de Estructura.
                                    #region Dispensario: Validamos si se lleno previamente las Mangueras del Dispensario.
                                    List<CVolJSonDTO.stMangueraDato> lstMangueras = new List<CVolJSonDTO.stMangueraDato>();
                                    int iIndexDisp = lstDispensario.FindIndex(d => d.ClaveDispensario.Equals(sClaveDisp));
                                    if (iIndexDisp < 0)
                                    {
                                        objDispensarioDatos = new CVolJSonDTO.stDispensarioDato();
                                        objDispensarioDatos.ClaveDispensario = sClaveDisp;

                                        #region Medición.
                                        Decimal dIncertMedDisp = 0;
                                        if (dIncertidumbreDisp > 0)
                                            dIncertMedDisp = (dIncertidumbreDisp / 100);
                                        else
                                            dIncertidumbreDisp = Convert.ToDecimal(0.001);

                                        List<CVolJSonDTO.stDispensarioMedicionDato> lstMedicionesDisp = new List<CVolJSonDTO.stDispensarioMedicionDato>();
                                        lstMedicionesDisp.Add(new CVolJSonDTO.stDispensarioMedicionDato
                                        {
                                            SistemaMedicionDispensario = sTipoMedDisp + "-DISP-" + iNDispensario.ToString("0000"),
                                            LocalizODescripSistMedicionDispensario = sDescripMedDisp,
                                            VigenciaCalibracionSistMedicionDispensario = dtFCalibracionDisp.ToString("yyyy-MM-dd"),
                                            IncertidumbreMedicionSistMedicionDispensario = Math.Round(dIncertMedDisp, 3)
                                        });

                                        objDispensarioDatos.Medidores = lstMedicionesDisp;
                                        #endregion

                                        lstDispensario.Add(objDispensarioDatos);
                                        iIndexDisp = lstDispensario.FindIndex(d => d.ClaveDispensario.Equals(sClaveDisp));
                                    }
                                    else
                                        lstMangueras = (List<CVolJSonDTO.stMangueraDato>)lstDispensario[iIndexDisp].Manguera;
                                    #endregion

                                    #region Dispensario: Mangueras.
                                    #region Consulta: Transacciones de la Manguera.
                                    var vQMTransacciones = (from th in objContext.SaleOrders
                                                            join td in objContext.SaleSuborders on th.SaleOrderId equals td.SaleOrderId
                                                            join p in objContext.Products on td.ProductId equals p.ProductId
                                                            where th.StoreId == viNEstacion &&
                                                                  th.StartDate >= dtPerDateIni && th.StartDate <= dtPerDateEnd &&
                                                                  th.HoseIdi == vDispDatos.HoseIdi &&
                                                                  p.IsFuel == true
                                                            select new
                                                            {
                                                                th.SaleOrderNumber,
                                                                td.Quantity,
                                                                td.Price,
                                                                StartDate = (th.StartDate ?? DateTime.Now),
                                                                consignment_sale = "N"
                                                            });
                                    #endregion

                                    #region Lectura: Transacciones de la Manguera.
                                    if (vQMTransacciones != null)
                                    {
                                        List<CVolJSonDTO.stEntregaMangueraDato> lstMangEntregas = new List<CVolJSonDTO.stEntregaMangueraDato>();
                                        List<CVolJSonDTO.stEntregaMangueraIndividualDato> lstMangEntregaDato = new List<CVolJSonDTO.stEntregaMangueraIndividualDato>();
                                        Decimal dTMangLitros = 0, dMangImporte = 0;

                                        foreach (var vTransDatos in vQMTransacciones)
                                        {
                                            #region Transacciones_Manguera: Asignamos Valores.
                                            String sClienteConsignacion = vTransDatos.consignment_sale;
                                            Decimal dMangLitros = (Decimal)vTransDatos.Quantity,
                                                    dMangPrecio = (Decimal)vTransDatos.Price;
                                            dTMangLitros += dMangLitros;
                                            #endregion

                                            if (iNTransaccionNew > 2000)
                                                iNTransaccionNew = 1;

                                            #region Transacciones_Manguera: Llenado de Estructura.
                                            CVolJSonDTO.stEntregaMangueraIndividualDato objMangEntDato = new CVolJSonDTO.stEntregaMangueraIndividualDato();
                                            objMangEntDato.NumeroDeRegistro = iNTransaccionNew;
                                            objMangEntDato.TipoDeRegistro = "D";
                                            objMangEntDato.FechaYHoraEntrega = Convert.ToDateTime(vTransDatos.StartDate).ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00";
                                            objMangEntDato.PrecioVentaTotalizadorInsta = Math.Round((dMangLitros * dMangPrecio), 2);
                                            dMangImporte += Math.Round((dMangLitros * dMangPrecio), 2);

                                            #region Volumen Entregado Total Instante.
                                            CVolJSonDTO.stCapacidadDato objVolEntTolInstDato = new CVolJSonDTO.stCapacidadDato();
                                            objVolEntTolInstDato.ValorNumerico = Math.Round(dMangLitros, 3);
                                            objVolEntTolInstDato.UnidadDeMedida = "UM03";
                                            objMangEntDato.VolumenEntregadoTotalizadorInsta = objVolEntTolInstDato;
                                            #endregion

                                            #region Volumen Entregado Total Acumulado.
                                            CVolJSonDTO.stCapacidadDato objVolEntTolAcumDato = new CVolJSonDTO.stCapacidadDato();
                                            objVolEntTolAcumDato.ValorNumerico = Math.Round(dTMangLitros, 3);
                                            objVolEntTolAcumDato.UnidadDeMedida = "UM03";
                                            objMangEntDato.VolumenEntregadoTotalizadorAcum = objVolEntTolAcumDato;
                                            #endregion

                                            #region Complemento: Entrega Manguera.
                                            object oEntComplDato = null;

                                            if (viGenerarComplemento && bCompEntregas)
                                                #region Aclaracion.
                                                if (sClienteConsignacion.Equals("Y"))
                                                {
                                                    CVolJSonDTO.stComplementoRecepcionDato objEntregaCompDato = new CVolJSonDTO.stComplementoRecepcionDato();
                                                    objMangEntDato.TipoDeRegistro = "E";

                                                    CVolJSonDTO.stAclaracionDato objAclaracionDato = new CVolJSonDTO.stAclaracionDato();
                                                    objAclaracionDato.Aclaracion = "ENTREGA CONTRATO PEMEX, " + iNTransaccionNew.ToString();
                                                    objEntregaCompDato.ACLARACION = objAclaracionDato;

                                                    oEntComplDato = objEntregaCompDato;
                                                }
                                            #endregion

                                            objMangEntDato.Complemento = oEntComplDato;
                                            #endregion

                                            lstMangEntregaDato.Add(objMangEntDato);

                                            #region Resumen Dia.
                                            if (dictDailySummary.ContainsKey(vProd.ProductoID))
                                            {
                                                dictDailySummary[vProd.ProductoID].Price = dMangPrecio;
                                                dictDailySummary[vProd.ProductoID].SaleAmount += dMangImporte;
                                                dictDailySummary[vProd.ProductoID].SaleQuantity += dMangLitros;
                                            }
                                            #endregion

                                            iNTransaccionNew++;
                                            #endregion
                                        }

                                        CVolJSonDTO.stEntregaMangueraDato objEntMangDato = new CVolJSonDTO.stEntregaMangueraDato();
                                        objEntMangDato.TotalEntregas = vQMTransacciones.Count();

                                        if (vQMTransacciones.Count() > 0)
                                            objEntMangDato.Entrega = lstMangEntregaDato;

                                        if (dMangImporte > 0)
                                            objEntMangDato.SumaVentas = dMangImporte;

                                        objEntMangDato.TotalDocumentos = vQMTransacciones.Count();

                                        #region Suma Volumen Entregado.
                                        CVolJSonDTO.stCapacidadDato objSumaVolEntDato = new CVolJSonDTO.stCapacidadDato();
                                        objSumaVolEntDato.ValorNumerico = Math.Round(dTMangLitros, 3);
                                        objSumaVolEntDato.UnidadDeMedida = "UM03";
                                        objEntMangDato.SumaVolumenEntregado = objSumaVolEntDato;
                                        #endregion

                                        CVolJSonDTO.stMangueraDato objMangueraDato = new CVolJSonDTO.stMangueraDato();
                                        objMangueraDato.IdentificadorManguera = sClaveDisp + "-MGA-" + iCManguera.ToString().PadLeft(4, '0');
                                        objMangueraDato.Entregas = objEntMangDato;

                                        lstMangueras.Add(objMangueraDato);
                                    }

                                    objDispensarioDatos = lstDispensario[iIndexDisp];
                                    objDispensarioDatos.Manguera = lstMangueras;
                                    lstDispensario[iIndexDisp] = objDispensarioDatos;
                                    #endregion
                                    #endregion

                                    #endregion
                                }
                            }
                            #endregion

                            iTotalDispensarios = lstDispensariosExist.Count;
                            objProductoDato.Dispensario = lstDispensario;
                            #endregion

                            // Calculamos los totales del Resumen Diario.
                            dictDailySummary[vProd.ProductoID].TheoryInventoryQuantity = ((dictDailySummary[vProd.ProductoID].StartInventoryQuantity + dictDailySummary[vProd.ProductoID].InventoryInQuantity) - dictDailySummary[vProd.ProductoID].SaleQuantity);
                            dictDailySummary[vProd.ProductoID].InventoryDifference = dictDailySummary[vProd.ProductoID].EndInventoryQuantity - dictDailySummary[vProd.ProductoID].TheoryInventoryQuantity;
                            dictDailySummary[vProd.ProductoID].InventoryDifferencePercentage = dictDailySummary[vProd.ProductoID].InventoryDifference / dictDailySummary[vProd.ProductoID].SaleQuantity;
                        }
                        else
                        {

                        }
                        break;
                    #endregion

                    #region Tipo: Reporte Mensual.
                    case CVolJSonDTO.eTipoReporte.Mes:
                        // Agregamos el registro para generar el resumen en la tabla "Monthly_Summary"
                        if (!dictMonthlySummary.ContainsKey(vProd.ProductoID))
                        {
                            #region Obtenemos el Volumen Inicial.
                            #region Consulta: Volumen Inicial.
                            var vQVolIniProd = (from ia in objContext.Inventories
                                                where ia.StoreId == viNEstacion &&
                                                      ia.ProductId == vProd.ProductoID &&
                                                      ia.Date >= dtPerDateIni && ia.Date <= dtPerDateEnd
                                                orderby ia.Date
                                                select (from ian in objContext.Inventories
                                                        where ian.StoreId == ia.StoreId &&
                                                              ian.ProductId == ia.ProductId &&
                                                              ian.Date >= ia.Date.Value.AddSeconds(-ia.Date.Value.Second) &&
                                                              ian.Date <= ia.Date.Value.AddSeconds(-ia.Date.Value.Second).AddSeconds(59)
                                                        select ian.Volume ?? 0).Sum()).FirstOrDefault();
                            #endregion
                            #endregion

                            #region Obtenemos el Volumen Final.
                            #region Consulta: Volumen Final.
                            var vQVolFinProd = (from ia in objContext.Inventories
                                                where ia.StoreId == viNEstacion &&
                                                      ia.ProductId == vProd.ProductoID &&
                                                      ia.Date >= dtPerDateIni && ia.Date <= dtPerDateEnd
                                                orderby ia.Date descending
                                                select (from ian in objContext.Inventories
                                                        where ian.StoreId == ia.StoreId &&
                                                              ian.ProductId == ia.ProductId &&
                                                              ian.Date >= ia.Date.Value.AddSeconds(-ia.Date.Value.Second) &&
                                                              ian.Date <= ia.Date.Value.AddSeconds(-ia.Date.Value.Second).AddSeconds(59)
                                                        select ian.Volume ?? 0).Sum()).FirstOrDefault();
                            #endregion
                            #endregion

                            dictMonthlySummary.Add(vProd.ProductoID, new MonthlySummary
                            {
                                StoreId = Guid.Parse(viNEstacion.ToString()),
                                Date = dtPerDateIni,
                                ProductId = vProd.ProductoID,
                                StartInventoryQuantity = vQVolIniProd,
                                EndInventoryQuantity = vQVolFinProd
                            });

                        }

                        CVolJSonDTO.stReporteVolumenMensualDato objRepVolMenDato = new CVolJSonDTO.stReporteVolumenMensualDato();

                        if (viReporteEstacion)
                        {
                            #region Reporte para Estación.
                            String sTipoComplemento = "";

                            #region Existencias.
                            CVolJSonDTO.stControlExistenciaDato objExistMesDato = new CVolJSonDTO.stControlExistenciaDato();

                            #region Consulta: Existencia del Producto.
                            var vQExistProd = (from t in objContext.Tanks
                                               where t.StoreId == viNEstacion && t.ProductId == vProd.ProductoID
                                               select (from i in objContext.InventoryIns
                                                       orderby i.StartDate descending
                                                       where i.StoreId == t.StoreId &&
                                                             i.TankIdi == t.TankIdi &&
                                                             i.StartDate >= dtPerDateIni && i.StartDate <= dtPerDateEnd.AddMinutes(7) &&
                                                             i.ProductId == t.ProductId
                                                       select i.Volume ?? 0).FirstOrDefault());
                            #endregion

                            #region Lectura: Existencia Datos.
                            if (vQExistProd != null)
                            {
                                Decimal dExistenciaDia = 0;

                                foreach (var vExistDatos in vQExistProd)
                                    dExistenciaDia += (Decimal)vExistDatos;

                                objExistMesDato.VolumenExistenciasMes = Math.Round(dExistenciaDia, 3);
                                objExistMesDato.FechaYHoraEstaMedicionMes = dtPerDateEnd.ToString("yyyy-MM-dd") + "T" +
                                                                            dtPerDateEnd.ToString("HH:mm:ss") + "-" +
                                                                            iDiferenciaHora.ToString("00") + ":00";
                            }
                            #endregion

                            objRepVolMenDato.ControlDeExistencias = objExistMesDato;
                            #endregion

                            #region Recepciones.
                            CVolJSonDTO.stRecepcionMesDato objRecepMesDato = new CVolJSonDTO.stRecepcionMesDato();

                            #region Consulta: Recepciones.
                            var vQRecepProd = (from r in objContext.InventoryIns
                                               join rd in objContext.InventoryInDocuments on new { f1 = r.StoreId, f2 = r.InventoryInId } equals new { f1 = rd.StoreId, f2 = rd.InventoryInId } into rf
                                               from rd in rf.DefaultIfEmpty()
                                               where r.StoreId == viNEstacion && r.ProductId == vProd.ProductoID &&
                                                     r.EndDate >= dtPerDateIni && r.EndDate <= dtPerDateEnd
                                               group new { r, rd } by r.StoreId into g
                                               select new
                                               {
                                                   CantRegistros = g.Count(),
                                                   LitrosRecep = g.Sum(o => o.rd.Volume),
                                                   ImporteRecep = g.Sum(o => o.rd.Volume * o.rd.Price)
                                               });
                            #endregion

                            #region Lectura: Recepciones.
                            if (vQRecepProd != null)
                                foreach (var vRecepDato in vQRecepProd)
                                {
                                    #region Recepción: Asignación de Valores.
                                    int iCantRecep = (int)vRecepDato.CantRegistros;
                                    Decimal dLitrosRecep = (Decimal)vRecepDato.LitrosRecep,
                                            dImporteRecep = (Decimal)vRecepDato.ImporteRecep;
                                    #endregion

                                    #region Recepción: Llenado de Estructura.
                                    objRecepMesDato.TotalRecepcionesMes = iCantRecep;
                                    objRecepMesDato.TotalDocumentosMes = iCantRecep;
                                    #region Suma Volumen Recepcion Mens.
                                    CVolJSonDTO.stCapacidadDato objSumVolMesDato = new CVolJSonDTO.stCapacidadDato();
                                    objSumVolMesDato.ValorNumerico = Math.Round(dLitrosRecep, 3);
                                    objSumVolMesDato.UnidadDeMedida = "UM03";
                                    objRecepMesDato.SumaVolumenRecepcionMes = objSumVolMesDato;
                                    #endregion
                                    objRecepMesDato.ImporteTotalRecepcionesMensual = Math.Round(dImporteRecep, 3);

                                    #region Complemento.
                                    List<CVolJSonDTO.stComplementoRecepcionDato> lstComplementos = new List<CVolJSonDTO.stComplementoRecepcionDato>();
                                    lstComplementos.Clear();

                                    if (viGenerarComplemento && bCompRecep)
                                    {
                                        #region Consulta: Recepcion Proveedores Datos.
                                        var vQRecepProv = (from c in objContext.InventoryIns
                                                           join cd in objContext.InventoryInDocuments on c.InventoryInId equals cd.InventoryInId
                                                           join sd in objContext.SupplierFuels on cd.SupplierFuelIdi equals sd.SupplierFuelIdi
                                                           where c.StoreId == viNEstacion &&
                                                                 c.StartDate >= dtPerDateIni && c.StartDate <= dtPerDateEnd &&
                                                                 c.ProductId == vProd.ProductoID
                                                           select new
                                                           {
                                                               cd.SupplierFuelIdi,
                                                               SupplierFuelId = sd.SupplierFuelIdi,
                                                               SupplierType = (sd.SupplierType ?? ""),
                                                               SupplierRfc = (sd.Rfc ?? ""),
                                                               Name = (sd.Name ?? ""),
                                                               SupplierPermission = (sd.FuelPermission ?? ""),
                                                               StorageAndDistributionPermission = (sd.StorageAndDistributionPermission ?? ""),
                                                               IsConsignment = (sd.IsConsignment ?? "N")
                                                           }).Distinct();
                                        #endregion

                                        #region Lectura: Recepcion Proveedores Datos.
                                        if (vQRecepProv != null)
                                            foreach (var vRProvDato in vQRecepProv)
                                            {
                                                CVolJSonDTO.stComplementoRecepcionDato objComplementoDato = new CVolJSonDTO.stComplementoRecepcionDato();
                                                List<CVolJSonDTO.stNacionalDato> lstNacional = new List<CVolJSonDTO.stNacionalDato>();
                                                String sAclaracionSAT = String.Empty;

                                                #region Asignacion: Recepcion Proveedores.
                                                String sTipoProveedor = vRProvDato.SupplierType,
                                                           sRFC_Proveedor = vRProvDato.SupplierRfc,
                                                           sNombreProveedor = vRProvDato.Name,
                                                           sPermisoProveedor = vRProvDato.SupplierPermission,
                                                           sPermisoAlmProveedor = vRProvDato.StorageAndDistributionPermission,
                                                           sConsignacionProveedor = vRProvDato.IsConsignment;
                                                int iNProveedor = vRProvDato.SupplierFuelId;
                                                //Guid? gProveedorID = vRProvDato.SupplierFuelId;
                                                #endregion

                                                #region Validacion: Recepción Proveedores.
                                                if (String.IsNullOrEmpty(sPermisoProveedor))
                                                    return BadRequest("Permiso del Proveedor sin definir. (SupFuel)");

                                                if (String.IsNullOrEmpty(sRFC_Proveedor))
                                                    return BadRequest("RFC del Proveedor sin definir. (SupFuel)");

                                                if (String.IsNullOrEmpty(sNombreProveedor))
                                                    return BadRequest("Nombre del Proveedor sin definir. (SupFuel)");
                                                else if (sNombreProveedor.Length < 10)
                                                    return BadRequest("El Nombre del Proveedor ('" + sRFC_Proveedor + "':'" + sNombreProveedor + "') debe contener minimo de 10 caracteres.");
                                                #endregion

                                                #region Llenado: Recepción Proveedores.
                                                #region TerminalAlmYDist
                                                CVolJSonDTO.stTerminalAlmDato objTerminalDato = new CVolJSonDTO.stTerminalAlmDato();
                                                objTerminalDato.TerminalAlmYDist = "Terminal con permiso " + sPermisoAlmProveedor;
                                                objTerminalDato.PermisoAlmYDist = sPermisoAlmProveedor;

                                                CVolJSonDTO.stAlmacenamientoDato objAlmacenDato = new CVolJSonDTO.stAlmacenamientoDato();
                                                objAlmacenDato.Almacenamiento = objTerminalDato;

                                                objComplementoDato.TerminalAlmYDist = objAlmacenDato;
                                                #endregion

                                                int iNCantRecepcionesProv = 0;
                                                switch (sTipoProveedor)
                                                {
                                                    #region Nacional.
                                                    case "N": // NACIONAL
                                                        CVolJSonDTO.stNacionalDato objNacionDato = new CVolJSonDTO.stNacionalDato();
                                                        objNacionDato.RfcClienteOProveedor = sRFC_Proveedor;
                                                        objNacionDato.NombreClienteOProveedor = sNombreProveedor;
                                                        objNacionDato.PermisoProveedor = sPermisoProveedor;

                                                        List<CVolJSonDTO.stCFDIDato> lstCfDIs = new List<CVolJSonDTO.stCFDIDato>();

                                                        #region Consulta: Recepciones Proveedor.
                                                        var vQProvRecep = (from c in objContext.InventoryIns
                                                                           join cd in objContext.InventoryInDocuments on c.InventoryInId equals cd.InventoryInId
                                                                           where c.StoreId == viNEstacion &&
                                                                                 c.StartDate >= dtPerDateIni && c.StartDate <= dtPerDateEnd &&
                                                                                 c.ProductId == vProd.ProductoID &&
                                                                                 //cd.SupplierFuelId == vRProvDato.SupplierFuelId
                                                                                 cd.SupplierFuelIdi == vRProvDato.SupplierFuelIdi
                                                                           select new
                                                                           {
                                                                               SatTypeComplement = (cd.JsonTipoComplementoId ?? String.Empty),//(cd.SatTypeComplement ?? ""),
                                                                               SatTypeCfdi = (cd.SatTipoComprobanteId ?? String.Empty),//(cd.SatTypeCfdi ?? ""),
                                                                               SatCfdi = (cd.Uuid ?? Guid.Empty),//(cd.SatCfdi ?? ""),
                                                                               Price = (cd.Price ?? 0),
                                                                               SalePrice = (cd.SalePrice ?? 0),
                                                                               PublicSalePrice = (cd.PublicSalePrice ?? 0),
                                                                               cd.Date,
                                                                               Volume = (cd.Volume ?? 0),
                                                                               VolumenDescargado = ((c.EndVolume ?? 0) - (c.StartVolume ?? 0)),
                                                                               SatMeasureUnit = (cd.JsonClaveUnidadMedidaId ?? String.Empty),//(cd.SatMeasureUnit ?? ""),
                                                                               SatClarification = (cd.SatAclaracion ?? String.Empty)//(cd.SatClarification ?? "")
                                                                           });
                                                        #endregion

                                                        #region Lectura: Recepciones Proveedor.
                                                        if (vQProvRecep != null)
                                                            foreach (var vPRecepDato in vQProvRecep)
                                                            {
                                                                #region Asignación: Recepción Proveedor.
                                                                sTipoComplemento = vPRecepDato.SatTypeComplement;
                                                                sAclaracionSAT = vPRecepDato.SatClarification;
                                                                String sNacCFDI = vPRecepDato.SatCfdi.ToString(),
                                                                       sNacTipoCFDI = vPRecepDato.SatTypeCfdi,
                                                                       sNacUnidMedida = vPRecepDato.SatMeasureUnit,
                                                                       sNacDescProd = vProd.ProductName;
                                                                Decimal dNacPrecioCompra = (Decimal)vPRecepDato.Price,
                                                                        dNacPrecioVTAPub = (Decimal)vPRecepDato.PublicSalePrice,
                                                                        dNacPrecioVTA = (Decimal)vPRecepDato.SalePrice,
                                                                        dNacLitros = (Decimal)vPRecepDato.Volume,
                                                                        dNacVolDescargado = (Decimal)vPRecepDato.VolumenDescargado;
                                                                DateTime dtNacFechaCompra = Convert.ToDateTime(vPRecepDato.Date);

                                                                Decimal dNacVolRestitucion = dNacVolDescargado - dNacLitros;
                                                                #endregion

                                                                #region Validacion:  Recepción Proveedor.
                                                                if (String.IsNullOrEmpty(sNacCFDI))
                                                                    return BadRequest("CFDI sin definir. (InvInDoct)");

                                                                if (String.IsNullOrEmpty(sNacTipoCFDI))
                                                                    return BadRequest("Tipo de CFDI sin definir. (InvInDoct)");

                                                                if (!String.IsNullOrEmpty(sAclaracionSAT))
                                                                    if (sAclaracionSAT.Length < 6)
                                                                        return BadRequest("La Aclaración de la Recepcion con CFDI '" + sNacCFDI + "' no debe ser menor a 6 caracteres.");
                                                                #endregion

                                                                #region Llenado: .
                                                                switch (sNacTipoCFDI.ToUpper())
                                                                {
                                                                    case "INGRESO": sNacTipoCFDI = "Ingreso"; break;
                                                                    case "EGRESO": sNacTipoCFDI = "Egreso"; break;
                                                                    case "TRASLADO": sNacTipoCFDI = "Traslado"; break;
                                                                }

                                                                CVolJSonDTO.stCFDIDato objCFDI_Dato = new CVolJSonDTO.stCFDIDato();
                                                                objCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                objCFDI_Dato.Cfdi = sNacCFDI;
                                                                objCFDI_Dato.PrecioCompra = Math.Round(dNacPrecioCompra, 3);
                                                                objCFDI_Dato.PrecioVenta = 0;//Math.Round(dNacPrecioVTA, 3);
                                                                objCFDI_Dato.PrecioDeVentaAlPublico = 0;//Math.Round(dNacPrecioVTAPub, 3);
                                                                objCFDI_Dato.FechaYHoraTransaccion = dtNacFechaCompra.ToString("s") + "-" +
                                                                                                     iDiferenciaHora.ToString("00") + ":00";

                                                                #region VolumenDocumentado.
                                                                CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                objVolumenDocumentado.ValorNumerico = Math.Round(dNacLitros, 3);//Math.Round(dNacLitros, 3);
                                                                objVolumenDocumentado.UnidadDeMedida = sNacUnidMedida;

                                                                objCFDI_Dato.VolumenDocumentado = objVolumenDocumentado;
                                                                #endregion

                                                                #region Aclaracion.
                                                                if (sConsignacionProveedor.Equals("Y"))
                                                                    objCFDI_Dato.Aclaracion = "RECEPCION BAJO CONTRATO PEMEX. Restitución de combustible por " + Math.Round(dNacVolRestitucion, 3).ToString() + " L de " + sNacDescProd + ". " +
                                                                                              "Volumen descargado " + Math.Round(dNacVolDescargado, 3).ToString() + " L. " +
                                                                                              "Volumen facturado " + Math.Round(dNacLitros, 3).ToString() + " L";
                                                                #endregion

                                                                lstCfDIs.Add(objCFDI_Dato);
                                                                #endregion
                                                            }
                                                        #endregion

                                                        if (lstCfDIs.Count > 0)
                                                        {
                                                            objNacionDato.CFDIs = lstCfDIs;
                                                            iNCantRecepcionesProv = lstCfDIs.Count;

                                                            lstNacional.Add(objNacionDato);
                                                        }
                                                        break;
                                                        #endregion
                                                }
                                                #endregion

                                                if (iNCantRecepcionesProv > 0)
                                                {
                                                    objComplementoDato.Nacional = lstNacional;
                                                    objComplementoDato.TipoComplemento = sTipoComplemento;

                                                    if (!String.IsNullOrEmpty(sAclaracionSAT))
                                                        objComplementoDato.ACLARACION = sAclaracionSAT;

                                                    lstComplementos.Add(objComplementoDato);
                                                }
                                            }
                                        #endregion
                                    }

                                    objRecepMesDato.Complemento = lstComplementos;
                                    #endregion

                                    #endregion

                                    // Resumen Mensual.
                                    dictMonthlySummary[vProd.ProductoID].InventoryInQuantity += dLitrosRecep;
                                }
                            #endregion

                            objRepVolMenDato.Recepciones = objRecepMesDato;
                            #endregion

                            #region Entregas.
                            CVolJSonDTO.stEntregasMesDato objEntMesDato = new CVolJSonDTO.stEntregasMesDato();

                            #region Consulta: Entregas del Producto.
                            var vQEntregaProd = (from th in objContext.SaleOrders
                                                 join td in objContext.SaleSuborders on new { f1 = th.SaleOrderId } equals new { f1 = td.SaleOrderId }
                                                 where th.StoreId == viNEstacion &&
                                                       th.StartDate >= dtPerDateIni && th.StartDate <= dtPerDateEnd &&
                                                       td.ProductId == vProd.ProductoID
                                                 group new { th, td } by th.StoreId into g
                                                 select new
                                                 {
                                                     CantEnt = g.Count(),
                                                     PrecioEnt = (g.Min(o => o.td.Price) ?? 0),
                                                     LitrosEnt = g.Sum(o => o.td.Quantity),
                                                     ImporteEnt = g.Sum(o => o.td.Amount)
                                                 });
                            #endregion

                            #region Lectura: Entregas del Producto.
                            if (vQEntregaProd != null)
                                foreach (var vEntregaDato in vQEntregaProd)
                                {
                                    #region Entrega Datos: Asignación de Valores.
                                    int iCantEnt = (int)vEntregaDato.CantEnt;
                                    Decimal dPrecioEnt = (Decimal)vEntregaDato.PrecioEnt,
                                            dLitrosEnt = (Decimal)vEntregaDato.LitrosEnt,
                                            dImporteEnt = (Decimal)vEntregaDato.ImporteEnt;
                                    #endregion

                                    #region Entrega Datos: Llenado de Estructura.
                                    objEntMesDato.TotalEntregasMes = iCantEnt;
                                    objEntMesDato.SumaVolumenEntregadoMes = Math.Round(dLitrosEnt, 3);
                                    #region Suma Volumen Entregado Mes.
                                    CVolJSonDTO.stCapacidadDato objSumVolEntMesDato = new CVolJSonDTO.stCapacidadDato();
                                    objSumVolEntMesDato.ValorNumerico = Math.Round(dLitrosEnt, 3);
                                    objSumVolEntMesDato.UnidadDeMedida = "UM03";
                                    objEntMesDato.SumaVolumenEntregadoMes = objSumVolEntMesDato;
                                    #endregion

                                    objEntMesDato.ImporteTotalEntregasMes = Math.Round(dImporteEnt, 3);
                                    objEntMesDato.TotalDocumentosMes = iCantEnt;

                                    #region Complemento.
                                    List<CVolJSonDTO.stComplementoRecepcionDato> lstComplementos = new List<CVolJSonDTO.stComplementoRecepcionDato>();
                                    lstComplementos.Clear();

                                    if (viGenerarComplemento && bCompEntregas)
                                    {
                                        CVolJSonDTO.stComplementoRecepcionDato objComplementoDato = new CVolJSonDTO.stComplementoRecepcionDato();
                                        List<CVolJSonDTO.stNacionalDato> lstNacional = new List<CVolJSonDTO.stNacionalDato>();

                                        List<String> lstTipoCFDIValidos = new List<String>();
                                        lstTipoCFDIValidos.Add("INGRESO");
                                        lstTipoCFDIValidos.Add("EGRESO");
                                        lstTipoCFDIValidos.Add("TRASLADO");

                                        #region Consulta: Facturas.
                                        var vQEFacuras = (from th in objContext.SaleOrders
                                                          join sd in objContext.SaleSuborders on th.SaleOrderId equals sd.SaleOrderId
                                                          join tf in objContext.InvoiceSaleOrders on th.SaleOrderId equals tf.SaleOrderId
                                                          join fh in objContext.Invoices on new { f1 = th.StoreId, f2 = tf.InvoiceId } equals new { f1 = fh.StoreId, f2 = fh.InvoiceId }
                                                          join fd in objContext.InvoiceDetails on fh.InvoiceId equals fd.InvoiceId
                                                          join tc in objContext.SatTipoComprobantes on fh.SatTipoComprobanteId equals tc.SatTipoComprobanteId into ltc
                                                          from tc in ltc.DefaultIfEmpty()
                                                          join c in objContext.Customers on fh.CustomerId equals c.CustomerId into lc
                                                          from c in lc.DefaultIfEmpty()
                                                          where th.StoreId == viNEstacion &&
                                                                th.StartDate >= dtPerDateIni && th.StartDate <= dtPerDateEnd &&
                                                                sd.ProductId == vProd.ProductoID
                                                          select new
                                                          {
                                                              Serie = fh.InvoiceSerieId,
                                                              Folio = (fh.Folio ?? "0"),
                                                              Fecha = fh.Date,
                                                              TipoComprobante = (tc.Descripcion ?? ""),
                                                              UUID = (fh.Uuid ?? ""),
                                                              Nombre = (c.Name ?? ""),
                                                              RFC = (c.Rfc ?? ""),
                                                              VentaConsignacion = (c.SatConsignmentSale ?? "N"),
                                                              Precio = (from sd in objContext.SaleSuborders
                                                                        where sd.SaleOrderId == th.SaleOrderId && sd.ProductId == vProd.ProductoID
                                                                        select new { sd.Price }).Min(p => p.Price),
                                                              Litros = (from sd in objContext.SaleSuborders
                                                                        where sd.SaleOrderId == th.SaleOrderId && sd.ProductId == vProd.ProductoID
                                                                        select new { sd.Quantity }).Sum(p => p.Quantity)
                                                          });
                                        #endregion

                                        #region Lectura: Facturas Entregas.
                                        if (vQEFacuras != null)
                                            foreach (var vEFacturaDato in vQEFacuras)
                                            {
                                                #region Asignación: Entrega Factura Datos
                                                String sNacCFDI = vEFacturaDato.UUID,
                                                       sNacTipoCFDI = vEFacturaDato.TipoComprobante,
                                                       sNacUnidMedida = "UM03",
                                                       sNacRFCCliente = vEFacturaDato.RFC,
                                                       sNacNombreCliente = vEFacturaDato.Nombre,
                                                       sNacSerie = vEFacturaDato.Serie;
                                                DateTime dtNacFechaCompra = Convert.ToDateTime(vEFacturaDato.Fecha);
                                                //<<<<<<< HEAD
                                                string iNacNumeroFact = vEFacturaDato.Folio;
                                                //=======
                                                //Int32 iNacNumeroFact = Convert.ToInt32(vEFacturaDato.Folio);
                                                //>>>>>>> 163c4b0ebfec45b202e8f2206252d12d4b2ba186
                                                Decimal dNacPrecioFact = vEFacturaDato.Precio.GetValueOrDefault(),
                                                        dNacLitrosFact = vEFacturaDato.Litros.GetValueOrDefault();
                                                String sNacClienteConsigacion = vEFacturaDato.VentaConsignacion;
                                                #endregion

                                                #region Validacion: Entrega Factura Datos.
                                                if (String.IsNullOrEmpty(sNacCFDI))
                                                    return BadRequest("CFDI sin definir. (Invo)");

                                                if (String.IsNullOrEmpty(sNacTipoCFDI))
                                                    return BadRequest("Tipo de CFDI sin definir de la Factura '" + sNacSerie + "-" + iNacNumeroFact.ToString() + "' (Invo)");

                                                if (!lstTipoCFDIValidos.Exists(t => t.Equals(sNacTipoCFDI.ToUpper())))
                                                    return BadRequest("El Tipo de CFDI '" + sNacTipoCFDI + "' no es valido de la Factura '" + sNacSerie + "-" + iNacNumeroFact.ToString() + "'. (Invo)");

                                                if (!String.IsNullOrEmpty(sNacNombreCliente))
                                                    if (sNacNombreCliente.Length < 10)
                                                        return BadRequest("El Nombre del Cliente ('" + sNacRFCCliente + "':'" + sNacNombreCliente + "') debe contener minimo de 10 caracteres. (Invo)");

                                                switch (sNacTipoCFDI.ToUpper())
                                                {
                                                    case "INGRESO": sNacTipoCFDI = "Ingreso"; break;
                                                    case "EGRESO": sNacTipoCFDI = "Egreso"; break;
                                                    case "TRASLADO": sNacTipoCFDI = "Traslado"; break;
                                                }
                                                #endregion

                                                #region Llenado: Entrega Factura Datos.
                                                CVolJSonDTO.stCFDIDato objCFDI_Dato = new CVolJSonDTO.stCFDIDato();
                                                objCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                objCFDI_Dato.Cfdi = sNacCFDI;
                                                objCFDI_Dato.PrecioCompra = 0.0; //Math.Round(dNacPrecio, 3);
                                                objCFDI_Dato.PrecioVenta = Math.Round(dNacPrecioFact, 3);
                                                objCFDI_Dato.PrecioDeVentaAlPublico = Math.Round(dNacPrecioFact, 3);
                                                objCFDI_Dato.FechaYHoraTransaccion = dtNacFechaCompra.ToString("s") + "-" +
                                                                                     iDiferenciaHora.ToString("00") + ":00";

                                                #region VolumenDocumentado.
                                                CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                objVolumenDocumentado.ValorNumerico = Math.Round(dNacLitrosFact, 3);
                                                objVolumenDocumentado.UnidadDeMedida = sNacUnidMedida;

                                                objCFDI_Dato.VolumenDocumentado = objVolumenDocumentado;
                                                #endregion

                                                if (sNacClienteConsigacion.Equals("Y"))
                                                    objCFDI_Dato.Aclaracion = "ENTREGA CONTRATO PEMEX. CFDI emitido por el pago de la contraprestación por la prestación de servicios derivados de contrato Pemex";

                                                #region Version Antes de 2021-12-08.
                                                List<CVolJSonDTO.stCFDIDato> lstCfDIs = new List<CVolJSonDTO.stCFDIDato>();
                                                lstCfDIs.Add(objCFDI_Dato);

                                                lstNacional.Add(new CVolJSonDTO.stNacionalDato
                                                {
                                                    RfcClienteOProveedor = sNacRFCCliente,
                                                    NombreClienteOProveedor = sNacNombreCliente,
                                                    CFDIs = lstCfDIs
                                                });
                                                #endregion
                                                #endregion
                                            }
                                        #endregion

                                        objComplementoDato.Nacional = lstNacional;
                                        // <2021-12-14> se pone por Default "Expendio" para las entregas.
                                        objComplementoDato.TipoComplemento = "Expendio";//sTipoComplemento;
                                        lstComplementos.Add(objComplementoDato);
                                    }

                                    objEntMesDato.Complemento = lstComplementos;
                                    #endregion
                                    #endregion

                                    #region Resumen Mensual.
                                    dictMonthlySummary[vProd.ProductoID].Price = dPrecioEnt;
                                    dictMonthlySummary[vProd.ProductoID].SaleQuantity += dLitrosEnt;
                                    dictMonthlySummary[vProd.ProductoID].SaleAmount += dImporteEnt;
                                    #endregion
                                }
                            #endregion

                            objRepVolMenDato.Entregas = objEntMesDato;
                            #endregion

                            objProductoDato.ReporteDeVolumenMensual = objRepVolMenDato;

                            // Calculamos los totales del Resumen Diario.
                            dictMonthlySummary[vProd.ProductoID].TheoryInventoryQuantity = ((dictMonthlySummary[vProd.ProductoID].StartInventoryQuantity + dictMonthlySummary[vProd.ProductoID].InventoryInQuantity) - dictMonthlySummary[vProd.ProductoID].SaleQuantity);
                            dictMonthlySummary[vProd.ProductoID].InventoryDifference = dictMonthlySummary[vProd.ProductoID].EndInventoryQuantity - dictMonthlySummary[vProd.ProductoID].TheoryInventoryQuantity;
                            dictMonthlySummary[vProd.ProductoID].InventoryDifferencePercentage = dictMonthlySummary[vProd.ProductoID].InventoryDifference / dictMonthlySummary[vProd.ProductoID].SaleQuantity;
                            #endregion
                        }
                        else
                        {
                            #region Reporte para Autotanques y Ductos.

                            #endregion
                        }

                        #region Calculo Totales Finales.
                        // Cantidad de Tanques.
                        iTotalTanques = (from t in objContext.Tanks
                                         where t.StoreId == viNEstacion && t.Active == true
                                         select t.TankIdi).Count();

                        iTotalTanques += (from at in objContext.AutoTanques
                                          join s in objContext.Stores on viNEstacion equals s.StoreId
                                          where at.NumeroEstacion == s.StoreNumber.ToString()
                                          select at.NumeroAutoTanque).Count();

                        iTotalDispensarios = (from d in objContext.Dispensaries
                                              where d.StoreId == viNEstacion && d.Active == true
                                              select d.DispensaryIdi).Count();
                        break;
                        #endregion
                        #endregion
                }
                #endregion

                objCVolDatos.Producto.Add(objProductoDato);
                dictProductosExis.Add(sNProducto, oSubClaveProducto.ToString());
            }
            #endregion

            objCVolDatos.NumeroTanques = iTotalTanques;
            objCVolDatos.NumeroDispensarios = iTotalDispensarios;

            #region Datos: Cantidad de Ductos.
            iTotalDuctos = (from d in objContext.Ductos
                            join s in objContext.Stores on viNEstacion equals s.StoreId
                            where d.NumeroEstacion == s.StoreNumber.ToString()
                            select d.NumeroDucto).Count();
            #endregion

            objCVolDatos.NumeroDuctosEntradaSalida = iTotalDuctos;
            objCVolDatos.NumeroDuctosTransporteDistribucion = iTotalDuctos;

            #region Bitacora.
            List<CVolJSonDTO.stBitacoraDato> lstBitacora = new List<CVolJSonDTO.stBitacoraDato>();

            switch (viTipoReporte)
            {
                case CVolJSonDTO.eTipoReporte.Dia:
                    lstBitacora.Add(new CVolJSonDTO.stBitacoraDato
                    {
                        NumeroRegistro = 1,
                        FechaYHoraEvento = dtPerDateIni.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00",
                        TipoEvento = 5,
                        DescripcionEvento = "Generacion de Archivo JSon Diario",
                        UsuarioResponsable = "ADMIN"
                        //IdentificacionComponenteAlarma = ""
                    });

                    objCVolDatos.Bitacora = lstBitacora;
                    break;

                case CVolJSonDTO.eTipoReporte.Mes:
                    lstBitacora.Add(new CVolJSonDTO.stBitacoraDato
                    {
                        NumeroRegistro = 1,
                        FechaYHoraEvento = dtPerDateIni.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00",
                        TipoEvento = 5,
                        DescripcionEvento = "Generacion de Archivo JSon Mensual",
                        UsuarioResponsable = "ADMIN"
                        //IdentificacionComponenteAlarma = ""
                    });

                    objCVolDatos.BitacoraMensual = lstBitacora;
                    break;
            }

            if (lstBitacora.Count <= 0)
                return BadRequest("No se cuenta con registro para la Bitacora.");
            #endregion

            #region Nombre del Archivo.
            String sNombreArchivo = String.Empty,
                   sTipoArchivo = String.Empty;

            switch (viTipoReporte)
            {
                case CVolJSonDTO.eTipoReporte.Dia: sTipoArchivo = "D"; break;
                case CVolJSonDTO.eTipoReporte.Mes: sTipoArchivo = "M"; break;
            }
            sNombreArchivo += sTipoArchivo + "_";

            sNombreArchivo += Guid.NewGuid() + "_" +// IdentificadorEnvio
                              objCVolDatos.RfcContribuyente + "_" + // RfcCV
                              objCVolDatos.RfcProveedor + "_" + // RFCProveedor
                              dtPerDateEnd.ToString("yyyy-MM-dd") + "_" + // Periodo
                              objCVolDatos.ClaveInstalacion + "_" +// CveInstalacion
                              sClaveTipoReporte + "_"; // TipoReporte
            sNombreArchivo += "JSON";
            #endregion

            #region Generacion de Estructura JSON.
            String sEstructuraDatos = String.Empty;
            sEstructuraDatos = JsonConvert.SerializeObject(objCVolDatos,
                                                           Newtonsoft.Json.Formatting.Indented,
                                                           new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            sEstructuraDatos = sEstructuraDatos.Replace("LocalizacionYODescripcionTanque", "Localizaciony/oDescripcionTanque");
            #endregion

            #region Generación de Archivo.
            String sRutaArchivo = GenerarRutaCarpetaArchivo(viRutaCarpetaRaiz: sRutaCarpetaRaiz,
                                                         viNEstacion: iNStore.ToString(),
                                                         viTipoReporte: sTipoArchivo,
                                                         viFecha: dtPerDateIni,
                                                         viNombreArchivo: sNombreArchivo);

            System.IO.File.Delete(sRutaArchivo + ".json");
            System.IO.File.Delete(sRutaArchivo + ".zip");

            System.IO.File.WriteAllText(sRutaArchivo + ".json", sEstructuraDatos);
            //String sOutZip = CrearRarArchivo(sRutaArchivo + ".json", sRutaArchivo + ".zip");

            using (ZipArchive objCarpeta = ZipFile.Open(sRutaArchivo + ".zip", ZipArchiveMode.Create))
            {
                objCarpeta.CreateEntryFromFile(sRutaArchivo + ".json", Path.GetFileName(sRutaArchivo + ".json"));
            }

            #region Guardar informacion del Archivo Generado.
            // Guardamos los datos de generación en la tabla 
            var vExistArchivo = objContext.Volumetrics.Any(a =>
                                                           a.StoreId.Equals(viNEstacion) &&
                                                           a.TypeOfPeriod == sTipoArchivo &&
                                                           a.Date.Equals(dtPerDateIni));
            Volumetric objArchivoDatos = new Volumetric();

            if (!vExistArchivo)
            {
                #region Insert.
                objArchivoDatos.StoreId = viNEstacion.GetValueOrDefault();
                objArchivoDatos.TypeOfPeriod = sTipoArchivo;
                objArchivoDatos.Date = dtPerDateIni;
                objArchivoDatos.IsGenarated = 1;
                objArchivoDatos.IsSent = 0;
                objArchivoDatos.EnableSend = 1;
                objArchivoDatos.EnableGenerated = 1;
                objArchivoDatos.NameFile = sNombreArchivo;
                objArchivoDatos.Updated = DateTime.Now;
                objArchivoDatos.Active = true;
                objArchivoDatos.Locked = false;
                objArchivoDatos.Deleted = false;

                objContext.Volumetrics.Add(objArchivoDatos);
                #endregion
            }
            else
            {
                #region UpDate.
                objArchivoDatos = objContext.Volumetrics.FirstOrDefault(a =>
                                                                        a.StoreId.Equals(viNEstacion) &&
                                                                        a.TypeOfPeriod == sTipoArchivo &&
                                                                        a.Date.Equals(dtPerDateIni));
                objArchivoDatos.NameFile = sNombreArchivo;
                objArchivoDatos.Updated = DateTime.Now;
                objContext.Volumetrics.Update(objArchivoDatos);
                #endregion
            }
            #endregion
            #endregion

            #region Resumenes (Dario y Mensual).
            switch (viTipoReporte)
            {
                #region Tipo: Reporte Día.
                case CVolJSonDTO.eTipoReporte.Dia:
                    foreach (KeyValuePair<Guid, DailySummary> producto in dictDailySummary)
                    {
                        var vExistDia = objContext.DailySummaries.Any(d =>
                                                                      d.StoreId.Equals(producto.Value.StoreId) &&
                                                                      d.Date.Equals(producto.Value.Date) &&
                                                                      d.ProductId.Equals(producto.Value.ProductId));
                        if (!vExistDia)
                        {
                            #region Insert.
                            producto.Value.Updated = DateTime.Now;
                            producto.Value.Active = true;
                            producto.Value.Locked = false;
                            producto.Value.Deleted = false;

                            objContext.DailySummaries.Add(producto.Value);
                            #endregion
                        }
                        else
                        {
                            #region UpDate.
                            DailySummary objDiaDatoUPD = objContext.DailySummaries.FirstOrDefault(d =>
                                                                                                  d.StoreId.Equals(producto.Value.StoreId) &&
                                                                                                  d.Date.Equals(producto.Value.Date) &&
                                                                                                  d.ProductId.Equals(producto.Value.ProductId));
                            objDiaDatoUPD.Price = producto.Value.Price;
                            objDiaDatoUPD.StartInventoryQuantity = producto.Value.StartInventoryQuantity;
                            objDiaDatoUPD.InventoryInQuantity = producto.Value.InventoryInQuantity;
                            objDiaDatoUPD.SaleQuantity = producto.Value.SaleQuantity;
                            objDiaDatoUPD.SaleAmount = producto.Value.SaleAmount;
                            objDiaDatoUPD.TheoryInventoryQuantity = producto.Value.TheoryInventoryQuantity;
                            objDiaDatoUPD.EndInventoryQuantity = producto.Value.EndInventoryQuantity;
                            objDiaDatoUPD.InventoryDifference = producto.Value.InventoryDifference;
                            objDiaDatoUPD.InventoryDifferencePercentage = producto.Value.InventoryDifferencePercentage;
                            objDiaDatoUPD.SaleSample = producto.Value.SaleSample;
                            objDiaDatoUPD.Updated = DateTime.Now;

                            //objContext.DailySummaries.Update(producto.Value);
                            objContext.DailySummaries.Update(objDiaDatoUPD);
                            #endregion
                        }

                        var storeId2 = viNEstacion;
                        var usuarioId = obtenerUsuarioId();
                        var ipUser = obtenetIP();
                        var tableName = "Se genero Json Sat Diario";
                        await servicioBinnacle.AddBinnacle(usuarioId, ipUser, tableName, storeId2);

                        objContext.SaveChanges();
                    }
                    break;
                #endregion

                #region Tipo: Reporte Mensual.
                case CVolJSonDTO.eTipoReporte.Mes:
                    foreach (KeyValuePair<Guid, MonthlySummary> producto in dictMonthlySummary)
                    {
                        var vExistMes = objContext.MonthlySummaries.Any(m =>
                                                                        m.StoreId.Equals(producto.Value.StoreId) &&
                                                                        m.Date.Equals(producto.Value.Date) &&
                                                                        m.ProductId.Equals(producto.Value.ProductId));
                        if (!vExistMes)
                        {
                            #region Insert.
                            producto.Value.Updated = DateTime.Now;
                            producto.Value.Active = true;
                            producto.Value.Locked = false;
                            producto.Value.Deleted = false;

                            objContext.MonthlySummaries.Add(producto.Value);
                            #endregion
                        }
                        else
                        {
                            #region UpDate.
                            MonthlySummary objMesDatoUPD = objContext.MonthlySummaries.FirstOrDefault(m =>
                                                                                                      m.StoreId.Equals(producto.Value.StoreId) &&
                                                                                                      m.Date.Equals(producto.Value.Date) &&
                                                                                                      m.ProductId.Equals(producto.Value.ProductId));
                            objMesDatoUPD.Price = producto.Value.Price;
                            objMesDatoUPD.StartInventoryQuantity = producto.Value.StartInventoryQuantity;
                            objMesDatoUPD.InventoryInQuantity = producto.Value.InventoryInQuantity;
                            objMesDatoUPD.SaleQuantity = producto.Value.SaleQuantity;
                            objMesDatoUPD.SaleAmount = producto.Value.SaleAmount;
                            objMesDatoUPD.TheoryInventoryQuantity = producto.Value.TheoryInventoryQuantity;
                            objMesDatoUPD.EndInventoryQuantity = producto.Value.EndInventoryQuantity;
                            objMesDatoUPD.InventoryDifference = producto.Value.InventoryDifference;
                            objMesDatoUPD.InventoryDifferencePercentage = producto.Value.InventoryDifferencePercentage;
                            objMesDatoUPD.SaleSample = producto.Value.SaleSample;
                            objMesDatoUPD.Updated = DateTime.Now;

                            objContext.MonthlySummaries.Update(objMesDatoUPD);
                            #endregion
                        }

                        var storeId2 = viNEstacion;
                        var usuarioId = obtenerUsuarioId();
                        var ipUser = obtenetIP();
                        var tableName = "Se genero Json Sat Mensual";
                        await servicioBinnacle.AddBinnacle(usuarioId, ipUser, tableName, storeId2);
                        objContext.SaveChanges();
                    }
                    break;
                    #endregion

            }
            #endregion

            return Ok("OK|Archivo Generado");
        }

        private static String GenerarRutaCarpetaArchivo(String viRutaCarpetaRaiz, String viNEstacion, String viTipoReporte, DateTime viFecha, String viNombreArchivo)
        {
            String sUbicacionArchivo = String.Empty,
                   //sRuta =  viRutaCarpetaRaiz + "\\" + viNEstacion + "\\" + viTipoReporte + "\\" + viFecha.ToString("yyyy-MM-dd") + "\\";
                   sRuta = viRutaCarpetaRaiz + "\\" + viNEstacion + "\\" + viFecha.Year.ToString("0000") + "\\";

            switch (viFecha.Month)
            {
                case 1: sRuta += viFecha.Month.ToString("00") + " Enero\\"; break;
                case 2: sRuta += viFecha.Month.ToString("00") + " Febrero\\"; break;
                case 3: sRuta += viFecha.Month.ToString("00") + " Marzo\\"; break;
                case 4: sRuta += viFecha.Month.ToString("00") + " Abril\\"; break;
                case 5: sRuta += viFecha.Month.ToString("00") + " Mayo\\"; break;
                case 6: sRuta += viFecha.Month.ToString("00") + " Junio\\"; break;
                case 7: sRuta += viFecha.Month.ToString("00") + " Julio\\"; break;
                case 8: sRuta += viFecha.Month.ToString("00") + " Agosto\\"; break;
                case 9: sRuta += viFecha.Month.ToString("00") + " Septiembre\\"; break;
                case 10: sRuta += viFecha.Month.ToString("00") + " Octubre\\"; break;
                case 11: sRuta += viFecha.Month.ToString("00") + " Noviembre\\"; break;
                case 12: sRuta += viFecha.Month.ToString("00") + " Diciembre\\"; break;
            }

            sRuta += viTipoReporte + "\\";

            switch (viTipoReporte.ToUpper())
            {
                case "D": sRuta += viFecha.ToString("yyyy-MM-dd") + "\\"; break;
            }

            if (!Directory.Exists(sRuta))
                Directory.CreateDirectory(sRuta);

            sUbicacionArchivo = Path.Combine(sRuta, viNombreArchivo);

            return sUbicacionArchivo;
        }

    }
}