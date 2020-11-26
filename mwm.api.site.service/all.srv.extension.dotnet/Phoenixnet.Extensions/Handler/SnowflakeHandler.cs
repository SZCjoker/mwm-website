using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Snowflake.Core;

namespace Phoenixnet.Extensions.Handler
{
    public class SnowflakeHandler : IGenerateId
    {
        private readonly IdWorker _idWorker;

        public SnowflakeHandler(string machineName)
        {
            using (var md5Hasher = MD5.Create())
            {
                var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(machineName));
                var intval = Math.Abs(BitConverter.ToInt16(hashed, 0));
                var rand = new Random(intval);
                var listLinq = new List<int>(Enumerable.Range(1, 31));
                listLinq = listLinq.OrderBy(num => rand.Next()).ToList();
                var randomise = listLinq.Skip(0).Take(2);
                _idWorker = new IdWorker(randomise.FirstOrDefault(), randomise.Last(), 1);
            }
        }

        public long GetId()
        {
            var id = _idWorker.NextId();

            return id;
        }
    }

    public interface IGenerateId
    {
        long GetId();
    }
}
