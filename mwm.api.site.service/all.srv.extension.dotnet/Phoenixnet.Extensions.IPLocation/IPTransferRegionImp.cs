using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenixnet.Extensions.IPLocation
{
    public class IPTransferRegionImp:IIPTransferRegion
    {
        private readonly City _city;
        public IPTransferRegionImp(City city)
        {
            _city = city;
        }

        public async Task<IPTransferRegionResponse> GetRegion(string address)
        {
            var region = _city.findInfo(address, "CN");

            if (region!=null)
            {
                return await Task.FromResult(new IPTransferRegionResponse() {CountryName = region?.getCountryName(), RegionName = region?.getRegionName(), CityName = region?.getCityName()});
            }
            else
            {
                return null;
            }


        }

        public async Task<IEnumerable<IPTransferRegionResponse>> GetRegions(string[] address)
        {
            return await Task.FromResult( address.Select(ip => _city.findInfo(ip, "CN")).Select(region => new IPTransferRegionResponse() {CountryName = region?.getCountryName(), RegionName = region?.getRegionName(), CityName = region?.getCityName()}).ToList());
        }
    }
}