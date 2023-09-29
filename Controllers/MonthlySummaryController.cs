using APIControlNet.DTOs;
using APIControlNet.Models;
using APIControlNet.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class MonthlySummaryController : CustomBaseController
    {
        private readonly CnetCoreContext context;
        private readonly IMapper mapper;
        private readonly ServicioBinnacle servicioBinnacle;

        public MonthlySummaryController(CnetCoreContext context, IMapper mapper, ServicioBinnacle servicioBinnacle)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioBinnacle = servicioBinnacle;
        }


        [HttpGet]
        [Route("sinPag")]
        public async Task<IEnumerable<MonthlySummaryDTO>> Get5(Guid storeId, DateTime dateIni, DateTime dateFin)
        {
            //var queryable = context.DailySummaries.Where(x => x.Date <= dateFin && x.Date >= dateIni && x.Active ==true)
            //    .OrderByDescending(x => x.DailySummaryIdx).AsQueryable().AsNoTracking();
            var queryable = context.MonthlySummaries.Where(x => x.Date <= dateFin && x.Date >= dateIni)
                .OrderByDescending(x => x.Date).AsQueryable().AsNoTracking();
            if (storeId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.StoreId == storeId);
            }
            var montlySum = await queryable
                .Include(x => x.Product)
                .AsNoTracking().ToListAsync();
            return mapper.Map<List<MonthlySummaryDTO>>(montlySum);
        }



    }
}
