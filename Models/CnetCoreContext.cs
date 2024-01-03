using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace APIControlNet.Models
{
    public partial class CnetCoreContext : IdentityDbContext
    {
        public CnetCoreContext()
        {
        }

        public CnetCoreContext(DbContextOptions<CnetCoreContext> options)
            : base(options)
        {
        }
        public virtual DbSet<AuthorizationSet> AuthorizationSets { get; set; }
        public virtual DbSet<AutoTanque> AutoTanques { get; set; }
        public virtual DbSet<BankCard> BankCards { get; set; }
        public virtual DbSet<Binnacle> Binnacles { get; set; }
        public virtual DbSet<BinnacleLog> BinnacleLogs { get; set; }
        public virtual DbSet<BinnacleService> BinnacleServices { get; set; }
        public virtual DbSet<BinnacleType> BinnacleTypes { get; set; }
        public virtual DbSet<Button> Buttons { get; set; }
        public virtual DbSet<ButtonPerUser> ButtonPerUsers { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<CardType> CardTypes { get; set; }
        public virtual DbSet<ClienteProveedorCvol> ClienteProveedorCvols { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyAddress> CompanyAddresses { get; set; }
        public virtual DbSet<CompanyFiel> CompanyFiels { get; set; }
        public virtual DbSet<ComposicionGasNaturalOcondensado> ComposicionGasNaturalOcondensados { get; set; }
        public virtual DbSet<CompraComplementoMesSat> CompraComplementoMesSats { get; set; }
        public virtual DbSet<ConfigGral> ConfigGrals { get; set; }
        public virtual DbSet<ConfigView> ConfigViews { get; set; }
        public virtual DbSet<ConfigViewFrame> ConfigViewFrames { get; set; }
        public virtual DbSet<ConfigViewItem> ConfigViewItems { get; set; }
        public virtual DbSet<ConfigViewRelated> ConfigViewRelateds { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public virtual DbSet<CustomerControl> CustomerControls { get; set; }
        public virtual DbSet<CustomerLimit> CustomerLimits { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }
        public virtual DbSet<DailySummary> DailySummaries { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceLog> DeviceLogs { get; set; }
        public virtual DbSet<Dispensary> Dispensaries { get; set; }
        public virtual DbSet<DispensaryBrand> DispensaryBrands { get; set; }
        public virtual DbSet<Ducto> Ductos { get; set; }
        public virtual DbSet<DuctoEntrega> DuctoEntregas { get; set; }
        public virtual DbSet<DuctoEntregaCfdi> DuctoEntregaCfdis { get; set; }
        public virtual DbSet<DuctoEntregaPedimento> DuctoEntregaPedimentos { get; set; }
        public virtual DbSet<DuctoEntregaTransporte> DuctoEntregaTransportes { get; set; }
        public virtual DbSet<DuctoMedidor> DuctoMedidors { get; set; }
        public virtual DbSet<DuctoRecepcion> DuctoRecepcions { get; set; }
        public virtual DbSet<DuctoRecepcionCfdi> DuctoRecepcionCfdis { get; set; }
        public virtual DbSet<DuctoRecepcionPedimento> DuctoRecepcionPedimentos { get; set; }
        public virtual DbSet<DuctoRecepcionTransporte> DuctoRecepcionTransportes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<FielType> FielTypes { get; set; }
        public virtual DbSet<Folio> Folios { get; set; }
        public virtual DbSet<Hose> Hoses { get; set; }
        public virtual DbSet<HoseCalibration> HoseCalibrations { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryIn> InventoryIns { get; set; }
        public virtual DbSet<InventoryInDocument> InventoryInDocuments { get; set; }
        public virtual DbSet<InventoryInSaleOrder> InventoryInSaleOrders { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceApplicationType> InvoiceApplicationTypes { get; set; }
        public virtual DbSet<InvoiceComparison> InvoiceComparisons { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual DbSet<InvoiceDownload> InvoiceDownloads { get; set; }
        public virtual DbSet<InvoiceFilename> InvoiceFilenames { get; set; }
        public virtual DbSet<InvoiceRelated> InvoiceRelateds { get; set; }
        public virtual DbSet<InvoiceSaleOrder> InvoiceSaleOrders { get; set; }
        public virtual DbSet<InvoiceSerie> InvoiceSeries { get; set; }
        public virtual DbSet<InvoiceSerieType> InvoiceSerieTypes { get; set; }
        public virtual DbSet<InvoiceStamped> InvoiceStampeds { get; set; }
        public virtual DbSet<Island> Islands { get; set; }
        public virtual DbSet<JsonClaveInstalacion> JsonClaveInstalacions { get; set; }
        public virtual DbSet<JsonClavePermiso> JsonClavePermisos { get; set; }
        public virtual DbSet<JsonClaveProducto> JsonClaveProductos { get; set; }
        public virtual DbSet<JsonClaveTanque> JsonClaveTanques { get; set; }
        public virtual DbSet<JsonClaveUnidadMedidum> JsonClaveUnidadMedida { get; set; }
        public virtual DbSet<JsonSubclaveProducto> JsonSubclaveProductos { get; set; }
        public virtual DbSet<JsonTipoComplemento> JsonTipoComplementos { get; set; }
        public virtual DbSet<JsonTipoComposicion> JsonTipoComposicions { get; set; }
        public virtual DbSet<JsonTipoDistribucion> JsonTipoDistribucions { get; set; }
        public virtual DbSet<JsonTipoDucto> JsonTipoDuctos { get; set; }
        public virtual DbSet<JsonTipoMedioAlmacenamiento> JsonTipoMedioAlmacenamientos { get; set; }
        public virtual DbSet<JsonTipoRegistro> JsonTipoRegistros { get; set; }
        public virtual DbSet<JsonTipoReporte> JsonTipoReportes { get; set; }
        public virtual DbSet<JsonTipoSistemaMedicion> JsonTipoSistemaMedicions { get; set; }
        public virtual DbSet<JsonTipoTanque> JsonTipoTanques { get; set; }
        public virtual DbSet<LastInventory> LastInventories { get; set; }
        public virtual DbSet<LastInventoryIn> LastInventoryIns { get; set; }
        public virtual DbSet<LastSaleHose> LastSaleHoses { get; set; }
        public virtual DbSet<LastShift> LastShifts { get; set; }
        public virtual DbSet<License> Licenses { get; set; }
        public virtual DbSet<LoadPosition> LoadPositions { get; set; }
        public virtual DbSet<LoadPositionResponse> LoadPositionResponses { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Monitor> Monitors { get; set; }
        public virtual DbSet<MonthlySummary> MonthlySummaries { get; set; }
        public virtual DbSet<MovementType> MovementTypes { get; set; }
        public virtual DbSet<Netgroup> Netgroups { get; set; }
        public virtual DbSet<NetgroupBalance> NetgroupBalances { get; set; }
        public virtual DbSet<NetgroupPrice> NetgroupPrices { get; set; }
        public virtual DbSet<NetgroupReward> NetgroupRewards { get; set; }
        public virtual DbSet<NetgroupStore> NetgroupStores { get; set; }
        public virtual DbSet<NetgroupUser> NetgroupUsers { get; set; }
        public virtual DbSet<Odr> Odrs { get; set; }
        public virtual DbSet<OdrStore> OdrStores { get; set; }
        public virtual DbSet<Operator> Operators { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<PagePerUserType> PagePerUserTypes { get; set; }
        public virtual DbSet<PaymentSubMode> PaymentSubModes { get; set; }
        public virtual DbSet<PeriodType> PeriodTypes { get; set; }
        public virtual DbSet<PetitionCustom> PetitionCustoms { get; set; }
        public virtual DbSet<PointSale> PointSales { get; set; }
        public virtual DbSet<Port> Ports { get; set; }
        public virtual DbSet<PortResponse> PortResponses { get; set; }
        public virtual DbSet<PortType> PortTypes { get; set; }
        public virtual DbSet<Printer> Printers { get; set; }
        public virtual DbSet<PrinterBrand> PrinterBrands { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductComposition> ProductCompositions { get; set; }
        public virtual DbSet<ProductPrice> ProductPrices { get; set; }
        public virtual DbSet<ProductSat> ProductSats { get; set; }
        public virtual DbSet<ProductStore> ProductStores { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportCode> ReportCodes { get; set; }
        public virtual DbSet<ReportInput> ReportInputs { get; set; }
        public virtual DbSet<ReportModule> ReportModules { get; set; }
        public virtual DbSet<ReportModuleDetail> ReportModuleDetails { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<SaleOrder> SaleOrders { get; set; }
        public virtual DbSet<SaleOrderPayment> SaleOrderPayments { get; set; }
        public virtual DbSet<SaleOrderPhoto> SaleOrderPhotos { get; set; }
        public virtual DbSet<SaleSuborder> SaleSuborders { get; set; }
        public virtual DbSet<SatClaveProductoServicio> SatClaveProductoServicios { get; set; }
        public virtual DbSet<SatClaveUnidad> SatClaveUnidads { get; set; }
        public virtual DbSet<SatCodigoPostal> SatCodigoPostals { get; set; }
        public virtual DbSet<SatErrorCancelacion> SatErrorCancelacions { get; set; }
        public virtual DbSet<SatEstado> SatEstados { get; set; }
        public virtual DbSet<SatFormaPago> SatFormaPagos { get; set; }
        public virtual DbSet<SatFormaSubpago> SatFormaSubpagos { get; set; }
        public virtual DbSet<SatLocalidad> SatLocalidads { get; set; }
        public virtual DbSet<SatMese> SatMeses { get; set; }
        public virtual DbSet<SatMetodoPago> SatMetodoPagos { get; set; }
        public virtual DbSet<SatMotivoCancelacion> SatMotivoCancelacions { get; set; }
        public virtual DbSet<SatMunicipio> SatMunicipios { get; set; }
        public virtual DbSet<SatPai> SatPais { get; set; }
        public virtual DbSet<SatPeriodicidad> SatPeriodicidads { get; set; }
        public virtual DbSet<SatRegimenFiscal> SatRegimenFiscals { get; set; }
        public virtual DbSet<SatRegimenfiscalUsocfdi> SatRegimenfiscalUsocfdis { get; set; }
        public virtual DbSet<SatTipoComprobante> SatTipoComprobantes { get; set; }
        public virtual DbSet<SatTipoRelacion> SatTipoRelacions { get; set; }
        public virtual DbSet<SatUsoCfdi> SatUsoCfdis { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<SettingModule> SettingModules { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<ShiftDeposit> ShiftDeposits { get; set; }
        public virtual DbSet<ShiftHead> ShiftHeads { get; set; }
        public virtual DbSet<StatusDispenser> StatusDispensers { get; set; }
        public virtual DbSet<StatusIsland> StatusIslands { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<StoreAddress> StoreAddresses { get; set; }
        public virtual DbSet<StoreHouse> StoreHouses { get; set; }
        public virtual DbSet<StoreHouseDetail> StoreHouseDetails { get; set; }
        public virtual DbSet<StoreHouseMovement> StoreHouseMovements { get; set; }
        public virtual DbSet<StoreHouseMovementDetail> StoreHouseMovementDetails { get; set; }
        public virtual DbSet<StoreInvoiceSerie> StoreInvoiceSeries { get; set; }
        public virtual DbSet<StoreNetwork> StoreNetworks { get; set; }
        public virtual DbSet<StoreNetworkDetail> StoreNetworkDetails { get; set; }
        public virtual DbSet<StoreSat> StoreSats { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SupplierAddress> SupplierAddresses { get; set; }
        public virtual DbSet<SupplierFuel> SupplierFuels { get; set; }
        public virtual DbSet<SupplierTransport> SupplierTransports { get; set; }
        public virtual DbSet<SupplierTransportRegister> SupplierTransportRegisters { get; set; }
        public virtual DbSet<Tank> Tanks { get; set; }
        public virtual DbSet<TankBrand> TankBrands { get; set; }
        public virtual DbSet<TankCalibrationPoint> TankCalibrationPoints { get; set; }
        public virtual DbSet<TankInprogress> TankInprogresses { get; set; }
        public virtual DbSet<TankShape> TankShapes { get; set; }
        public virtual DbSet<TankType> TankTypes { get; set; }
        public virtual DbSet<TaskType> TaskTypes { get; set; }
        public virtual DbSet<TransportMediumnCustom> TransportMediumnCustoms { get; set; }
        public virtual DbSet<TypeMovement> TypeMovements { get; set; }
        public virtual DbSet<UserDateCreate> UserDateCreates { get; set; }
        public virtual DbSet<UserDateCreate1> UserDateCreates1 { get; set; }
        public virtual DbSet<UserStore> UserStores { get; set; }
        public virtual DbSet<ValidateType> ValidateTypes { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Version> Versions { get; set; }
        public virtual DbSet<Volumetric> Volumetrics { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("server=www.controlnet.com.mx,14334\\SQLEXPRESS;Database=CnetCore;User ID=AdminCnet;Password=Control2207074rd;Trusted_Connection=false;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseCollation("Modern_Spanish_CI_AS");

            modelBuilder.Entity<AuthorizationSet>(entity =>
            {
                entity.HasKey(e => e.AuthorizationSetIdx)
                    .HasName("PK_Set_Autorizacion");

                entity.ToTable("authorization_set");

                entity.HasIndex(e => e.AuthorizationSetId, "IX_Set_Autorizacion")
                    .IsUnique();

                entity.Property(e => e.AuthorizationSetIdx).HasColumnName("authorization_set_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AuthorizationSetId).HasColumnName("authorization_set_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Definition).HasColumnName("definition");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.HoseId).HasColumnName("hose_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Network).HasColumnName("network");

                entity.Property(e => e.OperatorId).HasColumnName("operator_id");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("price");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("quantity");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TagUse).HasColumnName("tag_use");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Updated).HasColumnName("updated");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");
            });

            modelBuilder.Entity<AutoTanque>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEmpresa, e.NumeroEstacion, e.NumeroAutoTanque })
                    .HasName("PK__AutoTanq__3CE513749F89317A");

                entity.ToTable("AutoTanque");

                entity.Property(e => e.NumeroEmpresa).HasMaxLength(3);

                entity.Property(e => e.NumeroEstacion).HasMaxLength(4);

                entity.Property(e => e.AutoTanqueId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("AutoTanqueID");

                entity.Property(e => e.CapacidadFondaje).HasColumnType("numeric(15, 4)");

                entity.Property(e => e.CapacidadGasTalon).HasColumnType("numeric(15, 4)");

                entity.Property(e => e.CapacidadOperativa).HasColumnType("numeric(15, 4)");

                entity.Property(e => e.CapacidadTotal).HasColumnType("numeric(15, 4)");

                entity.Property(e => e.CapacidadUtil).HasColumnType("numeric(15, 4)");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.EstadoTanque)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IncertidumbreMedicionSistMedicion).HasColumnType("numeric(15, 3)");

                entity.Property(e => e.LocalizOdescripSistMedicion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("LocalizODescripSistMedicion");

                entity.Property(e => e.NumeroProducto)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TipoAutoTanque)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.TipoMedioAlmacenamiento)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.TipoSistMedicion)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.VigenciaCalibracionSistMedicion).HasColumnType("datetime");

                entity.Property(e => e.VolumenMinimoOperacion).HasColumnType("numeric(15, 4)");
            });

            modelBuilder.Entity<BankCard>(entity =>
            {
                entity.HasKey(e => e.BankCardIdx);

                entity.ToTable("bank_card");

                entity.Property(e => e.BankCardIdx).HasColumnName("bank_card_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BankCardId).HasColumnName("bank_card_id");

                entity.Property(e => e.BankCardNumber).HasColumnName("bank_card_number");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Binnacle>(entity =>
            {
                entity.HasKey(e => e.BinnacleIdx)
                    .HasName("PK_Bitacora");

                entity.ToTable("binnacle");

                entity.Property(e => e.BinnacleIdx).HasColumnName("binnacle_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BinnacleId).HasColumnName("binnacle_id");

                entity.Property(e => e.BinnacleTypeId).HasColumnName("binnacle_type_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.EndValue)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("end_value");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ip_address");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MacAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mac_address");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Response)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("response");

                entity.Property(e => e.StartValue)
                    .HasColumnType("text")
                    .HasColumnName("start_value");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .HasColumnName("user_id");

                entity.Property(e => e.ValueName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("value_name");
            });

            modelBuilder.Entity<BinnacleLog>(entity =>
            {
                entity.HasKey(e => e.BinnacleLogIdx)
                    .HasName("PK_Binnacle_log");

                entity.ToTable("binnacle_log");

                entity.Property(e => e.BinnacleLogIdx).HasColumnName("binnacle_log_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BinnacleLogId).HasColumnName("binnacle_log_id");

                entity.Property(e => e.BinnacleTypeId).HasColumnName("binnacle_type_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.KeyValue)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("key_value");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Response)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("response");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UserId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<BinnacleService>(entity =>
            {
                entity.HasKey(e => e.BinnacleServiceIdx);

                entity.ToTable("binnacle_service");

                entity.Property(e => e.BinnacleServiceIdx).HasColumnName("binnacle_service_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BinnacleTypeId).HasColumnName("binnacle_type_id");

                entity.Property(e => e.DataInput)
                    .HasColumnType("text")
                    .HasColumnName("data_input");

                entity.Property(e => e.DataResponse)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("data_response");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Response)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("response");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<BinnacleType>(entity =>
            {
                entity.HasKey(e => e.BinnacleTypeIdx);

                entity.ToTable("binnacle_type");

                entity.HasIndex(e => e.BinnacleTypeId, "IX_binnacle_type_id")
                    .IsUnique();

                entity.Property(e => e.BinnacleTypeIdx).HasColumnName("binnacle_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BinnacleTypeId).HasColumnName("binnacle_type_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.IsVolumetric).HasColumnName("is_volumetric");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Button>(entity =>
            {
                entity.HasKey(e => e.ButtonIdx)
                    .HasName("PK_Button");

                entity.ToTable("button");

                entity.Property(e => e.ButtonIdx).HasColumnName("button_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Id)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<ButtonPerUser>(entity =>
            {
                entity.HasKey(e => e.ButtonPerUserIdx)
                    .HasName("PK_PaginaTipoUsuButton");

                entity.ToTable("button_per_user");

                entity.Property(e => e.ButtonPerUserIdx).HasColumnName("button_per_user_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ButtonId).HasColumnName("button_id");

                entity.Property(e => e.ButtonPerUserId).HasColumnName("button_per_user_id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PageId).HasColumnName("page_id");

                entity.Property(e => e.Updated).HasColumnName("updated");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.CardIdx)
                    .HasName("PK_card_idx");

                entity.ToTable("card");

                entity.HasIndex(e => e.CardId, "IX_card_id")
                    .IsUnique();

                entity.Property(e => e.CardIdx).HasColumnName("card_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AskVehicle).HasColumnName("ask_vehicle");

                entity.Property(e => e.CardId)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("card_id");

                entity.Property(e => e.CardTypeId).HasColumnName("card_type_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EnableAuthorize).HasColumnName("enable_authorize");

                entity.Property(e => e.IdentifierId).HasColumnName("identifier_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.CardType)
                    .WithMany(p => p.Cards)
                    .HasPrincipalKey(p => p.CardTypeId)
                    .HasForeignKey(d => d.CardTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_card_card_type");
            });

            modelBuilder.Entity<CardType>(entity =>
            {
                entity.HasKey(e => e.CardTypeIdx)
                    .HasName("PK_card_type_idx");

                entity.ToTable("card_type");

                entity.HasIndex(e => e.CardTypeId, "IX_card_type")
                    .IsUnique();

                entity.Property(e => e.CardTypeIdx).HasColumnName("card_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CardTypeId)
                    .IsRequired()
                    .HasColumnName("card_type_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<ClienteProveedorCvol>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.Tipo, e.NumeroRegistro })
                    .HasName("PK__ClienteP__512EA4A086CAB0C2");

                entity.ToTable("ClienteProveedorCVol");

                entity.Property(e => e.NumeroEstacion).HasMaxLength(4);

                entity.Property(e => e.Tipo).HasMaxLength(15);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PermisoCre)
                    .HasMaxLength(30)
                    .HasColumnName("PermisoCRE");

                entity.Property(e => e.RegistroId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RegistroID");

                entity.Property(e => e.Rfc)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("RFC");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyIdx);

                entity.ToTable("company");

                entity.HasIndex(e => e.CompanyId, "IX_company")
                    .IsUnique();

                entity.Property(e => e.CompanyIdx).HasColumnName("company_idx");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Locked)
                    .HasColumnName("locked")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Rfc)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rfc");

                entity.Property(e => e.SatRegimenFiscalId)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("sat_regimen_fiscal_id");

                entity.Property(e => e.TradeName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("trade_name");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<CompanyAddress>(entity =>
            {
                entity.HasKey(e => e.CompanyAddressIdx);

                entity.ToTable("company_address");

                entity.HasIndex(e => new { e.CompanyId, e.CompanyAddressIdi }, "IX_company_address_compound")
                    .IsUnique();

                entity.Property(e => e.CompanyAddressIdx).HasColumnName("company_address_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Colony)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("colony");

                entity.Property(e => e.CompanyAddressIdi).HasColumnName("company_address_idi");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.InsideNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("inside_number");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.OutsideNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("outside_number");

                entity.Property(e => e.SatCodigoPostalId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_codigo_postal_id");

                entity.Property(e => e.SatEstadoId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_estado_id");

                entity.Property(e => e.SatLocalidadId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("sat_localidad_id");

                entity.Property(e => e.SatMunicipioId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_municipio_id");

                entity.Property(e => e.SatPaisId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_pais_id");

                entity.Property(e => e.Street)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("street");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyAddresses)
                    .HasPrincipalKey(p => p.CompanyId)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_company_address_company1");
            });

            modelBuilder.Entity<CompanyFiel>(entity =>
            {
                entity.HasKey(e => e.CompanyFielIdx);

                entity.ToTable("company_fiel");

                entity.HasIndex(e => new { e.CompanyId, e.FielTypeId }, "IX_company_fiel_compound")
                    .IsUnique();

                entity.Property(e => e.CompanyFielIdx).HasColumnName("company_fiel_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CertificateNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("certificate_number");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.FielTypeId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fiel_type_id");

                entity.Property(e => e.FileCertificate)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("file_certificate");

                entity.Property(e => e.FilePrivateKey)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("file_private_key");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PasswordPrivateKey)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("password_private_key");

                entity.Property(e => e.PemPrivateKey)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("pem_private_key");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.XmlPrivateKey)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("xml_private_key");
            });

            modelBuilder.Entity<ComposicionGasNaturalOcondensado>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroProducto, e.TipoCompuesto })
                    .HasName("PK__Composic__1FDB0471EA2C3471");

                entity.ToTable("ComposicionGasNaturalOCondensado");

                entity.Property(e => e.NumeroEstacion).HasMaxLength(4);

                entity.Property(e => e.NumeroProducto).HasMaxLength(10);

                entity.Property(e => e.TipoCompuesto).HasMaxLength(5);

                entity.Property(e => e.ComposicionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ComposicionID");

                entity.Property(e => e.FraccionMolar).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.PoderCalorifico).HasColumnType("decimal(12, 3)");
            });

            modelBuilder.Entity<CompraComplementoMesSat>(entity =>
            {
                entity.HasKey(e => e.IdxCompraComplementoSat);

                entity.ToTable("CompraComplementoMesSAT");

                entity.HasIndex(e => e.IdxCompraComplementoSat, "IX_CompraComplementoMesSAT")
                    .IsUnique();

                entity.Property(e => e.IdxCompraComplementoSat).HasColumnName("idxCompraComplementoSAT");

                entity.Property(e => e.AclaracionesSat)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("aclaracionesSAT");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Cfdi)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cfdi");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.IdComplementoSat).HasColumnName("idComplementoSAT");

                entity.Property(e => e.InventoryIn).HasColumnName("inventory_in");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PrecioVenta)
                    .HasColumnType("decimal(14, 3)")
                    .HasColumnName("precioVenta");

                entity.Property(e => e.PrecioVentaPublico)
                    .HasColumnType("decimal(14, 3)")
                    .HasColumnName("precioVentaPublico");

                entity.Property(e => e.TipoCfdi)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("tipoCfdi");

                entity.Property(e => e.TipoComplementoSat)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("tipoComplementoSAT");

                entity.Property(e => e.UnidadMedidaSat)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("unidadMedidaSAT");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<ConfigGral>(entity =>
            {
                entity.HasKey(e => new { e.StoreId, e.ModuleId, e.RowName });

                entity.ToTable("Config_Gral");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.RowName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.IdConfigGral).ValueGeneratedOnAdd();

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated).HasColumnName("updated");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ConfigView>(entity =>
            {
                entity.HasKey(e => e.ConfigViewIdx)
                    .HasName("PK_views");

                entity.ToTable("config_view");

                entity.HasIndex(e => e.Name, "IX_config_view")
                    .IsUnique();

                entity.Property(e => e.ConfigViewIdx).HasColumnName("config_view_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ConfigViewId).HasColumnName("config_view_id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.FieldKeyOne)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("field_key_one");

                entity.Property(e => e.FieldPairKey)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("field_pair_key");

                entity.Property(e => e.FieldPairValue)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("field_pair_value");

                entity.Property(e => e.Fields)
                    .HasColumnType("text")
                    .HasColumnName("fields");

                entity.Property(e => e.FieldsSave)
                    .HasColumnType("text")
                    .HasColumnName("fields_save");

                entity.Property(e => e.FrontendEnableActive).HasColumnName("frontend_enable_active");

                entity.Property(e => e.FrontendEnableBlock).HasColumnName("frontend_enable_block");

                entity.Property(e => e.FrontendEnableDeleted).HasColumnName("frontend_enable_deleted");

                entity.Property(e => e.FrontendEnableEdit).HasColumnName("frontend_enable_edit");

                entity.Property(e => e.FrontendEnableNext).HasColumnName("frontend_enable_next");

                entity.Property(e => e.FrontendEnableSave).HasColumnName("frontend_enable_save");

                entity.Property(e => e.FrontendFrameType).HasColumnName("frontend_frame_type");

                entity.Property(e => e.FrontendName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("frontend_name");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.QueryData)
                    .HasColumnType("text")
                    .HasColumnName("query_data");

                entity.Property(e => e.QueryDataCondition)
                    .HasColumnType("text")
                    .HasColumnName("query_data_condition");

                entity.Property(e => e.QueryResume)
                    .HasColumnType("text")
                    .HasColumnName("query_resume");

                entity.Property(e => e.TableKey)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("table_key");

                entity.Property(e => e.TableName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("table_name");

                entity.Property(e => e.TableSave)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("table_save");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<ConfigViewFrame>(entity =>
            {
                entity.HasKey(e => new { e.ConfigViewId, e.PositionFrameNumber })
                    .HasName("PK_config_view_frontend_frame");

                entity.ToTable("config_view_frame");

                entity.Property(e => e.ConfigViewId).HasColumnName("config_view_id");

                entity.Property(e => e.PositionFrameNumber).HasColumnName("position_frame_number");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ConfigViewFrameId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("config_view_frame_id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.FrameName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("frame_name");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<ConfigViewItem>(entity =>
            {
                entity.HasKey(e => new { e.ConfigViewId, e.Name })
                    .HasName("PK_config_view_frontend");

                entity.ToTable("config_view_item");

                entity.Property(e => e.ConfigViewId).HasColumnName("config_view_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AliasForm)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("alias_form");

                entity.Property(e => e.AliasList)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("alias_list");

                entity.Property(e => e.ConfigViewItemId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("config_view_item_id");

                entity.Property(e => e.DataType)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("data_type");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.DefaultValue)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("default_value");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EnableShowForm).HasColumnName("enable_show_form");

                entity.Property(e => e.EnableShowList).HasColumnName("enable_show_list");

                entity.Property(e => e.FieldType)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("field_type");

                entity.Property(e => e.FilterPositionFrameNumber).HasColumnName("filter_position_frame_number");

                entity.Property(e => e.FilterPositionSize).HasColumnName("filter_position_size");

                entity.Property(e => e.FilterPositionX).HasColumnName("filter_position_x");

                entity.Property(e => e.FilterPositionY).HasColumnName("filter_position_y");

                entity.Property(e => e.IsFieldInput).HasColumnName("is_field_input");

                entity.Property(e => e.IsFilter).HasColumnName("is_filter");

                entity.Property(e => e.IsKey).HasColumnName("is_key");

                entity.Property(e => e.IsOrder).HasColumnName("is_order");

                entity.Property(e => e.IsRequired).HasColumnName("is_required");

                entity.Property(e => e.IsVisible).HasColumnName("is_visible");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MethodListName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("method_list_name");

                entity.Property(e => e.PositionFrameNumber).HasColumnName("position_frame_number");

                entity.Property(e => e.PositionIndexList).HasColumnName("position_index_list");

                entity.Property(e => e.PositionSize).HasColumnName("position_size");

                entity.Property(e => e.PositionX).HasColumnName("position_x");

                entity.Property(e => e.PositionY).HasColumnName("position_y");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<ConfigViewRelated>(entity =>
            {
                entity.HasKey(e => new { e.ConfigViewId, e.ConfigViewIdRelated })
                    .HasName("PK_config_view_item");

                entity.ToTable("config_view_related");

                entity.Property(e => e.ConfigViewId).HasColumnName("config_view_id");

                entity.Property(e => e.ConfigViewIdRelated).HasColumnName("config_view_id_related");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ConfigViewRelatedId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("config_view_related_id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.IdFieldNameRelation)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("id_field_name_relation");

                entity.Property(e => e.IsChild).HasColumnName("is_child");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.ReturnOrder).HasColumnName("return_order");

                entity.Property(e => e.SaveOrder).HasColumnName("save_order");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerIdx);

                entity.ToTable("customer");

                entity.HasIndex(e => e.CustomerId, "IX_customer_id")
                    .IsUnique();

                entity.HasIndex(e => e.CustomerNumber, "IX_customer_number")
                    .IsUnique();

                entity.Property(e => e.CustomerIdx).HasColumnName("customer_idx");

                entity.Property(e => e.AbbreviatedName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("abbreviated_name");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Cellular)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cellular");

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contact_person")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Curp)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("curp");

                entity.Property(e => e.CustomerAddressIdi).HasColumnName("customer_address_idi");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.CustomerNumber).HasColumnName("customer_number");

                entity.Property(e => e.CustomerPermission)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("customer_permission")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CustomerTypeId).HasColumnName("customer_type_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MotherLastname)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("mother_lastname");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Rfc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rfc");

                entity.Property(e => e.SatConsignmentSale)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("sat_consignment_sale")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.SatFormaPagoId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_forma_pago_id");

                entity.Property(e => e.SatRegimenFiscalId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sat_regimen_fiscal_id");

                entity.Property(e => e.SatUsoCfdiId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sat_uso_cfdi_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.Customers)
                    .HasPrincipalKey(p => p.CustomerTypeId)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .HasConstraintName("FK_customer_customer_type");

                entity.HasOne(d => d.SatFormaPago)
                    .WithMany(p => p.Customers)
                    .HasPrincipalKey(p => p.SatFormaPagoId)
                    .HasForeignKey(d => d.SatFormaPagoId)
                    .HasConstraintName("FK_customer_sat_forma_pago");

                entity.HasOne(d => d.SatRegimenFiscal)
                    .WithMany(p => p.Customers)
                    .HasPrincipalKey(p => p.SatRegimenFiscalId)
                    .HasForeignKey(d => d.SatRegimenFiscalId)
                    .HasConstraintName("FK_customer_sat_regimen_fiscal");

                entity.HasOne(d => d.SatUsoCfdi)
                    .WithMany(p => p.Customers)
                    .HasPrincipalKey(p => p.SatUsoCfdiId)
                    .HasForeignKey(d => d.SatUsoCfdiId)
                    .HasConstraintName("FK_customer_sat_uso_cfdi");
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasKey(e => e.CustomerAddressIdx);

                entity.ToTable("customer_address");

                entity.Property(e => e.CustomerAddressIdx).HasColumnName("customer_address_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Colony)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("colony");

                entity.Property(e => e.CustomerAddressIdi).HasColumnName("customer_address_idi");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.InsideNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("inside_number");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.OutsideNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("outside_number");

                entity.Property(e => e.SatCodigoPostalId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_codigo_postal_id");

                entity.Property(e => e.SatEstadoId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_estado_id");

                entity.Property(e => e.SatLocalidadId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_localidad_id");

                entity.Property(e => e.SatMunicipioId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_municipio_id");

                entity.Property(e => e.SatPaisId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_pais_id");

                entity.Property(e => e.Street)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("street");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerAddresses)
                    .HasPrincipalKey(p => p.CustomerId)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_customer_address_customer");
            });

            modelBuilder.Entity<CustomerControl>(entity =>
            {
                entity.HasKey(e => e.CustomerControlIdx)
                    .HasName("PK_customer_control_idx");

                entity.ToTable("customer_control");

                entity.HasIndex(e => e.CustomerControlId, "IX_customer_control_id")
                    .IsUnique();

                entity.Property(e => e.CustomerControlIdx).HasColumnName("customer_control_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CardOperator1Id)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("card_operator1_id");

                entity.Property(e => e.CardOperator2Id)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("card_operator2_id");

                entity.Property(e => e.CardVehicleId)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("card_vehicle_id");

                entity.Property(e => e.CustomerControlId).HasColumnName("customer_control_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.OdometerEnd).HasColumnName("odometer_end");

                entity.Property(e => e.OdometerStart).HasColumnName("odometer_start");

                entity.Property(e => e.OdrId).HasColumnName("odr_id");

                entity.Property(e => e.Operator1Id).HasColumnName("operator1_id");

                entity.Property(e => e.Operator2Id).HasColumnName("operator2_id");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");
            });

            modelBuilder.Entity<CustomerLimit>(entity =>
            {
                entity.HasKey(e => e.CustomerLimitIdx);

                entity.ToTable("customer_limit");

                entity.Property(e => e.CustomerLimitIdx).HasColumnName("customer_limit_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AllowDay1).HasColumnName("allow_day_1");

                entity.Property(e => e.AllowDay2).HasColumnName("allow_day_2");

                entity.Property(e => e.AllowDay3).HasColumnName("allow_day_3");

                entity.Property(e => e.AllowDay4).HasColumnName("allow_day_4");

                entity.Property(e => e.AllowDay5).HasColumnName("allow_day_5");

                entity.Property(e => e.AllowDay6).HasColumnName("allow_day_6");

                entity.Property(e => e.AllowDay7).HasColumnName("allow_day_7");

                entity.Property(e => e.AmountCreditLimit)
                    .HasColumnType("decimal(16, 1)")
                    .HasColumnName("amount_credit_limit");

                entity.Property(e => e.CreditDays).HasColumnName("credit_days");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.InvoiceDay1).HasColumnName("invoice_day_1");

                entity.Property(e => e.InvoiceDay2).HasColumnName("invoice_day_2");

                entity.Property(e => e.InvoiceDay3).HasColumnName("invoice_day_3");

                entity.Property(e => e.InvoiceDay4).HasColumnName("invoice_day_4");

                entity.Property(e => e.InvoiceDay5).HasColumnName("invoice_day_5");

                entity.Property(e => e.InvoiceDay6).HasColumnName("invoice_day_6");

                entity.Property(e => e.InvoiceDay7).HasColumnName("invoice_day_7");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PaymentDay1).HasColumnName("payment_day_1");

                entity.Property(e => e.PaymentDay2).HasColumnName("payment_day_2");

                entity.Property(e => e.PaymentDay3).HasColumnName("payment_day_3");

                entity.Property(e => e.PaymentDay4).HasColumnName("payment_day_4");

                entity.Property(e => e.PaymentDay5).HasColumnName("payment_day_5");

                entity.Property(e => e.PaymentDay6).HasColumnName("payment_day_6");

                entity.Property(e => e.PaymentDay7).HasColumnName("payment_day_7");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.HasKey(e => e.CustomerTypeIdx);

                entity.ToTable("customer_type");

                entity.HasIndex(e => e.CustomerTypeId, "IX_customer_type")
                    .IsUnique();

                entity.Property(e => e.CustomerTypeIdx).HasColumnName("customer_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CustomerTypeId).HasColumnName("customer_type_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<DailySummary>(entity =>
            {
                entity.HasKey(e => e.DailySummaryIdx)
                    .HasName("Pk_daily_summary_idx");

                entity.ToTable("daily_summary");

                entity.HasIndex(e => new { e.StoreId, e.Date, e.ProductId }, "IX_daily_summary_compound")
                    .IsUnique();

                entity.Property(e => e.DailySummaryIdx).HasColumnName("daily_summary_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EndInventoryQuantity)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("end_inventory_quantity");

                entity.Property(e => e.InventoryDifference)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("inventory_difference");

                entity.Property(e => e.InventoryDifferencePercentage)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("inventory_difference_percentage");

                entity.Property(e => e.InventoryInQuantity)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("inventory_in_quantity");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("price");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.SaleAmount)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("sale_amount");

                entity.Property(e => e.SaleQuantity)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("sale_quantity");

                entity.Property(e => e.SaleSample)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("sale_sample");

                entity.Property(e => e.StartInventoryQuantity)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("start_inventory_quantity");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TheoryInventoryQuantity)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("theory_inventory_quantity");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DailySummaries)
                    .HasPrincipalKey(p => p.ProductId)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_daily_summary_product");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.DeviceIdx);

                entity.ToTable("device");

                entity.HasIndex(e => e.DeviceId, "IX_device_id")
                    .IsUnique();

                entity.Property(e => e.DeviceIdx).HasColumnName("device_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.DeviceId).HasColumnName("device_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<DeviceLog>(entity =>
            {
                entity.HasKey(e => e.DeviceLogIdx)
                    .HasName("PK_Log_Dispositivo");

                entity.ToTable("device_log");

                entity.Property(e => e.DeviceLogIdx).HasColumnName("device_log_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("device_id");

                entity.Property(e => e.DeviceLogId).HasColumnName("device_log_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Message)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("message");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Percentage)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("percentage");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<Dispensary>(entity =>
            {
                entity.HasKey(e => e.DispensaryIdx)
                    .HasName("PK_dispensary_idx");

                entity.ToTable("dispensary");

                entity.HasIndex(e => new { e.StoreId, e.DispensaryIdi }, "IX_dispensary_compound")
                    .IsUnique();

                entity.HasIndex(e => e.UniqueId, "IX_dispensary_unique_id")
                    .IsUnique();

                entity.Property(e => e.DispensaryIdx).HasColumnName("dispensary_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.DispensaryBrandId).HasColumnName("dispensary_brand_id");

                entity.Property(e => e.DispensaryIdi).HasColumnName("dispensary_idi");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.SatCalibrationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("sat_calibration_date");

                entity.Property(e => e.SatMeasurementDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("sat_measurement_description");

                entity.Property(e => e.SatMeasurementPercentageUncertainty)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("sat_measurement_percentage_uncertainty");

                entity.Property(e => e.SatMeasurementType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_measurement_type");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Subtype).HasColumnName("subtype");

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("unique_id")
                    .HasDefaultValueSql("(CONVERT([varchar](80),newid()))");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.DispensaryBrand)
                    .WithMany(p => p.Dispensaries)
                    .HasPrincipalKey(p => p.DispensaryBrandId)
                    .HasForeignKey(d => d.DispensaryBrandId)
                    .HasConstraintName("FK_dispensary_brandDispensary");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Dispensaries)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dispensary_store");
            });

            modelBuilder.Entity<DispensaryBrand>(entity =>
            {
                entity.HasKey(e => e.DispensaryBrandIdx)
                    .HasName("PK_brandDispensary_1");

                entity.ToTable("dispensary_brand");

                entity.HasIndex(e => e.DispensaryBrandId, "IX_brandDispensary")
                    .IsUnique();

                entity.Property(e => e.DispensaryBrandIdx).HasColumnName("dispensary_brand_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DispensaryBrandId).HasColumnName("dispensary_brand_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Ducto>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroDucto })
                    .HasName("PK__Ducto__84257FF5B6BE09EF");

                entity.ToTable("Ducto");

                entity.Property(e => e.NumeroEstacion)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Diametro).HasColumnType("numeric(6, 3)");

                entity.Property(e => e.DuctoId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DuctoID");

                entity.Property(e => e.NumeroProducto)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDucto)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UnidadMedida)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Volumen).HasColumnType("numeric(9, 3)");
            });

            modelBuilder.Entity<DuctoEntrega>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroEntrega })
                    .HasName("PK__DuctoEnt__97C42B5F7EDF5E87");

                entity.ToTable("DuctoEntrega");

                entity.Property(e => e.NumeroEstacion)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.EntregaId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("EntregaID");

                entity.Property(e => e.FechaYhoraFinalEntrega)
                    .HasColumnType("datetime")
                    .HasColumnName("FechaYHoraFinalEntrega");

                entity.Property(e => e.FechaYhoraInicialEntrega)
                    .HasColumnType("datetime")
                    .HasColumnName("FechaYHoraInicialEntrega");

                entity.Property(e => e.Importe).HasColumnType("numeric(9, 2)");

                entity.Property(e => e.NumeroProducto)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PoderCalorifico)
                    .HasColumnType("numeric(15, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Precio).HasColumnType("numeric(8, 2)");

                entity.Property(e => e.PresionAbsoluta).HasColumnType("numeric(6, 3)");

                entity.Property(e => e.Temperatura).HasColumnType("numeric(6, 3)");

                entity.Property(e => e.TipoDistribucion)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.VolumenEntregado).HasColumnType("numeric(12, 3)");

                entity.Property(e => e.VolumenFinal).HasColumnType("numeric(12, 3)");

                entity.Property(e => e.VolumenPuntoSalida).HasColumnType("numeric(12, 3)");
            });

            modelBuilder.Entity<DuctoEntregaCfdi>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroEntrega })
                    .HasName("PK__DuctoEnt__97C42B5F4F6AACFC");

                entity.ToTable("DuctoEntregaCFDI");

                entity.Property(e => e.NumeroEstacion)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Cfdi)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("CFDI");

                entity.Property(e => e.ClienteRfc)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ClienteRFC");

                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.Property(e => e.NombreCliente)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NumeroPermisoCre)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("NumeroPermisoCRE");

                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PrecioVtaPublico).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.RegistroId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RegistroID");

                entity.Property(e => e.Serie)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TipoCfdi)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("TipoCFDI");

                entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Volumen).HasColumnType("decimal(12, 3)");
            });

            modelBuilder.Entity<DuctoEntregaPedimento>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroEntrega })
                    .HasName("PK__DuctoEnt__97C42B5F79758C44");

                entity.ToTable("DuctoEntregaPedimento");

                entity.Property(e => e.NumeroEstacion).HasMaxLength(4);

                entity.Property(e => e.ClavePedimento)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.ClavePermisoImportOexport)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ClavePermisoImportOExport");

                entity.Property(e => e.Incoterms)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.MedioIngresoOsalida).HasColumnName("MedioIngresoOSalida");

                entity.Property(e => e.Pais)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.PedimentoId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PedimentoID");

                entity.Property(e => e.PrecioDeImportOexport)
                    .HasColumnType("numeric(12, 3)")
                    .HasColumnName("PrecioDeImportOExport");

                entity.Property(e => e.PuntoInternacionOextracccion).HasColumnName("PuntoInternacionOExtracccion");

                entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Volumen).HasColumnType("numeric(12, 3)");
            });

            modelBuilder.Entity<DuctoEntregaTransporte>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroEntrega })
                    .HasName("PK__DuctoEnt__97C42B5F67A17A18");

                entity.ToTable("DuctoEntregaTransporte");

                entity.Property(e => e.NumeroEstacion)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CargoCapacidadTrans).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.CargoUsoTrans).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.CargoVolumenTrans).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.ClaveVehiculo)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.ContraPrestacion).HasColumnType("decimal(12, 3)");

                entity.Property(e => e.Descuento).HasColumnType("decimal(12, 3)");

                entity.Property(e => e.PermisoTrasporte)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.TarifaSumnistro).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.TarifaTransporte).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.TransporteId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("TransporteID");
            });

            modelBuilder.Entity<DuctoMedidor>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroDucto, e.NumeroMedidor })
                    .HasName("PK__DuctoMed__D22D8A3B12DE3EBE");

                entity.ToTable("DuctoMedidor");

                entity.Property(e => e.NumeroEstacion).HasMaxLength(4);

                entity.Property(e => e.ClaveSistemaMedicion)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.IncertidumbreMedicion).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.VigenciaCalibracion).HasColumnType("datetime");
            });

            modelBuilder.Entity<DuctoRecepcion>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroRecepcion })
                    .HasName("PK__DuctoRec__66DEE232ED59AA10");

                entity.ToTable("DuctoRecepcion");

                entity.Property(e => e.NumeroEstacion)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.FechaYhoraFinalRecepcion)
                    .HasColumnType("datetime")
                    .HasColumnName("FechaYHoraFinalRecepcion");

                entity.Property(e => e.FechaYhoraInicioRecepcion)
                    .HasColumnType("datetime")
                    .HasColumnName("FechaYHoraInicioRecepcion");

                entity.Property(e => e.Importe).HasColumnType("numeric(9, 2)");

                entity.Property(e => e.NumeroProducto)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PoderCalorifico)
                    .HasColumnType("numeric(15, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Precio).HasColumnType("numeric(8, 2)");

                entity.Property(e => e.PresionAbsoluta).HasColumnType("numeric(6, 3)");

                entity.Property(e => e.RecepcionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RecepcionID");

                entity.Property(e => e.Temperatura).HasColumnType("numeric(6, 3)");

                entity.Property(e => e.TipoDistribucion)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.VolumenFinal).HasColumnType("numeric(12, 3)");

                entity.Property(e => e.VolumenPuntoEntrada).HasColumnType("numeric(12, 3)");

                entity.Property(e => e.VolumenRecepcion).HasColumnType("numeric(12, 3)");
            });

            modelBuilder.Entity<DuctoRecepcionCfdi>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroRecepcion })
                    .HasName("PK__DuctoRec__66DEE2328544402D");

                entity.ToTable("DuctoRecepcionCFDI");

                entity.Property(e => e.NumeroEstacion)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Cfdi)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("CFDI");

                entity.Property(e => e.ClienteRfc)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ClienteRFC");

                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.Property(e => e.NombreCliente)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NumeroPermisoCre)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("NumeroPermisoCRE");

                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PrecioVtaPublico).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.RegistroId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RegistroID");

                entity.Property(e => e.Serie)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TipoCfdi)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("TipoCFDI");

                entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Volumen).HasColumnType("decimal(12, 3)");
            });

            modelBuilder.Entity<DuctoRecepcionPedimento>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroRecepcion })
                    .HasName("PK__DuctoRec__66DEE2320524EAD0");

                entity.ToTable("DuctoRecepcionPedimento");

                entity.Property(e => e.NumeroEstacion).HasMaxLength(4);

                entity.Property(e => e.ClavePedimento)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.ClavePermisoImportOexport)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ClavePermisoImportOExport");

                entity.Property(e => e.Incoterms)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.MedioIngresoOsalida).HasColumnName("MedioIngresoOSalida");

                entity.Property(e => e.Pais)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.PedimentoId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PedimentoID");

                entity.Property(e => e.PrecioDeImportOexport)
                    .HasColumnType("numeric(12, 3)")
                    .HasColumnName("PrecioDeImportOExport");

                entity.Property(e => e.PuntoInternacionOextracccion).HasColumnName("PuntoInternacionOExtracccion");

                entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Volumen).HasColumnType("numeric(12, 3)");
            });

            modelBuilder.Entity<DuctoRecepcionTransporte>(entity =>
            {
                entity.HasKey(e => new { e.NumeroEstacion, e.NumeroRecepcion })
                    .HasName("PK__DuctoRec__66DEE23233F99965");

                entity.ToTable("DuctoRecepcionTransporte");

                entity.Property(e => e.NumeroEstacion)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CargoCapacidadTrans).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.CargoUsoTrans).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.CargoVolumenTrans).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.ClaveVehiculo)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.ContraPrestacion).HasColumnType("decimal(12, 3)");

                entity.Property(e => e.Descuento).HasColumnType("decimal(12, 3)");

                entity.Property(e => e.PermisoTrasporte)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.TarifaSumnistro).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.TarifaTransporte).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.TransporteId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("TransporteID");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeIdx)
                    .HasName("PK_Empleado");

                entity.ToTable("employee");

                entity.Property(e => e.EmployeeIdx).HasColumnName("employee_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Cellular)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cellular");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.EmployeeNumber).HasColumnName("employee_number");

                entity.Property(e => e.EnableCardPayment).HasColumnName("enable_card_payment");

                entity.Property(e => e.EnableDispensary).HasColumnName("enable_dispensary");

                entity.Property(e => e.EnableMakeShift).HasColumnName("enable_make_shift");

                entity.Property(e => e.EnableOpenIsland).HasColumnName("enable_open_island");

                entity.Property(e => e.EnableOwnVouchers).HasColumnName("enable_own_vouchers");

                entity.Property(e => e.EnablePhotoOnEnding).HasColumnName("enable_photo_on_ending");

                entity.Property(e => e.EnablePrintShift).HasColumnName("enable_print_shift");

                entity.Property(e => e.EnableTank).HasColumnName("enable_tank");

                entity.Property(e => e.IsSupervisor).HasColumnName("is_supervisor");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PasswordNumber).HasColumnName("password_number");

                entity.Property(e => e.Secret)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("secret");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UserName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Employees)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_employee_store");
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.HasKey(e => e.EventTypeIdx)
                    .HasName("PK_TipoEvento");

                entity.ToTable("event_type");

                entity.Property(e => e.EventTypeIdx).HasColumnName("event_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.EventTypeId).HasColumnName("event_type_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<FielType>(entity =>
            {
                entity.HasKey(e => e.FielTypeIdx);

                entity.ToTable("fiel_type");

                entity.HasIndex(e => e.FielTypeId, "IX_fiel_type_id")
                    .IsUnique();

                entity.Property(e => e.FielTypeIdx).HasColumnName("fiel_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.FielTypeId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fiel_type_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Folio>(entity =>
            {
                entity.HasKey(e => e.FolioIdx)
                    .HasName("PK_F0108_Uce");

                entity.ToTable("folio");

                entity.HasIndex(e => new { e.StoreId, e.Type }, "IX_F0108_Uce")
                    .IsUnique();

                entity.Property(e => e.FolioIdx).HasColumnName("folio_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Folio1).HasColumnName("folio");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Type)
                    .HasMaxLength(60)
                    .HasColumnName("type");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Folios)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_folio_store");
            });

            modelBuilder.Entity<Hose>(entity =>
            {
                entity.HasKey(e => e.HoseIdx)
                    .HasName("PK_Manguera");

                entity.ToTable("hose");

                entity.HasIndex(e => new { e.HoseIdi, e.StoreId }, "IX_hose")
                    .IsUnique();

                entity.Property(e => e.HoseIdx).HasColumnName("hose_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CpuAddressHose).HasColumnName("cpu_address_hose");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.HoseIdi).HasColumnName("hose_idi");

                entity.Property(e => e.LoadPositionIdi).HasColumnName("load_position_idi");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.SlowFlow)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("slow_flow");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Hoses)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_hose_store");

                entity.HasOne(d => d.LoadPosition)
                    .WithMany(p => p.Hoses)
                    .HasPrincipalKey(p => new { p.LoadPositionIdi, p.StoreId })
                    .HasForeignKey(d => new { d.LoadPositionIdi, d.StoreId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_hose_load_position");

                entity.HasOne(d => d.ProductStore)
                    .WithMany(p => p.Hoses)
                    .HasPrincipalKey(p => new { p.StoreId, p.ProductId })
                    .HasForeignKey(d => new { d.StoreId, d.ProductId })
                    .HasConstraintName("FK_hose_product_store");
            });

            modelBuilder.Entity<HoseCalibration>(entity =>
            {
                entity.HasKey(e => e.HoseCalibrationIdx)
                    .HasName("PK_CalibracionManguera");

                entity.ToTable("hose_calibration");

                entity.HasIndex(e => e.HoseCalibrationId, "IX_CalibracionManguera")
                    .IsUnique();

                entity.Property(e => e.HoseCalibrationIdx).HasColumnName("hose_calibration_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.HoseCalibrationId).HasColumnName("hose_calibration_id");

                entity.Property(e => e.HoseId).HasColumnName("hose_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.InventoryIdx)
                    .HasName("PK_Inventory_idx");

                entity.ToTable("inventory");

                entity.HasIndex(e => new { e.InventoryId, e.StoreId, e.Date }, "IX_Inventario_compound")
                    .IsUnique();

                entity.HasIndex(e => e.InventoryId, "IX_inventory_id")
                    .IsUnique();

                entity.Property(e => e.InventoryIdx).HasColumnName("inventory_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Height)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("height");

                entity.Property(e => e.HeightWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("height_water");

                entity.Property(e => e.InventoryId).HasColumnName("inventory_id");

                entity.Property(e => e.InventoryNumber).HasColumnName("inventory_number");

                entity.Property(e => e.IsFromShift).HasColumnName("is_from_shift");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Pressure)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("pressure");

                entity.Property(e => e.ProductCodeVeeder).HasColumnName("product_code_veeder");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ShiftHeadId).HasColumnName("shift_head_id");

                entity.Property(e => e.StatusRx).HasColumnName("status_rx");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TankIdi).HasColumnName("tank_idi");

                entity.Property(e => e.Temperature)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("temperature");

                entity.Property(e => e.ToFill)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("to_fill");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume");

                entity.Property(e => e.VolumeTc)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume_tc");

                entity.Property(e => e.VolumeWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume_water");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Inventories)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_inventory_store");
            });

            modelBuilder.Entity<InventoryIn>(entity =>
            {
                entity.HasKey(e => e.InventoryInIdx)
                    .HasName("PK_inventory_in_idx");

                entity.ToTable("inventory_in");

                entity.HasIndex(e => e.InventoryInId, "IX_inventory_in")
                    .IsUnique();

                entity.HasIndex(e => new { e.StoreId, e.InventoryInNumber, e.Date }, "IX_inventory_in_compound")
                    .IsUnique();

                entity.Property(e => e.InventoryInIdx).HasColumnName("inventory_in_idx");

                entity.Property(e => e.AbsolutePressure)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("absolute_pressure")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CalorificPower)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("calorific_power")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.EndHeight)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_height");

                entity.Property(e => e.EndTemperature)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_temperature");

                entity.Property(e => e.EndVolume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_volume");

                entity.Property(e => e.EndVolumeTc)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_volume_tc");

                entity.Property(e => e.EndVolumeWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_volume_water");

                entity.Property(e => e.ImportPermissionId)
                    .HasColumnName("import_permission_id")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InventoryInId).HasColumnName("inventory_in_id");

                entity.Property(e => e.InventoryInNumber).HasColumnName("inventory_in_number");

                entity.Property(e => e.JsonTipoDistribucionId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_distribucion_id")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.ProductCompositionId)
                    .HasColumnName("product_composition_id")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ShiftHeadId).HasColumnName("shift_head_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.StartHeight)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_height");

                entity.Property(e => e.StartTemperature)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_temperature");

                entity.Property(e => e.StartVolume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_volume");

                entity.Property(e => e.StartVolumeTc)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_volume_tc");

                entity.Property(e => e.StartVolumeWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_volume_water");

                entity.Property(e => e.StatusRx).HasColumnName("status_rx");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TankIdi).HasColumnName("tank_idi");

                entity.Property(e => e.TransportPermissionId)
                    .HasColumnName("transport_permission_id")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume");

                entity.Property(e => e.VolumeTc)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume_tc");

                entity.Property(e => e.VolumeWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume_water");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.InventoryIns)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_in_store");
            });

            modelBuilder.Entity<InventoryInDocument>(entity =>
            {
                entity.HasKey(e => e.InventoryInDocumentIdx);

                entity.ToTable("inventory_in_document");

                entity.HasIndex(e => new { e.StoreId, e.InventoryInId, e.InventoryInIdi }, "IX_inventory_in_document_compound")
                    .IsUnique();

                entity.Property(e => e.InventoryInDocumentIdx).HasColumnName("inventory_in_document_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("amount");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Folio).HasColumnName("folio");

                entity.Property(e => e.InventoryInId).HasColumnName("inventory_in_id");

                entity.Property(e => e.InventoryInIdi).HasColumnName("inventory_in_idi");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.JsonClaveUnidadMedidaId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("json_clave_unidad_medida_id");

                entity.Property(e => e.JsonTipoComplementoId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_complemento_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PetitionCustomsId).HasColumnName("petition_customs_id");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(11, 3)")
                    .HasColumnName("price")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PublicSalePrice)
                    .HasColumnType("decimal(11, 3)")
                    .HasColumnName("public_sale_price")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SalePrice)
                    .HasColumnType("decimal(11, 3)")
                    .HasColumnName("sale_price")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SatAclaracion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("sat_aclaracion");

                entity.Property(e => e.SatTipoComprobanteId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_tipo_comprobante_id");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.SupplierFuelIdi).HasColumnName("supplier_fuel_idi");

                entity.Property(e => e.SupplierTransportIdi).HasColumnName("supplier_transport_idi");

                entity.Property(e => e.SupplierTransportRegisterId).HasColumnName("supplier_transport_register_id");

                entity.Property(e => e.TerminalStorage)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("terminal_storage");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Uuid).HasColumnName("uuid");

                entity.Property(e => e.Vehicle)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("vehicle");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume");

                entity.HasOne(d => d.InventoryIn)
                    .WithMany(p => p.InventoryInDocuments)
                    .HasPrincipalKey(p => p.InventoryInId)
                    .HasForeignKey(d => d.InventoryInId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inventory_in_document_inventory_in");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.InventoryInDocuments)
                    .HasPrincipalKey(p => new { p.StoreId, p.SupplierTransportIdi })
                    .HasForeignKey(d => new { d.StoreId, d.SupplierTransportIdi })
                    .HasConstraintName("FK_inventory_in_document_supplier_transport");
            });

            modelBuilder.Entity<InventoryInSaleOrder>(entity =>
            {
                entity.HasKey(e => e.InventoryInSaleOrderIdx)
                    .HasName("PK_inventory_in_sale_order_idx");

                entity.ToTable("inventory_in_sale_order");

                entity.Property(e => e.InventoryInSaleOrderIdx).HasColumnName("inventory_in_sale_order_idx");

                entity.Property(e => e.AbsolutePressure)
                    .HasColumnType("decimal(6, 3)")
                    .HasColumnName("absolute_pressure")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CalorificPower)
                    .HasColumnType("decimal(6, 3)")
                    .HasColumnName("calorific_power")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.EndTemperature)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_temperature");

                entity.Property(e => e.EndVolume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_volume");

                entity.Property(e => e.InventoryInSaleOrderId).HasColumnName("inventory_in_sale_order_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.StartTemperature)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_temperature");

                entity.Property(e => e.StartVolume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_volume");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TankIdi).HasColumnName("tank_idi");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceIdx)
                    .HasName("PK_invoice_idx");

                entity.ToTable("invoice");

                entity.HasIndex(e => new { e.StoreId, e.InvoiceSerieId, e.Folio }, "IX_invoice_compound");

                entity.Property(e => e.InvoiceIdx).HasColumnName("invoice_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(16, 3)")
                    .HasColumnName("amount");

                entity.Property(e => e.AmountIeps)
                    .HasColumnType("decimal(16, 3)")
                    .HasColumnName("amount_ieps");

                entity.Property(e => e.AmountIsr)
                    .HasColumnType("decimal(16, 3)")
                    .HasColumnName("amount_isr");

                entity.Property(e => e.AmountTax)
                    .HasColumnType("decimal(16, 3)")
                    .HasColumnName("amount_tax");

                entity.Property(e => e.ClosingYear).HasColumnName("closing_year");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Folio)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("folio");

                entity.Property(e => e.InvoiceApplicationTypeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("invoice_application_type_id");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.InvoiceSerieId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("invoice_serie_id");

                entity.Property(e => e.IsCancelled).HasColumnName("is_cancelled");

                entity.Property(e => e.IsClosing).HasColumnName("is_closing");

                entity.Property(e => e.IsIssued).HasColumnName("is_issued");

                entity.Property(e => e.IsRelated).HasColumnName("is_related");

                entity.Property(e => e.IsStamped).HasColumnName("is_stamped");

                entity.Property(e => e.IsStampedCancelled).HasColumnName("is_stamped_cancelled");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PacId).HasColumnName("pac_id");

                entity.Property(e => e.SatFormaPagoId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_forma_pago_id");

                entity.Property(e => e.SatMesesId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_meses_id");

                entity.Property(e => e.SatMetodoPagoId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_metodo_pago_id");

                entity.Property(e => e.SatMotivoCancelacionId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_motivo_cancelacion_id");

                entity.Property(e => e.SatPeriodicidadId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_periodicidad_id");

                entity.Property(e => e.SatTipoComprobanteId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_tipo_comprobante_id");

                entity.Property(e => e.SatTipoRelacionId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_tipo_relacion_id");

                entity.Property(e => e.SatUsoCfdiId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_uso_cfdi_id");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(16, 3)")
                    .HasColumnName("subtotal");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Uuid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("uuid");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Invoices)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_invoice_store");
            });

            modelBuilder.Entity<InvoiceApplicationType>(entity =>
            {
                entity.HasKey(e => e.InvoiceApplicationTypeIdx)
                    .HasName("PK_application_type_idx");

                entity.ToTable("invoice_application_type");

                entity.HasIndex(e => e.InvoiceApplicationTypeId, "IX_invoice_application_type_id")
                    .IsUnique();

                entity.Property(e => e.InvoiceApplicationTypeIdx).HasColumnName("invoice_application_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.InvoiceApplicationTypeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("invoice_application_type_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<InvoiceComparison>(entity =>
            {
                entity.HasKey(e => e.InvoiceComparisonIdx)
                    .HasName("PK_invoice comparison_idx");

                entity.ToTable("invoice_comparison");

                entity.HasIndex(e => new { e.CompanyId, e.IsIssued, e.InvoiceSerieId, e.Folio, e.Date }, "IX_invoice_comparison_compound")
                    .IsUnique();

                entity.Property(e => e.InvoiceComparisonIdx).HasColumnName("invoice_comparison_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("amount");

                entity.Property(e => e.AmountIeps)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("amount_ieps");

                entity.Property(e => e.AmountIsr)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("amount_isr");

                entity.Property(e => e.AmountTax)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("amount_tax");

                entity.Property(e => e.CnetEnable).HasColumnName("cnet_enable");

                entity.Property(e => e.CnetIsCancelled).HasColumnName("cnet_is_cancelled");

                entity.Property(e => e.CnetIsStamped).HasColumnName("cnet_is_stamped");

                entity.Property(e => e.CnetIsStampedCancelled).HasColumnName("cnet_is_stamped_cancelled");

                entity.Property(e => e.CodigoStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("codigo_status");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("customer_name");

                entity.Property(e => e.CustomerRfc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("customer_rfc");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EnableValidateSat).HasColumnName("enable_validate_sat");

                entity.Property(e => e.EsCancelable)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("es_cancelable");

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.EstatusCancelacion)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("estatus_cancelacion");

                entity.Property(e => e.Folio)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("folio");

                entity.Property(e => e.InvoiceApplicationTypeId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("invoice_application_type_id");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.InvoiceSerieId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("invoice_serie_id");

                entity.Property(e => e.IsClosing).HasColumnName("is_closing");

                entity.Property(e => e.IsIssued).HasColumnName("is_issued");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Rfc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rfc");

                entity.Property(e => e.SatCustomerName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("sat_customer_name");

                entity.Property(e => e.SatCustomerRfc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_customer_rfc");

                entity.Property(e => e.SatEnable).HasColumnName("sat_enable");

                entity.Property(e => e.SatIsCancelled).HasColumnName("sat_is_cancelled");

                entity.Property(e => e.SatIsStamped).HasColumnName("sat_is_stamped");

                entity.Property(e => e.SatIsStampedCancelled).HasColumnName("sat_is_stamped_cancelled");

                entity.Property(e => e.SatTipoComprobanteId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_tipo_comprobante_id");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("subtotal");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Uuid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("uuid");

                entity.Property(e => e.ValidacionEfos)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("validacion_efos");

                entity.Property(e => e.ValidateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("validate_date");
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.InvoiceDetailIdx)
                    .HasName("PK_invoice_detail_idx");

                entity.ToTable("invoice_detail");

                entity.HasIndex(e => new { e.InvoiceId, e.InvoiceDetailIdi }, "IX_invoice_detail_compound")
                    .IsUnique();

                entity.Property(e => e.InvoiceDetailIdx).HasColumnName("invoice_detail_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(16, 4)")
                    .HasColumnName("amount");

                entity.Property(e => e.AmountIeps)
                    .HasColumnType("decimal(16, 4)")
                    .HasColumnName("amount_ieps");

                entity.Property(e => e.AmountTax)
                    .HasColumnType("decimal(16, 4)")
                    .HasColumnName("amount_tax");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Ieps)
                    .HasColumnType("decimal(16, 4)")
                    .HasColumnName("ieps");

                entity.Property(e => e.InvoiceDetailIdi).HasColumnName("invoice_detail_idi");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.Isr)
                    .HasColumnType("decimal(16, 4)")
                    .HasColumnName("isr");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(12, 4)")
                    .HasColumnName("price");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(16, 4)")
                    .HasColumnName("quantity");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(16, 4)")
                    .HasColumnName("subtotal");

                entity.Property(e => e.Tax)
                    .HasColumnType("decimal(12, 4)")
                    .HasColumnName("tax");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<InvoiceDownload>(entity =>
            {
                entity.HasKey(e => e.InvoiceDownloadIdx);

                entity.ToTable("invoice_download");

                entity.HasIndex(e => new { e.CompanyId, e.Date, e.PeriodTypeId, e.InvoiceDownloadIdi, e.Type }, "IX_invoice_download_compound")
                    .IsUnique();

                entity.Property(e => e.InvoiceDownloadIdx).HasColumnName("invoice_download_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DownloadStatus).HasColumnName("download_status");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.InvoiceDownloadIdi).HasColumnName("invoice_download_idi");

                entity.Property(e => e.IsChecked).HasColumnName("is_checked");

                entity.Property(e => e.IsDownloadFromInvoice).HasColumnName("is_download_from_invoice");

                entity.Property(e => e.IsFinished).HasColumnName("is_finished");

                entity.Property(e => e.IsFinishedVerify).HasColumnName("is_finished_verify");

                entity.Property(e => e.IsStarted).HasColumnName("is_started");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.NumberOfAttempts)
                    .HasColumnName("number_of_attempts")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NumberOfCfdis).HasColumnName("number_of_cfdis");

                entity.Property(e => e.PackagesId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("packages_id");

                entity.Property(e => e.PeriodTypeId).HasColumnName("period_type_id");

                entity.Property(e => e.RequestStatus).HasColumnName("request_status");

                entity.Property(e => e.Response)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("response");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UuidRequest).HasColumnName("uuid_request");
            });

            modelBuilder.Entity<InvoiceFilename>(entity =>
            {
                entity.HasKey(e => e.InvoiceFilenameIdx);

                entity.ToTable("invoice_filename");

                entity.HasIndex(e => new { e.Type, e.UuidRequest, e.PackageId }, "IX_invoice_filename_compound")
                    .IsUnique();

                entity.Property(e => e.InvoiceFilenameIdx).HasColumnName("invoice_filename_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("file_name");

                entity.Property(e => e.IsProcessed).HasColumnName("is_processed");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.NumberOfCfdi).HasColumnName("number_of_cfdi");

                entity.Property(e => e.PackageId)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("package_id");

                entity.Property(e => e.Response)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("response");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UuidRequest).HasColumnName("uuid_request");
            });

            modelBuilder.Entity<InvoiceRelated>(entity =>
            {
                entity.HasKey(e => e.InvoiceRelatedIdx)
                    .HasName("PK_invoice_relation");

                entity.ToTable("invoice_related");

                entity.HasIndex(e => new { e.InvoiceId, e.Uuid }, "IX_invoice_relation_compound")
                    .IsUnique();

                entity.Property(e => e.InvoiceRelatedIdx).HasColumnName("invoice_related_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.InvoiceIdRelated).HasColumnName("invoice_id_related");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Uuid).HasColumnName("uuid");
            });

            modelBuilder.Entity<InvoiceSaleOrder>(entity =>
            {
                entity.HasKey(e => e.InvoiceSaleOrderIdx)
                    .HasName("PK_invoice_sale_order_idx");

                entity.ToTable("invoice_sale_order");

                entity.HasIndex(e => new { e.InvoiceId, e.SaleOrderId }, "IX_invoice_sale_order_compound")
                    .IsUnique();

                entity.Property(e => e.InvoiceSaleOrderIdx).HasColumnName("invoice_sale_order_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(14, 3)")
                    .HasColumnName("amount");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SaleOrderId).HasColumnName("sale_order_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<InvoiceSerie>(entity =>
            {
                entity.HasKey(e => e.InvoiceSerieIdx)
                    .HasName("PK_invoice_serie_idx");

                entity.ToTable("invoice_serie");

                entity.HasIndex(e => new { e.CompanyId, e.StoreId, e.InvoiceSerieId }, "IX_invoice_serie_compound_1")
                    .IsUnique();

                entity.Property(e => e.InvoiceSerieIdx).HasColumnName("invoice_serie_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Folio)
                    .HasColumnName("folio")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.InvoiceSerieId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("invoice_serie_id");

                entity.Property(e => e.InvoiceSerieTypeId).HasColumnName("invoice_serie_type_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<InvoiceSerieType>(entity =>
            {
                entity.HasKey(e => e.InvoiceSerieTypeIdx)
                    .HasName("PK_invoice_serie_type_idx");

                entity.ToTable("invoice_serie_type");

                entity.HasIndex(e => e.InvoiceSerieTypeId, "IX_invoice_serie_type_id")
                    .IsUnique();

                entity.Property(e => e.InvoiceSerieTypeIdx).HasColumnName("invoice_serie_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.FactorForClosing).HasColumnName("factor_for_closing");

                entity.Property(e => e.InvoiceApplicationTypeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("invoice_application_type_id");

                entity.Property(e => e.InvoiceSerieTypeId).HasColumnName("invoice_serie_type_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatTipoComprobanteId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_tipo_comprobante_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<InvoiceStamped>(entity =>
            {
                entity.HasKey(e => e.InvoiceStampedIdx)
                    .HasName("PK_invoice_stamped_idx");

                entity.ToTable("invoice_stamped");

                entity.HasIndex(e => e.InvoiceId, "IX_invoice_stamped_compound")
                    .IsUnique();

                entity.Property(e => e.InvoiceStampedIdx).HasColumnName("invoice_stamped_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.DateStamped)
                    .HasColumnType("datetime")
                    .HasColumnName("date_stamped");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Uuid).HasColumnName("uuid");

                entity.Property(e => e.XmlStamped)
                    .HasColumnType("text")
                    .HasColumnName("xml_stamped");
            });

            modelBuilder.Entity<Island>(entity =>
            {
                entity.HasKey(e => e.IslandIdx)
                    .HasName("PK_Isla");

                entity.ToTable("island");

                entity.HasIndex(e => new { e.StoreId, e.IslandIdi }, "IX_island_1")
                    .IsUnique();

                entity.Property(e => e.IslandIdx).HasColumnName("island_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.IslandIdi).HasColumnName("island_idi");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Islands)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_island_store");
            });

            modelBuilder.Entity<JsonClaveInstalacion>(entity =>
            {
                entity.ToTable("json_clave_instalacion");

                entity.Property(e => e.JsonClaveInstalacionId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("json_clave_instalacion_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonClaveInstalacionIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("json_clave_instalacion_idx");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonClavePermiso>(entity =>
            {
                entity.ToTable("json_clave_permiso");

                entity.Property(e => e.JsonClavePermisoId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("json_clave_permiso_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonClavePermisoIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("json_clave_permiso_idx");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonClaveProducto>(entity =>
            {
                entity.ToTable("json_clave_producto");

                entity.Property(e => e.JsonClaveProductoId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("json_clave_producto_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonClaveProductoIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("json_clave_producto_idx");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonClaveTanque>(entity =>
            {
                entity.ToTable("json_clave_tanque");

                entity.Property(e => e.JsonClaveTanqueId)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("json_clave_tanque_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonClaveTanqueIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("json_clave_tanque_idx");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonClaveUnidadMedidum>(entity =>
            {
                entity.HasKey(e => e.JsonClaveUnidadMedidaId)
                    .HasName("PK__json_cla__F17167371FB126BA");

                entity.ToTable("json_clave_unidad_medida");

                entity.Property(e => e.JsonClaveUnidadMedidaId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("json_clave_unidad_medida_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonClaveUnidadMedidaIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("json_clave_unidad_medida_idx");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonSubclaveProducto>(entity =>
            {
                entity.ToTable("json_subclave_producto");

                entity.Property(e => e.JsonSubclaveProductoId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("json_subclave_producto_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonSubclaveProductoIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("json_subclave_producto_idx");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonTipoComplemento>(entity =>
            {
                entity.ToTable("json_tipo_complemento");

                entity.Property(e => e.JsonTipoComplementoId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_complemento_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonTipoComplementoIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("json_tipo_complemento_idx");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonTipoComposicion>(entity =>
            {
                entity.HasKey(e => e.JsonTipoComposicionIdx);

                entity.ToTable("json_tipo_composicion");

                entity.HasIndex(e => e.JsonTipoComposicionId, "IX_json_tipo_composicion_id")
                    .IsUnique();

                entity.Property(e => e.JsonTipoComposicionIdx).HasColumnName("json_tipo_composicion_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonTipoComposicionId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_composicion_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonTipoDistribucion>(entity =>
            {
                entity.ToTable("json_tipo_distribucion");

                entity.Property(e => e.JsonTipoDistribucionId)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_distribucion_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonTipoDistribucionIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("json_tipo_distribucion_idx");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonTipoDucto>(entity =>
            {
                entity.ToTable("json_tipo_ducto");

                entity.Property(e => e.JsonTipoDuctoId)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_ducto_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonTipoDuctoIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("json_tipo_ducto_idx");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonTipoMedioAlmacenamiento>(entity =>
            {
                entity.HasKey(e => e.JsonTipoMedioAlmacenamientoIdx);

                entity.ToTable("json_tipo_medio_almacenamiento");

                entity.HasIndex(e => e.JsonTipoMedioAlmacenamientoId, "IX_json_tipo_medio_almacenamiento_id")
                    .IsUnique();

                entity.Property(e => e.JsonTipoMedioAlmacenamientoIdx).HasColumnName("json_tipo_medio_almacenamiento_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonTipoMedioAlmacenamientoId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_medio_almacenamiento_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonTipoRegistro>(entity =>
            {
                entity.HasKey(e => e.JsonTipoRegistroIdx);

                entity.ToTable("json_tipo_registro");

                entity.HasIndex(e => e.JsonTipoRegistroId, "IX_json_tipo_registro_id")
                    .IsUnique();

                entity.Property(e => e.JsonTipoRegistroIdx).HasColumnName("json_tipo_registro_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonTipoRegistroId).HasColumnName("json_tipo_registro_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonTipoReporte>(entity =>
            {
                entity.ToTable("json_tipo_reporte");

                entity.Property(e => e.JsonTipoReporteId)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_reporte_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonTipoReporteIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("json_tipo_reporte_idx");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonTipoSistemaMedicion>(entity =>
            {
                entity.HasKey(e => e.JsonTipoSistemaMedicionIdx);

                entity.ToTable("json_tipo_sistema_medicion");

                entity.HasIndex(e => e.JsonTipoSistemaMedicionId, "IX_json_tipo_sistema_medicion_id")
                    .IsUnique();

                entity.Property(e => e.JsonTipoSistemaMedicionIdx).HasColumnName("json_tipo_sistema_medicion_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonTipoSistemaMedicionId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_sistema_medicion_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<JsonTipoTanque>(entity =>
            {
                entity.HasKey(e => e.JsonTipoTanqueIdx);

                entity.ToTable("json_tipo_tanque");

                entity.HasIndex(e => e.JsonTipoTanqueId, "IX_json_tipo_tanque_id")
                    .IsUnique();

                entity.Property(e => e.JsonTipoTanqueIdx).HasColumnName("json_tipo_tanque_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.JsonTipoTanqueId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_tanque_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<LastInventory>(entity =>
            {
                entity.HasKey(e => e.LastInventoryIdx)
                    .HasName("PK_last_inventory_idx");

                entity.ToTable("last_inventory");

                entity.HasIndex(e => new { e.StoreId, e.TankIdi }, "IX_last_inventory_compound")
                    .IsUnique();

                entity.Property(e => e.LastInventoryIdx).HasColumnName("last_inventory_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Height)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("height");

                entity.Property(e => e.HeightWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("height_water");

                entity.Property(e => e.InventoryId).HasColumnName("inventory_id");

                entity.Property(e => e.InventoryNumber).HasColumnName("inventory_number");

                entity.Property(e => e.LastDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_date");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Pressure)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("pressure");

                entity.Property(e => e.ProductCodeVeeder).HasColumnName("product_code_veeder");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.StatusResponse)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("status_response");

                entity.Property(e => e.StatusRx).HasColumnName("status_rx");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TankIdi).HasColumnName("tank_idi");

                entity.Property(e => e.Temperature)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("temperature");

                entity.Property(e => e.ToFill)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("to_fill");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume");

                entity.Property(e => e.VolumeTc)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume_tc");

                entity.Property(e => e.VolumeWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume_water");
            });

            modelBuilder.Entity<LastInventoryIn>(entity =>
            {
                entity.HasKey(e => e.LastInventoryInIdx)
                    .HasName("PK_last_inventory_in_idx");

                entity.ToTable("last_inventory_in");

                entity.HasIndex(e => new { e.StoreId, e.TankIdi }, "IX_last_inventory_in_idx_compound")
                    .IsUnique();

                entity.Property(e => e.LastInventoryInIdx).HasColumnName("last_inventory_in_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DocumentNumber).HasColumnName("document_number");

                entity.Property(e => e.DocumentType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("document_type");

                entity.Property(e => e.DocumentVehicle)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("document_vehicle");

                entity.Property(e => e.DocumentVolume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("document_volume");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.EndHeight)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_height");

                entity.Property(e => e.EndTemperature)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_temperature");

                entity.Property(e => e.EndVolume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_volume");

                entity.Property(e => e.EndVolumeTc)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_volume_tc");

                entity.Property(e => e.EndVolumeWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_volume_water");

                entity.Property(e => e.FuelSupplierId).HasColumnName("fuel_supplier_id");

                entity.Property(e => e.InventoryInId).HasColumnName("inventory_in_id");

                entity.Property(e => e.InventoryInNumber).HasColumnName("inventory_in_number");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.ProductCodeVeeder).HasColumnName("product_code_veeder");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.StartHeight)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_height");

                entity.Property(e => e.StartTemperature)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_temperature");

                entity.Property(e => e.StartVolume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_volume");

                entity.Property(e => e.StartVolumeTc)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_volume_tc");

                entity.Property(e => e.StartVolumeWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_volume_water");

                entity.Property(e => e.StatusRx).HasColumnName("status_rx");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TankIdi).HasColumnName("tank_idi");

                entity.Property(e => e.TransportationId).HasColumnName("Transportation_id");

                entity.Property(e => e.UnitaryPrice)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("unitary_price");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume");

                entity.Property(e => e.VolumeTc)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume_tc");

                entity.Property(e => e.VolumeWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume_water");
            });

            modelBuilder.Entity<LastSaleHose>(entity =>
            {
                entity.HasKey(e => e.LastSaleHoseIdx)
                    .HasName("PK_last_hose_idx");

                entity.ToTable("last_sale_hose");

                entity.HasIndex(e => new { e.StoreId, e.HoseIdi }, "IX_last_sale_hose_compound")
                    .IsUnique();

                entity.Property(e => e.LastSaleHoseIdx).HasColumnName("last_sale_hose_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(14, 3)")
                    .HasColumnName("amount");

                entity.Property(e => e.CardEmployeeId)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("card_employee_id");

                entity.Property(e => e.CpuAddressHose).HasColumnName("cpu_address_hose");

                entity.Property(e => e.CustomerControlId).HasColumnName("customer_control_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.HoseIdi).HasColumnName("hose_idi");

                entity.Property(e => e.LoadPositionIdi).HasColumnName("load_position_idi");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PresetQuantity)
                    .HasColumnType("numeric(11, 4)")
                    .HasColumnName("preset_quantity");

                entity.Property(e => e.PresetType).HasColumnName("preset_type");

                entity.Property(e => e.PresetValue)
                    .HasColumnType("numeric(11, 4)")
                    .HasColumnName("preset_value");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(14, 3)")
                    .HasColumnName("price");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(14, 3)")
                    .HasColumnName("quantity");

                entity.Property(e => e.SaleOrderId).HasColumnName("sale_order_id");

                entity.Property(e => e.SaleOrderNumber).HasColumnName("sale_order_number");

                entity.Property(e => e.SaleOrderNumberStart).HasColumnName("sale_order_number_start");

                entity.Property(e => e.ShiftId).HasColumnName("shift_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.StatusRun).HasColumnName("status_run");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TicketNumber).HasColumnName("ticket_number");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_amount");

                entity.Property(e => e.TotalAmountElectronic)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_amount_electronic");

                entity.Property(e => e.TotalQuantity)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_quantity");

                entity.Property(e => e.TotalQuantityElectronic)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_quantity_electronic");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");
            });

            modelBuilder.Entity<LastShift>(entity =>
            {
                entity.HasKey(e => e.LastShiftIdx)
                    .HasName("PK_last_shift_idx");

                entity.ToTable("last_shift");

                entity.HasIndex(e => new { e.StoreId, e.HoseIdi }, "IX_last_shift_compound")
                    .IsUnique();

                entity.Property(e => e.LastShiftIdx).HasColumnName("last_shift_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CardEmployeeId)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("card_employee_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.EndAmount)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("end_amount");

                entity.Property(e => e.EndAmountElectronic)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("end_amount_electronic");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.EndQuantity)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("end_quantity");

                entity.Property(e => e.EndQuantityElectronic)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("end_quantity_electronic");

                entity.Property(e => e.HoseIdi).HasColumnName("hose_idi");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("price");

                entity.Property(e => e.ShiftHeadId).HasColumnName("shift_head_id");

                entity.Property(e => e.ShiftHeadIdNew).HasColumnName("shift_head_id_new");

                entity.Property(e => e.ShiftId).HasColumnName("shift_id");

                entity.Property(e => e.StartAmount)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("start_amount");

                entity.Property(e => e.StartAmountElectronic)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("start_amount_electronic");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.StartQuantity)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("start_quantity");

                entity.Property(e => e.StartQuantityElectronic)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("start_quantity_electronic");

                entity.Property(e => e.StatusRun).HasColumnName("status_run");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_amount");

                entity.Property(e => e.TotalQuantity)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_quantity");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<License>(entity =>
            {
                entity.HasKey(e => e.LicenseIdx)
                    .HasName("PK_license_idx");

                entity.ToTable("license");

                entity.HasIndex(e => new { e.StoreId, e.SystemName, e.UniqueId }, "IX_license_compound")
                    .IsUnique();

                entity.HasIndex(e => e.LicenseId, "IX_license_id")
                    .IsUnique();

                entity.Property(e => e.LicenseIdx).HasColumnName("license_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.LicenseId).HasColumnName("license_id");

                entity.Property(e => e.LicenseKey)
                    .HasColumnType("text")
                    .HasColumnName("license_key");

                entity.Property(e => e.LicenseValue)
                    .HasColumnType("text")
                    .HasColumnName("license_value");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.SystemName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("system_name");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("unique_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Value)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("value");
            });

            modelBuilder.Entity<LoadPosition>(entity =>
            {
                entity.HasKey(e => e.LoadPositionIdx)
                    .HasName("PK_load_position_idx");

                entity.ToTable("load_position");

                entity.HasIndex(e => new { e.LoadPositionIdi, e.StoreId }, "IX_load_position_compound")
                    .IsUnique();

                entity.Property(e => e.LoadPositionIdx).HasColumnName("load_position_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AutomaticPrintingIsEnabled).HasColumnName("automatic_printing_is_enabled");

                entity.Property(e => e.CpuAddress).HasColumnName("cpu_address");

                entity.Property(e => e.CpuNumberLoop).HasColumnName("cpu_number_loop");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.DefaultQuantity).HasColumnName("default_quantity");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DispensaryIdi).HasColumnName("dispensary_idi");

                entity.Property(e => e.DispensingMode).HasColumnName("dispensing_mode");

                entity.Property(e => e.FactorGetCurrency)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("factor_get_currency");

                entity.Property(e => e.FactorGetPrice)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("factor_get_price");

                entity.Property(e => e.FactorGetQuantity)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("factor_get_quantity");

                entity.Property(e => e.FactorGetTotalCurrency)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("factor_get_total_currency");

                entity.Property(e => e.FactorGetTotalQuantity)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("factor_get_total_quantity");

                entity.Property(e => e.FactorPulseWayne)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("factor_pulse_wayne");

                entity.Property(e => e.FactorSetCurrency)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("factor_set_currency");

                entity.Property(e => e.FactorSetPrice)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("factor_set_price");

                entity.Property(e => e.FactorSetQuantity)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("factor_set_quantity");

                entity.Property(e => e.IsEnableSaveToZero).HasColumnName("is_enable_save_to_zero");

                entity.Property(e => e.IsSecurityEnabled).HasColumnName("is_security_enabled");

                entity.Property(e => e.IsStopCancelled).HasColumnName("is_stop_cancelled");

                entity.Property(e => e.IslandIdi).HasColumnName("island_idi");

                entity.Property(e => e.LoadPositionIdi).HasColumnName("load_position_idi");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MaximumAmount).HasColumnName("maximum_amount");

                entity.Property(e => e.MaximumQuantity).HasColumnName("maximum_quantity");

                entity.Property(e => e.MinimumAmount).HasColumnName("minimum_amount");

                entity.Property(e => e.MinimumQuantity).HasColumnName("minimum_quantity");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PointSaleIdi).HasColumnName("point_sale_idi");

                entity.Property(e => e.PortIdi).HasColumnName("port_idi");

                entity.Property(e => e.PriceLevel).HasColumnName("price_level");

                entity.Property(e => e.QuantityPrefix).HasColumnName("quantity_prefix");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.LoadPositions)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_load_position_store");

                entity.HasOne(d => d.Dispensary)
                    .WithMany(p => p.LoadPositions)
                    .HasPrincipalKey(p => new { p.StoreId, p.DispensaryIdi })
                    .HasForeignKey(d => new { d.StoreId, d.DispensaryIdi })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_load_position_dispensary");

                entity.HasOne(d => d.Island)
                    .WithMany(p => p.LoadPositions)
                    .HasPrincipalKey(p => new { p.StoreId, p.IslandIdi })
                    .HasForeignKey(d => new { d.StoreId, d.IslandIdi })
                    .HasConstraintName("FK_load_position_island");

                entity.HasOne(d => d.Port)
                    .WithMany(p => p.LoadPositions)
                    .HasPrincipalKey(p => new { p.StoreId, p.PortIdi })
                    .HasForeignKey(d => new { d.StoreId, d.PortIdi })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_load_position_port");
            });

            modelBuilder.Entity<LoadPositionResponse>(entity =>
            {
                entity.HasKey(e => e.LoadPositionResponseIdx)
                    .HasName("PK_load_position_response_idx");

                entity.ToTable("load_position_response");

                entity.HasIndex(e => new { e.StoreId, e.LoadPositionIdi }, "IX_load_position_response_compound")
                    .IsUnique();

                entity.Property(e => e.LoadPositionResponseIdx).HasColumnName("load_position_response_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("amount");

                entity.Property(e => e.CardEmployeeId)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("card_employee_id");

                entity.Property(e => e.CommPercentage).HasColumnName("comm_percentage");

                entity.Property(e => e.CpuAddressHose).HasColumnName("cpu_address_hose");

                entity.Property(e => e.CpuAddressHoseAuthorized).HasColumnName("cpu_address_hose_authorized");

                entity.Property(e => e.CustomerControlId).HasColumnName("customer_control_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.HoseIdi).HasColumnName("hose_idi");

                entity.Property(e => e.IsCash).HasColumnName("is_cash");

                entity.Property(e => e.LastStatusDispenser).HasColumnName("last_status_dispenser");

                entity.Property(e => e.LoadPositionIdi).HasColumnName("load_position_idi");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Message)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("message");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PresetQuantity)
                    .HasColumnType("numeric(11, 4)")
                    .HasColumnName("preset_quantity");

                entity.Property(e => e.PresetType).HasColumnName("preset_type");

                entity.Property(e => e.PresetValue)
                    .HasColumnType("numeric(11, 4)")
                    .HasColumnName("preset_value");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("price");

                entity.Property(e => e.PriceLevel).HasColumnName("price_level");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("quantity");

                entity.Property(e => e.SaleOrderNumberStart).HasColumnName("sale_order_number_start");

                entity.Property(e => e.ShiftId).HasColumnName("shift_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.StatusChangeTimes)
                    .HasColumnName("status_change_times")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StatusCommResp).HasColumnName("status_comm_resp");

                entity.Property(e => e.StatusDispenserIdi).HasColumnName("status_dispenser_idi");

                entity.Property(e => e.StatusDispenserRepetitions).HasColumnName("status_dispenser_repetitions");

                entity.Property(e => e.StatusRun).HasColumnName("status_run");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.StoreNumber).HasColumnName("store_number");

                entity.Property(e => e.TicketNumber).HasColumnName("ticket_number");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(11, 3)")
                    .HasColumnName("total_amount");

                entity.Property(e => e.TotalAmountElectronic)
                    .HasColumnType("decimal(11, 3)")
                    .HasColumnName("total_amount_electronic");

                entity.Property(e => e.TotalQuantity)
                    .HasColumnType("decimal(11, 3)")
                    .HasColumnName("total_quantity");

                entity.Property(e => e.TotalQuantityElectronic)
                    .HasColumnType("decimal(11, 3)")
                    .HasColumnName("total_quantity_electronic");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.HasOne(d => d.StatusDispenserIdiNavigation)
                    .WithMany(p => p.LoadPositionResponses)
                    .HasPrincipalKey(p => p.StatusDispenserIdi)
                    .HasForeignKey(d => d.StatusDispenserIdi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_load_position_response_status_dispenser");

                entity.HasOne(d => d.LoadPosition)
                    .WithMany(p => p.LoadPositionResponses)
                    .HasPrincipalKey(p => new { p.LoadPositionIdi, p.StoreId })
                    .HasForeignKey(d => new { d.LoadPositionIdi, d.StoreId })
                    .HasConstraintName("FK_load_position_response_load_position");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("menu");

                entity.Property(e => e.MenuId).HasColumnName("menu_id");

                entity.Property(e => e.IconName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("icon_name");

                entity.Property(e => e.MenuName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("menu_name");

                entity.Property(e => e.PageName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("page_name");

                entity.Property(e => e.ParentMenuId).HasColumnName("parent_menu_id");
            });

            modelBuilder.Entity<Monitor>(entity =>
            {
                entity.HasKey(e => e.MonitorIdx)
                    .HasName("PK_Monitor");

                entity.ToTable("monitor");

                entity.Property(e => e.MonitorIdx).HasColumnName("monitor_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Address)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PortTypeIdi).HasColumnName("port_type_idi");

                entity.Property(e => e.Request)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("request");

                entity.Property(e => e.Response)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("response");

                entity.Property(e => e.Status)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<MonthlySummary>(entity =>
            {
                entity.HasKey(e => e.MonthlySummaryIdx)
                    .HasName("Pk_monthly_summary_idx");

                entity.ToTable("monthly_summary");

                entity.HasIndex(e => new { e.StoreId, e.Date, e.ProductId }, "IX_monthly_summary_compound")
                    .IsUnique();

                entity.Property(e => e.MonthlySummaryIdx).HasColumnName("monthly_summary_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EndInventoryQuantity)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("end_inventory_quantity");

                entity.Property(e => e.InventoryDifference)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("inventory_difference");

                entity.Property(e => e.InventoryDifferencePercentage)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("inventory_difference_percentage");

                entity.Property(e => e.InventoryInQuantity)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("inventory_in_quantity");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("price");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.SaleAmount)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("sale_amount");

                entity.Property(e => e.SaleQuantity)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("sale_quantity");

                entity.Property(e => e.SaleSample)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("sale_sample");

                entity.Property(e => e.StartInventoryQuantity)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("start_inventory_quantity");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TheoryInventoryQuantity)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("theory_inventory_quantity");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.MonthlySummaries)
                    .HasPrincipalKey(p => p.ProductId)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_monthly_summary_product");
            });

            modelBuilder.Entity<MovementType>(entity =>
            {
                entity.HasKey(e => e.MovementTypeIdx)
                    .HasName("PK_movement_type_idx");

                entity.ToTable("movement_type");

                entity.Property(e => e.MovementTypeIdx).HasColumnName("movement_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CompanyId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("company_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MotionEffect).HasColumnName("motion_effect");

                entity.Property(e => e.MovementCounter).HasColumnName("movement_counter");

                entity.Property(e => e.MovementName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("movement_name");

                entity.Property(e => e.MovementTypeId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("movement_type_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Netgroup>(entity =>
            {
                entity.HasKey(e => e.NetgroupIdx)
                    .HasName("PK_netgroup_idx");

                entity.ToTable("netgroup");

                entity.HasIndex(e => e.NetgroupIdx, "IX_netgroup_id")
                    .IsUnique();

                entity.HasIndex(e => e.NetgroupIdx, "IX_netgroup_idi")
                    .IsUnique();

                entity.Property(e => e.NetgroupIdx).HasColumnName("netgroup_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Logo).HasColumnName("logo");

                entity.Property(e => e.NetgroupId).HasColumnName("netgroup_id");

                entity.Property(e => e.NetgroupIdi).HasColumnName("netgroup_idi");

                entity.Property(e => e.NetgroupName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("netgroup_name");

                entity.Property(e => e.NetgroupPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("netgroup_phone");

                entity.Property(e => e.ShortDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("short_description");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<NetgroupBalance>(entity =>
            {
                entity.HasKey(e => e.NetgroupBalanceIdx)
                    .HasName("PK_netgroup_balance_idx");

                entity.ToTable("netgroup_balance");

                entity.HasIndex(e => e.NetgroupBalanceId, "IX_netgroup_balance_id")
                    .IsUnique();

                entity.Property(e => e.NetgroupBalanceIdx).HasColumnName("netgroup_balance_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.ItsAReward).HasColumnName("its_a_reward");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.NetgroupBalanceId).HasColumnName("netgroup_balance_id");

                entity.Property(e => e.NetgroupId).HasColumnName("netgroup_id");

                entity.Property(e => e.NetgroupRewardId).HasColumnName("netgroup_reward_id");

                entity.Property(e => e.RewardPoints)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("reward_points");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<NetgroupPrice>(entity =>
            {
                entity.HasKey(e => e.NetgroupPriceIdx)
                    .HasName("PK_netgroup_price_idx");

                entity.ToTable("netgroup_price");

                entity.HasIndex(e => new { e.NetgroupId, e.NetgroupPriceId }, "IX_netgroup_price_compound")
                    .IsUnique();

                entity.Property(e => e.NetgroupPriceIdx).HasColumnName("netgroup_price_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Discount)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("discount");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.NetgroupId).HasColumnName("netgroup_id");

                entity.Property(e => e.NetgroupPriceId).HasColumnName("netgroup_price_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<NetgroupReward>(entity =>
            {
                entity.HasKey(e => e.NetgroupRewardIdx)
                    .HasName("PK_netgroup_reward_idx");

                entity.ToTable("netgroup_reward");

                entity.HasIndex(e => e.NetgroupRewardId, "IX_netgroup_reward_id")
                    .IsUnique();

                entity.Property(e => e.NetgroupRewardIdx).HasColumnName("netgroup_reward_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.NetgroupId).HasColumnName("netgroup_id");

                entity.Property(e => e.NetgroupRewardId).HasColumnName("netgroup_reward_id");

                entity.Property(e => e.Photo)
                    .HasColumnType("text")
                    .HasColumnName("photo");

                entity.Property(e => e.RewardPoints)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("reward_points");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<NetgroupStore>(entity =>
            {
                entity.HasKey(e => e.NetgroupStoreIdx)
                    .HasName("PK_netgroup_store_idx");

                entity.ToTable("netgroup_store");

                entity.HasIndex(e => new { e.NetgroupId, e.StoreId }, "IX_netgroup_store_compound")
                    .IsUnique();

                entity.Property(e => e.NetgroupStoreIdx).HasColumnName("netgroup_store_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.IsPriceDiscountEnable).HasColumnName("is_price_discount_enable");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.NetgroupId).HasColumnName("netgroup_id");

                entity.Property(e => e.NetgroupPriceId).HasColumnName("netgroup_price_id");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<NetgroupUser>(entity =>
            {
                entity.HasKey(e => e.NetgroupUserIdx)
                    .HasName("PK_netgroup_user_idx");

                entity.ToTable("netgroup_user");

                entity.HasIndex(e => new { e.NetgroupId, e.UserId }, "IX_netgroup_user_compound")
                    .IsUnique();

                entity.HasIndex(e => e.NetgroupUserId, "IX_netgroup_user_id")
                    .IsUnique();

                entity.Property(e => e.NetgroupUserIdx).HasColumnName("netgroup_user_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MakeInvoice).HasColumnName("make_invoice");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.NetgroupId).HasColumnName("netgroup_id");

                entity.Property(e => e.NetgroupUserId).HasColumnName("netgroup_user_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<Odr>(entity =>
            {
                entity.HasKey(e => e.OdrIdx)
                    .HasName("pk_odr_idx");

                entity.ToTable("odr");

                entity.HasIndex(e => e.CardOdr, "IX_card_odr")
                    .IsUnique();

                entity.HasIndex(e => new { e.CustomerId, e.OdrNumber }, "IX_odr_compound")
                    .IsUnique();

                entity.HasIndex(e => e.OdrId, "IX_odr_id")
                    .IsUnique();

                entity.Property(e => e.OdrIdx).HasColumnName("odr_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CardOdr)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("card_odr");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.OdrId).HasColumnName("odr_id");

                entity.Property(e => e.OdrNumber).HasColumnName("odr_number");

                entity.Property(e => e.OdrStoreId).HasColumnName("odr_store_id");

                entity.Property(e => e.OperatorId1).HasColumnName("operator_id_1");

                entity.Property(e => e.OperatorId2).HasColumnName("operator_id_2");

                entity.Property(e => e.PresetQuantity)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("preset_quantity");

                entity.Property(e => e.PresetType).HasColumnName("preset_type");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("product_code");

                entity.Property(e => e.SaleOrderId).HasColumnName("sale_order_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");
            });

            modelBuilder.Entity<OdrStore>(entity =>
            {
                entity.HasKey(e => e.OdrStoreIdx)
                    .HasName("pk_odr_store_idx");

                entity.ToTable("odr_store");

                entity.HasIndex(e => new { e.CustomerId, e.StoreId }, "IX_odr_store_compound")
                    .IsUnique();

                entity.HasIndex(e => e.OdrStoreIdx, "IX_odr_store_id")
                    .IsUnique();

                entity.Property(e => e.OdrStoreIdx).HasColumnName("odr_store_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.OdrStoreId).HasColumnName("odr_store_id");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Operator>(entity =>
            {
                entity.HasKey(e => e.OperatorIdx)
                    .HasName("PK_Operador");

                entity.ToTable("operator");

                entity.HasIndex(e => e.OperatorId, "IX_operador_id")
                    .IsUnique();

                entity.HasIndex(e => new { e.CustomerId, e.OperatorNumber }, "IX_operator_composite")
                    .IsUnique();

                entity.Property(e => e.OperatorIdx).HasColumnName("operator_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MotherLastname)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("mother_lastname");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.OperatorId).HasColumnName("operator_id");

                entity.Property(e => e.OperatorNumber).HasColumnName("operator_number");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasKey(e => e.PageIdx)
                    .HasName("PK_Pagina");

                entity.ToTable("page");

                entity.Property(e => e.PageIdx).HasColumnName("page_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PageId).HasColumnName("page_id");

                entity.Property(e => e.Updated).HasColumnName("updated");

                entity.Property(e => e.Url)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("url");
            });

            modelBuilder.Entity<PagePerUserType>(entity =>
            {
                entity.HasKey(e => e.PagePerUserTypeIdx)
                    .HasName("PK_PaginaTipoUsuario");

                entity.ToTable("page_per_user_type");

                entity.Property(e => e.PagePerUserTypeIdx).HasColumnName("page_per_user_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PagePerUserTypeId).HasColumnName("page_per_user_type_id");

                entity.Property(e => e.Updated).HasColumnName("updated");

                entity.Property(e => e.UserTypeId).HasColumnName("user_type_id");
            });

            modelBuilder.Entity<PaymentSubMode>(entity =>
            {
                entity.HasKey(e => e.PaymentSubModeIdx);

                entity.ToTable("payment_sub_mode");

                entity.Property(e => e.PaymentSubModeIdx).HasColumnName("payment_sub_mode_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PaymentModeId).HasColumnName("payment_mode_id");

                entity.Property(e => e.PaymentSubModeIdi).HasColumnName("payment_sub_mode_idi");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<PeriodType>(entity =>
            {
                entity.HasKey(e => e.PeriodTypeIdx)
                    .HasName("PK_period_type_idx");

                entity.ToTable("period_type");

                entity.HasIndex(e => e.PeriodTypeId, "IX_period_type_id")
                    .IsUnique();

                entity.Property(e => e.PeriodTypeIdx).HasColumnName("period_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PeriodTypeId).HasColumnName("period_type_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<PetitionCustom>(entity =>
            {
                entity.HasKey(e => e.PetitionCustomsIdx);

                entity.ToTable("petition_customs");

                entity.HasIndex(e => e.PetitionCustomsId, "IX_petition_customs_id")
                    .IsUnique();

                entity.Property(e => e.PetitionCustomsIdx).HasColumnName("petition_customs_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AmountOfImportationExportation)
                    .HasColumnType("decimal(12, 3)")
                    .HasColumnName("amount_of_importation_exportation");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Incoterms)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("incoterms");

                entity.Property(e => e.JsonClaveUnidadMedidadId)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("json_clave_unidad_medidad_id");

                entity.Property(e => e.KeyOfImportationExportation)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("key_of_importation_exportation");

                entity.Property(e => e.KeyPointOfInletOrOulet)
                    .HasColumnType("decimal(9, 3)")
                    .HasColumnName("key_point_of_inlet_or_oulet");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.NumberCustomsDeclaration)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("number_customs_declaration");

                entity.Property(e => e.PetitionCustomsId).HasColumnName("petition_customs_id");

                entity.Property(e => e.QuantityDocumented)
                    .HasColumnType("decimal(12, 3)")
                    .HasColumnName("quantity_Documented");

                entity.Property(e => e.SatPaisId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_pais_id");

                entity.Property(e => e.TransportMediumnCustomsId).HasColumnName("transport_mediumn_customs_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<PointSale>(entity =>
            {
                entity.HasKey(e => e.PointSaleIdx)
                    .HasName("PK_PuntoVenta");

                entity.ToTable("point_sale");

                entity.HasIndex(e => new { e.StoreId, e.PointSaleIdi }, "IX_point_sale_compound")
                    .IsUnique();

                entity.Property(e => e.PointSaleIdx).HasColumnName("point_sale_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.CommPercentage).HasColumnName("comm_percentage");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.InvoiceSerieId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("invoice_serie_id");

                entity.Property(e => e.IsEnabledPrintToPrinterIdi).HasColumnName("is_enabled_print_to_printer_idi");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PointSaleIdi).HasColumnName("point_sale_idi");

                entity.Property(e => e.PointSaleUnique)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("point_sale_unique");

                entity.Property(e => e.PortIdi).HasColumnName("port_idi");

                entity.Property(e => e.PrinterBaudRate).HasColumnName("printer_baud_rate");

                entity.Property(e => e.PrinterBrandId).HasColumnName("printer_brand_id");

                entity.Property(e => e.PrinterIdi).HasColumnName("printer_idi");

                entity.Property(e => e.StatusRes).HasColumnName("status_res");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Subtype).HasColumnName("subtype");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.TypeAuthorization).HasColumnName("type_authorization");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.PointSales)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_point_sale_store");
            });

            modelBuilder.Entity<Port>(entity =>
            {
                entity.HasKey(e => e.PortIdx)
                    .HasName("PK_Puerto");

                entity.ToTable("port");

                entity.HasIndex(e => new { e.StoreId, e.PortIdi }, "IX_port")
                    .IsUnique();

                entity.HasIndex(e => new { e.StoreId, e.Name }, "IX_port_name")
                    .IsUnique();

                entity.Property(e => e.PortIdx).HasColumnName("port_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BaudRate).HasColumnName("baud_rate");

                entity.Property(e => e.Bits).HasColumnName("bits");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.IsFound).HasColumnName("is_found");

                entity.Property(e => e.IsShowTxrx).HasColumnName("is_show_txrx");

                entity.Property(e => e.IsWithEcho).HasColumnName("is_with_echo");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MonitorDate)
                    .HasColumnType("datetime")
                    .HasColumnName("monitor_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.NameLinux)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("name_linux");

                entity.Property(e => e.Parity)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("parity");

                entity.Property(e => e.PortIdi).HasColumnName("port_idi");

                entity.Property(e => e.PortTypeIdi).HasColumnName("port_type_idi");

                entity.Property(e => e.StopBit).HasColumnName("stop_bit");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.PortTypeIdiNavigation)
                    .WithMany(p => p.Ports)
                    .HasPrincipalKey(p => p.PortTypeIdi)
                    .HasForeignKey(d => d.PortTypeIdi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_port_port_type");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Ports)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_port_store");
            });

            modelBuilder.Entity<PortResponse>(entity =>
            {
                entity.HasKey(e => e.PortResponseIdx);

                entity.ToTable("port_response");

                entity.HasIndex(e => new { e.StoreId, e.PortIdi, e.DeviceName, e.DeviceBrand, e.CpuAddress }, "IX_port_response_compound")
                    .IsUnique();

                entity.Property(e => e.PortResponseIdx).HasColumnName("port_response_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CommPercentage).HasColumnName("comm_percentage ");

                entity.Property(e => e.CpuAddress).HasColumnName("cpu_address");

                entity.Property(e => e.CpuNumberLoop).HasColumnName("cpu_number_loop");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DeviceBrand)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("device_brand");

                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("device_name");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PortIdi).HasColumnName("port_idi");

                entity.Property(e => e.Response)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("response");

                entity.Property(e => e.Response2)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("response2");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<PortType>(entity =>
            {
                entity.HasKey(e => e.PortTypeIdx);

                entity.ToTable("port_type");

                entity.HasIndex(e => e.PortTypeIdi, "IX_port_type")
                    .IsUnique();

                entity.Property(e => e.PortTypeIdx).HasColumnName("port_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PortTypeIdi).HasColumnName("port_type_idi");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Printer>(entity =>
            {
                entity.HasKey(e => e.PrinterIdx)
                    .HasName("PK_Printer");

                entity.ToTable("printer");

                entity.HasIndex(e => new { e.StoreId, e.PrinterIdi }, "IX_printer_compound")
                    .IsUnique();

                entity.Property(e => e.PrinterIdx).HasColumnName("printer_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.EnablePrintToPrinterName).HasColumnName("enable_print_to_printer_name");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PrinterBaudRate).HasColumnName("printer_baud_rate");

                entity.Property(e => e.PrinterBrandId).HasColumnName("printer_brand_id");

                entity.Property(e => e.PrinterIdi).HasColumnName("printer_idi");

                entity.Property(e => e.PrinterIpAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("printer_ip_address");

                entity.Property(e => e.PrinterIpPort).HasColumnName("printer_ip_port");

                entity.Property(e => e.PrinterName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("printer_name");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<PrinterBrand>(entity =>
            {
                entity.ToTable("printer_brand");

                entity.Property(e => e.PrinterBrandId)
                    .ValueGeneratedNever()
                    .HasColumnName("printer_brand_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductIdx)
                    .HasName("PK_product_idx");

                entity.ToTable("product");

                entity.HasIndex(e => e.ProductCode, "IX_product_code")
                    .IsUnique();

                entity.HasIndex(e => e.ProductId, "IX_product_id")
                    .IsUnique();

                entity.Property(e => e.ProductIdx).HasColumnName("product_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AppName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("app_name");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("barcode");

                entity.Property(e => e.Color)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.IsFuel).HasColumnName("is_fuel");

                entity.Property(e => e.JsonClaveUnidadMedidaId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("json_clave_unidad_medida_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_id");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("product_code");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.SatClaveProductoServicioId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_clave_producto_servicio_id");

                entity.Property(e => e.SatClaveUnidadId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_clave_unidad_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.ProductCategoryIdx)
                    .HasName("PK_category_idx");

                entity.ToTable("product_category");

                entity.HasIndex(e => e.ProductCategoryId, "IX_category_id")
                    .IsUnique();

                entity.Property(e => e.ProductCategoryIdx).HasColumnName("product_category_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<ProductComposition>(entity =>
            {
                entity.HasKey(e => e.ProductCompositionIdx);

                entity.ToTable("product_composition");

                entity.HasIndex(e => new { e.ProductCompositionId, e.ProductId, e.JsonTipoComposicionId }, "IX_product_composition")
                    .IsUnique();

                entity.Property(e => e.ProductCompositionIdx).HasColumnName("product_composition_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CalorificPower)
                    .HasColumnType("decimal(9, 3)")
                    .HasColumnName("calorific_power");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.JsonTipoComposicionId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_composicion_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MolarFraction)
                    .HasColumnType("decimal(4, 3)")
                    .HasColumnName("molar_fraction");

                entity.Property(e => e.ProductCompositionId).HasColumnName("product_composition_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<ProductPrice>(entity =>
            {
                entity.HasKey(e => e.ProductPriceIdx)
                    .HasName("PK_product_price_idx");

                entity.ToTable("product_price");

                entity.HasIndex(e => new { e.StoreId, e.ProductId, e.Date }, "IX_product_price_compound")
                    .IsUnique();

                entity.Property(e => e.ProductPriceIdx).HasColumnName("product_price_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ApplicationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("application_date");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Ieps)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("ieps");

                entity.Property(e => e.IsApplied).HasColumnName("is_applied");

                entity.Property(e => e.IsCancelled).HasColumnName("is_cancelled");

                entity.Property(e => e.IsImmediate).HasColumnName("is_immediate");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("price");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ProgrammingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("programming_date");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UserId)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductSat>(entity =>
            {
                entity.HasKey(e => e.ProductSatIdx)
                    .HasName("PK_product_sat_idx");

                entity.ToTable("product_sat");

                entity.HasIndex(e => new { e.StoreId, e.ProductId }, "IX_product_sat_compound")
                    .IsUnique();

                entity.Property(e => e.ProductSatIdx).HasColumnName("product_sat_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ComposDeAzufreEnPetroleo).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.ComposDeButanoEnGasLp)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("ComposDeButanoEnGasLP");

                entity.Property(e => e.ComposDePropanoEnGasLp)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("ComposDePropanoEnGasLP");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DensidadDePetroleo).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.DieselConCombustibleNoFosil).HasMaxLength(5);

                entity.Property(e => e.FraccionMolar).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.GasNaturalOcondensados)
                    .HasMaxLength(5)
                    .HasColumnName("GasNaturalOCondensados");

                entity.Property(e => e.GasolinaConCombustibleNoFosil).HasMaxLength(5);

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Otros).HasMaxLength(5);

                entity.Property(e => e.PoderCalorifico).HasColumnType("decimal(12, 3)");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.SatPercentageWithFossil)
                    .HasColumnName("sat_percentage_with_fossil")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SatProductKey)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("sat_product_key")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SatProductSubkey)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("sat_product_subkey")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SatWithFossil)
                    .HasColumnName("sat_with_fossil")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TipoCompuesto).HasMaxLength(5);

                entity.Property(e => e.TurbosinaConCombustibleNoFosil).HasMaxLength(5);

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSats)
                    .HasPrincipalKey(p => p.ProductId)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_sat_product");
            });

            modelBuilder.Entity<ProductStore>(entity =>
            {
                entity.HasKey(e => e.ProductStoreIdx)
                    .HasName("PK_product_store_idx");

                entity.ToTable("product_store");

                entity.HasIndex(e => new { e.StoreId, e.ProductId }, "IX_product_store_compound")
                    .IsUnique();

                entity.Property(e => e.ProductStoreIdx).HasColumnName("product_store_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Color)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.Cost)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("cost");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Existence)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("existence");

                entity.Property(e => e.FactorTc)
                    .HasColumnType("decimal(12, 10)")
                    .HasColumnName("factor_tc");

                entity.Property(e => e.Ieps)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("ieps");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("price");

                entity.Property(e => e.ProductDensityGramsPerCm3)
                    .HasColumnName("product_density_grams_per_cm3")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ProductPemex)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("product_pemex");

                entity.Property(e => e.ProductUce)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("product_uce");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.SystemGradeId).HasColumnName("system_grade_id");

                entity.Property(e => e.Tax)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("tax");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductStores)
                    .HasPrincipalKey(p => p.ProductId)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_store_product");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ProductStores)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_store_store");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.ReportIdx)
                    .HasName("PK_report_idx");

                entity.ToTable("report");

                entity.HasIndex(e => e.ReportId, "IX_report_id")
                    .IsUnique();

                entity.Property(e => e.ReportIdx).HasColumnName("report_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Head)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("head");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.NameOfColumnsForSubtotal)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name_of_columns_for_subtotal");

                entity.Property(e => e.NumberOfColumnsForSubtotal).HasColumnName("number_of_columns_for_subtotal");

                entity.Property(e => e.Query)
                    .HasColumnType("text")
                    .HasColumnName("query");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("short_name");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<ReportCode>(entity =>
            {
                entity.HasKey(e => e.ReportCodeIdx)
                    .HasName("PK_report_code_idx");

                entity.ToTable("report_code");

                entity.HasIndex(e => e.ReportCodeId, "IX_report_code_id")
                    .IsUnique();

                entity.Property(e => e.ReportCodeIdx).HasColumnName("report_code_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.ReportCodeId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("report_code_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<ReportInput>(entity =>
            {
                entity.HasKey(e => e.ReportInputIdx)
                    .HasName("PK_report_input_idx");

                entity.ToTable("report_input");

                entity.HasIndex(e => new { e.ReportId, e.ReportCodeId }, "IX_report_code_compound_2")
                    .IsUnique();

                entity.HasIndex(e => new { e.ReportId, e.ReportInputIdi }, "IX_report_input_compound")
                    .IsUnique();

                entity.Property(e => e.ReportInputIdx).HasColumnName("report_input_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.OrderNumber).HasColumnName("order_number");

                entity.Property(e => e.ReportCodeId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("report_code_id");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.ReportInputIdi).HasColumnName("report_input_idi");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<ReportModule>(entity =>
            {
                entity.HasKey(e => e.ReportModuleIdx)
                    .HasName("PK_report_module_idx");

                entity.ToTable("report_module");

                entity.HasIndex(e => e.ReportModuleIdi, "IX_report_module_idi")
                    .IsUnique();

                entity.Property(e => e.ReportModuleIdx).HasColumnName("report_module_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.ReportModuleIdi).HasColumnName("report_module_idi");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<ReportModuleDetail>(entity =>
            {
                entity.HasKey(e => e.ReportModuleDetailIdx)
                    .HasName("PK_report_module_detail_idx");

                entity.ToTable("report_module_detail");

                entity.HasIndex(e => new { e.ReportModuleIdi, e.ReportId }, "IX_report_module_detail_compound")
                    .IsUnique();

                entity.Property(e => e.ReportModuleDetailIdx).HasColumnName("report_module_detail_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.ReportModuleIdi).HasColumnName("report_module_idi");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => e.RolePermissionIdx);

                entity.ToTable("role_permission");

                entity.Property(e => e.RolePermissionIdx).HasColumnName("role_permission_idx");

                entity.Property(e => e.Action)
                    .HasMaxLength(100)
                    .HasColumnName("action");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(450)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("role_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SaleOrder>(entity =>
            {
                entity.HasKey(e => e.SaleOrderIdx)
                    .HasName("pk_sale_oder_idx");

                entity.ToTable("sale_order");

                entity.HasIndex(e => e.SaleOrderId, "IX_sale_order_id")
                    .IsUnique();

                entity.HasIndex(e => new { e.StoreId, e.SaleOrderNumber, e.Date }, "IX_sale_order_id_compound")
                    .IsUnique();

                entity.Property(e => e.SaleOrderIdx).HasColumnName("sale_order_idx");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(14, 3)")
                    .HasColumnName("amount");

                entity.Property(e => e.CardEmployeeId)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("card_employee_id");

                entity.Property(e => e.CustomerControlId).HasColumnName("customer_control_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.HoseIdi).HasColumnName("hose_idi");

                entity.Property(e => e.InventoryInSaleOrderId).HasColumnName("inventory_in_sale_order_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.SaleOrderId).HasColumnName("sale_order_id");

                entity.Property(e => e.SaleOrderNumber).HasColumnName("sale_order_number");

                entity.Property(e => e.SaleOrderNumberStart).HasColumnName("sale_order_number_start");

                entity.Property(e => e.ShiftId).HasColumnName("shift_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TankIdi).HasColumnName("tank_idi");

                entity.Property(e => e.TicketNumber).HasColumnName("ticket_number");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.SaleOrders)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_sale_order_store");
            });

            modelBuilder.Entity<SaleOrderPayment>(entity =>
            {
                entity.HasKey(e => e.SaleOrderPaymentIdx)
                    .HasName("PK_sale_order_payment_idx");

                entity.ToTable("sale_order_payment");

                entity.HasIndex(e => new { e.SaleOrderId, e.SaleOrderPaymentIdi }, "IX_sale_order_payment_compound");

                entity.Property(e => e.SaleOrderPaymentIdx).HasColumnName("sale_order_payment_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BankCardId).HasColumnName("bank_card_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Paid)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("paid");

                entity.Property(e => e.PaymentSubModeIdi).HasColumnName("payment_sub_mode_idi");

                entity.Property(e => e.Received)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("received");

                entity.Property(e => e.Reference)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("reference");

                entity.Property(e => e.Reimburse)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("reimburse");

                entity.Property(e => e.SaleOrderId).HasColumnName("sale_order_id");

                entity.Property(e => e.SaleOrderPaymentIdi).HasColumnName("sale_order_payment_idi");

                entity.Property(e => e.SatFormaPagoId)
                    .HasMaxLength(4)
                    .HasColumnName("sat_forma_pago_id");

                entity.Property(e => e.SatFormaSubpagoId).HasColumnName("sat_forma_subpago_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SaleOrderPhoto>(entity =>
            {
                entity.HasKey(e => e.SaleOrderPhotoIdx)
                    .HasName("PK_sale_order_photo_idx");

                entity.ToTable("sale_order_photo");

                entity.HasIndex(e => new { e.SaleOrderId, e.SaleOrderPhotoIdi }, "IX_sale_order_photo_compound");

                entity.Property(e => e.SaleOrderPhotoIdx).HasColumnName("sale_order_photo_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Photo)
                    .HasColumnType("text")
                    .HasColumnName("photo");

                entity.Property(e => e.SaleOrderId).HasColumnName("sale_order_id");

                entity.Property(e => e.SaleOrderPhotoIdi).HasColumnName("sale_order_photo_idi");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SaleSuborder>(entity =>
            {
                entity.HasKey(e => e.SaleSuborderIdx)
                    .HasName("PK_sale_suborder_idx");

                entity.ToTable("sale_suborder");

                entity.HasIndex(e => new { e.SaleOrderId, e.SaleSuborderIdi }, "IX_sale_suborder_compound")
                    .IsUnique();

                entity.Property(e => e.SaleSuborderIdx).HasColumnName("sale_suborder_idx");

                entity.Property(e => e.AbsolutePressure)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("absolute_pressure")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("amount");

                entity.Property(e => e.CalorificPower)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("calorific_power")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DeviceIdi).HasColumnName("device_idi");

                entity.Property(e => e.Discount)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("discount");

                entity.Property(e => e.EndQuantity)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("end_quantity")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Ieps)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("ieps");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PetitionCustomsId).HasColumnName("petition_customs_id");

                entity.Property(e => e.PresetQuantity)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("preset_quantity");

                entity.Property(e => e.PresetType).HasColumnName("preset_type");

                entity.Property(e => e.PresetValue)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("preset_value");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("price");

                entity.Property(e => e.ProductCompositionId)
                    .HasColumnName("product_composition_id")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("quantity");

                entity.Property(e => e.QuantityTc)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("quantity_tc");

                entity.Property(e => e.SaleOrderId).HasColumnName("sale_order_id");

                entity.Property(e => e.SaleSuborderIdi).HasColumnName("sale_suborder_idi");

                entity.Property(e => e.StartQuantity)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("start_quantity")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("subtotal");

                entity.Property(e => e.SupplierTransportRegisterId).HasColumnName("supplier_transport_register_id");

                entity.Property(e => e.Tax)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("tax");

                entity.Property(e => e.Temperature)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("temperature");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_amount");

                entity.Property(e => e.TotalAmountElectronic)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_amount_electronic");

                entity.Property(e => e.TotalQuantity)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_quantity");

                entity.Property(e => e.TotalQuantityElectronic)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_quantity_electronic");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatClaveProductoServicio>(entity =>
            {
                entity.HasKey(e => e.SatClaveProductoServicioIdx)
                    .HasName("PK_sat_clave_producto_servicio_idx");

                entity.ToTable("sat_clave_producto_servicio");

                entity.HasIndex(e => e.SatClaveProductoServicioId, "IX_sat_clave_producto_servicio")
                    .IsUnique();

                entity.Property(e => e.SatClaveProductoServicioIdx).HasColumnName("sat_clave_producto_servicio_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.FechaFinVigencia).HasColumnType("datetime");

                entity.Property(e => e.FechaInicioVigencia).HasColumnType("datetime");

                entity.Property(e => e.IncluirComplemento).HasMaxLength(50);

                entity.Property(e => e.IncluirIepsTrasladado).HasMaxLength(50);

                entity.Property(e => e.IncluirIvaTrasladado).HasMaxLength(50);

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PalabrasSimilares).HasColumnName("palabrasSimilares");

                entity.Property(e => e.SatClaveProductoServicioId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_clave_producto_servicio_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatClaveUnidad>(entity =>
            {
                entity.HasKey(e => e.SatClaveUnidadIdx);

                entity.ToTable("sat_clave_unidad");

                entity.HasIndex(e => e.SatClaveUnidadId, "IX_sat_clave_unidad")
                    .IsUnique();

                entity.Property(e => e.SatClaveUnidadIdx).HasColumnName("sat_clave_unidad_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.SatClaveUnidadId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_clave_unidad_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatCodigoPostal>(entity =>
            {
                entity.HasKey(e => e.SatCodigoPostalIdx);

                entity.ToTable("sat_codigo_postal");

                entity.HasIndex(e => e.SatCodigoPostalId, "IX_sat_codigo_postal")
                    .IsUnique();

                entity.Property(e => e.SatCodigoPostalIdx).HasColumnName("sat_codigo_postal_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatCodigoPostalId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_codigo_postal_id");

                entity.Property(e => e.SatEstadoId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_estado_id");

                entity.Property(e => e.SatLocalidadClave)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_localidad_clave");

                entity.Property(e => e.SatMunicipioClave)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_municipio_clave");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatErrorCancelacion>(entity =>
            {
                entity.ToTable("sat_error_cancelacion");

                entity.Property(e => e.SatErrorCancelacionId)
                    .ValueGeneratedNever()
                    .HasColumnName("sat_error_cancelacion_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Message)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("message");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatEstado>(entity =>
            {
                entity.HasKey(e => e.SatEstadoIdx)
                    .HasName("PK_estado_idx");

                entity.ToTable("sat_estado");

                entity.HasIndex(e => e.SatEstadoId, "IX_sat_estado")
                    .IsUnique();

                entity.Property(e => e.SatEstadoIdx).HasColumnName("sat_estado_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatEstadoId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_estado_id");

                entity.Property(e => e.SatPaisId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_pais_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatFormaPago>(entity =>
            {
                entity.HasKey(e => e.SatFormaPagoIdx)
                    .HasName("PK_payment_mode_idx");

                entity.ToTable("sat_forma_pago");

                entity.HasIndex(e => e.SatFormaPagoId, "IX_sat_forma_pago_id")
                    .IsUnique();

                entity.Property(e => e.SatFormaPagoIdx).HasColumnName("sat_forma_pago_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ActiveInvoiceOnFinalCustomer).HasColumnName("active_invoice_on_final_customer");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaFinVigencia)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_fin_vigencia");

                entity.Property(e => e.FechaInicioVigencia)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_inicio_vigencia");

                entity.Property(e => e.IsInMobileApp).HasColumnName("is_in_mobile_app");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatFormaPagoId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_forma_pago_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatFormaSubpago>(entity =>
            {
                entity.HasKey(e => e.SatFormaSubpagoIdx)
                    .HasName("PK_sat_forma_subpago_idx");

                entity.ToTable("sat_forma_subpago");

                entity.HasIndex(e => e.SatFormaSubpagoId, "IX_sat_forma_subpago_id")
                    .IsUnique();

                entity.Property(e => e.SatFormaSubpagoIdx).HasColumnName("sat_forma_subpago_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.SatFormaPagoId)
                    .HasMaxLength(4)
                    .HasColumnName("sat_forma_pago_id");

                entity.Property(e => e.SatFormaSubpagoId).HasColumnName("sat_forma_subpago_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatLocalidad>(entity =>
            {
                entity.HasKey(e => e.SatLocalidadIdx);

                entity.ToTable("sat_localidad");

                entity.HasIndex(e => e.SatLocalidadId, "IX_sat_localidad")
                    .IsUnique();

                entity.Property(e => e.SatLocalidadIdx).HasColumnName("sat_localidad_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatEstadoId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_estado_id");

                entity.Property(e => e.SatLocalidadClave)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_localidad_clave");

                entity.Property(e => e.SatLocalidadId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("sat_localidad_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatMese>(entity =>
            {
                entity.HasKey(e => e.SatMesesIdx)
                    .HasName("PK_meses_idx");

                entity.ToTable("sat_meses");

                entity.HasIndex(e => e.SatMesesId, "IX_sat_meses_id")
                    .IsUnique();

                entity.Property(e => e.SatMesesIdx).HasColumnName("sat_meses_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatMesesId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_meses_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatMetodoPago>(entity =>
            {
                entity.HasKey(e => e.SatMetodoPagoIdx)
                    .HasName("PK_metodo_pago_idx");

                entity.ToTable("sat_metodo_pago");

                entity.HasIndex(e => e.SatMetodoPagoId, "IX_metodo_pago_id")
                    .IsUnique();

                entity.Property(e => e.SatMetodoPagoIdx).HasColumnName("sat_metodo_pago_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatMetodoPagoId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_metodo_pago_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatMotivoCancelacion>(entity =>
            {
                entity.HasKey(e => e.SatMotivoCancelacionIdx)
                    .HasName("PK__sat_motivo_cancelacion_idx");

                entity.ToTable("sat_motivo_cancelacion");

                entity.HasIndex(e => e.SatMotivoCancelacionId, "IX_sat_motivo_cancelacion_id")
                    .IsUnique();

                entity.Property(e => e.SatMotivoCancelacionIdx).HasColumnName("sat_motivo_cancelacion_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaFinVigencia).HasColumnType("datetime");

                entity.Property(e => e.FechaInicioVigencia).HasColumnType("datetime");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatMotivoCancelacionId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_motivo_cancelacion_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatMunicipio>(entity =>
            {
                entity.HasKey(e => e.SatMunicipioIdx);

                entity.ToTable("sat_municipio");

                entity.HasIndex(e => e.SatMunicipioId, "IX_sat_municipio")
                    .IsUnique();

                entity.Property(e => e.SatMunicipioIdx).HasColumnName("sat_municipio_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatEstadoId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_estado_id");

                entity.Property(e => e.SatMunicipioClave)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_municipio_clave");

                entity.Property(e => e.SatMunicipioId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_municipio_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatPai>(entity =>
            {
                entity.HasKey(e => e.SatPaisIdx)
                    .HasName("PK_sat_pais_idx");

                entity.ToTable("sat_pais");

                entity.HasIndex(e => e.SatPaisId, "IX_sat_pais_id")
                    .IsUnique();

                entity.Property(e => e.SatPaisIdx).HasColumnName("sat_pais_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatPaisId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_pais_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatPeriodicidad>(entity =>
            {
                entity.HasKey(e => e.SatPeriodicidadIdx)
                    .HasName("PK_periodicidad_idx");

                entity.ToTable("sat_periodicidad");

                entity.HasIndex(e => e.SatPeriodicidadId, "IX_periodicidad_id")
                    .IsUnique();

                entity.Property(e => e.SatPeriodicidadIdx).HasColumnName("sat_periodicidad_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatPeriodicidadId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_periodicidad_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatRegimenFiscal>(entity =>
            {
                entity.HasKey(e => e.SatRegimenFiscalIdx);

                entity.ToTable("sat_regimen_fiscal");

                entity.HasIndex(e => e.SatRegimenFiscalId, "IX_sat_regimen_fiscal")
                    .IsUnique();

                entity.Property(e => e.SatRegimenFiscalIdx).HasColumnName("sat_regimen_fiscal_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaFinVigencia)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_fin_vigencia");

                entity.Property(e => e.FechaInicioVigencia)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_inicio_vigencia");

                entity.Property(e => e.Fisica)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("fisica");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Moral)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("moral");

                entity.Property(e => e.SatRegimenFiscalId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sat_regimen_fiscal_id");
            });

            modelBuilder.Entity<SatRegimenfiscalUsocfdi>(entity =>
            {
                entity.HasKey(e => e.SatRegimenfiscalUsocfdi1);

                entity.ToTable("sat_regimenfiscal_usocfdi");

                entity.Property(e => e.SatRegimenfiscalUsocfdi1).HasColumnName("sat_regimenfiscal_usocfdi");

                entity.Property(e => e.SatRegimenFiscalId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sat_regimen_fiscal_id");

                entity.Property(e => e.SatUsoCfdiId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sat_uso_cfdi_id");

                entity.HasOne(d => d.SatRegimenFiscal)
                    .WithMany(p => p.SatRegimenfiscalUsocfdis)
                    .HasPrincipalKey(p => p.SatRegimenFiscalId)
                    .HasForeignKey(d => d.SatRegimenFiscalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_sat_regimenfiscal_usocfdi_sat_regimen_fiscal");

                entity.HasOne(d => d.SatUsoCfdi)
                    .WithMany(p => p.SatRegimenfiscalUsocfdis)
                    .HasPrincipalKey(p => p.SatUsoCfdiId)
                    .HasForeignKey(d => d.SatUsoCfdiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_sat_regimenfiscal_usocfdi_sat_uso_cfdi");
            });

            modelBuilder.Entity<SatTipoComprobante>(entity =>
            {
                entity.HasKey(e => e.SatTipoComprobanteIdx)
                    .HasName("PK_tipo_comprobante_idx");

                entity.ToTable("sat_tipo_comprobante");

                entity.HasIndex(e => e.SatTipoComprobanteId, "IX_tipo_comprobante_id")
                    .IsUnique();

                entity.Property(e => e.SatTipoComprobanteIdx).HasColumnName("sat_tipo_comprobante_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatTipoComprobanteId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_tipo_comprobante_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatTipoRelacion>(entity =>
            {
                entity.HasKey(e => e.SatTipoRelacionIdx)
                    .HasName("PK_tipo_relacion_idx");

                entity.ToTable("sat_tipo_relacion");

                entity.HasIndex(e => e.SatTipoRelacionId, "IX_tipo_relacion_id")
                    .IsUnique();

                entity.Property(e => e.SatTipoRelacionIdx).HasColumnName("sat_tipo_relacion_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SatTipoRelacionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_tipo_relacion_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<SatUsoCfdi>(entity =>
            {
                entity.HasKey(e => e.SatUsoCdfiIdx)
                    .HasName("PK_sat_uso_cfdi_idx");

                entity.ToTable("sat_uso_cfdi");

                entity.HasIndex(e => e.SatUsoCfdiId, "IX_sat_uso_cfdi_id")
                    .IsUnique();

                entity.Property(e => e.SatUsoCdfiIdx).HasColumnName("sat_uso_cdfi_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Decripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("decripcion");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.FechaFinVigencia)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_fin_vigencia");

                entity.Property(e => e.FechaInicioVigencia)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_inicio_vigencia");

                entity.Property(e => e.Fisica)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("fisica");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Moral)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("moral");

                entity.Property(e => e.RegimenFiscalReceptor)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("regimen_fiscal_receptor");

                entity.Property(e => e.SatUsoCfdiId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sat_uso_cfdi_id");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => e.SettingIdx);

                entity.ToTable("setting");

                entity.HasIndex(e => new { e.StoreId, e.SettingModuleIdi, e.Field }, "IX_setting")
                    .IsUnique();

                entity.Property(e => e.SettingIdx).HasColumnName("setting_idx");

                entity.Property(e => e.AccumulatedTime)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("accumulated_time");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Field)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("field");

                entity.Property(e => e.IsSend).HasColumnName("is_send");

                entity.Property(e => e.IsThereError).HasColumnName("is_there_error");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Response)
                    .HasColumnType("text")
                    .HasColumnName("response");

                entity.Property(e => e.ResponseNodata)
                    .HasColumnType("text")
                    .HasColumnName("response_nodata");

                entity.Property(e => e.SettingId).HasColumnName("setting_id");

                entity.Property(e => e.SettingModuleIdi).HasColumnName("setting_module_idi");

                entity.Property(e => e.Sql)
                    .HasColumnType("text")
                    .HasColumnName("sql");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Type)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Value)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("value");

                entity.HasOne(d => d.SettingModuleIdiNavigation)
                    .WithMany(p => p.Settings)
                    .HasPrincipalKey(p => p.SettingModuleIdi)
                    .HasForeignKey(d => d.SettingModuleIdi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_setting_setting_module");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Settings)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_setting_store");
            });

            modelBuilder.Entity<SettingModule>(entity =>
            {
                entity.HasKey(e => e.SettingModuleIdx);

                entity.ToTable("setting_module");

                entity.HasIndex(e => e.SettingModuleIdi, "IX_setting_module")
                    .IsUnique();

                entity.Property(e => e.SettingModuleIdx).HasColumnName("setting_module_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.SettingModuleIdi).HasColumnName("setting_module_idi");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.HasKey(e => e.ShiftIdx)
                    .HasName("PK_shift_idx");

                entity.ToTable("shift");

                entity.HasIndex(e => new { e.ShiftHeadId, e.StoreId, e.HoseIdi }, "IX_shift_compound")
                    .IsUnique();

                entity.HasIndex(e => e.ShiftId, "IX_shift_id");

                entity.Property(e => e.ShiftIdx).HasColumnName("shift_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CardEmployeeId)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("card_employee_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.EndAmount)
                    .HasColumnType("decimal(13, 1)")
                    .HasColumnName("end_amount");

                entity.Property(e => e.EndAmountElectronic)
                    .HasColumnType("decimal(13, 1)")
                    .HasColumnName("end_amount_electronic");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.EndQuantity)
                    .HasColumnType("decimal(13, 1)")
                    .HasColumnName("end_quantity");

                entity.Property(e => e.EndQuantityElectronic)
                    .HasColumnType("decimal(13, 1)")
                    .HasColumnName("end_quantity_electronic");

                entity.Property(e => e.HoseIdi).HasColumnName("hose_idi");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("price");

                entity.Property(e => e.ShiftHeadId).HasColumnName("shift_head_id");

                entity.Property(e => e.ShiftId).HasColumnName("shift_id");

                entity.Property(e => e.ShiftIdNumber).HasColumnName("shift_id_number");

                entity.Property(e => e.StartAmount)
                    .HasColumnType("decimal(13, 1)")
                    .HasColumnName("start_amount");

                entity.Property(e => e.StartAmountElectronic)
                    .HasColumnType("decimal(13, 1)")
                    .HasColumnName("start_amount_electronic");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.StartQuantity)
                    .HasColumnType("decimal(13, 1)")
                    .HasColumnName("start_quantity");

                entity.Property(e => e.StartQuantityElectronic)
                    .HasColumnType("decimal(13, 1)")
                    .HasColumnName("start_quantity_electronic");

                entity.Property(e => e.StatusRun).HasColumnName("status_run");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(13, 2)")
                    .HasColumnName("total_amount");

                entity.Property(e => e.TotalQuantity)
                    .HasColumnType("decimal(13, 2)")
                    .HasColumnName("total_quantity");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<ShiftDeposit>(entity =>
            {
                entity.HasKey(e => e.ShiftDepositIdx);

                entity.ToTable("shift_deposit");

                entity.Property(e => e.ShiftDepositIdx).HasColumnName("shift_deposit_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("amount");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.IslandIdi).HasColumnName("island_idi");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.ShiftDepositNumber).HasColumnName("shift_deposit_number");

                entity.Property(e => e.ShiftHeadId).HasColumnName("shift_head_id");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<ShiftHead>(entity =>
            {
                entity.HasKey(e => e.ShiftHeadIdx)
                    .HasName("PK_shift_head_idx");

                entity.ToTable("shift_head");

                entity.HasIndex(e => new { e.StoreId, e.ShiftDate, e.ShiftNumber }, "IX_shift_head_compound")
                    .IsUnique();

                entity.HasIndex(e => e.ShiftHeadIdx, "IX_shift_head_id")
                    .IsUnique();

                entity.Property(e => e.ShiftHeadIdx).HasColumnName("shift_head_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.ShiftDate)
                    .HasColumnType("datetime")
                    .HasColumnName("shift_date");

                entity.Property(e => e.ShiftDay).HasColumnName("shift_day");

                entity.Property(e => e.ShiftHeadId).HasColumnName("shift_head_id");

                entity.Property(e => e.ShiftNumber).HasColumnName("shift_number");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<StatusDispenser>(entity =>
            {
                entity.HasKey(e => e.StatusDispenserIdx)
                    .HasName("PK_status_dispenser_idx");

                entity.ToTable("status_dispenser");

                entity.HasIndex(e => e.StatusDispenserIdi, "IX_status_dispenser_idi")
                    .IsUnique();

                entity.Property(e => e.StatusDispenserIdx).HasColumnName("status_dispenser_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ColorStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("color_status");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Logo)
                    .HasColumnType("text")
                    .HasColumnName("logo");

                entity.Property(e => e.LogoName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("logo_name");

                entity.Property(e => e.LogoNameHigh)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("logo_name_high");

                entity.Property(e => e.LogoNameLow)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("logo_name_low");

                entity.Property(e => e.StatusDispenserIdi).HasColumnName("status_dispenser_idi");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<StatusIsland>(entity =>
            {
                entity.HasKey(e => e.StatusIslandIdx)
                    .HasName("PK_status_island_idx");

                entity.ToTable("status_island");

                entity.HasIndex(e => e.StatusRun, "IX_status_island_status_run")
                    .IsUnique();

                entity.Property(e => e.StatusIslandIdx).HasColumnName("status_island_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ColorStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("color_status");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.LogoNameHigh)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("logo_name_high");

                entity.Property(e => e.LogoNameLow)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("logo_name_low");

                entity.Property(e => e.StatusRun).HasColumnName("status_run");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.StoreIdx)
                    .HasName("PK_store_idx");

                entity.ToTable("store");

                entity.HasIndex(e => e.StoreId, "IX_store_id")
                    .IsUnique();

                entity.HasIndex(e => new { e.StoreNumber, e.CompanyId }, "IX_store_number")
                    .IsUnique();

                entity.Property(e => e.StoreIdx).HasColumnName("store_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ApiUrlInvoicing)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("api_url_invoicing");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.ComplementType)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("complement_type")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DistributionType)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("distribution_type")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.ManualCustomerId).HasColumnName("manual_customer_id");

                entity.Property(e => e.ManualVehicleId).HasColumnName("manual_vehicle_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Rfc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rfc");

                entity.Property(e => e.RfcLegalRepresentative)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rfc_legal_representative");

                entity.Property(e => e.Send).HasColumnName("send");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("short_name");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.StoreNumber).HasColumnName("store_number");

                entity.Property(e => e.TimeDifference).HasColumnName("time_difference");

                entity.Property(e => e.TipPercentage).HasColumnName("tip_percentage");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Stores)
                    .HasPrincipalKey(p => p.CompanyId)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_store_company");
            });

            modelBuilder.Entity<StoreAddress>(entity =>
            {
                entity.HasKey(e => e.StoreAddressIdx);

                entity.ToTable("store_address");

                entity.HasIndex(e => new { e.StoreId, e.StoreAddressIdi }, "IX_store_address_compound")
                    .IsUnique();

                entity.Property(e => e.StoreAddressIdx).HasColumnName("store_address_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Colony)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("colony");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.InsideNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("inside_number");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.OutsideNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("outside_number");

                entity.Property(e => e.SatCodigoPostalId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_codigo_postal_id");

                entity.Property(e => e.SatEstadoId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_estado_id");

                entity.Property(e => e.SatLocalidadId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_localidad_id");

                entity.Property(e => e.SatMunicipioId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_municipio_id");

                entity.Property(e => e.SatPaisId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_pais_id");

                entity.Property(e => e.StoreAddressIdi).HasColumnName("store_address_idi");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Street)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("street");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreAddresses)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_store_address_store");
            });

            modelBuilder.Entity<StoreHouse>(entity =>
            {
                entity.HasKey(e => e.StoreHouseIdx);

                entity.ToTable("store_house");

                entity.HasIndex(e => e.StoreHouseId, "IX_store_house")
                    .IsUnique();

                entity.Property(e => e.StoreHouseIdx).HasColumnName("store_house_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.StoreHouseId).HasColumnName("store_house_id");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<StoreHouseDetail>(entity =>
            {
                entity.HasKey(e => e.StoreHouseDetailsIdx)
                    .HasName("PK_store_house_details_1");

                entity.ToTable("store_house_details");

                entity.Property(e => e.StoreHouseDetailsIdx).HasColumnName("store_house_details_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PeriodEnd).HasColumnName("periodEnd");

                entity.Property(e => e.PeriodStart).HasColumnName("periodStart");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("quantity");

                entity.Property(e => e.QuantityEntry)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("quantity_Entry");

                entity.Property(e => e.QuantityExit)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("quantity_Exit");

                entity.Property(e => e.StoreHouseIdDestination).HasColumnName("store_house_id_destination");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<StoreHouseMovement>(entity =>
            {
                entity.HasKey(e => e.StoreHouseMovementIdx);

                entity.ToTable("store_house_movement");

                entity.HasIndex(e => e.StoreHouseMovementId, "IX_store_house_movement")
                    .IsUnique();

                entity.Property(e => e.StoreHouseMovementIdx).HasColumnName("store_house_movement_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.ProviderId).HasColumnName("provider_id");

                entity.Property(e => e.StoreHouseIdDestination).HasColumnName("store_house_id_destination");

                entity.Property(e => e.StoreHouseIdOrigin).HasColumnName("store_house_id_origin");

                entity.Property(e => e.StoreHouseMovementId).HasColumnName("store_house_movement_id");

                entity.Property(e => e.TypeMovementIdx).HasColumnName("type_movement_idx");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.StoreHouseIdDestinationNavigation)
                    .WithMany(p => p.StoreHouseMovements)
                    .HasPrincipalKey(p => p.StoreHouseId)
                    .HasForeignKey(d => d.StoreHouseIdDestination)
                    .HasConstraintName("FK_store_house_movement_store_house");

                entity.HasOne(d => d.TypeMovementIdxNavigation)
                    .WithMany(p => p.StoreHouseMovements)
                    .HasForeignKey(d => d.TypeMovementIdx)
                    .HasConstraintName("FK_store_house_movement_type_movement");
            });

            modelBuilder.Entity<StoreHouseMovementDetail>(entity =>
            {
                entity.HasKey(e => e.StoreHouseMovementDetailIdx);

                entity.ToTable("store_house_movement_detail");

                entity.Property(e => e.StoreHouseMovementDetailIdx).HasColumnName("storeHouseMovementDetailIdx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("location");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.QuantityEntry)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("quantityEntry");

                entity.Property(e => e.QuantityExit)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("quantityExit");

                entity.Property(e => e.QuantityTrasfer)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("quantityTrasfer");

                entity.Property(e => e.StoreHouseMovementId).HasColumnName("storeHouseMovementId");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.StoreHouseMovement)
                    .WithMany(p => p.StoreHouseMovementDetails)
                    .HasPrincipalKey(p => p.StoreHouseMovementId)
                    .HasForeignKey(d => d.StoreHouseMovementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_store_house_movement_detail_store_house_movement1");
            });

            modelBuilder.Entity<StoreInvoiceSerie>(entity =>
            {
                entity.HasKey(e => e.StoreInvoiceSerieIdx)
                    .HasName("PK_store_invoice_serie_idx");

                entity.ToTable("store_invoice_serie");

                entity.HasIndex(e => e.StoreInvoiceSerieIdx, "IX_store_invoice_serie_id")
                    .IsUnique();

                entity.Property(e => e.StoreInvoiceSerieIdx).HasColumnName("store_invoice_serie_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.InvoiceSerieId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("invoice_serie_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<StoreNetwork>(entity =>
            {
                entity.HasKey(e => e.StoreNetworkIdx)
                    .HasName("PK_store_network_idx");

                entity.ToTable("store_network");

                entity.HasIndex(e => e.StoreNetworkId, "IX_store_network_id")
                    .IsUnique();

                entity.Property(e => e.StoreNetworkIdx).HasColumnName("store_network_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.StoreNetworkId).HasColumnName("store_network_id");

                entity.Property(e => e.StoreNetworkNumber).HasColumnName("store_network_number");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<StoreNetworkDetail>(entity =>
            {
                entity.HasKey(e => e.StoreNetworkDetailIdx)
                    .HasName("PK_store_network_detail_idx");

                entity.ToTable("store_network_detail");

                entity.HasIndex(e => new { e.StoreNetworkId, e.StoreId }, "IX_store_network_detail_compound")
                    .IsUnique();

                entity.Property(e => e.StoreNetworkDetailIdx).HasColumnName("store_network_detail_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.StoreNetworkId).HasColumnName("store_network_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<StoreSat>(entity =>
            {
                entity.HasKey(e => e.StoreSatIdx);

                entity.ToTable("store_sat");

                entity.HasIndex(e => e.StoreId, "IX_store_sat_store_id")
                    .IsUnique();

                entity.Property(e => e.StoreSatIdx).HasColumnName("store_sat_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CrePermission)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("cre_permission");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.JsonClaveInstalacionId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("json_clave_instalacion_id");

                entity.Property(e => e.JsonTipoComplementoId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_complemento_id");

                entity.Property(e => e.JsonTipoDistribucionId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_distribucion_id");

                entity.Property(e => e.JsonTipoMedicionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_medicion_id");

                entity.Property(e => e.JsonTipoReporteId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("json_tipo_reporte_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.RfcLegalRepresentative)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rfc_legal_representative");

                entity.Property(e => e.RfcSystemSupplier)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rfc_system_supplier");

                entity.Property(e => e.SatDescriptionInstallation)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("sat_description_installation");

                entity.Property(e => e.SatInstallationKey)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("sat_Installation_Key");

                entity.Property(e => e.SatPermission)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sat_permission");

                entity.Property(e => e.SatReportType)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("sat_report_type");

                entity.Property(e => e.SatRfcSupplier)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("sat_rfc_supplier");

                entity.Property(e => e.SatSystemMeasureTank)
                    .HasMaxLength(10)
                    .HasColumnName("sat_system_measure_tank")
                    .IsFixedLength();

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.SystemDescriptionInstallation)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("system_description_installation");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Store)
                    .WithOne(p => p.StoreSat)
                    .HasPrincipalKey<Store>(p => p.StoreId)
                    .HasForeignKey<StoreSat>(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_store_sat_store");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierIdx)
                    .HasName("PK_supplier_idx");

                entity.ToTable("supplier");

                entity.HasIndex(e => e.SupplierId, "IX_supplier")
                    .IsUnique();

                entity.Property(e => e.SupplierIdx).HasColumnName("supplier_idx");

                entity.Property(e => e.AbbreviatedName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("abbreviated_name");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Cellular)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cellular");

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("contact_person");

                entity.Property(e => e.Curp)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("curp");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IsSupplierOfFuel).HasColumnName("is_supplier_of_fuel");

                entity.Property(e => e.IsSupplierOfTransport).HasColumnName("is_supplier_of_transport");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MotherLastname)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("mother_lastname");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Rfc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rfc");

                entity.Property(e => e.SatRegimenFiscalId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sat_regimen_fiscal_id");

                entity.Property(e => e.SupplierAddressIdi).HasColumnName("supplier_address_idi");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.SupplierNumber).HasColumnName("supplier_number");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.SatRegimenFiscal)
                    .WithMany(p => p.Suppliers)
                    .HasPrincipalKey(p => p.SatRegimenFiscalId)
                    .HasForeignKey(d => d.SatRegimenFiscalId)
                    .HasConstraintName("FK_supplier_sat_regimen_fiscal");
            });

            modelBuilder.Entity<SupplierAddress>(entity =>
            {
                entity.HasKey(e => e.SupplierAddressIdx);

                entity.ToTable("supplier_address");

                entity.Property(e => e.SupplierAddressIdx).HasColumnName("supplier_address_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Colony)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("colony");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.InsideNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("inside_number");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.OutsideNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("outside_number");

                entity.Property(e => e.SatCodigoPostalId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_codigo_postal_id");

                entity.Property(e => e.SatEstadoId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_estado_id");

                entity.Property(e => e.SatLocalidadId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_localidad_id");

                entity.Property(e => e.SatMunicipioId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_municipio_id");

                entity.Property(e => e.SatPaisId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sat_pais_id");

                entity.Property(e => e.Street)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("street");

                entity.Property(e => e.SupplierAddressIdi).HasColumnName("supplier_address_idi");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SupplierAddresses)
                    .HasPrincipalKey(p => p.SupplierId)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_supplier_address_supplier");
            });

            modelBuilder.Entity<SupplierFuel>(entity =>
            {
                entity.HasKey(e => e.SupplierFuelIdx)
                    .HasName("PK_supplier_fuel_idx");

                entity.ToTable("supplier_fuel");

                entity.HasIndex(e => new { e.StoreId, e.SupplierFuelIdi, e.SupplierId }, "IX_supplier_fuel_compound")
                    .IsUnique();

                entity.HasIndex(e => new { e.SupplierId, e.StoreId }, "IX_supplier_fuel_id")
                    .IsUnique();

                entity.Property(e => e.SupplierFuelIdx).HasColumnName("supplier_fuel_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BrandName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("brand_name");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.FuelPermission)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("fuel_permission");

                entity.Property(e => e.ImportPermission)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("import_permission");

                entity.Property(e => e.IsConsignment)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("is_consignment")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Rfc)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("rfc");

                entity.Property(e => e.StorageAndDistributionPermission)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("storage_and_distribution_permission");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.SupplierFuelIdi).HasColumnName("supplier_fuel_idi");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.SupplierType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("supplier_type");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SupplierFuels)
                    .HasPrincipalKey(p => p.SupplierId)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_supplier_fuel_supplier");
            });

            modelBuilder.Entity<SupplierTransport>(entity =>
            {
                entity.HasKey(e => e.SupplierTransportIdx);

                entity.ToTable("supplier_transport");

                entity.HasIndex(e => new { e.SupplierId, e.StoreId, e.SupplierTransportIdx }, "IX_supplier_transport")
                    .IsUnique();

                entity.HasIndex(e => new { e.StoreId, e.SupplierTransportIdi }, "IX_supplier_transport_1")
                    .IsUnique();

                entity.Property(e => e.SupplierTransportIdx).HasColumnName("supplier_transport_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BrandName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("brand_name");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Rfc)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("rfc");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.SupplierTransportIdi).HasColumnName("supplier_transport_idi");

                entity.Property(e => e.TransportPermission)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("transport_permission");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SupplierTransports)
                    .HasPrincipalKey(p => p.SupplierId)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_supplier_transport_supplier");
            });

            modelBuilder.Entity<SupplierTransportRegister>(entity =>
            {
                entity.HasKey(e => e.SupplierTransportRegisterIdx);

                entity.ToTable("supplier_transport_register");

                entity.Property(e => e.SupplierTransportRegisterIdx).HasColumnName("supplier_transport_register_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AmountOfDiscount)
                    .HasColumnType("decimal(12, 3)")
                    .HasColumnName("amount_of_discount");

                entity.Property(e => e.AmountPerCapacity)
                    .HasColumnType("decimal(9, 3)")
                    .HasColumnName("amount_per_capacity");

                entity.Property(e => e.AmountPerFee)
                    .HasColumnType("decimal(9, 3)")
                    .HasColumnName("amount_per_fee");

                entity.Property(e => e.AmountPerService)
                    .HasColumnType("decimal(12, 3)")
                    .HasColumnName("amount_per_service");

                entity.Property(e => e.AmountPerUse)
                    .HasColumnType("decimal(9, 3)")
                    .HasColumnName("amount_per_use");

                entity.Property(e => e.AmountPerVolume)
                    .HasColumnType("decimal(9, 3)")
                    .HasColumnName("amount_per_volume");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.SupplierTransportRegisterId).HasColumnName("supplier_transport_register_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Tank>(entity =>
            {
                entity.HasKey(e => e.TankIdx)
                    .HasName("PK_tank_idx");

                entity.ToTable("tank");

                entity.HasIndex(e => new { e.StoreId, e.TankIdi }, "IX_tank_compound")
                    .IsUnique();

                entity.Property(e => e.TankIdx).HasColumnName("tank_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CalculateQuantityWithTable)
                    .HasColumnName("calculate_quantity_with_table")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CapacityGastalon)
                    .HasColumnType("decimal(9, 3)")
                    .HasColumnName("capacity_gastalon")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CapacityMinimumOperating).HasColumnName("capacity_minimum_operating");

                entity.Property(e => e.CapacityOperational).HasColumnName("capacity_operational");

                entity.Property(e => e.CapacityTotal).HasColumnName("capacity_total");

                entity.Property(e => e.CapacityUseful).HasColumnName("capacity_useful");

                entity.Property(e => e.CommPercentage).HasColumnName("comm_percentage");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DiameterOrWidth)
                    .HasColumnType("decimal(12, 5)")
                    .HasColumnName("diameter_or_width");

                entity.Property(e => e.EnableGetInventory).HasColumnName("enable_get_inventory");

                entity.Property(e => e.Fondage).HasColumnName("fondage");

                entity.Property(e => e.Height)
                    .HasColumnType("decimal(12, 5)")
                    .HasColumnName("height");

                entity.Property(e => e.HeightNotFuel)
                    .HasColumnType("decimal(12, 5)")
                    .HasColumnName("height_not_fuel");

                entity.Property(e => e.HeightStart)
                    .HasColumnType("decimal(12, 5)")
                    .HasColumnName("height_start");

                entity.Property(e => e.Length)
                    .HasColumnType("decimal(12, 5)")
                    .HasColumnName("length");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MultiplicationFactor)
                    .HasColumnType("decimal(12, 5)")
                    .HasColumnName("multiplication_factor")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Nuevocampo)
                    .HasMaxLength(10)
                    .HasColumnName("nuevocampo")
                    .IsFixedLength();

                entity.Property(e => e.PortIdi).HasColumnName("port_idi");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ResponseInventoryIn)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("response_inventory_in");

                entity.Property(e => e.SatDateCalibration)
                    .HasColumnType("datetime")
                    .HasColumnName("sat_date_calibration");

                entity.Property(e => e.SatDescriptionMeasurement)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_description_measurement");

                entity.Property(e => e.SatPercentageUncertaintyMeasurement).HasColumnName("sat_percentage_uncertainty_measurement");

                entity.Property(e => e.SatTankType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_tank_type");

                entity.Property(e => e.SatTypeMeasurement)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_type_measurement");

                entity.Property(e => e.SatTypeMediumStorage)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("sat_type_medium_storage");

                entity.Property(e => e.StatusRes)
                    .HasMaxLength(100)
                    .HasColumnName("status_res");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TankBrandId).HasColumnName("tank_brand_id");

                entity.Property(e => e.TankCpuAddress).HasColumnName("tank_cpu_address");

                entity.Property(e => e.TankCpuAddressNew)
                    .HasColumnName("tank_cpu_address_new")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TankIdi).HasColumnName("tank_idi");

                entity.Property(e => e.TankShapeId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tank_shape_id");

                entity.Property(e => e.TankTypeId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tank_type_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UtilityPercentaje).HasColumnName("utility_percentaje");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Tanks)
                    .HasPrincipalKey(p => p.StoreId)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tank_store");

                entity.HasOne(d => d.TankBrand)
                    .WithMany(p => p.Tanks)
                    .HasPrincipalKey(p => p.TankBrandId)
                    .HasForeignKey(d => d.TankBrandId)
                    .HasConstraintName("FK_tank_tank_brand");

                entity.HasOne(d => d.Port)
                    .WithMany(p => p.Tanks)
                    .HasPrincipalKey(p => new { p.StoreId, p.PortIdi })
                    .HasForeignKey(d => new { d.StoreId, d.PortIdi })
                    .HasConstraintName("FK_tank_port");
            });

            modelBuilder.Entity<TankBrand>(entity =>
            {
                entity.HasKey(e => e.TankBrandIdx);

                entity.ToTable("tank_brand");

                entity.HasIndex(e => e.TankBrandId, "IX_tank_brand")
                    .IsUnique();

                entity.Property(e => e.TankBrandIdx).HasColumnName("tank_brand_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.TankBrandId).HasColumnName("tank_brand_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<TankCalibrationPoint>(entity =>
            {
                entity.HasKey(e => e.TankCalibrationPointsIdx)
                    .HasName("PK_tank_calibration_points_idx");

                entity.ToTable("tank_calibration_points");

                entity.HasIndex(e => new { e.StoreId, e.TankIdi, e.PointsId }, "IX_tank_calibration_points_compound")
                    .IsUnique();

                entity.Property(e => e.TankCalibrationPointsIdx).HasColumnName("tank_calibration_points_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Height)
                    .HasColumnType("decimal(12, 5)")
                    .HasColumnName("height");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PointsId).HasColumnName("points_id");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(12, 5)")
                    .HasColumnName("quantity");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TankIdi).HasColumnName("tank_idi");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<TankInprogress>(entity =>
            {
                entity.HasKey(e => e.TankInprogressIdx)
                    .HasName("PK_tank_inprogress_idx");

                entity.ToTable("tank_inprogress");

                entity.HasIndex(e => new { e.StoreId, e.TankIdi }, "IX_tank_inprogress_compound")
                    .IsUnique();

                entity.Property(e => e.TankInprogressIdx).HasColumnName("tank_inprogress_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.CountFalse).HasColumnName("count_false");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Difference)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("difference");

                entity.Property(e => e.Height)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("height");

                entity.Property(e => e.HeightWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("height_water");

                entity.Property(e => e.IsInventoryInprogress).HasColumnName("is_inventory_inprogress");

                entity.Property(e => e.LastDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_date");

                entity.Property(e => e.LastVolume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("last_volume");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TankIdi).HasColumnName("tank_idi");

                entity.Property(e => e.Temperature)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("temperature");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Volume)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume");

                entity.Property(e => e.VolumeTc)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume_tc");

                entity.Property(e => e.VolumeWater)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("volume_water");
            });

            modelBuilder.Entity<TankShape>(entity =>
            {
                entity.HasKey(e => e.TankShapeIdx)
                    .HasName("PK_tank_shape_idx");

                entity.ToTable("tank_shape");

                entity.HasIndex(e => e.TankShapeId, "IX_tank_shape_id")
                    .IsUnique();

                entity.Property(e => e.TankShapeIdx).HasColumnName("tank_shape_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.TankShapeId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tank_shape_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<TankType>(entity =>
            {
                entity.HasKey(e => e.TankTypeIdx)
                    .HasName("PK_tank_type_idx");

                entity.ToTable("tank_type");

                entity.HasIndex(e => e.TankTypeId, "IX_tank_type_id")
                    .IsUnique();

                entity.Property(e => e.TankTypeIdx).HasColumnName("tank_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.TankTypeId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tank_type_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<TaskType>(entity =>
            {
                entity.HasKey(e => e.TaskTypeIdx)
                    .HasName("PK_TipoTarea");

                entity.ToTable("task_type");

                entity.Property(e => e.TaskTypeIdx).HasColumnName("task_type_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.EventTypeId).HasColumnName("event_type_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.TaskTypeId).HasColumnName("task_type_id");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<TransportMediumnCustom>(entity =>
            {
                entity.HasKey(e => e.TransportMediumnCustomsIdx);

                entity.ToTable("transport_mediumn_customs");

                entity.HasIndex(e => e.TransportMediumnCustomsId, "IX_transport_mediumn_customs_id")
                    .IsUnique();

                entity.Property(e => e.TransportMediumnCustomsIdx).HasColumnName("transport_mediumn_customs_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.TransportMediumn)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("transport_mediumn");

                entity.Property(e => e.TransportMediumnCustomsId).HasColumnName("transport_mediumn_customs_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<TypeMovement>(entity =>
            {
                entity.HasKey(e => e.TypeMovementIdx);

                entity.ToTable("type_movement");

                entity.Property(e => e.TypeMovementIdx).HasColumnName("type_movement_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<UserDateCreate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user_Date__Create");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Update)
                    .HasColumnType("datetime")
                    .HasColumnName("update");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("userName");
            });

            modelBuilder.Entity<UserDateCreate1>(entity =>
            {
                entity.HasKey(e => e.UserdateCreate);

                entity.ToTable("user_Date_Create");

                entity.Property(e => e.UserdateCreate).HasColumnName("userdateCreate");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Update)
                    .HasColumnType("datetime")
                    .HasColumnName("update");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("userName");
            });

            modelBuilder.Entity<UserStore>(entity =>
            {
                entity.HasKey(e => e.UserStoreIdx);

                entity.ToTable("user_store");

                entity.Property(e => e.UserStoreIdx).HasColumnName("user_store_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userId");
            });

            modelBuilder.Entity<ValidateType>(entity =>
            {
                entity.ToTable("validate_type");

                entity.HasIndex(e => e.ValidateTypeIdx, "IX_validate_type")
                    .IsUnique();

                entity.HasIndex(e => e.ValidateTypeId, "IX_validate_type_id")
                    .IsUnique();

                entity.Property(e => e.ValidateTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("validate_type_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.ValidateTypeIdx)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("validate_type_idx");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.VehicleIdx)
                    .HasName("PK_Vehiculo_idx");

                entity.ToTable("vehicle");

                entity.HasIndex(e => new { e.CustomerId, e.VehicleNumber }, "IX_Vehiculo")
                    .IsUnique();

                entity.Property(e => e.VehicleIdx).HasColumnName("vehicle_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AskOdometer).HasColumnName("ask_odometer");

                entity.Property(e => e.AskOdr).HasColumnName("ask_odr");

                entity.Property(e => e.AskOperator).HasColumnName("ask_operator");

                entity.Property(e => e.AskPin).HasColumnName("ask_pin");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.DayBalance).HasColumnName("day_balance");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Detail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("detail");

                entity.Property(e => e.EnableDay1).HasColumnName("enable_day_1");

                entity.Property(e => e.EnableDay2).HasColumnName("enable_day_2");

                entity.Property(e => e.EnableDay3).HasColumnName("enable_day_3");

                entity.Property(e => e.EnableDay4).HasColumnName("enable_day_4");

                entity.Property(e => e.EnableDay5).HasColumnName("enable_day_5");

                entity.Property(e => e.EnableDay6).HasColumnName("enable_day_6");

                entity.Property(e => e.EnableDay7).HasColumnName("enable_day_7");

                entity.Property(e => e.EnableDayBalance).HasColumnName("enable_day_balance");

                entity.Property(e => e.EnableHour1).HasColumnName("enable_hour_1");

                entity.Property(e => e.EnableHour2).HasColumnName("enable_hour_2");

                entity.Property(e => e.EnableHour3).HasColumnName("enable_hour_3");

                entity.Property(e => e.EnableMonthBalance).HasColumnName("enable_month_balance");

                entity.Property(e => e.EnableWeekBalance).HasColumnName("enable_week_balance");

                entity.Property(e => e.EndHour1)
                    .HasColumnType("datetime")
                    .HasColumnName("end_hour_1");

                entity.Property(e => e.EndHour2)
                    .HasColumnType("datetime")
                    .HasColumnName("end_hour_2");

                entity.Property(e => e.EndHour3)
                    .HasColumnType("datetime")
                    .HasColumnName("end_hour_3");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.MonthBalance).HasColumnName("month_balance");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Plate)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("plate");

                entity.Property(e => e.Series)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("series");

                entity.Property(e => e.StartHour1)
                    .HasColumnType("datetime")
                    .HasColumnName("start_hour_1");

                entity.Property(e => e.StartHour2)
                    .HasColumnType("datetime")
                    .HasColumnName("start_hour_2");

                entity.Property(e => e.StartHour3)
                    .HasColumnType("datetime")
                    .HasColumnName("start_hour_3");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.ValidateOdometer).HasColumnName("validate_odometer");

                entity.Property(e => e.ValidateOdr).HasColumnName("validate_odr");

                entity.Property(e => e.ValidateOperator).HasColumnName("validate_operator");

                entity.Property(e => e.ValidateTypeId).HasColumnName("validate_type_id");

                entity.Property(e => e.VehicleBrandTypeId).HasColumnName("vehicle_brand_type_id");

                entity.Property(e => e.VehicleColorId).HasColumnName("vehicle_color_id");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.Property(e => e.VehicleNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("vehicle_number");

                entity.Property(e => e.WeekBalance).HasColumnName("week_balance");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Vehicles)
                    .HasPrincipalKey(p => p.CustomerId)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_vehicle_customer");
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.HasKey(e => e.VersionIdx)
                    .HasName("PK_version_idx");

                entity.ToTable("version");

                entity.HasIndex(e => new { e.SystemId, e.VersionId, e.RevisionId }, "IX_version_compound")
                    .IsUnique();

                entity.Property(e => e.VersionIdx).HasColumnName("version_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Hash512)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("hash_512");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.RevisionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("revision_id");

                entity.Property(e => e.SystemId).HasColumnName("system_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.Upgrade)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("upgrade");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserNameCheck)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_name_check");

                entity.Property(e => e.VersionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("version_date");

                entity.Property(e => e.VersionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("version_id");
            });

            modelBuilder.Entity<Volumetric>(entity =>
            {
                entity.HasKey(e => e.VolumetricIdx)
                    .HasName("Pk_volumetric_idx");

                entity.ToTable("volumetric");

                entity.HasIndex(e => new { e.StoreId, e.TypeOfPeriod, e.Date }, "IX_volumetric_compound")
                    .IsUnique();

                entity.Property(e => e.VolumetricIdx).HasColumnName("volumetric_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EnableGenerated).HasColumnName("enable_generated");

                entity.Property(e => e.EnableSend).HasColumnName("enable_send");

                entity.Property(e => e.IsGenarated).HasColumnName("is_genarated");

                entity.Property(e => e.IsSent).HasColumnName("is_sent");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.NameFile)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name_file");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.TypeOfPeriod)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("type_of_period");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasKey(e => e.VoucherIdx)
                    .HasName("PK_voucher_idx");

                entity.ToTable("voucher");

                entity.HasIndex(e => new { e.StoreNetworkId, e.VoucherId }, "IX_voucher_compound")
                    .IsUnique();

                entity.Property(e => e.VoucherIdx).HasColumnName("voucher_idx");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("amount");

                entity.Property(e => e.AmountSpent)
                    .HasColumnType("decimal(11, 4)")
                    .HasColumnName("amount_spent");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.DateConsumed)
                    .HasColumnType("datetime")
                    .HasColumnName("date_consumed");

                entity.Property(e => e.DateExpiration)
                    .HasColumnType("datetime")
                    .HasColumnName("date_expiration");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.IsConsumed).HasColumnName("is_consumed");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.SaleOrderId).HasColumnName("sale_order_id");

                entity.Property(e => e.StoreNetworkId).HasColumnName("store_network_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherId).HasColumnName("voucher_id");

                entity.Property(e => e.VoucherNumber).HasColumnName("voucher_number");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
