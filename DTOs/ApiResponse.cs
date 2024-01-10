namespace APIControlNet.DTOs
{
    public class ApiResponse
    {
        public int NTotal { get; set; }
        public IEnumerable<CustomerDTO> NClientes { get; set; }

    }
}
