using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using APIControlNet.Utilidades;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleOrderController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public SaleOrderController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        public DateTime dateNowMenos1 = DateTime.Today.AddDays(-1);
        public DateTime dateToday = DateTime.Today;


        [HttpGet]
        [Route("fordate")]
        //[AllowAnonymous]
        public async Task<ActionResult<SaleOrderDTO>> Get5(
            Guid storeId, DateTime dateIni, DateTime dateFin, [FromQuery] string nombre)
        {
            List<SaleOrderDTO> listSO = new List<SaleOrderDTO>();

            if (!string.IsNullOrEmpty(nombre))
            {
                listSO = await (from so in context.SaleOrders
                                where so.Active == true
                                join ss in context.SaleSuborders on so.SaleOrderId equals ss.SaleOrderId
                                join products in context.Products on ss.ProductId equals products.ProductId
                                where so.Name.ToLower().Equals(nombre)
                                && so.StoreId == storeId
                                && products.IsFuel == true
                                orderby so.SaleOrderIdx descending

                                select new SaleOrderDTO
                                {
                                    SaleOrderIdx = so.SaleOrderIdx,
                                    SaleOrderNumber = so.SaleOrderNumber,
                                    HoseIdi = so.HoseIdi,
                                    Quantity = ss.Quantity,  // litros
                                    QuantityTC = ss.QuantityTc,  // litros con temperatura compenmsada a 20 grados.
                                    Price = ss.Price, // precio unitariio
                                    Amount = so.Amount,
                                    Date = ss.Date
                                }).AsNoTracking().ToListAsync();
            }
            else
            {
                listSO = await (from so in context.SaleOrders
                                where so.Active == true
                                join ss in context.SaleSuborders on so.SaleOrderId equals ss.SaleOrderId
                                join products in context.Products on ss.ProductId equals products.ProductId
                                where ss.Date >= (dateIni) && ss.Date <= dateFin
                                && so.StoreId == storeId
                                && products.IsFuel == true
                                orderby so.SaleOrderIdx descending

                                select new SaleOrderDTO
                                {
                                    SaleOrderIdx = so.SaleOrderIdx,
                                    SaleOrderNumber = so.SaleOrderNumber,
                                    HoseIdi = so.HoseIdi,
                                    ProductName = products.Name,
                                    Quantity = ss.Quantity,
                                    QuantityTC = ss.QuantityTc,
                                    Price = ss.Price,
                                    Amount = so.Amount,
                                    Date = ss.Date
                                }).AsNoTracking().ToListAsync();
            }
            return Ok(listSO);
        }

        //[HttpGet]
        //[Route("sinPag2")]
        //[AllowAnonymous]
        //public async Task<IEnumerable<SaleOrderDTO>> Get2([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre,
        //    Guid storeId, DateTime dateIni, DateTime dateFin)
        //{
        //    var queryable =  (from so in context.SaleOrders
        //                           join ss in context.SaleSuborders on so.SaleOrderId equals ss.SaleOrderId
        //                           join products in context.Products on ss.ProductId equals products.ProductId
        //                           where ss.Date >= (dateIni) && ss.Date <= dateFin
        //                           && so.StoreId == storeId
        //                           && products.IsFuel == true
        //                           orderby so.SaleOrderIdx descending

        //                           select new SaleOrderDTO
        //                           {
        //                               SaleOrderNumber = so.SaleOrderNumber,
        //                               HoseIdi = so.HoseIdi,
        //                               Quantity2 = ss.Quantity,
        //                               QuantityTC = ss.QuantityTc,
        //                               Price2 = ss.Price,
        //                               Amount = so.Amount,
        //                               Date = ss.Date
        //                           }).AsQueryable();

        //    await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
        //    var saleOrder = await queryable.Paginar(paginacionDTO).AsNoTracking().ToListAsync();
        //    return mapper.Map<List<SaleOrderDTO>>(saleOrder);
        //}


        //[HttpGet("sinPag")]
        ////[AllowAnonymous]
        //public async Task<IEnumerable<SaleOrderDTO>> Get5(DateTime dateIni, [FromQuery] string nombre)
        //{
        //    //var saleOrder = await context.SaleOrders.Where(x => x.Date >= dateIni)
        //    //    //.Include(x => x.SaleSuborders)
        //    //    .ToListAsync();

        //    //return mapper.Map<List<SaleOrderDTO>>(saleOrder);


        //    //var saleOrder = await context.Database
        //    //    ($"SELECT * FROM [SaleOrder] sale_order Where date >= {dateIni}")
        //    //    .IgnoreQueryFilters().ToListAsync();

        //    //return mapper.Map<List<SaleOrderDTO>>(saleOrder);

        //    var queryable = context.SaleOrders
        //       .OrderByDescending(x => x.SaleOrderIdx).AsQueryable();

        //    var saleOrder = await queryable
        //                        .Select(x => new
        //                        {
        //                            x.SaleOrderNumber,
        //                            x.HoseIdi,
        //                            x.Date,
        //                            quantity = x.SaleSuborders.Select(x => x.Quantity),
        //                            x.Amount
        //                            //fechaFormateada = x.Date.ToString()
        //                        }).AsNoTracking()
        //                        .ToListAsync();
        //    return mapper.Map<List<SaleOrderDTO>>(saleOrder);

        //}

        ////[HttpGet("sinPag")]
        ////[AllowAnonymous]
        //public async Task<IEnumerable<SaleOrderDTO>> Get(Guid storeId, DateTime dateIni, DateTime dateFin, [FromQuery] string nombre)
        //{
        //    ////var dateIni2 = dateIni.ToString("yyyy-MM-dd");
        //    var queryable = context.SaleOrders
        //        .OrderByDescending(x => x.SaleOrderIdx).AsQueryable();

        //    if (!string.IsNullOrEmpty(nombre))
        //    {
        //        queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
        //    }
        //    if (storeId != Guid.Empty)
        //    {
        //        queryable = queryable.Where(x => x.StoreId == storeId);
        //    }
        //    //if (dateIni > DateTime.MinValue & dateFin > DateTime.MinValue)
        //    //if (dateIni >  DateTime.MinValue.ToLongDateString)
        //    //{
        //    queryable = queryable.Where(x => x.Date >= dateIni);
        //    //queryable = queryable.Where(x => x.Date >= DateTime.Parse(dateIni));
        //    queryable = queryable.Where(x => x.Date <= dateFin);
        //    //queryable = queryable.Where(x => x.Date <= DateTime.Parse(dateFin));
        //    //}

        //    ////Console.WriteLine("CurrentCulture is {0}.", CultureInfo.CurrentCulture.Name);

        //    //var saleOrder = await queryable.Select(x => new SaleOrderDTO { SaleOrderNumber = x.SaleOrderNumber, HoseIdi=x.HoseIdi,
        //    //                Date=x.Date, Amount=x.Amount })
        //    //    //.Include(x => x.SaleSuborders)
        //    //    .AsNoTracking()
        //    //    .ToListAsync();
        //    //return saleOrder;

        //    //var saleOrder = await queryable.ProjectTo<SaleOrderDTO>(mapper.ConfigurationProvider)
        //    var saleOrder = await queryable
        //       .Include(x => x.SaleSuborders)
        //       //.Include()
        //       //.AsNoTracking()
        //       .ToListAsync();
        //    return mapper.Map<List<SaleOrderDTO>>(saleOrder);



        //    //var saleOrder = await context.SaleOrders.Where(x => x.Date >= DateTime.Today)
        //    //.OrderByDescending(x => x.SaleOrderIdx)
        //    ////.AsNoTracking().ToListAsync();
        //    //return (IEnumerable<SaleOrderDTO>)await context.SaleOrders.Where(x => x.Date >= dateNowMenos1).AsNoTracking().ToListAsync();
        //    //// return mapper.Map<List<SaleOrderDTO>>(saleOrder);
        //}


        //[HttpGet("{storeId?}")]
        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<SaleOrderDTO>> Get(Guid storeId, DateTime dateIni, DateTime dateFin, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre)
        {
            var queryable = context.SaleOrders.OrderByDescending(x => x.SaleOrderIdx).AsQueryable();
            if (!string.IsNullOrEmpty(nombre))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre));
                //queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) || x.SaleOrderNumber.ToString().ToLower().Contains(nombre)
                //|| (x.Date >= dateIni & x.Date <= dateFin));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            if (dateIni > DateTime.MinValue & dateFin > DateTime.MinValue)
            {
                queryable = queryable.Where(x => x.Date >= dateIni && x.Date <= dateFin);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var saleOrder = await queryable.Paginar(paginacionDTO)
                //.Include(x => x.SaleSuborders)
                //.Include(x => x.Store)
                //.Include(x => x.ProductStore).ThenInclude(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<SaleOrderDTO>>(saleOrder);
        }


        [HttpGet("Active")]
        //[AllowAnonymous]
        public async Task<IEnumerable<SaleOrderDTO>> Get2(Guid storeId, [FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string nombre,
            DateTime dateIni, DateTime dateFin)
        {
            var queryable = context.SaleOrders.Where(x => x.Active == true).OrderByDescending(x => x.SaleOrderIdx).AsQueryable();
            if (!string.IsNullOrEmpty(nombre) || dateIni > DateTime.MinValue)
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(nombre) || x.SaleOrderNumber.ToString().ToLower().Contains(nombre)
                || (x.Date >= dateIni & x.Date <= dateFin));
            }
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacionDTO.CantidadAMostrar);
            var saleOrder = await queryable.Paginar(paginacionDTO)
                //.Include(x => x.SaleSuborders)
                //.Include(x => x.Store)
                //.Include(x => x.ProductStore).ThenInclude(x => x.Product)
                .AsNoTracking()
                .ToListAsync();
            return mapper.Map<List<SaleOrderDTO>>(saleOrder);
        }


        [HttpGet("{id:int}", Name = "obtSaleOrder")]
        public async Task<ActionResult<SaleOrderDTO>> Get(int id)
        {
            var saleOrder = await context.SaleOrders.FirstOrDefaultAsync(x => x.SaleOrderIdx == id);

            if (saleOrder == null)
            {
                return NotFound();
            }

            return mapper.Map<SaleOrderDTO>(saleOrder);
        }

        [HttpGet]
        [Route("PruebaskipAndTake")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedData([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var pagedData = await context.SaleOrders
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalRecords = await context.SaleOrders.CountAsync();

            return Ok(new { Data = pagedData, TotalRecords = totalRecords });
        }
    }
}
