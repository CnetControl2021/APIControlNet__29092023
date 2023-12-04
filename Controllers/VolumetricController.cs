using APIControlNet.DTOs;
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
using System.Text.RegularExpressions;

using Excel;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web;

namespace APIControlNet.Controllers
{
    // =====  VERSION  =====
    // $@m&: 2023-12-04 17:49
    // =====================

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VolumetricController : CustomBaseController
    {
        private readonly CnetCoreContext objContext;
        private readonly IMapper objMapper;
        private readonly ServicioBinnacle objServicioBinnacle;

        #region CVol: JSON.
        public VolumetricController(CnetCoreContext viContext, IMapper viMapper, ServicioBinnacle viServicioBinnacle)
        {
            this.objContext = viContext;
            this.objMapper = viMapper;
            this.objServicioBinnacle = viServicioBinnacle;
        }

        [AllowAnonymous]
        [HttpGet(Name = "CVolJSon")]
        public async Task<IActionResult> GenerarCVolJSon(Guid? viNCompania, Guid? viNEstacion, CVolJSonDTO.eTipoReporte viTipoReporte, Boolean viReporteEstacion, DateTime viFecha, CVolJSonDTO.eProcesoArchivo viProcesoArchivo, Boolean viGenerarCompTransporte, Boolean viGenCompRecepcion, Boolean viGenComEntrega)
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

            switch (viTipoReporte) {
                #region TipoReporte: Diario.
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

                #region TipoReporte: Mensual.
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
            if (viProcesoArchivo == CVolJSonDTO.eProcesoArchivo.Guardar)
            {
                var vQSPath = (from s in objContext.Settings
                               where s.StoreId == viNEstacion &&
                                     s.Field == "setting_volumetric_path"
                               select new { Path = (s.Value ?? "") });

                if (vQSPath != null)
                {
                    foreach (var vPathDato in vQSPath)
                        sRutaCarpetaRaiz = vPathDato.Path;

                    if (String.IsNullOrEmpty(sRutaCarpetaRaiz))
                        return BadRequest("No se encontro la ruta de la carpeta de alojamiento en la configuración.");
                }
                else
                    return BadRequest("No se encontro la ruta de la carpeta de alojamiento en la configuración.");
            }
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
            String sCaracterValor = CVolJSonDTO.CARACTER_PERMISIONARIO, // <*DUDA*> Lugar de donde obtenerlo
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
                                return BadRequest(iNStore + "|RfcSystemSupplier");
                            else
                                objCVolDatos.RfcProveedor = sRfcProveedor;

                            if (String.IsNullOrEmpty(sNomClaveInstalacion))
                                return BadRequest(iNStore + "|JsonClaveInstalacionId");

                            if (String.IsNullOrEmpty(oModalidadPermiso.ToString()))
                                return BadRequest(iNStore + "|CrePermission");

                            if (String.IsNullOrEmpty(oNumPermiso.ToString()))
                                return BadRequest(iNStore + "|SatPermission");

                            if (String.IsNullOrEmpty(sClaveTipoReporte))
                                return BadRequest(iNStore + "|JsonTipoReporteId");

                            if (String.IsNullOrEmpty(sDescripInstalacion))
                                return BadRequest(iNStore + "|SystemDescriptionInstallation");

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
            Dictionary<String, String> dictProductosExis = new Dictionary<string, string>();
            dictProductosExis.Clear();

            objCVolDatos.Producto = new List<CVolJSonDTO.stProductoDato>();
            List<int> lstDispensariosExist = new List<int>();

            #region Consulta: Producto Datos.
            var vQProductos = (from th in objContext.SaleOrders
                               join td in objContext.SaleSuborders on th.SaleOrderId equals td.SaleOrderId
                               join p in objContext.Products on td.ProductId equals p.ProductId
                               join ps in objContext.ProductSats on new { f1 = th.StoreId, f2 = td.ProductId } equals new { f1 = ps.StoreId, f2 = ps.ProductId }
                               where th.StoreId == viNEstacion &&
                                     th.StartDate >= dtPerDateIni && th.StartDate <= dtPerDateEnd
                                     && p.IsFuel == true
                               group p by new { p.ProductId, p.ProductCode, p.Name, ps.SatProductKey, ps.SatProductSubkey, p.JsonClaveUnidadMedidaId, ps.ComposOctanajeGasolina, ps.SatWithFossil, ps.SatPercentageWithFossil, ps.TurbosinaConCombustibleNoFosil, ps.ComposDeCombustibleNoFosilEnTurbosina, ps.ComposDePropanoEnGasLp, ps.ComposDeButanoEnGasLp, ps.DensidadDePetroleo, ps.ComposDeAzufreEnPetroleo } into ProdDatos
                               select new
                               {
                                   ProductoID = ProdDatos.Key.ProductId,
                                   ProductCode = ProdDatos.Key.ProductCode,
                                   ProductName = ProdDatos.Key.Name,
                                   ClaveProducto = (ProdDatos.Key.SatProductKey ?? ""),
                                   ClaveSubProducto = (ProdDatos.Key.SatProductSubkey ?? ""),
                                   UnidadMedida = (ProdDatos.Key.JsonClaveUnidadMedidaId ?? "UM03"),
                                   ComposOctanajeGasolina = (ProdDatos.Key.ComposOctanajeGasolina ?? 0),
                                   ConFosil = (ProdDatos.Key.SatWithFossil ?? 0),
                                   PorcentajeConFosil = (ProdDatos.Key.SatPercentageWithFossil ?? 0),
                                   TurbosinaConCombustibleNoFosil = (ProdDatos.Key.TurbosinaConCombustibleNoFosil ?? "0"),
                                   ComposDeCombustibleNoFosilEnTurbosina = (ProdDatos.Key.ComposDeCombustibleNoFosilEnTurbosina ?? 0),
                                   ComposDePropanoEnGasLP = (ProdDatos.Key.ComposDePropanoEnGasLp ?? 0),
                                   ComposDeButanoEnGasLP = (ProdDatos.Key.ComposDeButanoEnGasLp ?? 0),
                                   DensidadDePetroleo = (ProdDatos.Key.DensidadDePetroleo ?? 0),
                                   ComposDeAzufreEnPetroleo = (ProdDatos.Key.ComposDeAzufreEnPetroleo ?? 0)
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
                Int32 iConFosil = vProd.ConFosil,
                      iPorcentajeFosil = vProd.PorcentajeConFosil,
                      iTurboConFosil = Convert.ToInt32(vProd.TurbosinaConCombustibleNoFosil.ToString()),
                      iTurboPorcenajeFosil = vProd.ComposDeCombustibleNoFosilEnTurbosina;
                Decimal dPorcPropano = vProd.ComposDePropanoEnGasLP,
                        dPorcButano = vProd.ComposDeButanoEnGasLP,
                        dPetroleoDensidad = vProd.DensidadDePetroleo,
                        dPetrolePorcenAzufre = vProd.DensidadDePetroleo;
                #endregion

                #region Producto: Validamos datos.
                if (String.IsNullOrEmpty(sClaveProducto))
                    return BadRequest("El producto '" + sNProducto + "' no contiene la Clave (Prod_SAT)");

                if (String.IsNullOrEmpty(vProd.ClaveSubProducto))
                    return BadRequest("El producto '" + sNProducto + "' no contiene SubClave (Prod_SAT)");

                if ((from c in objContext.JsonClaveProductos where c.JsonClaveProductoId == sClaveProducto.Trim() select c.JsonClaveProductoId).Count() <= 0)
                    return BadRequest("La ClaveSat '" + sClaveProducto + "' del Producto '" + sNProducto + "' no es valida.");

                if ((from sc in objContext.JsonSubclaveProductos where sc.JsonSubclaveProductoId == vProd.ClaveSubProducto.Trim() select sc.JsonSubclaveProductoId).Count() <= 0)
                    return BadRequest("La SubClaveSat '" + vProd.ClaveSubProducto + "' del Producto '" + sNProducto + "' no es valida.");

                oSubClaveProducto = vProd.ClaveSubProducto.ToString();
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

                    #region ClaveProducto: PR12 (GAS LP)
                    case "PR12":
                        if (dPorcPropano <= 0 || dPorcPropano > 99)
                            return BadRequest("El producto '" + sNProducto + "' tiene un Porcentaje de Propano MENOR a 0.1 o MAYOR a 99 (ProductSat)");

                        if (dPorcButano <= 0 || dPorcButano > 99)
                            return BadRequest("El producto '" + sNProducto + "' tiene un Porcentaje de Butano MENOR a 0.1 o MAYOR a 99 (ProductSat)");

                        objProductoDato.ComposDePropanoEnGasLP = Math.Round(dPorcPropano, 2);
                        objProductoDato.ComposDeButanoEnGasLP = Math.Round(dPorcButano, 2);
                        break;
                    #endregion

                    #region ClaveProducto: PR08 (Petroleo)
                    case "PR08":
                        if (dPetroleoDensidad <= 0 || dPetroleoDensidad > 80)
                            return BadRequest("El producto '" + sNProducto + "' tiene un Densidad MENOR a 0.1 o MAYOR a 80 (ProductSat)");

                        if (dPetrolePorcenAzufre <= 0 || dPetrolePorcenAzufre > 10)
                            return BadRequest("El producto '" + sNProducto + "' tiene un Porcentaje de Azufre MENOR a 0.1 o MAYOR a 10 (ProducSat)");

                        objProductoDato.DensidadDePetroleo = Math.Round(dPetroleoDensidad, 1);
                        objProductoDato.ComposDeAzufreEnPetroleo = Math.Round(dPetrolePorcenAzufre, 1);
                        break;
                        #endregion
                }

                #region Composición del Producto.
                //if(sCaracterValor.Equals(CVolJSonDTO.CARACTER_CONTRATISTA) || sCaracterValor.Equals(CVolJSonDTO.CARACTER_ASIGNATARIO))
                if (sClaveProducto.Equals("PR09") || sClaveProducto.Equals("PR10"))
                {
                    List<CVolJSonDTO.stGasNaturalOCondensadosDatos> lstGasComponentes = new List<CVolJSonDTO.stGasNaturalOCondensadosDatos>();

                    #region Consulta: Composición del Producto.
                    var vQComposicionProd = (from c in objContext.ProductCompositions
                                             where c.ProductId == vProd.ProductoID//Guid.Parse("7ECC6829-BDEA-4882-8939-DF65EFE9B6C2")
                                             select new
                                             {
                                                 TipoCompuesto = c.JsonTipoComposicionId,
                                                 FraccionMolar = c.MolarFraction,
                                                 PoderCalorifico = c.CalorificPower
                                             });
                    #endregion

                    #region Lectura: Composición del Producto.
                    if (vQComposicionProd != null)
                    {
                        foreach (var vComposicionDatos in vQComposicionProd)
                        {
                            #region Composición del Producto: Asignamos Valores.
                            Decimal dFraccionMolar = vComposicionDatos.FraccionMolar.GetValueOrDefault(),
                                    dPoderCalorifico = vComposicionDatos.PoderCalorifico.GetValueOrDefault();
                            String sTipoCompuesto = vComposicionDatos.TipoCompuesto;
                            #endregion

                            #region Composición del Producto: Validamos Valores.
                            if (String.IsNullOrEmpty(sTipoCompuesto))
                                return BadRequest("El producto '" + sNProducto + "' no tiene Tipo de Compuesto (ProductComposition).");

                            if (dFraccionMolar >= 1)
                                return BadRequest("El producto '" + sNProducto + "' tiene Fraccion Molar MAYOR a 0.999 (ProductComposition).");

                            if (dPoderCalorifico <= 0 || dPoderCalorifico > 1500000)
                                return BadRequest("El producto '" + sNProducto + "' tiene Poder Calorifico MENOR a 0.001 o MAYOR a 1500000 (ProductComposition).");
                            #endregion

                            #region Composición del Producto: Llenado de Estructura.
                            lstGasComponentes.Add(new CVolJSonDTO.stGasNaturalOCondensadosDatos
                            {
                                ComposGasNaturalOCondensados = sTipoCompuesto,
                                FraccionMolar = Math.Round(dFraccionMolar, 3),
                                PoderCalorifico = Math.Round(dPoderCalorifico, 3)
                            });
                            #endregion
                        }
                    }
                    else
                        return BadRequest("El Producto '" + sNProducto + "' no contiene definidos sus compuestos. (Compuesto, FraccionMolar, PoderCalorifico)");
                    #endregion

                    objProductoDato.GasNaturalOCondensados = lstGasComponentes;
                }
                #endregion

                if (objTipoDistribucion == CVolJSonDTO.eTipoDistribucion.Ductos)
                {
                    List<CVolJSonDTO.stDuctoDatos> lstDuctosNew = new List<CVolJSonDTO.stDuctoDatos>();
                    objProductoDato.Ducto = lstDuctosNew;
                }
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

                        #region JSON Día: Estación.
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
                                    String sTanqueUniMed = vProd.UnidadMedida,//"UM03",
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

                                    if (bCompRecep)
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
                                            if (bCompRecep)
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

                                            if (bCompEntregas)
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
                        #endregion

                        #region JSON Día: Autotanques y Ductos.
                        else
                        {
                            #region Consulta: Total de Recepciones y Entregas del Producto.
                            var vQTRecepEntProd = (from p in objContext.ProductStores
                                                   where p.StoreId == viNEstacion &&
                                                         p.ProductId == vProd.ProductoID
                                                   select new
                                                   {
                                                       idProducto = p.ProductId,
                                                       TotalRecepciones = (from r in objContext.InventoryIns
                                                                           where r.StoreId == p.StoreId &&
                                                                                 r.ProductId == p.ProductId &&
                                                                                 r.EndDate >= dtPerDateIni &&
                                                                                 r.EndDate <= dtPerDateEnd
                                                                           select r.InventoryInId).Count(),
                                                       TotalEntregas = (from eh in objContext.SaleOrders
                                                                        join ed in objContext.SaleSuborders on eh.SaleOrderId equals ed.SaleOrderId
                                                                        where eh.StoreId == p.StoreId &&
                                                                              eh.Date >= dtPerDateIni &&
                                                                              eh.Date <= dtPerDateEnd &&
                                                                              ed.ProductId == p.ProductId
                                                                        select eh.SaleOrderId).Count()
                                                       //TotalEntregas = (from eh in objContext.InventoryInSaleOrders // <*DUDA*>
                                                       //                 join ed in objContext.SaleSuborders on eh.InventoryInSaleOrderId equals ed.SaleOrderId
                                                       //                 where eh.StoreId == p.StoreId &&
                                                       //                       eh.ProductId == p.ProductId &&
                                                       //                       eh.EndDate >= dtPerDateIni &&
                                                       //                       eh.EndDate <= dtPerDateEnd
                                                       //                 select eh.InventoryInNumber).Count()
                                                   });
                            #endregion

                            #region Lectura: Total de Recepciones y Entregas del Producto.
                            if (vQTRecepEntProd != null)
                                foreach (var vRecepEntDatos in vQTRecepEntProd)
                                {
                                    #region Recepciones y Entregas: Asignamos Valores.
                                    int iTotalRecepProd = vRecepEntDatos.TotalRecepciones,
                                        iTotalEntProd = vRecepEntDatos.TotalEntregas;
                                    #endregion

                                    if (iTotalRecepProd > 0 || iTotalEntProd > 0)
                                    {
                                        List<CVolJSonDTO.stDuctoDatos> lstProdDuctos = new List<CVolJSonDTO.stDuctoDatos>();
                                        List<CVolJSonDTO.stTanqueDatos> lstProdAutoTanques = new List<CVolJSonDTO.stTanqueDatos>();

                                        #region Recepciones: Ducto y AutoTanque.
                                        if (iTotalRecepProd > 0)
                                        {
                                            #region Consulta: Recepcion Datos.
                                            var vQRecepDato = (from r in objContext.InventoryIns
                                                               join d in objContext.Tanks on new { f1 = r.StoreId, f2 = r.TankIdi } equals new { f1 = d.StoreId, f2 = d.TankIdi }
                                                               join rd in objContext.InventoryInDocuments on r.InventoryInId equals rd.InventoryInId
                                                               where r.StoreId == viNEstacion &&
                                                                     r.EndDate >= dtPerDateIni && r.EndDate <= dtPerDateEnd &&
                                                                     r.ProductId == vProd.ProductoID
                                                               select new
                                                               {
                                                                   GNumeroRecepcion = r.InventoryInId,
                                                                   TipoDucto = d.SatTypeMediumStorage,
                                                                   TipoTanqueJS = d.SatTankType,
                                                                   NumeroDucto = r.TankIdi,
                                                                   NumeroRecepcion = r.InventoryInNumber,
                                                                   VolumenPuntoEntrada = r.StartVolume,
                                                                   VolumenRecepcion = r.Volume,
                                                                   VolumenFinal = r.EndVolume,
                                                                   Temperatura = r.EndTemperature,
                                                                   PresionAbsoluta = r.AbsolutePressure,
                                                                   FechaYHoraInicioRecepcion = r.StartDate,
                                                                   FechaYHoraFinalRecepcion = r.EndDate,
                                                                   Precio = rd.Price,
                                                                   Importe = rd.Amount,
                                                                   TransporteID = rd.SupplierTransportRegisterId,
                                                                   PedimentoID = rd.PetitionCustomsId
                                                               });
                                            #endregion

                                            #region Lectura: Recepciones.
                                            if (vQRecepDato != null)
                                                foreach (var vRecepDatos in vQRecepDato)
                                                {
                                                    #region Recepcion: Asignamos Valores.
                                                    int iRecepNTanque = vRecepDatos.NumeroDucto,
                                                        iNRecepcion = vRecepDatos.NumeroRecepcion.GetValueOrDefault();
                                                    Decimal dRecepVolPuntoEntrada = vRecepDatos.VolumenPuntoEntrada.GetValueOrDefault(),
                                                            dRecepVolRecepcion = vRecepDatos.VolumenRecepcion.GetValueOrDefault(),
                                                            dRecepVolFinal = vRecepDatos.VolumenFinal.GetValueOrDefault(),
                                                            dRecepTemperatura = vRecepDatos.Temperatura.GetValueOrDefault(),
                                                            dRecepPresionAbs = vRecepDatos.PresionAbsoluta.GetValueOrDefault(),
                                                            dRecepPrecio = vRecepDatos.Precio.GetValueOrDefault(),
                                                            dRecepImporte = vRecepDatos.Importe.GetValueOrDefault();
                                                    DateTime dtRecepFIni = Convert.ToDateTime(vRecepDatos.FechaYHoraInicioRecepcion),
                                                             dtRecepFFin = Convert.ToDateTime(vRecepDatos.FechaYHoraFinalRecepcion);
                                                    String sTipoMedicion = vRecepDatos.TipoDucto.Trim(),
                                                           sRecepUniMed = vProd.UnidadMedida;
                                                    Guid gNRecepcionID = Guid.Parse(vRecepDatos.GNumeroRecepcion.ToString()),
                                                         gTransporteID = Guid.Parse(vRecepDatos.TransporteID.ToString()),
                                                         gPedimentoID = Guid.Parse(vRecepDatos.PedimentoID.ToString());
                                                    #endregion

                                                    // Tipo: Ductos | AutoTanques
                                                    switch (objTipoDistribucion)
                                                    {
                                                        #region Ductos.
                                                        case CVolJSonDTO.eTipoDistribucion.Ductos:
                                                            if (String.IsNullOrEmpty(sTipoMedicion))
                                                                return BadRequest("El Ducto '" + iRecepNTanque + "' no contiene la Tipo (tb:Tank)");

                                                            String sClaveDucto = "DUC-" + sTipoMedicion + "-" + iRecepNTanque.ToString("0000");

                                                            #region Datos Ducto.
                                                            int iIdxDucto = -1;
                                                            iIdxDucto = lstProdDuctos.FindIndex(d => d.ClaveIdentificacionDucto.Equals(sClaveDucto));

                                                            // Al no existir datos del Ducto consultamos la informacion par agregarla
                                                            if (iIdxDucto < 0)
                                                            {
                                                                #region Consulta: Ducto Datos.
                                                                var vQDuctoDatos = (from d in objContext.Tanks
                                                                                    where d.StoreId == viNEstacion &&
                                                                                          d.TankIdi == iRecepNTanque
                                                                                    select new
                                                                                    {
                                                                                        NumeroDucto = d.TankIdi,
                                                                                        ClaveSistemaMedicion = d.SatTypeMediumStorage,
                                                                                        Descripcion = d.Name,
                                                                                        Diametro = d.DiameterOrWidth,
                                                                                        VigenciaCalibracion = d.SatDateCalibration,
                                                                                        IncertidumbreMedicion = (d.SatPercentageUncertaintyMeasurement ?? 0),
                                                                                        UnidadMedida = "UM03",
                                                                                        Volumen = 0
                                                                                    });
                                                                #endregion

                                                                #region Lectura: Ducto Datos.
                                                                if (vQDuctoDatos != null)
                                                                    foreach (var vDuctoDatos in vQDuctoDatos)
                                                                    {
                                                                        #region Ducto Datos: Asignamos Valores.
                                                                        int iDuctoNMedidor = vDuctoDatos.NumeroDucto;
                                                                        Decimal dDuctoMedIncertidumbre = vDuctoDatos.IncertidumbreMedicion;
                                                                        DateTime dtDuctoMedVigencia = Convert.ToDateTime(vDuctoDatos.VigenciaCalibracion);
                                                                        String sDuctoMedClave = vDuctoDatos.ClaveSistemaMedicion,
                                                                               sDuctoDescripcion = vDuctoDatos.Descripcion,
                                                                               sDuctoUnidadMedida = vDuctoDatos.UnidadMedida;
                                                                        Decimal dDuctoDiametro = vDuctoDatos.Diametro.GetValueOrDefault();
                                                                        //dDuctoVolumen = vDuctoDatos.Volumen;
                                                                        #endregion

                                                                        #region Ducto Datos: Validamos Valores.
                                                                        if (String.IsNullOrEmpty(sDuctoDescripcion))
                                                                            return BadRequest("El Ducto '" + iRecepNTanque + "' no contiene la Descripción (tb:Tank)");
                                                                        else if (sDuctoDescripcion.Length < 2 || sDuctoDescripcion.Length > 250)
                                                                            return BadRequest("El Ducto '" + iRecepNTanque + "' contiene una Descripción menor a 2 o mayor a 250 caracteres (tb:Tank)");

                                                                        if (dDuctoDiametro < 1 || dDuctoDiametro > 100)
                                                                            return BadRequest("El Ducto '" + iRecepNTanque + "' contiene un diametro menor a 1 o mayor a 100 (tb:Tank)");
                                                                        #endregion

                                                                        #region Ducto Medidor Datos: Llenado de Estructura.
                                                                        List<CVolJSonDTO.stDuctoMedicionDato> lstDuctMedidores = new List<CVolJSonDTO.stDuctoMedicionDato>();

                                                                        lstDuctMedidores.Add(new CVolJSonDTO.stDuctoMedicionDato
                                                                        {
                                                                            SistemaMedicionDucto = "SMD-DUC-" + sDuctoMedClave + "-" + iDuctoNMedidor.ToString("0000"),
                                                                            LocalizODescripSistMedicionDucto = sDuctoDescripcion,
                                                                            IncertidumbreMedicionSistMedicionDucto = Math.Round(dDuctoMedIncertidumbre, 3),
                                                                            VigenciaCalibracionSistMedicionDucto = dtDuctoMedVigencia.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00",
                                                                        });

                                                                        if (lstDuctMedidores.Count < 0)
                                                                            return BadRequest("El Ducto '" + iRecepNTanque + "' no contiene medidores.");
                                                                        #endregion

                                                                        CVolJSonDTO.stRecepcionDato objRecepRecepciones = new CVolJSonDTO.stRecepcionDato();
                                                                        CVolJSonDTO.stEntregaDato objRecepEntrega = new CVolJSonDTO.stEntregaDato();

                                                                        lstProdDuctos.Add(new CVolJSonDTO.stDuctoDatos
                                                                        {
                                                                            ClaveIdentificacionDucto = sClaveDucto,
                                                                            DescripcionDucto = sDuctoDescripcion,
                                                                            DiametroDucto = Math.Round(dDuctoDiametro, 3),
                                                                            Medidores = lstDuctMedidores,
                                                                            Recepciones = objRecepRecepciones,
                                                                            ENTREGAS = objRecepEntrega
                                                                        });
                                                                        iTotalDuctos++;
                                                                    }
                                                                #endregion

                                                                iIdxDucto = lstProdDuctos.FindIndex(d => d.ClaveIdentificacionDucto.Equals(sClaveDucto));
                                                            }
                                                            #endregion

                                                            #region Agregamos la Recepcion.
                                                            List<CVolJSonDTO.stDuctoRecepcionIndividualDato> lstDuctoRecepciones = new List<CVolJSonDTO.stDuctoRecepcionIndividualDato>();

                                                            CVolJSonDTO.stDuctoDatos objProdDucto = lstProdDuctos[iIdxDucto];
                                                            CVolJSonDTO.stRecepcionDato objProdDucRecepciones = objProdDucto.Recepciones;
                                                            objProdDucRecepciones.TotalRecepciones++;
                                                            objProdDucRecepciones.TotalDocumentos++;

                                                            if (objProdDucRecepciones.SumaCompras == null)
                                                                objProdDucRecepciones.SumaCompras = dRecepImporte;
                                                            else
                                                                objProdDucRecepciones.SumaCompras = dRecepImporte + (Decimal)objProdDucRecepciones.SumaCompras;

                                                            #region Suma Volumen Recepcion.
                                                            CVolJSonDTO.stVolumenDato objSumVolRecepcionDato = new CVolJSonDTO.stVolumenDato();
                                                            if (objProdDucRecepciones.SumaVolumenRecepcion == null)
                                                            {
                                                                objSumVolRecepcionDato.UnidadDeMedida = "UM03";
                                                                objSumVolRecepcionDato.ValorNumerico = dRecepVolRecepcion;
                                                            }
                                                            else
                                                            {
                                                                objSumVolRecepcionDato = (CVolJSonDTO.stVolumenDato)objProdDucRecepciones.SumaVolumenRecepcion;
                                                                objSumVolRecepcionDato.ValorNumerico = dRecepVolRecepcion + (Decimal)objSumVolRecepcionDato.ValorNumerico;
                                                            }

                                                            objProdDucRecepciones.SumaVolumenRecepcion = objSumVolRecepcionDato;
                                                            #endregion

                                                            if (objProdDucRecepciones.Recepcion == null)
                                                                lstDuctoRecepciones = new List<CVolJSonDTO.stDuctoRecepcionIndividualDato>();
                                                            else
                                                                lstDuctoRecepciones = (List<CVolJSonDTO.stDuctoRecepcionIndividualDato>)objProdDucRecepciones.Recepcion;

                                                            #region Volumen Punto Entrada.
                                                            CVolJSonDTO.stVolumenDato objRecepVolPuntoEnt = new CVolJSonDTO.stVolumenDato();
                                                            objRecepVolPuntoEnt.UnidadDeMedida = "UM03";
                                                            objRecepVolPuntoEnt.ValorNumerico = Math.Round(dRecepVolPuntoEntrada, 3);
                                                            #endregion

                                                            #region Volumen Recepcion.
                                                            CVolJSonDTO.stVolumenDato objRecepVolRecepcion = new CVolJSonDTO.stVolumenDato();
                                                            objRecepVolRecepcion.UnidadDeMedida = "UM03";
                                                            objRecepVolRecepcion.ValorNumerico = Math.Round(dRecepVolRecepcion, 3);
                                                            #endregion

                                                            lstDuctoRecepciones.Add(new CVolJSonDTO.stDuctoRecepcionIndividualDato
                                                            {
                                                                NumeroDeRegistro = iNRecepcion,
                                                                VolumenPuntoEntrada = objRecepVolPuntoEnt,
                                                                VolumenRecepcion = objRecepVolRecepcion,
                                                                Temperatura = Math.Round(dRecepTemperatura, 3),
                                                                PresionAbsoluta = Math.Round(dRecepPresionAbs, 3),
                                                                FechaYHoraInicioRecepcion = dtRecepFIni.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00",
                                                                FechaYHoraFinalRecepcion = dtRecepFFin.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00"
                                                            });
                                                            #endregion

                                                            if (lstDuctoRecepciones.Count > 0)
                                                                objProdDucRecepciones.Recepcion = lstDuctoRecepciones;

                                                            objProdDucto.Recepciones = objProdDucRecepciones;
                                                            lstProdDuctos[iIdxDucto] = objProdDucto;

                                                            objProductoDato.Ducto = lstProdDuctos;
                                                            break;
                                                        #endregion

                                                        #region AutoTanque.
                                                        case CVolJSonDTO.eTipoDistribucion.Autotanques:
                                                            CVolJSonDTO.stTanqueDatos objAutoTanqueDatos;

                                                            if (String.IsNullOrEmpty(sTipoMedicion))
                                                                return BadRequest("El AutoTanque '" + iRecepNTanque.ToString() + "' no contiene la Tipo de Medición (tb:Tank)");

                                                            String sClaveAutoTanq = sTipoMedicion + "-" + sNomClaveInstalacion + "-" + iRecepNTanque.ToString("0000");

                                                            #region AutoTanque Datos.
                                                            int iIdxAutoTanq = -1;
                                                            iIdxAutoTanq = lstProdAutoTanques.FindIndex(d => d.ClaveIdentificacionTanque.Equals(sClaveAutoTanq));

                                                            // Al no existir datos del Ducto consultamos la informacion par agregarla
                                                            if (iIdxAutoTanq < 0)
                                                            {
                                                                #region Agregamos los datos del AutoTanque.
                                                                #region Consulta: AutoTanque Datos.
                                                                var vQAutoTanqueDatos = (from a in objContext.Tanks
                                                                                         where a.StoreId == viNEstacion &&
                                                                                               a.TankIdi == iRecepNTanque
                                                                                         //&& a.TankTypeId == "" //<*DUDA*> hasta que se defina el código de tipo de tanque "AutoTanque"
                                                                                         select new
                                                                                         {
                                                                                             TipoTanque = a.SatTankType,
                                                                                             Descripcion = a.Name,
                                                                                             UnidadMedida = "UM03",
                                                                                             CapacidadTotal = a.CapacityTotal,
                                                                                             CapacidadOperativa = a.CapacityOperational,
                                                                                             CapacidadUtil = a.CapacityUseful,
                                                                                             CapacidadFondaje = a.Fondage,
                                                                                             CapacidadGasTalon = a.CapacityGastalon,
                                                                                             VolumenMinimoOperacion = a.CapacityMinimumOperating,
                                                                                             EstadoTanque = a.Active,
                                                                                             TipoSistMedicion = a.SatTypeMeasurement,
                                                                                             LocalizODescripSistMedicion = a.SatDescriptionMeasurement,
                                                                                             IncertidumbreMedicionSistMedicion = a.SatPercentageUncertaintyMeasurement,
                                                                                             TipoMedioAlmacenamiento = a.SatTypeMediumStorage,
                                                                                             VigenciaCalibracionSistMedicion = a.SatDateCalibration,
                                                                                             RecepcionesAnt = (from r in objContext.InventoryIns
                                                                                                               where r.StoreId == viNEstacion &&
                                                                                                                     r.TankIdi == a.TankIdi &&
                                                                                                                     r.EndDate >= dtPerDateIni.AddDays(-1) &&
                                                                                                                     r.EndDate <= dtPerDateEnd.AddDays(-1) &&
                                                                                                                     r.ProductId == vProd.ProductoID
                                                                                                               select new { r.Volume }).Sum(r => r.Volume),
                                                                                             EntregasAnt = (from e in objContext.SaleOrders
                                                                                                            join ed in objContext.SaleSuborders on e.SaleOrderId equals ed.SaleOrderId
                                                                                                            where e.StoreId == viNEstacion &&
                                                                                                                  e.TankIdi == a.TankIdi &&
                                                                                                                  e.Date >= dtPerDateIni.AddDays(-1) &&
                                                                                                                  e.Date <= dtPerDateEnd.AddDays(-1) &&
                                                                                                                  ed.ProductId == vProd.ProductoID
                                                                                                            select new { ed.Quantity }).Sum(r => r.Quantity)
                                                                                         });
                                                                #endregion

                                                                #region Lectura: AutoTanque Datos.
                                                                if (vQAutoTanqueDatos != null)
                                                                    foreach (var vAutoTanqueDatos in vQAutoTanqueDatos)
                                                                    {
                                                                        #region AutoTanque: Asignamos Valores.
                                                                        int iCapTotal = vAutoTanqueDatos.CapacidadTotal.GetValueOrDefault(),
                                                                            iCapOperativa = vAutoTanqueDatos.CapacidadOperativa.GetValueOrDefault(),
                                                                            iCapUtil = vAutoTanqueDatos.CapacidadUtil.GetValueOrDefault(),
                                                                            iCapFondaje = vAutoTanqueDatos.CapacidadFondaje.GetValueOrDefault(),
                                                                            iVolMinOperacion = vAutoTanqueDatos.VolumenMinimoOperacion.GetValueOrDefault(),
                                                                            iTanqueIncertidumbre = vAutoTanqueDatos.IncertidumbreMedicionSistMedicion.GetValueOrDefault();
                                                                        String sDescripcionSAT = vAutoTanqueDatos.Descripcion;
                                                                        DateTime dtTanqueFCalibracion = Convert.ToDateTime(vAutoTanqueDatos.VigenciaCalibracionSistMedicion);
                                                                        String sTipoAutoTanq = vAutoTanqueDatos.TipoTanque,
                                                                               sTanqueUniMed = vAutoTanqueDatos.UnidadMedida,
                                                                               sTanqueTipoMedicion = vAutoTanqueDatos.TipoSistMedicion,
                                                                               sTanqueDescMedicion = vAutoTanqueDatos.LocalizODescripSistMedicion,
                                                                               sTanqueTipoMedAlmacenamiento = vAutoTanqueDatos.TipoMedioAlmacenamiento;
                                                                        Boolean sTanqueStatus = vAutoTanqueDatos.EstadoTanque.GetValueOrDefault();
                                                                        Decimal dRecepcionesAutTanqAnt = vAutoTanqueDatos.RecepcionesAnt.GetValueOrDefault(),
                                                                                dEntregasAutTanqAnt = vAutoTanqueDatos.EntregasAnt.GetValueOrDefault();
                                                                        #endregion

                                                                        #region AutoTanque: Validamos Valores.
                                                                        if (String.IsNullOrEmpty(sDescripcionSAT))
                                                                            return BadRequest("El AutoTanque no contiene la Descripción");

                                                                        if (String.IsNullOrEmpty(sTanqueTipoMedicion))
                                                                            return BadRequest("El AutoTanque no contiene un Tipo de Medición");

                                                                        if (String.IsNullOrEmpty(sTanqueTipoMedAlmacenamiento))
                                                                            return BadRequest("El AutoTanque no contiene un Tipo de Medio de Almacenamiento");
                                                                        else if (sTanqueTipoMedAlmacenamiento.Equals("0"))
                                                                            return BadRequest("El AutoTanque contiene un Tipo de Medio de Almacenamiento incorrecto '" + sTanqueTipoMedAlmacenamiento + "'");

                                                                        // sTanqueDescMedicion
                                                                        if (String.IsNullOrEmpty(sTanqueDescMedicion))
                                                                            return BadRequest("El AutoTanque no contiene la Descripción en la Medición");
                                                                        else if (sTanqueDescMedicion.Length < 2)
                                                                            return BadRequest("La Descripción de la Medición debe ser minimo de 2 caracteres");
                                                                        #endregion

                                                                        #region AutoTanque: Llenado de Estructura.
                                                                        CVolJSonDTO.stTanqueDatos objAutoTanqueNew = new CVolJSonDTO.stTanqueDatos();

                                                                        objAutoTanqueNew.ClaveIdentificacionTanque = sClaveAutoTanq;
                                                                        objAutoTanqueNew.LocalizacionYODescripcionTanque = sDescripcionSAT;
                                                                        objAutoTanqueNew.VigenciaCalibracionTanque = dtTanqueFCalibracion.ToString("yyyy-MM-dd");

                                                                        #region Capacidad Total Tanque.
                                                                        CVolJSonDTO.stCapacidadDato objCapacidadTotalDato = new CVolJSonDTO.stCapacidadDato();
                                                                        objCapacidadTotalDato.ValorNumerico = iCapTotal;
                                                                        objCapacidadTotalDato.UnidadDeMedida = sTanqueUniMed;

                                                                        objAutoTanqueNew.CapacidadTotalTanque = objCapacidadTotalDato;
                                                                        #endregion

                                                                        #region Capacidad Operativa.
                                                                        CVolJSonDTO.stCapacidadDato objCapOperativaDato = new CVolJSonDTO.stCapacidadDato();
                                                                        objCapOperativaDato.ValorNumerico = iCapOperativa;
                                                                        objCapOperativaDato.UnidadDeMedida = sTanqueUniMed;

                                                                        objAutoTanqueNew.CapacidadOperativaTanque = objCapOperativaDato;
                                                                        #endregion

                                                                        #region Capacidad Util.
                                                                        CVolJSonDTO.stCapacidadDato objCapUtilDato = new CVolJSonDTO.stCapacidadDato();
                                                                        objCapUtilDato.ValorNumerico = iCapUtil;
                                                                        objCapUtilDato.UnidadDeMedida = sTanqueUniMed;

                                                                        objAutoTanqueNew.CapacidadUtilTanque = objCapUtilDato;
                                                                        #endregion

                                                                        #region Capacidad Fondaje.
                                                                        CVolJSonDTO.stCapacidadDato objCapFondaje = new CVolJSonDTO.stCapacidadDato();
                                                                        objCapFondaje.ValorNumerico = iCapFondaje;
                                                                        objCapFondaje.UnidadDeMedida = sTanqueUniMed;

                                                                        objAutoTanqueNew.CapacidadFondajeTanque = objCapFondaje;
                                                                        #endregion

                                                                        #region Volumen Minimo Operación.
                                                                        CVolJSonDTO.stCapacidadDato objVolMinOperDato = new CVolJSonDTO.stCapacidadDato();
                                                                        objVolMinOperDato.ValorNumerico = iVolMinOperacion;
                                                                        objVolMinOperDato.UnidadDeMedida = sTanqueUniMed;

                                                                        objAutoTanqueNew.VolumenMinimoOperacion = objVolMinOperDato;
                                                                        #endregion

                                                                        #region Estado Tanque.
                                                                        objAutoTanqueNew.EstadoTanque = "F";
                                                                        if (sTanqueStatus)
                                                                            objAutoTanqueNew.EstadoTanque = "O";
                                                                        #endregion

                                                                        #region Medición de Tanques.
                                                                        Decimal dTanqIncertMed = 0;
                                                                        if (iTanqueIncertidumbre > 0)
                                                                            dTanqIncertMed = (iTanqueIncertidumbre / 100);

                                                                        String sSistMedicion = "";
                                                                        switch (sTanqueTipoMedicion)
                                                                        {
                                                                            case "SMD": // SMD-ETA-TQS-USP-0026.
                                                                                sSistMedicion = sTanqueTipoMedicion + "-" +
                                                                                                sTanqueTipoMedAlmacenamiento + "-" +
                                                                                                sTipoAutoTanq + "-" +
                                                                                                sNomClaveInstalacion + "-" +
                                                                                                iRecepNTanque.ToString().PadLeft(4, '0');
                                                                                break;

                                                                            case "SME": // SME-STQ-EDS-0021.
                                                                                sSistMedicion = sTanqueTipoMedicion + "-" +
                                                                                                sTipoAutoTanq + "-" +
                                                                                                sNomClaveInstalacion + "-" +
                                                                                                iRecepNTanque.ToString().PadLeft(4, '0');
                                                                                break;

                                                                            default:
                                                                                sSistMedicion = sTanqueTipoMedicion + "-" +
                                                                                                sTipoAutoTanq + "-" +
                                                                                                sNomClaveInstalacion + "-" +
                                                                                                iRecepNTanque.ToString().PadLeft(4, '0');
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

                                                                        objAutoTanqueNew.Medidores = lstMedidores;
                                                                        #endregion

                                                                        #region Existencias.
                                                                        CVolJSonDTO.stExistenciaDato objRecepExistenciaDatos = new CVolJSonDTO.stExistenciaDato();

                                                                        objRecepExistenciaDatos.VolumenExistenciasAnterior = 0;

                                                                        #region VolumenAcumOpsRecepcion.
                                                                        CVolJSonDTO.stVolumenDato objRecepVolAntDato = new CVolJSonDTO.stVolumenDato();
                                                                        objRecepVolAntDato.UnidadDeMedida = sRecepUniMed;
                                                                        objRecepVolAntDato.ValorNumerico = Math.Round(dRecepcionesAutTanqAnt, 3);
                                                                        objRecepExistenciaDatos.VolumenAcumOpsRecepcion = objRecepVolAntDato;
                                                                        #endregion

                                                                        objRecepExistenciaDatos.HoraRecepcionAcumulado = dtFechaRegistro.ToString("HH:mm:ss") + "-" +
                                                                                                                         iDiferenciaHora.ToString("00") + ":00";

                                                                        #region VolumenAcumOpsEntrega.
                                                                        CVolJSonDTO.stCapacidadDato objEntVolAntDato = new CVolJSonDTO.stCapacidadDato();
                                                                        objEntVolAntDato.UnidadDeMedida = sRecepUniMed;
                                                                        objEntVolAntDato.ValorNumerico = Math.Round(dEntregasAutTanqAnt, 3);
                                                                        objRecepExistenciaDatos.VolumenAcumOpsEntrega = objEntVolAntDato;
                                                                        #endregion

                                                                        objRecepExistenciaDatos.HoraEntregaAcumulado = dtFechaRegistro.ToString("HH:mm:ss") + "-" +
                                                                                                                       iDiferenciaHora.ToString("00") + ":00";
                                                                        objRecepExistenciaDatos.VolumenExistencias = Math.Round(dRecepcionesAutTanqAnt - dEntregasAutTanqAnt, 3);
                                                                        objRecepExistenciaDatos.FechaYHoraEstaMedicion = dtPerDateEnd.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00";
                                                                        objRecepExistenciaDatos.FechaYHoraMedicionAnterior = dtPerDateIni.AddSeconds(-1).ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00";

                                                                        objAutoTanqueNew.Existencias = objRecepExistenciaDatos;
                                                                        #endregion

                                                                        #region Inicializacion del Nodo: Recepciones.
                                                                        CVolJSonDTO.stRecepcionDato objRecepcionDatoNew = new CVolJSonDTO.stRecepcionDato();

                                                                        #region Suma Volumen Recepcion.
                                                                        CVolJSonDTO.stVolumenDato objSumaVolRecepNew = new CVolJSonDTO.stVolumenDato();
                                                                        objSumaVolRecepNew.UnidadDeMedida = sTanqueUniMed;
                                                                        objSumaVolRecepNew.ValorNumerico = 0;

                                                                        objRecepcionDatoNew.SumaVolumenRecepcion = objSumaVolRecepNew;
                                                                        #endregion

                                                                        #region Recepciones.
                                                                        List<CVolJSonDTO.stRecepcionIndividualDato> lstRecepcionesNew = new List<CVolJSonDTO.stRecepcionIndividualDato>();
                                                                        objRecepcionDatoNew.Recepcion = lstRecepcionesNew;
                                                                        #endregion

                                                                        objRecepcionDatoNew.SumaCompras = 0;
                                                                        objAutoTanqueNew.Recepciones = objRecepcionDatoNew;
                                                                        #endregion

                                                                        #region Inicializacion del Nodo:Entregas.
                                                                        CVolJSonDTO.stEntregaDato objEntregaDatoNew = new CVolJSonDTO.stEntregaDato();

                                                                        #region Suma Ventas.
                                                                        objEntregaDatoNew.SumaVentas = 0;
                                                                        #endregion

                                                                        #region Suma Volumen Entregado.
                                                                        CVolJSonDTO.stVolumenDato objSumaVolEntregadoNew = new CVolJSonDTO.stVolumenDato();
                                                                        objSumaVolEntregadoNew.UnidadDeMedida = sTanqueUniMed;
                                                                        objSumaVolEntregadoNew.ValorNumerico = 0;

                                                                        objEntregaDatoNew.SumaVolumenEntregado = objSumaVolEntregadoNew;
                                                                        #endregion

                                                                        #region Entregas.
                                                                        objEntregaDatoNew.Entrega = null;
                                                                        #endregion

                                                                        objAutoTanqueNew.Entregas = objEntregaDatoNew;
                                                                        #endregion

                                                                        lstProdAutoTanques.Add(objAutoTanqueNew);
                                                                        iTotalTanques++;
                                                                        #endregion
                                                                    }
                                                                #endregion
                                                                #endregion

                                                                iIdxAutoTanq = lstProdAutoTanques.FindIndex(d => d.ClaveIdentificacionTanque.Equals(sClaveAutoTanq));
                                                            }
                                                            #endregion

                                                            #region Agregamos la Recepcion.
                                                            objAutoTanqueDatos = lstProdAutoTanques[iIdxAutoTanq];
                                                            CVolJSonDTO.stRecepcionDato objAutoTanqRecepcion = objAutoTanqueDatos.Recepciones;
                                                            List<CVolJSonDTO.stRecepcionIndividualDato> lstAutoTanqRecepciones = (List<CVolJSonDTO.stRecepcionIndividualDato>)objAutoTanqRecepcion.Recepcion;

                                                            #region Volumen Inicial Dato.
                                                            CVolJSonDTO.stCapacidadDato objRecepVolIni = new CVolJSonDTO.stCapacidadDato();
                                                            objRecepVolIni.ValorNumerico = dRecepVolPuntoEntrada;
                                                            objRecepVolIni.UnidadDeMedida = sRecepUniMed;
                                                            #endregion

                                                            #region Volumen Final Dato.
                                                            CVolJSonDTO.stCapacidadDato objRecepVolFin = new CVolJSonDTO.stCapacidadDato();
                                                            objRecepVolFin.UnidadDeMedida = sRecepUniMed;
                                                            objRecepVolFin.ValorNumerico = dRecepVolFinal;
                                                            #endregion

                                                            #region Volumen de Recepcion.
                                                            CVolJSonDTO.stCapacidadDato objRecepVolumen = new CVolJSonDTO.stCapacidadDato();
                                                            objRecepVolumen.ValorNumerico = dRecepVolRecepcion;
                                                            objRecepVolumen.UnidadDeMedida = sRecepUniMed;
                                                            #endregion

                                                            #region Complementos.
                                                            Object oRecepcionComplemento = null;

                                                            if (bCompRecep)
                                                            {
                                                                List<CVolJSonDTO.stComplementoDistribucion> lstRecepComplementoDistribucion = new List<CVolJSonDTO.stComplementoDistribucion>();
                                                                List<CVolJSonDTO.stComplementoTransportista> lstRecepComplementoTransportista = new List<CVolJSonDTO.stComplementoTransportista>();
                                                                List<CVolJSonDTO.stComplementoComercializadora> lstRecepComplementoComercializadora = new List<CVolJSonDTO.stComplementoComercializadora>();

                                                                #region NODO: Nacional.
                                                                List<CVolJSonDTO.stCompleDistNacional> lstRecepDistNacional = new List<CVolJSonDTO.stCompleDistNacional>();
                                                                List<CVolJSonDTO.stComplementoTransNacional> lstRecepTransNacional = new List<CVolJSonDTO.stComplementoTransNacional>();
                                                                List<CVolJSonDTO.stCompleComerNacional> lstRecepComerNacional = new List<CVolJSonDTO.stCompleComerNacional>();

                                                                #region Consulta: CFDI Datos.
                                                                var vQRecepCfdiDato = (from r in objContext.InventoryIns
                                                                                       join rd in objContext.InventoryInDocuments on r.InventoryInId equals rd.InventoryInId
                                                                                       join pd in objContext.SupplierFuels on new { f1 = r.StoreId, f2 = rd.SupplierFuelIdi.GetValueOrDefault() } equals new { f1 = pd.StoreId, f2 = pd.SupplierFuelIdi }
                                                                                       join sd in objContext.SupplierTransportRegisters on rd.SupplierTransportRegisterId equals sd.SupplierTransportRegisterId into sdf
                                                                                       from sd in sdf.DefaultIfEmpty()
                                                                                       where r.StoreId == viNEstacion &&
                                                                                             r.InventoryInId == gNRecepcionID
                                                                                       select new
                                                                                       {
                                                                                           ClienteRFC = pd.Rfc,
                                                                                           NombreCliente = pd.Name,
                                                                                           NumeroPermisoCRE = pd.FuelPermission,
                                                                                           TipoCFDI = rd.SatTipoComprobanteId,
                                                                                           CFDI = rd.Uuid,
                                                                                           Precio = rd.Price,
                                                                                           PrecioVtaPublico = rd.PublicSalePrice,
                                                                                           FechaHora = rd.Date,
                                                                                           Volumen = rd.Volume,
                                                                                           ContraPrestacion = (sd.AmountPerService.ToString() ?? "0"),
                                                                                           TarifaTransporte = (sd.AmountPerFee.ToString() ?? "0"),
                                                                                           CargoCapacidadTrans = (sd.AmountPerCapacity.ToString() ?? "0"),
                                                                                           CargoUsoTrans = (sd.AmountPerUse.ToString() ?? "0"),
                                                                                           CargoVolumenTrans = (sd.AmountPerVolume.ToString() ?? "0"),
                                                                                           Descuento = (sd.AmountOfDiscount.ToString() ?? "0")
                                                                                       });
                                                                #endregion

                                                                #region Lectura: CFDI Datos.
                                                                if (vQRecepCfdiDato != null)
                                                                    foreach (var vDuctoRecepDatos in vQRecepCfdiDato)
                                                                    {
                                                                        #region CFDI: Asignamos Valores.
                                                                        String sNacClienteRFC = vDuctoRecepDatos.ClienteRFC,
                                                                               sNacNombreCliente = vDuctoRecepDatos.NombreCliente,
                                                                               sNacPermisoCRE = vDuctoRecepDatos.NumeroPermisoCRE,
                                                                               sNacTipoCFDI = vDuctoRecepDatos.TipoCFDI,
                                                                               sNacCFDI = vDuctoRecepDatos.CFDI.ToString(),
                                                                               sNacUnidadMedida = vProd.UnidadMedida;
                                                                        Decimal dNacPrecioCompra = vDuctoRecepDatos.Precio.GetValueOrDefault(),
                                                                                dNacPrecioVtaPublico = vDuctoRecepDatos.PrecioVtaPublico.GetValueOrDefault(),
                                                                                dNacVolumen = vDuctoRecepDatos.Volumen.GetValueOrDefault(),
                                                                                dNacContraPrestacion = Convert.ToDecimal(vDuctoRecepDatos.ContraPrestacion),
                                                                                dNacDescuento = Convert.ToDecimal(vDuctoRecepDatos.Descuento),
                                                                                dNacTarifaTrans = Convert.ToDecimal(vDuctoRecepDatos.TarifaTransporte),
                                                                                dNacCargoCapTrans = Convert.ToDecimal(vDuctoRecepDatos.CargoCapacidadTrans),
                                                                                dNacCargoUsoTrans = Convert.ToDecimal(vDuctoRecepDatos.CargoUsoTrans),
                                                                                dNacCargoVolTrans = Convert.ToDecimal(vDuctoRecepDatos.CargoVolumenTrans);
                                                                        DateTime dtNacFechaHora = Convert.ToDateTime(vDuctoRecepDatos.FechaHora);
                                                                        #endregion

                                                                        #region CFDI: Validamos Valores.
                                                                        if (!ValidarCFDI(sNacCFDI))
                                                                            return BadRequest("La Recepción '" + iNRecepcion.ToString() + "' contiene un CFDI con formato incorrecto '" + sNacCFDI + "'.");

                                                                        String sRecepMnsErrorTipoCFDI = "";
                                                                        switch (sNacTipoCFDI.ToUpper())
                                                                        {
                                                                            case "INGRESO": sNacTipoCFDI = "Ingreso"; break;
                                                                            case "EGRESO": sNacTipoCFDI = "Egreso"; break;
                                                                            case "TRASLADO": sNacTipoCFDI = "Traslado"; break;
                                                                            default: sRecepMnsErrorTipoCFDI = "Tipo de CFDI no valido '" + sNacTipoCFDI + "'"; break;
                                                                        }

                                                                        if (!String.IsNullOrEmpty(sRecepMnsErrorTipoCFDI))
                                                                            return BadRequest(sRecepMnsErrorTipoCFDI);
                                                                        #endregion

                                                                        #region CFDI: Llenado de Estructura.
                                                                        int iIdxCliente = -1;
                                                                        switch (objTipoComplemento)
                                                                        {
                                                                            #region Complemento: Distribuidor.
                                                                            case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                                                iIdxCliente = lstRecepDistNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));

                                                                                CVolJSonDTO.stCompleDistNacional objRecepDistNacionalDato;
                                                                                if (iIdxCliente < 0)
                                                                                {
                                                                                    objRecepDistNacionalDato = new CVolJSonDTO.stCompleDistNacional();
                                                                                    objRecepDistNacionalDato.RfcClienteOProveedor = sNacClienteRFC;
                                                                                    objRecepDistNacionalDato.NombreClienteOProveedor = sNacNombreCliente;

                                                                                    if (!String.IsNullOrEmpty(sNacPermisoCRE))
                                                                                        objRecepDistNacionalDato.PermisoClienteOProveedor = sNacPermisoCRE;

                                                                                    List<CVolJSonDTO.stCompleDistNacionalCfdis> lstCFDIsNew = new List<CVolJSonDTO.stCompleDistNacionalCfdis>();
                                                                                    objRecepDistNacionalDato.CFDIs = lstCFDIsNew;

                                                                                    lstRecepDistNacional.Add(objRecepDistNacionalDato);
                                                                                    iIdxCliente = lstRecepDistNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));
                                                                                }

                                                                                #region CFDI Datos.
                                                                                CVolJSonDTO.stCompleDistNacionalCfdis objDistCFDI_Dato = new CVolJSonDTO.stCompleDistNacionalCfdis();
                                                                                objDistCFDI_Dato.Cfdi = sNacCFDI;
                                                                                objDistCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                                objDistCFDI_Dato.PrecioVentaOCompraOContrap = Math.Round(dNacPrecioCompra, 3);
                                                                                objDistCFDI_Dato.FechaYHoraTransaccion = dtNacFechaHora.ToString("s") + "-" +
                                                                                                                         iDiferenciaHora.ToString("00") + ":00";
                                                                                #region VolumenDocumentado.
                                                                                CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                                objVolumenDocumentado.ValorNumerico = Math.Round(dNacVolumen, 3);
                                                                                objVolumenDocumentado.UnidadDeMedida = sNacUnidadMedida;

                                                                                objDistCFDI_Dato.VolumenDocumentado = objVolumenDocumentado;
                                                                                #endregion
                                                                                #endregion

                                                                                objRecepDistNacionalDato = (CVolJSonDTO.stCompleDistNacional)lstRecepDistNacional[iIdxCliente];
                                                                                List<CVolJSonDTO.stCompleDistNacionalCfdis> lstCfDIs = (List<CVolJSonDTO.stCompleDistNacionalCfdis>)objRecepDistNacionalDato.CFDIs;
                                                                                lstCfDIs.Add(objDistCFDI_Dato);
                                                                                objRecepDistNacionalDato.CFDIs = lstCfDIs;
                                                                                lstRecepDistNacional[iIdxCliente] = objRecepDistNacionalDato;
                                                                                break;
                                                                            #endregion

                                                                            #region Complemento: Transportista.
                                                                            case CVolJSonDTO.eTipoComplemento.Transportista: break;
                                                                            #endregion

                                                                            #region Complemento: Comercializadora.
                                                                            case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                                                iIdxCliente = lstRecepComerNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));

                                                                                CVolJSonDTO.stCompleComerNacional objRecepComerNacionalDato;
                                                                                if (iIdxCliente < 0)
                                                                                {
                                                                                    objRecepComerNacionalDato = new CVolJSonDTO.stCompleComerNacional();
                                                                                    objRecepComerNacionalDato.RfcClienteOProveedor = sNacClienteRFC;
                                                                                    objRecepComerNacionalDato.NombreClienteOProveedor = sNacNombreCliente;

                                                                                    if (!String.IsNullOrEmpty(sNacPermisoCRE))
                                                                                        objRecepComerNacionalDato.PermisoProveedor = sNacPermisoCRE;

                                                                                    List<CVolJSonDTO.stCompleComerNacionalCfdis> lstCFDIsNew = new List<CVolJSonDTO.stCompleComerNacionalCfdis>();
                                                                                    objRecepComerNacionalDato.CFDIs = lstCFDIsNew;

                                                                                    lstRecepComerNacional.Add(objRecepComerNacionalDato);
                                                                                    iIdxCliente = lstRecepComerNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));
                                                                                }

                                                                                #region CFDI Datos.
                                                                                CVolJSonDTO.stCompleComerNacionalCfdis objComerCFDI_Dato = new CVolJSonDTO.stCompleComerNacionalCfdis();
                                                                                objComerCFDI_Dato.Cfdi = sNacCFDI;
                                                                                objComerCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                                objComerCFDI_Dato.PrecioCompra = Math.Round(dNacPrecioCompra, 3);
                                                                                objComerCFDI_Dato.PrecioDeVentaAlPublico = Math.Round(dNacPrecioVtaPublico, 3);
                                                                                objComerCFDI_Dato.FechayHoraTransaccion = dtNacFechaHora.ToString("s") + "-" +
                                                                                                                          iDiferenciaHora.ToString("00") + ":00";
                                                                                #region VolumenDocumentado.
                                                                                CVolJSonDTO.stVolumenDato objRecepComerVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                                objRecepComerVolumenDocumentado.ValorNumerico = Math.Round(dNacVolumen, 3);
                                                                                objRecepComerVolumenDocumentado.UnidadDeMedida = sNacUnidadMedida;

                                                                                objComerCFDI_Dato.VolumenDocumentado = objRecepComerVolumenDocumentado;
                                                                                #endregion
                                                                                #endregion

                                                                                objRecepComerNacionalDato = (CVolJSonDTO.stCompleComerNacional)lstRecepComerNacional[iIdxCliente];
                                                                                List<CVolJSonDTO.stCompleComerNacionalCfdis> lstCompRecepCfDIs = (List<CVolJSonDTO.stCompleComerNacionalCfdis>)objRecepComerNacionalDato.CFDIs;
                                                                                lstCompRecepCfDIs.Add(objComerCFDI_Dato);
                                                                                objRecepComerNacionalDato.CFDIs = lstCompRecepCfDIs;
                                                                                lstRecepComerNacional[iIdxCliente] = objRecepComerNacionalDato;
                                                                                break;
                                                                                #endregion
                                                                        }
                                                                        #endregion
                                                                    }
                                                                #endregion
                                                                #endregion

                                                                #region NODO: Extranjero.
                                                                List<CVolJSonDTO.stCompleDistExtranjero> lstRecepDistExtranjero = new List<CVolJSonDTO.stCompleDistExtranjero>();
                                                                List<CVolJSonDTO.stCompleComerExtranjero> lstRecepComerExtranjero = new List<CVolJSonDTO.stCompleComerExtranjero>();

                                                                #region Consulta: Pedimento Datos.
                                                                var vQRecepPedimentoDato = (from p in objContext.PetitionCustoms
                                                                                            join t in objContext.TransportMediumnCustoms on p.TransportMediumnCustomsId equals t.TransportMediumnCustomsId
                                                                                            where p.PetitionCustomsId == gPedimentoID
                                                                                            select new
                                                                                            {
                                                                                                ClavePermisoImportOExport = p.KeyOfImportationExportation,
                                                                                                PuntoInternacionOExtracccion = p.KeyPointOfInletOrOulet,
                                                                                                Pais = (p.SatPaisId ?? String.Empty),
                                                                                                MedioIngresoOSalida = (t.TransportMediumn ?? "0"),
                                                                                                ClavePedimento = (p.NumberCustomsDeclaration ?? String.Empty),
                                                                                                Incoterms = (p.Incoterms ?? String.Empty),
                                                                                                PrecioDeImportOExport = p.AmountOfImportationExportation,
                                                                                                Volumen = p.QuantityDocumented,
                                                                                                UnidadMedida = (p.JsonClaveUnidadMedidadId ?? "UM03")
                                                                                            });
                                                                #endregion

                                                                #region Lectura: Pedimentos.
                                                                if (vQRecepPedimentoDato != null)
                                                                    foreach (var vPedimentoDatos in vQRecepPedimentoDato)
                                                                    {
                                                                        #region Pedimento Datos: Asignamos Valores.
                                                                        String sExtClavePermiso = vPedimentoDatos.ClavePermisoImportOExport,
                                                                               sExtPais = vPedimentoDatos.Pais,
                                                                               sExtClavePedimento = vPedimentoDatos.ClavePedimento,
                                                                               sExtIncoterms = vPedimentoDatos.Incoterms,
                                                                               sExtUnidadMedida = vPedimentoDatos.UnidadMedida;
                                                                        int iExtPuntoInterOExtra = Convert.ToInt32(vPedimentoDatos.PuntoInternacionOExtracccion),
                                                                            iExtMedioIngOSal = Convert.ToInt32(vPedimentoDatos.MedioIngresoOSalida);
                                                                        Decimal dExtImporte = vPedimentoDatos.PrecioDeImportOExport,
                                                                                dExtVolumen = vPedimentoDatos.Volumen;
                                                                        #endregion

                                                                        #region Pedimento Datos: Validamos Valores.
                                                                        if (String.IsNullOrEmpty(sExtClavePermiso))
                                                                            return BadRequest("No se encontro el dato 'ClavePermiso' del Pedimento de Recepción.");

                                                                        if (String.IsNullOrEmpty(sExtPais))
                                                                            return BadRequest("No se encontro el dato 'Pais' del Pedimento de Recepción.");

                                                                        if (String.IsNullOrEmpty(sExtClavePedimento))
                                                                            return BadRequest("No se encontro el dato 'ClavePedimento' del Pedimento de Recepción.");

                                                                        if (String.IsNullOrEmpty(sExtIncoterms))
                                                                            return BadRequest("No se encontro el dato 'Incoterms' del Pedimento de Recepción.");
                                                                        #endregion

                                                                        #region Pedimento Datos: Llenado de Estructura.
                                                                        int iIdxPermiso = -1;
                                                                        switch (objTipoComplemento)
                                                                        {
                                                                            #region Distribucion.
                                                                            case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                                                iIdxPermiso = lstRecepDistExtranjero.FindIndex(e => e.PermisoImportacionOExportacion.Equals(sExtClavePermiso));

                                                                                CVolJSonDTO.stCompleDistExtranjero objRecepDistExtranjeroDato;
                                                                                if (iIdxPermiso < 0)
                                                                                {
                                                                                    objRecepDistExtranjeroDato = new CVolJSonDTO.stCompleDistExtranjero();
                                                                                    objRecepDistExtranjeroDato.PermisoImportacionOExportacion = sExtClavePermiso;
                                                                                    List<CVolJSonDTO.stCompleDistExtranjeroPedimentos> lstPedimentosNew = new List<CVolJSonDTO.stCompleDistExtranjeroPedimentos>();
                                                                                    objRecepDistExtranjeroDato.Pedimentos = lstPedimentosNew;

                                                                                    lstRecepDistExtranjero.Add(objRecepDistExtranjeroDato);
                                                                                    iIdxPermiso = lstRecepDistExtranjero.FindIndex(e => e.PermisoImportacionOExportacion.Equals(sExtClavePermiso));
                                                                                }

                                                                                #region Pedimento Datos.
                                                                                CVolJSonDTO.stCompleDistExtranjeroPedimentos objRecepDistPedimentoDato = new CVolJSonDTO.stCompleDistExtranjeroPedimentos();
                                                                                objRecepDistPedimentoDato.PuntoDeInternacionOExtraccion = iExtPuntoInterOExtra.ToString();
                                                                                objRecepDistPedimentoDato.PaisOrigenODestino = sExtPais;
                                                                                objRecepDistPedimentoDato.MedioDeTransEntraOSaleAduana = iExtMedioIngOSal.ToString();
                                                                                objRecepDistPedimentoDato.Incoterms = sExtIncoterms;
                                                                                objRecepDistPedimentoDato.PrecioDeImportacionOExportacion = Math.Round(dExtImporte, 3);
                                                                                objRecepDistPedimentoDato.PedimentoAduanal = sExtClavePedimento;

                                                                                #region VolumenDocumentado.
                                                                                CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                                objVolumenDocumentado.ValorNumerico = Math.Round(dExtVolumen, 3);
                                                                                objVolumenDocumentado.UnidadDeMedida = sExtUnidadMedida;

                                                                                objRecepDistPedimentoDato.VolumenDocumentado = objVolumenDocumentado;
                                                                                #endregion
                                                                                #endregion

                                                                                objRecepDistExtranjeroDato = (CVolJSonDTO.stCompleDistExtranjero)lstRecepDistExtranjero[iIdxPermiso];
                                                                                List<CVolJSonDTO.stCompleDistExtranjeroPedimentos> lstRecepDistPedimentos = (List<CVolJSonDTO.stCompleDistExtranjeroPedimentos>)objRecepDistExtranjeroDato.Pedimentos;
                                                                                lstRecepDistPedimentos.Add(objRecepDistPedimentoDato);
                                                                                objRecepDistExtranjeroDato.Pedimentos = lstRecepDistPedimentos;
                                                                                lstRecepDistExtranjero[iIdxPermiso] = objRecepDistExtranjeroDato;
                                                                                break;
                                                                            #endregion

                                                                            #region Comercializadora.
                                                                            case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                                                iIdxPermiso = lstRecepComerExtranjero.FindIndex(e => e.PermisoImportacion.Equals(sExtClavePermiso));

                                                                                CVolJSonDTO.stCompleComerExtranjero objRecepComerExtranjeroDato;
                                                                                if (iIdxPermiso < 0)
                                                                                {
                                                                                    objRecepComerExtranjeroDato = new CVolJSonDTO.stCompleComerExtranjero();
                                                                                    objRecepComerExtranjeroDato.PermisoImportacion = sExtClavePermiso;
                                                                                    List<CVolJSonDTO.stCompleComerExtranjeroPedimentos> lstPedimentosNew = new List<CVolJSonDTO.stCompleComerExtranjeroPedimentos>();
                                                                                    objRecepComerExtranjeroDato.Pedimentos = lstPedimentosNew;

                                                                                    lstRecepComerExtranjero.Add(objRecepComerExtranjeroDato);
                                                                                    iIdxPermiso = lstRecepComerExtranjero.FindIndex(e => e.PermisoImportacion.Equals(sExtClavePermiso));
                                                                                }

                                                                                #region Pedimento Datos.
                                                                                CVolJSonDTO.stCompleComerExtranjeroPedimentos objRecepComerPedimentoDato = new CVolJSonDTO.stCompleComerExtranjeroPedimentos();
                                                                                objRecepComerPedimentoDato.PuntoDeInternacion = iExtPuntoInterOExtra;
                                                                                objRecepComerPedimentoDato.PaisOrigen = sExtPais;
                                                                                objRecepComerPedimentoDato.MedioDeTransEntraAduana = iExtMedioIngOSal;
                                                                                objRecepComerPedimentoDato.Incoterms = sExtIncoterms;
                                                                                objRecepComerPedimentoDato.PrecioDeImportacion = Math.Round(dExtImporte, 3);
                                                                                objRecepComerPedimentoDato.PedimentoAduanal = sExtClavePedimento;

                                                                                #region VolumenDocumentado.
                                                                                CVolJSonDTO.stVolumenDato objRecepComerVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                                objRecepComerVolumenDocumentado.ValorNumerico = Math.Round(dExtVolumen, 3);
                                                                                objRecepComerVolumenDocumentado.UnidadDeMedida = sExtUnidadMedida;

                                                                                objRecepComerPedimentoDato.VolumenDocumentado = objRecepComerVolumenDocumentado;
                                                                                #endregion
                                                                                #endregion

                                                                                objRecepComerExtranjeroDato = (CVolJSonDTO.stCompleComerExtranjero)lstRecepComerExtranjero[iIdxPermiso];
                                                                                List<CVolJSonDTO.stCompleComerExtranjeroPedimentos> lstRecepComerPedimentos = (List<CVolJSonDTO.stCompleComerExtranjeroPedimentos>)objRecepComerExtranjeroDato.Pedimentos;
                                                                                lstRecepComerPedimentos.Add(objRecepComerPedimentoDato);
                                                                                objRecepComerExtranjeroDato.Pedimentos = lstRecepComerPedimentos;
                                                                                lstRecepComerExtranjero[iIdxPermiso] = objRecepComerExtranjeroDato;
                                                                                break;
                                                                                #endregion
                                                                        }
                                                                        #endregion
                                                                    }
                                                                #endregion
                                                                #endregion

                                                                CVolJSonDTO.stComplementoDistribucion objRecepComplementoDistribuidor = new CVolJSonDTO.stComplementoDistribucion();
                                                                CVolJSonDTO.stComplementoTransportista objRecepComplementoTransportista = new CVolJSonDTO.stComplementoTransportista();
                                                                CVolJSonDTO.stComplementoComercializadora objRecepComplementoComercializadora = new CVolJSonDTO.stComplementoComercializadora();

                                                                switch (objTipoComplemento)
                                                                {
                                                                    #region Complemento: Distribuidor.
                                                                    case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                                        objRecepComplementoDistribuidor.TipoComplemento = "Distribucion";

                                                                        if (lstRecepDistNacional.Count > 0)
                                                                            objRecepComplementoDistribuidor.Nacional = lstRecepDistNacional;

                                                                        if (lstRecepDistExtranjero.Count > 0)
                                                                            objRecepComplementoDistribuidor.Extranjero = lstRecepDistExtranjero;

                                                                        if (lstRecepDistNacional.Count > 0 || lstRecepDistExtranjero.Count > 0)
                                                                            oRecepcionComplemento = objRecepComplementoDistribuidor;
                                                                        break;
                                                                    #endregion

                                                                    #region Complemento: Transportista.
                                                                    case CVolJSonDTO.eTipoComplemento.Transportista:
                                                                        objRecepComplementoTransportista.TipoComplemento = "Transporte";

                                                                        if (lstRecepTransNacional.Count > 0)
                                                                            objRecepComplementoTransportista.Nacional = lstRecepTransNacional;

                                                                        if (lstRecepTransNacional.Count > 0)
                                                                            oRecepcionComplemento = objRecepComplementoTransportista;
                                                                        break;
                                                                    #endregion

                                                                    #region Complemento: Comercializadora.
                                                                    case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                                        objRecepComplementoComercializadora.TipoComplemento = "Comercializacion";

                                                                        if (lstRecepComerNacional.Count > 0)
                                                                            objRecepComplementoComercializadora.Nacional = lstRecepComerNacional;

                                                                        if (lstRecepComerExtranjero.Count > 0)
                                                                            objRecepComplementoComercializadora.Extranjero = lstRecepComerExtranjero;

                                                                        if (lstRecepComerNacional.Count > 0 || lstRecepComerExtranjero.Count > 0)
                                                                            oRecepcionComplemento = objRecepComplementoComercializadora;
                                                                        break;
                                                                        #endregion
                                                                }
                                                            }
                                                            #endregion

                                                            lstAutoTanqRecepciones.Add(new CVolJSonDTO.stRecepcionIndividualDato
                                                            {
                                                                NumeroDeRegistro = iNRecepcion,
                                                                VolumenInicialTanque = objRecepVolIni,
                                                                VolumenFinalTanque = dRecepVolFinal,
                                                                VolumenRecepcion = objRecepVolumen,
                                                                Temperatura = Math.Round(dRecepTemperatura, 3),
                                                                PresionAbsoluta = Math.Round(dRecepPresionAbs, 3),
                                                                Complemento = oRecepcionComplemento,
                                                                FechaYHoraInicioRecepcion = dtRecepFIni.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00",
                                                                FechaYHoraFinalRecepcion = dtRecepFFin.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00"
                                                            });
                                                            #endregion

                                                            #region Guardamos las Recepciones.
                                                            objAutoTanqRecepcion.TotalDocumentos++;
                                                            objAutoTanqRecepcion.TotalRecepciones++;

                                                            #region Suma Compras.
                                                            Decimal dRecepTotalCompras = Convert.ToDecimal(objAutoTanqRecepcion.SumaCompras);
                                                            objAutoTanqRecepcion.SumaCompras = Math.Round(dRecepTotalCompras + dRecepImporte, 3);
                                                            #endregion

                                                            #region Suma Volumen Recepcion.
                                                            CVolJSonDTO.stVolumenDato objRecepSumaVol = (CVolJSonDTO.stVolumenDato)objAutoTanqRecepcion.SumaVolumenRecepcion;
                                                            Decimal dRecepTotalVol = Convert.ToDecimal(objRecepSumaVol.ValorNumerico);
                                                            objRecepSumaVol.ValorNumerico = dRecepTotalVol + dRecepVolRecepcion;
                                                            objAutoTanqRecepcion.SumaVolumenRecepcion = objRecepSumaVol;
                                                            #endregion

                                                            if (objAutoTanqRecepcion.TotalRecepciones > 0)
                                                                objAutoTanqRecepcion.Recepcion = lstAutoTanqRecepciones;

                                                            objAutoTanqueDatos.Recepciones = objAutoTanqRecepcion;
                                                            lstProdAutoTanques[iIdxAutoTanq] = objAutoTanqueDatos;
                                                            #endregion
                                                            break;
                                                            #endregion
                                                    }
                                                }
                                            #endregion
                                        }
                                        #endregion

                                        #region Entregas: Ducto y AutoTanque.
                                        if (iTotalEntProd > 0)
                                        {
                                            #region Consulta: Entregas Datos.
                                            //var vQEntregasDatos = (from e in objContext.InventoryInSaleOrders
                                            //                       join a in objContext.Tanks on new { f1 = e.StoreId, f2 = e.TankIdi.GetValueOrDefault() } equals new { f1 = a.StoreId, f2 = a.TankIdi }
                                            //                       join ed in objContext.InventoryInDocuments on e.InventoryIn equals ed.InventoryInId
                                            //                       where e.StoreId == viNEstacion &&
                                            //                             e.EndDate >= dtPerDateIni && e.EndDate <= dtPerDateEnd &&
                                            //                             e.ProductId == vProd.ProductoID
                                            //                       select new
                                            //                       {
                                            //                           TipoTanque = a.SatTankType, // eh.JsonTipoDistribucionId
                                            //                           TipoMedicion = a.SatTypeMediumStorage,
                                            //                           NumeroTanque = e.TankIdi,
                                            //                           NumeroEntrega = e.InventoryInNumber,
                                            //                           GNumeroEntrega = e.InventoryIn,
                                            //                           VolumenPuntoSalida = e.StartVolume,
                                            //                           VolumenEntregado = e.Volume,
                                            //                           VolumenFinal = e.EndVolume,
                                            //                           Temperatura = (e.EndTemperature ?? 0),
                                            //                           PresionAbsoluta = (e.AbsolutePressure ?? 0),
                                            //                           FechaYHoraInicialEntrega = e.Date,
                                            //                           FechaYHoraFinalEntrega = e.Date,
                                            //                           Precio = ed.Price,
                                            //                           Importe = ed.Amount,
                                            //                           PedimentoID = ed.PetitionCustomsId
                                            //                       });

                                            var vQEntregasDatos = (from e in objContext.SaleOrders
                                                                   join ed in objContext.SaleSuborders on new { f1 = e.SaleOrderId } equals new { f1 = ed.SaleOrderId }
                                                                   join a in objContext.Tanks on e.TankIdi equals a.TankIdi
                                                                   where e.StoreId == viNEstacion &&
                                                                        e.StartDate >= dtPerDateIni && e.StartDate <= dtPerDateEnd &&
                                                                        ed.ProductId == vProd.ProductoID
                                                                   select new
                                                                   {
                                                                       TipoTanque = a.SatTankType,
                                                                       TipoMedicion = a.SatTypeMediumStorage,
                                                                       NumeroTanque = (e.TankIdi ?? 0),
                                                                       NumeroEntrega = e.SaleOrderNumber,
                                                                       GNumeroEntrega = e.SaleOrderId,
                                                                       VolumenPuntoSalida = ed.StartQuantity,
                                                                       VolumenEntregado = ed.Quantity,
                                                                       VolumenFinal = ed.EndQuantity,
                                                                       Temperatura = (ed.Temperature ?? 0),
                                                                       PresionAbsoluta = (ed.AbsolutePressure ?? 0),
                                                                       FechaYHoraInicialEntrega = e.Date,
                                                                       FechaYHoraFinalEntrega = e.Date,
                                                                       Precio = ed.Price,
                                                                       Importe = ed.Amount,
                                                                       PedimentoID = ed.PetitionCustomsId
                                                                   });
                                            #endregion

                                            #region Lectura: Entregas.
                                            if (vQEntregasDatos != null)
                                                foreach (var vEntregaDatos in vQEntregasDatos)
                                                {
                                                    #region Entrega: Asignamos Valores.
                                                    int iEntregaNTanque = vEntregaDatos.NumeroTanque,
                                                        iNEntrega = vEntregaDatos.NumeroEntrega.GetValueOrDefault();
                                                    Guid gNEntrega = vEntregaDatos.GNumeroEntrega,
                                                         gPedimentoID = vEntregaDatos.PedimentoID.GetValueOrDefault();
                                                    Decimal dEntregaVolPuntoSalida = vEntregaDatos.VolumenPuntoSalida.GetValueOrDefault(),
                                                            dEntregaVolEntregado = vEntregaDatos.VolumenEntregado.GetValueOrDefault(),
                                                            dEntregaVolFinal = vEntregaDatos.VolumenFinal.GetValueOrDefault(),
                                                            dEntregaTemperatura = vEntregaDatos.Temperatura,
                                                            dEntregaPresionAbs = vEntregaDatos.PresionAbsoluta,
                                                            dEntregaPrecio = vEntregaDatos.Precio.GetValueOrDefault(),
                                                            dEntregaImporte = vEntregaDatos.Importe.GetValueOrDefault();
                                                    DateTime dtEntregaFIni = Convert.ToDateTime(vEntregaDatos.FechaYHoraInicialEntrega),
                                                             dtEntregaFFin = Convert.ToDateTime(vEntregaDatos.FechaYHoraFinalEntrega);
                                                    String sTipoTanque = vEntregaDatos.TipoTanque,
                                                           sTipoMedicion = vEntregaDatos.TipoMedicion,
                                                           sEUnidadMedida = vProd.UnidadMedida;//vEntregaDatos.UnidadMedida;
                                                    #endregion

                                                    // Tipo: Ductos | AutoTanques
                                                    switch (objTipoDistribucion)
                                                    {
                                                        #region Ductos.
                                                        case CVolJSonDTO.eTipoDistribucion.Ductos:
                                                            #region Entrega: Validamos Valores.
                                                            if (String.IsNullOrEmpty(sTipoTanque))
                                                                return BadRequest("El Ducto '" + iEntregaNTanque + "' no contiene la TipoDucto (tb:Tank)");
                                                            #endregion

                                                            String sClaveDucto = "DUC-" + sTipoMedicion + "-" + iEntregaNTanque.ToString("0000");

                                                            #region Datos Ducto.
                                                            int iIdxDucto = -1;
                                                            iIdxDucto = lstProdDuctos.FindIndex(d => d.ClaveIdentificacionDucto.Equals(sClaveDucto));

                                                            // Al no existir datos del Ducto consultamos la informacion par agregarla
                                                            if (iIdxDucto < 0)
                                                            {
                                                                #region Consulta: Ducto Datos.
                                                                var vQDuctoDatos = (from d in objContext.Tanks
                                                                                    where d.StoreId == viNEstacion &&
                                                                                          d.TankIdi == iEntregaNTanque
                                                                                    select new
                                                                                    {
                                                                                        NumeroDucto = d.TankIdi,
                                                                                        ClaveSistemaMedicion = d.SatTypeMediumStorage,
                                                                                        Descripcion = d.Name,
                                                                                        Diametro = d.DiameterOrWidth,
                                                                                        VigenciaCalibracion = d.SatDateCalibration,
                                                                                        IncertidumbreMedicion = (d.SatPercentageUncertaintyMeasurement ?? 0),
                                                                                        UnidadMedida = "UM03",
                                                                                        Volumen = 0
                                                                                    });
                                                                #endregion

                                                                #region Lectura: Ducto Datos.
                                                                if (vQDuctoDatos != null)
                                                                    foreach (var vDuctoDatos in vQDuctoDatos)
                                                                    {
                                                                        #region Ducto Datos: Asignamos Valores.
                                                                        int iDuctoNMedidor = vDuctoDatos.NumeroDucto;
                                                                        Decimal dDuctoMedIncertidumbre = vDuctoDatos.IncertidumbreMedicion;
                                                                        DateTime dtDuctoMedVigencia = Convert.ToDateTime(vDuctoDatos.VigenciaCalibracion);
                                                                        String sDuctoMedClave = vDuctoDatos.ClaveSistemaMedicion,
                                                                               sDuctoDescripcion = vDuctoDatos.Descripcion,
                                                                               sDuctoUnidadMedida = vDuctoDatos.UnidadMedida;
                                                                        Decimal dDuctoDiametro = vDuctoDatos.Diametro.GetValueOrDefault();
                                                                        //dDuctoVolumen = vDuctoDatos.Volumen;
                                                                        #endregion

                                                                        #region Ducto Datos: Validamos Valores.
                                                                        if (String.IsNullOrEmpty(sDuctoDescripcion))
                                                                            return BadRequest("El Ducto '" + iEntregaNTanque + "' no contiene la Descripción (tb:Tank)");
                                                                        else if (sDuctoDescripcion.Length < 2 || sDuctoDescripcion.Length > 250)
                                                                            return BadRequest("El Ducto '" + iEntregaNTanque + "' contiene una Descripción menor a 2 o mayor a 250 caracteres (tb:Tank)");

                                                                        if (dDuctoDiametro < 1 || dDuctoDiametro > 100)
                                                                            return BadRequest("El Ducto '" + iEntregaNTanque + "' contiene un diametro menor a 1 o mayor a 100 (tb:Tank)");
                                                                        #endregion

                                                                        #region Ducto Medidor Datos: Llenado de Estructura.
                                                                        List<CVolJSonDTO.stDuctoMedicionDato> lstDuctMedidores = new List<CVolJSonDTO.stDuctoMedicionDato>();

                                                                        lstDuctMedidores.Add(new CVolJSonDTO.stDuctoMedicionDato
                                                                        {
                                                                            SistemaMedicionDucto = "SMD-DUC-" + sDuctoMedClave + "-" + iDuctoNMedidor.ToString("0000"),
                                                                            LocalizODescripSistMedicionDucto = sDuctoDescripcion,
                                                                            IncertidumbreMedicionSistMedicionDucto = Math.Round(dDuctoMedIncertidumbre, 3),
                                                                            VigenciaCalibracionSistMedicionDucto = dtDuctoMedVigencia.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00",
                                                                        });

                                                                        if (lstDuctMedidores.Count < 0)
                                                                            return BadRequest("El Ducto '" + iEntregaNTanque + "' no contiene medidores.");
                                                                        #endregion

                                                                        CVolJSonDTO.stRecepcionDato objRecepRecepciones = new CVolJSonDTO.stRecepcionDato();
                                                                        CVolJSonDTO.stEntregaDato objRecepEntrega = new CVolJSonDTO.stEntregaDato();

                                                                        lstProdDuctos.Add(new CVolJSonDTO.stDuctoDatos
                                                                        {
                                                                            ClaveIdentificacionDucto = sClaveDucto,
                                                                            DescripcionDucto = sDuctoDescripcion,
                                                                            DiametroDucto = Math.Round(dDuctoDiametro, 3),
                                                                            Medidores = lstDuctMedidores,
                                                                            Recepciones = objRecepRecepciones,
                                                                            ENTREGAS = objRecepEntrega
                                                                        });
                                                                        iTotalDuctos++;
                                                                    }
                                                                #endregion

                                                                iIdxDucto = lstProdDuctos.FindIndex(d => d.ClaveIdentificacionDucto.Equals(sClaveDucto));
                                                            }
                                                            #endregion

                                                            #region Agregamos la Entrega.
                                                            List<CVolJSonDTO.stDuctoEntregaIndividualDato> lstDuctoEntregas = new List<CVolJSonDTO.stDuctoEntregaIndividualDato>();

                                                            CVolJSonDTO.stDuctoDatos objProdDucto = lstProdDuctos[iIdxDucto];
                                                            CVolJSonDTO.stEntregaDato objProdDucEntrega = objProdDucto.ENTREGAS;
                                                            objProdDucEntrega.TotalEntregas++;
                                                            objProdDucEntrega.TotalDocumentos++;

                                                            #region SumaVentas.
                                                            if (objProdDucEntrega.SumaVentas == null)
                                                                objProdDucEntrega.SumaVentas = dEntregaImporte;
                                                            else
                                                                objProdDucEntrega.SumaVentas = dEntregaImporte + (Decimal)objProdDucEntrega.SumaVentas;
                                                            #endregion

                                                            #region SumaVolumenEntregado.
                                                            CVolJSonDTO.stVolumenDato objSumVolEntreganDato;
                                                            if (objProdDucEntrega.SumaVolumenEntregado == null)
                                                                objSumVolEntreganDato = new CVolJSonDTO.stVolumenDato();
                                                            else
                                                                objSumVolEntreganDato = (CVolJSonDTO.stVolumenDato)objProdDucEntrega.SumaVolumenEntregado;

                                                            if (objSumVolEntreganDato.ValorNumerico == null)
                                                            {
                                                                objSumVolEntreganDato.UnidadDeMedida = sEUnidadMedida;
                                                                objSumVolEntreganDato.ValorNumerico = dEntregaVolEntregado;
                                                            }
                                                            else
                                                                objSumVolEntreganDato.ValorNumerico = dEntregaVolEntregado + (Decimal)objSumVolEntreganDato.ValorNumerico;
                                                            objProdDucEntrega.SumaVolumenEntregado = objSumVolEntreganDato;
                                                            #endregion

                                                            if (objProdDucEntrega.Entrega == null)
                                                                lstDuctoEntregas = new List<CVolJSonDTO.stDuctoEntregaIndividualDato>();
                                                            else
                                                                lstDuctoEntregas = (List<CVolJSonDTO.stDuctoEntregaIndividualDato>)objProdDucEntrega.Entrega;

                                                            #region Volumen Punto Salida.
                                                            CVolJSonDTO.stVolumenDato objEntregaVolPuntoSalida = new CVolJSonDTO.stVolumenDato();
                                                            objEntregaVolPuntoSalida.UnidadDeMedida = sEUnidadMedida;
                                                            objEntregaVolPuntoSalida.ValorNumerico = Math.Round(dEntregaVolPuntoSalida, 3);
                                                            #endregion

                                                            #region Volumen Entregado.
                                                            CVolJSonDTO.stVolumenDato objEntregaVolEntregado = new CVolJSonDTO.stVolumenDato();
                                                            objEntregaVolEntregado.UnidadDeMedida = sEUnidadMedida;
                                                            objEntregaVolEntregado.ValorNumerico = Math.Round(dEntregaVolEntregado, 3);
                                                            #endregion

                                                            lstDuctoEntregas.Add(new CVolJSonDTO.stDuctoEntregaIndividualDato
                                                            {
                                                                NumeroDeRegistro = iNEntrega,
                                                                VolumenPuntoSalida = objEntregaVolPuntoSalida,
                                                                VolumenEntregado = objEntregaVolEntregado,
                                                                PresionAbsoluta = dEntregaPresionAbs,
                                                                Temperatura = dEntregaTemperatura,
                                                                FechaYHoraInicialEntrega = dtEntregaFIni.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00",
                                                                FechaYHoraFinalEntrega = dtEntregaFFin.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00"
                                                            });
                                                            #endregion

                                                            objProdDucEntrega.Entrega = lstDuctoEntregas;
                                                            objProdDucto.ENTREGAS = objProdDucEntrega;
                                                            lstProdDuctos[iIdxDucto] = objProdDucto;

                                                            objProductoDato.Ducto = lstProdDuctos;
                                                            break;
                                                        #endregion

                                                        #region Autotanques.
                                                        case CVolJSonDTO.eTipoDistribucion.Autotanques:
                                                            CVolJSonDTO.stTanqueDatos objEntAutoTanqDatos;

                                                            #region Entrega: Validamos Valores.
                                                            if (String.IsNullOrEmpty(sTipoTanque))
                                                                return BadRequest("El AutoTanque '" + iEntregaNTanque.ToString() + "' no contiene la Tipo de Tanque (tb:Tank)");
                                                            #endregion

                                                            String sClaveAutoTanq = sTipoTanque + "-" + sNomClaveInstalacion + "-" + iEntregaNTanque.ToString("0000");

                                                            #region AutoTanque Datos.
                                                            int iIdxAutoTanq = -1;
                                                            iIdxAutoTanq = lstProdAutoTanques.FindIndex(d => d.ClaveIdentificacionTanque.Equals(sClaveAutoTanq));

                                                            // Al no existir datos del Ducto consultamos la informacion par agregarla
                                                            if (iIdxAutoTanq < 0)
                                                            {
                                                                #region Agregamos los datos del AutoTanque.
                                                                #region Consulta: AutoTanque Datos.
                                                                var vQAutoTanqueDatos = (from a in objContext.Tanks
                                                                                         where a.StoreId == viNEstacion &&
                                                                                               a.TankIdi == iEntregaNTanque
                                                                                         //&& a.TankTypeId == "" //<*DUDA*> hasta que se defina el código de tipo de tanque "AutoTanque"
                                                                                         select new
                                                                                         {
                                                                                             TipoTanque = a.SatTankType,
                                                                                             Descripcion = a.Name,
                                                                                             CapacidadTotal = a.CapacityTotal,
                                                                                             CapacidadOperativa = a.CapacityOperational,
                                                                                             CapacidadUtil = a.CapacityUseful,
                                                                                             CapacidadFondaje = a.Fondage,
                                                                                             CapacidadGasTalon = a.CapacityGastalon,
                                                                                             VolumenMinimoOperacion = a.CapacityMinimumOperating,
                                                                                             EstadoTanque = a.Active,
                                                                                             TipoSistMedicion = a.SatTypeMeasurement,
                                                                                             LocalizODescripSistMedicion = a.SatDescriptionMeasurement,
                                                                                             IncertidumbreMedicionSistMedicion = a.SatPercentageUncertaintyMeasurement,
                                                                                             TipoMedioAlmacenamiento = a.SatTypeMediumStorage,
                                                                                             VigenciaCalibracionSistMedicion = a.SatDateCalibration,
                                                                                             RecepcionesAnt = (from r in objContext.InventoryIns
                                                                                                               where r.StoreId == viNEstacion &&
                                                                                                                     r.TankIdi == a.TankIdi &&
                                                                                                                     r.EndDate >= dtPerDateIni.AddDays(-1) &&
                                                                                                                     r.EndDate <= dtPerDateEnd.AddDays(-1) &&
                                                                                                                     r.ProductId == vProd.ProductoID
                                                                                                               select new { r.Volume }).Sum(r => r.Volume),
                                                                                             EntregasAnt = (from e in objContext.SaleOrders
                                                                                                            join ed in objContext.SaleSuborders on e.SaleOrderId equals ed.SaleOrderId
                                                                                                            where e.StoreId == viNEstacion &&
                                                                                                                  e.TankIdi == a.TankIdi &&
                                                                                                                  e.Date >= dtPerDateIni.AddDays(-1) &&
                                                                                                                  e.Date <= dtPerDateEnd.AddDays(-1) &&
                                                                                                                  ed.ProductId == vProd.ProductoID
                                                                                                            select new { ed.Quantity }).Sum(r => r.Quantity)
                                                                                         });
                                                                #endregion

                                                                #region Lectura: AutoTanque Datos.
                                                                if (vQAutoTanqueDatos != null)
                                                                    foreach (var vAutoTanqueDatos in vQAutoTanqueDatos)
                                                                    {
                                                                        #region AutoTanque: Asignamos Valores.
                                                                        int iCapTotal = vAutoTanqueDatos.CapacidadTotal.GetValueOrDefault(),
                                                                            iCapOperativa = vAutoTanqueDatos.CapacidadOperativa.GetValueOrDefault(),
                                                                            iCapUtil = vAutoTanqueDatos.CapacidadUtil.GetValueOrDefault(),
                                                                            iCapFondaje = vAutoTanqueDatos.CapacidadFondaje.GetValueOrDefault(),
                                                                            iVolMinOperacion = vAutoTanqueDatos.VolumenMinimoOperacion.GetValueOrDefault(),
                                                                            iTanqueIncertidumbre = vAutoTanqueDatos.IncertidumbreMedicionSistMedicion.GetValueOrDefault();
                                                                        String sDescripcionSAT = vAutoTanqueDatos.Descripcion;
                                                                        DateTime dtTanqueFCalibracion = Convert.ToDateTime(vAutoTanqueDatos.VigenciaCalibracionSistMedicion);
                                                                        String sTipoAutoTanq = vAutoTanqueDatos.TipoTanque,
                                                                               sTanqueUniMed = vProd.UnidadMedida,//vAutoTanqueDatos.UnidadMedida,
                                                                               sTanqueTipoMedicion = vAutoTanqueDatos.TipoSistMedicion,
                                                                               sTanqueDescMedicion = vAutoTanqueDatos.LocalizODescripSistMedicion,
                                                                               sTanqueTipoMedAlmacenamiento = vAutoTanqueDatos.TipoMedioAlmacenamiento;
                                                                        Boolean sTanqueStatus = vAutoTanqueDatos.EstadoTanque.GetValueOrDefault();
                                                                        Decimal dRecepcionesAutTanqAnt = vAutoTanqueDatos.RecepcionesAnt.GetValueOrDefault(),
                                                                                dEntregasAutTanqAnt = vAutoTanqueDatos.EntregasAnt.GetValueOrDefault();
                                                                        #endregion

                                                                        #region AutoTanque: Validamos Valores.
                                                                        if (String.IsNullOrEmpty(sDescripcionSAT))
                                                                            return BadRequest("El AutoTanque no contiene la Descripción");

                                                                        if (String.IsNullOrEmpty(sTanqueTipoMedicion))
                                                                            return BadRequest("El AutoTanque no contiene un Tipo de Medición");

                                                                        if (String.IsNullOrEmpty(sTanqueTipoMedAlmacenamiento))
                                                                            return BadRequest("El AutoTanque no contiene un Tipo de Medio de Almacenamiento");
                                                                        else if (sTanqueTipoMedAlmacenamiento.Equals("0"))
                                                                            return BadRequest("El AutoTanque contiene un Tipo de Medio de Almacenamiento incorrecto '" + sTanqueTipoMedAlmacenamiento + "'");

                                                                        // sTanqueDescMedicion
                                                                        if (String.IsNullOrEmpty(sTanqueDescMedicion))
                                                                            return BadRequest("El AutoTanque no contiene la Descripción en la Medición");
                                                                        else if (sTanqueDescMedicion.Length < 2)
                                                                            return BadRequest("La Descripción de la Medición debe ser minimo de 2 caracteres");
                                                                        #endregion

                                                                        #region AutoTanque: Llenado de Estructura.
                                                                        CVolJSonDTO.stTanqueDatos objAutoTanqueNew = new CVolJSonDTO.stTanqueDatos();

                                                                        objAutoTanqueNew.ClaveIdentificacionTanque = sClaveAutoTanq;
                                                                        objAutoTanqueNew.LocalizacionYODescripcionTanque = sDescripcionSAT;
                                                                        objAutoTanqueNew.VigenciaCalibracionTanque = dtTanqueFCalibracion.ToString("yyyy-MM-dd");

                                                                        #region Capacidad Total Tanque.
                                                                        CVolJSonDTO.stCapacidadDato objCapacidadTotalDato = new CVolJSonDTO.stCapacidadDato();
                                                                        objCapacidadTotalDato.ValorNumerico = iCapTotal;
                                                                        objCapacidadTotalDato.UnidadDeMedida = sTanqueUniMed;

                                                                        objAutoTanqueNew.CapacidadTotalTanque = objCapacidadTotalDato;
                                                                        #endregion

                                                                        #region Capacidad Operativa.
                                                                        CVolJSonDTO.stCapacidadDato objCapOperativaDato = new CVolJSonDTO.stCapacidadDato();
                                                                        objCapOperativaDato.ValorNumerico = iCapOperativa;
                                                                        objCapOperativaDato.UnidadDeMedida = sTanqueUniMed;

                                                                        objAutoTanqueNew.CapacidadOperativaTanque = objCapOperativaDato;
                                                                        #endregion

                                                                        #region Capacidad Util.
                                                                        CVolJSonDTO.stCapacidadDato objCapUtilDato = new CVolJSonDTO.stCapacidadDato();
                                                                        objCapUtilDato.ValorNumerico = iCapUtil;
                                                                        objCapUtilDato.UnidadDeMedida = sTanqueUniMed;

                                                                        objAutoTanqueNew.CapacidadUtilTanque = objCapUtilDato;
                                                                        #endregion

                                                                        #region Capacidad Fondaje.
                                                                        CVolJSonDTO.stCapacidadDato objCapFondaje = new CVolJSonDTO.stCapacidadDato();
                                                                        objCapFondaje.ValorNumerico = iCapFondaje;
                                                                        objCapFondaje.UnidadDeMedida = sTanqueUniMed;

                                                                        objAutoTanqueNew.CapacidadFondajeTanque = objCapFondaje;
                                                                        #endregion

                                                                        #region Volumen Minimo Operación.
                                                                        CVolJSonDTO.stCapacidadDato objVolMinOperDato = new CVolJSonDTO.stCapacidadDato();
                                                                        objVolMinOperDato.ValorNumerico = iVolMinOperacion;
                                                                        objVolMinOperDato.UnidadDeMedida = sTanqueUniMed;

                                                                        objAutoTanqueNew.VolumenMinimoOperacion = objVolMinOperDato;
                                                                        #endregion

                                                                        #region Estado Tanque.
                                                                        objAutoTanqueNew.EstadoTanque = "F";
                                                                        if (sTanqueStatus)
                                                                            objAutoTanqueNew.EstadoTanque = "O";
                                                                        #endregion

                                                                        #region Medición de Tanques.
                                                                        Decimal dTanqIncertMed = 0;
                                                                        if (iTanqueIncertidumbre > 0)
                                                                            dTanqIncertMed = (iTanqueIncertidumbre / 100);

                                                                        String sSistMedicion = "";
                                                                        switch (sTanqueTipoMedicion)
                                                                        {
                                                                            case "SMD": // SMD-ETA-TQS-USP-0026.
                                                                                sSistMedicion = sTanqueTipoMedicion + "-" +
                                                                                                sTanqueTipoMedAlmacenamiento + "-" +
                                                                                                sTipoAutoTanq + "-" +
                                                                                                sNomClaveInstalacion + "-" +
                                                                                                iEntregaNTanque.ToString().PadLeft(4, '0');
                                                                                break;

                                                                            case "SME": // SME-STQ-EDS-0021.
                                                                                sSistMedicion = sTanqueTipoMedicion + "-" +
                                                                                                sTipoAutoTanq + "-" +
                                                                                                sNomClaveInstalacion + "-" +
                                                                                                iEntregaNTanque.ToString().PadLeft(4, '0');
                                                                                break;

                                                                            default:
                                                                                sSistMedicion = sTanqueTipoMedicion + "-" +
                                                                                                sTipoAutoTanq + "-" +
                                                                                                sNomClaveInstalacion + "-" +
                                                                                                iEntregaNTanque.ToString().PadLeft(4, '0');
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

                                                                        objAutoTanqueNew.Medidores = lstMedidores;
                                                                        #endregion

                                                                        #region Existencias.
                                                                        CVolJSonDTO.stExistenciaDato objRecepExistenciaDatos = new CVolJSonDTO.stExistenciaDato();

                                                                        objRecepExistenciaDatos.VolumenExistenciasAnterior = 0;

                                                                        #region VolumenAcumOpsRecepcion.
                                                                        CVolJSonDTO.stVolumenDato objRecepVolAntDato = new CVolJSonDTO.stVolumenDato();
                                                                        objRecepVolAntDato.UnidadDeMedida = sTanqueUniMed;
                                                                        objRecepVolAntDato.ValorNumerico = Math.Round(dRecepcionesAutTanqAnt, 3);
                                                                        objRecepExistenciaDatos.VolumenAcumOpsRecepcion = objRecepVolAntDato;
                                                                        #endregion

                                                                        objRecepExistenciaDatos.HoraRecepcionAcumulado = dtFechaRegistro.ToString("HH:mm:ss") + "-" +
                                                                                                                         iDiferenciaHora.ToString("00") + ":00";

                                                                        #region VolumenAcumOpsEntrega.
                                                                        CVolJSonDTO.stCapacidadDato objEntVolAntDato = new CVolJSonDTO.stCapacidadDato();
                                                                        objEntVolAntDato.UnidadDeMedida = sTanqueUniMed;
                                                                        objEntVolAntDato.ValorNumerico = Math.Round(dEntregasAutTanqAnt, 3);
                                                                        objRecepExistenciaDatos.VolumenAcumOpsEntrega = objEntVolAntDato;
                                                                        #endregion

                                                                        objRecepExistenciaDatos.HoraEntregaAcumulado = dtFechaRegistro.ToString("HH:mm:ss") + "-" +
                                                                                                                       iDiferenciaHora.ToString("00") + ":00";
                                                                        objRecepExistenciaDatos.VolumenExistencias = Math.Round(dRecepcionesAutTanqAnt - dEntregasAutTanqAnt, 3);
                                                                        objRecepExistenciaDatos.FechaYHoraEstaMedicion = dtPerDateEnd.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00";
                                                                        objRecepExistenciaDatos.FechaYHoraMedicionAnterior = dtPerDateIni.AddSeconds(-1).ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00";

                                                                        objAutoTanqueNew.Existencias = objRecepExistenciaDatos;
                                                                        #endregion

                                                                        #region Inicializacion del Nodo: Recepciones.
                                                                        CVolJSonDTO.stRecepcionDato objRecepcionDatoNew = new CVolJSonDTO.stRecepcionDato();

                                                                        #region Suma Volumen Recepcion.
                                                                        CVolJSonDTO.stVolumenDato objSumaVolRecepNew = new CVolJSonDTO.stVolumenDato();
                                                                        objSumaVolRecepNew.UnidadDeMedida = sTanqueUniMed;
                                                                        objSumaVolRecepNew.ValorNumerico = 0;

                                                                        objRecepcionDatoNew.SumaVolumenRecepcion = objSumaVolRecepNew;
                                                                        #endregion

                                                                        #region Recepciones.
                                                                        List<CVolJSonDTO.stRecepcionIndividualDato> lstRecepcionesNew = new List<CVolJSonDTO.stRecepcionIndividualDato>();
                                                                        objRecepcionDatoNew.Recepcion = lstRecepcionesNew;
                                                                        #endregion

                                                                        objRecepcionDatoNew.SumaCompras = 0;
                                                                        objAutoTanqueNew.Recepciones = objRecepcionDatoNew;
                                                                        #endregion

                                                                        #region Inicializacion del Nodo:Entregas.
                                                                        CVolJSonDTO.stEntregaDato objEntregaDatoNew = new CVolJSonDTO.stEntregaDato();

                                                                        #region Suma Ventas.
                                                                        objEntregaDatoNew.SumaVentas = 0;
                                                                        #endregion

                                                                        #region Suma Volumen Entregado.
                                                                        CVolJSonDTO.stVolumenDato objSumaVolEntregadoNew = new CVolJSonDTO.stVolumenDato();
                                                                        objSumaVolEntregadoNew.UnidadDeMedida = sTanqueUniMed;
                                                                        objSumaVolEntregadoNew.ValorNumerico = 0;

                                                                        objEntregaDatoNew.SumaVolumenEntregado = objSumaVolEntregadoNew;
                                                                        #endregion

                                                                        #region Entregas.
                                                                        objEntregaDatoNew.Entrega = null;
                                                                        #endregion

                                                                        objAutoTanqueNew.Entregas = objEntregaDatoNew;
                                                                        #endregion

                                                                        lstProdAutoTanques.Add(objAutoTanqueNew);
                                                                        iTotalTanques++;
                                                                        #endregion
                                                                    }
                                                                #endregion
                                                                #endregion

                                                                iIdxAutoTanq = lstProdAutoTanques.FindIndex(d => d.ClaveIdentificacionTanque.Equals(sClaveAutoTanq));
                                                            }
                                                            #endregion

                                                            #region Llenado: Entrega.
                                                            // Obtenemos el Nodo:Entregas del AutoTanque
                                                            objEntAutoTanqDatos = lstProdAutoTanques[iIdxAutoTanq];
                                                            CVolJSonDTO.stEntregaDato objAutoTanqEntrega = (CVolJSonDTO.stEntregaDato)objEntAutoTanqDatos.Entregas;
                                                            // Obtenemos la lista de Entregas del Tanque.

                                                            List<CVolJSonDTO.stEntregaIndividualDato> lstAutoTanqEntregas = new List<CVolJSonDTO.stEntregaIndividualDato>();
                                                            if (objAutoTanqEntrega.Entrega != null)
                                                                lstAutoTanqEntregas = (List<CVolJSonDTO.stEntregaIndividualDato>)objAutoTanqEntrega.Entrega;

                                                            #region Volumen Inicial Dato.
                                                            CVolJSonDTO.stCapacidadDato objEntVolIni = new CVolJSonDTO.stCapacidadDato();
                                                            objEntVolIni.ValorNumerico = dEntregaVolPuntoSalida;
                                                            objEntVolIni.UnidadDeMedida = sEUnidadMedida;
                                                            #endregion

                                                            #region Volumen Final Dato.
                                                            CVolJSonDTO.stCapacidadDato objEntVolFin = new CVolJSonDTO.stCapacidadDato();
                                                            objEntVolFin.UnidadDeMedida = sEUnidadMedida;
                                                            objEntVolFin.ValorNumerico = dEntregaVolFinal;
                                                            #endregion

                                                            #region Volumen Entregado.
                                                            CVolJSonDTO.stCapacidadDato objEntVolumen = new CVolJSonDTO.stCapacidadDato();
                                                            objEntVolumen.ValorNumerico = dEntregaVolEntregado;
                                                            objEntVolumen.UnidadDeMedida = sEUnidadMedida;
                                                            #endregion

                                                            #region Complemento.
                                                            Object oEntregaComplemento = null;

                                                            // ***  Indica si se genera el Complemento de Entregas  ***
                                                            if (bCompEntregas)
                                                            {
                                                                List<CVolJSonDTO.stComplementoDistribucion> lstEntComplementoDistribucion = new List<CVolJSonDTO.stComplementoDistribucion>();
                                                                List<CVolJSonDTO.stComplementoTransportista> lstEntComplementoTransportista = new List<CVolJSonDTO.stComplementoTransportista>();
                                                                List<CVolJSonDTO.stComplementoComercializadora> lstEntComplementoComercializadora = new List<CVolJSonDTO.stComplementoComercializadora>();

                                                                CVolJSonDTO.stComplementoDistribucion objEntComplementoDistribuidor = new CVolJSonDTO.stComplementoDistribucion();
                                                                CVolJSonDTO.stComplementoTransportista objEntComplementoTransportista = new CVolJSonDTO.stComplementoTransportista();
                                                                CVolJSonDTO.stComplementoComercializadora objEntComplementoComercializadora = new CVolJSonDTO.stComplementoComercializadora();

                                                                #region NODO: Nacional.
                                                                List<CVolJSonDTO.stCompleDistNacional> lstEntDistNacional = new List<CVolJSonDTO.stCompleDistNacional>();
                                                                List<CVolJSonDTO.stComplementoTransNacional> lstEntTransNacional = new List<CVolJSonDTO.stComplementoTransNacional>();
                                                                List<CVolJSonDTO.stCompleComerNacional> lstEntComerNacional = new List<CVolJSonDTO.stCompleComerNacional>();

                                                                #region Consulta: CFDI Datos.
                                                                var vQEntregaCfdiDato = (from tf in objContext.InvoiceSaleOrders
                                                                                         join fh in objContext.Invoices on tf.InvoiceId equals fh.InvoiceId
                                                                                         join fd in objContext.InvoiceDetails on fh.InvoiceId equals fd.InvoiceId
                                                                                         join c in objContext.Customers on fh.CustomerId equals c.CustomerId
                                                                                         join t in objContext.SaleOrders on tf.SaleOrderId equals t.SaleOrderId
                                                                                         join td in objContext.SaleSuborders on t.SaleOrderId equals td.SaleOrderId
                                                                                         join tc in objContext.SatTipoComprobantes on fh.SatTipoComprobanteId equals tc.SatTipoComprobanteId
                                                                                         join tr in objContext.SupplierTransportRegisters on td.SupplierTransportRegisterId.GetValueOrDefault() equals tr.SupplierTransportRegisterId into rtr
                                                                                         from tr in rtr.DefaultIfEmpty()
                                                                                         where tf.SaleOrderId == Guid.Parse("7ECC6829-BDEA-4882-8939-DF65EFE9B6C2") &&
                                                                                               fh.StoreId == viNEstacion
                                                                                         select new
                                                                                         {
                                                                                             NumeroEntrega = t.SaleOrderNumber,
                                                                                             ClienteRFC = c.Rfc,
                                                                                             NombreCliente = c.Name,
                                                                                             NumeroPermisoCRE = c.CustomerPermission,
                                                                                             TipoCfdiID = fh.SatTipoComprobanteId,
                                                                                             TipoCFDI = tc.Descripcion,
                                                                                             CFDI = fh.Uuid,
                                                                                             Precio = fd.Price,
                                                                                             PrecioVtaPublico = fd.Price,
                                                                                             FechaHora = fh.Date,
                                                                                             Volumen = fd.Quantity,
                                                                                             UnidadMedida = "UM03",
                                                                                             ContraPrestacion = (tr.AmountPerService.ToString() ?? "0"),
                                                                                             TarifaTransporte = (tr.AmountPerFee.ToString() ?? "0"),
                                                                                             CargoCapacidadTrans = (tr.AmountPerCapacity.ToString() ?? "0"),
                                                                                             CargoUsoTrans = (tr.AmountPerUse.ToString() ?? "0"),
                                                                                             CargoVolumenTrans = (tr.AmountPerVolume.ToString() ?? "0"),
                                                                                             Descuento = (tr.AmountOfDiscount.ToString() ?? "0")
                                                                                         });
                                                                #endregion

                                                                #region Lectura: CFDI Datos.
                                                                if (vQEntregaCfdiDato != null)
                                                                    foreach (var vCfdiDatos in vQEntregaCfdiDato)
                                                                    {
                                                                        #region CFDI: Asignamos Valores.
                                                                        String sNacClienteRFC = vCfdiDatos.ClienteRFC,
                                                                               sNacNombreCliente = vCfdiDatos.NombreCliente,
                                                                               sNacPermisoCRE = vCfdiDatos.NumeroPermisoCRE,
                                                                               sNacTipoCfdiID = vCfdiDatos.TipoCfdiID,
                                                                               sNacTipoCFDI = vCfdiDatos.TipoCFDI,
                                                                               sNacCFDI = vCfdiDatos.CFDI,
                                                                               sNacUnidadMedida = vCfdiDatos.UnidadMedida;
                                                                        Decimal dNacPrecioCompra = vCfdiDatos.Precio.GetValueOrDefault(),
                                                                                dNacPrecioVtaPublico = vCfdiDatos.PrecioVtaPublico.GetValueOrDefault(),
                                                                                dNacVolumen = vCfdiDatos.Volumen.GetValueOrDefault(),
                                                                                dNacContraPrestacion = Convert.ToDecimal(vCfdiDatos.ContraPrestacion),
                                                                                dNacDescuento = Convert.ToDecimal(vCfdiDatos.Descuento),
                                                                                dNacTarifaTrans = Convert.ToDecimal(vCfdiDatos.TarifaTransporte),
                                                                                dNacCargoCapTrans = Convert.ToDecimal(vCfdiDatos.CargoCapacidadTrans),
                                                                                dNacCargoUsoTrans = Convert.ToDecimal(vCfdiDatos.CargoUsoTrans),
                                                                                dNacCargoVolTrans = Convert.ToDecimal(vCfdiDatos.CargoVolumenTrans);
                                                                        DateTime dtNacFechaHora = Convert.ToDateTime(vCfdiDatos.FechaHora);
                                                                        Int32 iNumeroEntrega = vCfdiDatos.NumeroEntrega.GetValueOrDefault();
                                                                        #endregion

                                                                        #region CFDI: Validamos Valores.
                                                                        if (!sNacTipoCfdiID.ToUpper().Equals("I") && !sNacTipoCfdiID.ToUpper().Equals("E") && !sNacTipoCfdiID.ToUpper().Equals("T"))
                                                                            return BadRequest("Tipo de CFDI no valido '" + sNacTipoCFDI + "'");

                                                                        if (!ValidarCFDI(sNacCFDI))
                                                                            return BadRequest("La Entrega '" + iNEntrega.ToString() + "' contiene un CFDI con formato incorrecto '" + sNacCFDI + "'.");
                                                                        #endregion

                                                                        #region CFDI: Llenado de Estructura.
                                                                        int iIdxCliente = -1;

                                                                        switch (objTipoComplemento)
                                                                        {
                                                                            #region Distribuidor.
                                                                            case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                                                iIdxCliente = lstEntDistNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));

                                                                                CVolJSonDTO.stCompleDistNacional objEntDistNacionalDato;
                                                                                if (iIdxCliente < 0)
                                                                                {
                                                                                    objEntDistNacionalDato = new CVolJSonDTO.stCompleDistNacional();
                                                                                    objEntDistNacionalDato.RfcClienteOProveedor = sNacClienteRFC;
                                                                                    objEntDistNacionalDato.NombreClienteOProveedor = sNacNombreCliente;

                                                                                    if (!String.IsNullOrEmpty(sNacPermisoCRE))
                                                                                        objEntDistNacionalDato.PermisoClienteOProveedor = sNacPermisoCRE;

                                                                                    List<CVolJSonDTO.stCompleDistNacionalCfdis> lstCFDIsNew = new List<CVolJSonDTO.stCompleDistNacionalCfdis>();
                                                                                    objEntDistNacionalDato.CFDIs = lstCFDIsNew;

                                                                                    lstEntDistNacional.Add(objEntDistNacionalDato);
                                                                                    iIdxCliente = lstEntDistNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));
                                                                                }

                                                                                #region CFDI Datos.
                                                                                CVolJSonDTO.stCompleDistNacionalCfdis objDistCFDI_Dato = new CVolJSonDTO.stCompleDistNacionalCfdis();
                                                                                objDistCFDI_Dato.Cfdi = sNacCFDI;
                                                                                objDistCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                                objDistCFDI_Dato.PrecioVentaOCompraOContrap = Math.Round(dNacPrecioCompra, 3);
                                                                                objDistCFDI_Dato.FechaYHoraTransaccion = dtNacFechaHora.ToString("s") + "-" +
                                                                                                                         iDiferenciaHora.ToString("00") + ":00";
                                                                                #region VolumenDocumentado.
                                                                                CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                                objVolumenDocumentado.ValorNumerico = Math.Round(dNacVolumen, 3);
                                                                                objVolumenDocumentado.UnidadDeMedida = sNacUnidadMedida;

                                                                                objDistCFDI_Dato.VolumenDocumentado = objVolumenDocumentado;
                                                                                #endregion
                                                                                #endregion

                                                                                objEntDistNacionalDato = (CVolJSonDTO.stCompleDistNacional)lstEntDistNacional[iIdxCliente];
                                                                                List<CVolJSonDTO.stCompleDistNacionalCfdis> lstCfDIs = (List<CVolJSonDTO.stCompleDistNacionalCfdis>)objEntDistNacionalDato.CFDIs;
                                                                                lstCfDIs.Add(objDistCFDI_Dato);
                                                                                objEntDistNacionalDato.CFDIs = lstCfDIs;
                                                                                lstEntDistNacional[iIdxCliente] = objEntDistNacionalDato;
                                                                                break;
                                                                            #endregion

                                                                            #region Transportista.
                                                                            case CVolJSonDTO.eTipoComplemento.Transportista:
                                                                                iIdxCliente = lstEntTransNacional.FindIndex(n => n.RfcCliente.Equals(sNacClienteRFC));

                                                                                CVolJSonDTO.stComplementoTransNacional objEntTransNacionalDato;
                                                                                if (iIdxCliente < 0)
                                                                                {
                                                                                    objEntTransNacionalDato = new CVolJSonDTO.stComplementoTransNacional();
                                                                                    objEntTransNacionalDato.RfcCliente = sNacClienteRFC;
                                                                                    objEntTransNacionalDato.NombreCliente = sNacNombreCliente;

                                                                                    List<CVolJSonDTO.stCompleTransNacionalCfdis> lstCFDIsNew = new List<CVolJSonDTO.stCompleTransNacionalCfdis>();
                                                                                    objEntTransNacionalDato.CFDIs = lstCFDIsNew;

                                                                                    lstEntTransNacional.Add(objEntTransNacionalDato);
                                                                                    iIdxCliente = lstEntTransNacional.FindIndex(n => n.RfcCliente.Equals(sNacClienteRFC));
                                                                                }

                                                                                #region CFDI Datos.
                                                                                CVolJSonDTO.stCompleTransNacionalCfdis objTransCFDI_Dato = new CVolJSonDTO.stCompleTransNacionalCfdis();
                                                                                objTransCFDI_Dato.Cfdi = sNacCFDI;
                                                                                objTransCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                                objTransCFDI_Dato.Contraprestacion = dNacContraPrestacion;
                                                                                objTransCFDI_Dato.TarifaDeTransporte = dNacTarifaTrans;
                                                                                objTransCFDI_Dato.FechaYHoraTransaccion = dtNacFechaHora.ToString("s") + "-" +
                                                                                                                          iDiferenciaHora.ToString("00") + ":00";
                                                                                #region VolumenDocumentado.
                                                                                CVolJSonDTO.stVolumenDato objEntTransVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                                objEntTransVolumenDocumentado.ValorNumerico = Math.Round(dNacVolumen, 3);
                                                                                objEntTransVolumenDocumentado.UnidadDeMedida = sNacUnidadMedida;

                                                                                objTransCFDI_Dato.VolumenDocumentado = objEntTransVolumenDocumentado;
                                                                                #endregion

                                                                                if (dNacCargoCapTrans > 0)
                                                                                    objTransCFDI_Dato.CargoPorCapacidadDeTrans = dNacCargoCapTrans;

                                                                                if (dNacCargoUsoTrans > 0)
                                                                                    objTransCFDI_Dato.CargoPorUsoTrans = dNacCargoUsoTrans;

                                                                                if (dNacCargoVolTrans > 0)
                                                                                    objTransCFDI_Dato.CargoVolumetricoTrans = dNacCargoVolTrans;

                                                                                if (dNacDescuento > 0)
                                                                                    objTransCFDI_Dato.Descuento = dNacDescuento;
                                                                                #endregion

                                                                                objEntTransNacionalDato = (CVolJSonDTO.stComplementoTransNacional)lstEntTransNacional[iIdxCliente];
                                                                                List<CVolJSonDTO.stCompleTransNacionalCfdis> lstEntTransCfDIs = (List<CVolJSonDTO.stCompleTransNacionalCfdis>)objEntTransNacionalDato.CFDIs;
                                                                                lstEntTransCfDIs.Add(objTransCFDI_Dato);
                                                                                objEntTransNacionalDato.CFDIs = lstEntTransCfDIs;
                                                                                lstEntTransNacional[iIdxCliente] = objEntTransNacionalDato;
                                                                                break;
                                                                            #endregion

                                                                            #region Comercializadora.
                                                                            case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                                                iIdxCliente = lstEntComerNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));

                                                                                CVolJSonDTO.stCompleComerNacional objEntComerNacionalDato;
                                                                                if (iIdxCliente < 0)
                                                                                {
                                                                                    objEntComerNacionalDato = new CVolJSonDTO.stCompleComerNacional();
                                                                                    objEntComerNacionalDato.RfcClienteOProveedor = sNacClienteRFC;
                                                                                    objEntComerNacionalDato.NombreClienteOProveedor = sNacNombreCliente;

                                                                                    if (!String.IsNullOrEmpty(sNacPermisoCRE))
                                                                                        objEntComerNacionalDato.PermisoProveedor = sNacPermisoCRE;

                                                                                    List<CVolJSonDTO.stCompleComerNacionalCfdis> lstCFDIsNew = new List<CVolJSonDTO.stCompleComerNacionalCfdis>();
                                                                                    objEntComerNacionalDato.CFDIs = lstCFDIsNew;

                                                                                    lstEntComerNacional.Add(objEntComerNacionalDato);
                                                                                    iIdxCliente = lstEntComerNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));
                                                                                }

                                                                                #region CFDI Datos.
                                                                                CVolJSonDTO.stCompleComerNacionalCfdis objComerCFDI_Dato = new CVolJSonDTO.stCompleComerNacionalCfdis();
                                                                                objComerCFDI_Dato.Cfdi = sNacCFDI;
                                                                                objComerCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                                objComerCFDI_Dato.PrecioCompra = Math.Round(dNacPrecioCompra, 3);
                                                                                objComerCFDI_Dato.PrecioDeVentaAlPublico = Math.Round(dNacPrecioVtaPublico, 3);
                                                                                objComerCFDI_Dato.FechayHoraTransaccion = dtNacFechaHora.ToString("s") + "-" +
                                                                                                                          iDiferenciaHora.ToString("00") + ":00";
                                                                                #region VolumenDocumentado.
                                                                                CVolJSonDTO.stVolumenDato objEntComerVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                                objEntComerVolumenDocumentado.ValorNumerico = Math.Round(dNacVolumen, 3);
                                                                                objEntComerVolumenDocumentado.UnidadDeMedida = sNacUnidadMedida;

                                                                                objComerCFDI_Dato.VolumenDocumentado = objEntComerVolumenDocumentado;
                                                                                #endregion
                                                                                #endregion

                                                                                objEntComerNacionalDato = (CVolJSonDTO.stCompleComerNacional)lstEntComerNacional[iIdxCliente];
                                                                                List<CVolJSonDTO.stCompleComerNacionalCfdis> lstEntComerCfDIs = (List<CVolJSonDTO.stCompleComerNacionalCfdis>)objEntComerNacionalDato.CFDIs;
                                                                                lstEntComerCfDIs.Add(objComerCFDI_Dato);
                                                                                objEntComerNacionalDato.CFDIs = lstEntComerCfDIs;
                                                                                lstEntComerNacional[iIdxCliente] = objEntComerNacionalDato;
                                                                                break;
                                                                                #endregion
                                                                        }
                                                                        #endregion
                                                                    }
                                                                #endregion
                                                                #endregion

                                                                #region NODO: Extrajero.
                                                                List<CVolJSonDTO.stCompleDistExtranjero> lstEntDistExtranjero = new List<CVolJSonDTO.stCompleDistExtranjero>();
                                                                List<CVolJSonDTO.stCompleComerExtranjero> lstEntComerExtranjero = new List<CVolJSonDTO.stCompleComerExtranjero>();

                                                                #region Consulta: Pedimento Datos.
                                                                var vQEntregaPedimentoDato = (from p in objContext.PetitionCustoms
                                                                                              join t in objContext.TransportMediumnCustoms on p.TransportMediumnCustomsId equals t.TransportMediumnCustomsId
                                                                                              where p.PetitionCustomsId == gPedimentoID
                                                                                              select new
                                                                                              {
                                                                                                  ClavePermisoImportOExport = p.KeyOfImportationExportation,
                                                                                                  PuntoInternacionOExtracccion = p.KeyPointOfInletOrOulet,
                                                                                                  Pais = (p.SatPaisId ?? String.Empty),
                                                                                                  MedioIngresoOSalida = (t.TransportMediumn ?? "0"),
                                                                                                  ClavePedimento = (p.NumberCustomsDeclaration ?? String.Empty),
                                                                                                  Incoterms = (p.Incoterms ?? String.Empty),
                                                                                                  PrecioDeImportOExport = p.AmountOfImportationExportation,
                                                                                                  Volumen = p.QuantityDocumented,
                                                                                                  UnidadMedida = (p.JsonClaveUnidadMedidadId ?? sEUnidadMedida)
                                                                                              });
                                                                #endregion

                                                                #region Lectura: Datos Pedimentos.
                                                                if (vQEntregaPedimentoDato != null)
                                                                    foreach (var vPedimentoDato in vQEntregaPedimentoDato)
                                                                    {
                                                                        #region Pedimento: Asignamos Valores.
                                                                        String sExtClavePermiso = vPedimentoDato.ClavePermisoImportOExport,
                                                                               sExtPais = vPedimentoDato.Pais,
                                                                               sExtClavePedimento = vPedimentoDato.ClavePedimento,
                                                                               sExtIncoterms = vPedimentoDato.Incoterms,
                                                                               sExtUnidadMedida = vPedimentoDato.UnidadMedida;
                                                                        int iExtPuntoInterOExtra = Convert.ToInt32(vPedimentoDato.PuntoInternacionOExtracccion),
                                                                            iExtMedioIngOSal = Convert.ToInt32(vPedimentoDato.MedioIngresoOSalida);
                                                                        Decimal dExtImporte = vPedimentoDato.PrecioDeImportOExport,
                                                                                dExtVolumen = vPedimentoDato.Volumen;
                                                                        #endregion

                                                                        #region Pedimento: Validamos Valores.
                                                                        if (String.IsNullOrEmpty(sExtClavePermiso))
                                                                            return BadRequest("No se encontro el dato 'ClavePermiso' del Pedimento de Entrega.");

                                                                        if (String.IsNullOrEmpty(sExtPais))
                                                                            return BadRequest("No se encontro el dato 'Pais' del Pedimento de Entrega.");

                                                                        if (String.IsNullOrEmpty(sExtClavePedimento))
                                                                            return BadRequest("No se encontro el dato 'ClavePedimento' del Pedimento de Entrega.");

                                                                        if (String.IsNullOrEmpty(sExtIncoterms))
                                                                            return BadRequest("No se encontro el dato 'Incoterms' del Pedimento de Entrega.");
                                                                        #endregion

                                                                        #region Pedimento: Llenado de Estructura.
                                                                        int iIdxPermiso = -1;

                                                                        switch (objTipoComplemento)
                                                                        {
                                                                            #region Distribucion.
                                                                            case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                                                iIdxPermiso = lstEntDistExtranjero.FindIndex(e => e.PermisoImportacionOExportacion.Equals(sExtClavePermiso));

                                                                                CVolJSonDTO.stCompleDistExtranjero objEntDistExtranjeroDato;
                                                                                if (iIdxPermiso < 0)
                                                                                {
                                                                                    objEntDistExtranjeroDato = new CVolJSonDTO.stCompleDistExtranjero();
                                                                                    objEntDistExtranjeroDato.PermisoImportacionOExportacion = sExtClavePermiso;
                                                                                    List<CVolJSonDTO.stCompleDistExtranjeroPedimentos> lstPedimentosNew = new List<CVolJSonDTO.stCompleDistExtranjeroPedimentos>();
                                                                                    objEntDistExtranjeroDato.Pedimentos = lstPedimentosNew;

                                                                                    lstEntDistExtranjero.Add(objEntDistExtranjeroDato);
                                                                                    iIdxPermiso = lstEntDistExtranjero.FindIndex(e => e.PermisoImportacionOExportacion.Equals(sExtClavePermiso));
                                                                                }

                                                                                #region Pedimento Datos.
                                                                                CVolJSonDTO.stCompleDistExtranjeroPedimentos objEntDistPedimentoDato = new CVolJSonDTO.stCompleDistExtranjeroPedimentos();
                                                                                objEntDistPedimentoDato.PuntoDeInternacionOExtraccion = iExtPuntoInterOExtra.ToString();
                                                                                objEntDistPedimentoDato.PaisOrigenODestino = sExtPais;
                                                                                objEntDistPedimentoDato.MedioDeTransEntraOSaleAduana = iExtMedioIngOSal.ToString();
                                                                                objEntDistPedimentoDato.Incoterms = sExtIncoterms;
                                                                                objEntDistPedimentoDato.PrecioDeImportacionOExportacion = Math.Round(dExtImporte, 3);
                                                                                objEntDistPedimentoDato.PedimentoAduanal = sExtClavePedimento;

                                                                                #region VolumenDocumentado.
                                                                                CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                                objVolumenDocumentado.ValorNumerico = Math.Round(dExtVolumen, 3);
                                                                                objVolumenDocumentado.UnidadDeMedida = sExtUnidadMedida;

                                                                                objEntDistPedimentoDato.VolumenDocumentado = objVolumenDocumentado;
                                                                                #endregion
                                                                                #endregion

                                                                                objEntDistExtranjeroDato = (CVolJSonDTO.stCompleDistExtranjero)lstEntDistExtranjero[iIdxPermiso];
                                                                                List<CVolJSonDTO.stCompleDistExtranjeroPedimentos> lstEntDistPedimentos = (List<CVolJSonDTO.stCompleDistExtranjeroPedimentos>)objEntDistExtranjeroDato.Pedimentos;
                                                                                lstEntDistPedimentos.Add(objEntDistPedimentoDato);
                                                                                objEntDistExtranjeroDato.Pedimentos = lstEntDistPedimentos;
                                                                                lstEntDistExtranjero[iIdxPermiso] = objEntDistExtranjeroDato;
                                                                                break;
                                                                            #endregion

                                                                            #region Comercializadora.
                                                                            case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                                                iIdxPermiso = lstEntComerExtranjero.FindIndex(e => e.PermisoImportacion.Equals(sExtClavePermiso));

                                                                                CVolJSonDTO.stCompleComerExtranjero objEntComerExtranjeroDato;
                                                                                if (iIdxPermiso < 0)
                                                                                {
                                                                                    objEntComerExtranjeroDato = new CVolJSonDTO.stCompleComerExtranjero();
                                                                                    objEntComerExtranjeroDato.PermisoImportacion = sExtClavePermiso;
                                                                                    List<CVolJSonDTO.stCompleComerExtranjeroPedimentos> lstPedimentosNew = new List<CVolJSonDTO.stCompleComerExtranjeroPedimentos>();
                                                                                    objEntComerExtranjeroDato.Pedimentos = lstPedimentosNew;

                                                                                    lstEntComerExtranjero.Add(objEntComerExtranjeroDato);
                                                                                    iIdxPermiso = lstEntComerExtranjero.FindIndex(e => e.PermisoImportacion.Equals(sExtClavePermiso));
                                                                                }

                                                                                #region Pedimento Datos.
                                                                                CVolJSonDTO.stCompleComerExtranjeroPedimentos objEntComerPedimentoDato = new CVolJSonDTO.stCompleComerExtranjeroPedimentos();
                                                                                objEntComerPedimentoDato.PuntoDeInternacion = iExtPuntoInterOExtra;
                                                                                objEntComerPedimentoDato.PaisOrigen = sExtPais;
                                                                                objEntComerPedimentoDato.MedioDeTransEntraAduana = iExtMedioIngOSal;
                                                                                objEntComerPedimentoDato.Incoterms = sExtIncoterms;
                                                                                objEntComerPedimentoDato.PrecioDeImportacion = Math.Round(dExtImporte, 3);
                                                                                objEntComerPedimentoDato.PedimentoAduanal = sExtClavePedimento;

                                                                                #region VolumenDocumentado.
                                                                                CVolJSonDTO.stVolumenDato objEntComerVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                                objEntComerVolumenDocumentado.ValorNumerico = Math.Round(dExtVolumen, 3);
                                                                                objEntComerVolumenDocumentado.UnidadDeMedida = sExtUnidadMedida;

                                                                                objEntComerPedimentoDato.VolumenDocumentado = objEntComerVolumenDocumentado;
                                                                                #endregion
                                                                                #endregion

                                                                                objEntComerExtranjeroDato = (CVolJSonDTO.stCompleComerExtranjero)lstEntComerExtranjero[iIdxPermiso];
                                                                                List<CVolJSonDTO.stCompleComerExtranjeroPedimentos> lstEntComerPedimentos = (List<CVolJSonDTO.stCompleComerExtranjeroPedimentos>)objEntComerExtranjeroDato.Pedimentos;
                                                                                lstEntComerPedimentos.Add(objEntComerPedimentoDato);
                                                                                objEntComerExtranjeroDato.Pedimentos = lstEntComerPedimentos;
                                                                                lstEntComerExtranjero[iIdxPermiso] = objEntComerExtranjeroDato;
                                                                                break;
                                                                                #endregion
                                                                        }
                                                                        #endregion
                                                                    }
                                                                #endregion
                                                                #endregion

                                                                switch (objTipoComplemento)
                                                                {
                                                                    #region Distribuidor.
                                                                    case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                                        objEntComplementoDistribuidor.TipoComplemento = "Distribucion";

                                                                        if (lstEntDistNacional.Count > 0)
                                                                            objEntComplementoDistribuidor.Nacional = lstEntDistNacional;

                                                                        if (lstEntDistExtranjero.Count > 0)
                                                                            objEntComplementoDistribuidor.Extranjero = lstEntDistExtranjero;

                                                                        if (lstEntDistNacional.Count > 0 || lstEntDistExtranjero.Count > 0)
                                                                            oEntregaComplemento = objEntComplementoDistribuidor;
                                                                        break;
                                                                    #endregion

                                                                    #region Transportista.
                                                                    case CVolJSonDTO.eTipoComplemento.Transportista:
                                                                        objEntComplementoTransportista.TipoComplemento = "Transporte";

                                                                        if (lstEntTransNacional.Count > 0)
                                                                            objEntComplementoTransportista.Nacional = lstEntTransNacional;

                                                                        if (lstEntTransNacional.Count > 0)
                                                                        {
                                                                            lstEntComplementoTransportista.Add(objEntComplementoTransportista);
                                                                            oEntregaComplemento = objEntComplementoTransportista;
                                                                        }
                                                                        break;
                                                                    #endregion

                                                                    #region Comercializadora.
                                                                    case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                                        objEntComplementoComercializadora.TipoComplemento = "Comercializacion";

                                                                        if (lstEntComerNacional.Count > 0)
                                                                            objEntComplementoComercializadora.Nacional = lstEntComerNacional;

                                                                        if (lstEntComerExtranjero.Count > 0)
                                                                            objEntComplementoComercializadora.Extranjero = lstEntComerExtranjero;

                                                                        if (lstEntComerNacional.Count > 0 || lstEntComerExtranjero.Count > 0)
                                                                            oEntregaComplemento = objEntComplementoComercializadora;
                                                                        break;
                                                                        #endregion
                                                                }
                                                            }
                                                            #endregion

                                                            lstAutoTanqEntregas.Add(new CVolJSonDTO.stEntregaIndividualDato
                                                            {
                                                                NumeroDeRegistro = iNEntrega,
                                                                VolumenInicialTanque = objEntVolIni,
                                                                VolumenFinalTanque = Math.Round(dEntregaVolFinal, 3),
                                                                VolumenEntregado = objEntVolumen,
                                                                Temperatura = Math.Round(dEntregaTemperatura, 3),
                                                                PresionAbsoluta = Math.Round(dEntregaPresionAbs, 3),
                                                                FechaYHoraInicialEntrega = dtEntregaFIni.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00",
                                                                FechaYHoraFinalEntrega = dtEntregaFFin.ToString("s") + "-" + iDiferenciaHora.ToString("00") + ":00",
                                                                Complemento = oEntregaComplemento
                                                            });
                                                            #endregion

                                                            #region Guardamos las Entregas.
                                                            objAutoTanqEntrega.TotalDocumentos++;
                                                            objAutoTanqEntrega.TotalEntregas++;

                                                            #region Suma Ventas.
                                                            Decimal dSumTotalVentas = Convert.ToDecimal(objAutoTanqEntrega.SumaVentas);
                                                            objAutoTanqEntrega.SumaVentas = Math.Round((dSumTotalVentas + dEntregaImporte), 3);
                                                            #endregion

                                                            #region Suma Volumen Entregado.
                                                            CVolJSonDTO.stVolumenDato objEntSumaVol = (CVolJSonDTO.stVolumenDato)objAutoTanqEntrega.SumaVolumenEntregado;
                                                            Decimal dEntTotalVol = Convert.ToDecimal(objEntSumaVol.ValorNumerico);
                                                            objEntSumaVol.ValorNumerico = dEntTotalVol + dEntregaVolEntregado;
                                                            objAutoTanqEntrega.SumaVolumenEntregado = objEntSumaVol;
                                                            #endregion

                                                            if (objAutoTanqEntrega.TotalEntregas > 0)
                                                                objAutoTanqEntrega.Entrega = lstAutoTanqEntregas;
                                                            else
                                                                objAutoTanqEntrega.Entrega = null;

                                                            objEntAutoTanqDatos.Entregas = objAutoTanqEntrega;
                                                            lstProdAutoTanques[iIdxAutoTanq] = objEntAutoTanqDatos;
                                                            #endregion
                                                            break;
                                                            #endregion
                                                    }
                                                }
                                            #endregion
                                        }
                                        #endregion

                                        switch (objTipoDistribucion)
                                        {
                                            case CVolJSonDTO.eTipoDistribucion.Autotanques:
                                                if (lstProdAutoTanques.Count > 0)
                                                    objProductoDato.Tanque = lstProdAutoTanques;
                                                break;

                                            case CVolJSonDTO.eTipoDistribucion.Ductos:
                                                if (lstProdDuctos.Count > 0)
                                                    objProductoDato.Ducto = lstProdDuctos;
                                                break;
                                        }
                                    }
                                }
                            #endregion
                        }
                        #endregion
                        break;
                    #endregion

                    #region Tipo: Reporte Mensual.
                    case CVolJSonDTO.eTipoReporte.Mes:
                        #region Resumen Mensual. (Monthly_Summary)
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
                        #endregion

                        CVolJSonDTO.stReporteVolumenMensualDato objRepVolMenDato = new CVolJSonDTO.stReporteVolumenMensualDato();

                        #region JSON: Estación.
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

                                    if (bCompRecep)
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
                                                               //cd.SupplierFuelIdi,
                                                               SupplierFuelIdi = sd.SupplierFuelIdi,
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
                                                String sAclaracionSAT = "";

                                                #region Asignacion: Recepcion Proveedores.
                                                String sTipoProveedor = vRProvDato.SupplierType,
                                                           sRFC_Proveedor = vRProvDato.SupplierRfc,
                                                           sNombreProveedor = vRProvDato.Name,
                                                           sPermisoProveedor = vRProvDato.SupplierPermission,
                                                           sPermisoAlmProveedor = vRProvDato.StorageAndDistributionPermission,
                                                           sConsignacionProveedor = vRProvDato.IsConsignment;
                                                int iNProveedor = vRProvDato.SupplierFuelIdi;
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

                                                                #region Llenado: Recepcion CFDI.
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

                                    if (bCompEntregas)
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
                        #endregion

                        #region JSON: Autotanques y Ductos.
                        else
                        {
                            #region Consulta: Total de Recepciones y Entregas del Producto.
                            var vQMRecepEntProd = (from p in objContext.ProductStores
                                                   where p.StoreId == viNEstacion &&
                                                         p.ProductId == vProd.ProductoID
                                                   select new
                                                   {
                                                       idProducto = p.ProductId,
                                                       TotalRecepciones = (from r in objContext.InventoryIns
                                                                           where r.StoreId == p.StoreId &&
                                                                                 r.ProductId == p.ProductId &&
                                                                                 r.EndDate >= dtPerDateIni &&
                                                                                 r.EndDate <= dtPerDateEnd
                                                                           select r.InventoryInId).Count(),
                                                       TotalEntregas = (from eh in objContext.SaleOrders // <*DUDA*>
                                                                        join ed in objContext.SaleSuborders on eh.SaleOrderId equals ed.SaleOrderId
                                                                        where eh.StoreId == p.StoreId &&
                                                                              eh.Date >= dtPerDateIni &&
                                                                              eh.Date <= dtPerDateEnd &&
                                                                              ed.ProductId == p.ProductId
                                                                        select eh.SaleOrderId).Count()
                                                       //TotalEntregas = (from e in objContext.InventoryInSaleOrders
                                                       //                 where e.StoreId == p.StoreId &&
                                                       //                       e.ProductId == p.ProductId &&
                                                       //                       e.Date >= dtPerDateIni &&
                                                       //                       e.Date <= dtPerDateEnd
                                                       //                 select e.InventoryInNumber).Count()
                                                   });
                            #endregion

                            #region Lectura: Recepciones y Entregas del Producto.
                            if (vQMRecepEntProd != null)
                                foreach (var vRegistroDato in vQMRecepEntProd)
                                {
                                    #region Recepciones y Entregas: Asignamos Valores.
                                    int iTotalRecepProd = vRegistroDato.TotalRecepciones,
                                        iTotalEntProd = vRegistroDato.TotalEntregas;
                                    #endregion

                                    if (iTotalRecepProd > 0 || iTotalEntProd > 0)
                                    {
                                        #region Recepciones.
                                        List<String> lstRVacia = new List<string>();
                                        objRepVolMenDato = new CVolJSonDTO.stReporteVolumenMensualDato();
                                        CVolJSonDTO.stRecepcionMesDato objRecepMesDato = new CVolJSonDTO.stRecepcionMesDato();
                                        objRecepMesDato.TotalDocumentosMes = 0;
                                        objRecepMesDato.TotalRecepcionesMes = 0;
                                        objRecepMesDato.ImporteTotalRecepcionesMensual = 0;
                                        objRecepMesDato.Complemento = lstRVacia;

                                        #region Consulta: Recepcion Datos.
                                        var vQMRecepciones = (from r in objContext.InventoryIns
                                                              join rd in objContext.InventoryInDocuments on r.InventoryInId equals rd.InventoryInId
                                                              join pd in objContext.Products on r.ProductId equals pd.ProductId
                                                              where r.StoreId == viNEstacion &&
                                                                    r.EndDate >= dtPerDateIni && r.EndDate <= dtPerDateEnd &&
                                                                    r.ProductId == vProd.ProductoID
                                                              group new { r, rd } by r.ProductId into g
                                                              select new
                                                              {
                                                                  //UnidadMedida = "UM03", // <*DUDA*> Product.JsonClaveUnidadMedidaId
                                                                  CantRegistros = g.Count(),
                                                                  LitrosRecep = (g.Sum(l => l.r.Volume) ?? 0),
                                                                  PoderCalorifico = (g.Average(p => p.r.CalorificPower) ?? 0),
                                                                  ImporteRecep = (g.Sum(i => i.rd.Amount) ?? 0)
                                                              });
                                        #endregion

                                        #region Lectura: Recepciones.
                                        if (vQMRecepciones != null)
                                            foreach (var vRecepDatos in vQMRecepciones)
                                            {
                                                #region Recepcion: Asignamos Valores.
                                                int iCantRecep = vRecepDatos.CantRegistros;
                                                Decimal dLitrosRecep = vRecepDatos.LitrosRecep,
                                                        dImporteRecep = vRecepDatos.ImporteRecep,
                                                        dPoderCalorRecep = vRecepDatos.PoderCalorifico;
                                                String sUnidadMedidaRecep = vProd.UnidadMedida;//vRecepDatos.UnidadMedida;
                                                #endregion

                                                #region Recepcion: Llenado de Estructura.
                                                objRecepMesDato.TotalRecepcionesMes = iCantRecep;
                                                objRecepMesDato.TotalDocumentosMes = iCantRecep;
                                                #region Suma Volumen Recepcion Mens.
                                                CVolJSonDTO.stCapacidadDato objSumVolMesDato = new CVolJSonDTO.stCapacidadDato();
                                                objSumVolMesDato.ValorNumerico = dLitrosRecep;
                                                objSumVolMesDato.UnidadDeMedida = sUnidadMedidaRecep;
                                                objRecepMesDato.SumaVolumenRecepcionMes = objSumVolMesDato;
                                                #endregion
                                                objRecepMesDato.ImporteTotalRecepcionesMensual = Math.Round(dImporteRecep, 3);

                                                #region Poder Calorifico.
                                                if (sClaveProducto.Equals("PR09"))
                                                {
                                                    CVolJSonDTO.stVolumenDato objPoderCalorRecep = new CVolJSonDTO.stVolumenDato();
                                                    objPoderCalorRecep.UnidadDeMedida = sUnidadMedidaRecep; //"UM06"
                                                    objPoderCalorRecep.ValorNumerico = Math.Round(dPoderCalorRecep, 3);

                                                    objRecepMesDato.PoderCalorifico = objPoderCalorRecep;
                                                }
                                                #endregion

                                                #region Complemento.
                                                List<CVolJSonDTO.stComplementoDistribucion> lstRecepComplementoDistribucion = new List<CVolJSonDTO.stComplementoDistribucion>();
                                                List<CVolJSonDTO.stComplementoTransportista> lstRecepComplementoTransportista = new List<CVolJSonDTO.stComplementoTransportista>();
                                                List<CVolJSonDTO.stComplementoComercializadora> lstRecepComplementoComercializadora = new List<CVolJSonDTO.stComplementoComercializadora>();

                                                if (bCompRecep && objTipoComplemento != CVolJSonDTO.eTipoComplemento.Transportista)
                                                {
                                                    CVolJSonDTO.stComplementoDistribucion objRecepComplementoDistribuidor = new CVolJSonDTO.stComplementoDistribucion();
                                                    CVolJSonDTO.stComplementoTransportista objRecepComplementoTransportista = new CVolJSonDTO.stComplementoTransportista();
                                                    CVolJSonDTO.stComplementoComercializadora objRecepComplementoComercializadora = new CVolJSonDTO.stComplementoComercializadora();

                                                    #region NODO: Nacional.
                                                    List<CVolJSonDTO.stCompleDistNacional> lstRecepDistNacional = new List<CVolJSonDTO.stCompleDistNacional>();
                                                    List<CVolJSonDTO.stComplementoTransNacional> lstRecepTransNacional = new List<CVolJSonDTO.stComplementoTransNacional>();
                                                    List<CVolJSonDTO.stCompleComerNacional> lstRecepComerNacional = new List<CVolJSonDTO.stCompleComerNacional>();

                                                    #region Consulta: CFDI Datos.
                                                    var vQMCfdiDatos = (from r in objContext.InventoryIns
                                                                        join rd in objContext.InventoryInDocuments on r.InventoryInId equals rd.InventoryInId
                                                                        join pd in objContext.SupplierFuels on new { f1 = r.StoreId, f2 = rd.SupplierFuelIdi.GetValueOrDefault() } equals new { f1 = pd.StoreId, f2 = pd.SupplierFuelIdi }
                                                                        join sd in objContext.SupplierTransportRegisters on rd.SupplierTransportRegisterId equals sd.SupplierTransportRegisterId into sdf
                                                                        from sd in sdf.DefaultIfEmpty()
                                                                        where r.StoreId == viNEstacion &&
                                                                              r.EndDate >= dtPerDateIni && r.EndDate <= dtPerDateEnd &&
                                                                              r.ProductId == vProd.ProductoID
                                                                        orderby r.Date
                                                                        select new
                                                                        {
                                                                            NumeroRecepcion = r.InventoryInNumber,
                                                                            ClienteRFC = pd.Rfc,
                                                                            NombreCliente = pd.Name,
                                                                            NumeroPermisoCRE = pd.FuelPermission,
                                                                            TipoCFDI = rd.SatTipoComprobanteId,
                                                                            CFDI = rd.Uuid,
                                                                            Precio = rd.Price,
                                                                            PrecioVtaPublico = rd.PublicSalePrice,
                                                                            FechaHora = rd.Date,
                                                                            Volumen = rd.Volume,
                                                                            ContraPrestacion = (sd.AmountPerService.ToString() ?? "0"),
                                                                            TarifaTransporte = (sd.AmountPerFee.ToString() ?? "0"),
                                                                            CargoCapacidadTrans = (sd.AmountPerCapacity.ToString() ?? "0"),
                                                                            CargoUsoTrans = (sd.AmountPerUse.ToString() ?? "0"),
                                                                            CargoVolumenTrans = (sd.AmountPerVolume.ToString() ?? "0"),
                                                                            Descuento = (sd.AmountOfDiscount.ToString() ?? "0")
                                                                        });
                                                    #endregion

                                                    #region Lectura: CFDIs.
                                                    if (vQMCfdiDatos != null)
                                                        foreach (var vCfdiDatos in vQMCfdiDatos)
                                                        {
                                                            #region CFDI: Asignamos Valores.
                                                            String sNacClienteRFC = vCfdiDatos.ClienteRFC,
                                                                   sNacNombreCliente = vCfdiDatos.NombreCliente,
                                                                   sNacPermisoCRE = vCfdiDatos.NumeroPermisoCRE,
                                                                   sNacTipoCFDI = vCfdiDatos.TipoCFDI,
                                                                   sNacCFDI = vCfdiDatos.CFDI.ToString(),
                                                                   sNacUnidadMedida = vProd.UnidadMedida;//vCfdiDatos.UnidadMedida;
                                                            Decimal dNacPrecioCompra = vCfdiDatos.Precio.GetValueOrDefault(),
                                                                    dNacPrecioVtaPublico = vCfdiDatos.PrecioVtaPublico.GetValueOrDefault(),
                                                                    dNacVolumen = vCfdiDatos.Volumen.GetValueOrDefault(),
                                                                    dNacContraPrestacion = Convert.ToDecimal(vCfdiDatos.ContraPrestacion),
                                                                    dNacDescuento = Convert.ToDecimal(vCfdiDatos.Descuento),
                                                                    dNacTarifaTrans = Convert.ToDecimal(vCfdiDatos.TarifaTransporte),
                                                                    dNacCargoCapTrans = Convert.ToDecimal(vCfdiDatos.CargoCapacidadTrans),
                                                                    dNacCargoUsoTrans = Convert.ToDecimal(vCfdiDatos.CargoUsoTrans),
                                                                    dNacCargoVolTrans = Convert.ToDecimal(vCfdiDatos.CargoVolumenTrans);
                                                            int iNacNRecepcion = vCfdiDatos.NumeroRecepcion.GetValueOrDefault();
                                                            DateTime dtNacFechaHora = Convert.ToDateTime(vCfdiDatos.FechaHora);
                                                            #endregion

                                                            #region CFDI: Validamos Valores.
                                                            if (!ValidarCFDI(sNacCFDI))
                                                                return BadRequest("La Recepción '" + iNacNRecepcion.ToString() + "' contiene un CFDI con formato incorrecto '" + sNacCFDI + "'.");

                                                            String sRecepMnsErrorTipoCFDI = "";
                                                            switch (sNacTipoCFDI.ToUpper())
                                                            {
                                                                case "INGRESO": sNacTipoCFDI = "Ingreso"; break;
                                                                case "EGRESO": sNacTipoCFDI = "Egreso"; break;
                                                                case "TRASLADO": sNacTipoCFDI = "Traslado"; break;
                                                                default: sRecepMnsErrorTipoCFDI = "Tipo de CFDI no valido '" + sNacTipoCFDI + "'"; break;
                                                            }

                                                            if (!String.IsNullOrEmpty(sRecepMnsErrorTipoCFDI))
                                                                return BadRequest(sRecepMnsErrorTipoCFDI);
                                                            #endregion

                                                            #region CFDI: Llenado.
                                                            int iIdxCliente = -1;

                                                            switch (objTipoComplemento)
                                                            {
                                                                #region Distribuidor.
                                                                case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                                    iIdxCliente = lstRecepDistNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));

                                                                    CVolJSonDTO.stCompleDistNacional objRecepDistNacionalDato;
                                                                    if (iIdxCliente < 0)
                                                                    {
                                                                        objRecepDistNacionalDato = new CVolJSonDTO.stCompleDistNacional();
                                                                        objRecepDistNacionalDato.RfcClienteOProveedor = sNacClienteRFC;
                                                                        objRecepDistNacionalDato.NombreClienteOProveedor = sNacNombreCliente;

                                                                        if (!String.IsNullOrEmpty(sNacPermisoCRE))
                                                                            objRecepDistNacionalDato.PermisoClienteOProveedor = sNacPermisoCRE;

                                                                        List<CVolJSonDTO.stCompleDistNacionalCfdis> lstCFDIsNew = new List<CVolJSonDTO.stCompleDistNacionalCfdis>();
                                                                        objRecepDistNacionalDato.CFDIs = lstCFDIsNew;

                                                                        lstRecepDistNacional.Add(objRecepDistNacionalDato);
                                                                        iIdxCliente = lstRecepDistNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));
                                                                    }

                                                                    #region CFDI Datos.
                                                                    CVolJSonDTO.stCompleDistNacionalCfdis objDistCFDI_Dato = new CVolJSonDTO.stCompleDistNacionalCfdis();
                                                                    objDistCFDI_Dato.Cfdi = sNacCFDI;
                                                                    objDistCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                    objDistCFDI_Dato.PrecioVentaOCompraOContrap = Math.Round(dNacPrecioCompra, 3);
                                                                    objDistCFDI_Dato.FechaYHoraTransaccion = dtNacFechaHora.ToString("s") + "-" +
                                                                                                             iDiferenciaHora.ToString("00") + ":00";
                                                                    #region VolumenDocumentado.
                                                                    CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                    objVolumenDocumentado.ValorNumerico = Math.Round(dNacVolumen, 3);
                                                                    objVolumenDocumentado.UnidadDeMedida = sNacUnidadMedida;

                                                                    objDistCFDI_Dato.VolumenDocumentado = objVolumenDocumentado;
                                                                    #endregion
                                                                    #endregion

                                                                    objRecepDistNacionalDato = (CVolJSonDTO.stCompleDistNacional)lstRecepDistNacional[iIdxCliente];
                                                                    List<CVolJSonDTO.stCompleDistNacionalCfdis> lstCfDIs = (List<CVolJSonDTO.stCompleDistNacionalCfdis>)objRecepDistNacionalDato.CFDIs;
                                                                    lstCfDIs.Add(objDistCFDI_Dato);
                                                                    objRecepDistNacionalDato.CFDIs = lstCfDIs;
                                                                    lstRecepDistNacional[iIdxCliente] = objRecepDistNacionalDato;
                                                                    break;
                                                                #endregion

                                                                #region Comercializadora.
                                                                case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                                    iIdxCliente = lstRecepComerNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));

                                                                    CVolJSonDTO.stCompleComerNacional objRecepComerNacionalDato;
                                                                    if (iIdxCliente < 0)
                                                                    {
                                                                        objRecepComerNacionalDato = new CVolJSonDTO.stCompleComerNacional();
                                                                        objRecepComerNacionalDato.RfcClienteOProveedor = sNacClienteRFC;
                                                                        objRecepComerNacionalDato.NombreClienteOProveedor = sNacNombreCliente;

                                                                        if (!String.IsNullOrEmpty(sNacPermisoCRE))
                                                                            objRecepComerNacionalDato.PermisoProveedor = sNacPermisoCRE;

                                                                        List<CVolJSonDTO.stCompleComerNacionalCfdis> lstCFDIsNew = new List<CVolJSonDTO.stCompleComerNacionalCfdis>();
                                                                        objRecepComerNacionalDato.CFDIs = lstCFDIsNew;

                                                                        lstRecepComerNacional.Add(objRecepComerNacionalDato);
                                                                        iIdxCliente = lstRecepComerNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));
                                                                    }

                                                                    #region CFDI Datos.
                                                                    CVolJSonDTO.stCompleComerNacionalCfdis objComerCFDI_Dato = new CVolJSonDTO.stCompleComerNacionalCfdis();
                                                                    objComerCFDI_Dato.Cfdi = sNacCFDI;
                                                                    objComerCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                    objComerCFDI_Dato.PrecioCompra = Math.Round(dNacPrecioCompra, 3);
                                                                    objComerCFDI_Dato.PrecioDeVentaAlPublico = Math.Round(dNacPrecioVtaPublico, 3);
                                                                    objComerCFDI_Dato.FechayHoraTransaccion = dtNacFechaHora.ToString("s") + "-" +
                                                                                                              iDiferenciaHora.ToString("00") + ":00";
                                                                    #region VolumenDocumentado.
                                                                    CVolJSonDTO.stVolumenDato objRecepComerVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                    objRecepComerVolumenDocumentado.ValorNumerico = Math.Round(dNacVolumen, 3);
                                                                    objRecepComerVolumenDocumentado.UnidadDeMedida = sNacUnidadMedida;

                                                                    objComerCFDI_Dato.VolumenDocumentado = objRecepComerVolumenDocumentado;
                                                                    #endregion
                                                                    #endregion

                                                                    objRecepComerNacionalDato = (CVolJSonDTO.stCompleComerNacional)lstRecepComerNacional[iIdxCliente];
                                                                    List<CVolJSonDTO.stCompleComerNacionalCfdis> lstCompRecepCfDIs = (List<CVolJSonDTO.stCompleComerNacionalCfdis>)objRecepComerNacionalDato.CFDIs;
                                                                    lstCompRecepCfDIs.Add(objComerCFDI_Dato);
                                                                    objRecepComerNacionalDato.CFDIs = lstCompRecepCfDIs;
                                                                    lstRecepComerNacional[iIdxCliente] = objRecepComerNacionalDato;
                                                                    break;
                                                                    #endregion
                                                            }
                                                            #endregion
                                                        }
                                                    #endregion
                                                    #endregion

                                                    #region NODO: Extranjero.
                                                    List<CVolJSonDTO.stCompleDistExtranjero> lstRecepDistExtranjero = new List<CVolJSonDTO.stCompleDistExtranjero>();
                                                    List<CVolJSonDTO.stCompleComerExtranjero> lstRecepComerExtranjero = new List<CVolJSonDTO.stCompleComerExtranjero>();

                                                    #region Consulta: Pedimentos Datos.
                                                    var vQMPedimentos = (from r in objContext.InventoryIns
                                                                         join rd in objContext.InventoryInDocuments on r.InventoryInId equals rd.InventoryInId
                                                                         join p in objContext.PetitionCustoms on rd.PetitionCustomsId equals p.PetitionCustomsId
                                                                         join t in objContext.TransportMediumnCustoms on p.TransportMediumnCustomsId equals t.TransportMediumnCustomsId
                                                                         where r.StoreId == viNEstacion &&
                                                                               r.EndDate >= dtPerDateIni && r.EndDate <= dtPerDateEnd &&
                                                                               r.ProductId == vProd.ProductoID
                                                                         select new
                                                                         {
                                                                             ClavePermisoImportOExport = p.KeyOfImportationExportation,
                                                                             PuntoInternacionOExtracccion = p.KeyPointOfInletOrOulet,
                                                                             Pais = (p.SatPaisId ?? String.Empty),
                                                                             MedioIngresoOSalida = (t.TransportMediumn ?? "0"),
                                                                             ClavePedimento = (p.NumberCustomsDeclaration ?? String.Empty),
                                                                             Incoterms = (p.Incoterms ?? String.Empty),
                                                                             PrecioDeImportOExport = p.AmountOfImportationExportation,
                                                                             Volumen = p.QuantityDocumented,
                                                                             UnidadMedida = (p.JsonClaveUnidadMedidadId ?? "UM03")
                                                                         });
                                                    #endregion

                                                    #region Lectura: Pedimentos.
                                                    if (vQMPedimentos != null)
                                                        foreach (var vPedimentoDatos in vQMPedimentos)
                                                        {
                                                            #region Pedimento: Asignamos Valores.
                                                            String sExtClavePermiso = vPedimentoDatos.ClavePermisoImportOExport,
                                                                   sExtPais = vPedimentoDatos.Pais,
                                                                   sExtClavePedimento = vPedimentoDatos.ClavePedimento,
                                                                   sExtIncoterms = vPedimentoDatos.Incoterms,
                                                                   sExtUnidadMedida = vPedimentoDatos.UnidadMedida;
                                                            int iExtPuntoInterOExtra = Convert.ToInt32(vPedimentoDatos.PuntoInternacionOExtracccion),
                                                                iExtMedioIngOSal = Convert.ToInt32(vPedimentoDatos.MedioIngresoOSalida);
                                                            Decimal dExtImporte = vPedimentoDatos.PrecioDeImportOExport,
                                                                    dExtVolumen = vPedimentoDatos.Volumen;
                                                            #endregion

                                                            #region Pedimento: Validamos Valores.
                                                            if (String.IsNullOrEmpty(sExtClavePermiso))
                                                                return BadRequest("No se encontro el dato 'ClavePermiso' del Pedimento de Recepción.");

                                                            if (String.IsNullOrEmpty(sExtPais))
                                                                return BadRequest("No se encontro el dato 'Pais' del Pedimento de Recepción.");

                                                            if (String.IsNullOrEmpty(sExtClavePedimento))
                                                                return BadRequest("No se encontro el dato 'ClavePedimento' del Pedimento de Recepción.");

                                                            if (String.IsNullOrEmpty(sExtIncoterms))
                                                                return BadRequest("No se encontro el dato 'Incoterms' del Pedimento de Recepción.");
                                                            #endregion

                                                            #region Pedimento: Llenado de Estructura.
                                                            int iIdxPermiso = -1;

                                                            switch (objTipoComplemento)
                                                            {
                                                                #region Distribucion.
                                                                case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                                    iIdxPermiso = lstRecepDistExtranjero.FindIndex(e => e.PermisoImportacionOExportacion.Equals(sExtClavePermiso));

                                                                    CVolJSonDTO.stCompleDistExtranjero objRecepDistExtranjeroDato;
                                                                    if (iIdxPermiso < 0)
                                                                    {
                                                                        objRecepDistExtranjeroDato = new CVolJSonDTO.stCompleDistExtranjero();
                                                                        objRecepDistExtranjeroDato.PermisoImportacionOExportacion = sExtClavePermiso;
                                                                        List<CVolJSonDTO.stCompleDistExtranjeroPedimentos> lstPedimentosNew = new List<CVolJSonDTO.stCompleDistExtranjeroPedimentos>();
                                                                        objRecepDistExtranjeroDato.Pedimentos = lstPedimentosNew;

                                                                        lstRecepDistExtranjero.Add(objRecepDistExtranjeroDato);
                                                                        iIdxPermiso = lstRecepDistExtranjero.FindIndex(e => e.PermisoImportacionOExportacion.Equals(sExtClavePermiso));
                                                                    }

                                                                    #region Pedimento Datos.
                                                                    CVolJSonDTO.stCompleDistExtranjeroPedimentos objRecepDistPedimentoDato = new CVolJSonDTO.stCompleDistExtranjeroPedimentos();
                                                                    objRecepDistPedimentoDato.PuntoDeInternacionOExtraccion = iExtPuntoInterOExtra.ToString();
                                                                    objRecepDistPedimentoDato.PaisOrigenODestino = sExtPais;
                                                                    objRecepDistPedimentoDato.MedioDeTransEntraOSaleAduana = iExtMedioIngOSal.ToString();
                                                                    objRecepDistPedimentoDato.Incoterms = sExtIncoterms;
                                                                    objRecepDistPedimentoDato.PrecioDeImportacionOExportacion = Math.Round(dExtImporte, 3);
                                                                    objRecepDistPedimentoDato.PedimentoAduanal = sExtClavePedimento;

                                                                    #region VolumenDocumentado.
                                                                    CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                    objVolumenDocumentado.ValorNumerico = Math.Round(dExtVolumen, 3);
                                                                    objVolumenDocumentado.UnidadDeMedida = sExtUnidadMedida;

                                                                    objRecepDistPedimentoDato.VolumenDocumentado = objVolumenDocumentado;
                                                                    #endregion
                                                                    #endregion

                                                                    objRecepDistExtranjeroDato = (CVolJSonDTO.stCompleDistExtranjero)lstRecepDistExtranjero[iIdxPermiso];
                                                                    List<CVolJSonDTO.stCompleDistExtranjeroPedimentos> lstRecepDistPedimentos = (List<CVolJSonDTO.stCompleDistExtranjeroPedimentos>)objRecepDistExtranjeroDato.Pedimentos;
                                                                    lstRecepDistPedimentos.Add(objRecepDistPedimentoDato);
                                                                    objRecepDistExtranjeroDato.Pedimentos = lstRecepDistPedimentos;
                                                                    lstRecepDistExtranjero[iIdxPermiso] = objRecepDistExtranjeroDato;
                                                                    break;
                                                                #endregion

                                                                #region Comercializadora.
                                                                case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                                    iIdxPermiso = lstRecepComerExtranjero.FindIndex(e => e.PermisoImportacion.Equals(sExtClavePermiso));

                                                                    CVolJSonDTO.stCompleComerExtranjero objRecepComerExtranjeroDato;
                                                                    if (iIdxPermiso < 0)
                                                                    {
                                                                        objRecepComerExtranjeroDato = new CVolJSonDTO.stCompleComerExtranjero();
                                                                        objRecepComerExtranjeroDato.PermisoImportacion = sExtClavePermiso;
                                                                        List<CVolJSonDTO.stCompleComerExtranjeroPedimentos> lstPedimentosNew = new List<CVolJSonDTO.stCompleComerExtranjeroPedimentos>();
                                                                        objRecepComerExtranjeroDato.Pedimentos = lstPedimentosNew;

                                                                        lstRecepComerExtranjero.Add(objRecepComerExtranjeroDato);
                                                                        iIdxPermiso = lstRecepComerExtranjero.FindIndex(e => e.PermisoImportacion.Equals(sExtClavePermiso));
                                                                    }

                                                                    #region Pedimento Datos.
                                                                    CVolJSonDTO.stCompleComerExtranjeroPedimentos objRecepComerPedimentoDato = new CVolJSonDTO.stCompleComerExtranjeroPedimentos();
                                                                    objRecepComerPedimentoDato.PuntoDeInternacion = iExtPuntoInterOExtra;
                                                                    objRecepComerPedimentoDato.PaisOrigen = sExtPais;
                                                                    objRecepComerPedimentoDato.MedioDeTransEntraAduana = iExtMedioIngOSal;
                                                                    objRecepComerPedimentoDato.Incoterms = sExtIncoterms;
                                                                    objRecepComerPedimentoDato.PrecioDeImportacion = Math.Round(dExtImporte, 3);
                                                                    objRecepComerPedimentoDato.PedimentoAduanal = sExtClavePedimento;

                                                                    #region VolumenDocumentado.
                                                                    CVolJSonDTO.stVolumenDato objRecepComerVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                    objRecepComerVolumenDocumentado.ValorNumerico = Math.Round(dExtVolumen, 3);
                                                                    objRecepComerVolumenDocumentado.UnidadDeMedida = sExtUnidadMedida;

                                                                    objRecepComerPedimentoDato.VolumenDocumentado = objRecepComerVolumenDocumentado;
                                                                    #endregion
                                                                    #endregion

                                                                    objRecepComerExtranjeroDato = (CVolJSonDTO.stCompleComerExtranjero)lstRecepComerExtranjero[iIdxPermiso];
                                                                    List<CVolJSonDTO.stCompleComerExtranjeroPedimentos> lstRecepComerPedimentos = (List<CVolJSonDTO.stCompleComerExtranjeroPedimentos>)objRecepComerExtranjeroDato.Pedimentos;
                                                                    lstRecepComerPedimentos.Add(objRecepComerPedimentoDato);
                                                                    objRecepComerExtranjeroDato.Pedimentos = lstRecepComerPedimentos;
                                                                    lstRecepComerExtranjero[iIdxPermiso] = objRecepComerExtranjeroDato;
                                                                    break;
                                                                    #endregion
                                                            }
                                                            #endregion
                                                        }
                                                    #endregion
                                                    #endregion

                                                    switch (objTipoComplemento)
                                                    {
                                                        #region Distribuidor.
                                                        case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                            objRecepComplementoDistribuidor.TipoComplemento = "Distribucion";

                                                            if (lstRecepDistNacional.Count > 0)
                                                                objRecepComplementoDistribuidor.Nacional = lstRecepDistNacional;

                                                            if (lstRecepDistExtranjero.Count > 0)
                                                                objRecepComplementoDistribuidor.Extranjero = lstRecepDistExtranjero;

                                                            if (lstRecepDistNacional.Count > 0 || lstRecepDistExtranjero.Count > 0)
                                                                lstRecepComplementoDistribucion.Add(objRecepComplementoDistribuidor);
                                                            break;
                                                        #endregion

                                                        #region Transportista.
                                                        case CVolJSonDTO.eTipoComplemento.Transportista:
                                                            objRecepComplementoTransportista.TipoComplemento = "Transporte";

                                                            if (lstRecepTransNacional.Count > 0)
                                                                objRecepComplementoTransportista.Nacional = lstRecepTransNacional;

                                                            if (lstRecepTransNacional.Count > 0)
                                                                lstRecepComplementoTransportista.Add(objRecepComplementoTransportista);
                                                            break;
                                                        #endregion

                                                        #region Comercializadora.
                                                        case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                            objRecepComplementoComercializadora.TipoComplemento = "Comercializacion";

                                                            if (lstRecepComerNacional.Count > 0)
                                                                objRecepComplementoComercializadora.Nacional = lstRecepComerNacional;

                                                            if (lstRecepComerExtranjero.Count > 0)
                                                                objRecepComplementoComercializadora.Extranjero = lstRecepComerExtranjero;

                                                            if (lstRecepComerNacional.Count > 0 || lstRecepComerExtranjero.Count > 0)
                                                                lstRecepComplementoComercializadora.Add(objRecepComplementoComercializadora);
                                                            break;
                                                            #endregion
                                                    }
                                                }

                                                switch (objTipoComplemento)
                                                {
                                                    #region Distribuidor.
                                                    case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                        if (lstRecepComplementoDistribucion.Count > 0)
                                                            objRecepMesDato.Complemento = lstRecepComplementoDistribucion;
                                                        else
                                                            throw new Exception("Recepciones Mes: No se encontraron datos de Facturación, Pedimento, etc. Favor de captura la información para la generación del Complemento.");
                                                        break;
                                                    #endregion

                                                    #region Transportista.
                                                    case CVolJSonDTO.eTipoComplemento.Transportista:
                                                        if (lstRecepComplementoTransportista.Count > 0)
                                                            objRecepMesDato.Complemento = lstRecepComplementoTransportista;
                                                        else
                                                            throw new Exception("Recepciones Mes: No se encontraron datos de Facturación, Pedimento, etc. Favor de captura la información para la generación del Complemento.");
                                                        break;
                                                    #endregion

                                                    #region Comercializadora.
                                                    case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                        if (lstRecepComplementoComercializadora.Count > 0)
                                                            objRecepMesDato.Complemento = lstRecepComplementoComercializadora;
                                                        else
                                                            throw new Exception("Recepciones Mes: No se encontraron datos de Facturación, Pedimento, etc. Favor de captura la información para la generación del Complemento.");
                                                        break;
                                                        #endregion
                                                }
                                                #endregion
                                                #endregion
                                            }
                                        #endregion

                                        objRepVolMenDato.Recepciones = objRecepMesDato;
                                        #endregion

                                        #region Entregas.
                                        List<String> lstEVacia = new List<string>();
                                        CVolJSonDTO.stEntregasMesDato objEntregaMesDato = new CVolJSonDTO.stEntregasMesDato();
                                        objEntregaMesDato.TotalDocumentosMes = 0;
                                        objEntregaMesDato.TotalEntregasMes = 0;
                                        objEntregaMesDato.ImporteTotalEntregasMes = 0;
                                        objEntregaMesDato.Complemento = lstEVacia;

                                        #region Consulta: Entregas.
                                        var vQMEntregas = (from eh in objContext.SaleOrders // <*DUDA*>
                                                           join ed in objContext.SaleSuborders on eh.SaleOrderId equals ed.SaleOrderId
                                                           join p in objContext.Products on ed.ProductId equals p.ProductId
                                                           where eh.StoreId == viNEstacion &&
                                                                 eh.Date >= dtPerDateIni && eh.Date <= dtPerDateEnd &&
                                                                 ed.ProductId == vProd.ProductoID
                                                           group new { eh, ed, p } by p.JsonClaveUnidadMedidaId into g
                                                           select new
                                                           {
                                                               //UnidadMedida = (g.Key ?? "UM03"),
                                                               CantRegistros = g.Count(),
                                                               LitrosEntrega = (g.Sum(e => e.ed.Quantity) ?? 0),
                                                               PoderCalorifico = (g.Sum(e => e.ed.CalorificPower) ?? 0),
                                                               ImporteEntrega = (g.Sum(e => e.eh.Amount) ?? 0)
                                                           });
                                        //var vQMEntregas = (from e in objContext.InventoryInSaleOrders
                                        //                   join ed in objContext.InventoryInDocuments on e.InventoryIn equals ed.InventoryInId
                                        //                   join pd in objContext.Products on e.ProductId equals pd.ProductId
                                        //                   where e.StoreId == viNEstacion &&
                                        //                         e.EndDate >= dtPerDateIni && e.EndDate <= dtPerDateEnd &&
                                        //                         e.ProductId == vProd.ProductoID
                                        //                   group new { e, ed } by e.ProductId into g
                                        //                   select new
                                        //                   {
                                        //                       //UnidadMedida = "UM03", // <*DUDA*> Product.JsonClaveUnidadMedidaId
                                        //                       CantRegistros = g.Count(),
                                        //                       LitrosEntrega = (g.Sum(l => l.e.Volume) ?? 0),
                                        //                       PoderCalorifico = (g.Average(p => p.e.CalorificPower) ?? 0),
                                        //                       ImporteEntrega = (g.Sum(i => i.ed.Amount) ?? 0)
                                        //                   });
                                        #endregion

                                        #region Lectura: Entregas.
                                        if (vQMEntregas != null)
                                            foreach (var vEntregaDatos in vQMEntregas)
                                            {
                                                #region Entregas: Asignamos Valores.
                                                int iCantEnt = vEntregaDatos.CantRegistros;
                                                Decimal dLitrosEnt = vEntregaDatos.LitrosEntrega,
                                                        dImporteEnt = vEntregaDatos.ImporteEntrega,
                                                        dPoderCalorEnt = vEntregaDatos.PoderCalorifico;
                                                String sUnidadMedidaEnt = vProd.UnidadMedida;//vEntregaDatos.UnidadMedida;
                                                #endregion

                                                #region Entregas: Llenado.
                                                objEntregaMesDato.TotalEntregasMes = iCantEnt;
                                                objEntregaMesDato.SumaVolumenEntregadoMes = dLitrosEnt;
                                                #region Suma Volumen Entregado Mes.
                                                CVolJSonDTO.stCapacidadDato objSumVolEntMesDato = new CVolJSonDTO.stCapacidadDato();
                                                objSumVolEntMesDato.ValorNumerico = dLitrosEnt;
                                                objSumVolEntMesDato.UnidadDeMedida = sUnidadMedidaEnt;
                                                objEntregaMesDato.SumaVolumenEntregadoMes = objSumVolEntMesDato;
                                                #endregion

                                                objEntregaMesDato.ImporteTotalEntregasMes = dImporteEnt;
                                                objEntregaMesDato.TotalDocumentosMes = iCantEnt;

                                                #region Poder Calorifico.
                                                if (sClaveProducto.Equals("PR09"))
                                                {
                                                    CVolJSonDTO.stVolumenDato objPoderCalorEnt = new CVolJSonDTO.stVolumenDato();
                                                    objPoderCalorEnt.UnidadDeMedida = sUnidadMedidaEnt;
                                                    objPoderCalorEnt.ValorNumerico = Math.Round(dPoderCalorEnt, 3);
                                                    objEntregaMesDato.PoderCalorifico = objPoderCalorEnt;
                                                }
                                                #endregion

                                                #region Complementos.
                                                List<CVolJSonDTO.stComplementoDistribucion> lstEntComplementoDistribucion = new List<CVolJSonDTO.stComplementoDistribucion>();
                                                List<CVolJSonDTO.stComplementoTransportista> lstEntComplementoTransportista = new List<CVolJSonDTO.stComplementoTransportista>();
                                                List<CVolJSonDTO.stComplementoComercializadora> lstEntComplementoComercializadora = new List<CVolJSonDTO.stComplementoComercializadora>();

                                                if (bCompEntregas)
                                                {
                                                    CVolJSonDTO.stComplementoDistribucion objEntComplementoDistribuidor = new CVolJSonDTO.stComplementoDistribucion();
                                                    CVolJSonDTO.stComplementoTransportista objEntComplementoTransportista = new CVolJSonDTO.stComplementoTransportista();
                                                    CVolJSonDTO.stComplementoComercializadora objEntComplementoComercializadora = new CVolJSonDTO.stComplementoComercializadora();

                                                    #region NODO: Nacional.
                                                    List<CVolJSonDTO.stCompleDistNacional> lstEntDistNacional = new List<CVolJSonDTO.stCompleDistNacional>();
                                                    List<CVolJSonDTO.stComplementoTransNacional> lstEntTransNacional = new List<CVolJSonDTO.stComplementoTransNacional>();
                                                    List<CVolJSonDTO.stCompleComerNacional> lstEntComerNacional = new List<CVolJSonDTO.stCompleComerNacional>();

                                                    #region Consulta: CFDI Datos.
                                                    var vQMEntregaCfdiDato = (from tf in objContext.InvoiceSaleOrders
                                                                              join fh in objContext.Invoices on tf.InvoiceId equals fh.InvoiceId
                                                                              join fd in objContext.InvoiceDetails on fh.InvoiceId equals fd.InvoiceId
                                                                              join c in objContext.Customers on fh.CustomerId equals c.CustomerId
                                                                              join t in objContext.SaleOrders on tf.SaleOrderId equals t.SaleOrderId
                                                                              join td in objContext.SaleSuborders on t.SaleOrderId equals td.SaleOrderId
                                                                              join tc in objContext.SatTipoComprobantes on fh.SatTipoComprobanteId equals tc.SatTipoComprobanteId
                                                                              join tr in objContext.SupplierTransportRegisters on td.SupplierTransportRegisterId equals tr.SupplierTransportRegisterId into rtr
                                                                              from tr in rtr.DefaultIfEmpty()
                                                                              where fh.StoreId == viNEstacion &&
                                                                                    t.Date >= dtPerDateIni && t.Date <= dtPerDateEnd &&
                                                                                    td.ProductId == vProd.ProductoID
                                                                              select new
                                                                              {
                                                                                  NumeroEntrega = t.SaleOrderNumber,
                                                                                  ClienteRFC = c.Rfc,
                                                                                  NombreCliente = c.Name,
                                                                                  NumeroPermisoCRE = c.CustomerPermission,
                                                                                  TipoCfdiID = fh.SatTipoComprobanteId,
                                                                                  TipoCFDI = tc.Descripcion,
                                                                                  CFDI = fh.Uuid,
                                                                                  Precio = fd.Price,
                                                                                  PrecioVtaPublico = fd.Price,
                                                                                  FechaHora = fh.Date,
                                                                                  Volumen = fd.Quantity,
                                                                                  ContraPrestacion = (tr.AmountPerService.ToString() ?? "0"),
                                                                                  TarifaTransporte = (tr.AmountPerFee.ToString() ?? "0"),
                                                                                  CargoCapacidadTrans = (tr.AmountPerCapacity.ToString() ?? "0"),
                                                                                  CargoUsoTrans = (tr.AmountPerUse.ToString() ?? "0"),
                                                                                  CargoVolumenTrans = (tr.AmountPerVolume.ToString() ?? "0"),
                                                                                  Descuento = (tr.AmountOfDiscount.ToString() ?? "0")
                                                                              });
                                                    #endregion

                                                    #region Lectura: CFDI Datos.
                                                    if (vQMEntregaCfdiDato != null)
                                                        foreach (var vCfdiDatos in vQMEntregaCfdiDato)
                                                        {
                                                            #region CFDI: Asignamos Valores.
                                                            String sNacClienteRFC = vCfdiDatos.ClienteRFC,
                                                                   sNacNombreCliente = vCfdiDatos.NombreCliente,
                                                                   sNacPermisoCRE = vCfdiDatos.NumeroPermisoCRE,
                                                                   sNacTipoCfdiID = vCfdiDatos.TipoCfdiID,
                                                                   sNacTipoCFDI = vCfdiDatos.TipoCFDI,
                                                                   sNacCFDI = vCfdiDatos.CFDI;
                                                            Decimal dNacPrecioCompra = vCfdiDatos.Precio.GetValueOrDefault(),
                                                                    dNacPrecioVtaPublico = vCfdiDatos.PrecioVtaPublico.GetValueOrDefault(),
                                                                    dNacVolumen = vCfdiDatos.Volumen.GetValueOrDefault(),
                                                                    dNacContraPrestacion = Convert.ToDecimal(vCfdiDatos.ContraPrestacion),
                                                                    dNacDescuento = Convert.ToDecimal(vCfdiDatos.Descuento),
                                                                    dNacTarifaTrans = Convert.ToDecimal(vCfdiDatos.TarifaTransporte),
                                                                    dNacCargoCapTrans = Convert.ToDecimal(vCfdiDatos.CargoCapacidadTrans),
                                                                    dNacCargoUsoTrans = Convert.ToDecimal(vCfdiDatos.CargoUsoTrans),
                                                                    dNacCargoVolTrans = Convert.ToDecimal(vCfdiDatos.CargoVolumenTrans);
                                                            int iNacNEntrega = vCfdiDatos.NumeroEntrega.GetValueOrDefault();
                                                            DateTime dtNacFechaHora = Convert.ToDateTime(vCfdiDatos.FechaHora);
                                                            #endregion

                                                            #region CFDI: Validamos Valores.
                                                            if (!sNacTipoCfdiID.ToUpper().Equals("I") && !sNacTipoCfdiID.ToUpper().Equals("E") && !sNacTipoCfdiID.ToUpper().Equals("T"))
                                                                return BadRequest("Tipo de CFDI no valido '" + sNacTipoCFDI + "'");

                                                            if (!ValidarCFDI(sNacCFDI))
                                                                return BadRequest("La Entrega '" + iNacNEntrega.ToString() + "' contiene un CFDI con formato incorrecto '" + sNacCFDI + "'.");
                                                            #endregion

                                                            #region CFDI: Llenado de Estructura.
                                                            int iIdxCliente = -1;

                                                            switch (objTipoComplemento)
                                                            {
                                                                #region Distribuidora.
                                                                case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                                    iIdxCliente = lstEntDistNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));

                                                                    CVolJSonDTO.stCompleDistNacional objEntDistNacionalDato;
                                                                    if (iIdxCliente < 0)
                                                                    {
                                                                        objEntDistNacionalDato = new CVolJSonDTO.stCompleDistNacional();
                                                                        objEntDistNacionalDato.RfcClienteOProveedor = sNacClienteRFC;
                                                                        objEntDistNacionalDato.NombreClienteOProveedor = sNacNombreCliente;

                                                                        if (!String.IsNullOrEmpty(sNacPermisoCRE))
                                                                            objEntDistNacionalDato.PermisoClienteOProveedor = sNacPermisoCRE;

                                                                        List<CVolJSonDTO.stCompleDistNacionalCfdis> lstCFDIsNew = new List<CVolJSonDTO.stCompleDistNacionalCfdis>();
                                                                        objEntDistNacionalDato.CFDIs = lstCFDIsNew;

                                                                        lstEntDistNacional.Add(objEntDistNacionalDato);
                                                                        iIdxCliente = lstEntDistNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));
                                                                    }

                                                                    #region CFDI Datos.
                                                                    CVolJSonDTO.stCompleDistNacionalCfdis objDistCFDI_Dato = new CVolJSonDTO.stCompleDistNacionalCfdis();
                                                                    objDistCFDI_Dato.Cfdi = sNacCFDI;
                                                                    objDistCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                    objDistCFDI_Dato.PrecioVentaOCompraOContrap = Math.Round(dNacPrecioCompra, 3);
                                                                    objDistCFDI_Dato.FechaYHoraTransaccion = dtNacFechaHora.ToString("s") + "-" +
                                                                                                             iDiferenciaHora.ToString("00") + ":00";
                                                                    #region VolumenDocumentado.
                                                                    CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                    objVolumenDocumentado.ValorNumerico = Math.Round(dNacVolumen, 3);
                                                                    objVolumenDocumentado.UnidadDeMedida = vProd.UnidadMedida;

                                                                    objDistCFDI_Dato.VolumenDocumentado = objVolumenDocumentado;
                                                                    #endregion
                                                                    #endregion

                                                                    objEntDistNacionalDato = (CVolJSonDTO.stCompleDistNacional)lstEntDistNacional[iIdxCliente];
                                                                    List<CVolJSonDTO.stCompleDistNacionalCfdis> lstCfDIs = (List<CVolJSonDTO.stCompleDistNacionalCfdis>)objEntDistNacionalDato.CFDIs;
                                                                    lstCfDIs.Add(objDistCFDI_Dato);
                                                                    objEntDistNacionalDato.CFDIs = lstCfDIs;
                                                                    lstEntDistNacional[iIdxCliente] = objEntDistNacionalDato;
                                                                    break;
                                                                #endregion

                                                                #region Transportista.
                                                                case CVolJSonDTO.eTipoComplemento.Transportista:
                                                                    iIdxCliente = lstEntTransNacional.FindIndex(n => n.RfcCliente.Equals(sNacClienteRFC));

                                                                    CVolJSonDTO.stComplementoTransNacional objEntTransNacionalDato;
                                                                    if (iIdxCliente < 0)
                                                                    {
                                                                        objEntTransNacionalDato = new CVolJSonDTO.stComplementoTransNacional();
                                                                        objEntTransNacionalDato.RfcCliente = sNacClienteRFC;
                                                                        objEntTransNacionalDato.NombreCliente = sNacNombreCliente;

                                                                        List<CVolJSonDTO.stCompleTransNacionalCfdis> lstCFDIsNew = new List<CVolJSonDTO.stCompleTransNacionalCfdis>();
                                                                        objEntTransNacionalDato.CFDIs = lstCFDIsNew;

                                                                        lstEntTransNacional.Add(objEntTransNacionalDato);
                                                                        iIdxCliente = lstEntTransNacional.FindIndex(n => n.RfcCliente.Equals(sNacClienteRFC));
                                                                    }

                                                                    #region CFDI Datos.
                                                                    CVolJSonDTO.stCompleTransNacionalCfdis objTransCFDI_Dato = new CVolJSonDTO.stCompleTransNacionalCfdis();
                                                                    objTransCFDI_Dato.Cfdi = sNacCFDI;
                                                                    objTransCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                    objTransCFDI_Dato.Contraprestacion = dNacContraPrestacion;
                                                                    objTransCFDI_Dato.TarifaDeTransporte = dNacTarifaTrans;
                                                                    objTransCFDI_Dato.FechaYHoraTransaccion = dtNacFechaHora.ToString("s") + "-" +
                                                                                                              iDiferenciaHora.ToString("00") + ":00";
                                                                    #region VolumenDocumentado.
                                                                    CVolJSonDTO.stVolumenDato objEntTransVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                    objEntTransVolumenDocumentado.ValorNumerico = Math.Round(dNacVolumen, 3);
                                                                    objEntTransVolumenDocumentado.UnidadDeMedida = vProd.UnidadMedida;

                                                                    objTransCFDI_Dato.VolumenDocumentado = objEntTransVolumenDocumentado;
                                                                    #endregion

                                                                    if (dNacCargoCapTrans > 0)
                                                                        objTransCFDI_Dato.CargoPorCapacidadDeTrans = dNacCargoCapTrans;

                                                                    if (dNacCargoUsoTrans > 0)
                                                                        objTransCFDI_Dato.CargoPorUsoTrans = dNacCargoUsoTrans;

                                                                    if (dNacCargoVolTrans > 0)
                                                                        objTransCFDI_Dato.CargoVolumetricoTrans = dNacCargoVolTrans;

                                                                    if (dNacDescuento > 0)
                                                                        objTransCFDI_Dato.Descuento = dNacDescuento;
                                                                    #endregion

                                                                    objEntTransNacionalDato = (CVolJSonDTO.stComplementoTransNacional)lstEntTransNacional[iIdxCliente];
                                                                    List<CVolJSonDTO.stCompleTransNacionalCfdis> lstEntTransCfDIs = (List<CVolJSonDTO.stCompleTransNacionalCfdis>)objEntTransNacionalDato.CFDIs;
                                                                    lstEntTransCfDIs.Add(objTransCFDI_Dato);
                                                                    objEntTransNacionalDato.CFDIs = lstEntTransCfDIs;
                                                                    lstEntTransNacional[iIdxCliente] = objEntTransNacionalDato;
                                                                    break;
                                                                #endregion

                                                                #region Comercializadora.
                                                                case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                                    iIdxCliente = lstEntComerNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));

                                                                    CVolJSonDTO.stCompleComerNacional objEntComerNacionalDato;
                                                                    if (iIdxCliente < 0)
                                                                    {
                                                                        objEntComerNacionalDato = new CVolJSonDTO.stCompleComerNacional();
                                                                        objEntComerNacionalDato.RfcClienteOProveedor = sNacClienteRFC;
                                                                        objEntComerNacionalDato.NombreClienteOProveedor = sNacNombreCliente;

                                                                        if (!String.IsNullOrEmpty(sNacPermisoCRE))
                                                                            objEntComerNacionalDato.PermisoProveedor = sNacPermisoCRE;

                                                                        List<CVolJSonDTO.stCompleComerNacionalCfdis> lstCFDIsNew = new List<CVolJSonDTO.stCompleComerNacionalCfdis>();
                                                                        objEntComerNacionalDato.CFDIs = lstCFDIsNew;

                                                                        lstEntComerNacional.Add(objEntComerNacionalDato);
                                                                        iIdxCliente = lstEntComerNacional.FindIndex(n => n.RfcClienteOProveedor.Equals(sNacClienteRFC));
                                                                    }

                                                                    #region CFDI Datos.
                                                                    CVolJSonDTO.stCompleComerNacionalCfdis objComerCFDI_Dato = new CVolJSonDTO.stCompleComerNacionalCfdis();
                                                                    objComerCFDI_Dato.Cfdi = sNacCFDI;
                                                                    objComerCFDI_Dato.TipoCfdi = sNacTipoCFDI;
                                                                    objComerCFDI_Dato.PrecioCompra = Math.Round(dNacPrecioCompra, 3);
                                                                    objComerCFDI_Dato.PrecioDeVentaAlPublico = Math.Round(dNacPrecioVtaPublico, 3);
                                                                    objComerCFDI_Dato.FechayHoraTransaccion = dtNacFechaHora.ToString("s") + "-" +
                                                                                                              iDiferenciaHora.ToString("00") + ":00";
                                                                    #region VolumenDocumentado.
                                                                    CVolJSonDTO.stVolumenDato objEntComerVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                    objEntComerVolumenDocumentado.ValorNumerico = Math.Round(dNacVolumen, 3);
                                                                    objEntComerVolumenDocumentado.UnidadDeMedida = vProd.UnidadMedida;

                                                                    objComerCFDI_Dato.VolumenDocumentado = objEntComerVolumenDocumentado;
                                                                    #endregion
                                                                    #endregion

                                                                    objEntComerNacionalDato = (CVolJSonDTO.stCompleComerNacional)lstEntComerNacional[iIdxCliente];
                                                                    List<CVolJSonDTO.stCompleComerNacionalCfdis> lstEntComerCfDIs = (List<CVolJSonDTO.stCompleComerNacionalCfdis>)objEntComerNacionalDato.CFDIs;
                                                                    lstEntComerCfDIs.Add(objComerCFDI_Dato);
                                                                    objEntComerNacionalDato.CFDIs = lstEntComerCfDIs;
                                                                    lstEntComerNacional[iIdxCliente] = objEntComerNacionalDato;
                                                                    break;
                                                                    #endregion
                                                            }
                                                            #endregion
                                                        }
                                                    #endregion
                                                    #endregion

                                                    #region NODO: Extranjero.
                                                    List<CVolJSonDTO.stCompleDistExtranjero> lstEntDistExtranjero = new List<CVolJSonDTO.stCompleDistExtranjero>();
                                                    List<CVolJSonDTO.stCompleComerExtranjero> lstEntComerExtranjero = new List<CVolJSonDTO.stCompleComerExtranjero>();

                                                    #region Consulta: Pedimento Datos.
                                                    var vQMPedimentos = (from eh in objContext.SaleOrders
                                                                         join ed in objContext.SaleSuborders on eh.SaleOrderId equals ed.SaleOrderId
                                                                         join p in objContext.PetitionCustoms on ed.PetitionCustomsId equals p.PetitionCustomsId
                                                                         join t in objContext.TransportMediumnCustoms on p.TransportMediumnCustomsId equals t.TransportMediumnCustomsId
                                                                         where eh.StoreId == viNEstacion &&
                                                                               eh.Date >= dtPerDateIni && eh.Date <= dtPerDateEnd &&
                                                                               ed.ProductId == vProd.ProductoID
                                                                         select new
                                                                         {
                                                                             ClavePermisoImportOExport = p.KeyOfImportationExportation,
                                                                             PuntoInternacionOExtracccion = p.KeyPointOfInletOrOulet,
                                                                             Pais = (p.SatPaisId ?? String.Empty),
                                                                             MedioIngresoOSalida = (t.TransportMediumn ?? "0"),
                                                                             ClavePedimento = (p.NumberCustomsDeclaration ?? String.Empty),
                                                                             Incoterms = (p.Incoterms ?? String.Empty),
                                                                             PrecioDeImportOExport = p.AmountOfImportationExportation,
                                                                             Volumen = p.QuantityDocumented
                                                                         });
                                                    #endregion

                                                    #region Lectura: Pedimentos.
                                                    if (vQMPedimentos != null)
                                                        foreach (var vPedimentoDatos in vQMPedimentos)
                                                        {
                                                            #region Pedimento: Asignamos Valores.
                                                            String sExtClavePermiso = vPedimentoDatos.ClavePermisoImportOExport,
                                                                   sExtPais = vPedimentoDatos.Pais,
                                                                   sExtClavePedimento = vPedimentoDatos.ClavePedimento,
                                                                   sExtIncoterms = vPedimentoDatos.Incoterms,
                                                                   sExtUnidadMedida = vProd.UnidadMedida;
                                                            int iExtPuntoInterOExtra = Convert.ToInt32(vPedimentoDatos.PuntoInternacionOExtracccion),
                                                                iExtMedioIngOSal = Convert.ToInt32(vPedimentoDatos.MedioIngresoOSalida);
                                                            Decimal dExtImporte = vPedimentoDatos.PrecioDeImportOExport,
                                                                    dExtVolumen = vPedimentoDatos.Volumen;
                                                            #endregion

                                                            #region Pedimento: Validamos Valores.
                                                            if (String.IsNullOrEmpty(sExtClavePermiso))
                                                                return BadRequest("No se encontro el dato 'ClavePermiso' del Pedimento de Entrega.");

                                                            if (String.IsNullOrEmpty(sExtPais))
                                                                return BadRequest("No se encontro el dato 'Pais' del Pedimento de Entrega.");

                                                            if (String.IsNullOrEmpty(sExtClavePedimento))
                                                                return BadRequest("No se encontro el dato 'ClavePedimento' del Pedimento de Entrega.");

                                                            if (String.IsNullOrEmpty(sExtIncoterms))
                                                                return BadRequest("No se encontro el dato 'Incoterms' del Pedimento de Entrega.");
                                                            #endregion

                                                            #region Pedimento: Llenado de Estructura.
                                                            int iIdxPermiso = -1;

                                                            switch (objTipoComplemento)
                                                            {
                                                                #region Distribucion.
                                                                case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                                    iIdxPermiso = lstEntDistExtranjero.FindIndex(e => e.PermisoImportacionOExportacion.Equals(sExtClavePermiso));

                                                                    CVolJSonDTO.stCompleDistExtranjero objEntDistExtranjeroDato;
                                                                    if (iIdxPermiso < 0)
                                                                    {
                                                                        objEntDistExtranjeroDato = new CVolJSonDTO.stCompleDistExtranjero();
                                                                        objEntDistExtranjeroDato.PermisoImportacionOExportacion = sExtClavePermiso;
                                                                        List<CVolJSonDTO.stCompleDistExtranjeroPedimentos> lstPedimentosNew = new List<CVolJSonDTO.stCompleDistExtranjeroPedimentos>();
                                                                        objEntDistExtranjeroDato.Pedimentos = lstPedimentosNew;

                                                                        lstEntDistExtranjero.Add(objEntDistExtranjeroDato);
                                                                        iIdxPermiso = lstEntDistExtranjero.FindIndex(e => e.PermisoImportacionOExportacion.Equals(sExtClavePermiso));
                                                                    }

                                                                    #region Pedimento Datos.
                                                                    CVolJSonDTO.stCompleDistExtranjeroPedimentos objEntDistPedimentoDato = new CVolJSonDTO.stCompleDistExtranjeroPedimentos();
                                                                    objEntDistPedimentoDato.PuntoDeInternacionOExtraccion = iExtPuntoInterOExtra.ToString();
                                                                    objEntDistPedimentoDato.PaisOrigenODestino = sExtPais;
                                                                    objEntDistPedimentoDato.MedioDeTransEntraOSaleAduana = iExtMedioIngOSal.ToString();
                                                                    objEntDistPedimentoDato.Incoterms = sExtIncoterms;
                                                                    objEntDistPedimentoDato.PrecioDeImportacionOExportacion = Math.Round(dExtImporte, 3);
                                                                    objEntDistPedimentoDato.PedimentoAduanal = sExtClavePedimento;

                                                                    #region VolumenDocumentado.
                                                                    CVolJSonDTO.stVolumenDato objVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                    objVolumenDocumentado.ValorNumerico = Math.Round(dExtVolumen, 3);
                                                                    objVolumenDocumentado.UnidadDeMedida = sExtUnidadMedida;

                                                                    objEntDistPedimentoDato.VolumenDocumentado = objVolumenDocumentado;
                                                                    #endregion
                                                                    #endregion

                                                                    objEntDistExtranjeroDato = (CVolJSonDTO.stCompleDistExtranjero)lstEntDistExtranjero[iIdxPermiso];
                                                                    List<CVolJSonDTO.stCompleDistExtranjeroPedimentos> lstEntDistPedimentos = (List<CVolJSonDTO.stCompleDistExtranjeroPedimentos>)objEntDistExtranjeroDato.Pedimentos;
                                                                    lstEntDistPedimentos.Add(objEntDistPedimentoDato);
                                                                    objEntDistExtranjeroDato.Pedimentos = lstEntDistPedimentos;
                                                                    lstEntDistExtranjero[iIdxPermiso] = objEntDistExtranjeroDato;
                                                                    break;
                                                                #endregion

                                                                #region Comercializadora.
                                                                case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                                    iIdxPermiso = lstEntComerExtranjero.FindIndex(e => e.PermisoImportacion.Equals(sExtClavePermiso));

                                                                    CVolJSonDTO.stCompleComerExtranjero objEntComerExtranjeroDato;
                                                                    if (iIdxPermiso < 0)
                                                                    {
                                                                        objEntComerExtranjeroDato = new CVolJSonDTO.stCompleComerExtranjero();
                                                                        objEntComerExtranjeroDato.PermisoImportacion = sExtClavePermiso;
                                                                        List<CVolJSonDTO.stCompleComerExtranjeroPedimentos> lstPedimentosNew = new List<CVolJSonDTO.stCompleComerExtranjeroPedimentos>();
                                                                        objEntComerExtranjeroDato.Pedimentos = lstPedimentosNew;

                                                                        lstEntComerExtranjero.Add(objEntComerExtranjeroDato);
                                                                        iIdxPermiso = lstEntComerExtranjero.FindIndex(e => e.PermisoImportacion.Equals(sExtClavePermiso));
                                                                    }

                                                                    #region Pedimento Datos.
                                                                    CVolJSonDTO.stCompleComerExtranjeroPedimentos objEntComerPedimentoDato = new CVolJSonDTO.stCompleComerExtranjeroPedimentos();
                                                                    objEntComerPedimentoDato.PuntoDeInternacion = iExtPuntoInterOExtra;
                                                                    objEntComerPedimentoDato.PaisOrigen = sExtPais;
                                                                    objEntComerPedimentoDato.MedioDeTransEntraAduana = iExtMedioIngOSal;
                                                                    objEntComerPedimentoDato.Incoterms = sExtIncoterms;
                                                                    objEntComerPedimentoDato.PrecioDeImportacion = Math.Round(dExtImporte, 3);
                                                                    objEntComerPedimentoDato.PedimentoAduanal = sExtClavePedimento;

                                                                    #region VolumenDocumentado.
                                                                    CVolJSonDTO.stVolumenDato objEntComerVolumenDocumentado = new CVolJSonDTO.stVolumenDato();
                                                                    objEntComerVolumenDocumentado.ValorNumerico = Math.Round(dExtVolumen, 3);
                                                                    objEntComerVolumenDocumentado.UnidadDeMedida = sExtUnidadMedida;

                                                                    objEntComerPedimentoDato.VolumenDocumentado = objEntComerVolumenDocumentado;
                                                                    #endregion
                                                                    #endregion

                                                                    objEntComerExtranjeroDato = (CVolJSonDTO.stCompleComerExtranjero)lstEntComerExtranjero[iIdxPermiso];
                                                                    List<CVolJSonDTO.stCompleComerExtranjeroPedimentos> lstEntComerPedimentos = (List<CVolJSonDTO.stCompleComerExtranjeroPedimentos>)objEntComerExtranjeroDato.Pedimentos;
                                                                    lstEntComerPedimentos.Add(objEntComerPedimentoDato);
                                                                    objEntComerExtranjeroDato.Pedimentos = lstEntComerPedimentos;
                                                                    lstEntComerExtranjero[iIdxPermiso] = objEntComerExtranjeroDato;
                                                                    break;
                                                                    #endregion
                                                            }
                                                            #endregion
                                                        }
                                                    #endregion
                                                    #endregion

                                                    switch (objTipoComplemento)
                                                    {
                                                        #region Distribuidor.
                                                        case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                            objEntComplementoDistribuidor.TipoComplemento = "Distribucion";

                                                            if (lstEntDistNacional.Count > 0)
                                                                objEntComplementoDistribuidor.Nacional = lstEntDistNacional;

                                                            if (lstEntDistExtranjero.Count > 0)
                                                                objEntComplementoDistribuidor.Extranjero = lstEntDistExtranjero;

                                                            if (lstEntDistNacional.Count > 0 || lstEntDistExtranjero.Count > 0)
                                                                lstEntComplementoDistribucion.Add(objEntComplementoDistribuidor);
                                                            break;
                                                        #endregion

                                                        #region Transportista.
                                                        case CVolJSonDTO.eTipoComplemento.Transportista:
                                                            if (lstEntTransNacional.Count > 0)
                                                                objEntComplementoTransportista.Nacional = lstEntTransNacional;

                                                            if (lstEntTransNacional.Count > 0)
                                                            {
                                                                objEntComplementoTransportista.TipoComplemento = "Transporte";
                                                                lstEntComplementoTransportista.Add(objEntComplementoTransportista);
                                                            }
                                                            break;
                                                        #endregion

                                                        #region Comercializadora.
                                                        case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                            objEntComplementoComercializadora.TipoComplemento = "Comercializacion";

                                                            if (lstEntComerNacional.Count > 0)
                                                                objEntComplementoComercializadora.Nacional = lstEntComerNacional;

                                                            if (lstEntComerExtranjero.Count > 0)
                                                                objEntComplementoComercializadora.Extranjero = lstEntComerExtranjero;

                                                            if (lstEntComerNacional.Count > 0 || lstEntComerExtranjero.Count > 0)
                                                                lstEntComplementoComercializadora.Add(objEntComplementoComercializadora);
                                                            break;
                                                            #endregion
                                                    }
                                                }
                                                #endregion

                                                switch (objTipoComplemento)
                                                {
                                                    #region Distribuidor.
                                                    case CVolJSonDTO.eTipoComplemento.Distribuidor:
                                                        if (lstEntComplementoDistribucion.Count > 0)
                                                            objEntregaMesDato.Complemento = lstEntComplementoDistribucion;
                                                        else
                                                            throw new Exception("Entregas Mes: No se encontraron datos de Facturación, Pedimento, etc. Favor de captura la información para la generación del Complemento.");
                                                        break;
                                                    #endregion

                                                    #region Transportista.
                                                    case CVolJSonDTO.eTipoComplemento.Transportista:
                                                        if (lstEntComplementoTransportista.Count > 0)
                                                            objEntregaMesDato.Complemento = lstEntComplementoTransportista;
                                                        else
                                                            throw new Exception("Entregas Mes: No se encontraron datos de Facturación, Pedimento, etc. Favor de captura la información para la generación del Complemento.");
                                                        break;
                                                    #endregion

                                                    #region Comercializadora.
                                                    case CVolJSonDTO.eTipoComplemento.Comercializadora:
                                                        if (lstEntComplementoComercializadora.Count > 0)
                                                            objEntregaMesDato.Complemento = lstEntComplementoComercializadora;
                                                        else
                                                            throw new Exception("Entregas Mes: No se encontraron datos de Facturación, Pedimento, etc. Favor de captura la información para la generación del Complemento.");
                                                        break;
                                                        #endregion
                                                }
                                                #endregion
                                            }
                                        #endregion

                                        objRepVolMenDato.Entregas = objEntregaMesDato;
                                        #endregion

                                        objProductoDato.ReporteDeVolumenMensual = objRepVolMenDato;
                                    }
                                }
                            #endregion
                        }
                        #endregion

                        #region Calculo Totales Finales.
                        // Cantidad de Tanques.
                        iTotalTanques = (from t in objContext.Tanks
                                         where t.StoreId == viNEstacion && t.Active == true
                                         select t.TankIdi).Count();

                        //iTotalTanques += (from at in objContext.AutoTanques
                        //                  join s in objContext.Stores on viNEstacion equals s.StoreId
                        //                  where at.NumeroEstacion == s.StoreNumber.ToString()
                        //                  select at.NumeroAutoTanque).Count();

                        iTotalDispensarios = (from d in objContext.Dispensaries
                                              where d.StoreId == viNEstacion && d.Active == true
                                              select d.DispensaryIdi).Count();
                        #endregion
                        break;
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

            switch (viTipoReporte) {
                #region TipoReporte: Día.
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
                #endregion

                #region TipoReporte: Mes.
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
                #endregion
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
            String sRutaArchivo = String.Empty;

            if (viProcesoArchivo == CVolJSonDTO.eProcesoArchivo.Guardar)
            {
                sRutaArchivo = GenerarRutaCarpetaArchivo(viRutaCarpetaRaiz: sRutaCarpetaRaiz,
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
            }
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

                        objContext.SaveChanges();
                    }
                    break;
                    #endregion
            }
            #endregion

            String sRespuestaSalida = String.Empty;

            switch (viProcesoArchivo)
            {
                case CVolJSonDTO.eProcesoArchivo.Descargar: sRespuestaSalida = sEstructuraDatos; break;
                case CVolJSonDTO.eProcesoArchivo.Guardar: sRespuestaSalida = "OK|Archivo Generado|" + sRutaArchivo; break;
            }

            return Ok(sRespuestaSalida);
            //return Ok("OK|Archivo Generado");
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

        /// <summary>
        /// Validaa la estructura del CFDI.
        /// </summary>
        /// <param name="viCFDI">CFDI</param>
        /// <returns></returns>
        private static Boolean ValidarCFDI(String viCFDI)
        {
            bool isValid = false;
            if (!string.IsNullOrEmpty(viCFDI))
            {
                Regex isGuid = new Regex(@"^({){0,1}[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}(}){0,1}$", RegexOptions.Compiled);

                if (isGuid.IsMatch(viCFDI))
                    isValid = true;
            }
            return isValid;
        }
        #endregion

        #region Carga Masiva: Excel.
        /// <summary>
        /// Lectura de Archivo de Excel para guardar Recepciones y Entregas.
        /// </summary>
        /// <param name="viNCompania">ID Compañia</param>
        /// <param name="viNEstacion">ID Estación.</param>
        /// <param name="viTipoArchivo">Tipo de Registros "RECEPCION | ENTREGA"</param>
        /// <param name="viArchivo">Archivo Excel</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Importación de Recepciones y Entregas (Excel)")]
        public IActionResult CargaExcel(Guid? viNCompania, Guid? viNEstacion, String viTipoArchivo, IFormFile viArchivo)
        {
            #region Constantes.
            const String TIPO_ARCHIVO_RECEPCION = "RECEPCION";
            const String TIPO_ARCHIVO_ENTREGA = "ENTREGA";

            #region Hojas.
            const String HOJA_GENERAL = "GENERAL";
            const String HOJA_CFDI = "CFDI";
            const String HOJA_PEDIMENTO = "PEDIMENTO";
            const String HOJA_TRANSPORTE = "TRANSPORTE";
            #endregion

            #region Columnas.
            #region Generales.
            const int COLUMNA_GENERAL_NUMERO_FILA = 0;
            const int COLUMNA_GENERAL_NUMERO_DUCTO = 1;
            const int COLUMNA_GENERAL_NUMERO_PRODUCTO = 2;
            const int COLUMNA_GENERAL_VOLUMEN_INICIAL = 3;
            const int COLUMNA_GENERAL_VOLUMEN = 4;
            const int COLUMNA_GENERAL_VOLUMEN_FINAL = 5;
            const int COLUMNA_GENERAL_TEMPERATURA = 6;
            const int COLUMNA_GENERAL_PRESION_ABSOLUTA = 7;
            const int COLUMNA_GENERAL_PODER_CALORIFICO = 8;
            const int COLUMNA_GENERAL_FECHA_INICIAL = 9;
            const int COLUMNA_GENERAL_FECHA_FINAL = 10;
            const int COLUMNA_GENERAL_PRECIO = 11;
            const int COLUMNA_GENERAL_SUBTOTAL = 12;
            const int COLUMNA_GENERAL_IVA = 13;
            const int COLUMNA_GENERAL_IMPORTE = 14;
            #endregion

            #region CFDI.
            const int COLUMNA_CFDI_NUMERO_FILA = 0;
            const int COLUMNA_CFDI_RFC = 1;
            const int COLUMNA_CFDI_NOMBRE = 2;
            const int COLUMNA_CFDI_SERIE = 3;
            const int COLUMNA_CFDI_NUMERO_FACTURA = 4;
            const int COLUMNA_CFDI_TIPO_CFDI = 5;
            const int COLUMNA_CFDI_CFDI = 6;
            const int COLUMNA_CFDI_PRECIO = 7;
            const int COLUMNA_CFDI_IMPORTE = 8;
            const int COLUMNA_CFDI_FECHA_HORA = 9;
            const int COLUMNA_CFDI_VOLUMEN = 10;
            const int COLUMNA_CFDI_NUMERO_PERMISO_CRE = 12;
            #endregion

            #region Pedimento.
            const int COLUMNA_PEDIMENTO_NUMERO_FILA = 0;
            const int COLUMNA_PEDIMENTO_CLAVE_PERMISO = 1;
            const int COLUMNA_PEDIMENTO_PUNTO_INTERNACION = 2;
            const int COLUMNA_PEDIMENTO_PAIS = 3;
            const int COLUMNA_PEDIMENTO_MEDIO = 4;
            const int COLUMNA_PEDIMENTO_CLAVE_PEDIMENTO = 5;
            const int COLUMNA_PEDIMENTO_INCOTERMS = 6;
            const int COLUMNA_PEDIMENTO_PRECIO = 7;
            const int COLUMNA_PEDIMENTO_VOLUMEN = 8;
            #endregion

            #region Transporte.
            const int COLUMNA_TRANSPORTE_NUMERO_FILA = 0;
            const int COLUMNA_TRANSPORTE_PERMISO_TRANSPORTE = 1;
            const int COLUMNA_TRANSPORTE_CLAVE_VEHICULO = 2;
            const int COLUMNA_TRANSPORTE_TARIFA_TRANSPORTE = 3;
            const int COLUMNA_TRANSPORTE_CARGO_CAPACIDAD = 4;
            const int COLUMNA_TRANSPORTE_CARGO_USO = 5;
            const int COLUMNA_TRANSPORTE_CARGO_VOLUMEN = 6;
            const int COLUMNA_TRANSPORTE_TARIFA_SUMINISTRO = 7;
            const int COLUMNA_TRANSPORTE_CONTRAPRESTACION = 8;
            const int COLUMNA_TRANSPORTE_DESCUENTO = 9;
            #endregion
            #endregion
            #endregion

            IExcelDataReader objReader = null;
            Stream objStream = new MemoryStream();
            DataSet dsArchivo = null;
            int iNFilaActual = 0, iCantRegisSave = 0;

            if (viArchivo != null && viArchivo.Length > 0)
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                viArchivo.CopyTo(objStream);
                objStream.Position = 0;
                objReader = null;

                #region Validación: Principales.
                if (viArchivo.FileName.EndsWith(".xls"))
                    objReader = ExcelReaderFactory.CreateBinaryReader(objStream);
                else if (viArchivo.FileName.EndsWith(".xlsx"))
                    objReader = ExcelReaderFactory.CreateOpenXmlReader(objStream);
                else
                    return BadRequest("El Formato del Archivo no es valido.");

                if (!String.IsNullOrEmpty(objReader.ExceptionMessage))
                    return BadRequest(objReader.ExceptionMessage);

                objReader.IsFirstRowAsColumnNames = true;
                dsArchivo = objReader.AsDataSet();

                if (!dsArchivo.Tables.Contains(HOJA_GENERAL))
                    return BadRequest("No se encontro la hoja 'GENERAL' en el documento de Excel.");
                #endregion

                #region Lectura y Llenado de Estructuras.
                DataTable dtCfdi = new DataTable(),
                          dtPedimento = new DataTable(),
                          dtTransporte = new DataTable();

                if (dsArchivo.Tables.Contains(HOJA_CFDI))
                    dtCfdi = dsArchivo.Tables[HOJA_CFDI];

                if (dsArchivo.Tables.Contains(HOJA_PEDIMENTO))
                    dtPedimento = dsArchivo.Tables[HOJA_PEDIMENTO];

                if (dsArchivo.Tables.Contains(HOJA_TRANSPORTE))
                    dtTransporte = dsArchivo.Tables[HOJA_TRANSPORTE];

                foreach (DataRow drGeneral in dsArchivo.Tables[HOJA_GENERAL].Rows)
                {
                    InventoryIn objRecepcionDato = null;
                    SaleOrder objEntregDatos = null;

                    Guid gRecepcionID = Guid.Empty,
                         gEntregaID = Guid.Empty,
                         gProductoID = Guid.Empty,
                         gCfdiID = Guid.Empty,
                         gPedimentoID = Guid.Empty,
                         gTransporteID = Guid.Empty,
                         gTransSupplierID = Guid.Empty;

                    #region Conversion.
                    int iNFila = 0;
                    int.TryParse(drGeneral[COLUMNA_GENERAL_NUMERO_FILA].ToString().Trim(), out iNFila);

                    if (iNFila <= 0)
                        continue;

                    iNFilaActual = iNFila;
                    int iNDucto = 0;
                    Decimal dVolInicial = 0, dVolFinal = 0, dVolumen = 0, dTemperatura = 0, dPresionAbs = 0, dPrecio = 0, dSubtotal = 0, dIVA = 0, dImporte = 0, dPoderCalor = 0;
                    String sNProducto = drGeneral[COLUMNA_GENERAL_NUMERO_PRODUCTO].ToString().Trim(),
                           sUnidadMedida = String.Empty;
                    DateTime dtFInicial = Convert.ToDateTime(drGeneral[COLUMNA_GENERAL_FECHA_INICIAL].ToString().Trim()),
                             dtFFinal = Convert.ToDateTime(drGeneral[COLUMNA_GENERAL_FECHA_FINAL].ToString().Trim()),
                             dtUpdate = DateTime.Now;

                    int iCNFactura = 0;
                    Decimal dCPrecio = 0, dCImporte = 0, dCVolumen = 0;
                    int iPMedIngOSal = 0;
                    Decimal dPPrecio = 0, dPVolumen = 0, dPPuntoInterOExt = 0;
                    Decimal dTTarifa = 0, dTCargoCap = 0, dTCargoUso = 0, dTCargoVol = 0, dTTarifaSum = 0, dTContraPrest = 0, dTDescuento = 0;

                    #region Fecha Inicial.
                    if (drGeneral[COLUMNA_GENERAL_FECHA_INICIAL].GetType().Equals(typeof(Double)))
                    {
                        Double dFInicial = 0;
                        Double.TryParse(drGeneral[COLUMNA_GENERAL_FECHA_INICIAL].ToString().Trim(), out dFInicial);
                        dtFInicial = DateTime.FromOADate(dFInicial);
                    }
                    else
                        DateTime.TryParse(drGeneral[COLUMNA_GENERAL_FECHA_INICIAL].ToString().Trim(), out dtFInicial);
                    #endregion

                    #region Fecha Final.
                    if (drGeneral[COLUMNA_GENERAL_FECHA_FINAL].GetType().Equals(typeof(Double)))
                    {
                        Double dFFinal = 0;
                        Double.TryParse(drGeneral[COLUMNA_GENERAL_FECHA_FINAL].ToString().Trim(), out dFFinal);
                        dtFFinal = DateTime.FromOADate(dFFinal);
                    }
                    else
                        DateTime.TryParse(drGeneral[COLUMNA_GENERAL_FECHA_FINAL].ToString().Trim(), out dtFFinal);
                    #endregion

                    int.TryParse(drGeneral[COLUMNA_GENERAL_NUMERO_FILA].ToString().Trim(), out iNFila);
                    int.TryParse(drGeneral[COLUMNA_GENERAL_NUMERO_DUCTO].ToString().Trim(), out iNDucto);
                    Decimal.TryParse(drGeneral[COLUMNA_GENERAL_VOLUMEN_INICIAL].ToString().Trim(), out dVolInicial);
                    Decimal.TryParse(drGeneral[COLUMNA_GENERAL_VOLUMEN_FINAL].ToString().Trim(), out dVolFinal);
                    Decimal.TryParse(drGeneral[COLUMNA_GENERAL_VOLUMEN].ToString().Trim(), out dVolumen);
                    Decimal.TryParse(drGeneral[COLUMNA_GENERAL_TEMPERATURA].ToString().Trim(), out dTemperatura);
                    Decimal.TryParse(drGeneral[COLUMNA_GENERAL_PRESION_ABSOLUTA].ToString().Trim(), out dPresionAbs);
                    Decimal.TryParse(drGeneral[COLUMNA_GENERAL_PRECIO].ToString().Trim(), out dPrecio);
                    Decimal.TryParse(drGeneral[COLUMNA_GENERAL_SUBTOTAL].ToString().Trim(), out dSubtotal);
                    Decimal.TryParse(drGeneral[COLUMNA_GENERAL_IVA].ToString().Trim(), out dIVA);
                    Decimal.TryParse(drGeneral[COLUMNA_GENERAL_IMPORTE].ToString().Trim(), out dImporte);
                    Decimal.TryParse(drGeneral[COLUMNA_GENERAL_PODER_CALORIFICO].ToString().Trim(), out dPoderCalor);

                    // CFDI
                    Guid gCfdiPersonaID = Guid.Empty;
                    String sCfdiRfc = String.Empty, sCfdiTipoComprobante = String.Empty;
                    #endregion

                    #region Validaciones.
                    DataRow[] drConsultaCfdi = dtCfdi.Select(dtCfdi.Columns[COLUMNA_CFDI_NUMERO_FILA].ColumnName + "='" + iNFila.ToString().Trim() + "'");
                    DataRow[] drConsultaPedimento = dtPedimento.Select(dtPedimento.Columns[COLUMNA_PEDIMENTO_NUMERO_FILA].ColumnName + "='" + iNFila.ToString().Trim() + "'");
                    DataRow[] drConsultaTrans = dtTransporte.Select(dtTransporte.Columns[COLUMNA_TRANSPORTE_NUMERO_FILA].ColumnName + "='" + iNFila.ToString().Trim() + "'");

                    if (drConsultaCfdi.Length == 0 && drConsultaPedimento.Length == 0 && drConsultaTrans.Length == 0)
                        continue;

                    #region Validacion: Hoja "General".
                    String sMnsGeneral = "Fila No. " + iNFila.ToString();

                    if (String.IsNullOrEmpty(sNProducto))
                        sMnsGeneral += ": No se encontro el Número de Producto";

                    if ((from p in objContext.Products where p.ProductCode == sNProducto select p).Count() <= 0)
                        sMnsGeneral += ": No se encontro información del Producto '" + sNProducto + "'";
                    else
                    {
                        Product objProductoDatos = new Product();
                        objProductoDatos = (from p in objContext.Products where p.ProductCode == sNProducto select p).First();

                        gProductoID = objProductoDatos.ProductId;
                        sUnidadMedida = objProductoDatos.JsonClaveUnidadMedidaId;
                    }

                    if (String.IsNullOrEmpty(sUnidadMedida))
                        sMnsGeneral += ": No se encontro la Unidad de Medida del Producto '" + sNProducto + "'";

                    if (sMnsGeneral != "Fila No. " + iNFila.ToString())
                        return BadRequest(sMnsGeneral);
                    #endregion

                    #region Validación: Hoja "CFDI".
                    if (drConsultaCfdi.Length > 0)
                    {
                        String sCFDIMnsInicial = "Fila: '" + iNFila.ToString() + "'. ";

                        String sCFDI_Nombre = drConsultaCfdi[0][COLUMNA_CFDI_NOMBRE].ToString();
                        if (String.IsNullOrEmpty(sCFDI_Nombre))
                            return BadRequest(sCFDIMnsInicial + "Ingresa el Nombre del Cliente (CFDI).");

                        if (String.IsNullOrEmpty(drConsultaCfdi[0][COLUMNA_CFDI_CFDI].ToString()))
                            return BadRequest(sCFDIMnsInicial + "Ingresa el CFDI de la Factura (CFDI).");

                        if (!ValidarCFDI(viCFDI: drConsultaCfdi[0][COLUMNA_CFDI_CFDI].ToString()))
                            return BadRequest(sCFDIMnsInicial + "El CFDI capturado '" + drConsultaCfdi[0][COLUMNA_CFDI_CFDI].ToString() + "' no tiene el formato correcto.");

                        //if (String.IsNullOrEmpty(drConsultaCfdi[0][COLUMNA_CFDI_UNIDAD_MEDIDA].ToString()))
                        //    return BadRequest(sCFDIMnsInicial + "No se ha capturado la Unidad de Medida.");

                        // RFC
                        sCfdiRfc = drConsultaCfdi[0][COLUMNA_CFDI_RFC].ToString().Trim();

                        switch (viTipoArchivo)
                        {
                            #region Recepcion.
                            case TIPO_ARCHIVO_RECEPCION:
                                if ((from s in objContext.Suppliers where s.Rfc == sCfdiRfc select s).Count() <= 0)
                                    return BadRequest(sCFDIMnsInicial + "No se encontro información del RFC '" + sCfdiRfc + "'.");
                                else
                                    gCfdiPersonaID = (from s in objContext.Suppliers where s.Rfc == sCfdiRfc select s).First().SupplierId;
                                break;
                            #endregion

                            #region Entrega.
                            case TIPO_ARCHIVO_ENTREGA:
                                if ((from c in objContext.Customers where c.Rfc == sCfdiRfc select c).Count() <= 0)
                                    return BadRequest(sCFDIMnsInicial + "No se encontro información del RFC '" + sCfdiRfc + "'.");
                                else
                                    gCfdiPersonaID = (from c in objContext.Customers where c.Rfc == sCfdiRfc select c).First().CustomerId;
                                break;
                                #endregion
                        }

                        // Tipo Comprobante
                        sCfdiTipoComprobante = drConsultaCfdi[0][COLUMNA_CFDI_TIPO_CFDI].ToString().Trim();
                        if ((from t in objContext.SatTipoComprobantes where t.Descripcion.ToUpper() == sCfdiTipoComprobante.ToUpper() select t).Count() <= 0)
                            return BadRequest(sCFDIMnsInicial + "(CFDI) No se encontro el tipo de comprobante '" + sCfdiTipoComprobante + "'.");
                        else
                            sCfdiTipoComprobante = (from t in objContext.SatTipoComprobantes where t.Descripcion.ToUpper() == sCfdiTipoComprobante.ToUpper() select t).First().SatTipoComprobanteId;
                    }
                    #endregion

                    #region Validación: Hoja "Pedimento".                    
                    if (drConsultaPedimento.Length > 0)
                    {
                        String sPediMnsInicial = "Fila: '" + iNFila.ToString() + "'.";

                        if (String.IsNullOrEmpty(drConsultaPedimento[0][COLUMNA_PEDIMENTO_CLAVE_PERMISO].ToString()))
                            return BadRequest(sPediMnsInicial + " No se encontro la Clave de Permiso.");

                        if (String.IsNullOrEmpty(drConsultaPedimento[0][COLUMNA_PEDIMENTO_PAIS].ToString()))
                            return BadRequest(sPediMnsInicial + " No se encontro el Pais.");

                        if (String.IsNullOrEmpty(drConsultaPedimento[0][COLUMNA_PEDIMENTO_CLAVE_PEDIMENTO].ToString()))
                            return BadRequest(sPediMnsInicial + " No se encontro la Clave de Pedimento.");

                        if (String.IsNullOrEmpty(drConsultaPedimento[0][COLUMNA_PEDIMENTO_INCOTERMS].ToString()))
                            return BadRequest(sPediMnsInicial + " No se encontro la Incoterms");
                    }
                    #endregion

                    #region Validación: Hoja "Transporte".
                    //(from s in objContext.SupplierTransports
                    // where s.TransportPermission == sTPermisoTransporte
                    // select s).First().SupplierId
                    if(drConsultaTrans.Length > 0)
                    {
                        String sTransMnsInicial = "Fila: '" + iNFila.ToString() + "'.";
                        String sTPermisoTransporte = drConsultaTrans[0][COLUMNA_TRANSPORTE_PERMISO_TRANSPORTE].ToString().Trim();

                        if ((from s in objContext.SupplierTransports where s.StoreId == viNEstacion && s.TransportPermission == sTPermisoTransporte select s).Count() <= 0)
                            return BadRequest(sTransMnsInicial + "(Transporte) No se encontro el Supplier '" + sTPermisoTransporte + "'.");
                        else
                            gTransSupplierID = (from s in objContext.SupplierTransports where s.StoreId == viNEstacion && s.TransportPermission == sTPermisoTransporte select s).First().SupplierId;

                    }

                    #endregion
                    #endregion

                    #region Llenado.
                    switch (viTipoArchivo)
                    {
                        #region Recepcion Datos.
                        case TIPO_ARCHIVO_RECEPCION:
                            objRecepcionDato = new InventoryIn();
                            if ((from i in objContext.InventoryIns where i.StoreId == viNEstacion.GetValueOrDefault() && i.Date == dtFInicial select i).Count() <= 0)
                            {
                                objRecepcionDato.InventoryInId = Guid.NewGuid();
                                objRecepcionDato.StoreId = viNEstacion.GetValueOrDefault();
                                objRecepcionDato.InventoryInNumber = iNFila;
                                objRecepcionDato.TankIdi = iNDucto;
                                objRecepcionDato.ProductId = (from p in objContext.Products where p.ProductCode == sNProducto select p).First().ProductId;
                                objRecepcionDato.Date = dtFInicial;
                                objRecepcionDato.StartVolume = dVolInicial;
                                objRecepcionDato.Volume = dVolumen;
                                objRecepcionDato.EndVolume = dVolFinal;
                                objRecepcionDato.StartTemperature = dTemperatura;
                                objRecepcionDato.AbsolutePressure = dPresionAbs;
                                objRecepcionDato.CalorificPower = dPoderCalor;
                                objRecepcionDato.StartDate = dtFInicial;
                                objRecepcionDato.EndDate = dtFFinal;

                                objRecepcionDato.Active = true;
                                objRecepcionDato.Updated = dtUpdate;

                                objContext.InventoryIns.Add(objRecepcionDato);
                                objContext.SaveChanges();
                            }
                            else
                                objRecepcionDato = (from i in objContext.InventoryIns where i.StoreId == viNEstacion.GetValueOrDefault() && i.Date == dtFInicial select i).First();

                            gRecepcionID = objRecepcionDato.InventoryInId;
                            break;
                        #endregion

                        #region Entrega Datos.
                        case TIPO_ARCHIVO_ENTREGA:
                            objEntregDatos = new SaleOrder();
                            if ((from e in objContext.SaleOrders where e.StoreId == viNEstacion && e.Date == dtFInicial select e).Count() <= 0)
                            {
                                objEntregDatos.StoreId = viNEstacion.GetValueOrDefault();
                                objEntregDatos.SaleOrderId = Guid.NewGuid();
                                objEntregDatos.SaleOrderNumber = iNFila;
                                objEntregDatos.Name = String.Empty;
                                //objEntregDatos.EmployeeId = Guid.Empty;
                                objEntregDatos.SaleOrderNumberStart = iNFila;
                                //objEntregDatos.CustomerControlId = Guid.Empty;
                                //objEntregDatos.HoseIdi = 0;
                                //objEntregDatos.ShiftId = Guid.Empty;
                                //objEntregDatos.VehicleId = Guid.Empty;
                                //objEntregDatos.CardEmployeeId = String.Empty;
                                objEntregDatos.Amount = dImporte;
                                objEntregDatos.StartDate = dtFInicial;
                                objEntregDatos.Date = dtFInicial;
                                objEntregDatos.TankIdi = iNDucto;

                                objEntregDatos.Updated = dtUpdate;
                                objEntregDatos.Active = true;
                                objEntregDatos.Locked = false;
                                objEntregDatos.Deleted = false;

                                // <DUDA> GUARDAR Entrega (SaleOrder)
                                objContext.SaleOrders.Add(objEntregDatos);
                                objContext.SaveChanges();
                                gEntregaID = objEntregDatos.SaleOrderId;
                            }
                            else
                                gEntregaID = (from e in objContext.SaleOrders where e.StoreId == viNEstacion.GetValueOrDefault() && e.Date == dtFInicial select e).First().SaleOrderId;
                            break;
                            #endregion
                    }

                    #region CFDI Datos.
                    if (drConsultaCfdi.Length > 0)
                    {
                        int.TryParse(drConsultaCfdi[0][COLUMNA_CFDI_NUMERO_FACTURA].ToString().Trim(), out iCNFactura);
                        Decimal.TryParse(drConsultaCfdi[0][COLUMNA_CFDI_PRECIO].ToString().Trim(), out dCPrecio);
                        Decimal.TryParse(drConsultaCfdi[0][COLUMNA_CFDI_IMPORTE].ToString().Trim(), out dCImporte);
                        Decimal.TryParse(drConsultaCfdi[0][COLUMNA_CFDI_VOLUMEN].ToString().Trim(), out dCVolumen);
                        DateTime dtFFactura = Convert.ToDateTime(drConsultaCfdi[0][COLUMNA_CFDI_FECHA_HORA].ToString().Trim());
                        String sFSerie = drConsultaCfdi[0][COLUMNA_CFDI_SERIE].ToString().Trim();

                        #region Factura: Invoice.
                        Invoice objFacturaHdrDato = new Invoice();
                        if ((from i in objContext.Invoices where i.StoreId == viNEstacion.GetValueOrDefault() && i.InvoiceSerieId == sFSerie && i.Folio == iCNFactura.ToString().Trim() && i.Date == dtFFactura select i).Count() <= 0)
                        {
                            objFacturaHdrDato.InvoiceId = Guid.NewGuid();
                            objFacturaHdrDato.StoreId = viNEstacion.GetValueOrDefault();
                            switch (viTipoArchivo)
                            {
                                case TIPO_ARCHIVO_RECEPCION: objFacturaHdrDato.SupplierId = gCfdiPersonaID; break;
                                case TIPO_ARCHIVO_ENTREGA: objFacturaHdrDato.CustomerId = gCfdiPersonaID; break;
                            }
                            objFacturaHdrDato.InvoiceSerieId = drConsultaCfdi[0][COLUMNA_CFDI_SERIE].ToString().Trim();
                            objFacturaHdrDato.Folio = iCNFactura.ToString().Trim();
                            objFacturaHdrDato.SatTipoComprobanteId = sCfdiTipoComprobante;
                            objFacturaHdrDato.Uuid = drConsultaCfdi[0][COLUMNA_CFDI_CFDI].ToString().Trim();
                            objFacturaHdrDato.Subtotal = dSubtotal;
                            objFacturaHdrDato.AmountTax = dIVA;
                            objFacturaHdrDato.Amount = dCImporte;

                            objFacturaHdrDato.Active = true;
                            objFacturaHdrDato.Date = dtUpdate;
                            objFacturaHdrDato.Updated = dtUpdate;
                            objFacturaHdrDato.Locked = false;
                            objFacturaHdrDato.Deleted = false;

                            if (drConsultaCfdi[0][COLUMNA_CFDI_FECHA_HORA].GetType().Equals(typeof(Double)))
                            {
                                Double dCfdiFecha = 0;
                                Double.TryParse(drConsultaCfdi[0][COLUMNA_CFDI_FECHA_HORA].ToString().Trim(), out dCfdiFecha);
                                objFacturaHdrDato.Date = DateTime.FromOADate(dCfdiFecha);
                            }
                            else
                                objFacturaHdrDato.Date = Convert.ToDateTime(drConsultaCfdi[0][COLUMNA_CFDI_FECHA_HORA].ToString().Trim());

                            // <DUDA> GUARDAR Invoice
                            objContext.Invoices.Add(objFacturaHdrDato);
                            objContext.SaveChanges();

                            gCfdiID = objFacturaHdrDato.InvoiceId;
                        }
                        else
                            gCfdiID = (from i in objContext.Invoices
                                       where i.StoreId == viNEstacion.GetValueOrDefault() &&
                                             i.InvoiceSerieId == sFSerie &&
                                             i.Folio == iCNFactura.ToString().Trim() &&
                                             i.Date == dtFFactura
                                       select i).First().InvoiceId;
                        #endregion

                        #region Factura Detalle: Invoice Detail.
                        InvoiceDetail objFacturaDtlDato = new InvoiceDetail();
                        if ((from d in objContext.InvoiceDetails where d.InvoiceId == gCfdiID && 
                                                                       d.ProductId == gProductoID && 
                                                                       d.Price == dCPrecio &&
                                                                       d.Quantity == dCVolumen &&
                                                                       d.Amount == dCImporte
                                                                       select d).Count() <= 0) {
                            objFacturaDtlDato.InvoiceDetailIdi = (from d in objContext.InvoiceDetails where d.InvoiceId == gCfdiID select d).Count() + 1;
                            objFacturaDtlDato.InvoiceId = gCfdiID;
                            objFacturaDtlDato.ProductId = gProductoID;
                            objFacturaDtlDato.Date = dtUpdate;
                            objFacturaDtlDato.Quantity = dCVolumen;
                            objFacturaDtlDato.Price = dCPrecio;
                            objFacturaDtlDato.Subtotal = dSubtotal;
                            objFacturaDtlDato.AmountTax = dIVA;
                            objFacturaDtlDato.Amount = dCImporte;

                            objFacturaDtlDato.Active = 1;
                            objFacturaDtlDato.Updated = dtUpdate;
                            objFacturaDtlDato.Locked = 0;
                            objFacturaDtlDato.Deleted = 0;

                            // <DUDA> GUARDAR Invoice Detail
                            objContext.InvoiceDetails.Add(objFacturaDtlDato);
                            objContext.SaveChanges();

                            if(objFacturaDtlDato.InvoiceDetailIdi > 1)
                            {
                                Invoice objFacturaUPD = (from i in objContext.Invoices
                                                         where i.StoreId == viNEstacion.GetValueOrDefault() &&
                                                               i.InvoiceSerieId == sFSerie &&
                                                               i.Folio == iCNFactura.ToString().Trim() &&
                                                               i.Date == dtFFactura
                                                         select i).First();

                                objFacturaUPD.Amount = (from d in objContext.InvoiceDetails where d.InvoiceId == gCfdiID select d).Sum(d => d.Amount);
                                objContext.Invoices.Update(objFacturaUPD);
                                objContext.SaveChanges();
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region Pedimento Datos.
                    if (drConsultaPedimento.Length > 0)
                    {
                        Decimal.TryParse(drConsultaPedimento[0][COLUMNA_PEDIMENTO_PUNTO_INTERNACION].ToString().Trim(), out dPPuntoInterOExt);
                        int.TryParse(drConsultaPedimento[0][COLUMNA_PEDIMENTO_MEDIO].ToString().Trim(), out iPMedIngOSal);
                        Decimal.TryParse(drConsultaPedimento[0][COLUMNA_PEDIMENTO_PRECIO].ToString().Trim(), out dPPrecio);
                        Decimal.TryParse(drConsultaPedimento[0][COLUMNA_PEDIMENTO_VOLUMEN].ToString().Trim(), out dPVolumen);
                        String sPClaveDeclaracion = drConsultaPedimento[0][COLUMNA_PEDIMENTO_CLAVE_PEDIMENTO].ToString().Trim();

                        PetitionCustom objPedimentoDatos = new PetitionCustom();
                        if ((from p in objContext.PetitionCustoms where p.NumberCustomsDeclaration == sPClaveDeclaracion && p.Date == dtFInicial select p).Count() <= 0)
                        {
                            objPedimentoDatos.PetitionCustomsId = Guid.NewGuid();
                            objPedimentoDatos.KeyOfImportationExportation = drConsultaPedimento[0][COLUMNA_PEDIMENTO_CLAVE_PERMISO].ToString().Trim();
                            objPedimentoDatos.KeyPointOfInletOrOulet = dPPuntoInterOExt;
                            objPedimentoDatos.SatPaisId = drConsultaPedimento[0][COLUMNA_PEDIMENTO_PAIS].ToString().Trim();
                            objPedimentoDatos.TransportMediumnCustomsId = iPMedIngOSal;
                            objPedimentoDatos.NumberCustomsDeclaration = drConsultaPedimento[0][COLUMNA_PEDIMENTO_CLAVE_PEDIMENTO].ToString().Trim();
                            objPedimentoDatos.Incoterms = drConsultaPedimento[0][COLUMNA_PEDIMENTO_INCOTERMS].ToString().Trim();
                            objPedimentoDatos.QuantityDocumented = dPVolumen;
                            objPedimentoDatos.AmountOfImportationExportation = dPPrecio;
                            objPedimentoDatos.JsonClaveUnidadMedidadId = sUnidadMedida;

                            objPedimentoDatos.Active = 1;
                            objPedimentoDatos.Date = dtFInicial;
                            objPedimentoDatos.Updated = dtUpdate;
                            objPedimentoDatos.Locked = 0;
                            objPedimentoDatos.Deleted = 0;

                            // <DUDA> Guardar Pedimento.
                            objContext.PetitionCustoms.Add(objPedimentoDatos);
                            objContext.SaveChanges();
                            gPedimentoID = objPedimentoDatos.PetitionCustomsId;
                        }
                        else
                            gPedimentoID = (from p in objContext.PetitionCustoms
                                            where p.NumberCustomsDeclaration == sPClaveDeclaracion && p.Date >= dtFInicial
                                            select p).First().PetitionCustomsId;
                    }
                    #endregion

                    #region Transporte Datos.
                    if (drConsultaTrans.Length > 0)
                    {
                        //String sTPermisoTransporte = drConsultaTransporte[0][COLUMNA_TRANSPORTE_PERMISO_TRANSPORTE].ToString().Trim();
                        Decimal.TryParse(drConsultaTrans[0][COLUMNA_TRANSPORTE_TARIFA_TRANSPORTE].ToString().Trim(), out dTTarifa);
                        Decimal.TryParse(drConsultaTrans[0][COLUMNA_TRANSPORTE_CARGO_CAPACIDAD].ToString().Trim(), out dTCargoCap);
                        Decimal.TryParse(drConsultaTrans[0][COLUMNA_TRANSPORTE_CARGO_USO].ToString().Trim(), out dTCargoUso);
                        Decimal.TryParse(drConsultaTrans[0][COLUMNA_TRANSPORTE_CARGO_VOLUMEN].ToString().Trim(), out dTCargoVol);
                        Decimal.TryParse(drConsultaTrans[0][COLUMNA_TRANSPORTE_TARIFA_SUMINISTRO].ToString().Trim(), out dTTarifaSum);
                        Decimal.TryParse(drConsultaTrans[0][COLUMNA_TRANSPORTE_CONTRAPRESTACION].ToString().Trim(), out dTContraPrest);
                        Decimal.TryParse(drConsultaTrans[0][COLUMNA_TRANSPORTE_DESCUENTO].ToString().Trim(), out dTDescuento);

                        SupplierTransportRegister objTransporteDatos = new SupplierTransportRegister();
                        if ((from t in objContext.SupplierTransportRegisters where t.Date == dtFInicial select t).Count() <= 0)
                        {
                            objTransporteDatos.SupplierTransportRegisterId = Guid.NewGuid();
                            objTransporteDatos.SupplierId = gTransSupplierID; // *<DUDA>
                            objTransporteDatos.AmountPerFee = dTTarifa;
                            objTransporteDatos.AmountPerCapacity = dTCargoCap;
                            objTransporteDatos.AmountPerUse = dTCargoUso;
                            objTransporteDatos.AmountPerVolume = dTCargoVol;
                            objTransporteDatos.AmountPerService = dTContraPrest;
                            objTransporteDatos.AmountOfDiscount = dTDescuento;

                            objTransporteDatos.Active = 1;
                            objTransporteDatos.Date = dtFInicial;
                            objTransporteDatos.Updated = objTransporteDatos.Date;
                            objTransporteDatos.Deleted = 0;
                            objTransporteDatos.Locked = 0;

                            // GUARDAR Transporte
                            objContext.SupplierTransportRegisters.Add(objTransporteDatos);
                            objContext.SaveChanges();
                            gTransporteID = objTransporteDatos.SupplierTransportRegisterId;
                        }
                        else
                            gTransporteID = (from t in objContext.SupplierTransportRegisters
                                             where t.Date == objTransporteDatos.Date
                                             select t).First().SupplierTransportRegisterId;
                    }
                    #endregion

                    switch (viTipoArchivo)
                    {
                        #region Recepción.
                        case TIPO_ARCHIVO_RECEPCION:
                            // <DUDA> Guardar Recepcion Documento.
                            if ((from d in objContext.InventoryInDocuments where d.StoreId == viNEstacion && d.InventoryInId == gRecepcionID select d).Count() <= 0)
                            {
                                InventoryInDocument objRecepcionDocument = new InventoryInDocument();
                                objRecepcionDocument.StoreId = viNEstacion.GetValueOrDefault();
                                objRecepcionDocument.InventoryInId = gRecepcionID;
                                objRecepcionDocument.Type = String.Empty;
                                objRecepcionDocument.Folio = objRecepcionDato.InventoryInNumber.GetValueOrDefault();
                                objRecepcionDocument.Price = dPrecio;
                                objRecepcionDocument.Amount = dImporte;
                                objRecepcionDocument.JsonClaveUnidadMedidaId = sUnidadMedida;
                                objRecepcionDocument.Volume = dVolumen;

                                objRecepcionDocument.Active = true;
                                objRecepcionDocument.Date = dtUpdate;
                                objRecepcionDocument.Updated = dtUpdate;
                                objRecepcionDocument.Deleted = false;
                                objRecepcionDocument.Locked = false;

                                objRecepcionDocument.InvoiceId = gCfdiID;
                                objRecepcionDocument.PetitionCustomsId = gPedimentoID;
                                objRecepcionDocument.SupplierTransportRegisterId = gTransporteID;

                                objContext.InventoryInDocuments.Add(objRecepcionDocument);
                                objContext.SaveChanges();
                            }
                            break;
                        #endregion

                        #region Entrega.
                        case TIPO_ARCHIVO_ENTREGA:
                            if ((from d in objContext.SaleSuborders where d.SaleOrderId == gEntregaID select d).Count() <= 0)
                            {
                                SaleSuborder objEntregaDetalle = new SaleSuborder();
                                objEntregaDetalle.SaleOrderId = gEntregaID;
                                objEntregaDetalle.Name = String.Empty;
                                objEntregaDetalle.ProductId = gProductoID;
                                objEntregaDetalle.Quantity = dVolumen;
                                objEntregaDetalle.Amount = dImporte;
                                objEntregaDetalle.Price = dPrecio;
                                objEntregaDetalle.Discount = 0;
                                objEntregaDetalle.Subtotal = 0;
                                objEntregaDetalle.TotalAmount = dImporte;
                                objEntregaDetalle.TotalQuantity = dVolumen;
                                objEntregaDetalle.TotalAmountElectronic = 0;
                                objEntregaDetalle.TotalQuantityElectronic = 0;
                                objEntregaDetalle.PresetType = 0;
                                objEntregaDetalle.PresetQuantity = 0;
                                objEntregaDetalle.PresetValue = 0;
                                objEntregaDetalle.Temperature = dTemperatura;
                                objEntregaDetalle.QuantityTc = 0;
                                objEntregaDetalle.AbsolutePressure = dPresionAbs;
                                objEntregaDetalle.CalorificPower = dPoderCalor;
                                objEntregaDetalle.ProductCompositionId = 0;
                                objEntregaDetalle.StartQuantity = dVolInicial;
                                objEntregaDetalle.EndQuantity = dVolFinal;

                                objEntregaDetalle.InvoiceId = gCfdiID;
                                objEntregaDetalle.SupplierTransportRegisterId = gTransporteID;
                                objEntregaDetalle.PetitionCustomsId = gPedimentoID;

                                objEntregaDetalle.Date = dtUpdate;
                                objEntregaDetalle.Updated = dtUpdate;
                                objEntregaDetalle.Active = true;
                                objEntregaDetalle.Locked = false;
                                objEntregaDetalle.Deleted = false;

                                // Guardar Entrega
                                objContext.SaleSuborders.Add(objEntregaDetalle);
                                objContext.SaveChanges();
                            }
                            break;
                            #endregion
                    }
                    #endregion

                    iCantRegisSave++;
                }
                #endregion
            }

            return Ok("Registros Guardados: " + iCantRegisSave);
        }
        #endregion

        #region Carga Tanques: Excel.
        [AllowAnonymous]
        [HttpPost("Importación de Tanques (Excel)")]
        public IActionResult ImportarTanquesXlsx(IFormFile viArchivo)
        {
            #region Constantes.
            string COLUMNA_NUMERO_FILA = "tank_idx",
                    COLUMNA_STORE_ID = "store_id",
                    COLUMNA_TANK_IDI = "tank_idi",
                    COLUMNA_PRODUCT_ID = "product_id",
                    COLUMNA_TANK_CPU_ADDRESS = "tank_cpu_address",
                    COLUMNA_PORT_IDI = "port_idi",
                    COLUMNA_TANK_BRAND_ID = "tank_brand_id",
                    COLUMNA_NAME = "name",
                    COLUMNA_CAPACITY_TOTAL = "capacity_total",
                    COLUMNA_CAPACITY_OPERATIONAL = "capacity_operational",
                    COLUMNA_CAPACITY_MINIMUM_OPERATING = "capacity_minimum_operating",
                    COLUMNA_CAPACITY_USEFUL = "capacity_useful",
                    COLUMNA_FONDAGE = "fondage",
                    COLUMNA_SAT_DATE_CALIBRATION = "sat_date_calibration",
                    COLUMNA_SAT_TYPE_MEASUREMENT = "sat_type_measurement",
                    COLUMNA_SAT_TANK_TYPE = "sat_tank_type",
                    COLUMNA_SAT_TYPE_MEDIUM_STORAGE = "sat_type_medium_storage",
                    COLUMNA_SAT_DESCRIPTION_MEASUREMENT = "sat_description_measurement";
            #endregion

            IExcelDataReader objReader = null;
            Stream objStream = new MemoryStream();
            DataSet dsArchivo = null;
            int iCantRegisSave = 0;

            if (viArchivo != null && viArchivo.Length > 0)
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                viArchivo.CopyTo(objStream);
                objStream.Position = 0;
                objReader = null;

                #region Validación: Principales.
                if (viArchivo.FileName.EndsWith(".xls"))
                    objReader = ExcelReaderFactory.CreateBinaryReader(objStream);
                else if (viArchivo.FileName.EndsWith(".xlsx"))
                    objReader = ExcelReaderFactory.CreateOpenXmlReader(objStream);
                else
                    return BadRequest("El Formato del Archivo no es valido.");

                if (!String.IsNullOrEmpty(objReader.ExceptionMessage))
                    return BadRequest(objReader.ExceptionMessage);

                objReader.IsFirstRowAsColumnNames = true;
                dsArchivo = objReader.AsDataSet();

                if (dsArchivo.Tables.Count <= 0)
                    return BadRequest("No se encontro alguna hoja en el documento de Excel.");
                #endregion

                #region Lectura: Archivo.
                foreach (DataRow drGeneral in dsArchivo.Tables[0].Rows)
                {
                    #region Asignación de valores.
                    int iNFila = 0, iTankIdi = 0, iTankCpuAddress = 0, iPortIdi = 0, iTankBrandId = 0,
                        iCapacityTotal = 0, iCapacityOperational = 0, iCapacityMinimumOperating = 0, iCapacityUseful = 0, iFondage = 0;
                    Guid gStoreID = Guid.Empty, gProductId = Guid.Empty;
                    String sName = String.Empty, sSatTypeMeasurement = String.Empty, sSatTankType = String.Empty, sSatTypeMediumStorage = String.Empty, sSatDescriptionMeasurement = String.Empty;
                    //Decimal dCapacityTotal = , dCapacityOperational = 0, dCapacityMinimumOperating = 0, dCapacityUseful = 0, dFondage = 0;
                    DateTime dtSatDateCalibration;

                    #region Número de Fila.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_NUMERO_FILA))
                        int.TryParse(drGeneral[COLUMNA_NUMERO_FILA].ToString().Trim(), out iNFila);
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_NUMERO_FILA + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");

                    if (iNFila <= 0)
                        continue;
                    #endregion

                    String sNFilaActual = "Fila: " + iNFila.ToString() + " - ";

                    #region StoreID.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_STORE_ID))
                    {
                        if (String.IsNullOrEmpty(drGeneral[COLUMNA_STORE_ID].ToString().Trim()))
                            return BadRequest(sNFilaActual + "Captura el dato '" + COLUMNA_STORE_ID + "' del documento '" + viArchivo.FileName + "'.");

                        gStoreID = Guid.Parse(drGeneral[COLUMNA_STORE_ID].ToString().Trim());
                    }
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_STORE_ID + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region TankIdi.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_TANK_IDI))
                    {
                        int.TryParse(drGeneral[COLUMNA_TANK_IDI].ToString().Trim(), out iTankIdi);

                        if (iTankIdi <= 0)
                            return BadRequest(sNFilaActual + "Captura el dato '" + COLUMNA_TANK_IDI + "' del documento '" + viArchivo.FileName + "'.");
                    }
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_TANK_IDI + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region ProductId.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_PRODUCT_ID))
                    {
                        if (String.IsNullOrEmpty(drGeneral[COLUMNA_PRODUCT_ID].ToString().Trim()))
                            return BadRequest(sNFilaActual + "Captura el dato '" + gProductId.ToString() + "' del documento '" + viArchivo.FileName + "'.");

                        gProductId = Guid.Parse(drGeneral[COLUMNA_PRODUCT_ID].ToString().Trim());
                    }
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_PRODUCT_ID + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Tank CPU Address.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_TANK_CPU_ADDRESS))
                        int.TryParse(drGeneral[COLUMNA_TANK_CPU_ADDRESS].ToString().Trim(), out iTankCpuAddress);
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_TANK_CPU_ADDRESS + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region PortIdi.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_PORT_IDI))
                    {
                        int.TryParse(drGeneral[COLUMNA_PORT_IDI].ToString().Trim(), out iPortIdi);

                        if (iPortIdi <= 0)
                            return BadRequest(sNFilaActual + "Captura el dato '" + iPortIdi + "' del documento '" + viArchivo.FileName + "'.");
                    }
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_PORT_IDI + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region TankBrandId.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_TANK_BRAND_ID))
                    {
                        int.TryParse(drGeneral[COLUMNA_TANK_BRAND_ID].ToString().Trim(), out iTankBrandId);

                        if (iTankBrandId <= 0)
                            return BadRequest(sNFilaActual + "Captura el dato '" + iTankBrandId + "' del documento '" + viArchivo.FileName + "'.");
                    }
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_TANK_BRAND_ID + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Name.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_NAME))
                        sName = drGeneral[COLUMNA_NAME].ToString().Trim();
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_NAME + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Capacity Total.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_CAPACITY_TOTAL))
                        int.TryParse(drGeneral[COLUMNA_CAPACITY_TOTAL].ToString().Trim(), out iCapacityTotal);
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_CAPACITY_TOTAL + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Capacity Operational.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_CAPACITY_OPERATIONAL))
                        int.TryParse(drGeneral[COLUMNA_CAPACITY_OPERATIONAL].ToString().Trim(), out iCapacityOperational);
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_CAPACITY_OPERATIONAL + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Capacity Minimum Operating.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_CAPACITY_MINIMUM_OPERATING))
                        int.TryParse(drGeneral[COLUMNA_CAPACITY_MINIMUM_OPERATING].ToString().Trim(), out iCapacityMinimumOperating);
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_CAPACITY_MINIMUM_OPERATING + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Capacity Useful.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_CAPACITY_USEFUL))
                        int.TryParse(drGeneral[COLUMNA_CAPACITY_USEFUL].ToString().Trim(), out iCapacityUseful);
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_CAPACITY_USEFUL + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Fondage.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_FONDAGE))
                        int.TryParse(drGeneral[COLUMNA_FONDAGE].ToString().Trim(), out iFondage);
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_FONDAGE + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Sat Date Calibration.
                    if (!dsArchivo.Tables[0].Columns.Contains(COLUMNA_SAT_DATE_CALIBRATION))
                        return BadRequest("No se encontro la columna '" + COLUMNA_SAT_DATE_CALIBRATION + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    else
                    {
                        if (drGeneral[COLUMNA_SAT_DATE_CALIBRATION].GetType().Equals(typeof(Double)))
                        {
                            Double dFCalibracion = 0;
                            Double.TryParse(drGeneral[COLUMNA_SAT_DATE_CALIBRATION].ToString().Trim(), out dFCalibracion);
                            dtSatDateCalibration = DateTime.FromOADate(dFCalibracion);
                        }
                        else
                            DateTime.TryParse(drGeneral[COLUMNA_SAT_DATE_CALIBRATION].ToString().Trim(), out dtSatDateCalibration);
                    }
                    #endregion

                    #region Sat Type Measurement.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_SAT_TYPE_MEASUREMENT))
                    {
                        sSatTypeMeasurement = drGeneral[COLUMNA_SAT_TYPE_MEASUREMENT].ToString().Trim();

                        if (String.IsNullOrEmpty(sSatTypeMeasurement))
                            return BadRequest(sNFilaActual + "Captura el dato '" + COLUMNA_SAT_TYPE_MEASUREMENT + "'.");
                    }
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_SAT_TYPE_MEASUREMENT + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Sat Tank Type.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_SAT_TANK_TYPE))
                    {
                        sSatTankType = drGeneral[COLUMNA_SAT_TANK_TYPE].ToString().Trim();

                        if (String.IsNullOrEmpty(sSatTankType))
                            return BadRequest(sNFilaActual + "Captura el dato '" + COLUMNA_SAT_TANK_TYPE + "' del documento '" + viArchivo.FileName + "'.");
                    }
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_SAT_TANK_TYPE + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Sat Type Medium Storage.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_SAT_TYPE_MEDIUM_STORAGE))
                    {
                        sSatTypeMediumStorage = drGeneral[COLUMNA_SAT_TYPE_MEDIUM_STORAGE].ToString().Trim();

                        if (String.IsNullOrEmpty(sSatTypeMediumStorage))
                            return BadRequest(sNFilaActual + "Captura el dato '" + COLUMNA_SAT_TYPE_MEDIUM_STORAGE + "' del documento '" + viArchivo.FileName + "'.");
                    }
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_SAT_TYPE_MEDIUM_STORAGE + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion

                    #region Sat Description Measurement.
                    if (dsArchivo.Tables[0].Columns.Contains(COLUMNA_SAT_DESCRIPTION_MEASUREMENT))
                        sSatDescriptionMeasurement = drGeneral[COLUMNA_SAT_DESCRIPTION_MEASUREMENT].ToString().Trim();
                    else
                        return BadRequest("No se encontro la columna '" + COLUMNA_SAT_DESCRIPTION_MEASUREMENT + "' en la hoja '" + dsArchivo.Tables[0].TableName + "' del documento '" + viArchivo.FileName + "'.");
                    #endregion
                    #endregion

                    #region Validacion de Datos.
                    #region StoreID.
                    if ((from s in objContext.Stores where s.StoreId == gStoreID select s).Count() <= 0)
                        return BadRequest(sNFilaActual + "No se encontro la Estación '" + gStoreID.ToString() + "'.");
                    #endregion

                    #region TankIdi.
                    //if ((from t in objContext.Tanks where t.StoreId == gStoreID && t.TankIdi == iTankIdi select t).Count() <= 0)
                    //    return BadRequest(sNFilaActual + "No se encontro Tanque '" + iTankIdi.ToString() + "'.");
                    #endregion

                    #region ProductID.
                    if ((from p in objContext.Products where p.ProductId == gProductId select p).Count() <= 0)
                        return BadRequest(sNFilaActual + "No se encontro el Producto '" + gProductId.ToString() + "'.");
                    #endregion

                    #region Port Idi.
                    if ((from p in objContext.Ports where p.StoreId == gStoreID && p.PortIdi == iPortIdi select p).Count() <= 0)
                        return BadRequest(sNFilaActual + "No se encontro el Puerto '" + iPortIdi.ToString() + "'.");
                    #endregion

                    #region Tank Brand Id.
                    if ((from b in objContext.TankBrands where b.TankBrandId == iTankBrandId select b).Count() <= 0)
                        return BadRequest(sNFilaActual + "No se encontro TankBrand '" + iTankBrandId.ToString() + "'.");
                    #endregion

                    #region Sat Type Measurement.
                    if ((from s in objContext.JsonTipoSistemaMedicions where s.JsonTipoSistemaMedicionId == sSatTypeMeasurement select s).Count() <= 0)
                        return BadRequest(sNFilaActual + "No se encontro " + COLUMNA_SAT_TYPE_MEASUREMENT + " '" + sSatTypeMeasurement + "'.");
                    #endregion

                    #region Sat Tank Type.
                    if ((from s in objContext.JsonTipoTanques where s.JsonTipoTanqueId == sSatTankType select s).Count() <= 0)
                        return BadRequest(sNFilaActual + "No se encontro " + COLUMNA_SAT_TANK_TYPE + " '" + sSatTankType + "'.");
                    #endregion

                    #region Sat Type Medium Storage.
                    if ((from s in objContext.JsonTipoMedioAlmacenamientos where s.JsonTipoMedioAlmacenamientoId == sSatTypeMediumStorage select s).Count() <= 0)
                        return BadRequest(sNFilaActual + "No se encontro " + COLUMNA_SAT_TYPE_MEDIUM_STORAGE + " '" + sSatTypeMediumStorage + "'.");
                    #endregion
                    #endregion

                    #region Llenado.
                    Tank objTanqueDatos = new Tank();
                    Boolean bTankNuevo = false;

                    if((from t in objContext.Tanks where t.StoreId == gStoreID && t.TankIdi == iTankIdi select t).Count() <= 0)
                    {
                        objTanqueDatos.StoreId = gStoreID;
                        objTanqueDatos.TankIdi = iTankIdi;

                        objTanqueDatos.Date = DateTime.Now;
                        objTanqueDatos.Updated = objTanqueDatos.Date;
                        objTanqueDatos.Active = true;
                        objTanqueDatos.Locked = false;
                        objTanqueDatos.Deleted = false;
                        bTankNuevo = true;
                    }
                    else
                    {
                        objTanqueDatos = (from t in objContext.Tanks where t.StoreId == gStoreID && t.TankIdi == iTankIdi select t).First();
                        objTanqueDatos.Updated = DateTime.Now;
                    }

                    objTanqueDatos.ProductId = gProductId;
                    objTanqueDatos.TankCpuAddress = iTankCpuAddress;
                    objTanqueDatos.PortIdi = iPortIdi;
                    objTanqueDatos.TankBrandId = iTankBrandId;
                    objTanqueDatos.Name = sName;
                    objTanqueDatos.CapacityTotal = iCapacityTotal;
                    objTanqueDatos.CapacityOperational = iCapacityOperational;
                    objTanqueDatos.CapacityMinimumOperating = iCapacityMinimumOperating;
                    objTanqueDatos.CapacityUseful = iCapacityUseful;
                    objTanqueDatos.Fondage = iFondage;
                    objTanqueDatos.SatDateCalibration = dtSatDateCalibration;
                    objTanqueDatos.SatTypeMeasurement = sSatTypeMeasurement;
                    objTanqueDatos.SatTankType = sSatTankType;
                    objTanqueDatos.SatTypeMediumStorage = sSatTypeMediumStorage;
                    objTanqueDatos.SatDescriptionMeasurement = sSatDescriptionMeasurement;

                    if (bTankNuevo)
                        objContext.Tanks.Add(objTanqueDatos);
                    else
                        objContext.Tanks.Update(objTanqueDatos);

                    objContext.SaveChanges();
                    iCantRegisSave++;
                    #endregion
                }
                #endregion
            }

            return Ok("Registros Guardados: " + iCantRegisSave);
        }
        #endregion
    }
}