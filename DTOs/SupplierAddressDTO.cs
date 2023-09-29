﻿using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class SupplierAddressDTO
    {
        public int SupplierAddressIdx { get; set; }
        public Guid SupplierId { get; set; } //= Guid.NewGuid();
        public int? SupplierAddressIdi { get; set; }
        public string SatPaisId { get; set; } = "MX";
        public string SatEstadoId { get; set; }
        public string SatMunicipioId { get; set; }
        public string SatLocalidadId { get; set; }
        public string Street { get; set; }
        public string OutsideNumber { get; set; }
        public string InsideNumber { get; set; }
        public string Colony { get; set; }
        public string ZipCode { get; set; }
        public DateTime? Date { get; set; }=DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool? Active { get; set; } = true;
        public bool? Locked { get; set; } = false;
        public bool? Deleted { get; set; }=false;

        public virtual SupplierDTO Supplier { get; set; }
    }
}
