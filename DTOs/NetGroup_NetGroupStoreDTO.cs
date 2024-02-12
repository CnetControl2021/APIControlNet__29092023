using APIControlNet.Models;

namespace APIControlNet.DTOs
{
    public class NetGroup_NetGroupStoreDTO
    {
        public NetgroupDTO NNetgroupDTO { get; set; }
        public NetgroupStoreDTO NNetgroupStoreDTO { get; set; }
        public NetGroup_NetGroupStoreDTO()
        {
            NNetgroupDTO = new NetgroupDTO();
            NNetgroupStoreDTO = new NetgroupStoreDTO();
        }
    }
    
}
