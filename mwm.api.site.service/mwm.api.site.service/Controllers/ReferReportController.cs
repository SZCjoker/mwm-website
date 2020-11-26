using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MWM.API.Site.Service.Application.ReferReport;

namespace MWM.API.Site.Service.Controllers
{
    /// <summary>
    ///     流量數據
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReferReportController : Controller
    {
        private readonly IReferReportService _referReportService;

        /// <summary>
        /// </summary>
        /// <param name="referReportService"></param>
        public ReferReportController(IReferReportService referReportService)
        {
            _referReportService = referReportService;
        }

        /// <summary>
        ///     流量數據(顯示所有流量主)
        /// </summary>
        /// <param name="start_date">起始日期：20200210</param>
        /// <param name="end_date">結束日期：20200212</param>
        /// <param name="login_name">商戶名稱Login_name</param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("referAll")]
        public async Task<IActionResult> GetReferAll(int start_date = 0, int end_date = 0, string login_name = "", int index = 1, int size = 30)
        {
            if (start_date == 0 || end_date == 0)
            {
                start_date = Convert.ToInt32(DateTime.Now.AddDays(-7).ToString("yyyyMMdd"));
                end_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            }
            var result = await _referReportService.Get_ReferReportAll_Record(login_name, start_date, end_date, index, size);
            return Ok(result);
        }

        /// <summary>
        /// 流量數據(顯示單一流量主)
        /// </summary>
        /// <param name="start_date">起始日期：20200210</param>
        /// <param name="end_date">結束日期：20200212</param>
        /// <param name="account_id">商戶id</param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("referSiteMaster/{account_id}")]
        public async Task<IActionResult> GetReferSiteMaster(int account_id, int start_date = 0, int end_date = 0, int index = 1, int size = 30)
        {
            if (account_id == 0) return NoContent();

            if (start_date == 0 || end_date == 0)
            {
                start_date = Convert.ToInt32(DateTime.Now.AddDays(-7).ToString("yyyyMMdd"));
                end_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            }


            var result = await _referReportService.Get_ReferReportSiteMaster_Record(account_id, start_date, end_date, index, size);
            return Ok(result);
        }
        /// <summary>
        /// 流量數據(顯示單一流量主底下所有網域數據)
        /// </summary>
        /// <param name="account_id">商戶id</param>
        /// <param name="date">日期：20200210</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("referSiteMaster/detail/{account_id}/{date}")]
        public async Task<IActionResult> GetReferSiteMasterDetail(int account_id, int date )
        {
            var result = await _referReportService.Get_ReferReportSiteMasterDetail_Record(account_id, date);
            return Ok(result);
        }
        
        
        
        
    }
}