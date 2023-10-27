using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Store
    {
        public Store()
        {
            Dispensaries = new HashSet<Dispensary>();
            Employees = new HashSet<Employee>();
            Folios = new HashSet<Folio>();
            Hoses = new HashSet<Hose>();
            Inventories = new HashSet<Inventory>();
            InventoryIns = new HashSet<InventoryIn>();
            Invoices = new HashSet<Invoice>();
            Islands = new HashSet<Island>();
            LoadPositions = new HashSet<LoadPosition>();
            PointSales = new HashSet<PointSale>();
            Ports = new HashSet<Port>();
            ProductStores = new HashSet<ProductStore>();
            SaleOrders = new HashSet<SaleOrder>();
            Settings = new HashSet<Setting>();
            StoreAddresses = new HashSet<StoreAddress>();
            Tanks = new HashSet<Tank>();
        }

        public int StoreIdx { get; set; }
        public Guid StoreId { get; set; }
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
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Carrier { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string Rfc { get; set; }

        public virtual Company Company { get; set; }
        public virtual StoreSat StoreSat { get; set; }
        public virtual ICollection<Dispensary> Dispensaries { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Folio> Folios { get; set; }
        public virtual ICollection<Hose> Hoses { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<InventoryIn> InventoryIns { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Island> Islands { get; set; }
        public virtual ICollection<LoadPosition> LoadPositions { get; set; }
        public virtual ICollection<PointSale> PointSales { get; set; }
        public virtual ICollection<Port> Ports { get; set; }
        public virtual ICollection<ProductStore> ProductStores { get; set; }
        public virtual ICollection<SaleOrder> SaleOrders { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<StoreAddress> StoreAddresses { get; set; }
        public virtual ICollection<Tank> Tanks { get; set; }
    }
}
