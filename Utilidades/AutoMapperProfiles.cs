using APIControlNet.DTOs;
using APIControlNet.Models;
using AutoMapper;

namespace APIControlNet.Utilidades
{
    
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CompanyCustomerDTO, CompanyCustomer>();
            CreateMap<CompanyCustomer, CompanyCustomerDTO>();

            CreateMap<NetgroupCustomerDTO, NetgroupCustomer>();
            CreateMap<NetgroupCustomer, NetgroupCustomerDTO>();

            CreateMap<NetgroupUserTypeDTO, NetgroupUserType>();
            CreateMap<NetgroupUserType, NetgroupUserTypeDTO>();

            CreateMap<PresetTypeDTO, PresetType>();
            CreateMap<PresetType, PresetTypeDTO>();

            CreateMap<CustomerLimitDTO, CustomerLimit>();
            CreateMap<CustomerLimit, CustomerLimitDTO>();

            CreateMap<OdrDTO, Odr>();
            CreateMap<Odr, OdrDTO>();

            CreateMap<NetgroupNetDetailDTO, NetgroupNetDetail>();
            CreateMap<NetgroupNetDetail, NetgroupNetDetailDTO>();

            CreateMap<NetgroupNetDTO, NetgroupNet>();
            CreateMap<NetgroupNet, NetgroupNetDTO>();

            CreateMap<NetgroupUserDTO, NetgroupUser>();
            CreateMap<NetgroupUser, NetgroupUserDTO>();

            CreateMap<NetgroupStoreDTO, NetgroupStore>();
            CreateMap<NetgroupStore, NetgroupStoreDTO>();

            CreateMap<NetgroupDTO, Netgroup>();
            CreateMap<Netgroup, NetgroupDTO>();

            CreateMap<ValidateTypeDTO, ValidateType>();
            CreateMap<ValidateType, ValidateTypeDTO>();

            CreateMap<ProductPriceDTO, ProductPrice>();
            CreateMap<ProductPrice, ProductPriceDTO>();

            CreateMap<SqlReportDTO, SqlReport>();
            CreateMap<SqlReport, SqlReportDTO>();

            CreateMap<SupplierDTO, Supplier>();
            CreateMap<Supplier, SupplierDTO>();

            CreateMap<UserStoreDTO, UserStore>();
            CreateMap<UserStore, UserStoreDTO>();

            CreateMap<JsonSubclaveProductoDTO, JsonSubclaveProducto>();
            CreateMap<JsonSubclaveProducto, JsonSubclaveProductoDTO>();

            CreateMap<JsonClaveProductoDTO, JsonClaveProducto>();
            CreateMap<JsonClaveProducto, JsonClaveProductoDTO>();

            CreateMap<InventoryInSaleOrderDTO, InventoryInSaleOrder>();
            CreateMap<InventoryInSaleOrder, InventoryInSaleOrderDTO>();

            CreateMap<PetitionCustomDTO, PetitionCustom>();
            CreateMap<PetitionCustom, PetitionCustomDTO>();

            CreateMap<InventoryInDocumentDTO, InventoryInDocument>();
            CreateMap<InventoryInDocument, InventoryInDocumentDTO>();
            CreateMap<InvoiceDTO, Invoice>();
            CreateMap<Invoice, InvoiceDTO>();
            CreateMap<InvoiceDetailDTO, InvoiceDetail>();
            CreateMap<InvoiceDetail, InvoiceDetailDTO>();
            //CreateMap<InvInDoc_InvoiceDTO, InvInDoc_InvoiceDTO>();  //empaquetada

            CreateMap<CompanyDTO, Company>();
            CreateMap<Company, CompanyDTO>();   

            CreateMap<Store, StoreDTO>();
            CreateMap<StoreDTO, Store>();

            CreateMap<DispensaryDTO, Dispensary>();
            CreateMap<Dispensary, DispensaryDTO>();


            CreateMap<DispensaryBrand, DispensaryBrandDTO>();
            CreateMap<DispensaryBrandDTO, DispensaryBrand>();

            CreateMap<LoadPosition, LoadPositionDTO>();
            CreateMap<LoadPositionDTO, LoadPosition>();

            CreateMap<IslandDTO, Island>();
            CreateMap<Island, IslandDTO>();

