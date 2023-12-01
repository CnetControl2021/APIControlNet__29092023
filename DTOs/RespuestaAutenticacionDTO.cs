namespace APIControlNet.DTOs
{
    public class RespuestaAutenticacionDTO
    {
        public string Token { get ; set; }
        public DateTime Expiracion { get; set; }
        public bool EmailConfirmed { get; set; }
        public Guid dbComany { get; set; }
    }
}
