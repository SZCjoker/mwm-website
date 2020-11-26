using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MWM.API.Site.Service.Application.AdvertReport;

namespace MWM.API.Site.Service.Controllers
{
    /// <summary>
    /// 廣告點擊報表
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertReportController: Controller
    {
        private readonly IAdvertReportService _advertReportService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="advertReportService"></param>
        public AdvertReportController(IAdvertReportService advertReportService)
        {
            _advertReportService = advertReportService;
        }

        /// <summary>
        /// 廣告點擊-數據列表
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet("getlist")]
        public async Task<IActionResult> GetList(int start_date = 0, int end_date = 0, string login_name = "", int index = 1, int size = 30)
        {
            if (start_date == 0 || end_date == 0)
            {
                start_date = Convert.ToInt32(DateTime.Now.AddDays(-7).ToString("yyyyMMdd"));
                end_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            }

            var result = await _advertReportService.Get_List_Record(login_name, start_date, end_date, index, size);
            return Ok(result);
        }
        
        /// <summary>
        ///  廣告點擊-數據詳細頁
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet("getDetail")]
        public async Task<IActionResult> GetDetail(int account_id,int start_date = 0, int end_date = 0,  int index = 1, int size = 30)
        {
            if (start_date == 0 || end_date == 0)
            {
                start_date = Convert.ToInt32(DateTime.Now.AddDays(-7).ToString("yyyyMMdd"));
                end_date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            }

            var result = await _advertReportService.Get_Detail_Record(account_id, start_date, end_date, index, size);
            return Ok(result);
        }
    }
}