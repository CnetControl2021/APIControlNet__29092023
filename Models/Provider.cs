using System;
using System.Collections.Generic;

namespace APIControlNet.Models
{
    public partial class Provider
    {
        public int ProviderIdx { get; set; }
        public Guid ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Rfc { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string OutdoorNumber { get; set; }
        public string InteriorNumber { get; set; }
        public string Suburb { get; set; }
        public string Municipality { get; set; }
        public string Estate { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Active { get; set; }
        public bool? Locked { get; set; }
        public bool? Deleted { get; set; }
    }
}
