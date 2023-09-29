using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text.RegularExpressions;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvCompController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public string RPT { get; private set; }

        public InvCompController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }

        //public datet datefin2 = DateTime.Now.AddDays(-10);

        //[HttpGet("agrupadas")]
        //[AllowAnonymous]
        //public async Task<ActionResult> Get([FromQuery] Guid storeId, DateTime dateIni, DateTime dateFin)
        //{
        //    var listGroup = await context.InvoiceComparisons
        //        .Where(d => d.Date >= (dateIni) && d.Date <= dateFin && d.StoreId == storeId)
        //        .Join(
        //               context.Stores,
        //               t1 => t1.StoreId,
        //               t2 => t2.StoreId,
        //               (t1, t2) => new { InvoiceComparisons = t1, Stores = t2 }
        //                )
        //        .Select(x => new { invoiceserieid = x.InvoiceComparisons.InvoiceSerieId }).ToListAsync()


        //        .GroupBy(s => new { s.InvoiceComparisons.InvoiceSerieId, s.InvoiceComparisons.date })  //este es el valor de key
        //        .Select(g => new
        //        {
        //            invoiceSerieId = g.Key,
        //            conteo = g.Count(),
        //            Nombre = g.Select(x => x.Stores.Name).Distinct(),

        //            //Año = g.Select(x => x.InvoiceComparisons.Date.Value.Year)

        //            //invComp = g.ToList()
        //            //invoiceSerieId = g.Select(x => x.InvoiceSerieId)

        //        }).ToListAsync();
        //    return Ok(listGroup);

        //}

        [HttpGet("sql")]
        [AllowAnonymous]
        public async Task<ActionResult> Get([FromQuery] Guid storeId, DateTime dateIni, DateTime dateFin)
        {

            var listGroup = await context.InvoiceComparisons
                .FromSqlRaw($"SELECT * " +
                $"FROM invoice_comparison AS co " +
                $"WHERE co.invoice_serie_id = '2023' " +
                $"AND co.date BETWEEN '2023-01-01' and '2023-01-31' " +
                $"and store_id = '71930422-3B22-4ABA-8D49-5BC623067637' "
                                                    ).ToListAsync();

            return Ok(listGroup);

        }


        //    [HttpGet("active")]
        //[AllowAnonymous]
        //public async Task<ActionResult> Get2([FromQuery] Guid storeId, DateTime dateIni, DateTime dateFin)
        //{
        //    var list = await (from ico in context.InvoiceComparisons
        //                    where ico.Date >= (dateIni) && ico.Date <= dateFin && ico.StoreId == storeId

        //                    group ico by ico.InvoiceSerieId into g
        //                    //join s in context.Stores on g.Select(s => s.StoreId) equals s.StoreId

        //                      select new 
        //                    {
        //                        Nombre = s.Name,
        //                        invSerieId = g.Key,
        //                        conteo = g.Count(),



        //                    }).ToListAsync();
        //    return Ok(list);
        //}



        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult GroupRecords()
        //{
        //     var groups = context.InvoiceComparisons
        //            .GroupBy(e => e.InvoiceSerieId)
        //            .Select(group => new
        //            {
        //                GroupKey = group.Key,
        //                RecordCount = group.Count()
        //            })
        //            .ToList();

        //        return Ok(groups);

        //}

    }
} 
