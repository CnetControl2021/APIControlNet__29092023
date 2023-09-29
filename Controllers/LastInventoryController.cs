using APIControlNet.DTOs;
using APIControlNet.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIControlNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LastInventoryController : CustomBaseController
    {
        private readonly CnetCoreContext context;

        private readonly IMapper mapper;

        public LastInventoryController(CnetCoreContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IEnumerable<LastInventoryDTO>> Get5(Guid storeId)
        {
            List<LastInventoryDTO> list = new List<LastInventoryDTO>();
            if (storeId != Guid.Empty)
            {
                list = await (from lInv in context.LastInventories        
                              where lInv.StoreId == storeId
                              join pd in context.Products on lInv.ProductId equals pd.ProductId
                              join tk in context.Tanks on lInv.TankIdi equals tk.TankIdi
                              where tk.StoreId == storeId
                              

                              select new LastInventoryDTO
                              {
                                  LastInventoryIdx = lInv.LastInventoryIdx,
                                  TankName = tk.Name,
                                  ProductName = pd.Name,
                                  Volume = lInv.Volume,
                                  VolumeTc = lInv.VolumeTc,
                                  VolumeWater = lInv.VolumeWater,
                                  Temperature = lInv.Temperature,
                                  Updated = lInv.Updated,
                                  StatusResponse = lInv.StatusResponse
                              }).AsNoTracking().ToListAsync();
            }
            else
            {
                list = await (from lInv in context.LastInventories
                              join pd in context.Products on lInv.ProductId equals pd.ProductId
                              join tk in context.Tanks on lInv.TankIdi equals tk.TankIdi

                              select new LastInventoryDTO
                              {
                                  LastInventoryIdx = lInv.LastInventoryIdx,
                                  TankName = tk.Name,
                                  ProductName = pd.Name,
                                  Volume = lInv.Volume,
                                  VolumeTc = lInv.VolumeTc,
                                  VolumeWater = lInv.VolumeWater,
                                  Temperature = lInv.Temperature,
                                  Updated = lInv.Updated,
                                  StatusResponse = lInv.StatusResponse
                              }).AsNoTracking().ToListAsync();
            }
            return list;
        }


        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<List<LastInventoryDTO>> Get(Guid storeId)
        //{
        //    var queryable = context.LastInventories.AsQueryable();
        //    if (storeId != Guid.Empty)
        //    {
        //        queryable = queryable.Where(x => x.StoreId == storeId);
        //    }
        //    var lastInv = await queryable
        //        //.Include(x => x.StatusDispenserIdiNavigation)
        //        .AsNoTracking().ToListAsync();
        //    return mapper.Map<List<LastInventoryDTO>>(lastInv);
        //}
    }
}
