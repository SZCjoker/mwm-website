using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Phoenixnet.Extensions.Data;
using Phoenixnet.Extensions.Data.MySql;

namespace MWM.API.Advert.Service.Domain.VideoAdvert.video
{
    /// <summary>
    /// 官網後台-影片區域操作
    /// </summary>
    public class VideoAdvertRepository:IVideoAdvertRepository
    {
        private readonly IDbFactory _dbFactory;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="dbFactory"></param>
        public VideoAdvertRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_str"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<VideoAdvertEntity> record, long count)> GetRecords(string query_str, int index, int size)
        {
            var tsql = $@"SELECT 
                                 SQL_CALC_FOUND_ROWS
	                            `id`,
	                            `title`,
	                            `hyper_link`,
	                            `color`,
	                            `position`,
	                            `advert_text`,
	                            `loop_type`,
	                            `default_time`,
	                            `video_link`,
	                            `skip_time`,
	                            `start_time`,
	                            `end_time`,
	                            `status`,
	                            `video_advert_type`,
                                `ctime`,
                                `desc`,
                                `utime`
                                FROM `advert_video`
                                {query_str}
                                LIMIT  @index, @size; 

                                SELECT Found_rows() AS cnt;";
            using (var cn = await _dbFactory.OpenConnectionAsync())
            {
                var multiple = await cn.QueryMultipleAsync(tsql, new {  index, size});

                var resultRecord = await multiple.ReadAsync<VideoAdvertEntity>();

                var resultCnt = await multiple.ReadSingleAsync<long>();

                return (record: resultRecord, count: resultCnt);
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
  
        /// <returns></returns>
        public async Task<int> Del_Record(int id)
        {
            
         
            var  tsql =  @"DELETE FROM `advert_video` WHERE  `id`=@id ;";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, new{id});
            }
          
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Update_Record(VideoAdvertEntity entity)
        {
            var  tsql =  @"UPDATE `advert_video`
                            SET
                            `title` = @title,
                            `hyper_link` = @hyper_link,
                            `color` = @color,
                            `position` = @position,
                            `advert_text` = @advert_text,
                            `loop_type` = @loop_type,
                            `default_time` = @default_time,
                            `video_link` = @video_link,
                            `skip_time` = @skip_time,
                            `start_time` = @start_time,
                            `end_time` = @end_time,
                            `status` = @status,
                            `desc` =@desc,
                            `utime`=@utime
                            WHERE `id` = @id
                            and  `video_advert_type` =@video_advert_type;";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, entity);
            }
        }

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Add_Record(VideoAdvertEntity entity)
        {
            var tsql = @"INSERT INTO `advert_video`(`title`,`hyper_link`,`color`,`position`,`advert_text`,`loop_type`,`default_time`,`video_link`,`skip_time`,`start_time`,`end_time`,`status`,`desc`,`video_advert_type`,`ctime`,`utime`)
                         VALUES (@title,@hyper_link,@color,@position,@advert_text,@loop_type,@default_time,@video_link,@skip_time,@start_time,@end_time,@status,@desc,@video_advert_type,@ctime,@utime);";
            using (var cn = await _dbFactory.OpenConnectionAsync(AccessMode.ReadWrite))
            {
                return await cn.ExecuteAsync(tsql, entity);
            }
        }
    }
}