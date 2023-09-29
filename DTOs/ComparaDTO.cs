namespace APIControlNet.Utilidades
{
    public class ComparaDTO : IEquatable<ComparaDTO>
    {
        public Guid AlmacenDestino { get; set; }
        public Guid Productoid { get; set; }
        public decimal Quantity { get; set; }


        public bool Equals(ComparaDTO other)
        {
            if (other is null)
                return false;

            return this.AlmacenDestino == other.AlmacenDestino && this.Productoid == other.Productoid && this.Quantity == other.Quantity;
        }

        public override bool Equals(object obj) => Equals(obj as ComparaDTO);
        public override int GetHashCode() => (AlmacenDestino, Productoid, Quantity).GetHashCode();
    }
}
