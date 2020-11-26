using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.IPLocation
{
    public interface IIPTransferRegion
    {
        Task<IPTransferRegionResponse> GetRegion(string address);
        
        Task< IEnumerable<IPTransferRegionResponse>> GetRegions(string [] address);
    }
}