            CreateMap<HoseDTO, Hose>();
            CreateMap<Hose, HoseDTO>();

            CreateMap<SatFormaPagoDTO, SatFormaPago>();
            CreateMap<SatFormaPago, SatFormaPagoDTO>();

            CreateMap<StoreHouseMovement, StoreHouseMovementDTO>().ReverseMap();
            CreateMap<StoreHouseMovementDTO, StoreHouseMovement>();

            CreateMap<StoreHouseMovementDetail, StoreHouseMovementDetailDTO>();
            CreateMap<StoreHouseMovementDetailDTO, StoreHouseMovementDetail>();
            

            CreateMap<StoreHouseDTO, StoreHouse>();
            CreateMap<StoreHouse, StoreHouseDTO>();

            CreateMap<TypeMovementDTO, TypeMovement>();
            CreateMap<TypeMovement, TypeMovementDTO>();

            CreateMap<StoreHouseDetailDTO, StoreHouseDetail>();
            CreateMap<StoreHouseDetail, StoreHouseDetailDTO>().ReverseMap();

            CreateMap<ComparaDTO, StoreHouseDetail>();
            CreateMap<StoreHouseDetail, ComparaDTO>();


            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>();

            CreateMap<CustomerType, CustomerTypeDTO>();
            CreateMap<CustomerTypeDTO, CustomerType>();


            CreateMap<Port, PortDTO>();
            CreateMap<PortDTO, Port>();

            CreateMap<PortType, PortTypeDTO>();
            CreateMap<PortTypeDTO, PortType>();

            CreateMap<Setting, SettingDTO>();
            CreateMap<SettingDTO,  Setting>();

            CreateMap<SettingModule, SettingModuleDTO>();
            CreateMap<SettingModuleDTO, SettingModuleDTO>();

            CreateMap<Folio, FolioDTO>();
            CreateMap<FolioDTO, Folio>();

            CreateMap<Tank, TankDTO>();
            CreateMap<TankDTO, Tank>();

            CreateMap<TankBrand, TankBrandDTO>();
            CreateMap<TankBrandDTO, TankBrand>();

            CreateMap<Binnacle, BinnacleDTO>();
            CreateMap<BinnacleDTO, Binnacle>();

            CreateMap<Vehicle, VehicleDTO>();
            CreateMap<VehicleDTO, Vehicle>();

            CreateMap<Product, ProductDTO>();
              //  .ForMember(x => x.SatClaveUnidadId, option => option.Ignore());
            CreateMap<ProductDTO, Product>();

            CreateMap<ProductSat, ProductSatDTO>();
            CreateMap<ProductSatDTO, ProductSat>();

            CreateMap<ProductStore, ProductStoreDTO>();
            CreateMap<ProductStoreDTO, ProductStore>();

            CreateMap<ProductCategory, ProductCategoryDTO>();
            CreateMap<ProductCategoryDTO, ProductCategory>();

            CreateMap<SatClaveProductoServicio, SatClaveProductoServicioDTO>();
              //  .ForMember(x => x.SatClaveProductoServicioId, option => option.Ignore());
            CreateMap<SatClaveProductoServicioDTO, SatClaveProductoServicio>();

            CreateMap<SatClaveUnidad, SatClaveUnidadDTO>();
            CreateMap<SatClaveUnidadDTO, SatClaveUnidad>();

            CreateMap<SatRegimenFiscalDTO, SatRegimenFiscal>();
            CreateMap<SatRegimenFiscal, SatRegimenFiscalDTO>();

            CreateMap<SatUsoCfdiDTO, SatUsoCfdi>();
            CreateMap<SatUsoCfdi, SatUsoCfdiDTO>();

            CreateMap<CustomerAddressDTO, CustomerAddress>();
            CreateMap<CustomerAddress, CustomerAddressDTO>();

            CreateMap<NetgroupDTO, Netgroup>();
            CreateMap<Netgroup, NetgroupDTO>();

            CreateMap<NetgroupUserDTO, NetgroupUser>();
            CreateMap<NetgroupUser, NetgroupUserDTO>();

            CreateMap<SaleOrderDTO, SaleOrder>();
            CreateMap<SaleOrder, SaleOrderDTO>();

            CreateMap<SaleSuborderDTO, SaleSuborder>();
            CreateMap<SaleSuborder, SaleSuborderDTO>();

