using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            Vehicles = new HashSet<Vehicle>();
        }

        public int CustomerIdx { get; set; }
        public Guid CustomerId { get; set; }
        public int? CustomerNumber { get; set; }
        public int? CustomerTypeId { get; set; }
        public string SatFormaPagoId { get; set; }
        public string SatRegimenFiscalId { get; set; }
        public string SatUsoCfdiId { get; set; }
        public int? CustomerAddressIdi { get; set; }
        public string Name { get; set; }
        public string AbbreviatedName { get; set; }
        public string Lastname { get; set; }
        public string MotherLastname { get; set; }
        public string Curp { get; set; }
        public string Rfc { get; set; }
        public string Email { get; set; }
        public string Cellular { get; set; }
        public string Phone { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
        public string ContactPerson { get; set; }
        public string SatConsignmentSale { get; set; }
        public string CustomerPermission { get; set; }

        public virtual CustomerType CustomerType { get; set; }
        public virtual SatFormaPago SatFormaPago { get; set; }
        public virtual SatRegimenFiscal SatRegimenFiscal { get; set; }
        public virtual SatUsoCfdi SatUsoCfdi { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
