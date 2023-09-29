using APIControlNet.Models;
using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class StoreDTO
    {
        public StoreDTO()
        {
            Dispensaries = new HashSet<DispensaryDTO>();
            Employees = new HashSet<EmployeeDTO>();
            Folios = new HashSet<FolioDTO>();
            Hoses = new HashSet<HoseDTO>();
            //Inventories = new HashSet<InventoryDTO>();
            InventoryIns = new HashSet<InventoryInDTO>();
            //Invoices = new HashSet<InvoiceDTO>();
            Islands = new HashSet<IslandDTO>();
            LoadPositions = new HashSet<LoadPositionDTO>();
            PointSales = new HashSet<PointSaleDTO>();
            Ports = new HashSet<PortDTO>();
            ProductStores = new HashSet<ProductStoreDTO>();
            SaleOrders = new HashSet<SaleOrderDTO>();
            Settings = new HashSet<SettingDTO>();
            StoreAddresses = new HashSet<StoreAddressDTO>();
            Tanks = new HashSet<TankDTO>();
        }

        public int StoreIdx { get; set; }
        public Guid StoreId { get; set; } = Guid.NewGuid();
        public Guid CompanyId { get; set; }
        public int StoreNumber { get; set; }
        public string Name { get; set; }
        public int? TimeDifference { get; set; }
        public string RfcLegalRepresentative { get; set; }
        public string ApiUrlInvoicing { get; set; }
        public Guid? ManualCustomerId { get; set; }
        public Guid? ManualVehicleId { get; set; }
        public string DistributionType { get; set; }
        public string ComplementType { get; set; }
        public int? TipPercentage { get; set; }
        public int? Send { get; set; }
        public DateTime? Date { get; set; }=DateTime.Now;
        public DateTime? Updated { get; set; }=DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; }=false;
        public bool? Deleted { get; set; } = false;

        public virtual CompanyDTO Company { get; set; }
        public virtual StoreSatDTO StoreSat { get; set; }
        public virtual ICollection<DispensaryDTO> Dispensaries { get; set; }
        public virtual ICollection<EmployeeDTO> Employees { get; set; }
        public virtual ICollection<FolioDTO> Folios { get; set; }
        public virtual ICollection<HoseDTO> Hoses { get; set; }
        //public virtual ICollection<InventoryDTO> Inventories { get; set; }
        public virtual ICollection<InventoryInDTO> InventoryIns { get; set; }
        //public virtual ICollection<InvoiceDTO> Invoices { get; set; }
        public virtual ICollection<IslandDTO> Islands { get; set; }
        public virtual ICollection<LoadPositionDTO> LoadPositions { get; set; }
        public virtual ICollection<PointSaleDTO> PointSales { get; set; }
        public virtual ICollection<PortDTO> Ports { get; set; }
        public virtual ICollection<ProductStoreDTO> ProductStores { get; set; }
        public virtual ICollection<SaleOrderDTO> SaleOrders { get; set; }
        public virtual ICollection<SettingDTO> Settings { get; set; }
        public virtual ICollection<StoreAddressDTO> StoreAddresses { get; set; }
        //public virtual ICollection<StoreSatDTO> StoreSats { get; set; }
        public virtual ICollection<TankDTO> Tanks { get; set; }

    }
}
