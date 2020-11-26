using Phoenixnet.Extensions.Object;
using System.Threading.Tasks;

namespace MWM.API.Account.Service.Application.Common
{
    public class VerifyRequest : IVerifyRequest
    {
        public (bool isAuth, BasicResponse response) GetBySecretId(string sid)
        {
            if (sid.Length > 20)
            {
                return (false, new BasicResponse
                {
                    desc = "sid length limit",
                    code = 101
                });
            }

            return (true, new BasicResponse
            {
                desc = "success",
                code = 200
            });
        }

        public (bool isAuth, BasicResponse response) GetBySecretIdCid(string sid, string cid)
        {
            if (sid.Length > 20)
            {
                return (false, new BasicResponse
                {
                    desc = "sid length limit",
                    code = 101
                });
            }

            if (cid.Length > 20)
            {
                return (false, new BasicResponse
                {
                    desc = "cid length limit",
                    code = 102
                });
            }

            return (true, new BasicResponse
            {
                desc = "success",
                code = 200
            });
        }
    }
}