            CreateMap<InventoryInDTO, InventoryIn>();
            CreateMap<InventoryIn, InventoryInDTO>();

            CreateMap<EmployeeDTO, Employee>();
            CreateMap<Employee, EmployeeDTO>();

            CreateMap<CardDTO, Card>();
            CreateMap<Card, CardDTO>();

            CreateMap<CardTypeDTO, CardType>();
            CreateMap<CardType, CardTypeDTO>();

            CreateMap<PointSaleDTO, PointSale>();
            CreateMap<PointSale, PointSaleDTO>();

            CreateMap<DailySummaryDTO, DailySummary>();
            CreateMap<DailySummary, DailySummaryDTO>();

            CreateMap<MonthlySummaryDTO, MonthlySummary>();
            CreateMap<MonthlySummary, MonthlySummaryDTO>();

            CreateMap<VolumetricDTO, Volumetric>();
            CreateMap<Volumetric, VolumetricDTO>();

            CreateMap<InventoryInDocumentDTO, InventoryInDocument>();
            CreateMap<InventoryInDocument, InventoryInDocumentDTO>();

            CreateMap<BinnacleDTO, Binnacle>();
            CreateMap<Binnacle, BinnacleDTO>();

            CreateMap<SupplierFuel, SupplierFuelDTO>();
            CreateMap<SupplierFuelDTO, SupplierFuel>();

            CreateMap<SupplierTransportDTO, SupplierTransport>();
            CreateMap<SupplierTransport, SupplierTransportDTO>();

            CreateMap<CompanyAddress, CompanyAddressDTO>();
            CreateMap<CompanyAddressDTO, CompanyAddress>();

            CreateMap<SatMunicipioDTO, SatMunicipio>();
            CreateMap<SatMunicipio, SatMunicipioDTO>();

            CreateMap<SatEstadoDTO, SatEstado>();
            CreateMap<SatEstado, SatEstadoDTO>();

            CreateMap<SatCodigoPostal, SatCodigoPostalDTO>();
            CreateMap<SatCodigoPostalDTO, SatCodigoPostal>();

            CreateMap<StoreAddressDTO, StoreAddress>();
            CreateMap<StoreAddress, StoreAddressDTO>();

            CreateMap<StoreSatDTO, StoreSat>();
            CreateMap<StoreSat, StoreSatDTO>();

            CreateMap<SupplierAddressDTO, SupplierAddress>();
            CreateMap<SupplierAddress, SupplierAddressDTO>();

            CreateMap<StatusDispenserDTO, StatusDispenser>();
            CreateMap<StatusDispenser, StatusDispenserDTO>();

            CreateMap<LastInventoryDTO, LastInventory>();
            CreateMap<LastInventory, LastInventoryDTO>();

            CreateMap<LoadPositionResponseDTO, LoadPositionResponse>();
            CreateMap<LoadPositionResponse, LoadPositionResponseDTO>();

            CreateMap<VersionDTO, Models.Version>();
            CreateMap<Models.Version, VersionDTO>();

            CreateMap<SupplierTransportRegisterDTO, SupplierTransportRegister>();
            CreateMap<SupplierTransportRegister, SupplierTransportRegisterDTO>();

            CreateMap<SatTipoComprobanteDTO, SatTipoComprobante>();
            CreateMap<SatTipoComprobante, SatTipoComprobanteDTO>();

            CreateMap<InvoiceComparisonDTO, InvoiceComparison>();
            CreateMap<InvoiceComparison, InvoiceComparisonDTO>();

            //CreateMap<AspNetRolesDTO, AspNetRole>()
            //    .ForMember(AspNetRole => AspNetRole.ChPageRols, opciones => opciones.MapFrom(MapChpagRols));
            //CreateMap<AspNetRolesDTO, AspNetRole>();
        }


        //private List<ChPageRol> MapChpagRols(AspNetRolesDTO aspNetRolesDTO, AspNetRole aspNetRole)
        //{
        //    var resultado = new List<ChPageRol>();  

        //    if (aspNetRolesDTO.ChPagesIds == null) { return resultado; }

        //    foreach (var chpageid in aspNetRolesDTO.ChPagesIds)
        //    {
        //        resultado.Add(new ChPageRol() { IdPage = chpageid}); 
        //    }
        //    return resultado;
        //}

    }
}
