namespace APIControlNet.DTOs
{
    public class PaginacionDTO 
    {
        public int Pagina { get; set; } = 1;
        public int CantidadAMostrar { get; set; } = 7;

        //private readonly int cantidadMaximaRegistrosPorPagina = 50;

        //public int CantidadRegistrosPorPagina
        //{
        //    get
        //    {
        //        return CantidadAMostrar;
        //    }
        //    set
        //    {
        //        CantidadAMostrar = (value > cantidadMaximaRegistrosPorPagina) ? cantidadMaximaRegistrosPorPagina : value;
        //    }
        //}

    }
}
