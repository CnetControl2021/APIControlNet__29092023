namespace APIControlNet.DTOs
{
    /// <summary>
    /// Modelo de Datos para la creación del JSon para reporte de Control Volumetrico.
    /// </summary>
    public class CVolJSonDTO
    {
        #region Constantes.
        #region Caracter.
        /// <summary>
        /// Caracter: contratista.
        /// </summary>
        public const String CARACTER_CONTRATISTA = "contratista";
        /// <summary>
        /// Caracter: asignatario.
        /// </summary>
        public const String CARACTER_ASIGNATARIO = "asignatario";
        /// <summary>
        /// Caracter: permisionario.
        /// </summary>
        public const String CARACTER_PERMISIONARIO = "permisionario";
        /// <summary>
        /// Caracter: usuario.
        /// </summary>
        public const String CARACTER_USUARIO = "usuario";
        #endregion

        #region Tipos de Complemento.
        /// <summary>
        /// TipoComplementoCVol: DISTRIBUIDOR.
        /// </summary>
        public const String TIPO_COMPLEMENTO_DISTRIBUIDOR = "DISTRIBUIDOR";
        /// <summary>
        /// TipoComplementoCVol: TRANSPORTISTA.
        /// </summary>
        public const String TIPO_COMPLEMENTO_TRANSPORTE = "TRANSPORTISTA";
        /// <summary>
        /// TipoComplementoCVol: COMERCIALIZADOR.
        /// </summary>
        public const String TIPO_COMPLEMENTO_COMERCIALIZADOR = "COMERCIALIZADORA";
        #endregion
        #endregion

        #region Enumeradores.
        public enum eTipoReporte { Dia = 0, Mes = 1 }
        /// <summary>
        /// Tipo de Complemento a Implementar 
        /// </summary>
        public enum eTipoComplemento { Distribuidor = 0, Comercializadora = 1, Transportista = 2 }
        /// <summary>
        /// Tipo de Distribucion. (Ductos, AutoTanques)
        /// </summary>
        public enum eTipoDistribucion { Ductos = 0, Autotanques = 1 }
        public enum eProcesoArchivo { Descargar = 0, Guardar = 1 }
        #endregion

        #region Gets And Sets.
        /// <summary>
        /// Inicializa los Datos de la Estructura.
        /// </summary>
        public void InicializarDatos()
        {
            this.Caracter = new List<stCaracterDato>();
            this.Geolocalizacion = new List<stGeolocalizacionDato>();
        }

        #region RAIZ.
        #region Datos Generales.
        #region Version.
        /// <summary>
        /// REQUERIDO: 
        /// Este dato lo integra el programa informático que utilizas para generar el archivo XML o JSON, el cual se conforma por números.
        /// </summary>
        public String Version { set; get; }
        #endregion

        #region RFC Contribuyente.
        /// <summary>
        /// REQUERIDO:  
        /// Debes registrar tu clave en el Registro Federal de Contribuyentes como sujeto obligado a llevar controles volumétricos.
        /// Si eres persona física, este elemento debe contener 13 caracteres, tratándose de personas morales debe contener 12 caracteres.
        /// </summary>
        public String RfcContribuyente { set; get; }
        #endregion

        #region RFC Representante Legal.
        /// <summary>
        /// CONDICIONAL: 
        /// la presencia de este elemento es requerida si el contribuyente obligado a llevar 
        /// controles volumétricos es una persona moral, en caso contrario no deberá existir.
        /// </summary>
        public Object RfcRepresentanteLegal { set; get; }
        #endregion

        #region RFC Proveedor.
        /// <summary>
        /// REQUERIDO: 
        /// Debes registrar la clave en el RFC del proveedor del programa informático para llevar controles volumétricos autorizado por el SAT.
        /// El RFC deberá contener 12 caracteres al tratarse de una persona moral.
        /// </summary>
        public String RfcProveedor { set; get; }
        #endregion

        #region RFC Proveedores.
        /// <summary>
        /// OPCIONAL: 
        /// Se emplea en caso de que cuentes con más de un proveedor de equipos, sistemas, programas informáticos o 
        /// cualquier otro componente para llevar controles volumétricos. 
        /// 
        /// En este elemento podrás manifestar a tus proveedores personas físicas, así como los que sean personas
        /// morales, puede repetirse las veces que sea necesario para registrar a los proveedores con los que cuentas.
        /// </summary>
        public String RfcProveedores { set; get; }
        #endregion

        #region Carater.
        ///// <summary>
        ///// REQUERIDA: (ERROR de primera version)
        ///// Datos de los Caracteres.
        ///// </summary>
        //public List<stCaracterDato> Caracter { set; get; }
        #endregion

        #region Version 4.0.
        #region Caracter.
        /// <summary>
        /// REQUERIDO: 
        /// * contratista: si suscribiste un contrato para exploración y extracción de hidrocarburos con la
        /// Comisión Nacional de Hidrocarburos, y te encuentras en la etapa de extracción. 
        /// * asignatario: si eres Petróleos Mexicanos o cualquier otra empresa productiva del Estado que
        /// sea titular de una asignación emitida por la Secretaría de Energía, y te encuentras en la etapa de extracción. 
        /// * permisionario: si cuentas con un permiso emitido por la Secretaría de Energía o por la Comisión Reguladora de Energía. 
        /// * usuario: si almacenas gas natural para autoconsumo en instalaciones fijas para su recepción y no cuentas con algún permiso de la Comisión Reguladora de Energía.
        /// </summary>
        public object Caracter { set; get; }
        #endregion

        #region Modalidad Permiso.
        /// <summary>
        /// CONDICIONAL: 
        /// Al ser "PERMISIONARIO" se coloca el tipo de permiso con el que cuenta, empleando el Apéndice 1 “Catálogo modalidad de permisos emitidos por la CRE y la SENER” de la presente Guía. 
        /// Patrón: PL/XXXXX/ALM/AAAA
        /// </summary>
        public object ModalidadPermiso { set; get; }
        #endregion

        #region Número de Permiso.
        /// <summary>
        /// CONDICIONAL: 
        /// Al ser "PERMISIONARIO" se coloca el Número de permiso con el que cuentas y registrarlo tal como aparece en el permiso que te otorgó la Secretaría de Energía o la Comisión Reguladora de Energía.
        /// </summary>
        public object NumPermiso { set; get; }
        #endregion

        #region Número de Contrato o Asignación.
        /// <summary>
        /// CONDICIONAL: 
        /// Al ser "CONTRATISTA" o "ASIGNATARIO", Número de contrato o asignación que te otorgó la Comisión Nacional de Hidrocarburos o la Secretaría de Energía, según corresponda. 
        /// Si cuentas con más de un contrato o asignación, deberás generar un reporte por cada contrato o asignación y sus hidrocarburos relacionados. 
        /// Ejemplo CONTRATISTA: CNH-R10-L03-A23/2050, 
        /// Ejemplo ASIGNATARIO: AE-0001-CANA
        /// </summary>
        public object NumContratoOAsignacion { set; get; }
        #endregion

        #region Instalacion Almacen Gas Natural.
        /// <summary>
        /// CONDICIONAL: 
        /// Al ser "USUARIO", la presencia de este elemento es requerida si no cuentas con un permiso emitido por la 
        /// Comisión Reguladora de Energía, pero sí almacenas gas natural para usos propios en instalaciones fijas para autoconsumo, 
        /// en caso contrario no deberá existir. 
        /// Ejemplo: InstalacionAlmacenGasNatural = terminal de almacenamiento de gas natural TRES CRUCES con capacidad de 20000 m3.
        /// </summary>
        public object InstalacionAlmacenGasNatural { set; get; }
        #endregion
        #endregion

        #region Clave Instalación.
        /// <summary>
        /// REQUERIDO: 
        /// Debes asignar una clave a la instalación o proceso donde realizas tu actividad e instalaste sistemas de medición para llevar controles volumétricos.
        /// </summary>
        public object ClaveInstalacion { set; get; }
        #endregion

        #region Descripción de Instalación.
        /// <summary>
        /// REQUERIDO: 
        /// Debes describir de la mejor manera posible la instalación o proceso donde realizas tu actividad e instalaste 
        /// sistemas de medición para llevar controles volumétricos de acuerdo a tu contrato, asignación o permiso.
        /// </summary>
        public object DescripcionInstalacion { set; get; }
        #endregion

        #region Geolocalización.
        /// <summary>
        /// OPCIONAL: 
        /// Ubicación geográfica, constituida por latitud y longitud, de la instalación con la que cuentas o utilizas.
        /// </summary>
        public List<stGeolocalizacionDato> Geolocalizacion { set; get; }
        #endregion

        #region Número de Pozos.
        /// <summary>
        /// REQUERIDO: 
        /// Si eres "CONTRATISTA" o "ASIGNATARIO" debes registrar el número de pozos con los que cuentas, al amparo del
        /// contrato o asignación que registraste en el elemento NumContratoOAsignacion. 
        /// Si no eres "CONTRATISTA" o "ASIGNATARIO" o no cuentas con pozos deberás registrar el número “0”.
        /// </summary>
        public object NumeroPozos { set; get; }
        #endregion

        #region Número de Tanques.
        /// <summary>
        /// REQUERIDO: 
        /// Número de tanques con los que cuentas en tu instalación o utilizas en tus procesos para
        /// realizar tus actividades al amparo del contrato o asignación que registraste en el elemento
        /// NumContratoOAsignacion o del permiso que registraste en el elemento ModalidadPermiso, según corresponda. 
        /// Si no cuentas con tanques deberás registrar el número “0”.
        /// </summary>
        public object NumeroTanques { set; get; }
        #endregion

        #region Número de Ductos de Entrada y Salida.
        /// <summary>
        /// REQUERIDO: 
        /// Número de ductos de entrada o salida a medios de almacenamiento o de carga y descarga a medios de transporte o de distribución,
        /// con los que cuentas en tu instalación o que utilizas en tus procesos para realizar tus actividades al amparo del contrato o 
        /// asignación que registraste en el elemento "NumContratoOAsignacion" o del permiso que registraste en el elemento "ModalidadPermiso", 
        /// según corresponda. 
        /// Si no cuentas con este tipo de ductos deberás registrar el número “0”.
        /// </summary>
        public object NumeroDuctosEntradaSalida { set; get; }
        #endregion

        #region Número de Ductos de Transportes.
        /// <summary>
        /// REQUERIDO: 
        /// Número de ductos con los que realizas la actividad de transporte o distribución al amparo
        /// del permiso que registraste en el elemento "ModalidadPermiso".
        /// Si no realizas la actividad de transporte o distribución por medio de ductos deberás registrar el Número "0".
        /// </summary>
        public object NumeroDuctosTransporteDistribucion { set; get; }
        #endregion

        #region Número de Dispensarios.
        /// <summary>
        /// REQUERIDO: 
        /// Número de dispensarios con los que cuentas en tu instalación o que utilizas en tus procesos para 
        /// realizar tus actividades al amparo del permiso que registraste en el elemento "ModalidadPermiso". 
        /// Si no cuentas con dispensarios deberás registrar el número "0".
        /// </summary>
        public object NumeroDispensarios { set; get; }
        #endregion

        #region Fecha y Hora de Corte.
        /// <summary>
        /// REQUERIDO: 
        /// (DIARIO) Fecha y hora a la que estás generando el reporte, deberás expresar la hora en UTC con la
        /// indicación del huso horario, empleando el formato yyyy-mm-ddThh:mm:ss±hh:mm, de acuerdo con la
        /// especificación ISO 8601. 
        /// Ejemplo: FechaYHoraCorte=2019-10-30T23:59:45-01:00.
        /// </summary>
        public Object FechaYHoraCorte { set; get; }
        #endregion

        #region Fecha y Hora Reporte Mes.
        /// <summary>
        /// REQUERIDO: 
        /// (MENSUAL) Fecha y hora a la que estás generando el reporte, deberás expresar la hora en UTC con la
        /// indicación del huso horario, empleando el formato yyyy-mm-ddThh:mm:ss±hh:mm, de acuerdo con la
        /// especificación ISO 8601. 
        /// Ejemplo: FechaYHoraCorte=2019-10-30T23:59:45-01:00.
        /// </summary>
        public Object FechaYHoraReporteMes { set; get; }
        #endregion
        #endregion

        #region Productos.
        /// <summary>
        /// REQUERIDO: 
        /// Este elemento debe replicarse las veces necesarias para manifestar productos distintos o el mismo tipo de
        /// producto, pero con características distintas, sólo recuerda que deben ser productos para un mismo permiso,
        /// área contractual o de asignación.
        /// </summary>
        public List<stProductoDato> Producto { set; get; }
        #endregion

        #region Bitacora.
        /// <summary>
        /// (DIARIO) 
        /// </summary>
        public List<stBitacoraDato> Bitacora { set; get; }
        #endregion
        #endregion

        // <===  STRUCTURAS  ===>
        #region Bitacora Mensual.
        /// <summary>
        /// (MENSUAL) 
        /// </summary>
        public List<stBitacoraDato> BitacoraMensual { set; get; }
        #endregion

        #region Caracter Datos.
        /// <summary>
        /// Datos del Caracter.
        /// </summary>
        public struct stCaracterDato
        {
            #region Tipo (XML).
            /// <summary>
            /// REQUERIDO (XML):  
            /// * contratista: si suscribiste un contrato para exploración y extracción de hidrocarburos con la
            /// Comisión Nacional de Hidrocarburos, y te encuentras en la etapa de extracción. 
            /// * asignatario: si eres Petróleos Mexicanos o cualquier otra empresa productiva del Estado que
            /// sea titular de una asignación emitida por la Secretaría de Energía, y te encuentras en la etapa de extracción. 
            /// * permisionario: si cuentas con un permiso emitido por la Secretaría de Energía o por la Comisión Reguladora de Energía. 
            /// * usuario: si almacenas gas natural para autoconsumo en instalaciones fijas para su recepción y no cuentas con algún permiso de la Comisión Reguladora de Energía.
            /// </summary>
            public object TipoCaracter { set; get; }
            #endregion

            #region Tipo (JSON).
            /// <summary>
            /// REQUERIDO (JSON): 
            /// * contratista: si suscribiste un contrato para exploración y extracción de hidrocarburos con la
            /// Comisión Nacional de Hidrocarburos, y te encuentras en la etapa de extracción. 
            /// * asignatario: si eres Petróleos Mexicanos o cualquier otra empresa productiva del Estado que
            /// sea titular de una asignación emitida por la Secretaría de Energía, y te encuentras en la etapa de extracción. 
            /// * permisionario: si cuentas con un permiso emitido por la Secretaría de Energía o por la Comisión Reguladora de Energía. 
            /// * usuario: si almacenas gas natural para autoconsumo en instalaciones fijas para su recepción y no cuentas con algún permiso de la Comisión Reguladora de Energía.
            /// </summary>
            public object Caracter { set; get; }
            #endregion

            #region Modalidad Permiso.
            /// <summary>
            /// CONDICIONAL: 
            /// Al ser "PERMISIONARIO" se coloca el tipo de permiso con el que cuenta, empleando el Apéndice 1 “Catálogo modalidad de permisos emitidos por la CRE y la SENER” de la presente Guía. 
            /// Patrón: PL/XXXXX/ALM/AAAA
            /// </summary>
            public object ModalidadPermiso { set; get; }
            #endregion

            #region Número de Permiso.
            /// <summary>
            /// CONDICIONAL: 
            /// Al ser "PERMISIONARIO" se coloca el Número de permiso con el que cuentas y registrarlo tal como aparece en el permiso que te otorgó la Secretaría de Energía o la Comisión Reguladora de Energía.
            /// </summary>
            public object NumPermiso { set; get; }
            #endregion

            #region Número de Contrato o Asignación.
            /// <summary>
            /// CONDICIONAL: 
            /// Al ser "CONTRATISTA" o "ASIGNATARIO", Número de contrato o asignación que te otorgó la Comisión Nacional de Hidrocarburos o la Secretaría de Energía, según corresponda. 
            /// Si cuentas con más de un contrato o asignación, deberás generar un reporte por cada contrato o asignación y sus hidrocarburos relacionados. 
            /// Ejemplo CONTRATISTA: CNH-R10-L03-A23/2050, 
            /// Ejemplo ASIGNATARIO: AE-0001-CANA
            /// </summary>
            public object NumContratoOAsignacion { set; get; }
            #endregion

            #region Instalacion Almacen Gas Natural.
            /// <summary>
            /// CONDICIONAL: 
            /// Al ser "USUARIO", la presencia de este elemento es requerida si no cuentas con un permiso emitido por la 
            /// Comisión Reguladora de Energía, pero sí almacenas gas natural para usos propios en instalaciones fijas para autoconsumo, 
            /// en caso contrario no deberá existir. 
            /// Ejemplo: InstalacionAlmacenGasNatural = terminal de almacenamiento de gas natural TRES CRUCES con capacidad de 20000 m3.
            /// </summary>
            public object InstalacionAlmacenGasNatural { set; get; }
            #endregion

            /// <summary>
            /// Inicializa los Campos.
            /// </summary>
            public void Inicializar()
            {
            }
        }
        #endregion

        #region Geolocazacion Datos.
        public struct stGeolocalizacionDato
        {
            #region Latitud.
            /// <summary>
            /// REQUERIDO: 
            /// Debes registrar la latitud de la instalación con la que cuentas, expresada en Grados decimales (DD), puede registrar valores de -90 a 90.
            /// Ejemplo: GeolocalizacionLatitud = 21.8041458.
            /// </summary>
            public Decimal GeolocalizacionLatitud { set; get; }
            #endregion

            #region Longitud.
            /// <summary>
            /// REQUERIDO: 
            /// Debes registrar la longitud de la instalación con la que cuentas, expresada en Grados decimales (DD), puedes registrar valores de -180 a 180.
            /// Ejemplo: GeolocalizacionLongitud = -104.8409271.
            /// </summary>
            public Decimal GeolocalizacionLongitud { set; get; }
            #endregion

            /// <summary>
            /// Inicializa los Campos.
            /// </summary>
            public void Inicializar()
            {
                this.GeolocalizacionLatitud = 0;
                this.GeolocalizacionLongitud = 0;
            }
        }
        #endregion

        #region Producto Datos.
        public struct stProductoDato
        {
            #region Clave Producto.
            /// <summary>
            /// REQUERIDO: 
            /// Clave del producto objeto de tus operaciones. Apéndice 2 “Catálogo de hidrocarburos y petrolíferos” de la presente Guía.
            /// </summary>
            public object ClaveProducto { set; get; }
            #endregion

            #region Clave SubProducto.
            /// <summary>
            /// CONDICIONAL: 
            /// La presencia de este elemento es requerida si manifestaste como producto al diésel, 
            /// a la gasolina, al petróleo, al gas natural, a la turbosina, al combustóleo, algún
            /// bioenergético, al gasóleo, a las naftas, al gasavión o a los hidratos de metano, 
            /// en caso contrario no deberá existir.
            /// </summary>
            public object ClaveSubProducto { set; get; }
            #endregion

            #region Reporte de Volumen Mensual.
            /// <summary>
            /// Reporte Volumen Mensual. 
            /// Estructura: stReporteVolumenMensualDato.
            /// </summary>
            public Object ReporteDeVolumenMensual { set; get; }
            #endregion

            #region Gasolina.
            ///// <summary>
            ///// CONDICIONAL: 
            ///// Si elemento "PRODUCTO" hayas manifestado el producto "PR07". 
            ///// Estructura: stGasolinaDatos.
            ///// </summary>
            //public Object Gasolina { set; get; }
            public Object ComposOctanajeGasolina { set; get; }
            public Object GasolinaConCombustibleNoFosil { set; get; }
            public Object ComposDeCombustibleNoFosilEnGasolina { set; get; }
            #endregion

            #region Diesel.
            ///// <summary>
            ///// CONDICIONAL: 
            ///// Si el elemento "PRODUCTO" hayas manifestado el producto "PR03".
            ///// Estructura: stDieselDatos.
            ///// </summary>
            //public Object Diesel { set; get; }

            public Object DieselConCombustibleNoFosil { set; get; }
            public Object ComposDeCombustibleNoFosilEnDiesel { set; get; }
            #endregion

            #region Turbosina.
            ///// <summary>
            ///// CONDICIONAL: 
            ///// Si el elemento "PRODUCTO" hayas manifestado el producto "PR11". 
            ///// Estructura: stTurbosinaDatos.
            ///// </summary>
            //public Object Turbosina { set; get; }

            public Object TurbosinaConCombustibleNoFosil { set; get; }
            public Object ComposDeCombustibleNoFosilEnTurbosina { set; get; }
            #endregion

            #region GasLP.
            public Object ComposDePropanoEnGasLP { set; get; }
            public Object ComposDeButanoEnGasLP { set; get; }
            #endregion

            #region Petroleo.
            ///// <summary>
            ///// CONDICIONAL: 
            ///// Si el elemento "TIPOCARACTER" o "CARACTER" hayas manifestado ser "contratista" o "asignatario" y 
            ///// el elemento "PRODUCTO" hayas manifestado el producto "PR08". 
            ///// Estructura: stPetroleoDatos.
            ///// </summary>
            //public Object Petroleo { set; get; }

            public Object DensidadDePetroleo { set; get; }
            public Object ComposDeAzufreEnPetroleo { set; get; }
            #endregion

            #region GasNaturalOCondensados.
            /// <summary>
            /// (NO SE USA v4.0) CONDICIONAL: 
            /// Si el elemento "TIPOCARACTER" o "CARACTER" hayas manifestado ser "contratista" o "asignatario" y 
            /// el elemento "PRODUCTO" hayas manifestado el producto "PR09" o "PR10". 
            /// Estructura: stGasNaturalOCondensadosDatos.
            /// </summary>
            public Object GasNaturalOCondensados { set; get; }
            #endregion

            #region Otros.
            /// <summary>
            /// CONDICIONAL: 
            /// Si el elemento Producto hayas manifestado el "PRODUCTO" "PR15" y a su vez el "SUBPRODUCTO" "SP20". 
            /// Describe el tipo de bioenergético de que se trate y que no forma parte del catálogo de productos y subproductos contenido en el Apéndice 2. 
            /// Ejemplo: Otros = biometanol.
            /// </summary>
            public Object Otros { set; get; }
            #endregion

            #region Marca Comercial.
            /// <summary>
            /// OPCIONAL: 
            /// Producto o Subproducto con alguna marca comercial. 
            /// Ejemplo: MarcaComercial=Blue-ultrapower 5000.
            /// </summary>
            public Object MarcaComercial { set; get; }
            #endregion

            #region Marcaje.
            /// <summary>
            /// OPCIONAL: 
            /// Sustancia química empleada como marcador.
            /// Ejemplo: Marcaje=Nitrógeno.
            /// </summary>
            public Object Marcaje { set; get; }
            #endregion

            #region Concentracion de Sustancias Marcaje.
            /// <summary>
            /// CONDICIONAL: 
            /// Si se capturo el elemento Marcaje. 
            /// Indica la concentración del marcador en el producto, la cual se mide en ppm. 
            /// Ejemplo: ConcentracionSustanciaMarcaje=100.
            /// </summary>
            public Object ConcentracionSustanciaMarcaje { set; get; }
            #endregion

            #region Tanque.
            /// <summary>
            /// CONDICIONAL: 
            /// SI el elemento "NumeroTanques" hayas manifestado al menos 1 tanque. 
            /// TipoDato: List<stTanqueDatos>.
            /// </summary>
            public Object Tanque { set; get; }
            #endregion

            #region Ducto.
            /// <summary>
            /// CONDICIONAL: 
            /// SI en los elementos NumeroDuctosEntradaSalida o NumeroDuctosTransporteDistribucion hayas manifestado al menos 1 ducto. 
            /// Estructura: List<stDuctoDatos>.
            /// </summary>
            public Object Ducto { set; get; }
            #endregion

            #region Pozo.
            /// <summary>
            /// CONDICIONAL: 
            /// SI el elemento "NumeroPozos" hayas manifestado al menos 1 pozo. 
            /// Manifiesta las características de los pozos, también se manifiestan los registros de volumen 
            /// de los hidrocarburos producidos, así como los que se reinyectaron al final del día. 
            /// Estructura: List<stPozoDato>.
            /// </summary>
            public Object Pozo { set; get; }
            #endregion

            #region Dispensario.
            /// <summary>
            /// CONDICIONAL: 
            /// SI el elemento "NumeroDispensarios" hayas manifestado al menos 1 dispensario. 
            /// Se registran las características de los dispensarios, también se manifiestan los 
            /// registros de volumen de los hidrocarburos o petrolíferos entregados. 
            /// Estructura: List<stDispensarioDato>.
            /// </summary>
            public Object Dispensario { set; get; }
            #endregion
        }

        #region Reporte Volumen Mensual Datos.
        public struct stReporteVolumenMensualDato
        {
            /// <summary>
            /// Estructura: stControlExistenciaDato.
            /// </summary>
            public Object ControlDeExistencias { set; get; } //CONTROLDEEXISTENCIAS { set; get; }
            /// <summary>
            /// Estructura: stRecepcionMesDato.
            /// </summary>
            public Object Recepciones { set; get; } // RECEPCIONES { set; get; }
            /// <summary>
            /// Estructura: stEntregasMesDato.
            /// </summary>
            public Object Entregas { set; get; } // ENTREGAS { set; get; }
        }

        /// <summary>
        /// Datos del Control de Existencias.
        /// </summary>
        public struct stControlExistenciaDato
        {
            /// <summary>
            /// REQUERIDO, Estructura: stCapacidadDato.
            /// </summary>
            public Object VolumenExistenciasMes { set; get; }

            /// <summary>
            /// REQUERIDO. 
            /// Formato: yyyy-mm-ddThh:mm:ss±hh:mm. 
            /// Ejemplo: 2020-10-31T23:59:45-01:00.
            /// </summary>
            public Object FechaYHoraEstaMedicionMes { set; get; }
        }
        /// <summary>
        /// Datos de las Recepciones del Mes.
        /// </summary>
        public struct stRecepcionMesDato
        {
            public Object TotalRecepcionesMes { set; get; }
            public Object SumaVolumenRecepcionMes { set; get; }
            public Object TotalDocumentosMes { set; get; }
            public Object PoderCalorifico { set; get; }
            public Object ImporteTotalRecepcionesMensual { set; get; }
            public Object Complemento { set; get; }
        }
        public struct stEntregasMesDato
        {
            public Object TotalEntregasMes { set; get; }
            public Object SumaVolumenEntregadoMes { set; get; }
            public Object PoderCalorifico { set; get; }
            public Object TotalDocumentosMes { set; get; }
            public Object ImporteTotalEntregasMes { set; get; }
            public Object Complemento { set; get; }
        }
        #endregion

        #region Datos Del Producto.
        #region Gasolina Datos
        /// <summary>
        /// Datos de la Gasolina.
        /// </summary>
        public struct stGasolinaDatos
        {
            #region Octanaje.
            /// <summary>
            /// CONDICIONAL: 
            /// Si elemento "PRODUCTO" hayas manifestado el producto "PR07". 
            /// Octanaje de la gasolina.
            /// </summary>
            public object ComposOctanajeGasolina { set; get; }
            #endregion

            #region Gasolina con Combustible Fosil.
            /// <summary>
            /// CONDICIONAL: 
            /// Si elemento "PRODUCTO" hayas manifestado el producto "PR07". 
            /// Indica si la gasolina contiene o no combustible no fósil.
            /// </summary>
            public object GasolinaConCombustibleNoFosil { set; get; }
            #endregion

            #region Porcentaje De Combustible No Fosil En Gasolina.
            /// <summary>
            /// CONDICIONAL: 
            /// Si "GasolinaConCombustibleNoFosil" hayas manifestado "SI". 
            /// Porcentaje de combustible no fósil contenido en la gasolina.
            /// </summary>
            public object ComposDeCombustibleNoFosilEnGasolina { set; get; }
            #endregion
        }
        #endregion

        #region Diesel Datos.
        /// <summary>
        /// Datos del Diesel.
        /// </summary>
        public struct stDieselDatos
        {
            #region Diesel Con Fosil.
            /// <summary>
            /// CONDICIONAL: 
            /// Si elemento "Producto" hayas manifestado el producto "PR03". 
            /// Indica si la diésel contiene o no combustible no fósil.
            /// </summary>
            public object DieselConCombustibleNoFosil { set; get; }
            #endregion

            #region Porcentaje Fosil En Diesel.
            /// <summary>
            /// CONDICIONAL: 
            /// Si el elemento "DieselConCombustibleNoFosil" hayas manifestado "SI". 
            /// Porcentaje de combustible no fósil contenido en el diésel.
            /// </summary>
            public object ComposDeCombustibleNoFosilEnDiesel { set; get; }
            #endregion
        }
        #endregion

        #region Turbosina Datos.
        /// <summary>
        /// Datos de la Turbosina.
        /// </summary>
        public struct stTurbosinaDatos
        {
            #region Turbosina Con Fosil.
            /// <summary>
            /// CONDICIONAL: 
            /// Si el elemento "PRODUCTO" hayas manifestado el producto "PR11". 
            /// Indica si la turbosina contiene combustible no fósil.
            /// </summary>
            public Object TurbosinaConCombustibleNoFosil { set; get; }
            #endregion

            #region Porcentaje de Fosil.
            /// <summary>
            /// CONDICIONAL: 
            /// Si el elemento "TurbosinaConCombustibleNoFosil" hayas manifestado "SI", 
            /// indica el porcentaje de combustible no fósil contenido en la turbosina.
            /// </summary>
            public Object ComposDeCombustibleNoFosilEnTurbosina { set; get; }
            #endregion
        }
        #endregion

        #region GasLP Datos.
        /// <summary>
        /// Datos del GasLP.
        /// </summary>
        public struct stGasLpDatos
        {
            #region Porcentaje de Gas Propano.
            /// <summary>
            /// REQUERIDO: 
            /// Indica el porcentaje de propano en el gas licuado de petróleo. 
            /// Ejemplo: ComposDePropanoEnGasLP=60.00.
            /// </summary>
            public Object ComposDePropanoEnGasLP { set; get; }
            #endregion

            #region Porcentaje de Gas Butano.
            /// <summary>
            /// REQUERIDO: 
            /// Indica el porcentaje de butano en el gas licuado de petróleo. 
            /// Ejemplo: ComposDeButanoEnGasLP=40.00.
            /// </summary>
            public Object ComposDeButanoEnGasLP { set; get; }
            #endregion
        }
        #endregion

        #region Petroleo Datos.
        /// <summary>
        /// Datos del Petroleo.
        /// </summary>
        public struct stPetroleoDatos
        {
            #region Densidad.
            /// <summary>
            /// REQUERIDO: 
            /// Indica la densidad del petróleo en grados API. 
            /// Ejemplo: DensidadDePetroleo=52.2.
            /// </summary>
            public Object DensidadDePetroleo { set; get; }
            #endregion

            #region Porcentaje de Azufre.
            /// <summary>
            /// REQUERIDO: 
            /// Indica el porcentaje de azufre que contiene el petróleo. 
            /// Ejemplo: ComposDeAzufreEnPetroleo=3.9.
            /// </summary>
            public Object ComposDeAzufreEnPetroleo { set; get; }
            #endregion
        }
        #endregion

        #region Gas Natural o Condensado Datos.
        /// <summary>
        /// Datos del Gas Natural o Condensado.
        /// </summary>
        public struct stGasNaturalOCondensadosDatos
        {
            #region Componente de Gas Natural.
            /// <summary>
            /// REQUERIDO: 
            /// Indica cada componente del gas natural o condensado conforme a lo señalado en la
            /// sección “Catálogo complementos gas natural o condensado” del Apéndice 2 “Catálogo de
            /// hidrocarburos y petrolíferos”. 
            /// Ejemplo: ComposGasNaturalOCondensados = GNC03.
            /// </summary>
            public String ComposGasNaturalOCondensados { set; get; }
            #endregion

            #region Fraccion Molar.
            /// <summary>
            /// REQUERIDO: 
            /// Indica la fracción molar de cada componente que registraste en el elemento "ComposGasNaturalOCondensados". 
            /// Ejemplo: FraccionMolar=0.25.
            /// </summary>
            public Object FraccionMolar { set; get; }
            #endregion

            #region Poder Calorifico.
            /// <summary>
            /// REQUERIDO: 
            /// Indica el poder calorífico de cada componente que registraste en el elemento "ComposGasNaturalOCondensados". 
            /// Ejemplo: PoderCalorifico = 1000.
            /// </summary>
            public Object PoderCalorifico { set; get; }
            #endregion

            /// <summary>
            /// Inicializa los Valores.
            /// </summary>
            public void Inicializar()
            {
                this.ComposGasNaturalOCondensados = String.Empty;
                this.FraccionMolar = 0;
                this.PoderCalorifico = 0.001;
            }
        }
        #endregion
        #endregion

        #region Tanque Datos.
        /// <summary>
        /// Datos del Tanque.
        /// </summary>
        public struct stTanqueDatos
        {
            #region Clave Identificacion Tanque.
            /// <summary>
            /// REQUERIDA: 
            /// Clave del tanque, para generarla deberás emplear el Apéndice 3.
            /// Ejemplo: ClaveIdentificacionTanque=TQS-TDA-0001.
            /// </summary>
            public Object ClaveIdentificacionTanque { set; get; }
            #endregion

            #region Localizacion o Descripción.
            /// <summary>
            /// REQUERIDO: 
            /// Ubicación de cada tanque, es decir, la localización, o en su caso, la descripción de cada tanque. 
            /// Ejemplo: LocalizacionY/ODescripcionTanque=Tanque de almacenamiento ubicado en la terminal 2 de reparto Gas LP XXXXX.
            /// </summary>
            public Object LocalizacionYODescripcionTanque { set; get; }
            //public Object Localizaciony/oDescripcionTanque { set; get; }
            #endregion

            #region Vigencia Calibración.
            /// <summary>
            /// REQUERIDO: 
            /// Validez de la calibración o cubicación de cada tanque, es contar con una calibración válida, 
            /// para lo cual deberás manifestar la fecha del certificado o del documento donde conste la calibración 
            /// en formato "yyyy-mm-dd", de acuerdo con la especificación ISO 8601. 
            /// Ejemplo: VigenciaCalibracionTanque=2020-06-29.
            /// </summary>
            public String VigenciaCalibracionTanque { set; get; }
            #endregion

            #region Capacidad Total.
            /// <summary>
            /// REQUERIDO: 
            /// Capacidad máxima de diseño del tanque. 
            /// Ejemplo: .
            /// </summary>
            public stCapacidadDato CapacidadTotalTanque { set; get; }
            #endregion

            #region Capacidad Operativa.
            /// <summary>
            /// REQUERIDO: 
            /// Capacidad que es susceptible de ser extraída de cada tanque, ya que por cuestiones de diseño y 
            /// características de cada tanque no es posible extraer todo el volumen de fluido que puede contener.
            /// </summary>
            public stCapacidadDato CapacidadOperativaTanque { set; get; }
            #endregion

            #region Capacidad Util.
            /// <summary>
            /// REQUERIDO: 
            /// Capacidad útil del tanque, misma que resulta de restar de la capacidad total del tanque menos el volumen mínimo de operación.
            /// </summary>
            public stCapacidadDato CapacidadUtilTanque { set; get; }
            #endregion

            #region Capacidad Fondaje.
            /// <summary>
            /// CONDICIONAL: 
            /// SI el Producto que hayas manifestado se reporte en fase líquida. 
            /// Debes indicar la capacidad mínima de líquido de fondo que el tanque debe tener para que las bombas puedan extraer los fluidos del tanque.
            /// </summary>
            public stCapacidadDato CapacidadFondajeTanque { set; get; }
            #endregion

            #region Capacidad Gas Talon.
            /// <summary>
            /// CONDICIONAL: 
            /// SI el Producto "PR09". 
            /// Nivel mínimo operativo de llenado de los tanques que puedan almacenar gases.
            /// Estructura: stCapacidadDato.
            /// </summary>
            public Object CapacidadGasTalon { set; get; }
            #endregion

            #region Volumen Minimo Operación.
            /// <summary>
            /// REQUERIDO: 
            /// Indica el volumen por debajo del cual no puede utilizarse la instalación al no estar 
            /// garantizada la fiabilidad y la seguridad operativa de los equipos y del propio tanque.
            /// </summary>
            public stCapacidadDato VolumenMinimoOperacion { set; get; }
            #endregion

            #region Estado del Tanque.
            /// <summary>
            /// REQUERIDO: 
            /// Indica si el tanque está en operación o fuera de operación. 
            /// Los únicos valores permitidos son "O" que significa que el tanque se encuentra en 
            /// operación, o en su caso "F" que significa que el tanque se encuentra fuera de operación.
            /// </summary>
            public String EstadoTanque { set; get; }
            #endregion

            #region Medicion Tanque. (XML)
            /// <summary>
            /// REQUERIDO (XML): 
            /// Describe los medidores instalados en el tanque y replicar este elemento por cada medidor instalado. 
            /// Estructura: List<stMedicionTanqueDato>.
            /// </summary>
            public Object MedicionTanque { set; get; }
            #endregion

            #region Medidores. (JSON)
            /// <summary>
            /// REQUERIDO (JSON): 
            /// Describe los medidores instalados en el tanque y replicar este elemento por cada medidor instalado. 
            /// Estructura: List<stMedicionTanqueDato>.
            /// </summary>
            public Object Medidores { set; get; }
            #endregion

            #region Existencia.
            /// <summary>
            /// REQUERIDO: 
            /// Volúmenes necesarios para calcular las existencias por el programa informático para llevar controles volumétricos del tanque.
            /// </summary>
            public stExistenciaDato Existencias { set; get; }// .EXISTENCIAS { set; get; }
            #endregion

            #region Recepciones.
            /// <summary>
            /// REQUERIDO: 
            /// Totalidad de las recepciones del día.
            /// </summary>
            public stRecepcionDato Recepciones { set; get; }// .RECEPCIONES { set; get; }
            #endregion

            #region Entregas.
            /// <summary>
            /// Registra la totalidad de las entregas del día.
            /// </summary>
            public Object Entregas { set; get; } // .ENTREGAS { set; get; }
            #endregion
        }

        #region Capacidad Datos.
        /// <summary>
        /// Datos de la Capacidad. (ValorNumerico, UM (XML), UnidadDeMedida (JSON))
        /// </summary>
        public struct stCapacidadDato
        {
            #region Valor.
            /// <summary>
            /// REQUERIDO: 
            /// Capacidad en número. 
            /// Ejemplo: ValorNumerico=1000000.
            /// </summary>
            public Object ValorNumerico { set; get; }
            #endregion

            #region UM PARA XML.
            /// <summary>
            /// REQUERIDO (XML): 
            /// Clave que corresponda a la unidad de medida. 
            /// "UM01" para barriles, "UM02" para pies cúbicos, "UM03" para litros y "UM04" para metros cúbicos. 
            /// Ejemplo: UM=UM03.
            /// </summary>
            public Object UM { set; get; }
            #endregion

            #region UnidadDeMedida JSON.
            /// <summary>
            /// REQUERIDO (JSON): 
            /// Clave que corresponda a la unidad de medida. 
            /// "UM01" para barriles, "UM02" para pies cúbicos, "UM03" para litros y "UM04" para metros cúbicos. 
            /// Ejemplo: UnidadDeMedida=UM03.
            /// </summary>
            public Object UnidadDeMedida { set; get; }
            #endregion
        }
        #endregion
        #endregion

        #region Ducto Datos.
        /// <summary>
        /// Datos del Ducto.
        /// </summary>
        public struct stDuctoDatos
        {
            #region Clave Identificacion de Ducto.
            /// <summary>
            /// REQUERIDO: 
            /// Clave de identificación al ducto de que se trate ya sea de transporte o distribución, de entrada o salida a medios de almacenamiento, de carga o descarga a medios de transporte o distribución. 
            /// Ejemplo: ClaveIdentificacionDucto=DUC-DES-004.
            /// </summary>
            public String ClaveIdentificacionDucto { set; get; }
            #endregion

            #region Decripcion del Ducto.
            /// <summary>
            /// REQUERIDO: 
            /// Información sobre la ubicación de cada ducto, es decir, la localización, nombre, tramo o en su caso, la descripción de cada ducto. 
            /// Ejemplo: DescripcionDucto = ducto de descarga del autotanque de clave TQS-ATQ-1234 de distribución de petrolíferos.
            /// </summary>
            public String DescripcionDucto { set; get; }
            #endregion

            #region Diametro del Ducto.
            /// <summary>
            /// REQUERIDO: 
            /// Registro del diámetro nominal en pulgadas del ducto. 
            /// Ejemplo: .
            /// </summary>
            public Decimal DiametroDucto { set; get; }
            #endregion

            #region MedicionDucto (XML).
            /// <summary>
            /// REQUERIDO: 
            /// Describe los medidores instalados en el ducto y replicar este elemento por cada medidor instalado. (XML)
            /// Estructura: List<stDuctoMedicionDato>.
            /// </summary>
            public Object MedicionDucto { set; get; }
            #endregion

            #region Medidores. (JSON).
            /// <summary>
            /// REQUERIDO: 
            /// Describe los medidores instalados en el ducto y replicar este elemento por cada medidor instalado. (JSON)
            /// </summary>
            public List<stDuctoMedicionDato> Medidores { set; get; }
            #endregion

            #region CapacidadGasTalon.
            /// <summary>
            /// CONDICIONAL: 
            /// SI el elemento "PRODUCTO" hayas manifestado el producto "PR09". 
            /// Indica el nivel mínimo operativo de las redes de transporte en el sistema gasista. Esta cantidad corresponde al nivel mínimo de llenado de los gasoductos de transporte.
            /// </summary>
            public Object CapacidadGasTalon { set; get; }
            #endregion

            #region Recepciones.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la totalidad de las recepciones del día.
            /// </summary>
            public stRecepcionDato Recepciones { set; get; }
            #endregion

            #region Entregas.
            /// <summary>
            /// REQUERIDA: 
            /// Registro de la totalidad de las entregas del día.
            /// </summary>
            public stEntregaDato ENTREGAS { set; get; }
            #endregion
        }

        #region Ducto Medicion Datos.
        /// <summary>
        /// Datos de Medición del Ducto.
        /// </summary>
        public struct stDuctoMedicionDato
        {
            #region Sistema de Medición del Ducto.
            /// <summary>
            /// REQUERIDO: 
            /// Clave de identificación del sistema de medición instalado en cada ducto, parar generarla deberás emplear el Apéndice 3. 
            /// Ejemplo: SistemaMedicionDucto=SMD- DUC-DES-004.
            /// </summary>
            public string SistemaMedicionDucto { set; get; }
            #endregion

            #region Localizacion o Descripcion del Sistema de Medicion del Ducto.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la información sobre la localización o descripción del sistema de medición que registraste en "SistemaMedicionDucto".
            /// Ejemplo: LocalizODescripSistMedicionDucto=Medidor dinámico Marca DuctMex.
            /// </summary>
            public String LocalizODescripSistMedicionDucto { set; get; }
            #endregion

            #region Vigencia Calibracion.
            /// <summary>
            /// REQUERIDO: 
            /// Validez de la calibración del sistema de medición, formato yyyy-mm-dd. 
            /// Ejemplo: VigenciaCalibracionSistMedicionDucto=2018-06-30.
            /// </summary>
            public String VigenciaCalibracionSistMedicionDucto { set; get; }
            #endregion

            #region Incertidumbre de Medición.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la incertidumbre de medición del medidor registrado en "SistemaMedicionDucto". 
            /// Ejemplo: IncertidumbreMedicionSistMedicionDucto=0.009.
            /// </summary>
            public Decimal IncertidumbreMedicionSistMedicionDucto { set; get; }
            #endregion
        }
        #endregion
        #endregion

        #region Pozo Datos.
        /// <summary>
        /// Datos del Pozo.
        /// </summary>
        public struct stPozoDato
        {
            #region Clave Pozo.
            /// <summary>
            /// REQUERIDO: 
            /// Clave al pozo, para generarla deberás emplear el Apéndice 3.
            /// Ejemplo: ClavePozo=POZ-NOBLES0001DEL.
            /// </summary>
            public String ClavePozo { set; get; }
            #endregion

            #region Descripción del Pozo.
            /// <summary>
            /// REQUERIDO: 
            /// Registro de la información sobre la ubicación de cada pozo, es decir la localización, o en su caso, la descripción de cada pozo. 
            /// Ejemplo: DescripcionPozo=Pozo delimitador ubicado en el área contractual México 45, profundidad 4534 m.
            /// </summary>
            public String DescripcionPozo { set; get; }
            #endregion

            #region Medicion Pozo (XML).
            /// <summary>
            /// REQUERIDO: 
            /// Describe los medidores instalados en el pozo y replicar este elemento por cada medidor instalado. (XML)
            /// </summary>
            public stPozoMedicionDato MedicionPozo { set; get; }
            #endregion

            #region Medicion (JSON).
            /// <summary>
            /// REQUERIDO: 
            /// Describe los medidores instalados en el pozo y replicar este elemento por cada medidor instalado. (JSON)
            /// </summary>
            public stPozoMedicionDato Medidores { set; get; }
            #endregion

            #region Recepciones.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la totalidad de las recepciones del día.
            /// </summary>
            public stRecepcionDato Recepciones { set; get; }
            #endregion

            #region Entregas.
            /// <summary>
            /// REQUERIDA: 
            /// Registro de la totalidad de las entregas del día.
            /// </summary>
            public stEntregaDato ENTREGAS { set; get; }
            #endregion
        }

        #region Pozo Medicion Datos.
        /// <summary>
        /// Datos de Medición del Pozo.
        /// </summary>
        public struct stPozoMedicionDato
        {
            #region Sistema de Medición del Pozo.
            /// <summary>
            /// REQUERIDO: 
            /// Clave de identificación al sistema de medición instalado en el pozo, parar generarla deberás emplear el Apéndice 3. 
            /// Ejemplo: SistemaMedicionPozo=SMD-POZ-NOBLES0001DEL.
            /// </summary>
            public string SistemaMedicionPozo { set; get; }
            #endregion

            #region Localizacion o Descripcion del Sistema de Medicion del Ducto.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la información sobre la localización o descripción del sistema de medición que registraste en "SistemaMedicionPozo".
            /// Ejemplo: LocalizODescripSistMedicionPozo=medidor multifásico instalado en el cabezal del pozo POZ-NOBLES0001DEL.
            /// </summary>
            public String LocalizODescripSistMedicionPozo { set; get; }
            #endregion

            #region Vigencia Calibracion.
            /// <summary>
            /// REQUERIDO: 
            /// Validez de la calibración del sistema de medición, formato yyyy-mm-dd. 
            /// Ejemplo: VigenciaCalibracionSistMedicionPozo=2020-07-07.
            /// </summary>
            public String VigenciaCalibracionSistMedicionPozo { set; get; }
            #endregion

            #region Incertidumbre de Medición.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la incertidumbre de medición del medidor registrado en "SistemaMedicionPozo". 
            /// Ejemplo: IncertidumbreMedicionSistMedicionPozo=0.010.
            /// </summary>
            public Decimal IncertidumbreMedicionSistMedicionPozo { set; get; }
            #endregion
        }
        #endregion
        #endregion

        #region Dispensario Datos.
        /// <summary>
        /// Datos del Dispensario.
        /// </summary>
        public struct stDispensarioDato
        {
            #region Clave Dispensario.
            /// <summary>
            /// REQUERIDO: 
            /// Clave al dispensario, para generarla deberás emplear el Apéndice 3. 
            /// Ejemplo: ClaveDispensario=DISP-0004.
            /// </summary>
            public String ClaveDispensario { set; get; }
            #endregion

            #region Medición Dispensario. (XML)
            /// <summary>
            /// REQUERIDO (XML): 
            /// Describe los medidores instalados en el dispensario y replicar este elemento por cada medidor instalado. 
            /// Estructura: stDispensarioMedicionDato.
            /// </summary>
            public Object MedicionDispensario { set; get; }
            #endregion

            #region Medición Dispensario. (JSON)
            /// <summary>
            /// REQUERIDO (JSON): 
            /// Describe los medidores instalados en el dispensario y replicar este elemento por cada medidor instalado. 
            /// Estructura: stDispensarioMedicionDato.
            /// </summary>
            public Object Medidores { set; get; }
            #endregion

            #region Manguera.
            /// <summary>
            /// REQUERIDO: 
            /// Información de las mangueras, también se manifiestan los registros de volumen de las entregas de producto.Recuerda replicar este elemento por cada manguera con la que cuente el dispensario. 
            /// Estructura: List<stMangueraDato>.
            /// </summary>
            public Object Manguera { set; get; }
            #endregion
        }

        #region Dispensario Medicion Datos.
        /// <summary>
        /// Datos de Medición del Pozo.
        /// </summary>
        public struct stDispensarioMedicionDato
        {
            #region Sistema de Medición del Dispensario.
            /// <summary>
            /// REQUERIDO: 
            /// Clave de identificación al sistema de medición instalado en el Dispensario, parar generarla deberás emplear el Apéndice 3. 
            /// Ejemplo: SistemaMedicionDispensario=SMD-DISP-0004.
            /// </summary>
            public String SistemaMedicionDispensario { set; get; }
            #endregion

            #region Localizacion o Descripcion del Sistema de Medicion del Ducto.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la información sobre la localización o descripción del sistema de medición que registraste en "SistemaMedicionDispensario".
            /// Ejemplo: LocalizODescripSistMedicionDispensario=Totalizador acumulado electrónico Marca LIQUID MEX3000.
            /// </summary>
            public String LocalizODescripSistMedicionDispensario { set; get; }
            #endregion

            #region Vigencia Calibracion.
            /// <summary>
            /// REQUERIDO: 
            /// Validez de la calibración del sistema de medición, formato yyyy-mm-dd. 
            /// Ejemplo: VigenciaCalibracionSistMedicionDispensario=2020-09-09.
            /// </summary>
            public String VigenciaCalibracionSistMedicionDispensario { set; get; }
            #endregion

            #region Incertidumbre de Medición.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la incertidumbre de medición del medidor registrado en "SistemaMedicionDispensario".
            /// Ejemplo: IncertidumbreMedicionSistMedicionDispensario=0.010.
            /// </summary>
            public Object IncertidumbreMedicionSistMedicionDispensario { set; get; }
            #endregion
        }
        #endregion

        #region Manguera Datos.
        /// <summary>
        /// Datos de la Manguera.
        /// </summary>
        public struct stMangueraDato
        {
            #region Identificador de la Manguera.
            /// <summary>
            /// REQUERIDO: 
            /// Clave a la manguera, para generarla deberás emplear el Apéndice 3. 
            /// Ejemplo: IdentificadorManguera=DISP-0004-MGA-0002.
            /// </summary>
            public String IdentificadorManguera { set; get; }
            #endregion

            #region Entregas.
            /// <summary>
            /// Registra la totalidad de las entregas del día.
            /// </summary>
            //public List<stEntregaMangueraDato> Entregas { set; get; }
            public stEntregaMangueraDato Entregas { set; get; }
            #endregion
        }
        #endregion

        public struct stEntregaMangueraDato
        {
            #region Total de Entregas.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el número total de las operaciones de entrega por las ventas. 
            /// Ejemplo: TotalEntregas = 4.
            /// </summary>
            public int TotalEntregas { set; get; }
            #endregion

            #region Suma de Volumen Entregado.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la suma del volumen que entregaste por ventas, autoconsumo o por prestaciones de servicio, según corresponda.
            /// </summary>
            public stCapacidadDato SumaVolumenEntregado { set; get; }
            #endregion

            #region Total Documentos.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el total de complementos vayas a relacionar, que contiene la información que ampare tus entregas. 
            /// Ejemplo: TotalDocumentos=4.
            /// </summary>
            public int TotalDocumentos { set; get; }
            #endregion

            #region Suma de Ventas.
            /// <summary>
            /// OPCIONAL: 
            /// Registra la suma de los importes totales de las transacciones de venta, así como de las contraprestaciones de servicios que realices. 
            /// Ejemplo: SumaVentas=400032.455.
            /// </summary>
            public Object SumaVentas { set; get; }
            #endregion

            #region ENTREGA.
            /// <summary>
            /// REQUERIDA: 
            /// Registra la totalidad de las entregas del día.
            /// Tipo: List<stEntregaMangueraIndividualDato>
            /// </summary>
            public Object Entrega { set; get; }
            #endregion
        }

        /// <summary>
        /// Datos de Entrega de Manguera Individual.
        /// </summary>
        public struct stEntregaMangueraIndividualDato
        {
            #region Numero de Registro.
            /// <summary>
            /// REQUERIDO: 
            /// Número de registro único y consecutivo de cada
            /// entrega por ventas o autoconsumo que realices, generado por el
            /// programa informático del sistema de medición. 
            /// Ejemplo: NumeroDeRegistro=670.
            /// </summary>
            public int NumeroDeRegistro { set; get; }
            #endregion

            #region Tipo de Registro.
            /// <summary>
            /// REQUERIDO: 
            /// Tipo de registro por transacción. 
            /// Ejemplo: TipoDeRegistro=D.
            /// </summary>
            public String TipoDeRegistro { set; get; }
            #endregion

            #region Volumen Entregado Totalizador Acum.
            /// <summary>
            /// REQUERIDO: 
            /// Volumen entregado, que resulta de la suma de cada operación o despacho de combustible efectuado(dato obtenido del totalizador acumulado).
            /// </summary>
            public stCapacidadDato VolumenEntregadoTotalizadorAcum { set; get; }
            #endregion

            #region Volumen Entregado Totalizador Insta.
            /// <summary>
            /// REQUERIDO: 
            /// Volumen entregado por cada operación o despacho de combustible(volumen del totalizador instantáneo). 
            /// </summary>
            public stCapacidadDato VolumenEntregadoTotalizadorInsta { set; get; }
            #endregion

            #region Precio Venta Totalizador Insta.
            /// <summary>
            /// OPCIONAL: 
            /// Registra el precio de venta en pesos por cada despacho de combustible, registrado en el totalizador instantáneo(registro de venta). 
            /// Ejemplo: PrecioVentaTotalizadorInsta = 570.000.
            /// </summary>
            public Decimal PrecioVentaTotalizadorInsta { set; get; }
            #endregion

            #region FechaYHoraEntrega.
            /// <summary>
            /// REQUERIDO: 
            /// Fecha y hora de cada operación de entrega o despacho de combustible, deberás señalar la hora del totalizador instantáneo. 
            /// Ejemplo: FechaYHoraEntrega=2020-12-25T20:34:10-01:00.
            /// </summary>
            public String FechaYHoraEntrega { set; get; }
            #endregion

            #region Complemento.
            /// <summary>
            /// OPCIONAL: 
            /// Relaciona un complemento por cada operación al volumen de entrega que acabas de manifestar.
            /// </summary>
            public Object Complemento { set; get; }
            #endregion
        }
        #endregion


        // =====  GENERALES  =====
        #region Medicion Datos.
        /// <summary>
        /// Datos de Medición del Tanque.
        /// </summary>
        public struct stMedicionTanqueDato
        {
            #region Sistema de Medición.
            /// <summary>
            /// REQUERIDO: 
            /// Clave de identificación al sistema de medición instalado en el tanque, para generarla deberás emplear el Apéndice 3. 
            /// Ejemplo: SistemaMedicionTanque=SME-TQS-TDA-0001.
            /// </summary>
            public String SistemaMedicionTanque { set; get; }
            #endregion

            #region Localizacion o Descripcion.
            /// <summary>
            /// REQUERIDO: 
            /// Información sobre la localización o descripción del sistema de medición que registraste en SistemaMedicionTanque. 
            /// Ejemplo: LocalizODescripSistMedicionTanque=medidor de nivel MEDIMEX30000.
            /// </summary>
            public String LocalizODescripSistMedicionTanque { set; get; }
            #endregion

            #region Vigencia de Calibración.
            /// <summary>
            /// REQUERIDO: 
            /// Validez de la calibración del sistema de medición de cada tanque, en formato "yyyy-mm-dd", de acuerdo con la especificación ISO 8601. 
            /// Ejemplo: VigenciaCalibracionSistMedicionTanque=2020-06-29.
            /// </summary>
            public String VigenciaCalibracionSistMedicionTanque { set; get; }
            #endregion

            #region Incertidumbre de Medición.
            /// <summary>
            /// REQUERIDO: 
            /// Incertidumbre de medición del medidor registrado en SistemaMedicionTanque. 
            /// Ejemplo: IncertidumbreMedicionSistMedicionTanque=0.010.
            /// </summary>
            public Object IncertidumbreMedicionSistMedicionTanque { set; get; }
            #endregion

            public void Inicializar()
            {
                this.SistemaMedicionTanque = String.Empty;
                this.LocalizODescripSistMedicionTanque = String.Empty;
                this.VigenciaCalibracionSistMedicionTanque = String.Empty;
                this.IncertidumbreMedicionSistMedicionTanque = 0;
            }
        }
        #endregion

        #region Existencia Datos.
        /// <summary>
        /// Datos de la Existencia.
        /// </summary>
        public struct stExistenciaDato
        {
            #region Volumen Existencia.
            /// <summary>
            /// REQUERIDO: 
            /// Registrar las existencias del día anterior, es decir, se refiere al inventario inicial con el cual el tanque inicia en el día. 
            /// </summary>
            //public stCapacidadDato VolumenExistenciasAnterior { set; get; }
            public object VolumenExistenciasAnterior { set; get; }
            #endregion

            #region Volumen Acum Ops Recepcion.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el volumen total de las operaciones de recepción realizadas en las 24 horas anteriores a generar el registro.
            /// </summary>
            //public stCapacidadDato VolumenAcumOpsRecepcion { set; get; }
            public stVolumenDato VolumenAcumOpsRecepcion { set; get; }
            #endregion

            #region Hora Recepcion Acumulado.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la hora en la que se efectuaste el registro de volumen total de las operaciones de recepción. 
            /// Ejemplo: HoraRecepcionAcumulado=11:15:45-01:00.
            /// </summary>
            public String HoraRecepcionAcumulado { set; get; }
            #endregion

            #region Volumen Acum Ops Entrega.
            /// <summary>
            /// REQUERIDO: 
            /// Sirve para registrar el volumen total de las operaciones de entrega realizadas en las 24 horas anteriores a generar el registro.
            /// </summary>
            public stCapacidadDato VolumenAcumOpsEntrega { set; get; }
            #endregion

            #region Hora Entrega Acumulado.
            /// <summary>
            /// REQUERIDO: 
            /// Hora en la que efectuaste el registro de volumen acumulado.
            /// Ejemplo: HoraEntregaAcumulado=11:32:14-01:00.
            /// </summary>
            public String HoraEntregaAcumulado { set; get; }
            #endregion

            #region Volumen Existencias.
            /// <summary>
            /// REQUERIDO: 
            /// La existencia del día se obtiene de sumar a las existencias del día anterior, el volumen total de
            /// las operaciones de recepción realizadas en las 24 horas anteriores y restando el volumen total
            /// de las operaciones de entrega realizadas en las 24 horas anteriores.
            /// </summary>
            //public stCapacidadDato VolumenExistencias { set; get; }
            public Object VolumenExistencias { set; get; }
            #endregion

            #region Fecha Y Hora Esta Medicion.
            /// <summary>
            /// REQUERIDO: 
            /// Fecha y hora de la medición de las existencias del día. 
            /// Ejemplo: FechaYHoraEstaMedicion=2020-10-31T11:59:45-01:00.
            /// </summary>
            public String FechaYHoraEstaMedicion { set; get; }
            #endregion

            #region Fecha Y Hora Medicion Anterior.
            /// <summary>
            /// REQUERIDO: 
            /// Fecha y hora de la medición de las existencias del día anterior. 
            /// Ejemplo: FechaYHoraMedicionAnterior=2020-10-30T11:59:45-01:00.
            /// </summary>
            public String FechaYHoraMedicionAnterior { set; get; }
            #endregion
        }
        #endregion

        #region Recepción Datos.
        public struct stRecepcionDato
        {
            #region Total de Recepciones.
            /// <summary>
            /// REQUERIDO: 
            /// Total de las operaciones de recepción del tanque en el día de la generación del reporte. 
            /// Ejemplo: TotalRecepciones=2.
            /// </summary>
            public int TotalRecepciones { set; get; }
            #endregion

            #region Suma Volumen Recepcion.
            /// <summary>
            /// REQUERIDO: 
            /// Debes registrar la suma del volumen que recibiste, ya sea por compras o por prestaciones de servicio o 
            /// recepciones provenientes de la misma instalación, producto de las actividades que realizas.
            /// </summary>
            //public stCapacidadDato SumaVolumenRecepcion { set; get; }
            public Object SumaVolumenRecepcion { set; get; }
            #endregion

            #region Total Documentos.
            /// <summary>
            /// REQUERIDO: 
            /// Total de complementos que vayas a relacionar, que contiene la información que ampare tus recepciones. 
            /// Ejemplo: TotalDocumentos=2.
            /// </summary>
            public int TotalDocumentos { set; get; }
            #endregion

            #region Suma Compras.
            /// <summary>
            /// OPCIONAL: 
            /// Registra la suma de los importes totales de las transacciones de compra, en
            /// caso de que tu actividad sea una prestación de servicios no deberá existir. 
            /// Ejemplo: SumaCompras=150455.000.
            /// </summary>
            public Object SumaCompras { set; get; }
            #endregion

            #region Recepción.
            /// <summary>
            /// REQUERIDO: 
            /// Registra de manera individual cada una de las recepciones.
            /// Estructura: List<stRecepcionIndividualDato>
            /// </summary>
            public Object Recepcion { set; get; }
            #endregion
        }

        #region Recepcion Individual Datos.
        /// <summary>
        /// Datos de la Recepcion Individual.
        /// </summary>
        public struct stRecepcionIndividualDato
        {
            #region Numero de Registros.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el número de registro, único y consecutivo, de cada recepción, recopilado por el programa informático del sistema de medición. 
            /// Ejemplo: NumeroDeRegistro=1680.
            /// </summary>
            /// 
            public int NumeroDeRegistro { set; get; }
            #endregion

            #region Volumen Inicial Tanque
            /// <summary>
            /// REQUERIDO: 
            /// Registra el volumen inicial del tanque antes de la recepción de producto. 
            /// </summary>
            public stCapacidadDato VolumenInicialTanque { set; get; }
            #endregion

            #region Volumen Final Tanque.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el volumen final después de la recepción de producto.
            /// </summary>
            //public stCapacidadDato VolumenFinalTanque { set; get; }
            public Object VolumenFinalTanque { set; get; }
            #endregion

            #region Volumen Recepcion.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el volumen recibido de producto, es decir, el registro de volumen
            /// proveniente de la medición dinámica de la carga al tanque, o la calculada a partir
            /// de tus volúmenes iniciales y finales provenientes de la telemedición del tanque,
            /// tomando en cuenta las entregas que se realicen durante la recepción al tanque.
            /// </summary>
            public stCapacidadDato VolumenRecepcion { set; get; }
            #endregion

            #region Temperatura.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la temperatura del hidrocarburo o petrolífero a condiciones de referencia. 
            /// Ejemplo: Temperatura=20.
            /// </summary>
            public Object Temperatura { set; get; }
            #endregion

            #region Presion Absoluta.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la presión a condiciones de referencia. Este dato debes obtenerlo de tu sistema de medición. 
            /// Ejemplo: PresionAbsoluta=101.325.
            /// </summary>
            public Decimal PresionAbsoluta { set; get; }
            #endregion

            #region Fecha Y Hora Inicio de Recepcion.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la fecha y hora inicial de la operación de recepción. 
            /// Ejemplo: FechaYHoraInicioRecepcion=2020-10-31T10:59:45-01:00.
            /// </summary>
            public String FechaYHoraInicioRecepcion { set; get; }
            #endregion

            #region Fecha Y Hora Final de Recepción.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la fecha y hora final de la operación de recepción. 
            /// Ejemplo: FechaYHoraFinalRecepcion=2020-10-31T11:59:45-01:00.
            /// </summary>
            public String FechaYHoraFinalRecepcion { set; get; }
            #endregion

            #region Complemento.
            /// <summary>
            /// OPCIONAL: 
            /// Registra la relacion un complemento por cada operación al volumen de recepción que acabas de manifestar.
            /// </summary>
            public Object Complemento { set; get; }
            #endregion
        }

        /// <summary>
        /// Datos de la Recepcion Individual.
        /// </summary>
        public struct stDuctoRecepcionIndividualDato
        {
            #region Numero de Registros.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el número de registro, único y consecutivo, de cada recepción, recopilado por el programa informático del sistema de medición. 
            /// Ejemplo: NumeroDeRegistro=1680.
            /// </summary>
            /// 
            public int NumeroDeRegistro { set; get; }
            #endregion

            #region Volumen Punto Entrada
            /// <summary>
            /// REQUERIDO: 
            /// Requerido para expresar el volumen a la entrada del ducto antes de la recepción del producto por cada operación, puede considerar el volumen de gas de empaque. 
            /// </summary>
            public Object VolumenPuntoEntrada { set; get; }
            #endregion

            #region Volumen Recepcion.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el volumen recibido de producto, es decir, el registro de volumen
            /// proveniente de la medición dinámica de la carga al tanque, o la calculada a partir
            /// de tus volúmenes iniciales y finales provenientes de la telemedición del tanque,
            /// tomando en cuenta las entregas que se realicen durante la recepción al tanque.
            /// </summary>
            public Object VolumenRecepcion { set; get; }
            #endregion

            #region Temperatura.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la temperatura del hidrocarburo o petrolífero a condiciones de referencia. 
            /// Ejemplo: Temperatura=20.
            /// </summary>
            public Object Temperatura { set; get; }
            #endregion

            #region Presion Absoluta.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la presión a condiciones de referencia. Este dato debes obtenerlo de tu sistema de medición. 
            /// Ejemplo: PresionAbsoluta=101.325.
            /// </summary>
            public Decimal PresionAbsoluta { set; get; }
            #endregion

            #region Fecha Y Hora Inicio de Recepcion.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la fecha y hora inicial de la operación de recepción. 
            /// Ejemplo: FechaYHoraInicioRecepcion=2020-10-31T10:59:45-01:00.
            /// </summary>
            public String FechaYHoraInicioRecepcion { set; get; }
            #endregion

            #region Fecha Y Hora Final de Recepción.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la fecha y hora final de la operación de recepción. 
            /// Ejemplo: FechaYHoraFinalRecepcion=2020-10-31T11:59:45-01:00.
            /// </summary>
            public String FechaYHoraFinalRecepcion { set; get; }
            #endregion

            #region Complemento.
            /// <summary>
            /// OPCIONAL: 
            /// Registra la relacion un complemento por cada operación al volumen de recepción que acabas de manifestar.
            /// </summary>
            public Object Complemento { set; get; }
            #endregion
        }
        #endregion

        #region Complemento Estacion.
        public struct stComplementoRecepcionDato
        {
            /// <summary>
            /// Opciones: “Almacenamiento", “CDLR", “Comercializacion", “Distribucion", “Expendio", “Extraccion", “Refinacion", “Transporte".
            /// </summary>
            public Object TipoComplemento { set; get; }
            /// <summary>
            /// Deberá manifestarse cuando se contraten servicios de almacenamiento, distribución, transporte, licuefacción o regasificación. 
            /// Estructura: stAlmacenamientoDato (Objeto)
            /// </summary>
            public Object TerminalAlmYDist { set; get; }
            /// <summary>
            /// Estructura: stNacionalDato. (Array) 
            /// Deberá manifestarse cuando el cliente o proveedor sea nacional.
            /// </summary>
            public Object Nacional { set; get; }
            public Object Dictamen { set; get; }
            public Object Extranjero { set; get; }
            public Object ACLARACION { set; get; }
        }

        /// <summary>
        /// Estructura: stTerminalAlmDato (Objeto) 
        /// </summary>
        public struct stAlmacenamientoDato
        {
            /// <summary>
            /// Objeto para describir la terminal de almacenamiento. 
            /// Estructura: stTerminalAlmDato.
            /// </summary>
            public Object Almacenamiento { set; get; }
            /// <summary>
            /// Objeto para describir el servicio de transporte contratado.
            /// </summary>
            public Object Transporte { set; get; }
        }
        /// <summary>
        /// Datos del Transporte.
        /// </summary>
        public struct stTransporteDato
        {
            public Object PermisoTrasporte { set; get; }
            public Object ClaveVehiculo { set; get; }
            public Object TarifaTransporte { set; get; }
            public Object CargoPorCapacidadTrans { set; get; }
            public Object CargoUsoTrans { set; get; }
            public Object CargoVolumetricoTrans { set; get; }
            public Object TarifaSuministro { set; get; }
        }
        public struct stTerminalAlmDato
        {
            /// <summary>
            /// REQUERIDO: 
            /// Requerido para especificar la terminal de almacenamiento y distribución de embarque del producto o distribuidor autorizado, cuando el permisionario de comercialización gestione la entrega en un terminal de almacenamiento o distribución o contrate servicios de licuefacción o regasificación de gas natural. 
            /// </summary>
            public Object TerminalAlmYDist { set; get; }
            /// <summary>
            /// Permiso.
            /// </summary>
            public Object PermisoAlmYDist { set; get; }
        }

        /// <summary>
        /// Datos del Nacional.
        /// </summary>
        public struct stNacionalDato
        {
            public Object RfcClienteOProveedor { set; get; }
            public Object NombreClienteOProveedor { set; get; }
            public Object PermisoProveedor { set; get; }
            /// <summary>
            /// Estructura: stCFDIDato. (ARRAY)
            /// </summary>
            public Object CFDIs { set; get; }
        }

        public struct stExtranjeroDato
        {
            public Object PermisoImportacionOExportacion { set; get; }
            /// <summary>
            /// Lista con los Pedimentos. (Estructura: stPedimentoDato)
            /// </summary>
            public Object PEDIMENTOS { set; get; }
        }

        public struct stPedimentoDato
        {
            public Object PuntoDeInternacionOExtraccion { set; get; }
            public Object PaisOrigenODestino { set; get; }
            public Object MedioDeTransEntraOSaleAduana { set; get; }
            public Object PedimentoAduanal { set; get; }
            public Object Incoterms { set; get; }
            public Object PrecioDeImportacionOExportacion { set; get; }
            /// <summary>
            /// Estructura: stVolumenDato
            /// </summary>
            public Object VolumenDocumentado { set; get; }
        }

        /// <summary>
        /// Datos del CFDI.
        /// </summary>
        public struct stCFDIDato
        {
            /// <summary>
            /// REQUERIDO: 
            /// </summary>
            public Object Cfdi { set; get; }
            /// <summary>
            /// REQUERIDO: 
            /// Opciones “Ingreso", “Egreso", “Traslado"
            /// </summary>
            public Object TipoCfdi { set; get; }
            /// <summary>
            /// REQUERIDO
            /// </summary>
            public Object PrecioCompra { set; get; }
            public Object PrecioDeVentaAlPublico { set; get; }
            public Object PrecioVenta { set; get; }
            public Object FechaYHoraTransaccion { set; get; }
            /// <summary>
            /// Estructura: stVolumenDato. (Objeto)
            /// </summary>
            public Object VolumenDocumentado { set; get; }

            /// <summary>
            /// Campo definido para pruebas al generar Rechazo de Complemento.
            /// </summary>
            public Object PrecioVentaOCompraOContrap { set; get; }
            /// <summary>
            /// Utilizado en Reporte Mensual
            /// </summary>
            public Object Aclaracion { set; get; }
        }

        public struct stAclaracionDato
        {
            public Object Aclaracion { set; get; }
        }
        #endregion
        #endregion

        #region Entrega Datos.
        public struct stEntregaDato
        {
            #region Total de Entregas.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el número total de las operaciones de entrega por las ventas. 
            /// Ejemplo: TotalEntregas = 4.
            /// </summary>
            public int TotalEntregas { set; get; }
            #endregion

            #region Suma de Volumen Entregado.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la suma del volumen que entregaste por ventas, autoconsumo o por prestaciones de servicio, según corresponda.
            /// </summary>
            //public stCapacidadDato SumaVolumenEntregado { set; get; }
            public Object SumaVolumenEntregado { set; get; }
            #endregion

            #region Total Documentos.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el total de complementos vayas a relacionar, que contiene la información que ampare tus entregas. 
            /// Ejemplo: TotalDocumentos=4.
            /// </summary>
            public int TotalDocumentos { set; get; }
            #endregion

            #region Suma de Ventas.
            /// <summary>
            /// OPCIONAL: 
            /// Registra la suma de los importes totales de las transacciones de venta, así como de las contraprestaciones de servicios que realices. 
            /// Ejemplo: SumaVentas=400032.455.
            /// </summary>
            public Object SumaVentas { set; get; }
            #endregion

            #region ENTREGA.
            /// <summary>
            /// REQUERIDA: 
            /// Registra la totalidad de las entregas del día.
            /// Estructura: List<stEntregaIndividualDato>.
            /// </summary>
            public Object Entrega { set; get; }
            #endregion
        }

        #region Entrega Individual Datos.
        /// <summary>
        /// Datos de la Entrega Individual.
        /// </summary>
        public struct stEntregaIndividualDato
        {
            #region Numero de Registros.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el número de registro, único y consecutivo, de cada recepción, recopilado por el programa informático del sistema de medición. 
            /// Ejemplo: NumeroDeRegistro=1680.
            /// </summary>
            /// 
            public int NumeroDeRegistro { set; get; }
            #endregion

            #region Volumen Inicial Tanque
            /// <summary>
            /// REQUERIDO: 
            /// Registra el volumen inicial del tanque antes de la recepción de producto. 
            /// </summary>
            public stCapacidadDato VolumenInicialTanque { set; get; }
            #endregion

            #region Volumen Final Tanque.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el volumen final después de la recepción de producto.
            /// </summary>
            public Object VolumenFinalTanque { set; get; }
            #endregion

            #region VolumenEntregado.
            /// <summary>
            /// REQUERIDO: 
            /// Requerido para expresar el volumen entregado por cada operación
            /// de venta, entrega, suministro o exportación, a partir de la
            /// medición anterior(cantidad expresada en litros tratándose de petrolíferos,
            /// pie cúbico tratándose de gas natural para contratistas y
            /// asignatarios, metro cúbico tratándose de gas natural en los demás
            /// casos, barril tratándose de petróleo y condensados).
            /// </summary>
            public stCapacidadDato VolumenEntregado { set; get; }
            #endregion

            #region Temperatura.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la temperatura del hidrocarburo o petrolífero a condiciones de referencia. 
            /// Ejemplo: Temperatura=20.
            /// </summary>
            public Object Temperatura { set; get; }
            #endregion

            #region Presion Absoluta.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la presión a condiciones de referencia. Este dato debes obtenerlo de tu sistema de medición. 
            /// Ejemplo: PresionAbsoluta=101.325.
            /// </summary>
            public Decimal PresionAbsoluta { set; get; }
            #endregion

            #region FechaYHoraInicialEntrega.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la fecha y hora inicial de la operación de entrega. 
            /// Ejemplo: FechaYHoraInicioRecepcion=2020-10-31T10:59:45-01:00.
            /// </summary>
            public String FechaYHoraInicialEntrega { set; get; }
            #endregion

            #region Fecha Y Hora Final de Entrega.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la fecha y hora final de la operación de recepción. 
            /// Ejemplo: FechaYHoraFinalRecepcion=2020-10-31T11:59:45-01:00.
            /// </summary>
            public String FechaYHoraFinalEntrega { set; get; }
            #endregion

            #region Complemento.
            /// <summary>
            /// OPCIONAL: 
            /// Registra la relacion un complemento por cada operación al volumen de recepción que acabas de manifestar.
            /// </summary>
            public Object Complemento { set; get; }
            #endregion
        }

        /// <summary>
        /// Datos de la Entrega Individual para los Ductos.
        /// </summary>
        public struct stDuctoEntregaIndividualDato
        {
            #region Numero de Registros.
            /// <summary>
            /// REQUERIDO: 
            /// Registra el número de registro, único y consecutivo, de cada recepción, recopilado por el programa informático del sistema de medición. 
            /// Ejemplo: NumeroDeRegistro=1680.
            /// </summary>
            /// 
            public int NumeroDeRegistro { set; get; }
            #endregion

            #region Volumen Punto Salida
            /// <summary>
            /// REQUERIDO: 
            /// Requerido para expresar el volumen a la salida del ducto antes de la entrega del producto por cada operación de venta, entrega, suministro o exportación. 
            /// </summary>
            public Object VolumenPuntoSalida { set; get; }
            #endregion

            #region Volumen Entregado.
            /// <summary>
            /// REQUERIDO: 
            /// Requerido para expresar el volumen entregado por cada operación de venta, entrega, suministro o exportación.
            /// </summary>
            public Object VolumenEntregado { set; get; }
            #endregion

            #region Temperatura.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la temperatura del hidrocarburo o petrolífero a condiciones de referencia. 
            /// Ejemplo: Temperatura=20.
            /// </summary>
            public Object Temperatura { set; get; }
            #endregion

            #region Presion Absoluta.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la presión a condiciones de referencia. Este dato debes obtenerlo de tu sistema de medición. 
            /// Ejemplo: PresionAbsoluta=101.325.
            /// </summary>
            public Decimal PresionAbsoluta { set; get; }
            #endregion

            #region Fecha Y Hora Inicio de Entrega.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la fecha y hora inicial de la operación de entrega. 
            /// Ejemplo: FechaYHoraInicioRecepcion=2020-10-31T10:59:45-01:00.
            /// </summary>
            public String FechaYHoraInicialEntrega { set; get; }
            #endregion

            #region Fecha Y Hora Final de Entrega.
            /// <summary>
            /// REQUERIDO: 
            /// Registra la fecha y hora final de la operación de entrega. 
            /// Ejemplo: FechaYHoraFinalRecepcion=2020-10-31T11:59:45-01:00.
            /// </summary>
            public String FechaYHoraFinalEntrega { set; get; }
            #endregion
        }
        #endregion
        #endregion

        public struct stVolumenDato
        {
            public Object ValorNumerico { set; get; }
            //public stUnidadMedidaDato UnidadDeMedida { set; get; }
            public Object UnidadDeMedida { set; get; }
        }

        #region Complemento: Distribución.
        /// <summary>
        /// Estructura Inicial para el Complemento de Distribución.
        /// </summary>
        public struct stComplementoDistribucion
        {
            /// <summary>
            /// Tipo de Complemento: “Almacenamiento", “CDLR", “Comercializacion", “Distribucion", “Expendio", “Extraccion", “Refinacion", “Transporte".
            /// </summary>
            public Object TipoComplemento { set; get; }
            /// <summary>
            /// Estructura: stCompleDistTerminalAlmYTrans
            /// </summary>
            public Object TerminalAlmYTrans { set; get; }
            public Object Dictamen { set; get; }
            public Object Certificado { set; get; }
            /// <summary>
            /// Estructura: stCompleDistNacional
            /// </summary>
            public Object Nacional { set; get; }
            /// <summary>
            /// Estructura: stCompleDistExtranjero
            /// </summary>
            public Object Extranjero { set; get; }
            public Object Aclaracion { set; get; }
        }

        #region Estructuras Nodo: TERMINALALMYTRANS.
        public struct stCompleDistTerminalAlmYTrans
        {
            /// <summary>
            /// Estructura: stCompleDistTerminalAlmYTrans_AlmacenDatos
            /// </summary>
            public Object Almacenamiento { set; get; }
            /// <summary>
            /// Estructura: stCompleDistTerminalAlmYTrans_TransporteDato
            /// </summary>
            public Object Transporte { set; get; }
        }

        public struct stCompleDistTerminalAlmYTrans_TransporteDato
        {
            public Object PermisoTransporte { set; get; }
            public Object ClaveDeVehiculo { set; get; }
            public Object TarifaDeTransporte { set; get; }
            public Object CargoPorCapacidadTrans { set; get; }
            public Object CargoPorUsoTrans { set; get; }
            public Object CargoVolumetricoTrans { set; get; }
            public Object TarifaDeSuministro { set; get; }
        }
        #endregion

        #region Estructuras Nodo: NACIONAL.
        public struct stCompleDistNacional
        {
            public Object RfcClienteOProveedor { set; get; }
            public Object NombreClienteOProveedor { set; get; }
            public Object PermisoClienteOProveedor { set; get; }
            /// <summary>
            /// Estructura: stCompleDistNacionalCfdis
            /// </summary>
            public Object CFDIs { set; get; }
        }

        public struct stCompleDistNacionalCfdis
        {
            public Object Cfdi { set; get; }
            public Object TipoCfdi { set; get; }
            public Object PrecioVentaOCompraOContrap { set; get; }
            public Object FechaYHoraTransaccion { set; get; }
            /// <summary>
            /// Estructura: stVolumenDatos.
            /// </summary>
            public Object VolumenDocumentado { set; get; }
        }
        #endregion

        #region Estructura Nodo: EXTRANJERO.
        public struct stCompleDistExtranjero
        {
            public Object PermisoImportacionOExportacion { set; get; }
            /// <summary>
            /// Estructura: stCompleDistExtranjeroPedimentos.
            /// </summary>
            public Object Pedimentos { set; get; }
        }

        public struct stCompleDistExtranjeroPedimentos
        {
            public Object PuntoDeInternacionOExtraccion { set; get; }
            public Object PaisOrigenODestino { set; get; }
            public Object MedioDeTransEntraOSaleAduana { set; get; }
            public Object PedimentoAduanal { set; get; }
            public Object Incoterms { set; get; }
            public Object PrecioDeImportacionOExportacion { set; get; }
            /// <summary>
            /// Estructura: stVolumenDatos.
            /// </summary>
            public Object VolumenDocumentado { set; get; }
        }
        #endregion
        #endregion

        #region Complemento: Transportista.
        public struct stComplementoTransportista
        {
            /// <summary>
            /// Tipo de Complemento: “Almacenamiento", “CDLR", “Comercializacion", “Distribucion", “Expendio", “Extraccion", “Refinacion", “Transporte".
            /// </summary>
            public Object TipoComplemento { set; get; }
            /// <summary>
            /// Estructura: stComplementoTransTerminalAlmYDist.
            /// </summary>
            public Object TerminalAlmYDist { set; get; }
            public Object Certificado { set; get; }
            /// <summary>
            /// Estructura: stComplementoTransNacional.
            /// </summary>
            public Object Nacional { set; get; }
            public Object Aclaracion { set; get; }
        }

        #region Estructura Nodo: TERMINALALMYDIST.
        public struct stComplementoTransTerminalAlmYDist
        {
            public Object TerminalAlmYDist { set; get; }
            public Object PermisoAlmYDist { set; get; }
        }
        #endregion

        #region Estructura Nodo: NACIONAL.
        public struct stComplementoTransNacional
        {
            public Object RfcCliente { set; get; }
            public Object NombreCliente { set; get; }
            /// <summary>
            /// Estructura: stCompleTransNacionalCfdis.
            /// </summary>
            public Object CFDIs { set; get; }
        }
        public struct stCompleTransNacionalCfdis
        {
            public Object Cfdi { set; get; }
            public Object TipoCfdi { set; get; }
            public Object Contraprestacion { set; get; }
            public Object TarifaDeTransporte { set; get; }
            public Object CargoPorCapacidadDeTrans { set; get; }
            public Object CargoPorUsoTrans { set; get; }
            public Object CargoVolumetricoTrans { set; get; }
            public Object Descuento { set; get; }
            public Object FechaYHoraTransaccion { set; get; }
            /// <summary>
            /// Estructura: stVolumenDatos.
            /// </summary>
            public Object VolumenDocumentado { set; get; }
        }
        #endregion
        #endregion

        #region Complemento: Comercializadora.
        public struct stComplementoComercializadora
        {
            /// <summary>
            /// Tipo de Complemento: “Almacenamiento", “CDLR", “Comercializacion", “Distribucion", “Expendio", “Extraccion", “Refinacion", “Transporte".
            /// </summary>
            public Object TipoComplemento { set; get; }
            /// <summary>
            /// Estructura: stCompleComerTerminalAlmYDist.
            /// </summary>
            public Object TerminalAlmYDist { set; get; }
            public Object Dictamen { set; get; }
            public Object Certificado { set; get; }
            /// <summary>
            /// Estructura: stCompleComerNacional.
            /// </summary>
            public Object Nacional { set; get; }
            /// <summary>
            /// Estructura: stCompleComerExtranjero.
            /// </summary>
            public Object Extranjero { set; get; }
            public Object Aclaracion { set; get; }
        }

        #region Estructura Nodo: TERMINALALMYDIST.
        public struct stCompleComerTerminalAlmYDist
        {
            public Object Almacenamiento { set; get; }
            /// <summary>
            /// Estructura: stCompleComerTerminalAlmYTrans_TransporteDato.
            /// </summary>
            public Object Transporte { set; get; }
        }

        public struct stCompleComerTerminalAlmYTrans_TransporteDato
        {
            public Object PermisoTransporte { set; get; }
            public Object ClaveDeVehiculo { set; get; }
            public Object TarifaDeTransporte { set; get; }
            public Object CargoPorCapacidadTrans { set; get; }
            public Object CargoPorUsoTrans { set; get; }
            public Object CargoVolumetricoTrans { set; get; }
        }
        #endregion

        #region Estructura Nodo: NACIONAL.
        public struct stCompleComerNacional
        {
            public Object RfcClienteOProveedor { set; get; }
            public Object NombreClienteOProveedor { set; get; }
            public Object PermisoProveedor { set; get; }
            /// <summary>
            /// Estructura: stCompleComerNacionalCfdis.
            /// </summary>
            public Object CFDIs { set; get; }
        }

        public struct stCompleComerNacionalCfdis
        {
            public Object Cfdi { set; get; }
            public Object TipoCfdi { set; get; }
            public Object PrecioCompra { set; get; }
            public Object PrecioDeVentaAlPublico { set; get; }
            public Object PrecioVenta { set; get; }
            public Object FechayHoraTransaccion { set; get; }
            /// <summary>
            /// Estructura: stVolumenDatos.
            /// </summary>
            public Object VolumenDocumentado { set; get; }
        }
        #endregion

        #region Estructura Nodo: EXTRANJERO.
        public struct stCompleComerExtranjero
        {
            public Object PermisoImportacion { set; get; }
            /// <summary>
            /// Estructura: stCompleComerExtranjeroPedimentos.
            /// </summary>
            public Object Pedimentos { set; get; }
        }

        public struct stCompleComerExtranjeroPedimentos
        {
            public Object PuntoDeInternacion { set; get; }
            public Object PaisOrigen { set; get; }
            public Object MedioDeTransEntraAduana { set; get; }
            public Object PedimentoAduanal { set; get; }
            public Object Incoterms { set; get; }
            public Object PrecioDeImportacion { set; get; }
            /// <summary>
            /// Estructura: stVolumenDatos.
            /// </summary>
            public Object VolumenDocumentado { set; get; }
        }
        #endregion
        #endregion

        #endregion
        // =======================
        //#endregion

        #region Bitacora Datos.
        public struct stBitacoraDato
        {
            #region Número Registros.
            /// <summary>
            /// REQUERIDO: 
            /// Número de registro, único y consecutivo del evento. 
            /// Ejemplo: NumeroRegistro=25.
            /// </summary>
            public int NumeroRegistro { set; get; }
            #endregion

            #region Fecha y Hora Eventos.
            /// <summary>
            /// REQUERIDO: 
            /// Fecha y hora en que se generó el evento. 
            /// Ejemplo: NumeroRegistro=2020-04-10T12:00:00-00:00.
            /// </summary>
            public String FechaYHoraEvento { set; get; }
            #endregion

            #region Usuario Responsable.
            /// <summary>
            /// OPCIONAL: 
            /// Registro del usuario responsable de registrar el evento.
            /// Ejemplo: UsuarioResponsable=Emiliano Torres Mejía.
            /// </summary>
            public String UsuarioResponsable { set; get; }
            #endregion

            #region Tipo de Evento.
            /// <summary>
            /// REQUERIDO: 
            /// Registro del tipo de evento que se generó. 
            /// Ejemplo: TipoEvento=15.
            /// </summary>
            public int TipoEvento { set; get; }
            #endregion

            #region Descripcion del Evento.
            /// <summary>
            /// REQUERIDO: 
            /// Descripción del evento de que se trate. En caso de que hayas 
            /// registrado en el elemento TipoEvento el 7, deberás registrar 
            /// dentro de la descripción el porcentaje exacto de diferencia. 
            /// Ejemplo: DescripcionEvento=falla eléctrica, se reinicia sistema no se reportan incidentes.
            /// </summary>
            public String DescripcionEvento { set; get; }
            #endregion

            #region .
            /// <summary>
            /// OPCIONAL: 
            /// SI en caso de que hayas registrado alguno de los siguientes eventos en el elemento TipoEvento: 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 o 21. 
            /// Ejemplo: IdentificacionComponenteAlarma = canal de comunicación.
            /// </summary>
            public object IdentificacionComponenteAlarma { set; get; }
            #endregion
        }
        #endregion
        // ======================
        #endregion
    }
}