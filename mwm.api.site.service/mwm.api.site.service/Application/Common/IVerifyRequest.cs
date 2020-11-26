using Phoenixnet.Extensions.Object;
using System.Threading.Tasks;

namespace MWM.API.Site.Service.Application.Common
{
    public interface IVerifyRequest
    {
        (bool isAuth, BasicResponse response) GetBySecretId(string sid); 
    }
}